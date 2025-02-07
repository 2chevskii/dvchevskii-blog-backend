﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Dvchevskii.Blog.Application.Authentication;
using Dvchevskii.Blog.Application.Authentication.Middleware;
using Dvchevskii.Blog.Application.Authentication.Services;
using Dvchevskii.Blog.Application.Authentication.Users;
using Dvchevskii.Blog.Application.Common;
using Dvchevskii.Blog.Application.Content.Files;
using Dvchevskii.Blog.Application.Infrastructure.Services;
using Dvchevskii.Blog.Application.Posts;
using Dvchevskii.Blog.Contracts.Authentication;
using Dvchevskii.Blog.Contracts.Authentication.Services;
using Dvchevskii.Blog.Contracts.Authentication.Users;
using Dvchevskii.Blog.Contracts.Files;
using Dvchevskii.Blog.Contracts.Infrastructure;
using Dvchevskii.Blog.Contracts.Posts.Services;
using Dvchevskii.Blog.Infrastructure.Persistence.Authentication.Users;
using Dvchevskii.Blog.Infrastructure.Persistence.DbContexts;
using Dvchevskii.Blog.Infrastructure.Persistence.Interceptors;
using Dvchevskii.Blog.Infrastructure.Persistence.SetupHandlers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;

namespace Dvchevskii.Blog.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBlogDatabase(this IServiceCollection services)
    {
        services.AddSetupHandler<BlogDatabaseSetupHandler>();
        services.AddScoped<EntityAuditInfoSetterInterceptor>();
        services.AddDbContext<BlogDbContext>((serviceProvider, options) =>
        {
            var connectionString = serviceProvider.GetRequiredService<IConfiguration>()
                .GetConnectionString("Database");

            var customDbHost = serviceProvider.GetRequiredService<IConfiguration>()
                .GetValue<string>("Db_Host");

            if (!string.IsNullOrEmpty(customDbHost))
            {
                connectionString += $"Host={customDbHost};";
            }

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new Exception("Connection string Database was not found");
            }

            var auditInfoSetterInterceptor = serviceProvider.GetRequiredService<EntityAuditInfoSetterInterceptor>();
            options.AddInterceptors(auditInfoSetterInterceptor)
                .UseMySql(
                    connectionString,
                    new MySqlServerVersion("8.4.3"),
                    mysql => mysql.MigrationsAssembly(typeof(BlogDbContext).Assembly.GetName().Name)
                );
        });
        return services;
    }

    public static IServiceCollection AddApplicationAuthentication(this IServiceCollection services)
    {
        services.AddSingleton<IAuthenticationContextFactory, AuthenticationContextFactory>();
        services.AddSingleton<IAuthenticationContextProvider, AuthenticationContextProvider>();
        services.AddSingleton<IAuthenticationContext>(
            serviceProvider =>
                (AuthenticationContextProvider)serviceProvider.GetRequiredService<IAuthenticationContextProvider>()
        );

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Events.OnValidatePrincipal += context =>
                {
                    if (context.Principal != null)
                    {
                        context.ReplacePrincipal(new BlogClaimsPrincipal(context.Principal));
                    }

                    return Task.CompletedTask;
                };
                options.Events.OnRedirectToAccessDenied +=
                    context => Results.Unauthorized().ExecuteAsync(context.HttpContext);
                options.Events.OnRedirectToLogin += context => Results.Unauthorized().ExecuteAsync(context.HttpContext);
            });
        services.AddScoped<AuthenticationContextSetterMiddleware>();

        return services;
    }

    public static IServiceCollection AddApplicationAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(authz =>
        {
            authz.AddPolicy("user", p =>
            {
                p.RequireAuthenticatedUser()
                    .RequireRole("user");
            });
            authz.AddPolicy("admin", p =>
            {
                p.RequireAuthenticatedUser()
                    .RequireRole("admin");
            });

            authz.DefaultPolicy = authz.GetPolicy("user")!;
        });

        return services;
    }

    public static IServiceCollection AddAdminServices(this IServiceCollection services)
    {
        services.AddScoped<IPostManagerService, PostManagerService>();

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IBlogAuthenticationService, BlogAuthenticationService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserPasswordService, UserPasswordService>();

        /*services.AddAutoMapper(config =>
        {
            config.AddProfile<EntityBaseMapperProfile>();
            config.AddProfile<AuditInfoMapperProfile>();
            config.AddProfile<PostsMapperProfile>();
            config.AddProfile<ImagesMapperProfile>();
        });*/
        services.AddSetupHandler<AutoMapperValidationSetupHandler>();

        services.AddScoped<IImageService, ImageService>();
        services.AddScoped<ImageStorage>();
        services.AddOptions<ImageStorageOptions>().Configure(x => { x.DirectoryName = "BlogImages"; });

        services.AddScoped<IPostReaderService, PostReaderService>();

        services.AddScoped<IUserProfileService, UserProfileService>();

        return services;
    }

    public static IServiceCollection AddMapping(this IServiceCollection services, List<Type> profiles)
    {
        services.AddAutoMapper(config =>
        {
            config.AddProfile<EntityBaseMapperProfile>();
            config.AddProfile<AuditInfoMapperProfile>();
            config.AddProfile<PostsMapperProfile>();
            config.AddProfile<ImagesMapperProfile>();
            config.AddProfile<UsersMapperProfile>();

            profiles.ForEach(config.AddProfile);
        });

        return services;
    }

    public static IServiceCollection AddUtilities(this IServiceCollection services)
    {
        services.AddSingleton<IUsernameNormalizer, UsernameNormalizer>();
        services.AddSingleton(TimeProvider.System);
        services.AddSingleton<SetupHandlerRunner>();
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<UserRepository>();
        return services;
    }

    public static BlogClaimsPrincipal AsBlogClaimsPrincipal(this ClaimsPrincipal claimsPrincipal) =>
        (BlogClaimsPrincipal)claimsPrincipal;

    public static IServiceCollection AddSetupHandler<THandler>(this IServiceCollection services)
        where THandler : class, ISetupHandler
    {
        services.AddScoped<ISetupHandler, THandler>();
        return services;
    }

    private static JwtSecurityToken AsJwt(this SecurityToken self) => (JwtSecurityToken)self;
}
