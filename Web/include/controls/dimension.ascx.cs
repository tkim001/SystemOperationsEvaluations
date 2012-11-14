using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SystemOperationsEvaluation.Domain;
using SystemOperationsEvaluation.Domain.Enumerations;
using System.Web.UI.HtmlControls;

namespace SystemOperationsEvaluation.Web
{
	public partial class DimensionControl : BaseControl
	{
		private static int questionNo = 1;
		private Dimension dimension;
		private bool? existingValue2;
		public bool existingValue
		{
			get
			{
				if (!existingValue2.HasValue)
				{
					existingValue2 = (hfExistingValue.Value == "true");
				}
				return existingValue2.Value;
			}
			set
			{
				existingValue2 = value;
			}
		}
		public new void LoadPage()
		{
			if (CurrentEvaluation.CurrentDimensionIndex == -1 || !CurrentEvaluation.CurrentDimensionIndex.HasValue)
			{
				Response.Redirect("/eval/timeout.aspx", true);
			}

			dimension = CurrentEvaluation.Dimensions[CurrentEvaluation.CurrentDimensionIndex.Value];

			if (!DisplayExistingValue())
			{
				lbNext.Attributes.Remove("disabled");
				lbNext.Attributes.Add("disabled", "disabled");
			}
			else
			{
				lbNext.CssClass = "next";
				lbNext.Attributes.Remove("disabled");
			}

			questionNo = 1;
			DisplayLevels(dimension);
		}

		private bool DisplayExistingValue()
		{
			if (CurrentEvaluation.CurrentLevels.Find(i => i.DimensionID == dimension.ID) != null || CurrentEvaluation.Responses.Find(i => i.DimensionID == dimension.ID) != null)
			{
				existingValue = true;
			}
			hfExistingValue.Value = existingValue.ToString();
			return existingValue;
		}

		private void DisplayLevels(Dimension dimension)
		{
			List<Level> levels = dimension.Levels.Where(i => i.QuestionCount > 0).ToList();
			try
			{
				if (!existingValue)
				{
					currentLevel.Value = levels[0].ID.ToString();
				}
				else if (CurrentEvaluation.CurrentLevels.Find(i => i.DimensionID == dimension.ID) != null)
				{
					currentLevel.Value = CurrentEvaluation.CurrentLevels.Find(i => i.DimensionID == dimension.ID).ID.ToString();
				}
			}
			catch (Exception ex)
			{
				Response.Redirect("/eval/timeout.aspx", true);
			}

			rptLevels.DataSource = levels;
			rptLevels.DataBind();
		}

		protected List<Domain.Question> DisplayQuestions(object dataItem)
		{
			Domain.Level level = dataItem as Domain.Level;
			return level.Questions;
		}

		public string DisplayQuestionNum()
		{
			return questionNo++.ToString();
		}

		protected void lbBack_OnClick(object sender, EventArgs e)
		{
			if (CurrentEvaluation.CurrentDimensionIndex > 1)
			{
				CurrentEvaluation.CurrentDimensionIndex--;
				LoadPage();
			}
			else
			{
				DisplayProgramDeploymentPage();
			}
		}

		private void DisplayProgramDeploymentPage()
		{
			HideControl("DimensionControl1");
			ShowControl("ProgDeploymentControl1");
			CurrentEvaluation.CurrentDimensionIndex = 0;

			LoadControl("ProgDeploymentControl", "ProgDeploymentControl1");
		}

