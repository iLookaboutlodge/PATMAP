<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="viewpropcode.aspx.vb" Inherits="PATMAP.viewpropcode"%>
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
                    <p class="Title">View Present Use Code</p>
                </div>
            </div>
        </div>
        <div class="commonForm">
            <div class="btnsTop"><asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/images/btnAddPropCode.gif" /></div>                       
        </div>
        <div class="subHeader">List</div>        
        <asp:UpdatePanel ID="uplPropCode" runat="server">
            <ContentTemplate>
                <div class="totalCount"><asp:Label ID="lblTotal" runat="server" Text="Property Use Codes: "></asp:Label><asp:Label ID="txtTotal" runat="server" Text=""></asp:Label></div>
                 <asp:GridView ID="grdPropCode" runat="server" CellPadding="3" AutoGenerateColumns="False" CssClass="grdLgStyle" AllowPaging="True" AllowSorting="True" DataKeyNames="presentUseCodeID">
                     <Columns>
                         <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmEdit.gif&quot; /&gt;" CommandName="editPropCode" >
                             <ItemStyle CssClass="colEdit" />
                         </asp:ButtonField>
                         <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmDelete.gif&quot; /&gt;" CommandName="deletePropCode" >
                             <ItemStyle CssClass="colDelete" />
                         </asp:ButtonField>
                         <asp:BoundField HeaderText="Present Use Code" DataField="presentUseCodeID" SortExpression="presentUseCodeID" ItemStyle-Width="25%"/>
                         <asp:BoundField HeaderText="Short Description" DataField="shortDescription" SortExpression="shortDescription" ItemStyle-Width="75%"/>                 
                     </Columns>
                     <HeaderStyle CssClass="colHeader" />
                     <AlternatingRowStyle CssClass="alertnateRow" />
                     <PagerSettings Position="Top" />
                     <PagerStyle CssClass="grdPage" />
                 </asp:GridView>
            </ContentTemplate>
            <Triggers>                
                <asp:AsyncPostBackTrigger ControlID="grdPropCode" EventName="PageIndexChanging" />
                <asp:AsyncPostBackTrigger ControlID="grdPropCode" EventName="RowCommand" />
                <asp:AsyncPostBackTrigger ControlID="grdPropCode" EventName="Sorting" />
            </Triggers>
        </asp:UpdatePanel>                
    </div>         
</asp:Content>

