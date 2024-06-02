Imports System.Data.SqlClient
Partial Public Class editfunctions
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            'Clears out the error message
            Master.errorMsg = ""

            If Not IsPostBack Then

                'get function ID
                Dim editFunctionID As String
                editFunctionID = Session("editFunctionID")

                'setup database connection
                Dim con As New SqlClient.SqlConnection
                con.ConnectionString = PATMAP.Global_asax.connString
                Dim query As New SqlClient.SqlCommand
                query.Connection = con
                Dim dr As SqlClient.SqlDataReader

                If editFunctionID <> "" Then
                    con.Open()

                    'get function details from database
                    query.CommandText = "select functionName, description, formula, notes from functions where functionID = " & editFunctionID
                    dr = query.ExecuteReader
                    dr.Read()

                    'fill user detail into first section form fields                
                    txtFunctionName.Text = dr.GetValue(0)

                    If IsDBNull(dr.GetValue(1)) Then
                        txtDescription.Text = ""
                    Else
                        txtDescription.Text = dr.GetValue(1)
                    End If

                    If IsDBNull(dr.GetValue(3)) Then
                        txtNotes.Text = ""
                    Else
                        txtNotes.Text = dr.GetValue(3)
                    End If

                    txtFormula.Text = dr.GetValue(2)

                    'cleanup
                    dr.Close()

                    'check if should show reset button
                    query.CommandText = "select functionID from functionsReset where functionID = " & editFunctionID
                    dr = query.ExecuteReader
                    If dr.Read() Then
                        btnReset.Visible = True
                    Else
                        btnReset.Visible = False
                    End If
                    dr.Close()

                    'clean up
                    con.Close()
                End If

                'if the user is add the function very first time, then there is no need for reset button.
                '(no data available to point at)
                If editFunctionID = "" Then
                    btnReset.Visible = False
                End If

            End If

        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try

    End Sub

    Private Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Try
            'change page title and breadcrumb to 
            'if the mode is either edit or add
            common.ChangeTitle(Session("editFunctionID"), lblTitle)
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

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSave.Click
        Try
            'make sure required fields are filled out
            If String.IsNullOrEmpty(Trim(txtFunctionName.Text)) Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP43")
                Exit Sub
            End If
            If String.IsNullOrEmpty(Trim(txtFormula.Text)) Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP44")
                Exit Sub
            End If
            If Not common.ValidateNoSpecialChar(Trim(txtFunctionName.Text)) Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP96")
                Exit Sub
            End If


            'make sure the other fields are empty strings if not given
            If String.IsNullOrEmpty(Trim(txtDescription.Text)) Then
                txtDescription.Text = ""
            End If
            If String.IsNullOrEmpty(Trim(txtNotes.Text)) Then
                txtNotes.Text = ""
            End If

            'remove any single quotes from fields (plus, any other validation req'd)
            txtFunctionName.Text = Trim(txtFunctionName.Text.Replace("'", "''"))
            txtDescription.Text = Trim(txtDescription.Text.Replace("'", "''"))
            txtFormula.Text = Trim(txtFormula.Text.Replace("'", ""))
            txtNotes.Text = Trim(txtNotes.Text.Replace("'", "''"))

            'get function ID
            Dim editFunctionID As String
            editFunctionID = Session("editFunctionID")

            'setup database connection
            Dim con As New SqlConnection
            con.ConnectionString = PATMAP.Global_asax.connString
            Dim query As New SqlCommand
            query.Connection = con
            Dim dr As SqlClient.SqlDataReader
            con.Open()

            If editFunctionID = "" Then
                query.CommandText = "select functionName from functions where functionName = '" & txtFunctionName.Text & "'"
            Else
                query.CommandText = "select functionName from functions where functionID <> " & editFunctionID & " and functionName = '" & txtFunctionName.Text & "'"
            End If
            dr = query.ExecuteReader()

            If dr.Read() Then
                dr.Close()
                con.Close()
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP112")
                Exit Sub
            End If

            dr.Close()


            Dim sql As String = ""
            'save the function details
            If editFunctionID = "" Then
                'insert function 
                sql = "insert into functions (functionName, description, formula, notes) values ('" & Trim(txtFunctionName.Text) & "','" & Trim(txtDescription.Text) & "','" & Trim(txtFormula.Text) & "','" & Trim(txtNotes.Text) & "')" & vbCrLf
            Else
                'update function
                sql = "update functions set functionName = '" & Trim(txtFunctionName.Text) & "', description = '" & Trim(txtDescription.Text) & "', formula = '" & Trim(txtFormula.Text) & "', notes = '" & Trim(txtNotes.Text) & "' where functionID = " & editFunctionID & vbCrLf
            End If

            'check if this has been select to be a restore point
            If ckbReset.Checked = True Then
                'get the functionID of the record which was just inserted
                If editFunctionID = "" Then
                    query.CommandText = "select max(functionID) from functions"
                    dr = query.ExecuteReader
                    dr.Read()
                    editFunctionID = dr.GetValue(0)
                    dr.Close()
                End If

                sql += "delete from functionsReset where functionID = " & editFunctionID & vbCrLf
                sql += "insert into functionsReset select * from functions where functionID = " & editFunctionID & "" & vbCrLf
            End If

            sql &= "update taxYearModelDescription set dataStale = 1 where taxYearStatusID in (1,3) " & vbCrLf & _
                    "update assessmentTaxModel set dataStale = 1 " & vbCrLf & _
                    "update liveAssessmentTaxModel set dataStale = 1 " & vbCrLf & _
                    "update boundaryModel set dataStale = 1 where status in (1,3) " & vbCrLf

            Dim trans As SqlClient.SqlTransaction
            trans = con.BeginTransaction()
            query.Transaction = trans
            query.CommandText = sql
            Try
                query.ExecuteNonQuery()
                trans.Commit()
            Catch
                trans.Rollback()
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP101")
                con.Close()
                Exit Sub
            End Try

            'clean up
            Session.Remove("editFunctionID")
            con.Close()

        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        'return to home page
        Response.Redirect("viewfunctions.aspx")

    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCancel.Click
        Session.Remove("editFunctionID")
        Response.Redirect("viewfunctions.aspx")
    End Sub


    Private Sub btnReset_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnReset.Click
        Try
            'get function ID
            Dim editFunctionID As String
            editFunctionID = Session("editFunctionID")

            'setup database connection
            Dim con As New SqlClient.SqlConnection
            con.ConnectionString = PATMAP.Global_asax.connString
            Dim query As New SqlClient.SqlCommand
            query.Connection = con
            Dim dr As SqlClient.SqlDataReader

            'get function details from database
            con.Open()
            query.CommandText = "select functionName, description, formula, notes from functionsReset where functionID = " & editFunctionID
            dr = query.ExecuteReader
            dr.Read()

            'fill user detail into first section form fields
            txtFunctionName.Text = dr.GetValue(0)

            If IsDBNull(dr.GetValue(1)) Then
                txtDescription.Text = ""
            Else
                txtDescription.Text = dr.GetValue(1)
            End If

            If IsDBNull(dr.GetValue(2)) Then
                txtFormula.Text = ""
            Else
                txtFormula.Text = dr.GetValue(2)
            End If

            txtNotes.Text = dr.GetValue(3)

            'cleanup
            dr.Close()
            con.Close()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub
End Class