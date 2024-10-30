using AutoMapper;
using Dvchevskii.Blog.Contracts.Infrastructure;

namespace Dvchevskii.Blog.Application.Common;

public class AutoMapperValidationSetupHandler(IConfigurationProvider mapperConfigurationProvider) : ISetupHandler
{
    public Task Execute()
    {
        mapperConfigurationProvider.AssertConfigurationIsValid();
        return Task.CompletedTask;
    }
}
