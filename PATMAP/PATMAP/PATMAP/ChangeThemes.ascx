<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChangeThemes.ascx.cs" Inherits="ChangeThemes" %>
<script src="ChangeThemes.js"></script>
<table style="width:100%">
    <tr>
        <td style="width: 93px">
        </td>
        <td>
            <asp:Label ID="lblAChangeThemes" runat="server" Text="Change Themes" CssClass="sectionHeader"></asp:Label></td>
    </tr>
</table>
<asp:DropDownList ID="ddThemeSets" runat="server" DataSourceID="dsMapThemeSets"
    DataTextField="ThemeSetName" DataValueField="ThemeSetID" Width="170px" AutoPostBack="True" OnSelectedIndexChanged="ddThemeSets_SelectedIndexChanged" OnDataBound="ddThemeSets_DataBound">
</asp:DropDownList>
<asp:LinkButton ID="btnAddTheme" runat="server" OnClick="btnAddTheme_Click" OnClientClick="return getThemeName();">Add</asp:LinkButton>
&nbsp;
<asp:LinkButton ID="btnRenameTheme" runat="server" OnClick="btnRename_Click" OnClientClick="return getThemeName();">Rename</asp:LinkButton>
&nbsp;
<asp:LinkButton ID="btnDeleteTheme" runat="server" OnClick="btnDeleteTheme_Click" OnClientClick="return confirm('Are you sure you wish to delete the current theme?');">Delete</asp:LinkButton><br />
<asp:RadioButton ID="rbValue" runat="server" AutoPostBack="True" OnCheckedChanged="rbValue_CheckedChanged"
    Text="By Value" />
<br />
<asp:RadioButton ID="rbPercentage" runat="server" AutoPostBack="True" OnCheckedChanged="rbPercentage_CheckedChanged"
    Text="By Percentage" /><br />
<asp:GridView ID="gvMapThemes" runat="server" DataKeyNames="ThemeID" AutoGenerateColumns="False" DataSourceID="dsMapThemes" style="width:100%" OnRowDeleted="gvMapThemes_RowDeleted" OnRowUpdated="gvMapThemes_RowUpdated" OnRowUpdating="gvMapThemes_RowUpdating" OnInit="gvMapThemes_Init">
    <Columns>
        <asp:BoundField DataField="ThemeID" HeaderText="ThemeID" InsertVisible="False"
            ReadOnly="True" SortExpression="ThemeID" Visible="False" />
        <asp:BoundField DataField="LegendLabel" HeaderText="Legend Label" SortExpression="LegendLabel" visible=False/>
        <asp:TemplateField HeaderText="Min">
            <ItemTemplate><asp:Label id="lblMinThemeValue" runat="server" Text='<%# Eval("MinThemeValue") %>'></asp:Label></ItemTemplate>
            <EditItemTemplate><asp:TextBox id="txtMinThemeValue" runat="server" Text='<%# Bind("MinThemeValue") %>' Columns="8"></asp:TextBox>&nbsp;
                <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="txtMinThemeValue"
                    Display="Dynamic" ErrorMessage="Value out of range" MaximumValue="999999999999999999999"
                    MinimumValue="-999999999999999999999" Type="Double" Enabled="False"></asp:RangeValidator>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Max">
            <ItemTemplate><asp:Label id="lblMaxThemeValue" runat="server" Text='<%# Eval("MaxThemeValue") %>'></asp:Label></ItemTemplate>
            <EditItemTemplate><asp:TextBox id="txtMaxThemeValue" runat="server" Text='<%# Bind("MaxThemeValue") %>' Columns="8"></asp:TextBox>&nbsp;
                <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtMaxThemeValue"
                    Display="Dynamic" ErrorMessage="Value out of range" MaximumValue="999999999999999999999"
                    MinimumValue="-999999999999999999999" Type="Double" Enabled="False"></asp:RangeValidator>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="FillColorIndex" HeaderText="Fill Color" SortExpression="FillColorIndex" visible="False"/>
        <asp:Templatefield HeaderText="Colour">
            <ItemTemplate>
		          <div style="width:20px;background:<%# MapManager.convertToColour((int)Eval("FillColorIndex")) %>">&nbsp;</div>
            </ItemTemplate>
        </asp:Templatefield>
        <asp:BoundField DataField="ThemeID" HeaderText="ThemeID" SortExpression="ThemeID" Visible="False"/>
        <asp:CommandField ButtonType="Image" DeleteImageUrl="~/images/delete.png" EditImageUrl="~/images/edit.png" CancelImageUrl="~/images/cancel.png" UpdateImageUrl="~/images/update.png"
            ShowEditButton="True" ShowDeleteButton="True" />
    </Columns>
    
