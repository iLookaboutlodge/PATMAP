<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master"
    Codebehind="editpemr.aspx.vb" Inherits="PATMAP.editpemr" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register TagPrefix="patmap" TagName="dataTabMenu" Src="~/tabmenu.ascx" %>
<asp:Content ID="tabMenu" ContentPlaceHolderID="tabMenu" runat="server">
    <patmap:dataTabMenu ID="subMenu" runat="server"></patmap:dataTabMenu>
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server">
    <div class="commonContent">
        <div class="commonHeader">
            <div class="PageHeaderModule">
                <div class="Header">
                    <p class="Title">
                        Edit PEMR</p>
                </div>
            </div>
        </div>
        <div class="commonForm">
            <div class="btnsTop">
                <asp:ImageButton ID="ibSave" runat="server" ImageUrl="~/images/btnSave.gif" />
                <asp:ImageButton ID="ibCancel" runat="server" ImageUrl="~/images/btnCancel.gif" CausesValidation="false" />
            </div>
            <br />
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td class="label">
                        <asp:Label ID="lblDataSet" runat="server" Text="Data Set Name" AssociatedControlID="txtDataSet"></asp:Label>
                    </td>
                    <td class="field">
                        <asp:TextBox ID="txtDataSet" runat="server" Enabled="false" CssClass="txtLong"></asp:TextBox>
                        <asp:ImageButton ID="ibDataSetHelp" runat="server" ImageUrl="~/images/btnHelp.gif"
                            CssClass="btnHelp" CommandArgument="442"></asp:ImageButton>
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label ID="lblYear" runat="server" Text="Year" AssociatedControlID="ddlYear"></asp:Label>
                    </td>
                    <td class="field">
                        <asp:DropDownList ID="ddlYear" runat="server" Enabled="false">
                        </asp:DropDownList>
                        <asp:ImageButton ID="ibYearHelp" runat="server" ImageUrl="~/images/btnHelp.gif" CssClass="btnHelp"
                            CommandArgument="443"></asp:ImageButton>
                    </td>
                </tr>
            </table>
            <asp:UpdatePanel ID="uplPEMR" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="grdPEMR" runat="server" CellPadding="3" AutoGenerateColumns="False"
                        CssClass="grdSmStyle" DataSourceID="sdsPEMR" DataKeyNames="mainTaxClassID,tier">
                        <Columns>
                            <asp:TemplateField HeaderText="Tax Class">
                                <ItemTemplate>
                                    <asp:Label ID="lblTaxClass" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "mainTaxClass") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tier">
                                <ItemTemplate>
                                    <asp:Label ID="lblTier" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "tier") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="colNumeric" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Assessment Tier">
                                <ItemTemplate>
                                    <asp:Label ID="lblMinAssessment" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "minAssessment", "{0:#,##0}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="lblMaxAssessment" runat="server" Text='<%# String.Concat(DataBinder.Eval(Container.DataItem, "maxAssessment", "{0:#,##0}"), " ", DataBinder.Eval(Container.DataItem, "maxAssessmentText")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Mill Rate">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtMillRate" runat="server" Text='<%# Bind("PEMR", "{0:##0.00}") %>'
                                        CssClass="txtShort" MaxLength="6"></asp:TextBox>
                                    <ajaxToolkit:FilteredTextBoxExtender ID="ftbMillRate" runat="server" TargetControlID="txtMillRate"
                                        FilterType="Custom,Numbers" ValidChars=".">
                                    </ajaxToolkit:FilteredTextBoxExtender>
                                </ItemTemplate>
                                <ItemStyle CssClass="colNumeric" />
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="colHeader" />
                        <AlternatingRowStyle CssClass="alertnateRow" />
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <asp:CustomValidator ID="cvMillRate" runat="server" Display="None" OnServerValidate="ValidateMillRate"></asp:CustomValidator>
    <asp:SqlDataSource ID="sdsPEMR" runat="server" ConnectionString="<%$ ConnectionStrings:PATMAPConnection %>"
        SelectCommand="SELECT DISTINCT pt.tier, pt.minAssessment, pt.maxAssessment, pt.maxAssessmentText, p.PEMR * 1000 AS PEMR, pmtc.mainTaxClass, pmtc.mainTaxClassID, pmtc.sort FROM PEMRTiers pt INNER JOIN PEMRMainTaxClasses pmtc ON pt.mainTaxClassID = pmtc.mainTaxClassID INNER JOIN PEMRTaxClasses ptc ON pmtc.mainTaxClassID = ptc.mainTaxClassID LEFT OUTER JOIN PEMR p ON ptc.taxClassID = p.taxClassID AND pt.tier = p.tier WHERE pt.active = 1 AND PEMRID = @PEMRID ORDER BY pmtc.sort, pt.tier"
        UpdateCommand="PEMRUpdate" UpdateCommandType="StoredProcedure">
        <SelectParameters>
            <asp:QueryStringParameter Type="Int32" Name="PEMRID" QueryStringField="PEMRID" />
        </SelectParameters>
        <UpdateParameters>
            <asp:QueryStringParameter Name="PEMRID" Type="Int32" QueryStringField="PEMRID" />
        </UpdateParameters>
    </asp:SqlDataSource>
</asp:Content>
