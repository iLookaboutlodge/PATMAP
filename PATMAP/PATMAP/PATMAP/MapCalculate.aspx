<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="MapCalculate.aspx.vb" Inherits="PATMAP.MapCalculate"  %>
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

</asp:Content>
