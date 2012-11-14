using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemOperationsEvaluation.Domain.Enumerations;

namespace SystemOperationsEvaluation.Domain
{
	[Serializable]
	public class Question : BaseClass
	{
		public Dimension Dimension { get; set; }
		public int DimensionID { get; set; }
		public Level Level { get; set; }
		public int LevelID { get; set; }
		public int LevelNumber { get; set; }
	}
}
