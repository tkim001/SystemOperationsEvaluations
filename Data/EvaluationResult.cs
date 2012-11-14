using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemOperationsEvaluation.Data
{
	public partial class EvaluationResult
	{
		public Domain.EvaluationResult GetDomainObject()
		{
			Domain.EvaluationResult dto = new Domain.EvaluationResult();
			dto.ID = this.EvaluationResultID;
			dto.Name = this.Name;
			dto.Description = this.Description;
			dto.DateModified = this.DateModified;
			return dto;
		}

		public static List<Domain.EvaluationResult> GetEvaluationResults()
		{
			using (EvaluationDBDataContext db = new EvaluationDBDataContext())
			{
				List<Domain.EvaluationResult> EvaluationResults = db.EvaluationResults.Select(q => q.GetDomainObject()).ToList();
				return EvaluationResults;
			}
		}

		public static void UpdateEvaluationResult(int EvaluationResultID, string resultText)
		{
			using (EvaluationDBDataContext db = new EvaluationDBDataContext())
			{
				EvaluationResult EvaluationResult = db.EvaluationResults.Where(i => i.EvaluationResultID == EvaluationResultID).SingleOrDefault();

				if (EvaluationResult != null)
				{
					EvaluationResult.Description = resultText;
					EvaluationResult.DateModified = DateTime.Now;
					db.SubmitChanges();
				}
			}
		}
	}
}
