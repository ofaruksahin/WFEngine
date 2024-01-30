using WFEngine.Domain.Authorization.Entities;
using WFEngine.Domain.Common.Contracts;

namespace WFEngine.Domain.Authorization.Repositories
{
    public interface IUserClaimRepository : IRepository<UserClaim>
	{
		Task<List<UserClaim>> GetClaims(int userId);
	}
}

