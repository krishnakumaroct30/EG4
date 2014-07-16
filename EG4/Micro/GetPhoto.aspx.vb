Imports system.data
Imports system.data.sqlclient
Imports System.Configuration
Imports System.IO
Imports System.IO.BinaryReader
Imports System.IO.BinaryWriter
Partial Class GetPhoto
    Inherits System.Web.UI.Page
    Dim sqlConnString As String
    Public sqlconn As New SqlClient.SqlConnection
    'Public myCatNo As Object = Nothing
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            sqlConnString = System.Configuration.ConfigurationManager.AppSettings("dbConnString")
            sqlconn = New SqlClient.SqlConnection(sqlConnString)
            sqlconn.Open()
            If Not sqlconn.State = ConnectionState.Open Then
                Exit Sub
            Else
                Dim Command As SqlCommand
                Dim drCat As SqlDataReader

                Dim myArt As String = Request.QueryString("ART_NO")
                Command = New SqlCommand("SELECT PHOTO_FILE FROM ARTICLES WHERE(ART_NO='" & Trim(myArt) & "')", sqlconn)
                drCat = Command.ExecuteReader

                If Not drCat.HasRows Then
                    drCat.Close()
                    Exit Sub
                Else
                    drCat.Read()
                    Response.BinaryWrite(drCat("PHOTO_FILe"))
                End If
            End If
        Catch s As Exception
            Exit Sub
        End Try
    End Sub
End Class

