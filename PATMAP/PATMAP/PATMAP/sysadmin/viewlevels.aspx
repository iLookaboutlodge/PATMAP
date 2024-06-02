<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="viewlevels.aspx.vb" Inherits="PATMAP.viewlevels" %>
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
                    <p class="Title">View User Levels</p>
                </div>
            </div>
        </div>
        <div class="commonForm">
            <div class="btnsTop"><asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/images/btnAddLevel.gif" /></div>           
        </div>
        <div class="subHeader">List</div>         
        <asp:UpdatePanel ID="uplLevels" runat="server" >
            <ContentTemplate>
                <div class="totalCount"><asp:Label ID="lblTotal" runat="server" Text="User Levels: "></asp:Label><asp:Label ID="txtTotal" runat="server" Text=""></asp:Label></div>
                <asp:GridView ID="grdLevels" runat="server" CellPadding="3" AutoGenerateColumns="False" CssClass="grdLgStyle" DataKeyNames="levelID" AllowPaging="true" AllowSorting="true" OnSelectedIndexChanged="grdLevels_SelectedIndexChanged">
                 <Columns>
                     <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmEdit.gif&quot; /&gt;" CommandName="editLevel">
                         <ItemStyle CssClass="colEdit" />
                     </asp:ButtonField>
                     <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmDelete.gif&quot; /&gt;" CommandName="deleteLevel">
                         <ItemStyle CssClass="colDelete" />
                     </asp:ButtonField>
                     <asp:BoundField HeaderText="User Level" DataField="levelName" SortExpression="levelName" ItemStyle-Width="50%"/>
                     <asp:BoundField HeaderText="Description" DataField="description" SortExpression="description" ItemStyle-Width="50%"/>
                 </Columns>
                 <HeaderStyle CssClass="colHeader" />
                 <AlternatingRowStyle CssClass="alertnateRow" />
                 <PagerSettings Position="Top" />
                 <PagerStyle CssClass="grdPage" />
             </asp:GridView>
            </ContentTemplate>
             <Triggers>                
                <asp:AsyncPostBackTrigger ControlID="grdLevels" EventName="PageIndexChanging" />
                <asp:AsyncPostBackTrigger ControlID="grdLevels" EventName="RowCommand" />
                <asp:AsyncPostBackTrigger ControlID="grdLevels" EventName="Sorting" />
            </Triggers>
        </asp:UpdatePanel> 
    </div>         
</asp:Content>
