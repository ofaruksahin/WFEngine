using System.Net;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Localization;
using WFEngine.Application.Common.Constants;
using WFEngine.Application.Common.Models;
using WFEngine.Domain.Authorization.Entities;
using WFEngine.Domain.Authorization.Repositories;
using WFEngine.Domain.Common.Contracts;

namespace WFEngine.Application.AuthorizationServer.Queries.GetUserClaimsForLogin
{
    public class GetUserClaimsForLoginQuery : IRequest<ApiResponse<List<UserClaim>>>
	{
		public string Email { get; private set; }
		public string Password { get; private set; }
		public string TenantId { get; private set; }

        public GetUserClaimsForLoginQuery(string email, string password, string tenantId)
        {
            Email = email;
            Password = password;
            TenantId = tenantId;
        }
    }

	public class GetUserClaimsForLoginQueryValidator : AbstractValidator<GetUserClaimsForLoginQuery>
	{
		private readonly IStringLocalizer<ValidationMessageConstants> _l;

		public GetUserClaimsForLoginQueryValidator(IStringLocalizer<ValidationMessageConstants> l)
		{
			_l = l;

            RuleFor(p => p.Email)
                .NotEmpty()
                .WithMessage(string.Format(_l[ValidationMessageConstants.Required], nameof(GetUserClaimsForLoginQuery.Email)))
                .EmailAddress()
                .WithMessage(string.Format(_l[ValidationMessageConstants.MustBeEmail], nameof(GetUserClaimsForLoginQuery.Email)))
                .MaximumLength(200)
                .WithMessage(string.Format(_l[ValidationMessageConstants.MaximumLength200Characters], nameof(GetUserClaimsForLoginQuery.Email)));

            RuleFor(p => p.Password)
                .NotEmpty()
                .WithMessage(string.Format(_l[ValidationMessageConstants.Required], nameof(GetUserClaimsForLoginQuery.Password)))
                .MaximumLength(50)
                .WithMessage(string.Format(_l[ValidationMessageConstants.MaximumLength50Characters], nameof(GetUserClaimsForLoginQuery.Password)));

            RuleFor(p => p.TenantId)
                .NotEmpty()
                .WithMessage(string.Format(_l[ValidationMessageConstants.Required], nameof(GetUserClaimsForLoginQuery.TenantId)))
                .MaximumLength(16)
                .WithMessage(string.Format(_l[ValidationMessageConstants.MaximumLength16Characters], nameof(GetUserClaimsForLoginQuery.TenantId)));
        }
    }

    public class GetUserClaimsForLoginQueryHandler : IRequestHandler<GetUserClaimsForLoginQuery, ApiResponse<List<UserClaim>>>
    {
        private readonly IStringLocalizer<ValidationMessageConstants> _l;
        private readonly ICryptographyService _cryptographyService;
        private readonly IUserRepository _userRepository;
        private readonly IUserClaimRepository _userClaimRepository;

        public GetUserClaimsForLoginQueryHandler(
            IStringLocalizer<ValidationMessageConstants> l,
            ICryptographyService cryptographyService,
            IUserRepository userRepository,
            IUserClaimRepository userClaimRepository)
        {
            _l = l;
            _cryptographyService = cryptographyService;
            _userRepository = userRepository;
            _userClaimRepository = userClaimRepository;
        }

        public async Task<ApiResponse<List<UserClaim>>> Handle(GetUserClaimsForLoginQuery request, CancellationToken cancellationToken)
        {
            var response = new ApiResponse<List<UserClaim>>();

            var validator = new GetUserClaimsForLoginQueryValidator(_l);
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

            var selectedTenant = user
                .Tenants
                .Where(tenantUser => tenantUser.TenantId == request.TenantId)
                .Select(tenantUser => tenantUser.Tenant)
                .FirstOrDefault();

            if (selectedTenant is null)
                return response
                    .SetFailure()
                    .AddMessage(_l[ValidationMessageConstants.NotFound]);

            var claims = await _userClaimRepository.GetClaims(user.Id);
            claims.Add(new UserClaim(user.Id, "tenant_id", request.TenantId, true));

            return response
                .SetSuccess()
                .AddData(claims);
        }
    }
}

