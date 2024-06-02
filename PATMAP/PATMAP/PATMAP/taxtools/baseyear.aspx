<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="baseyear.aspx.vb" Inherits="PATMAP.baseyear" %>
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
                    <td style="width:35%">
                        <asp:Label ID="lblSubjYr" runat="server" Text="Subject Year: " CssClass="labelTaxModel"></asp:Label> &nbsp;
                        <asp:Label ID="lblLiveSubjYr" runat="server" Text=""></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblSubjMun" runat="server" Text="Subject Municipality: " CssClass="labelTaxModel"></asp:Label> &nbsp;
                        <asp:Label ID="lblLiveSubjMun" runat="server" Text=""></asp:Label>
                    </td>         
                </tr>
            </table>
            <br />               
            <div class="PageHeaderModule">
                <div class="Header">
                    <p class="Title">Base Year Revenue and Tax Tool Detail</p>
                </div>
            </div>
        </div>        
        <div class="commonForm">
            <asp:UpdatePanel id="uplBaseYear" runat="server" >
                <ContentTemplate>
                    <table cellspacing="0" cellpadding="3" width="70%" class="displayRevisions" rules="all" border="1" style="border-collapse:collapse; border:solid 1px snow">
                        <tr class="colHeader">
                            <td style="text-align:left; text-indent:5px"><asp:Label ID="lblRevenueSummary" runat="server" Text="Revenue Summary"></asp:Label></td>
                            <td><asp:Label ID="lblSubjYrDetails" runat="server" Text="Base Year"></asp:Label></td>
                            <td style="width:66px"><asp:Label ID="lblEdit" runat="server" Text="Edit"></asp:Label></td>
                        </tr>
                        <tr>
                            <td style="text-align:left; height:30px; text-indent:5px"><asp:Label ID="lblUniformMillRate" runat="server" Text="Uniform Mill Rate"></asp:Label></td>
                            <td><asp:TextBox ID="txtBaseYrUniformMillRate" runat="server" Width="188px" style="text-align:right" Height="15px" Enabled="False">0.0000</asp:TextBox></td>
                            <td><asp:RadioButton ID="rdoEditUniformMillRate" runat="server" GroupName="editValues" Height="20px" AutoPostBack="True" /></td>
                        </tr>
                        <tr class="alternateRowLTT">
                            <td style="text-align:left; height:30px; text-indent:5px"><asp:Label ID="lblMunicipalRevenue" runat="server" Text="Municipal Revenue"></asp:Label></td>
                            <td><asp:TextBox ID="txtBaseYrMunicipalRevenue" runat="server" Width="188px" style="text-align:right" Wrap="False" Enabled="False">$00,000,000.00</asp:TextBox></td>
                            <td><asp:RadioButton ID="rdoEditMunicipalRevenue" runat="server" GroupName="editValues" AutoPostBack="True" /></td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
            <br />
            <br />
            
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <p><b>
                        <asp:Label ID="lblLTTScheme" runat="server" Text="Local Tax Tool Scheme" Width="241px"></asp:Label>
                        <asp:Label ID="lblBaseYr" runat="server" Text="Base Year"></asp:Label>
                        </b></p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:UpdatePanel ID="uplLTTScheme" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="grdLTTScheme" runat="server" CellPadding="3" AutoGenerateColumns="False" CssClass="grdSmStyle" DataKeyNames="taxClassID">
                                    <Columns>
                                        <asp:BoundField HeaderText="Tax Class" ShowHeader="False" DataField="taxClass">
                                            <ItemStyle HorizontalAlign="Left" cssClass="taxClassIndent" />
                                        </asp:BoundField>
                                        
                                        <asp:TemplateField HeaderText="Base Tax">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtBaseTax" runat="server" Width="75" Text='<%# DataBinder.Eval(Container.DataItem,"baseBaseTax") %>' Enabled="true" AutoPostBack="false"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Minimum Tax">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtMinTax" runat="server" Width="75" Text='<%# DataBinder.Eval(Container.DataItem,"baseMinTax") %>' Enabled="true" AutoPostBack="false"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="Mill Rate Factor">
                                            <ItemTemplate >
                                                <asp:TextBox ID="txtMRF" runat="server" Width="75" Text='<%# DataBinder.Eval(Container.DataItem,"baseMRF", "{0:F4}") %>' Enabled="true" AutoPostBack="false"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="colHeader" />
                                    <AlternatingRowStyle CssClass="alternateRowLTT" />
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:ImageButton ID="btnSubmit" runat="server" ImageUrl="~/images/btnSubmit.gif" />
                        <asp:ImageButton ID="btnClear" runat="server" ImageUrl="~/images/btnClear.gif" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>