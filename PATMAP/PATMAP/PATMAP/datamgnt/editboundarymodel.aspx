<%@ Page Language="vb" validateRequest="false" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="editboundarymodel.aspx.vb" Inherits="PATMAP.editboundarymodel" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server">
    <script language="javascript" type="text/javascript" src="../js/general.js"></script>       
     <div class="commonContent">
       <div class="commonHeader">
             <div class="PageHeaderModule">
                <div class="Header">
                    <p class="Title"><asp:Label ID="lblTitle" runat="server" Text="Edit Boundary Model"></asp:Label></p>
                </div>
            </div>
        </div>
        <div class="commonForm">
            <div class="btnsTop"><asp:ImageButton ID="btnSave" runat="server" ImageUrl="~/images/btnSave.gif" /> <asp:ImageButton ID="btnCancel" runat="server" ImageUrl="~/images/btnCancel.gif" /></div><br />
            <div class="clear"></div>
            <p>Fields marked with an asterisk (<span class="requiredField">*</span>) are required.</p>
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td class="label"><asp:Label ID="lblBoundaryModel" runat="server" Text="Boundary Model"></asp:Label> <span class="requiredField">*</span></td>
                    <td class="field" colspan="2"><asp:TextBox ID="txtBoundaryModel" runat="server" CssClass="txtLong"></asp:TextBox> <asp:ImageButton ID="btnHBoundaryModel" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr>  
               <tr>
                    <td class="label"><asp:Label ID="lblYear" runat="server" Text="Year"></asp:Label> <span class="requiredField">*</span></td>
                    <td class="field" colspan="2"><asp:DropDownList ID="ddlYear" runat="server">
                        </asp:DropDownList> <asp:ImageButton ID="btnHYear" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr>
                <tr>
                    <td class="label"><asp:Label ID="lblStatus" runat="server" Text="Status"></asp:Label> <span class="requiredField">*</span></td>
                    <td class="field" colspan="2">
                        <asp:DropDownList ID="ddlStatus" runat="server">
                            <asp:ListItem>&lt;select&gt;</asp:ListItem>
                            <asp:ListItem>Active</asp:ListItem>
                            <asp:ListItem>Inactive</asp:ListItem>
                            <asp:ListItem>History</asp:ListItem>
                        </asp:DropDownList> <asp:ImageButton ID="btnHStatus" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr>                              
                 <tr>
                    <td class="label">
                        <asp:Label ID="lblNotes" runat="server" Text="Notes"></asp:Label>
                    </td>
                    <td class="field">
                        <asp:TextBox ID="txtNotes" runat="server" TextMode="MultiLine" CssClass="txtArea"></asp:TextBox>
                    </td>
                    <td valign="top"><asp:ImageButton ID="btnHNotes" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr> 
                <tr>
                    <td class="label" style="height: 22px"><asp:Label ID="lblAssessmentDS" runat="server" Text="Assessment Data Set"></asp:Label> <span class="requiredField">*</span></td>
                    <td class="field" colspan="2" style="height: 22px"><asp:DropDownList ID="ddlAssessmentDS" runat="server">
                        </asp:DropDownList> <asp:ImageButton ID="btnHAssessmentDS" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr>
                <tr>
                    <td class="label" style="height: 22px"><asp:Label ID="lblBaseAssessmentDS" runat="server" Text="Base Assessment Data Set"></asp:Label> <span class="requiredField">*</span></td>
                    <td class="field" colspan="2" style="height: 22px"><asp:DropDownList ID="ddlBaseAssessmentDS" runat="server">
                        </asp:DropDownList> <asp:ImageButton ID="btnHBaseAssessmentDS" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr>                
                <tr>
                    <td class="label" style="height: 22px"><asp:Label ID="lblMillRateDS" runat="server" Text="Mill Rate Survey Data Set"></asp:Label> <span class="requiredField">*</span></td>
                    <td class="field" colspan="2" style="height: 22px"><asp:DropDownList ID="ddlMillRateDS" runat="server">
                        </asp:DropDownList> <asp:ImageButton ID="btnHMillRateDS" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr>                 
                 <tr>
                    <td class="label"><asp:Label ID="lblPOVDS" runat="server" Text="POV Data Set"></asp:Label> <span class="requiredField">*</span></td>
                    <td class="field" colspan="2"><asp:DropDownList ID="ddlPOVDS" runat="server">
                        </asp:DropDownList> <asp:ImageButton ID="btnHPOVDS" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr> 
                 <tr>
                    <td class="label"><asp:Label ID="lblBasePOVDS" runat="server" Text="Base POV Data Set"></asp:Label> <span class="requiredField">*</span></td>
                    <td class="field" colspan="2"><asp:DropDownList ID="ddlBasePOVDS" runat="server">
                        </asp:DropDownList> <asp:ImageButton ID="btnHBasePOVDS" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr>                                             
                <tr>
                    <td class="label"><asp:Label ID="lblAttachFile" runat="server" Text="Attach File"></asp:Label></td>
                    <td class="field" colspan="2"><asp:FileUpload ID="fpAttachFile" runat="server" /> <asp:ImageButton ID="btnHAttachFile" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton> <asp:ImageButton ID="btnAttach" runat="server" ImageUrl="~/images/btnAttach.gif" /></td>                    
                </tr>
                <tr>                
                    <td class="label">&nbsp;</td>
                    <td class="field" colspan="2">
                        <asp:GridView ID="grdFiles" runat="server" CellPadding="3" AutoGenerateColumns="False" CssClass="grdLgStyle" DataKeyNames="fileID,filename">
                             <Columns>                                                                                          
                                 <asp:HyperLinkField HeaderText="Attached Files" DataTextField="filename" />                                
                                  <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmDelete.gif&quot; /&gt;" CommandName="deleteFile" >
                                     <ItemStyle CssClass="colDelete" />
                                 </asp:ButtonField>    
                             </Columns>
                             <HeaderStyle CssClass="colHeader" />
                             <AlternatingRowStyle CssClass="alertnateRow" />
                             <PagerSettings Position="Top" />
                         </asp:GridView> 
                    </td>
                </tr>
            </table>
        </div>              
    </div>         
</asp:Content>

