Partial Public Class graphs2
    Inherits System.Web.UI.Page

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Try
			'Clears out the error message
			Master.errorMsg = ""

			'added for calculating LTT tables when Map is first time loaded	17-sep-2013
			System.Web.HttpContext.Current.Session.Remove("LTTcalculated")

			If Not IsPostBack Then
				'Sets submenu to be displayed
				subMenu.setStartNode(menu.taxtools)

				'applies Subject Municipality and Subject Year to labels
				If Not IsNothing(Session("LTTSubjectMunicipality")) Then
					lblLiveSubjMun.Text = Replace(StrConv(Session("LTTSubjectMunicipality"), VbStrConv.ProperCase), "Of ", "of ")

				Else
					'hides Subject Municipality label if no Subject has been selected (precationary - shouldn't happen)
					lblSubjMun.Visible = False
					lblLiveSubjMun.Visible = False
				End If
				lblLiveSubjYr.Text = Session("LTTsubjYear")
				lblLiveStartingRevenue.Text = Session("LTTSubjMunRev")
				lblLiveStartingUniformMillRate.Text = Session("LTTSubjUMR")

				Dim userID As Integer = Session("userID")

				'    'error checking
				'    If ckList.Items.Count = 0 Then
				'        'thow exception
				'        'Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
				'    End If

				'get the names of the subclasses or main classes depending on the permission

				'setup database connection
				Dim con As New SqlClient.SqlConnection
				con.ConnectionString = PATMAP.Global_asax.connString
				con.Open()

				Dim query As New SqlClient.SqlCommand
				Dim dr As SqlClient.SqlDataReader
				query.Connection = con

				If Session("phaseInBaseYearAccess") Then
					query.CommandText = "execute calcPhaseIn " & userID.ToString 'creates and populates liveLTTPhaseIn_(userID) table.
					query.ExecuteNonQuery()

					query.CommandText = "execute calcLTTPhaseInSummary " & userID.ToString
					query.ExecuteNonQuery()
				End If

				query.CommandText = "execute calcLTTResult " & userID.ToString
				query.ExecuteNonQuery()

				query.CommandText = "execute calcLTTSummary " & userID.ToString
				query.ExecuteNonQuery()


				Dim da As New SqlClient.SqlDataAdapter
				Dim dt As New DataTable

				If Session("showFullTaxClasses") = True Then
					da.SelectCommand = New SqlClient.SqlCommand("select t.taxClassID, t.taxClass, l.show from liveTaxClasses l inner join taxClasses t on l.taxClassID = t.taxClassID INNER JOIN liveLTTtaxClasses_" & Session("userID").ToString & " ON liveLTTtaxClasses_" & Session("userID").ToString & ".taxClassID = t.taxClassID WHERE t.active = 1 AND liveLTTtaxClasses_" & Session("userID").ToString & ".show = 1 AND userID = " & Session("userID").ToString, con)
				Else
					da.SelectCommand = New SqlClient.SqlCommand("SELECT DISTINCT m.taxClassID, m.taxClass FROM liveLTTValues v INNER JOIN LTTtaxClasses t ON t.taxClassID = v.taxClassID INNER JOIN LTTmainTaxClasses m ON m.taxClassID = t.LTTmainTaxClassID INNER JOIN liveLTTtaxClasses_" & Session("userID").ToString & " l ON l.taxClassID = t.taxClassID WHERE t.active = 1 AND l.show = 1 AND userID = " & Session("userID").ToString, con)
				End If

				da.Fill(dt)

				cklTaxClasses.DataSource = dt
				cklTaxClasses.DataValueField = "taxClassID"
				cklTaxClasses.DataTextField = "taxClass"
				cklTaxClasses.DataBind()

				'format checkboxlist to display 2 columns if there are more than 5 classes in a table
				If cklTaxClasses.Items.Count > 0 Then
					cklTaxClasses.RepeatColumns = "0"
					If cklTaxClasses.Items.Count > 5 Then
						cklTaxClasses.RepeatColumns = "2"
					End If
				End If

				'mark the subclasses or main classes previously selected by the user
				If Session("showFullTaxClasses") <> True Then
					'check if all the subclasses for the Agriculture are selected
					query.CommandText = "select * from liveTaxClasses where userID = " & Session("userID").ToString & " and taxClassID in (select taxClassID from LTTtaxClasses where LTTmainTaxClassID = 3) and show = 1"
					dr = query.ExecuteReader()
					If dr.Read() Then
						cklTaxClasses.Items(0).Selected = True
					End If
					dr.Close()

					'check if all the subclasses for the Residential are selected
					query.CommandText = "select * from liveTaxClasses where userID = " & Session("userID").ToString & " and taxClassID in (select taxClassID from LTTtaxClasses where LTTmainTaxClassID = 3) and show = 2"
					dr = query.ExecuteReader()
					If dr.Read() Then
						cklTaxClasses.Items(1).Selected = True
					End If
					dr.Close()

					'check if all the subclasses for the Commercial are selected
					query.CommandText = "select * from liveTaxClasses where userID = " & Session("userID").ToString & " and taxClassID in (select taxClassID from LTTtaxClasses where LTTmainTaxClassID = 3) and show = 3"
					dr = query.ExecuteReader()
					If dr.Read() Then
						cklTaxClasses.Items(2).Selected = True
					End If
					dr.Close()
				Else
					Dim numItems As Integer = dt.Rows.Count
					While numItems > 0
						If dt.Rows(numItems - 1).Item(2) = True Then
							cklTaxClasses.Items.FindByValue(dt.Rows(numItems - 1).Item(0)).Selected = True
						End If
						numItems = numItems - 1
					End While
				End If

				'mark the tax status previously selected by the user
				query.CommandText = "select taxstatusID, selected from liveTaxStatus where userID = " & Session("userID").ToString
				dr = query.ExecuteReader
				While dr.Read = True
					'taxable
					If dr.GetValue(0) = "1" And dr.GetValue(1) = True Then
						cklTaxStatus.Items(0).Selected = True
					End If

					'PGIL
					If dr.GetValue(0) = "6" And dr.GetValue(1) = True Then
						cklTaxStatus.Items(1).Selected = True
					End If

					'FGIL
					If dr.GetValue(0) = "5" And dr.GetValue(1) = True Then
						cklTaxStatus.Items(2).Selected = True
					End If

					'Exempt
					If dr.GetValue(0) = "4" And dr.GetValue(1) = True Then
						cklTaxStatus.Items(3).Selected = True
					End If
				End While
				dr.Close()

				con.Close()

				'report types
				'create an instance of our web service
				Dim ws As New PATMAPWebService.ReportingService2005
				'pass in the default credentials - meaning currently logged in user
				'ws.Credentials = System.Net.CredentialCache.DefaultCredentials
				'pass in the network credentials to access the reporting service
				ws.Credentials = New reportServerCredentials().NetworkCredentials

				'checks if the folder exists in the report server
				If ws.GetItemType(PATMAP.Global_asax.ReportLTTGraphsFolder) = PATMAPWebService.ItemTypeEnum.Folder Then

					Dim items As PATMAPWebService.CatalogItem() = ws.ListChildren(PATMAP.Global_asax.ReportLTTGraphsFolder, True)
					Dim catItem As PATMAPWebService.CatalogItem

					Dim ddlFirstItem = New ListItem("--Report Type--", "0")
					ddlReportType.Items.Add(ddlFirstItem)
					For Each catItem In items
						Dim ddlItem = New ListItem(catItem.Name, catItem.Path)
						ddlReportType.Items.Add(ddlItem)
					Next

				End If

			End If
		Catch
			'retrieves error message
			Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
		End Try
	End Sub

    Private Sub show_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles show.Click
        'setup database connection
        Dim con As New SqlClient.SqlConnection
        Dim query As New SqlClient.SqlCommand


        'If IsNothing(Session("LTTgraphreportPath")) Or Session("LTTgraphreportPath") = "0" Then
        If ddlReportType.SelectedValue = "" Or ddlReportType.SelectedValue = "0" Then
            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP131")
            Exit Sub
        End If

        con.ConnectionString = PATMAP.Global_asax.connString
        query.Connection = con
        query.CommandTimeout = 60000
        con.Open()

        query.CommandText = ""
        Dim numItems As Integer
        Dim anySelected As Integer = 0
        If Session("showFullTaxClasses") = True Then
            'update the live tbls with the selected classes and tax status
            numItems = cklTaxClasses.Items.Count
            While numItems > 0
                If cklTaxClasses.Items(numItems - 1).Selected Then
                    query.CommandText += "update liveTaxClasses set show = 1 where userID = " & Session("userID") & " and taxClassID = '" & cklTaxClasses.Items(numItems - 1).Value & "'" & vbCrLf
                    anySelected = 1
                Else
                    query.CommandText += "update liveTaxClasses set show = 0 where userID = " & Session("userID") & " and taxClassID = '" & cklTaxClasses.Items(numItems - 1).Value & "'" & vbCrLf
                End If
                numItems = numItems - 1
            End While
            'updates all inactive taxClasses as unselected
            query.CommandText += "update liveTaxClasses set show = 0 where userID = " & Session("userID") & " and taxClassID in (select taxClassID from LTTtaxClasses where active = 0)" & vbCrLf

            If anySelected = 0 Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP324")
                Exit Sub
            Else
                query.ExecuteNonQuery()
            End If
        Else
            'Update live tax classes by RollUp

            '**Inky's code... update live tbls by rollup 
            Dim i As Integer
            query.CommandText = ""
            For i = 0 To cklTaxClasses.Items.Count - 1
                If cklTaxClasses.Items(i).Selected = True Then
                    query.CommandText += "update liveTaxClasses set show = 1 where userID = " & Session("userID") & " and taxClassID in (select taxClassID from LTTtaxClasses where active = 1 and LTTmainTaxClassID = " & cklTaxClasses.Items(i).Value & ")" & vbCrLf
                    anySelected = 1
                Else
                    query.CommandText += "update liveTaxClasses set show = 0 where userID = " & Session("userID") & " and taxClassID in (select taxClassID from LTTtaxClasses where LTTmainTaxClassID = " & cklTaxClasses.Items(i).Value & ")" & vbCrLf
                End If
            Next

            If anySelected = 0 Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP324")
                Exit Sub
            Else
                query.ExecuteNonQuery()
            End If
        End If

        query.CommandText = ""
        numItems = cklTaxStatus.Items.Count
        anySelected = 0
        While numItems > 0
            If cklTaxStatus.Items(numItems - 1).Selected Then
                query.CommandText += "update liveTaxStatus set selected = 1 where userID = " & Session("userID") & " and taxstatusID = " & cklTaxStatus.Items(numItems - 1).Value & vbCrLf
                anySelected = 1
            Else
                query.CommandText += "update liveTaxStatus set selected = 0 where userID = " & Session("userID") & " and taxstatusID = " & cklTaxStatus.Items(numItems - 1).Value & vbCrLf
            End If
            numItems = numItems - 1
        End While
        If anySelected = 0 Then
            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP325")
            Exit Sub
        Else
            query.ExecuteNonQuery()
        End If
        con.Close()

        Dim strScript As String
        strScript = "<script language=javascript> window.open('LTTviewReport.aspx','newWin','toolbar=0,resizable=1,scrollbars=1'); </script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "Startup", strScript)


    End Sub

    Private Sub ddlReportType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlReportType.SelectedIndexChanged
        Session.Remove("LTTgraphreportPath")
        Session.Add("LTTgraphreportPath", ddlReportType.SelectedValue)
    End Sub
End Class