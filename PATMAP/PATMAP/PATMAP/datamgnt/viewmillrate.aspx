<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="viewmillrate.aspx.vb" Inherits="PATMAP.viewmillrate" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register TagPrefix="patmap" TagName="dataTabMenu" Src="~/tabmenu.ascx" %>
<asp:Content ID="tabMenu" ContentPlaceHolderID="tabMenu" runat="server">   
    <patmap:dataTabMenu id="subMenu" runat="server"></patmap:dataTabMenu>    
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server">       
    <div class="commonContent">
            <div class="commonHeader">
                <div class="PageHeaderModule">
                    <div class="Header">
                        <p class="Title">
                            View Mill Rate Survey</p>
                    </div>
                </div>
            </div>
            <div class="commonForm">
                <div class="btnsTop">
                    <asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/images/btnAddMillRate.gif" OnClick="btnAdd_Click" /></div>
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td class="label">
                            <asp:Label ID="lblDSN" runat="server" Text="Data Set Name"></asp:Label></td>
                        <td class="field">
                            <asp:DropDownList ID="ddlDSN" runat="server">
                            </asp:DropDownList>
                            <asp:ImageButton ID="btnHDSN" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False"
                                CssClass="btnHelp"></asp:ImageButton></td>
                        <td rowspan="2">
                            &nbsp;</td>
                    </tr>
                    <%-- <tr>
                    <td class="label"><asp:Label ID="lblYear" runat="server" Text="Year"></asp:Label> </td>
                    <td class="field"><asp:DropDownList ID="ddlYear" runat="server"></asp:DropDownList> <asp:ImageButton ID="btnHYear" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>                    
                </tr>  --%>
                    <tr>
                        <td class="label">
                            <asp:Label ID="lblJurisdiction" runat="server" Text="Jurisdiction"></asp:Label></td>
                        <td class="field">
                            <asp:DropDownList ID="ddlJurisdiction" runat="server">
                            </asp:DropDownList>
                            <asp:ImageButton ID="btnHJurisdiction" runat="server" ImageUrl="~/images/btnHelp.gif"
                                Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                        <td class="btns">
                            <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/btnSearch.gif" OnClick="btnSearch_Click" /></td>
                    </tr>
                </table>
            </div>
            <div class="subHeader">
                List</div>
            <%-- <asp:UpdatePanel ID="uplMillRate" runat="server">
            <ContentTemplate>--%>
            <div class="totalCount">
                <asp:Label ID="lblTotal" runat="server" Text="Mill Rates: "></asp:Label><asp:Label
                    ID="txtTotal" runat="server" Text=""></asp:Label></div>
            <asp:GridView ID="grdMillRate" runat="server" CellPadding="3" AutoGenerateColumns="False"
                CssClass="grdLgStyle" AllowPaging="True" AllowSorting="True" DataKeyNames="RowID" OnPageIndexChanging="grdMillRate_PageIndexChanging" OnRowCommand="grdMillRate_RowCommand" OnSorting="grdMillRate_Sorting">
                <Columns>
                    <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmEdit.gif&quot; /&gt;" CommandName="colEdit">
                        <ItemStyle CssClass="colEdit" />
                    </asp:ButtonField>
                    <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmDelete.gif&quot; /&gt;" CommandName="colDelete">
                        <ItemStyle CssClass="colDelete" />
                    </asp:ButtonField>
                    <asp:BoundField DataField="RowID" HeaderText="RowID" SortExpression="RowID" Visible="false" />
                    <asp:BoundField DataField="dataSetName" HeaderText="Data Set Name" SortExpression="dataSetName" />
                    <asp:BoundField DataField="JurisdictionType" HeaderText="JurisdictionType" SortExpression="JurisdictionType" />
                    <asp:BoundField DataField="MunicipalityID" HeaderText="MunicipalityID" SortExpression="MunicipalityID" />
                    <asp:BoundField DataField="Levy" HeaderText="Levy" SortExpression="Levy" />
                </Columns>
                <HeaderStyle CssClass="colHeader" />
                <AlternatingRowStyle CssClass="alertnateRow" />
                <PagerSettings Position="Top" />
                <PagerStyle CssClass="grdPage" />
            </asp:GridView>
            <%--</ContentTemplate>
             <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="grdMillRate" EventName="PageIndexChanging" />
                <asp:AsyncPostBackTrigger ControlID="grdMillRate" EventName="RowCommand" />
                <asp:AsyncPostBackTrigger ControlID="grdMillRate" EventName="Sorting" />
            </Triggers>
         </asp:UpdatePanel>        --%>
        </div>
</asp:Content>
