using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemOperationsEvaluation.Data
{
	public partial class Evaluation
	{
		public Domain.Evaluation GetDomainObject(bool getChildrenObject, bool getParent)
		{
			return GetDomainObject(getChildrenObject, getParent, false);
		}

		public Domain.Evaluation GetDomainObject(bool getChildrenObject, bool getParent, bool getUserInfo)
		{
			Domain.Evaluation dto = new Domain.Evaluation();
			dto.ID = this.EvaluationID;
			dto.UserID = this.UserID;
			
			dto.Name = this.Name;
			dto.Description = this.Description;
			dto.RoleID = this.RoleID;
			dto.Role = DataAccess.Roles.Find(i => i.ID == this.RoleID);
			dto.StatusID = this.StatusID;
			dto.DateCreated = this.DateCreated;
			dto.DateModified = this.DateModified;
			dto.CurrentLevels = EvaluationLevel.GetLevels(this.EvaluationID);

			if (getChildrenObject)
			{
				dto.Dimensions = DataAccess.GetDimensionsForRole(this.RoleID);
				dto.Responses = Response.GetResponses(this.EvaluationID);
			}

			if (getParent)
			{
			}

			if (getUserInfo)
			{
				dto.User = User.GetUser(this.UserID);
			}
			return dto;
		}

		#region CRUD
		public static List<Domain.Evaluation> GetEvaluations()
		{
			using (EvaluationDBDataContext db = new EvaluationDBDataContext())
			{
				return db.Evaluations.Select(i => i.GetDomainObject(false, false)).ToList();
			}
		}

		public static List<Domain.Evaluation> GetEvaluations(int userID)
		{
			using (EvaluationDBDataContext db = new EvaluationDBDataContext())
			{
				return db.Evaluations.Where(i => i.UserID == userID).Select(i => i.GetDomainObject(false, false)).ToList();
			}
		}

		public static List<Domain.Evaluation> GetEvaluationsAdmin()
		{
			using (EvaluationDBDataContext db = new EvaluationDBDataContext())
			{
				return db.Evaluations.Where(i => i.StatusID == 2).Select(i => i.GetDomainObject(false, false, true)).ToList();
			}
		}

		public static List<Domain.Evaluation> GetEvaluations(int userID, int statusID)
		{
			using (EvaluationDBDataContext db = new EvaluationDBDataContext())
			{
				return db.Evaluations.Where(i => i.UserID == userID && i.StatusID == statusID).Select(i => i.GetDomainObject(false, false)).ToList().OrderByDescending(i => i.DateModified).ToList();
			}
		}

		public static Domain.Evaluation GetEvaluation(int evaluationID)
		{
			using (EvaluationDBDataContext db = new EvaluationDBDataContext())
			{
				var q = (from Evaluations in db.Evaluations
					    where Evaluations.EvaluationID == evaluationID
					    select Evaluations).FirstOrDefault();

				return q.GetDomainObject(true, true);
			}
		}

		public static int AddEvaluation(Domain.Evaluation dto)
		{
			using (EvaluationDBDataContext db = new EvaluationDBDataContext())
			{
				Evaluation Evaluation = new Evaluation
				{
					Name = dto.Name,
					Description = dto.Description,
					StatusID = dto.StatusID,
					UserID = dto.UserID,
					RoleID = dto.RoleID,
					DateCreated = DateTime.Now,
					DateModified = DateTime.Now
				};

				// Update evaluation levels

				// Update responses

				db.Evaluations.InsertOnSubmit(Evaluation);
				db.SubmitChanges();
				dto.ID = Evaluation.EvaluationID;

				return dto.ID;
			}
		}

		public static void UpdateEvaluation(Domain.Evaluation dto)
		{
			using (EvaluationDBDataContext db = new EvaluationDBDataContext())
			{
				Evaluation foundEvaluation = db.Evaluations.Where(i => i.EvaluationID == dto.ID).SingleOrDefault();

				if (foundEvaluation != null)
				{
					foundEvaluation.Name = dto.Name;
					foundEvaluation.Description = dto.Description;
					foundEvaluation.StatusID = dto.StatusID;
					foundEvaluation.RoleID = dto.RoleID;
					foundEvaluation.DateModified = DateTime.Now;
					db.SubmitChanges();
				}

				// Update evaluation responses
			}
		}

		public static void UpdateEvaluationLastModified(int EvaluationID, DateTime LastModified)
		{
			using (EvaluationDBDataContext db = new EvaluationDBDataContext())
			{
				Evaluation foundEvaluation = db.Evaluations.Where(i => i.EvaluationID == EvaluationID).SingleOrDefault();

				if (foundEvaluation != null)
				{
					foundEvaluation.DateModified = LastModified;
					db.SubmitChanges();
				}
			}
		}

		public static void DeleteEvaluation(int evaluationID)
		{
			using (EvaluationDBDataContext db = new EvaluationDBDataContext())
			{				
				// Delete evaluation levels
				EvaluationLevel.DeleteEvaluationLevels(evaluationID);

				// Delete responses
				Response.DeleteResponses(evaluationID);

				// Delete evaluation
				var rowsToDelete = from q in db.Evaluations
							    where q.EvaluationID == evaluationID
							    select q;

				db.Evaluations.DeleteAllOnSubmit(rowsToDelete);
				db.SubmitChanges();
			}

		}

		#endregion
	}
}
