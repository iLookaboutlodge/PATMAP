<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="viewtaxstatus.aspx.vb" Inherits="PATMAP.viewtaxstatus" %>
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
                    <p class="Title">View Tax Status</p>
                </div>
            </div>
        </div>
        <div class="commonForm">
            <div class="btnsTop"><asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/images/btnAddStatus.gif" /></div>                       
        </div>
        <div class="subHeader">List</div>       
        <asp:UpdatePanel ID="uplStatus" runat="server">
            <ContentTemplate>
                <div class="totalCount"><asp:Label ID="lblTotal" runat="server" Text="Tax Statuses: "></asp:Label><asp:Label ID="txtTotal" runat="server" Text=""></asp:Label></div>
                 <asp:GridView ID="grdTaxStatus" runat="server" CellPadding="3" AutoGenerateColumns="False" CssClass="grdLgStyle" AllowPaging="True" AllowSorting="True" DataKeyNames="taxStatusID">
                     <Columns>
                        <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmEdit.gif&quot; /&gt;" CommandName="editTaxStatus" >
                             <ItemStyle CssClass="colEdit" />
                         </asp:ButtonField>
                         <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmDelete.gif&quot; /&gt;" CommandName="deleteTaxStatus" >
                             <ItemStyle CssClass="colDelete" />
                         </asp:ButtonField>                
                         <asp:BoundField HeaderText="Tax Status" DataField="taxStatus" SortExpression="taxStatus" ItemStyle-Width="40%"/>
                         <asp:BoundField HeaderText="Description" DataField="description" SortExpression="description" ItemStyle-Width="60%"/>                 
                     </Columns>
                     <HeaderStyle CssClass="colHeader" />
                     <AlternatingRowStyle CssClass="alertnateRow" />
                      <PagerSettings Position="Top" />
                     <PagerStyle CssClass="grdPage" />
                 </asp:GridView> 
            </ContentTemplate>
            <Triggers>                
                <asp:AsyncPostBackTrigger ControlID="grdTaxStatus" EventName="PageIndexChanging" />
                <asp:AsyncPostBackTrigger ControlID="grdTaxStatus" EventName="RowCommand" />
                <asp:AsyncPostBackTrigger ControlID="grdTaxStatus" EventName="Sorting" />
            </Triggers>
        </asp:UpdatePanel>               
    </div>         
</asp:Content>
