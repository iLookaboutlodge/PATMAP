<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="main.aspx.vb" Inherits="PATMAP.main" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>  
<%@ Register TagPrefix="patmap" TagName="boundaryTabMenu" Src="~/tabmenu.ascx" %>
<asp:Content ID="tabMenu" ContentPlaceHolderID="tabMenu" runat="server">   
    <patmap:boundaryTabMenu ID="subMenu" runat="server" />
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server">       
     <div class="commonContent">        
       <div class="commonHeader">
             <div class="PageHeaderModule">
                <div class="Header">
                    <p class="Title"><asp:Label ID="lblTitle" runat="server" Text="Boundary Changes"></asp:Label></p>
                </div>
            </div>
        </div>     
        <div class="commonForm">
<%--        <asp:UpdatePanel ID="uplSubjectMun" runat="server">
            <ContentTemplate>--%>
          
          
           <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td class="label" style="height: 22px"><asp:Label ID="lblSubjMun" runat="server" Text="Select Subject Municipality"></asp:Label></td>
                    <td class="field" style="width: 76%; height: 22px;"><asp:DropDownList ID="ddlSubjMun" runat="server" AutoPostBack="True"></asp:DropDownList> <asp:ImageButton ID="btnHSubjMun" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr> 
                <tr>
                    <td colspan="2">
                       <div class="groupBox">
                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                <tr>
                                    <td class="label"><asp:Label ID="lblOriginMun" runat="server" Text="Origin Municipality"></asp:Label></td>
                                    <td class="field" style="width: 73%;"><asp:DropDownList ID="ddlOriginMun" runat="server" AutoPostBack="True"></asp:DropDownList> <asp:ImageButton ID="btnHOriginMun" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                </tr>
                                 <tr>
                                    <td class="label"><asp:Label ID="lblDestinationMun" runat="server" Text="Destination Municipality"></asp:Label></td>
                                    <td class="field"><asp:DropDownList ID="ddlDestinationMun" runat="server" AutoPostBack="True"></asp:DropDownList> <asp:ImageButton ID="btnHDestinationMun" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                </tr>
                                 <tr>
                                    <td class="label"><asp:Label ID="lblGroupName" runat="server" Text="Group Name"></asp:Label></td>
                                    <td class="field"><asp:TextBox ID="txtGroupName" runat="server"></asp:TextBox> <asp:ImageButton ID="btnHGroupName" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                </tr>
                                <tr>
                                    <asp:Panel ID="pnlRestructuredLevy" runat="server">
                                    <td class="label"><asp:Label ID="lblRestructuredLevy" runat="server" Text="Restructured Levy"></asp:Label></td>
                                    <td class="field">
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td>
                                                    <asp:RadioButtonList ID="rblRestructuredLevy" runat="server" RepeatDirection="Horizontal">
                                                        <asp:ListItem>Combine</asp:ListItem>
                                                        <asp:ListItem>Separate</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                                <td valign="top" style="width: 20px"><asp:ImageButton ID="btnHRestructuredLevy" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                            </tr>
                                        </table>
                                     </td>
                                     </asp:Panel>
                                </tr>                                
                            </table>  
                       </div>
                       <br /><br />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="height: 127px">
                         <div class="groupBox">
                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                <tr>
                                    <td class="label" colspan="2"><asp:Label ID="lblTransfer" runat="server" Text="Transfer Properties" Font-Bold="true"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="label" style="width: 215px; height: 24px;"><asp:Label ID="lblParcelNo" runat="server" Text="Parcel No. or Parcel Prefix"></asp:Label></td>
                                    <td class="field" style="width: 73%; height: 24px;"><asp:TextBox ID="txtParcelNo" runat="server" CssClass="txtNormal"></asp:TextBox> 
                                        <asp:CheckBox ID="chkSelectAll" runat="server" Text="Select All Parcels" CausesValidation="True" AutoPostBack="True"  />
                                        <asp:ImageButton ID="btnHParcelNo" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                               </tr>
                                <tr>
                                    <td align="right" style="width: 215px">&nbsp;</td>
                                    <td class="field"><asp:ImageButton ID="btnLinearAdj" runat="server" ImageUrl="~/images/btnLinearAdj.gif" /> <asp:ImageButton ID="btnHLinearAdj" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                </tr>
                                <tr>
                                    <td align="right" valign="top" style="width: 215px; height: 23px;">OR</td>
                                    <td class="field" style="height: 23px"><asp:ImageButton ID="btnUseMap" runat="server" ImageUrl="~/images/btnUseMap.gif" /> <asp:ImageButton ID="btnHUseMap" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="btns" align="right" style="height: 19px"><asp:ImageButton ID="btnTransfer" runat="server" ImageUrl="~/images/btnTransfer.gif" /></td>
                                </tr>
                            </table>
                         </div>
                    </td>
                </tr> 
           </table>
