<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="PATMAP.viewusers" Codebehind="viewusers.aspx.vb" %>
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
                    <p class="Title">View Users</p>
                </div>
            </div>
        </div>            
            <div class="commonForm">
                <div class="btnsTop"><asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/images/btnAddUser.gif" /></div>
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td class="label"><asp:Label ID="lblName" runat="server" Text="Name"></asp:Label></td>
                        <td class="field"><asp:TextBox ID="txtName" runat="server" CssClass="txtNormal"></asp:TextBox> <asp:ImageButton ID="btnHName" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>                                       
                        <td rowspan="3">&nbsp;</td>
                    </tr>                          
                    <tr>
                        <td class="label"><asp:Label ID="lblUsername" runat="server" Text="Username"></asp:Label></td>
                        <td class="field"><asp:TextBox ID="txtUsername" runat="server" CssClass="txtNormal"></asp:TextBox> <asp:ImageButton ID="btnHUsername" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                    </tr>
                    <tr>
                        <td class="label"><asp:Label ID="lblUserGroup" runat="server" Text="User Group"></asp:Label></td>
                        <td class="field"><asp:DropDownList ID="ddlUserGroup" runat="server"></asp:DropDownList> <asp:ImageButton ID="btnHUserGroup" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>                                           
                    </tr>
                    <tr>
                        <td class="label"><asp:Label ID="lblUserLevel" runat="server" Text="User Level"></asp:Label></td>
                        <td class="field"><asp:DropDownList ID="ddlUserLevel" runat="server"></asp:DropDownList> <asp:ImageButton ID="btnHUserLevel" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>                   
                        <td class="btns"><asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/btnSearch.gif" /></td>
                    </tr>                    
                </table>            
            </div>
            <div class="subHeader">List</div>
            <asp:UpdatePanel ID="uplUsers" runat="server" >
            <ContentTemplate>                
                 <div class="totalCount"><asp:Label ID="lblTotalUser" runat="server" Text="Users: "></asp:Label><asp:Label ID="txtTotalUser" runat="server" Text=""></asp:Label></div>
                    <asp:GridView ID="grdUsers" runat="server" CellPadding="3" AutoGenerateColumns="False" CssClass="grdLgStyle" DataKeyNames="userID" AllowPaging="true" AllowSorting="true">
                     <Columns>
                         <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmEdit.gif&quot; &gt;" CommandName="editUser">
                             <ItemStyle CssClass="colEdit" />
                         </asp:ButtonField>
                         <asp:ButtonField CommandName="deleteUser" Text="&lt;img src=&quot;../images/btnSmDelete.gif&quot; &gt;" >
                             <ItemStyle CssClass="colDelete" />
                         </asp:ButtonField>
                         <asp:BoundField DataField="userID" HeaderText="userID" SortExpression="userID" ItemStyle-Width="5%"/>
                         <asp:BoundField HeaderText="Firstname" DataField="firstName" SortExpression="firstName" ItemStyle-Width="20%"/>
                         <asp:BoundField HeaderText="Lastname" DataField="lastName" SortExpression="lastName" ItemStyle-Width="20%"/>
                         <asp:BoundField HeaderText="Username" DataField="loginName" SortExpression="loginName" ItemStyle-Width="13%"/>
                         <asp:BoundField HeaderText="User Group" DataField="groupName" SortExpression="groupName" ItemStyle-Width="13%"/>
                         <%--<asp:BoundField HeaderText="User Level" DataField="levelName" ItemStyle-Width="15%"/>--%>
                         <asp:BoundField HeaderText="Last Login" DataField="loginDateTime" SortExpression="loginDateTime" ItemStyle-Width="14%"/>
                     </Columns>
                     <HeaderStyle CssClass="colHeader" />
                     <AlternatingRowStyle CssClass="alertnateRow" />
                     <PagerSettings Position="Top" />
                     <PagerStyle CssClass="grdPage" />
                 </asp:GridView> 
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="grdUsers" EventName="PageIndexChanging" />
                <asp:AsyncPostBackTrigger ControlID="grdUsers" EventName="RowCommand" />
                <asp:AsyncPostBackTrigger ControlID="grdUsers" EventName="Sorting" />
            </Triggers>                      
        </asp:UpdatePanel>   
        <br /><br />
        <div class="subHeader">Pending Requests</div>
        <asp:UpdatePanel ID="uplPending" runat="server">
            <ContentTemplate>                 
                 <div class="totalCount"><asp:Label ID="lblPendingRequest" runat="server" Text="Users: "></asp:Label><asp:Label ID="txtPendingRequest" runat="server" Text=""></asp:Label></div>
                 <asp:GridView ID="grdRequests" runat="server" CellPadding="3" AutoGenerateColumns="False" CssClass="grdLgStyle" DataKeyNames="userID" AllowPaging="true" AllowSorting="true">
                     <Columns>
                         <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmEdit.gif&quot; &gt;" CommandName="editUser" >
                             <ItemStyle CssClass="colEdit" />
                         </asp:ButtonField>
                         <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmReject.gif&quot; &gt;" CommandName="deleteUser" ItemStyle-Width="57" />
                         <asp:BoundField HeaderText="Firstname" DataField="firstName" SortExpression="firstName" />
                         <asp:BoundField HeaderText="Lastname" DataField="lastName" SortExpression="lastName" />                 
                         <asp:BoundField HeaderText="Sign-Up Date" DataField="signupDate" SortExpression="signupDate" />
                     </Columns>
                     <HeaderStyle CssClass="colHeader" />
                     <AlternatingRowStyle CssClass="alertnateRow" />
                     <PagerSettings Position="Top" />
                     <PagerStyle CssClass="grdPage" />
                 </asp:GridView> 
            </ContentTemplate>
             <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="grdRequests" EventName="PageIndexChanging" />
                <asp:AsyncPostBackTrigger ControlID="grdRequests" EventName="RowCommand" />
                <asp:AsyncPostBackTrigger ControlID="grdRequests" EventName="Sorting" />
            </Triggers>   
        </asp:UpdatePanel>
    </div>      
</asp:Content>