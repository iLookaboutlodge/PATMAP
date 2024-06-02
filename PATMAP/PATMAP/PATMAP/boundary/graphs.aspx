<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="graphs.aspx.vb" Inherits="PATMAP.graphs1" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>  
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register TagPrefix="patmap" TagName="boundaryTabMenu" Src="~/tabmenu.ascx" %>
<asp:Content ID="tabMenu" ContentPlaceHolderID="tabMenu" runat="server">   
    <patmap:boundaryTabMenu ID="subMenu" runat="server" />
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server">       
     <div class="commonContent">       
       <div class="commonHeader">
             <div class="PageHeaderModule">
                <div class="Header">
                    <p class="Title">Graphs</p>
                </div>
            </div>
        </div>     
        <div class="commonForm">           
           <table cellpadding="0" cellspacing="0" border="0" width="100%">
               <tr>
                    <td>
                        <br />  
                        <div class="groupBox">
                            <table cellpadding="0" cellspacing="2" border="0">
                                <tr>
                                     <td style="height: 23px"><asp:DropDownList ID="ddlReport" runat="server"></asp:DropDownList> <asp:ImageButton ID="btnHReport" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton> </td>
                                     <td style="height: 23px"><asp:DropDownList ID="ddlTaxType" runat="server" AutoPostBack="True">
                                            <asp:ListItem Value="0">--Tax Type--</asp:ListItem>
                                            <asp:ListItem Value="1">Municipal Tax</asp:ListItem>
                                     </asp:DropDownList> <asp:ImageButton ID="btnHTaxType" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>                                
                                </tr>
                                <tr>
                                    <td style="height: 23px"><asp:DropDownList ID="ddlTaxStatus" runat="server" AutoPostBack="True"></asp:DropDownList> <asp:ImageButton ID="btnHTaxStatus" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                    <td style="height: 23px"><asp:ImageButton ID="btnClasses" runat="server" ImageUrl="~/images/btnClasses.gif"></asp:ImageButton> <asp:ImageButton ID="btnHClasses" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                </tr>                               
                            </table>                                
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="btnsBottom" style="height: 19px"><asp:ImageButton ID="btnShow" runat="server" ImageUrl="~/images/btnShow.gif" OnClientClick="openReportWindow('viewReport.aspx','toolbar=0,resizable=1,scrollbars=1'); return false;"></asp:ImageButton></td>
                </tr>                 
           </table>           
           <rsweb:ReportViewer ID="rpvReports" runat="server" Height="550px" Width="650px"></rsweb:ReportViewer>
        </div> 
                              
    </div>         
</asp:Content>



