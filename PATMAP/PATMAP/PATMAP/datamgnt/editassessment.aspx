<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    Codebehind="editassessment.aspx.vb" Inherits="PATMAP.editassessment" %>
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
                    <p class="Title">
                        <asp:Label ID="lblTitle" runat="server" Text="Edit Assessment Data"></asp:Label></p>
                </div>
            </div>
        </div>
        <div class="commonForm">
            <div class="btnsTop">
                <asp:ImageButton ID="btnSave" runat="server" ImageUrl="~/images/btnSave.gif" OnClick="btnSave_Click"
                    TabIndex="16" />
                <asp:ImageButton ID="btnCancel" runat="server" ImageUrl="~/images/btnCancel.gif"
                    OnClick="btnCancel_Click" TabIndex="17" /></div>
            <br />
            <div class="clear">
            </div>
            <p>
                Fields marked with an asterisk (<span class="requiredField">*</span>) are required.</p>
            <table cellpadding="0" cellspacing="0" border="0">
                <%--<tr>
                    <td colspan="2">
                        <asp:Label ID="lb_Message" runat="server" Width="100%" ForeColor="#669966"></asp:Label></td>
                </tr>--%>
                <tr id="tr_DSN" runat="server">
                    <td class="label">
                        <asp:Label ID="lblDSN" runat="server" Text="Data Set Name"></asp:Label>
                        <span class="requiredField">*</span></td>
                    <td class="field" colspan="2">
                        <asp:DropDownList ID="ddlDSN" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlDSN_SelectedIndexChanged"
                            TabIndex="1">
                        </asp:DropDownList>
                        <asp:ImageButton ID="btnHDSN" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False"
                            CssClass="btnHelp"></asp:ImageButton></td>
                </tr>
                <%-- <tr id="tr_Year" runat="server">
                    <td class="label">
                        <asp:Label ID="lblYear" runat="server" Text="Year"></asp:Label>
                        <span class="requiredField">*</span></td>
                    <td class="field">
                        <asp:DropDownList ID="ddlYear" runat="server">
                        </asp:DropDownList>
                        <asp:ImageButton ID="btnHYear" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False"
                            CssClass="btnHelp"></asp:ImageButton></td>
                </tr>--%>
                <tr>
                    <td class="label">
                        <asp:Label ID="lblAlternateParcelID" runat="server" Text="Alternate Parcel ID"></asp:Label>
                        <span class="requiredField">*</span></td>
                    <td class="field" colspan="2">
                        <asp:TextBox ID="txtAlternateParcelID" runat="server" CssClass="txtLong" TabIndex="2"></asp:TextBox>
                        <asp:ImageButton ID="btnHAlternateID" runat="server" ImageUrl="~/images/btnHelp.gif"
                            Visible="False" CssClass="btnHelp"></asp:ImageButton>
                        <asp:RangeValidator ID="RangeValidator9" ControlToValidate="txtAlternateParcelID"
                            Type="Integer" MinimumValue="0" MaximumValue="2147483647" Text="Only Int type less then 2147483647"
                            runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label ID="lblParcelID" runat="server" Text="Parcel Number"></asp:Label>
                        <span class="requiredField">*</span></td>
                    <td class="field" colspan="2">
                        <asp:TextBox ID="txtParcelID" runat="server" CssClass="txtLong" TabIndex="2"></asp:TextBox>
                        <asp:ImageButton ID="btnHParcelNo" runat="server" ImageUrl="~/images/btnHelp.gif"
                            Visible="False" CssClass="btnHelp"></asp:ImageButton>
                        <asp:RangeValidator ID="RangeValidator8" ControlToValidate="txtParcelID" Type="Integer"
                            MinimumValue="0" MaximumValue="2147483647" Text="Only Int type less then 2147483647"
                            runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label ID="lblMunicipality" runat="server" Text="Municipality"></asp:Label>
                        <span class="requiredField">*</span></td>
                    <td class="field" colspan="2">
                        <asp:DropDownList ID="ddlMunicipality" runat="server" TabIndex="3">
                        </asp:DropDownList>
                        <asp:ImageButton ID="btnHMunicipality" runat="server" ImageUrl="~/images/btnHelp.gif"
                            Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label ID="lblSchoolDivision" runat="server" Text="School Division"></asp:Label>
                        <span class="requiredField">*</span></td>
                    <td class="field" colspan="2">
                        <asp:DropDownList ID="ddlSchoolDivision" runat="server" TabIndex="4">
                        </asp:DropDownList>
                        <asp:ImageButton ID="btnHSchoolDivision" runat="server" ImageUrl="~/images/btnHelp.gif"
                            Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr>
                <tr>
                    <td class="label" style="height: 38px">
                        <asp:Label ID="lblLegalLand" runat="server" Text="Legal Land"></asp:Label></td>
                    <td class="field" style="height: 38px">
                        <asp:TextBox ID="txtLegalLand" TextMode="MultiLine" CssClass="txtArea" runat="server"
                            MaxLength="250" TabIndex="5"></asp:TextBox></td>
                    <td valign="top" style="height: 38px">
                        <asp:ImageButton ID="btnHLegalLand" runat="server" ImageUrl="~/images/btnHelp.gif"
                            Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label ID="lblCivicAddr" runat="server" Text="Civic Address"></asp:Label></td>
                    <td class="field">
                        <asp:TextBox ID="txtCivicAddr" TextMode="MultiLine" CssClass="txtArea" runat="server"
                            MaxLength="250" TabIndex="6"></asp:TextBox></td>
                    <td valign="top">
                        <asp:ImageButton ID="btnHCivicAddr" runat="server" ImageUrl="~/images/btnHelp.gif"
                            Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label ID="lblPresentUseCode" runat="server" Text="Present Use Code"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:TextBox ID="txtPresentUseCode" runat="server" TabIndex="7"></asp:TextBox>
                        <asp:ImageButton ID="btnHPresentUseCode" runat="server" ImageUrl="~/images/btnHelp.gif"
                            Visible="False" CssClass="btnHelp"></asp:ImageButton>
                        <asp:RangeValidator ID="RangeValidator10" ControlToValidate="txtPresentUseCode" Type="Integer"
                            MinimumValue="0" MaximumValue="2147483647" Text="Only Int type less then 2147483647"
                            runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label ID="lblTaxClass" runat="server" Text="Tax Class"></asp:Label>
                        <span class="requiredField">*</span></td>
                    <td class="field" colspan="2">
                        <asp:DropDownList ID="ddlTaxClass" runat="server" TabIndex="8">
                        </asp:DropDownList>
                        <asp:ImageButton ID="btnHTaxClass" runat="server" ImageUrl="~/images/btnHelp.gif"
                            Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label ID="lblMarketValue" runat="server" Text="Market Value"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:TextBox ID="txtMarketValue" runat="server" TabIndex="9"></asp:TextBox>
                        <asp:ImageButton ID="btnHMarketValue" runat="server" ImageUrl="~/images/btnHelp.gif"
                            Visible="False" CssClass="btnHelp"></asp:ImageButton>
                        <asp:RangeValidator ID="RangeValidator4" ControlToValidate="txtMarketValue" Type="Double"
                            MinimumValue="0" MaximumValue="9999999999" Text="Only Float type" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label ID="lblTaxAssessment" runat="server" Text="Tax Assessment"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:TextBox ID="txtTaxAssessment" runat="server" TabIndex="10"></asp:TextBox>
                        <asp:ImageButton ID="btnHTaxAssessment" runat="server" ImageUrl="~/images/btnHelp.gif"
                            Visible="False" CssClass="btnHelp"></asp:ImageButton>
                        <asp:RangeValidator ID="RangeValidator1" ControlToValidate="txtTaxAssessment" Type="Double"
                            MinimumValue="0" MaximumValue="9999999999" Text="Only Float type" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label ID="lblOtherExempt" runat="server" Text="Other Exemption"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:TextBox ID="txtOtherExempt" runat="server" TabIndex="11"></asp:TextBox>
                        <asp:ImageButton ID="btnHOtherExempt" runat="server" ImageUrl="~/images/btnHelp.gif"
                            Visible="False" CssClass="btnHelp"></asp:ImageButton>
                        <asp:RangeValidator ID="RangeValidator2" ControlToValidate="txtOtherExempt" Type="Double"
                            MinimumValue="0" MaximumValue="9999999999" Text="Only Float type" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label ID="lblFedGIL" runat="server" Text="Federal GIL"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:TextBox ID="txtFedGIL" runat="server" TabIndex="12"></asp:TextBox>
                        <asp:ImageButton ID="btnHFedGIL" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False"
                            CssClass="btnHelp"></asp:ImageButton>
                        <asp:RangeValidator ID="RangeValidator3" ControlToValidate="txtFedGIL" Type="Double"
                            MinimumValue="0" MaximumValue="9999999999" Text="Only Float type" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label ID="lblProvGIL" runat="server" Text="Provincial GIL"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:TextBox ID="txtProvGIL" runat="server" TabIndex="13"></asp:TextBox>
                        <asp:ImageButton ID="btnHProvGIL" runat="server" ImageUrl="~/images/btnHelp.gif"
                            Visible="False" CssClass="btnHelp"></asp:ImageButton>
                        <asp:RangeValidator ID="RangeValidator5" ControlToValidate="txtProvGIL" Type="Double"
                            MinimumValue="0" MaximumValue="9999999999" Text="Only Float type" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label ID="lblSection293" runat="server" Text="Section 293"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:TextBox ID="txtSection293" runat="server" TabIndex="14"></asp:TextBox>
                        <asp:ImageButton ID="btnHSection33" runat="server" ImageUrl="~/images/btnHelp.gif"
                            Visible="False" CssClass="btnHelp"></asp:ImageButton>
                        <asp:RangeValidator ID="RangeValidator6" ControlToValidate="txtSection293" Type="Double"
                            MinimumValue="0" MaximumValue="9999999999" Text="Only Float type" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label ID="lblBylaw" runat="server" Text="Bylaw Exempt"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:TextBox ID="txtBylaw" runat="server" TabIndex="15"></asp:TextBox>
                        <asp:ImageButton ID="btnHBylaw" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False"
                            CssClass="btnHelp"></asp:ImageButton>
                        <asp:RangeValidator ID="RangeValidator7" ControlToValidate="txtBylaw" Type="Double"
                            MinimumValue="0" MaximumValue="9999999999" Text="Only Float type" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
