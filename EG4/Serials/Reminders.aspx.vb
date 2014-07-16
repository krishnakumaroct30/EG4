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

Public Class Reminders
    Inherits System.Web.UI.Page
    Dim index As Integer = 0
    Public Srno As Integer = 0
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session.Item("IsThisU") <> Nothing Then
                If Not SConnection() = True Then
                    Label14.Text = "Database Connection is lost..Try Again !'"
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Database Connection is Lost!');", True)
                Else
                    If Page.IsPostBack = False Then
                        PopulateSubscriptionYears()
                        PopulateTitles()
                        PopulateVendors()
                    End If
                    Dim CreateSUserButton As Object 'System.Web.UI.WebControls.Button
                    CreateSUserButton = Master.FindControl("Accordion1").FindControl("SerPane").FindControl("Ser_Reminders_Bttn")
                    CreateSUserButton.ForeColor = Drawing.Color.Red
                    myPaneName = "SerPane"
                End If
            Else
                Response.Redirect("~/Default.aspx", False)
            End If
            LibCode = Session.Item("LoggedLibcode")
        Catch ex As Exception
            Label14.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    'populate subscription year in  drop-down
    Public Sub PopulateSubscriptionYears()
        Dim Sel As String = "SELECT DISTINCT SUBS_YEAR FROM SUBSCRIPTIONS WHERE (LIB_CODE ='" & (LibCode) & "') AND  (SUBS_YEAR IS NOT NULL) ORDER BY SUBS_YEAR DESC"
        Dim cmd As New SqlCommand(Sel, SqlConn)
        Dim rdr As SqlDataReader = Nothing
        Try
            SqlConn.Open()
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            If rdr.HasRows = True Then
                Me.DDL_SubscriptionYears.DataTextField = "SUBS_YEAR"
                Me.DDL_SubscriptionYears.DataValueField = "SUBS_YEAR"
                Me.DDL_SubscriptionYears.DataSource = rdr
                Me.DDL_SubscriptionYears.DataBind()
                DDL_SubscriptionYears.Items.Insert(0, "")
            Else
                Me.DDL_SubscriptionYears.DataSource = Nothing
                Me.DDL_SubscriptionYears.DataBind()
                DDL_SubscriptionYears.Items.Clear()
            End If
            SqlConn.Close()
        Catch ex As System.Exception
            If rdr IsNot Nothing Then
                rdr.Close()
            Else
                SqlConn.Close()
            End If
            Label14.Text = ex.Message.ToString()
        Finally
            SqlConn.Close()
        End Try
    End Sub
    Protected Sub PopulateTitles()
        Dim dtSearch As DataTable = Nothing
        Try

            Dim SQL As String = Nothing
            SQL = "SELECT DISTINCT SUBSCRIPTIONS.CAT_NO, CATS_AUTHORS_VIEW.TITLE FROM CATS_AUTHORS_VIEW RIGHT OUTER JOIN SUBSCRIPTIONS ON CATS_AUTHORS_VIEW.CAT_NO = SUBSCRIPTIONS.CAT_NO WHERE (SUBSCRIPTIONS.LIB_CODE = '" & Trim(LibCode) & "')" ' AND (SUBSCRIPTIONS.SUBS_YEAR = '" & Trim(SUBS_YEAR) & "')"

            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            da.Fill(ds)

            dtSearch = ds.Tables(0).Copy
            Dim RecordCount As Long = 0
            If dtSearch.Rows.Count = 0 Then
                Me.DDL_Titles.DataSource = Nothing
                DDL_Titles.DataBind()
                DDL_Titles.Items.Clear()
                'Label33.Text = "Titles Being Subscribed in this Year: 0 "
            Else
                RecordCount = dtSearch.Rows.Count
                Me.DDL_Titles.DataSource = dtSearch
                Me.DDL_Titles.DataTextField = "TITLE"
                Me.DDL_Titles.DataValueField = "CAT_NO"
                Me.DDL_Titles.DataBind()
                DDL_Titles.Items.Insert(0, "")
                ' Label33.Text = "Titles Being Subscribed in this Year: " & RecordCount
            End If
        Catch s As Exception
            Label14.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
            DDL_SubscriptionYears.Focus()
        End Try
    End Sub
    'populate subscription year in  drop-down
    Public Sub PopulateVendors()
        DDL_Vendors.DataTextField = "VEND_NAME"
        DDL_Vendors.DataValueField = "VEND_ID"
        DDL_Vendors.DataSource = GetVendorList()
        DDL_Vendors.DataBind()
        DDL_Vendors.Items.Insert(0, "")
    End Sub
    'search
    Protected Sub Reminder_Search_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Reminder_Search_Bttn.Click
        PopulateLooseIssuesGrid()
    End Sub
    Public Sub PopulateLooseIssuesGrid()
        Dim dtSearch As DataTable = Nothing
        Try
            Dim c, counter1, counter2, counter3, counter4, counter5 As Integer
            Dim iloop As Integer
            Dim strcurrentchar As Object

            Dim SUBS_YEAR As Integer = Nothing
            If DDL_SubscriptionYears.Text <> "" Then
                SUBS_YEAR = Trim(DDL_SubscriptionYears.SelectedValue)
                SUBS_YEAR = RemoveQuotes(SUBS_YEAR)

                If Not IsNumeric(SUBS_YEAR) Then
                    Label14.Text = "Error: Subs Year is not Valid!"
                    Me.DDL_SubscriptionYears.Focus()
                    Exit Sub
                End If
                If SUBS_YEAR.ToString.Length <> 4 Then
                    Label14.Text = "Error: Subs Year is not Valid!"
                    Me.DDL_SubscriptionYears.Focus()
                    Exit Sub
                End If
                SUBS_YEAR = " " & SUBS_YEAR & " "
                If InStr(1, SUBS_YEAR, "CREATE", 1) > 0 Or InStr(1, SUBS_YEAR, "DELETE", 1) > 0 Or InStr(1, SUBS_YEAR, "DROP", 1) > 0 Or InStr(1, SUBS_YEAR, "INSERT", 1) > 1 Or InStr(1, SUBS_YEAR, "TRACK", 1) > 1 Or InStr(1, SUBS_YEAR, "TRACE", 1) > 1 Then
                    Label14.Text = "Error:  Input is not Valid !"
                    Me.DDL_SubscriptionYears.Focus()
                    Exit Sub
                End If
                SUBS_YEAR = TrimX(SUBS_YEAR)
                'check unwanted characters
                c = 0
                counter1 = 0
                For iloop = 1 To Len(SUBS_YEAR.ToString)
                    strcurrentchar = Mid(SUBS_YEAR, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter1 = 1
                        End If
                    End If
                Next
                If counter1 = 1 Then
                    Label14.Text = "Error: data is not Valid !"
                    Me.DDL_SubscriptionYears.Focus()
                    Exit Sub
                End If
            Else
                SUBS_YEAR = Nothing
            End If

            Dim CAT_NO As Integer = Nothing
            If DDL_Titles.Text <> "" Then
                CAT_NO = Trim(DDL_Titles.SelectedValue)
                CAT_NO = RemoveQuotes(CAT_NO)

                If Not IsNumeric(CAT_NO) Then
                    Label14.Text = "Error: Cat No is not Valid!"
                    Me.DDL_Titles.Focus()
                    Exit Sub
                End If
                If CAT_NO.ToString.Length > 9 Then
                    Label14.Text = "Error: Cat No is not Valid!"
                    Me.DDL_Titles.Focus()
                    Exit Sub
                End If
                CAT_NO = " " & CAT_NO & " "
                If InStr(1, CAT_NO, "CREATE", 1) > 0 Or InStr(1, CAT_NO, "DELETE", 1) > 0 Or InStr(1, CAT_NO, "DROP", 1) > 0 Or InStr(1, CAT_NO, "INSERT", 1) > 1 Or InStr(1, CAT_NO, "TRACK", 1) > 1 Or InStr(1, CAT_NO, "TRACE", 1) > 1 Then
                    Label14.Text = "Error:  Input is not Valid !"
                    Me.DDL_Titles.Focus()
                    Exit Sub
                End If
                CAT_NO = TrimX(CAT_NO)
                'check unwanted characters
                c = 0
                counter2 = 0
                For iloop = 1 To Len(CAT_NO.ToString)
                    strcurrentchar = Mid(CAT_NO, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter2 = 1
                        End If
                    End If
                Next
                If counter2 = 1 Then
                    Label14.Text = "Error: data is not Valid !"
                    Me.DDL_Titles.Focus()
                    Exit Sub
                End If
            Else
                CAT_NO = Nothing
            End If

            Dim VEND_ID As Integer = Nothing
            If DDL_Vendors.Text <> "" Then
                VEND_ID = Trim(DDL_Vendors.SelectedValue)
                VEND_ID = RemoveQuotes(VEND_ID)

                If Not IsNumeric(VEND_ID) Then
                    Label14.Text = "Error: Vendor id is not Valid!"
                    Me.DDL_Vendors.Focus()
                    Exit Sub
                End If
                If VEND_ID.ToString.Length > 9 Then
                    Label14.Text = "Error: Vendor id is not Valid!"
                    Me.DDL_Vendors.Focus()
                    Exit Sub
                End If
                VEND_ID = " " & VEND_ID & " "
                If InStr(1, VEND_ID, "CREATE", 1) > 0 Or InStr(1, VEND_ID, "DELETE", 1) > 0 Or InStr(1, VEND_ID, "DROP", 1) > 0 Or InStr(1, VEND_ID, "INSERT", 1) > 1 Or InStr(1, VEND_ID, "TRACK", 1) > 1 Or InStr(1, VEND_ID, "TRACE", 1) > 1 Then
                    Label14.Text = "Error:  Input is not Valid !"
                    Me.DDL_Vendors.Focus()
                    Exit Sub
                End If
                VEND_ID = TrimX(VEND_ID)
                'check unwanted characters
                c = 0
                counter3 = 0
                For iloop = 1 To Len(VEND_ID.ToString)
                    strcurrentchar = Mid(VEND_ID, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter3 = 1
                        End If
                    End If
                Next
                If counter3 = 1 Then
                    Label14.Text = "Error: data is not Valid !"
                    Me.DDL_Vendors.Focus()
                    Exit Sub
                End If
            Else
                VEND_ID = Nothing
            End If

            Dim SQL As String = Nothing
            SQL = "SELECT ISS_ID,CAT_NO,SUBS_YEAR,VOL_NO,SUBS_ID,ACQ_ID,VEND_ID, ISSUE_NO,PART_NO, convert(varchar(10),ISS_DATE,103) as ISS_DATE, convert(varchar(10),RECEIVED_DATE,103) as RECEIVED_DATE, RECEIVED,convert(varchar(10),DUE_DATE,103) as DUE_DATE, COPY_ORDERED,COPY_RECEIVED,REMARKS, convert(varchar(10),DATE_ADDED,103) as DATE_ADDED , SUPP,VEND_NAME,VEND_PLACE,CON_CODE,VEND_ADDRESS,CONTACT_PERSON,TITLE,SUBS_NO FROM JOURNALS_REMINDER_VIEW "
            SQL = SQL & " WHERE (DUE_DATE < GetDate() And LOOSE_ISSUE_LIB_CODE='" & Trim(LibCode) & "') AND (COPY_RECEIVED < COPY_ORDERED or RECEIVED='N') "
            If SUBS_YEAR = Nothing And CAT_NO = Nothing And VEND_ID = Nothing Then
                SQL = SQL
            End If

            If SUBS_YEAR = Nothing And CAT_NO <> Nothing And VEND_ID = Nothing Then
                SQL = SQL & " AND (CAT_NO = '" & Trim(CAT_NO) & "') "
            End If
            If SUBS_YEAR <> Nothing And CAT_NO <> Nothing And VEND_ID = Nothing Then
                SQL = SQL & " AND (CAT_NO = '" & Trim(CAT_NO) & "') and (SUBS_YEAR = '" & Trim(SUBS_YEAR) & "') "
            End If

            If SUBS_YEAR = Nothing And CAT_NO = Nothing And VEND_ID <> Nothing Then
                SQL = SQL & " AND (VEND_ID = '" & Trim(VEND_ID) & "') "
            End If

            If SUBS_YEAR <> Nothing And CAT_NO = Nothing And VEND_ID <> Nothing Then
                SQL = SQL & " AND (VEND_ID = '" & Trim(VEND_ID) & "')  and (SUBS_YEAR = '" & Trim(SUBS_YEAR) & "') "
            End If

            If SUBS_YEAR <> Nothing And CAT_NO = Nothing And VEND_ID = Nothing Then
                SQL = SQL & " AND (SUBS_YEAR = '" & Trim(SUBS_YEAR) & "') "
            End If

            If SUBS_YEAR <> Nothing And CAT_NO <> Nothing And VEND_ID <> Nothing Then
                SQL = SQL & " AND (CAT_NO = '" & Trim(CAT_NO) & "') AND (VEND_ID = '" & Trim(VEND_ID) & "')  and (SUBS_YEAR = '" & Trim(SUBS_YEAR) & "') "
            End If

            SQL = SQL & " ORDER BY TITLE, SUBS_YEAR ASC, "
            SQL = SQL & " CASE WHEN LEFT(VOL_NO,1) LIKE  '[A-Za-z]'  THEN dbo.Sort_AlphaNumeric(VOL_NO, 'A-Za-z') END ASC, CAST(dbo.Sort_AlphaNumeric(VOL_NO, '0-9') AS float) ASC ,"
            SQL = SQL & " CASE WHEN LEFT(ISSUE_NO,1) LIKE  '[A-Za-z]'  THEN dbo.Sort_AlphaNumeric(ISSUE_NO, 'A-Za-z') END ASC, CAST(dbo.Sort_AlphaNumeric(ISSUE_NO, '0-9') AS float) ASC  ,"
            SQL = SQL & " CASE WHEN LEFT(PART_NO,1) LIKE  '[A-Za-z]'  THEN dbo.Sort_AlphaNumeric(PART_NO, 'A-Za-z') END ASC, CAST(dbo.Sort_AlphaNumeric(PART_NO, '0-9') AS float) ASC ;"


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
                Label37.Text = "Total Record(s): 0 "
            Else
                Grid1.Visible = True
                RecordCount = dtSearch.Rows.Count
                Grid1.DataSource = dtSearch
                Grid1.DataBind()
                Label37.Text = "Total Record(s): " & RecordCount
            End If
            ViewState("dt") = dtSearch

        Catch s As Exception
            Label14.Text = "Error: " & (s.Message())
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
            Label14.Text = "Error:  there is error in page index !"
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
        Dim dr As SqlDataReader = Nothing
        Try
            txt_Loose_VolNo.Text = ""
            txt_Loose_IssueNo.Text = ""
            txt_Loose_PartNo.Text = ""
            txt_Loose_IssueDate.Text = ""
            txt_Loose_DueDate.Text = ""
            txt_Loose_Recd.Text = ""
            txt_Loose_Remarks.Text = ""
            txt_Loose_Copy.Text = ""
            RadioButton1.Checked = True
            Label36.Text = ""

            If e.CommandName = "Select" Then
                Dim myRowID, ISS_ID As Integer
                myRowID = e.CommandArgument.ToString()
                ISS_ID = Grid1.DataKeys(myRowID).Value
                If Not String.IsNullOrEmpty(ISS_ID) And ISS_ID <> 0 Then
                    Label36.Text = ISS_ID
                    Dim iloop As Integer
                    Dim strcurrentchar As Object
                    Dim c As Integer
                    Dim counter1 As Integer

                    ISS_ID = TrimX(ISS_ID)
                    ISS_ID = RemoveQuotes(ISS_ID)

                    If Not IsNumeric(ISS_ID.ToString) Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Input is not Proper... ');", True)
                        Exit Sub
                    End If

                    If Len(ISS_ID).ToString > 10 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Length of Input is not Proper... ');", True)
                        Exit Sub
                    End If
                    ISS_ID = " " & ISS_ID & " "
                    If InStr(1, ISS_ID, " CREATE ", 1) > 0 Or InStr(1, ISS_ID, " DELETE ", 1) > 0 Or InStr(1, ISS_ID, " DROP ", 1) > 0 Or InStr(1, ISS_ID, " INSERT ", 1) > 1 Or InStr(1, ISS_ID, " TRACK ", 1) > 1 Or InStr(1, ISS_ID, " TRACE ", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do not use reserve words... ');", True)
                        Exit Sub
                    End If
                    ISS_ID = TrimX(ISS_ID)
                    'get record details from database
                    Dim SQL As String = Nothing
                    SQL = " SELECT *  FROM LOOSE_ISSUES WHERE (ISS_ID = '" & Trim(ISS_ID) & "') AND (LIB_CODE ='" & Trim(LibCode) & "') "
                    Command = New SqlCommand(SQL, SqlConn)
                    SqlConn.Open()
                    dr = Command.ExecuteReader(CommandBehavior.CloseConnection)
                    dr.Read()
                    If dr.HasRows = True Then
                        If dr.Item("VOL_NO").ToString <> "" Then
                            txt_Loose_VolNo.Text = dr.Item("VOL_NO").ToString
                        Else
                            txt_Loose_VolNo.Text = ""
                        End If
                        If dr.Item("ISSUE_NO").ToString <> "" Then
                            txt_Loose_IssueNo.Text = dr.Item("ISSUE_NO").ToString
                        Else
                            txt_Loose_IssueNo.Text = ""
                        End If
                        If dr.Item("PART_NO").ToString <> "" Then
                            txt_Loose_PartNo.Text = dr.Item("PART_NO").ToString
                        Else
                            txt_Loose_PartNo.Text = ""
                        End If
                        If dr.Item("ISS_DATE").ToString <> "" Then
                            txt_Loose_IssueDate.Text = Format(dr.Item("ISS_DATE"), "dd/MM/yyyy")
                        Else
                            txt_Loose_IssueDate.Text = ""
                        End If

                        If dr.Item("DUE_DATE").ToString <> "" Then
                            txt_Loose_DueDate.Text = Format(dr.Item("DUE_DATE"), "dd/MM/yyyy")
                        Else
                            txt_Loose_DueDate.Text = ""
                        End If

                        If dr.Item("RECEIVED_DATE").ToString <> "" Then
                            txt_Loose_ReceivedDate.Text = Format(dr.Item("RECEIVED_DATE"), "dd/MM/yyyy")
                        Else
                            txt_Loose_ReceivedDate.Text = ""
                        End If

                        If dr.Item("RECEIVED").ToString <> "" Then
                            txt_Loose_Recd.Text = dr.Item("RECEIVED").ToString
                        Else
                            txt_Loose_Recd.Text = ""
                        End If

                        If dr.Item("COPY_ORDERED").ToString <> "" Then
                            txt_Loose_Copy.Text = dr.Item("COPY_ORDERED").ToString
                        Else
                            txt_Loose_Copy.Text = 1
                        End If

                        If dr.Item("COPY_RECEIVED").ToString <> "" Then
                            txt_Loose_CopyAlreadyRecd.Text = dr.Item("COPY_RECEIVED").ToString
                        Else
                            txt_Loose_CopyAlreadyRecd.Text = ""
                        End If

                        If dr.Item("REMARKS").ToString <> "" Then
                            txt_Loose_Remarks.Text = TrimAll(dr.Item("REMARKS").ToString)
                        Else
                            txt_Loose_Remarks.Text = ""
                        End If

                        If dr.Item("SUPP").ToString <> "" Then
                            RadioButton2.Checked = True
                        ElseIf dr.Item("SPL").ToString <> "" Then
                            RadioButton3.Checked = True
                        ElseIf dr.Item("INDEX_ISSUE").ToString <> "" Then
                            RadioButton4.Checked = True
                        Else
                            RadioButton1.Checked = True
                        End If
                        dr.Close()
                        SqlConn.Close()
                    Else
                        Label36.Text = ""
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('No Record to Receive!');", True)
                    End If
                Else
                    Label36.Text = ""
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' No Record to Receive!');", True)
                End If
            End If
        Catch s As Exception
            Label14.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
End Class