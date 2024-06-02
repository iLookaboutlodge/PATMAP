<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="starttax.aspx.vb" Inherits="PATMAP.starttax" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %> 
<%@ Register TagPrefix="patmap" TagName="taxtoolsTabMenu" Src="~/tabmenu.ascx" %>
<asp:Content ID="tabMenu" ContentPlaceHolderID="tabMenu" runat="server">   
    <patmap:taxtoolsTabMenu ID="subMenu" runat="server" />
</asp:Content>

<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server">
     <div class="commonContent">  
        <div class="commonHeader">
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="width:35%">
                        <asp:Label ID="lblSubjYr" runat="server" Text="Subject Year: " CssClass="labelTaxModel"></asp:Label> &nbsp;
                        <asp:Label ID="lblLiveSubjYr" runat="server" Text=""></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblSubjMun" runat="server" Text="Subject Municipality: " CssClass="labelTaxModel"></asp:Label> &nbsp;
                        <asp:Label ID="lblLiveSubjMun" runat="server" Text=""></asp:Label>
                    </td>         
                </tr>
            </table>
            <br />
            <div class="PageHeaderModule">
                <div class="Header">
                    <p class="Title">Model Starting Rate and Revenue <span style="font-weight:normal; text-transform:capitalize;">(No tax tools Applied)</span></p>
                </div>
            </div>
        </div>
        <div class="commonForm">
        <p><b>Subject Year Starting Revenue</b></p>
       
        <asp:UpdatePanel ID="uplStartTax" runat="server">
            <ContentTemplate>
            <table width="100%" class="displayRevisions" cellpadding="3" cellspacing="0" rules="all" border="1" style="border-collapse:collapse; border:solid 1px snow">
                <tr class="colHeader">
                    <td>&nbsp;</td>
                    <td>PATMAP Calculated</td>
                    <td>Model Start Values</td>
                    <td style="width:66px">Edit</td>
                </tr>
                <tr>
                    <td style="height:30px; text-align:right; text-indent:5px">Uniform Mill Rate</td>
                    <td><asp:TextBox ID="txtUniformMillRate" runat="server" Text="0" Width="188px" Enabled="false" style="text-align:right; vertical-align:middle; margin:0px 2px"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtEditUniformMillRate" runat="server" width="188px" wrap="False" style="text-align:right; margin: 0px 2px; vertical-align: middle;" Enabled="False">0.0000</asp:TextBox></td>
                    <td>&nbsp;<asp:RadioButton ID="rdoEditUniformMillRate" runat="server" GroupName="editValues" ToolTip="Select this option to edit Uniform Mill Rate" AutoPostBack="True" /></td>
                </tr>
                <tr class="alternateRowLTT">
                    <td style="height:30px; text-align:right; text-indent:5px">Municipal Revenue</td>
                    <td><asp:TextBox ID="txtMunicipalRevenue" runat="server" Text="0" Width="188px" Enabled="false" style="vertical-align: middle; text-align: right; margin: 0px 2px;"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtEditMunicipalRevenue" runat="server" Width="188px" style="text-align:right; margin: 0px 2px; vertical-align: middle;" Enabled="False">$00,000,000.00</asp:TextBox></td>
                    <td>&nbsp;<asp:RadioButton ID="rdoEditMunicipalRevenue" runat="server" GroupName="editValues" ToolTip="Select this option to edit Municipal Revenue" AutoPostBack="True" /></td>
                </tr>
            </table>
            </ContentTemplate>
          </asp:UpdatePanel>
          
            </div>
        <br />
            <table cellpadding="0" cellspacing="0" border="0" width="100%">             
                <tr>
                    <td class="btnsBottom" colspan="4"><asp:ImageButton ID="btnSubmit" runat="server" ImageUrl="~/images/btnSubmit.gif" /> <asp:ImageButton ID="btnClear" runat="server" ImageUrl="~/images/btnClear.gif" /></td>
                </tr>
           </table>
          
            
     </div>
</asp:Content>    
