using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Dvchevskii.Blog.Application.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplicationBuilder UseCanonicalRoutes(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<RouteOptions>(options =>
        {
            options.LowercaseUrls = true;
            options.LowercaseQueryStrings = true;
        });
        return builder;
    }

    public static WebApplicationBuilder UseApplicationLogging(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, config) =>
        {
            config.MinimumLevel.Debug()
                .WriteTo.Console();
        });

        return builder;
    }
}
