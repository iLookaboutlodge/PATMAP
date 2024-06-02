<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="tables.aspx.vb" Inherits="PATMAP.tables2" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %> 
<%@ Register TagPrefix="patmap" TagName="taxtoolsTabMenu" Src="~/tabmenu.ascx" %>

<asp:Content ID="tabMenu" ContentPlaceHolderID="tabMenu" runat="server">   
    <patmap:taxtoolsTabMenu ID="subMenu" runat="server" />
</asp:Content>

<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server">
    <div class="commonContent">  
        <div class="commonHeader">
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="width:35%; height:25px">
                        <asp:Label ID="lblSubjYr" runat="server" Text="Subject Year: " CssClass="labelTaxModel"></asp:Label> &nbsp;
                        <asp:Label ID="lblLiveSubjYr" runat="server" Text="2008" />
                    </td>
                    <td>
                        <asp:Label ID="lblSubjMun" runat="server" Text="Subject Municipality: " CssClass="labelTaxModel"></asp:Label> &nbsp;
                        <asp:Label ID="lblLiveSubjMun" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblStartingUniformMillRate" runat="server" Text="Starting Uniform Mill Rate: " CssClass="labelTaxModel"></asp:Label> &nbsp;
                        <asp:Label ID="lblLiveStartingUniformMillRate" runat="server" Text="0.0000"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblStartingRevenue" runat="server" Text="Starting Revenue: " CssClass="labelTaxModel"></asp:Label> &nbsp;
                        <asp:Label ID="lblLiveStartingRevenue" runat="server" Text="$00,000,000"></asp:Label>
                    </td>
                </tr>
            </table>
            <br />              
            <div class="PageHeaderModule">
                <div class="Header">
                    <p class="Title">Model Report Tables</p>
                </div>
            </div>
            <div class="commonForm">
                <br />
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td>
                            <div class="groupBox">
                            <table cellpadding="5px" cellspacing="5px" border="0">
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlReportType" runat="server" ToolTip="Select Report Type" AutoPostBack="True">
                                        </asp:DropDownList> <asp:ImageButton ID="btnHReportType" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp" />
                                    </td>
                                </tr>
                                 <tr>                       
                                    <td id="tdTaxClasses" runat="server" style="vertical-align:top">
                                        <div class="groupBox">
                                            <b>Classes</b>  <asp:ImageButton ID="btnHTaxClasses" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp" style="vertical-align:text-bottom" />
                                            <asp:CheckBoxList ID="cklTaxClasses" runat="server" style="width:300px"></asp:CheckBoxList>
                                        </div>
                                     </td>
                                    <td>
                                        <div class="groupBox">
                                            <b>Tax Status</b>  <asp:ImageButton ID="btnHTaxStatus" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp" style="vertical-align:text-bottom" />
                                            <asp:CheckBoxList ID="cklTaxStatus" runat="server" style="width:150px; padding:5px">
                                                <asp:ListItem Value="1"> Taxable</asp:ListItem>
                                                <asp:ListItem Value="6"> PGIL</asp:ListItem>
                                                <asp:ListItem Value="5"> FGIL</asp:ListItem>
                                                <asp:ListItem Value="4"> Exempt</asp:ListItem>
                                            </asp:CheckBoxList>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="btnsBottom" align="right">           
                            <asp:ImageButton id="show" runat="server" ImageUrl="~/images/btnShow.gif"/>
                            <hr />  
                        </td>
                    </tr>
                </table>   
            </div>
        </div>
    </div>
</asp:Content>
