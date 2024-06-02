<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="editfunctions.aspx.vb" Inherits="PATMAP.editfunctions" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server">       
    <div class="commonContent">
       <div class="commonHeader">
             <div class="PageHeaderModule">
                <div class="Header">
                    <p class="Title"><asp:Label ID="lblTitle" runat="server" Text="Edit Function"></asp:Label></p>
                </div>
            </div>
        </div>
        <div class="commonForm">
            <div class="btnsTop"><asp:ImageButton ID="btnSave" runat="server" ImageUrl="~/images/btnSave.gif" /> <asp:ImageButton ID="btnCancel" runat="server" ImageUrl="~/images/btnCancel.gif" /></div><br />
            <div class="clear"></div>
            <p>Fields marked with an asterisk (<span class="requiredField">*</span>) are required.</p>
            <table cellpadding="0" cellspacing="0" border="0">
                 <tr>
                    <td class="label"><asp:Label ID="lblFunctionName" runat="server" Text="Name"></asp:Label> <span class="requiredField">*</span></td>
                    <td class="field" colspan="2"><asp:TextBox ID="txtFunctionName" runat="server" CssClass="txtNormal"></asp:TextBox> <asp:ImageButton ID="btnHFunctionName" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr>     
                <tr>
                    <td class="label">
                        <asp:Label ID="lblDescription" runat="server" Text="Description"></asp:Label>
                    </td>
                    <td class="field">
                        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="txtArea" Height="50"></asp:TextBox>
                    </td>
                    <td valign="top"><asp:ImageButton ID="btnHDescription" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr> 
                <tr>
                    <td class="label"><asp:Label ID="lblFormula" runat="server" Text="Formula"></asp:Label> <span class="requiredField">*</span></td>
                    <td class="field" colspan="2"><asp:TextBox ID="txtFormula" TextMode="MultiLine" runat="server" CssClass="txtArea" Height="100"></asp:TextBox> <asp:ImageButton ID="btnHFormula" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr>  
                <tr>
                    <td>&nbsp;</td>
                    <td class="field" colspan="2">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td><asp:CheckBox ID="ckbReset" Text="Set as reset point" runat="server" /></td>
                                <td><asp:ImageButton ID="btnHReset" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>                                
                                <td class="btns"><asp:ImageButton ID="btnReset" runat="server" ImageUrl="~/images/btnReset.gif" /></td>                                
                            </tr>
                        </table>   
                    </td>
                </tr>                       
                 <tr>
                    <td class="label">
                        <asp:Label ID="lblNotes" runat="server" Text="Notes"></asp:Label>
                    </td>
                    <td class="field">
                        <asp:TextBox ID="txtNotes" runat="server" TextMode="MultiLine" CssClass="txtArea" Height="50"></asp:TextBox>
                    </td>
                    <td valign="top"><asp:ImageButton ID="btnHNotes" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr>                      
            </table>           
        </div>                    
    </div>        
</asp:Content>


