Public Partial Class editpotash
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Clears out the error message
            Master.errorMsg = ""
            If Not IsPostBack Then
                'Sets submenu to be displayed
                subMenu.setStartNode(menu.editData)
                If (Not (Session("potash_RowID")) Is Nothing) Then
                    CurrentRowID = Session("potash_RowID").ToString
                    CurrentPotashType = Session("potash_PotashType").ToString
                    ispotashNew = False
                    BindData_Currentpotash()
                    'Update
                    tr_DSN.Visible = False
                    tr_MunicipalityType.Visible = False
                    PotashTypeRelatedFields()
                Else
                    ispotashNew = True
                    BindData_ddl_DataSetName()
                    'new
                End If
            End If
        Catch
            'retrieves error message
            Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
        End Try
    End Sub

    'Private Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
    '    Try
    '        'change page title and breadcrumb to 
    '        'if the mode is either edit or add
    '        common.ChangeTitle(Session("potashAreaID"), lblTitle)
    '    Catch
    '        'retrieves error message
    '        Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
    '    End Try
    'End Sub

    'Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
    '    Try
    '        common.UndoChange()
    '    Catch
    '        'retrieves error message
    '        Master.errorMsg = common.GetErrorMessage(Err.Number, Err)
    '    End Try
    'End Sub




    Protected Property ispotashNew() As Boolean
        Get
            Return Boolean.Parse(ViewState("ispotashNew").ToString)
        End Get
        Set(ByVal value As Boolean)
            ViewState("ispotashNew") = value
        End Set
    End Property

    Protected Property CurrentRowID() As String
        Get
            Return ViewState("CurrentRowID").ToString
        End Get
        Set(ByVal value As String)
            ViewState("CurrentRowID") = value
        End Set
    End Property

    Protected Property CurrentPotashType() As String
        Get
            Return ViewState("CurrentPotashType").ToString
        End Get
        Set(ByVal value As String)
            ViewState("CurrentPotashType") = value
        End Set
    End Property

    Protected Property potashID() As Integer
        Get
            Return Integer.Parse(ViewState("potashID").ToString)
        End Get
        Set(ByVal value As Integer)
            ViewState("potashID") = value.ToString
        End Set
    End Property

    Protected Sub PotashTypeRelatedFields()
        If CurrentPotashType.ToLower.Equals("rural") Then
            tr_areaInSquareMiles.Visible = True
            tr_statutoryDiscountPercentage.Visible = True
            tr_millRateFactor.Visible = True
            tr_boardAdjustments.Visible = True
        End If
        If CurrentPotashType.ToLower.Equals("urban") Then
            tr_areaInSquareMiles.Visible = False
            tr_statutoryDiscountPercentage.Visible = False
            tr_millRateFactor.Visible = False
            tr_boardAdjustments.Visible = False
        End If
    End Sub

    Protected Sub BindData_Currentpotash()
        Dim CommandText As String = "select * from potash" + CurrentPotashType + " where RowID = '" + CurrentRowID + "'"
        Dim dt As DataTable = SqlDbAccess.RunSqlReturnDataTable(CommandText)
        potashID = Integer.Parse(dt.Rows(0)("potashID").ToString)
        Dim MunicipalityID As String = dt.Rows(0)("MunicipalityID").ToString
        txtpotashAreaID.Text = dt.Rows(0)("potashAreaID").ToString
        If CurrentPotashType.ToLower.Equals("rural") Then
            txtareaInSquareMiles.Text = dt.Rows(0)("areaInSquareMiles").ToString
            txtstatutoryDiscountPercentage.Text = dt.Rows(0)("statutoryDiscountPercentage").ToString
            txtmillRateFactor.Text = dt.Rows(0)("millRateFactor").ToString
            txtboardAdjustments.Text = dt.Rows(0)("boardAdjustments").ToString
        End If
        txttotalPoints.Text = dt.Rows(0)("totalPoints").ToString
        txttotalGrant.Text = dt.Rows(0)("totalGrant").ToString
        BindData_ddl(MunicipalityID)
    End Sub

    Protected Sub BindData_ddl_DataSetName()
        Dim CommandText As String = " select potashID, dataSetName from potashDescription where statusID = 1 order by dataSetName"
        Dim ds As DataSet = SqlDbAccess.RunSqlReturnDataSet(CommandText)
        If (ds.Tables(0).Rows.Count > 0) Then
            ddlDSN.DataSource = ds.Tables(0)
            ddlDSN.DataValueField = "potashID"
            ddlDSN.DataTextField = "dataSetName"
            ddlDSN.DataBind()
            potashID = Integer.Parse(ddlDSN.SelectedValue.ToString)
            ddlMunicipalityType.Items.Insert(0, "Rural")
            ddlMunicipalityType.Items.Insert(1, "Urban")
            CurrentPotashType = ddlMunicipalityType.SelectedValue
            BindData_ddl("")
            PotashTypeRelatedFields()
        Else
            Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP303")
            btnSave.Visible = False
        End If
    End Sub

    Protected Sub ddlDSN_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        potashID = Integer.Parse(ddlDSN.SelectedValue.ToString)
        BindData_ddl("")
    End Sub

    Protected Sub ddlMunicipalityType_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        CurrentPotashType = ddlMunicipalityType.SelectedValue
        BindData_ddl("")
        PotashTypeRelatedFields()
    End Sub

    Protected Sub BindData_ddl(ByVal MunicipalityID As String)
        Dim CommandText As String = " select number from entities where "
        If CurrentPotashType.ToLower.Equals("rural") Then
            CommandText = CommandText + " jurisdictionTypeID not in (1,2,3,4,5,6,7,8,10,11) "
        End If
        If CurrentPotashType.ToLower.Equals("urban") Then
            CommandText = CommandText + " jurisdictionTypeID not in (1,9) "
        End If
        CommandText = (CommandText + (" and number not in (select MunicipalityID from potash" _
                    + (CurrentPotashType + (" where potashID = '" _
                    + (potashID.ToString + ("')" + " order by number"))))))
        Dim ds As DataSet = SqlDbAccess.RunSqlReturnDataSet(CommandText)
        ddlMunicipalityID.DataSource = ds.Tables(0)
        ddlMunicipalityID.DataTextField = "number"
        ddlMunicipalityID.DataBind()
        If ispotashNew Then
            ddlMunicipalityID.Items.Insert(0, New ListItem("Select Data", "-1"))
        Else
            ddlMunicipalityID.Items.Insert(0, MunicipalityID)
            ddlMunicipalityID.Items.FindByValue(MunicipalityID).Selected = True
        End If
        'string CommandText = "";
        'if (CurrentPotashType.ToLower().Equals("rural"))
        '{
        '    CommandText = " select distinct municipalityID from potashRural where potashID = '" + potashID.ToString() + "' order by municipalityID";
        '}
        'if (CurrentPotashType.ToLower().Equals("urban"))
        '{
        '    CommandText = " select distinct municipalityID from potashUrban where potashID = '" + potashID.ToString() + "' order by municipalityID";
        '}
        'DataSet ds = SqlDbAccess.RunSqlReturnDataSet(CommandText);
        'ddlMunicipalityID.DataSource = ds.Tables[0];
        'ddlMunicipalityID.DataTextField = "municipalityID";
        'ddlMunicipalityID.DataBind();
        'if (ispotashNew)//New
        '{
        '    ddlMunicipalityID.Items.Insert(0, new ListItem("Select Data", "-1"));
        '}
        'else//Update
        '{
        '    ddlMunicipalityID.Items.FindByValue(MunicipalityID).Selected = true;
        '}
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        If (Not ddlMunicipalityID.SelectedValue.Equals("-1") _
                    AndAlso (Not txtpotashAreaID.Text.Trim.Equals(String.Empty) _
                    AndAlso (Not txttotalPoints.Text.Trim.Equals(String.Empty) _
                    AndAlso (Not txttotalGrant.Text.Trim.Equals(String.Empty) _
                    AndAlso ((CurrentPotashType.ToLower.Equals("rural") _
                    AndAlso (Not txtareaInSquareMiles.Text.Trim.Equals(String.Empty) _
                    AndAlso (Not txtstatutoryDiscountPercentage.Text.Trim.Equals(String.Empty) _
                    AndAlso (Not txtmillRateFactor.Text.Trim.Equals(String.Empty) _
                    AndAlso Not txtboardAdjustments.Text.Trim.Equals(String.Empty))))) _
                    OrElse CurrentPotashType.ToLower.Equals("urban")))))) Then
            Dim CommandText As String = ""
            If ispotashNew Then
                CommandText = " INSERT INTO [potash" + CurrentPotashType + "]" _
                    + "([potashID],[potashAreaID],[municipalityID],[totalPoints],[totalGrant]"
                If CurrentPotashType.ToLower.Equals("rural") Then
                    CommandText = CommandText + ",[areaInSquareMiles],[statutoryDiscountPercentage],[millRateFactor],[boardAdjustments]"
                End If
                CommandText = CommandText + ") VALUES (" _
                + "'" + potashID.ToString() + "'" _
                + ",'" + txtpotashAreaID.Text + "'" _
                + ",'" + ddlMunicipalityID.SelectedValue + "'" _
                + ",'" + txttotalPoints.Text + "'" _
                + ",'" + txttotalGrant.Text + "'"
                If CurrentPotashType.ToLower.Equals("rural") Then
                    CommandText = CommandText _
                + ",'" + txtareaInSquareMiles.Text + "'" _
                + ",'" + txtstatutoryDiscountPercentage.Text + "'" _
                + ",'" + txtmillRateFactor.Text + "'" _
                + ",'" + txtboardAdjustments.Text + "'"
                End If
                CommandText = (CommandText + ")")
                If SqlDbAccess.RunSql(CommandText) Then
                    Session("potash_RowID") = Nothing
                    Session("potash_PotashType") = Nothing
                    Response.Redirect("viewpotash.aspx")
                Else
                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP304")
                End If
            Else
                CommandText = " UPDATE [potash" + CurrentPotashType + "] SET " _
                    + "[potashID] = '" + potashID.ToString() + "'" _
                    + ",[potashAreaID] = '" + txtpotashAreaID.Text + "'" _
                    + ",[municipalityID] = '" + ddlMunicipalityID.SelectedValue + "'" _
                    + ",[totalPoints] = '" + txttotalPoints.Text + "'" _
                    + ",[totalGrant] = '" + txttotalGrant.Text + "'"
                If CurrentPotashType.ToLower.Equals("rural") Then
                    CommandText = CommandText _
                    + ",[areaInSquareMiles] = '" + txtareaInSquareMiles.Text + "'" _
                    + ",[statutoryDiscountPercentage] = '" + txtstatutoryDiscountPercentage.Text + "'" _
                    + ",[millRateFactor] = '" + txtmillRateFactor.Text + "'" _
                    + ",[boardAdjustments] = '" + txtboardAdjustments.Text + "'"
                End If
                CommandText = CommandText + " WHERE [RowID]='" + CurrentRowID + "'"
                If SqlDbAccess.RunSql(CommandText) Then

                    'MTE code Start
                    CommandText = "update taxYearModelDescription set dataStale = 1 where taxYearStatusID in (1,3) and PotashID = (select PotashID from potash" + CurrentPotashType + " where RowID = '" + CurrentRowID + "')" + vbCrLf _
                                                    + "update assessmentTaxModel set dataStale = 1 where SubjectTaxYearModelID in (select taxYearModelID from taxYearModelDescription where PotashID = (select PotashID from potash" + CurrentPotashType + " where RowID = '" + CurrentRowID + "'))" + vbCrLf _
                                                    + "update assessmentTaxModel set dataStale = 1 where BaseTaxYearModelID in (select taxYearModelID from taxYearModelDescription where PotashID = (select PotashID from potash" + CurrentPotashType + " where RowID = '" + CurrentRowID + "'))" + vbCrLf _
                                                    + "update liveAssessmentTaxModel set dataStale = 1 where SubjectTaxYearModelID in (select taxYearModelID from taxYearModelDescription where PotashID = (select PotashID from potash" + CurrentPotashType + " where RowID = '" + CurrentRowID + "'))" + vbCrLf _
                                                    + "update liveAssessmentTaxModel set dataStale = 1 where BaseTaxYearModelID in (select taxYearModelID from taxYearModelDescription where PotashID = (select PotashID from potash" + CurrentPotashType + " where RowID = '" + CurrentRowID + "'))"
                    SqlDbAccess.RunSql(CommandText)
                    'MTE code End

                    Session("potash_RowID") = Nothing
                    Session("potash_PotashType") = Nothing
                    Response.Redirect("viewpotash.aspx")
                Else
                    Master.errorMsg = PATMAP.common.GetErrorMessage("PATMAP305")
                End If
            End If
        Else
            Dim message As String = ""
            If ddlMunicipalityID.SelectedValue.Equals("-1") Then
                message = (message + "<br>Select Municipality ID Please")
            End If
            If txtpotashAreaID.Text.Trim.Equals(String.Empty) Then
                message = (message + "<br>Type potashAreaID Please")
            End If
            If txttotalPoints.Text.Trim.Equals(String.Empty) Then
                message = (message + "<br>Type totalPoints Please")
            End If
            If txttotalGrant.Text.Trim.Equals(String.Empty) Then
                message = (message + "<br>Type totalGrant Please")
            End If
            If CurrentPotashType.ToLower.Equals("rural") Then
                If txtareaInSquareMiles.Text.Trim.Equals(String.Empty) Then
                    message = (message + "<br>Type areaInSquareMiles Please")
                End If
                If txtstatutoryDiscountPercentage.Text.Trim.Equals(String.Empty) Then
                    message = (message + "<br>Type statutoryDiscountPercentage Please")
                End If
                If txtmillRateFactor.Text.Trim.Equals(String.Empty) Then
                    message = (message + "<br>Type millRateFactor Please")
                End If
                If txtboardAdjustments.Text.Trim.Equals(String.Empty) Then
                    message = (message + "<br>Type boardAdjustments Please")
                End If
            End If
            Master.errorMsg = message
        End If
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Response.Redirect("viewpotash.aspx")
    End Sub
End Class