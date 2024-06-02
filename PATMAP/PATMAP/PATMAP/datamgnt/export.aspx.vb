Public Partial Class export
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Master.errorMsg = ""

            If Not IsPostBack Then
                Dim query As String

                'fill municipality ddl
                ddlMunicipality.DataSource = common.FillMunicipality(0)
                ddlMunicipality.DataValueField = "number"
                ddlMunicipality.DataTextField = "jurisdiction"
                ddlMunicipality.DataBind()

                'fill school division ddl
                query = "select 0 as number, '--School Division--' as jurisdiction  union all select e.number, dbo.ProperCase(e.jurisdiction) as jurisdiction from entities e where e.jurisdictionTypeID=1"
                fillDropDown(ddlSchoolDivision, "number", "jurisdiction", query)

                'fill tax type ddl
                query = "select 0 as taxStatusID, '--Tax Type--' as taxStatus  union all select taxStatusID, taxStatus from taxStatus"
                fillDropDown(ddlTaxType, "taxStatusID", "taxStatus", query)

                'fill tax class ddl
                query = "select ' ' as taxClassID, '--Tax Classes--' as taxClass  union all select taxClassID, taxClass from taxClasses"
                fillDropDown(ddlTaxClass, "taxClassID", "taxClass", query)

                'fill potash area ddl
                query = "select 0 as potashAreaID, '--Potash Areas--' as potashArea  union all select potashAreaID, potashArea from potashAreas"
                fillDropDown(ddlPotashArea, "potashAreaID", "potashArea", query)

            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub ddlDataSource_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDataSource.SelectedIndexChanged
        Try
            Master.errorMsg = ""

            'get the data source name selected
            Dim dsNm As Integer
            dsNm = ddlDataSource.SelectedValue

            'Based on the selection of the data source, populate the data set list
            'setup database connection
            Dim con As New SqlClient.SqlConnection
            con.ConnectionString = PATMAP.Global_asax.connString
            con.Open()

            Dim da As New SqlClient.SqlDataAdapter
            da.SelectCommand = New SqlClient.SqlCommand()
            da.SelectCommand.Connection = con

            Dim dt As New DataTable
            Dim query As String

            Select Case dsNm
                'If the data source is assessment
                Case 1
                    'enable the filters
                    lblParcelNo.Visible = True
                    txtParcelNo.Visible = True
                    lblMunicipality.Visible = True
                    ddlMunicipality.Visible = True
                    lblSchoolDivision.Visible = True
                    ddlSchoolDivision.Visible = True
                    lblTaxType.Visible = True
                    ddlTaxType.Visible = True
                    lblTaxClass.Visible = True
                    ddlTaxClass.Visible = True
                    lblPotashArea.Visible = False
                    ddlPotashArea.Visible = False
                    lblFileType.Visible = False
                    ddlFileType.Visible = False

                    rpvReports.Visible = False

                    'fill data set ddl
                    query = "select 0 as dataSetID, '--Data Set--' as dataSetName union all select assessmentID, dataSetName from assessmentDescription where statusID=1"
                    fillDropDown(ddlDSN, "dataSetID", "dataSetName", query)

                    clearDDL()

                    'If the data source is tax credit
                    'Donna - Tax Credit option has been removed.
                    'Case 2
                    '    'enable filters
                    '    lblTaxClass.Visible = True
                    '    ddlTaxClass.Visible = True
                    '    lblParcelNo.Visible = False
                    '    txtParcelNo.Visible = False
                    '    lblMunicipality.Visible = False
                    '    ddlMunicipality.Visible = False
                    '    lblSchoolDivision.Visible = False
                    '    ddlSchoolDivision.Visible = False
                    '    lblTaxType.Visible = False
                    '    ddlTaxType.Visible = False
                    '    lblPotashArea.Visible = False
                    '    ddlPotashArea.Visible = False
                    '    lblFileType.Visible = False
                    '    ddlFileType.Visible = False

                    '    rpvReports.Visible = False

                    '    'fill data set ddl
                    '    query = "select 0 as dataSetID, '--Data Set--' as dataSetName union all select taxCreditID, dataSetName from taxCreditDescription  where statusID=1"
                    '    fillDropDown(ddlDSN, "dataSetID", "dataSetName", query)

                    '    clearDDL()

                    'If the data source is pvo
                Case 3
                    'enable filters
                    lblTaxClass.Visible = True
                    ddlTaxClass.Visible = True
                    lblParcelNo.Visible = False
                    txtParcelNo.Visible = False
                    lblMunicipality.Visible = False
                    ddlMunicipality.Visible = False
                    lblSchoolDivision.Visible = False
                    ddlSchoolDivision.Visible = False
                    lblTaxType.Visible = False
                    ddlTaxType.Visible = False
                    lblPotashArea.Visible = False
                    ddlPotashArea.Visible = False
                    lblFileType.Visible = False
                    ddlFileType.Visible = False

                    rpvReports.Visible = False

                    'fill data set ddl
                    query = "select 0 as dataSetID, '--Data Set--' as dataSetName union all select POVID, dataSetName from POVDescription  where statusID=1"
                    fillDropDown(ddlDSN, "dataSetID", "dataSetName", query)

                    clearDDL()

                    'If the data source is K12
                    'Donna - K12OG option has been removed.
                    'Case 4
                    '    'enable filters
                    '    lblTaxClass.Visible = False
                    '    ddlTaxClass.Visible = False
                    '    lblParcelNo.Visible = False
                    '    txtParcelNo.Visible = False
                    '    lblMunicipality.Visible = False
                    '    ddlMunicipality.Visible = False
                    '    lblSchoolDivision.Visible = True
                    '    ddlSchoolDivision.Visible = True
                    '    lblTaxType.Visible = False
                    '    ddlTaxType.Visible = False
                    '    lblPotashArea.Visible = False
                    '    ddlPotashArea.Visible = False
                    '    lblFileType.Visible = False
                    '    ddlFileType.Visible = False

                    '    rpvReports.Visible = False

                    '    'fill data set ddl
                    '    query = "select 0 as dataSetID, '--Data Set--' as dataSetName union all select K12ID, dataSetName from K12Description  where statusID=1"
                    '    fillDropDown(ddlDSN, "dataSetID", "dataSetName", query)

                    '    clearDDL()

                    'If the data source is millratesurvey
                Case 5
                    'enable filters
                    lblTaxClass.Visible = False
                    ddlTaxClass.Visible = False
                    lblParcelNo.Visible = False
                    txtParcelNo.Visible = False
                    lblMunicipality.Visible = True
                    ddlMunicipality.Visible = True
                    lblSchoolDivision.Visible = False
                    ddlSchoolDivision.Visible = False
                    lblTaxType.Visible = False
                    ddlTaxType.Visible = False
                    lblPotashArea.Visible = False
                    ddlPotashArea.Visible = False
                    lblFileType.Visible = False
                    ddlFileType.Visible = False

                    rpvReports.Visible = False

                    'fill data set ddl
                    query = "select 0 as dataSetID, '--Data Set--' as dataSetName union all select millRateSurveyID, dataSetName from millRateSurveyDescription  where statusID=1"
                    fillDropDown(ddlDSN, "dataSetID", "dataSetName", query)

                    clearDDL()

                    'If the data source is potash
                Case 6
                    'enable filters
                    lblTaxClass.Visible = False
                    ddlTaxClass.Visible = False
                    lblParcelNo.Visible = False
                    txtParcelNo.Visible = False
                    lblMunicipality.Visible = True
                    ddlMunicipality.Visible = True
                    lblSchoolDivision.Visible = False
                    ddlSchoolDivision.Visible = False
                    lblTaxType.Visible = False
                    ddlTaxType.Visible = False
                    lblPotashArea.Visible = True
                    ddlPotashArea.Visible = True
                    lblFileType.Visible = True
                    ddlFileType.Visible = True

                    rpvReports.Visible = False

                    'fill data set ddl
                    query = "select 0 as dataSetID, '--Data Set--' as dataSetName union all select potashID, dataSetName from potashDescription  where statusID=1"
                    fillDropDown(ddlDSN, "dataSetID", "dataSetName", query)

                    clearDDL()

                Case Else
                    'disable filters
                    lblTaxClass.Visible = False
                    ddlTaxClass.Visible = False
                    lblParcelNo.Visible = False
                    txtParcelNo.Visible = False
                    lblMunicipality.Visible = False
                    ddlMunicipality.Visible = False
                    lblSchoolDivision.Visible = False
                    ddlSchoolDivision.Visible = False
                    lblTaxType.Visible = False
                    ddlTaxType.Visible = False
                    lblPotashArea.Visible = False
                    ddlPotashArea.Visible = False
                    lblFileType.Visible = False
                    ddlFileType.Visible = False

                    rpvReports.Visible = False


                    clearDDL()

            End Select

            'clean up
            con.Close()

        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub fillDropDown(ByRef ddlControl As System.Web.UI.WebControls.DropDownList, ByVal dataValueField As String, ByVal dataTextField As String, ByVal query As String)
        Try
            'Based on the selection of the data source, populate the data set list
            'setup database connection
            Dim con As New SqlClient.SqlConnection
            con.ConnectionString = PATMAP.Global_asax.connString
            con.Open()

            Dim da As New SqlClient.SqlDataAdapter
            da.SelectCommand = New SqlClient.SqlCommand()
            da.SelectCommand.Connection = con

            Dim dt As New DataTable

            da.SelectCommand.CommandText = query
            da.Fill(dt)
            ddlControl.DataSource = dt
            ddlControl.DataValueField = dataValueField
            ddlControl.DataTextField = dataTextField
            ddlControl.DataBind()

            con.Close()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSearch.Click
        Try
            Dim dataSource As String = ddlDataSource.SelectedValue
            'if no data source is selected, display an error message
            If dataSource = 0 Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP132")
                Exit Sub
            End If

            rpvReports.Visible = True

            'create the param list
            Dim reportParamArray(7) As Microsoft.Reporting.WebForms.ReportParameter

            Dim dataSetID As String = "0"
            If dataSource <> "6" Or ddlFileType.SelectedValue <> "3" Then
                dataSetID = ddlDSN.SelectedValue
            End If
            Dim param0 = New Microsoft.Reporting.WebForms.ReportParameter("dataSetID", dataSetID)
            reportParamArray(0) = param0

            Dim parcelID As String = "-1"
            If Trim(txtParcelNo.Text) <> "" And dataSource = "1" Then
                parcelID = Trim(txtParcelNo.Text)
            End If
            Dim param1 = New Microsoft.Reporting.WebForms.ReportParameter("parcelID", parcelID)
            reportParamArray(1) = param1

            Dim municipalityID As String = " "
            If ddlMunicipality.SelectedValue <> " " And (dataSource = "1" Or dataSource = "5" Or dataSource = "6") Then
                municipalityID = ddlMunicipality.SelectedValue
            End If
            Dim param2 = New Microsoft.Reporting.WebForms.ReportParameter("municipalityID", municipalityID)
            reportParamArray(2) = param2

            Dim schoolDivisionID As String = "0"
            If ddlSchoolDivision.SelectedValue <> "0" And (dataSource = "1" Or dataSource = "4") Then
                schoolDivisionID = ddlSchoolDivision.SelectedValue
            End If
            Dim param3 = New Microsoft.Reporting.WebForms.ReportParameter("schoolDivisionID", schoolDivisionID)
            reportParamArray(3) = param3

            Dim taxTypeID As String = "0"
            If ddlTaxType.SelectedValue <> "0" And dataSource = "1" Then
                taxTypeID = ddlTaxType.SelectedValue
            End If
            Dim param4 = New Microsoft.Reporting.WebForms.ReportParameter("taxTypeID", taxTypeID)
            reportParamArray(4) = param4

            Dim taxClassID As String = " "
            If ddlTaxClass.SelectedValue <> " " And (dataSource = "1" Or dataSource = "2" Or dataSource = "3") Then
                taxClassID = ddlTaxClass.SelectedValue
            End If
            Dim param5 = New Microsoft.Reporting.WebForms.ReportParameter("taxClassID", taxClassID)
            reportParamArray(5) = param5

            Dim fileType As String = "0"
            If dataSource = "6" Then
                fileType = ddlFileType.SelectedValue
            End If
            Dim param6 = New Microsoft.Reporting.WebForms.ReportParameter("fileType", fileType)
            reportParamArray(6) = param6

            Dim potashAreaID As String = "0"
            If dataSource = "6" Then
                potashAreaID = ddlPotashArea.SelectedValue
            End If
            Dim param7 = New Microsoft.Reporting.WebForms.ReportParameter("potashAreaID", potashAreaID)
            reportParamArray(7) = param7

            rpvReports.ShowCredentialPrompts = False
            rpvReports.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote
            rpvReports.ServerReport.ReportServerUrl = New Uri(PATMAP.Global_asax.SQLReportingServer)
            rpvReports.ServerReport.ReportServerCredentials = New reportServerCredentials()

            'create an instance of our web service
            Dim ws As New PATMAPWebService.ReportingService2005
            ws.Credentials = New reportServerCredentials().NetworkCredentials

            'checks if the folder exists in the report server
            If ws.GetItemType(PATMAP.Global_asax.ReportExportFolder) = PATMAPWebService.ItemTypeEnum.Folder Then

                'choose various reports based on the selection of datasource
                If dataSource = "1" Then
                    rpvReports.ServerReport.ReportPath = PATMAP.Global_asax.ReportExportFolder & "/Assessment"
                    'Donna - Tax Credit option has been removed.
                    'ElseIf dataSource = "2" Then
                    '    rpvReports.ServerReport.ReportPath = PATMAP.Global_asax.ReportExportFolder & "/Tax Credit"
                ElseIf dataSource = "3" Then
                    rpvReports.ServerReport.ReportPath = PATMAP.Global_asax.ReportExportFolder & "/POV"
                    'Donna - K12OG option has been removed.
                    'ElseIf dataSource = "4" Then
                    '    rpvReports.ServerReport.ReportPath = PATMAP.Global_asax.ReportExportFolder & "/K12OG"
                ElseIf dataSource = "5" Then
                    rpvReports.ServerReport.ReportPath = PATMAP.Global_asax.ReportExportFolder & "/Mill Rate Survey"
                Else
                    If ddlFileType.SelectedValue = "1" Then
                        rpvReports.ServerReport.ReportPath = PATMAP.Global_asax.ReportExportFolder & "/Rural Municipalities"
                    End If

                    If ddlFileType.SelectedValue = "2" Then
                        rpvReports.ServerReport.ReportPath = PATMAP.Global_asax.ReportExportFolder & "/Urban Municipalities"
                    End If

                    If ddlFileType.SelectedValue = "3" Then
                        rpvReports.ServerReport.ReportPath = PATMAP.Global_asax.ReportExportFolder & "/Potash Properties"
                    End If
                End If


                rpvReports.ServerReport.SetParameters(reportParamArray)
                rpvReports.ZoomMode = Microsoft.Reporting.WebForms.ZoomMode.PageWidth
                rpvReports.ServerReport.Refresh()

            Else
                Master.errorMsg = common.GetErrorMessage("PATMAP136")
                Exit Sub
            End If

        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub clearDDL()
        Try
            ddlTaxClass.Items(ddlTaxClass.SelectedIndex).Selected = False
            ddlTaxClass.Items(0).Selected = True

            txtParcelNo.Text = ""

            ddlMunicipality.Items(ddlMunicipality.SelectedIndex).Selected = False
            ddlMunicipality.Items(0).Selected = True

            ddlSchoolDivision.Items(ddlSchoolDivision.SelectedIndex).Selected = False
            ddlSchoolDivision.Items(0).Selected = True

            ddlTaxType.Items(ddlTaxType.SelectedIndex).Selected = False
            ddlTaxType.Items(0).Selected = True

            ddlFileType.Items(ddlFileType.SelectedIndex).Selected = False
            ddlFileType.Items(0).Selected = True

            ddlPotashArea.Items(ddlPotashArea.SelectedIndex).Selected = False
            ddlPotashArea.Items(0).Selected = True
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub ddlFileType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFileType.SelectedIndexChanged
        'if file type is potash properties
        If ddlFileType.SelectedValue = 3 Then
            lblDSN.Visible = False
            ddlDSN.Visible = False            
        Else
            'for everything else
            lblDSN.Visible = True
            ddlDSN.Visible = True
        End If

        rpvReports.Visible = False
    End Sub
End Class