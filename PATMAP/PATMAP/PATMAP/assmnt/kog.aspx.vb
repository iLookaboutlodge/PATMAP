Public Partial Class kog
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            'Clears out the error message
            Master.errorMsg = ""

            If Not IsPostBack Then

                'Sets submenu to be displayed
                subMenu.setStartNode(menu.assmnt)

                Dim levelID As Integer = Session("levelID")

                'Presentation users has to re-save current scenario in 
                'in a different scenario name
                If levelID = 3 Or levelID = 49 Then
                    btnSave.ImageUrl = "~/images/btnSaveAs.gif"
                End If


                If Not IsNothing(Session("missingK12DataSet")) Then
                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP53")
                    Session.Remove("missingK12DataSet")
                End If

                'get userid
                Dim userID As Integer = Session("userID")

                'setup database connection
                Dim con As New SqlClient.SqlConnection
                con.ConnectionString = PATMAP.Global_asax.connString
                con.Open()

                Dim da As New SqlClient.SqlDataAdapter
                Dim dt As New DataTable

                'populate the 'subject yr k12 data set' drop down list
                Dim constr As String
                constr = "select 0 as K12ID, '<Select>' as dataSetName union all select K12ID, dataSetName from K12Description where statusID=1"
                da.SelectCommand = New SqlClient.SqlCommand(constr, con)

                da.Fill(dt)
                ddlSubjectKOG.DataSource = dt
                ddlSubjectKOG.DataValueField = "K12ID"
                ddlSubjectKOG.DataTextField = "dataSetName"
                ddlSubjectKOG.DataBind()

                Dim dr As SqlClient.SqlDataReader
                Dim query As New SqlClient.SqlCommand
                query.Connection = con

                'Gets Tax Year Model names used by the scenario 
                common.GetModelNames(txtBaseTaxYrModel.Text, txtSubjectTaxYrModel.Text, Session("userID"), txtScenarioName.Text)

                'set default values for k-12 OG                
                txtKOGAmt.Text = "0.00"
                rblKOGAmt.Items(1).Selected = True

                'populate k12 data set info (if exists)
                constr = "select k.K12ID, k.dataSetName, l.K12Amount, l.K12Replacement from liveAssessmentTaxModel l left join K12Description k on k.K12ID=l.SubjectK12ID where l.userID=" & userID
                query.CommandText = constr
                dr = query.ExecuteReader()
                dr.Read()

                If Not IsDBNull(dr.GetValue(0)) Then
                    'ddlSubjectKOG.Items.FindByValue(dr.GetValue(0)).Selected = True
                    ddlSubjectKOG.SelectedValue = dr.GetValue(0)
                End If

                If Not IsDBNull(dr.GetValue(2)) Then
                    txtKOGAmt.Text = dr.GetValue(2).ToString()
                End If

                If Not IsDBNull(dr.GetValue(3)) Then
                    If dr.GetValue(3) Then
                        rblKOGAmt.Items(0).Selected = True
                        rblKOGAmt.Items(1).Selected = False
                    Else
                        rblKOGAmt.Items(1).Selected = True
                        rblKOGAmt.Items(0).Selected = False
                    End If
                End If

                'clean up
                dr.Close()
                con.Close()

            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSubmit.Click
        Try
            'make sure that the k12 subject yr data set field is selected
            If ddlSubjectKOG.Items(0).Selected Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP53")
                Exit Sub
            End If

            'update k12 data set info
            'setup database connection
            Dim con As New SqlClient.SqlConnection
            con.ConnectionString = PATMAP.Global_asax.connString
            con.Open()

            If Trim(txtKOGAmt.Text) <> "" And Not Regex.IsMatch(Trim(txtKOGAmt.Text), "^\d*$|^\d*\.\d+$") Then
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP80")
                Exit Sub
            End If

            If String.IsNullOrEmpty(Trim(txtKOGAmt.Text)) Then
                txtKOGAmt.Text = 0.0
            End If

            Dim query As New SqlClient.SqlCommand
            query.Connection = con
            Dim sql As String = ""
            sql = "update liveAssessmentTaxModel set K12Amount=" & Trim(txtKOGAmt.Text) & ", SubjectK12ID=" & ddlSubjectKOG.SelectedValue & ", "
            If rblKOGAmt.Items(0).Selected Then
                sql += "K12Replacement=1 "
            Else
                sql += "K12Replacement=0 "
            End If

            Dim trans As SqlClient.SqlTransaction
            trans = con.BeginTransaction()
            query.Transaction = trans

            Dim updateAuditTrailSQL As StringBuilder = getUpdateAuditTrailSQL(Session("userID").ToString, ddlSubjectKOG.SelectedItem.Text.Replace("'", "''"), ddlSubjectKOG.SelectedItem.Value, Trim(txtKOGAmt.Text), rblKOGAmt.SelectedItem.Value)
            If IsNothing(updateAuditTrailSQL) Then
                sql += "where userID=" & Session("userID") & vbCrLf
                query.CommandText = sql
            Else
                'if k-12 og data set, k-12 og amount is changed then the everything should be recalculated
                sql += ", dataStale=1 where userID=" & Session("userID") & vbCrLf
                query.CommandText = sql & updateAuditTrailSQL.ToString()
            End If
            Try
                query.ExecuteNonQuery()
                trans.Commit()
            Catch
                trans.Rollback()
                Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP101")
                con.Close()
                Exit Sub
            End Try

            con.Close()

        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        'Response.Redirect("general.aspx")
        common.gotoNextPage(3, 6, Session("levelID"))
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

    Private Sub kog_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If IsNothing(Session("assessmentTaxModelID")) Then
            Response.Redirect("start.aspx")
        End If
    End Sub

    Protected Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnClear.Click
        ddlSubjectKOG.SelectedItem.Selected = False
        ddlSubjectKOG.Items(0).Selected = True
        txtKOGAmt.Text = ""
        rblKOGAmt.Items(1).Selected = True
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

    Private Function getUpdateAuditTrailSQL(ByVal userID As String, ByVal K12DataSetNm As String, ByVal K12DataSetID As String, ByVal K12Amount As String, ByVal K12Replacement As String) As StringBuilder

        'setup database connection            
        Dim con As New SqlClient.SqlConnection
        Dim com As New SqlClient.SqlCommand
        con.ConnectionString = PATMAP.Global_asax.connString
        con.Open()
        com.Connection = con
        Dim dr As SqlClient.SqlDataReader

        com.CommandText = "select k.dataSetName, l.SubjectK12ID, l.K12Amount, l.K12Replacement, l.auditTrailText from liveAssessmentTaxModel l left join K12Description k on k.K12ID=l.SubjectK12ID where l.userID=" & userID
        dr = com.ExecuteReader
        dr.Read()

        Dim sql As String = ""
        Dim auditTrail As String = ""
        If dr.GetInt32(1) <> CType(K12DataSetID, Integer) Then
            If dr.GetInt32(1) = 0 Then
                auditTrail += "[" & Now.ToString("MM/dd/yyyy") & "]Subject Year K-12 OG Data Set is set to " & K12DataSetNm & vbCrLf
            Else
                auditTrail += "[" & Now.ToString("MM/dd/yyyy") & "]Subject Year K-12 OG Data Set changed from " & dr.GetValue(0) & " to " & K12DataSetNm & vbCrLf
            End If
        End If
        If dr.GetDouble(2) <> CType(K12Amount, Double) Then
            auditTrail += "[" & Now.ToString("MM/dd/yyyy") & "]K-12 OG Amount changed from " & dr.GetValue(2).ToString & " to " & K12Amount & vbCrLf
        End If
        If dr.GetBoolean(3) And K12Replacement = "In addition" Then
            auditTrail += "[" & Now.ToString("MM/dd/yyyy") & "]K-12 OG Amount action changed from Replacement to " & K12Replacement & vbCrLf
        End If
        If Not dr.GetBoolean(3) And K12Replacement = "Replacement" Then
            auditTrail += "[" & Now.ToString("MM/dd/yyyy") & "]K-12 OG Amount action changed from In addition to " & K12Replacement & vbCrLf
        End If

        Dim updateAuditTrailSQL As StringBuilder = Nothing
        If auditTrail <> "" Then
            updateAuditTrailSQL = New StringBuilder("update liveAssessmentTaxModel set auditTrailText='" & auditTrail & dr.GetValue(4).ToString.Replace("'", "''") & "' where userID=" & userID.ToString)
        End If

        dr.Close()
        con.Close()

        Return updateAuditTrailSQL
    End Function
End Class