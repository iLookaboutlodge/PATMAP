<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="viewhelpscreen.aspx.vb" Inherits="PATMAP.viewhelpscreen" %>
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
                    <p class="Title">View Help Screens</p>
                </div>
            </div>
        </div>        
        <div class="commonForm">            
            <div class="btnsTop"><asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/images/btnAddScreen.gif" /></div>
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td class="label"><asp:Label ID="lblSection" runat="server" Text="Section"></asp:Label></td>
                    <td class="field" colspan="2"><asp:DropDownList ID="ddlSection" runat="server" AutoPostBack="True"></asp:DropDownList> <asp:ImageButton ID="btnHSection" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr> 
                <tr>
                    <td class="label"><asp:Label ID="lblScreen" runat="server" Text="Screen"></asp:Label></td>
                    <td class="field">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="uplScreens" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlScreen" runat="server"></asp:DropDownList> 
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlSection" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                                <td><asp:ImageButton ID="btnHScreen" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                            </tr>
                        </table>                                                
                    </td>                   
                    <td class="btns"><asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/btnSearch.gif" /></td>
                </tr>
            </table>            
        </div>
        <div class="subHeader">List</div>
        <asp:UpdatePanel ID="uplScreens2" runat="server">
            <ContentTemplate>
                <div class="totalCount"><asp:Label ID="lblTotal" runat="server" Text="Help Screens: "></asp:Label><asp:Label ID="txtTotal" runat="server" Text=""></asp:Label></div>
                 <asp:GridView ID="grdScreens" runat="server" CellPadding="3" AutoGenerateColumns="False" CssClass="grdLgStyle" AllowPaging="True" AllowSorting="True" DataKeyNames="ScreenNameID">
                     <Columns>
                         <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmEdit.gif&quot; &gt;" CommandName="editHelpScreen">
                             <ItemStyle CssClass="colEdit" />
                         </asp:ButtonField>
                         <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmDelete.gif&quot; &gt;" CommandName="deleteHelpScreen" >
                             <ItemStyle CssClass="colDelete" />
                         </asp:ButtonField>
                        <asp:BoundField HeaderText="Section" DataField="sectionName" SortExpression="sectionName" ItemStyle-Width="23%"/> 
                        <asp:BoundField HeaderText="Screen" DataField="screenName" SortExpression="screenName" ItemStyle-Width="38%"/>  
                        <asp:BoundField HeaderText="Description" DataField="description" SortExpression="description" ItemStyle-Width="38%"/> 
                     </Columns>
                     <HeaderStyle CssClass="colHeader" />
                     <AlternatingRowStyle CssClass="alertnateRow" />
                     <PagerSettings Position="Top" />
                     <PagerStyle CssClass="grdPage" />
                 </asp:GridView> 
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="grdScreens" EventName="PageIndexChanging" />
                <asp:AsyncPostBackTrigger ControlID="grdScreens" EventName="RowCommand" />
                <asp:AsyncPostBackTrigger ControlID="grdScreens" EventName="Sorting" />
            </Triggers>
        </asp:UpdatePanel>                     
    </div>         
</asp:Content>
