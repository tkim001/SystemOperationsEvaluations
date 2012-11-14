using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;

namespace SystemOperationsEvaluation.Domain
{
	[Serializable]
	// Common attributes shared by all classes
	public class BaseClass
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime DateModified { get; set; }
	}
}
