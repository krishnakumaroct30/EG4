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
Imports EG4.PopulateCombo
Public Class OPAC
    Inherits System.Web.UI.MasterPage
    Dim ds As New DataSet
    Public CatCount As Long = 0
    Public HoldCount As Long = 0
    Public LibCount As Integer = 0
    Public mySalt As Object
    Dim myIntroMaster As String = Nothing
    Dim LIB_EMAIL As String = Nothing
    Dim LIB_FAX As String = Nothing
    Dim LIB_PHONE As String = Nothing
    Dim CONTACT_PERSON As String = Nothing
    Dim PARENT_ORG As String = Nothing
    Dim CLUSTER_ADDRESS As String = Nothing
    Dim CLUSTER_NAME2 As String = Nothing

    Dim LIB_NAME2 As String = Nothing
    Dim LIB_CITY2 As String = Nothing
    Dim PARENT_BODY2 As String = Nothing
    Dim LIB_ADD2 As String = Nothing
    Dim LIB_INCHARGE2 As String = Nothing


    Dim dr As SqlDataReader
    Dim myLastIP, myLastDate, myLastTime As Object
    Public CLUSTER_NAME As String = Nothing
    Public LIBRARY_NAME As String = Nothing
    Public LIBRARY_ADD As String = Nothing
    Public LoggedinMemberName As Object = Nothing
    Dim LoggedInMemberNo As String = Nothing
    Dim LoggedLibraryCode As String = Nothing
    Public LoggedMemID As Integer = Nothing
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Page.Master.IsPostBack = False Then
            mySalt = RandomString(10)
            Session.Item("MySaltRKM1") = mySalt
            LoggedInMemberNo = Session.Item("LoggedMemberNo")
            LoggedLibraryCode = Session("LoggedLibCode")
            LoggedMemID = Session.Item("LoggedMemID")
            If Session.Item("LoggedMemberName") <> "" Then
                Label2.Text = "Login: " & Session.Item("LoggedMemberName") & "(" & Session.Item("LoggedMemberNo") & ")"
                GetLastLogin()
            End If
        Else
            LoggedInMemberNo = Session.Item("LoggedMemberNo")
            LoggedLibraryCode = Session("LoggedLibCode")
            LoggedMemID = Session.Item("LoggedMemID")
            mySalt = Session.Item("MySaltRKM1")
            If Session.Item("LoggedMemberName") <> "" Then
                Label2.Text = "Login: " & Session.Item("LoggedMemberName") & "(" & Session.Item("LoggedMemberNo") & ")"
            End If
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Page.Header.DataBind()
            If Not SConnection() = True Then
                FGP.Visible = False
                TrLogOut.Visible = False
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Database Connection is Lost mm!');", True)
            Else
                GetClusterDetails()
                GetLibraryDetails()
                If Page.Master.IsPostBack = False Then
                    Session.Item("IsThisU") = True 'this
                    txtLibCode.Focus() 'this
                End If

                If Label2.Text <> "" Then
                    TrLogOut.Visible = True
                    Tdx.Visible = False
                    trcaptcha.Visible = False
                Else
                    TrLogOut.Visible = False
                End If

                If LoggedInMemberNo = "" Then
                    TrMemPhoto.Visible = False
                    TR_EDITPROFILE.Visible = False
                    TR_CHANGEPASSWORD.Visible = False
                    trMemLogin.Visible = True
                    TR_LIBCODE.Visible = True
                    trUser.Visible = True
                    trPassword.Visible = True
                    trButton.Visible = True
                    trLable.Visible = True
                    trcaptcha.Visible = True
                Else
                    TrMemPhoto.Visible = True
                    TR_EDITPROFILE.Visible = True
                    TR_CHANGEPASSWORD.Visible = True
                    trMemLogin.Visible = False
                    TR_LIBCODE.Visible = False
                    trUser.Visible = False
                    trPassword.Visible = False
                    trButton.Visible = False
                    trLable.Visible = False
                    trcaptcha.Visible = False
                    GetMemberPhoto()
                    If Page.Master.IsPostBack = False Then
                        AddChildNodes()
                    End If

                End If
                GetCatsStatistics()
                GetHoldStatistics()
                GetLibStatistics()
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Database Connection is Lost rk1!');", True)
        End Try
    End Sub
    Public Sub AddChildNodes()

        'TreeView1.Nodes.Clear()

        Dim ChildNode7 As New TreeNode
        ChildNode7.Text = "Circulation Transactions"
        ChildNode7.Value = "Transactions"
        ChildNode7.NavigateUrl = "Default.aspx?PARAM=Y"
        TreeView1.Nodes(0).ChildNodes.Add(ChildNode7)


        Dim ChildNode1 As New TreeNode
        ChildNode1.Text = "Objectives"
        ChildNode1.Value = "Objectives"
        TreeView1.Nodes(0).ChildNodes.Add(ChildNode1)

        Dim ChildNode2 As New TreeNode
        ChildNode2.Text = "History"
        ChildNode2.Value = "History"
        TreeView1.Nodes(0).ChildNodes.Add(ChildNode2)

        Dim ChildNode3 As New TreeNode
        ChildNode3.Text = "services"
        ChildNode3.Value = "Services"
        TreeView1.Nodes(0).ChildNodes.Add(ChildNode3)

        Dim ChildNode4 As New TreeNode
        ChildNode4.Text = "Library Rules"
        ChildNode4.Value = "Rules"
        TreeView1.Nodes(0).ChildNodes.Add(ChildNode4)

        Dim ChildNode5 As New TreeNode
        ChildNode5.Text = "Library Committee"
        ChildNode5.Value = "Committee"
        TreeView1.Nodes(0).ChildNodes.Add(ChildNode5)

        Dim ChildNode6 As New TreeNode
        ChildNode6.Text = "Photo Gallery"
        ChildNode6.Value = "Photo"
        TreeView1.Nodes(0).ChildNodes.Add(ChildNode6)

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
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert(' Get Cat Stat Error: Contact Administrator !');", True)
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
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Get Hold Stat Error: Contact Administrator !');", True)
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
                If LibCount = 1 Then
                    Command = New SqlCommand("SELECT  LIB_CODE FROM LIBRARIES ", SqlConn)
                    SqlConn.Open()
                    drLib = Command.ExecuteReader(CommandBehavior.CloseConnection)
                    drLib.Read()
                    txtLibCode.Text = drLib.Item("LIB_CODE").ToString
                    txtLibCode.Enabled = False
                    txtMemberNo.Focus()
                End If


            End If
        Catch s As Exception
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Get Lib Stat Error: Contact Administrator !');", True)
        Finally
            Command.Dispose()
            drLib.Dispose()
            SqlConn.Close()
        End Try
    End Sub
    Protected Sub bttn_LogIn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles bttn_LogIn.Click
        Try
            paneSelectedIndex = 0
            'Server Validation for User Code

            'captcha validation
            Dim loginCAPTCHA As WebControlCaptcha.CaptchaControl = Me.CAPTCHA

            If loginCAPTCHA.UserValidated = False Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Captcha Value is not Valid!');", True)
                Me.CAPTCHA.Focus()
                Exit Sub
            End If


            Dim c As Integer = 0
            Dim counter1 As Integer = 0
            Dim counter2 As Integer = 0
            Dim counter3 As Integer = 0
            Dim strcurrentchar As Object = ""
            LoggedInMemberNo = TrimX(txtMemberNo.Text)
            LoggedInMemberNo = UCase(LoggedInMemberNo)
            LoggedLibraryCode = TrimX(txtLibCode.Text)

            If Not String.IsNullOrEmpty(LoggedInMemberNo) Then
                LoggedInMemberNo = RemoveQuotes(LoggedInMemberNo)
                If LoggedInMemberNo.Length > 12 Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert(' Length of User Code is not Proper... ');", True)
                    Me.txtMemberNo.Focus()
                    Exit Sub
                End If
                LoggedInMemberNo = " " & LoggedInMemberNo & " "
                If InStr(1, LoggedInMemberNo, " CREATE ", 1) > 0 Or InStr(1, LoggedInMemberNo, " DELETE ", 1) > 0 Or InStr(1, LoggedInMemberNo, " DROP ", 1) > 0 Or InStr(1, LoggedInMemberNo, " INSERT ", 1) > 1 Or InStr(1, LoggedInMemberNo, " TRACK ", 1) > 1 Or InStr(1, LoggedInMemberNo, " TRACE ", 1) > 1 Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert(' Do not use reserve words... ');", True)
                    Me.txtMemberNo.Focus()
                    Exit Sub
                End If
                LoggedInMemberNo = TrimX(LoggedInMemberNo)
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert(' Enter User Code... ');", True)
                Me.txtMemberNo.Focus()
                Exit Sub
            End If
            'check unwanted characters
            c = 0
            counter1 = 0
            For iloop = 1 To Len(LoggedInMemberNo)
                strcurrentchar = Mid(LoggedInMemberNo, iloop, 1)
                If c = 0 Then
                    If Not InStr("';~!@#$^&*-_=+|[]{}?,<>=()%""", strcurrentchar) <= 0 Then
                        c = c + 1
                        counter1 = 1
                    End If
                End If
            Next
            If counter1 = 1 Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert(' Do Not Use Un-wanted Characters... ');", True)
                Me.txtMemberNo.Focus()
                Exit Sub
            End If

            'Logged Library Code
            If Not String.IsNullOrEmpty(LoggedLibraryCode) Then
                LoggedLibraryCode = RemoveQuotes(LoggedLibraryCode)
                If LoggedLibraryCode.Length > 12 Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert(' Length of User Code is not Proper... ');", True)
                    Me.txtLibCode.Focus()
                    Exit Sub
                End If
                LoggedLibraryCode = " " & LoggedLibraryCode & " "
                If InStr(1, LoggedLibraryCode, " CREATE ", 1) > 0 Or InStr(1, LoggedLibraryCode, " DELETE ", 1) > 0 Or InStr(1, LoggedLibraryCode, " DROP ", 1) > 0 Or InStr(1, LoggedLibraryCode, " INSERT ", 1) > 1 Or InStr(1, LoggedLibraryCode, " TRACK ", 1) > 1 Or InStr(1, LoggedLibraryCode, " TRACE ", 1) > 1 Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert(' Do not use reserve words... ');", True)
                    Me.txtLibCode.Focus()
                    Exit Sub
                End If
                LoggedLibraryCode = TrimX(LoggedLibraryCode)
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert(' Enter User Code... ');", True)
                Me.txtLibCode.Focus()
                Exit Sub
            End If
            'check unwanted characters
            c = 0
            counter2 = 0
            For iloop = 1 To Len(LoggedLibraryCode)
                strcurrentchar = Mid(LoggedLibraryCode, iloop, 1)
                If c = 0 Then
                    If Not InStr("';~!@#$^&*-_=+|[]{}?,<>=()%""", strcurrentchar) <= 0 Then
                        c = c + 1
                        counter2 = 1
                    End If
                End If
            Next
            If counter2 = 1 Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert(' Do Not Use Un-wanted Characters... ');", True)
                Me.txtLibCode.Focus()
                Exit Sub
            End If

            'SERVER VALIDATION FOR PASSWORD
            '*******************************************************************************************************
            Dim Hashed As Object
            Hashed = RKPass2.Value
            If Hashed <> "" Then
                Hashed = RemoveQuotes(Hashed)
                If Hashed.ToString.Length > 100 Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert(' Plz type correct password !');", True)
                    txtPassword.Focus()
                    Exit Sub
                End If
                If (InStr(1, Hashed, "CREATE", 1) > 0) Or (InStr(1, Hashed, "INSERT", 1) > 0) Or (InStr(1, Hashed, "DROP", 1) > 0) Or (InStr(1, Hashed, "TRACK", 1) Or (InStr(1, Hashed, "TRACE", 1) > 0)) Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Plx type correct password !');", True)
                    txtPassword.Focus()
                    Exit Sub
                End If
            Else
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Plx type correct password !');", True)
                txtPassword.Focus()
                Exit Sub
            End If
            'check unwanted characters
            c = 0
            counter3 = 0
            For iloop = 1 To Len(Hashed)
                strcurrentchar = Mid(Hashed, iloop, 1)
                If c = 0 Then
                    If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                        c = c + 1
                        counter3 = 1
                    End If
                End If
            Next
            If counter3 = 1 Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Plz type correct password !');", True)
                txtMemberNo.Focus()
                Exit Sub
            End If

            'check incorrect try for unsuccessful logn
            Dim Cmd9 As SqlCommand
            Dim da9 As SqlDataAdapter = Nothing
            Dim Ds9 As New DataSet
            Dim SQL9 As String
            SQL9 = "SELECT MEM_NO FROM USER_LOGS WHERE (LOGIN_DATE='" & Format(Today, "MM/dd/yyyy") & "' AND MEM_NO ='" & Trim(LoggedInMemberNo) & "' AND LIB_CODE ='" & Trim(LoggedLibraryCode) & "' AND SUCCESS ='N')"
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
                TRModules.Visible = True

                SqlConn.Close()
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
            SQL = "SELECT MEMB_PASSWORD, MEM_ID, MEM_NO, MEM_NAME, MEM_STATUS, LIB_CODE FROM MEMBERSHIPS WHERE (MEM_NO='" & Trim(LoggedInMemberNo) & "' AND LIB_CODE='" & Trim(LoggedLibraryCode) & "' AND MEM_STATUS = 'CU') "

            Cmd = New SqlCommand(SQL, SqlConn)
            If SqlConn.State = ConnectionState.Closed Then
                SqlConn.Open()
            End If
            dr = Cmd.ExecuteReader(CommandBehavior.CloseConnection)
            If dr.HasRows = True Then
                dr.Read()

                Dim myRSMemPassword As Object = Nothing
                myRSMemPassword = dr.Item("MEMB_PASSWORD").ToString
                myRSMemPassword = Session.Item("MySaltRKM1") + myRSMemPassword
                myRSMemPassword = getMD5Hash(myRSMemPassword)
                If String.Compare(Hashed, myRSMemPassword, False) = 0 Then
                    If Trim(dr("MEM_STATUS").ToString) = "CU" Then
                        LoggedinMemberName = dr.Item("MEM_NAME").ToString
                        LoggedLibraryCode = dr.Item("LIB_CODE").ToString
                        LoggedMemID = dr.Item("MEM_ID").ToString

                        Session.Item("LoggedMemberNo") = LoggedInMemberNo
                        Session.Item("LoggedMemberName") = LoggedinMemberName
                        Session.Item("LoggedLibCode") = LoggedLibraryCode
                        Session.Item("LoggedMemID") = LoggedMemID
                        Session("blnIsUserGood") = True

                        Dim authCookie, RandomNo As Object
                        RandomNo = RandomString(10)
                        authCookie = "AUTHCookie=" & RandomNo
                        Response.Cookies("AUTHCookie").Value = RandomNo
                        Response.Cookies("AUTHCookie").HttpOnly = True
                        'Response.Cookies("AUTHCookie").Secure = True
                        Session("authCookie") = RandomNo
                        dr.Close()
                        SqlConn.Close()
                        'call OPAC_LOG Update
                        UpdateUserLOG1()
                        Me.Label2.Text = LoggedinMemberName.ToString & "......"
                        TrLogOut.Visible = True
                        Response.Redirect("~/OPAC/Default.aspx", True)
                        Response.End()
                    End If
                Else
                    dr.Close()
                    SqlConn.Close()
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
                SqlConn.Close()
                UpdateUser_LOG2(0) 'sending flg value 0 to identify invalid login
                Me.Label1.Text = "User is invalid Try again !.."
                Session("blnIsUserGood") = False
                Me.Label2.Text = ""
                txtMemberNo.Focus()
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

            If Not String.IsNullOrEmpty(LoggedInMemberNo) Then
                dtRow("MEM_NO") = LoggedInMemberNo.Trim
            End If
            If Not String.IsNullOrEmpty(LoggedLibraryCode) Then
                dtRow("LIB_CODE") = LoggedLibraryCode.Trim
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
            dtRow("UI_TYPE") = "OPAC"

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

            If Not String.IsNullOrEmpty(LoggedInMemberNo) Then
                dtRow8("MEM_NO") = LoggedInMemberNo.Trim
            End If
            If Not String.IsNullOrEmpty(LoggedLibraryCode) Then
                dtRow8("LIB_CODE") = LoggedLibraryCode.Trim
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

            dtRow8("UI_TYPE") = "OPAC"

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
    Public Sub GetMemberPhoto()
        Dim Command As SqlCommand = Nothing
        Dim drMembers As SqlDataReader = Nothing
        Try
            If LoggedInMemberNo <> "" Then
                Dim SQL As String
                SQL = "SELECT MEM_ID, MEM_NO, PHOTO, LIB_CODE FROM MEMBERSHIPS WHERE (MEM_NO='" & Trim(LoggedInMemberNo) & "') AND (MEM_STATUS = 'CU') AND (LIB_CODE ='" & Trim(LoggedLibraryCode) & "'); "
                Command = New SqlCommand(SQL, SqlConn)
                SqlConn.Open()

                drMembers = Command.ExecuteReader(CommandBehavior.CloseConnection)

                If drMembers.HasRows = True Then
                    drMembers.Read()
                    If drMembers.Item("PHOTO").ToString <> "" Then
                        TrMemPhoto.Visible = True
                        Dim strURL As String = "Mem_GetPhoto.aspx?MEM_ID=" & drMembers.Item("MEM_ID").ToString & ""
                        Image3.ImageUrl = strURL
                    Else
                        Image3.Visible = False
                        TrMemPhoto.Visible = False
                    End If
                End If
                drMembers.Dispose()
                SqlConn.Close()
            End If
        Catch s As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('User Permiss Error: Contact Administrator !');", True)
        Finally
            drMembers.Dispose()
            SqlConn.Close()
        End Try
    End Sub
    'get last login info
    Public Sub GetLastLogin()
        Dim Command As SqlCommand
        Dim drUsers As SqlDataReader
        Try
            If LoggedInMemberNo <> "" Then
                Dim SQL As String
                SQL = "SELECT TOP 1 * FROM  ( SELECT TOP 2 * FROM USER_LOGS WHERE (MEM_NO='" & Trim(LoggedInMemberNo) & "') AND (LIB_CODE='" & Trim(LoggedLibraryCode) & "') AND (SUCCESS = 'Y') AND (ACTION ='LoggedIn') ORDER BY USER_LOG_ID DESC) x  ORDER BY USER_LOG_ID  "

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
                    CLUSTER_NAME2 = drCluster.Item("NAME").ToString
                Else
                    CLUSTER_NAME = ""
                    CLUSTER_NAME2 = ""
                End If

                If drCluster.Item("REMARKS").ToString <> "" Then
                    myIntroMaster = drCluster.Item("REMARKS").ToString.Replace(Environment.NewLine, "<br />")
                End If

                If LoggedInMemberNo <> "" And LoggedLibraryCode <> "" Then
                    CLUSTER_ADDRESS = ""
                Else
                    CLUSTER_NAME = ""
                    Lbl_LibName.Text = "Online Library Catalogs"
                    Lbl_Parent.Text = drCluster.Item("NAME").ToString
                    Lbl_City.Text = drCluster.Item("ADDRESS").ToString
                    CLUSTER_ADDRESS = Lbl_City.Text
                End If
                drCluster.Dispose()
                SqlConn.Close()
            Else
                CLUSTER_NAME = ""
            End If

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
            If LoggedInMemberNo <> "" And LoggedLibraryCode <> "" Then
                Dim SQL As String
                SQL = "SELECT  * FROM  LIBRARIES WHERE (LIB_CODE ='" & Trim(LoggedLibraryCode) & "')  "

                Command = New SqlCommand(SQL, SqlConn)
                SqlConn.Open()

                drLibrary = Command.ExecuteReader(CommandBehavior.CloseConnection)
                If drLibrary.HasRows = True Then
                    drLibrary.Read()
                    If drLibrary.Item("LIB_NAME").ToString <> "" Then
                        Lbl_LibName.Text = "   " & drLibrary.Item("LIB_NAME").ToString
                        LIBRARY_NAME = drLibrary.Item("LIB_NAME").ToString
                        RKLibraryName = drLibrary.Item("LIB_NAME").ToString
                    Else
                        LIBRARY_NAME = ""
                    End If

                    If drLibrary.Item("PARENT_BODY").ToString <> "" Then
                        Lbl_Parent.Text = "   " & drLibrary.Item("PARENT_BODY").ToString
                        PARENT_ORG = drLibrary.Item("PARENT_BODY").ToString
                        RKLibraryParent = drLibrary.Item("PARENT_BODY").ToString
                    Else
                        PARENT_ORG = ""
                    End If

                    If drLibrary.Item("LIB_INCHARGE").ToString <> "" Then
                        CONTACT_PERSON = drLibrary.Item("LIB_INCHARGE").ToString
                    Else
                        CONTACT_PERSON = ""
                    End If
                   
                    If drLibrary.Item("LIB_ADDRESS").ToString <> "" Then
                        Lbl_City.Text = "   " & drLibrary.Item("LIB_ADDRESS").ToString
                        LIBRARY_ADD = drLibrary.Item("LIB_ADDRESS").ToString
                        RKLibraryAddress = drLibrary.Item("LIB_ADDRESS").ToString
                    Else
                        LIBRARY_ADD = ""
                    End If

                    If drLibrary.Item("LIB_FAX").ToString <> "" Then
                        LIB_FAX = drLibrary.Item("LIB_FAX").ToString
                    Else
                        LIB_FAX = ""
                    End If

                    If drLibrary.Item("LIB_PHONE").ToString <> "" Then
                        LIB_PHONE = drLibrary.Item("LIB_PHONE").ToString
                    Else
                        LIB_PHONE = ""
                    End If

                    If drLibrary.Item("LIB_EMAIL").ToString <> "" Then
                        LIB_EMAIL = drLibrary.Item("LIB_EMAIL").ToString
                    Else
                        LIB_EMAIL = ""
                    End If

                    If drLibrary.Item("LIB_INTRO").ToString <> "" Then
                        myIntroMaster = drLibrary.Item("LIB_INTRO").ToString.Replace(Environment.NewLine, "<br />")
                    End If


                    If drLibrary.Item("LIB_INCHARGE2").ToString <> "" Then
                        LIB_INCHARGE2 = drLibrary.Item("LIB_INCHARGE2").ToString
                    Else
                        LIB_INCHARGE2 = ""
                    End If

                    If drLibrary.Item("LIB_NAME2").ToString <> "" Then
                        LIB_NAME2 = drLibrary.Item("LIB_NAME2").ToString
                    Else
                        LIB_NAME2 = ""
                    End If

                    If drLibrary.Item("PARENT_BODY2").ToString <> "" Then
                        PARENT_BODY2 = drLibrary.Item("PARENT_BODY2").ToString
                    Else
                        PARENT_BODY2 = ""
                    End If

                    If drLibrary.Item("LIB_ADDRESS2").ToString <> "" Then
                        LIB_ADD2 = drLibrary.Item("LIB_ADDRESS2").ToString
                    Else
                        LIB_ADD2 = ""
                    End If

                Else
                    LIBRARY_NAME = ""
                    Lbl_LibName.Text = ""
                    Lbl_City.Text = ""
                    Lbl_Parent.Text = ""
                    LIB_EMAIL = ""
                    LIB_FAX = ""
                    LIB_PHONE = ""
                    LIBRARY_ADD = ""
                    PARENT_ORG = ""
                    CONTACT_PERSON = ""

                    LIB_NAME2 = Nothing
                    LIB_CITY2 = Nothing
                    PARENT_BODY2 = Nothing
                    LIB_ADD2 = Nothing
                    LIB_INCHARGE2 = Nothing

                End If

                drLibrary.Dispose()
                SqlConn.Close()
            End If

        Catch s As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Cluster Login Error: Contact Administrator !');", True)
        Finally
            SqlConn.Close()
        End Try
    End Sub
    Protected Sub TreeView1_TreeNodeExpanded(ByVal sender As Object, ByVal e As TreeNodeEventArgs)
        For Each tn As TreeNode In TreeView1.Nodes
            If tn.Text <> "General Information" Then
                If tn IsNot e.Node Then
                    tn.Collapse()
                End If
            End If
        Next tn
    End Sub
    'click event of tree node
    Protected Sub TreeView1_SelectedNodeChanged(ByVal sender As Object, ByVal e As EventArgs) Handles TreeView1.SelectedNodeChanged
        'Dim content As ContentPlaceHolder
        'content = Page.Master.FindControl("MainContent")

        If TreeView1.SelectedNode.Value = "About" Then
            RKTreeviewValue = "About"
            Response.Redirect("Default.aspx", True)
        End If
        If TreeView1.SelectedNode.Value = "Contact" Then
            RKTreeviewValue = "Contact"
            Response.Redirect("Default.aspx", True)
        End If

    End Sub

   

End Class