using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SystemOperationsEvaluation.Domain;

namespace SystemOperationsEvaluation.Web
{
	public class BasePage : System.Web.UI.Page
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
	}
}