<%--             </ContentTemplate>
        </asp:UpdatePanel>--%>
            <br /><br />
            <div class="subHeader" style="width:679px">Transferred Properties</div>
            <asp:GridView ID="grdProperties" runat="server" CellPadding="3" AutoGenerateColumns="False" CssClass="grdLgStyle" AllowPaging="True" DataKeyNames="boundarytransferID,boundaryGroupID">
                 <Columns>                         
                     <asp:ButtonField Text="&lt;img src=&quot;../images/btnSmDelete.gif&quot; &gt;" CommandName="deleteTransfer" >
                         <ItemStyle CssClass="colDelete" />
                     </asp:ButtonField>
                     <asp:BoundField HeaderText="Origin Municipality" DataField="originMun" />
                     <asp:BoundField HeaderText="Parcel No or Parcel Prefix" DataField="alternate_parcelID" />
                     <asp:BoundField HeaderText="Destination Municipality" DataField="destinationMun" />                   
                 </Columns>
                 <HeaderStyle CssClass="colHeader" />
                 <AlternatingRowStyle CssClass="alertnateRow" />
                 <PagerSettings Position="Top" />
                 <PagerStyle CssClass="grdPage" />
             </asp:GridView> 
             <br />
             <div class="btnsTop"><asp:ImageButton ID="btnSubmit" runat="server" ImageUrl="~/images/btnSubmit.gif" CssClass="btns" Visible="False" /></div>
             <div class="clear"></div>
             <br /><br />
             <div class="subHeader" style="width:679px">Assessment and Tax Shifts</div>
            <asp:GridView ID="grdAssessment" runat="server" CellPadding="3" AutoGenerateColumns="False" CssClass="grdLgStyle" AllowPaging="True" DataKeyNames="boundaryGroupID,originMunicipalityID,destinationMunicipalityID" OnRowDataBound="grdAssessment_RowDataBound">
                 <Columns>  
                     <asp:CommandField ButtonType="Image" CancelImageUrl="~/images/btnSmCancel.gif" DeleteImageUrl="~/images/btnSmDelete.gif"
                         EditImageUrl="~/images/btnSmEdit.gif" InsertVisible="False" ShowDeleteButton="True"
                         ShowEditButton="True" UpdateImageUrl="~/images/btnSmSave.gif" UpdateText="Save"/>
                     <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="applyLTT" runat="server" ImageUrl="~/images/btnSmAddTaxTools.gif" ToolTip="Apply to Local Tax Tools" CommandName="applyLTT" />
                        </ItemTemplate>
                     </asp:TemplateField>
                     <asp:BoundField HeaderText="Group Name" DataField="boundaryGroupName" ReadOnly="True" />
                     <asp:BoundField HeaderText="Levy Status" DataField="LevyStatus" ReadOnly="True" />
                     <asp:BoundField HeaderText="Assessment" DataField="assessment" DataFormatString="{0:n0}" ReadOnly="True" HtmlEncode="False" >
                         <ItemStyle HorizontalAlign="Right" />
                     </asp:BoundField>
                     <asp:BoundField HeaderText="Group Levy (Origin Mun)" DataField="originLevy" DataFormatString="{0:c2}" ReadOnly="True" HtmlEncode="False" >
                         <ItemStyle HorizontalAlign="Right" />
                     </asp:BoundField>                      
                     <asp:BoundField HeaderText="Group Levy (Subject Mun)" DataField="originalLevy" DataFormatString="{0:c2}" ReadOnly="True" HtmlEncode="False" >
                         <ItemStyle HorizontalAlign="Right" />
                     </asp:BoundField>                                                          
                     <asp:BoundField DataField="restructuredLevy" DataFormatString="{0:c2}" HeaderText="Restructured Levy (Subject Mun)" HtmlEncode="False">
                         <ItemStyle HorizontalAlign="Right" />
                     </asp:BoundField>
                     <asp:BoundField DataField="uniformMillRate" DataFormatString="{0:f4}" HeaderText="Restructured UMR (Subject Mun)" HtmlEncode="False" ReadOnly="True">
                         <ItemStyle HorizontalAlign="Right" />
                     </asp:BoundField>                     
                 </Columns>
                 <HeaderStyle CssClass="colHeader" />
                 <AlternatingRowStyle CssClass="alertnateRow" />
                 <PagerSettings Position="Top" />
                 <PagerStyle CssClass="grdPage" />
             </asp:GridView> <br />
            <br />
             <div class="subHeader" style="vertical-align:middle; width:679px">Duplicate Properties <asp:ImageButton Visible="false" ID="btnPrintDpl" runat="server" ImageUrl="~/images/btnExport.gif" CssClass="btns" ImageAlign="Right" Height="16px" style="position:relative; left:459px"/>
                 </div>
             <asp:GridView ID="grdDuplicateProp" runat="server" style="width:685px" CellPadding="3" AutoGenerateColumns="False" CssClass="grdLgStyle" AllowPaging="True" DataKeyNames="boundaryGroupID" >
                 <Columns>  
                     <asp:BoundField HeaderText="Group Name" DataField="boundaryGroupName"/>
                     <asp:BoundField HeaderText="Parcel No." DataField="alternate_parcelID">
                         <ItemStyle HorizontalAlign="Right" />
                     </asp:BoundField>
                     <asp:BoundField HeaderText="Assessment" DataField="assessment" DataFormatString="{0:n0}" HtmlEncode="False" >
                         <ItemStyle HorizontalAlign="Right" />
                     </asp:BoundField>                   
                 </Columns>
                 <HeaderStyle CssClass="colHeader" />
                 <AlternatingRowStyle CssClass="alertnateRow" />
                 <PagerSettings Position="Top" />
                 <PagerStyle CssClass="grdPage" />
             </asp:GridView>              &nbsp;
        </div>                               
    </div>         
</asp:Content>