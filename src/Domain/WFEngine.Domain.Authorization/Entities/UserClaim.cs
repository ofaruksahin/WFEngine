using WFEngine.Domain.Common;
using WFEngine.Domain.Common.Enums;

namespace WFEngine.Domain.Authorization.Entities
{
    public class UserClaim : BaseEntity
	{
		public int UserId { get; set; }
		public string Name { get; set; }
		public string Value { get; set; }
		public bool IsAddToken { get; set; }

		public virtual User User { get; set; }

		public UserClaim()
		{
			Status = EnumStatus.Active;
		}

		public UserClaim(int userId, string name, string value, bool isAddToken)
		{
			UserId = userId;
			Name = name;
			Value = value;
			IsAddToken = isAddToken;

            Status = EnumStatus.Active;
        }
    }
}

