<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    Codebehind="loadschool.aspx.vb" Inherits="PATMAP.loadschool" %>

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
                        School Division</p>
                </div>
            </div>
        </div>
        <div class="commonForm">
            <table cellpadding="0" cellspacing="0" border="0">
               <%-- <tr>
                    <td colspan="3">
                        <asp:Label ID="lb_Message" runat="server" Width="100%" ForeColor="#669966"></asp:Label></td>
                </tr>--%>
                <tr>
                    <td class="label">
                        <asp:Label ID="lblFile" runat="server" Text="File"></asp:Label></td>
                    <td class="field" style="width: 224px">
                        <asp:FileUpload ID="fpFile" runat="server" Width="200px" /><asp:ImageButton ID="btnHFile"
                            runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp">
                        </asp:ImageButton></td>
                    <td class="field">
                        <asp:ImageButton ID="btnFileUpload" runat="server" ImageUrl="~/images/btnLoad.gif"
                            OnClick="btnFileUpload_Click" />
                    </td>
                </tr>
                <tr id="tr_ddlTableSchKey" runat="server" visible="false">
                    <td class="label">
                        <asp:Label ID="lb_ddlTableSchKey" runat="server" Text="Sheet: SchKey"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:DropDownList ID="ddlTableSchKey" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTableNames_SelectedIndexChanged" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr id="tr_btnLoad" runat="server" visible="false">
                    <td>
                    </td>
                    <td class="btns" style="height: 25px" colspan="2">
                        <asp:ImageButton ID="btnLoad" runat="server" ImageUrl="~/images/btnLoad.gif" OnClientClick="return confirmPrompt('Do you want to load school data?');"/>
                        <%--OnClick="btnLoad_Click" />--%>
                    </td>
                </tr>
            </table>
            <rsweb:ReportViewer ID="rpvReports" runat="server">
            </rsweb:ReportViewer>
        </div>
    </div>
</asp:Content>
