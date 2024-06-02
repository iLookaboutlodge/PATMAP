Public Partial Class editlevel
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            'Clears out the error message
            Master.errorMsg = ""

            If Not IsPostBack Then

                'Sets submenu to be displayed
                subMenu.setStartNode(menu.userGroup)

                grdPermission.PageSize = PATMAP.Global_asax.pageSize

                'get User Level ID
                Dim editLevelID As Integer
                editLevelID = Session("editLevelID")

                'setup database connection
                Dim con As New SqlClient.SqlConnection
                con.ConnectionString = PATMAP.Global_asax.connString

                Dim query As New SqlClient.SqlCommand
                query.Connection = con
                Dim dr As SqlClient.SqlDataReader

                con.Open()

                If editLevelID <> 0 Then

                    'get user level details from database
                    query.CommandText = "select levelName, description, notes from levels where levelID = " & editLevelID
                    dr = query.ExecuteReader
                    dr.Read()

                    'fill detail into form fields
                    If Not IsDBNull(dr.GetValue(0)) Then
                        txtUserLevel.Text = dr.GetValue(0)
                    End If

                    If Not IsDBNull(dr.GetValue(1)) Then
                        txtDescription.Text = dr.GetValue(1)
                    End If

                    If Not IsDBNull(dr.GetValue(2)) Then
                        txtNotes.Text = dr.GetValue(2)
                    End If

                    'cleanup
                    dr.Close()

                End If

                fillPermission()

                fillTaxClass()

                getLTTSelection() 'LTT TAX CLASS SELECTION

                con.Close()

            End If

        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try


    End Sub

    Private Sub fillPermission()
        'get User Level ID
        Dim editLevelID As Integer
        editLevelID = Session("editLevelID")

        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString

        'get permissions
        Dim da As New SqlClient.SqlDataAdapter
        Dim query As String = ""
        Dim dt As New DataTable

        da.SelectCommand = New SqlClient.SqlCommand()
        da.SelectCommand.Connection = con

        Dim access As Integer

        If editLevelID = 0 Then
            access = 1
        Else
            access = 0
        End If

        '***Inky's Update: Apr-2010***
        query += "select screenNames.screenNameID, screenNames.screenName, sections.sectionID, sections.sectionName, isNull(levelsPermission.access," & access & ") as access" & vbCrLf
        query += "from screenNames" & vbCrLf
        query += "left join sections on sections.sectionID = screenNames.sectionID" & vbCrLf
        query += "left join levelsPermission on levelsPermission.screenNameID = screenNames.screenNameID and levelsPermission.levelID = " & editLevelID & vbCrLf
        query += "where pageAddress not like '%/edit%' and (sections.sectionID not in (1,2,7,8) or screenNames.screenNameID = 90)" & vbCrLf
        query += "and screenNames.screenNameID not in ('6', '47', '15', '77', '56', '90')" & vbCrLf 'removed access to K12-OG and TaxCredit screens '***Inky removed ID '90' Satellite screen (May-2010). No longer part of PATMAP.
        query += "order by sections.sectionID, screenNames.screenName"

        ''''''''''query = "select screenNames.screenNameID, screenNames.screenName, sections.sectionID, sections.sectionName, isNull(levelsPermission.access," & access & ") as access" & vbCrLf & _
        ''''''''''         "from screenNames" & vbCrLf & _
        ''''''''''         "left join sections on sections.sectionID = screenNames.sectionID" & vbCrLf & _
        ''''''''''         "left join levelsPermission on levelsPermission.screenNameID = screenNames.screenNameID and levelsPermission.levelID = " & editLevelID & vbCrLf & _
        ''''''''''         "where pageAddress not like '%/edit%' and (sections.sectionID not in (1,2,7,8) or screenNames.screenNameID = 90)" & vbCrLf & _
        ''''''''''         "order by sections.sectionID, screenNames.screenName"
        '*** Inky:End ***

        da.SelectCommand.CommandText = query

        con.Open()
        da.Fill(dt)
        con.Close()

        grdPermission.DataSource = dt
        grdPermission.DataBind()

        If IsNothing(Cache("levelPermission")) Then
            Cache.Add("levelPermission", dt, Nothing, Now().AddMinutes(PATMAP.Global_asax.cacheExpiration), Nothing, CacheItemPriority.Low, Nothing)
        Else
            Cache("levelPermission") = dt
        End If

        txtTotal.Text = dt.Rows.Count

        If dt.Rows.Count = 0 Then
            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP134")
        End If

    End Sub

    Private Sub fillTaxClass()
        'get User Level ID
        Dim editLevelID As Integer
        editLevelID = Session("editLevelID")

        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString

        'get permissions
        Dim da As New SqlClient.SqlDataAdapter
        Dim query As String = ""
        Dim dt As New DataTable

        da.SelectCommand = New SqlClient.SqlCommand()
        da.SelectCommand.Connection = con

        Dim access As Integer

        If editLevelID = 0 Then
            access = 1
        Else
            access = 0
        End If

        query = "select taxClass.*, isNull(taxClassesPermission.access," & access & ") as access" & vbCrLf & _
                "from" & vbCrLf & _
                "(" & vbCrLf & _
                "	select taxClassID, taxClass, parentTaxClassID, sort from taxClasses where [active] = 1 and parentTaxClassID = 'none' union" & vbCrLf & _
                "	select taxClassID, taxClass, parentTaxClassID, sort from taxClasses where [active] = 1 and parentTaxClassID <> 'none'" & vbCrLf & _
                ") taxClass" & vbCrLf & _
                "left join taxClassesPermission on taxClassesPermission.taxClassID = taxClass.taxClassID and taxClassesPermission.levelID = " & editLevelID & vbCrLf & _
                "order by taxClass.sort"

        da.SelectCommand.CommandText = query

        con.Open()
        da.Fill(dt)
        con.Close()

        grdClasses.DataSource = dt
        grdClasses.DataBind()

        txtTotalClass.Text = dt.Rows.Count

    End Sub

    Private Sub getLTTSelection()
        'get User Level ID
        Dim editLevelID As Integer
        editLevelID = Session("editLevelID")

        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString

        'get permissions
        Dim dr As SqlClient.SqlDataReader
        Dim query As New SqlClient.SqlCommand
        query.Connection = con
        con.Open()

        query.CommandText = "SELECT LTTfullView FROM LTTlevels WHERE levelID = '" & Session("editLevelID") & "'"
        dr = query.ExecuteReader
        dr.Read()

        If dr.HasRows Then
            If dr.GetValue(0) = True Then
                chkShowLTTtaxClasses.Checked = True
            Else
                chkShowLTTtaxClasses.Checked = False
            End If
        Else
            chkShowLTTtaxClasses.Checked = False
        End If

        con.Close()

    End Sub

    Private Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Try
            'change page title and breadcrumb to 
            'if the mode is either edit or add
            common.ChangeTitle(Session("editLevelID"), lblTitle)
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

    Private Sub grdPermission_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdPermission.PageIndexChanging
        Try
            Dim dt As DataTable

            'sets grid's page index
            grdPermission.PageIndex = e.NewPageIndex

            'fill the users level grid
            If Not IsNothing(Cache("levelPermission")) Then
                updateDataSet()
                dt = CType(Cache("levelPermission"), DataTable)
                grdPermission.DataSource = dt
                grdPermission.DataBind()
            Else
                fillPermission()
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub grdPermission_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdPermission.Sorting
        Try
            Dim dt As DataTable
            Dim dv As DataView

            'fill the grid if there's no data stored in cache
            If IsNothing(Cache("levelPermission")) Then
                fillPermission()
            Else
                updateDataSet()
            End If

            dt = CType(Cache("levelPermission"), DataTable)
            dv = dt.DefaultView
            'sort grid
            dv.Sort = common.SortGrid(e.SortExpression, dv.Sort)
            grdPermission.DataSource = dt
            grdPermission.DataBind()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub


    'updateDataSet()
    'Accepts no parameter
    'Updates selection in the data set
    Private Sub updateDataSet()
        Dim counter As Integer
        Dim formField As System.Web.UI.WebControls.CheckBox
        Dim dt As DataTable
        Dim dr() As DataRow
        Dim selected As Integer

        dt = CType(Cache("levelPermission"), DataTable)

        'iterates through grid's row
        For counter = 0 To grdPermission.Rows.Count - 1

            If (grdPermission.Rows(counter).RowType = DataControlRowType.DataRow) Then
                'selects current row
                grdPermission.SelectedIndex = counter

                'gets values entered in the textbox
                formField = CType(grdPermission.Rows(counter).Cells(0).FindControl("ckbAccess"), CheckBox)

                If formField.Checked Then
                    selected = 1
                Else
                    selected = 0
                End If

                'selects the tax class from the data set
                dr = dt.Select("screenNameID = " & grdPermission.SelectedDataKey.Value.ToString())

                'updates tax class's value
                If dr.Length > 0 Then
                    dr(0).Item("access") = selected
                End If

            End If

        Next

    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSave.Click
        Try
            'make sure required fields are filled out
            If String.IsNullOrEmpty(Trim(txtUserLevel.Text)) Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP17")
                Exit Sub
            End If

            If Not common.ValidateNoSpecialChar(Trim(txtUserLevel.Text)) Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP90")
                Exit Sub
            End If

            'do any error checking of the form fields
            txtUserLevel.Text = Trim(txtUserLevel.Text.Replace("'", "''"))
            txtDescription.Text = Trim(txtDescription.Text.Replace("'", "''"))
            txtNotes.Text = Trim(txtNotes.Text.Replace("'", "''"))

            ''get User Level ID
            Dim editLevelID As Integer
            editLevelID = Session("editLevelID")

            'setup database connection
            Dim con As New SqlClient.SqlConnection
            con.ConnectionString = PATMAP.Global_asax.connString
            Dim query As New SqlClient.SqlCommand
            Dim dr As SqlClient.SqlDataReader
            Dim sql As String = ""

            query.Connection = con

            con.Open()

            query.CommandText = "select levelName from levels where levelName = '" & Trim(txtUserLevel.Text) & "' and levelID <> " & editLevelID
            dr = query.ExecuteReader()

            If dr.Read() Then
                dr.Close()
                con.Close()
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP108")
                Exit Sub
            End If

            dr.Close()

            'set LTT view value
            Dim intLTTfullView As Integer

            If chkShowLTTtaxClasses.Checked = True Then
                intLTTfullView = 1
            Else
                intLTTfullView = 0
            End If

            'save the User Level details
            If editLevelID = 0 Then
                'insert User Level 
                query.CommandText = "insert into levels (levelName, description, notes) values ('" & Trim(txtUserLevel.Text) & "','" & Trim(txtDescription.Text) & "','" & Trim(txtNotes.Text) & "')" & vbCrLf & _
                                    "select max(levelID) from levels"
            Else
                'update User Level
                query.CommandText = "update levels set levelName = '" & Trim(txtUserLevel.Text) & "', description = '" & Trim(txtDescription.Text) & "', notes = '" & Trim(txtNotes.Text) & "' where levelID = " & editLevelID
            End If

            Dim dt As DataTable
            Dim rows As DataRow()

            updateDataSet()
            dt = CType(Cache("levelPermission"), DataTable)

            dr = query.ExecuteReader

            Dim newLevelAdded As Boolean = False

            If editLevelID = 0 Then
                If dr.Read() Then
                    editLevelID = dr.Item(0)
                    newLevelAdded = True
                    'immediately grant access to home, user profile, error, help pages
                    sql &= "insert into levelsPermission (levelID, screenNameID, access) values (" & editLevelID & ",27,1)" & vbCrLf & _
                            "insert into levelsPermission (levelID, screenNameID, access) values (" & editLevelID & ",4,1)" & vbCrLf & _
                            "insert into levelsPermission (levelID, screenNameID, access) values (" & editLevelID & ",93,1)" & vbCrLf
                End If

                rows = dt.Select()
            Else
                rows = dt.Select("", "", DataViewRowState.ModifiedCurrent)
            End If

            dr.Close()

            '-------------------------------------------------------
            ' Save/Update User Level details to the LTTlevels table
            '-------------------------------------------------------
            If newLevelAdded Then
                'insert User Level details into LTTlevels
                query.CommandText = "INSERT INTO LTTlevels (levelID, levelName, description, notes, LTTfullView) VALUES (" & editLevelID & ", '" & Trim(txtUserLevel.Text) & "','" & Trim(txtDescription.Text) & "','" & Trim(txtNotes.Text) & "', " & intLTTfullView & ")"

            Else
                'update LTTlevels table
                query.CommandText = "UPDATE LTTlevels SET levelID = " & editLevelID & ", levelName = '" & Trim(txtUserLevel.Text) & "', description = '" & Trim(txtDescription.Text) & "', notes = '" & Trim(txtNotes.Text) & "', LTTfullView = " & intLTTfullView & " WHERE levelID = " & editLevelID

            End If
            dr = query.ExecuteReader
            dr.Close()
            '---------------------------------
            ' End save/update LTTlevels table
            '---------------------------------

            Dim counter As Integer
            Dim selected As Integer


            'iterates through grid's row
            For counter = 0 To rows.Length - 1

                If rows(counter).Item("access") Then
                    selected = 1
                Else
                    selected = 0
                End If

                sql &= "If (select count(*) from levelsPermission where screenNameID = " & rows(counter).Item("screenNameID") & " and levelID = " & editLevelID & ") > 0" & vbCrLf & _
                        "Begin" & vbCrLf & _
                        "	update levelsPermission set access = " & selected & " where screenNameID = " & rows(counter).Item("screenNameID") & " and levelID = " & editLevelID & vbCrLf & _
                        "End" & vbCrLf & _
                        "Else" & vbCrLf & _
                        "Begin" & vbCrLf & _
                        "	insert into levelsPermission (levelID, screenNameID, access) values (" & editLevelID & "," & rows(counter).Item("screenNameID") & "," & selected & ")" & vbCrLf & _
                        "End" & vbCrLf

            Next


            'save permission
            query.CommandText = sql
            query.ExecuteNonQuery()

            'checks for LTT Phase-in/Base Year conditions; If Phase-in not selected, Base-Year must not be selected and vice versa.
            sql = "IF (SELECT COUNT(*) FROM levelsPermission WHERE levelID = " & editLevelID & " AND screenNameID IN (106, 111) AND access = 0) > 0" & vbCrLf & _
                  "BEGIN" & vbCrLf & _
                  "UPDATE levelsPermission SET access = 0 WHERE levelID = " & editLevelID & " AND screenNameID IN (106, 111)" & vbCrLf & _
                  "End" & vbCrLf

            'checks for LTT Main/Start Tax conditions; Main and Start Tax must remain selected. User cannot de-select.
            'sql += "IF (SELECT count(*) FROM levelsPermission WHERE levelID = " & editLevelID & " and screenNameID in (104, 105) and access = 0) > 0" & vbCrLf & _
            '       "BEGIN" & vbCrLf & _
            '       "UPDATE levelsPermission SET access = 1 WHERE levelID = " & editLevelID & " AND screenNameID in (104, 105)" & vbCrLf & _
            '       "End" & vbCrLf

            query.CommandText = sql
            query.ExecuteNonQuery()


            Dim taxClassID As String
            Dim formField As System.Web.UI.WebControls.CheckBox
            sql = ""

            For counter = 0 To grdClasses.Rows.Count - 1
                If (grdClasses.Rows(counter).RowType = DataControlRowType.DataRow) Then
                    grdClasses.SelectedIndex = counter
                    taxClassID = Trim(grdClasses.SelectedDataKey.Value.ToString())
                    formField = CType(grdClasses.Rows(counter).Cells(0).FindControl("ckbAccess"), CheckBox)

                    If formField.Checked Then
                        selected = 1
                    Else
                        selected = 0
                    End If

                    sql &= "If (select count(*) from taxClassesPermission where taxClassID = '" & taxClassID & "' and levelID = " & editLevelID & ") > 0" & vbCrLf & _
                        "Begin" & vbCrLf & _
                        "	update taxClassesPermission set access = " & selected & " where taxClassID = '" & taxClassID & "' and levelID = " & editLevelID & vbCrLf & _
                        "End" & vbCrLf & _
                        "Else" & vbCrLf & _
                        "Begin" & vbCrLf & _
                        "	insert into taxClassesPermission (levelID, taxClassID, access) values (" & editLevelID & ",'" & taxClassID & "'," & selected & ")" & vbCrLf & _
                        "End" & vbCrLf
                End If
            Next

            'save class permission
            query.CommandText = sql
            query.ExecuteNonQuery()

            query.CommandText = "select taxClassID from taxClasses where active = 0"
            dr = query.ExecuteReader

            sql = ""
            If dr.Read() = True Then

                While dr.Read()
                    sql &= "If (select count(*) from taxClassesPermission where taxClassID = '" & dr.GetValue(0) & "' and levelID = " & editLevelID & ") > 0" & vbCrLf & _
                        "Begin" & vbCrLf & _
                        "	update taxClassesPermission set access = 0 where taxClassID = '" & dr.GetValue(0) & "' and levelID = " & editLevelID & vbCrLf & _
                        "End" & vbCrLf & _
                        "Else" & vbCrLf & _
                        "Begin" & vbCrLf & _
                        "	insert into taxClassesPermission (levelID, taxClassID, access) values (" & editLevelID & ",'" & dr.GetValue(0) & "',0)" & vbCrLf & _
                        "End" & vbCrLf
                End While

                dr.Close()
                query.CommandText = sql
                query.ExecuteNonQuery()
            End If

            'clean up
            Session.Remove("editLevelID")
            con.Close()

        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        'return to home page
        Response.Redirect("viewlevels.aspx")

    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCancel.Click
        Session.Remove("editLevelID")
        Response.Redirect("viewlevels.aspx")
    End Sub
End Class