<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" ValidateRequest="false"
    Codebehind="loadpotash.aspx.vb" Inherits="PATMAP.loadpotash" %>

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
                        Potash</p>
                </div>
            </div>
        </div>
        <div class="commonForm">
            <p>
                All fields are required.</p>
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
                <tr id="tr_ddlTableAreas" runat="server" visible="false">
                    <td class="label">
                        <asp:Label ID="Label1" runat="server" Text="Areas Sheet Name"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:DropDownList ID="ddlTableAreas" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTables_SelectedIndexChanged" />
                    </td>
                </tr>
                <tr id="tr_ddlTableParcels" runat="server" visible="false">
                    <td class="label">
                        <asp:Label ID="Label2" runat="server" Text="Parcels Sheet Name"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:DropDownList ID="ddlTableParcels" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTables_SelectedIndexChanged" />
                    </td>
                </tr>
                <tr id="tr_ddlTableRural" runat="server" visible="false">
                    <td class="label">
                        <asp:Label ID="Label3" runat="server" Text="Rural Sheet Name"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:DropDownList ID="ddlTableRural" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTables_SelectedIndexChanged" />
                    </td>
                </tr>
                <tr id="tr_ddlTableUrban" runat="server" visible="false">
                    <td class="label">
                        <asp:Label ID="Label4" runat="server" Text="Urban Sheet Name"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:DropDownList ID="ddlTableUrban" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTables_SelectedIndexChanged" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr id="tr_DataSet" runat="server" visible="false">
                    <td class="label">
                        <asp:Label ID="lblDataSet" runat="server" Text="Data Set Name"></asp:Label></td>
                    <td class="field" colspan="2">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td>
                                    <asp:Label ID="lblExisting" runat="server" Text="Existing"></asp:Label></td>
                                <td>
                                    <asp:DropDownList ID="ddlDSN" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlDSN_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:ImageButton ID="btnHDSN" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False"
                                        CssClass="btnHelp"></asp:ImageButton>
                                </td>
                            </tr>
                            <tr id="tr_txtNewDSN" runat="server">
                                <td align="center">
                                    <asp:Label ID="lblOr" runat="server" Text="Or New"></asp:Label></td>
                                <td>
                                    <asp:TextBox ID="txtNewDSN" runat="server" CssClass="txtLong" AutoPostBack="True"
                                        OnTextChanged="txtNewDSN_TextChanged"></asp:TextBox>
                                    <asp:ImageButton ID="btnHNewDSN" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False"
                                        CssClass="btnHelp"></asp:ImageButton>
                                </td>
                            </tr>
                            <tr id="tr_Year" runat="server">
                                <td class="label">
                                    <asp:Label ID="lblYear" runat="server" Text="Year"></asp:Label>
                                    <span class="requiredField">*</span></td>
                                <td class="field">
                                    <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:ImageButton ID="btnHYear" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False"
                                        CssClass="btnHelp"></asp:ImageButton></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="tr_btnLoad" runat="server" visible="false">
                    <td>
                    </td>
                    <td class="btns" style="height: 25px" colspan="2">
                        <asp:ImageButton ID="btnLoad" runat="server" ImageUrl="~/images/btnLoad.gif" OnClientClick="return confirmPrompt('Do you want to load potash data?');" />
                        <%--OnClick="btnLoad_Click" />--%>
                    </td>
                </tr>
            </table>
            <rsweb:ReportViewer ID="rpvReports" runat="server">
            </rsweb:ReportViewer>
        </div>
    </div>
</asp:Content>
