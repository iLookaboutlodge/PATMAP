<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ParcelThemes.ascx.cs" Inherits="ParcelThemes" %>
<asp:Table ID="tblParcelTypes" runat="server" CssClass="parcelThemeRow" CellPadding="0" CellSpacing="0">
</asp:Table>
<asp:SqlDataSource ID="dsTaxClasses" runat="server" ConnectionString="<%$ ConnectionStrings:PATMAPConnection %>"
    SelectCommand="SELECT [taxClassID], [taxClass] FROM [taxClasses] WHERE ([parentTaxClassID] = @parentTaxClassID) ORDER BY [taxClass]">
    <SelectParameters>
        <asp:Parameter DefaultValue="none" Name="parentTaxClassID" Type="String" />
    </SelectParameters>
</asp:SqlDataSource>