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
Imports EG4.PopulateCombo
Public Class Transactions
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
                        RadioButton1.Checked = True
                        RadioButton2.Checked = False
                        PopulateLetters()
                        PopulateMemberNo()
                        DDL_MemberNo.Items.Insert(0, "")
                        PopulateMemberName()
                        DDL_MemberName.Items.Insert(0, "")
                        PopulateCategories()
                        DDL_MemberCategories.Items.Insert(0, "")
                        PopulateSubCategories()
                        DDL_MemberSubCategories.Items.Insert(0, "")
                        DDL_Status.Items.Insert(0, "")
                        PopulateLibraryStaff()
                        DDL_LibraryStaff.Items.Insert(0, "")
                        DDL_CollectionType.Enabled = True
                        DDL_CollectionType.Items.Insert(0, "")
                        PopulateAccNo()
                        DDL_AccessionNo.Items.Insert(0, "")
                    End If
                End If
                Dim CreateSUserButton As Object 'System.Web.UI.WebControls.Button
                CreateSUserButton = Master.FindControl("Accordion1").FindControl("CirPane").FindControl("Cir_Transactions_Bttn")
                CreateSUserButton.ForeColor = Drawing.Color.Red
                myPaneName = "CirPane" 'paneSelectedIndex = 0
            Else
                Response.Redirect("~/Default.aspx", False)
            End If
            LibCode = Session.Item("LoggedLibcode")
        Catch ex As Exception
            Lbl_Error.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    'populate letter templates
    Public Sub PopulateLetters()
        Me.DDL_Letters.DataTextField = "FORM_NAME"
        Me.DDL_Letters.DataValueField = "MESS_ID"
        Me.DDL_Letters.DataSource = GetLetters(LibCode)
        Me.DDL_Letters.DataBind()
        DDL_Letters.Items.Insert(0, "")
    End Sub
    Public Sub PopulateMemberNo()
        Dim dtSearch As DataTable = Nothing
        Try
            Dim SQL As String = Nothing
            SQL = "SELECT MEM_ID, MEM_NO, MEM_NAME FROM MEMBERSHIPS WHERE (LIB_CODE = '" & Trim(LibCode) & "') ORDER BY MEM_NO"
            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            da.Fill(ds)

            dtSearch = ds.Tables(0).Copy

            If dtSearch.Rows.Count = 0 Then
                Me.DDL_MemberNo.DataSource = Nothing
                DDL_MemberNo.Items.Clear()
            Else
                Me.DDL_MemberNo.DataSource = dtSearch
                Me.DDL_MemberNo.DataTextField = "MEM_NO"
                Me.DDL_MemberNo.DataValueField = "MEM_ID"
                Me.DDL_MemberNo.DataBind()
            End If
            dtSearch.Dispose()
            SqlConn.Close()
        Catch s As Exception
            Lbl_Error.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    Public Sub PopulateMemberName()
        Dim dtSearch As DataTable = Nothing
        Try
            Dim SQL As String = Nothing
            SQL = "SELECT MEM_ID, MEM_NO, MEM_NAME FROM MEMBERSHIPS WHERE (LIB_CODE = '" & Trim(LibCode) & "') ORDER BY MEM_NO"
            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            da.Fill(ds)

            dtSearch = ds.Tables(0).Copy

            If dtSearch.Rows.Count = 0 Then
                Me.DDL_MemberName.DataSource = Nothing
                DDL_MemberName.Items.Clear()
            Else
                Me.DDL_MemberName.DataSource = dtSearch
                Me.DDL_MemberName.DataTextField = "MEM_NAME"
                Me.DDL_MemberName.DataValueField = "MEM_ID"
                Me.DDL_MemberName.DataBind()
            End If
            dtSearch.Dispose()
            SqlConn.Close()
        Catch s As Exception
            Lbl_Error.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    Public Sub PopulateCategories()
        Dim Command As SqlCommand = Nothing
        Dim dt As DataTable = Nothing
        Try
            Command = New SqlCommand("SELECT CAT_ID, CAT_NAME FROM CATEGORIES WHERE (LIB_CODE = '" & Trim(LibCode) & "') ORDER BY CAT_NAME ", SqlConn)
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            Dim da As New SqlDataAdapter(Command)
            Dim ds As New DataSet
            Dim RecordCount As Long = 0
            da.Fill(ds)

            dt = ds.Tables(0).Copy

            If dt.Rows.Count = 0 Then
                Me.DDL_MemberCategories.DataSource = Nothing
                DDL_MemberCategories.Items.Clear()
            Else
                Me.DDL_MemberCategories.DataSource = dt
                Me.DDL_MemberCategories.DataTextField = "CAT_NAME"
                Me.DDL_MemberCategories.DataValueField = "CAT_ID"
                Me.DDL_MemberCategories.DataBind()
            End If
            dt.Dispose()
            SqlConn.Close()
        Catch s As Exception
            Lbl_Error.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    Public Sub PopulateSubCategories()
        Dim Command As SqlCommand = Nothing
        Dim dt As DataTable = Nothing
        Try
            Command = New SqlCommand("SELECT SUBCAT_ID, SUBCAT_NAME FROM SUB_CATEGORIES WHERE (LIB_CODE = '" & Trim(LibCode) & "') ORDER BY SUBCAT_NAME ", SqlConn)
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            Dim da As New SqlDataAdapter(Command)
            Dim ds As New DataSet
            Dim RecordCount As Long = 0
            da.Fill(ds)

            dt = ds.Tables(0).Copy

            If dt.Rows.Count = 0 Then
                Me.DDL_MemberSubCategories.DataSource = Nothing
                DDL_MemberSubCategories.Items.Clear()
            Else
                Me.DDL_MemberSubCategories.DataSource = dt
                Me.DDL_MemberSubCategories.DataTextField = "SUBCAT_NAME"
                Me.DDL_MemberSubCategories.DataValueField = "SUBCAT_ID"
                Me.DDL_MemberSubCategories.DataBind()
            End If
            dt.Dispose()
            SqlConn.Close()
        Catch s As Exception
            Lbl_Error.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    Public Sub PopulateLibraryStaff()
        Dim dtSearch As DataTable = Nothing
        Try
            Dim SQL As String = Nothing
            SQL = "SELECT USER_CODE, USER_NAME FROM USERS WHERE (LIB_CODE = '" & Trim(LibCode) & "') AND (USER_TYPE ='USER' OR USER_TYPE = 'SUSER') ORDER BY USER_NAME"
            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            da.Fill(ds)

            dtSearch = ds.Tables(0).Copy

            Dim Dr As DataRow
            Dr = dtSearch.NewRow
            Dr("USER_CODE") = ""
            Dr("USER_NAME") = ""
            dtSearch.Rows.InsertAt(Dr, 0)

            If dtSearch.Rows.Count = 0 Then
                Me.DDL_LibraryStaff.DataSource = Nothing
                DDL_LibraryStaff.Items.Clear()
            Else
                Me.DDL_LibraryStaff.DataSource = dtSearch
                Me.DDL_LibraryStaff.DataTextField = "USER_NAME"
                Me.DDL_LibraryStaff.DataValueField = "USER_CODE"
                Me.DDL_LibraryStaff.DataBind()
            End If
            dtSearch.Dispose()
            SqlConn.Close()
        Catch s As Exception
            Lbl_Error.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    Public Sub PopulateAccNo()
        Dim dtSearch As DataTable = Nothing
        Try
            Dim SQL As String = Nothing
            SQL = "SELECT HOLD_ID, ACCESSION_NO FROM HOLDINGS WHERE HOLD_ID in (SELECT HOLD_ID FROM CIRCULATION WHERE LIB_CODE = '" & Trim(LibCode) & "')"
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
            Else
                Me.DDL_AccessionNo.DataSource = dtSearch
                Me.DDL_AccessionNo.DataTextField = "ACCESSION_NO"
                Me.DDL_AccessionNo.DataValueField = "HOLD_ID"
                Me.DDL_AccessionNo.DataBind()
                DDL_AccessionNo.Items.Insert(0, "")
            End If
            dtSearch.Dispose()
            SqlConn.Close()
        Catch s As Exception
            Lbl_Error.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    Public Sub PopulateItemID()
        Dim dtSearch As DataTable = Nothing
        Try
            Dim SQL As String = Nothing
            SQL = "SELECT COPY_ID FROM LOOSE_ISSUES_COPIES WHERE COPY_ID in (SELECT COPY_ID FROM CIRCULATION WHERE LIB_CODE = '" & Trim(LibCode) & "' AND COPY_ID IS NOT Null)"
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
            Else
                Me.DDL_AccessionNo.DataSource = dtSearch
                Me.DDL_AccessionNo.DataTextField = "COPY_ID"
                Me.DDL_AccessionNo.DataValueField = "COPY_ID"
                Me.DDL_AccessionNo.DataBind()
                DDL_AccessionNo.Items.Insert(0, "")
            End If
            dtSearch.Dispose()
            SqlConn.Close()
        Catch s As Exception
            Lbl_Error.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    Protected Sub RadioButton1_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles RadioButton1.CheckedChanged
        Label12.Text = "Accession No"
        PopulateAccNo()
    End Sub
    Protected Sub RadioButton2_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles RadioButton2.CheckedChanged
        Label12.Text = "Item ID"
        PopulateItemID()
        Grid1.DataSource = Nothing
        Grid1.DataBind()
    End Sub
    'search grid
    Protected Sub Tra_Search_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Tra_Search_Bttn.Click
        Dim dtSearch As DataTable = Nothing
        Try
            If IsPostBack = True Then
                Dim iloop As Integer
                Dim strcurrentchar As Object
                Dim c As Integer
                Dim counter1, counter2, counter3, counter4, counter5, counter6, counter7, counter8, counter9, counter10, counter11, counter12, counter13, counter14, counter15, counter16, counter17, counter18, counter19 As Integer

                Dim MEM_ID As Integer = Nothing
                If DDL_MemberNo.Text <> "" Then
                    MEM_ID = Trim(DDL_MemberNo.SelectedValue)
                    
                    If Len(MEM_ID) > 10 Then
                        Lbl_Error.Text = "Error: Member No is not Proper!"
                        DDL_MemberNo.Focus()
                        Exit Sub
                    End If
                    MEM_ID = " " & MEM_ID & " "
                    If InStr(1, MEM_ID, "CREATE", 1) > 0 Or InStr(1, MEM_ID, "DELETE", 1) > 0 Or InStr(1, MEM_ID, "DROP", 1) > 0 Or InStr(1, MEM_ID, "INSERT", 1) > 1 Or InStr(1, MEM_ID, "TRACK", 1) > 1 Or InStr(1, MEM_ID, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Error: Member No Input is not Valid !"
                        DDL_MemberNo.Focus()
                        Exit Sub
                    End If
                    MEM_ID = TrimX(MEM_ID)
                    'check unwanted characters
                    c = 0
                    counter1 = 0
                    For iloop = 1 To Len(MEM_ID.ToString)
                        strcurrentchar = Mid(MEM_ID, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter1 = 1
                            End If
                        End If
                    Next
                    If counter1 = 1 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        DDL_MemberNo.Focus()
                        Exit Sub
                    End If
                Else
                    MEM_ID = Nothing
                End If

                Dim MEM_ID2 As Integer = Nothing
                If DDL_MemberName.Text <> "" Then
                    MEM_ID2 = Trim(DDL_MemberName.SelectedValue)

                    If Len(MEM_ID2) > 50 Then
                        Lbl_Error.Text = "Error: Member No is not Proper!"
                        DDL_MemberName.Focus()
                        Exit Sub
                    End If
                    MEM_ID2 = " " & MEM_ID2 & " "
                    If InStr(1, MEM_ID2, "CREATE", 1) > 0 Or InStr(1, MEM_ID2, "DELETE", 1) > 0 Or InStr(1, MEM_ID2, "DROP", 1) > 0 Or InStr(1, MEM_ID2, "INSERT", 1) > 1 Or InStr(1, MEM_ID2, "TRACK", 1) > 1 Or InStr(1, MEM_ID2, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Error: Member No Input is not Valid !"
                        DDL_MemberName.Focus()
                        Exit Sub
                    End If
                    MEM_ID2 = TrimX(MEM_ID2)
                    'check unwanted characters
                    c = 0
                    counter2 = 0
                    For iloop = 1 To Len(MEM_ID2.ToString)
                        strcurrentchar = Mid(MEM_ID2, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter2 = 1
                            End If
                        End If
                    Next
                    If counter2 = 1 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        DDL_MemberName.Focus()
                        Exit Sub
                    End If
                Else
                    MEM_ID2 = Nothing
                End If

                Dim CAT_ID As Integer = Nothing
                If DDL_MemberCategories.Text <> "" Then
                    CAT_ID = Trim(DDL_MemberCategories.SelectedValue)
                    If IsNumeric(CAT_ID) = False Then
                        Lbl_Error.Text = "Error: Member ID is not Proper!"
                        DDL_MemberCategories.Focus()
                        Exit Sub
                    End If
                    If Len(CAT_ID) > 10 Then
                        Lbl_Error.Text = "Error: Member No is not Proper!"
                        DDL_MemberCategories.Focus()
                        Exit Sub
                    End If
                    CAT_ID = " " & CAT_ID & " "
                    If InStr(1, CAT_ID, "CREATE", 1) > 0 Or InStr(1, CAT_ID, "DELETE", 1) > 0 Or InStr(1, CAT_ID, "DROP", 1) > 0 Or InStr(1, CAT_ID, "INSERT", 1) > 1 Or InStr(1, CAT_ID, "TRACK", 1) > 1 Or InStr(1, CAT_ID, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Error: Member No Input is not Valid !"
                        DDL_MemberCategories.Focus()
                        Exit Sub
                    End If
                    CAT_ID = TrimX(CAT_ID)
                    'check unwanted characters
                    c = 0
                    counter3 = 0
                    For iloop = 1 To Len(CAT_ID.ToString)
                        strcurrentchar = Mid(CAT_ID, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter3 = 1
                            End If
                        End If
                    Next
                    If counter3 = 1 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        DDL_MemberCategories.Focus()
                        Exit Sub
                    End If
                Else
                    CAT_ID = Nothing
                End If

                Dim SUBCAT_ID As Integer = Nothing
                If DDL_MemberSubCategories.Text <> "" Then
                    SUBCAT_ID = Trim(DDL_MemberSubCategories.SelectedValue)
                    If IsNumeric(SUBCAT_ID) = False Then
                        Lbl_Error.Text = "Error: Member ID is not Proper!"
                        DDL_MemberSubCategories.Focus()
                        Exit Sub
                    End If
                    If Len(SUBCAT_ID) > 10 Then
                        Lbl_Error.Text = "Error: Member No is not Proper!"
                        DDL_MemberSubCategories.Focus()
                        Exit Sub
                    End If
                    SUBCAT_ID = " " & SUBCAT_ID & " "
                    If InStr(1, SUBCAT_ID, "CREATE", 1) > 0 Or InStr(1, SUBCAT_ID, "DELETE", 1) > 0 Or InStr(1, SUBCAT_ID, "DROP", 1) > 0 Or InStr(1, SUBCAT_ID, "INSERT", 1) > 1 Or InStr(1, SUBCAT_ID, "TRACK", 1) > 1 Or InStr(1, SUBCAT_ID, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Error: Member No Input is not Valid !"
                        DDL_MemberSubCategories.Focus()
                        Exit Sub
                    End If
                    SUBCAT_ID = TrimX(SUBCAT_ID)
                    'check unwanted characters
                    c = 0
                    counter4 = 0
                    For iloop = 1 To Len(SUBCAT_ID.ToString)
                        strcurrentchar = Mid(SUBCAT_ID, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter4 = 1
                            End If
                        End If
                    Next
                    If counter4 = 1 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        DDL_MemberSubCategories.Focus()
                        Exit Sub
                    End If
                Else
                    SUBCAT_ID = Nothing
                End If

                Dim USER_CODE As Object = Nothing
                If DDL_LibraryStaff.Text <> "" Then
                    USER_CODE = Trim(DDL_LibraryStaff.SelectedValue)

                    If Len(USER_CODE) > 10 Then
                        Lbl_Error.Text = "Error: Member No is not Proper!"
                        DDL_LibraryStaff.Focus()
                        Exit Sub
                    End If
                    USER_CODE = " " & USER_CODE & " "
                    If InStr(1, USER_CODE, "CREATE", 1) > 0 Or InStr(1, USER_CODE, "DELETE", 1) > 0 Or InStr(1, USER_CODE, "DROP", 1) > 0 Or InStr(1, USER_CODE, "INSERT", 1) > 1 Or InStr(1, USER_CODE, "TRACK", 1) > 1 Or InStr(1, USER_CODE, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Error: Member No Input is not Valid !"
                        DDL_LibraryStaff.Focus()
                        Exit Sub
                    End If
                    USER_CODE = TrimX(USER_CODE)
                    'check unwanted characters
                    c = 0
                    counter5 = 0
                    For iloop = 1 To Len(USER_CODE.ToString)
                        strcurrentchar = Mid(USER_CODE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter5 = 1
                            End If
                        End If
                    Next
                    If counter5 = 1 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        DDL_LibraryStaff.Focus()
                        Exit Sub
                    End If
                Else
                    USER_CODE = Nothing
                End If

                Dim HOLD_ID As Integer = Nothing
                If DDL_AccessionNo.Text <> "" Then
                    HOLD_ID = Trim(DDL_AccessionNo.SelectedValue)

                    If Len(HOLD_ID) > 10 Then
                        Lbl_Error.Text = "Error: Member No is not Proper!"
                        DDL_AccessionNo.Focus()
                        Exit Sub
                    End If
                    HOLD_ID = " " & HOLD_ID & " "
                    If InStr(1, HOLD_ID, "CREATE", 1) > 0 Or InStr(1, HOLD_ID, "DELETE", 1) > 0 Or InStr(1, HOLD_ID, "DROP", 1) > 0 Or InStr(1, HOLD_ID, "INSERT", 1) > 1 Or InStr(1, HOLD_ID, "TRACK", 1) > 1 Or InStr(1, HOLD_ID, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Error: Member No Input is not Valid !"
                        DDL_AccessionNo.Focus()
                        Exit Sub
                    End If
                    HOLD_ID = TrimX(HOLD_ID)
                    'check unwanted characters
                    c = 0
                    counter6 = 0
                    For iloop = 1 To Len(HOLD_ID.ToString)
                        strcurrentchar = Mid(HOLD_ID, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter6 = 1
                            End If
                        End If
                    Next
                    If counter6 = 1 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        DDL_AccessionNo.Focus()
                        Exit Sub
                    End If
                Else
                    HOLD_ID = Nothing
                End If

                Dim STATUS As Object = Nothing
                If DDL_Status.Text <> "" Then
                    STATUS = Trim(DDL_Status.SelectedValue)

                    If Len(STATUS) > 10 Then
                        Lbl_Error.Text = "Error: Member No is not Proper!"
                        DDL_Status.Focus()
                        Exit Sub
                    End If
                    STATUS = " " & STATUS & " "
                    If InStr(1, STATUS, "CREATE", 1) > 0 Or InStr(1, STATUS, "DELETE", 1) > 0 Or InStr(1, STATUS, "DROP", 1) > 0 Or InStr(1, STATUS, "INSERT", 1) > 1 Or InStr(1, STATUS, "TRACK", 1) > 1 Or InStr(1, STATUS, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Error: Member No Input is not Valid !"
                        DDL_Status.Focus()
                        Exit Sub
                    End If
                    STATUS = TrimX(STATUS)
                    'check unwanted characters
                    c = 0
                    counter7 = 0
                    For iloop = 1 To Len(STATUS.ToString)
                        strcurrentchar = Mid(STATUS, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter7 = 1
                            End If
                        End If
                    Next
                    If counter7 = 1 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        DDL_Status.Focus()
                        Exit Sub
                    End If
                Else
                    STATUS = Nothing
                End If

                Dim COLLECTION_TYPE As Object = Nothing
                If DDL_CollectionType.Text <> "" Then
                    COLLECTION_TYPE = Trim(DDL_CollectionType.SelectedValue)

                    If Len(COLLECTION_TYPE) > 10 Then
                        Lbl_Error.Text = "Error: Member No is not Proper!"
                        DDL_CollectionType.Focus()
                        Exit Sub
                    End If
                    COLLECTION_TYPE = " " & COLLECTION_TYPE & " "
                    If InStr(1, COLLECTION_TYPE, "CREATE", 1) > 0 Or InStr(1, COLLECTION_TYPE, "DELETE", 1) > 0 Or InStr(1, COLLECTION_TYPE, "DROP", 1) > 0 Or InStr(1, COLLECTION_TYPE, "INSERT", 1) > 1 Or InStr(1, COLLECTION_TYPE, "TRACK", 1) > 1 Or InStr(1, COLLECTION_TYPE, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Error: Member No Input is not Valid !"
                        DDL_CollectionType.Focus()
                        Exit Sub
                    End If
                    COLLECTION_TYPE = TrimX(COLLECTION_TYPE)
                    'check unwanted characters
                    c = 0
                    counter8 = 0
                    For iloop = 1 To Len(COLLECTION_TYPE.ToString)
                        strcurrentchar = Mid(COLLECTION_TYPE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter8 = 1
                            End If
                        End If
                    Next
                    If counter8 = 1 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        DDL_CollectionType.Focus()
                        Exit Sub
                    End If
                Else
                    COLLECTION_TYPE = Nothing
                End If

                Dim S_DATE As Object = Nothing
                If txt_Tra_SDate.Text <> "" Then
                    S_DATE = TrimX(txt_Tra_SDate.Text)
                    S_DATE = RemoveQuotes(S_DATE)
                    S_DATE = Convert.ToDateTime(S_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                    If Len(S_DATE) > 12 Then
                        Lbl_Error.Text = " Input is not Valid..."
                        Me.txt_Tra_SDate.Focus()
                        Exit Sub
                    End If
                    S_DATE = " " & S_DATE & " "
                    If InStr(1, S_DATE, "CREATE", 1) > 0 Or InStr(1, S_DATE, "DELETE", 1) > 0 Or InStr(1, S_DATE, "DROP", 1) > 0 Or InStr(1, S_DATE, "INSERT", 1) > 1 Or InStr(1, S_DATE, "TRACK", 1) > 1 Or InStr(1, S_DATE, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "  Input is not Valid... "
                        Me.txt_Tra_SDate.Focus()
                        Exit Sub
                    End If
                    S_DATE = TrimX(S_DATE)
                    'check unwanted characters
                    c = 0
                    counter9 = 0
                    For iloop = 1 To Len(S_DATE)
                        strcurrentchar = Mid(S_DATE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!@#$%^&*-_+<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter9 = 1
                            End If
                        End If
                    Next
                    If counter9 = 1 Then
                        Lbl_Error.Text = "data is not Valid... "
                        Me.txt_Tra_SDate.Focus()
                        Exit Sub
                    End If
                Else
                    S_DATE = ""
                End If

                Dim E_DATE As Object = Nothing
                If txt_Tra_EDate.Text <> "" Then
                    E_DATE = TrimX(txt_Tra_EDate.Text)
                    E_DATE = RemoveQuotes(E_DATE)
                    E_DATE = Convert.ToDateTime(E_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                    If Len(E_DATE) > 12 Then
                        Lbl_Error.Text = " Input is not Valid..."
                        Me.txt_Tra_EDate.Focus()
                        Exit Sub
                    End If
                    E_DATE = " " & E_DATE & " "
                    If InStr(1, E_DATE, "CREATE", 1) > 0 Or InStr(1, E_DATE, "DELETE", 1) > 0 Or InStr(1, E_DATE, "DROP", 1) > 0 Or InStr(1, E_DATE, "INSERT", 1) > 1 Or InStr(1, E_DATE, "TRACK", 1) > 1 Or InStr(1, E_DATE, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "  Input is not Valid... "
                        Me.txt_Tra_EDate.Focus()
                        Exit Sub
                    End If
                    E_DATE = TrimX(E_DATE)
                    'check unwanted characters
                    c = 0
                    counter10 = 0
                    For iloop = 1 To Len(E_DATE)
                        strcurrentchar = Mid(E_DATE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!@#$%^&*-_+<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter10 = 1
                            End If
                        End If
                    Next
                    If counter10 = 1 Then
                        Lbl_Error.Text = "data is not Valid... "
                        Me.txt_Tra_EDate.Focus()
                        Exit Sub
                    End If
                Else
                    E_DATE = ""
                End If

                Dim ORDER_BY As Object = Nothing
                If DDL_OrderBy.Text <> "" Then
                    ORDER_BY = Trim(DDL_OrderBy.SelectedValue)

                    If Len(ORDER_BY) > 20 Then
                        Lbl_Error.Text = "Error: Member No is not Proper!"
                        DDL_OrderBy.Focus()
                        Exit Sub
                    End If
                    ORDER_BY = " " & ORDER_BY & " "
                    If InStr(1, ORDER_BY, "CREATE", 1) > 0 Or InStr(1, ORDER_BY, "DELETE", 1) > 0 Or InStr(1, ORDER_BY, "DROP", 1) > 0 Or InStr(1, ORDER_BY, "INSERT", 1) > 1 Or InStr(1, ORDER_BY, "TRACK", 1) > 1 Or InStr(1, ORDER_BY, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Error: Member No Input is not Valid !"
                        DDL_OrderBy.Focus()
                        Exit Sub
                    End If
                    ORDER_BY = TrimX(ORDER_BY)
                    'check unwanted characters
                    c = 0
                    counter11 = 0
                    For iloop = 1 To Len(ORDER_BY.ToString)
                        strcurrentchar = Mid(ORDER_BY, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter11 = 1
                            End If
                        End If
                    Next
                    If counter11 = 1 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        DDL_OrderBy.Focus()
                        Exit Sub
                    End If
                Else
                    ORDER_BY = "MEM_NO"
                End If

                Dim SORT_BY As Object = Nothing
                If DDL_SortBy.Text <> "" Then
                    SORT_BY = Trim(DDL_SortBy.SelectedValue)

                    If Len(SORT_BY) > 5 Then
                        Lbl_Error.Text = "Error: Member No is not Proper!"
                        DDL_SortBy.Focus()
                        Exit Sub
                    End If
                    SORT_BY = " " & SORT_BY & " "
                    If InStr(1, SORT_BY, "CREATE", 1) > 0 Or InStr(1, SORT_BY, "DELETE", 1) > 0 Or InStr(1, SORT_BY, "DROP", 1) > 0 Or InStr(1, SORT_BY, "INSERT", 1) > 1 Or InStr(1, SORT_BY, "TRACK", 1) > 1 Or InStr(1, SORT_BY, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Error: Member No Input is not Valid !"
                        DDL_SortBy.Focus()
                        Exit Sub
                    End If
                    SORT_BY = TrimX(SORT_BY)
                    'check unwanted characters
                    c = 0
                    counter12 = 0
                    For iloop = 1 To Len(SORT_BY.ToString)
                        strcurrentchar = Mid(SORT_BY, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter12 = 1
                            End If
                        End If
                    Next
                    If counter12 = 1 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        DDL_SortBy.Focus()
                        Exit Sub
                    End If
                Else
                    SORT_BY = "ASC"
                End If

                Dim SQL As String = Nothing
                SQL = "SELECT CIRCULATION_TRANSACTIONS_VIEW.CIR_ID,MEM_NAME, MEM_ID, MEM_NO, Accession = CASE (isnull(accession_no,'')) WHen ''  THEN cast(copy_id as varchar(20))" & _
                       " ELSE accession_no END, Title = CASE (isnull(Title,'')) WHen ''  THEN journal ELSE title END, convert(char(10),ISSUE_DATE,103) as ISSUE_DATE,ISSUE_TIME,convert(char(10),DUE_DATE,103) as DUE_DATE,convert(char(10),RETURN_DATE,103) as RETURN_DATE,RETURN_TIME," & _
                       " FINE_COLLECTED, convert(char(10),RESERVE_DATE,103) as RESERVE_DATE, convert(char(10),RENEW_DATE,103) as RENEW_DATE, STATUS, VOL_NO, CIR_LIB_CODE  FROM CIRCULATION_TRANSACTIONS_VIEW WHERE (CIR_LIB_CODE = '" & Trim(LibCode) & "') "

                If RadioButton1.Checked = True Then
                    SQL = SQL & " AND (COPY_ID IS NULL) "
                Else
                    SQL = SQL & " AND (COPY_ID IS NOT NULL) "
                End If


                If DDL_MemberNo.Text = "" And DDL_MemberName.Text = "" And DDL_MemberCategories.Text = "" And DDL_MemberSubCategories.Text = "" And DDL_LibraryStaff.Text = "" And DDL_AccessionNo.Text = "" Then
                    SQL = SQL
                End If

                If DDL_MemberNo.Text <> "" And DDL_MemberName.Text = "" And DDL_MemberCategories.Text = "" And DDL_MemberSubCategories.Text = "" And DDL_LibraryStaff.Text = "" And DDL_AccessionNo.Text = "" Then
                    SQL = SQL & " AND (MEM_ID ='" & Trim(MEM_ID) & "') "
                End If

                If DDL_MemberNo.Text = "" And DDL_MemberName.Text <> "" And DDL_MemberCategories.Text = "" And DDL_MemberSubCategories.Text = "" And DDL_LibraryStaff.Text = "" And DDL_AccessionNo.Text = "" Then
                    SQL = SQL & " AND (MEM_ID ='" & Trim(MEM_ID2) & "') "
                End If

                If DDL_MemberNo.Text = "" And DDL_MemberName.Text = "" And DDL_MemberCategories.Text <> "" And DDL_MemberSubCategories.Text = "" And DDL_LibraryStaff.Text = "" And DDL_AccessionNo.Text = "" Then
                    SQL = SQL & " AND (CAT_ID ='" & Trim(CAT_ID) & "') "
                End If

                If DDL_MemberNo.Text = "" And DDL_MemberName.Text = "" And DDL_MemberCategories.Text = "" And DDL_MemberSubCategories.Text <> "" And DDL_LibraryStaff.Text = "" And DDL_AccessionNo.Text = "" Then
                    SQL = SQL & " AND (SUBCAT_ID ='" & Trim(SUBCAT_ID) & "') "
                End If

                If DDL_MemberNo.Text = "" And DDL_MemberName.Text = "" And DDL_MemberCategories.Text = "" And DDL_MemberSubCategories.Text = "" And DDL_LibraryStaff.Text <> "" And DDL_AccessionNo.Text = "" Then
                    SQL = SQL & " AND (USER_CODE ='" & Trim(USER_CODE) & "') "
                End If

                If DDL_MemberNo.Text = "" And DDL_MemberName.Text = "" And DDL_MemberCategories.Text = "" And DDL_MemberSubCategories.Text = "" And DDL_LibraryStaff.Text = "" And DDL_AccessionNo.Text <> "" Then
                    SQL = SQL & " AND (HOLD_ID ='" & Trim(HOLD_ID) & "') "
                End If

                If DDL_Status.Text <> "" Then
                    If STATUS = "Renewed" Then
                        SQL = SQL & " AND (STATUS = 'Issued' AND RENEW_DATE <>'') "
                    Else
                        SQL = SQL & " AND (STATUS = '" & Trim(STATUS) & "') "
                    End If
                End If

                If txt_Tra_SDate.Text <> "" And txt_Tra_EDate.Text <> "" Then
                    If DDL_Date.Text <> "" Then
                        SQL = SQL & " AND (" & DDL_Date.SelectedValue & ">= '" & Trim(S_DATE) & "' AND " & DDL_Date.SelectedValue & "<= '" & Trim(E_DATE) & "')"
                    End If
                End If

                If DDL_CollectionType.Text <> "" Then
                    SQL = SQL & " AND HOLD_ID IN (SELECT HOLD_ID FROM HOLDINGS WHERE COLLECTION_TYPE = '" & Trim(COLLECTION_TYPE) & "')"
                End If

                If ORDER_BY = "ACCESSION_NO" Then
                    SQL = SQL & " ORDER BY CASE WHEN LEFT(Accession_NO,1) LIKE  '[A-Za-z]'  THEN dbo.Sort_AlphaNumeric(Accession_No, 'A-Za-z') END ASC, CAST(dbo.Sort_AlphaNumeric(Accession_no, '0-9') AS float) " & SORT_BY
                ElseIf ORDER_BY = "MEM_NO" Then
                    SQL = SQL & " ORDER BY CASE WHEN LEFT(MEM_NO,1) LIKE  '[A-Za-z]'  THEN dbo.Sort_AlphaNumeric(MEM_NO, 'A-Za-z') END ASC, CAST(dbo.Sort_AlphaNumeric(MEM_NO, '0-9') AS float) " & SORT_BY
                ElseIf ORDER_BY = "MEM_NAME" Then
                    SQL = SQL & " ORDER BY CIRCULATION_TRANSACTIONS_VIEW.MEM_NAME " & SORT_BY
                Else
                    SQL = SQL & " ORDER BY " & ORDER_BY & " " & SORT_BY
                End If

                Dim ds As New DataSet
                Dim da As New SqlDataAdapter(SQL, SqlConn)
                If SqlConn.State = 0 Then
                    SqlConn.Open()
                End If
                da.Fill(ds)

                dtSearch = ds.Tables(0).Copy
                Dim RecordCount As Long = 0
                Grid2.DataSource = Nothing
                Grid2.DataBind()
                Grid2.Visible = False
                TR_Grid2.Visible = False

                Grid3.DataSource = Nothing
                Grid3.DataBind()
                Grid3.Visible = False
                TR_Grid3.Visible = False

                If dtSearch.Rows.Count = 0 Then
                    Grid1.DataSource = Nothing
                    Grid1.DataBind()
                    Label1.Text = "Total Record(s): 0 "
                    Tra_Delete_Bttn.Visible = False
                    Print_Summary_Bttn.Visible = False
                    Print_Reminder_Bttn.Visible = False
                    Print_MostIssuedBooks_Bttn.Visible = False
                    Print_TopBorrowers_Bttn.Visible = False
                Else
                    Grid1.AutoGenerateColumns = False
                    RecordCount = dtSearch.Rows.Count
                    Me.Grid1.DataSource = dtSearch
                    Me.Grid1.DataBind()
                    TR_Grid1.Visible = True
                    Grid1.Visible = True
                    RecordCount = dtSearch.Rows.Count
                    Label1.Text = "Total Record(s): " & RecordCount
                    Tra_Delete_Bttn.Visible = True
                    Print_Summary_Bttn.Visible = True
                    Print_Reminder_Bttn.Visible = False
                    Print_MostIssuedBooks_Bttn.Visible = False
                    Print_TopBorrowers_Bttn.Visible = False
                End If
                ViewState("dt") = dtSearch
            End If
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
    'clear member no
    Protected Sub DDL_MemberNo_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDL_MemberNo.TextChanged
        DDL_MemberName.ClearSelection()
        DDL_MemberCategories.ClearSelection()
        DDL_MemberSubCategories.ClearSelection()
        DDL_LibraryStaff.ClearSelection()
        DDL_AccessionNo.ClearSelection()
        DDL_MemberNo.Focus()
    End Sub
    'clear member name
    Protected Sub DDL_MemberName_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDL_MemberName.SelectedIndexChanged
        DDL_MemberNo.ClearSelection()
        DDL_MemberCategories.ClearSelection()
        DDL_MemberSubCategories.ClearSelection()
        DDL_LibraryStaff.ClearSelection()
        DDL_AccessionNo.ClearSelection()
        DDL_MemberName.Focus()
    End Sub
    'clear category
    Protected Sub DDL_MemberCategories_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDL_MemberCategories.SelectedIndexChanged
        DDL_MemberNo.ClearSelection()
        DDL_MemberName.ClearSelection()
        DDL_MemberSubCategories.ClearSelection()
        DDL_LibraryStaff.ClearSelection()
        DDL_AccessionNo.ClearSelection()
        DDL_MemberCategories.Focus()
    End Sub
    'clear sub category
    Protected Sub DDL_MemberSubCategories_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDL_MemberSubCategories.SelectedIndexChanged
        DDL_MemberNo.ClearSelection()
        DDL_MemberName.ClearSelection()
        DDL_MemberCategories.ClearSelection()
        DDL_LibraryStaff.ClearSelection()
        DDL_AccessionNo.ClearSelection()
        DDL_MemberSubCategories.Focus()
    End Sub
    'clear library staff
    Protected Sub DDL_LibraryStaff_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDL_LibraryStaff.SelectedIndexChanged
        DDL_MemberNo.ClearSelection()
        DDL_MemberName.ClearSelection()
        DDL_MemberCategories.ClearSelection()
        DDL_MemberSubCategories.ClearSelection()
        DDL_AccessionNo.ClearSelection()
        DDL_LibraryStaff.Focus()
    End Sub
    'clear accession no
    Protected Sub DDL_AccessionNo_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDL_AccessionNo.SelectedIndexChanged
        DDL_MemberNo.ClearSelection()
        DDL_MemberName.ClearSelection()
        DDL_MemberCategories.ClearSelection()
        DDL_MemberSubCategories.ClearSelection()
        DDL_LibraryStaff.ClearSelection()
        DDL_AccessionNo.Focus()
    End Sub
    'delete transactions
    Protected Sub Tra_Delete_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Tra_Delete_Bttn.Click
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing
        Try
            For Each row As GridViewRow In Grid1.Rows
                Dim cb As CheckBox = DirectCast(row.FindControl("cbd"), CheckBox)
                If cb IsNot Nothing AndAlso cb.Checked = True Then
                    Dim ID As Integer = Convert.ToInt32(Grid1.DataKeys(row.RowIndex).Value)

                    If SqlConn.State = 0 Then
                        SqlConn.Open()
                    End If

                    thisTransaction = SqlConn.BeginTransaction()
                    Dim objCommand As New SqlCommand
                    objCommand.Connection = SqlConn
                    objCommand.Transaction = thisTransaction
                    objCommand.CommandType = CommandType.Text
                    objCommand.CommandText = "DELETE FROM CIRCULATION WHERE (CIR_ID =@CIR_ID) AND (LIB_CODE =@LIB_CODE) AND (STATUS ='Returned' OR STATUS ='Reserved') "

                    objCommand.Parameters.Add("@CIR_ID", SqlDbType.Int)
                    objCommand.Parameters("@CIR_ID").Value = ID

                    objCommand.Parameters.Add("@LIB_CODE", SqlDbType.NVarChar)
                    objCommand.Parameters("@LIB_CODE").Value = LibCode

                    objCommand.ExecuteNonQuery()
                    thisTransaction.Commit()
                    SqlConn.Close()
                End If
            Next
            Lbl_Error.Text = "Selected Record(s) Deleted!"
            Dim s As Object = Nothing
            Dim e1 As EventArgs = Nothing
            Tra_Search_Bttn_Click(sender, e)
        Catch s As Exception
            thisTransaction.Rollback()
            Lbl_Error.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    Protected Sub Tra_AllOverdue_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Tra_AllOverdue_Bttn.Click
        Dim dtSearch As DataTable = Nothing
        Try
            If IsPostBack = True Then
                Dim iloop As Integer
                Dim strcurrentchar As Object
                Dim c As Integer
                Dim counter1, counter2, counter3, counter4, counter5, counter6, counter7, counter8, counter9, counter10, counter11, counter12, counter13, counter14, counter15, counter16, counter17, counter18, counter19 As Integer

                Dim TODAY_DATE As Object = Nothing
                TODAY_DATE = Now.Date

                Dim DueDays As Double = Nothing
                If txt_Cir_DueDays.Text <> "" Then
                    DueDays = TrimX(txt_Cir_DueDays.Text)
                End If

                If txt_Cir_DueDays.Text <> "" Then
                    TODAY_DATE = DateAdd("d", DueDays, TODAY_DATE)
                End If
                TODAY_DATE = Convert.ToDateTime(TODAY_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us


                Dim MEM_ID As Integer = Nothing
                If DDL_MemberNo.Text <> "" Then
                    MEM_ID = Trim(DDL_MemberNo.SelectedValue)

                    If Len(MEM_ID) > 10 Then
                        Lbl_Error.Text = "Error: Member No is not Proper!"
                        DDL_MemberNo.Focus()
                        Exit Sub
                    End If
                    MEM_ID = " " & MEM_ID & " "
                    If InStr(1, MEM_ID, "CREATE", 1) > 0 Or InStr(1, MEM_ID, "DELETE", 1) > 0 Or InStr(1, MEM_ID, "DROP", 1) > 0 Or InStr(1, MEM_ID, "INSERT", 1) > 1 Or InStr(1, MEM_ID, "TRACK", 1) > 1 Or InStr(1, MEM_ID, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Error: Member No Input is not Valid !"
                        DDL_MemberNo.Focus()
                        Exit Sub
                    End If
                    MEM_ID = TrimX(MEM_ID)
                    'check unwanted characters
                    c = 0
                    counter1 = 0
                    For iloop = 1 To Len(MEM_ID.ToString)
                        strcurrentchar = Mid(MEM_ID, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter1 = 1
                            End If
                        End If
                    Next
                    If counter1 = 1 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        DDL_MemberNo.Focus()
                        Exit Sub
                    End If
                Else
                    MEM_ID = Nothing
                End If

                Dim MEM_ID2 As Integer = Nothing
                If DDL_MemberName.Text <> "" Then
                    MEM_ID2 = Trim(DDL_MemberName.SelectedValue)

                    If Len(MEM_ID2) > 50 Then
                        Lbl_Error.Text = "Error: Member No is not Proper!"
                        DDL_MemberName.Focus()
                        Exit Sub
                    End If
                    MEM_ID2 = " " & MEM_ID2 & " "
                    If InStr(1, MEM_ID2, "CREATE", 1) > 0 Or InStr(1, MEM_ID2, "DELETE", 1) > 0 Or InStr(1, MEM_ID2, "DROP", 1) > 0 Or InStr(1, MEM_ID2, "INSERT", 1) > 1 Or InStr(1, MEM_ID2, "TRACK", 1) > 1 Or InStr(1, MEM_ID2, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Error: Member No Input is not Valid !"
                        DDL_MemberName.Focus()
                        Exit Sub
                    End If
                    MEM_ID2 = TrimX(MEM_ID2)
                    'check unwanted characters
                    c = 0
                    counter2 = 0
                    For iloop = 1 To Len(MEM_ID2.ToString)
                        strcurrentchar = Mid(MEM_ID2, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter2 = 1
                            End If
                        End If
                    Next
                    If counter2 = 1 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        DDL_MemberName.Focus()
                        Exit Sub
                    End If
                Else
                    MEM_ID2 = Nothing
                End If

                Dim CAT_ID As Integer = Nothing
                If DDL_MemberCategories.Text <> "" Then
                    CAT_ID = Trim(DDL_MemberCategories.SelectedValue)
                    If IsNumeric(CAT_ID) = False Then
                        Lbl_Error.Text = "Error: Member ID is not Proper!"
                        DDL_MemberCategories.Focus()
                        Exit Sub
                    End If
                    If Len(CAT_ID) > 10 Then
                        Lbl_Error.Text = "Error: Member No is not Proper!"
                        DDL_MemberCategories.Focus()
                        Exit Sub
                    End If
                    CAT_ID = " " & CAT_ID & " "
                    If InStr(1, CAT_ID, "CREATE", 1) > 0 Or InStr(1, CAT_ID, "DELETE", 1) > 0 Or InStr(1, CAT_ID, "DROP", 1) > 0 Or InStr(1, CAT_ID, "INSERT", 1) > 1 Or InStr(1, CAT_ID, "TRACK", 1) > 1 Or InStr(1, CAT_ID, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Error: Member No Input is not Valid !"
                        DDL_MemberCategories.Focus()
                        Exit Sub
                    End If
                    CAT_ID = TrimX(CAT_ID)
                    'check unwanted characters
                    c = 0
                    counter3 = 0
                    For iloop = 1 To Len(CAT_ID.ToString)
                        strcurrentchar = Mid(CAT_ID, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter3 = 1
                            End If
                        End If
                    Next
                    If counter3 = 1 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        DDL_MemberCategories.Focus()
                        Exit Sub
                    End If
                Else
                    CAT_ID = Nothing
                End If

                Dim SUBCAT_ID As Integer = Nothing
                If DDL_MemberSubCategories.Text <> "" Then
                    SUBCAT_ID = Trim(DDL_MemberSubCategories.SelectedValue)
                    If IsNumeric(SUBCAT_ID) = False Then
                        Lbl_Error.Text = "Error: Member ID is not Proper!"
                        DDL_MemberSubCategories.Focus()
                        Exit Sub
                    End If
                    If Len(SUBCAT_ID) > 10 Then
                        Lbl_Error.Text = "Error: Member No is not Proper!"
                        DDL_MemberSubCategories.Focus()
                        Exit Sub
                    End If
                    SUBCAT_ID = " " & SUBCAT_ID & " "
                    If InStr(1, SUBCAT_ID, "CREATE", 1) > 0 Or InStr(1, SUBCAT_ID, "DELETE", 1) > 0 Or InStr(1, SUBCAT_ID, "DROP", 1) > 0 Or InStr(1, SUBCAT_ID, "INSERT", 1) > 1 Or InStr(1, SUBCAT_ID, "TRACK", 1) > 1 Or InStr(1, SUBCAT_ID, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Error: Member No Input is not Valid !"
                        DDL_MemberSubCategories.Focus()
                        Exit Sub
                    End If
                    SUBCAT_ID = TrimX(SUBCAT_ID)
                    'check unwanted characters
                    c = 0
                    counter4 = 0
                    For iloop = 1 To Len(SUBCAT_ID.ToString)
                        strcurrentchar = Mid(SUBCAT_ID, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter4 = 1
                            End If
                        End If
                    Next
                    If counter4 = 1 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        DDL_MemberSubCategories.Focus()
                        Exit Sub
                    End If
                Else
                    SUBCAT_ID = Nothing
                End If

                Dim HOLD_ID As Integer = Nothing
                If DDL_AccessionNo.Text <> "" Then
                    HOLD_ID = Trim(DDL_AccessionNo.SelectedValue)

                    If Len(HOLD_ID) > 10 Then
                        Lbl_Error.Text = "Error: Member No is not Proper!"
                        DDL_AccessionNo.Focus()
                        Exit Sub
                    End If
                    HOLD_ID = " " & HOLD_ID & " "
                    If InStr(1, HOLD_ID, "CREATE", 1) > 0 Or InStr(1, HOLD_ID, "DELETE", 1) > 0 Or InStr(1, HOLD_ID, "DROP", 1) > 0 Or InStr(1, HOLD_ID, "INSERT", 1) > 1 Or InStr(1, HOLD_ID, "TRACK", 1) > 1 Or InStr(1, HOLD_ID, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Error: Member No Input is not Valid !"
                        DDL_AccessionNo.Focus()
                        Exit Sub
                    End If
                    HOLD_ID = TrimX(HOLD_ID)
                    'check unwanted characters
                    c = 0
                    counter6 = 0
                    For iloop = 1 To Len(HOLD_ID.ToString)
                        strcurrentchar = Mid(HOLD_ID, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter6 = 1
                            End If
                        End If
                    Next
                    If counter6 = 1 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        DDL_AccessionNo.Focus()
                        Exit Sub
                    End If
                Else
                    HOLD_ID = Nothing
                End If

                Dim COLLECTION_TYPE As Object = Nothing
                If DDL_CollectionType.Text <> "" Then
                    COLLECTION_TYPE = Trim(DDL_CollectionType.SelectedValue)

                    If Len(COLLECTION_TYPE) > 10 Then
                        Lbl_Error.Text = "Error: Member No is not Proper!"
                        DDL_CollectionType.Focus()
                        Exit Sub
                    End If
                    COLLECTION_TYPE = " " & COLLECTION_TYPE & " "
                    If InStr(1, COLLECTION_TYPE, "CREATE", 1) > 0 Or InStr(1, COLLECTION_TYPE, "DELETE", 1) > 0 Or InStr(1, COLLECTION_TYPE, "DROP", 1) > 0 Or InStr(1, COLLECTION_TYPE, "INSERT", 1) > 1 Or InStr(1, COLLECTION_TYPE, "TRACK", 1) > 1 Or InStr(1, COLLECTION_TYPE, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Error: Member No Input is not Valid !"
                        DDL_CollectionType.Focus()
                        Exit Sub
                    End If
                    COLLECTION_TYPE = TrimX(COLLECTION_TYPE)
                    'check unwanted characters
                    c = 0
                    counter8 = 0
                    For iloop = 1 To Len(COLLECTION_TYPE.ToString)
                        strcurrentchar = Mid(COLLECTION_TYPE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter8 = 1
                            End If
                        End If
                    Next
                    If counter8 = 1 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        DDL_CollectionType.Focus()
                        Exit Sub
                    End If
                Else
                    COLLECTION_TYPE = Nothing
                End If

                Dim ORDER_BY As Object = Nothing
                If DDL_OrderBy.Text <> "" Then
                    ORDER_BY = Trim(DDL_OrderBy.SelectedValue)

                    If Len(ORDER_BY) > 20 Then
                        Lbl_Error.Text = "Error: Member No is not Proper!"
                        DDL_OrderBy.Focus()
                        Exit Sub
                    End If
                    ORDER_BY = " " & ORDER_BY & " "
                    If InStr(1, ORDER_BY, "CREATE", 1) > 0 Or InStr(1, ORDER_BY, "DELETE", 1) > 0 Or InStr(1, ORDER_BY, "DROP", 1) > 0 Or InStr(1, ORDER_BY, "INSERT", 1) > 1 Or InStr(1, ORDER_BY, "TRACK", 1) > 1 Or InStr(1, ORDER_BY, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Error: Member No Input is not Valid !"
                        DDL_OrderBy.Focus()
                        Exit Sub
                    End If
                    ORDER_BY = TrimX(ORDER_BY)
                    'check unwanted characters
                    c = 0
                    counter11 = 0
                    For iloop = 1 To Len(ORDER_BY.ToString)
                        strcurrentchar = Mid(ORDER_BY, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter11 = 1
                            End If
                        End If
                    Next
                    If counter11 = 1 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        DDL_OrderBy.Focus()
                        Exit Sub
                    End If
                Else
                    ORDER_BY = "MEM_NO"
                End If

                Dim SORT_BY As Object = Nothing
                If DDL_SortBy.Text <> "" Then
                    SORT_BY = Trim(DDL_SortBy.SelectedValue)

                    If Len(SORT_BY) > 5 Then
                        Lbl_Error.Text = "Error: Member No is not Proper!"
                        DDL_SortBy.Focus()
                        Exit Sub
                    End If
                    SORT_BY = " " & SORT_BY & " "
                    If InStr(1, SORT_BY, "CREATE", 1) > 0 Or InStr(1, SORT_BY, "DELETE", 1) > 0 Or InStr(1, SORT_BY, "DROP", 1) > 0 Or InStr(1, SORT_BY, "INSERT", 1) > 1 Or InStr(1, SORT_BY, "TRACK", 1) > 1 Or InStr(1, SORT_BY, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Error: Member No Input is not Valid !"
                        DDL_SortBy.Focus()
                        Exit Sub
                    End If
                    SORT_BY = TrimX(SORT_BY)
                    'check unwanted characters
                    c = 0
                    counter12 = 0
                    For iloop = 1 To Len(SORT_BY.ToString)
                        strcurrentchar = Mid(SORT_BY, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter12 = 1
                            End If
                        End If
                    Next
                    If counter12 = 1 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        DDL_SortBy.Focus()
                        Exit Sub
                    End If
                Else
                    SORT_BY = "ASC"
                End If

                Dim SQL As String = Nothing
                'SQL = "SELECT CIRCULATION_TRANSACTIONS_VIEW.CIR_ID, MEM_NAME, MEM_ID, MEM_NO, Accession = CASE (isnull(accession_no,'')) WHen ''  THEN cast(copy_id as varchar(20))" & _
                ' " ELSE accession_no END, Title = CASE (isnull(Title,'')) WHen ''  THEN journal ELSE title END, convert(char(10),ISSUE_DATE,103) as ISSUE_DATE,ISSUE_TIME,convert(char(10),DUE_DATE,103) as DUE_DATE,convert(char(10),RETURN_DATE,103) as RETURN_DATE,RETURN_TIME," & _
                ' " FINE_COLLECTED, convert(char(10),RESERVE_DATE,103) as RESERVE_DATE, convert(char(10),RENEW_DATE,103) as RENEW_DATE, STATUS, VOL_NO, VOL_NO2, ISSUE_NO, PART_NO, convert(char(10),ISS_DATE,103) as PUBLICATION_DATE, CIR_LIB_CODE  FROM CIRCULATION_TRANSACTIONS_VIEW WHERE (CIR_LIB_CODE = '" & Trim(LibCode) & "') AND (DUE_DATE <='" & Trim(TODAY_DATE) & "') AND (STATUS ='Issued') "

                SQL = "SELECT CIRCULATION_TRANSACTIONS_VIEW.CIR_ID, CIRCULATION_TRANSACTIONS_VIEW.MEM_NAME, CIRCULATION_TRANSACTIONS_VIEW.MEM_ID, CIRCULATION_TRANSACTIONS_VIEW.MEM_NO, Accession = CASE (isnull(accession_no,'')) WHen ''  THEN cast(copy_id as varchar(20))" _
                        & " ELSE accession_no END, Title = CASE (isnull(Title,'')) WHen ''  THEN journal ELSE title END, convert(char(10),ISSUE_DATE,103) as ISSUE_DATE,ISSUE_TIME,convert(char(10),DUE_DATE,103) as DUE_DATE,convert(char(10),RETURN_DATE,103) as RETURN_DATE,RETURN_TIME," _
                        & " FINE_COLLECTED, convert(char(10),RESERVE_DATE,103) as RESERVE_DATE, convert(char(10),RENEW_DATE,103) as RENEW_DATE, STATUS, VOL_NO, VOL_NO2, ISSUE_NO, PART_NO, convert(char(10),ISS_DATE,103) as PUBLICATION_DATE, CIR_LIB_CODE" _
                        & " , MEMB_GROUP_DESIG_SUB_VIEW.CAT_NAME, MEMB_GROUP_DESIG_SUB_VIEW.SUBCAT_NAME FROM CIRCULATION_TRANSACTIONS_VIEW LEFT OUTER JOIN" _
                        & " MEMB_GROUP_DESIG_SUB_VIEW ON CIRCULATION_TRANSACTIONS_VIEW.MEM_ID = MEMB_GROUP_DESIG_SUB_VIEW.MEM_ID" _
                        & " WHERE (CIRCULATION_TRANSACTIONS_VIEW.CIR_LIB_CODE = '" & Trim(LibCode) & "') AND (CIRCULATION_TRANSACTIONS_VIEW.DUE_DATE <='" & Trim(TODAY_DATE) & "') AND (CIRCULATION_TRANSACTIONS_VIEW.STATUS ='Issued') "

                If DDL_MemberNo.Text = "" And DDL_MemberName.Text = "" And DDL_MemberCategories.Text = "" And DDL_MemberSubCategories.Text = "" And DDL_AccessionNo.Text = "" Then
                    SQL = SQL
                End If

                If DDL_MemberNo.Text <> "" And DDL_MemberName.Text = "" And DDL_MemberCategories.Text = "" And DDL_MemberSubCategories.Text = "" And DDL_AccessionNo.Text = "" Then
                    SQL = SQL & " AND (CIRCULATION_TRANSACTIONS_VIEW.MEM_ID ='" & Trim(MEM_ID) & "') "
                End If

                If DDL_MemberNo.Text = "" And DDL_MemberName.Text <> "" And DDL_MemberCategories.Text = "" And DDL_MemberSubCategories.Text = "" And DDL_AccessionNo.Text = "" Then
                    SQL = SQL & " AND (CIRCULATION_TRANSACTIONS_VIEW.MEM_ID ='" & Trim(MEM_ID2) & "') "
                End If

                If DDL_MemberNo.Text = "" And DDL_MemberName.Text = "" And DDL_MemberCategories.Text <> "" And DDL_MemberSubCategories.Text = "" And DDL_AccessionNo.Text = "" Then
                    SQL = SQL & " AND (CIRCULATION_TRANSACTIONS_VIEW.CAT_ID ='" & Trim(CAT_ID) & "') "
                End If

                If DDL_MemberNo.Text = "" And DDL_MemberName.Text = "" And DDL_MemberCategories.Text = "" And DDL_MemberSubCategories.Text <> "" And DDL_AccessionNo.Text = "" Then
                    SQL = SQL & " AND (CIRCULATION_TRANSACTIONS_VIEW.SUBCAT_ID ='" & Trim(SUBCAT_ID) & "') "
                End If

                If DDL_MemberNo.Text = "" And DDL_MemberName.Text = "" And DDL_MemberCategories.Text = "" And DDL_MemberSubCategories.Text = "" And DDL_AccessionNo.Text <> "" Then
                    SQL = SQL & " AND (CIRCULATION_TRANSACTIONS_VIEW.HOLD_ID ='" & Trim(HOLD_ID) & "') "
                End If

                If DDL_CollectionType.Text <> "" Then
                    SQL = SQL & " AND CIRCULATION_TRANSACTIONS_VIEW.HOLD_ID IN (SELECT HOLD_ID FROM HOLDINGS WHERE COLLECTION_TYPE = '" & Trim(COLLECTION_TYPE) & "')"
                End If

                If ORDER_BY = "ACCESSION_NO" Then
                    SQL = SQL & " ORDER BY CASE WHEN LEFT(Accession_NO,1) LIKE  '[A-Za-z]'  THEN dbo.Sort_AlphaNumeric(Accession_No, 'A-Za-z') END ASC, CAST(dbo.Sort_AlphaNumeric(Accession_no, '0-9') AS float) " & SORT_BY
                ElseIf ORDER_BY = "MEM_NO" Then
                    SQL = SQL & " ORDER BY CASE WHEN LEFT(MEM_NO,1) LIKE  '[A-Za-z]'  THEN dbo.Sort_AlphaNumeric(MEM_NO, 'A-Za-z') END ASC, CAST(dbo.Sort_AlphaNumeric(MEM_NO, '0-9') AS float) " & SORT_BY
                ElseIf ORDER_BY = "MEM_NAME" Then
                    SQL = SQL & " ORDER BY CIRCULATION_TRANSACTIONS_VIEW.MEM_NAME " & SORT_BY
                Else
                    SQL = SQL & " ORDER BY " & ORDER_BY & " " & SORT_BY
                End If

                Dim ds As New DataSet
                Dim da As New SqlDataAdapter(SQL, SqlConn)
                If SqlConn.State = 0 Then
                    SqlConn.Open()
                End If
                da.Fill(ds)

                dtSearch = ds.Tables(0).Copy
                Dim RecordCount As Long = 0
                Grid2.DataSource = Nothing
                Grid2.DataBind()
                Grid2.Visible = False
                TR_Grid2.Visible = False

                Grid3.DataSource = Nothing
                Grid3.DataBind()
                Grid3.Visible = False
                TR_Grid3.Visible = False

                If dtSearch.Rows.Count = 0 Then
                    Grid1.DataSource = Nothing
                    Grid1.DataBind()
                    Label1.Text = "Total Record(s): 0 "
                    Tra_Delete_Bttn.Visible = False
                    Print_Summary_Bttn.Visible = False
                    Print_Reminder_Bttn.Visible = False
                    Print_MostIssuedBooks_Bttn.Visible = False
                    Print_TopBorrowers_Bttn.Visible = False
                Else
                    RecordCount = dtSearch.Rows.Count
                    Me.Grid1.DataSource = dtSearch
                    Me.Grid1.DataBind()
                    Grid1.Visible = True
                    TR_Grid1.Visible = True
                    Label1.Text = "Total Record(s): " & RecordCount
                    Tra_Delete_Bttn.Visible = False
                    Print_Summary_Bttn.Visible = True
                    Print_Reminder_Bttn.Visible = True
                    Print_MostIssuedBooks_Bttn.Visible = False
                    Print_TopBorrowers_Bttn.Visible = False
                End If
                ViewState("dt") = dtSearch
            End If
        Catch s As Exception
            Lbl_Error.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'most issued books
    Protected Sub Tra_MostIssuedBooks_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Tra_MostIssuedBooks_Bttn.Click
        Dim dtSearch As DataTable = Nothing
        Try
            If IsPostBack = True Then
                Dim SQL As String = Nothing
                If RadioButton1.Checked = True Then
                    SQL = "SELECT Accession = CASE (isnull(accession_no,'')) WHen ''  THEN cast(copy_id as varchar(20))" & _
                           " ELSE accession_NO END, Title = CASE (isnull(Title,'')) WHen ''  THEN JOURNAL ELSE title END, " & _
                           " COUNT (CASE (ISNULL (ACCESSION_NO, '')) WHEN '' THEN CAST (COPY_ID AS VARCHAR (500)) ELSE ACCESSION_NO END) AS TIMES " & _
                            " FROM CIRCULATION_TRANSACTIONS_VIEW WHERE (CIR_LIB_CODE = '" & Trim(LibCode) & "') AND (ACCESSION_NO<>'" & DBNull.Value & "')  " & _
                            " GROUP BY ACCESSION_NO, TITLE, JOURNAL, COPY_ID  ORDER BY TIMES DESC "
                End If
                If RadioButton2.Checked = True Then
                    SQL = "SELECT Accession = CASE (isnull(accession_no,'')) WHen ''  THEN cast(copy_id as varchar(20))" & _
                           " ELSE accession_NO END, Title = CASE (isnull(Title,'')) WHen ''  THEN JOURNAL ELSE title END, " & _
                           " COUNT (CASE (ISNULL (ACCESSION_NO, '')) WHEN '' THEN CAST (COPY_ID AS VARCHAR (500)) ELSE ACCESSION_NO END) AS TIMES " & _
                            " FROM CIRCULATION_TRANSACTIONS_VIEW WHERE (CIR_LIB_CODE = '" & Trim(LibCode) & "') AND (COPY_ID<>'" & 0 & "')  " & _
                            " GROUP BY ACCESSION_NO, TITLE, JOURNAL, COPY_ID  ORDER BY TIMES DESC "
                End If

                Dim ds As New DataSet
                Dim da As New SqlDataAdapter(SQL, SqlConn)
                If SqlConn.State = 0 Then
                    SqlConn.Open()
                End If
                da.Fill(ds)

                dtSearch = ds.Tables(0).Copy
                Dim RecordCount As Long = 0
                Grid1.DataSource = Nothing
                Grid1.DataBind()
                Grid1.Visible = False
                TR_Grid1.Visible = False

                Grid3.DataSource = Nothing
                Grid3.DataBind()
                Grid3.Visible = False
                TR_Grid3.Visible = False

                If dtSearch.Rows.Count = 0 Then
                    Grid2.DataSource = Nothing
                    Grid2.DataBind()
                    Label4.Text = "Total Record(s): 0 "
                    Tra_Delete_Bttn.Visible = False
                    Print_Summary_Bttn.Visible = False
                    Print_Reminder_Bttn.Visible = False
                    Print_MostIssuedBooks_Bttn.Visible = False
                    Print_TopBorrowers_Bttn.Visible = False
                Else
                    RecordCount = dtSearch.Rows.Count
                    Grid2.DataSource = dtSearch
                    Me.Grid2.DataBind()
                    Grid2.Visible = True
                    TR_Grid2.Visible = True
                    Label4.Text = "Total Record(s): " & RecordCount
                    Tra_Delete_Bttn.Visible = False
                    Print_Summary_Bttn.Visible = False
                    Print_Reminder_Bttn.Visible = False
                    Print_MostIssuedBooks_Bttn.Visible = True
                    Print_TopBorrowers_Bttn.Visible = False
                End If
                ViewState("dt") = dtSearch
            End If
        Catch s As Exception
            Lbl_Error.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'top borrowers
    Protected Sub Tra_TopBorrowers_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Tra_TopBorrowers_Bttn.Click
          Dim dtSearch As DataTable = Nothing
        Try
            If IsPostBack = True Then
                Dim SQL As String = Nothing
                If RadioButton1.Checked = True Then
                    SQL = "SELECT MEM_NO, MEM_NAME,  COUNT (MEM_NO) AS TIMES  FROM CIRCULATION_TRANSACTIONS_VIEW WHERE (CIR_LIB_CODE = '" & Trim(LibCode) & "') AND (ACCESSION_NO<>'" & DBNull.Value & "') GROUP BY MEM_NO, MEM_NAME ORDER BY TIMES DESC "
                End If
                If RadioButton2.Checked = True Then
                    SQL = "SELECT MEM_NO, MEM_NAME,  COUNT (MEM_NO) AS TIMES  FROM CIRCULATION_TRANSACTIONS_VIEW WHERE (CIR_LIB_CODE = '" & Trim(LibCode) & "') AND (COPY_ID<>'" & 0 & "') GROUP BY MEM_NO, MEM_NAME ORDER BY TIMES DESC "
                End If

                Dim ds As New DataSet
                Dim da As New SqlDataAdapter(SQL, SqlConn)
                If SqlConn.State = 0 Then
                    SqlConn.Open()
                End If
                da.Fill(ds)

                dtSearch = ds.Tables(0).Copy
                Dim RecordCount As Long = 0
                Grid1.DataSource = Nothing
                Grid1.DataBind()
                Grid1.Visible = False
                TR_Grid1.Visible = False

                Grid2.DataSource = Nothing
                Grid2.DataBind()
                Grid2.Visible = False
                TR_Grid2.Visible = False

                If dtSearch.Rows.Count = 0 Then
                    Grid3.DataSource = Nothing
                    Grid3.DataBind()
                    Label5.Text = "Total Record(s): 0 "
                    Tra_Delete_Bttn.Visible = False
                    Print_Summary_Bttn.Visible = False
                    Print_Reminder_Bttn.Visible = False
                    Print_MostIssuedBooks_Bttn.Visible = False
                    Print_TopBorrowers_Bttn.Visible = False
                Else
                    RecordCount = dtSearch.Rows.Count
                    Grid3.DataSource = dtSearch
                    Me.Grid3.DataBind()
                    Grid3.Visible = True
                    TR_Grid3.Visible = True
                    Label5.Text = "Total Record(s): " & RecordCount
                    Tra_Delete_Bttn.Visible = False
                    Print_Summary_Bttn.Visible = False
                    Print_Reminder_Bttn.Visible = False
                    Print_MostIssuedBooks_Bttn.Visible = False
                    Print_TopBorrowers_Bttn.Visible = True
                End If
                ViewState("dt") = dtSearch
            End If
        Catch s As Exception
            Lbl_Error.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'grid view page index changing event
    Protected Sub Grid2_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles Grid2.PageIndexChanging
        Try
            'rebind datagrid
            Grid2.DataSource = ViewState("dt") 'temp
            Grid2.PageIndex = e.NewPageIndex
            index = e.NewPageIndex * Grid2.PageSize
            Grid2.DataBind()
        Catch s As Exception
            Lbl_Error.Text = "Error:  there is error in page index !"
        End Try
    End Sub
    'gridview sorting event
    Protected Sub Grid2_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles Grid2.Sorting
        Dim temp As DataTable = CType(ViewState("dt"), DataTable)
        GridViewSortExpression = e.SortExpression
        Grid2.DataSource = temp
        Dim pageIndex As Integer = Grid2.PageIndex
        Grid2.DataSource = SortDataTable(Grid2.DataSource, False)
        Grid2.DataBind()
        Grid2.PageIndex = pageIndex
    End Sub
    'grid view page index changing event
    Protected Sub Grid3_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles Grid3.PageIndexChanging
        Try
            'rebind datagrid
            Grid3.DataSource = ViewState("dt") 'temp
            Grid3.PageIndex = e.NewPageIndex
            index = e.NewPageIndex * Grid3.PageSize
            Grid3.DataBind()
        Catch s As Exception
            Lbl_Error.Text = "Error:  there is error in page index !"
        End Try
    End Sub
    'gridview sorting event
    Protected Sub Grid3_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles Grid3.Sorting
        Dim temp As DataTable = CType(ViewState("dt"), DataTable)
        GridViewSortExpression = e.SortExpression
        Grid3.DataSource = temp
        Dim pageIndex As Integer = Grid3.PageIndex
        Grid3.DataSource = SortDataTable(Grid3.DataSource, False)
        Grid3.DataBind()
        Grid3.PageIndex = pageIndex
    End Sub
    'load report
    Protected Sub Print_Summary_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Print_Summary_Bttn.Click
        Try
            Dim Reportdoc As New ReportDocument
            Reportdoc = Report_Load_Summary()

            If DDL_PrintFormats.Text <> "" Then
                If DDL_PrintFormats.SelectedValue = "PDF" Then
                    Reportdoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "Report_CirTransactions_Books_" + Format(DateTime.Now, "ddMMyyyy"))
                End If
                If DDL_PrintFormats.SelectedValue = "DOC" Then
                    Reportdoc.ExportToHttpResponse(ExportFormatType.WordForWindows, Response, True, "Report_CirTransactions_Books_" + Format(DateTime.Now, "ddMMyyyy"))
                End If
                If DDL_PrintFormats.SelectedValue = "EXCEL" Then
                    If RadioButton1.Checked = True Then
                        ExportTo_excel_Books()
                    Else
                        ExportTo_excel_LooseIssues()
                    End If
                End If
            Else
                Reportdoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "Report_CirTransactions_Books_" + Format(DateTime.Now, "ddMMyyyy"))
            End If
        Catch ex As Exception
            Label20.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    Public Function Report_Load_Summary() As ReportDocument
        Dim Rpt As New ReportDocument
        Dim dtLibrary As New DataTable
        dtLibrary = (ViewState("dt"))

        Rpt.Load(Server.MapPath("~/Reports/CircullationTr_Summary_Report.rpt"))
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
            myGroupName = Rpt.ReportDefinition.Sections("ReportHeaderSection1").ReportObjects.Item("Text2")
            myGroupName.Text = "Group By: " & DDL_GroupBy.SelectedItem().Text
        Else
            Rpt.ReportDefinition.Sections("ReportHeaderSection1").SectionFormat.EnableSuppress = True
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
    Public Sub ExportTo_excel_Books()

        Dim strFilename As String = "Export_CirTransactions_Books_" + Format(DateTime.Now, "ddMMyyyy") + ".xls" '"ExportToExcel.xls"
        Dim attachment As String = "attachment; filename=" & strFilename
        Response.ClearContent()
        Response.AddHeader("content-disposition", attachment)
        Response.ContentType = "application/vnd.ms-excel"

        Dim Flag As Object = Nothing
        Dim dt As DataTable = Nothing
        For Each row As GridViewRow In Grid1.Rows
            Dim CIR_ID As Integer = Convert.ToInt32(Grid1.DataKeys(row.RowIndex).Value)
            Dim SQL As String = Nothing
            SQL = "SELECT CIRCULATION_TRANSACTIONS_VIEW.CIR_ID,MEM_NAME, MEM_NO, Accession = CASE (isnull(accession_no,'')) WHen ''  THEN cast(copy_id as varchar(20)) ELSE accession_no END, Title = CASE (isnull(Title,'')) WHen ''  THEN journal ELSE title END, convert(char(10),ISSUE_DATE,103) as ISSUE_DATE,ISSUE_TIME,convert(char(10),DUE_DATE,103) as DUE_DATE,convert(char(10),RETURN_DATE,103) as RETURN_DATE,RETURN_TIME, FINE_DUE, FINE_COLLECTED, convert(char(10),RESERVE_DATE,103) as RESERVE_DATE, convert(char(10),RENEW_DATE,103) as RENEW_DATE, STATUS, VOL_NO, CIR_LIB_CODE, MEM_STATUS  FROM CIRCULATION_TRANSACTIONS_VIEW WHERE (CIRCULATION_TRANSACTIONS_VIEW.CIR_LIB_CODE = '" & Trim(LibCode) & "') AND (CIRCULATION_TRANSACTIONS_VIEW.CIR_ID ='" & Trim(CIR_ID) & "');"
            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            da.Fill(ds)
            dt = ds.Tables(0).Copy

            Dim tab As String = String.Empty
            If Flag = "" Then
                For Each dtcol As DataColumn In dt.Columns
                    Response.Write(tab + dtcol.ColumnName)
                    tab = vbTab
                Next
                Flag = "Yes"
            End If

            Response.Write(vbLf)
            For Each dr As DataRow In dt.Rows
                tab = ""
                For j As Integer = 0 To dt.Columns.Count - 1
                    Response.Write(tab & Convert.ToString(dr(j)))
                    tab = vbTab
                Next
            Next
        Next
        Response.Flush()
        Response.End()
    End Sub
    Public Sub ExportTo_excel_LooseIssues()

        Dim strFilename As String = "Export_CirTransactions_LooseIssues_" + Format(DateTime.Now, "ddMMyyyy") + ".xls" '"ExportToExcel.xls"
        Dim attachment As String = "attachment; filename=" & strFilename
        Response.ClearContent()
        Response.AddHeader("content-disposition", attachment)
        Response.ContentType = "application/vnd.ms-excel"

        Dim Flag As Object = Nothing
        Dim dt As DataTable = Nothing
        For Each row As GridViewRow In Grid1.Rows
            Dim CIR_ID As Integer = Convert.ToInt32(Grid1.DataKeys(row.RowIndex).Value)
            Dim SQL As String = Nothing
            SQL = "SELECT CIRCULATION_TRANSACTIONS_VIEW.CIR_ID,MEM_NAME, MEM_NO, Accession = CASE (isnull(accession_no,'')) WHen ''  THEN cast(copy_id as varchar(20)) ELSE accession_no END, Title = CASE (isnull(Title,'')) WHen ''  THEN journal ELSE title END, VOL_NO2, ISSUE_NO, PART_NO, convert(char(10),ISS_DATE,103) as PUBLICATION_DATE, convert(char(10),ISSUE_DATE,103) as ISSUE_DATE,ISSUE_TIME,convert(char(10),DUE_DATE,103) as DUE_DATE,convert(char(10),RETURN_DATE,103) as RETURN_DATE,RETURN_TIME, FINE_DUE, FINE_COLLECTED, convert(char(10),RESERVE_DATE,103) as RESERVE_DATE, convert(char(10),RENEW_DATE,103) as RENEW_DATE, STATUS, VOL_NO, CIR_LIB_CODE, MEM_STATUS  FROM CIRCULATION_TRANSACTIONS_VIEW WHERE (CIRCULATION_TRANSACTIONS_VIEW.CIR_LIB_CODE = '" & Trim(LibCode) & "') AND (CIRCULATION_TRANSACTIONS_VIEW.CIR_ID ='" & Trim(CIR_ID) & "') ;"
            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            da.Fill(ds)
            dt = ds.Tables(0).Copy

            Dim tab As String = String.Empty
            If Flag = "" Then
                For Each dtcol As DataColumn In dt.Columns
                    Response.Write(tab + dtcol.ColumnName)
                    tab = vbTab
                Next
                Flag = "Yes"
            End If

            Response.Write(vbLf)
            For Each dr As DataRow In dt.Rows
                tab = ""
                For j As Integer = 0 To dt.Columns.Count - 1
                    Response.Write(tab & Convert.ToString(dr(j)))
                    tab = vbTab
                Next
            Next
        Next
        Response.Flush()
        Response.End()
    End Sub
    'print Reminder for Results to Grid1
    Protected Sub Print_Reminder_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Print_Reminder_Bttn.Click
        Try
            Dim Reportdoc As New ReportDocument
            Reportdoc = Report_Load_OvedueLetter()

            If DDL_PrintFormats.Text <> "" Then
                If DDL_PrintFormats.SelectedValue = "PDF" Then
                    Reportdoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "Report_OVerdue_Notice_" + Format(DateTime.Now, "ddMMyyyy"))
                End If
                If DDL_PrintFormats.SelectedValue = "DOC" Then
                    Reportdoc.ExportToHttpResponse(ExportFormatType.WordForWindows, Response, True, "Report_OVerdue _Notice_" + Format(DateTime.Now, "ddMMyyyy"))
                End If
            Else
                Reportdoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "Report_OVerdue_Notice_" + Format(DateTime.Now, "ddMMyyyy"))
            End If
        Catch ex As Exception
            Lbl_Error.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    'print approval form
    Public Function Report_Load_OvedueLetter() As ReportDocument
        Dim dtMess As DataTable = Nothing
        Dim dt As DataTable = Nothing
        Dim Rpt As New ReportDocument
        Try
            Dim MESS_ID As Integer = Nothing
            If DDL_Letters.Text <> "" Then
                MESS_ID = DDL_Letters.SelectedValue
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Plz Select Letter Template from Drop-Down!');", True)
                DDL_Letters.Focus()
                Exit Function
            End If

            'search form elements
            Dim ds As New DataSet
            Dim SQL As String = Nothing
            SQL = "SELECT * FROM MESSAGES WHERE (LIB_CODE = '" & Trim(LibCode) & "') AND (MESS_ID = '" & Trim(MESS_ID) & "') ;"

            Dim da As New SqlDataAdapter(SQL, SqlConn)
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            da.Fill(ds)

            dtMess = ds.Tables(0).Copy

            Dim myFileNo As Object = Nothing
            Dim mySubject As Object = Nothing
            Dim myTopMessage As Object = Nothing
            Dim myBottomMessage As Object = Nothing
            Dim mySignAuthority As Object = Nothing
            Dim mySalutation As Object = Nothing

            If dtMess.Rows.Count <> 0 Then
                If dtMess.Rows(0).Item("FILE_NO").ToString <> "" Then
                    myFileNo = Trim(dtMess.Rows(0).Item("FILE_NO").ToString)
                Else
                    myFileNo = ""
                End If

                If dtMess.Rows(0).Item("SUBJECT").ToString <> "" Then
                    mySubject = Trim(dtMess.Rows(0).Item("SUBJECT").ToString)
                Else
                    mySubject = ""
                End If

                If dtMess.Rows(0).Item("TOP_MESSAGE").ToString <> "" Then
                    myTopMessage = Trim(dtMess.Rows(0).Item("TOP_MESSAGE").ToString)
                Else
                    myTopMessage = ""
                End If

                If dtMess.Rows(0).Item("BOTTOM_MESSAGE").ToString <> "" Then
                    myBottomMessage = Trim(dtMess.Rows(0).Item("BOTTOM_MESSAGE").ToString)
                Else
                    myBottomMessage = ""
                End If

                If dtMess.Rows(0).Item("SIGN_AUTHORITY").ToString <> "" Then
                    mySignAuthority = Trim(dtMess.Rows(0).Item("SIGN_AUTHORITY").ToString)
                Else
                    mySignAuthority = ""
                End If
                If dtMess.Rows(0).Item("SALUTATION").ToString <> "" Then
                    mySalutation = Trim(dtMess.Rows(0).Item("SALUTATION").ToString)
                Else
                    mySalutation = ""
                End If
            Else
                myFileNo = Nothing
                mySubject = Nothing
                myTopMessage = Nothing
                myBottomMessage = Nothing
                mySignAuthority = Nothing
                mySalutation = Nothing
            End If

            dt = ViewState("dt")
            If dt.Rows.Count <> 0 Then

                Rpt.Load(Server.MapPath("~/Reports/Overdue_allMembers.rpt"))
                Rpt.SetDataSource(dt)
                Rpt.Refresh()

                Dim FileNoText As CrystalReports.Engine.TextObject
                If myFileNo <> "" Then
                    FileNoText = Rpt.ReportDefinition.Sections("GroupHeaderSection1").ReportObjects.Item("Text6")
                    FileNoText.Text = myFileNo
                Else
                    'Rpt.ReportDefinition.Sections("ReportHeaderSection2").SectionFormat.EnableSuppress = True
                End If

                Dim SubjectText As CrystalReports.Engine.TextObject
                If mySubject <> "" Then
                    SubjectText = Rpt.ReportDefinition.Sections("GroupHeaderSection3").ReportObjects.Item("Text1")
                    SubjectText.Text = mySubject
                Else
                    'Rpt.ReportDefinition.Sections("ReportHeaderSection4").SectionFormat.EnableSuppress = True
                End If

                Dim SalutationText As CrystalReports.Engine.TextObject
                If mySalutation <> "" Then
                    SalutationText = Rpt.ReportDefinition.Sections("GroupHeaderSection4").ReportObjects.Item("Text7")
                    SalutationText.Text = mySalutation
                End If

                Dim TopMessageText As CrystalReports.Engine.TextObject
                If myTopMessage <> "" Then
                    TopMessageText = Rpt.ReportDefinition.Sections("GroupHeaderSection4").ReportObjects.Item("Text2")
                    TopMessageText.Text = myTopMessage
                Else
                    'Rpt.ReportDefinition.Sections("ReportHeaderSection3").SectionFormat.EnableSuppress = True
                End If

                Dim BottomMessageText As CrystalReports.Engine.TextObject
                If myBottomMessage <> "" Then
                    BottomMessageText = Rpt.ReportDefinition.Sections("GroupFooterSection1").ReportObjects.Item("Text3")
                    BottomMessageText.Text = myBottomMessage
                Else
                    'Rpt.ReportDefinition.Sections("ReportFooterSection1").SectionFormat.EnableSuppress = True
                End If

                Dim SignAuthorityText As CrystalReports.Engine.TextObject
                If mySignAuthority <> "" Then
                    SignAuthorityText = Rpt.ReportDefinition.Sections("GroupFooterSection2").ReportObjects.Item("Text4")
                    SignAuthorityText.Text = mySignAuthority
                Else
                    'Rpt.ReportDefinition.Sections("ReportFooterSection1").SectionFormat.EnableSuppress = True
                End If
                Rpt.SummaryInfo.ReportAuthor = RKLibraryParent
                Rpt.SummaryInfo.ReportComments = RKLibraryAddress
                Rpt.SummaryInfo.ReportTitle = RKLibraryName

                Response.Buffer = False
                Response.ClearContent()
                Response.ClearHeaders()
                dt.Dispose()
                Return Rpt
            Else
                MsgBox("No  Record(s) exist(s)..")
            End If

        Catch s As Exception
            Label20.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Function
    'print most issued books
    Protected Sub Print_MostIssuedBooks_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Print_MostIssuedBooks_Bttn.Click
        Try
            Dim Reportdoc As New ReportDocument
            Reportdoc = Report_Load_MostIssuedBooks()

            If DDL_PrintFormats.Text <> "" Then
                If DDL_PrintFormats.SelectedValue = "PDF" Then
                    Reportdoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "Report_MostIssued_Report_" + Format(DateTime.Now, "ddMMyyyy"))
                End If
                If DDL_PrintFormats.SelectedValue = "DOC" Then
                    Reportdoc.ExportToHttpResponse(ExportFormatType.WordForWindows, Response, True, "Report_MostIssue_Report_" + Format(DateTime.Now, "ddMMyyyy"))
                End If
            Else
                Reportdoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "Report_MostIssued_Report_" + Format(DateTime.Now, "ddMMyyyy"))
            End If
        Catch ex As Exception
            Lbl_Error.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    'print approval form
    Public Function Report_Load_MostIssuedBooks() As ReportDocument
        Dim dt As DataTable = Nothing
        Dim Rpt As New ReportDocument
        Try
            dt = ViewState("dt")
            If dt.Rows.Count <> 0 Then
                Rpt.Load(Server.MapPath("~/Reports/Most_Issued_Books_Report.rpt"))
                Rpt.SetDataSource(dt)
                Rpt.Refresh()
                Rpt.SummaryInfo.ReportAuthor = RKLibraryParent
                Rpt.SummaryInfo.ReportComments = RKLibraryAddress
                Rpt.SummaryInfo.ReportTitle = RKLibraryName

                Response.Buffer = False
                Response.ClearContent()
                Response.ClearHeaders()
                dt.Dispose()
                Return Rpt
            Else
                MsgBox("No  Record(s) exist(s)..")
            End If

        Catch s As Exception
            Lbl_Error.Text = "Error: " & (s.Message())
        Finally

        End Try
    End Function
    'print top borrowers
    Protected Sub Print_TopBorrowers_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Print_TopBorrowers_Bttn.Click
        Try
            Dim Reportdoc As New ReportDocument
            Reportdoc = Report_Load_TopBorrowers()

            If DDL_PrintFormats.Text <> "" Then
                If DDL_PrintFormats.SelectedValue = "PDF" Then
                    Reportdoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "Report_TopBorrowers_Report_" + Format(DateTime.Now, "ddMMyyyy"))
                End If
                If DDL_PrintFormats.SelectedValue = "DOC" Then
                    Reportdoc.ExportToHttpResponse(ExportFormatType.WordForWindows, Response, True, "Report_TopBorrowers_Report_" + Format(DateTime.Now, "ddMMyyyy"))
                End If
            Else
                Reportdoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "Report_TopBorrowers_Report_" + Format(DateTime.Now, "ddMMyyyy"))
            End If
        Catch ex As Exception
            Lbl_Error.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    'print top borrowers form
    Public Function Report_Load_TopBorrowers() As ReportDocument
        Dim dt As DataTable = Nothing
        Dim Rpt As New ReportDocument
        Try
            dt = ViewState("dt")
            If dt.Rows.Count <> 0 Then
                Rpt.Load(Server.MapPath("~/Reports/Top_Borrowers_Report.rpt"))
                Rpt.SetDataSource(dt)
                Rpt.Refresh()
                Rpt.SummaryInfo.ReportAuthor = RKLibraryParent
                Rpt.SummaryInfo.ReportComments = RKLibraryAddress
                Rpt.SummaryInfo.ReportTitle = RKLibraryName

                Response.Buffer = False
                Response.ClearContent()
                Response.ClearHeaders()
                dt.Dispose()
                Return Rpt
            Else
                MsgBox("No  Record(s) exist(s)..")
            End If

        Catch s As Exception
            Lbl_Error.Text = "Error: " & (s.Message())
        Finally

        End Try
    End Function
End Class