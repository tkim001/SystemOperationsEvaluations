<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="results.ascx.cs" Inherits="SystemOperationsEvaluation.Web.ResultsControl" %>
<div id="evaluation">
	<div id="summaryDesc">
		<p>
			<span class="bold">Evaluation Date: </span>
			<asp:Label ID="lblDate" runat="server" /></p>
	</div>
</div>
<script language="javascript" type="text/javascript">
	$(function () {

		$(".aToggle").click(function (event) {
			if ($(this).parents('tbody').next().css("display") == "none") {
				$(this).children("img").attr("src", "/images/bullet_arrow_down.gif");
				$(this).children("img").attr("height", "7");
			}
			else {
				$(this).children("img").attr("src", "/images/bullet_arrow.gif");
				$(this).children("img").attr("height", "6");
			}
			$(this).parents('tbody').next().toggle();
			event.preventDefault();
		});
	});
</script>
