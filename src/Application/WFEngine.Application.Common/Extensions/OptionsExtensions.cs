using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WFEngine.Domain.Common.Contracts;

namespace WFEngine.Application.Common.Extensions
{
    public static class OptionsExtensions
    {
        public static IServiceCollection RegisterOptions(this IServiceCollection @this, IConfiguration configuration)
        {
            AppDomain
                .CurrentDomain
                .GetAssemblies()
                .Select(assembly => assembly.GetTypes())
                .Select(types =>
                    types.Where(type =>
                            type.GetInterface(nameof(IOptions)) is not null
                        )
                    )
                .SelectMany(types => types)
                .ToList()
                .ForEach(optionType =>
                {
                    var optionInstance = (IOptions)Activator.CreateInstance(optionType);
                    configuration.GetSection(optionInstance.Key).Bind(optionInstance);

                    @this.AddSingleton(optionType, optionInstance);
                });

            return @this;
        }

        public static TOption GetOptions<TOption>(this IConfiguration configuration)
            where TOption : IOptions
        {
            TOption option = (TOption)Activator.CreateInstance(typeof(TOption));
            configuration.GetSection(option.Key).Bind(option);

            return option;
        }
    }
}
