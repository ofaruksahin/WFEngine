using WFEngine.Domain.Authorization.Entities;

namespace WFEngine.Domain.Authorization.Repositories
{
    public interface IUserClientRepository
	{
		Task<UserClient> GetClient(string clientId);
	}
}

