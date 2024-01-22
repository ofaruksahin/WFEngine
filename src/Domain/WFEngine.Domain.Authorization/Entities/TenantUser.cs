using WFEngine.Domain.Authorization.Constants;
using WFEngine.Domain.Common;
using WFEngine.Domain.Common.Enums;

namespace WFEngine.Domain.Authorization.Entities
{
    public class TenantUser : BaseEntity
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Language { get; set; }
        public virtual ICollection<Tenant> Tenants { get; set; }

        public TenantUser()
        {
            Language = DefaultLanguageConstant.DefaultLanguage;
            Status = EnumStatus.Active;

            Tenants = new List<Tenant>();
        }

        public TenantUser(string email, string password)
        {
            Email = email;
            Password = password;
            Language = DefaultLanguageConstant.DefaultLanguage;
            Status = EnumStatus.Active;


            Tenants = new List<Tenant>();
        }
    }
}
