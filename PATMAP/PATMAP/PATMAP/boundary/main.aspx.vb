Public Partial Class main
    Inherits System.Web.UI.Page

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Try
			'Clears out the error message
			Master.errorMsg = ""

			'setup database connection
			Dim con As New SqlClient.SqlConnection
			con.ConnectionString = PATMAP.Global_asax.connString
			con.Open()

			Dim query As New SqlClient.SqlCommand
			query.Connection = con

			'get userid
			Dim userID As Integer = Session("userID")

			'Sets submenu to be displayed
			subMenu.setStartNode(menu.boundary)

			If Not IsPostBack Then
				'remove the stored session value
				Session.Remove("CODE_DestinationMunicipality")

				'Sets submenu to be displayed
				'subMenu.setStartNode(menu.boundary)

				'clear previous boundary and linear modelling for this user                
				query.CommandTimeout = 6000
				query.Connection = con

				'load active classes
				query.CommandText = "delete from liveTaxClasses where userID = " & userID.ToString
				query.ExecuteNonQuery()
				'query.CommandText = "insert into liveTaxClasses select " & userID.ToString & " as userID, taxClassID, [default] as show from taxClasses where active = 1"
				query.CommandText = "insert into liveTaxClasses select " & userID.ToString & " as userID, taxClasses.taxClassID, [default] as show from taxClasses inner join taxClassesPermission on taxClasses.taxClassID = taxClassesPermission.taxClassID where active = 1 and taxClassesPermission.levelID = " & Session("levelID").ToString & " and taxClassesPermission.access = 1"
				query.ExecuteNonQuery()

				'add an entry into liveBoundarymodel
				query.CommandText = "delete from liveBoundaryModel where userID = " & userID.ToString
				query.ExecuteNonQuery()
				query.CommandText = "insert into liveBoundaryModel values(" & userID.ToString & ",1)"
				query.ExecuteNonQuery()

				If Not IsNothing(Session("MapboundaryGroupId")) Then
					Dim dr As SqlClient.SqlDataReader
					query.CommandText = "select * from mapBoundaryTransfers where userID=" & userID.ToString & " and groupid = " & Session("MapboundaryGroupId")
					dr = query.ExecuteReader
					If dr.Read() Then
						Session.Add("map", 1)
					Else
						Session.Add("map", 0)
					End If
					dr.Close()
				Else
					Session.Add("map", 0)
				End If

				If Session("map") = 0 Then
					query.CommandText = "delete from boundarytransfers where boundaryGroupID in (select boundaryGroupID from boundaryGroups where userid = " & userID.ToString & ")"
					query.ExecuteNonQuery()
					query.CommandText = "delete from boundaryLinearTransfers where boundaryGroupID in (select boundaryGroupID from boundaryGroups where userid = " & userID.ToString & ")"
					query.ExecuteNonQuery()
					query.CommandText = "delete boundaryGroups where userid = " & userID.ToString
					query.ExecuteNonQuery()
				End If


				Dim da As New SqlClient.SqlDataAdapter
				Dim dt As New DataTable
				'populate the municipality drop down lists
				Dim constr As String
				constr = "select ' ' as number, '--Municipality--' as jurisdiction from jurisdictionTypes where jurisdictiontypeid = 1 union select number, jurisdiction from entities where jurisdictionTypeid > 1 order by Jurisdiction"
				da.SelectCommand = New SqlClient.SqlCommand(constr, con)

				da.Fill(dt)
				ddlSubjMun.DataSource = dt
				ddlSubjMun.DataValueField = "number"
				ddlSubjMun.DataTextField = "jurisdiction"
				ddlSubjMun.DataBind()
				If Session("map") = 1 Then
					ddlSubjMun.SelectedValue = Session("MapBoundaryChangeSubject")
				End If

				ddlOriginMun.DataSource = dt
				ddlOriginMun.DataValueField = "number"
				ddlOriginMun.DataTextField = "jurisdiction"
				ddlOriginMun.DataBind()
				If Session("map") = 1 Then
					ddlOriginMun.SelectedValue = Session("MapBoundaryChangeSource")
				End If

				ddlDestinationMun.DataSource = dt
				ddlDestinationMun.DataValueField = "number"
				ddlDestinationMun.DataTextField = "jurisdiction"
				ddlDestinationMun.DataBind()
				If Session("map") = 1 Then
					ddlDestinationMun.SelectedValue = Session("MapBoundaryChangeDestination")
				End If

				If Session("map") = 1 Then
					If Session("MapRestructuredLevy") Then
						rblRestructuredLevy.Items(0).Selected = True
					Else
						rblRestructuredLevy.Items(1).Selected = True
					End If
				Else
					rblRestructuredLevy.Items(0).Selected = True
				End If

				If Session("map") = 1 Then
					txtGroupName.Text = Session("MapGroupName").ToString
				Else
					txtGroupName.Text = ""
				End If


				If Session("map") = 1 Then

					transferParcels(userID, Session("MapboundaryGroupId"), Session("map"))

					updateBoundaryGroup(Session("MapboundaryGroupId"), userID, "load")

					updateLinearPropertyAdjustment(Session("MapboundaryGroupId"), userID, Session("assessmentID"))

					populateAssessmentTaxTable(userID)

					populateDuplicateGrd(userID, Session("assessmentID"), Session("map"))

					txtParcelNo.Text = ""

					'to save code for destination municipality	
					If IsNothing(Session("CODE_DestinationMunicipality")) Then
						Session.Add("CODE_DestinationMunicipality", Trim(ddlDestinationMun.SelectedValue))
					Else
						Session("CODE_DestinationMunicipality") = Trim(ddlDestinationMun.SelectedValue)
					End If
					'initSessionVariables according to map	20-sep-2013
					common.BOUNDARYCHANGE_InitRequiredSessionVars()

					Session.Remove("MapBoundaryChangeSource")
					Session.Remove("MapBoundaryChangeSubject")
					Session.Remove("MapBoundaryChangeDestination")
					Session.Remove("MapboundaryGroupId")
					Session.Remove("MapGroupName")
					Session.Remove("MapRestructuredLevy")

				End If

			Else

				query.Connection = con
				query.CommandText = "select userID from boundaryGroups where userID = " & userID.ToString
				Dim dr As SqlClient.SqlDataReader
				dr = query.ExecuteReader
				If dr.Read Then
					ddlSubjMun.Enabled = False
				Else
					ddlSubjMun.Enabled = True
				End If
				dr.Close()
			End If

			con.Close()
		Catch
			'retrieves error message
			Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
		End Try
	End Sub

	Protected Sub btnTransfer_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnTransfer.Click
		Try


			'---------------------------------
			'VALIDATE THE INPUT FIELDS
			'---------------------------------


			'make sure a municipalites are selected for subject, origin and destination: Inky's Code
			If (ddlSubjMun.Items(0).Selected) Or (ddlDestinationMun.Items(0).Selected) Or (ddlOriginMun.Items(0).Selected) Then
				Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP190")
				Exit Sub
			Else
				'disable the subject mun drop down list
				ddlSubjMun.Enabled = False
			End If

			'make sure the subject municipality is either the origin or destination
			If Not ((ddlSubjMun.SelectedValue = ddlOriginMun.SelectedValue) Or (ddlSubjMun.SelectedValue = ddlDestinationMun.SelectedValue)) Then
				Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP66")
				Exit Sub
			End If
			'make sure the origin municipality does not equal destination
			If ddlOriginMun.SelectedValue = ddlDestinationMun.SelectedValue Then
				Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP68")
				Exit Sub
			End If
			'make sure that the group name has been filled out
			If Trim(txtGroupName.Text.ToUpper) = "SUBJECT" Then
				Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP64")
				Exit Sub
			End If
			'make sure that the group name has been filled out
			If String.IsNullOrEmpty(txtGroupName.Text) Then
				Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP54")
				Exit Sub
			End If

			txtGroupName.Text = Trim(txtGroupName.Text.Replace("'", "''"))

			'make sure that the group name has no special characters
			If Not common.ValidateNoSpecialChar(txtGroupName.Text) Then
				Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP84")
				Exit Sub
			End If

			txtParcelNo.Text = Trim(txtParcelNo.Text)

			'make sure that a parcel has been entered if ALL
			If chkSelectAll.Checked = False Then
				If String.IsNullOrEmpty(txtParcelNo.Text) Then
					Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP55")
					Exit Sub
				End If
			End If

			'get userid
			Dim userID As Integer = Session("userID")

			'---------------------------------
			'END - VALIDATE THE INPUT FIELDS
			'---------------------------------

			'setup database connection
			Dim con As New SqlClient.SqlConnection
			con.ConnectionString = PATMAP.Global_asax.connString
			con.Open()

			'command obj
			Dim query As New SqlClient.SqlCommand
			query.Connection = con
			query.CommandTimeout = 6000

			'data reader obj
			Dim dr As SqlClient.SqlDataReader

			'check if entered parcel exists
			If Session("map") = 0 Then
				If chkSelectAll.Checked = False Then
					query.CommandText = "select alternate_parcelID from assessment where municipalityID = '" & ddlOriginMun.SelectedValue & "' and assessmentID = ( select assessmentID from boundaryModel where status = 1) and alternate_parcelID like '" & txtParcelNo.Text & "%'"
				Else
					query.CommandText = "select alternate_parcelID from assessment where municipalityID = '" & ddlOriginMun.SelectedValue & "' and assessmentID = ( select assessmentID from boundaryModel where status = 1) "
				End If
				dr = query.ExecuteReader
				If Not dr.Read Then
					Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP56")
					dr.Close()
					Exit Sub
				End If
				dr.Close()
			End If

			'---------------------------------
			'VALIDATE THE EXISTING GROUP            
			'---------------------------------

			'check if entered group exists
			Dim boundaryGroupID As Integer = 0
			query.CommandText = "select ltrim(rtrim(SubjectMunicipalityID)), ltrim(rtrim(OriginMunicipalityID)), ltrim(rtrim(DestinationMunicipalityID)), restructuredLevyCombined, boundaryGroupID from boundaryGroups where userid = " & userID.ToString & " and boundaryGroupName = '" & txtGroupName.Text & "'"
			dr = query.ExecuteReader
			If dr.Read Then
				'group already exists, check if parameters are the same
				'if the subject municipality is the same
				If ddlSubjMun.SelectedValue <> dr.GetValue(0) Then
					Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP58")
					dr.Close()
					Exit Sub
				End If
				'if the origin municipality is the same
				If ddlOriginMun.SelectedValue <> dr.GetValue(1) Then
					Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP59")
					dr.Close()
					Exit Sub
				End If
				'if the destination municipality is the same
				If ddlDestinationMun.SelectedValue <> dr.GetValue(2) Then
					Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP60")
					dr.Close()
					Exit Sub
				End If
				'if the restructured levy type (combine/separate) is the same
				If rblRestructuredLevy.Items(0).Selected <> dr.GetValue(3) Then
					Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP61")
					dr.Close()
					Exit Sub
				End If

				boundaryGroupID = dr.GetValue(4)
			End If
			dr.Close()

			'---------------------------------
			'END - VALIDATE THE EXISTING GROUP
			'---------------------------------

			'get the id's associated with the active boundary model
			Dim assessmentID As Integer
			query.CommandText = "select assessmentID, millRateSurveyID, POVID, (select formula from dbo.[functions] where functionID = 94) as taxFormula, (select formula from dbo.[functions] where functionID = 91) as assessmentFormula from boundaryModel where status = 1"
			dr = query.ExecuteReader
			dr.Read()
			assessmentID = dr.GetValue(0)
			Session.Add("assessmentID", assessmentID)
			dr.Close()

			con.Close()

			createSubjectGroup(userID, boundaryGroupID, 0)

			transferParcels(userID, boundaryGroupID, 0)

			updateBoundaryGroup(boundaryGroupID, userID, "load")

			If Not chkSelectAll.Checked Then
				updateLinearPropertyAdjustment(boundaryGroupID, userID, assessmentID)
			End If

			populateAssessmentTaxTable(userID)

			populateDuplicateGrd(userID, assessmentID, 0)

			txtParcelNo.Text = ""

			'to save code for destination municipality	
			If IsNothing(Session("CODE_DestinationMunicipality")) Then
				Session.Add("CODE_DestinationMunicipality", Trim(ddlDestinationMun.SelectedValue))
			Else
				Session("CODE_DestinationMunicipality") = Trim(ddlDestinationMun.SelectedValue)
			End If
			'initSessionVariables according to map	20-sep-2013
			common.BOUNDARYCHANGE_InitRequiredSessionVars()

		Catch
			'retrieves error message
			Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
		End Try
	End Sub
    Private Sub transferParcels(ByVal userID As Integer, ByRef boundaryGroupId As Integer, ByVal map As Boolean)
        'con obj
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString
        con.Open()

        'command obj
        Dim query As New SqlClient.SqlCommand
        query.Connection = con
        query.CommandTimeout = 6000

        'data reader obj
        Dim dr As SqlClient.SqlDataReader

        'variables to hold ID's
        Dim assessmentID As Integer
        Dim millRateSurveyID As Integer
        Dim POVID As Integer
        Dim taxFormula As String
        Dim assessmentFormula As String
        Dim taxableAssessmentFormula As String
        Dim FGILAssessmentFormula As String
        Dim PGILAssessmentFormula As String
        Dim taxableTaxFormula As String
        Dim FGILTaxFormula As String
        Dim PGILTaxFormula As String

        query.CommandText = "select assessmentID, millRateSurveyID, POVID, "
        query.CommandText += "(select formula from dbo.[functions] where functionID = 94) as taxFormula, "
        query.CommandText += "(select formula from dbo.[functions] where functionID = 91) as assessmentFormula, "
        query.CommandText += "(select formula from dbo.[functions] where functionID = 113) as assessmentFormula, "
        query.CommandText += "(select formula from dbo.[functions] where functionID = 118) as assessmentFormula, "
        query.CommandText += "(select formula from dbo.[functions] where functionID = 116) as assessmentFormula, "
        query.CommandText += "(select formula from dbo.[functions] where functionID = 115) as assessmentFormula, "
        query.CommandText += "(select formula from dbo.[functions] where functionID = 119) as assessmentFormula, "
        query.CommandText += "(select formula from dbo.[functions] where functionID = 117) as assessmentFormula "
        query.CommandText += "from boundaryModel where status = 1"
        dr = query.ExecuteReader
        dr.Read()
        assessmentID = dr.GetValue(0)
        millRateSurveyID = dr.GetValue(1)
        POVID = dr.GetValue(2)
        If Not IsDBNull(dr.GetValue(3)) Then
            taxFormula = dr.GetValue(3)
        Else
            taxFormula = "0"
        End If
        If Not IsDBNull(dr.GetValue(4)) Then
            assessmentFormula = dr.GetValue(4)
        Else
            assessmentFormula = "0"
        End If

        FGILAssessmentFormula = dr.GetValue(5)
        PGILAssessmentFormula = dr.GetValue(6)
        taxableAssessmentFormula = dr.GetValue(7)
        FGILTaxFormula = dr.GetValue(8)
        PGILTaxFormula = dr.GetValue(9)
        taxableTaxFormula = dr.GetValue(10)

        dr.Close()

        '-----------------------------------------------
        'Add proporties to list of transfered properties
        'This also exclude the duplicate properties (if any)
        '-----------------------------------------------
        query.CommandText = "  insert into boundarytransfers "
        If ddlSubjMun.SelectedValue = ddlOriginMun.SelectedValue Then
            query.CommandText += " select " & boundaryGroupId.ToString & ", alternate_parcelID, -1 * " & assessmentFormula & " , -1 * " & taxFormula.Replace("uniformMunicipalMillRate", "bu1.uniformMunicipalMillRate") & ", -1 * " & taxFormula.Replace("uniformMunicipalMillRate", "bu2.uniformMunicipalMillRate")
            query.CommandText += ", -1 * " & FGILAssessmentFormula & ", -1 * " & PGILAssessmentFormula & ", -1 * " & taxableAssessmentFormula
            query.CommandText += ", -1 * " & FGILTaxFormula.Replace("uniformMunicipalMillRate", "bu1.uniformMunicipalMillRate") & ", -1 * " & PGILTaxFormula.Replace("uniformMunicipalMillRate", "bu1.uniformMunicipalMillRate")
            query.CommandText += ", -1 * " & taxableTaxFormula.Replace("uniformMunicipalMillRate", "bu1.uniformMunicipalMillRate") & ", -1 * " & FGILTaxFormula.Replace("uniformMunicipalMillRate", "bu2.uniformMunicipalMillRate")
            query.CommandText += ", -1 * " & PGILTaxFormula.Replace("uniformMunicipalMillRate", "bu2.uniformMunicipalMillRate") & ", -1 * " & taxableTaxFormula.Replace("uniformMunicipalMillRate", "bu2.uniformMunicipalMillRate")
        Else
            query.CommandText += " select " & boundaryGroupId.ToString & ", alternate_parcelID, " & assessmentFormula & " , " & taxFormula.Replace("uniformMunicipalMillRate", "bu1.uniformMunicipalMillRate") & ", " & taxFormula.Replace("uniformMunicipalMillRate", "bu2.uniformMunicipalMillRate")
            query.CommandText += ", " & FGILAssessmentFormula & ", " & PGILAssessmentFormula & ", " & taxableAssessmentFormula
            query.CommandText += ", " & FGILTaxFormula.Replace("uniformMunicipalMillRate", "bu1.uniformMunicipalMillRate") & ", " & PGILTaxFormula.Replace("uniformMunicipalMillRate", "bu1.uniformMunicipalMillRate")
            query.CommandText += ", " & taxableTaxFormula.Replace("uniformMunicipalMillRate", "bu1.uniformMunicipalMillRate") & ", " & FGILTaxFormula.Replace("uniformMunicipalMillRate", "bu2.uniformMunicipalMillRate")
            query.CommandText += ", " & PGILTaxFormula.Replace("uniformMunicipalMillRate", "bu2.uniformMunicipalMillRate") & ", " & taxableTaxFormula.Replace("uniformMunicipalMillRate", "bu2.uniformMunicipalMillRate")
        End If
        query.CommandText += " from assessment"
        query.CommandText += "       inner join pov on assessment.taxclassID = pov.taxclassID"
        query.CommandText += "       inner join boundaryUniformMunicipalMillRate bu1 on rtrim(ltrim(bu1.municipalityID)) = rtrim(ltrim('" & ddlSubjMun.SelectedValue & "'))"
        query.CommandText += "       inner join boundaryUniformMunicipalMillRate bu2 on rtrim(ltrim(bu2.municipalityID)) = rtrim(ltrim('" & ddlOriginMun.SelectedValue & "'))"
        query.CommandText += " where assessmentID = " & assessmentID & " and povID = " & POVID & " and ltrim(rtrim(assessment.municipalityID)) = ltrim(rtrim('" & ddlOriginMun.SelectedValue & "')) and alternate_parcelID "
        If Session("map") = 0 Then
            query.CommandText += " like '" & txtParcelNo.Text & "%'"
        Else
            query.CommandText += " in (select alternate_parcelID from mapBoundaryTransfers where userID = " & userID & " and groupid = " & Session("MapboundaryGroupId").ToString & ")"
        End If
        query.CommandText += "and alternate_parcelID not in (select alternate_parcelID from boundaryGroups bg inner join boundaryTransfers bt on bg.boundaryGroupID = bt.boundaryGroupID  where bg.OriginMunicipalityID = '" & ddlOriginMun.SelectedValue & "' and bg.userID = " & userID & " and bg.boundaryGroupName not in ('Subject'))"
        query.CommandText += " group by alternate_parcelID"

        query.ExecuteNonQuery()

        'update the data satle flag to true
        query.CommandText = "update liveBoundaryModel set mapDataStale=1 where userid=" & userID.ToString
        query.ExecuteNonQuery()

        con.Close()
    End Sub

    Private Sub createSubjectGroup(ByVal userID As Integer, ByRef boundaryGroupId As Integer, ByVal map As Boolean)
        'con obj
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString
        con.Open()

        'command obj
        Dim query As New SqlClient.SqlCommand
        query.Connection = con
        query.CommandTimeout = 60000

        'data reader obj
        Dim dr As SqlClient.SqlDataReader

        'variables to hold ID's
        Dim assessmentID As Integer
        Dim millRateSurveyID As Integer
        Dim POVID As Integer
        Dim taxFormula As String
        Dim assessmentFormula As String

        query.CommandText = "select assessmentID, millRateSurveyID, POVID, (select formula from dbo.[functions] where functionID = 94) as taxFormula, (select formula from dbo.[functions] where functionID = 91) as assessmentFormula from boundaryModel where status = 1"
        dr = query.ExecuteReader
        dr.Read()
        assessmentID = dr.GetValue(0)
        millRateSurveyID = dr.GetValue(1)
        POVID = dr.GetValue(2)
        If Not IsDBNull(dr.GetValue(3)) Then
            taxFormula = dr.GetValue(3)
        Else
            taxFormula = "0"
        End If
        If Not IsDBNull(dr.GetValue(4)) Then
            assessmentFormula = dr.GetValue(4)
        Else
            assessmentFormula = "0"
        End If
        dr.Close()

        'check if this is the first group entered
        query.CommandText = "select userID from boundaryGroups where userid = " & userID.ToString
        dr = query.ExecuteReader()
        If Not dr.Read() Then
            dr.Close()

            '-------------------
            'CREATE THE SUBJECT WITH ORIGINAL assessment(w), levy(s), millrate
            '-------------------

            'get the original assessment(w) and the levy for subject municipality (using subject numicipality millrate)
            query.CommandText = "  select " & assessmentFormula & " as marketValue, " & taxFormula.Replace("uniformMunicipalMillRate", "bu1.uniformMunicipalMillRate") & " as originalLevy"
            query.CommandText += " from assessment"
            query.CommandText += "       inner join pov on assessment.taxclassID = pov.taxclassID "
            query.CommandText += "       inner join boundaryUniformMunicipalMillRate bu1 on rtrim(ltrim(bu1.municipalityID)) = rtrim(ltrim('" & ddlSubjMun.SelectedValue & "'))"
            query.CommandText += " where assessmentID = " & assessmentID & " and povID = " & POVID & " and ltrim(rtrim(assessment.municipalityID)) = rtrim(ltrim('" & ddlSubjMun.SelectedValue & "'))"
            dr = query.ExecuteReader()
            dr.Read()

            Dim marketValue As Double = dr.GetValue(0)
            Session.Add("subjectOriginalDrivedAssessment", marketValue)
            Dim originalLevy As Double = dr.GetValue(1)
            Session.Add("subjectOriginalLevy", originalLevy)
            dr.Close()

            Dim formula As Integer
            If originalLevy = 0 Or marketValue = 0 Then
                formula = 0
            Else
                formula = originalLevy / (marketValue / 1000)
            End If

            'create subject municipality
            'for subject municipality the origin levy is same as the original levy (there is no two diferent millrates to calc the levy for the subject alone)
            query.CommandText = "insert into boundaryGroups (userID, boundaryGroupName, SubjectMunicipalityID, OriginMunicipalityID, DestinationMunicipalityID, restructuredLevyCombined, assessment, originalLevy, restructuredLevy,uniformMillRate,originLevy) VALUES (" & userID.ToString & ",'Subject','" & ddlSubjMun.SelectedValue & "','" & ddlOriginMun.SelectedValue & "','" & ddlDestinationMun.SelectedValue & "',1," & marketValue & "," & originalLevy & " ," & originalLevy & "," & formula & "," & originalLevy & ")"
            query.ExecuteNonQuery()
        Else
            dr.Close()
        End If

        'check if this is a new boundary group
        If boundaryGroupId = 0 Then

            '-------------------
            'CREATE THE GROUP WITHOUT assessment(w), lvey(s), millrate
            '-------------------

            If rblRestructuredLevy.Items(0).Selected Then
                query.CommandText = "insert into boundaryGroups (userID, boundaryGroupName, SubjectMunicipalityID, OriginMunicipalityID, DestinationMunicipalityID, restructuredLevyCombined) VALUES (" & userID.ToString & ",'" & txtGroupName.Text & "','" & ddlSubjMun.SelectedValue & "','" & ddlOriginMun.SelectedValue & "','" & ddlDestinationMun.SelectedValue & "',1) select @@IDENTITY"
            Else
                query.CommandText = "insert into boundaryGroups (userID, boundaryGroupName, SubjectMunicipalityID, OriginMunicipalityID, DestinationMunicipalityID, restructuredLevyCombined) VALUES (" & userID.ToString & ",'" & txtGroupName.Text & "','" & ddlSubjMun.SelectedValue & "','" & ddlOriginMun.SelectedValue & "','" & ddlDestinationMun.SelectedValue & "',0) select @@IDENTITY"
            End If
            dr = query.ExecuteReader()
            dr.Read()
            boundaryGroupId = dr.GetValue(0)
            Session("MapboundaryGroupId") = dr.GetValue(0)

            dr.Close()
        End If
        con.Close()
    End Sub
    Private Sub populateDuplicateGrd(ByVal userID As Integer, ByVal assessmentID As Integer, ByVal map As Boolean)
        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString
        con.Open()

        Dim da As New SqlClient.SqlDataAdapter
        da.SelectCommand = New SqlClient.SqlCommand
        da.SelectCommand.Connection = con
        da.SelectCommand.CommandTimeout = 10000
        da.SelectCommand.CommandText = "select bg.boundaryGroupID, bg.boundaryGroupName, bt.assessment, alternate_parcelID"
        da.SelectCommand.CommandText += " from boundaryGroups bg inner join boundaryTransfers bt on bg.boundaryGroupID = bt.boundaryGroupID"
        da.SelectCommand.CommandText += " where bg.userID = " & userID & " and bg.boundaryGroupName not in ('Subject','" & txtGroupName.Text & "') and OriginMunicipalityID + cast(alternate_parcelID as varchar(8000)) in"
        da.SelectCommand.CommandText += " ("
        da.SelectCommand.CommandText += " select municipalityID + cast(alternate_parcelID as varchar(8000))"
        da.SelectCommand.CommandText += " from assessment"
        da.SelectCommand.CommandText += " where assessmentID = " & assessmentID & " and alternate_parcelID"
        If Session("map") = 0 Then
            da.SelectCommand.CommandText += " like '" & txtParcelNo.Text & "%'"
        Else
            da.SelectCommand.CommandText += " in (select alternate_parcelID from mapBoundaryTransfers where userID = " & userID & " and groupid = " & Session("MapboundaryGroupId").ToString & ")"
        End If
        da.SelectCommand.CommandText += "and municipalityID = '" & ddlOriginMun.SelectedValue & "')"


        Dim dt As New DataTable
        da.Fill(dt)

        grdDuplicateProp.DataSource = dt
        grdDuplicateProp.DataBind()

        Session("dupProperties") = dt

        'if there are overlap display the grid with the duplicates
        'Dim Master As New PATMAP.MasterPage
        If dt.Rows.Count > 0 Then
            grdDuplicateProp.Visible = True
            btnPrintDpl.Visible = True
            Master.errorMsg = common.GetErrorMessage("PATMAP321")
        Else
            'otherwise hide the grid
            btnPrintDpl.Visible = False
            grdDuplicateProp.Visible = False
            Master.errorMsg = ""
        End If
        con.Close()

    End Sub


    Private Sub updateLinearPropertyAdjustment(ByRef boundaryGroupID As Integer, ByRef userID As Integer, ByRef assessmentID As Integer)

        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString
        con.Open()
        Dim query As New SqlClient.SqlCommand
        query.Connection = con
        query.CommandTimeout = 6000


        Dim linearIncludedInBoundaryAssessment As Decimal = 0
        Dim linearIncludedInBoundaryLevy As Decimal = 0
        Dim originIncludedInBoundaryLevy As Decimal = 0

        'how much of the assessment is already lost or gained for pipeline and railway
        query.CommandText = " Select isnull(sum(bt.assessment),0), isnull(sum(bt.levy),0), isnull(sum(bt.originLevy),0)" & vbCrLf & _
                            " from boundaryGroups bg inner join boundaryTransfers bt on bg.boundaryGroupID = bt.boundaryGroupID" & vbCrLf & _
                            " inner join" & vbCrLf & _
                            " (" & vbCrLf & _
                            " select municipalityID, alternate_parcelID " & vbCrLf & _
                            " from assessment" & vbCrLf & _
                            " where assessmentID = " & assessmentID & " and municipalityID=CONVERT(CHAR,(select OriginMunicipalityID from boundaryGroups where boundaryGroupID = " & boundaryGroupID & ")) and  assessment.taxClassID in ('P','PP')" & vbCrLf & _
                            " ) temp on temp.municipalityID = bg.OriginMunicipalityID and temp.alternate_parcelID = bt.alternate_parcelID" & vbCrLf & _
                            " where(bg.boundaryGroupID = " & boundaryGroupID & " and userID = " & userID & ")"
        Dim dr As SqlClient.SqlDataReader = query.ExecuteReader()
        dr.Read()

        linearIncludedInBoundaryAssessment = dr.GetValue(0)
        linearIncludedInBoundaryLevy = dr.GetValue(1)
        originIncludedInBoundaryLevy = dr.GetValue(2)

        'calulate the linear assessement to be transfered based on the percentage
        dr.Close()
        Dim linearAssessment As Decimal = 0
        Dim linearLevy As Decimal = 0
        Dim originLevy As Decimal = 0
        query.CommandText = "select isnull(sum(taxableAssessmentValue * percentageTransfer),0) from boundaryLinearTransfers where boundaryGroupID = " & boundaryGroupID & " and taxClassID in ('P','PP')"
        dr = query.ExecuteReader()
        If dr.Read() Then

            linearAssessment = dr.GetValue(0)
            dr.Close()
            query.CommandText = "select uniformMunicipalMillRate * " & (linearAssessment / 1000) & " from boundaryGroups bg inner join boundaryUniformMunicipalMillRate bu on bg.SubjectMunicipalityID=bu.municipalityID where bg.boundaryGroupName = 'Subject' and bg.userID = " & userID
            dr = query.ExecuteReader()
            If dr.Read() Then
                linearLevy = dr.GetValue(0)
            Else
                linearLevy = 0
            End If

            dr.Close()
            query.CommandText = "select uniformMunicipalMillRate * " & (linearAssessment / 1000) & " from boundaryGroups bg inner join boundaryUniformMunicipalMillRate bu on bg.OriginMunicipalityID=bu.municipalityID where bg.boundaryGroupName = 'Subject' and bg.userID = " & userID
            dr = query.ExecuteReader()
            If dr.Read() Then
                originLevy = dr.GetValue(0)
            Else
                originLevy = 0
            End If

            'adjust the assessment and the groupLevy for the group
            dr.Close()
            query.CommandText = "update boundaryGroups set assessment = ((select assessment from boundaryGroups where boundaryGroupID = " & boundaryGroupID & ") - (" & linearIncludedInBoundaryAssessment & ")) + " & linearAssessment & vbCrLf & _
                                ", originalLevy = ((select originalLevy from boundaryGroups where boundaryGroupID = " & boundaryGroupID & ") - (" & linearIncludedInBoundaryLevy & ")) + " & linearLevy & vbCrLf & _
                                ", originLevy = ((select originLevy from boundaryGroups where boundaryGroupID = " & boundaryGroupID & ") - (" & originIncludedInBoundaryLevy & ")) + " & originLevy & vbCrLf & _
                                " where boundaryGroupName <> 'Subject' and boundaryGroupID = " & boundaryGroupID
            query.ExecuteNonQuery()

            If ddlOriginMun.SelectedValue <> ddlSubjMun.SelectedValue Then
                'GAIN
                'combine - adjust the subject
                If rblRestructuredLevy.SelectedIndex = 0 Then
                    query.CommandText = "update boundaryGroups set restructuredLevy = ((select restructuredLevy from boundaryGroups where boundaryGroupName = 'Subject' and userID = " & userID & ") - " & linearIncludedInBoundaryLevy & ") + " & linearLevy & " where userid = " & userID.ToString & " and boundaryGroupName = 'Subject'"
                    query.ExecuteNonQuery()

                    query.CommandText = "  update boundaryGroups "
                    query.CommandText += " set uniformMillRate = restructuredLevy/((select sum(assessment) from boundaryGroups where restructuredLevyCombined = 1 and userID = " & userID.ToString & ")/1000)"
                    query.CommandText += " where userID = " & userID.ToString & " and boundaryGroupName = 'Subject'"
                    query.ExecuteNonQuery()

                Else
                    'separate - adjust the group
                    query.CommandText = "update boundaryGroups set restructuredLevy = ((select restructuredLevy from boundaryGroups where boundaryGroupID = " & boundaryGroupID & ") - " & linearIncludedInBoundaryLevy & ") + " & linearLevy & " where boundaryGroupName <> 'Subject' and boundaryGroupID = " & boundaryGroupID.ToString
                    query.ExecuteNonQuery()

                    query.CommandText = "  update boundaryGroups "
                    query.CommandText += " set uniformMillRate = case when assessment = 0 then 0 else restructuredLevy/(assessment/1000) end"
                    query.CommandText += " where boundaryGroupName <> 'Subject' and boundaryGroupID = " & boundaryGroupID.ToString
                    query.ExecuteNonQuery()
                End If
            Else
                'LOST
                'adjust the subject
                query.CommandText = "update boundaryGroups set assessment = ((select assessment from boundaryGroups where boundaryGroupName = 'Subject' and userID = " & userID & ") - (" & linearIncludedInBoundaryAssessment & ")) + " & linearAssessment
                query.CommandText += ", originalLevy = ((select originalLevy from boundaryGroups where boundaryGroupName = 'Subject' and userID = " & userID & ") - (" & linearIncludedInBoundaryLevy & ")) + " & linearLevy
                query.CommandText += ", originLevy = ((select originalLevy from boundaryGroups where boundaryGroupName = 'Subject' and userID = " & userID & ") - (" & linearIncludedInBoundaryLevy & ")) + " & linearLevy
                query.CommandText += ", restructuredLevy = ((select restructuredLevy from boundaryGroups where boundaryGroupName = 'Subject' and userID = " & userID & ") - (" & linearIncludedInBoundaryLevy & ")) + " & linearLevy
                query.CommandText += " where userid = " & userID.ToString & " and boundaryGroupName = 'Subject'"
                query.ExecuteNonQuery()

                query.CommandText = "  update boundaryGroups "
                query.CommandText += " set uniformMillRate = restructuredLevy/((select sum(assessment) from boundaryGroups where restructuredLevyCombined = 1 and userID = " & userID.ToString & ")/1000)"
                query.CommandText += " where userID = " & userID.ToString & " and boundaryGroupName = 'Subject'"
                query.ExecuteNonQuery()

            End If


            'update the boundaryTransfer table
            query.CommandText = " delete from boundaryTransfers" & vbCrLf & _
                                " where alternate_parcelID in " & vbCrLf & _
                                " (" & vbCrLf & _
                                " select alternate_parcelID " & vbCrLf & _
                                " from assessment" & vbCrLf & _
                                " where assessmentID = " & assessmentID & " and municipalityID=CONVERT(CHAR,(select OriginMunicipalityID from boundaryGroups where boundaryGroupID = " & boundaryGroupID & ")) and  assessment.taxClassID in ('P', 'PP')" & vbCrLf & _
                                " ) and boundaryGroupID=" & boundaryGroupID.ToString
            query.ExecuteNonQuery()

            query.CommandText = "insert into boundaryTransfers "
            query.CommandText += " select blt.boundaryGroupID, blt.alternate_parcelID, (blt.taxableAssessmentValue * blt.percentageTransfer), ((blt.taxableAssessmentValue * blt.percentageTransfer) / 1000) * bu1.uniformMunicipalMillRate, ((blt.taxableAssessmentValue * blt.percentageTransfer) / 1000) * bu2.uniformMunicipalMillRate"
            query.CommandText += ",(blt.FGILAssessmentValue * blt.percentageTransfer),(blt.PGILAssessmentValue * blt.percentageTransfer),(blt.taxableAssessmentValue * blt.percentageTransfer)"
            query.CommandText += ",((blt.FGILAssessmentValue * blt.percentageTransfer) / 1000) * bu1.uniformMunicipalMillRate,((blt.PGILAssessmentValue * blt.percentageTransfer) / 1000) * bu1.uniformMunicipalMillRate,((blt.taxableAssessmentValue * blt.percentageTransfer) / 1000) * bu1.uniformMunicipalMillRate"
            query.CommandText += ",((blt.FGILAssessmentValue * blt.percentageTransfer) / 1000) * bu2.uniformMunicipalMillRate,((blt.PGILAssessmentValue * blt.percentageTransfer) / 1000) * bu2.uniformMunicipalMillRate,((blt.taxableAssessmentValue * blt.percentageTransfer) / 1000) * bu1.uniformMunicipalMillRate"
            query.CommandText += " from boundaryLinearTransfers blt"
            query.CommandText += " inner join boundaryGroups bg on bg.boundaryGroupID = blt.boundaryGroupID and blt.percentageTransfer <> 0 and blt.boundaryGroupID = " & boundaryGroupID
            query.CommandText += " inner join boundaryUniformMunicipalMillRate bu1 on bu1.municipalityID = bg.SubjectMunicipalityID"
            query.CommandText += " inner join boundaryUniformMunicipalMillRate bu2 on bu2.municipalityID = bg.OriginMunicipalityID"
            query.ExecuteNonQuery()

        End If

        con.Close()
    End Sub

	Private Sub updateBoundaryGroup(ByRef boundaryGroupID As Integer, ByRef userID As Integer, ByRef mode As String)

		Try
			'setup database connection
			Dim con As New SqlClient.SqlConnection
			con.ConnectionString = PATMAP.Global_asax.connString
			con.Open()
			Dim query As New SqlClient.SqlCommand
			query.Connection = con
			query.CommandTimeout = 6000
			Dim dr As SqlClient.SqlDataReader


			'if loss, and the same group is being updated
			query.CommandText = "select assessment, originalLevy, restructuredLevy from boundaryGroups where userid = " & userID.ToString & " and boundaryGroupID = " & boundaryGroupID
			dr = query.ExecuteReader()
			dr.Read()
			Dim prevAssessment As Decimal = 0
			Dim prevLevy As Decimal = 0
			Dim prevRestructuredLevy As Decimal = 0
			If dr.GetValue(0) Then
				prevAssessment = (-1) * dr.GetValue(0)
				prevLevy = (-1) * dr.GetValue(1)
				prevRestructuredLevy = (-1) * dr.GetValue(2)
			End If
			dr.Close()


			'---------------------------------------------
			'UPDATE THE GROUP - 
			'assessment
			'original levy - levy using subject millrate
			'origin levy - levy using origin muncipality millrate
			'restructured levy
			'   combined - its self(which is 0)
			'   separate - sum from the Boundary Transfers table            
			'---------------------------------------------
			query.CommandText = "  update boundaryGroups "
			query.CommandText += " set assessment = isnull((select sum(assessment) from boundarytransfers where boundaryGroupID = " & boundaryGroupID.ToString & "),0.0)"
			query.CommandText += " , originalLevy = isnull((select sum(levy) from boundarytransfers where boundaryGroupID = " & boundaryGroupID.ToString & "),0.0)"
			query.CommandText += " , originLevy = isnull((select sum(originlevy) from boundarytransfers where boundaryGroupID = " & boundaryGroupID.ToString & "),0.0)"
			If mode = "load" Then
				query.CommandText += " , restructuredLevy = case when restructuredLevyCombined = 1 then isnull(restructuredLevy,0) else isnull((select sum(levy) from boundarytransfers where boundaryGroupID = " & boundaryGroupID.ToString & "),0.0) end"
			End If
			query.CommandText += " where boundaryGroupName <> 'Subject' and boundaryGroupID = " & boundaryGroupID.ToString
			query.ExecuteNonQuery()

			'---------------------------------------------
			'UPDATE THE GROUP's MILLRATE - 
			'---------------------------------------------
			query.CommandText = "  update boundaryGroups "
			query.CommandText += " set uniformMillRate = case when assessment = 0 then 0 else restructuredLevy/(assessment/1000) end"
			query.CommandText += " where boundaryGroupName <> 'Subject' and boundaryGroupID = " & boundaryGroupID.ToString
			query.ExecuteNonQuery()

			'get levy adjustment for subject municipality
			query.CommandText = "  select isnull(sum(assessmentAdjustment),0.0) as assessmentAdjustment, isnull(sum(groupLevyAdjustment),0.0) as groupLevyAdjustment, isnull(sum(restructuredLevyAdjustment),0.0) as restructuredLevyAdjustment"
			query.CommandText += " from"
			query.CommandText += " ("
			query.CommandText += "      select case when SubjectMunicipalityID = OriginMunicipalityID then assessment else 0.0 end as assessmentAdjustment, case when SubjectMunicipalityID = OriginMunicipalityID then originalLevy else 0.0 end as groupLevyAdjustment, case when restructuredLevyCombined = 0 then case when SubjectMunicipalityID = OriginMunicipalityID then originalLevy else 0.0 end else originalLevy end as restructuredLevyAdjustment"
			query.CommandText += "      from boundaryGroups temp1"
			query.CommandText += "      where boundaryGroupID in (select boundaryGroupID from boundaryGroups where userid = " & userID.ToString & " and boundaryGroupName <> 'Subject'"
			'if loss, then only the assessment and levy of the current group should be deducted (or adjusted)            
			If ddlSubjMun.SelectedValue = ddlOriginMun.SelectedValue And mode = "load" Then
				query.CommandText += " and boundaryGroupID = " & boundaryGroupID
			Else
				'if gain (combine/separate) don't include the loss into the sum
				query.CommandText += " and OriginMunicipalityID <> SubjectMunicipalityID"
			End If
			query.CommandText += " )) temp1"
			dr = query.ExecuteReader()
			dr.Read()

			'if gain (combined) all the restructuredLevyAdjustment will have values (which to be added to the restructuredLevy of the subject)
			'if gain (separate) all = 0
			'if loss all will have the values (which to be deducted from the subject assessment and the levy's)
			Dim assessmentAdjustment As Double = dr.GetValue(0)
			Dim groupLevyAdjustment As Double = dr.GetValue(1)
			Dim restructuredLevyAdjustment As Double = dr.GetValue(2)
			dr.Close()


			'IF SEPARATE (GAIN)
			If restructuredLevyAdjustment = 0 Then

				Dim assessmentID As Integer
				Dim millRateSurveyID As Integer
				Dim POVID As Integer
				Dim taxFormula As String
				Dim assessmentFormula As String

				query.CommandText = "select assessmentID, millRateSurveyID, POVID, (select formula from dbo.[functions] where functionID = 94) as taxFormula, (select formula from dbo.[functions] where functionID = 91) as assessmentFormula from boundaryModel where status = 1"
				dr = query.ExecuteReader
				dr.Read()
				assessmentID = dr.GetValue(0)
				millRateSurveyID = dr.GetValue(1)
				POVID = dr.GetValue(2)
				If Not IsDBNull(dr.GetValue(3)) Then
					taxFormula = dr.GetValue(3)
				Else
					taxFormula = "0"
				End If
				If Not IsDBNull(dr.GetValue(4)) Then
					assessmentFormula = dr.GetValue(4)
				Else
					assessmentFormula = "0"
				End If
				dr.Close()


				'-------------------
				'UPDATE THE SUBJECT WITH ORIGINAL assessment(w), levy(s), millrate
				'-------------------

				If IsNothing(Session("revisedRestLevy")) Then
					'get the original assessment(w) and the levy for subject municipality (using subject numicipality millrate)
					query.CommandText = "  select " & assessmentFormula & " as marketValue, " & taxFormula.Replace("uniformMunicipalMillRate", "bu1.uniformMunicipalMillRate") & " as originalLevy"
					query.CommandText += " from assessment"
					query.CommandText += "       inner join pov on assessment.taxclassID = pov.taxclassID "
					query.CommandText += "       inner join boundaryUniformMunicipalMillRate bu1 on rtrim(ltrim(bu1.municipalityID)) = rtrim(ltrim('" & ddlSubjMun.SelectedValue & "'))"
					query.CommandText += " where assessmentID = " & assessmentID & " and povID = " & POVID & " and ltrim(rtrim(assessment.municipalityID)) = rtrim(ltrim('" & ddlSubjMun.SelectedValue & "'))"
					dr = query.ExecuteReader()
					dr.Read()

					assessmentAdjustment = dr.GetValue(0)
					restructuredLevyAdjustment = dr.GetValue(1)
					dr.Close()

					'update the groups assessment and levy totals
					'for subject municipality the origin levy is same as the original levy (there is no two diferent millrates to calc the levy for the subject alone)
					query.CommandText = "  update boundaryGroups "
					query.CommandText += " set assessment = " & assessmentAdjustment & " ,originalLevy = " & restructuredLevyAdjustment & " ,restructuredLevy = " & restructuredLevyAdjustment & ",originLevy = " & restructuredLevyAdjustment
					query.CommandText += " where userid = " & userID.ToString & " and boundaryGroupName = 'Subject'"
					query.ExecuteNonQuery()
				Else
					'get the original assessment(w) and the levy for subject municipality (using subject numicipality millrate)
					query.CommandText = "  select " & assessmentFormula & " as marketValue, " & taxFormula.Replace("uniformMunicipalMillRate", "bu1.uniformMunicipalMillRate") & " as originalLevy"
					query.CommandText += " from assessment"
					query.CommandText += "       inner join pov on assessment.taxclassID = pov.taxclassID "
					query.CommandText += "       inner join boundaryUniformMunicipalMillRate bu1 on rtrim(ltrim(bu1.municipalityID)) = rtrim(ltrim('" & ddlSubjMun.SelectedValue & "'))"
					query.CommandText += " where assessmentID = " & assessmentID & " and povID = " & POVID & " and ltrim(rtrim(assessment.municipalityID)) = rtrim(ltrim('" & ddlSubjMun.SelectedValue & "'))"
					dr = query.ExecuteReader()
					dr.Read()

					assessmentAdjustment = dr.GetValue(0)
					restructuredLevyAdjustment = dr.GetValue(1)
					dr.Close()

					'update the groups assessment and levy totals
					'for subject municipality the origin levy is same as the original levy (there is no two diferent millrates to calc the levy for the subject alone)
					query.CommandText = "  update boundaryGroups "
					query.CommandText += " set assessment = " & assessmentAdjustment & " ,originalLevy = " & restructuredLevyAdjustment & " ,restructuredLevy = " & Session("revisedRestLevy") & ",originLevy = " & restructuredLevyAdjustment
					query.CommandText += " where userid = " & userID.ToString & " and boundaryGroupName = 'Subject'"
					query.ExecuteNonQuery()

					Session.Remove("revisedRestLevy")
				End If


			Else
				'IF LOSS 
				If assessmentAdjustment <> 0 And groupLevyAdjustment <> 0 Then

					If IsNothing(Session("revisedRestLevy")) Then
						'----------------------------------------------
						'UPDATE THE SUBJECT WITH ADJUSTED assessment(w), levy(s), millrate
						'----------------------------------------------
						If prevAssessment <> 0 Then
							query.CommandText = "  update boundaryGroups "
							query.CommandText += " set assessment = assessment + " & prevAssessment & " + " & assessmentAdjustment & " ,originalLevy = originalLevy + " & prevLevy & " + " & groupLevyAdjustment & " ,restructuredLevy = restructuredLevy + " & prevLevy & " + " & restructuredLevyAdjustment & ",originLevy = originalLevy + " & prevLevy & " + " & groupLevyAdjustment
							query.CommandText += " where userid = " & userID.ToString & " and boundaryGroupName = 'Subject'"
							query.ExecuteNonQuery()
						Else
							query.CommandText = "  update boundaryGroups "
							query.CommandText += " set assessment = assessment + " & assessmentAdjustment & " ,originalLevy = originalLevy + " & groupLevyAdjustment & " ,restructuredLevy = restructuredLevy + " & restructuredLevyAdjustment & ",originLevy = originalLevy + " & groupLevyAdjustment
							query.CommandText += " where userid = " & userID.ToString & " and boundaryGroupName = 'Subject'"
							query.ExecuteNonQuery()
						End If
					Else
						If prevAssessment <> 0 Then
							query.CommandText = "  update boundaryGroups "
							query.CommandText += " set assessment = assessment + " & prevAssessment & " + " & assessmentAdjustment & " ,originalLevy = originalLevy + " & prevLevy & " + " & groupLevyAdjustment & " ,restructuredLevy = restructuredLevy + " & prevLevy & " + " & restructuredLevyAdjustment & ",originLevy = originalLevy + " & prevLevy & " + " & groupLevyAdjustment
							query.CommandText += " where userid = " & userID.ToString & " and boundaryGroupName = 'Subject'"
							query.ExecuteNonQuery()
						Else
							query.CommandText = "  update boundaryGroups "
							query.CommandText += " set assessment = assessment + " & assessmentAdjustment & " ,originalLevy = originalLevy + " & groupLevyAdjustment & " ,restructuredLevy = restructuredLevy + " & restructuredLevyAdjustment & ",originLevy = originalLevy + " & groupLevyAdjustment
							query.CommandText += " where userid = " & userID.ToString & " and boundaryGroupName = 'Subject'"
							query.ExecuteNonQuery()
						End If
					End If
				Else
					If IsNothing(Session("revisedRestLevy")) Then
						'IF COMBINED (GAIN)
						'----------------------------------------------
						'UPDATE THE SUBJECT WITH ADJUSTED assessment(w), levy(s), millrate
						'----------------------------------------------
						query.CommandText = "  update boundaryGroups "
						query.CommandText += " set assessment = assessment + " & assessmentAdjustment & " ,originalLevy = originalLevy + " & groupLevyAdjustment & " ,restructuredLevy = originalLevy + " & restructuredLevyAdjustment & ",originLevy = originalLevy + " & groupLevyAdjustment
						query.CommandText += " where userid = " & userID.ToString & " and boundaryGroupName = 'Subject'"
						query.ExecuteNonQuery()
					Else
						query.CommandText = "  update boundaryGroups "
						query.CommandText += " set assessment = assessment + " & assessmentAdjustment & " ,originalLevy = originalLevy + " & groupLevyAdjustment & " ,restructuredLevy = " & Session("revisedRestLevy") & ",originLevy = originalLevy + " & groupLevyAdjustment
						query.CommandText += " where userid = " & userID.ToString & " and boundaryGroupName = 'Subject'"
						query.ExecuteNonQuery()

						Session.Remove("revisedRestLevy")
					End If
				End If
			End If

			'---------------------------------------------
			'UPDATE THE SUBJECT's MILLRATE - 
			'---------------------------------------------
			query.CommandText = "  update boundaryGroups "
			query.CommandText += " set uniformMillRate = case when restructuredLevy = 0 then 0 when (select sum(assessment) from boundaryGroups where restructuredLevyCombined = 1 and userID = " & userID.ToString & ") = 0 then 0 else restructuredLevy /((select sum(assessment) from boundaryGroups where restructuredLevyCombined = 1 and userID = " & userID.ToString & ")/1000) end"
			query.CommandText += " where userid = " & userID.ToString & " and boundaryGroupName = 'Subject'"
			query.ExecuteNonQuery()
			'query.ExecuteNonQuery()
			con.Close()
		Catch
			'retrieves error message
			Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
		End Try

	End Sub

	Private Sub populateTransferTable(ByVal boundaryGroupID As Integer)
		'setup database connection
		Dim con As New SqlClient.SqlConnection
		con.ConnectionString = PATMAP.Global_asax.connString
		Dim query As New SqlClient.SqlCommand
		query.Connection = con
		con.Open()

		'fill the users drop down
		Dim da As New SqlClient.SqlDataAdapter
		da.SelectCommand = New SqlClient.SqlCommand
		da.SelectCommand.Connection = con
		da.SelectCommand.CommandText = "select boundarytransferID, e1.jurisdiction originMun, alternate_parcelID, e2.jurisdiction as destinationMun, boundaryTransfers.boundaryGroupID from boundaryGroups inner join boundaryTransfers on boundaryGroups.boundaryGroupID = boundaryTransfers.boundaryGroupID inner join entities e1 on originMunicipalityID = e1.number inner join entities e2 on destinationMunicipalityID = e2.number where boundaryGroups.boundaryGroupID = " & boundaryGroupID & " order by alternate_parcelID"
		Dim dt As New DataTable
		da.Fill(dt)
		grdProperties.DataSource = dt
		grdProperties.DataBind()

		Session("transferProperties") = dt

	End Sub

    Private Sub grdProperties_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdProperties.PageIndexChanging
        Try
            Dim dt As DataTable

            'sets grid's page index
            grdProperties.PageIndex = e.NewPageIndex

            'fill the grid
            If Not IsNothing(Session("transferProperties")) Then
                dt = CType(Session("transferProperties"), DataTable)
                grdProperties.DataSource = dt
                grdProperties.DataBind()
            Else
                populateTransferTable(grdProperties.DataKeys(e.NewPageIndex).Values("boundaryGroupID"))
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

	Private Sub grdProperties_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdProperties.RowCommand
		Try
			'If grid is not being sorted
			If LCase(e.CommandName) <> "sort" Then

				'check what type of command has been fired by the grid
				Select Case e.CommandName
					Case "deleteTransfer"

						'get selected row index and corresponding userID to that row
						Dim index As Integer = Convert.ToInt32(e.CommandArgument)
						Dim boundarytransferID As Integer = grdProperties.DataKeys(index).Values("boundarytransferID")
						Dim boundaryGroupID As Integer = grdProperties.DataKeys(index).Values("boundaryGroupID")

						'setup database connection
						Dim con As New SqlClient.SqlConnection
						con.ConnectionString = PATMAP.Global_asax.connString
						Dim query As New SqlClient.SqlCommand
						query.Connection = con

						'get userid
						Dim userID As Integer = Session("userID")

						'delete specified transfer
						con.Open()
						query.CommandText = "delete from boundarytransfers where boundarytransferID = " & boundarytransferID
						query.ExecuteNonQuery()
						con.Close()

						Dim currentRow As Integer
						currentRow = grdAssessment.EditIndex
						Dim eArgs As New System.Web.UI.WebControls.GridViewEditEventArgs(currentRow)

						'update groups grid
						updateBoundaryGroup(boundaryGroupID, userID, "load")
						populateAssessmentTaxTable(userID)
						grdAssessment_RowEditing(grdAssessment, eArgs)

				End Select
			End If
		Catch
			'retrieves error message
			Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
		End Try
	End Sub

    Private Sub populateAssessmentTaxTable(ByRef userID As Integer)
        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString
        con.Open()

        'fill the users drop down
        Dim da As New SqlClient.SqlDataAdapter
        da.SelectCommand = New SqlClient.SqlCommand
        da.SelectCommand.Connection = con
        da.SelectCommand.CommandText = "select rtrim(ltrim(originMunicipalityID)) as originMunicipalityID, rtrim(ltrim(DestinationMunicipalityID)) as DestinationMunicipalityID, boundaryGroupID, boundaryGroupName, assessment, round(originalLevy,2) as originalLevy, case when boundaryGroupName = 'Subject' then 'Subject' when subjectMunicipalityID = originMunicipalityID then 'Lost' when restructuredLevyCombined = 1 then 'Combined' else 'Separate' end as LevyStatus, round(restructuredLevy,2) as restructuredLevy, originLevy, uniformMillRate from boundaryGroups where userid = " & userID & " order by boundaryGroupID"
        Dim dt As New DataTable
        da.Fill(dt)

        grdAssessment.DataSource = dt
        grdAssessment.DataBind()
        con.Close()

        'con.Close()

    End Sub


    Private Sub grdAssessment_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles grdAssessment.RowCancelingEdit
        Try
            btnTransfer.Enabled = True
            btnLinearAdj.Enabled = True
            btnUseMap.Enabled = True

            grdAssessment.EditIndex = -1

            'enable controls and hide transfered properties table
            ddlOriginMun.Enabled = True
            ddlDestinationMun.Enabled = True
            txtGroupName.Enabled = True
            RestructuredLevyPanelTest()
            grdProperties.DataBind()

            'update the datagrid so it doesn't show in edit mode
            populateAssessmentTaxTable(Session("userID"))
            grdAssessment.DataBind()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub grdAssessment_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles grdAssessment.RowDeleting
        Try
            Dim index As Integer = e.RowIndex
            Dim boundaryGroupID = grdAssessment.DataKeys(index).Values("boundaryGroupID")
            Dim status As String = grdAssessment.Rows(index).Cells(3).Text '*** INKY CHANGED CELLS INDEX +1 ***

            'setup database connection
            Dim con As New SqlClient.SqlConnection
            con.ConnectionString = PATMAP.Global_asax.connString
            Dim query As New SqlClient.SqlCommand
            query.Connection = con
            con.Open()

            'get userid
            Dim userID As Integer = Session("userID")

            'updateBoundaryGroup on deletion
            updateBoundaryGroupOnDeleteGroup(boundaryGroupID, userID, status)

            query.CommandText = "delete from boundaryGroups where boundaryGroupName <> 'Subject' and boundaryGroupID = " & boundaryGroupID
            query.ExecuteNonQuery()
            query.CommandText = "delete from boundaryLinearTransfers where boundaryGroupID = " & boundaryGroupID
            query.ExecuteNonQuery()
            query.CommandText = "delete from boundarytransfers where boundaryGroupID = " & boundaryGroupID
            query.ExecuteNonQuery()

            con.Close()



            'update the assessment and levy data of the group
            'updateBoundaryGroup(boundaryGroupID, userID, "delete")

            'update the Assessment and tax shift table because of the delete
            populateAssessmentTaxTable(userID)
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub
    Private Sub updateBoundaryGroupOnDeleteGroup(ByVal boundaryGroupID As Integer, ByVal userID As Integer, ByVal status As String)

        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString

        Dim query As New SqlClient.SqlCommand
        query.Connection = con
        con.Open()



        If status = "Subject" Then
            query.CommandText = " delete boundaryTransfers from boundaryTransfers, boundaryGroups where boundaryTransfers.boundaryGroupID = boundaryGroups.boundaryGroupID and userID = " & userID
            query.CommandText += " delete boundaryLinearTransfers from boundaryLinearTransfers, boundaryGroups where boundaryLinearTransfers.boundaryGroupID = boundaryGroups.boundaryGroupID and userID = " & userID
            query.CommandText += " delete boundaryGroups where userID = " & userID
            query.ExecuteNonQuery()

            'enable the subject mun drop down list
            ddlSubjMun.Enabled = True

            'clear all the fields
            ddlSubjMun.SelectedIndex = -1
            ddlOriginMun.SelectedIndex = -1
            ddlDestinationMun.SelectedIndex = -1
            txtGroupName.Text = ""
            txtParcelNo.Text = ""

            grdDuplicateProp.Visible = False

        Else
            query.CommandText = "select assessment, originalLevy from boundaryGroups where userID=" & userID & " and boundaryGroupName <> 'Subject' and boundaryGroupID=" & boundaryGroupID
            Dim dr As SqlClient.SqlDataReader = query.ExecuteReader()
            If dr.Read() Then
                Dim assessmentAdjustment As Decimal = dr.GetValue(0)
                Dim levyAdjustment As Decimal = dr.GetValue(1)
                dr.Close()

                'for lost and combined, update the subject
                If status = "Lost" Then
                    query.CommandText = "update boundaryGroups set assessment = assessment + " & (-1) * assessmentAdjustment & ", originalLevy = originalLevy + " & (-1) * levyAdjustment & ", originLevy = originalLevy + " & (-1) * levyAdjustment & ", restructuredLevy = restructuredLevy + " & (-1) * levyAdjustment & " where userID = " & userID & " and boundaryGroupName = 'Subject'"
                    query.ExecuteNonQuery()
                End If

                If status = "Separate" Then
                    'do nothing
                End If

                If status = "Combined" Then
                    query.CommandText = "update boundaryGroups set restructuredLevy = restructuredLevy + " & (-1) * levyAdjustment & " where userID = " & userID & " and boundaryGroupName = 'Subject'"
                    query.ExecuteNonQuery()
                End If
            End If

            dr.Close()
        End If

        'clean up
        con.Close()
    End Sub

    Private Sub grdAssessment_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles grdAssessment.RowEditing
        Try
            'when the users are in edit mode (editing any groups), disable all the buttons until they click save or cancell
            btnTransfer.Enabled = False
            btnLinearAdj.Enabled = False
            btnUseMap.Enabled = False
            grdAssessment.EditIndex = e.NewEditIndex
            ddlOriginMun.Enabled = False
            ddlDestinationMun.Enabled = False
            txtGroupName.Enabled = False

            'clear the parcel number or prefix
            txtParcelNo.Text = ""

            'if its not the subject row clicked on
            If e.NewEditIndex > 0 Then
                'fill in the values to be editted
                txtGroupName.Text = grdAssessment.Rows(e.NewEditIndex).Cells(2).Text
                ddlOriginMun.SelectedValue = grdAssessment.DataKeys(e.NewEditIndex).Values("originMunicipalityID")
                ddlDestinationMun.SelectedValue = grdAssessment.DataKeys(e.NewEditIndex).Values("destinationMunicipalityID")
                Select Case grdAssessment.Rows(e.NewEditIndex).Cells(3).Text '*** INKY CHANGED CELLS INDEX +1 ***
                    Case "Combined"
                        rblRestructuredLevy.Items(1).Selected = False
                        rblRestructuredLevy.Items(0).Selected = True
                        pnlRestructuredLevy.Visible = True
                    Case "Separate"
                        rblRestructuredLevy.Items(0).Selected = False
                        rblRestructuredLevy.Items(1).Selected = True
                        pnlRestructuredLevy.Visible = True
                    Case "Lost"
                        pnlRestructuredLevy.Visible = False
                    Case "Subject"
                        pnlRestructuredLevy.Visible = False
                End Select

                'fill the properties transfered data grid
                populateTransferTable(grdAssessment.DataKeys(e.NewEditIndex).Values("boundaryGroupID"))
            End If

            populateAssessmentTaxTable(Session("userID"))
            grdAssessment.DataBind()

            e.NewEditIndex = grdAssessment.EditIndex
            If grdAssessment.Rows(e.NewEditIndex).Cells(3).Text <> "Lost" And grdAssessment.Rows(e.NewEditIndex).Cells(3).Text <> "Combined" Then '*** INKY CHANGED CELL INDEX +1 ***
                CType(grdAssessment.Rows(e.NewEditIndex).Cells(7).Controls(0), TextBox).ReadOnly = False '*** INKY CHANGED CELL INDEX +1 ***
            Else
                CType(grdAssessment.Rows(e.NewEditIndex).Cells(7).Controls(0), TextBox).ReadOnly = True '*** INKY CHANGED CELL INDEX +1 ***
            End If



            'grdAssessment.DataBind()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

	Private Sub grdAssessment_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles grdAssessment.RowUpdating
		Try
			btnTransfer.Enabled = True
			btnLinearAdj.Enabled = True
			btnUseMap.Enabled = True

			'variables for ID's
			Dim index As Integer = e.RowIndex
			Dim boundaryGroupID = grdAssessment.DataKeys(index).Values("boundaryGroupID")

			'enable controls and hide transfered properties table
			ddlOriginMun.Enabled = True
			ddlDestinationMun.Enabled = True
			txtGroupName.Enabled = True
			RestructuredLevyPanelTest()
			grdProperties.DataBind()

			'setup database connection
			Dim con As New SqlClient.SqlConnection
			con.ConnectionString = PATMAP.Global_asax.connString
			Dim query As New SqlClient.SqlCommand
			Dim dr As SqlClient.SqlDataReader
			query.Connection = con
			con.Open()

			Dim type As Integer
			Dim levyVal As String = "0"
			Dim combinedLevy As Decimal

			If grdAssessment.Rows(index).Cells(3).Text = "Subject" Then	'*** INKY CHANGED CELLS INDEX +1 ***
				levyVal = CType(grdAssessment.Rows(index).Cells(7).Controls(0), TextBox).Text	'*** INKY CHANGED CELLS INDEX +1 ***

				query.CommandText = "select sum(originalLevy) from boundaryGroups where restructuredLevyCombined = 1 and boundaryGroupName <> 'Subject' and userID = " & Session("userID")
				dr = query.ExecuteReader()

				If dr.Read() Then
					If Not dr.IsDBNull(0) Then
						combinedLevy = dr.Item(0)
					End If
				End If
				dr.Close()
			Else
				'if the group is not a SUBJECT
				'Combined Groups
				If rblRestructuredLevy.SelectedIndex = 0 Then
					'reset the status to combined
					type = 1

					'do not reset the restructured levy = 0
					'levyVal = 0
					'get the value from the textbox
					levyVal = CType(grdAssessment.Rows(index).Cells(7).Controls(0), TextBox).Text	'*** INKY CHANGED CELLS INDEX +1 ***
				Else
					'Separate Groups
					type = 0

					query.CommandText = "select restructuredLevyCombined from boundaryGroups where boundaryGroupID = " & boundaryGroupID.ToString
					dr = query.ExecuteReader()

					'Check if it's a Combined group before
					If dr.Read() Then
						If dr.Item(0) Then
							levyVal = "isnull((select sum(levy) from boundarytransfers where boundaryGroupID = " & boundaryGroupID.ToString & "),0.0)"
						Else
							levyVal = CType(grdAssessment.Rows(index).Cells(7).Controls(0), TextBox).Text	'*** INKY CHANGED CELLS INDEX +1 ***
						End If
					End If

					dr.Close()
				End If

			End If


			'update fields in database
			query.CommandText = "update boundaryGroups set "

			'if the group is not a SUBJECT
			If grdAssessment.Rows(index).Cells(3).Text <> "Subject" Then '*** INKY CHANGED CELLS INDEX +1 ***
				'update the new levy status and the restructured levy for seperate, combined or lost
				query.CommandText &= " restructuredLevy = " & levyVal
				query.CommandText &= " , restructuredLevyCombined = " & type
				query.CommandText &= " where boundaryGroupID = " & boundaryGroupID
				query.ExecuteNonQuery()
			Else
				query.CommandText &= " originalLevy = " & levyVal - combinedLevy
				query.CommandText &= " where boundaryGroupID = " & boundaryGroupID
				'query.ExecuteNonQuery()

				query.CommandText = "select restructuredLevy from boundaryGroups where boundaryGroupName = 'Subject' and userID = " & Session("userID")
				dr = query.ExecuteReader()
				dr.Read()
				Dim resLevy = dr.GetValue(0)

				If (levyVal - resLevy) <> 0 Then
					Session.Add("revisedRestLevy", levyVal)
				End If
			End If



			con.Close()

			'get userid
			Dim userID As Integer = Session("userID")

			'update the assessment and levy data of the group
			updateBoundaryGroup(boundaryGroupID, userID, "save")

			'update the Assessment and tax shift table because of the delete
			grdAssessment.EditIndex = -1
			populateAssessmentTaxTable(userID)
		Catch
			'retrieves error message
			Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
		End Try
	End Sub
    Protected Sub grdAssessment_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdAssessment.RowDataBound
        Try

            Dim levyStatus As String

            If e.Row.RowType = DataControlRowType.DataRow Then 'And (e.Row.RowState = DataControlRowState.Normal Or e.Row.RowState = DataControlRowState.Alternate) Then

                Dim imgBtn As ImageButton = CType(e.Row.FindControl("applyLTT"), ImageButton)
                levyStatus = e.Row.Cells(3).Text

                If levyStatus = "Combined" Or levyStatus = "Lost" Then

                    If imgBtn IsNot Nothing Then
                        imgBtn.Visible = False
                    End If
                End If
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

	Protected Sub ddlSubjMun_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlSubjMun.SelectedIndexChanged
		Try
			Session.Remove("CODE_DestinationMunicipality")
			RestructuredLevyPanelTest()
		Catch
			'retrieves error message
			Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
		End Try
	End Sub

	Protected Sub ddlOriginMun_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlOriginMun.SelectedIndexChanged
		Try
			Session.Remove("CODE_DestinationMunicipality")
			RestructuredLevyPanelTest()
		Catch
			'retrieves error message
			Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
		End Try
	End Sub

	Private Sub ddlDestinationMun_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDestinationMun.SelectedIndexChanged
		Session.Remove("CODE_DestinationMunicipality")
	End Sub

    Private Sub RestructuredLevyPanelTest()
        If ddlSubjMun.SelectedValue = ddlOriginMun.SelectedValue Then
            pnlRestructuredLevy.Visible = False
        Else
            pnlRestructuredLevy.Visible = True
        End If
    End Sub

    Protected Sub btnLinearAdj_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnLinearAdj.Click
        Try
            '-----------------------------
            'VALIDATE BOUNDARY INFORMATION
            '-----------------------------
            'make sure the subject municipality is either the origin or destination
            If Not ((ddlSubjMun.SelectedValue = ddlOriginMun.SelectedValue) Or (ddlSubjMun.SelectedValue = ddlDestinationMun.SelectedValue)) Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP66")
                Exit Sub
            End If
            'make sure the origin municipality does not equal destination
            If ddlOriginMun.SelectedValue = ddlDestinationMun.SelectedValue Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP68")
                Exit Sub
            End If
            'make sure that subject municipality has been selected
            If txtGroupName.Text = "Subject" Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP64")
                Exit Sub
            End If
            'make sure that the group name has been filled out
            If String.IsNullOrEmpty(txtGroupName.Text) Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP54")
                Exit Sub
            End If


            '-----------------------------
            'BEGIN CREATING A TRANSFER GROUP
            '-----------------------------
            'setup database connection
            Dim con As New SqlClient.SqlConnection
            con.ConnectionString = PATMAP.Global_asax.connString
            con.Open()
            Dim query As New SqlClient.SqlCommand
            query.Connection = con
            Dim dr As SqlClient.SqlDataReader

            'get userid
            Dim userID As Integer = Session("userID")

            'check if entered group exists
            Dim boundaryGroupID As Integer = 0
            query.CommandText = "select ltrim(rtrim(SubjectMunicipalityID)), ltrim(rtrim(OriginMunicipalityID)), ltrim(rtrim(DestinationMunicipalityID)), restructuredLevyCombined, boundaryGroupID from boundaryGroups where userid = " & userID.ToString & " and boundaryGroupName = '" & txtGroupName.Text & "'"
            dr = query.ExecuteReader
            If dr.Read Then
                'group already exists, check if parameters are the same
                If ddlSubjMun.SelectedValue <> dr.GetValue(0) Then
                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP58")
                    dr.Close()
                    Exit Sub
                End If
                If ddlOriginMun.SelectedValue <> dr.GetValue(1) Then
                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP59")
                    dr.Close()
                    Exit Sub
                End If
                If ddlDestinationMun.SelectedValue <> dr.GetValue(2) Then
                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP60")
                    dr.Close()
                    Exit Sub
                End If
                If rblRestructuredLevy.Items(0).Selected <> dr.GetValue(3) Then
                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP61")
                    dr.Close()
                    Exit Sub
                End If

                boundaryGroupID = dr.GetValue(4)
            End If

            'clean up 
            dr.Close()

            'variables to hold ID's
            Dim assessmentID As Integer
            Dim millRateSurveyID As Integer
            Dim POVID As Integer
            Dim assessmentFormula As String
            Dim taxFormula As String

            query.CommandText = "select assessmentID, millRateSurveyID, POVID, (select formula from dbo.[functions] where functionID = 94) as taxFormula, (select formula from dbo.[functions] where functionID = 91) as assessmentFormula from boundaryModel where status = 1"
            dr = query.ExecuteReader
            dr.Read()
            assessmentID = dr.GetValue(0)
            millRateSurveyID = dr.GetValue(1)
            POVID = dr.GetValue(2)
            taxFormula = dr.GetValue(3)
            assessmentFormula = dr.GetValue(4)
            dr.Close()

            'check if this is the first group entered
            query.CommandText = "select userID from boundaryGroups where userid = " & userID.ToString
            dr = query.ExecuteReader()
            If Not dr.Read() Then
                dr.Close()

                'get original levy for subject municipality
                'query.CommandText = "  select sum(marketValue) as marketValue, sum((marketValue/1000) * POV * uniformMunicipalMillRate) as originalLevy"
                'query.CommandText += " from assessment"
                'query.CommandText += "       inner join pov on assessment.taxclassID = pov.taxclassID "
                'query.CommandText += "       inner join boundaryUniformMunicipalMillRate on rtrim(ltrim(boundaryUniformMunicipalMillRate.municipalityID)) = rtrim(ltrim('" & ddlSubjMun.SelectedValue & "'))"
                'query.CommandText += " where assessmentID = " & assessmentID & " and povID = " & POVID & " and ltrim(rtrim(assessment.municipalityID)) = rtrim(ltrim('" & ddlSubjMun.SelectedValue & "'))"
                'dr = query.ExecuteReader()
                'dr.Read()
                'query.CommandText = "  select " & assessmentFormula & " as marketValue, " & taxFormula & " as originalLevy"
                'query.CommandText += " from assessment"
                'query.CommandText += "       inner join pov on assessment.taxclassID = pov.taxclassID "
                'query.CommandText += "       inner join boundaryUniformMunicipalMillRate on rtrim(ltrim(boundaryUniformMunicipalMillRate.municipalityID)) = rtrim(ltrim('" & ddlSubjMun.SelectedValue & "'))"
                'query.CommandText += " where assessmentID = " & assessmentID & " and povID = " & POVID & " and ltrim(rtrim(assessment.municipalityID)) = rtrim(ltrim('" & ddlSubjMun.SelectedValue & "'))"

                query.CommandText = "  select " & assessmentFormula & " as marketValue, " & taxFormula.Replace("uniformMunicipalMillRate", "bu1.uniformMunicipalMillRate") & " as originalLevy"
                'query.CommandText = "  select " & assessmentFormula & " as marketValue, " & taxFormula.Replace("uniformMunicipalMillRate", "bu1.uniformMunicipalMillRate") & " as originalLevy, " & taxFormula.Replace("uniformMunicipalMillRate", "bu2.uniformMunicipalMillRate") & " as originLevy"
                query.CommandText += " from assessment"
                query.CommandText += "       inner join pov on assessment.taxclassID = pov.taxclassID "
                query.CommandText += "       inner join boundaryUniformMunicipalMillRate bu1 on rtrim(ltrim(bu1.municipalityID)) = rtrim(ltrim('" & ddlSubjMun.SelectedValue & "'))"
                'query.CommandText += "       inner join boundaryUniformMunicipalMillRate bu2 on rtrim(ltrim(bu2.municipalityID)) = rtrim(ltrim('" & ddlOriginMun & "'))"
                query.CommandText += " where assessmentID = " & assessmentID & " and povID = " & POVID & " and ltrim(rtrim(assessment.municipalityID)) = rtrim(ltrim('" & ddlSubjMun.SelectedValue & "'))"

                dr = query.ExecuteReader()
                dr.Read()
                Dim marketValue As Double = dr.GetValue(0)
                Dim originalLevy As Double = dr.GetValue(1)
                dr.Close()

                'create group for subject municipality
                query.CommandText = "insert into boundaryGroups (userID, boundaryGroupName, SubjectMunicipalityID, OriginMunicipalityID, DestinationMunicipalityID, restructuredLevyCombined, assessment, originalLevy, restructuredLevy, uniformMillRate, originLevy) VALUES (" & userID.ToString & ",'Subject','" & ddlSubjMun.SelectedValue & "','" & ddlOriginMun.SelectedValue & "','" & ddlDestinationMun.SelectedValue & "',1," & marketValue & "," & originalLevy & "," & originalLevy & " ," & originalLevy / (marketValue / 1000) & "," & originalLevy & ")"
                query.ExecuteNonQuery()
            Else
                dr.Close()
            End If

            'check if this is a new boundary group
            If boundaryGroupID = 0 Then
                'create new boundary group
                If rblRestructuredLevy.Items(0).Selected Then
                    query.CommandText = "insert into boundaryGroups (userID, boundaryGroupName, SubjectMunicipalityID, OriginMunicipalityID, DestinationMunicipalityID, restructuredLevyCombined) VALUES (" & userID.ToString & ",'" & txtGroupName.Text & "','" & ddlSubjMun.SelectedValue & "','" & ddlOriginMun.SelectedValue & "','" & ddlDestinationMun.SelectedValue & "',1) select @@IDENTITY"
                Else
                    query.CommandText = "insert into boundaryGroups (userID, boundaryGroupName, SubjectMunicipalityID, OriginMunicipalityID, DestinationMunicipalityID, restructuredLevyCombined) VALUES (" & userID.ToString & ",'" & txtGroupName.Text & "','" & ddlSubjMun.SelectedValue & "','" & ddlOriginMun.SelectedValue & "','" & ddlDestinationMun.SelectedValue & "',0) select @@IDENTITY"
                End If
                dr = query.ExecuteReader()
                dr.Read()
                boundaryGroupID = dr.GetValue(0)
                dr.Close()
            End If
            '-----------------------------
            'END CREATING A TRANSFER GROUP
            '-----------------------------

            '-----------------------------
            'UPLOAD BOUNDARY LINEAR INFORMATION
            '-----------------------------
            'only if the group does not exist in the linear table
            query.CommandText = "select boundaryLinearTransferID from boundaryLinearTransfers where boundaryGroupID=" & boundaryGroupID
            dr = query.ExecuteReader()
            If Not dr.Read() Then
                'get the market value of the origin municipality for railway and pipeline classes
                'then insert a record into boundaryLinerTransfer table per class with the market value
                dr.Close()
                'query.CommandText = "select sum(marketValue), taxClassID from assessment where taxClassID in ('P', 'PP') and assessmentID=" & assessmentID & " and municipalityID='" & ddlOriginMun.SelectedValue & "' group by taxClassID"
                query.CommandText = " select isnull(((marketValue * (case when (taxable+otherExempt+FGIL+PGIL+Section293+ByLawExemption) = 0 then 0 else (taxable)/(taxable+otherExempt+FGIL+PGIL+Section293+ByLawExemption) end)) * POV),0) as taxableAssessmentValue," & vbCrLf & _
                                    " isnull(((marketValue * (case when (taxable+otherExempt+FGIL+PGIL+Section293+ByLawExemption) = 0 then 0 else (FGIL)/(taxable+otherExempt+FGIL+PGIL+Section293+ByLawExemption) end)) * POV),0) as FGILAssessmentValue," & vbCrLf & _
                                    " isnull(((marketValue * (case when (taxable+otherExempt+FGIL+PGIL+Section293+ByLawExemption) = 0 then 0 else (PGIL)/(taxable+otherExempt+FGIL+PGIL+Section293+ByLawExemption) end)) * POV),0) as PGILAssessmentValue," & vbCrLf & _
                                    " isnull(((marketValue * (case when (taxable+otherExempt+FGIL+PGIL+Section293+ByLawExemption) = 0 then 0 else (otherExempt)/(taxable+otherExempt+FGIL+PGIL+Section293+ByLawExemption) end)) * POV),0) as otherExemptAssessmentValue," & vbCrLf & _
                                    " isnull(((marketValue * (case when (taxable+otherExempt+FGIL+PGIL+Section293+ByLawExemption) = 0 then 0 else (Section293)/(taxable+otherExempt+FGIL+PGIL+Section293+ByLawExemption) end)) * POV),0) as Section293AssessmentValue," & vbCrLf & _
                                    " isnull(((marketValue * (case when (taxable+otherExempt+FGIL+PGIL+Section293+ByLawExemption) = 0 then 0 else (ByLawExemption)/(taxable+otherExempt+FGIL+PGIL+Section293+ByLawExemption) end)) * POV),0) as ByLawExemptionAssessmentValue," & vbCrLf & _
                                    " assessment.taxClassID, alternate_parcelID" & vbCrLf & _
                                    " from assessment" & vbCrLf & _
                                    " join POV on POV.POVID = " & POVID & " and assessment.taxClassID = POV.taxClassID" & vbCrLf & _
                                    " where assessment.taxClassID in ('P', 'PP') and assessmentID=" & assessmentID & " and municipalityID='" & ddlOriginMun.SelectedValue & "'"
                dr = query.ExecuteReader()
                Dim cmdText = ""
                'query.CommandText = ""
                While dr.Read()
                    If ddlOriginMun.SelectedValue <> ddlSubjMun.SelectedValue Then
                        cmdText = "insert into boundaryLinearTransfers values (" & boundaryGroupID & ",'" & dr.GetValue(6) & "'," & dr.GetValue(0) & "," & dr.GetValue(1) & "," & dr.GetValue(2) & "," & dr.GetValue(3) & "," & dr.GetValue(4) & "," & dr.GetValue(5) & "," & dr.GetValue(7) & ",0)" & vbCrLf
                    Else
                        cmdText += "insert into boundaryLinearTransfers values (" & boundaryGroupID & ",'" & dr.GetValue(6) & "', (-1) * " & dr.GetValue(0) & ", (-1) * " & dr.GetValue(1) & ",(-1) * " & dr.GetValue(2) & ", (-1) * " & dr.GetValue(3) & ", (-1) * " & dr.GetValue(4) & ", (-1) * " & dr.GetValue(5) & "," & dr.GetValue(7) & ",0)" & vbCrLf
                    End If
                End While
                dr.Close()
                If cmdText <> "" Then
                    query.CommandText = cmdText
                    query.ExecuteNonQuery()
                End If
            Else
                dr.Close()
            End If

            con.Close()

            '-----------------------------
            'Call linearProperty page
            '-----------------------------
            Session.Add("boundaryGroupID", boundaryGroupID)
            Dim str As String
            str = "<script = language='javascript'>"
            str += "window.open('linearproperty.aspx', 'LinearPropertyAdjustment', 'left=50,top=50,width=500,height=400,toolbar=0,resizable=1,scrollbars=1')"
            str += "</script>"
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "code", str)
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSubmit.Click

    End Sub

    Protected Sub chkSelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSelectAll.CheckedChanged
        If chkSelectAll.Checked = True Then
            txtParcelNo.Enabled = False
            btnLinearAdj.Enabled = False
        Else
            txtParcelNo.Enabled = True
            btnLinearAdj.Enabled = True
        End If
    End Sub

    Private Sub btnUseMap_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnUseMap.Click
        Try
            'disable the subject mun drop down list
            ddlSubjMun.Enabled = False

            '---------------------------------
            'VALIDATE THE INPUT FIELDS
            '---------------------------------

            'make sure the subject municipality is either the origin or destination
            If Not ((ddlSubjMun.SelectedValue = ddlOriginMun.SelectedValue) Or (ddlSubjMun.SelectedValue = ddlDestinationMun.SelectedValue)) Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP66")
                Exit Sub
            End If
            'make sure the origin municipality does not equal destination
            If ddlOriginMun.SelectedValue = ddlDestinationMun.SelectedValue Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP68")
                Exit Sub
            End If
            'make sure that the group name has been filled out
            If txtGroupName.Text = "Subject" Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP64")
                Exit Sub
            End If
            'make sure that the group name has been filled out
            If String.IsNullOrEmpty(txtGroupName.Text) Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP54")
                Exit Sub
            End If

            txtGroupName.Text = Trim(txtGroupName.Text.Replace("'", "''"))

            'make sure that the group name has no special characters
            If Not common.ValidateNoSpecialChar(txtGroupName.Text) Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP84")
                Exit Sub
            End If

            'get userid
            Dim userID As Integer = Session("userID")

            'clear the parcel number or prefix
            txtParcelNo.Text = ""

            '---------------------------------
            'END - VALIDATE THE INPUT FIELDS
            '---------------------------------

            'setup database connection
            Dim con As New SqlClient.SqlConnection
            con.ConnectionString = PATMAP.Global_asax.connString
            con.Open()

            'command obj
            Dim query As New SqlClient.SqlCommand
            query.Connection = con

            'data reader obj
            Dim dr As SqlClient.SqlDataReader


            '---------------------------------
            'VALIDATE THE EXISTING GROUP            
            '---------------------------------

            'check if entered group exists
            Dim boundaryGroupID As Integer = 0
            query.CommandText = "select ltrim(rtrim(SubjectMunicipalityID)), ltrim(rtrim(OriginMunicipalityID)), ltrim(rtrim(DestinationMunicipalityID)), restructuredLevyCombined, boundaryGroupID from boundaryGroups where userid = " & userID.ToString & " and boundaryGroupName = '" & txtGroupName.Text & "'"
            dr = query.ExecuteReader
            If dr.Read Then
                'group already exists, check if parameters are the same
                'if the subject municipality is the same
                If ddlSubjMun.SelectedValue <> dr.GetValue(0) Then
                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP58")
                    dr.Close()
                    Exit Sub
                End If
                'if the origin municipality is the same
                If ddlOriginMun.SelectedValue <> dr.GetValue(1) Then
                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP59")
                    dr.Close()
                    Exit Sub
                End If
                'if the destination municipality is the same
                If ddlDestinationMun.SelectedValue <> dr.GetValue(2) Then
                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP60")
                    dr.Close()
                    Exit Sub
                End If
                'if the restructured levy type (combine/separate) is the same
                If rblRestructuredLevy.Items(0).Selected <> dr.GetValue(3) Then
                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP61")
                    dr.Close()
                    Exit Sub
                End If

                boundaryGroupID = dr.GetValue(4)
            End If
            dr.Close()

            '---------------------------------
            'END - VALIDATE THE EXISTING GROUP
            '---------------------------------

            'variables to hold ID's
            Dim assessmentID As Integer
            query.CommandText = "select assessmentID, millRateSurveyID, POVID, (select formula from dbo.[functions] where functionID = 94) as taxFormula, (select formula from dbo.[functions] where functionID = 91) as assessmentFormula from boundaryModel where status = 1"
            dr = query.ExecuteReader
            dr.Read()
            assessmentID = dr.GetValue(0)
            Session.Add("assessmentID", assessmentID)
            dr.Close()

            'clear the map parcels table
            'query.CommandText = "delete from mapBoundaryTransfers where userID = " & userID
            'query.ExecuteNonQuery()

            createSubjectGroup(userID, boundaryGroupID, 1)

            Dim RestructuredLevy As Boolean
            If rblRestructuredLevy.Items(0).Selected Then RestructuredLevy = 1 Else RestructuredLevy = 0

            Session.Add("MapBoundaryChangeSource", ddlOriginMun.SelectedValue)
            Session.Add("MapBoundaryChangeSubject", ddlSubjMun.SelectedValue)
            Session.Add("MapBoundaryChangeDestination", ddlDestinationMun.SelectedValue)
            Session.Add("MapboundaryGroupId", boundaryGroupID)
            Session.Add("MapGroupName", txtGroupName.Text)
            Session.Add("MapRestructuredLevy", RestructuredLevy)
            Response.Redirect("BoundaryAdjustmentMap.aspx")
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try



    End Sub

    Private Sub btnHLinearAdj_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnHLinearAdj.Click

    End Sub

    Private Sub btnTransfer_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTransfer.Load

    End Sub

    Private Sub grdDuplicateProp_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdDuplicateProp.PageIndexChanging
        Try
            Dim dt As DataTable

            'sets grid's page index
            grdDuplicateProp.PageIndex = e.NewPageIndex

            'fill the grid
            If Not IsNothing(Session("dupProperties")) Then
                dt = CType(Session("dupProperties"), DataTable)
                grdDuplicateProp.DataSource = dt
                grdDuplicateProp.DataBind()
            Else
                populateDuplicateGrd(Session("userID"), Session("assessmentID"), Session("map"))
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub btnHUseMap_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnHUseMap.Click

	End Sub

	Public Overrides Sub VerifyRenderingInServerForm(ByVal grdDuplicateProp As Control)

	End Sub

    Protected Sub btnPrintDpl_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnPrintDpl.Click
        Dim attachment As String = "attachment; filename=Duplicate_Properties.xls"
        With Response
            .ClearContent()
            .AddHeader("content-disposition", attachment)
            .ContentType = "application/ms-excel"
            Dim sw As New System.IO.StringWriter
            Dim index As Integer = 0
            Dim htw As New HtmlTextWriter(sw)
            With grdDuplicateProp
                For index = 0 To grdDuplicateProp.Columns.Count - 1
                    .Columns(index).SortExpression = Nothing
                Next
                .AllowSorting = False
                .AllowPaging = False
                .GridLines = GridLines.None
                .HeaderStyle.Font.Bold = True
                .BackColor = System.Drawing.Color.Transparent
                .ForeColor = System.Drawing.Color.Black
                Try
                    Dim dt As DataTable

                    'fill the grid
                    If Not IsNothing(Session("dupProperties")) Then
                        dt = CType(Session("dupProperties"), DataTable)
                        grdDuplicateProp.DataSource = dt
                        grdDuplicateProp.DataBind()
                    Else
                        populateDuplicateGrd(Session("userID"), Session("assessmentID"), Session("map"))
                    End If
                Catch
                    'retrieves error message
                    Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
                End Try

                index = .Rows.Count
                .RenderControl(htw)
            End With
            .Write(sw.ToString)
            .End()
        End With
    End Sub

    Private Sub grdAssessment_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdAssessment.RowCommand

        Try
            'assign boundaryGroupID of selected ROW as session variable 
            If e.CommandName = "applyLTT" Then
                Dim grdRow As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
                Dim boundaryGrpID As Integer = grdAssessment.DataKeys(grdRow.RowIndex).Values("boundaryGroupID")

                Session.Add("boundarySelection", boundaryGrpID)
                Response.Redirect("~/taxtools/main.aspx")
            End If

        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)

        End Try
	End Sub

End Class