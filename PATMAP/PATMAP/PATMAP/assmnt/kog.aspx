<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="kog.aspx.vb" Inherits="PATMAP.kog" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register TagPrefix="patmap" TagName="assmntTabMenu" Src="~/tabmenu.ascx" %>
<asp:Content ID="tabMenu" ContentPlaceHolderID="tabMenu" runat="server">   
    <patmap:assmntTabMenu ID="subMenu" runat="server" />
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server">       
     <div class="commonContent">
        <table cellpadding="0" cellspacing="0" border="0" width="92%">
            <tr>
                <td colspan="3" style="padding-left:10px;padding-bottom:15px;">
                    <p>Fields marked with an asterisk (<span class="requiredField">*</span>) are required.</p>
                </td>
            </tr>
            <tr>                                
                <td class="label" style="padding-left:10px;width:100px;"><asp:Label ID="lblScenarioName" runat="server" Text="Scenario Name:"></asp:Label><span class="requiredField">*</span></td>
                <td class="field">
                    <table cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td>
                                 <asp:UpdatePanel ID="uplName" runat="server">
                                    <ContentTemplate><asp:TextBox ID="txtScenarioName" runat="server" CssClass="txtLong" OnTextChanged="changeName" AutoPostBack="True"></asp:TextBox></ContentTemplate>
                                 </asp:UpdatePanel>
                            </td>
                            <td><asp:ImageButton ID="btnHScenarioName" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                        </tr>
                    </table>
                </td>         
                <td align="right" style="width:58px;"><asp:ImageButton ID="btnSave" runat="server" ImageUrl="~/images/btnSave.gif" /></td>
            </tr>
        </table>
       <div class="commonHeader">
             <div class="PageHeaderModule">
                <div class="Header">
                    <p class="Title">K-12 Operating Grant</p>
                </div>
            </div>
        </div>     
        <div class="commonForm">           
           <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td class="labelBase"><asp:Label ID="lblBase" runat="server" Text="Base Tax Year Model:" CssClass="labelTaxModel"></asp:Label></td>
                    <td style="width: 30%;padding-left:10px;"><asp:Label ID="txtBaseTaxYrModel" runat="server" Text=""></asp:Label></td>
                    <td class="labelSubject"><asp:Label ID="lblSubject" runat="server" Text="Subject Tax Year Model:" CssClass="labelTaxModel"></asp:Label></td>
                    <td style="width: 30%;padding-left:10px;"><asp:Label ID="txtSubjectTaxYrModel" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <td colspan="4">
                        <br />                        
                        <table cellpadding="0" cellspacing="0" border="0">                            
                            <tr>
                                <td class="label"><asp:Label ID="lblSubjectKOG" runat="server" Text="Subject Year K-12 OG Data Set"></asp:Label> <span class="requiredField">*</span></td>
                                <td class="field"><asp:DropDownList ID="ddlSubjectKOG" runat="server"></asp:DropDownList> <asp:ImageButton ID="btnHSubjectKOG" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                            </tr>
                            <tr>
                                <td class="label"><asp:Label ID="lblKOGAmt" runat="server" Text="K-12 OG Amount"></asp:Label></td>
                                <td class="field">
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td valign="top" style="padding-top: 4px;"><asp:TextBox ID="txtKOGAmt" runat="server" Width="70"></asp:TextBox></td>
                                            <td class="field">
                                                    <asp:RadioButtonList ID="rblKOGAmt" runat="server">
                                                    <asp:ListItem>Replacement</asp:ListItem>
                                                    <asp:ListItem>In addition</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td class="btnHelp"><asp:ImageButton ID="btnHKOGAmt" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>               
                <tr>
                    <td class="btnsBottom" colspan="4"><asp:ImageButton ID="btnSubmit" runat="server" ImageUrl="~/images/btnSubmit.gif" /> <asp:ImageButton ID="btnClear" runat="server" ImageUrl="~/images/btnClear.gif" /></td>
                </tr>
           </table>
        </div> 
                              
    </div>         
</asp:Content>
