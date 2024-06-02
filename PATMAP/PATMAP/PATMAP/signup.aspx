<%@ Page Language="VB" MasterPageFile="~/NoLogin.master" AutoEventWireup="false" Inherits="PATMAP.SignUp" Codebehind="signup.aspx.vb" %>
<%@ MasterType VirtualPath="~/NoLogin.master" %>
<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server">                 
    <div class="commonContent">
       <div id="signupHeader">
             <div class="PageHeaderModule">
                <div class="Header">
                    <p class="Title">Sign Up</p>
                </div>
            </div>
        </div>
        <div id="signupForm">
            <asp:Panel ID="pnlForm" runat="server">
            
            <p>Fields marked with an asterisk (<span class="requiredField">*</span>) are required.</p>
            <table cellpadding="0" cellspacing="0" border="0">
               <tr>
                    <td class="label">
                        <asp:Label ID="lblFirstName" runat="server" Text="First Name"></asp:Label> <span class="requiredField">*</span> 
                    </td>
                    <td class="field">
                        <asp:TextBox ID="txtFirstName" runat="server" CssClass="txtNormal"></asp:TextBox> <asp:ImageButton ID="btnHFirstName" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton>
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label ID="lblLastName" runat="server" Text="Last Name"></asp:Label> <span class="requiredField">*</span> 
                    </td>
                    <td class="field">
                        <asp:TextBox ID="txtLastName" runat="server" CssClass="txtNormal"></asp:TextBox> <asp:ImageButton ID="btnHLastName" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton>
                    </td>
                </tr>   
                <tr>
                    <td class="label">
                        <asp:Label ID="lblOrganization" runat="server" Text="Organization"></asp:Label> <span class="requiredField">*</span> 
                    </td>
                    <td class="field">
                        <asp:TextBox ID="txtOrganization" runat="server" CssClass="txtNormal"></asp:TextBox> <asp:ImageButton ID="btnHOrganization" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton>
                    </td>
                </tr> 
                <tr>
                    <td class="label">
                        <asp:Label ID="lblPosition" runat="server" Text="Position"></asp:Label> <span class="requiredField">*</span> 
                    </td>
                    <td class="field">
                        <asp:TextBox ID="txtPosition" runat="server" CssClass="txtNormal"></asp:TextBox> <asp:ImageButton ID="btnHPosition" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton>
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label ID="lblEmail" runat="server" Text="Email"></asp:Label> <span class="requiredField">*</span> 
                    </td>
                    <td class="field">
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="txtNormal"></asp:TextBox> <asp:ImageButton ID="btnHEmail" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton>
                    </td>
                </tr> 
                <tr>
                    <td class="label">
                        <asp:Label ID="lblWorkPhone" runat="server" Text="Work Phone"></asp:Label> <span class="requiredField">*</span> 
                    </td>
                    <td class="field">
                        (<asp:TextBox ID="txtWP1" runat="server" Width="35" MaxLength="3"></asp:TextBox>) - <asp:TextBox ID="txtWP2" runat="server" Width="35" MaxLength="3"></asp:TextBox> - <asp:TextBox ID="txtWP3" runat="server" Width="45" MaxLength="4"></asp:TextBox> Ext. <asp:TextBox ID="txtWP4" runat="server" Width="45" MaxLength="4"></asp:TextBox> <asp:ImageButton ID="btnHWP1" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton>
                    </td>
                </tr>  
                <tr>
                    <td class="label">
                        <asp:Label ID="lblFax" runat="server" Text="Fax"></asp:Label>
                    </td>
                    <td class="field">
                        (<asp:TextBox ID="txtFax1" runat="server" Width="35" MaxLength="3"></asp:TextBox>) - <asp:TextBox ID="txtFax2" runat="server" Width="35" MaxLength="3"></asp:TextBox> - <asp:TextBox ID="txtFax3" runat="server" Width="45" MaxLength="4"></asp:TextBox> <asp:ImageButton ID="btnHFax1" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton>
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label ID="lblAddress1" runat="server" Text="Address1"></asp:Label> 
                    </td>
                    <td class="field">
                        <asp:TextBox ID="txtAddress1" runat="server" CssClass="txtLong"></asp:TextBox> <asp:ImageButton ID="btnHAddress1" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton>
                    </td>
                </tr> 
                <tr>
                    <td class="label">
                        <asp:Label ID="lblAddress2" runat="server" Text="Address2"></asp:Label> 
                    </td>
                    <td class="field">
                        <asp:TextBox ID="txtAddress2" runat="server" CssClass="txtLong"></asp:TextBox> <asp:ImageButton ID="btnHAddress2" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton>
                    </td>
                </tr>    
                <tr>
                    <td class="label">
                        <asp:Label ID="lblMunicipality" runat="server" Text="Municipality"></asp:Label> 
                    </td>
                    <td class="field">
                        <asp:TextBox ID="txtMunicipality" runat="server" CssClass="txtNormal"></asp:TextBox> <asp:ImageButton ID="btnHMuncipality" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton>
                    </td>
                </tr> 
                <tr>
                    <td class="label">
                        <asp:Label ID="lblProvince" runat="server" Text="Province"></asp:Label> 
                    </td>
                    <td class="field">
                        <asp:DropDownList ID="ddlProvince" runat="server"></asp:DropDownList> <asp:ImageButton ID="btnHProvince" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton>
                    </td>
                </tr> 
                <tr>
                    <td class="label">
                        <asp:Label ID="lblPostalCode" runat="server" Text="PostalCode"></asp:Label>
                    </td>
                    <td class="field">
                        <asp:TextBox ID="txtPostalCode" runat="server" Width="55" MaxLength="7"></asp:TextBox> <asp:ImageButton ID="btnHPostalCode" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton>
                    </td>
                </tr>  
                <tr>
                    <td class="label">
                        <asp:Label ID="lblRequest" runat="server" Text="Requested Interest"></asp:Label>
                    </td>
                    <td class="field">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td valign="top">
                                    <asp:CheckBoxList ID="cklRequest" runat="server">
                                        <asp:ListItem>Assessment and Tax Model</asp:ListItem>
                                        <asp:ListItem>Boundary Changes</asp:ListItem>
                                    </asp:CheckBoxList>
                                </td>
                                <td style="padding-left:4px;" valign="top"><asp:ImageButton ID="btnHRequest" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                            </tr>
                        </table>                        
                    </td>
                </tr> 
                 <tr>
                    <td class="label">
                        <asp:Label ID="lblSecurity" runat="server" Text="Security Question"></asp:Label> <span class="requiredField">*</span> 
                    </td>
                    <td class="field">
                        <asp:DropDownList ID="ddlSecurity" runat="server"></asp:DropDownList> <asp:ImageButton ID="btnHSecurity" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton>
                    </td>
                </tr>     
                <tr>
                    <td class="label">
                        <asp:Label ID="lblAnswer" runat="server" Text="Answer"></asp:Label> <span class="requiredField">*</span> 
                    </td>
                    <td class="field">
                        <asp:TextBox ID="txtAnswer" runat="server" CssClass="txtLong"></asp:TextBox> <asp:ImageButton ID="btnHAnswer" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton>
                    </td>
                </tr>
                <tr class="btnsBottom">  
                    <td>&nbsp;</td>                 
                    <td class="field"><br /><asp:ImageButton ID="btnSubmit" runat="server" ImageUrl="~/images/btnSubmit.gif" /> <asp:ImageButton ID="btnClear" runat="server" ImageUrl="~/images/btnClear.gif" /></td>
                </tr>         
            </table>
            
            </asp:Panel>
            <asp:Label ID="lblSuccess" runat="server" Text="Your information has been submitted. Your username and password will be emailed to you." Visible="False"></asp:Label>            
        </div>
    </div>
</asp:Content>

