<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="loadkog.aspx.vb" Inherits="PATMAP.loadkog" %>
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
                    <p class="Title">K-12 Operating Grant</p>
                </div>
            </div>
        </div>
        <div class="commonForm">
            <%--<p>All fields are required.</p>--%>
            <table cellpadding="0" cellspacing="0" border="0">
                <%--<tr>
                    <td colspan="3">
                        <asp:Label ID="lb_Message" runat="server" Width="100%" ForeColor="#669966"></asp:Label></td>
                </tr>--%>
                <tr id="tr_newFile" runat="server">
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
                <tr id="tr_ddlTableNames" runat="server" visible="false">
                    <td class="label">
                        <asp:Label ID="Label1" runat="server" Text="Select Table Name"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:DropDownList ID="ddlTableNames" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTableNames_SelectedIndexChanged" />
                    </td>
                </tr>
                <tr id="tr_ddldivisionNumber" runat="server" visible="false">
                    <td class="label">
                        <asp:Label ID="lb_ddldivisionNumber" runat="server" Text="Field: divisionNumber"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:DropDownList ID="ddldivisionNumber" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTableColumns_SelectedIndexChanged" />
                    </td>
                </tr>
                <tr id="tr_ddldivisionName" runat="server" visible="false">
                    <td class="label">
                        <asp:Label ID="lb_ddldivisionName" runat="server" Text="Field: divisionName"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:DropDownList ID="ddldivisionName" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTableColumns_SelectedIndexChanged" />
                    </td>
                </tr>
                <tr id="tr_ddldivisionType" runat="server" visible="false">
                    <td class="label">
                        <asp:Label ID="lb_ddldivisionType" runat="server" Text="Field: divisionType"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:DropDownList ID="ddldivisionType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTableColumns_SelectedIndexChanged" />
                    </td>
                </tr>
                <tr id="tr_ddltotalRecogExp" runat="server" visible="false">
                    <td class="label">
                        <asp:Label ID="lb_ddltotalRecogExp" runat="server" Text="Field: totalRecogExp"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:DropDownList ID="ddltotalRecogExp" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTableColumns_SelectedIndexChanged" />
                    </td>
                </tr>
                <tr id="tr_ddlassessment" runat="server" visible="false">
                    <td class="label">
                        <asp:Label ID="lb_ddlassessment" runat="server" Text="Field: assessment"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:DropDownList ID="ddlassessment" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTableColumns_SelectedIndexChanged" />
                    </td>
                </tr>
                <tr id="tr_ddlderivedGILAssessment" runat="server" visible="false">
                    <td class="label">
                        <asp:Label ID="lb_ddlderivedGILAssessment" runat="server" Text="Field: derivedGILAssessment"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:DropDownList ID="ddlderivedGILAssessment" runat="server" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlTableColumns_SelectedIndexChanged" />
                    </td>
                </tr>
                <tr id="tr_ddltotalAssessment" runat="server" visible="false">
                    <td class="label">
                        <asp:Label ID="lb_ddltotalAssessment" runat="server" Text="Field: totalAssessment"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:DropDownList ID="ddltotalAssessment" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTableColumns_SelectedIndexChanged" />
                    </td>
                </tr>
                <tr id="tr_ddlEQFactor" runat="server" visible="false">
                    <td class="label">
                        <asp:Label ID="lb_ddlEQFactor" runat="server" Text="Field: EQFactor"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:DropDownList ID="ddlEQFactor" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTableColumns_SelectedIndexChanged" />
                    </td>
                </tr>
                <tr id="tr_ddllocationRevenue" runat="server" visible="false">
                    <td class="label">
                        <asp:Label ID="lb_ddllocationRevenue" runat="server" Text="Field: locationRevenue"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:DropDownList ID="ddllocationRevenue" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTableColumns_SelectedIndexChanged" />
                    </td>
                </tr>
                <tr id="tr_ddlotherRevenue" runat="server" visible="false">
                    <td class="label">
                        <asp:Label ID="lb_ddlotherRevenue" runat="server" Text="Field: otherRevenue"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:DropDownList ID="ddlotherRevenue" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTableColumns_SelectedIndexChanged" />
                    </td>
                </tr>
                <tr id="tr_ddltotalRevenue" runat="server" visible="false">
                    <td class="label">
                        <asp:Label ID="lb_ddltotalRevenue" runat="server" Text="Field: totalRevenue"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:DropDownList ID="ddltotalRevenue" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTableColumns_SelectedIndexChanged" />
                    </td>
                </tr>
                <tr id="tr_ddltotalGrant" runat="server" visible="false">
                    <td class="label">
                        <asp:Label ID="lb_ddltotalGrant" runat="server" Text="Field: totalGrant"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:DropDownList ID="ddltotalGrant" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTableColumns_SelectedIndexChanged" />
                    </td>
                </tr>
                <tr id="tr_ddlgrantEntitlement" runat="server" visible="false">
                    <td class="label">
                        <asp:Label ID="lb_ddlgrantEntitlement" runat="server" Text="Field: grantEntitlement"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:DropDownList ID="ddlgrantEntitlement" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTableColumns_SelectedIndexChanged" />
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
                                    <asp:Label ID="lblExisting" runat="server" Text="Existing&nbsp;&nbsp;"></asp:Label></td>
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
                        <asp:ImageButton ID="btnLoad" runat="server" ImageUrl="~/images/btnLoad.gif" OnClientClick="return confirmPrompt('Do you want to load K12 data?');"/>
                        <%--OnClick="btnLoad_Click" />--%>
                    </td>
                </tr>
            </table>
            <rsweb:ReportViewer ID="rpvReports" runat="server"></rsweb:ReportViewer>
        </div>             
    </div>         
</asp:Content>
