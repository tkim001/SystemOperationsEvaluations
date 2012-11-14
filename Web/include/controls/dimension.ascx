<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="dimension.ascx.cs" Inherits="SystemOperationsEvaluation.Web.DimensionControl" %>
<script type="text/javascript">
	$(document).ready(function () {
		$(".level_container:first").find("input.qValue").each(function () {
			if ($(this).val() == 'default') {
				$(this).val('');
			}
		});
		validatePage();
	});
</script>
<div id="evaluation">
	<asp:Repeater ID="rptLevels" runat="server">
		<ItemTemplate>
			<div class="level_container" <%# DisplayLevelExpanded(Container.DataItem)%> id="level<%#(((RepeaterItem)Container).ItemIndex+1).ToString() %>">
				<input type="hidden" id="levelNo" class="levelNo" runat="server" value='<%# Eval("ID") %>' />
				<table class="questions" width="93%">
					<asp:Repeater ID="rptQuestions" runat="server" DataSource='<%# DisplayQuestions(Container.DataItem) %>'
						OnItemDataBound="rptQuestions_OnItemDataBound">
						<ItemTemplate>
							<tr>
								<td width="4%" class="center">
									<%# DisplayQuestionNum() %>
								</td>
								<td width="70%">
									<%# Eval("Description") %>
									<input type="hidden" id="hfQValue" runat="server" value="default" class="qValue" />
								</td>
								<td width="11%" class="yes" title="Yes" id="tdYes" runat="server">
									Yes
								</td>
								<td width="11%" class="no" title="No" id="tdNo" runat="server">
									No
								</td>
							</tr>
						</ItemTemplate>
					</asp:Repeater>
				</table>
			</div>
		</ItemTemplate>
	</asp:Repeater>
	<div class="footer">
		<table border="0" class="footerNav">
			<tr>
				<td width="30%">
					<asp:LinkButton ID="lbBack" runat="server" OnClick="lbBack_OnClick" CssClass="back"
						Width="67px" ToolTip="&lt; Back"></asp:LinkButton>
				</td>
				<td width="60%" align="right">
					<div id="validateMessage">
						Please answer all questions on the page to proceed.</div>
				</td>
				<td width="15%" class="buttonContainerRight">
					<asp:LinkButton ID="lbNext" runat="server" OnClick="lbNext_OnClick" CssClass="nextInactive"
						OnClientClick="return submitForm()" Width="67px" ToolTip="Please answer all questions on the page to proceed."></asp:LinkButton>
				</td>
			</tr>
		</table>
	</div>
</div>
<input type="hidden" id="currentLevel" class="currentLevel" runat="server" value="0" />
<input type="hidden" id="hfExistingValue" runat="server" value="0" />
<script language="javascript" type="text/javascript">

	function submitForm() {
		if ($("#<%= lbNext.ClientID %>").attr("class") == "next") {
			return true;
		}
		return false;
	}

	$(function () {

		$(".yes").live('click', function (event) {
			var $item = $(this);
			var $no_item = $item.next();
			var currentDivID = '#' + $(this).parents('.level_container').attr('id');
			if (currentDivID != '#level3') {
				$no_item.attr('class', 'noInactive');  // disable the corresponding no item
			}
			$item.attr('class', 'answeredYes');
			$item.parent().find('.qValue').val('yes');
			if (levelComplete(currentDivID)) {
				if (currentDivID != '#level3') {
					$(currentDivID).next('.level_container').slideDown(1000, function () {
						$no_item.attr('class', 'no');
					});  // re-enable the no field
				}

				$(currentDivID).next('.level_container').find('.qValue').val('');
				var levelNo = $(currentDivID).next('.level_container').find('.levelNo').val();
				if (levelNo != undefined) {
					$('.currentLevel').val(levelNo);
				}
				else {
					$('.currentLevel').val('max');
				}

				if (currentDivID == '#level3') {
					$no_item.attr('class', 'no');
				}
			}
			else {
				if (currentDivID != '#level3') {
					$no_item.attr('class', 'no'); //re-enable the no item
				}
			}

			validatePage();
		});
	});
	$(function () {
		$(".no").live('click', function (event) {
			var $item = $(this);
			$item.parent().find('.qValue').val('no');
			$item.attr('class', 'answeredNo');
			$item.prev().attr('class', 'yes');

			$item.parents('.level_container').nextAll('.level_container').each(function (i) {
				$(this).find('.qValue').val('default');
				$(this).find('.answeredYes').attr('class', 'yes');
				$(this).find('.answeredNo').attr('class', 'no');
				$(this).hide();
			});

			var levelNo = $(this).parents('.level_container').find('.levelNo').val();
			$('.currentLevel').val(levelNo);

			validatePage();
		});
	});

	function levelComplete(currentDivID) {
		var allAnsweredYes = true;

		$(currentDivID).find('.qValue').each(function () {
			if ($(this).val() != 'yes') {
				allAnsweredYes = false;
				return allAnsweredYes;
			}
		});
		return allAnsweredYes;
	}

	function activateNextButton() {
		$("#<%= lbNext.ClientID %>").attr('title', 'Next').attr('class', 'next').removeAttr("disabled");
		$("#validateMessage").html('&nbsp;');
	}

	function deactivateNextButton() {
		$("#<%= lbNext.ClientID %>").attr('class', 'nextInactive').attr("disabled", "disabled").attr('title', 'Please answer all questions on the page to proceed.');
		$("#validateMessage").html('Please answer all questions on the page to proceed.');
	}

	function validatePage() {
		if (questionsSelected()) {
			activateNextButton();
		}
		else {
			deactivateNextButton();
		}
		return false;
	}

	function questionsSelected() {
		var allAnswered = true;

		$('.qValue').each(function () {
			if ($(this).val() == '') {
				allAnswered = false;
				return allAnswered;
			}
		});
		return allAnswered;
	}
</script>
