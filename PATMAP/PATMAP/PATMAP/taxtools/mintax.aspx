<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="mintax.aspx.vb" Inherits="PATMAP.mintax" %>
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
                    <p class="Title">Minimum Tax</p>
                </div>
            </div>
        </div>
        <div class="commonForm">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td>
                    <asp:UpdatePanel ID="uplMinTax" runat="server" rendermode="Inline">
                        <ContentTemplate>
                            <asp:GridView ID="grdMinTax" runat="server" AutoGenerateColumns="False" CssClass="grdSmStyle" Width="90%" CellPadding="3" DataKeyNames="taxClassID">
                                <Columns>
                                    <asp:BoundField HeaderText="Tax Class" DataField="taxClass" >
                                        <ItemStyle Width="150px" HorizontalAlign="Left" cssClass="taxClassIndent" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Base Year Minimum Tax">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtBaseMinTax" runat="server" Width="100px" Enabled="false" Text='<%# DataBinder.Eval(Container.DataItem,"baseMinTax") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Model Minimum Tax">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtModelMinTax" runat="server" Width="100px" Text='<%# DataBinder.Eval(Container.DataItem,"modelMinTax") %>' AutoPostBack="false"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Revenue Neutral">
                                        <ItemTemplate>
                                            <asp:RadioButtonList ID="rdlRevenueNeutral" runat="server" AutoPostBack="false" TextAlign="right">
                                                <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                                                <asp:ListItem Text=" &nbsp;No" Value="False"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="colHeader" />
                                <AlternatingRowStyle CssClass="alternateRowLTT" />
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:ImageButton ID="btnSubmit" runat="server" ImageUrl="~/images/btnSubmit.gif" />
                    <asp:ImageButton ID="btnReset" runat="server" ImageUrl="~/images/btnClear.gif" /> 
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
