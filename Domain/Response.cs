using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemOperationsEvaluation.Domain.Enumerations;

namespace SystemOperationsEvaluation.Domain
{
	[Serializable]
	public class Response : BaseClass
	{
		public int EvaluationID { get; set; }
		public int DimensionID { get; set; }
		public int LevelID { get; set; }
		public int QuestionID { get; set; }
		public ResponseEnum SelectedValue { get; set; }
	}
}
