using FluentValidation;
using MediatR;
using System.Net;
using System.Reflection;

namespace WFEngine.Application.Common.Pipelines.Validation
{
    public class FluentValidationPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : BaseResponse
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IRequestHandler<TRequest, TResponse> _handler;

        public FluentValidationPipeline(
            IServiceProvider serviceProvider,
            IRequestHandler<TRequest, TResponse> handler)
        {
            _serviceProvider = serviceProvider;
            _handler = handler;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var validationAttribute = _handler
                .GetType()
                .GetCustomAttributes<ValidateAttribute>(false)
                .FirstOrDefault() as ValidateAttribute;

            if(validationAttribute is null)
                return await next();

            var validator = _serviceProvider.GetService(validationAttribute.ValidatorType);

            if (validator is not null && validator is IValidator requestValidator)
            {
                var validationContextType = typeof(ValidationContext<>).MakeGenericType(validationAttribute.ValidatorContextType);
                var validationContext = Activator.CreateInstance(validationContextType, request) as IValidationContext;

                var validationResult = requestValidator.Validate(validationContext);
                if (!validationResult.IsValid)
                {
                    var validationMessages = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                    return (TResponse)BaseResponse.Failure(new NoContent(), HttpStatusCode.BadRequest, validationMessages);
                }
            }

            return await next();
        }
    }
}
