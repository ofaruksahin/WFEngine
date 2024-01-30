using WFEngine.Domain.Authorization.Constants;
using WFEngine.Domain.Common;
using WFEngine.Domain.Common.Enums;

namespace WFEngine.Domain.Authorization.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Language { get; set; }

        public virtual ICollection<TenantUser> Tenants { get; set; }
        public virtual ICollection<UserClaim> Claims { get; set; }
        public virtual ICollection<UserClient> Clients { get; set; }

        public User()
        {
            Language = DefaultLanguageConstant.DefaultLanguage;
            Status = EnumStatus.Active;

            Tenants = new List<TenantUser>();
            Claims = new List<UserClaim>();
            Clients = new List<UserClient>();
        }

        public User(string email, string password)
        {
            Email = email;
            Password = password;
            Language = DefaultLanguageConstant.DefaultLanguage;
            Status = EnumStatus.Active;

            Tenants = new List<TenantUser>();
            Claims = new List<UserClaim>();
            Clients = new List<UserClient>();
        }
    }
}
