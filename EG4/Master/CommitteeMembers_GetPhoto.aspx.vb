Imports system.data
Imports system.data.sqlclient
Imports System.Configuration
Imports System.IO
Imports System.IO.BinaryReader
Imports System.IO.BinaryWriter
Partial Class CommitteeMembers_GetPhoto
    Inherits System.Web.UI.Page
    Dim sqlConnString As String
    Public sqlconn As New SqlClient.SqlConnection
    Public myStaffID As Object = Nothing
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

                Dim myStaffID As String = Request.QueryString("TEAM_ID")
                Command = New SqlCommand("SELECT MEMBER_PHOTO FROM COMMITTEE_MEMBERS WHERE(COMMEM_ID='" & Trim(myStaffID) & "')", sqlconn)
                drUser = Command.ExecuteReader

                If Not drUser.HasRows Then
                    drUser.Close()
                    Exit Sub
                Else
                    drUser.Read()
                    Response.BinaryWrite(drUser("MEMBER_PHOTO"))
                End If
            End If
        Catch s As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' No Database Connection Exists ..');", True)
        End Try
    End Sub
End Class

