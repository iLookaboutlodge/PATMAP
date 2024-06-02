Public Partial Class viewassessment
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Clears out the error message
            Master.errorMsg = ""

            If Not IsPostBack Then
                'Sets submenu to be displayed
                subMenu.setStartNode(menu.editData)
                BindData_ddl_DataSetName()
                ViewState("sSortBy") = "parcelID"
                ViewState("bCheckDESC") = "false"

            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number)
        End Try
    End Sub
   
    Protected Sub BindData_ddl_DataSetName()
        Dim CommandText As String = "select assessmentID, dataSetName from assessmentDescription where statusID = 1"
        Dim dt As DataTable = SqlDbAccess.RunSqlReturnDataTable(CommandText)
        'Session["DT_DSN"] = dt;//For EditAssassment
        If (dt.Rows.Count > 0) Then
            ddlDSN.DataSource = dt
            ddlDSN.DataValueField = "assessmentID"
            ddlDSN.DataTextField = "dataSetName"
            ddlDSN.DataBind()
            BindData_ddl(Integer.Parse(ddlDSN.SelectedValue.ToString))
        Else
            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP303")
            btnSearch.Visible = False
            btnAdd.Visible = False
        End If
    End Sub

    Protected Sub ddlDSN_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        BindData_ddl(Integer.Parse(ddlDSN.SelectedValue.ToString))
    End Sub

    Protected Sub BindData_ddl(ByVal assessmentID As Integer)
        'Dim CommandText As String = " select distinct municipalityID from assessment where assessmentID = '" + assessmentID.ToString + "' order by municipalityID" _
        '+ " select distinct schoolID from assessment where assessmentID = '" + assessmentID.ToString + "' order by schoolID" _
        '+ " select distinct taxClassID from assessment where assessmentID = '" + assessmentID.ToString + "' order by taxClassID" _
        '+ " select distinct presentUseCodeID from assessment where assessmentID = '" + assessmentID.ToString + "' order by presentUseCodeID"
        ''+ " select distinct parcelID from assessment where assessmentID = '" + assessmentID.ToString + "' order by parcelID" _
        'Dim ds As DataSet = SqlDbAccess.RunSqlReturnDataSet(CommandText)

        'ddlMunicipality.DataSource = ds.Tables(0)
        ''ddlMunicipality.DataValueField = "municipalityID";
        'ddlMunicipality.DataTextField = "municipalityID"
        'ddlMunicipality.DataBind()
        ''ddlMunicipality.Items.Insert(0, New ListItem("Select Data", "-1"))

        'ddlSchoolDivision.DataSource = ds.Tables(1)
        ''ddlSchoolDivision.DataValueField = "schoolID";
        'ddlSchoolDivision.DataTextField = "schoolID"
        'ddlSchoolDivision.DataBind()
        'ddlSchoolDivision.Items.Insert(0, New ListItem("Select Data", "-1"))

        'ddlTaxClass.DataSource = ds.Tables(2)
        ''ddlTaxClass.DataValueField = "taxClassID";
        'ddlTaxClass.DataTextField = "taxClassID"
        'ddlTaxClass.DataBind()
        'ddlTaxClass.Items.Insert(0, New ListItem("Select Data", "-1"))

        'ddlPresentUseCode.DataSource = ds.Tables(3)
        ''ddlPresentUseCode.DataValueField = "presentUseCodeID";
        'ddlPresentUseCode.DataTextField = "presentUseCodeID"
        'ddlPresentUseCode.DataBind()
        'ddlPresentUseCode.Items.Insert(0, New ListItem("Select Data", "-1"))

        'ddlParcelNo.DataSource = ds.Tables(4)
        'ddlParcelNo.DataTextField = "parcelID"
        'ddlParcelNo.DataBind()
        'ddlParcelNo.Items.Insert(0, New ListItem("Select Data", "-1"))

        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString
        con.Open()

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Dim CommandText As String = ""

        CommandText = "select distinct municipalityID from assessment where assessmentID = '" + assessmentID.ToString + "' order by municipalityID"
        da.SelectCommand = New SqlClient.SqlCommand()
        da.SelectCommand.Connection = con
        da.SelectCommand.CommandTimeout = 6000 'Inky's addition

        da.SelectCommand.CommandText = CommandText
        da.Fill(dt)
        ddlMunicipality.DataSource = dt
        ddlMunicipality.DataTextField = "municipalityID"
        ddlMunicipality.DataBind()
        ddlMunicipality.Items.Insert(0, New ListItem("Select Data", "-1"))


        dt.Clear()
        CommandText = "select distinct schoolID from assessment where assessmentID = '" + assessmentID.ToString + "' order by schoolID"
        da.SelectCommand = New SqlClient.SqlCommand()
        da.SelectCommand.Connection = con
        da.SelectCommand.CommandTimeout = 6000 'Inky's addition
        da.SelectCommand.CommandText = CommandText
        da.Fill(dt)
        ddlSchoolDivision.DataSource = dt
        ddlSchoolDivision.DataTextField = "schoolID"
        ddlSchoolDivision.DataBind()
        ddlSchoolDivision.Items.Insert(0, New ListItem("Select Data", "-1"))

        dt.Clear()
        CommandText = "select distinct taxClassID from assessment where assessmentID = '" + assessmentID.ToString + "' order by taxClassID"
        da.SelectCommand = New SqlClient.SqlCommand()
        da.SelectCommand.Connection = con
        da.SelectCommand.CommandTimeout = 6000 'Inky's addition
        da.SelectCommand.CommandText = CommandText
        da.Fill(dt)
        ddlTaxClass.DataSource = dt
        ddlTaxClass.DataTextField = "taxClassID"
        ddlTaxClass.DataBind()
        ddlTaxClass.Items.Insert(0, New ListItem("Select Data", "-1"))

        dt.Clear()
        CommandText = "select distinct presentUseCodeID from assessment where assessmentID = '" + assessmentID.ToString + "' order by presentUseCodeID"
        da.SelectCommand = New SqlClient.SqlCommand()
        da.SelectCommand.Connection = con
        da.SelectCommand.CommandTimeout = 6000 'Inky's addition
        da.SelectCommand.CommandText = CommandText
        da.Fill(dt)
        ddlPresentUseCode.DataSource = dt
        ddlPresentUseCode.DataTextField = "presentUseCodeID"
        ddlPresentUseCode.DataBind()
        ddlPresentUseCode.Items.Insert(0, New ListItem("Select Data", "-1"))
    End Sub

	Public Function IsSelectionValid() As Boolean
		If Not String.IsNullOrEmpty(Trim(txtAlternateParcelID.Text)) Then
			Return True
		End If
		If Not ddlMunicipality.SelectedValue.Equals("-1") Then
			Return True
		End If
		If Not ddlSchoolDivision.SelectedValue.Equals("-1") Then
			Return True
		End If
		If Not ddlTaxClass.SelectedValue.Equals("-1") Then
			Return True
		End If
		If Not ddlPresentUseCode.SelectedValue.Equals("-1") Then
			Return True
		End If
		Return False
	End Function

	Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
		If IsSelectionValid() Then
			DTAssessment_DataBind()
		Else
			Master.errorMsg = "No Selection, refine your search and try again"
		End If
	End Sub

    Protected Sub DTAssessment_DataBind()
        Dim CommandText As String = (" select * from assessment where " + (" assessmentID = '" _
                    + (ddlDSN.SelectedValue + "'")))
        'count(*) as Total,
        If Not ddlMunicipality.SelectedValue.Equals("-1") Then
            CommandText = (CommandText + (" and municipalityID='" _
                        + (ddlMunicipality.SelectedValue + "'")))
        End If
        If Not ddlSchoolDivision.SelectedValue.Equals("-1") Then
            CommandText = (CommandText + (" and schoolID='" _
                        + (ddlSchoolDivision.SelectedValue + "'")))
        End If
        'If Not txtParcelNo.Text.Equals("") Then
        '    CommandText = (CommandText + (" and parcelID='" _
        '                + (txtParcelNo.Text + "'")))
        'End If
        If Not txtAlternateParcelID.Text.Equals("") Then
            CommandText = (CommandText + (" and alternate_parcelID='" _
                        + (txtAlternateParcelID.Text + "'")))
        End If

        'If Not ddlParcelNo.SelectedValue.Equals("-1") Then
        '    CommandText = (CommandText + (" and parcelID='" _
        '                + (ddlParcelNo.SelectedValue + "'")))
        'End If

        If Not ddlTaxClass.SelectedValue.Equals("-1") Then
            CommandText = (CommandText + (" and taxClassID='" _
                        + (ddlTaxClass.SelectedValue + "'")))
        End If
        If Not ddlPresentUseCode.SelectedValue.Equals("-1") Then
            CommandText = (CommandText + (" and presentUseCodeID='" _
                        + (ddlPresentUseCode.SelectedValue + "'")))
        End If
        Dim dt As DataTable = SqlDbAccess.RunSqlReturnDataTable(CommandText)
        txtTotal.Text = dt.Rows.Count.ToString
        ViewState("DTgrdAssessment") = dt
        grdAssessment_DataBind()
    End Sub

    Protected Sub grdAssessment_DataBind()
        Dim sSortBy As String = ViewState("sSortBy").ToString
        Dim bCheckDESC As String = ViewState("bCheckDESC").ToString
        ' set atrributes of DataGrid to remember the sort settings
        grdAssessment.Attributes("SortItem") = sSortBy
        grdAssessment.Attributes("SortDesc") = bCheckDESC.ToString
        ' sort the data view
        Dim dt As DataTable = CType(ViewState("DTgrdAssessment"), DataTable)
        Dim asc As String = ""
        If Boolean.Parse(bCheckDESC) Then
            Asc = "DESC"
        Else
            Asc = "ASC"
        End If
        dt.DefaultView.Sort = String.Format("{0} {1}", sSortBy, Asc)
        'TODO: Warning!!!, inline IF is not supported ?
        ' set the data source of DataGrid
        grdAssessment.DataSource = dt
        ' Bind data to DataGrid
        grdAssessment.DataBind()
    End Sub

    Protected Sub grdAssessment_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        grdAssessment.PageIndex = e.NewPageIndex
        grdAssessment_DataBind()
        'grdAssessment.DataSource = ((DataTable)ViewState["DTgrdAssessment"]);
        'grdAssessment.DataBind();
    End Sub

    Protected Sub grdAssessment_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs)
        ' get the new sort item 
        Dim strSortItem As String = e.SortExpression
        ' get the old sort item
        Dim strOldSortItem As String = grdAssessment.Attributes("SortItem")
        ' set the default value to bDescend
        Dim bDescend As Boolean = False
        ' get the old value of bDescend
        Dim bOldDescend As Boolean = (grdAssessment.Attributes("SortDesc") = Boolean.TrueString)
        ' check if the same item be clicked and change the way of sorting
        If (strSortItem = strOldSortItem) Then
            bDescend = Not bOldDescend
        End If
        ' databind sorted data
        ViewState("sSortBy") = strSortItem
        ViewState("bCheckDESC") = bDescend.ToString
        grdAssessment_DataBind()
    End Sub

    Protected Sub grdAssessment_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)
        Try
            ' If grid is not being sorted
            If (Not e.CommandName.ToLower.Equals("sort") _
                        AndAlso Not e.CommandName.ToLower.Equals("page")) Then
                ' get selected row index and corresponding userID to that row
                Dim index As Integer = Integer.Parse(e.CommandArgument.ToString)
                Dim editRowID As String = grdAssessment.DataKeys(index)("RowID").ToString
                ' check what type of command has been fired by the grid
                Select Case (e.CommandName)
                    Case "colEdit"
                        Session.Add("assessmentID", editRowID)
                        Response.Redirect("editassessment.aspx")
                    Case "colDelete"
                        Dim CommandText As String = ("DELETE FROM assessment WHERE RowID = '" _
                                    + (editRowID + "'"))
                        SqlDbAccess.RunSql(CommandText)
                        DTAssessment_DataBind()
                End Select
            End If
        Catch

        End Try
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Session("assessmentID") = Nothing
        Response.Redirect("editassessment.aspx")
    End Sub
    Private Sub grdAssessment_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdAssessment.RowDataBound
        Try
            'attaches confirm script to button
            common.ConfirmDel(e, 1, DataBinder.Eval(e.Row.DataItem, "parcelID"))
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub
End Class