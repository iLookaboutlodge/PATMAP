<%@ Page validateRequest="false" Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="edithelpfield.aspx.vb" Inherits="PATMAP.edithelpfield" %>
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
                    <p class="Title"><asp:Label ID="lblTitle" runat="server" Text="Edit Form Field Help Text"></asp:Label></p>
                </div>
            </div>
        </div>
        <div class="commonForm">
            <div class="btnsTop"><asp:ImageButton ID="btnSave" runat="server" ImageUrl="~/images/btnSave.gif" /> <asp:ImageButton ID="btnReset" runat="server" ImageUrl="~/images/btnReset.gif" /> <asp:ImageButton ID="btnCancel" runat="server" ImageUrl="~/images/btnCancel.gif" /></div><br />
            <div class="clear"></div>
            <p>Fields marked with an asterisk (<span class="requiredField">*</span>) are required.</p>
            <%--<asp:UpdatePanel ID="uplHelp" runat="server">
                <ContentTemplate>--%>
                    <table cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td class="label" style="height: 34px"><asp:Label ID="lblSection" runat="server" Text="Section"></asp:Label></td>
                            <td class="field" colspan="2" style="height: 34px"><asp:DropDownList ID="ddlSection" runat="server" AutoPostBack="True"></asp:DropDownList> <asp:ImageButton ID="btnHSection" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                        </tr> 
                        <tr>
                            <td class="label"><asp:Label ID="lblScreen" runat="server" Text="Screen"></asp:Label></td>
                            <td class="field" colspan="2"><asp:DropDownList ID="ddlScreen" runat="server" AutoPostBack="True"></asp:DropDownList> <asp:ImageButton ID="btnHScreen" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>                                       
                        </tr>
                        <tr>
                            <td class="label"><asp:Label ID="lblType" runat="server" Text="Type"></asp:Label></td>
                            <td class="field" colspan="2"><asp:DropDownList ID="ddlType" runat="server" AutoPostBack="True"></asp:DropDownList> <asp:ImageButton ID="btnHType" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>                                       
                        </tr>
                         <tr>
                            <td class="label"><asp:Label ID="lblFieldName" runat="server" Text="Fieldname"></asp:Label></td>
                            <td class="field" colspan="2"><asp:DropDownList ID="ddlFieldName" runat="server"></asp:DropDownList> <asp:ImageButton ID="btnHFieldName" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>                                       
                        </tr>
                        <tr>
                            <td class="label">
                                <asp:Label ID="lblDescription" runat="server" Text="Description"></asp:Label>
                            </td>
                            <td class="field" colspan="2">
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td><asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="txtArea"></asp:TextBox></td>
                                        <td valign="top"><asp:ImageButton ID="btnHDescription" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                    </tr>
                                </table>                                
                            </td>                            
                        </tr> 
                        <tr>
                            <td class="label">
                                <asp:Label ID="lblHelpText" runat="server" Text="Help Text"></asp:Label> 
                            </td>   
                            <td class="field">
                                <FTB:FreeTextBox id="ftbHelpText"
                                  ToolbarLayout="Cut,Copy,Paste,Undo,Redo|ParagraphMenu,FontFacesMenu,FontSizesMenu|Strikethrough,Superscript,Subscript|FontForeColorsMenu,FontForeColorPicker,FontBackColorsMenu,FontBackColorPicker
                                        |Bold,Italic,Underline|
                                        NumberedList,NumberedList,BulletedList|Indent, Outdent|JustifyLeft,JustifyRight,JustifyCenter,JustifyFull|InsertImage,CreateLink, Unlink|SymbolsMenu,StyleMenu"
                                   Width="570px" Height="100px" runat="Server" /> 
                            </td> 
                            <td valign="top"><asp:ImageButton ID="btnHHelpText" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>                
                        </tr> 
                        <tr>
                             <td><asp:Label ID="lblReset" runat="server" Text="Reset Point"></asp:Label></td>
                            <td class="field" colspan="2">
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td><asp:CheckBox ID="ckbReset" Text="Set as reset point" runat="server" /></td>
                                        <td class="btns"><asp:ImageButton ID="btnHReset" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                        <td style="padding-left: 20px;"><asp:CheckBox ID="ckbAll" Text="Use the same text for all the same fields" runat="server" /></td>
                                    </tr>
                                </table>   
                            </td>
                        </tr>                                 
                         <tr>
                            <td class="label">                               
                                <asp:Label ID="lblNotes" runat="server" Text="Notes"></asp:Label>
                            </td>
                            <td class="field" colspan="2">
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td><asp:TextBox ID="txtNotes" runat="server" TextMode="MultiLine" CssClass="txtArea"></asp:TextBox></td>
                                        <td valign="top"><asp:ImageButton ID="btnHNotes" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                    </tr>    
                                </table>                                
                            </td>                            
                        </tr>                      
                    </table>  
                <%--</ContentTemplate>
                 <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>--%>                     
        </div>                    
    </div>        
</asp:Content>


