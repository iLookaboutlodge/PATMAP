<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    Codebehind="viewpotash.aspx.vb" Inherits="PATMAP.viewpotash" %>

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
                        View Potash</p>
                </div>
            </div>
        </div>
        <div class="commonForm">
            <div class="btnsTop">
                <asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/images/btnAddPotash.gif"
                    OnClick="btnAdd_Click" /></div>
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td class="label">
                        <asp:Label ID="lblDSN" runat="server" Text="Data Set Name"></asp:Label></td>
                    <td class="field">
                        <asp:DropDownList ID="ddlDSN" runat="server">
                        </asp:DropDownList>
                        <asp:ImageButton ID="btnHDSN" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False"
                            CssClass="btnHelp"></asp:ImageButton></td>
                    <td rowspan="5">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label ID="lblMunicipalityType" runat="server" Text="Municipality Type"></asp:Label></td>
                    <td class="field">
                        <asp:DropDownList ID="ddlMunicipalityType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlMunicipalityType_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:ImageButton ID="btnHMunicipalityType" runat="server" ImageUrl="~/images/btnHelp.gif"
                            Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label ID="lblpotashAreaID" runat="server" Text="Potash AreaID"></asp:Label></td>
                    <td class="field">
                        <asp:TextBox ID="txtpotashAreaID" runat="server" Width="50"></asp:TextBox>
                        <asp:ImageButton ID="btnHpotashAreaID" runat="server" ImageUrl="~/images/btnHelp.gif"
                            Visible="False" CssClass="btnHelp"></asp:ImageButton>
                        <asp:RangeValidator ID="RangeValidatortxtpotashAreaID" ControlToValidate="txtpotashAreaID"
                            Type="Integer" MinimumValue="0" MaximumValue="32767" Text="Only Int type less then 32767"
                            runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label ID="lblMunicipalityID" runat="server" Text="Municipality ID"></asp:Label></td>
                    <td class="field">
                        <asp:DropDownList ID="ddlMunicipalityID" runat="server">
                        </asp:DropDownList>
                        <asp:ImageButton ID="btnHMunicipalityID" runat="server" ImageUrl="~/images/btnHelp.gif"
                            Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td class="btns">
                        <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/btnSearch.gif"
                            OnClick="btnSearch_Click" /></td>
                </tr>
            </table>
        </div>
        <div class="subHeader">
            List</div>
        <%--<asp:UpdatePanel ID="uplPotash" runat="server" >
            <ContentTemplate>--%>
        <div class="totalCount">
            <asp:Label ID="lblTotal" runat="server" Text="Potash: "></asp:Label><asp:Label ID="txtTotal"
                runat="server" Text=""></asp:Label></div>
        <asp:GridView ID="grdPotash" runat="server" CellPadding="3" AutoGenerateColumns="False"
            CssClass="grdPotash" AllowPaging="True" AllowSorting="True" DataKeyNames="RowID"
            OnPageIndexChanging="grdPotash_PageIndexChanging" OnRowCommand="grdPotash_RowCommand"
            OnSorting="grdPotash_Sorting">
            <Columns>
                <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmEdit.gif&quot; /&gt;" CommandName="colEdit">
                    <ItemStyle CssClass="colEdit" />
                </asp:ButtonField>
                <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmDelete.gif&quot; /&gt;" CommandName="colDelete">
                    <ItemStyle CssClass="colDelete" />
                </asp:ButtonField>
                <asp:BoundField DataField="RowID" HeaderText="RowID" SortExpression="RowID" Visible="false" />
                <asp:BoundField DataField="potashAreaID" HeaderText="Area ID" SortExpression="potashAreaID" />
                <asp:BoundField DataField="MunicipalityID" HeaderText="Municipality ID" SortExpression="MunicipalityID" />
                <asp:BoundField DataField="totalPoints" HeaderText="Total Points" SortExpression="totalPoints" />
                <asp:BoundField DataField="totalGrant" HeaderText="Total Grant" SortExpression="totalGrant" />
                <asp:BoundField DataField="PotashType" HeaderText="PotashType" SortExpression="PotashType" />
                <%--<asp:BoundField DataField="dataSetName" HeaderText="Data Set Name" SortExpression="dataSetName" />--%>
            </Columns>
            <HeaderStyle CssClass="colHeader" />
            <AlternatingRowStyle CssClass="alertnateRow" />
            <PagerSettings Position="Top" />
            <PagerStyle CssClass="grdPage" />
        </asp:GridView>
        <%--</ContentTemplate>
         <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="grdPotash" EventName="PageIndexChanging" />
                <asp:AsyncPostBackTrigger ControlID="grdPotash" EventName="RowCommand" />
                <asp:AsyncPostBackTrigger ControlID="grdPotash" EventName="Sorting" />
            </Triggers>
       </asp:UpdatePanel>   --%>
    </div>
</asp:Content>
