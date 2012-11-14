using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemOperationsEvaluation.Domain
{
	[Serializable]

	public class Level : BaseClass
	{
		public int LevelNumber { get; set; }
		public int NextLevelNumber
		{
			get
			{
				return LevelNumber + 1;
			}
		}
		public Dimension Dimension { get; set; }
		public int DimensionID { get; set; }
		public List<Question> Questions { get; set; }

		public int QuestionCount
		{
			get
			{
				return Questions.Count;
			}
		}

		public Level()
		{
			Questions = new List<Question>();
			LevelNumber = 0;
		}
	}
}
