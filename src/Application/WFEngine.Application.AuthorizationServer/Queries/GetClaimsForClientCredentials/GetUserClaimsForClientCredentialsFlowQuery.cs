using MediatR;
using Microsoft.Extensions.Localization;
using WFEngine.Application.Common.Constants;
using WFEngine.Application.Common.Models;
using WFEngine.Domain.Authorization.Entities;
using WFEngine.Domain.Authorization.Repositories;

namespace WFEngine.Application.AuthorizationServer.Queries.GetClaimsForClientCredentials
{
    public class GetUserClaimsForClientCredentialsFlowQuery : IRequest<ApiResponse<UserClaim>>
    {
        public string ClientId { get; private set; }
        public string ClientSecret { get; private set; }

        public GetUserClaimsForClientCredentialsFlowQuery(string clientId, string clientSecret)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
        }
    }

    public class GetUserClaimsForClientCredentialsFlowQueryHandler : IRequestHandler<GetUserClaimsForClientCredentialsFlowQuery, ApiResponse<UserClaim>>
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

        public Task<ApiResponse<UserClaim>> Handle(GetUserClaimsForClientCredentialsFlowQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
