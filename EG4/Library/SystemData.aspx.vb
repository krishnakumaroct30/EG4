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
Imports Microsoft.Office.Interop.Excel
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports.Engine.TextObject
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Shared
Imports System.Drawing.Printing
Imports CrystalDecisions
Public Class SystemData
    Inherits System.Web.UI.Page
    Dim ID As String = ""
    Dim CODE As String = ""
    Dim NAME As String = ""
    Dim DESC As String = ""
    Dim MYTABLE As String = ""
    Dim index As Integer = 0
    Public Srno As Integer = 0
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session.Item("IsThisU") <> Nothing Then
                If Not SConnection() = True Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Database Connection is lost..Try Again !');", True)
                Else
                    If Page.IsPostBack = False Then
                        Me.bttn_Save.Visible = True
                        Me.bttn_Update.Visible = False
                        Me.bttn_Delete.Visible = False
                        Grid1_SysData.DataSource = Nothing
                    End If
                    Dim CreateSUserButton As Object 'System.Web.UI.WebControls.Button
                    CreateSUserButton = Master.FindControl("Accordion1").FindControl("LibAdminPane").FindControl("Lib_SysData_Bttn")
                    CreateSUserButton.ForeColor = Drawing.Color.Red
                    myPaneName = "LibAdminPane"  'paneSelectedIndex = 0
                End If
            Else
                Response.Redirect("~/Default.aspx", False)
            End If
            GetmyTable()
        Catch ex As Exception
            Label6.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    Private Sub SystemData_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        DropDownList1.Focus()
    End Sub
    Public Sub GetmyTable()
        If DropDownList1.Text <> "" Then
            If DropDownList1.SelectedValue = "BIB_LEVELS" Then
                myTable = "BIB_LEVELS"
                ID = "BIB_ID"
                CODE = "BIB_CODE"
                NAME = "BIB_NAME"
                DESC = "BIB_DESC"
                MarcTag.Visible = False
                MatCodeTr.Visible = False
                BibLvlTr.Visible = False
                Label3.Text = "Add/Edit Bibliographic Levels"
            End If
            If DropDownList1.SelectedValue = "MATERIALS" Then
                myTable = "MATERIALS"
                ID = "MAT_ID"
                CODE = "MAT_CODE"
                NAME = "MAT_NAME"
                DESC = "MAT_DESC"
                MarcTag.Visible = False
                MatCodeTr.Visible = False
                BibLvlTr.Visible = True
                Label3.Text = "Add/Edit Materials Types"
            End If
            If DropDownList1.SelectedValue = "DOC_TYPES" Then
                myTable = "DOC_TYPES"
                ID = "DOC_TYPE_ID"
                CODE = "DOC_TYPE_CODE"
                NAME = "DOC_TYPE_NAME"
                DESC = "DOC_TYPE_DESC"
                MarcTag.Visible = False
                MatCodeTr.Visible = True
                BibLvlTr.Visible = True
                Label3.Text = "Add/Edit Documents Type"
            End If
            If DropDownList1.SelectedValue = "ACC_MATERIALS" Then
                MYTABLE = "ACC_MATERIALS"
                ID = "ACC_MAT_ID"
                CODE = "ACC_MAT_CODE"
                NAME = "ACC_MAT_NAME"
                DESC = "ACC_MAT_DESC"
                MarcTag.Visible = False
                MatCodeTr.Visible = False
                BibLvlTr.Visible = False
                Label3.Text = "Add/Edit Accompanying Materials"
            End If
            If DropDownList1.SelectedValue = "ACQMODES" Then
                MYTABLE = "ACQMODES"
                ID = "ACQMODE_ID"
                CODE = "ACQMODE_CODE"
                NAME = "ACQMODE_NAME"
                DESC = "ACQMODE_DESC"
                MarcTag.Visible = False
                MatCodeTr.Visible = False
                BibLvlTr.Visible = False
                Label3.Text = "Add/Edit Acquisition Modes"
            End If
            If DropDownList1.SelectedValue = "BOOKSTATUS" Then
                MYTABLE = "BOOKSTATUS"
                ID = "STA_ID"
                CODE = "STA_CODE"
                NAME = "STA_NAME"
                DESC = "STA_DESC"
                MarcTag.Visible = False
                MatCodeTr.Visible = False
                BibLvlTr.Visible = False
                Label3.Text = "Add/Edit Copy Status "
            End If
            If DropDownList1.SelectedValue = "BINDINGS" Then
                MYTABLE = "BINDINGS"
                ID = "BIND_ID"
                CODE = "BIND_CODE"
                NAME = "BIND_NAME"
                DESC = "BIND_DESC"
                MarcTag.Visible = False
                MatCodeTr.Visible = False
                BibLvlTr.Visible = False
                Label3.Text = "Add/Edit Type of Bindings "
            End If
            If DropDownList1.SelectedValue = "COUNTRIES" Then
                MYTABLE = "COUNTRIES"
                ID = "CON_ID"
                CODE = "CON_CODE"
                NAME = "CON_NAME"
                DESC = "CON_DESC"
                MarcTag.Visible = True
                MatCodeTr.Visible = False
                BibLvlTr.Visible = False
                Label3.Text = "Add/Edit Country Codes "
            End If
            If DropDownList1.SelectedValue = "CURRENCIES" Then
                MYTABLE = "CURRENCIES"
                ID = "CUR_ID"
                CODE = "CUR_CODE"
                NAME = "CUR_NAME"
                DESC = "CUR_DESC"
                MarcTag.Visible = False
                MatCodeTr.Visible = False
                BibLvlTr.Visible = False
                Label3.Text = "Add/Edit Currency Codes "
            End If
            If DropDownList1.SelectedValue = "FREQUENCIES" Then
                MYTABLE = "FREQUENCIES"
                ID = "FREQ_ID"
                CODE = "FREQ_CODE"
                NAME = "FREQ_NAME"
                DESC = "FREQ_DESC"
                MarcTag.Visible = False
                MatCodeTr.Visible = False
                BibLvlTr.Visible = False
                Label3.Text = "Add/Edit Serials Frequencies "
            End If
            If DropDownList1.SelectedValue = "LANGUAGES" Then
                MYTABLE = "LANGUAGES"
                ID = "LANG_ID"
                CODE = "LANG_CODE"
                NAME = "LANG_NAME"
                DESC = "LANG_DESC"
                MarcTag.Visible = True
                MatCodeTr.Visible = False
                BibLvlTr.Visible = False
                Label3.Text = "Add/Edit Language Codes "
            End If
            If DropDownList1.SelectedValue = "PHYSICAL_FORMATS" Then
                MYTABLE = "PHYSICAL_FORMATS"
                ID = "FORMAT_ID"
                CODE = "FORMAT_CODE"
                NAME = "FORMAT_NAME"
                DESC = "FORMAT_DESC"
                MarcTag.Visible = True
                MatCodeTr.Visible = False
                BibLvlTr.Visible = False
                Label3.Text = "Add/Edit Physcial Formats "
            End If
        Else
            DropDownList1.Focus()
        End If
    End Sub
    'populate bib levels
    Public Sub PopulateBibLevels()
        Dim Command As SqlCommand = Nothing
        Dim dt As DataTable = Nothing
        Try
            Command = New SqlCommand("SELECT BIB_ID, BIB_CODE, BIB_NAME FROM BIB_LEVELS ORDER BY BIB_NAME ", SqlConn)
            SqlConn.Open()
            Dim da As New SqlDataAdapter(Command)
            Dim ds As New DataSet
            Dim RecordCount As Long = 0
            da.Fill(ds)

            dt = ds.Tables(0).Copy

            Dim Dr As DataRow
            Dr = dt.NewRow
            Dr("BIB_CODE") = ""
            Dr("BIB_NAME") = ""
            dt.Rows.InsertAt(Dr, 0)

            If dt.Rows.Count = 0 Then
                Me.DropDownList3.DataSource = Nothing
            Else
                Me.DropDownList3.DataSource = dt
                Me.DropDownList3.DataTextField = "BIB_NAME"
                Me.DropDownList3.DataValueField = "BIB_CODE"
                Me.DropDownList3.DataBind()
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
    'populate materials type
    Public Sub DropDownList3_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownList3.SelectedIndexChanged
        If MYTABLE = "DOC_TYPES" Then
            Dim Command As SqlCommand = Nothing
            Dim dt As DataTable = Nothing
            Dim myBibLevl As String = Nothing
            Try
                If DropDownList3.Text <> "" Then
                    myBibLevl = DropDownList3.SelectedValue
                    Command = New SqlCommand("SELECT MAT_ID, MAT_CODE, MAT_NAME FROM MATERIALS WHERE (BIB_CODE ='" & Trim(myBibLevl) & "') ORDER BY MAT_NAME ", SqlConn)
                    SqlConn.Open()
                    Dim da As New SqlDataAdapter(Command)
                    Dim ds As New DataSet
                    Dim RecordCount As Long = 0
                    da.Fill(ds)

                    dt = ds.Tables(0).Copy

                    Dim Dr As DataRow
                    Dr = dt.NewRow
                    Dr("MAT_CODE") = ""
                    Dr("MAT_NAME") = ""
                    dt.Rows.InsertAt(Dr, 0)

                    If dt.Rows.Count = 0 Then
                        Me.DropDownList4.DataSource = Nothing
                    Else
                        Me.DropDownList4.DataSource = dt
                        Me.DropDownList4.DataTextField = "MAT_NAME"
                        Me.DropDownList4.DataValueField = "MAT_CODE"
                        Me.DropDownList4.DataBind()
                    End If
                    dt.Dispose()
                Else
                    DropDownList4.DataSource = Nothing
                    DropDownList4.Items.Clear()
                End If
            Catch s As Exception
                Label6.Text = "Error: " & (s.Message())
            Finally
                'UpdatePanel2.Update()
                SqlConn.Close()
            End Try
        End If
    End Sub
    'save record
    Protected Sub Save_Click(ByVal sender As Object, ByVal e As EventArgs) Handles bttn_Save.Click
        If IsPostBack = True Then
            'check unwanted characters
            Dim iloop As Integer
            Dim strcurrentchar As Object
            Dim c As Integer
            Dim counter1, counter2, counter3, counter4, Counter5, Counter6 As Integer
            Dim thisTransaction As SqlClient.SqlTransaction = Nothing
            Try
                If DropDownList1.SelectedValue = "BIB_LEVELS" Then
                    Label6.Text = "You can not add new Bibliographic Levels!"
                    Label3.Text = ""
                    Exit Sub
                End If
                If DropDownList1.SelectedValue = "MATERIALS" Then
                    Label6.Text = "You can not add new Materials Type!"
                    Label3.Text = ""
                    Exit Sub
                End If



                GetmyTable() ' get table structure
                'Server Validation for User Code
                Dim newCode As String = Nothing
                newCode = TrimX(txt_Sys_Code.Text)
                newCode = UCase(newCode)
                If Not String.IsNullOrEmpty(newCode) Then
                    newCode = RemoveQuotes(newCode)
                    If newCode.Length > 4 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Length of Code is not Proper... ');", True)
                        Me.txt_Sys_Code.Focus()
                        Exit Sub
                    End If
                    newCode = " " & newCode & " "
                    If InStr(1, newCode, " CREATE ", 1) > 0 Or InStr(1, newCode, " DELETE ", 1) > 0 Or InStr(1, newCode, " DROP ", 1) > 0 Or InStr(1, newCode, " INSERT ", 1) > 1 Or InStr(1, newCode, " TRACK ", 1) > 1 Or InStr(1, newCode, " TRACE ", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do not use reserve words... ');", True)
                        Me.txt_Sys_Code.Focus()
                        Exit Sub
                    End If
                    newCode = TrimX(newCode)
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Enter Distinct Code... ');", True)
                    Me.txt_Sys_Code.Focus()
                    Exit Sub
                End If

                'check unwanted characters
                c = 0
                counter1 = 0
                For iloop = 1 To Len(newCode)
                    strcurrentchar = Mid(newCode, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';~!@#$^&*-_=+|[]{}?,<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter1 = 1
                        End If
                    End If
                Next
                If counter1 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Un-wanted Characters... ');", True)
                    Me.txt_Sys_Code.Focus()
                    Exit Sub
                End If

                'Check Duplicate User Code
                Dim str As Object = Nothing
                Dim flag As Object = Nothing
                str = "SELECT " & ID & " FROM " & MYTABLE & " WHERE (" & CODE & " ='" & Trim(newCode) & "') "
                Dim cmd1 As New SqlCommand(str, SqlConn)
                SqlConn.Open()
                flag = cmd1.ExecuteScalar
                If flag <> Nothing Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Code Already Exists, Please try to enter other Distinct Code... ');", True)
                    Me.txt_Sys_Code.Focus()
                    Exit Sub
                End If
                SqlConn.Close()

                '********************************************************************************************************************

                'Server Validation for Full Name
                Dim Names As String = Nothing
                Names = TrimAll(txt_Sys_Name.Text)
                If String.IsNullOrEmpty(Names) Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Enter Name... ');", True)
                    Me.txt_Sys_Name.Focus()
                    Exit Sub
                End If
                Names = RemoveQuotes(Names)
                If Names.Length > 100 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Name must be of Proper Length.. ');", True)
                    txt_Sys_Name.Focus()
                    Exit Sub
                End If
                Names = " " & Names & " "
                If InStr(1, Names, "CREATE", 1) > 0 Or InStr(1, Names, "DELETE", 1) > 0 Or InStr(1, Names, "DROP", 1) > 0 Or InStr(1, Names, "INSERT", 1) > 1 Or InStr(1, Names, "TRACK", 1) > 1 Or InStr(1, Names, "TRACE", 1) > 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Reserve Word... ');", True)
                    txt_Sys_Name.Focus()
                    Exit Sub
                End If
                Names = TrimAll(Names)
                c = 0
                counter2 = 0
                For iloop = 1 To Len(Names)
                    strcurrentchar = Mid(Names, iloop, 1)
                    If c = 0 Then
                        If Not InStr("'~!@#$^&*=+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter2 = 1
                        End If
                    End If
                Next
                If counter2 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Do Not Use Un-Wated Characters... ');", True)
                    txt_Sys_Name.Focus()
                    Exit Sub
                End If

                ''****************************************************************************************
                ''Server Validation for description
                Dim Description As String = Nothing
                If txt_Sys_Desc.Text <> "" Then
                    Description = TrimAll(txt_Sys_Desc.Text)
                    Description = RemoveQuotes(Description)
                    If Description.Length > 250 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not of proper length... ');", True)
                        Me.txt_Sys_Desc.Focus()
                        Exit Sub
                    End If
                    Description = " " & Description & " "
                    If InStr(1, Description, "CREATE", 1) > 0 Or InStr(1, Description, "DELETE", 1) > 0 Or InStr(1, Description, "DROP", 1) > 0 Or InStr(1, Description, "INSERT", 1) > 1 Or InStr(1, Description, "TRACK", 1) > 1 Or InStr(1, Description, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Do Not use Reserve Words... ');", True)
                        Me.txt_Sys_Desc.Focus()
                        Exit Sub
                    End If
                    Description = TrimAll(Description)
                    'check unwanted characters
                    c = 0
                    counter3 = 0
                    For iloop = 1 To Len(Description)
                        strcurrentchar = Mid(Description, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*-_=+|[]{}?<>%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter3 = 1
                            End If
                        End If
                    Next
                    If counter3 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do not use un-wanted letters... ');", True)
                        Me.txt_Sys_Desc.Focus()
                        Exit Sub
                    End If
                Else
                    Description = ""
                End If

                'special fields
                Dim MARCode As String = Nothing
                If MYTABLE = "COUNTRIES" Or MYTABLE = "LANGUAGES" Then
                    If txt_Sys_Marc.Text <> "" Then
                        MARCode = TrimAll(txt_Sys_Marc.Text)
                        MARCode = RemoveQuotes(MARCode)
                        If MARCode.Length > 3 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not of proper length... ');", True)
                            Me.txt_Sys_Marc.Focus()
                            Exit Sub
                        End If
                        MARCode = " " & MARCode & " "
                        If InStr(1, MARCode, "CREATE", 1) > 0 Or InStr(1, MARCode, "DELETE", 1) > 0 Or InStr(1, MARCode, "DROP", 1) > 0 Or InStr(1, MARCode, "INSERT", 1) > 1 Or InStr(1, MARCode, "TRACK", 1) > 1 Or InStr(1, MARCode, "TRACE", 1) > 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Do Not use Reserve Words... ');", True)
                            Me.txt_Sys_Marc.Focus()
                            Exit Sub
                        End If
                        MARCode = TrimAll(MARCode)
                        'check unwanted characters
                        c = 0
                        counter4 = 0
                        For iloop = 1 To Len(MARCode)
                            strcurrentchar = Mid(MARCode, iloop, 1)
                            If c = 0 Then
                                If Not InStr("'~!@#$^&*-_=+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter4 = 1
                                End If
                            End If
                        Next
                        If counter4 = 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do not use un-wanted letters... ');", True)
                            Me.txt_Sys_Marc.Focus()
                            Exit Sub
                        End If
                    Else
                        MARCode = ""
                    End If
                End If

                'special fields BibCode
                Dim BibCodeNew As String = Nothing
                If MYTABLE = "MATERIALS" Or MYTABLE = "DOC_TYPES" Then
                    If DropDownList3.Text <> "" Then
                        BibCodeNew = Trim(DropDownList3.SelectedValue)
                        BibCodeNew = RemoveQuotes(BibCodeNew)
                        If BibCodeNew.Length > 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not of proper length... ');", True)
                            Me.DropDownList3.Focus()
                            Exit Sub
                        End If
                        BibCodeNew = " " & BibCodeNew & " "
                        If InStr(1, BibCodeNew, "CREATE", 1) > 0 Or InStr(1, BibCodeNew, "DELETE", 1) > 0 Or InStr(1, BibCodeNew, "DROP", 1) > 0 Or InStr(1, BibCodeNew, "INSERT", 1) > 1 Or InStr(1, BibCodeNew, "TRACK", 1) > 1 Or InStr(1, BibCodeNew, "TRACE", 1) > 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Do Not use Reserve Words... ');", True)
                            Me.DropDownList3.Focus()
                            Exit Sub
                        End If
                        BibCodeNew = TrimAll(BibCodeNew)
                        'check unwanted characters
                        c = 0
                        Counter5 = 0
                        For iloop = 1 To Len(BibCodeNew)
                            strcurrentchar = Mid(BibCodeNew, iloop, 1)
                            If c = 0 Then
                                If Not InStr("';~!@#$^&*-_=+|[]{}?,<>=()%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    Counter5 = 1
                                End If
                            End If
                        Next
                        If Counter5 = 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do not use un-wanted letters... ');", True)
                            Me.DropDownList3.Focus()
                            Exit Sub
                        End If
                    Else
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Plz Select Bibliographic Levels from Drop-DownList... ');", True)
                        Me.DropDownList3.Focus()
                        Exit Sub
                    End If
                End If

                'special fields mat code
                Dim MatCodeNew As String = Nothing
                If MYTABLE = "DOC_TYPES" Then
                    If DropDownList4.Text <> "" Then
                        MatCodeNew = Trim(DropDownList4.SelectedValue)
                        MatCodeNew = RemoveQuotes(MatCodeNew)
                        If MatCodeNew.Length > 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not of proper length... ');", True)
                            Me.DropDownList4.Focus()
                            Exit Sub
                        End If
                        MatCodeNew = " " & MatCodeNew & " "
                        If InStr(1, MatCodeNew, "CREATE", 1) > 0 Or InStr(1, MatCodeNew, "DELETE", 1) > 0 Or InStr(1, MatCodeNew, "DROP", 1) > 0 Or InStr(1, MatCodeNew, "INSERT", 1) > 1 Or InStr(1, MatCodeNew, "TRACK", 1) > 1 Or InStr(1, MatCodeNew, "TRACE", 1) > 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Do Not use Reserve Words... ');", True)
                            Me.DropDownList4.Focus()
                            Exit Sub
                        End If
                        MatCodeNew = TrimAll(MatCodeNew)
                        'check unwanted characters
                        c = 0
                        Counter6 = 0
                        For iloop = 1 To Len(MatCodeNew)
                            strcurrentchar = Mid(MatCodeNew, iloop, 1)
                            If c = 0 Then
                                If Not InStr("';~!@#$^&*-_=+|[]{}?,<>=()%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    Counter6 = 1
                                End If
                            End If
                        Next
                        If Counter6 = 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do not use un-wanted letters... ');", True)
                            Me.DropDownList4.Focus()
                            Exit Sub
                        End If
                    Else
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Plz Do not use un-wanted letters... .. ');", True)
                        Me.DropDownList4.Focus()
                        Exit Sub
                    End If
                End If
                '********************************************************************************************************
                'INSERT THE RECORD IN TO THE DATABASE
                Dim SQL As String
                Dim Cmd As SqlCommand
                Dim da As SqlDataAdapter
                Dim ds As New DataSet
                Dim CB As SqlCommandBuilder
                Dim dtrow As DataRow
                SQL = "SELECT * FROM " & MYTABLE & " WHERE (" & CODE & " = '00')"
                Cmd = New SqlCommand(SQL, SqlConn)
                da = New SqlDataAdapter(Cmd)
                CB = New SqlCommandBuilder(da)
                SqlConn.Open()
                da.Fill(ds, "STable")
                dtrow = ds.Tables("STable").NewRow

                If Not String.IsNullOrEmpty(newCode) Then
                    dtrow(CODE) = newCode.Trim
                End If
                If Not String.IsNullOrEmpty(Names) Then
                    dtrow(NAME) = Names.Trim
                End If
                If Not String.IsNullOrEmpty(Description) Then
                    dtrow(DESC) = Description.Trim
                Else
                    dtrow(DESC) = System.DBNull.Value
                End If
                If MYTABLE = "COUNTRIES" Or MYTABLE = "LANGUAGES" Then
                    If Not String.IsNullOrEmpty(MARCode) Then
                        dtrow("MARC_CODE") = MARCode.Trim
                    Else
                        dtrow("MARC_CODE") = System.DBNull.Value
                    End If
                End If
                If MYTABLE = "MATERIALS" Or MYTABLE = "DOC_TYPES" Then
                    If Not String.IsNullOrEmpty(BibCodeNew) Then
                        dtrow("BIB_CODE") = BibCodeNew.Trim
                    Else
                        dtrow("BIB_CODE") = System.DBNull.Value
                    End If
                End If
                If MYTABLE = "DOC_TYPES" Then
                    If Not String.IsNullOrEmpty(MatCodeNew) Then
                        dtrow("MAT_CODE") = MatCodeNew.Trim
                    Else
                        dtrow("MAT_CODE") = System.DBNull.Value
                    End If
                End If
                dtrow("DATE_ADDED") = Now.ToShortDateString

                ds.Tables("STable").Rows.Add(dtrow)
                thisTransaction = SqlConn.BeginTransaction()
                da.SelectCommand.Transaction = thisTransaction
                da.Update(ds, "STable")
                thisTransaction.Commit()
                ClearFields()
                ds.Dispose()
                Label6.Text = "Record Added Successfully!"
            Catch q As SqlException
                thisTransaction.Rollback()
            Catch ex As Exception
                Label6.Text = "Error: " & (ex.Message())
            Finally
                SqlConn.Close()
                SearchResults()
                txt_Sys_Code.Focus()
            End Try
        End If
    End Sub
    Public Sub ClearFields()
        txt_Sys_Code.Text = ""
        txt_Sys_Desc.Text = ""
        txt_Sys_Marc.Text = ""
        txt_Sys_Name.Text = ""
        DropDownList3.Text = ""
        DropDownList4.Text = ""
        bttn_Save.Visible = True
        bttn_Update.Visible = False
        Me.bttn_Delete.Visible = False
    End Sub
    Public Sub Cancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Cancel.Click
        Label6.Text = ""
        txt_Sys_Code.Enabled = True
        ClearFields()
    End Sub
    'change grid on selection
    Private Sub DropDownList1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownList1.TextChanged
        GetmyTable()
        If MYTABLE = "COUNTRIES" Or MYTABLE = "LANGUAGES" Then
            MarcTag.Visible = True
        Else
            MarcTag.Visible = False
        End If
        If MYTABLE = "MATERIALS" Then
            BibLvlTr.Visible = True
            PopulateBibLevels()
        Else
            BibLvlTr.Visible = False
        End If
        If MYTABLE = "DOC_TYPES" Then
            BibLvlTr.Visible = True
            MatCodeTr.Visible = True
            PopulateBibLevels()
        Else
            MatCodeTr.Visible = False
        End If

        SearchResults()
    End Sub
    'Populate Grid
    Public Sub SearchResults()
        Dim dtRecords As DataTable = Nothing
        Try
            Dim c, counter1, counter2, counter3, counter4 As Integer
            Dim iloop As Integer
            Dim strcurrentchar As Object

            'search string validation
            Dim mySearchTable As Object = Nothing
            If DropDownList1.Text <> "" Then
                mySearchTable = TrimAll(DropDownList1.SelectedValue)
                mySearchTable = RemoveQuotes(mySearchTable)
                If mySearchTable.Length > 20 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input Length is not Valid... ');", True)
                    Me.DropDownList1.Focus()
                    Exit Sub
                End If
                mySearchTable = " " & mySearchTable & " "
                If InStr(1, mySearchTable, "CREATE", 1) > 0 Or InStr(1, mySearchTable, "DELETE", 1) > 0 Or InStr(1, mySearchTable, "DROP", 1) > 0 Or InStr(1, mySearchTable, "INSERT", 1) > 1 Or InStr(1, mySearchTable, "TRACK", 1) > 1 Or InStr(1, mySearchTable, "TRACE", 1) > 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Do no use Reserve Words... ');", True)
                    Me.DropDownList1.Focus()
                    Exit Sub
                End If
                mySearchTable = TrimAll(mySearchTable)
                'check unwanted characters
                c = 0
                counter1 = 0
                For iloop = 1 To Len(mySearchTable)
                    strcurrentchar = Mid(mySearchTable, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter1 = 1
                        End If
                    End If
                Next
                If counter1 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' do not use un-wanted characters... ');", True)
                    Me.DropDownList1.Focus()
                    Exit Sub
                End If
            Else
                ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Plz Select the Value from Drop-down... ');", True)
                Me.DropDownList1.Focus()
                Exit Sub
            End If

            Dim SQL As String = Nothing
            SQL = "SELECT * FROM " & mySearchTable & " "
            SQL = SQL & " ORDER BY " & NAME & " ASC "

            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            da.Fill(ds)

            dtRecords = ds.Tables(0).Copy
            Dim RecordCount As Long = 0
            If dtRecords.Rows.Count = 0 Then
                Grid1_SysData.AutoGenerateColumns = True
                Me.Grid1_SysData.DataSource = Nothing
                Grid1_SysData.DataBind()
                Grid1_SysData.Columns.Clear()
                Label2.Text = "Total Record(s): 0 "
                Print_Summary_Bttn.Visible = False
            Else
                RecordCount = dtRecords.Rows.Count
                Grid1_SysData.Columns.Clear()
                Grid1_SysData.AutoGenerateColumns = False
                Grid1_SysData.AllowSorting = True
                Grid1_SysData.Font.Name = "Tahoma"
                Grid1_SysData.HorizontalAlign = HorizontalAlign.Left
                Grid1_SysData.DataSource = dtRecords
                Print_Summary_Bttn.Visible = True

                Dim EditBttn As New ButtonField
                EditBttn.HeaderText = "Edit"
                EditBttn.ItemStyle.Width = 80
                EditBttn.ButtonType = ButtonType.Link
                EditBttn.ItemStyle.HorizontalAlign = HorizontalAlign.Center
                EditBttn.ItemStyle.ForeColor = Drawing.Color.Red
                EditBttn.Text = "Edit"
                EditBttn.CommandName = "Select" '= "Edit"
                EditBttn.ShowHeader = True
                Grid1_SysData.Columns.Add(EditBttn)


                Dim CodeField As New BoundField
                CodeField.HeaderText = "Code"
                CodeField.ItemStyle.Width = 80
                CodeField.SortExpression = CODE
                CodeField.ItemStyle.HorizontalAlign = HorizontalAlign.Center
                CodeField.HeaderStyle.Font.Size = FontSize.XXLarge
                CodeField.DataField = dtRecords.Columns(CODE).ToString()
                'CodeField.DataField = dtRecords.Columns(1).ToString()
                CodeField.ShowHeader = True
                Grid1_SysData.Columns.Add(CodeField)


                Dim NameField As New BoundField
                NameField.HeaderText = "Name"
                NameField.ItemStyle.Width = 300
                NameField.SortExpression = NAME
                NameField.HeaderStyle.Font.Size = FontSize.XXLarge
                NameField.DataField = dtRecords.Columns(NAME).ToString()
                NameField.ShowHeader = True
                Grid1_SysData.Columns.Add(NameField)

                Dim DescField As New BoundField
                DescField.HeaderText = "Remarks"
                DescField.NullDisplayText = " "
                DescField.HeaderStyle.Font.Size = FontSize.XXLarge
                DescField.DataField = dtRecords.Columns(DESC).ToString()
                DescField.ShowHeader = True
                Grid1_SysData.Columns.Add(DescField)

                If MYTABLE = "COUNTRIES" Or MYTABLE = "LANGUAGES" Then
                    Dim MarcCode As New BoundField
                    MarcCode.HeaderText = "MARC Code"
                    MarcCode.SortExpression = "MARC_CODE"
                    MarcCode.ItemStyle.HorizontalAlign = HorizontalAlign.Center
                    MarcCode.HeaderStyle.Font.Size = FontSize.XXLarge
                    MarcCode.DataField = dtRecords.Columns("MARC_CODE").ToString()
                    MarcCode.ShowHeader = True
                    Grid1_SysData.Columns.Add(MarcCode)
                End If
                If MYTABLE = "MATERIALS" Or MYTABLE = "DOC_TYPES" Then
                    Dim BibCode As New BoundField
                    BibCode.HeaderText = "Bibliographic Level"
                    BibCode.SortExpression = "BIB_CODE"
                    BibCode.ItemStyle.HorizontalAlign = HorizontalAlign.Center
                    BibCode.HeaderStyle.Font.Size = FontSize.XXLarge
                    BibCode.DataField = dtRecords.Columns("BIB_CODE").ToString()
                    BibCode.ShowHeader = True
                    Grid1_SysData.Columns.Add(BibCode)
                End If
                If MYTABLE = "DOC_TYPES" Then
                    Dim MatCode As New BoundField
                    MatCode.HeaderText = "Material Type"
                    MatCode.SortExpression = "MAT_CODE"
                    MatCode.ItemStyle.HorizontalAlign = HorizontalAlign.Center
                    MatCode.HeaderStyle.Font.Size = FontSize.XXLarge
                    MatCode.DataField = dtRecords.Columns("MAT_CODE").ToString()
                    MatCode.ShowHeader = True
                    Grid1_SysData.Columns.Add(MatCode)
                End If
                Grid1_SysData.HeaderStyle.Font.Size = FontSize.XXLarge
                Grid1_SysData.DataBind()
                Label2.Text = "Total Record(s): " & RecordCount
                Grid1_SysData.ToolTip = "Total Record(s): " & RecordCount
            End If
            ViewState("dt") = dtRecords
            DropDownList1.Focus()

        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
            ClearFields()
            Dim sender As Object
            Dim e As EventArgs
            bttn_No_Click(sender, e)
            DropDownList1.Focus()
        End Try
    End Sub
    Private Function ColumnEqual(ByVal A As Object, ByVal B As Object) As Boolean
        If A Is DBNull.Value And B Is DBNull.Value Then Return True ' Both are DBNull.Value.
        If A Is DBNull.Value Or B Is DBNull.Value Then Return False ' Only one is DBNull.Value.
        Return A = B                                                ' Value type standard comparison
    End Function
    'grid view page index changing event
    Protected Sub Grid1_SysData_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles Grid1_SysData.PageIndexChanging
        Try
            'rebind datagrid
            Grid1_SysData.DataSource = ViewState("dt") 'temp
            Grid1_SysData.PageIndex = e.NewPageIndex
            index = e.NewPageIndex * Grid1_SysData.PageSize
            Grid1_SysData.DataBind()
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
            Return dataView
        Else
            Return New DataView()
        End If
    End Function
    'gridview sorting event
    Protected Sub Grid1_SysData_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles Grid1_SysData.Sorting
        Dim temp As DataTable = CType(ViewState("dt"), DataTable)
        GridViewSortExpression = e.SortExpression
        Grid1_SysData.DataSource = temp
        Dim pageIndex As Integer = Grid1_SysData.PageIndex
        Grid1_SysData.DataSource = SortDataTable(Grid1_SysData.DataSource, False)
        Grid1_SysData.DataBind()
        Grid1_SysData.PageIndex = pageIndex
    End Sub
    Protected Sub Grid1_SysData_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Grid1_SysData.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes("onmouseover") = "this.style.cursor='hand';this.style.textDecoration='bold';this.style.background='#FFDFDF'"
            e.Row.Attributes("onmouseout") = "this.style.textDecoration='none';this.style.background='none'"
            'e.Row.Attributes("onclick") = ClientScript.GetPostBackClientHyperlink(Me, "Select$" & Convert.ToString(e.Row.RowIndex))
        End If

    End Sub
    Private Shared Function t() As Object
        Throw New NotImplementedException
    End Function
    'get value of row from grid
    Private Sub Grid1_SysData_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles Grid1_SysData.RowCommand
        Try
            If e.CommandName = "Select" Then

                Dim myRowID As Integer
                myRowID = e.CommandArgument.ToString()

                Dim myEdCode As String = Nothing
                myEdCode = Grid1_SysData.Rows(myRowID).Cells(1).Text
                Label7.Text = Grid1_SysData.Rows(myRowID).Cells(1).Text
                txt_Sys_Code.Text = myEdCode
                If Grid1_SysData.Rows(myRowID).Cells(2).Text <> "" Then
                    txt_Sys_Name.Text = Grid1_SysData.Rows(myRowID).Cells(2).Text
                    Label8.Text = Grid1_SysData.Rows(myRowID).Cells(2).Text
                Else
                    txt_Sys_Name.Text = ""
                    Label8.Text = ""
                End If
                If Grid1_SysData.Rows(myRowID).Cells(3).Text <> "" Then
                    txt_Sys_Desc.Text = Grid1_SysData.Rows(myRowID).Cells(3).Text
                    Label9.Text = Grid1_SysData.Rows(myRowID).Cells(3).Text
                Else
                    txt_Sys_Desc.Text = ""
                    Label9.Text = ""
                End If
                If MYTABLE = "COUNTRIES" Or MYTABLE = "LANGUAGES" Then
                    If Grid1_SysData.Rows(myRowID).Cells(4).Text <> "" Then
                        If Grid1_SysData.Rows(myRowID).Cells(4).Text = "&nbsp;" Then
                            txt_Sys_Marc.Text = ""
                        Else
                            txt_Sys_Marc.Text = Grid1_SysData.Rows(myRowID).Cells(4).Text
                        End If
                    Else
                        txt_Sys_Marc.Text = ""
                    End If
                End If
                If MYTABLE = "MATERIALS" Or MYTABLE = "DOC_TYPES" Then
                    If Grid1_SysData.Rows(myRowID).Cells(4).Text <> "" Then
                        DropDownList3.SelectedValue = Grid1_SysData.Rows(myRowID).Cells(4).Text
                        DropDownList3_SelectedIndexChanged(sender, e)
                        UpdatePanel2.Update()
                    Else
                        DropDownList3.Text = ""
                    End If
                End If
                If MYTABLE = "DOC_TYPES" Then
                    If Grid1_SysData.Rows(myRowID).Cells(5).Text <> "" Then
                        DropDownList4.SelectedValue = Grid1_SysData.Rows(myRowID).Cells(5).Text
                    Else
                        DropDownList4.Text = ""
                    End If
                End If

                Me.bttn_Save.Visible = False
                Me.bttn_Update.Visible = True
                Me.bttn_Delete.Visible = True
                Me.txt_Sys_Code.Enabled = False

            End If
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
        Finally
            Label6.Text = "Press UPDATE button to Update the Record"
        End Try
    End Sub 'Grid1_ItemCommand
    'update the record
    Protected Sub bttn_Update_Click(ByVal sender As Object, ByVal e As EventArgs) Handles bttn_Update.Click
        Dim SQL As String = Nothing
        Dim da As SqlDataAdapter
        Dim cb As SqlCommandBuilder
        Dim ds As New DataSet
        Dim dt As New DataTable
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing
        Try
            If IsPostBack = True Then
                'check unwanted characters
                Dim iloop As Integer
                Dim strcurrentchar As Object
                Dim c As Integer
                Dim counter1, counter2, counter3, counter4, Counter5, Counter6 As Integer

                GetmyTable() ' get table structure
                'Server Validation for User Code
                Dim newCode As String = Nothing
                newCode = TrimX(txt_Sys_Code.Text)
                newCode = UCase(newCode)
                If Not String.IsNullOrEmpty(newCode) Then
                    newCode = RemoveQuotes(newCode)
                    If newCode.Length > 4 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Length of Code is not Proper... ');", True)
                        Me.txt_Sys_Code.Focus()
                        Exit Sub
                    End If
                    newCode = " " & newCode & " "
                    If InStr(1, newCode, " CREATE ", 1) > 0 Or InStr(1, newCode, " DELETE ", 1) > 0 Or InStr(1, newCode, " DROP ", 1) > 0 Or InStr(1, newCode, " INSERT ", 1) > 1 Or InStr(1, newCode, " TRACK ", 1) > 1 Or InStr(1, newCode, " TRACE ", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do not use reserve words... ');", True)
                        Me.txt_Sys_Code.Focus()
                        Exit Sub
                    End If
                    newCode = TrimX(newCode)
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Enter Distinct Code... ');", True)
                    Me.txt_Sys_Code.Focus()
                    Exit Sub
                End If

                'check unwanted characters
                c = 0
                counter1 = 0
                For iloop = 1 To Len(newCode)
                    strcurrentchar = Mid(newCode, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';~!@#$^&*-_=+|[]{}?,<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter1 = 1
                        End If
                    End If
                Next
                If counter1 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Un-wanted Characters... ');", True)
                    Me.txt_Sys_Code.Focus()
                    Exit Sub
                End If

                '********************************************************************************************************************
                'Server Validation for Full Name
                Dim Names As String = Nothing
                Names = TrimAll(txt_Sys_Name.Text)
                If String.IsNullOrEmpty(Names) Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Enter Name... ');", True)
                    Me.txt_Sys_Name.Focus()
                    Exit Sub
                End If
                Names = RemoveQuotes(Names)
                If Names.Length > 100 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Name must be of Proper Length.. ');", True)
                    txt_Sys_Name.Focus()
                    Exit Sub
                End If
                Names = " " & Names & " "
                If InStr(1, Names, "CREATE", 1) > 0 Or InStr(1, Names, "DELETE", 1) > 0 Or InStr(1, Names, "DROP", 1) > 0 Or InStr(1, Names, "INSERT", 1) > 1 Or InStr(1, Names, "TRACK", 1) > 1 Or InStr(1, Names, "TRACE", 1) > 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Reserve Word... ');", True)
                    txt_Sys_Name.Focus()
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
                    txt_Sys_Name.Focus()
                    Exit Sub
                End If

                ''****************************************************************************************
                ''Server Validation for description
                Dim Description As String = Nothing
                If txt_Sys_Desc.Text <> "" Then
                    Description = TrimAll(txt_Sys_Desc.Text)
                    Description = RemoveQuotes(Description)
                    If Description.Length > 250 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not of proper length... ');", True)
                        Me.txt_Sys_Desc.Focus()
                        Exit Sub
                    End If
                    Description = " " & Description & " "
                    If InStr(1, Description, "CREATE", 1) > 0 Or InStr(1, Description, "DELETE", 1) > 0 Or InStr(1, Description, "DROP", 1) > 0 Or InStr(1, Description, "INSERT", 1) > 1 Or InStr(1, Description, "TRACK", 1) > 1 Or InStr(1, Description, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Do Not use Reserve Words... ');", True)
                        Me.txt_Sys_Desc.Focus()
                        Exit Sub
                    End If
                    Description = TrimAll(Description)
                    'check unwanted characters
                    c = 0
                    counter3 = 0
                    For iloop = 1 To Len(Description)
                        strcurrentchar = Mid(Description, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';~!@#$^&*-_=+|[]{}?,<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter3 = 1
                            End If
                        End If
                    Next
                    If counter3 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do not use un-wanted letters... ');", True)
                        Me.txt_Sys_Desc.Focus()
                        Exit Sub
                    End If
                Else
                    Description = ""
                End If

                'special fields
                Dim MARCode As String = Nothing
                If MYTABLE = "COUNTRIES" Or MYTABLE = "LANGUAGES" Then
                    If txt_Sys_Marc.Text <> "" Then
                        MARCode = TrimAll(txt_Sys_Marc.Text)
                        MARCode = RemoveQuotes(MARCode)
                        If MARCode.Length > 3 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not of proper length... ');", True)
                            Me.txt_Sys_Marc.Focus()
                            Exit Sub
                        End If
                        MARCode = " " & MARCode & " "
                        If InStr(1, MARCode, "CREATE", 1) > 0 Or InStr(1, MARCode, "DELETE", 1) > 0 Or InStr(1, MARCode, "DROP", 1) > 0 Or InStr(1, MARCode, "INSERT", 1) > 1 Or InStr(1, MARCode, "TRACK", 1) > 1 Or InStr(1, MARCode, "TRACE", 1) > 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Do Not use Reserve Words... ');", True)
                            Me.txt_Sys_Marc.Focus()
                            Exit Sub
                        End If
                        MARCode = TrimAll(MARCode)
                        'check unwanted characters
                        c = 0
                        counter4 = 0
                        For iloop = 1 To Len(MARCode)
                            strcurrentchar = Mid(MARCode, iloop, 1)
                            If c = 0 Then
                                If Not InStr("';~!@#$^&*-_=+|[]{}?,<>=()%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter4 = 1
                                End If
                            End If
                        Next
                        If counter4 = 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do not use un-wanted letters... ');", True)
                            Me.txt_Sys_Marc.Focus()
                            Exit Sub
                        End If
                    Else
                        MARCode = ""
                    End If
                End If

                'special fields BibCode
                Dim BibCodeNew As String = Nothing
                If MYTABLE = "MATERIALS" Or MYTABLE = "DOC_TYPES" Then
                    If DropDownList3.Text <> "" Then
                        BibCodeNew = Trim(DropDownList3.SelectedValue)
                        BibCodeNew = RemoveQuotes(BibCodeNew)
                        If BibCodeNew.Length > 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not of proper length... ');", True)
                            Me.DropDownList3.Focus()
                            Exit Sub
                        End If
                        BibCodeNew = " " & BibCodeNew & " "
                        If InStr(1, BibCodeNew, "CREATE", 1) > 0 Or InStr(1, BibCodeNew, "DELETE", 1) > 0 Or InStr(1, BibCodeNew, "DROP", 1) > 0 Or InStr(1, BibCodeNew, "INSERT", 1) > 1 Or InStr(1, BibCodeNew, "TRACK", 1) > 1 Or InStr(1, BibCodeNew, "TRACE", 1) > 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Do Not use Reserve Words... ');", True)
                            Me.DropDownList3.Focus()
                            Exit Sub
                        End If
                        BibCodeNew = TrimAll(BibCodeNew)
                        'check unwanted characters
                        c = 0
                        Counter5 = 0
                        For iloop = 1 To Len(BibCodeNew)
                            strcurrentchar = Mid(BibCodeNew, iloop, 1)
                            If c = 0 Then
                                If Not InStr("';~!@#$^&*-_=+|[]{}?,<>=()%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    Counter5 = 1
                                End If
                            End If
                        Next
                        If Counter5 = 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do not use un-wanted letters... ');", True)
                            Me.DropDownList3.Focus()
                            Exit Sub
                        End If
                    Else
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Plz Select Bibliographic Levels from Drop-DownList... ');", True)
                        Me.DropDownList3.Focus()
                        Exit Sub
                    End If
                End If

                'special fields mat code
                Dim MatCodeNew As String = Nothing
                If MYTABLE = "DOC_TYPES" Then
                    If DropDownList4.Text <> "" Then
                        MatCodeNew = Trim(DropDownList4.SelectedValue)
                        MatCodeNew = RemoveQuotes(MatCodeNew)
                        If MatCodeNew.Length > 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not of proper length... ');", True)
                            Me.DropDownList4.Focus()
                            Exit Sub
                        End If
                        MatCodeNew = " " & MatCodeNew & " "
                        If InStr(1, MatCodeNew, "CREATE", 1) > 0 Or InStr(1, MatCodeNew, "DELETE", 1) > 0 Or InStr(1, MatCodeNew, "DROP", 1) > 0 Or InStr(1, MatCodeNew, "INSERT", 1) > 1 Or InStr(1, MatCodeNew, "TRACK", 1) > 1 Or InStr(1, MatCodeNew, "TRACE", 1) > 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Do Not use Reserve Words... ');", True)
                            Me.DropDownList4.Focus()
                            Exit Sub
                        End If
                        MatCodeNew = TrimAll(MatCodeNew)
                        'check unwanted characters
                        c = 0
                        Counter6 = 0
                        For iloop = 1 To Len(MatCodeNew)
                            strcurrentchar = Mid(MatCodeNew, iloop, 1)
                            If c = 0 Then
                                If Not InStr("';~!@#$^&*-_=+|[]{}?,<>=()%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    Counter6 = 1
                                End If
                            End If
                        Next
                        If Counter6 = 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do not use un-wanted letters... ');", True)
                            Me.DropDownList4.Focus()
                            Exit Sub
                        End If
                    Else
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Plz Do not use un-wanted letters... .. ');", True)
                        Me.DropDownList4.Focus()
                        Exit Sub
                    End If
                End If


                '****************************************************************************************************
                'UPDATE THE LIBRARY PROFILE   
                SQL = "SELECT * FROM " & MYTABLE & " WHERE (" & CODE & " = '" & Trim(newCode) & "')"
                SqlConn.Open()
                da = New SqlDataAdapter(SQL, SqlConn)
                cb = New SqlCommandBuilder(da)
                da.Fill(ds, "USERS")
                If ds.Tables("USERS").Rows.Count <> 0 Then
                    If Not String.IsNullOrEmpty(Names) Then
                        ds.Tables("USERS").Rows(0)(NAME) = Names.Trim
                    Else
                        ds.Tables("USERS").Rows(0)(NAME) = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(Description) Then
                        ds.Tables("USERS").Rows(0)(DESC) = Description.Trim
                    Else
                        ds.Tables("USERS").Rows(0)(DESC) = System.DBNull.Value
                    End If
                    If MYTABLE = "COUNTRIES" Or MYTABLE = "LANGUAGES" Then
                        If Not String.IsNullOrEmpty(MARCode) Then
                            ds.Tables("USERS").Rows(0)("MARC_CODE") = MARCode.Trim
                        Else
                            ds.Tables("USERS").Rows(0)("MARC_CODE") = System.DBNull.Value
                        End If
                    End If
                    If MYTABLE = "MATERIALS" Or MYTABLE = "DOC_TYPES" Then
                        If Not String.IsNullOrEmpty(BibCodeNew) Then
                            ds.Tables("USERS").Rows(0)("BIB_CODE") = BibCodeNew.Trim
                        Else
                            ds.Tables("USERS").Rows(0)("BIB_CODE") = System.DBNull.Value
                        End If
                    End If
                    If MYTABLE = "DOC_TYPES" Then
                        If Not String.IsNullOrEmpty(MatCodeNew) Then
                            ds.Tables("USERS").Rows(0)("MAT_CODE") = MatCodeNew.Trim
                        Else
                            ds.Tables("USERS").Rows(0)("MAT_CODE") = System.DBNull.Value
                        End If
                    End If
                    ds.Tables("USERS").Rows(0)("DATE_MODIFIED") = Now.Date

                    thisTransaction = SqlConn.BeginTransaction()
                    da.SelectCommand.Transaction = thisTransaction
                    da.Update(ds, "USERS")
                    thisTransaction.Commit()
                    Names = Nothing
                    Description = Nothing
                    MARCode = Nothing
                    BibCodeNew = Nothing
                    MatCodeNew = Nothing
                    Label6.Visible = True
                    Label6.Text = "Record Updated Successfully"
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' System Record Updated Successfully... ');", True)
                    ClearFields()
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('System Record Update  - Please Contact System Administrator... ');", True)
                    Exit Sub
                End If
            End If
            SqlConn.Close()
        Catch q As SqlException
            thisTransaction.Rollback()
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
        Finally
            txt_Sys_Code.Enabled = True
            SqlConn.Close()
            SearchResults()
        End Try
    End Sub
    Public Sub bttn_No_Click(ByVal sender As Object, ByVal e As EventArgs) Handles bttn_No.Click
        txt_Sys_Code.Enabled = True
        ClearFields()
    End Sub
    'delete the record
    Protected Sub bttn_Yes_Click(ByVal sender As Object, ByVal e As EventArgs) Handles bttn_Yes.Click
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing
        Try

            If IsPostBack = True Then
                'check unwanted characters
                Dim iloop As Integer
                Dim strcurrentchar As Object
                Dim c As Integer
                Dim counter1 As Integer

                GetmyTable() ' get table structure
                'Server Validation for User Code
                Dim newCode As String = Nothing
                newCode = TrimX(txt_Sys_Code.Text)
                newCode = UCase(newCode)
                If Not String.IsNullOrEmpty(newCode) Then
                    newCode = RemoveQuotes(newCode)
                    If newCode.Length > 4 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Length of Code is not Proper... ');", True)
                        Me.txt_Sys_Code.Focus()
                        Exit Sub
                    End If
                    newCode = " " & newCode & " "
                    If InStr(1, newCode, " CREATE ", 1) > 0 Or InStr(1, newCode, " DELETE ", 1) > 0 Or InStr(1, newCode, " DROP ", 1) > 0 Or InStr(1, newCode, " INSERT ", 1) > 1 Or InStr(1, newCode, " TRACK ", 1) > 1 Or InStr(1, newCode, " TRACE ", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do not use reserve words... ');", True)
                        Me.txt_Sys_Code.Focus()
                        Exit Sub
                    End If
                    newCode = TrimX(newCode)

                    'check unwanted characters
                    c = 0
                    counter1 = 0
                    For iloop = 1 To Len(newCode)
                        strcurrentchar = Mid(newCode, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';~!@#$^&*-_=+|[]{}?,<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter1 = 1
                            End If
                        End If
                    Next
                    If counter1 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Un-wanted Characters... ');", True)
                        Me.txt_Sys_Code.Focus()
                        Exit Sub
                    End If
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Plz Select the Record to Delete... ');", True)
                    Me.txt_Sys_Code.Focus()
                    Exit Sub
                End If

                'Check Reference in Related Tables
                Dim SQL As Object = Nothing
                Dim flag As Object = Nothing

                If MYTABLE = "ACC_MATERIALS" Or MYTABLE = "BINDINGS" Or MYTABLE = "BOOKSTATUS" Or MYTABLE = "PHYSICAL_FORMATS" Then
                    SQL = "SELECT HOLD_ID FROM HOLDINGS  WHERE (" & CODE & " ='" & Trim(newCode) & "') "
                End If
                If MYTABLE = "ACQMODES" Or MYTABLE = "CURRENCIES" Then
                    SQL = "SELECT ACQ_ID FROM ACQUISITIONS  WHERE (" & CODE & " ='" & Trim(newCode) & "') "
                End If
                If MYTABLE = "FREQUENCIES" Then
                    SQL = "SELECT SUBS_ID FROM SUBSCRIPTIONS  WHERE (" & CODE & " ='" & Trim(newCode) & "') "
                End If
                If MYTABLE = "BIB_LEVELS" Or MYTABLE = "MATERIALS" Or MYTABLE = "DOC_TYPES" Or MYTABLE = "COUNTRIES" Or MYTABLE = "LANGUAGES" Then
                    SQL = "SELECT CAT_NO FROM CATS  WHERE (" & CODE & " ='" & Trim(newCode) & "') "
                End If

                If SQL = "" Then
                    Exit Sub
                End If

                SqlConn.Open()
                Dim cmd1 As New SqlCommand(SQL, SqlConn)
                flag = cmd1.ExecuteScalar
                If flag <> Nothing Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' You can not delete this record as reference exists for this in the ... " & MYTABLE & "');", True)
                    Me.txt_Sys_Code.Focus()
                    Exit Sub
                End If
                SqlConn.Close()

                Dim SQLDelete As String = Nothing
                SQLDelete = "DELETE FROM " & MYTABLE & " WHERE (" & CODE & " ='" & Trim(newCode) & "') "

                SqlConn.Open()
                Dim cmd2 As New SqlCommand(SQLDelete, SqlConn)
                cmd2.ExecuteNonQuery()
                SqlConn.Close()


            End If
            SqlConn.Close()
        Catch q As SqlException
            thisTransaction.Rollback()
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
            SearchResults()
        End Try
    End Sub
    'load report
    Protected Sub Print_Summary_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Print_Summary_Bttn.Click
        Try
            Dim Reportdoc As New ReportDocument
            Reportdoc = Report_Load_Summary()

            If DDL_PrintFormats.Text <> "" Then
                If DDL_PrintFormats.SelectedValue = "PDF" Then
                    Reportdoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "Report_SystemData_" + Format(DateTime.Now, "ddMMyyyy"))
                End If
                If DDL_PrintFormats.SelectedValue = "DOC" Then
                    Reportdoc.ExportToHttpResponse(ExportFormatType.WordForWindows, Response, True, "Report_SystemData_" + Format(DateTime.Now, "ddMMyyyy"))
                End If
            Else
                Reportdoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "Report_SystemData_" + Format(DateTime.Now, "ddMMyyyy"))
            End If
        Catch ex As Exception
            Label6.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    Public Function Report_Load_Summary() As ReportDocument
        Dim Rpt As New ReportDocument
        Dim dtLibrary As New DataTable
        dtLibrary = (ViewState("dt"))

        If DropDownList1.Text <> "" Then
            If DropDownList1.SelectedValue = "BIB_LEVELS" Then
                Rpt.Load(Server.MapPath("~/Reports/BibLevels_Summary_Report.rpt"))
            End If

            If DropDownList1.SelectedValue = "MATERIALS" Then
                Rpt.Load(Server.MapPath("~/Reports/Materials_Summary_Report.rpt"))
            End If

            If DropDownList1.SelectedValue = "DOC_TYPES" Then
                Rpt.Load(Server.MapPath("~/Reports/Documents_Summary_Report.rpt"))
            End If

            If DropDownList1.SelectedValue = "ACC_MATERIALS" Then
                Rpt.Load(Server.MapPath("~/Reports/AccompanyingMaterials_Summary_Report.rpt"))
            End If

            If DropDownList1.SelectedValue = "ACQMODES" Then
                Rpt.Load(Server.MapPath("~/Reports/AcquisitionModes_Summary_Report.rpt"))
            End If

            If DropDownList1.SelectedValue = "BINDINGS" Then
                Rpt.Load(Server.MapPath("~/Reports/Bindings_Summary_Report.rpt"))
            End If
            If DropDownList1.SelectedValue = "BOOKSTATUS" Then
                Rpt.Load(Server.MapPath("~/Reports/CopyStatus_Summary_Report.rpt"))
            End If
            If DropDownList1.SelectedValue = "COUNTRIES" Then
                Rpt.Load(Server.MapPath("~/Reports/Countries_Summary_Report.rpt"))
            End If
            If DropDownList1.SelectedValue = "CURRENCIES" Then
                Rpt.Load(Server.MapPath("~/Reports/Currencies_Summary_Report.rpt"))
            End If
            If DropDownList1.SelectedValue = "PHYSICAL_FORMATS" Then
                Rpt.Load(Server.MapPath("~/Reports/Formats_Summary_Report.rpt"))
            End If
            If DropDownList1.SelectedValue = "LANGUAGES" Then
                Rpt.Load(Server.MapPath("~/Reports/Languages_Summary_Report.rpt"))
            End If
            If DropDownList1.SelectedValue = "FREQUENCIES" Then
                Rpt.Load(Server.MapPath("~/Reports/Frequencies_Summary_Report.rpt"))
            End If
        Else
            Exit Function
        End If

        Rpt.SetDataSource(dtLibrary)
        Rpt.Refresh()

        'Dim myGrpName As Object = Nothing
        'If DDL_GroupBy.Text <> "" Then
        '    myGrpName = Trim(DDL_GroupBy.SelectedValue)
        'Else
        '    myGrpName = Nothing
        'End If

        'Dim myGroupName As CrystalReports.Engine.TextObject
        'If myGrpName <> "" Then
        '    myGroupName = Rpt.ReportDefinition.Sections("ReportHeaderSection1").ReportObjects.Item("Text2")
        '    myGroupName.Text = "Group By: " & DDL_GroupBy.SelectedItem().Text
        'Else
        '    Rpt.ReportDefinition.Sections("ReportHeaderSection1").SectionFormat.EnableSuppress = True
        'End If

        ''Group By the Report
        'Dim grpline As FieldDefinition
        'Dim FieldDef As FieldDefinition
        'If myGrpName <> "" Then
        '    grpline = Rpt.Database.Tables(0).Fields.Item(myGrpName)
        '    Rpt.DataDefinition.Groups.Item(0).ConditionField = grpline
        '    FieldDef = Rpt.Database.Tables.Item(0).Fields.Item(myGrpName) '("CLASS_NO")
        '    Rpt.DataDefinition.SortFields.Item(0).Field = FieldDef
        'Else
        '    Rpt.ReportDefinition.Sections("GroupHeaderSection1").SectionFormat.EnableSuppress = True
        'End If

        Rpt.SummaryInfo.ReportAuthor = RKLibraryParent
        Rpt.SummaryInfo.ReportComments = RKLibraryAddress
        Rpt.SummaryInfo.ReportTitle = RKLibraryName

        Response.Buffer = False
        Response.ClearContent()
        Response.ClearHeaders()
        dtLibrary.Dispose()
        Return Rpt
    End Function
End Class