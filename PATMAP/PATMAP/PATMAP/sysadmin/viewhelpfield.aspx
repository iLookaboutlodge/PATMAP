<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="viewhelpfield.aspx.vb" Inherits="PATMAP.viewhelpfield" %>
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
                    <p class="Title">View Form Field Help Text</p>
                </div>
            </div>
        </div>       
        <asp:UpdatePanel ID="uplFields" runat="server">
            <ContentTemplate>
                <div class="commonForm">            
                <div class="btnsTop"><asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/images/btnAddField.gif" /></div>
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td class="label"><asp:Label ID="lblSection" runat="server" Text="Section"></asp:Label></td>
                        <td class="field" colspan="2"><asp:DropDownList ID="ddlSection" runat="server" AutoPostBack="True"></asp:DropDownList> <asp:ImageButton ID="btnHSection" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                    </tr> 
                    <tr>
                        <td class="label"><asp:Label ID="lblScreen" runat="server" Text="Screen"></asp:Label></td>
                        <td class="field" colspan="2"><asp:DropDownList ID="ddlScreen" runat="server" AutoPostBack="True"></asp:DropDownList> <asp:ImageButton ID="btnHScreen" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>                                       
                    </tr>
                    <tr>
                        <td class="label"><asp:Label ID="lblType" runat="server" Text="Type"></asp:Label></td>
                        <td class="field" colspan="2"><asp:DropDownList ID="ddlType" runat="server" AutoPostBack="True"></asp:DropDownList> <asp:ImageButton ID="btnHType" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>                                       
                    </tr>
                     <tr>
                        <td class="label"><asp:Label ID="lblFieldName" runat="server" Text="Fieldname"></asp:Label></td>
                        <td class="field"><asp:DropDownList ID="ddlFieldName" runat="server"></asp:DropDownList> <asp:ImageButton ID="btnHFieldName" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>                   
                        <td class="btns"><asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/btnSearch.gif" /></td>
                    </tr>
                </table>            
            </div>
            <div class="subHeader">List</div>
            <div class="totalCount"><asp:Label ID="lblTotal" runat="server" Text="Form Fields Help Text: "></asp:Label><asp:Label ID="txtTotal" runat="server" Text=""></asp:Label></div>
             <asp:GridView ID="grdFields" runat="server" CellPadding="3" AutoGenerateColumns="False" CssClass="grdLgStyle" AllowPaging="True" AllowSorting="True" DataKeyNames="controlID">
                 <Columns>
                     <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmEdit.gif&quot; &gt;" CommandName="editHelpControl">
                         <ItemStyle CssClass="colEdit" />
                     </asp:ButtonField>
                     <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmDelete.gif&quot; &gt;" CommandName="deleteHelpControl">
                         <ItemStyle CssClass="colDelete" />
                     </asp:ButtonField>
                    <asp:BoundField HeaderText="Form Fields" DataField="controlName" SortExpression="controlName" ItemStyle-Width="38%"/>  
                    <asp:BoundField HeaderText="Type" DataField="controlType" SortExpression="controlType" ItemStyle-Width="23%"/> 
                    <asp:BoundField HeaderText="Description" DataField="description" SortExpression="description" ItemStyle-Width="38%"/> 
                 </Columns>
                 <HeaderStyle CssClass="colHeader" />
                 <AlternatingRowStyle CssClass="alertnateRow" />
                 <PagerSettings Position="Top" />
                 <PagerStyle CssClass="grdPage" />
             </asp:GridView> 
            </ContentTemplate>
        </asp:UpdatePanel>                     
    </div>         
</asp:Content>

