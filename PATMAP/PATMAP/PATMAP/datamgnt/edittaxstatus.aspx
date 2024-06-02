<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="edittaxstatus.aspx.vb" Inherits="PATMAP.edittaxstatus" %>
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
                    <p class="Title"><asp:Label ID="lblTitle" runat="server" Text="Edit Tax Status"></asp:Label></p>
                </div>
            </div>
        </div>
        <div class="commonForm">
            <div class="btnsTop"><asp:ImageButton ID="btnSave" runat="server" ImageUrl="~/images/btnSave.gif" /> <asp:ImageButton ID="btnCancel" runat="server" ImageUrl="~/images/btnCancel.gif" /></div><br />
            <div class="clear"></div>
            <p>Fields marked with an asterisk (<span class="requiredField">*</span>) are required.</p>
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td class="label"><asp:Label ID="lblTaxStatus" runat="server" Text="Tax Status"></asp:Label> <span class="requiredField">*</span></td>
                    <td class="field" colspan="2"><asp:TextBox ID="txtTaxStatus" runat="server" CssClass="txtNormal"></asp:TextBox> <asp:ImageButton ID="btnHTaxStatus" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
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
