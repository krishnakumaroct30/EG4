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

Public Class LooseIssues
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
                    End If
                    Dim CreateSUserButton As Object 'System.Web.UI.WebControls.Button
                    CreateSUserButton = Master.FindControl("Accordion1").FindControl("SerPane").FindControl("Ser_ReceiveLoose_Bttn")
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
    Protected Sub DDL_SubscriptionYears_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDL_SubscriptionYears.SelectedIndexChanged
        Dim dtSearch As DataTable = Nothing
        Try
            'clear Title details
            Label19.Text = ""
            Label16.Text = ""
            Label17.Text = ""
            Label18.Text = ""
            Label23.Text = ""
            Image4.ImageUrl = Nothing
            Image4.Visible = True
            DDL_Titles.ClearSelection()
            DDL_Titles_SelectedIndexChanged(sender, e)
            ClearFields()
            Dim c, counter1, counter2, counter3, counter4, counter5 As Integer
            Dim iloop As Integer
            Dim strcurrentchar As Object

            Dim SUBS_YEAR As Integer = Nothing
            If DDL_SubscriptionYears.Text <> "" Then
                SUBS_YEAR = TrimAll(DDL_SubscriptionYears.SelectedValue)
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
            End If

            Dim SQL As String = Nothing
            SQL = "SELECT SUBSCRIPTIONS.SUBS_ID, SUBSCRIPTIONS.CAT_NO, CATS_AUTHORS_VIEW.TITLE FROM CATS_AUTHORS_VIEW RIGHT OUTER JOIN SUBSCRIPTIONS ON CATS_AUTHORS_VIEW.CAT_NO = SUBSCRIPTIONS.CAT_NO WHERE (SUBSCRIPTIONS.LIB_CODE = '" & Trim(LibCode) & "') AND (SUBSCRIPTIONS.SUBS_YEAR = '" & Trim(SUBS_YEAR) & "')"

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
                Label33.Text = "Titles Being Subscribed in this Year: 0 "
            Else
                RecordCount = dtSearch.Rows.Count
                Me.DDL_Titles.DataSource = dtSearch
                Me.DDL_Titles.DataTextField = "TITLE"
                Me.DDL_Titles.DataValueField = "CAT_NO"
                Me.DDL_Titles.DataBind()
                DDL_Titles.Items.Insert(0, "")
                Label33.Text = "Titles Being Subscribed in this Year: " & RecordCount
            End If
        Catch s As Exception
            Label14.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
            DDL_SubscriptionYears.Focus()
        End Try
    End Sub
    'load / display fields
    Protected Sub DDL_Titles_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDL_Titles.SelectedIndexChanged
        Dim dt As New DataTable
        Try
            ClearFields()

            Dim myCatNo As Integer = Nothing
            If DDL_Titles.Text <> "" Then
                myCatNo = DDL_Titles.SelectedValue

                Dim SQL As String = Nothing
                If myCatNo <> 0 Then
                    SQL = "SELECT CATS.CAT_NO, CATS.TITLE, CATS.SUB_TITLE, CATS.EDITOR, CATS.CORPORATE_AUTHOR, CATS.PHOTO, PUBLISHERS.PUB_NAME, CATS.PLACE_OF_PUB FROM CATS  LEFT OUTER JOIN PUBLISHERS ON CATS.PUB_ID = PUBLISHERS.PUB_ID WHERE (CATS.CAT_NO = '" & Trim(myCatNo) & "') "
                End If
                Dim ds As New DataSet
                Dim da As New SqlDataAdapter(SQL, SqlConn)
                If SqlConn.State = 0 Then
                    SqlConn.Open()
                End If
                da.Fill(ds)

                dt = ds.Tables(0).Copy
                SqlConn.Close()

                If dt.Rows.Count <> 0 Then
                    If dt.Rows(0).Item("TITLE").ToString <> "" Then
                        Label19.Text = dt.Rows(0).Item("CAT_NO").ToString
                        If dt.Rows(0).Item("SUB_TITLE").ToString <> "" Then
                            Label16.Text = dt.Rows(0).Item("TITLE").ToString & ": " & dt.Rows(0).Item("SUB_TITLE").ToString
                        Else
                            Label16.Text = dt.Rows(0).Item("TITLE").ToString
                        End If
                    Else
                        Label16.Text = ""
                        Label19.Text = ""
                    End If

                    'Editor
                    If dt.Rows(0).Item("EDITOR").ToString <> "" Then
                        Label17.Text = dt.Rows(0).Item("EDITOR").ToString
                    Else
                        Label17.Text = ""
                    End If
                    'publisher
                    If dt.Rows(0).Item("PUB_NAME").ToString <> "" Then
                        If dt.Rows(0).Item("PLACE_OF_PUB").ToString <> "" Then
                            Label18.Text = dt.Rows(0).Item("PUB_NAME").ToString & "; " & dt.Rows(0).Item("PLACE_OF_PUB").ToString
                        Else
                            Label18.Text = dt.Rows(0).Item("PUB_NAME").ToString
                        End If
                    Else
                        Label18.Text = dt.Rows(0).Item("PUB_NAME").ToString
                    End If

                    'multi-vol
                    If dt.Rows(0).Item("CORPORATE_AUTHOR").ToString <> "" Then
                        Label23.Text = dt.Rows(0).Item("CORPORATE_AUTHOR").ToString
                    Else
                        Label23.Text = ""
                    End If

                    'photo
                    If dt.Rows(0).Item("PHOTO").ToString <> "" Then
                        Dim strURL As String = "~/Acquisition/Cats_GetPhoto.aspx?CAT_NO=" & myCatNo & ""
                        Image4.ImageUrl = strURL
                        Image4.Visible = True
                    Else
                        Image4.ImageUrl = Nothing
                        Image4.Visible = True
                    End If
                Else
                    Label19.Text = ""
                    Label16.Text = ""
                    Label17.Text = ""
                    Label18.Text = ""
                    Label23.Text = ""
                    Image4.ImageUrl = Nothing
                    Image4.Visible = True
                End If
            Else
                Label19.Text = ""
                Label16.Text = ""
                Label17.Text = ""
                Label18.Text = ""
                Label23.Text = ""
                Image4.ImageUrl = Nothing
                Image4.Visible = True
            End If
            PopulateLooseIssuesGrid()
        Catch ex As Exception
            Label14.Text = "Error: " & (ex.Message())
        Finally
            SqlConn.Close()
            DDL_Titles.Focus()
        End Try
    End Sub
    'fill Grid with Approved Acq Records
    Public Sub PopulateLooseIssuesGrid()
        Dim dtSearch As DataTable = Nothing
        Try
            If DDL_SubscriptionYears.Text <> "" Then
                If DDL_Titles.Text <> "" Then

                    Dim SUBS_YEAR As Object = Nothing
                    Dim SQL As String = Nothing
                    SQL = "SELECT * FROM LOOSE_ISSUES WHERE (LIB_CODE = '" & Trim(LibCode) & "') AND (CAT_NO = '" & DDL_Titles.SelectedValue & "')  AND (SUBS_YEAR = '" & Trim(DDL_SubscriptionYears.SelectedValue) & "') "
                    SQL = SQL & " ORDER BY CASE WHEN LEFT(VOL_NO,1) LIKE  '[A-Za-z]'  THEN dbo.Sort_AlphaNumeric(VOL_NO, 'A-Za-z') END ASC, CAST(dbo.Sort_AlphaNumeric(VOL_NO, '0-9') AS float) ASC ,"
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
                        Label35.Text = "Total Record(s): 0 "
                    Else
                        Grid1.Visible = True
                        RecordCount = dtSearch.Rows.Count
                        Grid1.DataSource = dtSearch
                        Grid1.DataBind()
                        Label35.Text = "Total Record(s): " & RecordCount
                    End If
                    ViewState("dt") = dtSearch
                Else
                    Me.Grid1.DataSource = Nothing
                    Grid1.DataBind()
                    Label35.Text = "Total Record(s): 0 "
                End If
            Else
                Me.Grid1.DataSource = Nothing
                Grid1.DataBind()
                Label35.Text = "Total Record(s): 0 "
            End If
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
    Public Sub ClearFields()
        txt_Loose_VolNo.Text = ""
        txt_Loose_IssueNo.Text = ""
        txt_Loose_PartNo.Text = ""
        txt_Loose_IssueDate.Text = ""
        txt_Loose_DueDate.Text = ""
        txt_Loose_Recd.Text = ""
        txt_Loose_Remarks.Text = ""
        txt_Loose_Copy.Text = ""
        txt_Loose_CopyAlreadyRecd.Text = ""
        Label36.Text = ""
    End Sub
    'cancel button
    Protected Sub Loose_Cancel_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Loose_Cancel_Bttn.Click
        Loose_Save_Bttn.Visible = False
        Loose_Delete_Bttn.Visible = False
        ClearFields()
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
                        Loose_Delete_Bttn.Visible = True
                        Loose_Save_Bttn.Visible = True
                        dr.Close()
                        SqlConn.Close()
                    Else
                        Label36.Text = ""
                        Loose_Delete_Bttn.Visible = False
                        Loose_Save_Bttn.Visible = False
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('No Record to Receive!');", True)
                    End If
                Else
                    Label36.Text = ""
                    Loose_Delete_Bttn.Visible = False
                    Loose_Save_Bttn.Visible = False
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' No Record to Receive!');", True)
                End If
            End If
        Catch s As Exception
            Label14.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub 'Grid1_ItemCommand
    'recv loose issues
    Protected Sub Loose_Save_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Loose_Save_Bttn.Click
        Dim SQL As String = Nothing
        Dim da As SqlDataAdapter
        Dim cb As SqlCommandBuilder
        Dim ds As New DataSet
        Dim dt As New DataTable
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing
        Try
            If IsPostBack = True Then
                Dim iloop As Integer
                Dim strcurrentchar As Object
                Dim c As Integer
                Dim counter1, counter2, counter3, counter4, counter5, counter6, counter7, counter8, counter9, counter10, counter11, counter12, counter13, counter14, counter15, counter16, counter17, counter18, counter19 As Integer

                If DDL_Titles.Text = "" Then
                    Label14.Text = "Plz Select Title from Drop-Down!"
                    DDL_Titles.Focus()
                    Exit Sub
                End If

                Dim ISS_ID As Integer = Nothing
                If Me.Label36.Text <> "" Then
                    ISS_ID = Trim(Label36.Text)
                    ISS_ID = RemoveQuotes(ISS_ID)
                    If Not IsNumeric(ISS_ID) = True Then
                        Label14.Text = "Error: Input is not Valid !"
                        DDL_Titles.Focus()
                        Exit Sub
                    End If
                    If ISS_ID.ToString.Length > 5 Then
                        Label14.Text = "Error: Input length is not Valid !"
                        DDL_Titles.Focus()
                        Exit Sub
                    End If
                    ISS_ID = " " & ISS_ID & " "
                    If InStr(1, ISS_ID, "CREATE", 1) > 0 Or InStr(1, ISS_ID, "DELETE", 1) > 0 Or InStr(1, ISS_ID, "DROP", 1) > 0 Or InStr(1, ISS_ID, "INSERT", 1) > 1 Or InStr(1, ISS_ID, "TRACK", 1) > 1 Or InStr(1, ISS_ID, "TRACE", 1) > 1 Then
                        Label14.Text = "Error: Do not use Reserve Word !"
                        DDL_Titles.Focus()
                        Exit Sub
                    End If
                    ISS_ID = TrimX(ISS_ID)
                    'check unwanted characters
                    c = 0
                    counter1 = 0
                    For iloop = 1 To Len(ISS_ID.ToString)
                        strcurrentchar = Mid(ISS_ID, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter1 = 1
                            End If
                        End If
                    Next
                    If counter1 = 1 Then
                        Label14.Text = "Error: Do not useun-wanted characters !"
                        DDL_Titles.Focus()
                        Exit Sub
                    End If
                Else
                    Label14.Text = "Error: Plz Select Title from Drop-Down !"
                    DDL_Titles.Focus()
                    Exit Sub
                End If

                'validation for VOL_NO
                Dim VOL_NO As Object = Nothing
                If Me.txt_Loose_VolNo.Text <> "" Then
                    VOL_NO = TrimAll(txt_Loose_VolNo.Text)
                    VOL_NO = RemoveQuotes(VOL_NO)
                   
                    If Len(VOL_NO) > 20 Then
                        Label14.Text = "Error: Input is not Valid !"
                        txt_Loose_VolNo.Focus()
                        Exit Sub
                    End If
                    VOL_NO = " " & VOL_NO & " "
                    If InStr(1, VOL_NO, "CREATE", 1) > 0 Or InStr(1, VOL_NO, "DELETE", 1) > 0 Or InStr(1, VOL_NO, "DROP", 1) > 0 Or InStr(1, VOL_NO, "INSERT", 1) > 1 Or InStr(1, VOL_NO, "TRACK", 1) > 1 Or InStr(1, VOL_NO, "TRACE", 1) > 1 Then
                        Label14.Text = "Error: Input is not Valid !"
                        txt_Loose_VolNo.Focus()
                        Exit Sub
                    End If
                    VOL_NO = TrimAll(VOL_NO)
                Else
                    VOL_NO = ""
                End If

                'validation for ISSUE_NO
                Dim ISSUE_NO As Object = Nothing
                If Me.txt_Loose_IssueNo.Text <> "" Then
                    ISSUE_NO = TrimAll(txt_Loose_IssueNo.Text)
                    ISSUE_NO = RemoveQuotes(ISSUE_NO)

                    If Len(ISSUE_NO) > 20 Then
                        Label14.Text = "Error: Input is not Valid !"
                        txt_Loose_IssueNo.Focus()
                        Exit Sub
                    End If
                    ISSUE_NO = " " & ISSUE_NO & " "
                    If InStr(1, ISSUE_NO, "CREATE", 1) > 0 Or InStr(1, ISSUE_NO, "DELETE", 1) > 0 Or InStr(1, ISSUE_NO, "DROP", 1) > 0 Or InStr(1, ISSUE_NO, "INSERT", 1) > 1 Or InStr(1, ISSUE_NO, "TRACK", 1) > 1 Or InStr(1, ISSUE_NO, "TRACE", 1) > 1 Then
                        Label14.Text = "Error: Input is not Valid !"
                        txt_Loose_IssueNo.Focus()
                        Exit Sub
                    End If
                    ISSUE_NO = TrimAll(ISSUE_NO)
                Else
                    ISSUE_NO = ""
                End If

                'validation for PART_NO
                Dim PART_NO As Object = Nothing
                If Me.txt_Loose_PartNo.Text <> "" Then
                    PART_NO = TrimAll(txt_Loose_PartNo.Text)
                    PART_NO = RemoveQuotes(PART_NO)

                    If Len(PART_NO) > 20 Then
                        Label14.Text = "Error: Input is not Valid !"
                        txt_Loose_PartNo.Focus()
                        Exit Sub
                    End If
                    PART_NO = " " & PART_NO & " "
                    If InStr(1, PART_NO, "CREATE", 1) > 0 Or InStr(1, PART_NO, "DELETE", 1) > 0 Or InStr(1, PART_NO, "DROP", 1) > 0 Or InStr(1, PART_NO, "INSERT", 1) > 1 Or InStr(1, PART_NO, "TRACK", 1) > 1 Or InStr(1, PART_NO, "TRACE", 1) > 1 Then
                        Label14.Text = "Error: Input is not Valid !"
                        txt_Loose_PartNo.Focus()
                        Exit Sub
                    End If
                    PART_NO = TrimAll(PART_NO)
                Else
                    PART_NO = ""
                End If

                'issue  date
                Dim ISSUE_DATE As Object = Nothing
                If txt_Loose_IssueDate.Text <> "" Then
                    ISSUE_DATE = TrimX(txt_Loose_IssueDate.Text)

                    If Len(ISSUE_DATE) <> 10 Then
                        Label14.Text = " Input is not of proper length!"
                        Me.txt_Loose_IssueDate.Focus()
                        Exit Sub
                    End If
                    ISSUE_DATE = " " & ISSUE_DATE & " "
                    If InStr(1, ISSUE_DATE, "CREATE", 1) > 0 Or InStr(1, ISSUE_DATE, "DELETE", 1) > 0 Or InStr(1, ISSUE_DATE, "DROP", 1) > 0 Or InStr(1, ISSUE_DATE, "INSERT", 1) > 1 Or InStr(1, ISSUE_DATE, "TRACK", 1) > 1 Or InStr(1, ISSUE_DATE, "TRACE", 1) > 1 Then
                        Label14.Text = "  Error: Do not use Reserve Word!"
                        Me.txt_Loose_IssueDate.Focus()
                        Exit Sub
                    End If
                    ISSUE_DATE = TrimX(ISSUE_DATE)
                    'check unwanted characters
                    c = 0
                    counter2 = 0
                    For iloop = 1 To Len(ISSUE_DATE)
                        strcurrentchar = Mid(ISSUE_DATE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter2 = 1
                            End If
                        End If
                    Next
                    If counter2 = 1 Then
                        Label14.Text = "Error: Do not use un-wanted characters!"
                        Me.txt_Loose_IssueDate.Focus()
                        Exit Sub
                    End If
                    ISSUE_DATE = Convert.ToDateTime(ISSUE_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us
                Else
                    Label14.Text = "Plz Enter Issue Date !"
                    Me.txt_Loose_IssueDate.Focus()
                    Exit Sub
                End If


                'Due  date
                Dim DUE_DATE As Object = Nothing
                If txt_Loose_IssueDate.Text <> "" Then
                    DUE_DATE = TrimX(txt_Loose_IssueDate.Text)

                    If Len(DUE_DATE) <> 10 Then
                        Label14.Text = " Input is not of proper length!"
                        Me.txt_Loose_IssueDate.Focus()
                        Exit Sub
                    End If
                    DUE_DATE = " " & DUE_DATE & " "
                    If InStr(1, DUE_DATE, "CREATE", 1) > 0 Or InStr(1, DUE_DATE, "DELETE", 1) > 0 Or InStr(1, DUE_DATE, "DROP", 1) > 0 Or InStr(1, DUE_DATE, "INSERT", 1) > 1 Or InStr(1, DUE_DATE, "TRACK", 1) > 1 Or InStr(1, DUE_DATE, "TRACE", 1) > 1 Then
                        Label14.Text = "  Error: Do not use Reserve Word!"
                        Me.txt_Loose_IssueDate.Focus()
                        Exit Sub
                    End If
                    DUE_DATE = TrimX(DUE_DATE)
                    'check unwanted characters
                    c = 0
                    counter3 = 0
                    For iloop = 1 To Len(DUE_DATE)
                        strcurrentchar = Mid(DUE_DATE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter3 = 1
                            End If
                        End If
                    Next
                    If counter3 = 1 Then
                        Label14.Text = "Error: Do not use un-wanted characters!"
                        Me.txt_Loose_IssueDate.Focus()
                        Exit Sub
                    End If
                    DUE_DATE = Convert.ToDateTime(DUE_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us
                Else
                    DUE_DATE = Nothing
                End If

                Dim RECEIVED_DATE As Object = Nothing
                RECEIVED_DATE = Now.Date
                RECEIVED_DATE = Convert.ToDateTime(RECEIVED_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                Dim TOTAL_COPY As Integer = Nothing
                Dim COPY As Integer = Nothing
                If txt_Loose_Copy.Text <> "" Then
                    COPY = TrimX(txt_Loose_Copy.Text)
                    COPY = RemoveQuotes(COPY)
                    If Not IsNumeric(COPY) = True Then
                        Label14.Text = "Error: Input is not Valid !"
                        txt_Loose_Copy.Focus()
                        Exit Sub
                    End If
                    If COPY.ToString.Length > 8 Then
                        Label14.Text = "Error: Input length is not Valid !"
                        txt_Loose_Copy.Focus()
                        Exit Sub
                    End If
                    COPY = " " & COPY & " "
                    If InStr(1, COPY, "CREATE", 1) > 0 Or InStr(1, COPY, "DELETE", 1) > 0 Or InStr(1, COPY, "DROP", 1) > 0 Or InStr(1, COPY, "INSERT", 1) > 1 Or InStr(1, COPY, "TRACK", 1) > 1 Or InStr(1, COPY, "TRACE", 1) > 1 Then
                        Label14.Text = "Error: Do not use Reserve Word !"
                        txt_Loose_Copy.Focus()
                        Exit Sub
                    End If
                    COPY = TrimX(COPY)
                    'check unwanted characters
                    c = 0
                    counter4 = 0
                    For iloop = 1 To Len(COPY.ToString)
                        strcurrentchar = Mid(COPY, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter4 = 1
                            End If
                        End If
                    Next
                    If counter4 = 1 Then
                        Label14.Text = "Error: Do not useun-wanted characters !"
                        txt_Loose_Copy.Focus()
                        Exit Sub
                    End If

                    'If COPY < 0 Then
                    'COPY = 0
                    ' TOTAL_COPY = COPY
                    'Else
                    If txt_Loose_CopyAlreadyRecd.Text <> "" Then
                        If COPY < 0 Then
                            COPY = Replace(COPY, "-", "")
                            TOTAL_COPY = Convert.ToInt32(txt_Loose_CopyAlreadyRecd.Text) - COPY
                        Else
                            TOTAL_COPY = COPY + Convert.ToInt32(txt_Loose_CopyAlreadyRecd.Text)
                        End If
                    Else
                        If COPY < 0 Then
                            Exit Sub
                        Else
                            TOTAL_COPY = COPY
                        End If
                    End If
                    ' End If
                Else
                    TOTAL_COPY = 1
                End If

                'Server validation for  : txt_Cat_Remarks
                Dim REMARKS As Object = Nothing
                If txt_Loose_Remarks.Text <> "" Then
                    REMARKS = TrimAll(txt_Loose_Remarks.Text)
                    REMARKS = RemoveQuotes(REMARKS)
                    If REMARKS.Length > 250 Then 'maximum length
                        Label14.Text = " Data must be of Proper Length.. "
                        txt_Loose_Remarks.Focus()
                        Exit Sub
                    End If
                Else
                    REMARKS = ""
                End If

                Dim SUPP As Object = Nothing
                If RadioButton2.Checked = True Then
                    SUPP = "Y"
                Else
                    SUPP = ""
                End If

                Dim SPL As Object = Nothing
                If RadioButton3.Checked = True Then
                    SPL = "Y"
                Else
                    SPL = ""
                End If

                Dim INDEX_ISSUE As Object = Nothing
                If RadioButton4.Checked = True Then
                    INDEX_ISSUE = "Y"
                Else
                    INDEX_ISSUE = ""
                End If

                Dim LIB_CODE As Object = Nothing
                If Session.Item("LoggedLibcode") <> "" Then
                    LIB_CODE = Session.Item("LoggedLibcode")
                Else
                    Label6.Text = "No Library Code Exists..Login Again  "
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Database Connection is Lost!');", True)
                    Exit Sub
                End If

                Dim USER_CODE As Object = Nothing
                USER_CODE = Session.Item("LoggedUser")

                Dim DATE_ADDED As Object = Nothing
                DATE_ADDED = Now.Date
                DATE_ADDED = Convert.ToDateTime(DATE_ADDED, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us


                Dim IP As Object = Nothing
                IP = Request.UserHostAddress.Trim

                'UPDATE THE Record  
                If Label36.Text <> "" Then
                    SQL = "SELECT * FROM LOOSE_ISSUES WHERE (ISS_ID='" & Trim(ISS_ID) & "') and (LIB_CODE = '" & Trim(LibCode) & "') "
                    SqlConn.Open()
                    da = New SqlDataAdapter(SQL, SqlConn)
                    cb = New SqlCommandBuilder(da)
                    da.Fill(ds, "LOOSE")
                    If ds.Tables("LOOSE").Rows.Count <> 0 Then
                        If VOL_NO = "" Then
                            ds.Tables("LOOSE").Rows(0)("VOL_NO") = System.DBNull.Value
                        Else
                            ds.Tables("LOOSE").Rows(0)("VOL_NO") = VOL_NO
                        End If

                        If ISSUE_NO = "" Then
                            ds.Tables("LOOSE").Rows(0)("ISSUE_NO") = System.DBNull.Value
                        Else
                            ds.Tables("LOOSE").Rows(0)("ISSUE_NO") = ISSUE_NO
                        End If

                        If PART_NO = "" Then
                            ds.Tables("LOOSE").Rows(0)("PART_NO") = System.DBNull.Value
                        Else
                            ds.Tables("LOOSE").Rows(0)("PART_NO") = PART_NO
                        End If

                        If Not ISSUE_DATE = Nothing Then
                            ds.Tables("LOOSE").Rows(0)("ISS_DATE") = ISSUE_DATE
                        Else
                            ds.Tables("LOOSE").Rows(0)("ISS_DATE") = System.DBNull.Value
                        End If

                        'If Not RECEIVED_DATE = Nothing Then
                        '    ds.Tables("LOOSE").Rows(0)("RECEIVED_DATE") = RECEIVED_DATE
                        'Else
                        '    ds.Tables("LOOSE").Rows(0)("RECEIVED_DATE") = System.DBNull.Value
                        'End If

                        If Not DUE_DATE = Nothing Then
                            ds.Tables("LOOSE").Rows(0)("DUE_DATE") = DUE_DATE
                        Else
                            ds.Tables("LOOSE").Rows(0)("DUE_DATE") = System.DBNull.Value
                        End If

                        If Not REMARKS = Nothing Then
                            ds.Tables("LOOSE").Rows(0)("REMARKS") = REMARKS
                        Else
                            ds.Tables("LOOSE").Rows(0)("REMARKS") = System.DBNull.Value
                        End If

                        If Not SUPP = Nothing Then
                            ds.Tables("LOOSE").Rows(0)("SUPP") = SUPP
                        Else
                            ds.Tables("LOOSE").Rows(0)("SUPP") = System.DBNull.Value
                        End If

                        If Not SPL = Nothing Then
                            ds.Tables("LOOSE").Rows(0)("SPL") = SPL
                        Else
                            ds.Tables("LOOSE").Rows(0)("SPL") = System.DBNull.Value
                        End If

                        If Not INDEX_ISSUE = Nothing Then
                            ds.Tables("LOOSE").Rows(0)("INDEX_ISSUE") = INDEX_ISSUE
                        Else
                            ds.Tables("LOOSE").Rows(0)("INDEX_ISSUE") = System.DBNull.Value
                        End If

                        If TOTAL_COPY <= 0 Then
                            ds.Tables("LOOSE").Rows(0)("COPY_RECEIVED") = System.DBNull.Value
                            ds.Tables("LOOSE").Rows(0)("RECEIVED") = "N"
                            ds.Tables("LOOSE").Rows(0)("RECEIVED_DATE") = System.DBNull.Value
                        Else
                            ds.Tables("LOOSE").Rows(0)("COPY_RECEIVED") = TOTAL_COPY
                            ds.Tables("LOOSE").Rows(0)("RECEIVED") = "Y"
                            ds.Tables("LOOSE").Rows(0)("RECEIVED_DATE") = RECEIVED_DATE
                        End If

                        'ds.Tables("LOOSE").Rows(0)("RECEIVED") = "Y"
                        ds.Tables("LOOSE").Rows(0)("UPDATED_BY") = UserCode.Trim
                        ds.Tables("LOOSE").Rows(0)("DATE_MODIFIED") = Now.Date
                        ds.Tables("LOOSE").Rows(0)("IP") = Request.UserHostAddress.Trim

                        thisTransaction = SqlConn.BeginTransaction()
                        da.SelectCommand.Transaction = thisTransaction
                        da.Update(ds, "LOOSE")

                        Dim ItemId As Integer = 0
                        'count LOOSE_ISSUES_COPIES
                        Dim ReceivedCount As Integer = Nothing
                        Dim str As Object = Nothing
                        str = "SELECT COUNT (*) FROM LOOSE_ISSUES_COPIES WHERE (ISS_ID ='" & Trim(ISS_ID) & "')  AND (LIB_CODE ='" & Trim(LibCode) & "') "
                        Dim cmd As New SqlCommand(str, SqlConn)
                        cmd.Transaction = thisTransaction
                        ReceivedCount = cmd.ExecuteScalar

                        Dim CopiesToBeAdded As Integer = Nothing
                        CopiesToBeAdded = TOTAL_COPY - ReceivedCount
                        Dim cpyid As New ArrayList
                        If CopiesToBeAdded > 0 Then
                            'insert copies in loose_issue_copies
                            For i = 1 To CopiesToBeAdded
                                If ISS_ID <> 0 Then
                                    Dim objCommand4 As New SqlCommand
                                    objCommand4.Connection = SqlConn
                                    objCommand4.Transaction = thisTransaction
                                    objCommand4.CommandType = CommandType.Text
                                    objCommand4.CommandText = "INSERT INTO LOOSE_ISSUES_COPIES (ISS_ID, STA_CODE, COLLECTION_TYPE, REMARKS, DATE_ADDED, USER_CODE, LIB_CODE, IP) " & _
                                                              " VALUES (@ISS_ID, @STA_CODE, @COLLECTION_TYPE, @REMARKS, @DATE_ADDED, @USER_CODE, @LIB_CODE, @IP);  " & _
                                                              " SELECT SCOPE_IDENTITY();"

                                    objCommand4.Parameters.Add("@ISS_ID", SqlDbType.Int)
                                    objCommand4.Parameters("@ISS_ID").Value = ISS_ID

                                    objCommand4.Parameters.Add("@STA_CODE", SqlDbType.VarChar)
                                    objCommand4.Parameters("@STA_CODE").Value = "1"

                                    objCommand4.Parameters.Add("@COLLECTION_TYPE", SqlDbType.VarChar)
                                    objCommand4.Parameters("@COLLECTION_TYPE").Value = "C"


                                    objCommand4.Parameters.Add("@REMARKS", SqlDbType.NVarChar)
                                    If REMARKS = "" Then
                                        objCommand4.Parameters("@REMARKS").Value = System.DBNull.Value
                                    Else
                                        objCommand4.Parameters("@REMARKS").Value = REMARKS
                                    End If

                                    objCommand4.Parameters.Add("@DATE_ADDED", SqlDbType.DateTime)
                                    If DATE_ADDED = "" Then
                                        objCommand4.Parameters("@DATE_ADDED").Value = System.DBNull.Value
                                    Else
                                        objCommand4.Parameters("@DATE_ADDED").Value = DATE_ADDED
                                    End If

                                    objCommand4.Parameters.Add("@USER_CODE", SqlDbType.NVarChar)
                                    If USER_CODE = "" Then
                                        objCommand4.Parameters("@USER_CODE").Value = System.DBNull.Value
                                    Else
                                        objCommand4.Parameters("@USER_CODE").Value = USER_CODE
                                    End If

                                    objCommand4.Parameters.Add("@IP", SqlDbType.NVarChar)
                                    If IP = "" Then
                                        objCommand4.Parameters("@IP").Value = System.DBNull.Value
                                    Else
                                        objCommand4.Parameters("@IP").Value = IP
                                    End If

                                    objCommand4.Parameters.Add("@LIB_CODE", SqlDbType.NVarChar)
                                    objCommand4.Parameters("@LIB_CODE").Value = LibCode

                                    Dim dr As SqlDataReader
                                    dr = objCommand4.ExecuteReader()
                                    If dr.Read Then
                                        ItemId = dr.Item(0).ToString
                                        cpyid.Add(ItemId)
                                    End If
                                    dr.Close()

                                End If
                            Next
                        End If

                        Dim CopyToBeDeleted As Integer = Nothing
                        If TOTAL_COPY < ReceivedCount Then
                            CopyToBeDeleted = ReceivedCount - TOTAL_COPY
                        End If
                        If CopyToBeDeleted > 0 Then
                            'delete copies
                            Dim objCommand As New SqlCommand
                            objCommand.Connection = SqlConn
                            objCommand.Transaction = thisTransaction
                            objCommand.CommandType = CommandType.Text
                            objCommand.CommandText = "DELETE FROM LOOSE_ISSUES_COPIES WHERE (COPY_ID  IN (SELECT TOP " & CopyToBeDeleted & " COPY_ID FROM LOOSE_ISSUES_COPIES WHERE (ISS_ID =@ISS_ID) AND (LIB_CODE =@LIB_CODE) AND (STA_CODE<>'2'))) "

                            objCommand.Parameters.Add("@ISS_ID", SqlDbType.Int)
                            objCommand.Parameters("@ISS_ID").Value = ISS_ID

                            objCommand.Parameters.Add("@LIB_CODE", SqlDbType.NVarChar)
                            objCommand.Parameters("@LIB_CODE").Value = LibCode

                            objCommand.ExecuteNonQuery()

                        End If

                        thisTransaction.Commit()

                        Dim x As String = ""
                        Dim e1 As Integer = 0
                        If cpyid.Count <> 0 Then
                            For e1 = 0 To cpyid.Count - 1
                                If x = "" Then
                                    x = cpyid.Item(e1)
                                Else
                                    x = x + " ; " + Convert.ToString(cpyid.Item(e1))
                                End If
                            Next
                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "alert", "alert('Loose Issues Received Successfully and Item ID is/are: " & x & "; " & " " & "');", True)
                        End If

                        Label6.Text = ""
                        SqlConn.Close()
                        Loose_Cancel_Bttn_Click(sender, e)
                    Else
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('There is Error!');", True)
                        Exit Sub
                    End If
                    End If
            Else
                'record not selected
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Record not Selected!');", True)
                Exit Sub
            End If
            PopulateLooseIssuesGrid()
        Catch q As SqlException
            thisTransaction.Rollback()
            Label14.Text = "Error: " & (q.Message())
        Catch s As Exception
            Label14.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
End Class