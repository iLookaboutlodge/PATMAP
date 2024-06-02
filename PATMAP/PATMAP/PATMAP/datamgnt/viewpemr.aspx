<%@ Page Language="vb" ValidateRequest="false" AutoEventWireup="false" MasterPageFile="~/MasterPage.master"
    Codebehind="viewpemr.aspx.vb" Inherits="PATMAP.viewpemr" %>

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
                        View PEMR</p>
                </div>
            </div>
        </div>
        <div class="commonForm">
           <%-- <div class="btnsTop">
                <asp:ImageButton ID="ibAdd" runat="server" ImageUrl="~/images/btnAdd.gif" />
            </div>--%>
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td class="label">
                        <asp:Label ID="lblDataSet" runat="server" Text="Data Set Name"></asp:Label>
                    </td>
                    <td class="field">
                        <asp:DropDownList ID="ddlDataSet" runat="server" EnableViewState="true" DataSourceID="sdsDataSets"
                            DataValueField="PEMRID" DataTextField="dataSetName" AppendDataBoundItems="true">
                            <asp:ListItem Text="Select Data" Value="" Selected="true"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:ImageButton ID="ibDataSetHelp" runat="server" ImageUrl="~/images/btnHelp.gif"
                            Visible="False" CssClass="btnHelp" CommandArgument="439"></asp:ImageButton>
                        <asp:ImageButton ID="btnHDSN" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False"
                            CssClass="btnHelp"></asp:ImageButton>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label ID="lblYear" runat="server" Text="Year" AssociatedControlID="ddlYear"></asp:Label>
                    </td>
                    <td class="field">
                        <asp:DropDownList ID="ddlYear" runat="server">
                        </asp:DropDownList>
                        <asp:ImageButton ID="ibYearHelp" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False"
                            CssClass="btnHelp" CommandArgument="440"></asp:ImageButton>
                    </td>
                    <td class="btns">
                        <asp:ImageButton ID="ibSearch" runat="server" ImageUrl="~/images/btnSearch.gif" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="subHeader">
            List
        </div>
        <asp:UpdatePanel ID="uplPEMR" runat="server">
            <ContentTemplate>
                <div class="totalCount">
                    <asp:Label ID="lblTotal" runat="server" Text="PEMR: "></asp:Label>
                </div>
                <asp:GridView ID="grdPEMR" runat="server" AutoGenerateColumns="False" CssClass="grdLgStyle"
                    AllowPaging="True" AllowSorting="True" DataKeyNames="PEMRID">
                    <Columns>
                        <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmEdit.gif&quot; /&gt;" CommandName="Edit">
                            <ItemStyle CssClass="colEdit" />
                        </asp:ButtonField>
                        <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmDelete.gif&quot; /&gt;" CommandName="Delete">
                            <ItemStyle CssClass="colDelete" />
                        </asp:ButtonField>
                        <asp:BoundField HeaderText="Data Set Name" DataField="dataSetName" SortExpression="dataSetName" />
                        <asp:BoundField HeaderText="Year" DataField="year" SortExpression="year">
                            <ItemStyle CssClass="colNumeric" />
                        </asp:BoundField>
                    </Columns>
                    <HeaderStyle CssClass="colHeader" />
                    <AlternatingRowStyle CssClass="alertnateRow" />
                    <PagerSettings Position="Top" />
                    <PagerStyle CssClass="grdPage" />
                </asp:GridView>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ibSearch" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <asp:SqlDataSource ID="sdsDataSets" runat="server" SelectCommand="SELECT PEMRID, dataSetName FROM PEMRDescription WHERE statusID = 1"
        ConnectionString="<%$ ConnectionStrings:PATMAPConnection %>"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsPEMRDescriptions" runat="server" SelectCommand="PEMRDescriptionSelect"
        SelectCommandType="StoredProcedure" CancelSelectOnNullParameter="False" DeleteCommandType="StoredProcedure"
        DeleteCommand="PEMRDescriptionDelete" ConnectionString="<%$ ConnectionStrings:PATMAPConnection %>">
        <SelectParameters>
            <asp:ControlParameter Name="PEMRID" ControlID="ddlDataSet" />
            <asp:ControlParameter Name="year" ControlID="ddlYear" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="PEMRID" />
        </DeleteParameters>
    </asp:SqlDataSource>
</asp:Content>
