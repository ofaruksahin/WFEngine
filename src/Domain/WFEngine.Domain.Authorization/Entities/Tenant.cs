using WFEngine.Domain.Common;
using WFEngine.Domain.Common.Enums;

namespace WFEngine.Domain.Authorization.Entities
{
    public class Tenant : BaseEntity
    {
        public string TenantId { get; set; }
        public string TenantName { get; set; }
        public string Domain { get; set; }

        public virtual ICollection<TenantUser> TenantUsers { get; set; }
        public virtual ICollection<UserClient> Clients { get; set; }

        public Tenant()
        {
            Status = EnumStatus.Active;

            TenantUsers = new List<TenantUser>();
            Clients = new List<UserClient>();
        }

        public Tenant(string tenantId, string tenantName, string domain)
        {
            TenantId = tenantId;
            TenantName = tenantName;
            Domain = domain;

            Status = EnumStatus.Active;

            TenantUsers = new List<TenantUser>();
            Clients = new List<UserClient>();
        }
    }
}
