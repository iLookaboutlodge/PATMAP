Imports System.Data.SqlClient

Partial Public Class tables1
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

				Dim mapDataStale As Boolean = False

				'get current user
				Dim userID As Integer = Session("userID")

				'check if data is stale (ie: if the user has transfered some new properties from/to the subject mun)
				query.CommandText = "select mapDataStale from liveBoundaryModel where userid = " & userID
				dr = query.ExecuteReader
				dr.Read()
				mapDataStale = CType(dr.GetValue(0), Boolean)
				dr.Close()

				If mapDataStale Or IsNothing(System.Web.HttpContext.Current.Session("calculated")) Then
					'change the status of the BoundaryChangeStale to true
					If IsNothing(System.Web.HttpContext.Current.Session("BoundaryChangeStale")) Then
						System.Web.HttpContext.Current.Session.Add("BoundaryChangeStale", True)
					End If
					System.Web.HttpContext.Current.Session("BoundaryChangeStale") = True

					Response.Write(common.JavascriptFunctions())
					Response.Flush()

					Response.Write(common.DisplayDiv("Please wait while the calculation is in progress...", -1))
					Response.Flush()

					common.calculateBoundaryModel(userID)

					query.CommandText = "update liveBoundaryModel set mapDataStale = 0 where userid = " & userID
					query.ExecuteNonQuery()

					System.Web.HttpContext.Current.Session("BoundaryChangeStale") = False

					Dim delStr As New StringBuilder
					delStr.Append("DELETE MunicipalitiesChanges WHERE UserID = @UserID DELETE mapBoundaryTransfers WHERE UserID = @UserID")
					Dim clearChangesCmd As SqlCommand = New SqlCommand(delStr.ToString(), con)
					clearChangesCmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userID
					clearChangesCmd.ExecuteNonQuery()

					BuildBoundary.buildBoundary(userID)

					'create School Municipality and Parcels tables for Map viewing
					common.CreateBOUNDARYCHANGE_School_Mun_Parcel_Tables()


					System.Web.HttpContext.Current.Session.Add("calculated", "true")

					Response.Write(common.HideDiv())
					Response.Flush()

					Response.Write("<script language=javascript>window.navigate('tables.aspx');</script>")
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
								Case "PGIL"
									ddlTaxStatus.SelectedValue = 6
									Exit For
								Case "FGIL"
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

						If ddlTaxStatus.SelectedValue > 0 Then
							Session("TaxStatus") = ddlTaxStatus.SelectedValue
						End If

					End If

					'tax classes
					dt.Clear()
					da.SelectCommand.CommandText = "select ' ' as taxClassID, '--Tax Classes--' as taxClass  union all select t.taxClassID, t.taxClass from taxClasses t, liveTaxClasses l where t.taxClassID = l.taxClassID AND l.show=1 AND userID=" & Session("userID")
					da.Fill(dt)
					ddlTaxClasses.DataSource = dt
					ddlTaxClasses.DataValueField = "taxClassID"
					ddlTaxClasses.DataTextField = "taxClass"
					ddlTaxClasses.DataBind()

					If Not IsNothing(Session("TaxClass")) Then
						ddlTaxClasses.SelectedValue = Session("TaxClass")
					End If

					If Not IsNothing(Session("TaxStatus")) Then
						ddlTaxStatus.SelectedValue = Session("TaxStatus")
					End If

					If IsNothing(Session("taxStatusPageID")) Then
						query.CommandText = "Delete from liveTaxStatus where userID = " & userID
						query.ExecuteNonQuery()

						query.CommandText = "Insert into liveTaxStatus values(" & userID & ",1,1) "
						query.CommandText += "Insert into liveTaxStatus values(" & userID & ",5,1) "
						query.CommandText += "Insert into liveTaxStatus values(" & userID & ",6,1) "
						query.CommandText += "Insert into liveTaxStatus values(" & userID & ",4,0) "
						query.ExecuteNonQuery()
					End If

					'clean up
					con.Close()

					'report types
					'create an instance of our web service
					Dim ws As New PATMAPWebService.ReportingService2005
					ws.Credentials = New reportServerCredentials().NetworkCredentials

					'checks if the folder exists in the report server
					If ws.GetItemType(PATMAP.Global_asax.ReportBoundaryTablesFolder) = PATMAPWebService.ItemTypeEnum.Folder Then

						Dim items As PATMAPWebService.CatalogItem() = ws.ListChildren(PATMAP.Global_asax.ReportBoundaryTablesFolder, True)
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
						reportPath = Replace(Session("reportPath"), PATMAP.Global_asax.ReportGraphsFolder, PATMAP.Global_asax.ReportBoundaryTablesFolder)
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
		Session.Add("pageID", 1)
		Response.Redirect("classes.aspx")
	End Sub

	'Private Sub ddlTaxStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTaxStatus.SelectedIndexChanged
	'    Try
	'        If ddlTaxStatus.SelectedValue = 0 Then
	'            Session.Remove("MapTaxStatusFilters")
	'        Else
	'            Dim selectedTaxStatus As New List(Of String)

	'            Select Case Trim(ddlTaxStatus.SelectedValue)
	'                Case 6
	'                    selectedTaxStatus.Add("PGIL")
	'                Case 5
	'                    selectedTaxStatus.Add("FGIL")
	'                Case 4
	'                    selectedTaxStatus.Add("Exempt")
	'                Case 1
	'                    selectedTaxStatus.Add("Taxable")
	'                Case Else
	'                    selectedTaxStatus.Add(ddlTaxStatus.SelectedItem.Text)
	'            End Select


	'            Session("MapTaxStatusFilters") = selectedTaxStatus
	'        End If
	'    Catch
	'        Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
	'    End Try
	'End Sub

	Private Sub btnTaxStatus_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnTaxStatus.Click
		Session.Add("pageID", 1)
		Response.Redirect("taxStatusBoundary.aspx")
	End Sub

	Private Sub ddlReport_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlReport.SelectedIndexChanged
		If ddlReport.SelectedIndex <> 0 Then
			Session("reportPath") = ddlReport.SelectedValue
		Else
			Session.Remove("reportPath")
		End If
	End Sub

	Private Sub ddlTaxClasses_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTaxClasses.SelectedIndexChanged
		Try
			If ddlTaxClasses.SelectedValue = " " Then
				Session.Remove("TaxClass")
			Else
				Session("TaxClass") = ddlTaxClasses.SelectedValue
			End If
		Catch
			Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
		End Try
	End Sub
	Private Sub ddlTaxStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTaxStatus.SelectedIndexChanged
		Try
			If ddlTaxStatus.SelectedValue = 0 Then
				Session.Remove("TaxStatus")
			Else
				Session("TaxStatus") = ddlTaxStatus.SelectedValue
			End If
		Catch
			Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
		End Try
	End Sub
End Class