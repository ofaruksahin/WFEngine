using WFEngine.Domain.Authorization.Entities;
using WFEngine.Domain.Common.Contracts;

namespace WFEngine.Domain.Authorization.Repositories
{
    public interface IUserClientRepository : IRepository<UserClient>
	{
		Task<UserClient> GetClient(string clientId,string clientSecret);
	}
}

