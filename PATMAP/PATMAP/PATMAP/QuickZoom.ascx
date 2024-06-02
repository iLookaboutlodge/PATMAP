<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuickZoom.ascx.cs" Inherits="QuickZoom" %>
<table style="width:100%">
    <tr>
           <td align="center"><asp:Label ID="lblQuickZoom" runat="server" Text="Quick Zoom" CssClass="sectionHeader"></asp:Label></td>
    </tr>
    <tr>
        <td >
            Municipality</td>
    </tr>
    <tr style="width: 100%">
        <td>
            <asp:DropDownList ID="ddMunicipalities" runat="server" Width="100%" AppendDataBoundItems="True" AutoPostBack="True" DataSourceID="dsMunicipalities" DataTextField="jurisdiction" DataValueField="number" OnSelectedIndexChanged="ddMunicipalities_SelectedIndexChanged">
                <asp:ListItem Value="-1">-Select Municipality-</asp:ListItem>
            </asp:DropDownList></td>
    </tr>
    <tr id="schoolDivision" runat="server">
        <td style="width: 100%">
            School Division</td>
    </tr>
    <tr>
        <td>
            <asp:DropDownList ID="ddSchoolDistricts" runat="server" OnSelectedIndexChanged="ddSchoolDistricts_SelectedIndexChanged" Width="100%" AppendDataBoundItems="True" AutoPostBack="True" DataSourceID="dsSchoolDistricts" DataTextField="jurisdiction" DataValueField="number">
                <asp:ListItem Value="-1">-Select School Division-</asp:ListItem>
            </asp:DropDownList></td>
    </tr>
    <tr>
        <td style="width: 100%">
            Constituencies</td>
    </tr>
    <tr>
        <td>
            <asp:DropDownList ID="ddConstituencyBoundaries" runat="server" Width="100%" AppendDataBoundItems="True" AutoPostBack="True" DataSourceID="dsConstituencyBoundaries" DataTextField="CON_NAME" DataValueField="CON_NUM" OnSelectedIndexChanged="ddConstituencyBoundaries_SelectedIndexChanged">
                <asp:ListItem Value="-1">-Select Constituency-</asp:ListItem>
            </asp:DropDownList></td>
    </tr>
</table>
<asp:SqlDataSource ID="dsMunicipalities" runat="server" ConnectionString="<%$ ConnectionStrings:PATMAPConnection %>"
    SelectCommand="SELECT DISTINCT number, CASE WHEN MunID IS NULL THEN jurisdiction + '(not on map)' ELSE jurisdiction END jurisdiction, entities.jurisdictionTypeID FROM entities INNER JOIN jurisdictionTypes ON entities.jurisdictionTypeID = jurisdictionTypes.jurisdictionTypeID LEFT OUTER JOIN [PATMAP].[dbo].[MunicipalitiesMapLink]	ON [SAMA_Code] = number LEFT OUTER JOIN Municipalities ON MunID = [PPID] WHERE entities.jurisdictionTypeID > 1 ORDER BY entities.jurisdictionTypeID ASC, jurisdiction ASC">
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsSchoolDistricts" runat="server" ConnectionString="<%$ ConnectionStrings:PATMAPConnection %>"
    SelectCommand="SELECT DISTINCT number, CASE WHEN SD_NUM IS NULL THEN jurisdiction + '(not on map)' ELSE jurisdiction END jurisdiction FROM entities LEFT OUTER JOIN SchoolDivisions ON SD_NUM = number WHERE jurisdictionTypeID = 1 ORDER BY jurisdiction">
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsConstituencyBoundaries" runat="server" ConnectionString="<%$ ConnectionStrings:PATMAPConnection %>"
    SelectCommand="SELECT DISTINCT [CON_NUM], [CON_NAME] FROM ConstituencyBoundaries">
</asp:SqlDataSource>
&nbsp;&nbsp;
