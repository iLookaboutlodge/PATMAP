<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master"
    Codebehind="viewpemrtiers.aspx.vb" Inherits="PATMAP.viewpemrtiers" %>

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
                        VIEW PEMR TIERS</p>
                </div>
            </div>
        </div>
        <div class="commonForm">
            <div class="btnsTop">
                <asp:ImageButton ID="ibSaveTop" runat="server" ImageUrl="~/images/btnSave.gif" />
                <asp:ImageButton ID="ibCancelTop" runat="server" ImageUrl="~/images/btnCancel.gif"
                    CausesValidation="false" />
            </div>
            <br />
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td style="width: 250px;">
                        <asp:Label ID="lblHeaderText" runat="server" Text="Edit the Provincial Education Mill Rate Tiers:"></asp:Label>
                    </td>
                    <td>
                        <asp:ImageButton ID="ibTiersHelp" runat="server" ImageUrl="~/images/btnHelp.gif"
                            CssClass="btnHelp" CommandArgument="445"></asp:ImageButton>
                    </td>
                </tr>
            </table>
            <asp:UpdatePanel ID="uplTiers" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="grdTiers" runat="server" CellPadding="3" AutoGenerateColumns="False"
                        CssClass="grdSmStyle" DataSourceID="sdsPEMRTiers" DataKeyNames="mainTaxClassID,tier">
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
                            <asp:TemplateField HeaderText="Active">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbActive" runat="server" Checked='<%# Bind("active") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="colNumeric" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Assessment Tier">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtMinAssessment" runat="server" Text='<%# Bind("minAssessment", "{0:#,##0}") %>'
                                        CssClass="txtShort"></asp:TextBox>
                                    <ajaxToolkit:FilteredTextBoxExtender ID="ftbMinAssessment" runat="server" TargetControlID="txtMinAssessment"
                                        FilterType="Custom,Numbers" ValidChars=",">
                                    </ajaxToolkit:FilteredTextBoxExtender>
                                </ItemTemplate>
                                <ItemStyle CssClass="colNumeric" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtMaxAssessment" runat="server" Text='<%# String.Concat(Eval("maxAssessment", "{0:#,##0}"), " ", Eval("maxAssessmentText")) %>'
                                        CssClass="txtShort"></asp:TextBox>
                                    <ajaxToolkit:FilteredTextBoxExtender ID="ftbMaxAssessment" runat="server" TargetControlID="txtMaxAssessment"
                                        FilterType="Custom,Numbers" ValidChars=",> ">
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
            <div class="btnsTop">
                <asp:ImageButton ID="ibSaveBottom" runat="server" ImageUrl="~/images/btnSave.gif" />
                <asp:ImageButton ID="ibCancelBottom" runat="server" ImageUrl="~/images/btnCancel.gif"
                    CausesValidation="false" />
            </div>
        </div>
    </div>
    <asp:CustomValidator ID="cvValidateTiers" runat="server" Display="None" OnServerValidate="ValidateTiers"></asp:CustomValidator>
    <asp:SqlDataSource ID="sdsPEMRTiers" runat="server" ConnectionString="<%$ ConnectionStrings:PATMAPConnection %>"
        SelectCommand="SELECT pt.*, pmtc.mainTaxClass FROM PEMRTiers pt INNER JOIN PEMRMainTaxClasses pmtc ON pt.mainTaxClassID = pmtc.mainTaxClassID ORDER BY sort, tier"
        UpdateCommand="PEMRTiersUpdate" UpdateCommandType="StoredProcedure"></asp:SqlDataSource>
</asp:Content>
