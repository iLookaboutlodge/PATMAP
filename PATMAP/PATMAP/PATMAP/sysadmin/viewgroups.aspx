<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="viewgroups.aspx.vb" Inherits="PATMAP.viewgroup" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register TagPrefix="patmap" TagName="sysTabMenu" Src="~/tabmenu.ascx" %>
<asp:Content ID="tabMenu" ContentPlaceHolderID="tabMenu" runat="server">   
   <patmap:sysTabMenu id="subMenu" runat="server"></patmap:sysTabMenu>
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server">       
     <div class="commonContent">
       <div class="commonHeader">
             <div class="PageHeaderModule">
                <div class="Header">
                    <p class="Title">View User Groups</p>
                </div>
            </div>
        </div>
        <div class="commonForm">
            <div class="btnsTop"><asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/images/btnAddGroup.gif" /></div>                       
        </div>
        <div class="subHeader">List</div>        
         <asp:UpdatePanel ID="uplGroups" runat="server">
            <ContentTemplate>
                <div class="totalCount"><asp:Label ID="lblTotal" runat="server" Text="User Groups: "></asp:Label><asp:Label ID="txtTotal" runat="server" Text=""></asp:Label></div>
                 <asp:GridView ID="grdGroups" runat="server" CellPadding="3" AutoGenerateColumns="False" CssClass="grdLgStyle" DataKeyNames="groupID" AllowPaging="true" AllowSorting="true">
                     <Columns>
                         <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmEdit.gif&quot; /&gt;" CommandName="editGroup" >
                             <ItemStyle CssClass="colEdit" />
                         </asp:ButtonField>
                         <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmDelete.gif&quot; /&gt;" CommandName="deleteGroup" >
                             <ItemStyle CssClass="colDelete" />
                         </asp:ButtonField>
                         <asp:BoundField HeaderText="User Groups" DataField="groupName" SortExpression="groupName" ItemStyle-Width="43%"/>
                         <asp:BoundField HeaderText="Number of Members" DataField="memberCount" SortExpression="memberCount" ItemStyle-Width="13%"/> 
                         <asp:BoundField HeaderText="Description" DataField="description" SortExpression="description" ItemStyle-Width="43%"/>
                     </Columns>
                     <HeaderStyle CssClass="colHeader" />
                     <AlternatingRowStyle CssClass="alertnateRow" />
                     <PagerSettings Position="Top" />
                     <PagerStyle CssClass="grdPage" />
                 </asp:GridView>  
            </ContentTemplate>
            <Triggers>                
                <asp:AsyncPostBackTrigger ControlID="grdGroups" EventName="PageIndexChanging" />
                <asp:AsyncPostBackTrigger ControlID="grdGroups" EventName="RowCommand" />
                <asp:AsyncPostBackTrigger ControlID="grdGroups" EventName="Sorting" />
            </Triggers>
          </asp:UpdatePanel>              
    </div>         
</asp:Content>
