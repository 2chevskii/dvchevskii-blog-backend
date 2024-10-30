using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Dvchevskii.Blog.Contracts.Authentication;
using Dvchevskii.Blog.Contracts.Authentication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Dvchevskii.Blog.WebApi.Debug;

[ApiController, Route("[controller]")]
public class DebugController(
    ILogger<DebugController> logger,
    IAuthenticationContext authContext,
    IAuthenticationContextProvider authContextProvider,
    IAuthenticationContextFactory authenticationContextFactory
) : ControllerBase
{
    [HttpGet("authentication-context/async")]
    public async Task<IActionResult> DebugAuthenticationContextAsync()
    {
        throw new NotImplementedException();
    }

    [HttpGet("authentication-context/sync")]
    public IActionResult DebugAuthenticationContextSync()
    {
        logger.LogInformation("DebugAuthenticationContextSync START");

        logger.LogInformation("Auth context: {AuthContext}", authContext.ToDebugString());
        var scope = authContextProvider.CreateScope(authenticationContextFactory.Create(1, "test1", false));
        logger.LogInformation("Auth context: {AuthContext}", authContext.ToDebugString());

        var scope2 = authContextProvider.CreateScope(authenticationContextFactory.Create(2, "test2", true));
        logger.LogInformation("Auth context: {AuthContext}", authContext.ToDebugString());
        scope2.Dispose();

        logger.LogInformation("Auth context: {AuthContext}", authContext.ToDebugString());

        scope.Dispose();
        logger.LogInformation("Auth context: {AuthContext}", authContext.ToDebugString());
        logger.LogInformation("DebugAuthenticationContextSync END");
        return Ok();
    }

    [HttpGet("jwt")]
    public IActionResult DebugToken()
    {
        logger.LogInformation("Debug create security JWT");
        var token = new JwtSecurityTokenHandler().CreateJwtSecurityToken(
            new SecurityTokenDescriptor
            {
                Claims = new Dictionary<string, object>
                {
                    { "username", "debug_user" },
                    { "userid", 42 },
                },
                Expires = DateTime.UtcNow.AddDays(30),
                Audience = "http://localhost:3000",
                Issuer = "http://localhost:3000",
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow,
                TokenType = "JWT",
                SigningCredentials = new SigningCredentials(
                    new RsaSecurityKey(RSA.Create()),
                    SecurityAlgorithms.RsaSha256
                ),
            }
        ).RawData;

        return Ok(new { token });
    }

    [HttpGet("rsa")]
    public IActionResult DebugRsa()
    {
        logger.LogInformation("Debug create security RSA");

        var rsa = RSA.Create();
        var sk = new RsaSecurityKey(rsa);
        var publicKey = rsa.ExportRSAPublicKeyPem();
        var privateKey = rsa.ExportRSAPrivateKeyPem();

        return Ok(new { publicKey, privateKey });
    }
}
