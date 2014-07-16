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
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports.Engine.TextObject
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Shared
Imports System.Drawing.Printing
Imports CrystalDecisions
Imports EG4.PopulateCombo

Public Class Orders
    Inherits System.Web.UI.Page
    Dim index As Integer = 0
    Public Srno As Integer = 0
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session.Item("IsThisU") <> Nothing Then
                If Not SConnection() = True Then
                    Label6.Text = "Database Connection is lost..Try Again !'"
                    Label15.Text = ""
                Else
                    If Page.IsPostBack = False Then
                        Ord_Save_Bttn.Visible = False
                        Ord_Cancel_Bttn.Visible = False
                        Ord_Delete_Bttn.Visible = False
                        Label15.Text = "Enter Order No and Press ADD ORDER Button to save the data in selected Record(s)"
                        Label6.Text = ""
                        PopulateApprovals()
                        PopulateVendors()
                        PopulateLetters()
                        If MultiView1.ActiveViewIndex = 0 Then
                            DDL_Approvals.Focus()
                        End If
                        If MultiView1.ActiveViewIndex = 1 Then
                            DDL_Orders.Focus()
                        End If
                    End If
                    Dim CreateSUserButton As Object 'System.Web.UI.WebControls.Button
                    CreateSUserButton = Master.FindControl("Accordion1").FindControl("AcqPane").FindControl("Acq_Orders_Bttn")
                    CreateSUserButton.ForeColor = Drawing.Color.Red
                    myPaneName = "AcqPane"
                End If
            Else
                Response.Redirect("~/Default.aspx", False)
            End If
            LibCode = Session.Item("LoggedLibcode")
        Catch ex As Exception
            Label6.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As MenuEventArgs) Handles Menu1.MenuItemClick
        MultiView1.ActiveViewIndex = Int32.Parse(e.Item.Value)
        If MultiView1.ActiveViewIndex = 0 Then
            Menu1.Items(0).ImageUrl = "~/Images/OrderUp_bttn.png"
            Menu1.Items(1).ImageUrl = "~/Images/OrderGenerateOver_bttn.png"
            Menu1.Items(2).ImageUrl = "~/Images/ReceiveOver.png"
            PopulateApprovals()
            DDL_Approvals.Focus()
        End If
        If MultiView1.ActiveViewIndex = 1 Then 'generate approval
            Menu1.Items(0).ImageUrl = "~/Images/OrderOver_bttn.png"
            Menu1.Items(1).ImageUrl = "~/Images/OrderGenerateUp_bttn.png"
            Menu1.Items(2).ImageUrl = "~/Images/ReceiveOver.png"
            PopulateOrders()
            DDL_Orders.Focus()
        End If
        If MultiView1.ActiveViewIndex = 2 Then
            Menu1.Items(0).ImageUrl = "~/Images/OrderOver_bttn.png"
            Menu1.Items(1).ImageUrl = "~/Images/OrderGenerateOver_bttn.png"
            Menu1.Items(2).ImageUrl = "~/Images/ReceiveUp.png"
            PopulateOrders2()
            DDL_Orders2.Focus()
        End If

    End Sub
    'populate letter templates
    Public Sub PopulateLetters()
        Me.DDL_Letters.DataTextField = "FORM_NAME"
        Me.DDL_Letters.DataValueField = "MESS_ID"
        Me.DDL_Letters.DataSource = GetLetters(LibCode)
        Me.DDL_Letters.DataBind()
        DDL_Letters.Items.Insert(0, "")
    End Sub
    'populate Approved acqrecrods
    Public Sub PopulateApprovals()
        Dim Sel As String = "SELECT DISTINCT APP_NO FROM APPROVAL_VIEW2 WHERE (PROCESS_STATUS = 'Approved') AND (LIB_CODE ='" & (LibCode) & "') AND  (BIB_CODE <> 'S')  ORDER BY APP_NO DESC"
        Dim cmd As New SqlCommand(Sel, SqlConn)
        Dim rdr As SqlDataReader = Nothing
        Try
            SqlConn.Open()
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            If rdr.HasRows = True Then
                Me.DDL_Approvals.DataTextField = "APP_NO"
                Me.DDL_Approvals.DataValueField = "APP_NO"
                Me.DDL_Approvals.DataSource = rdr
                Me.DDL_Approvals.DataBind()
                DDL_Approvals.Items.Insert(0, "")
            Else
                Me.DDL_Approvals.DataSource = rdr
                Me.DDL_Approvals.DataBind()
            End If
            SqlConn.Close()
            Me.Grid3.DataSource = Nothing
            Grid3.DataBind()
        Catch ex As System.Exception
            If rdr IsNot Nothing Then
                rdr.Close()
            Else
                SqlConn.Close()
            End If
            ex.Message.ToString()
        End Try
    End Sub
    <System.Web.Script.Services.ScriptMethod(), _
