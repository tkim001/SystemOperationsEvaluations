using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Net.Mail;
using System.Text;

namespace SystemOperationsEvaluation.Web
{
    public class global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }   

        protected void Application_Error(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Error in: " + Request.Path + "\nUrl: " + Request.RawUrl + "\n\n");

            // Get the exception object for the last error message that occured.
            Exception errorInfo = Server.GetLastError().GetBaseException();
            sb.Append("Error Message: " + errorInfo.Message +
                "\nError Source: " + errorInfo.Source +
                "\nError Target Site: " + errorInfo.TargetSite +
                "\nError To String: " + errorInfo.ToString() +
                "\n\nQueryString Data:\n-----------------\n");

            // Gathering QueryString information
            for (int i = 0; i < HttpContext.Current.Request.QueryString.Count; i++)
            {
                sb.Append(HttpContext.Current.Request.QueryString.Keys[i] + ":\t\t" + HttpContext.Current.Request.QueryString[i] + "\n");
            }
            sb.Append("\nPost Data:\n----------\n");

            // Gathering Post Data information
            for (int i = 0; i < HttpContext.Current.Request.Form.Count; i++)
            {
                sb.Append(HttpContext.Current.Request.Form.Keys[i] + ":\t\t" + HttpContext.Current.Request.Form[i] + "\n");
            }
            sb.Append("\nException Stack Trace:\n----------------------\n" + Server.GetLastError().StackTrace +
                "\n\nServer Variables:\n-----------------\n");

            // Gathering Server Variables information
            for (int i = 0; i < HttpContext.Current.Request.ServerVariables.Count; i++)
            {
                sb.Append(HttpContext.Current.Request.ServerVariables.Keys[i] + ":\t\t" + HttpContext.Current.Request.ServerVariables[i] + "\n");
            }

            // Sending error message to administration via e-mail
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}