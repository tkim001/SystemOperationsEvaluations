using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemOperationsEvaluation.Data
{
	public partial class TimeZone
	{
		public Domain.TimeZone GetDomainObject()
		{
			Domain.TimeZone dto = new Domain.TimeZone();
			dto.ID = this.TimeZoneID;
			dto.Name = this.TimeZoneName;
			dto.DaylightSavingsOffsetMinutes = this.DaylightSavingsOffsetMinutes;
			dto.StandardOffsetMinutes = this.StandardOffsetMinutes;
			return dto;
		}

		public static List<Domain.TimeZone> GetTimeZones()
		{
			using (EvaluationDBDataContext db = new EvaluationDBDataContext())
			{
				List<Domain.TimeZone> timeZones = db.TimeZones.Select(q => q.GetDomainObject()).ToList();
				return timeZones;
			}
		}
	}
}
