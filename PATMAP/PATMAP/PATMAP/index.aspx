<%@ Page Language="VB" MasterPageFile="NoLogin.master" AutoEventWireup="false" Inherits="PATMAP.index" Codebehind="index.aspx.vb" %>
<%@ MasterType VirtualPath="NoLogin.master" %>
<asp:Content ContentPlaceHolderID="mainContent" runat="server">              
    <div id="loginContent">
        <div id="loginHeader">
             <div class="PageHeaderModule">
                <div class="Header">
	                <p class="Title">Login</p>
                </div>
            </div>
        </div>
        <div id="loginBox">
            <asp:panel defaultbutton="btnLogin" runat="server">
            <table border="0" cellpadding="0">
                <tr>
                    <td class="label"><asp:Label ID="lblUsername" runat="server" Text="User Name"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox> <asp:ImageButton ID="btnHUsername" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False"></asp:ImageButton>
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label ID="lblPassword" runat="server" Text="Password"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" autocomplete="off"></asp:TextBox> <asp:ImageButton ID="btnHPassword" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False"></asp:ImageButton>
                    </td>
                </tr>                                    
                <tr>
                    <td>&nbsp;</td>
                    <td class="field" style="width: 13px">
                        <asp:ImageButton ID="btnLogin" runat="server" AlternateText="Login" ImageUrl="~/images/btnLogin.gif"/>
                    </td>
                    <td class="field">
                        <asp:ImageButton ID="btnSSO" runat="server" AlternateText="Singal Sign-On" ImageUrl="~/images/sso-button.png" Height="19px" Width="103px" />
                    </td>
                    <td class="field">
                        &nbsp;</td>
                </tr>
            </table>
            </asp:panel>
            <asp:Panel ID="panelLoginLinks" runat="Server">
                <div id="loginLinks">
                    <br />
                    <a href="signup.aspx">Sign up</a>
                    <br />
                    <a href="forgotpassword.aspx">Forgot your password?</a>
                    <br />
                    <br />
                </div>
            </asp:Panel>            
        </div>      
        <asp:Panel ID="pnlMoreLinks" runat="Server">
            <div id="requirements">
                <br />


                </p>
            </div>    
        </asp:Panel>           
    </div>    
</asp:Content>
