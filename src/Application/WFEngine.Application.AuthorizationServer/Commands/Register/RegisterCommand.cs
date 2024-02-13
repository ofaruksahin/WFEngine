using FluentValidation;
using MediatR;
using Microsoft.Extensions.Localization;
using WFEngine.Application.Common;
using WFEngine.Application.Common.Constants;
using WFEngine.Application.Common.Pipelines.Validation;
using WFEngine.Application.Common.Validators;

namespace WFEngine.Application.AuthorizationServer.Commands.Register
{
    public class RegisterCommand : IRequest<BaseResponse>
    {
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RegisterCommandValidator : BaseValidator<RegisterCommand>
    {
        public RegisterCommandValidator(IStringLocalizer<ValidationMessageConstants> l) : base(l)
        {
            RuleFor(p => p.CompanyName)
                .NotEmpty()
                .WithMessage(string.Format(_l[ValidationMessageConstants.Required], nameof(RegisterCommand.CompanyName)))
                .MaximumLength(100)
                .WithMessage(string.Format(_l[ValidationMessageConstants.MaximumLength100Characters], nameof(RegisterCommand.Email)));

            RuleFor(p => p.Email)
             .NotEmpty()
             .WithMessage(string.Format(_l[ValidationMessageConstants.Required], nameof(RegisterCommand.Email)))
             .EmailAddress()
             .WithMessage(string.Format(_l[ValidationMessageConstants.MustBeEmail], nameof(RegisterCommand.Email)))
             .MaximumLength(200)
             .WithMessage(string.Format(_l[ValidationMessageConstants.MaximumLength200Characters], nameof(RegisterCommand.Email)));

            RuleFor(p => p.Password)
                .NotEmpty()
                .WithMessage(string.Format(_l[ValidationMessageConstants.Required], nameof(RegisterCommand.Password)))
                .MaximumLength(50)
                .WithMessage(string.Format(_l[ValidationMessageConstants.MaximumLength50Characters], nameof(RegisterCommand.Password)));
        }
    }

    [Validate(typeof(RegisterCommandValidator), typeof(RegisterCommand))]
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, BaseResponse>
    {
        public async Task<BaseResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            return null;
        }
    }
}
