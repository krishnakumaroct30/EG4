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
Public Class UpdateLibraryProfile
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session.Item("IsThisU") <> Nothing Then
                If Not SConnection() = True Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Database Connection is lost..Try Again !');", True)
                Else
                    txt_Lib_Code.Enabled = False
                    txt_Lib_Code.Text = Session.Item("LoggedLibcode") 'LibCode
                    If Page.IsPostBack = False Then
                        GetLibraryDetails()
                    End If
                    Me.txt_Lib_Name.Focus()
                End If

                    'edit profile button in lib admin
                Dim EditLibProfileButton As Object 'System.Web.UI.WebControls.Button
                EditLibProfileButton = Master.FindControl("Accordion1").FindControl("LibAdminPane").FindControl("Lib_UpdateLibProfile_Bttn")
                EditLibProfileButton.ForeColor = Drawing.Color.Red
                myPaneName = "LibAdminPane"  'paneSelectedIndex = 0
            Else
                Response.Redirect("~/Default.aspx", False)
            End If
        Catch ex As Exception
            Label6.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    Private Sub EditUserProfile_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Me.txt_Lib_Name.Focus()
    End Sub
    Public Sub GetLibraryDetails()
        Dim Command As SqlCommand = Nothing
        Dim drLibraries As SqlDataReader = Nothing
        Try
            If txt_Lib_Code.Text <> "" Then

                Dim iloop As Integer
                Dim strcurrentchar As Object
                Dim c As Integer
                Dim counter1 As Integer

                Dim myEdLibCode As String = String.Empty
                myEdLibCode = TrimX(Me.txt_Lib_Code.Text)
                myEdLibCode = UCase(myEdLibCode)
                If Not String.IsNullOrEmpty(myEdLibCode) Then
                    myEdLibCode = RemoveQuotes(myEdLibCode)
                    If myEdLibCode.Length > 11 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Length of Library Code is not Proper... ');", True)
                        Me.txt_Lib_Code.Focus()
                        Exit Sub
                    End If
                    myEdLibCode = " " & myEdLibCode & " "
                    If InStr(1, myEdLibCode, " CREATE ", 1) > 0 Or InStr(1, myEdLibCode, " DELETE ", 1) > 0 Or InStr(1, myEdLibCode, " DROP ", 1) > 0 Or InStr(1, myEdLibCode, " INSERT ", 1) > 1 Or InStr(1, myEdLibCode, " TRACK ", 1) > 1 Or InStr(1, myEdLibCode, " TRACE ", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do not use reserve words... ');", True)
                        Me.txt_Lib_Code.Focus()
                        Exit Sub
                    End If
                    myEdLibCode = TrimX(myEdLibCode)
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Enter Library Code... ');", True)
                    Me.txt_Lib_Code.Focus()
                    Exit Sub
                End If
                'check unwanted characters
                c = 0
                counter1 = 0
                For iloop = 1 To Len(myEdLibCode)
                    strcurrentchar = Mid(myEdLibCode, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';~!@#$^&*-_=+|[]{}?,<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter1 = 1
                        End If
                    End If
                Next
                If counter1 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Un-wanted Characters... ');", True)
                    Me.txt_Lib_Code.Focus()
                    Exit Sub
                End If

                'get record details from database
                Dim SQL As String = Nothing
                SQL = " SELECT * FROM LIBRARIES WHERE (LIB_CODE = '" & TrimX(Me.txt_Lib_Code.Text) & "') "
                Command = New SqlCommand(SQL, SqlConn)
                SqlConn.Open()
                drLibraries = Command.ExecuteReader(CommandBehavior.CloseConnection)
                drLibraries.Read()
                If drLibraries.HasRows = True Then
                    If drLibraries.Item("LIB_NAME").ToString <> "" Then
                        txt_Lib_Name.Text = drLibraries.Item("LIB_NAME").ToString
                    Else
                        txt_Lib_Name.Text = ""
                    End If
                    If drLibraries.Item("LIB_NAME2").ToString <> "" Then
                        txt_Lib_Name2.Text = drLibraries.Item("LIB_NAME2").ToString
                    Else
                        txt_Lib_Name2.Text = ""
                    End If
                    If drLibraries.Item("LIB_ADDRESS").ToString <> "" Then
                        txt_Lib_Add.Text = drLibraries.Item("LIB_ADDRESS").ToString
                    Else
                        txt_Lib_Add.Text = ""
                    End If
                    If drLibraries.Item("LIB_ADDRESS2").ToString <> "" Then
                        txt_Lib_Add2.Text = drLibraries.Item("LIB_ADDRESS2").ToString
                    Else
                        txt_Lib_Add2.Text = ""
                    End If
                    If drLibraries.Item("LIB_CITY").ToString <> "" Then
                        txt_Lib_City.Text = drLibraries.Item("LIB_CITY").ToString
                    Else
                        txt_Lib_City.Text = ""
                    End If
                    If drLibraries.Item("LIB_CITY2").ToString <> "" Then
                        txt_Lib_City2.Text = drLibraries.Item("LIB_CITY2").ToString
                    Else
                        txt_Lib_City2.Text = ""
                    End If
                    If drLibraries.Item("LIB_DISTRICT").ToString <> "" Then
                        txt_Lib_District.Text = drLibraries.Item("LIB_DISTRICT").ToString
                    Else
                        txt_Lib_District.Text = ""
                    End If
                    If drLibraries.Item("LIB_DISTRICT2").ToString <> "" Then
                        txt_Lib_District2.Text = drLibraries.Item("LIB_DISTRICT2").ToString
                    Else
                        txt_Lib_District2.Text = ""
                    End If
                    If drLibraries.Item("LIB_STATE").ToString <> "" Then
                        txt_Lib_State.Text = drLibraries.Item("LIB_STATE").ToString
                    Else
                        txt_Lib_State.Text = ""
                    End If
                    If drLibraries.Item("LIB_STATE2").ToString <> "" Then
                        txt_Lib_State2.Text = drLibraries.Item("LIB_STATE2").ToString
                    Else
                        txt_Lib_State2.Text = ""
                    End If
                    If drLibraries.Item("LIB_PHONE").ToString <> "" Then
                        txt_Lib_Phone.Text = drLibraries.Item("LIB_PHONE").ToString
                    Else
                        txt_Lib_Phone.Text = ""
                    End If
                    If drLibraries.Item("LIB_FAX").ToString <> "" Then
                        txt_Lib_Fax.Text = drLibraries.Item("LIB_FAX").ToString
                    Else
                        txt_Lib_Fax.Text = ""
                    End If
                    If drLibraries.Item("LIB_EMAIL").ToString <> "" Then
                        txt_Lib_Email.Text = drLibraries.Item("LIB_EMAIL").ToString
                    Else
                        txt_Lib_Email.Text = ""
                    End If
                    If drLibraries.Item("LIB_URL").ToString <> "" Then
                        txt_Lib_URL.Text = drLibraries.Item("LIB_URL").ToString
                    Else
                        txt_Lib_URL.Text = ""
                    End If
                    If drLibraries.Item("LIB_INCHARGE").ToString <> "" Then
                        txt_Lib_Incharge.Text = drLibraries.Item("LIB_INCHARGE").ToString
                    Else
                        txt_Lib_Incharge.Text = ""
                    End If
                    If drLibraries.Item("LIB_INCHARGE2").ToString <> "" Then
                        txt_Lib_Incharge2.Text = drLibraries.Item("LIB_INCHARGE2").ToString
                    Else
                        txt_Lib_Incharge2.Text = ""
                    End If
                    If drLibraries.Item("PARENT_BODY").ToString <> "" Then
                        txt_Lib_Parent.Text = drLibraries.Item("PARENT_BODY").ToString
                    Else
                        txt_Lib_Parent.Text = ""
                    End If
                    If drLibraries.Item("PARENT_BODY2").ToString <> "" Then
                        txt_Lib_Parent2.Text = drLibraries.Item("PARENT_BODY2").ToString
                    Else
                        txt_Lib_Parent2.Text = ""
                    End If
                    If drLibraries.Item("LIB_INTRO").ToString <> "" Then
                        txt_Lib_Intro.Text = drLibraries.Item("LIB_INTRO").ToString
                    Else
                        txt_Lib_Intro.Text = ""
                    End If
                    If drLibraries.Item("LIB_INTRO2").ToString <> "" Then
                        txt_Lib_Intro2.Text = drLibraries.Item("LIB_INTRO2").ToString
                    Else
                        txt_Lib_Intro2.Text = ""
                    End If
                    If drLibraries.Item("LIB_OBJECTIVE").ToString <> "" Then
                        txt_Lib_Objective.Text = drLibraries.Item("LIB_OBJECTIVE").ToString
                    Else
                        txt_Lib_Objective.Text = ""
                    End If
                    If drLibraries.Item("LIB_OBJECTIVE2").ToString <> "" Then
                        txt_Lib_Objective2.Text = drLibraries.Item("LIB_OBJECTIVE2").ToString
                    Else
                        txt_Lib_Objective2.Text = ""
                    End If
                    If drLibraries.Item("LIB_HISTORY").ToString <> "" Then
                        txt_Lib_History.Text = drLibraries.Item("LIB_HISTORY").ToString
                    Else
                        txt_Lib_History.Text = ""
                    End If
                    If drLibraries.Item("LIB_HISTORY2").ToString <> "" Then
                        txt_Lib_History2.Text = drLibraries.Item("LIB_HISTORY2").ToString
                    Else
                        txt_Lib_History2.Text = ""
                    End If
                    If drLibraries.Item("LIB_SERVICES").ToString <> "" Then
                        txt_Lib_Services.Text = drLibraries.Item("LIB_SERVICES").ToString
                    Else
                        txt_Lib_Services.Text = ""
                    End If
                    If drLibraries.Item("LIB_SERVICES2").ToString <> "" Then
                        txt_Lib_Services2.Text = drLibraries.Item("LIB_SERVICES2").ToString
                    Else
                        txt_Lib_Services2.Text = ""
                    End If
                    If drLibraries.Item("LIB_TIMING").ToString <> "" Then
                        txt_Lib_Timing.Text = drLibraries.Item("LIB_TIMING").ToString
                    Else
                        txt_Lib_Timing.Text = ""
                    End If
                    If drLibraries.Item("PARENT_URL").ToString <> "" Then
                        txt_Lib_ParentURL.Text = drLibraries.Item("PARENT_URL").ToString
                    Else
                        txt_Lib_ParentURL.Text = ""
                    End If
                    If drLibraries.Item("LIB_SERVICES").ToString <> "" Then
                        txt_Lib_Services.Text = drLibraries.Item("LIB_SERVICES").ToString
                    Else
                        txt_Lib_Services.Text = ""
                    End If
                    If drLibraries.Item("REMARKS").ToString <> "" Then
                        txt_Lib_Remarks.Text = drLibraries.Item("REMARKS").ToString
                    Else
                        txt_Lib_Remarks.Text = ""
                    End If
                    If drLibraries.Item("SMS_UID").ToString <> "" Then
                        txt_Lib_SMSuid.Text = drLibraries.Item("SMS_UID").ToString
                    Else
                        txt_Lib_SMSuid.Text = ""
                    End If
                    If drLibraries.Item("SMS_PW").ToString <> "" Then
                        txt_Lib_SMSpw.Text = drLibraries.Item("SMS_PW").ToString
                    Else
                        txt_Lib_SMSpw.Text = ""
                    End If
                    If drLibraries.Item("SMS_SENDER").ToString <> "" Then
                        txt_Lib_SMSsender.Text = drLibraries.Item("SMS_SENDER").ToString
                    Else
                        txt_Lib_SMSsender.Text = ""
                    End If

                    If drLibraries.Item("SMS_IP").ToString <> "" Then
                        txt_Lib_SMSip.Text = drLibraries.Item("SMS_IP").ToString
                    Else
                        txt_Lib_SMSip.Text = ""
                    End If

                    If drLibraries.Item("BARCODE_PRINTER").ToString <> "" Then
                        txt_Lib_Barcode.Text = drLibraries.Item("BARCODE_PRINTER").ToString
                    Else
                        txt_Lib_Barcode.Text = ""
                    End If

                    If drLibraries.Item("CLASS_SCHEME").ToString <> "" Then
                        DropDownList1.SelectedValue = drLibraries.Item("CLASS_SCHEME").ToString
                    Else
                        DropDownList1.Text = ""
                    End If

                    If drLibraries.Item("CAT_CODE").ToString <> "" Then
                        DropDownList2.SelectedValue = drLibraries.Item("CAT_CODE").ToString
                    Else
                        DropDownList2.Text = ""
                    End If

                    If drLibraries.Item("ACQ_RETRO").ToString <> "" Then
                        DDL_Acq.SelectedValue = drLibraries.Item("ACQ_RETRO").ToString
                    Else
                        DropDownList2.SelectedValue = "N"
                    End If

                    If drLibraries.Item("LIB_PHOTO").ToString <> "" Then
                        Dim strURL As String = "~/Account/Lib_GetPhoto.aspx?LIB_CODE=" & txt_Lib_Code.Text & ""
                        Image22.ImageUrl = strURL
                    Else
                        Image22.Visible = False
                    End If

                    If drLibraries.Item("LIB_LOGO").ToString <> "" Then
                        Dim strURL As String = "~/Account/Lib_GetLogo.aspx?LIB_CODE=" & txt_Lib_Code.Text & ""
                        Image23.ImageUrl = strURL
                    Else
                        Image23.Visible = False
                    End If
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' No Record to Edit... ');", True)
                    Exit Sub
                End If

            End If
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
        Finally
            Command.Dispose()
            drLibraries.Close()
            SqlConn.Close()
        End Try
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
                Dim counter1, counter2, counter3, counter4, Counter5, counter6, counter7, counter8, counter9, counter10, counter11, counter12, counter13, counter14, counter15, counter16, counter17, counter18, counter19, counter20, counter21, counter22, counter23, counter24, counter25, counter26, counter27, counter28, counter29, counter30, counter31, counter32, counter33, counter34, counter35, counter36, counter37 As Integer

                'Server Validation for User Code
                Dim newLbCode As String = Nothing
                newLbCode = TrimX(txt_Lib_Code.Text)
                newLbCode = UCase(newLbCode)
                If Not String.IsNullOrEmpty(newLbCode) Then
                    newLbCode = RemoveQuotes(newLbCode)
                    If newLbCode.Length > 11 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Length of User Code is not Proper... ');", True)
                        Me.txt_Lib_Code.Focus()
                        Exit Sub
                    End If
                    newLbCode = " " & newLbCode & " "
                    If InStr(1, newLbCode, " CREATE ", 1) > 0 Or InStr(1, newLbCode, " DELETE ", 1) > 0 Or InStr(1, newLbCode, " DROP ", 1) > 0 Or InStr(1, newLbCode, " INSERT ", 1) > 1 Or InStr(1, newLbCode, " TRACK ", 1) > 1 Or InStr(1, newLbCode, " TRACE ", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do not use reserve words... ');", True)
                        Me.txt_Lib_Code.Focus()
                        Exit Sub
                    End If
                    newLbCode = TrimX(newLbCode)
                    'check unwanted characters
                    c = 0
                    counter1 = 0
                    For iloop = 1 To Len(newLbCode)
                        strcurrentchar = Mid(newLbCode, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';~!@#$^&*-_=+|[]{}?,<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter1 = 1
                            End If
                        End If
                    Next
                    If counter1 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Un-wanted Characters... ');", True)
                        Me.txt_Lib_Code.Focus()
                        Exit Sub
                    End If
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Enter User Code... ');", True)
                    Me.txt_Lib_Code.Focus()
                    Exit Sub
                End If
               

                'Server Validation for Full Name
                Dim LbName As String = Nothing
                LbName = TrimAll(txt_Lib_Name.Text)
                If String.IsNullOrEmpty(LbName) Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Enter Library Name... ');", True)
                    Me.txt_Lib_Name.Focus()
                    Exit Sub
                Else
                    LbName = RemoveQuotes(LbName)
                    If LbName.Length > 256 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Lib Name must be of Proper Length.. ');", True)
                        txt_Lib_Name.Focus()
                        Exit Sub
                    End If
                    LbName = " " & LbName & " "
                    If InStr(1, LbName, "CREATE", 1) > 0 Or InStr(1, LbName, "DELETE", 1) > 0 Or InStr(1, LbName, "DROP", 1) > 0 Or InStr(1, LbName, "INSERT", 1) > 1 Or InStr(1, LbName, "TRACK", 1) > 1 Or InStr(1, LbName, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Reserve Word... ');", True)
                        txt_Lib_Name.Focus()
                        Exit Sub
                    End If
                    LbName = TrimAll(LbName)
                    c = 0
                    counter2 = 0
                    For iloop = 1 To Len(LbName)
                        strcurrentchar = Mid(LbName, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';~!@#$^&*=+|[]{}?,<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter2 = 1
                            End If
                        End If
                    Next
                    If counter2 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Do Not Use Un-Wated Characters... ');", True)
                        txt_Lib_Name.Focus()
                        Exit Sub
                    End If
                End If

                'Server Validation for lib Name in local lang
                Dim LbName2 As String = Nothing
                LbName2 = TrimAll(txt_Lib_Name2.Text)
                If String.IsNullOrEmpty(LbName2) Then
                    LbName2 = ""
                Else
                    LbName2 = RemoveQuotes(LbName2)
                    If LbName2.Length > 256 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Lib Name must be of Proper Length.. ');", True)
                        txt_Lib_Name2.Focus()
                        Exit Sub
                    End If
                    LbName2 = " " & LbName2 & " "
                    If InStr(1, LbName2, "CREATE", 1) > 0 Or InStr(1, LbName2, "DELETE", 1) > 0 Or InStr(1, LbName2, "DROP", 1) > 0 Or InStr(1, LbName2, "INSERT", 1) > 1 Or InStr(1, LbName2, "TRACK", 1) > 1 Or InStr(1, LbName2, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Reserve Word... ');", True)
                        txt_Lib_Name2.Focus()
                        Exit Sub
                    End If
                    LbName2 = TrimAll(LbName2)
                    c = 0
                    counter3 = 0
                    For iloop = 1 To Len(LbName2)
                        strcurrentchar = Mid(LbName2, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';~!@#$^&*=+|[]{}?,<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter3 = 1
                            End If
                        End If
                    Next
                    If counter3 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Do Not Use Un-Wated Characters... ');", True)
                        txt_Lib_Name2.Focus()
                        Exit Sub
                    End If
                End If

                '****************************************************************************************
                'Server Validation for Parent Org
                Dim NewParent As String = Nothing
                NewParent = TrimAll(txt_Lib_Parent.Text)
                If Not String.IsNullOrEmpty(NewParent) Then
                    NewParent = RemoveQuotes(NewParent)
                    If NewParent.Length > 401 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not proper Length... ');", True)
                        Me.txt_Lib_Parent.Focus()
                        Exit Sub
                    End If
                    NewParent = " " & NewParent & " "
                    If InStr(1, NewParent, "CREATE", 1) > 0 Or InStr(1, NewParent, "DELETE", 1) > 0 Or InStr(1, NewParent, "DROP", 1) > 0 Or InStr(1, NewParent, "INSERT", 1) > 1 Or InStr(1, NewParent, "TRACK", 1) > 1 Or InStr(1, NewParent, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Do not use reserve keywords... ');", True)
                        Me.txt_Lib_Parent.Focus()
                        Exit Sub
                    End If
                    NewParent = TrimAll(NewParent)
                    'check unwanted characters
                    c = 0
                    counter4 = 0
                    For iloop = 1 To Len(Parent)
                        strcurrentchar = Mid(NewParent, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';~!@#$^&*-_=+|[]{}?,<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter4 = 1
                            End If
                        End If
                    Next
                    If counter4 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not proper... ');", True)
                        Me.txt_Lib_Parent.Focus()
                        Exit Sub
                    End If
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Enter Parent Organization... ');", True)
                    Me.txt_Lib_Parent.Focus()
                    Exit Sub
                End If

                '****************************************************************************************
                'Server Validation for Parent Org in local lang
                Dim NewParent2 As String = Nothing
                NewParent2 = TrimAll(txt_Lib_Parent2.Text)
                If Not String.IsNullOrEmpty(NewParent2) Then
                    NewParent2 = RemoveQuotes(NewParent2)
                    If NewParent2.Length > 401 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not proper... ');", True)
                        Me.txt_Lib_Parent2.Focus()
                        Exit Sub
                    End If
                    NewParent2 = " " & NewParent2 & " "
                    If InStr(1, NewParent2, "CREATE", 1) > 0 Or InStr(1, NewParent2, "DELETE", 1) > 0 Or InStr(1, NewParent2, "DROP", 1) > 0 Or InStr(1, NewParent2, "INSERT", 1) > 1 Or InStr(1, NewParent2, "TRACK", 1) > 1 Or InStr(1, NewParent2, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not proper... ');", True)
                        Me.txt_Lib_Parent2.Focus()
                        Exit Sub
                    End If
                    NewParent2 = TrimAll(NewParent2)
                    'check unwanted characters
                    c = 0
                    Counter5 = 0
                    For iloop = 1 To Len(NewParent2)
                        strcurrentchar = Mid(NewParent2, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';~!@#$^&*-_=+|[]{}?,<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                Counter5 = 1
                            End If
                        End If
                    Next
                    If Counter5 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not proper... ');", True)
                        Me.txt_Lib_Parent2.Focus()
                        Exit Sub
                    End If
                Else
                    NewParent2 = ""
                End If

                '********************************************************************************************************
                'Server Validation for Address
                Dim LibAdd As String = Nothing
                LibAdd = TrimAll(txt_Lib_Add.Text)
                If Not String.IsNullOrEmpty(LibAdd) Then
                    LibAdd = RemoveQuotes(LibAdd)
                    If LibAdd.Length > 256 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Lib_Add.Focus()
                        Exit Sub
                    End If
                    LibAdd = " " & LibAdd & " "
                    If InStr(1, LibAdd, "CREATE", 1) > 0 Or InStr(1, LibAdd, "DELETE", 1) > 0 Or InStr(1, LibAdd, "DROP", 1) > 0 Or InStr(1, LibAdd, "INSERT", 1) > 1 Or InStr(1, LibAdd, "TRACK", 1) > 1 Or InStr(1, LibAdd, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Lib_Add.Focus()
                        Exit Sub
                    End If
                    LibAdd = TrimAll(LibAdd)

                    'check unwanted characters
                    c = 0
                    counter6 = 0
                    For iloop = 1 To Len(LibAdd)
                        strcurrentchar = Mid(LibAdd, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter6 = 1
                            End If
                        End If
                    Next
                    If counter6 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Me.txt_Lib_Add.Focus()
                        Exit Sub
                    End If
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Enter Address... ');", True)
                    Me.txt_Lib_Add.Focus()
                    Exit Sub
                End If

                'Server Validation for Address in local lang
                Dim LibAdd2 As String = Nothing
                LibAdd2 = TrimAll(txt_Lib_Add2.Text)
                If Not String.IsNullOrEmpty(LibAdd2) Then
                    LibAdd2 = RemoveQuotes(LibAdd2)
                    If LibAdd2.Length > 256 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Lib_Add2.Focus()
                        Exit Sub
                    End If
                    LibAdd2 = " " & LibAdd2 & " "
                    If InStr(1, LibAdd2, "CREATE", 1) > 0 Or InStr(1, LibAdd2, "DELETE", 1) > 0 Or InStr(1, LibAdd2, "DROP", 1) > 0 Or InStr(1, LibAdd2, "INSERT", 1) > 1 Or InStr(1, LibAdd2, "TRACK", 1) > 1 Or InStr(1, LibAdd2, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Lib_Add2.Focus()
                        Exit Sub
                    End If
                    LibAdd2 = TrimAll(LibAdd2)

                    'check unwanted characters
                    c = 0
                    counter7 = 0
                    For iloop = 1 To Len(LibAdd2)
                        strcurrentchar = Mid(LibAdd2, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter7 = 1
                            End If
                        End If
                    Next
                    If counter7 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Me.txt_Lib_Add2.Focus()
                        Exit Sub
                    End If
                Else
                    LibAdd2 = String.Empty
                End If

                '*******************************************************************************************************
                'Server Validation for City
                Dim LibCity As String = Nothing
                LibCity = TrimAll(txt_Lib_City.Text)
                If Not String.IsNullOrEmpty(LibCity) Then
                    LibCity = RemoveQuotes(LibCity)
                    If LibCity.Length > 101 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Lib_City.Focus()
                        Exit Sub
                    End If
                    LibCity = " " & LibCity & " "
                    If InStr(1, LibCity, "CREATE", 1) > 0 Or InStr(1, LibCity, "DELETE", 1) > 0 Or InStr(1, LibCity, "DROP", 1) > 0 Or InStr(1, LibCity, "INSERT", 1) > 1 Or InStr(1, LibCity, "TRACK", 1) > 1 Or InStr(1, LibCity, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Lib_City.Focus()
                        Exit Sub
                    End If
                    LibCity = TrimAll(LibCity)
                    'check unwanted characters
                    c = 0
                    counter8 = 0
                    For iloop = 1 To Len(LibCity)
                        strcurrentchar = Mid(LibCity, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter8 = 1
                            End If
                        End If
                    Next
                    If counter8 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Me.txt_Lib_City.Focus()
                        Exit Sub
                    End If
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Enter City Name... ');", True)
                    Me.txt_Lib_City.Focus()
                    Exit Sub
                End If

                'Server Validation for City in local lang
                Dim LibCity2 As String = Nothing
                LibCity2 = TrimAll(txt_Lib_City2.Text)
                If Not String.IsNullOrEmpty(LibCity2) Then
                    LibCity2 = RemoveQuotes(LibCity2)
                    If LibCity2.Length > 101 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Lib_City2.Focus()
                        Exit Sub
                    End If
                    LibCity2 = " " & LibCity2 & " "
                    If InStr(1, LibCity2, "CREATE", 1) > 0 Or InStr(1, LibCity2, "DELETE", 1) > 0 Or InStr(1, LibCity2, "DROP", 1) > 0 Or InStr(1, LibCity2, "INSERT", 1) > 1 Or InStr(1, LibCity2, "TRACK", 1) > 1 Or InStr(1, LibCity2, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Lib_City2.Focus()
                        Exit Sub
                    End If
                    LibCity2 = TrimAll(LibCity2)
                    'check unwanted characters
                    c = 0
                    counter9 = 0
                    For iloop = 1 To Len(LibCity2)
                        strcurrentchar = Mid(LibCity2, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter9 = 1
                            End If
                        End If
                    Next
                    If counter9 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Me.txt_Lib_City2.Focus()
                        Exit Sub
                    End If
                Else
                    LibCity2 = String.Empty
                End If

                '****************************************************************************************
                'Server Validation for Distict
                Dim LibDist As String = Nothing
                LibDist = TrimX(txt_Lib_District.Text)
                If Not String.IsNullOrEmpty(LibDist) Then
                    LibDist = RemoveQuotes(LibDist)
                    If LibDist.Length > 101 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Me.txt_Lib_District.Focus()
                        Exit Sub
                    End If
                    LibDist = " " & LibDist & " "
                    If InStr(1, LibDist, "CREATE", 1) > 0 Or InStr(1, LibDist, "DELETE", 1) > 0 Or InStr(1, LibDist, "DROP", 1) > 0 Or InStr(1, LibDist, "INSERT", 1) > 1 Or InStr(1, LibDist, "TRACK", 1) > 1 Or InStr(1, LibDist, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Lib_District.Focus()
                        Exit Sub
                    End If
                    'check unwanted characters
                    c = 0
                    counter10 = 0
                    For iloop = 1 To Len(LibDist)
                        strcurrentchar = Mid(LibDist, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';~!#$^&*|[]{}?<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter10 = 1
                            End If
                        End If
                    Next
                    If counter10 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Lib_District.Focus()
                        Exit Sub
                    End If
                Else
                    LibDist = ""
                End If

                'Server Validation for Distict in local lang
                Dim LibDist2 As String = Nothing
                LibDist2 = TrimX(txt_Lib_District2.Text)
                If Not String.IsNullOrEmpty(LibDist2) Then
                    LibDist2 = RemoveQuotes(LibDist2)
                    If LibDist2.Length > 101 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Me.txt_Lib_District2.Focus()
                        Exit Sub
                    End If
                    LibDist2 = " " & LibDist2 & " "
                    If InStr(1, LibDist2, "CREATE", 1) > 0 Or InStr(1, LibDist2, "DELETE", 1) > 0 Or InStr(1, LibDist2, "DROP", 1) > 0 Or InStr(1, LibDist2, "INSERT", 1) > 1 Or InStr(1, LibDist2, "TRACK", 1) > 1 Or InStr(1, LibDist2, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Lib_District2.Focus()
                        Exit Sub
                    End If
                    'check unwanted characters
                    c = 0
                    counter11 = 0
                    For iloop = 1 To Len(LibDist2)
                        strcurrentchar = Mid(LibDist2, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';~!#$^&*|[]{}?<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter11 = 1
                            End If
                        End If
                    Next
                    If counter11 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Lib_District2.Focus()
                        Exit Sub
                    End If
                Else
                    LibDist2 = ""
                End If

                'Server Validation for State ************************************************
                Dim LibState As String = Nothing
                LibState = TrimAll(txt_Lib_State.Text)
                If Not String.IsNullOrEmpty(LibState) Then
                    LibState = RemoveQuotes(LibState)
                    If LibState.Length > 101 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Data must be of Proper Length... ');", True)
                        txt_Lib_State.Focus()
                        Exit Sub
                    End If
                    LibState = " " & LibState & " "
                    If InStr(1, LibState, "CREATE", 1) > 0 Or InStr(1, LibState, "DELETE", 1) > 0 Or InStr(1, LibState, "DROP", 1) > 0 Or InStr(1, LibState, "INSERT", 1) > 1 Or InStr(1, LibState, "TRACK", 1) > 1 Or InStr(1, LibState, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Do Not Use Reserve Word... ');", True)
                        txt_Lib_State.Focus()
                        Exit Sub
                    End If
                    LibState = TrimAll(LibState)
                    c = 0
                    counter12 = 0
                    For iloop = 1 To Len(LibState)
                        strcurrentchar = Mid(LibState, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*+|[]{}<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter12 = 1
                            End If
                        End If
                    Next
                    If counter12 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Un-Wated Characters... ');", True)
                        txt_Lib_State.Focus()
                        Exit Sub
                    End If
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Enter State Name... ');", True)
                    Me.txt_Lib_State.Focus()
                    Exit Sub
                End If

                'Server Validation for State ************************************************
                Dim LibState2 As String = Nothing
                LibState2 = TrimAll(txt_Lib_State2.Text)
                If Not String.IsNullOrEmpty(LibState2) Then
                    LibState2 = RemoveQuotes(LibState2)
                    If LibState2.Length > 101 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Data must be of Proper Length... ');", True)
                        txt_Lib_State2.Focus()
                        Exit Sub
                    End If
                    LibState2 = " " & LibState2 & " "
                    If InStr(1, LibState2, "CREATE", 1) > 0 Or InStr(1, LibState2, "DELETE", 1) > 0 Or InStr(1, LibState2, "DROP", 1) > 0 Or InStr(1, LibState2, "INSERT", 1) > 1 Or InStr(1, LibState2, "TRACK", 1) > 1 Or InStr(1, LibState2, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Do Not Use Reserve Word... ');", True)
                        txt_Lib_State2.Focus()
                        Exit Sub
                    End If
                    LibState2 = TrimAll(LibState2)
                    c = 0
                    counter13 = 0
                    For iloop = 1 To Len(LibState2)
                        strcurrentchar = Mid(LibState2, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*+|[]{}<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter13 = 1
                            End If
                        End If
                    Next
                    If counter13 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Un-Wated Characters... ');", True)
                        txt_Lib_State2.Focus()
                        Exit Sub
                    End If
                Else
                    LibState2 = ""
                End If

                '********************************************************************************************************
                'Server Validation for Phone Number
                Dim Phone As String = Nothing
                Phone = TrimAll(txt_Lib_Phone.Text)
                If Not String.IsNullOrEmpty(Phone) Then
                    Phone = RemoveQuotes(Phone)
                    If Phone.Length > 101 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Lib_Phone.Focus()
                        Exit Sub
                    End If
                    Phone = " " & Phone & " "
                    If InStr(1, Phone, "CREATE", 1) > 0 Or InStr(1, Phone, "DELETE", 1) > 0 Or InStr(1, Phone, "DROP", 1) > 0 Or InStr(1, Phone, "INSERT", 1) > 1 Or InStr(1, Phone, "TRACK", 1) > 1 Or InStr(1, Phone, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Lib_Phone.Focus()
                        Exit Sub
                    End If
                    Phone = TrimAll(Phone)

                    'check unwanted characters
                    c = 0
                    counter14 = 0
                    For iloop = 1 To Len(Phone)
                        strcurrentchar = Mid(Phone, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter14 = 1
                            End If
                        End If
                    Next
                    If counter14 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Me.txt_Lib_Phone.Focus()
                        Exit Sub
                    End If
                Else
                    Phone = String.Empty
                End If

                '********************************************************************************************************
                'Server Validation for fax Number
                Dim Fax As String = Nothing
                Fax = TrimAll(txt_Lib_Fax.Text)
                If Not String.IsNullOrEmpty(Fax) Then
                    Fax = RemoveQuotes(Fax)
                    If Fax.Length > 101 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Lib_Fax.Focus()
                        Exit Sub
                    End If
                    Fax = " " & Fax & " "
                    If InStr(1, Fax, "CREATE", 1) > 0 Or InStr(1, Fax, "DELETE", 1) > 0 Or InStr(1, Fax, "DROP", 1) > 0 Or InStr(1, Fax, "INSERT", 1) > 1 Or InStr(1, Fax, "TRACK", 1) > 1 Or InStr(1, Fax, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Lib_Fax.Focus()
                        Exit Sub
                    End If
                    Fax = TrimAll(Fax)

                    'check unwanted characters
                    c = 0
                    counter15 = 0
                    For iloop = 1 To Len(Fax)
                        strcurrentchar = Mid(Fax, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter15 = 1
                            End If
                        End If
                    Next
                    If counter15 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Me.txt_Lib_Fax.Focus()
                        Exit Sub
                    End If
                Else
                    Fax = String.Empty
                End If

                '****************************************************************************************
                'Server Validation for E-Mail Address
                Dim email As String = Nothing
                email = TrimX(txt_Lib_Email.Text)
                If Not String.IsNullOrEmpty(email) Then
                    email = RemoveQuotes(email)
                    If email.Length > 201 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Me.txt_Lib_Email.Focus()
                        Exit Sub
                    End If
                    email = " " & email & " "
                    If InStr(1, email, "CREATE", 1) > 0 Or InStr(1, email, "DELETE", 1) > 0 Or InStr(1, email, "DROP", 1) > 0 Or InStr(1, email, "INSERT", 1) > 1 Or InStr(1, email, "TRACK", 1) > 1 Or InStr(1, email, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Lib_Email.Focus()
                        Exit Sub
                    End If
                    email = TrimX(email)
                    'check unwanted characters
                    c = 0
                    counter16 = 0
                    For iloop = 1 To Len(email)
                        strcurrentchar = Mid(email, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';~!#$^&*|[]{}?<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter16 = 1
                            End If
                        End If
                    Next
                    If counter16 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Lib_Email.Focus()
                        Exit Sub
                    End If
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Enter Proper Email... ');", True)
                    Me.txt_Lib_Email.Focus()
                    Exit Sub
                End If

                '****************************************************************************************
                'Server Validation for E-Mail Address
                Dim url As String = Nothing
                url = TrimX(txt_Lib_URL.Text)
                If Not String.IsNullOrEmpty(url) Then
                    url = RemoveQuotes(url)
                    If url.Length > 256 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Me.txt_Lib_URL.Focus()
                        Exit Sub
                    End If
                    url = " " & url & " "
                    If InStr(1, url, "CREATE", 1) > 0 Or InStr(1, url, "DELETE", 1) > 0 Or InStr(1, url, "DROP", 1) > 0 Or InStr(1, url, "INSERT", 1) > 1 Or InStr(1, url, "TRACK", 1) > 1 Or InStr(1, url, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Lib_URL.Focus()
                        Exit Sub
                    End If
                    url = TrimX(url)
                    'check unwanted characters
                    c = 0
                    counter17 = 0
                    For iloop = 1 To Len(url)
                        strcurrentchar = Mid(url, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';~!#$^&*|[]{}?<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter17 = 1
                            End If
                        End If
                    Next
                    If counter17 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Lib_URL.Focus()
                        Exit Sub
                    End If
                    If InStr(url, "http://") = 0 Then
                        url = "http://" & url
                    End If
                Else
                    url = ""
                End If

                '****************************************************************************************
                'Server Validation for parent URL Address
                Dim Prnturl As String = Nothing
                Prnturl = TrimX(txt_Lib_ParentURL.Text)
                If Not String.IsNullOrEmpty(Prnturl) Then
                    Prnturl = RemoveQuotes(Prnturl)
                    If Prnturl.Length > 256 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Me.txt_Lib_ParentURL.Focus()
                        Exit Sub
                    End If
                    Prnturl = " " & Prnturl & " "
                    If InStr(1, Prnturl, "CREATE", 1) > 0 Or InStr(1, Prnturl, "DELETE", 1) > 0 Or InStr(1, Prnturl, "DROP", 1) > 0 Or InStr(1, Prnturl, "INSERT", 1) > 1 Or InStr(1, Prnturl, "TRACK", 1) > 1 Or InStr(1, Prnturl, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Lib_ParentURL.Focus()
                        Exit Sub
                    End If
                    Prnturl = TrimX(Prnturl)
                    'check unwanted characters
                    c = 0
                    counter18 = 0
                    For iloop = 1 To Len(Prnturl)
                        strcurrentchar = Mid(Prnturl, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';~!#$^&*|[]{}?<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter18 = 1
                            End If
                        End If
                    Next

                    If counter18 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Input is not Valid... ');", True)
                        Me.txt_Lib_ParentURL.Focus()
                        Exit Sub
                    End If
                    If InStr(Prnturl, "http://") = 0 Then
                        Prnturl = "http://" & Prnturl
                    End If
                Else
                    Prnturl = ""
                End If

                'Server Validation for library InCharge URL Address
                Dim LibIncharge As String = Nothing
                LibIncharge = TrimX(txt_Lib_Incharge.Text)
                If Not String.IsNullOrEmpty(LibIncharge) Then
                    LibIncharge = RemoveQuotes(LibIncharge)
                    If LibIncharge.Length > 251 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Me.txt_Lib_Incharge.Focus()
                        Exit Sub
                    End If
                    LibIncharge = " " & LibIncharge & " "
                    If InStr(1, LibIncharge, "CREATE", 1) > 0 Or InStr(1, LibIncharge, "DELETE", 1) > 0 Or InStr(1, LibIncharge, "DROP", 1) > 0 Or InStr(1, LibIncharge, "INSERT", 1) > 1 Or InStr(1, LibIncharge, "TRACK", 1) > 1 Or InStr(1, LibIncharge, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Lib_Incharge.Focus()
                        Exit Sub
                    End If
                    LibIncharge = TrimAll(LibIncharge)
                    'check unwanted characters
                    c = 0
                    counter19 = 0
                    For iloop = 1 To Len(LibIncharge)
                        strcurrentchar = Mid(LibIncharge, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';~!#$^&*|[]{}?<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter19 = 1
                            End If
                        End If
                    Next

                    If counter19 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Input is not Valid... ');", True)
                        Me.txt_Lib_Incharge.Focus()
                        Exit Sub
                    End If
                Else
                    LibIncharge = ""
                End If
               
                'Server Validation for library InCharge in local lang
                Dim LibIncharge2 As String = Nothing
                LibIncharge2 = TrimX(txt_Lib_Incharge2.Text)
                If Not String.IsNullOrEmpty(LibIncharge2) Then
                    LibIncharge2 = RemoveQuotes(LibIncharge2)
                    If LibIncharge2.Length > 251 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Me.txt_Lib_Incharge2.Focus()
                        Exit Sub
                    End If
                    LibIncharge2 = " " & LibIncharge2 & " "
                    If InStr(1, LibIncharge2, "CREATE", 1) > 0 Or InStr(1, LibIncharge2, "DELETE", 1) > 0 Or InStr(1, LibIncharge2, "DROP", 1) > 0 Or InStr(1, LibIncharge2, "INSERT", 1) > 1 Or InStr(1, LibIncharge2, "TRACK", 1) > 1 Or InStr(1, LibIncharge2, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Lib_Incharge2.Focus()
                        Exit Sub
                    End If
                    LibIncharge2 = TrimAll(LibIncharge2)
                    'check unwanted characters
                    c = 0
                    counter20 = 0
                    For iloop = 1 To Len(LibIncharge2)
                        strcurrentchar = Mid(LibIncharge2, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';~!#$^&*|[]{}?<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter20 = 1
                            End If
                        End If
                    Next
                    If counter20 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Input is not Valid... ');", True)
                        Me.txt_Lib_Incharge2.Focus()
                        Exit Sub
                    End If
                Else
                    LibIncharge2 = ""
                End If

                'Server Validation for library timing
                Dim LibTime As String = Nothing
                LibTime = TrimAll(txt_Lib_Timing.Text)
                If Not String.IsNullOrEmpty(LibTime) Then
                    LibTime = RemoveQuotes(LibTime)
                    If LibTime.Length > 256 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Me.txt_Lib_Timing.Focus()
                        Exit Sub
                    End If
                    LibTime = " " & LibTime & " "
                    If InStr(1, LibTime, "CREATE", 1) > 0 Or InStr(1, LibTime, "DELETE", 1) > 0 Or InStr(1, LibTime, "DROP", 1) > 0 Or InStr(1, LibTime, "INSERT", 1) > 1 Or InStr(1, LibTime, "TRACK", 1) > 1 Or InStr(1, LibTime, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Lib_Timing.Focus()
                        Exit Sub
                    End If
                    LibTime = TrimAll(LibTime)
                    'check unwanted characters
                    c = 0
                    counter21 = 0
                    For iloop = 1 To Len(LibTime)
                        strcurrentchar = Mid(LibTime, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';~!#$^&*|[]{}?<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter21 = 1
                            End If
                        End If
                    Next

                    If counter21 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Input is not Valid... ');", True)
                        Me.txt_Lib_Timing.Focus()
                        Exit Sub
                    End If
                Else
                    LibTime = ""
                End If
               
                'Server Validation for library SMS UID
                Dim SMSUID As String = Nothing
                SMSUID = TrimX(txt_Lib_SMSuid.Text)
                If Not String.IsNullOrEmpty(SMSUID) Then
                    SMSUID = RemoveQuotes(SMSUID)
                    If SMSUID.Length > 150 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Me.txt_Lib_SMSuid.Focus()
                        Exit Sub
                    End If
                    SMSUID = " " & SMSUID & " "
                    If InStr(1, SMSUID, "CREATE", 1) > 0 Or InStr(1, SMSUID, "DELETE", 1) > 0 Or InStr(1, SMSUID, "DROP", 1) > 0 Or InStr(1, SMSUID, "INSERT", 1) > 1 Or InStr(1, SMSUID, "TRACK", 1) > 1 Or InStr(1, SMSUID, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Lib_SMSuid.Focus()
                        Exit Sub
                    End If
                    SMSUID = TrimAll(SMSUID)
                    'check unwanted characters
                    c = 0
                    counter22 = 0
                    For iloop = 1 To Len(SMSUID)
                        strcurrentchar = Mid(SMSUID, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';~!#$^&*|[]{}?<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter22 = 1
                            End If
                        End If
                    Next

                    If counter22 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Input is not Valid... ');", True)
                        Me.txt_Lib_SMSuid.Focus()
                        Exit Sub
                    End If
                Else
                    SMSUID = ""
                End If
                
                'Server Validation for library SMS pw
                Dim SMSPw As String = Nothing
                SMSPw = TrimX(txt_Lib_SMSpw.Text)
                If Not String.IsNullOrEmpty(SMSPw) Then
                    SMSPw = RemoveQuotes(SMSPw)
                    If SMSPw.Length > 150 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Me.txt_Lib_SMSpw.Focus()
                        Exit Sub
                    End If
                    SMSPw = " " & SMSPw & " "
                    If InStr(1, SMSPw, "CREATE", 1) > 0 Or InStr(1, SMSPw, "DELETE", 1) > 0 Or InStr(1, SMSPw, "DROP", 1) > 0 Or InStr(1, SMSPw, "INSERT", 1) > 1 Or InStr(1, SMSPw, "TRACK", 1) > 1 Or InStr(1, SMSPw, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Lib_SMSpw.Focus()
                        Exit Sub
                    End If
                    SMSPw = TrimAll(SMSPw)
                    'check unwanted characters
                    c = 0
                    counter23 = 0
                    For iloop = 1 To Len(SMSPw)
                        strcurrentchar = Mid(SMSPw, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';~!#$^&*|[]{}?<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter23 = 1
                            End If
                        End If
                    Next
                    If counter23 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Input is not Valid... ');", True)
                        Me.txt_Lib_SMSpw.Focus()
                        Exit Sub
                    End If
                Else
                    SMSPw = ""
                End If

                'Server Validation for library SMS sender
                Dim SMSsender As String = Nothing
                SMSsender = TrimAll(txt_Lib_SMSsender.Text)
                If Not String.IsNullOrEmpty(SMSsender) Then
                    SMSsender = RemoveQuotes(SMSsender)
                    If SMSsender.Length > 150 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Me.txt_Lib_SMSsender.Focus()
                        Exit Sub
                    End If
                    SMSsender = " " & SMSsender & " "
                    If InStr(1, SMSsender, "CREATE", 1) > 0 Or InStr(1, SMSsender, "DELETE", 1) > 0 Or InStr(1, SMSsender, "DROP", 1) > 0 Or InStr(1, SMSsender, "INSERT", 1) > 1 Or InStr(1, SMSsender, "TRACK", 1) > 1 Or InStr(1, SMSsender, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Lib_SMSsender.Focus()
                        Exit Sub
                    End If
                    SMSsender = TrimAll(SMSsender)
                    'check unwanted characters
                    c = 0
                    counter24 = 0
                    For iloop = 1 To Len(SMSsender)
                        strcurrentchar = Mid(SMSsender, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';~!#$^&*|[]{}?<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter24 = 1
                            End If
                        End If
                    Next
                    If counter24 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Input is not Valid... ');", True)
                        Me.txt_Lib_SMSsender.Focus()
                        Exit Sub
                    End If
                Else
                    SMSsender = ""
                End If

                'Server Validation for library SMS IP
                Dim SMSIP As String = Nothing
                SMSIP = TrimAll(txt_Lib_SMSip.Text)
                If Not String.IsNullOrEmpty(SMSsender) Then
                    SMSIP = RemoveQuotes(SMSIP)
                    If SMSsender.Length > 150 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Me.txt_Lib_SMSip.Focus()
                        Exit Sub
                    End If
                    SMSIP = " " & SMSIP & " "
                    If InStr(1, SMSIP, "CREATE", 1) > 0 Or InStr(1, SMSIP, "DELETE", 1) > 0 Or InStr(1, SMSIP, "DROP", 1) > 0 Or InStr(1, SMSIP, "INSERT", 1) > 1 Or InStr(1, SMSIP, "TRACK", 1) > 1 Or InStr(1, SMSIP, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Lib_SMSip.Focus()
                        Exit Sub
                    End If
                    SMSIP = TrimAll(SMSIP)
                    'check unwanted characters
                    c = 0
                    counter25 = 0
                    For iloop = 1 To Len(SMSIP)
                        strcurrentchar = Mid(SMSIP, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';~!#$^&*|[]{}?<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter25 = 1
                            End If
                        End If
                    Next
                    If counter25 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Input is not Valid... ');", True)
                        Me.txt_Lib_SMSip.Focus()
                        Exit Sub
                    End If
                Else
                    SMSIP = ""
                End If

                'Server Validation for library SMS IP
                Dim BarCode As String = Nothing
                BarCode = TrimAll(txt_Lib_Barcode.Text)
                If Not String.IsNullOrEmpty(BarCode) Then
                    BarCode = RemoveQuotes(BarCode)
                    If BarCode.Length > 150 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Me.txt_Lib_Barcode.Focus()
                        Exit Sub
                    End If
                    BarCode = " " & BarCode & " "
                    If InStr(1, BarCode, "CREATE", 1) > 0 Or InStr(1, BarCode, "DELETE", 1) > 0 Or InStr(1, BarCode, "DROP", 1) > 0 Or InStr(1, BarCode, "INSERT", 1) > 1 Or InStr(1, BarCode, "TRACK", 1) > 1 Or InStr(1, BarCode, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Lib_Barcode.Focus()
                        Exit Sub
                    End If
                    BarCode = TrimAll(BarCode)
                    'check unwanted characters
                    c = 0
                    counter26 = 0
                    For iloop = 1 To Len(BarCode)
                        strcurrentchar = Mid(BarCode, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';~!#$^&*|[]{}?<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter26 = 1
                            End If
                        End If
                    Next
                    If counter26 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Input is not Valid... ');", True)
                        Me.txt_Lib_Barcode.Focus()
                        Exit Sub
                    End If
                Else
                    BarCode = ""
                End If
                '**************************************************************************************
                'Server Validation for Classification Scheme
                Dim ClassScheme As String = Nothing
                ClassScheme = DropDownList1.SelectedValue
                If Not String.IsNullOrEmpty(ClassScheme) Then
                    ClassScheme = RemoveQuotes(ClassScheme)
                    If ClassScheme.Length > 11 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input not Valid... ');", True)
                        DropDownList1.Focus()
                        Exit Sub
                    End If
                    If InStr(1, ClassScheme, "CREATE", 1) > 0 Or InStr(1, ClassScheme, "DELETE", 1) > 0 Or InStr(1, ClassScheme, "DROP", 1) > 0 Or InStr(1, ClassScheme, "INSERT", 1) > 1 Or InStr(1, ClassScheme, "TRACK", 1) > 1 Or InStr(1, ClassScheme, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        DropDownList1.Focus()
                        Exit Sub
                    End If
                    ClassScheme = TrimAll(ClassScheme)
                    'check unwanted characters
                    c = 0
                    counter27 = 0
                    For iloop = 1 To Len(ClassScheme)
                        strcurrentchar = Mid(ClassScheme, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter27 = 1
                            End If
                        End If
                    Next
                    If counter27 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                        DropDownList1.Focus()
                        Exit Sub
                    End If
                Else
                    ClassScheme = "DDC"
                End If

                counter27 = Nothing
                'Server Validation for ACQ_RETRO
                Dim ACQ_RETRO As Object = Nothing
                ACQ_RETRO = DDL_Acq.SelectedValue
                If Not String.IsNullOrEmpty(ACQ_RETRO) Then
                    ClassScheme = RemoveQuotes(ACQ_RETRO)
                    If ACQ_RETRO.Length > 2 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input not Valid... ');", True)
                        DDL_Acq.Focus()
                        Exit Sub
                    End If
                    If InStr(1, ACQ_RETRO, "CREATE", 1) > 0 Or InStr(1, ACQ_RETRO, "DELETE", 1) > 0 Or InStr(1, ACQ_RETRO, "DROP", 1) > 0 Or InStr(1, ACQ_RETRO, "INSERT", 1) > 1 Or InStr(1, ACQ_RETRO, "TRACK", 1) > 1 Or InStr(1, ACQ_RETRO, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        DDL_Acq.Focus()
                        Exit Sub
                    End If
                    ACQ_RETRO = TrimAll(ACQ_RETRO)
                    'check unwanted characters
                    c = 0
                    counter27 = 0
                    For iloop = 1 To Len(ACQ_RETRO)
                        strcurrentchar = Mid(ACQ_RETRO, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter27 = 1
                            End If
                        End If
                    Next
                    If counter27 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                        DDL_Acq.Focus()
                        Exit Sub
                    End If
                Else
                    ACQ_RETRO = "N"
                End If

                '**************************************************************************************
                'Server Validation for Cataloging  Scheme
                Dim CatScheme As String = Nothing
                CatScheme = DropDownList2.SelectedValue
                If Not String.IsNullOrEmpty(CatScheme) Then
                    CatScheme = RemoveQuotes(CatScheme)
                    If CatScheme.Length > 11 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input not Valid... ');", True)
                        DropDownList2.Focus()
                        Exit Sub
                    End If
                    If InStr(1, CatScheme, "CREATE", 1) > 0 Or InStr(1, CatScheme, "DELETE", 1) > 0 Or InStr(1, CatScheme, "DROP", 1) > 0 Or InStr(1, CatScheme, "INSERT", 1) > 1 Or InStr(1, CatScheme, "TRACK", 1) > 1 Or InStr(1, CatScheme, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not User Reserve Words.. ');", True)
                        DropDownList2.Focus()
                        Exit Sub
                    End If
                    CatScheme = TrimAll(CatScheme)
                    'check unwanted characters
                    c = 0
                    counter28 = 0
                    For iloop = 1 To Len(CatScheme)
                        strcurrentchar = Mid(CatScheme, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter28 = 1
                            End If
                        End If
                    Next
                    If counter28 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                        DropDownList2.Focus()
                        Exit Sub
                    End If
                Else
                    CatScheme = "DDC"
                End If

                '****************************************************************************************88
                'remareks
                Dim myRemarks As String = Nothing
                myRemarks = Trim(Me.txt_Lib_Remarks.Text)
                If Not String.IsNullOrEmpty(myRemarks) Then
                    myRemarks = RemoveQuotes(myRemarks)
                    If myRemarks.Length > 8000 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input length is not Proper.. ');", True)
                        Me.txt_Lib_Remarks.Focus()
                        Exit Sub
                    End If
                Else
                    myRemarks = String.Empty
                End If

                '****************************************************************************************88
                'introduction 
                Dim LibIntro As String = Nothing
                LibIntro = Trim(Me.txt_Lib_Intro.Text)
                If Not String.IsNullOrEmpty(LibIntro) Then
                    LibIntro = RemoveQuotes(LibIntro)
                    If LibIntro.Length > 8000 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input length is not Proper.. ');", True)
                        Me.txt_Lib_Intro.Focus()
                        Exit Sub
                    End If
                Else
                    LibIntro = String.Empty
                End If

                '****************************************************************************************88
                'introduction in local lang
                Dim LibIntro2 As String = Nothing
                LibIntro2 = Trim(Me.txt_Lib_Intro2.Text)
                If Not String.IsNullOrEmpty(LibIntro2) Then
                    LibIntro2 = RemoveQuotes(LibIntro2)
                    If LibIntro2.Length > 8000 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input length is not Proper.. ');", True)
                        Me.txt_Lib_Intro2.Focus()
                        Exit Sub
                    End If
                Else
                    LibIntro2 = String.Empty
                End If

                'objective 
                Dim LibObjective As String = Nothing
                LibObjective = Trim(Me.txt_Lib_Objective.Text)
                If Not String.IsNullOrEmpty(LibObjective) Then
                    LibObjective = RemoveQuotes(LibObjective)
                    If LibObjective.Length > 8000 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input length is not Proper.. ');", True)
                        Me.txt_Lib_Objective.Focus()
                        Exit Sub
                    End If
                Else
                    LibObjective = String.Empty
                End If

                'objective in local lang 
                Dim LibObjective2 As String = Nothing
                LibObjective2 = Trim(Me.txt_Lib_Objective2.Text)
                If Not String.IsNullOrEmpty(LibObjective2) Then
                    LibObjective2 = RemoveQuotes(LibObjective2)
                    If LibObjective2.Length > 8000 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input length is not Proper.. ');", True)
                        Me.txt_Lib_Objective2.Focus()
                        Exit Sub
                    End If
                Else
                    LibObjective2 = String.Empty
                End If

                'History  
                Dim LibHist As String = Nothing
                LibHist = Trim(Me.txt_Lib_History.Text)
                If Not String.IsNullOrEmpty(LibHist) Then
                    LibHist = RemoveQuotes(LibHist)
                    If LibHist.Length > 8000 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input length is not Proper.. ');", True)
                        Me.txt_Lib_History.Focus()
                        Exit Sub
                    End If
                Else
                    LibHist = String.Empty
                End If

                'History  in local lang
                Dim LibHist2 As String = Nothing
                LibHist2 = Trim(Me.txt_Lib_History2.Text)
                If Not String.IsNullOrEmpty(LibHist2) Then
                    LibHist2 = RemoveQuotes(LibHist2)
                    If LibHist2.Length > 8000 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input length is not Proper.. ');", True)
                        Me.txt_Lib_History2.Focus()
                        Exit Sub
                    End If
                Else
                    LibHist2 = String.Empty
                End If

                'Lib Services  in local lang
                Dim LibService As String = Nothing
                LibService = Trim(Me.txt_Lib_Services.Text)
                If Not String.IsNullOrEmpty(LibService) Then
                    LibService = RemoveQuotes(LibService)
                    If LibService.Length > 8000 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input length is not Proper.. ');", True)
                        Me.txt_Lib_Services.Focus()
                        Exit Sub
                    End If
                Else
                    LibService = String.Empty
                End If


                'Lib Services  in local lang
                Dim LibService2 As String = Nothing
                LibService2 = Trim(Me.txt_Lib_Services2.Text)
                If Not String.IsNullOrEmpty(LibService2) Then
                    LibService2 = RemoveQuotes(LibService2)
                    If LibService2.Length > 8000 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input length is not Proper.. ');", True)
                        Me.txt_Lib_Services2.Focus()
                        Exit Sub
                    End If
                Else
                    LibService2 = String.Empty
                End If
                '*******************************************************************************************************
                'upload library photo
                Dim arrContent2 As Byte()
                Dim intLength2Photo As Integer = 0
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
                    intLength2Photo = Convert.ToInt32(FileUpload13.PostedFile.InputStream.Length)
                    ReDim arrContent2(intLength2Photo)
                    FileUpload13.PostedFile.InputStream.Read(arrContent2, 0, intLength2Photo)
                End If

                'upload library logo
                Dim arrContent3 As Byte()
                Dim intLength2Logo As Integer = 0
                If FileUpload1.FileName = "" Then
                    '    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Select Proper file... ');", True)
                    '    Me.FileUpload12.Focus()
                    '    Exit Sub
                Else
                    Dim fileName As String = FileUpload1.PostedFile.FileName
                    Dim ext As String = fileName.Substring(fileName.LastIndexOf("."))
                    ext = ext.ToLower
                    Dim imgType = FileUpload1.PostedFile.ContentType

                    If ext = ".jpg" Then

                    ElseIf ext = ".bmp" Then

                    ElseIf ext = ".gif" Then

                    ElseIf ext = "jpg" Then

                    ElseIf ext = "bmp" Then

                    ElseIf ext = "gif" Then

                    Else
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Select Only gif, bmp, or jpg format files supported... ');", True)
                        Me.FileUpload1.Focus()
                        Exit Sub
                    End If
                    intLength2Logo = Convert.ToInt32(FileUpload1.PostedFile.InputStream.Length)
                    ReDim arrContent3(intLength2Logo)
                    FileUpload1.PostedFile.InputStream.Read(arrContent3, 0, intLength2Logo)
                End If

                '****************************************************************************************************
                'UPDATE THE LIBRARY PROFILE   
                SQL = "SELECT * FROM LIBRARIES WHERE (LIB_CODE='" & Trim(newLbCode) & "')"
                SqlConn.Open()
                da = New SqlDataAdapter(SQL, SqlConn)
                cb = New SqlCommandBuilder(da)
                da.Fill(ds, "LIBRARIES")

                If ds.Tables("LIBRARIES").Rows.Count <> 0 Then
                    If Not String.IsNullOrEmpty(LbName) Then
                        ds.Tables("LIBRARIES").Rows(0)("LIB_NAME") = LbName.Trim
                    Else
                        ds.Tables("LIBRARIES").Rows(0)("LIB_NAME") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(LbName2) Then
                        ds.Tables("LIBRARIES").Rows(0)("LIB_NAME2") = LbName2.Trim
                    Else
                        ds.Tables("LIBRARIES").Rows(0)("LIB_NAME2") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(NewParent) Then
                        ds.Tables("LIBRARIES").Rows(0)("PARENT_BODY") = NewParent.Trim
                    Else
                        ds.Tables("LIBRARIES").Rows(0)("PARENT_BODY") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(NewParent2) Then
                        ds.Tables("LIBRARIES").Rows(0)("PARENT_BODY2") = NewParent2.Trim
                    Else
                        ds.Tables("LIBRARIES").Rows(0)("PARENT_BODY2") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(LibAdd) Then
                        ds.Tables("LIBRARIES").Rows(0)("LIB_ADDRESS") = LibAdd.Trim
                    Else
                        ds.Tables("LIBRARIES").Rows(0)("LIB_ADDRESS") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(LibAdd2) Then
                        ds.Tables("LIBRARIES").Rows(0)("LIB_ADDRESS2") = LibAdd2.Trim
                    Else
                        ds.Tables("LIBRARIES").Rows(0)("LIB_ADDRESS2") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(LibCity) Then
                        ds.Tables("LIBRARIES").Rows(0)("LIB_CITY") = LibCity.Trim
                    Else
                        ds.Tables("LIBRARIES").Rows(0)("LIB_CITY") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(LibCity2) Then
                        ds.Tables("LIBRARIES").Rows(0)("LIB_CITY2") = LibCity2.Trim
                    Else
                        ds.Tables("LIBRARIES").Rows(0)("LIB_CITY2") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(LibDist) Then
                        ds.Tables("LIBRARIES").Rows(0)("LIB_DISTRICT") = LibDist.Trim
                    Else
                        ds.Tables("LIBRARIES").Rows(0)("LIB_DISTRICT") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(LibDist2) Then
                        ds.Tables("LIBRARIES").Rows(0)("LIB_DISTRICT2") = LibDist2.Trim
                    Else
                        ds.Tables("LIBRARIES").Rows(0)("LIB_DISTRICT2") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(LibState) Then
                        ds.Tables("LIBRARIES").Rows(0)("LIB_STATE") = LibState.Trim
                    Else
                        ds.Tables("LIBRARIES").Rows(0)("LIB_STATE") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(LibState2) Then
                        ds.Tables("LIBRARIES").Rows(0)("LIB_STATE2") = LibState2.Trim
                    Else
                        ds.Tables("LIBRARIES").Rows(0)("LIB_STATE2") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(Phone) Then
                        ds.Tables("LIBRARIES").Rows(0)("LIB_PHONE") = Phone.Trim
                    Else
                        ds.Tables("LIBRARIES").Rows(0)("LIB_PHONE") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(Fax) Then
                        ds.Tables("LIBRARIES").Rows(0)("LIB_FAX") = Fax.Trim
                    Else
                        ds.Tables("LIBRARIES").Rows(0)("LIB_FAX") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(email) Then
                        ds.Tables("LIBRARIES").Rows(0)("LIB_EMAIL") = email.Trim
                    Else
                        ds.Tables("LIBRARIES").Rows(0)("LIB_EMAIL") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(url) Then
                        ds.Tables("LIBRARIES").Rows(0)("LIB_URL") = url.Trim
                    Else
                        ds.Tables("LIBRARIES").Rows(0)("LIB_URL") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(Prnturl) Then
                        ds.Tables("LIBRARIES").Rows(0)("PARENT_URL") = Prnturl.Trim
                    Else
                        ds.Tables("LIBRARIES").Rows(0)("PARENT_URL") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(LibIncharge) Then
                        ds.Tables("LIBRARIES").Rows(0)("LIB_INCHARGE") = LibIncharge.Trim
                    Else
                        ds.Tables("LIBRARIES").Rows(0)("LIB_INCHARGE") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(LibIncharge2) Then
                        ds.Tables("LIBRARIES").Rows(0)("LIB_INCHARGE2") = LibIncharge2.Trim
                    Else
                        ds.Tables("LIBRARIES").Rows(0)("LIB_INCHARGE2") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(LibTime) Then
                        ds.Tables("LIBRARIES").Rows(0)("LIB_TIMING") = LibTime.Trim
                    Else
                        ds.Tables("LIBRARIES").Rows(0)("LIB_TIMING") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(SMSUID) Then
                        ds.Tables("LIBRARIES").Rows(0)("SMS_UID") = SMSUID.Trim
                    Else
                        ds.Tables("LIBRARIES").Rows(0)("SMS_UID") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(SMSPw) Then
                        ds.Tables("LIBRARIES").Rows(0)("SMS_PW") = SMSPw.Trim
                    Else
                        ds.Tables("LIBRARIES").Rows(0)("SMS_PW") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(SMSsender) Then
                        ds.Tables("LIBRARIES").Rows(0)("SMS_SENDER") = SMSsender.Trim
                    Else
                        ds.Tables("LIBRARIES").Rows(0)("SMS_SENDER") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(SMSIP) Then
                        ds.Tables("LIBRARIES").Rows(0)("SMS_IP") = SMSIP.Trim
                    Else
                        ds.Tables("LIBRARIES").Rows(0)("SMS_IP") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(BarCode) Then
                        ds.Tables("LIBRARIES").Rows(0)("BARCODE_PRINTER") = BarCode.Trim
                    Else
                        ds.Tables("LIBRARIES").Rows(0)("BARCODE_PRINTER") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(ClassScheme) Then
                        ds.Tables("LIBRARIES").Rows(0)("CLASS_SCHEME") = ClassScheme.Trim
                    Else
                        ds.Tables("LIBRARIES").Rows(0)("CLASS_SCHEME") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(CatScheme) Then
                        ds.Tables("LIBRARIES").Rows(0)("CAT_CODE") = CatScheme.Trim
                    Else
                        ds.Tables("LIBRARIES").Rows(0)("CAT_CODE") = System.DBNull.Value
                    End If

                    If Not String.IsNullOrEmpty(ACQ_RETRO) Then
                        ds.Tables("LIBRARIES").Rows(0)("ACQ_RETRO") = ACQ_RETRO
                    Else
                        ds.Tables("LIBRARIES").Rows(0)("ACQ_RETRO") = System.DBNull.Value
                    End If

                    If Not String.IsNullOrEmpty(myRemarks) Then
                        ds.Tables("LIBRARIES").Rows(0)("REMARKS") = myRemarks.ToString.Trim
                    Else
                        ds.Tables("LIBRARIES").Rows(0)("REMARKS") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(LibIntro) Then
                        ds.Tables("LIBRARIES").Rows(0)("LIB_INTRO") = LibIntro.Trim
                    Else
                        ds.Tables("LIBRARIES").Rows(0)("LIB_INTRO") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(LibIntro2) Then
                        ds.Tables("LIBRARIES").Rows(0)("LIB_INTRO2") = LibIntro2.Trim
                    Else
                        ds.Tables("LIBRARIES").Rows(0)("LIB_INTRO2") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(LibObjective) Then
                        ds.Tables("LIBRARIES").Rows(0)("LIB_OBJECTIVE") = LibObjective.Trim
                    Else
                        ds.Tables("LIBRARIES").Rows(0)("LIB_OBJECTIVE") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(LibObjective2) Then
                        ds.Tables("LIBRARIES").Rows(0)("LIB_OBJECTIVE2") = LibObjective2.Trim
                    Else
                        ds.Tables("LIBRARIES").Rows(0)("LIB_OBJECTIVE2") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(LibHist) Then
                        ds.Tables("LIBRARIES").Rows(0)("LIB_HISTORY") = LibHist.Trim
                    Else
                        ds.Tables("LIBRARIES").Rows(0)("LIB_HISTORY") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(LibHist2) Then
                        ds.Tables("LIBRARIES").Rows(0)("LIB_HISTORY2") = LibHist2.Trim
                    Else
                        ds.Tables("LIBRARIES").Rows(0)("LIB_HISTORY2") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(LibService) Then
                        ds.Tables("LIBRARIES").Rows(0)("LIB_SERVICES") = LibService.Trim
                    Else
                        ds.Tables("LIBRARIES").Rows(0)("LIB_SERVICES") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(LibService2) Then
                        ds.Tables("LIBRARIES").Rows(0)("LIB_SERVICES2") = LibService2.Trim
                    Else
                        ds.Tables("LIBRARIES").Rows(0)("LIB_SERVICES2") = System.DBNull.Value
                    End If
                    ds.Tables("LIBRARIES").Rows(0)("USER_CODE") = UserCode
                    ds.Tables("LIBRARIES").Rows(0)("DATE_MODIFIED") = Now.Date
                    ds.Tables("LIBRARIES").Rows(0)("IP") = Request.UserHostAddress.Trim
                    If FileUpload13.FileName <> "" Then
                        ds.Tables("LIBRARIES").Rows(0)("LIB_PHOTO") = arrContent2
                    End If
                    If FileUpload1.FileName <> "" Then
                        ds.Tables("LIBRARIES").Rows(0)("LIB_LOGO") = arrContent3
                    End If

                    thisTransaction = SqlConn.BeginTransaction()
                    da.SelectCommand.Transaction = thisTransaction
                    da.Update(ds, "LIBRARIES")
                    thisTransaction.Commit()

                    newLbCode = Nothing
                    LbName = Nothing
                    LbName2 = Nothing
                    NewParent = Nothing
                    NewParent2 = Nothing
                    LibAdd = Nothing
                    LibAdd2 = Nothing
                    LibCity = Nothing
                    LibCity2 = Nothing
                    LibDist = Nothing
                    LibDist2 = Nothing
                    LibState = Nothing
                    LibState2 = Nothing
                    Phone = Nothing
                    Fax = Nothing
                    email = Nothing
                    url = Nothing
                    Prnturl = Nothing
                    LibIncharge = Nothing
                    LibIncharge2 = Nothing
                    LibTime = Nothing
                    SMSUID = Nothing
                    SMSPw = Nothing
                    SMSsender = Nothing
                    SMSIP = Nothing
                    BarCode = Nothing
                    ClassScheme = Nothing
                    CatScheme = Nothing
                    myRemarks = Nothing
                    LibIntro = Nothing
                    LibIntro2 = Nothing
                    LibObjective = Nothing
                    LibObjective2 = Nothing
                    LibHist = Nothing
                    LibHist2 = Nothing
                    LibService = Nothing
                    LibService2 = Nothing
                    Label6.Visible = True
                    Label6.Text = "Library Profile Updated Successfully"
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Library Profile Updated Successfully... ');", True)
                    ClearFields()
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Library Profile Update  - Please Contact System Administrator... ');", True)
                End If
            End If
                SqlConn.Close()
        Catch q As SqlException
            thisTransaction.Rollback()
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
            GetLibraryDetails()
        End Try
    End Sub
    Public Sub ClearFields()
        Me.txt_Lib_Name2.Text = ""
        Me.txt_Lib_City.Text = ""
        Me.txt_Lib_Add2.Text = ""
        txt_Lib_Name.Text = ""
        txt_Lib_Add.Text = ""
        txt_Lib_Remarks.Text = ""
    End Sub
End Class