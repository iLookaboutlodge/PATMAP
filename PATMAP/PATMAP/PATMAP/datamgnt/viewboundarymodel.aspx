<%@ Page Language="vb" validateRequest="false" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="viewboundarymodel.aspx.vb" Inherits="PATMAP.viewboundarymodel" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server"> 
    <script language="javascript" type="text/javascript" src="../js/general.js"></script>      
     <div class="commonContent">
       <div class="commonHeader">
             <div class="PageHeaderModule">
                <div class="Header">
                    <p class="Title">View Boundary Model</p>
                </div>
            </div>
        </div>
        <div class="commonForm">
           <div class="btnsTop"><asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/images/btnAddBoundary.gif"></asp:ImageButton></div>
           <table cellpadding="0" cellspacing="0" border="0">
                 <tr>
                    <td class="label"><asp:Label ID="lblYear" runat="server" Text="Year"></asp:Label></td>
                    <td class="field"><asp:DropDownList ID="ddlYear" runat="server"></asp:DropDownList><asp:ImageButton ID="btnHYear" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                    <td rowspan="2">&nbsp;</td>
                 </tr>
                 <tr>
                    <td class="label"><asp:Label ID="lblBoundaryModel" runat="server" Text="Boundary Model"></asp:Label></td>
                    <td class="field"><asp:TextBox ID="txtBoundaryModel" runat="server" CssClass="txtNormal"></asp:TextBox><asp:ImageButton ID="btnHBoundaryModel" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr>
                <tr>
                    <td class="label"><asp:Label ID="lblStatus" runat="server" Text="Status"></asp:Label></td>
                    <td class="field">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td><asp:RadioButtonList ID="rdlStatus" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem>Active</asp:ListItem>
                                        <asp:ListItem>Inactive</asp:ListItem>
                                        <asp:ListItem>History</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td style="width:20px"><asp:ImageButton ID="btnHStatus" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                            </tr>
                        </table>
                    </td>                     
                    <td class="btns" style="padding-top:4px;"><asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/btnSearch.gif"></asp:ImageButton></td>
                </tr>
           </table>
        </div> 
        <div class="subHeader">List</div>       
         <asp:UpdatePanel ID="uplBoundaryModel" runat="server" >
            <ContentTemplate>
                <div class="totalCount"><asp:Label ID="lblTotal" runat="server" Text="Boundary Models: "></asp:Label><asp:Label ID="txtTotal" runat="server" Text=""></asp:Label></div>
                 <asp:GridView ID="grdBoundaryModel" runat="server" CellPadding="3" AutoGenerateColumns="False" CssClass="grdLgStyle" AllowPaging="True" AllowSorting="True" DataKeyNames="boundaryModelID">
                     <Columns>
                        <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmEdit.gif&quot; /&gt;" CommandName="editBoundaryModel">
                             <ItemStyle CssClass="colEdit" />
                         </asp:ButtonField>
                         <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmDelete.gif&quot; /&gt;" CommandName="deleteBoundaryModel">
                             <ItemStyle CssClass="colDelete" />
                         </asp:ButtonField>           
                         <asp:BoundField HeaderText="Boundary Model" DataField="boundaryModelName"  SortExpression="boundaryModelName"/>  
                         <asp:BoundField HeaderText="Year" DataField="year" SortExpression="year" />                              
                     </Columns>
                     <HeaderStyle CssClass="colHeader" />
                     <AlternatingRowStyle CssClass="alertnateRow" />
                      <PagerSettings Position="Top" />
                     <PagerStyle CssClass="grdPage" />
                 </asp:GridView>      
            </ContentTemplate>
             <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="grdBoundaryModel" EventName="PageIndexChanging" />
                <asp:AsyncPostBackTrigger ControlID="grdBoundaryModel" EventName="RowCommand" />
                <asp:AsyncPostBackTrigger ControlID="grdBoundaryModel" EventName="Sorting" />
            </Triggers>
         </asp:UpdatePanel>                  
    </div>         
</asp:Content>

