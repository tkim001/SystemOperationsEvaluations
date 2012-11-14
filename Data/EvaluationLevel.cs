using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemOperationsEvaluation.Data
{
	public partial class EvaluationLevel
	{
		public static List<Domain.Level> GetLevels(int evaluationID)
		{
			using (EvaluationDBDataContext db = new EvaluationDBDataContext())
			{
				var q = (from eLevels in db.EvaluationLevels
					    join levels in db.Levels on eLevels.LevelID equals levels.LevelID
					    where eLevels.EvaluationID == evaluationID
					    select levels);

				return q.Select(i => i.GetDomainObject(0, false, false)).ToList().OrderBy(i => i.Name).ToList();
			}
		}

		public static void AddEvaluationLevel(Domain.Evaluation dto)
		{
			DeleteEvaluationLevels(dto.ID);

			using (EvaluationDBDataContext db = new EvaluationDBDataContext())
			{
				foreach (Domain.Level level in dto.CurrentLevels)
				{
					if (level.LevelNumber != 0)
					{
						EvaluationLevel eLevel = new EvaluationLevel
						{
							EvaluationID = dto.ID,
							LevelID = level.ID,
							DimensionID = level.DimensionID,
							DateCreated = DateTime.Now,
							DateModified = DateTime.Now
						};

						db.EvaluationLevels.InsertOnSubmit(eLevel);
						db.SubmitChanges();
					}
				}
			}
		}

		public static void AddUpdateEvaluationLevel(int evaluationID, int dimensionID, int levelID)
		{
			DeleteEvaluationLevel(evaluationID, dimensionID);

			using (EvaluationDBDataContext db = new EvaluationDBDataContext())
			{
				EvaluationLevel eLevel = new EvaluationLevel
				{
					EvaluationID = evaluationID,
					LevelID = levelID,
					DimensionID = dimensionID,
					DateCreated = DateTime.Now,
					DateModified = DateTime.Now
				};

				db.EvaluationLevels.InsertOnSubmit(eLevel);
				db.SubmitChanges();
			}
		}

		public static void DeleteEvaluationLevels(int evaluationID)
		{
			using (EvaluationDBDataContext db = new EvaluationDBDataContext())
			{
				var rowsToDelete = from q in db.EvaluationLevels
							    where q.EvaluationID == evaluationID
							    select q;

				db.EvaluationLevels.DeleteAllOnSubmit(rowsToDelete);
				db.SubmitChanges();
			}
		}

		public static void DeleteEvaluationLevel(int evaluationID, int dimensionID)
		{
			using (EvaluationDBDataContext db = new EvaluationDBDataContext())
			{
				var rowsToDelete = from q in db.EvaluationLevels
							    where q.EvaluationID == evaluationID && q.DimensionID == dimensionID
							    select q;

				db.EvaluationLevels.DeleteAllOnSubmit(rowsToDelete);
				db.SubmitChanges();
			}
		}
	}
}
