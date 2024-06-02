Partial Public Class taxstatusBoundary
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

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

                Dim userID As String
                userID = Session("userID")

                'setup database connection
                Dim con As New SqlClient.SqlConnection
                con.ConnectionString = PATMAP.Global_asax.connString
                con.Open()

                Dim query As New SqlClient.SqlCommand
                query.Connection = con

                Dim dr As SqlClient.SqlDataReader

                query.CommandText = "select taxstatusID, selected from liveTaxStatus where userid = " & userID
                dr = query.ExecuteReader

                While (dr.Read())
                    Dim taxStatusID As Integer
                    Dim selected As Boolean

                    taxStatusID = dr.GetValue(0)
                    selected = dr.GetValue(1)

                    Select Case taxStatusID
                        Case 1
                            If selected Then
                                cklTaxStatusTaxable.Items(0).Selected = True
                            Else
                                cklTaxStatusTaxable.Items(0).Selected = False
                            End If
                        Case 4
                            If selected Then
                                cklTaxStatusExempt.Items(0).Selected = True
                            Else
                                cklTaxStatusExempt.Items(0).Selected = False
                            End If
                        Case 5
                            If selected Then
                                cklTaxStatusFGIL.Items(0).Selected = True
                            Else
                                cklTaxStatusFGIL.Items(0).Selected = False
                            End If
                        Case 6
                            If selected Then
                                cklTaxStatusPGIL.Items(0).Selected = True
                            Else
                                cklTaxStatusPGIL.Items(0).Selected = False
                            End If
                    End Select

                End While

                con.Close()

                'gets Tax Year Model names used by the scenario 
                common.GetModelNames(txtBaseTaxYrModel.Text, txtSubjectTaxYrModel.Text, Session("userID"), txtScenarioName.Text)

            End If
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
        'If IsNothing(Session("assessmentTaxModelID")) Then
        '    Response.Redirect("start.aspx")
        'End If
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

            'setup database connection
            Dim con As New SqlClient.SqlConnection
            con.ConnectionString = PATMAP.Global_asax.connString
            con.Open()

            Dim query As New SqlClient.SqlCommand
            query.Connection = con

            If cklTaxStatusTaxable.Items(0).Selected Then
                query.CommandText = "update liveTaxStatus set selected = 1 where userID = " & userID & " and taxstatusID = " & cklTaxStatusTaxable.Items(0).Value
            Else
                query.CommandText = "update liveTaxStatus set selected = 0 where userID = " & userID & " and taxstatusID = " & cklTaxStatusTaxable.Items(0).Value
            End If

            If cklTaxStatusFGIL.Items(0).Selected Then
                query.CommandText += "update liveTaxStatus set selected = 1 where userID = " & userID & " and taxstatusID = " & cklTaxStatusFGIL.Items(0).Value
            Else
                query.CommandText += "update liveTaxStatus set selected = 0 where userID = " & userID & " and taxstatusID = " & cklTaxStatusFGIL.Items(0).Value
            End If

            If cklTaxStatusPGIL.Items(0).Selected Then
                query.CommandText += "update liveTaxStatus set selected = 1 where userID = " & userID & " and taxstatusID = " & cklTaxStatusPGIL.Items(0).Value
            Else
                query.CommandText += "update liveTaxStatus set selected = 0 where userID = " & userID & " and taxstatusID = " & cklTaxStatusPGIL.Items(0).Value
            End If

            If cklTaxStatusExempt.Items(0).Selected Then
                query.CommandText += "update liveTaxStatus set selected = 1 where userID = " & userID & " and taxstatusID = " & cklTaxStatusExempt.Items(0).Value
            Else
                query.CommandText += "update liveTaxStatus set selected = 0 where userID = " & userID & " and taxstatusID = " & cklTaxStatusExempt.Items(0).Value
            End If

            query.ExecuteNonQuery()

            'clean up
            con.Close()

        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        If IsNothing(Session("taxStatusPageID")) Then
            Session.Add("taxStatusPageID", 0)
        End If

        If pageID = 0 Then
            Response.Redirect("graphs.aspx")
        Else
            Response.Redirect("tables.aspx")
        End If
    End Sub
End Class