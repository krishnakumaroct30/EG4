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

Public Class Accessioning
    Inherits System.Web.UI.Page
    Dim index As Integer = 0
    Public Srno As Integer = 0
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session.Item("IsThisU") <> Nothing Then
                If Not SConnection() = True Then
                    Label10.Text = "Database Connection is lost..Try Again !'"
                    Label8.Text = ""
                Else
                    LibCode = Session.Item("LoggedLibcode")
                    If Page.IsPostBack = False Then
                        Hold_Save_Bttn.Visible = True
                        Hold_Update_Bttn.Visible = False
                        Hold_Cancel_Bttn.Visible = True
                        PopulateOrders()
                        PopulateSections()
                        PopulateLibrary()
                        PopulateAccMaterials()
                        PopulateBindings()
                        PopulateFormats()
                        PopulateSections()
                        PopulateStatus()
                        DDL_Format.SelectedValue = "PT"
                        DDL_Show.SelectedValue = "Y"
                        DDL_Issuable.SelectedValue = "Y"
                        DDL_CollectionType.SelectedValue = "C"
                        DDL_Status.SelectedValue = "1"
                        DDL_Binding.SelectedValue = "P"
                        DDL_Library.SelectedValue = LibCode
                    End If
                    Dim CreateSUserButton As Object 'System.Web.UI.WebControls.Button
                    CreateSUserButton = Master.FindControl("Accordion1").FindControl("AcqPane").FindControl("Acq_Accessioing_Bttn")
                    CreateSUserButton.ForeColor = Drawing.Color.Red
                    myPaneName = "AcqPane"
                End If
            Else
                Response.Redirect("~/Default.aspx", False)
            End If
        Catch ex As Exception
            Label10.Text = "Error: " & (ex.Message())
            Label8.Text = ""
        End Try
    End Sub
    'populate Orders in Drop-down
    Public Sub PopulateOrders()
        Dim Sel As String = "SELECT DISTINCT ORDER_NO FROM APPROVAL_VIEW2 WHERE (PROCESS_STATUS = 'Accessioned'  OR PROCESS_STATUS = 'Partially Accessioned' OR PROCESS_STATUS = 'Received') AND (LIB_CODE ='" & (LibCode) & "') AND  (BIB_CODE <> 'S')  ORDER BY ORDER_NO DESC"
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
            DDL_Orders.Focus()
        Catch ex As System.Exception
            If rdr IsNot Nothing Then
                rdr.Close()
            Else
                SqlConn.Close()
            End If
            Label10.Text = ex.Message.ToString()
            Label8.Text = ""
        Finally
            DDL_Orders.Focus()
        End Try
    End Sub
    'populate Grid1
    Protected Sub DDL_Orders_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDL_Orders.SelectedIndexChanged
        Dim dt As DataTable
        Try
            Dim ORDER_NO As Object = Nothing
            If DDL_Orders.Text <> "" Then
                ORDER_NO = DDL_Orders.SelectedValue
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
                Me.Grid2.DataSource = Nothing
                Grid2.DataBind()
                ClearFields()
                Label_CATNO.Text = ""
                Label_ACQID.Text = ""
                DDL_YN.Text = ""
                Label_MATTYPE.Text = ""
                Label_DOCTYPE.Text = ""
                Label_TITLE.Text = ""

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

                    Label_CATNO.Text = ""
                    Label_ACQID.Text = ""
                    DDL_YN.Text = ""
                    Label_MATTYPE.Text = ""
                    Label_DOCTYPE.Text = ""
                    Label_TITLE.Text = ""
                    Me.Grid2.DataSource = Nothing
                    Grid2.DataBind()
                    ClearFields()
                    Label_HOLDID.Text = ""
                    Me.Hold_Save_Bttn.Visible = True
                    Me.Hold_Update_Bttn.Visible = False
                End If
            Else
                Label14.Text = ""
                Label13.Text = ""

                Label_CATNO.Text = ""
                Label_ACQID.Text = ""
                DDL_YN.Text = ""
                Label_MATTYPE.Text = ""
                Label_DOCTYPE.Text = ""
                Label_TITLE.Text = ""
                Me.Grid2.DataSource = Nothing
                Grid2.DataBind()
                ClearFields()
                Label_HOLDID.Text = ""
                Me.Hold_Save_Bttn.Visible = True
                Me.Hold_Update_Bttn.Visible = False
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
                SQL = "SELECT ACQ_ID, ORDER_NO, ORDER_DATE, VOL_NO, PROCESS_STATUS, COPY_PROPOSED, COPY_APPROVED, COPY_ORDERED, COPY_RECEIVED,COPY_ACCESSIONED, VEND_NAME, TITLE FROM APPROVAL_VIEW2 WHERE (ORDER_NO = '" & ORDER_NO & "')  AND (PROCESS_STATUS = 'Received' OR PROCESS_STATUS = 'Accessioned' OR PROCESS_STATUS = 'Partially Accessioned') AND (LIB_CODE ='" & (LibCode) & "') AND  (BIB_CODE <> 'S') ORDER BY ACQ_ID DESC"

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
                Else
                    Grid1.Visible = True
                    RecordCount = dtSearch.Rows.Count
                    Grid1.DataSource = dtSearch
                    Grid1.DataBind()
                    Label11.Text = "Total Record(s): " & RecordCount
                End If
                ViewState("dt") = dtSearch
            Else
                Me.Grid1.DataSource = Nothing
                Grid1.DataBind()
                Label11.Text = "Total Record(s): 0 "
            End If
        Catch s As Exception
            Label10.Text = "Error: " & (s.Message())
            Label8.Text = ""
        Finally
            SqlConn.Close()
            DDL_Orders.Focus()
        End Try
    End Sub
    Private Function ColumnEqual(ByVal A As Object, ByVal B As Object) As Boolean
        If A Is DBNull.Value And B Is DBNull.Value Then Return True ' Both are DBNull.Value.
        If A Is DBNull.Value Or B Is DBNull.Value Then Return False ' Only one is DBNull.Value.
        Return A = B                                                ' Value type standard comparison
    End Function
    'grid view page index changing event
    Protected Sub Grid_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles Grid1.PageIndexChanging
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
    Protected Sub Grid_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles Grid1.Sorting
        Dim temp As DataTable = CType(ViewState("dt"), DataTable)
        GridViewSortExpression = e.SortExpression
        Grid1.DataSource = temp
        Dim pageIndex As Integer = Grid1.PageIndex
        Grid1.DataSource = SortDataTable(Grid1.DataSource, False)
        Grid1.DataBind()
        Grid1.PageIndex = pageIndex
    End Sub
    Protected Sub Grid_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Grid1.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Lblsr As Label = e.Row.FindControl("lblsr")
            Srno = index + 1
            If Lblsr Is Nothing = False Then
                Lblsr.Text = Srno
                index += 1
            End If
        End If
    End Sub
    Private Shared Function t() As Object
        Throw New NotImplementedException
    End Function
    Public Sub PopulateFormats()
        DDL_Format.DataTextField = "FORMAT_NAME"
        DDL_Format.DataValueField = "FORMAT_CODE"
        DDL_Format.DataSource = GetFormatList()
        DDL_Format.DataBind()
        DDL_Format.Items.Insert(0, "")
    End Sub
    Public Sub PopulateStatus()
        DDL_Status.DataTextField = "STA_NAME"
        DDL_Status.DataValueField = "STA_CODE"
        DDL_Status.DataSource = GetCopyStatusList()
        DDL_Status.DataBind()
        DDL_Status.Items.Insert(0, "")
    End Sub
    Public Sub PopulateBindings()
        DDL_Binding.DataTextField = "BIND_NAME"
        DDL_Binding.DataValueField = "BIND_CODE"
        DDL_Binding.DataSource = GetBindingList()
        DDL_Binding.DataBind()
        DDL_Binding.Items.Insert(0, "")
    End Sub
    Public Sub PopulateAccMaterials()
        DDL_AccMaterials.DataTextField = "ACC_MAT_NAME"
        DDL_AccMaterials.DataValueField = "ACC_MAT_CODE"
        DDL_AccMaterials.DataSource = GetAccMaterialsList()
        DDL_AccMaterials.DataBind()
        DDL_AccMaterials.Items.Insert(0, "")
    End Sub
    'populate libraries
    Public Sub PopulateLibrary()
        Dim Command As SqlCommand = Nothing
        Dim dt As DataTable = Nothing
        Try
            Command = New SqlCommand("SELECT  LIB_CODE, LIB_NAME FROM LIBRARIES WHERE (LIB_CODE = '" & Trim(LibCode) & "' OR MAIN_LIB_CODE = '" & Trim(LibCode) & "') ORDER BY LIB_NAME ", SqlConn)
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
            Dr("LIB_CODE") = ""
            Dr("LIB_NAME") = ""
            dt.Rows.InsertAt(Dr, 0)

            If dt.Rows.Count = 0 Then
                Me.DDL_Library.DataSource = Nothing
            Else
                Me.DDL_Library.DataSource = dt
                Me.DDL_Library.DataTextField = "LIB_NAME"
                Me.DDL_Library.DataValueField = "LIB_CODE"
                Me.DDL_Library.DataBind()
            End If
            dt.Dispose()
            SqlConn.Close()
            DDL_Library.Text = ""
        Catch s As Exception
            Label15.Text = "Error: " & (s.Message())
            Label1.Text = ""
        Finally
            Command.Dispose()
            dt.Dispose()
            SqlConn.Close()
        End Try
    End Sub
    'populate libraries
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
            dt.Rows.InsertAt(Dr, 0)

            If dt.Rows.Count = 0 Then
                Me.DDL_Section.DataSource = Nothing
            Else
                Me.DDL_Section.DataSource = dt
                Me.DDL_Section.DataTextField = "SEC_NAME"
                Me.DDL_Section.DataValueField = "SEC_CODE"
                Me.DDL_Section.DataBind()
            End If
            dt.Dispose()
            SqlConn.Close()
            DDL_Section.Text = ""
        Catch s As Exception
            Label15.Text = "Error: " & (s.Message())
            Label1.Text = ""
        Finally
            Command.Dispose()
            dt.Dispose()
            SqlConn.Close()
        End Try

    End Sub
    'get value of row from grid
    Private Sub Grid1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles Grid1.RowCommand
        Dim Command As SqlCommand = Nothing
        Dim dr As SqlDataReader = Nothing
        Try
            If e.CommandName = "Select" Then
                Label15.Text = ""
                Label1.Text = "Type Data and Press SAVE/UPDATE Button!"

                Dim myRowID, AcqID As Integer
                myRowID = e.CommandArgument.ToString()
                AcqID = Grid1.DataKeys(myRowID).Value
                If Not String.IsNullOrEmpty(AcqID) And AcqID <> 0 Then
                    Dim iloop As Integer
                    Dim strcurrentchar As Object
                    Dim c As Integer
                    Dim counter1 As Integer

                    AcqID = TrimX(AcqID)
                    AcqID = UCase(AcqID)

                    AcqID = RemoveQuotes(AcqID)
                    If Len(AcqID).ToString > 10 Then
                        Label15.Text = "Length of Input is not Proper!"
                        Label1.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Length of Input is not Proper... ');", True)
                        Exit Sub
                    End If
                    AcqID = " " & AcqID & " "
                    If InStr(1, AcqID, " CREATE ", 1) > 0 Or InStr(1, AcqID, " DELETE ", 1) > 0 Or InStr(1, AcqID, " DROP ", 1) > 0 Or InStr(1, AcqID, " INSERT ", 1) > 1 Or InStr(1, AcqID, " TRACK ", 1) > 1 Or InStr(1, AcqID, " TRACE ", 1) > 1 Then
                        Label15.Text = "Do not use reserve words... !"
                        Label1.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do not use reserve words... ');", True)
                        Exit Sub
                    End If
                    AcqID = TrimX(AcqID)
                    'get record details from database
                    Dim SQL As String = Nothing
                    SQL = " SELECT *  FROM APPROVAL_VIEW2 WHERE (ACQ_ID = '" & Trim(AcqID) & "') "
                    Command = New SqlCommand(SQL, SqlConn)
                    SqlConn.Open()
                    dr = Command.ExecuteReader(CommandBehavior.CloseConnection)
                    dr.Read()
                    Label_HOLDID.Text = ""
                    ClearFields()
                    If dr.HasRows = True Then
                        If dr.Item("CAT_NO").ToString <> "" Then
                            Label_CATNO.Text = dr.Item("CAT_NO").ToString
                        Else
                            Label_CATNO.Text = ""
                        End If
                        If dr.Item("ACQ_ID").ToString <> "" Then
                            Label_ACQID.Text = dr.Item("ACQ_ID").ToString
                        Else
                            Label_ACQID.Text = ""
                        End If
                        If dr.Item("MULTI_VOL").ToString <> "" Then
                            DDL_YN.SelectedValue = dr.Item("MULTI_VOL").ToString
                        Else
                            DDL_YN.Text = ""
                        End If

                        If dr.Item("MULTI_VOL").ToString = "Y" Then
                            TR_VOL_NO.Visible = True
                            TR_VOL_YEAR.Visible = True
                            TR_VOL_TITLE.Visible = True
                            TR_VOL_EDITORS.Visible = True
                            TR_COPY_ISBN.Visible = True
                        Else
                            TR_VOL_NO.Visible = False
                            TR_VOL_YEAR.Visible = False
                            TR_VOL_TITLE.Visible = False
                            TR_VOL_EDITORS.Visible = False
                            TR_COPY_ISBN.Visible = False
                        End If
                        If dr.Item("MAT_CODE").ToString <> "" Then
                            Label_MATTYPE.Text = dr.Item("MAT_CODE").ToString
                        Else
                            Label_MATTYPE.Text = ""
                        End If
                        If dr.Item("DOC_TYPE_CODE").ToString <> "" Then
                            Label_DOCTYPE.Text = dr.Item("DOC_TYPE_CODE").ToString
                        Else
                            Label_DOCTYPE.Text = ""
                        End If

                        If dr.Item("TITLE").ToString <> "" Then
                            Label_TITLE.Text = dr.Item("TITLE").ToString
                        Else
                            Label_TITLE.Text = ""
                        End If

                        'load DE Format
                        Me.Hold_Save_Bttn.Visible = True
                        Me.Hold_Update_Bttn.Visible = False

                        dr.Close()
                        PopulateHoldingsGrid(Label_CATNO.Text, AcqID)
                        LoadHoldFormat()
                    Else
                        Label_CATNO.Text = ""
                        Label_ACQID.Text = ""
                        Label_MATTYPE.Text = ""
                        Label_HOLDID.Text = ""
                        Label_TITLE.Text = ""
                        Label15.Text = "Record Not Selected"
                        Label1.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' No Record Selected!');", True)
                    End If
                Else
                    Label_CATNO.Text = ""
                    Label_ACQID.Text = ""
                    Label_MATTYPE.Text = ""
                    Label_HOLDID.Text = ""
                    Label_TITLE.Text = ""
                    Label15.Text = "Record Not Selected"
                    Label1.Text = ""
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' No Record Selected!');", True)
                End If
            End If
        Catch s As Exception
            Label15.Text = "Error: " & (s.Message())
            Label1.Text = ""
            Label_CATNO.Text = ""
            Label_ACQID.Text = ""
            Label_MATTYPE.Text = ""
            Label_HOLDID.Text = ""
            Label_TITLE.Text = ""
        Finally
            SqlConn.Close()
            txt_Hold_AccDate.Text = Format(Date.Now, "dd/MM/yyyy")
            Me.txt_Hold_AccNo.Focus()
        End Try
    End Sub 'Grid1_ItemCommand
    Public Sub LoadHoldFormat()
        Dim Command As SqlCommand = Nothing
        Dim dt As DataTable = Nothing
        Try
            Dim myHoldFields As Object = Nothing

            Dim myDocName As Object = Nothing
            If Label_DOCTYPE.Text <> "" Then
                myDocName = Label_DOCTYPE.Text
            Else
                Exit Sub
            End If

            Dim SQL As String = Nothing
            SQL = "SELECT DEF_ID, HOLD_FORMAT, DOC_TYPE_NAME  FROM DEFORMATS WHERE (LIB_CODE = '" & Trim(LibCode) & "'  AND DOC_TYPE_CODE = '" & Trim(myDocName) & "') "
            Command = New SqlCommand(SQL, SqlConn)
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            Dim da As New SqlDataAdapter(Command)
            Dim ds As New DataSet
            Dim RecordCount As Long = 0
            da.Fill(ds)
            dt = ds.Tables(0).Copy

            If dt.Rows.Count <> 0 Then
                If dt.Rows(0).Item("HOLD_FORMAT").ToString <> "" Then
                    myHoldFields = dt.Rows(0).Item("HOLD_FORMAT")
                    If myHoldFields <> "" Then
                        If dt.Rows(0).Item("DOC_TYPE_NAME").ToString <> "" Then
                            Label48.Text = "(" & dt.Rows(0).Item("DOC_TYPE_NAME").ToString & ")"
                        Else
                            Label48.Text = ""
                        End If
                        If InStr(myHoldFields, "CLASS_NO") <> 0 Then
                            TR_CLASS_NO.Visible = True
                        Else
                            TR_CLASS_NO.Visible = False
                        End If
                        If InStr(myHoldFields, "BOOK_NO") <> 0 Then
                            TR_BOOK_NO.Visible = True
                        Else
                            TR_BOOK_NO.Visible = False
                        End If
                        If InStr(myHoldFields, "PAGINATION") <> 0 Then
                            TR_PAGINATION.Visible = True
                        Else
                            TR_PAGINATION.Visible = False
                        End If

                        If InStr(myHoldFields, "ILLUSTRATION") <> 0 Then
                            TR_ILLUSTRATION.Visible = True
                        Else
                            TR_ILLUSTRATION.Visible = False
                        End If

                        If InStr(myHoldFields, "SIZE") <> 0 Then
                            TR_SIZE.Visible = True
                        Else
                            TR_SIZE.Visible = False
                        End If

                        If InStr(myHoldFields, "COLLECTION_TYPE") <> 0 Then
                            TR_COLLECTION_TYPE.Visible = True
                        Else
                            TR_COLLECTION_TYPE.Visible = False
                        End If
                        If InStr(myHoldFields, "PHYSICAL_LOCATION") <> 0 Then
                            TR_PHYSICAL_LOCATION.Visible = True
                        Else
                            TR_PHYSICAL_LOCATION.Visible = False
                        End If
                        If InStr(myHoldFields, "STA_CODE") <> 0 Then
                            TR_STA_CODE.Visible = True
                        Else
                            TR_STA_CODE.Visible = False
                        End If
                        If InStr(myHoldFields, "BIND_CODE") <> 0 Then
                            TR_BIND_CODE.Visible = True
                        Else
                            TR_BIND_CODE.Visible = False
                        End If
                        If InStr(myHoldFields, "ACC_MAT_CODE") <> 0 Then
                            TR_ACC_MAT_CODE.Visible = True
                        Else
                            TR_ACC_MAT_CODE.Visible = False
                        End If
                        If InStr(myHoldFields, "SEC_CODE") <> 0 Then
                            TR_SEC_CODE.Visible = True
                        Else
                            TR_SEC_CODE.Visible = False
                        End If
                        If InStr(myHoldFields, "FORMAT_CODE") <> 0 Then
                            TR_FORMAT_CODE.Visible = True
                        Else
                            TR_FORMAT_CODE.Visible = False
                        End If
                        If InStr(myHoldFields, "COPY_ISBN") <> 0 Then
                            TR_COPY_ISBN.Visible = True
                        Else
                            TR_COPY_ISBN.Visible = False
                        End If
                        If InStr(myHoldFields, "REFERENCE_NO") <> 0 Then
                            TR_REFERENCE_NO.Visible = True
                        Else
                            TR_REFERENCE_NO.Visible = False
                        End If
                        If InStr(myHoldFields, "REMARKS") <> 0 Then
                            TR_REMARKS.Visible = True
                        Else
                            TR_REMARKS.Visible = False
                        End If
                        If InStr(myHoldFields, "MEDIUM") <> 0 Then
                            TR_MEDIUM.Visible = True
                        Else
                            TR_MEDIUM.Visible = False
                        End If
                        If InStr(myHoldFields, "RECORDING_CATEGORY") <> 0 Then
                            TR_RECORDING_CATEGORY.Visible = True
                        Else
                            TR_RECORDING_CATEGORY.Visible = False
                        End If
                        If InStr(myHoldFields, "RECORDING_FORM") <> 0 Then
                            TR_RECORDING_FORM.Visible = True
                        Else
                            TR_RECORDING_FORM.Visible = False
                        End If
                        If InStr(myHoldFields, "RECORDING_FORMAT") <> 0 Then
                            TR_RECORDING_FORMAT.Visible = True
                        Else
                            TR_RECORDING_FORMAT.Visible = False
                        End If
                        If InStr(myHoldFields, "RECORDING_SPEED") <> 0 Then
                            TR_RECORDING_SPEED.Visible = True
                        Else
                            TR_RECORDING_SPEED.Visible = False
                        End If
                        If InStr(myHoldFields, "RECORDING_STORAGE_TECH") <> 0 Then
                            TR_RECORDING_STORAGE_TECH.Visible = True
                        Else
                            TR_RECORDING_STORAGE_TECH.Visible = False
                        End If
                        If InStr(myHoldFields, "RECORDING_PLAY_DURATION") <> 0 Then
                            TR_RECORDING_PLAY_DURATION.Visible = True
                        Else
                            TR_RECORDING_PLAY_DURATION.Visible = False
                        End If
                        If InStr(myHoldFields, "VIDEO_TYPEOFVISUAL") <> 0 Then
                            TR_VIDEO_TYPEOFVISUAL.Visible = True
                        Else
                            TR_VIDEO_TYPEOFVISUAL.Visible = False
                        End If
                        If InStr(myHoldFields, "VIDEO_COLOR") <> 0 Then
                            TR_VIDEO_COLOR.Visible = True
                        Else
                            TR_VIDEO_COLOR.Visible = False
                        End If
                        If InStr(myHoldFields, "PLAYBACK_CHANNELS") <> 0 Then
                            TR_PLAYBACK_CHANNELS.Visible = True
                        Else
                            TR_PLAYBACK_CHANNELS.Visible = False
                        End If
                        If InStr(myHoldFields, "TAPE_WIDTH") <> 0 Then
                            TR_TAPE_WIDTH.Visible = True
                        Else
                            TR_TAPE_WIDTH.Visible = False
                        End If
                        If InStr(myHoldFields, "TAPE_CONFIGURATION") <> 0 Then
                            TR_TAPE_CONFIGURATION.Visible = True
                        Else
                            TR_TAPE_CONFIGURATION.Visible = False
                        End If
                        If InStr(myHoldFields, "KIND_OF_DISK") <> 0 Then
                            TR_KIND_OF_DISK.Visible = True
                        Else
                            TR_KIND_OF_DISK.Visible = False
                        End If
                        If InStr(myHoldFields, "KIND_OF_CUTTING") <> 0 Then
                            TR_KIND_OF_CUTTING.Visible = True
                        Else
                            TR_KIND_OF_CUTTING.Visible = False
                        End If
                        If InStr(myHoldFields, "ENCODING_STANDARD") <> 0 Then
                            TR_ENCODING_STANDARD.Visible = True
                        Else
                            TR_ENCODING_STANDARD.Visible = False
                        End If
                        If InStr(myHoldFields, "CAPTURE_TECHNIQUE") <> 0 Then
                            TR_CAPTURE_TECHNIQUE.Visible = True
                        Else
                            TR_CAPTURE_TECHNIQUE.Visible = False
                        End If
                        If InStr(myHoldFields, "CARTOGRAPHIC_MEDIUM") <> 0 Then
                            TR_CARTOGRAPHIC_MEDIUM.Visible = True
                        Else
                            TR_CARTOGRAPHIC_MEDIUM.Visible = False
                        End If
                        If InStr(myHoldFields, "CARTOGRAPHIC_COORDINATES") <> 0 Then
                            TR_CARTOGRAPHIC_COORDINATES.Visible = True
                        Else
                            TR_CARTOGRAPHIC_COORDINATES.Visible = False
                        End If
                        If InStr(myHoldFields, "CARTOGRAPHIC_GEOGRAPHIC_LOCATION") <> 0 Then
                            TR_CARTOGRAPHIC_GEOGRAPHIC_LOCATION.Visible = True
                        Else
                            TR_CARTOGRAPHIC_GEOGRAPHIC_LOCATION.Visible = False
                        End If
                        If InStr(myHoldFields, "CARTOGRAPHIC_DATAGATHERING_DATE") <> 0 Then
                            TR_CARTOGRAPHIC_DATAGATHERING_DATE.Visible = True
                        Else
                            TR_CARTOGRAPHIC_DATAGATHERING_DATE.Visible = False
                        End If
                        If InStr(myHoldFields, "CARTOGRAPHIC_COMPILATION_DATE") <> 0 Then
                            TR_CARTOGRAPHIC_COMPILATION_DATE.Visible = True
                        Else
                            TR_CARTOGRAPHIC_COMPILATION_DATE.Visible = False
                        End If
                        If InStr(myHoldFields, "CARTOGRAPHIC_INSPECTION_DATE") <> 0 Then
                            TR_CARTOGRAPHIC_INSPECTION_DATE.Visible = True
                        Else
                            TR_CARTOGRAPHIC_INSPECTION_DATE.Visible = False
                        End If
                        If InStr(myHoldFields, "CARTOGRAPHIC_SCALE") <> 0 Then
                            TR_CARTOGRAPHIC_SCALE.Visible = True
                        Else
                            TR_CARTOGRAPHIC_SCALE.Visible = False
                        End If
                        If InStr(myHoldFields, "CARTOGRAPHIC_PROJECTION") <> 0 Then
                            TR_CARTOGRAPHIC_PROJECTION.Visible = True
                        Else
                            TR_CARTOGRAPHIC_PROJECTION.Visible = False
                        End If
                        If InStr(myHoldFields, "CREATION_DATE") <> 0 Then
                            TR_CREATION_DATE.Visible = True
                        Else
                            TR_CREATION_DATE.Visible = False
                        End If
                        If InStr(myHoldFields, "PHOTO_NO") <> 0 Then
                            TR_PHOTO_NO.Visible = True
                        Else
                            TR_PHOTO_NO.Visible = False
                        End If
                        If InStr(myHoldFields, "PHOTO_ALBUM_NO") <> 0 Then
                            TR_PHOTO_ALBUM_NO.Visible = True
                        Else
                            TR_PHOTO_ALBUM_NO.Visible = False
                        End If
                        If InStr(myHoldFields, "PHOTO_OCASION") <> 0 Then
                            TR_PHOTO_OCASION.Visible = True
                        Else
                            TR_PHOTO_OCASION.Visible = False
                        End If

                        If InStr(myHoldFields, "IMAGE_VIEW_TYPE") <> 0 Then
                            TR_IMAGE_VIEW_TYPE.Visible = True
                        Else
                            TR_IMAGE_VIEW_TYPE.Visible = False
                        End If

                        If InStr(myHoldFields, "VIEW_DATE") <> 0 Then
                            TR_VIEW_DATE.Visible = True
                        Else
                            TR_VIEW_DATE.Visible = False
                        End If

                        If InStr(myHoldFields, "THEME") <> 0 Then
                            TR_THEME.Visible = True
                        Else
                            TR_THEME.Visible = False
                        End If

                        If InStr(myHoldFields, "STYLE") <> 0 Then
                            TR_STYLE.Visible = True
                        Else
                            TR_STYLE.Visible = False
                        End If

                        If InStr(myHoldFields, "CULTURE") <> 0 Then
                            TR_CULTURE.Visible = True
                        Else
                            TR_CULTURE.Visible = False
                        End If

                        If InStr(myHoldFields, "CURRENT_STIE") <> 0 Then
                            TR_CURRENT_STIE.Visible = True
                        Else
                            TR_CURRENT_STIE.Visible = False
                        End If

                        If InStr(myHoldFields, "CREATION_SITE") <> 0 Then
                            TR_CREATION_SITE.Visible = True
                        Else
                            TR_CREATION_SITE.Visible = False
                        End If

                        If InStr(myHoldFields, "YARN_COUNT") <> 0 Then
                            TR_YARNCOUNT.Visible = True
                        Else
                            TR_YARNCOUNT.Visible = False
                        End If

                        If InStr(myHoldFields, "MATERIAL_TYPE") <> 0 Then
                            TR_MATERIAL_TYPE.Visible = True
                        Else
                            TR_MATERIAL_TYPE.Visible = False
                        End If

                        If InStr(myHoldFields, "TECHNIQUE") <> 0 Then
                            TR_TECHNIQUE.Visible = True
                        Else
                            TR_TECHNIQUE.Visible = False
                        End If

                        If InStr(myHoldFields, "TECH_DETAILS") <> 0 Then
                            TR_TECH_DETAILS.Visible = True
                        Else
                            TR_TECH_DETAILS.Visible = False
                        End If

                        If InStr(myHoldFields, "INSCRIPTIONS") <> 0 Then
                            TR_INSCRIPTIONS.Visible = True
                        Else
                            TR_INSCRIPTIONS.Visible = False
                        End If

                        If InStr(myHoldFields, "DESCRIPTION") <> 0 Then
                            TR_DESCRIPTION.Visible = True
                        Else
                            TR_DESCRIPTION.Visible = False
                        End If

                        If InStr(myHoldFields, "GLOBE_TYPE") <> 0 Then
                            TR_GLOBE_TYPE.Visible = True
                        Else
                            TR_GLOBE_TYPE.Visible = False
                        End If

                        If InStr(myHoldFields, "ALTER_DATE") <> 0 Then
                            TR_ALTER_DATE.Visible = True
                        Else
                            TR_ALTER_DATE.Visible = False
                        End If
                    Else
                        Label15.Text = "Data Entry Format is not proper..Create Data Entry Format for this Document Type in Library Administrator Module"
                        Label1.Text = ""
                    End If
                Else
                    Label15.Text = "Data Entry Format has not been added..Create Data Entry Format for this Document Type in Library Administrator Module"
                    Label1.Text = ""
                End If
            Else
                Label15.Text = "Data Entry Format has not been added..Create Data Entry Format for this Document Type in Library Administrator Module"
                Label1.Text = ""
                TR_VOL_NO.Visible = True
                TR_VOL_EDITORS.Visible = True
                TR_VOL_YEAR.Visible = True
                TR_CLASS_NO.Visible = True
                TR_BOOK_NO.Visible = True
                TR_PAGINATION.Visible = True
                TR_ILLUSTRATION.Visible = True
                TR_SIZE.Visible = True
                TR_COLLECTION_TYPE.Visible = True
                TR_PHYSICAL_LOCATION.Visible = True
                TR_STA_CODE.Visible = True
                TR_BIND_CODE.Visible = True
                TR_ACC_MAT_CODE.Visible = True
                TR_SEC_CODE.Visible = True
                TR_FORMAT_CODE.Visible = True
                TR_COPY_ISBN.Visible = True
                TR_REFERENCE_NO.Visible = True
                TR_REMARKS.Visible = True
                TR_MEDIUM.Visible = True
                TR_RECORDING_CATEGORY.Visible = True
                TR_RECORDING_FORM.Visible = True
                TR_RECORDING_FORMAT.Visible = True
                TR_RECORDING_SPEED.Visible = True
                TR_RECORDING_STORAGE_TECH.Visible = True
                TR_RECORDING_PLAY_DURATION.Visible = True
                TR_VIDEO_TYPEOFVISUAL.Visible = True
                TR_VIDEO_COLOR.Visible = True
                TR_TAPE_WIDTH.Visible = True
                TR_TAPE_CONFIGURATION.Visible = True
                TR_KIND_OF_DISK.Visible = True
                TR_KIND_OF_CUTTING.Visible = True
                TR_ENCODING_STANDARD.Visible = True
                TR_CAPTURE_TECHNIQUE.Visible = True
                TR_CARTOGRAPHIC_MEDIUM.Visible = True
                TR_CARTOGRAPHIC_COORDINATES.Visible = True
                TR_CARTOGRAPHIC_GEOGRAPHIC_LOCATION.Visible = True
                TR_CARTOGRAPHIC_DATAGATHERING_DATE.Visible = True
                TR_CARTOGRAPHIC_COMPILATION_DATE.Visible = True
                TR_CARTOGRAPHIC_INSPECTION_DATE.Visible = True
                TR_CARTOGRAPHIC_SCALE.Visible = True
                TR_CARTOGRAPHIC_PROJECTION.Visible = True
                TR_CREATION_DATE.Visible = True
                TR_PHOTO_NO.Visible = True
                TR_PHOTO_ALBUM_NO.Visible = True
                TR_PHOTO_OCASION.Visible = True
                TR_IMAGE_VIEW_TYPE.Visible = True
                TR_VIEW_DATE.Visible = True
                TR_THEME.Visible = True
                TR_STYLE.Visible = True
                TR_CULTURE.Visible = True
                TR_CURRENT_STIE.Visible = True
                TR_CREATION_SITE.Visible = True
                TR_YARNCOUNT.Visible = True
                TR_MATERIAL_TYPE.Visible = True
                TR_TECHNIQUE.Visible = True
                TR_TECH_DETAILS.Visible = True
                TR_INSCRIPTIONS.Visible = True
                TR_DESCRIPTION.Visible = True
                TR_GLOBE_TYPE.Visible = True
                TR_ALTER_DATE.Visible = True
            End If
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
            Label15.Text = ""
        Finally
            Command.Dispose()
            dt.Dispose()
            SqlConn.Close()
        End Try
    End Sub
    'save holdings in table and update ACQ Table also    
    Protected Sub Hold_Save_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Hold_Save_Bttn.Click
        If IsPostBack = True Then
            Dim CopiesRecd As Long = 0
            'check unwanted characters
            Dim iloop As Integer
            Dim strcurrentchar As Object
            Dim c As Integer
            Dim counter1, counter2, counter3, counter4, counter5, counter6, counter7, counter8, counter9, counter10 As Integer
            Dim counter11, counter12, counter13, counter14, counter15, counter16, counter17, counter18, counter19, counter20 As Integer
            Dim counter21, counter22, counter23, counter24, counter25, counter26, counter27, counter28, counter29, counter30 As Integer
            Dim thisTransaction As SqlClient.SqlTransaction = Nothing
            Try

                If DDL_Orders.Text = "" Then
                    Label15.Text = "Plz Select Order from Drop-Down!"
                    Label1.Text = ""
                    DDL_Orders.Focus()
                    Exit Sub
                End If

                'validation for acq_id
                Dim ACQ_ID As Long = Nothing
                If Me.Label_ACQID.Text <> "" Then
                    ACQ_ID = Convert.ToInt16(TrimX(Label_ACQID.Text))
                    ACQ_ID = RemoveQuotes(ACQ_ID)
                    If Not IsNumeric(ACQ_ID) = True Then
                        Label15.Text = "Error:Select Record from Above Grid by Clicking the CLICK button !"
                        Label1.Text = ""
                        Exit Sub
                    End If
                    If Len(ACQ_ID) > 10 Then
                        Label15.Text = "Error: Input is not Valid !"
                        Label1.Text = ""
                        Exit Sub
                    End If
                    ACQ_ID = " " & ACQ_ID & " "
                    If InStr(1, ACQ_ID, "CREATE", 1) > 0 Or InStr(1, ACQ_ID, "DELETE", 1) > 0 Or InStr(1, ACQ_ID, "DROP", 1) > 0 Or InStr(1, ACQ_ID, "INSERT", 1) > 1 Or InStr(1, ACQ_ID, "TRACK", 1) > 1 Or InStr(1, ACQ_ID, "TRACE", 1) > 1 Then
                        Label15.Text = "Error: Input is not Valid !"
                        Label1.Text = ""
                        Exit Sub
                    End If
                    ACQ_ID = TrimX(ACQ_ID)
                    'check unwanted characters
                    c = 0
                    counter2 = 0
                    For iloop = 1 To Len(ACQ_ID.ToString)
                        strcurrentchar = Mid(ACQ_ID, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter2 = 1
                            End If
                        End If
                    Next
                    If counter2 = 1 Then
                        Label15.Text = "Select Record from Above Grid by Clicking the CLICK button !"
                        Label1.Text = ""
                        Exit Sub
                    End If

                    'check process status
                    Dim str As Object = Nothing
                    Dim flag As Object = Nothing
                    str = "SELECT ACQ_ID FROM ACQUISITIONS WHERE (ACQ_ID = '" & Trim(ACQ_ID) & "')  AND (PROCESS_STATUS = 'Accessioned')"
                    Dim cmd1 As New SqlCommand(str, SqlConn)
                    SqlConn.Open()
                    flag = cmd1.ExecuteScalar
                    If flag <> Nothing Then
                        Label15.Text = "All Received Copy(ies) has/have been Accessioned, Can Not Add More Copy ! "
                        Label1.Text = ""
                        Me.txt_Hold_AccNo.Focus()
                        Exit Sub
                    End If
                    SqlConn.Close()

                    'check copy Recd in this acqid
                    Dim str2 As Object = Nothing
                    str2 = "SELECT COPY_RECEIVED FROM ACQUISITIONS WHERE (ACQ_ID = '" & Trim(ACQ_ID) & "')"
                    Dim cmd2 As New SqlCommand(str2, SqlConn)
                    SqlConn.Open()
                    CopiesRecd = cmd2.ExecuteScalar
                    SqlConn.Close()
                Else
                    Label15.Text = "Error: Select Record from Above Grid by Clicking the CLICK button!"
                    Label1.Text = ""
                    Exit Sub
                End If

                'validation for cat_no
                Dim CAT_NO As Long = Nothing
                If Me.Label_CATNO.Text <> "" Then
                    CAT_NO = Convert.ToInt16(TrimX(Label_CATNO.Text))
                    CAT_NO = RemoveQuotes(CAT_NO)
                    If Not IsNumeric(CAT_NO) = True Then
                        Label15.Text = "Error:Select Record from Above Grid by Clicking the CLICK button !"
                        Label1.Text = ""
                        Exit Sub
                    End If
                    If Len(CAT_NO) > 10 Then
                        Label15.Text = "Error: Input is not Valid !"
                        Label1.Text = ""
                        Exit Sub
                    End If
                    CAT_NO = " " & CAT_NO & " "
                    If InStr(1, CAT_NO, "CREATE", 1) > 0 Or InStr(1, CAT_NO, "DELETE", 1) > 0 Or InStr(1, CAT_NO, "DROP", 1) > 0 Or InStr(1, CAT_NO, "INSERT", 1) > 1 Or InStr(1, CAT_NO, "TRACK", 1) > 1 Or InStr(1, CAT_NO, "TRACE", 1) > 1 Then
                        Label15.Text = "Error: Input is not Valid !"
                        Label1.Text = ""
                        Exit Sub
                    End If
                    CAT_NO = TrimX(CAT_NO)
                    'check unwanted characters
                    c = 0
                    counter1 = 0
                    For iloop = 1 To Len(CAT_NO.ToString)
                        strcurrentchar = Mid(CAT_NO, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter1 = 1
                            End If
                        End If
                    Next
                    If counter1 = 1 Then
                        Label15.Text = "Error: Input is not Valid !"
                        Label1.Text = ""
                        Exit Sub
                    End If
                Else
                    Label15.Text = "Error: Select Record from Above Grid by Clicking the CLICK button !"
                    Label1.Text = ""
                    Exit Sub
                End If

                'txt_Hold_AccSereis
                Dim ACCESSION_SERIES As Object = Nothing
                If txt_Hold_AccSereis.Text <> "" Then
                    ACCESSION_SERIES = TrimX(UCase(txt_Hold_AccSereis.Text))
                    ACCESSION_SERIES = RemoveQuotes(ACCESSION_SERIES)
                    If ACCESSION_SERIES.Length > 10 Then 'maximum length
                        Label15.Text = "Error: Input is not Valid !"
                        Label1.Text = ""
                        txt_Hold_AccSereis.Focus()
                        Exit Sub
                    End If

                    ACCESSION_SERIES = " " & ACCESSION_SERIES & " "
                    If InStr(1, ACCESSION_SERIES, "CREATE", 1) > 0 Or InStr(1, ACCESSION_SERIES, "DELETE", 1) > 0 Or InStr(1, ACCESSION_SERIES, "DROP", 1) > 0 Or InStr(1, ACCESSION_SERIES, "INSERT", 1) > 1 Or InStr(1, ACCESSION_SERIES, "TRACK", 1) > 1 Or InStr(1, ACCESSION_SERIES, "TRACE", 1) > 1 Then
                        Label15.Text = "Error: Input is not Valid !"
                        Label1.Text = ""
                        Me.txt_Hold_AccSereis.Focus()
                        Exit Sub
                    End If
                    ACCESSION_SERIES = TrimX(ACCESSION_SERIES)
                    'check unwanted characters
                    c = 0
                    counter3 = 0
                    For iloop = 1 To Len(ACCESSION_SERIES.ToString)
                        strcurrentchar = Mid(ACCESSION_SERIES, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^*+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter3 = 1
                            End If
                        End If
                    Next
                    If counter3 = 1 Then
                        Label15.Text = "Error: Input is not Valid !"
                        Label1.Text = ""
                        Me.txt_Hold_AccSereis.Focus()
                        Exit Sub
                    End If
                Else
                    ACCESSION_SERIES = ""
                End If


                'Server Validation for accession no
                Dim ACCESSION_NO As Object = Nothing
                If txt_Hold_AccNo.Text <> "" Then
                    ACCESSION_NO = TrimX(UCase(txt_Hold_AccNo.Text))
                    ACCESSION_NO = RemoveQuotes(ACCESSION_NO)
                    If ACCESSION_NO.Length > 30 Then 'maximum length
                        Label15.Text = "Error: Input is not Valid !"
                        Label1.Text = ""
                        txt_Hold_AccNo.Focus()
                        Exit Sub
                    End If

                    ACCESSION_NO = " " & ACCESSION_NO & " "
                    If InStr(1, ACCESSION_NO, "CREATE", 1) > 0 Or InStr(1, ACCESSION_NO, "DELETE", 1) > 0 Or InStr(1, ACCESSION_NO, "DROP", 1) > 0 Or InStr(1, ACCESSION_NO, "INSERT", 1) > 1 Or InStr(1, ACCESSION_NO, "TRACK", 1) > 1 Or InStr(1, ACCESSION_NO, "TRACE", 1) > 1 Then
                        Label15.Text = "Error: Input is not Valid !"
                        Label1.Text = ""
                        Me.txt_Hold_AccNo.Focus()
                        Exit Sub
                    End If
                    ACCESSION_NO = TrimX(ACCESSION_NO)
                    'check unwanted characters
                    c = 0
                    counter3 = 0
                    For iloop = 1 To Len(ACCESSION_NO.ToString)
                        strcurrentchar = Mid(ACCESSION_NO, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^*+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter3 = 1
                            End If
                        End If
                    Next
                    If counter3 = 1 Then
                        Label15.Text = "Error: Input is not Valid !"
                        Label1.Text = ""
                        Me.txt_Hold_AccNo.Focus()
                        Exit Sub
                    End If

                    'if range of accn no  then chk if it is numeric
                    If CB_RecvAll.Checked = True Then
                        If IsNumeric(ACCESSION_NO) = False Then
                            Label15.Text = "In Case you wish to Accession in bulk Then Plz Enter Accession Series(if any) in Sereis Text Box and Enter ACCESSION NO in Digits Only !"
                            Label1.Text = ""
                            Me.txt_Hold_AccNo.Focus()
                            Exit Sub
                        End If
                    End If

                    'add sereis with acc no
                    If ACCESSION_SERIES <> "" Then
                        ACCESSION_NO = ACCESSION_SERIES + ACCESSION_NO
                    End If
                    If Me.CB_RecvAll.Checked = False Then
                        'check duplicate isbn
                        Dim str As Object = Nothing
                        Dim flag As Object = Nothing
                        str = "SELECT HOLD_ID FROM HOLDINGS WHERE (ACCESSION_NO = '" & Trim(ACCESSION_NO) & "')  AND (LIB_CODE = '" & Trim(DDL_Library.SelectedValue) & "')"
                        Dim cmd1 As New SqlCommand(str, SqlConn)
                        SqlConn.Open()
                        flag = cmd1.ExecuteScalar
                        If flag <> Nothing Then
                            Label15.Text = "This Accession No Already Exists ! "
                            Label1.Text = ""
                            Me.txt_Hold_AccNo.Focus()
                            Exit Sub
                        End If
                        SqlConn.Close()
                    End If
                Else
                    Label15.Text = "Error: Input is Required !"
                    Label1.Text = ""
                    Me.txt_Hold_AccNo.Focus()
                    Exit Sub
                End If

                'search accession date
                Dim ACCESSION_DATE As Object = Nothing
                If txt_Hold_AccDate.Text <> "" Then
                    ACCESSION_DATE = TrimX(txt_Hold_AccDate.Text)
                    ACCESSION_DATE = RemoveQuotes(ACCESSION_DATE)
                    ACCESSION_DATE = Convert.ToDateTime(ACCESSION_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                    If Len(ACCESSION_DATE) > 12 Then
                        Label15.Text = " Input is not Valid..."
                        Label1.Text = ""
                        Me.txt_Hold_AccDate.Focus()
                        Exit Sub
                    End If
                    ACCESSION_DATE = " " & ACCESSION_DATE & " "
                    If InStr(1, ACCESSION_DATE, "CREATE", 1) > 0 Or InStr(1, ACCESSION_DATE, "DELETE", 1) > 0 Or InStr(1, ACCESSION_DATE, "DROP", 1) > 0 Or InStr(1, ACCESSION_DATE, "INSERT", 1) > 1 Or InStr(1, ACCESSION_DATE, "TRACK", 1) > 1 Or InStr(1, ACCESSION_DATE, "TRACE", 1) > 1 Then
                        Label15.Text = "  Input is not Valid... "
                        Label1.Text = ""
                        Me.txt_Hold_AccDate.Focus()
                        Exit Sub
                    End If
                    ACCESSION_DATE = TrimX(ACCESSION_DATE)
                    'check unwanted characters
                    c = 0
                    counter4 = 0
                    For iloop = 1 To Len(ACCESSION_DATE)
                        strcurrentchar = Mid(ACCESSION_DATE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter4 = 1
                            End If
                        End If
                    Next
                    If counter4 = 1 Then
                        Label15.Text = "data is not Valid... "
                        Label1.Text = ""
                        Me.txt_Hold_AccDate.Focus()
                        Exit Sub
                    End If
                Else
                    ACCESSION_DATE = Now.Date
                    ACCESSION_DATE = Convert.ToDateTime(ACCESSION_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us
                End If

                'validation for Format
                Dim FORMAT_CODE As Object = Nothing
                If TR_FORMAT_CODE.Visible = True Then
                    FORMAT_CODE = DDL_Format.SelectedValue
                    If Not String.IsNullOrEmpty(FORMAT_CODE) Then
                        FORMAT_CODE = RemoveQuotes(FORMAT_CODE)
                        If FORMAT_CODE.Length > 3 Then 'maximum length
                            Label15.Text = " Data must be of Proper Length.. "
                            Label1.Text = ""
                            DDL_Format.Focus()
                            Exit Sub
                        End If

                        FORMAT_CODE = " " & FORMAT_CODE & " "
                        If InStr(1, FORMAT_CODE, "CREATE", 1) > 0 Or InStr(1, FORMAT_CODE, "DELETE", 1) > 0 Or InStr(1, FORMAT_CODE, "DROP", 1) > 0 Or InStr(1, FORMAT_CODE, "INSERT", 1) > 1 Or InStr(1, FORMAT_CODE, "TRACK", 1) > 1 Or InStr(1, FORMAT_CODE, "TRACE", 1) > 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                            DDL_Format.Focus()
                            Exit Sub
                        End If
                        FORMAT_CODE = TrimX(FORMAT_CODE)
                        'check unwanted characters
                        c = 0
                        counter5 = 0
                        For iloop = 1 To Len(FORMAT_CODE)
                            strcurrentchar = Mid(FORMAT_CODE, iloop, 1)
                            If c = 0 Then
                                If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter5 = 1
                                End If
                            End If
                        Next
                        If counter5 = 1 Then
                            Label15.Text = " Input  is not Valid!"
                            Label1.Text = ""
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                            DDL_Format.Focus()
                            Exit Sub
                        End If
                    Else
                        FORMAT_CODE = "PT"
                    End If
                Else
                    FORMAT_CODE = "PT"
                End If

                'validation for SHOW
                Dim SHOW As Object = Nothing
                SHOW = DDL_Show.SelectedValue
                If Not String.IsNullOrEmpty(SHOW) Then
                    SHOW = RemoveQuotes(SHOW)
                    If SHOW.Length > 2 Then 'maximum length
                        Label15.Text = " Data must be of Proper Length.. "
                        Label1.Text = ""
                        DDL_Show.Focus()
                        Exit Sub
                    End If

                    SHOW = " " & SHOW & " "
                    If InStr(1, SHOW, "CREATE", 1) > 0 Or InStr(1, SHOW, "DELETE", 1) > 0 Or InStr(1, SHOW, "DROP", 1) > 0 Or InStr(1, SHOW, "INSERT", 1) > 1 Or InStr(1, SHOW, "TRACK", 1) > 1 Or InStr(1, SHOW, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        DDL_Show.Focus()
                        Exit Sub
                    End If
                    SHOW = TrimX(SHOW)
                    'check unwanted characters
                    c = 0
                    counter6 = 0
                    For iloop = 1 To Len(SHOW)
                        strcurrentchar = Mid(SHOW, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter6 = 1
                            End If
                        End If
                    Next
                    If counter6 = 1 Then
                        Label15.Text = " Input  is not Valid!"
                        Label1.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                        DDL_Show.Focus()
                        Exit Sub
                    End If
                Else
                    SHOW = "Y"
                End If

                'validation for ISSUABLE
                Dim ISSUABLE As Object = Nothing
                ISSUABLE = DDL_Issuable.SelectedValue
                If Not String.IsNullOrEmpty(ISSUABLE) Then
                    ISSUABLE = RemoveQuotes(ISSUABLE)
                    If ISSUABLE.Length > 2 Then 'maximum length
                        Label15.Text = " Data must be of Proper Length.. "
                        Label1.Text = ""
                        DDL_Issuable.Focus()
                        Exit Sub
                    End If

                    ISSUABLE = " " & ISSUABLE & " "
                    If InStr(1, ISSUABLE, "CREATE", 1) > 0 Or InStr(1, ISSUABLE, "DELETE", 1) > 0 Or InStr(1, ISSUABLE, "DROP", 1) > 0 Or InStr(1, ISSUABLE, "INSERT", 1) > 1 Or InStr(1, ISSUABLE, "TRACK", 1) > 1 Or InStr(1, ISSUABLE, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        DDL_Issuable.Focus()
                        Exit Sub
                    End If
                    ISSUABLE = TrimX(ISSUABLE)
                    'check unwanted characters
                    c = 0
                    counter7 = 0
                    For iloop = 1 To Len(ISSUABLE)
                        strcurrentchar = Mid(ISSUABLE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter7 = 1
                            End If
                        End If
                    Next
                    If counter7 = 1 Then
                        Label15.Text = " Input  is not Valid!"
                        Label1.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                        DDL_Issuable.Focus()
                        Exit Sub
                    End If
                Else
                    ISSUABLE = "Y"
                End If

                'validation for VOL_NO
                Dim VOL_NO As Object = Nothing
                If TR_VOL_NO.Visible = True Then
                    VOL_NO = Trim(txt_Hold_VolNo.Text)
                    If Not String.IsNullOrEmpty(VOL_NO) Then
                        VOL_NO = RemoveQuotes(VOL_NO)
                        If VOL_NO.Length > 30 Then 'maximum length
                            Label15.Text = " Data must be of Proper Length.. "
                            Label1.Text = ""
                            txt_Hold_VolNo.Focus()
                            Exit Sub
                        End If

                        VOL_NO = " " & VOL_NO & " "
                        If InStr(1, VOL_NO, "CREATE", 1) > 0 Or InStr(1, VOL_NO, "DELETE", 1) > 0 Or InStr(1, VOL_NO, "DROP", 1) > 0 Or InStr(1, VOL_NO, "INSERT", 1) > 1 Or InStr(1, VOL_NO, "TRACK", 1) > 1 Or InStr(1, VOL_NO, "TRACE", 1) > 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                            txt_Hold_VolNo.Focus()
                            Exit Sub
                        End If
                        VOL_NO = TrimAll(VOL_NO)
                        'check unwanted characters
                        c = 0
                        counter8 = 0
                        For iloop = 1 To Len(VOL_NO)
                            strcurrentchar = Mid(VOL_NO, iloop, 1)
                            If c = 0 Then
                                If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter8 = 1
                                End If
                            End If
                        Next
                        If counter8 = 1 Then
                            Label15.Text = " Input  is not Valid!"
                            Label1.Text = ""
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                            txt_Hold_VolNo.Focus()
                            Exit Sub
                        End If
                    Else
                        VOL_NO = ""
                    End If
                Else
                    VOL_NO = ""
                End If

                'validation for cat_no
                Dim VOL_YEAR As Integer = Nothing
                If TR_VOL_YEAR.Visible = True Then
                    If Me.txt_Hold_VolYear.Text <> "" Then
                        VOL_YEAR = Convert.ToInt16(TrimX(txt_Hold_VolYear.Text))
                        VOL_YEAR = RemoveQuotes(VOL_YEAR)
                        If Not IsNumeric(VOL_YEAR) = True Then
                            Label15.Text = "Error: Input is not Valid !"
                            Label1.Text = ""
                            txt_Hold_VolYear.Focus()
                            Exit Sub
                        End If
                        If Len(VOL_YEAR) > 5 Then
                            Label15.Text = "Error: Input is not Valid !"
                            Label1.Text = ""
                            txt_Hold_VolYear.Focus()
                            Exit Sub
                        End If
                        VOL_YEAR = " " & VOL_YEAR & " "
                        If InStr(1, VOL_YEAR, "CREATE", 1) > 0 Or InStr(1, VOL_YEAR, "DELETE", 1) > 0 Or InStr(1, VOL_YEAR, "DROP", 1) > 0 Or InStr(1, VOL_YEAR, "INSERT", 1) > 1 Or InStr(1, VOL_YEAR, "TRACK", 1) > 1 Or InStr(1, VOL_YEAR, "TRACE", 1) > 1 Then
                            Label15.Text = "Error: Input is not Valid !"
                            Label1.Text = ""
                            txt_Hold_VolYear.Focus()
                            Exit Sub
                        End If
                        VOL_YEAR = TrimX(VOL_YEAR)
                        'check unwanted characters
                        c = 0
                        counter9 = 0
                        For iloop = 1 To Len(VOL_YEAR.ToString)
                            strcurrentchar = Mid(VOL_YEAR, iloop, 1)
                            If c = 0 Then
                                If Not InStr("'<>=%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter9 = 1
                                End If
                            End If
                        Next
                        If counter9 = 1 Then
                            Label15.Text = "Error: Input is not Valid !"
                            Label1.Text = ""
                            txt_Hold_VolYear.Focus()
                            Exit Sub
                        End If
                    Else
                        VOL_YEAR = 0
                    End If
                Else
                    VOL_YEAR = 0
                End If

                'validation for VOL_EDITORS
                Dim VOL_EDITORS As Object = Nothing
                If TR_VOL_EDITORS.Visible = True Then
                    VOL_EDITORS = TrimAll(txt_Hold_VolEditors.Text)
                    If Not String.IsNullOrEmpty(VOL_EDITORS) Then
                        VOL_EDITORS = RemoveQuotes(VOL_EDITORS)
                        If VOL_EDITORS.Length > 501 Then 'maximum length
                            Label15.Text = " Data must be of Proper Length.. "
                            Label1.Text = ""
                            txt_Hold_VolEditors.Focus()
                            Exit Sub
                        End If

                        VOL_EDITORS = " " & VOL_EDITORS & " "
                        If InStr(1, VOL_EDITORS, "CREATE", 1) > 0 Or InStr(1, VOL_EDITORS, "DELETE", 1) > 0 Or InStr(1, VOL_EDITORS, "DROP", 1) > 0 Or InStr(1, VOL_EDITORS, "INSERT", 1) > 1 Or InStr(1, VOL_EDITORS, "TRACK", 1) > 1 Or InStr(1, VOL_EDITORS, "TRACE", 1) > 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                            txt_Hold_VolEditors.Focus()
                            Exit Sub
                        End If
                        VOL_EDITORS = TrimAll(VOL_EDITORS)
                        'check unwanted characters
                        c = 0
                        counter10 = 0
                        For iloop = 1 To Len(VOL_EDITORS)
                            strcurrentchar = Mid(VOL_EDITORS, iloop, 1)
                            If c = 0 Then
                                If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter10 = 1
                                End If
                            End If
                        Next
                        If counter10 = 1 Then
                            Label15.Text = " Input  is not Valid!"
                            Label1.Text = ""
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                            txt_Hold_VolEditors.Focus()
                            Exit Sub
                        End If
                    Else
                        VOL_EDITORS = ""
                    End If
                Else
                    VOL_EDITORS = ""
                End If

                'validation for VOL_TITLE
                Dim VOL_TITLE As Object = Nothing
                If TR_VOL_TITLE.Visible = True Then
                    VOL_TITLE = TrimAll(txt_Hold_VolTitle.Text)
                    If Not String.IsNullOrEmpty(VOL_TITLE) Then
                        VOL_TITLE = RemoveQuotes(VOL_TITLE)
                        If VOL_TITLE.Length > 501 Then 'maximum length
                            Label15.Text = " Data must be of Proper Length.. "
                            Label1.Text = ""
                            txt_Hold_VolTitle.Focus()
                            Exit Sub
                        End If

                        VOL_TITLE = " " & VOL_TITLE & " "
                        If InStr(1, VOL_TITLE, "CREATE", 1) > 0 Or InStr(1, VOL_TITLE, "DELETE", 1) > 0 Or InStr(1, VOL_TITLE, "DROP", 1) > 0 Or InStr(1, VOL_TITLE, "INSERT", 1) > 1 Or InStr(1, VOL_TITLE, "TRACK", 1) > 1 Or InStr(1, VOL_TITLE, "TRACE", 1) > 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                            txt_Hold_VolTitle.Focus()
                            Exit Sub
                        End If
                        VOL_TITLE = TrimAll(VOL_TITLE)
                        'check unwanted characters
                        c = 0
                        counter11 = 0
                        For iloop = 1 To Len(VOL_TITLE)
                            strcurrentchar = Mid(VOL_TITLE, iloop, 1)
                            If c = 0 Then
                                If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter11 = 1
                                End If
                            End If
                        Next
                        If counter11 = 1 Then
                            Label15.Text = " Input  is not Valid!"
                            Label1.Text = ""
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                            txt_Hold_VolTitle.Focus()
                            Exit Sub
                        End If
                    Else
                        VOL_TITLE = ""
                    End If
                Else
                    VOL_TITLE = ""
                End If

                'validation for VOL_ISBN
                Dim COPY_ISBN As Object = Nothing
                If TR_COPY_ISBN.Visible = True Then
                    COPY_ISBN = TrimX(txt_Hold_CopyISBN.Text)
                    If Not String.IsNullOrEmpty(COPY_ISBN) Then
                        COPY_ISBN = RemoveQuotes(COPY_ISBN)
                        If COPY_ISBN.Length > 30 Then 'maximum length
                            Label15.Text = " Data must be of Proper Length.. "
                            Label1.Text = ""
                            txt_Hold_CopyISBN.Focus()
                            Exit Sub
                        End If

                        COPY_ISBN = " " & COPY_ISBN & " "
                        If InStr(1, COPY_ISBN, "CREATE", 1) > 0 Or InStr(1, COPY_ISBN, "DELETE", 1) > 0 Or InStr(1, COPY_ISBN, "DROP", 1) > 0 Or InStr(1, COPY_ISBN, "INSERT", 1) > 1 Or InStr(1, COPY_ISBN, "TRACK", 1) > 1 Or InStr(1, COPY_ISBN, "TRACE", 1) > 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                            Label15.Text = "Do not use reserved Words!"
                            Label1.Text = ""
                            txt_Hold_CopyISBN.Focus()
                            Exit Sub
                        End If
                        COPY_ISBN = TrimX(COPY_ISBN)
                        'check unwanted characters
                        c = 0
                        counter12 = 0
                        For iloop = 1 To Len(COPY_ISBN)
                            strcurrentchar = Mid(COPY_ISBN, iloop, 1)
                            If c = 0 Then
                                If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter12 = 1
                                End If
                            End If
                        Next
                        If counter12 = 1 Then
                            Label15.Text = " Input  is not Valid!"
                            Label1.Text = ""
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                            txt_Hold_CopyISBN.Focus()
                            Exit Sub
                        End If
                    Else
                        COPY_ISBN = ""
                    End If
                Else
                    COPY_ISBN = ""
                End If

                'validation for CLASS NO
                Dim CLASS_NO As Object = Nothing
                If TR_CLASS_NO.Visible = True Then
                    CLASS_NO = TrimX(txt_Hold_ClassNo.Text)
                    If Not String.IsNullOrEmpty(CLASS_NO) Then
                        CLASS_NO = RemoveQuotes(CLASS_NO)
                        If CLASS_NO.Length > 150 Then 'maximum length
                            Label15.Text = " Data must be of Proper Length.. "
                            Label1.Text = ""
                            txt_Hold_ClassNo.Focus()
                            Exit Sub
                        End If

                        CLASS_NO = " " & CLASS_NO & " "
                        If InStr(1, CLASS_NO, "CREATE", 1) > 0 Or InStr(1, CLASS_NO, "DELETE", 1) > 0 Or InStr(1, CLASS_NO, "DROP", 1) > 0 Or InStr(1, CLASS_NO, "INSERT", 1) > 1 Or InStr(1, CLASS_NO, "TRACK", 1) > 1 Or InStr(1, CLASS_NO, "TRACE", 1) > 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                            Label15.Text = "Do not use reserved Words!"
                            Label1.Text = ""
                            txt_Hold_ClassNo.Focus()
                            Exit Sub
                        End If
                        CLASS_NO = TrimX(CLASS_NO)
                        'check unwanted characters
                        c = 0
                        counter13 = 0
                        For iloop = 1 To Len(CLASS_NO)
                            strcurrentchar = Mid(CLASS_NO, iloop, 1)
                            If c = 0 Then
                                If Not InStr("'""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter13 = 1
                                End If
                            End If
                        Next
                        If counter13 = 1 Then
                            Label15.Text = " Input  is not Valid!"
                            Label1.Text = ""
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                            txt_Hold_ClassNo.Focus()
                            Exit Sub
                        End If
                    Else
                        CLASS_NO = ""
                    End If
                Else
                    CLASS_NO = ""
                End If

                'validation for CLASS NO
                Dim BOOK_NO As Object = Nothing
                If TR_BOOK_NO.Visible = True Then
                    BOOK_NO = TrimAll(UCase(txt_Hold_BookNo.Text))
                    If Not String.IsNullOrEmpty(BOOK_NO) Then
                        BOOK_NO = RemoveQuotes(BOOK_NO)
                        If BOOK_NO.Length > 50 Then 'maximum length
                            Label15.Text = " Data must be of Proper Length.. "
                            Label1.Text = ""
                            txt_Hold_BookNo.Focus()
                            Exit Sub
                        End If

                        BOOK_NO = " " & BOOK_NO & " "
                        If InStr(1, BOOK_NO, "CREATE", 1) > 0 Or InStr(1, BOOK_NO, "DELETE", 1) > 0 Or InStr(1, BOOK_NO, "DROP", 1) > 0 Or InStr(1, BOOK_NO, "INSERT", 1) > 1 Or InStr(1, BOOK_NO, "TRACK", 1) > 1 Or InStr(1, BOOK_NO, "TRACE", 1) > 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                            Label15.Text = "Do not use reserved Words!"
                            Label1.Text = ""
                            txt_Hold_BookNo.Focus()
                            Exit Sub
                        End If
                        BOOK_NO = TrimAll(BOOK_NO)
                        'check unwanted characters
                        c = 0
                        counter14 = 0
                        For iloop = 1 To Len(BOOK_NO)
                            strcurrentchar = Mid(BOOK_NO, iloop, 1)
                            If c = 0 Then
                                If Not InStr("'""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter14 = 1
                                End If
                            End If
                        Next
                        If counter14 = 1 Then
                            Label15.Text = " Input  is not Valid!"
                            Label1.Text = ""
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                            txt_Hold_BookNo.Focus()
                            Exit Sub
                        End If
                    Else
                        BOOK_NO = ""
                    End If
                Else
                    BOOK_NO = ""
                End If

                'validation for PAGES
                Dim PAGINATION As Object = Nothing
                If TR_PAGINATION.Visible = True Then
                    PAGINATION = TrimAll(txt_Hold_Pages.Text)
                    If Not String.IsNullOrEmpty(PAGINATION) Then
                        PAGINATION = RemoveQuotes(PAGINATION)
                        If CLASS_NO.Length > 50 Then 'maximum length
                            Label15.Text = " Data must be of Proper Length.. "
                            Label1.Text = ""
                            txt_Hold_Pages.Focus()
                            Exit Sub
                        End If

                        PAGINATION = " " & PAGINATION & " "
                        If InStr(1, PAGINATION, "CREATE", 1) > 0 Or InStr(1, PAGINATION, "DELETE", 1) > 0 Or InStr(1, PAGINATION, "DROP", 1) > 0 Or InStr(1, PAGINATION, "INSERT", 1) > 1 Or InStr(1, PAGINATION, "TRACK", 1) > 1 Or InStr(1, PAGINATION, "TRACE", 1) > 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                            Label15.Text = "Do not use reserved Words!"
                            Label1.Text = ""
                            txt_Hold_Pages.Focus()
                            Exit Sub
                        End If
                        PAGINATION = TrimAll(PAGINATION)
                        'check unwanted characters
                        c = 0
                        counter15 = 0
                        For iloop = 1 To Len(PAGINATION)
                            strcurrentchar = Mid(PAGINATION, iloop, 1)
                            If c = 0 Then
                                If Not InStr("'""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter15 = 1
                                End If
                            End If
                        Next
                        If counter15 = 1 Then
                            Label15.Text = " Input  is not Valid!"
                            Label1.Text = ""
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                            txt_Hold_Pages.Focus()
                            Exit Sub
                        End If
                        If InStr(PAGINATION, "p.") = 0 Then
                            PAGINATION = PAGINATION + "p."
                        End If
                        If InStr(PAGINATION, "..") <> 0 Then
                            PAGINATION = PAGINATION + "."
                        End If
                        If InStr(PAGINATION, "pp.") <> 0 Then
                            PAGINATION = PAGINATION + "p."
                        End If
                        If InStr(PAGINATION, "p.p.") <> 0 Then
                            PAGINATION = PAGINATION + "p."
                        End If
                    Else
                        PAGINATION = ""
                    End If
                Else
                    PAGINATION = ""
                End If

                'validation for SIZE
                Dim SIZE As Object = Nothing
                If TR_SIZE.Visible = True Then
                    SIZE = TrimAll(txt_Hold_Size.Text)
                    If Not String.IsNullOrEmpty(SIZE) Then
                        SIZE = RemoveQuotes(SIZE)
                        If SIZE.Length > 50 Then 'maximum length
                            Label15.Text = " Data must be of Proper Length.. "
                            Label1.Text = ""
                            txt_Hold_Size.Focus()
                            Exit Sub
                        End If

                        SIZE = " " & SIZE & " "
                        If InStr(1, SIZE, "CREATE", 1) > 0 Or InStr(1, SIZE, "DELETE", 1) > 0 Or InStr(1, SIZE, "DROP", 1) > 0 Or InStr(1, SIZE, "INSERT", 1) > 1 Or InStr(1, SIZE, "TRACK", 1) > 1 Or InStr(1, SIZE, "TRACE", 1) > 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                            Label15.Text = "Do not use reserved Words!"
                            Label1.Text = ""
                            txt_Hold_Size.Focus()
                            Exit Sub
                        End If
                        SIZE = TrimAll(SIZE)
                        'check unwanted characters
                        c = 0
                        counter16 = 0
                        For iloop = 1 To Len(SIZE)
                            strcurrentchar = Mid(SIZE, iloop, 1)
                            If c = 0 Then
                                If Not InStr("'""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter16 = 1
                                End If
                            End If
                        Next
                        If counter16 = 1 Then
                            Label15.Text = " Input  is not Valid!"
                            Label1.Text = ""
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                            txt_Hold_Size.Focus()
                            Exit Sub
                        End If
                    Else
                        SIZE = ""
                    End If
                Else
                    SIZE = ""
                End If

                Dim ILLUSTRATION As Object = Nothing
                If TR_ILLUSTRATION.Visible = True Then
                    If CB_Illus.Checked = True Then
                        ILLUSTRATION = "Y"
                    Else
                        ILLUSTRATION = "N"
                    End If
                Else
                    ILLUSTRATION = "N"
                End If

                'validation for COLLECTION TYPE
                Dim COLLECTION_TYPE As Object = Nothing
                If TR_COLLECTION_TYPE.Visible = True Then
                    COLLECTION_TYPE = DDL_CollectionType.SelectedValue
                    If Not String.IsNullOrEmpty(COLLECTION_TYPE) Then
                        COLLECTION_TYPE = RemoveQuotes(COLLECTION_TYPE)
                        If COLLECTION_TYPE.Length > 2 Then 'maximum length
                            Label15.Text = " Data must be of Proper Length.. "
                            Label1.Text = ""
                            DDL_CollectionType.Focus()
                            Exit Sub
                        End If

                        COLLECTION_TYPE = " " & COLLECTION_TYPE & " "
                        If InStr(1, COLLECTION_TYPE, "CREATE", 1) > 0 Or InStr(1, COLLECTION_TYPE, "DELETE", 1) > 0 Or InStr(1, COLLECTION_TYPE, "DROP", 1) > 0 Or InStr(1, COLLECTION_TYPE, "INSERT", 1) > 1 Or InStr(1, COLLECTION_TYPE, "TRACK", 1) > 1 Or InStr(1, COLLECTION_TYPE, "TRACE", 1) > 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                            Label15.Text = " Input  is not Valid!"
                            Label1.Text = ""
                            DDL_CollectionType.Focus()
                            Exit Sub
                        End If
                        COLLECTION_TYPE = TrimX(COLLECTION_TYPE)
                        'check unwanted characters
                        c = 0
                        counter17 = 0
                        For iloop = 1 To Len(COLLECTION_TYPE)
                            strcurrentchar = Mid(COLLECTION_TYPE, iloop, 1)
                            If c = 0 Then
                                If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter17 = 1
                                End If
                            End If
                        Next
                        If counter17 = 1 Then
                            Label15.Text = " Input  is not Valid!"
                            Label1.Text = ""
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                            DDL_CollectionType.Focus()
                            Exit Sub
                        End If
                    Else
                        COLLECTION_TYPE = "C"
                    End If
                Else
                    COLLECTION_TYPE = "C"
                End If

                'validation for STATUS
                Dim STA_CODE As Object = Nothing
                If TR_STA_CODE.Visible = True Then
                    If DDL_Status.Text <> "" Then
                        STA_CODE = DDL_Status.SelectedValue
                        If STA_CODE = "2" Then
                            STA_CODE = "1"
                        End If
                    Else
                        STA_CODE = "1"
                    End If
                Else
                    STA_CODE = "1"
                End If


                'validation for BINDING TYPE
                Dim BIND_CODE As Object = Nothing
                If TR_BIND_CODE.Visible = True Then
                    BIND_CODE = DDL_Binding.SelectedValue
                    If Not String.IsNullOrEmpty(BIND_CODE) Then
                        BIND_CODE = RemoveQuotes(BIND_CODE)
                        If BIND_CODE.Length > 11 Then 'maximum length
                            Label15.Text = " Data must be of Proper Length.. "
                            Label1.Text = ""
                            DDL_Binding.Focus()
                            Exit Sub
                        End If

                        BIND_CODE = " " & BIND_CODE & " "
                        If InStr(1, BIND_CODE, "CREATE", 1) > 0 Or InStr(1, BIND_CODE, "DELETE", 1) > 0 Or InStr(1, BIND_CODE, "DROP", 1) > 0 Or InStr(1, BIND_CODE, "INSERT", 1) > 1 Or InStr(1, BIND_CODE, "TRACK", 1) > 1 Or InStr(1, BIND_CODE, "TRACE", 1) > 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                            Label15.Text = " Input  is not Valid!"
                            Label1.Text = ""
                            DDL_Binding.Focus()
                            Exit Sub
                        End If
                        BIND_CODE = TrimX(BIND_CODE)
                        'check unwanted characters
                        c = 0
                        counter18 = 0
                        For iloop = 1 To Len(BIND_CODE)
                            strcurrentchar = Mid(BIND_CODE, iloop, 1)
                            If c = 0 Then
                                If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter18 = 1
                                End If
                            End If
                        Next
                        If counter18 = 1 Then
                            Label15.Text = " Input  is not Valid!"
                            Label1.Text = ""
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                            DDL_Binding.Focus()
                            Exit Sub
                        End If
                    Else
                        BIND_CODE = "U"
                    End If
                Else
                    BIND_CODE = "U"
                End If

                'validation for SECTION
                Dim SEC_CODE As Object = Nothing
                If TR_SEC_CODE.Visible = True Then
                    SEC_CODE = DDL_Section.SelectedValue
                    If Not String.IsNullOrEmpty(SEC_CODE) Then
                        SEC_CODE = RemoveQuotes(SEC_CODE)
                        If SEC_CODE.Length > 11 Then 'maximum length
                            Label15.Text = " Data must be of Proper Length.. "
                            Label1.Text = ""
                            DDL_Section.Focus()
                            Exit Sub
                        End If

                        SEC_CODE = " " & SEC_CODE & " "
                        If InStr(1, SEC_CODE, "CREATE", 1) > 0 Or InStr(1, SEC_CODE, "DELETE", 1) > 0 Or InStr(1, SEC_CODE, "DROP", 1) > 0 Or InStr(1, SEC_CODE, "INSERT", 1) > 1 Or InStr(1, SEC_CODE, "TRACK", 1) > 1 Or InStr(1, SEC_CODE, "TRACE", 1) > 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                            Label15.Text = " Input  is not Valid!"
                            Label1.Text = ""
                            DDL_Section.Focus()
                            Exit Sub
                        End If
                        SEC_CODE = TrimX(SEC_CODE)
                        'check unwanted characters
                        c = 0
                        counter19 = 0
                        For iloop = 1 To Len(SEC_CODE)
                            strcurrentchar = Mid(SEC_CODE, iloop, 1)
                            If c = 0 Then
                                If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter19 = 1
                                End If
                            End If
                        Next
                        If counter19 = 1 Then
                            Label15.Text = " Input  is not Valid!"
                            Label1.Text = ""
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                            DDL_Section.Focus()
                            Exit Sub
                        End If
                    Else
                        SEC_CODE = ""
                    End If
                Else
                    SEC_CODE = ""
                End If

                'validation for holding Library
                Dim HOLD_LIB_CODE As Object = Nothing
                HOLD_LIB_CODE = DDL_Library.SelectedValue
                If Not String.IsNullOrEmpty(HOLD_LIB_CODE) Then
                    HOLD_LIB_CODE = RemoveQuotes(HOLD_LIB_CODE)
                    If HOLD_LIB_CODE.Length > 11 Then 'maximum length
                        Label15.Text = " Data must be of Proper Length.. "
                        Label1.Text = ""
                        DDL_Library.Focus()
                        Exit Sub
                    End If

                    HOLD_LIB_CODE = " " & HOLD_LIB_CODE & " "
                    If InStr(1, HOLD_LIB_CODE, "CREATE", 1) > 0 Or InStr(1, HOLD_LIB_CODE, "DELETE", 1) > 0 Or InStr(1, HOLD_LIB_CODE, "DROP", 1) > 0 Or InStr(1, HOLD_LIB_CODE, "INSERT", 1) > 1 Or InStr(1, HOLD_LIB_CODE, "TRACK", 1) > 1 Or InStr(1, HOLD_LIB_CODE, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Label15.Text = " Input  is not Valid!"
                        Label1.Text = ""
                        DDL_Library.Focus()
                        Exit Sub
                    End If
                    HOLD_LIB_CODE = TrimX(HOLD_LIB_CODE)
                    'check unwanted characters
                    c = 0
                    counter20 = 0
                    For iloop = 1 To Len(HOLD_LIB_CODE)
                        strcurrentchar = Mid(HOLD_LIB_CODE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter20 = 1
                            End If
                        End If
                    Next
                    If counter20 = 1 Then
                        Label15.Text = " Input  is not Valid!"
                        Label1.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                        DDL_Library.Focus()
                        Exit Sub
                    End If
                Else
                    HOLD_LIB_CODE = LibCode
                End If

                'validation for accompanying materials
                Dim ACC_MAT_CODE As Object = Nothing
                If TR_ACC_MAT_CODE.Visible = True Then
                    ACC_MAT_CODE = DDL_AccMaterials.SelectedValue
                    If Not String.IsNullOrEmpty(ACC_MAT_CODE) Then
                        ACC_MAT_CODE = RemoveQuotes(ACC_MAT_CODE)
                        If ACC_MAT_CODE.Length > 11 Then 'maximum length
                            Label15.Text = " Data must be of Proper Length.. "
                            Label1.Text = ""
                            DDL_AccMaterials.Focus()
                            Exit Sub
                        End If

                        ACC_MAT_CODE = " " & ACC_MAT_CODE & " "
                        If InStr(1, ACC_MAT_CODE, "CREATE", 1) > 0 Or InStr(1, ACC_MAT_CODE, "DELETE", 1) > 0 Or InStr(1, ACC_MAT_CODE, "DROP", 1) > 0 Or InStr(1, ACC_MAT_CODE, "INSERT", 1) > 1 Or InStr(1, ACC_MAT_CODE, "TRACK", 1) > 1 Or InStr(1, ACC_MAT_CODE, "TRACE", 1) > 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                            Label15.Text = " Input  is not Valid!"
                            Label1.Text = ""
                            DDL_AccMaterials.Focus()
                            Exit Sub
                        End If
                        ACC_MAT_CODE = TrimX(ACC_MAT_CODE)
                        'check unwanted characters
                        c = 0
                        counter21 = 0
                        For iloop = 1 To Len(ACC_MAT_CODE)
                            strcurrentchar = Mid(ACC_MAT_CODE, iloop, 1)
                            If c = 0 Then
                                If Not InStr("'<>=%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter21 = 1
                                End If
                            End If
                        Next
                        If counter21 = 1 Then
                            Label15.Text = " Input  is not Valid!"
                            Label1.Text = ""
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                            DDL_AccMaterials.Focus()
                            Exit Sub
                        End If
                    Else
                        ACC_MAT_CODE = ""
                    End If
                Else
                    ACC_MAT_CODE = ""
                End If

                'validation for REMARKS
                Dim REMARKS As Object = Nothing
                If TR_REMARKS.Visible = True Then
                    REMARKS = TrimAll(txt_Hold_Remarks.Text)
                    If Not String.IsNullOrEmpty(REMARKS) Then
                        REMARKS = RemoveQuotes(REMARKS)
                        If REMARKS.Length > 251 Then 'maximum length
                            Label15.Text = " Data must be of Proper Length.. "
                            Label1.Text = ""
                            txt_Hold_Remarks.Focus()
                            Exit Sub
                        End If
                    Else
                        REMARKS = ""
                    End If
                Else
                    REMARKS = ""
                End If

                'validation for Physical Location
                Dim PHYSICAL_LOCATION As Object = Nothing
                If TR_REMARKS.Visible = True Then
                    PHYSICAL_LOCATION = TrimAll(txt_Hold_Location.Text)
                    If Not String.IsNullOrEmpty(PHYSICAL_LOCATION) Then
                        PHYSICAL_LOCATION = RemoveQuotes(PHYSICAL_LOCATION)
                        If PHYSICAL_LOCATION.Length > 50 Then 'maximum length
                            Label15.Text = " Data must be of Proper Length.. "
                            Label1.Text = ""
                            txt_Hold_Location.Focus()
                            Exit Sub
                        End If

                        PHYSICAL_LOCATION = " " & PHYSICAL_LOCATION & " "
                        If InStr(1, PHYSICAL_LOCATION, "CREATE", 1) > 0 Or InStr(1, PHYSICAL_LOCATION, "DELETE", 1) > 0 Or InStr(1, PHYSICAL_LOCATION, "DROP", 1) > 0 Or InStr(1, PHYSICAL_LOCATION, "INSERT", 1) > 1 Or InStr(1, PHYSICAL_LOCATION, "TRACK", 1) > 1 Or InStr(1, PHYSICAL_LOCATION, "TRACE", 1) > 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                            Label15.Text = "Do not use reserved Words!"
                            Label1.Text = ""
                            txt_Hold_Location.Focus()
                            Exit Sub
                        End If
                        PHYSICAL_LOCATION = TrimAll(PHYSICAL_LOCATION)
                        'check unwanted characters
                        c = 0
                        counter22 = 0
                        For iloop = 1 To Len(PHYSICAL_LOCATION)
                            strcurrentchar = Mid(PHYSICAL_LOCATION, iloop, 1)
                            If c = 0 Then
                                If Not InStr("'""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter22 = 1
                                End If
                            End If
                        Next
                        If counter22 = 1 Then
                            Label15.Text = " Input  is not Valid!"
                            Label1.Text = ""
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                            txt_Hold_Location.Focus()
                            Exit Sub
                        End If
                    Else
                        PHYSICAL_LOCATION = ""
                    End If
                Else
                    PHYSICAL_LOCATION = ""
                End If

                'validation for REFERENCE NO
                Dim REFERENCE_NO As Object = Nothing
                If TR_REFERENCE_NO.Visible = True Then
                    REFERENCE_NO = TrimAll(txt_Hold_ReferenceNo.Text)
                    If Not String.IsNullOrEmpty(REFERENCE_NO) Then
                        REFERENCE_NO = RemoveQuotes(REFERENCE_NO)
                        If REFERENCE_NO.Length > 50 Then 'maximum length
                            Label15.Text = " Data must be of Proper Length.. "
                            Label1.Text = ""
                            txt_Hold_ReferenceNo.Focus()
                            Exit Sub
                        End If

                        REFERENCE_NO = " " & REFERENCE_NO & " "
                        If InStr(1, REFERENCE_NO, "CREATE", 1) > 0 Or InStr(1, REFERENCE_NO, "DELETE", 1) > 0 Or InStr(1, REFERENCE_NO, "DROP", 1) > 0 Or InStr(1, REFERENCE_NO, "INSERT", 1) > 1 Or InStr(1, REFERENCE_NO, "TRACK", 1) > 1 Or InStr(1, REFERENCE_NO, "TRACE", 1) > 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                            Label15.Text = "Do not use reserved Words!"
                            Label1.Text = ""
                            txt_Hold_ReferenceNo.Focus()
                            Exit Sub
                        End If
                        REFERENCE_NO = TrimAll(REFERENCE_NO)
                        'check unwanted characters
                        c = 0
                        counter23 = 0
                        For iloop = 1 To Len(REFERENCE_NO)
                            strcurrentchar = Mid(REFERENCE_NO, iloop, 1)
                            If c = 0 Then
                                If Not InStr("'""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter23 = 1
                                End If
                            End If
                        Next
                        If counter23 = 1 Then
                            Label15.Text = " Input  is not Valid!"
                            Label1.Text = ""
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                            txt_Hold_ReferenceNo.Focus()
                            Exit Sub
                        End If
                    Else
                        REFERENCE_NO = ""
                    End If
                Else
                    REFERENCE_NO = ""
                End If

                'validation for RECODING_MEDIUM
                Dim RECORDING_MEDIUM As Object = Nothing
                If TR_MEDIUM.Visible = True Then
                    RECORDING_MEDIUM = TrimAll(txt_Hold_RecordingMedium.Text)
                    If Not String.IsNullOrEmpty(RECORDING_MEDIUM) Then
                        RECORDING_MEDIUM = RemoveQuotes(RECORDING_MEDIUM)
                        If RECORDING_MEDIUM.Length > 51 Then 'maximum length
                            Label15.Text = " Data must be of Proper Length.. "
                            Label1.Text = ""
                            txt_Hold_RecordingMedium.Focus()
                            Exit Sub
                        End If
                    Else
                        RECORDING_MEDIUM = ""
                    End If
                Else
                    RECORDING_MEDIUM = ""
                End If

                'validation for RECODING_MEDIUM
                Dim RECORDING_CATEGORY As Object = Nothing
                If TR_RECORDING_CATEGORY.Visible = True Then
                    RECORDING_CATEGORY = TrimAll(txt_Hold_RecordingCategory.Text)
                    If Not String.IsNullOrEmpty(RECORDING_CATEGORY) Then
                        RECORDING_CATEGORY = RemoveQuotes(RECORDING_CATEGORY)
                        If RECORDING_CATEGORY.Length > 51 Then 'maximum length
                            Label15.Text = " Data must be of Proper Length.. "
                            Label1.Text = ""
                            txt_Hold_RecordingCategory.Focus()
                            Exit Sub
                        End If
                    Else
                        RECORDING_CATEGORY = ""
                    End If
                Else
                    RECORDING_CATEGORY = ""
                End If

                'validation for RECORDING_FORM
                Dim RECORDING_FORM As Object = Nothing
                If TR_RECORDING_FORM.Visible = True Then
                    RECORDING_FORM = TrimAll(txt_Hold_RecordingForm.Text)
                    If Not String.IsNullOrEmpty(RECORDING_FORM) Then
                        RECORDING_FORM = RemoveQuotes(RECORDING_FORM)
                        If RECORDING_FORM.Length > 51 Then 'maximum length
                            Label15.Text = " Data must be of Proper Length.. "
                            Label1.Text = ""
                            txt_Hold_RecordingForm.Focus()
                            Exit Sub
                        End If
                    Else
                        RECORDING_FORM = ""
                    End If
                Else
                    RECORDING_FORM = ""
                End If

                'validation for RECORDING_FORMAT
                Dim RECORDING_FORMAT As Object = Nothing
                If TR_RECORDING_FORMAT.Visible = True Then
                    RECORDING_FORMAT = TrimAll(txt_Hold_RecordingFormat.Text)
                    If Not String.IsNullOrEmpty(RECORDING_FORMAT) Then
                        RECORDING_FORMAT = RemoveQuotes(RECORDING_FORMAT)
                        If RECORDING_FORMAT.Length > 51 Then 'maximum length
                            Label15.Text = " Data must be of Proper Length.. "
                            Label1.Text = ""
                            txt_Hold_RecordingFormat.Focus()
                            Exit Sub
                        End If
                    Else
                        RECORDING_FORMAT = ""
                    End If
                Else
                    RECORDING_FORMAT = ""
                End If

                'validation for RECORDING_SPEED
                Dim RECORDING_SPEED As Object = Nothing
                If TR_RECORDING_SPEED.Visible = True Then
                    RECORDING_SPEED = TrimAll(txt_Hold_RecordingSpeed.Text)
                    If Not String.IsNullOrEmpty(RECORDING_SPEED) Then
                        RECORDING_SPEED = RemoveQuotes(RECORDING_SPEED)
                        If RECORDING_SPEED.Length > 51 Then 'maximum length
                            Label15.Text = " Data must be of Proper Length.. "
                            Label1.Text = ""
                            txt_Hold_RecordingSpeed.Focus()
                            Exit Sub
                        End If
                    Else
                        RECORDING_SPEED = ""
                    End If
                Else
                    RECORDING_SPEED = ""
                End If

                'validation for RECORDING_STORAGE_TECH
                Dim RECORDING_STORAGE_TECH As Object = Nothing
                If TR_RECORDING_STORAGE_TECH.Visible = True Then
                    RECORDING_STORAGE_TECH = TrimAll(txt_Hold_RecordingStorageTech.Text)
                    If Not String.IsNullOrEmpty(RECORDING_STORAGE_TECH) Then
                        RECORDING_STORAGE_TECH = RemoveQuotes(RECORDING_STORAGE_TECH)
                        If RECORDING_STORAGE_TECH.Length > 51 Then 'maximum length
                            Label15.Text = " Data must be of Proper Length.. "
                            Label1.Text = ""
                            txt_Hold_RecordingStorageTech.Focus()
                            Exit Sub
                        End If
                    Else
                        RECORDING_STORAGE_TECH = ""
                    End If
                Else
                    RECORDING_STORAGE_TECH = ""
                End If

                'validation for RECORDING_PLAY_DURATION
                Dim RECORDING_PLAY_DURATION As Object = Nothing
                If TR_RECORDING_PLAY_DURATION.Visible = True Then
                    RECORDING_PLAY_DURATION = TrimAll(txt_Hold_RecordingDuration.Text)
                    If Not String.IsNullOrEmpty(RECORDING_PLAY_DURATION) Then
                        RECORDING_PLAY_DURATION = RemoveQuotes(RECORDING_PLAY_DURATION)
                        If RECORDING_PLAY_DURATION.Length > 51 Then 'maximum length
                            Label15.Text = " Data must be of Proper Length.. "
                            Label1.Text = ""
                            txt_Hold_RecordingDuration.Focus()
                            Exit Sub
                        End If
                    Else
                        RECORDING_PLAY_DURATION = ""
                    End If
                Else
                    RECORDING_PLAY_DURATION = ""
                End If

                'validation for TYPE OF VISUALS
                Dim VIDEO_TYPEOFVISUALS As Object = Nothing
                If TR_VIDEO_TYPEOFVISUAL.Visible = True Then
                    VIDEO_TYPEOFVISUALS = TrimAll(txt_Hold_TypeOfVisuals.Text)
                    If Not String.IsNullOrEmpty(VIDEO_TYPEOFVISUALS) Then
                        VIDEO_TYPEOFVISUALS = RemoveQuotes(VIDEO_TYPEOFVISUALS)
                        If VIDEO_TYPEOFVISUALS.Length > 51 Then 'maximum length
                            Label15.Text = " Data must be of Proper Length.. "
                            Label1.Text = ""
                            txt_Hold_TypeOfVisuals.Focus()
                            Exit Sub
                        End If
                    Else
                        VIDEO_TYPEOFVISUALS = ""
                    End If
                Else
                    VIDEO_TYPEOFVISUALS = ""
                End If

                'validation for CARTOGRAPHIC_SCALE
                Dim CARTOGRAPHIC_SCALE As Object = Nothing
                If TR_CARTOGRAPHIC_SCALE.Visible = True Then
                    CARTOGRAPHIC_SCALE = TrimAll(txt_Hold_Scale.Text)
                    If Not String.IsNullOrEmpty(CARTOGRAPHIC_SCALE) Then
                        CARTOGRAPHIC_SCALE = RemoveQuotes(CARTOGRAPHIC_SCALE)
                        If CARTOGRAPHIC_SCALE.Length > 51 Then 'maximum length
                            Label15.Text = " Data must be of Proper Length.. "
                            Label1.Text = ""
                            txt_Hold_Scale.Focus()
                            Exit Sub
                        End If
                    Else
                        CARTOGRAPHIC_SCALE = ""
                    End If
                Else
                    CARTOGRAPHIC_SCALE = ""
                End If

                'validation for CARTOGRAPHIC_PROJECTION
                Dim CARTOGRAPHIC_PROJECTION As Object = Nothing
                If TR_CARTOGRAPHIC_PROJECTION.Visible = True Then
                    CARTOGRAPHIC_PROJECTION = TrimAll(txt_Hold_Projection.Text)
                    If Not String.IsNullOrEmpty(CARTOGRAPHIC_PROJECTION) Then
                        CARTOGRAPHIC_PROJECTION = RemoveQuotes(CARTOGRAPHIC_PROJECTION)
                        If CARTOGRAPHIC_PROJECTION.Length > 51 Then 'maximum length
                            Label15.Text = " Data must be of Proper Length.. "
                            Label1.Text = ""
                            txt_Hold_Projection.Focus()
                            Exit Sub
                        End If
                    Else
                        CARTOGRAPHIC_PROJECTION = ""
                    End If
                Else
                    CARTOGRAPHIC_PROJECTION = ""
                End If

                'validation for CARTOGRAPHIC_COORDINATES
                Dim CARTOGRAPHIC_COORDINATES As Object = Nothing
                If TR_CARTOGRAPHIC_COORDINATES.Visible = True Then
                    CARTOGRAPHIC_COORDINATES = TrimAll(txt_Hold_Coordinates.Text)
                    If Not String.IsNullOrEmpty(CARTOGRAPHIC_COORDINATES) Then
                        CARTOGRAPHIC_COORDINATES = RemoveQuotes(CARTOGRAPHIC_COORDINATES)
                        If CARTOGRAPHIC_COORDINATES.Length > 51 Then 'maximum length
                            Label15.Text = " Data must be of Proper Length.. "
                            Label1.Text = ""
                            txt_Hold_Coordinates.Focus()
                            Exit Sub
                        End If
                    Else
                        CARTOGRAPHIC_COORDINATES = ""
                    End If
                Else
                    CARTOGRAPHIC_COORDINATES = ""
                End If


                'validation for CARTOGRAPHIC_GEOGRAPHIC_LOCATION
                Dim CARTOGRAPHIC_GEOGRAPHIC_LOCATION As Object = Nothing
                If TR_CARTOGRAPHIC_GEOGRAPHIC_LOCATION.Visible = True Then
                    CARTOGRAPHIC_COORDINATES = TrimAll(txt_Hold_GeographicLocation.Text)
                    If Not String.IsNullOrEmpty(CARTOGRAPHIC_GEOGRAPHIC_LOCATION) Then
                        CARTOGRAPHIC_GEOGRAPHIC_LOCATION = RemoveQuotes(CARTOGRAPHIC_GEOGRAPHIC_LOCATION)
                        If CARTOGRAPHIC_GEOGRAPHIC_LOCATION.Length > 51 Then 'maximum length
                            Label15.Text = " Data must be of Proper Length.. "
                            Label1.Text = ""
                            txt_Hold_GeographicLocation.Focus()
                            Exit Sub
                        End If
                    Else
                        CARTOGRAPHIC_GEOGRAPHIC_LOCATION = ""
                    End If
                Else
                    CARTOGRAPHIC_GEOGRAPHIC_LOCATION = ""
                End If

                'validation for CARTOGRAPHIC_MEDIUM
                Dim CARTOGRAPHIC_MEDIUM As Object = Nothing
                If TR_CARTOGRAPHIC_MEDIUM.Visible = True Then
                    CARTOGRAPHIC_MEDIUM = Trim(DDL_GeographicMedium.SelectedValue)
                    If Not String.IsNullOrEmpty(CARTOGRAPHIC_GEOGRAPHIC_LOCATION) Then
                        CARTOGRAPHIC_MEDIUM = RemoveQuotes(CARTOGRAPHIC_MEDIUM)
                        If CARTOGRAPHIC_MEDIUM.Length > 50 Then 'maximum length
                            Label15.Text = " Data must be of Proper Length.. "
                            Label1.Text = ""
                            DDL_GeographicMedium.Focus()
                            Exit Sub
                        End If
                    Else
                        CARTOGRAPHIC_MEDIUM = ""
                    End If
                Else
                    CARTOGRAPHIC_MEDIUM = ""
                End If

                'search CARTOGRAPHIC_DATAGATHERING_DATE
                Dim CARTOGRAPHIC_DATAGATHERING_DATE As Object = Nothing
                If TR_CARTOGRAPHIC_DATAGATHERING_DATE.Visible = True Then
                    If txt_Hold_DataGatheringDate.Text <> "" Then
                        CARTOGRAPHIC_DATAGATHERING_DATE = TrimX(txt_Hold_DataGatheringDate.Text)
                        CARTOGRAPHIC_DATAGATHERING_DATE = RemoveQuotes(CARTOGRAPHIC_DATAGATHERING_DATE)
                        CARTOGRAPHIC_DATAGATHERING_DATE = Convert.ToDateTime(CARTOGRAPHIC_DATAGATHERING_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                        If Len(CARTOGRAPHIC_DATAGATHERING_DATE) > 12 Then
                            Label15.Text = " Input is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_DataGatheringDate.Focus()
                            Exit Sub
                        End If
                    Else
                        CARTOGRAPHIC_DATAGATHERING_DATE = ""
                    End If
                Else
                    CARTOGRAPHIC_DATAGATHERING_DATE = ""
                End If

                'search CARTOGRAPHIC_CREATION_DATE
                Dim CARTOGRAPHIC_CREATION_DATE As Object = Nothing
                If TR_CREATION_DATE.Visible = True Then
                    If txt_Hold_CreationDate.Text <> "" Then
                        CARTOGRAPHIC_CREATION_DATE = TrimX(txt_Hold_CreationDate.Text)
                        CARTOGRAPHIC_CREATION_DATE = RemoveQuotes(CARTOGRAPHIC_CREATION_DATE)
                        CARTOGRAPHIC_CREATION_DATE = Convert.ToDateTime(CARTOGRAPHIC_CREATION_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                        If Len(CARTOGRAPHIC_CREATION_DATE) > 12 Then
                            Label15.Text = " Input is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_CreationDate.Focus()
                            Exit Sub
                        End If
                    Else
                        CARTOGRAPHIC_CREATION_DATE = ""
                    End If
                Else
                    CARTOGRAPHIC_CREATION_DATE = ""
                End If

                'search CARTOGRAPHIC_COMPILATION_DATE
                Dim CARTOGRAPHIC_COMPILATION_DATE As Object = Nothing
                If TR_CARTOGRAPHIC_COMPILATION_DATE.Visible = True Then
                    If txt_Hold_CompilationDate.Text <> "" Then
                        CARTOGRAPHIC_COMPILATION_DATE = TrimX(txt_Hold_CompilationDate.Text)
                        CARTOGRAPHIC_COMPILATION_DATE = RemoveQuotes(CARTOGRAPHIC_COMPILATION_DATE)
                        CARTOGRAPHIC_COMPILATION_DATE = Convert.ToDateTime(CARTOGRAPHIC_COMPILATION_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                        If Len(CARTOGRAPHIC_COMPILATION_DATE) > 12 Then
                            Label15.Text = " Input is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_CompilationDate.Focus()
                            Exit Sub
                        End If
                    Else
                        CARTOGRAPHIC_COMPILATION_DATE = ""
                    End If
                Else
                    CARTOGRAPHIC_COMPILATION_DATE = ""
                End If

                'validation for  CARTOGRAPHIC_INSPECTION_DATE
                Dim CARTOGRAPHIC_INSPECTION_DATE As Object = Nothing
                If TR_CARTOGRAPHIC_INSPECTION_DATE.Visible = True Then
                    If txt_Hold_InspectionDate.Text <> "" Then
                        CARTOGRAPHIC_INSPECTION_DATE = TrimX(txt_Hold_InspectionDate.Text)
                        CARTOGRAPHIC_INSPECTION_DATE = RemoveQuotes(CARTOGRAPHIC_INSPECTION_DATE)
                        CARTOGRAPHIC_INSPECTION_DATE = Convert.ToDateTime(CARTOGRAPHIC_INSPECTION_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                        If Len(CARTOGRAPHIC_INSPECTION_DATE) > 12 Then
                            Label15.Text = " Input is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_InspectionDate.Focus()
                            Exit Sub
                        End If
                    Else
                        CARTOGRAPHIC_INSPECTION_DATE = ""
                    End If
                Else
                    CARTOGRAPHIC_INSPECTION_DATE = ""
                End If


                'validation for  VIDEO_COLOR
                Dim VIDEO_COLOR As Object = Nothing
                If TR_VIDEO_COLOR.Visible = True Then
                    If txt_Hold_Color.Text <> "" Then
                        VIDEO_COLOR = TrimAll(txt_Hold_Color.Text)
                        VIDEO_COLOR = RemoveQuotes(VIDEO_COLOR)

                        If Len(VIDEO_COLOR) > 50 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_Color.Focus()
                            Exit Sub
                        End If
                    Else
                        VIDEO_COLOR = ""
                    End If
                Else
                    VIDEO_COLOR = ""
                End If


                'validation for  PLAYBACK_CHANNELS
                Dim PLAYBACK_CHANNELS As Object = Nothing
                If TR_PLAYBACK_CHANNELS.Visible = True Then
                    If txt_Hold_PlayBackChannel.Text <> "" Then
                        PLAYBACK_CHANNELS = TrimAll(txt_Hold_PlayBackChannel.Text)
                        PLAYBACK_CHANNELS = RemoveQuotes(PLAYBACK_CHANNELS)
                        If Len(PLAYBACK_CHANNELS) > 50 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_PlayBackChannel.Focus()
                            Exit Sub
                        End If
                    Else
                        PLAYBACK_CHANNELS = ""
                    End If
                Else
                    PLAYBACK_CHANNELS = ""
                End If


                'validation for  TAPE_WIDTH
                Dim TAPE_WIDTH As Object = Nothing
                If TR_TAPE_WIDTH.Visible = True Then
                    If txt_Hold_PlayBackChannel.Text <> "" Then
                        TAPE_WIDTH = TrimAll(txt_Hold_PlayBackChannel.Text)
                        TAPE_WIDTH = RemoveQuotes(TAPE_WIDTH)
                        If Len(TAPE_WIDTH) > 50 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_PlayBackChannel.Focus()
                            Exit Sub
                        End If
                    Else
                        TAPE_WIDTH = ""
                    End If
                Else
                    TAPE_WIDTH = ""
                End If

                'validation for  TAPE_CONFIGURATION
                Dim TAPE_CONFIGURATION As Object = Nothing
                If TR_TAPE_CONFIGURATION.Visible = True Then
                    If txt_Hold_TapeConfiguration.Text <> "" Then
                        TAPE_CONFIGURATION = TrimAll(txt_Hold_TapeConfiguration.Text)
                        TAPE_CONFIGURATION = RemoveQuotes(TAPE_CONFIGURATION)
                        If Len(TAPE_CONFIGURATION) > 50 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_TapeConfiguration.Focus()
                            Exit Sub
                        End If
                    Else
                        TAPE_CONFIGURATION = ""
                    End If
                Else
                    TAPE_CONFIGURATION = ""
                End If

                'validation for  KIND_OF_DISK
                Dim KIND_OF_DISK As Object = Nothing
                If TR_KIND_OF_DISK.Visible = True Then
                    If txt_Hold_KindofDisk.Text <> "" Then
                        KIND_OF_DISK = TrimAll(txt_Hold_KindofDisk.Text)
                        KIND_OF_DISK = RemoveQuotes(KIND_OF_DISK)
                        If Len(KIND_OF_DISK) > 50 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_KindofDisk.Focus()
                            Exit Sub
                        End If
                    Else
                        KIND_OF_DISK = ""
                    End If
                Else
                    KIND_OF_DISK = ""
                End If

                'validation for  KIND_OF_CUTTING
                Dim KIND_OF_CUTTING As Object = Nothing
                If TR_KIND_OF_CUTTING.Visible = True Then
                    If txt_Hold_KindofCutting.Text <> "" Then
                        KIND_OF_CUTTING = TrimAll(txt_Hold_KindofCutting.Text)
                        KIND_OF_CUTTING = RemoveQuotes(KIND_OF_CUTTING)
                        If Len(KIND_OF_DISK) > 50 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_KindofCutting.Focus()
                            Exit Sub
                        End If
                    Else
                        KIND_OF_CUTTING = ""
                    End If
                Else
                    KIND_OF_CUTTING = ""
                End If


                'validation for  ENCODING_STANDARD
                Dim ENCODING_STANDARD As Object = Nothing
                If TR_ENCODING_STANDARD.Visible = True Then
                    If txt_Hold_EncodingStandard.Text <> "" Then
                        ENCODING_STANDARD = TrimAll(txt_Hold_EncodingStandard.Text)
                        ENCODING_STANDARD = RemoveQuotes(ENCODING_STANDARD)
                        If Len(KIND_OF_DISK) > 50 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_EncodingStandard.Focus()
                            Exit Sub
                        End If
                    Else
                        ENCODING_STANDARD = ""
                    End If
                Else
                    ENCODING_STANDARD = ""
                End If

                'validation for  CAPTURE_TECHNIQUE
                Dim CAPTURE_TECHNIQUE As Object = Nothing
                If TR_CAPTURE_TECHNIQUE.Visible = True Then
                    If txt_Hold_CaptureTechnique.Text <> "" Then
                        CAPTURE_TECHNIQUE = TrimAll(txt_Hold_CaptureTechnique.Text)
                        CAPTURE_TECHNIQUE = RemoveQuotes(CAPTURE_TECHNIQUE)
                        If Len(CAPTURE_TECHNIQUE) > 50 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_CaptureTechnique.Focus()
                            Exit Sub
                        End If
                    Else
                        CAPTURE_TECHNIQUE = ""
                    End If
                Else
                    CAPTURE_TECHNIQUE = ""
                End If

                'validation for  PHOTO_NO
                Dim PHOTO_NO As Object = Nothing
                If TR_PHOTO_NO.Visible = True Then
                    If txt_Hold_PhotoNo.Text <> "" Then
                        PHOTO_NO = TrimAll(txt_Hold_PhotoNo.Text)
                        PHOTO_NO = RemoveQuotes(PHOTO_NO)
                        If Len(PHOTO_NO) > 50 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_PhotoNo.Focus()
                            Exit Sub
                        End If
                    Else
                        PHOTO_NO = ""
                    End If
                Else
                    PHOTO_NO = ""
                End If


                'validation for  PHOTO_ALBUM_NO
                Dim PHOTO_ALBUM_NO As Object = Nothing
                If TR_PHOTO_ALBUM_NO.Visible = True Then
                    If txt_Hold_PhotoAlbumNo.Text <> "" Then
                        PHOTO_ALBUM_NO = TrimAll(txt_Hold_PhotoAlbumNo.Text)
                        PHOTO_ALBUM_NO = RemoveQuotes(PHOTO_ALBUM_NO)
                        If Len(PHOTO_ALBUM_NO) > 150 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_PhotoAlbumNo.Focus()
                            Exit Sub
                        End If
                    Else
                        PHOTO_ALBUM_NO = ""
                    End If
                Else
                    PHOTO_ALBUM_NO = ""
                End If

                'validation for  PHOTO_OCASION
                Dim PHOTO_OCASION As Object = Nothing
                If TR_PHOTO_OCASION.Visible = True Then
                    If txt_Hold_Ocasion.Text <> "" Then
                        PHOTO_OCASION = TrimAll(txt_Hold_Ocasion.Text)
                        PHOTO_OCASION = RemoveQuotes(PHOTO_OCASION)
                        If Len(PHOTO_OCASION) > 150 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_Ocasion.Focus()
                            Exit Sub
                        End If
                    Else
                        PHOTO_OCASION = ""
                    End If
                Else
                    PHOTO_OCASION = ""
                End If

                'validation for  IMAGE_VIEW_TYPE
                Dim IMAGE_VIEW_TYPE As Object = Nothing
                If TR_IMAGE_VIEW_TYPE.Visible = True Then
                    If txt_Hold_ImageViewType.Text <> "" Then
                        IMAGE_VIEW_TYPE = TrimAll(txt_Hold_ImageViewType.Text)
                        IMAGE_VIEW_TYPE = RemoveQuotes(IMAGE_VIEW_TYPE)
                        If Len(IMAGE_VIEW_TYPE) > 50 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_ImageViewType.Focus()
                            Exit Sub
                        End If
                    Else
                        IMAGE_VIEW_TYPE = ""
                    End If
                Else
                    IMAGE_VIEW_TYPE = ""
                End If

                'search VIEW_DATE
                Dim VIEW_DATE As Object = Nothing
                If TR_VIEW_DATE.Visible = True Then
                    If txt_Hold_ViewDate.Text <> "" Then
                        VIEW_DATE = TrimX(txt_Hold_ViewDate.Text)
                        VIEW_DATE = RemoveQuotes(VIEW_DATE)
                        VIEW_DATE = Convert.ToDateTime(VIEW_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                        If Len(VIEW_DATE) > 12 Then
                            Label15.Text = " Input is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_ViewDate.Focus()
                            Exit Sub
                        End If
                    Else
                        VIEW_DATE = ""
                    End If
                Else
                    VIEW_DATE = ""
                End If


                'validation for  THEME
                Dim THEME As Object = Nothing
                If TR_THEME.Visible = True Then
                    If txt_Hold_Theme.Text <> "" Then
                        THEME = TrimAll(txt_Hold_Theme.Text)
                        THEME = RemoveQuotes(THEME)
                        If Len(THEME) > 150 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_Theme.Focus()
                            Exit Sub
                        End If
                    Else
                        THEME = ""
                    End If
                Else
                    THEME = ""
                End If

                'validation for  STYLE
                Dim STYLE As Object = Nothing
                If TR_STYLE.Visible = True Then
                    If txt_Hold_Style.Text <> "" Then
                        STYLE = TrimAll(txt_Hold_Style.Text)
                        STYLE = RemoveQuotes(STYLE)
                        If Len(STYLE) > 150 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_Style.Focus()
                            Exit Sub
                        End If
                    Else
                        STYLE = ""
                    End If
                Else
                    STYLE = ""
                End If

                'validation for  CULTURE
                Dim CULTURE As Object = Nothing
                If TR_CULTURE.Visible = True Then
                    If txt_Hold_Culture.Text <> "" Then
                        CULTURE = TrimAll(txt_Hold_Culture.Text)
                        CULTURE = RemoveQuotes(CULTURE)
                        If Len(CULTURE) > 150 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_Culture.Focus()
                            Exit Sub
                        End If
                    Else
                        CULTURE = ""
                    End If
                Else
                    CULTURE = ""
                End If

                'validation for  CURRENT_SITE
                Dim CURRENT_SITE As Object = Nothing
                If TR_CULTURE.Visible = True Then
                    If txt_Hold_CurrentSite.Text <> "" Then
                        CURRENT_SITE = TrimAll(txt_Hold_CurrentSite.Text)
                        CURRENT_SITE = RemoveQuotes(CURRENT_SITE)
                        If Len(CURRENT_SITE) > 150 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_CurrentSite.Focus()
                            Exit Sub
                        End If
                    Else
                        CURRENT_SITE = ""
                    End If
                Else
                    CURRENT_SITE = ""
                End If

                'validation for  CREATION_SITE
                Dim CREATION_SITE As Object = Nothing
                If TR_CREATION_SITE.Visible = True Then
                    If txt_Hold_CreationSite.Text <> "" Then
                        CREATION_SITE = TrimAll(txt_Hold_CreationSite.Text)
                        CREATION_SITE = RemoveQuotes(CREATION_SITE)
                        If Len(CREATION_SITE) > 150 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_CreationSite.Focus()
                            Exit Sub
                        End If
                    Else
                        CREATION_SITE = ""
                    End If
                Else
                    CREATION_SITE = ""
                End If

                'validation for  YARNCOUNT
                Dim YARNCOUNT As Object = Nothing
                If TR_YARNCOUNT.Visible = True Then
                    If txt_Hold_YarnCount.Text <> "" Then
                        YARNCOUNT = TrimAll(txt_Hold_YarnCount.Text)
                        YARNCOUNT = RemoveQuotes(YARNCOUNT)
                        If Len(YARNCOUNT) > 150 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_YarnCount.Focus()
                            Exit Sub
                        End If
                    Else
                        YARNCOUNT = ""
                    End If
                Else
                    YARNCOUNT = ""
                End If

                'validation for  MATERIAL_TYPE
                Dim MATERIAL_TYPE As Object = Nothing
                If TR_MATERIAL_TYPE.Visible = True Then
                    If txt_Hold_MaterialsType.Text <> "" Then
                        MATERIAL_TYPE = TrimAll(txt_Hold_MaterialsType.Text)
                        MATERIAL_TYPE = RemoveQuotes(MATERIAL_TYPE)
                        If Len(MATERIAL_TYPE) > 150 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_MaterialsType.Focus()
                            Exit Sub
                        End If
                    Else
                        MATERIAL_TYPE = ""
                    End If
                Else
                    MATERIAL_TYPE = ""
                End If

                'validation for  TECHNIQUE
                Dim TECHNIQUE As Object = Nothing
                If TR_TECHNIQUE.Visible = True Then
                    If txt_Hold_Technique.Text <> "" Then
                        TECHNIQUE = TrimAll(txt_Hold_Technique.Text)
                        TECHNIQUE = RemoveQuotes(TECHNIQUE)
                        If Len(TECHNIQUE) > 150 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_Technique.Focus()
                            Exit Sub
                        End If
                    Else
                        TECHNIQUE = ""
                    End If
                Else
                    TECHNIQUE = ""
                End If

                'validation for  TECH_DETAILS
                Dim TECH_DETAILS As Object = Nothing
                If TR_TECH_DETAILS.Visible = True Then
                    If txt_Hold_TechDetails.Text <> "" Then
                        TECH_DETAILS = TrimAll(txt_Hold_TechDetails.Text)
                        TECH_DETAILS = RemoveQuotes(TECH_DETAILS)
                        If Len(TECH_DETAILS) > 250 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_TechDetails.Focus()
                            Exit Sub
                        End If
                    Else
                        TECH_DETAILS = ""
                    End If
                Else
                    TECH_DETAILS = ""
                End If

                'validation for  INSCRIPTIONS
                Dim INSCRIPTIONS As Object = Nothing
                If TR_INSCRIPTIONS.Visible = True Then
                    If txt_Hold_Inscription.Text <> "" Then
                        INSCRIPTIONS = TrimAll(txt_Hold_Inscription.Text)
                        INSCRIPTIONS = RemoveQuotes(INSCRIPTIONS)
                        If Len(INSCRIPTIONS) > 150 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_Inscription.Focus()
                            Exit Sub
                        End If
                    Else
                        INSCRIPTIONS = ""
                    End If
                Else
                    INSCRIPTIONS = ""
                End If

                'validation for  DESCRIPTION
                Dim DESCRIPTION As Object = Nothing
                If TR_DESCRIPTION.Visible = True Then
                    If txt_Hold_Description.Text <> "" Then
                        DESCRIPTION = TrimAll(txt_Hold_Description.Text)
                        DESCRIPTION = RemoveQuotes(DESCRIPTION)
                        If Len(DESCRIPTION) > 250 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_Description.Focus()
                            Exit Sub
                        End If
                    Else
                        DESCRIPTION = ""
                    End If
                Else
                    DESCRIPTION = ""
                End If

                'validation for  GLOBE_TYPE
                Dim GLOBE_TYPE As Object = Nothing
                If TR_GLOBE_TYPE.Visible = True Then
                    If txt_Hold_GlobeType.Text <> "" Then
                        GLOBE_TYPE = TrimAll(txt_Hold_GlobeType.Text)
                        GLOBE_TYPE = RemoveQuotes(GLOBE_TYPE)
                        If Len(GLOBE_TYPE) > 50 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_GlobeType.Focus()
                            Exit Sub
                        End If
                    Else
                        GLOBE_TYPE = ""
                    End If
                Else
                    GLOBE_TYPE = ""
                End If

                'search ALTER_DATE
                Dim ALTER_DATE As Object = Nothing
                If TR_ALTER_DATE.Visible = True Then
                    If txt_Hold_AlterDate.Text <> "" Then
                        ALTER_DATE = TrimX(txt_Hold_AlterDate.Text)
                        ALTER_DATE = RemoveQuotes(ALTER_DATE)
                        ALTER_DATE = Convert.ToDateTime(ALTER_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                        If Len(ALTER_DATE) > 12 Then
                            Label15.Text = " Input is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_AlterDate.Focus()
                            Exit Sub
                        End If
                    Else
                        ALTER_DATE = ""
                    End If
                Else
                    ALTER_DATE = ""
                End If







                Dim USER_CODE As Object = Nothing
                USER_CODE = Session.Item("LoggedUser")

                Dim DATE_ADDED As Object = Nothing
                DATE_ADDED = Now.Date
                DATE_ADDED = Convert.ToDateTime(DATE_ADDED, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                Dim IP As Object = Nothing
                IP = Request.UserHostAddress.Trim

                '*******************************************************************************************

                '*******************************************************************************************
                'thisTransaction = SqlConn.BeginTransaction()
                If Me.CB_RecvAll.Checked = False Then
                    If SqlConn.State = 0 Then
                        SqlConn.Open()
                    End If
                    thisTransaction = SqlConn.BeginTransaction()

                    Dim intValue As Integer = 0
                    Dim objCommand As New SqlCommand
                    objCommand.Connection = SqlConn
                    objCommand.Transaction = thisTransaction
                    objCommand.CommandType = CommandType.Text
                    objCommand.CommandText = "INSERT INTO HOLDINGS (CAT_NO, ACQ_ID, ACCESSION_NO, ACCESSION_DATE, FORMAT_CODE, SHOW, ISSUEABLE, VOL_NO, VOL_YEAR, VOL_EDITORS, VOL_TITLE, COPY_ISBN, CLASS_NO, BOOK_NO, PAGINATION, SIZE, ILLUSTRATION, COLLECTION_TYPE, STA_CODE, BIND_CODE, SEC_CODE, LIB_CODE, ACC_MAT_CODE, REMARKS, PHYSICAL_LOCATION, REFERENCE_NO, MEDIUM, RECORDING_CATEGORY, RECORDING_FORM, RECORDING_FORMAT, RECORDING_SPEED, RECORDING_STORAGE_TECH, RECORDING_PLAY_DURATION, VIDEO_TYPEOFVISUAL, CARTOGRAPHIC_SCALE, CARTOGRAPHIC_PROJECTION, CARTOGRAPHIC_COORDINATES, CARTOGRAPHIC_GEOGRAPHIC_LOCATION, CARTOGRAPHIC_MEDIUM, CARTOGRAPHIC_DATAGATHERING_DATE, CREATION_DATE, CARTOGRAPHIC_COMPILATION_DATE, CARTOGRAPHIC_INSPECTION_DATE, DATE_ADDED, USER_CODE, IP,VIDEO_COLOR,PLAYBACK_CHANNELS,TAPE_WIDTH,TAPE_CONFIGURATION,KIND_OF_DISK,KIND_OF_CUTTING,ENCODING_STANDARD,CAPTURE_TECHNIQUE,PHOTO_NO,PHOTO_ALBUM_NO,PHOTO_OCASION,IMAGE_VIEW_TYPE,VIEW_DATE,THEME,STYLE,CULTURE,CURRENT_SITE,CREATION_SITE,YARNCOUNT,MATERIAL_TYPE,TECHNIQUE,TECH_DETAILS,INSCRIPTIONS,DESCRIPTION,GLOBE_TYPE,ALTER_DATE ) " & _
                                             " VALUES (@CAT_NO, @ACQ_ID, @ACCESSION_NO, @ACCESSION_DATE, @FORMAT_CODE, @SHOW, @ISSUABLE, @VOL_NO, @VOL_YEAR, @VOL_EDITORS, @VOL_TITLE, @COPY_ISBN, @CLASS_NO, @BOOK_NO, @PAGINATION, @SIZE, @ILLUSTRATION, @COLLECTION_TYPE, @STA_CODE, @BIND_CODE, @SEC_CODE, @LIB_CODE, @ACC_MAT_CODE, @REMARKS, @PHYSICAL_LOCATION, @REFERENCE_NO, @RECORDING_MEDIUM, @RECORDING_CATEGORY, @RECORDING_FORM, @RECORDING_FORMAT, @RECORDING_SPEED, @RECORDING_STORAGE_TECH, @RECORDING_PLAY_DURATION, @VIDEO_TYPEOFVISUALS, @CARTOGRAPHIC_SCALE, @CARTOGRAPHIC_PROJECTION, @CARTOGRAPHIC_COORDINATES, @CARTOGRAPHIC_GEOGRAPHIC_LOCATION, @CARTOGRAPHIC_MEDIUM, @CARTOGRAPHIC_DATAGATHERING_DATE, @CARTOGRAPHIC_CREATION_DATE, @CARTOGRAPHIC_COMPILATION_DATE, @CARTOGRAPHIC_INSPECTION_DATE, @DATE_ADDED, @USER_CODE, @IP,@VIDEO_COLOR,@PLAYBACK_CHANNELS,@TAPE_WIDTH,@TAPE_CONFIGURATION,@KIND_OF_DISK,@KIND_OF_CUTTING,@ENCODING_STANDARD,@CAPTURE_TECHNIQUE,@PHOTO_NO,@PHOTO_ALBUM_NO,@PHOTO_OCASION,@IMAGE_VIEW_TYPE,@VIEW_DATE,@THEME,@STYLE,@CULTURE,@CURRENT_SITE,@CREATION_SITE,@YARNCOUNT,@MATERIAL_TYPE,@TECHNIQUE,@TECH_DETAILS,@INSCRIPTIONS,@DESCRIPTION,@GLOBE_TYPE,@ALTER_DATE);  " & _
                                             " SELECT SCOPE_IDENTITY()"

                    objCommand.Parameters.Add("@CAT_NO", SqlDbType.Int)
                    objCommand.Parameters("@CAT_NO").Value = CAT_NO

                    objCommand.Parameters.Add("@ACQ_ID", SqlDbType.Int)
                    objCommand.Parameters("@ACQ_ID").Value = ACQ_ID

                    If ACCESSION_NO = "" Then ACCESSION_NO = System.DBNull.Value
                    objCommand.Parameters.Add("@ACCESSION_NO", SqlDbType.NVarChar)
                    objCommand.Parameters("@ACCESSION_NO").Value = ACCESSION_NO

                    If ACCESSION_DATE = "" Then ACCESSION_DATE = System.DBNull.Value
                    objCommand.Parameters.Add("@ACCESSION_DATE", SqlDbType.DateTime)
                    objCommand.Parameters("@ACCESSION_DATE").Value = ACCESSION_DATE

                    If FORMAT_CODE = "" Then FORMAT_CODE = "PT"
                    objCommand.Parameters.Add("@FORMAT_CODE", SqlDbType.VarChar)
                    objCommand.Parameters("@FORMAT_CODE").Value = FORMAT_CODE

                    If SHOW = "" Then SHOW = "Y"
                    objCommand.Parameters.Add("@SHOW", SqlDbType.VarChar)
                    objCommand.Parameters("@SHOW").Value = SHOW

                    If ISSUABLE = "" Then ISSUABLE = "Y"
                    objCommand.Parameters.Add("@ISSUABLE", SqlDbType.VarChar)
                    objCommand.Parameters("@ISSUABLE").Value = ISSUABLE

                    If VOL_NO = "" Then VOL_NO = System.DBNull.Value
                    objCommand.Parameters.Add("@VOL_NO", SqlDbType.NVarChar)
                    objCommand.Parameters("@VOL_NO").Value = VOL_NO

                    objCommand.Parameters.Add("@VOL_YEAR", SqlDbType.Int)
                    If VOL_YEAR = 0 Then
                        objCommand.Parameters("@VOL_YEAR").Value = System.DBNull.Value
                    Else
                        objCommand.Parameters("@VOL_YEAR").Value = VOL_YEAR
                    End If

                    If VOL_EDITORS = "" Then VOL_EDITORS = System.DBNull.Value
                    objCommand.Parameters.Add("@VOL_EDITORS", SqlDbType.NVarChar)
                    objCommand.Parameters("@VOL_EDITORS").Value = VOL_EDITORS

                    If VOL_TITLE = "" Then VOL_TITLE = System.DBNull.Value
                    objCommand.Parameters.Add("@VOL_TITLE", SqlDbType.NVarChar)
                    objCommand.Parameters("@VOL_TITLE").Value = VOL_TITLE

                    If COPY_ISBN = "" Then COPY_ISBN = System.DBNull.Value
                    objCommand.Parameters.Add("@COPY_ISBN", SqlDbType.VarChar)
                    objCommand.Parameters("@COPY_ISBN").Value = COPY_ISBN

                    If CLASS_NO = "" Then CLASS_NO = System.DBNull.Value
                    objCommand.Parameters.Add("@CLASS_NO", SqlDbType.NVarChar)
                    objCommand.Parameters("@CLASS_NO").Value = CLASS_NO

                    If BOOK_NO = "" Then BOOK_NO = System.DBNull.Value
                    objCommand.Parameters.Add("@BOOK_NO", SqlDbType.NVarChar)
                    objCommand.Parameters("@BOOK_NO").Value = BOOK_NO

                    If PAGINATION = "" Then PAGINATION = System.DBNull.Value
                    objCommand.Parameters.Add("@PAGINATION", SqlDbType.NVarChar)
                    objCommand.Parameters("@PAGINATION").Value = PAGINATION

                    If SIZE = "" Then SIZE = System.DBNull.Value
                    objCommand.Parameters.Add("@SIZE", SqlDbType.VarChar)
                    objCommand.Parameters("@SIZE").Value = SIZE

                    If ILLUSTRATION = "" Then ILLUSTRATION = System.DBNull.Value
                    objCommand.Parameters.Add("@ILLUSTRATION", SqlDbType.VarChar)
                    objCommand.Parameters("@ILLUSTRATION").Value = ILLUSTRATION

                    If COLLECTION_TYPE = "" Then COLLECTION_TYPE = System.DBNull.Value
                    objCommand.Parameters.Add("@COLLECTION_TYPE", SqlDbType.VarChar)
                    objCommand.Parameters("@COLLECTION_TYPE").Value = COLLECTION_TYPE

                    If STA_CODE = "" Then STA_CODE = "1"
                    objCommand.Parameters.Add("@STA_CODE", SqlDbType.VarChar)
                    objCommand.Parameters("@STA_CODE").Value = STA_CODE

                    If BIND_CODE = "" Then BIND_CODE = System.DBNull.Value
                    objCommand.Parameters.Add("@BIND_CODE", SqlDbType.VarChar)
                    objCommand.Parameters("@BIND_CODE").Value = BIND_CODE

                    If SEC_CODE = "" Then SEC_CODE = System.DBNull.Value
                    objCommand.Parameters.Add("@SEC_CODE", SqlDbType.VarChar)
                    objCommand.Parameters("@SEC_CODE").Value = SEC_CODE

                    If HOLD_LIB_CODE = "" Then HOLD_LIB_CODE = System.DBNull.Value
                    objCommand.Parameters.Add("@LIB_CODE", SqlDbType.NVarChar)
                    objCommand.Parameters("@LIB_CODE").Value = HOLD_LIB_CODE

                    If ACC_MAT_CODE = "" Then ACC_MAT_CODE = System.DBNull.Value
                    objCommand.Parameters.Add("@ACC_MAT_CODE", SqlDbType.VarChar)
                    objCommand.Parameters("@ACC_MAT_CODE").Value = ACC_MAT_CODE

                    If REMARKS = "" Then REMARKS = System.DBNull.Value
                    objCommand.Parameters.Add("@REMARKS", SqlDbType.NVarChar)
                    objCommand.Parameters("@REMARKS").Value = REMARKS

                    If PHYSICAL_LOCATION = "" Then PHYSICAL_LOCATION = System.DBNull.Value
                    objCommand.Parameters.Add("@PHYSICAL_LOCATION", SqlDbType.NVarChar)
                    objCommand.Parameters("@PHYSICAL_LOCATION").Value = PHYSICAL_LOCATION

                    If REFERENCE_NO = "" Then REFERENCE_NO = System.DBNull.Value
                    objCommand.Parameters.Add("@REFERENCE_NO", SqlDbType.NVarChar)
                    objCommand.Parameters("@REFERENCE_NO").Value = REFERENCE_NO

                    If RECORDING_MEDIUM = "" Then RECORDING_MEDIUM = System.DBNull.Value
                    objCommand.Parameters.Add("@RECORDING_MEDIUM", SqlDbType.NVarChar)
                    objCommand.Parameters("@RECORDING_MEDIUM").Value = RECORDING_MEDIUM

                    If RECORDING_CATEGORY = "" Then RECORDING_CATEGORY = System.DBNull.Value
                    objCommand.Parameters.Add("@RECORDING_CATEGORY", SqlDbType.NVarChar)
                    objCommand.Parameters("@RECORDING_CATEGORY").Value = RECORDING_CATEGORY

                    If RECORDING_FORM = "" Then RECORDING_FORM = System.DBNull.Value
                    objCommand.Parameters.Add("@RECORDING_FORM", SqlDbType.NVarChar)
                    objCommand.Parameters("@RECORDING_FORM").Value = RECORDING_FORM

                    If RECORDING_FORMAT = "" Then RECORDING_FORMAT = System.DBNull.Value
                    objCommand.Parameters.Add("@RECORDING_FORMAT", SqlDbType.NVarChar)
                    objCommand.Parameters("@RECORDING_FORMAT").Value = RECORDING_FORMAT

                    If RECORDING_SPEED = "" Then RECORDING_SPEED = System.DBNull.Value
                    objCommand.Parameters.Add("@RECORDING_SPEED", SqlDbType.NVarChar)
                    objCommand.Parameters("@RECORDING_SPEED").Value = RECORDING_SPEED

                    If RECORDING_STORAGE_TECH = "" Then RECORDING_STORAGE_TECH = System.DBNull.Value
                    objCommand.Parameters.Add("@RECORDING_STORAGE_TECH", SqlDbType.NVarChar)
                    objCommand.Parameters("@RECORDING_STORAGE_TECH").Value = RECORDING_STORAGE_TECH

                    If RECORDING_PLAY_DURATION = "" Then RECORDING_PLAY_DURATION = System.DBNull.Value
                    objCommand.Parameters.Add("@RECORDING_PLAY_DURATION", SqlDbType.NVarChar)
                    objCommand.Parameters("@RECORDING_PLAY_DURATION").Value = RECORDING_PLAY_DURATION

                    If VIDEO_TYPEOFVISUALS = "" Then VIDEO_TYPEOFVISUALS = System.DBNull.Value
                    objCommand.Parameters.Add("@VIDEO_TYPEOFVISUALS", SqlDbType.NVarChar)
                    objCommand.Parameters("@VIDEO_TYPEOFVISUALS").Value = VIDEO_TYPEOFVISUALS

                    If CARTOGRAPHIC_SCALE = "" Then CARTOGRAPHIC_SCALE = System.DBNull.Value
                    objCommand.Parameters.Add("@CARTOGRAPHIC_SCALE", SqlDbType.NVarChar)
                    objCommand.Parameters("@CARTOGRAPHIC_SCALE").Value = CARTOGRAPHIC_SCALE

                    If CARTOGRAPHIC_PROJECTION = "" Then CARTOGRAPHIC_PROJECTION = System.DBNull.Value
                    objCommand.Parameters.Add("@CARTOGRAPHIC_PROJECTION", SqlDbType.NVarChar)
                    objCommand.Parameters("@CARTOGRAPHIC_PROJECTION").Value = CARTOGRAPHIC_PROJECTION

                    If CARTOGRAPHIC_COORDINATES = "" Then CARTOGRAPHIC_COORDINATES = System.DBNull.Value
                    objCommand.Parameters.Add("@CARTOGRAPHIC_COORDINATES", SqlDbType.NVarChar)
                    objCommand.Parameters("@CARTOGRAPHIC_COORDINATES").Value = CARTOGRAPHIC_COORDINATES

                    If CARTOGRAPHIC_GEOGRAPHIC_LOCATION = "" Then CARTOGRAPHIC_GEOGRAPHIC_LOCATION = System.DBNull.Value
                    objCommand.Parameters.Add("@CARTOGRAPHIC_GEOGRAPHIC_LOCATION", SqlDbType.NVarChar)
                    objCommand.Parameters("@CARTOGRAPHIC_GEOGRAPHIC_LOCATION").Value = CARTOGRAPHIC_GEOGRAPHIC_LOCATION

                    If CARTOGRAPHIC_MEDIUM = "" Then CARTOGRAPHIC_MEDIUM = System.DBNull.Value
                    objCommand.Parameters.Add("@CARTOGRAPHIC_MEDIUM", SqlDbType.NVarChar)
                    objCommand.Parameters("@CARTOGRAPHIC_MEDIUM").Value = CARTOGRAPHIC_MEDIUM

                    If CARTOGRAPHIC_DATAGATHERING_DATE = "" Then CARTOGRAPHIC_DATAGATHERING_DATE = System.DBNull.Value
                    objCommand.Parameters.Add("@CARTOGRAPHIC_DATAGATHERING_DATE", SqlDbType.DateTime)
                    objCommand.Parameters("@CARTOGRAPHIC_DATAGATHERING_DATE").Value = CARTOGRAPHIC_DATAGATHERING_DATE

                    If CARTOGRAPHIC_CREATION_DATE = "" Then CARTOGRAPHIC_CREATION_DATE = System.DBNull.Value
                    objCommand.Parameters.Add("@CARTOGRAPHIC_CREATION_DATE", SqlDbType.DateTime)
                    objCommand.Parameters("@CARTOGRAPHIC_CREATION_DATE").Value = CARTOGRAPHIC_CREATION_DATE

                    If CARTOGRAPHIC_COMPILATION_DATE = "" Then CARTOGRAPHIC_COMPILATION_DATE = System.DBNull.Value
                    objCommand.Parameters.Add("@CARTOGRAPHIC_COMPILATION_DATE", SqlDbType.DateTime)
                    objCommand.Parameters("@CARTOGRAPHIC_COMPILATION_DATE").Value = CARTOGRAPHIC_COMPILATION_DATE

                    If CARTOGRAPHIC_INSPECTION_DATE = "" Then CARTOGRAPHIC_INSPECTION_DATE = System.DBNull.Value
                    objCommand.Parameters.Add("@CARTOGRAPHIC_INSPECTION_DATE", SqlDbType.DateTime)
                    objCommand.Parameters("@CARTOGRAPHIC_INSPECTION_DATE").Value = CARTOGRAPHIC_INSPECTION_DATE


                    If VIDEO_COLOR = "" Then VIDEO_COLOR = System.DBNull.Value
                    objCommand.Parameters.Add("@VIDEO_COLOR", SqlDbType.NVarChar)
                    objCommand.Parameters("@VIDEO_COLOR").Value = VIDEO_COLOR

                    If PLAYBACK_CHANNELS = "" Then PLAYBACK_CHANNELS = System.DBNull.Value
                    objCommand.Parameters.Add("@PLAYBACK_CHANNELS", SqlDbType.NVarChar)
                    objCommand.Parameters("@PLAYBACK_CHANNELS").Value = PLAYBACK_CHANNELS

                    If TAPE_WIDTH = "" Then TAPE_WIDTH = System.DBNull.Value
                    objCommand.Parameters.Add("@TAPE_WIDTH", SqlDbType.NVarChar)
                    objCommand.Parameters("@TAPE_WIDTH").Value = TAPE_WIDTH

                    If TAPE_CONFIGURATION = "" Then TAPE_CONFIGURATION = System.DBNull.Value
                    objCommand.Parameters.Add("@TAPE_CONFIGURATION", SqlDbType.NVarChar)
                    objCommand.Parameters("@TAPE_CONFIGURATION").Value = TAPE_CONFIGURATION

                    If KIND_OF_DISK = "" Then KIND_OF_DISK = System.DBNull.Value
                    objCommand.Parameters.Add("@KIND_OF_DISK", SqlDbType.NVarChar)
                    objCommand.Parameters("@KIND_OF_DISK").Value = KIND_OF_DISK

                    If KIND_OF_CUTTING = "" Then KIND_OF_CUTTING = System.DBNull.Value
                    objCommand.Parameters.Add("@KIND_OF_CUTTING", SqlDbType.NVarChar)
                    objCommand.Parameters("@KIND_OF_CUTTING").Value = KIND_OF_CUTTING

                    If ENCODING_STANDARD = "" Then ENCODING_STANDARD = System.DBNull.Value
                    objCommand.Parameters.Add("@ENCODING_STANDARD", SqlDbType.NVarChar)
                    objCommand.Parameters("@ENCODING_STANDARD").Value = ENCODING_STANDARD

                    If CAPTURE_TECHNIQUE = "" Then CAPTURE_TECHNIQUE = System.DBNull.Value
                    objCommand.Parameters.Add("@CAPTURE_TECHNIQUE", SqlDbType.NVarChar)
                    objCommand.Parameters("@CAPTURE_TECHNIQUE").Value = CAPTURE_TECHNIQUE

                    If PHOTO_NO = "" Then PHOTO_NO = System.DBNull.Value
                    objCommand.Parameters.Add("@PHOTO_NO", SqlDbType.NVarChar)
                    objCommand.Parameters("@PHOTO_NO").Value = PHOTO_NO

                    If PHOTO_ALBUM_NO = "" Then PHOTO_ALBUM_NO = System.DBNull.Value
                    objCommand.Parameters.Add("@PHOTO_ALBUM_NO", SqlDbType.NVarChar)
                    objCommand.Parameters("@PHOTO_ALBUM_NO").Value = PHOTO_ALBUM_NO

                    If PHOTO_OCASION = "" Then PHOTO_OCASION = System.DBNull.Value
                    objCommand.Parameters.Add("@PHOTO_OCASION", SqlDbType.NVarChar)
                    objCommand.Parameters("@PHOTO_OCASION").Value = PHOTO_OCASION

                    If IMAGE_VIEW_TYPE = "" Then IMAGE_VIEW_TYPE = System.DBNull.Value
                    objCommand.Parameters.Add("@IMAGE_VIEW_TYPE", SqlDbType.NVarChar)
                    objCommand.Parameters("@IMAGE_VIEW_TYPE").Value = IMAGE_VIEW_TYPE

                    If THEME = "" Then THEME = System.DBNull.Value
                    objCommand.Parameters.Add("@THEME", SqlDbType.NVarChar)
                    objCommand.Parameters("@THEME").Value = THEME

                    If STYLE = "" Then STYLE = System.DBNull.Value
                    objCommand.Parameters.Add("@STYLE", SqlDbType.NVarChar)
                    objCommand.Parameters("@STYLE").Value = STYLE

                    If CULTURE = "" Then CULTURE = System.DBNull.Value
                    objCommand.Parameters.Add("@CULTURE", SqlDbType.NVarChar)
                    objCommand.Parameters("@CULTURE").Value = CULTURE

                    If CURRENT_SITE = "" Then CURRENT_SITE = System.DBNull.Value
                    objCommand.Parameters.Add("@CURRENT_SITE", SqlDbType.NVarChar)
                    objCommand.Parameters("@CURRENT_SITE").Value = CURRENT_SITE

                    If CREATION_SITE = "" Then CREATION_SITE = System.DBNull.Value
                    objCommand.Parameters.Add("@CREATION_SITE", SqlDbType.NVarChar)
                    objCommand.Parameters("@CREATION_SITE").Value = CREATION_SITE

                    If YARNCOUNT = "" Then YARNCOUNT = System.DBNull.Value
                    objCommand.Parameters.Add("@YARNCOUNT", SqlDbType.NVarChar)
                    objCommand.Parameters("@YARNCOUNT").Value = YARNCOUNT

                    If MATERIAL_TYPE = "" Then MATERIAL_TYPE = System.DBNull.Value
                    objCommand.Parameters.Add("@MATERIAL_TYPE", SqlDbType.NVarChar)
                    objCommand.Parameters("@MATERIAL_TYPE").Value = MATERIAL_TYPE

                    If TECHNIQUE = "" Then TECHNIQUE = System.DBNull.Value
                    objCommand.Parameters.Add("@TECHNIQUE", SqlDbType.NVarChar)
                    objCommand.Parameters("@TECHNIQUE").Value = TECHNIQUE

                    If TECH_DETAILS = "" Then TECH_DETAILS = System.DBNull.Value
                    objCommand.Parameters.Add("@TECH_DETAILS", SqlDbType.NVarChar)
                    objCommand.Parameters("@TECH_DETAILS").Value = TECH_DETAILS

                    If INSCRIPTIONS = "" Then INSCRIPTIONS = System.DBNull.Value
                    objCommand.Parameters.Add("@INSCRIPTIONS", SqlDbType.NVarChar)
                    objCommand.Parameters("@INSCRIPTIONS").Value = INSCRIPTIONS

                    If DESCRIPTION = "" Then DESCRIPTION = System.DBNull.Value
                    objCommand.Parameters.Add("@DESCRIPTION", SqlDbType.NVarChar)
                    objCommand.Parameters("@DESCRIPTION").Value = DESCRIPTION

                    If GLOBE_TYPE = "" Then GLOBE_TYPE = System.DBNull.Value
                    objCommand.Parameters.Add("@GLOBE_TYPE", SqlDbType.NVarChar)
                    objCommand.Parameters("@GLOBE_TYPE").Value = GLOBE_TYPE

                    If ALTER_DATE = "" Then ALTER_DATE = System.DBNull.Value
                    objCommand.Parameters.Add("@ALTER_DATE", SqlDbType.DateTime)
                    objCommand.Parameters("@ALTER_DATE").Value = ALTER_DATE

                    If VIEW_DATE = "" Then VIEW_DATE = System.DBNull.Value
                    objCommand.Parameters.Add("@VIEW_DATE", SqlDbType.DateTime)
                    objCommand.Parameters("@VIEW_DATE").Value = VIEW_DATE

                    If DATE_ADDED = "" Then DATE_ADDED = System.DBNull.Value
                    objCommand.Parameters.Add("@DATE_ADDED", SqlDbType.DateTime)
                    objCommand.Parameters("@DATE_ADDED").Value = DATE_ADDED

                    If USER_CODE = "" Then USER_CODE = System.DBNull.Value
                    objCommand.Parameters.Add("@USER_CODE", SqlDbType.NVarChar)
                    objCommand.Parameters("@USER_CODE").Value = USER_CODE

                    If IP = "" Then IP = System.DBNull.Value
                    objCommand.Parameters.Add("@IP", SqlDbType.VarChar)
                    objCommand.Parameters("@IP").Value = IP

                    Dim dr As SqlDataReader
                    dr = objCommand.ExecuteReader()
                    If dr.Read Then
                        intValue = dr.GetValue(0)
                    End If
                    dr.Close()

                    'count copies from holdings
                    If intValue <> 0 Then
                        Dim objCommand2 As New SqlCommand
                        objCommand2.Connection = SqlConn
                        objCommand2.Transaction = thisTransaction
                        objCommand2.CommandType = CommandType.Text
                        objCommand2.CommandText = "UPDATE ACQUISITIONS SET COPY_ACCESSIONED =(SELECT COUNT(*) FROM HOLDINGS WHERE ACQ_ID = @ACQ_ID) WHERE (ACQ_ID = @ACQ_ID)"

                        objCommand2.Parameters.Add("@ACQ_ID", SqlDbType.Int)
                        objCommand2.Parameters("@ACQ_ID").Value = ACQ_ID

                        Dim dr2 As SqlDataReader
                        dr2 = objCommand2.ExecuteReader()
                        dr2.Close()
                    End If
                    If intValue <> 0 Then
                        Dim objCommand3 As New SqlCommand
                        objCommand3.Connection = SqlConn
                        objCommand3.Transaction = thisTransaction
                        objCommand3.CommandType = CommandType.Text
                        objCommand3.CommandText = "UPDATE ACQUISITIONS SET PROCESS_STATUS =(CASE WHEN COPY_RECEIVED=COPY_ACCESSIONED THEN 'Accessioned' ELSE 'Partially Accessioned' END) WHERE (ACQ_ID = @ACQ_ID)"

                        objCommand3.Parameters.Add("@ACQ_ID", SqlDbType.Int)
                        objCommand3.Parameters("@ACQ_ID").Value = ACQ_ID

                        Dim dr3 As SqlDataReader
                        dr3 = objCommand3.ExecuteReader()
                        dr3.Close()
                    End If

                    If intValue <> 0 Then
                        Dim objCommand4 As New SqlCommand
                        objCommand4.Connection = SqlConn
                        objCommand4.Transaction = thisTransaction
                        objCommand4.CommandType = CommandType.Text
                        objCommand4.CommandText = "UPDATE CATS SET CAT_LEVEL ='Full' WHERE (CAT_NO = @CAT_NO)"

                        objCommand4.Parameters.Add("@CAT_NO", SqlDbType.Int)
                        objCommand4.Parameters("@CAT_NO").Value = CAT_NO

                        Dim dr4 As SqlDataReader
                        dr4 = objCommand4.ExecuteReader()
                        dr4.Close()
                    End If
                    Label1.Text = "Record Added Successfully! " & "HOLD ID: " & intValue.ToString
                    Label15.Text = ""

                    thisTransaction.Commit()
                    SqlConn.Close()
                Else
                    Dim objr As StreamWriter
                    Dim fs1 As New FileStream("D:\Library\Test.txt", FileMode.Create, FileAccess.Write)
                    Dim s1 As New StreamWriter(fs1)
                    fs1.Close()
                    fs1 = Nothing
                    s1 = Nothing
                    Dim line As Object = Nothing

                    objr = New StreamWriter("D:\Library\Test.txt", True)

                    Dim myAccNO As Integer = 0
                    myAccNO = TrimX(txt_Hold_AccNo.Text)
                    Dim x As Integer = 0
                    For x = 0 To CopiesRecd - 1
                        'validation for acq_id check process status
                        Dim str As Object = Nothing
                        Dim flag As Object = Nothing
                        str = "SELECT ACQ_ID FROM ACQUISITIONS WHERE (ACQ_ID = '" & Trim(ACQ_ID) & "')  AND (PROCESS_STATUS = 'Accessioned')"
                        Dim cmd1 As New SqlCommand(str, SqlConn)
                        SqlConn.Open()
                        flag = cmd1.ExecuteScalar
                        If flag <> Nothing Then
                            Label15.Text = "All Received Copy(ies) has/have been Accessioned, Can Not Add More Copy ! "
                            Label1.Text = ""
                            SqlConn.Close()
                            Exit For
                        End If
                        SqlConn.Close()

                        Dim myNextAccNo As Object = Nothing
                        If x = 0 Then
                            myNextAccNo = ACCESSION_SERIES + Convert.ToString(myAccNO) 'TrimX(txt_Hold_AccNo.Text)
                        Else
                            myAccNO = myAccNO + 1
                            myNextAccNo = ACCESSION_SERIES + Convert.ToString(myAccNO) ' + 1)
                        End If

                        'check duplicate isbn
                        Dim str2 As Object = Nothing
                        Dim flag2 As Object = Nothing
                        str2 = "SELECT HOLD_ID FROM HOLDINGS WHERE (ACCESSION_NO = '" & Trim(myNextAccNo) & "')  AND (LIB_CODE = '" & Trim(DDL_Library.SelectedValue) & "')"
                        Dim cmd2 As New SqlCommand(str2, SqlConn)
                        SqlConn.Open()
                        flag2 = cmd2.ExecuteScalar
                        SqlConn.Close()
                        If flag2 <> Nothing Then
                            Label15.Text = "This Accession No Already Exists ! "
                            Label1.Text = ""
                            If x = 0 Then
                                myAccNO = myAccNO + 1
                            End If
                            x = x - 1
                        Else
                            line = line & myNextAccNo
                            If SqlConn.State = 0 Then
                                SqlConn.Open()
                            End If
                            thisTransaction = SqlConn.BeginTransaction()

                            Dim intValue As Integer = 0
                            Dim objCommand As New SqlCommand
                            objCommand.Connection = SqlConn
                            objCommand.Transaction = thisTransaction
                            objCommand.CommandType = CommandType.Text
                            objCommand.CommandText = "INSERT INTO HOLDINGS (CAT_NO, ACQ_ID, ACCESSION_NO, ACCESSION_DATE, FORMAT_CODE, SHOW, ISSUEABLE, VOL_NO, VOL_YEAR, VOL_EDITORS, VOL_TITLE, COPY_ISBN, CLASS_NO, BOOK_NO, PAGINATION, SIZE, ILLUSTRATION, COLLECTION_TYPE, STA_CODE, BIND_CODE, SEC_CODE, LIB_CODE, ACC_MAT_CODE, REMARKS, PHYSICAL_LOCATION, REFERENCE_NO, MEDIUM, RECORDING_CATEGORY, RECORDING_FORM, RECORDING_FORMAT, RECORDING_SPEED, RECORDING_STORAGE_TECH, RECORDING_PLAY_DURATION, VIDEO_TYPEOFVISUAL, CARTOGRAPHIC_SCALE, CARTOGRAPHIC_PROJECTION, CARTOGRAPHIC_COORDINATES, CARTOGRAPHIC_GEOGRAPHIC_LOCATION, CARTOGRAPHIC_MEDIUM, CARTOGRAPHIC_DATAGATHERING_DATE, CREATION_DATE, CARTOGRAPHIC_COMPILATION_DATE, CARTOGRAPHIC_INSPECTION_DATE, DATE_ADDED, USER_CODE, IP,VIDEO_COLOR,PLAYBACK_CHANNELS,TAPE_WIDTH,TAPE_CONFIGURATION,KIND_OF_DISK,KIND_OF_CUTTING,ENCODING_STANDARD,CAPTURE_TECHNIQUE,PHOTO_NO,PHOTO_ALBUM_NO,PHOTO_OCASION,IMAGE_VIEW_TYPE,VIEW_DATE,THEME,STYLE,CULTURE,CURRENT_SITE,CREATION_SITE,YARNCOUNT,MATERIAL_TYPE,TECHNIQUE,TECH_DETAILS,INSCRIPTIONS,DESCRIPTION,GLOBE_TYPE,ALTER_DATE ) " & _
                                                     " VALUES (@CAT_NO, @ACQ_ID, @ACCESSION_NO, @ACCESSION_DATE, @FORMAT_CODE, @SHOW, @ISSUABLE, @VOL_NO, @VOL_YEAR, @VOL_EDITORS, @VOL_TITLE, @COPY_ISBN, @CLASS_NO, @BOOK_NO, @PAGINATION, @SIZE, @ILLUSTRATION, @COLLECTION_TYPE, @STA_CODE, @BIND_CODE, @SEC_CODE, @LIB_CODE, @ACC_MAT_CODE, @REMARKS, @PHYSICAL_LOCATION, @REFERENCE_NO, @RECORDING_MEDIUM, @RECORDING_CATEGORY, @RECORDING_FORM, @RECORDING_FORMAT, @RECORDING_SPEED, @RECORDING_STORAGE_TECH, @RECORDING_PLAY_DURATION, @VIDEO_TYPEOFVISUALS, @CARTOGRAPHIC_SCALE, @CARTOGRAPHIC_PROJECTION, @CARTOGRAPHIC_COORDINATES, @CARTOGRAPHIC_GEOGRAPHIC_LOCATION, @CARTOGRAPHIC_MEDIUM, @CARTOGRAPHIC_DATAGATHERING_DATE, @CARTOGRAPHIC_CREATION_DATE, @CARTOGRAPHIC_COMPILATION_DATE, @CARTOGRAPHIC_INSPECTION_DATE, @DATE_ADDED, @USER_CODE, @IP,@VIDEO_COLOR,@PLAYBACK_CHANNELS,@TAPE_WIDTH,@TAPE_CONFIGURATION,@KIND_OF_DISK,@KIND_OF_CUTTING,@ENCODING_STANDARD,@CAPTURE_TECHNIQUE,@PHOTO_NO,@PHOTO_ALBUM_NO,@PHOTO_OCASION,@IMAGE_VIEW_TYPE,@VIEW_DATE,@THEME,@STYLE,@CULTURE,@CURRENT_SITE,@CREATION_SITE,@YARNCOUNT,@MATERIAL_TYPE,@TECHNIQUE,@TECH_DETAILS,@INSCRIPTIONS,@DESCRIPTION,@GLOBE_TYPE,@ALTER_DATE);  " & _
                                                     " SELECT SCOPE_IDENTITY()"

                            objCommand.Parameters.Add("@CAT_NO", SqlDbType.Int)
                            objCommand.Parameters("@CAT_NO").Value = CAT_NO

                            objCommand.Parameters.Add("@ACQ_ID", SqlDbType.Int)
                            objCommand.Parameters("@ACQ_ID").Value = ACQ_ID

                            If myNextAccNo = "" Then myNextAccNo = System.DBNull.Value
                            objCommand.Parameters.Add("@ACCESSION_NO", SqlDbType.NVarChar)
                            objCommand.Parameters("@ACCESSION_NO").Value = myNextAccNo

                            If ACCESSION_DATE = "" Then ACCESSION_DATE = System.DBNull.Value
                            objCommand.Parameters.Add("@ACCESSION_DATE", SqlDbType.DateTime)
                            objCommand.Parameters("@ACCESSION_DATE").Value = ACCESSION_DATE

                            objCommand.Parameters.Add("@FORMAT_CODE", SqlDbType.VarChar)
                            If FORMAT_CODE = "" Or FORMAT_CODE = Nothing Then
                                objCommand.Parameters("@FORMAT_CODE").Value = "PT"
                            Else
                                objCommand.Parameters("@FORMAT_CODE").Value = FORMAT_CODE
                            End If

                            objCommand.Parameters.Add("@SHOW", SqlDbType.VarChar)
                            If SHOW = "" Or SHOW = Nothing Then
                                objCommand.Parameters("@SHOW").Value = "Y"
                            Else
                                objCommand.Parameters("@SHOW").Value = SHOW
                            End If

                            objCommand.Parameters.Add("@ISSUABLE", SqlDbType.VarChar)
                            If ISSUABLE = "" Or ISSUABLE = Nothing Then
                                objCommand.Parameters("@ISSUABLE").Value = "Y"
                            Else
                                objCommand.Parameters("@ISSUABLE").Value = ISSUABLE
                            End If

                            objCommand.Parameters.Add("@VOL_NO", SqlDbType.NVarChar)
                            If VOL_NO = "" Or VOL_NO = Nothing Then
                                objCommand.Parameters("@VOL_NO").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@VOL_NO").Value = VOL_NO
                            End If

                            objCommand.Parameters.Add("@VOL_YEAR", SqlDbType.Int)
                            If VOL_YEAR = 0 Then
                                objCommand.Parameters("@VOL_YEAR").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@VOL_YEAR").Value = VOL_YEAR
                            End If

                            objCommand.Parameters.Add("@VOL_EDITORS", SqlDbType.NVarChar)
                            If VOL_EDITORS = "" Or VOL_EDITORS = Nothing Then
                                objCommand.Parameters("@VOL_EDITORS").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@VOL_EDITORS").Value = VOL_EDITORS
                            End If

                            objCommand.Parameters.Add("@VOL_TITLE", SqlDbType.NVarChar)
                            If VOL_TITLE = "" Or VOL_TITLE = Nothing Then
                                objCommand.Parameters("@VOL_TITLE").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@VOL_TITLE").Value = VOL_TITLE
                            End If

                            objCommand.Parameters.Add("@COPY_ISBN", SqlDbType.VarChar)
                            If COPY_ISBN = "" Or COPY_ISBN = Nothing Then
                                objCommand.Parameters("@COPY_ISBN").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@COPY_ISBN").Value = COPY_ISBN
                            End If

                            objCommand.Parameters.Add("@CLASS_NO", SqlDbType.NVarChar)
                            If CLASS_NO = "" Or CLASS_NO = Nothing Then
                                objCommand.Parameters("@CLASS_NO").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@CLASS_NO").Value = CLASS_NO
                            End If

                            objCommand.Parameters.Add("@BOOK_NO", SqlDbType.NVarChar)
                            If BOOK_NO = "" Or BOOK_NO = Nothing Then
                                objCommand.Parameters("@BOOK_NO").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@BOOK_NO").Value = BOOK_NO
                            End If

                            objCommand.Parameters.Add("@PAGINATION", SqlDbType.NVarChar)
                            If PAGINATION = "" Or PAGINATION = Nothing Then
                                objCommand.Parameters("@PAGINATION").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@PAGINATION").Value = PAGINATION
                            End If

                            objCommand.Parameters.Add("@SIZE", SqlDbType.VarChar)
                            If SIZE = "" Or SIZE = Nothing Then
                                objCommand.Parameters("@SIZE").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@SIZE").Value = SIZE
                            End If

                            objCommand.Parameters.Add("@ILLUSTRATION", SqlDbType.VarChar)
                            If ILLUSTRATION = "" Or ILLUSTRATION = Nothing Then
                                objCommand.Parameters("@ILLUSTRATION").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@ILLUSTRATION").Value = ILLUSTRATION
                            End If

                            objCommand.Parameters.Add("@COLLECTION_TYPE", SqlDbType.VarChar)
                            If COLLECTION_TYPE = "" Or COLLECTION_TYPE = Nothing Then
                                objCommand.Parameters("@COLLECTION_TYPE").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@COLLECTION_TYPE").Value = COLLECTION_TYPE
                            End If

                            objCommand.Parameters.Add("@STA_CODE", SqlDbType.VarChar)
                            If STA_CODE = "" Or STA_CODE = Nothing Then
                                objCommand.Parameters("@STA_CODE").Value = "1"
                            Else
                                objCommand.Parameters("@STA_CODE").Value = STA_CODE
                            End If

                            objCommand.Parameters.Add("@BIND_CODE", SqlDbType.VarChar)
                            If BIND_CODE = "" Or BIND_CODE = Nothing Then
                                objCommand.Parameters("@BIND_CODE").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@BIND_CODE").Value = BIND_CODE
                            End If

                            objCommand.Parameters.Add("@SEC_CODE", SqlDbType.VarChar)
                            If SEC_CODE = "" Or SEC_CODE = Nothing Then
                                objCommand.Parameters("@SEC_CODE").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@SEC_CODE").Value = SEC_CODE
                            End If

                            objCommand.Parameters.Add("@LIB_CODE", SqlDbType.NVarChar)
                            If HOLD_LIB_CODE = "" Or HOLD_LIB_CODE = Nothing Then
                                objCommand.Parameters("@LIB_CODE").Value = HOLD_LIB_CODE
                            Else
                                objCommand.Parameters("@LIB_CODE").Value = HOLD_LIB_CODE
                            End If

                            objCommand.Parameters.Add("@ACC_MAT_CODE", SqlDbType.VarChar)
                            If ACC_MAT_CODE = "" Or ACC_MAT_CODE = Nothing Then
                                objCommand.Parameters("@ACC_MAT_CODE").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@ACC_MAT_CODE").Value = ACC_MAT_CODE
                            End If

                            objCommand.Parameters.Add("@REMARKS", SqlDbType.NVarChar)
                            If REMARKS = "" Or REMARKS = Nothing Then
                                objCommand.Parameters("@REMARKS").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@REMARKS").Value = REMARKS
                            End If

                            objCommand.Parameters.Add("@PHYSICAL_LOCATION", SqlDbType.NVarChar)
                            If PHYSICAL_LOCATION = "" Or PHYSICAL_LOCATION = Nothing Then
                                objCommand.Parameters("@PHYSICAL_LOCATION").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@PHYSICAL_LOCATION").Value = PHYSICAL_LOCATION
                            End If

                            objCommand.Parameters.Add("@REFERENCE_NO", SqlDbType.NVarChar)
                            If REFERENCE_NO = "" Or REFERENCE_NO = Nothing Then
                                objCommand.Parameters("@REFERENCE_NO").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@REFERENCE_NO").Value = REFERENCE_NO
                            End If

                            objCommand.Parameters.Add("@RECORDING_MEDIUM", SqlDbType.NVarChar)
                            If RECORDING_MEDIUM = "" Or RECORDING_MEDIUM = Nothing Then
                                objCommand.Parameters("@RECORDING_MEDIUM").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@RECORDING_MEDIUM").Value = RECORDING_MEDIUM
                            End If

                            objCommand.Parameters.Add("@RECORDING_CATEGORY", SqlDbType.NVarChar)
                            If RECORDING_CATEGORY = "" Or RECORDING_CATEGORY = Nothing Then
                                objCommand.Parameters("@RECORDING_CATEGORY").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@RECORDING_CATEGORY").Value = RECORDING_CATEGORY
                            End If

                            objCommand.Parameters.Add("@RECORDING_FORM", SqlDbType.NVarChar)
                            If RECORDING_FORM = "" Or RECORDING_FORM = Nothing Then
                                objCommand.Parameters("@RECORDING_FORM").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@RECORDING_FORM").Value = RECORDING_FORM
                            End If

                            objCommand.Parameters.Add("@RECORDING_FORMAT", SqlDbType.NVarChar)
                            If RECORDING_FORMAT = "" Or RECORDING_FORMAT = Nothing Then
                                objCommand.Parameters("@RECORDING_FORMAT").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@RECORDING_FORMAT").Value = RECORDING_FORMAT
                            End If

                            objCommand.Parameters.Add("@RECORDING_SPEED", SqlDbType.NVarChar)
                            If RECORDING_SPEED = "" Or RECORDING_SPEED = Nothing Then
                                objCommand.Parameters("@RECORDING_SPEED").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@RECORDING_SPEED").Value = RECORDING_SPEED
                            End If

                            objCommand.Parameters.Add("@RECORDING_STORAGE_TECH", SqlDbType.NVarChar)
                            If RECORDING_STORAGE_TECH = "" Or RECORDING_STORAGE_TECH = Nothing Then
                                objCommand.Parameters("@RECORDING_STORAGE_TECH").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@RECORDING_STORAGE_TECH").Value = RECORDING_STORAGE_TECH
                            End If

                            objCommand.Parameters.Add("@RECORDING_PLAY_DURATION", SqlDbType.NVarChar)
                            If RECORDING_PLAY_DURATION = "" Or RECORDING_PLAY_DURATION = Nothing Then
                                objCommand.Parameters("@RECORDING_PLAY_DURATION").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@RECORDING_PLAY_DURATION").Value = RECORDING_PLAY_DURATION
                            End If

                            objCommand.Parameters.Add("@VIDEO_TYPEOFVISUALS", SqlDbType.NVarChar)
                            If VIDEO_TYPEOFVISUALS = "" Or VIDEO_TYPEOFVISUALS = Nothing Then
                                objCommand.Parameters("@VIDEO_TYPEOFVISUALS").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@VIDEO_TYPEOFVISUALS").Value = VIDEO_TYPEOFVISUALS
                            End If

                            objCommand.Parameters.Add("@CARTOGRAPHIC_SCALE", SqlDbType.NVarChar)
                            If CARTOGRAPHIC_SCALE = "" Or CARTOGRAPHIC_SCALE = Nothing Then
                                objCommand.Parameters("@CARTOGRAPHIC_SCALE").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@CARTOGRAPHIC_SCALE").Value = CARTOGRAPHIC_SCALE
                            End If

                            objCommand.Parameters.Add("@CARTOGRAPHIC_PROJECTION", SqlDbType.NVarChar)
                            If CARTOGRAPHIC_PROJECTION = "" Or CARTOGRAPHIC_PROJECTION = Nothing Then
                                objCommand.Parameters("@CARTOGRAPHIC_PROJECTION").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@CARTOGRAPHIC_PROJECTION").Value = CARTOGRAPHIC_PROJECTION
                            End If

                            objCommand.Parameters.Add("@CARTOGRAPHIC_COORDINATES", SqlDbType.NVarChar)
                            If CARTOGRAPHIC_COORDINATES = "" Or CARTOGRAPHIC_COORDINATES = Nothing Then
                                objCommand.Parameters("@CARTOGRAPHIC_COORDINATES").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@CARTOGRAPHIC_COORDINATES").Value = CARTOGRAPHIC_COORDINATES
                            End If

                            objCommand.Parameters.Add("@CARTOGRAPHIC_GEOGRAPHIC_LOCATION", SqlDbType.NVarChar)
                            If CARTOGRAPHIC_GEOGRAPHIC_LOCATION = "" Or CARTOGRAPHIC_GEOGRAPHIC_LOCATION = Nothing Then
                                objCommand.Parameters("@CARTOGRAPHIC_GEOGRAPHIC_LOCATION").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@CARTOGRAPHIC_GEOGRAPHIC_LOCATION").Value = CARTOGRAPHIC_GEOGRAPHIC_LOCATION
                            End If

                            objCommand.Parameters.Add("@CARTOGRAPHIC_MEDIUM", SqlDbType.NVarChar)
                            If CARTOGRAPHIC_MEDIUM = "" Or CARTOGRAPHIC_MEDIUM = Nothing Then
                                objCommand.Parameters("@CARTOGRAPHIC_MEDIUM").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@CARTOGRAPHIC_MEDIUM").Value = CARTOGRAPHIC_MEDIUM
                            End If

                            objCommand.Parameters.Add("@CARTOGRAPHIC_DATAGATHERING_DATE", SqlDbType.DateTime)
                            If CARTOGRAPHIC_DATAGATHERING_DATE <> "" Then
                                objCommand.Parameters("@CARTOGRAPHIC_DATAGATHERING_DATE").Value = CARTOGRAPHIC_DATAGATHERING_DATE
                            Else
                                objCommand.Parameters("@CARTOGRAPHIC_DATAGATHERING_DATE").Value = System.DBNull.Value
                            End If

                            objCommand.Parameters.Add("@CARTOGRAPHIC_CREATION_DATE", SqlDbType.DateTime)
                            If CARTOGRAPHIC_CREATION_DATE <> "" Then
                                objCommand.Parameters("@CARTOGRAPHIC_CREATION_DATE").Value = CARTOGRAPHIC_CREATION_DATE
                            Else
                                objCommand.Parameters("@CARTOGRAPHIC_CREATION_DATE").Value = System.DBNull.Value
                            End If

                            objCommand.Parameters.Add("@CARTOGRAPHIC_COMPILATION_DATE", SqlDbType.DateTime)
                            If CARTOGRAPHIC_COMPILATION_DATE <> "" Then
                                objCommand.Parameters("@CARTOGRAPHIC_COMPILATION_DATE").Value = CARTOGRAPHIC_COMPILATION_DATE
                            Else
                                objCommand.Parameters("@CARTOGRAPHIC_COMPILATION_DATE").Value = System.DBNull.Value
                            End If

                            objCommand.Parameters.Add("@CARTOGRAPHIC_INSPECTION_DATE", SqlDbType.DateTime)
                            If CARTOGRAPHIC_INSPECTION_DATE <> "" Then
                                objCommand.Parameters("@CARTOGRAPHIC_INSPECTION_DATE").Value = CARTOGRAPHIC_INSPECTION_DATE
                            Else
                                objCommand.Parameters("@CARTOGRAPHIC_INSPECTION_DATE").Value = System.DBNull.Value
                            End If




                            objCommand.Parameters.Add("@VIDEO_COLOR", SqlDbType.NVarChar)
                            If VIDEO_COLOR = "" Or VIDEO_COLOR = Nothing Then
                                objCommand.Parameters("@VIDEO_COLOR").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@VIDEO_COLOR").Value = VIDEO_COLOR
                            End If


                            objCommand.Parameters.Add("@PLAYBACK_CHANNELS", SqlDbType.NVarChar)
                            If PLAYBACK_CHANNELS = "" Or PLAYBACK_CHANNELS = Nothing Then
                                objCommand.Parameters("@PLAYBACK_CHANNELS").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@PLAYBACK_CHANNELS").Value = PLAYBACK_CHANNELS
                            End If

                            objCommand.Parameters.Add("@TAPE_WIDTH", SqlDbType.NVarChar)
                            If TAPE_WIDTH = "" Or TAPE_WIDTH = Nothing Then
                                objCommand.Parameters("@TAPE_WIDTH").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@TAPE_WIDTH").Value = TAPE_WIDTH
                            End If

                            objCommand.Parameters.Add("@TAPE_CONFIGURATION", SqlDbType.NVarChar)
                            If TAPE_CONFIGURATION = "" Or TAPE_CONFIGURATION = Nothing Then
                                objCommand.Parameters("@TAPE_CONFIGURATION").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@TAPE_CONFIGURATION").Value = TAPE_CONFIGURATION
                            End If

                            objCommand.Parameters.Add("@KIND_OF_DISK", SqlDbType.NVarChar)
                            If KIND_OF_DISK = "" Or KIND_OF_DISK = Nothing Then
                                objCommand.Parameters("@KIND_OF_DISK").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@KIND_OF_DISK").Value = KIND_OF_DISK
                            End If

                            objCommand.Parameters.Add("@KIND_OF_CUTTING", SqlDbType.NVarChar)
                            If KIND_OF_CUTTING = "" Or KIND_OF_CUTTING = Nothing Then
                                objCommand.Parameters("@KIND_OF_CUTTING").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@KIND_OF_CUTTING").Value = KIND_OF_CUTTING
                            End If

                            objCommand.Parameters.Add("@ENCODING_STANDARD", SqlDbType.NVarChar)
                            If ENCODING_STANDARD = "" Or ENCODING_STANDARD = Nothing Then
                                objCommand.Parameters("@ENCODING_STANDARD").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@ENCODING_STANDARD").Value = ENCODING_STANDARD
                            End If

                            objCommand.Parameters.Add("@CAPTURE_TECHNIQUE", SqlDbType.NVarChar)
                            If CAPTURE_TECHNIQUE = "" Or CAPTURE_TECHNIQUE = Nothing Then
                                objCommand.Parameters("@CAPTURE_TECHNIQUE").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@CAPTURE_TECHNIQUE").Value = CAPTURE_TECHNIQUE
                            End If

                            objCommand.Parameters.Add("@PHOTO_NO", SqlDbType.NVarChar)
                            If PHOTO_NO = "" Or PHOTO_NO = Nothing Then
                                objCommand.Parameters("@PHOTO_NO").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@PHOTO_NO").Value = PHOTO_NO
                            End If

                            objCommand.Parameters.Add("@PHOTO_ALBUM_NO", SqlDbType.NVarChar)
                            If PHOTO_ALBUM_NO = "" Or PHOTO_ALBUM_NO = Nothing Then
                                objCommand.Parameters("@PHOTO_ALBUM_NO").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@PHOTO_ALBUM_NO").Value = PHOTO_ALBUM_NO
                            End If

                            objCommand.Parameters.Add("@PHOTO_OCASION", SqlDbType.NVarChar)
                            If PHOTO_OCASION = "" Or PHOTO_OCASION = Nothing Then
                                objCommand.Parameters("@PHOTO_OCASION").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@PHOTO_OCASION").Value = PHOTO_OCASION
                            End If

                            objCommand.Parameters.Add("@IMAGE_VIEW_TYPE", SqlDbType.NVarChar)
                            If IMAGE_VIEW_TYPE = "" Or IMAGE_VIEW_TYPE = Nothing Then
                                objCommand.Parameters("@IMAGE_VIEW_TYPE").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@IMAGE_VIEW_TYPE").Value = IMAGE_VIEW_TYPE
                            End If

                            objCommand.Parameters.Add("@THEME", SqlDbType.NVarChar)
                            If THEME = "" Or THEME = Nothing Then
                                objCommand.Parameters("@THEME").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@THEME").Value = THEME
                            End If

                            objCommand.Parameters.Add("@STYLE", SqlDbType.NVarChar)
                            If STYLE = "" Or STYLE = Nothing Then
                                objCommand.Parameters("@STYLE").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@STYLE").Value = STYLE
                            End If

                            objCommand.Parameters.Add("@CULTURE", SqlDbType.NVarChar)
                            If CULTURE = "" Or CULTURE = Nothing Then
                                objCommand.Parameters("@CULTURE").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@CULTURE").Value = CULTURE
                            End If

                            objCommand.Parameters.Add("@CURRENT_SITE", SqlDbType.NVarChar)
                            If CURRENT_SITE = "" Or CURRENT_SITE = Nothing Then
                                objCommand.Parameters("@CURRENT_SITE").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@CURRENT_SITE").Value = CURRENT_SITE
                            End If

                            objCommand.Parameters.Add("@CREATION_SITE", SqlDbType.NVarChar)
                            If CREATION_SITE = "" Or CREATION_SITE = Nothing Then
                                objCommand.Parameters("@CREATION_SITE").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@CREATION_SITE").Value = CREATION_SITE
                            End If

                            objCommand.Parameters.Add("@YARNCOUNT", SqlDbType.NVarChar)
                            If YARNCOUNT = "" Or YARNCOUNT = Nothing Then
                                objCommand.Parameters("@YARNCOUNT").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@YARNCOUNT").Value = YARNCOUNT
                            End If

                            objCommand.Parameters.Add("@MATERIAL_TYPE", SqlDbType.NVarChar)
                            If MATERIAL_TYPE = "" Or MATERIAL_TYPE = Nothing Then
                                objCommand.Parameters("@MATERIAL_TYPE").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@MATERIAL_TYPE").Value = MATERIAL_TYPE
                            End If

                            objCommand.Parameters.Add("@TECHNIQUE", SqlDbType.NVarChar)
                            If TECHNIQUE = "" Or TECHNIQUE = Nothing Then
                                objCommand.Parameters("@TECHNIQUE").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@TECHNIQUE").Value = TECHNIQUE
                            End If

                            objCommand.Parameters.Add("@TECH_DETAILS", SqlDbType.NVarChar)
                            If TECH_DETAILS = "" Or TECH_DETAILS = Nothing Then
                                objCommand.Parameters("@TECH_DETAILS").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@TECH_DETAILS").Value = TECH_DETAILS
                            End If

                            objCommand.Parameters.Add("@INSCRIPTIONS", SqlDbType.NVarChar)
                            If INSCRIPTIONS = "" Or INSCRIPTIONS = Nothing Then
                                objCommand.Parameters("@INSCRIPTIONS").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@INSCRIPTIONS").Value = INSCRIPTIONS
                            End If

                            objCommand.Parameters.Add("@DESCRIPTION", SqlDbType.NVarChar)
                            If DESCRIPTION = "" Or DESCRIPTION = Nothing Then
                                objCommand.Parameters("@DESCRIPTION").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@DESCRIPTION").Value = DESCRIPTION
                            End If

                            objCommand.Parameters.Add("@GLOBE_TYPE", SqlDbType.NVarChar)
                            If GLOBE_TYPE = "" Or GLOBE_TYPE = Nothing Then
                                objCommand.Parameters("@GLOBE_TYPE").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@GLOBE_TYPE").Value = GLOBE_TYPE
                            End If

                            objCommand.Parameters.Add("@ALTER_DATE", SqlDbType.DateTime)
                            If ALTER_DATE = "" Or ALTER_DATE = Nothing Then
                                objCommand.Parameters("@ALTER_DATE").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@ALTER_DATE").Value = ALTER_DATE
                            End If

                            objCommand.Parameters.Add("@VIEW_DATE", SqlDbType.DateTime)
                            If VIEW_DATE = "" Or VIEW_DATE = Nothing Then
                                objCommand.Parameters("@VIEW_DATE").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@VIEW_DATE").Value = VIEW_DATE
                            End If

                            objCommand.Parameters.Add("@DATE_ADDED", SqlDbType.DateTime)
                            If DATE_ADDED = "" Or DATE_ADDED = Nothing Then
                                objCommand.Parameters("@DATE_ADDED").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@DATE_ADDED").Value = DATE_ADDED
                            End If

                            If USER_CODE = "" Then USER_CODE = System.DBNull.Value
                            objCommand.Parameters.Add("@USER_CODE", SqlDbType.NVarChar)
                            objCommand.Parameters("@USER_CODE").Value = USER_CODE

                            If IP = "" Then IP = System.DBNull.Value
                            objCommand.Parameters.Add("@IP", SqlDbType.VarChar)
                            objCommand.Parameters("@IP").Value = IP

                            Dim dr As SqlDataReader
                            dr = objCommand.ExecuteReader()
                            If dr.Read Then
                                intValue = dr.GetValue(0)
                            End If
                            dr.Close()

                            'count copies from holdings
                            If intValue <> 0 Then
                                Dim objCommand2 As New SqlCommand
                                objCommand2.Connection = SqlConn
                                objCommand2.Transaction = thisTransaction
                                objCommand2.CommandType = CommandType.Text
                                objCommand2.CommandText = "UPDATE ACQUISITIONS SET COPY_ACCESSIONED =(SELECT COUNT(*) FROM HOLDINGS WHERE ACQ_ID = @ACQ_ID) WHERE (ACQ_ID = @ACQ_ID)"

                                objCommand2.Parameters.Add("@ACQ_ID", SqlDbType.Int)
                                objCommand2.Parameters("@ACQ_ID").Value = ACQ_ID

                                Dim dr2 As SqlDataReader
                                dr2 = objCommand2.ExecuteReader()
                                dr2.Close()
                            End If
                            If intValue <> 0 Then
                                Dim objCommand3 As New SqlCommand
                                objCommand3.Connection = SqlConn
                                objCommand3.Transaction = thisTransaction
                                objCommand3.CommandType = CommandType.Text
                                objCommand3.CommandText = "UPDATE ACQUISITIONS SET PROCESS_STATUS =(CASE WHEN COPY_RECEIVED=COPY_ACCESSIONED THEN 'Accessioned' ELSE 'Partially Accessioned' END) WHERE (ACQ_ID = @ACQ_ID)"

                                objCommand3.Parameters.Add("@ACQ_ID", SqlDbType.Int)
                                objCommand3.Parameters("@ACQ_ID").Value = ACQ_ID

                                Dim dr3 As SqlDataReader
                                dr3 = objCommand3.ExecuteReader()
                                dr3.Close()
                            End If

                            If intValue <> 0 Then
                                Dim objCommand4 As New SqlCommand
                                objCommand4.Connection = SqlConn
                                objCommand4.Transaction = thisTransaction
                                objCommand4.CommandType = CommandType.Text
                                objCommand4.CommandText = "UPDATE CATS SET CAT_LEVEL ='Full' WHERE (CAT_NO = @CAT_NO)"

                                objCommand4.Parameters.Add("@CAT_NO", SqlDbType.Int)
                                objCommand4.Parameters("@CAT_NO").Value = CAT_NO

                                Dim dr4 As SqlDataReader
                                dr4 = objCommand4.ExecuteReader()
                                dr4.Close()
                            End If
                            Label1.Text = "Record Added Successfully! " & "HOLD ID: " & intValue.ToString
                            Label15.Text = ""

                            thisTransaction.Commit()
                            SqlConn.Close()

                        End If

                    Next
                    objr.WriteLine(line)
                    Label1.Text = "Records Added Successfully! " & "Acc No: " & line.ToString
                    Label15.Text = ""
                    objr.Close()
                End If

                Me.Hold_Save_Bttn.Visible = True
                Hold_Update_Bttn.Visible = False
                txt_Hold_AccNo.Text = ""
                PopulateOrderGrid1(DDL_Orders.SelectedValue)
                PopulateHoldingsGrid(Label_CATNO.Text, ACQ_ID)
                txt_Hold_AccNo.Focus()
            Catch q As SqlException
                thisTransaction.Rollback()
                Label15.Text = "Database Error -SAVE: " & (q.Message() & ", " & q.Number & ", " & q.GetType.ToString)
                Label1.Text = ""
            Catch ex As Exception
                Label15.Text = "Error-SAVE: " & (ex.Message())
                Label1.Text = ""
            Finally
                SqlConn.Close()
            End Try
        End If
    End Sub
    Public Sub ClearFields()
        Me.txt_Hold_AccDate.Text = ""
        Me.txt_Hold_AccNo.Text = ""
        Me.txt_Hold_BookNo.Text = ""
        Me.txt_Hold_ClassNo.Text = ""
        Me.txt_Hold_CompilationDate.Text = ""
        Me.txt_Hold_Coordinates.Text = ""
        Me.txt_Hold_CopyISBN.Text = ""
        Me.txt_Hold_CreationDate.Text = ""
        Me.txt_Hold_DataGatheringDate.Text = ""
        Me.txt_Hold_GeographicLocation.Text = ""
        Me.txt_Hold_InspectionDate.Text = ""
        Me.txt_Hold_Location.Text = ""
        Me.txt_Hold_Pages.Text = ""
        Me.txt_Hold_Projection.Text = ""
        Me.txt_Hold_RecordingCategory.Text = ""
        Me.txt_Hold_RecordingDuration.Text = ""
        Me.txt_Hold_RecordingForm.Text = ""
        Me.txt_Hold_RecordingFormat.Text = ""
        Me.txt_Hold_RecordingMedium.Text = ""
        Me.txt_Hold_RecordingSpeed.Text = ""
        Me.txt_Hold_RecordingStorageTech.Text = ""
        Me.txt_Hold_ReferenceNo.Text = ""
        Me.txt_Hold_Remarks.Text = ""
        Me.txt_Hold_Scale.Text = ""
        Me.txt_Hold_Size.Text = ""
        Me.txt_Hold_TypeOfVisuals.Text = ""
        Me.txt_Hold_VolEditors.Text = ""
        Me.txt_Hold_VolNo.Text = ""
        Me.txt_Hold_VolTitle.Text = ""
        Me.txt_Hold_VolYear.Text = ""

        txt_Hold_Color.Text = ""
        txt_Hold_PlayBackChannel.Text = ""
        txt_Hold_TapeWidth.Text = ""
        txt_Hold_TapeConfiguration.Text = ""
        txt_Hold_KindofDisk.Text = ""
        txt_Hold_KindofCutting.Text = ""
        txt_Hold_EncodingStandard.Text = ""
        txt_Hold_CaptureTechnique.Text = ""
        txt_Hold_PhotoNo.Text = ""
        txt_Hold_PhotoAlbumNo.Text = ""
        txt_Hold_Color.Text = ""
        txt_Hold_Ocasion.Text = ""
        txt_Hold_ImageViewType.Text = ""
        txt_Hold_ViewDate.Text = ""
        txt_Hold_Theme.Text = ""
        txt_Hold_Style.Text = ""
        txt_Hold_Culture.Text = ""
        txt_Hold_CurrentSite.Text = ""
        txt_Hold_CreationSite.Text = ""
        txt_Hold_YarnCount.Text = ""
        txt_Hold_MaterialsType.Text = ""
        txt_Hold_Technique.Text = ""
        txt_Hold_TechDetails.Text = ""
        txt_Hold_Inscription.Text = ""
        txt_Hold_Description.Text = ""
        txt_Hold_GlobeType.Text = ""
        txt_Hold_AlterDate.Text = ""
        DDL_AccMaterials.ClearSelection()
        Me.DDL_GeographicMedium.ClearSelection()
    End Sub
    'fill Grid with holdings
    Public Sub PopulateHoldingsGrid(ByVal CAT_NO As Object, ByVal ACQ_ID As Object)
        Dim dtSearch As DataTable = Nothing
        Try
            If CAT_NO <> "" Then
                Dim SQL As String = Nothing
                SQL = "SELECT HOLD_ID, CAT_NO, ACQ_ID, ACCESSION_NO, ACCESSION_DATE, VOL_NO, CLASS_NO, BOOK_NO, PAGINATION, PHYSICAL_LOCATION, COLLECTION_TYPE, LIB_CODE FROM HOLDINGS WHERE (CAT_NO = '" & CAT_NO & "')   AND (ACQ_ID = '" & Trim(ACQ_ID) & "') ORDER BY HOLD_ID DESC"

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
                    Label16.Text = "Display Holdings/Copies Record(s); Total Record(s): 0 "
                    Hold_Delete_Bttn.Visible = False
                Else
                    Grid2.Visible = True
                    RecordCount = dtSearch.Rows.Count
                    Grid2.DataSource = dtSearch
                    Grid2.DataBind()
                    Label16.Text = "Display Holdings/Copies Record(s); Total Record(s): " & RecordCount
                    Hold_Delete_Bttn.Visible = True
                End If
                ViewState("dt") = dtSearch
            Else
                Me.Grid2.DataSource = Nothing
                Grid2.DataBind()
                Label16.Text = "Display Holdings/Copies Record(s); Total Record(s): 0 "
                Hold_Delete_Bttn.Visible = False
            End If
        Catch s As Exception
            Label16.Text = "Error: " & (s.Message())
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
                    ClearFields()
                    If dr.HasRows = True Then
                        If dr.Item("CAT_NO").ToString <> "" Then
                            Label_CATNO.Text = dr.Item("CAT_NO").ToString
                        Else
                            Label_CATNO.Text = ""
                        End If
                        If dr.Item("ACQ_ID").ToString <> "" Then
                            Label_ACQID.Text = dr.Item("ACQ_ID").ToString
                        Else
                            Label_ACQID.Text = ""
                        End If

                        If dr.Item("HOLD_ID").ToString <> "" Then
                            Label_HOLDID.Text = dr.Item("HOLD_ID").ToString
                        Else
                            Label_HOLDID.Text = ""
                        End If

                        If dr.Item("ACCESSION_NO").ToString <> "" Then
                            txt_Hold_AccNo.Text = dr.Item("ACCESSION_NO").ToString
                        Else
                            txt_Hold_AccNo.Text = ""
                        End If

                        If dr.Item("ACCESSION_DATE").ToString <> "" Then
                            txt_Hold_AccDate.Text = Format(dr.Item("ACCESSION_DATE"), "dd/MM/yyyy")
                        Else
                            txt_Hold_AccDate.Text = ""
                        End If

                        If dr.Item("FORMAT_CODE").ToString <> "" Then
                            DDL_Format.SelectedValue = dr.Item("FORMAT_CODE").ToString
                        Else
                            DDL_Format.ClearSelection()
                        End If

                        If dr.Item("SHOW").ToString <> "" Then
                            DDL_Show.SelectedValue = dr.Item("SHOW").ToString
                        Else
                            DDL_Show.ClearSelection()
                        End If

                        If dr.Item("ISSUEABLE").ToString <> "" Then
                            DDL_Issuable.SelectedValue = dr.Item("ISSUEABLE").ToString
                        Else
                            DDL_Issuable.ClearSelection()
                        End If

                        If dr.Item("VOL_NO").ToString <> "" Then
                            txt_Hold_VolNo.Text = dr.Item("VOL_NO").ToString
                        Else
                            txt_Hold_VolNo.Text = ""
                        End If

                        If dr.Item("VOL_YEAR").ToString <> "" Then
                            txt_Hold_VolYear.Text = dr.Item("VOL_YEAR").ToString
                        Else
                            txt_Hold_VolYear.Text = ""
                        End If

                        If dr.Item("VOL_EDITORS").ToString <> "" Then
                            txt_Hold_VolEditors.Text = dr.Item("VOL_EDITORS").ToString
                        Else
                            txt_Hold_VolEditors.Text = ""
                        End If

                        If dr.Item("VOL_TITLE").ToString <> "" Then
                            txt_Hold_VolTitle.Text = dr.Item("VOL_TITLE").ToString
                        Else
                            txt_Hold_VolTitle.Text = ""
                        End If

                        If dr.Item("COPY_ISBN").ToString <> "" Then
                            txt_Hold_CopyISBN.Text = dr.Item("COPY_ISBN").ToString
                        Else
                            txt_Hold_CopyISBN.Text = ""
                        End If

                        If dr.Item("CLASS_NO").ToString <> "" Then
                            txt_Hold_ClassNo.Text = dr.Item("CLASS_NO").ToString
                        Else
                            txt_Hold_ClassNo.Text = ""
                        End If

                        If dr.Item("BOOK_NO").ToString <> "" Then
                            txt_Hold_BookNo.Text = dr.Item("BOOK_NO").ToString
                        Else
                            txt_Hold_BookNo.Text = ""
                        End If

                        If dr.Item("PAGINATION").ToString <> "" Then
                            txt_Hold_Pages.Text = dr.Item("PAGINATION").ToString
                        Else
                            txt_Hold_Pages.Text = ""
                        End If

                        If dr.Item("SIZE").ToString <> "" Then
                            txt_Hold_Size.Text = dr.Item("SIZE").ToString
                        Else
                            txt_Hold_Size.Text = ""
                        End If

                        If dr.Item("ILLUSTRATION").ToString <> "" Then
                            CB_Illus.Checked = True
                        Else
                            CB_Illus.Checked = False
                        End If

                        If dr.Item("COLLECTION_TYPE").ToString <> "" Then
                            DDL_CollectionType.SelectedValue = dr.Item("COLLECTION_TYPE").ToString
                        Else
                            DDL_CollectionType.ClearSelection()
                        End If

                        If dr.Item("STA_CODE").ToString <> "" Then
                            DDL_Status.SelectedValue = dr.Item("STA_CODE").ToString
                        Else
                            DDL_Status.ClearSelection()
                        End If

                        If dr.Item("BIND_CODE").ToString <> "" Then
                            DDL_Binding.SelectedValue = dr.Item("BIND_CODE").ToString
                        Else
                            DDL_Binding.ClearSelection()
                        End If

                        If dr.Item("SEC_CODE").ToString <> "" Then
                            DDL_Section.SelectedValue = dr.Item("SEC_CODE").ToString
                        Else
                            DDL_Section.ClearSelection()
                        End If

                        If dr.Item("LIB_CODE").ToString <> "" Then
                            DDL_Library.SelectedValue = dr.Item("LIB_CODE").ToString
                        Else
                            DDL_Library.ClearSelection()
                        End If

                        If dr.Item("ACC_MAT_CODE").ToString <> "" Then
                            DDL_AccMaterials.SelectedValue = dr.Item("ACC_MAT_CODE").ToString
                        Else
                            DDL_AccMaterials.ClearSelection()
                        End If

                        If dr.Item("REMARKS").ToString <> "" Then
                            txt_Hold_Remarks.Text = dr.Item("REMARKS").ToString
                        Else
                            txt_Hold_Remarks.Text = ""
                        End If

                        If dr.Item("PHYSICAL_LOCATION").ToString <> "" Then
                            txt_Hold_Location.Text = dr.Item("PHYSICAL_LOCATION").ToString
                        Else
                            txt_Hold_Location.Text = ""
                        End If

                        If dr.Item("REFERENCE_NO").ToString <> "" Then
                            txt_Hold_ReferenceNo.Text = dr.Item("REFERENCE_NO").ToString
                        Else
                            txt_Hold_ReferenceNo.Text = ""
                        End If

                        If dr.Item("MEDIUM").ToString <> "" Then
                            txt_Hold_RecordingMedium.Text = dr.Item("MEDIUM").ToString
                        Else
                            txt_Hold_RecordingMedium.Text = ""
                        End If

                        If dr.Item("RECORDING_CATEGORY").ToString <> "" Then
                            txt_Hold_RecordingCategory.Text = dr.Item("RECORDING_CATEGORY").ToString
                        Else
                            txt_Hold_RecordingCategory.Text = ""
                        End If

                        If dr.Item("RECORDING_FORM").ToString <> "" Then
                            txt_Hold_RecordingForm.Text = dr.Item("RECORDING_FORM").ToString
                        Else
                            txt_Hold_RecordingForm.Text = ""
                        End If

                        If dr.Item("RECORDING_FORMAT").ToString <> "" Then
                            txt_Hold_RecordingFormat.Text = dr.Item("RECORDING_FORMAT").ToString
                        Else
                            txt_Hold_RecordingFormat.Text = ""
                        End If

                        If dr.Item("RECORDING_SPEED").ToString <> "" Then
                            txt_Hold_RecordingSpeed.Text = dr.Item("RECORDING_SPEED").ToString
                        Else
                            txt_Hold_RecordingSpeed.Text = ""
                        End If

                        If dr.Item("RECORDING_STORAGE_TECH").ToString <> "" Then
                            txt_Hold_RecordingStorageTech.Text = dr.Item("RECORDING_STORAGE_TECH").ToString
                        Else
                            txt_Hold_RecordingStorageTech.Text = ""
                        End If

                        If dr.Item("RECORDING_PLAY_DURATION").ToString <> "" Then
                            txt_Hold_RecordingDuration.Text = dr.Item("RECORDING_PLAY_DURATION").ToString
                        Else
                            txt_Hold_RecordingDuration.Text = ""
                        End If

                        If dr.Item("VIDEO_TYPEOFVISUAL").ToString <> "" Then
                            txt_Hold_TypeOfVisuals.Text = dr.Item("VIDEO_TYPEOFVISUAL").ToString
                        Else
                            txt_Hold_TypeOfVisuals.Text = ""
                        End If

                        If dr.Item("CARTOGRAPHIC_SCALE").ToString <> "" Then
                            txt_Hold_Scale.Text = dr.Item("CARTOGRAPHIC_SCALE").ToString
                        Else
                            txt_Hold_Scale.Text = ""
                        End If

                        If dr.Item("CARTOGRAPHIC_PROJECTION").ToString <> "" Then
                            txt_Hold_Projection.Text = dr.Item("CARTOGRAPHIC_PROJECTION").ToString
                        Else
                            txt_Hold_Projection.Text = ""
                        End If

                        If dr.Item("CARTOGRAPHIC_COORDINATES").ToString <> "" Then
                            txt_Hold_Coordinates.Text = dr.Item("CARTOGRAPHIC_COORDINATES").ToString
                        Else
                            txt_Hold_Coordinates.Text = ""
                        End If

                        If dr.Item("CARTOGRAPHIC_GEOGRAPHIC_LOCATION").ToString <> "" Then
                            txt_Hold_GeographicLocation.Text = dr.Item("CARTOGRAPHIC_GEOGRAPHIC_LOCATION").ToString
                        Else
                            txt_Hold_GeographicLocation.Text = ""
                        End If

                        If dr.Item("CARTOGRAPHIC_MEDIUM").ToString <> "" Then
                            DDL_GeographicMedium.SelectedValue = dr.Item("CARTOGRAPHIC_MEDIUM").ToString
                        Else
                            DDL_GeographicMedium.ClearSelection()
                        End If

                        If dr.Item("CARTOGRAPHIC_DATAGATHERING_DATE").ToString <> "" Then
                            txt_Hold_DataGatheringDate.Text = Format(dr.Item("CARTOGRAPHIC_DATAGATHERING_DATE"), "dd/MM/yyyy")
                        Else
                            txt_Hold_DataGatheringDate.Text = ""
                        End If

                        If dr.Item("CREATION_DATE").ToString <> "" Then
                            txt_Hold_CreationDate.Text = Format(dr.Item("CREATION_DATE"), "dd/MM/yyyy")
                        Else
                            txt_Hold_CreationDate.Text = ""
                        End If

                        If dr.Item("CARTOGRAPHIC_COMPILATION_DATE").ToString <> "" Then
                            txt_Hold_CompilationDate.Text = Format(dr.Item("CARTOGRAPHIC_COMPILATION_DATE"), "dd/MM/yyyy")
                        Else
                            txt_Hold_CompilationDate.Text = ""
                        End If

                        If dr.Item("CARTOGRAPHIC_INSPECTION_DATE").ToString <> "" Then
                            txt_Hold_InspectionDate.Text = Format(dr.Item("CARTOGRAPHIC_INSPECTION_DATE"), "dd/MM/yyyy")
                        Else
                            txt_Hold_InspectionDate.Text = ""
                        End If

                        If dr.Item("ALTER_DATE").ToString <> "" Then
                            txt_Hold_AlterDate.Text = Format(dr.Item("ALTER_DATE"), "dd/MM/yyyy")
                        Else
                            txt_Hold_AlterDate.Text = ""
                        End If

                        If dr.Item("VIEW_DATE").ToString <> "" Then
                            txt_Hold_ViewDate.Text = Format(dr.Item("VIEW_DATE"), "dd/MM/yyyy")
                        Else
                            txt_Hold_ViewDate.Text = ""
                        End If

                        If dr.Item("VIDEO_COLOR").ToString <> "" Then
                            txt_Hold_Color.Text = dr.Item("VIDEO_COLOR").ToString
                        Else
                            txt_Hold_Color.Text = ""
                        End If

                        If dr.Item("PLAYBACK_CHANNELS").ToString <> "" Then
                            txt_Hold_PlayBackChannel.Text = dr.Item("PLAYBACK_CHANNELS").ToString
                        Else
                            txt_Hold_PlayBackChannel.Text = ""
                        End If

                        If dr.Item("TAPE_WIDTH").ToString <> "" Then
                            txt_Hold_TapeWidth.Text = dr.Item("TAPE_WIDTH").ToString
                        Else
                            txt_Hold_TapeWidth.Text = ""
                        End If

                        If dr.Item("TAPE_CONFIGURATION").ToString <> "" Then
                            txt_Hold_TapeConfiguration.Text = dr.Item("TAPE_CONFIGURATION").ToString
                        Else
                            txt_Hold_TapeConfiguration.Text = ""
                        End If

                        If dr.Item("KIND_OF_DISK").ToString <> "" Then
                            txt_Hold_KindofDisk.Text = dr.Item("KIND_OF_DISK").ToString
                        Else
                            txt_Hold_KindofDisk.Text = ""
                        End If

                        If dr.Item("KIND_OF_CUTTING").ToString <> "" Then
                            txt_Hold_KindofCutting.Text = dr.Item("KIND_OF_CUTTING").ToString
                        Else
                            txt_Hold_KindofCutting.Text = ""
                        End If

                        If dr.Item("ENCODING_STANDARD").ToString <> "" Then
                            txt_Hold_EncodingStandard.Text = dr.Item("ENCODING_STANDARD").ToString
                        Else
                            txt_Hold_EncodingStandard.Text = ""
                        End If

                        If dr.Item("CAPTURE_TECHNIQUE").ToString <> "" Then
                            txt_Hold_CaptureTechnique.Text = dr.Item("CAPTURE_TECHNIQUE").ToString
                        Else
                            txt_Hold_CaptureTechnique.Text = ""
                        End If

                        If dr.Item("PHOTO_NO").ToString <> "" Then
                            txt_Hold_PhotoNo.Text = dr.Item("PHOTO_NO").ToString
                        Else
                            txt_Hold_PhotoNo.Text = ""
                        End If

                        If dr.Item("PHOTO_ALBUM_NO").ToString <> "" Then
                            txt_Hold_PhotoAlbumNo.Text = dr.Item("PHOTO_ALBUM_NO").ToString
                        Else
                            txt_Hold_PhotoAlbumNo.Text = ""
                        End If

                        If dr.Item("PHOTO_OCASION").ToString <> "" Then
                            txt_Hold_Ocasion.Text = dr.Item("PHOTO_OCASION").ToString
                        Else
                            txt_Hold_Ocasion.Text = ""
                        End If

                        If dr.Item("IMAGE_VIEW_TYPE").ToString <> "" Then
                            txt_Hold_ImageViewType.Text = dr.Item("IMAGE_VIEW_TYPE").ToString
                        Else
                            txt_Hold_TapeConfiguration.Text = ""
                        End If

                        If dr.Item("THEME").ToString <> "" Then
                            txt_Hold_Theme.Text = dr.Item("THEME").ToString
                        Else
                            txt_Hold_Theme.Text = ""
                        End If

                        If dr.Item("STYLE").ToString <> "" Then
                            txt_Hold_Style.Text = dr.Item("STYLE").ToString
                        Else
                            txt_Hold_Style.Text = ""
                        End If

                        If dr.Item("CULTURE").ToString <> "" Then
                            txt_Hold_Culture.Text = dr.Item("CULTURE").ToString
                        Else
                            txt_Hold_Culture.Text = ""
                        End If

                        If dr.Item("CURRENT_SITE").ToString <> "" Then
                            txt_Hold_CurrentSite.Text = dr.Item("CURRENT_SITE").ToString
                        Else
                            txt_Hold_CurrentSite.Text = ""
                        End If

                        If dr.Item("CREATION_SITE").ToString <> "" Then
                            txt_Hold_CreationDate.Text = dr.Item("CREATION_SITE").ToString
                        Else
                            txt_Hold_CreationSite.Text = ""
                        End If

                        If dr.Item("YARNCOUNT").ToString <> "" Then
                            txt_Hold_YarnCount.Text = dr.Item("YARNCOUNT").ToString
                        Else
                            txt_Hold_YarnCount.Text = ""
                        End If

                        If dr.Item("MATERIAL_TYPE").ToString <> "" Then
                            txt_Hold_MaterialsType.Text = dr.Item("MATERIAL_TYPE").ToString
                        Else
                            txt_Hold_MaterialsType.Text = ""
                        End If

                        If dr.Item("TECHNIQUE").ToString <> "" Then
                            txt_Hold_Technique.Text = dr.Item("TECHNIQUE").ToString
                        Else
                            txt_Hold_Technique.Text = ""
                        End If

                        If dr.Item("TECH_DETAILS").ToString <> "" Then
                            txt_Hold_TechDetails.Text = dr.Item("TECH_DETAILS").ToString
                        Else
                            txt_Hold_TechDetails.Text = ""
                        End If

                        If dr.Item("INSCRIPTIONS").ToString <> "" Then
                            txt_Hold_Inscription.Text = dr.Item("INSCRIPTIONS").ToString
                        Else
                            txt_Hold_Inscription.Text = ""
                        End If

                        If dr.Item("DESCRIPTION").ToString <> "" Then
                            txt_Hold_Description.Text = dr.Item("DESCRIPTION").ToString
                        Else
                            txt_Hold_Description.Text = ""
                        End If

                        If dr.Item("GLOBE_TYPE").ToString <> "" Then
                            txt_Hold_GlobeType.Text = dr.Item("GLOBE_TYPE").ToString
                        Else
                            txt_Hold_GlobeType.Text = ""
                        End If

                        dr.Close()
                        Me.Hold_Save_Bttn.Visible = True
                        Me.Hold_Update_Bttn.Visible = True
                    Else
                        Label_CATNO.Text = ""
                        Label_ACQID.Text = ""
                        Label_MATTYPE.Text = ""
                        Label_HOLDID.Text = ""
                        Label_TITLE.Text = ""
                        Label15.Text = "Record Not Selected"
                        Label1.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' No Record Selected!');", True)
                    End If
                Else
                    Label_CATNO.Text = ""
                    Label_ACQID.Text = ""
                    Label_MATTYPE.Text = ""
                    Label_HOLDID.Text = ""
                    Label_TITLE.Text = ""
                    Label15.Text = "Record Not Selected"
                    Label1.Text = ""
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' No Record Selected!');", True)
                End If
            End If
        Catch s As Exception
            Label15.Text = "Error: " & (s.Message())
            Label1.Text = ""
            Label_CATNO.Text = ""
            Label_ACQID.Text = ""
            Label_MATTYPE.Text = ""
            Label_HOLDID.Text = ""
            Label_TITLE.Text = ""
        Finally
            SqlConn.Close()
            Me.txt_Hold_AccNo.Focus()
        End Try
    End Sub 'Grid1_ItemCommand
    'clear fields
    Protected Sub Hold_Cancel_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Hold_Cancel_Bttn.Click
        ClearFields()
        Label_HOLDID.Text = ""
        Me.Hold_Save_Bttn.Visible = True
        Me.Hold_Update_Bttn.Visible = False
    End Sub
    'update holding record    
    Protected Sub Hold_Update_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Hold_Update_Bttn.Click
        Dim SQL As String = Nothing
        Dim da As SqlDataAdapter
        Dim cb As SqlCommandBuilder
        Dim ds As New DataSet
        Dim dt As New DataTable
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing

        If IsPostBack = True Then
            Dim iloop As Integer
            Dim strcurrentchar As Object
            Dim c As Integer
            Dim counter1, counter2, counter3, counter4, counter5, counter6, counter7, counter8, counter9, counter10 As Integer
            Dim counter11, counter12, counter13, counter14, counter15, counter16, counter17, counter18, counter19, counter20 As Integer
            Dim counter21, counter22, counter23, counter24, counter25, counter26, counter27, counter28, counter29, counter30 As Integer
            Try
                'validation for acq_id
                Dim HOLD_ID As Long = Nothing
                If Me.Label_HOLDID.Text <> "" Then
                    HOLD_ID = Convert.ToInt16(TrimX(Label_HOLDID.Text))
                    HOLD_ID = RemoveQuotes(HOLD_ID)
                    If Not IsNumeric(HOLD_ID) = True Then
                        Label15.Text = "Error: Input is not Valid !"
                        Label1.Text = ""
                        Exit Sub
                    End If
                    If Len(HOLD_ID) > 10 Then
                        Label15.Text = "Error: Input is not Valid !"
                        Label1.Text = ""
                        Exit Sub
                    End If
                    HOLD_ID = " " & HOLD_ID & " "
                    If InStr(1, HOLD_ID, "CREATE", 1) > 0 Or InStr(1, HOLD_ID, "DELETE", 1) > 0 Or InStr(1, HOLD_ID, "DROP", 1) > 0 Or InStr(1, HOLD_ID, "INSERT", 1) > 1 Or InStr(1, HOLD_ID, "TRACK", 1) > 1 Or InStr(1, HOLD_ID, "TRACE", 1) > 1 Then
                        Label15.Text = "Error: Input is not Valid !"
                        Label1.Text = ""
                        Exit Sub
                    End If
                    HOLD_ID = TrimX(HOLD_ID)
                    'check unwanted characters
                    c = 0
                    counter2 = 0
                    For iloop = 1 To Len(HOLD_ID.ToString)
                        strcurrentchar = Mid(HOLD_ID, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter2 = 1
                            End If
                        End If
                    Next
                    If counter2 = 1 Then
                        Label15.Text = "Error: Input is not Valid !"
                        Label1.Text = ""
                        Exit Sub
                    End If
                Else
                    Label15.Text = "Error: Input is not Valid !"
                    Label1.Text = ""
                    Exit Sub
                End If

                'Server Validation for accession no
                Dim ACCESSION_NO As Object = Nothing
                If txt_Hold_AccNo.Text <> "" Then
                    ACCESSION_NO = TrimX(UCase(txt_Hold_AccNo.Text))
                    ACCESSION_NO = RemoveQuotes(ACCESSION_NO)
                    If ACCESSION_NO.Length > 30 Then 'maximum length
                        Label15.Text = "Error: Input is not Valid !"
                        Label1.Text = ""
                        txt_Hold_AccNo.Focus()
                        Exit Sub
                    End If

                    ACCESSION_NO = " " & ACCESSION_NO & " "
                    If InStr(1, ACCESSION_NO, "CREATE", 1) > 0 Or InStr(1, ACCESSION_NO, "DELETE", 1) > 0 Or InStr(1, ACCESSION_NO, "DROP", 1) > 0 Or InStr(1, ACCESSION_NO, "INSERT", 1) > 1 Or InStr(1, ACCESSION_NO, "TRACK", 1) > 1 Or InStr(1, ACCESSION_NO, "TRACE", 1) > 1 Then
                        Label15.Text = "Error: Input is not Valid !"
                        Label1.Text = ""
                        Me.txt_Hold_AccNo.Focus()
                        Exit Sub
                    End If
                    ACCESSION_NO = TrimX(ACCESSION_NO)
                    'check unwanted characters
                    c = 0
                    counter3 = 0
                    For iloop = 1 To Len(ACCESSION_NO.ToString)
                        strcurrentchar = Mid(ACCESSION_NO, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^*+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter3 = 1
                            End If
                        End If
                    Next
                    If counter3 = 1 Then
                        Label15.Text = "Error: Input is not Valid !"
                        Label1.Text = ""
                        Me.txt_Hold_AccNo.Focus()
                        Exit Sub
                    End If

                    'check duplicate acc no
                    Dim str As Object = Nothing
                    Dim flag As Object = Nothing
                    str = "SELECT HOLD_ID FROM HOLDINGS WHERE (HOLD_ID <> ' " & Trim(HOLD_ID) & "'  AND ACCESSION_NO = '" & Trim(ACCESSION_NO) & "'  AND LIB_CODE = '" & Trim(DDL_Library.SelectedValue) & "')"
                    Dim cmd1 As New SqlCommand(str, SqlConn)
                    SqlConn.Open()
                    flag = cmd1.ExecuteScalar
                    If flag <> Nothing Then
                        Label15.Text = "This Accession No Already Exists ! "
                        Label1.Text = ""
                        Me.txt_Hold_AccNo.Focus()
                        Exit Sub
                    End If
                    SqlConn.Close()
                Else
                    Label15.Text = "Error: Input is Required !"
                    Label1.Text = ""
                    Me.txt_Hold_AccNo.Focus()
                    Exit Sub
                End If

                'search accession date
                Dim ACCESSION_DATE As Object = Nothing
                If txt_Hold_AccDate.Text <> "" Then
                    ACCESSION_DATE = TrimX(txt_Hold_AccDate.Text)
                    ACCESSION_DATE = RemoveQuotes(ACCESSION_DATE)
                    ACCESSION_DATE = Convert.ToDateTime(ACCESSION_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                    If Len(ACCESSION_DATE) > 12 Then
                        Label15.Text = " Input is not Valid..."
                        Label1.Text = ""
                        Me.txt_Hold_AccDate.Focus()
                        Exit Sub
                    End If
                    ACCESSION_DATE = " " & ACCESSION_DATE & " "
                    If InStr(1, ACCESSION_DATE, "CREATE", 1) > 0 Or InStr(1, ACCESSION_DATE, "DELETE", 1) > 0 Or InStr(1, ACCESSION_DATE, "DROP", 1) > 0 Or InStr(1, ACCESSION_DATE, "INSERT", 1) > 1 Or InStr(1, ACCESSION_DATE, "TRACK", 1) > 1 Or InStr(1, ACCESSION_DATE, "TRACE", 1) > 1 Then
                        Label15.Text = "  Input is not Valid... "
                        Label1.Text = ""
                        Me.txt_Hold_AccDate.Focus()
                        Exit Sub
                    End If
                    ACCESSION_DATE = TrimX(ACCESSION_DATE)
                    'check unwanted characters
                    c = 0
                    counter4 = 0
                    For iloop = 1 To Len(ACCESSION_DATE)
                        strcurrentchar = Mid(ACCESSION_DATE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter4 = 1
                            End If
                        End If
                    Next
                    If counter4 = 1 Then
                        Label15.Text = "data is not Valid... "
                        Label1.Text = ""
                        Me.txt_Hold_AccDate.Focus()
                        Exit Sub
                    End If
                Else
                    ACCESSION_DATE = Now.Date
                    ACCESSION_DATE = Convert.ToDateTime(ACCESSION_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us
                End If

                'validation for Format
                Dim FORMAT_CODE As Object = Nothing
                FORMAT_CODE = DDL_Format.SelectedValue
                If Not String.IsNullOrEmpty(FORMAT_CODE) Then
                    FORMAT_CODE = RemoveQuotes(FORMAT_CODE)
                    If FORMAT_CODE.Length > 3 Then 'maximum length
                        Label15.Text = " Data must be of Proper Length.. "
                        Label1.Text = ""
                        DDL_Format.Focus()
                        Exit Sub
                    End If

                    FORMAT_CODE = " " & FORMAT_CODE & " "
                    If InStr(1, FORMAT_CODE, "CREATE", 1) > 0 Or InStr(1, FORMAT_CODE, "DELETE", 1) > 0 Or InStr(1, FORMAT_CODE, "DROP", 1) > 0 Or InStr(1, FORMAT_CODE, "INSERT", 1) > 1 Or InStr(1, FORMAT_CODE, "TRACK", 1) > 1 Or InStr(1, FORMAT_CODE, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        DDL_Format.Focus()
                        Exit Sub
                    End If
                    FORMAT_CODE = TrimX(FORMAT_CODE)
                    'check unwanted characters
                    c = 0
                    counter5 = 0
                    For iloop = 1 To Len(FORMAT_CODE)
                        strcurrentchar = Mid(FORMAT_CODE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter5 = 1
                            End If
                        End If
                    Next
                    If counter5 = 1 Then
                        Label15.Text = " Input  is not Valid!"
                        Label1.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                        DDL_Format.Focus()
                        Exit Sub
                    End If
                Else
                    FORMAT_CODE = "PT"
                End If

                'validation for SHOW
                Dim SHOW As Object = Nothing
                SHOW = DDL_Show.SelectedValue
                If Not String.IsNullOrEmpty(SHOW) Then
                    SHOW = RemoveQuotes(SHOW)
                    If SHOW.Length > 2 Then 'maximum length
                        Label15.Text = " Data must be of Proper Length.. "
                        Label1.Text = ""
                        DDL_Show.Focus()
                        Exit Sub
                    End If

                    SHOW = " " & SHOW & " "
                    If InStr(1, SHOW, "CREATE", 1) > 0 Or InStr(1, SHOW, "DELETE", 1) > 0 Or InStr(1, SHOW, "DROP", 1) > 0 Or InStr(1, SHOW, "INSERT", 1) > 1 Or InStr(1, SHOW, "TRACK", 1) > 1 Or InStr(1, SHOW, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        DDL_Show.Focus()
                        Exit Sub
                    End If
                    SHOW = TrimX(SHOW)
                    'check unwanted characters
                    c = 0
                    counter6 = 0
                    For iloop = 1 To Len(SHOW)
                        strcurrentchar = Mid(SHOW, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter6 = 1
                            End If
                        End If
                    Next
                    If counter6 = 1 Then
                        Label15.Text = " Input  is not Valid!"
                        Label1.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                        DDL_Show.Focus()
                        Exit Sub
                    End If
                Else
                    SHOW = "Y"
                End If

                'validation for ISSUABLE
                Dim ISSUABLE As Object = Nothing
                ISSUABLE = DDL_Issuable.SelectedValue
                If Not String.IsNullOrEmpty(ISSUABLE) Then
                    ISSUABLE = RemoveQuotes(ISSUABLE)
                    If ISSUABLE.Length > 2 Then 'maximum length
                        Label15.Text = " Data must be of Proper Length.. "
                        Label1.Text = ""
                        DDL_Issuable.Focus()
                        Exit Sub
                    End If

                    ISSUABLE = " " & ISSUABLE & " "
                    If InStr(1, ISSUABLE, "CREATE", 1) > 0 Or InStr(1, ISSUABLE, "DELETE", 1) > 0 Or InStr(1, ISSUABLE, "DROP", 1) > 0 Or InStr(1, ISSUABLE, "INSERT", 1) > 1 Or InStr(1, ISSUABLE, "TRACK", 1) > 1 Or InStr(1, ISSUABLE, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        DDL_Issuable.Focus()
                        Exit Sub
                    End If
                    ISSUABLE = TrimX(ISSUABLE)
                    'check unwanted characters
                    c = 0
                    counter7 = 0
                    For iloop = 1 To Len(ISSUABLE)
                        strcurrentchar = Mid(ISSUABLE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter7 = 1
                            End If
                        End If
                    Next
                    If counter7 = 1 Then
                        Label15.Text = " Input  is not Valid!"
                        Label1.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                        DDL_Issuable.Focus()
                        Exit Sub
                    End If
                Else
                    ISSUABLE = "Y"
                End If

                'validation for VOL_NO
                Dim VOL_NO As Object = Nothing
                'If TR_VOL_NO.Visible = True Then
                VOL_NO = Trim(txt_Hold_VolNo.Text)
                If Not String.IsNullOrEmpty(VOL_NO) Then
                    VOL_NO = RemoveQuotes(VOL_NO)
                    If VOL_NO.Length > 30 Then 'maximum length
                        Label15.Text = " Data must be of Proper Length.. "
                        Label1.Text = ""
                        txt_Hold_VolNo.Focus()
                        Exit Sub
                    End If

                    VOL_NO = " " & VOL_NO & " "
                    If InStr(1, VOL_NO, "CREATE", 1) > 0 Or InStr(1, VOL_NO, "DELETE", 1) > 0 Or InStr(1, VOL_NO, "DROP", 1) > 0 Or InStr(1, VOL_NO, "INSERT", 1) > 1 Or InStr(1, VOL_NO, "TRACK", 1) > 1 Or InStr(1, VOL_NO, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        txt_Hold_VolNo.Focus()
                        Exit Sub
                    End If
                    VOL_NO = TrimAll(VOL_NO)
                    'check unwanted characters
                    c = 0
                    counter8 = 0
                    For iloop = 1 To Len(VOL_NO)
                        strcurrentchar = Mid(VOL_NO, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter8 = 1
                            End If
                        End If
                    Next
                    If counter8 = 1 Then
                        Label15.Text = " Input  is not Valid!"
                        Label1.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                        txt_Hold_VolNo.Focus()
                        Exit Sub
                    End If
                Else
                    VOL_NO = ""
                End If
                'Else
                'VOL_NO = ""
                'End If

                'validation for cat_no
                Dim VOL_YEAR As Integer = Nothing
                ' If TR_VOL_YEAR.Visible = True Then
                If Me.txt_Hold_VolYear.Text <> "" Then
                    VOL_YEAR = Convert.ToInt16(TrimX(txt_Hold_VolYear.Text))
                    VOL_YEAR = RemoveQuotes(VOL_YEAR)
                    If Not IsNumeric(VOL_YEAR) = True Then
                        Label15.Text = "Error: Input is not Valid !"
                        Label1.Text = ""
                        txt_Hold_VolYear.Focus()
                        Exit Sub
                    End If
                    If Len(VOL_YEAR) > 5 Then
                        Label15.Text = "Error: Input is not Valid !"
                        Label1.Text = ""
                        txt_Hold_VolYear.Focus()
                        Exit Sub
                    End If
                    VOL_YEAR = " " & VOL_YEAR & " "
                    If InStr(1, VOL_YEAR, "CREATE", 1) > 0 Or InStr(1, VOL_YEAR, "DELETE", 1) > 0 Or InStr(1, VOL_YEAR, "DROP", 1) > 0 Or InStr(1, VOL_YEAR, "INSERT", 1) > 1 Or InStr(1, VOL_YEAR, "TRACK", 1) > 1 Or InStr(1, VOL_YEAR, "TRACE", 1) > 1 Then
                        Label15.Text = "Error: Input is not Valid !"
                        Label1.Text = ""
                        txt_Hold_VolYear.Focus()
                        Exit Sub
                    End If
                    VOL_YEAR = TrimX(VOL_YEAR)
                    'check unwanted characters
                    c = 0
                    counter9 = 0
                    For iloop = 1 To Len(VOL_YEAR.ToString)
                        strcurrentchar = Mid(VOL_YEAR, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter9 = 1
                            End If
                        End If
                    Next
                    If counter9 = 1 Then
                        Label15.Text = "Error: Input is not Valid !"
                        Label1.Text = ""
                        txt_Hold_VolYear.Focus()
                        Exit Sub
                    End If
                Else
                    VOL_YEAR = 0
                End If
                'Else
                'VOL_YEAR = 0
                'End If

                'validation for VOL_EDITORS
                Dim VOL_EDITORS As Object = Nothing
                'If TR_VOL_EDITORS.Visible = True Then
                VOL_EDITORS = TrimAll(txt_Hold_VolEditors.Text)
                If Not String.IsNullOrEmpty(VOL_EDITORS) Then
                    VOL_EDITORS = RemoveQuotes(VOL_EDITORS)
                    If VOL_EDITORS.Length > 501 Then 'maximum length
                        Label15.Text = " Data must be of Proper Length.. "
                        Label1.Text = ""
                        txt_Hold_VolEditors.Focus()
                        Exit Sub
                    End If

                    VOL_EDITORS = " " & VOL_EDITORS & " "
                    If InStr(1, VOL_EDITORS, "CREATE", 1) > 0 Or InStr(1, VOL_EDITORS, "DELETE", 1) > 0 Or InStr(1, VOL_EDITORS, "DROP", 1) > 0 Or InStr(1, VOL_EDITORS, "INSERT", 1) > 1 Or InStr(1, VOL_EDITORS, "TRACK", 1) > 1 Or InStr(1, VOL_EDITORS, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        txt_Hold_VolEditors.Focus()
                        Exit Sub
                    End If
                    VOL_EDITORS = TrimAll(VOL_EDITORS)
                    'check unwanted characters
                    c = 0
                    counter10 = 0
                    For iloop = 1 To Len(VOL_EDITORS)
                        strcurrentchar = Mid(VOL_EDITORS, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter10 = 1
                            End If
                        End If
                    Next
                    If counter10 = 1 Then
                        Label15.Text = " Input  is not Valid!"
                        Label1.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                        txt_Hold_VolEditors.Focus()
                        Exit Sub
                    End If
                Else
                    VOL_EDITORS = ""
                End If
                'Else
                'VOL_EDITORS = ""
                'End If

                'validation for VOL_TITLE
                Dim VOL_TITLE As Object = Nothing
                ' If TR_VOL_TITLE.Visible = True Then
                VOL_TITLE = TrimAll(txt_Hold_VolTitle.Text)
                If Not String.IsNullOrEmpty(VOL_TITLE) Then
                    VOL_TITLE = RemoveQuotes(VOL_TITLE)
                    If VOL_TITLE.Length > 501 Then 'maximum length
                        Label15.Text = " Data must be of Proper Length.. "
                        Label1.Text = ""
                        txt_Hold_VolTitle.Focus()
                        Exit Sub
                    End If

                    VOL_TITLE = " " & VOL_TITLE & " "
                    If InStr(1, VOL_TITLE, "CREATE", 1) > 0 Or InStr(1, VOL_TITLE, "DELETE", 1) > 0 Or InStr(1, VOL_TITLE, "DROP", 1) > 0 Or InStr(1, VOL_TITLE, "INSERT", 1) > 1 Or InStr(1, VOL_TITLE, "TRACK", 1) > 1 Or InStr(1, VOL_TITLE, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        txt_Hold_VolTitle.Focus()
                        Exit Sub
                    End If
                    VOL_TITLE = TrimAll(VOL_TITLE)
                    'check unwanted characters
                    c = 0
                    counter11 = 0
                    For iloop = 1 To Len(VOL_TITLE)
                        strcurrentchar = Mid(VOL_TITLE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter11 = 1
                            End If
                        End If
                    Next
                    If counter11 = 1 Then
                        Label15.Text = " Input  is not Valid!"
                        Label1.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                        txt_Hold_VolTitle.Focus()
                        Exit Sub
                    End If
                Else
                    VOL_TITLE = ""
                End If
                'Else
                'VOL_TITLE = ""
                'End If

                'validation for VOL_ISBN
                Dim COPY_ISBN As Object = Nothing
                ' If TR_COPY_ISBN.Visible = True Then
                COPY_ISBN = TrimX(txt_Hold_CopyISBN.Text)
                If Not String.IsNullOrEmpty(COPY_ISBN) Then
                    COPY_ISBN = RemoveQuotes(COPY_ISBN)
                    If COPY_ISBN.Length > 30 Then 'maximum length
                        Label15.Text = " Data must be of Proper Length.. "
                        Label1.Text = ""
                        txt_Hold_CopyISBN.Focus()
                        Exit Sub
                    End If

                    COPY_ISBN = " " & COPY_ISBN & " "
                    If InStr(1, COPY_ISBN, "CREATE", 1) > 0 Or InStr(1, COPY_ISBN, "DELETE", 1) > 0 Or InStr(1, COPY_ISBN, "DROP", 1) > 0 Or InStr(1, COPY_ISBN, "INSERT", 1) > 1 Or InStr(1, COPY_ISBN, "TRACK", 1) > 1 Or InStr(1, COPY_ISBN, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Label15.Text = "Do not use reserved Words!"
                        Label1.Text = ""
                        txt_Hold_CopyISBN.Focus()
                        Exit Sub
                    End If
                    COPY_ISBN = TrimX(COPY_ISBN)
                    'check unwanted characters
                    c = 0
                    counter12 = 0
                    For iloop = 1 To Len(COPY_ISBN)
                        strcurrentchar = Mid(COPY_ISBN, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter12 = 1
                            End If
                        End If
                    Next
                    If counter12 = 1 Then
                        Label15.Text = " Input  is not Valid!"
                        Label1.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                        txt_Hold_CopyISBN.Focus()
                        Exit Sub
                    End If
                Else
                    COPY_ISBN = ""
                End If
                'Else
                'COPY_ISBN = ""
                'End If

                'validation for CLASS NO
                Dim CLASS_NO As Object = Nothing
                CLASS_NO = TrimX(txt_Hold_ClassNo.Text)
                If Not String.IsNullOrEmpty(CLASS_NO) Then
                    CLASS_NO = RemoveQuotes(CLASS_NO)
                    If CLASS_NO.Length > 150 Then 'maximum length
                        Label15.Text = " Data must be of Proper Length.. "
                        Label1.Text = ""
                        txt_Hold_ClassNo.Focus()
                        Exit Sub
                    End If

                    CLASS_NO = " " & CLASS_NO & " "
                    If InStr(1, CLASS_NO, "CREATE", 1) > 0 Or InStr(1, CLASS_NO, "DELETE", 1) > 0 Or InStr(1, CLASS_NO, "DROP", 1) > 0 Or InStr(1, CLASS_NO, "INSERT", 1) > 1 Or InStr(1, CLASS_NO, "TRACK", 1) > 1 Or InStr(1, CLASS_NO, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Label15.Text = "Do not use reserved Words!"
                        Label1.Text = ""
                        txt_Hold_ClassNo.Focus()
                        Exit Sub
                    End If
                    CLASS_NO = TrimX(CLASS_NO)
                    'check unwanted characters
                    c = 0
                    counter13 = 0
                    For iloop = 1 To Len(CLASS_NO)
                        strcurrentchar = Mid(CLASS_NO, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter13 = 1
                            End If
                        End If
                    Next
                    If counter13 = 1 Then
                        Label15.Text = " Input  is not Valid!"
                        Label1.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                        txt_Hold_ClassNo.Focus()
                        Exit Sub
                    End If
                Else
                    CLASS_NO = ""
                End If

                'validation for CLASS NO
                Dim BOOK_NO As Object = Nothing
                BOOK_NO = TrimAll(UCase(txt_Hold_BookNo.Text))
                If Not String.IsNullOrEmpty(BOOK_NO) Then
                    BOOK_NO = RemoveQuotes(BOOK_NO)
                    If BOOK_NO.Length > 50 Then 'maximum length
                        Label15.Text = " Data must be of Proper Length.. "
                        Label1.Text = ""
                        txt_Hold_BookNo.Focus()
                        Exit Sub
                    End If

                    BOOK_NO = " " & BOOK_NO & " "
                    If InStr(1, BOOK_NO, "CREATE", 1) > 0 Or InStr(1, BOOK_NO, "DELETE", 1) > 0 Or InStr(1, BOOK_NO, "DROP", 1) > 0 Or InStr(1, BOOK_NO, "INSERT", 1) > 1 Or InStr(1, BOOK_NO, "TRACK", 1) > 1 Or InStr(1, BOOK_NO, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Label15.Text = "Do not use reserved Words!"
                        Label1.Text = ""
                        txt_Hold_BookNo.Focus()
                        Exit Sub
                    End If
                    BOOK_NO = TrimAll(BOOK_NO)
                    'check unwanted characters
                    c = 0
                    counter14 = 0
                    For iloop = 1 To Len(BOOK_NO)
                        strcurrentchar = Mid(BOOK_NO, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter14 = 1
                            End If
                        End If
                    Next
                    If counter14 = 1 Then
                        Label15.Text = " Input  is not Valid!"
                        Label1.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                        txt_Hold_BookNo.Focus()
                        Exit Sub
                    End If
                Else
                    BOOK_NO = ""
                End If

                'validation for PAGES
                Dim PAGINATION As Object = Nothing
                PAGINATION = TrimAll(txt_Hold_Pages.Text)
                If Not String.IsNullOrEmpty(PAGINATION) Then
                    PAGINATION = RemoveQuotes(PAGINATION)
                    If CLASS_NO.Length > 50 Then 'maximum length
                        Label15.Text = " Data must be of Proper Length.. "
                        Label1.Text = ""
                        txt_Hold_Pages.Focus()
                        Exit Sub
                    End If

                    PAGINATION = " " & PAGINATION & " "
                    If InStr(1, PAGINATION, "CREATE", 1) > 0 Or InStr(1, PAGINATION, "DELETE", 1) > 0 Or InStr(1, PAGINATION, "DROP", 1) > 0 Or InStr(1, PAGINATION, "INSERT", 1) > 1 Or InStr(1, PAGINATION, "TRACK", 1) > 1 Or InStr(1, PAGINATION, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Label15.Text = "Do not use reserved Words!"
                        Label1.Text = ""
                        txt_Hold_Pages.Focus()
                        Exit Sub
                    End If
                    PAGINATION = TrimAll(PAGINATION)
                    'check unwanted characters
                    c = 0
                    counter15 = 0
                    For iloop = 1 To Len(PAGINATION)
                        strcurrentchar = Mid(PAGINATION, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter15 = 1
                            End If
                        End If
                    Next
                    If counter15 = 1 Then
                        Label15.Text = " Input  is not Valid!"
                        Label1.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                        txt_Hold_Pages.Focus()
                        Exit Sub
                    End If
                    If InStr(PAGINATION, "p.") = 0 Then
                        PAGINATION = PAGINATION + "p."
                    End If
                    If InStr(PAGINATION, "..") <> 0 Then
                        PAGINATION = PAGINATION + "."
                    End If
                    If InStr(PAGINATION, "pp.") <> 0 Then
                        PAGINATION = PAGINATION + "p."
                    End If
                    If InStr(PAGINATION, "p.p.") <> 0 Then
                        PAGINATION = PAGINATION + "p."
                    End If
                Else
                    PAGINATION = ""
                End If

                'validation for SIZE
                Dim SIZE As Object = Nothing
                SIZE = TrimAll(txt_Hold_Size.Text)
                If Not String.IsNullOrEmpty(SIZE) Then
                    SIZE = RemoveQuotes(SIZE)
                    If SIZE.Length > 50 Then 'maximum length
                        Label15.Text = " Data must be of Proper Length.. "
                        Label1.Text = ""
                        txt_Hold_Size.Focus()
                        Exit Sub
                    End If

                    SIZE = " " & SIZE & " "
                    If InStr(1, SIZE, "CREATE", 1) > 0 Or InStr(1, SIZE, "DELETE", 1) > 0 Or InStr(1, SIZE, "DROP", 1) > 0 Or InStr(1, SIZE, "INSERT", 1) > 1 Or InStr(1, SIZE, "TRACK", 1) > 1 Or InStr(1, SIZE, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Label15.Text = "Do not use reserved Words!"
                        Label1.Text = ""
                        txt_Hold_Size.Focus()
                        Exit Sub
                    End If
                    SIZE = TrimAll(SIZE)
                    'check unwanted characters
                    c = 0
                    counter16 = 0
                    For iloop = 1 To Len(SIZE)
                        strcurrentchar = Mid(SIZE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter16 = 1
                            End If
                        End If
                    Next
                    If counter16 = 1 Then
                        Label15.Text = " Input  is not Valid!"
                        Label1.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                        txt_Hold_Size.Focus()
                        Exit Sub
                    End If
                Else
                    SIZE = ""
                End If

                Dim ILLUSTRATION As Object = Nothing
                If CB_Illus.Checked = True Then
                    ILLUSTRATION = "Y"
                Else
                    ILLUSTRATION = "N"
                End If

                'validation for COLLECTION TYPE
                Dim COLLECTION_TYPE As Object = Nothing
                COLLECTION_TYPE = DDL_CollectionType.SelectedValue
                If Not String.IsNullOrEmpty(COLLECTION_TYPE) Then
                    COLLECTION_TYPE = RemoveQuotes(COLLECTION_TYPE)
                    If COLLECTION_TYPE.Length > 2 Then 'maximum length
                        Label15.Text = " Data must be of Proper Length.. "
                        Label1.Text = ""
                        DDL_CollectionType.Focus()
                        Exit Sub
                    End If

                    COLLECTION_TYPE = " " & COLLECTION_TYPE & " "
                    If InStr(1, COLLECTION_TYPE, "CREATE", 1) > 0 Or InStr(1, COLLECTION_TYPE, "DELETE", 1) > 0 Or InStr(1, COLLECTION_TYPE, "DROP", 1) > 0 Or InStr(1, COLLECTION_TYPE, "INSERT", 1) > 1 Or InStr(1, COLLECTION_TYPE, "TRACK", 1) > 1 Or InStr(1, COLLECTION_TYPE, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Label15.Text = " Input  is not Valid!"
                        Label1.Text = ""
                        DDL_CollectionType.Focus()
                        Exit Sub
                    End If
                    COLLECTION_TYPE = TrimX(COLLECTION_TYPE)
                    'check unwanted characters
                    c = 0
                    counter17 = 0
                    For iloop = 1 To Len(COLLECTION_TYPE)
                        strcurrentchar = Mid(COLLECTION_TYPE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter17 = 1
                            End If
                        End If
                    Next
                    If counter17 = 1 Then
                        Label15.Text = " Input  is not Valid!"
                        Label1.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                        DDL_CollectionType.Focus()
                        Exit Sub
                    End If
                Else
                    COLLECTION_TYPE = ""
                End If

                ''validation for STATUS
                'Dim STA_CODE As Object = Nothing
                'STA_CODE = DDL_Status.SelectedValue
                'If Not String.IsNullOrEmpty(STA_CODE) Then
                '    STA_CODE = RemoveQuotes(STA_CODE)
                '    If STA_CODE.Length > 11 Then 'maximum length
                '        Label15.Text = " Data must be of Proper Length.. "
                '        Label1.Text = ""
                '        DDL_Status.Focus()
                '        Exit Sub
                '    End If

                '    STA_CODE = " " & STA_CODE & " "
                '    If InStr(1, STA_CODE, "CREATE", 1) > 0 Or InStr(1, STA_CODE, "DELETE", 1) > 0 Or InStr(1, STA_CODE, "DROP", 1) > 0 Or InStr(1, STA_CODE, "INSERT", 1) > 1 Or InStr(1, STA_CODE, "TRACK", 1) > 1 Or InStr(1, STA_CODE, "TRACE", 1) > 1 Then
                '        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                '        Label15.Text = " Input  is not Valid!"
                '        Label1.Text = ""
                '        DDL_Status.Focus()
                '        Exit Sub
                '    End If
                '    STA_CODE = TrimX(STA_CODE)
                '    'check unwanted characters
                '    c = 0
                '    counter18 = 0
                '    For iloop = 1 To Len(STA_CODE)
                '        strcurrentchar = Mid(STA_CODE, iloop, 1)
                '        If c = 0 Then
                '            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                '                c = c + 1
                '                counter18 = 1
                '            End If
                '        End If
                '    Next
                '    If counter18 = 1 Then
                '        Label15.Text = " Input  is not Valid!"
                '        Label1.Text = ""
                '        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                '        DDL_Status.Focus()
                '        Exit Sub
                '    End If
                'Else
                '    STA_CODE = "1"
                'End If

                'validation for BINDING TYPE
                Dim BIND_CODE As Object = Nothing
                BIND_CODE = DDL_Binding.SelectedValue
                If Not String.IsNullOrEmpty(BIND_CODE) Then
                    BIND_CODE = RemoveQuotes(BIND_CODE)
                    If BIND_CODE.Length > 11 Then 'maximum length
                        Label15.Text = " Data must be of Proper Length.. "
                        Label1.Text = ""
                        DDL_Binding.Focus()
                        Exit Sub
                    End If

                    BIND_CODE = " " & BIND_CODE & " "
                    If InStr(1, BIND_CODE, "CREATE", 1) > 0 Or InStr(1, BIND_CODE, "DELETE", 1) > 0 Or InStr(1, BIND_CODE, "DROP", 1) > 0 Or InStr(1, BIND_CODE, "INSERT", 1) > 1 Or InStr(1, BIND_CODE, "TRACK", 1) > 1 Or InStr(1, BIND_CODE, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Label15.Text = " Input  is not Valid!"
                        Label1.Text = ""
                        DDL_Binding.Focus()
                        Exit Sub
                    End If
                    BIND_CODE = TrimX(BIND_CODE)
                    'check unwanted characters
                    c = 0
                    counter18 = 0
                    For iloop = 1 To Len(BIND_CODE)
                        strcurrentchar = Mid(BIND_CODE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter18 = 1
                            End If
                        End If
                    Next
                    If counter18 = 1 Then
                        Label15.Text = " Input  is not Valid!"
                        Label1.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                        DDL_Binding.Focus()
                        Exit Sub
                    End If
                Else
                    BIND_CODE = "U"
                End If

                'validation for SECTION
                Dim SEC_CODE As Object = Nothing
                SEC_CODE = DDL_Section.SelectedValue
                If Not String.IsNullOrEmpty(SEC_CODE) Then
                    SEC_CODE = RemoveQuotes(SEC_CODE)
                    If SEC_CODE.Length > 11 Then 'maximum length
                        Label15.Text = " Data must be of Proper Length.. "
                        Label1.Text = ""
                        DDL_Section.Focus()
                        Exit Sub
                    End If

                    SEC_CODE = " " & SEC_CODE & " "
                    If InStr(1, SEC_CODE, "CREATE", 1) > 0 Or InStr(1, SEC_CODE, "DELETE", 1) > 0 Or InStr(1, SEC_CODE, "DROP", 1) > 0 Or InStr(1, SEC_CODE, "INSERT", 1) > 1 Or InStr(1, SEC_CODE, "TRACK", 1) > 1 Or InStr(1, SEC_CODE, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Label15.Text = " Input  is not Valid!"
                        Label1.Text = ""
                        DDL_Section.Focus()
                        Exit Sub
                    End If
                    SEC_CODE = TrimX(SEC_CODE)
                    'check unwanted characters
                    c = 0
                    counter19 = 0
                    For iloop = 1 To Len(SEC_CODE)
                        strcurrentchar = Mid(SEC_CODE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter19 = 1
                            End If
                        End If
                    Next
                    If counter19 = 1 Then
                        Label15.Text = " Input  is not Valid!"
                        Label1.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                        DDL_Section.Focus()
                        Exit Sub
                    End If
                Else
                    SEC_CODE = ""
                End If

                'validation for holding Library
                Dim HOLD_LIB_CODE As Object = Nothing
                HOLD_LIB_CODE = DDL_Library.SelectedValue
                If Not String.IsNullOrEmpty(HOLD_LIB_CODE) Then
                    HOLD_LIB_CODE = RemoveQuotes(HOLD_LIB_CODE)
                    If HOLD_LIB_CODE.Length > 11 Then 'maximum length
                        Label15.Text = " Data must be of Proper Length.. "
                        Label1.Text = ""
                        DDL_Library.Focus()
                        Exit Sub
                    End If

                    HOLD_LIB_CODE = " " & HOLD_LIB_CODE & " "
                    If InStr(1, HOLD_LIB_CODE, "CREATE", 1) > 0 Or InStr(1, HOLD_LIB_CODE, "DELETE", 1) > 0 Or InStr(1, HOLD_LIB_CODE, "DROP", 1) > 0 Or InStr(1, HOLD_LIB_CODE, "INSERT", 1) > 1 Or InStr(1, HOLD_LIB_CODE, "TRACK", 1) > 1 Or InStr(1, HOLD_LIB_CODE, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Label15.Text = " Input  is not Valid!"
                        Label1.Text = ""
                        DDL_Library.Focus()
                        Exit Sub
                    End If
                    HOLD_LIB_CODE = TrimX(HOLD_LIB_CODE)
                    'check unwanted characters
                    c = 0
                    counter20 = 0
                    For iloop = 1 To Len(HOLD_LIB_CODE)
                        strcurrentchar = Mid(HOLD_LIB_CODE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter20 = 1
                            End If
                        End If
                    Next
                    If counter20 = 1 Then
                        Label15.Text = " Input  is not Valid!"
                        Label1.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                        DDL_Library.Focus()
                        Exit Sub
                    End If
                Else
                    HOLD_LIB_CODE = LibCode
                End If

                'validation for accompanying materials
                Dim ACC_MAT_CODE As Object = Nothing
                ACC_MAT_CODE = DDL_AccMaterials.SelectedValue
                If Not String.IsNullOrEmpty(ACC_MAT_CODE) Then
                    ACC_MAT_CODE = RemoveQuotes(ACC_MAT_CODE)
                    If ACC_MAT_CODE.Length > 11 Then 'maximum length
                        Label15.Text = " Data must be of Proper Length.. "
                        Label1.Text = ""
                        DDL_AccMaterials.Focus()
                        Exit Sub
                    End If

                    ACC_MAT_CODE = " " & ACC_MAT_CODE & " "
                    If InStr(1, ACC_MAT_CODE, "CREATE", 1) > 0 Or InStr(1, ACC_MAT_CODE, "DELETE", 1) > 0 Or InStr(1, ACC_MAT_CODE, "DROP", 1) > 0 Or InStr(1, ACC_MAT_CODE, "INSERT", 1) > 1 Or InStr(1, ACC_MAT_CODE, "TRACK", 1) > 1 Or InStr(1, ACC_MAT_CODE, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Label15.Text = " Input  is not Valid!"
                        Label1.Text = ""
                        DDL_AccMaterials.Focus()
                        Exit Sub
                    End If
                    ACC_MAT_CODE = TrimX(ACC_MAT_CODE)
                    'check unwanted characters
                    c = 0
                    counter21 = 0
                    For iloop = 1 To Len(ACC_MAT_CODE)
                        strcurrentchar = Mid(ACC_MAT_CODE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter21 = 1
                            End If
                        End If
                    Next
                    If counter21 = 1 Then
                        Label15.Text = " Input  is not Valid!"
                        Label1.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                        DDL_AccMaterials.Focus()
                        Exit Sub
                    End If
                Else
                    ACC_MAT_CODE = ""
                End If

                'validation for REMARKS
                Dim REMARKS As Object = Nothing
                REMARKS = TrimAll(txt_Hold_Remarks.Text)
                If Not String.IsNullOrEmpty(REMARKS) Then
                    REMARKS = RemoveQuotes(REMARKS)
                    If REMARKS.Length > 251 Then 'maximum length
                        Label15.Text = " Data must be of Proper Length.. "
                        Label1.Text = ""
                        txt_Hold_Remarks.Focus()
                        Exit Sub
                    End If
                Else
                    REMARKS = ""
                End If

                'validation for Physical Location
                Dim PHYSICAL_LOCATION As Object = Nothing
                'If TR_REMARKS.Visible = True Then
                PHYSICAL_LOCATION = TrimAll(txt_Hold_Location.Text)
                If Not String.IsNullOrEmpty(PHYSICAL_LOCATION) Then
                    PHYSICAL_LOCATION = RemoveQuotes(PHYSICAL_LOCATION)
                    If PHYSICAL_LOCATION.Length > 50 Then 'maximum length
                        Label15.Text = " Data must be of Proper Length.. "
                        Label1.Text = ""
                        txt_Hold_Location.Focus()
                        Exit Sub
                    End If

                    PHYSICAL_LOCATION = " " & PHYSICAL_LOCATION & " "
                    If InStr(1, PHYSICAL_LOCATION, "CREATE", 1) > 0 Or InStr(1, PHYSICAL_LOCATION, "DELETE", 1) > 0 Or InStr(1, PHYSICAL_LOCATION, "DROP", 1) > 0 Or InStr(1, PHYSICAL_LOCATION, "INSERT", 1) > 1 Or InStr(1, PHYSICAL_LOCATION, "TRACK", 1) > 1 Or InStr(1, PHYSICAL_LOCATION, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Label15.Text = "Do not use reserved Words!"
                        Label1.Text = ""
                        txt_Hold_Location.Focus()
                        Exit Sub
                    End If
                    PHYSICAL_LOCATION = TrimAll(PHYSICAL_LOCATION)
                    'check unwanted characters
                    c = 0
                    counter22 = 0
                    For iloop = 1 To Len(PHYSICAL_LOCATION)
                        strcurrentchar = Mid(PHYSICAL_LOCATION, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter22 = 1
                            End If
                        End If
                    Next
                    If counter22 = 1 Then
                        Label15.Text = " Input  is not Valid!"
                        Label1.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                        txt_Hold_Location.Focus()
                        Exit Sub
                    End If
                Else
                    PHYSICAL_LOCATION = ""
                End If
                'Else
                'PHYSICAL_LOCATION = ""
                'End If

                'validation for REFERENCE NO
                Dim REFERENCE_NO As Object = Nothing
                ' If TR_REFERENCE_NO.Visible = True Then
                REFERENCE_NO = TrimAll(txt_Hold_ReferenceNo.Text)
                If Not String.IsNullOrEmpty(REFERENCE_NO) Then
                    REFERENCE_NO = RemoveQuotes(REFERENCE_NO)
                    If REFERENCE_NO.Length > 50 Then 'maximum length
                        Label15.Text = " Data must be of Proper Length.. "
                        Label1.Text = ""
                        txt_Hold_ReferenceNo.Focus()
                        Exit Sub
                    End If

                    REFERENCE_NO = " " & REFERENCE_NO & " "
                    If InStr(1, REFERENCE_NO, "CREATE", 1) > 0 Or InStr(1, REFERENCE_NO, "DELETE", 1) > 0 Or InStr(1, REFERENCE_NO, "DROP", 1) > 0 Or InStr(1, REFERENCE_NO, "INSERT", 1) > 1 Or InStr(1, REFERENCE_NO, "TRACK", 1) > 1 Or InStr(1, REFERENCE_NO, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Label15.Text = "Do not use reserved Words!"
                        Label1.Text = ""
                        txt_Hold_ReferenceNo.Focus()
                        Exit Sub
                    End If
                    REFERENCE_NO = TrimAll(REFERENCE_NO)
                    'check unwanted characters
                    c = 0
                    counter23 = 0
                    For iloop = 1 To Len(REFERENCE_NO)
                        strcurrentchar = Mid(REFERENCE_NO, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter23 = 1
                            End If
                        End If
                    Next
                    If counter23 = 1 Then
                        Label15.Text = " Input  is not Valid!"
                        Label1.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                        txt_Hold_ReferenceNo.Focus()
                        Exit Sub
                    End If
                Else
                    REFERENCE_NO = ""
                End If
                'Else
                'REFERENCE_NO = ""
                'End If

                'validation for RECODING_MEDIUM
                Dim RECORDING_MEDIUM As Object = Nothing
                ' If TR_MEDIUM.Visible = True Then
                RECORDING_MEDIUM = TrimAll(txt_Hold_RecordingMedium.Text)
                If Not String.IsNullOrEmpty(RECORDING_MEDIUM) Then
                    RECORDING_MEDIUM = RemoveQuotes(RECORDING_MEDIUM)
                    If RECORDING_MEDIUM.Length > 51 Then 'maximum length
                        Label15.Text = " Data must be of Proper Length.. "
                        Label1.Text = ""
                        txt_Hold_RecordingMedium.Focus()
                        Exit Sub
                    End If
                Else
                    RECORDING_MEDIUM = ""
                End If
                'Else
                'RECORDING_MEDIUM = ""
                'End If

                'validation for RECODING_MEDIUM
                Dim RECORDING_CATEGORY As Object = Nothing
                ' If TR_RECORDING_CATEGORY.Visible = True Then
                RECORDING_CATEGORY = TrimAll(txt_Hold_RecordingCategory.Text)
                If Not String.IsNullOrEmpty(RECORDING_CATEGORY) Then
                    RECORDING_CATEGORY = RemoveQuotes(RECORDING_CATEGORY)
                    If RECORDING_CATEGORY.Length > 51 Then 'maximum length
                        Label15.Text = " Data must be of Proper Length.. "
                        Label1.Text = ""
                        txt_Hold_RecordingCategory.Focus()
                        Exit Sub
                    End If
                Else
                    RECORDING_CATEGORY = ""
                End If
                'Else
                'RECORDING_CATEGORY = ""
                'End If

                'validation for RECORDING_FORM
                Dim RECORDING_FORM As Object = Nothing
                'If TR_RECORDING_FORM.Visible = True Then
                RECORDING_FORM = TrimAll(txt_Hold_RecordingForm.Text)
                If Not String.IsNullOrEmpty(RECORDING_FORM) Then
                    RECORDING_FORM = RemoveQuotes(RECORDING_FORM)
                    If RECORDING_FORM.Length > 51 Then 'maximum length
                        Label15.Text = " Data must be of Proper Length.. "
                        Label1.Text = ""
                        txt_Hold_RecordingForm.Focus()
                        Exit Sub
                    End If
                Else
                    RECORDING_FORM = ""
                End If
                'Else
                'RECORDING_FORM = ""
                'End If

                'validation for RECORDING_FORMAT
                Dim RECORDING_FORMAT As Object = Nothing
                'If TR_RECORDING_FORMAT.Visible = True Then
                RECORDING_FORMAT = TrimAll(txt_Hold_RecordingFormat.Text)
                If Not String.IsNullOrEmpty(RECORDING_FORMAT) Then
                    RECORDING_FORMAT = RemoveQuotes(RECORDING_FORMAT)
                    If RECORDING_FORMAT.Length > 51 Then 'maximum length
                        Label15.Text = " Data must be of Proper Length.. "
                        Label1.Text = ""
                        txt_Hold_RecordingFormat.Focus()
                        Exit Sub
                    End If
                Else
                    RECORDING_FORMAT = ""
                End If
                'Else
                'RECORDING_FORMAT = ""
                'End If

                'validation for RECORDING_SPEED
                Dim RECORDING_SPEED As Object = Nothing
                ' If TR_RECORDING_SPEED.Visible = True Then
                RECORDING_SPEED = TrimAll(txt_Hold_RecordingSpeed.Text)
                If Not String.IsNullOrEmpty(RECORDING_SPEED) Then
                    RECORDING_SPEED = RemoveQuotes(RECORDING_SPEED)
                    If RECORDING_SPEED.Length > 51 Then 'maximum length
                        Label15.Text = " Data must be of Proper Length.. "
                        Label1.Text = ""
                        txt_Hold_RecordingSpeed.Focus()
                        Exit Sub
                    End If
                Else
                    RECORDING_SPEED = ""
                End If
                'Else
                'RECORDING_SPEED = ""
                'End If

                'validation for RECORDING_STORAGE_TECH
                Dim RECORDING_STORAGE_TECH As Object = Nothing
                'If TR_RECORDING_STORAGE_TECH.Visible = True Then
                RECORDING_STORAGE_TECH = TrimAll(txt_Hold_RecordingStorageTech.Text)
                If Not String.IsNullOrEmpty(RECORDING_STORAGE_TECH) Then
                    RECORDING_STORAGE_TECH = RemoveQuotes(RECORDING_STORAGE_TECH)
                    If RECORDING_STORAGE_TECH.Length > 51 Then 'maximum length
                        Label15.Text = " Data must be of Proper Length.. "
                        Label1.Text = ""
                        txt_Hold_RecordingStorageTech.Focus()
                        Exit Sub
                    End If
                Else
                    RECORDING_STORAGE_TECH = ""
                End If
                'Else
                'RECORDING_STORAGE_TECH = ""
                'End If

                'validation for RECORDING_PLAY_DURATION
                Dim RECORDING_PLAY_DURATION As Object = Nothing
                'If TR_RECORDING_PLAY_DURATION.Visible = True Then
                RECORDING_PLAY_DURATION = TrimAll(txt_Hold_RecordingDuration.Text)
                If Not String.IsNullOrEmpty(RECORDING_PLAY_DURATION) Then
                    RECORDING_PLAY_DURATION = RemoveQuotes(RECORDING_PLAY_DURATION)
                    If RECORDING_PLAY_DURATION.Length > 51 Then 'maximum length
                        Label15.Text = " Data must be of Proper Length.. "
                        Label1.Text = ""
                        txt_Hold_RecordingDuration.Focus()
                        Exit Sub
                    End If
                Else
                    RECORDING_PLAY_DURATION = ""
                End If
                'Else
                'RECORDING_PLAY_DURATION = ""
                'End If

                'validation for TYPE OF VISUALS
                Dim VIDEO_TYPEOFVISUALS As Object = Nothing
                ' If TR_VIDEO_TYPEOFVISUAL.Visible = True Then
                VIDEO_TYPEOFVISUALS = TrimAll(txt_Hold_TypeOfVisuals.Text)
                If Not String.IsNullOrEmpty(VIDEO_TYPEOFVISUALS) Then
                    VIDEO_TYPEOFVISUALS = RemoveQuotes(VIDEO_TYPEOFVISUALS)
                    If VIDEO_TYPEOFVISUALS.Length > 51 Then 'maximum length
                        Label15.Text = " Data must be of Proper Length.. "
                        Label1.Text = ""
                        txt_Hold_TypeOfVisuals.Focus()
                        Exit Sub
                    End If
                Else
                    VIDEO_TYPEOFVISUALS = ""
                End If
                'Else
                'VIDEO_TYPEOFVISUALS = ""
                'End If

                'validation for CARTOGRAPHIC_SCALE
                Dim CARTOGRAPHIC_SCALE As Object = Nothing
                'If TR_CARTOGRAPHIC_SCALE.Visible = True Then
                CARTOGRAPHIC_SCALE = TrimAll(txt_Hold_Scale.Text)
                If Not String.IsNullOrEmpty(CARTOGRAPHIC_SCALE) Then
                    CARTOGRAPHIC_SCALE = RemoveQuotes(CARTOGRAPHIC_SCALE)
                    If CARTOGRAPHIC_SCALE.Length > 51 Then 'maximum length
                        Label15.Text = " Data must be of Proper Length.. "
                        Label1.Text = ""
                        txt_Hold_Scale.Focus()
                        Exit Sub
                    End If
                Else
                    CARTOGRAPHIC_SCALE = ""
                End If
                'Else
                'CARTOGRAPHIC_SCALE = ""
                'End If

                'validation for CARTOGRAPHIC_PROJECTION
                Dim CARTOGRAPHIC_PROJECTION As Object = Nothing
                'If TR_CARTOGRAPHIC_PROJECTION.Visible = True Then
                CARTOGRAPHIC_PROJECTION = TrimAll(txt_Hold_Projection.Text)
                If Not String.IsNullOrEmpty(CARTOGRAPHIC_PROJECTION) Then
                    CARTOGRAPHIC_PROJECTION = RemoveQuotes(CARTOGRAPHIC_PROJECTION)
                    If CARTOGRAPHIC_PROJECTION.Length > 51 Then 'maximum length
                        Label15.Text = " Data must be of Proper Length.. "
                        Label1.Text = ""
                        txt_Hold_Projection.Focus()
                        Exit Sub
                    End If
                Else
                    CARTOGRAPHIC_PROJECTION = ""
                End If
                'Else
                'CARTOGRAPHIC_PROJECTION = ""
                'End If

                'validation for CARTOGRAPHIC_COORDINATES
                Dim CARTOGRAPHIC_COORDINATES As Object = Nothing
                ' If TR_CARTOGRAPHIC_COORDINATES.Visible = True Then
                CARTOGRAPHIC_COORDINATES = TrimAll(txt_Hold_Coordinates.Text)
                If Not String.IsNullOrEmpty(CARTOGRAPHIC_COORDINATES) Then
                    CARTOGRAPHIC_COORDINATES = RemoveQuotes(CARTOGRAPHIC_COORDINATES)
                    If CARTOGRAPHIC_COORDINATES.Length > 51 Then 'maximum length
                        Label15.Text = " Data must be of Proper Length.. "
                        Label1.Text = ""
                        txt_Hold_Coordinates.Focus()
                        Exit Sub
                    End If
                Else
                    CARTOGRAPHIC_COORDINATES = ""
                End If
                'Else
                'CARTOGRAPHIC_COORDINATES = ""
                'End If


                'validation for CARTOGRAPHIC_GEOGRAPHIC_LOCATION
                Dim CARTOGRAPHIC_GEOGRAPHIC_LOCATION As Object = Nothing
                'If TR_CARTOGRAPHIC_GEOGRAPHIC_LOCATION.Visible = True Then
                CARTOGRAPHIC_COORDINATES = TrimAll(txt_Hold_GeographicLocation.Text)
                If Not String.IsNullOrEmpty(CARTOGRAPHIC_GEOGRAPHIC_LOCATION) Then
                    CARTOGRAPHIC_GEOGRAPHIC_LOCATION = RemoveQuotes(CARTOGRAPHIC_GEOGRAPHIC_LOCATION)
                    If CARTOGRAPHIC_GEOGRAPHIC_LOCATION.Length > 51 Then 'maximum length
                        Label15.Text = " Data must be of Proper Length.. "
                        Label1.Text = ""
                        txt_Hold_GeographicLocation.Focus()
                        Exit Sub
                    End If
                Else
                    CARTOGRAPHIC_GEOGRAPHIC_LOCATION = ""
                End If
                'Else
                'CARTOGRAPHIC_GEOGRAPHIC_LOCATION = ""
                'End If

                'validation for CARTOGRAPHIC_MEDIUM
                Dim CARTOGRAPHIC_MEDIUM As Object = Nothing
                'If TR_CARTOGRAPHIC_MEDIUM.Visible = True Then
                CARTOGRAPHIC_MEDIUM = Trim(DDL_GeographicMedium.SelectedValue)
                If Not String.IsNullOrEmpty(CARTOGRAPHIC_MEDIUM) Then
                    CARTOGRAPHIC_MEDIUM = RemoveQuotes(CARTOGRAPHIC_MEDIUM)
                    If CARTOGRAPHIC_MEDIUM.Length > 50 Then 'maximum length
                        Label15.Text = " Data must be of Proper Length.. "
                        Label1.Text = ""
                        DDL_GeographicMedium.Focus()
                        Exit Sub
                    End If
                Else
                    CARTOGRAPHIC_MEDIUM = ""
                End If
                'Else
                'CARTOGRAPHIC_MEDIUM = ""
                'End If

                'search CARTOGRAPHIC_DATAGATHERING_DATE
                Dim CARTOGRAPHIC_DATAGATHERING_DATE As Object = Nothing
                'If TR_CARTOGRAPHIC_DATAGATHERING_DATE.Visible = True Then
                If txt_Hold_DataGatheringDate.Text <> "" Then
                    CARTOGRAPHIC_DATAGATHERING_DATE = TrimX(txt_Hold_DataGatheringDate.Text)
                    CARTOGRAPHIC_DATAGATHERING_DATE = RemoveQuotes(CARTOGRAPHIC_DATAGATHERING_DATE)
                    CARTOGRAPHIC_DATAGATHERING_DATE = Convert.ToDateTime(CARTOGRAPHIC_DATAGATHERING_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                    If Len(CARTOGRAPHIC_DATAGATHERING_DATE) > 12 Then
                        Label15.Text = " Input is not Valid..."
                        Label1.Text = ""
                        Me.txt_Hold_DataGatheringDate.Focus()
                        Exit Sub
                    End If
                Else
                    CARTOGRAPHIC_DATAGATHERING_DATE = ""
                End If
                'Else
                'CARTOGRAPHIC_DATAGATHERING_DATE = ""
                'End If

                'search CARTOGRAPHIC_CREATION_DATE
                Dim CARTOGRAPHIC_CREATION_DATE As Object = Nothing
                'If TR_CREATION_DATE.Visible = True Then
                If txt_Hold_CreationDate.Text <> "" Then
                    CARTOGRAPHIC_CREATION_DATE = TrimX(txt_Hold_CreationDate.Text)
                    CARTOGRAPHIC_CREATION_DATE = RemoveQuotes(CARTOGRAPHIC_CREATION_DATE)
                    CARTOGRAPHIC_CREATION_DATE = Convert.ToDateTime(CARTOGRAPHIC_CREATION_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                    If Len(CARTOGRAPHIC_CREATION_DATE) > 12 Then
                        Label15.Text = " Input is not Valid..."
                        Label1.Text = ""
                        Me.txt_Hold_CreationDate.Focus()
                        Exit Sub
                    End If
                Else
                    CARTOGRAPHIC_CREATION_DATE = ""
                End If
                'Else
                'CARTOGRAPHIC_CREATION_DATE = ""
                'End If

                'search CARTOGRAPHIC_COMPILATION_DATE
                Dim CARTOGRAPHIC_COMPILATION_DATE As Object = Nothing
                'If TR_CARTOGRAPHIC_COMPILATION_DATE.Visible = True Then
                If txt_Hold_CompilationDate.Text <> "" Then
                    CARTOGRAPHIC_COMPILATION_DATE = TrimX(txt_Hold_CompilationDate.Text)
                    CARTOGRAPHIC_COMPILATION_DATE = RemoveQuotes(CARTOGRAPHIC_COMPILATION_DATE)
                    CARTOGRAPHIC_COMPILATION_DATE = Convert.ToDateTime(CARTOGRAPHIC_COMPILATION_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                    If Len(CARTOGRAPHIC_COMPILATION_DATE) > 12 Then
                        Label15.Text = " Input is not Valid..."
                        Label1.Text = ""
                        Me.txt_Hold_CompilationDate.Focus()
                        Exit Sub
                    End If
                Else
                    CARTOGRAPHIC_COMPILATION_DATE = ""
                End If
                'Else
                'CARTOGRAPHIC_COMPILATION_DATE = ""
                'End If

                'validation for  CARTOGRAPHIC_INSPECTION_DATE
                Dim CARTOGRAPHIC_INSPECTION_DATE As Object = Nothing
                ' If TR_CARTOGRAPHIC_INSPECTION_DATE.Visible = True Then
                If txt_Hold_InspectionDate.Text <> "" Then
                    CARTOGRAPHIC_INSPECTION_DATE = TrimX(txt_Hold_InspectionDate.Text)
                    CARTOGRAPHIC_INSPECTION_DATE = RemoveQuotes(CARTOGRAPHIC_INSPECTION_DATE)
                    CARTOGRAPHIC_INSPECTION_DATE = Convert.ToDateTime(CARTOGRAPHIC_INSPECTION_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                    If Len(CARTOGRAPHIC_INSPECTION_DATE) > 12 Then
                        Label15.Text = " Input is not Valid..."
                        Label1.Text = ""
                        Me.txt_Hold_InspectionDate.Focus()
                        Exit Sub
                    End If
                Else
                    CARTOGRAPHIC_INSPECTION_DATE = ""
                End If
                'Else
                'CARTOGRAPHIC_INSPECTION_DATE = ""
                'End If





                'validation for  VIDEO_COLOR
                Dim VIDEO_COLOR As Object = Nothing
                If TR_VIDEO_COLOR.Visible = True Then
                    If txt_Hold_Color.Text <> "" Then
                        VIDEO_COLOR = TrimAll(txt_Hold_Color.Text)
                        VIDEO_COLOR = RemoveQuotes(VIDEO_COLOR)

                        If Len(VIDEO_COLOR) > 50 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_Color.Focus()
                            Exit Sub
                        End If
                    Else
                        VIDEO_COLOR = ""
                    End If
                Else
                    VIDEO_COLOR = ""
                End If


                'validation for  PLAYBACK_CHANNELS
                Dim PLAYBACK_CHANNELS As Object = Nothing
                If TR_PLAYBACK_CHANNELS.Visible = True Then
                    If txt_Hold_PlayBackChannel.Text <> "" Then
                        PLAYBACK_CHANNELS = TrimAll(txt_Hold_PlayBackChannel.Text)
                        PLAYBACK_CHANNELS = RemoveQuotes(PLAYBACK_CHANNELS)
                        If Len(PLAYBACK_CHANNELS) > 50 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_PlayBackChannel.Focus()
                            Exit Sub
                        End If
                    Else
                        PLAYBACK_CHANNELS = ""
                    End If
                Else
                    PLAYBACK_CHANNELS = ""
                End If


                'validation for  TAPE_WIDTH
                Dim TAPE_WIDTH As Object = Nothing
                If TR_TAPE_WIDTH.Visible = True Then
                    If txt_Hold_PlayBackChannel.Text <> "" Then
                        TAPE_WIDTH = TrimAll(txt_Hold_PlayBackChannel.Text)
                        TAPE_WIDTH = RemoveQuotes(TAPE_WIDTH)
                        If Len(TAPE_WIDTH) > 50 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_PlayBackChannel.Focus()
                            Exit Sub
                        End If
                    Else
                        TAPE_WIDTH = ""
                    End If
                Else
                    TAPE_WIDTH = ""
                End If

                'validation for  TAPE_CONFIGURATION
                Dim TAPE_CONFIGURATION As Object = Nothing
                If TR_TAPE_CONFIGURATION.Visible = True Then
                    If txt_Hold_TapeConfiguration.Text <> "" Then
                        TAPE_CONFIGURATION = TrimAll(txt_Hold_TapeConfiguration.Text)
                        TAPE_CONFIGURATION = RemoveQuotes(TAPE_CONFIGURATION)
                        If Len(TAPE_CONFIGURATION) > 50 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_TapeConfiguration.Focus()
                            Exit Sub
                        End If
                    Else
                        TAPE_CONFIGURATION = ""
                    End If
                Else
                    TAPE_CONFIGURATION = ""
                End If

                'validation for  KIND_OF_DISK
                Dim KIND_OF_DISK As Object = Nothing
                If TR_KIND_OF_DISK.Visible = True Then
                    If txt_Hold_KindofDisk.Text <> "" Then
                        KIND_OF_DISK = TrimAll(txt_Hold_KindofDisk.Text)
                        KIND_OF_DISK = RemoveQuotes(KIND_OF_DISK)
                        If Len(KIND_OF_DISK) > 50 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_KindofDisk.Focus()
                            Exit Sub
                        End If
                    Else
                        KIND_OF_DISK = ""
                    End If
                Else
                    KIND_OF_DISK = ""
                End If

                'validation for  KIND_OF_CUTTING
                Dim KIND_OF_CUTTING As Object = Nothing
                If TR_KIND_OF_CUTTING.Visible = True Then
                    If txt_Hold_KindofCutting.Text <> "" Then
                        KIND_OF_CUTTING = TrimAll(txt_Hold_KindofCutting.Text)
                        KIND_OF_CUTTING = RemoveQuotes(KIND_OF_CUTTING)
                        If Len(KIND_OF_DISK) > 50 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_KindofCutting.Focus()
                            Exit Sub
                        End If
                    Else
                        KIND_OF_CUTTING = ""
                    End If
                Else
                    KIND_OF_CUTTING = ""
                End If


                'validation for  ENCODING_STANDARD
                Dim ENCODING_STANDARD As Object = Nothing
                If TR_ENCODING_STANDARD.Visible = True Then
                    If txt_Hold_EncodingStandard.Text <> "" Then
                        ENCODING_STANDARD = TrimAll(txt_Hold_EncodingStandard.Text)
                        ENCODING_STANDARD = RemoveQuotes(ENCODING_STANDARD)
                        If Len(KIND_OF_DISK) > 50 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_EncodingStandard.Focus()
                            Exit Sub
                        End If
                    Else
                        ENCODING_STANDARD = ""
                    End If
                Else
                    ENCODING_STANDARD = ""
                End If

                'validation for  CAPTURE_TECHNIQUE
                Dim CAPTURE_TECHNIQUE As Object = Nothing
                If TR_CAPTURE_TECHNIQUE.Visible = True Then
                    If txt_Hold_CaptureTechnique.Text <> "" Then
                        CAPTURE_TECHNIQUE = TrimAll(txt_Hold_CaptureTechnique.Text)
                        CAPTURE_TECHNIQUE = RemoveQuotes(CAPTURE_TECHNIQUE)
                        If Len(CAPTURE_TECHNIQUE) > 50 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_CaptureTechnique.Focus()
                            Exit Sub
                        End If
                    Else
                        CAPTURE_TECHNIQUE = ""
                    End If
                Else
                    CAPTURE_TECHNIQUE = ""
                End If

                'validation for  PHOTO_NO
                Dim PHOTO_NO As Object = Nothing
                If TR_PHOTO_NO.Visible = True Then
                    If txt_Hold_PhotoNo.Text <> "" Then
                        PHOTO_NO = TrimAll(txt_Hold_PhotoNo.Text)
                        PHOTO_NO = RemoveQuotes(PHOTO_NO)
                        If Len(PHOTO_NO) > 50 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_PhotoNo.Focus()
                            Exit Sub
                        End If
                    Else
                        PHOTO_NO = ""
                    End If
                Else
                    PHOTO_NO = ""
                End If


                'validation for  PHOTO_ALBUM_NO
                Dim PHOTO_ALBUM_NO As Object = Nothing
                If TR_PHOTO_ALBUM_NO.Visible = True Then
                    If txt_Hold_PhotoAlbumNo.Text <> "" Then
                        PHOTO_ALBUM_NO = TrimAll(txt_Hold_PhotoAlbumNo.Text)
                        PHOTO_ALBUM_NO = RemoveQuotes(PHOTO_ALBUM_NO)
                        If Len(PHOTO_ALBUM_NO) > 150 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_PhotoAlbumNo.Focus()
                            Exit Sub
                        End If
                    Else
                        PHOTO_ALBUM_NO = ""
                    End If
                Else
                    PHOTO_ALBUM_NO = ""
                End If

                'validation for  PHOTO_OCASION
                Dim PHOTO_OCASION As Object = Nothing
                If TR_PHOTO_OCASION.Visible = True Then
                    If txt_Hold_Ocasion.Text <> "" Then
                        PHOTO_OCASION = TrimAll(txt_Hold_Ocasion.Text)
                        PHOTO_OCASION = RemoveQuotes(PHOTO_OCASION)
                        If Len(PHOTO_OCASION) > 150 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_Ocasion.Focus()
                            Exit Sub
                        End If
                    Else
                        PHOTO_OCASION = ""
                    End If
                Else
                    PHOTO_OCASION = ""
                End If

                'validation for  IMAGE_VIEW_TYPE
                Dim IMAGE_VIEW_TYPE As Object = Nothing
                If TR_IMAGE_VIEW_TYPE.Visible = True Then
                    If txt_Hold_ImageViewType.Text <> "" Then
                        IMAGE_VIEW_TYPE = TrimAll(txt_Hold_ImageViewType.Text)
                        IMAGE_VIEW_TYPE = RemoveQuotes(IMAGE_VIEW_TYPE)
                        If Len(IMAGE_VIEW_TYPE) > 50 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_ImageViewType.Focus()
                            Exit Sub
                        End If
                    Else
                        IMAGE_VIEW_TYPE = ""
                    End If
                Else
                    IMAGE_VIEW_TYPE = ""
                End If

                'search VIEW_DATE
                Dim VIEW_DATE As Object = Nothing
                If TR_VIEW_DATE.Visible = True Then
                    If txt_Hold_ViewDate.Text <> "" Then
                        VIEW_DATE = TrimX(txt_Hold_ViewDate.Text)
                        VIEW_DATE = RemoveQuotes(VIEW_DATE)
                        VIEW_DATE = Convert.ToDateTime(VIEW_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                        If Len(VIEW_DATE) > 12 Then
                            Label15.Text = " Input is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_ViewDate.Focus()
                            Exit Sub
                        End If
                    Else
                        VIEW_DATE = ""
                    End If
                Else
                    VIEW_DATE = ""
                End If


                'validation for  THEME
                Dim THEME As Object = Nothing
                If TR_THEME.Visible = True Then
                    If txt_Hold_Theme.Text <> "" Then
                        THEME = TrimAll(txt_Hold_Theme.Text)
                        THEME = RemoveQuotes(THEME)
                        If Len(THEME) > 150 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_Theme.Focus()
                            Exit Sub
                        End If
                    Else
                        THEME = ""
                    End If
                Else
                    THEME = ""
                End If

                'validation for  STYLE
                Dim STYLE As Object = Nothing
                If TR_STYLE.Visible = True Then
                    If txt_Hold_Style.Text <> "" Then
                        STYLE = TrimAll(txt_Hold_Style.Text)
                        STYLE = RemoveQuotes(STYLE)
                        If Len(STYLE) > 150 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_Style.Focus()
                            Exit Sub
                        End If
                    Else
                        STYLE = ""
                    End If
                Else
                    STYLE = ""
                End If

                'validation for  CULTURE
                Dim CULTURE As Object = Nothing
                If TR_CULTURE.Visible = True Then
                    If txt_Hold_Culture.Text <> "" Then
                        CULTURE = TrimAll(txt_Hold_Culture.Text)
                        CULTURE = RemoveQuotes(CULTURE)
                        If Len(CULTURE) > 150 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_Culture.Focus()
                            Exit Sub
                        End If
                    Else
                        CULTURE = ""
                    End If
                Else
                    CULTURE = ""
                End If

                'validation for  CURRENT_SITE
                Dim CURRENT_SITE As Object = Nothing
                If TR_CULTURE.Visible = True Then
                    If txt_Hold_CurrentSite.Text <> "" Then
                        CURRENT_SITE = TrimAll(txt_Hold_CurrentSite.Text)
                        CURRENT_SITE = RemoveQuotes(CURRENT_SITE)
                        If Len(CURRENT_SITE) > 150 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_CurrentSite.Focus()
                            Exit Sub
                        End If
                    Else
                        CURRENT_SITE = ""
                    End If
                Else
                    CURRENT_SITE = ""
                End If

                'validation for  CREATION_SITE
                Dim CREATION_SITE As Object = Nothing
                If TR_CREATION_SITE.Visible = True Then
                    If txt_Hold_CreationSite.Text <> "" Then
                        CREATION_SITE = TrimAll(txt_Hold_CreationSite.Text)
                        CREATION_SITE = RemoveQuotes(CREATION_SITE)
                        If Len(CREATION_SITE) > 150 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_CreationSite.Focus()
                            Exit Sub
                        End If
                    Else
                        CREATION_SITE = ""
                    End If
                Else
                    CREATION_SITE = ""
                End If

                'validation for  YARNCOUNT
                Dim YARNCOUNT As Object = Nothing
                If TR_YARNCOUNT.Visible = True Then
                    If txt_Hold_YarnCount.Text <> "" Then
                        YARNCOUNT = TrimAll(txt_Hold_YarnCount.Text)
                        YARNCOUNT = RemoveQuotes(YARNCOUNT)
                        If Len(YARNCOUNT) > 150 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_YarnCount.Focus()
                            Exit Sub
                        End If
                    Else
                        YARNCOUNT = ""
                    End If
                Else
                    YARNCOUNT = ""
                End If

                'validation for  MATERIAL_TYPE
                Dim MATERIAL_TYPE As Object = Nothing
                If TR_MATERIAL_TYPE.Visible = True Then
                    If txt_Hold_MaterialsType.Text <> "" Then
                        MATERIAL_TYPE = TrimAll(txt_Hold_MaterialsType.Text)
                        MATERIAL_TYPE = RemoveQuotes(MATERIAL_TYPE)
                        If Len(MATERIAL_TYPE) > 150 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_MaterialsType.Focus()
                            Exit Sub
                        End If
                    Else
                        MATERIAL_TYPE = ""
                    End If
                Else
                    MATERIAL_TYPE = ""
                End If

                'validation for  TECHNIQUE
                Dim TECHNIQUE As Object = Nothing
                If TR_TECHNIQUE.Visible = True Then
                    If txt_Hold_Technique.Text <> "" Then
                        TECHNIQUE = TrimAll(txt_Hold_Technique.Text)
                        TECHNIQUE = RemoveQuotes(TECHNIQUE)
                        If Len(TECHNIQUE) > 150 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_Technique.Focus()
                            Exit Sub
                        End If
                    Else
                        TECHNIQUE = ""
                    End If
                Else
                    TECHNIQUE = ""
                End If

                'validation for  TECH_DETAILS
                Dim TECH_DETAILS As Object = Nothing
                If TR_TECH_DETAILS.Visible = True Then
                    If txt_Hold_TechDetails.Text <> "" Then
                        TECH_DETAILS = TrimAll(txt_Hold_TechDetails.Text)
                        TECH_DETAILS = RemoveQuotes(TECH_DETAILS)
                        If Len(TECH_DETAILS) > 250 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_TechDetails.Focus()
                            Exit Sub
                        End If
                    Else
                        TECH_DETAILS = ""
                    End If
                Else
                    TECH_DETAILS = ""
                End If

                'validation for  INSCRIPTIONS
                Dim INSCRIPTIONS As Object = Nothing
                If TR_INSCRIPTIONS.Visible = True Then
                    If txt_Hold_Inscription.Text <> "" Then
                        INSCRIPTIONS = TrimAll(txt_Hold_Inscription.Text)
                        INSCRIPTIONS = RemoveQuotes(INSCRIPTIONS)
                        If Len(INSCRIPTIONS) > 150 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_Inscription.Focus()
                            Exit Sub
                        End If
                    Else
                        INSCRIPTIONS = ""
                    End If
                Else
                    INSCRIPTIONS = ""
                End If

                'validation for  DESCRIPTION
                Dim DESCRIPTION As Object = Nothing
                If TR_DESCRIPTION.Visible = True Then
                    If txt_Hold_Description.Text <> "" Then
                        DESCRIPTION = TrimAll(txt_Hold_Description.Text)
                        DESCRIPTION = RemoveQuotes(DESCRIPTION)
                        If Len(DESCRIPTION) > 250 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_Description.Focus()
                            Exit Sub
                        End If
                    Else
                        DESCRIPTION = ""
                    End If
                Else
                    DESCRIPTION = ""
                End If

                'validation for  GLOBE_TYPE
                Dim GLOBE_TYPE As Object = Nothing
                If TR_GLOBE_TYPE.Visible = True Then
                    If txt_Hold_GlobeType.Text <> "" Then
                        GLOBE_TYPE = TrimAll(txt_Hold_GlobeType.Text)
                        GLOBE_TYPE = RemoveQuotes(GLOBE_TYPE)
                        If Len(GLOBE_TYPE) > 50 Then
                            Label15.Text = " Input Length is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_GlobeType.Focus()
                            Exit Sub
                        End If
                    Else
                        GLOBE_TYPE = ""
                    End If
                Else
                    GLOBE_TYPE = ""
                End If

                'search ALTER_DATE
                Dim ALTER_DATE As Object = Nothing
                If TR_ALTER_DATE.Visible = True Then
                    If txt_Hold_AlterDate.Text <> "" Then
                        ALTER_DATE = TrimX(txt_Hold_AlterDate.Text)
                        ALTER_DATE = RemoveQuotes(ALTER_DATE)
                        ALTER_DATE = Convert.ToDateTime(ALTER_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                        If Len(ALTER_DATE) > 12 Then
                            Label15.Text = " Input is not Valid..."
                            Label1.Text = ""
                            Me.txt_Hold_AlterDate.Focus()
                            Exit Sub
                        End If
                    Else
                        ALTER_DATE = ""
                    End If
                Else
                    ALTER_DATE = ""
                End If

                Dim LIB_CODE As Object = Nothing
                If Session.Item("LoggedLibcode") <> "" Then
                    LIB_CODE = Session.Item("LoggedLibcode")
                Else
                    Label1.Text = "No Library Code Exists..Login Again  "
                    Label15.Text = ""
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('No Library Code Exists..Login Again !');", True)
                    Exit Sub
                End If

                Dim USER_CODE As Object = Nothing
                USER_CODE = Session.Item("LoggedUser")

                Dim DATE_MODIFIED As Object = Nothing
                DATE_MODIFIED = Now.Date
                DATE_MODIFIED = Convert.ToDateTime(DATE_MODIFIED, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                Dim IP As Object = Nothing
                IP = Request.UserHostAddress.Trim

                '*******************************************************************************************
                If Label_HOLDID.Text <> "" Then
                    If SqlConn.State = 0 Then
                        SqlConn.Open()
                    End If

                    SQL = "SELECT * FROM HOLDINGS WHERE (HOLD_ID='" & Trim(HOLD_ID) & "')"

                    da = New SqlDataAdapter(SQL, SqlConn)
                    cb = New SqlCommandBuilder(da)
                    da.Fill(ds, "HOLD")
                    If ds.Tables("HOLD").Rows.Count <> 0 Then
                        If Not String.IsNullOrEmpty(ACCESSION_NO) Then
                            ds.Tables("HOLD").Rows(0)("ACCESSION_NO") = ACCESSION_NO.Trim
                        Else
                            ds.Tables("HOLD").Rows(0)("ACCESSION_NO") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(ACCESSION_DATE) Then
                            ds.Tables("HOLD").Rows(0)("ACCESSION_DATE") = ACCESSION_DATE.ToString.Trim
                        Else
                            ds.Tables("HOLD").Rows(0)("ACCESSION_DATE") = System.DBNull.Value
                        End If

                        If TR_FORMAT_CODE.Visible = True Then
                            If Not String.IsNullOrEmpty(FORMAT_CODE) Then
                                ds.Tables("HOLD").Rows(0)("FORMAT_CODE") = FORMAT_CODE.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("FORMAT_CODE") = System.DBNull.Value
                            End If
                        End If

                        If Not String.IsNullOrEmpty(SHOW) Then
                            ds.Tables("HOLD").Rows(0)("SHOW") = SHOW.Trim
                        Else
                            ds.Tables("HOLD").Rows(0)("SHOW") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(ISSUABLE) Then
                            ds.Tables("HOLD").Rows(0)("ISSUEABLE") = ISSUABLE.Trim
                        Else
                            ds.Tables("HOLD").Rows(0)("ISSUEABLE") = System.DBNull.Value
                        End If

                        If TR_VOL_NO.Visible = True Then
                            If Not String.IsNullOrEmpty(VOL_NO) Then
                                ds.Tables("HOLD").Rows(0)("VOL_NO") = VOL_NO.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("VOL_NO") = System.DBNull.Value
                            End If
                        End If

                        If TR_VOL_YEAR.Visible = True Then
                            If Not String.IsNullOrEmpty(VOL_YEAR) Then
                                ds.Tables("HOLD").Rows(0)("VOL_YEAR") = VOL_YEAR.ToString.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("VOL_YEAR") = System.DBNull.Value
                            End If
                        End If

                        If TR_VOL_EDITORS.Visible = True Then
                            If Not String.IsNullOrEmpty(VOL_EDITORS) Then
                                ds.Tables("HOLD").Rows(0)("VOL_EDITORS") = VOL_EDITORS.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("VOL_EDITORS") = System.DBNull.Value
                            End If
                        End If

                        If TR_VOL_TITLE.Visible = True Then
                            If Not String.IsNullOrEmpty(VOL_TITLE) Then
                                ds.Tables("HOLD").Rows(0)("VOL_TITLE") = VOL_TITLE.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("VOL_TITLE") = System.DBNull.Value
                            End If
                        End If

                        If TR_COPY_ISBN.Visible = True Then
                            If Not String.IsNullOrEmpty(COPY_ISBN) Then
                                ds.Tables("HOLD").Rows(0)("COPY_ISBN") = COPY_ISBN.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("COPY_ISBN") = System.DBNull.Value
                            End If
                        End If

                        If TR_CLASS_NO.Visible = True Then
                            If Not String.IsNullOrEmpty(CLASS_NO) Then
                                ds.Tables("HOLD").Rows(0)("CLASS_NO") = CLASS_NO.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("CLASS_NO") = System.DBNull.Value
                            End If
                        End If

                        If TR_BOOK_NO.Visible = True Then
                            If Not String.IsNullOrEmpty(BOOK_NO) Then
                                ds.Tables("HOLD").Rows(0)("BOOK_NO") = BOOK_NO.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("BOOK_NO") = System.DBNull.Value
                            End If
                        End If

                        If TR_PAGINATION.Visible = True Then
                            If Not String.IsNullOrEmpty(PAGINATION) Then
                                ds.Tables("HOLD").Rows(0)("PAGINATION") = PAGINATION.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("PAGINATION") = System.DBNull.Value
                            End If
                        End If

                        If TR_SIZE.Visible = True Then
                            If Not String.IsNullOrEmpty(SIZE) Then
                                ds.Tables("HOLD").Rows(0)("SIZE") = SIZE.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("SIZE") = System.DBNull.Value
                            End If
                        End If

                        If TR_ILLUSTRATION.Visible = True Then
                            If Not String.IsNullOrEmpty(ILLUSTRATION) Then
                                ds.Tables("HOLD").Rows(0)("ILLUSTRATION") = ILLUSTRATION.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("ILLUSTRATION") = System.DBNull.Value
                            End If
                        End If

                        If TR_COLLECTION_TYPE.Visible = True Then
                            If Not String.IsNullOrEmpty(COLLECTION_TYPE) Then
                                ds.Tables("HOLD").Rows(0)("COLLECTION_TYPE") = COLLECTION_TYPE.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("COLLECTION_TYPE") = System.DBNull.Value
                            End If
                        End If

                        'If TR_STA_CODE.Visible = True Then
                        '    If Not String.IsNullOrEmpty(STA_CODE) Then
                        '        ds.Tables("HOLD").Rows(0)("STA_CODE") = STA_CODE.Trim
                        '    Else
                        '        ds.Tables("HOLD").Rows(0)("STA_CODE") = System.DBNull.Value
                        '    End If
                        'End If

                        If TR_BIND_CODE.Visible = True Then
                            If Not String.IsNullOrEmpty(BIND_CODE) Then
                                ds.Tables("HOLD").Rows(0)("BIND_CODE") = BIND_CODE.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("BIND_CODE") = System.DBNull.Value
                            End If
                        End If

                        If TR_SEC_CODE.Visible = True Then
                            If Not String.IsNullOrEmpty(SEC_CODE) Then
                                ds.Tables("HOLD").Rows(0)("SEC_CODE") = SEC_CODE.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("SEC_CODE") = System.DBNull.Value
                            End If
                        End If

                        If Not String.IsNullOrEmpty(HOLD_LIB_CODE) Then
                            ds.Tables("HOLD").Rows(0)("LIB_CODE") = HOLD_LIB_CODE.Trim
                        Else
                            ds.Tables("HOLD").Rows(0)("LIB_CODE") = LIB_CODE.Trim
                        End If

                        If TR_ACC_MAT_CODE.Visible = True Then
                            If Not String.IsNullOrEmpty(ACC_MAT_CODE) Then
                                ds.Tables("HOLD").Rows(0)("ACC_MAT_CODE") = ACC_MAT_CODE.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("ACC_MAT_CODE") = System.DBNull.Value
                            End If
                        End If

                        If TR_REMARKS.Visible = True Then
                            If Not String.IsNullOrEmpty(REMARKS) Then
                                ds.Tables("HOLD").Rows(0)("REMARKS") = REMARKS.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("REMARKS") = System.DBNull.Value
                            End If
                        End If

                        If TR_PHYSICAL_LOCATION.Visible = True Then
                            If Not String.IsNullOrEmpty(PHYSICAL_LOCATION) Then
                                ds.Tables("HOLD").Rows(0)("PHYSICAL_LOCATION") = PHYSICAL_LOCATION.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("PHYSICAL_LOCATION") = System.DBNull.Value
                            End If
                        End If

                        If TR_REFERENCE_NO.Visible = True Then
                            If Not String.IsNullOrEmpty(REFERENCE_NO) Then
                                ds.Tables("HOLD").Rows(0)("REFERENCE_NO") = REFERENCE_NO.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("REFERENCE_NO") = System.DBNull.Value
                            End If
                        End If

                        If TR_MEDIUM.Visible = True Then
                            If Not String.IsNullOrEmpty(RECORDING_MEDIUM) Then
                                ds.Tables("HOLD").Rows(0)("MEDIUM") = RECORDING_MEDIUM.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("MEDIUM") = System.DBNull.Value
                            End If
                        End If

                        If TR_RECORDING_CATEGORY.Visible = True Then
                            If Not String.IsNullOrEmpty(RECORDING_CATEGORY) Then
                                ds.Tables("HOLD").Rows(0)("RECORDING_CATEGORY") = RECORDING_CATEGORY.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("RECORDING_CATEGORY") = System.DBNull.Value
                            End If
                        End If

                        If TR_RECORDING_FORM.Visible = True Then
                            If Not String.IsNullOrEmpty(RECORDING_FORM) Then
                                ds.Tables("HOLD").Rows(0)("RECORDING_FORM") = RECORDING_FORM.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("RECORDING_FORM") = System.DBNull.Value
                            End If
                        End If

                        If TR_RECORDING_FORMAT.Visible = True Then
                            If Not String.IsNullOrEmpty(RECORDING_FORMAT) Then
                                ds.Tables("HOLD").Rows(0)("RECORDING_FORMAT") = RECORDING_FORMAT.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("RECORDING_FORMAT") = System.DBNull.Value
                            End If
                        End If

                        If TR_RECORDING_SPEED.Visible = True Then
                            If Not String.IsNullOrEmpty(RECORDING_SPEED) Then
                                ds.Tables("HOLD").Rows(0)("RECORDING_SPEED") = RECORDING_SPEED.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("RECORDING_SPEED") = System.DBNull.Value
                            End If
                        End If

                        If TR_RECORDING_STORAGE_TECH.Visible = True Then
                            If Not String.IsNullOrEmpty(RECORDING_STORAGE_TECH) Then
                                ds.Tables("HOLD").Rows(0)("RECORDING_STORAGE_TECH") = RECORDING_STORAGE_TECH.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("RECORDING_STORAGE_TECH") = System.DBNull.Value
                            End If
                        End If

                        If TR_RECORDING_PLAY_DURATION.Visible = True Then
                            If Not String.IsNullOrEmpty(RECORDING_PLAY_DURATION) Then
                                ds.Tables("HOLD").Rows(0)("RECORDING_PLAY_DURATION") = RECORDING_PLAY_DURATION.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("RECORDING_PLAY_DURATION") = System.DBNull.Value
                            End If
                        End If

                        If TR_VIDEO_TYPEOFVISUAL.Visible = True Then
                            If Not String.IsNullOrEmpty(VIDEO_TYPEOFVISUALS) Then
                                ds.Tables("HOLD").Rows(0)("VIDEO_TYPEOFVISUALS") = VIDEO_TYPEOFVISUALS.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("VIDEO_TYPEOFVISUALS") = System.DBNull.Value
                            End If
                        End If

                        If TR_CARTOGRAPHIC_SCALE.Visible = True Then
                            If Not String.IsNullOrEmpty(CARTOGRAPHIC_SCALE) Then
                                ds.Tables("HOLD").Rows(0)("CARTOGRAPHIC_SCALE") = CARTOGRAPHIC_SCALE.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("CARTOGRAPHIC_SCALE") = System.DBNull.Value
                            End If
                        End If

                        If TR_CARTOGRAPHIC_PROJECTION.Visible = True Then
                            If Not String.IsNullOrEmpty(CARTOGRAPHIC_PROJECTION) Then
                                ds.Tables("HOLD").Rows(0)("CARTOGRAPHIC_PROJECTION") = CARTOGRAPHIC_PROJECTION.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("CARTOGRAPHIC_PROJECTION") = System.DBNull.Value
                            End If
                        End If

                        If TR_CARTOGRAPHIC_COORDINATES.Visible = True Then
                            If Not String.IsNullOrEmpty(CARTOGRAPHIC_COORDINATES) Then
                                ds.Tables("HOLD").Rows(0)("CARTOGRAPHIC_COORDINATES") = CARTOGRAPHIC_COORDINATES.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("CARTOGRAPHIC_COORDINATES") = System.DBNull.Value
                            End If
                        End If

                        If TR_CARTOGRAPHIC_GEOGRAPHIC_LOCATION.Visible = True Then
                            If Not String.IsNullOrEmpty(CARTOGRAPHIC_GEOGRAPHIC_LOCATION) Then
                                ds.Tables("HOLD").Rows(0)("CARTOGRAPHIC_GEOGRAPHIC_LOCATION") = CARTOGRAPHIC_GEOGRAPHIC_LOCATION.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("CARTOGRAPHIC_GEOGRAPHIC_LOCATION") = System.DBNull.Value
                            End If
                        End If

                        If TR_CARTOGRAPHIC_MEDIUM.Visible = True Then
                            If Not String.IsNullOrEmpty(CARTOGRAPHIC_MEDIUM) Then
                                ds.Tables("HOLD").Rows(0)("CARTOGRAPHIC_MEDIUM") = CARTOGRAPHIC_MEDIUM.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("CARTOGRAPHIC_MEDIUM") = System.DBNull.Value
                            End If
                        End If

                        If TR_CARTOGRAPHIC_DATAGATHERING_DATE.Visible = True Then
                            If Not String.IsNullOrEmpty(CARTOGRAPHIC_DATAGATHERING_DATE) Then
                                ds.Tables("HOLD").Rows(0)("CARTOGRAPHIC_DATAGATHERING_DATE") = CARTOGRAPHIC_DATAGATHERING_DATE.ToString.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("CARTOGRAPHIC_DATAGATHERING_DATE") = System.DBNull.Value
                            End If
                        End If

                        If TR_CREATION_DATE.Visible = True Then
                            If Not String.IsNullOrEmpty(CARTOGRAPHIC_CREATION_DATE) Then
                                ds.Tables("HOLD").Rows(0)("CREATION_DATE") = CARTOGRAPHIC_CREATION_DATE.ToString.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("CREATION_DATE") = System.DBNull.Value
                            End If
                        End If

                        If TR_CARTOGRAPHIC_COMPILATION_DATE.Visible = True Then
                            If Not String.IsNullOrEmpty(CARTOGRAPHIC_COMPILATION_DATE) Then
                                ds.Tables("HOLD").Rows(0)("CARTOGRAPHIC_COMPILATION_DATE") = CARTOGRAPHIC_COMPILATION_DATE.ToString.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("CARTOGRAPHIC_COMPILATION_DATE") = System.DBNull.Value
                            End If
                        End If

                        If TR_CARTOGRAPHIC_INSPECTION_DATE.Visible = True Then
                            If Not String.IsNullOrEmpty(CARTOGRAPHIC_INSPECTION_DATE) Then
                                ds.Tables("HOLD").Rows(0)("CARTOGRAPHIC_INSPECTION_DATE") = CARTOGRAPHIC_INSPECTION_DATE.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("CARTOGRAPHIC_INSPECTION_DATE") = System.DBNull.Value
                            End If
                        End If

                        If TR_VIDEO_COLOR.Visible = True Then
                            If Not String.IsNullOrEmpty(VIDEO_COLOR) Then
                                ds.Tables("HOLD").Rows(0)("VIDEO_COLOR") = VIDEO_COLOR.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("VIDEO_COLOR") = System.DBNull.Value
                            End If
                        End If

                        If TR_PLAYBACK_CHANNELS.Visible = True Then
                            If Not String.IsNullOrEmpty(PLAYBACK_CHANNELS) Then
                                ds.Tables("HOLD").Rows(0)("PLAYBACK_CHANNELS") = PLAYBACK_CHANNELS.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("PLAYBACK_CHANNELS") = System.DBNull.Value
                            End If
                        End If

                        If TR_TAPE_WIDTH.Visible = True Then
                            If Not String.IsNullOrEmpty(TAPE_WIDTH) Then
                                ds.Tables("HOLD").Rows(0)("TAPE_WIDTH") = TAPE_WIDTH.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("TAPE_WIDTH") = System.DBNull.Value
                            End If
                        End If

                        If TR_TAPE_CONFIGURATION.Visible = True Then
                            If Not String.IsNullOrEmpty(TAPE_CONFIGURATION) Then
                                ds.Tables("HOLD").Rows(0)("TAPE_CONFIGURATION") = TAPE_CONFIGURATION.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("TAPE_CONFIGURATION") = System.DBNull.Value
                            End If
                        End If

                        If TR_KIND_OF_DISK.Visible = True Then
                            If Not String.IsNullOrEmpty(KIND_OF_DISK) Then
                                ds.Tables("HOLD").Rows(0)("KIND_OF_DISK") = KIND_OF_DISK.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("KIND_OF_DISK") = System.DBNull.Value
                            End If
                        End If

                        If TR_KIND_OF_CUTTING.Visible = True Then
                            If Not String.IsNullOrEmpty(KIND_OF_CUTTING) Then
                                ds.Tables("HOLD").Rows(0)("KIND_OF_CUTTING") = KIND_OF_CUTTING.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("KIND_OF_CUTTING") = System.DBNull.Value
                            End If
                        End If

                        If TR_ENCODING_STANDARD.Visible = True Then
                            If Not String.IsNullOrEmpty(ENCODING_STANDARD) Then
                                ds.Tables("HOLD").Rows(0)("ENCODING_STANDARD") = ENCODING_STANDARD.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("ENCODING_STANDARD") = System.DBNull.Value
                            End If
                        End If

                        If TR_CAPTURE_TECHNIQUE.Visible = True Then
                            If Not String.IsNullOrEmpty(CAPTURE_TECHNIQUE) Then
                                ds.Tables("HOLD").Rows(0)("CAPTURE_TECHNIQUE") = CAPTURE_TECHNIQUE.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("CAPTURE_TECHNIQUE") = System.DBNull.Value
                            End If
                        End If

                        If TR_PHOTO_NO.Visible = True Then
                            If Not String.IsNullOrEmpty(PHOTO_NO) Then
                                ds.Tables("HOLD").Rows(0)("PHOTO_NO") = PHOTO_NO.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("PHOTO_NO") = System.DBNull.Value
                            End If
                        End If

                        If TR_PHOTO_ALBUM_NO.Visible = True Then
                            If Not String.IsNullOrEmpty(PHOTO_ALBUM_NO) Then
                                ds.Tables("HOLD").Rows(0)("PHOTO_ALBUM_NO") = PHOTO_ALBUM_NO.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("PHOTO_ALBUM_NO") = System.DBNull.Value
                            End If
                        End If

                        If TR_PHOTO_OCASION.Visible = True Then
                            If Not String.IsNullOrEmpty(PHOTO_OCASION) Then
                                ds.Tables("HOLD").Rows(0)("PHOTO_OCASION") = PHOTO_OCASION.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("PHOTO_OCASION") = System.DBNull.Value
                            End If
                        End If

                        If TR_IMAGE_VIEW_TYPE.Visible = True Then
                            If Not String.IsNullOrEmpty(IMAGE_VIEW_TYPE) Then
                                ds.Tables("HOLD").Rows(0)("IMAGE_VIEW_TYPE") = IMAGE_VIEW_TYPE.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("IMAGE_VIEW_TYPE") = System.DBNull.Value
                            End If
                        End If

                        If TR_VIEW_DATE.Visible = True Then
                            If Not String.IsNullOrEmpty(VIEW_DATE) Then
                                ds.Tables("HOLD").Rows(0)("VIEW_DATE") = VIEW_DATE.ToString.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("VIEW_DATE") = System.DBNull.Value
                            End If
                        End If

                        If TR_THEME.Visible = True Then
                            If Not String.IsNullOrEmpty(THEME) Then
                                ds.Tables("HOLD").Rows(0)("THEME") = THEME.ToString.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("THEME") = System.DBNull.Value
                            End If
                        End If

                        If TR_STYLE.Visible = True Then
                            If Not String.IsNullOrEmpty(STYLE) Then
                                ds.Tables("HOLD").Rows(0)("STYLE") = STYLE.ToString.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("STYLE") = System.DBNull.Value
                            End If
                        End If

                        If TR_CULTURE.Visible = True Then
                            If Not String.IsNullOrEmpty(CULTURE) Then
                                ds.Tables("HOLD").Rows(0)("CULTURE") = CULTURE.ToString.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("CULTURE") = System.DBNull.Value
                            End If
                        End If

                        If TR_CURRENT_STIE.Visible = True Then
                            If Not String.IsNullOrEmpty(CURRENT_SITE) Then
                                ds.Tables("HOLD").Rows(0)("CURRENT_SITE") = CURRENT_SITE.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("CURRENT_SITE") = System.DBNull.Value
                            End If
                        End If

                        If TR_CREATION_SITE.Visible = True Then
                            If Not String.IsNullOrEmpty(CREATION_SITE) Then
                                ds.Tables("HOLD").Rows(0)("CREATION_SITE") = CREATION_SITE.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("CREATION_SITE") = System.DBNull.Value
                            End If
                        End If

                        If TR_YARNCOUNT.Visible = True Then
                            If Not String.IsNullOrEmpty(YARNCOUNT) Then
                                ds.Tables("HOLD").Rows(0)("YARNCOUNT") = YARNCOUNT.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("YARNCOUNT") = System.DBNull.Value
                            End If
                        End If

                        If TR_MATERIAL_TYPE.Visible = True Then
                            If Not String.IsNullOrEmpty(MATERIAL_TYPE) Then
                                ds.Tables("HOLD").Rows(0)("MATERIAL_TYPE") = MATERIAL_TYPE.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("MATERIAL_TYPE") = System.DBNull.Value
                            End If
                        End If

                        If TR_TECHNIQUE.Visible = True Then
                            If Not String.IsNullOrEmpty(TECHNIQUE) Then
                                ds.Tables("HOLD").Rows(0)("TECHNIQUE") = TECHNIQUE.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("TECHNIQUE") = System.DBNull.Value
                            End If
                        End If

                        If TR_TECH_DETAILS.Visible = True Then
                            If Not String.IsNullOrEmpty(TECH_DETAILS) Then
                                ds.Tables("HOLD").Rows(0)("TECH_DETAILS") = TECH_DETAILS.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("TECH_DETAILS") = System.DBNull.Value
                            End If
                        End If

                        If TR_INSCRIPTIONS.Visible = True Then
                            If Not String.IsNullOrEmpty(INSCRIPTIONS) Then
                                ds.Tables("HOLD").Rows(0)("INSCRIPTIONS") = INSCRIPTIONS.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("INSCRIPTIONS") = System.DBNull.Value
                            End If
                        End If

                        If TR_DESCRIPTION.Visible = True Then
                            If Not String.IsNullOrEmpty(DESCRIPTION) Then
                                ds.Tables("HOLD").Rows(0)("DESCRIPTION") = DESCRIPTION.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("DESCRIPTION") = System.DBNull.Value
                            End If
                        End If

                        If TR_GLOBE_TYPE.Visible = True Then
                            If Not String.IsNullOrEmpty(GLOBE_TYPE) Then
                                ds.Tables("HOLD").Rows(0)("GLOBE_TYPE") = GLOBE_TYPE.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("GLOBE_TYPE") = System.DBNull.Value
                            End If
                        End If

                        If TR_ALTER_DATE.Visible = True Then
                            If Not String.IsNullOrEmpty(ALTER_DATE) Then
                                ds.Tables("HOLD").Rows(0)("ALTER_DATE") = ALTER_DATE.ToString.Trim
                            Else
                                ds.Tables("HOLD").Rows(0)("ALTER_DATE") = System.DBNull.Value
                            End If
                        End If

                        ds.Tables("HOLD").Rows(0)("UPDATED_BY") = UserCode.Trim
                        ds.Tables("HOLD").Rows(0)("DATE_MODIFIED") = Now.Date
                        ds.Tables("HOLD").Rows(0)("IP") = Request.UserHostAddress.Trim

                        thisTransaction = SqlConn.BeginTransaction()
                        da.SelectCommand.Transaction = thisTransaction
                        da.Update(ds, "HOLD")
                        thisTransaction.Commit()
                        SqlConn.Close()
                        Label15.Text = " "
                        Label1.Text = "Record Updated Successfully!"
                        PopulateHoldingsGrid(Label_CATNO.Text, Label_ACQID.Text)
                    Else
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('User Profile Update  - Please Contact System Administrator... ');", True)
                        Label15.Text = "Record Not Updated! "
                        Label1.Text = ""
                        Exit Sub
                        End If
                    End If
            Catch q As SqlException
                thisTransaction.Rollback()
                Label15.Text = "Database Error -UPDATE: " & (q.Message() & ", " & q.Number & ", " & q.GetType.ToString)
                Label1.Text = ""
            Catch ex As Exception
                Label15.Text = "Error-UPDATE: " & (ex.Message())
                Label1.Text = ""
            Finally
                SqlConn.Close()
            End Try
        End If

    End Sub
    'Delete Selecte Holdings Records
    Protected Sub Hold_Delete_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Hold_Delete_Bttn.Click
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing
        Try
            For Each row As GridViewRow In Grid2.Rows
                Dim cb As CheckBox = DirectCast(row.FindControl("cbd"), CheckBox)
                If cb IsNot Nothing AndAlso cb.Checked = True Then
                    Dim ID As Integer = Convert.ToInt32(Grid2.DataKeys(row.RowIndex).Value)

                    'First Check the status of each Accession No / Delete only if status is other than 2
                    Dim str As Object = Nothing
                    Dim flag As Object = Nothing
                    str = "SELECT HOLD_ID FROM HOLDINGS WHERE (HOLD_ID = '" & Trim(ID) & "')  AND (STA_CODE = '2') AND (LIB_CODE = '" & Trim(LibCode) & "')"
                    Dim cmd1 As New SqlCommand(str, SqlConn)
                    SqlConn.Open()
                    flag = cmd1.ExecuteScalar
                    SqlConn.Close()
                    If flag = Nothing Then
                        'validation for acq_id
                        Dim ACQ_ID As Long = Nothing
                        If Me.Label_ACQID.Text <> "" Then
                            ACQ_ID = Convert.ToInt16(TrimX(Label_ACQID.Text))
                            ACQ_ID = RemoveQuotes(ACQ_ID)
                            If Not IsNumeric(ACQ_ID) = True Then
                                Label15.Text = "Error: Input is not Valid !"
                                Label1.Text = ""
                                Exit Sub
                            End If
                            If Len(ACQ_ID) > 10 Then
                                Label15.Text = "Error: Input is not Valid !"
                                Label1.Text = ""
                                Exit Sub
                            End If
                            ACQ_ID = " " & ACQ_ID & " "
                            If InStr(1, ACQ_ID, "CREATE", 1) > 0 Or InStr(1, ACQ_ID, "DELETE", 1) > 0 Or InStr(1, ACQ_ID, "DROP", 1) > 0 Or InStr(1, ACQ_ID, "INSERT", 1) > 1 Or InStr(1, ACQ_ID, "TRACK", 1) > 1 Or InStr(1, ACQ_ID, "TRACE", 1) > 1 Then
                                Label15.Text = "Error: Input is not Valid !"
                                Label1.Text = ""
                                Exit Sub
                            End If
                            ACQ_ID = TrimX(ACQ_ID)
                        End If

                        'validation for cat_no
                        Dim CAT_NO As Long = Nothing
                        If Me.Label_CATNO.Text <> "" Then
                            CAT_NO = Convert.ToInt16(TrimX(Label_CATNO.Text))
                            CAT_NO = RemoveQuotes(CAT_NO)
                            If Not IsNumeric(CAT_NO) = True Then
                                Label15.Text = "Error: Input is not Valid !"
                                Label1.Text = ""
                                Exit Sub
                            End If
                            If Len(CAT_NO) > 10 Then
                                Label15.Text = "Error: Input is not Valid !"
                                Label1.Text = ""
                                Exit Sub
                            End If
                            CAT_NO = " " & CAT_NO & " "
                            If InStr(1, CAT_NO, "CREATE", 1) > 0 Or InStr(1, CAT_NO, "DELETE", 1) > 0 Or InStr(1, CAT_NO, "DROP", 1) > 0 Or InStr(1, CAT_NO, "INSERT", 1) > 1 Or InStr(1, CAT_NO, "TRACK", 1) > 1 Or InStr(1, CAT_NO, "TRACE", 1) > 1 Then
                                Label15.Text = "Error: Input is not Valid !"
                                Label1.Text = ""
                                Exit Sub
                            End If
                            CAT_NO = TrimX(CAT_NO)
                        End If



                        If SqlConn.State = 0 Then
                            SqlConn.Open()
                        End If

                        thisTransaction = SqlConn.BeginTransaction()
                        Dim objCommand As New SqlCommand
                        objCommand.Connection = SqlConn
                        objCommand.Transaction = thisTransaction
                        objCommand.CommandType = CommandType.Text
                        objCommand.CommandText = "DELETE FROM  HOLDINGS WHERE HOLD_ID = @HOLD_ID "

                        objCommand.Parameters.Add("@HOLD_ID", SqlDbType.Int)
                        objCommand.Parameters("@HOLD_ID").Value = ID

                        objCommand.ExecuteNonQuery()


                        'count copies from holdings
                        Dim objCommand2 As New SqlCommand
                        objCommand2.Connection = SqlConn
                        objCommand2.Transaction = thisTransaction
                        objCommand2.CommandType = CommandType.Text
                        objCommand2.CommandText = "UPDATE ACQUISITIONS SET COPY_ACCESSIONED =(SELECT COUNT(*) FROM HOLDINGS WHERE ACQ_ID = @ACQ_ID AND LIB_CODE = @LIB_CODE) WHERE (ACQ_ID = @ACQ_ID AND LIB_CODE = @LIB_CODE)"

                        objCommand2.Parameters.Add("@ACQ_ID", SqlDbType.Int)
                        objCommand2.Parameters("@ACQ_ID").Value = ACQ_ID

                        If LibCode = "" Then LibCode = System.DBNull.Value
                        objCommand2.Parameters.Add("@LIB_CODE", SqlDbType.NVarChar)
                        objCommand2.Parameters("@LIB_CODE").Value = LibCode

                        Dim dr2 As SqlDataReader
                        dr2 = objCommand2.ExecuteReader()
                        dr2.Close()

                        'update acq status
                        Dim objCommand3 As New SqlCommand
                        objCommand3.Connection = SqlConn
                        objCommand3.Transaction = thisTransaction
                        objCommand3.CommandType = CommandType.Text
                        objCommand3.CommandText = "UPDATE ACQUISITIONS SET PROCESS_STATUS =(CASE WHEN COPY_RECEIVED=COPY_ACCESSIONED THEN 'Accessioned' when COPY_ACCESSIONED=0 then 'Received' ELSE 'Partially Accessioned' END) WHERE (ACQ_ID = @ACQ_ID AND LIB_CODE = @LIB_CODE)"

                        objCommand3.Parameters.Add("@ACQ_ID", SqlDbType.Int)
                        objCommand3.Parameters("@ACQ_ID").Value = ACQ_ID

                        If LibCode = "" Then LibCode = System.DBNull.Value
                        objCommand3.Parameters.Add("@LIB_CODE", SqlDbType.NVarChar)
                        objCommand3.Parameters("@LIB_CODE").Value = LibCode

                        Dim dr3 As SqlDataReader
                        dr3 = objCommand3.ExecuteReader()
                        dr3.Close()

                        'update cats table status
                        Dim objCommand4 As New SqlCommand
                        objCommand4.Connection = SqlConn
                        objCommand4.Transaction = thisTransaction
                        objCommand4.CommandType = CommandType.Text
                        objCommand4.CommandText = "UPDATE CATS SET CAT_LEVEL = (SELECT CASE WHEN COUNT(*)= 0 THEN 'Partial' Else 'Full' END as a FROM HOLDINGS WHERE CAT_NO = @CAT_NO) WHERE (CAT_NO = @CAT_NO)"

                        objCommand4.Parameters.Add("@CAT_NO", SqlDbType.Int)
                        objCommand4.Parameters("@CAT_NO").Value = CAT_NO

                        Dim dr4 As SqlDataReader
                        dr4 = objCommand4.ExecuteReader()
                        dr4.Close()

                        thisTransaction.Commit()
                        SqlConn.Close()
                        Label15.Text = ""
                        Label1.Text = "Holding Record Deleted Succesfully!"
                    Else
                        Label15.Text = "This Accession No is ISSUED!"
                        Label1.Text = ""
                    End If
                Else
                    Label15.Text = "No Recrod was selected for Deletion !"
                    Label1.Text = ""
                End If
            Next
            Dim s As Object = Nothing
            Dim e1 As EventArgs = Nothing
            PopulateOrderGrid1(DDL_Orders.SelectedValue)
            PopulateHoldingsGrid(Label_CATNO.Text, Label_ACQID.Text)
        Catch s As Exception
            thisTransaction.Rollback()
            Label1.Text = ""
            Label15.Text = "Error: " & (s.Message())
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

    'autocomplete method for discount
    <System.Web.Script.Services.ScriptMethod(), _
  System.Web.Services.WebMethod()> _
    Public Shared Function SearchClassNumber(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Return SearchClassNo(prefixText, count)
    End Function
    'search class No
    <System.Web.Script.Services.ScriptMethod(), _
System.Web.Services.WebMethod()> _
    Public Shared Function SearchClassNo(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim sdr As SqlDataReader
        Dim cmd As SqlCommand = New SqlCommand
        cmd.CommandText = "SELECT DISTINCT CLASS_NO from HOLDINGS WHERE (LIB_CODE ='" & LibCode & "')  AND (CLASS_NO like '" + prefixText + "%')"
        cmd.Connection = SqlConn
        Try
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            Dim classNo As List(Of String) = New List(Of String)
            sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            While sdr.Read
                classNo.Add(sdr("CLASS_NO").ToString)
            End While
            Return classNo
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

   
End Class