using Dvchevskii.Blog.Application.Extensions;
using Dvchevskii.Blog.Contracts.Authentication;
using Dvchevskii.Blog.Contracts.Authentication.Services;
using Microsoft.AspNetCore.Http;

namespace Dvchevskii.Blog.Application.Authentication.Middleware;

public class AuthenticationContextSetterMiddleware(
    IAuthenticationContextProvider authenticationContextProvider,
    IAuthenticationContextFactory authenticationContextFactory
) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var endpoint = context.GetEndpoint();
        var endpointMetadata = endpoint!.Metadata.GetMetadata<UseSystemUserAttribute>();

        if (endpointMetadata != null)
        {
            using var scope = authenticationContextProvider.CreateScope(
                authenticationContextFactory.Create(TechnicalUsers.System)
            );
            await next(context);
            return;
        }

        if (context.User.Identity?.IsAuthenticated != true)
        {
            await next(context);
            return;
        }

        var principal = context.User.AsBlogClaimsPrincipal();

        using var authenticationContextScope = authenticationContextProvider.CreateScope(
            new AuthenticationContextData
            {
                IsAuthenticated = true,
                UserId = principal.UserId,
                Username = principal.Username,
                IsAdmin = principal.IsAdmin,
            }
        );
        await next(context);
    }
}
