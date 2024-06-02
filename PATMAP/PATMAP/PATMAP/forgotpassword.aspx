<%@ Page Language="VB" MasterPageFile="NoLogin.master" AutoEventWireup="false" Inherits="PATMAP.forgotpassword" Codebehind="forgotpassword.aspx.vb" %>
<%@ MasterType VirtualPath="NoLogin.master" %>
<asp:Content ContentPlaceHolderID="mainContent" runat="server">                 
    <div id="loginContent">
        <div id="loginHeader">
             <div class="PageHeaderModule">
                <div class="Header">
	                <p class="Title">Forgot Your Password</p>
                </div>
            </div>
        </div>
        <div id="loginBox">                   
            <asp:Panel ID="pnlEmail" CssClass="panelForm" style="width: 100%;" runat="server">
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td class="label"><asp:Label ID="lblEmail" runat="server" Text="Email"></asp:Label></td>
                        <td class="field"><asp:TextBox ID="txtEmail" runat="server" CssClass="txtNormal"></asp:TextBox> <asp:ImageButton ID="btnHEmail" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td class="field"><asp:ImageButton ID="btnESubmit" runat="server" ImageUrl="~/images/btnSubmit.gif"/></td>
                    </tr>
               </table> 
            </asp:Panel>    
             <asp:Panel ID="pnlQuestion" Visible="false" CssClass="panelForm" style="width: 100%;" runat="server">
                <table cellpadding="0" cellspacing="0" border="0" width="150">
                    <tr>
                        <td class="label"><asp:Label ID="lblQuestion" runat="server" Text="Security Question"></asp:Label></td>
                        <td class="field"><asp:Label ID="txtQuestion" runat="server" Text="" CssClass="txtNormal"></asp:Label> <asp:ImageButton ID="btnHQuestion" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                    </tr>
                    <tr>
                        <td class="label"><asp:Label ID="lblAnswer" runat="server" Text="Answer"></asp:Label></td>
                        <td class="field"><asp:TextBox ID="txtAnswer" runat="server" CssClass="txtNormal"></asp:TextBox> <asp:ImageButton ID="btnHAnswer" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td class="field"><asp:ImageButton ID="btnQSubmit" runat="server" ImageUrl="~/images/btnSubmit.gif"/></td>
                    </tr>
               </table> 
            </asp:Panel>    
             <asp:Panel ID="pnlSuccess" Visible="false" CssClass="panelForm" style="width: 80%;" runat="server">
                 <asp:Label ID="lblSuccess" runat="server" Text="Your password has been reset.  Your username and new password has been sent to your email address."></asp:Label><br /><br />
                 To sign-in, <a href="index.aspx">click here</a>
            </asp:Panel>                                         
        </div>    
        <br /><br />        
    </div>    
</asp:Content>

