Public Partial Class sorttaxclass
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                'setup database connection
                Dim con As New SqlClient.SqlConnection
                con.ConnectionString = PATMAP.Global_asax.connString
                con.Open()

                Dim da As New SqlClient.SqlDataAdapter
                da.SelectCommand = New SqlClient.SqlCommand()
                da.SelectCommand.Connection = con
                da.SelectCommand.CommandText = "select  taxClassID + ',' + parentTaxClassID as taxClassID, taxClass from taxClasses order by sort"

                Dim dt As New DataTable
                da.Fill(dt)

                'loop through the result and populate the listbox
                Dim dr As DataRow
                For Each dr In dt.Rows
                    Dim lstItemValue As String = dr(0)

                    Dim parentTaxClassID As String = dr(0).ToString
                    Dim lstItemText As String
                    If parentTaxClassID.Substring(parentTaxClassID.IndexOf(",") + 1) = "none" Then
                        lstItemText = dr(1)
                    Else
                        lstItemText = "." & vbTab & vbTab & dr(1)
                    End If

                    Dim lstItem As New ListItem(lstItemText, lstItemValue)
                    lstTaxClass.Items.Add(lstItem)
                Next

                'lstTaxClass.DataSource = dt
                'lstTaxClass.DataValueField = "taxClassID"
                'lstTaxClass.DataTextField = "taxClass"
                'lstTaxClass.DataBind()

                con.Close()
            End If
            lblErrorText.Text = ""
        Catch
            'retrieves error message
            lblErrorText.Text = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub btnMoveUp_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnMoveUp.Click
        Try
            Dim currentTaxClassIDIndex As Integer = lstTaxClass.SelectedIndex

            If currentTaxClassIDIndex = 0 Then
                Exit Sub
            End If

            Dim separator(1) As Char
            separator(0) = ","

            Dim list(2) As String

            list = lstTaxClass.Items(currentTaxClassIDIndex - 1).Value.Split(separator)
            Dim previousTaxClassID As String = list(0)
            Dim previousParentTaxClassID As String = list(1)
            list = lstTaxClass.SelectedValue.Split(separator)
            Dim currentTaxClassID As String = list(0)
            Dim currentParentTaxClassID As String = list(1)

            If previousParentTaxClassID = currentParentTaxClassID And previousParentTaxClassID <> "none" And currentParentTaxClassID <> "none" Then
                Dim pervLstItem As ListItem

                pervLstItem = lstTaxClass.Items(currentTaxClassIDIndex - 1)

                lstTaxClass.Items.Remove(pervLstItem)
                lstTaxClass.Items.Insert(currentTaxClassIDIndex, pervLstItem)

            ElseIf previousParentTaxClassID = currentParentTaxClassID Or currentParentTaxClassID = "none" Then
                Dim lstItems As New ArrayList()
                Dim lstItem As ListItem

                'remove the currently selected class and associated the subclasses from the list box
                'and store them in the array list
                lstItem = lstTaxClass.SelectedItem
                lstItems.Add(lstItem)
                lstTaxClass.Items.Remove(lstItem)

                Dim nextParentTaxClassID As String
                While (currentTaxClassIDIndex < lstTaxClass.Items.Count)
                    list = lstTaxClass.Items(currentTaxClassIDIndex).Value.Split(separator)
                    nextParentTaxClassID = list(1)

                    If nextParentTaxClassID = currentTaxClassID Then
                        lstItem = lstTaxClass.Items(currentTaxClassIDIndex)
                        lstItems.Add(lstItem)
                        lstTaxClass.Items.Remove(lstItem)
                    Else
                        Exit While
                    End If
                End While

                'insert the classes and subclasses back into the list box at the correct index
                Dim moveAtListItem As ListItem
                Dim moveAtIndex As Integer

                If previousParentTaxClassID <> "none" Then
                    moveAtListItem = lstTaxClass.Items.FindByValue(previousParentTaxClassID & ",none")
                    moveAtIndex = lstTaxClass.Items.IndexOf(moveAtListItem)
                Else
                    moveAtIndex = currentTaxClassIDIndex - 1
                End If

                For Each lstItem In lstItems
                    lstTaxClass.Items.Insert(moveAtIndex, lstItem)
                    lstTaxClass.Items(moveAtIndex).Attributes.Add("style", "background-color:DodgerBlue")
                    moveAtIndex = moveAtIndex + 1
                Next
            Else
                lblErrorText.Text = PATMAP.common.GetErrorMessage("PATMAP125")
            End If
        Catch
            'retrieves error message
            lblErrorText.Text = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub btnMoveDown_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnMoveDown.Click
        Try
            Dim currentTaxClassIDIndex As Integer = lstTaxClass.SelectedIndex

            If currentTaxClassIDIndex = lstTaxClass.Items.Count - 1 Then
                Exit Sub
            End If

            Dim separator(1) As Char
            separator(0) = ","

            Dim list(2) As String

            list = lstTaxClass.Items(currentTaxClassIDIndex + 1).Value.Split(separator)
            Dim nextTaxClassID As String = list(0)
            Dim nextParentTaxClassID As String = list(1)

            list = lstTaxClass.SelectedValue.Split(separator)
            Dim currentTaxClassID As String = list(0)
            Dim currentParentTaxClassID As String = list(1)

            list = lstTaxClass.Items(lstTaxClass.Items.Count - 1).Value.Split(separator)
            If list(1) = currentTaxClassID Then
                Exit Sub
            End If

            If nextParentTaxClassID = currentParentTaxClassID And nextParentTaxClassID <> "none" And currentParentTaxClassID <> "none" Then
                Dim nextLstItem As ListItem

                nextLstItem = lstTaxClass.Items(currentTaxClassIDIndex + 1)

                lstTaxClass.Items.Remove(nextLstItem)
                lstTaxClass.Items.Insert(currentTaxClassIDIndex, nextLstItem)

            ElseIf nextParentTaxClassID = currentParentTaxClassID Or currentParentTaxClassID = "none" Then
                Dim lstItems As New ArrayList()
                Dim lstItem As ListItem

                'remove the currently selected class and associated the subclasses from the list box
                'and store them in the array list
                lstItem = lstTaxClass.SelectedItem
                lstItems.Add(lstItem)
                lstTaxClass.Items.Remove(lstItem)

                Dim subsequentParentTaxClassID As String
                Dim moveAtIndex As Integer
                While (True)
                    list = lstTaxClass.Items(currentTaxClassIDIndex).Value.Split(separator)
                    subsequentParentTaxClassID = list(1)

                    If subsequentParentTaxClassID = currentTaxClassID Then
                        lstItem = lstTaxClass.Items(currentTaxClassIDIndex)
                        lstItems.Add(lstItem)
                        lstTaxClass.Items.Remove(lstItem)
                    Else
                        moveAtIndex = currentTaxClassIDIndex + 1
                        Dim index As Integer = 1
                        While (currentTaxClassIDIndex + index < lstTaxClass.Items.Count)
                            list = lstTaxClass.Items(currentTaxClassIDIndex + index).Value.Split(separator)
                            subsequentParentTaxClassID = list(1)
                            If subsequentParentTaxClassID = "none" Then
                                Exit While
                            End If
                            moveAtIndex = currentTaxClassIDIndex + index + 1
                            index = index + 1
                        End While
                        Exit While
                    End If
                End While

                'insert the classes and subclasses back into the list box at the correct index
                For Each lstItem In lstItems
                    lstTaxClass.Items.Insert(moveAtIndex, lstItem)
                    lstTaxClass.Items(moveAtIndex).Attributes.Add("style", "background-color:DodgerBlue")
                    moveAtIndex = moveAtIndex + 1
                Next
            Else
                lblErrorText.Text = PATMAP.common.GetErrorMessage("PATMAP126")
            End If
        Catch
            'retrieves error message
            lblErrorText.Text = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSave.Click
        Try
            Dim index As Integer = 0
            Dim query As String = ""
            While (index < lstTaxClass.Items.Count)
                Dim separator(1) As Char
                separator(0) = ","

                Dim list(2) As String

                list = lstTaxClass.Items(index).Value.Split(separator)
                query += "update taxClasses set sort=" & index + 1 & " where taxClassID='" & list(0) & "'" & vbCrLf
                index = index + 1
            End While

            'setup database connection
            Dim con As New SqlClient.SqlConnection
            con.ConnectionString = PATMAP.Global_asax.connString
            con.Open()

            Dim cmd As New SqlClient.SqlCommand
            cmd.Connection = con
            cmd.CommandText = query

            Dim trans As SqlClient.SqlTransaction
            trans = con.BeginTransaction()
            cmd.Transaction = trans

            Try
                cmd.ExecuteNonQuery()
                trans.Commit()
            Catch
                trans.Rollback()
                lblErrorText.Text = PATMAP.common.GetErrorMessage("PATMAP101")
                con.Close()
                Exit Sub
            End Try

            Response.Write("<script language='javascript'> { window.close();}</script>")

        Catch
            'retrieves error message
            lblErrorText.Text = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    Private Sub lstTaxClass_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstTaxClass.SelectedIndexChanged
        lstTaxClass.BackColor = Drawing.Color.White

        Dim separator(1) As Char
        separator(0) = ","

        Dim list(2) As String

        list = lstTaxClass.SelectedItem.Value.Split(separator)
        Dim taxClassID As String = list(0)
        Dim parentTaxClassID As String = list(1)

        Dim index As Integer = lstTaxClass.SelectedIndex + 1
        If parentTaxClassID = "none" Then
            lstTaxClass.SelectedItem.Attributes.Add("style", "background-color:DodgerBlue")
            While (index < lstTaxClass.Items.Count)
                list = lstTaxClass.Items(index).Value.Split(separator)
                If list(1) = taxClassID Then
                    lstTaxClass.Items(index).Attributes.Add("style", "background-color:DodgerBlue")
                    index = index + 1
                Else
                    Exit While
                End If
            End While
        End If
    End Sub
End Class