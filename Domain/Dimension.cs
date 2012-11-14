using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemOperationsEvaluation.Domain
{
	[Serializable]
	public class Dimension : BaseClass
	{
		public string Summary { get; set; }
		public List<Level> Levels { get; set; }
		public int CurrentLevelNumber { get; set; }

		public Dimension()
		{
			Levels = new List<Level>();
		}
	}
}
