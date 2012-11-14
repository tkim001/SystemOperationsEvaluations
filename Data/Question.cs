using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemOperationsEvaluation.Domain;

namespace SystemOperationsEvaluation.Data
{
	public partial class Question
	{
		#region Data Object Methods

		public Domain.Question GetDomainObject(bool getParent)
		{
			Domain.Question dto = new Domain.Question();
			dto.ID = this.QuestionID;
			dto.Name = this.Name;
			dto.Description = this.Description;
			dto.DateCreated = this.DateCreated;
			dto.DateModified = this.DateModified;

			if (getParent)
			{
				dto.Level = Level.GetQuestionLevel(this.QuestionID);
				dto.LevelID = dto.Level.ID;
				dto.DimensionID = dto.Level.DimensionID;
			}

			return dto;
		}

		public Domain.Question GetDomainObject()
		{
			return GetDomainObject(false);
		}

		#endregion
		public static List<Domain.Question> GetQuestions()
		{
			using (EvaluationDBDataContext db = new EvaluationDBDataContext())
			{
				return db.Questions.Select(i => i.GetDomainObject(true)).ToList();
			}
		}

		public static List<Domain.Question> GetDimensionQuestions(int dimensionID, bool getParent)
		{
			using (EvaluationDBDataContext db = new EvaluationDBDataContext())
			{
				var q = (from questions in db.Questions
					    join levelQuestions in db.LevelQuestions on questions.QuestionID equals levelQuestions.QuestionID
					    join level in db.Levels on levelQuestions.LevelID equals level.LevelID
					    join dimensions in db.Dimensions on level.DimensionID equals dimensions.DimensionID
					    where dimensions.DimensionID == dimensionID
					    select questions).Distinct();

				return q.Select(i => i.GetDomainObject(getParent)).ToList();
			}
		}

		public static List<Domain.Question> GetLevelQuestions(int levelID)
		{
			using (EvaluationDBDataContext db = new EvaluationDBDataContext())
			{
				var q = (from questions in db.Questions
					    join levelQuestions in db.LevelQuestions on questions.QuestionID equals levelQuestions.QuestionID
					    where levelQuestions.LevelID == levelID && levelQuestions.CongestionID == null
					    select questions).Distinct();

				return q.Select(i => i.GetDomainObject()).ToList();
			}
		}

		public static List<Domain.Question> GetLevelQuestions(int levelID, int congestionID)
		{
			using (EvaluationDBDataContext db = new EvaluationDBDataContext())
			{
				var q = (from questions in db.Questions
					    join levelQuestions in db.LevelQuestions on questions.QuestionID equals levelQuestions.QuestionID
					    where levelQuestions.LevelID == levelID && levelQuestions.CongestionID == congestionID
					    select questions).Distinct();

				return q.Select(i => i.GetDomainObject()).ToList();
			}
		}

		// We do not allow new questions to be created.  Only existing questions may be updated
		public static void UpdateQuestionText(int questionID, string questionText)
		{
			using (EvaluationDBDataContext db = new EvaluationDBDataContext())
			{
				Question question = db.Questions.Where(i => i.QuestionID == questionID).SingleOrDefault();

				if (question != null)
				{
					question.Description = questionText;
					question.DateModified = DateTime.Now;
					db.SubmitChanges();
				}
			}
		}
	}
}