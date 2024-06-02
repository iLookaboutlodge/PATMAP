Imports System.Data.Sql
Imports System.Data.SqlClient

Public Class SqlDbAccess

    'Public Shared SQLEngineServer As String = System.Configuration.ConfigurationManager.AppSettings("SQLEngineServer")
    Public Shared connString As String = PATMAP.Global_asax.connString
    ' "Data Source=" & SQLEngineServer & ";Initial Catalog=PATMAP;Persist Security Info=True;User ID=PATMAP;Password=iwantaccess;"


    Public Shared Function RunSqlReturnDataTable(ByVal CommandText As String) As DataTable
        Return RunSqlReturnDataSet(CommandText).Tables(0)
    End Function

    Public Shared Function RunSqlReturnDataSet(ByVal CommandText As String) As DataSet
        Dim con As SqlConnection = New SqlConnection
        con.ConnectionString = connString

        'Dim query As SqlCommand = New SqlCommand
        'query.Connection = con
        'query.CommandTimeout = 60000
        Dim da As SqlDataAdapter = New SqlDataAdapter
        da.SelectCommand = New SqlCommand
        da.SelectCommand.Connection = con
        da.SelectCommand.CommandTimeout = 60000
        da.SelectCommand.CommandText = CommandText
        Dim ds As DataSet = New DataSet
        Try
            con.Open()
            da.Fill(ds)
        Catch sqlEx As SqlException
            Throw New Exception(("ERROR - Database Access" + sqlEx.Message))
        Catch ex As Exception
            Throw New Exception("ERROR - Database Access", ex)
        Finally
            Try
                'Make DB connection is closed even error occurred
                con.Close()
            Catch
                'If Connection is not Open, then can't close. So, Ignore it.
            End Try
        End Try
        Return ds
    End Function

    Public Shared Function RunSql(ByVal CommandText As String) As Boolean
        Dim status As Boolean = True
        Dim con As SqlConnection = New SqlConnection
        con.ConnectionString = connString
        Dim query As SqlCommand = New SqlCommand
        query.Connection = con
        query.CommandText = CommandText
        query.CommandTimeout = 60000
        Try
            con.Open()
            query.ExecuteNonQuery()
        Catch sqlEx As SqlException
            status = False
            Throw New Exception(("ERROR - Database Access" + sqlEx.Message))
        Catch ex As Exception
            status = False
            Throw New Exception("ERROR - Database Access", ex)
        Finally
            Try
                con.Close()
                'Make DB connection is closed even error occurred
            Catch
                status = False
                'If Connection is not Open, then can't close. So, Ignore it.
            End Try
        End Try
        Return status
    End Function

    Public Shared Function RunSqlReturnIdentity(ByVal CommandText As String, ByRef Identity As String) As Boolean
        Dim status As Boolean = True
        Dim con As SqlConnection = New SqlConnection
        con.ConnectionString = connString

        Dim query As SqlCommand = New SqlCommand
        query.Connection = con
        query.CommandText = CommandText
        query.CommandTimeout = 60000
        Dim dr As SqlDataReader
        Try
            con.Open()
            dr = query.ExecuteReader
            dr.Read()
            Identity = dr.GetValue(0).ToString
        Catch sqlEx As SqlException
            status = False
            Throw New Exception(("ERROR - Database Access" + sqlEx.Message))
        Catch ex As Exception
            status = False
            Throw New Exception("ERROR - Database Access", ex)
        Finally
            Try
                con.Close()
                'Make DB connection is closed even error occurred
            Catch
                status = False
                'If Connection is not Open, then can't close. So, Ignore it.
            End Try
        End Try
        Return status
    End Function


    Public Shared Function RunSp(ByVal spName As String, ByVal param() As SqlParameter) As Integer
        Dim sqlConn As SqlConnection = New SqlConnection(connString)
        Dim sqlCmd As SqlCommand = New SqlCommand(spName, sqlConn)
        sqlCmd.CommandTimeout = 60000
        sqlCmd.CommandType = CommandType.StoredProcedure
        If (Not (param) Is Nothing) Then
            AddParam(sqlCmd, param)
        End If
        ' add the parameters to the sql
        Dim intRowsAffected As Integer = -1
        Dim retval As Integer = 0
        Try
            sqlConn.Open()
            intRowsAffected = CType(sqlCmd.ExecuteNonQuery, Integer)
            retval = CType(sqlCmd.Parameters("@ReturnVal").Value, Integer)
        Catch sqlEx As SqlException
            Throw New Exception(sqlEx.Message)
        Catch ex As Exception
            Throw New Exception(("ERROR - Database Access" + ex.Message))
        Finally
            Try
                'Make DB connection is closed even error occurred
                sqlConn.Close()
            Catch
                'If Connection is not Open, then can't close. So, Ignore it.
            End Try
        End Try
        Return retval
    End Function

    Private Shared Sub AddParam(ByRef sqlCmd As SqlCommand, ByVal param() As SqlParameter)
        For Each p As SqlParameter In param
            If ((p.Direction = ParameterDirection.InputOutput) _
                        OrElse (p.Value = Nothing)) Then
                p.Value = DBNull.Value
            End If
            sqlCmd.Parameters.Add(p)
        Next
    End Sub




End Class
