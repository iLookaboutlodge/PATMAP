<%@ Control Language="vb" AutoEventWireup="false" Codebehind="header.ascx.vb" Inherits="PATMAP.header" %>
<table cellpadding="0" cellspacing="0" border="0" width="92%">
    <tr>
        <td colspan="3" style="padding-left: 10px; padding-bottom: 15px;">
            <p>
                Fields marked with an asterisk (<span class="requiredField">*</span>) are required.</p>
        </td>
    </tr>
    <tr>
        <td class="label" style="padding-left: 10px; width: 100px;">
            <asp:Label ID="lblScenarioName" runat="server" Text="Scenario Name:"></asp:Label>
            <span class="requiredField">*</span>
        </td>
        <td class="field">
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td>
                        <asp:UpdatePanel ID="uplName" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtScenarioName" runat="server" CssClass="txtLong" OnTextChanged="txtScenarioName_TextChanged"
                                    AutoPostBack="True"></asp:TextBox>
                                <asp:ImageButton ID="ibScenarioHelp" runat="server" ImageUrl="~/images/btnHelp.gif"
                                    Visible="False" CssClass="btnHelp" CommandArgument="330"></asp:ImageButton>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </td>
        <td align="right" style="width: 58px;">
            <asp:ImageButton ID="ibSave" runat="server" ImageUrl="~/images/btnSave.gif" />
        </td>
    </tr>
</table>
<div class="commonHeader">
    <div class="PageHeaderModule">
        <div class="Header">
            <p class="Title">
                <asp:Label ID="lblTitle" runat="server"></asp:Label>
            </p>
        </div>
    </div>
</div>
<div class="headerForm">
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td class="labelBase">
                <asp:Label ID="lblBase" runat="server" Text="Base Tax Year Model:" CssClass="labelTaxModel"></asp:Label>
            </td>
            <td style="width: 30%; padding-left: 10px;">
                <asp:Label ID="txtBaseTaxYrModel" runat="server"></asp:Label>
            </td>
            <td class="labelSubject">
                <asp:Label ID="lblSubject" runat="server" Text="Subject Tax Year Model:" CssClass="labelTaxModel"></asp:Label>
            </td>
            <td style="width: 30%; padding-left: 10px;">
                <asp:Label ID="txtSubjectTaxYrModel" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
</div>
