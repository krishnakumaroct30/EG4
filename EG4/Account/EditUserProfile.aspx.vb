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
Public Class EditUserProfile
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session.Item("IsThisU") <> Nothing Then
                If Not SConnection() = True Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Database Connection is lost..Try Again !');", True)
                Else
                    txt_UserCode.Enabled = False
                    txt_UserCode.Text = UserCode
                    If Page.IsPostBack = False Then
                        GetUserDetails()
                    End If
                    Me.txt_UserName.Focus()
                    End If
                'edit profile button in administrator

                Dim CreateSUserButton As Object 'System.Web.UI.WebControls.Button
                CreateSUserButton = Master.FindControl("Accordion1").FindControl("DBAdminPane").FindControl("Adm_Edit_Bttn")
                CreateSUserButton.ForeColor = Drawing.Color.Red
                ' 
                'edit profile button in lib admin
                Dim EditLibUserButton As Object 'System.Web.UI.WebControls.Button
                EditLibUserButton = Master.FindControl("Accordion1").FindControl("MasterPane").FindControl("M_UpdateProfile_Bttn")
                EditLibUserButton.ForeColor = Drawing.Color.Red

                'edit profile button in lib admin
                Dim EditLibSUserButton As Object 'System.Web.UI.WebControls.Button
                EditLibSUserButton = Master.FindControl("Accordion1").FindControl("LibAdminPane").FindControl("Lib_UpdateSUserProfile_Bttn")
                EditLibSUserButton.ForeColor = Drawing.Color.Red
            Else
                Response.Redirect("~/Default.aspx", False)
            End If
        Catch ex As Exception
            Label6.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    Private Sub EditUserProfile_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
            Me.txt_UserName.Focus()
    End Sub
    Public Sub GetUserDetails()
        Dim Command As SqlCommand = Nothing
        Dim drUSERS As SqlDataReader = Nothing
        Try
            Dim myEdUserCode As String = String.Empty
            myEdUserCode = UserCode
            Me.CheckBox1.Checked = False
            Me.CheckBox1.Enabled = True

            Dim iloop As Integer
            Dim strcurrentchar As Object
            Dim c As Integer
            Dim counter1 As Integer

            myEdUserCode = TrimX(myEdUserCode)
            myEdUserCode = UCase(myEdUserCode)
                If Not String.IsNullOrEmpty(myEdUserCode) Then
                    myEdUserCode = RemoveQuotes(myEdUserCode)
                    If myEdUserCode.Length > 12 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Length of User Code is not Proper... ');", True)
                        Me.txt_UserCode.Focus()
                        Exit Sub
                    End If
                    myEdUserCode = " " & myEdUserCode & " "
                    If InStr(1, myEdUserCode, " CREATE ", 1) > 0 Or InStr(1, myEdUserCode, " DELETE ", 1) > 0 Or InStr(1, myEdUserCode, " DROP ", 1) > 0 Or InStr(1, myEdUserCode, " INSERT ", 1) > 1 Or InStr(1, myEdUserCode, " TRACK ", 1) > 1 Or InStr(1, myEdUserCode, " TRACE ", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do not use reserve words... ');", True)
                        Me.txt_UserCode.Focus()
                        Exit Sub
                    End If
                    myEdUserCode = TrimX(myEdUserCode)
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Enter User Code... ');", True)
                    Me.txt_UserCode.Focus()
                    Exit Sub
                End If
                'check unwanted characters
                c = 0
                counter1 = 0
                For iloop = 1 To Len(myEdUserCode)
                    strcurrentchar = Mid(myEdUserCode, iloop, 1)
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

                'get record details from database
                Dim SQL As String = Nothing
                SQL = " SELECT *  FROM USERS WHERE (USER_CODE = '" & TrimX(Me.txt_UserCode.Text) & "') "
                Command = New SqlCommand(SQL, SqlConn)
                SqlConn.Open()
                drUSERS = Command.ExecuteReader(CommandBehavior.CloseConnection)
                drUSERS.Read()
                If drUSERS.HasRows = True Then
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
                If drUSERS.Item("USER_Q").ToString <> "" Then
                    DropDownList1.SelectedValue = drUSERS.Item("USER_Q").ToString
                Else
                    DropDownList1.Text = ""
                End If
                If drUSERS.Item("USER_ANS").ToString <> "" Then
                    txt_UserAns.Text = drUSERS.Item("USER_ANS").ToString
                Else
                    txt_UserAns.Text = ""
                End If
                If drUSERS.Item("USER_TYPE").ToString <> "" And drUSERS.Item("USER_TYPE").ToString = "ADMIN" Then
                    DropDownList2.SelectedValue = Nothing
                    tr_download.Visible = False
                Else
                    tr_download.Visible = True
                    DropDownList2.SelectedValue = drUSERS.Item("DOWNLOAD_CATS").ToString
                End If
                If drUSERS.Item("USER_TYPE").ToString <> "" And drUSERS.Item("USER_TYPE").ToString = "ADMIN" Then
                    DropDownList3.SelectedValue = Nothing
                    tr_dispaly.Visible = False
                Else
                    tr_dispaly.Visible = True
                    DropDownList3.SelectedValue = drUSERS.Item("DISPLAY_CATS").ToString
                End If
                If drUSERS.Item("USER_PHOTO").ToString <> "" Then
                    Dim strURL As String = "~/Account/User_GetPhoto.aspx?USER_CODE=" & txt_UserCode.Text & ""
                    Image21.ImageUrl = strURL
                Else
                    Image21.Visible = False
                End If

                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' No Record to Edit... ');", True)
                    Exit Sub
            End If
            Me.CheckBox1.Checked = False
            Me.CheckBox1.Enabled = True
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
        Finally
            Command.Dispose()
            drUSERS.Close()
            SqlConn.Close()
        End Try
    End Sub
    Public Sub ClearFields()
        Me.txt_UserDesig.Text = ""
        Me.txt_UserEmail.Text = ""
        Me.txt_UserMobile.Text = ""
        txt_UserName.Text = ""
        txt_UserPass.Text = ""
        txt_UserPhone.Text = ""
        txt_UserRePass.Text = ""
        txt_Remarks.Text = ""
        If tr_download.Visible = True Then
            DropDownList2.Text = ""
        End If
        If tr_dispaly.Visible = True Then
            DropDownList3.Text = ""
        End If
    End Sub
    Protected Sub Cancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Cancel.Click
        ClearFields()
    End Sub
    'update Record
    Protected Sub bttn_Update_Click(ByVal sender As Object, ByVal e As EventArgs) Handles bttn_Update.Click
        Dim SQL As String = Nothing
        Dim da As SqlDataAdapter = Nothing
        Dim cb As SqlCommandBuilder = Nothing
        Dim ds As New DataSet
        Dim dt As New DataTable
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing
        Try
            If IsPostBack = True Then
                'Server Validation for Lib Code
                Dim iloop As Integer
                Dim strcurrentchar As Object
                Dim c As Integer
                Dim counter1, counter2, counter3, counter4, Counter5, counter6, counter7, counter8, counter9, counter10, counter11, counter12 As Integer

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
                counter7 = 0
                For iloop = 1 To Len(myRemarks)
                    strcurrentchar = Mid(myRemarks, iloop, 1)
                    If c = 0 Then
                        If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter7 = 1
                        End If
                    End If
                Next
                If counter7 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                    Me.txt_Remarks.Focus()
                    Exit Sub
                End If

                'Server Validation for security Q ************************************************
                Dim SecQ As String
                SecQ = TrimAll(DropDownList1.Text)
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
                counter8 = 0
                For iloop = 1 To Len(SecQ)
                    strcurrentchar = Mid(SecQ, iloop, 1)
                    If c = 0 Then
                        If Not InStr("'~!@#$^&*+|[]{}<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter8 = 1
                        End If
                    End If
                Next
                If counter8 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Un-Wated Characters... ');", True)
                    DropDownList1.Focus()
                    Exit Sub
                End If


                'Server Validation for  security Answer 
                If DropDownList1.Text <> "" And txt_UserAns.Text = "" Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Plz Enter the Answer of the above Security Question.. ');", True)
                    txt_UserAns.Focus()
                    Exit Sub
                End If
                Dim SecAns As String
                SecAns = TrimAll(Me.txt_UserAns.Text)
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
                counter9 = 0
                For iloop = 1 To Len(SecAns)
                    strcurrentchar = Mid(SecAns, iloop, 1)
                    If c = 0 Then
                        If Not InStr("'~!@#$^&*+|[]{}?<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter9 = 1
                        End If
                    End If
                Next
                If counter9 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Un-Wated Characters... ');", True)
                    txt_UserAns.Focus()
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
                counter10 = 0
                For iloop = 1 To Len(Password)
                    strcurrentchar = Mid(Password, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter10 = 1
                        End If
                    End If
                Next
                If counter10 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Do Not Use Un-wanted Characters... ');", True)
                    Me.txt_UserPass.Focus()
                    Exit Sub
                End If
            End If

            'SERVER VALIDATION FOR downlaod rcord fropm internet
            '*******************************************************************************************************
                Dim myDownload As Object = Nothing
                If tr_download.Visible = True Then
                    If DropDownList2.Text <> "" Then
                        myDownload = DropDownList2.SelectedValue
                        If Not String.IsNullOrEmpty(myDownload) Then
                            myDownload = RemoveQuotes(myDownload)
                            If myDownload.Length > 2 Then
                                ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Length of Field is not Proper... ');", True)
                                Me.DropDownList2.Focus()
                                Exit Sub
                            End If
                            If InStr(1, myDownload, "CREATE", 1) > 0 Or InStr(1, myDownload, "DELETE", 1) > 0 Or InStr(1, myDownload, "DROP", 1) > 0 Or InStr(1, myDownload, "INSERT", 1) > 1 Or InStr(1, myDownload, "TRACK", 1) > 1 Or InStr(1, myDownload, "TRACE", 1) > 1 Then
                                ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Do not use reserve words... ');", True)
                                Me.DropDownList2.Focus()
                                Exit Sub
                            End If
                        Else
                            myDownload = "Y"
                        End If
                        'check unwanted characters
                        c = 0
                        counter11 = 0
                        For iloop = 1 To Len(myDownload)
                            strcurrentchar = Mid(myDownload, iloop, 1)
                            If c = 0 Then
                                If Not InStr("';""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter11 = 1
                                End If
                            End If
                        Next
                        If counter11 = 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Do Not Use Un-wanted Characters... ');", True)
                            Me.DropDownList2.Focus()
                            Exit Sub
                        End If
                    Else
                        myDownload = "Y"
                    End If
                Else
                    myDownload = "N"
                End If


                Dim myDisplay As Object = Nothing
                If tr_dispaly.Visible = True Then
                    If DropDownList3.Text <> "" Then
                        myDisplay = DropDownList3.SelectedValue
                        If Not String.IsNullOrEmpty(myDisplay) Then
                            myDisplay = RemoveQuotes(myDisplay)
                            If myDisplay.Length > 2 Then
                                ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Length of Field is not Proper... ');", True)
                                Me.DropDownList3.Focus()
                                Exit Sub
                            End If
                            If InStr(1, myDisplay, "CREATE", 1) > 0 Or InStr(1, myDisplay, "DELETE", 1) > 0 Or InStr(1, myDisplay, "DROP", 1) > 0 Or InStr(1, myDisplay, "INSERT", 1) > 1 Or InStr(1, myDisplay, "TRACK", 1) > 1 Or InStr(1, myDisplay, "TRACE", 1) > 1 Then
                                ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Do not use reserve words... ');", True)
                                Me.DropDownList3.Focus()
                                Exit Sub
                            End If
                        Else
                            myDisplay = "N"
                        End If
                        'check unwanted characters
                        c = 0
                        counter12 = 0
                        For iloop = 1 To Len(myDisplay)
                            strcurrentchar = Mid(myDisplay, iloop, 1)
                            If c = 0 Then
                                If Not InStr("';""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter12 = 1
                                End If
                            End If
                        Next
                        If counter12 = 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Do Not Use Un-wanted Characters... ');", True)
                            Me.DropDownList3.Focus()
                            Exit Sub
                        End If
                    Else
                        myDisplay = "N"
                    End If
                Else
                    myDisplay = "N"
                End If

                'upload user photo
                Dim arrContent2 As Byte()
                Dim intLength2 As Integer = 0

                If FileUpload13.FileName = "" Then
                    '    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Select Proper file... ');", True)
                    '    Me.FileUpload12.Focus()
                    '    Exit Sub
                Else
                    Dim fileName As String = FileUpload13.PostedFile.FileName
                    Dim ext As String = fileName.Substring(fileName.LastIndexOf("."))
                    ext = ext.ToLower
                    Dim imgType = FileUpload13.PostedFile.ContentType

                    If ext = ".jpg" Then

                    ElseIf ext = ".bmp" Then

                    ElseIf ext = ".gif" Then

                    ElseIf ext = "jpg" Then

                    ElseIf ext = "bmp" Then

                    ElseIf ext = "gif" Then

                    Else
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Select Only gif, bmp, or jpg format files supported... ');", True)
                        Me.FileUpload13.Focus()
                        Exit Sub
                    End If
                    intLength2 = Convert.ToInt32(FileUpload13.PostedFile.InputStream.Length)
                    ReDim arrContent2(intLength2)
                    If intLength2 > 9000 Then
                        Label6.Text = "Error: Photo Size is Bigger than 6 KB"
                        Exit Sub
                    End If

                    FileUpload13.PostedFile.InputStream.Read(arrContent2, 0, intLength2)

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
                    If Not String.IsNullOrEmpty(SecQ) Then
                        ds.Tables("USERS").Rows(0)("USER_Q") = SecQ.Trim
                    End If
                    If Not String.IsNullOrEmpty(SecAns) Then
                        ds.Tables("USERS").Rows(0)("USER_ANS") = SecAns.Trim
                    End If
                    If Not String.IsNullOrEmpty(myRemarks) Then
                        ds.Tables("USERS").Rows(0)("REMARKS") = myRemarks.ToString.Trim
                    Else
                        ds.Tables("USERS").Rows(0)("REMARKS") = System.DBNull.Value
                    End If
                    ds.Tables("USERS").Rows(0)("DOWNLOAD_CATS") = myDownload
                    ds.Tables("USERS").Rows(0)("DISPLAY_CATS") = myDisplay

                    ds.Tables("USERS").Rows(0)("DATE_MODIFIED") = Now.Date
                    ds.Tables("USERS").Rows(0)("USER_IP") = Request.UserHostAddress.Trim
                    If FileUpload13.FileName <> "" Then
                        ds.Tables("USERS").Rows(0)("USER_PHOTO") = arrContent2
                    End If

                    thisTransaction = SqlConn.BeginTransaction()
                    da.SelectCommand.Transaction = thisTransaction
                    da.Update(ds, "USERS")
                    thisTransaction.Commit()

                    Names = Nothing
                    Designation = Nothing
                    Phone = Nothing
                    Mobile = Nothing
                    email = Nothing
                    myRemarks = Nothing
                    Label6.Visible = True
                    Label6.Text = "User Profile Updated Successfully"
                    'ScriptManager.RegisterStartupScript((Me.GetType(), "myalert", "alert(' User Profile Updated Successfully... ');", True)

                    ' ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert(' User Profile Updated Successfully...');", True)

                    ClearFields()
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('User Profile Update  - Please Contact System Administrator... ');", True)
                    Exit Sub
                End If
            End If
                SqlConn.Close()
        Catch q As SqlException
            thisTransaction.Rollback()
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
            GetUserDetails()
        End Try
    End Sub
End Class