</asp:GridView>
<asp:LinkButton ID="btnAddThemeRow" runat="server" OnClick="btnAdd_Click">Add Row</asp:LinkButton>
<br />
<asp:SqlDataSource ID="dsMapThemeSets" runat="server" ConnectionString="<%$ ConnectionStrings:PATMAPConnection %>"
    DeleteCommand="DELETE FROM [MapThemes] WHERE [ThemeSetID] = @ThemeSetID; DELETE FROM [MapThemeSets] WHERE [ThemeSetID] = @ThemeSetID" InsertCommand="INSERT INTO [MapThemeSets] ([ThemeSetName], [UserID]) VALUES (@ThemeSetName, @UserID)"
    SelectCommand="SELECT [ThemeSetID], [ThemeSetName] FROM [MapThemeSets] WHERE ([UserID] = @UserID OR UserID = -1)"
    UpdateCommand="UPDATE [MapThemeSets] SET [ThemeSetName] = @ThemeSetName WHERE [ThemeSetID] = @ThemeSetID">
    <DeleteParameters>
        <asp:Parameter Name="ThemeSetID" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="ThemeSetName" Type="String" />
        <asp:Parameter Name="ThemeSetID" Type="Int32" />
    </UpdateParameters>
    <SelectParameters>
        <asp:SessionParameter Name="UserID" SessionField="UserID" Type="Int32" />
    </SelectParameters>
    <InsertParameters>
        <asp:Parameter Name="ThemeSetName" Type="String" />
        <asp:Parameter Name="UserID" Type="Int32" />
    </InsertParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsMapThemes" runat="server" ConnectionString="<%$ ConnectionStrings:PATMAPConnection %>"
    SelectCommand="SELECT [ThemeID], [LegendLabel], [MinThemeValue], [MaxThemeValue], [FillColorIndex], [ThemeSetID] FROM [MapThemes] WHERE ([ThemeSetID] = @ThemeSetID)" DeleteCommand="DELETE FROM [MapThemes] WHERE [ThemeID] = @ThemeID" InsertCommand="INSERT INTO [MapThemes] ([LegendLabel], [MinThemeValue], [MaxThemeValue], [FillColorIndex], [ThemeSetID]) VALUES (@LegendLabel, @MinThemeValue, @MaxThemeValue, @FillColorIndex, @ThemeSetID)" UpdateCommand="UPDATE [MapThemes] SET [LegendLabel] = @LegendLabel, [MinThemeValue] = @MinThemeValue, [MaxThemeValue] = @MaxThemeValue, [FillColorIndex] = @FillColorIndex WHERE [ThemeID] = @ThemeID" OnSelecting="dsMapThemes_Selecting"
    >
    <SelectParameters>
        <asp:SessionParameter SessionField="MapThemeID" Type="Int32" Name="ThemeSetID" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="ThemeID" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="LegendLabel" Type="String" />
        <asp:Parameter Name="MinThemeValue" Type="Double" />
        <asp:Parameter Name="MaxThemeValue" Type="Double" />
        <asp:Parameter Name="FillColorIndex" Type="Int32" />
        <asp:Parameter Name="ThemeID" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="LegendLabel" Type="String" DefaultValue="new" />
        <asp:Parameter Name="MinThemeValue" Type="Double" DefaultValue="0" />
        <asp:Parameter Name="MaxThemeValue" Type="Double" DefaultValue="0" />
        <asp:Parameter Name="FillColorIndex" Type="Int32" DefaultValue="1" />
        <asp:ControlParameter ControlID="ddThemeSets" DefaultValue="" Name="ThemeSetID" PropertyName="SelectedValue"
            Type="Int32" />
    </InsertParameters>
</asp:SqlDataSource>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
<input id="hdnFillColorIndex" type="hidden" name="hdnFillColorIndex" />&nbsp;
<input id="hdnThemeSetName" name="hdnThemeSetName" type="hidden" /><br />
<asp:LinkButton ID="btnRefreshMap" runat="server" OnClick="btnRefreshMap_Click" 
	Visible="False">Refresh Map</asp:LinkButton>
