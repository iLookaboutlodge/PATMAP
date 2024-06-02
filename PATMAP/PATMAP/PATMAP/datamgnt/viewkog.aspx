<%@ Page Language="vb" MasterPageFile="~/MasterPage.master"AutoEventWireup="false" CodeBehind="viewkog.aspx.vb" Inherits="PATMAP.viewkog" %>
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
                            View K-12 OG</p>
                    </div>
                </div>
            </div>
            <div class="commonForm">
                <div class="btnsTop">
                    <asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/images/btnAddKOG.gif" OnClick="btnAdd_Click" /></div>
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td class="label">
                            <asp:Label ID="lblDSN" runat="server" Text="Data Set Name"></asp:Label></td>
                        <td class="field">
                            <asp:DropDownList ID="ddlDSN" runat="server">
                            </asp:DropDownList>
                            <asp:ImageButton ID="btnHDSN" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False"
                                CssClass="btnHelp"></asp:ImageButton></td>
                        <td rowspan="3">
                            &nbsp;</td>
                    </tr>
                    <%--   <tr>
                    <td class="label"><asp:Label ID="lblYear" runat="server" Text="Year"></asp:Label> </td>
                    <td class="field"><asp:DropDownList ID="ddlYear" runat="server"></asp:DropDownList> <asp:ImageButton ID="btnHYear" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr>--%>
                    <tr>
                        <td class="label">
                            <asp:Label ID="lblSchoolDivision" runat="server" Text="School Division"></asp:Label></td>
                        <td class="field">
                            <asp:DropDownList ID="ddlSchoolDivision" runat="server">
                            </asp:DropDownList>
                            <asp:ImageButton ID="btnHSchoolDivision" runat="server" ImageUrl="~/images/btnHelp.gif"
                                Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                        <td class="btns">
                            <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/btnSearch.gif" OnClick="btnSearch_Click" /></td>
                    </tr>
                </table>
            </div>
            <%--<asp:UpdatePanel ID="uplKOG" runat="server" >
            <ContentTemplate>--%>
            <div class="totalCount">
                <asp:Label ID="lblTotal" runat="server" Text="K12 Data: ">
                </asp:Label><asp:Label ID="txtTotal" runat="server" Text=""></asp:Label></div>
            <div class="subHeader">
                List</div>
                    
            <asp:GridView ID="grdKOG" runat="server" CellPadding="3" AutoGenerateColumns="False"
                CssClass="grdLgStyle" AllowPaging="True" AllowSorting="True" DataKeyNames="RowID" OnPageIndexChanging="grdKOG_PageIndexChanging" OnRowCommand="grdKOG_RowCommand" OnSorting="grdKOG_Sorting">
                <Columns>
                    <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmEdit.gif&quot; /&gt;" CommandName="colEdit">
                        <ItemStyle CssClass="colEdit" />
                    </asp:ButtonField>
                    <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmDelete.gif&quot; /&gt;" CommandName="colDelete">
                        <ItemStyle CssClass="colDelete" />
                    </asp:ButtonField>
                    <asp:BoundField DataField="RowID" HeaderText="RowID" SortExpression="RowID" Visible="false" />
                    <asp:BoundField DataField="schoolID" HeaderText="School Division" SortExpression="schoolID" />
                    <asp:BoundField DataField="dataSetName" HeaderText="Data Set Name" SortExpression="dataSetName" />
                    <%--<asp:BoundField HeaderText="Year" />    --%>
                </Columns>
                <HeaderStyle CssClass="colHeader" />
                <AlternatingRowStyle CssClass="alertnateRow" />
                <PagerSettings Position="Top" />
                <PagerStyle CssClass="grdPage" />
            </asp:GridView>
            <%-- </ContentTemplate>
             <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="grdKOG" EventName="PageIndexChanging" />
                <asp:AsyncPostBackTrigger ControlID="grdKOG" EventName="RowCommand" />
                <asp:AsyncPostBackTrigger ControlID="grdKOG" EventName="Sorting" />
            </Triggers>
          </asp:UpdatePanel>      --%>
        </div>
</asp:Content>


