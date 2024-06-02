Public Partial Class downloadfile
    Inherits System.Web.UI.Page

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Try

			Dim fileID As String = Request.QueryString("file") '-- if something was passed to the file querystring  
			Dim id As String = Request.QueryString("id")
			Dim subFolder As String = Request.QueryString("type")

			'Don't allow file dowload if user
			'hasn't successfully logged in or
			'his session expires          

			If fileID <> "" And id <> "" And subFolder <> "" And Not IsNothing(Session("userID")) Then 'get absolute path of the file  

				Using con As SqlClient.SqlConnection = New SqlClient.SqlConnection(PATMAP.Global_asax.connString)
					If Not con.State = ConnectionState.Open Then
						con.Open()
					End If

					'Dim tableName As String = ""
					Dim fileName As String = ""

					Dim strQry As String = ""

					Select Case subFolder
						Case edittaxyearmodel.subFolder
							'tableName = "taxYearModelFile"
							strQry = "select [filename] from taxYearModelFile where fileID = @fileID"
						Case editdataset.assessmentSubFolder
							'tableName = "assessmentFile"
							strQry = "select [filename] from assessmentFile where fileID = @fileID"
						Case editdataset.taxCreditSubFolder
							'tableName = "taxCreditFile"
							strQry = "select [filename] from taxCreditFile where fileID = @fileID"
						Case editdataset.POVSubFolder
							'tableName = "POVFile"
							strQry = "select [filename] from POVFile where fileID = @fileID"
						Case editdataset.millReateSurveySubFolder
							'tableName = "millRateSurveyFile"
							strQry = "select [filename] from millRateSurveyFile where fileID = @fileID"
						Case editdataset.K12OGSurveySubFolder
							'tableName = "K12File"
							strQry = "select [filename] from K12File where fileID = @fileID"
						Case general.liveSubFolder
							'tableName = "liveAssessmentTaxModelFile"
							strQry = "select [filename] from liveAssessmentTaxModelFile where fileID = @fileID"
						Case editboundarymodel.subFolder
							'tableName = "boundaryModelFile"
							strQry = "select [filename] from boundaryModelFile where fileID = @fileID"
					End Select

					If Not String.IsNullOrEmpty(strQry) Then

						Using query As SqlClient.SqlCommand = New SqlClient.SqlCommand
							query.Connection = con
							query.CommandText = strQry
							query.Parameters.Add(New SqlClient.SqlParameter("@fileID", SqlDbType.Int)).Value = fileID

							Using dr As SqlClient.SqlDataReader = query.ExecuteReader
								If dr.Read() Then
									fileName = dr.Item(0)
								End If
							End Using

							Dim path As String = common.GetFilePath(id, subFolder) & fileName
							Dim file As System.IO.FileInfo = New System.IO.FileInfo(path)	'get file object as FileInfo  

							'if the file exists on the server  
							If file.Exists Then	'set appropriate headers  
								Response.Clear()
								Response.AddHeader("Content-Disposition", "attachment; filename=" & file.Name)
								Response.AddHeader("Content-Length", file.Length.ToString())
								Response.ContentType = "application/octet-stream"
								Response.WriteFile(file.FullName)
							Else
								'if file does not exist
								Response.Write(PATMAP.common.GetErrorMessage("PATMAP48"))
								Exit Sub
							End If

						End Using

					End If
				End Using
			Else
				Response.Write(PATMAP.common.GetErrorMessage("PATMAP48"))
				Exit Sub
			End If

		Catch
			Response.Write(common.GetErrorMessage(Err.Number, Err))
			Exit Sub
		End Try

		Response.End()

	End Sub

	'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
	'    Try

	'        Dim fileID As String = Request.QueryString("file") '-- if something was passed to the file querystring  
	'        Dim id As String = Request.QueryString("id")
	'        Dim subFolder As String = Request.QueryString("type")

	'        'Don't allow file dowload if user
	'        'hasn't successfully logged in or
	'        'his session expires          

	'        If fileID <> "" And id <> "" And subFolder <> "" And Not IsNothing(Session("userID")) Then 'get absolute path of the file  

	'            'setup database connection
	'            Dim con As New SqlClient.SqlConnection
	'            con.ConnectionString = PATMAP.Global_asax.connString

	'            con.Open()

	'            Dim query As New SqlClient.SqlCommand
	'            Dim dr As SqlClient.SqlDataReader
	'            Dim tableName As String = ""
	'            Dim fileName As String = ""

	'            Select Case subFolder
	'                Case edittaxyearmodel.subFolder
	'                    tableName = "taxYearModelFile"
	'                Case editdataset.assessmentSubFolder
	'                    tableName = "assessmentFile"
	'                Case editdataset.taxCreditSubFolder
	'                    tableName = "taxCreditFile"
	'                Case editdataset.POVSubFolder
	'                    tableName = "POVFile"
	'                Case editdataset.millReateSurveySubFolder
	'                    tableName = "millRateSurveyFile"
	'                Case editdataset.K12OGSurveySubFolder
	'                    tableName = "K12File"
	'                Case general.liveSubFolder
	'                    tableName = "liveAssessmentTaxModelFile"
	'                Case editboundarymodel.subFolder
	'                    tableName = "boundaryModelFile"
	'            End Select

	'            If tableName <> "" Then
	'                'gets file information
	'                query.Connection = con
	'                query.CommandText = "select [filename] from " & tableName & " where fileID = " & fileID

	'                dr = query.ExecuteReader()

	'                If dr.Read() Then
	'                    fileName = dr.Item(0)
	'                End If

	'                con.Close()

	'                Dim path As String = common.GetFilePath(id, subFolder) & fileName
	'                Dim file As System.IO.FileInfo = New System.IO.FileInfo(path) 'get file object as FileInfo  

	'                'if the file exists on the server  
	'                If file.Exists Then 'set appropriate headers  
	'                    Response.Clear()
	'                    Response.AddHeader("Content-Disposition", "attachment; filename=" & file.Name)
	'                    Response.AddHeader("Content-Length", file.Length.ToString())
	'                    Response.ContentType = "application/octet-stream"
	'                    Response.WriteFile(file.FullName)
	'                Else
	'                    'if file does not exist
	'                    Response.Write(PATMAP.common.GetErrorMessage("PATMAP48"))
	'                    Exit Sub
	'                End If
	'            End If

	'        Else
	'            Response.Write(PATMAP.common.GetErrorMessage("PATMAP48"))
	'            Exit Sub
	'        End If
	'    Catch
	'        Response.Write(common.GetErrorMessage(Err.Number, Err))
	'        Exit Sub
	'    End Try

	'    Response.End()

	'End Sub

End Class