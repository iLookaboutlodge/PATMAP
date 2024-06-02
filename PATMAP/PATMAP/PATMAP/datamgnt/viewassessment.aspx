<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="viewassessment.aspx.vb" Inherits="PATMAP.viewassessment" %>
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
                            View Assessment Data</p>
                    </div>
                </div>
            </div>
            <div class="commonForm">
                <div class="btnsTop">
                    <asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/images/btnAddAssessment.gif" OnClick="btnAdd_Click" /></div>
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td class="label">
                            <asp:Label ID="lblDSN" runat="server" Text="Data Set Name"></asp:Label></td>
                        <td class="field" colspan="2">
                            <asp:DropDownList ID="ddlDSN" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlDSN_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:ImageButton ID="btnHDSN" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False"
                                CssClass="btnHelp"></asp:ImageButton></td>
                    </tr>
                    <tr>
                        <td class="label">
                            <asp:Label ID="lblMunicipality" runat="server" Text="Municipality"></asp:Label></td>
                        <td class="field" colspan="2">
                            <asp:DropDownList ID="ddlMunicipality" runat="server">
                            </asp:DropDownList>
                            <asp:ImageButton ID="btnHMunicipality" runat="server" ImageUrl="~/images/btnHelp.gif"
                                Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                    </tr>
                    <tr>
                        <td class="label">
                            <asp:Label ID="lblSchoolDivision" runat="server" Text="School Division"></asp:Label></td>
                        <td class="field" colspan="2">
                            <asp:DropDownList ID="ddlSchoolDivision" runat="server">
                            </asp:DropDownList>
                            <asp:ImageButton ID="btnHSchoolDivision" runat="server" ImageUrl="~/images/btnHelp.gif"
                                Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                    </tr>
                    <tr>
                        <td class="label">
                            <asp:Label ID="lblParcelNo" runat="server" Text="Alternate Parcel ID"></asp:Label></td>
                        <td class="field" colspan="2">
                           <%-- <asp:DropDownList ID="ddlParcelNo" runat="server">
                            </asp:DropDownList>--%>
                            <asp:TextBox ID="txtAlternateParcelID" runat="server" CssClass="txtLong"></asp:TextBox>
                            <asp:ImageButton ID="btnHAlternateID" runat="server" ImageUrl="~/images/btnHelp.gif"
                                Visible="False" CssClass="btnHelp"></asp:ImageButton>
                                <asp:RangeValidator ID="RangeValidator9" ControlToValidate="txtAlternateParcelID" Type="Integer"
                            MinimumValue="0" MaximumValue="2147483647" Text="Only Int type less then 2147483647" runat="server" />
                                </td>
                    </tr>
                    <tr>
                        <td class="label">
                            <asp:Label ID="lblTaxClass" runat="server" Text="Tax Class"></asp:Label></td>
                        <td class="field" colspan="2">
                            <asp:DropDownList ID="ddlTaxClass" runat="server">
                            </asp:DropDownList>
                            <asp:ImageButton ID="btnHTaxClass" runat="server" ImageUrl="~/images/btnHelp.gif"
                                Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                    </tr>
                    <tr>
                        <td class="label">
                            <asp:Label ID="lblPresentUseCode" runat="server" Text="Present Use Code"></asp:Label></td>
                        <td class="field">
                            <asp:DropDownList ID="ddlPresentUseCode" runat="server">
                            </asp:DropDownList>
                            <%--<asp:TextBox ID="txtPresentUseCode" runat="server"></asp:TextBox>--%>
                            <asp:ImageButton ID="btnHPresentUseCode" runat="server" ImageUrl="~/images/btnHelp.gif"
                                Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                        <td class="btns">
                            <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/btnSearch.gif"
                                OnClick="btnSearch_Click" /></td>
                    </tr>
                </table>
            </div>
            <div class="subHeader">
                List</div>
            <%-- <asp:UpdatePanel ID="uplAssessment" runat="server">
            <ContentTemplate>
            --%>
            <div class="totalCount">
                <asp:Label ID="lblTotal" runat="server" Text="Assessment Data: "></asp:Label><asp:Label
                    ID="txtTotal" runat="server" Text=""></asp:Label></div>
            <asp:GridView ID="grdAssessment" runat="server" CellPadding="3" AutoGenerateColumns="False"
                CssClass="grdLgStyle" AllowPaging="True" AllowSorting="True" OnPageIndexChanging="grdAssessment_PageIndexChanging"
                OnSorting="grdAssessment_Sorting" OnRowCommand="grdAssessment_RowCommand" DataKeyNames="RowID" >
                <Columns>
                    <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmEdit.gif&quot; /&gt;" CommandName="colEdit">
                        <ItemStyle CssClass="colEdit" />
                    </asp:ButtonField>
                    <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmDelete.gif&quot; /&gt;" CommandName="colDelete">
                        <ItemStyle CssClass="colDelete" />
                    </asp:ButtonField>
                    <%--<asp:BoundField DataField="parcelID" HeaderText="Parcel Number" SortExpression="parcelID" />--%>
                    <asp:BoundField DataField="alternate_parcelID" HeaderText="Alternate Parcel ID" SortExpression="alternate_parcelID" />
                    <asp:BoundField DataField="municipalityID" HeaderText="Municipal" SortExpression="municipalityID" />
                    <asp:BoundField DataField="schoolID" HeaderText="School Division" SortExpression="schoolID" />
                    <asp:BoundField DataField="taxClassID" HeaderText="Tax Class" SortExpression="taxClassID" />
                    <asp:BoundField DataField="presentUseCodeID" HeaderText="PresentUseCodeID" SortExpression="presentUseCodeID" />
                    <asp:BoundField DataField="RowID" HeaderText="RowID" Visible="false" />
                </Columns>
                <HeaderStyle CssClass="colHeader" />
                <AlternatingRowStyle CssClass="alertnateRow" />
                <PagerSettings Position="Top" />
                <PagerStyle CssClass="grdPage" />
            </asp:GridView>
            <%--
           </ContentTemplate>
            <Triggers>      
                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />          
                <asp:AsyncPostBackTrigger ControlID="grdAssessment" EventName="PageIndexChanging" />
                <asp:AsyncPostBackTrigger ControlID="grdAssessment" EventName="RowCommand" />
                <asp:AsyncPostBackTrigger ControlID="grdAssessment" EventName="Sorting" />
            </Triggers>
         </asp:UpdatePanel> 
           --%>
        </div>
</asp:Content>
