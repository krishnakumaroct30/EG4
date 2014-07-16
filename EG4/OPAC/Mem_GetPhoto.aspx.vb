Imports system.data
Imports system.data.sqlclient
Imports System.Configuration
Imports System.IO
Imports System.IO.BinaryReader
Imports System.IO.BinaryWriter
Partial Class Mem_GetPhoto
    Inherits System.Web.UI.Page
    Dim sqlConnString As String
    Public sqlconn As New SqlClient.SqlConnection
    Public MEM_ID As Integer = Nothing
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            sqlConnString = System.Configuration.ConfigurationManager.AppSettings("dbConnString")
            sqlconn = New SqlClient.SqlConnection(sqlConnString)
            sqlconn.Open()
            If Not sqlconn.State = ConnectionState.Open Then
                ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' No Database Connection Exists ..');", True)
            Else
                Dim Command As SqlCommand
                Dim drMember As SqlDataReader

                Dim MEM_ID As String = Request.QueryString("MEM_ID")
                Command = New SqlCommand("SELECT PHOTO FROM MEMBERSHIPS WHERE (MEM_ID='" & Trim(MEM_ID) & "')", sqlconn)
                drMember = Command.ExecuteReader

                If Not drMember.HasRows Then
                    drMember.Close()
                    Exit Sub
                Else
                    drMember.Read()
                    Response.BinaryWrite(drMember("PHOTO"))
                End If
            End If
        Catch s As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' No Database Connection Exists ..');", True)
        End Try
    End Sub
End Class

