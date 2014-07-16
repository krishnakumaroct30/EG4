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

Public Class Schedules
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
                        PopulateFrequencies()
                        PopulateSubscriptionYears()
                    End If
                    Dim CreateSUserButton As Object 'System.Web.UI.WebControls.Button
                    CreateSUserButton = Master.FindControl("Accordion1").FindControl("SerPane").FindControl("Ser_Schedules_Bttn")
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
            SubscriptionDetails()
        Catch ex As Exception
            Label14.Text = "Error: " & (ex.Message())
        Finally
            SqlConn.Close()
            DDL_Titles.Focus()
        End Try
    End Sub
    'display purchasing record of selected year
    Public Sub SubscriptionDetails()
        Dim dt As New DataTable
        Try
            ClearFields()
            Dim myCatNo As Integer = Nothing
            Dim SUBS_YEAR As Integer = Nothing
            If DDL_Titles.Text <> "" Then
                myCatNo = DDL_Titles.SelectedValue

                If DDL_SubscriptionYears.Text <> "" Then
                    SUBS_YEAR = DDL_SubscriptionYears.SelectedValue

                    Dim SQL As String = Nothing
                    If myCatNo <> 0 And SUBS_YEAR <> 0 Then
                        SQL = "SELECT * FROM SUBSCRIPTIONS WHERE (LIB_CODE = '" & Trim(LibCode) & "') AND (CAT_NO ='" & Trim(DDL_Titles.SelectedValue) & "') AND (SUBS_YEAR = '" & Trim(DDL_SubscriptionYears.SelectedValue) & "') "
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
                        If dt.Rows(0).Item("SUBS_ID").ToString <> "" Then
                            Label34.Text = dt.Rows(0).Item("SUBS_ID").ToString
                        Else
                            Label34.Text = ""
                        End If

                        If dt.Rows(0).Item("S_DATE").ToString <> "" Then
                            txt_Subs_SDate.Text = Format(dt.Rows(0).Item("S_DATE"), "dd/MM/yyyy")
                        Else
                            txt_Subs_SDate.Text = ""
                        End If

                        If dt.Rows(0).Item("E_DATE").ToString <> "" Then
                            txt_Subs_EDate.Text = Format(dt.Rows(0).Item("E_DATE"), "dd/MM/yyyy")
                        Else
                            txt_Subs_EDate.Text = ""
                        End If

                        If dt.Rows(0).Item("F_ISSUE_DATE").ToString <> "" Then
                            txt_Subs_FisrtIssueDate.Text = Format(dt.Rows(0).Item("F_ISSUE_DATE"), "dd/MM/yyyy")
                        Else
                            txt_Subs_FisrtIssueDate.Text = ""
                        End If

                        If dt.Rows(0).Item("FREQ_CODE").ToString <> "" Then
                            DDL_Frequencies.SelectedValue = dt.Rows(0).Item("FREQ_CODE").ToString
                        Else
                            DDL_Frequencies.ClearSelection()
                        End If

                        If dt.Rows(0).Item("VOL_YEAR").ToString <> "" Then
                            txt_Subs_VolPerYear.Text = dt.Rows(0).Item("VOL_YEAR").ToString
                        Else
                            txt_Subs_VolPerYear.Text = ""
                        End If

                        If dt.Rows(0).Item("ISSUE_YEAR").ToString <> "" Then
                            txt_Subs_IssuesPerYear.Text = dt.Rows(0).Item("ISSUE_YEAR").ToString
                        Else
                            txt_Subs_IssuesPerYear.Text = ""
                        End If

                        If dt.Rows(0).Item("ISSUE_VOL").ToString <> "" Then
                            txt_Subs_IssuesPerVol.Text = dt.Rows(0).Item("ISSUE_VOL").ToString
                        Else
                            txt_Subs_IssuesPerVol.Text = ""
                        End If

                        If dt.Rows(0).Item("S_VOL").ToString <> "" Then
                            txt_Subs_StartVolNo.Text = dt.Rows(0).Item("S_VOL").ToString
                        Else
                            txt_Subs_StartVolNo.Text = ""
                        End If

                        If dt.Rows(0).Item("S_ISSUE").ToString <> "" Then
                            txt_Subs_StartIssueNo.Text = dt.Rows(0).Item("S_ISSUE").ToString
                        Else
                            txt_Subs_StartIssueNo.Text = ""
                        End If

                        If dt.Rows(0).Item("GRACE_DAYS").ToString <> "" Then
                            txt_Subs_GraceDays.Text = dt.Rows(0).Item("GRACE_DAYS").ToString
                        Else
                            txt_Subs_GraceDays.Text = ""
                        End If

                        If dt.Rows(0).Item("ISSUE_CONTINUE").ToString <> "" Then
                            DDL_Continue.SelectedValue = dt.Rows(0).Item("ISSUE_CONTINUE").ToString
                        Else
                            DDL_Continue.ClearSelection()
                        End If

                        If dt.Rows(0).Item("COPY").ToString <> "" Then
                            txt_Subs_Copy.Text = dt.Rows(0).Item("COPY").ToString
                        Else
                            txt_Subs_Copy.Text = ""
                        End If

                        If dt.Rows(0).Item("SUBS_NO").ToString <> "" Then
                            txt_Subs_SubsNo.Text = dt.Rows(0).Item("SUBS_NO").ToString
                        Else
                            txt_Subs_SubsNo.Text = ""
                        End If

                        If dt.Rows(0).Item("LOCATION").ToString <> "" Then
                            txt_Subs_Location.Text = dt.Rows(0).Item("LOCATION").ToString
                        Else
                            txt_Subs_Location.Text = ""
                        End If

                        If dt.Rows(0).Item("REMARKS").ToString <> "" Then
                            txt_Subs_Remarks.Text = dt.Rows(0).Item("REMARKS").ToString
                        Else
                            txt_Subs_Remarks.Text = ""
                        End If
                    Else
                        txt_Subs_SDate.Text = ""
                        txt_Subs_EDate.Text = ""
                        txt_Subs_FisrtIssueDate.Text = ""
                        DDL_Frequencies.ClearSelection()
                        txt_Subs_VolPerYear.Text = ""
                        txt_Subs_IssuesPerYear.Text = ""
                        txt_Subs_IssuesPerVol.Text = ""
                        txt_Subs_StartVolNo.Text = ""
                        txt_Subs_StartIssueNo.Text = ""
                        txt_Subs_GraceDays.Text = ""
                        DDL_Continue.ClearSelection()
                        txt_Subs_Copy.Text = ""
                        txt_Subs_SubsNo.Text = ""
                        txt_Subs_Location.Text = ""
                        txt_Subs_Remarks.Text = ""
                        Label34.Text = ""
                    End If
                Else
                    txt_Subs_SDate.Text = ""
                    txt_Subs_EDate.Text = ""
                    txt_Subs_FisrtIssueDate.Text = ""
                    DDL_Frequencies.ClearSelection()
                    txt_Subs_VolPerYear.Text = ""
                    txt_Subs_IssuesPerYear.Text = ""
                    txt_Subs_IssuesPerVol.Text = ""
                    txt_Subs_StartVolNo.Text = ""
                    txt_Subs_StartIssueNo.Text = ""
                    txt_Subs_GraceDays.Text = ""
                    DDL_Continue.ClearSelection()
                    txt_Subs_Copy.Text = ""
                    txt_Subs_SubsNo.Text = ""
                    txt_Subs_Location.Text = ""
                    txt_Subs_Remarks.Text = ""
                    Label34.Text = ""
                End If
            Else
                Subs_Schedule_Bttn.Visible = False
            End If
            PopulateLooseIssuesGrid()
        Catch ex As Exception
            Label14.Text = "Error: " & (ex.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    Public Sub PopulateFrequencies()
        Me.DDL_Frequencies.DataTextField = "FREQ_NAME"
        Me.DDL_Frequencies.DataValueField = "FREQ_CODE"
        Me.DDL_Frequencies.DataSource = GetFreqList()
        Me.DDL_Frequencies.DataBind()
        DDL_Frequencies.Items.Insert(0, "")
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
                        Subs_Schedule_Bttn.Visible = True
                        Sch_Delete_Bttn.Visible = False
                    Else
                        Grid1.Visible = True
                        RecordCount = dtSearch.Rows.Count
                        Grid1.DataSource = dtSearch
                        Grid1.DataBind()
                        Label35.Text = "Total Record(s): " & RecordCount
                        Subs_Schedule_Bttn.Visible = False
                        Sch_Delete_Bttn.Visible = True
                    End If
                    ViewState("dt") = dtSearch
                Else
                    Me.Grid1.DataSource = Nothing
                    Grid1.DataBind()
                    Label35.Text = "Total Record(s): 0 "
                    Sch_Delete_Bttn.Visible = False
                End If
            Else
                Me.Grid1.DataSource = Nothing
                Grid1.DataBind()
                Label35.Text = "Total Record(s): 0 "
                Sch_Delete_Bttn.Visible = False
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
        txt_Subs_SDate.Text = ""
        txt_Subs_EDate.Text = ""
        txt_Subs_FisrtIssueDate.Text = ""
        DDL_Frequencies.ClearSelection()
        txt_Subs_VolPerYear.Text = ""
        txt_Subs_IssuesPerYear.Text = ""
        txt_Subs_IssuesPerVol.Text = ""
        txt_Subs_StartVolNo.Text = ""
        txt_Subs_StartIssueNo.Text = ""
        txt_Subs_GraceDays.Text = ""
        DDL_Continue.ClearSelection()
        txt_Subs_Copy.Text = ""
        txt_Subs_SubsNo.Text = ""
        txt_Subs_Location.Text = ""
        txt_Subs_Remarks.Text = ""
        Label34.Text = ""

        Sch_Save_Bttn.Visible = True
        Sch_Update_Bttn.Visible = False
        Sch_DeleteIssue_Bttn.Visible = False
        Subs_Schedule_Bttn.Visible = False
    End Sub
    Public Sub ClearScheduleFields()
        txt_Sch_VolNo.Text = ""
        txt_Sch_IssueNo.Text = ""
        txt_Sch_PartNo.Text = ""
        txt_Sch_IssueDate.Text = ""
        txt_Sch_DueDate.Text = ""
        txt_Sch_Status.Text = ""
        txt_Sch_Remarks.Text = ""
        Label36.Text = ""
        txt_Sch_Status.Enabled = True
    End Sub
    'cancel button
    Protected Sub App_Cancel_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Sch_Cancel_Bttn.Click
        Sch_Save_Bttn.Visible = True
        Sch_Update_Bttn.Visible = False
        Sch_DeleteIssue_Bttn.Visible = False
        ClearScheduleFields()
    End Sub
    'generate schedule  
    Protected Sub Subs_Schedule_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Subs_Schedule_Bttn.Click

        If Label34.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Subscription Details of This Title not Exists, Plz complete Previous Form!');", True)
            Me.DDL_Titles.Focus()
            Exit Sub
        End If

        Dim myNewSubsYear As Object
        If Trim(DDL_SubscriptionYears.Text) = "" Then
            Label14.Text = "Please select Subscription Year from drop-down!"
            DDL_SubscriptionYears.Focus()
            Exit Sub
        Else
            myNewSubsYear = TrimX(DDL_SubscriptionYears.SelectedValue)
        End If

        Dim myNewCatNo As Long
        If Trim(DDL_Titles.Text) = "" Then
            Label14.Text = "Please select journal from drop-down!"
            DDL_Titles.Focus()
            Exit Sub
        Else
            myNewCatNo = TrimX(DDL_Titles.SelectedValue)
        End If

        'check whether loose issues alredy exist for this title / during this year
        'check duplicate isbn
        Dim str As Object = Nothing
        Dim flag As Object = Nothing
        str = "SELECT ISS_ID FROM LOOSE_ISSUES WHERE (LIB_CODE ='" & Trim(LibCode) & "' ) AND (CAT_NO = '" & Trim(myNewCatNo) & "') AND (SUBS_YEAR = '" & Trim(myNewSubsYear) & "'); "
        Dim cmd1 As New SqlCommand(str, SqlConn)
        SqlConn.Open()
        flag = cmd1.ExecuteScalar
        SqlConn.Close()
        If flag <> Nothing Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Loose Issues Schedule Already Exists!!');", True)
            Me.DDL_Titles.Focus()
            Exit Sub
        End If
        SqlConn.Close()
        CalculateLooseIssues()
    End Sub
    Public Sub CalculateLooseIssues()
        'calculate schedule
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing
        Try
            Dim myNewCatNo As Long = Nothing
            If Trim(DDL_Titles.Text) = "" Then
                Label14.Text = "Please select Title from drop-down!"
                DDL_Titles.Focus()
                Exit Sub
            Else
                myNewCatNo = TrimX(DDL_Titles.SelectedValue)
            End If

            Dim myNewSubsYear As Integer = Nothing
            If Trim(DDL_SubscriptionYears.Text) = "" Then
                Label14.Text = "Please select Subscription Year from drop-down!"
                DDL_SubscriptionYears.Focus()
                Exit Sub
            Else
                myNewSubsYear = TrimX(DDL_SubscriptionYears.SelectedValue)
            End If

            Dim mySchSubsId As Integer = Nothing
            If Trim(Label34.Text) = "" Then
                Label4.Text = "Please select journal from drop-down"
                DDL_Titles.Focus()
                Exit Sub
            Else
                mySchSubsId = TrimX(Label34.Text)
            End If

            Dim myFirstIssDate As Object = Nothing
            If Trim(txt_Subs_FisrtIssueDate.Text) = "" Then
                Label14.Text = "Plz Update Subscription Details"
                Exit Sub
            Else
                myFirstIssDate = TrimX(txt_Subs_FisrtIssueDate.Text)
                If Len(myFirstIssDate) <> 10 Then
                    Label14.Text = "First Issue Date is not Valid !"
                    Exit Sub
                End If
                myFirstIssDate = Convert.ToDateTime(myFirstIssDate, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") 'convert from indian to us
            End If

            Dim myStartDate As Object = Nothing
            If Trim(txt_Subs_SDate.Text) = "" Then
                Label4.Text = "please update subscription details"
                Exit Sub
            Else
                myStartDate = TrimX(txt_Subs_SDate.Text)
                If Len(myStartDate) <> 10 Then
                    Label14.Text = "Subscription Start Date is not Valid !"
                    Exit Sub
                End If
                myStartDate = Convert.ToDateTime(myStartDate, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") 'convert from indian to us
            End If

            Dim myEndDate As Object = Nothing
            If Trim(txt_Subs_EDate.Text) = "" Then
                Label4.Text = "please update subscription details"
                Exit Sub
            Else
                myEndDate = TrimX(txt_Subs_EDate.Text)
                If myEndDate.ToString.Length <> 10 Then
                    Label14.Text = "End Date is not Valid..."
                    Me.txt_Subs_SDate.Focus()
                    Exit Sub
                End If
                myEndDate = Convert.ToDateTime(myEndDate, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us
            End If

            Dim myVolPerYear As Object = Nothing
            If Trim(txt_Subs_VolPerYear.Text) <> "" Then
                myVolPerYear = Trim(txt_Subs_VolPerYear.Text)
            Else
                myVolPerYear = ""
            End If

            Dim myIssuePerYear As Object = Nothing
            If Trim(txt_Subs_IssuesPerYear.Text) = "" Then
                Label4.Text = "Plz update subscription details!"
                Exit Sub
            Else
                myIssuePerYear = TrimX(txt_Subs_IssuesPerYear.Text)
            End If

            Dim myIssuePerVol As Object = Nothing
            If Trim(txt_Subs_IssuesPerVol.Text) <> "" Then
                myIssuePerVol = Trim(txt_Subs_IssuesPerVol.Text)
            Else
                myIssuePerVol = ""
            End If

            Dim myStartVol As Object = Nothing
            If Trim(txt_Subs_StartVolNo.Text) <> "" Then
                myStartVol = Trim(txt_Subs_StartVolNo.Text)
            Else
                myStartVol = ""
            End If

            Dim myStartIssue As Object = Nothing
            If Trim(txt_Subs_StartIssueNo.Text) <> "" Then
                myStartIssue = Trim(txt_Subs_StartIssueNo.Text)
            Else
                myStartIssue = ""
            End If

            Dim myGraceDays As Integer = Nothing
            If Trim(txt_Subs_GraceDays.Text) = "" Then
                myGraceDays = Nothing
            Else
                myGraceDays = TrimX(txt_Subs_GraceDays.Text)
            End If

            Dim mySchCopyOrdered As Integer = Nothing
            If Trim(txt_Subs_Copy.Text) = "" Then
                mySchCopyOrdered = 1
            Else
                mySchCopyOrdered = TrimX(txt_Subs_Copy.Text)
            End If

            Dim myDateAdded As Object = Nothing
            myDateAdded = Now.Date
            myDateAdded = Convert.ToDateTime(myDateAdded, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

            Dim IP As Object = Nothing
            IP = Request.UserHostAddress.Trim

            Dim mySchUserCode As Object = Nothing
            mySchUserCode = TrimX(UserCode)

            Dim myCont As Object = Nothing
            If Trim(DDL_Continue.Text) <> "" Then
                myCont = TrimX(DDL_Continue.SelectedValue)
            Else
                myCont = "N"
            End If

            Dim myFreq As Object = Nothing
            If Trim(DDL_Frequencies.Text) = "" Then
                Label4.Text = "please update subscription details"
                Exit Sub
            Else
                myFreq = Trim(DDL_Frequencies.SelectedValue)
            End If

            Dim myTotalIssue As Integer = Nothing
            If Trim(myStartIssue) <> "" Then
                myTotalIssue = (myStartIssue - 1) + 366
            Else
                myTotalIssue = 366
            End If

            Dim myNextIssue As Integer = Nothing
            Dim a As Integer = Nothing
            If Trim(myStartIssue) <> "" Then
                myNextIssue = CInt(myStartIssue)
                a = 0
            Else
                myNextIssue = 1
                myStartIssue = 1
                a = 1
            End If

            Dim amit As Integer = Nothing
            amit = myStartIssue + 366

            Dim dates(10000) As Date
            Dim newdates(10000) As Date
            Dim I As Integer = Nothing
            Dim j As Integer = Nothing
            Dim myNextIssueDate As Date = Nothing
            Dim myNextVol As String = Nothing
            Dim ka As Integer = Nothing
            ka = 1

            myNextIssueDate = CDate(myFirstIssDate)

            If myIssuePerYear = "1" Then
                For I = myStartIssue To amit
                    dates(I) = DateAdd("yyyy", (1 * ka) - 1, myNextIssueDate)
                    ka = ka + 1
                Next I
            End If

            If myIssuePerYear = "2" Then
                For I = myStartIssue To amit
                    dates(I) = DateAdd("m", (6 * ka) - 6, myNextIssueDate)
                    ka = ka + 1
                Next I
            End If

            If myIssuePerYear = "3" Then
                For I = myStartIssue To amit
                    dates(I) = DateAdd("m", (4 * ka) - 4, myNextIssueDate)
                    ka = ka + 1
                Next I
            End If

            If myIssuePerYear = "4" Then
                For I = myStartIssue To amit
                    dates(I) = DateAdd("m", (3 * ka) - 3, myNextIssueDate)
                    ka = ka + 1
                Next I
            End If

            If myIssuePerYear = "5" Then
                For I = myStartIssue To amit
                    newdates(I) = DateAdd("m", (2 * ka) - 2, myNextIssueDate)
                    dates(I) = DateAdd("d", (15 * ka) - 15, newdates(I))
                    ka = ka + 1
                    'If ka > 5 Then
                    '    Exit For
                    'End If
                Next I
            End If

            If myIssuePerYear = "6" Then
                For I = myStartIssue To amit
                    dates(I) = DateAdd("m", (2 * ka) - 2, myNextIssueDate)
                    ka = ka + 1
                Next I
            End If

            If myIssuePerYear = "8" Then
                For I = myStartIssue To amit
                    newdates(I) = DateAdd("m", (1 * ka) - 1, myNextIssueDate)
                    dates(I) = DateAdd("d", (16 * ka) - 16, newdates(I))
                    ka = ka + 1
                    'If ka > 8 Then
                    '    Exit For
                    'End If
                Next I
            End If

            If myIssuePerYear = "9" Then
                For I = myStartIssue To amit
                    newdates(I) = DateAdd("m", (1 * ka) - 1, myNextIssueDate)
                    dates(I) = DateAdd("d", (11 * ka) - 11, newdates(I))
                    ka = ka + 1
                    'If ka > 9 Then
                    '    Exit For
                    'End If
                Next I
            End If

            If myIssuePerYear = "10" Then
                For I = myStartIssue To amit
                    newdates(I) = DateAdd("m", (1 * ka) - 1, myNextIssueDate)
                    dates(I) = DateAdd("d", (7 * ka) - 7, newdates(I))
                    ka = ka + 1
                    'If ka > 10 Then
                    '    Exit For
                    'End If
                Next I
            End If

            If myIssuePerYear = "12" Then
                For I = myStartIssue To amit
                    dates(I) = DateAdd("m", (1 * ka) - 1, myNextIssueDate)
                    ka = ka + 1
                Next I
            End If

            If myIssuePerYear = "24" Then
                For I = myStartIssue To amit
                    dates(I) = DateAdd("d", (15 * ka) - (15), myNextIssueDate)
                    ka = ka + 1
                    'If ka > 24 Then
                    '    Exit For
                    'End If
                Next I
            End If

            If myIssuePerYear = "26" Then
                For I = myStartIssue To amit
                    dates(I) = DateAdd("ww", (2 * ka) - 2, myNextIssueDate)
                    ka = ka + 1
                    'If ka > 26 Then
                    '    Exit For
                    'End If
                Next I
            End If

            If myIssuePerYear = "51" Then
                For I = myStartIssue To amit
                    dates(I) = DateAdd("ww", (1 * ka) - 1, myNextIssueDate)
                    ka = ka + 1
                    'If ka > 51 Then
                    '    Exit For
                    'End If
                Next I
            End If

            If myIssuePerYear = "52" Then
                For I = myStartIssue To amit
                    dates(I) = DateAdd("ww", (1 * ka) - 1, myNextIssueDate)
                    ka = ka + 1
                    'If ka > 52 Then
                    '    Exit For
                    'End If
                Next I
            End If

            If myIssuePerYear = "365" Then
                For I = myStartIssue To amit
                    dates(I) = DateAdd("d", (1 * ka) - 1, myNextIssueDate)
                    ka = ka + 1
                    'If ka > 365 Then
                    '    Exit For
                    'End If
                Next I
            End If

            Dim n As Integer = Nothing
            Dim m As Integer = Nothing
            Dim myNewNextIssue As Integer = Nothing
            If myStartVol <> "" Then
                myNextVol = CInt(myStartVol)
            Else
                myNextVol = 1
            End If
            j = 1
            n = 1
            m = 0

            Dim intValue As Integer = Nothing
            'display schedule
            For myNextIssue = Trim(myStartIssue) To myTotalIssue
                myNextIssueDate = dates(myNextIssue)
                myNewNextIssue = myNextIssue

                If myIssuePerVol <> "" Then
                    If j <= Trim(myIssuePerVol) Then
                        myNextVol = myNextVol
                    Else
                        myNextVol = myNextVol + 1
                        j = 1
                    End If
                End If

                If myCont = "N" Then
                    Do While myNextIssue <= myTotalIssue
                        myNextIssueDate = dates(myNextIssue)
                        myNewNextIssue = myStartIssue + m

                        If myIssuePerVol <> "" Then
                            If myNewNextIssue > CInt(myIssuePerVol) Then
                                myStartIssue = 1
                                myNewNextIssue = 1
                                m = 0
                            End If

                            If j <= Trim(myIssuePerVol) Then
                                myNextVol = myNextVol
                            Else
                                myNextVol = myNextVol + 1
                                j = 1
                            End If
                        End If

                        If (myNextIssueDate >= myStartDate) And (myNextIssueDate <= myEndDate) Then
                            If SqlConn.State = 0 Then
                                SqlConn.Open()
                            End If

                            thisTransaction = SqlConn.BeginTransaction()
                            Dim objCommand As New SqlCommand
                            objCommand.Connection = SqlConn
                            objCommand.Transaction = thisTransaction
                            objCommand.CommandType = CommandType.Text
                            objCommand.CommandText = "INSERT INTO LOOSE_ISSUES (CAT_NO, SUBS_YEAR, VOL_NO, ISSUE_NO, ISS_DATE, RECEIVED, DUE_DATE, COPY_ORDERED, DATE_ADDED, USER_CODE, LIB_CODE,IP, SUBS_ID) " & _
                                             " VALUES (@CAT_NO, @SUBS_YEAR, @VOL_NO, @ISSUE_NO, @ISS_DATE, @RECEIVED, @DUE_DATE, @COPY_ORDERED, @DATE_ADDED, @USER_CODE, @LIB_CODE,@IP, @SUBS_ID);  "

                            objCommand.Parameters.Add("@CAT_NO", SqlDbType.Int)
                            objCommand.Parameters("@CAT_NO").Value = myNewCatNo

                            objCommand.Parameters.Add("@SUBS_YEAR", SqlDbType.Int)
                            objCommand.Parameters("@SUBS_YEAR").Value = myNewSubsYear


                            objCommand.Parameters.Add("@VOL_NO", SqlDbType.NVarChar)
                            If txt_Subs_StartVolNo.Text <> "" Then
                                If myNextVol = "" Then
                                    objCommand.Parameters("@VOL_NO").Value = System.DBNull.Value
                                Else
                                    objCommand.Parameters("@VOL_NO").Value = myNextVol
                                End If
                            Else
                                objCommand.Parameters("@VOL_NO").Value = System.DBNull.Value
                            End If


                            objCommand.Parameters.Add("@ISSUE_NO", SqlDbType.NVarChar)
                            If txt_Subs_StartIssueNo.Text <> "" Then
                                If myNextIssue = 0 Then
                                    objCommand.Parameters("@ISSUE_NO").Value = System.DBNull.Value
                                Else
                                    objCommand.Parameters("@ISSUE_NO").Value = myNewNextIssue
                                End If
                            Else
                                objCommand.Parameters("@ISSUE_NO").Value = System.DBNull.Value
                            End If

                            objCommand.Parameters.Add("@ISS_DATE", SqlDbType.DateTime)
                            If myNextIssueDate = Nothing Then
                                objCommand.Parameters("@ISS_DATE").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@ISS_DATE").Value = myNextIssueDate
                            End If

                            objCommand.Parameters.Add("@RECEIVED", SqlDbType.VarChar)
                            objCommand.Parameters("@RECEIVED").Value = "N"

                            'calcualte DUE DATE
                            Dim DUE_DATE As Date = Nothing
                            If myGraceDays <> 0 Then
                                DUE_DATE = myNextIssueDate.AddDays(myGraceDays)
                            Else
                                DUE_DATE = myNextIssueDate
                            End If

                            objCommand.Parameters.Add("@DUE_DATE", SqlDbType.DateTime)
                            If myNextIssueDate = Nothing Then
                                objCommand.Parameters("@DUE_DATE").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@DUE_DATE").Value = DUE_DATE
                            End If

                            objCommand.Parameters.Add("@COPY_ORDERED", SqlDbType.Int)
                            If mySchCopyOrdered = 0 Then
                                objCommand.Parameters("@COPY_ORDERED").Value = System.DBNull.Value
                            Else
                                objCommand.Parameters("@COPY_ORDERED").Value = mySchCopyOrdered
                            End If

                            If LibCode = "" Then LibCode = System.DBNull.Value
                            objCommand.Parameters.Add("@LIB_CODE", SqlDbType.NVarChar)
                            objCommand.Parameters("@LIB_CODE").Value = LibCode

                            objCommand.Parameters.Add("@DATE_ADDED", SqlDbType.DateTime)
                            objCommand.Parameters("@DATE_ADDED").Value = myDateAdded

                            objCommand.Parameters.Add("@USER_CODE", SqlDbType.NVarChar)
                            objCommand.Parameters("@USER_CODE").Value = UserCode

                            If IP = "" Then IP = System.DBNull.Value
                            objCommand.Parameters.Add("@IP", SqlDbType.VarChar)
                            objCommand.Parameters("@IP").Value = IP


                            objCommand.Parameters.Add("@SUBS_ID", SqlDbType.Int)
                            objCommand.Parameters("@SUBS_ID").Value = mySchSubsId

                            objCommand.ExecuteNonQuery()

                            thisTransaction.Commit()
                            SqlConn.Close()
                        End If
                            j = j + 1
                            m = m + 1
                            n = n + 1
                            myNextIssue = myNextIssue + 1
                    Loop
                    PopulateLooseIssuesGrid()
                    Exit Sub
                End If

                If (myNextIssueDate >= myStartDate) And (myNextIssueDate <= myEndDate) Then
                      If SqlConn.State = 0 Then
                        SqlConn.Open()
                    End If

                    thisTransaction = SqlConn.BeginTransaction()
                    Dim objCommand As New SqlCommand
                    objCommand.Connection = SqlConn
                    objCommand.Transaction = thisTransaction
                    objCommand.CommandType = CommandType.Text
                    objCommand.CommandText = "INSERT INTO LOOSE_ISSUES (CAT_NO, SUBS_YEAR, VOL_NO, ISSUE_NO, ISS_DATE, RECEIVED, DUE_DATE, COPY_ORDERED, DATE_ADDED, USER_CODE, LIB_CODE,IP, SUBS_ID) " & _
                                     " VALUES (@CAT_NO, @SUBS_YEAR, @VOL_NO, @ISSUE_NO, @ISS_DATE, @RECEIVED, @DUE_DATE, @COPY_ORDERED, @DATE_ADDED, @USER_CODE, @LIB_CODE,@IP, @SUBS_ID);  "

                    objCommand.Parameters.Add("@CAT_NO", SqlDbType.Int)
                    objCommand.Parameters("@CAT_NO").Value = myNewCatNo

                    objCommand.Parameters.Add("@SUBS_YEAR", SqlDbType.Int)
                    objCommand.Parameters("@SUBS_YEAR").Value = myNewSubsYear

                    objCommand.Parameters.Add("@VOL_NO", SqlDbType.NVarChar)
                    If myNextVol = "" Then
                        objCommand.Parameters("@VOL_NO").Value = System.DBNull.Value
                    Else
                        objCommand.Parameters("@VOL_NO").Value = myNextVol
                    End If

                    objCommand.Parameters.Add("@ISSUE_NO", SqlDbType.NVarChar)
                    If myNextVol = 0 Then
                        objCommand.Parameters("@ISSUE_NO").Value = System.DBNull.Value
                    Else
                        objCommand.Parameters("@ISSUE_NO").Value = myNewNextIssue
                    End If

                    objCommand.Parameters.Add("@ISS_DATE", SqlDbType.DateTime)
                    If myNextIssueDate = Nothing Then
                        objCommand.Parameters("@ISS_DATE").Value = System.DBNull.Value
                    Else
                        objCommand.Parameters("@ISS_DATE").Value = myNextIssueDate
                    End If

                    objCommand.Parameters.Add("@RECEIVED", SqlDbType.VarChar)
                    objCommand.Parameters("@RECEIVED").Value = "N"

                    'calcualte DUE DATE
                    Dim DUE_DATE As Date = Nothing
                    If myGraceDays <> 0 Then
                        DUE_DATE = myNextIssueDate.AddDays(myGraceDays)
                    Else
                        DUE_DATE = myNextIssueDate
                    End If

                    objCommand.Parameters.Add("@DUE_DATE", SqlDbType.DateTime)
                    If myNextIssueDate = Nothing Then
                        objCommand.Parameters("@DUE_DATE").Value = System.DBNull.Value
                    Else
                        objCommand.Parameters("@DUE_DATE").Value = DUE_DATE
                    End If

                    objCommand.Parameters.Add("@COPY_ORDERED", SqlDbType.Int)
                    If mySchCopyOrdered = 0 Then
                        objCommand.Parameters("@COPY_ORDERED").Value = System.DBNull.Value
                    Else
                        objCommand.Parameters("@COPY_ORDERED").Value = mySchCopyOrdered
                    End If

                    If LibCode = "" Then LibCode = System.DBNull.Value
                    objCommand.Parameters.Add("@LIB_CODE", SqlDbType.NVarChar)
                    objCommand.Parameters("@LIB_CODE").Value = LibCode

                    objCommand.Parameters.Add("@DATE_ADDED", SqlDbType.DateTime)
                    objCommand.Parameters("@DATE_ADDED").Value = myDateAdded

                    objCommand.Parameters.Add("@USER_CODE", SqlDbType.NVarChar)
                    objCommand.Parameters("@USER_CODE").Value = UserCode

                    If IP = "" Then IP = System.DBNull.Value
                    objCommand.Parameters.Add("@IP", SqlDbType.VarChar)
                    objCommand.Parameters("@IP").Value = IP


                    objCommand.Parameters.Add("@SUBS_ID", SqlDbType.Int)
                    objCommand.Parameters("@SUBS_ID").Value = mySchSubsId

                    objCommand.ExecuteNonQuery()

                    thisTransaction.Commit()
                    SqlConn.Close()
                End If
                j = j + 1
            Next myNextIssue
            PopulateLooseIssuesGrid()
        Catch q As SqlException
            thisTransaction.Rollback()
            Label14.Text = "Database Error -SAVE: " & (q.Message() & ", " & q.Number & ", " & q.GetType.ToString)
        Catch ex As Exception
            Label14.Text = "Error-SAVE: " & (ex.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'delete records   
    Protected Sub Sch_Delete_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Sch_Delete_Bttn.Click
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing
        Try
            For Each row As GridViewRow In Grid1.Rows
                Dim cb As CheckBox = DirectCast(row.FindControl("cbd"), CheckBox)
                If cb IsNot Nothing AndAlso cb.Checked = True Then
                    Dim ISS_ID As Integer = Convert.ToInt32(Grid1.DataKeys(row.RowIndex).Value)

                    If SqlConn.State = 0 Then
                        SqlConn.Open()
                    End If

                    thisTransaction = SqlConn.BeginTransaction()
                    Dim objCommand As New SqlCommand
                    objCommand.Connection = SqlConn
                    objCommand.Transaction = thisTransaction
                    objCommand.CommandType = CommandType.Text
                    objCommand.CommandText = "DELETE FROM LOOSE_ISSUES WHERE (ISS_ID =@ISS_ID) AND (LIB_CODE =@LIB_CODE) AND (RECEIVED ='N') "

                    objCommand.Parameters.Add("@ISS_ID", SqlDbType.Int)
                    objCommand.Parameters("@ISS_ID").Value = ISS_ID

                    objCommand.Parameters.Add("@LIB_CODE", SqlDbType.NVarChar)
                    objCommand.Parameters("@LIB_CODE").Value = LibCode

                    objCommand.ExecuteNonQuery()
                    thisTransaction.Commit()
                    SqlConn.Close()
                End If
            Next
            PopulateLooseIssuesGrid()
        Catch s As SqlException
            thisTransaction.Rollback()
            Label14.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'get value of row from grid
    Private Sub Grid1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles Grid1.RowCommand
        Dim Command As SqlCommand = Nothing
        Dim dr As SqlDataReader = Nothing
        Try
            txt_Sch_VolNo.Text = ""
            txt_Sch_IssueNo.Text = ""
            txt_Sch_PartNo.Text = ""
            txt_Sch_IssueDate.Text = ""
            txt_Sch_DueDate.Text = ""
            txt_Sch_Status.Text = ""
            txt_Sch_Remarks.Text = ""
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
                            txt_Sch_VolNo.Text = dr.Item("VOL_NO").ToString
                        Else
                            txt_Sch_VolNo.Text = ""
                        End If
                        If dr.Item("ISSUE_NO").ToString <> "" Then
                            txt_Sch_IssueNo.Text = dr.Item("ISSUE_NO").ToString
                        Else
                            txt_Sch_IssueNo.Text = ""
                        End If
                        If dr.Item("PART_NO").ToString <> "" Then
                            txt_Sch_PartNo.Text = dr.Item("PART_NO").ToString
                        Else
                            txt_Sch_PartNo.Text = ""
                        End If
                        If dr.Item("ISS_DATE").ToString <> "" Then
                            txt_Sch_IssueDate.Text = Format(dr.Item("ISS_DATE"), "dd/MM/yyyy")
                        Else
                            txt_Sch_IssueDate.Text = ""
                        End If

                        If dr.Item("DUE_DATE").ToString <> "" Then
                            txt_Sch_DueDate.Text = Format(dr.Item("DUE_DATE"), "dd/MM/yyyy")
                        Else
                            txt_Sch_DueDate.Text = ""
                        End If

                        txt_Sch_Status.Enabled = True
                        If dr.Item("RECEIVED").ToString <> "" Then
                            txt_Sch_Status.Text = dr.Item("RECEIVED").ToString
                            If txt_Sch_Status.Text = "Y" Then
                                txt_Sch_Status.Enabled = False
                            Else
                                txt_Sch_Status.Enabled = True
                            End If
                        Else
                            txt_Sch_Status.Text = ""
                        End If

                        If dr.Item("REMARKS").ToString <> "" Then
                            txt_Sch_Remarks.Text = dr.Item("REMARKS").ToString
                        Else
                            txt_Sch_Remarks.Text = ""
                        End If
                        dr.Close()
                        SqlConn.Close()

                        Sch_Update_Bttn.Visible = True
                        Sch_DeleteIssue_Bttn.Visible = True
                        Sch_Save_Bttn.Visible = False
                    Else
                        Label36.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' No Record to Edit... ');", True)

                        Sch_Update_Bttn.Visible = False
                        Sch_DeleteIssue_Bttn.Visible = False
                        Sch_Save_Bttn.Visible = True
                    End If
                Else
                    Label36.Text = ""
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' No Record to Edit... ');", True)

                    Sch_Update_Bttn.Visible = False
                    Sch_DeleteIssue_Bttn.Visible = False
                    Sch_Save_Bttn.Visible = True
                End If
            End If
        Catch s As Exception
            Label14.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub 'Grid1_ItemCommand
    'save new issue
    Protected Sub Sch_Save_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Sch_Save_Bttn.Click
        If IsPostBack = True Then
            'check unwanted characters
            Dim iloop As Integer
            Dim strcurrentchar As Object
            Dim c As Integer
            Dim counter1, counter2, counter3, counter4, counter5, counter6, counter7, counter8, counter9 As Integer
            Dim thisTransaction As SqlClient.SqlTransaction = Nothing
            Try
                'validation for CAT_NO
                Dim CAT_NO As Integer = Nothing
                If Me.Label19.Text <> "" Then
                    CAT_NO = TrimX(Label19.Text)
                    CAT_NO = RemoveQuotes(CAT_NO)
                    If CAT_NO.ToString.Length > 10 Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Cat No length is not proper!');", True)
                        DDL_Titles.Focus()
                        Exit Sub
                    End If
                    CAT_NO = " " & CAT_NO & " "
                    If InStr(1, CAT_NO, "CREATE", 1) > 0 Or InStr(1, CAT_NO, "DELETE", 1) > 0 Or InStr(1, CAT_NO, "DROP", 1) > 0 Or InStr(1, CAT_NO, "INSERT", 1) > 1 Or InStr(1, CAT_NO, "TRACK", 1) > 1 Or InStr(1, CAT_NO, "TRACE", 1) > 1 Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Do not use Reserve Keywords!');", True)
                        DDL_Titles.Focus()
                        Exit Sub
                    End If
                    CAT_NO = TrimAll(CAT_NO)
                Else
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Plz Select Title from drop-down!');", True)
                    DDL_Titles.Focus()
                    Exit Sub
                End If

                'Server validation for  : subs year
                Dim SUBS_YEAR As Integer = Nothing
                If DDL_SubscriptionYears.Text <> "" Then
                    SUBS_YEAR = Trim(DDL_SubscriptionYears.SelectedValue)
                    SUBS_YEAR = RemoveQuotes(SUBS_YEAR)
                    If SUBS_YEAR.ToString.Length <> 4 Then 'maximum length
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Input Length is not proper!');", True)
                        DDL_SubscriptionYears.Focus()
                        Exit Sub
                    End If

                    SUBS_YEAR = " " & SUBS_YEAR & " "
                    If InStr(1, SUBS_YEAR, "CREATE", 1) > 0 Or InStr(1, SUBS_YEAR, "DELETE", 1) > 0 Or InStr(1, SUBS_YEAR, "DROP", 1) > 0 Or InStr(1, SUBS_YEAR, "INSERT", 1) > 1 Or InStr(1, SUBS_YEAR, "TRACK", 1) > 1 Or InStr(1, SUBS_YEAR, "TRACE", 1) > 1 Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Do not use Reserve Words!');", True)
                        DDL_SubscriptionYears.Focus()
                        Exit Sub
                    End If
                    SUBS_YEAR = TrimX(SUBS_YEAR)
                Else
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Subscription Year is Required!');", True)
                    DDL_SubscriptionYears.Focus()
                    Exit Sub
                End If

                'Server validation for  : VOL NO
                Dim VOL_NO As Object = Nothing
                If txt_Sch_VolNo.Text <> "" Then
                    VOL_NO = TrimAll(txt_Sch_VolNo.Text)
                    VOL_NO = RemoveQuotes(VOL_NO)
                    If VOL_NO.Length > 50 Then 'maximum length
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Input Length is not proper!');", True)
                        txt_Sch_VolNo.Focus()
                        Exit Sub
                    End If

                    VOL_NO = " " & VOL_NO & " "
                    If InStr(1, VOL_NO, "CREATE", 1) > 0 Or InStr(1, VOL_NO, "DELETE", 1) > 0 Or InStr(1, VOL_NO, "DROP", 1) > 0 Or InStr(1, VOL_NO, "INSERT", 1) > 1 Or InStr(1, VOL_NO, "TRACK", 1) > 1 Or InStr(1, VOL_NO, "TRACE", 1) > 1 Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Do not use Reserve Words!');", True)
                        txt_Sch_VolNo.Focus()
                        Exit Sub
                    End If
                    VOL_NO = TrimAll(VOL_NO)
                Else
                    VOL_NO = Nothing
                End If

                'Server validation for  : ISSUE NO
                Dim ISSUE_NO As Object = Nothing
                If txt_Sch_IssueNo.Text <> "" Then
                    ISSUE_NO = TrimAll(txt_Sch_IssueNo.Text)
                    ISSUE_NO = RemoveQuotes(ISSUE_NO)
                    If ISSUE_NO.Length > 50 Then 'maximum length
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Input Length is not proper!');", True)
                        txt_Sch_IssueNo.Focus()
                        Exit Sub
                    End If

                    ISSUE_NO = " " & ISSUE_NO & " "
                    If InStr(1, ISSUE_NO, "CREATE", 1) > 0 Or InStr(1, ISSUE_NO, "DELETE", 1) > 0 Or InStr(1, ISSUE_NO, "DROP", 1) > 0 Or InStr(1, ISSUE_NO, "INSERT", 1) > 1 Or InStr(1, ISSUE_NO, "TRACK", 1) > 1 Or InStr(1, ISSUE_NO, "TRACE", 1) > 1 Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Do not use Reserve Words!');", True)
                        txt_Sch_IssueNo.Focus()
                        Exit Sub
                    End If
                    ISSUE_NO = TrimAll(ISSUE_NO)
                Else
                    ISSUE_NO = Nothing
                End If

                'Server validation for  : PART NO
                Dim PART_NO As Object = Nothing
                If txt_Sch_PartNo.Text <> "" Then
                    PART_NO = TrimAll(txt_Sch_PartNo.Text)
                    PART_NO = RemoveQuotes(PART_NO)
                    If PART_NO.Length > 50 Then 'maximum length
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Input Length is not proper!');", True)
                        txt_Sch_PartNo.Focus()
                        Exit Sub
                    End If

                    PART_NO = " " & PART_NO & " "
                    If InStr(1, PART_NO, "CREATE", 1) > 0 Or InStr(1, PART_NO, "DELETE", 1) > 0 Or InStr(1, PART_NO, "DROP", 1) > 0 Or InStr(1, PART_NO, "INSERT", 1) > 1 Or InStr(1, PART_NO, "TRACK", 1) > 1 Or InStr(1, PART_NO, "TRACE", 1) > 1 Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Do not use Reserve Words!');", True)
                        txt_Sch_PartNo.Focus()
                        Exit Sub
                    End If
                    PART_NO = TrimAll(PART_NO)
                Else
                    PART_NO = Nothing
                End If

                'search issue date
                Dim ISS_DATE As Object = Nothing
                If txt_Sch_IssueDate.Text <> "" Then
                    ISS_DATE = TrimX(txt_Sch_IssueDate.Text)

                    If Len(ISS_DATE.ToString) <> 10 Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Input Length is not proper!');", True)
                        Me.txt_Sch_IssueDate.Focus()
                        Exit Sub
                    End If
                    ISS_DATE = " " & ISS_DATE & " "
                    If InStr(1, ISS_DATE, "CREATE", 1) > 0 Or InStr(1, ISS_DATE, "DELETE", 1) > 0 Or InStr(1, ISS_DATE, "DROP", 1) > 0 Or InStr(1, ISS_DATE, "INSERT", 1) > 1 Or InStr(1, ISS_DATE, "TRACK", 1) > 1 Or InStr(1, ISS_DATE, "TRACE", 1) > 1 Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Do not use Reserve Words!');", True)
                        Me.txt_Sch_IssueDate.Focus()
                        Exit Sub
                    End If
                    ISS_DATE = TrimX(ISS_DATE)
                    'check unwanted characters
                    c = 0
                    counter1 = 0
                    For iloop = 1 To Len(ISS_DATE)
                        strcurrentchar = Mid(ISS_DATE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter1 = 1
                            End If
                        End If
                    Next
                    If counter1 = 1 Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Do not use un-wanted Words!');", True)
                        Me.txt_Sch_IssueDate.Focus()
                        Exit Sub
                    End If
                    ISS_DATE = Convert.ToDateTime(ISS_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us
                Else
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Issue Date is Required!');", True)
                    Me.txt_Sch_IssueDate.Focus()
                    Exit Sub
                End If

                'search DUE_DATE date
                Dim DUE_DATE As Object = Nothing
                If txt_Sch_DueDate.Text <> "" Then
                    DUE_DATE = TrimX(txt_Sch_DueDate.Text)

                    If Len(DUE_DATE.ToString) <> 10 Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Input Length is not proper!');", True)
                        Me.txt_Sch_DueDate.Focus()
                        Exit Sub
                    End If
                    DUE_DATE = " " & DUE_DATE & " "
                    If InStr(1, DUE_DATE, "CREATE", 1) > 0 Or InStr(1, DUE_DATE, "DELETE", 1) > 0 Or InStr(1, DUE_DATE, "DROP", 1) > 0 Or InStr(1, DUE_DATE, "INSERT", 1) > 1 Or InStr(1, DUE_DATE, "TRACK", 1) > 1 Or InStr(1, DUE_DATE, "TRACE", 1) > 1 Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Do not use Reserve Words!');", True)
                        Me.txt_Sch_DueDate.Focus()
                        Exit Sub
                    End If
                    DUE_DATE = TrimX(DUE_DATE)
                    'check unwanted characters
                    c = 0
                    counter2 = 0
                    For iloop = 1 To Len(DUE_DATE)
                        strcurrentchar = Mid(DUE_DATE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter2 = 1
                            End If
                        End If
                    Next
                    If counter2 = 1 Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Do not use un-wanted Words!');", True)
                        Me.txt_Sch_DueDate.Focus()
                        Exit Sub
                    End If
                    DUE_DATE = Convert.ToDateTime(DUE_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us
                Else
                    DUE_DATE = Nothing
                End If

                'search RECEIVED
                Dim RECEIVED As Object = Nothing
                If txt_Sch_Status.Text <> "" Then
                    RECEIVED = TrimX(txt_Sch_Status.Text)

                    If Len(RECEIVED.ToString) <> 1 Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Input Length is not proper!');", True)
                        Me.txt_Sch_Status.Focus()
                        Exit Sub
                    End If
                    'check unwanted characters
                    c = 0
                    counter3 = 0
                    For iloop = 1 To Len(RECEIVED)
                        strcurrentchar = Mid(RECEIVED, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>ABCDEFGHIJKLMOQRSTUVWZabcdefghijklmoqrstuvwz=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter3 = 1
                            End If
                        End If
                    Next
                    If counter3 = 1 Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Do not use un-wanted Words!');", True)
                        Me.txt_Sch_Status.Focus()
                        Exit Sub
                    End If
                Else
                    RECEIVED = "N"
                End If

                'Server validation for  : REMARKS
                Dim REMARKS As Object = Nothing
                If txt_Sch_Remarks.Text <> "" Then
                    REMARKS = TrimAll(txt_Sch_Remarks.Text)
                    REMARKS = RemoveQuotes(REMARKS)
                    If REMARKS.Length > 250 Then 'maximum length
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Input Length is not proper!');", True)
                        txt_Sch_Remarks.Focus()
                        Exit Sub
                    End If
                Else
                    REMARKS = Nothing
                End If

                Dim SUBS_ID As Integer = Nothing
                If Label34.Text <> "" Then
                    SUBS_ID = Label34.Text
                Else
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Subscription Year is must!');", True)
                    DDL_Titles.Focus()
                    Exit Sub
                End If

                Dim COPY_ORDERED As Integer = Nothing
                If txt_Subs_Copy.Text <> "" Then
                    COPY_ORDERED = txt_Subs_Copy.Text
                Else
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Copy Ordered is Required!');", True)
                    DDL_Titles.Focus()
                    Exit Sub
                End If

                Dim LIB_CODE As Object = Nothing
                If Session.Item("LoggedLibcode") <> "" Then
                    LIB_CODE = Session.Item("LoggedLibcode")
                Else
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Do not use Reserve Words!');", True)
                    Exit Sub
                End If

                Dim USER_CODE As Object = Nothing
                USER_CODE = Session.Item("LoggedUser")

                Dim DATE_ADDED As Object = Nothing
                DATE_ADDED = Now.Date
                DATE_ADDED = Convert.ToDateTime(DATE_ADDED, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                Dim IP As Object = Nothing
                IP = Request.UserHostAddress.Trim

                '********************************************************************************************
                If SqlConn.State = 0 Then
                    SqlConn.Open()
                End If
                thisTransaction = SqlConn.BeginTransaction()
                Dim intValue As Integer = 0
                Dim objCommand As New SqlCommand
                objCommand.Connection = SqlConn
                objCommand.Transaction = thisTransaction
                objCommand.CommandType = CommandType.Text
                objCommand.CommandText = "INSERT INTO LOOSE_ISSUES (CAT_NO, SUBS_YEAR, VOL_NO, ISSUE_NO, PART_NO, ISS_DATE, RECEIVED, DUE_DATE, COPY_ORDERED, REMARKS, SUBS_ID, DATE_ADDED, USER_CODE, LIB_CODE, IP) " & _
                                         " VALUES (@CAT_NO, @SUBS_YEAR, @VOL_NO, @ISSUE_NO, @PART_NO, @ISS_DATE, @RECEIVED, @DUE_DATE, @COPY_ORDERED, @REMARKS, @SUBS_ID, @DATE_ADDED, @USER_CODE, @LIB_CODE, @IP); "

                objCommand.Parameters.Add("@CAT_NO", SqlDbType.Int)
                objCommand.Parameters("@CAT_NO").Value = CAT_NO

                objCommand.Parameters.Add("@SUBS_YEAR", SqlDbType.Int)
                objCommand.Parameters("@SUBS_YEAR").Value = SUBS_YEAR

                objCommand.Parameters.Add("@VOL_NO", SqlDbType.NVarChar)
                If VOL_NO = "" Then
                    objCommand.Parameters("@VOL_NO").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@VOL_NO").Value = VOL_NO
                End If

                objCommand.Parameters.Add("@ISSUE_NO", SqlDbType.NVarChar)
                If ISSUE_NO = "" Then
                    objCommand.Parameters("@ISSUE_NO").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@ISSUE_NO").Value = ISSUE_NO
                End If

                objCommand.Parameters.Add("@PART_NO", SqlDbType.NVarChar)
                If PART_NO = "" Then
                    objCommand.Parameters("@PART_NO").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@PART_NO").Value = PART_NO
                End If

                objCommand.Parameters.Add("@ISS_DATE", SqlDbType.DateTime)
                objCommand.Parameters("@ISS_DATE").Value = ISS_DATE

                objCommand.Parameters.Add("@RECEIVED", SqlDbType.VarChar)
                objCommand.Parameters("@RECEIVED").Value = RECEIVED

                objCommand.Parameters.Add("@DUE_DATE", SqlDbType.DateTime)
                If DUE_DATE = "" Then
                    objCommand.Parameters("@DUE_DATE").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@DUE_DATE").Value = DUE_DATE
                End If

                objCommand.Parameters.Add("@COPY_ORDERED", SqlDbType.Int)
                If COPY_ORDERED = 0 Then
                    objCommand.Parameters("@COPY_ORDERED").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@COPY_ORDERED").Value = COPY_ORDERED
                End If

                objCommand.Parameters.Add("@SUBS_ID", SqlDbType.Int)
                If SUBS_ID = 0 Then
                    objCommand.Parameters("@SUBS_ID").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@SUBS_ID").Value = SUBS_ID
                End If

                objCommand.Parameters.Add("@REMARKS", SqlDbType.NVarChar)
                If REMARKS = "" Then
                    objCommand.Parameters("@REMARKS").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@REMARKS").Value = REMARKS
                End If

                objCommand.Parameters.Add("@DATE_ADDED", SqlDbType.DateTime)
                objCommand.Parameters("@DATE_ADDED").Value = DATE_ADDED

                If USER_CODE = "" Then USER_CODE = System.DBNull.Value
                objCommand.Parameters.Add("@USER_CODE", SqlDbType.NVarChar)
                objCommand.Parameters("@USER_CODE").Value = USER_CODE

                If LIB_CODE = "" Then LIB_CODE = System.DBNull.Value
                objCommand.Parameters.Add("@LIB_CODE", SqlDbType.NVarChar)
                objCommand.Parameters("@LIB_CODE").Value = LIB_CODE

                If IP = "" Then IP = System.DBNull.Value
                objCommand.Parameters.Add("@IP", SqlDbType.VarChar)
                objCommand.Parameters("@IP").Value = IP

                objCommand.ExecuteNonQuery()

                thisTransaction.Commit()
                SqlConn.Close()

                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Record Added Successfully!');", True)

                Me.Sch_Save_Bttn.Visible = True
                Sch_Update_Bttn.Visible = False
                Sch_DeleteIssue_Bttn.Visible = False
                PopulateLooseIssuesGrid()
                txt_Sch_VolNo.Focus()
            Catch q As SqlException
                thisTransaction.Rollback()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('There is an Error in database!');", True)
            Catch ex As Exception
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('There is an General Error in Application!');", True)
            Finally
                SqlConn.Close()
            End Try
        End If
    End Sub
    'update issue details
    Protected Sub Sch_Update_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Sch_Update_Bttn.Click
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
                Dim counter1, counter2, counter3, counter4, counter5, counter6, counter7, counter8, counter9 As Integer
                '****************************************************************************************************
                'validation for CAT_NO
                Dim ISS_ID As Integer = Nothing
                If Me.Label36.Text <> "" Then
                    ISS_ID = TrimX(Label36.Text)
                    ISS_ID = RemoveQuotes(ISS_ID)
                    If ISS_ID.ToString.Length > 10 Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Input length is not proper!');", True)
                        Exit Sub
                    End If

                    If Not IsNumeric(ISS_ID) Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Input is not proper!');", True)
                        Exit Sub
                    End If

                    ISS_ID = " " & ISS_ID & " "
                    If InStr(1, ISS_ID, "CREATE", 1) > 0 Or InStr(1, ISS_ID, "DELETE", 1) > 0 Or InStr(1, ISS_ID, "DROP", 1) > 0 Or InStr(1, ISS_ID, "INSERT", 1) > 1 Or InStr(1, ISS_ID, "TRACK", 1) > 1 Or InStr(1, ISS_ID, "TRACE", 1) > 1 Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Do not use Reserve Keywords!');", True)
                        DDL_Titles.Focus()
                        Exit Sub
                    End If
                    ISS_ID = TrimX(ISS_ID)
                Else
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Plz Select Record to Update!');", True)
                    DDL_Titles.Focus()
                    Exit Sub
                End If

                'Server validation for  : VOL NO
                Dim VOL_NO As Object = Nothing
                If txt_Sch_VolNo.Text <> "" Then
                    VOL_NO = TrimAll(txt_Sch_VolNo.Text)
                    VOL_NO = RemoveQuotes(VOL_NO)
                    If VOL_NO.Length > 50 Then 'maximum length
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Input Length is not proper!');", True)
                        txt_Sch_VolNo.Focus()
                        Exit Sub
                    End If

                    VOL_NO = " " & VOL_NO & " "
                    If InStr(1, VOL_NO, "CREATE", 1) > 0 Or InStr(1, VOL_NO, "DELETE", 1) > 0 Or InStr(1, VOL_NO, "DROP", 1) > 0 Or InStr(1, VOL_NO, "INSERT", 1) > 1 Or InStr(1, VOL_NO, "TRACK", 1) > 1 Or InStr(1, VOL_NO, "TRACE", 1) > 1 Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Do not use Reserve Words!');", True)
                        txt_Sch_VolNo.Focus()
                        Exit Sub
                    End If
                    VOL_NO = TrimAll(VOL_NO)
                Else
                    VOL_NO = Nothing
                End If

                'Server validation for  : ISSUE NO
                Dim ISSUE_NO As Object = Nothing
                If txt_Sch_IssueNo.Text <> "" Then
                    ISSUE_NO = TrimAll(txt_Sch_IssueNo.Text)
                    ISSUE_NO = RemoveQuotes(ISSUE_NO)
                    If ISSUE_NO.Length > 50 Then 'maximum length
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Input Length is not proper!');", True)
                        txt_Sch_IssueNo.Focus()
                        Exit Sub
                    End If

                    ISSUE_NO = " " & ISSUE_NO & " "
                    If InStr(1, ISSUE_NO, "CREATE", 1) > 0 Or InStr(1, ISSUE_NO, "DELETE", 1) > 0 Or InStr(1, ISSUE_NO, "DROP", 1) > 0 Or InStr(1, ISSUE_NO, "INSERT", 1) > 1 Or InStr(1, ISSUE_NO, "TRACK", 1) > 1 Or InStr(1, ISSUE_NO, "TRACE", 1) > 1 Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Do not use Reserve Words!');", True)
                        txt_Sch_IssueNo.Focus()
                        Exit Sub
                    End If
                    ISSUE_NO = TrimAll(ISSUE_NO)
                Else
                    ISSUE_NO = Nothing
                End If

                'Server validation for  : PART NO
                Dim PART_NO As Object = Nothing
                If txt_Sch_PartNo.Text <> "" Then
                    PART_NO = TrimAll(txt_Sch_PartNo.Text)
                    PART_NO = RemoveQuotes(PART_NO)
                    If PART_NO.Length > 50 Then 'maximum length
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Input Length is not proper!');", True)
                        txt_Sch_PartNo.Focus()
                        Exit Sub
                    End If

                    PART_NO = " " & PART_NO & " "
                    If InStr(1, PART_NO, "CREATE", 1) > 0 Or InStr(1, PART_NO, "DELETE", 1) > 0 Or InStr(1, PART_NO, "DROP", 1) > 0 Or InStr(1, PART_NO, "INSERT", 1) > 1 Or InStr(1, PART_NO, "TRACK", 1) > 1 Or InStr(1, PART_NO, "TRACE", 1) > 1 Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Do not use Reserve Words!');", True)
                        txt_Sch_PartNo.Focus()
                        Exit Sub
                    End If
                    PART_NO = TrimAll(PART_NO)
                Else
                    PART_NO = Nothing
                End If

                'search issue date
                Dim ISS_DATE As Object = Nothing
                If txt_Sch_IssueDate.Text <> "" Then
                    ISS_DATE = TrimX(txt_Sch_IssueDate.Text)

                    If Len(ISS_DATE.ToString) <> 10 Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Input Length is not proper!');", True)
                        Me.txt_Sch_IssueDate.Focus()
                        Exit Sub
                    End If
                    ISS_DATE = " " & ISS_DATE & " "
                    If InStr(1, ISS_DATE, "CREATE", 1) > 0 Or InStr(1, ISS_DATE, "DELETE", 1) > 0 Or InStr(1, ISS_DATE, "DROP", 1) > 0 Or InStr(1, ISS_DATE, "INSERT", 1) > 1 Or InStr(1, ISS_DATE, "TRACK", 1) > 1 Or InStr(1, ISS_DATE, "TRACE", 1) > 1 Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Do not use Reserve Words!');", True)
                        Me.txt_Sch_IssueDate.Focus()
                        Exit Sub
                    End If
                    ISS_DATE = TrimX(ISS_DATE)
                    'check unwanted characters
                    c = 0
                    counter1 = 0
                    For iloop = 1 To Len(ISS_DATE)
                        strcurrentchar = Mid(ISS_DATE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter1 = 1
                            End If
                        End If
                    Next
                    If counter1 = 1 Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Do not use un-wanted Words!');", True)
                        Me.txt_Sch_IssueDate.Focus()
                        Exit Sub
                    End If
                    ISS_DATE = Convert.ToDateTime(ISS_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us
                Else
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Issue Date is Required!');", True)
                    Me.txt_Sch_IssueDate.Focus()
                    Exit Sub
                End If

                'search DUE_DATE date
                Dim DUE_DATE As Object = Nothing
                If txt_Sch_DueDate.Text <> "" Then
                    DUE_DATE = TrimX(txt_Sch_DueDate.Text)

                    If Len(DUE_DATE.ToString) <> 10 Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Input Length is not proper!');", True)
                        Me.txt_Sch_DueDate.Focus()
                        Exit Sub
                    End If
                    DUE_DATE = " " & DUE_DATE & " "
                    If InStr(1, DUE_DATE, "CREATE", 1) > 0 Or InStr(1, DUE_DATE, "DELETE", 1) > 0 Or InStr(1, DUE_DATE, "DROP", 1) > 0 Or InStr(1, DUE_DATE, "INSERT", 1) > 1 Or InStr(1, DUE_DATE, "TRACK", 1) > 1 Or InStr(1, DUE_DATE, "TRACE", 1) > 1 Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Do not use Reserve Words!');", True)
                        Me.txt_Sch_DueDate.Focus()
                        Exit Sub
                    End If
                    DUE_DATE = TrimX(DUE_DATE)
                    'check unwanted characters
                    c = 0
                    counter2 = 0
                    For iloop = 1 To Len(DUE_DATE)
                        strcurrentchar = Mid(DUE_DATE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter2 = 1
                            End If
                        End If
                    Next
                    If counter2 = 1 Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Do not use un-wanted Words!');", True)
                        Me.txt_Sch_DueDate.Focus()
                        Exit Sub
                    End If
                    DUE_DATE = Convert.ToDateTime(DUE_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us
                Else
                    DUE_DATE = Nothing
                End If

                'search RECEIVED
                Dim RECEIVED As Object = Nothing
                If txt_Sch_Status.Text <> "" Then
                    RECEIVED = TrimX(txt_Sch_Status.Text)

                    If Len(RECEIVED.ToString) <> 1 Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Input Length is not proper!');", True)
                        Me.txt_Sch_Status.Focus()
                        Exit Sub
                    End If
                    'check unwanted characters
                    c = 0
                    counter3 = 0
                    For iloop = 1 To Len(RECEIVED)
                        strcurrentchar = Mid(RECEIVED, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>ABCDEFGHIJKLMOQRSTUVWZabcdefghijklmoqrstuvwz=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter3 = 1
                            End If
                        End If
                    Next
                    If counter3 = 1 Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Do not use un-wanted Words!');", True)
                        Me.txt_Sch_Status.Focus()
                        Exit Sub
                    End If
                Else
                    RECEIVED = "N"
                End If

                'Server validation for  : REMARKS
                Dim REMARKS As Object = Nothing
                If txt_Sch_Remarks.Text <> "" Then
                    REMARKS = TrimAll(txt_Sch_Remarks.Text)
                    REMARKS = RemoveQuotes(REMARKS)
                    If REMARKS.Length > 250 Then 'maximum length
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Input Length is not proper!');", True)
                        txt_Sch_Remarks.Focus()
                        Exit Sub
                    End If
                Else
                    REMARKS = Nothing
                End If

                Dim USER_CODE As Object = Nothing
                USER_CODE = Session.Item("LoggedUser")

                Dim DATE_MODIFIED As Object = Nothing
                DATE_MODIFIED = Now.Date
                DATE_MODIFIED = Convert.ToDateTime(DATE_MODIFIED, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                Dim IP As Object = Nothing
                IP = Request.UserHostAddress.Trim

                'UPDATE THE LIBRARY PROFILE  
                If Label23.Text <> "" Then
                    SQL = "SELECT * FROM LOOSE_ISSUES WHERE (ISS_ID='" & Trim(ISS_ID) & "')"
                    SqlConn.Open()
                    da = New SqlDataAdapter(SQL, SqlConn)
                    cb = New SqlCommandBuilder(da)
                    da.Fill(ds, "LOOSE_ISSUES")
                    If ds.Tables("LOOSE_ISSUES").Rows.Count <> 0 Then
                        If VOL_NO <> "" Then
                            ds.Tables("LOOSE_ISSUES").Rows(0)("VOL_NO") = VOL_NO
                        Else
                            ds.Tables("LOOSE_ISSUES").Rows(0)("VOL_NO") = System.DBNull.Value
                        End If

                        If ISSUE_NO <> "" Then
                            ds.Tables("LOOSE_ISSUES").Rows(0)("ISSUE_NO") = ISSUE_NO
                        Else
                            ds.Tables("LOOSE_ISSUES").Rows(0)("ISSUE_NO") = System.DBNull.Value
                        End If

                        If PART_NO <> "" Then
                            ds.Tables("LOOSE_ISSUES").Rows(0)("PART_NO") = PART_NO
                        Else
                            ds.Tables("LOOSE_ISSUES").Rows(0)("PART_NO") = System.DBNull.Value
                        End If

                        If ISS_DATE <> "" Then
                            ds.Tables("LOOSE_ISSUES").Rows(0)("ISS_DATE") = ISS_DATE
                        Else
                            ds.Tables("LOOSE_ISSUES").Rows(0)("ISS_DATE") = System.DBNull.Value
                        End If

                        If DUE_DATE <> "" Then
                            ds.Tables("LOOSE_ISSUES").Rows(0)("DUE_DATE") = DUE_DATE
                        Else
                            ds.Tables("LOOSE_ISSUES").Rows(0)("DUE_DATE") = System.DBNull.Value
                        End If

                        If RECEIVED <> "" Then
                            ds.Tables("LOOSE_ISSUES").Rows(0)("RECEIVED") = RECEIVED
                        Else
                            ds.Tables("LOOSE_ISSUES").Rows(0)("RECEIVED") = System.DBNull.Value
                        End If

                        If REMARKS <> "" Then
                            ds.Tables("LOOSE_ISSUES").Rows(0)("REMARKS") = REMARKS
                        Else
                            ds.Tables("LOOSE_ISSUES").Rows(0)("REMARKS") = System.DBNull.Value
                        End If

                        ds.Tables("LOOSE_ISSUES").Rows(0)("UPDATED_BY") = USER_CODE
                        ds.Tables("LOOSE_ISSUES").Rows(0)("DATE_MODIFIED") = DATE_MODIFIED
                        ds.Tables("LOOSE_ISSUES").Rows(0)("IP") = IP

                        thisTransaction = SqlConn.BeginTransaction()
                        da.SelectCommand.Transaction = thisTransaction
                        da.Update(ds, "LOOSE_ISSUES")
                        thisTransaction.Commit()
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Record Updated Successfully!');", True)
                        Sch_Save_Bttn.Visible = True
                        Sch_Update_Bttn.Visible = False
                        Sch_DeleteIssue_Bttn.Visible = False
                        ClearScheduleFields()
                    Else
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('No Record Selected For Updation!');", True)
                        Sch_Save_Bttn.Visible = True
                        Sch_Update_Bttn.Visible = False
                        Sch_DeleteIssue_Bttn.Visible = False
                        ClearScheduleFields()
                    End If
                End If
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('No Record Selected For Updation!');", True)
                Sch_Save_Bttn.Visible = True
                Sch_Update_Bttn.Visible = False
                Sch_DeleteIssue_Bttn.Visible = False
                ClearScheduleFields()
            End If
            SqlConn.Close()
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
    'delete selected record
    Protected Sub Sch_DeleteIssue_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Sch_DeleteIssue_Bttn.Click
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing
        Try
            Dim ISS_ID As Integer = Nothing
            If Me.Label36.Text <> "" Then
                ISS_ID = TrimX(Label36.Text)
                ISS_ID = RemoveQuotes(ISS_ID)
                If ISS_ID.ToString.Length > 10 Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Input length is not proper!');", True)
                    Exit Sub
                End If

                If Not IsNumeric(ISS_ID) Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Input is not proper!');", True)
                    Exit Sub
                End If

                ISS_ID = " " & ISS_ID & " "
                If InStr(1, ISS_ID, "CREATE", 1) > 0 Or InStr(1, ISS_ID, "DELETE", 1) > 0 Or InStr(1, ISS_ID, "DROP", 1) > 0 Or InStr(1, ISS_ID, "INSERT", 1) > 1 Or InStr(1, ISS_ID, "TRACK", 1) > 1 Or InStr(1, ISS_ID, "TRACE", 1) > 1 Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Do not use Reserve Keywords!');", True)
                    DDL_Titles.Focus()
                    Exit Sub
                End If
                ISS_ID = TrimX(ISS_ID)
                SqlConn.Open()
                thisTransaction = SqlConn.BeginTransaction()
                Dim objCommand As New SqlCommand
                objCommand.Connection = SqlConn
                objCommand.Transaction = thisTransaction
                objCommand.CommandType = CommandType.Text
                objCommand.CommandText = "DELETE FROM LOOSE_ISSUES WHERE (ISS_ID =@ISS_ID) AND (LIB_CODE =@LIB_CODE) AND (RECEIVED ='N') "

                objCommand.Parameters.Add("@ISS_ID", SqlDbType.Int)
                objCommand.Parameters("@ISS_ID").Value = ISS_ID

                objCommand.Parameters.Add("@LIB_CODE", SqlDbType.NVarChar)
                objCommand.Parameters("@LIB_CODE").Value = LibCode

                objCommand.ExecuteNonQuery()
                thisTransaction.Commit()
                SqlConn.Close()
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Plz Select Record to Update!');", True)
                DDL_Titles.Focus()
                Exit Sub
            End If

            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            ClearScheduleFields()
            PopulateLooseIssuesGrid()
        Catch s As SqlException
            thisTransaction.Rollback()
            Label14.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
End Class