using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemOperationsEvaluation.Domain
{
	[Serializable]
	public class User : BaseClass
	{
		public string EmailAddress { get; set; }
		public string Password { get; set; }
		public string Organization { get; set; }
		public string Title { get; set; }

		public int RoleID { get; set; }
		public Role Role { get; set; }

		public int TimeZoneID { get; set; }
		public TimeZone TimeZone { get; set; }
		public int TimeZoneOffSet { get; set; }

		public List<Evaluation> Evaluations { get; set; }
		public List<UserLoginHistory> LoginHistory { get; set; }

		public DateTime? LastLoginDate { get; set; }
		public string LastLoginDateDisplay
		{
			get
			{
				if (LastLoginDate.HasValue)
				{
					return String.Format("{0:g}", LastLoginDate.Value);
				}
				return "";
			}
		}

		public string ContactInfo
		{
			get
			{
				if (Name != "")
				{
					return Name + " (" + EmailAddress + ")";
				}
				return EmailAddress;
			}
		}

		public User()
		{
		
		}
	}
}
