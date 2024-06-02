Public Partial Class graphs1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Clears out the error message
            Master.errorMsg = ""

            If Not IsPostBack Then
                'Keeps session for the reportviewer. ReportViewer is in a frame so session
                'isn't normally tracked.
                Response.AddHeader("P3P", "CP=""CAO PSA OUR""")

                'Sets submenu to be displayed
                subMenu.setStartNode(menu.boundary)

                'setup database connection
                Dim con As New SqlClient.SqlConnection
                Dim da As New SqlClient.SqlDataAdapter
                Dim dt As New DataTable
                con.ConnectionString = PATMAP.Global_asax.connString
                Dim query As New SqlClient.SqlCommand
                query.Connection = con
                con.Open()
                Dim dr As SqlClient.SqlDataReader

                Dim mapDataStale As Integer

                'get current user
                Dim userID As Integer = Session("userID")

                'check if data is stale (ie: if the user has transfered some new properties from/to the subject mun)
                query.CommandText = "select mapDataStale from dbo.liveBoundaryModel where userid = " & userID
                dr = query.ExecuteReader
                dr.Read()
                mapDataStale = dr.GetValue(0)
                dr.Close()

                If mapDataStale Or IsNothing(Session("calculated")) Then
                    Response.Write(common.JavascriptFunctions())
                    Response.Flush()

                    Response.Write(common.DisplayDiv("Please wait while the calculation is in progress...", -1))
                    Response.Flush()

                    common.calculateBoundaryModel(userID)

                    query.CommandText = "update liveBoundaryModel set mapDataStale = 0 where userid = " & userID
                    query.ExecuteNonQuery()

                    Session.Add("calculated", "true")

                    Response.Write(common.HideDiv())
                    Response.Flush()

                    Response.Write("<script language=javascript>window.navigate('graphs.aspx');</script>")
                    Response.End()
                Else
                    da.SelectCommand = query
                    da.SelectCommand.CommandText = "select 0 as taxStatusID, '--Tax Status--' as taxStatus  union all select taxStatusID, taxStatus from taxStatus"
                    da.Fill(dt)
                    ddlTaxStatus.DataSource = dt
                    ddlTaxStatus.DataValueField = "taxStatusID"
                    ddlTaxStatus.DataTextField = "taxStatus"
                    ddlTaxStatus.DataBind()

                    Dim selectedTaxStatus As List(Of String)
                    Dim counter As Integer

                    If Not IsNothing(Session("MapTaxStatusFilters")) Then
                        selectedTaxStatus = CType(Session("MapTaxStatusFilters"), List(Of String))

                        For counter = 0 To selectedTaxStatus.Count - 1

                            Select Case Trim(selectedTaxStatus(counter))
                                Case "Provincial grant in lieu"
                                    ddlTaxStatus.SelectedValue = 6
                                    Exit For
                                Case "Federal grant in lieu"
                                    ddlTaxStatus.SelectedValue = 5
                                    Exit For
                                Case "Exempt"
                                    ddlTaxStatus.SelectedValue = 4
                                    Exit For
                                Case "Taxable"
                                    ddlTaxStatus.SelectedValue = 1
                                    Exit For
                                Case Else
                                    If Not IsNothing(ddlTaxStatus.Items.FindByValue(Trim(selectedTaxStatus(counter)))) Then
                                        ddlTaxStatus.SelectedValue = ddlTaxStatus.Items.FindByValue(Trim(selectedTaxStatus(counter))).Value
                                        Exit For
                                    End If
                            End Select
                        Next
                    End If

                    Dim selectedTaxShift As List(Of String)

                    If Not IsNothing(Session("MapTaxShiftFilters")) Then

                        selectedTaxShift = CType(Session("MapTaxShiftFilters"), List(Of String))

                        For counter = 0 To selectedTaxShift.Count - 1
                            Select Case Trim(selectedTaxShift(counter))
                                Case "Municipal Tax"
                                    ddlTaxType.SelectedValue = 1
                                    Exit For
                                Case "School Tax"
                                    ddlTaxType.SelectedValue = 2
                                    Exit For
                                Case "Grant"
                                    ddlTaxType.SelectedValue = 3
                                    Exit For
                                Case Else
                                    If Not IsNothing(ddlTaxType.Items.FindByValue(Trim(selectedTaxShift(counter)))) Then
                                        ddlTaxType.SelectedValue = ddlTaxType.Items.FindByValue(Trim(selectedTaxShift(counter))).Value
                                        Exit For
                                    End If
                            End Select
                        Next
                    End If

                    'clean up
                    con.Close()

                    'report types
                    'create an instance of our web service
                    Dim ws As New PATMAPWebService.ReportingService2005
                    ws.Credentials = New reportServerCredentials().NetworkCredentials

                    'checks if the folder exists in the report server
                    If ws.GetItemType(PATMAP.Global_asax.ReportBoundaryTablesFolder) = PATMAPWebService.ItemTypeEnum.Folder Then

                        Dim items As PATMAPWebService.CatalogItem() = ws.ListChildren(PATMAP.Global_asax.ReportTablesFolder, True)
                        Dim catItem As PATMAPWebService.CatalogItem

                        Dim ddlFirstItem = New ListItem("--Report Type--", "0")
                        ddlReport.Items.Add(ddlFirstItem)
                        For Each catItem In items
                            Dim ddlItem = New ListItem(catItem.Name, catItem.Path)
                            ddlReport.Items.Add(ddlItem)
                        Next
                    End If

                    Dim reportPath As String = ""

                    If Not IsNothing(Session("reportPath")) Then

                        reportPath = Replace(Session("reportPath"), PATMAP.Global_asax.ReportGraphsFolder, PATMAP.Global_asax.ReportTablesFolder)

                        If Not IsNothing(ddlReport.Items.FindByValue(reportPath)) Then
                            ddlReport.SelectedValue = reportPath
                        End If
                    End If

                End If

            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub btnClasses_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnClasses.Click
        Session.Add("pageID", 0)
        Response.Redirect("classes.aspx")
    End Sub

    Private Sub ddlTaxType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTaxType.SelectedIndexChanged
        Try
            If ddlTaxType.SelectedValue = 0 Then
                Session.Remove("MapTaxShiftFilters")
            Else

                Dim selectedTaxShift As New List(Of String)

                Select Case ddlTaxType.SelectedValue
                    Case 1
                        selectedTaxShift.Add("Municipal Tax")
                    Case 2
                        selectedTaxShift.Add("School Tax")
                    Case 3
                        selectedTaxShift.Add("Grant")
                End Select

                Session("MapTaxShiftFilters") = selectedTaxShift
            End If
        Catch
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub ddlTaxStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTaxStatus.SelectedIndexChanged
        Try
            If ddlTaxStatus.SelectedValue = 0 Then
                Session.Remove("MapTaxStatusFilters")
            Else
                Dim selectedTaxStatus As New List(Of String)

                Select Case Trim(ddlTaxStatus.SelectedValue)
                    Case 6
                        selectedTaxStatus.Add("Provincial grant in lieu")
                    Case 5
                        selectedTaxStatus.Add("Federal grant in lieu")
                    Case 4
                        selectedTaxStatus.Add("Exempt")
                    Case 1
                        selectedTaxStatus.Add("Taxable")
                    Case Else
                        selectedTaxStatus.Add(ddlTaxStatus.SelectedItem.Text)
                End Select


                Session("MapTaxStatusFilters") = selectedTaxStatus
            End If
        Catch
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub
End Class