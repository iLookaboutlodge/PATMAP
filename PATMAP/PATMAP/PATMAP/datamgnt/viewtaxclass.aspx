<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="viewtaxclass.aspx.vb" Inherits="PATMAP.viewtaxclass" %>
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
                    <p class="Title">View Tax Class</p>
                </div>
            </div>
        </div>
        <div class="commonForm">
            <div class="btnsTop">
                <asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/images/btnAddClass.gif" />                
            </div>
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td class="label"><asp:Label ID="lblMainClass" runat="server" Text="Main Class"></asp:Label></td>
                    <td class="field">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="uplDSN" runat="server">
                                    <ContentTemplate>                    
                                        <asp:DropDownList ID="ddlMainClass" runat="server"></asp:DropDownList> 
                                    </ContentTemplate>
                                   </asp:UpdatePanel>
                                </td>                                        
                                <td><asp:ImageButton ID="btnHMainClass" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                            </tr>
                        </table>                                
                    </td>                                       
                    <td>&nbsp;</td>
                </tr>   
                 <%--<tr>
                    <td class="label"><asp:Label ID="lblTaxStatus" runat="server" Text="Tax Status"></asp:Label></td>
                    <td class="field"><asp:DropDownList ID="ddlTaxStatus" runat="server"></asp:DropDownList> <asp:ImageButton ID="btnHTaxStatus" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>                                       
                    <td>&nbsp;</td>
                </tr>--%>
                <tr>
                    <td class="label"><asp:Label ID="lblPropCode" runat="server" Text="Present Use Code"></asp:Label></td>
                    <td class="field"><asp:DropDownList ID="ddlPropCode" runat="server"></asp:DropDownList> <asp:ImageButton ID="btnHPropCode" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>                                       
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="label" colspan="2"><asp:CheckBox ID="chkActive" runat="server" Text="Active" /> <asp:CheckBox ID="chkDefault" runat="server" Text="Default" /> <asp:ImageButton ID="btnHActiveDefault" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>                    
                    <td class="btns"><asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/btnSearch.gif" /></td>
                </tr>
            </table>            
        </div>
        <div><asp:LinkButton ID="lnkSortTaxClass" runat="server" OnClientClick="openWindow('sorttaxclass.aspx','left=50,top=50,width=500,height=600,toolbar=0,resizable=1,scrollbars=1'); return false;">Sort Tax Class</asp:LinkButton></div>         
        <%--<div><asp:LinkButton ID="lnkSortTaxClass" runat="server">Sort Tax Class</asp:LinkButton></div>--%>               
        <div class="subHeader">List</div>       
<%--        <asp:UpdatePanel ID="uplClass" runat="server">
            <ContentTemplate>--%>
                <div class="totalCount"><asp:Label ID="lblTotal" runat="server" Text="Tax Classes: "></asp:Label><asp:Label ID="txtTotal" runat="server" Text=""></asp:Label></div>
                 <asp:GridView ID="grdTaxClass" runat="server" CellPadding="3" AutoGenerateColumns="False" CssClass="grdLgStyle" AllowPaging="True" AllowSorting="True" DataKeyNames="taxClassID">
                     <Columns>
                        <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmEdit.gif&quot; /&gt;" CommandName="editTaxClass" >
                             <ItemStyle CssClass="colEdit" />
                         </asp:ButtonField>
                         <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmDelete.gif&quot; /&gt;" CommandName="deleteTaxClass" >
                             <ItemStyle CssClass="colDelete" />
                         </asp:ButtonField>
                         <asp:BoundField HeaderText="Code" DataField="taxClassID" SortExpression="taxClassID" ItemStyle-Width="20%"/>
                         <asp:BoundField HeaderText="Tax Class" DataField="taxClass" SortExpression="taxClass" ItemStyle-Width="40%" />                 
                         <asp:BoundField HeaderText="Description" DataField="description" ItemStyle-Width="40%" />
                     </Columns>
                     <HeaderStyle CssClass="colHeader" />
                     <AlternatingRowStyle CssClass="alertnateRow" />
                     <PagerSettings Position="Top" />
                     <PagerStyle CssClass="grdPage" />
                 </asp:GridView> 
 <%--           </ContentTemplate>
             <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="grdTaxClass" EventName="PageIndexChanging" />
                <asp:AsyncPostBackTrigger ControlID="grdTaxClass" EventName="RowCommand" />
                <asp:AsyncPostBackTrigger ControlID="grdTaxClass" EventName="Sorting" />
            </Triggers>
        </asp:UpdatePanel> --%>                      
    </div>         
</asp:Content>