<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="editreports.aspx.vb" Inherits="PATMAP.editreports" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server">       
    <div class="commonContent">
       <div class="commonHeader">
             <div class="PageHeaderModule">
                <div class="Header">
                    <p class="Title"><asp:Label ID="lblTitle" runat="server" Text="Edit Report"></asp:Label></p>
                </div>
            </div>
        </div>
        <div class="commonForm">
            <div class="btnsTop"><asp:ImageButton ID="btnSave" runat="server" ImageUrl="~/images/btnSave.gif" /> <asp:ImageButton ID="btnCancel" runat="server" ImageUrl="~/images/btnCancel.gif" /></div><br />
            <div class="clear"></div>
            <p>Fields marked with an asterisk (<span class="requiredField">*</span>) are required.</p>
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td class="label"><asp:Label ID="lblReportName" runat="server" Text="Name"></asp:Label> <span class="requiredField">*</span></td>
                    <td class="field" colspan="2"><asp:TextBox ID="txtReportName" runat="server" CssClass="txtNormal"></asp:TextBox> <asp:ImageButton ID="btnHReportName" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr>
                <tr>
                    <td class="label"><asp:Label ID="lblReportType" runat="server" Text="Type"></asp:Label> <span class="requiredField">*</span></td>
                    <td class="field" colspan="2"><asp:DropDownList ID="ddlReportType" runat="server">
                        <asp:ListItem>&lt;select&gt;</asp:ListItem>
                        <asp:ListItem>Standard</asp:ListItem>
                        <asp:ListItem>Customized</asp:ListItem>
                    </asp:DropDownList> <asp:ImageButton ID="btnHReportType" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr>
                 <tr>
                    <td class="label"><asp:Label ID="lblDisplayType" runat="server" Text="Display Type"></asp:Label> <span class="requiredField">*</span></td>
                    <td class="field" colspan="2"><asp:DropDownList ID="ddlDisplayType" runat="server">
                        <asp:ListItem>&lt;select&gt;</asp:ListItem>
                        <asp:ListItem>Tables</asp:ListItem>
                        <asp:ListItem>Graphs</asp:ListItem>
                    </asp:DropDownList> <asp:ImageButton ID="btnHDisplayType" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr>    
                <tr>
                    <td class="label">
                        <asp:Label ID="lblDescription" runat="server" Text="Description"></asp:Label>
                    </td>
                    <td class="field">
                        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="txtArea"></asp:TextBox>
                    </td>
                    <td valign="top"><asp:ImageButton ID="btnHDescription" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr> 
                <tr>
                    <td class="label"><asp:Label ID="lblFilename" runat="server" Text="Filename"></asp:Label> <span class="requiredField">*</span></td>
                    <td class="field" colspan="2"><asp:TextBox ID="txtFilename" runat="server" CssClass="txtLong"></asp:TextBox> <asp:ImageButton ID="btnHFilename" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr>                                      
                 <tr>
                    <td class="label">
                        <asp:Label ID="lblNotes" runat="server" Text="Notes"></asp:Label>
                    </td>
                    <td class="field">
                        <asp:TextBox ID="txtNotes" runat="server" TextMode="MultiLine" CssClass="txtArea"></asp:TextBox>
                    </td>
                    <td valign="top"><asp:ImageButton ID="btnHNotes" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr>                      
            </table>           
        </div>                    
    </div>        
</asp:Content>



