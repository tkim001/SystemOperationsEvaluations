<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="login.ascx.cs" Inherits="SystemOperationsEvaluation.Web.LoginControl" %>
<div class="evaluation2"><h1 runat="server" id="loginTitle">
					Login</h1>
	<asp:Panel ID="plIncorrectPassword" runat="server" Visible="false">
		<div class="confirmation">
			<img src="/images/error.png" width="16" height="16" alt="Confirm" />
			<span class="error">You did not enter the correct email address and/or password.</span>
			<p>
				If you are a new user and have not registered, please <a href="/profile/register.aspx">
					register</a>.
				<br />
				If you have forgotten your password, please <a href="/profile/forgot_password.aspx">request
					a new password</a>.</p>
		</div>
	</asp:Panel>
	<asp:Panel ID="pnlLogin" runat="server">
		<table class="login2">
			<tr>
				<td colspan="2">
		<p class="bold">Log in to save a session in progress, update a session in progress, or save a completed session.</p>
				</td>
			</tr>
			<tr>
				<td width="24%">
					Email Address:
				</td>
				<td>
					<asp:TextBox ID="txtEmail" runat="server" MaxLength="50" Width="250px" /><asp:RequiredFieldValidator
						EnableClientScript="false" ErrorMessage=" Required." ID="emailValidator" ControlToValidate="txtEmail"
						Display="Dynamic" runat="server" ValidationGroup="VGLogin" />
				</td>
			</tr>
			<tr>
				<td>
					Password:
				</td>
				<td>
					<SystemOperationsEvaluationUtils:PasswordTextBox ID="txtPassword" TextMode="Password" MaxLength="50"
						runat="server" Width="250px" />
					<asp:RequiredFieldValidator EnableClientScript="false" ErrorMessage="Required." ID="passwordValidator"
						ControlToValidate="txtPassword" Display="Dynamic" runat="server" ValidationGroup="VGLogin" />
				</td>
			</tr>
			<tr>
				<td>
					Starting Page:
				</td>
				<td>
					<asp:DropDownList ID="ddlStartingPage" runat="server">
						<asp:ListItem Text="My Evaluations" Value="MyEvaluations" />
						<asp:ListItem Text="My Profile" Value="MyProfile" />
						<asp:ListItem Text="New Evaluation" Value="NewEvaluation" />
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td height="10px">
				</td>
			</tr>
			<tr>
				<td>
				</td>
				<td>
					<asp:CheckBox ID="cbRemember" runat="server" Text="Remember Login Information" />
				</td>
			</tr>
			<tr>
				<td>
					&nbsp;
				</td>
			</tr>
			<tr>
				<td>
					&nbsp;
				</td>
				<td>
					<div class="left">
						<ul class="button2">
							<li>
								<asp:LinkButton ID="Button1" runat="server" ValidationGroup="VGLogin" OnClick="btnLogin_OnClick"><span><strong>&gt; Submit</strong></span></asp:LinkButton></li>
						</ul>
					</div>
					<div class="left padTop5">
						<a href="/profile/forgot_password.aspx">Forgot Your Password?</a>
					</div>
				</td>
			</tr>
			<tr>
				<td>
					&nbsp;
				</td>
			</tr>
			<tr>
				<td colspan="2">
					<p>
						<b>New user?</b> <a href="/profile/register.aspx">Register Now</a> to save your sessions
						and evaluation history.</p>
				</td>
			</tr>
			<tr>
				<td colspan="2">
					<p>
						Not interested in creating an account right now? <a href="/eval/evaluation.aspx">
							Begin Your Evaluation</a>. You can register/login when the evaluation is complete.</p>
				</td>
			</tr>
			<tr>
				<td colspan="2" align="center">
					<div class="button2center">
						<ul class="button2">
							<li><a href="/eval/evaluation.aspx?new=1"><span><strong>&gt; Begin Evaluation</strong></span></a></li>
						</ul>
					</div>
				</td>
			</tr>
		</table>
	</asp:Panel>
	<asp:Panel ID="pnlLoggedIn" runat="server" Visible="false">
		<div class="button" style="width: 95%">
			<ul class="button1">
				<li><a href="/eval/evaluation.aspx"><span><strong>&gt; Begin Evaluation</strong></span></a></li>
			</ul>
		</div>
	</asp:Panel>
</div>
