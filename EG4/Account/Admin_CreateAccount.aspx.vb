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
Imports System.Net.Mail
Public Class Admin_CreateAccount
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session.Item("IsThisU") <> Nothing Then
                If Not SConnection() = True Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Database Connection is lost..Try Again !');", True)
                Else
                    ADMT1.Visible = True
                    Me.ADMT2.Visible = False
                    CheckAdminUser()
                    Me.txt_UserCode.Focus()
                End If
                Dim CreateSUserButton As Object 'System.Web.UI.WebControls.Button
                CreateSUserButton = Master.FindControl("CreateAdmin_Bttn")
                CreateSUserButton.ForeColor = Drawing.Color.Red
                myPaneName = "DBAdminPane" ' paneSelectedIndex = 0
            Else
                Response.Redirect("~/Default.aspx", False)
            End If
        Catch ex As Exception
            Label6.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    Private Sub Admin_CreateAccount_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Me.txt_UserCode.Focus()
    End Sub
    Public Sub CheckAdminUser()
        Dim Command As SqlCommand
        Dim drUsers As SqlDataReader = Nothing
        Try
            Command = New SqlCommand("SELECT  USER_ID FROM USERS WHERE (USER_TYPE ='ADMIN') AND (STATUS = 'CU') ", SqlConn)
            SqlConn.Open()
            drUsers = Command.ExecuteReader(CommandBehavior.CloseConnection)

            If drUsers.HasRows = True Then
                Alert.Show("Admin User Already Exists !")
                Response.Redirect("~/Default.aspx", False)
            Else
                drUsers.Read()
                drUsers.Close()
                Dim CreateAdminButton As System.Web.UI.WebControls.Button
                CreateAdminButton = Master.FindControl("CreateAdmin_Bttn")
                CreateAdminButton.ForeColor = Drawing.Color.Red
                CreateAdminButton.Enabled = False
                CreateAdminButton.Visible = False
                Me.txt_UserCode.Focus()
            End If
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
        Finally
            Command.Dispose()
            drUsers.Close()
            SqlConn.Close()
        End Try
        
    End Sub
    'save admin account
    Protected Sub Submit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Submit.Click
        If IsPostBack = True Then
            'check unwanted characters
            Dim iloop As Integer
            Dim strcurrentchar As Object
            Dim c As Integer
            Dim counter1, counter2, counter3, counter4, Counter5, counter6, Counter7, counter8, counter9, counter10 As Integer

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
                If flag <> Nothing Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' User Code Already Exists, Please try to enter other User Code... ');", True)
                    Me.txt_UserCode.Focus()
                    Exit Sub
                End If
                SqlConn.Close()
                '********************************************************************************************************************

                'Server Validation for Full Name
                Dim Names As String
                Names = TrimAll(txt_UserName.Text)
                If String.IsNullOrEmpty(Names) Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Enter Full Name... ');", True)
                    Me.txt_UserName.Focus()
                    Exit Sub
                End If
                Names = RemoveQuotes(Names)
                If Names.Length > 150 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Name must be of Proper Length.. ');", True)
                    txt_UserName.Focus()
                    Exit Sub
                End If
                Names = " " & Names & " "
                If InStr(1, Names, "CREATE", 1) > 0 Or InStr(1, Names, "DELETE", 1) > 0 Or InStr(1, Names, "DROP", 1) > 0 Or InStr(1, Names, "INSERT", 1) > 1 Or InStr(1, Names, "TRACK", 1) > 1 Or InStr(1, Names, "TRACE", 1) > 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Reserve Word... ');", True)
                    txt_UserName.Focus()
                    Exit Sub
                End If
                Names = TrimAll(Names)
                c = 0
                counter2 = 0
                For iloop = 1 To Len(Names)
                    strcurrentchar = Mid(Names, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';~!@#$^&*=+|[]{}?,<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter2 = 1
                        End If
                    End If
                Next
                If counter2 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Do Not Use Un-Wated Characters... ');", True)
                    txt_UserName.Focus()
                    Exit Sub
                End If

                '****************************************************************************************
                'Server Validation for Designation
                Dim Designation As String
                Designation = TrimAll(txt_UserDesig.Text)
                If Not String.IsNullOrEmpty(Designation) Then
                    Designation = RemoveQuotes(Designation)
                    If Designation.Length > 250 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not proper... ');", True)
                        Me.txt_UserDesig.Focus()
                        Exit Sub
                    End If
                    Designation = " " & Designation & " "
                    If InStr(1, Designation, "CREATE", 1) > 0 Or InStr(1, Designation, "DELETE", 1) > 0 Or InStr(1, Designation, "DROP", 1) > 0 Or InStr(1, Designation, "INSERT", 1) > 1 Or InStr(1, Designation, "TRACK", 1) > 1 Or InStr(1, Designation, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not proper... ');", True)
                        Me.txt_UserDesig.Focus()
                        Exit Sub
                    End If
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not proper... ');", True)
                    Me.txt_UserDesig.Focus()
                    Exit Sub
                End If
                Designation = TrimAll(Designation)
                'check unwanted characters
                c = 0
                counter3 = 0
                For iloop = 1 To Len(Designation)
                    strcurrentchar = Mid(Designation, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';~!@#$^&*-_=+|[]{}?,<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter3 = 1
                        End If
                    End If
                Next
                If counter3 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not proper... ');", True)
                    Me.txt_UserDesig.Focus()
                    Exit Sub
                End If

                '********************************************************************************************************
                'Server Validation for Phone Number
                Dim Phone As String
                Phone = TrimAll(txt_UserPhone.Text)
                If Not String.IsNullOrEmpty(Phone) Then
                    Phone = RemoveQuotes(Phone)
                    If Phone.Length > 150 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_UserPhone.Focus()
                        Exit Sub
                    End If
                    Phone = " " & Phone & " "
                    If InStr(1, Phone, "CREATE", 1) > 0 Or InStr(1, Phone, "DELETE", 1) > 0 Or InStr(1, Phone, "DROP", 1) > 0 Or InStr(1, Phone, "INSERT", 1) > 1 Or InStr(1, Phone, "TRACK", 1) > 1 Or InStr(1, Phone, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_UserPhone.Focus()
                        Exit Sub
                    End If
                    Dim pattern1 As String = "^\d{5,12}$"
                    Dim phonenoMatch As Match = Regex.Match(Phone, pattern1)
                    If phonenoMatch.Success = False Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Phone Number- Input is not Valid... ');", True)
                        Me.txt_UserPhone.Focus()
                        Exit Sub
                    End If
                Else
                    Phone = String.Empty
                End If
                Phone = TrimAll(Phone)

                'check unwanted characters
                c = 0
                counter4 = 0
                For iloop = 1 To Len(Phone)
                    strcurrentchar = Mid(Phone, iloop, 1)
                    If c = 0 Then
                        If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter4 = 1
                        End If
                    End If
                Next
                If counter4 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                    Me.txt_UserPhone.Focus()
                    Exit Sub
                End If

                '*******************************************************************************************************
                'Server Validation for Mobile Number
                Dim Mobile As String
                Mobile = TrimAll(txt_UserMobile.Text)
                If Not String.IsNullOrEmpty(Mobile) Then
                    Mobile = RemoveQuotes(Mobile)
                    If Phone.Length > 150 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_UserMobile.Focus()
                        Exit Sub
                    End If
                    Mobile = " " & Mobile & " "
                    If InStr(1, Mobile, "CREATE", 1) > 0 Or InStr(1, Mobile, "DELETE", 1) > 0 Or InStr(1, Mobile, "DROP", 1) > 0 Or InStr(1, Mobile, "INSERT", 1) > 1 Or InStr(1, Mobile, "TRACK", 1) > 1 Or InStr(1, Mobile, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_UserMobile.Focus()
                        Exit Sub
                    End If
                    Dim pattern1 As String = "^\d{10,15}$"
                    Dim phonenoMatch As Match = Regex.Match(Mobile, pattern1)
                    If phonenoMatch.Success = False Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Phone Number- Input is not Valid... ');", True)
                        Me.txt_UserMobile.Focus()
                        Exit Sub
                    End If
                Else
                    Mobile = String.Empty
                End If

                Mobile = TrimAll(Mobile)
                'check unwanted characters
                c = 0
                Counter5 = 0
                For iloop = 1 To Len(Mobile)
                    strcurrentchar = Mid(Mobile, iloop, 1)
                    If c = 0 Then
                        If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            Counter5 = 1
                        End If
                    End If
                Next
                If Counter5 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                    Me.txt_UserMobile.Focus()
                    Exit Sub
                End If

                '****************************************************************************************
                'Server Validation for E-Mail Address
                Dim email As String
                email = TrimX(txt_UserEmail.Text)
                If Not String.IsNullOrEmpty(email) Then
                    email = RemoveQuotes(email)
                    If email.Length > 150 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Me.txt_UserEmail.Focus()
                        Exit Sub
                    End If
                    email = " " & email & " "
                    If InStr(1, email, "CREATE", 1) > 0 Or InStr(1, email, "DELETE", 1) > 0 Or InStr(1, email, "DROP", 1) > 0 Or InStr(1, email, "INSERT", 1) > 1 Or InStr(1, email, "TRACK", 1) > 1 Or InStr(1, email, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_UserEmail.Focus()
                        Exit Sub
                    End If
                    Dim pattern As String = "^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"
                    Dim emailAddressMatch As Match = Regex.Match(email, pattern)
                    If emailAddressMatch.Success = False Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' E-Mail- Input is not Valid... ');", True)
                        Me.txt_UserEmail.Focus()
                        Exit Sub
                    End If
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                    Me.txt_UserEmail.Focus()
                    Exit Sub
                End If
                email = TrimX(email)
                'check unwanted characters
                c = 0
                counter6 = 0
                For iloop = 1 To Len(email)
                    strcurrentchar = Mid(email, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';~!#$^&*|[]{}?<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter6 = 1
                        End If
                    End If
                Next
                If counter6 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                    Me.txt_UserEmail.Focus()
                    Exit Sub
                End If


                'Server Validation for security Q ************************************************
                Dim SecQ As String
                SecQ = TrimAll(DropDownList1.Text)
                If SecQ = "-- Select One --" Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Please Select Security Question... ');", True)
                    DropDownList1.Focus()
                    Exit Sub
                End If
                If SecQ <> "What is your pets name?" Or SecQ <> "What is your favorite color?" Or SecQ <> "What was the name of your first school?" Or SecQ <> "Who was your favorite actor?" Or SecQ <> "What is your grand fathers name?" Then
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
                counter9 = 0
                For iloop = 1 To Len(SecQ)
                    strcurrentchar = Mid(SecQ, iloop, 1)
                    If c = 0 Then
                        If Not InStr("'~!@#$^&*+|[]{}<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter9 = 1
                        End If
                    End If
                Next
                If counter9 = 1 Then
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
                counter10 = 0
                For iloop = 1 To Len(SecAns)
                    strcurrentchar = Mid(SecAns, iloop, 1)
                    If c = 0 Then
                        If Not InStr("'~!@#$^&*+|[]{}?<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter10 = 1
                        End If
                    End If
                Next
                If counter10 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Un-Wated Characters... ');", True)
                    txt_UserAns.Focus()
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

                '**************************************************************************************************
                'check duplicate admin account
                'Check Duplicate User Code
                Dim str2 As Object = Nothing
                Dim flag2 As Object = Nothing
                str = "SELECT  USER_ID FROM USERS WHERE (USER_TYPE ='ADMIN') AND (STATUS = 'CU')"
                Dim cmd2 As New SqlCommand(str, SqlConn)
                SqlConn.Open()
                flag2 = cmd2.ExecuteScalar
                If flag2 <> Nothing Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Admin User Already Exists, Please abort... ');", True)
                    Me.txt_UserCode.Focus()
                    Exit Sub
                End If
                SqlConn.Close()

                'INSERT THE RECORD IN TO THE DATABASE
                Dim SQL As String
                Dim Cmd As SqlCommand
                Dim da As SqlDataAdapter
                Dim ds As New DataSet
                Dim CB As SqlCommandBuilder
                Dim dtrow As DataRow
                SQL = "SELECT * FROM USERS WHERE (USER_ID = '0')"
                Cmd = New SqlCommand(SQL, SqlConn)
                da = New SqlDataAdapter(Cmd)
                CB = New SqlCommandBuilder(da)
                SqlConn.Open()
                da.Fill(ds, "users")
                dtrow = ds.Tables("users").NewRow
                If Not String.IsNullOrEmpty(Names) Then
                    dtrow("USER_NAME") = Names.Trim
                End If
                If Not String.IsNullOrEmpty(UserCode) Then
                    dtrow("USER_CODE") = UserCode.Trim
                End If
                If Not String.IsNullOrEmpty(Password) Then
                    dtrow("USER_PASSWORD") = Password.Trim
                End If
                If Not String.IsNullOrEmpty(Designation) Then
                    dtrow("USER_DESIG") = Designation.Trim
                Else
                    dtrow("USER_DESIG") = System.DBNull.Value
                End If
                If Not String.IsNullOrEmpty(Phone) Then
                    dtrow("PHONE") = Phone.Trim
                Else
                    dtrow("PHONE") = System.DBNull.Value
                End If
                If Not String.IsNullOrEmpty(Mobile) Then
                    dtrow("MOBILE") = Mobile.Trim
                Else
                    dtrow("MOBILE") = System.DBNull.Value
                End If
                If Not String.IsNullOrEmpty(email) Then
                    dtrow("USER_EMAIL") = email.Trim
                End If
                If Not String.IsNullOrEmpty(SecQ) Then
                    dtrow("USER_Q") = SecQ.Trim
                End If
                If Not String.IsNullOrEmpty(SecAns) Then
                    dtrow("USER_ANS") = SecAns.Trim
                End If

                dtrow("USER_TYPE") = "ADMIN"
                dtrow("STATUS") = "CU"

                dtrow("DATE_ADDED") = Now.ToShortDateString
                dtrow("USER_IP") = IP.Trim
                ds.Tables("users").Rows.Add(dtrow)
                da.Update(ds, "users")
                ClearFields()
                UpdateUserLOG1()
                ADMT1.Visible = False
                Me.ADMT2.Visible = True
                Label2.Text = UserCode
                Label3.Text = Names
                Label4.Text = Designation
                Label5.Text = Phone
                Label6.Text = Mobile
                Label7.Text = email
                UserCode = Nothing
                Names = Nothing
                Designation = Nothing
                Phone = Nothing
                Mobile = Nothing
                email = Nothing
                ds.Dispose()
                'Alert.Show("Admin Account Created Sucessfully !")
                'Dim url = "../Default.aspx"
                'ClientScript.RegisterStartupScript(Me.GetType(), "callfunction", "alert('Admin User Registered Sucessfully !  ');window.location.href = '" + url + "';", True)
            Catch ex As Exception
                Label6.Text = "Error: " & (ex.Message())
            Finally
                SqlConn.Close()
            End Try
        End If
    End Sub
    Public Sub UpdateUserLOG1()

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
            dtRow("ACTION") = "DB Admin Account Registered Successfully"
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
    Public Sub ClearFields()
        Me.txt_UserCode.Text = ""
        Me.txt_UserDesig.Text = ""
        Me.txt_UserEmail.Text = ""
        Me.txt_UserMobile.Text = ""
        txt_UserName.Text = ""
        txt_UserPass.Text = ""
        txt_UserPhone.Text = ""
        txt_UserRePass.Text = ""
        CAPTCHA1.Text = ""
    End Sub
    'close button
    Protected Sub Close_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Close_Bttn.Click
        Response.Redirect("~/Default.aspx", False)
        'Dim url = "../Default.aspx"
        'ClientScript.RegisterStartupScript(Me.GetType(), "callfunction", "alert('Admin User Registered Sucessfully !  ');window.location.href = '" + url + "';", True)
    End Sub
    Protected Sub Cancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Cancel.Click
        Response.Redirect("~/Default.aspx", False)
    End Sub

    
End Class