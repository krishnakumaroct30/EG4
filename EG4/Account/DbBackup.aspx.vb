Imports System.Data
Imports System.Data.SqlClient
Imports AjaxControlToolkit
Imports System.IO
Imports System.Xml
Imports System.Web
Imports System.Web.UI
Imports Microsoft.VisualBasic
Imports System.Configuration
Imports System.Security.Cryptography
Imports EG4.Module1
Imports EG4.CaptchaImage
Imports CDO
Imports System.Net.Mail
Public Class DbBackup
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session.Item("IsThisU") <> Nothing Then
                If Not SConnection() = True Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Database Connection is lost..Try Again !');", True)
                Else
                    If Page.IsPostBack = False Then

                    End If
                End If
                Dim CreateSUserButton As Object 'System.Web.UI.WebControls.Button
                CreateSUserButton = Master.FindControl("Accordion1").FindControl("DBAdminPane").FindControl("Adm_Backup_bttn")
                CreateSUserButton.ForeColor = Drawing.Color.Red
                myPaneName = "DBAdminPane" 'paneSelectedIndex = 0
            Else
                Response.Redirect("~/Default.aspx", False)
            End If
        Catch ex As Exception
            Label6.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    Private Sub ClusterProfile_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete

    End Sub
    Protected Sub bttn_Backup_Click(ByVal sender As Object, ByVal e As EventArgs) Handles bttn_Backup.Click
        Try
            Label1.Text = "Plz wait....."
            Dim myDrive As String = Nothing
            If DropDownList1.Text <> "" Then
                myDrive = DropDownList1.SelectedValue
            Else
                myDrive = "C:\"
            End If
            Dim myDatabase As String = Nothing
            Dim myServerName As String = Nothing
            myDatabase = SqlConn.Database
            myServerName = SqlConn.DataSource

            Dim SQL As String = Nothing
            SQL = "BACKUP DATABASE " & myDatabase & " TO DISK = '" & myDrive & "Library\" & myDatabase & ".Bak'" & _
                " WITH FORMAT, NAME ='Full Backup of " & myDatabase & "'"

            Dim Command As SqlCommand = Nothing
            Command = New SqlCommand(SQL, SqlConn)
            SqlConn.Open()
            Command.CommandTimeout = 120000
            Command.ExecuteNonQuery()
            SqlConn.Close()
            Label1.Text = "Backup done successfully in " & myDrive & "Library folder..and Saved Backup file with the name " & myDatabase & ".Bak"
            ' Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Backup  done successfully !');", True)
            ' ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Backup  done successfully !!');", True)
            'UpdatePanel1.Update()
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
            Label1.Text = s.Message.ToString & " In case LIBRARY folder does not exist in the Drive specified  then first create LIBRARY folder and try again !"
        Finally
            SqlConn.Close()
        End Try
    End Sub

   

End Class