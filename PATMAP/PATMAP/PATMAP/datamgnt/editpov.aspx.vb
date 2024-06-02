Public Partial Class editpov
    Inherits System.Web.UI.Page

    Private strFilter As String
    Private bckgColor As System.Drawing.Color

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Clears out the error message
            Master.errorMsg = ""

            If Not IsPostBack Then
                'Sets submenu to be displayed
                subMenu.setStartNode(menu.editData)

                'Sets default filter
                'Show only main classes
                strFilter = "parentTaxClassID = 'none'"
                Session("classFilter") = strFilter

                getDSNameYear()

                'Fill grid
                fillPOVGrid()

                Dim dt As DataTable
                dt = CType(Session("taxClasses"), DataTable)
                dt.AcceptChanges()
                Session("taxClasses") = dt

            Else
                If Not IsNothing(Session("classFilter")) Then
                    strFilter = Session("classFilter")
                Else
                    strFilter = "parentTaxClassID = 'none'"
                End If
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub getDSNameYear()
        Dim POVID As Integer = Session("POVID")

        ddlYear.DataSource = common.GetYears()
        ddlYear.DataBind()

        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString
        con.Open()

        Dim query As New SqlClient.SqlCommand()
        Dim dr As SqlClient.SqlDataReader

        query.Connection = con
        query.CommandText = "select dataSetName, [year] from POVDescription where POVID = " & POVID
        dr = query.ExecuteReader()

        If dr.Read() Then
            If Not IsDBNull(dr.Item(0)) Then
                txtNewDSN.Text = Server.HtmlDecode(dr.Item(0))
            End If

            If Not IsDBNull(dr.Item(1)) Then
                ddlYear.SelectedValue = dr.Item(1)
            End If
        End If

        dr.Close()
        con.Close()
    End Sub


    Private Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Try
            'change page title and breadcrumb to 
            'if the mode is either edit or add
            If common.ChangeTitle(Session("POVID"), lblTitle) Then
                txtNewDSN.Enabled = True
                ddlYear.Enabled = True
            Else
                txtNewDSN.Enabled = False
                ddlYear.Enabled = False
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub fillPOVGrid()
        'setup database connection
        Dim con As New SqlClient.SqlConnection
        Dim POVID As Integer = Session("POVID")

        con.ConnectionString = PATMAP.Global_asax.connString
        con.Open()

        Dim da As New SqlClient.SqlDataAdapter()
        Dim dt As New DataTable
        Dim dv As New DataView
        Dim query As String

        If POVID <> 0 Then
            query = "select taxClass.*, ROUND(POV.POV, 4) * 100 As BasePOV" & vbCrLf & _
                "from" & vbCrLf & _
                "(" & vbCrLf & _
                "	select taxClassID, taxClass, parentTaxClassID, 'collapse' As state, taxClassID As groupClasss, sort, 0 as subSort from taxClasses where [active] = 1 and parentTaxClassID = 'none' union" & vbCrLf & _
                "	select taxClassID, taxClass, parentTaxClassID, 'collapse' As state, parentTaxClassID As groupClasss, (select sort from taxClasses class where taxClassID = t.parentTaxClassID) as sort, sort as subSort from taxClasses t where [active] = 1 and parentTaxClassID <> 'none'" & vbCrLf & _
                ") taxClass" & vbCrLf & _
                "inner join POV on POV.taxClassID = taxClass.taxClassID AND POV.povID = " & POVID & vbCrLf & _
                "order by taxClass.sort, taxClass.subSort"
        Else
            query = "select taxClass.*, 0 As BasePOV" & vbCrLf & _
                "from" & vbCrLf & _
                "(" & vbCrLf & _
                "	select taxClassID, taxClass, parentTaxClassID, 'collapse' As state, taxClassID As groupClasss, sort, 0 as subSort from taxClasses where [active] = 1 and parentTaxClassID = 'none' union" & vbCrLf & _
                "	select taxClassID, taxClass, parentTaxClassID, 'collapse' As state, parentTaxClassID As groupClasss, (select sort from taxClasses class where taxClassID = t.parentTaxClassID) as sort, sort as subSort from taxClasses t where [active] = 1 and parentTaxClassID <> 'none'" & vbCrLf & _
                ") taxClass" & vbCrLf & _
                "order by taxClass.sort, taxClass.subSort"
        End If


        'fill in the tax class table
        da.SelectCommand = New SqlClient.SqlCommand(query, con)
        da.Fill(dt)
        con.Close()

        'sets grid's filter
        dv = dt.DefaultView
        dv.RowFilter = strFilter

        'Displays two decimal places for Base Year POV
        common.FormatDecimal(dt.Select(), "BasePOV", "2")

        grdPOV.DataSource = dt
        grdPOV.DataBind()

        'stores result in the session
        Session("taxClasses") = dt
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            common.UndoChange()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub grdPOV_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdPOV.RowCommand
        Try
            Dim dv As New DataView
            Dim dt As New DataTable

            If e.CommandName = "expandClass" Then

                'if there's no tax class data set in session,
                'retrieves data from database
                If IsNothing(Session("taxClasses")) Then
                    fillPOVGrid()
                End If

                dt = CType(Session("taxClasses"), DataTable)

                'selects current row in the grid
                grdPOV.SelectedIndex = e.CommandArgument
                'expands selected row to display subclasses
                strFilter = common.ExpandCollapse(dt, strFilter, grdPOV.SelectedDataKey.Value.ToString())
                'stores current filter
                Session("classFilter") = strFilter

                dv = dt.DefaultView
                dv.RowFilter = strFilter

                grdPOV.DataSource = dt
                grdPOV.DataBind()

            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    'updateDataSet()
    'Accepts no parameter
    'Updates data set before grid is expanded to show subclasses
    Private Sub updateDataSet()
        Dim counter, i As Integer
        Dim formField As System.Web.UI.WebControls.TextBox
        Dim dt As DataTable
        Dim dr() As DataRow
        Dim POV As Decimal

        dt = CType(Session("taxClasses"), DataTable)

        counter = grdPOV.SelectedIndex

        If (grdPOV.Rows(counter).RowType = DataControlRowType.DataRow) Then

            'gets values entered in the textbox
            formField = CType(grdPOV.Rows(counter).Cells(2).FindControl("txtBasePOV"), TextBox)

            If Not IsNothing(formField) Then

                formField.Text = Trim(formField.Text)

                If Not IsNumeric(Replace(formField.Text, "%", "")) Then
                    Master.errorMsg = common.GetErrorMessage("PATMAP72")
                    Exit Sub
                End If

                'If values have the percentage sign,
                'remove it
                If InStr(formField.Text, "%") > 0 Then
                    POV = Replace(formField.Text, "%", "")
                Else
                    POV = formField.Text

                    'If value is less than 1 converts it to percentage value
                    If POV < 1 Then
                        POV *= 100
                    End If
                End If

                If Not common.ValidateRange(POV, 0, 100) Then
                    Master.errorMsg = common.RangeErrorMsg(0, 100)
                    Exit Sub
                End If

                'selects the tax class from the data set
                dr = dt.Select("taxClassID = '" & grdPOV.SelectedDataKey.Value.ToString() & "'")

                'updates tax class's value
                If dr.Length > 0 Then
                    dr(0).Item("BasePOV") = POV

                    'selects the sub tax classes from the data set
                    dr = dt.Select("groupClasss = '" & grdPOV.SelectedDataKey.Value.ToString() & "' and taxClassID <> '" & grdPOV.SelectedDataKey.Value.ToString() & "'")

                    For i = 0 To dr.Length - 1
                        dr(i).Item("BasePOV") = POV
                    Next

                    'Displays two decimal places for Base POV
                    common.FormatDecimal(dr, "BasePOV", "2")
                End If

            End If

        End If

        grdPOV.DataSource = dt
        grdPOV.DataBind()

    End Sub

    Private Sub grdPOV_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdPOV.RowDataBound
        Try
            'Displays expand/collapse button for main classes with subclasses
            bckgColor = common.DisplayExpand(e, grdPOV.DataSource, bckgColor, strFilter)
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSave.Click
        Try

            'make sure required fields are filled out
            If String.IsNullOrEmpty(Trim(txtNewDSN.Text)) Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP135")
                Exit Sub
            End If

            If Not common.ValidateNoSpecialChar(Trim(txtNewDSN.Text)) Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP84")
                Exit Sub
            End If

            If ddlYear.SelectedValue = "<Select>" Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP86")
                Exit Sub
            End If

            txtNewDSN.Text = Server.HtmlEncode(Replace(Trim(txtNewDSN.Text), "'", "''"))

            Dim query As String
            Dim counter As Integer
            Dim POV As Decimal
            Dim dr As DataRow()
            Dim dt As DataTable
            Dim POVID As Integer = Session("POVID")

            'setup database connection
            Dim con As New SqlClient.SqlConnection
            Dim com As New SqlClient.SqlCommand
            Dim drSQL As SqlClient.SqlDataReader

            con.ConnectionString = PATMAP.Global_asax.connString
            con.Open()
            com.Connection = con

            'check if the data set name alreday exists in the database
            com.CommandText = "select dataSetName from POVDescription where POVID <> " & POVID & " AND dataSetName='" & txtNewDSN.Text & "' AND statusID in (1,3)"
            drSQL = com.ExecuteReader
            If drSQL.Read() Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP113")
                drSQL.Close()
                con.Close()
                Exit Sub
            End If

            drSQL.Close()

            query = ""
            dt = CType(Session("taxClasses"), DataTable)


            If POVID = 0 Then
                com.CommandText = "insert into POVDescription (notes, dataSetName, year, statusID) values ('', '" & txtNewDSN.Text & "' ," & ddlYear.SelectedValue & ", 1) select @@IDENTITY"
                drSQL = com.ExecuteReader()

                If drSQL.Read() Then
                    POVID = drSQL.Item(0)
                End If

                drSQL.Close()

                dr = dt.Select()
            Else
                com.CommandText = "update taxYearModelDescription set dataStale = 1 where taxYearStatusID in (1,3) and POVID = " & POVID
                com.ExecuteNonQuery()

                com.CommandText = "update assessmentTaxModel set dataStale = 1 where SubjectTaxYearModelID in (select taxYearModelID from taxYearModelDescription where POVID = " & POVID & ")"
                com.ExecuteNonQuery()
                com.CommandText = "update assessmentTaxModel set dataStale = 1 where BaseTaxYearModelID in (select taxYearModelID from taxYearModelDescription where POVID = " & POVID & ")"
                com.ExecuteNonQuery()

                com.CommandText = "update liveAssessmentTaxModel set dataStale = 1 where SubjectTaxYearModelID in (select taxYearModelID from taxYearModelDescription where POVID = " & POVID & ")"
                com.ExecuteNonQuery()
                com.CommandText = "update liveAssessmentTaxModel set dataStale = 1 where BaseTaxYearModelID in (select taxYearModelID from taxYearModelDescription where POVID = " & POVID & ")"
                com.ExecuteNonQuery()


                dr = dt.Select("", "", DataViewRowState.ModifiedCurrent)
            End If



            'iterates through data rows
            For counter = 0 To dr.Length - 1

                POV = dr(counter).Item("BasePOV") / 100

                'Formats values to be rounded into four decimal places
                POV = Math.Truncate(Math.Round(POV, 4) * 10000) / 10000

                'builds the query that will update database
                query &= "If (select count(*) from POV where POVID = " & POVID & " and  taxClassID = '" & dr(counter).Item("taxClassID") & "') > 0" & vbCrLf & _
                        "Begin" & vbCrLf & _
                        "	update POV set POV = " & POV & " where POVID = " & POVID & " and  taxClassID = '" & dr(counter).Item("taxClassID") & "'" & vbCrLf & _
                        "End" & vbCrLf & _
                        "Else" & vbCrLf & _
                        "Begin" & vbCrLf & _
                        "	insert into POV (POVID, taxClassID, POV) values (" & POVID & ",'" & dr(counter).Item("taxClassID") & "'," & POV & ")" & vbCrLf & _
                        "End" & vbCrLf
            Next

            If query <> "" Then
                Dim trans As SqlClient.SqlTransaction
                trans = con.BeginTransaction()
                com.Transaction = trans
                com.CommandText = query
                Try
                    com.ExecuteNonQuery()
                    trans.Commit()
                Catch
                    trans.Rollback()
                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP101")
                    con.Close()
                    Exit Sub
                End Try
            End If

            con.Close()

            'Removes any filter or tax class data set in session
            Session.Remove("classFilter")
            Session.Remove("taxClasses")

        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        Response.Redirect("viewpov.aspx")

    End Sub

    Public Sub valueChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            btnSave.Enabled = False
            grdPOV.SelectedIndex = sender.parent.parent.dataitemindex

            updateDataSet()
            btnSave.Enabled = True
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCancel.Click
        'Removes any filter or tax class data set in session
        Session.Remove("classFilter")
        Session.Remove("taxClasses")

        Response.Redirect("viewpov.aspx")
    End Sub
End Class