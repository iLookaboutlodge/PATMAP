<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="edittaxentity.aspx.vb" Inherits="PATMAP.edittaxentity" %>
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
                    <p class="Title"><asp:Label ID="lblTitle" runat="server" Text="Edit Tax Entity"></asp:Label></p>
                </div>
            </div>
        </div>
        <div class="commonForm">
            <div class="btnsTop"><asp:ImageButton ID="btnSave" runat="server" ImageUrl="~/images/btnSave.gif" /> <asp:ImageButton ID="btnCancel" runat="server" ImageUrl="~/images/btnCancel.gif" /></div><br />
            <div class="clear"></div>
            <p>Fields marked with an asterisk (<span class="requiredField">*</span>) are required.</p>
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td class="label"><asp:Label ID="lblJurisType" runat="server" Text="Jurisdiction Type"></asp:Label> <span class="requiredField">*</span></td>
                    <td class="field"><asp:DropDownList ID="ddlJurisType" runat="server"></asp:DropDownList> <asp:ImageButton ID="btnHJurisType" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr>
                <tr>
                    <td class="label"><asp:Label ID="lblJurisdiction" runat="server" Text="Jurisdiction"></asp:Label> <span class="requiredField">*</span></td>
                    <td class="field"><asp:TextBox ID="txtJurisdiction" runat="server" CssClass="txtNormal"></asp:TextBox> <asp:ImageButton ID="btnHJurisdiction" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr>
                <tr>
                    <td class="label"><asp:Label ID="lblNumber" runat="server" Text="Number"></asp:Label> <span class="requiredField">*</span></td>
                    <td class="field"><asp:TextBox ID="txtNumber" runat="server" Width="50"></asp:TextBox> <asp:ImageButton ID="btnHNumber" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr>
                <asp:Panel ID="pnlSchool" runat="server">
                    <tr>
                        <td class="label"><asp:Label ID="lblRevenue" runat="server" Text="Revenue"></asp:Label></td>                        
                        <td class="field"><asp:TextBox ID="txtRevenue" runat="server" Width="150"></asp:TextBox> <asp:ImageButton ID="btnHRevenue" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                    </tr>
                     <tr>
                        <td class="label"><asp:Label ID="lblExpenditure" runat="server" Text="Expenditure"></asp:Label></td>                        
                        <td class="field"><asp:TextBox ID="txtExpenditure" runat="server" Width="150"></asp:TextBox> <asp:ImageButton ID="btnHExpenditure" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                    </tr>
                    <tr>
                        <td class="label"><asp:Label ID="lblPublic" runat="server" Text="% Public"></asp:Label></td>
                        <td class="field"><asp:TextBox ID="txtPublic" runat="server" Width="50"></asp:TextBox> <asp:ImageButton ID="btnHPublic" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                    </tr>
                     <tr>
                        <td class="label"><asp:Label ID="lblSeparate" runat="server" Text="% Separate"></asp:Label></td>
                        <td class="field"><asp:TextBox ID="txtSeparate" runat="server" Width="50"></asp:TextBox> <asp:ImageButton ID="btnHSeparate" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>                        
                    </tr>
                </asp:Panel>
                 <tr>
                    <td class="label">
                        <asp:Label ID="lblNotes" runat="server" Text="Notes"></asp:Label>
                    </td>
                    <td class="field">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td><asp:TextBox ID="txtNotes" runat="server" TextMode="MultiLine" CssClass="txtArea"></asp:TextBox></td>
                                <td valign="top"><asp:ImageButton ID="btnHNotes" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                            </tr>
                        </table>                        
                    </td>                    
                </tr>  
            </table>
        </div>              
    </div>         
</asp:Content>