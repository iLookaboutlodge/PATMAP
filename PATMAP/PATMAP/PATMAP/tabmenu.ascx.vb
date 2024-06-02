Public Partial Class tabmenu
    Inherits System.Web.UI.UserControl

	Private Sub subMenu_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles subMenu.PreRender
		Dim mstr As MasterPage
		Dim node As SiteMapNode

		If Not IsPostBack Then
			'References control's page master
			mstr = Page.Master

			Try
				'Retrieves starting node from SiteMap
				node = SiteMap.Provider.FindSiteMapNode(subSiteMapDataSource.StartingNodeUrl)

				'Dynammically loads tab menu
				menu.loadSubMenu(subMenu, node, imgNavLeft, imgNavRight, Session("levelID"))

			Catch
				'Retrieves error message
				mstr.errorMsg = common.GetErrorMessage(Err.Number, Err)
			End Try
		Else
			' if not postback do folling for Boundary Changes only
			Try
				'References control's page master
				mstr = Page.Master
				If Not IsNothing(subSiteMapDataSource.StartingNodeUrl) Then
					'Retrieves starting node from SiteMap
					node = SiteMap.Provider.FindSiteMapNode(subSiteMapDataSource.StartingNodeUrl)
					If node IsNot Nothing Then
						If node.Title = "Boundary Changes" Then
							If SiteMap.CurrentNode.Title = "Main" Then
								'Dynammically loads tab menu
								menu.loadSubMenu(subMenu, node, imgNavLeft, imgNavRight, Session("levelID"))
							End If
						End If
					End If
				End If
			Catch
				'Retrieves error message
				mstr.errorMsg = common.GetErrorMessage(Err.Number, Err)
			End Try
		End If

	End Sub

    'setStartNode() Mehtod
    'Accepts a String parameter
    'Sets starting node for the sitemap data source
    Public Sub setStartNode(ByVal path As String)
        subSiteMapDataSource.StartingNodeUrl = path
    End Sub
End Class