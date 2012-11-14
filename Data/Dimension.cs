using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemOperationsEvaluation.Domain.Enumerations;

namespace SystemOperationsEvaluation.Data
{
	public partial class Dimension
	{
		public Domain.Dimension GetDomainObject(bool getChildrenObject)
		{
			Domain.Dimension dto = new Domain.Dimension();
			dto.ID = this.DimensionID;
			dto.Name = this.Name;
			dto.Description = this.Description;
			dto.Summary = this.Summary;
			if (getChildrenObject)
			{
				if (this.DimensionID >= (int)DimensionEnum.Implementation)
				{
					dto.Levels = Level.GetLevels(this.DimensionID, getChildrenObject, false);
				}
			}
			return dto;
		}

		public static Domain.Dimension GetDimension(int dimensionID, bool getChildrenObject)
		{
			using (EvaluationDBDataContext db = new EvaluationDBDataContext())
			{
				Domain.Dimension dimension = db.Dimensions.Where(i => i.DimensionID == dimensionID).Select(i => i.GetDomainObject(getChildrenObject)).FirstOrDefault();
				return dimension;
			}
		}

		public static List<Domain.Dimension> GetDimensions()
		{
			using (EvaluationDBDataContext db = new EvaluationDBDataContext())
			{
				return db.Dimensions.Select(i => i.GetDomainObject(false)).ToList();
			}
		}

		public static List<Domain.Dimension> GetDimensions(int roleID, bool getChildrenObject)
		{
			// These roles share dimensions
			if (roleID == (int)RoleEnum.SeniorStaff)
			{
				roleID = (int)RoleEnum.ActivityManager;
			}
			using (EvaluationDBDataContext db = new EvaluationDBDataContext())
			{
				var q = (from dimensionRoles in db.DimensionRoles
					    join dimension in db.Dimensions on dimensionRoles.DimensionID equals dimension.DimensionID
					    where dimensionRoles.RoleID == roleID
					    orderby dimension.DimensionID
					    select dimension).Distinct();

				List<Domain.Dimension> dimensions = q.Select(i => i.GetDomainObject(getChildrenObject)).ToList();
				return dimensions;
			}
		}
	}
}
