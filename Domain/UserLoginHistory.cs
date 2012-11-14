using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemOperationsEvaluation.Domain
{
	public class UserLoginHistory
	{
		public int UserID { get; set; }
		public DateTime LoginDate { get; set; }

		public UserLoginHistory()
		{

		}

		public UserLoginHistory(int userID, DateTime loginDate)
		{
			UserID = userID;
			LoginDate = loginDate;
		}
	}
}
