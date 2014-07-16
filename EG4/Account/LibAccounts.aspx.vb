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
Public Class LibAccounts
    Inherits System.Web.UI.Page
    Dim index As Integer = 0
    Public Srno As Integer = 0
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
                    End If
                    Dim CreateSUserButton As Object 'System.Web.UI.WebControls.Button
                    CreateSUserButton = Master.FindControl("Accordion1").FindControl("DBAdminPane").FindControl("Adm_CreateLibAccount_bttn")
                    CreateSUserButton.ForeColor = Drawing.Color.Red
                    myPaneName = "DBAdminPane" ' paneSelectedIndex = 0
                    Me.txt_LibCode.Focus()
                End If
            Else
                Response.Redirect("~/Default.aspx", False)
            End If
        Catch ex As Exception
            Label6.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    Private Sub LibAccounts_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        If Me.txt_LibCode.Enabled = False Then
            Me.txt_LibName.Focus()
        Else
            Me.txt_LibCode.Focus()
        End If
    End Sub
    Public Sub AllLibraries()
        Dim Command As SqlCommand = Nothing
        Dim dt As DataTable = Nothing
        Try
            Command = New SqlCommand("SELECT  LIB_CODE, LIB_NAME FROM LIBRARIES ", SqlConn)
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
                Me.Drop_Libraries.DataBind()
                Me.Drop_Libraries.Items.Clear()
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
            SqlConn.Close()
        End Try
    End Sub
    'click event of drop-down
    Protected Sub DropDownList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DropDownList1.SelectedIndexChanged
        If DropDownList1.SelectedValue = "B" Then
            AllLibraries()
        Else
            Me.Drop_Libraries.Items.Clear()
        End If
    End Sub
    'save admin account
    Protected Sub Submit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles bttn_Save.Click
        If IsPostBack = True Then
            Dim iloop As Integer
            Dim strcurrentchar As Object
            Dim c As Integer
            Dim counter1, counter2, counter3, counter4, Counter5, counter6, counter8, counter9 As Integer
            Try
                'Server Validation for Lib Code
                Dim NewLibCode As String = Nothing
                NewLibCode = TrimX(txt_LibCode.Text)
                NewLibCode = UCase(NewLibCode)
                If Not String.IsNullOrEmpty(NewLibCode) Then
                    NewLibCode = RemoveQuotes(NewLibCode)
                    If NewLibCode.Length > 12 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Length of User Code is not Proper... ');", True)
                        Me.txt_LibCode.Focus()
                        Exit Sub
                    End If
                    NewLibCode = " " & NewLibCode & " "
                    If InStr(1, NewLibCode, " CREATE ", 1) > 0 Or InStr(1, NewLibCode, " DELETE ", 1) > 0 Or InStr(1, NewLibCode, " DROP ", 1) > 0 Or InStr(1, NewLibCode, " INSERT ", 1) > 1 Or InStr(1, NewLibCode, " TRACK ", 1) > 1 Or InStr(1, NewLibCode, " TRACE ", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do not use reserve words... ');", True)
                        Me.txt_LibCode.Focus()
                        Exit Sub
                    End If
                    NewLibCode = TrimX(NewLibCode)
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Enter User Code... ');", True)
                    Me.txt_LibCode.Focus()
                    Exit Sub
                End If
                'check unwanted characters
                c = 0
                counter1 = 0
                For iloop = 1 To Len(NewLibCode)
                    strcurrentchar = Mid(NewLibCode, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';~!@#$^&*-_=+|[]{}?,<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter1 = 1
                        End If
                    End If
                Next
                If counter1 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Un-wanted Characters... ');", True)
                    Me.txt_LibCode.Focus()
                    Exit Sub
                End If

                '********************************************************************************************************************
                'Server Validation for Library Name
                Dim NewLibName As String = Nothing
                NewLibName = TrimAll(txt_LibName.Text)
                If String.IsNullOrEmpty(NewLibName) Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Enter Library Name... ');", True)
                    Me.txt_LibName.Focus()
                    Exit Sub
                End If
                NewLibName = RemoveQuotes(NewLibName)
                If NewLibName.Length > 250 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Name must be of Proper Length.. ');", True)
                    txt_LibName.Focus()
                    Exit Sub
                End If
                NewLibName = " " & NewLibName & " "
                If InStr(1, NewLibName, "CREATE", 1) > 0 Or InStr(1, NewLibName, "DELETE", 1) > 0 Or InStr(1, NewLibName, "DROP", 1) > 0 Or InStr(1, NewLibName, "INSERT", 1) > 1 Or InStr(1, NewLibName, "TRACK", 1) > 1 Or InStr(1, NewLibName, "TRACE", 1) > 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Reserve Word... ');", True)
                    txt_LibName.Focus()
                    Exit Sub
                End If
                NewLibName = TrimAll(NewLibName)
                c = 0
                counter2 = 0
                For iloop = 1 To Len(NewLibName)
                    strcurrentchar = Mid(NewLibName, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';~!@#$^&*=+|[]{}?,<>=%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter2 = 1
                        End If
                    End If
                Next
                If counter2 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Do Not Use Un-Wated Characters... ');", True)
                    txt_LibName.Focus()
                    Exit Sub
                End If

                '****************************************************************************************
                'Server Validation for Parent Org
                Dim NewParent As String = Nothing
                NewParent = TrimAll(txt_ParentBody.Text)
                If Not String.IsNullOrEmpty(NewParent) Then
                    NewParent = RemoveQuotes(NewParent)
                    If NewParent.Length > 250 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not proper... ');", True)
                        Me.txt_ParentBody.Focus()
                        Exit Sub
                    End If
                    NewParent = " " & NewParent & " "
                    If InStr(1, NewParent, "CREATE", 1) > 0 Or InStr(1, NewParent, "DELETE", 1) > 0 Or InStr(1, NewParent, "DROP", 1) > 0 Or InStr(1, NewParent, "INSERT", 1) > 1 Or InStr(1, NewParent, "TRACK", 1) > 1 Or InStr(1, NewParent, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not proper... ');", True)
                        Me.txt_ParentBody.Focus()
                        Exit Sub
                    End If
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not proper... ');", True)
                    Me.txt_ParentBody.Focus()
                    Exit Sub
                End If
                NewParent = TrimAll(NewParent)
                'check unwanted characters
                c = 0
                counter3 = 0
                For iloop = 1 To Len(Parent)
                    strcurrentchar = Mid(NewParent, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';~!@#$^&*-_=+|[]{}?,<>=%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter3 = 1
                        End If
                    End If
                Next
                If counter3 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not proper... ');", True)
                    Me.txt_ParentBody.Focus()
                    Exit Sub
                End If

                '********************************************************************************************************
                'Server Validation for Address
                Dim LibAdd As String = Nothing
                LibAdd = TrimAll(txt_LibAdd.Text)
                If Not String.IsNullOrEmpty(LibAdd) Then
                    LibAdd = RemoveQuotes(LibAdd)
                    If LibAdd.Length > 250 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_LibAdd.Focus()
                        Exit Sub
                    End If
                    LibAdd = " " & LibAdd & " "
                    If InStr(1, LibAdd, "CREATE", 1) > 0 Or InStr(1, LibAdd, "DELETE", 1) > 0 Or InStr(1, LibAdd, "DROP", 1) > 0 Or InStr(1, LibAdd, "INSERT", 1) > 1 Or InStr(1, LibAdd, "TRACK", 1) > 1 Or InStr(1, LibAdd, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_LibAdd.Focus()
                        Exit Sub
                    End If
                Else
                    LibAdd = String.Empty
                End If
                LibAdd = TrimAll(LibAdd)

                'check unwanted characters
                c = 0
                counter4 = 0
                For iloop = 1 To Len(LibAdd)
                    strcurrentchar = Mid(LibAdd, iloop, 1)
                    If c = 0 Then
                        If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter4 = 1
                        End If
                    End If
                Next
                If counter4 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                    Me.txt_LibAdd.Focus()
                    Exit Sub
                End If

                '*******************************************************************************************************
                'Server Validation for City
                Dim LibCity As String = Nothing
                LibCity = TrimAll(txt_LibCity.Text)
                If Not String.IsNullOrEmpty(LibCity) Then
                    LibCity = RemoveQuotes(LibCity)
                    If LibAdd.Length > 100 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_LibCity.Focus()
                        Exit Sub
                    End If
                    LibCity = " " & LibCity & " "
                    If InStr(1, LibCity, "CREATE", 1) > 0 Or InStr(1, LibCity, "DELETE", 1) > 0 Or InStr(1, LibCity, "DROP", 1) > 0 Or InStr(1, LibCity, "INSERT", 1) > 1 Or InStr(1, LibCity, "TRACK", 1) > 1 Or InStr(1, LibCity, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_LibCity.Focus()
                        Exit Sub
                    End If
                Else
                    LibCity = String.Empty
                End If

                LibCity = TrimAll(LibCity)
                'check unwanted characters
                c = 0
                Counter5 = 0
                For iloop = 1 To Len(LibCity)
                    strcurrentchar = Mid(LibCity, iloop, 1)
                    If c = 0 Then
                        If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            Counter5 = 1
                        End If
                    End If
                Next
                If Counter5 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                    Me.txt_LibCity.Focus()
                    Exit Sub
                End If

                '****************************************************************************************
                'Server Validation for Distict
                Dim LibDist As String = Nothing
                LibDist = TrimX(txt_LibDist.Text)
                If Not String.IsNullOrEmpty(LibDist) Then
                    LibDist = RemoveQuotes(LibDist)
                    If LibDist.Length > 100 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Me.txt_LibDist.Focus()
                        Exit Sub
                    End If
                    LibDist = " " & LibDist & " "
                    If InStr(1, LibDist, "CREATE", 1) > 0 Or InStr(1, LibDist, "DELETE", 1) > 0 Or InStr(1, LibDist, "DROP", 1) > 0 Or InStr(1, LibDist, "INSERT", 1) > 1 Or InStr(1, LibDist, "TRACK", 1) > 1 Or InStr(1, LibDist, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_LibDist.Focus()
                        Exit Sub
                    End If
                    'check unwanted characters
                    c = 0
                    counter6 = 0
                    For iloop = 1 To Len(LibDist)
                        strcurrentchar = Mid(LibDist, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';~!#$^&*|[]{}?<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter6 = 1
                            End If
                        End If
                    Next
                    If counter6 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_LibDist.Focus()
                        Exit Sub
                    End If
                Else
                    LibDist = ""
                End If

                'Server Validation for State ************************************************
                Dim LibState As String = Nothing
                LibState = TrimAll(txt_LibState.Text)
                If Not String.IsNullOrEmpty(LibState) Then
                    LibState = RemoveQuotes(LibState)
                    If LibState.Length > 100 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Data must be of Proper Length... ');", True)
                        txt_LibState.Focus()
                        Exit Sub
                    End If
                    LibState = " " & LibState & " "
                    If InStr(1, LibState, "CREATE", 1) > 0 Or InStr(1, LibState, "DELETE", 1) > 0 Or InStr(1, LibState, "DROP", 1) > 0 Or InStr(1, LibState, "INSERT", 1) > 1 Or InStr(1, LibState, "TRACK", 1) > 1 Or InStr(1, LibState, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Do Not Use Reserve Word... ');", True)
                        txt_LibState.Focus()
                        Exit Sub
                    End If
                    LibState = TrimAll(LibState)
                    c = 0
                    counter9 = 0
                    For iloop = 1 To Len(LibState)
                        strcurrentchar = Mid(LibState, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*+|[]{}<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter9 = 1
                            End If
                        End If
                    Next
                    If counter9 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Un-Wated Characters... ');", True)
                        txt_LibState.Focus()
                        Exit Sub
                    End If
                Else
                    LibState = ""
                End If

                '****************************************************************************************
                'Server Validation for Library Type
                Dim LibType As String = Nothing
                LibType = DropDownList1.SelectedValue
                If Not String.IsNullOrEmpty(LibType) Then
                    LibType = RemoveQuotes(LibType)
                    If LibType.Length > 2 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' LibType Address is not Valid... ');", True)
                        DropDownList1.Focus()
                        Exit Sub
                    End If
                    If InStr(1, LibType, "CREATE", 1) > 0 Or InStr(1, LibType, "DELETE", 1) > 0 Or InStr(1, LibType, "DROP", 1) > 0 Or InStr(1, LibType, "INSERT", 1) > 1 Or InStr(1, LibType, "TRACK", 1) > 1 Or InStr(1, LibType, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' LibType Address is not Valid... ');", True)
                        DropDownList1.Focus()
                        Exit Sub
                    End If
                Else
                    LibType = "M"
                End If

                'check unwanted characters
                c = 0
                counter8 = 0
                For iloop = 1 To Len(LibType)
                    strcurrentchar = Mid(LibType, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter8 = 1
                        End If
                    End If
                Next
                If counter8 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                    Exit Sub
                End If

                'Server Validation for Main Lib Code
                Dim MainLibCode As String = Nothing
                If LibType = "B" Then
                    MainLibCode = Drop_Libraries.SelectedValue
                    If Not String.IsNullOrEmpty(MainLibCode) Then
                        MainLibCode = RemoveQuotes(MainLibCode)
                        If LibType.Length > 12 Then
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
                    counter8 = 0
                    For iloop = 1 To Len(MainLibCode)
                        strcurrentchar = Mid(MainLibCode, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter8 = 1
                            End If
                        End If
                    Next
                    If counter8 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Main Library Code is not Valid... ');", True)
                        Exit Sub
                    End If
                Else
                    MainLibCode = ""
                End If

                '**************************************************************************************************
                'Check Duplicate User Code
                Dim str2 As Object = Nothing
                Dim flag2 As Object = Nothing
                str2 = "SELECT LIB_ID FROM LIBRARIES WHERE (LIB_CODE ='" & Trim(NewLibCode) & "')"
                Dim cmd2 As New SqlCommand(str2, SqlConn)
                SqlConn.Open()
                flag2 = cmd2.ExecuteScalar
                If flag2 <> Nothing Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' LIBRARY CODE Already Exists, Please abort... ');", True)
                    Me.txt_LibCode.Focus()
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
                SQL = "SELECT * FROM LIBRARIES WHERE (LIB_ID = '00')"
                Cmd = New SqlCommand(SQL, SqlConn)
                da = New SqlDataAdapter(Cmd)
                CB = New SqlCommandBuilder(da)
                SqlConn.Open()
                da.Fill(ds, "libraries")
                dtrow = ds.Tables("libraries").NewRow

                If Not String.IsNullOrEmpty(NewLibCode) Then
                    dtrow("LIB_CODE") = NewLibCode.Trim
                End If
                If Not String.IsNullOrEmpty(NewLibName) Then
                    dtrow("LIB_NAME") = NewLibName.Trim
                End If
                If Not String.IsNullOrEmpty(NewParent) Then
                    dtrow("PARENT_BODY") = NewParent.Trim
                End If
                If Not String.IsNullOrEmpty(LibAdd) Then
                    dtrow("LIB_ADDRESS") = LibAdd.Trim
                Else
                    dtrow("LIB_ADDRESS") = System.DBNull.Value
                End If
                If Not String.IsNullOrEmpty(LibCity) Then
                    dtrow("LIB_CITY") = LibCity.Trim
                Else
                    dtrow("LIB_CITY") = System.DBNull.Value
                End If
                If Not String.IsNullOrEmpty(LibDist) Then
                    dtrow("LIB_DISTRICT") = LibDist.Trim
                Else
                    dtrow("LIB_DISTRICT") = System.DBNull.Value
                End If
                If Not String.IsNullOrEmpty(LibState) Then
                    dtrow("LIB_STATE") = LibState.Trim
                Else
                    dtrow("LIB_STATE") = System.DBNull.Value
                End If
                If Not String.IsNullOrEmpty(LibType) Then
                    dtrow("LIB_TYPE") = LibType.Trim
                Else
                    dtrow("LIB_TYPE") = "M"
                End If
                If Not String.IsNullOrEmpty(MainLibCode) Then
                    dtrow("MAIN_LIB_CODE") = MainLibCode.Trim
                Else
                    dtrow("MAIN_LIB_CODE") = System.DBNull.Value
                End If
                dtrow("DATE_ADDED") = Now.Date
                dtrow("USER_CODE") = Session.Item("LoggedUser")
                dtrow("STATUS") = "CU"
                dtrow("IP") = Request.UserHostAddress.Trim
                ds.Tables("libraries").Rows.Add(dtrow)
                da.Update(ds, "libraries")
                ClearFields()
                NewLibCode = Nothing
                NewLibName = Nothing
                NewParent = Nothing
                LibAdd = Nothing
                LibCity = Nothing
                LibDist = Nothing
                LibState = Nothing
                LibType = Nothing
                MainLibCode = Nothing
                ds.Dispose()

                Label6.Visible = True
                Label6.Text = "New Library Profile Created Successfully"
                SqlConn.Close()
                AllLibraries()
            Catch ex As Exception
                Label6.Text = "Error: " & (ex.Message())
            Finally
                SqlConn.Close()
            End Try
        End If
    End Sub
    Public Sub ClearFields()
        Me.txt_LibCode.Text = ""
        Me.txt_LibName.Text = ""
        Me.txt_ParentBody.Text = ""
        Me.txt_LibDist.Text = ""
        Me.txt_LibCity.Text = ""
        txt_LibState.Text = ""
        txt_LibAdd.Text = ""
        Drop_Libraries.ClearSelection()
        DropDownList1.ClearSelection()

        txt_LibCode.Enabled = True
        bttn_Save.Visible = True
        bttn_Update.Visible = False
        tr_status.Visible = False
    End Sub
    Protected Sub Cancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Cancel.Click
        ClearFields()
    End Sub
    'Populate the library in grid     'search librareis
    Protected Sub Search_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Search_Bttn.Click
        Dim dtLibrary As DataTable = Nothing
        Try
            Dim c, counter1, counter2, counter3, counter4 As Integer
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
                myfield = "PARENT_BODY"
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

            'Library Status validation
            Dim myLibStatus As String = Nothing
            If DropDownList4.Text <> "" Then
                myLibStatus = TrimAll(DropDownList4.SelectedValue)
                myLibStatus = RemoveQuotes(myLibStatus)
                If myLibStatus.Length > 20 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                    Me.DropDownList4.Focus()
                    Exit Sub
                End If
                myLibStatus = " " & myLibStatus & " "
                If InStr(1, myLibStatus, "CREATE", 1) > 0 Or InStr(1, myLibStatus, "DELETE", 1) > 0 Or InStr(1, myLibStatus, "DROP", 1) > 0 Or InStr(1, myLibStatus, "INSERT", 1) > 1 Or InStr(1, myLibStatus, "TRACK", 1) > 1 Or InStr(1, myLibStatus, "TRACE", 1) > 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                    Me.DropDownList4.Focus()
                    Exit Sub
                End If
                myLibStatus = TrimAll(myLibStatus)
                'check unwanted characters
                c = 0
                counter4 = 0
                For iloop = 1 To Len(myLibStatus)
                    strcurrentchar = Mid(myLibStatus, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter4 = 1
                        End If
                    End If
                Next
                If counter4 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' data is not Valid... ');", True)
                    DropDownList4.Focus()
                    Exit Sub
                End If
            Else
                myLibStatus = "CU"
            End If

            Dim SQL As String = Nothing
            SQL = "SELECT * FROM LIBRARIES WHERE ( STATUS = '" & Trim(myLibStatus) & "') "

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
            SQL = SQL & " ORDER BY PARENT_BODY ASC "

            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            da.Fill(ds)

            dtLibrary = ds.Tables(0).Copy
            Dim RecordCount As Long = 0
            If dtLibrary.Rows.Count = 0 Then
                Me.Grid1.DataSource = Nothing
                Label5.Text = "Total Record(s): " & Grid1.Rows.Count
            Else
                RecordCount = dtLibrary.Rows.Count
                Grid1.AutoGenerateColumns = False
                Grid1.DataSource = dtLibrary
                Grid1.DataBind()
                Label5.Text = "Total Record(s): " & Grid1.Rows.Count
            End If
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    Private Function ColumnEqual(ByVal A As Object, ByVal B As Object) As Boolean
        If A Is DBNull.Value And B Is DBNull.Value Then Return True ' Both are DBNull.Value.
        If A Is DBNull.Value Or B Is DBNull.Value Then Return False ' Only one is DBNull.Value.
        Return A = B                                                ' Value type standard comparison
    End Function
    'grid view page index changing event
    Protected Sub Grid_Pub_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles Grid1.PageIndexChanging
        Try
            'rebind datagrid
            Grid1.DataSource = ViewState("dt") 'temp
            Grid1.PageIndex = e.NewPageIndex
            index = e.NewPageIndex * Grid1.PageSize
            Grid1.DataBind()

        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
        End Try
    End Sub
    'property to set sortdirection
    Private Property GridViewSortDirection() As String
        Get
            Return IIf(ViewState("SortDirection") = Nothing, "ASC", ViewState("SortDirection"))
        End Get
        Set(ByVal value As String)
            ViewState("SortDirection") = value
        End Set

    End Property
    'property to set gridviewsortexpression
    Private Property GridViewSortExpression() As String
        Get
            Return IIf(ViewState("SortExpression") = Nothing, String.Empty, ViewState("SortExpression"))
        End Get
        Set(ByVal value As String)
            ViewState("SortExpression") = value
        End Set

    End Property
    'private fxn to get sort direction
    Private Function GetSortDirection() As String
        Select Case GridViewSortDirection
            Case "ASC"
                GridViewSortDirection = "DESC"
            Case "DESC"
                GridViewSortDirection = "ASC"
        End Select
        Return GridViewSortDirection
        Me.UpdatePanel1.Update()
    End Function
    'fxn to sort data table
    Protected Function SortDataTable(ByVal datatable As DataTable, ByVal isPageIndexChanging As Boolean) As DataView
        If Not datatable Is Nothing Then
            Dim dataView As New DataView(datatable)
            If GridViewSortExpression <> String.Empty Then
                If isPageIndexChanging Then
                    dataView.Sort = String.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection)
                Else
                    dataView.Sort = String.Format("{0} {1}", GridViewSortExpression, GetSortDirection())
                End If
            End If
            UpdatePanel1.Update()
            Return dataView
        Else
            Return New DataView()
        End If
    End Function
    'gridview sorting event
    Protected Sub Grid_Pub_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles Grid1.Sorting
        Dim temp As DataTable = CType(ViewState("dt"), DataTable)
        GridViewSortExpression = e.SortExpression
        Grid1.DataSource = temp
        Dim pageIndex As Integer = Grid1.PageIndex
        Grid1.DataSource = SortDataTable(Grid1.DataSource, False)
        Grid1.DataBind()
        Grid1.PageIndex = pageIndex
        UpdatePanel1.Update()
    End Sub
    Protected Sub Grid1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Grid1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Lblsr As Label = e.Row.FindControl("lblsr")
            Srno = index + 1
            If Lblsr Is Nothing = False Then
                Lblsr.Text = Srno
                index += 1
            End If
            e.Row.Attributes("onmouseover") = "this.style.cursor='hand';this.style.textDecoration='bold';this.style.background='#FFDFDF'"
            e.Row.Attributes("onmouseout") = "this.style.textDecoration='none';this.style.background='none'"
            'e.Row.Attributes("onclick") = ClientScript.GetPostBackClientHyperlink(Me, "Select$" & Convert.ToString(e.Row.RowIndex))
        End If
    End Sub
    Private Shared Function t() As Object
        Throw New NotImplementedException
    End Function
    'get value of Libcode from grid
    Private Sub Grid1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles Grid1.RowCommand
        Dim Command As SqlCommand = Nothing
        Dim drLibraries As SqlDataReader = Nothing
        AllLibraries()
        Try
            If e.CommandName = "Select" Then
                Dim myRowID As Integer
                Dim LIB_CODE As Object = Nothing
                myRowID = e.CommandArgument.ToString()
                LIB_CODE = Grid1.DataKeys(myRowID).Value
                If LIB_CODE <> "" Then
                    Dim iloop As Integer
                    Dim strcurrentchar As Object
                    Dim c As Integer
                    Dim counter1 As Integer

                    LIB_CODE = TrimX(LIB_CODE)
                    LIB_CODE = RemoveQuotes(LIB_CODE)

                    If Len(LIB_CODE).ToString > 10 Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Length of Input is not Proper!');", True)
                        Exit Sub
                    End If



                LIB_CODE = " " & LIB_CODE & " "
                If InStr(1, LIB_CODE, " CREATE ", 1) > 0 Or InStr(1, LIB_CODE, " DELETE ", 1) > 0 Or InStr(1, LIB_CODE, " DROP ", 1) > 0 Or InStr(1, LIB_CODE, " INSERT ", 1) > 1 Or InStr(1, LIB_CODE, " TRACK ", 1) > 1 Or InStr(1, LIB_CODE, " TRACE ", 1) > 1 Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Do Not Use Reserve Words!');", True)
                    Exit Sub
                End If
                LIB_CODE = TrimX(LIB_CODE)

                Me.bttn_Save.Visible = False
                Me.bttn_Update.Visible = True
                Me.txt_LibCode.Enabled = False

                'get record details from database
                Dim SQL As String = Nothing
                SQL = " SELECT * FROM LIBRARIES WHERE (LIB_CODE = '" & TrimX(LIB_CODE) & "') "
                Command = New SqlCommand(SQL, SqlConn)
                SqlConn.Open()
                drLibraries = Command.ExecuteReader(CommandBehavior.CloseConnection)
                drLibraries.Read()
                If drLibraries.HasRows = True Then

                    If drLibraries.Item("LIB_CODE").ToString <> "" Then
                        txt_LibCode.Text = drLibraries.Item("LIB_CODE").ToString
                    Else
                        txt_LibCode.Text = ""
                    End If

                    If drLibraries.Item("LIB_NAME").ToString <> "" Then
                        txt_LibName.Text = drLibraries.Item("LIB_NAME").ToString
                    Else
                        txt_LibName.Text = ""
                    End If
                    If drLibraries.Item("PARENT_BODY").ToString <> "" Then
                        txt_ParentBody.Text = drLibraries.Item("PARENT_BODY").ToString
                    Else
                        txt_ParentBody.Text = ""
                    End If
                    If drLibraries.Item("LIB_ADDRESS").ToString <> "" Then
                        txt_LibAdd.Text = drLibraries.Item("LIB_ADDRESS").ToString
                    Else
                        txt_LibAdd.Text = ""
                    End If
                    If drLibraries.Item("LIB_CITY").ToString <> "" Then
                        txt_LibCity.Text = drLibraries.Item("LIB_CITY").ToString
                    Else
                        txt_LibCity.Text = ""
                    End If
                    If drLibraries.Item("LIB_DISTRICT").ToString <> "" Then
                        txt_LibDist.Text = drLibraries.Item("LIB_DISTRICT").ToString
                    Else
                        txt_LibDist.Text = ""
                    End If
                    If drLibraries.Item("LIB_STATE").ToString <> "" Then
                        txt_LibState.Text = drLibraries.Item("LIB_STATE").ToString
                    Else
                        txt_LibState.Text = ""
                    End If
                    If drLibraries.Item("LIB_TYPE").ToString <> "" Then
                        DropDownList1.SelectedValue = drLibraries.Item("LIB_TYPE").ToString
                    Else
                        DropDownList1.SelectedValue = "M"
                    End If
                    If drLibraries.Item("MAIN_LIB_CODE").ToString <> "" Then
                        Drop_Libraries.SelectedValue = drLibraries.Item("MAIN_LIB_CODE").ToString
                    Else
                        Drop_Libraries.Text = ""
                    End If

                    tr_status.Visible = True
                    If drLibraries.Item("STATUS").ToString = "CU" Then
                        Me.RB_Current.Checked = True
                        Me.RB_Closed.Checked = False
                    Else
                        Me.RB_Closed.Checked = True
                        Me.RB_Current.Checked = False
                    End If
                    Me.txt_LibName.Focus()
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' No Record to Edit... ');", True)
                    Exit Sub
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
                Dim counter1, counter2, counter3, counter4, Counter5, counter6, counter8, counter9 As Integer

                Dim NewLibCode As String = Nothing
                NewLibCode = TrimX(txt_LibCode.Text)
                NewLibCode = UCase(NewLibCode)
                If Not String.IsNullOrEmpty(NewLibCode) Then
                    NewLibCode = RemoveQuotes(NewLibCode)
                    If NewLibCode.Length > 12 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Length of User Code is not Proper... ');", True)
                        Me.txt_LibCode.Focus()
                        Exit Sub
                    End If
                    NewLibCode = " " & NewLibCode & " "
                    If InStr(1, NewLibCode, " CREATE ", 1) > 0 Or InStr(1, NewLibCode, " DELETE ", 1) > 0 Or InStr(1, NewLibCode, " DROP ", 1) > 0 Or InStr(1, NewLibCode, " INSERT ", 1) > 1 Or InStr(1, NewLibCode, " TRACK ", 1) > 1 Or InStr(1, NewLibCode, " TRACE ", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do not use reserve words... ');", True)
                        Me.txt_LibCode.Focus()
                        Exit Sub
                    End If
                    NewLibCode = TrimX(NewLibCode)
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Enter User Code... ');", True)
                    Me.txt_LibCode.Focus()
                    Exit Sub
                End If
                'check unwanted characters
                c = 0
                counter1 = 0
                For iloop = 1 To Len(NewLibCode)
                    strcurrentchar = Mid(NewLibCode, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';~!@#$^&*-_=+|[]{}?,<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter1 = 1
                        End If
                    End If
                Next
                If counter1 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Un-wanted Characters... ');", True)
                    Me.txt_LibCode.Focus()
                    Exit Sub
                End If

                '********************************************************************************************************************

                'Server Validation for Library Name
                Dim NewLibName As String = Nothing
                NewLibName = TrimAll(txt_LibName.Text)
                If String.IsNullOrEmpty(NewLibName) Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Enter Library Name... ');", True)
                    Me.txt_LibName.Focus()
                    Exit Sub
                End If
                NewLibName = RemoveQuotes(NewLibName)
                If NewLibName.Length > 250 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Name must be of Proper Length.. ');", True)
                    txt_LibName.Focus()
                    Exit Sub
                End If
                NewLibName = " " & NewLibName & " "
                If InStr(1, NewLibName, "CREATE", 1) > 0 Or InStr(1, NewLibName, "DELETE", 1) > 0 Or InStr(1, NewLibName, "DROP", 1) > 0 Or InStr(1, NewLibName, "INSERT", 1) > 1 Or InStr(1, NewLibName, "TRACK", 1) > 1 Or InStr(1, NewLibName, "TRACE", 1) > 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Reserve Word... ');", True)
                    txt_LibName.Focus()
                    Exit Sub
                End If
                NewLibName = TrimAll(NewLibName)
                c = 0
                counter2 = 0
                For iloop = 1 To Len(NewLibName)
                    strcurrentchar = Mid(NewLibName, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';~!@#$^&*=+|[]{}?,<>=%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter2 = 1
                        End If
                    End If
                Next
                If counter2 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Do Not Use Un-Wated Characters... ');", True)
                    txt_LibName.Focus()
                    Exit Sub
                End If

                '****************************************************************************************
                'Server Validation for Parent Org
                Dim NewParent As String = Nothing
                NewParent = TrimAll(txt_ParentBody.Text)
                If Not String.IsNullOrEmpty(NewParent) Then
                    NewParent = RemoveQuotes(NewParent)
                    If NewParent.Length > 250 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not proper... ');", True)
                        Me.txt_ParentBody.Focus()
                        Exit Sub
                    End If
                    NewParent = " " & NewParent & " "
                    If InStr(1, NewParent, "CREATE", 1) > 0 Or InStr(1, NewParent, "DELETE", 1) > 0 Or InStr(1, NewParent, "DROP", 1) > 0 Or InStr(1, NewParent, "INSERT", 1) > 1 Or InStr(1, NewParent, "TRACK", 1) > 1 Or InStr(1, NewParent, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not proper... ');", True)
                        Me.txt_ParentBody.Focus()
                        Exit Sub
                    End If
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not proper... ');", True)
                    Me.txt_ParentBody.Focus()
                    Exit Sub
                End If
                NewParent = TrimAll(NewParent)
                'check unwanted characters
                c = 0
                counter3 = 0
                For iloop = 1 To Len(Parent)
                    strcurrentchar = Mid(NewParent, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';~!@#$^&*-_=+|[]{}?,<>=%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter3 = 1
                        End If
                    End If
                Next
                If counter3 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not proper... ');", True)
                    Me.txt_ParentBody.Focus()
                    Exit Sub
                End If

                '********************************************************************************************************
                'Server Validation for Address
                Dim LibAdd As String = Nothing
                LibAdd = TrimAll(txt_LibAdd.Text)
                If Not String.IsNullOrEmpty(LibAdd) Then
                    LibAdd = RemoveQuotes(LibAdd)
                    If LibAdd.Length > 250 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_LibAdd.Focus()
                        Exit Sub
                    End If
                    LibAdd = " " & LibAdd & " "
                    If InStr(1, LibAdd, "CREATE", 1) > 0 Or InStr(1, LibAdd, "DELETE", 1) > 0 Or InStr(1, LibAdd, "DROP", 1) > 0 Or InStr(1, LibAdd, "INSERT", 1) > 1 Or InStr(1, LibAdd, "TRACK", 1) > 1 Or InStr(1, LibAdd, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_LibAdd.Focus()
                        Exit Sub
                    End If
                Else
                    LibAdd = String.Empty
                End If
                LibAdd = TrimAll(LibAdd)

                'check unwanted characters
                c = 0
                counter4 = 0
                For iloop = 1 To Len(LibAdd)
                    strcurrentchar = Mid(LibAdd, iloop, 1)
                    If c = 0 Then
                        If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter4 = 1
                        End If
                    End If
                Next
                If counter4 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                    Me.txt_LibAdd.Focus()
                    Exit Sub
                End If

                '*******************************************************************************************************
                'Server Validation for City
                Dim LibCity As String = Nothing
                LibCity = TrimAll(txt_LibCity.Text)
                If Not String.IsNullOrEmpty(LibCity) Then
                    LibCity = RemoveQuotes(LibCity)
                    If LibAdd.Length > 100 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_LibCity.Focus()
                        Exit Sub
                    End If
                    LibCity = " " & LibCity & " "
                    If InStr(1, LibCity, "CREATE", 1) > 0 Or InStr(1, LibCity, "DELETE", 1) > 0 Or InStr(1, LibCity, "DROP", 1) > 0 Or InStr(1, LibCity, "INSERT", 1) > 1 Or InStr(1, LibCity, "TRACK", 1) > 1 Or InStr(1, LibCity, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_LibCity.Focus()
                        Exit Sub
                    End If
                Else
                    LibCity = String.Empty
                End If

                LibCity = TrimAll(LibCity)
                'check unwanted characters
                c = 0
                Counter5 = 0
                For iloop = 1 To Len(LibCity)
                    strcurrentchar = Mid(LibCity, iloop, 1)
                    If c = 0 Then
                        If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            Counter5 = 1
                        End If
                    End If
                Next
                If Counter5 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                    Me.txt_LibCity.Focus()
                    Exit Sub
                End If

                '****************************************************************************************
                'Server Validation for Distict
                Dim LibDist As String = Nothing
                LibDist = TrimX(txt_LibDist.Text)
                If Not String.IsNullOrEmpty(LibDist) Then
                    LibDist = RemoveQuotes(LibDist)
                    If LibDist.Length > 100 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Me.txt_LibDist.Focus()
                        Exit Sub
                    End If
                    LibDist = " " & LibDist & " "
                    If InStr(1, LibDist, "CREATE", 1) > 0 Or InStr(1, LibDist, "DELETE", 1) > 0 Or InStr(1, LibDist, "DROP", 1) > 0 Or InStr(1, LibDist, "INSERT", 1) > 1 Or InStr(1, LibDist, "TRACK", 1) > 1 Or InStr(1, LibDist, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_LibDist.Focus()
                        Exit Sub
                    End If
                    'check unwanted characters
                    c = 0
                    counter6 = 0
                    For iloop = 1 To Len(LibDist)
                        strcurrentchar = Mid(LibDist, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';~!#$^&*|[]{}?<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter6 = 1
                            End If
                        End If
                    Next
                    If counter6 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_LibDist.Focus()
                        Exit Sub
                    End If
                Else
                    LibDist = ""
                End If

                'Server Validation for State ************************************************
                Dim LibState As String = Nothing
                LibState = TrimAll(txt_LibState.Text)
                If Not String.IsNullOrEmpty(LibState) Then
                    LibState = RemoveQuotes(LibState)
                    If LibState.Length > 100 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Data must be of Proper Length... ');", True)
                        txt_LibState.Focus()
                        Exit Sub
                    End If
                    LibState = " " & LibState & " "
                    If InStr(1, LibState, "CREATE", 1) > 0 Or InStr(1, LibState, "DELETE", 1) > 0 Or InStr(1, LibState, "DROP", 1) > 0 Or InStr(1, LibState, "INSERT", 1) > 1 Or InStr(1, LibState, "TRACK", 1) > 1 Or InStr(1, LibState, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Do Not Use Reserve Word... ');", True)
                        txt_LibState.Focus()
                        Exit Sub
                    End If
                    LibState = TrimAll(LibState)
                    c = 0
                    counter9 = 0
                    For iloop = 1 To Len(LibState)
                        strcurrentchar = Mid(LibState, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*+|[]{}<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter9 = 1
                            End If
                        End If
                    Next
                    If counter9 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Un-Wated Characters... ');", True)
                        txt_LibState.Focus()
                        Exit Sub
                    End If
                Else
                    LibState = ""
                End If

                '****************************************************************************************
                'Server Validation for Library Type
                Dim LibType As String = Nothing
                LibType = DropDownList1.SelectedValue
                If Not String.IsNullOrEmpty(LibType) Then
                    LibType = RemoveQuotes(LibType)
                    If LibType.Length > 2 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' LibType Address is not Valid... ');", True)
                        DropDownList1.Focus()
                        Exit Sub
                    End If
                    If InStr(1, LibType, "CREATE", 1) > 0 Or InStr(1, LibType, "DELETE", 1) > 0 Or InStr(1, LibType, "DROP", 1) > 0 Or InStr(1, LibType, "INSERT", 1) > 1 Or InStr(1, LibType, "TRACK", 1) > 1 Or InStr(1, LibType, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' LibType Address is not Valid... ');", True)
                        DropDownList1.Focus()
                        Exit Sub
                    End If
                Else
                    LibType = "M"
                End If

                'check unwanted characters
                c = 0
                counter8 = 0
                For iloop = 1 To Len(LibType)
                    strcurrentchar = Mid(LibType, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter8 = 1
                        End If
                    End If
                Next
                If counter8 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                    Exit Sub
                End If

                'Server Validation for Main Lib Code
                Dim MainLibCode As String = Nothing
                If LibType = "B" Then
                    MainLibCode = Drop_Libraries.SelectedValue
                    If Not String.IsNullOrEmpty(MainLibCode) Then
                        MainLibCode = RemoveQuotes(MainLibCode)
                        If LibType.Length > 12 Then
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
                    counter8 = 0
                    For iloop = 1 To Len(MainLibCode)
                        strcurrentchar = Mid(MainLibCode, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter8 = 1
                            End If
                        End If
                    Next
                    If counter8 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Main Library Code is not Valid... ');", True)
                        Exit Sub
                    End If
                Else
                    MainLibCode = ""
                End If

                '****************************************************************************************************
                'UPDATE THE LIBRARY PROFILE   
                SQL = "SELECT * FROM LIBRARIES WHERE (LIB_CODE='" & Trim(NewLibCode) & "')"
                SqlConn.Open()
                da = New SqlDataAdapter(SQL, SqlConn)
                cb = New SqlCommandBuilder(da)
                da.Fill(ds, "LIBRARIES")
                If ds.Tables("LIBRARIES").Rows.Count <> 0 Then
                    If Not String.IsNullOrEmpty(NewLibName) Then
                        ds.Tables("LIBRARIES").Rows(0)("LIB_NAME") = NewLibName.Trim
                    Else
                        ds.Tables("LIBRARIES").Rows(0)("LIB_NAME") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(NewParent) Then
                        ds.Tables("LIBRARIES").Rows(0)("PARENT_BODY") = NewParent.Trim
                    Else
                        ds.Tables("LIBRARIES").Rows(0)("PARENT_BODY") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(LibAdd) Then
                        ds.Tables("LIBRARIES").Rows(0)("LIB_ADDRESS") = LibAdd.Trim
                    Else
                        ds.Tables("LIBRARIES").Rows(0)("LIB_ADDRESS") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(LibCity) Then
                        ds.Tables("LIBRARIES").Rows(0)("LIB_CITY") = LibCity.Trim
                    Else
                        ds.Tables("LIBRARIES").Rows(0)("LIB_CITY") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(LibDist) Then
                        ds.Tables("LIBRARIES").Rows(0)("LIB_DISTRICT") = LibDist.Trim
                    Else
                        ds.Tables("LIBRARIES").Rows(0)("LIB_DISTRICT") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(LibState) Then
                        ds.Tables("LIBRARIES").Rows(0)("LIB_STATE") = LibState.ToString.Trim
                    Else
                        ds.Tables("LIBRARIES").Rows(0)("LIB_STATE") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(LibType) Then
                        ds.Tables("LIBRARIES").Rows(0)("LIB_TYPE") = LibType.ToString.Trim
                    Else
                        ds.Tables("LIBRARIES").Rows(0)("LIB_TYPE") = "M"
                    End If
                    If Not String.IsNullOrEmpty(MainLibCode) Then
                        ds.Tables("LIBRARIES").Rows(0)("MAIN_LIB_CODE") = MainLibCode.ToString.Trim
                    Else
                        ds.Tables("LIBRARIES").Rows(0)("MAIN_LIB_CODE") = System.DBNull.Value
                    End If

                    If Me.RB_Current.Checked = True Then
                        ds.Tables("LIBRARIES").Rows(0)("STATUS") = "CU"
                    Else
                        ds.Tables("LIBRARIES").Rows(0)("STATUS") = "CL"
                    End If

                    ds.Tables("LIBRARIES").Rows(0)("DATE_MODIFIED") = Now.Date
                    ds.Tables("LIBRARIES").Rows(0)("IP") = Request.UserHostAddress.Trim
                    ds.Tables("LIBRARIES").Rows(0)("UPDATED_BY") = UserCode.Trim
                    da.Update(ds, "LIBRARIES")
                    NewLibCode = Nothing
                    NewLibName = Nothing
                    NewParent = Nothing
                    LibAdd = Nothing
                    LibCity = Nothing
                    LibDist = Nothing
                    LibState = Nothing
                    LibType = Nothing
                    MainLibCode = Nothing

                    Label6.Visible = True
                    Label6.Text = "Library Profile Updated Successfully"
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Library Profile Updated Successfully... ');", True)
                    ClearFields()
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Library Profile Update - Please Contact System Administrator... ');", True)
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