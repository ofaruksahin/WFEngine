using System;
using System.Linq;
using WFEngineCore.Interface;
using WFEngineCore.Services.DefinitionStorage;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWorkflowDSL(this IServiceCollection services)
        {
            services.AddTransient<IDefinitionLoader, DefinitionLoader>();
            return services;
        }
    }
}

