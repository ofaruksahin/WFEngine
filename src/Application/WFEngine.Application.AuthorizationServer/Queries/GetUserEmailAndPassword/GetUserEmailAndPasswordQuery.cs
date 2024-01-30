using FluentValidation;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Net;
using WFEngine.Application.Common.Constants;
using WFEngine.Application.Common.Models;
using WFEngine.Domain.Authorization.Entities;
using WFEngine.Domain.Authorization.Repositories;
using WFEngine.Domain.Common.Contracts;

namespace WFEngine.Application.AuthorizationServer.Queries.GetUserEmailAndPassword
{
    public class GetUserEmailAndPasswordQuery : IRequest<ApiResponse<User>>
    {
        public string Email { get; private set; }
        public string Password { get; private set; }
        public bool RememberMe { get; private set; }

        public GetUserEmailAndPasswordQuery(string email, string password, bool rememberMe)
        {
            Email = email;
            Password = password;
            RememberMe = rememberMe;
        }
    }

    public class GetUserEmailAndPasswordQueryValidator : AbstractValidator<GetUserEmailAndPasswordQuery>
    {
        private IStringLocalizer<ValidationMessageConstants> _l;

        public GetUserEmailAndPasswordQueryValidator(IStringLocalizer<ValidationMessageConstants> l)
        {
            _l = l;

            RuleFor(p => p.Email)
                .NotEmpty()
                .WithMessage(string.Format(_l[ValidationMessageConstants.Required], nameof(GetUserEmailAndPasswordQuery.Email)))
                .EmailAddress()
                .WithMessage(string.Format(_l[ValidationMessageConstants.MustBeEmail], nameof(GetUserEmailAndPasswordQuery.Email)))
                .MaximumLength(200)
                .WithMessage(string.Format(_l[ValidationMessageConstants.MaximumLength200Characters], nameof(GetUserEmailAndPasswordQuery.Email)));

            RuleFor(p => p.Password)
                .NotEmpty()
                .WithMessage(string.Format(_l[ValidationMessageConstants.Required], nameof(GetUserEmailAndPasswordQuery.Password)))
                .MaximumLength(50)
                .WithMessage(string.Format(_l[ValidationMessageConstants.MaximumLength50Characters], nameof(GetUserEmailAndPasswordQuery.Password)));

            RuleFor(p => p.RememberMe)
                .NotNull()
                .WithMessage(string.Format(_l[ValidationMessageConstants.Required], nameof(GetUserEmailAndPasswordQuery.RememberMe)));
        }
    }

    public class GetUserEmailAndPasswordQueryHandler : IRequestHandler<GetUserEmailAndPasswordQuery, ApiResponse<User>>
    {
        private readonly IStringLocalizer<ValidationMessageConstants> _l;
        private readonly ICryptographyService _cryptographyService;
        private readonly IUserRepository _userRepository;

        public GetUserEmailAndPasswordQueryHandler(
            IStringLocalizer<ValidationMessageConstants> l,
            ICryptographyService cryptographyService,
            IUserRepository userRepository)
        {
            _l = l;
            _cryptographyService = cryptographyService;
            _userRepository = userRepository;
        }

        public async Task<ApiResponse<User>> Handle(GetUserEmailAndPasswordQuery request, CancellationToken cancellationToken)
        {
            var response = new ApiResponse<User>();

            var validator = new GetUserEmailAndPasswordQueryValidator(_l);
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                var validationMessages = validationResult.Errors.Select(error => error.ErrorMessage);

                return response
                    .SetFailure(HttpStatusCode.BadRequest)
                    .AddMessages(validationMessages);
            }

            var encryptedPassword = _cryptographyService.Encrypt(request.Password);

            var user = await _userRepository.GetUserWithEmailAndPassword(request.Email, encryptedPassword);

            if (user is null)
                return response
                    .SetFailure()
                    .AddMessage(_l[ValidationMessageConstants.NotFound]);

            return response
                .SetSuccess()
                .AddData(user)
                .AddMessage(_l[ValidationMessageConstants.Success]);
        }
    }
}
