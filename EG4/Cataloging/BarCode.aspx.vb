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

Public Class BarCode
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
                        PopulateBibCodes()
                        DDL_Bib_Level.SelectedValue = "M"
                        Me.DDL_Bib_Level_SelectedIndexChanged(sender, e)
                        DDL_Mat_Type.SelectedValue = "B"
                        DDL_Mat_Type_SelectedIndexChanged(sender, e)
                        DDL_Doc_Type.SelectedValue = "BK"

                        PopulateAcqModes()
                        PopulateCurrentStatus()
                        PopulateSections()
                        PopulateLocation()
                        PopulateClassNo()
                        RadioButton6.Checked = True
                        RadioButton6.Focus()


                    End If
                    Dim CreateSUserButton As Object 'System.Web.UI.WebControls.Button
                    CreateSUserButton = Master.FindControl("Accordion1").FindControl("CatPane").FindControl("Cat_BarCode_Bttn")
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
    Public Sub PopulateBibCodes()
        Me.DDL_Bib_Level.DataTextField = "BIB_NAME"
        Me.DDL_Bib_Level.DataValueField = "BIB_CODE"
        Me.DDL_Bib_Level.DataSource = GetBibLevelist()
        Me.DDL_Bib_Level.DataBind()
    End Sub
    'select materials type on selection of bib levels
    Protected Sub DDL_Bib_Level_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDL_Bib_Level.SelectedIndexChanged
        Dim Command As SqlCommand = Nothing
        Dim dt As DataTable = Nothing
        Try
            Dim myBibName As Object = Nothing
            If DDL_Bib_Level.Text <> "" Then
                myBibName = DDL_Bib_Level.SelectedValue
            Else
                myBibName = ""
            End If

            If myBibName = "S" Then
                Label6.Text = "This Form can not be used to Add SERIALS - use SERIALS Module for such materials.)"
                Label15.Text = ""
                DDL_Mat_Type.Items.Clear()
                DDL_Doc_Type.Items.Clear()
                DDL_Bib_Level.Focus()
                Exit Sub
            End If
            Command = New SqlCommand("SELECT  MAT_CODE, MAT_NAME FROM MATERIALS WHERE (BIB_CODE = '" & Trim(myBibName) & "') ORDER BY MAT_NAME ", SqlConn)
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
            Dr("MAT_CODE") = ""
            Dr("MAT_NAME") = ""
            dt.Rows.InsertAt(Dr, 0)

            If dt.Rows.Count = 0 Then
                Me.DDL_Mat_Type.DataSource = Nothing
            Else
                Me.DDL_Mat_Type.DataSource = dt
                Me.DDL_Mat_Type.DataTextField = "MAT_NAME"
                Me.DDL_Mat_Type.DataValueField = "MAT_CODE"
                Me.DDL_Mat_Type.DataBind()
            End If
            dt.Dispose()
            SqlConn.Close()
            DDL_Doc_Type.Items.Clear()
            DDL_Bib_Level.Focus()
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
            Label15.Text = ""
        Finally
            SqlConn.Close()
        End Try

    End Sub
    'select doc type on selection of materials
    Public Sub DDL_Mat_Type_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDL_Mat_Type.SelectedIndexChanged
        Dim Command As SqlCommand = Nothing
        Dim dt As DataTable = Nothing
        Try
            Dim myMatName As Object = Nothing
            If DDL_Mat_Type.Text <> "" Then
                myMatName = DDL_Mat_Type.SelectedValue
            Else
                myMatName = ""
            End If
            Command = New SqlCommand("SELECT  DOC_TYPE_CODE, DOC_TYPE_NAME FROM DOC_TYPES WHERE (MAT_CODE = '" & Trim(myMatName) & "') ORDER BY DOC_TYPE_NAME ", SqlConn)
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
            Dr("DOC_TYPE_CODE") = ""
            Dr("DOC_TYPE_NAME") = ""
            dt.Rows.InsertAt(Dr, 0)

            If dt.Rows.Count = 0 Then
                Me.DDL_Doc_Type.DataSource = Nothing
            Else
                Me.DDL_Doc_Type.DataSource = dt
                Me.DDL_Doc_Type.DataTextField = "DOC_TYPE_NAME"
                Me.DDL_Doc_Type.DataValueField = "DOC_TYPE_CODE"
                Me.DDL_Doc_Type.DataBind()
            End If
            dt.Dispose()
            SqlConn.Close()
            DDL_Mat_Type.Focus()
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
            Label15.Text = ""
        Finally
            Command.Dispose()
            dt.Dispose()
            SqlConn.Close()
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
            Label6.Text = "Error:  there is error in page index !"
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
    Protected Sub Grid2_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Grid2.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Lblsr As Label = e.Row.FindControl("lblsr")
            Srno = index + 1
            If Lblsr Is Nothing = False Then
                Lblsr.Text = Srno
                index += 1
            End If

        End If
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
            Dim iloop As Integer
            Dim strcurrentchar As Object
            Dim c As Integer
            Dim counter1, counter2, counter3, counter4 As Object

            'validation for DDL_Bib_Level
            Dim myBibLevel As Object = Nothing
            myBibLevel = DDL_Bib_Level.SelectedValue
            If Not String.IsNullOrEmpty(myBibLevel) Then
                myBibLevel = RemoveQuotes(myBibLevel)
                If myBibLevel.Length > 2 Then
                    Label6.Text = "Error: Input is not Valid !"
                    Label15.Text = ""
                    DDL_Bib_Level.Focus()
                    Exit Sub
                End If
                myBibLevel = " " & myBibLevel & " "
                If InStr(1, myBibLevel, "CREATE", 1) > 0 Or InStr(1, myBibLevel, "DELETE", 1) > 0 Or InStr(1, myBibLevel, "DROP", 1) > 0 Or InStr(1, myBibLevel, "INSERT", 1) > 1 Or InStr(1, myBibLevel, "TRACK", 1) > 1 Or InStr(1, myBibLevel, "TRACE", 1) > 1 Then
                    Label6.Text = "Error: Input is not Valid !"
                    Label15.Text = ""
                    DDL_Bib_Level.Focus()
                    Exit Sub
                End If
                myBibLevel = TrimX(myBibLevel)
                'check unwanted characters
                c = 0
                counter1 = 0
                For iloop = 1 To Len(myBibLevel)
                    strcurrentchar = Mid(myBibLevel, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter1 = 1
                        End If
                    End If
                Next
                If counter1 = 1 Then
                    Label6.Text = "Error: Input is not Valid !"
                    Label15.Text = ""
                    DDL_Bib_Level.Focus()
                    Exit Sub
                End If
            Else
                myBibLevel = "M"
            End If

            'validation for DDL_Mat_Type
            Dim myMatType As Object = Nothing
            myMatType = DDL_Mat_Type.SelectedValue
            If Not String.IsNullOrEmpty(myMatType) Then
                myMatType = RemoveQuotes(myMatType)
                If myMatType.Length > 2 Then
                    Label6.Text = "Error: Input is not Valid !"
                    Label15.Text = ""
                    DDL_Mat_Type.Focus()
                    Exit Sub
                End If
                myMatType = " " & myMatType & " "
                If InStr(1, myMatType, "CREATE", 1) > 0 Or InStr(1, myMatType, "DELETE", 1) > 0 Or InStr(1, myMatType, "DROP", 1) > 0 Or InStr(1, myMatType, "INSERT", 1) > 1 Or InStr(1, myMatType, "TRACK", 1) > 1 Or InStr(1, myMatType, "TRACE", 1) > 1 Then
                    Label6.Text = "Error: Input is not Valid !"
                    Label15.Text = ""
                    DDL_Mat_Type.Focus()
                    Exit Sub
                End If
                myMatType = TrimX(myMatType)
                'check unwanted characters
                c = 0
                counter2 = 0
                For iloop = 1 To Len(myMatType)
                    strcurrentchar = Mid(myBibLevel, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter2 = 1
                        End If
                    End If
                Next
                If counter2 = 1 Then
                    Label6.Text = "Error: Input is not Valid !"
                    Label15.Text = ""
                    DDL_Mat_Type.Focus()
                    Exit Sub
                End If
            Else
                myMatType = ""
            End If

            'validation for document Type
            Dim myDocType As Object = Nothing
            myDocType = DDL_Doc_Type.SelectedValue
            If Not String.IsNullOrEmpty(myDocType) Then
                myDocType = RemoveQuotes(myDocType)
                If myDocType.Length > 4 Then
                    Label6.Text = "Error: Input is not Valid !"
                    Label15.Text = ""
                    DDL_Doc_Type.Focus()
                    Exit Sub
                End If
                myDocType = " " & myDocType & " "
                If InStr(1, myDocType, "CREATE", 1) > 0 Or InStr(1, myDocType, "DELETE", 1) > 0 Or InStr(1, myDocType, "DROP", 1) > 0 Or InStr(1, myDocType, "INSERT", 1) > 1 Or InStr(1, myDocType, "TRACK", 1) > 1 Or InStr(1, myDocType, "TRACE", 1) > 1 Then
                    Label6.Text = "Error: Input is not Valid !"
                    Label15.Text = ""
                    DDL_Doc_Type.Focus()
                    Exit Sub
                End If
                myDocType = TrimX(myDocType)
                'check unwanted characters
                c = 0
                counter3 = 0
                For iloop = 1 To Len(myDocType)
                    strcurrentchar = Mid(myDocType, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter3 = 1
                        End If
                    End If
                Next
                If counter3 = 1 Then
                    Label6.Text = "Error: Input is not Valid !"
                    Label15.Text = ""
                    DDL_Doc_Type.Focus()
                    Exit Sub
                End If
            Else
                myDocType = ""
            End If

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
            SQL = "SELECT HOLD_ID, CAT_NO, ACQ_ID, ACCESSION_NO, ACCESSION_DATE, VOL_NO, CLASS_NO, BOOK_NO, PAGINATION, PHYSICAL_LOCATION, STA_CODE, COLLECTION_TYPE, LIB_CODE, TITLE = CASE (ISNULL(SUB_TITLE, '')) WHEN '' THEN TITLE ELSE (TITLE + ': '+SUB_TITLE) END  FROM BOOKS_ACC_REGISTER_VIEW WHERE (LIB_CODE = '" & Trim(LibCode) & "') AND (BIB_CODE='" & Trim(myBibLevel) & "') "
            If myMatType <> "" Then
                SQL = SQL & " AND  (MAT_CODE ='" & Trim(myMatType) & "')"
            End If
            If myDocType <> "" Then
                SQL = SQL & " AND  (DOC_TYPE_CODE ='" & Trim(myDocType) & "')"
            End If

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
                BarCode_Generate_Bttn.Enabled = False
            Else
                Grid2.Visible = True
                RecordCount = dtSearch.Rows.Count
                Grid2.DataSource = dtSearch
                Grid2.DataBind()
                Label1.Text = "Total Record(s): " & RecordCount
                BarCode_Generate_Bttn.Enabled = True
            End If
            ViewState("dt") = dtSearch
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
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
    Protected Sub Status_Update_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BarCode_Generate_Bttn.Click
        Dim Command As SqlCommand = Nothing
        Dim dr As SqlDataReader = Nothing
        Try
            Dim myLine As String = Nothing
            Dim myfileName As String = Nothing
            Dim Stream_Writer As IO.StreamWriter
            Dim sCommand As String = Nothing
            Dim myPath As String = Nothing

            If System.IO.Directory.Exists("C:\LIBRARY") = False Then
                System.IO.Directory.CreateDirectory("C:\LIBRARY")
            End If

            Dim counter1 As Integer
            Dim c As Integer
            Dim iloop As Integer
            Dim strcurrentchar As Object

            Dim myLibrary As String = Nothing
            If TextBox1.Text <> "" Then
                myLibrary = TrimAll(TextBox1.Text)
            Else
                myLibrary = LibCode
            End If

            Dim myAccNO As Object = Nothing
            Dim myClassNo As Object = Nothing
            Dim myCollectionType As Object = Nothing
            Dim myCallNo As Object = Nothing
            Dim myBookNo As Object = Nothing
            Dim myLocation As Object = Nothing


            'create PRN file
            myfileName = "C:\LIBRARY\TestPRN.prn"
            Stream_Writer = New IO.StreamWriter(myfileName, False)


            If Grid2.Rows.Count <> 0 Then
                For Each row As GridViewRow In Grid2.Rows
                    If row.RowType = DataControlRowType.DataRow Then
                        Dim cb As CheckBox = DirectCast(row.FindControl("cbd"), CheckBox)
                        If cb IsNot Nothing AndAlso cb.Checked = True Then
                            Dim HOLD_ID As Integer = Convert.ToInt32(Grid2.DataKeys(row.RowIndex).Value)

                            Dim SQL As String = Nothing
                            SQL = "SELECT HOLD_ID, ACCESSION_NO, CLASS_NO, BOOK_NO, PHYSICAL_LOCATION FROM HOLDINGS WHERE (HOLD_ID = '" & HOLD_ID & "') AND (LIB_CODE ='" & (LibCode) & "') "

                            Command = New SqlCommand(SQL, SqlConn)
                            SqlConn.Open()
                            dr = Command.ExecuteReader(CommandBehavior.CloseConnection)
                            dr.Read()

                            If dr.HasRows = True Then
                                If dr.Item("HOLD_ID").ToString <> "" Then
                                    myAccNO = dr.Item("ACCESSION_NO").ToString
                                Else
                                    myAccNO = ""
                                    Continue For
                                End If

                                If dr.Item("HOLD_ID").ToString <> "" Then
                                    myClassNo = dr.Item("CLASS_NO").ToString
                                Else
                                    myClassNo = ""
                                End If

                                If dr.Item("HOLD_ID").ToString <> "" Then
                                    myBookNo = dr.Item("BOOK_NO").ToString
                                Else
                                    myBookNo = ""
                                End If


                                myLine = ""
                                myLine = myLine & "FMT(1,885d,300d,0,0,1)" & vbCrLf
                                myLine = myLine & "DMD(1)" & vbCrLf
                                myLine = myLine & "DPD(1)" & vbCrLf
                                myLine = myLine & "ACL()" & vbCrLf
                                myLine = myLine & "FAG(2)" & vbCrLf
                                myLine = myLine & "BDN(3,8)" & vbCrLf
                                myLine = myLine & "BFL(1,830d,241d,0,1,106d)" & vbCrLf
                                myLine = myLine & "COL(0)" & vbCrLf
                                myLine = myLine & "SDZ(37,37,0)" & vbCrLf
                                myLine = myLine & "CFL(2,829d,284d,32,1,1)" & vbCrLf
                                myLine = myLine & "BDN(3,8)" & vbCrLf
                                myLine = myLine & "BFL(3,380d,241d,0,1,106d)" & vbCrLf
                                myLine = myLine & "SDZ(37,37,0)" & vbCrLf
                                myLine = myLine & "CFL(4,379d,284d,32,1,1)" & vbCrLf
                                myLine = myLine & "SDZ(35,35,0)" & vbCrLf
                                myLine = myLine & "CFL(5,650d,285d,32,1,1)" & vbCrLf
                                myLine = myLine & "SDZ(35,35,0)" & vbCrLf
                                myLine = myLine & "CFL(6,200d,285d,32,1,1)" & vbCrLf
                                myLine = myLine & "SDZ(37,37,0)" & vbCrLf
                                myLine = myLine & "CFL(7,829d,127d,32,1,1)" & vbCrLf
                                myLine = myLine & "SDZ(37,37,0)" & vbCrLf
                                myLine = myLine & "CFL(8,379d,127d,32,1,1)" & vbCrLf
                                myLine = myLine & "SDZ(37,37,0)" & vbCrLf
                                myLine = myLine & "CFL(9,829d,68d,32,1,1)" & vbCrLf
                                myLine = myLine & "SDZ(37,37,0)" & vbCrLf
                                myLine = myLine & "CFL(10,379d,68d,32,1,1)" & vbCrLf
                                myLine = myLine & "DAT(1,*" & myAccNO & "*)" & vbCrLf
                                myLine = myLine & "DAT(2," & myAccNO & ")" & vbCrLf
                                myLine = myLine & "DAT(3,*" & myAccNO & "*)" & vbCrLf
                                myLine = myLine & "DAT(4," & myAccNO & ")" & vbCrLf
                                myLine = myLine & "DAT(5," & LibCode & ")" & vbCrLf
                                myLine = myLine & "DAT(6," & LibCode & ")" & vbCrLf
                                myLine = myLine & "DAT(7," & myClassNo & ")" & vbCrLf
                                myLine = myLine & "DAT(8," & myClassNo & ")" & vbCrLf
                                myLine = myLine & "DAT(9," & myBookNo & ")" & vbCrLf
                                myLine = myLine & "DAT(10," & myBookNo & ")" & vbCrLf
                                myLine = myLine & "PRT(1,0,1)" & vbCrLf
                                Stream_Writer.WriteLine(myLine)


                                dr.Close()
                            End If

                            SqlConn.Close()
                        End If
                    End If
                Next
            End If
            Stream_Writer.Close()
            'print command
            myPath = "C:\LIBRARY\TestPRN.prn"
            sCommand = "copy " & myPath & " lpt1"
            Shell("cmd.exe /c" & sCommand)

            Label15.Text = "Print sent on Printer"
            Label6.Text = ""

            ''Dim ports As String() = System.IO.Ports.SerialPort.GetPortNames()
            'Dim ports As String() = System.IO.Ports.SerialPort.GetPortNames()
            'Dim port As String
            'For Each port In ports
            '    Console.WriteLine(port)
            'Next port

        Catch s As Exception
            Label15.Text = ""
            Label6.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
   




    
End Class