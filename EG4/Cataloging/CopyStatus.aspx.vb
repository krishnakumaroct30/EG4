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
Imports EG4.PopulateCombo
Imports System.Drawing
Imports Microsoft.Office.Interop.Excel

Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports.Engine.TextObject
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Shared
Imports System.Drawing.Printing
Imports CrystalDecisions

Public Class CopyStatus
    Inherits System.Web.UI.Page
    Dim index As Integer = 0
    Public Srno As Integer = 0
    Dim myStartSN As Long = Nothing
    Dim myEndSN As Long = Nothing
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session.Item("IsThisU") <> Nothing Then
                If Not SConnection() = True Then
                    Label15.Text = ""
                    Label6.Text = "Database Connection is Broken!"
                Else
                    LibCode = Session.Item("LoggedLibcode")
                    If Page.IsPostBack = False Then
                        PopulateAcqModes()
                        PopulateCurrentStatus()
                        PopulateNewStatus()
                        PopulateSections()
                        PopulateLocation()
                        PopulateClassNo()
                        RadioButton6.Checked = True
                        RadioButton6.Focus()
                    End If
                    Dim CreateSUserButton As Object 'System.Web.UI.WebControls.Button
                    CreateSUserButton = Master.FindControl("Accordion1").FindControl("CatPane").FindControl("Cat_Status_Bttn")
                    CreateSUserButton.ForeColor = Drawing.Color.Red
                    myPaneName = "CatPane"
                End If
            Else
                Response.Redirect("~/Default.aspx", False)
            End If
            LibCode = Session.Item("LoggedLibcode")
        Catch ex As Exception
            Label15.Text = ""
            Label6.Text = "Error in Loading Form!"
        End Try
    End Sub
    Private Function ColumnEqual(ByVal A As Object, ByVal B As Object) As Boolean
        If A Is DBNull.Value And B Is DBNull.Value Then Return True ' Both are DBNull.Value.
        If A Is DBNull.Value Or B Is DBNull.Value Then Return False ' Only one is DBNull.Value.
        Return A = B                                                ' Value type standard comparison
    End Function
    'grid view page index changing event
    Protected Sub Grid2_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles Grid2.PageIndexChanging
        Try
            'rebind datagrid
            Grid2.DataSource = ViewState("dt") 'temp
            Grid2.PageIndex = e.NewPageIndex
            index = e.NewPageIndex * Grid2.PageSize
            Grid2.DataBind()
        Catch s As Exception
            Label16.Text = "Error:  there is error in page index !"
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
    Public Sub PopulateAcqModes()
        Me.DDL_AcqModes.DataTextField = "ACQMODE_NAME"
        Me.DDL_AcqModes.DataValueField = "ACQMODE_CODE"
        Me.DDL_AcqModes.DataSource = GetAcqModesList()
        Me.DDL_AcqModes.DataBind()
    End Sub
    Public Sub PopulateCurrentStatus()
        DDL_CurrentStatus.DataTextField = "STA_NAME"
        DDL_CurrentStatus.DataValueField = "STA_CODE"
        DDL_CurrentStatus.DataSource = GetCopyStatusList()
        DDL_CurrentStatus.DataBind()
        ' DDL_CurrentStatus.Items.Insert(0, "")
    End Sub
    Public Sub PopulateNewStatus()
        DDL_NewStatus.DataTextField = "STA_NAME"
        DDL_NewStatus.DataValueField = "STA_CODE"
        DDL_NewStatus.DataSource = GetCopyStatusList()
        DDL_NewStatus.DataBind()
        ' DDL_NewStatus.Items.Insert(0, "")
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
            ' dt.Rows.InsertAt(Dr, 0)

            If dt.Rows.Count = 0 Then
                Me.DDL_Section1.DataSource = Nothing
            Else
                Me.DDL_Section1.DataSource = dt
                Me.DDL_Section1.DataTextField = "SEC_NAME"
                Me.DDL_Section1.DataValueField = "SEC_CODE"
                Me.DDL_Section1.DataBind()
            End If
            dt.Dispose()
            SqlConn.Close()
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
            Label15.Text = ""
        Finally
            dt.Dispose()
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
            ' dt.Rows.InsertAt(Dr, 0)

            If dt.Rows.Count = 0 Then
                Me.DDL_Location.DataSource = Nothing
            Else
                Me.DDL_Location.DataSource = dt
                Me.DDL_Location.DataTextField = "PHYSICAL_LOCATION"
                Me.DDL_Location.DataValueField = "PHYSICAL_LOCATION"
                Me.DDL_Location.DataBind()
            End If
            dt.Dispose()
            SqlConn.Close()
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
            Label15.Text = ""
        Finally
            dt.Dispose()
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
            ' dt.Rows.InsertAt(Dr, 0)

            If dt.Rows.Count = 0 Then
                Me.DDL_ClassNo.DataSource = Nothing
            Else
                Me.DDL_ClassNo.DataSource = dt
                Me.DDL_ClassNo.DataTextField = "CLASS_NO"
                Me.DDL_ClassNo.DataValueField = "CLASS_NO"
                Me.DDL_ClassNo.DataBind()
            End If
            dt.Dispose()
            SqlConn.Close()
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
            Label15.Text = ""
        Finally
            dt.Dispose()
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
        DDL_CurrentStatus.Visible = False
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
        DDL_CurrentStatus.Visible = False
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
        DDL_CurrentStatus.Visible = False
        DDL_Location.Visible = False
        txt_Status_RandomAccession.Visible = False
    End Sub
    Protected Sub RadioButton1_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles RadioButton1.CheckedChanged
        DDL_AcqModes.Visible = True
        DDL_AcqModes.Focus()

        DDL_Section1.Visible = False
        DDL_ClassNo.Visible = False
        DDL_CurrentStatus.Visible = False
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
        DDL_CurrentStatus.Visible = False
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
        DDL_CurrentStatus.Visible = False
        DDL_Location.Visible = False
        txt_Status_RandomAccession.Visible = False
        txt_Status_AccessionFrom.Visible = False
        txt_Status_AccessionTo.Visible = False
        txt_Status_AccDateFrom.Visible = False
        txt_Status_AccDateTo.Visible = False
    End Sub
    Protected Sub RadioButton2_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles RadioButton2.CheckedChanged
        DDL_CurrentStatus.Visible = True
        DDL_CurrentStatus.Focus()

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

        DDL_CurrentStatus.Visible = False
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
        DDL_CurrentStatus.Visible = False
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
                    Label6.Text = "Plz enter both the value"
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
                        Label6.Text = "Enter proper Date in dd/MM/yyyy format"
                        Label15.Text = ""
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
                        Label6.Text = "Enter proper Date in dd/MM/yyyy format"
                        Label15.Text = ""
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
                If DDL_CurrentStatus.Text <> "" Then
                    STATUS = DDL_CurrentStatus.SelectedValue
                    STATUS = RemoveQuotes(STATUS)
                Else
                    STATUS = Nothing
                End If
            End If

            Dim SQL As String = Nothing
            'SQL = "SELECT HOLD_ID, CAT_NO, ACQ_ID, ACCESSION_NO, ACCESSION_DATE, VOL_NO, CLASS_NO, BOOK_NO, PAGINATION, PHYSICAL_LOCATION, STA_CODE, COLLECTION_TYPE, LIB_CODE, TITLE = CASE (ISNULL(SUB_TITLE, '')) WHEN '' THEN TITLE ELSE (TITLE + ': '+SUB_TITLE) END  FROM BOOKS_ACC_REGISTER_VIEW WHERE (LIB_CODE = '" & Trim(LibCode) & "') AND (BIB_CODE='" & Trim(myMaterial) & "') "
            SQL = "SELECT HOLD_ID, CAT_NO, ACQ_ID, ACCESSION_NO, ACCESSION_DATE, VOL_NO, CLASS_NO, BOOK_NO, PAGINATION, PHYSICAL_LOCATION, BOOKS_ACC_REGISTER_VIEW.STA_CODE, COLLECTION_TYPE, BOOKS_ACC_REGISTER_VIEW.LIB_CODE, TITLE = CASE (ISNULL(SUB_TITLE, '')) WHEN '' THEN TITLE ELSE (TITLE + ': '+SUB_TITLE) END,  BOOKSTATUS.STA_NAME FROM BOOKS_ACC_REGISTER_VIEW INNER JOIN BOOKSTATUS ON BOOKS_ACC_REGISTER_VIEW.STA_CODE = BOOKSTATUS.STA_CODE WHERE (BOOKS_ACC_REGISTER_VIEW.LIB_CODE = '" & Trim(LibCode) & "') AND (BOOKS_ACC_REGISTER_VIEW.BIB_CODE='" & Trim(myMaterial) & "') "

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
                Me.Grid2.DataSource = Nothing
                Grid2.DataBind()
                Label1.Text = "Total Record(s): 0 "
                Status_Update_Bttn.Enabled = False
                Print_Compact_Bttn.Visible = False
                Print_Summary_Bttn.Visible = False
                Print_Detail_Bttn.Visible = False
            Else
                Grid2.Visible = True
                RecordCount = dtSearch.Rows.Count
                Grid2.DataSource = dtSearch
                Grid2.DataBind()
                Label1.Text = "Total Record(s): " & RecordCount
                Status_Update_Bttn.Enabled = True
                Print_Compact_Bttn.Visible = True
                Print_Summary_Bttn.Visible = True
                Print_Detail_Bttn.Visible = True
            End If
            ViewState("dt") = dtSearch
        Catch s As Exception
            Label16.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try

    End Sub
    'get RowNuber for range of accno
    Public Sub GetAccFromRowNumber(ByVal AccFrom As Object)
        Dim dtAcc As DataTable = Nothing
        Try
            ' Dim x As Integer = 0
            ' For x = 0 To 100
            'Dim myNextAccNo As Object = Nothing
            'If x = 0 Then
            '    myAccNO = myAccNO + 1
            '    myNextAccNo = ACCESSION_SERIES + Convert.ToString(myAccNO) 'TrimX(txt_Hold_AccNo.Text)
            'Else
            '    myAccNO = myAccNO + 1
            '    myNextAccNo = ACCESSION_SERIES + Convert.ToString(myAccNO) ' + 1)
            'End If


            'check duplicate acc no
            Dim str2 As Object = Nothing
            Dim flag2 As Long = Nothing
            str2 = "SELECT ROWNUMBER FROM BOOKS_ACC_REGISTER_VIEW WHERE (ACCESSION_NO = '" & Trim(AccFrom) & "')  AND (LIB_CODE = '" & Trim(LibCode) & "')"
            Dim cmd2 As New SqlCommand(str2, SqlConn)
            SqlConn.Open()
            flag2 = cmd2.ExecuteScalar
            SqlConn.Close()

            If flag2 = Nothing Then
                'AccFrom = AccFrom + 1
                'Continue For
                myStartSN = Nothing
            Else
                myStartSN = Convert.ToInt16(flag2)
            End If
            'Next
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
            ' Dim x As Integer = 0
            ' For x = 0 To 100
            'Dim myNextAccNo As Object = Nothing
            'If x = 0 Then
            '    myAccNO = myAccNO + 1
            '    myNextAccNo = ACCESSION_SERIES + Convert.ToString(myAccNO) 'TrimX(txt_Hold_AccNo.Text)
            'Else
            '    myAccNO = myAccNO + 1
            '    myNextAccNo = ACCESSION_SERIES + Convert.ToString(myAccNO) ' + 1)
            'End If


            'check duplicate acc no
            Dim str2 As Object = Nothing
            Dim flag2 As Long = Nothing
            str2 = "SELECT ROWNUMBER FROM BOOKS_ACC_REGISTER_VIEW WHERE (ACCESSION_NO = '" & Trim(AccTo) & "')  AND (LIB_CODE = '" & Trim(LibCode) & "')"
            Dim cmd2 As New SqlCommand(str2, SqlConn)
            SqlConn.Open()
            flag2 = cmd2.ExecuteScalar
            SqlConn.Close()

            If flag2 = Nothing Then
                'AccFrom = AccFrom + 1
                'Continue For
                myEndSN = Nothing
            Else
                myEndSN = Convert.ToInt16(flag2)
            End If
            'Next
        Catch ex As Exception
            Label6.Text = ex.ToString
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'autocomplete method for discount
    <System.Web.Script.Services.ScriptMethod(), _
  System.Web.Services.WebMethod()> _
    Public Shared Function SearchAccessionNo(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Return SearchAccNo(prefixText, count)
    End Function
    'search Accession No
    <System.Web.Script.Services.ScriptMethod(), _
System.Web.Services.WebMethod()> _
    Public Shared Function SearchAccNo(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim sdr As SqlDataReader
        Dim cmd As SqlCommand = New SqlCommand
        cmd.CommandText = "SELECT ACCESSION_NO from HOLDINGS WHERE (LIB_CODE ='" & LibCode & "')  AND (ACCESSION_NO like '" + prefixText + "%') ORDER BY CASE WHEN LEFT(Accession_NO,1) LIKE  '[A-Za-z]'  THEN dbo.Sort_AlphaNumeric(Accession_No, 'A-Za-z') END ASC, CAST(dbo.Sort_AlphaNumeric(Accession_no, '0-9') AS float) ASC "
        cmd.Connection = SqlConn
        Try
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            Dim accno As List(Of String) = New List(Of String)
            sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            While sdr.Read
                accno.Add(sdr("ACCESSION_NO").ToString)
            End While
            Return accno
        Catch e As Exception
            If sdr IsNot Nothing Then
                sdr.Close()
            Else
                SqlConn.Close()
            End If
        Finally
            SqlConn.Close()
        End Try
    End Function
    'update selected Record
    Protected Sub Status_Update_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Status_Update_Bttn.Click
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing
        Try
            Dim counter1 As Integer
            Dim c As Integer
            Dim iloop As Integer
            Dim strcurrentchar As Object
            If Grid2.Rows.Count <> 0 Then
                For Each row As GridViewRow In Grid2.Rows
                    If row.RowType = DataControlRowType.DataRow Then
                        Dim cb As CheckBox = DirectCast(row.FindControl("cbd"), CheckBox)
                        If cb IsNot Nothing AndAlso cb.Checked = True Then
                            Dim HOLD_ID As Integer = Convert.ToInt32(Grid2.DataKeys(row.RowIndex).Value)
                            'validation for order No
                            Dim STA_CODE As Object = Nothing
                            If DDL_NewStatus.Text <> "" Then
                                STA_CODE = Trim(DDL_NewStatus.SelectedValue)
                                STA_CODE = RemoveQuotes(STA_CODE)
                                If STA_CODE.Length > 10 Then
                                    Label6.Text = "Error: Input is not Valid !"
                                    Label15.Text = ""
                                    DDL_NewStatus.Focus()
                                    Exit Sub
                                End If
                                STA_CODE = " " & STA_CODE & " "
                                If InStr(1, STA_CODE, "CREATE", 1) > 0 Or InStr(1, STA_CODE, "DELETE", 1) > 0 Or InStr(1, STA_CODE, "DROP", 1) > 0 Or InStr(1, STA_CODE, "INSERT", 1) > 1 Or InStr(1, STA_CODE, "TRACK", 1) > 1 Or InStr(1, STA_CODE, "TRACE", 1) > 1 Then
                                    Label6.Text = "Error: Input is not Valid !"
                                    Label15.Text = ""
                                    DDL_NewStatus.Focus()
                                    Exit Sub
                                End If
                                STA_CODE = TrimX(UCase(STA_CODE))
                                'check unwanted characters
                                c = 0
                                counter1 = 0
                                For iloop = 1 To Len(STA_CODE)
                                    strcurrentchar = Mid(STA_CODE, iloop, 1)
                                    If c = 0 Then
                                        If Not InStr("';<>=%""", strcurrentchar) <= 0 Then
                                            c = c + 1
                                            counter1 = 1
                                        End If
                                    End If
                                Next
                                If counter1 = 1 Then
                                    Label6.Text = "Error: Input is not Valid !"
                                    Label15.Text = ""
                                    DDL_NewStatus.Focus()
                                    Exit Sub
                                End If

                                If STA_CODE = "2" Then
                                    Label6.Text = "Error: U can not Issue Book Here. Use Circulation Module for This Purpose!"
                                    Label15.Text = ""
                                    DDL_NewStatus.Focus()
                                    Exit Sub
                                End If

                                Dim DATE_MODIFIED As Object = Nothing
                                DATE_MODIFIED = Now.Date
                                DATE_MODIFIED = Convert.ToDateTime(DATE_MODIFIED, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                                Dim IP As Object = Nothing
                                IP = Request.UserHostAddress.Trim

                                'update record
                                Dim SQL As String = Nothing
                                SQL = "SELECT HOLD_ID, STA_CODE, DATE_MODIFIED, UPDATED_BY, IP FROM HOLDINGS WHERE (HOLD_ID = '" & HOLD_ID & "') AND (STA_CODE<>'2') AND (LIB_CODE ='" & (LibCode) & "') "

                                SqlConn.Open()

                                Dim da As SqlDataAdapter
                                Dim cmdb As SqlCommandBuilder
                                Dim ds As New DataSet
                                Dim dt As New DataTable

                                da = New SqlDataAdapter(SQL, SqlConn)
                                cmdb = New SqlCommandBuilder(da)
                                da.Fill(ds, "APP")
                                If ds.Tables("APP").Rows.Count <> 0 Then
                                    If STA_CODE <> "" Then
                                        ds.Tables("APP").Rows(0)("STA_CODE") = STA_CODE
                                    End If

                                    ds.Tables("APP").Rows(0)("UPDATED_BY") = UserCode
                                    ds.Tables("APP").Rows(0)("DATE_MODIFIED") = DATE_MODIFIED
                                    ds.Tables("APP").Rows(0)("IP") = IP

                                    thisTransaction = SqlConn.BeginTransaction()
                                    da.SelectCommand.Transaction = thisTransaction
                                    da.Update(ds, "APP")
                                    thisTransaction.Commit()
                                End If
                                SqlConn.Close()
                            End If
                        End If
                    End If
                Next
            End If
            Search_Bttn_Click(sender, e)
        Catch q As SqlException
            thisTransaction.Rollback()
        Catch s As Exception
            Label15.Text = ""
            Label6.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'get value of row from grid
    Private Sub Grid2_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles Grid2.RowCommand
        Dim Command As SqlCommand = Nothing
        Dim dr As SqlDataReader = Nothing
        Try
            If e.CommandName = "Select" Then
                Dim myRowID, HOLD_ID As Integer
                myRowID = e.CommandArgument.ToString()
                HOLD_ID = Grid2.DataKeys(myRowID).Value
                If Not String.IsNullOrEmpty(HOLD_ID) And HOLD_ID <> 0 Then
                    Dim iloop As Integer
                    Dim strcurrentchar As Object
                    Dim c As Integer
                    Dim counter1 As Integer

                    HOLD_ID = TrimX(HOLD_ID)
                    HOLD_ID = UCase(HOLD_ID)

                    HOLD_ID = RemoveQuotes(HOLD_ID)
                    If Len(HOLD_ID).ToString > 10 Then
                        Label15.Text = "Length of Input is not Proper!"
                        Label1.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Length of Input is not Proper... ');", True)
                        Exit Sub
                    End If
                    HOLD_ID = " " & HOLD_ID & " "
                    If InStr(1, HOLD_ID, " CREATE ", 1) > 0 Or InStr(1, HOLD_ID, " DELETE ", 1) > 0 Or InStr(1, HOLD_ID, " DROP ", 1) > 0 Or InStr(1, HOLD_ID, " INSERT ", 1) > 1 Or InStr(1, HOLD_ID, " TRACK ", 1) > 1 Or InStr(1, HOLD_ID, " TRACE ", 1) > 1 Then
                        Label15.Text = "Do not use reserve words... !"
                        Label1.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do not use reserve words... ');", True)
                        Exit Sub
                    End If
                    HOLD_ID = TrimX(HOLD_ID)
                    'get record details from database
                    Dim SQL As String = Nothing
                    SQL = " SELECT *  FROM HOLDINGS WHERE (HOLD_ID = '" & Trim(HOLD_ID) & "') "
                    Command = New SqlCommand(SQL, SqlConn)
                    SqlConn.Open()
                    dr = Command.ExecuteReader(CommandBehavior.CloseConnection)
                    dr.Read()

                    If dr.HasRows = True Then
                        If dr.Item("HOLD_ID").ToString <> "" Then
                            Label_HoldID.Text = dr.Item("HOLD_ID").ToString
                        Else
                            Label_HoldID.Text = ""
                        End If

                        If dr.Item("ACCESSION_NO").ToString <> "" Then
                            Label_AccessionNo.Text = dr.Item("ACCESSION_NO").ToString
                        Else
                            Label_AccessionNo.Text = ""
                        End If

                        If dr.Item("ACCESSION_DATE").ToString <> "" Then
                            Label_AccessionDate.Text = Format(dr.Item("ACCESSION_DATE"), "dd/MM/yyyy")
                        Else
                            Label_AccessionDate.Text = ""
                        End If

                        If dr.Item("VOL_NO").ToString <> "" Then
                            Label_VolNo.Text = dr.Item("VOL_NO").ToString
                        Else
                            Label_VolNo.Text = ""
                        End If


                        If dr.Item("REMARKS").ToString <> "" Then
                            txt_Hold_Remarks.Text = dr.Item("REMARKS").ToString
                        Else
                            txt_Hold_Remarks.Text = ""
                        End If

                        dr.Close()
                        Me.Acq_Update_Bttn.Visible = True
                        Me.Acq_Cancel_Bttn.Visible = True
                    Else
                        Label_HoldID.Text = ""
                        Label_AccessionNo.Text = ""
                        Label_AccessionDate.Text = ""
                        Label_HOLDID.Text = ""
                        Label_VolNo.Text = ""
                        Label15.Text = "Record Not Selected"
                        Label1.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' No Record Selected!');", True)
                    End If
                Else
                    Label_HoldID.Text = ""
                    Label_AccessionNo.Text = ""
                    Label_AccessionDate.Text = ""
                    Label_HoldID.Text = ""
                    Label_VolNo.Text = ""
                    Label15.Text = "Record Not Selected"
                    Label1.Text = ""
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' No Record Selected!');", True)
                End If
            End If
        Catch s As Exception
            Label15.Text = "Error: " & (s.Message())
            Label1.Text = ""
        Finally
            SqlConn.Close()
        End Try
    End Sub 'Grid1_ItemCommand
    'update Remarks
    Protected Sub Acq_Update_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Acq_Update_Bttn.Click
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing
        Try
            Dim counter1 As Integer
            Dim c As Integer
            Dim iloop As Integer
            Dim strcurrentchar As Object

            Dim REMARKS As Object = Nothing
            If txt_Hold_Remarks.Text <> "" Then
                REMARKS = TrimAll(txt_Hold_Remarks.Text)
                REMARKS = RemoveQuotes(REMARKS)
                If REMARKS.Length > 250 Then
                    Label6.Text = "Error: Input is not Valid !"
                    Label15.Text = ""
                    txt_Hold_Remarks.Focus()
                    Exit Sub
                End If
                REMARKS = " " & REMARKS & " "
                If InStr(1, REMARKS, "CREATE", 1) > 0 Or InStr(1, REMARKS, "DELETE", 1) > 0 Or InStr(1, REMARKS, "DROP", 1) > 0 Or InStr(1, REMARKS, "INSERT", 1) > 1 Or InStr(1, REMARKS, "TRACK", 1) > 1 Or InStr(1, REMARKS, "TRACE", 1) > 1 Then
                    Label6.Text = "Error: Input is not Valid !"
                    Label15.Text = ""
                    txt_Hold_Remarks.Focus()
                    Exit Sub
                End If
                REMARKS = TrimAll(REMARKS)

                Dim DATE_MODIFIED As Object = Nothing
                DATE_MODIFIED = Now.Date
                DATE_MODIFIED = Convert.ToDateTime(DATE_MODIFIED, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                Dim IP As Object = Nothing
                IP = Request.UserHostAddress.Trim

                Dim HOLD_ID As Long = Nothing
                If Label_HoldID.Text <> "" Then
                    HOLD_ID = Trim(Label_HoldID.Text)
                    HOLD_ID = RemoveQuotes(HOLD_ID)
                    If HOLD_ID.ToString.Length > 10 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        Exit Sub
                    End If
                    HOLD_ID = " " & HOLD_ID & " "
                    If InStr(1, HOLD_ID, "CREATE", 1) > 0 Or InStr(1, HOLD_ID, "DELETE", 1) > 0 Or InStr(1, HOLD_ID, "DROP", 1) > 0 Or InStr(1, HOLD_ID, "INSERT", 1) > 1 Or InStr(1, HOLD_ID, "TRACK", 1) > 1 Or InStr(1, HOLD_ID, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        Exit Sub
                    End If
                    HOLD_ID = TrimX(HOLD_ID)
                    'check unwanted characters
                    c = 0
                    counter1 = 0
                    For iloop = 1 To Len(HOLD_ID.ToString)
                        strcurrentchar = Mid(HOLD_ID, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter1 = 1
                            End If
                        End If
                    Next
                    If counter1 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        Exit Sub
                    End If


                    'update record
                    Dim SQL As String = Nothing
                    SQL = "SELECT HOLD_ID, REMARKS, DATE_MODIFIED, UPDATED_BY, IP FROM HOLDINGS WHERE (HOLD_ID = '" & HOLD_ID & "') AND (LIB_CODE ='" & (LibCode) & "') "

                    SqlConn.Open()

                    Dim da As SqlDataAdapter
                    Dim cmdb As SqlCommandBuilder
                    Dim ds As New DataSet
                    Dim dt As New DataTable

                    da = New SqlDataAdapter(SQL, SqlConn)
                    cmdb = New SqlCommandBuilder(da)
                    da.Fill(ds, "HOLD")
                    If ds.Tables("HOLD").Rows.Count <> 0 Then
                        If REMARKS <> "" Then
                            ds.Tables("HOLD").Rows(0)("REMARKS") = REMARKS
                        End If

                        ds.Tables("HOLD").Rows(0)("UPDATED_BY") = UserCode
                        ds.Tables("HOLD").Rows(0)("DATE_MODIFIED") = DATE_MODIFIED
                        ds.Tables("HOLD").Rows(0)("IP") = IP

                        thisTransaction = SqlConn.BeginTransaction()
                        da.SelectCommand.Transaction = thisTransaction
                        da.Update(ds, "HOLD")
                        thisTransaction.Commit()
                    End If
                    SqlConn.Close()
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Record Updated Successfully!');", True)
                End If
            End If
        Catch q As SqlException
            thisTransaction.Rollback()
        Catch s As Exception
            Label15.Text = ""
            Label6.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'print compact report
    Protected Sub Print_Compact_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Print_Compact_Bttn.Click
        Try
            Dim Reportdoc As New ReportDocument
            Reportdoc = Report_Load_Compact()

            If DDL_PrintFormats.SelectedValue <> "" Then
                If DDL_PrintFormats.SelectedValue = "PDF" Then
                    Reportdoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "Report_StatysWise_Compact_" + Format(DateTime.Now, "ddMMyyyy"))
                End If
                If DDL_PrintFormats.SelectedValue = "DOC" Then
                    Reportdoc.ExportToHttpResponse(ExportFormatType.WordForWindows, Response, True, "Report_StatusWise_Compact_" + Format(DateTime.Now, "ddMMyyyy"))
                End If
                If DDL_PrintFormats.SelectedValue = "EXCEL" Then
                    If RB_Monographs.Checked = True Then
                        ExportTo_excel_Books()
                    Else
                        ExportTo_excel_Serials()
                    End If
                End If
            Else
                Reportdoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "Report_StatusWise_Compact_" + Format(DateTime.Now, "ddMMyyyy"))
            End If
        Catch ex As Exception
            Label6.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    Public Function Report_Load_Compact() As ReportDocument
        Dim Rpt As New ReportDocument
        Dim dtLibrary As New DataTable
        dtLibrary = (ViewState("dt"))

        Rpt.Load(Server.MapPath("~/Reports/Title_Compact_StatusWise_Report.rpt"))
        
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
    Public Sub ExportTo_excel_Books()

        Dim strFilename As String = "Export_StatusWise_Books_" + Format(DateTime.Now, "ddMMyyyy") + ".xls" '"ExportToExcel.xls"
        Dim attachment As String = "attachment; filename=" & strFilename
        Response.ClearContent()
        Response.AddHeader("content-disposition", attachment)
        Response.ContentType = "application/vnd.ms-excel"

        Dim Flag As Object = Nothing
        Dim dt As DataTable = Nothing
        For Each row As GridViewRow In Grid2.Rows
            Dim HOLD_ID As Integer = Convert.ToInt32(Grid2.DataKeys(row.RowIndex).Value)
            Dim SQL As String = Nothing
            SQL = "SELECT BOOKS_ACC_REGISTER_VIEW.*, BOOKSTATUS.STA_NAME FROM BOOKS_ACC_REGISTER_VIEW INNER JOIN BOOKSTATUS ON BOOKS_ACC_REGISTER_VIEW.STA_CODE = BOOKSTATUS.STA_CODE WHERE (BOOKS_ACC_REGISTER_VIEW.LIB_CODE = '" & Trim(LibCode) & "') AND (BOOKS_ACC_REGISTER_VIEW.HOLD_ID ='" & Trim(HOLD_ID) & "');"
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
    Public Sub ExportTo_excel_Serials()

        Dim strFilename As String = "Export_StatusWise_Serials_" + Format(DateTime.Now, "ddMMyyyy") + ".xls" '"ExportToExcel.xls"
        Dim attachment As String = "attachment; filename=" & strFilename
        Response.ClearContent()
        Response.AddHeader("content-disposition", attachment)
        Response.ContentType = "application/vnd.ms-excel"

        Dim Flag As Object = Nothing
        Dim dt As DataTable = Nothing
        For Each row As GridViewRow In Grid2.Rows
            Dim HOLD_ID As Integer = Convert.ToInt32(Grid2.DataKeys(row.RowIndex).Value)
            Dim SQL As String = Nothing
            SQL = "SELECT HOLDINGS.*, CATS_AUTHORS_VIEW.LANG_CODE, CATS_AUTHORS_VIEW.BIB_CODE, CATS_AUTHORS_VIEW.MAT_CODE, CATS_AUTHORS_VIEW.DOC_TYPE_CODE, CATS_AUTHORS_VIEW.STANDARD_NO, CATS_AUTHORS_VIEW.TITLE, CATS_AUTHORS_VIEW.SUB_TITLE, CATS_AUTHORS_VIEW.VAR_TITLE, CATS_AUTHORS_VIEW.CORPORATE_AUTHOR, CATS_AUTHORS_VIEW.EDITOR, CATS_AUTHORS_VIEW.PLACE_OF_PUB, CATS_AUTHORS_VIEW.URL, CATS_AUTHORS_VIEW.PUB_NAME, CATS_AUTHORS_VIEW.PUB_PLACE,CATS_AUTHORS_VIEW.SUB_NAME, J_HISTORY.CODEN, J_HISTORY.J_START_VOL, J_HISTORY.J_START_ISSUE, J_HISTORY.J_START_MONTH, J_HISTORY.J_START_YEAR, J_HISTORY.FREQ_CODE, BOOKSTATUS.STA_NAME FROM HOLDINGS INNER JOIN CATS_AUTHORS_VIEW ON dbo.HOLDINGS.CAT_NO = CATS_AUTHORS_VIEW.CAT_NO INNER JOIN BOOKSTATUS ON HOLDINGS.STA_CODE = BOOKSTATUS.STA_CODE CROSS JOIN J_HISTORY WHERE (HOLDINGS.LIB_CODE = '" & Trim(LibCode) & "') AND (HOLDINGS.HOLD_ID ='" & Trim(HOLD_ID) & "');"
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
    'print summary report
    Protected Sub Print_Summary_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Print_Summary_Bttn.Click
        Try
            Dim Reportdoc As New ReportDocument
            Reportdoc = Report_Load_Summary()

            If DDL_PrintFormats.SelectedValue <> "" Then
                If DDL_PrintFormats.SelectedValue = "PDF" Then
                    Reportdoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "Report_StatusWise_Summary_" + Format(DateTime.Now, "ddMMyyyy"))
                End If
                If DDL_PrintFormats.SelectedValue = "DOC" Then
                    Reportdoc.ExportToHttpResponse(ExportFormatType.WordForWindows, Response, True, "Report_StatusWise_Summary_" + Format(DateTime.Now, "ddMMyyyy"))
                End If
                If DDL_PrintFormats.SelectedValue = "HTML" Then

                End If
                If DDL_PrintFormats.SelectedValue = "EXCEL" Then
                    If RB_Monographs.Checked = True Then
                        ExportTo_excel_Books()
                    Else
                        ExportTo_excel_Serials()
                    End If
                End If
            Else
                Reportdoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "Report_StatusWise_Summary_" + Format(DateTime.Now, "ddMMyyyy"))
            End If
        Catch ex As Exception
            Label6.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    Public Function Report_Load_Summary() As ReportDocument
        Dim Rpt As New ReportDocument
        Dim dt As DataTable = Nothing
        Try
            Dim SQL As String = Nothing
            If RB_Monographs.Checked = True Then
                SQL = "SELECT BOOKS_ACC_REGISTER_VIEW.*, BOOKSTATUS.STA_NAME FROM BOOKS_ACC_REGISTER_VIEW INNER JOIN BOOKSTATUS ON BOOKS_ACC_REGISTER_VIEW.STA_CODE = BOOKSTATUS.STA_CODE WHERE (BOOKS_ACC_REGISTER_VIEW.LIB_CODE = '" & Trim(LibCode) & "') "
            Else
                SQL = "SELECT BOOKS_ACC_REGISTER_VIEW.*, J_HISTORY.CODEN, J_HISTORY.J_START_VOL, J_HISTORY.J_START_ISSUE, J_HISTORY.J_START_MONTH, J_HISTORY.J_START_YEAR, J_HISTORY.FREQ_CODE, BOOKSTATUS.STA_NAME FROM BOOKS_ACC_REGISTER_VIEW  INNER JOIN BOOKSTATUS ON BOOKS_ACC_REGISTER_VIEW.STA_CODE = BOOKSTATUS.STA_CODE CROSS JOIN J_HISTORY WHERE (BOOKS_ACC_REGISTER_VIEW.LIB_CODE = '" & Trim(LibCode) & "') "
            End If

            Dim SQL2 As String = Nothing
            For Each row As GridViewRow In Grid2.Rows
                Dim HOLD_ID As Integer = Convert.ToInt32(Grid2.DataKeys(row.RowIndex).Value)
                If SQL2 = "" Then
                    SQL2 = " (BOOKS_ACC_REGISTER_VIEW.HOLD_ID ='" & Trim(HOLD_ID) & "' "
                Else
                    SQL2 = SQL2 + " OR BOOKS_ACC_REGISTER_VIEW.HOLD_ID ='" & Trim(HOLD_ID) & "' "
                End If
            Next
            SQL = SQL + " AND " + SQL2 + ")"

            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            da.Fill(ds)
            dt = ds.Tables(0).Copy

            If RB_Monographs.Checked = True Then
                Rpt.Load(Server.MapPath("~/Reports/Title_Summary_StatusWise_Books_Report.rpt"))
            Else
                Rpt.Load(Server.MapPath("~/Reports/Title_Summary_StatusWise_Serials_Report.rpt"))
            End If
            Rpt.SetDataSource(dt)
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
            dt.Dispose()
            Return Rpt
        Catch ex As Exception
            Label6.Text = "Error: " & (ex.Message())
        Finally
            SqlConn.Close()
        End Try
    End Function
    'print detail report
    Protected Sub Print_Detail_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Print_Detail_Bttn.Click
        Try
            Dim Reportdoc As New ReportDocument
            Reportdoc = Report_Load_Detail()

            If DDL_PrintFormats.SelectedValue <> "" Then
                If DDL_PrintFormats.SelectedValue = "PDF" Then
                    Reportdoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "Report_StatusWise_Detail_" + Format(DateTime.Now, "ddMMyyyy"))
                End If
                If DDL_PrintFormats.SelectedValue = "DOC" Then
                    Reportdoc.ExportToHttpResponse(ExportFormatType.WordForWindows, Response, True, "Report_StatusWise_Detail_" + Format(DateTime.Now, "ddMMyyyy"))
                End If
                If DDL_PrintFormats.SelectedValue = "HTML" Then

                End If
                If DDL_PrintFormats.SelectedValue = "EXCEL" Then
                    If RB_Monographs.Checked = True Then
                        ExportTo_excel_Books()
                    Else
                        ExportTo_excel_Serials()
                    End If
                End If
            Else
                Reportdoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "Report_StatusWise_Summary_" + Format(DateTime.Now, "ddMMyyyy"))
            End If
        Catch ex As Exception
            Label6.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    Public Function Report_Load_Detail() As ReportDocument
        Dim Rpt As New ReportDocument
        Dim dt As DataTable = Nothing
        Try
            Dim SQL As String = Nothing
            If RB_Monographs.Checked = True Then
                SQL = "SELECT BOOKS_ACC_REGISTER_VIEW.*, BOOKSTATUS.STA_NAME FROM BOOKS_ACC_REGISTER_VIEW INNER JOIN BOOKSTATUS ON BOOKS_ACC_REGISTER_VIEW.STA_CODE = BOOKSTATUS.STA_CODE WHERE (BOOKS_ACC_REGISTER_VIEW.LIB_CODE = '" & Trim(LibCode) & "') "
            Else
                SQL = "SELECT BOOKS_ACC_REGISTER_VIEW.*, J_HISTORY.CODEN, J_HISTORY.J_START_VOL, J_HISTORY.J_START_ISSUE, J_HISTORY.J_START_MONTH, J_HISTORY.J_START_YEAR, J_HISTORY.FREQ_CODE, BOOKSTATUS.STA_NAME FROM BOOKS_ACC_REGISTER_VIEW  INNER JOIN BOOKSTATUS ON BOOKS_ACC_REGISTER_VIEW.STA_CODE = BOOKSTATUS.STA_CODE CROSS JOIN J_HISTORY WHERE (BOOKS_ACC_REGISTER_VIEW.LIB_CODE = '" & Trim(LibCode) & "') "
            End If

            Dim SQL2 As String = Nothing
            For Each row As GridViewRow In Grid2.Rows
                Dim HOLD_ID As Integer = Convert.ToInt32(Grid2.DataKeys(row.RowIndex).Value)
                If SQL2 = "" Then
                    SQL2 = " (BOOKS_ACC_REGISTER_VIEW.HOLD_ID ='" & Trim(HOLD_ID) & "' "
                Else
                    SQL2 = SQL2 + " OR BOOKS_ACC_REGISTER_VIEW.HOLD_ID ='" & Trim(HOLD_ID) & "' "
                End If
            Next
            SQL = SQL + " AND " + SQL2 + ")"

            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            da.Fill(ds)
            dt = ds.Tables(0).Copy

            If RB_Monographs.Checked = True Then
                Rpt.Load(Server.MapPath("~/Reports/Title_Details_StatusWise_Books_Report.rpt"))
            Else
                Rpt.Load(Server.MapPath("~/Reports/Title_Details_StatusWise_Serials_Report.rpt"))
            End If
            Rpt.SetDataSource(dt)
            Rpt.Refresh()

            Dim myGrpName As Object = Nothing
            If DDL_GroupBy.Text <> "" Then
                myGrpName = Trim(DDL_GroupBy.SelectedValue)
            Else
                myGrpName = Nothing
            End If

            Dim myGroupName As CrystalReports.Engine.TextObject
            If myGrpName <> "" Then
                myGroupName = Rpt.ReportDefinition.Sections("ReportHeaderSection1").ReportObjects.Item("Text28")
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
            dt.Dispose()
            Return Rpt
        Catch ex As Exception
            Label6.Text = "Error: " & (ex.Message())
        Finally
            SqlConn.Close()
        End Try
    End Function
End Class