<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" ValidateRequest="false"  AutoEventWireup="false" CodeBehind="viewtaxcredit.aspx.vb" Inherits="PATMAP.viewtaxcredit" %>
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
                    <p class="Title">View Tax Credit</p>
                </div>
            </div>
        </div>
        <div class="commonForm">
            <div class="btnsTop"><asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/images/btnAddTaxCredit.gif" /></div>             
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td class="label"><asp:Label ID="lblDSN" runat="server" Text="Data Set Name"></asp:Label> </td>
                    <td class="field">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="uplDSN" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlDSN" runat="server"></asp:DropDownList>
                                    </ContentTemplate>
                                   </asp:UpdatePanel>
                                </td>
                                <td><asp:ImageButton ID="btnHDSN" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                            </tr>
                        </table>                     
                    </td>
                    <td>&nbsp;</td>
                </tr>
                 <tr>
                    <td class="label"><asp:Label ID="lblYear" runat="server" Text="Year"></asp:Label> </td>
                    <td class="field"><asp:DropDownList ID="ddlYear" runat="server"></asp:DropDownList> <asp:ImageButton ID="btnHYear" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                    <td class="btns"><asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/btnSearch.gif" /></td>
                </tr>   
            </table>
        </div>
        <div class="subHeader">List</div>       
        <asp:UpdatePanel ID="uplTaxCredit" runat="server">
            <ContentTemplate>
                <div class="totalCount"><asp:Label ID="lblTotal" runat="server" Text="Tax Credits: "></asp:Label><asp:Label ID="txtTotal" runat="server" Text=""></asp:Label></div>               
                <asp:GridView ID="grdTaxCredit" runat="server" CellPadding="3" AutoGenerateColumns="False" CssClass="grdLgStyle" AllowPaging="True" AllowSorting="True" DataKeyNames="taxCreditID">
                     <Columns>
                        <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmEdit.gif&quot; /&gt;" CommandName="editTaxCredit">
                             <ItemStyle CssClass="colEdit" />
                         </asp:ButtonField>
                         <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmDelete.gif&quot; /&gt;" CommandName="deleteTaxCredit">
                             <ItemStyle CssClass="colDelete" />
                         </asp:ButtonField>                                    
                         <asp:BoundField HeaderText="Data Set Name" DataField="dataSetName" SortExpression="dataSetName"/>                             
                         <asp:BoundField HeaderText="Year" DataField="year" SortExpression="year"/>
                     </Columns>
                     <HeaderStyle CssClass="colHeader" />
                     <AlternatingRowStyle CssClass="alertnateRow" />
                      <PagerSettings Position="Top" />
                     <PagerStyle CssClass="grdPage" />
                 </asp:GridView>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="grdTaxCredit" EventName="PageIndexChanging" />
                <asp:AsyncPostBackTrigger ControlID="grdTaxCredit" EventName="RowCommand" />
                <asp:AsyncPostBackTrigger ControlID="grdTaxCredit" EventName="Sorting" />
            </Triggers>
        </asp:UpdatePanel>         
    </div>         
</asp:Content>
