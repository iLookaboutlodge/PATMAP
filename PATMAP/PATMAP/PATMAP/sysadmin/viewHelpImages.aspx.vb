Public Partial Class viewHelpImages
	Inherits System.Web.UI.Page

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Try
			lblErrorText.Text = ""
			If Not IsPostBack Then
				searchImages()
				'Else
				'	Dim ControlID As String = ""
				'	CheckAutoPostBack(ControlID)
			End If
		Catch
			'retrieves error message
			lblErrorText.Text = common.GetErrorMessage(Err.Number)
		End Try

	End Sub

	'Protected Function CheckAutoPostBack(ByRef ControlID As String)
	'	Dim IsAutoPostBack As Boolean = False
	'	Dim currentButton As HyperLink = Nothing
	'	Dim controlName As String = Page.Request.Params("__EVENTTARGET")
	'	If Not String.IsNullOrEmpty(controlName) Then
	'		If Not IsNothing(Page.FindControl(controlName)) AndAlso TypeOf (Page.FindControl(controlName)) Is HyperLink Then
	'			currentButton = CType(Page.FindControl(controlName), HyperLink)
	'			'If lnkButton.ID = "lbStep1" Or lnkButton.ID = "lbStep2" Then
	'			If currentButton.UniqueID.IndexOf("grdImages$") <> -1 Then
	'				ControlID = currentButton.ID
	'				IsAutoPostBack = True
	'				image.ImageUrl = Page.Request.Params("__EVENTARGUMENT")
	'			End If
	'		End If
	'	End If
	'	Return IsAutoPostBack
	'End Function

	Private Sub grdImages_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdImages.RowDataBound
		Try
			'builds naviagteUrl link
			Dim currentButton As System.Web.UI.WebControls.HyperLink = Nothing

			If (e.Row.RowType = DataControlRowType.DataRow) Then
				currentButton = CType(e.Row.Cells.Item(1).Controls.Item(0), HyperLink)
				Dim URIString As String = HttpUtility.HtmlDecode(e.Row.Cells(2).Text)

				'currentButton.NavigateUrl = "javascript:displayImage('" & e.Row.Cells(2).Text & "')"
				currentButton.NavigateUrl = "javascript:displayImage('" & URIString & "')"

				'currentButton.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(currentButton, e.Row.Cells(2).Text))
				'currentButton.NavigateUrl = "#"
			End If

			'attaches confirm script to button
			common.ConfirmDel(e, 0, DataBinder.Eval(e.Row.DataItem, "imageName"))
		Catch
			'retrieves error message
			lblErrorText.Text = common.GetErrorMessage(Err.Number, Err)
		End Try
	End Sub

    Private Sub grdImages_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdImages.PageIndexChanging
        Try
            Dim dt As DataTable

            'sets grid's page index
            grdImages.PageIndex = e.NewPageIndex

            'fill the grid
            If Not IsNothing(Cache("helpImages")) Then
                dt = CType(Cache("helpImages"), DataTable)
                grdImages.DataSource = dt
                grdImages.DataBind()
            Else
                searchImages()
            End If
        Catch
            'retrieves error message
            lblErrorText.Text = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub grdImages_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdImages.RowCommand
        Try
            If LCase(e.CommandName) <> "sort" And LCase(e.CommandName) <> "page" Then
                'find the row index which to be edited/deleted
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)

                'check what type of command has been fired by the grid
                Select Case e.CommandName
                    Case "deleteFunction"

                        'check if the image is being used in any help pages
                        'setup database connection
                        Dim con As New SqlClient.SqlConnection
                        con.ConnectionString = PATMAP.Global_asax.connString
                        con.Open()

                        Dim dr As SqlClient.SqlDataReader
                        Dim query As New SqlClient.SqlCommand

                        query.Connection = con
                        query.CommandText = "select screenNameID from helpScreens where helpText like '%" & grdImages.DataKeys(index).Values("imageName").ToString & "%'"
                        dr = query.ExecuteReader

                        If dr.Read() Then
                            lblErrorText.Text = PATMAP.common.GetErrorMessage("PATMAP106")
                            Exit Sub
                        End If

                        Impersonate.impersonateValidUser(Global_asax.domainUser, Global_asax.domainName, Global_asax.domainPassword, "")
                        IO.File.Delete(PATMAP.Global_asax.imageFilePath & grdImages.DataKeys(index).Values("imageName").ToString)
                        Impersonate.undoImpersonation()

                        'update images search grid
                        searchImages()
                End Select
            End If
        Catch
            'retrieves error message
            lblErrorText.Text = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub grdImages_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdImages.Sorting
        Try
            Dim dt As DataTable
            Dim dv As DataView

            'fill the grid if there's no data stored in cache
            If IsNothing(Cache("helpImages")) Then
                searchImages()
            End If

            dt = CType(Cache("helpImages"), DataTable)
            dv = dt.DefaultView

            'sort grid
            dv.Sort = common.SortGrid(e.SortExpression, dv.Sort)
            grdImages.DataSource = dt
            grdImages.DataBind()

        Catch
            'retrieves error message
            lblErrorText.Text = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

	Private Sub searchImages()
		Dim di As New IO.DirectoryInfo(PATMAP.Global_asax.imageFilePath)
		Dim diar1 As IO.FileInfo()

		'Dim p2 = HttpContext.Current.Server.MapPath("~/images/helpTextImages")
		'Dim di As New IO.DirectoryInfo(p2)

		'make sure the images folder exists
		If di.Exists Then
			diar1 = di.GetFiles()
		Else
			lblErrorText.Text = "Help images folder is not found"
			Exit Sub
		End If

		Dim dra As IO.FileInfo
		Dim dr As System.Data.DataRow
		Dim requestUrl As String
		Dim domainUrl As String
		Dim domainProtocol As String

		domainUrl = Request.Url.Authority
		domainProtocol = Request.Url.Scheme	'assigns URL protocol (Eg. "http," "https," etc...)
		domainProtocol += Uri.SchemeDelimiter	'assigns protocol delimeter (i.e. "://")

		If Request.ApplicationPath <> "/" Then
			domainUrl &= Request.ApplicationPath
		End If

		'create a data table
		Dim dt As New DataTable
		dt.Columns.Add("imageName")
		dt.Columns.Add("imageUrl")

		'add rows to the data table - images' info
		For Each dra In diar1
			dr = dt.NewRow()
			dr("imageName") = dra.ToString()
			If Not IsNothing(Request.Url) Then
				requestUrl = Request.Url.ToString()
				'dr("imageUrl") = """" & domainProtocol & domainUrl & "/images/helpTextImages/" & dra.ToString()
				dr("imageUrl") = domainProtocol & domainUrl & "/images/helpTextImages/" & dra.ToString()
			Else
				dr("imageUrl") = ""
			End If
			dt.Rows.Add(dr)
		Next

		'store it in the cache
		If IsNothing(Cache("helpImages")) Then
			Cache.Add("helpImages", dt, Nothing, Now().AddMinutes(PATMAP.Global_asax.cacheExpiration), Nothing, CacheItemPriority.Low, Nothing)
		Else
			Cache("helpImages") = dt
		End If

		'bind the data grid
		grdImages.DataSource = dt
		grdImages.DataBind()
		txtTotal.Text = diar1.Length
	End Sub
End Class