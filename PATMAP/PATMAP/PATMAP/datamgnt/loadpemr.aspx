<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master"
    Codebehind="loadpemr.aspx.vb" Inherits="PATMAP.loadpemr" %>

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
                        PROVINCIAL EDUCATION MILL RATE</p>
                </div>
            </div>
        </div>
        <div class="commonForm">
            <div class="btnsTop">
                <asp:ImageButton ID="ibSave" runat="server" ImageUrl="~/images/btnSave.gif" />
            </div>
            <p>
                Fields marked with an asterisk (<span class="requiredField">*</span>) are required.</p>
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td class="label">
                        <asp:Label ID="lblDataSet" runat="server" Text="Data Set Name" AssociatedControlID="txtDataSet"></asp:Label>
                        <span class="requiredField">*</span>
                    </td>
                    <td class="field">
                        <asp:TextBox ID="txtDataSet" runat="server" CssClass="txtLong"></asp:TextBox>
                        <asp:ImageButton ID="ibNameHelp" runat="server" ImageUrl="~/images/btnHelp.gif" CssClass="btnHelp"
                            Visible="false" CommandArgument="444"></asp:ImageButton>
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label ID="lblYear" runat="server" Text="Year" AssociatedControlID="ddlYear"></asp:Label>
                        <span class="requiredField">*</span>
                    </td>
                    <td class="field">
                        <asp:DropDownList ID="ddlYear" runat="server">
                        </asp:DropDownList>
                        <asp:ImageButton ID="ibYearHelp" runat="server" ImageUrl="~/images/btnHelp.gif" CssClass="btnHelp"
                            CommandArgument="448"></asp:ImageButton>
                    </td>
                </tr>
            </table>
            <asp:UpdatePanel ID="uplPEMR" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="grdPEMR" runat="server" CellPadding="3" AutoGenerateColumns="False"
                        CssClass="grdSmStyle" DataSourceID="sdsTiers" DataKeyNames="mainTaxClassID, tier">
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
                                    <asp:TextBox ID="txtMillRate" runat="server" CssClass="txtShort" MaxLength="6"></asp:TextBox>
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
    <asp:CustomValidator ID="cvPage" runat="server" Display="None" OnServerValidate="ValidatePage"></asp:CustomValidator>
    <asp:SqlDataSource ID="sdsTiers" runat="server" ConnectionString="<%$ ConnectionStrings:PATMAPConnection %>"
        SelectCommand="SELECT pmtc.mainTaxClassID, pmtc.mainTaxClass, pt.tier, pt.minAssessment, pt.maxAssessment, pt.maxAssessmentText FROM PEMRTiers pt INNER JOIN PEMRMainTaxClasses pmtc ON pt.mainTaxClassID = pmtc.mainTaxClassID WHERE pt.active = 1 ORDER BY pmtc.sort, pt.tier"
        UpdateCommand="PEMRInsert" UpdateCommandType="StoredProcedure"></asp:SqlDataSource>
</asp:Content>
