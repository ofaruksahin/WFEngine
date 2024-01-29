using WFEngine.Domain.Common;

namespace WFEngine.Domain.Authorization.Entities
{
    public class TenantUser : BaseEntity
    {
        public string TenantId { get; set; }
        public int UserId { get; set; }
        public bool IsMaster { get; set; }

        public virtual Tenant Tenant { get; set; }
        public virtual User User { get; set; }
    }
}
