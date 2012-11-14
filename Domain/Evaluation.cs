using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemOperationsEvaluation.Domain.Enumerations;
using System.Configuration;

namespace SystemOperationsEvaluation.Domain
{
	// Container for class evaluation data
	[Serializable]
	public class Evaluation : BaseClass
	{
		public User User { get; set; }
		public int UserID { get; set; }
		public Role Role { get; set; }
		public int RoleID { get; set; }

		public EvaluationStatusEnum Status { get; set; }
		public int StatusID { get; set; }

		public List<Dimension> Dimensions { get; set; }
		public List<Level> CurrentLevels { get; set; }
		public List<Response> Responses { get; set; }
		public int EvaluationResultID { get; set; }

		public int NumDimensions { get { return Dimensions.Count; } }

		public int? CurrentDimensionIndex { get; set; }

		private int? GetCurrentLevelDisplay(int DimensionID)
		{
			if (CurrentLevels.Find(i => i.DimensionID == DimensionID) != null)
			{
				return CurrentLevels.Find(i => i.DimensionID == DimensionID).LevelNumber;
			}
			return null;
		}

		public Evaluation()
		{
			ID = 0;
			Name = "";
			Description = "";
			StatusID = (int)EvaluationStatusEnum.InProgress;
			User = new User();
			Role = new Role();
			Responses = new List<Response>();
			Dimensions = new List<Dimension>();
			CurrentLevels = new List<Level>();
			CurrentDimensionIndex = -1;
		}
	}
}
