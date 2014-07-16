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
Imports CDO
Imports System.Net.Mail
Public Class ForgotPassword
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session.Item("IsThisU") <> Nothing Then
                If Not SConnection() = True Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Database Connection is lost..Try Again !');", True)
                Else
                    'hide forget passoword link
                    Dim myTR As System.Web.UI.HtmlControls.HtmlTableCell
                    myTR = Master.FindControl("Tdx")
                    myTR.Visible = False

                    'hide login button
                    Dim mybttn_LogIn As System.Web.UI.WebControls.Button
                    mybttn_LogIn = Master.FindControl("bttn_LogIn")
                    mybttn_LogIn.Visible = False

                    txt_UserCode.Focus()
                End If
            Else
                Response.Redirect("~/Default.aspx", False)
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Contact Administrator !');", True)
        End Try
    End Sub
    'save admin account
    Protected Sub Submit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Submit.Click
        If IsPostBack = True Then
            'check unwanted characters
            Dim iloop As Integer
            Dim strcurrentchar As Object
            Dim c As Integer
            Dim counter1, counter2, counter3 As Integer

            Try
                'Server Validation for captcha code
                Dim loginCAPTCHA As WebControlCaptcha.CaptchaControl = Me.CAPTCHA1
                'Dim loginCAPTCHA As WebControlCaptcha.CaptchaControl = CType(Me.FindControl("CAPTCHA"), WebControlCaptcha.CaptchaControl)

                If loginCAPTCHA.UserValidated = False Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Plz Enter Security code... ');", True)
                    CAPTCHA1.Focus()
                    Exit Sub
                End If
                'Server Validation for User Code
                Dim UserCode As String
                UserCode = TrimX(txt_UserCode.Text)
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

                'Check Duplicate User Code
                Dim str As Object = Nothing
                Dim flag As Object = Nothing
                str = "SELECT USER_ID FROM USERS WHERE (USER_CODE ='" & Trim(UserCode) & "')"
                Dim cmd1 As New SqlCommand(str, SqlConn)
                SqlConn.Open()
                flag = cmd1.ExecuteScalar
                If flag = Nothing Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Invalid UserCode/Question/Answer... ');", True)
                    Me.txt_UserCode.Focus()
                    Exit Sub
                End If
                SqlConn.Close()

                'Server Validation for security Q ************************************************
                Dim SecQ As String
                SecQ = TrimAll(DropDownList1.Text)
                If SecQ = "-- Select One --" Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Please Select Security Question... ');", True)
                    DropDownList1.Focus()
                    Exit Sub
                End If
                If SecQ <> "What is your pets name?" And SecQ <> "What is your favorite color?" And SecQ <> "What was the name of your first school?" And SecQ <> "Who was your favorite actor?" And SecQ <> "What is your grand fathers name?" Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Please Select Security Question... ');", True)
                    DropDownList1.Focus()
                    Exit Sub
                End If
                If String.IsNullOrEmpty(SecQ) Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Select Security Question... ');", True)
                    Me.DropDownList1.Focus()
                    Exit Sub
                End If
                SecQ = RemoveQuotes(SecQ)
                If SecQ.Length > 200 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Data must be of Proper Length... ');", True)
                    DropDownList1.Focus()
                    Exit Sub
                End If
                SecQ = " " & SecQ & " "
                If InStr(1, SecQ, "CREATE", 1) > 0 Or InStr(1, SecQ, "DELETE", 1) > 0 Or InStr(1, SecQ, "DROP", 1) > 0 Or InStr(1, SecQ, "INSERT", 1) > 1 Or InStr(1, SecQ, "TRACK", 1) > 1 Or InStr(1, SecQ, "TRACE", 1) > 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Do Not Use Reserve Word... ');", True)
                    DropDownList1.Focus()
                    Exit Sub
                End If
                SecQ = TrimAll(SecQ)
                c = 0
                counter2 = 0
                For iloop = 1 To Len(SecQ)
                    strcurrentchar = Mid(SecQ, iloop, 1)
                    If c = 0 Then
                        If Not InStr("'~!@#$^&*+|[]{}<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter2 = 1
                        End If
                    End If
                Next
                If counter2 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Un-Wated Characters... ');", True)
                    DropDownList1.Focus()
                    Exit Sub
                End If

                'Server Validation for  security Answer ************************************************
                Dim SecAns As String
                SecAns = TrimAll(Me.txt_UserAns.Text)
                If String.IsNullOrEmpty(SecQ) Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Enter your Security Answer... ');", True)
                    Me.txt_UserAns.Focus()
                    Exit Sub
                End If
                SecAns = RemoveQuotes(SecAns)
                If SecAns.Length > 150 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Data must be of proper length.. ');", True)
                    txt_UserAns.Focus()
                    Exit Sub
                End If
                SecAns = " " & SecAns & " "
                If InStr(1, SecAns, "CREATE", 1) > 0 Or InStr(1, SecAns, "DELETE", 1) > 0 Or InStr(1, SecAns, "DROP", 1) > 0 Or InStr(1, SecAns, "INSERT", 1) > 1 Or InStr(1, SecAns, "TRACK", 1) > 1 Or InStr(1, SecAns, "TRACE", 1) > 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Reserve Word... ');", True)
                    txt_UserAns.Focus()
                    Exit Sub
                End If
                SecAns = TrimAll(SecAns)
                c = 0
                counter3 = 0
                For iloop = 1 To Len(SecAns)
                    strcurrentchar = Mid(SecAns, iloop, 1)
                    If c = 0 Then
                        If Not InStr("'~!@#$^&*+|[]{}?<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter3 = 1
                        End If
                    End If
                Next
                If counter3 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Un-Wated Characters... ');", True)
                    txt_UserAns.Focus()
                    Exit Sub
                End If

                '**************************************************************************************************
                'Check User Code
                'Dim str As Object = Nothing
                'Dim flag2 As Object = Nothing
                'str = "SELECT  USER_ID FROM USERS WHERE (USER_CODE ='" & Trim(UserCode) & " ') and (USER_ANS ='" & Trim(SecAns) & "')  AND (STATUS = 'CU')"
                'Dim cmd2 As New SqlCommand(str, SqlConn)
                'SqlConn.Open()
                'flag2 = cmd2.ExecuteScalar
                'If flag2 = Nothing Then
                '    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' User does not Exist, Please abort... ');", True)
                '    Me.txt_UserCode.Focus()
                '    Exit Sub
                'Else
                '    Response.Redirect("ResetPassword.aspx?USER_CODE=" & UserCode, False)
                'End If

                Dim dr As SqlDataReader
                Dim Cmd As SqlCommand
                Dim SQL As String

                SQL = "SELECT USER_EMAIL, USER_Q, USER_ANS FROM USERS WHERE (USER_CODE='" & Trim(UserCode) & "')"
                Cmd = New SqlCommand(SQL, SqlConn)
                SqlConn.Open()
                dr = Cmd.ExecuteReader
                If dr.HasRows Then
                    dr.Read()
                    Dim myscrtq As String
                    Dim myanswr As String
                    Dim myemail As String
                    myscrtq = dr.Item("USER_Q").ToString
                    myanswr = dr.Item("USER_ANS").ToString
                    myemail = dr.Item("USER_EMAIL").ToString
                    dr.Close()
                    If (myscrtq = SecQ And myanswr = SecAns) Then
                        Dim randcode As Object
                        randcode = RandomString(12)
                        updatepassword(randcode, UserCode, myemail)
                        'mailpwd(UserCode, randpwd, myemail)
                        'Session("VALID") = True                   
                        'Session("UC") = Trim(UserCode)

                    Else
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Invalid UserCode/Question/Answer... ');", True)
                        Me.DropDownList1.Focus()
                        Exit Sub
                    End If
                Else
                    Response.Write("<script>alert('Invalid UserCode/Question/Answer..');<" & Chr(47) & "script>")
                End If

            Catch ex As Exception
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Submit Correct Data !');", True)
            Finally
                SqlConn.Close()
            End Try
        End If
    End Sub
    'update password to database
    Public Sub updatepassword(ByVal randpwd As Object, ByVal usercode As Object, ByVal email As Object)
        Try
            Dim obj As New HashClass1
            Dim myMem_Hash_token_id As Object = Nothing
            If (randpwd.ToString <> String.Empty) Then
                Dim str As Object = Nothing
                str = randpwd + usercode + email
                myMem_Hash_token_id = obj.getMD5Hash(str) ' to MD5 the pwd
                str = "UPDATE USERS SET PASS_TOKEN='" & Trim(myMem_Hash_token_id) & "',PASS_TOKEN_ACTIVE='N', PASS_TOKEN_DATE='" & Now & "' WHERE (USER_CODE ='" & Trim(usercode) & "')"
                Dim cmd1 As New SqlCommand(str, SqlConn)
                cmd1.ExecuteNonQuery()
                Update_OPAC_LOG1(usercode)
                'Mail password to end user
                'Response.Redirect("forgot_change_password.aspx?uid=" + usercode + "&tid=" + myMem_Hash_token_id)
                mailpwd(usercode, myMem_Hash_token_id, email)
            Else
                Response.Write("<script>alert('Sorry....User_Code or Password Is Not Match');<" & Chr(47) & "script>")
                Response.Redirect("~")
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Contact Administrator !');", True)
        End Try
    End Sub
    Public Sub mailpwd(ByVal usercode As Object, ByVal token_id As Object, ByVal email As Object)
        Try
            Dim msg As New CDO.Message()
            Dim iConfg As CDO.IConfiguration
            iConfg = msg.Configuration
            msg.From = "eg4@nic.in"
            msg.[To] = email
            msg.Subject = "eGranthalaya 4.0 Web Edition- Request for Password Reset"
            Dim link_add As Object
            'link_add = Server.MapPath(".") + "/Forgot_Change_Password.aspx?uid=" + usercode + "&tid=" + token_id
            link_add = "http://demotemp306.nic.in/Account/ResetPassword.aspx?uid=" + usercode + "&tid=" + token_id
            link_add = Replace(link_add, "\", "/")
            Dim txtMsg As String = "<html><body>Dear User:" + usercode + ",<br/> Click on this link to reset your password: " & link_add & "</b><br/>" & "If you didn't request for a password  reset. Please contact NIC HQ Delhi eGranthalaya Team <br/><br/><br/>" & " Thanks and Regards,<br/> NIC eG Team<br/> </body></html>"
            msg.HTMLBody = txtMsg
            msg.Send()
           
            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Reset Password link sent to your registered e-mail');", True)
            'updatepassword(randpwd, usercode, email)
            Response.Redirect("~/Default.aspx")
            'Response.Redirect("Msg.htm", True)
        Catch ex As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Error in Password Reset...Contact System Administrator');", True)
        End Try
    End Sub
    Public Sub Update_OPAC_LOG1(ByVal UserCode As String)

        'UPDATE THE MEMBER PROFILE    
        Dim SQLX As String
        Dim CmdX As SqlCommand
        Dim daX As SqlDataAdapter = Nothing
        Dim dsX As New DataSet
        Dim CB As SqlCommandBuilder
        Dim dtRow As DataRow
        Try
            SQLX = "SELECT * FROM USER_LOGS WHERE (USER_LOG_ID = '00')"
            CmdX = New SqlCommand(SQLX, SqlConn)
            If SqlConn.State = ConnectionState.Closed Then
                SqlConn.Open()
            End If

            daX = New SqlDataAdapter(CmdX)
            CB = New SqlCommandBuilder(daX)
            daX.Fill(dsX, "LOGS")
            dtRow = dsX.Tables("LOGS").NewRow

            If Not String.IsNullOrEmpty(UserCode) Then
                dtRow("USER_CODE") = UserCode.Trim
            End If
            Dim myIP As String = Nothing
            myIP = Trim(Request.UserHostAddress)
            If Not String.IsNullOrEmpty(myIP) Then
                dtRow("IP") = myIP.Trim
            Else
                dtRow("IP") = System.DBNull.Value
            End If

            Dim myDate As Date
            myDate = Format(Today, "MM/dd/yyyy")
            If Not String.IsNullOrEmpty(myDate) Then
                dtRow("LOGIN_DATE") = myDate
            Else
                dtRow("LOGIN_DATE") = System.DBNull.Value
            End If

            Dim myTime As String
            myTime = Now.ToLongTimeString
            If Not String.IsNullOrEmpty(myTime) Then
                dtRow("LOGIN_TIME") = myTime
            Else
                dtRow("LOGIN_TIME") = System.DBNull.Value
            End If
            Dim myPageVisited As String = Nothing
            myPageVisited = Request.ServerVariables("SCRIPT_NAME").ToString()
            If Not String.IsNullOrEmpty(myPageVisited) Then
                dtRow("PAGE_VISITED") = myPageVisited.Trim
            Else
                dtRow("PAGE_VISITED") = System.DBNull.Value
            End If
            dtRow("SUCCESS") = "Y"
            dtRow("ACTION") = "Reset Password token id generated"
            dtRow("UI_TYPE") = "DataEntry"

            dsX.Tables("LOGS").Rows.Add(dtRow)
            daX.Update(dsX, "LOGS")
            daX.Dispose()
            dsX.Dispose()
        Catch EX As Exception
            Response.Write(EX.Message)
        Finally
            daX.Dispose()
            dsX.Dispose()
            SqlConn.Close()
        End Try
    End Sub
    
    Protected Sub Cancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Cancel.Click
        Response.Redirect("~/Default.aspx", False)
    End Sub
    Private Sub ForgotPassword_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Me.txt_UserCode.Focus()
    End Sub
End Class