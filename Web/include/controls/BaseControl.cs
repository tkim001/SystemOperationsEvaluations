using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SystemOperationsEvaluation.Domain;

namespace SystemOperationsEvaluation.Web
{
	public class BaseControl	: System.Web.UI.UserControl
	{
		#region Session Values
		private string revisit = "REVISIT";

		public bool Revisit
		{
			get
			{
				if (Session[revisit] != null)
				{
					return (bool)Session[revisit];
				}
				return false;
			}
			set
			{
				Session[revisit] = value;
			}
		}

		private string review = "REVIEW";

		public bool Review
		{
			get
			{
				if (Session[review] != null)
				{
					return (bool)Session[review];
				}
				return false;
			}
			set
			{
				Session[review] = value;
			}
		}

		public bool IsLoggedIn
		{
			get
			{
				if (CurrentUserID != 0)
				{
					return true;
				}
				return false;
			}
		}

		private string currentUserID = "CURRENT_USER_ID";
		public int CurrentUserID
		{
			get
			{
				if (Session[currentUserID] != null)
				{
					return int.Parse(Session[currentUserID].ToString());
				}
				return 0;
			}
			set
			{
				Session[currentUserID] = value;
			}
		}

		public int CurrentEvaluationID
		{
			get
			{
				return CurrentEvaluation.ID;
			}
		}

		private string currentUser = "CURRENT_USER";

		public User CurrentUser
		{
			get
			{
				if (Session[currentUser] != null)
				{
					return (User)Session[currentUser];
				}
				else
				{
					if (CurrentUserID != 0)
					{
						return Data.User.GetUser(CurrentUserID);
					}
					else
					{
						return new User();
					}
				}
			}
			set
			{
				Session[currentUser] = value;
			}
		}

		private string currentEvaluation = "CURRENT_EVALUATION";

		public Evaluation CurrentEvaluation
		{
			get
			{
				if (Session[currentEvaluation] != null)
				{
					return (Evaluation)Session[currentEvaluation];
				}
				return new Evaluation();
			}
			set
			{
				Session[currentEvaluation] = value;
			}
		}

		#endregion

		public void SaveCurrentEvaluationComplete(bool loggedIn)
		{
			SaveEvaluationToDB(loggedIn);
			if (CurrentEvaluationID != 0)
			{
				SaveResultsToDB(loggedIn);
				foreach (Dimension dimension in CurrentEvaluation.Dimensions)
				{
					Data.Response.AddResponses(CurrentEvaluation.Responses.Where(i => i.DimensionID == dimension.ID).ToList(), CurrentEvaluationID, dimension.ID);
				}
			}
		}
		public void SaveEvaluationToDB(bool loggedIn)
		{
			if (IsLoggedIn || loggedIn)
			{
				if (CurrentEvaluation.ID == 0)
				{
					CurrentEvaluation.ID = Data.Evaluation.AddEvaluation(CurrentEvaluation);
				}
				else
				{
					Data.Evaluation.UpdateEvaluation(CurrentEvaluation);
				}
			}
		}

		public void SaveResultsToDB(bool loggedIn)
		{
			if (IsLoggedIn || loggedIn)
			{
				if (CurrentEvaluationID != 0)
				{
					// Save EvaluationResultID
					SaveEvaluationToDB(loggedIn);
					// Save current levels for each dimension
					Data.EvaluationLevel.AddEvaluationLevel(CurrentEvaluation);
				}
			}
		}

		#region Save
		public void SaveCurrentEvaluationComplete()
		{
			SaveEvaluationToDB();
			if (CurrentEvaluationID != 0)
			{
				SaveResultsToDB();
				foreach (Dimension dimension in CurrentEvaluation.Dimensions)
				{
					Data.Response.AddResponses(CurrentEvaluation.Responses.Where(i => i.DimensionID == dimension.ID).ToList(), CurrentEvaluationID, dimension.ID);
				}
			}
		}

		public void SaveEvaluationToDB()
		{
			if (IsLoggedIn)
			{
				if (CurrentEvaluationID == 0)
				{
					CurrentEvaluation.ID = Data.Evaluation.AddEvaluation(CurrentEvaluation);
				}
				else
				{
					Data.Evaluation.UpdateEvaluation(CurrentEvaluation);
				}
			}
		}
		
		public void SaveDimensionToDB(List<Response> responses, int dimensionID, int levelID)
		{
			if (IsLoggedIn)
			{
				if (CurrentEvaluationID != 0)
				{
					Data.Response.DeleteResponses(CurrentEvaluationID, dimensionID);
					Data.Response.AddResponses(responses, CurrentEvaluationID, dimensionID);
					Data.EvaluationLevel.AddUpdateEvaluationLevel(CurrentEvaluationID, dimensionID, levelID);
				}
			}
		}

		public void SaveResultsToDB()
		{
			if (IsLoggedIn)
			{
				if (CurrentEvaluationID != 0)
				{
					// Save EvaluationResultID
					SaveEvaluationToDB();
					// Save current levels for each dimension
					Data.EvaluationLevel.AddEvaluationLevel(CurrentEvaluation);
				}
			}
		}

		#endregion
		public void ShowControl(string controlID)
		{
			((EvaluationControl)this.Parent).FindControl(controlID).Visible = true;
		}

		public void HideControl(string controlID)
		{
			((EvaluationControl)this.Parent).FindControl(controlID).Visible = false;
		}

		public void LoadPage()
		{
		}

		public void LoadControl(string controlName, string controlID)
		{
			if (controlName == "DimensionControl")
			{
				((DimensionControl)((EvaluationControl)this.Parent).FindControl(controlID)).LoadPage();
			}
			else if (controlName == "ResultsControl")
			{
				((ResultsControl)((EvaluationControl)this.Parent).FindControl(controlID)).LoadPage();
			}
		}
	}
}