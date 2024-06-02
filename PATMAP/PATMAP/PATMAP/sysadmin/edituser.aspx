<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="edituser.aspx.vb" Inherits="PATMAP.edituser" %>
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
                    <p class="Title"><asp:Label ID="lblTitle" runat="server" Text="Edit User"></asp:Label></p>
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
                        <asp:Label ID="lblFirstName" runat="server" Text="First Name"></asp:Label> <span class="requiredField">*</span> 
                    </td>
                    <td class="field">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td><asp:UpdatePanel ID="uplFirstName" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtFirstName" runat="server" CssClass="txtNormal" AutoPostBack="True" OnTextChanged="updateUsername"></asp:TextBox> 
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td><asp:ImageButton ID="btnHFirstName" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                            </tr>
                        </table>                        
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label ID="lblLastName" runat="server" Text="Last Name"></asp:Label> <span class="requiredField">*</span> 
                    </td>
                    <td class="field">
                         <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td> <asp:UpdatePanel ID="uplLastName" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtLastName" runat="server" CssClass="txtNormal" AutoPostBack="True" OnTextChanged="updateUsername"></asp:TextBox>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td><asp:ImageButton ID="btnHLastName" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                            </tr>
                        </table>                         
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
                        <asp:TextBox ID="txtMunicipality" runat="server" CssClass="txtNormal"></asp:TextBox> <asp:ImageButton ID="btnHMunicipality" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton>
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
                                <td valign="top"><asp:ImageButton ID="btnHRequest" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                            </tr>
                        </table>     
                    </td>
                </tr>                                        
            </table>
        </div>
        <div class="commonHeader">
             <div class="PageHeaderModule">
                <div class="Header">
                    <p class="Title">User Account</p>
                </div>
            </div>
        </div>
        <div class="commonForm">            
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                 <tr>
                    <td class="label"><asp:Label ID="lblUsername" runat="server" Text="User Name"></asp:Label> <span class="requiredField">*</span></td>
                    <td class="field" colspan="2">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="uplUsername" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:ImageButton ID="btnHUsername" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton>
                                </td>
                            </tr>
                        </table>                         
                    </td>
                    <td rowspan="6" class="field" valign="top">
                        <asp:UpdatePanel ID="uplSection" runat="server">
                            <ContentTemplate>
                                 <asp:GridView ID="grdSection" CellPadding="3" runat="server" AutoGenerateColumns="False" CssClass="grdSmStyle">
                                    <Columns>
                                        <asp:BoundField HeaderText="Section" DataField="sectionName" />
                                        <asp:BoundField HeaderText="Access" DataField="access" ItemStyle-HorizontalAlign="Center" />
                                    </Columns>
                                    <HeaderStyle CssClass="colHeader" />
                                    <AlternatingRowStyle CssClass="alertnateRow" />
                                </asp:GridView> 
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnAddLevel" EventName="Click" /> 
                                <asp:AsyncPostBackTrigger ControlID="btnRemoveLevel" EventName="Click" />                              
                            </Triggers>
                         </asp:UpdatePanel>                        
                    </td>
                </tr>                
                <tr>
                    <td class="label">
                        <asp:Label ID="lblPassword" runat="server" Text="Password"></asp:Label> <span class="requiredField">*</span></td>
                    <td class="field"> 
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="uplPwd" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" AutoPostBack="true" OnTextChanged="passwordChanged" autocomplete="off"></asp:TextBox>                                            
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnGenerate" EventName="Click" />                               
                                            <asp:AsyncPostBackTrigger ControlID="txtPassword" EventName="TextChanged" />
                                        </Triggers>
                                     </asp:UpdatePanel>                                     
                                </td>
                                <td valign="top"><asp:ImageButton ID="btnHPassword" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                            </tr>
                        </table>             
                    </td>
                    <td valign="top" style="width: 50px;"><asp:ImageButton ID="btnGenerate" runat="server" ImageUrl="~/images/btnGenerate.gif" /></td>
                </tr>  
                <tr>
                    <td class="label">
                        <asp:Label ID="lblUserGroup" runat="server" Text="User Group"></asp:Label> <span class="requiredField">*</span> 
                    </td>
                    <td class="field" valign="top">
                        <asp:DropDownList ID="ddlUserGroup" runat="server"></asp:DropDownList> <asp:ImageButton ID="btnHGroup" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton>
                    </td>
                    <td></td>
                </tr> 
                <tr>
                    <td class="label">
                        <asp:Label ID="lblUserLevel" runat="server" Text="User Level"></asp:Label> <span class="requiredField">*</span> 
                    </td>
                    <td class="field">
                        <table cellpadding="0" border="0">
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="uplLevel2" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlUserLevel" runat="server"></asp:DropDownList> 
                                        </ContentTemplate>
                                         <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnAddLevel" EventName="Click" />                               
                                            <asp:AsyncPostBackTrigger ControlID="btnRemoveLevel" EventName="Click" />                               
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                                <td><asp:ImageButton ID="btnHUserLevel" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                            </tr>
                        </table>                                                
                    </td>
                    <td valign="top"><asp:ImageButton ID="btnAddLevel" runat="server" ImageUrl="~/images/btnAdd.gif" /></td>
                </tr> 
                <tr>
                    <td class="label">&nbsp;</td>
                    <td class="field">
                     <asp:UpdatePanel ID="uplLevel" runat="server">
                        <ContentTemplate>
                            <asp:ListBox ID="lstUserLevel" runat="server"></asp:ListBox> 
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnAddLevel" EventName="Click" />                               
                            <asp:AsyncPostBackTrigger ControlID="btnRemoveLevel" EventName="Click" />                               
                        </Triggers>
                       </asp:UpdatePanel>
                    </td>
                    <td valign="top"><asp:ImageButton ID="btnRemoveLevel" runat="server" ImageUrl="~/images/btnRemove.gif" /></td>
                </tr>
                 <tr>
                    <td class="label">
                        <asp:Label ID="lblSecurity" runat="server" Text="Security Question"></asp:Label> <span class="requiredField">*</span> 
                    </td>
                    <td class="field" colspan="4">
                        <asp:DropDownList ID="ddlSecurity" runat="server"></asp:DropDownList> <asp:ImageButton ID="btnHSecurity" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton>
                    </td>
                </tr>     
                <tr>
                    <td class="label">
                        <asp:Label ID="lblAnswer" runat="server" Text="Answer"></asp:Label> <span class="requiredField">*</span> 
                    </td>
                    <td class="field" colspan="4">
                        <asp:TextBox ID="txtAnswer" runat="server" CssClass="txtLong"></asp:TextBox> <asp:ImageButton ID="btnHAnswer" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton>
                    </td>
                </tr> 
                <tr>
                    <td class="label">
                        <asp:Label ID="lblActive" runat="server" Text="Active"></asp:Label></td>
                    <td class="field" colspan="4">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td><asp:RadioButtonList ID="rblActive" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem>Yes</asp:ListItem>
                                        <asp:ListItem>No</asp:ListItem>
                                    </asp:RadioButtonList></td>
                                <td style="padding-left:4px;"><asp:ImageButton ID="btnHActive" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                            </tr>
                        </table>
                    </td>
                </tr>       
            </table>
        </div>
    </div>         
</asp:Content>
