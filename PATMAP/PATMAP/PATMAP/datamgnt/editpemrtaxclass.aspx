<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master"
    Codebehind="editpemrtaxclass.aspx.vb" Inherits="PATMAP.editpemrtaxclass" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register TagPrefix="patmap" TagName="dataTabMenu" Src="~/tabmenu.ascx" %>
<asp:Content ID="tabMenu" ContentPlaceHolderID="tabMenu" runat="server">
    <patmap:dataTabMenu ID="subMenu" runat="server"></patmap:dataTabMenu>
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server">
    <div class="commonContent">
        <div class="commonHeader">
            <div class="PageHeaderModule">
                <div class="Header">
                    <p class="Title">
                        <asp:Label ID="lblTitle" runat="server" Text="Edit PEMR Tax Class"></asp:Label></p>
                </div>
            </div>
        </div>
        <div class="commonForm">
            <div class="btnsTop">
                <asp:ImageButton ID="btnSave" runat="server" ImageUrl="~/images/btnSave.gif" />
                <asp:ImageButton ID="btnCancel" runat="server" ImageUrl="~/images/btnCancel.gif" /></div>
            <br />
            <div class="clear">
            </div>
            <p>
                Fields marked with an asterisk (<span class="requiredField">*</span>) are required.</p>
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td class="label">
                        <asp:Label ID="lblTaxClass" runat="server" Text="PEMR Tax Class"></asp:Label>
                        <span class="requiredField">*</span></td>
                    <td class="field" colspan="2">
                        <asp:TextBox ID="txtTaxClass" runat="server" CssClass="txtNormal"></asp:TextBox>
                        <asp:ImageButton ID="btnHPEMRTaxClass" runat="server" ImageUrl="~/images/btnHelp.gif"
                            CssClass="btnHelp" Visible="False"></asp:ImageButton></td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
