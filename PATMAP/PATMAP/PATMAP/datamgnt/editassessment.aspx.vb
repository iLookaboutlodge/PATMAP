Partial Public Class editassessment
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Clears out the error message
            Master.errorMsg = ""

            If Not IsPostBack Then
                'Sets submenu to be displayed
                subMenu.setStartNode(menu.editData)
                If (Not (Session("assessmentID")) Is Nothing) Then
                    CurrentRowID = Session("assessmentID").ToString
                    isAssessmentNew = False
                    BindData_CurrentAssessment()
                    tr_DSN.Visible = False
                Else
                    isAssessmentNew = True
                    BindData_ddl_DataSetName()
                End If
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number)
        End Try
    End Sub

    Private Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Try
            'change page title and breadcrumb to 
            'if the mode is either edit or add
            common.ChangeTitle(Session("assessmentID"), lblTitle)
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number)
        End Try
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            common.UndoChange()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number)
        End Try
    End Sub

    Protected Property isAssessmentNew() As Boolean
        Get
            Return Boolean.Parse(ViewState("isAssessmentNew").ToString)
        End Get
        Set(ByVal value As Boolean)
            ViewState("isAssessmentNew") = value
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

    Protected Property AssessmentID() As Integer
        Get
            Return Integer.Parse(ViewState("AssessmentID").ToString)
        End Get
        Set(ByVal value As Integer)
            ViewState("AssessmentID") = value.ToString
        End Set
    End Property

    Protected Sub BindData_CurrentAssessment()
        Dim CommandText As String = ("select * from assessment where RowID = '" _
                    + (CurrentRowID + "'"))
        Dim dt As DataTable = SqlDbAccess.RunSqlReturnDataTable(CommandText)
        AssessmentID = Integer.Parse(dt.Rows(0)("assessmentID").ToString)
        txtParcelID.Text = dt.Rows(0)("parcelID").ToString
        txtAlternateParcelID.Text = dt.Rows(0)("alternate_parcelID").ToString
        txtLegalLand.Text = dt.Rows(0)("LLD").ToString
        txtCivicAddr.Text = dt.Rows(0)("civicAddress").ToString
        txtPresentUseCode.Text = dt.Rows(0)("presentUseCodeID").ToString
        txtMarketValue.Text = dt.Rows(0)("marketValue").ToString
        txtTaxAssessment.Text = dt.Rows(0)("taxable").ToString
        txtOtherExempt.Text = dt.Rows(0)("otherExempt").ToString
        txtFedGIL.Text = dt.Rows(0)("FGIL").ToString
        txtProvGIL.Text = dt.Rows(0)("PGIL").ToString
        txtSection293.Text = dt.Rows(0)("Section293").ToString
        txtBylaw.Text = dt.Rows(0)("ByLawExemption").ToString
        BindData_ddl(AssessmentID, dt.Rows(0)("municipalityID").ToString, dt.Rows(0)("schoolID").ToString, dt.Rows(0)("taxClassID").ToString)
    End Sub

    Protected Sub BindData_ddl_DataSetName()
        Dim CommandText As String = "select assessmentID, dataSetName from assessmentDescription where statusID = 1"
        Dim dt As DataTable = SqlDbAccess.RunSqlReturnDataTable(CommandText)
        If (dt.Rows.Count > 0) Then
            ddlDSN.DataSource = dt
            ddlDSN.DataValueField = "assessmentID"
            ddlDSN.DataTextField = "dataSetName"
            ddlDSN.DataBind()
            AssessmentID = Integer.Parse(ddlDSN.SelectedValue.ToString)
            BindData_ddl(AssessmentID, "", "", "")
        Else
            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP303")
            btnSave.Visible = False
        End If
    End Sub

    Protected Sub ddlDSN_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        AssessmentID = Integer.Parse(ddlDSN.SelectedValue.ToString)
        BindData_ddl(AssessmentID, "", "", "")
    End Sub

    Protected Sub BindData_ddl(ByVal assessmentID As Integer, ByVal Municipality As String, ByVal SchoolDivision As String, ByVal TaxClass As String)
        Dim CommandText As String = (" select distinct municipalityID from assessment where assessmentID = '" _
                    + (assessmentID.ToString + ("' order by municipalityID" + (" select distinct schoolID from assessment where assessmentID = '" _
                    + (assessmentID.ToString + ("' order by schoolID" + (" select distinct taxClassID from assessment where assessmentID = '" _
                    + (assessmentID.ToString + "' order by taxClassID"))))))))
        Dim ds As DataSet = SqlDbAccess.RunSqlReturnDataSet(CommandText)
        ddlMunicipality.DataSource = ds.Tables(0)
        ddlMunicipality.DataTextField = "municipalityID"
        ddlMunicipality.DataBind()
        If isAssessmentNew Then
            ddlMunicipality.Items.Insert(0, New ListItem("Select Data", "-1"))
            ddlMunicipality.SelectedIndex = 0
        Else
            ddlMunicipality.Items.FindByValue(Municipality).Selected = True
        End If
        ddlSchoolDivision.DataSource = ds.Tables(1)
        ddlSchoolDivision.DataTextField = "schoolID"
        ddlSchoolDivision.DataBind()
        If isAssessmentNew Then
            ddlSchoolDivision.Items.Insert(0, New ListItem("Select Data", "-1"))
        Else
            ddlSchoolDivision.Items.FindByValue(SchoolDivision).Selected = True
        End If
        ddlTaxClass.DataSource = ds.Tables(2)
        ddlTaxClass.DataTextField = "taxClassID"
        ddlTaxClass.DataBind()
        If isAssessmentNew Then
            ddlTaxClass.Items.Insert(0, New ListItem("Select Data", "-1"))
        Else
            ddlTaxClass.Items.FindByValue(TaxClass).Selected = True
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        If (Not ddlMunicipality.SelectedValue.Equals("-1") _
                    AndAlso (Not ddlSchoolDivision.SelectedValue.Equals("-1") _
                    AndAlso (Not ddlTaxClass.SelectedValue.Equals("-1") _
                    AndAlso (Not txtParcelID.Text.Equals("") _
                    AndAlso Not txtAlternateParcelID.Text.Equals(""))))) Then
            If isAssessmentNew Then
                Dim CommandText As String = "INSERT INTO [assessment] ([assessmentID],[ISCParcelNumber],[parcelID],[municipalityID_orig],[municipalityID],[alternate_parcelID],[LLD],[civicAddress],[presentUseCodeID],[schoolID],[taxClassID_orig],[taxClassID],[marketValue],[taxable],[otherExempt],[FGIL],[PGIL],[Section293],[ByLawExemption]) VALUES (" _
                    + "'" + AssessmentID.ToString() + "'" _
                    + ", NULL" _
                    + ",'" + txtParcelID.Text + "'" _
                    + ",'" + ddlMunicipality.SelectedValue + "'" _
                    + ",'" + ddlMunicipality.SelectedValue + "'" _
                    + ",'" + txtAlternateParcelID.Text + "'" _
                    + ",'" + txtLegalLand.Text + "'" _
                    + ",'" + txtCivicAddr.Text + "'" _
                    + ",'" + txtPresentUseCode.Text + "'" _
                    + ",'" + ddlSchoolDivision.SelectedValue + "'" _
                    + ",'" + ddlTaxClass.SelectedValue + "'" _
                    + ",'" + ddlTaxClass.SelectedValue + "'" _
                    + ",'" + txtMarketValue.Text + "'" _
                    + ",'" + txtTaxAssessment.Text + "'" _
                    + ",'" + txtOtherExempt.Text + "'" _
                    + ",'" + txtFedGIL.Text + "'" _
                    + ",'" + txtProvGIL.Text + "'" _
                    + ",'" + txtSection293.Text + "'" _
                    + ",'" + txtBylaw.Text + "'" _
                    + ")"
                If SqlDbAccess.RunSql(CommandText) Then
                    Session("assessmentID") = Nothing
                    Response.Redirect("viewassessment.aspx")
                Else
                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP304")
                End If
            Else
                Dim CommandText As String = "UPDATE [assessment] SET " _
                + " [assessmentID] = '" + AssessmentID.ToString + "'" _
                + ",[ISCParcelNumber] = NULL" _
                + ",[parcelID] = '" + txtParcelID.Text + "'" _
                + ",[municipalityID] = '" + ddlMunicipality.SelectedValue + "'" _
                + ",[alternate_parcelID] = '" + txtAlternateParcelID.Text + "'" _
                + ",[LLD] = '" + txtLegalLand.Text + "'" _
                + ",[civicAddress] = '" + txtCivicAddr.Text + "'" _
                + ",[presentUseCodeID] = '" + txtPresentUseCode.Text + "'" _
                + ",[schoolID] = '" + ddlSchoolDivision.SelectedValue + "'" _
                + ",[taxClassID] = '" + ddlTaxClass.SelectedValue + "'" _
                + ",[marketValue] = '" + txtMarketValue.Text + "'" _
                + ",[taxable] = '" + txtTaxAssessment.Text + "'" _
                + ",[otherExempt] = '" + txtOtherExempt.Text + "'" _
                + ",[FGIL] = '" + txtFedGIL.Text + "'" _
                + ",[PGIL] = '" + txtProvGIL.Text + "'" _
                + ",[Section293] = '" + txtSection293.Text + "'" _
                + ",[ByLawExemption] = '" + txtBylaw.Text + "'" _
                + " WHERE [RowID]='" + CurrentRowID + "'"
                If SqlDbAccess.RunSql(CommandText) Then

                    'MTE code Start
                    CommandText = "update taxYearModelDescription set dataStale = 1 where taxYearStatusID in (1,3) and assessmentID = (select assessmentID from assessment where RowID = '" + CurrentRowID + "')" + vbCrLf _
                                + "update assessmentDescription set dataStale = 1 where assessmentID = (select assessmentID from assessment where RowID = '" + CurrentRowID + "')" + vbCrLf _
                                + "update assessmentTaxModel set dataStale = 1 where SubjectTaxYearModelID in (select taxYearModelID from taxYearModelDescription where assessmentID = (select assessmentID from assessment where RowID = '" + CurrentRowID + "'))" + vbCrLf _
                                + "update assessmentTaxModel set dataStale = 1 where BaseTaxYearModelID in (select taxYearModelID from taxYearModelDescription where assessmentID = (select assessmentID from assessment where RowID = '" + CurrentRowID + "'))" + vbCrLf _
                                + "update liveAssessmentTaxModel set dataStale = 1 where SubjectTaxYearModelID in (select taxYearModelID from taxYearModelDescription where assessmentID = (select assessmentID from assessment where RowID = '" + CurrentRowID + "'))" + vbCrLf _
                                + "update liveAssessmentTaxModel set dataStale = 1 where BaseTaxYearModelID in (select taxYearModelID from taxYearModelDescription where assessmentID = (select assessmentID from assessment where RowID = '" + CurrentRowID + "'))"
                    SqlDbAccess.RunSql(CommandText)
                    'MTE code End

                    Session("assessmentID") = Nothing
                    Response.Redirect("viewassessment.aspx")
                Else
                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP305")
                End If
            End If
        Else
            Dim message As String = ""
            If txtParcelID.Text.Equals("") Then
                message = (message + "<br>Type Parcel ID Please")
            End If
            If txtAlternateParcelID.Text.Equals("") Then
                message = (message + "<br>Type Alternate Parcel ID Please")
            End If
            If ddlMunicipality.SelectedValue.Equals("-1") Then
                message = (message + "<br>Select Municipality Please")
            End If
            If ddlSchoolDivision.SelectedValue.Equals("-1") Then
                message = (message + "<br>Select SchoolDivision Please")
            End If
            If ddlTaxClass.SelectedValue.Equals("-1") Then
                message = (message + "<br>Select TaxClass Please")
            End If
            Master.errorMsg = message
        End If
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Response.Redirect("viewassessment.aspx")
    End Sub


End Class