using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemOperationsEvaluation.Domain.Enumerations;

namespace SystemOperationsEvaluation.Data
{
	public partial class Response
	{
		#region Data Object Methods

		public Domain.Response GetDomainObject()
		{
			Domain.Response dto = new Domain.Response();
			dto.ID = this.ResponseID;
			dto.QuestionID = this.QuestionID;
			dto.EvaluationID = this.EvaluationID;
			dto.DimensionID = this.DimensionID;
			dto.SelectedValue = (ResponseEnum)this.SelectedValue;
			dto.Name = this.Name;
			dto.DateCreated = this.DateCreated;
			dto.DateModified = this.DateModified;

			return dto;
		}
		#endregion

		public static List<Domain.Response> GetResponses(int evaluationID, int dimensionID)
		{
			using (EvaluationDBDataContext db = new EvaluationDBDataContext())
			{
				return db.Responses.Where(i => i.EvaluationID == evaluationID && i.DimensionID == dimensionID).Select(i => i.GetDomainObject()).ToList();
			}
		}

		public static List<Domain.Response> GetResponses(int evaluationID)
		{
			using (EvaluationDBDataContext db = new EvaluationDBDataContext())
			{
				return db.Responses.Where(i => i.EvaluationID == evaluationID && i.DimensionID > (int)DimensionEnum.ScopeEvaluation).Select(i => i.GetDomainObject()).ToList();
			}
		}

		public static void AddResponses(List<Domain.Response> responses, int evaluationID, int dimensionID)
		{
			// Clear existing values first
			//DeleteResponses(evaluationID, dimensionID);

			using (EvaluationDBDataContext db = new EvaluationDBDataContext())
			{
				foreach (Domain.Response dto in responses)
				{
					Response response = new Response
					{
						QuestionID = dto.QuestionID,
						EvaluationID = evaluationID,
						DimensionID = dimensionID,
						Name = "",
						SelectedValue = (int)dto.SelectedValue,
						DateCreated = DateTime.Now,
						DateModified = DateTime.Now
					};

					db.Responses.InsertOnSubmit(response);
					db.SubmitChanges();
				}
			}

			Evaluation.UpdateEvaluationLastModified(evaluationID, DateTime.Now);
		}

		public static void DeleteResponses(int evaluationID, int dimensionID)
		{
			using (EvaluationDBDataContext db = new EvaluationDBDataContext())
			{
				var rowsToDelete = from q in db.Responses
							    where q.EvaluationID == evaluationID && q.DimensionID == dimensionID
							    select q;

				db.Responses.DeleteAllOnSubmit(rowsToDelete);
				db.SubmitChanges();
			}
		}

		public static void DeleteResponses(int evaluationID)
		{
			using (EvaluationDBDataContext db = new EvaluationDBDataContext())
			{
				var rowsToDelete = from q in db.Responses
							    where q.EvaluationID == evaluationID
							    select q;

				db.Responses.DeleteAllOnSubmit(rowsToDelete);
				db.SubmitChanges();
			}
		}
	}
}
