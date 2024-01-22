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

        public Tenant()
        {
            TenantUsers = new List<TenantUser>();
            Status = EnumStatus.Active;
        }

        public Tenant(string tenantId, string tenantName, string domain)
        {
            TenantId = tenantId;
            TenantName = tenantName;
            Domain = domain;

            TenantUsers = new List<TenantUser>();
            Status = EnumStatus.Active;
        }
    }
}
