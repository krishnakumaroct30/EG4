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
Public Class Budgets
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
                        Me.bttn_Save.Visible = True
                        Me.bttn_Update.Visible = False
                        Label6.Text = "Enter Data and Press SAVE Button to save the record"
                        Label8.Text = ""
                        PopulateBudgets()
                    End If
                End If
                Dim CreateSUserButton As Object 'System.Web.UI.WebControls.Button
                CreateSUserButton = Master.FindControl("Accordion1").FindControl("BudgPane").FindControl("Budg_Budgets_Bttn")
                CreateSUserButton.ForeColor = Drawing.Color.Red
                myPaneName = "BudgPane" 'paneSelectedIndex = 0
            Else
                Response.Redirect("~/Default.aspx", False)
            End If
            LibCode = Session.Item("LoggedLibcode")
        Catch ex As Exception
            Label8.Text = "Error: " & (ex.Message())
            Label6.Text = ""
        End Try
    End Sub
    'Populate the users in grid     'search users
    Public Sub PopulateBudgets()
        Dim dt As DataTable = Nothing
        Dim Check As Integer
        Try
            Dim SQL As String = Nothing
            SQL = "SELECT * FROM BUDGETS WHERE  (LIB_CODE='" & Trim(LibCode) & "')  "

            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            da.Fill(ds)

            dt = ds.Tables(0).Copy
            Dim RecordCount As Long = 0
            If dt.Rows.Count = 0 Then
                Me.Grid_Budgets.DataSource = Nothing
                Grid_Budgets.DataBind()
                Label1.Text = "Total Record(s): 0 "
                Delete_Bttn.Enabled = False
            Else
                Grid_Budgets.Visible = True
                RecordCount = dt.Rows.Count
                Grid_Budgets.DataSource = dt
                Grid_Budgets.DataBind()
                Label1.Text = "Total Record(s): " & RecordCount
                Delete_Bttn.Enabled = True
            End If
            ViewState("dt") = dt
            UpdatePanel1.Update()
        Catch s As Exception
            Label8.Text = "Error - Populate Budgets:  " & (s.Message())
            Label6.Text = ""
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
    Protected Sub Grid_Budgets_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles Grid_Budgets.PageIndexChanging
        Try
            'rebind datagrid
            Grid_Budgets.DataSource = ViewState("dt") 'temp
            Grid_Budgets.PageIndex = e.NewPageIndex
            index = e.NewPageIndex * Grid_Budgets.PageSize
            Grid_Budgets.DataBind()
        Catch s As Exception
            Label8.Text = "Error - Populate Budgets:  " & (s.Message())
            Label6.Text = ""
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
    Protected Sub Grid_Budgets_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles Grid_Budgets.Sorting
        Dim temp As DataTable = CType(ViewState("dt"), DataTable)
        GridViewSortExpression = e.SortExpression
        Grid_Budgets.DataSource = temp
        Dim pageIndex As Integer = Grid_Budgets.PageIndex
        Grid_Budgets.DataSource = SortDataTable(Grid_Budgets.DataSource, False)
        Grid_Budgets.DataBind()
        Grid_Budgets.PageIndex = pageIndex
        UpdatePanel1.Update()
    End Sub
    Protected Sub Grid_Budgets_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Grid_Budgets.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Lblsr As Label = e.Row.FindControl("lblsr")
            Srno = index + 1
            If Lblsr Is Nothing = False Then
                Lblsr.Text = Srno
                index += 1
            End If
            e.Row.Attributes("onmouseover") = "this.style.cursor='hand';this.style.textDecoration='bold';this.style.background='#FFDFDF'"
            e.Row.Attributes("onmouseout") = "this.style.textDecoration='none';this.style.background='none'"
        End If
    End Sub
    Private Shared Function t() As Object
        Throw New NotImplementedException
    End Function
    'get value of row from grid
    Private Sub Grid_Budgets_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles Grid_Budgets.RowCommand
        Dim Command As SqlCommand = Nothing
        Dim dr As SqlDataReader = Nothing
        Try
            If e.CommandName = "Select" Then
                Dim myRowID, BudgID As Integer
                myRowID = e.CommandArgument.ToString()
                BudgID = Grid_Budgets.DataKeys(myRowID).Value

                If Not String.IsNullOrEmpty(BudgID) And BudgID <> 0 Then
                    Label7.Text = BudgID
                    Dim iloop As Integer
                    Dim strcurrentchar As Object
                    Dim c As Integer
                    Dim counter1 As Integer

                    BudgID = TrimX(BudgID)
                    BudgID = RemoveQuotes(BudgID)
                    If Len(BudgID).ToString > 10 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Length of Input is not Proper... ');", True)
                        Exit Sub
                    End If
                    If IsNumeric(BudgID) = False Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Enter Valid Data Only!');", True)
                        Label8.Text = "Plz Enter Valid Data Only!"
                        Label6.Text = ""
                        txt_Budg_Year.Focus()
                        Exit Sub
                    End If

                    BudgID = " " & BudgID & " "
                    If InStr(1, BudgID, " CREATE ", 1) > 0 Or InStr(1, BudgID, " DELETE ", 1) > 0 Or InStr(1, BudgID, " DROP ", 1) > 0 Or InStr(1, BudgID, " INSERT ", 1) > 1 Or InStr(1, BudgID, " TRACK ", 1) > 1 Or InStr(1, BudgID, " TRACE ", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do not use reserve words... ');", True)
                        Exit Sub
                    End If
                    BudgID = TrimX(BudgID)

                    'get record details from database
                    Dim SQL As String = Nothing
                    SQL = " SELECT *  FROM BUDGETS WHERE (BUDG_ID = '" & Trim(BudgID) & "') "
                    Command = New SqlCommand(SQL, SqlConn)
                    SqlConn.Open()
                    dr = Command.ExecuteReader(CommandBehavior.CloseConnection)
                    dr.Read()
                    If dr.HasRows = True Then
                        ClearFields()
                        If dr.Item("BUDG_YEAR").ToString <> "" Then
                            Me.txt_Budg_Year.Text = dr.Item("BUDG_YEAR").ToString
                        Else
                            txt_Budg_Year.Text = ""
                        End If
                        If dr.Item("BUDG_HEAD").ToString <> "" Then
                            Me.txt_Budg_Head.Text = dr.Item("BUDG_HEAD").ToString
                        Else
                            Me.txt_Budg_Head.Text = ""
                        End If
                        If dr.Item("BUDG_PERIOD").ToString <> "" Then
                            txt_Budg_Period.Text = dr.Item("BUDG_PERIOD").ToString
                        Else
                            txt_Budg_Period.Text = ""
                        End If
                        If dr.Item("BUDG_AMOUNT").ToString <> "" Then
                            Me.txt_Budg_Amount.Text = dr.Item("BUDG_AMOUNT").ToString
                        Else
                            Me.txt_Budg_Amount.Text = ""
                        End If
                        If dr.Item("BUDG_REMARKS").ToString <> "" Then
                            Me.txt_Budg_Remarks.Text = dr.Item("BUDG_REMARKS").ToString
                        Else
                            Me.txt_Budg_Remarks.Text = ""
                        End If

                        bttn_Save.Visible = False
                        bttn_Update.Visible = True
                        Label6.Text = "Press UPDATE Button to save the Changes if any.."
                        Label8.Text = ""
                        dr.Close()
                        SqlConn.Close()
                    Else
                        Label6.Text = ""
                        Label6.Text = "No Record Selected from Grid"
                    End If
                Else
                    Label6.Text = ""
                    Label6.Text = "No Record Selected from Grid"
                End If
            End If
        Catch s As Exception
            Label8.Text = "Error: " & (s.Message())
            Label6.Text = ""
        Finally
            SqlConn.Close()
        End Try
    End Sub 'Grid1_ItemCommand
    ' save data
    Protected Sub Save_Click(ByVal sender As Object, ByVal e As EventArgs) Handles bttn_Save.Click
        If IsPostBack = True Then
            Dim thisTransaction As SqlClient.SqlTransaction = Nothing
            Dim iloop As Integer
            Dim strcurrentchar As Object
            Dim c As Integer
            Dim counter1, counter2, counter3, counter4, Counter5 As Integer
            Try
                'Server Validation for BUDG_YEAR
                Dim BUDG_YEAR As Integer = 0
                If txt_Budg_Year.Text <> "" Then
                    BUDG_YEAR = TrimX(txt_Budg_Year.Text)
                    BUDG_YEAR = RemoveQuotes(BUDG_YEAR)
                    If IsNumeric(BUDG_YEAR) = False Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Enter Budget Year in YYYY Format!');", True)
                        Label8.Text = "Plz Enter Budget Year in YYYY Format with 4 Digits Only!"
                        Label6.Text = ""
                        txt_Budg_Year.Focus()
                        Exit Sub
                    End If
                    If BUDG_YEAR.ToString.Length > 5 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Enter Budget Year in YYYY Format!');", True)
                        Label8.Text = "Plz Enter Budget Year in YYYY Format with 4 Digits Only!"
                        Label6.Text = ""
                        txt_Budg_Year.Focus()
                        Exit Sub
                    End If
                    BUDG_YEAR = " " & BUDG_YEAR & " "
                    If InStr(1, BUDG_YEAR, "CREATE", 1) > 0 Or InStr(1, BUDG_YEAR, "DELETE", 1) > 0 Or InStr(1, BUDG_YEAR, "DROP", 1) > 0 Or InStr(1, BUDG_YEAR, "INSERT", 1) > 1 Or InStr(1, BUDG_YEAR, "TRACK", 1) > 1 Or InStr(1, BUDG_YEAR, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not use Reserve Words... ');", True)
                        Label8.Text = "Do Not Use Reserve Workds!"
                        Label6.Text = ""
                        txt_Budg_Year.Focus()
                        Exit Sub
                    End If
                    BUDG_YEAR = TrimX(BUDG_YEAR)

                    c = 0
                    counter1 = 0
                    For iloop = 1 To Len(BUDG_YEAR)
                        strcurrentchar = Mid(BUDG_YEAR, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter1 = 1
                            End If
                        End If
                    Next
                    If counter1 = 1 Then
                        Label8.Text = "Do Not Use Un-Wanted Charactrs!"
                        Label6.Text = ""
                        txt_Budg_Year.Focus()
                        Exit Sub
                    End If
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Enter Budget Year in YYYY Format!);", True)
                    Label8.Text = "Plz Enter Budget Year in YYYY Format in 4 Digits Only!"
                    Label6.Text = ""
                    Me.txt_Budg_Year.Focus()
                    Exit Sub
                End If

                '****************************************************************************************
                'Server Validation for BUDG_HEAD
                Dim BUDG_HEAD As Object = Nothing
                If txt_Budg_Head.Text <> "" Then
                    BUDG_HEAD = TrimAll(txt_Budg_Head.Text)
                    BUDG_HEAD = RemoveQuotes(BUDG_HEAD)
                    If BUDG_HEAD.ToString.Length > 150 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Enter data with Proper Length!');", True)
                        Label8.Text = "Plz Enter data with  Proper Length!"
                        Label6.Text = ""
                        txt_Budg_Head.Focus()
                        Exit Sub
                    End If
                    BUDG_HEAD = " " & BUDG_HEAD & " "
                    If InStr(1, BUDG_HEAD, "CREATE", 1) > 0 Or InStr(1, BUDG_HEAD, "DELETE", 1) > 0 Or InStr(1, BUDG_HEAD, "DROP", 1) > 0 Or InStr(1, BUDG_HEAD, "INSERT", 1) > 1 Or InStr(1, BUDG_HEAD, "TRACK", 1) > 1 Or InStr(1, BUDG_HEAD, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not use Reserve Words... ');", True)
                        Label8.Text = "Do Not Use Reserve Words!"
                        Label6.Text = ""
                        txt_Budg_Head.Focus()
                        Exit Sub
                    End If
                    BUDG_HEAD = TrimAll(BUDG_HEAD)
                    c = 0
                    counter2 = 0
                    For iloop = 1 To Len(BUDG_HEAD)
                        strcurrentchar = Mid(BUDG_HEAD, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter2 = 1
                            End If
                        End If
                    Next
                    If counter2 = 1 Then
                        Label8.Text = "Do Not Use Un-Wanted Charactrs!"
                        Label6.Text = ""
                        txt_Budg_Head.Focus()
                        Exit Sub
                    End If
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Enter Budget Head!);", True)
                    Label8.Text = "Plz Enter Budget Head!"
                    Label6.Text = ""
                    Me.txt_Budg_Head.Focus()
                    Exit Sub
                End If

                'chk duplicate              
                Dim str As Object = Nothing
                Dim Flag As Object = Nothing
                str = "SELECT BUDG_ID FROM BUDGETS WHERE (BUDG_YEAR = '" & Trim(BUDG_YEAR) & "' AND BUDG_HEAD = '" & Trim(BUDG_HEAD) & "' AND LIB_CODE = '" & Trim(LibCode) & "')"
                Dim cmd1 As New SqlCommand(str, SqlConn)
                SqlConn.Open()
                flag = cmd1.ExecuteScalar
                If flag <> Nothing Then
                    Label8.Text = "This Budget Head Already Exists ! "
                    Label6.Text = ""
                    Me.txt_Budg_Head.Focus()
                    Exit Sub
                End If
                SqlConn.Close()

                '****************************************************************************************
                'Server Validation for BUDG_PERIOD
                Dim BUDG_PERIOD As Object = Nothing
                If txt_Budg_Period.Text <> "" Then
                    BUDG_PERIOD = TrimAll(txt_Budg_Period.Text)
                    BUDG_PERIOD = RemoveQuotes(BUDG_PERIOD)
                    If BUDG_PERIOD.ToString.Length > 100 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Enter data with Proper Length!');", True)
                        Label8.Text = "Plz Enter data with  Proper Length!"
                        Label6.Text = ""
                        txt_Budg_Period.Focus()
                        Exit Sub
                    End If
                    BUDG_PERIOD = " " & BUDG_PERIOD & " "
                    If InStr(1, BUDG_PERIOD, "CREATE", 1) > 0 Or InStr(1, BUDG_PERIOD, "DELETE", 1) > 0 Or InStr(1, BUDG_PERIOD, "DROP", 1) > 0 Or InStr(1, BUDG_PERIOD, "INSERT", 1) > 1 Or InStr(1, BUDG_PERIOD, "TRACK", 1) > 1 Or InStr(1, BUDG_PERIOD, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not use Reserve Words... ');", True)
                        Label8.Text = "Do Not Use Reserve Words!"
                        Label6.Text = ""
                        txt_Budg_Period.Focus()
                        Exit Sub
                    End If
                    BUDG_PERIOD = TrimAll(BUDG_PERIOD)
                    c = 0
                    counter3 = 0
                    For iloop = 1 To Len(BUDG_PERIOD)
                        strcurrentchar = Mid(BUDG_PERIOD, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter3 = 1
                            End If
                        End If
                    Next
                    If counter3 = 1 Then
                        Label8.Text = "Do Not Use Un-Wanted Charactrs!"
                        Label6.Text = ""
                        txt_Budg_Period.Focus()
                        Exit Sub
                    End If
                Else
                    BUDG_PERIOD = ""
                End If


                'Server Validation for BUDG_AMOUNT
                Dim BUDG_AMOUNT As Object = Nothing
                If txt_Budg_Amount.Text <> "" Then
                    BUDG_AMOUNT = TrimX(txt_Budg_Amount.Text)
                    BUDG_AMOUNT = RemoveQuotes(BUDG_AMOUNT)
                    If BUDG_AMOUNT.ToString.Length > 15 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Enter data with Proper Length!');", True)
                        Label8.Text = "Plz Enter data with  Proper Length!"
                        Label6.Text = ""
                        txt_Budg_Amount.Focus()
                        Exit Sub
                    End If
                    BUDG_AMOUNT = " " & BUDG_AMOUNT & " "
                    If InStr(1, BUDG_AMOUNT, "CREATE", 1) > 0 Or InStr(1, BUDG_AMOUNT, "DELETE", 1) > 0 Or InStr(1, BUDG_AMOUNT, "DROP", 1) > 0 Or InStr(1, BUDG_AMOUNT, "INSERT", 1) > 1 Or InStr(1, BUDG_AMOUNT, "TRACK", 1) > 1 Or InStr(1, BUDG_AMOUNT, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not use Reserve Words... ');", True)
                        Label8.Text = "Do Not Use Reserve Words!"
                        Label6.Text = ""
                        txt_Budg_Amount.Focus()
                        Exit Sub
                    End If
                    BUDG_AMOUNT = TrimX(BUDG_AMOUNT)
                    c = 0
                    counter4 = 0
                    For iloop = 1 To Len(BUDG_AMOUNT)
                        strcurrentchar = Mid(BUDG_AMOUNT, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>=%abcdefghijklmnopqrstuvwxyz,/;:_)(""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter4 = 1
                            End If
                        End If
                    Next
                    If counter4 = 1 Then
                        Label8.Text = "Do Not Use Un-Wanted Charactrs!"
                        Label6.Text = ""
                        txt_Budg_Amount.Focus()
                        Exit Sub
                    End If
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Enter Budget Amount in Rupees!);", True)
                    Label8.Text = "Plz Enter Budget Amount in Rupees!"
                    Label6.Text = ""
                    Me.txt_Budg_Amount.Focus()
                    Exit Sub
                End If
                '****************************************************************************************88
                'Server Validation for BUDG_REMARKS
                Dim BUDG_REMARKS As Object = Nothing
                If txt_Budg_Remarks.Text <> "" Then
                    BUDG_REMARKS = TrimAll(txt_Budg_Remarks.Text)
                    BUDG_REMARKS = RemoveQuotes(BUDG_REMARKS)
                    If BUDG_REMARKS.ToString.Length > 150 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Enter data with Proper Length!');", True)
                        Label8.Text = "Plz Enter data with  Proper Length!"
                        Label6.Text = ""
                        txt_Budg_Remarks.Focus()
                        Exit Sub
                    End If
                    BUDG_REMARKS = " " & BUDG_REMARKS & " "
                    If InStr(1, BUDG_REMARKS, "CREATE", 1) > 0 Or InStr(1, BUDG_REMARKS, "DELETE", 1) > 0 Or InStr(1, BUDG_REMARKS, "DROP", 1) > 0 Or InStr(1, BUDG_REMARKS, "INSERT", 1) > 1 Or InStr(1, BUDG_REMARKS, "TRACK", 1) > 1 Or InStr(1, BUDG_REMARKS, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not use Reserve Words... ');", True)
                        Label8.Text = "Do Not Use Reserve Words!"
                        Label6.Text = ""
                        txt_Budg_Remarks.Focus()
                        Exit Sub
                    End If
                    BUDG_REMARKS = TrimAll(BUDG_REMARKS)
                    c = 0
                    Counter5 = 0
                    For iloop = 1 To Len(BUDG_REMARKS)
                        strcurrentchar = Mid(BUDG_REMARKS, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                Counter5 = 1
                            End If
                        End If
                    Next
                    If Counter5 = 1 Then
                        Label8.Text = "Do Not Use Un-Wanted Charactrs!"
                        Label6.Text = ""
                        txt_Budg_Remarks.Focus()
                        Exit Sub
                    End If
                Else
                    BUDG_REMARKS = ""
                End If


                Dim LIB_CODE As Object = Nothing
                If LibCode = "" Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Plz Login Again!');", True)
                    Label8.Text = "Plz Login Again!"
                    Label6.Text = ""
                    Exit Sub
                Else
                    LIB_CODE = LibCode
                End If

                Dim USER_CODE As Object = Nothing
                USER_CODE = Session.Item("LoggedUser")

                Dim DATE_ADDED As Object = Nothing
                DATE_ADDED = Now.Date
                DATE_ADDED = Convert.ToDateTime(DATE_ADDED, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                Dim IP As Object = Nothing
                IP = Request.UserHostAddress.Trim

                '*******************************************************************************************
                If SqlConn.State = 0 Then
                    SqlConn.Open()
                End If
                thisTransaction = SqlConn.BeginTransaction()

                ' Dim intValue As Integer = 0
                Dim objCommand As New SqlCommand
                objCommand.Connection = SqlConn
                objCommand.Transaction = thisTransaction
                objCommand.CommandType = CommandType.Text
                objCommand.CommandText = "INSERT INTO BUDGETS (BUDG_YEAR, BUDG_HEAD, BUDG_PERIOD, BUDG_AMOUNT, BUDG_REMARKS, LIB_CODE, DATE_ADDED, USER_CODE, IP) " & _
                                             " VALUES (@BUDG_YEAR, @BUDG_HEAD, @BUDG_PERIOD, @BUDG_AMOUNT, @BUDG_REMARKS, @LIB_CODE, @DATE_ADDED, @USER_CODE, @IP); " & _
                                             " SELECT SCOPE_IDENTITY()"

                objCommand.Parameters.Add("@BUDG_YEAR", SqlDbType.Int)
                objCommand.Parameters("@BUDG_YEAR").Value = BUDG_YEAR

                objCommand.Parameters.Add("@BUDG_HEAD", SqlDbType.NVarChar)
                objCommand.Parameters("@BUDG_HEAD").Value = BUDG_HEAD

                If BUDG_PERIOD = "" Then BUDG_PERIOD = System.DBNull.Value
                objCommand.Parameters.Add("@BUDG_PERIOD", SqlDbType.VarChar)
                objCommand.Parameters("@BUDG_PERIOD").Value = BUDG_PERIOD

                objCommand.Parameters.Add("@BUDG_AMOUNT", SqlDbType.Decimal)
                objCommand.Parameters("@BUDG_AMOUNT").Value = BUDG_AMOUNT

                If BUDG_REMARKS = "" Then BUDG_REMARKS = System.DBNull.Value
                objCommand.Parameters.Add("@BUDG_REMARKS", SqlDbType.NVarChar)
                objCommand.Parameters("@BUDG_REMARKS").Value = BUDG_REMARKS

                objCommand.Parameters.Add("@LIB_CODE", SqlDbType.NVarChar)
                objCommand.Parameters("@LIB_CODE").Value = LIB_CODE

                If DATE_ADDED = "" Then DATE_ADDED = System.DBNull.Value
                objCommand.Parameters.Add("@DATE_ADDED", SqlDbType.DateTime)
                objCommand.Parameters("@DATE_ADDED").Value = DATE_ADDED

                If USER_CODE = "" Then USER_CODE = System.DBNull.Value
                objCommand.Parameters.Add("@USER_CODE", SqlDbType.NVarChar)
                objCommand.Parameters("@USER_CODE").Value = USER_CODE

                If IP = "" Then IP = System.DBNull.Value
                objCommand.Parameters.Add("@IP", SqlDbType.VarChar)
                objCommand.Parameters("@IP").Value = IP

                objCommand.ExecuteNonQuery()

                thisTransaction.Commit()
                SqlConn.Close()

                Label6.Text = "Data Added Successfully!"
                Label8.Text = ""
                bttn_Save.Visible = True
                bttn_Update.Visible = False
                PopulateBudgets()
                BUDG_YEAR = Nothing
                BUDG_HEAD = Nothing
                BUDG_PERIOD = Nothing
                BUDG_REMARKS = Nothing
                BUDG_AMOUNT = Nothing
            Catch q As SqlException
                thisTransaction.Rollback()
                Label8.Text = "Error: " & (q.Message())
                Label6.Text = ""
            Catch ex As Exception
                Label8.Text = "Error: " & (ex.Message())
                Label6.Text = ""
            Finally
                SqlConn.Close()
            End Try
        End If
    End Sub
    Public Sub Cancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Cancel.Click
        bttn_Save.Visible = True
        bttn_Update.Visible = False
        Label6.Text = "Enter Data and Press SAVE Button to save the record.."
        ClearFields()
    End Sub
    Public Sub ClearFields()
        txt_Budg_Year.Text = ""
        txt_Budg_Head.Text = ""
        txt_Budg_Remarks.Text = ""
        txt_Budg_Period.Text = ""
        txt_Budg_Amount.Text = ""
    End Sub
    ''update record
    Protected Sub bttn_Update_Click(ByVal sender As Object, ByVal e As EventArgs) Handles bttn_Update.Click
        Dim SQL As String = Nothing
        Dim da As SqlDataAdapter
        Dim cb As SqlCommandBuilder
        Dim ds As New DataSet
        Dim dt As New DataTable
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing
        Dim iloop As Integer
        Dim strcurrentchar As Object
        Dim c As Integer
        Dim counter1, counter2, counter3, counter4, Counter5 As Integer
        Try
            If IsPostBack = True Then
                Dim BUDG_ID As Integer = 0
                If Label7.Text <> "" Then
                    BUDG_ID = Label7.Text
                    BUDG_ID = TrimX(BUDG_ID)
                    BUDG_ID = RemoveQuotes(BUDG_ID)
                    If Len(BUDG_ID).ToString > 10 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Length of Input is not Proper... ');", True)
                        Exit Sub
                    End If
                    BUDG_ID = " " & BUDG_ID & " "
                    If InStr(1, BUDG_ID, " CREATE ", 1) > 0 Or InStr(1, BUDG_ID, " DELETE ", 1) > 0 Or InStr(1, BUDG_ID, " DROP ", 1) > 0 Or InStr(1, BUDG_ID, " INSERT ", 1) > 1 Or InStr(1, BUDG_ID, " TRACK ", 1) > 1 Or InStr(1, BUDG_ID, " TRACE ", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do not use reserve words... ');", True)
                        Exit Sub
                    End If
                    BUDG_ID = TrimX(BUDG_ID)
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('There is no Record Selected for Updation');", True)
                    Exit Sub
                End If

                'Server Validation for BUDG_YEAR
                Dim BUDG_YEAR As Integer = 0
                If txt_Budg_Year.Text <> "" Then
                    BUDG_YEAR = TrimX(txt_Budg_Year.Text)
                    BUDG_YEAR = RemoveQuotes(BUDG_YEAR)
                    If IsNumeric(BUDG_YEAR) = False Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Enter Budget Year in YYYY Format!');", True)
                        Label8.Text = "Plz Enter Budget Year in YYYY Format with 4 Digits Only!"
                        Label6.Text = ""
                        txt_Budg_Year.Focus()
                        Exit Sub
                    End If
                    If BUDG_YEAR.ToString.Length > 5 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Enter Budget Year in YYYY Format!');", True)
                        Label8.Text = "Plz Enter Budget Year in YYYY Format with 4 Digits Only!"
                        Label6.Text = ""
                        txt_Budg_Year.Focus()
                        Exit Sub
                    End If
                    BUDG_YEAR = " " & BUDG_YEAR & " "
                    If InStr(1, BUDG_YEAR, "CREATE", 1) > 0 Or InStr(1, BUDG_YEAR, "DELETE", 1) > 0 Or InStr(1, BUDG_YEAR, "DROP", 1) > 0 Or InStr(1, BUDG_YEAR, "INSERT", 1) > 1 Or InStr(1, BUDG_YEAR, "TRACK", 1) > 1 Or InStr(1, BUDG_YEAR, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not use Reserve Words... ');", True)
                        Label8.Text = "Do Not Use Reserve Workds!"
                        Label6.Text = ""
                        txt_Budg_Year.Focus()
                        Exit Sub
                    End If
                    BUDG_YEAR = TrimX(BUDG_YEAR)

                    c = 0
                    counter1 = 0
                    For iloop = 1 To Len(BUDG_YEAR)
                        strcurrentchar = Mid(BUDG_YEAR, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter1 = 1
                            End If
                        End If
                    Next
                    If counter1 = 1 Then
                        Label8.Text = "Do Not Use Un-Wanted Charactrs!"
                        Label6.Text = ""
                        txt_Budg_Year.Focus()
                        Exit Sub
                    End If
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Enter Budget Year in YYYY Format!);", True)
                    Label8.Text = "Plz Enter Budget Year in YYYY Format in 4 Digits Only!"
                    Label6.Text = ""
                    Me.txt_Budg_Year.Focus()
                    Exit Sub
                End If

                '****************************************************************************************
                'Server Validation for BUDG_HEAD
                Dim BUDG_HEAD As Object = Nothing
                If txt_Budg_Head.Text <> "" Then
                    BUDG_HEAD = TrimAll(txt_Budg_Head.Text)
                    BUDG_HEAD = RemoveQuotes(BUDG_HEAD)
                    If BUDG_HEAD.ToString.Length > 150 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Enter data with Proper Length!');", True)
                        Label8.Text = "Plz Enter data with  Proper Length!"
                        Label6.Text = ""
                        txt_Budg_Head.Focus()
                        Exit Sub
                    End If
                    BUDG_HEAD = " " & BUDG_HEAD & " "
                    If InStr(1, BUDG_HEAD, "CREATE", 1) > 0 Or InStr(1, BUDG_HEAD, "DELETE", 1) > 0 Or InStr(1, BUDG_HEAD, "DROP", 1) > 0 Or InStr(1, BUDG_HEAD, "INSERT", 1) > 1 Or InStr(1, BUDG_HEAD, "TRACK", 1) > 1 Or InStr(1, BUDG_HEAD, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not use Reserve Words... ');", True)
                        Label8.Text = "Do Not Use Reserve Words!"
                        Label6.Text = ""
                        txt_Budg_Head.Focus()
                        Exit Sub
                    End If
                    BUDG_HEAD = TrimAll(BUDG_HEAD)
                    c = 0
                    counter2 = 0
                    For iloop = 1 To Len(BUDG_HEAD)
                        strcurrentchar = Mid(BUDG_HEAD, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter2 = 1
                            End If
                        End If
                    Next
                    If counter2 = 1 Then
                        Label8.Text = "Do Not Use Un-Wanted Charactrs!"
                        Label6.Text = ""
                        txt_Budg_Head.Focus()
                        Exit Sub
                    End If
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Enter Budget Head!);", True)
                    Label8.Text = "Plz Enter Budget Head!"
                    Label6.Text = ""
                    Me.txt_Budg_Head.Focus()
                    Exit Sub
                End If

                'chk duplicate

                Dim str As Object = Nothing
                Dim Flag As Object = Nothing
                str = "SELECT BUDG_ID FROM BUDGETS WHERE (BUDG_YEAR = '" & Trim(BUDG_YEAR) & "' AND BUDG_HEAD = '" & Trim(BUDG_HEAD) & "' AND LIB_CODE = '" & Trim(LibCode) & "' AND BUDG_ID <> '" & Trim(BUDG_ID) & "') "
                Dim cmd1 As New SqlCommand(str, SqlConn)
                SqlConn.Open()
                Flag = cmd1.ExecuteScalar
                If Flag <> Nothing Then
                    Label8.Text = "This Budget Head Already Exists ! "
                    Label6.Text = ""
                    Me.txt_Budg_Head.Focus()
                    Exit Sub
                End If
                SqlConn.Close()

                '****************************************************************************************
                'Server Validation for BUDG_PERIOD
                Dim BUDG_PERIOD As Object = Nothing
                If txt_Budg_Period.Text <> "" Then
                    BUDG_PERIOD = TrimAll(txt_Budg_Period.Text)
                    BUDG_PERIOD = RemoveQuotes(BUDG_PERIOD)
                    If BUDG_PERIOD.ToString.Length > 100 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Enter data with Proper Length!');", True)
                        Label8.Text = "Plz Enter data with  Proper Length!"
                        Label6.Text = ""
                        txt_Budg_Period.Focus()
                        Exit Sub
                    End If
                    BUDG_PERIOD = " " & BUDG_PERIOD & " "
                    If InStr(1, BUDG_PERIOD, "CREATE", 1) > 0 Or InStr(1, BUDG_PERIOD, "DELETE", 1) > 0 Or InStr(1, BUDG_PERIOD, "DROP", 1) > 0 Or InStr(1, BUDG_PERIOD, "INSERT", 1) > 1 Or InStr(1, BUDG_PERIOD, "TRACK", 1) > 1 Or InStr(1, BUDG_PERIOD, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not use Reserve Words... ');", True)
                        Label8.Text = "Do Not Use Reserve Words!"
                        Label6.Text = ""
                        txt_Budg_Period.Focus()
                        Exit Sub
                    End If
                    BUDG_PERIOD = TrimAll(BUDG_PERIOD)
                    c = 0
                    counter3 = 0
                    For iloop = 1 To Len(BUDG_PERIOD)
                        strcurrentchar = Mid(BUDG_PERIOD, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter3 = 1
                            End If
                        End If
                    Next
                    If counter3 = 1 Then
                        Label8.Text = "Do Not Use Un-Wanted Charactrs!"
                        Label6.Text = ""
                        txt_Budg_Period.Focus()
                        Exit Sub
                    End If
                Else
                    BUDG_PERIOD = ""
                End If


                'Server Validation for BUDG_AMOUNT
                Dim BUDG_AMOUNT As Object = Nothing
                If txt_Budg_Amount.Text <> "" Then
                    BUDG_AMOUNT = TrimX(txt_Budg_Amount.Text)
                    BUDG_AMOUNT = RemoveQuotes(BUDG_AMOUNT)
                    If BUDG_AMOUNT.ToString.Length > 15 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Enter data with Proper Length!');", True)
                        Label8.Text = "Plz Enter data with  Proper Length!"
                        Label6.Text = ""
                        txt_Budg_Amount.Focus()
                        Exit Sub
                    End If
                    BUDG_AMOUNT = " " & BUDG_AMOUNT & " "
                    If InStr(1, BUDG_AMOUNT, "CREATE", 1) > 0 Or InStr(1, BUDG_AMOUNT, "DELETE", 1) > 0 Or InStr(1, BUDG_AMOUNT, "DROP", 1) > 0 Or InStr(1, BUDG_AMOUNT, "INSERT", 1) > 1 Or InStr(1, BUDG_AMOUNT, "TRACK", 1) > 1 Or InStr(1, BUDG_AMOUNT, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not use Reserve Words... ');", True)
                        Label8.Text = "Do Not Use Reserve Words!"
                        Label6.Text = ""
                        txt_Budg_Amount.Focus()
                        Exit Sub
                    End If
                    BUDG_AMOUNT = TrimX(BUDG_AMOUNT)
                    c = 0
                    counter4 = 0
                    For iloop = 1 To Len(BUDG_AMOUNT)
                        strcurrentchar = Mid(BUDG_AMOUNT, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>=%abcdefghijklmnopqrstuvwxyz,/;:_)(""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter4 = 1
                            End If
                        End If
                    Next
                    If counter4 = 1 Then
                        Label8.Text = "Do Not Use Un-Wanted Charactrs!"
                        Label6.Text = ""
                        txt_Budg_Amount.Focus()
                        Exit Sub
                    End If
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Enter Budget Amount in Rupees!);", True)
                    Label8.Text = "Plz Enter Budget Amount in Rupees!"
                    Label6.Text = ""
                    Me.txt_Budg_Amount.Focus()
                    Exit Sub
                End If
                '****************************************************************************************88
                'Server Validation for BUDG_REMARKS
                Dim BUDG_REMARKS As Object = Nothing
                If txt_Budg_Remarks.Text <> "" Then
                    BUDG_REMARKS = TrimAll(txt_Budg_Remarks.Text)
                    BUDG_REMARKS = RemoveQuotes(BUDG_REMARKS)
                    If BUDG_REMARKS.ToString.Length > 150 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Enter data with Proper Length!');", True)
                        Label8.Text = "Plz Enter data with  Proper Length!"
                        Label6.Text = ""
                        txt_Budg_Remarks.Focus()
                        Exit Sub
                    End If
                    BUDG_REMARKS = " " & BUDG_REMARKS & " "
                    If InStr(1, BUDG_REMARKS, "CREATE", 1) > 0 Or InStr(1, BUDG_REMARKS, "DELETE", 1) > 0 Or InStr(1, BUDG_REMARKS, "DROP", 1) > 0 Or InStr(1, BUDG_REMARKS, "INSERT", 1) > 1 Or InStr(1, BUDG_REMARKS, "TRACK", 1) > 1 Or InStr(1, BUDG_REMARKS, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not use Reserve Words... ');", True)
                        Label8.Text = "Do Not Use Reserve Words!"
                        Label6.Text = ""
                        txt_Budg_Remarks.Focus()
                        Exit Sub
                    End If
                    BUDG_REMARKS = TrimAll(BUDG_REMARKS)
                    c = 0
                    Counter5 = 0
                    For iloop = 1 To Len(BUDG_REMARKS)
                        strcurrentchar = Mid(BUDG_REMARKS, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                Counter5 = 1
                            End If
                        End If
                    Next
                    If Counter5 = 1 Then
                        Label8.Text = "Do Not Use Un-Wanted Charactrs!"
                        Label6.Text = ""
                        txt_Budg_Remarks.Focus()
                        Exit Sub
                    End If
                Else
                    BUDG_REMARKS = ""
                End If


                Dim LIB_CODE As Object = Nothing
                If LibCode = "" Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Plz Login Again!');", True)
                    Label8.Text = "Plz Login Again!"
                    Label6.Text = ""
                    Exit Sub
                Else
                    LIB_CODE = LibCode
                End If

                Dim USER_CODE As Object = Nothing
                USER_CODE = Session.Item("LoggedUser")

                '            '****************************************************************************************************
                '            'UPDATE   
                If Label7.Text <> "" Then
                    SQL = "SELECT * FROM BUDGETS WHERE (BUDG_ID='" & Trim(BUDG_ID) & "')"
                    SqlConn.Open()
                    da = New SqlDataAdapter(SQL, SqlConn)
                    cb = New SqlCommandBuilder(da)
                    da.Fill(ds, "BUDGETS")
                    If ds.Tables("BUDGETS").Rows.Count <> 0 Then
                        If Not String.IsNullOrEmpty(BUDG_YEAR) Then
                            ds.Tables("BUDGETS").Rows(0)("BUDG_YEAR") = BUDG_YEAR.ToString.Trim
                        Else
                            ds.Tables("BUDGETS").Rows(0)("BUDG_YEAR") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(BUDG_PERIOD) Then
                            ds.Tables("BUDGETS").Rows(0)("BUDG_PERIOD") = BUDG_PERIOD.Trim
                        Else
                            ds.Tables("BUDGETS").Rows(0)("BUDG_PERIOD") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(BUDG_AMOUNT) Then
                            ds.Tables("BUDGETS").Rows(0)("BUDG_AMOUNT") = BUDG_AMOUNT.Trim
                        Else
                            ds.Tables("BUDGETS").Rows(0)("BUDG_AMOUNT") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(BUDG_HEAD) Then
                            ds.Tables("BUDGETS").Rows(0)("BUDG_HEAD") = BUDG_HEAD.Trim
                        Else
                            ds.Tables("BUDGETS").Rows(0)("BUDG_HEAD") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(BUDG_REMARKS) Then
                            ds.Tables("BUDGETS").Rows(0)("BUDG_REMARKS") = BUDG_REMARKS.Trim
                        Else
                            ds.Tables("BUDGETS").Rows(0)("BUDG_REMARKS") = System.DBNull.Value
                        End If

                        ds.Tables("BUDGETS").Rows(0)("UPDATED_BY") = UserCode.Trim
                        ds.Tables("BUDGETS").Rows(0)("DATE_MODIFIED") = Now.Date
                        ds.Tables("BUDGETS").Rows(0)("IP") = Request.UserHostAddress.Trim

                        thisTransaction = SqlConn.BeginTransaction()
                        da.SelectCommand.Transaction = thisTransaction
                        da.Update(ds, "BUDGETS")
                        thisTransaction.Commit()

                        BUDG_YEAR = Nothing
                        BUDG_AMOUNT = Nothing
                        BUDG_HEAD = Nothing
                        BUDG_PERIOD = Nothing
                        BUDG_REMARKS = Nothing
                        Label6.Text = "User Record Updated Successfully"
                        Label8.Text = ""
                        ClearFields()
                        PopulateBudgets()
                    Else
                        Label8.Text = "No Record Updated"
                        Label6.Text = ""
                        Exit Sub
                    End If
                End If
            Else
                'record not selected
                Label8.Text = "No Record Selected"
                Label6.Text = ""
                Exit Sub
            End If
            SqlConn.Close()
            ClearFields()
            Label7.Text = ""
            Me.bttn_Save.Visible = True
            Me.bttn_Update.Visible = False
        Catch q As SqlException
            thisTransaction.Rollback()
            Label8.Text = "Error: " & (q.Message())
            Label6.Text = ""
        Catch s As Exception
            Label8.Text = "Error: " & (s.Message())
            Label6.Text = ""
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'delete selected rows
    Protected Sub Delete_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Delete_Bttn.Click
        Try
            For Each row As GridViewRow In Grid_Budgets.Rows
                Dim cb As CheckBox = DirectCast(row.FindControl("cbd"), CheckBox)
                If cb IsNot Nothing AndAlso cb.Checked = True Then
                    Dim NotID As Integer = Convert.ToInt32(Grid_Budgets.DataKeys(row.RowIndex).Value)

                    'chk reference in BILLS Table
                    Dim str As Object = Nothing
                    Dim Flag As Object = Nothing
                    str = "SELECT BUDG_ID FROM BILLS WHERE (BUDG_ID = '" & Trim(NotID) & "') "
                    Dim cmd1 As New SqlCommand(str, SqlConn)
                    SqlConn.Open()
                    Flag = cmd1.ExecuteScalar
                    SqlConn.Close()
                    If Flag <> Nothing Then
                        Label8.Text = "This Budget Record can not be deleted as it has been processed in the Bills ! "
                        Label6.Text = ""
                        Me.txt_Budg_Head.Focus()
                        Exit Sub
                    Else
                        'get cat record
                        Dim SQL As String = Nothing
                        SQL = "DELETE FROM BUDGETS WHERE (BUDG_ID ='" & Trim(NotID) & "') "
                        Dim objCommand As New SqlCommand(SQL, SqlConn)
                        Dim da As New SqlDataAdapter(objCommand)
                        Dim ds As New DataSet
                        da.Fill(ds)
                    End If
                End If
            Next
            Dim s As Object = Nothing
            Dim e1 As EventArgs = Nothing
            PopulateBudgets()
        Catch s As Exception
            Label8.Text = "Error: " & (s.Message())
            Label6.Text = ""
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'autocomplete method for budget head
    <System.Web.Script.Services.ScriptMethod(), _
  System.Web.Services.WebMethod()> _
    Public Shared Function SearchBudgHead(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Return SearchBudgetHead(prefixText, count)
    End Function
    <System.Web.Script.Services.ScriptMethod(), _
System.Web.Services.WebMethod()> _
    Public Shared Function SearchBudgetHead(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim sdr As SqlDataReader
        Dim cmd As SqlCommand = New SqlCommand
        cmd.CommandText = "select DISTINCT BUDG_HEAD from BUDGETS where (BUDG_HEAD like '" + prefixText + "%') AND (LIB_CODE = '" & Trim(LibCode) & "')"
        cmd.Connection = SqlConn
        Try
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            Dim budgetHead As List(Of String) = New List(Of String)
            sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            While sdr.Read
                budgetHead.Add(sdr("BUDG_HEAD").ToString)
            End While
            Return budgetHead
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

    'autocomplete method for budget head
    <System.Web.Script.Services.ScriptMethod(), _
  System.Web.Services.WebMethod()> _
    Public Shared Function SearchBudgYear(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Return SearchBudgetYear(prefixText, count)
    End Function
    <System.Web.Script.Services.ScriptMethod(), _
System.Web.Services.WebMethod()> _
    Public Shared Function SearchBudgetYear(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim sdr As SqlDataReader
        Dim cmd As SqlCommand = New SqlCommand
        cmd.CommandText = "select DISTINCT BUDG_YEAR from BUDGETS where (BUDG_YEAR like '" + prefixText + "%') AND (LIB_CODE = '" & Trim(LibCode) & "')"
        cmd.Connection = SqlConn
        Try
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            Dim budgetYear As List(Of String) = New List(Of String)
            sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            While sdr.Read
                budgetYear.Add(sdr("BUDG_YEAR").ToString)
            End While
            Return budgetYear
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