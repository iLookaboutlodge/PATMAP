<%@ Page ValidateRequest="false" Language="vb" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="false" Codebehind="loadmillrate.aspx.vb" Inherits="PATMAP.loadmillrate" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register TagPrefix="patmap" TagName="dataTabMenu" Src="~/tabmenu.ascx" %>
<asp:Content ID="tabMenu" ContentPlaceHolderID="tabMenu" runat="server">
    <patmap:dataTabMenu ID="subMenu" runat="server"></patmap:dataTabMenu>
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server">
    <div class="commonContent">
        <div class="commonHeader">
            <div class="PageHeaderModule">
                <div class="Header">
                    <p class="Title">
                        Mill Rate Survey</p>
                </div>
            </div>
        </div>
        <div class="commonForm">
            <%--<p>
                All fields are required.</p>--%>
            <table cellpadding="0" cellspacing="0" border="0">
                <%--<tr>
                    <td colspan="3">
                        <asp:Label ID="lb_Message" runat="server" Width="100%" ForeColor="#669966"></asp:Label></td>
                </tr>--%>
                <tr id="tr_XlsColumnsInRowNumber" runat="server" visible="false">
                    <td>
                        <asp:Label ID="lbXlsColumnsInRowNumber" runat="server" Text="User must identify which row<br> number contains column headers."
                            ForeColor="red"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txtXlsColumnsInRowNumber" runat="server" />
                        <asp:RangeValidator ID="RangeValidator9" ControlToValidate="txtXlsColumnsInRowNumber"
                            Type="Integer" MinimumValue="1" MaximumValue="99" Text="Only Int type from 1 to 99"
                            runat="server" />
                    </td>
                </tr>
                <tr id="tr_newFile" runat="server">
                    <td class="label">
                        <asp:Label ID="lblFile" runat="server" Text="File"></asp:Label></td>
                    <td class="field" style="width: 224px">
                        <asp:FileUpload ID="fpFile" runat="server" Width="200px" /><asp:ImageButton ID="btnHFile"
                            runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp">
                        </asp:ImageButton></td>
                    <td class="field">
                        <asp:ImageButton ID="btnFileUpload" runat="server" ImageUrl="~/images/btnLoad.gif"
                            OnClick="btnFileUpload_Click" />
                    </td>
                </tr>
                <tr id="tr_ddlTableNames" runat="server" visible="false">
                    <td class="label">
                        <asp:Label ID="Label1" runat="server" Text="Select Table Name"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:DropDownList ID="ddlTableNames" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTableNames_SelectedIndexChanged" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr id="tr_DataSet" runat="server" visible="false">
                    <td class="label">
                        <asp:Label ID="lblDataSet" runat="server" Text="Data Set Name"></asp:Label></td>
                    <td class="field" colspan="2">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td>
                                    <asp:Label ID="lblExisting" runat="server" Text="Existing&nbsp;&nbsp;"></asp:Label></td>
                                <td>
                                    <asp:DropDownList ID="ddlDSN" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlDSN_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:ImageButton ID="btnHDSN" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False"
                                        CssClass="btnHelp"></asp:ImageButton>&nbsp;<br />
                                </td>
                            </tr>
                            <%--<tr id="tr_rb" runat="server">
                                <td>
                                    <asp:RadioButtonList ID="rb" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Selected="True">Insert</asp:ListItem>
                                        <asp:ListItem>Update</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>--%>
                            <tr id="tr_txtNewDSN" runat="server">
                                <td align="center">
                                    <asp:Label ID="lblOr" runat="server" Text="Or New"></asp:Label></td>
                                <td>
                                    <asp:TextBox ID="txtNewDSN" runat="server" CssClass="txtLong" AutoPostBack="True"
                                        OnTextChanged="txtNewDSN_TextChanged"></asp:TextBox>
                                    <asp:ImageButton ID="btnHNewDSN" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False"
                                        CssClass="btnHelp"></asp:ImageButton>
                                </td>
                            </tr>
                            <tr id="tr_Year" runat="server">
                                <td class="label">
                                    <asp:Label ID="lblYear" runat="server" Text="Year"></asp:Label>
                                    <span class="requiredField">*</span></td>
                                <td class="field">
                                    <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:ImageButton ID="btnHYear" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False"
                                        CssClass="btnHelp"></asp:ImageButton></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="tr_btnLoad" runat="server" visible="false">
                    <td>
                    </td>
                    <td class="btns" style="height: 25px" colspan="2">
                        <asp:ImageButton ID="btnLoad" runat="server" ImageUrl="~/images/btnLoad.gif" OnClientClick="return confirmPrompt('Do you want to load millrate data?');"/>
                        <%--OnClick="btnLoad_Click" />--%>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
