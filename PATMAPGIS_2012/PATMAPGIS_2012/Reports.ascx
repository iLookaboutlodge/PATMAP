<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Reports.ascx.cs" Inherits="Reports" %>
<script src="Reports.js"></script>
<table style="width:100%">
    <tr>
        <td align="center">
            <asp:Label ID="lblReports" runat="server" Text="Reports" CssClass="sectionHeader"></asp:Label></td>
    </tr>
</table>
<br />
<table style="width:100%">
<tr>
<td align="center">
<asp:LinkButton ID="btnOpenTables" runat=server Text="Open Tables" OnClick="btnOpenTables_Click"></asp:LinkButton>
</td>
</tr>
<tr>
<td align="center">
<asp:LinkButton ID="btnOpenGraphs" runat=server Text="Open Graphs" OnClick="btnOpenGraphs_Click"></asp:LinkButton>
</td>
</tr>
</table>
<input type="hidden" id="ParcelID" name="ParcelID" />
<input type="hidden" id="MunicipalityID" name="MunicipalityID" />
<input type="hidden" id="SchoolID" name="SchoolID" />
