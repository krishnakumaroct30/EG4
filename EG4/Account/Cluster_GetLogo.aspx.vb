Imports system.data
Imports system.data.sqlclient
Imports System.Configuration
Imports System.IO
Imports System.IO.BinaryReader
Imports System.IO.BinaryWriter
Partial Class Cluster_GetLogo
    Inherits System.Web.UI.Page
    Dim sqlConnString As String
    Public sqlconn As New SqlClient.SqlConnection
    ' Public myClusterID As Integer = Nothing
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            sqlConnString = System.Configuration.ConfigurationManager.AppSettings("dbConnString")
            sqlconn = New SqlClient.SqlConnection(sqlConnString)
            sqlconn.Open()
            If Not sqlconn.State = ConnectionState.Open Then
                ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' No Database Connection Exists ..');", True)
            Else
                Dim Command As SqlCommand
                Dim drCluster As SqlDataReader

                ' Dim myClusterID As String = Request.QueryString("ID")
                Command = New SqlCommand("SELECT LOGO FROM CLUSTER ", sqlconn)
                drCluster = Command.ExecuteReader

                If Not drCluster.HasRows Then
                    drCluster.Close()
                    Exit Sub
                Else
                    drCluster.Read()
                    Response.BinaryWrite(drCluster("LOGO"))
                End If
            End If
        Catch s As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' No Database Connection Exists ..');", True)
        End Try
    End Sub
End Class

