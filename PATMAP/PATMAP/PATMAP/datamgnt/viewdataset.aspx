<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="viewdataset.aspx.vb" Inherits="PATMAP.viewdataset" %>
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
                    <p class="Title">View Data Set</p>
                </div>
            </div>
        </div>
        <div class="commonForm">        
           <table cellpadding="0" cellspacing="0" border="0">
                 <tr>
                    <td class="label"><asp:Label ID="lblDataSource" runat="server" Text="Data Source"></asp:Label></td>
                    <td class="field">
                        <asp:DropDownList ID="ddlDataSource" runat="server" AutoPostBack="True">                           
                            <asp:ListItem Value="1">Assessment</asp:ListItem>
                            <%--<asp:ListItem Value="2">Tax Credit</asp:ListItem>--%>
                            <asp:ListItem Value="3">POV</asp:ListItem>
                            <%--<asp:ListItem Value="4">K12OG</asp:ListItem>--%>
                            <asp:ListItem Value="5">Mill Rate Survey</asp:ListItem>
                            <asp:ListItem Value="6">Potash</asp:ListItem>
                        </asp:DropDownList> 
                        <asp:ImageButton ID="btnHDataSource" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                    <td rowspan="2">&nbsp;</td>
                 </tr>
                 <tr>
                    <td class="label"><asp:Label ID="lblDataSet" runat="server" Text="Data Set Name"></asp:Label></td>
                    <td class="field">
                        <table cellpadding="0" cellspacing="0" border="0">                            
                            <tr>
                                <td>
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td>
                                                 <asp:UpdatePanel ID="uplDSN" runat="server" >
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlDSN" runat="server"></asp:DropDownList> 
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddlDataSource" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="grdDataSet" EventName="RowCommand" />
                                                    <asp:AsyncPostBackTrigger ControlID="rdlStatus" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td><asp:ImageButton ID="btnHDSN" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                        </tr>
                                    </table>                                     
                                </td>                                
                            </tr>
                            <tr>
                                <td align="center"><asp:Label ID="lblOr" runat="server" Text="Or"></asp:Label></td>
                            </tr>                            
                             <tr>
                                <td><asp:TextBox ID="txtDSN" runat="server" CssClass="txtLong"></asp:TextBox> <asp:ImageButton ID="btnHDSN2" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>                                
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="label"><asp:Label ID="lblStatus" runat="server" Text="Status"></asp:Label></td>
                    <td class="field">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="uplStatus" runat="server" >
                                        <ContentTemplate>
                                             <asp:RadioButtonList ID="rdlStatus" runat="server" RepeatDirection="Horizontal" AutoPostBack="true">
                                            <asp:ListItem>Active</asp:ListItem>
                                            <asp:ListItem>History</asp:ListItem>
                                            </asp:RadioButtonList> 
                                        </ContentTemplate>
                                    </asp:UpdatePanel>                                                              
                                </td>
                                <td><asp:ImageButton ID="btnHStatus" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                            </tr>
                        </table>
                    </td>
                    <td class="btns"><asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/btnSearch.gif" /></td>
                </tr>
           </table>
        </div> 
        <div class="subHeader">List</div>        
         <asp:UpdatePanel ID="uplDataSet" runat="server" >
            <ContentTemplate>
                <div class="totalCount"><asp:Label ID="lblTotal" runat="server" Text="Data Sets: "></asp:Label><asp:Label ID="txtTotal" runat="server" Text=""></asp:Label></div>
                 <asp:GridView ID="grdDataSet" runat="server" CellPadding="3" AutoGenerateColumns="False" CssClass="grdLgStyle" AllowPaging="True" AllowSorting="True" DataKeyNames="dataSetID">
                     <Columns>
                        <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmEdit.gif&quot; /&gt;" CommandName="editDataSet" >
                             <ItemStyle CssClass="colEdit" />
                         </asp:ButtonField>
                         <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmDelete.gif&quot; /&gt;" CommandName="deleteDataSet" >
                             <ItemStyle CssClass="colDelete" />
                         </asp:ButtonField>                  
                         <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmHistory.gif&quot; /&gt;" CommandName="historyDataSet" >
                             <ItemStyle CssClass="colHistory" />
                         </asp:ButtonField>                               
                         <asp:BoundField HeaderText="Data Set Name" DataField="dataSetName" SortExpression="dataSetName">                                
                         </asp:BoundField>                                 
                     </Columns>
                     <HeaderStyle CssClass="colHeader" />
                     <AlternatingRowStyle CssClass="alertnateRow" />
                      <PagerSettings Position="Top" />
                     <PagerStyle CssClass="grdPage" />
                 </asp:GridView> 
            </ContentTemplate>
             <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="grdDataSet" EventName="PageIndexChanging" />
                <asp:AsyncPostBackTrigger ControlID="grdDataSet" EventName="RowCommand" />
                <asp:AsyncPostBackTrigger ControlID="grdDataSet" EventName="Sorting" />
            </Triggers>
         </asp:UpdatePanel>                       
    </div>         
</asp:Content>

