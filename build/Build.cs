using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.IO;
using Nuke.Common.Tools.Docker;
using Nuke.Common.Utilities.Collections;
using Renci.SshNet;
using Serilog;
using static Nuke.Common.EnvironmentInfo;

class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Dummy);

    [Parameter] readonly string DeployHost;
    [Parameter] readonly int DeploySshPort;
    [Parameter] readonly string RunnerSshUser;
    [Parameter] readonly string RunnerSshPrivkey;
    [Parameter] readonly string DeployPath;
    [Parameter] readonly string GhEnvironmentName;
    [Parameter] readonly string DbRootPasswd;


    string RemoteDockerComposeFilePath => $"{DeployPath}/infrastructure/docker-compose.yml";
    string RemoteDbInitScriptsDir => $"{DeployPath}/infrastructure/db/init.d";

    Target Dummy => _ => _.Executes(() =>
    {
    });

    Target EnvironmentInit => _ => _.Executes(async () =>
    {
        Log.Information("Setting up environment {EnvironmentName}...", GhEnvironmentName);

        using var sshClient = CreateSshClient();
        using var scpClient = CreateScpClient();

        await sshClient.ConnectAsync(default);
        await scpClient.ConnectAsync(default);

        string[] dirsToCreate =
        [
            $"{DeployPath}/infrastructure",
            $"{DeployPath}/infrastructure/db",
            RemoteDbInitScriptsDir,
            $"{DeployPath}/infrastructure/db/data",
        ];
        Log.Information("Creating infrastructure directories");
        dirsToCreate.ForEach(directoryPath =>
        {
            Log.Debug("Creating directory: {DirectoryPath}", directoryPath);
            sshClient.RunCommand($"mkdir -p '{directoryPath}'");
        });

        var localDockerComposeFilePath = RootDirectory / "docker/docker-compose.development.yml";
        Log.Information(
            "Copying docker-compose file from local {Src} to remote {Target}",
            localDockerComposeFilePath,
            RemoteDockerComposeFilePath
        );
        scpClient.Upload(localDockerComposeFilePath.ToFileInfo(), RemoteDockerComposeFilePath);

        Log.Information("Copying database init scripts");
        var localDatabaseInitScriptsDirectory = RootDirectory / "scripts/database";
        localDatabaseInitScriptsDirectory.GetFiles("*.sql")
            .ForEach(localFilePath =>
            {
                var remoteFilePath = $"{RemoteDbInitScriptsDir}/{localFilePath.Name}";
                Log.Debug("Copying local file {Src} to remote {Target}", localFilePath, remoteFilePath);
                scpClient.Upload(localFilePath.ToFileInfo(), remoteFilePath);
            });

        Log.Information("Running docker compose up");
        sshClient.RunCommand(
            $"DB_ROOT_PASSWD={DbRootPasswd} DB_INIT_SCRIPTS_DIR={RemoteDbInitScriptsDir} docker compose -f {RemoteDockerComposeFilePath} up -d"
        );
    });

    Target EnvironmentShutdown => _ => _.Executes(async () =>
    {
        Log.Information("Shutting down environment {EnvironmentName}...", GhEnvironmentName);

        using var sshClient = CreateSshClient();

        await sshClient.ConnectAsync(default);

        Log.Information("Running docker compose stop");
        sshClient.RunCommand(
            $"docker compose -f {RemoteDockerComposeFilePath} stop -t 15"
        );
    });

    private SshClient CreateSshClient()
    {
        Log.Debug(
            "Creating SSH client with URI {User}@{Host}:{Port}",
            RunnerSshUser,
            DeployHost,
            DeploySshPort
        );

        var sshClient = new SshClient(
            new PrivateKeyConnectionInfo(
                DeployHost,
                DeploySshPort,
                RunnerSshUser,
                new PrivateKeyFile(CreatePrivateKeyStream())
            )
        );

        return sshClient;
    }

    private ScpClient CreateScpClient()
    {
        Log.Debug(
            "Creating SCP client with URI {User}@{Host}:{Port}",
            RunnerSshUser,
            DeployHost,
            DeploySshPort
        );

        var scpClient = new ScpClient(
            new PrivateKeyConnectionInfo(
                DeployHost,
                DeploySshPort,
                RunnerSshUser,
                new PrivateKeyFile(CreatePrivateKeyStream())
            )
        );

        return scpClient;
    }

    private MemoryStream CreatePrivateKeyStream()
    {
        var keyStream = new MemoryStream(Encoding.ASCII.GetBytes(RunnerSshPrivkey));
        return keyStream;
    }

    private string ExpandEnvironmentVariables(string str)
    {
        Log.Information("Expanding environment variables in a string: {Input}", str);
        var variableRegex = new Regex(@"\$([a-z_][a-z0-9_]*)", RegexOptions.IgnoreCase);
        var variableMatches = variableRegex.Matches(str);
        foreach (Match match in variableMatches)
        {
            Log.Information("Found variable: {Name}", match.Groups[1].Value);
            var value = GetVariable<string>(match.Groups[1].Value);
            Log.Information("Variable value: {Value}", value);
            str = str.Replace(match.Value, value);
        }

        Log.Information("Expanded string: {Result}", str);
        return str;
    }
}
