<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="mrfactors.aspx.vb" Inherits="PATMAP.mrfactors" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %> 
<%@ Register TagPrefix="patmap" TagName="taxtoolsTabMenu" Src="~/tabmenu.ascx" %>

<asp:Content ID="tabMenu" ContentPlaceHolderID="tabMenu" runat="server">   
    <patmap:taxtoolsTabMenu ID="subMenu" runat="server" />
</asp:Content>

<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server">
    <div class="commonContent">  
        <div class="commonHeader">
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="width:35%; height:25px">
                        <asp:Label ID="lblSubjYr" runat="server" Text="Subject Year: " CssClass="labelTaxModel"></asp:Label> &nbsp;
                        <asp:Label ID="lblLiveSubjYr" runat="server" Text="2008" />
                    </td>
                    <td>
                        <asp:Label ID="lblSubjMun" runat="server" Text="Subject Municipality: " CssClass="labelTaxModel"></asp:Label> &nbsp;
                        <asp:Label ID="lblLiveSubjMun" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblStartingUniformMillRate" runat="server" Text="Starting Uniform Mill Rate: " CssClass="labelTaxModel"></asp:Label> &nbsp;
                        <asp:Label ID="lblLiveStartingUniformMillRate" runat="server" Text="0.0000"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblStartingRevenue" runat="server" Text="Starting Revenue: " CssClass="labelTaxModel"></asp:Label> &nbsp;
                        <asp:Label ID="lblLiveStartingRevenue" runat="server" Text="$00,000,000"></asp:Label>
                    </td>
                </tr>
            </table>
            <br />              
            <div class="PageHeaderModule">
                <div class="Header">
                    <p class="Title">Mill Rate Factors</p>
                </div>
            </div>
        </div>
        <div class="commonForm">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td>
                    <asp:UpdatePanel ID="uplMRFactors" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="grdMRFactor" runat="server" AutoGenerateColumns="False" CssClass="grdSmStyle" Width="90%" CellPadding="3" DataKeyNames="taxClassID">
                                <Columns>
                                    <asp:BoundField HeaderText="Tax Class" DataField="taxClass" >
                                        <ItemStyle Width="150px" HorizontalAlign="Left" cssClass="taxClassIndent" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Base Year MR Factors">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtBaseMRF" runat="server" Width="100px" Enabled="false" Text='<%# DataBinder.Eval(Container.DataItem,"baseMRF", "{0:F4}") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Model MR Factors">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtModelMRF" runat="server" Width="100px" Text='<%# DataBinder.Eval(Container.DataItem,"modelMRF", "{0:F4}") %>' AutoPostBack="false"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="colHeader" />
                                <AlternatingRowStyle CssClass="alternateRowLTT" />
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblRevNeutral" runat="server" Text="Revenue Neutral" />
                    <asp:RadioButton ID="rdoRevNeutralYes" runat="server" GroupName="RevNeutral" Text=" Yes" />
                    <asp:RadioButton ID="rdoRevNeutralNo" runat="server" GroupName="RevNeutral" Text=" No" />
                    <asp:ImageButton ID="btnHRevenueNeutral" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp" />
                </td>
            </tr>
            <tr>
                <td>
                    <br /><br />
                    <asp:ImageButton ID="btnSubmit" runat="server" ImageUrl="~/images/btnSubmit.gif" />
                    <asp:ImageButton ID="btnClear" runat="server" ImageUrl="~/images/btnClear.gif" /> 
                    <hr />
                </td>
            </tr>
        </table>
        <table cellspacing="1" cellpadding="3" border="0" class="displayRevisions">
            <tr>
                <td class="revisedTotal">
                    <asp:Label ID="lblRevisedUniformMillRate" runat="server" Text="&nbsp;&nbsp;Revised Uniform Mill Rate" Width="220px"></asp:Label>
                    <asp:Textbox ID="txtRevisedUniformMillRateValue" runat="server" Text="0.0000" Width="100px" Enabled="false" style="text-align:right" ></asp:Textbox>
                </td>
            </tr>
            <tr>
                <td class="revisedTotal">
                    <asp:Label ID="lblRevisedModelRevenue" runat="server" Text="&nbsp;&nbsp;Revised Model Revenue" Width="220px" ></asp:Label>
                    <asp:Textbox ID="txtRevisedModelRevenueValue" runat="server" Text="$ 00,000,000" Width="100px" Enabled="false" style="text-align:right" ></asp:Textbox>
                </td>
            </tr>
        </table>
        </div>
    </div>
</asp:Content>
