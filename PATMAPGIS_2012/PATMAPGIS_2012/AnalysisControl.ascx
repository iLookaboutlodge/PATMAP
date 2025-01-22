<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AnalysisControl.ascx.cs" Inherits="AnalysisControl" %>
<%@ Register Src="ParcelThemes.ascx" TagName="ParcelThemes" TagPrefix="uc2" %>
<%@ Register Src="FiltersControl.ascx" TagName="FiltersControl" TagPrefix="uc1" %>
<table style="width:100%">
    <tr>
        <td align="center">
            <asp:Label ID="lblAnalysisControls" runat="server" Text="Analysis Layers" CssClass="sectionHeader"></asp:Label></td>
    </tr>
</table>
<table class="navigation" runat="server" id="tblNavigation" style="display:none">
    <tr>
        <td>
            <asp:LinkButton ID="btnProvicialThemes" Text="Provicial Themes" CommandName="0" runat="server" OnClick="tabControls_MenuItemClick"></asp:LinkButton>
        </td>
        <td>
            <asp:LinkButton ID="btnParcelThemes" Text="Parcel Themes" CommandName="1" runat="server" OnClick="tabControls_MenuItemClick"></asp:LinkButton>
        </td>
    </tr>
</table>
<asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
    <asp:View ID="ProvicialThemesTab" runat="server">
    <asp:UpdatePanel ID="updAnalysis" runat="server">
			<ContentTemplate>
	    <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click" style="display:none"
					>Test Map</asp:LinkButton>
			</ContentTemplate>
    </asp:UpdatePanel>
				
    <asp:LinkButton ID="btnRefreshMap" runat="server" OnClick="btnRefreshMap_Click" 
				onclientclick="screenBusyOn()">Refresh Map</asp:LinkButton>
    <br />
    <asp:Label ID="lblAnalysisLayer" runat="server" Text="Analysis Layer"></asp:Label>
		<br />
        <asp:DropDownList ID="ddBoundaryType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddBoundaryType_SelectedIndexChanged">
            <asp:ListItem Value="Municipalities">Municipal Boundaries</asp:ListItem>
            <asp:ListItem Value="SchoolDivisions">School Divisions</asp:ListItem>
        </asp:DropDownList>&nbsp;
        <br />
        <asp:Label ID="lblThemeSet" runat="server" Text="Current Theme"></asp:Label>
        <br />
        <asp:DropDownList ID="ddMapThemeSet" runat="server" DataTextField="ThemeSetName" DataValueField="ThemeSetID" OnSelectedIndexChanged="ddMapThemeSet_SelectedIndexChanged" Width="100%" AutoPostBack="True">
        </asp:DropDownList><br />
        <asp:Label ID="lblShifts" runat="server" Text="Shifts"></asp:Label><br />
        <asp:CheckBoxList ID="chkShifts" runat="server" AutoPostBack="True" CssClass="checkboxList" OnSelectedIndexChanged="chkShifts_SelectedIndexChanged">
            <asp:ListItem Value="Municipal Tax" Selected="True">Municipal Tax</asp:ListItem>
            <asp:ListItem Value="Levy">Municipal Tax (Levy)</asp:ListItem>
            <asp:ListItem Value="School Tax" Selected="True">School Tax</asp:ListItem>
            <asp:ListItem Value="Assessment Value">Assessment</asp:ListItem>
            <asp:ListItem Value="Total Impact" Selected="True">Total Impact</asp:ListItem>
            <asp:ListItem Value="Total Tax" Selected="True">Total Tax</asp:ListItem>
            <asp:ListItem Value="Phase-In Amount" Selected="True">Phase-In Amount</asp:ListItem>
            <asp:ListItem Value="Minimum Tax" Selected="True">Minimum Tax</asp:ListItem>
            <asp:ListItem Value="Base Tax" Selected="True">Base Tax</asp:ListItem>
                    </asp:CheckBoxList><br />
        
        Tax Status<br />
        <asp:CheckBoxList ID="chkTaxStatuses" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chkTaxStatuses_SelectedIndexChanged" CssClass="checkboxList">
            <asp:ListItem Value="Taxable" Selected="True">Taxable</asp:ListItem>
            <asp:ListItem Value="Exempt">Exempt</asp:ListItem>
            <asp:ListItem Value="Provincial grant in lieu">Provincial grant in lieu</asp:ListItem>
            <asp:ListItem Value="Federal grant in lieu">Federal grant in lieu</asp:ListItem>
        </asp:CheckBoxList>
        <br />
        <asp:Label ID="lblTaxClasses" runat="server" Text="Tax Classes"></asp:Label>
        <uc2:ParcelThemes id="ParcelThemes1" runat="server">
        </uc2:ParcelThemes>
        &nbsp;
    </asp:View>
    <asp:View ID="ParcelThemesTab" runat="server">
        &nbsp;</asp:View>
    <asp:View ID="FiltersTab" runat="server">
       <uc1:FiltersControl id="FiltersControl1" runat="server">
        </uc1:FiltersControl>
    </asp:View>
</asp:MultiView>&nbsp;
