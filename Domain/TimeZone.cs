using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemOperationsEvaluation.Domain
{
	[Serializable]
	public class TimeZone : BaseClass
	{
		public int DaylightSavingsOffsetMinutes { get; set; }
		public int StandardOffsetMinutes { get; set; }
	}
}
