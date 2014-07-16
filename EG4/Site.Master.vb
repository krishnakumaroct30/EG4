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
Public Class Site
    Inherits System.Web.UI.MasterPage
    Dim ds As New DataSet
    Public CatCount As Long = 0
    Public HoldCount As Long = 0
    Public LibCount As Integer = 0
    Public mySalt As Object
    Public LoginUserName As Object
    Dim dr As SqlDataReader
    Dim myLastIP, myLastDate, myLastTime As Object
    Public CLUSTER_NAME As String = Nothing
    Public LIBRARY_NAME As String = Nothing
    Public LIBRARY_ADD As String = Nothing
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Page.Master.IsPostBack = False Then
            mySalt = RandomString(10)
            Session.Item("MySaltRKM") = mySalt
            UserCode = Session.Item("LoggedUser")
            If Session.Item("LoggedUserName") <> "" Then
                Label2.Text = "Login: " & Session.Item("LoggedUserName")
                GetLastLogin()
            End If
        Else
            UserCode = Session.Item("LoggedUser")
            LibCode = Session("LIBCODE")
            mySalt = Session.Item("MySaltRKM")
            If Session.Item("LoggedUserName") <> "" Then
                Label2.Text = "Login: " & Session.Item("LoggedUserName")
            End If
        End If
    End Sub
    'load pages
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Page.Header.DataBind()
            If Not SConnection() = True Then
                FGP.Visible = False
                TrLogOut.Visible = False
                CreateAdmin_Bttn.Visible = False
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Database Connection is lost..Try Again !');", True)
            Else
                If Page.Master.IsPostBack = False Then
                    Session.Item("IsThisU") = True 'this
                    txtUserCode.Focus() 'this
                End If
                If Label2.Text <> "" Then
                    TrLogOut.Visible = True
                    Tdx.Visible = False
                Else
                    TrLogOut.Visible = False
                End If
                If UserCode = "" Then
                    TRModules.Visible = False
                    TRAccordion.Visible = False
                    Me.Accordion1.Enabled = False
                    Me.Accordion1.Visible = False
                    TrUserPhoto.Visible = False
                Else
                    TrUserPhoto.Visible = False
                    GetUserPermission()
                End If
                GetCatsStatistics()
                GetHoldStatistics()
                GetLibStatistics()
                GetUserStatistics()
                GetClusterDetails()
                GetLibraryDetails()
            End If

            If myPaneName <> "" Then
                Dim mynewid As Integer = Nothing
                For i As Integer = 0 To Accordion1.Panes.Count - 1
                    If Accordion1.Panes(i).Enabled = True Then
                        If mynewid = 0 Then
                            mynewid = 1
                        Else
                            mynewid = mynewid + 1
                        End If
                        If Accordion1.Panes(i).ID = myPaneName Then
                            Accordion1.SelectedIndex = mynewid - 1
                            Exit For
                        End If
                    End If
                Next i
            End If
            'GetCatsStatistics()
            'GetHoldStatistics()
            'GetLibStatistics()
            'GetUserStatistics()
            'GetClusterDetails()
            'GetLibraryDetails()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Main Form error: Contact Administrator !');", True)
        End Try
    End Sub
    'get User statistics
    Public Sub GetUserStatistics()
        Dim Command As SqlCommand
        Dim drUsers As SqlDataReader = Nothing
        Try
            Command = New SqlCommand("SELECT  USER_ID FROM USERS WHERE (STATUS = 'CU')  ", SqlConn)
            SqlConn.Open()
            drUsers = Command.ExecuteReader(CommandBehavior.CloseConnection)

            If drUsers.HasRows = True Then
                If Session.Item("LoggedUserName") = "" Then
                    Me.trLogin.Visible = True
                    trUser.Visible = True
                    Me.trPassword.Visible = True
                    trcaptcha.Visible = True
                    trButton.Visible = True
                    trLable.Visible = True
                    FGP.Visible = True
                    CreateAdmin_Bttn.Visible = False
                Else
                    Me.trLogin.Visible = False
                    trUser.Visible = False
                    Me.trPassword.Visible = False
                    trcaptcha.Visible = False
                    trButton.Visible = False
                    trLable.Visible = False
                    FGP.Visible = False
                    CreateAdmin_Bttn.Visible = False
                End If
            Else
                Me.trLogin.Visible = False
                trUser.Visible = False
                Me.trPassword.Visible = False
                trButton.Visible = False
                trLable.Visible = False
                FGP.Visible = False
                CreateAdmin_Bttn.Visible = True
            End If
        Catch s As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Get User Stat Error: Contact Administrator !');", True)
        Finally
            drUsers.Dispose()
            SqlConn.Close()
        End Try
    End Sub
    'get librry statistics
    Public Sub GetCatsStatistics()
        Dim Command As SqlCommand = Nothing
        Dim drCats As SqlDataReader = Nothing
        Try
            Command = New SqlCommand("SELECT  COUNT(CAT_NO) as Total FROM CATS ", SqlConn)
            SqlConn.Open()
            drCats = Command.ExecuteReader(CommandBehavior.CloseConnection)

            If drCats.HasRows Then
                drCats.Read()
                CatCount = drCats.Item("Total")
                drCats.Close()
            End If
        Catch s As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Get Cat Stat Error: Contact Administrator !');", True)
        Finally
            Command.Dispose()
            drCats.Dispose()
            SqlConn.Close()
        End Try
    End Sub
    'get holdings statistics
    Public Sub GetHoldStatistics()
        Dim Command As SqlCommand = Nothing
        Dim drHold As SqlDataReader = Nothing
        Try
            Command = New SqlCommand("SELECT  COUNT(HOLD_ID) as Total FROM HOLDINGS ", SqlConn)
            SqlConn.Open()
            drHold = Command.ExecuteReader(CommandBehavior.CloseConnection)

            If drHold.HasRows Then
                drHold.Read()
                HoldCount = drHold.Item("Total")
                drHold.Close()
            End If
        Catch s As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Get Hold Stat Error: Contact Administrator !');", True)
        Finally
            Command.Dispose()
            drHold.Dispose()
            SqlConn.Close()
        End Try
    End Sub
    'get no of libraries
    Public Sub GetLibStatistics()
        Dim Command As SqlCommand = Nothing
        Dim drLib As SqlDataReader = Nothing
        Try
            Command = New SqlCommand("SELECT  COUNT(LIB_ID) as Total FROM LIBRARIES ", SqlConn)
            SqlConn.Open()
            drLib = Command.ExecuteReader(CommandBehavior.CloseConnection)

            If drLib.HasRows Then
                drLib.Read()
                LibCount = drLib.Item("Total")
                drLib.Close()
            End If
        Catch s As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Get Lib Stat Error: Contact Administrator !');", True)
        Finally
            Command.Dispose()
            drLib.Dispose()
            SqlConn.Close()
        End Try
    End Sub
    Protected Sub bttn_LogIn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles bttn_LogIn.Click
        Try
            paneSelectedIndex = 0

            'captcha validation
            Dim loginCAPTCHA As WebControlCaptcha.CaptchaControl = Me.CAPTCHA

            If loginCAPTCHA.UserValidated = False Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Captcha Value is not Valid!');", True)
                Me.CAPTCHA.Focus()
                Exit Sub
            End If

            'Server Validation for User Code
            Dim c As Integer = 0
            Dim counter1 As Integer = 0
            Dim counter2 As Integer = 0
            Dim strcurrentchar As Object = ""
            UserCode = TrimX(txtUserCode.Text)
            UserCode = UCase(UserCode)
            If Not String.IsNullOrEmpty(UserCode) Then
                UserCode = RemoveQuotes(UserCode)
                If UserCode.Length > 12 Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Length of User Code is not Proper... ');", True)
                    Me.txtUserCode.Focus()
                    Exit Sub
                End If
                UserCode = " " & UserCode & " "
                If InStr(1, UserCode, " CREATE ", 1) > 0 Or InStr(1, UserCode, " DELETE ", 1) > 0 Or InStr(1, UserCode, " DROP ", 1) > 0 Or InStr(1, UserCode, " INSERT ", 1) > 1 Or InStr(1, UserCode, " TRACK ", 1) > 1 Or InStr(1, UserCode, " TRACE ", 1) > 1 Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do not use reserve words... ');", True)
                    Me.txtUserCode.Focus()
                    Exit Sub
                End If
                UserCode = TrimX(UserCode)
            Else
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Enter User Code... ');", True)
                Me.txtUserCode.Focus()
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
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Un-wanted Characters... ');", True)
                Me.txtUserCode.Focus()
                Exit Sub
            End If

            'SERVER VALIDATION FOR PASSWORD
            '*******************************************************************************************************
            Dim Hashed As Object
            Hashed = RKPass.Value
            If Hashed <> "" Then
                Hashed = RemoveQuotes(Hashed)
                If Hashed.ToString.Length > 100 Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Plz type correct password !');", True)
                    txtUserCode.Focus()
                    Exit Sub
                End If
                If (InStr(1, Hashed, "CREATE", 1) > 0) Or (InStr(1, Hashed, "INSERT", 1) > 0) Or (InStr(1, Hashed, "DROP", 1) > 0) Or (InStr(1, Hashed, "TRACK", 1) Or (InStr(1, Hashed, "TRACE", 1) > 0)) Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Plx type correct password !');", True)
                    txtUserCode.Focus()
                    Exit Sub
                End If
            Else
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Plx type correct password !');", True)
                txtUserCode.Focus()
                Exit Sub
            End If
            'check unwanted characters
            c = 0
            counter2 = 0
            For iloop = 1 To Len(Hashed)
                strcurrentchar = Mid(Hashed, iloop, 1)
                If c = 0 Then
                    If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                        c = c + 1
                        counter2 = 1
                    End If
                End If
            Next
            If counter2 = 1 Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Plz type correct password !');", True)
                txtUserCode.Focus()
                Exit Sub
            End If

            'check incorrect try for unsuccessful logn
            Dim Cmd9 As SqlCommand
            Dim da9 As SqlDataAdapter = Nothing
            Dim Ds9 As New DataSet
            Dim SQL9 As String
            SQL9 = "SELECT USER_CODE, LOGIN_DATE, SUCCESS FROM USER_LOGS WHERE (LOGIN_DATE='" & Format(Today, "MM/dd/yyyy") & "' AND USER_CODE ='" & Trim(UserCode) & "' AND SUCCESS ='N')"
            Cmd9 = New SqlCommand(SQL9, SqlConn)
            SqlConn.Open()
            da9 = New SqlDataAdapter(Cmd9)
            da9.Fill(Ds9, "TEMP")

            Dim myCount As Long
            myCount = Ds9.Tables(0).Rows.Count
            If myCount >= 3 Then
                da9.Dispose()
                da9.Dispose()
                UpdateUser_LOG2(1) 'sending flg value 1 to identify account locked in USER_LOGS
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Sorry...You can not try more today with wrong info...Try Again!!');", True)
                Me.Label1.Text = "Account Locked!.."
                Session("blnIsUserGood") = False

                TRModules.Visible = False
                TRAccordion.Visible = False
                Me.Accordion1.Enabled = False
                Me.Accordion1.Visible = False
                Exit Sub
            Else
                da9.Dispose()
                da9.Dispose()
            End If
            SqlConn.Close()


            'Response.Write("salt only " & (mySalt) & "<BR>")
            '***************************************************************************************************************

            Dim Cmd As SqlCommand
            Dim SQL As String
            SQL = "SELECT USER_PASSWORD, USER_NAME, USER_CODE, USER_TYPE, STATUS, LIB_CODE FROM USERS WHERE (USER_CODE='" & Trim(UserCode) & "') AND (STATUS = 'CU') "

            Cmd = New SqlCommand(SQL, SqlConn)
            If SqlConn.State = ConnectionState.Closed Then
                SqlConn.Open()
            End If
            dr = Cmd.ExecuteReader(CommandBehavior.CloseConnection)
            If dr.HasRows = True Then
                dr.Read()
                Dim myRSMemPassword As Object = Nothing
                myRSMemPassword = dr.Item("USER_PASSWORD").ToString
                myRSMemPassword = Session.Item("MySaltRKM") + myRSMemPassword
                myRSMemPassword = getMD5Hash(myRSMemPassword)
                If String.Compare(Hashed, myRSMemPassword, False) = 0 Then
                    If Trim(dr("STATUS").ToString) = "CU" Then
                        LoginUserName = dr.Item("USER_NAME").ToString
                        LibCode = dr.Item("LIB_CODE").ToString

                        Session.Item("LoggedUser") = UserCode
                        Session.Item("LoggedUserName") = LoginUserName
                        Session.Item("LoggedLibcode") = LibCode
                        Session("blnIsUserGood") = True

                        Dim authCookie, RandomNo As Object
                        RandomNo = RandomString(10)
                        authCookie = "AUTHCookie=" & RandomNo
                        Response.Cookies("AUTHCookie").Value = RandomNo
                        Response.Cookies("AUTHCookie").HttpOnly = True
                        'Response.Cookies("AUTHCookie").Secure = True
                        Session("authCookie") = RandomNo
                        dr.Close()
                        'call OPAC_LOG Update
                        UpdateUserLOG1()
                        ' Response.Redirect("Edit_LibDetails.aspx?LIB_CODE=" & libcode, False)
                        Me.Label2.Text = LoginUserName.ToString & "......"
                        TrLogOut.Visible = True
                        Response.Redirect("Default.aspx", True)
                    End If
                Else
                    dr.Close()
                    UpdateUser_LOG2(0) 'sending flg value 0 to identify invalid login
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Submit Correct Password!');", True)
                    trLable.Visible = True
                    Me.Label1.Text = "Invalid Account ! .."
                    Session("blnIsUserGood") = False
                    Me.Label2.Text = ""
                    txtPassword.Focus()
                End If
            Else
                dr.Close()
                UpdateUser_LOG2(0) 'sending flg value 0 to identify invalid login
                Me.Label1.Text = "User is invalid Try again !.."
                Session("blnIsUserGood") = False
                Me.Label2.Text = ""
                txtUserCode.Focus()
            End If
            dr.Close()
            SqlConn.Close()
            GetLibraryDetails()
        Catch s As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Login: Contact Administrator !');", True)
        Finally
            SqlConn.Close()
        End Try
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
            dtRow("ACTION") = "LoggedIn"
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
    Public Sub UpdateUser_LOG2(ByVal flg As Integer)

        'UPDATE THE MEMBER PROFILE    
        Dim SQL8 As String
        Dim Cmd8 As SqlCommand
        Dim da8 As SqlDataAdapter = Nothing
        Dim ds8 As New DataSet
        Dim CB8 As SqlCommandBuilder
        Dim dtRow8 As DataRow
        Try
            SQL8 = "SELECT * FROM USER_LOGS WHERE (USER_LOG_ID = '00')"
            Cmd8 = New SqlCommand(SQL8, SqlConn)
            SqlConn.Open()
            da8 = New SqlDataAdapter(Cmd8)
            CB8 = New SqlCommandBuilder(da8)
            da8.Fill(ds8, "LOGS")
            dtRow8 = ds8.Tables("LOGS").NewRow

            If Not String.IsNullOrEmpty(UserCode) Then
                dtRow8("USER_CODE") = UserCode.Trim
            End If
            Dim myIP As String = Nothing
            myIP = Trim(Request.UserHostAddress)
            If Not String.IsNullOrEmpty(myIP) Then
                dtRow8("IP") = myIP.Trim
            Else
                dtRow8("IP") = System.DBNull.Value
            End If

            Dim myDate As Date
            myDate = Format(Today, "MM/dd/yyyy")
            If Not String.IsNullOrEmpty(myDate) Then
                dtRow8("LOGIN_DATE") = myDate
            Else
                dtRow8("LOGIN_DATE") = System.DBNull.Value
            End If

            Dim myTime As String
            myTime = Now.ToLongTimeString
            If Not String.IsNullOrEmpty(myTime) Then
                dtRow8("LOGIN_TIME") = myTime
            Else
                dtRow8("LOGIN_TIME") = System.DBNull.Value
            End If
            Dim myPageVisited As String = Nothing
            myPageVisited = Request.ServerVariables("SCRIPT_NAME").ToString()
            If Not String.IsNullOrEmpty(myPageVisited) Then
                dtRow8("PAGE_VISITED") = myPageVisited.Trim
            Else
                dtRow8("PAGE_VISITED") = System.DBNull.Value
            End If
            dtRow8("SUCCESS") = "N"
            If flg = 0 Then
                dtRow8("ACTION") = "Failure"
            Else
                dtRow8("ACTION") = "Account locked for 24hrs"
            End If

            dtRow8("UI_TYPE") = "DataEntry"

            ds8.Tables("LOGS").Rows.Add(dtRow8)
            da8.Update(ds8, "LOGS")
            da8.Dispose()
            ds8.Dispose()
        Catch EX As Exception
            Response.Write(EX.Message)
        Finally
            da8.Dispose()
            ds8.Dispose()
            SqlConn.Close()
        End Try
    End Sub
    'check user permission on login
    Public Sub GetUserPermission()
        Dim Command As SqlCommand = Nothing
        Dim drUsers As SqlDataReader = Nothing
        Try
            If UserCode <> "" Then
                Dim SQL As String
                SQL = "SELECT * FROM USERS WHERE (USER_CODE='" & Trim(UserCode) & "') AND (STATUS = 'CU')"
                Command = New SqlCommand(SQL, SqlConn)
                SqlConn.Open()

                drUsers = Command.ExecuteReader(CommandBehavior.CloseConnection)

                If drUsers.HasRows = True Then
                    drUsers.Read()
                    If drUsers.Item("USER_TYPE").ToString = "ADMIN" Then ' db admin
                        TRModules.Visible = True
                        TRAccordion.Visible = True
                        Me.Accordion1.Visible = True
                        Me.Accordion1.Enabled = True
                        DBAdminPane.Enabled = True


                        LibAdminPane.Visible = False
                        MasterPane.Visible = False
                        AcqPane.Visible = False
                        CatPane.Visible = False
                        CirPane.Visible = False
                        SerPane.Visible = False
                        ArtPane.Visible = False
                        BudgPane.Visible = False
                        SearchPane.Enabled = True
                    ElseIf drUsers.Item("USER_TYPE").ToString = "SUSER" Then ' for library admin
                        TRModules.Visible = True
                        TRAccordion.Visible = True
                        Me.Accordion1.Visible = True
                        Me.Accordion1.Enabled = True

                        DBAdminPane.Enabled = False
                        DBAdminPane.Visible = False
                        LibAdminPane.Visible = True
                        LibAdminPane.Enabled = True



                        MasterPane.Visible = True
                        MasterPane.Enabled = True
                        AcqPane.Visible = True
                        AcqPane.Enabled = True
                        CatPane.Visible = True
                        CatPane.Enabled = True
                        CirPane.Visible = True
                        CirPane.Enabled = True
                        SerPane.Visible = True
                        SerPane.Enabled = True
                        ArtPane.Visible = True
                        ArtPane.Enabled = True
                        BudgPane.Visible = True
                        BudgPane.Enabled = True
                        SearchPane.Enabled = True
                        SearchPane.Enabled = True
                    Else 'general user
                        If drUsers.Item("USER_ADMIN").ToString = "Y" Then
                            LibAdminPane.Visible = True
                            LibAdminPane.Enabled = True
                        Else
                            LibAdminPane.Visible = False
                            LibAdminPane.Enabled = False
                        End If
                        If drUsers.Item("USER_ACQ").ToString = "Y" Then
                            AcqPane.Visible = True
                            AcqPane.Enabled = True
                        Else
                            AcqPane.Visible = False
                            AcqPane.Enabled = False
                        End If
                        If drUsers.Item("USER_CAT").ToString = "Y" Then
                            CatPane.Visible = True
                            CatPane.Enabled = True
                        Else
                            CatPane.Visible = False
                            CatPane.Enabled = False
                        End If
                        If drUsers.Item("USER_CIR").ToString = "Y" Then
                            CirPane.Visible = True
                            CirPane.Enabled = True
                        Else
                            CirPane.Visible = False
                            CirPane.Enabled = False
                        End If
                        If drUsers.Item("USER_SER").ToString = "Y" Then
                            SerPane.Visible = True
                            SerPane.Enabled = True
                        Else
                            SerPane.Visible = False
                            SerPane.Enabled = False
                        End If
                        If drUsers.Item("USER_ART").ToString = "Y" Then
                            ArtPane.Visible = True
                            ArtPane.Enabled = True
                        Else
                            ArtPane.Visible = False
                            ArtPane.Enabled = False
                        End If
                        If drUsers.Item("USER_BUDGET").ToString = "Y" Then
                            BudgPane.Visible = True
                            BudgPane.Enabled = True
                        Else
                            BudgPane.Visible = False
                            BudgPane.Enabled = False
                        End If
                        If drUsers.Item("USER_SEARCH").ToString = "Y" Then
                            SearchPane.Visible = True
                            SearchPane.Enabled = True
                        Else
                            SearchPane.Visible = False
                            SearchPane.Enabled = False
                        End If
                        DBAdminPane.Visible = False
                        DBAdminPane.Enabled = False
                        TRModules.Visible = True
                        TRAccordion.Visible = True
                        Me.Accordion1.Enabled = True
                        Me.Accordion1.Visible = True
                    End If
                End If
                If drUsers.Item("USER_PHOTO").ToString <> "" Then
                    TrUserPhoto.Visible = True
                    Dim strURL As String = "~/Account/User_GetPhoto.aspx?USER_CODE=" & UserCode & ""
                    Image3.ImageUrl = strURL
                Else
                    Image3.Visible = False
                    TrUserPhoto.Visible = False
                End If
                drUsers.Dispose()
                SqlConn.Close()
            End If
        Catch s As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('User Permiss Error: Contact Administrator !');", True)
        Finally
            drUsers.Dispose()
            SqlConn.Close()
        End Try

    End Sub
    'get last login info
    Public Sub GetLastLogin()
        Dim Command As SqlCommand
        Dim drUsers As SqlDataReader
        Try
            If UserCode <> "" Then
                Dim SQL As String
                SQL = "SELECT TOP 1 * FROM  ( SELECT TOP 2 * FROM USER_LOGS WHERE (USER_CODE='" & Trim(UserCode) & "') AND (SUCCESS = 'Y') AND (ACTION ='LoggedIn') ORDER BY USER_LOG_ID DESC) x  ORDER BY USER_LOG_ID  "

                Command = New SqlCommand(SQL, SqlConn)
                SqlConn.Open()

                drUsers = Command.ExecuteReader(CommandBehavior.CloseConnection)

                If drUsers.HasRows = True Then
                    drUsers.Read()
                    If drUsers.Item("IP").ToString <> "" Then
                        myLastIP = drUsers.Item("IP").ToString
                    Else
                        myLastIP = ""
                    End If
                    If drUsers.Item("LOGIN_DATE").ToString <> "" Then
                        myLastDate = Format(drUsers.Item("LOGIN_DATE"), "dd/MM/yyyy").ToString
                    Else
                        myLastDate = ""
                    End If
                    If drUsers.Item("LOGIN_TIME").ToString <> "" Then
                        myLastTime = drUsers.Item("LOGIN_TIME").ToString
                    Else
                        myLastTime = ""
                    End If
                    Me.Label4.Text = "Last Login: IP- " & myLastIP & ", Date - " & myLastDate & ", Last time - " & myLastTime
                Else
                    Me.Label4.Text = ""
                End If
                drUsers.Dispose()
                SqlConn.Close()
            End If
        Catch s As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Get Last Login Error: Contact Administrator !');", True)
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'get last login info
    Public Sub GetClusterDetails()
        Dim Command As SqlCommand
        Dim drCluster As SqlDataReader
        Try
            Dim SQL As String
            SQL = "SELECT  * FROM  CLUSTER  "

            Command = New SqlCommand(SQL, SqlConn)
            SqlConn.Open()

            drCluster = Command.ExecuteReader(CommandBehavior.CloseConnection)

            If drCluster.HasRows = True Then
                drCluster.Read()
                If drCluster.Item("NAME").ToString <> "" Then
                    CLUSTER_NAME = drCluster.Item("NAME").ToString
                Else
                    CLUSTER_NAME = ""
                End If

                drCluster.Dispose()
                SqlConn.Close()
            End If
            ' End If
        Catch s As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Get Cluster Login Error: Contact Administrator !');", True)
        Finally
            SqlConn.Close()
        End Try
    End Sub
    Public Sub GetLibraryDetails()
        Dim Command As SqlCommand
        Dim drLibrary As SqlDataReader
        Try
            If UserCode <> "" And LibCode <> "" Then
                Dim SQL As String
                SQL = "SELECT  * FROM  LIBRARIES WHERE (LIB_CODE ='" & Trim(LibCode) & "')  "

                Command = New SqlCommand(SQL, SqlConn)
                SqlConn.Open()

                drLibrary = Command.ExecuteReader(CommandBehavior.CloseConnection)
                If drLibrary.HasRows = True Then
                    drLibrary.Read()
                    If drLibrary.Item("LIB_NAME").ToString <> "" Then
                        LIBRARY_NAME = drLibrary.Item("LIB_NAME").ToString
                        If drLibrary.Item("LIB_NAME").ToString <> "" Then
                            LIBRARY_NAME = LIBRARY_NAME & ", " & drLibrary.Item("PARENT_BODY").ToString
                        End If
                        If drLibrary.Item("LIB_CITY").ToString <> "" Then
                            LIBRARY_NAME = LIBRARY_NAME & ", " & drLibrary.Item("LIB_CITY").ToString
                        End If
                        If drLibrary.Item("PARENT_BODY").ToString <> "" Then
                            RKLibraryParent = drLibrary.Item("PARENT_BODY").ToString
                        End If

                        If drLibrary.Item("LIB_NAME").ToString <> "" Then
                            RKLibraryName = drLibrary.Item("LIB_NAME").ToString
                        End If

                        If drLibrary.Item("LIB_ADDRESS").ToString <> "" Then
                            RKLibraryAddress = drLibrary.Item("LIB_ADDRESS").ToString
                        End If

                        If drLibrary.Item("LIB_STATE").ToString <> "" Then
                            ' LIBRARY_NAME = LIBRARY_NAME & ", " & drLibrary.Item("LIB_STATE").ToString
                        End If
                    Else
                        LIBRARY_NAME = ""
                    End If

                    drLibrary.Dispose()
                    SqlConn.Close()
                End If
            End If
        Catch s As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Cluster Login Error: Contact Administrator !');", True)
        Finally
            SqlConn.Close()
        End Try
    End Sub

   
End Class