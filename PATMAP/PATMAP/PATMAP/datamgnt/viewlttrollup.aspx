<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="viewlttrollup.aspx.vb" Inherits="PATMAP.viewlttrollup" %>
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
                    <p class="Title">View LTT Rollup Class</p>
                </div>
            </div>
        </div>
        <div class="commonForm">
            <div class="btnsTop">
                <asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/images/btnAddRollupClass.gif" />                
            </div>
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td class="label"><asp:Label ID="lblRollupClass" runat="server" Text="Main LTT Rollup Class: "></asp:Label></td>
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
                    <td class="btns">&nbsp;<asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/btnSearch.gif" /></td>
                </tr>   
                 <%--<tr>
                    <td class="label"><asp:Label ID="lblTaxStatus" runat="server" Text="Tax Status"></asp:Label></td>
                    <td class="field"><asp:DropDownList ID="ddlTaxStatus" runat="server"></asp:DropDownList> <asp:ImageButton ID="btnHTaxStatus" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>                                       
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="label"></td>
                    <td class="field"></td>                                       
                    <td></td>
                </tr> 
                <tr>
                    <td class="label" colspan="2">
                        &nbsp;
                    </td>                   
                    <td class="btns"></td>
                </tr>--%>
            </table>            
        </div>
        <div>
            &nbsp;</div>         
        <%--<div><asp:LinkButton ID="lnkSortTaxClass" runat="server">Sort Tax Class</asp:LinkButton></div>--%>               
        <div class="subHeader">List</div>       
<%--        <asp:UpdatePanel ID="uplClass" runat="server">
            <ContentTemplate>--%>
                <div class="totalCount"><asp:Label ID="lblTotal" runat="server" Text="Rollup Classes: "></asp:Label><asp:Label ID="txtTotal" runat="server" Text=""></asp:Label></div>
                 <asp:GridView ID="grdRollupClass" runat="server" CellPadding="3" AutoGenerateColumns="False" CssClass="grdLgStyle" AllowPaging="True" AllowSorting="True" DataKeyNames="taxClassID">
                     <Columns>
                        <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmEdit.gif&quot; /&gt;" CommandName="editRollupClass" >
                             <ItemStyle CssClass="colEdit" />
                         </asp:ButtonField>
                         <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmDelete.gif&quot; /&gt;" CommandName="deleteRollupClass" >
                             <ItemStyle CssClass="colDelete" />
                         </asp:ButtonField>
                         <asp:BoundField HeaderText="Rollup Class" DataField="taxClass" SortExpression="taxClass" >
                             <ItemStyle Width="40%" />
                         </asp:BoundField>                 
                         <asp:BoundField HeaderText="Description" DataField="description" >
                             <ItemStyle Width="40%" />
                         </asp:BoundField>
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