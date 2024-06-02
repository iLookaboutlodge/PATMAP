<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" ValidateRequest="false" Codebehind="loadassessment.aspx.vb" Inherits="PATMAP.loadassessment" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register TagPrefix="patmap" TagName="dataTabMenu" Src="~/tabmenu.ascx" %>

<asp:Content ID="tabMenu" ContentPlaceHolderID="tabMenu" runat="server">
    <patmap:dataTabMenu ID="subMenu" runat="server"></patmap:dataTabMenu>
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server">
    <script language="javascript" type="text/javascript" src="../jquery-3.4.1.min.js"></script>

    <script language="javascript" type="text/javascript">
        var counter;

//        function UploadFileAjax() {
//            var files = $("#<%=fpFile.ClientID%>").get(0).files;
//            counter = 0;

//            for (var i = 0; i < files.length ; i++) {
//                var file = files[i];
//                var formdata = new FormData();
//                formdata.append("file1", file);
//                var ajax = new XMLHttpRequest();

//                ajax.upload.addEventListener("progress", progressHandler, false);
//                ajax.addEventListener("load", completeHandler, false);
//               ajax.addEventListener("error", errorHandler, false);
//                ajax.addEventListener("abort", abortHandler, false);
//                ajax.open("POST", "../fileuploadhandler.ashx");
//                ajax.send(formdata);
//            }
//        }


        $(document).ready(function () {
            $("[id$=btnFileUpload_New]").on("click", event, UploadFileClick);
        });

        function UploadFileClick(e) {
            e.preventDefault();

            //alert("Handler for .click() called.");

            var files = $("#<%=fpFile.ClientID%>").get(0).files;
            counter = 0;

            for (var i = 0; i < files.length ; i++) {
                var file = files[i];
                var formdata = new FormData();
                formdata.append("file1", file);

                var ajax = new XMLHttpRequest();

                ajax.upload.addEventListener("progress", progressHandler, false);
                ajax.addEventListener("load", completeHandler, false);
                ajax.addEventListener("error", errorHandler, false);
                ajax.addEventListener("abort", abortHandler, false);
                ajax.open("POST", "../fileuploadhandler.ashx");
                ajax.send(formdata);

            }

        }


        function progressHandler(event) {
            $("#loaded_n_total").html("Uploaded " + event.loaded + " bytes of " + event.total);
            var percent = (event.loaded / event.total) * 100;
            $("#progressBar").val(Math.round(percent));
            $("#status").html(Math.round(percent) + "% uploaded... please wait");
        }

        function completeHandler(event) {
            counter++
            if (event.target.status === 200) {
                $("#status").html(counter + " " + event.target.responseText);

                var elem = document.getElementById("<%=fpFile.ClientID%>");
                if (elem) { elem.value = ''; };
                $("#<%=fpFile.ClientID%>").val('');

                __doPostBack('<%=LinkButton1.UniqueID%>', '');

                //$("[id$=LinkButton1]").click();
            } else {
                $("#status").html(event.target.responseText);
            }

        }

        function errorHandler(event) {
            $("#status").html("Upload Failed");
        }

        function abortHandler(event) {
            $("#status").html("Upload Aborted");
        }
    </script>

    <div class="commonContent">
        <div class="commonHeader">
            <div class="PageHeaderModule">
                <div class="Header">
                    <p class="Title">
                        Assessment Data</p>
                </div>
            </div>
        </div>
        <div class="commonForm">

            <%--<p>
                All fields are required.</p>--%>
            <table cellpadding="0" cellspacing="0" border="0">
                <tr id="FlashfileUpload" runat="server">
                    <td class="label">
                        <asp:Label ID="lblFile" runat="server" Text="File"></asp:Label>
                    </td>
                    <td class="field" style="width: 224px">
                        <asp:FileUpload ID="fpFile" runat="server" Width="200px" AllowMultiple="false" />
                            <asp:ImageButton ID="btnHFile" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp">
                        </asp:ImageButton>
                    </td>
                    <td class="field">
                        <asp:ImageButton ID="btnFileUpload_New" runat="server" ImageUrl="~/images/btnLoad.gif" />
                    </td>
                </tr>
                <tr id="FlashfileUpload_p" runat="server">
                    <td colspan="3">
                        <%--<asp:Label ID="lb_Message" runat="server" Width="100%" ForeColor="#669966"></asp:Label>--%>
                        <asp:LinkButton ID="LinkButton1" runat="server" onclick="LinkButton1_Click"></asp:LinkButton>
                        <div >
                            <progress id="progressBar" value="0" max="100" style="width:100%;"></progress>
                            <h3 id="status"></h3>
                            <p id="loaded_n_total"></p>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Label ID="Label2" runat="server" Text=""></asp:Label>
                    </td>
                </tr>

