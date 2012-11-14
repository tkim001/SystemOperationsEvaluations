using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using SystemOperationsEvaluation.Domain.Utilities;
using System.Web.Security;

namespace SystemOperationsEvaluation.Web
{
	public partial class LoginControl : BaseControl
	{
		private string loginInfoCookie = ConfigurationManager.AppSettings["loginInfoCookie"];
		private string encryptionKey = ConfigurationManager.AppSettings["encryptionKey"];

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				if (IsLoggedIn)
				{
					loginTitle.Visible = false;
					pnlLogin.Visible = false;
					pnlLoggedIn.Visible = true;
				}
				else 
				{
					PopulateLoginInfo();
					if (!String.IsNullOrEmpty(Request.QueryString["referrer"]))
					{
						string referrer = Request.QueryString["referrer"];
						if (referrer == "/profile/")
						{
							ddlStartingPage.Items.FindByValue("MyProfile").Selected = true;
						}
						else if (referrer.Contains("evaluation"))
						{
							ddlStartingPage.Items.FindByValue("MyEvaluations").Selected = true;
						}
					}
				}
			}
		}
		#region Login
		private void PopulateLoginInfo()
		{
			if (Request.Cookies[loginInfoCookie] != null)
			{
				try
				{
					HttpCookie cookie = (HttpCookie)Request.Cookies[loginInfoCookie];
					txtEmail.Text = cookie["username"];
					txtPassword.Text = EncryptDecrypt.Decrypt(cookie["password"], encryptionKey);
					cbRemember.Checked = true;
				}
				catch
				{
				}
			}
		}

		private void SaveLoginInfo(string username, string password)
		{
			try
			{
				HttpCookie cookie = new HttpCookie(loginInfoCookie);
				cookie["username"] = username;
				cookie["password"] = EncryptDecrypt.Encrypt(password, encryptionKey);
				// This cookie should not expire right away
				cookie.Expires = DateTime.Now.AddYears(1);
				Response.Cookies.Add(cookie);
			}
			catch
			{
			}
		}

		private void ClearLoginInfo()
		{
			try
			{
				HttpCookie cookie = (HttpCookie)Request.Cookies[loginInfoCookie];
				cookie.Expires = DateTime.Now.AddDays(-30);
				Response.Cookies.Add(cookie);
			}
			catch
			{
			}
		}

		protected void btnLogin_OnClick(object sender, EventArgs e)
		{
			// Process contct info
			if (Page.IsValid)
			{
				LoginUser();
			}
		}

		private void LoginUser()
		{
			plIncorrectPassword.Visible = false;

			FormsAuthentication.SignOut();
			FormsAuthentication.Initialize();

			string username = txtEmail.Text;
			string encryptedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(txtPassword.Text, "sha1");
			int userID = Data.User.LoginUser(username, encryptedPassword);
			if (userID != 0)
			{
				try
				{
					CurrentUserID = userID;
					SetUserAuthentication(userID.ToString());

					// Save username/password in a cookie if desired
					if (cbRemember.Checked)
					{
						SaveLoginInfo(username, txtPassword.Text);
					}
					else
					{
						ClearLoginInfo();
					}

					// if there are any evaluations in progress, save it
					if (CurrentEvaluation.Responses.Count > 0 && CurrentEvaluation.UserID == 0)
					{
						CurrentEvaluation.UserID = userID;
						SaveCurrentEvaluationComplete(true);
					}
				}
				catch (Exception ex)
				{
				}
				finally
				{
					plIncorrectPassword.Visible = false;
					string redirectURL = GetRedirectURL();
					Response.Redirect(redirectURL, true);
				}
			}
			else
			{
				plIncorrectPassword.Visible = true;
				txtPassword.Text = "";
			}
		}

		private string GetRedirectURL()
		{
			string redirectURL = "~/profile/evaluations.aspx";
			string returnPage = ddlStartingPage.SelectedValue;

			switch (returnPage)
			{
				case "MyEvaluations": redirectURL = "~/profile/evaluations.aspx";
					break;
				case "MyProfile": redirectURL = "~/profile/";
					break;
				case "NewEvaluation": redirectURL = "~/eval/evaluation.aspx";
					break;
			}

			return redirectURL;
		}

		private void SetUserAuthentication(string userID)
		{
			// Create a new ticket used for authentication
			//FormsAuthentication.HashPasswordForStoringInConfigFile(password, "sha1");
			FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, userID,
			    DateTime.Now,
			    DateTime.Now.AddMinutes(240), // 240 minutes
			    true, // true for persistent user cookie
			    userID);
			string hash = FormsAuthentication.Encrypt(ticket);

			HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash);
			// You must expire the cookie. 
			// Otherwise, the user will never have to authenticate against the database again.
			cookie.Expires = ticket.Expiration;

			// Add the cookie to the list for outgoing response
			Response.Cookies.Add(cookie);
		}

		#endregion
	}
}