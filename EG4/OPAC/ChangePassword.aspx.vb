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
Public Class ChangePassword
    Inherits System.Web.UI.Page
    Dim myCUserCode As Object = Nothing
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session.Item("IsThisU") <> Nothing Then
                If Not SConnection() = True Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Database Connection is lost..Try Again !');", True)
                Else
                    myCUserCode = TrimX(txt_UserCode.Text) 'Request.QueryString("USER_CODE")
                    txt_UserCode.Enabled = False
                    If myCUserCode <> "" Then
                        txt_UserCode.Text = myCUserCode
                    Else
                        txt_UserCode.Text = Session.Item("OldMemNo")
                        Session.Item("OldMemNo") = Nothing
                    End If
                    ADMT2.Visible = False

                    Dim myTR As System.Web.UI.HtmlControls.HtmlTableCell
                    myTR = Master.FindControl("Tdx")
                    myTR.Visible = False

                    'hide login button
                    Dim mybttn_LogIn As System.Web.UI.WebControls.Button
                    mybttn_LogIn = Master.FindControl("bttn_LogIn")
                    mybttn_LogIn.Visible = False

                    txt_UserPass.Focus()
                End If
            Else
                Response.Redirect("~/OPAC/Default.aspx", False)
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Contact Administrator !');", True)
        End Try
    End Sub
    'Reset Password
    Protected Sub Submit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Submit.Click
        If IsPostBack = True Then
            'check unwanted characters
            Dim iloop As Integer
            Dim strcurrentchar As Object
            Dim c As Integer
            Dim counter1, Counter7, counter8 As Integer

            Try
                'Server Validation for User Code
                Dim UserCode As String
                UserCode = TrimX(myCUserCode)
                UserCode = UCase(UserCode)
                If Not String.IsNullOrEmpty(UserCode) Then
                    UserCode = RemoveQuotes(UserCode)
                    If UserCode.Length > 12 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Length of User Code is not Proper... ');", True)
                        Me.txt_UserCode.Focus()
                        Exit Sub
                    End If
                    UserCode = " " & UserCode & " "
                    If InStr(1, UserCode, " CREATE ", 1) > 0 Or InStr(1, UserCode, " DELETE ", 1) > 0 Or InStr(1, UserCode, " DROP ", 1) > 0 Or InStr(1, UserCode, " INSERT ", 1) > 1 Or InStr(1, UserCode, " TRACK ", 1) > 1 Or InStr(1, UserCode, " TRACE ", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do not use reserve words... ');", True)
                        Me.txt_UserCode.Focus()
                        Exit Sub
                    End If
                    UserCode = TrimX(UserCode)
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Enter User Code... ');", True)
                    Me.txt_UserCode.Focus()
                    Exit Sub
                End If
                'check unwanted characters
                c = 0
                counter1 = 0
                For iloop = 1 To Len(UserCode)
                    strcurrentchar = Mid(UserCode, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';~!@#$^&*-_=+|[]{}?,<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter1 = 1
                        End If
                    End If
                Next
                If counter1 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Un-wanted Characters... ');", True)
                    Me.txt_UserCode.Focus()
                    Exit Sub
                End If

                'SERVER VALIDATION FOR PASSWORD
                '*******************************************************************************************************
                Dim Password As Object
                Dim Hashed As Object
                Hashed = HashPass2.Value 'Request.Form("HashPass")
                Password = TrimX(Hashed)
                If Not String.IsNullOrEmpty(Password) Then
                    Password = RemoveQuotes(Password)
                    If Password.Length > 72 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Length of password is not Proper... ');", True)
                        Me.txt_UserPass.Focus()
                        Exit Sub
                    End If
                    If InStr(1, Password, "CREATE", 1) > 0 Or InStr(1, Password, "DELETE", 1) > 0 Or InStr(1, Password, "DROP", 1) > 0 Or InStr(1, Password, "INSERT", 1) > 1 Or InStr(1, Password, "TRACK", 1) > 1 Or InStr(1, Password, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Do not use reserve words... ');", True)
                        Me.txt_UserPass.Focus()
                        Exit Sub
                    End If
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Enter Password... ');", True)
                    Me.txt_UserPass.Focus()
                    Exit Sub
                End If
                'check unwanted characters
                c = 0
                Counter7 = 0
                For iloop = 1 To Len(Password)
                    strcurrentchar = Mid(Password, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';""", strcurrentchar) <= 0 Then
                            c = c + 1
                            Counter7 = 1
                        End If
                    End If
                Next
                If Counter7 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Do Not Use Un-wanted Characters... ');", True)
                    Me.txt_UserPass.Focus()
                    Exit Sub
                End If

                '****************************************************************************************
                'Server Validation for IP ADDRESS
                Dim IP As String
                IP = Request.UserHostAddress
                If Not String.IsNullOrEmpty(IP) Then
                    IP = RemoveQuotes(IP)
                    If IP.Length > 50 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' IP Address is not Valid... ');", True)
                        Exit Sub
                    End If
                    If InStr(1, IP, "CREATE", 1) > 0 Or InStr(1, IP, "DELETE", 1) > 0 Or InStr(1, IP, "DROP", 1) > 0 Or InStr(1, IP, "INSERT", 1) > 1 Or InStr(1, IP, "TRACK", 1) > 1 Or InStr(1, IP, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' IP Address is not Valid... ');", True)
                        Exit Sub
                    End If
                Else
                    IP = String.Empty
                End If

                'check unwanted characters
                c = 0
                counter8 = 0
                For iloop = 1 To Len(IP)
                    strcurrentchar = Mid(IP, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter8 = 1
                        End If
                    End If
                Next
                If counter8 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' IP Address  is not Valid... ');", True)
                    Exit Sub
                End If

                'UPDATE THE USER PROFILE    
                Dim SQL1 As String
                Dim da As SqlDataAdapter
                Dim cb As SqlCommandBuilder
                Dim ds As New DataSet
                Dim dt As New DataTable

                SQL1 = "SELECT * FROM MEMBERSHIPS WHERE (MEM_NO='" & Trim(UserCode) & "') AND (LIB_CODE = '" & Trim(Session.Item("OldLibcode")) & "') AND (MEM_STATUS = 'CU') "
                da = New SqlDataAdapter(SQL1, SqlConn)
                cb = New SqlCommandBuilder(da)
                SqlConn.Open()
                da.Fill(ds, "USERS")
                If ds.Tables("USERS").Rows.Count <> 0 Then
                    ds.Tables("USERS").Rows(0)("MEMB_PASSWORD") = Password.Trim
                    ds.Tables("USERS").Rows(0)("DATE_MODIFIED") = Now.ToShortDateString
                    ds.Tables("USERS").Rows(0)("IP") = IP.Trim
                    da.Update(ds, "USERS")
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert(' Your Password Changed Successfully... ');", True)
                    ADMT2.Visible = True
                    ADMT1.Visible = False
                    Session.Item("OldLibcode") = Nothing
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Contact System Administrator... ');", True)
                    Exit Sub
                End If
                da.Dispose()
            Catch ex As Exception
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Submit Correct Data !');", True)
            Finally
                SqlConn.Close()
            End Try
        End If
    End Sub
    'close button
    Protected Sub Close_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Close_Bttn.Click
        Response.Redirect("~/OPAC/Default.aspx", False)
        'Dim url = "../Default.aspx"
        'ClientScript.RegisterStartupScript(Me.GetType(), "callfunction", "alert('Admin User Registered Sucessfully !  ');window.location.href = '" + url + "';", True)
    End Sub
    Protected Sub Cancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Cancel.Click
        Response.Redirect("~/OPAC/Default.aspx", False)
    End Sub
    Private Sub ResetPassword_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Me.txt_UserPass.Focus()
    End Sub
End Class