<%--                <tr id="FlashfileUpload" runat="server">
                    <td class="label">
                        <asp:Label ID="lblFile" runat="server" Text="File"></asp:Label>
                    </td>

                    <td class="field" style="width: 224px">
                        <asp:FileUpload ID="fpFile" runat="server" Width="200px" /><asp:ImageButton ID="btnHFile"
                            runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp">
                        </asp:ImageButton></td>
                    <td class="field">
                        <asp:ImageButton ID="btnFileUpload" runat="server" ImageUrl="~/images/btnLoad.gif"
                            OnClick="btnFileUpload_Click" />
                    </td>
                </tr>--%>

<%--                <tr id="FlashfileUpload" runat="server">
                    <td class="field" style="width: 224px">
                        <asp:FileUpload ID="fpFile" runat="server" Width="200px" /><asp:ImageButton ID="btnHFile"
                            runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp">
                        </asp:ImageButton>
                        </td>
                    <td class="field">
                        <asp:ImageButton ID="btnFileUpload" runat="server" ImageUrl="~/images/btnLoad.gif"
                            OnClick="btnFileUpload_Click" />
                    </td>

                    <td colspan="2">
                        <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="https://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,29,0"
                            width="550" height="100" id="fileUpload" align="middle">
                            <param name="allowScriptAccess" value="sameDomain" />
                            <param name="movie" value="fileUpload.swf" />
                            <param name="quality" value="high" />
                            <param name="wmode" value="transparent" />
                            <param name="FlashVars" value='uploadPage=Upload.axd<%=GetFlashVars()%>&completeFunction=UploadComplete()' />
                        </object>
                    </td>

                </tr>--%>



                <tr id="tr_ddlTableNames" runat="server" visible="false">
                    <td class="label">
                        <asp:Label ID="Label1" runat="server" Text="Select Table Name"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:DropDownList ID="ddlTableNames" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTableNames_SelectedIndexChanged" />
                    </td>
                </tr>
                <tr id="tr_ddlISCParcelNumber" runat="server" visible="false">
                    <td class="label">
                        <asp:Label ID="lb_ddlISCParcelNumber" runat="server" Text="Field: ISCParcelNumber"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:DropDownList ID="ddlISCParcelNumber" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTableColumns_SelectedIndexChanged" />
                    </td>
                </tr>
                <tr id="tr_ddlparcelID" runat="server" visible="false">
                    <td class="label">
                        <asp:Label ID="lb_ddlparcelID" runat="server" Text="Field: parcelID"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:DropDownList ID="ddlparcelID" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTableColumns_SelectedIndexChanged" />
                    </td>
                </tr>
                <tr id="tr_ddlmunicipalityID" runat="server" visible="false">
                    <td class="label">
                        <asp:Label ID="lb_ddlmunicipalityID" runat="server" Text="Field: municipalityID"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:DropDownList ID="ddlmunicipalityID" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTableColumns_SelectedIndexChanged" />
                    </td>
                </tr>
                <tr id="tr_ddlalternate_parcelID" runat="server" visible="false">
                    <td class="label">
                        <asp:Label ID="lb_ddlalternate_parcelID" runat="server" Text="Field: alternate_parcelID"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:DropDownList ID="ddlalternate_parcelID" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTableColumns_SelectedIndexChanged" />
                    </td>
                </tr>
                <tr id="tr_ddlLLD" runat="server" visible="false">
                    <td class="label">
                        <asp:Label ID="lb_ddlLLD" runat="server" Text="Field: LLD"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:DropDownList ID="ddlLLD" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTableColumns_SelectedIndexChanged" />
                    </td>
                </tr>
                <tr id="tr_ddlcivicAddress" runat="server" visible="false">
                    <td class="label">
                        <asp:Label ID="lb_ddlcivicAddress" runat="server" Text="Field: civicAddress"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:DropDownList ID="ddlcivicAddress" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTableColumns_SelectedIndexChanged" />
                    </td>
                </tr>
                <tr id="tr_ddlpresentUseCodeID" runat="server" visible="false">
                    <td class="label">
                        <asp:Label ID="lb_ddlpresentUseCodeID" runat="server" Text="Field: presentUseCodeID"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:DropDownList ID="ddlpresentUseCodeID" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTableColumns_SelectedIndexChanged" />
                    </td>
                </tr>
                <tr id="tr_ddlschoolID" runat="server" visible="false">
                    <td class="label">
                        <asp:Label ID="lb_ddlschoolID" runat="server" Text="Field: schoolID"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:DropDownList ID="ddlschoolID" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTableColumns_SelectedIndexChanged" />
                    </td>
                </tr>
                <tr id="tr_ddltaxClassID_orig" runat="server" visible="false">
                    <td class="label">
                        <asp:Label ID="lb_ddltaxClassID_orig" runat="server" Text="Field: taxClassID_orig"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:DropDownList ID="ddltaxClassID_orig" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTableColumns_SelectedIndexChanged" />
                    </td>
                </tr>
                <tr id="tr_ddlmarketValue" runat="server" visible="false">
                    <td class="label">
                        <asp:Label ID="lb_ddlmarketValue" runat="server" Text="Field: marketValue"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:DropDownList ID="ddlmarketValue" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTableColumns_SelectedIndexChanged" />
                    </td>
                </tr>
                <tr id="tr_ddltaxable" runat="server" visible="false">
                    <td class="label">
                        <asp:Label ID="lb_ddltaxable" runat="server" Text="Field: taxable"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:DropDownList ID="ddltaxable" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTableColumns_SelectedIndexChanged" />
                    </td>
                </tr>
                <tr id="tr_ddlotherExempt" runat="server" visible="false">
                    <td class="label">
                        <asp:Label ID="lb_ddlotherExempt" runat="server" Text="Field: otherExempt"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:DropDownList ID="ddlotherExempt" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTableColumns_SelectedIndexChanged" />
                    </td>
                </tr>
                <tr id="tr_ddlFGIL" runat="server" visible="false">
                    <td class="label">
                        <asp:Label ID="lb_ddlFGIL" runat="server" Text="Field: FGIL"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:DropDownList ID="ddlFGIL" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTableColumns_SelectedIndexChanged" />
                    </td>
                </tr>
                <tr id="tr_ddlPGIL" runat="server" visible="false">
                    <td class="label">
                        <asp:Label ID="lb_ddlPGIL" runat="server" Text="Field: PGIL"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:DropDownList ID="ddlPGIL" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTableColumns_SelectedIndexChanged" />
                    </td>
                </tr>
                <tr id="tr_ddlSection293" runat="server" visible="false">
                    <td class="label">
                        <asp:Label ID="lb_ddlSection293" runat="server" Text="Field: Section293"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:DropDownList ID="ddlSection293" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTableColumns_SelectedIndexChanged" />
                    </td>
                </tr>
                <tr id="tr_ddlByLawExemption" runat="server" visible="false">
                    <td class="label">
                        <asp:Label ID="lb_ddlByLawExemption" runat="server" Text="Field: ByLawExemption"></asp:Label></td>
                    <td class="field" colspan="2">
                        <asp:DropDownList ID="ddlByLawExemption" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTableColumns_SelectedIndexChanged" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr id="tr_Aggregate" runat="server" visible="false">
                    <td>
                    </td>
                    <td class="field" colspan="2">
                        <asp:CheckBox ID="ckbAggregate" runat="server" Text="Aggregate Data Only" />
                        <asp:ImageButton ID="btnHAggregate" runat="server" ImageUrl="~/images/btnHelp.gif"
                            Visible="False" CssClass="btnHelp"></asp:ImageButton>
                    </td>
                </tr>
                <tr id="tr_DataSet" runat="server" visible="false">
                    <td class="label">
                        <asp:Label ID="lblDataSet" runat="server" Text="Data Set Name"></asp:Label></td>
                    <td class="field" colspan="2">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td>
                                    <asp:Label ID="lblExisting" runat="server" Text="Existing"></asp:Label></td>
                                <td>
                                    <asp:DropDownList ID="ddlDSN" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlDSN_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:ImageButton ID="btnHDSN" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False"
                                        CssClass="btnHelp"></asp:ImageButton>
                                </td>
                            </tr>
                            <tr id="tr_txtNewDSN" runat="server">
                                <td align="center">
                                    <asp:Label ID="lblOr" runat="server" Text="Or New"></asp:Label></td>
                                <td>
                                    <asp:TextBox ID="txtNewDSN" runat="server" CssClass="txtLong"
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
                <tr id="tr_btnLoad_SSIS" runat="server" visible="false">
                    <td>
                    </td>
                    <td class="btns" style="height: 25px" colspan="2">
                        <asp:ImageButton ID="btnLoad_SSIS" runat="server" ImageUrl="~/images/btnLoad.gif"
                            OnClick="btnLoad_Click_SSIS" />
                    </td>
                </tr>
                <tr id="tr_btnLoad_SQL" runat="server" visible="false">
                    <td>
                    </td>
                    <td class="btns" style="height: 25px" colspan="2">
                        <asp:ImageButton ID="btnLoad_SQL" runat="server" ImageUrl="~/images/btnLoad.gif" OnClientClick="return confirmPrompt('Do you want to load assessment data?');"/>
                            <%--OnClick="btnLoad_SQL_Click" />--%>
                    </td>
                </tr>
            </table>
            <rsweb:ReportViewer ID="rpvReports" runat="server">
            </rsweb:ReportViewer>
        </div>
    </div>
</asp:Content>
