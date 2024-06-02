Public Partial Class edittaxclass
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Clears out the error message
            Master.errorMsg = ""

            'check if its the first load or a post back
            If Not Page.IsPostBack Then
                'Sets submenu to be displayed
                subMenu.setStartNode(menu.editData)

                'get entityID to edit
                Dim editTaxClassID As String
                editTaxClassID = Session("editTaxClassID")

                'setup database connection
                Dim con As New SqlClient.SqlConnection
                con.ConnectionString = PATMAP.Global_asax.connString
                Dim query As New SqlClient.SqlCommand
                query.Connection = con
                Dim dr As SqlClient.SqlDataReader
                con.Open()

                'fill the tax class list box
                Dim da As New SqlClient.SqlDataAdapter
                da.SelectCommand = New SqlClient.SqlCommand
                da.SelectCommand.Connection = con
                da.SelectCommand = New SqlClient.SqlCommand("select '0' as taxClassID,'<Select>' as taxClass union select taxClassID, taxClass from taxClasses where parentTaxClassID = 'none'", con)
                Dim dt As New DataTable
                da.Fill(dt)
                ddlMainClasses.DataSource = dt
                ddlMainClasses.DataValueField = "taxClassID"
                ddlMainClasses.DataTextField = "taxClass"
                ddlMainClasses.DataBind()

                'Coded by: Jereen
                '********               

                da = New SqlClient.SqlDataAdapter
                da.SelectCommand = New SqlClient.SqlCommand("select p.presentUseCodeID, p.shortDescription from presentUseCodes p where p.presentUseCodeID not in (select presentUseCodeID from taxClassesUpdated) order by p.shortDescription", con)
                dt = New DataTable
                da.Fill(dt)
                lstPropCodeLeft.DataSource = dt
                lstPropCodeLeft.DataValueField = "presentUseCodeID"
                lstPropCodeLeft.DataTextField = "shortDescription"
                lstPropCodeLeft.DataBind()
                '********

                'fill LTTrollupClass dropdownbox

                da = New SqlClient.SqlDataAdapter
                da.SelectCommand = New SqlClient.SqlCommand("SELECT taxClassID, taxClass FROM LTTmainTaxClasses WHERE active = 1", con)
                dt = New DataTable
                da.Fill(dt)
                ddlLTTRollup.DataSource = dt
                ddlLTTRollup.DataValueField = "taxClassID"
                ddlLTTRollup.DataTextField = "taxClass"
                ddlLTTRollup.DataBind()


                If editTaxClassID <> "0" Then
                    'get entity details from database
                    query.CommandText = "select taxClass, taxClassID, description, parentTaxClassID, [default], active, notes from taxClasses where taxClassID = '" & editTaxClassID & "'"
                    dr = query.ExecuteReader
                    dr.Read()

                    'fill entity detail into the form fields
                    txtTaxClass.Text = dr.GetValue(0)
                    Session.Add("taxClass", txtTaxClass.Text)
                    txtCode.Text = dr.GetValue(1)
                    txtDescription.Text = dr.GetValue(2)
                    If dr.GetValue(3) = "none" Then
                        rblType.Items(0).Selected = True
                        Session.Add("classType", "none")
                    Else
                        rblType.Items(1).Selected = True
                        ddlMainClasses.SelectedValue = dr.GetValue(3)
                        Session.Add("classType", dr.GetValue(3))
                    End If

                    If dr.GetValue(4) = True Then
                        rblDefault.Items(0).Selected = True
                    Else
                        rblDefault.Items(1).Selected = True
                    End If
                    If dr.GetValue(5) = True Then
                        rblActive.Items(0).Selected = True
                    Else
                        rblActive.Items(1).Selected = True
                    End If
                    txtNotes.Text = dr.GetValue(6)

                    'cleanup
                    dr.Close()

                    'set default dropdown selection for LTTrollup
                    query.CommandText = "SELECT l.taxClassID FROM LTTmainTaxClasses l INNER JOIN LTTtaxClasses ON LTTtaxClasses.LTTmainTaxClassID = l.taxClassID WHERE l.active = 1 AND LTTtaxClasses.taxClassID = '" & editTaxClassID & "'"
                    dr = query.ExecuteReader
                    dr.Read()
                    ddlLTTRollup.ClearSelection()
                    ddlLTTRollup.SelectedValue = dr.GetValue(0)
                    'cleanup
                    dr.Close()

                    'Donna Start
                    ddlPEMRRollup.DataBind()

                    'Set default dropdown selection for PEMR Rollup.
                    query.CommandText = "SELECT mainTaxClassID FROM PEMRTaxClasses WHERE taxClassID = '" & editTaxClassID & "'"
                    dr = query.ExecuteReader

                    If dr.Read() Then
                        ddlPEMRRollup.ClearSelection()
                        ddlPEMRRollup.SelectedValue = dr.GetValue(0)
                    End If

                    dr.Close()
                    'Donna End

                    da = New SqlClient.SqlDataAdapter
                    da.SelectCommand = New SqlClient.SqlCommand("select p.presentUseCodeID as presentUseCodeID, p.shortDescription as shortDescription from taxClassesUpdated t, presentUseCodes p where t.presentUseCodeID = p.presentUseCodeID and taxClassID = '" & editTaxClassID & "'", con)
                    dt = New DataTable
                    da.Fill(dt)
                    lstPropCodeRight.DataSource = dt
                    lstPropCodeRight.DataValueField = "presentUseCodeID"
                    lstPropCodeRight.DataTextField = "shortDescription"
                    lstPropCodeRight.DataBind()
                Else
                    'default values for the check boxes when adding a new tax class 
                    rblDefault.Items(1).Selected = True
                    rblActive.Items(1).Selected = True
                End If

                'clean up
                con.Close()
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try

    End Sub

    Protected Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSave.Click
        Try
            'make sure required fields are filled out
            If Trim(txtTaxClass.Text) = "" Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP25")
                Exit Sub
            End If
            If Trim(txtCode.Text) = "" Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP26")
                Exit Sub
            End If
            If rblType.Items(0).Selected = False Then
                If rblType.Items(1).Selected = False Then
                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP27")
                    Exit Sub
                Else
                    If (String.IsNullOrEmpty(ddlMainClasses.SelectedValue) Or ddlMainClasses.SelectedValue = "0") Then
                        Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP28")
                        Exit Sub
                    End If
                End If
            End If

            'remove any single quotes from fields
            txtTaxClass.Text = txtTaxClass.Text.Replace("'", "''")
            txtCode.Text = txtCode.Text.Replace("'", "''")
            txtDescription.Text = txtDescription.Text.Replace("'", "''")
            txtNotes.Text = txtNotes.Text.Replace("'", "''")

            'get groupID to edit
            Dim editTaxClassID As String
            editTaxClassID = Session("editTaxClassID")

            'setup database connection
            Dim con As New SqlClient.SqlConnection
            con.ConnectionString = PATMAP.Global_asax.connString
            Dim query As New SqlClient.SqlCommand
            query.Connection = con
            con.Open()

            'convert form values to database values
            Dim parenttaxclassid As String
            If rblType.Items(0).Selected = True Then
                parenttaxclassid = "none"
            Else
                parenttaxclassid = ddlMainClasses.SelectedValue
            End If

            Dim chkDefault As Integer
            If rblDefault.Items(0).Selected = True Then
                chkDefault = 1
            Else
                chkDefault = 0
            End If
            Dim chkActive As Integer
            If rblActive.Items(0).Selected = True Then
                chkActive = 1
            Else
                chkActive = 0
            End If

            'build add/update string      
            Dim dReader As SqlClient.SqlDataReader
            If editTaxClassID = "0" Then
                query.CommandText = "select taxClassID, taxClass from taxClasses where taxClassID='" & Trim(txtCode.Text) & "' or taxClass='" & Trim(txtTaxClass.Text) & "'"
                dReader = query.ExecuteReader()
                If dReader.Read() Then
                    If dReader.GetValue(0) = Trim(txtCode.Text) Then
                        Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP122")
                        dReader.Close()
                        con.Close()
                        Exit Sub
                    End If
                    If dReader.GetValue(1) = Trim(txtTaxClass.Text) Then
                        Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP123")
                        dReader.Close()
                        con.Close()
                        Exit Sub
                    End If
                End If
                dReader.Close()

                'insert as a main class
                Dim sort As Integer = 0
                If parenttaxclassid = "none" Then
                    query.CommandText = "select max(sort) from taxClasses"
                    dReader = query.ExecuteReader
                    dReader.Read()
                    sort = dReader.GetInt32(0) + 1
                    dReader.Close()
                    query.CommandText = ""
                Else
                    'insert as a sub class
                    query.CommandText = "select max(sort) from taxClasses where taxClassID='" & parenttaxclassid & "' or parentTaxClassID = '" & parenttaxclassid & "'"
                    dReader = query.ExecuteReader
                    dReader.Read()
                    sort = dReader.GetInt32(0) + 1
                    dReader.Close()

                    query.CommandText = "update taxclasses set sort = sort + 1 where sort >= " & sort & vbCrLf
                End If

                query.CommandText += "insert into taxclasses (taxClassID, taxClass, description, parentTaxClassID, [default], active, notes, sort) values ('" & Trim(txtCode.Text) & "','" & Trim(txtTaxClass.Text) & "','" & Trim(txtDescription.Text) & "','" & parenttaxclassid & "'," & chkDefault & "," & chkActive & ",'" & Trim(txtNotes.Text) & "'," & sort & ")" & vbCrLf

                '****temp variable for sort order to add to LTT
                Session.Add("LTTsort", sort)
            Else
                query.CommandText = "select count(taxClassID) from taxClasses where taxClassID='" & Trim(txtCode.Text) & "'"
                dReader = query.ExecuteReader()
                dReader.Read()
                If editTaxClassID <> Trim(txtCode.Text) Then
                    If dReader.GetValue(0) >= 1 Then
                        Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP122")
                        dReader.Close()
                        con.Close()
                        Exit Sub
                    End If
                    dReader.Close()
                Else
                    If dReader.GetValue(0) > 1 Then
                        Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP122")
                        dReader.Close()
                        con.Close()
                        Exit Sub
                    End If
                    dReader.Close()
                End If


                query.CommandText = "select count(taxClass) from taxClasses where taxClass='" & Trim(txtTaxClass.Text) & "'"
                dReader = query.ExecuteReader()
                dReader.Read()
                If Session("taxClass") <> Trim(txtTaxClass.Text) Then
                    If dReader.GetValue(0) >= 1 Then
                        Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP123")
                        dReader.Close()
                        con.Close()
                        Exit Sub
                    End If
                    dReader.Close()
                Else
                    If dReader.GetValue(0) > 1 Then
                        Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP123")
                        dReader.Close()
                        con.Close()
                        Exit Sub
                    End If
                    dReader.Close()
                End If

                If Session("classType") <> parenttaxclassid Then
                    'subclass to subclass
                    If Session("classType") <> "none" And parenttaxclassid <> "none" Then
                        query.CommandText = "select sort from taxClasses where taxClassID='" & Trim(txtCode.Text) & "'" & vbCrLf
                        query.CommandText += "select max(sort) from taxClasses where taxClassID='" & parenttaxclassid & "' or parentTaxClassID = '" & parenttaxclassid & "'"
                        dReader = query.ExecuteReader()
                        dReader.Read()
                        Dim sortA As Integer = 0
                        sortA = dReader.GetInt32(0)
                        Dim sortB As Integer = 0
                        dReader.NextResult()
                        dReader.Read()
                        sortB = dReader.GetInt32(0) + 1
                        dReader.Close()

                        If sortB < sortA Then
                            query.CommandText = "update taxclasses set sort = sort + 1 where sort >= " & sortB & " and sort < " & sortA & vbCrLf
                            query.CommandText += "update taxclasses set sort = " & sortB & " where  taxClassID='" & Trim(txtCode.Text) & "'" & vbCrLf
                        End If

                        If sortB > sortA Then
                            query.CommandText = "update taxclasses set sort = sort - 1 where sort > " & sortA & " and sort < " & sortB & vbCrLf
                            query.CommandText += "update taxclasses set sort = " & sortB - 1 & " where  taxClassID='" & Trim(txtCode.Text) & "'" & vbCrLf
                        End If
                    End If
                    'subclass to mainclass
                    If Session("classType") <> "none" And parenttaxclassid = "none" Then
                        query.CommandText = "select sort from taxClasses where taxClassID='" & Trim(txtCode.Text) & "'"
                        dReader = query.ExecuteReader
                        dReader.Read()
                        Dim sortA As Integer = 0
                        sortA = dReader.GetInt32(0)
                        dReader.Close()

                        query.CommandText = "update taxclasses set sort = sort - 1 where sort > " & sortA & vbCrLf
                        query.CommandText += "update taxclasses set sort = (select max(sort) from taxclasses) + 1 where taxClassID='" & Trim(txtCode.Text) & "'"
                    End If
                    'mainclass to subclass
                    If Session("classType") = "none" And parenttaxclassid <> "none" Then
                        query.CommandText = "select taxClassID from taxclasses where parentTaxClassID='" & Trim(txtCode.Text) & "'" & vbCrLf
                        dReader = query.ExecuteReader
                        If dReader.Read() Then
                            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP124")
                            dReader.Close()
                            con.Close()
                            Exit Sub
                        End If
                        dReader.Close()

                        query.CommandText = "select sort from taxClasses where taxClassID='" & Trim(txtCode.Text) & "'" & vbCrLf
                        query.CommandText += "select max(sort) from taxClasses where taxClassID='" & parenttaxclassid & "' or parentTaxClassID = '" & parenttaxclassid & "'"
                        dReader = query.ExecuteReader()
                        dReader.Read()
                        Dim sortA As Integer = 0
                        sortA = dReader.GetInt32(0)
                        Dim sortB As Integer = 0
                        dReader.NextResult()
                        dReader.Read()
                        sortB = dReader.GetInt32(0) + 1
                        dReader.Close()

                        If sortB < sortA Then
                            query.CommandText = "update taxclasses set sort = sort + 1 where sort >= " & sortB & " and sort < " & sortA & vbCrLf
                            query.CommandText += "update taxclasses set sort = " & sortB & " where  taxClassID='" & Trim(txtCode.Text) & "'" & vbCrLf
                        End If

                        If sortB > sortA Then
                            query.CommandText = "update taxclasses set sort = sort - 1 where sort > " & sortA & " and sort < " & sortB & vbCrLf
                            query.CommandText += "update taxclasses set sort = " & sortB - 1 & " where  taxClassID='" & Trim(txtCode.Text) & "'" & vbCrLf
                        End If
                    End If
                Else
                    query.CommandText = ""
                End If

                'if the parent class is inactive, then set all the subclasses to inactive
                If parenttaxclassid = "none" Then
                    query.CommandText += "update taxclasses set active=" & chkActive & " where parentTaxClassID='" & Trim(txtCode.Text) & "'" & vbCrLf
                End If

                query.CommandText += "update taxclasses set taxClassID = '" & Trim(txtCode.Text) & "', taxClass = '" & Trim(txtTaxClass.Text) & "', description = '" & Trim(txtDescription.Text) & "', parentTaxClassID = '" & parenttaxclassid & "', [default] = " & chkDefault & ", active = " & chkActive & ", notes = '" & Trim(txtNotes.Text) & "' where taxClassID = '" & editTaxClassID & "'" & vbCrLf
            End If

            'updates tax year model data sets (POV, Tax Credit, PMR) and user level permission
            If editTaxClassID = "0" Then
                query.CommandText += updateLiveModelTables(Trim(txtCode.Text), parenttaxclassid)
            End If

            '********
            'get the list of presentusecodes for which the tax class id has been updated
            Dim da As New SqlClient.SqlDataAdapter
            Dim dt As New DataTable
            da.SelectCommand = New SqlClient.SqlCommand()
            da.SelectCommand.Connection = con
            da.SelectCommand.CommandText = "select presentUseCodeID from taxClassesUpdated where taxClassID='" & editTaxClassID & "'"
            da.Fill(dt)
            dt.PrimaryKey = New DataColumn() {dt.Columns("presentUseCodeID")}

            Dim dr As DataRow
            Dim index As Integer
            Dim list As String = ""

            If editTaxClassID <> "0" Then
                'find out for which present use codes, the tax class id has been updated to the original
                'sqlStrUpdateTaxClassID = "update assessment set taxClassID=(select a2.taxClassID_orig from assessment a2 where a2.presentUseCodeID = assessment.presentUseCodeID group by a2.taxClassID_orig) where presentUseCodeID in (-1"            
                For index = 0 To dt.Rows.Count - 1 Step 1
                    dr = dt.Rows(index)
                    Dim lstItem As ListItem = lstPropCodeRight.Items.FindByValue(dr(0))
                    If IsNothing(lstItem) Then
                        list = CType(dr(0), String) + ","
                    End If
                Next
                If list <> "" Then
                    list = "(" & list.Remove(list.Length - 1, 1) & ")"
                    query.CommandText += "update assessment set taxClassID=taxClassID_orig from assessment where taxClassID='" & editTaxClassID & "' and presentUseCodeID in " & list & vbCrLf
                    query.CommandText += "delete taxClassesUpdated where taxClassID='" & editTaxClassID & "' AND presentUseCodeID in " & list & vbCrLf
                    'query.CommandText += "update assessmentTaxModel set dataStale='True' from assessmentTaxModel a where BaseTaxYearModelID in (select taxYearModelID from taxYearModelDescription t join assessment a on t.assessmentID = a.assessmentID where a.presentUseCodeID in " & list & ") OR SubjectTaxYearModelID in (select taxYearModelID from taxYearModelDescription t join assessment a on t.assessmentID = a.assessmentID where a.presentUseCodeID in " & list & ")" & vbCrLf
                    query.CommandText += "update assessmentDescription set dataStale = 1 where assessmentID in (select assessmentID from assessment where presentUseCodeID in " & list & " group by assessmentID)" & vbCrLf
                End If

                'find out for which present use codes, the tax class id has been updated to a new value
                index = 0
                list = ""
                For index = 0 To lstPropCodeRight.Items.Count - 1 Step 1
                    If dt.Rows.Contains(lstPropCodeRight.Items(index).Value) = False Then
                        query.CommandText += "insert into taxClassesUpdated Values(" & lstPropCodeRight.Items(index).Value & ", '" & Trim(txtCode.Text) & "')" & vbCrLf
                        list = list + lstPropCodeRight.Items(index).Value + ","
                    End If
                Next
                If list <> "" Then
                    list = "(" & list.Remove(list.Length - 1, 1) & ")"
                    query.CommandText += "update assessment set taxClassID='" & editTaxClassID & "' where presentUseCodeID in " & list & vbCrLf
                    'query.CommandText += "update assessmentTaxModel set dataStale='True' from assessmentTaxModel a where BaseTaxYearModelID in (select taxYearModelID from taxYearModelDescription t join assessment a on t.assessmentID = a.assessmentID where a.presentUseCodeID in " & list & ") OR SubjectTaxYearModelID in (select taxYearModelID from taxYearModelDescription t join assessment a on t.assessmentID = a.assessmentID where a.presentUseCodeID in " & list & ")" & vbCrLf
                    query.CommandText += "update assessmentDescription set dataStale = 1 where assessmentID in (select assessmentID from assessment where presentUseCodeID in " & list & " group by assessmentID)" & vbCrLf
                End If
            End If

            If editTaxClassID = "0" Then
                'find out for which present use codes, the tax class id has been updated to a new value
                index = 0
                list = ""
                For index = 0 To lstPropCodeRight.Items.Count - 1 Step 1
                    If dt.Rows.Contains(lstPropCodeRight.Items(index).Value) = False Then
                        query.CommandText += "insert into taxClassesUpdated Values(" & lstPropCodeRight.Items(index).Value & ", '" & Trim(txtCode.Text) & "')" & vbCrLf
                        list = list + lstPropCodeRight.Items(index).Value + ","
                    End If
                Next
                If list <> "" Then
                    list = "(" & list.Remove(list.Length - 1, 1) & ")"
                    query.CommandText += "update assessment set taxClassID='" & Trim(txtCode.Text) & "' where presentUseCodeID in " & list & vbCrLf
                    'query.CommandText += "update taxYearModelDescription set dataStale='True' from taxYearModelDescription t join assessment a on t.assessmentID = a.assessmentID where a.presentUseCodeID in " & list & vbCrLf
                    query.CommandText += "update assessmentDescription set dataStale = 1 where assessmentID in (select assessmentID from assessment where presentUseCodeID in " & list & " group by assessmentID)" & vbCrLf
                End If
            End If
            '********


            '***** Start of Inky's LTT Code *****
            If editTaxClassID = "0" Then
                query.CommandText += "INSERT INTO LTTtaxClasses (taxClassID, taxClass, description, parentTaxClassID, [default], active, notes, sort, LTTmainTaxClassID) VALUES ('" & Trim(txtCode.Text) & "','" & Trim(txtTaxClass.Text) & "','" & Trim(txtDescription.Text) & "','" & parenttaxclassid & "'," & chkDefault & "," & chkActive & ",'" & Trim(txtNotes.Text) & "'," & Session("LTTsort") & "," & ddlLTTRollup.SelectedValue.ToString() & ")" & vbCrLf
                query.CommandText += "INSERT INTO PEMRTaxClasses (taxClassID, taxClass, active, mainTaxClassID, originalMainTaxClassID) SELECT '" & Trim(txtCode.Text) & "', '" & Trim(txtTaxClass.Text) & "', " & chkActive & ", " & ddlPEMRRollup.SelectedValue.ToString() & ", " & ddlPEMRRollup.SelectedValue.ToString() & vbCrLf
            Else
                query.CommandText += "UPDATE LTTtaxClasses SET taxClassID = '" & Trim(txtCode.Text) & "', taxClass = '" & Trim(txtTaxClass.Text) & "', description = '" & Trim(txtDescription.Text) & "', parentTaxClassID = '" & parenttaxclassid & "', [default] = " & chkDefault & ", active = " & chkActive & ", notes = '" & Trim(txtNotes.Text) & "', LTTmainTaxClassID = '" & ddlLTTRollup.SelectedValue.ToString() & "' WHERE taxClassID = '" & editTaxClassID & "'" & vbCrLf
                'query.CommandText += "UPDATE PEMRTaxClasses SET taxClassID = '" & Trim(txtCode.Text) & "', taxClass = '" & Trim(txtTaxClass.Text) & "', active = " & chkActive & ", mainTaxClassID = " & ddlPEMRRollup.SelectedValue.ToString() & " WHERE taxClassID = '" & editTaxClassID & "'" & vbCrLf
                'query.CommandText += "UPDATE assessmentTaxModel SET dataStale = 1" & vbCrLf
                'query.CommandText += "UPDATE liveAssessmentTaxModel SET dataStale = 1" & vbCrLf
            End If
            Session.Remove("LTTsort")
            '****** End of Inky's LTT Code ******


            Dim trans As SqlClient.SqlTransaction
            trans = con.BeginTransaction()
            query.Transaction = trans

            Try
                query.CommandTimeout = 6000
                query.ExecuteNonQuery()
                trans.Commit()
            Catch
                trans.Rollback()
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP101")
                con.Close()
                Exit Sub
            End Try

            'Donna start
            Try
                query.CommandText = "PEMRTaxClassesUpdate"
                query.CommandType = CommandType.StoredProcedure

                With query.Parameters
                    .AddWithValue("@taxClassID", txtCode.Text.Trim)
                    .AddWithValue("@taxClass", txtTaxClass.Text.Trim)
                    .AddWithValue("@active", chkActive)
                    .AddWithValue("@mainTaxClassID", ddlPEMRRollup.SelectedValue.ToString())
                    .AddWithValue("@originalTaxClassID", editTaxClassID)
                End With

                query.ExecuteNonQuery()
            Catch
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP101")

                Exit Sub
            Finally
                If con IsNot Nothing Then
                    con.Close()
                End If
            End Try
            'Donna end

            'clean up
            'con.Close()
            Session.Remove("editTaxClassID")
            Session.Remove("taxClass")
            Session.Remove("classType")

        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        Response.Redirect("viewtaxclass.aspx")

    End Sub

    Protected Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCancel.Click
        Session.Remove("editTaxClassID")
        Session.Remove("taxClass")
        Session.Remove("classType")
        Response.Redirect("viewtaxclass.aspx")
    End Sub

    Private Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Try
            'change page title and breadcrumb to 
            'if the mode is either edit or add
            common.ChangeTitle(Session("editTaxClassID"), lblTitle)
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            common.UndoChange()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub btnRemoveMember_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnRemoveMember.Click
        Try
            lstPropCodeLeft.Items.Add(lstPropCodeRight.SelectedItem)
            lstPropCodeRight.Items.Remove(lstPropCodeRight.SelectedItem)

            lstPropCodeLeft.SelectedIndex = -1
            lstPropCodeLeft.SelectedIndex = -1
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub btnAddMember_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAddMember.Click
        Try
            lstPropCodeRight.Items.Add(lstPropCodeLeft.SelectedItem)
            lstPropCodeLeft.Items.Remove(lstPropCodeLeft.SelectedItem)

            lstPropCodeRight.SelectedIndex = -1
            lstPropCodeRight.SelectedIndex = -1
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Function updateLiveModelTables(ByVal taxClassID As String, ByVal parenttaxclassid As String) As String
        'setup database connection
        Dim con As New SqlClient.SqlConnection
        Dim conTemp As New SqlClient.SqlConnection
        Dim query As New SqlClient.SqlCommand
        Dim queryTemp As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim drTemp As SqlClient.SqlDataReader
        Dim sql As String = ""

        con.ConnectionString = PATMAP.Global_asax.connString
        query.Connection = con
        con.Open()

        conTemp.ConnectionString = PATMAP.Global_asax.connString
        queryTemp.Connection = conTemp
        conTemp.Open()

        'update POV table
        query.CommandText = "select POVID from POVDescription"
        dr = query.ExecuteReader

        Dim POV As Decimal = 0.0
        While dr.Read()
            queryTemp.CommandText = "select POV from POV where POVID = " & dr.Item(0) & " and taxClassID = '" & parenttaxclassid & "'"
            drTemp = queryTemp.ExecuteReader
            If drTemp.Read() Then
                POV = drTemp.GetValue(0)
                drTemp.Close()
                sql &= "insert into POV (POVID,taxClassID,POV) values (" & dr.Item(0) & ",'" & taxClassID & "'," & POV & ")" & vbCrLf
            Else
                sql &= "insert into POV (POVID,taxClassID,POV) values (" & dr.Item(0) & ",'" & taxClassID & "',1)" & vbCrLf
            End If
            drTemp.Close()
        End While

        dr.Close()

        'update Tax Credit table
        query.CommandText = "select taxCreditID from taxCreditDescription"
        dr = query.ExecuteReader

        Dim taxCredit As Decimal = 0.0
        Dim capped As Decimal = 0.0
        While dr.Read()
            queryTemp.CommandText = "select taxCredit, capped from taxCredit where taxCreditID = " & dr.Item(0) & " and taxClassID = '" & parenttaxclassid & "'"
            drTemp = queryTemp.ExecuteReader
            If drTemp.Read() Then
                taxCredit = drTemp.GetValue(0)
                capped = drTemp.GetValue(1)
                drTemp.Close()
                sql &= "insert into taxCredit (taxCreditID,taxClassID,taxCredit,capped) values (" & dr.Item(0) & ",'" & taxClassID & "'," & taxCredit & "," & capped & ")" & vbCrLf
            Else
                sql &= "insert into taxCredit (taxCreditID,taxClassID,taxCredit,capped) values (" & dr.Item(0) & ",'" & taxClassID & "',1,0)" & vbCrLf
            End If
            drTemp.Close()
        End While

        dr.Close()

        'update PMR table
        query.CommandText = "select PMRID from PMRDescription"
        dr = query.ExecuteReader

        Dim PMR As Decimal = 0.0
        Dim PMRReplacement As Integer = 0
        Dim assessmentInclude As Integer = 0
        While dr.Read()
            queryTemp.CommandText = "select PMR, PMRReplacement, assessmentInclude from PMR where PMRID = " & dr.Item(0) & " and taxClassID = '" & parenttaxclassid & "'"
            drTemp = queryTemp.ExecuteReader
            If drTemp.Read() Then
                PMR = drTemp.GetValue(0)
                PMRReplacement = drTemp.GetValue(1)
                assessmentInclude = drTemp.GetValue(2)
                drTemp.Close()
                sql &= "insert into PMR (PMRID,taxClassID,PMR,PMRReplacement,assessmentInclude) values (" & dr.Item(0) & ",'" & taxClassID & "'," & PMR & "," & PMRReplacement & "," & assessmentInclude & ")" & vbCrLf
            Else
                sql &= "insert into PMR (PMRID,taxClassID,PMR,PMRReplacement,assessmentInclude) values (" & dr.Item(0) & ",'" & taxClassID & "',0,0,1)" & vbCrLf
            End If
            drTemp.Close()
        End While

        dr.Close()

        'update EDPOV table
        query.CommandText = "select EDPOVID from EDPOVDescription"
        dr = query.ExecuteReader

        Dim EDPOV As Decimal = 0.0
        Dim EDPOVFactor As Integer = 0
        While dr.Read()
            queryTemp.CommandText = "select EDPOV, EDPOVFactor from EDPOV where EDPOVID = " & dr.Item(0) & " and taxClassID = '" & parenttaxclassid & "'"
            drTemp = queryTemp.ExecuteReader
            If drTemp.Read() Then
                POV = drTemp.GetValue(0)
                EDPOVFactor = drTemp.GetValue(1)
                drTemp.Close()
                sql &= "insert into EDPOV (EDPOVID,taxClassID,EDPOV, EDPOVFactor) values (" & dr.Item(0) & ",'" & taxClassID & "'," & POV & "," & EDPOVFactor & ")" & vbCrLf
            Else
                sql &= "insert into EDPOV (EDPOVID,taxClassID,EDPOV, EDPOVFactor) values (" & dr.Item(0) & ",'" & taxClassID & "',1,0)" & vbCrLf
            End If
            drTemp.Close()
        End While

        dr.Close()


        'update PMR table
        query.CommandText = "select levelID from levels"
        dr = query.ExecuteReader

        Dim access As Integer = 0
        While dr.Read()
            queryTemp.CommandText = "select access from taxClassesPermission where levelID = " & dr.Item(0) & " and taxClassID = '" & parenttaxclassid & "'"
            drTemp = queryTemp.ExecuteReader
            If drTemp.Read() Then
                access = drTemp.GetValue(0)
                drTemp.Close()
                If dr.Item(0) = 1 Then
                    sql &= "insert into taxClassesPermission (levelID,taxClassID,access) values (" & dr.Item(0) & ",'" & taxClassID & "',1)" & vbCrLf
                Else
                    sql &= "insert into taxClassesPermission (levelID,taxClassID,access) values (" & dr.Item(0) & ",'" & taxClassID & "',0)" & vbCrLf
                End If
            Else
                sql &= "insert into taxClassesPermission (levelID,taxClassID,access) values (" & dr.Item(0) & ",'" & taxClassID & "',1)" & vbCrLf
            End If
            drTemp.Close()
        End While

        dr.Close()

        Return sql
    End Function

End Class