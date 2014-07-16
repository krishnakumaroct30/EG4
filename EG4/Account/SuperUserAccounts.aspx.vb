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
Public Class SuperUserAccounts
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session.Item("IsThisU") <> Nothing Then
                If Not SConnection() = True Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Database Connection is lost..Try Again !');", True)
                Else
                    If Page.IsPostBack = False Then
                        AllLibraries()
                        Me.bttn_Save.Visible = True
                        Me.bttn_Update.Visible = False
                        tr_status.Visible = False
                        Me.CheckBox1.Checked = True
                        Me.CheckBox1.Enabled = False
                    End If
                    Me.txt_UserCode.Focus()
                End If
                Dim CreateSUserButton As Object 'System.Web.UI.WebControls.Button
                CreateSUserButton = Master.FindControl("Accordion1").FindControl("DBAdminPane").FindControl("Adm_CreateSUserAccount_bttn")
                CreateSUserButton.ForeColor = Drawing.Color.Red
                myPaneName = "DBAdminPane"  ' paneSelectedIndex = 0
            Else
                Response.Redirect("~/Default.aspx", False)
            End If
        Catch ex As Exception
            Label6.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    Private Sub LibAccounts_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        If Me.txt_UserCode.Enabled = False Then
            Me.txt_UserName.Focus()
        Else
            Me.txt_UserCode.Focus()
        End If
    End Sub
    Public Sub AllLibraries()
        Dim Command As SqlCommand = Nothing
        Dim dt As DataTable = Nothing
        Try
            Command = New SqlCommand("SELECT  LIB_CODE, LIB_NAME FROM LIBRARIES WHERE (STATUS='CU') ", SqlConn)
            SqlConn.Open()
            Dim da As New SqlDataAdapter(Command)
            Dim ds As New DataSet
            Dim RecordCount As Long = 0
            da.Fill(ds)

            dt = ds.Tables(0).Copy

            Dim Dr As DataRow
            Dr = dt.NewRow
            Dr("LIB_CODE") = ""
            Dr("LIB_NAME") = ""
            dt.Rows.InsertAt(Dr, 0)

            If dt.Rows.Count = 0 Then
                Me.Drop_Libraries.DataSource = Nothing
            Else
                Me.Drop_Libraries.DataSource = dt
                Me.Drop_Libraries.DataTextField = "LIB_NAME"
                Me.Drop_Libraries.DataValueField = "LIB_CODE"
                Me.Drop_Libraries.DataBind()
            End If
            dt.Dispose()
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
        Finally
            Command.Dispose()
            dt.Dispose()
            SqlConn.Close()
        End Try
    End Sub
    'save user account
    Protected Sub Save_Click(ByVal sender As Object, ByVal e As EventArgs) Handles bttn_Save.Click
        If IsPostBack = True Then
            'check unwanted characters
            Dim iloop As Integer
            Dim strcurrentchar As Object
            Dim c As Integer
            Dim counter1, counter2, counter3, counter4, Counter5, counter6, Counter7, counter8, counter9, counter10 As Integer
            Try
                'Server Validation for User Code
                Dim newUserCode As String = Nothing
                newUserCode = TrimX(txt_UserCode.Text)
                newUserCode = UCase(newUserCode)
                If Not String.IsNullOrEmpty(newUserCode) Then
                    UserCode = RemoveQuotes(newUserCode)
                    If newUserCode.Length > 12 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Length of User Code is not Proper... ');", True)
                        Me.txt_UserCode.Focus()
                        Exit Sub
                    End If
                    newUserCode = " " & newUserCode & " "
                    If InStr(1, newUserCode, " CREATE ", 1) > 0 Or InStr(1, newUserCode, " DELETE ", 1) > 0 Or InStr(1, newUserCode, " DROP ", 1) > 0 Or InStr(1, newUserCode, " INSERT ", 1) > 1 Or InStr(1, newUserCode, " TRACK ", 1) > 1 Or InStr(1, newUserCode, " TRACE ", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do not use reserve words... ');", True)
                        Me.txt_UserCode.Focus()
                        Exit Sub
                    End If
                    newUserCode = TrimX(newUserCode)
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Enter User Code... ');", True)
                    Me.txt_UserCode.Focus()
                    Exit Sub
                End If
                'check unwanted characters
                c = 0
                counter1 = 0
                For iloop = 1 To Len(newUserCode)
                    strcurrentchar = Mid(newUserCode, iloop, 1)
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
                str = "SELECT USER_ID FROM USERS WHERE (USER_CODE ='" & Trim(newUserCode) & "') "
                Dim cmd1 As New SqlCommand(str, SqlConn)
                SqlConn.Open()
                flag = cmd1.ExecuteScalar
                If flag <> Nothing Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('User Code Already Exists, Please try to enter other User Code... ');", True)
                    'Alert.Show("This User Already Exists !")
                    Me.txt_UserCode.Focus()
                    Exit Sub
                End If
                SqlConn.Close()
                '********************************************************************************************************************

                'Server Validation for Full Name
                Dim Names As String = Nothing
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
                Dim Designation As String = Nothing
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
                Dim Phone As String = Nothing
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
                Dim Mobile As String = Nothing
                Mobile = TrimAll(txt_UserMobile.Text)
                If Not String.IsNullOrEmpty(Mobile) Then
                    Mobile = RemoveQuotes(Mobile)
                    If Mobile.Length > 150 Then
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
                Dim email As String = Nothing
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


                'validation for library
                Dim MainLibCode As String = Nothing
                MainLibCode = Drop_Libraries.SelectedValue
                If Not String.IsNullOrEmpty(MainLibCode) Then
                    MainLibCode = RemoveQuotes(MainLibCode)
                    If MainLibCode.Length > 12 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' LibType is not Valid... ');", True)
                        Drop_Libraries.Focus()
                        Exit Sub
                    End If
                    If InStr(1, MainLibCode, "CREATE", 1) > 0 Or InStr(1, MainLibCode, "DELETE", 1) > 0 Or InStr(1, MainLibCode, "DROP", 1) > 0 Or InStr(1, MainLibCode, "INSERT", 1) > 1 Or InStr(1, MainLibCode, "TRACK", 1) > 1 Or InStr(1, MainLibCode, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' LibType Address is not Valid... ');", True)
                        Drop_Libraries.Focus()
                        Exit Sub
                    End If
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Select the Main Library from drop-down... ');", True)
                    Me.Drop_Libraries.Focus()
                    Exit Sub
                End If

                'check unwanted characters
                c = 0
                Counter7 = 0
                For iloop = 1 To Len(MainLibCode)
                    strcurrentchar = Mid(MainLibCode, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            Counter7 = 1
                        End If
                    End If
                Next
                If Counter7 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Main Library Code is not Valid... ');", True)
                    Exit Sub
                End If

                '****************************************************************************************88
                'remareks
                'Server Validation for Mobile Number
                Dim myRemarks As String = Nothing
                myRemarks = TrimAll(Me.txt_Remarks.Text)
                If Not String.IsNullOrEmpty(myRemarks) Then
                    myRemarks = RemoveQuotes(myRemarks)
                    If myRemarks.Length > 250 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Remarks.Focus()
                        Exit Sub
                    End If
                    myRemarks = " " & myRemarks & " "
                    If InStr(1, myRemarks, "CREATE", 1) > 0 Or InStr(1, myRemarks, "DELETE", 1) > 0 Or InStr(1, myRemarks, "DROP", 1) > 0 Or InStr(1, myRemarks, "INSERT", 1) > 1 Or InStr(1, myRemarks, "TRACK", 1) > 1 Or InStr(1, myRemarks, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Remarks.Focus()
                        Exit Sub
                    End If
                Else
                    myRemarks = String.Empty
                End If

                myRemarks = TrimAll(myRemarks)
                'check unwanted characters
                c = 0
                counter8 = 0
                For iloop = 1 To Len(myRemarks)
                    strcurrentchar = Mid(myRemarks, iloop, 1)
                    If c = 0 Then
                        If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter8 = 1
                        End If
                    End If
                Next
                If counter8 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                    Me.txt_Remarks.Focus()
                    Exit Sub
                End If

        'SERVER VALIDATION FOR PASSWORD
                '*******************************************************************************************************

                Dim Password As Object = Nothing
                Dim Hashed As Object = Nothing
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
                counter9 = 0
        For iloop = 1 To Len(Password)
            strcurrentchar = Mid(Password, iloop, 1)
            If c = 0 Then
                If Not InStr("';""", strcurrentchar) <= 0 Then
                    c = c + 1
                            counter9 = 1
                End If
            End If
                Next
                If counter9 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Do Not Use Un-wanted Characters... ');", True)
                    Me.txt_UserPass.Focus()
                    Exit Sub
                End If

        '****************************************************************************************
        'Server Validation for IP ADDRESS
                Dim IP As String = Nothing
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
                counter10 = 0
        For iloop = 1 To Len(IP)
            strcurrentchar = Mid(IP, iloop, 1)
            If c = 0 Then
                If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                    c = c + 1
                            counter10 = 1
                End If
            End If
        Next
                If counter10 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' IP Address  is not Valid... ');", True)
                    Exit Sub
                End If

        '**************************************************************************************************
        'Check Duplicate User Code
        Dim str2 As Object = Nothing
        Dim flag2 As Object = Nothing
                str2 = "SELECT  USER_ID FROM USERS WHERE (USER_TYPE ='SUSER') AND (LIB_CODE = '" & TrimX(MainLibCode) & "') "
                Dim cmd2 As New SqlCommand(str2, SqlConn)
        SqlConn.Open()
        flag2 = cmd2.ExecuteScalar
        If flag2 <> Nothing Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Super User Already Exists, Only One Super User allowed for One Library!');", True)
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
        SQL = "SELECT * FROM USERS WHERE (USER_ID = '00')"
        Cmd = New SqlCommand(SQL, SqlConn)
        da = New SqlDataAdapter(Cmd)
        CB = New SqlCommandBuilder(da)
        SqlConn.Open()
        da.Fill(ds, "users")
        dtrow = ds.Tables("users").NewRow

                If Not String.IsNullOrEmpty(Names) Then
                    dtrow("USER_NAME") = Names.Trim
                End If

                If Not String.IsNullOrEmpty(newUserCode) Then
                    dtrow("USER_CODE") = newUserCode.Trim
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
                Else
                    dtrow("USER_EMAIL") = System.DBNull.Value
                End If

                If Not String.IsNullOrEmpty(MainLibCode) Then
                    dtrow("LIB_CODE") = MainLibCode.Trim
                Else
                    dtrow("LIB_CODE") = System.DBNull.Value
                End If

                If Not String.IsNullOrEmpty(myRemarks) Then
                    dtrow("REMARKS") = myRemarks.Trim
                Else
                    dtrow("REMARKS") = System.DBNull.Value
                End If

        dtrow("USER_TYPE") = "SUSER"
        dtrow("STATUS") = "CU"

        dtrow("DATE_ADDED") = Now.ToShortDateString
                dtrow("USER_IP") = IP.Trim
                dtrow("USER_ADMIN") = "Y"
                dtrow("USER_ACQ") = "Y"
                dtrow("USER_CAT") = "Y"
                dtrow("USER_CIR") = "Y"
                dtrow("USER_SER") = "Y"
                dtrow("USER_ART") = "Y"
                dtrow("USER_BUDGET") = "Y"
                dtrow("USER_MASTER") = "Y"
                dtrow("USER_SEARCH") = "Y"
                dtrow("USER_ISSUE") = "Y"
                dtrow("USER_RETURN") = "Y"
                dtrow("USER_RESERVE") = "Y"
                dtrow("USER_PRINT") = "Y"
                dtrow("USER_ADDNEW") = "Y"
                dtrow("USER_EDIT") = "Y"
                dtrow("USER_DELETE") = "Y"

        ds.Tables("users").Rows.Add(dtrow)
        da.Update(ds, "users")
        ClearFields()
        'mailpwd(UserCode, email)
                newUserCode = Nothing
                Names = Nothing
                Password = Nothing
                Designation = Nothing
                Phone = Nothing
                Mobile = Nothing
                email = Nothing
                MainLibCode = Nothing
                myRemarks = Nothing

                ds.Dispose()
                Label6.Text = "Super User Added Successfully!"
            Catch ex As Exception
                Label6.Text = "Error: " & (ex.Message())
        Finally
            SqlConn.Close()
        End Try
        End If
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
        txt_Remarks.Text = ""
        Drop_Libraries.Text = ""

        txt_UserCode.Enabled = True
        bttn_Save.Visible = True
        bttn_Update.Visible = False
        tr_status.Visible = False
        trPw1.Visible = True
        trRpw1.Visible = True
        Me.CheckBox1.Checked = True
        Me.CheckBox1.Enabled = False
    End Sub
    Protected Sub Cancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Cancel.Click
        ClearFields()
    End Sub
    'Populate the users in grid     'search users
    Protected Sub Search_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Search_Bttn.Click
        Dim dtUsers As DataTable = Nothing
        Try
            Dim c, counter1, counter2, counter3 As Integer
            Dim iloop As Integer
            Dim strcurrentchar As Object

            'search string validation
            Dim mySearchString As Object = Nothing
            If txt_Search.Text <> "" Then
                mySearchString = TrimAll(txt_Search.Text)
                mySearchString = RemoveQuotes(mySearchString)
                If mySearchString.Length > 250 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                    Me.txt_Search.Focus()
                    Exit Sub
                End If
                mySearchString = " " & mySearchString & " "
                If InStr(1, mySearchString, "CREATE", 1) > 0 Or InStr(1, mySearchString, "DELETE", 1) > 0 Or InStr(1, mySearchString, "DROP", 1) > 0 Or InStr(1, mySearchString, "INSERT", 1) > 1 Or InStr(1, mySearchString, "TRACK", 1) > 1 Or InStr(1, mySearchString, "TRACE", 1) > 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                    Me.txt_Search.Focus()
                    Exit Sub
                End If
                mySearchString = TrimAll(mySearchString)
                'check unwanted characters
                c = 0
                counter1 = 0
                For iloop = 1 To Len(mySearchString)
                    strcurrentchar = Mid(mySearchString, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter1 = 1
                        End If
                    End If
                Next
                If counter1 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' data is not Valid... ');", True)
                    Me.txt_Search.Focus()
                    Exit Sub
                End If
            Else
                mySearchString = String.Empty
            End If

            'Field Name validation
            Dim myfield As String = Nothing
            If DropDownList2.Text <> "" Then
                myfield = TrimAll(DropDownList2.SelectedValue)
                myfield = RemoveQuotes(myfield)
                If myfield.Length > 50 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                    Me.DropDownList2.Focus()
                    Exit Sub
                End If
                myfield = " " & myfield & " "
                If InStr(1, myfield, "CREATE", 1) > 0 Or InStr(1, myfield, "DELETE", 1) > 0 Or InStr(1, myfield, "DROP", 1) > 0 Or InStr(1, myfield, "INSERT", 1) > 1 Or InStr(1, myfield, "TRACK", 1) > 1 Or InStr(1, myfield, "TRACE", 1) > 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                    Me.DropDownList2.Focus()
                    Exit Sub
                End If
                myfield = TrimAll(myfield)
                'check unwanted characters
                c = 0
                counter2 = 0
                For iloop = 1 To Len(myfield)
                    strcurrentchar = Mid(myfield, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter2 = 1
                        End If
                    End If
                Next
                If counter2 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' data is not Valid... ');", True)
                    DropDownList2.Focus()
                    Exit Sub
                End If
            Else
                myfield = "USER_NAME"
            End If

            'Boolean Operator validation
            Dim myBoolean As String = Nothing
            If DropDownList3.Text <> "" Then
                myBoolean = TrimAll(DropDownList3.SelectedValue)
                myBoolean = RemoveQuotes(myBoolean)
                If myBoolean.Length > 20 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                    Me.DropDownList3.Focus()
                    Exit Sub
                End If
                myBoolean = " " & myBoolean & " "
                If InStr(1, myBoolean, "CREATE", 1) > 0 Or InStr(1, myBoolean, "DELETE", 1) > 0 Or InStr(1, myBoolean, "DROP", 1) > 0 Or InStr(1, myBoolean, "INSERT", 1) > 1 Or InStr(1, myBoolean, "TRACK", 1) > 1 Or InStr(1, myBoolean, "TRACE", 1) > 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                    Me.DropDownList3.Focus()
                    Exit Sub
                End If
                myBoolean = TrimAll(myBoolean)
                'check unwanted characters
                c = 0
                counter3 = 0
                For iloop = 1 To Len(myBoolean)
                    strcurrentchar = Mid(myBoolean, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter3 = 1
                        End If
                    End If
                Next
                If counter3 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' data is not Valid... ');", True)
                    DropDownList3.Focus()
                    Exit Sub
                End If
            Else
                myBoolean = "AND"
            End If

            Dim SQL As String = Nothing
            'SQL = "SELECT * FROM USERS WHERE (USER_TYPE ='SUSER') "
            SQL = "SELECT USERS.*, LIBRARIES.LIB_NAME FROM USERS INNER JOIN LIBRARIES ON USERS.LIB_CODE = LIBRARIES.LIB_CODE WHERE (USERS.USER_TYPE ='SUSER')"

            If txt_Search.Text <> "" Then
                If myBoolean = "LIKE" Then
                    SQL = SQL & " AND (" & myfield & " LIKE N'%" & Trim(mySearchString) & "%') "
                End If
                If myBoolean = "SW" Then
                    SQL = SQL & " AND (" & myfield & " LIKE N'" & Trim(mySearchString) & "%') "
                End If
                If myBoolean = "EW" Then
                    SQL = SQL & " AND (" & myfield & " LIKE N'%" & Trim(mySearchString) & "') "
                End If
                If myBoolean = "AND" Then
                    Dim h As Integer
                    Dim myNewSearchString As Object
                    myNewSearchString = Split(mySearchString, " ")
                    SQL = SQL & " AND (" & myfield & " LIKE N'%" & Trim(myNewSearchString(0)) & "%' "
                    For h = 1 To UBound(myNewSearchString)
                        SQL = SQL & " AND " & myfield & " LIKE N'%" & Trim(myNewSearchString(h)) & "%'"
                    Next
                    SQL = SQL & ")"
                End If
                If myBoolean = "OR" Then
                    Dim h As Integer
                    Dim myNewSearchString As Object
                    myNewSearchString = Split(mySearchString, " ")
                    SQL = SQL & " AND (" & myfield & " LIKE N'%" & Trim(myNewSearchString(0)) & "%' "
                    For h = 1 To UBound(myNewSearchString)
                        SQL = SQL & " OR " & myfield & " LIKE N'%" & Trim(myNewSearchString(h)) & "%' "
                    Next
                    SQL = SQL & ")"
                End If
            End If
            SQL = SQL & " ORDER BY USER_NAME ASC "

            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            da.Fill(ds)

            dtUsers = ds.Tables(0).Copy
            Dim RecordCount As Long = 0
            If dtUsers.Rows.Count = 0 Then
                Me.Grid2.DataSource = Nothing
                Grid2.DataBind()
                Label5.Text = "Total Record(s): " & Grid2.Rows.Count
            Else
                RecordCount = dtUsers.Rows.Count
                Grid2.DataSource = dtUsers
                Grid2.DataBind()
                Label5.Text = "Total Record(s): " & Grid2.Rows.Count
            End If
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'get value of Libcode from grid
    Private Sub Grid2_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles Grid2.RowCommand
        Dim Command As SqlCommand = Nothing
        Dim drUSERS As SqlDataReader = Nothing
        Try
           If e.CommandName = "Select" Then
                Dim myRowID As Integer
                Dim USER_CODE As Object = Nothing
                myRowID = e.CommandArgument.ToString()
                USER_CODE = Grid2.DataKeys(myRowID).Value
                If USER_CODE <> "" Then
                    Dim iloop As Integer
                    Dim strcurrentchar As Object
                    Dim c As Integer
                    Dim counter1 As Integer

                    USER_CODE = TrimX(USER_CODE)
                    USER_CODE = RemoveQuotes(USER_CODE)

                    If Len(USER_CODE).ToString > 10 Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Length of Input is not Proper!');", True)
                        Exit Sub
                    End If
                    USER_CODE = " " & USER_CODE & " "
                    If InStr(1, USER_CODE, " CREATE ", 1) > 0 Or InStr(1, USER_CODE, " DELETE ", 1) > 0 Or InStr(1, USER_CODE, " DROP ", 1) > 0 Or InStr(1, USER_CODE, " INSERT ", 1) > 1 Or InStr(1, USER_CODE, " TRACK ", 1) > 1 Or InStr(1, USER_CODE, " TRACE ", 1) > 1 Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Do Not Use Reserve Words!');", True)
                        Exit Sub
                    End If
                    USER_CODE = TrimX(USER_CODE)

                    'get record details from database
                    Dim SQL As String = Nothing
                    SQL = " SELECT *  FROM USERS WHERE (USER_CODE = '" & TrimX(USER_CODE) & "') "
                    Command = New SqlCommand(SQL, SqlConn)
                    SqlConn.Open()
                    drUSERS = Command.ExecuteReader(CommandBehavior.CloseConnection)
                    drUSERS.Read()
                    If drUSERS.HasRows = True Then
                        If drUSERS.Item("USER_CODE").ToString <> "" Then
                            txt_UserCode.Text = drUSERS.Item("USER_CODE").ToString
                        Else
                            txt_UserCode.Text = ""
                        End If

                        If drUSERS.Item("USER_NAME").ToString <> "" Then
                            txt_UserName.Text = drUSERS.Item("USER_NAME").ToString
                        Else
                            txt_UserName.Text = ""
                        End If
                        If drUSERS.Item("USER_DESIG").ToString <> "" Then
                            txt_UserDesig.Text = drUSERS.Item("USER_DESIG").ToString
                        Else
                            txt_UserDesig.Text = ""
                        End If
                        If drUSERS.Item("PHONE").ToString <> "" Then
                            txt_UserPhone.Text = drUSERS.Item("PHONE").ToString
                        Else
                            txt_UserPhone.Text = ""
                        End If
                        If drUSERS.Item("MOBILE").ToString <> "" Then
                            txt_UserMobile.Text = drUSERS.Item("MOBILE").ToString
                        Else
                            txt_UserMobile.Text = ""
                        End If
                        If drUSERS.Item("USER_EMAIL").ToString <> "" Then
                            txt_UserEmail.Text = drUSERS.Item("USER_EMAIL").ToString
                        Else
                            txt_UserEmail.Text = ""
                        End If
                        If drUSERS.Item("REMARKS").ToString <> "" Then
                            txt_Remarks.Text = drUSERS.Item("REMARKS").ToString
                        Else
                            txt_Remarks.Text = ""
                        End If
                        If drUSERS.Item("LIB_CODE").ToString <> "" Then
                            Drop_Libraries.SelectedValue = drUSERS.Item("LIB_CODE").ToString
                        Else
                            Drop_Libraries.Text = ""
                        End If

                        bttn_Save.Visible = False
                        bttn_Update.Visible = True

                        CheckBox1.Enabled = True
                        CheckBox1.Checked = False
                    Else
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' No Record to Edit... ');", True)
                    End If
                End If
            End If
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub 'Grid1_ItemCommand
    'update Record
    Protected Sub bttn_Update_Click(ByVal sender As Object, ByVal e As EventArgs) Handles bttn_Update.Click
        Dim SQL As String = Nothing
        Dim da As SqlDataAdapter
        Dim cb As SqlCommandBuilder
        Dim ds As New DataSet
        Dim dt As New DataTable
        Try
            If IsPostBack = True Then
                'Server Validation for Lib Code
                Dim iloop As Integer
                Dim strcurrentchar As Object
                Dim c As Integer
                Dim counter1, counter2, counter3, counter4, Counter5, counter6, counter7, counter8, counter9, counter10 As Integer

                'Server Validation for User Code
                Dim newUserCode As String = Nothing
                newUserCode = TrimX(txt_UserCode.Text)
                newUserCode = UCase(newUserCode)
                If Not String.IsNullOrEmpty(newUserCode) Then
                    UserCode = RemoveQuotes(newUserCode)
                    If newUserCode.Length > 12 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Length of User Code is not Proper... ');", True)
                        Me.txt_UserCode.Focus()
                        Exit Sub
                    End If
                    newUserCode = " " & newUserCode & " "
                    If InStr(1, newUserCode, " CREATE ", 1) > 0 Or InStr(1, newUserCode, " DELETE ", 1) > 0 Or InStr(1, newUserCode, " DROP ", 1) > 0 Or InStr(1, newUserCode, " INSERT ", 1) > 1 Or InStr(1, newUserCode, " TRACK ", 1) > 1 Or InStr(1, newUserCode, " TRACE ", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do not use reserve words... ');", True)
                        Me.txt_UserCode.Focus()
                        Exit Sub
                    End If
                    newUserCode = TrimX(newUserCode)
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Enter User Code... ');", True)
                    Me.txt_UserCode.Focus()
                    Exit Sub
                End If
                'check unwanted characters
                c = 0
                counter1 = 0
                For iloop = 1 To Len(newUserCode)
                    strcurrentchar = Mid(newUserCode, iloop, 1)
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

                'Server Validation for Full Name
                Dim Names As String = Nothing
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
                Dim Designation As String = Nothing
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
                Dim Phone As String = Nothing
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
                Dim Mobile As String = Nothing
                Mobile = TrimAll(txt_UserMobile.Text)
                If Not String.IsNullOrEmpty(Mobile) Then
                    Mobile = RemoveQuotes(Mobile)
                    If Mobile.Length > 150 Then
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
                Dim email As String = Nothing
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

                'validation for library
                Dim MainLibCode As String = Nothing
                MainLibCode = Drop_Libraries.SelectedValue
                If Not String.IsNullOrEmpty(MainLibCode) Then
                    MainLibCode = RemoveQuotes(MainLibCode)
                    If MainLibCode.Length > 12 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' LibType is not Valid... ');", True)
                        Drop_Libraries.Focus()
                        Exit Sub
                    End If
                    If InStr(1, MainLibCode, "CREATE", 1) > 0 Or InStr(1, MainLibCode, "DELETE", 1) > 0 Or InStr(1, MainLibCode, "DROP", 1) > 0 Or InStr(1, MainLibCode, "INSERT", 1) > 1 Or InStr(1, MainLibCode, "TRACK", 1) > 1 Or InStr(1, MainLibCode, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' LibType Address is not Valid... ');", True)
                        Drop_Libraries.Focus()
                        Exit Sub
                    End If
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Select the Main Library from drop-down... ');", True)
                    Me.Drop_Libraries.Focus()
                    Exit Sub
                End If

                'check unwanted characters
                c = 0
                Counter7 = 0
                For iloop = 1 To Len(MainLibCode)
                    strcurrentchar = Mid(MainLibCode, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            Counter7 = 1
                        End If
                    End If
                Next
                If Counter7 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Main Library Code is not Valid... ');", True)
                    Exit Sub
                End If

                'Check Duplicate User Code
                Dim str2 As Object = Nothing
                Dim flag2 As Object = Nothing
                str2 = "SELECT  USER_ID FROM USERS WHERE (USER_TYPE ='SUSER') AND (LIB_CODE= '" & TrimX(MainLibCode) & "') AND (USER_CODE <> '" & TrimX(newUserCode) & "')"
                Dim cmd2 As New SqlCommand(str2, SqlConn)
                SqlConn.Open()
                flag2 = cmd2.ExecuteScalar
                If flag2 <> Nothing Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Super User Already Exists for This Library, Please abort... ');", True)
                    Me.txt_UserCode.Focus()
                    Exit Sub
                End If
                SqlConn.Close()
                '****************************************************************************************88
                'remareks
                Dim myRemarks As String = Nothing
                myRemarks = TrimAll(Me.txt_Remarks.Text)
                If Not String.IsNullOrEmpty(myRemarks) Then
                    myRemarks = RemoveQuotes(myRemarks)
                    If myRemarks.Length > 250 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Remarks.Focus()
                        Exit Sub
                    End If
                    myRemarks = " " & myRemarks & " "
                    If InStr(1, myRemarks, "CREATE", 1) > 0 Or InStr(1, myRemarks, "DELETE", 1) > 0 Or InStr(1, myRemarks, "DROP", 1) > 0 Or InStr(1, myRemarks, "INSERT", 1) > 1 Or InStr(1, myRemarks, "TRACK", 1) > 1 Or InStr(1, myRemarks, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Remarks.Focus()
                        Exit Sub
                    End If
                Else
                    myRemarks = String.Empty
                End If

                myRemarks = TrimAll(myRemarks)
                'check unwanted characters
                c = 0
                counter8 = 0
                For iloop = 1 To Len(myRemarks)
                    strcurrentchar = Mid(myRemarks, iloop, 1)
                    If c = 0 Then
                        If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter8 = 1
                        End If
                    End If
                Next
                If counter8 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                    Me.txt_Remarks.Focus()
                    Exit Sub
                End If



                'SERVER VALIDATION FOR PASSWORD
                '*******************************************************************************************************
                Dim Password As Object = Nothing
                If CheckBox1.Checked = True Then
                    Dim Hashed As Object = Nothing
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
                    counter9 = 0
                    For iloop = 1 To Len(Password)
                        strcurrentchar = Mid(Password, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter9 = 1
                            End If
                        End If
                    Next
                    If counter9 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Do Not Use Un-wanted Characters... ');", True)
                        Me.txt_UserPass.Focus()
                        Exit Sub
                    End If
                End If

                '****************************************************************************************************
                'UPDATE THE LIBRARY PROFILE   
                SQL = "SELECT * FROM USERS WHERE (USER_CODE='" & Trim(newUserCode) & "')"
                SqlConn.Open()
                da = New SqlDataAdapter(SQL, SqlConn)
                cb = New SqlCommandBuilder(da)
                da.Fill(ds, "USERS")
                If ds.Tables("USERS").Rows.Count <> 0 Then
                    If CheckBox1.Checked = True Then
                        ds.Tables("USERS").Rows(0)("USER_PASSWORD") = Password.Trim
                    End If
                    If Not String.IsNullOrEmpty(Names) Then
                        ds.Tables("USERS").Rows(0)("USER_NAME") = Names.Trim
                    Else
                        ds.Tables("USERS").Rows(0)("USER_NAME") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(Designation) Then
                        ds.Tables("USERS").Rows(0)("USER_DESIG") = Designation.Trim
                    Else
                        ds.Tables("USERS").Rows(0)("USER_DESIG") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(Phone) Then
                        ds.Tables("USERS").Rows(0)("PHONE") = Phone.Trim
                    Else
                        ds.Tables("USERS").Rows(0)("PHONE") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(Mobile) Then
                        ds.Tables("USERS").Rows(0)("MOBILE") = Mobile.Trim
                    Else
                        ds.Tables("USERS").Rows(0)("MOBILE") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(email) Then
                        ds.Tables("USERS").Rows(0)("USER_EMAIL") = email.Trim
                    Else
                        ds.Tables("USERS").Rows(0)("USER_EMAIL") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(MainLibCode) Then
                        ds.Tables("USERS").Rows(0)("LIB_CODE") = MainLibCode.Trim
                    Else
                        ds.Tables("USERS").Rows(0)("LIB_CODE") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(myRemarks) Then
                        ds.Tables("USERS").Rows(0)("REMARKS") = myRemarks.ToString.Trim
                    Else
                        ds.Tables("USERS").Rows(0)("REMARKS") = System.DBNull.Value
                    End If

                    ds.Tables("USERS").Rows(0)("DATE_MODIFIED") = Now.Date
                    ds.Tables("USERS").Rows(0)("USER_IP") = Request.UserHostAddress.Trim
                    da.Update(ds, "USERS")
                    Names = Nothing
                    Designation = Nothing
                    Phone = Nothing
                    Mobile = Nothing
                    email = Nothing
                    MainLibCode = Nothing
                    myRemarks = Nothing
                    Label6.Visible = True
                    bttn_Save.Visible = True
                    bttn_Update.Visible = False

                    CheckBox1.Enabled = False
                    CheckBox1.Checked = True

                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' User Profile Updated Successfully... ');", True)
                    ClearFields()
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('User Profile Update  - Please Contact System Administrator... ');", True)
                    Exit Sub
                End If
            End If
            SqlConn.Close()
            AllLibraries()
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
End Class