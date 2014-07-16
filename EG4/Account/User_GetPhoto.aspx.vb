Imports system.data
Imports system.data.sqlclient
Imports System.Configuration
Imports System.IO
Imports System.IO.BinaryReader
Imports System.IO.BinaryWriter
Partial Class User_GetPhoto
    Inherits System.Web.UI.Page
    Dim sqlConnString As String
    Public sqlconn As New SqlClient.SqlConnection
    Public myUserCode As Object = Nothing
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            sqlConnString = System.Configuration.ConfigurationManager.AppSettings("dbConnString")
            sqlconn = New SqlClient.SqlConnection(sqlConnString)
            sqlconn.Open()
            If Not sqlconn.State = ConnectionState.Open Then
                ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' No Database Connection Exists ..');", True)
            Else
                Dim Command As SqlCommand
                Dim drUser As SqlDataReader

                Dim myCode As String = Request.QueryString("USER_CODE")
                Command = New SqlCommand("SELECT USER_PHOTO FROM USERS WHERE(USER_CODE='" & Trim(myCode) & "')", sqlconn)
                drUser = Command.ExecuteReader

                If Not drUser.HasRows Then
                    drUser.Close()
                    Exit Sub
                Else
                    drUser.Read()
                    Response.BinaryWrite(drUser("USER_PHOTO"))
                End If
            End If
        Catch s As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' No Database Connection Exists ..');", True)
        End Try
    End Sub
End Class

