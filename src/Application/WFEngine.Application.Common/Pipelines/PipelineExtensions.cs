using MediatR;
using Microsoft.Extensions.DependencyInjection;
using WFEngine.Application.Common.Pipelines.Validation;

namespace WFEngine.Application.Common.Pipelines
{
    public static class PipelineExtensions
    {
        public static IServiceCollection AddMediatorPipelines(this IServiceCollection @this)
        {
            @this.AddScoped(typeof(IPipelineBehavior<,>), typeof(FluentValidationPipeline<,>));

            return @this;
        }
    }
}