		protected void rptQuestions_OnItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				if (existingValue)
				{
					Domain.Question question = e.Item.DataItem as Domain.Question;

					if (CurrentEvaluation.Responses.Find(i => i.QuestionID == question.ID) != null)
					{
						HtmlInputHidden hfQValue = e.Item.FindControl("hfQValue") as HtmlInputHidden;
						HtmlTableCell tdYes = e.Item.FindControl("tdYes") as HtmlTableCell;
						HtmlTableCell tdNo = e.Item.FindControl("tdNo") as HtmlTableCell;

						if (CurrentEvaluation.Responses.Find(i => i.QuestionID == question.ID).SelectedValue.Equals(ResponseEnum.yes))
						{
							hfQValue.Value = "yes";
							tdYes.Attributes["class"] = "answeredYes";
							tdNo.Attributes["class"] = "no";
						}
						else
						{
							hfQValue.Value = "no";
							tdYes.Attributes["class"] = "yes";
							tdNo.Attributes["class"] = "answeredNo";
						}
					}
				}
			}
		}

		protected void lbNext_OnClick(object sender, EventArgs e)
		{
			SaveDimensionData();

			if (Revisit)
			{
				DisplayResultsPage();
			}
			else if (CurrentEvaluation.CurrentDimensionIndex < CurrentEvaluation.NumDimensions - 1)
			{
				CurrentEvaluation.CurrentDimensionIndex++;
				LoadPage();
			}
			else
			{
				DisplayResultsPage();
			}
		}

		private void SaveDimensionData()
		{
			if (CurrentEvaluation.Dimensions.Count == 0)
			{
				Response.Redirect("/eval/timeout.aspx", true);
			}
			else
			{
				Level level = new Level();
				List<Response> currentPageResponses = new List<Response>();

				if (CurrentEvaluation.CurrentDimensionIndex > 0 && CurrentEvaluation.CurrentDimensionIndex < CurrentEvaluation.NumDimensions)
				{
					dimension = CurrentEvaluation.Dimensions[CurrentEvaluation.CurrentDimensionIndex.Value];
					for (int i = 0; i < rptLevels.Items.Count; i++)
					{
						Repeater rptQuestions = rptLevels.Items[i].FindControl("rptQuestions") as Repeater;

						for (int j = 0; j < rptQuestions.Items.Count; j++)
						{
							HtmlInputHidden hfQValue = rptQuestions.Items[j].FindControl("hfQValue") as HtmlInputHidden;
							Response response = new Response();
							response.DimensionID = dimension.ID;

							if (dimension.ID == (int)DimensionEnum.Implementation)
							{

								response.QuestionID = dimension.Levels[i].Questions[j].ID;
								if (hfQValue.Value == "yes")
								{
									response.SelectedValue = ResponseEnum.yes;
									currentPageResponses.Add(response);
								}
								else if (hfQValue.Value == "no")
								{
									response.SelectedValue = ResponseEnum.no;
									currentPageResponses.Add(response);
								}
							}
							else
							{
								response.QuestionID = dimension.Levels[i].Questions[j].ID;
								if (hfQValue.Value == "yes")
								{
									response.SelectedValue = ResponseEnum.yes;
									currentPageResponses.Add(response);
								}
								else if (hfQValue.Value == "no")
								{
									response.SelectedValue = ResponseEnum.no;
									currentPageResponses.Add(response);
								}
							}
						}
					}

					if (currentLevel.Value != "max")
					{
						level = dimension.Levels.Find(i => i.ID == int.Parse(currentLevel.Value));
					}
					else
					{
						level = dimension.Levels.Last();
					}

					if (CurrentEvaluation.CurrentLevels.Find(i => i.DimensionID == dimension.ID) != null)
					{
						CurrentEvaluation.CurrentLevels[CurrentEvaluation.CurrentLevels.FindIndex(i => i.DimensionID == dimension.ID)] = level;
					}
					else
					{
						CurrentEvaluation.CurrentLevels.Add(level);
					}
				}

				// Delete all responses for current dimension and re-save
				CurrentEvaluation.Responses.RemoveAll(i => i.DimensionID == dimension.ID);
				CurrentEvaluation.Responses.AddRange(currentPageResponses);

				SaveDimensionToDB(currentPageResponses, dimension.ID, level.ID);
			}
		}

		protected string DisplayLevelExpanded(object dataItem)
		{
			Domain.Level level = dataItem as Domain.Level;
			if (existingValue)
			{
				if (CurrentEvaluation.CurrentLevels.Find(i => i.DimensionID == dimension.ID) != null)
				{
					if (level.LevelNumber <= CurrentEvaluation.CurrentLevels.Find(i => i.DimensionID == dimension.ID).LevelNumber)
					{
						return "";
					}
				}
			}
			else if (level.LevelNumber == 1)
			{
				return "";
			}
			return " style=\"display:none\"";
		}

		protected string DisplayDefaultValue(object dataItem)
		{
			Domain.Level level = dataItem as Domain.Level;
			if (existingValue)
			{
				if (level.LevelNumber <= CurrentEvaluation.CurrentLevels.Find(i => i.DimensionID == dimension.ID).LevelNumber)
				{
					return "";
				}
			}
			else if (level.LevelNumber == 1)
			{
				return "";
			}
			return " style=\"display:none\"";
		}

		private void DisplayResultsPage()
		{
			HideControl("DimensionControl1");
			ShowControl("ResultsControl1");
			CurrentEvaluation.CurrentDimensionIndex = CurrentEvaluation.NumDimensions;
			LoadControl("ResultsControl", "ResultsControl1");
		}
	}
}
