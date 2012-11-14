using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;

namespace SystemOperationsEvaluation.Domain.Utilities
{
	public class Email
	{
		public static bool SendMail(string from, string to, string cc, string subject, string body, string smtpServer, bool isHTML)
		{
			bool mailSuccess = true;
			MailMessage mailMessage = new MailMessage();
			mailMessage.From = new MailAddress(from);

			if (cc != "")
			{
				mailMessage.CC.Add(cc.Replace(";", ","));
			}

			mailMessage.To.Add(to.Replace(';', ','));
			mailMessage.Subject = subject;
			mailMessage.Body = body;
			mailMessage.IsBodyHtml = isHTML;

			try
			{
				SmtpClient client = new SmtpClient(smtpServer);
				client.Send(mailMessage);
			}
			catch (Exception ex)
			{
				mailSuccess = false;
			}
			finally
			{
				mailMessage = null;
			}

			return mailSuccess;
		}
	}
}
