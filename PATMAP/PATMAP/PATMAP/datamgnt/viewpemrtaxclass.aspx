<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master"
    Codebehind="viewpemrtaxclass.aspx.vb" Inherits="PATMAP.viewpemrtaxclass" %>

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
                        View PEMR Tax Classes</p>
                </div>
            </div>
        </div>
        <div class="commonForm">
            <div class="btnsTop">
                <asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/images/btnAddClass.gif" PostBackUrl="editpemrtaxclass.aspx" /></div>
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td class="label">
                        <asp:Label ID="lbltaxclass" runat="server" Text="PEMR Tax Classes"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:DropDownList ID="ddlTaxClass" runat="server" AppendDataBoundItems="true">
                            <asp:ListItem Text="<Select>" Value="" Selected="true"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:ImageButton ID="btnHPEMRTaxClasses" runat="server" ImageUrl="~/images/btnHelp.gif"
                            Visible="False" CssClass="btnHelp"></asp:ImageButton>
                        &nbsp;&nbsp;
                        <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/btnSearch.gif" /></td>
                </tr>
            </table>
        </div>
        <div class="subHeader">
            LIST</div>
        <asp:UpdatePanel ID="uplTaxClass" runat="server">
            <contenttemplate>
                <div class="totalCount">
                    <asp:Label ID="lblTotal" runat="server" Text="PEMR Tax Classes: "></asp:Label>
                    <asp:Label
                        ID="txtTotal" runat="server" Text=""></asp:Label></div>
                <asp:GridView ID="grdTaxClass" runat="server" CellPadding="3" AutoGenerateColumns="False"
                    CssClass="grdLgStyle" AllowPaging="True" DataKeyNames="mainTaxClassID">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="ibEdit" runat="server" ImageUrl="../images/btnSmEdit.gif" CommandName="Edit" />
                                <asp:ImageButton ID="ibDelete" runat="server" ImageUrl="../images/btnSmDelete.gif"
                                    Visible='<%# Eval("isUserDefined") %>' CommandName="Delete" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="PEMR Main Tax Classes" DataField="mainTaxClass">
                            <ItemStyle Width="85%" />
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <%--<asp:BoundField HeaderText="Sort Order" DataField="sort" Visible="False">
                            <ItemStyle Width="25%" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Active" DataField="active" Visible="False">
                            <ItemStyle Width="25%" />
                        </asp:BoundField>--%>
                    </Columns>
                    <HeaderStyle CssClass="colHeader" />
                    <AlternatingRowStyle CssClass="alertnateRow" />
                    <PagerSettings Position="Top" />
                    <PagerStyle CssClass="grdPage" />
                </asp:GridView>
            </contenttemplate>
            <triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="grdTaxClass" EventName="PageIndexChanging" />
                <asp:AsyncPostBackTrigger ControlID="grdTaxClass" EventName="RowCommand" />
                <asp:AsyncPostBackTrigger ControlID="grdTaxClass" EventName="Sorting" />
            </triggers>
        </asp:UpdatePanel>
    </div>
    <asp:SqlDataSource ID="sdsPEMRMainTaxClasses" runat="server" SelectCommand="PEMRMainTaxClassesSelect"
        SelectCommandType="StoredProcedure" CancelSelectOnNullParameter="False" DeleteCommandType="StoredProcedure"
        DeleteCommand="PEMRMainTaxClassesDelete" ConnectionString="<%$ ConnectionStrings:PATMAPConnection %>">
        <SelectParameters>
            <asp:ControlParameter Name="mainTaxClassID" ControlID="ddlTaxClass" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="mainTaxClassID" />
        </DeleteParameters>
    </asp:SqlDataSource>
</asp:Content>
