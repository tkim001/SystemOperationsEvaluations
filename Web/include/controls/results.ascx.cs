using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SystemOperationsEvaluation.Domain.Enumerations;
using System.Text;
using SystemOperationsEvaluation.Data;
using System.Configuration;

namespace SystemOperationsEvaluation.Web
{
	public partial class ResultsControl : BaseControl
	{
		public new void LoadPage()
		{
			try
			{
				Revisit = false;
				DisplayEvaluationResult();
				DisplayResults();
			}
			catch (Exception ex)
			{
				CurrentEvaluation = null;
				Response.Redirect("/eval/timeout.aspx", true);
			}
		}

		protected void btnSubmit_Click(object sender, EventArgs e)
		{
			SaveResultsToDB();
		}


		private void DisplayEvaluationResult()
		{
			int implementationIndex = CurrentEvaluation.Dimensions.FindIndex(i => i.ID == (int)DimensionEnum.Implementation);
			int implementationLevel = CurrentEvaluation.CurrentLevels.Find(i => i.DimensionID == (int)DimensionEnum.Implementation).LevelNumber;
			
			string messageKey = implementationLevel.ToString();

			Domain.EvaluationResult result = DataAccess.EvaluationResults.Where(i => i.Name == messageKey).FirstOrDefault();
			CurrentEvaluation.EvaluationResultID = result.ID;
		}

		private void DisplayResults()
		{
			lblDate.Text = DateTime.Today.ToShortDateString();
		}

		protected List<Domain.Level> GetLevels(object dataItem)
		{
			Domain.Dimension dimension = dataItem as Domain.Dimension;
			return dimension.Levels;
		}

		protected string DisplayLevelSelection(object dataItem, object parentDataItem)
		{
			Domain.Dimension dimension = parentDataItem as Domain.Dimension;
			Domain.Level level = dataItem as Domain.Level;

			if (CurrentEvaluation.CurrentLevels.Find(i => i.DimensionID == dimension.ID).ID == level.ID)
			{
				return "colCustomLevel smText";
			}
			return "expandedRow smText";
		}

		protected string DisplayLevelChecked(object dataItem, object parentDataItem, bool displayCheck)
		{
			Domain.Dimension dimension = parentDataItem as Domain.Dimension;
			Domain.Level level = dataItem as Domain.Level;

			if (CurrentEvaluation.CurrentLevels.Find(i => i.DimensionID == dimension.ID).ID == level.ID)
			{
				if (displayCheck)
				{
					return "colCustomLevel check";
				}
				else
				{
					return "colCustomLevel";
				}
			}
			return "";
		}

		protected void lbRevisitAll_OnClick(object sender, EventArgs e)
		{
			HideControl("ResultsControl1");
			ShowControl("ProgDeploymentControl1");
			Revisit = false;

			CurrentEvaluation.CurrentDimensionIndex = 0;
		}

		protected void rptDimensions_ItemCommand(object sender, RepeaterCommandEventArgs e)
		{
			HideControl("ResultsControl1");
			ShowControl("DimensionControl1");

			CurrentEvaluation.CurrentDimensionIndex = CurrentEvaluation.Dimensions.FindIndex(i => i.ID == int.Parse(e.CommandArgument.ToString()));
			Revisit = true;
			LoadControl("DimensionControl", "DimensionControl1");
		}

		protected void lbStartOver_OnClick(object sender, EventArgs e)
		{
			CurrentEvaluation = null;
			Response.Redirect("/eval/", true);
		}
	}
}