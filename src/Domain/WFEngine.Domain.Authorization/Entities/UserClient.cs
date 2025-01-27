﻿using WFEngine.Domain.Common;
using WFEngine.Domain.Common.Enums;

namespace WFEngine.Domain.Authorization.Entities
{
    public class UserClient : BaseEntity
	{
		public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public int UserId { get; set; }
		public string TenantId { get; set; }

		public virtual User User { get; set; }
		public virtual Tenant Tenant { get; set; }

		public UserClient()
		{
			Status = EnumStatus.Active;
		}
	}
}

