using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemOperationsEvaluation.Domain.Enumerations;

namespace SystemOperationsEvaluation.Data
{
	public partial class Level
	{
		public Domain.Level GetDomainObject(int dimensionID, bool getChildrenObject, bool getParent)
		{
			Domain.Level dto = new Domain.Level();
			dto.ID = this.LevelID;
			dto.DimensionID = this.DimensionID;
			dto.LevelNumber = this.LevelNumber;
			dto.Name = this.Name;
			dto.Description = this.Description;
			dto.DateCreated = this.DateCreated;
			dto.DateModified = this.DateModified;

			if (getChildrenObject)
			{
				dto.Questions = Question.GetLevelQuestions(this.LevelID);
			}

			if (getParent)
			{
				dto.Dimension = Dimension.GetDimension(this.DimensionID, false);
			}
			return dto;
		}

		public static Domain.Level GetQuestionLevel(int questionID)
		{
			using (EvaluationDBDataContext db = new EvaluationDBDataContext())
			{
				var q = (from levels in db.Levels
					    join levelQuestions in db.LevelQuestions on levels.LevelID equals levelQuestions.LevelID
					    where levelQuestions.QuestionID == questionID
					    select levels).FirstOrDefault();

				return q.GetDomainObject(0, false, true);
			}
		}

		public static List<Domain.Level> GetLevels(int dimensionID, bool getChildrenObject, bool getParentObject)
		{
			using (EvaluationDBDataContext db = new EvaluationDBDataContext())
			{
				List<Domain.Level> levels = db.Levels.OrderBy(i => i.LevelNumber).Where(i => i.DimensionID == dimensionID).Select(i => i.GetDomainObject(dimensionID, getChildrenObject, getParentObject)).ToList();
				return levels;
			}
		}

		public static List<Domain.Level> GetLevels()
		{
			using (EvaluationDBDataContext db = new EvaluationDBDataContext())
			{
				List<Domain.Level> levels = db.Levels.Where(i => i.DimensionID > (int)DimensionEnum.Implementation).OrderBy(i => i.DimensionID).Select(i => i.GetDomainObject(0, false, true)).ToList();
				return levels;
			}
		}

		// We do not allow new levels to be created.  Only existing levels may be updated
		public static void UpdateLevel(int levelID, string description, string capability, string pdfName)
		{
			using (EvaluationDBDataContext db = new EvaluationDBDataContext())
			{
				Level level = db.Levels.Where(i => i.LevelID == levelID).SingleOrDefault();

				if (level != null)
				{
					level.Description = description;
					level.Capability = capability;
					level.PdfName = pdfName;
					level.DateModified = DateTime.Now;
					db.SubmitChanges();
				}
			}
		}
	}
}
