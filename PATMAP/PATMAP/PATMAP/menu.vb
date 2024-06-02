Public Class menu

    'Sets starting nodes for each section
    Public Shared loadData As String = "~/datamgnt/loadassessment.aspx?id=def"
    Public Shared editData As String = "~/datamgnt/viewassessment.aspx?id=def"
    Public Shared userGroup As String = "~/sysadmin/viewusers.aspx?id=def"
    Public Shared taxYearModel As String = "~/datamgnt/viewtaxyearmodel.aspx?id=def"
    Public Shared boundaryModel As String = "~/datamgnt/viewboundarymodel.aspx?id=def"
    Public Shared helpText As String = "~/sysadmin/viewhelpscreen.aspx?id=def"
    Public Shared calcFunction As String = "~/sysadmin/viewfunctions.aspx?id=def"
    Public Shared assmnt As String = "~/assmnt/start.aspx?id=def"
    Public Shared boundary As String = "~/boundary/main.aspx?id=def"
    Public Shared taxtools As String = "~/taxtools/main.aspx?id=def"

    'to handle Code Data menu item
    Public Shared codeDataFound As Boolean
    Public Shared codeDataURL As String = ""


    'loadMenu() Method
    'Accepts menu web control and page object as parameters
    'Dynamically loads menu items from the Web.sitemap
    'Sets CssClass for selected menu item
    Public Shared Sub loadMenu(ByRef mainMenu As System.Web.UI.WebControls.Menu, ByRef Session As System.Web.SessionState.HttpSessionState)
        Dim selectedMenu As Boolean = False
        Dim SMN As SiteMapNode
        Dim selectedItem As SiteMapNode
        Dim currentNode As SiteMapNode
        Dim dt As DataTable
        Dim dr As DataRow()
        Dim addMenu As Boolean
        Dim addSubMenu As Boolean
        Dim levelID As Integer

        If IsNumeric(Session("levelID")) Then
            levelID = Session("levelID")
        Else
            HttpContext.Current.Response.Redirect("~/index.aspx")
        End If

        'Gets access permission for currently logged in user
        dt = common.GetPermission(levelID)

        currentNode = SiteMap.CurrentNode

        'Clears menu items
        selectedItem = Nothing
        mainMenu.Items.Clear()

        If Not IsNothing(dt) Then

            'Iterates through the items in Web.sitemap
            For Each SMN In SiteMap.RootNode.ChildNodes
                Dim newMenu As New MenuItem()

                'Adds a new menu item to the main navigation if the link is not to 
                'to the Disclaimer or Privacy page
                If SMN.Title <> "Disclaimer" And SMN.Title <> "Privacy" And SMN.Title <> "Error" Then

                    'Doesn't add menu if there's no subpages or user doesn't have permission
                    addMenu = False
                    addSubMenu = False

                    dr = dt.Select("screenName = '" & SMN.Title & "'")

                    'If user have access to current SiteMapNode 
                    If dr.Length > 0 Then

                        'Assigns menu item's text and url
                        newMenu.Text = SMN.Title
                        newMenu.NavigateUrl = Replace(SMN.Url, "?id=def", "")

                        'Adds flyout submenu items to the Data Management 
                        'and System Admin menu items only
                        'Check if user also has permission the each flyout menu item
                        If SMN.Title = "Data Management" Or SMN.Title = "System Admin" Then
                            addSubMenu = True
                        End If

                        If SMN.Title = "Help" Then
                            Session.Add("currentNode", currentNode.Key)
                            newMenu.Target = "_blank"
                        End If

                        If SMN.Title <> "Home" And SMN.Title <> "Help" And SMN.Title <> "Logout" Then

                            Dim url As String = ""

                            addMenu = checkChildMenu(SMN, newMenu, dt, True, addSubMenu, url)

                            If addSubMenu And newMenu.ChildItems.Count > 0 Then
                                addMenu = True
                            End If

                            If addMenu And InStr(newMenu.NavigateUrl, "?id=def") And url <> "" And newMenu.ChildItems.Count = 0 Then
                                newMenu.NavigateUrl = url
                            End If

                        Else
                            addMenu = True
                        End If

                        'Checks to see if the current page's URL matches 
                        'current item's URL link or the page is one of the subsections.
                        'If both URL matches, sets menu item as selected
                        If Not selectedMenu And Not IsNothing(currentNode.Url) Then

                            If SMN.Url <> "" And InStr(currentNode.Url, SMN.Url) Then
                                selectedMenu = True
                            Else
                                selectedMenu = currentNode.IsDescendantOf(SMN)
                            End If

                            'Sets CssClass style to the selected root menu
                            If selectedMenu Then
                                selectedItem = SMN
                                newMenu.Selected = True
                            End If

                        End If

                        If addMenu Then
                            mainMenu.Items.Add(newMenu)
                        End If

                    End If

                End If
            Next


            '********* INKY'S Local Tax Tools Code *********
            'Check if the selected menu item is part of the LTT module
            Dim pageURL As String = currentNode.Url.ToString
            Dim LTTmod As Boolean = False

            HttpContext.Current.Session.Add("useLiveLTTtable", LTTmod)

            'if user outside of the LTT module check for LTTtable and delete if found.
            If (InStr(pageURL, "taxtools") = 0) Then

                'delete liveLTTtable and removes session variables if it exsists once user exits the LTT module
                If HttpContext.Current.Session("liveLTTtableCreated") Then
                    common.deleteLiveLTTtable()
                    HttpContext.Current.Session.Remove("boundarySelection") 'boudarySelection located in boundary/main RowCommand sub
                    HttpContext.Current.Session.Remove("LTTdropDownChoice")
                    HttpContext.Current.Session.Remove("LTTfreeReign")
                    HttpContext.Current.Session.Remove("LTTsubjYear")
                    HttpContext.Current.Session.Remove("LTTSubjectMunicipality")
                End If
                '******* End of Inky's LTT Code ***************
            End If
        End If

    End Sub

    'checkChildMenu()
    'Accepts SiteMapNode, MenuItem, DataTable, Two Boolean (Optional) and String (Optional) as parameters
    'Returns a boolean; True if menu should be added to the list and False if not
    'Iterates through Child SiteMapNode to look for at least one that the user can access
    'If there's no accessible Child SiteMapNode, menu is hidden
    'Sets root menu's navigate url to the first accessible child menu 
    Public Shared Function checkChildMenu(ByVal SMN As SiteMapNode, ByRef newMenu As MenuItem, ByVal dt As DataTable, Optional ByVal rootLevel As Boolean = False, Optional ByVal addSubMenu As Boolean = False, Optional ByRef url As String = "") As Boolean
        Dim child As SiteMapNode
        Dim dr As DataRow()
        Dim addMenu As Boolean
        Dim currentURL As String
        Dim folders As String()


        'Iterates through submenus
        For Each child In SMN.ChildNodes
            currentURL = Replace(child.Url, "?id=def", "")

            folders = currentURL.Split("/")

            If folders.Length > 0 Then
                currentURL = folders(folders.Length - 2) & "/" & folders(folders.Length - 1)
            End If

            dr = dt.Select("pageAddress like '%" & currentURL & "%'")

            'If a flyout menu needs to be created
            If addSubMenu And url = "" Then
                'Searches for the first accessible submenu to set root menu's default url
                addMenu = checkChildMenu(child, newMenu, dt, False, addSubMenu, url)
            End If

            If dr.Length > 0 Then
                addMenu = True
            End If

            'setup database connection
            Dim con As New SqlClient.SqlConnection
            con.ConnectionString = PATMAP.Global_asax.connString
            con.Open()

            Dim query As New SqlClient.SqlCommand
            Dim dataReader As SqlClient.SqlDataReader
            query.Connection = con

            query.CommandText = "SELECT screenNameID, access FROM levelsPermission WHERE levelID = " & HttpContext.Current.Session("levelID") & "  AND screenNameID IN (55, 56, 58, 61, 63, 77) AND access <> 0 order by screenNameID"
            dataReader = query.ExecuteReader



            If Not (dataReader.HasRows) Then
                If child.Title = "Edit Data" Then
                    'url = codeDataURL
                    addMenu = False
                End If
            End If
            dataReader.Close()
            con.Close()


            'Sets root menu's url equavalent to the first accessible submenu's url as a default
            If addSubMenu And addMenu And url <> "" Then

                Dim newSubMenu As New MenuItem()

                'Assigns menu item's text and url  
                newSubMenu.Text = child.Title
                If child.Title = "Edit Data" Then

                    If codeDataFound = False Then
                        '*****Inky's Code: assign "Edit Data" startpage URL dynamically...
                        'setup database connection
                        ''Dim con As New SqlClient.SqlConnection
                        con.ConnectionString = PATMAP.Global_asax.connString
                        con.Open()

                        ''Dim query As New SqlClient.SqlCommand
                        ''Dim dataReader As SqlClient.SqlDataReader
                        query.Connection = con

                        'query.CommandText = "SELECT screenNameID, access FROM levelsPermission WHERE levelID = " & HttpContext.Current.Session("levelID") & "  AND screenNameID IN (55, 56, 58, 61, 63, 77, 25, 8, 31, 32) order by screenNameID"
                        query.CommandText = "SELECT screenNameID, access FROM levelsPermission WHERE levelID = " & HttpContext.Current.Session("levelID") & "  AND screenNameID IN (55, 56, 58, 61, 63, 77, 127) order by screenNameID"
                        dataReader = query.ExecuteReader

                        Dim screenID As Integer = 0
                        Dim editDataAccess As Boolean = False

                        'Assign URL based on first screen with access based on level-permission
                        If dataReader.HasRows Then
                            While dataReader.Read()
                                screenID = dataReader.GetValue(0)
                                editDataAccess = dataReader.GetValue(1)

                                If (screenID = 55) And (editDataAccess = True) Then
                                    newSubMenu.NavigateUrl = "~/datamgnt/viewassessment.aspx"
                                    Exit While

                                ElseIf (screenID = 56) And (editDataAccess = True) Then
                                    newSubMenu.NavigateUrl = "~/datamgnt/viewtaxcredit.aspx"
                                    Exit While

                                ElseIf (screenID = 58) And (editDataAccess = True) Then
                                    newSubMenu.NavigateUrl = "~/datamgnt/viewmillrate.aspx"
                                    Exit While

                                ElseIf (screenID = 61) And (editDataAccess = True) Then
                                    newSubMenu.NavigateUrl = "~/datamgnt/viewpov.aspx"
                                    Exit While

                                ElseIf (screenID = 63) And (editDataAccess = True) Then
                                    newSubMenu.NavigateUrl = "~/datamgnt/viewpotash.aspx"
                                    Exit While

                                ElseIf (screenID = 77) And (editDataAccess = True) Then
                                    newSubMenu.NavigateUrl = "~/datamgnt/viewkog.aspx"
                                    Exit While
                                    'Else

                                    '    If (screenID = 8) And (editDataAccess = True) Then
                                    '        newSubMenu.NavigateUrl = "~/datamgnt/viewtaxclass.aspx"
                                    '        Exit While

                                    '    ElseIf (screenID = 25) And (editDataAccess = True) Then
                                    '        newSubMenu.NavigateUrl = "~/datamgnt/viewtaxentity.aspx"
                                    '        Exit While

                                    '    ElseIf (screenID = 31) And (editDataAccess = True) Then
                                    '        newSubMenu.NavigateUrl = "~/datamgnt/viewjuristype.aspx"
                                    '        Exit While

                                    '    ElseIf (screenID = 32) And (editDataAccess = True) Then
                                    '        newSubMenu.NavigateUrl = "~/datamgnt/viewpropcode.aspx"
                                    '        Exit While
                                    '    End If

                                End If
                            End While
                        End If
                        dataReader.Close()
                        con.Close()

                        '**** End of Inky's code
                    Else
                        newSubMenu.NavigateUrl = codeDataURL
                    End If

                Else
                    newSubMenu.NavigateUrl = url
                End If
                newMenu.ChildItems.Add(newSubMenu)

                url = ""

            ElseIf addMenu And addSubMenu And rootLevel Then
                'If user has permission to previously set default page

                Dim newSubMenu As New MenuItem()

                'Assigns menu item's text and url  
                newSubMenu.Text = child.Title
                newSubMenu.NavigateUrl = Replace(child.Url, "?id=def", "")
                newMenu.ChildItems.Add(newSubMenu)

            ElseIf addSubMenu And addMenu And url = "" Then
                'Returns submenu's url 

                url = Replace(child.Url, "?id=def", "")
                Return addMenu

            ElseIf addMenu Then

                url = child.Url
                Exit For

            End If

        Next

        Return addMenu

    End Function

    'loadSubMeu() Method
    'Accepts menu web control, sitemapnode and two image as parameters
    'Dynamically loads menu items from the sitemapnode
	Public Shared Sub loadSubMenu(ByRef subMenu As System.Web.UI.WebControls.Menu, ByRef node As SiteMapNode, ByRef leftImage As System.Web.UI.WebControls.Image, ByRef rightImage As System.Web.UI.WebControls.Image, ByVal levelID As Integer)
		Dim SMN As SiteMapNode
		Dim currentNode As SiteMapNode
		Dim selectedMenu As Boolean
		Dim selectedItem As New MenuItem
		Dim menuEnabled As Boolean
		Dim dt As DataTable
		Dim dr As DataRow()
		Dim addMenu As Boolean
		Dim counter As Integer = 0
		Dim currentURL As String
		Dim LTTStartTaxDone As Boolean
		Dim LTTBaseYearDone As Boolean

		'Gets access permission for currently logged in user
		dt = common.GetPermission(levelID)

		currentNode = SiteMap.CurrentNode

		'Clears menu items
		subMenu.MaximumDynamicDisplayLevels = 1
		subMenu.Items.Clear()

		menuEnabled = True

		'Iterates through the childnodes
		For Each SMN In node.ChildNodes
			Dim newMenu As New MenuItem()

			addMenu = True

			currentURL = Replace(SMN.Url, "?id=def", "")

			Dim OrigURL As String = currentURL

			'added for tweaking security for MapCalculate.aspx page same like Map.aspx security 17-jul-2014
			If currentURL.IndexOf("MapCalculate") > -1 Then
				currentURL = Replace(currentURL, "MapCalculate", "Map")
			End If

			dr = dt.Select("pageAddress like '%" & currentURL & "%'")	'Inky's code

			'replace original url back
			currentURL = OrigURL

			'If currentURL.LastIndexOf("/") > -1 Then
			'    currentURL = currentURL.Substring(currentURL.LastIndexOf("/"))
			'End If

			'dr = dt.Select("pageAddress like '%" & currentURL & "%'")


			If dr.Length > 0 Or SMN.Title = "Code Data" Then

				If currentURL.LastIndexOf("/") > -1 Then 'Inky's code
					currentURL = currentURL.Substring(currentURL.LastIndexOf("/")) 'Inky's code
				End If 'Inky's code


				'Assigns menu item's text and url
				newMenu.Text = SMN.Title
				newMenu.NavigateUrl = SMN.Url
				newMenu.Enabled = menuEnabled

				'If current page is the Assessment Tax Year Model - Start page, disable other menu items
				If currentNode.Title = "Start" Then
					menuEnabled = False
				End If

				'************* Local Tax Tools (LTT) Menu controls *********
				If IsNothing(HttpContext.Current.Session("LTTfreeReign")) Then

					'If current page is Local Tax Tools "Main" page, disable other LTT menu items.
					If currentNode.Description = "Local Tax Tools Main" Then
						menuEnabled = False
					End If

					'If current page is LTT "Base Year," keep Main enabled, disable all other LTT menu tabs.
					If LTTBaseYearDone Then
						menuEnabled = False
					End If

					'If current page is LTT "Start Tax," keep Main & Base Year pages enabled, 
					'disable all other LTT menu tabs.
					If LTTStartTaxDone Then
						menuEnabled = False
					End If
				End If
				'******* End of Local Tax Tools (LTT) Menu controls *********


				'Boundary Change Menu Controls
				If currentNode.ParentNode.Title = "Boundary Changes" Then
					If IsNothing(HttpContext.Current.Session("CODE_DestinationMunicipality")) Then
						If SMN.Title = "Map" Then
							newMenu.Enabled = False
							'menuEnabled = False
						End If
					End If
				End If

				'Adds flyout menu items to Code Data menu item only
				If SMN.Title = "Code Data" Then

					'Doesn't add menu if there's no accessible submenu
					addMenu = False

					addMenu = checkChildMenu(SMN, newMenu, dt, True, True)

					If addMenu Then
						newMenu.NavigateUrl = newMenu.ChildItems.Item(0).NavigateUrl
					End If

				End If

				'Checks to see if the current page's URL matches 
				'current item's URL link or the page is one of the subsections.
				'If both URL matches, sets menu item as selected
				If Not selectedMenu And Not IsNothing(currentNode.Url) Then

					If SMN.Url <> "" And InStr(currentNode.Url, SMN.Url) Then
						selectedMenu = True
					Else
						If SMN.Description = currentNode.ParentNode.Title Then
							selectedMenu = True
						ElseIf currentNode.IsDescendantOf(SMN) Then
							If SMN.Description = currentNode.ParentNode.Title Then
								selectedMenu = True
							ElseIf SMN.Description = currentNode.ParentNode.ParentNode.Title Then
								selectedMenu = True
							End If
						End If
					End If

					'Sets CssClass style to the selected root menu
					If selectedMenu Then
						newMenu.Selected = True
						selectedItem = newMenu
					End If

				End If

				If addMenu Then

					'Sets menu image separator to each item except the last one
					If counter > 0 Then
                        subMenu.Items(counter - 1).SeparatorImageUrl = "~/images/subNavDivider.gif"
                        'subMenu.Items(counter - 1).SeparatorImageUrl = "../images/subNavDivider.gif"
                    End If

					subMenu.Items.Add(newMenu)
					counter += 1
				End If

			End If


			'************* Local Tax Tools (LTT) Menu controls Code ************ 
			If IsNothing(HttpContext.Current.Session("LTTfreeReign")) And currentNode.ParentNode.Title = "Local Tax Tools" Then
				'check to see if BaseYear has been clicked and the Main menu styles have already been applied
				If Not LTTBaseYearDone Then
					If currentNode.Title = "Base Year" And newMenu.Text = "Main" Then
						'set values for users not entering from Boundary WITH access to Base Year/PhaseIn screens
						menuEnabled = True
						LTTBaseYearDone = True

						'ElseIf currentNode.Title <> "Main" And Not IsNothing(HttpContext.Current.Session("BoundarySelection")) And newMenu.Text = "Main" Then
						'    'sets values for users entertering from Boundary (access to Base Year/PhaseIn screens will be denied by default)
						'    LTTBaseYearDone = True

					ElseIf InStr("Start Tax Base Tax Min Tax MR Factors Phase-In Tables Graphs MAP", currentNode.Title) <> 0 Then	' currentNode.Title = "Start Tax" Then
						'for users not entering LTT from Boundary, but who's user-level permissions deny access to Base Year/Phase-In screens
						menuEnabled = True
						LTTBaseYearDone = True
					End If
				End If

				If Not LTTStartTaxDone Then
					If currentNode.Title = "Start Tax" And newMenu.Text = "Base Year" Then
						menuEnabled = True
						HttpContext.Current.Session.Add("LTTStartTaxDone", True)

					ElseIf currentNode.Title = "Start Tax" And HttpContext.Current.Session("phaseInBaseYearAccess") = False Then 'InStr("Base Tax Min Tax MR Factors Phase-In Tables Graphs MAP", currentNode.Title) <> 0 And newMenu.Text = "Main" Then '<> 0 And HttpContext.Current.Session("LTTBaseYearDone") Then ' <> "Start Tax" And currentNode.NextSibling.Title <> "Start Tax")  Then 'currentNode.Title <> "Main" And Not IsNothing(HttpContext.Current.Session("BoundarySelection")) And newMenu.Text = "Main" Then
						menuEnabled = True
						LTTStartTaxDone = True
					End If
				End If

				If InStr("Base Tax Min Tax MR Factors Phase-In Tables Graphs MAP", currentNode.Title) <> 0 Then	'currentNode.Title = "Base Tax" Then
					'sets session variable to enable the user to click any LTT menu tab
					'they'd like once they've completed ONE initial set up of the table values
					HttpContext.Current.Session.Add("LTTfreeReign", True)
				End If
			End If
			'************ End of Local Tax Tools (LTT) Menu controls Code *********

		Next

		'Check if the selected menu item is part of the LTT module
		Dim pageURL As String = currentNode.Url.ToString
		Dim LTTmod As Boolean = False

		HttpContext.Current.Session.Add("useLiveLTTtable", LTTmod)

		'Format selected menu item
		menu.subMenuSelected(subMenu, selectedItem, leftImage, rightImage)

	End Sub

    'subMenuSelected() Method
    'Accepts menu web control and two images as parameters
    'Sets selected menu's separator image to none
    'and higlights selected tab
    Public Shared Sub subMenuSelected(ByRef subMenu As System.Web.UI.WebControls.Menu, ByRef subMenuItem As MenuItem, ByRef leftImage As System.Web.UI.WebControls.Image, ByRef rightImage As System.Web.UI.WebControls.Image)
        Dim position As Integer

        If Not IsNothing(subMenuItem) Then
            subMenuItem.Selected = True

            'Remove image separator on both sides of selected
            'menu item
            subMenuItem.SeparatorImageUrl = ""

            position = subMenu.Items.IndexOf(subMenuItem)

            If position > 0 Then
                position -= 1

            ElseIf position < 0 Then
                position = 0
            ElseIf position = 0 Then
                subMenu.Items(position).SeparatorImageUrl = ""
            End If

            'If codeDataFound Then

            'End If

            ' subMenu.Items(position).SeparatorImageUrl = ""

            'If there's only one item in the tab menu
            'and it's the item selected, corner images
            'are both changed
            If subMenu.Items.Count = 1 Then
                If subMenu.Items(0).Equals(subMenuItem) Then
                    leftImage.ImageUrl = "~/images/subNavLeftOn.gif"
                    rightImage.ImageUrl = "~/images/subNavRightOn.gif"
                End If

            ElseIf subMenu.Items.Count > 1 Then
                'If first item in tab menu is selected,
                'change left corner image
                If subMenu.Items(0).Equals(subMenuItem) Then
                    leftImage.ImageUrl = "~/images/subNavLeftOn.gif"
                End If

                'If last item in tab menu is selected,
                'change right corner image
                If subMenu.Items(subMenu.Items.Count - 1).Equals(subMenuItem) Then
                    rightImage.ImageUrl = "~/images/subNavRightOn.gif"
                End If
            End If
        End If
    End Sub


End Class
