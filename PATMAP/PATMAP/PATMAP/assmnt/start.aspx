<%@ Page validateRequest="false" Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="start.aspx.vb" Inherits="PATMAP.start" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register TagPrefix="patmap" TagName="assmntTabMenu" Src="~/tabmenu.ascx" %>
<asp:Content ID="tabMenu" ContentPlaceHolderID="tabMenu" runat="server">   
    <patmap:assmntTabMenu ID="subMenu" runat="server" />
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server">       
     <div class="commonContent">
       <div class="commonHeader">
             <div class="PageHeaderModule">
                <div class="Header">
                    <p class="Title">Start</p>
                </div>
            </div>
        </div>
        <div class="commonForm">           
           <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td>
                        <asp:Panel ID="pnlChoice" runat="server">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td><asp:RadioButtonList ID="rdlScenarioType" runat="server" RepeatDirection="Horizontal" AutoPostBack="True">
                                    <asp:ListItem Selected="True">Create New Scenario</asp:ListItem>
                                    <asp:ListItem>Recall Saved Scenario</asp:ListItem>
                                    </asp:RadioButtonList></td>
                                <td><asp:ImageButton ID="btnHScenarioType" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                            </tr>
                        </table>
                        </asp:Panel>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlNew" runat="server">
                            <table cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td class="label"><asp:Label ID="lblBaseModel" runat="server" Text="Select Base Tax Year Model"></asp:Label></td>
                                    <td class="field"><asp:DropDownList ID="ddlBaseModel" runat="server"></asp:DropDownList> <asp:ImageButton ID="btnHBaseModel" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                </tr>
                                <tr>
                                    <td class="label"><asp:Label ID="lblSubjectModel" runat="server" Text="Select Subject Tax Year Model"></asp:Label></td>
                                    <td class="field"><asp:DropDownList ID="ddlSubjectModel" runat="server"></asp:DropDownList> <asp:ImageButton ID="btnHSubjectModel" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                </tr>
                            </table>
                        </asp:Panel> 
                    </td>
                </tr>
                <tr>
                    <td>                                               
                        <asp:Panel ID="pnlSaved" runat="server" Visible="false">
                            <div class="groupBox">                                
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td colspan="3">&nbsp;</td>                                       
                                        <td class="field"><asp:Label ID="lblFrom" runat="server" Text="From" CssClass="labelSmall"></asp:Label></td>
                                        <td class="field"><asp:Label ID="lblTo" runat="server" Text="To" CssClass="labelSmall"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td class="label"><asp:Label ID="lblUser" runat="server" Text="User"></asp:Label></td>
                                        <td class="field"><asp:DropDownList ID="ddlUser" runat="server"></asp:DropDownList> <asp:ImageButton ID="btnHUser" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                        <td class="label" style="padding-left:30px;"><asp:Label ID="lblDateCreated" runat="server" Text="Date Created"></asp:Label></td>
                                        <td class="field">
                                            <asp:TextBox ID="txtDateFrom" runat="server" width="60" ReadOnly="true"></asp:TextBox>                                           
                                            <asp:ImageButton runat="Server" ID="btnDateFrom" ImageUrl="~/images/btnCalendar.gif" AlternateText="Click to show calendar" />
                                            <ajaxToolkit:CalendarExtender ID="calDateFrom" runat="server" TargetControlID="txtDateFrom" PopupButtonID="btnDateFrom" Format="M/d/yyyy"/>
                                        </td>
                                        <td class="field"><asp:TextBox ID="txtDateTo" runat="server" width="60" ReadOnly="true"></asp:TextBox>
                                        <asp:ImageButton runat="Server" ID="btnDateTo" ImageUrl="~/images/btnCalendar.gif" AlternateText="Click to show calendar" />
                                            <ajaxToolkit:CalendarExtender ID="calDateTo" runat="server" TargetControlID="txtDateTo" PopupButtonID="btnDateTo" Format="M/d/yyyy"/> <asp:ImageButton ID="btnHDateCreated" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                    </tr>
                                     <tr>
                                        <td class="label"><asp:Label ID="lblScenarioName" runat="server" Text="Scenario Name"></asp:Label></td>
                                        <td class="field" colspan="4"><asp:TextBox ID="txtScenarioName" runat="server" CssClass="txtLong"></asp:TextBox> <asp:ImageButton ID="btnHScenarioName" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                    </tr>
                                    <tr>
                                        <td class="label"><asp:Label ID="lblSavedBase" runat="server" Text="Base Tax Year Model"></asp:Label></td>
                                        <td class="field" colspan="4"><asp:DropDownList ID="ddlSavedBase" runat="server"></asp:DropDownList> <asp:ImageButton ID="btnHSavedBase" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                    </tr>
                                    <tr>
                                        <td class="label"><asp:Label ID="lblSavedSubject" runat="server" Text="Subject Tax Year Model"></asp:Label></td>
                                        <td class="field" colspan="4"><asp:DropDownList ID="ddlSavedSubject" runat="server"></asp:DropDownList> <asp:ImageButton ID="btnHSavedSubject" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                    </tr>
                                    <tr>
                                        <td class="label" style="height: 22px"><asp:Label ID="lblAudience" runat="server" Text="Audience"></asp:Label></td>
                                        <td class="field" colspan="4" style="height: 22px"><asp:DropDownList ID="ddlAudience" runat="server">
                                            <asp:ListItem Value="0">&lt;Please select&gt;</asp:ListItem>
                                            <asp:ListItem Value="1">Presentation</asp:ListItem>
                                            <asp:ListItem Value="2">External Users</asp:ListItem>
                                        </asp:DropDownList> <asp:ImageButton ID="btnHAudience" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>                       
                                    </tr>
                                    <tr>
                                        <td colspan="5" class="btns" align="right"><asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/btnSearch.gif" /> <asp:ImageButton ID="btnClear" runat="server" ImageUrl="~/images/btnClear.gif" /></td>
                                    </tr>
                                </table>              
                            </div>
                            <br />
                            <div class="subHeader">List</div>
                             <asp:UpdatePanel ID="uplScenario" runat="server">
                                <ContentTemplate>
                                    <div class="totalCount"><asp:Label ID="lblTotal" runat="server" Text="Saved Scenarios: "></asp:Label><asp:Label ID="txtTotal" runat="server" Text=""></asp:Label></div>
                                         <asp:GridView ID="grdStart" runat="server" CellPadding="3" AutoGenerateColumns="False" CssClass="grdLgStyle" AllowPaging="True" AllowSorting="True" DataKeyNames="assessmentTaxModelID">
                                             <Columns>                                            
                                                 <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="1%">
                                                     <ItemTemplate>
                                                         <asp:RadioButton ID="selectScenario" runat="server" GroupName="select" OnCheckedChanged="gridRowChecked" AutoPostBack="true" /> 
                                                     </ItemTemplate>
                                                 </asp:TemplateField>
                                                 <asp:BoundField HeaderText="User" DataField="name" SortExpression="name" />  
                                                 <asp:BoundField HeaderText="Scenario Name" DataField="assessmentTaxModelName" SortExpression="assessmentTaxModelName" />
                                                 <asp:BoundField HeaderText="Base Tax Year Model" DataField="baseTaxYearModelName" SortExpression="baseTaxYearModelName" />                              
                                                 <asp:BoundField HeaderText="Subject Year Tax Year Model" DataField="SubjectTaxYearModelName" SortExpression="SubjectTaxYearModelName" />
                                                 <asp:BoundField HeaderText="Description" DataField="description" SortExpression="description" />
                                                 <asp:BoundField HeaderText="Date Created" DataField="dateCreated" SortExpression="dateCreated" />
                                             </Columns>
                                             <HeaderStyle CssClass="colHeader" />
                                             <AlternatingRowStyle CssClass="alertnateRow" />
                                             <PagerSettings Position="Top" />
                                             <PagerStyle CssClass="grdPage" />
                                         </asp:GridView> 
                                </ContentTemplate>                               
                            </asp:UpdatePanel>                             
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td class="btnsBottom" style="height: 19px">                         
                        <asp:ImageButton ID="btnContinue" runat="server" ImageUrl="~/images/btnContinue.gif" />
                     </td>
                </tr>
           </table>
        </div>
                              
    </div>         
</asp:Content>


