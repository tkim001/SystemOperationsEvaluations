using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SystemOperationsEvaluation.Domain;

namespace SystemOperationsEvaluation.Web
{
	public partial class EvaluationControl : BaseControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				if (!String.IsNullOrEmpty(Request.QueryString["new"]))
				{
					CurrentEvaluation = null;
					Response.Redirect("/eval/evaluation.aspx");
				}

				if (Review)
				{
					CurrentEvaluation.CurrentDimensionIndex = 0;
					Review = false;
				}
			}
		}
	}
}