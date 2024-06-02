<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="tables.aspx.vb" Inherits="PATMAP.tables" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %> 
<%@ Register TagPrefix="patmap" TagName="assmntTabMenu" Src="~/tabmenu.ascx" %>

<asp:Content ID="tabMenu" ContentPlaceHolderID="tabMenu" runat="server">   
    <patmap:assmntTabMenu ID="subMenu" runat="server" />
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server"> 
<script type="text/javascript" language="javascript">
	try {
	
		var prm = Sys.WebForms.PageRequestManager.getInstance();
		prm.add_initializeRequest(InitRequestHandler);
		prm.add_endRequest(EndRequestHandler);
		
		function ShowProgressBar() {
			var elem = document.getElementById('<%= PContainer %>');
			if (elem) { elem.style.display = 'block';};
		}
		function HideProgressBar() {
			var elem = document.getElementById('<%= PContainer %>');
			if (elem) { elem.style.display = 'none';};
		}

		var postbkid = null;
		
		function InitRequestHandler(sender, args) {
			var elemMessage = document.getElementById('<%= PMessage %>');
			postbkid = args.get_postBackElement().id;
			if (postbkid) {
				var numSuffix = IsRegMatch(postbkid);
				if (numSuffix !== null) {
					if (elemMessage) { elemMessage.innerText = "Step " + numSuffix.toString() + " Started"; };
					ShowProgressBar();
				} else if (postbkid.indexOf("lbStepFinal") != -1) {
					if (elemMessage) { elemMessage.innerText = "Step Final Started"; };
					ShowProgressBar();
				}
			}
			
			/*
			if (postbkid) {
				if (postbkid.indexOf("lbStep1") != -1) {
						if (elemMessage) { elemMessage.innerText = "Step 1 Started"; };
						ShowProgressBar();
					} else if (postbkid.indexOf("lbStep2") != -1) {
						if (elemMessage) { elemMessage.innerText = "Step 2 Started"; };
						ShowProgressBar();
					} else if (postbkid.indexOf("lbStep3") != -1) {
						if (elemMessage) { elemMessage.innerText = "Step 3 Started"; };
						ShowProgressBar();
					} else if (postbkid.indexOf("lbStep4") != -1) {
						if (elemMessage) { elemMessage.innerText = "Step 4 Started"; };
						ShowProgressBar();
					} else if (postbkid.indexOf("lbStep5") != -1) {
						if (elemMessage) { elemMessage.innerText = "Step 5 Started"; };
						ShowProgressBar();
					} else if (postbkid.indexOf("lbStepFinal") != -1) {
						if (elemMessage) { elemMessage.innerText = "Step Final Started"; };
						ShowProgressBar();
				}
			}
			*/
		} // end function

		function EndRequestHandler(sender, args) {
			var elemMessage = document.getElementById('<%= PMessage %>');
			
			if (postbkid) {
				var numSuffix = IsRegMatch(postbkid);
				if (numSuffix !== null) {
					if (elemMessage) { elemMessage.innerText = "Step " + numSuffix.toString() + " Finished"; };
				} else if (postbkid.indexOf("lbStepFinal") != -1) {
					if (elemMessage) { elemMessage.innerText = "Step Final Finished"; };
				}
			}

			HideProgressBar();
			
			if (postbkid) {
				if (postbkid.indexOf("lbStep1") != -1) {
					var dataItems = args.get_dataItems()['<%= lbStep1.ClientID %>'];
					if (dataItems != null) {
						//alert(dataItems);
						//	theForm.submit();
						//__doPostBack('ctl00$mainContent$lbStep1', '');
						__doPostBack('<%= lbStep2.UniqueID %>', '');
					}
				} else if (postbkid.indexOf("lbStep2") != -1) {
				var dataItems = args.get_dataItems()['<%= lbStep2.ClientID %>'];
					if (dataItems != null) {
						__doPostBack('<%= lbStep3.UniqueID %>', '');
					}
				} else if (postbkid.indexOf("lbStep3") != -1) {
				var dataItems = args.get_dataItems()['<%= lbStep3.ClientID %>'];
					if (dataItems != null) {
						__doPostBack('<%= lbStep4.UniqueID %>', '');
					}
				} else if (postbkid.indexOf("lbStep4") != -1) {
				var dataItems = args.get_dataItems()['<%= lbStep4.ClientID %>'];
					if (dataItems != null) {
						__doPostBack('<%= lbStep5.UniqueID %>', '');
					}
				} else if (postbkid.indexOf("lbStep5") != -1) {
				var dataItems = args.get_dataItems()['<%= lbStep5.ClientID %>'];
					if (dataItems != null) {
						__doPostBack('<%= lbStepFinal.UniqueID %>', '');
					}
					
				}
			}

		} //end function

		function IsRegMatch(elemID) {
			var re = /(lbStep)(\d+)/;
			var match = re.exec(elemID);
			if (match !== null) {
				if (typeof (match[2]) === "undefined") {
					return null;
				} else {
					return match[2];
				}
			} else {
				return null;
			}
			return null;
		}		
		
	} catch (err) {
		alert(err);
	}
	
