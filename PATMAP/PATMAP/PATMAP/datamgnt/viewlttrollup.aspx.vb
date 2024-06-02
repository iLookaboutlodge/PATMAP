Partial Public Class viewlttrollup
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Clears out the error message
            Master.errorMsg = ""

            If Not Page.IsPostBack Then
                'Sets submenu to be displayed
                subMenu.setStartNode(menu.editData)

                grdRollupClass.PageSize = PATMAP.Global_asax.pageSize

                'setup database connection
                Dim con As New SqlClient.SqlConnection
                con.ConnectionString = PATMAP.Global_asax.connString

                'get main tax classes for drop down
                Dim da As New SqlClient.SqlDataAdapter
                da.SelectCommand = New SqlClient.SqlCommand("select '0' as taxClassID,'<Select>' as taxClass union select taxClassID, taxClass from LTTmainTaxClasses where active = '1'", con)
                Dim dt As New DataTable
                con.Open()
                da.Fill(dt)
                con.Close()
                ddlMainClass.DataSource = dt
                ddlMainClass.DataValueField = "taxClassID"
                ddlMainClass.DataTextField = "taxClass"
                ddlMainClass.DataBind()

                ' '' '' '' '' ''get tax status for drop down
                ' '' '' '' '' ''da = New SqlClient.SqlDataAdapter
                ' '' '' '' '' ''da.SelectCommand = New SqlClient.SqlCommand("select 0 as taxStatusID,'<Select>' as taxStatus union select taxStatusID, taxStatus from taxStatus", con)
                ' '' '' '' '' ''dt = New DataTable
                ' '' '' '' '' ''con.Open()
                ' '' '' '' '' ''da.Fill(dt)
                ' '' '' '' '' ''con.Close()
                ' '' '' '' '' ''ddlTaxStatus.DataSource = dt
                ' '' '' '' '' ''ddlTaxStatus.DataValueField = "taxStatusID"
                ' '' '' '' '' ''ddlTaxStatus.DataTextField = "taxStatus"
                ' '' '' '' '' ''ddlTaxStatus.DataBind()

                ' '' '' '' '' ''get present use codes for drop down
                '' '' '' '' ''da = New SqlClient.SqlDataAdapter
                '' '' '' '' ''da.SelectCommand = New SqlClient.SqlCommand("select 0 as presentUseCodeID,'<Select>' as shortDescription union select presentUseCodeID, shortDescription from presentUseCodes order by presentUseCodeID", con)
                '' '' '' '' ''dt = New DataTable
                '' '' '' '' ''con.Open()
                '' '' '' '' ''da.Fill(dt)
                '' '' '' '' ''con.Close()
                '' '' '' '' ''ddlPropCode.DataSource = dt
                '' '' '' '' ''ddlPropCode.DataValueField = "presentUseCodeID"
                '' '' '' '' ''ddlPropCode.DataTextField = "shortDescription"
                '' '' '' '' ''ddlPropCode.DataBind()


            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try

    End Sub

    Protected Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAdd.Click
        Session.Add("editRollupClassID", "0")
        Response.Redirect("editlttrollup.aspx")
    End Sub


    Protected Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSearch.Click
        Try
            performTaxClassSearch()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try

    End Sub

    Private Sub performTaxClassSearch()
        'setup database connection
        Dim con As New SqlClient.SqlConnection
        con.ConnectionString = PATMAP.Global_asax.connString

        'setup the query to get entity details
        Dim da As New SqlClient.SqlDataAdapter
        da.SelectCommand = New SqlClient.SqlCommand()
        da.SelectCommand.Connection = con

        If Not (String.IsNullOrEmpty(ddlMainClass.SelectedValue) Or ddlMainClass.SelectedValue = "0") Then
            da.SelectCommand.CommandText = "select t.taxClassID, t.taxClass, t.description from LTTmainTaxClasses t where t.taxClassID='" & ddlMainClass.SelectedValue & "'"
        Else
            da.SelectCommand.CommandText = "select t.taxClassID, t.taxClass, t.description from LTTmainTaxClasses t where active = '1'"
        End If

        '' '' '' '' ''If chkActive.Checked = True Then
        '' '' '' '' ''    da.SelectCommand.CommandText += " and t.active = 1"
        '' '' '' '' ''End If
        '' '' '' '' ''If chkDefault.Checked = True Then
        '' '' '' '' ''    da.SelectCommand.CommandText += " and [default] = 1"
        '' '' '' '' ''End If

        'get the data for the entity grid
        Dim dt As New DataTable
        con.Open()
        da.Fill(dt)
        con.Close()
        grdRollupClass.DataSource = dt
        grdRollupClass.DataBind()

        If IsNothing(Cache("rollupClasses")) Then
            Cache.Add("rollupClasses", dt, Nothing, Now().AddMinutes(PATMAP.Global_asax.cacheExpiration), Nothing, CacheItemPriority.Low, Nothing)
        Else
            Cache("rollupClasses") = dt
        End If
        txtTotal.Text = dt.Rows.Count

        If dt.Rows.Count = 0 Then
            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP134")
        End If
    End Sub

    Private Sub grdTaxClass_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdRollupClass.PageIndexChanging
        Try
            Dim dt As DataTable

            'sets grid's page index
            grdRollupClass.PageIndex = e.NewPageIndex

            'fill the grid
            If Not IsNothing(Cache("rollupClasses")) Then
                dt = CType(Cache("rollupClasses"), DataTable)
                grdRollupClass.DataSource = dt
                grdRollupClass.DataBind()
            Else
                performTaxClassSearch()
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub grdTaxClass_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdRollupClass.RowCommand
        Try

            'If grid is not being sorted
            If LCase(e.CommandName) <> "sort" And LCase(e.CommandName) <> "page" Then
                'get selected row index and corresponding userID to that row
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim editRollupClassID As String = grdRollupClass.DataKeys(index).Values("taxClassID")

                'check what type of command has been fired by the grid
                Select Case e.CommandName
                    Case "editRollupClass"
                        Session.Add("editRollupClassID", editRollupClassID)

                    Case "deleteRollupClass"
                        'setup database connection
                        Dim con As New SqlClient.SqlConnection
                        con.ConnectionString = PATMAP.Global_asax.connString
                        Dim query As New SqlClient.SqlCommand
                        query.Connection = con
                        con.Open()

                        Dim dr As SqlClient.SqlDataReader
                        query.CommandText = "select taxClassID from LTTtaxClasses where LTTmainTaxClassID ='" & editRollupClassID & "'"
                        dr = query.ExecuteReader
                        If dr.Read() Then
                            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP158")
                            con.Close()
                            Exit Sub
                        End If

                        'delete rollup class (actually just makes it inactive)
                        dr.Close()
                        query.CommandText = "update LTTmainTaxClasses set active = '0' where taxClassID ='" & editRollupClassID & "'" & vbCrLf
                        '' '' '' '' ''query.CommandText += "delete EDPOV from EDPOV, EDPOVDescription where EDPOV.EDPOVID = EDPOVDescription.EDPOVID and EDPOV.taxClassID = '" & editRollupClassID & "'" & vbCrLf
                        '' '' '' '' ''query.CommandText += "delete POV from POV, POVDescription where POV.POVID = POVDescription.POVID and POV.taxClassID = '" & editRollupClassID & "'" & vbCrLf
                        '' '' '' '' ''query.CommandText += "delete taxCredit from taxCredit, taxCreditDescription where taxCredit.taxCreditID = taxCreditDescription.taxCreditID and taxCredit.taxClassID = '" & editRollupClassID & "'" & vbCrLf
                        '' '' '' '' ''query.CommandText += "delete PMR from PMR, PMRDescription where PMR.PMRID = PMRDescription.PMRID and PMR.taxClassID = '" & editRollupClassID & "'" & vbCrLf
                        '' '' '' '' ''query.CommandText += "delete taxClassesPermission from taxClassesPermission, levels where taxClassesPermission.levelID = levels.levelID and taxClassesPermission.taxClassID = '" & editRollupClassID & "'" & vbCrLf
                        '' '' '' '' ''query.CommandText += "update assessment set taxClassID=taxClassID_orig from assessment where taxClassID= '" & editRollupClassID & "' and presentUseCodeID in (select presentUseCodeID from taxClassesUpdated where taxClassID = '" & editRollupClassID & "')" & vbCrLf
                        '' '' '' '' ''query.CommandText += "update assessmentDescription set dataStale = 1 where assessmentID in (select assessmentID from assessment where presentUseCodeID in (select presentUseCodeID from taxClassesUpdated where taxClassID = '" & editRollupClassID & "') group by assessmentID)" & vbCrLf
                        '' '' '' '' ''query.CommandText += "delete from taxClassesUpdated where taxClassID = '" & editRollupClassID & "'" & vbCrLf
                        '' '' '' '' ''query.CommandText += "delete from LTTtaxClasses where taxClassID = '" & editRollupClassID & "'" & vbCrLf
                        '' '' '' '' ''query.CommandText += "delete from taxClasses where taxClassID = '" & editRollupClassID & "'" & vbCrLf
                        '' '' '' '' ''query.CommandText += "DELETE FROM PEMRTaxClasses WHERE taxClassID = '" & editRollupClassID & "'" & vbCrLf

                        Dim trans As SqlClient.SqlTransaction
                        trans = con.BeginTransaction()
                        query.Transaction = trans

                        Try
                            query.CommandTimeout = 6000
                            query.ExecuteNonQuery()
                            trans.Commit()

                            'get main tax classes for drop down
                            Dim da As New SqlClient.SqlDataAdapter
                            da.SelectCommand = New SqlClient.SqlCommand("select '0' as taxClassID,'<Select>' as taxClass union select taxClassID, taxClass from LTTmainTaxClasses where active = '1'", con)
                            Dim dt As New DataTable
                            da.Fill(dt)
                            ddlMainClass.DataSource = dt
                            ddlMainClass.DataValueField = "taxClassID"
                            ddlMainClass.DataTextField = "taxClass"
                            ddlMainClass.DataBind()

                        Catch
                            trans.Rollback()
                            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP101")
                            con.Close()
                            Exit Sub
                        End Try

                        con.Close()

                        'update user search grid
                        performTaxClassSearch()

                        Master.errorMsg = ""
                End Select
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
            Exit Sub
        End Try

        If e.CommandName = "editRollupClass" Then
            Response.Redirect("editlttrollup.aspx")
        End If

    End Sub

    Private Sub grdRollupClass_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdRollupClass.RowDataBound
        Try
            'attaches confirm script to button
            common.ConfirmDel(e, 1, DataBinder.Eval(e.Row.DataItem, "taxClass"))
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub grdRollupClass_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdRollupClass.Sorting
        Try
            Dim dt As DataTable
            Dim dv As DataView

            'fill the grid if there's no data stored in cache
            If IsNothing(Cache("rollupClasses")) Then
                performTaxClassSearch()
            End If

            dt = CType(Cache("rollupClasses"), DataTable)
            dv = dt.DefaultView
            'sort grid
            dv.Sort = common.SortGrid(e.SortExpression, dv.Sort)
            grdRollupClass.DataSource = dt
            grdRollupClass.DataBind()
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    'Private Sub lnkSortTaxClass_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkSortTaxClass.Click
    '    Response.Redirect("sorttaxclass.aspx")
    'End Sub
End Class