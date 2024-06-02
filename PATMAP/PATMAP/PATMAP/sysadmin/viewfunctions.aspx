<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="viewfunctions.aspx.vb" Inherits="PATMAP.viewfunctions" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server">       
     <script language="javascript" type="text/javascript" src="../js/general.js"></script>

     <div class="commonContent">
       <div class="commonHeader">
             <div class="PageHeaderModule">
                <div class="Header">
                    <p class="Title">View Functions</p>
                </div>
            </div>
        </div>
        <div class="commonForm">            
            <div class="btnsTop"><asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/images/btnAddFunction.gif" /></div>
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td class="label"><asp:Label ID="lblFunctionName" runat="server" Text="Name"></asp:Label></td>
                    <td class="field"><asp:TextBox ID="txtFunctionName" runat="server" CssClass="txtNormal"></asp:TextBox> <asp:ImageButton ID="btnHFunctionName" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                    <td class="btns"><asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/btnSearch.gif" /></td>
                </tr>                 
            </table>            
        </div>
        <div class="subHeader">List</div>         
         <asp:UpdatePanel ID="uplFunction" runat="server">            
         <ContentTemplate>
            <div class="totalCount"><asp:Label ID="lblTotal" runat="server" Text="Functions: "></asp:Label><asp:Label ID="txtTotal" runat="server" Text=""></asp:Label></div>
             <asp:GridView ID="grdFunctions" runat="server" CellPadding="3" AutoGenerateColumns="False" CssClass="grdLgStyle" AllowPaging="True" AllowSorting="True" DataKeyNames="functionID">
                 <Columns>
                     <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmEdit.gif&quot; &gt;" CommandName="editFunction">
                         <ItemStyle CssClass="colEdit" />
                     </asp:ButtonField>
                     <%--<asp:ButtonField Text="&lt;img src=&quot;../images/btnSmDelete.gif&quot; &gt;" CommandName="deleteFunction">
                         <ItemStyle CssClass="colDelete" />
                     </asp:ButtonField>--%>
                    <asp:BoundField HeaderText="Function" DataField="functionName" SortExpression="functionName" ItemStyle-Width="50%"/>   
                    <asp:BoundField HeaderText="Description" DataField="description" SortExpression="description" ItemStyle-Width="50%"/> 
                 </Columns>
                 <HeaderStyle CssClass="colHeader" />
                 <AlternatingRowStyle CssClass="alertnateRow" />
                 <PagerSettings Position="Top" />
                 <PagerStyle CssClass="grdPage" />
             </asp:GridView>
         </ContentTemplate>
         <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="grdFunctions" EventName="PageIndexChanging" />
                <asp:AsyncPostBackTrigger ControlID="grdFunctions" EventName="RowCommand" />
                <asp:AsyncPostBackTrigger ControlID="grdFunctions" EventName="Sorting" />
            </Triggers>
         </asp:UpdatePanel>                     
    </div>         
</asp:Content>


