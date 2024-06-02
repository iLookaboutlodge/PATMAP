Public Partial Class editkog
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Clears out the error message
            Master.errorMsg = ""

            If Not IsPostBack Then
                'Sets submenu to be displayed
                subMenu.setStartNode(menu.editData)
                If (Not (Session("K12_RowID")) Is Nothing) Then
                    CurrentRowID = Session("K12_RowID").ToString
                    isK12New = False
                    BindData_CurrentK12()
                    tr_DSN.Visible = False
                Else
                    isK12New = True
                    BindData_ddl_DataSetName()
                End If
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number)
        End Try
    End Sub

    Protected Property isK12New() As Boolean
        Get
            Return Boolean.Parse(ViewState("isK12New").ToString)
        End Get
        Set(ByVal value As Boolean)
            ViewState("isK12New") = value
        End Set
    End Property

    Protected Property CurrentRowID() As String
        Get
            Return ViewState("CurrentRowID").ToString
        End Get
        Set(ByVal value As String)
            ViewState("CurrentRowID") = value
        End Set
    End Property

    Protected Property K12ID() As Integer
        Get
            Return Integer.Parse(ViewState("K12ID").ToString)
        End Get
        Set(ByVal value As Integer)
            ViewState("K12ID") = value.ToString
        End Set
    End Property

    Protected Sub BindData_CurrentK12()
        Dim CommandText As String = "select * from K12 where RowID = '" + CurrentRowID + "'"
        Dim dt As DataTable = SqlDbAccess.RunSqlReturnDataTable(CommandText)
        K12ID = Integer.Parse(dt.Rows(0)("K12ID").ToString)
        txtdivisionName.Text = dt.Rows(0)("divisionName").ToString
        txtdivisionType.Text = dt.Rows(0)("divisionType").ToString
        txttotalRecogExp.Text = dt.Rows(0)("totalRecogExp").ToString
        txtassessment.Text = dt.Rows(0)("assessment").ToString
        txtderivedGILAssessment.Text = dt.Rows(0)("derivedGILAssessment").ToString
        txttotalAssessment.Text = dt.Rows(0)("totalAssessment").ToString
        txtEQFactor.Text = dt.Rows(0)("EQFactor").ToString
        txtlocationRevenue.Text = dt.Rows(0)("locationRevenue").ToString
        txtotherRevenue.Text = dt.Rows(0)("otherRevenue").ToString
        txttotalRevenue.Text = dt.Rows(0)("totalRevenue").ToString
        txttotalGrant.Text = dt.Rows(0)("totalGrant").ToString
        txtgrantEntitlement.Text = dt.Rows(0)("grantEntitlement").ToString
        BindData_ddl(K12ID, dt.Rows(0)("schoolID").ToString)
    End Sub

    Protected Sub BindData_ddl_DataSetName()
        Dim CommandText As String = "select K12ID, dataSetName from K12Description where statusID = 1"
        Dim dt As DataTable = SqlDbAccess.RunSqlReturnDataTable(CommandText)
        If (dt.Rows.Count > 0) Then
            ddlDSN.DataSource = dt
            ddlDSN.DataValueField = "K12ID"
            ddlDSN.DataTextField = "dataSetName"
            ddlDSN.DataBind()
            K12ID = Integer.Parse(ddlDSN.SelectedValue.ToString)
            BindData_ddl(K12ID, "")
        Else
            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP303")
            btnSave.Visible = False
        End If
    End Sub

    Protected Sub ddlDSN_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        K12ID = Integer.Parse(ddlDSN.SelectedValue.ToString)
        BindData_ddl(K12ID, "")
    End Sub

    Protected Sub BindData_ddl(ByVal K12ID As Integer, ByVal SchoolDivision As String)

        Dim CommandText As String = "select number from entities where jurisdictionTypeID = 1 and number not in (select schoolID from K12 where K12ID = '" + K12ID.ToString + "') order by convert(int, number)"
        Dim ds As DataSet = SqlDbAccess.RunSqlReturnDataSet(CommandText)
        ddlSchoolDivision.DataSource = ds.Tables(0)
        ddlSchoolDivision.DataTextField = "number"
        ddlSchoolDivision.DataBind()
        If isK12New Then
            ddlSchoolDivision.Items.Insert(0, New ListItem("Select Data", "-1"))
        Else
            ddlSchoolDivision.Items.Insert(0, SchoolDivision)
            ddlSchoolDivision.Items.FindByValue(SchoolDivision).Selected = True
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        If Not ddlSchoolDivision.SelectedValue.Equals("-1") Then
            If isK12New Then
                Dim CommandText As String = "INSERT INTO [K12] ([K12ID],[schoolID],[divisionName],[divisionType],[totalRecogExp],[assessment],[derivedGILAssessment],[totalAssessment],[EQFactor],[locationRevenue],[otherRevenue],[totalRevenue],[totalGrant],[grantEntitlement]) VALUES (" _
                    + "'" + K12ID.ToString() + "'" _
                    + ",'" + ddlSchoolDivision.SelectedValue + "'" _
                    + ",'" + txtdivisionName.Text + "'" _
                    + ",'" + txtdivisionType.Text + "'" _
                    + ",'" + txttotalRecogExp.Text + "'" _
                    + ",'" + txtassessment.Text + "'" _
                    + ",'" + txtderivedGILAssessment.Text + "'" _
                    + ",'" + txttotalAssessment.Text + "'" _
                    + ",'" + txtEQFactor.Text + "'" _
                    + ",'" + txtlocationRevenue.Text + "'" _
                    + ",'" + txtotherRevenue.Text + "'" _
                    + ",'" + txttotalRevenue.Text + "'" _
                    + ",'" + txttotalGrant.Text + "'" _
                    + ",'" + txtgrantEntitlement.Text + "'" _
                    + ")"
                If SqlDbAccess.RunSql(CommandText) Then
                    Session("K12ID") = Nothing
                    Response.Redirect("viewkog.aspx")
                Else
                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP304")
                End If
            Else
                Dim CommandText As String = "UPDATE [K12] SET " _
                    + "[K12ID] = '" + K12ID.ToString() + "'" _
                    + ",[schoolID] = '" + ddlSchoolDivision.SelectedValue + "'" _
                    + ",[divisionName] = '" + txtdivisionName.Text + "'" _
                    + ",[divisionType] = '" + txtdivisionType.Text + "'" _
                    + ",[totalRecogExp] = '" + txttotalRecogExp.Text + "'" _
                    + ",[assessment] = '" + txtassessment.Text + "'" _
                    + ",[derivedGILAssessment] = '" + txtderivedGILAssessment.Text + "'" _
                    + ",[totalAssessment] = '" + txttotalAssessment.Text + "'" _
                    + ",[EQFactor] = '" + txtEQFactor.Text + "'" _
                    + ",[locationRevenue] = '" + txtlocationRevenue.Text + "'" _
                    + ",[otherRevenue] = '" + txtotherRevenue.Text + "'" _
                    + ",[totalRevenue] = '" + txttotalRevenue.Text + "'" _
                    + ",[totalGrant] = '" + txttotalGrant.Text + "'" _
                    + ",[grantEntitlement] = '" + txtgrantEntitlement.Text + "'" _
                    + " WHERE [RowID]='" + CurrentRowID + "'"
                If SqlDbAccess.RunSql(CommandText) Then
                    'MTE code Start
                    CommandText = "update taxYearModelDescription set dataStale = 1 where taxYearStatusID in (1,3) and K12ID = (select K12ID from K12 where RowID = '" + CurrentRowID + "')" + vbCrLf _
                                                       + "update assessmentTaxModel set dataStale = 1 where SubjectTaxYearModelID in (select taxYearModelID from taxYearModelDescription where K12ID = (select K12ID from K12 where RowID = '" + CurrentRowID + "'))" + vbCrLf _
                                                        + "update assessmentTaxModel set dataStale = 1 where BaseTaxYearModelID in (select taxYearModelID from taxYearModelDescription where K12ID = (select K12ID from K12 where RowID = '" + CurrentRowID + "'))" + vbCrLf _
                                                        + "update liveAssessmentTaxModel set dataStale = 1 where SubjectTaxYearModelID in (select taxYearModelID from taxYearModelDescription where K12ID = (select K12ID from K12 where RowID = '" + CurrentRowID + "'))" + vbCrLf _
                                                        + "update liveAssessmentTaxModel set dataStale = 1 where BaseTaxYearModelID in (select taxYearModelID from taxYearModelDescription where K12ID = (select K12ID from K12 where RowID = '" + CurrentRowID + "'))"

                    SqlDbAccess.RunSql(CommandText)
                    'MTE code End
                    Session("K12ID") = Nothing
                    Response.Redirect("viewkog.aspx")
                Else
                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP305")
                End If
            End If
        Else
            Dim message As String = ""
            If ddlSchoolDivision.SelectedValue.Equals("-1") Then
                message = (message + "<br>Select SchoolDivision Please")
            End If
            Master.errorMsg = message
        End If
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Response.Redirect("viewkog.aspx")
    End Sub



End Class