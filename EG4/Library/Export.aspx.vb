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
Imports EG4.PopulateCombo
Imports EG4.Report_From
Imports System.Drawing
Imports Microsoft.Office.Interop.Excel

Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports.Engine.TextObject
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Shared
Imports System.Drawing.Printing
Imports CrystalDecisions

Public Class Export
    Inherits System.Web.UI.Page
    Dim index As Integer = 0
    Public Srno As Integer = 0
    Dim myStartSN As Long = Nothing
    Dim myEndSN As Long = Nothing
    Dim writer As XmlTextWriter
    Dim sb As New StringBuilder()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session.Item("IsThisU") <> Nothing Then
                If Not SConnection() = True Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Database Connection is Lost!');", True)
                Else
                    LibCode = Session.Item("LoggedLibcode")
                    If Page.IsPostBack = False Then
                        PopulateAcqModes()
                        PopulateCurrentStatus()
                        PopulateStatus()
                        PopulateSections()
                        PopulateLocation()
                        PopulateClassNo()
                        RadioButton6.Checked = True
                    End If
                End If
                Dim CreateSUserButton As Object 'System.Web.UI.WebControls.Button
                CreateSUserButton = Master.FindControl("Accordion1").FindControl("LibAdminPane").FindControl("Lib_Export_Bttn")
                CreateSUserButton.ForeColor = Drawing.Color.Red
                myPaneName = "LibAdminPane" ' paneSelectedIndex = 1
            Else
                Response.Redirect("~/Default.aspx", False)
            End If
            LibCode = Session.Item("LoggedLibcode")
        Catch ex As Exception
            Lbl_Error.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    Public Function removeascii(ByVal mydata)

        Dim z, y
        Dim flag As Integer
        flag = 1

        While (flag = 1)
            If Len(mydata) <> 0 Then
                z = mydata.Substring(mydata.Length - 1)
                y = Asc(z)

                If y.ToString = "32" Or y.ToString = "10" Or y.ToString = "13" Then
                    mydata = mydata.Substring(0, mydata.Length - 1)
                Else
                    flag = 0
                End If

            Else
                flag = 0
            End If
        End While
        If Trim(mydata) <> "" Then
            mydata = Replace(mydata, Chr(10), "")
            mydata = Replace(mydata, Chr(13), "")
        End If
        Return mydata
    End Function
    Public Sub PopulateAcqModes()
        Me.DDL_AcqModes.DataTextField = "ACQMODE_NAME"
        Me.DDL_AcqModes.DataValueField = "ACQMODE_CODE"
        Me.DDL_AcqModes.DataSource = GetAcqModesList()
        Me.DDL_AcqModes.DataBind()
    End Sub
    Public Sub PopulateCurrentStatus()
        DDL_Status.DataTextField = "STA_NAME"
        DDL_Status.DataValueField = "STA_CODE"
        DDL_Status.DataSource = GetCopyStatusList()
        DDL_Status.DataBind()
    End Sub
    Public Sub PopulateStatus()
        DDL_Status.DataTextField = "STA_NAME"
        DDL_Status.DataValueField = "STA_CODE"
        DDL_Status.DataSource = GetCopyStatusList()
        DDL_Status.DataBind()
    End Sub
    'populate section
    Public Sub PopulateSections()
        Dim Command As SqlCommand = Nothing
        Dim dt As DataTable = Nothing
        Try
            Command = New SqlCommand("SELECT  SEC_CODE, SEC_NAME FROM SECTIONS WHERE (LIB_CODE = '" & Trim(LibCode) & "') ORDER BY SEC_NAME ", SqlConn)
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            Dim da As New SqlDataAdapter(Command)
            Dim ds As New DataSet
            Dim RecordCount As Long = 0
            da.Fill(ds)

            dt = ds.Tables(0).Copy

            Dim Dr As DataRow
            Dr = dt.NewRow
            Dr("SEC_CODE") = ""
            Dr("SEC_NAME") = ""
            If dt.Rows.Count = 0 Then
                Me.DDL_Section1.DataSource = Nothing
            Else
                Me.DDL_Section1.DataSource = dt
                Me.DDL_Section1.DataTextField = "SEC_NAME"
                Me.DDL_Section1.DataValueField = "SEC_CODE"
                Me.DDL_Section1.DataBind()
            End If
            SqlConn.Close()
        Catch s As Exception
            Lbl_Error.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'populate location
    Public Sub PopulateLocation()
        Dim Command As SqlCommand = Nothing
        Dim dt As DataTable = Nothing
        Try
            Command = New SqlCommand("select distinct PHYSICAL_LOCATION from holdings WHERE (PHYSICAL_LOCATION<>'Null') AND (LIB_CODE = '" & Trim(LibCode) & "') ORDER BY PHYSICAL_LOCATION ", SqlConn)
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            Dim da As New SqlDataAdapter(Command)
            Dim ds As New DataSet
            Dim RecordCount As Long = 0
            da.Fill(ds)

            dt = ds.Tables(0).Copy

            Dim Dr As DataRow
            Dr = dt.NewRow
            Dr("PHYSICAL_LOCATION") = ""
            Dr("PHYSICAL_LOCATION") = ""

            If dt.Rows.Count = 0 Then
                Me.DDL_Location.DataSource = Nothing
            Else
                Me.DDL_Location.DataSource = dt
                Me.DDL_Location.DataTextField = "PHYSICAL_LOCATION"
                Me.DDL_Location.DataValueField = "PHYSICAL_LOCATION"
                Me.DDL_Location.DataBind()
            End If
            SqlConn.Close()
        Catch s As Exception
            Lbl_Error.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'populate Class No
    Public Sub PopulateClassNo()
        Dim Command As SqlCommand = Nothing
        Dim dt As DataTable = Nothing
        Try
            Command = New SqlCommand("select distinct CLASS_NO from holdings WHERE (CLASS_NO<>'Null') AND (LIB_CODE = '" & Trim(LibCode) & "') ORDER BY CLASS_NO ", SqlConn)
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            Dim da As New SqlDataAdapter(Command)
            Dim ds As New DataSet
            Dim RecordCount As Long = 0
            da.Fill(ds)

            dt = ds.Tables(0).Copy

            Dim Dr As DataRow
            Dr = dt.NewRow
            Dr("CLASS_NO") = ""
            Dr("CLASS_NO") = ""

            If dt.Rows.Count = 0 Then
                Me.DDL_ClassNo.DataSource = Nothing
            Else
                Me.DDL_ClassNo.DataSource = dt
                Me.DDL_ClassNo.DataTextField = "CLASS_NO"
                Me.DDL_ClassNo.DataValueField = "CLASS_NO"
                Me.DDL_ClassNo.DataBind()
            End If
            SqlConn.Close()
        Catch s As Exception
            Lbl_Error.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    Protected Sub RadioButton7_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles RadioButton7.CheckedChanged
        txt_Status_RandomAccession.Visible = True
        txt_Status_RandomAccession.Focus()

        txt_Status_AccessionFrom.Visible = False
        txt_Status_AccessionTo.Visible = False
        txt_Status_AccDateFrom.Visible = False
        txt_Status_AccDateTo.Visible = False
        DDL_AcqModes.Visible = False
        DDL_Section1.Visible = False
        DDL_ClassNo.Visible = False
        DDL_Status.Visible = False
        DDL_Location.Visible = False
    End Sub
    Protected Sub RadioButton5_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles RadioButton5.CheckedChanged
        txt_Status_AccessionFrom.Visible = True
        txt_Status_AccessionTo.Visible = True
        txt_Status_AccessionFrom.Focus()

        txt_Status_AccDateFrom.Visible = False
        txt_Status_AccDateTo.Visible = False
        DDL_AcqModes.Visible = False
        DDL_Section1.Visible = False
        DDL_ClassNo.Visible = False
        DDL_Status.Visible = False
        DDL_Location.Visible = False
        txt_Status_RandomAccession.Visible = False
    End Sub
    Protected Sub RadioButton8_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles RadioButton8.CheckedChanged
        txt_Status_AccDateFrom.Visible = True
        txt_Status_AccDateTo.Visible = True
        txt_Status_AccDateFrom.Focus()

        txt_Status_AccessionFrom.Visible = False
        txt_Status_AccessionTo.Visible = False
        DDL_AcqModes.Visible = False
        DDL_Section1.Visible = False
        DDL_ClassNo.Visible = False
        DDL_Status.Visible = False
        DDL_Location.Visible = False
        txt_Status_RandomAccession.Visible = False
    End Sub
    Protected Sub RadioButton1_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles RadioButton1.CheckedChanged
        DDL_AcqModes.Visible = True
        DDL_AcqModes.Focus()

        DDL_Section1.Visible = False
        DDL_ClassNo.Visible = False
        DDL_Status.Visible = False
        DDL_Location.Visible = False
        txt_Status_RandomAccession.Visible = False
        txt_Status_AccessionFrom.Visible = False
        txt_Status_AccessionTo.Visible = False
        txt_Status_AccDateFrom.Visible = False
        txt_Status_AccDateTo.Visible = False
    End Sub
    Protected Sub RadioButton9_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles RadioButton9.CheckedChanged
        DDL_Section1.Visible = True
        DDL_Section1.Focus()

        DDL_AcqModes.Visible = False
        DDL_ClassNo.Visible = False
        DDL_Status.Visible = False
        DDL_Location.Visible = False
        txt_Status_RandomAccession.Visible = False
        txt_Status_AccessionFrom.Visible = False
        txt_Status_AccessionTo.Visible = False
        txt_Status_AccDateFrom.Visible = False
        txt_Status_AccDateTo.Visible = False
    End Sub
    Protected Sub RadioButton11_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles RadioButton11.CheckedChanged
        DDL_ClassNo.Visible = True
        DDL_ClassNo.Focus()

        DDL_Section1.Visible = False
        DDL_AcqModes.Visible = False
        DDL_Status.Visible = False
        DDL_Location.Visible = False
        txt_Status_RandomAccession.Visible = False
        txt_Status_AccessionFrom.Visible = False
        txt_Status_AccessionTo.Visible = False
        txt_Status_AccDateFrom.Visible = False
        txt_Status_AccDateTo.Visible = False
    End Sub
    Protected Sub RadioButton2_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles RadioButton2.CheckedChanged
        DDL_Status.Visible = True
        DDL_Status.Focus()

        DDL_Location.Visible = False
        txt_Status_RandomAccession.Visible = False
        txt_Status_AccessionFrom.Visible = False
        txt_Status_AccessionTo.Visible = False
        txt_Status_AccDateFrom.Visible = False
        txt_Status_AccDateTo.Visible = False
        DDL_ClassNo.Visible = False
        DDL_Section1.Visible = False
        DDL_AcqModes.Visible = False
    End Sub
    Protected Sub RadioButton10_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles RadioButton10.CheckedChanged
        DDL_Location.Visible = True
        DDL_Location.Focus()

        DDL_Status.Visible = False
        txt_Status_RandomAccession.Visible = False
        txt_Status_AccessionFrom.Visible = False
        txt_Status_AccessionTo.Visible = False
        txt_Status_AccDateFrom.Visible = False
        txt_Status_AccDateTo.Visible = False
        DDL_ClassNo.Visible = False
        DDL_Section1.Visible = False
        DDL_AcqModes.Visible = False
    End Sub
    Protected Sub RadioButton6_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles RadioButton6.CheckedChanged
        DDL_Location.Visible = False
        DDL_Status.Visible = False
        txt_Status_RandomAccession.Visible = False
        txt_Status_AccessionFrom.Visible = False
        txt_Status_AccessionTo.Visible = False
        txt_Status_AccDateFrom.Visible = False
        txt_Status_AccDateTo.Visible = False
        DDL_ClassNo.Visible = False
        DDL_Section1.Visible = False
        DDL_AcqModes.Visible = False
    End Sub
    'search records
    Protected Sub Search_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Search_Bttn.Click
        Dim dtSearch As DataTable = Nothing
        Try
            Dim myMaterial As Object = Nothing
            If RB_Monographs.Checked = True Then
                myMaterial = "M"
            Else
                myMaterial = "S"
            End If
            myMaterial = RemoveQuotes(myMaterial)

            Dim myAccNoFrom As Object = Nothing
            Dim myAccNoTo As Object = Nothing
            If RadioButton5.Checked = True Then 'acc no range
                If (txt_Status_AccessionFrom.Text <> "" And txt_Status_AccessionTo.Text = "") Or (txt_Status_AccessionFrom.Text = "" And txt_Status_AccessionTo.Text <> "") Or (txt_Status_AccessionFrom.Text = "" And txt_Status_AccessionTo.Text = "") Then
                    Lbl_Error.Text = "Plz enter both the value"
                    txt_Status_AccessionFrom.Focus()
                    Exit Sub
                Else
                    myAccNoFrom = TrimX(txt_Status_AccessionFrom.Text)
                    myAccNoFrom = RemoveQuotes(myAccNoFrom)
                    GetAccFromRowNumber(myAccNoFrom)

                    myAccNoTo = TrimX(txt_Status_AccessionTo.Text)
                    myAccNoTo = RemoveQuotes(myAccNoTo)
                    GetAccToRowNumber(myAccNoTo)
                End If
            End If

            'range of acc date
            Dim DateFrom As Object = Nothing
            Dim DateTo As Object = Nothing
            If RadioButton8.Checked = True Then
                If txt_Status_AccDateFrom.Text <> "" Then
                    DateFrom = TrimX(txt_Status_AccDateFrom.Text)
                    DateFrom = Convert.ToDateTime(DateFrom, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") 'convert from indian to us
                    DateFrom = RemoveQuotes(DateFrom)
                    If DateFrom.ToString.Length > 11 Then
                        Lbl_Error.Text = "Enter proper Date in dd/MM/yyyy format"
                        txt_Status_AccDateFrom.Focus()
                        Exit Sub
                    End If
                Else
                    DateFrom = Nothing
                End If

                If txt_Status_AccDateTo.Text <> "" Then
                    DateTo = TrimX(txt_Status_AccDateTo.Text)
                    DateTo = Convert.ToDateTime(DateTo, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") 'convert from indian to us
                    DateTo = RemoveQuotes(DateTo)
                    If DateTo.ToString.Length > 11 Then
                        Lbl_Error.Text = "Enter proper Date in dd/MM/yyyy format"
                        txt_Status_AccDateTo.Focus()
                        Exit Sub
                    End If
                Else
                    DateTo = Nothing
                End If
            End If

            'acq mode
            Dim ACQ_MODE As Object = Nothing
            If RadioButton1.Checked = True Then
                If DDL_AcqModes.Text <> "" Then
                    ACQ_MODE = DDL_AcqModes.SelectedValue
                    ACQ_MODE = RemoveQuotes(ACQ_MODE)
                End If
            End If

            'section wise
            Dim SEC_CODE As Object = Nothing
            If RadioButton9.Checked = True Then
                If DDL_Section1.Text <> "" Then
                    SEC_CODE = DDL_Section1.SelectedValue
                    SEC_CODE = RemoveQuotes(SEC_CODE)
                Else
                    SEC_CODE = Nothing
                End If
            End If

            'location
            Dim LOCATION As Object = Nothing
            If RadioButton10.Checked = True Then
                If DDL_Location.Text <> "" Then
                    LOCATION = DDL_Location.SelectedValue
                    LOCATION = RemoveQuotes(LOCATION)
                Else
                    LOCATION = Nothing
                End If
            End If

            'class no
            Dim CLASS_NO As Object = Nothing
            If RadioButton11.Checked = True Then
                If DDL_ClassNo.Text <> "" Then
                    CLASS_NO = DDL_ClassNo.SelectedValue
                    CLASS_NO = RemoveQuotes(CLASS_NO)
                Else
                    CLASS_NO = Nothing
                End If
            End If

            'current status
            Dim STATUS As Object = Nothing
            If RadioButton2.Checked = True Then
                If DDL_Status.Text <> "" Then
                    STATUS = DDL_Status.SelectedValue
                    STATUS = RemoveQuotes(STATUS)
                Else
                    STATUS = Nothing
                End If
            End If

            Dim SQL As String = Nothing
            'SQL = "SELECT HOLD_ID, CAT_NO, ACQ_ID, ACCESSION_NO, ACCESSION_DATE, VOL_NO, CLASS_NO, BOOK_NO, PAGINATION, PHYSICAL_LOCATION, STA_CODE, COLLECTION_TYPE, LIB_CODE, TITLE = CASE (ISNULL(SUB_TITLE, '')) WHEN '' THEN TITLE ELSE (TITLE + ': '+SUB_TITLE) END  FROM BOOKS_ACC_REGISTER_VIEW WHERE (LIB_CODE = '" & Trim(LibCode) & "') AND (BIB_CODE='" & Trim(myMaterial) & "') "
            SQL = "SELECT BOOKS_ACC_REGISTER_VIEW.*, BOOKSTATUS.STA_NAME FROM BOOKS_ACC_REGISTER_VIEW INNER JOIN BOOKSTATUS ON BOOKS_ACC_REGISTER_VIEW.STA_CODE = BOOKSTATUS.STA_CODE WHERE (BOOKS_ACC_REGISTER_VIEW.LIB_CODE = '" & Trim(LibCode) & "') AND (BOOKS_ACC_REGISTER_VIEW.BIB_CODE='" & Trim(myMaterial) & "')"
            'SQL = "SELECT * FROM BOOKS_ACC_REGISTER_VIEW WHERE (LIB_CODE = '" & Trim(LibCode) & "') AND (BIB_CODE='" & Trim(myMaterial) & "') "

            If RadioButton7.Checked = True Then ' random accession no
                If txt_Status_RandomAccession.Text <> "" Then
                    Dim Str1 As Object = Nothing
                    Str1 = Split(txt_Status_RandomAccession.Text, ";")
                    SQL = SQL & " AND (ACCESSION_NO = '" & TrimX(Str1(0)) & "'   "
                    Dim i As Integer = Nothing
                    For i = 1 To Str1.length - 1
                        SQL = SQL & "OR ACCESSION_NO = '" & TrimX(Str1(i)) & "' "
                    Next
                    SQL = SQL & ")"
                End If
            End If

            If RadioButton1.Checked = True Then
                SQL = SQL & " AND (ACQMODE_CODE = '" & Trim(ACQ_MODE) & "') "
            End If
            If RadioButton5.Checked = True Then 'Range of Acc No
                SQL = SQL & " AND ((ROWNUMBER) >= '" & Trim(myStartSN) & "' AND (ROWNUMBER) <= '" & Trim(myEndSN) & "') "
            End If

            If RadioButton8.Checked = True Then
                SQL = SQL & " AND ((ACCESSION_DATE) >= '" & Trim(DateFrom) & "' AND (ACCESSION_DATE) <= '" & Trim(DateTo) & "') "
            End If

            If RadioButton9.Checked = True Then
                SQL = SQL & " AND (SEC_CODE = '" & Trim(SEC_CODE) & "') "
            End If

            If RadioButton10.Checked = True Then
                SQL = SQL & " AND (PHYSICAL_LOCATION = '" & Trim(LOCATION) & "') "
            End If
            If RadioButton11.Checked = True Then
                SQL = SQL & " AND (CLASS_NO = '" & Trim(CLASS_NO) & "') "
            End If

            If RadioButton2.Checked = True Then
                SQL = SQL & " AND (STA_CODE = '" & Trim(STATUS) & "') "
            End If
            SQL = SQL & " ORDER BY case when left(accession_no,1) like '[A-Za-z]' then dbo.Sort_AlphaNumeric(Accession_no, '[A-Za-z]') end asc,   cast(dbo.Sort_AlphaNumeric(accession_no,'0-9') as float) asc"

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
                Label9.Text = "Total Record(s): 0 "
                Export_Bttn.Enabled = False
                Print_Compact_Bttn.Visible = False
                Print_Summary_Bttn.Visible = False
                Print_Detail_Bttn.Visible = False
            Else
                Grid1.Visible = True
                RecordCount = dtSearch.Rows.Count
                Grid1.DataSource = dtSearch
                Grid1.DataBind()
                Label9.Text = "Total Record(s): " & RecordCount
                Export_Bttn.Enabled = True
                Print_Compact_Bttn.Visible = True
                Print_Summary_Bttn.Visible = True
                Print_Detail_Bttn.Visible = True
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
    'get value of row from grid
    Private Sub Grid1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles Grid1.RowCommand
        Dim Command As SqlCommand = Nothing
        Dim drNOTICES As SqlDataReader = Nothing
        Try
            If e.CommandName = "Select" Then
                Dim myRowID, HOLD_ID As Integer
                myRowID = e.CommandArgument.ToString()
                HOLD_ID = Grid1.DataKeys(myRowID).Value

                If Not String.IsNullOrEmpty(HOLD_ID) And HOLD_ID <> 0 Then

                    Dim iloop As Integer
                    Dim strcurrentchar As Object
                    Dim c As Integer
                    Dim counter1 As Integer

                    HOLD_ID = TrimX(HOLD_ID)
                    HOLD_ID = UCase(HOLD_ID)
                    HOLD_ID = RemoveQuotes(HOLD_ID)
                    If Len(HOLD_ID).ToString > 10 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Length of Input is not Proper... ');", True)
                        Exit Sub
                    End If
                    HOLD_ID = " " & HOLD_ID & " "
                    If InStr(1, HOLD_ID, " CREATE ", 1) > 0 Or InStr(1, HOLD_ID, " DELETE ", 1) > 0 Or InStr(1, HOLD_ID, " DROP ", 1) > 0 Or InStr(1, HOLD_ID, " INSERT ", 1) > 1 Or InStr(1, HOLD_ID, " TRACK ", 1) > 1 Or InStr(1, HOLD_ID, " TRACE ", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do not use reserve words... ');", True)
                        Exit Sub
                    End If
                    HOLD_ID = TrimX(HOLD_ID)


                    Dim SQL As String = Nothing
                    SQL = "SELECT HOLDINGS_CATS_AUTHORS_VIEW.*, CATS.PHOTO  FROM HOLDINGS_CATS_AUTHORS_VIEW INNER JOIN CATS ON HOLDINGS_CATS_AUTHORS_VIEW.CAT_NO = CATS.CAT_NO WHERE (HOLDINGS_CATS_AUTHORS_VIEW.LIB_CODE ='" & Trim(LibCode) & "'  AND HOLDINGS_CATS_AUTHORS_VIEW.HOLD_ID ='" & Trim(HOLD_ID) & "')"

                    Dim dt As DataTable
                    Dim ds As New DataSet
                    Dim da As New SqlDataAdapter(SQL, SqlConn)
                    If SqlConn.State = 0 Then
                        SqlConn.Open()
                    End If
                    da.Fill(ds)

                    dt = ds.Tables(0).Copy

                    Dim myDetails As String = Nothing
                    If dt.Rows.Count <> 0 Then
                        If dt.Rows(0).Item("ACCESSION_NO").ToString <> "" Then
                            Label517.Text = dt.Rows(0).Item("ACCESSION_NO").ToString
                        Else
                            Label517.Text = dt.Rows(0).Item("ACCESSION_NO").ToString
                        End If
                        If dt.Rows(0).Item("SUB_TITLE").ToString <> "" Then
                            Label518.Text = dt.Rows(0).Item("TITLE").ToString & ": " & dt.Rows(0).Item("SUB_TITLE").ToString
                        Else
                            Label518.Text = dt.Rows(0).Item("TITLE").ToString
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

                        If dt.Rows(0).Item("EDITOR").ToString <> "" Then
                            myDetails = myDetails & " /Edited By " & dt.Rows(0).Item("EDITOR").ToString
                        End If
                        If dt.Rows(0).Item("EDITION").ToString <> "" Then
                            Dim myEdition As Object = Nothing
                            myEdition = dt.Rows(0).Item("EDITION").ToString

                            If InStr(myEdition, "edition") = 0 Or InStr(myEdition, "ed") = 0 Or InStr(myEdition, "ed.") = 0 Or InStr(myEdition, "edition.") = 0 Then
                                myDetails = myDetails & ". " & dt.Rows(0).Item("EDITION").ToString & " ed. "
                            Else
                                myDetails = myDetails & " . " & dt.Rows(0).Item("EDITION").ToString
                            End If
                        End If
                        If dt.Rows(0).Item("PUB_PLACE").ToString <> "" Then
                            myDetails = myDetails & ", " & dt.Rows(0).Item("PUB_PLACE").ToString
                        End If
                        If dt.Rows(0).Item("PUB_NAME").ToString <> "" Then
                            myDetails = myDetails & ", " & dt.Rows(0).Item("PUB_NAME").ToString
                        End If
                        If dt.Rows(0).Item("YEAR_OF_PUB").ToString <> "" Then
                            myDetails = myDetails & ", " & dt.Rows(0).Item("YEAR_OF_PUB").ToString
                        End If
                        If dt.Rows(0).Item("VOL_NO").ToString <> "" Then
                            myDetails = myDetails & ". Vol No.: " & dt.Rows(0).Item("VOL_NO").ToString
                        End If
                        If dt.Rows(0).Item("PAGINATION").ToString <> "" Then
                            myDetails = myDetails & ", " & dt.Rows(0).Item("PAGINATION").ToString
                        End If
                        Label519.Text = myDetails
                    Else
                        Label517.Text = ""
                        Label518.Text = ""
                        Label519.Text = ""
                    End If
                Else
                    Label517.Text = ""
                    Label518.Text = ""
                    Label519.Text = ""
                End If
            End If
        Catch s As Exception
            Lbl_Error.Text = "Error: " & (s.Message())
        Finally

        End Try
    End Sub 'Grid1_ItemCommand
    'get RowNuber for range of accno
    Public Sub GetAccFromRowNumber(ByVal AccFrom As Object)
        Dim dtAcc As DataTable = Nothing
        Try
            'check duplicate acc no
            Dim str2 As Object = Nothing
            Dim flag2 As Long = Nothing
            str2 = "SELECT ROWNUMBER FROM BOOKS_ACC_REGISTER_VIEW WHERE (ACCESSION_NO = '" & Trim(AccFrom) & "')  AND (LIB_CODE = '" & Trim(LibCode) & "')"
            Dim cmd2 As New SqlCommand(str2, SqlConn)
            SqlConn.Open()
            flag2 = cmd2.ExecuteScalar
            SqlConn.Close()

            If flag2 = Nothing Then
                myStartSN = Nothing
            Else
                myStartSN = Convert.ToInt16(flag2)
            End If
        Catch ex As Exception
            Label6.Text = ex.ToString
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'get RowNuber for range of accno
    Public Sub GetAccToRowNumber(ByVal AccTo As Object)
        Dim dtAcc As DataTable = Nothing
        Try
            'check duplicate acc no
            Dim str2 As Object = Nothing
            Dim flag2 As Long = Nothing
            str2 = "SELECT ROWNUMBER FROM BOOKS_ACC_REGISTER_VIEW WHERE (ACCESSION_NO = '" & Trim(AccTo) & "')  AND (LIB_CODE = '" & Trim(LibCode) & "')"
            Dim cmd2 As New SqlCommand(str2, SqlConn)
            SqlConn.Open()
            flag2 = cmd2.ExecuteScalar
            SqlConn.Close()

            If flag2 = Nothing Then
                myEndSN = Nothing
            Else
                myEndSN = Convert.ToInt16(flag2)
            End If
        Catch ex As Exception
            Lbl_Error.Text = ex.ToString
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'export 
    Protected Sub Export_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Export_Bttn.Click
        If DDL_Formats.Text <> "" Then
            If DDL_Formats.SelectedValue = "CSV" Then
                Response.Clear()
                Response.Buffer = True
                Dim FileName As String = "Export_" + Format(DateTime.Now, "ddMMyyyy") + ".txt"
                Response.AddHeader("content-disposition", String.Format("attachment;filename={0}", FileName))
                Response.Charset = ""
                Response.ContentType = "application/text "

                ExportToCSV()

                Response.Output.Write(sb.ToString())
                Response.Flush()
                Response.End()
                sb.Clear()
            End If
            If DDL_Formats.SelectedValue = "MARC21D" Then
                If RB_Monographs.Checked = True Then

                    Response.Clear()
                    Response.Buffer = True
                    Dim FileName As String = "Export_MARC21DF_Books_" + Format(DateTime.Now, "ddMMyyyy") + ".mrc"
                    Response.AddHeader("content-disposition", String.Format("attachment;filename={0}", FileName))
                    Response.Charset = ""
                    Response.ContentType = "application/text "

                    ExportToMARC21DisplaryFormatBooks()

                    Response.Output.Write(sb.ToString())
                    Response.Flush()
                    Response.End()
                    sb.Clear()
                Else
                    Response.Clear()
                    Response.Buffer = True
                    Dim FileName As String = "Export_MARC21DF_Serials_" + Format(DateTime.Now, "ddMMyyyy") + ".mrc"
                    Response.AddHeader("content-disposition", String.Format("attachment;filename={0}", FileName))
                    Response.Charset = ""
                    Response.ContentType = "application/text "

                    ExportToMARC21DisplaryFormatSerieals()

                    Response.Output.Write(sb.ToString())
                    Response.Flush()
                    Response.End()
                    sb.Clear()
                End If
            End If

            If DDL_Formats.SelectedValue = "MARC21C" Then
                If RB_Monographs.Checked = True Then
                    ConverToMARCCCF_Books()
                Else
                    ConverToMARCCCF_Serials()
                End If
            End If

            If DDL_Formats.SelectedValue = "MARCXML" Then
                If RB_Monographs.Checked = True Then
                    ConverToXML_Books()
                Else
                    ConverToXML_Books()
                End If
            End If
            If DDL_Formats.SelectedValue = "ISI2709" Then
                If RB_Monographs.Checked = True Then
                    ConvertToISO2709_Books()
                Else
                    ConvertToISO2709_Serials()
                End If
            End If
            If DDL_Formats.SelectedValue = "EXCEL" Then
                If RB_Monographs.Checked = True Then
                    ExportTo_excel_Books()
                Else
                    ExportTo_excel_Serials()
                End If
            End If
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Plz Select Format!');", True)
            DDL_Formats.Focus()
            Exit Sub
        End If

    End Sub
    Public Sub ConverToXML_Books()
        Dim dt As DataTable = Nothing
        Dim Line As String = Nothing
        Try
            Response.AppendHeader("content-disposition", "attachment; filename=ExportToXML_Books.xml")
            Response.ContentType = "text/xml"

            writer = New XmlTextWriter(Response.OutputStream, System.Text.Encoding.UTF8)
            writer.WriteStartDocument()
            writer.Formatting = Formatting.Indented
            writer.Indentation = 2
            writer.WriteStartElement("marc", "collection", "http://www.loc.gov/MARC21/slim")

            For Each row As GridViewRow In Grid1.Rows
                Dim cb As CheckBox = DirectCast(row.FindControl("cbd"), CheckBox)
                If cb IsNot Nothing AndAlso cb.Checked = True Then
                    Dim HOLD_ID As Integer = Convert.ToInt32(Grid1.DataKeys(row.RowIndex).Value)
                    Dim SQL As String = Nothing
                    SQL = "SELECT * FROM BOOKS_ACC_REGISTER_VIEW WHERE (LIB_CODE = '" & Trim(LibCode) & "') AND (HOLD_ID ='" & Trim(HOLD_ID) & "');"
                    Dim ds As New DataSet
                    Dim da As New SqlDataAdapter(SQL, SqlConn)
                    If SqlConn.State = 0 Then
                        SqlConn.Open()
                    End If
                    da.Fill(ds)
                    dt = ds.Tables(0).Copy

                    'sb.Append(
                    If dt.Rows.Count <> 0 Then
                        writer.WriteStartElement("marc:record")
                        Line = ""
                        '000 leader, 24 chr, 00-23 
                        Line = Line & "00000" '00-04: record length will be calculated by system
                        Line = Line & "n" '05:Record Status: always 'n'=new
                        Line = Line & "a" '06:type of record:  a=lanuage materia
                        If dt.Rows(0).Item("BIB_CODE") = "M" Then ' 07: bib level
                            Line = Line & "m"
                        Else
                            Line = Line & "s"
                        End If
                        Line = Line & "\" ' 08: Type of control: blank(#) for not specified
                        Line = Line & "a" '09: Character coding scheme: a=UCS/UNICODE
                        Line = Line & "2" '10: indicator count: always 2
                        Line = Line & "2" '11: subfield code count: always 2
                        Line = Line & "00000" '12-16: base address of data:Calcualted by computer for each record
                        Line = Line & "7" '17: Encoding level: 7=minimal level
                        Line = Line & "a" '18: Descriptive cataloging form: a=AACR2
                        Line = Line & "\" '19: Multipart Resource Record level: #=not applicable
                        Line = Line & "4" '20: lenght of the-length-of-field position: always 4
                        Line = Line & "5" '21: Length of the starting character position: always 5
                        Line = Line & "0" '22: length of the implementation defined portion: always 0
                        Line = Line & "0" & vbCrLf '23: undefined: always 0

                        'write leader in xml
                        writer.WriteStartElement("marc:leader", "")
                        writer.WriteString(Line)
                        writer.WriteEndElement()
                        Line = ""

                        '001: Control Number , max 12 chr A/N Var12 (Cat No)
                        Line = TrimX(dt.Rows(0).Item("CAT_NO").ToString)

                        writer.WriteStartElement("marc:controlfield")
                        writer.WriteAttributeString("tag", "001")
                        writer.WriteString(Line)
                        writer.WriteEndElement()
                        Line = ""

                        '003: Control Number Identification : In-DelNIC : Alpha/Var 8 max chr
                        Line = Trim(LibCode)

                        writer.WriteStartElement("marc:controlfield")
                        writer.WriteAttributeString("tag", "003")
                        writer.WriteString(Line)
                        writer.WriteEndElement()
                        Line = ""

                        '005: Date and Time of the latest Transaction : 16chr long, yyyymmddhhuuss
                        If dt.Rows(0).Item("ACCESSION_DATE").ToString <> "" Then
                            Line = Format(dt.Rows(0).Item("ACCESSION_DATE"), "yyyyMMdd") + "000000.0" & vbCrLf
                        Else
                            Line = Format(Date.Today, "yyyyMMdd") + "000000.0" & vbCrLf
                        End If

                        If Line <> "" Then
                            writer.WriteStartElement("marc:controlfield")
                            writer.WriteAttributeString("tag", "005")
                            writer.WriteString(Line)
                            writer.WriteEndElement()
                        End If
                        Line = ""

                        '008 40 chr fixed length data, 00-39
                        '008/00-05 Date of creation in yyMMdd format
                        If dt.Rows(0).Item("ACCESSION_DATE").ToString <> "" Then
                            Line = Line & Format(dt.Rows(0).Item("ACCESSION_DATE"), "yyMMdd")
                        Else
                            Line = Line & Format(Today.Date, "yyMMdd")
                        End If
                        '008/06: 
                        Line = Line & "s" '06 type of dates s=1 single dates
                        '008/07-10 :Date 1
                        If dt.Rows(0).Item("YEAR_OF_PUB").ToString <> "" Then
                            Line = Line & Trim(dt.Rows(0).Item("YEAR_OF_PUB").ToString)
                        Else
                            Line = Line & Trim(Today.Year) '7-10 date 1
                        End If
                        '008/11-14 Date 2: blank in case s=single date
                        Line = Line & "\\\\" '11-14 date2 = blank

                        '008/15-17: MARC country Code, Country of Publication
                        Dim myConcode As String = Nothing
                        If dt.Rows(0).Item("CON_CODE").ToString <> "" Then
                            Select Case dt.Rows(0).Item("CON_CODE").ToString
                                Case "IND"
                                    myConcode = "ii\"
                                Case "AUS"
                                    myConcode = "at\"
                                Case "FRA"
                                    myConcode = "fr\"
                                Case "GBR"
                                    myConcode = "xxk"
                                Case "HKG"
                                    myConcode = "cc\"
                                Case "JPN"
                                    myConcode = "ja\"
                                Case "PAK"
                                    myConcode = "pk\"
                                Case "RUS"
                                    myConcode = "ru\"
                                Case "USA"
                                    myConcode = "xxu"
                                Case Else
                                    myConcode = "ii\"
                            End Select
                            Line = Line & myConcode
                        Else
                            Line = Line & "xx\" '15-17 con code
                        End If

                        '008/18-21 Illustration
                        Line = Line & "\\\\" 'ill 18-21
                        '008/22: Target Audience
                        Line = Line & "\" 'Audience 22
                        '008/23: Form of Item
                        Line = Line & "d" 'Audience 23 d=large print
                        '008/24-27: 
                        Line = Line & "\\\\" '24-27 nature of contents
                        '008/28: Govt publication
                        Line = Line & "\" '28 Govt Publication
                        '008/29: Conf Publication
                        If dt.Rows(0).Item("CONF_NAME").ToString <> "" Then
                            Line = Line & "1" 'yes
                        Else
                            Line = Line & "0" 'no
                        End If
                        '008/30: Festchrift
                        Line = Line & "0" '30 not a festchrift
                        '008/31: Index Y/N
                        Line = Line & "0" '31 index present
                        '008/32: undefined
                        Line = Line & "\" '32 undefined
                        '008/33: Literary Form
                        Line = Line & "u" '33 literary form no u=unknown
                        '008/34: Biography
                        Line = Line & "\" '34 biography no
                        '008/35-37 lang code
                        If dt.Rows(0).Item("LANG_CODE").ToString <> "" Then
                            Line = Line & LCase(dt.Rows(0).Item("LANG_CODE").ToString)
                        Else
                            Line = Line & "eng"
                        End If
                        '008/38: Modified Record
                        Line = Line & "\" '38 record not modified
                        '008/39: Cataloging source
                        Line = Line & "d" '39 cataloging agency u=unknown

                        If Line <> "" Then
                            writer.WriteStartElement("marc:controlfield")
                            writer.WriteAttributeString("tag", "008")
                            writer.WriteString(Line)
                            writer.WriteEndElement()
                        End If
                        Line = ""

                        '020 ISBN
                        Dim myISBN As Object
                        If dt.Rows(0).Item("BIB_CODE").ToString = "M" Then
                            If dt.Rows(0).Item("STANDARD_NO").ToString <> "" Then
                                myISBN = TrimX(Replace(TrimX(dt.Rows(0).Item("STANDARD_NO").ToString()), "-", ""))
                                If Len(myISBN) = 10 Or Len(myISBN) = 13 Then
                                    Line = Line & Replace(TrimX(dt.Rows(0).Item("STANDARD_NO").ToString()), "-", "")
                                    writer.WriteStartElement("marc:datafield")
                                    writer.WriteAttributeString("tag", "020")
                                    writer.WriteAttributeString("ind1", "")
                                    writer.WriteAttributeString("ind2", "")
                                    writer.WriteStartElement("marc:subfield")
                                    writer.WriteAttributeString("code", "a")
                                    writer.WriteString(Line)
                                    writer.WriteEndElement()
                                    writer.WriteEndElement()
                                Else
                                    Line = Line & Replace(TrimX(dt.Rows(0).Item("STANDARD_NO").ToString()), "-", "") & vbCrLf
                                    writer.WriteStartElement("marc:datafield")
                                    writer.WriteAttributeString("tag", "020")
                                    writer.WriteAttributeString("ind1", "")
                                    writer.WriteAttributeString("ind2", "")
                                    writer.WriteStartElement("marc:subfield")
                                    writer.WriteAttributeString("code", "z")
                                    writer.WriteString(Line)
                                    writer.WriteEndElement()
                                    writer.WriteEndElement()
                                End If
                            End If
                        End If
                        Line = ""

                        '040 cataloging agency
                        writer.WriteStartElement("marc:datafield")
                        writer.WriteAttributeString("tag", "040")
                        writer.WriteAttributeString("ind1", "")
                        writer.WriteAttributeString("ind2", "")
                        writer.WriteStartElement("marc:subfield")
                        writer.WriteAttributeString("code", "a")
                        writer.WriteString(LibCode)
                        writer.WriteEndElement()
                        writer.WriteStartElement("marc:subfield")
                        writer.WriteAttributeString("code", "c")
                        writer.WriteString(LibCode)
                        writer.WriteEndElement()
                        writer.WriteEndElement()
                        Line = ""

                        '080 UDC No
                        If RadioButton1.Checked = True Then 'for UDC
                            If dt.Rows(0).Item("CLASS_NO").ToString <> "" Then
                                writer.WriteStartElement("marc:datafield")
                                writer.WriteAttributeString("tag", "080")
                                writer.WriteAttributeString("ind1", "")
                                writer.WriteAttributeString("ind2", "")
                                writer.WriteStartElement("marc:subfield")
                                writer.WriteAttributeString("code", "a")
                                writer.WriteString(dt.Rows(0).Item("CLASS_NO").ToString)
                                writer.WriteEndElement()
                                If dt.Rows(0).Item("BOOK_NO").ToString <> "" Then
                                    writer.WriteStartElement("marc:subfield")
                                    writer.WriteAttributeString("code", "b")
                                    writer.WriteString(dt.Rows(0).Item("BOOK_NO").ToString)
                                    writer.WriteEndElement()
                                End If
                                writer.WriteEndElement()
                                Line = ""
                            End If
                        End If

                        '082 DDC No
                        If RadioButton2.Checked = True Then 'for DDC
                            If dt.Rows(0).Item("CLASS_NO").ToString <> "" Then
                                writer.WriteStartElement("marc:datafield")
                                writer.WriteAttributeString("tag", "082")
                                writer.WriteAttributeString("ind1", "")
                                writer.WriteAttributeString("ind2", "")
                                writer.WriteStartElement("marc:subfield")
                                writer.WriteAttributeString("code", "a")
                                writer.WriteString(dt.Rows(0).Item("CLASS_NO").ToString)
                                writer.WriteEndElement()
                                If dt.Rows(0).Item("BOOK_NO").ToString <> "" Then
                                    writer.WriteStartElement("marc:subfield")
                                    writer.WriteAttributeString("code", "b")
                                    writer.WriteString(dt.Rows(0).Item("BOOK_NO").ToString)
                                    writer.WriteEndElement()
                                End If
                                writer.WriteEndElement()
                                Line = ""
                            End If
                        End If

                        '088 Report Number Optional
                        If dt.Rows(0).Item("REPORT_NO").ToString <> "" Then
                            writer.WriteStartElement("marc:datafield")
                            writer.WriteAttributeString("tag", "088")
                            writer.WriteAttributeString("ind1", "")
                            writer.WriteAttributeString("ind2", "")
                            writer.WriteStartElement("marc:subfield")
                            writer.WriteAttributeString("code", "a")
                            writer.WriteString(Trim(dt.Rows(0).Item("REPORT_NO").ToString))
                            writer.WriteEndElement()
                            writer.WriteEndElement()
                        End If
                        Line = ""

                        '100 Personal Author for Main entry
                        Dim myAuth As Object = Nothing
                        If dt.Rows(0).Item("AUTHOR1").ToString <> "" Then
                            If InStr(dt.Rows(0).Item("AUTHOR1"), ",").ToString <> 0 Then
                                myAuth = TrimAll(Replace(dt.Rows(0).Item("AUTHOR1").ToString(), ",", ", "))
                                myAuth = TrimAll(Replace(myAuth, ".", " "))
                                myAuth = TrimAll(myAuth) & "."
                                writer.WriteStartElement("marc:datafield")
                                writer.WriteAttributeString("tag", "100")
                                writer.WriteAttributeString("ind1", "1")
                                writer.WriteAttributeString("ind2", "")
                                writer.WriteStartElement("marc:subfield")
                                writer.WriteAttributeString("code", "a")
                                writer.WriteString(myAuth)
                                writer.WriteEndElement()
                                writer.WriteEndElement()
                            Else
                                myAuth = TrimAll(Replace(dt.Rows(0).Item("AUTHOR1").ToString(), ".", " "))
                                myAuth = TrimAll(myAuth) & "."
                                writer.WriteStartElement("marc:datafield")
                                writer.WriteAttributeString("tag", "100")
                                writer.WriteAttributeString("ind1", "0")
                                writer.WriteAttributeString("ind2", "")
                                writer.WriteStartElement("marc:subfield")
                                writer.WriteAttributeString("code", "a")
                                writer.WriteString(myAuth)
                                writer.WriteEndElement()
                                writer.WriteEndElement()
                            End If
                        End If
                        Line = ""

                        '110 Corporate Author for Main entry
                        Dim CorpAuth As Object = Nothing
                        If dt.Rows(0).Item("AUTHOR1").ToString = "" And dt.Rows(0).Item("CORPORATE_AUTHOR").ToString <> "" Then
                            CorpAuth = TrimAll(Replace(dt.Rows(0).Item("CORPORATE_AUTHOR").ToString(), ".", " "))
                            CorpAuth = TrimAll(CorpAuth) & "."
                            writer.WriteStartElement("marc:datafield")
                            writer.WriteAttributeString("tag", "110")
                            writer.WriteAttributeString("ind1", "2")
                            writer.WriteAttributeString("ind2", "")
                            writer.WriteStartElement("marc:subfield")
                            writer.WriteAttributeString("code", "a")
                            writer.WriteString(CorpAuth)
                            writer.WriteEndElement()
                            writer.WriteEndElement()
                        End If

                        '111 Conference for Main entry
                        Dim ConfName As Object = Nothing
                        If dt.Rows(0).Item("AUTHOR1").ToString = "" And dt.Rows(0).Item("CORPORATE_AUTHOR").ToString = "" And dt.Rows(0).Item("CONF_NAME").ToString <> "" Then
                            ConfName = TrimAll(Replace(dt.Rows(0).Item("CONF_NAME").ToString(), ".", " "))
                            ConfName = TrimAll(ConfName) & "."
                            writer.WriteStartElement("marc:datafield")
                            writer.WriteAttributeString("tag", "111")
                            writer.WriteAttributeString("ind1", "2")
                            writer.WriteAttributeString("ind2", "")
                            writer.WriteStartElement("marc:subfield")
                            writer.WriteAttributeString("code", "a")
                            writer.WriteString(ConfName)
                            writer.WriteEndElement()
                            writer.WriteEndElement()
                        End If

                        '130 UNIFORM TITLE for Main entry
                        Dim OneThirty As Object = Nothing
                        Dim my130Title As Object = Nothing
                        If dt.Rows(0).Item("AUTHOR1").ToString = "" And dt.Rows(0).Item("CORPORATE_AUTHOR").ToString = "" And dt.Rows(0).Item("CONF_NAME").ToString = "" Then
                            my130Title = TrimAll(dt.Rows(0).Item("TITLE").ToString())
                            my130Title = TrimAll(Replace(my130Title, "a ", "", 1, 2))
                            my130Title = TrimAll(Replace(my130Title, "A ", "", 1, 2))
                            my130Title = TrimAll(Replace(my130Title, "an ", "", 1, 3))
                            my130Title = TrimAll(Replace(my130Title, "An ", "", 1, 3))
                            my130Title = TrimAll(Replace(my130Title, "AN ", "", 1, 3))
                            my130Title = TrimAll(Replace(my130Title, "aN ", "", 1, 3))
                            my130Title = TrimAll(Replace(my130Title, "The ", "", 1, 4))
                            my130Title = TrimAll(Replace(my130Title, "the ", "", 1, 4))
                            my130Title = TrimAll(Replace(my130Title, "THe ", "", 1, 4))
                            my130Title = TrimAll(Replace(my130Title, "THE ", "", 1, 4))
                            my130Title = TrimAll(Replace(my130Title, "tHe ", "", 1, 4))
                            my130Title = TrimAll(Replace(my130Title, "tHE ", "", 1, 4))
                            Dim myLang As Object = Nothing
                            If dt.Rows(0).Item("LANG_CODE").ToString <> "" Then
                                myLang = TrimX(dt.Rows(0).Item("LANG_CODE").ToString()) & "."
                            Else
                                myLang = "ENG."
                            End If
                            writer.WriteStartElement("marc:datafield")
                            writer.WriteAttributeString("tag", "130")
                            writer.WriteAttributeString("ind1", "0")
                            writer.WriteAttributeString("ind2", "")
                            writer.WriteStartElement("marc:subfield")
                            writer.WriteAttributeString("code", "a")
                            writer.WriteString(my130Title)
                            writer.WriteEndElement()
                            writer.WriteStartElement("marc:subfield")
                            writer.WriteAttributeString("code", "l")
                            writer.WriteString(myLang)
                            writer.WriteEndElement()
                            writer.WriteEndElement()
                            OneThirty = "YES"
                        Else
                            OneThirty = "NO"
                        End If
                        Line = ""

                        '245 Main Title
                        Dim mySplitAuthor1, SurName1, ForeName1, MyNewAuthor1 As Object
                        If dt.Rows(0).Item("AUTHOR1").ToString <> "" Then
                            If InStr(dt.Rows(0).Item("AUTHOR1"), ",") <> 0 Then
                                mySplitAuthor1 = TrimAll(dt.Rows(0).Item("AUTHOR1").ToString())
                                mySplitAuthor1 = Split(mySplitAuthor1, ",")
                                SurName1 = mySplitAuthor1(0)
                                ForeName1 = mySplitAuthor1(1)
                                MyNewAuthor1 = Trim(ForeName1) & " " & Trim(SurName1)
                            Else
                                MyNewAuthor1 = TrimAll(dt.Rows(0).Item("AUTHOR1").ToString())
                            End If
                        End If

                        Dim mySplitAuthor2, SurName2, ForeName2, MyNewAuthor2 As Object
                        If dt.Rows(0).Item("AUTHOR2").ToString <> "" Then
                            If InStr(dt.Rows(0).Item("AUTHOR2"), ",") <> 0 Then
                                mySplitAuthor2 = TrimAll(dt.Rows(0).Item("AUTHOR2").ToString())
                                mySplitAuthor2 = Split(mySplitAuthor2, ",")
                                SurName2 = mySplitAuthor2(0)
                                ForeName2 = mySplitAuthor2(1)
                                MyNewAuthor2 = Trim(ForeName2) & " " & Trim(SurName2)
                            Else
                                MyNewAuthor2 = TrimAll(dt.Rows(0).Item("AUTHOR2").ToString())
                            End If
                        End If

                        Dim mySplitAuthor3, SurName3, ForeName3, MyNewAuthor3 As Object
                        If dt.Rows(0).Item("AUTHOR3").ToString <> "" Then
                            If InStr(dt.Rows(0).Item("AUTHOR3"), ",") <> 0 Then
                                mySplitAuthor3 = TrimAll(dt.Rows(0).Item("AUTHOR3").ToString())
                                mySplitAuthor3 = Split(mySplitAuthor3, ",")
                                SurName3 = mySplitAuthor3(0)
                                ForeName3 = mySplitAuthor3(1)
                                MyNewAuthor3 = Trim(ForeName3) & " " & Trim(SurName3)
                            Else
                                MyNewAuthor3 = TrimAll(dt.Rows(0).Item("AUTHOR3").ToString())
                            End If
                        End If

                        Dim myRkm As Object = Nothing
                        Dim myTitle As String = Nothing
                        Dim mySubTitle As String = Nothing
                        myTitle = Replace(TrimAll(dt.Rows(0).Item("TITLE").ToString()), ":", ",")
                        myTitle = Replace(TrimAll(dt.Rows(0).Item("TITLE").ToString()), "...", "--")
                        myTitle = TrimAll(myTitle)
                        myTitle = removeascii(myTitle)
                        If dt.Rows(0).Item("AUTHOR1").ToString <> "" Or dt.Rows(0).Item("CORPORATE_AUTHOR").ToString <> "" Or dt.Rows(0).Item("CONF_NAME").ToString <> "" Or OneThirty = "YES" Then
                            Line = Line & "1" '  index the title as an added entry
                            ''count non-filing caharacters - omiited by RKM
                            If Len(myTitle) > 1 Then
                                If TrimAll(dt.Rows(0).Item("TITLE")).Substring(0, 2) = "a " Or TrimAll(dt.Rows(0).Item("TITLE")).Substring(0, 2) = "A " Then
                                    Line = Line & "2"
                                    myRkm = "YES"
                                End If
                            End If
                            If Len(myTitle) > 2 Then
                                If TrimAll(dt.Rows(0).Item("TITLE")).Substring(0, 3) = "an " Or TrimAll(dt.Rows(0).Item("TITLE")).Substring(0, 3) = "An " Then
                                    Line = Line & "3"
                                    myRkm = "YES"
                                End If
                            End If
                            If Len(myTitle) > 3 Then
                                If TrimAll(dt.Rows(0).Item("TITLE")).Substring(0, 4) = "The " Or TrimAll(dt.Rows(0).Item("TITLE")).Substring(0, 4) = "the " Or TrimAll(dt.Rows(0).Item("TITLE")).Substring(0, 4) = "THE " Then
                                    Line = Line & "4"
                                    myRkm = "YES"
                                End If
                            End If
                            If myRkm <> "YES" Then
                                Line = Line & "0"
                            End If
                            Line = Line & "$a" & myTitle
                            If dt.Rows(0).Item("SUB_TITLE").ToString <> "" Then
                                Line = Line & " :$b" & TrimAll(dt.Rows(0).Item("SUB_TITLE").ToString())
                                mySubTitle = TrimAll(dt.Rows(0).Item("SUB_TITLE").ToString())
                            End If
                            If dt.Rows(0).Item("AUTHOR1").ToString <> "" And dt.Rows(0).Item("AUTHOR2").ToString <> "" And dt.Rows(0).Item("AUTHOR3").ToString <> "" Then
                                Line = Line & " /$cBy " & Trim(MyNewAuthor1) & ", " & MyNewAuthor2 & " and " & MyNewAuthor3
                                If dt.Rows(0).Item("EDITOR").ToString <> "" Then
                                    Line = Line & "; Edited by " & dt.Rows(0).Item("EDITOR").ToString
                                End If
                                If dt.Rows(0).Item("TRANSLATOR").ToString <> "" Then
                                    Line = Line & "; Tr. by " & dt.Rows(0).Item("TRANSLATOR").ToString
                                End If
                                If dt.Rows(0).Item("ILLUSTRATOR").ToString <> "" Then
                                    Line = Line & "; Illus. by " & dt.Rows(0).Item("ILLUSTRATOR").ToString
                                End If
                            End If
                            If dt.Rows(0).Item("AUTHOR1").ToString <> "" And dt.Rows(0).Item("AUTHOR2").ToString <> "" And dt.Rows(0).Item("AUTHOR3").ToString = "" Then
                                Line = Line & " /$cBy " & Trim(MyNewAuthor1) & ", " & MyNewAuthor2
                                If dt.Rows(0).Item("EDITOR").ToString <> "" Then
                                    Line = Line & "; edited by " & dt.Rows(0).Item("EDITOR").ToString
                                End If
                                If dt.Rows(0).Item("TRANSLATOR").ToString <> "" Then
                                    Line = Line & "; Tr. by " & dt.Rows(0).Item("TRANSLATOR").ToString
                                End If
                                If dt.Rows(0).Item("ILLUSTRATOR").ToString <> "" Then
                                    Line = Line & "; Illus. by " & dt.Rows(0).Item("ILLUSTRATOR").ToString
                                End If
                            End If
                            If dt.Rows(0).Item("AUTHOR1").ToString <> "" And dt.Rows(0).Item("AUTHOR2").ToString = "" And dt.Rows(0).Item("AUTHOR3").ToString = "" Then
                                Line = Line & " /$cBy " & Trim(MyNewAuthor1)
                                If dt.Rows(0).Item("EDITOR").ToString <> "" Then
                                    Line = Line & "; edited by " & dt.Rows(0).Item("EDITOR").ToString
                                End If
                                If dt.Rows(0).Item("TRANSLATOR").ToString <> "" Then
                                    Line = Line & "; Tr. by " & dt.Rows(0).Item("TRANSLATOR").ToString
                                End If
                                If dt.Rows(0).Item("ILLUSTRATOR").ToString <> "" Then
                                    Line = Line & "; Illus. by " & dt.Rows(0).Item("ILLUSTRATOR").ToString
                                End If
                            End If
                            Line = Line & "." & vbCrLf
                        Else
                            Line = Line & "0"  ' do not Index title as an added entry
                            ''count non-filing caharacters - omiited by RKM
                            If TrimAll(dt.Rows(0).Item("TITLE")).Substring(0, 2) = "a " Or TrimAll(dt.Rows(0).Item("TITLE")).Substring(0, 2) = "A " Then
                                Line = Line & "2"
                                myRkm = "YES"
                            End If
                            If TrimAll(dt.Rows(0).Item("TITLE")).Substring(0, 3) = "an " Or TrimAll(dt.Rows(0).Item("TITLE")).Substring(0, 3) = "An " Then
                                Line = Line & "3"
                                myRkm = "YES"
                            End If
                            If TrimAll(dt.Rows(0).Item("TITLE")).Substring(0, 4) = "The " Or TrimAll(dt.Rows(0).Item("TITLE")).Substring(0, 4) = "the " Or TrimAll(dt.Rows(0).Item("TITLE")).Substring(0, 4) = "THE " Then
                                Line = Line & "4"
                                myRkm = "YES"
                            End If
                            If myRkm <> "YES" Then
                                Line = Line & "0"
                            End If
                            Line = Line & "$a" & myTitle
                            If dt.Rows(0).Item("SUB_TITLE").ToString <> "" Then
                                Line = Line & " :$b" & TrimAll(dt.Rows(0).Item("SUB_TITLE").ToString())
                                mySubTitle = TrimAll(dt.Rows(0).Item("SUB_TITLE").ToString())
                            End If
                            If dt.Rows(0).Item("AUTHOR1").ToString <> "" And dt.Rows(0).Item("AUTHOR2").ToString <> "" And dt.Rows(0).Item("AUTHOR3").ToString <> "" Then
                                Line = Line & " /$cBy " & Trim(MyNewAuthor1) & ", " & MyNewAuthor2 & " and " & MyNewAuthor3
                                If dt.Rows(0).Item("EDITOR").ToString <> "" Then
                                    Line = Line & "; Edited by " & dt.Rows(0).Item("EDITOR").ToString
                                End If
                                If dt.Rows(0).Item("TRANSLATOR").ToString <> "" Then
                                    Line = Line & "; Tr. by " & dt.Rows(0).Item("TRANSLATOR").ToString
                                End If
                                If dt.Rows(0).Item("ILLUSTRATOR").ToString <> "" Then
                                    Line = Line & "; Illus. by " & dt.Rows(0).Item("ILLUSTRATOR").ToString
                                End If
                            End If
                            If dt.Rows(0).Item("AUTHOR1").ToString <> "" And dt.Rows(0).Item("AUTHOR2").ToString <> "" And dt.Rows(0).Item("AUTHOR3").ToString = "" Then
                                Line = Line & " /$cBy " & Trim(MyNewAuthor1) & ", " & MyNewAuthor2
                                If dt.Rows(0).Item("EDITOR").ToString <> "" Then
                                    Line = Line & "; edited by " & dt.Rows(0).Item("EDITOR").ToString
                                End If
                                If dt.Rows(0).Item("TRANSLATOR").ToString <> "" Then
                                    Line = Line & "; Tr. by " & dt.Rows(0).Item("TRANSLATOR").ToString
                                End If
                                If dt.Rows(0).Item("ILLUSTRATOR").ToString <> "" Then
                                    Line = Line & "; Illus. by " & dt.Rows(0).Item("ILLUSTRATOR").ToString
                                End If
                            End If
                            If dt.Rows(0).Item("AUTHOR1").ToString <> "" And dt.Rows(0).Item("AUTHOR2").ToString = "" And dt.Rows(0).Item("AUTHOR3").ToString = "" Then
                                Line = Line & " /$cBy " & Trim(MyNewAuthor1)
                                If dt.Rows(0).Item("EDITOR").ToString <> "" Then
                                    Line = Line & "; edited by " & dt.Rows(0).Item("EDITOR").ToString
                                End If
                                If dt.Rows(0).Item("TRANSLATOR").ToString <> "" Then
                                    Line = Line & "; Tr. by " & dt.Rows(0).Item("TRANSLATOR").ToString
                                End If
                                If dt.Rows(0).Item("ILLUSTRATOR").ToString <> "" Then
                                    Line = Line & "; Illus. by " & dt.Rows(0).Item("ILLUSTRATOR").ToString
                                End If
                            End If
                            Line = Line & "." & vbCrLf
                        End If
                        Dim FI As String = Nothing
                        Dim SI As String = Nothing
                        FI = Line.Substring(0, 1)
                        SI = Line.Substring(1, 1)

                        'get $C
                        Dim myResp As Object = Nothing
                        Dim myNewResp As Object = Nothing
                        If InStr(Line, "/$c") <> 0 Then
                            myResp = Split(Line, "/$c", , CompareMethod.Text)
                            myNewResp = myResp(1)
                        End If

                        writer.WriteStartElement("marc:datafield")
                        writer.WriteAttributeString("tag", "245")
                        writer.WriteAttributeString("ind1", FI)
                        writer.WriteAttributeString("ind2", SI)
                        writer.WriteStartElement("marc:subfield")
                        writer.WriteAttributeString("code", "a")
                        writer.WriteString(myTitle)
                        writer.WriteEndElement()
                        If mySubTitle <> "" Then
                            writer.WriteStartElement("marc:subfield")
                            writer.WriteAttributeString("code", "b")
                            writer.WriteString(mySubTitle)
                            writer.WriteEndElement()
                        End If
                        If myNewResp <> "" Then
                            writer.WriteStartElement("marc:subfield")
                            writer.WriteAttributeString("code", "c")
                            writer.WriteString(myNewResp)
                            writer.WriteEndElement()
                        End If
                        writer.WriteEndElement()
                        Line = ""

                        '246 VAR_TITLE
                        If dt.Rows(0).Item("VAR_TITLE").ToString <> "" Then
                            writer.WriteStartElement("marc:datafield")
                            writer.WriteAttributeString("tag", "246")
                            writer.WriteAttributeString("ind1", "3")
                            writer.WriteAttributeString("ind2", "3")
                            writer.WriteStartElement("marc:subfield")
                            writer.WriteAttributeString("code", "a")
                            writer.WriteString(TrimAll(dt.Rows(0).Item("VAR_TITLE").ToString()))
                            writer.WriteEndElement()
                            writer.WriteEndElement()
                        End If
                        Line = ""

                        '250 Edition Statement
                        If dt.Rows(0).Item("EDITION").ToString <> "" Then
                            Dim myEd As Object
                            myEd = TrimAll(dt.Rows(0).Item("EDITION").ToString)
                            myEd = Replace(myEd, ".", "")
                            If InStr(myEd, "Ed") = 0 Then
                                myEd = myEd + " Ed"
                            End If
                            myEd = Replace(myEd, "Edition", "Ed")
                            myEd = Replace(myEd, "Edition.", "Ed")
                            myEd = Replace(myEd, "edition", "Ed")
                            myEd = Replace(myEd, "edition.", "Ed")

                            writer.WriteStartElement("marc:datafield")
                            writer.WriteAttributeString("tag", "250")
                            writer.WriteAttributeString("ind1", "")
                            writer.WriteAttributeString("ind2", "")
                            writer.WriteStartElement("marc:subfield")
                            writer.WriteAttributeString("code", "a")
                            writer.WriteString(myEd & ".")
                            writer.WriteEndElement()
                            writer.WriteEndElement()
                        Else
                            writer.WriteStartElement("marc:datafield")
                            writer.WriteAttributeString("tag", "250")
                            writer.WriteAttributeString("ind1", "")
                            writer.WriteAttributeString("ind2", "")
                            writer.WriteStartElement("marc:subfield")
                            writer.WriteAttributeString("code", "a")
                            writer.WriteString("1st Ed.")
                            writer.WriteEndElement()
                            writer.WriteEndElement()
                        End If
                        Line = ""

                        '260 Imprint Statement
                        Dim Place As String = Nothing
                        Dim Pub As String = Nothing
                        Dim Year As Object = Nothing
                        If dt.Rows(0).Item("PLACE_OF_PUB").ToString <> "" Then
                            Place = TrimAll(dt.Rows(0).Item("PLACE_OF_PUB").ToString()) & " :"
                        Else
                            Place = "[S.l.] :"
                        End If
                        Dim myPub As Object = Nothing
                        If dt.Rows(0).Item("PUB_NAME").ToString <> "" Then
                            myPub = TrimAll(dt.Rows(0).Item("PUB_NAME").ToString())
                            myPub = TrimAll(Replace(myPub, "a ", "", 1, 2))
                            myPub = TrimAll(Replace(myPub, "A ", "", 1, 2))
                            myPub = TrimAll(Replace(myPub, "an ", "", 1, 3))
                            myPub = TrimAll(Replace(myPub, "An ", "", 1, 3))
                            myPub = TrimAll(Replace(myPub, "AN ", "", 1, 3))
                            myPub = TrimAll(Replace(myPub, "aN ", "", 1, 3))
                            myPub = TrimAll(Replace(myPub, "The ", "", 1, 4))
                            myPub = TrimAll(Replace(myPub, "the ", "", 1, 4))
                            myPub = TrimAll(Replace(myPub, "THe ", "", 1, 4))
                            myPub = TrimAll(Replace(myPub, "THE ", "", 1, 4))
                            myPub = TrimAll(Replace(myPub, "tHe ", "", 1, 4))
                            myPub = TrimAll(Replace(myPub, "tHE ", "", 1, 4))
                            Line = Line & "$b" & TrimAll(myPub) & ","
                            Pub = TrimAll(myPub) & ","
                        Else
                            Pub = "[S.l.],"
                        End If
                        If dt.Rows(0).Item("YEAR_OF_PUB").ToString <> "" Then
                            Line = Line & "$c" & TrimAll(dt.Rows(0).Item("YEAR_OF_PUB").ToString()) & "."
                            Year = TrimAll(dt.Rows(0).Item("YEAR_OF_PUB").ToString()) & "."
                        Else
                            Year = Today.Year & "?]."
                        End If

                        writer.WriteStartElement("marc:datafield")
                        writer.WriteAttributeString("tag", "260")
                        writer.WriteAttributeString("ind1", "")
                        writer.WriteAttributeString("ind2", "")
                        writer.WriteStartElement("marc:subfield")
                        writer.WriteAttributeString("code", "a")
                        writer.WriteString(Place)
                        writer.WriteEndElement()
                        writer.WriteStartElement("marc:subfield")
                        writer.WriteAttributeString("code", "b")
                        writer.WriteString(Pub)
                        writer.WriteEndElement()
                        writer.WriteStartElement("marc:subfield")
                        writer.WriteAttributeString("code", "c")
                        writer.WriteString(Year)
                        writer.WriteEndElement()
                        writer.WriteEndElement()
                        Line = ""

                        '300 pagination
                        Dim Pages As Object = Nothing
                        Dim AccMAt As Object = Nothing
                        Dim Vol As Object = Nothing
                        Dim Dimen As Object = Nothing
                        If dt.Rows(0).Item("PAGINATION").ToString <> "" Then
                            Dim myPages As Object = Nothing
                            myPages = TrimAll(dt.Rows(0).Item("PAGINATION").ToString())
                            myPages = Replace(myPages, ";", ",")
                            myPages = Replace(myPages, ".", "")
                            If dt.Rows(0).Item("MULTI_VOL").ToString = "N" Then
                                If InStr(myPages, "p") = 0 Then
                                    Pages = TrimAll(myPages) + "p."
                                Else
                                    Line = Line & "$a" & TrimAll(myPages)
                                    Pages = TrimAll(myPages) + "."
                                End If
                                If dt.Rows(0).Item("ACC_MAT_CODE").ToString <> "" Then
                                    AccMAt = TrimAll(dt.Rows(0).Item("ACC_MAT_CODE").ToString()) + "."
                                End If
                            Else
                                If InStr(myPages, "p") = 0 Then
                                    If dt.Rows(0).Item("VOL_NO").ToString() <> "" Then
                                        Vol = " v.<" & TrimAll(dt.Rows(0).Item("VOL_NO").ToString()) & ">."
                                        Pages = TrimAll(myPages) + "p." + Vol
                                    End If
                                Else
                                    Pages = TrimAll(myPages) + TrimAll(myPages) & " v.<" & TrimAll(dt.Rows(0).Item("VOL_NO").ToString()) & ">."
                                End If
                                Dimen = " 00 cm."
                                If dt.Rows(0).Item("ACC_MAT_CODE").ToString <> "" Then
                                    AccMAt = TrimAll(dt.Rows(0).Item("ACC_MAT_CODE").ToString()) + "."
                                End If
                            End If

                            writer.WriteStartElement("marc:datafield")
                            writer.WriteAttributeString("tag", "300")
                            writer.WriteAttributeString("ind1", "")
                            writer.WriteAttributeString("ind2", "")
                            writer.WriteStartElement("marc:subfield")
                            writer.WriteAttributeString("code", "a")
                            writer.WriteString(Pages)
                            writer.WriteEndElement()
                            If Dimen <> "" Then
                                writer.WriteStartElement("marc:subfield")
                                writer.WriteAttributeString("code", "c")
                                writer.WriteString(Dimen)
                                writer.WriteEndElement()
                            End If
                            If AccMAt <> "" Then
                                writer.WriteStartElement("marc:subfield")
                                writer.WriteAttributeString("code", "e")
                                writer.WriteString(AccMAt)
                                writer.WriteEndElement()
                            End If
                            writer.WriteEndElement()
                        End If
                        Line = ""

                        '490 Series Statement
                        Dim FII As String = Nothing
                        If dt.Rows(0).Item("SERIES_TITLE").ToString <> "" Then
                            If dt.Rows(0).Item("SERIES_EDITOR").ToString <> "" Then
                                FII = "1"
                            Else
                                FII = "0"
                            End If
                            writer.WriteStartElement("marc:datafield")
                            writer.WriteAttributeString("tag", "490")
                            writer.WriteAttributeString("ind1", FII)
                            writer.WriteAttributeString("ind2", "")
                            writer.WriteStartElement("marc:subfield")
                            writer.WriteAttributeString("code", "a")
                            writer.WriteString(TrimAll(dt.Rows(0).Item("SERIES_TITLE").ToString()))
                            writer.WriteEndElement()
                            writer.WriteEndElement()
                        End If
                        Line = ""

                        '500 Note
                        If dt.Rows(0).Item("NOTE").ToString <> "" Then
                            Dim my500 As Object
                            my500 = TrimAll(dt.Rows(0).Item("NOTE").ToString)
                            my500 = removeascii(my500)
                            If my500.ToString <> "" Then
                                If dt.Rows(0).Item("NOTE").ToString <> "" Then
                                    writer.WriteStartElement("marc:datafield")
                                    writer.WriteAttributeString("tag", "500")
                                    writer.WriteAttributeString("ind1", "")
                                    writer.WriteAttributeString("ind2", "")
                                    writer.WriteStartElement("marc:subfield")
                                    writer.WriteAttributeString("code", "a")
                                    writer.WriteString(TrimAll(dt.Rows(0).Item("NOTE").ToString()) & ".")
                                    writer.WriteEndElement()
                                    writer.WriteEndElement()
                                End If
                                Line = ""
                            End If
                        End If

                        '520 Summary/abstract
                        If dt.Rows(0).Item("ABSTRACT").ToString <> "" Then
                            writer.WriteStartElement("marc:datafield")
                            writer.WriteAttributeString("tag", "520")
                            writer.WriteAttributeString("ind1", "3")
                            writer.WriteAttributeString("ind2", "")
                            writer.WriteStartElement("marc:subfield")
                            writer.WriteAttributeString("code", "a")
                            writer.WriteString(TrimAll(dt.Rows(0).Item("ABSTRACT").ToString()) & ".")
                            writer.WriteEndElement()
                            writer.WriteEndElement()
                        End If
                        Line = ""

                        '650 SUBJECT - TROPICAL
                        If dt.Rows(0).Item("SUB_NAME").ToString <> "" Then
                            writer.WriteStartElement("marc:datafield")
                            writer.WriteAttributeString("tag", "650")
                            writer.WriteAttributeString("ind1", "")
                            writer.WriteAttributeString("ind2", "4")
                            writer.WriteStartElement("marc:subfield")
                            writer.WriteAttributeString("code", "a")
                            writer.WriteString(TrimAll(dt.Rows(0).Item("SUB_NAME").ToString() & "."))
                            writer.WriteEndElement()
                            writer.WriteEndElement()
                        End If

                        If dt.Rows(0).Item("KEYWORDS").ToString <> "" Then
                            If InStr(dt.Rows(0).Item("KEYWORDS"), ";") <> 0 Then
                                Dim myKeyword As Object
                                myKeyword = TrimAll(dt.Rows(0).Item("KEYWORDS").ToString)
                                myKeyword = Split(myKeyword, ";")
                                Dim m
                                For m = 0 To UBound(myKeyword)
                                    writer.WriteStartElement("marc:datafield")
                                    writer.WriteAttributeString("tag", "650")
                                    writer.WriteAttributeString("ind1", "")
                                    writer.WriteAttributeString("ind2", "4")
                                    writer.WriteStartElement("marc:subfield")
                                    writer.WriteAttributeString("code", "a")
                                    writer.WriteString(TrimAll(myKeyword(m)) & ".")
                                    writer.WriteEndElement()
                                    writer.WriteEndElement()
                                Next
                            Else
                                writer.WriteStartElement("marc:datafield")
                                writer.WriteAttributeString("tag", "650")
                                writer.WriteAttributeString("ind1", "")
                                writer.WriteAttributeString("ind2", "4")
                                writer.WriteStartElement("marc:subfield")
                                writer.WriteAttributeString("code", "a")
                                writer.WriteString(TrimAll(dt.Rows(0).Item("KEYWORDS").ToString) & ".")
                                writer.WriteEndElement()
                                writer.WriteEndElement()
                            End If
                        End If
                        Line = ""

                        '700 ADDED ENTRIES - Personal Name
                        Dim Ind1 As String = Nothing
                        Dim Ind2 As String = Nothing
                        Dim JAuth As String = Nothing
                        If dt.Rows(0).Item("AUTHOR2").ToString <> "" Or dt.Rows(0).Item("AUTHOR3").ToString <> "" Or dt.Rows(0).Item("EDITOR").ToString <> "" Or dt.Rows(0).Item("TRANSLATOR").ToString <> "" Or dt.Rows(0).Item("ILLUSTRATOR").ToString <> "" Then
                            If dt.Rows(0).Item("AUTHOR2").ToString <> "" Then
                                If InStr(dt.Rows(0).Item("AUTHOR2"), ",") <> 0 Then
                                    Ind1 = "1"
                                    Ind2 = ""
                                    JAuth = TrimAll(Replace(dt.Rows(0).Item("AUTHOR2").ToString(), ".", " "))
                                Else
                                    Ind1 = "0"
                                    Ind2 = ""
                                    JAuth = TrimAll(Replace(dt.Rows(0).Item("AUTHOR2").ToString(), ".", " "))
                                End If
                                writer.WriteStartElement("marc:datafield")
                                writer.WriteAttributeString("tag", "700")
                                writer.WriteAttributeString("ind1", Ind1)
                                writer.WriteAttributeString("ind2", Ind2)
                                writer.WriteStartElement("marc:subfield")
                                writer.WriteAttributeString("code", "a")
                                writer.WriteString(JAuth)
                                writer.WriteEndElement()
                                writer.WriteStartElement("marc:subfield")
                                writer.WriteAttributeString("code", "e")
                                writer.WriteString("Jt.Author")
                                writer.WriteEndElement()
                                writer.WriteEndElement()
                            End If

                            'jt.Autbhr 3
                            Dim In1 As String = Nothing
                            Dim In2 As String = Nothing
                            Dim JAuth3 As String = Nothing
                            If dt.Rows(0).Item("AUTHOR3").ToString <> "" Then
                                If InStr(dt.Rows(0).Item("AUTHOR3"), ",") <> 0 Then
                                    In1 = "1"
                                    In2 = ""
                                    JAuth3 = TrimAll(Replace(dt.Rows(0).Item("AUTHOR3").ToString(), ".", " "))
                                Else
                                    In1 = "0"
                                    In2 = ""
                                    JAuth3 = TrimAll(Replace(dt.Rows(0).Item("AUTHOR3").ToString(), ".", " "))
                                End If
                                writer.WriteStartElement("marc:datafield")
                                writer.WriteAttributeString("tag", "700")
                                writer.WriteAttributeString("ind1", In1)
                                writer.WriteAttributeString("ind2", In2)
                                writer.WriteStartElement("marc:subfield")
                                writer.WriteAttributeString("code", "a")
                                writer.WriteString(JAuth3)
                                writer.WriteEndElement()
                                writer.WriteStartElement("marc:subfield")
                                writer.WriteAttributeString("code", "e")
                                writer.WriteString("Jt.Author")
                                writer.WriteEndElement()
                                writer.WriteEndElement()
                            End If

                            Dim N1 As String = Nothing
                            Dim N2 As String = Nothing
                            Dim Ed As String = Nothing
                            If dt.Rows(0).Item("EDITOR").ToString <> "" Then
                                If InStr(dt.Rows(0).Item("EDITOR"), ";") <> 0 Then
                                    Dim myEditor As Object
                                    myEditor = TrimAll(dt.Rows(0).Item("EDITOR").ToString)
                                    myEditor = Split(myEditor, ";")
                                    Dim m
                                    For m = 0 To UBound(myEditor)
                                        If InStr(myEditor(m), ",") <> 0 Then
                                            N1 = "1"
                                            N2 = ""
                                            Ed = TrimAll(Replace(myEditor(m), ".", " "))
                                        Else
                                            N1 = "0"
                                            N2 = ""
                                            Ed = TrimAll(Replace(myEditor(m), ".", " "))
                                        End If
                                        writer.WriteStartElement("marc:datafield")
                                        writer.WriteAttributeString("tag", "700")
                                        writer.WriteAttributeString("ind1", N1)
                                        writer.WriteAttributeString("ind2", N2)
                                        writer.WriteStartElement("marc:subfield")
                                        writer.WriteAttributeString("code", "a")
                                        writer.WriteString(Ed)
                                        writer.WriteEndElement()
                                        writer.WriteStartElement("marc:subfield")
                                        writer.WriteAttributeString("code", "e")
                                        writer.WriteString("Ed")
                                        writer.WriteEndElement()
                                        writer.WriteEndElement()
                                    Next
                                Else
                                    If InStr(dt.Rows(0).Item("EDITOR"), ",") <> 0 Then
                                        N1 = "1"
                                        N2 = ""
                                        Ed = TrimAll(Replace(dt.Rows(0).Item("EDITOR").ToString(), ".", " "))
                                    Else
                                        N1 = "0"
                                        N2 = ""
                                        Ed = TrimAll(Replace(dt.Rows(0).Item("EDITOR").ToString(), ".", " "))
                                    End If
                                    writer.WriteStartElement("marc:datafield")
                                    writer.WriteAttributeString("tag", "700")
                                    writer.WriteAttributeString("ind1", N1)
                                    writer.WriteAttributeString("ind2", N2)
                                    writer.WriteStartElement("marc:subfield")
                                    writer.WriteAttributeString("code", "a")
                                    writer.WriteString(Ed)
                                    writer.WriteEndElement()
                                    writer.WriteStartElement("marc:subfield")
                                    writer.WriteAttributeString("code", "e")
                                    writer.WriteString("Ed")
                                    writer.WriteEndElement()
                                    writer.WriteEndElement()
                                End If
                            End If

                            Dim V1 As String = Nothing
                            Dim V2 As String = Nothing
                            Dim Tr As String = Nothing
                            If dt.Rows(0).Item("TRANSLATOR").ToString <> "" Then
                                If InStr(dt.Rows(0).Item("TRANSLATOR"), ";") <> 0 Then
                                    Dim myTr As Object
                                    myTr = TrimAll(dt.Rows(0).Item("TRANSLATOR").ToString)
                                    myTr = Split(myTr, ";")
                                    Dim m
                                    For m = 0 To UBound(myTr)
                                        If InStr(myTr(m), ",") <> 0 Then
                                            V1 = "1"
                                            V2 = ""
                                            Tr = TrimAll(Replace(myTr(m), ".", " "))
                                        Else
                                            V1 = "0"
                                            V2 = ""
                                            Tr = TrimAll(Replace(myTr(m), ".", " "))
                                        End If
                                        writer.WriteStartElement("marc:datafield")
                                        writer.WriteAttributeString("tag", "700")
                                        writer.WriteAttributeString("ind1", V1)
                                        writer.WriteAttributeString("ind2", V2)
                                        writer.WriteStartElement("marc:subfield")
                                        writer.WriteAttributeString("code", "a")
                                        writer.WriteString(Tr)
                                        writer.WriteEndElement()
                                        writer.WriteStartElement("marc:subfield")
                                        writer.WriteAttributeString("code", "e")
                                        writer.WriteString("Tr")
                                        writer.WriteEndElement()
                                        writer.WriteEndElement()
                                    Next
                                Else
                                    If InStr(dt.Rows(0).Item("TRANSLATOR"), ",") <> 0 Then
                                        V1 = "1"
                                        V2 = ""
                                        Tr = TrimAll(Replace(dt.Rows(0).Item("TRANSLATOR").ToString(), ".", " "))
                                    Else
                                        V1 = "0"
                                        V2 = ""
                                        Tr = TrimAll(Replace(dt.Rows(0).Item("TRANSLATOR").ToString(), ".", " "))
                                    End If
                                    writer.WriteStartElement("marc:datafield")
                                    writer.WriteAttributeString("tag", "700")
                                    writer.WriteAttributeString("ind1", V1)
                                    writer.WriteAttributeString("ind2", V2)
                                    writer.WriteStartElement("marc:subfield")
                                    writer.WriteAttributeString("code", "a")
                                    writer.WriteString(Tr)
                                    writer.WriteEndElement()
                                    writer.WriteStartElement("marc:subfield")
                                    writer.WriteAttributeString("code", "e")
                                    writer.WriteString("Tr")
                                    writer.WriteEndElement()
                                    writer.WriteEndElement()
                                End If
                            End If

                            Dim IL1 As String = Nothing
                            Dim IL2 As String = Nothing
                            Dim Illus As String = Nothing
                            If dt.Rows(0).Item("ILLUSTRATOR").ToString <> "" Then
                                If InStr(dt.Rows(0).Item("ILLUSTRATOR"), ";") <> 0 Then
                                    Dim myIllus As Object
                                    myIllus = TrimAll(dt.Rows(0).Item("ILLUSTRATOR").ToString)
                                    myIllus = Split(myIllus, ";")
                                    Dim m
                                    For m = 0 To UBound(myIllus)
                                        If InStr(myIllus(m), ",") <> 0 Then
                                            IL1 = "1"
                                            IL2 = ""
                                            Illus = TrimAll(Replace(myIllus(m), ".", " "))
                                        Else
                                            IL1 = "0"
                                            IL2 = ""
                                            Illus = TrimAll(Replace(myIllus(m), ".", " "))
                                        End If
                                        writer.WriteStartElement("marc:datafield")
                                        writer.WriteAttributeString("tag", "700")
                                        writer.WriteAttributeString("ind1", IL1)
                                        writer.WriteAttributeString("ind2", IL2)
                                        writer.WriteStartElement("marc:subfield")
                                        writer.WriteAttributeString("code", "a")
                                        writer.WriteString(Illus)
                                        writer.WriteEndElement()
                                        writer.WriteStartElement("marc:subfield")
                                        writer.WriteAttributeString("code", "e")
                                        writer.WriteString("Illus")
                                        writer.WriteEndElement()
                                        writer.WriteEndElement()
                                    Next
                                Else
                                    If InStr(dt.Rows(0).Item("ILLUSTRATOR"), ",") <> 0 Then
                                        IL1 = "1"
                                        IL2 = ""
                                        Illus = TrimAll(Replace(dt.Rows(0).Item("ILLUSTRATOR").ToString(), ".", " "))
                                    Else
                                        IL1 = "0"
                                        IL2 = ""
                                        Illus = TrimAll(Replace(dt.Rows(0).Item("ILLUSTRATOR").ToString(), ".", " "))
                                    End If
                                    writer.WriteStartElement("marc:datafield")
                                    writer.WriteAttributeString("tag", "700")
                                    writer.WriteAttributeString("ind1", IL1)
                                    writer.WriteAttributeString("ind2", IL2)
                                    writer.WriteStartElement("marc:subfield")
                                    writer.WriteAttributeString("code", "a")
                                    writer.WriteString(Illus)
                                    writer.WriteEndElement()
                                    writer.WriteStartElement("marc:subfield")
                                    writer.WriteAttributeString("code", "e")
                                    writer.WriteString("Illus")
                                    writer.WriteEndElement()
                                    writer.WriteEndElement()
                                End If
                            End If
                        End If
                        Line = ""

                        '710 Added entry for Corporate Authro
                        If dt.Rows(0).Item("AUTHOR1").ToString <> "" Then
                            If dt.Rows(0).Item("CORPORATE_AUTHOR").ToString <> "" Then
                                writer.WriteStartElement("marc:datafield")
                                writer.WriteAttributeString("tag", "710")
                                writer.WriteAttributeString("ind1", "2")
                                writer.WriteAttributeString("ind2", "")
                                writer.WriteStartElement("marc:subfield")
                                writer.WriteAttributeString("code", "a")
                                writer.WriteString(TrimAll(dt.Rows(0).Item("CORPORATE_AUTHOR").ToString()))
                                writer.WriteEndElement()
                                writer.WriteEndElement()
                            End If
                        End If

                        '711 Added entry for Conference / meeting
                        If dt.Rows(0).Item("AUTHOR1").ToString <> "" Or dt.Rows(0).Item("CORPORATE_AUTHOR").ToString <> "" Then
                            If dt.Rows(0).Item("CONF_NAME").ToString <> "" Then
                                writer.WriteStartElement("marc:datafield")
                                writer.WriteAttributeString("tag", "711")
                                writer.WriteAttributeString("ind1", "2")
                                writer.WriteAttributeString("ind2", "")
                                writer.WriteStartElement("marc:subfield")
                                writer.WriteAttributeString("code", "a")
                                writer.WriteString(TrimAll(dt.Rows(0).Item("CONF_NAME").ToString()))
                                writer.WriteEndElement()
                                writer.WriteEndElement()
                            End If
                        End If
                        Line = ""

                        '800 Series Editor
                        Dim Nx1 As String = Nothing
                        Dim Nx2 As String = Nothing
                        Dim SEd As String = Nothing
                        If dt.Rows(0).Item("SERIES_EDITOR").ToString <> "" Then
                            If InStr(dt.Rows(0).Item("SERIES_EDITOR"), ";") <> 0 Then
                                Dim myEditor As Object
                                myEditor = TrimAll(dt.Rows(0).Item("SERIES_EDITOR").ToString)
                                myEditor = Split(myEditor, ";")
                                Dim m
                                For m = 0 To UBound(myEditor)
                                    If InStr(myEditor(m), ",") <> 0 Then
                                        Nx1 = "1"
                                        Nx2 = ""
                                        SEd = TrimAll(Replace(myEditor(m), ".", " "))
                                    Else
                                        Nx1 = "0"
                                        Nx2 = ""
                                        SEd = TrimAll(Replace(myEditor(m), ".", " "))
                                    End If
                                    writer.WriteStartElement("marc:datafield")
                                    writer.WriteAttributeString("tag", "800")
                                    writer.WriteAttributeString("ind1", Nx1)
                                    writer.WriteAttributeString("ind2", Nx2)
                                    writer.WriteStartElement("marc:subfield")
                                    writer.WriteAttributeString("code", "a")
                                    writer.WriteString(SEd)
                                    writer.WriteEndElement()
                                    writer.WriteStartElement("marc:subfield")
                                    writer.WriteAttributeString("code", "t")
                                    writer.WriteString(TrimAll(dt.Rows(0).Item("SERIES_TITLE").ToString()) & ".")
                                    writer.WriteEndElement()
                                    writer.WriteEndElement()
                                Next
                            Else
                                If InStr(dt.Rows(0).Item("SERIES_EDITOR"), ",") <> 0 Then
                                    Nx1 = "1"
                                    Nx2 = ""
                                    SEd = TrimAll(Replace(dt.Rows(0).Item("SERIES_EDITOR").ToString(), ".", " "))
                                Else
                                    Nx1 = "0"
                                    Nx2 = ""
                                    SEd = TrimAll(Replace(dt.Rows(0).Item("SERIES_EDITOR").ToString(), ".", " "))
                                End If
                                writer.WriteStartElement("marc:datafield")
                                writer.WriteAttributeString("tag", "800")
                                writer.WriteAttributeString("ind1", Nx1)
                                writer.WriteAttributeString("ind2", Nx2)
                                writer.WriteStartElement("marc:subfield")
                                writer.WriteAttributeString("code", "a")
                                writer.WriteString(SEd)
                                writer.WriteEndElement()
                                writer.WriteStartElement("marc:subfield")
                                writer.WriteAttributeString("code", "t")
                                writer.WriteString(TrimAll(dt.Rows(0).Item("SERIES_TITLE").ToString()) & ".")
                                writer.WriteEndElement()
                                writer.WriteEndElement()
                            End If
                        End If
                        Line = ""

                        '850 Holding Institution                            
                        writer.WriteStartElement("marc:datafield")
                        writer.WriteAttributeString("tag", "850")
                        writer.WriteAttributeString("ind1", "")
                        writer.WriteAttributeString("ind2", "")
                        writer.WriteStartElement("marc:subfield")
                        writer.WriteAttributeString("code", "a")
                        writer.WriteString(LibCode)
                        writer.WriteEndElement()
                        writer.WriteEndElement()

                        '852 location on shelf  
                        If dt.Rows(0).Item("PHYSICAL_LOCATION").ToString <> "" Then
                            writer.WriteStartElement("marc:datafield")
                            writer.WriteAttributeString("tag", "852")
                            writer.WriteAttributeString("ind1", "")
                            writer.WriteAttributeString("ind2", "")
                            writer.WriteStartElement("marc:subfield")
                            writer.WriteAttributeString("code", "a")
                            writer.WriteString(TrimAll(dt.Rows(0).Item("PHYSICAL_LOCATION").ToString))
                            writer.WriteEndElement()
                            writer.WriteEndElement()
                        End If

                        '856 URL 
                        If dt.Rows(0).Item("URL").ToString <> "" Then
                            writer.WriteStartElement("marc:datafield")
                            writer.WriteAttributeString("tag", "852")
                            writer.WriteAttributeString("ind1", "4")
                            writer.WriteAttributeString("ind2", "")
                            writer.WriteStartElement("marc:subfield")
                            writer.WriteAttributeString("code", "u")
                            writer.WriteString(TrimAll(dt.Rows(0).Item("URL").ToString))
                            writer.WriteEndElement()
                            writer.WriteEndElement()
                        End If

                        '901 Hodlings data 
                        If dt.Rows(0).Item("ACCESSION_NO").ToString <> "" Then
                            writer.WriteStartElement("marc:datafield")
                            writer.WriteAttributeString("tag", "901")
                            writer.WriteAttributeString("ind1", "")
                            writer.WriteAttributeString("ind2", "")
                            writer.WriteStartElement("marc:subfield")
                            writer.WriteAttributeString("code", "a")
                            writer.WriteString(dt.Rows(0).Item("ACCESSION_NO").ToString)
                            writer.WriteEndElement()
                            Dim myACCDate As Object = Nothing
                            If dt.Rows(0).Item("ACCESSION_DATE").ToString <> "" Then
                                myACCDate = Format(dt.Rows(0).Item("ACCESSION_DATE"), "yyyyMMdd") + "000000.0" & vbCrLf
                            Else
                                myACCDate = Format(Date.Today, "yyyyMMdd") + "000000.0" & vbCrLf
                            End If
                            writer.WriteStartElement("marc:subfield")
                            writer.WriteAttributeString("code", "b")
                            writer.WriteString(myACCDate)
                            writer.WriteEndElement()
                            writer.WriteEndElement()
                        End If

                        '902 class no
                        If dt.Rows(0).Item("CLASS_NO").ToString <> "" Then
                            writer.WriteStartElement("marc:datafield")
                            writer.WriteAttributeString("tag", "902")
                            writer.WriteAttributeString("ind1", "")
                            writer.WriteAttributeString("ind2", "")
                            writer.WriteStartElement("marc:subfield")
                            writer.WriteAttributeString("code", "a")
                            writer.WriteString(dt.Rows(0).Item("CLASS_NO").ToString)
                            writer.WriteEndElement()
                            Dim myBookNo As Object = Nothing
                            If dt.Rows(0).Item("BOOK_NO").ToString <> "" Then
                                myBookNo = dt.Rows(0).Item("BOOK_NO").ToString & vbCrLf
                            Else
                                myBookNo = ""
                            End If
                            writer.WriteStartElement("marc:subfield")
                            writer.WriteAttributeString("code", "b")
                            writer.WriteString(myBookNo)
                            writer.WriteEndElement()
                            writer.WriteEndElement()
                        End If




                        writer.WriteEndElement()
                    End If
                End If
            Next

            writer.WriteEndDocument()
            writer.Close()

            Response.Flush()
            Response.End()

        Catch ex As Exception
            ' Lbl_Error.Text = ex.ToString
        Finally
            SqlConn.Close()
        End Try
    End Sub
    Public Sub ExportToCSV()
        Dim dt As DataTable = Nothing
        sb.Clear()
        Try
            Dim flag As Object = Nothing
            For Each row As GridViewRow In Grid1.Rows
                Dim cb As CheckBox = DirectCast(row.FindControl("cbd"), CheckBox)
                If cb IsNot Nothing AndAlso cb.Checked = True Then
                    Dim HOLD_ID As Integer = Convert.ToInt32(Grid1.DataKeys(row.RowIndex).Value)
                    Dim SQL As String = Nothing
                    SQL = "SELECT * FROM BOOKS_ACC_REGISTER_VIEW WHERE (LIB_CODE = '" & Trim(LibCode) & "') AND (HOLD_ID ='" & Trim(HOLD_ID) & "');"
                    Dim ds As New DataSet
                    Dim da As New SqlDataAdapter(SQL, SqlConn)
                    If SqlConn.State = 0 Then
                        SqlConn.Open()
                    End If
                    da.Fill(ds)
                    dt = ds.Tables(0).Copy
                    If flag = Nothing Then
                        For k As Integer = 0 To dt.Columns.Count - 1
                            sb.Append(dt.Columns(k).ColumnName + "#")
                        Next
                        sb.Append(vbCr & vbLf)
                    End If
                    flag = "x"

                    For i As Integer = 0 To dt.Rows.Count - 1
                        For k As Integer = 0 To dt.Columns.Count - 1
                            sb.Append(dt.Rows(i).Item(k).ToString + "#")
                        Next
                        sb.Append(vbCr & vbLf)
                    Next
                    SqlConn.Close()
                End If
            Next
        Catch ex As Exception
            Lbl_Error.Text = ex.ToString
        Finally
            SqlConn.Close()
        End Try
    End Sub
    Public Sub ExportToMARC21DisplaryFormatSerieals()
        Dim dt As DataTable = Nothing
        sb.Clear()
        Try
            For Each row As GridViewRow In Grid1.Rows
                Dim cb As CheckBox = DirectCast(row.FindControl("cbd"), CheckBox)
                If cb IsNot Nothing AndAlso cb.Checked = True Then
                    Dim HOLD_ID As Integer = Convert.ToInt32(Grid1.DataKeys(row.RowIndex).Value)
                    Dim SQL As String = Nothing
                    SQL = "SELECT * FROM BOOKS_ACC_REGISTER_VIEW WHERE (LIB_CODE = '" & Trim(LibCode) & "') AND (HOLD_ID ='" & Trim(HOLD_ID) & "');"
                    Dim ds As New DataSet
                    Dim da As New SqlDataAdapter(SQL, SqlConn)
                    If SqlConn.State = 0 Then
                        SqlConn.Open()
                    End If
                    da.Fill(ds)
                    dt = ds.Tables(0).Copy

                    If dt.Rows.Count <> 0 Then
                        '000 leader, 24 chr, 00-23 
                        sb.Append("=LDR  " & "00000") '00-04: record length will be calculated by system
                        sb.Append("n") '05:Record Status: always 'n'=new
                        sb.Append("a") '06:type of record:  a=lanuage materia                    
                        sb.Append("s") '07 Bib Level for Serials

                        sb.Append("\") ' 08: Type of control: blank(#) for not specified
                        sb.Append("a") '09: Character coding scheme: a=UCS/UNICODE
                        sb.Append("2") '10: indicator count: always 2
                        sb.Append("2") '11: subfield code count: always 2
                        sb.Append("00000") '12-16: base address of data:Calcualted by computer for each record
                        sb.Append("7") '17: Encoding level: 7=minimal level
                        sb.Append("a") '18: Descriptive cataloging form: a=AACR2
                        sb.Append("\") '19: Multipart Resource Record level: #=not applicable
                        sb.Append("4") '20: lenght of the-length-of-field position: always 4
                        sb.Append("5") '21: Length of the starting character position: always 5
                        sb.Append("0") '22: length of the implementation defined portion: always 0
                        sb.Append("0") '& vbCrLf '23: undefined: always 0
                        sb.Append(vbCrLf)

                        '001: Control Number , max 12 chr A/N Var12 (Cat No)
                        sb.Append("=001  " & dt.Rows(0).Item("CAT_NO").ToString) ' & vbCrLf
                        sb.Append(vbCrLf)

                        '003: Control Number Identification : In-DelNIC : Alpha/Var 8 max chr
                        sb.Append("=003  " & LibCode) '& vbCrLf
                        sb.Append(vbCrLf)

                        '005: Date and Time of the latest Transaction : 16chr long, yyyymmddhhuuss
                        If dt.Rows(0).Item("ACCESSION_DATE").ToString <> "" Then
                            sb.Append("=005  " & Format(dt.Rows(0).Item("ACCESSION_DATE"), "yyyyMMdd") + "000000.0") ' & vbCrLf
                            sb.Append(vbCrLf)
                        Else
                            sb.Append("=005  " & Format(Date.Today, "yyyyMMdd") + "000000.0") ' & vbCrLf
                            sb.Append(vbCrLf)
                        End If

                        'get serielas history details
                        Dim dtHist As DataTable = Nothing
                        Dim SQL3 As String
                        SQL3 = "SELECT * FROM J_HISTORY WHERE (CAT_NO = '" & Trim(dt.Rows(0).Item("CAT_NO").ToString) & "') "
                        Dim ds2 As New DataSet
                        Dim da2 As New SqlDataAdapter(SQL3, SqlConn)
                        If SqlConn.State = 0 Then
                            SqlConn.Open()
                        End If
                        da2.Fill(ds2)
                        If ds.Tables.Count > 0 Then
                            dtHist = ds2.Tables(0)
                        Else
                            Continue For
                        End If

                        '008 40 chr fixed length data, 00-39 (0-17 and 35-39 are common for all materials, 18-34 are separate for group of materiasl
                        '008/00-05 Date of creation in yyMMdd format
                        If dt.Rows(0).Item("ACCESSION_DATE").ToString <> "" Then
                            sb.Append("=008  " & Format(dt.Rows(0).Item("ACCESSION_DATE"), "yyMMdd"))
                        Else
                            sb.Append("=008  " & Format(Today.Date, "yyMMdd"))
                        End If
                        '008/06: 

                        sb.Append("u") '06 status unknown
                        '008/07-10 :Date 1 (Start date for serials)
                        If dtHist.Rows(0).Item("J_START_YEAR").ToString <> "" Then
                            sb.Append(Trim(dtHist.Rows(0).Item("J_START_YEAR").ToString))
                        Else
                            sb.Append(Trim(Today.Year)) '7-10 date 1
                        End If
                        '008/11-14 Date 2: serial end year
                        If dtHist.Rows(0).Item("J_CLOSE_YEAR").ToString <> "" Then
                            sb.Append(Trim(dtHist.Rows(0).Item("J_CLOSE_YEAR").ToString))
                        Else
                            sb.Append("uuuu") '11-14 date2 = blank
                        End If
                        '008/15-17: MARC country Code, Country of Publication
                        Dim myConcode As String = Nothing
                        If dt.Rows(0).Item("CON_CODE").ToString <> "" Then
                            Select Case dt.Rows(0).Item("CON_CODE").ToString
                                Case "IND"
                                    myConcode = "ii\"
                                Case "AUS"
                                    myConcode = "at\"
                                Case "FRA"
                                    myConcode = "fr\"
                                Case "GBR"
                                    myConcode = "xxk"
                                Case "HKG"
                                    myConcode = "cc\"
                                Case "JPN"
                                    myConcode = "ja\"
                                Case "PAK"
                                    myConcode = "pk\"
                                Case "RUS"
                                    myConcode = "ru\"
                                Case "USA"
                                    myConcode = "xxu"
                                Case Else
                                    myConcode = "ii\"
                            End Select
                            sb.Append(myConcode)
                        Else
                            sb.Append("xx\") '15-17 con code
                        End If

                        '008/18 Frequency
                        Dim FreqCode As Object = Nothing
                        If dtHist.Rows(0).Item("FREQ_CODE").ToString <> "" Then
                            Select Case dtHist.Rows(0).Item("FREQ_CODE").ToString
                                Case "DL"
                                    FreqCode = "d"
                                Case "SW"
                                    FreqCode = "c"
                                Case "WK"
                                    FreqCode = "w"
                                Case "M3"
                                    FreqCode = "j"
                                Case "BW"
                                    FreqCode = "e"
                                Case "FN"
                                    FreqCode = "s"
                                Case "MT"
                                    FreqCode = "m"
                                Case "BM"
                                    FreqCode = "b"
                                Case "QT"
                                    FreqCode = "q"
                                Case "Q3"
                                    FreqCode = "t"
                                Case "HY"
                                    FreqCode = "f"
                                Case "YY"
                                    FreqCode = "a"
                                Case Else
                                    FreqCode = "u"
                            End Select
                            sb.Append(FreqCode)
                        Else
                            sb.Append("u")
                        End If

                        '008/19 ' Regularity
                        sb.Append("r") '(r=regular)
                        '008/20: Undefined
                        sb.Append("\") '20
                        '008/21: Type of continuing resource
                        If dt.Rows(0).Item("MAT_CODE").ToString <> "" Then
                            If dt.Rows(0).Item("MAT_CODE").ToString = "P" Then
                                sb.Append("p") 'p=periodical
                            Else
                                sb.Append("n") 'n=newspaper
                            End If
                        Else
                            sb.Append("p") 'p=periodical
                        End If
                        '008/22: Form of original item
                        sb.Append("d") 'd=large print
                        '008/23: Form of Item
                        sb.Append("d") 'd=large print
                        '008/24: Nature of entire work
                        sb.Append("\") 'blank
                        '008/25-27: Nature of contents
                        sb.Append("\\\") '
                        '008/28: Government publication
                        sb.Append("\") 'blank
                        '008/29: Conf Publication                    
                        sb.Append("0") '0=not a conference
                        '008/30-32: Undefined
                        sb.Append("\\\") 'blank

                        '008/33: Origianl Alphabet or script of file
                        Dim LangCode As String = Nothing
                        If dt.Rows(0).Item("LANG_CODE").ToString <> "" Then
                            Select Case dt.Rows(0).Item("LANG_CODE").ToString
                                Case "ENG"
                                    LangCode = "a" 'roman
                                Case "TAM"
                                    LangCode = "l"
                                Case Else
                                    LangCode = "j" 'devnagri
                            End Select
                            sb.Append(LangCode)
                        Else
                            sb.Append("a") 'eng
                        End If

                        '008/34: entry convention
                        sb.Append("0") '0=successive entry
                        '008/35-37 lang code
                        If dt.Rows(0).Item("LANG_CODE").ToString <> "" Then
                            sb.Append(LCase(dt.Rows(0).Item("LANG_CODE").ToString))
                        Else
                            sb.Append("eng")
                        End If
                        '008/38: Modified Record
                        sb.Append("\") 'not modified
                        '008/39: Cataloging source
                        sb.Append("d") ' & vbCrLf 'd=other
                        sb.Append(vbCrLf)

                        '022 ISSN
                        Dim myISSN As Object
                        If dt.Rows(0).Item("STANDARD_NO").ToString <> "" Then
                            myISSN = TrimX(Replace(TrimX(dt.Rows(0).Item("STANDARD_NO").ToString()), "-", ""))
                            If Len(myISSN) = 10 Or Len(myISSN) = 13 Then
                                sb.Append("=022  " & "\\" & "$a" & Replace(TrimX(dt.Rows(0).Item("STANDARD_NO").ToString()), "-", "")) ' & vbCrLf
                            Else
                                sb.Append("=022 " & "\\" & "$z" & Replace(TrimX(dt.Rows(0).Item("STANDARD_NO").ToString()), "-", "")) ' & vbCrLf
                            End If
                            sb.Append(vbCrLf)
                        End If

                        '030 CODEN
                        If dtHist.Rows(0).Item("CODEN").ToString <> "" Then
                            sb.Append("=030  " & "\\" & "$a" & TrimX(dtHist.Rows(0).Item("CODEN").ToString())) ' & vbCrLf
                            sb.Append(vbCrLf)
                        End If

                        '040 cataloging agency
                        sb.Append("=040  " & "\\" & "$a" & LibCode & "$c" & LibCode) ' & vbCrLf
                        sb.Append(vbCrLf)

                        '041 Language Code
                        If dt.Rows(0).Item("LANG_CODE").ToString <> "" Then
                            sb.Append("=041  " & "0\" & "$a" & LCase(TrimAll(dt.Rows(0).Item("LANG_CODE").ToString))) ' & vbCrLf
                            sb.Append(vbCrLf)
                        Else
                            sb.Append("=041  " & "0\" & "$a" & "eng") ' & vbCrLf
                            sb.Append(vbCrLf)
                        End If

                        '044 Country of Publication
                        If myConcode <> "" Then
                            sb.Append("=044  " & "\\" & "$a" & myConcode) ' & vbCrLf
                            sb.Append(vbCrLf)
                        Else
                            sb.Append("=044  " & "\\" & "$a" & "xx\") ' & vbCrLf
                            sb.Append(vbCrLf)
                        End If

                        '080 UDC No
                        If RB_UDC.Checked = True Then 'for UDC
                            If dt.Rows(0).Item("CLASS_NO").ToString <> "" Then
                                sb.Append("=080  " & "\\" & "$a" & TrimAll(dt.Rows(0).Item("CLASS_NO").ToString))
                                If dt.Rows(0).Item("BOOK_NO").ToString <> "" Then
                                    sb.Append("$b" & dt.Rows(0).Item("BOOK_NO").ToString)
                                End If
                                sb.Append(vbCrLf)
                            End If
                        End If

                        '082 DDC No
                        If RB_DDC.Checked = True Then 'for DDC
                            If dt.Rows(0).Item("CLASS_NO").ToString <> "" Then
                                sb.Append("=082  " & "04" & "$a" & TrimAll(dt.Rows(0).Item("CLASS_NO").ToString))
                                If dt.Rows(0).Item("BOOK_NO").ToString <> "" Then
                                    sb.Append("$b" & dt.Rows(0).Item("BOOK_NO").ToString)
                                End If
                                sb.Append(vbCrLf)
                            End If
                        End If

                        '245 Main Title
                        Dim myTitle As String = Nothing
                        Dim myRKM As Object = Nothing
                        myTitle = Replace(TrimAll(dt.Rows(0).Item("TITLE").ToString()), ":", ",")
                        myTitle = Replace(myTitle, "...", "--")
                        myTitle = TrimAll(myTitle)
                        myTitle = removeascii(myTitle)

                        If myTitle <> "" Then
                            sb.Append("=245  " & "0") '  index the title as an added entry
                            ''count non-filing caharacters - omiited by RKM
                            If Len(myTitle) > 1 Then
                                If TrimAll(dt.Rows(0).Item("TITLE")).Substring(0, 2) = "a " Or TrimAll(dt.Rows(0).Item("TITLE")).Substring(0, 2) = "A " Then
                                    sb.Append("2")
                                    myRKM = "RKM"
                                End If
                            End If
                            If Len(myTitle) > 2 Then
                                If TrimAll(dt.Rows(0).Item("TITLE")).Substring(0, 3) = "an " Or TrimAll(dt.Rows(0).Item("TITLE")).Substring(0, 3) = "An " Then
                                    sb.Append("3")
                                    myRKM = "RKM"
                                End If
                            End If
                            If Len(myTitle) > 3 Then
                                If TrimAll(dt.Rows(0).Item("TITLE")).Substring(0, 4) = "The " Or TrimAll(dt.Rows(0).Item("TITLE")).Substring(0, 4) = "the " Or TrimAll(dt.Rows(0).Item("TITLE")).Substring(0, 4) = "THE " Then
                                    sb.Append("4")
                                    myRKM = "RKM"
                                End If
                            End If
                            If myRKM <> "RKM" Then
                                sb.Append("0")
                            End If
                            sb.Append("$a" & myTitle)

                            Dim subTitle As Object
                            subTitle = removeascii(TrimAll(dt.Rows(0).Item("SUB_TITLE").ToString))
                            If Not Trim(subTitle.ToString) = String.Empty Then
                                sb.Append(" :$b" & Trim(subTitle))
                            End If
                            If dt.Rows(0).Item("EDITOR").ToString <> "" Then
                                sb.Append(" /$c")
                                If dt.Rows(0).Item("EDITOR").ToString <> "" Then
                                    sb.Append(" Edited by " & dt.Rows(0).Item("EDITOR").ToString)
                                End If
                            End If
                            sb.Append("." & vbCrLf)
                        End If

                        '246 VAR_TITLE
                        If dt.Rows(0).Item("VAR_TITLE").ToString <> "" Then
                            sb.Append("=246  " & "33" & "$a" & TrimAll(dt.Rows(0).Item("VAR_TITLE").ToString())) ' & vbCrLf
                        End If

                        '260 Imprint Statement
                        If dt.Rows(0).Item("PUB_NAME").ToString <> "" Then
                            sb.Append("=260  " & "3\") '3=current publisher
                            If dt.Rows(0).Item("PLACE_OF_PUB").ToString <> "" Then
                                sb.Append("$a" & TrimAll(dt.Rows(0).Item("PLACE_OF_PUB").ToString()) & " :")
                            Else
                                sb.Append("$a" & "[S.l.] :")
                            End If

                            Dim myPub As Object = Nothing
                            If dt.Rows(0).Item("PUB_NAME").ToString <> "" Then
                                myPub = TrimAll(dt.Rows(0).Item("PUB_NAME").ToString())
                                myPub = TrimAll(Replace(myPub, "a ", "", 1, 2))
                                myPub = TrimAll(Replace(myPub, "A ", "", 1, 2))
                                myPub = TrimAll(Replace(myPub, "an ", "", 1, 3))
                                myPub = TrimAll(Replace(myPub, "An ", "", 1, 3))
                                myPub = TrimAll(Replace(myPub, "AN ", "", 1, 3))
                                myPub = TrimAll(Replace(myPub, "aN ", "", 1, 3))
                                myPub = TrimAll(Replace(myPub, "The ", "", 1, 4))
                                myPub = TrimAll(Replace(myPub, "the ", "", 1, 4))
                                myPub = TrimAll(Replace(myPub, "THe ", "", 1, 4))
                                myPub = TrimAll(Replace(myPub, "THE ", "", 1, 4))
                                myPub = TrimAll(Replace(myPub, "tHe ", "", 1, 4))
                                myPub = TrimAll(Replace(myPub, "tHE ", "", 1, 4))
                                sb.Append("$b" & TrimAll(myPub) & ",")
                            Else
                                sb.Append("$b" & "[S.l.],")
                            End If
                            If dt.Rows(0).Item("JYEAR").ToString <> "" Then
                                sb.Append("$c" & TrimAll(dt.Rows(0).Item("JYEAR").ToString()) & ".")
                            Else
                                sb.Append("$c[" & Today.Year & "?]")
                            End If
                            sb.Append(vbCrLf)
                        End If

                        '300 pagination
                        If dt.Rows(0).Item("PAGINATION").ToString <> "" Then
                            Dim myPages As Object = Nothing
                            myPages = Replace(TrimAll(dt.Rows(0).Item("PAGINATION").ToString()), ";", ",")
                            sb.Append("=300  " & "\\" & "$av. :$bill.")
                            'If InStr(dt.Rows(0).Item("PAGINATION").ToString(), "p") = 0 Then
                            '    sb.Append("$a" & "v."
                            '    'If dt.Rows(0).Item("VOL_NO").ToString() <> "" Then
                            '    '    sb.Append("v.<" & TrimAll(dt.Rows(0).Item("VOL_NO").ToString()) & ">."
                            '    'End If
                            'Else
                            '    sb.Append("$a" & TrimAll(myPages) & "v.<" & TrimAll(dt.Rows(0).Item("VOL_NO").ToString()) & ">."
                            'End If
                            sb.Append(" ;$c00 cm.")
                            If dt.Rows(0).Item("ACC_MAT_CODE").ToString <> "" Then
                                sb.Append("; +$e" & TrimAll(dt.Rows(0).Item("ACC_MAT_CODE").ToString()) + ".")
                            End If
                            sb.Append(vbCrLf)
                        End If

                        '310 Current Publication Freq
                        Dim Freq As Object = Nothing
                        sb.Append("=310  " & "\\")
                        If dtHist.Rows(0).Item("FREQ_CODE").ToString <> "" Then
                            Select Case dtHist.Rows(0).Item("FREQ_CODE").ToString
                                Case "DL"
                                    Freq = "d"
                                Case "SW"
                                    Freq = "c"
                                Case "WK"
                                    Freq = "w"
                                Case "M3"
                                    Freq = "j"
                                Case "BW"
                                    Freq = "e"
                                Case "FN"
                                    Freq = "s"
                                Case "MT"
                                    Freq = "m"
                                Case "BM"
                                    Freq = "b"
                                Case "QT"
                                    Freq = "q"
                                Case "Q3"
                                    Freq = "t"
                                Case "HY"
                                    Freq = "f"
                                Case "YY"
                                    Freq = "a"
                                Case Else
                                    Freq = "u"
                            End Select
                            sb.Append("$a" & Freq) ' & vbCrLf
                            sb.Append(vbCrLf)
                        Else
                            sb.Append("$a" & "u") ' & vbCrLf
                            sb.Append(vbCrLf)
                        End If

                        '362 Date of Publicatioins and Sequential Designations
                        If dt.Rows(0).Item("JYEAR").ToString <> "" Then
                            sb.Append("=362  " & "1\")
                            sb.Append("$a")
                            If dt.Rows(0).Item("VOL_NO").ToString() <> "" Then
                                sb.Append("Vol." & TrimAll(dt.Rows(0).Item("VOL_NO").ToString()))
                            End If
                            If dt.Rows(0).Item("ISSUE_NO").ToString() <> "" Then
                                sb.Append(",No." & TrimAll(dt.Rows(0).Item("ISSUE_NO").ToString()))
                            End If
                            If dt.Rows(0).Item("JYEAR").ToString() <> "" Then
                                sb.Append("(" & TrimAll(dt.Rows(0).Item("JYEAR").ToString()) & ")")
                            End If
                            sb.Append(".") ' & vbCrLf
                            sb.Append(vbCrLf)
                        End If

                        '500 Note
                        If dt.Rows(0).Item("NOTE").ToString <> "" Then
                            Dim my500 As Object
                            my500 = TrimAll(dt.Rows(0).Item("NOTE").ToString)
                            my500 = removeascii(my500)
                            If my500.ToString <> "" Then
                                sb.Append("=500  " & "\\")
                                sb.Append("$a" & removeascii(TrimAll(dt.Rows(0).Item("NOTE").ToString())) & ".") ' & vbCrLf
                                sb.Append(vbCrLf)
                            End If
                        End If

                        '650 SUBJECT - TROPICAL
                        If dt.Rows(0).Item("SUB_NAME").ToString <> "" Then
                            sb.Append("=650  " & "\4")
                            sb.Append("$a" & TrimAll(dt.Rows(0).Item("SUB_NAME").ToString()) & ".") ' & vbCrLf
                            sb.Append(vbCrLf)
                        End If

                        If dt.Rows(0).Item("KEYWORDS").ToString <> "" Then
                            If InStr(dt.Rows(0).Item("KEYWORDS"), ";") <> 0 Then
                                Dim myKeyword As Object
                                myKeyword = TrimAll(dt.Rows(0).Item("KEYWORDS").ToString)
                                myKeyword = Split(myKeyword, ";")
                                Dim m
                                For m = 0 To UBound(myKeyword)
                                    sb.Append("=650  " & "\4")
                                    sb.Append("$a" & TrimAll(myKeyword(m)))
                                    sb.Append(".") ' & vbCrLf
                                    sb.Append(vbCrLf)
                                Next
                            Else
                                sb.Append("=650  " & "\4")
                                sb.Append("$a" & TrimAll(dt.Rows(0).Item("KEYWORDS").ToString))
                                sb.Append(".") ' & vbCrLf
                                sb.Append(vbCrLf)
                            End If
                        End If

                        '700 ADDED ENTRIES - Personal Name
                        If dt.Rows(0).Item("EDITOR").ToString <> "" Then
                            If InStr(dt.Rows(0).Item("EDITOR"), ";") <> 0 Then
                                Dim myEditor As Object
                                myEditor = TrimAll(dt.Rows(0).Item("EDITOR").ToString)
                                myEditor = Split(myEditor, ";")
                                Dim m
                                For m = 0 To UBound(myEditor)
                                    If InStr(myEditor(m), ",") <> 0 Then
                                        sb.Append("=700  " & "1\" & "$a" & TrimAll(Replace((myEditor(m)), ".", " ")))
                                        sb.Append(",$e" & "Ed.") ' & vbCrLf
                                        sb.Append(vbCrLf)
                                    Else
                                        sb.Append("=700  " & "0\" & "$a" & TrimAll(Replace((myEditor(m)), ".", " ")))
                                        sb.Append(",$e" & "Ed.") ' & vbCrLf
                                    End If
                                Next
                            Else
                                If InStr(dt.Rows(0).Item("EDITOR"), ",") <> 0 Then
                                    sb.Append("=700  " & "1\" & "$a" & TrimAll(Replace(dt.Rows(0).Item("EDITOR").ToString(), ".", " ")))
                                    sb.Append(",$e" & "Ed.") ' & vbCrLf
                                Else
                                    sb.Append("=700  " & "0\" & "$a" & TrimAll(Replace(dt.Rows(0).Item("EDITOR").ToString(), ".", " ")))
                                    sb.Append(",$e" & "Ed.") ' & vbCrLf
                                End If
                            End If
                        End If

                        If dt.Rows(0).Item("TRANSLATOR").ToString <> "" Then
                            If InStr(dt.Rows(0).Item("TRANSLATOR"), ";") <> 0 Then
                                Dim myTr As Object
                                myTr = TrimAll(dt.Rows(0).Item("TRANSLATOR").ToString)
                                myTr = Split(myTr, ";")
                                Dim m
                                For m = 0 To UBound(myTr)
                                    If InStr(myTr(m), ",") <> 0 Then
                                        sb.Append("=700  " & "1\" & "$a" & TrimAll(Replace((myTr(m)), ".", " ")))
                                        sb.Append(",$e" & "Tr.") ' & vbCrLf
                                        sb.Append(vbCrLf)
                                    Else
                                        sb.Append("=700  " & "0\" & "$a" & TrimAll(Replace((myTr(m)), ".", " ")))
                                        sb.Append(",$e" & "Tr.") ' & vbCrLf
                                        sb.Append(vbCrLf)
                                    End If
                                Next
                            Else
                                If InStr(dt.Rows(0).Item("TRANSLATOR"), ",") <> 0 Then
                                    sb.Append("=700  " & "1\" & "$a" & TrimAll(Replace(dt.Rows(0).Item("TRANSLATOR").ToString(), ".", " ")))
                                    sb.Append(",$e" & "Tr.") ' & vbCrLf
                                Else
                                    sb.Append("=700  " & "0\" & "$a" & TrimAll(Replace(dt.Rows(0).Item("TRANSLATOR").ToString(), ".", " ")))
                                    sb.Append(",$e" & "Tr.") ' & vbCrLf
                                End If
                            End If
                        End If

                        '850 Holding Institution                            
                        sb.Append("=850  " & "\\" & "$a" & Trim(LibCode)) ' & vbCrLf
                        sb.Append(vbCrLf)

                        '852 location on shelf  
                        If dt.Rows(0).Item("PHYSICAL_LOCATION").ToString <> "" Then
                            sb.Append("=852  " & "\\" & "$a" & TrimAll(dt.Rows(0).Item("PHYSICAL_LOCATION").ToString)) ' & vbCrLf
                            sb.Append(vbCrLf)
                        End If
                        '856 URL  
                        If dt.Rows(0).Item("URL").ToString <> "" Then
                            sb.Append("=856  " & "4\" & "$u" & TrimAll(dt.Rows(0).Item("URL").ToString)) ' & vbCrLf
                            sb.Append(vbCrLf)
                        End If

                        '901 Hodlings data 
                        If dt.Rows(0).Item("ACCESSION_NO").ToString <> "" Then
                            sb.Append("=901  " & "\\" & "$a" & TrimAll(dt.Rows(0).Item("ACCESSION_NO").ToString))
                            If dt.Rows(0).Item("ACCESSION_DATE").ToString <> "" Then
                                sb.Append("$b" & Format(dt.Rows(0).Item("ACCESSION_DATE"), "yyyyMMdd") + "000000.0") ' & vbCrLf
                                sb.Append(vbCrLf)
                            Else
                                sb.Append("$b" & Format(Date.Today, "yyyyMMdd") + "000000.0") ' & vbCrLf
                            End If
                        End If
                        '902 class no 
                        If dt.Rows(0).Item("CLASS_NO").ToString <> "" Then
                            sb.Append("=902  " & "\\" & "$a" & TrimAll(dt.Rows(0).Item("CLASS_NO").ToString))
                            If dt.Rows(0).Item("BOOK_NO").ToString <> "" Then
                                sb.Append("$b" & dt.Rows(0).Item("BOOK_NO").ToString)
                            End If
                            sb.Append(vbCrLf)
                        End If

                        sb.Replace("..", ".")
                        sb.Append("*")
                        sb.Append(vbCr & vbLf)
                    End If
                    SqlConn.Close()
                End If
            Next
        Catch ex As Exception
            Lbl_Error.Text = "Error: " & (ex.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    Public Sub ExportToMARC21DisplaryFormatBooks()
        Dim dt As DataTable = Nothing
        sb.Clear()
        Try
            For Each row As GridViewRow In Grid1.Rows
                Dim cb As CheckBox = DirectCast(row.FindControl("cbd"), CheckBox)
                If cb IsNot Nothing AndAlso cb.Checked = True Then
                    Dim HOLD_ID As Integer = Convert.ToInt32(Grid1.DataKeys(row.RowIndex).Value)
                    Dim SQL As String = Nothing
                    SQL = "SELECT * FROM BOOKS_ACC_REGISTER_VIEW WHERE (LIB_CODE = '" & Trim(LibCode) & "') AND (HOLD_ID ='" & Trim(HOLD_ID) & "');"
                    Dim ds As New DataSet
                    Dim da As New SqlDataAdapter(SQL, SqlConn)
                    If SqlConn.State = 0 Then
                        SqlConn.Open()
                    End If
                    da.Fill(ds)
                    dt = ds.Tables(0).Copy

                    If dt.Rows.Count <> 0 Then
                        sb.Append("")
                        '000 leader, 24 chr, 00-23 
                        sb.Append("=LDR  " & "00000") '00-04: record length will be calculated by system
                        sb.Append("n") '05:Record Status: always 'n'=new
                        sb.Append("a") '06:type of record:  a=lanuage materia
                        If dt.Rows(0).Item("BIB_CODE") = "M" Then ' 07: bib level
                            sb.Append("m")
                        Else
                            sb.Append("s")
                        End If
                        sb.Append("\") ' 08: Type of control: blank(#) for not specified
                        sb.Append("a") '09: Character coding scheme: a=UCS/UNICODE
                        sb.Append("2") '10: indicator count: always 2
                        sb.Append("2") '11: subfield code count: always 2
                        sb.Append("00000") '12-16: base address of data:Calcualted by computer for each record
                        sb.Append("7") '17: Encoding level: 7=minimal level
                        sb.Append("a") '18: Descriptive cataloging form: a=AACR2
                        sb.Append("\") '19: Multipart Resource Record level: #=not applicable
                        sb.Append("4") '20: lenght of the-length-of-field position: always 4
                        sb.Append("5") '21: Length of the starting character position: always 5
                        sb.Append("0") '22: length of the implementation defined portion: always 0
                        sb.Append("0") '23: undefined: always 0
                        sb.Append(vbCrLf)

                        '001: Control Number , max 12 chr A/N Var12 (Cat No)'
                        sb.Append("=001  " & dt.Rows(0).Item("CAT_NO").ToString) '& vbCrLf
                        sb.Append(vbCrLf)

                        '003: Control Number Identification : In-DelNIC : Alpha/Var 8 max chr
                        sb.Append("=003  " & LibCode) ' & vbCrLf
                        sb.Append(vbCr & vbLf)

                        '005: Date and Time of the latest Transaction : 16chr long, yyyymmddhhuuss
                        If dt.Rows(0).Item("ACCESSION_DATE").ToString <> "" Then
                            sb.Append("=005  " & Format(dt.Rows(0).Item("ACCESSION_DATE"), "yyyyMMdd") + "000000.0") ' & vbCrLf
                        Else
                            sb.Append("=005  " & Format(Date.Today, "yyyyMMdd") + "000000.0") ' & vbCrLf
                        End If
                        sb.Append(vbCr & vbLf)

                        '008 40 chr fixed length data, 00-39 (0-17 and 35-39 are common for all materials, 18-34 are separate for group of materiasl
                        '008/00-05 Date of creation in yyMMdd format
                        If dt.Rows(0).Item("ACCESSION_DATE").ToString <> "" Then
                            sb.Append("=008  " & Format(dt.Rows(0).Item("ACCESSION_DATE"), "yyMMdd"))
                        Else
                            sb.Append("=008  " & Format(Today.Date, "yyMMdd"))
                        End If

                        '008/06: 
                        sb.Append("s") '06 type of dates s=1 single dates
                        '008/07-10 :Date 1 (Start date for serials)
                        If dt.Rows(0).Item("YEAR_OF_PUB").ToString <> "" Then
                            sb.Append(Trim(dt.Rows(0).Item("YEAR_OF_PUB").ToString))
                        Else
                            sb.Append(Trim(Today.Year)) '7-10 date 1
                        End If
                        '008/11-14 Date 2: blank in case s=single date (end date for Serials)
                        sb.Append("\\\\") '11-14 date2 = blank

                        '008/15-17: MARC country Code, Country of Publication
                        Dim myConcode As String = Nothing
                        If dt.Rows(0).Item("CON_CODE").ToString <> "" Then
                            Select Case dt.Rows(0).Item("CON_CODE").ToString
                                Case "IND"
                                    myConcode = "ii\"
                                Case "AUS"
                                    myConcode = "at\"
                                Case "FRA"
                                    myConcode = "fr\"
                                Case "GBR"
                                    myConcode = "xxk"
                                Case "HKG"
                                    myConcode = "cc\"
                                Case "JPN"
                                    myConcode = "ja\"
                                Case "PAK"
                                    myConcode = "pk\"
                                Case "RUS"
                                    myConcode = "ru\"
                                Case "USA"
                                    myConcode = "xxu"
                                Case Else
                                    myConcode = "ii\"
                            End Select
                            sb.Append(myConcode)
                        Else
                            sb.Append("xx\") '15-17 con code
                        End If

                        '18-34 are separate for books and serials
                        '008/18-21 Illustration
                        sb.Append("\\\\") 'ill 18-21
                        '008/22: Target Audience
                        sb.Append("\") 'Audience 22
                        '008/23: Form of Item
                        sb.Append("d") 'Audience 23 d=large print
                        '008/24-27: 
                        sb.Append("\\\\") '24-27 nature of contents
                        '008/28: Govt publication
                        sb.Append("\") '28 Govt Publication
                        '008/29: Conf Publication
                        If dt.Rows(0).Item("CONF_NAME").ToString <> "" Then
                            sb.Append("1") 'yes
                        Else
                            sb.Append("0") 'no
                        End If
                        '008/30: Festchrift
                        sb.Append("0") '30 not a festchrift
                        '008/31: Index Y/N
                        sb.Append("0") '31 index present
                        '008/32: undefined
                        sb.Append("\") '32 undefined
                        '008/33: Literary Form
                        sb.Append("u") '33 literary form no u=unknown
                        '008/34: Biography
                        sb.Append("\") '34 biography no

                        '008/35-37 lang code
                        If dt.Rows(0).Item("LANG_CODE").ToString <> "" Then
                            sb.Append(LCase(dt.Rows(0).Item("LANG_CODE").ToString))
                        Else
                            sb.Append("eng")
                        End If
                        '008/38: Modified Record
                        sb.Append("\") '38 record not modified
                        '008/39: Cataloging source
                        sb.Append("d") '& vbCrLf '39 cataloging agency u=unknown
                        sb.Append(vbCrLf)

                        '020 ISBN
                        Dim myISBN As Object
                        If dt.Rows(0).Item("STANDARD_NO").ToString <> "" Then
                            myISBN = TrimX(Replace(TrimX(dt.Rows(0).Item("STANDARD_NO").ToString()), "-", ""))
                            If Len(myISBN) = 10 Or Len(myISBN) = 13 Then
                                sb.Append("=020  " & "\\" & "$a" & Replace(TrimX(dt.Rows(0).Item("STANDARD_NO").ToString()), "-", "")) ' & vbCrLf
                            Else
                                sb.Append("=020  " & "\\" & "$z" & Replace(TrimX(dt.Rows(0).Item("STANDARD_NO").ToString()), "-", "")) ' & vbCrLf
                            End If
                            sb.Append(vbCrLf)
                        End If

                        '040 cataloging agency
                        sb.Append("=040  " & "\\" & "$a" & LibCode & "$c" & LibCode) '& vbCrLf
                        sb.Append(vbCrLf)

                        '080 UDC No
                        If RB_UDC.Checked = True Then 'for UDC
                            If dt.Rows(0).Item("CLASS_NO").ToString <> "" Then
                                sb.Append("=080  " & "\\" & "$a" & TrimAll(dt.Rows(0).Item("CLASS_NO").ToString))
                                If dt.Rows(0).Item("BOOK_NO").ToString <> "" Then
                                    sb.Append("$b" & dt.Rows(0).Item("BOOK_NO").ToString)
                                End If
                                sb.Append(vbCrLf)
                            End If
                        End If

                        '082 DDC No
                        If RB_DDC.Checked = True Then 'for DDC
                            If dt.Rows(0).Item("CLASS_NO").ToString <> "" Then
                                sb.Append("=082  " & "04" & "$a" & TrimAll(dt.Rows(0).Item("CLASS_NO").ToString))
                                If dt.Rows(0).Item("BOOK_NO").ToString <> "" Then
                                    sb.Append("$b" & dt.Rows(0).Item("BOOK_NO").ToString)
                                End If
                                sb.Append(vbCrLf)
                            End If
                        End If

                        '088 Report Number Optional
                        If dt.Rows(0).Item("REPORT_NO").ToString <> "" Then
                            sb.Append("=088  " & "\\" & "$a" & dt.Rows(0).Item("REPORT_NO").ToString) ' & vbCrLf
                            sb.Append(vbCrLf)
                        End If

                        '100 Personal Author for Main entry
                        Dim myAuth As Object = Nothing
                        If dt.Rows(0).Item("AUTHOR1").ToString <> "" Then
                            If InStr(dt.Rows(0).Item("AUTHOR1"), ",").ToString <> 0 Then
                                myAuth = TrimAll(Replace(dt.Rows(0).Item("AUTHOR1").ToString(), ",", ", "))
                                myAuth = TrimAll(Replace(myAuth, ".", " "))
                                If Trim(myAuth.ToString.Substring(Len(myAuth) - 1)) = ")" Then
                                    sb.Append("=100  " & "1\" & "$a" & TrimAll(myAuth)) ' & vbCrLf
                                Else
                                    sb.Append("=100  " & "1\" & "$a" & TrimAll(myAuth) & ".") ' & vbCrLf
                                End If
                            Else
                                myAuth = TrimAll(Replace(dt.Rows(0).Item("AUTHOR1").ToString(), ".", " "))
                                If Trim(myAuth.ToString.Substring(Len(myAuth) - 1)) = ")" Then
                                    sb.Append("=100  " & "0\" & "$a" & TrimAll(myAuth)) ' & vbCrLf
                                Else
                                    sb.Append("=100  " & "0\" & "$a" & TrimAll(myAuth) & ".") ' & vbCrLf
                                End If
                            End If
                            sb.Append(vbCrLf)
                        End If

                        '110 Corporate Author for Main entry
                        If dt.Rows(0).Item("AUTHOR1").ToString = "" And dt.Rows(0).Item("CORPORATE_AUTHOR").ToString <> "" Then
                            sb.Append("=110  " & "2\" & "$a" & TrimAll(dt.Rows(0).Item("CORPORATE_AUTHOR").ToString()) & ".") ' & vbCrLf
                            sb.Append(vbCrLf)
                        End If
                        '111 Conference for Main entry
                        If dt.Rows(0).Item("AUTHOR1").ToString = "" And dt.Rows(0).Item("CORPORATE_AUTHOR").ToString = "" And dt.Rows(0).Item("CONF_NAME").ToString <> "" Then
                            sb.Append("=111  " & "2\" & "$a" & TrimAll(dt.Rows(0).Item("CONF_NAME").ToString()) & ".") ' & vbCrLf
                            sb.Append(vbCrLf)
                        End If

                        '130 UNIFORM TITLE for Main entry
                        Dim OneThirty As Object = Nothing
                        Dim my130Title As Object = Nothing
                        If dt.Rows(0).Item("AUTHOR1").ToString = "" And dt.Rows(0).Item("CORPORATE_AUTHOR").ToString = "" And dt.Rows(0).Item("CONF_NAME").ToString = "" Then
                            my130Title = TrimAll(dt.Rows(0).Item("TITLE").ToString())
                            my130Title = TrimAll(Replace(my130Title, "a ", "", 1, 2))
                            my130Title = TrimAll(Replace(my130Title, "A ", "", 1, 2))
                            my130Title = TrimAll(Replace(my130Title, "an ", "", 1, 3))
                            my130Title = TrimAll(Replace(my130Title, "An ", "", 1, 3))
                            my130Title = TrimAll(Replace(my130Title, "AN ", "", 1, 3))
                            my130Title = TrimAll(Replace(my130Title, "aN ", "", 1, 3))
                            my130Title = TrimAll(Replace(my130Title, "The ", "", 1, 4))
                            my130Title = TrimAll(Replace(my130Title, "the ", "", 1, 4))
                            my130Title = TrimAll(Replace(my130Title, "THe ", "", 1, 4))
                            my130Title = TrimAll(Replace(my130Title, "THE ", "", 1, 4))
                            my130Title = TrimAll(Replace(my130Title, "tHe ", "", 1, 4))
                            my130Title = TrimAll(Replace(my130Title, "tHE ", "", 1, 4))
                            sb.Append("=130  " & "0\" & "$a" & my130Title)
                            If dt.Rows(0).Item("LANG_CODE").ToString <> "" Then
                                sb.Append(".$l" & TrimAll(dt.Rows(0).Item("LANG_CODE").ToString()) & ".")
                            End If
                            sb.Append(vbCrLf)
                            OneThirty = "YES"
                        Else
                            OneThirty = "NO"
                        End If

                        '245 Main Title
                        Dim mySplitAuthor1, SurName1, ForeName1, MyNewAuthor1 As Object
                        If dt.Rows(0).Item("AUTHOR1").ToString <> "" Then
                            If InStr(dt.Rows(0).Item("AUTHOR1"), ",") <> 0 Then
                                mySplitAuthor1 = TrimAll(dt.Rows(0).Item("AUTHOR1").ToString())
                                mySplitAuthor1 = Split(mySplitAuthor1, ",")
                                SurName1 = mySplitAuthor1(0)
                                ForeName1 = mySplitAuthor1(1)
                                MyNewAuthor1 = Trim(ForeName1) & " " & Trim(SurName1)
                            Else
                                MyNewAuthor1 = TrimAll(dt.Rows(0).Item("AUTHOR1").ToString())
                            End If
                        End If

                        Dim mySplitAuthor2, SurName2, ForeName2, MyNewAuthor2 As Object
                        If dt.Rows(0).Item("AUTHOR2").ToString <> "" Then
                            If InStr(dt.Rows(0).Item("AUTHOR2"), ",") <> 0 Then
                                mySplitAuthor2 = TrimAll(dt.Rows(0).Item("AUTHOR2").ToString())
                                mySplitAuthor2 = Split(mySplitAuthor2, ",")
                                SurName2 = mySplitAuthor2(0)
                                ForeName2 = mySplitAuthor2(1)
                                MyNewAuthor2 = Trim(ForeName2) & " " & Trim(SurName2)
                            Else
                                MyNewAuthor2 = TrimAll(dt.Rows(0).Item("AUTHOR2").ToString())
                            End If
                        End If

                        Dim mySplitAuthor3, SurName3, ForeName3, MyNewAuthor3 As Object
                        If dt.Rows(0).Item("AUTHOR3").ToString <> "" Then
                            If InStr(dt.Rows(0).Item("AUTHOR3"), ",") <> 0 Then
                                mySplitAuthor3 = TrimAll(dt.Rows(0).Item("AUTHOR3").ToString())
                                mySplitAuthor3 = Split(mySplitAuthor3, ",")
                                SurName3 = mySplitAuthor3(0)
                                ForeName3 = mySplitAuthor3(1)
                                MyNewAuthor3 = Trim(ForeName3) & " " & Trim(SurName3)
                            Else
                                MyNewAuthor3 = TrimAll(dt.Rows(0).Item("AUTHOR3").ToString())
                            End If
                        End If

                        Dim myTitle As String = Nothing
                        Dim myRKM As Object = Nothing
                        myTitle = Replace(TrimAll(dt.Rows(0).Item("TITLE").ToString()), ":", ",")
                        myTitle = Replace(myTitle, "...", "--")
                        myTitle = TrimAll(myTitle)
                        myTitle = removeascii(myTitle)

                        If dt.Rows(0).Item("AUTHOR1").ToString <> "" Or dt.Rows(0).Item("CORPORATE_AUTHOR").ToString <> "" Or dt.Rows(0).Item("CONF_NAME").ToString <> "" Or OneThirty = "YES" Then
                            sb.Append("=245  " & "1") '  index the title as an added entry
                            ''count non-filing caharacters - omiited by RKM
                            If Len(myTitle) > 1 Then
                                If TrimAll(dt.Rows(0).Item("TITLE")).Substring(0, 2) = "a " Or TrimAll(dt.Rows(0).Item("TITLE")).Substring(0, 2) = "A " Then
                                    sb.Append("2")
                                    myRKM = "RKM"
                                End If
                            End If
                            If Len(myTitle) > 2 Then
                                If TrimAll(dt.Rows(0).Item("TITLE")).Substring(0, 3) = "an " Or TrimAll(dt.Rows(0).Item("TITLE")).Substring(0, 3) = "An " Then
                                    sb.Append("3")
                                    myRKM = "RKM"
                                End If
                            End If
                            If Len(myTitle) > 3 Then
                                If TrimAll(dt.Rows(0).Item("TITLE")).Substring(0, 4) = "The " Or TrimAll(dt.Rows(0).Item("TITLE")).Substring(0, 4) = "the " Or TrimAll(dt.Rows(0).Item("TITLE")).Substring(0, 4) = "THE " Then
                                    sb.Append("4")
                                    myRKM = "RKM"
                                End If
                            End If
                            If myRKM <> "RKM" Then
                                sb.Append("0")
                            End If
                            sb.Append("$a" & myTitle)
                            If dt.Rows(0).Item("SUB_TITLE").ToString <> "" Then
                                sb.Append(" :$b" & removeascii(TrimAll(dt.Rows(0).Item("SUB_TITLE").ToString())))
                            End If
                            If dt.Rows(0).Item("AUTHOR1").ToString <> "" And dt.Rows(0).Item("AUTHOR2").ToString <> "" And dt.Rows(0).Item("AUTHOR3").ToString <> "" Then
                                sb.Append(" /$cBy " & Trim(MyNewAuthor1) & ", " & MyNewAuthor2 & " and " & MyNewAuthor3)
                                If dt.Rows(0).Item("EDITOR").ToString <> "" Then
                                    sb.Append("; Edited by " & dt.Rows(0).Item("EDITOR").ToString)
                                End If
                                If dt.Rows(0).Item("TRANSLATOR").ToString <> "" Then
                                    sb.Append("; Tr. by " & dt.Rows(0).Item("TRANSLATOR").ToString)
                                End If
                                If dt.Rows(0).Item("ILLUSTRATOR").ToString <> "" Then
                                    sb.Append("; Illus. by " & dt.Rows(0).Item("ILLUSTRATOR").ToString)
                                End If
                            End If
                            If dt.Rows(0).Item("AUTHOR1").ToString <> "" And dt.Rows(0).Item("AUTHOR2").ToString <> "" And dt.Rows(0).Item("AUTHOR3").ToString = "" Then
                                sb.Append(" /$cBy " & Trim(MyNewAuthor1) & ", " & MyNewAuthor2)
                                If dt.Rows(0).Item("EDITOR").ToString <> "" Then
                                    sb.Append("; edited by " & dt.Rows(0).Item("EDITOR").ToString)
                                End If
                                If dt.Rows(0).Item("TRANSLATOR").ToString <> "" Then
                                    sb.Append("; Tr. by " & dt.Rows(0).Item("TRANSLATOR").ToString)
                                End If
                                If dt.Rows(0).Item("ILLUSTRATOR").ToString <> "" Then
                                    sb.Append("; Illus. by " & dt.Rows(0).Item("ILLUSTRATOR").ToString)
                                End If
                            End If
                            If dt.Rows(0).Item("AUTHOR1").ToString <> "" And dt.Rows(0).Item("AUTHOR2").ToString = "" And dt.Rows(0).Item("AUTHOR3").ToString = "" Then
                                sb.Append(" /$cBy " & Trim(MyNewAuthor1))
                                If dt.Rows(0).Item("EDITOR").ToString <> "" Then
                                    sb.Append("; edited by " & dt.Rows(0).Item("EDITOR").ToString)
                                End If
                                If dt.Rows(0).Item("TRANSLATOR").ToString <> "" Then
                                    sb.Append("; Tr. by " & dt.Rows(0).Item("TRANSLATOR").ToString)
                                End If
                                If dt.Rows(0).Item("ILLUSTRATOR").ToString <> "" Then
                                    sb.Append("; Illus. by " & dt.Rows(0).Item("ILLUSTRATOR").ToString)
                                End If
                            End If
                            sb.Append("." & vbCrLf)
                        Else
                            sb.Append("=245  " & "0")  ' do not Index title as an added entry
                            ''count non-filing caharacters - omiited by RKM
                            If TrimAll(dt.Rows(0).Item("TITLE")).Substring(0, 2) = "a " Or TrimAll(dt.Rows(0).Item("TITLE")).Substring(0, 2) = "A " Then
                                sb.Append("2")
                                myRKM = "RKM"
                            End If
                            If TrimAll(dt.Rows(0).Item("TITLE")).Substring(0, 3) = "an " Or TrimAll(dt.Rows(0).Item("TITLE")).Substring(0, 3) = "An " Then
                                sb.Append("3")
                                myRKM = "RKM"
                            End If
                            If TrimAll(dt.Rows(0).Item("TITLE")).Substring(0, 4) = "The " Or TrimAll(dt.Rows(0).Item("TITLE")).Substring(0, 4) = "the " Or TrimAll(dt.Rows(0).Item("TITLE")).Substring(0, 4) = "THE " Then
                                sb.Append("4")
                                myRKM = "RKM"
                            End If
                            If myRKM <> "RKM" Then
                                sb.Append("0")
                            End If
                            sb.Append("$a" & myTitle)
                            If dt.Rows(0).Item("SUB_TITLE").ToString <> "" Then
                                sb.Append(" :$b" & TrimAll(dt.Rows(0).Item("SUB_TITLE").ToString()))
                            End If
                            If dt.Rows(0).Item("AUTHOR1").ToString <> "" And dt.Rows(0).Item("AUTHOR2").ToString <> "" And dt.Rows(0).Item("AUTHOR3").ToString <> "" Then
                                sb.Append(" /$cBy " & Trim(MyNewAuthor1) & ", " & MyNewAuthor2 & " and " & MyNewAuthor3)
                                If dt.Rows(0).Item("EDITOR").ToString <> "" Then
                                    sb.Append("; Edited by " & dt.Rows(0).Item("EDITOR").ToString)
                                End If
                                If dt.Rows(0).Item("TRANSLATOR").ToString <> "" Then
                                    sb.Append("; Tr. by " & dt.Rows(0).Item("TRANSLATOR").ToString)
                                End If
                                If dt.Rows(0).Item("ILLUSTRATOR").ToString <> "" Then
                                    sb.Append("; Illus. by " & dt.Rows(0).Item("ILLUSTRATOR").ToString)
                                End If
                            End If
                            If dt.Rows(0).Item("AUTHOR1").ToString <> "" And dt.Rows(0).Item("AUTHOR2").ToString <> "" And dt.Rows(0).Item("AUTHOR3").ToString = "" Then
                                sb.Append(" /$cBy " & Trim(MyNewAuthor1) & ", " & MyNewAuthor2)
                                If dt.Rows(0).Item("EDITOR").ToString <> "" Then
                                    sb.Append("; edited by " & dt.Rows(0).Item("EDITOR").ToString)
                                End If
                                If dt.Rows(0).Item("TRANSLATOR").ToString <> "" Then
                                    sb.Append("; Tr. by " & dt.Rows(0).Item("TRANSLATOR").ToString)
                                End If
                                If dt.Rows(0).Item("ILLUSTRATOR").ToString <> "" Then
                                    sb.Append("; Illus. by " & dt.Rows(0).Item("ILLUSTRATOR").ToString)
                                End If
                            End If
                            If dt.Rows(0).Item("AUTHOR1").ToString <> "" And dt.Rows(0).Item("AUTHOR2").ToString = "" And dt.Rows(0).Item("AUTHOR3").ToString = "" Then
                                sb.Append(" /$cBy " & Trim(MyNewAuthor1))
                                If dt.Rows(0).Item("EDITOR").ToString <> "" Then
                                    sb.Append("; edited by " & dt.Rows(0).Item("EDITOR").ToString)
                                End If
                                If dt.Rows(0).Item("TRANSLATOR").ToString <> "" Then
                                    sb.Append("; Tr. by " & dt.Rows(0).Item("TRANSLATOR").ToString)
                                End If
                                If dt.Rows(0).Item("ILLUSTRATOR").ToString <> "" Then
                                    sb.Append("; Illus. by " & dt.Rows(0).Item("ILLUSTRATOR").ToString)
                                End If
                            End If
                            sb.Append("." & vbCrLf)
                        End If


                        '246 VAR_TITLE
                        If dt.Rows(0).Item("VAR_TITLE").ToString <> "" Then
                            sb.Append("=246  " & "33" & "$a" & TrimAll(dt.Rows(0).Item("VAR_TITLE").ToString())) ' & vbCrLf
                            sb.Append(vbCrLf)
                        End If
                        '250 Edition Statement
                        If dt.Rows(0).Item("EDITION").ToString <> "" Then
                            Dim myEd As Object
                            myEd = TrimAll(dt.Rows(0).Item("EDITION").ToString)
                            myEd = Replace(myEd, "Edition", "Ed")
                            myEd = Replace(myEd, "Edition.", "Ed")
                            myEd = Replace(myEd, "edition", "Ed")
                            myEd = Replace(myEd, "edition.", "Ed")
                            myEd = Replace(myEd, "First", "1st")
                            myEd = Replace(myEd, "first", "1st")
                            myEd = Replace(myEd, "Seventh", "7th")
                            myEd = Replace(myEd, ".", "")
                            sb.Append("=250  " & "\\" & "$a" & myEd & ".") '& vbCrLf
                            sb.Append(vbCrLf)
                        Else
                            sb.Append("=250  " & "\\" & "$a" & "1st Ed.") ' & vbCrLf
                            sb.Append(vbCrLf)
                        End If

                        '260 Imprint Statement
                        sb.Append("=260  " & "\\")
                        If dt.Rows(0).Item("PLACE_OF_PUB").ToString <> "" Then
                            sb.Append("$a" & TrimAll(dt.Rows(0).Item("PLACE_OF_PUB").ToString()) & " :")
                        Else
                            sb.Append("$a" & "[S.l.] :")
                        End If
                        Dim myPub As Object = Nothing
                        If dt.Rows(0).Item("PUB_NAME").ToString <> "" Then
                            myPub = TrimAll(dt.Rows(0).Item("PUB_NAME").ToString())
                            myPub = TrimAll(Replace(myPub, "a ", "", 1, 2))
                            myPub = TrimAll(Replace(myPub, "A ", "", 1, 2))
                            myPub = TrimAll(Replace(myPub, "an ", "", 1, 3))
                            myPub = TrimAll(Replace(myPub, "An ", "", 1, 3))
                            myPub = TrimAll(Replace(myPub, "AN ", "", 1, 3))
                            myPub = TrimAll(Replace(myPub, "aN ", "", 1, 3))
                            myPub = TrimAll(Replace(myPub, "The ", "", 1, 4))
                            myPub = TrimAll(Replace(myPub, "the ", "", 1, 4))
                            myPub = TrimAll(Replace(myPub, "THe ", "", 1, 4))
                            myPub = TrimAll(Replace(myPub, "THE ", "", 1, 4))
                            myPub = TrimAll(Replace(myPub, "tHe ", "", 1, 4))
                            myPub = TrimAll(Replace(myPub, "tHE ", "", 1, 4))
                            sb.Append("$b" & TrimAll(myPub) & ",")
                        Else
                            sb.Append("$b" & "[S.l.],")
                        End If
                        If dt.Rows(0).Item("YEAR_OF_PUB").ToString <> "" Then
                            sb.Append("$c" & TrimAll(dt.Rows(0).Item("YEAR_OF_PUB").ToString()) & ".")
                        Else
                            sb.Append("$c[" & Today.Year & "?]")
                        End If
                        sb.Append(vbCrLf)

                        '300 pagination
                        If dt.Rows(0).Item("PAGINATION").ToString <> "" Then
                            Dim myPages As Object = Nothing
                            myPages = Replace(TrimAll(dt.Rows(0).Item("PAGINATION").ToString()), ";", ",")
                            sb.Append("=300  " & "\\")
                            If dt.Rows(0).Item("MULTI_VOL").ToString = "N" Then
                                If InStr(dt.Rows(0).Item("PAGINATION").ToString(), "p") = 0 Then
                                    sb.Append("$a" & TrimAll(myPages) + "p.")
                                Else
                                    sb.Append("$a" & TrimAll(myPages) + ".")
                                End If
                                sb.Append(" ;$c00 cm.")
                                If dt.Rows(0).Item("ACC_MAT_CODE").ToString <> "" Then
                                    sb.Append(" ; +$e" & TrimAll(dt.Rows(0).Item("ACC_MAT_CODE").ToString()) + ".")
                                End If
                            Else
                                If InStr(dt.Rows(0).Item("PAGINATION").ToString(), "p") = 0 Then
                                    sb.Append("$a" & TrimAll(myPages) & "p.")
                                    If dt.Rows(0).Item("VOL_NO").ToString() <> "" Then
                                        sb.Append("v.<" & TrimAll(dt.Rows(0).Item("VOL_NO").ToString()) & ">.")
                                    End If
                                Else
                                    sb.Append("$a" & TrimAll(myPages) & "v.<" & TrimAll(dt.Rows(0).Item("VOL_NO").ToString()) & ">.")
                                End If
                                sb.Append(" ;$c00 cm.")
                                If dt.Rows(0).Item("ACC_MAT_CODE").ToString <> "" Then
                                    sb.Append(" ; +$e" & TrimAll(dt.Rows(0).Item("ACC_MAT_CODE").ToString()) + ".")
                                End If
                            End If
                            sb.Append(vbCrLf)
                        Else
                            Continue For
                        End If

                        '4 Series Statement
                        If dt.Rows(0).Item("SERIES_TITLE").ToString <> "" Then
                            sb.Append("=490  ")
                            sb.Append("1\")
                            sb.Append("$a" & TrimAll(dt.Rows(0).Item("SERIES_TITLE").ToString())) ' & vbCrLf
                            sb.Append(vbCrLf)
                        End If

                        '500 Note
                        If dt.Rows(0).Item("NOTE").ToString <> "" Then
                            Dim my500 As Object
                            my500 = TrimAll(dt.Rows(0).Item("NOTE").ToString)
                            my500 = removeascii(my500)
                            If my500.ToString <> "" Then
                                sb.Append("=500  " & "\\")
                                sb.Append("$a" & removeascii(TrimAll(dt.Rows(0).Item("NOTE").ToString())) & ".") ' & vbCrLf
                                sb.Append(vbCrLf)
                            End If
                        End If

                        '520 Summary/abstract
                        If dt.Rows(0).Item("ABSTRACT").ToString <> "" Then
                            sb.Append("=520  " & "3\")
                            sb.Append("$a" & TrimAll(dt.Rows(0).Item("ABSTRACT").ToString()) & ".") ' & vbCrLf
                            sb.Append(vbCrLf)
                        End If

                        '650 SUBJECT - TOPICAL
                        If dt.Rows(0).Item("SUB_NAME").ToString <> "" Then
                            sb.Append("=650  " & "\4")
                            sb.Append("$a" & TrimAll(dt.Rows(0).Item("SUB_NAME").ToString()) & ".") ' & vbCrLf
                            sb.Append(vbCrLf)
                        End If

                        If dt.Rows(0).Item("KEYWORDS").ToString <> "" Then
                            If InStr(dt.Rows(0).Item("KEYWORDS"), ";") <> 0 Then
                                Dim myKeyword As Object
                                myKeyword = TrimAll(dt.Rows(0).Item("KEYWORDS").ToString)
                                myKeyword = Split(myKeyword, ";")
                                Dim m
                                For m = 0 To UBound(myKeyword)
                                    sb.Append("=650  " & "\4")
                                    sb.Append("$a" & TrimAll(myKeyword(m)))
                                    sb.Append(".") ' & vbCrLf
                                    sb.Append(vbCrLf)
                                Next
                            Else
                                sb.Append("=650  " & "\4")
                                sb.Append("$a" & TrimAll(dt.Rows(0).Item("KEYWORDS").ToString))
                                sb.Append(".") ' & vbCrLf
                                sb.Append(vbCrLf)
                            End If
                        End If

                        '700 ADDED ENTRIES - Personal Name
                        If dt.Rows(0).Item("AUTHOR2").ToString <> "" Or dt.Rows(0).Item("AUTHOR3").ToString <> "" Or dt.Rows(0).Item("EDITOR").ToString <> "" Or dt.Rows(0).Item("TRANSLATOR").ToString <> "" Or dt.Rows(0).Item("ILLUSTRATOR").ToString <> "" Then
                            If dt.Rows(0).Item("AUTHOR2").ToString <> "" Then
                                If InStr(dt.Rows(0).Item("AUTHOR2"), ",") <> 0 Then
                                    sb.Append("=700  " & "1\" & "$a" & TrimAll(Replace(dt.Rows(0).Item("AUTHOR2").ToString(), ".", " ")))
                                    sb.Append(",$e" & "Jt.Author")
                                Else
                                    sb.Append("=700  " & "0\" & "$a" & TrimAll(Replace(dt.Rows(0).Item("AUTHOR2").ToString(), ".", " ")))
                                    sb.Append(",$e" & "Jt.Author")
                                End If
                                sb.Append(".") ' & vbCrLf
                                sb.Append(vbCrLf)
                            End If
                            If dt.Rows(0).Item("AUTHOR3").ToString <> "" Then
                                If InStr(dt.Rows(0).Item("AUTHOR3"), ",") <> 0 Then
                                    sb.Append("=700  " & "1\" & "$a" & TrimAll(Replace(dt.Rows(0).Item("AUTHOR3").ToString(), ".", " ")))
                                    sb.Append(",$e" & "Jt.Author")
                                Else
                                    sb.Append("=700  " & "0\" & "$a" & TrimAll(Replace(dt.Rows(0).Item("AUTHOR3").ToString(), ".", " ")))
                                    sb.Append(",$e" & "Jt.Author")
                                End If
                                sb.Append(".") ' & vbCrLf
                                sb.Append(vbCrLf)
                            End If

                            If dt.Rows(0).Item("EDITOR").ToString <> "" Then
                                If InStr(dt.Rows(0).Item("EDITOR"), ";") <> 0 Then
                                    Dim myEditor As Object
                                    myEditor = TrimAll(dt.Rows(0).Item("EDITOR").ToString)
                                    myEditor = Split(myEditor, ";")
                                    Dim m
                                    For m = 0 To UBound(myEditor)
                                        If InStr(myEditor(m), ",") <> 0 Then
                                            sb.Append("=700  " & "1\" & "$a" & TrimAll(Replace((myEditor(m)), ".", " ")))
                                            sb.Append(",$e" & "Ed.") ' & vbCrLf
                                        Else
                                            sb.Append("=700  " & "0\" & "$a" & TrimAll(Replace((myEditor(m)), ".", " ")))
                                            sb.Append(",$e" & "Ed.") ' & vbCrLf
                                        End If
                                        sb.Append(vbCrLf)
                                    Next
                                Else
                                    If InStr(dt.Rows(0).Item("EDITOR"), ",") <> 0 Then
                                        sb.Append("=700  " & "1\" & "$a" & TrimAll(Replace(dt.Rows(0).Item("EDITOR").ToString(), ".", " ")))
                                        sb.Append(",$e" & "Ed.") ' & vbCrLf
                                    Else
                                        sb.Append("=700  " & "0\" & "$a" & TrimAll(Replace(dt.Rows(0).Item("EDITOR").ToString(), ".", " ")))
                                        sb.Append(",$e" & "Ed.") ' & vbCrLf
                                    End If
                                    sb.Append(vbCrLf)
                                End If
                            End If

                            If dt.Rows(0).Item("TRANSLATOR").ToString <> "" Then
                                If InStr(dt.Rows(0).Item("TRANSLATOR"), ";") <> 0 Then
                                    Dim myTr As Object
                                    myTr = TrimAll(dt.Rows(0).Item("TRANSLATOR").ToString)
                                    myTr = Split(myTr, ";")
                                    Dim m
                                    For m = 0 To UBound(myTr)
                                        If InStr(myTr(m), ",") <> 0 Then
                                            sb.Append("=700  " & "1\" & "$a" & TrimAll(Replace((myTr(m)), ".", " ")))
                                            sb.Append(",$e" & "Tr.") ' & vbCrLf
                                        Else
                                            sb.Append("=700  " & "0\" & "$a" & TrimAll(Replace((myTr(m)), ".", " ")))
                                            sb.Append(",$e" & "Tr.") ' & vbCrLf
                                        End If
                                        sb.Append(vbCrLf)
                                    Next
                                Else
                                    If InStr(dt.Rows(0).Item("TRANSLATOR"), ",") <> 0 Then
                                        sb.Append("=700  " & "1\" & "$a" & TrimAll(Replace(dt.Rows(0).Item("TRANSLATOR").ToString(), ".", " ")))
                                        sb.Append(",$e" & "Tr.") ' & vbCrLf
                                    Else
                                        sb.Append("=700  " & "0\" & "$a" & TrimAll(Replace(dt.Rows(0).Item("TRANSLATOR").ToString(), ".", " ")))
                                        sb.Append(",$e" & "Tr.") ' & vbCrLf
                                    End If
                                    sb.Append(vbCrLf)
                                End If
                            End If

                            If dt.Rows(0).Item("ILLUSTRATOR").ToString <> "" Then
                                If InStr(dt.Rows(0).Item("ILLUSTRATOR"), ";") <> 0 Then
                                    Dim myIllus As Object
                                    myIllus = TrimAll(dt.Rows(0).Item("ILLUSTRATOR").ToString)
                                    myIllus = Split(myIllus, ";")
                                    Dim m
                                    For m = 0 To UBound(myIllus)
                                        If InStr(myIllus(m), ",") <> 0 Then
                                            sb.Append("=700  " & "1\" & "$a" & TrimAll(Replace((myIllus(m)), ".", " ")))
                                            sb.Append(",$e" & "Illus.") ' & vbCrLf
                                        Else
                                            sb.Append("=700  " & "0\" & "$a" & TrimAll(Replace((myIllus(m)), ".", " ")))
                                            sb.Append(",$e" & "Illus.") ' & vbCrLf
                                        End If
                                        sb.Append(vbCrLf)
                                    Next
                                Else
                                    If InStr(dt.Rows(0).Item("ILLUSTRATOR"), ",") <> 0 Then
                                        sb.Append("=700  " & "1\" & "$a" & TrimAll(Replace(dt.Rows(0).Item("ILLUSTRATOR").ToString(), ".", " ")))
                                        sb.Append(",$e" & "Illus.") ' & vbCrLf
                                    Else
                                        sb.Append("=700  " & "0\" & "$a" & TrimAll(Replace(dt.Rows(0).Item("ILLUSTRATOR").ToString(), ".", " ")))
                                        sb.Append(",$e" & "Illus.") ' & vbCrLf
                                    End If
                                    sb.Append(vbCrLf)
                                End If
                            End If
                        End If

                        '710 Added entry for Corporate Authro
                        If dt.Rows(0).Item("AUTHOR1").ToString <> "" Then
                            If dt.Rows(0).Item("CORPORATE_AUTHOR").ToString <> "" Then
                                sb.Append("=710  " & "2\" & "$a" & TrimAll(dt.Rows(0).Item("CORPORATE_AUTHOR").ToString()) & ".") ' & vbCrLf
                                sb.Append(vbCrLf)
                            End If
                        End If
                        '711 Added entry for Conference / meeting
                        If dt.Rows(0).Item("AUTHOR1").ToString <> "" Or dt.Rows(0).Item("CORPORATE_AUTHOR").ToString <> "" Then
                            If dt.Rows(0).Item("CONF_NAME").ToString <> "" Then
                                sb.Append("=711  " & "2\" & "$a" & TrimAll(dt.Rows(0).Item("CONF_NAME").ToString()) & ".") ' & vbCrLf
                                sb.Append(vbCrLf)
                            End If
                        End If

                        '800 Series Editor
                        If dt.Rows(0).Item("SERIES_EDITOR").ToString <> "" Then
                            If InStr(dt.Rows(0).Item("SERIES_EDITOR"), ";") <> 0 Then
                                Dim mySEditor As Object
                                mySEditor = TrimAll(dt.Rows(0).Item("SERIES_EDITOR").ToString)
                                mySEditor = Split(mySEditor, ";")
                                Dim m
                                For m = 0 To UBound(mySEditor)
                                    If InStr(mySEditor(m), ",") <> 0 Then
                                        sb.Append("=800  " & "1\" & "$a" & TrimAll(Replace((mySEditor(m)), ".", " ")))
                                        sb.Append(".$t" & TrimAll(dt.Rows(0).Item("SERIES_TITLE").ToString()) & ".") ' & vbCrLf
                                    Else
                                        sb.Append("=800  " & "0\" & "$a" & TrimAll(Replace((mySEditor(m)), ".", " ")))
                                        sb.Append(".$t" & TrimAll(dt.Rows(0).Item("SERIES_TITLE").ToString()) & ".") ' & vbCrLf
                                    End If
                                    sb.Append(vbCrLf)
                                Next
                            Else
                                If InStr(dt.Rows(0).Item("SERIES_EDITOR"), ",") <> 0 Then
                                    sb.Append("=800  " & "1\" & "$a" & TrimAll(Replace(dt.Rows(0).Item("SERIES_EDITOR").ToString(), ".", " ")))
                                    sb.Append(".$t" & TrimAll(dt.Rows(0).Item("SERIES_TITLE").ToString()) & ".") ' & vbCrLf
                                Else
                                    sb.Append("=800  " & "0\" & "$a" & TrimAll(Replace(dt.Rows(0).Item("SERIES_EDITOR").ToString(), ".", " ")))
                                    sb.Append(".$t" & TrimAll(dt.Rows(0).Item("SERIES_TITLE").ToString()) & ".") ' & vbCrLf
                                End If
                                sb.Append(vbCrLf)
                            End If
                        End If

                        '830 Add Series entry
                        If dt.Rows(0).Item("SERIES_TITLE").ToString <> "" Then
                            sb.Append("=830  ")
                            sb.Append("\0")

                            Dim mySer As String = ""
                            mySer = TrimAll(dt.Rows(0).Item("SERIES_TITLE").ToString())
                            mySer = TrimAll(Replace(mySer, "a ", "", 1, 2))
                            mySer = TrimAll(Replace(mySer, "A ", "", 1, 2))
                            mySer = TrimAll(Replace(mySer, "an ", "", 1, 3))
                            mySer = TrimAll(Replace(mySer, "An ", "", 1, 3))
                            mySer = TrimAll(Replace(mySer, "AN ", "", 1, 3))
                            mySer = TrimAll(Replace(mySer, "aN ", "", 1, 3))
                            mySer = TrimAll(Replace(mySer, "The ", "", 1, 4))
                            mySer = TrimAll(Replace(mySer, "the ", "", 1, 4))
                            mySer = TrimAll(Replace(mySer, "THe ", "", 1, 4))
                            mySer = TrimAll(Replace(mySer, "THE ", "", 1, 4))
                            mySer = TrimAll(Replace(mySer, "tHe ", "", 1, 4))
                            mySer = TrimAll(Replace(mySer, "tHE ", "", 1, 4))
                            If Trim(mySer.Substring(mySer.Length - 1)) = ")" Then
                                sb.Append("$a" & mySer) ' & vbCrLf
                            Else
                                sb.Append("$a" & mySer & ".") ' & vbCrLf
                            End If
                            sb.Append(vbCrLf)
                        End If

                        '850 Holding Institution                            
                        sb.Append("=850  " & "\\" & "$a" & Trim(LibCode)) '& vbCrLf
                        sb.Append(vbCrLf)
                        '852 location on shelf  
                        If dt.Rows(0).Item("PHYSICAL_LOCATION").ToString <> "" Then
                            sb.Append("=852  " & "\\" & "$a" & TrimAll(dt.Rows(0).Item("PHYSICAL_LOCATION").ToString)) ' & vbCrLf
                            sb.Append(vbCrLf)
                        End If
                        '856 URL  
                        If dt.Rows(0).Item("URL").ToString <> "" Then
                            sb.Append("=856  " & "4\" & "$u" & TrimAll(dt.Rows(0).Item("URL").ToString)) ' & vbCrLf
                            sb.Append(vbCrLf)
                        End If

                        '901 Hodlings data 
                        If dt.Rows(0).Item("ACCESSION_NO").ToString <> "" Then
                            sb.Append("=901  " & "\\" & "$a" & TrimAll(dt.Rows(0).Item("ACCESSION_NO").ToString))
                            If dt.Rows(0).Item("ACCESSION_DATE").ToString <> "" Then
                                sb.Append("$b" & Format(dt.Rows(0).Item("ACCESSION_DATE"), "yyyyMMdd") + "000000.0") ' & vbCrLf
                            Else
                                sb.Append("$b" & Format(Date.Today, "yyyyMMdd") + "000000.0") ' & vbCrLf
                            End If
                            sb.Append(vbCrLf)
                        End If

                        '902 class no 
                        If dt.Rows(0).Item("CLASS_NO").ToString <> "" Then
                            sb.Append("=902  " & "\\" & "$a" & TrimAll(dt.Rows(0).Item("CLASS_NO").ToString))
                            If dt.Rows(0).Item("BOOK_NO").ToString <> "" Then
                                sb.Append("$b" & dt.Rows(0).Item("BOOK_NO").ToString)
                            End If
                            sb.Append(vbCrLf)
                        End If

                        sb.Replace("..", ".")
                        sb.Append("*")
                        sb.Append(vbCr & vbLf)
                    End If
                    SqlConn.Close()
                End If
            Next
        Catch ex As Exception
            Lbl_Error.Text = "Error: " & (ex.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    Public Sub ConverToMARCCCF_Books()
        Try
            ExportToMARC21DisplaryFormatBooks()

            Dim myRecord = Split(sb.ToString, "*")
            Dim i As Integer
            i = 0
            Dim s, d, l, t, m, rec
            rec = Nothing

            Response.Clear()
            Response.Buffer = True
            Dim FileName As String = "Export_MARC21CCF_Books_" + Format(DateTime.Now, "ddMMyyyy") + ".mrc"
            Response.AddHeader("content-disposition", String.Format("attachment;filename={0}", FileName))
            Response.Charset = ""
            Response.ContentType = "application/text "

            Dim j As Integer = 0
            For j = 0 To UBound(myRecord)
                If Len(myRecord(j)) <> 2 Then
                    Dim myLine = Split(myRecord(j), vbCrLf)
                    Dim k As Integer = 0
                    s = Nothing
                    l = Nothing
                    t = Nothing
                    d = Nothing
                    m = Nothing
                    For k = 0 To UBound(myLine)
                        If Len(myLine(k)) <> 0 Then
                            Dim myVarLine As Object = Nothing
                            myVarLine = myLine(k)
                            If myVarLine <> "" And myVarLine.ToString.Substring(0, 1) = "=" Then
                                Dim a, t1
                                If myVarLine.ToString.Substring(0, 4) <> "=LDR" Then
                                    t1 = Trim(myVarLine.ToString.Substring(1, 3))
                                    a = Trim(myVarLine.ToString.Substring(6))
                                    s = Convert.ToInt32(s) + Convert.ToInt32(l)
                                    s = Lfive(s)
                                    l = Len(a) + 1
                                    l = Lfour(l)
                                    t1 = t1 + l + s
                                    t = t + t1
                                    d = d + a + Chr(30)
                                    d = Replace(d, "\", Chr(32))
                                Else
                                    a = Trim(myVarLine.ToString.Substring(6))
                                    t = Replace(a, "\", Chr(32))
                                End If
                            End If
                        End If
                    Next
                    If Trim(d) <> "" Then
                        t = t + Chr(30)
                        m = t + d + Chr(29)
                        Dim rlen, badd
                        rlen = Len(m)
                        rlen = Lfive(rlen)
                        badd = Len(t)
                        badd = Lfive(badd)
                        m = m.ToString.Remove(0, 5)
                        m = rlen + m
                        m = m.ToString.Remove(12, 5)
                        m = m.ToString.Insert(12, badd)
                        m = Replace(m, "$", Chr(31))
                        rec = rec + m
                    End If
                End If
            Next

            Response.Output.Write(rec.ToString())
            Response.Flush()
            Response.End()
        Catch ex As Exception
            Lbl_Error.Text = "Error: " & (ex.Message())
        Finally

        End Try
    End Sub
    Public Sub ConverToMARCCCF_Serials()
        Try
            ExportToMARC21DisplaryFormatSerieals()

            Dim myRecord = Split(sb.ToString, "*")
            Dim i As Integer
            i = 0
            Dim s, d, l, t, m, rec
            rec = Nothing

            Response.Clear()
            Response.Buffer = True
            Dim FileName As String = "Export_MARC21CCF_Serials_" + Format(DateTime.Now, "ddMMyyyy") + ".mrc"
            Response.AddHeader("content-disposition", String.Format("attachment;filename={0}", FileName))
            Response.Charset = ""
            Response.ContentType = "application/text "

            Dim j As Integer = 0
            For j = 0 To UBound(myRecord)
                If Len(myRecord(j)) <> 2 Then
                    Dim myLine = Split(myRecord(j), vbCrLf)
                    Dim k As Integer = 0
                    s = Nothing
                    l = Nothing
                    t = Nothing
                    d = Nothing
                    m = Nothing
                    For k = 0 To UBound(myLine)
                        If Len(myLine(k)) <> 0 Then
                            Dim myVarLine As Object = Nothing
                            myVarLine = myLine(k)
                            If myVarLine <> "" And myVarLine.ToString.Substring(0, 1) = "=" Then
                                Dim a, t1
                                If myVarLine.ToString.Substring(0, 4) <> "=LDR" Then
                                    t1 = Trim(myVarLine.ToString.Substring(1, 3))
                                    a = Trim(myVarLine.ToString.Substring(6))
                                    s = Convert.ToInt32(s) + Convert.ToInt32(l)
                                    s = Lfive(s)
                                    l = Len(a) + 1
                                    l = Lfour(l)
                                    t1 = t1 + l + s
                                    t = t + t1
                                    d = d + a + Chr(30)
                                    d = Replace(d, "\", Chr(32))
                                Else
                                    a = Trim(myVarLine.ToString.Substring(6))
                                    t = Replace(a, "\", Chr(32))
                                End If
                            End If
                        End If
                    Next
                    If Trim(d) <> "" Then
                        t = t + Chr(30)
                        m = t + d + Chr(29)
                        Dim rlen, badd
                        rlen = Len(m)
                        rlen = Lfive(rlen)
                        badd = Len(t)
                        badd = Lfive(badd)
                        m = m.ToString.Remove(0, 5)
                        m = rlen + m
                        m = m.ToString.Remove(12, 5)
                        m = m.ToString.Insert(12, badd)
                        m = Replace(m, "$", Chr(31))
                        rec = rec + m
                    End If
                End If
            Next

            Response.Output.Write(rec.ToString())
            Response.Flush()
            Response.End()
        Catch ex As Exception
            Lbl_Error.Text = "Error: " & (ex.Message())
        Finally

        End Try
    End Sub
    Public Function Lfour(ByVal l As Integer) As String
        Dim s1, s2 As String
        s1 = "0000"
        s1 = l.ToString
        s2 = s1.Length
        If s2 = 1 Then
            s1 = "000" + l.ToString
        ElseIf s2 = 2 Then
            s1 = "00" + l.ToString
        ElseIf s2 = 3 Then
            s1 = "0" + l.ToString
        ElseIf s2 = 4 Then
            s1 = l.ToString
        Else
            MsgBox("Length should be 4-digit")
        End If
        Return s1
    End Function
    Public Function Lfive(ByVal l As Integer) As String
        Dim s1, s2 As String
        s1 = "00000"
        s1 = l.ToString
        s2 = s1.Length
        If s2 = 1 Then
            s1 = "0000" + l.ToString
        ElseIf s2 = 2 Then
            s1 = "000" + l.ToString
        ElseIf s2 = 3 Then
            s1 = "00" + l.ToString
        ElseIf s2 = 4 Then
            s1 = "0" + l.ToString
        ElseIf s2 = 5 Then
            s1 = l.ToString
        Else
            MsgBox("Length should be 5-digit")
        End If
        Return s1
    End Function
    Public Sub ConvertToISO2709_Serials()
        ExportToMARC21DisplaryFormatSerieals()

        Dim myRecord = Split(sb.ToString, "*")
        Dim i As Integer
        i = 0
        Dim s, d, l, t, m, rec
        rec = Nothing

        Response.Clear()
        Response.Buffer = True
        Dim FileName As String = "Export_ISO2709_Serials_" + Format(DateTime.Now, "ddMMyyyy") + ".mrc"
        Response.AddHeader("content-disposition", String.Format("attachment;filename={0}", FileName))
        Response.Charset = ""
        Response.ContentType = "application/text "

        Dim j As Integer = 0
        For j = 0 To UBound(myRecord)
            If Len(myRecord(j)) <> 2 Then
                Dim myLine = Split(myRecord(j), vbCrLf)
                Dim k As Integer = 0
                s = Nothing
                l = Nothing
                t = Nothing
                d = Nothing
                m = Nothing
                For k = 0 To UBound(myLine)
                    If Len(myLine(k)) <> 0 Then
                        Dim myVarLine As Object = Nothing
                        myVarLine = myLine(k)
                        If myVarLine <> "" Then
                            Dim a, t1
                            If myVarLine.ToString.Substring(0, 4) <> "=LDR" Then
                                t1 = Trim(myVarLine.ToString.Substring(1, 3)) 'tag
                                a = Trim(myVarLine.ToString.Substring(6)) 'data
                                s = Convert.ToInt32(s) + Convert.ToInt32(l)
                                s = Lfive(s) 'starting position
                                l = Len(a) + 1
                                l = Lfour(l) 'length of the feild
                                t1 = t1 + l + s
                                t = t + t1 'directory entry
                                d = d + a + Chr(30) '"#" 'Chr(30) 'add field separator ASCII(30)
                                d = Replace(d, "#", Chr(32))
                            Else
                                a = Trim(myVarLine.ToString.Substring(6))
                                t = a
                            End If
                        End If
                    End If
                Next

                t = t + Chr(30) '"#" 'Chr(30) 'Field Separator ASCII(30)-Record Separator
                m = t + d + Chr(29) 'Chr(29) 'Record Separator ASCII(29)-Group Separator
                Dim rlen, badd
                rlen = Len(m)
                rlen = Lfive(rlen)
                badd = Len(t)
                badd = Lfive(badd)
                m = m.ToString.Remove(0, 5)
                m = rlen + m
                m = m.ToString.Remove(12, 5)
                m = m.ToString.Insert(12, badd)
                rec = rec + m + Chr(13) + Chr(10)
            End If
        Next
        Response.Output.Write(rec.ToString())
        Response.Flush()
        Response.End()
    End Sub
    Public Sub ConvertToISO2709_Books()

        ExportToMARC21DisplaryFormatBooks()

        Dim myRecord = Split(sb.ToString, "*")
        Dim i As Integer
        i = 0
        Dim s, d, l, t, m, rec
        rec = Nothing

        Response.Clear()
        Response.Buffer = True
        Dim FileName As String = "Export_ISO2709_Books_" + Format(DateTime.Now, "ddMMyyyy") + ".mrc"
        Response.AddHeader("content-disposition", String.Format("attachment;filename={0}", FileName))
        Response.Charset = ""
        Response.ContentType = "application/text "

        Dim j As Integer = 0
        For j = 0 To UBound(myRecord)
            If Len(myRecord(j)) <> 2 Then
                Dim myLine = Split(myRecord(j), vbCrLf)
                Dim k As Integer = 0
                s = Nothing
                l = Nothing
                t = Nothing
                d = Nothing
                m = Nothing
                For k = 0 To UBound(myLine)
                    If Len(myLine(k)) <> 0 Then
                        Dim myVarLine As Object = Nothing
                        myVarLine = myLine(k)
                        If myVarLine <> "" Then
                            Dim a, t1
                            If myVarLine.ToString.Substring(0, 4) <> "=LDR" Then
                                t1 = Trim(myVarLine.ToString.Substring(1, 3)) 'tag
                                a = Trim(myVarLine.ToString.Substring(6)) 'data
                                s = Convert.ToInt32(s) + Convert.ToInt32(l)
                                s = Lfive(s) 'starting position
                                l = Len(a) + 1
                                l = Lfour(l) 'length of the feild
                                t1 = t1 + l + s
                                t = t + t1 'directory entry
                                d = d + a + Chr(30) '"#" 'Chr(30) 'add field separator ASCII(30)
                                d = Replace(d, "#", Chr(32))
                            Else
                                a = Trim(myVarLine.ToString.Substring(6))
                                t = a
                            End If
                        End If
                    End If
                Next

                t = t + Chr(30) '"#" 'Chr(30) 'Field Separator ASCII(30)-Record Separator
                m = t + d + Chr(29) 'Chr(29) 'Record Separator ASCII(29)-Group Separator
                Dim rlen, badd
                rlen = Len(m)
                rlen = Lfive(rlen)
                badd = Len(t)
                badd = Lfive(badd)
                m = m.ToString.Remove(0, 5)
                m = rlen + m
                m = m.ToString.Remove(12, 5)
                m = m.ToString.Insert(12, badd)
                rec = rec + m + Chr(13) + Chr(10)
            End If
        Next
        Response.Output.Write(rec.ToString())
        Response.Flush()
        Response.End()
    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        ' Verifies that the control is rendered 
    End Sub
    Public Sub ExportTo_excel_Serials()

        Dim strFilename As String = "Export_MARC21DF_Books_" + Format(DateTime.Now, "ddMMyyyy") + ".xls" '"ExportToExcel.xls"
        Dim attachment As String = "attachment; filename=" & strFilename
        Response.ClearContent()
        Response.AddHeader("content-disposition", attachment)
        Response.ContentType = "application/vnd.ms-excel"

        Dim Flag As Object = Nothing
        Dim dt As DataTable = Nothing
        For Each row As GridViewRow In Grid1.Rows
            Dim cb As CheckBox = DirectCast(row.FindControl("cbd"), CheckBox)
            If cb IsNot Nothing AndAlso cb.Checked = True Then
                Dim HOLD_ID As Integer = Convert.ToInt32(Grid1.DataKeys(row.RowIndex).Value)
                Dim SQL As String = Nothing
                SQL = "SELECT HOLDINGS.*, CATS_AUTHORS_VIEW.LANG_CODE, CATS_AUTHORS_VIEW.BIB_CODE, CATS_AUTHORS_VIEW.MAT_CODE, CATS_AUTHORS_VIEW.DOC_TYPE_CODE, CATS_AUTHORS_VIEW.STANDARD_NO, CATS_AUTHORS_VIEW.TITLE, CATS_AUTHORS_VIEW.SUB_TITLE, CATS_AUTHORS_VIEW.VAR_TITLE, CATS_AUTHORS_VIEW.CORPORATE_AUTHOR, CATS_AUTHORS_VIEW.EDITOR, CATS_AUTHORS_VIEW.PLACE_OF_PUB, CATS_AUTHORS_VIEW.URL, CATS_AUTHORS_VIEW.PUB_NAME, CATS_AUTHORS_VIEW.PUB_PLACE,CATS_AUTHORS_VIEW.SUB_NAME, J_HISTORY.CODEN, J_HISTORY.J_START_VOL, J_HISTORY.J_START_ISSUE, J_HISTORY.J_START_MONTH, J_HISTORY.J_START_YEAR, J_HISTORY.FREQ_CODE FROM HOLDINGS INNER JOIN CATS_AUTHORS_VIEW ON HOLDINGS.CAT_NO = CATS_AUTHORS_VIEW.CAT_NO CROSS JOIN J_HISTORY WHERE (HOLDINGS.LIB_CODE = '" & Trim(LibCode) & "') AND (HOLDINGS.HOLD_ID ='" & Trim(HOLD_ID) & "');"
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
            End If
        Next
        Response.Flush()
        Response.End()
    End Sub
    Public Sub ExportTo_excel_Books()

        Dim strFilename As String = "Export_MARC21DF_Books_" + Format(DateTime.Now, "ddMMyyyy") + ".xls" '"ExportToExcel.xls"
        Dim attachment As String = "attachment; filename=" & strFilename
        Response.ClearContent()
        Response.AddHeader("content-disposition", attachment)
        Response.ContentType = "application/vnd.ms-excel"

        Dim Flag As Object = Nothing
        Dim dt As DataTable = Nothing
        For Each row As GridViewRow In Grid1.Rows
            Dim cb As CheckBox = DirectCast(row.FindControl("cbd"), CheckBox)
            If cb IsNot Nothing AndAlso cb.Checked = True Then
                Dim HOLD_ID As Integer = Convert.ToInt32(Grid1.DataKeys(row.RowIndex).Value)
                Dim SQL As String = Nothing
                SQL = "SELECT * FROM BOOKS_ACC_REGISTER_VIEW WHERE (LIB_CODE = '" & Trim(LibCode) & "') AND (HOLD_ID ='" & Trim(HOLD_ID) & "');"
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
            End If
        Next
        Response.Flush()
        Response.End()
    End Sub
    Private Sub releaseObject(ByVal obj As Object)
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            obj = Nothing
        Catch ex As Exception
            obj = Nothing
            ' MessageBox.Show("Exception Occured while releasing object " & ex.ToString())
        Finally
            GC.Collect()
        End Try
    End Sub

    Public Sub Print()
        Try
            'dt = GetTableData("View_Item", SQLString, SQLOrderByClause)
            'dt = Your DataTable 
            Dim dt As New DataTable
            dt = (ViewState("dt"))
            Dim oRpt As New ReportDocument
            'oRpt = New YourReportName
            oRpt.SetDataSource(dt)
            'View_PickingSlip.ReportSource = oRpt
            Dim exp As ExportOptions
            Dim req As ExportRequestContext
            Dim st As System.IO.Stream
            Dim b() As Byte
            Dim pg As Page
            ' pg = View_PickingSlip.Page
            exp = New ExportOptions
            exp.ExportFormatType = ExportFormatType.PortableDocFormat
            exp.FormatOptions = New PdfRtfWordFormatOptions
            req = New ExportRequestContext
            req.ExportInfo = exp
            With oRpt.FormatEngine.PrintOptions
                '.PaperSize = PaperSize.PaperLegal
                .PaperOrientation = PaperOrientation.Landscape
            End With
            st = oRpt.FormatEngine.ExportToStream(req)
            pg.Response.ClearHeaders()
            pg.Response.ClearContent()
            pg.Response.ContentType = "application/pdf"
            ReDim b(st.Length)
            st.Read(b, 0, CInt(st.Length))
            pg.Response.BinaryWrite(b)
            pg.Response.End()
            dt.Dispose()
        Catch ex As Exception
            ' ShowError(ex.Message)
        End Try

    End Sub

    'Print Report
    Protected Sub Print_Compact_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Print_Compact_Bttn.Click
        Try
            'THSE LINES FOR DISPLAYING FILE AUTOMATICALLY (NOT AS ATTACHMENT)
            'Dim report As New ReportDocument
            'report.Load(Server.MapPath("~/Reports/CrystalReport1.rpt"))
            'report.SetDataSource(ViewState("dt"))
            'Dim oStream As MemoryStream
            'oStream = DirectCast(report.ExportToStream(CrystalDecisions.[Shared].ExportFormatType.PortableDocFormat), MemoryStream)
            'Response.Clear()
            'Response.Buffer = True
            'Response.ContentType = "application/pdf"
            'Response.BinaryWrite(oStream.ToArray())
            'Response.Flush()
            'Response.End()
            'report.Close()
            'report.Dispose()


            Dim Reportdoc As New ReportDocument
            Reportdoc = Report_Load_Compact()

            If DDL_PrintFormats.SelectedValue <> "" Then
                If DDL_PrintFormats.SelectedValue = "PDF" Then
                    Reportdoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "Report_Export_Compact_" + Format(DateTime.Now, "ddMMyyyy"))
                End If
                If DDL_PrintFormats.SelectedValue = "DOC" Then
                    Reportdoc.ExportToHttpResponse(ExportFormatType.WordForWindows, Response, True, "Report_Export_Compact_" + Format(DateTime.Now, "ddMMyyyy"))
                End If
                If DDL_PrintFormats.SelectedValue = "EXCEL" Then
                    Reportdoc.ExportToHttpResponse(ExportFormatType.Excel, Response, True, "Report_Export_Compact_" + Format(DateTime.Now, "ddMMyyyy"))
                End If
            Else
                Reportdoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "Report_Export_Compact_" + Format(DateTime.Now, "ddMMyyyy"))
            End If
        Catch ex As Exception
            Lbl_Error.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    Public Function Report_Load_Compact() As ReportDocument
        Dim Rpt As New ReportDocument
        Dim dtLibrary As New DataTable
        dtLibrary = (ViewState("dt"))

        If RB_Monographs.Checked = True Then
            Rpt.Load(Server.MapPath("~/Reports/Holdings_Books_Compact_Report.rpt"))
        Else
            Rpt.Load(Server.MapPath("~/Reports/Holdings_Serials_Compact_Report.rpt"))
        End If
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
            myGroupName = Rpt.ReportDefinition.Sections("ReportHeaderSection2").ReportObjects.Item("Text4")
            myGroupName.Text = "Group By: " & DDL_GroupBy.SelectedItem().Text
        Else
            Rpt.ReportDefinition.Sections("ReportHeaderSection2").SectionFormat.EnableSuppress = True
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
    Protected Sub Print_Summary_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Print_Summary_Bttn.Click
        Try

            Dim Reportdoc As New ReportDocument
            Reportdoc = Report_Load_Summary()

            If DDL_PrintFormats.SelectedValue <> "" Then
                If DDL_PrintFormats.SelectedValue = "PDF" Then
                    Reportdoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "Report_Export_Summary_" + Format(DateTime.Now, "ddMMyyyy"))
                End If
                If DDL_PrintFormats.SelectedValue = "DOC" Then
                    Reportdoc.ExportToHttpResponse(ExportFormatType.WordForWindows, Response, True, "Report_Export_Summary_" + Format(DateTime.Now, "ddMMyyyy"))
                End If
                If DDL_PrintFormats.SelectedValue = "HTML" Then

                End If
            Else
                Reportdoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "Report_Export_Summary_" + Format(DateTime.Now, "ddMMyyyy"))
            End If
        Catch ex As Exception
            Lbl_Error.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    Public Function Report_Load_Summary() As ReportDocument
        Dim Rpt As New ReportDocument
        Dim dtLibrary As New DataTable
        dtLibrary = (ViewState("dt"))

        Rpt.Load(Server.MapPath("~/Reports/Holdings_Summary_Report.rpt"))
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
            myGroupName = Rpt.ReportDefinition.Sections("ReportHeaderSection1").ReportObjects.Item("Text17")
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
    'details report  
    Protected Sub Print_Detail_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Print_Detail_Bttn.Click
        Dim Reportdoc As New ReportDocument
        Reportdoc = Report_Load_Detail()
        Try
            If DDL_PrintFormats.SelectedValue <> "" Then
                If DDL_PrintFormats.SelectedValue = "PDF" Then
                    Reportdoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "Report_Export_Detail_" + Format(DateTime.Now, "ddMMyyyy"))
                End If
                If DDL_PrintFormats.SelectedValue = "DOC" Then
                    Reportdoc.ExportToHttpResponse(ExportFormatType.WordForWindows, Response, True, "Report_Export_Detail_" + Format(DateTime.Now, "ddMMyyyy"))
                End If
                If DDL_PrintFormats.SelectedValue = "HTML" Then

                End If
            Else
                Reportdoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "Report_Export_Detail_" + Format(DateTime.Now, "ddMMyyyy"))
            End If
        Catch ex As Exception
            Lbl_Error.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    Public Function Report_Load_Detail() As ReportDocument
        Dim Rpt As New ReportDocument
        Dim dtLibrary As New DataTable
        dtLibrary = (ViewState("dt"))

        Rpt.Load(Server.MapPath("~/Reports/Holdings_Books_Full_Report.rpt"))
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
            myGroupName = Rpt.ReportDefinition.Sections("ReportHeaderSection2").ReportObjects.Item("Text4")
            myGroupName.Text = "Group By: " & DDL_GroupBy.SelectedItem().Text
        Else
            Rpt.ReportDefinition.Sections("ReportHeaderSection2").SectionFormat.EnableSuppress = True
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