Imports System.IO
Partial Public Class viewdataset
    Inherits System.Web.UI.Page

    Const ACTIVE As Integer = 1
    Const DELETE As Integer = 2
    Const HISTORY As Integer = 3
    Const INACTIVE As Integer = 4


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Clears out the error message
            Master.errorMsg = ""

            If Not IsPostBack Then

                'Sets submenu to be displayed
                subMenu.setStartNode(menu.taxYearModel)

                Bind_Dll()

                'set 'Active' - by default
                rdlStatus.Items(0).Selected = "True"

                grdDataSet.PageSize = PATMAP.Global_asax.pageSize
            End If

        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try

    End Sub

    Private Sub grdDataSet_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdDataSet.RowCommand
        Try
            If LCase(e.CommandName) <> "sort" And LCase(e.CommandName) <> "page" Then

                'find the row index which to be edited/deleted
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim editDataSetID As String = grdDataSet.DataKeys(index).Values("dataSetID")

                'get data source
                Dim dataSource As String
                dataSource = Session("dataSource")

                Dim dataSetTblNm As String = ""
                Dim dataSetTblPrimaryKey As String = ""
                'Dim deleteFolderNm = ""

                Select Case dataSource
                    Case "Assessment"
                        dataSetTblNm = "assessmentDescription"
                        dataSetTblPrimaryKey = "assessmentID"
                        'deleteFolderNm = "Assessment"
                        'Donna - Removed Tax Credit.
                        'Case "Tax Credit"
                        '    dataSetTblNm = "taxCreditDescription"
                        '    dataSetTblPrimaryKey = "taxCreditID"
                        '    'deleteFolderNm = "TaxCredit"

                    Case "POV"
                        dataSetTblNm = "POVDescription"
                        dataSetTblPrimaryKey = "POVID"
                        'deleteFolderNm = "POV"

                    Case "Mill Rate Survey"
                        dataSetTblNm = "millRateSurveyDescription"
                        dataSetTblPrimaryKey = "millRateSurveyID"
                        'deleteFolderNm = "MillRateSurvey"
                        'Donna - Removed K12OG.
                        'Case "K12OG"
                        '    dataSetTblNm = "K12Description"
                        '    dataSetTblPrimaryKey = "K12ID"
                        '    'deleteFolderNm = "K12OG"

                    Case "Potash"
                        dataSetTblNm = "potashDescription"
                        dataSetTblPrimaryKey = "potashID"
                        'deleteFolderNm = "Potash"
                End Select

                'check what type of command has been fired by the grid
                Select Case e.CommandName
                    Case "editDataSet"
                        Session.Add("editDataSetID", editDataSetID)
                    Case "deleteDataSet"
                        'setup database connection
                        Dim con As New SqlClient.SqlConnection
                        con.ConnectionString = PATMAP.Global_asax.connString
                        con.Open()
                        Dim query As New SqlClient.SqlCommand
                        query.Connection = con


                        'check if the dataset is being used in another assessment taxYrModel
                        Dim dr As SqlClient.SqlDataReader
                        query.CommandText = "select taxYearModelID, taxYearModelName from taxYearModelDescription where " & dataSetTblPrimaryKey & "=" & editDataSetID & " AND (taxYearStatusID=" & ACTIVE & " or taxYearStatusID=" & HISTORY & ")"
                        dr = query.ExecuteReader

                        If dr.Read() Then
                            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP65")
                            dr.Close()
                            con.Close()
                            Exit Sub

                        Else
                            'check if the dataset is being used in another bndary taxYrModel
                            If dataSetTblPrimaryKey = "assessmentID" Or dataSetTblPrimaryKey = "millRateSurveyID" Or dataSetTblPrimaryKey = "POVID" Then
                                query.CommandText = "select boundaryModelID from boundaryModel where " & dataSetTblPrimaryKey & "=" & editDataSetID & " AND status = 1"
                                dr.Close()
                                dr = query.ExecuteReader
                                If dr.Read() Then
                                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP322")
                                    dr.Close()
                                    con.Close()
                                    Exit Sub
                                End If
                            End If

                            'dr.Close()
                            'query.CommandText = "select boundaryModelID from boundaryModel where " & dataSetTblPrimaryKey & "=" & editDataSetID & " AND (status=" & ACTIVE & " or status=" & HISTORY & " or status=" & INACTIVE & ")"
                            'dr = query.ExecuteReader

                            'If dr.Read() Then
                            '    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP320")
                            '    dr.Close()
                            '    con.Close()
                            '    Exit Sub
                            'End If

                            'delete function
                            dr.Close()
                            query.CommandText = "update " & dataSetTblNm & " set statusID=" & DELETE & " where " & dataSetTblPrimaryKey & "=" & editDataSetID
                            query.ExecuteNonQuery()

                            'delete the associated dataset folder...
                            'Impersonate.impersonateValidUser(Global_asax.domainUser, Global_asax.domainName, Global_asax.domainPassword, Master.errorMsg)
                            'Dim strPath As String = ""
                            'strPath = PATMAP.Global_asax.FileRootPath & deleteFolderNm & "\" & editDataSetID & "\"
                            'If Directory.Exists(strPath) Then
                            '    Directory.Delete(strPath, True)
                            'End If
                            'Impersonate.undoImpersonation()

                        End If

                        'cleanup
                        dr.Close()
                        con.Close()

                        'update tax year model search grid
                        Rebind()

                    Case "restoreTaxYearModel"
                        'setup database connection
                        Dim con As New SqlClient.SqlConnection
                        con.ConnectionString = PATMAP.Global_asax.connString
                        Dim query As New SqlClient.SqlCommand
                        query.Connection = con

                        'restore tax year model from history
                        query.CommandText = "update " & dataSetTblNm & " set statusID=" & ACTIVE & " where " & dataSetTblPrimaryKey & "=" & editDataSetID
                        con.Open()
                        query.ExecuteNonQuery()
                        con.Close()

                        'update tax year model search grid
                        Rebind()

                    Case "toHistoryTaxYearModel"
                        'setup database connection
                        Dim con As New SqlClient.SqlConnection
                        con.ConnectionString = PATMAP.Global_asax.connString
                        con.Open()
                        Dim query As New SqlClient.SqlCommand
                        query.Connection = con

                        'check if the dataset is being used in another taxYrModel
                        Dim dr As SqlClient.SqlDataReader
                        query.CommandText = "select taxYearModelID, taxYearModelName from taxYearModelDescription where " & dataSetTblPrimaryKey & "=" & editDataSetID & " AND taxYearStatusID=" & ACTIVE
                        dr = query.ExecuteReader

                        If dr.Read() Then
                            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP115")
                            dr.Close()
                            con.Close()
                            Exit Sub

                        Else
                            If dataSetTblPrimaryKey = "assessmentID" Or dataSetTblPrimaryKey = "millRateSurveyID" Or dataSetTblPrimaryKey = "POVID" Then
                                query.CommandText = "select boundaryModelID from boundaryModel where " & dataSetTblPrimaryKey & "=" & editDataSetID & " AND status = 1"
                                dr.Close()
                                dr = query.ExecuteReader
                                If dr.Read() Then
                                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP322")
                                    dr.Close()
                                    con.Close()
                                    Exit Sub
                                End If
                            End If

                            'send tax year model to history
                            dr.Close()
                            query.CommandText = "update " & dataSetTblNm & " set statusID=" & HISTORY & " where " & dataSetTblPrimaryKey & "=" & editDataSetID
                            query.ExecuteNonQuery()






                        End If

                        con.Close()

                        'update tax year model search grid
                        Rebind()
                End Select
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        If e.CommandName = "editDataSet" Then
            Response.Redirect("editdataset.aspx")
        End If

    End Sub

    Private Sub Rebind()

        If grdDataSet.Rows.Count > 1 Then
            'update function search grid
            performDataSetSearch()
        Else
            Bind_Dll()
            grdDataSet.DataSource = Nothing
            grdDataSet.DataBind()
            txtTotal.Text = ""
        End If
    End Sub


    Private Sub grdDataSet_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdDataSet.RowDataBound
        Try
            'if current row is a datarow type
            If (e.Row.RowType = DataControlRowType.DataRow) Then

                Dim btnHistory As System.Web.UI.WebControls.LinkButton

                btnHistory = CType(e.Row.Cells.Item(2).Controls.Item(0), LinkButton)

                If DataBinder.Eval(e.Row.DataItem, "statusID") = HISTORY Then
                    'If user is searching records in history, change "To History" button
                    'to "Restore"
                    btnHistory.Text = "<img src='../images/btnSmRestore.gif'>"
                    btnHistory.CommandName = "restoreTaxYearModel"
                Else
                    'If user is searching active records, change back to "To History" button
                    btnHistory.Text = "<img src='../images/btnSmHistory.gif'>"
                    btnHistory.CommandName = "toHistoryTaxYearModel"
                End If

                'attaches confirm script to button
                common.ConfirmDel(e, 1, DataBinder.Eval(e.Row.DataItem, "dataSetName"))
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub ddlDataSource_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDataSource.SelectedIndexChanged
        Try
            Bind_Dll()
        Catch

            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try

    End Sub

    Private Sub Bind_Dll()
        Dim status As Integer

        If rdlStatus.SelectedIndex = 0 Or rdlStatus.SelectedIndex = -1 Then
            status = ACTIVE
        Else
            status = HISTORY
        End If

        'get the data source name selected
        Dim dsNm As String
        dsNm = ddlDataSource.SelectedItem.Text

        'Based on the selection of the data source, populate the data set list
        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString
        con.Open()

        Dim da As New SqlClient.SqlDataAdapter
        da.SelectCommand = New SqlClient.SqlCommand()
        da.SelectCommand.Connection = con

        Dim dt As New DataTable
        Dim constr As String

        Select Case dsNm
            'If the data source is assessment
            Case "Assessment"
                constr = "select 0 as dataSetID, '<Select>' as dataSetName union all select assessmentID, dataSetName from assessmentDescription where statusID = " & status
                da.SelectCommand.CommandText = constr
                da.Fill(dt)
                ddlDSN.DataSource = dt
                ddlDSN.DataValueField = "dataSetID"
                ddlDSN.DataTextField = "dataSetName"
                ddlDSN.DataBind()
                dt.Clear()
                'Donna - Removed Tax Credit.
                'Case "Tax Credit"
                '    constr = "select 0 as dataSetID, '<Select>' as dataSetName union all select taxCreditID, dataSetName from taxCreditDescription  where statusID = " & status
                '    da.SelectCommand.CommandText = constr
                '    da.Fill(dt)
                '    ddlDSN.DataSource = dt
                '    ddlDSN.DataValueField = "dataSetID"
                '    ddlDSN.DataTextField = "dataSetName"
                '    ddlDSN.DataBind()
                '    dt.Clear()

            Case "POV"
                constr = "select 0 as dataSetID, '<Select>' as dataSetName union all select POVID, dataSetName from POVDescription  where statusID = " & status
                da.SelectCommand.CommandText = constr
                da.Fill(dt)
                ddlDSN.DataSource = dt
                ddlDSN.DataValueField = "dataSetID"
                ddlDSN.DataTextField = "dataSetName"
                ddlDSN.DataBind()
                dt.Clear()

            Case "Mill Rate Survey"
                constr = "select 0 as dataSetID, '<Select>' as dataSetName union all select millRateSurveyID, dataSetName from millRateSurveyDescription  where statusID = " & status
                da.SelectCommand.CommandText = constr
                da.Fill(dt)
                ddlDSN.DataSource = dt
                ddlDSN.DataValueField = "dataSetID"
                ddlDSN.DataTextField = "dataSetName"
                ddlDSN.DataBind()
                dt.Clear()
                'Donna - Removed K12OG.
                'Case "K12OG"
                '    constr = "select 0 as dataSetID, '<Select>' as dataSetName union all select K12ID, dataSetName from K12Description  where statusID = " & status
                '    da.SelectCommand.CommandText = constr
                '    da.Fill(dt)
                '    ddlDSN.DataSource = dt
                '    ddlDSN.DataValueField = "dataSetID"
                '    ddlDSN.DataTextField = "dataSetName"
                '    ddlDSN.DataBind()
                '    dt.Clear()

            Case "Potash"
                constr = "select 0 as dataSetID, '<Select>' as dataSetName union all select potashID, dataSetName from potashDescription  where statusID = " & status
                da.SelectCommand.CommandText = constr
                da.Fill(dt)
                ddlDSN.DataSource = dt
                ddlDSN.DataValueField = "dataSetID"
                ddlDSN.DataTextField = "dataSetName"
                ddlDSN.DataBind()
                dt.Clear()

        End Select

        'clean up
        con.Close()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSearch.Click
        Try
            'make sure the required field (Data Source) is selected
            'If ddlDataSource.SelectedItem.Text = "<Select>" Then
            '    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP49")
            '    Exit Sub
            'End If

            'fill grid with search results
            performDataSetSearch()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub performDataSetSearch()

        Dim dataSourceNm As String
        Dim dataSetID As Integer
        Dim dataSetNm As String
        Dim status As String

        'get the values for the input fields from the form
        dataSourceNm = ddlDataSource.SelectedItem.Text
        dataSetID = CType(ddlDSN.SelectedValue, Integer)
        dataSetNm = Trim(txtDSN.Text)
        status = LCase(rdlStatus.SelectedItem.Text)

        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString
        con.Open()

        Dim constr As String = ""

        'based on the datasource name, contruct the base query 
        Select Case dataSourceNm
            Case "Assessment"
                constr = "select a.statusID, a.assessmentID As dataSetID, a.dataSetName from assessmentDescription a where 1=1"
                Session.Add("dataSource", "Assessment")
                'Donna - Removed Tax Credit.
                'Case "Tax Credit"
                '    constr = "select a.statusID, a.taxCreditID As dataSetID, a.dataSetName from taxCreditDescription a where 1=1"
                '    Session.Add("dataSource", "Tax Credit")

            Case "POV"
                constr = "select a.statusID, a.POVID As dataSetID, a.dataSetName from POVDescription a where 1=1"
                Session.Add("dataSource", "POV")

            Case "Mill Rate Survey"
                constr = "select a.statusID, a.millRateSurveyID As dataSetID, a.dataSetName from millRateSurveyDescription a where 1=1"
                Session.Add("dataSource", "Mill Rate Survey")
                'Donna - Removed K12OG.
                'Case "K12OG"
                '    constr = "select a.statusID, a.K12ID As dataSetID, a.dataSetName from K12Description a where 1=1"
                '    Session.Add("dataSource", "K12OG")

            Case "Potash"
                constr = "select a.statusID, a.potashID As dataSetID, a.dataSetName from potashDescription a where 1=1"
                Session.Add("dataSource", "Potash")

                'Continue for other data sources
        End Select

        'check the data set name selection
        If dataSetID <> 0 Then

            Select Case dataSourceNm

                Case "Assessment"
                    constr += " AND a.assessmentID=" & dataSetID
                    'Donna - Removed Tax Credit.
                    'Case "Tax Credit"
                    '    constr += " AND a.taxCreditID=" & dataSetID

                Case "POV"
                    constr += " AND a.POVID=" & dataSetID

                Case "Mill Rate Survey"
                    constr += " AND a.millRateSurveyID=" & dataSetID
                    'Donna - Removed K12OG.
                    'Case "K12OG"
                    '    constr += " AND a.K12ID=" & dataSetID

                Case "Potash"
                    constr += " AND a.potashID=" & dataSetID

            End Select
        ElseIf dataSetNm <> "" Then
            constr += " AND a.dataSetName like '%" & dataSetNm & "%'"
        End If

        'check the status selection - active/history
        Select Case status
            Case "active"
                constr += " AND a.StatusID=" & ACTIVE
            Case "history"
                constr += " AND a.StatusID=" & HISTORY
        End Select

        Dim da As New SqlClient.SqlDataAdapter
        da.SelectCommand = New SqlClient.SqlCommand(constr, con)
        Dim dt As New DataTable
        da.Fill(dt)
        grdDataSet.DataSource = dt
        grdDataSet.DataBind()

        'clean up
        con.Close()

        If IsNothing(Cache("dataset")) Then
            Cache.Add("dataset", dt, Nothing, Now().AddMinutes(PATMAP.Global_asax.cacheExpiration), Nothing, CacheItemPriority.Low, Nothing)
        Else
            Cache("dataset") = dt
        End If

        txtTotal.Text = dt.Rows.Count

        If dt.Rows.Count = 0 Then
            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP134")
        End If

    End Sub

    Private Sub grdDataSet_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdDataSet.PageIndexChanging
        Try
            Dim dt As DataTable

            'sets grid's page index
            grdDataSet.PageIndex = e.NewPageIndex

            'fill the grid
            If Not IsNothing(Cache("dataset")) Then
                dt = CType(Cache("dataset"), DataTable)
                grdDataSet.DataSource = dt
                grdDataSet.DataBind()
            Else
                performDataSetSearch()
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub
    Private Sub grdDataSet_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdDataSet.Sorting
        Try
            Dim dt As DataTable
            Dim dv As DataView

            'fill the grid if there's no data stored in cache
            If IsNothing(Cache("dataset")) Then
                performDataSetSearch()
            End If

            dt = CType(Cache("dataset"), DataTable)
            dv = dt.DefaultView
            'sort grid
            dv.Sort = common.SortGrid(e.SortExpression, dv.Sort)
            grdDataSet.DataSource = dt
            grdDataSet.DataBind()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub
   
    Private Sub rdlStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdlStatus.SelectedIndexChanged
        Bind_Dll()
    End Sub
End Class