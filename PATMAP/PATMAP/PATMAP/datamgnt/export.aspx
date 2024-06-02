<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="export.aspx.vb" Inherits="PATMAP.export" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server">       
     <div class="commonContent">
       <div class="commonHeader">
             <div class="PageHeaderModule">
                <div class="Header">
                    <p class="Title">Export</p>
                </div>
            </div>
        </div>
        <div class="commonForm">                        
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td class="label"><asp:Label ID="lblDataSource" runat="server" Text="Data Source"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:DropDownList ID="ddlDataSource" runat="server" AutoPostBack="True">
                            <asp:ListItem Value="0">--Data Source--</asp:ListItem>
                            <asp:ListItem Value="1">Assessment</asp:ListItem>
                            <%--<asp:ListItem Value="2">Tax Credit</asp:ListItem>--%>
                            <asp:ListItem Value="3">POV</asp:ListItem>
                            <%--<asp:ListItem Value="4">K12OG</asp:ListItem>--%>
                            <asp:ListItem Value="5">Mill Rate Survey</asp:ListItem>
                            <asp:ListItem Value="6">Potash</asp:ListItem>
                        </asp:DropDownList> 
                    </td>
                </tr>
                <tr>
                    <td class="label"><asp:Label ID="lblFileType" runat="server" Text="File Type" Visible="False" ></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:DropDownList ID="ddlFileType" runat="server" AutoPostBack="True" Visible="False">
                            <asp:ListItem Value="1">Rural Municipalities</asp:ListItem>
                            <asp:ListItem Value="2">Urban Municipalities</asp:ListItem>
                            <asp:ListItem Value="3">Potash Properties</asp:ListItem>               
                        </asp:DropDownList> 
                    </td>
                </tr>
                <tr>
                    <td class="label"><asp:Label ID="lblDSN" runat="server" Text="Data Set Name"></asp:Label></td>
                    <td class="field" colspan="2"><asp:DropDownList ID="ddlDSN" runat="server"></asp:DropDownList> <asp:ImageButton ID="btnHDSN" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr>
                 <tr>
                    <td class="label"><asp:Label ID="lblParcelNo" runat="server" Text="Parcel No. or Parcel Prefix" Visible="False"></asp:Label></td>
                    <td class="field" colspan="2"><asp:TextBox ID="txtParcelNo" runat="server" CssClass="txtNormal" Visible="False"></asp:TextBox> <asp:ImageButton ID="btnHParcelNo" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr>
                 <tr>
                    <td class="label"><asp:Label ID="lblMunicipality" runat="server" Text="Municipality" Visible="False"></asp:Label></td>
                    <td class="field" colspan="2"><asp:DropDownList ID="ddlMunicipality" runat="server" Visible="False"></asp:DropDownList> <asp:ImageButton ID="btnHMunicipality" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr>
                <tr>
                    <td class="label"><asp:Label ID="lblSchoolDivision" runat="server" Text="School Division" Visible="False"></asp:Label></td>
                    <td class="field" colspan="2"><asp:DropDownList ID="ddlSchoolDivision" runat="server" Visible="False"></asp:DropDownList> <asp:ImageButton ID="btnHSchoolDivision" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr>
                <tr>
                    <td class="label"><asp:Label ID="lblTaxType" runat="server" Text="Tax Type" Visible="False"></asp:Label></td>
                    <td class="field" colspan="2"><asp:DropDownList ID="ddlTaxType" runat="server" Visible="False"></asp:DropDownList> <asp:ImageButton ID="btnHTaxType" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr>
                <tr>
                    <td class="label"><asp:Label ID="lblTaxClass" runat="server" Text="Tax Class" Visible="False"></asp:Label></td>
                    <td class="field"><asp:DropDownList ID="ddlTaxClass" runat="server" Visible="False"></asp:DropDownList> <asp:ImageButton ID="btnHTaxClass" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>                    
                </tr> 
                <tr>
                    <td class="label"><asp:Label ID="lblPotashArea" runat="server" Text="Potash Area" Visible="False"></asp:Label></td>
                    <td class="field"><asp:DropDownList ID="ddlPotashArea" runat="server" Visible="False"></asp:DropDownList> <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>                    
                </tr>                 
                <tr>
                    <td><asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/btnSearch.gif" /></td>
                </tr>                
            </table>            
        </div>
       <rsweb:ReportViewer ID="rpvReports" runat="server" Height="550px" Width="650px" ShowParameterPrompts="False"></rsweb:ReportViewer>                  
    </div>         
</asp:Content>
