<%@ Page validateRequest="false" Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="edithelperr.aspx.vb" Inherits="PATMAP.edithelperr" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register TagPrefix="patmap" TagName="sysTabMenu" Src="~/tabmenu.ascx" %>
<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<asp:Content ID="tabMenu" ContentPlaceHolderID="tabMenu" runat="server">   
   <patmap:sysTabMenu id="subMenu" runat="server"></patmap:sysTabMenu>
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server">       
    <div class="commonContent">
       <div class="commonHeader">
             <div class="PageHeaderModule">
                <div class="Header">
                    <p class="Title"><asp:Label ID="lblTitle" runat="server" Text="Edit Error Message Help Text"></asp:Label></p>
                </div>
            </div>
        </div>
        <div class="commonForm">
            <div class="btnsTop"><asp:ImageButton ID="btnSave" runat="server" ImageUrl="~/images/btnSave.gif" /> <asp:ImageButton ID="btnReset" runat="server" ImageUrl="~/images/btnReset.gif"></asp:ImageButton> <asp:ImageButton ID="btnCancel" runat="server" ImageUrl="~/images/btnCancel.gif" /></div><br />
            <div class="clear"></div>
            <p>Fields marked with an asterisk (<span class="requiredField">*</span>) are required.</p>           
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td class="label"><asp:Label ID="lblErrName" runat="server" Text="Name"></asp:Label> <span class="requiredField">*</span></td>
                    <td class="field" colspan="2"><asp:TextBox ID="txtErrName" runat="server" CssClass="txtNormal"></asp:TextBox> <asp:ImageButton ID="btnHErrName" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr> 
                <tr>
                    <td class="label"><asp:Label ID="lblCode" runat="server" Text="Code"></asp:Label> <span class="requiredField">*</span></td>
                    <td class="field" colspan="2"><asp:TextBox ID="txtCode" runat="server" style="width:100px;"></asp:TextBox> <asp:ImageButton ID="btnHCode" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label ID="lblDescription" runat="server" Text="Description"></asp:Label>
                    </td>
                    <td class="field">
                        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="txtArea"></asp:TextBox>
                    </td>
                    <td valign="top" style="width:40%;"><asp:ImageButton ID="btnHDescription" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr> 
                <tr>
                    <td class="label">
                        <asp:Label ID="lblHelpText" runat="server" Text="Help Text"></asp:Label> 
                    </td>   
                    <td class="field" colspan="2">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td><FTB:FreeTextBox id="ftbHelpText"
                          ToolbarLayout="Cut,Copy,Paste,Undo,Redo|ParagraphMenu,FontFacesMenu,FontSizesMenu|Strikethrough,Superscript,Subscript|FontForeColorsMenu,FontForeColorPicker,FontBackColorsMenu,FontBackColorPicker
                                        |Bold,Italic,Underline|
                                        NumberedList,NumberedList,BulletedList|Indent, Outdent|JustifyLeft,JustifyRight,JustifyCenter,JustifyFull|InsertImage,CreateLink, Unlink|SymbolsMenu,StyleMenu"
                           Width="570px" Height="100px" runat="Server" /> </td>
                                <td valign="top"><asp:ImageButton ID="btnHHelpText" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>                
                            </tr>
                        </table>                        
                    </td>                     
                </tr> 
                <tr>
                    <td><asp:Label ID="lblReset" runat="server" Text="Reset Point"></asp:Label></td>
                    <td class="field" colspan="2">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td><asp:CheckBox ID="ckbReset" Text="Set as reset point" runat="server" /></td>
                                <td><asp:ImageButton ID="btnHReset" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>                                                                      
                            </tr>
                        </table>  
                    </td>
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

