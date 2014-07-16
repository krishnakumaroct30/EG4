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
Imports System.Reflection
Public Class ResetPassword
    Inherits System.Web.UI.Page
    Dim myCUserCode As Object = Nothing
    Public myGenS, myGenT As Object

    Private Sub ResetPassword_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        Try
            If Session.Item("IsThisU") <> Nothing Then
                If Not SConnection() = True Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Database Connection is lost..Try Again !');", True)
                Else
                    If IsPostBack = False Then
                        myGenS = RandomString(12)
                        myGenT = RandomString(10)
                        Session.Item("MyNewSalt") = myGenS
                        valid_link()
                    Else
                        myGenS = Session.Item("MyNewSalt")
                        If Session.Item("UC") = Nothing Then
                            Session.Abandon()
                            Response.Redirect("~/Default.aspx", True)
                            Exit Sub
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Contact Administrator !');", True)
        End Try
    End Sub
    Public Sub valid_link()
        Try
            Dim uid, tid As Object

            If Request.QueryString.Count = 2 Then
                If Request.QueryString.Keys(0) = "uid" Then
                    If Request.QueryString.Keys(1) = "tid" Then
                        uid = Request.QueryString("uid")
                        tid = Request.QueryString("tid")
                        If Trim(uid.ToString) <> String.Empty And Trim(tid.ToString) <> String.Empty Then
                            'Request.QueryString.Remove("uid")
                            'Request.QueryString.Remove("tid")
                            'Request.QueryString.Clear()
                            Dim isreadonly As PropertyInfo = GetType(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance Or BindingFlags.NonPublic)
                            ' make collection editable
                            isreadonly.SetValue(Me.Request.QueryString, False, Nothing)
                            ' remove
                            Me.Request.QueryString.Remove("uid")
                            Me.Request.QueryString.Remove("tid")

                            check_parms(uid, tid)
                        Else
                            Session.Abandon()
                            Response.Redirect("~/Default.aspx", True)
                            Exit Sub
                        End If
                    Else
                        Session.Abandon()
                        Response.Redirect("~/Default.aspx", True)
                        Exit Sub
                    End If
                Else
                    Session.Abandon()
                    Response.Redirect("~/Default.aspx", True)
                    Exit Sub
                End If
            Else
                Session.Abandon()
                Response.Redirect("~/Default.aspx", True)
                Exit Sub
            End If

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Contact Administrator !');", True)
        End Try
    End Sub
    Public Sub check_parms(ByVal uid As Object, ByVal tid As Object)
        Try
            Dim iloop As Integer
            Dim strcurrentchar As Object
            Dim c As Integer
            Dim counter1, counter2, counter3 As Integer
            c = 0
            counter1 = 0
            '****************************************************************************************
            'Server Validation for User Code
            Dim UserCode As String
            UserCode = TrimX(uid)
            If Not String.IsNullOrEmpty(UserCode) Then
                UserCode = RemoveQuotes(UserCode)
                If UserCode.Length > 20 Then
                    Session.Abandon()
                    Response.Redirect("~/Default.aspx", True)
                    Exit Sub
                End If
                If InStr(1, UserCode, "CREATE", 1) > 0 Or InStr(1, UserCode, "DELETE", 1) > 0 Or InStr(1, UserCode, "DROP", 1) > 0 Or InStr(1, UserCode, "INSERT", 1) > 1 Or InStr(1, UserCode, "TRACK", 1) > 1 Or InStr(1, UserCode, "TRACE", 1) > 1 Then
                    Session.Abandon()
                    Response.Redirect("~/Default.aspx", True)
                    Exit Sub
                End If
            Else
                Session.Abandon()
                Response.Redirect("~/Default.aspx", True)
                Exit Sub
            End If
            'check unwanted characters
            c = 0
            counter2 = 0
            For iloop = 1 To Len(UserCode)
                strcurrentchar = Mid(UserCode, iloop, 1)
                If c = 0 Then
                    If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                        c = c + 1
                        counter2 = 1
                    End If
                End If
            Next
            If counter2 = 1 Then
                Session.Abandon()
                Response.Redirect("~/Default.aspx", True)
                Exit Sub
            End If

            '****************************************************************************************
            Dim dr As SqlDataReader
            Dim Cmd As SqlCommand
            Dim SQL As String

            SQL = "SELECT PASS_TOKEN, PASS_TOKEN_ACTIVE, PASS_TOKEN_DATE FROM USERS WHERE (USER_CODE='" & Trim(UserCode) & "')"
            Cmd = New SqlCommand(SQL, SqlConn)
            dr = Cmd.ExecuteReader
            If dr.HasRows Then
                dr.Read()
                Dim mytkid As String
                Dim mytkac As String
                Dim mytkdt As DateTime
                mytkid = dr.Item("PASS_TOKEN").ToString
                mytkac = dr.Item("PASS_TOKEN_ACTIVE").ToString
                mytkdt = dr.Item("PASS_TOKEN_DATE")
                dr.Close()
                If Trim(mytkac) = "Y" Then
                    Session.Abandon()
                    Response.Redirect("~/Default.aspx", True)
                    Exit Sub
                End If
                Dim vdt, pdt As DateTime
                vdt = mytkdt.AddHours(+6)
                pdt = Now
                If pdt > vdt Then
                    Session.Abandon()
                    Response.Redirect("~/Default.aspx", True)
                    Exit Sub
                End If

                If (mytkid = tid) Then
                    Dim str As Object = Nothing
                    str = "UPDATE USERS SET PASS_TOKEN_ACTIVE='Y' WHERE (USER_CODE ='" & Trim(UserCode) & "')"
                    Dim cmd1 As New SqlCommand(str, SqlConn)
                    cmd1.ExecuteNonQuery()
                    Session("UC") = Trim(UserCode)
                    'Response.Redirect("Forgot_Change_Password.aspx")
                    Update_OPAC_LOG1(UserCode, 1)
                Else
                    Session.Abandon()
                    Response.Redirect("~/Default.aspx", True)
                    Exit Sub
                End If
            Else
                Session.Abandon()
                Response.Redirect("~/Default.aspx", True)
                Exit Sub
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Contact Administrator !');", True)
        End Try
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session.Item("IsThisU") <> Nothing Then
                If Not SConnection() = True Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Database Connection is lost..Try Again !');", True)
                Else
                    myCUserCode = Session.Item("UC")
                    txt_UserCode.Enabled = False
                    txt_UserCode.Text = myCUserCode
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
                Response.Redirect("~/Default.aspx", False)
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
                UserCode = Session.Item("UC")
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

                'CHECK VALID USER TO UPDATE PASSWORD
                '*******************************************************************************************************
                Dim Hashed As Object
                Hashed = HashPasschk.Value
                HashPasschk.Value = ""
                If Hashed <> "" Then
                    Hashed = RemoveQuotes(Hashed)
                    If Hashed.ToString.Length > 100 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Sorry... Improper Submit...Try Again!!');", True)
                        Session.Abandon()
                        Response.Redirect("..\Default.aspx")
                        Response.End()
                    End If
                    If (InStr(1, Hashed, "CREATE", 1) > 0) Or (InStr(1, Hashed, "INSERT", 1) > 0) Or (InStr(1, Hashed, "DROP", 1) > 0) Or (InStr(1, Hashed, "TRACK", 1) Or (InStr(1, Hashed, "TRACE", 1) > 0)) Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Sorry... Improper Submit...Try Again!!');", True)
                        Session.Abandon()
                        Response.Redirect("..\Default.aspx")
                        Response.End()
                    End If
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Sorry... Improper Submit...Try Again!!');", True)
                    Session.Abandon()
                    Response.Redirect("..\Default.aspx")
                    Response.End()
                End If
                'check unwanted characters
                c = 0
                counter1 = 0
                For iloop = 1 To Len(Hashed)
                    strcurrentchar = Mid(Hashed, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter1 = 1
                        End If
                    End If
                Next
                If counter1 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Sorry... Improper Submit...Try Again!!');", True)
                    Session.Abandon()
                    Response.Redirect("..\Default.aspx")
                    Response.End()
                End If

                Dim myagainHash As Object = Nothing
                myagainHash = Session.Item("MyNewSalt")
                Dim obj1 As New HashClass1
                myagainHash = obj1.getMD5Hash(myagainHash)

                If Not Hashed = myagainHash Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Sorry... Improper Submit...Try Again!!');", True)
                    Session.Abandon()
                    Response.Redirect("..\Default.aspx")
                    Response.End()
                End If



                'SERVER VALIDATION FOR PASSWORD
                '*******************************************************************************************************
                Dim Password As Object
                Dim NewHashed As Object
                NewHashed = HashPass2.Value 'Request.Form("HashPass")
                Password = TrimX(NewHashed)
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

                SQL1 = "SELECT *FROM USERS WHERE (USER_CODE='" & Trim(UserCode) & "') AND (STATUS = 'CU') "
                da = New SqlDataAdapter(SQL1, SqlConn)
                cb = New SqlCommandBuilder(da)
                SqlConn.Open()
                da.Fill(ds, "USERS")
                If ds.Tables("USERS").Rows.Count <> 0 Then
                    ds.Tables("USERS").Rows(0)("USER_PASSWORD") = Password.Trim
                    ds.Tables("USERS").Rows(0)("DATE_MODIFIED") = Now.ToShortDateString
                    ds.Tables("USERS").Rows(0)("USER_IP") = IP.Trim
                    da.Update(ds, "USERS")

                    Update_OPAC_LOG1(UserCode, 0)

                    'ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Your Password Changed Successfully... ');", True)
                    ADMT2.Visible = True
                    ADMT1.Visible = False
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

    Public Sub Update_OPAC_LOG1(ByVal UserCode As String, ByVal tk As Integer)

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
            SqlConn.Open()
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

            If tk = 0 Then
                dtRow("ACTION") = "Reset Password Successfully"
            ElseIf tk = 1 Then
                dtRow("ACTION") = "Reset Password activation link Activated"
            End If

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

    'close button
    'Protected Sub Close_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Close_Bttn.Click
    '    Response.Redirect("~/Default.aspx", False)
    '    'Dim url = "../Default.aspx"
    '    'ClientScript.RegisterStartupScript(Me.GetType(), "callfunction", "alert('Admin User Registered Sucessfully !  ');window.location.href = '" + url + "';", True)
    'End Sub
    'Protected Sub Cancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Cancel.Click
    '    Response.Redirect("~/Default.aspx", False)
    'End Sub
    'Private Sub ResetPassword_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
    '    Me.txt_UserPass.Focus()
    'End Sub
End Class