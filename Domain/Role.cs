using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemOperationsEvaluation.Domain
{
	[Serializable]
	public class Role : BaseClass
	{
		public List<Dimension> Dimensions { get; set; }

		public Role()
		{
			Dimensions = new List<Dimension>();
		}
	}
}
