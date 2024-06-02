Public Partial Class editmillrate
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Clears out the error message
            Master.errorMsg = ""

            If Not IsPostBack Then
                'Sets submenu to be displayed
                subMenu.setStartNode(menu.editData)
                If (Not (Session("millRateSurvey_RowID")) Is Nothing) Then
                    CurrentRowID = Session("millRateSurvey_RowID").ToString
                    ismillRateSurveyNew = False
                    BindData_CurrentmillRateSurvey()
                    tr_DSN.Visible = False
                Else
                    ismillRateSurveyNew = True
                    BindData_ddl_DataSetName()
                End If
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number)
        End Try
    End Sub

    Protected Property ismillRateSurveyNew() As Boolean
        Get
            Return Boolean.Parse(ViewState("ismillRateSurveyNew").ToString)
        End Get
        Set(ByVal value As Boolean)
            ViewState("ismillRateSurveyNew") = value
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

    Protected Property millRateSurveyID() As Integer
        Get
            Return Integer.Parse(ViewState("millRateSurveyID").ToString)
        End Get
        Set(ByVal value As Integer)
            ViewState("millRateSurveyID") = value.ToString
        End Set
    End Property

    Protected Sub BindData_CurrentmillRateSurvey()
        Dim CommandText As String = ("select * from millRateSurvey where RowID = '" _
                    + (CurrentRowID + "'"))
        Dim dt As DataTable = SqlDbAccess.RunSqlReturnDataTable(CommandText)
        millRateSurveyID = Integer.Parse(dt.Rows(0)("millRateSurveyID").ToString)
        txtRevenue.Text = dt.Rows(0)("Levy").ToString
        Dim MunicipalityID As String = dt.Rows(0)("MunicipalityID").ToString
        BindData_ddl(millRateSurveyID, MunicipalityID)
    End Sub

    Protected Sub BindData_ddl_DataSetName()
        Dim CommandText As String = "select millRateSurveyID, dataSetName from millRateSurveyDescription where statusID = 1"
        Dim dt As DataTable = SqlDbAccess.RunSqlReturnDataTable(CommandText)
        If (dt.Rows.Count > 0) Then
            ddlDSN.DataSource = dt
            ddlDSN.DataValueField = "millRateSurveyID"
            ddlDSN.DataTextField = "dataSetName"
            ddlDSN.DataBind()
            millRateSurveyID = Integer.Parse(ddlDSN.SelectedValue.ToString)
            BindData_ddl(millRateSurveyID, "")
        Else
            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP303")
            btnSave.Visible = False
        End If
    End Sub

    Protected Sub BindData_ddl(ByVal millRateSurveyID As Integer, ByVal MunicipalityID As String)
        Dim CommandText As String = " select number from entities where " _
            + " jurisdictionTypeID <> 1 " _
            + " and number not in (select MunicipalityID from millRateSurvey where millRateSurveyID = '" + millRateSurveyID.ToString() + "')" _
            + " order by number"
        Dim ds As DataSet = SqlDbAccess.RunSqlReturnDataSet(CommandText)
        ddlMunicipalityID.DataSource = ds.Tables(0)
        ddlMunicipalityID.DataTextField = "number"
        ddlMunicipalityID.DataBind()
        If ismillRateSurveyNew Then
            ddlMunicipalityID.Items.Insert(0, New ListItem("Select Data", "-1"))
        Else
            ddlMunicipalityID.Items.Insert(0, MunicipalityID)
            ddlMunicipalityID.Items.FindByValue(MunicipalityID).Selected = True
        End If
    End Sub

    Protected Sub ddlDSN_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlDSN.SelectedIndexChanged
        millRateSurveyID = Integer.Parse(ddlDSN.SelectedValue.ToString)
        BindData_ddl(millRateSurveyID, "")
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        If (Not ddlMunicipalityID.SelectedValue.Equals("-1") _
                    AndAlso Not txtRevenue.Text.Equals("")) Then
            If ismillRateSurveyNew Then
                Dim CommandText As String = "INSERT INTO [millRateSurvey] ([millRateSurveyID],[MunicipalityID],[Levy]) VALUES (" _
                    + "'" + millRateSurveyID.ToString() + "'" _
                    + ",'" + ddlMunicipalityID.SelectedValue + "'" _
                    + ",'" + txtRevenue.Text + "'" _
                    + ")"
                If SqlDbAccess.RunSql(CommandText) Then
                    Session("millRateSurveyID") = Nothing
                    Response.Redirect("viewmillrate.aspx")
                Else
                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP304")
                End If
            Else
                Dim CommandText As String = "UPDATE [millRateSurvey] SET " _
                    + "[millRateSurveyID] = '" + millRateSurveyID.ToString() + "'" _
                    + ",[MunicipalityID] = '" + ddlMunicipalityID.SelectedValue + "'" _
                    + ",[Levy] = '" + txtRevenue.Text + "'" _
                    + " WHERE [RowID]='" + CurrentRowID + "'"
                If SqlDbAccess.RunSql(CommandText) Then

                    'MTE code Start
                    CommandText = "update taxYearModelDescription set dataStale = 1 where taxYearStatusID in (1,3) and millRateSurveyID = (select millRateSurveyID from millRateSurvey where RowID = '" + CurrentRowID + "')" + vbCrLf _
                                    + "update assessmentTaxModel set dataStale = 1 where SubjectTaxYearModelID in (select taxYearModelID from taxYearModelDescription where millRateSurveyID = (select millRateSurveyID from millRateSurvey where RowID = '" + CurrentRowID + "'))" + vbCrLf _
                                    + "update assessmentTaxModel set dataStale = 1 where BaseTaxYearModelID in (select taxYearModelID from taxYearModelDescription where millRateSurveyID = (select millRateSurveyID from millRateSurvey where RowID = '" + CurrentRowID + "'))" + vbCrLf _
                                     + "update liveAssessmentTaxModel set dataStale = 1 where SubjectTaxYearModelID in (select taxYearModelID from taxYearModelDescription where millRateSurveyID = (select millRateSurveyID from millRateSurvey where RowID = '" + CurrentRowID + "'))" + vbCrLf _
                                    + "update liveAssessmentTaxModel set dataStale = 1 where BaseTaxYearModelID in (select taxYearModelID from taxYearModelDescription where millRateSurveyID = (select millRateSurveyID from millRateSurvey where RowID = '" + CurrentRowID + "'))"

                    SqlDbAccess.RunSql(CommandText)
                    'MTE code End

                    Session("millRateSurveyID") = Nothing
                    Response.Redirect("viewmillrate.aspx")
                Else
                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP305")
                End If
            End If
        Else
            Dim message As String = ""
            If ddlMunicipalityID.SelectedValue.Equals("-1") Then
                message = (message + "<br>Select Municipality ID Please")
            End If
            If txtRevenue.Text.Equals("") Then
                message = (message + "<br>Type Revenue Please")
            End If
            Master.errorMsg = message
        End If
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Response.Redirect("viewmillrate.aspx")
    End Sub



End Class