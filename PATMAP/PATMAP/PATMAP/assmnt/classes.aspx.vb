Public Partial Class classes
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Try

        'clears out the error message
        Master.errorMsg = ""

        If Not IsPostBack Then
            'sets submenu to be displayed
            subMenu.setStartNode(menu.assmnt)

            Dim levelID As Integer = Session("levelID")

            'Presentation users has to re-save current scenario 
            'using a different scenario name
            If levelID = 3 Then
                btnSave.ImageUrl = "~/images/btnSaveAs.gif"
            End If


            'gets Tax Year Model names used by the scenario 
            common.GetModelNames(txtBaseTaxYrModel.Text, txtSubjectTaxYrModel.Text, Session("userID"), txtScenarioName.Text)

            'setup database connection
            Dim con As New SqlClient.SqlConnection
            con.ConnectionString = PATMAP.Global_asax.connString
            con.Open()

            Dim dr As SqlClient.SqlDataReader
            Dim query As New SqlClient.SqlCommand
            Dim sql As String = ""
            Dim userID As Integer = Session("userID")

            'select main classes
            query.Connection = con
            query.CommandText = "select taxClassID from livetaxClasses where userID = " & userID
            dr = query.ExecuteReader()

            Dim selectedTaxClasses As List(Of String)
            Dim counter As Integer
            Dim strSQL As String = ""
            Dim show As Integer = 0

            'if user came from the Map page and made changes
            'to which tax classes he or she wants to view
            If Not IsNothing(Session("MapPropertyClassFilters")) Then
                selectedTaxClasses = CType(Session("MapPropertyClassFilters"), List(Of String))
                Session.Remove("MapPropertyClassFilters")

                If selectedTaxClasses.Count > 0 Then
                    While dr.Read()

                        If selectedTaxClasses.Contains(dr.Item(0)) Then
                            show = 1
                        Else
                            show = 0
                        End If

                        'update DB table for user selection
                        strSQL &= "update livetaxClasses set show = " & show & " where userID = " & userID & " and taxClassID = '" & dr.Item(0) & "'" & vbCrLf
                    End While
                End If
            End If

            dr.Close()

            If strSQL <> "" Then
                query.CommandText = strSQL
                query.ExecuteNonQuery()
            End If

            'retrieves main tax classes
            query.CommandText = "select taxClassID from taxClasses where active = 1 and parentTaxClassID = 'none' order by sort"
            dr = query.ExecuteReader()

            'select sub classes for each main class
            While dr.Read()
                sql &= "select taxClasses.taxClassID, taxClasses.taxClass, show as [default] from livetaxClasses inner join taxClasses on livetaxClasses.taxClassID = taxClasses.taxClassID where (taxClasses.taxClassID = '" & dr.Item(0) & "' or taxClasses.parentTaxClassID = '" & dr.Item(0) & "') and livetaxClasses.userID=" & Session("userID") & " order by taxClasses.sort" & vbCrLf
            End While

            'clean up
            dr.Close()

            Dim da As New SqlClient.SqlDataAdapter
            Dim ds As New DataSet

            da = New SqlClient.SqlDataAdapter
            da.SelectCommand = New SqlClient.SqlCommand
            da.SelectCommand.Connection = con

            da.SelectCommand.CommandText = sql
            da.Fill(ds)

            Dim i As Integer
            Dim ckList As CheckBoxList
            'Dim divCls As String

            'loop through the dataset to bind the appropriate group of main/sub classes
            For counter = 0 To ds.Tables.Count - 1
                ckList = Page.FindControl("ctl00$mainContent$cklTaxClass" & counter + 1)
                If Not IsNothing(ckList) Then
                    ckList.DataSource = ds.Tables(counter)
                    ckList.DataTextField = "taxClass"
                    ckList.DataValueField = "taxClassID"
                    ckList.DataBind()

                    Dim pClassList As Boolean = False
                    For i = 0 To ckList.Items.Count - 1
                        ckList.Items(i).Selected = ds.Tables(counter).Rows(i).Item("default")
                        pClassList = True
                    Next


                    ckList.Visible = True
                    If Not pClassList Then
                        ckList.Visible = False
                    End If

                End If
            Next

            'clean up                
            con.Close()

        End If
        'Catch
        'retrieves error message
        'Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        'End Try
    End Sub

    Private Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Try
            'change page title and breadcrumb to 
            'if the mode is either edit or add
            common.ChangeTitle(Session("pageID"), lblTitle)
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub classes_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If IsNothing(Session("assessmentTaxModelID")) Then
            Response.Redirect("start.aspx")
        End If
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            common.UndoChange()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSubmit.Click

        Dim pageID As Integer = Session("pageID")

        Try

            Session.Remove("pageID")

            Dim userID As String
            userID = Session("userID")

            Dim counter, i As Integer
            Dim ckList As CheckBoxList
            Dim sqlSelected As String = ""
            Dim sqlNotSelected As String = ""

            'setup database connection
            Dim con As New SqlClient.SqlConnection
            con.ConnectionString = PATMAP.Global_asax.connString
            con.Open()

            Dim query As New SqlClient.SqlCommand
            query.Connection = con

            sqlSelected = ""
            sqlNotSelected = ""
            counter = 1
            ckList = Page.FindControl("ctl00$mainContent$cklTaxClass" & counter)
            'loop through each class and just update the 
            While Not IsNothing(ckList)
                For i = 0 To ckList.Items.Count - 1
                    If ckList.Items(i).Selected Then
                        sqlSelected += "'" & ckList.Items(i).Value & "',"
                    Else
                        sqlNotSelected += "'" & ckList.Items(i).Value & "',"
                    End If
                Next
                counter += 1
                ckList = Page.FindControl("ctl00$mainContent$cklTaxClass" & counter)
            End While

            query.CommandText = ""
            If sqlSelected <> "" Then
                sqlSelected = "(" & sqlSelected.Remove(sqlSelected.Length - 1, 1) & ")"
                query.CommandText += "update liveTaxClasses set show=1 where userID=" & userID & " And taxClassID in " & sqlSelected & vbCrLf
            End If
            If sqlNotSelected <> "" Then
                sqlNotSelected = "(" & sqlNotSelected.Remove(sqlNotSelected.Length - 1, 1) & ")"
                query.CommandText += "update liveTaxClasses set show=0 where userID=" & userID & " And taxClassID in " & sqlNotSelected & vbCrLf
            End If

            Dim trans As SqlClient.SqlTransaction
            trans = con.BeginTransaction()
            query.Transaction = trans

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
            con.Close()

        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        If pageID = 0 Then
            Response.Redirect("graphs.aspx")
        Else
            Response.Redirect("tables.aspx")
        End If

    End Sub

    Private Sub btnReset_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnReset.Click
        Try
            'setup database connection
            Dim con As New SqlClient.SqlConnection
            con.ConnectionString = PATMAP.Global_asax.connString
            con.Open()

            Dim dr As SqlClient.SqlDataReader
            Dim query As New SqlClient.SqlCommand
            query.Connection = con

            Dim sql As String = ""

            query.Connection = con
            query.CommandText = "select taxClassID from taxClasses where active = 1 and parentTaxClassID = 'none' order by sort"
            dr = query.ExecuteReader()

            While dr.Read()
                sql &= "select taxClassID, taxClass, [default] as [default] from taxClasses where (taxClassID = '" & dr.Item(0) & "' or parentTaxClassID = '" & dr.Item(0) & "') order by taxClasses.sort" & vbCrLf
            End While

            'clean up
            dr.Close()

            Dim da As New SqlClient.SqlDataAdapter
            Dim ds As New DataSet

            da = New SqlClient.SqlDataAdapter
            da.SelectCommand = New SqlClient.SqlCommand
            da.SelectCommand.Connection = con

            da.SelectCommand.CommandText = sql
            da.Fill(ds)

            Dim counter, i As Integer
            Dim ckList As CheckBoxList

            For counter = 0 To ds.Tables.Count - 1
                ckList = Page.FindControl("ctl00$mainContent$cklTaxClass" & counter + 1)

                If Not IsNothing(ckList) Then
                    ckList.DataSource = ds.Tables(counter)
                    ckList.DataTextField = "taxClass"
                    ckList.DataValueField = "taxClassID"
                    ckList.DataBind()

                    For i = 0 To ckList.Items.Count - 1
                        ckList.Items(i).Selected = ds.Tables(counter).Rows(i).Item("default")
                    Next

                End If
            Next

            'clean up
            con.Close()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Public Sub changeName(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim errCode As String

            errCode = common.UpdateScenarioName(Session("userID"), Session("assessmentTaxModelID"), txtScenarioName.Text)

            If errCode <> "" Then
                Master.errorMsg = common.GetErrorMessage(errCode)
            End If
        Catch
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSave.Click
        Try
            Dim errCode As String

            If Trim(txtScenarioName.Text) = "" Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP99")
                Exit Sub
            End If

            errCode = common.UpdateScenarioName(Session("userID"), Session("assessmentTaxModelID"), txtScenarioName.Text)

            If errCode <> "" Then
                Master.errorMsg = common.GetErrorMessage(errCode)
                Exit Sub
            End If

            common.saveLiveModelTables(Session("userID"))

        Catch
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        Response.Redirect("start.aspx")
    End Sub
End Class