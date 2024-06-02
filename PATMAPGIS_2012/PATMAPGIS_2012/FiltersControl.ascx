<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FiltersControl.ascx.cs" Inherits="FiltersControl" %>
<asp:CheckBoxList ID="chkPropertyClasses" runat="server" DataSourceID="dsTaxClasses" DataTextField="taxClass" DataValueField="taxClassID" OnDataBound="chkPropertyClasses_DataBound" AutoPostBack="True" OnSelectedIndexChanged="chkPropertyClasses_SelectedIndexChanged">
</asp:CheckBoxList>
<asp:SqlDataSource ID="dsTaxClasses" runat="server" ConnectionString="<%$ ConnectionStrings:PATMAPConnection %>"
    SelectCommand="SELECT [taxClassID], [taxClass] FROM [taxClasses] WHERE ([parentTaxClassID] = @parentTaxClassID) ORDER BY [taxClass]">
    <SelectParameters>
        <asp:Parameter DefaultValue="none" Name="parentTaxClassID" Type="String" />
    </SelectParameters>
</asp:SqlDataSource>
