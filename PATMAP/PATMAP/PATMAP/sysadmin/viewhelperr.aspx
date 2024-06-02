<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="viewhelperr.aspx.vb" Inherits="PATMAP.viewhelperr" %>
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
                    <p class="Title">View Error Message Help Text</p>
                </div>
            </div>
        </div>
        <div class="commonForm">            
            <div class="btnsTop"><asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/images/btnAddError.gif" /></div>
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td class="label"><asp:Label ID="lblErrName" runat="server" Text="Name"></asp:Label></td>
                    <td class="field"><asp:TextBox ID="txtErrName" runat="server" CssClass="txtNormal"></asp:TextBox> <asp:ImageButton ID="btnHErrName" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                    <td>&nbsp;</td>
                </tr> 
                <tr>
                    <td class="label"><asp:Label ID="lblCode" runat="server" Text="Code"></asp:Label></td>
                    <td class="field"><asp:TextBox ID="txtCode" runat="server" style="width:100px;"></asp:TextBox> <asp:ImageButton ID="btnHCode" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                    <td class="btns"><asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/btnSearch.gif" /></td>
                </tr>
            </table>            
        </div>
        <div class="subHeader">List</div>       
         <asp:UpdatePanel ID="uplErrors" runat="server">
            <ContentTemplate>
                <div class="totalCount"><asp:Label ID="lblTotal" runat="server" Text="Error Msg Help Text: "></asp:Label><asp:Label ID="txtTotal" runat="server" Text=""></asp:Label></div>
                 <asp:GridView ID="grdErrors" runat="server" CellPadding="3" AutoGenerateColumns="False" CssClass="grdLgStyle" AllowPaging="True" AllowSorting="True" DataKeyNames="errorCode">
                     <Columns>
                         <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmEdit.gif&quot; &gt;" CommandName="editErrorCode">
                             <ItemStyle CssClass="colEdit" />
                         </asp:ButtonField>
                         <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmDelete.gif&quot; &gt;" CommandName="deleteErrorCode" >
                             <ItemStyle CssClass="colDelete" />
                         </asp:ButtonField>
                        <asp:BoundField HeaderText="Code" DataField="errorCode" SortExpression="errorCode" ItemStyle-Width="13%"/>  
                        <asp:BoundField HeaderText="Name" DataField="errorName" SortExpression="errorName" ItemStyle-Width="43%"/> 
                        <asp:BoundField HeaderText="Description" DataField="description" SortExpression="description" ItemStyle-Width="43%"/> 
                     </Columns>
                     <HeaderStyle CssClass="colHeader" />
                     <AlternatingRowStyle CssClass="alertnateRow" />
                     <PagerSettings Position="Top" />
                     <PagerStyle CssClass="grdPage" />
                 </asp:GridView>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="grdErrors" EventName="PageIndexChanging" />
                <asp:AsyncPostBackTrigger ControlID="grdErrors" EventName="RowCommand" />
                <asp:AsyncPostBackTrigger ControlID="grdErrors" EventName="Sorting" />
            </Triggers>
         </asp:UpdatePanel>                      
    </div>         
</asp:Content>