</script>
       
<asp:UpdatePanel id="updTables" runat="server" UpdateMode="Conditional" >
<ContentTemplate>
     <asp:LinkButton ID="lbStep1" runat="server" style="display:none;" /> 
     <asp:LinkButton ID="lbStep2" runat="server" style="display:none;" /> 
     <asp:LinkButton ID="lbStep3" runat="server" style="display:none;" /> 
     <asp:LinkButton ID="lbStep4" runat="server" style="display:none;" /> 
     <asp:LinkButton ID="lbStep5" runat="server" style="display:none;" /> 
     <asp:LinkButton ID="lbStepFinal" runat="server" style="display:none;" /> 
 </ContentTemplate>    
</asp:UpdatePanel>    

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
                    <p class="Title">Tables</p>
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
                        
                       <div class="groupBox">
                            <table cellpadding="3" cellspacing="2" border="0">
                                <tr>
                                     <td colspan="3" style="height: 22px"><asp:DropDownList ID="ddlReport" runat="server" AutoPostBack="True"></asp:DropDownList> <asp:ImageButton ID="btnHReport" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton> </td>                                            
                                </tr>
                                <tr>
                                    <td><asp:DropDownList ID="ddlJurisTypeGroup" runat="server" AutoPostBack="True"></asp:DropDownList> <asp:ImageButton ID="btnHJurisTypeGroup" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>                                
                                     <td class="filterDivider"></td>
                                     <td><asp:DropDownList ID="ddlJurisType" runat="server" AutoPostBack="True"></asp:DropDownList> <asp:ImageButton ID="btnHJurisType" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>                                
                                </tr>
                                <tr>
                                     <td><asp:DropDownList ID="ddlMunicipality" runat="server" AutoPostBack="True"></asp:DropDownList> <asp:ImageButton ID="btnHMunicipality" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                     <td class="filterDivider"></td>
                                     <td><asp:DropDownList ID="ddlSchoolDivision" runat="server" AutoPostBack="True"></asp:DropDownList> <asp:ImageButton ID="btnHSchoolDivision" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:ImageButton ID="btnTaxStaus" runat="server" ImageUrl="~/images/btnTaxStatus.gif"></asp:ImageButton> 
                                        <asp:ImageButton ID="btnHTaxStaus" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton>
                                    </td>
                                    <td class="filterDivider"></td>
                                    <td>
                                        <asp:DropDownList ID="ddlTaxStatus" runat="server" AutoPostBack="True"></asp:DropDownList>
                                        <asp:ImageButton ID="btnHTaxStatus" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:ImageButton ID="btnClasses" runat="server" ImageUrl="~/images/btnClasses.gif"></asp:ImageButton> 
                                        <asp:ImageButton ID="btnHClasses" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton>
                                    </td>
                                    <td class="filterDivider"></td>
                                    <td>
                                        <asp:DropDownList ID="ddlTaxClasses" runat="server" AutoPostBack="True"></asp:DropDownList> <asp:ImageButton ID="btnHTaxClasses" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton> 
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlTaxType" runat="server" AutoPostBack="True">
                                            <asp:ListItem Value="0">--Tax Type--</asp:ListItem>
                                            <asp:ListItem Value="1">Municipal Tax</asp:ListItem>
                                            <asp:ListItem Value="2">School Tax</asp:ListItem>
                                        </asp:DropDownList> 
                                        <!--<asp:ListItem Value="3">Grant Tax</asp:ListItem> Inky Removed from dropdown Apr-2010 as per PATMAP restructuring project-->
                                        <asp:ImageButton ID="btnHTaxType" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton>
                                    </td>                                
                                    <td class="filterDivider"></td>
                                    <td>
                                        <asp:TextBox ID="txtParcelNo" runat="server" Text="-- Enter Parcel ID --" AutoPostBack="True"></asp:TextBox> <asp:ImageButton ID="btnHParcelNo" runat="server" ImageUrl="~/images/btnHelp.gif" Visible="False" CssClass="btnHelp"></asp:ImageButton>
                                    </td>                                            
                                </tr>
                            </table>                                
                        </div>
                                                  
                    </td>
                </tr> 
                 <tr>
                    <%--<td colspan="4" align="right" class="btnsBottom"><asp:ImageButton ID="btnShow" runat="server" ImageUrl="~/images/btnShow.gif" OnClientClick="alert(win); if (win) { win.close(); } win=openWindow('viewReport.aspx','toolbar=0,resizable=1,scrollbars=1'); return false;"></asp:ImageButton></td>--%>
                    <td colspan="4" align="right" class="btnsBottom"><asp:ImageButton ID="btnShow" runat="server" ImageUrl="~/images/btnShow.gif" OnClientClick="openReportWindow('viewReport.aspx','toolbar=0,resizable=1,scrollbars=1'); return false;"></asp:ImageButton></td>
                </tr>               
           </table>
            <%--<rsweb:ReportViewer ID="rpvReports" runat="server" ShowParameterPrompts="False" Height="550px" Width="650px"></rsweb:ReportViewer>         --%>
        </div> 
    </div>   
    
</asp:Content>

