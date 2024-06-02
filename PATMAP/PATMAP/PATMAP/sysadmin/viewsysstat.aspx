<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="viewsysstat.aspx.vb" Inherits="PATMAP.viewsysstat" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server">
     <script language="javascript" type="text/javascript" src="../js/general.js"></script>
     
     <div class="commonContent">
       <div class="commonHeader">
             <div class="PageHeaderModule">
                <div class="Header">
                    <p class="Title">Clean up the database and files</p>
                </div>
            </div>
       </div>  
       <div class="commonForm">
           <asp:ImageButton ID="btnSubmit" runat="server" ImageUrl="~/images/btnSubmit.gif" />
       </div> 
       </br>
       </br>
       <div class="commonHeader">
             <div class="PageHeaderModule">
                <div class="Header">
                    <p class="Title">System Statistics</p>
                </div>
            </div>
        </div>
        <div class="commonForm">   
            <table cellpadding="0" cellspacing="0" border="0">                
                <tr>
                    <td class="label"><asp:Label ID="lblReport" runat="server" Text="Report"></asp:Label></td>
                    <td class="field" colspan="3"><asp:DropDownList ID="ddlReport" runat="server"></asp:DropDownList> <asp:ImageButton ID="btnHReport" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>           
                </tr> 
                <tr>
                    <td>&nbsp;</td>                                       
                    <td class="field"><asp:Label ID="lblFrom" runat="server" Text="From" CssClass="labelSmall"></asp:Label></td>
                    <td class="field" colspan="2"><asp:Label ID="lblTo" runat="server" Text="To" CssClass="labelSmall"></asp:Label></td>                   
                </tr>
                 <tr>                    
                    <td class="label"><asp:Label ID="lblDate" runat="server" Text="Date"></asp:Label></td>
                    <td class="field">
                        <asp:TextBox ID="txtDateFrom" runat="server" width="60" ReadOnly="true"></asp:TextBox>                                           
                        <asp:ImageButton runat="Server" ID="btnDateFrom" ImageUrl="~/images/btnCalendar.gif" AlternateText="Click to show calendar" />
                        <ajaxToolkit:CalendarExtender ID="calDateFrom" runat="server" TargetControlID="txtDateFrom" PopupButtonID="btnDateFrom" Format="M/d/yyyy"/>
                    </td>
                    <td class="field"><asp:TextBox ID="txtDateTo" runat="server" width="60" ReadOnly="true"></asp:TextBox>
                        <asp:ImageButton runat="Server" ID="btnDateTo" ImageUrl="~/images/btnCalendar.gif" AlternateText="Click to show calendar" />
                        <ajaxToolkit:CalendarExtender ID="calDateTo" runat="server" TargetControlID="txtDateTo" PopupButtonID="btnDateTo" Format="M/d/yyyy"/> <asp:ImageButton ID="btnHDateRange" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                    <td class="btns" align="right"><asp:ImageButton ID="btnShow" runat="server" ImageUrl="~/images/btnShow.gif" /> <asp:ImageButton ID="btnClear" runat="server" ImageUrl="~/images/btnClear.gif" /></td>
                </tr>                             
            </table> 
            <br /><br />
            <rsweb:ReportViewer ID="rpvReports" runat="server" ProcessingMode="Remote" Height="550px" Width="650px" ShowParameterPrompts="False" ></rsweb:ReportViewer>           
        </div>                       
    </div>         
</asp:Content>