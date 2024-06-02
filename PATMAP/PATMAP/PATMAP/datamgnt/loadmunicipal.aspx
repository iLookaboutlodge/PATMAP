<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="loadmunicipal.aspx.vb" Inherits="PATMAP.loadmunicipal" %>
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
                    <p class="Title">Municipal</p>
                </div>
            </div>
        </div>
        <div class="commonForm">            
        <table cellpadding="0" cellspacing="0" border="0">
            <%--<tr>
                <td colspan="3">
                    <asp:Label ID="lb_Message" runat="server" Width="100%" ForeColor="#669966"></asp:Label></td>
            </tr>--%>
            <tr>
                <td class="label" style="height: 23px">
                    <asp:Label ID="lblFile" runat="server" Text="Entities File"></asp:Label></td>
                <td class="field" style="width: 224px; height: 23px;">
                    <asp:FileUpload ID="fpFile" runat="server" Width="200px" /><asp:ImageButton ID="btnHFile"
                        runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp">
                    </asp:ImageButton></td>
                <td class="field" style="height: 23px">
                    <asp:ImageButton ID="btnFileUpload" runat="server" ImageUrl="~/images/btnLoad.gif"
                        OnClick="btnFileUpload_Click" />
                </td>
            </tr>
            <tr >
                <td class="label" style="height: 23px">
                    <asp:Label ID="lblFile1" runat="server" Text="Mapping Municipal IDs File" ></asp:Label></td>
                <td class="field" style="width: 224px; height: 23px;">
                    <asp:FileUpload ID="fpFileMap" runat="server" Width="200px" /><asp:ImageButton ID="btnHMapFile"
                        runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp">
                    </asp:ImageButton></td>
                <td class="field" style="height: 23px">
                    <asp:ImageButton ID="btnFileUploadMAP" runat="server" ImageUrl="~/images/btnLoad.gif"
                        OnClick="btnFileUploadMap_Click"/>
                </td>
            </tr>
            <tr id="tr_ddlTableMunicipalitiesMapLink" runat="server" visible="false">
                <td class="label">
                    <asp:Label ID="lb_ddlTableMunicipalitiesMapLink" runat="server" Text="Sheet: Match Roll-Up"></asp:Label></td>
                <td class="field" colspan="2">
                    <asp:DropDownList ID="ddlTableMunicipalitiesMapLink" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTableNamesMAP_SelectedIndexChanged" />
                </td>
            </tr>
            <tr id="tr_ddlTableJurisdGroups" runat="server" visible="false">
                <td class="label">
                    <asp:Label ID="lb_ddlTableJurisdGroups" runat="server" Text="Sheet: JurisdGroups"></asp:Label></td>
                <td class="field" colspan="2">
                    <asp:DropDownList ID="ddlTableJurisdGroups" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTableNames_SelectedIndexChanged" />
                </td>
            </tr>
            <tr id="tr_ddlTableJurisdTypes" runat="server" visible="false">
                <td class="label">
                    <asp:Label ID="lb_ddlTableJurisdTypes" runat="server" Text="Sheet: JurisdTypes"></asp:Label></td>
                <td class="field" colspan="2">
                    <asp:DropDownList ID="ddlTableJurisdTypes" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTableNames_SelectedIndexChanged" />
                </td>
            </tr>
            <tr id="tr_ddlTableEntities" runat="server" visible="false">
                <td class="label">
                    <asp:Label ID="lb_ddlTableEntities" runat="server" Text="Sheet: Entities"></asp:Label></td>
                <td class="field" colspan="2">
                    <asp:DropDownList ID="ddlTableEntities" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTableNames_SelectedIndexChanged" />
                </td>
            </tr>
            <tr id="tr_ddlTableMunicRollup" runat="server" visible="false">
                <td class="label">
                    <asp:Label ID="lb_ddlTableMunicRollup" runat="server" Text="Sheet: MunicRollup"></asp:Label></td>
                <td class="field" colspan="2">
                    <asp:DropDownList ID="ddlTableMunicRollup" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTableNames_SelectedIndexChanged" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr id="tr_btnLoad" runat="server" visible="false">
                <td>
                </td>
                <td class="btns" style="height: 25px" colspan="2">
                    <asp:ImageButton ID="btnLoad" runat="server" ImageUrl="~/images/btnLoad.gif" OnClick="btnLoad_Click" />
                </td>
            </tr>
            <tr id="tr_btnLoadMap" runat="server" visible="false">
                <td>
                </td>
                <td class="btns" style="height: 25px" colspan="2">
                    <asp:ImageButton ID="btnLoadMap" runat="server" ImageUrl="~/images/btnLoad.gif" OnClientClick="return confirmPrompt('Do you want to load municipal data?');"/>
                    <%--OnClick="btnLoadMap_Click" />--%>
                </td>
            </tr>            
        </table>
            <rsweb:ReportViewer ID="rpvReports" runat="server"></rsweb:ReportViewer>
        </div>             
    </div>         
</asp:Content>


