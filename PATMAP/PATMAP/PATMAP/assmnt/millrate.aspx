<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="millrate.aspx.vb" Inherits="PATMAP.millrate" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register TagPrefix="patmap" TagName="assmntTabMenu" Src="~/tabmenu.ascx" %>
<asp:Content ID="tabMenu" ContentPlaceHolderID="tabMenu" runat="server">   
    <patmap:assmntTabMenu ID="subMenu" runat="server" />
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server">       
     <div class="commonContent">
        <table cellpadding="0" cellspacing="0" border="0" width="92%">
            <tr>
                <td colspan="3" style="padding-left:10px;padding-bottom:15px;">
                    <p>Fields marked with an asterisk (<span class="requiredField">*</span>) are required.</p>
                </td>
            </tr>        
            <tr>                                
                <td class="label" style="padding-left:10px;width:100px;"><asp:Label ID="lblScenarioName" runat="server" Text="Scenario Name:"></asp:Label><span class="requiredField">*</span></td>
                <td class="field">
                    <table cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td>
                                 <asp:UpdatePanel ID="uplName" runat="server">
                                    <ContentTemplate><asp:TextBox ID="txtScenarioName" runat="server" CssClass="txtLong" OnTextChanged="changeName" AutoPostBack="True"></asp:TextBox></ContentTemplate>
                                 </asp:UpdatePanel>
                            </td>
                            <td><asp:ImageButton ID="btnHScenarioName" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                        </tr>
                    </table>
                </td>         
                <td align="right" style="width:58px;"><asp:ImageButton ID="btnSave" runat="server" ImageUrl="~/images/btnSave.gif" /></td>
            </tr>
        </table>
       <div class="commonHeader">
             <div class="PageHeaderModule">
                <div class="Header">
                    <p class="Title">Provincial Mill Rate</p>
                </div>
            </div>
        </div>     
        <div class="commonForm">           
           <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td class="labelBase"><asp:Label ID="lblBase" runat="server" Text="Base Tax Year Model:" CssClass="labelTaxModel"></asp:Label></td>
                    <td style="width: 30%;padding-left:10px;"><asp:Label ID="txtBaseTaxYrModel" runat="server" Text=""></asp:Label></td>
                    <td class="labelSubject"><asp:Label ID="lblSubject" runat="server" Text="Subject Tax Year Model:" CssClass="labelTaxModel"></asp:Label></td>
                    <td style="width: 30%;padding-left:10px;"><asp:Label ID="txtSubjectTaxYrModel" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <td colspan="4">
                        <br />
                        <p><b>Scenario Provincial Mill Rate</b></p>                        
                         <asp:UpdatePanel ID="uplMillRate" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="grdMillRate" runat="server" CellPadding="3" AutoGenerateColumns="False" CssClass="grdSmStyle" DataKeyNames="taxClassID">
                                 <Columns>                                            
                                     <asp:ButtonField Text="&lt;img src=&quot;../images/btnExpand.gif&quot; &gt;" CommandName="expandClass" ItemStyle-HorizontalAlign="Center"/>
                                     <asp:BoundField HeaderText="Tax Class" DataField="taxClass"/>
                                     <asp:TemplateField HeaderText ="Provincial Education Mill Rate (Mill)" HeaderStyle-Wrap="true" HeaderStyle-Width="130">
                                         <ItemTemplate>
                                             <asp:TextBox ID="txtMillRate" runat="server" Width="100" Text='<%# DataBinder.Eval(Container.DataItem,"PMR","{0:N4}") %>' AutoPostBack="true" OnTextChanged="valueChanged"></asp:TextBox>
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Action" >
                                         <ItemTemplate>
                                             <asp:RadioButtonList ID="rdlAction" runat="server" AutoPostBack="true" OnSelectedIndexChanged="valueChanged">
                                                <asp:ListItem Text="Replacement" Value="Replacement"></asp:ListItem>
                                                <asp:ListItem Text="In Addition" Value="In Addition"></asp:ListItem>
                                             </asp:RadioButtonList>
                                         </ItemTemplate>
                                     </asp:TemplateField>  
                                     <asp:TemplateField HeaderText="Assessment for Subject Year Local Levy" HeaderStyle-Width="150">
                                         <ItemTemplate>
                                             <asp:RadioButtonList ID="rdlLocalLevy" runat="server" AutoPostBack="true" OnSelectedIndexChanged="valueChanged">
                                                <asp:ListItem Text="Include Assessment" Value="Include Assessment"></asp:ListItem>
                                                <asp:ListItem Text="Exclude Assessment" Value="Exclude Assessment"></asp:ListItem>
                                             </asp:RadioButtonList>
                                         </ItemTemplate>
                                     </asp:TemplateField>                                                                  
                                 </Columns>
                                 <HeaderStyle CssClass="colHeader" />
                                 <AlternatingRowStyle CssClass="alertnateRow" />                         
                                </asp:GridView> 
                            </ContentTemplate>
                          </asp:UpdatePanel>                        
                    </td>
                </tr>               
                <tr>
                    <td class="btnsBottom" colspan="4"><asp:ImageButton ID="btnSubmit" runat="server" ImageUrl="~/images/btnSubmit.gif" /> <asp:ImageButton ID="btnClear" runat="server" ImageUrl="~/images/btnClear.gif" /></td>
                </tr>
           </table>
        </div> 
                              
    </div>         
</asp:Content>
