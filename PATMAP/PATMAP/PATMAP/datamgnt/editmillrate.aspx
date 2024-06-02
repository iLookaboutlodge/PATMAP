<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="editmillrate.aspx.vb" Inherits="PATMAP.editmillrate" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register TagPrefix="patmap" TagName="dataTabMenu" Src="~/tabmenu.ascx" %>
<asp:Content ID="tabMenu" ContentPlaceHolderID="tabMenu" runat="server">   
    <patmap:dataTabMenu id="subMenu" runat="server"></patmap:dataTabMenu>    
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server">       
        <div class="commonContent">
            <div class="commonHeader">
                <div class="PageHeaderModule">
                    <div class="Header">
                        <p class="Title">
                            <asp:Label ID="lblTitle" runat="server" Text="Edit Mill Rate Survey"></asp:Label></p>
                    </div>
                </div>
            </div>
            <div class="commonForm">
                <div class="btnsTop">
                    <asp:ImageButton ID="btnSave" runat="server" ImageUrl="~/images/btnSave.gif" OnClick="btnSave_Click" />
                    <asp:ImageButton ID="btnCancel" runat="server" ImageUrl="~/images/btnCancel.gif"
                        OnClick="btnCancel_Click" /></div>
                <br />
                <div class="clear">
                </div>
                <p>
                    Fields marked with an asterisk (<span class="requiredField">*</span>) are required.</p>
                <table cellpadding="0" cellspacing="0" border="0">
                    <%--<tr>
                        <td colspan="2">
                            <asp:Label ID="lb_Message" runat="server" Width="100%" ForeColor="#669966"></asp:Label></td>
                    </tr>--%>
                    <tr id="tr_DSN" runat="server">
                        <td class="label">
                            <asp:Label ID="lblDSN" runat="server" Text="Data Set Name"></asp:Label>
                            <span class="requiredField">*</span></td>
                        <td class="field" colspan="2">
                            <asp:DropDownList ID="ddlDSN" runat="server" AutoPostBack="True">
                            </asp:DropDownList>
                            <asp:ImageButton ID="btnHDSN" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False"
                                CssClass="btnHelp"></asp:ImageButton></td>
                    </tr>
                    <%-- <tr>
                <td class="label">
                    <asp:Label ID="lblYear" runat="server" Text="Year"></asp:Label>
                    <span class="requiredField">*</span></td>
                <td class="field">
                    <asp:DropDownList ID="ddlYear" runat="server">
                    </asp:DropDownList>
                    <asp:ImageButton ID="btnHYear" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False"
                        CssClass="btnHelp"></asp:ImageButton></td>
            </tr>
            <tr>
                <td class="label">
                    <asp:Label ID="lblJurisdiction" runat="server" Text="Jurisdiction"></asp:Label>
                    <span class="requiredField">*</span></td>
                <td class="field" colspan="2">
                    <asp:DropDownList ID="ddlJurisdiction" runat="server">
                    </asp:DropDownList>
                    <asp:ImageButton ID="btnHJurisdiction" runat="server" ImageUrl="~/images/btnHelp.gif"
                        Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
            </tr>--%>
                    <tr>
                        <td class="label">
                            <asp:Label ID="lblMunicipalityID" runat="server" Text="Municipality ID"></asp:Label></td>
                        <td class="field" colspan="2">
                            <asp:DropDownList ID="ddlMunicipalityID" runat="server" />
                            <asp:ImageButton ID="btnHMunicipalityID" runat="server" ImageUrl="~/images/btnHelp.gif"
                                Visible="False" CssClass="btnHelp"></asp:ImageButton>
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            <asp:Label ID="lblRevenue" runat="server" Text="Revenue"></asp:Label>
                            <span class="requiredField">*</span></td>
                        <td class="field" colspan="2">
                            <asp:TextBox ID="txtRevenue" runat="server"></asp:TextBox>
                            <asp:ImageButton ID="btnHRevenue" runat="server" ImageUrl="~/images/btnHelp.gif"
                                Visible="False" CssClass="btnHelp"></asp:ImageButton>
                            <asp:RangeValidator ID="RangeValidator2" ControlToValidate="txtRevenue" Type="Double"
                                MinimumValue="0" MaximumValue="9999999999" Text="Only Float type" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
</asp:Content>
