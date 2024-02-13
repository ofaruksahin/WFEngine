using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WFEngine.Application.Common.Extensions;
using WFEngine.Application.Common.Options;

namespace WFEngine.Application.Common.Validators
{
    public static class ValidatorExtensions
    {
        public static IServiceCollection RegisterValidators(this IServiceCollection @this,IConfiguration configuration)
        {
            var validationOptions = configuration.GetOptions<FluentValidationOptions>();

            foreach (var assembly in validationOptions.Assemblies)
            {
                var types = assembly
                    .GetTypes()
                    .Where(type => type
                        .GetInterfaces()
                        .Any(@interface => @interface.Name.Contains(nameof(IValidator)) && @interface.IsGenericType) &&
                        !type.IsAbstract)
                    .ToList();

                foreach(var type in types)
                    @this.AddScoped(type);
            }

            return @this;
        }
    }
}
