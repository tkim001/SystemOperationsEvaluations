using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemOperationsEvaluation.Data
{
	public partial class Role
	{
		public Domain.Role GetDomainObject(bool getChildrenObject)
		{
			Domain.Role dto = new Domain.Role();
			dto.ID = this.RoleID;
			dto.Name = this.Name;
			dto.Description = this.Description;
			if (getChildrenObject)
			{
				dto.Dimensions = Dimension.GetDimensions(this.RoleID, getChildrenObject);
			}
			return dto;
		}

		public static List<Domain.Role> GetRoles(bool getChildrenObject)
		{
			using (EvaluationDBDataContext db = new EvaluationDBDataContext())
			{
				List<Domain.Role> roles = db.Roles.Select(q => q.GetDomainObject(getChildrenObject)).ToList();
				return roles;
			}
		}
	}
}
