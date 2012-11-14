using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using SystemOperationsEvaluation.Domain;

namespace SystemOperationsEvaluation.Data
{
	public class DataAccess
	{
		public static List<Domain.Role> Roles
		{
			get
			{
				Cache cache = HttpRuntime.Cache;
				if (cache["Roles"] == null)
				{
					cache["Roles"] = Role.GetRoles(true);
				}
				return (List<Domain.Role>)cache["Roles"];
			}
			set
			{
				Cache cache = HttpRuntime.Cache;
				cache.Remove("Roles");
			}
		}

		public static List<Domain.Question> Questions
		{
			get
			{
				Cache cache = HttpRuntime.Cache;
				if (cache["Questions"] == null)
				{
					cache["Questions"] = Question.GetQuestions();
				}
				return (List<Domain.Question>)cache["Questions"];
			}
			set
			{
				Cache cache = HttpRuntime.Cache;
				cache.Remove("Questions");
			}
		}

		public static List<Domain.Level> Levels
		{
			get
			{
				Cache cache = HttpRuntime.Cache;
				if (cache["Levels"] == null)
				{
					cache["Levels"] = Level.GetLevels();
				}
				return (List<Domain.Level>)cache["Levels"];
			}
			set
			{
				Cache cache = HttpRuntime.Cache;
				cache.Remove("Levels");
			}
		}

		public static List<Domain.Dimension> Dimensions
		{
			get
			{
				Cache cache = HttpRuntime.Cache;
				if (cache["Dimensions"] == null)
				{
					cache["Dimensions"] = Dimension.GetDimensions();
				}
				return (List<Domain.Dimension>)cache["Dimensions"];
			}
			set
			{
				Cache cache = HttpRuntime.Cache;
				cache.Remove("Dimensions");
			}
		}

		public static List<Domain.Dimension> GetDimensionsForRole(int roleID)
		{
			Cache cache = HttpRuntime.Cache;
			if (cache["Dimensions" + roleID] == null)
			{
				cache["Dimensions" + roleID] = Dimension.GetDimensions(roleID, true);
			}
			return (List<Domain.Dimension>)cache["Dimensions" + roleID];
		}

		public static List<Domain.TimeZone> TimeZones
		{
			get
			{
				Cache cache = HttpRuntime.Cache;
				if (cache["TimeZones"] == null)
				{
					cache["TimeZones"] = TimeZone.GetTimeZones();
				}
				return (List<Domain.TimeZone>)cache["TimeZones"];
			}
			set
			{
				Cache cache = HttpRuntime.Cache;
				cache.Remove("TimeZones");
			}
		}

		public static List<Domain.EvaluationResult> EvaluationResults
		{
			get
			{
				Cache cache = HttpRuntime.Cache;
				if (cache["EvaluationResult"] == null)
				{
					cache["EvaluationResult"] = EvaluationResult.GetEvaluationResults();
				}
				return (List<Domain.EvaluationResult>)cache["EvaluationResult"];
			}
			set
			{
				Cache cache = HttpRuntime.Cache;
				cache.Remove("EvaluationResult");
			}
		}
	}
}
