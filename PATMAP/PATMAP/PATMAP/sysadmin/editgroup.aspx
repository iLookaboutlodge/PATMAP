<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="editgroup.aspx.vb" Inherits="PATMAP.editgroup" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register TagPrefix="patmap" TagName="sysTabMenu" Src="~/tabmenu.ascx" %>
<asp:Content ID="tabMenu" ContentPlaceHolderID="tabMenu" runat="server">   
   <patmap:sysTabMenu id="subMenu" runat="server"></patmap:sysTabMenu>
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server">       
     <div class="commonContent">
       <div class="commonHeader">
             <div class="PageHeaderModule">
                <div class="Header">
                    <p class="Title"><asp:Label ID="lblTitle" runat="server" Text="Edit User Group"></asp:Label></p>
                </div>
            </div>
        </div>
        <div class="commonForm">
            <div class="btnsTop"><asp:ImageButton ID="btnSave" runat="server" ImageUrl="~/images/btnSave.gif" /> <asp:ImageButton ID="btnCancel" runat="server" ImageUrl="~/images/btnCancel.gif" /></div><br />
            <div class="clear"></div>
            <p>Fields marked with an asterisk (<span class="requiredField">*</span>) are required.</p>
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td class="label">
                        <asp:Label ID="lblGroupName" runat="server" Text="Group Name"></asp:Label> <span class="requiredField">*</span> 
                    </td>
                    <td class="field">
                        <asp:TextBox ID="txtGroupName" runat="server" CssClass="txtNormal"></asp:TextBox> <asp:ImageButton ID="btnHGroupName" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton>
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label ID="lblDescription" runat="server" Text="Description"></asp:Label>
                    </td>
                    <td class="field">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td><asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="txtArea"></asp:TextBox></td>
                                <td valign="top" style="padding-left: 4px;"><asp:ImageButton ID="btnHDescription" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                            </tr>
                        </table>                        
                    </td>
                </tr> 
                <tr>
                    <td colspan="2" align="center">
                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                            <tr>
                                <td><asp:Label ID="lblUsers" runat="server" Text="Users"></asp:Label> <asp:ImageButton ID="btnHUsers" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False"></asp:ImageButton></td>
                                <td>&nbsp;</td>
                                <td><asp:Label ID="lblMembers" runat="server" Text="Members"></asp:Label> <asp:ImageButton ID="btnHMembers" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False"></asp:ImageButton></td>
                            </tr>
                                <tr>
                                    <td><asp:ListBox ID="lstUsers" runat="server"></asp:ListBox></td>
                                    <td>
                                        <asp:ImageButton ID="btnAddMember" runat="server" ImageUrl="~/images/btnMoveRight.gif" /><br /><br />
                                        <asp:ImageButton ID="btnRemoveMember" runat="server" ImageUrl="~/images/btnMoveLeft.gif" />
                                    </td>
                                    <td><asp:ListBox ID="lstMembers" runat="server"></asp:ListBox></td>
                                </tr>
                       </table>  
                        <br />
                    </td>                    
                </tr> 
                <tr>
                    <td class="label">
                        <asp:Label ID="lblNotes" runat="server" Text="Notes"></asp:Label>
                    </td>
                    <td class="field">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td><asp:TextBox ID="txtNotes" runat="server" TextMode="MultiLine" CssClass="txtArea"></asp:TextBox></td>
                                <td valign="top" style="padding-left:4px;"><asp:ImageButton ID="btnHNotes" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                            </tr>
                        </table>                        
                    </td>
                </tr>                                                  
            </table>
        </div>       
    </div>         
</asp:Content>
