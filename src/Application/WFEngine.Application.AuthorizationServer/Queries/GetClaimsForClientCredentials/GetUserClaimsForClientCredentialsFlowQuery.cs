using MediatR;
using Microsoft.Extensions.Localization;
using WFEngine.Application.Common.Constants;
using WFEngine.Application.Common.Models;
using WFEngine.Domain.Authorization.Entities;
using WFEngine.Domain.Authorization.Repositories;

namespace WFEngine.Application.AuthorizationServer.Queries.GetClaimsForClientCredentials
{
    public class GetUserClaimsForClientCredentialsFlowQuery : IRequest<ApiResponse<List<UserClaim>>>
    {
        public string ClientId { get; private set; }
        public string ClientSecret { get; private set; }

        public GetUserClaimsForClientCredentialsFlowQuery(string clientId, string clientSecret)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
        }
    }

    public class GetUserClaimsForClientCredentialsFlowQueryHandler : IRequestHandler<GetUserClaimsForClientCredentialsFlowQuery, ApiResponse<List<UserClaim>>>
    {
        private readonly IStringLocalizer<ValidationMessageConstants> _l;
        private readonly IUserClientRepository _userClientRepository;
        private readonly IUserClaimRepository _userClaimRepository;

        public GetUserClaimsForClientCredentialsFlowQueryHandler(
            IStringLocalizer<ValidationMessageConstants> l, 
            IUserClientRepository userClientRepository,
            IUserClaimRepository userClaimRepository)
        {
            _l = l;
            _userClientRepository = userClientRepository;
            _userClaimRepository = userClaimRepository;
        }

        public async Task<ApiResponse<List<UserClaim>>> Handle(GetUserClaimsForClientCredentialsFlowQuery request, CancellationToken cancellationToken)
        {
            var response = new ApiResponse<List<UserClaim>>();

            var userClient = await _userClientRepository.GetClient(request.ClientId,request.ClientSecret);

            if (userClient is null)
                return response
                    .SetFailure()
                    .AddMessage(_l[ValidationMessageConstants.NotFound]);

            var claims = await _userClaimRepository.GetClaims(userClient.UserId);
            claims.Add(new UserClaim(userClient.UserId, "sub", userClient.Id.ToString(), true));
            claims.Add(new UserClaim(userClient.UserId, "tenant_id", userClient.TenantId, true));

            return response
                .SetSuccess()
                .AddData(claims)
                .AddMessage(_l[ValidationMessageConstants.Success]);
        }
    }
}
