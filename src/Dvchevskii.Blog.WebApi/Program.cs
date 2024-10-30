using Dvchevskii.Blog.Application.Authentication.Middleware;
using Dvchevskii.Blog.Application.Extensions;
using Dvchevskii.Blog.Application.Infrastructure.Services;
using Dvchevskii.Blog.WebApi.Auth;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

builder.UseApplicationLogging();

builder.Services.AddControllers(options => { options.ModelValidatorProviders.Clear(); });
builder.UseCanonicalRoutes();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<KestrelServerOptions>(options => { options.AllowSynchronousIO = true; });

builder.Services
    .AddBlogDatabase()
    .AddRepositories()
    .AddApplicationServices()
    .AddAdminServices()
    .AddUtilities()
    .AddApplicationAuthentication();

builder.Services.AddHttpContextAccessor();

builder.Services.AddMapping([
    typeof(AuthRequestsMapperProfile),
]);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseMiddleware<AuthenticationContextSetterMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.UseCors(cors =>
{
    cors.WithOrigins("http://localhost:3000")
        .AllowCredentials()
        .AllowAnyHeader()
        .AllowAnyMethod();
});

await app.Services.GetRequiredService<SetupHandlerRunner>().Run();
app.Run();
