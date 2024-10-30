using System.Reflection;
using Dvchevskii.Blog.Contracts.Authentication;
using Dvchevskii.Blog.Contracts.Authentication.Services;
using Dvchevskii.Blog.Contracts.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Dvchevskii.Blog.Application.Infrastructure.Services;

public sealed class SetupHandlerRunner(
    ILogger<SetupHandlerRunner> logger,
    IAuthenticationContextProvider authenticationContextProvider,
    IAuthenticationContextFactory authenticationContextFactory,
    IServiceProvider serviceProvider
)
{
    public async Task Run()
    {
        await using var serviceScope = serviceProvider.CreateAsyncScope();
        var setupHandlers = serviceScope.ServiceProvider.GetServices<ISetupHandler>();
        var totalHandlers = setupHandlers.Count();
        var processedHandlers = 0;
        logger.LogDebug("Running setup handlers, {HandlerCount} total", setupHandlers.Count());
        var orderedHandlers = OrderHandlers(setupHandlers);

        foreach (var setupHandler in orderedHandlers)
        {
            logger.LogInformation(
                "{ProcessedHandlers}/{TotalHandlers} Running handler {HandlerTypeName} with order {HandlerOrder}",
                ++processedHandlers,
                totalHandlers,
                setupHandler.GetType().Name,
                setupHandler.GetType().GetCustomAttribute<SetupOrderAttribute>()?.Order ?? int.MaxValue
            );

            UserDto? userDto = null;
            var setupUserAttribute = setupHandler.GetType().GetCustomAttribute<SetupUserAttribute>();

            if (setupUserAttribute != null)
            {
                userDto = setupUserAttribute.User;
            }

            var authenticationContext = userDto == null
                ? authenticationContextFactory.CreateDefault()
                : authenticationContextFactory.Create(userDto);

            using var authenticationContextScope = authenticationContextProvider.CreateScope(authenticationContext);

            await setupHandler.Execute();
        }
    }

    public async Task RunIdempotent()
    {
        throw new NotImplementedException();
    }

    private static IEnumerable<ISetupHandler> OrderHandlers(IEnumerable<ISetupHandler> setupHandlers)
    {
        return setupHandlers.OrderBy(x => x.GetType().GetCustomAttribute<SetupOrderAttribute>()?.Order ?? int.MaxValue);
    }
}
