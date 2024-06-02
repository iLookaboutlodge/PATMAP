<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="editkog.aspx.vb" Inherits="PATMAP.editkog" %>
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
                            <asp:Label ID="lblTitle" runat="server" Text="Edit K-12 OG"></asp:Label></p>
                    </div>
                </div>
            </div>
            <div class="commonForm">
                <div class="btnsTop">
                    <asp:ImageButton ID="btnSave" runat="server" ImageUrl="~/images/btnSave.gif" OnClick="btnSave_Click" />
                    <asp:ImageButton ID="btnCancel" runat="server" ImageUrl="~/images/btnCancel.gif" OnClick="btnCancel_Click" /></div>
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
                            <asp:DropDownList ID="ddlDSN" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlDSN_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:ImageButton ID="btnHDSN" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False"
                                CssClass="btnHelp"></asp:ImageButton></td>
                    </tr>
                    <tr>
                        <td class="label">
                            <asp:Label ID="lblSchoolDivision" runat="server" Text="School Division"></asp:Label>
                            <span class="requiredField">*</span></td>
                        <td class="field" colspan="2">
                            <asp:DropDownList ID="ddlSchoolDivision" runat="server">
                            </asp:DropDownList>
                            <asp:ImageButton ID="btnHSchoolDivision" runat="server" ImageUrl="~/images/btnHelp.gif"
                                Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                    </tr>
                    <tr>
                        <td class="label">
                            <asp:Label ID="lbldivisionName" runat="server" Text="Division Name"></asp:Label></td>
                        <td class="field" colspan="2">
                            <asp:TextBox ID="txtdivisionName" CssClass="txtArea" runat="server" MaxLength="100"></asp:TextBox>
                            <asp:ImageButton ID="btnHdivisionName" runat="server" ImageUrl="~/images/btnHelp.gif"
                                Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                    </tr>
                    <tr>
                        <td class="label">
                            <asp:Label ID="lbldivisionType" runat="server" Text="Division Type"></asp:Label></td>
                        <td class="field" colspan="2">
                            <asp:TextBox ID="txtdivisionType" CssClass="txtArea" runat="server" MaxLength="50"></asp:TextBox>
                            <asp:ImageButton ID="btnHdivisionType" runat="server" ImageUrl="~/images/btnHelp.gif"
                                Visible="False" CssClass="btnHelp"></asp:ImageButton>
                        </td>
                    </tr>

                    <tr>
                        <td class="label">
                            <asp:Label ID="lbltotalRecogExp" runat="server" Text="Total Recog Exp"></asp:Label></td>
                        <td class="field" colspan="2">
                            <asp:TextBox ID="txttotalRecogExp" runat="server"></asp:TextBox>
                            <asp:ImageButton ID="btnHtotalRecogExp" runat="server" ImageUrl="~/images/btnHelp.gif"
                                Visible="False" CssClass="btnHelp"></asp:ImageButton>
                            <asp:RangeValidator ID="RangeValidator8" ControlToValidate="txttotalRecogExp" Type="Double"
                                MinimumValue="0" MaximumValue="9999999999" Text="Only Float type" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            <asp:Label ID="lblassessment" runat="server" Text="Assessment"></asp:Label></td>
                        <td class="field" colspan="2">
                           <asp:TextBox ID="txtassessment" runat="server" />
                            <asp:ImageButton ID="btnHassessment" runat="server" ImageUrl="~/images/btnHelp.gif"
                                Visible="False" CssClass="btnHelp"></asp:ImageButton>
                            <asp:RangeValidator ID="RangeValidator10" ControlToValidate="txtassessment" Type="Double"
                                MinimumValue="0" MaximumValue="9999999999" Text="Only Float type" runat="server" />
                                </td>
                    </tr>
                    <tr>
                        <td class="label">
                            <asp:Label ID="lblderivedGILAssessment" runat="server" Text="Derived GIL Assessment"></asp:Label></td>
                        <td class="field" colspan="2">
                            <asp:TextBox ID="txtderivedGILAssessment" runat="server"></asp:TextBox>
                            <asp:ImageButton ID="btnHderivedGILAssessment" runat="server" ImageUrl="~/images/btnHelp.gif"
                                Visible="False" CssClass="btnHelp"></asp:ImageButton>
                            <asp:RangeValidator ID="RangeValidator4" ControlToValidate="txtderivedGILAssessment" Type="Double"
                                MinimumValue="0" MaximumValue="9999999999" Text="Only Float type" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            <asp:Label ID="lbltotalAssessment" runat="server" Text="Total Assessment"></asp:Label></td>
                        <td class="field" colspan="2">
                            <asp:TextBox ID="txttotalAssessment" runat="server"></asp:TextBox>
                            <asp:ImageButton ID="btnHtotalAssessment" runat="server" ImageUrl="~/images/btnHelp.gif"
                                Visible="False" CssClass="btnHelp"></asp:ImageButton>
                            <asp:RangeValidator ID="RangeValidator1" ControlToValidate="txttotalAssessment" Type="Double"
                                MinimumValue="0" MaximumValue="9999999999" Text="Only Float type" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            <asp:Label ID="lblEQFactor" runat="server" Text="EQ Factor"></asp:Label></td>
                        <td class="field" colspan="2">
                            <asp:TextBox ID="txtEQFactor" runat="server"></asp:TextBox>
                            <asp:ImageButton ID="btnHEQFactor" runat="server" ImageUrl="~/images/btnHelp.gif"
                                Visible="False" CssClass="btnHelp"></asp:ImageButton>
                            <asp:RangeValidator ID="RangeValidator2" ControlToValidate="txtEQFactor" Type="Double"
                                MinimumValue="0" MaximumValue="9999999999" Text="Only Float type" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            <asp:Label ID="lbllocationRevenue" runat="server" Text="Location Revenue"></asp:Label></td>
                        <td class="field" colspan="2">
                            <asp:TextBox ID="txtlocationRevenue" runat="server"></asp:TextBox>
                            <asp:ImageButton ID="btnHlocationRevenue" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False"
                                CssClass="btnHelp"></asp:ImageButton>
                            <asp:RangeValidator ID="RangeValidator3" ControlToValidate="txtlocationRevenue" Type="Double"
                                MinimumValue="0" MaximumValue="9999999999" Text="Only Float type" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            <asp:Label ID="lblotherRevenue" runat="server" Text="Other Revenue"></asp:Label></td>
                        <td class="field" colspan="2">
                            <asp:TextBox ID="txtotherRevenue" runat="server"></asp:TextBox>
                            <asp:ImageButton ID="btnHotherRevenue" runat="server" ImageUrl="~/images/btnHelp.gif"
                                Visible="False" CssClass="btnHelp"></asp:ImageButton>
                            <asp:RangeValidator ID="RangeValidator5" ControlToValidate="txtotherRevenue" Type="Double"
                                MinimumValue="0" MaximumValue="9999999999" Text="Only Float type" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            <asp:Label ID="lbltotalRevenue" runat="server" Text="Total Revenue"></asp:Label></td>
                        <td class="field" colspan="2">
                            <asp:TextBox ID="txttotalRevenue" runat="server"></asp:TextBox>
                            <asp:ImageButton ID="btnHSection33" runat="server" ImageUrl="~/images/btnHelp.gif"
                                Visible="False" CssClass="btnHelp"></asp:ImageButton>
                            <asp:RangeValidator ID="RangeValidator6" ControlToValidate="txttotalRevenue" Type="Double"
                                MinimumValue="0" MaximumValue="9999999999" Text="Only Float type" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            <asp:Label ID="lbltotalGrant" runat="server" Text="Total Grant"></asp:Label></td>
                        <td class="field" colspan="2">
                            <asp:TextBox ID="txttotalGrant" runat="server"></asp:TextBox>
                            <asp:ImageButton ID="btnHtotalGrant" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False"
                                CssClass="btnHelp"></asp:ImageButton>
                            <asp:RangeValidator ID="RangeValidator7" ControlToValidate="txttotalGrant" Type="Double"
                                MinimumValue="0" MaximumValue="9999999999" Text="Only Float type" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            <asp:Label ID="lblgrantEntitlement" runat="server" Text="Grant Entitlement"></asp:Label></td>
                        <td class="field" colspan="2">
                            <asp:TextBox ID="txtgrantEntitlement" runat="server"></asp:TextBox>
                            <asp:ImageButton ID="btnHgrantEntitlement" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False"
                                CssClass="btnHelp"></asp:ImageButton>
                            <asp:RangeValidator ID="RangeValidator9" ControlToValidate="txtgrantEntitlement" Type="Double"
                                MinimumValue="0" MaximumValue="9999999999" Text="Only Float type" runat="server" />
                        </td>
                    </tr>
                    
                </table>
            </div>
        </div>
</asp:Content>


