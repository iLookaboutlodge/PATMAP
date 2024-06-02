<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    Codebehind="pemr.aspx.vb" Inherits="PATMAP.pemr" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register TagPrefix="patmap" TagName="assmntTabMenu" Src="~/tabmenu.ascx" %>
<%@ Register TagPrefix="patmap" TagName="header" Src="~/assmnt/controls/header.ascx" %>
<asp:Content ID="tabMenu" ContentPlaceHolderID="tabMenu" runat="server">
    <patmap:assmntTabMenu ID="subMenu" runat="server" />
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server">
    <div class="commonContent">
        <patmap:header ID="ucHeader" runat="server" Title="PROVINCIAL EDUCATION MILL RATE" />
        <div class="commonForm">
            <b>Scenario Provincial Education Mill Rate</b>
            <asp:UpdatePanel ID="uplPEMR" runat="server">
                <ContentTemplate>
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td>
                                <asp:RadioButton ID="rbEnter" runat="server" Text="Enter / Edit Education Mill Rates"
                                    Checked="true" OnCheckedChanged="MillRateOptionChanged" GroupName="MillRates"
                                    AutoPostBack="true" />
                            </td>
                            <td>
                                <asp:RadioButton ID="rbCalculate" runat="server" Text="Calculate Revenue Neutral Education Mill Rates"
                                    OnCheckedChanged="MillRateOptionChanged" GroupName="MillRates" AutoPostBack="true" />
                                <asp:ImageButton ID="ibMillRateHelp" runat="server" ImageUrl="~/images/btnHelp.gif"
                                    Visible="False" CssClass="btnHelp" CommandArgument="437"></asp:ImageButton>
                            </td>
                        </tr>
                        <tr id="levy" runat="server" visible="false">
                            <td>
                                <asp:RadioButton ID="rbTotalLevy" runat="server" Text="Revenue Neutral Rates by Total Levy"
                                    GroupName="Levy" />
                            </td>
                            <td>
                                <asp:RadioButton ID="rbClassLevy" runat="server" Text="Revenue Neutral Rates by Class Levy"
                                    GroupName="Levy" />
                                <asp:ImageButton ID="ibLevyHelp" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False"
                                    CssClass="btnHelp" CommandArgument="438"></asp:ImageButton>
                            </td>
                        </tr>
                    </table>
                    <asp:GridView ID="grdPEMR" runat="server" AutoGenerateColumns="False" CssClass="grdSmStyle"
                        DataKeyNames="mainTaxClassID" DataSourceID="sdsPEMR">
                        <Columns>
                            <asp:TemplateField HeaderText="Tax Class">
                                <ItemTemplate>
                                    <asp:Label ID="lblTaxClass" runat="server" Text='<%# Eval("mainTaxClass") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tier">
                                <ItemTemplate>
                                    <asp:Label ID="lblTier" runat="server" Text='<%# Bind("tier") %>'></asp:Label>
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
                                    <asp:Label ID="lblMaxAssessment" runat="server" Text='<%# String.Concat(DataBinder.Eval(Container.DataItem, "maxAssessment", "{0:#,##0}"), DataBinder.Eval(Container.DataItem, "maxAssessmentText")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Base Year PEMR">
                                <ItemTemplate>
                                    <asp:Label ID="lblBasePEMR" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "basePEMR", "{0:##0.00}") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="colFormattedNumeric" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Subject Year PEMR">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtSubjectPEMR" runat="server" Text='<%# Bind("subjectPEMR", "{0:##0.00}") %>'
                                        CssClass="txtShort" MaxLength="6"></asp:TextBox>
                                    <ajaxToolkit:FilteredTextBoxExtender ID="ftbSubjectPEMR" runat="server" TargetControlID="txtSubjectPEMR"
                                        FilterType="Custom,Numbers" ValidChars=".">
                                    </ajaxToolkit:FilteredTextBoxExtender>
                                </ItemTemplate>
                                <ItemStyle CssClass="colNumeric" />
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="colHeader" />
                        <AlternatingRowStyle CssClass="alternateEditRow" />
                    </asp:GridView>
                    <br />
                    <asp:ImageButton ID="ibSubmit" runat="server" ImageUrl="~/images/btnSubmit.gif" />
                    <asp:ImageButton ID="ibClear" runat="server" ImageUrl="~/images/btnClear.gif" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <asp:CustomValidator ID="cvSubjectPEMR" runat="server" Display="None" OnServerValidate="ValidateSubjectPEMR"></asp:CustomValidator>
    <asp:SqlDataSource ID="sdsPEMR" runat="server" ConnectionString="<%$ ConnectionStrings:PATMAPConnection %>"
        SelectCommand="SELECT DISTINCT pmtc.mainTaxClassID, pmtc.mainTaxClass, subject.tier, pt.minAssessment, pt.maxAssessment, pt.maxAssessmentText, base.PEMR * 1000 AS basePEMR, subject.PEMR * 1000 AS subjectPEMR, pmtc.sort, base.tier FROM PEMRTiers pt INNER JOIN PEMRMainTaxClasses pmtc ON pt.mainTaxClassID = pmtc.mainTaxClassID INNER JOIN PEMRTaxClasses ptc ON pt.mainTaxClassID = ptc.mainTaxClassID LEFT OUTER JOIN livePEMR subject ON ptc.taxClassID = subject.taxClassID AND pt.tier = subject.tier LEFT OUTER JOIN PEMR base ON ptc.taxClassID = base.taxClassID AND pt.tier = base.tier AND base.PEMRID = (SELECT PEMRID FROM taxYearModelDescription WHERE taxYearModelID = (SELECT liveassessmenttaxmodel.baseTaxYearModelID FROM liveAssessmentTaxModel WHERE userID = @userID)) INNER JOIN taxClassesPermission tcp ON ptc.taxClassID = tcp.taxClassID WHERE subject.userID = @userID AND pt.active = 1 AND tcp.levelID = @levelID AND access = 1 ORDER BY pmtc.sort, base.tier"
        UpdateCommand="livePEMRUpdate" UpdateCommandType="StoredProcedure">
        <SelectParameters>
            <asp:Parameter Name="userID" Type="Int32" />
            <asp:Parameter Name="levelID" Type="Int32" />
        </SelectParameters>
        <UpdateParameters>
            <asp:SessionParameter Name="userID" Type="Int32" SessionField="userID" />
        </UpdateParameters>
    </asp:SqlDataSource>
</asp:Content>
