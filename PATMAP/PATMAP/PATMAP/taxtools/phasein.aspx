<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="phasein.aspx.vb" Inherits="PATMAP.phasein" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %> 
<%@ Register TagPrefix="patmap" TagName="taxtoolsTabMenu" Src="~/tabmenu.ascx" %>

<asp:Content ID="tabMenu" ContentPlaceHolderID="tabMenu" runat="server">   
    <patmap:taxtoolsTabMenu ID="subMenu" runat="server" />
</asp:Content>

<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server">
    <div class="commonContent">  
        <div class="commonHeader">
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="width:35%; height:25px">
                        <asp:Label ID="lblSubjYr" runat="server" Text="Subject Year: " CssClass="labelTaxModel"></asp:Label> &nbsp;
                        <asp:Label ID="lblLiveSubjYr" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="lblSubjMun" runat="server" Text="Subject Municipality: " CssClass="labelTaxModel"></asp:Label> &nbsp;
                        <asp:Label ID="lblLiveSubjMun" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblStartingUniformMillRate" runat="server" Text="Starting Uniform Mill Rate: " CssClass="labelTaxModel"></asp:Label> &nbsp;
                        <asp:Label ID="lblLiveStartingUniformMillRate" runat="server" Text="0.0000"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblStartingRevenue" runat="server" Text="Starting Revenue: " CssClass="labelTaxModel"></asp:Label> &nbsp;
                        <asp:Label ID="lblLiveStartingRevenue" runat="server" Text="$00,000,000"></asp:Label>
                    </td>
                </tr>
            </table>
            <br />              
            <div class="PageHeaderModule">
                <div class="Header">
                    <p class="Title">Phase In</p>
                </div>
            </div>
        </div>
        <div class="commonForm">   
        <asp:UpdatePanel id="uplPhaseIn" runat="server" >
            <ContentTemplate>          
            <asp:Repeater ID="rptPhaseIn" runat="server">
                <HeaderTemplate>
                    <table id="tblPhaseIn" cellspacing="0" cellpadding="3" width="100%" rules="cols" style="border-collapse:collapse; border:solid 1px snow">
                        <tr class="colHeader">
                            <th>&nbsp;</th>
                            <th>Years</th>
                            <th>Threshold</th>
                            <th>Year 1</th>
                            <th>Year 2</th>
                            <th>Year 3</th>
                            <th>Year 4</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                        <tr>
                            <td colspan="3" style="height:20px; text-indent:5px"><asp:label ID="lblTaxClass" runat="server" Text='<%#Container.DataItem("taxClass")%>' Font-Bold="true"></asp:label></td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td><asp:label ID="lblTaxClassID" runat="server" Text='<%#Container.DataItem("taxClassID")%>' Visible="False"></asp:label></td>
                        </tr>
                        <tr>
                            <td style="text-align:right">Increase</td>
                            <td class="alignCenter">
                                <asp:dropdownlist ID="ddlYrsIncr" runat="server" AutoPostBack="true" OnSelectedIndexChanged="updateYrAccess">
                                    <asp:ListItem>4</asp:ListItem>
                                    <asp:ListItem>3</asp:ListItem>
                                    <asp:ListItem>2</asp:ListItem>
                                    <asp:ListItem>0</asp:ListItem>
                                </asp:dropdownlist>
                                 
                            </td>
                            <td class="alignCenter">$<asp:TextBox ID="txtThresholdIncr" runat="server" CssClass="phaseInData" Width="80px" Text='<%#Container.DataItem("thresholdIncrease")%>' AutoPostBack="true" OnTextChanged="validationSequence"></asp:TextBox></td>
                            <td class="alignCenter"><asp:TextBox ID="txtIncrYr1" runat="server" CssClass="phaseInData" Text='<%#Container.DataItem("Y1Increase")%>' AutoPostBack="true" OnTextChanged="validationSequence"></asp:TextBox>%</td>
                            <td class="alignCenter"><asp:TextBox ID="txtIncrYr2" runat="server" CssClass="phaseInData" Text='<%#Container.DataItem("Y2Increase")%>' AutoPostBack="true" OnTextChanged="validationSequence"></asp:TextBox>%</td>
                            <td class="alignCenter"><asp:TextBox ID="txtIncrYr3" runat="server" CssClass="phaseInData" Text='<%#Container.DataItem("Y3Increase")%>' AutoPostBack="true" OnTextChanged="validationSequence"></asp:TextBox>%</td>
                            <td class="alignCenter"><asp:TextBox ID="txtIncrYr4" runat="server" CssClass="phaseInData" Text='<%#Container.DataItem("Y4Increase")%>' AutoPostBack="true" OnTextChanged="validationSequence"></asp:TextBox>%</td>
                        </tr>
                        <tr style="vertical-align:top">
                            <td style="text-align:right">Decrease</td>
                            <td class="alignCenter">
                                <asp:dropdownlist ID="ddlYrsDecr" runat="server" AutoPostBack="true" OnSelectedIndexChanged="updateYrAccess">
                                    <asp:ListItem>4</asp:ListItem>
                                    <asp:ListItem>3</asp:ListItem>
                                    <asp:ListItem>2</asp:ListItem>
                                    <asp:ListItem>0</asp:ListItem>
                                </asp:dropdownlist>
                           
                            </td>
                            <td class="alignCenter">$<asp:TextBox ID="txtThresholdDecr" runat="server" CssClass="phaseInData" Width="80px" Text='<%#Container.DataItem("thresholdDecrease")%>' AutoPostBack="true" OnTextChanged="validationSequence"></asp:TextBox></td>
                            <td class="alignCenter"><asp:TextBox ID="txtDecrYr1" runat="server" CssClass="phaseInData" Text='<%#Container.DataItem("Y1Decrease")%>' AutoPostBack="true" OnTextChanged="validationSequence"></asp:TextBox>%</td>
                            <td class="alignCenter"><asp:TextBox ID="txtDecrYr2" runat="server" CssClass="phaseInData" Text='<%#Container.DataItem("Y2Decrease")%>' AutoPostBack="true" OnTextChanged="validationSequence"></asp:TextBox>%</td>
                            <td class="alignCenter"><asp:TextBox ID="txtDecrYr3" runat="server" CssClass="phaseInData" Text='<%#Container.DataItem("Y3Decrease")%>' AutoPostBack="true" OnTextChanged="validationSequence"></asp:TextBox>%</td>
                            <td class="alignCenter" style="height:40px"><asp:TextBox ID="txtDecrYr4" runat="server" CssClass="phaseInData" Text='<%#Container.DataItem("Y4Decrease")%>' AutoPostBack="false"></asp:TextBox>%</td>
                        </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                        <tr class="alternateRowLTT">
                            <td colspan="3" style="height:20px; text-indent:5px"><asp:label ID="lblTaxClass" runat="server" Text='<%#Container.DataItem("taxClass")%>' Font-Bold="true"></asp:label></td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td><asp:label ID="lblTaxClassID" runat="server" Text='<%#Container.DataItem("taxClassID")%>' Visible="False"></asp:label></td>
                        </tr>
                        <tr class="alternateRowLTT">
                            <td style="text-align:right">Increase</td>
                            <td class="alignCenter">
                                <asp:dropdownlist ID="ddlYrsIncr" runat="server" AutoPostBack="true" OnSelectedIndexChanged="updateYrAccess">
                                    <asp:ListItem>4</asp:ListItem>
                                    <asp:ListItem>3</asp:ListItem>
                                    <asp:ListItem>2</asp:ListItem>
                                    <asp:ListItem>0</asp:ListItem>
                                </asp:dropdownlist>
                                 
                            </td>
                            <td class="alignCenter">$<asp:TextBox ID="txtThresholdIncr" runat="server" CssClass="phaseInData" Width="80px" Text='<%#Container.DataItem("thresholdIncrease")%>' AutoPostBack="true" OnTextChanged="validationSequence"></asp:TextBox></td>
                            <td class="alignCenter"><asp:TextBox ID="txtIncrYr1" runat="server" CssClass="phaseInData" Text='<%#Container.DataItem("Y1Increase")%>' AutoPostBack="true" OnTextChanged="validationSequence"></asp:TextBox>%</td>
                            <td class="alignCenter"><asp:TextBox ID="txtIncrYr2" runat="server" CssClass="phaseInData" Text='<%#Container.DataItem("Y2Increase")%>' AutoPostBack="true" OnTextChanged="validationSequence"></asp:TextBox>%</td>
                            <td class="alignCenter"><asp:TextBox ID="txtIncrYr3" runat="server" CssClass="phaseInData" Text='<%#Container.DataItem("Y3Increase")%>' AutoPostBack="true" OnTextChanged="validationSequence"></asp:TextBox>%</td>
                            <td class="alignCenter"><asp:TextBox ID="txtIncrYr4" runat="server" CssClass="phaseInData" Text='<%#Container.DataItem("Y4Increase")%>' AutoPostBack="true" OnTextChanged="validationSequence"></asp:TextBox>%</td>
                        </tr>
                        <tr class="alternateRowLTT" style="vertical-align:top">
                            <td style="text-align:right">Decrease</td>
                            <td class="alignCenter">
                                <asp:dropdownlist ID="ddlYrsDecr" runat="server" AutoPostBack="true" OnSelectedIndexChanged="updateYrAccess">
                                    <asp:ListItem>4</asp:ListItem>
                                    <asp:ListItem>3</asp:ListItem>
                                    <asp:ListItem>2</asp:ListItem>
                                    <asp:ListItem>0</asp:ListItem>
                                </asp:dropdownlist>
                                
                            </td>
                            <td class="alignCenter">$<asp:TextBox ID="txtThresholdDecr" runat="server" CssClass="phaseInData" Width="80px" Text='<%#Container.DataItem("thresholdDecrease")%>' AutoPostBack="true" OnTextChanged="validationSequence"></asp:TextBox></td>
                            <td class="alignCenter"><asp:TextBox ID="txtDecrYr1" runat="server" CssClass="phaseInData" Text='<%#Container.DataItem("Y1Decrease")%>' AutoPostBack="true" OnTextChanged="validationSequence"></asp:TextBox>%</td>
                            <td class="alignCenter"><asp:TextBox ID="txtDecrYr2" runat="server" CssClass="phaseInData" Text='<%#Container.DataItem("Y2Decrease")%>' AutoPostBack="true" OnTextChanged="validationSequence"></asp:TextBox>%</td>
                            <td class="alignCenter"><asp:TextBox ID="txtDecrYr3" runat="server" CssClass="phaseInData" Text='<%#Container.DataItem("Y3Decrease")%>' AutoPostBack="true" OnTextChanged="validationSequence"></asp:TextBox>%</td>
                            <td class="alignCenter" style="height:40px"><asp:TextBox ID="txtDecrYr4" runat="server" CssClass="phaseInData" Text='<%#Container.DataItem("Y4Decrease")%>' AutoPostBack="true" OnTextChanged="validationSequence"></asp:TextBox>%</td>
                        </tr>
                </AlternatingItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
            </ContentTemplate>
        </asp:UpdatePanel>
            <br />
            <br />
            <table cellpadding="3" cellspacing="0" border="0" width="100%">             
                <tr>
                    <td class="btnsBottom" colspan="4">
                        <asp:ImageButton ID="btnSubmit" runat="server" ImageUrl="~/images/btnSubmit.gif" /> <asp:ImageButton ID="btnClear" runat="server" ImageUrl="~/images/btnClear.gif" />
                        <hr />
                    </td>
                </tr>
            </table>
            
            <table width="100%" cellspacing="0" cellpadding="3" class="displayRevisions" rules="all" style="border-collapse:collapse; border:solid 1px snow" >
                <tr class="colHeader">
                    <td colspan="2" style="height:25px">&nbsp;</td>
                    <td style="text-align:center">Year 1</td>
                    <td style="text-align:center">Year 2</td>
                    <td style="text-align:center">Year 3</td>
                    <td style="text-align:center">Year 4</td>            
                </tr>
                <tr>
                    <td style="height:25px; text-align:left; text-indent:5px; border-left:hidden">Revised Uniform Mill Rate</td>
                    <td><asp:TextBox ID="txtRevisedUMR" runat="server" cssclass="phaseInTotal" Text="" Enabled="False" Width="65px"></asp:TextBox></td>
                    <td colspan="4">&nbsp;</td>
                    
                </tr>
                <tr class="alternateRowLTT">
                    <td style="height:25px; text-align:left; text-indent:5px; border-left:hidden">Model Revenue without Phase-In</td>
                    <td>&nbsp;</td>
                    <td><asp:TextBox ID="txtRevY1" runat="server" cssclass="phaseInTotal" Text="" Enabled="False"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtRevY2" runat="server" cssclass="phaseInTotal" Text="" Enabled="False"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtRevY3" runat="server" cssclass="phaseInTotal" Text="" Enabled="False"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtRevY4" runat="server" cssclass="phaseInTotal" Text="" Enabled="False"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="height:25px; text-align:left; text-indent:5px; border-left:hidden">Model Revenue With Phase-In</td>
                    <td>&nbsp;</td>
                    <td><asp:TextBox ID="txtRevPhaseInY1" runat="server" cssclass="phaseInTotal" Text="" Enabled="False"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtRevPhaseInY2" runat="server" cssclass="phaseInTotal" Text="" Enabled="False"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtRevPhaseInY3" runat="server" cssclass="phaseInTotal" Text="" Enabled="False"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtRevPhaseInY4" runat="server" cssclass="phaseInTotal" Text="" Enabled="False"></asp:TextBox></td>
                </tr>
                <tr class="alternateRowLTT">
                    <td style="height:25px; text-align:left; text-indent:5px; border-left:hidden">Phase-In Revenue Impact</td>
                    <td>&nbsp;</td>
                    <td><asp:TextBox ID="txtImpactY1" runat="server" cssclass="phaseInTotal" Text="" Enabled="False"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtImpactY2" runat="server" cssclass="phaseInTotal" Text="" Enabled="False"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtImpactY3" runat="server" cssclass="phaseInTotal" Text="" Enabled="False"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtImpactY4" runat="server" cssclass="phaseInTotal" Text="" Enabled="False"></asp:TextBox></td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
