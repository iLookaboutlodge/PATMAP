Imports System.Web.UI.HtmlControls

Partial Public Class main2
    Inherits System.Web.UI.Page

    Dim taxClassSelected As Boolean = False
    Dim LTTvalidated As Boolean = False
    'Dim boundaryGrpID As Integer = 0

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Try
			'Clears out the error message
			Master.errorMsg = ""

			'added for calculating LTT tables when Map is first time loaded	17-sep-2013
			System.Web.HttpContext.Current.Session.Remove("LTTcalculated")

			If Not IsPostBack Then

				'Sets submenu to be displayed
				subMenu.setStartNode(menu.taxtools)

				'*** Assign Subject Municipality and Subj Year values to Labels

				'setup database connection
				Dim con As New SqlClient.SqlConnection
				Dim query As New SqlClient.SqlCommand
				Dim dr As SqlClient.SqlDataReader

				con.ConnectionString = PATMAP.Global_asax.connString
				query.Connection = con
				query.CommandTimeout = 60000
				con.Open()
				query.CommandText = "SELECT year FROM boundaryModel WHERE status = 1"
				dr = query.ExecuteReader()
				dr.Read()
				Dim subjYear As String = dr.GetValue(0)
				dr.Close()

				'sets Subject Year for the LTT module.
				If IsNothing(Session("LTTsubjYear")) Then
					Session.Add("LTTsubjYear", subjYear)
				Else
					Session("LTTsubjYear") = subjYear
				End If
				lblLiveSubjYr.Text = Session("LTTsubjYear")

				If IsNothing(Session("LTTSubjectMunicipality")) Then
					lblSubjMun.Visible = False
					lblLiveSubjMun.Visible = False
				Else
					lblSubjMun.Visible = True
					lblLiveSubjMun.Visible = True
					lblLiveSubjMun.Text = Replace(StrConv(Session("LTTSubjectMunicipality"), vbProperCase), "Of ", "of ")	'convert boundaryName to propercase
					'lblLiveSubjMun.Text = Session("LTTSubjectMunicipality")
				End If



				'fill Subject Municipality dropdown list
				fillDropDown()

				'Fill Tax Classes checkbox list
				fillTaxClasses()

			End If

		Catch
			'retrieves error message
			Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
		End Try
	End Sub

	Protected Sub btnSubmit_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSubmit.Click
		Try

			updateLiveLTTtable()
			setLiveTaxClasses()
			validateLTTselection()

			'initSessionVariables according to map	19-sep-2013
			common.LTTMAP_InitRequiredSessionVars()

			If LTTvalidated Then
				If taxClassSelected Then
					fillLiveLTTValues()
					If Session("phaseInBaseYearAccess") Then
						fillLiveLTTPhaseInValues()
					End If

					'forces the user to start over once the submit button is clicked
					'user must click the submit on Main, Base Year (if applicable) and Start Tax page to continue...
					If Not IsNothing(Session("LTTfreeReign")) Then
						Session.Remove("LTTfreeReign")
					End If

					'setup database connection
					Dim con As New SqlClient.SqlConnection
					Dim query As New SqlClient.SqlCommand
					Dim dr As SqlClient.SqlDataReader

					con.ConnectionString = PATMAP.Global_asax.connString
					query.Connection = con
					query.CommandTimeout = 60000
					con.Open()

					'get IDs
					query.CommandText = "select assessmentID, baseAssessmentID, POVID from boundaryModel where status = 1"
					dr = query.ExecuteReader
					dr.Read()
					Dim baseAssessmentID As Integer
					Dim subjectAssessmentID As Integer
					Dim POVID As Integer
					subjectAssessmentID = dr.GetValue(0)
					baseAssessmentID = dr.GetValue(1)
					POVID = dr.GetValue(2)

					If Session("phaseInBaseYearAccess") Then
						'excute compare
						dr.Close()
						query.CommandText = "exec compareBoundaryBaseandSubject " & Session("userID").ToString & "," & subjectAssessmentID.ToString & "," & baseAssessmentID.ToString & ",'" & ddlSubjMun.SelectedValue & "'," & POVID.ToString
						query.ExecuteNonQuery()
					Else

						dr.Close()
						Dim subMunID As String
						query.CommandText = "select number from entities where jurisdiction = '" & Replace(Session("LTTSubjectMunicipality").ToString, "'", "''") & "'"
						dr = query.ExecuteReader()
						dr.Read()
						subMunID = dr.GetValue(0)
						dr.Close()

						If Not IsNothing(Session("boundarySelection")) Then
							If ddlSubjMun.SelectedItem.Text = "Subject" Then
								query.CommandText = "exec populateLTTSubjectAssessmentFromBoundary " & Session("userID").ToString & ",'" & subMunID & "'," & subjectAssessmentID.ToString & ",1," & POVID.ToString
								query.ExecuteNonQuery()
							Else
								query.CommandText = "exec populateLTTSubjectAssessmentFromBoundary " & Session("userID").ToString & ",'" & subMunID & "'," & subjectAssessmentID.ToString & ",0," & POVID.ToString
								query.ExecuteNonQuery()
							End If
						Else
							query.CommandText = "exec populateLTTSubjectAssessmentWithoutPhaseIn " & Session("userID").ToString & ",'" & subMunID & "'," & subjectAssessmentID.ToString & "," & POVID.ToString
							query.ExecuteNonQuery()
						End If
					End If

					'drop live base, subject and model tables - to reset
					query.CommandText = "SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[liveLLTBase_" & Session("userID").ToString & "]') AND type in (N'U')"
					dr = query.ExecuteReader
					If dr.Read() Then
						dr.Close()
						query.CommandText = "drop table liveLLTBase_" & Session("userID")
						query.ExecuteNonQuery()
					End If

					dr.Close()
					query.CommandText = "SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[liveLLTSubject_" & Session("userID").ToString & "]') AND type in (N'U')"
					dr = query.ExecuteReader
					If dr.Read() Then
						dr.Close()
						query.CommandText = "drop table liveLLTSubject_" & Session("userID")
						query.ExecuteNonQuery()
					End If

					dr.Close()
					query.CommandText = "SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[liveLLTSubjectModel_" & Session("userID").ToString & "]') AND type in (N'U')"
					dr = query.ExecuteReader
					If dr.Read() Then
						dr.Close()
						query.CommandText = "drop table liveLLTSubjectModel_" & Session("userID")
						query.ExecuteNonQuery()
					End If

					dr.Close()
					query.CommandText = "SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[liveLTTPhaseIn_" & Session("userID").ToString & "]') AND type in (N'U')"
					dr = query.ExecuteReader
					If dr.Read() Then
						dr.Close()
						query.CommandText = "drop table liveLTTPhaseIn_" & Session("userID")
						query.ExecuteNonQuery()
					End If

					dr.Close()
					query.CommandText = "SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[liveLTTResults_" & Session("userID").ToString & "]') AND type in (N'U')"
					dr = query.ExecuteReader
					If dr.Read() Then
						dr.Close()
						query.CommandText = "drop table liveLTTResults_" & Session("userID")
						query.ExecuteNonQuery()
					End If

					dr.Close()
					query.CommandText = "SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[liveLTTSummary_" & Session("userID").ToString & "]') AND type in (N'U')"
					dr = query.ExecuteReader
					If dr.Read() Then
						dr.Close()
						query.CommandText = "drop table liveLTTSummary_" & Session("userID")
						query.ExecuteNonQuery()
					End If

					dr.Close()
					query.CommandText = "SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[liveLTTPhaseInSummary_" & Session("userID").ToString & "]') AND type in (N'U')"
					dr = query.ExecuteReader
					If dr.Read() Then
						dr.Close()
						query.CommandText = "drop table liveLTTPhaseInSummary_" & Session("userID")
						query.ExecuteNonQuery()
					End If

					dr.Close()

					'reset live tax classes and tax status tbls.
					query.CommandText = "delete liveTaxClasses where userID = " & Session("userID")
					query.ExecuteNonQuery()

					query.CommandText = "insert liveTaxClasses select " & Session("userID") & ", taxClassID, 0 from LTTTaxClasses"
					query.ExecuteNonQuery()

					query.CommandText = "delete liveTaxStatus where userID = " & Session("userID")
					query.ExecuteNonQuery()

					query.CommandText = "insert liveTaxStatus select " & Session("userID") & ", taxstatusID, 0 from taxStatus"
					query.ExecuteNonQuery()

					Session.Remove("orgSubjUMR")
					Session.Remove("orgSubjLevy")
					Session.Remove("revSubjUMR")

					If Session("phaseInBaseYearAccess") Then
						Response.Redirect("baseyear.aspx")
					Else
						Response.Redirect("starttax.aspx")
					End If
				End If

			End If

		Catch
			'retrieves error message
			Master.errorMsg = common.GetErrorMessage(Err.Number, Err)

		End Try
	End Sub

    Protected Sub fillTaxClasses()

        Dim userID As Integer = Session("userID")

        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString
        Dim query As New SqlClient.SqlCommand
        query.Connection = con
        con.Open()

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dr As SqlClient.SqlDataReader
        Dim sql As String = ""

        'check to see if userlevel permission enable user to view only rolled up main tax classes
        'or the (full) explanded tax classes view
        query.CommandText = "select LTTfullView from LTTlevels where levelID = " & Session("levelID")
        dr = query.ExecuteReader
        dr.Read()

        If dr.HasRows Then
            Dim showFullView As Boolean = dr.GetValue(0)
            Session.Add("showFullTaxClasses", showFullView)
        End If

        'clean up
        dr.Close()

        '-------------------------------------------------------------------
        ' POPULATE SUB CLASSES BASED ON LEVEL (and userlevel permissions)
        '-------------------------------------------------------------------

        '(Re)Create liveLTTtable once user enters LTT module
        If Session("liveLTTtableCreated") = False Then
            common.createLiveLTTtable()
        End If

        Dim ds As New DataSet

        da = New SqlClient.SqlDataAdapter
        da.SelectCommand = New SqlClient.SqlCommand
        da.SelectCommand.Connection = con

        query.CommandText = "select taxClassID from LTTmainTaxClasses where active = 1"
        dr = query.ExecuteReader()

        'for each main roll-up class, get applicable "sub" tax classes
        While dr.Read()
            'Precautionary
            If Session("liveLTTtableCreated") = False Then
                common.createLiveLTTtable()
            End If

            sql &= "select liveLTTtaxClasses_" & userID & ".taxClassID, LTTtaxClasses.taxClass, show as [default], LTTmainTaxClasses.taxClass as rollUpClass from liveLTTtaxClasses_" & userID & " inner join LTTtaxClasses on LTTtaxClasses.taxClassID = liveLTTtaxClasses_" & userID & ".taxClassID inner join LTTmainTaxClasses on LTTtaxClasses.LTTmainTaxClassID = LTTmainTaxClasses.taxClassID where LTTtaxClasses.LTTmainTaxClassID = '" & dr.Item(0) & "' order by LTTtaxClasses.sort" & vbCrLf

        End While

        'clean up
        dr.Close()

        'fill dataset
        da.SelectCommand.CommandText = sql
        da.Fill(ds)


        Dim i As Integer
        Dim counter As Integer

        'shows subclasses if userlevel contains sufficient permissions
        If Session("showFullTaxClasses") Then

            'Hide Main LTT roll-up Class tables
            For counter = 1 To 3
                Page.FindControl("ctl00$mainContent$tblRollup" & counter).Visible = False
            Next

            Dim ckList As CheckBoxList
            Dim tblTaxClass As HtmlTable

            'populate checkboxes
            For counter = 0 To ds.Tables.Count - 1
                ckList = Page.FindControl("ctl00$mainContent$cklTaxClass" & counter + 1)
                tblTaxClass = Page.FindControl("ctl00$mainContent$tblTaxC1ass" & counter + 1)

                'bind checkboxlist to dataset
                If Not IsNothing(ckList) Then
                    ckList.DataSource = ds.Tables(counter)
                    ckList.DataTextField = "taxClass"
                    ckList.DataValueField = "taxClassID"
                    ckList.DataBind()

                    'format checkboxlist to display 2 columns if there are more than 5 classes in a table
                    If ckList.Items.Count > 0 Then

                        ckList.RepeatColumns = "0"

                        If ckList.Items.Count > 5 Then
                            ckList.RepeatColumns = "2"
                        End If

                    Else
                        'hide table if no tax classes in checkboxlist
                        tblTaxClass.Visible = False

                    End If

                    'list the boxes for each main "roll-up" class 
                    For i = 0 To ckList.Items.Count - 1
                        ckList.Items(i).Selected = ds.Tables(counter).Rows(i).Item("default")
                    Next

                End If
            Next
            'hide any remaining taxclass tables
            If counter < 3 Then
                'do stuff
                While counter < 3
                    counter += 1
                    Page.FindControl("ctl00$mainContent$tblTaxC1ass" & counter).Visible = False
                End While
            End If
        Else

            'show only rolled-up classes if userlevel does not have permission to see full view
            Dim tblRollup As HtmlTable
            Dim chkBox As CheckBox

            For counter = 0 To ds.Tables.Count - 1
                tblRollup = Page.FindControl("ctl00$mainContent$tblRollup" & counter + 1)

                'if table has no data hide the roll-up class in the user view
                If ds.Tables(counter).Rows.Count = 0 Then
                    tblRollup.Visible = False
                Else
                    tblRollup.Visible = True 'make visible
                    chkBox = Page.FindControl("ctl00$mainContent$chkRollup" & counter + 1)
                    chkBox.Checked = ds.Tables(counter).Rows(i).Item("default") 'set checkbox to checked by default
                    chkBox.Text = ds.Tables(counter).Rows(i).Item("rollUpClass")
                End If

            Next

            'hide tax classes (show only roll-up classes)
            For counter = 1 To 3
                Page.FindControl("ctl00$mainContent$tblTaxC1ass" & counter).Visible = False
            Next
            '' '' ''tblTaxC1ass1.Visible = False 'aka Agriculture tax classes
            '' '' ''tblTaxC1ass2.Visible = False 'aka Residental tax classes
            '' '' ''tblTaxC1ass3.Visible = False 'aka Commercial tax classes

        End If

        'format table to display with center alignment
        tblTaxClasses.Align = "center"

        'clean up                
        con.Close()

    End Sub

    Protected Sub updateLiveLTTtable()
        Try

            Dim userID As Integer = Session("UserID")
            Dim counter As Integer
            Dim sql As String
            Dim ckboxCount As Integer = 0
            Dim ckboxOff As Integer = 0

            'setup database connection
            Dim con As New SqlClient.SqlConnection
            con.ConnectionString = PATMAP.Global_asax.connString
            Dim query As New SqlClient.SqlCommand
            query.Connection = con
            con.Open()

            Dim i As Integer

            If Session("showFullTaxClasses") Then
                Dim ckList As CheckBoxList

                'update table with checkbox selections
                counter = 1
                ckList = Page.FindControl("ctl00$mainContent$cklTaxClass" & counter)

                'loop through each class and update status for liveLTTtaxClasses
                'liveLTTtaxClasses is created from common.vb default value for show is 1 on creation
                sql = ""

                While Not IsNothing(ckList)
                    For i = 0 To ckList.Items.Count - 1
                        If ckList.Items(i).Selected Then
                            sql &= "update liveLTTtaxClasses_" & userID & " set show = 1 where taxClassID = '" & ckList.Items(i).Value & "'" & vbCrLf
                        Else
                            sql &= "update liveLTTtaxClasses_" & userID & " set show = 0 where taxClassID = '" & ckList.Items(i).Value & "'" & vbCrLf
                            ckboxOff += 1
                        End If

                    Next
                    ckboxCount += ckList.Items.Count
                    counter += 1
                    ckList = Page.FindControl("ctl00$mainContent$cklTaxClass" & counter)
                End While
                If sql <> "" Then
                    query.CommandText = sql
                    query.ExecuteNonQuery()

                Else
                    'throw exception
                    Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
                End If

            Else
                Dim chkBox As CheckBox

                counter = 1
                chkBox = Page.FindControl("ctl00$mainContent$chkRollup" & counter)

                'loop through each selected class and update status
                sql = ""
                While Not IsNothing(chkBox)
                    If chkBox.Checked Then
                        sql &= "update liveLTTtaxClasses_" & userID & " set show = 1 where taxClassID in (select taxClassID from LTTtaxClasses where LTTmainTaxClassID = " & counter & ")" & vbCrLf
                    Else
                        sql &= "update liveLTTtaxClasses_" & userID & " set show = 0 where taxClassID in (select taxClassID from LTTtaxClasses where LTTmainTaxClassID = " & counter & ")" & vbCrLf
                        ckboxOff += 1
                    End If
                    ckboxCount += 1
                    counter += 1
                    chkBox = Page.FindControl("ctl00$mainContent$chkRollup" & counter)
                End While
                If sql <> "" Then
                    query.CommandText = sql
                    query.ExecuteNonQuery()
                Else
                    'throw exception
                    Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
                End If

            End If

            'Checks to ensure at least one tax class is selected for the LTT module
            If ckboxCount = ckboxOff Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP143")
                taxClassSelected = False
            Else
                taxClassSelected = True
            End If

        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try

    End Sub

    Protected Sub fillDropDown()

        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString
        Dim query As New SqlClient.SqlCommand
        query.Connection = con
        con.Open()

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim sql As String

        If IsNothing(Session("boundarySelection")) Then 'user has NOT entered LTT from the boundary page

            'enable Subject Municipality drop down list
            lblSelectSubjMun.Text = "Select Subject Municipality"

            'hide Subject Municipality labels
            'lblSubjMun.Visible = False
            'lblLiveSubjMun.Visible = False

            'get users Subj Municipality selection from the entities table
            sql = "select ' ' as number, '--Municipality--' as jurisdiction from jurisdictionTypes where jurisdictiontypeid = 1 "
            sql += "union select number, jurisdiction from entities where jurisdictionTypeid > 1 order by jurisdiction"
            da.SelectCommand = New SqlClient.SqlCommand(sql, con)

            da.Fill(dt)
            ddlSubjMun.DataSource = dt
            ddlSubjMun.DataValueField = "number"
            ddlSubjMun.DataTextField = "jurisdiction"
            ddlSubjMun.DataBind()

            ddlSubjMun.SelectedItem.Selected = False
            ddlSubjMun.Items(0).Selected = True

        Else 'If user enters LTT from the Boundary page

            Dim boundaryGrpID As Integer = Session("boundarySelection")

            'get get boundary information from boundaryGroups table
            sql = "SELECT boundaryGroupID, boundaryGroupName, SubjectMunicipalityID, jurisdiction FROM boundaryGroups inner join entities on entities.number = boundaryGroups.SubjectMunicipalityID WHERE (boundaryGroupName = 'Subject' or (boundaryGroupName <> 'Subject' AND restructuredLevyCombined = 0 AND subjectMunicipalityID <> originMunicipalityID)) AND userID = '" & Session("userID") & "'"
            da.SelectCommand = New SqlClient.SqlCommand(sql, con)

            da.Fill(dt)
            ddlSubjMun.DataSource = dt
            ddlSubjMun.DataValueField = "boundaryGroupID"
            ddlSubjMun.DataTextField = "boundaryGroupName"
            ddlSubjMun.DataBind()

            ddlSubjMun.ClearSelection()
            ddlSubjMun.SelectedValue = boundaryGrpID 'sets ddl to users selected row from boundary page.
            lblSelectSubjMun.Text = "Select Boundary Type"

            'show the Subject Municipality label
            lblSubjMun.Visible = True
            lblLiveSubjMun.Visible = True
            lblLiveSubjMun.Text = StrConv(dt.Rows(0).Item("jurisdiction"), vbProperCase) 'convert boundaryName to propercase
            lblLiveSubjMun.Text = Replace(lblLiveSubjMun.Text, "Of ", "of ") 'ensures "of" is not capitalized.


        End If

        If Not IsNothing(Session("LTTdropDownChoice")) Then
            ddlSubjMun.ClearSelection()
            ddlSubjMun.SelectedValue = Session("LTTdropDownChoice")
        End If

        'clean up
        con.Close()
    End Sub


    Protected Sub cklTaxClass1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cklTaxClass1.SelectedIndexChanged

    End Sub

    Private Sub btnReset_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnReset.Click
        ddlSubjMun.SelectedItem.Selected = False
        ddlSubjMun.Items(0).Selected = True
        Session("liveLTTtableCreated") = False
        fillTaxClasses()
    End Sub

    Protected Sub setLiveTaxClasses()
        'setup database connection
        Dim con As New SqlClient.SqlConnection
        Dim userID As Integer = Session("userID")
        Dim query As New SqlClient.SqlCommand
        con.ConnectionString = PATMAP.Global_asax.connString
        query.Connection = con
        con.Open()

        Dim da As New SqlClient.SqlDataAdapter()
        Dim dt As New DataTable
        Dim dr As SqlClient.SqlDataReader
        Dim sql As String

        '-------------------------------------
        'FILL IN LTT live TAX CLASS DATATABLE
        '-------------------------------------
        sql = "select taxClasses.taxClassID, taxClasses.taxClass from taxClasses inner join liveLTTtaxClasses_" & userID & " on liveLTTtaxClasses_" & userID & ".taxClassID = taxClasses.taxClassID where liveLTTtaxClasses_" & userID & ".show = 1 order by taxClasses.sort"
        da.SelectCommand = New SqlClient.SqlCommand(sql, con)
        da.Fill(dt)

        'set LTTliveTaxClasses Session variable
        Session.Add("LTTliveTaxClasses", dt)


        '----------------------------------------
        'FILL IN LTT live ROLL-UP CLASS DATATABLE
        '----------------------------------------
        dt = New DataTable
        sql = "SELECT DISTINCT LTTmainTaxClassID, LTTmainTaxClasses.taxClass FROM liveLTTtaxClasses_" & userID & " inner join LTTmainTaxClasses on LTTmainTaxClasses.taxClassID = liveLTTtaxClasses_" & userID & ".LTTmainTaxClassID where show = '1'"
        da.SelectCommand = New SqlClient.SqlCommand(sql, con)
        da.Fill(dt)

        'set LTTliveRollUpClasses Session variable
        Session.Add("LTTliveRollUpClasses", dt)

        '-------------------------------
        'SET PHASE-IN SESSION VARIABLE 'sets variable to allow/deny access to specified LTT pages/features
        '-------------------------------
        query.CommandText = "SELECT count(*) FROM levelsPermission WHERE levelID = '" & Session("levelID") & "' AND screenNameID IN(106,111) AND access = 1" 'ScreenNameID 111 = 'Phase-In screen' ScreenNameID 106 = 'BaseYear'
        dr = query.ExecuteReader()

        Dim accessGranted As Integer
        If dr.HasRows Then
            dr.Read()
            accessGranted = dr.GetValue(0)
        Else
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage("PATMAP151")
            Exit Sub
        End If
        dr.Close()

        'if accessGranted = 2, user has access to both Phase-In Page AND Base Year page
        'if accessGranted = 0, user does NOT have access to the Phase-In OR Base Year page
        'if accessGranted = 1, there is an error in the program
        If accessGranted = 2 Then
            Session.Add("phaseInBaseYearAccess", True)

        ElseIf accessGranted = 0 Then
            Session.Add("phaseInBaseYearAccess", False)
        Else
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage("PATMAP151")
            Exit Sub
        End If

        'if user is coming from the Boundary section restrict access to baseyear/phaseIn regardless of permission
        If Not IsNothing(Session("boundarySelection")) Then
            Session("phaseInBaseYearAccess") = False
        End If


        '-----------------------------------
        ' SET SUBJECT MUNICIPALITY VARIABLE 'assign Subject Municipality selected for LTT to session variable
        '-----------------------------------
        If IsNothing(Session("boundarySelection")) Then
            Dim LTTSubjMun As String = ddlSubjMun.SelectedItem.ToString()
            Session.Add("LTTSubjectMunicipality", LTTSubjMun)
        Else
            Session.Add("LTTSubjectMunicipality", lblLiveSubjMun.Text)
        End If

		'added for saving SAMA_CODE		12-sep-2013
		If Not IsNothing(Session("LTTSubjectMunicipality")) Then
			dr.Close()
			Dim SAMA_CODE As String = ""
			query.CommandText = "select SAMA_CODE FROM [MunicipalitiesMapLink] INNER JOIN entities on [MunicipalitiesMapLink].PATMAP_Code = entities.number WHERE entities.jurisdiction = '" & Replace(Session("LTTSubjectMunicipality").ToString, "'", "''") & "'"
			dr = query.ExecuteReader()
			While dr.Read()
				SAMA_CODE = common.NullToStr(dr.GetValue(0))
			End While
			dr.Close()
			Session.Add("CODE_LTTSubjectMunicipality", SAMA_CODE)
		End If

				'--------------------------
        'SET DropDownList SELECTION
        '--------------------------
        If IsNothing(Session("LTTdropDownChoice")) Then
            Session.Add("LTTdropDownChoice", ddlSubjMun.SelectedValue)
        Else
            Session("LTTdropDownChoice") = ddlSubjMun.SelectedValue
        End If

        'clean up

        con.Close()

    End Sub

    Protected Sub validateLTTselection()

        Dim subjMunID As String
        Dim errorMsg As String
        Dim municipalityFound As Integer

        'set up database connection...
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString
        Dim query As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        query.Connection = con
        query.CommandTimeout = 60000
        con.Open()


        If IsNothing(Session("boundarySelection")) Then
            subjMunID = ddlSubjMun.SelectedValue.ToString()
            errorMsg = "PATMAP145" 'set error message (pulled from table: errorCodes)

            'If the user attempts to submit with the selection value "--Municipality--"
            If ddlSubjMun.Items(0).Selected Then
                LTTvalidated = False
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP145")
                Exit Sub
            End If

            'check to see if selected municipality is within the user's liveAssesment...Summary table
            'query.CommandText = "SELECT COUNT(*) FROM assessment WHERE assessmentID = (SELECT assessmentID FROM boundaryModel WHERE status = 1) AND municipalityID = '" & subjMunID & "'"
            query.CommandText = "SELECT TOP 1 assessmentID FROM assessment WHERE assessmentID = (SELECT assessmentID FROM boundaryModel WHERE status = 1) AND municipalityID = '" & subjMunID & "'"

            dr = query.ExecuteReader
            dr.Read()
            If dr.HasRows() Then
                municipalityFound = dr.GetValue(0)
            Else
                LTTvalidated = False
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP145")
                dr.Close()
                Exit Sub
            End If
            'clean up
            dr.Close()
        Else

            'user is enterting LTT from Boundary section. No need to check municipality validity.
            municipalityFound = 1

            query.CommandText = "select number from entities where jurisdiction = '" & Replace(lblLiveSubjMun.Text, "'", "''") & "'"
            dr = query.ExecuteReader
            dr.Read()
            subjMunID = dr.GetValue(0)
            'clean up
            dr.Close()

            errorMsg = "PATMAP146" 'set error message (pulled from table: errorCodes)
        End If

        'If the user attempts to submit with the selection value "--Municipality--"
        'If ddlSubjMun.Items(0).Selected Then
        '    LTTvalidated = False
        '    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP145")
        '    Exit Sub
        'End If

        ''check to see if selected municipality is within the user's liveAssesment...Summary table
        'query.CommandText = "SELECT COUNT(*) FROM assessment WHERE assessmentID = (SELECT assessmentID FROM boundaryModel WHERE status = 1) AND municipalityID = '" & subjMunID & "'"
        'dr = query.ExecuteReader
        'dr.Read()
        'Dim municipalityFound As Integer = dr.GetValue(0)

        ''clean up
        'dr.Close()

        If municipalityFound > 0 Then
            LTTvalidated = True
        Else
            Master.errorMsg = PATMAP.common.GetErrorMessage(errorMsg)
            LTTvalidated = False
        End If

    End Sub

    Protected Sub fillLiveLTTValues()

        'Dim userID As Integer = Session("userID")

        ''set up database connection...
        'Dim con As New SqlClient.SqlConnection
        'con.ConnectionString = PATMAP.Global_asax.connString
        'Dim query As New SqlClient.SqlCommand
        'query.Connection = con
        'con.Open()

        'Dim dt As DataTable
        'Dim counter As Integer
        'Dim taxClassID As String

        ''use dataTable created in setLiveTaxClasses procedure.
        'dt = CType(Session("LTTliveTaxClasses"), DataTable)

        'query.CommandText = "SELECT taxClassID FROM liveLTTtaxClasses_" & userID

        ''delete all entries from the table where the userID exisits, if applicable...
        'query.CommandText = "DELETE FROM liveLTTValues WHERE userID = '" & userID & "'" & vbCrLf

        'For counter = 0 To dt.Rows.Count - 1
        '    taxClassID = dt.Rows(counter).Item("taxClassID")
        '    query.CommandText += "INSERT INTO liveLTTValues VALUES (" & userID & ", '" & taxClassID & "' , 0, 0, 1.0000, 0, 1, 0, 1, 1, 1)" & vbCrLf
        'Next

        ''write to the liveLTTValues table
        'query.ExecuteNonQuery()

        'set up database connection...
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString
        Dim query As New SqlClient.SqlCommand
        query.Connection = con
        con.Open()

        'delete all entries from the table where the userID exisits, if applicable...
        query.CommandText = "DELETE FROM liveLTTValues WHERE userID = " & Session("userID").ToString & vbCrLf
        query.CommandText += "INSERT into liveLTTValues SELECT " & Session("userID").ToString & ",taxClassID,0,0,1.0000,0,1,0,1,1.0000,1 FROM LTTtaxClasses"

        'write to the liveLTTValues table
        query.ExecuteNonQuery()


    End Sub


    Protected Sub fillLiveLTTPhaseInValues()

        Dim userID As Integer = Session("userID")

        'set up database connection...
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString
        Dim query As New SqlClient.SqlCommand
        query.Connection = con
        con.Open()

        'delete all entries from the PhaseIn table where the userID exisits, if applicable...
        query.CommandText = "DELETE FROM liveLTTPhaseInValues WHERE userID = " & userID & vbCrLf

        'insert default values into the PhaseIn table based on userID and taxClass selection...
        query.CommandText += "INSERT into liveLTTPhaseInValues SELECT " & userID & ", taxClassID, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 FROM liveLTTtaxClasses_" & userID & " WHERE show = 1" & vbCrLf
        query.CommandText += "INSERT into liveLTTPhaseInValues SELECT " & userID & ", taxClassID, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 FROM LTTtaxClasses WHERE taxClassID NOT IN (select taxClassID from liveLTTtaxClasses_" & userID & " where show = 1)" & vbCrLf
        'query.CommandText += "INSERT into liveLTTPhaseInValues SELECT " & userID & ", taxClassID, 0, 0, .25, .25, .25, .25, .25, .25, .25, .25, 4, 4 FROM liveLTTtaxClasses_" & userID & " WHERE show = 1" & vbCrLf
        'query.CommandText += "INSERT into liveLTTPhaseInValues SELECT " & userID & ", taxClassID, 0, 0, .25, .25, .25, .25, .25, .25, .25, .25, 4, 4 FROM LTTtaxClasses"

        'write to the liveLTTPhaseInValues table
        query.ExecuteNonQuery()

    End Sub

    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad

    End Sub

    Private Sub ddlSubjMun_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSubjMun.SelectedIndexChanged
        'Clears out the error message
        Master.errorMsg = ""
    End Sub
End Class

