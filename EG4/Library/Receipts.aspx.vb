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
Imports System.Net
Imports System.Drawing
Imports System.Drawing.Bitmap
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports.Engine.TextObject
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Shared
Imports System.Drawing.Printing
Imports CrystalDecisions
Imports EG4.PopulateCombo
Public Class Receipts
    Inherits System.Web.UI.Page
    Dim index As Integer = 0
    Public Srno As Integer = 0
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session.Item("IsThisU") <> Nothing Then
                If Not SConnection() = True Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Database Connection is Lost!');", True)
                Else
                    If Page.IsPostBack = False Then
                        Me.Rect_Save_Bttn.Visible = True
                        Me.Rect_Update_Bttn.Visible = False
                        Rect_Delete_Bttn.Visible = False
                        PopulateMembers()
                        PopulateAccNo()
                        PopulateCopyId()
                        PopulateReceiptId()
                        txt_Rect_Date.Text = Format(Now.Date, "dd/MM/yyyy")
                    End If
                End If
                Dim CreateSUserButton As Object 'System.Web.UI.WebControls.Button
                CreateSUserButton = Master.FindControl("Accordion1").FindControl("LibAdminPane").FindControl("Lib_Rect_Bttn")
                CreateSUserButton.ForeColor = Drawing.Color.Red
                'paneSelectedIndex = 1
                myPaneName = "LibAdminPane"

            Else
                Response.Redirect("~/Default.aspx", False)
            End If
            LibCode = Session.Item("LoggedLibcode")
        Catch ex As Exception
            Lbl_Error.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    Public Sub PopulateMembers()
        Dim dtSearch As DataTable = Nothing
        Try
            Dim SQL As String = Nothing
            SQL = "SELECT MEM_ID, MEM_NAME FROM MEMBERSHIPS WHERE (LIB_CODE = '" & Trim(LibCode) & "') ORDER BY MEM_NAME ;"

            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            da.Fill(ds)

            dtSearch = ds.Tables(0).Copy
            Dim RecordCount As Long = 0
            If dtSearch.Rows.Count = 0 Then
                Me.DDL_Members.DataSource = Nothing
                DDL_Members.DataBind()
                DDL_Members.Items.Clear()
                Label24.Text = "Total Record(s): 0 "

                Me.DDL_Members2.DataSource = Nothing
                DDL_Members2.DataBind()
                DDL_Members2.Items.Clear()
            Else
                RecordCount = dtSearch.Rows.Count
                Me.DDL_Members.DataSource = dtSearch
                Me.DDL_Members.DataTextField = "MEM_NAME"
                Me.DDL_Members.DataValueField = "MEM_ID"
                Me.DDL_Members.DataBind()
                DDL_Members.Items.Insert(0, "")
                Label24.Text = "Total Record(s): " & RecordCount
                DDL_Members.Focus()

                Me.DDL_Members2.DataSource = dtSearch
                Me.DDL_Members2.DataTextField = "MEM_NAME"
                Me.DDL_Members2.DataValueField = "MEM_ID"
                Me.DDL_Members2.DataBind()
                DDL_Members2.Items.Insert(0, "")
            End If
            ViewState("dt") = dtSearch
        Catch s As Exception
            Lbl_Error.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    Protected Sub DDL_Members_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDL_Members.SelectedIndexChanged
        Dim dt As New DataTable
        Try
            Dim MEM_ID As Integer = Nothing
            If Trim(DDL_Members.Text) <> "" Then
                MEM_ID = TrimX(DDL_Members.SelectedValue)
                MEM_ID = RemoveQuotes(MEM_ID)
            Else
                Image4.ImageUrl = Nothing
                Label19.Text = ""
                Label16.Text = ""
                Label3.Text = ""
                Label17.Text = ""
                Exit Sub
            End If

            Dim SQL As String = Nothing
            SQL = "SELECT MEMBERSHIPS.MEM_ID, MEMBERSHIPS.MEM_NO, MEMBERSHIPS.MEM_NAME, MEMBERSHIPS.CAT_ID, CATEGORIES.CAT_NAME,  MEMBERSHIPS.SUBCAT_ID, SUB_CATEGORIES.SUBCAT_NAME, MEMBERSHIPS.PHOTO FROM  MEMBERSHIPS LEFT OUTER JOIN SUB_CATEGORIES ON MEMBERSHIPS.SUBCAT_ID = SUB_CATEGORIES.SUBCAT_ID LEFT OUTER JOIN CATEGORIES ON MEMBERSHIPS.CAT_ID = CATEGORIES.CAT_ID WHERE (MEMBERSHIPS.LIB_CODE ='" & Trim(LibCode) & "'  AND MEMBERSHIPS.MEM_ID ='" & Trim(MEM_ID) & "')"

            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            da.Fill(ds)

            dt = ds.Tables(0).Copy

            ClearFields()
            Label26.Text = ""
            Label27.Text = ""
            If dt.Rows.Count <> 0 Then
                If dt.Rows(0).Item("MEM_NO").ToString <> "" Then
                    Label19.Text = dt.Rows(0).Item("MEM_NO").ToString
                Else
                    Label19.Text = ""
                End If

                If dt.Rows(0).Item("MEM_NAME").ToString <> "" Then
                    Label16.Text = dt.Rows(0).Item("MEM_NAME").ToString
                Else
                    Label16.Text = ""
                End If

                If dt.Rows(0).Item("CAT_NAME").ToString <> "" Then
                    Label3.Text = dt.Rows(0).Item("CAT_NAME").ToString
                Else
                    Label3.Text = ""
                End If

                If dt.Rows(0).Item("SUBCAT_NAME").ToString <> "" Then
                    Label17.Text = dt.Rows(0).Item("SUBCAT_NAME").ToString
                Else
                    Label17.Text = ""
                End If

                If dt.Rows(0).Item("PHOTO").ToString <> "" Then
                    Dim strURL As String = "~/Circulation/Member_GetPhoto.aspx?MEM_ID=" & Trim(dt.Rows(0).Item("MEM_ID").ToString) & ""
                    Image4.ImageUrl = strURL
                    Image4.Visible = True
                Else
                    Image4.Visible = False
                End If
            Else
                Image4.ImageUrl = Nothing
                Label19.Text = ""
                Label16.Text = ""
                Label3.Text = ""
                Label17.Text = ""
            End If
            DDL_Members.Focus()
            PopulateGrid()
        Catch s As Exception
            Lbl_Error.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'Populate the users in grid     'search users
    Public Sub Search_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Search_Bttn.Click
        Dim dtUsers As DataTable = Nothing
        Try
            Dim c, counter1, counter2, counter3, counter4, counter5, counter6, counter7, counter8 As Integer
            Dim iloop As Integer
            Dim strcurrentchar As Object

            'search string validation
            Dim REC_ID As Integer = Nothing
            If DDL_Receipts.Text <> "" Then
                REC_ID = DDL_Receipts.SelectedValue

                If Not IsNumeric(REC_ID.ToString) Then
                    Lbl_Error.Text = "Error: Input is not Valid !"
                    DDL_Receipts.Focus()
                    Exit Sub
                End If

                If REC_ID.ToString.Length > 5 Then
                    Lbl_Error.Text = "Error: Input is not Valid !"
                    DDL_Receipts.Focus()
                    Exit Sub
                End If
                REC_ID = " " & REC_ID & " "
                If InStr(1, REC_ID, "CREATE", 1) > 0 Or InStr(1, REC_ID, "DELETE", 1) > 0 Or InStr(1, REC_ID, "DROP", 1) > 0 Or InStr(1, REC_ID, "INSERT", 1) > 1 Or InStr(1, REC_ID, "TRACK", 1) > 1 Or InStr(1, REC_ID, "TRACE", 1) > 1 Then
                    Lbl_Error.Text = "Error: Input is not Valid !"
                    DDL_Receipts.Focus()
                    Exit Sub
                End If
                REC_ID = TrimX(REC_ID)
                'check unwanted characters
                c = 0
                counter1 = 0
                For iloop = 1 To Len(REC_ID.ToString)
                    strcurrentchar = Mid(REC_ID, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';<>=%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter1 = 1
                        End If
                    End If
                Next
                If counter1 = 1 Then
                    Lbl_Error.Text = "Error: Input is not Valid !"
                    DDL_Receipts.Focus()
                    Exit Sub
                End If
            Else
                REC_ID = Nothing
            End If

            Dim MEM_ID As Integer = Nothing
            If DDL_Members2.Text <> "" Then
                MEM_ID = DDL_Members2.SelectedValue

                If Not IsNumeric(MEM_ID.ToString) Then
                    Lbl_Error.Text = "Error: Input is not Valid !"
                    DDL_Members2.Focus()
                    Exit Sub
                End If

                If MEM_ID.ToString.Length > 5 Then
                    Lbl_Error.Text = "Error: Input is not Valid !"
                    DDL_Members2.Focus()
                    Exit Sub
                End If
                MEM_ID = " " & MEM_ID & " "
                If InStr(1, MEM_ID, "CREATE", 1) > 0 Or InStr(1, MEM_ID, "DELETE", 1) > 0 Or InStr(1, MEM_ID, "DROP", 1) > 0 Or InStr(1, MEM_ID, "INSERT", 1) > 1 Or InStr(1, MEM_ID, "TRACK", 1) > 1 Or InStr(1, MEM_ID, "TRACE", 1) > 1 Then
                    Lbl_Error.Text = "Error: Input is not Valid !"
                    DDL_Members2.Focus()
                    Exit Sub
                End If
                MEM_ID = TrimX(MEM_ID)
                'check unwanted characters
                c = 0
                counter2 = 0
                For iloop = 1 To Len(MEM_ID.ToString)
                    strcurrentchar = Mid(MEM_ID, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';<>=%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter2 = 1
                        End If
                    End If
                Next
                If counter2 = 1 Then
                    Lbl_Error.Text = "Error: Input is not Valid !"
                    DDL_Members2.Focus()
                    Exit Sub
                End If
            Else
                MEM_ID = Nothing
            End If

            Dim HOLD_ID As Integer = Nothing
            If DDL_AccessionNo2.Text <> "" Then
                HOLD_ID = DDL_AccessionNo2.SelectedValue

                If Not IsNumeric(HOLD_ID.ToString) Then
                    Lbl_Error.Text = "Error: Input is not Valid !"
                    DDL_AccessionNo2.Focus()
                    Exit Sub
                End If

                If HOLD_ID.ToString.Length > 5 Then
                    Lbl_Error.Text = "Error: Input is not Valid !"
                    DDL_AccessionNo2.Focus()
                    Exit Sub
                End If
                HOLD_ID = " " & HOLD_ID & " "
                If InStr(1, HOLD_ID, "CREATE", 1) > 0 Or InStr(1, HOLD_ID, "DELETE", 1) > 0 Or InStr(1, HOLD_ID, "DROP", 1) > 0 Or InStr(1, HOLD_ID, "INSERT", 1) > 1 Or InStr(1, HOLD_ID, "TRACK", 1) > 1 Or InStr(1, HOLD_ID, "TRACE", 1) > 1 Then
                    Lbl_Error.Text = "Error: Input is not Valid !"
                    DDL_AccessionNo2.Focus()
                    Exit Sub
                End If
                HOLD_ID = TrimX(HOLD_ID)
                'check unwanted characters
                c = 0
                counter3 = 0
                For iloop = 1 To Len(HOLD_ID.ToString)
                    strcurrentchar = Mid(HOLD_ID, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';<>=%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter3 = 1
                        End If
                    End If
                Next
                If counter3 = 1 Then
                    Lbl_Error.Text = "Error: Input is not Valid !"
                    DDL_AccessionNo2.Focus()
                    Exit Sub
                End If
            Else
                HOLD_ID = Nothing
            End If

            Dim COPY_ID As Integer = Nothing
            If DDL_CopyID2.Text <> "" Then
                COPY_ID = DDL_CopyID2.SelectedValue

                If Not IsNumeric(COPY_ID.ToString) Then
                    Lbl_Error.Text = "Error: Input is not Valid !"
                    DDL_CopyID2.Focus()
                    Exit Sub
                End If

                If COPY_ID.ToString.Length > 5 Then
                    Lbl_Error.Text = "Error: Input is not Valid !"
                    DDL_CopyID2.Focus()
                    Exit Sub
                End If
                COPY_ID = " " & COPY_ID & " "
                If InStr(1, COPY_ID, "CREATE", 1) > 0 Or InStr(1, COPY_ID, "DELETE", 1) > 0 Or InStr(1, COPY_ID, "DROP", 1) > 0 Or InStr(1, COPY_ID, "INSERT", 1) > 1 Or InStr(1, COPY_ID, "TRACK", 1) > 1 Or InStr(1, COPY_ID, "TRACE", 1) > 1 Then
                    Lbl_Error.Text = "Error: Input is not Valid !"
                    DDL_CopyID2.Focus()
                    Exit Sub
                End If
                COPY_ID = TrimX(COPY_ID)
                'check unwanted characters
                c = 0
                counter4 = 0
                For iloop = 1 To Len(COPY_ID.ToString)
                    strcurrentchar = Mid(COPY_ID, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';<>=%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter4 = 1
                        End If
                    End If
                Next
                If counter4 = 1 Then
                    Lbl_Error.Text = "Error: Input is not Valid !"
                    DDL_CopyID2.Focus()
                    Exit Sub
                End If
            Else
                COPY_ID = Nothing
            End If

            'validation for RECD_FOR
            Dim RECD_FOR As Object = Nothing
            If DDL_PmtFor2.Text <> "" Then
                RECD_FOR = Trim(DDL_PmtFor2.SelectedValue)
                RECD_FOR = RemoveQuotes(RECD_FOR)
                If RECD_FOR.Length > 1 Then 'maximum length
                    Lbl_Error.Text = " Data must be of Proper Length.. "
                    DDL_PmtFor2.Focus()
                    Exit Sub
                End If

                RECD_FOR = " " & RECD_FOR & " "
                If InStr(1, RECD_FOR, "CREATE", 1) > 0 Or InStr(1, RECD_FOR, "DELETE", 1) > 0 Or InStr(1, RECD_FOR, "DROP", 1) > 0 Or InStr(1, RECD_FOR, "INSERT", 1) > 1 Or InStr(1, RECD_FOR, "TRACK", 1) > 1 Or InStr(1, RECD_FOR, "TRACE", 1) > 1 Then
                    Lbl_Error.Text = "Do not use Reserve Word"
                    DDL_PmtFor2.Focus()
                    Exit Sub
                End If
                RECD_FOR = TrimAll(RECD_FOR)
                'check unwanted characters
                c = 0
                counter5 = 0
                For iloop = 1 To Len(RECD_FOR.ToString)
                    strcurrentchar = Mid(RECD_FOR, iloop, 1)
                    If c = 0 Then
                        If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter5 = 1
                        End If
                    End If
                Next
                If counter5 = 1 Then
                    Lbl_Error.Text = " Input  is not Valid!"
                    DDL_PmtFor2.Focus()
                    Exit Sub
                End If
            Else
                RECD_FOR = ""
            End If

            'validation for STATUS
            Dim STATUS As Object = Nothing
            If DDL_Status.Text <> "" Then
                STATUS = Trim(DDL_Status.SelectedValue)
                STATUS = RemoveQuotes(STATUS)
                If STATUS.Length > 30 Then 'maximum length
                    Lbl_Error.Text = " Data must be of Proper Length.. "
                    DDL_Status.Focus()
                    Exit Sub
                End If

                STATUS = " " & STATUS & " "
                If InStr(1, STATUS, "CREATE", 1) > 0 Or InStr(1, STATUS, "DELETE", 1) > 0 Or InStr(1, STATUS, "DROP", 1) > 0 Or InStr(1, STATUS, "INSERT", 1) > 1 Or InStr(1, STATUS, "TRACK", 1) > 1 Or InStr(1, STATUS, "TRACE", 1) > 1 Then
                    Lbl_Error.Text = "Do not use Reserve Word"
                    DDL_Status.Focus()
                    Exit Sub
                End If
                STATUS = TrimAll(STATUS)
                'check unwanted characters
                c = 0
                counter6 = 0
                For iloop = 1 To Len(STATUS.ToString)
                    strcurrentchar = Mid(STATUS, iloop, 1)
                    If c = 0 Then
                        If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter6 = 1
                        End If
                    End If
                Next
                If counter6 = 1 Then
                    Lbl_Error.Text = " Input  is not Valid!"
                    DDL_Status.Focus()
                    Exit Sub
                End If
            Else
                STATUS = ""
            End If

            Dim DATE_RECD_FROM As Object = Nothing
            If txt_Rect_SDate.Text <> "" Then
                DATE_RECD_FROM = TrimX(txt_Rect_SDate.Text)
                If Len(DATE_RECD_FROM) <> 10 Then
                    Lbl_Error.Text = " Input is not Valid..."
                    Me.txt_Rect_SDate.Focus()
                    Exit Sub
                End If
                DATE_RECD_FROM = " " & DATE_RECD_FROM & " "
                If InStr(1, DATE_RECD_FROM, "CREATE", 1) > 0 Or InStr(1, DATE_RECD_FROM, "DELETE", 1) > 0 Or InStr(1, DATE_RECD_FROM, "DROP", 1) > 0 Or InStr(1, DATE_RECD_FROM, "INSERT", 1) > 1 Or InStr(1, DATE_RECD_FROM, "TRACK", 1) > 1 Or InStr(1, DATE_RECD_FROM, "TRACE", 1) > 1 Then
                    Lbl_Error.Text = "  Input is not Valid... "
                    Me.txt_Rect_SDate.Focus()
                    Exit Sub
                End If
                DATE_RECD_FROM = TrimX(DATE_RECD_FROM)
                'check unwanted characters
                c = 0
                counter7 = 0
                For iloop = 1 To Len(DATE_RECD_FROM.ToString)
                    strcurrentchar = Mid(DATE_RECD_FROM, iloop, 1)
                    If c = 0 Then
                        If Not InStr("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz,$#@!^&*_+';<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter7 = 1
                        End If
                    End If
                Next
                If counter7 = 1 Then
                    Lbl_Error.Text = "data is not Valid... "
                    Me.txt_Rect_SDate.Focus()
                    Exit Sub
                End If
                DATE_RECD_FROM = Convert.ToDateTime(DATE_RECD_FROM, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us
            Else
                DATE_RECD_FROM = ""
            End If

            Dim DATE_RECD_TO As Object = Nothing
            If txt_Rect_EDate.Text <> "" Then
                DATE_RECD_TO = TrimX(txt_Rect_EDate.Text)
                If Len(DATE_RECD_TO) <> 10 Then
                    Lbl_Error.Text = " Input is not Valid..."
                    Me.txt_Rect_EDate.Focus()
                    Exit Sub
                End If
                DATE_RECD_TO = " " & DATE_RECD_TO & " "
                If InStr(1, DATE_RECD_TO, "CREATE", 1) > 0 Or InStr(1, DATE_RECD_TO, "DELETE", 1) > 0 Or InStr(1, DATE_RECD_TO, "DROP", 1) > 0 Or InStr(1, DATE_RECD_TO, "INSERT", 1) > 1 Or InStr(1, DATE_RECD_TO, "TRACK", 1) > 1 Or InStr(1, DATE_RECD_TO, "TRACE", 1) > 1 Then
                    Lbl_Error.Text = "  Input is not Valid... "
                    Me.txt_Rect_EDate.Focus()
                    Exit Sub
                End If
                DATE_RECD_TO = TrimX(DATE_RECD_TO)
                'check unwanted characters
                c = 0
                counter8 = 0
                For iloop = 1 To Len(DATE_RECD_TO.ToString)
                    strcurrentchar = Mid(DATE_RECD_TO, iloop, 1)
                    If c = 0 Then
                        If Not InStr("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz,$#@!^&*_+';<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter8 = 1
                        End If
                    End If
                Next
                If counter8 = 1 Then
                    Lbl_Error.Text = "data is not Valid... "
                    Me.txt_Rect_EDate.Focus()
                    Exit Sub
                End If
                DATE_RECD_TO = Convert.ToDateTime(DATE_RECD_TO, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us
            Else
                DATE_RECD_TO = ""
            End If

            Dim SQL As String = Nothing
            SQL = "SELECT RECEIPTS.*, MEMBERSHIPS.MEM_NO, MEMBERSHIPS.MEM_NAME, MEMBERSHIPS.MEM_RES_ADD, " & _
                  " MEMBERSHIPS.MEM_OFF_ADD, MEMBERSHIPS.MEM_EMAIL, HOLDINGS_CATS_AUTHORS_VIEW.ACCESSION_NO, " & _
                  " HOLDINGS_CATS_AUTHORS_VIEW.STANDARD_NO, HOLDINGS_CATS_AUTHORS_VIEW.TITLE, " & _
                  " HOLDINGS_CATS_AUTHORS_VIEW.SUB_TITLE, HOLDINGS_CATS_AUTHORS_VIEW.AUTHOR1, " & _
                  " HOLDINGS_CATS_AUTHORS_VIEW.AUTHOR2, HOLDINGS_CATS_AUTHORS_VIEW.AUTHOR3, " & _
                  " HOLDINGS_CATS_AUTHORS_VIEW.YEAR_OF_PUB, HOLDINGS_CATS_AUTHORS_VIEW.EDITION, " & _
                  " HOLDINGS_CATS_AUTHORS_VIEW.PLACE_OF_PUB, HOLDINGS_CATS_AUTHORS_VIEW.PUB_NAME, " & _
                  " CATS_LOOSE_ISSUES_COPIES_VIEW.TITLE AS JTITLE, CATS_LOOSE_ISSUES_COPIES_VIEW.PUB_NAME AS JPUBNAME, " & _
                  " CATS_LOOSE_ISSUES_COPIES_VIEW.VOL_NO, CATS_LOOSE_ISSUES_COPIES_VIEW.ISSUE_NO, " & _
                  " CATS_LOOSE_ISSUES_COPIES_VIEW.SUBS_YEAR " & _
                  " FROM  RECEIPTS LEFT OUTER JOIN " & _
                  " CATS_LOOSE_ISSUES_COPIES_VIEW ON RECEIPTS.COPY_ID = CATS_LOOSE_ISSUES_COPIES_VIEW.COPY_ID LEFT OUTER JOIN " & _
                  " HOLDINGS_CATS_AUTHORS_VIEW ON RECEIPTS.HOLD_ID = HOLDINGS_CATS_AUTHORS_VIEW.HOLD_ID LEFT OUTER JOIN " & _
                  " MEMBERSHIPS ON RECEIPTS.MEM_ID = MEMBERSHIPS.MEM_ID WHERE (RECEIPTS.LIB_CODE ='" & Trim(LibCode) & "') "

            If REC_ID = 0 And MEM_ID = 0 And HOLD_ID = 0 And COPY_ID = 0 And RECD_FOR = "" And DATE_RECD_FROM = "" And DATE_RECD_TO = "" And STATUS = "" Then
                SQL = SQL
            End If
            If REC_ID <> 0 And MEM_ID = 0 And HOLD_ID = 0 And COPY_ID = 0 And RECD_FOR = "" And DATE_RECD_FROM = "" And DATE_RECD_TO = "" And STATUS = "" Then
                SQL = SQL & " AND (RECEIPTS.REC_ID = '" & Trim(REC_ID) & "') "
            End If
            If REC_ID = 0 And MEM_ID <> 0 And HOLD_ID = 0 And COPY_ID = 0 And RECD_FOR = "" And DATE_RECD_FROM = "" And DATE_RECD_TO = "" And STATUS = "" Then
                SQL = SQL & " AND (RECEIPTS.MEM_ID = '" & Trim(MEM_ID) & "') "
            End If
            If REC_ID = 0 And MEM_ID <> 0 And HOLD_ID = 0 And COPY_ID = 0 And RECD_FOR <> "" And DATE_RECD_FROM = "" And DATE_RECD_TO = "" And STATUS = "" Then
                SQL = SQL & " AND (RECEIPTS.MEM_ID = '" & Trim(MEM_ID) & "') AND (RECEIPTS.RECD_FOR = '" & Trim(RECD_FOR) & "')"
            End If
            If REC_ID = 0 And MEM_ID <> 0 And HOLD_ID = 0 And COPY_ID = 0 And RECD_FOR <> "" And DATE_RECD_FROM <> "" And DATE_RECD_TO <> "" And STATUS = "" Then
                SQL = SQL & " AND (RECEIPTS.MEM_ID = '" & Trim(MEM_ID) & "') AND (RECEIPTS.RECD_FOR = '" & Trim(RECD_FOR) & "') AND (RECEIPTS.DATE_RECD >= '" & Trim(DATE_RECD_FROM) & "' AND DATE_RECD <=  '" & Trim(DATE_RECD_TO) & "') "
            End If
            If REC_ID = 0 And MEM_ID <> 0 And HOLD_ID = 0 And COPY_ID = 0 And RECD_FOR = "" And DATE_RECD_FROM <> "" And DATE_RECD_TO <> "" And STATUS = "" Then
                SQL = SQL & " AND (RECEIPTS.MEM_ID = '" & Trim(MEM_ID) & "') AND (RECEIPTS.DATE_RECD >= '" & Trim(DATE_RECD_FROM) & "' AND DATE_RECD <=  '" & Trim(DATE_RECD_TO) & "') "
            End If
            If REC_ID = 0 And MEM_ID = 0 And HOLD_ID <> 0 And COPY_ID = 0 And RECD_FOR = "" And DATE_RECD_FROM = "" And DATE_RECD_TO = "" And STATUS = "" Then
                SQL = SQL & " AND (RECEIPTS.HOLD_ID = '" & Trim(HOLD_ID) & "') "
            End If
            If REC_ID = 0 And MEM_ID = 0 And HOLD_ID <> 0 And COPY_ID = 0 And RECD_FOR = "" And DATE_RECD_FROM <> "" And DATE_RECD_TO <> "" And STATUS = "" Then
                SQL = SQL & " AND (RECEIPTS.HOLD_ID = '" & Trim(HOLD_ID) & "') AND (RECEIPTS.DATE_RECD >= '" & Trim(DATE_RECD_FROM) & "' AND DATE_RECD <=  '" & Trim(DATE_RECD_TO) & "')"
            End If
            If REC_ID = 0 And MEM_ID = 0 And HOLD_ID = 0 And COPY_ID <> 0 And RECD_FOR = "" And DATE_RECD_FROM = "" And DATE_RECD_TO = "" And STATUS = "" Then
                SQL = SQL & " AND (RECEIPTS.COPY_ID = '" & Trim(COPY_ID) & "') "
            End If
            If REC_ID = 0 And MEM_ID = 0 And HOLD_ID = 0 And COPY_ID <> 0 And RECD_FOR = "" And DATE_RECD_FROM <> "" And DATE_RECD_TO <> "" And STATUS = "" Then
                SQL = SQL & " AND (RECEIPTS.COPY_ID = '" & Trim(COPY_ID) & "') AND (RECEIPTS.DATE_RECD >= '" & Trim(DATE_RECD_FROM) & "' AND DATE_RECD <=  '" & Trim(DATE_RECD_TO) & "')"
            End If
            If REC_ID = 0 And MEM_ID = 0 And HOLD_ID = 0 And COPY_ID = 0 And RECD_FOR <> "" And DATE_RECD_FROM = "" And DATE_RECD_TO = "" And STATUS = "" Then
                SQL = SQL & " AND (RECEIPTS.RECD_FOR = '" & Trim(RECD_FOR) & "') "
            End If
            If REC_ID = 0 And MEM_ID = 0 And HOLD_ID = 0 And COPY_ID = 0 And RECD_FOR <> "" And DATE_RECD_FROM <> "" And DATE_RECD_TO <> "" And STATUS = "" Then
                SQL = SQL & " AND (RECEIPTS.RECD_FOR = '" & Trim(RECD_FOR) & "') AND (RECEIPTS.DATE_RECD >= '" & Trim(DATE_RECD_FROM) & "' AND DATE_RECD <=  '" & Trim(DATE_RECD_TO) & "') "
            End If
            If REC_ID = 0 And MEM_ID = 0 And HOLD_ID = 0 And COPY_ID = 0 And RECD_FOR = "" And DATE_RECD_FROM <> "" And DATE_RECD_TO <> "" And STATUS = "" Then
                SQL = SQL & " AND (RECEIPTS.DATE_RECD >= '" & Trim(DATE_RECD_FROM) & "' AND DATE_RECD <=  '" & Trim(DATE_RECD_TO) & "') "
            End If
            If REC_ID = 0 And MEM_ID = 0 And HOLD_ID = 0 And COPY_ID = 0 And RECD_FOR = "" And DATE_RECD_FROM <> "" And DATE_RECD_TO <> "" And STATUS <> "" Then
                SQL = SQL & " AND (RECEIPTS.DATE_RECD >= '" & Trim(DATE_RECD_FROM) & "' AND DATE_RECD <=  '" & Trim(DATE_RECD_TO) & "') AND (STATUS= '" & Trim(STATUS) & "') "
            End If
            If REC_ID = 0 And MEM_ID = 0 And HOLD_ID = 0 And COPY_ID = 0 And RECD_FOR = "" And DATE_RECD_FROM = "" And DATE_RECD_TO = "" And STATUS <> "" Then
                SQL = SQL & " AND  (STATUS= '" & Trim(STATUS) & "') "
            End If

            SQL = SQL & " ORDER BY RECEIPTS.REC_ID ASC "

            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            da.Fill(ds)

            dtUsers = ds.Tables(0).Copy
            Dim RecordCount As Long = 0
            If dtUsers.Rows.Count = 0 Then
                Me.Grid1_Search.DataSource = Nothing
                Grid1_Search.DataBind()
                Label7.Text = "Total Record(s): 0 "
                Delete_Bttn.Enabled = False
                Print_Summary_Bttn.Visible = False
                Print_Details_Bttn.Visible = False
            Else
                Grid1_Search.Visible = True
                RecordCount = dtUsers.Rows.Count
                Grid1_Search.DataSource = dtUsers
                Grid1_Search.DataBind()
                Label7.Text = "Total Record(s): " & RecordCount
                Delete_Bttn.Enabled = True
                Print_Summary_Bttn.Visible = True
                Print_Details_Bttn.Visible = True
            End If
            ViewState("dt") = dtUsers
            UpdatePanel1.Update()
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'grid view page index changing event
    Protected Sub Grid1_Search_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles Grid1_Search.PageIndexChanging
        Try
            'rebind datagrid
            Grid1_Search.DataSource = ViewState("dt") 'temp
            Grid1_Search.PageIndex = e.NewPageIndex
            index = e.NewPageIndex * Grid1_Search.PageSize
            Grid1_Search.DataBind()
        Catch s As Exception
            Lbl_Error.Text = "Error:  there is error in page index !"
        End Try
    End Sub
    'gridview sorting event
    Protected Sub Grid1_Search_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles Grid1_Search.Sorting
        Dim temp As DataTable = CType(ViewState("dt"), DataTable)
        GridViewSortExpression = e.SortExpression
        Grid1_Search.DataSource = temp
        Dim pageIndex As Integer = Grid1_Search.PageIndex
        Grid1_Search.DataSource = SortDataTable(Grid1.DataSource, False)
        Grid1_Search.DataBind()
        Grid1_Search.PageIndex = pageIndex
    End Sub
    'get value of row from grid
    Private Sub Grid1_Search_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles Grid1_Search.RowCommand
        Dim Command As SqlCommand = Nothing
        Dim dr As SqlDataReader = Nothing
        Try
            If e.CommandName = "Select" Then
                Dim myRowID, REC_ID As Integer
                myRowID = e.CommandArgument.ToString()
                REC_ID = Grid1_Search.DataKeys(myRowID).Value
                If Not String.IsNullOrEmpty(REC_ID) And REC_ID <> 0 Then
                    Label26.Text = REC_ID
                    Dim iloop As Integer
                    Dim strcurrentchar As Object
                    Dim c As Integer
                    Dim counter1 As Integer

                    REC_ID = TrimX(REC_ID)
                    REC_ID = RemoveQuotes(REC_ID)

                    If Len(REC_ID).ToString > 10 Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Length of Input is not Proper!');", True)
                        Exit Sub
                    End If

                    If Not IsNumeric(REC_ID.ToString) = True Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Input type is not Proper!');", True)
                        Exit Sub
                    End If

                    REC_ID = " " & REC_ID & " "
                    If InStr(1, REC_ID, " CREATE ", 1) > 0 Or InStr(1, REC_ID, " DELETE ", 1) > 0 Or InStr(1, REC_ID, " DROP ", 1) > 0 Or InStr(1, REC_ID, " INSERT ", 1) > 1 Or InStr(1, REC_ID, " TRACK ", 1) > 1 Or InStr(1, REC_ID, " TRACE ", 1) > 1 Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Do Not Use Reserve Words!');", True)
                        Exit Sub
                    End If
                    REC_ID = TrimX(REC_ID)

                    ClearFields()
                    'get record details from database
                    Dim SQL As String = Nothing
                    SQL = " SELECT *  FROM RECEIPTS WHERE (REC_ID = '" & Trim(REC_ID) & "') AND (LIB_CODE = '" & Trim(LibCode) & "') "
                    Command = New SqlCommand(SQL, SqlConn)
                    SqlConn.Open()
                    dr = Command.ExecuteReader(CommandBehavior.CloseConnection)
                    dr.Read()

                    If dr.HasRows = True Then
                        If dr.Item("DATE_RECD").ToString <> "" Then
                            txt_Rect_Date.Text = Format(dr.Item("DATE_RECD"), "dd/MM/yyyy")
                        Else
                            txt_Rect_Date.Text = ""
                        End If

                        If dr.Item("RECD_FOR").ToString <> "" Then
                            DDL_PmtFor.SelectedValue = dr.Item("RECD_FOR").ToString
                        Else
                            DDL_PmtFor.ClearSelection()
                        End If

                        If dr.Item("SECURITY_DEPOSIT").ToString <> "" Then
                            txt_Rect_SecurityDeposit.Text = dr.Item("SECURITY_DEPOSIT").ToString
                            TR_SECURITY_DEPOSIT.Visible = True
                            TR_AMOUNT_RECD.Visible = False
                        Else
                            txt_Rect_SecurityDeposit.Text = ""
                            TR_SECURITY_DEPOSIT.Visible = False
                            TR_AMOUNT_RECD.Visible = True
                        End If

                        If dr.Item("AMOUNT_DUE").ToString <> "" Then
                            txt_Rect_AmountDue.Text = dr.Item("AMOUNT_DUE").ToString
                        Else
                            txt_Rect_AmountDue.Text = ""
                        End If

                        If dr.Item("AMOUNT_RECD").ToString <> "" Then
                            txt_Rect_AmountRecd.Text = dr.Item("AMOUNT_RECD").ToString
                        Else
                            txt_Rect_AmountRecd.Text = ""
                        End If

                        If dr.Item("MEM_PERIOD").ToString <> "" Then
                            txt_Rect_Period.Text = dr.Item("MEM_PERIOD").ToString
                        Else
                            txt_Rect_Period.Text = ""
                        End If

                        If dr.Item("PMT_MODE").ToString <> "" Then
                            DDL_PmtMode.SelectedValue = dr.Item("PMT_MODE").ToString
                        Else
                            DDL_PmtMode.ClearSelection()
                        End If

                        If dr.Item("CHEQUE_NO").ToString <> "" Then
                            txt_Rect_ChqNo.Text = dr.Item("CHEQUE_NO").ToString
                        Else
                            txt_Rect_ChqNo.Text = ""
                        End If

                        If dr.Item("CHEQUE_DATE").ToString <> "" Then
                            txt_Rect_ChqDate.Text = Format(dr.Item("CHEQUE_DATE"), "dd/MM/yyyy")
                        Else
                            txt_Rect_ChqDate.Text = ""
                        End If

                        If dr.Item("HOLD_ID").ToString <> "" Then
                            DDL_AccessionNo.SelectedValue = dr.Item("HOLD_ID").ToString
                        Else
                            DDL_AccessionNo.ClearSelection()
                        End If

                        If dr.Item("COPY_ID").ToString <> "" Then
                            DDL_CopyID.SelectedValue = dr.Item("COPY_ID").ToString
                        Else
                            DDL_CopyID.ClearSelection()
                        End If

                        If dr.Item("REMARKS").ToString <> "" Then
                            txt_Rect_Remarks.Text = dr.Item("REMARKS").ToString
                        Else
                            txt_Rect_Remarks.Text = ""
                        End If

                        If dr.Item("STATUS").ToString <> "" Then
                            Label27.Text = dr.Item("STATUS").ToString
                        Else
                            Label27.Text = ""
                        End If

                        If dr.Item("MEM_ID").ToString <> "" Then
                            DDL_Members.SelectedValue = dr.Item("MEM_ID").ToString
                        Else
                            DDL_Members.ClearSelection()
                        End If

                        Grid1.DataSource = Nothing
                        Grid1.DataBind()
                        Image4.ImageUrl = Nothing
                        Label19.Text = ""
                        Label16.Text = ""
                        Label3.Text = ""
                        Label17.Text = ""

                        Rect_Save_Bttn.Visible = False
                        Rect_Update_Bttn.Visible = True
                        Rect_Delete_Bttn.Visible = True
                        dr.Close()
                    Else
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Record No Selected for Edit!');", True)
                    End If
                Else
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Record No Selected for Edit!');", True)
                End If
            End If
            txt_Rect_Date.Focus()
        Catch s As Exception
            Lbl_Error.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub 'Grid1_ItemCommand
    Public Sub PopulateGrid()
        Dim dtSearch As DataTable = Nothing
        Try
            '**********************************************************************************
            Dim SQL As String = Nothing
            ' SQL = "SELECT * FROM RECEIPTS WHERE (LIB_CODE ='" & Trim(LibCode) & "') AND (MEM_ID = '" & Trim(DDL_Members.SelectedValue) & "') ORDER BY REC_ID DESC; "
            SQL = "SELECT RECEIPTS.*, HOLDINGS.ACCESSION_NO FROM RECEIPTS  LEFT OUTER JOIN HOLDINGS ON RECEIPTS.HOLD_ID = HOLDINGS.HOLD_ID WHERE (RECEIPTS.LIB_CODE ='" & Trim(LibCode) & "') AND (RECEIPTS.MEM_ID = '" & Trim(DDL_Members.SelectedValue) & "') ORDER BY RECEIPTS.REC_ID DESC; "
            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            da.Fill(ds)

            dtSearch = ds.Tables(0).Copy
            Dim RecordCount As Long = 0
            If dtSearch.Rows.Count = 0 Then
                Me.Grid1.DataSource = Nothing
                Grid1.DataBind()
                Label25.Text = "Total Record(s): 0 "
                Grid1.Visible = False
                Grid1.Enabled = False
                Rect_DeleteAll_Bttn.Visible = False
            Else
                Grid1.Visible = True
                RecordCount = dtSearch.Rows.Count
                Grid1.DataSource = dtSearch
                Grid1.DataBind()
                Label25.Text = "Total Record(s): " & RecordCount
                Rect_DeleteAll_Bttn.Visible = True
            End If
            ViewState("dt") = dtSearch
        Catch s As Exception
            Lbl_Error.Text = "Error: " & (s.Message())
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
    Protected Sub Grid1_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles Grid1.PageIndexChanging
        Try
            'rebind datagrid
            Grid1.DataSource = ViewState("dt") 'temp
            Grid1.PageIndex = e.NewPageIndex
            index = e.NewPageIndex * Grid1.PageSize
            Grid1.DataBind()
        Catch s As Exception
            Lbl_Error.Text = "Error:  there is error in page index !"
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
    Protected Sub Grid1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles Grid1.Sorting
        Dim temp As DataTable = CType(ViewState("dt"), DataTable)
        GridViewSortExpression = e.SortExpression
        Grid1.DataSource = temp
        Dim pageIndex As Integer = Grid1.PageIndex
        Grid1.DataSource = SortDataTable(Grid1.DataSource, False)
        Grid1.DataBind()
        Grid1.PageIndex = pageIndex
    End Sub
    Protected Sub DDL_PmtFor_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDL_PmtFor.SelectedIndexChanged
        If DDL_PmtFor.SelectedValue = "S" Then
            TR_SECURITY_DEPOSIT.Visible = True
            TR_AMOUNT_RECD.Visible = False
            TR_ACCESSION.Visible = False
            TR_COPY.Visible = False
            txt_Rect_SecurityDeposit.Focus()
        Else
            TR_AMOUNT_RECD.Visible = True
            TR_SECURITY_DEPOSIT.Visible = False
            TR_ACCESSION.Visible = True
            TR_COPY.Visible = True
            txt_Rect_AmountDue.Focus()
        End If
    End Sub
    'save record    
    Protected Sub Rect_Save_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Rect_Save_Bttn.Click
        If IsPostBack = True Then
            'check unwanted characters
            Dim iloop As Integer
            Dim strcurrentchar As Object
            Dim c As Integer
            Dim counter1, counter2, counter3, counter4, counter5, counter6, counter7, counter8, counter9, counter10, counter11, counter12, counter13, counter14, counter15, counter16, counter17, counter18, counter19 As Integer
            Dim thisTransaction As SqlClient.SqlTransaction = Nothing
            Try
                If DDL_Members.Text = "" Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Plz Select the Member from Drop-Downn!');", True)
                    DDL_Members.Focus()
                    Exit Sub
                End If

                'MEM_ID
                Dim MEM_ID As Integer = Nothing
                If DDL_Members.Text <> "" Then
                    MEM_ID = TrimX(DDL_Members.SelectedValue)
                    If Len(MEM_ID) > 10 Then
                        Lbl_Error.Text = " Input is not Valid..."
                        Me.DDL_Members.Focus()
                        Exit Sub
                    End If
                    If Not IsNumeric(MEM_ID) = True Then
                        Lbl_Error.Text = " Input is not Valid..."
                        Me.DDL_Members.Focus()
                        Exit Sub
                    End If

                    MEM_ID = " " & MEM_ID & " "
                    If InStr(1, MEM_ID, "CREATE", 1) > 0 Or InStr(1, MEM_ID, "DELETE", 1) > 0 Or InStr(1, MEM_ID, "DROP", 1) > 0 Or InStr(1, MEM_ID, "INSERT", 1) > 1 Or InStr(1, MEM_ID, "TRACK", 1) > 1 Or InStr(1, MEM_ID, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "  Input is not Valid... "
                        Me.DDL_Members.Focus()
                        Exit Sub
                    End If
                    MEM_ID = TrimX(MEM_ID)
                    'check unwanted characters
                    c = 0
                    counter1 = 0
                    For iloop = 1 To Len(MEM_ID.ToString)
                        strcurrentchar = Mid(MEM_ID, iloop, 1)
                        If c = 0 Then
                            If Not InStr("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz,$#@!^&*_+'.;<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter1 = 1
                            End If
                        End If
                    Next
                    If counter1 = 1 Then
                        Lbl_Error.Text = "data is not Valid... "
                        Me.DDL_Members.Focus()
                        Exit Sub
                    End If
                Else
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Plz Select the Member from Drop-Downn!');", True)
                    DDL_Members.Focus()
                    Exit Sub
                End If

                'DATE_RECT
                Dim DATE_RECD As Object = Nothing
                If txt_Rect_Date.Text <> "" Then
                    DATE_RECD = TrimX(txt_Rect_Date.Text)
                    If Len(DATE_RECD) <> 10 Then
                        Lbl_Error.Text = " Input is not Valid..."
                        Me.txt_Rect_Date.Focus()
                        Exit Sub
                    End If
                    DATE_RECD = " " & DATE_RECD & " "
                    If InStr(1, DATE_RECD, "CREATE", 1) > 0 Or InStr(1, DATE_RECD, "DELETE", 1) > 0 Or InStr(1, DATE_RECD, "DROP", 1) > 0 Or InStr(1, DATE_RECD, "INSERT", 1) > 1 Or InStr(1, DATE_RECD, "TRACK", 1) > 1 Or InStr(1, DATE_RECD, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "  Input is not Valid... "
                        Me.txt_Rect_Date.Focus()
                        Exit Sub
                    End If
                    DATE_RECD = TrimX(DATE_RECD)
                    'check unwanted characters
                    c = 0
                    counter2 = 0
                    For iloop = 1 To Len(DATE_RECD.ToString)
                        strcurrentchar = Mid(DATE_RECD, iloop, 1)
                        If c = 0 Then
                            If Not InStr("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz,$#@!^&*_+';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter2 = 1
                            End If
                        End If
                    Next
                    If counter2 = 1 Then
                        Lbl_Error.Text = "data is not Valid... "
                        Me.txt_Rect_Date.Focus()
                        Exit Sub
                    End If
                    DATE_RECD = Convert.ToDateTime(DATE_RECD, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us
                Else
                    DATE_RECD = Now.Date
                End If

                'validation for RECD_FOR
                Dim RECD_FOR As Object = Nothing
                If DDL_PmtFor.Text <> "" Then
                    RECD_FOR = Trim(DDL_PmtFor.SelectedValue)
                    RECD_FOR = RemoveQuotes(RECD_FOR)
                    If RECD_FOR.Length > 1 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        DDL_PmtFor.Focus()
                        Exit Sub
                    End If

                    RECD_FOR = " " & RECD_FOR & " "
                    If InStr(1, RECD_FOR, "CREATE", 1) > 0 Or InStr(1, RECD_FOR, "DELETE", 1) > 0 Or InStr(1, RECD_FOR, "DROP", 1) > 0 Or InStr(1, RECD_FOR, "INSERT", 1) > 1 Or InStr(1, RECD_FOR, "TRACK", 1) > 1 Or InStr(1, RECD_FOR, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Do not use Reserve Word"
                        DDL_PmtFor.Focus()
                        Exit Sub
                    End If
                    RECD_FOR = TrimAll(RECD_FOR)
                    'check unwanted characters
                    c = 0
                    counter3 = 0
                    For iloop = 1 To Len(RECD_FOR.ToString)
                        strcurrentchar = Mid(RECD_FOR, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter3 = 1
                            End If
                        End If
                    Next
                    If counter3 = 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        DDL_PmtFor.Focus()
                        Exit Sub
                    End If
                Else
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Plz Select the Value from Drop-Down!');", True)
                    DDL_PmtFor.Focus()
                    Exit Sub
                End If

                If RECD_FOR = "S" Then
                    If txt_Rect_SecurityDeposit.Text = "" Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Plz Enter Amount for Security Deposit!');", True)
                        txt_Rect_SecurityDeposit.Focus()
                        Exit Sub
                    End If
                End If

                'validation for SECURITY_DEPOSIT 
                Dim SECURITY_DEPOSIT As Object = Nothing
                If TR_SECURITY_DEPOSIT.Visible = True Then
                    If txt_Rect_SecurityDeposit.Text <> "" Then
                        SECURITY_DEPOSIT = TrimX(txt_Rect_SecurityDeposit.Text)
                        SECURITY_DEPOSIT = RemoveQuotes(SECURITY_DEPOSIT)
                        If SECURITY_DEPOSIT.Length > 10 Then
                            Lbl_Error.Text = "Error: Input is not Valid !"
                            txt_Rect_SecurityDeposit.Focus()
                            Exit Sub
                        End If
                        SECURITY_DEPOSIT = " " & SECURITY_DEPOSIT & " "
                        If InStr(1, SECURITY_DEPOSIT, "CREATE", 1) > 0 Or InStr(1, SECURITY_DEPOSIT, "DELETE", 1) > 0 Or InStr(1, SECURITY_DEPOSIT, "DROP", 1) > 0 Or InStr(1, SECURITY_DEPOSIT, "INSERT", 1) > 1 Or InStr(1, SECURITY_DEPOSIT, "TRACK", 1) > 1 Or InStr(1, SECURITY_DEPOSIT, "TRACE", 1) > 1 Then
                            Lbl_Error.Text = "Error: Input is not Valid !"
                            txt_Rect_SecurityDeposit.Focus()
                            Exit Sub
                        End If
                        SECURITY_DEPOSIT = TrimX(SECURITY_DEPOSIT)
                        'check unwanted characters
                        c = 0
                        counter4 = 0
                        For iloop = 1 To Len(SECURITY_DEPOSIT.ToString)
                            strcurrentchar = Mid(SECURITY_DEPOSIT, iloop, 1)
                            If c = 0 Then
                                If Not InStr("'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz~`!@#$%^&*()_-++<>?/,;<>=()%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter4 = 1
                                End If
                            End If
                        Next
                        If counter4 = 1 Then
                            Lbl_Error.Text = "Error: Input is not Valid !"
                            txt_Rect_SecurityDeposit.Focus()
                            Exit Sub
                        End If
                    Else
                        Lbl_Error.Text = "Plz Enter the Item Price !"
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Plz Enter Amount for Security Deposit!');", True)
                        txt_Rect_SecurityDeposit.Focus()
                        Exit Sub
                    End If
                Else
                    SECURITY_DEPOSIT = Nothing
                End If

                'validation for AMOUNT_DUE 
                Dim AMOUNT_DUE As Object = Nothing
                Dim AMOUNT_RECD As Object = Nothing
                If TR_AMOUNT_RECD.Visible = True Then
                    If txt_Rect_AmountDue.Text <> "" Then
                        AMOUNT_DUE = TrimX(txt_Rect_AmountDue.Text)
                        AMOUNT_DUE = RemoveQuotes(AMOUNT_DUE)
                        If AMOUNT_DUE.Length > 10 Then
                            Lbl_Error.Text = "Error: Input is not Valid !"
                            txt_Rect_AmountDue.Focus()
                            Exit Sub
                        End If
                        AMOUNT_DUE = " " & AMOUNT_DUE & " "
                        If InStr(1, AMOUNT_DUE, "CREATE", 1) > 0 Or InStr(1, AMOUNT_DUE, "DELETE", 1) > 0 Or InStr(1, AMOUNT_DUE, "DROP", 1) > 0 Or InStr(1, AMOUNT_DUE, "INSERT", 1) > 1 Or InStr(1, AMOUNT_DUE, "TRACK", 1) > 1 Or InStr(1, AMOUNT_DUE, "TRACE", 1) > 1 Then
                            Lbl_Error.Text = "Error: Input is not Valid !"
                            txt_Rect_AmountDue.Focus()
                            Exit Sub
                        End If
                        AMOUNT_DUE = TrimX(AMOUNT_DUE)
                        'check unwanted characters
                        c = 0
                        counter5 = 0
                        For iloop = 1 To Len(AMOUNT_DUE.ToString)
                            strcurrentchar = Mid(AMOUNT_DUE, iloop, 1)
                            If c = 0 Then
                                If Not InStr("'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz~`!@#$%^&*()_-++<>?/,;<>=()%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter5 = 1
                                End If
                            End If
                        Next
                        If counter5 = 1 Then
                            Lbl_Error.Text = "Error: Input is not Valid !"
                            txt_Rect_AmountDue.Focus()
                            Exit Sub
                        End If
                    Else
                        Lbl_Error.Text = "Plz Enter the Item Price !"
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Plz Enter Amount Due!');", True)
                        txt_Rect_AmountDue.Focus()
                        Exit Sub
                    End If

                    'amount Received
                    If txt_Rect_AmountRecd.Text <> "" Then
                        AMOUNT_RECD = TrimX(txt_Rect_AmountRecd.Text)
                        AMOUNT_RECD = RemoveQuotes(AMOUNT_RECD)
                        If AMOUNT_RECD.Length > 10 Then
                            Lbl_Error.Text = "Error: Input is not Valid !"
                            txt_Rect_AmountRecd.Focus()
                            Exit Sub
                        End If
                        AMOUNT_RECD = " " & AMOUNT_RECD & " "
                        If InStr(1, AMOUNT_RECD, "CREATE", 1) > 0 Or InStr(1, AMOUNT_RECD, "DELETE", 1) > 0 Or InStr(1, AMOUNT_RECD, "DROP", 1) > 0 Or InStr(1, AMOUNT_RECD, "INSERT", 1) > 1 Or InStr(1, AMOUNT_RECD, "TRACK", 1) > 1 Or InStr(1, AMOUNT_RECD, "TRACE", 1) > 1 Then
                            Lbl_Error.Text = "Error: Input is not Valid !"
                            txt_Rect_AmountRecd.Focus()
                            Exit Sub
                        End If
                        AMOUNT_RECD = TrimX(AMOUNT_RECD)
                        'check unwanted characters
                        c = 0
                        counter6 = 0
                        For iloop = 1 To Len(AMOUNT_RECD.ToString)
                            strcurrentchar = Mid(AMOUNT_RECD, iloop, 1)
                            If c = 0 Then
                                If Not InStr("'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz~`!@#$%^&*()_-++<>?/,;<>=()%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter6 = 1
                                End If
                            End If
                        Next
                        If counter6 = 1 Then
                            Lbl_Error.Text = "Error: Input is not Valid !"
                            txt_Rect_AmountRecd.Focus()
                            Exit Sub
                        End If
                    Else
                        Lbl_Error.Text = "Plz Enter the Item Price !"
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Plz Enter Amount Due!');", True)
                        txt_Rect_AmountRecd.Focus()
                        Exit Sub
                    End If
                Else
                    AMOUNT_DUE = Nothing
                    AMOUNT_RECD = Nothing
                End If

                'validation for MEM_PERIOD
                Dim MEM_PERIOD As Object = Nothing
                If txt_Rect_Period.Text <> "" Then
                    MEM_PERIOD = Trim(txt_Rect_Period.Text)
                    MEM_PERIOD = RemoveQuotes(MEM_PERIOD)
                    If MEM_PERIOD.Length > 50 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        txt_Rect_Period.Focus()
                        Exit Sub
                    End If

                    MEM_PERIOD = " " & MEM_PERIOD & " "
                    If InStr(1, MEM_PERIOD, "CREATE", 1) > 0 Or InStr(1, MEM_PERIOD, "DELETE", 1) > 0 Or InStr(1, MEM_PERIOD, "DROP", 1) > 0 Or InStr(1, MEM_PERIOD, "INSERT", 1) > 1 Or InStr(1, MEM_PERIOD, "TRACK", 1) > 1 Or InStr(1, MEM_PERIOD, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Do not use Reserve Word"
                        txt_Rect_Period.Focus()
                        Exit Sub
                    End If
                    MEM_PERIOD = TrimAll(MEM_PERIOD)
                    'check unwanted characters
                    c = 0
                    counter7 = 0
                    For iloop = 1 To Len(MEM_PERIOD.ToString)
                        strcurrentchar = Mid(MEM_PERIOD, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter7 = 1
                            End If
                        End If
                    Next
                    If counter7 = 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        txt_Rect_Period.Focus()
                        Exit Sub
                    End If
                Else
                    MEM_PERIOD = Nothing
                End If

                'validation for PMT_MODE
                Dim PMT_MODE As Object = Nothing
                If DDL_PmtMode.Text <> "" Then
                    PMT_MODE = Trim(DDL_PmtMode.SelectedValue)
                    PMT_MODE = RemoveQuotes(PMT_MODE)
                    If PMT_MODE.Length > 1 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        DDL_PmtMode.Focus()
                        Exit Sub
                    End If

                    PMT_MODE = " " & PMT_MODE & " "
                    If InStr(1, PMT_MODE, "CREATE", 1) > 0 Or InStr(1, PMT_MODE, "DELETE", 1) > 0 Or InStr(1, PMT_MODE, "DROP", 1) > 0 Or InStr(1, PMT_MODE, "INSERT", 1) > 1 Or InStr(1, PMT_MODE, "TRACK", 1) > 1 Or InStr(1, PMT_MODE, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Do not use Reserve Word"
                        DDL_PmtMode.Focus()
                        Exit Sub
                    End If
                    PMT_MODE = TrimAll(PMT_MODE)
                    'check unwanted characters
                    c = 0
                    counter8 = 0
                    For iloop = 1 To Len(PMT_MODE.ToString)
                        strcurrentchar = Mid(PMT_MODE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter8 = 1
                            End If
                        End If
                    Next
                    If counter8 = 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        DDL_PmtMode.Focus()
                        Exit Sub
                    End If
                Else
                    PMT_MODE = "C"
                End If

                'validation for CHEQUE_NO
                Dim CHEQUE_NO As Object = Nothing
                If txt_Rect_ChqNo.Text <> "" Then
                    CHEQUE_NO = TrimAll(txt_Rect_ChqNo.Text)
                    CHEQUE_NO = RemoveQuotes(CHEQUE_NO)
                    If CHEQUE_NO.Length > 50 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        txt_Rect_ChqNo.Focus()
                        Exit Sub
                    End If

                    CHEQUE_NO = " " & CHEQUE_NO & " "
                    If InStr(1, CHEQUE_NO, "CREATE", 1) > 0 Or InStr(1, CHEQUE_NO, "DELETE", 1) > 0 Or InStr(1, CHEQUE_NO, "DROP", 1) > 0 Or InStr(1, CHEQUE_NO, "INSERT", 1) > 1 Or InStr(1, CHEQUE_NO, "TRACK", 1) > 1 Or InStr(1, CHEQUE_NO, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Do not use Reserve Word"
                        txt_Rect_ChqNo.Focus()
                        Exit Sub
                    End If
                    CHEQUE_NO = TrimAll(CHEQUE_NO)
                    'check unwanted characters
                    c = 0
                    counter9 = 0
                    For iloop = 1 To Len(CHEQUE_NO.ToString)
                        strcurrentchar = Mid(CHEQUE_NO, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter9 = 1
                            End If
                        End If
                    Next
                    If counter9 = 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        txt_Rect_ChqNo.Focus()
                        Exit Sub
                    End If
                Else
                    CHEQUE_NO = Nothing
                End If

                'CHEQUE_DATE
                Dim CHEQUE_DATE As Object = Nothing
                If txt_Rect_ChqDate.Text <> "" Then
                    CHEQUE_DATE = TrimX(txt_Rect_ChqDate.Text)
                    If Len(CHEQUE_DATE) <> 10 Then
                        Lbl_Error.Text = " Input is not Valid..."
                        Me.txt_Rect_ChqDate.Focus()
                        Exit Sub
                    End If
                    CHEQUE_DATE = " " & CHEQUE_DATE & " "
                    If InStr(1, CHEQUE_DATE, "CREATE", 1) > 0 Or InStr(1, CHEQUE_DATE, "DELETE", 1) > 0 Or InStr(1, CHEQUE_DATE, "DROP", 1) > 0 Or InStr(1, CHEQUE_DATE, "INSERT", 1) > 1 Or InStr(1, CHEQUE_DATE, "TRACK", 1) > 1 Or InStr(1, CHEQUE_DATE, "TRACE", 1) > 1 Then
                        Label6.Text = "  Input is not Valid... "
                        Me.txt_Rect_ChqDate.Focus()
                        Exit Sub
                    End If
                    CHEQUE_DATE = TrimX(CHEQUE_DATE)
                    'check unwanted characters
                    c = 0
                    counter10 = 0
                    For iloop = 1 To Len(CHEQUE_DATE.ToString)
                        strcurrentchar = Mid(CHEQUE_DATE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz,$#@!^&*_+';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter10 = 1
                            End If
                        End If
                    Next
                    If counter10 = 1 Then
                        Label6.Text = "data is not Valid... "
                        Me.txt_Rect_ChqDate.Focus()
                        Exit Sub
                    End If
                    CHEQUE_DATE = Convert.ToDateTime(CHEQUE_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us
                Else
                    CHEQUE_DATE = Nothing
                End If

                'validation for REMARKS
                Dim REMARKS As Object = Nothing
                If txt_Rect_ChqNo.Text <> "" Then
                    REMARKS = TrimAll(txt_Rect_ChqNo.Text)
                    REMARKS = RemoveQuotes(REMARKS)
                    If REMARKS.Length > 255 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        txt_Rect_ChqNo.Focus()
                        Exit Sub
                    End If

                    REMARKS = " " & REMARKS & " "
                    If InStr(1, REMARKS, "CREATE", 1) > 0 Or InStr(1, REMARKS, "DELETE", 1) > 0 Or InStr(1, REMARKS, "DROP", 1) > 0 Or InStr(1, REMARKS, "INSERT", 1) > 1 Or InStr(1, REMARKS, "TRACK", 1) > 1 Or InStr(1, REMARKS, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Do not use Reserve Word"
                        txt_Rect_ChqNo.Focus()
                        Exit Sub
                    End If
                    REMARKS = TrimAll(REMARKS)
                    'check unwanted characters
                    c = 0
                    counter11 = 0
                    For iloop = 1 To Len(REMARKS.ToString)
                        strcurrentchar = Mid(REMARKS, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter11 = 1
                            End If
                        End If
                    Next
                    If counter11 = 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        txt_Rect_ChqNo.Focus()
                        Exit Sub
                    End If
                Else
                    REMARKS = Nothing
                End If

                Dim STATUS As Object = Nothing
                If TR_AMOUNT_RECD.Visible = True Then
                    If txt_Rect_AmountDue.Text = "" And txt_Rect_AmountRecd.Text <> "" Then
                        STATUS = "Paid"
                    Else
                        If txt_Rect_AmountDue.Text < txt_Rect_AmountRecd.Text Then 'if amount due is less than paid
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Amount Due is less than Amount being paid!');", True)
                            Exit Sub
                        Else
                            If Convert.ToDouble(txt_Rect_AmountDue.Text) = Convert.ToDouble(txt_Rect_AmountRecd.Text) Then
                                STATUS = "Paid"
                            Else
                                STATUS = "Pending"
                            End If
                        End If
                    End If
                Else
                    STATUS = "Paid"
                End If

                Dim HOLD_ID As Integer = Nothing
                If TR_ACCESSION.Visible = True Then
                    If DDL_AccessionNo.Text <> "" Then
                        HOLD_ID = TrimX(DDL_AccessionNo.SelectedValue)
                        HOLD_ID = RemoveQuotes(HOLD_ID)
                        If HOLD_ID.ToString.Length > 10 Then
                            Lbl_Error.Text = "Error: Input is not Valid !"
                            DDL_AccessionNo.Focus()
                            Exit Sub
                        End If
                        HOLD_ID = " " & HOLD_ID & " "
                        If InStr(1, HOLD_ID, "CREATE", 1) > 0 Or InStr(1, HOLD_ID, "DELETE", 1) > 0 Or InStr(1, HOLD_ID, "DROP", 1) > 0 Or InStr(1, HOLD_ID, "INSERT", 1) > 1 Or InStr(1, HOLD_ID, "TRACK", 1) > 1 Or InStr(1, HOLD_ID, "TRACE", 1) > 1 Then
                            Lbl_Error.Text = "Error: Input is not Valid !"
                            DDL_AccessionNo.Focus()
                            Exit Sub
                        End If
                        HOLD_ID = TrimX(HOLD_ID)
                        'check unwanted characters
                        c = 0
                        counter12 = 0
                        For iloop = 1 To Len(HOLD_ID.ToString)
                            strcurrentchar = Mid(HOLD_ID, iloop, 1)
                            If c = 0 Then
                                If Not InStr("'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz~`!@#$%^&*()_-++<>?/,;<>=()%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter12 = 1
                                End If
                            End If
                        Next
                        If counter12 = 1 Then
                            Lbl_Error.Text = "Error: Input is not Valid !"
                            DDL_AccessionNo.Focus()
                            Exit Sub
                        End If
                    Else
                        HOLD_ID = Nothing
                    End If
                Else
                    HOLD_ID = Nothing
                End If

                Dim COPY_ID As Integer = Nothing
                If TR_COPY.Visible = True Then
                    If DDL_CopyID.Text <> "" Then
                        COPY_ID = TrimX(DDL_CopyID.SelectedValue)
                        COPY_ID = RemoveQuotes(COPY_ID)
                        If COPY_ID.ToString.Length > 10 Then
                            Lbl_Error.Text = "Error: Input is not Valid !"
                            DDL_CopyID.Focus()
                            Exit Sub
                        End If
                        COPY_ID = " " & COPY_ID & " "
                        If InStr(1, COPY_ID, "CREATE", 1) > 0 Or InStr(1, COPY_ID, "DELETE", 1) > 0 Or InStr(1, COPY_ID, "DROP", 1) > 0 Or InStr(1, COPY_ID, "INSERT", 1) > 1 Or InStr(1, COPY_ID, "TRACK", 1) > 1 Or InStr(1, COPY_ID, "TRACE", 1) > 1 Then
                            Lbl_Error.Text = "Error: Input is not Valid !"
                            DDL_CopyID.Focus()
                            Exit Sub
                        End If
                        COPY_ID = TrimX(COPY_ID)
                        'check unwanted characters
                        c = 0
                        counter13 = 0
                        For iloop = 1 To Len(COPY_ID.ToString)
                            strcurrentchar = Mid(COPY_ID, iloop, 1)
                            If c = 0 Then
                                If Not InStr("'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz~`!@#$%^&*()_-++<>?/,;<>=()%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter13 = 1
                                End If
                            End If
                        Next
                        If counter13 = 1 Then
                            Lbl_Error.Text = "Error: Input is not Valid !"
                            DDL_CopyID.Focus()
                            Exit Sub
                        End If
                    Else
                        COPY_ID = Nothing
                    End If
                Else
                    COPY_ID = Nothing
                End If

                'check duplicate for same year security deposit
                Dim str As Object = Nothing
                Dim flag As Object = Nothing
                str = "SELECT REC_ID FROM RECEIPTS WHERE (MEM_ID = '" & Trim(MEM_ID) & "') AND (MEM_PERIOD ='" & Trim(MEM_PERIOD) & "') AND (SECURITY_DEPOSIT IS NOT NULL); "
                Dim cmd1 As New SqlCommand(str, SqlConn)
                SqlConn.Open()
                flag = cmd1.ExecuteScalar
                SqlConn.Close()
                If flag <> Nothing Then
                    Lbl_Error.Text = "Security Deposit for This Period/Year already Received ! "
                    txt_Rect_SecurityDeposit.Focus()
                    Exit Sub
                End If

                Dim USER_CODE As Object = Nothing
                USER_CODE = Session.Item("LoggedUser")

                Dim DATE_ADDED As Object = Nothing
                DATE_ADDED = Now.Date
                DATE_ADDED = Convert.ToDateTime(DATE_ADDED, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                Dim LIB_CODE As Object = Nothing
                LIB_CODE = LibCode

                Dim IP As Object = Nothing
                IP = Request.UserHostAddress.Trim

                If SqlConn.State = 0 Then
                    SqlConn.Open()
                End If

                thisTransaction = SqlConn.BeginTransaction()
                Dim intValue As Integer = 0
                Dim objCommand As New SqlCommand
                objCommand.Connection = SqlConn
                objCommand.Transaction = thisTransaction
                objCommand.CommandType = CommandType.Text
                objCommand.CommandText = "INSERT INTO RECEIPTS (MEM_ID, DATE_RECD, RECD_FOR, SECURITY_DEPOSIT, AMOUNT_DUE, AMOUNT_RECD, MEM_PERIOD, PMT_MODE, CHEQUE_NO, CHEQUE_DATE, REMARKS, STATUS, HOLD_ID, COPY_ID, DATE_ADDED, USER_CODE, LIB_CODE, IP) " & _
                                         " VALUES (@MEM_ID, @DATE_RECD, @RECD_FOR, @SECURITY_DEPOSIT, @AMOUNT_DUE, @AMOUNT_RECD, @MEM_PERIOD, @PMT_MODE, @CHEQUE_NO, @CHEQUE_DATE, @REMARKS, @STATUS, @HOLD_ID, @COPY_ID, @DATE_ADDED, @USER_CODE, @LIB_CODE, @IP) " & _
                                         " SELECT SCOPE_IDENTITY(); "

                objCommand.Parameters.Add("@MEM_ID", SqlDbType.Int)
                If MEM_ID <> 0 Then
                    objCommand.Parameters("@MEM_ID").Value = MEM_ID
                Else
                    objCommand.Parameters("@MEM_ID").Value = System.DBNull.Value
                End If

                objCommand.Parameters.Add("@DATE_RECD", SqlDbType.DateTime)
                If DATE_RECD <> "" Then
                    objCommand.Parameters("@DATE_RECD").Value = DATE_RECD
                Else
                    objCommand.Parameters("@DATE_RECD").Value = System.DBNull.Value
                End If

                objCommand.Parameters.Add("@RECD_FOR", SqlDbType.VarChar)
                If RECD_FOR <> "" Then
                    objCommand.Parameters("@RECD_FOR").Value = RECD_FOR
                Else
                    objCommand.Parameters("@RECD_FOR").Value = System.DBNull.Value
                End If

                objCommand.Parameters.Add("@SECURITY_DEPOSIT", SqlDbType.Decimal)
                If SECURITY_DEPOSIT <> "" Then
                    objCommand.Parameters("@SECURITY_DEPOSIT").Value = SECURITY_DEPOSIT
                Else
                    objCommand.Parameters("@SECURITY_DEPOSIT").Value = System.DBNull.Value
                End If

                objCommand.Parameters.Add("@AMOUNT_DUE", SqlDbType.Decimal)
                If AMOUNT_DUE <> "" Then
                    objCommand.Parameters("@AMOUNT_DUE").Value = AMOUNT_DUE
                Else
                    objCommand.Parameters("@AMOUNT_DUE").Value = System.DBNull.Value
                End If

                objCommand.Parameters.Add("@AMOUNT_RECD", SqlDbType.Decimal)
                If AMOUNT_RECD <> "" Then
                    objCommand.Parameters("@AMOUNT_RECD").Value = AMOUNT_RECD
                Else
                    objCommand.Parameters("@AMOUNT_RECD").Value = System.DBNull.Value
                End If

                objCommand.Parameters.Add("@MEM_PERIOD", SqlDbType.VarChar)
                If MEM_PERIOD <> "" Then
                    objCommand.Parameters("@MEM_PERIOD").Value = MEM_PERIOD
                Else
                    objCommand.Parameters("@MEM_PERIOD").Value = System.DBNull.Value
                End If

                objCommand.Parameters.Add("@PMT_MODE", SqlDbType.VarChar)
                If PMT_MODE <> "" Then
                    objCommand.Parameters("@PMT_MODE").Value = PMT_MODE
                Else
                    objCommand.Parameters("@PMT_MODE").Value = System.DBNull.Value
                End If

                objCommand.Parameters.Add("@CHEQUE_NO", SqlDbType.VarChar)
                If CHEQUE_NO <> "" Then
                    objCommand.Parameters("@CHEQUE_NO").Value = CHEQUE_NO
                Else
                    objCommand.Parameters("@CHEQUE_NO").Value = System.DBNull.Value
                End If

                objCommand.Parameters.Add("@CHEQUE_DATE", SqlDbType.DateTime)
                If CHEQUE_DATE <> "" Then
                    objCommand.Parameters("@CHEQUE_DATE").Value = CHEQUE_DATE
                Else
                    objCommand.Parameters("@CHEQUE_DATE").Value = System.DBNull.Value
                End If

                objCommand.Parameters.Add("@REMARKS", SqlDbType.NVarChar)
                If REMARKS <> "" Then
                    objCommand.Parameters("@REMARKS").Value = REMARKS
                Else
                    objCommand.Parameters("@REMARKS").Value = System.DBNull.Value
                End If

                objCommand.Parameters.Add("@STATUS", SqlDbType.NVarChar)
                If STATUS <> "" Then
                    objCommand.Parameters("@STATUS").Value = STATUS
                Else
                    objCommand.Parameters("@STATUS").Value = System.DBNull.Value
                End If

                objCommand.Parameters.Add("@HOLD_ID", SqlDbType.Int)
                If HOLD_ID <> 0 Then
                    objCommand.Parameters("@HOLD_ID").Value = HOLD_ID
                Else
                    objCommand.Parameters("@HOLD_ID").Value = System.DBNull.Value
                End If

                objCommand.Parameters.Add("@COPY_ID", SqlDbType.Int)
                If COPY_ID <> 0 Then
                    objCommand.Parameters("@COPY_ID").Value = COPY_ID
                Else
                    objCommand.Parameters("@COPY_ID").Value = System.DBNull.Value
                End If

                objCommand.Parameters.Add("@DATE_ADDED", SqlDbType.DateTime)
                If DATE_ADDED <> "" Then
                    objCommand.Parameters("@DATE_ADDED").Value = DATE_ADDED
                Else
                    objCommand.Parameters("@DATE_ADDED").Value = System.DBNull.Value
                End If

                objCommand.Parameters.Add("@USER_CODE", SqlDbType.NVarChar)
                If USER_CODE <> "" Then
                    objCommand.Parameters("@USER_CODE").Value = USER_CODE
                Else
                    objCommand.Parameters("@USER_CODE").Value = System.DBNull.Value
                End If

                objCommand.Parameters.Add("@LIB_CODE", SqlDbType.NVarChar)
                If LIB_CODE <> "" Then
                    objCommand.Parameters("@LIB_CODE").Value = LIB_CODE
                Else
                    objCommand.Parameters("@LIB_CODE").Value = System.DBNull.Value
                End If

                objCommand.Parameters.Add("@IP", SqlDbType.NVarChar)
                If IP <> "" Then
                    objCommand.Parameters("@IP").Value = IP
                Else
                    objCommand.Parameters("@IP").Value = System.DBNull.Value
                End If

                Dim dr As SqlDataReader
                dr = objCommand.ExecuteReader()
                If dr.Read Then
                    intValue = dr.GetValue(0)
                End If
                dr.Close()

                thisTransaction.Commit()
                SqlConn.Close()

                ClearFields()
                Rect_Save_Bttn.Visible = True
                Rect_Update_Bttn.Visible = False
                Rect_Delete_Bttn.Visible = False
                PopulateGrid()
                DDL_Members.Focus()
            Catch q As SqlException
                thisTransaction.Rollback()
                Lbl_Error.Text = "Database Error -SAVE: " & (q.Message() & ", " & q.Number & ", " & q.GetType.ToString)
            Catch ex As Exception
                Lbl_Error.Text = "Error-SAVE: " & (ex.Message())
            Finally
                SqlConn.Close()
            End Try
        End If
    End Sub
    Public Sub ClearFields()
        txt_Rect_AmountDue.Text = ""
        txt_Rect_AmountRecd.Text = ""
        txt_Rect_ChqDate.Text = ""
        txt_Rect_ChqNo.Text = ""
        txt_Rect_Date.Text = ""
        txt_Rect_Period.Text = ""
        txt_Rect_Remarks.Text = ""
        txt_Rect_SecurityDeposit.Text = ""
        DDL_AccessionNo.ClearSelection()
        DDL_CopyID.ClearSelection()
        DDL_PmtFor.ClearSelection()
        DDL_PmtMode.ClearSelection()
    End Sub
    'get value of row from grid
    Private Sub Grid1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles Grid1.RowCommand
        Dim Command As SqlCommand = Nothing
        Dim dr As SqlDataReader = Nothing
        Try
            If e.CommandName = "Select" Then
                Dim myRowID, REC_ID As Integer
                myRowID = e.CommandArgument.ToString()
                REC_ID = Grid1.DataKeys(myRowID).Value
                If Not String.IsNullOrEmpty(REC_ID) And REC_ID <> 0 Then
                    Label26.Text = REC_ID
                    Dim iloop As Integer
                    Dim strcurrentchar As Object
                    Dim c As Integer
                    Dim counter1 As Integer

                    REC_ID = TrimX(REC_ID)
                    REC_ID = RemoveQuotes(REC_ID)

                    If Len(REC_ID).ToString > 10 Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Length of Input is not Proper!');", True)
                        Exit Sub
                    End If

                    If Not IsNumeric(REC_ID.ToString) = True Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Input type is not Proper!');", True)
                        Exit Sub
                    End If

                    REC_ID = " " & REC_ID & " "
                    If InStr(1, REC_ID, " CREATE ", 1) > 0 Or InStr(1, REC_ID, " DELETE ", 1) > 0 Or InStr(1, REC_ID, " DROP ", 1) > 0 Or InStr(1, REC_ID, " INSERT ", 1) > 1 Or InStr(1, REC_ID, " TRACK ", 1) > 1 Or InStr(1, REC_ID, " TRACE ", 1) > 1 Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Do Not Use Reserve Words!');", True)
                        Exit Sub
                    End If
                    REC_ID = TrimX(REC_ID)

                    ClearFields()
                    'get record details from database
                    Dim SQL As String = Nothing
                    SQL = " SELECT *  FROM RECEIPTS WHERE (REC_ID = '" & Trim(REC_ID) & "') AND (LIB_CODE = '" & Trim(LibCode) & "') "
                    Command = New SqlCommand(SQL, SqlConn)
                    SqlConn.Open()
                    dr = Command.ExecuteReader(CommandBehavior.CloseConnection)
                    dr.Read()

                    If dr.HasRows = True Then
                        If dr.Item("DATE_RECD").ToString <> "" Then
                            txt_Rect_Date.Text = Format(dr.Item("DATE_RECD"), "dd/MM/yyyy")
                        Else
                            txt_Rect_Date.Text = ""
                        End If

                        If dr.Item("RECD_FOR").ToString <> "" Then
                            DDL_PmtFor.SelectedValue = dr.Item("RECD_FOR").ToString
                        Else
                            DDL_PmtFor.ClearSelection()
                        End If

                        If dr.Item("SECURITY_DEPOSIT").ToString <> "" Then
                            txt_Rect_SecurityDeposit.Text = dr.Item("SECURITY_DEPOSIT").ToString
                            TR_SECURITY_DEPOSIT.Visible = True
                            TR_AMOUNT_RECD.Visible = False
                        Else
                            txt_Rect_SecurityDeposit.Text = ""
                            TR_SECURITY_DEPOSIT.Visible = False
                            TR_AMOUNT_RECD.Visible = True
                        End If

                        If dr.Item("AMOUNT_DUE").ToString <> "" Then
                            txt_Rect_AmountDue.Text = dr.Item("AMOUNT_DUE").ToString
                        Else
                            txt_Rect_AmountDue.Text = ""
                        End If

                        If dr.Item("AMOUNT_RECD").ToString <> "" Then
                            txt_Rect_AmountRecd.Text = dr.Item("AMOUNT_RECD").ToString
                        Else
                            txt_Rect_AmountRecd.Text = ""
                        End If

                        If dr.Item("MEM_PERIOD").ToString <> "" Then
                            txt_Rect_Period.Text = dr.Item("MEM_PERIOD").ToString
                        Else
                            txt_Rect_Period.Text = ""
                        End If

                        If dr.Item("PMT_MODE").ToString <> "" Then
                            DDL_PmtMode.SelectedValue = dr.Item("PMT_MODE").ToString
                        Else
                            DDL_PmtMode.ClearSelection()
                        End If

                        If dr.Item("CHEQUE_NO").ToString <> "" Then
                            txt_Rect_ChqNo.Text = dr.Item("CHEQUE_NO").ToString
                        Else
                            txt_Rect_ChqNo.Text = ""
                        End If

                        If dr.Item("CHEQUE_DATE").ToString <> "" Then
                            txt_Rect_ChqDate.Text = Format(dr.Item("CHEQUE_DATE"), "dd/MM/yyyy")
                        Else
                            txt_Rect_ChqDate.Text = ""
                        End If

                        If dr.Item("HOLD_ID").ToString <> "" Then
                            DDL_AccessionNo.SelectedValue = dr.Item("HOLD_ID").ToString
                        Else
                            DDL_AccessionNo.ClearSelection()
                        End If

                        If dr.Item("COPY_ID").ToString <> "" Then
                            DDL_CopyID.SelectedValue = dr.Item("COPY_ID").ToString
                        Else
                            DDL_CopyID.ClearSelection()
                        End If

                        If dr.Item("REMARKS").ToString <> "" Then
                            txt_Rect_Remarks.Text = dr.Item("REMARKS").ToString
                        Else
                            txt_Rect_Remarks.Text = ""
                        End If

                        If dr.Item("STATUS").ToString <> "" Then
                            Label27.Text = dr.Item("STATUS").ToString
                        Else
                            Label27.Text = ""
                        End If
                        Rect_Save_Bttn.Visible = False
                        Rect_Update_Bttn.Visible = True
                        Rect_Delete_Bttn.Visible = True
                        dr.Close()
                    Else
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Record No Selected for Edit!');", True)
                    End If
                Else
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Record No Selected for Edit!');", True)
                End If
            End If
            txt_Rect_Date.Focus()
        Catch s As Exception
            Lbl_Error.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub 'Grid1_ItemCommand
    'cacel click
    Protected Sub Rect_Cancel_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Rect_Cancel_Bttn.Click
        ClearFields()
        Label26.Text = ""
        Label27.Text = ""
        Rect_Save_Bttn.Visible = True
        Rect_Update_Bttn.Visible = False
        Rect_Delete_Bttn.Visible = False
    End Sub
    'update records
    Protected Sub Rect_Update_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Rect_Update_Bttn.Click
        Dim SQL As String = Nothing
        Dim da As SqlDataAdapter
        Dim cb As SqlCommandBuilder
        Dim ds As New DataSet
        Dim dt As New DataTable
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing
        Try
            If IsPostBack = True Then
                'Server Validation for Lib Code
                Dim iloop As Integer
                Dim strcurrentchar As Object
                Dim c As Integer
                Dim counter1, counter2, counter3, counter4, Counter5, counter6, counter7, counter8, counter9, counter10, counter11, counter12, counter13, counter14 As Integer

                If Label26.Text <> "" Then
                    'validation for REC_ID
                    Dim REC_ID As Integer = Nothing
                    If Me.Label26.Text <> "" Then
                        REC_ID = Trim(Label26.Text)
                        REC_ID = RemoveQuotes(REC_ID)

                        If Not IsNumeric(REC_ID.ToString) Then
                            Lbl_Error.Text = "Error: Input is not Valid !"
                            DDL_Members.Focus()
                            Exit Sub
                        End If

                        If REC_ID.ToString.Length > 5 Then
                            Lbl_Error.Text = "Error: Input is not Valid !"
                            DDL_Members.Focus()
                            Exit Sub
                        End If
                        REC_ID = " " & REC_ID & " "
                        If InStr(1, REC_ID, "CREATE", 1) > 0 Or InStr(1, REC_ID, "DELETE", 1) > 0 Or InStr(1, REC_ID, "DROP", 1) > 0 Or InStr(1, REC_ID, "INSERT", 1) > 1 Or InStr(1, REC_ID, "TRACK", 1) > 1 Or InStr(1, REC_ID, "TRACE", 1) > 1 Then
                            Lbl_Error.Text = "Error: Input is not Valid !"
                            DDL_Members.Focus()
                            Exit Sub
                        End If
                        REC_ID = TrimX(REC_ID)
                        'check unwanted characters
                        c = 0
                        counter14 = 0
                        For iloop = 1 To Len(REC_ID.ToString)
                            strcurrentchar = Mid(REC_ID, iloop, 1)
                            If c = 0 Then
                                If Not InStr("';<>=%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter14 = 1
                                End If
                            End If
                        Next
                        If counter14 = 1 Then
                            Lbl_Error.Text = "Error: Input is not Valid !"
                            DDL_Members.Focus()
                            Exit Sub
                        End If
                    Else
                        Lbl_Error.Text = "Error: Plz Select Title from Drop-Down !"
                        DDL_Members.Focus()
                        Exit Sub
                    End If

                    'MEM_ID
                    Dim MEM_ID As Integer = Nothing
                    If DDL_Members.Text <> "" Then
                        MEM_ID = TrimX(DDL_Members.SelectedValue)
                        If Len(MEM_ID) > 10 Then
                            Lbl_Error.Text = " Input is not Valid..."
                            Me.DDL_Members.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(MEM_ID) = True Then
                            Lbl_Error.Text = " Input is not Valid..."
                            Me.DDL_Members.Focus()
                            Exit Sub
                        End If

                        MEM_ID = " " & MEM_ID & " "
                        If InStr(1, MEM_ID, "CREATE", 1) > 0 Or InStr(1, MEM_ID, "DELETE", 1) > 0 Or InStr(1, MEM_ID, "DROP", 1) > 0 Or InStr(1, MEM_ID, "INSERT", 1) > 1 Or InStr(1, MEM_ID, "TRACK", 1) > 1 Or InStr(1, MEM_ID, "TRACE", 1) > 1 Then
                            Lbl_Error.Text = "  Input is not Valid... "
                            Me.DDL_Members.Focus()
                            Exit Sub
                        End If
                        MEM_ID = TrimX(MEM_ID)
                        'check unwanted characters
                        c = 0
                        counter1 = 0
                        For iloop = 1 To Len(MEM_ID.ToString)
                            strcurrentchar = Mid(MEM_ID, iloop, 1)
                            If c = 0 Then
                                If Not InStr("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz,$#@!^&*_+'.;<>=()%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter1 = 1
                                End If
                            End If
                        Next
                        If counter1 = 1 Then
                            Lbl_Error.Text = "data is not Valid... "
                            Me.DDL_Members.Focus()
                            Exit Sub
                        End If
                    Else
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Plz Select the Member from Drop-Downn!');", True)
                        DDL_Members.Focus()
                        Exit Sub
                    End If

                    'DATE_RECT
                    Dim DATE_RECD As Object = Nothing
                    If txt_Rect_Date.Text <> "" Then
                        DATE_RECD = TrimX(txt_Rect_Date.Text)
                        If Len(DATE_RECD) <> 10 Then
                            Lbl_Error.Text = " Input is not Valid..."
                            Me.txt_Rect_Date.Focus()
                            Exit Sub
                        End If
                        DATE_RECD = " " & DATE_RECD & " "
                        If InStr(1, DATE_RECD, "CREATE", 1) > 0 Or InStr(1, DATE_RECD, "DELETE", 1) > 0 Or InStr(1, DATE_RECD, "DROP", 1) > 0 Or InStr(1, DATE_RECD, "INSERT", 1) > 1 Or InStr(1, DATE_RECD, "TRACK", 1) > 1 Or InStr(1, DATE_RECD, "TRACE", 1) > 1 Then
                            Lbl_Error.Text = "  Input is not Valid... "
                            Me.txt_Rect_Date.Focus()
                            Exit Sub
                        End If
                        DATE_RECD = TrimX(DATE_RECD)
                        'check unwanted characters
                        c = 0
                        counter2 = 0
                        For iloop = 1 To Len(DATE_RECD.ToString)
                            strcurrentchar = Mid(DATE_RECD, iloop, 1)
                            If c = 0 Then
                                If Not InStr("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz,$#@!^&*_+';<>=()%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter2 = 1
                                End If
                            End If
                        Next
                        If counter2 = 1 Then
                            Lbl_Error.Text = "data is not Valid... "
                            Me.txt_Rect_Date.Focus()
                            Exit Sub
                        End If
                        DATE_RECD = Convert.ToDateTime(DATE_RECD, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us
                    Else
                        DATE_RECD = Now.Date
                    End If

                    'validation for RECD_FOR
                    Dim RECD_FOR As Object = Nothing
                    If DDL_PmtFor.Text <> "" Then
                        RECD_FOR = Trim(DDL_PmtFor.SelectedValue)
                        RECD_FOR = RemoveQuotes(RECD_FOR)
                        If RECD_FOR.Length > 1 Then 'maximum length
                            Lbl_Error.Text = " Data must be of Proper Length.. "
                            DDL_PmtFor.Focus()
                            Exit Sub
                        End If

                        RECD_FOR = " " & RECD_FOR & " "
                        If InStr(1, RECD_FOR, "CREATE", 1) > 0 Or InStr(1, RECD_FOR, "DELETE", 1) > 0 Or InStr(1, RECD_FOR, "DROP", 1) > 0 Or InStr(1, RECD_FOR, "INSERT", 1) > 1 Or InStr(1, RECD_FOR, "TRACK", 1) > 1 Or InStr(1, RECD_FOR, "TRACE", 1) > 1 Then
                            Lbl_Error.Text = "Do not use Reserve Word"
                            DDL_PmtFor.Focus()
                            Exit Sub
                        End If
                        RECD_FOR = TrimAll(RECD_FOR)
                        'check unwanted characters
                        c = 0
                        counter3 = 0
                        For iloop = 1 To Len(RECD_FOR.ToString)
                            strcurrentchar = Mid(RECD_FOR, iloop, 1)
                            If c = 0 Then
                                If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter3 = 1
                                End If
                            End If
                        Next
                        If counter3 = 1 Then
                            Lbl_Error.Text = " Input  is not Valid!"
                            DDL_PmtFor.Focus()
                            Exit Sub
                        End If
                    Else
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Plz Select the Value from Drop-Down!');", True)
                        DDL_PmtFor.Focus()
                        Exit Sub
                    End If

                    If RECD_FOR = "S" Then
                        If txt_Rect_SecurityDeposit.Text = "" Then
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Plz Enter Amount for Security Deposit!');", True)
                            txt_Rect_SecurityDeposit.Focus()
                            Exit Sub
                        End If
                    End If

                    'validation for SECURITY_DEPOSIT 
                    Dim SECURITY_DEPOSIT As Object = Nothing
                    If TR_SECURITY_DEPOSIT.Visible = True Then
                        If txt_Rect_SecurityDeposit.Text <> "" Then
                            SECURITY_DEPOSIT = TrimX(txt_Rect_SecurityDeposit.Text)
                            SECURITY_DEPOSIT = RemoveQuotes(SECURITY_DEPOSIT)
                            If SECURITY_DEPOSIT.Length > 10 Then
                                Lbl_Error.Text = "Error: Input is not Valid !"
                                txt_Rect_SecurityDeposit.Focus()
                                Exit Sub
                            End If
                            SECURITY_DEPOSIT = " " & SECURITY_DEPOSIT & " "
                            If InStr(1, SECURITY_DEPOSIT, "CREATE", 1) > 0 Or InStr(1, SECURITY_DEPOSIT, "DELETE", 1) > 0 Or InStr(1, SECURITY_DEPOSIT, "DROP", 1) > 0 Or InStr(1, SECURITY_DEPOSIT, "INSERT", 1) > 1 Or InStr(1, SECURITY_DEPOSIT, "TRACK", 1) > 1 Or InStr(1, SECURITY_DEPOSIT, "TRACE", 1) > 1 Then
                                Lbl_Error.Text = "Error: Input is not Valid !"
                                txt_Rect_SecurityDeposit.Focus()
                                Exit Sub
                            End If
                            SECURITY_DEPOSIT = TrimX(SECURITY_DEPOSIT)
                            'check unwanted characters
                            c = 0
                            counter4 = 0
                            For iloop = 1 To Len(SECURITY_DEPOSIT.ToString)
                                strcurrentchar = Mid(SECURITY_DEPOSIT, iloop, 1)
                                If c = 0 Then
                                    If Not InStr("'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz~`!@#$%^&*()_-++<>?/,;<>=()%""", strcurrentchar) <= 0 Then
                                        c = c + 1
                                        counter4 = 1
                                    End If
                                End If
                            Next
                            If counter4 = 1 Then
                                Lbl_Error.Text = "Error: Input is not Valid !"
                                txt_Rect_SecurityDeposit.Focus()
                                Exit Sub
                            End If
                        Else
                            Lbl_Error.Text = "Plz Enter the Item Price !"
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Plz Enter Amount for Security Deposit!');", True)
                            txt_Rect_SecurityDeposit.Focus()
                            Exit Sub
                        End If
                    End If

                    'validation for AMOUNT_DUE 
                    Dim AMOUNT_DUE As Object = Nothing
                    Dim AMOUNT_RECD As Object = Nothing
                    If TR_AMOUNT_RECD.Visible = True Then
                        If txt_Rect_AmountDue.Text <> "" Then
                            AMOUNT_DUE = TrimX(txt_Rect_AmountDue.Text)
                            AMOUNT_DUE = RemoveQuotes(AMOUNT_DUE)
                            If AMOUNT_DUE.Length > 10 Then
                                Lbl_Error.Text = "Error: Input is not Valid !"
                                txt_Rect_AmountDue.Focus()
                                Exit Sub
                            End If
                            AMOUNT_DUE = " " & AMOUNT_DUE & " "
                            If InStr(1, AMOUNT_DUE, "CREATE", 1) > 0 Or InStr(1, AMOUNT_DUE, "DELETE", 1) > 0 Or InStr(1, AMOUNT_DUE, "DROP", 1) > 0 Or InStr(1, AMOUNT_DUE, "INSERT", 1) > 1 Or InStr(1, AMOUNT_DUE, "TRACK", 1) > 1 Or InStr(1, AMOUNT_DUE, "TRACE", 1) > 1 Then
                                Lbl_Error.Text = "Error: Input is not Valid !"
                                txt_Rect_AmountDue.Focus()
                                Exit Sub
                            End If
                            AMOUNT_DUE = TrimX(AMOUNT_DUE)
                            'check unwanted characters
                            c = 0
                            Counter5 = 0
                            For iloop = 1 To Len(AMOUNT_DUE.ToString)
                                strcurrentchar = Mid(AMOUNT_DUE, iloop, 1)
                                If c = 0 Then
                                    If Not InStr("'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz~`!@#$%^&*()_-++<>?/,;<>=()%""", strcurrentchar) <= 0 Then
                                        c = c + 1
                                        Counter5 = 1
                                    End If
                                End If
                            Next
                            If Counter5 = 1 Then
                                Lbl_Error.Text = "Error: Input is not Valid !"
                                txt_Rect_AmountDue.Focus()
                                Exit Sub
                            End If
                        Else
                            Lbl_Error.Text = "Plz Enter the Item Price !"
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Plz Enter Amount Due!');", True)
                            txt_Rect_AmountDue.Focus()
                            Exit Sub
                        End If

                        'amount Received
                        If txt_Rect_AmountRecd.Text <> "" Then
                            AMOUNT_RECD = TrimX(txt_Rect_AmountRecd.Text)
                            AMOUNT_RECD = RemoveQuotes(AMOUNT_RECD)
                            If AMOUNT_RECD.Length > 10 Then
                                Lbl_Error.Text = "Error: Input is not Valid !"
                                txt_Rect_AmountRecd.Focus()
                                Exit Sub
                            End If
                            AMOUNT_RECD = " " & AMOUNT_RECD & " "
                            If InStr(1, AMOUNT_RECD, "CREATE", 1) > 0 Or InStr(1, AMOUNT_RECD, "DELETE", 1) > 0 Or InStr(1, AMOUNT_RECD, "DROP", 1) > 0 Or InStr(1, AMOUNT_RECD, "INSERT", 1) > 1 Or InStr(1, AMOUNT_RECD, "TRACK", 1) > 1 Or InStr(1, AMOUNT_RECD, "TRACE", 1) > 1 Then
                                Lbl_Error.Text = "Error: Input is not Valid !"
                                txt_Rect_AmountRecd.Focus()
                                Exit Sub
                            End If
                            AMOUNT_RECD = TrimX(AMOUNT_RECD)
                            'check unwanted characters
                            c = 0
                            counter6 = 0
                            For iloop = 1 To Len(AMOUNT_RECD.ToString)
                                strcurrentchar = Mid(AMOUNT_RECD, iloop, 1)
                                If c = 0 Then
                                    If Not InStr("'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz~`!@#$%^&*()_-++<>?/,;<>=()%""", strcurrentchar) <= 0 Then
                                        c = c + 1
                                        counter6 = 1
                                    End If
                                End If
                            Next
                            If counter6 = 1 Then
                                Lbl_Error.Text = "Error: Input is not Valid !"
                                txt_Rect_AmountRecd.Focus()
                                Exit Sub
                            End If
                        Else
                            Lbl_Error.Text = "Plz Enter the Item Price !"
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Plz Enter Amount Due!');", True)
                            txt_Rect_AmountRecd.Focus()
                            Exit Sub
                        End If
                    Else
                        AMOUNT_DUE = Nothing
                        AMOUNT_RECD = Nothing
                    End If

                    'validation for MEM_PERIOD
                    Dim MEM_PERIOD As Object = Nothing
                    If txt_Rect_Period.Text <> "" Then
                        MEM_PERIOD = Trim(txt_Rect_Period.Text)
                        MEM_PERIOD = RemoveQuotes(MEM_PERIOD)
                        If MEM_PERIOD.Length > 50 Then 'maximum length
                            Lbl_Error.Text = " Data must be of Proper Length.. "
                            txt_Rect_Period.Focus()
                            Exit Sub
                        End If

                        MEM_PERIOD = " " & MEM_PERIOD & " "
                        If InStr(1, MEM_PERIOD, "CREATE", 1) > 0 Or InStr(1, MEM_PERIOD, "DELETE", 1) > 0 Or InStr(1, MEM_PERIOD, "DROP", 1) > 0 Or InStr(1, MEM_PERIOD, "INSERT", 1) > 1 Or InStr(1, MEM_PERIOD, "TRACK", 1) > 1 Or InStr(1, MEM_PERIOD, "TRACE", 1) > 1 Then
                            Lbl_Error.Text = "Do not use Reserve Word"
                            txt_Rect_Period.Focus()
                            Exit Sub
                        End If
                        MEM_PERIOD = TrimAll(MEM_PERIOD)
                        'check unwanted characters
                        c = 0
                        counter7 = 0
                        For iloop = 1 To Len(MEM_PERIOD.ToString)
                            strcurrentchar = Mid(MEM_PERIOD, iloop, 1)
                            If c = 0 Then
                                If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter7 = 1
                                End If
                            End If
                        Next
                        If counter7 = 1 Then
                            Lbl_Error.Text = " Input  is not Valid!"
                            txt_Rect_Period.Focus()
                            Exit Sub
                        End If
                    Else
                        MEM_PERIOD = Nothing
                    End If

                    'validation for PMT_MODE
                    Dim PMT_MODE As Object = Nothing
                    If DDL_PmtMode.Text <> "" Then
                        PMT_MODE = Trim(DDL_PmtMode.SelectedValue)
                        PMT_MODE = RemoveQuotes(PMT_MODE)
                        If PMT_MODE.Length > 1 Then 'maximum length
                            Lbl_Error.Text = " Data must be of Proper Length.. "
                            DDL_PmtMode.Focus()
                            Exit Sub
                        End If

                        PMT_MODE = " " & PMT_MODE & " "
                        If InStr(1, PMT_MODE, "CREATE", 1) > 0 Or InStr(1, PMT_MODE, "DELETE", 1) > 0 Or InStr(1, PMT_MODE, "DROP", 1) > 0 Or InStr(1, PMT_MODE, "INSERT", 1) > 1 Or InStr(1, PMT_MODE, "TRACK", 1) > 1 Or InStr(1, PMT_MODE, "TRACE", 1) > 1 Then
                            Lbl_Error.Text = "Do not use Reserve Word"
                            DDL_PmtMode.Focus()
                            Exit Sub
                        End If
                        PMT_MODE = TrimAll(PMT_MODE)
                        'check unwanted characters
                        c = 0
                        counter8 = 0
                        For iloop = 1 To Len(PMT_MODE.ToString)
                            strcurrentchar = Mid(PMT_MODE, iloop, 1)
                            If c = 0 Then
                                If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter8 = 1
                                End If
                            End If
                        Next
                        If counter8 = 1 Then
                            Lbl_Error.Text = " Input  is not Valid!"
                            DDL_PmtMode.Focus()
                            Exit Sub
                        End If
                    Else
                        PMT_MODE = "C"
                    End If

                    'validation for CHEQUE_NO
                    Dim CHEQUE_NO As Object = Nothing
                    If txt_Rect_ChqNo.Text <> "" Then
                        CHEQUE_NO = TrimAll(txt_Rect_ChqNo.Text)
                        CHEQUE_NO = RemoveQuotes(CHEQUE_NO)
                        If CHEQUE_NO.Length > 50 Then 'maximum length
                            Lbl_Error.Text = " Data must be of Proper Length.. "
                            txt_Rect_ChqNo.Focus()
                            Exit Sub
                        End If

                        CHEQUE_NO = " " & CHEQUE_NO & " "
                        If InStr(1, CHEQUE_NO, "CREATE", 1) > 0 Or InStr(1, CHEQUE_NO, "DELETE", 1) > 0 Or InStr(1, CHEQUE_NO, "DROP", 1) > 0 Or InStr(1, CHEQUE_NO, "INSERT", 1) > 1 Or InStr(1, CHEQUE_NO, "TRACK", 1) > 1 Or InStr(1, CHEQUE_NO, "TRACE", 1) > 1 Then
                            Lbl_Error.Text = "Do not use Reserve Word"
                            txt_Rect_ChqNo.Focus()
                            Exit Sub
                        End If
                        CHEQUE_NO = TrimAll(CHEQUE_NO)
                        'check unwanted characters
                        c = 0
                        counter9 = 0
                        For iloop = 1 To Len(CHEQUE_NO.ToString)
                            strcurrentchar = Mid(CHEQUE_NO, iloop, 1)
                            If c = 0 Then
                                If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter9 = 1
                                End If
                            End If
                        Next
                        If counter9 = 1 Then
                            Lbl_Error.Text = " Input  is not Valid!"
                            txt_Rect_ChqNo.Focus()
                            Exit Sub
                        End If
                    Else
                        CHEQUE_NO = Nothing
                    End If

                    'CHEQUE_DATE
                    Dim CHEQUE_DATE As Object = Nothing
                    If txt_Rect_ChqDate.Text <> "" Then
                        CHEQUE_DATE = TrimX(txt_Rect_ChqDate.Text)
                        If Len(CHEQUE_DATE) <> 10 Then
                            Lbl_Error.Text = " Input is not Valid..."
                            Me.txt_Rect_ChqDate.Focus()
                            Exit Sub
                        End If
                        CHEQUE_DATE = " " & CHEQUE_DATE & " "
                        If InStr(1, CHEQUE_DATE, "CREATE", 1) > 0 Or InStr(1, CHEQUE_DATE, "DELETE", 1) > 0 Or InStr(1, CHEQUE_DATE, "DROP", 1) > 0 Or InStr(1, CHEQUE_DATE, "INSERT", 1) > 1 Or InStr(1, CHEQUE_DATE, "TRACK", 1) > 1 Or InStr(1, CHEQUE_DATE, "TRACE", 1) > 1 Then
                            Label6.Text = "  Input is not Valid... "
                            Me.txt_Rect_ChqDate.Focus()
                            Exit Sub
                        End If
                        CHEQUE_DATE = TrimX(CHEQUE_DATE)
                        'check unwanted characters
                        c = 0
                        counter10 = 0
                        For iloop = 1 To Len(CHEQUE_DATE.ToString)
                            strcurrentchar = Mid(CHEQUE_DATE, iloop, 1)
                            If c = 0 Then
                                If Not InStr("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz,$#@!^&*_+';<>=()%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter10 = 1
                                End If
                            End If
                        Next
                        If counter10 = 1 Then
                            Label6.Text = "data is not Valid... "
                            Me.txt_Rect_ChqDate.Focus()
                            Exit Sub
                        End If
                        CHEQUE_DATE = Convert.ToDateTime(CHEQUE_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us
                    Else
                        CHEQUE_DATE = Nothing
                    End If

                    'validation for REMARKS
                    Dim REMARKS As Object = Nothing
                    If txt_Rect_ChqNo.Text <> "" Then
                        REMARKS = TrimAll(txt_Rect_ChqNo.Text)
                        REMARKS = RemoveQuotes(REMARKS)
                        If REMARKS.Length > 255 Then 'maximum length
                            Lbl_Error.Text = " Data must be of Proper Length.. "
                            txt_Rect_ChqNo.Focus()
                            Exit Sub
                        End If

                        REMARKS = " " & REMARKS & " "
                        If InStr(1, REMARKS, "CREATE", 1) > 0 Or InStr(1, REMARKS, "DELETE", 1) > 0 Or InStr(1, REMARKS, "DROP", 1) > 0 Or InStr(1, REMARKS, "INSERT", 1) > 1 Or InStr(1, REMARKS, "TRACK", 1) > 1 Or InStr(1, REMARKS, "TRACE", 1) > 1 Then
                            Lbl_Error.Text = "Do not use Reserve Word"
                            txt_Rect_ChqNo.Focus()
                            Exit Sub
                        End If
                        REMARKS = TrimAll(REMARKS)
                        'check unwanted characters
                        c = 0
                        counter11 = 0
                        For iloop = 1 To Len(REMARKS.ToString)
                            strcurrentchar = Mid(REMARKS, iloop, 1)
                            If c = 0 Then
                                If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter11 = 1
                                End If
                            End If
                        Next
                        If counter11 = 1 Then
                            Lbl_Error.Text = " Input  is not Valid!"
                            txt_Rect_ChqNo.Focus()
                            Exit Sub
                        End If
                    Else
                        REMARKS = Nothing
                    End If

                    Dim STATUS As Object = Nothing
                    If TR_AMOUNT_RECD.Visible = True Then
                        If txt_Rect_AmountDue.Text = "" And txt_Rect_AmountRecd.Text <> "" Then
                            STATUS = "Paid"
                        Else
                            If txt_Rect_AmountDue.Text < txt_Rect_AmountRecd.Text Then 'if amount due is less than paid
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Amount Due is less than Amount being paid!');", True)
                                Exit Sub
                            Else
                                If Convert.ToDouble(txt_Rect_AmountDue.Text) = Convert.ToDouble(txt_Rect_AmountRecd.Text) Then
                                    STATUS = "Paid"
                                Else
                                    STATUS = "Pending"
                                End If
                            End If
                        End If
                    Else
                        STATUS = "Paid"
                    End If

                    Dim HOLD_ID As Integer = Nothing
                    If TR_ACCESSION.Visible = True Then
                        If DDL_AccessionNo.Text <> "" Then
                            HOLD_ID = TrimX(DDL_AccessionNo.SelectedValue)
                            HOLD_ID = RemoveQuotes(HOLD_ID)
                            If HOLD_ID.ToString.Length > 10 Then
                                Lbl_Error.Text = "Error: Input is not Valid !"
                                DDL_AccessionNo.Focus()
                                Exit Sub
                            End If
                            HOLD_ID = " " & HOLD_ID & " "
                            If InStr(1, HOLD_ID, "CREATE", 1) > 0 Or InStr(1, HOLD_ID, "DELETE", 1) > 0 Or InStr(1, HOLD_ID, "DROP", 1) > 0 Or InStr(1, HOLD_ID, "INSERT", 1) > 1 Or InStr(1, HOLD_ID, "TRACK", 1) > 1 Or InStr(1, HOLD_ID, "TRACE", 1) > 1 Then
                                Lbl_Error.Text = "Error: Input is not Valid !"
                                DDL_AccessionNo.Focus()
                                Exit Sub
                            End If
                            HOLD_ID = TrimX(HOLD_ID)
                            'check unwanted characters
                            c = 0
                            counter12 = 0
                            For iloop = 1 To Len(HOLD_ID.ToString)
                                strcurrentchar = Mid(HOLD_ID, iloop, 1)
                                If c = 0 Then
                                    If Not InStr("'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz~`!@#$%^&*()_-++<>?/,;<>=()%""", strcurrentchar) <= 0 Then
                                        c = c + 1
                                        counter12 = 1
                                    End If
                                End If
                            Next
                            If counter12 = 1 Then
                                Lbl_Error.Text = "Error: Input is not Valid !"
                                DDL_AccessionNo.Focus()
                                Exit Sub
                            End If
                        Else
                            HOLD_ID = Nothing
                        End If
                    Else
                        HOLD_ID = Nothing
                    End If

                    Dim COPY_ID As Integer = Nothing
                    If TR_COPY.Visible = True Then
                        If DDL_CopyID.Text <> "" Then
                            COPY_ID = TrimX(DDL_CopyID.SelectedValue)
                            COPY_ID = RemoveQuotes(COPY_ID)
                            If COPY_ID.ToString.Length > 10 Then
                                Lbl_Error.Text = "Error: Input is not Valid !"
                                DDL_CopyID.Focus()
                                Exit Sub
                            End If
                            COPY_ID = " " & COPY_ID & " "
                            If InStr(1, COPY_ID, "CREATE", 1) > 0 Or InStr(1, COPY_ID, "DELETE", 1) > 0 Or InStr(1, COPY_ID, "DROP", 1) > 0 Or InStr(1, COPY_ID, "INSERT", 1) > 1 Or InStr(1, COPY_ID, "TRACK", 1) > 1 Or InStr(1, COPY_ID, "TRACE", 1) > 1 Then
                                Lbl_Error.Text = "Error: Input is not Valid !"
                                DDL_CopyID.Focus()
                                Exit Sub
                            End If
                            COPY_ID = TrimX(COPY_ID)
                            'check unwanted characters
                            c = 0
                            counter13 = 0
                            For iloop = 1 To Len(COPY_ID.ToString)
                                strcurrentchar = Mid(COPY_ID, iloop, 1)
                                If c = 0 Then
                                    If Not InStr("'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz~`!@#$%^&*()_-++<>?/,;<>=()%""", strcurrentchar) <= 0 Then
                                        c = c + 1
                                        counter13 = 1
                                    End If
                                End If
                            Next
                            If counter13 = 1 Then
                                Lbl_Error.Text = "Error: Input is not Valid !"
                                DDL_CopyID.Focus()
                                Exit Sub
                            End If
                        Else
                            COPY_ID = Nothing
                        End If
                    Else
                        COPY_ID = Nothing
                    End If

                    Dim USER_CODE As Object = Nothing
                    USER_CODE = Session.Item("LoggedUser")

                    Dim DATE_MODIFIED As Object = Nothing
                    DATE_MODIFIED = Now.Date
                    DATE_MODIFIED = Convert.ToDateTime(DATE_MODIFIED, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                    Dim LIB_CODE As Object = Nothing
                    LIB_CODE = LibCode

                    Dim IP As Object = Nothing
                    IP = Request.UserHostAddress.Trim

                    SQL = "SELECT * FROM RECEIPTS WHERE (REC_ID='" & Trim(Label26.Text) & "')"
                    SqlConn.Open()
                    da = New SqlDataAdapter(SQL, SqlConn)
                    cb = New SqlCommandBuilder(da)
                    da.Fill(ds, "RECEIPTS")
                    If ds.Tables("RECEIPTS").Rows.Count <> 0 Then

                        If TR_ACCESSION.Visible = True Then
                            If HOLD_ID <> 0 Then
                                ds.Tables("RECEIPTS").Rows(0)("HOLD_ID") = HOLD_ID
                            Else
                                ds.Tables("RECEIPTS").Rows(0)("HOLD_ID") = System.DBNull.Value
                            End If
                        End If

                        If TR_COPY.Visible = True Then
                            If COPY_ID <> 0 Then
                                ds.Tables("RECEIPTS").Rows(0)("COPY_ID") = COPY_ID
                            Else
                                ds.Tables("RECEIPTS").Rows(0)("COPY_ID") = System.DBNull.Value
                            End If
                        End If

                        If TR_SECURITY_DEPOSIT.Visible = True Then
                            If SECURITY_DEPOSIT <> "" Then
                                ds.Tables("RECEIPTS").Rows(0)("SECURITY_DEPOSIT") = SECURITY_DEPOSIT
                            Else
                                ds.Tables("RECEIPTS").Rows(0)("SECURITY_DEPOSIT") = System.DBNull.Value
                            End If
                        End If

                        If TR_AMOUNT_RECD.Visible = True Then
                            If AMOUNT_DUE <> "" Then
                                ds.Tables("RECEIPTS").Rows(0)("AMOUNT_DUE") = AMOUNT_DUE
                            Else
                                ds.Tables("RECEIPTS").Rows(0)("AMOUNT_DUE") = System.DBNull.Value
                            End If

                            If AMOUNT_RECD <> "" Then
                                ds.Tables("RECEIPTS").Rows(0)("AMOUNT_RECD") = AMOUNT_RECD
                            Else
                                ds.Tables("RECEIPTS").Rows(0)("AMOUNT_RECD") = System.DBNull.Value
                            End If
                        End If

                        If DATE_RECD <> "" Then
                            ds.Tables("RECEIPTS").Rows(0)("DATE_RECD") = DATE_RECD
                        Else
                            ds.Tables("RECEIPTS").Rows(0)("DATE_RECD") = System.DBNull.Value
                        End If

                        If MEM_PERIOD <> "" Then
                            ds.Tables("RECEIPTS").Rows(0)("MEM_PERIOD") = MEM_PERIOD
                        Else
                            ds.Tables("RECEIPTS").Rows(0)("MEM_PERIOD") = System.DBNull.Value
                        End If

                        If RECD_FOR <> "" Then
                            ds.Tables("RECEIPTS").Rows(0)("RECD_FOR") = RECD_FOR
                        Else
                            ds.Tables("RECEIPTS").Rows(0)("RECD_FOR") = System.DBNull.Value
                        End If

                        If REMARKS <> "" Then
                            ds.Tables("RECEIPTS").Rows(0)("REMARKS") = REMARKS
                        Else
                            ds.Tables("RECEIPTS").Rows(0)("REMARKS") = System.DBNull.Value
                        End If

                        If PMT_MODE <> "" Then
                            ds.Tables("RECEIPTS").Rows(0)("PMT_MODE") = PMT_MODE
                        Else
                            ds.Tables("RECEIPTS").Rows(0)("PMT_MODE") = System.DBNull.Value
                        End If

                        If CHEQUE_NO <> "" Then
                            ds.Tables("RECEIPTS").Rows(0)("CHEQUE_NO") = CHEQUE_NO
                        Else
                            ds.Tables("RECEIPTS").Rows(0)("CHEQUE_NO") = System.DBNull.Value
                        End If

                        If CHEQUE_DATE <> "" Then
                            ds.Tables("RECEIPTS").Rows(0)("CHEQUE_DATE") = CHEQUE_DATE
                        Else
                            ds.Tables("RECEIPTS").Rows(0)("CHEQUE_DATE") = System.DBNull.Value
                        End If

                        If STATUS <> "" Then
                            ds.Tables("RECEIPTS").Rows(0)("STATUS") = STATUS
                        Else
                            ds.Tables("RECEIPTS").Rows(0)("STATUS") = System.DBNull.Value
                        End If

                        ds.Tables("RECEIPTS").Rows(0)("UPDATED_BY") = UserCode.Trim
                        ds.Tables("RECEIPTS").Rows(0)("DATE_MODIFIED") = Now.Date
                        ds.Tables("RECEIPTS").Rows(0)("IP") = Request.UserHostAddress.Trim

                        thisTransaction = SqlConn.BeginTransaction()
                        da.SelectCommand.Transaction = thisTransaction
                        da.Update(ds, "RECEIPTS")
                        thisTransaction.Commit()

                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Record updated Sucessfulyy!');", True)
                        ClearFields()
                        Label26.Text = ""
                        Label27.Text = ""
                    Else
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Error in Record Updation!');", True)
                        Exit Sub
                    End If
                End If
                Else
                    'record not selected
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Record Not Selected for Edit!');", True)
                    Exit Sub
                End If
                SqlConn.Close()
                PopulateGrid()
                ClearFields()
                Label26.Text = ""
                Label27.Text = ""
                Rect_Save_Bttn.Visible = True
                Rect_Update_Bttn.Visible = False
                Rect_Delete_Bttn.Visible = False
        Catch q As SqlException
            thisTransaction.Rollback()
        Catch s As Exception
            Lbl_Error.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'delete record
    Protected Sub Rect_Delete_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Rect_Delete_Bttn.Click
        Try
            If IsPostBack = True Then
                'Server Validation for Lib Code
                    Dim iloop As Integer
                    Dim strcurrentchar As Object
                    Dim c As Integer
                    Dim counter1, counter2, counter3, counter4, Counter5, counter6, counter7, counter8, counter9, counter10, counter11, counter12, counter13, counter14 As Integer

                    If Label26.Text <> "" Then
                        'validation for REC_ID
                        Dim REC_ID As Integer = Nothing
                        If Me.Label26.Text <> "" Then
                            REC_ID = Trim(Label26.Text)
                            REC_ID = RemoveQuotes(REC_ID)

                            If Not IsNumeric(REC_ID.ToString) Then
                                Lbl_Error.Text = "Error: Input is not Valid !"
                                DDL_Members.Focus()
                                Exit Sub
                            End If

                            If REC_ID.ToString.Length > 5 Then
                                Lbl_Error.Text = "Error: Input is not Valid !"
                                DDL_Members.Focus()
                                Exit Sub
                            End If
                            REC_ID = " " & REC_ID & " "
                            If InStr(1, REC_ID, "CREATE", 1) > 0 Or InStr(1, REC_ID, "DELETE", 1) > 0 Or InStr(1, REC_ID, "DROP", 1) > 0 Or InStr(1, REC_ID, "INSERT", 1) > 1 Or InStr(1, REC_ID, "TRACK", 1) > 1 Or InStr(1, REC_ID, "TRACE", 1) > 1 Then
                                Lbl_Error.Text = "Error: Input is not Valid !"
                                DDL_Members.Focus()
                                Exit Sub
                            End If
                            REC_ID = TrimX(REC_ID)
                            'check unwanted characters
                            c = 0
                            counter14 = 0
                            For iloop = 1 To Len(REC_ID.ToString)
                                strcurrentchar = Mid(REC_ID, iloop, 1)
                                If c = 0 Then
                                    If Not InStr("';<>=%""", strcurrentchar) <= 0 Then
                                        c = c + 1
                                        counter14 = 1
                                    End If
                                End If
                            Next
                            If counter14 = 1 Then
                                Lbl_Error.Text = "Error: Input is not Valid !"
                                DDL_Members.Focus()
                                Exit Sub
                            End If
                        Else
                            Lbl_Error.Text = "Error: Plz Select Title from Drop-Down !"
                            DDL_Members.Focus()
                            Exit Sub
                        End If

                    'delete Record
                    Dim SQL As String = Nothing
                    SQL = "DELETE FROM RECEIPTS WHERE (REC_ID ='" & Trim(REC_ID) & "') "
                    Dim objCommand As New SqlCommand(SQL, SqlConn)
                    Dim da As New SqlDataAdapter(objCommand)
                    Dim ds As New DataSet
                    da.Fill(ds)
                    ClearFields()
                    Label26.Text = ""
                    Label27.Text = ""
                    Me.Rect_Save_Bttn.Visible = True
                    Me.Rect_Update_Bttn.Visible = False
                    Me.Rect_Delete_Bttn.Visible = False
                End If
            End If
            PopulateGrid()
        Catch q As SqlException
            Lbl_Error.Text = "Database Error -SAVE: " & (q.Message() & ", " & q.Number & ", " & q.GetType.ToString)
        Catch s As Exception
            Lbl_Error.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'delete selected records
    Protected Sub Rect_DeleteAll_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Rect_DeleteAll_Bttn.Click
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing
        Dim dt As DataTable = Nothing
        Try
            For Each row As GridViewRow In Grid1.Rows
                Dim cb As CheckBox = DirectCast(row.FindControl("cbd"), CheckBox)
                If cb IsNot Nothing AndAlso cb.Checked = True Then
                    Dim REC_ID As Integer = Convert.ToInt32(Grid1.DataKeys(row.RowIndex).Value)

                    Dim counter1 As Integer = Nothing
                    Dim iloop As Integer
                    Dim strcurrentchar As Object
                    Dim c As Integer

                    'validation for HOLD_ID
                    REC_ID = RemoveQuotes(REC_ID)

                    If Not IsNumeric(REC_ID.ToString) Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        DDL_Members.Focus()
                        Exit Sub
                    End If

                    If REC_ID.ToString.Length > 5 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        DDL_Members.Focus()
                        Exit Sub
                    End If
                    REC_ID = " " & REC_ID & " "
                    If InStr(1, REC_ID, "CREATE", 1) > 0 Or InStr(1, REC_ID, "DELETE", 1) > 0 Or InStr(1, REC_ID, "DROP", 1) > 0 Or InStr(1, REC_ID, "INSERT", 1) > 1 Or InStr(1, REC_ID, "TRACK", 1) > 1 Or InStr(1, REC_ID, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        DDL_Members.Focus()
                        Exit Sub
                    End If
                    REC_ID = TrimX(REC_ID)
                        'check unwanted characters
                    c = 0
                    counter1 = 0
                    For iloop = 1 To Len(REC_ID.ToString)
                        strcurrentchar = Mid(REC_ID, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter1 = 1
                            End If
                        End If
                    Next
                    If counter1 = 1 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        DDL_Members.Focus()
                        Exit Sub
                    End If
               
                    If SqlConn.State = 0 Then
                        SqlConn.Open()
                    End If

                    thisTransaction = SqlConn.BeginTransaction()
                    Dim objCommand As New SqlCommand
                    objCommand.Connection = SqlConn
                    objCommand.Transaction = thisTransaction
                    objCommand.CommandType = CommandType.Text
                    objCommand.CommandText = "DELETE FROM  RECEIPTS WHERE (REC_ID = @REC_ID); "

                    objCommand.Parameters.Add("@REC_ID", SqlDbType.Int)
                    objCommand.Parameters("@REC_ID").Value = REC_ID

                    objCommand.ExecuteNonQuery()

                    thisTransaction.Commit()
                    SqlConn.Close()
                End If
            Next
            PopulateGrid()
        Catch q As SqlException
            thisTransaction.Rollback()
            Lbl_Error.Text = "Database Error -UPDATE: " & (q.Message() & ", " & q.Number & ", " & q.GetType.ToString)
        Catch ex As Exception
            Lbl_Error.Text = "Error-UPDATE: " & (ex.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    Public Sub PopulateAccNo()
        Dim dtSearch As DataTable = Nothing
        Try
            Dim SQL As String = Nothing
            SQL = "SELECT HOLD_ID, ACCESSION_NO FROM HOLDINGS WHERE (LIB_CODE = '" & Trim(LibCode) & "') ORDER BY CASE WHEN LEFT(Accession_NO,1) LIKE  '[A-Za-z]'  THEN dbo.Sort_AlphaNumeric(Accession_No, 'A-Za-z') END ASC, CAST(dbo.Sort_AlphaNumeric(Accession_no, '0-9') AS float) ASC ; "
            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            da.Fill(ds)

            dtSearch = ds.Tables(0).Copy

            If dtSearch.Rows.Count = 0 Then
                Me.DDL_AccessionNo.DataSource = Nothing
                DDL_AccessionNo.Items.Clear()

                Me.DDL_AccessionNo2.DataSource = Nothing
                DDL_AccessionNo2.Items.Clear()
            Else
                Me.DDL_AccessionNo.DataSource = dtSearch
                Me.DDL_AccessionNo.DataTextField = "ACCESSION_NO"
                Me.DDL_AccessionNo.DataValueField = "HOLD_ID"
                Me.DDL_AccessionNo.DataBind()
                DDL_AccessionNo.Items.Insert(0, "")

                Me.DDL_AccessionNo2.DataSource = dtSearch
                Me.DDL_AccessionNo2.DataTextField = "ACCESSION_NO"
                Me.DDL_AccessionNo2.DataValueField = "HOLD_ID"
                Me.DDL_AccessionNo2.DataBind()
                DDL_AccessionNo2.Items.Insert(0, "")
            End If
            dtSearch.Dispose()
            SqlConn.Close()
        Catch s As Exception
            Lbl_Error.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    Public Sub PopulateCopyId()
        Dim dtSearch As DataTable = Nothing
        Try
            Dim SQL As String = Nothing
            SQL = "SELECT COPY_ID FROM LOOSE_ISSUES_COPIES WHERE (LIB_CODE = '" & Trim(LibCode) & "') ORDER BY COPY_ID ASC ; "
            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            da.Fill(ds)

            dtSearch = ds.Tables(0).Copy

            If dtSearch.Rows.Count = 0 Then
                Me.DDL_CopyID.DataSource = Nothing
                DDL_CopyID.Items.Clear()

                Me.DDL_CopyID2.DataSource = Nothing
                DDL_CopyID2.Items.Clear()
            Else
                Me.DDL_CopyID.DataSource = dtSearch
                Me.DDL_CopyID.DataTextField = "COPY_ID"
                Me.DDL_CopyID.DataValueField = "COPY_ID"
                Me.DDL_CopyID.DataBind()
                DDL_CopyID.Items.Insert(0, "")

                Me.DDL_CopyID2.DataSource = dtSearch
                Me.DDL_CopyID2.DataTextField = "COPY_ID"
                Me.DDL_CopyID2.DataValueField = "COPY_ID"
                Me.DDL_CopyID2.DataBind()
                DDL_CopyID2.Items.Insert(0, "")
            End If
            dtSearch.Dispose()
            SqlConn.Close()
        Catch s As Exception
            Lbl_Error.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    Public Sub PopulateReceiptId()
        Dim dtSearch As DataTable = Nothing
        Try
            Dim SQL As String = Nothing
            SQL = "SELECT REC_ID FROM RECEIPTS WHERE (LIB_CODE = '" & Trim(LibCode) & "') ORDER BY REC_ID DESC ; "
            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            da.Fill(ds)

            dtSearch = ds.Tables(0).Copy

            If dtSearch.Rows.Count = 0 Then
                Me.DDL_Receipts.DataSource = Nothing
                DDL_Receipts.Items.Clear()
            Else
                Me.DDL_Receipts.DataSource = dtSearch
                Me.DDL_Receipts.DataTextField = "REC_ID"
                Me.DDL_Receipts.DataValueField = "REC_ID"
                Me.DDL_Receipts.DataBind()
                DDL_Receipts.Items.Insert(0, "")
            End If
            dtSearch.Dispose()
            SqlConn.Close()
        Catch s As Exception
            Lbl_Error.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'display title details
    Protected Sub DDL_AccessionNo_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDL_AccessionNo.SelectedIndexChanged
        Dim dt As DataTable
        Try
            Dim HOLD_ID As Integer = Nothing
            If DDL_AccessionNo.SelectedValue <> 0 Then
                HOLD_ID = DDL_AccessionNo.SelectedValue

                Dim SQL As String = Nothing
                'SQL = "SELECT HOLDINGS_CATS_AUTHORS_VIEW.*, CATS.PHOTO  FROM HOLDINGS_CATS_AUTHORS_VIEW INNER JOIN CATS ON HOLDINGS_CATS_AUTHORS_VIEW.CAT_NO = CATS.CAT_NO WHERE (HOLDINGS_CATS_AUTHORS_VIEW.LIB_CODE ='" & Trim(LibCode) & "'  AND HOLDINGS_CATS_AUTHORS_VIEW.HOLD_ID ='" & Trim(HOLD_ID) & "')"
                SQL = "SELECT HOLDINGS.HOLD_ID, HOLDINGS.ACCESSION_NO, CATS.TITLE, CATS.SUB_TITLE, CATS.AUTHOR1, CATS.AUTHOR2, CATS.AUTHOR3 FROM HOLDINGS LEFT OUTER JOIN CATS ON HOLDINGS.CAT_NO = CATS.CAT_NO WHERE (HOLDINGS.LIB_CODE ='" & Trim(LibCode) & "'  AND HOLDINGS.HOLD_ID ='" & Trim(HOLD_ID) & "');"
                Dim ds As New DataSet
                Dim da As New SqlDataAdapter(SQL, SqlConn)
                If SqlConn.State = 0 Then
                    SqlConn.Open()
                End If
                da.Fill(ds)

                dt = ds.Tables(0).Copy

                Dim myDetails As String = Nothing
                If dt.Rows.Count <> 0 Then
                    If dt.Rows(0).Item("SUB_TITLE").ToString <> "" Then
                        myDetails = dt.Rows(0).Item("TITLE").ToString & ": " & dt.Rows(0).Item("SUB_TITLE").ToString
                    Else
                        myDetails = dt.Rows(0).Item("TITLE").ToString
                    End If

                    If dt.Rows(0).Item("AUTHOR1").ToString <> "" And dt.Rows(0).Item("AUTHOR2").ToString <> "" And dt.Rows(0).Item("AUTHOR3").ToString <> "" Then
                        myDetails = myDetails & " /By " & dt.Rows(0).Item("AUTHOR1").ToString & ";  " & dt.Rows(0).Item("AUTHOR2").ToString & " and " & dt.Rows(0).Item("AUTHOR3").ToString
                    End If
                    If dt.Rows(0).Item("AUTHOR1").ToString <> "" And dt.Rows(0).Item("AUTHOR2").ToString <> "" And dt.Rows(0).Item("AUTHOR3").ToString = "" Then
                        myDetails = myDetails & " /By " & dt.Rows(0).Item("AUTHOR1").ToString & " and " & dt.Rows(0).Item("AUTHOR2").ToString
                    End If
                    If dt.Rows(0).Item("AUTHOR1").ToString <> "" And dt.Rows(0).Item("AUTHOR2").ToString = "" And dt.Rows(0).Item("AUTHOR3").ToString = "" Then
                        myDetails = myDetails & " /By " & dt.Rows(0).Item("AUTHOR1").ToString
                    End If

                    If myDetails <> "" Then
                        Label2.Text = myDetails
                    Else
                        Label2.Text = ""
                    End If
                Else
                    Label2.Text = ""
                End If
            Else
                Label2.Text = ""
            End If
        Catch s As Exception
            Lbl_Error.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'display loose issue details
    Protected Sub DDL_CopyID_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDL_CopyID.SelectedIndexChanged
        Dim dt As DataTable
        Try
            Dim COPY_ID As Integer = Nothing
            If DDL_CopyID.SelectedValue <> 0 Then
                COPY_ID = DDL_CopyID.SelectedValue
                Dim SQL As String = Nothing
                SQL = "SELECT * FROM CATS_LOOSE_ISSUES_COPIES_VIEW WHERE (LOOSE_ISSUE_LIB_CODE ='" & Trim(LibCode) & "'  AND COPY_ID ='" & Trim(COPY_ID) & "')"

                Dim ds As New DataSet
                Dim da As New SqlDataAdapter(SQL, SqlConn)
                If SqlConn.State = 0 Then
                    SqlConn.Open()
                End If
                da.Fill(ds)

                dt = ds.Tables(0).Copy

                Dim myDetails As String = Nothing
                If dt.Rows.Count <> 0 Then
                    If dt.Rows(0).Item("SUB_TITLE").ToString <> "" Then
                        myDetails = dt.Rows(0).Item("TITLE").ToString & ": " & dt.Rows(0).Item("SUB_TITLE").ToString
                    Else
                        myDetails = dt.Rows(0).Item("TITLE").ToString
                    End If

                    If dt.Rows(0).Item("EDITOR").ToString <> "" Then
                        myDetails = myDetails & " /Edited By " & dt.Rows(0).Item("EDITOR").ToString
                    End If

                    If dt.Rows(0).Item("PUB_PLACE").ToString <> "" Then
                        myDetails = myDetails & ", " & dt.Rows(0).Item("PUB_PLACE").ToString
                    End If
                    If dt.Rows(0).Item("PUB_NAME").ToString <> "" Then
                        myDetails = myDetails & ", " & dt.Rows(0).Item("PUB_NAME").ToString
                    End If

                    If dt.Rows(0).Item("VOL_NO").ToString <> "" Then
                        myDetails = myDetails & ". Vol No.: " & dt.Rows(0).Item("VOL_NO").ToString
                    End If
                    If dt.Rows(0).Item("ISSUE_NO").ToString <> "" Then
                        myDetails = myDetails & " (" & dt.Rows(0).Item("ISSUE_NO").ToString & ")"
                    End If
                    If dt.Rows(0).Item("PART_NO").ToString <> "" Then
                        myDetails = myDetails & ", Part NO:" & dt.Rows(0).Item("PART_NO").ToString
                    End If

                    If dt.Rows(0).Item("ISS_DATE").ToString <> "" Then
                        myDetails = myDetails & ", Issue Date:" & Format(dt.Rows(0).Item("ISS_DATE"), "dd/MM/yyyy")
                    End If

                    If myDetails <> "" Then
                        Label5.Text = myDetails
                    Else
                        Label5.Text = ""
                    End If
                Else
                    Label5.Text = ""
                End If
            Else
                Label5.Text = ""
            End If
        Catch s As Exception
            Lbl_Error.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'clear other drop-downs
    Protected Sub DDL_Receipts_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDL_Receipts.SelectedIndexChanged
        'DDL_Receipts.ClearSelection
        DDL_Members2.ClearSelection()
        DDL_AccessionNo2.ClearSelection()
        DDL_CopyID2.ClearSelection()
        DDL_PmtFor2.ClearSelection()
        DDL_Status.ClearSelection()
    End Sub
    Protected Sub DDL_Members2_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDL_Members2.SelectedIndexChanged
        DDL_Receipts.ClearSelection()
        'DDL_Members2.ClearSelection()
        DDL_AccessionNo2.ClearSelection()
        DDL_CopyID2.ClearSelection()
        'DDL_PmtFor2.ClearSelection()
        DDL_Status.ClearSelection()
    End Sub
    Protected Sub DDL_AccessionNo2_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDL_AccessionNo2.SelectedIndexChanged
        DDL_Receipts.ClearSelection()
        DDL_Members2.ClearSelection()
        'DDL_AccessionNo2.ClearSelection()
        DDL_CopyID2.ClearSelection()
        DDL_PmtFor2.ClearSelection()
        DDL_Status.ClearSelection()
    End Sub
    Protected Sub DDL_CopyID2_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDL_CopyID2.SelectedIndexChanged
        DDL_Receipts.ClearSelection()
        DDL_Members2.ClearSelection()
        DDL_AccessionNo2.ClearSelection()
        ' DDL_CopyID2.ClearSelection()
        DDL_PmtFor2.ClearSelection()
        DDL_Status.ClearSelection()
    End Sub
    Protected Sub DDL_PmtFor2_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDL_PmtFor2.SelectedIndexChanged
        DDL_Receipts.ClearSelection()
        ' DDL_Members2.ClearSelection()
        DDL_AccessionNo2.ClearSelection()
        DDL_CopyID2.ClearSelection()
        'DDL_PmtFor2.ClearSelection()
        DDL_Status.ClearSelection()
    End Sub
    Protected Sub DDL_Status_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDL_Status.SelectedIndexChanged
        DDL_Receipts.ClearSelection()
        DDL_Members2.ClearSelection()
        DDL_AccessionNo2.ClearSelection()
        DDL_CopyID2.ClearSelection()
        DDL_PmtFor2.ClearSelection()
        'DDL_Status.ClearSelection()
    End Sub
    'delete selected records
    Protected Sub Delete_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Delete_Bttn.Click
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing
        Dim dt As DataTable = Nothing
        Try
            For Each row As GridViewRow In Grid1_Search.Rows
                Dim cb As CheckBox = DirectCast(row.FindControl("cbd"), CheckBox)
                If cb IsNot Nothing AndAlso cb.Checked = True Then
                    Dim REC_ID As Integer = Convert.ToInt32(Grid1_Search.DataKeys(row.RowIndex).Value)

                    Dim counter1 As Integer = Nothing
                    Dim iloop As Integer
                    Dim strcurrentchar As Object
                    Dim c As Integer

                    'validation for HOLD_ID
                    REC_ID = RemoveQuotes(REC_ID)

                    If Not IsNumeric(REC_ID.ToString) Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        DDL_Members.Focus()
                        Exit Sub
                    End If

                    If REC_ID.ToString.Length > 5 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        DDL_Members.Focus()
                        Exit Sub
                    End If
                    REC_ID = " " & REC_ID & " "
                    If InStr(1, REC_ID, "CREATE", 1) > 0 Or InStr(1, REC_ID, "DELETE", 1) > 0 Or InStr(1, REC_ID, "DROP", 1) > 0 Or InStr(1, REC_ID, "INSERT", 1) > 1 Or InStr(1, REC_ID, "TRACK", 1) > 1 Or InStr(1, REC_ID, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        DDL_Members.Focus()
                        Exit Sub
                    End If
                    REC_ID = TrimX(REC_ID)
                    'check unwanted characters
                    c = 0
                    counter1 = 0
                    For iloop = 1 To Len(REC_ID.ToString)
                        strcurrentchar = Mid(REC_ID, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter1 = 1
                            End If
                        End If
                    Next
                    If counter1 = 1 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        DDL_Members.Focus()
                        Exit Sub
                    End If

                    If SqlConn.State = 0 Then
                        SqlConn.Open()
                    End If

                    thisTransaction = SqlConn.BeginTransaction()
                    Dim objCommand As New SqlCommand
                    objCommand.Connection = SqlConn
                    objCommand.Transaction = thisTransaction
                    objCommand.CommandType = CommandType.Text
                    objCommand.CommandText = "DELETE FROM  RECEIPTS WHERE (REC_ID = @REC_ID); "

                    objCommand.Parameters.Add("@REC_ID", SqlDbType.Int)
                    objCommand.Parameters("@REC_ID").Value = REC_ID

                    objCommand.ExecuteNonQuery()

                    thisTransaction.Commit()
                    SqlConn.Close()
                End If
            Next
            Search_Bttn_Click(sender, e)
        Catch q As SqlException
            thisTransaction.Rollback()
            Lbl_Error.Text = "Database Error -UPDATE: " & (q.Message() & ", " & q.Number & ", " & q.GetType.ToString)
        Catch ex As Exception
            Lbl_Error.Text = "Error-UPDATE: " & (ex.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    Protected Sub Print_Summary_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Print_Summary_Bttn.Click
        Try

            Dim Reportdoc As New ReportDocument
            Reportdoc = Report_Load_Summary()

            If DDL_PrintFormats.SelectedValue <> "" Then
                If DDL_PrintFormats.SelectedValue = "PDF" Then
                    Reportdoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "Report_Receipt_Summary_" + Format(DateTime.Now, "ddMMyyyy"))
                End If
                If DDL_PrintFormats.SelectedValue = "DOC" Then
                    Reportdoc.ExportToHttpResponse(ExportFormatType.WordForWindows, Response, True, "Report_Receipt_Summary_" + Format(DateTime.Now, "ddMMyyyy"))
                End If
                If DDL_PrintFormats.SelectedValue = "HTML" Then

                End If
                If DDL_PrintFormats.SelectedValue = "EXCEL" Then
                    ExportTo_Excel()
                End If
            Else
                Reportdoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "Report_Receipt_Summary_" + Format(DateTime.Now, "ddMMyyyy"))
            End If
        Catch ex As Exception
            Lbl_Error.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    Public Function Report_Load_Summary() As ReportDocument
        Dim Rpt As New ReportDocument
        Dim dtLibrary As New DataTable
        dtLibrary = (ViewState("dt"))

        Rpt.Load(Server.MapPath("~/Reports/Payment_Summary_Report.rpt"))
        Rpt.SetDataSource(dtLibrary)
        Rpt.Refresh()

        Dim myGrpName As Object = Nothing
        If DDL_GroupBy.Text <> "" Then
            myGrpName = Trim(DDL_GroupBy.SelectedValue)
        Else
            myGrpName = Nothing
        End If

        Dim myGroupName As CrystalReports.Engine.TextObject
        If myGrpName <> "" Then
            myGroupName = Rpt.ReportDefinition.Sections("PageHeaderSection1").ReportObjects.Item("Text4")
            myGroupName.Text = "Group By: " & DDL_GroupBy.SelectedItem().Text
        Else
            Rpt.ReportDefinition.Sections("PageHeaderSection1").SectionFormat.EnableSuppress = True
            Rpt.ReportDefinition.Sections("GroupFooterSection1").SectionFormat.EnableSuppress = True
        End If

        'Group By the Report
        Dim grpline As FieldDefinition
        Dim FieldDef As FieldDefinition
        If myGrpName <> "" Then
            grpline = Rpt.Database.Tables(0).Fields.Item(myGrpName)
            Rpt.DataDefinition.Groups.Item(0).ConditionField = grpline
            FieldDef = Rpt.Database.Tables.Item(0).Fields.Item(myGrpName) '("CLASS_NO")
            Rpt.DataDefinition.SortFields.Item(0).Field = FieldDef
        Else
            Rpt.ReportDefinition.Sections("GroupHeaderSection1").SectionFormat.EnableSuppress = True
        End If

        Rpt.SummaryInfo.ReportAuthor = RKLibraryParent
        Rpt.SummaryInfo.ReportComments = RKLibraryAddress
        Rpt.SummaryInfo.ReportTitle = RKLibraryName

        Response.Buffer = False
        Response.ClearContent()
        Response.ClearHeaders()
        dtLibrary.Dispose()
        Return Rpt
    End Function
    Public Sub ExportTo_Excel()

        Dim strFilename As String = "Export_SV_" + Format(DateTime.Now, "ddMMyyyy") + ".xls" '"ExportToExcel.xls"
        Dim attachment As String = "attachment; filename=" & strFilename
        Response.ClearContent()
        Response.AddHeader("content-disposition", attachment)
        Response.ContentType = "application/vnd.ms-excel"

        Dim Flag As Object = Nothing
        Dim dtExcel As DataTable = Nothing
        For Each row As GridViewRow In Grid1_Search.Rows
            Dim CIR_ID As Integer = Convert.ToInt32(Grid1_Search.DataKeys(row.RowIndex).Value)
            Dim SQL As String = Nothing
            SQL = "SELECT RECEIPTS.SECURITY_DEPOSIT, RECEIPTS.AMOUNT_DUE, RECEIPTS.AMOUNT_RECD, RECEIPTS.DATE_RECD, RECEIPTS.RECD_FOR, RECEIPTS.REMARKS, RECEIPTS.MEM_PERIOD, RECEIPTS.PMT_MODE, RECEIPTS.CHEQUE_NO, RECEIPTS.CHEQUE_DATE, RECEIPTS.STATUS, MEMBERSHIPS.MEM_NO, MEMBERSHIPS.MEM_NAME, HOLDINGS_CATS_AUTHORS_VIEW.STANDARD_NO,  TITLE = CASE (isnull(CATS_LOOSE_ISSUES_COPIES_VIEW.Title,'')) WHen ''  THEN HOLDINGS_CATS_AUTHORS_VIEW.TITLE ELSE CATS_LOOSE_ISSUES_COPIES_VIEW.title END, ACCESSION = CASE (isnull(HOLDINGS_CATS_AUTHORS_VIEW.accession_no,'')) WHen ''  THEN cast(CATS_LOOSE_ISSUES_COPIES_VIEW.copy_id as varchar(20)) ELSE HOLDINGS_CATS_AUTHORS_VIEW.accession_no END, HOLDINGS_CATS_AUTHORS_VIEW.SUB_TITLE, HOLDINGS_CATS_AUTHORS_VIEW.AUTHOR1, HOLDINGS_CATS_AUTHORS_VIEW.AUTHOR2, HOLDINGS_CATS_AUTHORS_VIEW.AUTHOR3, HOLDINGS_CATS_AUTHORS_VIEW.YEAR_OF_PUB, HOLDINGS_CATS_AUTHORS_VIEW.EDITION, HOLDINGS_CATS_AUTHORS_VIEW.PLACE_OF_PUB, HOLDINGS_CATS_AUTHORS_VIEW.PUB_NAME, CATS_LOOSE_ISSUES_COPIES_VIEW.VOL_NO, CATS_LOOSE_ISSUES_COPIES_VIEW.ISSUE_NO FROM  RECEIPTS LEFT OUTER JOIN CATS_LOOSE_ISSUES_COPIES_VIEW ON RECEIPTS.COPY_ID = CATS_LOOSE_ISSUES_COPIES_VIEW.COPY_ID LEFT OUTER JOIN HOLDINGS_CATS_AUTHORS_VIEW ON RECEIPTS.HOLD_ID = HOLDINGS_CATS_AUTHORS_VIEW.HOLD_ID LEFT OUTER JOIN MEMBERSHIPS ON RECEIPTS.MEM_ID = MEMBERSHIPS.MEM_ID WHERE (RECEIPTS.LIB_CODE='" & Trim(LibCode) & "') AND (RECEIPTS.REC_ID='" & Trim(CIR_ID) & "');"


            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            da.Fill(ds)
            dtExcel = ds.Tables(0).Copy

            Dim tab As String = String.Empty
            If Flag = "" Then
                For Each dtcol As DataColumn In dtExcel.Columns
                    Response.Write(tab + dtcol.ColumnName)
                    tab = vbTab
                Next
                Flag = "Yes"
            End If

            Response.Write(vbLf)

            For Each dr As DataRow In dtExcel.Rows
                tab = ""
                For j As Integer = 0 To dtExcel.Columns.Count - 1
                    Response.Write(tab & Convert.ToString(dr(j)))
                    tab = vbTab
                Next
            Next

        Next
        Response.Flush()
        Response.End()
    End Sub
    Protected Sub Print_Details_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Print_Details_Bttn.Click
        Try

            Dim Reportdoc As New ReportDocument
            Reportdoc = Report_Load_Details()

            If DDL_PrintFormats.SelectedValue <> "" Then
                If DDL_PrintFormats.SelectedValue = "PDF" Then
                    Reportdoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "Report_Receipt_Detail_" + Format(DateTime.Now, "ddMMyyyy"))
                End If
                If DDL_PrintFormats.SelectedValue = "DOC" Then
                    Reportdoc.ExportToHttpResponse(ExportFormatType.WordForWindows, Response, True, "Report_Receipt_Detail_" + Format(DateTime.Now, "ddMMyyyy"))
                End If
                If DDL_PrintFormats.SelectedValue = "HTML" Then

                End If
                If DDL_PrintFormats.SelectedValue = "EXCEL" Then
                    ExportTo_Excel()
                End If
            Else
                Reportdoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "Report_Receipt_Detail_" + Format(DateTime.Now, "ddMMyyyy"))
            End If
        Catch ex As Exception
            Lbl_Error.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    Public Function Report_Load_Details() As ReportDocument
        Dim Rpt As New ReportDocument
        Dim dtLibrary As New DataTable
        dtLibrary = (ViewState("dt"))

        Rpt.Load(Server.MapPath("~/Reports/Payment_Detail_Report.rpt"))
        Rpt.SetDataSource(dtLibrary)
        Rpt.Refresh()

        Dim myGrpName As Object = Nothing
        If DDL_GroupBy.Text <> "" Then
            myGrpName = Trim(DDL_GroupBy.SelectedValue)
        Else
            myGrpName = Nothing
        End If

        Dim myGroupName As CrystalReports.Engine.TextObject
        If myGrpName <> "" Then
            myGroupName = Rpt.ReportDefinition.Sections("PageHeaderSection1").ReportObjects.Item("Text4")
            myGroupName.Text = "Group By: " & DDL_GroupBy.SelectedItem().Text
        Else
            Rpt.ReportDefinition.Sections("PageHeaderSection1").SectionFormat.EnableSuppress = True
            Rpt.ReportDefinition.Sections("GroupFooterSection1").SectionFormat.EnableSuppress = True
        End If

        'Group By the Report
        Dim grpline As FieldDefinition
        Dim FieldDef As FieldDefinition
        If myGrpName <> "" Then
            grpline = Rpt.Database.Tables(0).Fields.Item(myGrpName)
            Rpt.DataDefinition.Groups.Item(0).ConditionField = grpline
            FieldDef = Rpt.Database.Tables.Item(0).Fields.Item(myGrpName) '("CLASS_NO")
            Rpt.DataDefinition.SortFields.Item(0).Field = FieldDef
        Else
            Rpt.ReportDefinition.Sections("GroupHeaderSection1").SectionFormat.EnableSuppress = True
        End If

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