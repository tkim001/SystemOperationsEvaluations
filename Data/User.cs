using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemOperationsEvaluation.Data
{
	public partial class User
	{
		public Domain.User GetDomainObject(bool getChildrenObject, bool getParent)
		{
			Domain.User dto = new Domain.User();
			dto.ID = this.UserID;
			dto.Name = this.Name;
			dto.EmailAddress = this.EmailAddress;
			dto.Organization = this.Organization;
			dto.Title = this.Title;			

			dto.RoleID = this.RoleID;
			dto.Role = DataAccess.Roles.Find(i => i.ID == this.RoleID);

			dto.TimeZoneID = this.TimeZoneID;
			dto.TimeZone = DataAccess.TimeZones.Find(i => i.ID == this.TimeZoneID);

			dto.LastLoginDate = this.LastLoginDate;

			dto.DateCreated = this.DateCreated;
			dto.DateModified = this.DateModified;

			if (getChildrenObject)
			{
				// Get login history
				dto.LoginHistory = UserLoginHistory.GetLoginHistory(this.UserID);
			}

			if (getParent)
			{
				// Get evaluation history
			}
			return dto;
		}

		#region CRUD

		public static List<Domain.User> GetUsers()
		{
			using (EvaluationDBDataContext db = new EvaluationDBDataContext())
			{
				return db.Users.Select(i => i.GetDomainObject(false, false)).ToList();
			}
		}

		public static Domain.User GetUser(int userID)
		{
			using (EvaluationDBDataContext db = new EvaluationDBDataContext())
			{
				var q = (from Users in db.Users
					    where Users.UserID == userID
					    select Users).FirstOrDefault();

				return q.GetDomainObject(true, true);
			}
		}

		public static int GetUserIDByEmail(string emailAddress)
		{
			using (EvaluationDBDataContext db = new EvaluationDBDataContext())
			{
				User foundUser = db.Users.Where(i => i.EmailAddress == emailAddress).SingleOrDefault();

				if (foundUser != null)
				{
					return foundUser.UserID;
				}
				return 0;
			}
		}

		public static int AddUser(Domain.User dto)
		{
			using (EvaluationDBDataContext db = new EvaluationDBDataContext())
			{
				DateTime CurrentTime = DateTime.Now;

				User User = new User
				{
					Name = dto.Name,
					EmailAddress = dto.EmailAddress,
					Password = dto.Password,
					Title = dto.Title,
					Organization = dto.Organization,
					RoleID = dto.RoleID,
					TimeZoneID = dto.TimeZoneID,
					DateCreated = CurrentTime,
					DateModified = CurrentTime
				};

				db.Users.InsertOnSubmit(User);
				db.SubmitChanges();
				dto.ID = User.UserID;

				UpdateUserLastLogin(dto.ID, CurrentTime);
				UserLoginHistory.AddUserLoginHistory(new Domain.UserLoginHistory(dto.ID, CurrentTime));
				return dto.ID;
			}
		}

		public static void UpdateUser(Domain.User dto)
		{
			using (EvaluationDBDataContext db = new EvaluationDBDataContext())
			{
				User foundUser = db.Users.Where(i => i.UserID == dto.ID).SingleOrDefault();

				if (foundUser != null)
				{
					foundUser.Name = dto.Name;
					foundUser.EmailAddress = dto.EmailAddress;
					foundUser.Title = dto.Title;
					foundUser.Organization = dto.Organization;
					foundUser.RoleID = dto.RoleID;
					foundUser.TimeZoneID = dto.TimeZoneID;
					if (dto.Password != "")
					{
						foundUser.Password = dto.Password;
					}
					foundUser.DateModified = DateTime.Now;
					db.SubmitChanges();
				}
			}
		}

		public static void UpdateUserLastLogin(int userID, DateTime lastLoginDate)
		{
			using (EvaluationDBDataContext db = new EvaluationDBDataContext())
			{
				User foundUser = db.Users.Where(i => i.UserID == userID).SingleOrDefault();

				if (foundUser != null)
				{
					foundUser.LastLoginDate = lastLoginDate;
					db.SubmitChanges();
				}
			}
		}

		public static void UpdateUserPassword(int userID, string password)
		{
			using (EvaluationDBDataContext db = new EvaluationDBDataContext())
			{
				User foundUser = db.Users.Where(i => i.UserID == userID).SingleOrDefault();

				if (foundUser != null)
				{
					foundUser.Password = password;
					foundUser.DateModified = DateTime.Now;
					db.SubmitChanges();
				}
			}
		}

		public static List<Domain.User> GetUsers(bool getChildrenObject, bool getParentObject)
		{
			using (EvaluationDBDataContext db = new EvaluationDBDataContext())
			{
				List<Domain.User> Users = db.Users.OrderBy(i => i.Name).Select(i => i.GetDomainObject(getChildrenObject, getParentObject)).ToList();
				return Users;
			}
		}
		#endregion

		#region Login
		public static int LoginUser(string emailAddress, string password)
		{
			using (EvaluationDBDataContext db = new EvaluationDBDataContext())
			{
				User foundUser = db.Users.Where(i => i.EmailAddress == emailAddress && i.Password == password).SingleOrDefault();

				if (foundUser != null)
				{
					int userID = foundUser.UserID;
					DateTime loginDateTime = DateTime.Now;

					UpdateUserLastLogin(userID, loginDateTime);
					UserLoginHistory.AddUserLoginHistory(new Domain.UserLoginHistory() { UserID = userID, LoginDate = loginDateTime });
					return userID;
				}
				return 0;
			}

		}

		public static bool IsExistingUser(string emailAddress)
		{
			using (EvaluationDBDataContext db = new EvaluationDBDataContext())
			{
				User foundUser = db.Users.Where(i => i.EmailAddress == emailAddress).SingleOrDefault();

				if (foundUser != null)
				{
					return true;
				}
				return false;
			}
		}
		#endregion
	}
}