System.Web.Services.WebMethod()> _
    Public Shared Function SearchAppNo(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim sdr As SqlDataReader
        Dim cmd As SqlCommand = New SqlCommand
        cmd.CommandText = "select DISTINCT APP_NO from ACQUISITIONS where (APP_NO like '" + prefixText + "%') and (PROCESS_STATUS ='Approved') AND (LIB_CODE ='" & LibCode & "')"
        cmd.Connection = SqlConn
        Try
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            Dim appno As List(Of String) = New List(Of String)
            sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            While sdr.Read
                appno.Add(sdr("APP_NO").ToString)
            End While
            Return appno
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
    Protected Sub DDL_Approvals_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDL_Approvals.SelectedIndexChanged
        Dim dt As DataTable
        Try
            Dim APP_NO As Object = Nothing
            If DDL_Approvals.Text <> "" Then
                APP_NO = DDL_Approvals.SelectedValue
                Dim SQL As String = Nothing
                SQL = "SELECT APP_DATE, COM_CODE  FROM ACQUISITIONS WHERE (APP_NO = '" & Trim(APP_NO) & "') AND  (LIB_CODE ='" & (LibCode) & "') "
                Dim ds As New DataSet
                Dim da As New SqlDataAdapter(SQL, SqlConn)
                If SqlConn.State = 0 Then
                    SqlConn.Open()
                End If
                da.Fill(ds)

                dt = ds.Tables(0).Copy
                SqlConn.Close()

                If dt.Rows.Count <> 0 Then
                    If dt.Rows(0).Item("APP_DATE").ToString <> "" Then
                        Label23.Text = "Approval Date: " & Format(dt.Rows(0).Item("APP_DATE"), "dd/MM/yyyy").ToString
                    Else
                        Label23.Text = ""
                    End If

                    If dt.Rows(0).Item("COM_CODE").ToString <> "" Then
                        Label24.Text = "Committee: " & dt.Rows(0).Item("COM_CODE").ToString
                    Else
                        Label24.Text = ""
                    End If
                Else
                    Label23.Text = ""
                    Label24.Text = ""
                End If
            Else
                Label23.Text = ""
                Label24.Text = ""
            End If
            PopulateAppGrid(APP_NO)
        Catch s As Exception
            Label7.Text = ""
            Label20.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'fill Grid with Approved Acq Records
    Public Sub PopulateAppGrid(ByVal APP_NO As Object)
        Dim dtSearch As DataTable = Nothing
        Try
            If APP_NO <> "" Then
                Dim SQL As String = Nothing
                SQL = "SELECT ACQ_ID, APP_NO, APP_DATE, VOL_NO, PROCESS_STATUS, COPY_PROPOSED, COPY_APPROVED, COPY_ORDERED, COM_CODE, TITLE, ORDER_NO, ORDER_DATE, VEND_NAME FROM APPROVAL_VIEW2 WHERE (APP_NO = '" & APP_NO & "')  AND (PROCESS_STATUS = 'Approved') AND (LIB_CODE ='" & (LibCode) & "') AND  (BIB_CODE <> 'S') ORDER BY ACQ_ID DESC"

                Dim ds As New DataSet
                Dim da As New SqlDataAdapter(SQL, SqlConn)
                If SqlConn.State = 0 Then
                    SqlConn.Open()
                End If
                da.Fill(ds)

                dtSearch = ds.Tables(0).Copy
                Dim RecordCount As Long = 0
                If dtSearch.Rows.Count = 0 Then
                    Me.Grid3.DataSource = Nothing
                    Grid3.DataBind()
                    Label1.Text = "Total Record(s): 0 "
                    Me.Ord_Cancel_Bttn.Visible = False
                    Ord_Delete_Bttn.Visible = False
                    Ord_Save_Bttn.Visible = False
                Else
                    Grid3.Visible = True
                    RecordCount = dtSearch.Rows.Count
                    Grid3.DataSource = dtSearch
                    Grid3.DataBind()
                    Label1.Text = "Total Record(s): " & RecordCount
                    Me.Ord_Cancel_Bttn.Visible = True
                    Ord_Delete_Bttn.Visible = True
                    Ord_Save_Bttn.Visible = True
                End If
                ViewState("dt") = dtSearch
            Else
                Me.Grid3.DataSource = Nothing
                Grid3.DataBind()
                Label1.Text = "Total Record(s): 0 "
                Me.Ord_Cancel_Bttn.Visible = False
                Ord_Delete_Bttn.Visible = False
                Ord_Save_Bttn.Visible = False
            End If
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
            Label15.Text = ""
        Finally
            SqlConn.Close()
            DDL_Approvals.Focus()
        End Try
    End Sub
    Public Sub PopulateVendors()
        DDL_Vendors.DataTextField = "VEND_NAME"
        DDL_Vendors.DataValueField = "VEND_ID"
        DDL_Vendors.DataSource = GetVendorList()
        DDL_Vendors.DataBind()
        DDL_Vendors.Items.Insert(0, "")
    End Sub
    'autocomplete in order no text box

    <System.Web.Script.Services.ScriptMethod(), _
System.Web.Services.WebMethod()> _
    Public Shared Function SearchOrdNo(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim sdr As SqlDataReader
        Dim cmd As SqlCommand = New SqlCommand
        cmd.CommandText = "select DISTINCT ORDER_NO from ACQUISITIONS where (ORDER_NO like '" + prefixText + "%') and (PROCESS_STATUS ='Approved') AND (LIB_CODE ='" & LibCode & "')"
        cmd.Connection = SqlConn
        Try
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            Dim OrdNo As List(Of String) = New List(Of String)
            sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            While sdr.Read
                OrdNo.Add(sdr("ORDER_NO").ToString)
            End While
            Return OrdNo
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
    Private Function ColumnEqual(ByVal A As Object, ByVal B As Object) As Boolean
        If A Is DBNull.Value And B Is DBNull.Value Then Return True ' Both are DBNull.Value.
        If A Is DBNull.Value Or B Is DBNull.Value Then Return False ' Only one is DBNull.Value.
        Return A = B                                                ' Value type standard comparison
    End Function
    'grid view page index changing event
    Protected Sub Grid3_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles Grid3.PageIndexChanging
        Try
            'rebind datagrid
            Grid3.DataSource = ViewState("dt") 'temp
            Grid3.PageIndex = e.NewPageIndex
            index = e.NewPageIndex * Grid3.PageSize
            Grid3.DataBind()
        Catch s As Exception
            Label6.Text = "Error:  there is error in page index !"
            Label15.Text = ""
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
    Protected Sub Grid3_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles Grid3.Sorting
        Dim temp As DataTable = CType(ViewState("dt"), DataTable)
        GridViewSortExpression = e.SortExpression
        Grid3.DataSource = temp
        Dim pageIndex As Integer = Grid3.PageIndex
        Grid3.DataSource = SortDataTable(Grid3.DataSource, False)
        Grid3.DataBind()
        Grid3.PageIndex = pageIndex
    End Sub
    Protected Sub Grid3_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Grid3.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Lblsr As Label = e.Row.FindControl("lblsr1")
            Srno = index + 1
            If Lblsr Is Nothing = False Then
                Lblsr.Text = Srno
                index += 1
            End If
            'fill copy Appd in copy appd text box when it is blank
            Dim txtsn As TextBox = DirectCast(e.Row.FindControl("txt_App_CopyOrd"), TextBox)
            If txtsn.Text = "" Then
                If e.Row.Cells(7).Text = "" Then
                    txtsn.Text = e.Row.Cells(6).Text
                End If
            End If
        End If
    End Sub
    Private Shared Function t() As Object
        Throw New NotImplementedException
    End Function
    'save order in selected records    
    Protected Sub Ord_Save_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Ord_Save_Bttn.Click
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing
        Try
            Dim counter1 As Integer
            Dim c As Integer
            Dim iloop As Integer
            Dim strcurrentchar As Object
            If DDL_Approvals.SelectedValue <> "" Then
                If txt_Acq_OrderNo.Text <> "" Then
                    If Grid3.Rows.Count <> 0 Then
                        For Each row As GridViewRow In Grid3.Rows
                            If row.RowType = DataControlRowType.DataRow Then
                                Dim cb As CheckBox = DirectCast(row.FindControl("cbd"), CheckBox)
                                Dim ID As Integer = Convert.ToInt32(Grid3.DataKeys(row.RowIndex).Value)
                                Dim CopyAppd As Int32 = Nothing
                                If Grid3.Rows(row.RowIndex).Cells(6).Text <> "" Then
                                    CopyAppd = Convert.ToInt32(Grid3.Rows(row.RowIndex).Cells(6).Text)
                                Else
                                    Label15.Text = ""
                                    Label6.Text = "Copy Approved Field is Blank!"
                                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Copy Approved Field is Blank!');", True)
                                    Exit Sub
                                End If

                                Dim txtsn As TextBox = DirectCast(row.FindControl("txt_App_CopyOrd"), TextBox)
                                Dim CopyOrd As Int32 = Nothing
                                If txtsn.Text <> "" Then
                                    CopyOrd = Convert.ToInt32(txtsn.Text)
                                Else
                                    CopyOrd = 0
                                End If

                                If cb IsNot Nothing AndAlso cb.Checked = True Then 'approved
                                    If txtsn.Text = "" Then
                                        Label15.Text = ""
                                        Label6.Text = "Plz Enter the No of Copies Approved!"
                                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Plz Enter the No of Copies being Ordered!');", True)
                                        txtsn.Enabled = True
                                        chckchkbox()
                                        txtsn.Focus()
                                        Exit Sub
                                    Else
                                        If CShort(CopyOrd) > CShort(CopyAppd) Then
                                            Label15.Text = ""
                                            Label6.Text = "No of Copies Ordered should be equal or less then copies Approved!"
                                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('No of Copies Ordered should be equal or less then copies Approved!');", True)
                                            txtsn.Enabled = True
                                            chckchkbox()
                                            txtsn.Focus()
                                            Exit Sub
                                        End If
                                    End If
                                End If


                                'validation for order No
                                Dim ORDER_NO As Object = Nothing
                                If txt_Acq_OrderNo.Text <> "" Then
                                    ORDER_NO = Trim(txt_Acq_OrderNo.Text)
                                    ORDER_NO = RemoveQuotes(ORDER_NO)
                                    If ORDER_NO.Length > 50 Then
                                        Label6.Text = "Error: Input is not Valid !"
                                        Label15.Text = ""
                                        txt_Acq_OrderNo.Focus()
                                        Exit Sub
                                    End If
                                    ORDER_NO = " " & ORDER_NO & " "
                                    If InStr(1, ORDER_NO, "CREATE", 1) > 0 Or InStr(1, ORDER_NO, "DELETE", 1) > 0 Or InStr(1, ORDER_NO, "DROP", 1) > 0 Or InStr(1, ORDER_NO, "INSERT", 1) > 1 Or InStr(1, ORDER_NO, "TRACK", 1) > 1 Or InStr(1, ORDER_NO, "TRACE", 1) > 1 Then
                                        Label6.Text = "Error: Input is not Valid !"
                                        Label15.Text = ""
                                        txt_Acq_OrderNo.Focus()
                                        Exit Sub
                                    End If
                                    ORDER_NO = TrimX(UCase(ORDER_NO))
                                    'check unwanted characters
                                    c = 0
                                    counter1 = 0
                                    For iloop = 1 To Len(ORDER_NO)
                                        strcurrentchar = Mid(ORDER_NO, iloop, 1)
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
                                        txt_Acq_OrderNo.Focus()
                                        Exit Sub
                                    End If

                                    'check duplicate order no
                                    Dim str As Object = Nothing
                                    Dim flag As Object = Nothing
                                    str = "SELECT ORDER_NO FROM ACQUISITIONS WHERE (ORDER_NO ='" & Trim(ORDER_NO) & "') AND (PROCESS_STATUS<>'Approved') AND (LIB_CODE ='" & Trim(LibCode) & "')"
                                    Dim cmd1 As New SqlCommand(str, SqlConn)
                                    SqlConn.Open()
                                    flag = cmd1.ExecuteScalar
                                    If flag <> Nothing Then
                                        Label6.Text = "This Order No has already been processed, Use another one !"
                                        Label15.Text = ""
                                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('This Order No has already been processed, Use another one !');", True)
                                        txt_Acq_OrderNo.Focus()
                                        Exit Sub
                                    End If
                                    SqlConn.Close()

                                Else
                                    Label6.Text = "Plz Type Order No. in the Text box !"
                                    Label15.Text = ""
                                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Plz Type Order No. in the Text box !');", True)
                                    txt_Acq_OrderNo.Focus()
                                    Exit Sub
                                End If

                                Dim DATE_MODIFIED As Object = Nothing
                                DATE_MODIFIED = Now.Date
                                DATE_MODIFIED = Convert.ToDateTime(DATE_MODIFIED, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                                Dim IP As Object = Nothing
                                IP = Request.UserHostAddress.Trim

                                'update record
                                Dim SQL As String = Nothing
                                SQL = "SELECT ACQ_ID, PROCESS_STATUS, COPY_APPROVED, COPY_ORDERED, ORDER_NO, DATE_MODIFIED, UPDATED_BY, IP FROM ACQUISITIONS WHERE (ACQ_ID = '" & ID & "')  AND (PROCESS_STATUS = 'Approved') AND (LIB_CODE ='" & (LibCode) & "') "

                                SqlConn.Open()

                                Dim da As SqlDataAdapter
                                Dim cmdb As SqlCommandBuilder
                                Dim ds As New DataSet
                                Dim dt As New DataTable

                                da = New SqlDataAdapter(SQL, SqlConn)
                                cmdb = New SqlCommandBuilder(da)
                                da.Fill(ds, "APP")
                                If ds.Tables("APP").Rows.Count <> 0 Then

                                    If cb IsNot Nothing AndAlso cb.Checked = True Then
                                        If ORDER_NO <> "" Then
                                            ds.Tables("APP").Rows(0)("ORDER_NO") = ORDER_NO
                                        End If
                                        If CopyOrd <> 0 Then
                                            ds.Tables("APP").Rows(0)("COPY_ORDERED") = CopyOrd
                                        End If
                                    Else
                                        'ds.Tables("APP").Rows(0)("ORDER_NO") = System.DBNull.Value
                                        'ds.Tables("APP").Rows(0)("COPY_ORDERED") = System.DBNull.Value
                                    End If

                                    ds.Tables("APP").Rows(0)("UPDATED_BY") = UserCode
                                    ds.Tables("APP").Rows(0)("DATE_MODIFIED") = DATE_MODIFIED
                                    ds.Tables("APP").Rows(0)("IP") = IP

                                    thisTransaction = SqlConn.BeginTransaction()
                                    da.SelectCommand.Transaction = thisTransaction
                                    da.Update(ds, "APP")
                                    thisTransaction.Commit()
                                End If
                            End If
                            SqlConn.Close()
                        Next
                        Label15.Text = "Record(s) Updated Successfully!"
                        Label6.Text = ""
                        PopulateAppGrid(DDL_Approvals.SelectedValue)
                        DDL_Approvals.Focus()
                    Else
                        Label15.Text = ""
                        Label6.Text = "Select Approval No from Drop-Down!"
                        DDL_Approvals.Focus()
                    End If
                Else
                    Label15.Text = ""
                    Label6.Text = "Enter Order No in the Text box!"
                    Me.txt_Acq_OrderNo.Focus()
                End If
            Else
                Label15.Text = ""
                Label6.Text = "Select Approval No from Drop-Down!"
                DDL_Approvals.Focus()
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
    'enable text box inside grid
    Public Sub chckchkbox()
        For Each row As GridViewRow In Grid3.Rows
            If row.RowType = DataControlRowType.DataRow Then
                Dim cb As CheckBox = DirectCast(row.FindControl("cbd"), CheckBox)
                Dim ID As Integer = Convert.ToInt32(Grid3.DataKeys(row.RowIndex).Value)

                Dim txtsn As TextBox = DirectCast(row.FindControl("txt_App_CopyOrd"), TextBox)
                If cb IsNot Nothing AndAlso cb.Checked = True Then
                    txtsn.Enabled = True
                Else
                    txtsn.Enabled = False
                End If
            End If
        Next
    End Sub
    'delete order no from selected recrods of grid
    Protected Sub Ord_Delete_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Ord_Delete_Bttn.Click
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing
        Try
            Dim counter1 As Integer
            Dim c As Integer
            Dim iloop As Integer
            Dim strcurrentchar As Object
            If Grid3.Rows.Count <> 0 Then
                For Each row As GridViewRow In Grid3.Rows
                    If row.RowType = DataControlRowType.DataRow Then
                        Dim cb As CheckBox = DirectCast(row.FindControl("cbd"), CheckBox)
                        If cb IsNot Nothing AndAlso cb.Checked = True Then
                            Dim ID As Integer = Convert.ToInt32(Grid3.DataKeys(row.RowIndex).Value)

                            Dim DATE_MODIFIED As Object = Nothing
                            DATE_MODIFIED = Now.Date
                            DATE_MODIFIED = Convert.ToDateTime(DATE_MODIFIED, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                            Dim IP As Object = Nothing
                            IP = Request.UserHostAddress.Trim

                            'update record
                            Dim SQL As String = Nothing
                            SQL = "SELECT ACQ_ID, PROCESS_STATUS, COPY_APPROVED, COPY_ORDERED, ORDER_NO, DATE_MODIFIED, UPDATED_BY, IP FROM ACQUISITIONS WHERE (ACQ_ID = '" & ID & "')  AND (PROCESS_STATUS = 'Approved') AND (LIB_CODE ='" & (LibCode) & "') "

                            SqlConn.Open()

                            Dim da As SqlDataAdapter
                            Dim cmdb As SqlCommandBuilder
                            Dim ds As New DataSet
                            Dim dt As New DataTable

                            da = New SqlDataAdapter(SQL, SqlConn)
                            cmdb = New SqlCommandBuilder(da)
                            da.Fill(ds, "APP")
                            If ds.Tables("APP").Rows.Count <> 0 Then
                                If cb IsNot Nothing AndAlso cb.Checked = True Then
                                    ds.Tables("APP").Rows(0)("ORDER_NO") = System.DBNull.Value
                                    ds.Tables("APP").Rows(0)("COPY_ORDERED") = System.DBNull.Value

                                    ds.Tables("APP").Rows(0)("UPDATED_BY") = UserCode
                                    ds.Tables("APP").Rows(0)("DATE_MODIFIED") = DATE_MODIFIED
                                    ds.Tables("APP").Rows(0)("IP") = IP

                                    thisTransaction = SqlConn.BeginTransaction()
                                    da.SelectCommand.Transaction = thisTransaction
                                    da.Update(ds, "APP")
                                    thisTransaction.Commit()
                                End If
                            End If
                        End If
                    End If
                    SqlConn.Close()
                Next
                Label15.Text = "Record(s) Updated Successfully!"
                Label6.Text = ""
                PopulateAppGrid(DDL_Approvals.SelectedValue)
                DDL_Approvals.Focus()
            Else
                Label15.Text = ""
                Label6.Text = "Select Approval No from Drop-Down!"
                DDL_Approvals.Focus()
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

    '***********************************************************************************************
    'ORDER PROCESSING START HERE
    'populate Approved acqrecrods
    Public Sub PopulateOrders()
        Dim Sel As String = "SELECT DISTINCT ORDER_NO FROM APPROVAL_VIEW2 WHERE (PROCESS_STATUS = 'Approved' OR PROCESS_STATUS = 'Ordered') AND (LIB_CODE ='" & (LibCode) & "') AND  (BIB_CODE <> 'S')  ORDER BY ORDER_NO DESC"
        Dim cmd As New SqlCommand(Sel, SqlConn)
        Dim rdr As SqlDataReader = Nothing
        Try
            SqlConn.Open()
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            If rdr.HasRows = True Then
                Me.DDL_Orders.DataTextField = "ORDER_NO"
                Me.DDL_Orders.DataValueField = "ORDER_NO"
                Me.DDL_Orders.DataSource = rdr
                Me.DDL_Orders.DataBind()
                DDL_Orders.Items.Insert(0, "")
            Else
                Me.DDL_Orders.DataSource = rdr
                Me.DDL_Orders.DataBind()
            End If
            SqlConn.Close()
            Me.Grid2.DataSource = Nothing
            Grid2.DataBind()
        Catch ex As System.Exception
            If rdr IsNot Nothing Then
                rdr.Close()
            Else
                SqlConn.Close()
            End If
            ex.Message.ToString()
        End Try
    End Sub
    'populate acq records in 2nd grid
    Protected Sub DDL_Orders_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDL_Orders.SelectedIndexChanged
        Dim dt As DataTable
        Try
            Dim ORDER_NO As Object = Nothing
            If DDL_Orders.Text <> "" Then
                ORDER_NO = DDL_Orders.SelectedValue
                Dim SQL As String = Nothing
                SQL = "SELECT ORDER_DATE, VEND_ID  FROM ACQUISITIONS WHERE (ORDER_NO = '" & Trim(ORDER_NO) & "') AND  (LIB_CODE ='" & (LibCode) & "') "
                Dim ds As New DataSet
                Dim da As New SqlDataAdapter(SQL, SqlConn)
                If SqlConn.State = 0 Then
                    SqlConn.Open()
                End If
                da.Fill(ds)

                dt = ds.Tables(0).Copy
                SqlConn.Close()

                Me.Grid2.DataSource = Nothing
                Grid2.DataBind()
                If dt.Rows.Count <> 0 Then
                    If dt.Rows(0).Item("ORDER_DATE").ToString <> "" Then
                        txt_Acq_OrdDate.Text = Format(dt.Rows(0).Item("ORDER_DATE"), "dd/MM/yyyy").ToString
                    Else
                        txt_Acq_OrdDate.Text = ""
                    End If

                    If dt.Rows(0).Item("VEND_ID").ToString <> "" Then
                        DDL_Vendors.SelectedValue = dt.Rows(0).Item("VEND_ID").ToString
                    Else
                        DDL_Vendors.Text = ""
                    End If
                Else
                    txt_Acq_OrdDate.Text = ""
                    DDL_Vendors.Text = ""
                End If
            Else
                txt_Acq_OrdDate.Text = ""
                DDL_Vendors.Text = ""
            End If
            PopulateOrderGrid(ORDER_NO)
        Catch s As Exception
            Label7.Text = ""
            Label20.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'fill Grid with Approved Acq Records
    Public Sub PopulateOrderGrid(ByVal ORDER_NO As Object)
        Dim dtSearch As DataTable = Nothing
        Try
            If ORDER_NO <> "" Then
                Dim SQL As String = Nothing
                SQL = "SELECT ACQ_ID, ORDER_NO, ORDER_DATE, VOL_NO, PROCESS_STATUS, COPY_PROPOSED, COPY_APPROVED, COPY_ORDERED, VEND_NAME, TITLE, SUB_TITLE, AUTHOR1, AUTHOR2, AUTHOR3, EDITION, PLACE_OF_PUB, YEAR_OF_PUB,PUB_PLACE, STANDARD_NO, PUB_NAME, CAT_NO FROM APPROVAL_VIEW2 WHERE (ORDER_NO = '" & ORDER_NO & "')  AND (PROCESS_STATUS = 'Approved' OR PROCESS_STATUS = 'Ordered') AND (LIB_CODE ='" & (LibCode) & "') AND  (BIB_CODE <> 'S') ORDER BY ACQ_ID DESC"

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
                    Label9.Text = "Total Record(s): 0 "
                    Ord_Process_Bttn.Visible = False
                    Ord_UnProcess_Bttn.Visible = False
                    Ord_Print_Bttn.Visible = False
                Else
                    Grid2.Visible = True
                    RecordCount = dtSearch.Rows.Count
                    Grid2.DataSource = dtSearch
                    Grid2.DataBind()
                    Label9.Text = "Total Record(s): " & RecordCount
                    Ord_Process_Bttn.Visible = True
                    Ord_UnProcess_Bttn.Visible = True
                    Ord_Print_Bttn.Visible = True
                End If
                ViewState("dt") = dtSearch
            Else
                Me.Grid2.DataSource = Nothing
                Grid2.DataBind()
                Label9.Text = "Total Record(s): 0 "
                Ord_Process_Bttn.Visible = False
                Ord_UnProcess_Bttn.Visible = False
                Ord_Print_Bttn.Visible = False
            End If
        Catch s As Exception
            Label20.Text = "Error: " & (s.Message())
            Label7.Text = ""
        Finally
            SqlConn.Close()
            DDL_Orders.Focus()
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
            Label20.Text = "Error:  there is error in page index !"
            Label7.Text = ""
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
    'process order
    Protected Sub App_Process_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Ord_Process_Bttn.Click
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing
        Dim iloop As Integer
        Dim strcurrentchar As Object
        Dim c As Integer
        Dim counter1, counter2 As Integer
        Try
            If DDL_Orders.SelectedValue <> "" Then
                If Grid2.Rows.Count <> 0 Then
                    For Each row As GridViewRow In Grid2.Rows
                        If row.RowType = DataControlRowType.DataRow Then
                            Dim ID As Integer = Convert.ToInt32(Grid2.DataKeys(row.RowIndex).Value)

                            'validation for Vendor name
                            Dim VEND_ID As Integer = Nothing
                            If Me.DDL_Vendors.Text <> "" Then
                                VEND_ID = Convert.ToInt16(TrimX(DDL_Vendors.SelectedValue))
                                VEND_ID = RemoveQuotes(VEND_ID)
                                If Not IsNumeric(VEND_ID) = True Then
                                    Label20.Text = "Error: Input is not Valid !"
                                    Label7.Text = ""
                                    DDL_Vendors.Focus()
                                    Exit Sub
                                End If
                                If Len(VEND_ID) > 8 Then
                                    Label20.Text = "Error: Input is not Valid !"
                                    Label7.Text = ""
                                    DDL_Vendors.Focus()
                                    Exit Sub
                                End If
                                VEND_ID = " " & VEND_ID & " "
                                If InStr(1, VEND_ID, "CREATE", 1) > 0 Or InStr(1, VEND_ID, "DELETE", 1) > 0 Or InStr(1, VEND_ID, "DROP", 1) > 0 Or InStr(1, VEND_ID, "INSERT", 1) > 1 Or InStr(1, VEND_ID, "TRACK", 1) > 1 Or InStr(1, VEND_ID, "TRACE", 1) > 1 Then
                                    Label20.Text = "Error: Input is not Valid !"
                                    Label7.Text = ""
                                    DDL_Vendors.Focus()
                                    Exit Sub
                                End If
                                VEND_ID = TrimAll(VEND_ID)
                                'check unwanted characters
                                c = 0
                                counter1 = 0
                                For iloop = 1 To Len(VEND_ID.ToString)
                                    strcurrentchar = Mid(VEND_ID, iloop, 1)
                                    If c = 0 Then
                                        If Not InStr("';<>=%""", strcurrentchar) <= 0 Then
                                            c = c + 1
                                            counter1 = 1
                                        End If
                                    End If
                                Next
                                If counter1 = 1 Then
                                    Label20.Text = "Error: Input is not Valid !"
                                    Label7.Text = ""
                                    DDL_Vendors.Focus()
                                    Exit Sub
                                End If
                            Else
                                Label20.Text = "Select Vendor from Drop-Down !"
                                Label7.Text = ""
                                DDL_Vendors.Focus()
                                Exit Sub
                            End If

                            'Order Date
                            'search start date
                            Dim ORDER_DATE As Object = Nothing
                            If txt_Acq_OrdDate.Text <> "" Then
                                ORDER_DATE = TrimX(txt_Acq_OrdDate.Text)
                                ORDER_DATE = RemoveQuotes(ORDER_DATE)
                                ORDER_DATE = Convert.ToDateTime(ORDER_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                                If Len(ORDER_DATE) > 12 Then
                                    Label20.Text = " Input is not Valid..."
                                    Label7.Text = ""
                                    Me.txt_Acq_OrdDate.Focus()
                                    Exit Sub
                                End If
                                ORDER_DATE = " " & ORDER_DATE & " "
                                If InStr(1, ORDER_DATE, "CREATE", 1) > 0 Or InStr(1, ORDER_DATE, "DELETE", 1) > 0 Or InStr(1, ORDER_DATE, "DROP", 1) > 0 Or InStr(1, ORDER_DATE, "INSERT", 1) > 1 Or InStr(1, ORDER_DATE, "TRACK", 1) > 1 Or InStr(1, ORDER_DATE, "TRACE", 1) > 1 Then
                                    Label20.Text = "  Input is not Valid... "
                                    Label7.Text = ""
                                    Me.txt_Acq_OrdDate.Focus()
                                    Exit Sub
                                End If
                                ORDER_DATE = TrimX(ORDER_DATE)
                                'check unwanted characters
                                c = 0
                                counter2 = 0
                                For iloop = 1 To Len(ORDER_DATE)
                                    strcurrentchar = Mid(ORDER_DATE, iloop, 1)
                                    If c = 0 Then
                                        If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                            c = c + 1
                                            counter2 = 1
                                        End If
                                    End If
                                Next
                                If counter2 = 1 Then
                                    Label20.Text = "data is not Valid... "
                                    Label7.Text = ""
                                    Me.txt_Acq_OrdDate.Focus()
                                    Exit Sub
                                End If
                            Else
                                ORDER_DATE = Now.Date
                                ORDER_DATE = Convert.ToDateTime(ORDER_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us
                            End If


                            Dim DATE_MODIFIED As Object = Nothing
                            DATE_MODIFIED = Now.Date
                            DATE_MODIFIED = Convert.ToDateTime(DATE_MODIFIED, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                            Dim IP As Object = Nothing
                            IP = Request.UserHostAddress.Trim

                            'update record
                            Dim SQL As String = Nothing
                            SQL = "SELECT ACQ_ID, PROCESS_STATUS, VEND_ID, ORDER_DATE, DATE_MODIFIED, UPDATED_BY, IP FROM ACQUISITIONS WHERE (ACQ_ID = '" & ID & "')  AND (PROCESS_STATUS = 'Approved' or PROCESS_STATUS = 'Ordered') AND (LIB_CODE ='" & (LibCode) & "') "

                            SqlConn.Open()

                            Dim da As SqlDataAdapter
                            Dim cmdb As SqlCommandBuilder
                            Dim ds As New DataSet
                            Dim dt As New DataTable

                            da = New SqlDataAdapter(SQL, SqlConn)
                            cmdb = New SqlCommandBuilder(da)
                            da.Fill(ds, "APP")
                            If ds.Tables("APP").Rows.Count <> 0 Then
                                If Not String.IsNullOrEmpty(VEND_ID) Then
                                    ds.Tables("APP").Rows(0)("VEND_ID") = VEND_ID
                                    ds.Tables("APP").Rows(0)("PROCESS_STATUS") = "Ordered"
                                End If
                                If Not String.IsNullOrEmpty(ORDER_DATE) Then
                                    ds.Tables("APP").Rows(0)("ORDER_DATE") = ORDER_DATE
                                End If

                                ds.Tables("APP").Rows(0)("UPDATED_BY") = UserCode
                                ds.Tables("APP").Rows(0)("DATE_MODIFIED") = DATE_MODIFIED
                                ds.Tables("APP").Rows(0)("IP") = IP

                                thisTransaction = SqlConn.BeginTransaction()
                                da.SelectCommand.Transaction = thisTransaction
                                da.Update(ds, "APP")
                                thisTransaction.Commit()
                            End If
                        End If
                        SqlConn.Close()
                    Next
                    Label7.Text = "Record(s) Updated Successfully!"
                    Label20.Text = ""
                    PopulateOrderGrid(DDL_Orders.SelectedValue)
                    DDL_Orders.Focus()
                Else
                    Label7.Text = ""
                    Label20.Text = "Select Approval No from Drop-Down!"
                    DDL_Orders.Focus()
                End If
            Else
                Label7.Text = ""
                Label20.Text = "Select Approval No from Drop-Down!"
                DDL_Orders.Focus()
            End If
        Catch q As SqlException
            thisTransaction.Rollback()
        Catch s As Exception
            Label7.Text = ""
            Label20.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'UN-process order
    Protected Sub Ord_UnProcess_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Ord_UnProcess_Bttn.Click
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing
        Dim iloop As Integer
        Dim strcurrentchar As Object
        Dim c As Integer
        Dim counter1, counter2 As Integer
        Try
            If DDL_Orders.SelectedValue <> "" Then
                If Grid2.Rows.Count <> 0 Then
                    For Each row As GridViewRow In Grid2.Rows
                        If row.RowType = DataControlRowType.DataRow Then
                            Dim ID As Integer = Convert.ToInt32(Grid2.DataKeys(row.RowIndex).Value)

                            Dim DATE_MODIFIED As Object = Nothing
                            DATE_MODIFIED = Now.Date
                            DATE_MODIFIED = Convert.ToDateTime(DATE_MODIFIED, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                            Dim IP As Object = Nothing
                            IP = Request.UserHostAddress.Trim

                            'update record
                            Dim SQL As String = Nothing
                            SQL = "SELECT ACQ_ID, PROCESS_STATUS, VEND_ID, ORDER_DATE, DATE_MODIFIED, UPDATED_BY, IP, COPY_ORDERED FROM ACQUISITIONS WHERE (ACQ_ID = '" & ID & "')  AND (PROCESS_STATUS = 'Approved' or PROCESS_STATUS = 'Ordered') AND (LIB_CODE ='" & (LibCode) & "') "

                            SqlConn.Open()

                            Dim da As SqlDataAdapter
                            Dim cmdb As SqlCommandBuilder
                            Dim ds As New DataSet
                            Dim dt As New DataTable

                            da = New SqlDataAdapter(SQL, SqlConn)
                            cmdb = New SqlCommandBuilder(da)
                            da.Fill(ds, "APP")
                            If ds.Tables("APP").Rows.Count <> 0 Then
                                ds.Tables("APP").Rows(0)("VEND_ID") = System.DBNull.Value
                                ds.Tables("APP").Rows(0)("PROCESS_STATUS") = "Approved"
                                ds.Tables("APP").Rows(0)("ORDER_DATE") = System.DBNull.Value
                                ds.Tables("APP").Rows(0)("COPY_ORDERED") = System.DBNull.Value
                                ds.Tables("APP").Rows(0)("UPDATED_BY") = UserCode
                                ds.Tables("APP").Rows(0)("DATE_MODIFIED") = DATE_MODIFIED
                                ds.Tables("APP").Rows(0)("IP") = IP
                                thisTransaction = SqlConn.BeginTransaction()
                                da.SelectCommand.Transaction = thisTransaction
                                da.Update(ds, "APP")
                                thisTransaction.Commit()
                            End If
                        End If
                        SqlConn.Close()
                    Next
                    Label7.Text = "Order Info deleted Successfully!"
                    Label20.Text = ""
                    PopulateOrderGrid(DDL_Orders.SelectedValue)
                    DDL_Orders.Focus()
                Else
                    Label7.Text = ""
                    Label20.Text = "Select Approval No from Drop-Down!"
                    DDL_Orders.Focus()
                End If
            Else
                Label7.Text = ""
                Label20.Text = "Select Approval No from Drop-Down!"
                DDL_Orders.Focus()
            End If
        Catch q As SqlException
            thisTransaction.Rollback()
        Catch s As Exception
            Label7.Text = ""
            Label20.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    '***********************************************************************************************
    '***********************************************************************************************
    'Received Documents
    'populate Orders in Drop-down
    Public Sub PopulateOrders2()
        Dim Sel As String = "SELECT DISTINCT ORDER_NO FROM APPROVAL_VIEW2 WHERE (PROCESS_STATUS = 'Ordered' OR PROCESS_STATUS = 'Received' OR PROCESS_STATUS = 'Partially Received' OR PROCESS_STATUS = 'Partially Accessioned') AND (LIB_CODE ='" & (LibCode) & "') AND  (BIB_CODE <> 'S')  ORDER BY ORDER_NO DESC"
        Dim cmd As New SqlCommand(Sel, SqlConn)
        Dim rdr As SqlDataReader = Nothing
        Try
            SqlConn.Open()
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            If rdr.HasRows = True Then
                Me.DDL_Orders2.DataTextField = "ORDER_NO"
                Me.DDL_Orders2.DataValueField = "ORDER_NO"
                Me.DDL_Orders2.DataSource = rdr
                Me.DDL_Orders2.DataBind()
                DDL_Orders2.Items.Insert(0, "")
            Else
                Me.DDL_Orders2.DataSource = rdr
                Me.DDL_Orders2.DataBind()
            End If
            SqlConn.Close()
            Me.Grid1.DataSource = Nothing
            Grid1.DataBind()
        Catch ex As System.Exception
            If rdr IsNot Nothing Then
                rdr.Close()
            Else
                SqlConn.Close()
            End If
            Label10.Text = ex.Message.ToString()
        End Try
    End Sub
    'populate Grid1
    Protected Sub DDL_Orders2_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDL_Orders2.SelectedIndexChanged
        Dim dt As DataTable
        Try
            Dim ORDER_NO As Object = Nothing
            If DDL_Orders2.Text <> "" Then
                ORDER_NO = DDL_Orders2.SelectedValue
                Dim SQL As String = Nothing
                SQL = "SELECT ORDER_DATE, VEND_NAME  FROM APPROVAL_VIEW2 WHERE (ORDER_NO = '" & Trim(ORDER_NO) & "') AND  (LIB_CODE ='" & (LibCode) & "') "
                Dim ds As New DataSet
                Dim da As New SqlDataAdapter(SQL, SqlConn)
                If SqlConn.State = 0 Then
                    SqlConn.Open()
                End If
                da.Fill(ds)

                dt = ds.Tables(0).Copy
                SqlConn.Close()

                If dt.Rows.Count <> 0 Then
                    If dt.Rows(0).Item("ORDER_DATE").ToString <> "" Then
                        Label14.Text = "Order Date: " & Format(dt.Rows(0).Item("ORDER_DATE"), "dd/MM/yyyy").ToString
                    Else
                        Label14.Text = ""
                    End If

                    If dt.Rows(0).Item("VEND_NAME").ToString <> "" Then
                        Label13.Text = dt.Rows(0).Item("VEND_NAME").ToString
                    Else
                        Label13.Text = ""
                    End If
                Else
                    Label14.Text = ""
                    Label13.Text = ""
                End If
            Else
                Label14.Text = ""
                Label13.Text = ""
            End If
            PopulateOrderGrid1(ORDER_NO)
        Catch s As Exception
            Label8.Text = ""
            Label10.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'fill Grid with Approved Acq Records
    Public Sub PopulateOrderGrid1(ByVal ORDER_NO As Object)
        Dim dtSearch As DataTable = Nothing
        Try
            If ORDER_NO <> "" Then
                Dim SQL As String = Nothing
                SQL = "SELECT ACQ_ID, ORDER_NO, ORDER_DATE, VOL_NO, PROCESS_STATUS, COPY_PROPOSED, COPY_APPROVED, COPY_ORDERED, COPY_RECEIVED, VEND_NAME, TITLE FROM APPROVAL_VIEW2 WHERE (ORDER_NO = '" & ORDER_NO & "')  AND (PROCESS_STATUS = 'Received' OR PROCESS_STATUS = 'Ordered' OR PROCESS_STATUS = 'Partially Received') AND (LIB_CODE ='" & (LibCode) & "') AND  (BIB_CODE <> 'S') ORDER BY ACQ_ID DESC"

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
                    Label11.Text = "Total Record(s): 0 "
                    Acq_Recv_Bttn.Visible = False
                Else
                    Grid1.Visible = True
                    RecordCount = dtSearch.Rows.Count
                    Grid1.DataSource = dtSearch
                    Grid1.DataBind()
                    Label11.Text = "Total Record(s): " & RecordCount
                    Acq_Recv_Bttn.Visible = True
                End If
                ViewState("dt") = dtSearch
            Else
                Me.Grid1.DataSource = Nothing
                Grid1.DataBind()
                Label11.Text = "Total Record(s): 0 "
                Acq_Recv_Bttn.Visible = False
            End If
        Catch s As Exception
            Label10.Text = "Error: " & (s.Message())
            Label8.Text = ""
        Finally
            SqlConn.Close()
            DDL_Orders2.Focus()
        End Try
    End Sub
    'grid view page index changing event
    Protected Sub Grid1_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles Grid1.PageIndexChanging
        Try
            'rebind datagrid
            Grid1.DataSource = ViewState("dt") 'temp
            Grid1.PageIndex = e.NewPageIndex
            index = e.NewPageIndex * Grid1.PageSize
            Grid1.DataBind()
        Catch s As Exception
            Label10.Text = "Error:  there is error in page index !"
            Label8.Text = ""
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
    Protected Sub Grid1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Grid1.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Lblsr As Label = e.Row.FindControl("lblsr")
            Srno = index + 1
            If Lblsr Is Nothing = False Then
                Lblsr.Text = Srno
                index += 1
            End If
            'fill copy Appd in copy appd text box when it is blank
            Dim txtsn As TextBox = DirectCast(e.Row.FindControl("txt_App_CopyRecd"), TextBox)
            If txtsn.Text = "" Then
                If e.Row.Cells(9).Text = "" Then
                    txtsn.Text = e.Row.Cells(8).Text
                End If
            End If
        End If
    End Sub
    'receive documents update table
    Protected Sub Acq_Recv_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Acq_Recv_Bttn.Click
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing
        Try
            Dim counter1 As Integer
            Dim c As Integer
            Dim iloop As Integer
            Dim strcurrentchar As Object
            If DDL_Orders2.SelectedValue <> "" Then
                If Grid1.Rows.Count <> 0 Then
                    For Each row As GridViewRow In Grid1.Rows
                        If row.RowType = DataControlRowType.DataRow Then
                            Dim cb As CheckBox = DirectCast(row.FindControl("cbd"), CheckBox)
                            Dim ID As Integer = Convert.ToInt32(Grid1.DataKeys(row.RowIndex).Value)
                            Dim CopyOrd As Int32 = Nothing
                            If Grid1.Rows(row.RowIndex).Cells(8).Text <> "" Then
                                CopyOrd = Convert.ToInt32(Grid1.Rows(row.RowIndex).Cells(8).Text)
                            Else
                                Label8.Text = ""
                                Label10.Text = "Copy Ordered Field is Blank!"
                                ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Copy Ordered Field is Blank!');", True)
                                Exit Sub
                            End If

                            Dim txtsn As TextBox = DirectCast(row.FindControl("txt_App_CopyRecd"), TextBox)
                            Dim CopyRecd As Int32 = Nothing
                            If txtsn.Text <> "" Then
                                CopyRecd = Convert.ToInt32(txtsn.Text)
                            Else
                                CopyRecd = 0
                            End If

                            If cb IsNot Nothing AndAlso cb.Checked = True Then 'approved
                                If txtsn.Text = "" Then
                                    Label8.Text = ""
                                    Label10.Text = "Plz Enter the No of Copies being Received!"
                                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Plz Enter the No of Copies being Received!');", True)
                                    txtsn.Enabled = True
                                    chckchkboxX()
                                    txtsn.Focus()
                                    Exit Sub
                                Else
                                    If CShort(CopyRecd) > CShort(CopyOrd) Then
                                        Label8.Text = ""
                                        Label10.Text = "No of Copies Received should be equal or less then copies Ordered!"
                                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('No of Copies Received should be equal or less then copies Ordered!');", True)
                                        txtsn.Enabled = True
                                        chckchkboxX()
                                        txtsn.Focus()
                                        Exit Sub
                                    End If
                                End If
                            End If

                            Dim DATE_MODIFIED As Object = Nothing
                            DATE_MODIFIED = Now.Date
                            DATE_MODIFIED = Convert.ToDateTime(DATE_MODIFIED, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                            Dim IP As Object = Nothing
                            IP = Request.UserHostAddress.Trim

                            'update record
                            Dim SQL As String = Nothing
                            SQL = "SELECT ACQ_ID, PROCESS_STATUS, COPY_RECEIVED, DATE_MODIFIED, UPDATED_BY, IP FROM ACQUISITIONS WHERE (ACQ_ID = '" & ID & "')  AND (LIB_CODE ='" & (LibCode) & "') "

                            SqlConn.Open()

                            Dim da As SqlDataAdapter
                            Dim cmdb As SqlCommandBuilder
                            Dim ds As New DataSet
                            Dim dt As New DataTable

                            da = New SqlDataAdapter(SQL, SqlConn)
                            cmdb = New SqlCommandBuilder(da)
                            da.Fill(ds, "APP")
                            If ds.Tables("APP").Rows.Count <> 0 Then

                                If cb IsNot Nothing AndAlso cb.Checked = True Then
                                    If CShort(CopyRecd) < CShort(CopyOrd) Then
                                        ds.Tables("APP").Rows(0)("PROCESS_STATUS") = "Partially Received"
                                    Else
                                        ds.Tables("APP").Rows(0)("PROCESS_STATUS") = "Received"
                                    End If
                                    ds.Tables("APP").Rows(0)("COPY_RECEIVED") = CopyRecd
                                Else
                                    ds.Tables("APP").Rows(0)("COPY_RECEIVED") = System.DBNull.Value
                                    ds.Tables("APP").Rows(0)("PROCESS_STATUS") = "Ordered"
                                End If

                                ds.Tables("APP").Rows(0)("UPDATED_BY") = UserCode
                                    ds.Tables("APP").Rows(0)("DATE_MODIFIED") = DATE_MODIFIED
                                    ds.Tables("APP").Rows(0)("IP") = IP

                                    thisTransaction = SqlConn.BeginTransaction()
                                    da.SelectCommand.Transaction = thisTransaction
                                    da.Update(ds, "APP")
                                    thisTransaction.Commit()
                                End If
                        End If
                        SqlConn.Close()
                    Next
                    Label8.Text = "Record(s) Updated Successfully!"
                    Label10.Text = ""
                    PopulateOrderGrid1(DDL_Orders2.SelectedValue)
                    DDL_Orders2.Focus()
                Else
                    Label8.Text = ""
                    Label10.Text = "Select Approval No from Drop-Down!"
                    DDL_Orders2.Focus()
                End If
            Else
                Label8.Text = ""
                Label10.Text = "select Order From Drop-Down!"
                DDL_Orders2.Focus()
            End If
           
        Catch q As SqlException
            thisTransaction.Rollback()
        Catch s As Exception
            Label8.Text = ""
            Label10.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'enable text box inside grid
    Public Sub chckchkboxX()
        For Each row As GridViewRow In Grid3.Rows
            If row.RowType = DataControlRowType.DataRow Then
                Dim cb As CheckBox = DirectCast(row.FindControl("cbd"), CheckBox)
                Dim ID As Integer = Convert.ToInt32(Grid1.DataKeys(row.RowIndex).Value)

                Dim txtsn As TextBox = Nothing
                txtsn = DirectCast(row.FindControl("txt_App_CopyRecd"), TextBox)
                If cb IsNot Nothing AndAlso cb.Checked = True Then
                    txtsn.Enabled = True
                Else
                    txtsn.Enabled = False
                End If
            End If
        Next
    End Sub
    'load report
    Protected Sub Ord_Print_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Ord_Print_Bttn.Click
        Try
            If DDL_PrintFormats.Text = "" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Plz Select Value from Drop-Down!');", True)
                DDL_PrintFormats.Focus()
                Exit Sub
            End If

            If DDL_Orders.Text = "" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Plz Select Value from Drop-Down!');", True)
                DDL_Orders.Focus()
                Exit Sub
            End If

            If DDL_Vendors.Text = "" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Plz Select Value from Drop-Down!');", True)
                DDL_Vendors.Focus()
                Exit Sub
            End If

            If DDL_Letters.Text = "" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Plz Select Letter Template from Drop-Down!');", True)
                DDL_Letters.Focus()
                Exit Sub
            End If


            Dim Reportdoc As New ReportDocument
            Reportdoc = Report_Load_Letter()

            If DDL_PrintFormats.SelectedValue = "PDF" Then
                Reportdoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "Report_Order_Books_" + Format(DateTime.Now, "ddMMyyyy"))
            End If
            If DDL_PrintFormats.SelectedValue = "DOC" Then
                Reportdoc.ExportToHttpResponse(ExportFormatType.WordForWindows, Response, True, "Report_Order_Books_" + Format(DateTime.Now, "ddMMyyyy"))
            End If
        Catch ex As Exception
            Label20.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    'print approval form
    Public Function Report_Load_Letter() As ReportDocument
        Dim dtMess As DataTable = Nothing
        Dim dtVend As DataTable = Nothing
        Dim dt As DataTable = Nothing
        Dim Rpt As New ReportDocument
        Try
            Dim ORDER_NO As Object = Nothing
            If DDL_Orders.Text <> "" Then
                ORDER_NO = DDL_Orders.SelectedValue
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Plz Select Value from Drop-Down!');", True)
                DDL_Orders.Focus()
                Exit Function
            End If

            Dim VEND_ID As Integer = Nothing
            If DDL_Vendors.Text <> "" Then
                VEND_ID = DDL_Vendors.SelectedValue
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Plz Select Value from Drop-Down!');", True)
                DDL_Vendors.Focus()
                Exit Function
            End If

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

            'Get Vendor details
            Dim ds2 As New DataSet
            Dim SQL2 As String = Nothing
            SQL2 = "SELECT * FROM VENDORS WHERE (VEND_ID = '" & Trim(VEND_ID) & "') ;"

            Dim da2 As New SqlDataAdapter(SQL2, SqlConn)
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            da2.Fill(ds2)

            dtVend = ds2.Tables(0).Copy

            Dim myVendName As Object = Nothing
            Dim myVendAddress As Object = Nothing
            Dim myVendEmail As Object = Nothing
            Dim myVendPlace As Object = Nothing

            If dtVend.Rows.Count <> 0 Then
                If dtVend.Rows(0).Item("VEND_NAME").ToString <> "" Then
                    myVendName = Trim(dtVend.Rows(0).Item("VEND_NAME").ToString)
                Else
                    myVendName = ""
                End If

                If dtVend.Rows(0).Item("VEND_ADDRESS").ToString <> "" Then
                    myVendAddress = Trim(dtVend.Rows(0).Item("VEND_ADDRESS").ToString)
                Else
                    myVendAddress = ""
                End If

                If dtVend.Rows(0).Item("VEND_EMAIL").ToString <> "" Then
                    myVendEmail = Trim(dtVend.Rows(0).Item("VEND_EMAIL").ToString)
                Else
                    myVendEmail = ""
                End If

                If dtVend.Rows(0).Item("VEND_PLACE").ToString <> "" Then
                    myVendPlace = Trim(dtVend.Rows(0).Item("VEND_PLACE").ToString)
                Else
                    myVendPlace = ""
                End If
            Else
                myVendName = Nothing
                myVendAddress = Nothing
                myVendEmail = Nothing
                myVendPlace = Nothing
            End If

            dt = ViewState("dt")
            If dt.Rows.Count <> 0 Then
                Rpt.Load(Server.MapPath("~/Reports/Books_Order_Report.rpt"))
                Rpt.SetDataSource(dt)
                Rpt.Refresh()

                Dim FileNoText As CrystalReports.Engine.TextObject
                If myFileNo <> "" Then
                    FileNoText = Rpt.ReportDefinition.Sections("ReportHeaderSection1").ReportObjects.Item("Text13")
                    FileNoText.Text = myFileNo
                End If

                Dim OrderNoText As CrystalReports.Engine.TextObject
                If ORDER_NO <> "" Then
                    OrderNoText = Rpt.ReportDefinition.Sections("ReportHeaderSection2").ReportObjects.Item("Text7")
                    OrderNoText.Text = ORDER_NO
                Else
                    ' Rpt.ReportDefinition.Sections("ReportHeaderSection2").SectionFormat.EnableSuppress = True
                End If

                Dim VendNameText As CrystalReports.Engine.TextObject
                If myVendName <> "" Then
                    VendNameText = Rpt.ReportDefinition.Sections("ReportHeaderSection5").ReportObjects.Item("Text17")
                    VendNameText.Text = myVendName
                Else
                    Rpt.ReportDefinition.Sections("ReportHeaderSection5").SectionFormat.EnableSuppress = True
                End If

                Dim VendAddressText As CrystalReports.Engine.TextObject
                If myVendAddress <> "" Then
                    VendAddressText = Rpt.ReportDefinition.Sections("ReportHeaderSection6").ReportObjects.Item("Text18")
                    VendAddressText.Text = myVendAddress
                Else
                    Rpt.ReportDefinition.Sections("ReportHeaderSection6").SectionFormat.EnableSuppress = True
                End If

                Dim SubjectText As CrystalReports.Engine.TextObject
                If mySubject <> "" Then
                    SubjectText = Rpt.ReportDefinition.Sections("ReportHeaderSection4").ReportObjects.Item("Text15")
                    SubjectText.Text = mySubject
                Else
                    Rpt.ReportDefinition.Sections("ReportHeaderSection4").SectionFormat.EnableSuppress = True
                End If

                Dim SalutationText As CrystalReports.Engine.TextObject
                If mySalutation <> "" Then
                    SalutationText = Rpt.ReportDefinition.Sections("ReportHeaderSection3").ReportObjects.Item("Text8")
                    SalutationText.Text = mySalutation
                End If

                Dim TopMessageText As CrystalReports.Engine.TextObject
                If myTopMessage <> "" Then
                    TopMessageText = Rpt.ReportDefinition.Sections("ReportHeaderSection3").ReportObjects.Item("Text10")
                    TopMessageText.Text = myTopMessage
                Else
                    'Rpt.ReportDefinition.Sections("ReportHeaderSection3").SectionFormat.EnableSuppress = True
                End If

                Dim BottomMessageText As CrystalReports.Engine.TextObject
                If myBottomMessage <> "" Then
                    BottomMessageText = Rpt.ReportDefinition.Sections("ReportFooterSection1").ReportObjects.Item("Text12")
                    BottomMessageText.Text = myBottomMessage
                Else
                    'Rpt.ReportDefinition.Sections("ReportFooterSection1").SectionFormat.EnableSuppress = True
                End If

                Dim SignAuthorityText As CrystalReports.Engine.TextObject
                If mySignAuthority <> "" Then
                    SignAuthorityText = Rpt.ReportDefinition.Sections("ReportFooterSection2").ReportObjects.Item("Text16")
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

   
End Class