using WFEngine.Domain.Authorization.Entities;
using WFEngine.Domain.Common.Contracts;

namespace WFEngine.Domain.Authorization.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserWithEmailAndPassword(string email, string password);
    }
}
