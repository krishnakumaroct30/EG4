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

Public Class BoundJournals
    Inherits System.Web.UI.Page
    Dim index As Integer = 0
    Public Srno As Integer = 0
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session.Item("IsThisU") <> Nothing Then
                If Not SConnection() = True Then
                    Lbl_Error.Text = "Database Connection is lost..Try Again !'"
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Database Connection is Lost!');", True)
                Else
                    If Page.IsPostBack = False Then
                        PopulateTitles()
                        PopulateFormats()
                        PopulateStatus()
                        PopulateBindings()
                        PopulateSections()
                        DDL_Status.SelectedValue = "1"
                        DDL_Binding.SelectedValue = "H"
                        DDL_Format.SelectedValue = "P"
                        JHold_Cancel_Bttn.Visible = True
                        JHold_DeleteAll_Bttn.Visible = False
                        JHold_Save_Bttn.Visible = True
                        JHold_Update_Bttn.Visible = False
                        JHold_Delete_Bttn.Visible = False
                    End If
                    Dim CreateSUserButton As Object 'System.Web.UI.WebControls.Button
                    CreateSUserButton = Master.FindControl("Accordion1").FindControl("SerPane").FindControl("Ser_Bound_Bttn")
                    CreateSUserButton.ForeColor = Drawing.Color.Red
                    myPaneName = "SerPane"
                End If
            Else
                Response.Redirect("~/Default.aspx", False)
            End If
            LibCode = Session.Item("LoggedLibcode")
        Catch ex As Exception
            Lbl_Error.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    Public Sub PopulateTitles()
        Dim dtSearch As DataTable = Nothing
        Try
            Dim SQL As String = Nothing
            SQL = "SELECT CAT_NO, TITLE FROM CATS WHERE (BIB_CODE ='S') ORDER BY TITLE "

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
                Label37.Text = "Total Titles: 0 "
            Else
                RecordCount = dtSearch.Rows.Count
                Me.DDL_Titles.DataSource = dtSearch
                Me.DDL_Titles.DataTextField = "TITLE"
                Me.DDL_Titles.DataValueField = "CAT_NO"
                Me.DDL_Titles.DataBind()
                DDL_Titles.Items.Insert(0, "")
                Label37.Text = "Total Titles: " & RecordCount
            End If
        Catch s As Exception
            Lbl_Error.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
            DDL_Titles.Focus()
        End Try
    End Sub
    'load / display fields
    Protected Sub DDL_Titles_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDL_Titles.SelectedIndexChanged
        Dim dt As New DataTable
        Try
            ClearFields()

            txt_JHold_Search.Text = ""
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
                    Image4.ImageUrl = Nothing
                    Image4.Visible = True
                End If
            Else
                Label19.Text = ""
                Label16.Text = ""
                Label17.Text = ""
                Label18.Text = ""
                Image4.ImageUrl = Nothing
                Image4.Visible = True
            End If
            PopulateLooseIssuesGrid()
            txt_JHold_AccessionNo.Focus()
        Catch ex As Exception
            Lbl_Error.Text = "Error: " & (ex.Message())
        Finally
            SqlConn.Close()
            DDL_Titles.Focus()
        End Try
    End Sub
    'search by acc no
    Protected Sub txt_JHold_Search_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txt_JHold_Search.TextChanged
        Dim dt As New DataTable
        Try
            ClearFields()

            Dim ACCESSION_NO As Object = Nothing
            If txt_JHold_Search.Text <> "" Then
                ACCESSION_NO = TrimX(txt_JHold_Search.Text)

                Dim SQL As String = Nothing
                If ACCESSION_NO <> "" Then
                    SQL = "SELECT HOLDINGS.*, CATS_AUTHORS_VIEW.BIB_CODE, CATS_AUTHORS_VIEW.MAT_CODE, CATS_AUTHORS_VIEW.DOC_TYPE_CODE, CATS_AUTHORS_VIEW.STANDARD_NO, CATS_AUTHORS_VIEW.TITLE, CATS_AUTHORS_VIEW.SUB_TITLE, CATS_AUTHORS_VIEW.EDITOR, CATS_AUTHORS_VIEW.CORPORATE_AUTHOR, CATS_AUTHORS_VIEW.PUB_NAME, CATS_AUTHORS_VIEW.PLACE_OF_PUB, CATS_AUTHORS_VIEW.PHOTO FROM  HOLDINGS LEFT OUTER JOIN CATS_AUTHORS_VIEW ON HOLDINGS.CAT_NO = CATS_AUTHORS_VIEW.CAT_NO WHERE (HOLDINGS.ACCESSION_NO = '" & Trim(ACCESSION_NO) & "') AND (HOLDINGS.LIB_CODE = '" & Trim(LibCode) & "') AND (CATS_AUTHORS_VIEW.BIB_CODE ='S'); "
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
                    DDL_Titles.SelectedValue = dt.Rows(0).Item("CAT_NO").ToString
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

                    'photo
                    If dt.Rows(0).Item("PHOTO").ToString <> "" Then
                        Dim strURL As String = "~/Acquisition/Cats_GetPhoto.aspx?CAT_NO=" & dt.Rows(0).Item("CAT_NO").ToString & ""
                        Image4.ImageUrl = strURL
                        Image4.Visible = True
                    Else
                        Image4.ImageUrl = Nothing
                        Image4.Visible = True
                    End If



                    'Holdings 
                    If dt.Rows(0).Item("HOLD_ID").ToString <> "" Then
                        Label36.Text = dt.Rows(0).Item("HOLD_ID").ToString
                    Else
                        Label36.Text = ""
                    End If
                    If dt.Rows(0).Item("ACCESSION_NO").ToString <> "" Then
                        txt_JHold_AccessionNo.Text = dt.Rows(0).Item("ACCESSION_NO").ToString
                    Else
                        txt_JHold_AccessionNo.Text = ""
                    End If

                    If dt.Rows(0).Item("ACCESSION_DATE").ToString <> "" Then
                        txt_JHold_AccessionDate.Text = Format(dt.Rows(0).Item("ACCESSION_DATE"), "dd/MM/yyyy")
                    Else
                        txt_JHold_AccessionDate.Text = ""
                    End If

                    If dt.Rows(0).Item("VOL_NO").ToString <> "" Then
                        txt_JHold_VolNo.Text = dt.Rows(0).Item("VOL_NO").ToString
                    Else
                        txt_JHold_VolNo.Text = ""
                    End If

                    If dt.Rows(0).Item("ISSUE_NO").ToString <> "" Then
                        txt_JHold_IssueNo.Text = dt.Rows(0).Item("ISSUE_NO").ToString
                    Else
                        txt_JHold_IssueNo.Text = ""
                    End If

                    If dt.Rows(0).Item("JYEAR").ToString <> "" Then
                        txt_JHold_Year.Text = dt.Rows(0).Item("JYEAR").ToString
                    Else
                        txt_JHold_Year.Text = ""
                    End If

                    If dt.Rows(0).Item("VOL_TITLE").ToString <> "" Then
                        txt_JHold_VolTitle.Text = dt.Rows(0).Item("VOL_TITLE").ToString
                    Else
                        txt_JHold_VolTitle.Text = ""
                    End If


                    If dt.Rows(0).Item("VOL_EDITORS").ToString <> "" Then
                        txt_JHold_VolEditors.Text = dt.Rows(0).Item("VOL_EDITORS").ToString
                    Else
                        txt_JHold_VolEditors.Text = ""
                    End If

                    If dt.Rows(0).Item("CLASS_NO").ToString <> "" Then
                        txt_JHold_ClassNo.Text = dt.Rows(0).Item("CLASS_NO").ToString
                    Else
                        txt_JHold_ClassNo.Text = ""
                    End If

                    If dt.Rows(0).Item("BOOK_NO").ToString <> "" Then
                        txt_JHold_BookNo.Text = dt.Rows(0).Item("BOOK_NO").ToString
                    Else
                        txt_JHold_BookNo.Text = ""
                    End If

                    If dt.Rows(0).Item("PAGINATION").ToString <> "" Then
                        txt_JHold_Pagination.Text = dt.Rows(0).Item("PAGINATION").ToString
                    Else
                        txt_JHold_Pagination.Text = ""
                    End If

                    If dt.Rows(0).Item("COLLECTION_TYPE").ToString <> "" Then
                        DDL_CollectionType.SelectedValue = dt.Rows(0).Item("COLLECTION_TYPE").ToString
                    Else
                        DDL_CollectionType.ClearSelection()
                    End If

                    If dt.Rows(0).Item("PERIOD").ToString <> "" Then
                        txt_JHold_Period.Text = dt.Rows(0).Item("PERIOD").ToString
                    Else
                        txt_JHold_Period.Text = ""
                    End If

                    If dt.Rows(0).Item("STA_CODE").ToString <> "" Then
                        DDL_Status.SelectedValue = dt.Rows(0).Item("STA_CODE").ToString
                    Else
                        DDL_Status.ClearSelection()
                    End If

                    If dt.Rows(0).Item("BIND_CODE").ToString <> "" Then
                        DDL_Binding.SelectedValue = dt.Rows(0).Item("BIND_CODE").ToString
                    Else
                        DDL_Binding.ClearSelection()
                    End If

                    If dt.Rows(0).Item("SEC_CODE").ToString <> "" Then
                        DDL_Section.SelectedValue = dt.Rows(0).Item("SEC_CODE").ToString
                    Else
                        DDL_Section.ClearSelection()
                    End If

                    If dt.Rows(0).Item("FORMAT_CODE").ToString <> "" Then
                        DDL_Format.SelectedValue = dt.Rows(0).Item("FORMAT_CODE").ToString
                    Else
                        DDL_Format.ClearSelection()
                    End If

                    If dt.Rows(0).Item("MISSING_ISSUES").ToString <> "" Then
                        txt_JHold_MissingIssues.Text = dt.Rows(0).Item("MISSING_ISSUES").ToString
                    Else
                        txt_JHold_MissingIssues.Text = ""
                    End If

                    If dt.Rows(0).Item("PHYSICAL_LOCATION").ToString <> "" Then
                        txt_JHold_Location.Text = dt.Rows(0).Item("PHYSICAL_LOCATION").ToString
                    Else
                        txt_JHold_Location.Text = ""
                    End If

                    If dt.Rows(0).Item("REMARKS").ToString <> "" Then
                        txt_JHold_Remarks.Text = dt.Rows(0).Item("REMARKS").ToString
                    Else
                        txt_JHold_Remarks.Text = ""
                    End If

                    JHold_Save_Bttn.Visible = False
                    JHold_Update_Bttn.Visible = True
                    JHold_Delete_Bttn.Visible = True
                Else
                    Label19.Text = ""
                    Label16.Text = ""
                    Label17.Text = ""
                    Label18.Text = ""
                    Image4.ImageUrl = Nothing
                    Image4.Visible = True
                    JHold_Save_Bttn.Visible = True
                    JHold_Update_Bttn.Visible = False
                    JHold_Delete_Bttn.Visible = False
                    DDL_Titles.ClearSelection()
                End If
            Else
                Label19.Text = ""
                Label16.Text = ""
                Label17.Text = ""
                Label18.Text = ""
                Image4.ImageUrl = Nothing
                Image4.Visible = True
                JHold_Save_Bttn.Visible = True
                JHold_Update_Bttn.Visible = False
                JHold_Delete_Bttn.Visible = False
                DDL_Titles.ClearSelection()
            End If
            PopulateLooseIssuesGrid()
            txt_JHold_AccessionNo.Focus()
        Catch ex As Exception
            Lbl_Error.Text = "Error: " & (ex.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'fill Grid with Approved Acq Records
    Public Sub PopulateLooseIssuesGrid()
        Dim dtSearch As DataTable = Nothing
        Try
            If DDL_Titles.Text <> "" Then
                Dim SQL As String = Nothing
                SQL = "SELECT HOLDINGS.*,BOOKSTATUS.STA_NAME  FROM HOLDINGS LEFT OUTER JOIN BOOKSTATUS ON HOLDINGS.STA_CODE = BOOKSTATUS.STA_CODE WHERE (HOLDINGS.LIB_CODE = '" & Trim(LibCode) & "') AND (HOLDINGS.CAT_NO = '" & DDL_Titles.SelectedValue & "')  "
                'SQL = SQL & " ORDER BY CASE WHEN LEFT(VOL_NO,1) LIKE  '[A-Za-z]'  THEN dbo.Sort_AlphaNumeric(VOL_NO, 'A-Za-z') END ASC, CAST(dbo.Sort_AlphaNumeric(VOL_NO, '0-9') AS float) ASC ,"
                'SQL = SQL & " CASE WHEN LEFT(ISSUE_NO,1) LIKE  '[A-Za-z]'  THEN dbo.Sort_AlphaNumeric(ISSUE_NO, 'A-Za-z') END ASC, CAST(dbo.Sort_AlphaNumeric(ISSUE_NO, '0-9') AS float) ASC  ,"
                'SQL = SQL & " CASE WHEN LEFT(PART_NO,1) LIKE  '[A-Za-z]'  THEN dbo.Sort_AlphaNumeric(PART_NO, 'A-Za-z') END ASC, CAST(dbo.Sort_AlphaNumeric(PART_NO, '0-9') AS float) ASC ;"
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
                    JHold_DeleteAll_Bttn.Visible = False
                Else
                    Grid1.Visible = True
                    RecordCount = dtSearch.Rows.Count
                    Grid1.DataSource = dtSearch
                    Grid1.DataBind()
                    Label35.Text = "Total Record(s): " & RecordCount
                    JHold_DeleteAll_Bttn.Visible = True
                End If
                ViewState("dt") = dtSearch
            Else
                Me.Grid1.DataSource = Nothing
                Grid1.DataBind()
                Label35.Text = "Total Record(s): 0 "
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
        txt_JHold_AccessionDate.Text = ""
        txt_JHold_AccessionNo.Text = ""
        txt_JHold_BookNo.Text = ""
        txt_JHold_ClassNo.Text = ""
        txt_JHold_IssueNo.Text = ""
        txt_JHold_Location.Text = ""
        txt_JHold_MissingIssues.Text = ""
        txt_JHold_Pagination.Text = ""
        txt_JHold_Period.Text = ""
        txt_JHold_Remarks.Text = ""
        txt_JHold_VolEditors.Text = ""
        txt_JHold_VolNo.Text = ""
        txt_JHold_VolTitle.Text = ""
        txt_JHold_Year.Text = ""
    End Sub
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
    'populate sections
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
            Lbl_Error.Text = "Error: " & (s.Message())
        Finally
            Command.Dispose()
            dt.Dispose()
            SqlConn.Close()
        End Try

    End Sub
    'save bound journal
    Protected Sub JHold_Save_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles JHold_Save_Bttn.Click
        If IsPostBack = True Then
            Dim CopiesRecd As Long = 0
            'check unwanted characters
            Dim iloop As Integer
            Dim strcurrentchar As Object
            Dim c As Integer
            Dim counter1, counter2, counter3, counter4, counter5, counter6, counter7, counter8, counter9, counter10 As Integer
            Dim counter11, counter12, counter13, counter14, counter15, counter16, counter17, counter18 As Integer
            Dim thisTransaction As SqlClient.SqlTransaction = Nothing
            Try

                If DDL_Titles.Text = "" Then
                    Lbl_Error.Text = "Plz Select Title from Drop-Down!"
                    DDL_Titles.Focus()
                    Exit Sub
                End If

                'validation for cat_no
                Dim CAT_NO As Long = Nothing
                If Me.DDL_Titles.Text <> "" Then
                    CAT_NO = Convert.ToInt16(DDL_Titles.SelectedValue)
                    CAT_NO = RemoveQuotes(CAT_NO)
                    If Not IsNumeric(CAT_NO) = True Then
                        Lbl_Error.Text = "Error:Select Title from Drop-Down!"
                        DDL_Titles.Focus()
                        Exit Sub
                    End If
                    If Len(CAT_NO) > 10 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        DDL_Titles.Focus()
                        Exit Sub
                    End If
                    CAT_NO = " " & CAT_NO & " "
                    If InStr(1, CAT_NO, "CREATE", 1) > 0 Or InStr(1, CAT_NO, "DELETE", 1) > 0 Or InStr(1, CAT_NO, "DROP", 1) > 0 Or InStr(1, CAT_NO, "INSERT", 1) > 1 Or InStr(1, CAT_NO, "TRACK", 1) > 1 Or InStr(1, CAT_NO, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
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
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        DDL_Titles.Focus()
                        Exit Sub
                    End If
                Else
                    Lbl_Error.Text = "Error: Select Title from Drop-Down!"
                    DDL_Titles.Focus()
                    Exit Sub
                End If

                'Server Validation for accession no
                Dim ACCESSION_NO As Object = Nothing
                If txt_JHold_AccessionNo.Text <> "" Then
                    ACCESSION_NO = TrimX(UCase(txt_JHold_AccessionNo.Text))
                    ACCESSION_NO = RemoveQuotes(ACCESSION_NO)
                    If ACCESSION_NO.Length > 30 Then 'maximum length
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        txt_JHold_AccessionNo.Focus()
                        Exit Sub
                    End If

                    ACCESSION_NO = " " & ACCESSION_NO & " "
                    If InStr(1, ACCESSION_NO, "CREATE", 1) > 0 Or InStr(1, ACCESSION_NO, "DELETE", 1) > 0 Or InStr(1, ACCESSION_NO, "DROP", 1) > 0 Or InStr(1, ACCESSION_NO, "INSERT", 1) > 1 Or InStr(1, ACCESSION_NO, "TRACK", 1) > 1 Or InStr(1, ACCESSION_NO, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        Me.txt_JHold_AccessionNo.Focus()
                        Exit Sub
                    End If
                    ACCESSION_NO = TrimX(ACCESSION_NO)
                    'check unwanted characters
                    c = 0
                    counter2 = 0
                    For iloop = 1 To Len(ACCESSION_NO.ToString)
                        strcurrentchar = Mid(ACCESSION_NO, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^*+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter2 = 1
                            End If
                        End If
                    Next
                    If counter2 = 1 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        Me.txt_JHold_AccessionNo.Focus()
                        Exit Sub
                    End If

                    'check duplicate isbn
                    Dim str As Object = Nothing
                    Dim flag As Object = Nothing
                    str = "SELECT HOLD_ID FROM HOLDINGS WHERE (ACCESSION_NO = '" & Trim(ACCESSION_NO) & "')  AND (LIB_CODE = '" & Trim(LibCode) & "')"
                    Dim cmd1 As New SqlCommand(str, SqlConn)
                    SqlConn.Open()
                    flag = cmd1.ExecuteScalar
                    SqlConn.Close()
                    If flag <> Nothing Then
                        Lbl_Error.Text = "This Accession No Already Exists ! "
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('This Accession No Already Exists!');", True)
                        Me.txt_JHold_AccessionNo.Focus()
                        Exit Sub
                    End If
                    SqlConn.Close()
                Else
                    Lbl_Error.Text = "Error: Input is not Valid !"
                    txt_JHold_AccessionNo.Focus()
                    Exit Sub
                End If

                'search accession date
                Dim ACCESSION_DATE As Object = Nothing
                If txt_JHold_AccessionDate.Text <> "" Then
                    ACCESSION_DATE = TrimX(txt_JHold_AccessionDate.Text)
                    ACCESSION_DATE = RemoveQuotes(ACCESSION_DATE)
                   
                    If Len(ACCESSION_DATE) <> 10 Then
                        Lbl_Error.Text = " Input is not Valid..."
                        Me.txt_JHold_AccessionDate.Focus()
                        Exit Sub
                    End If
                    ACCESSION_DATE = " " & ACCESSION_DATE & " "
                    If InStr(1, ACCESSION_DATE, "CREATE", 1) > 0 Or InStr(1, ACCESSION_DATE, "DELETE", 1) > 0 Or InStr(1, ACCESSION_DATE, "DROP", 1) > 0 Or InStr(1, ACCESSION_DATE, "INSERT", 1) > 1 Or InStr(1, ACCESSION_DATE, "TRACK", 1) > 1 Or InStr(1, ACCESSION_DATE, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "  Input is not Valid... "
                        Me.txt_JHold_AccessionDate.Focus()
                        Exit Sub
                    End If
                    ACCESSION_DATE = TrimX(ACCESSION_DATE)
                    'check unwanted characters
                    c = 0
                    counter3 = 0
                    For iloop = 1 To Len(ACCESSION_DATE)
                        strcurrentchar = Mid(ACCESSION_DATE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter3 = 1
                            End If
                        End If
                    Next
                    If counter3 = 1 Then
                        Lbl_Error.Text = "data is not Valid... "
                        Me.txt_JHold_AccessionDate.Focus()
                        Exit Sub
                    End If
                    ACCESSION_DATE = Convert.ToDateTime(ACCESSION_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us
                Else
                    ACCESSION_DATE = Now.Date
                    ACCESSION_DATE = Convert.ToDateTime(ACCESSION_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us
                End If

                'validation for Format
                Dim FORMAT_CODE As Object = Nothing
                If DDL_Format.Text <> "" Then
                    FORMAT_CODE = DDL_Format.SelectedValue
                    FORMAT_CODE = RemoveQuotes(FORMAT_CODE)
                    If FORMAT_CODE.Length > 3 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        DDL_Format.Focus()
                        Exit Sub
                    End If

                    FORMAT_CODE = " " & FORMAT_CODE & " "
                    If InStr(1, FORMAT_CODE, "CREATE", 1) > 0 Or InStr(1, FORMAT_CODE, "DELETE", 1) > 0 Or InStr(1, FORMAT_CODE, "DROP", 1) > 0 Or InStr(1, FORMAT_CODE, "INSERT", 1) > 1 Or InStr(1, FORMAT_CODE, "TRACK", 1) > 1 Or InStr(1, FORMAT_CODE, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Do not use Reserve Words!"
                        DDL_Format.Focus()
                        Exit Sub
                    End If
                    FORMAT_CODE = TrimX(FORMAT_CODE)
                    'check unwanted characters
                    c = 0
                    counter4 = 0
                    For iloop = 1 To Len(FORMAT_CODE)
                        strcurrentchar = Mid(FORMAT_CODE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter4 = 1
                            End If
                        End If
                    Next
                    If counter4 = 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        DDL_Format.Focus()
                        Exit Sub
                    End If
                Else
                    FORMAT_CODE = "PT"
                End If

                Dim SHOW As Object = Nothing
                SHOW = "Y"

                Dim ISSUABLE As Object = Nothing
                ISSUABLE = "Y"

                'validation for VOL_NO
                Dim VOL_NO As Object = Nothing
                If txt_JHold_VolNo.Text <> "" Then
                    VOL_NO = TrimAll(txt_JHold_VolNo.Text)
                    VOL_NO = RemoveQuotes(VOL_NO)
                    If VOL_NO.Length > 30 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        txt_JHold_VolNo.Focus()
                        Exit Sub
                    End If

                    VOL_NO = " " & VOL_NO & " "
                    If InStr(1, VOL_NO, "CREATE", 1) > 0 Or InStr(1, VOL_NO, "DELETE", 1) > 0 Or InStr(1, VOL_NO, "DROP", 1) > 0 Or InStr(1, VOL_NO, "INSERT", 1) > 1 Or InStr(1, VOL_NO, "TRACK", 1) > 1 Or InStr(1, VOL_NO, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Do not use Reserve Word"
                        txt_JHold_VolNo.Focus()
                        Exit Sub
                    End If
                    VOL_NO = TrimAll(VOL_NO)
                    'check unwanted characters
                    c = 0
                    counter5 = 0
                    For iloop = 1 To Len(VOL_NO)
                        strcurrentchar = Mid(VOL_NO, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter5 = 1
                            End If
                        End If
                    Next
                    If counter5 = 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        txt_JHold_VolNo.Focus()
                        Exit Sub
                    End If
                Else
                    VOL_NO = ""
                End If

                'validation for ISSUE_NO
                Dim ISSUE_NO As Object = Nothing
                If txt_JHold_IssueNo.Text <> "" Then
                    ISSUE_NO = TrimAll(txt_JHold_IssueNo.Text)
                    ISSUE_NO = RemoveQuotes(ISSUE_NO)
                    If ISSUE_NO.Length > 30 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        txt_JHold_VolNo.Focus()
                        Exit Sub
                    End If

                    ISSUE_NO = " " & ISSUE_NO & " "
                    If InStr(1, ISSUE_NO, "CREATE", 1) > 0 Or InStr(1, ISSUE_NO, "DELETE", 1) > 0 Or InStr(1, ISSUE_NO, "DROP", 1) > 0 Or InStr(1, ISSUE_NO, "INSERT", 1) > 1 Or InStr(1, ISSUE_NO, "TRACK", 1) > 1 Or InStr(1, ISSUE_NO, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Do not use Reserve Word"
                        txt_JHold_IssueNo.Focus()
                        Exit Sub
                    End If
                    ISSUE_NO = TrimAll(ISSUE_NO)
                    'check unwanted characters
                    c = 0
                    counter6 = 0
                    For iloop = 1 To Len(ISSUE_NO)
                        strcurrentchar = Mid(ISSUE_NO, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter6 = 1
                            End If
                        End If
                    Next
                    If counter6 = 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        txt_JHold_IssueNo.Focus()
                        Exit Sub
                    End If
                Else
                    ISSUE_NO = ""
                End If

                'validation for JYEAR
                Dim JYEAR As Object = Nothing
                If Me.txt_JHold_Year.Text <> "" Then
                    JYEAR = TrimAll(txt_JHold_Year.Text)
                    JYEAR = RemoveQuotes(JYEAR)

                    If Len(JYEAR) > 50 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        txt_JHold_Year.Focus()
                        Exit Sub
                    End If
                    JYEAR = " " & JYEAR & " "
                    If InStr(1, JYEAR, "CREATE", 1) > 0 Or InStr(1, JYEAR, "DELETE", 1) > 0 Or InStr(1, JYEAR, "DROP", 1) > 0 Or InStr(1, JYEAR, "INSERT", 1) > 1 Or InStr(1, JYEAR, "TRACK", 1) > 1 Or InStr(1, JYEAR, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        txt_JHold_Year.Focus()
                        Exit Sub
                    End If
                    JYEAR = TrimAll(JYEAR)
                Else
                    JYEAR = ""
                End If

                'validation for VOL_EDITORS
                Dim VOL_EDITORS As Object = Nothing
                If txt_JHold_VolEditors.Text <> "" Then
                    VOL_EDITORS = TrimAll(txt_JHold_VolEditors.Text)
                    VOL_EDITORS = RemoveQuotes(VOL_EDITORS)
                    If VOL_EDITORS.Length > 400 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        txt_JHold_VolEditors.Focus()
                        Exit Sub
                    End If

                    VOL_EDITORS = " " & VOL_EDITORS & " "
                    If InStr(1, VOL_EDITORS, "CREATE", 1) > 0 Or InStr(1, VOL_EDITORS, "DELETE", 1) > 0 Or InStr(1, VOL_EDITORS, "DROP", 1) > 0 Or InStr(1, VOL_EDITORS, "INSERT", 1) > 1 Or InStr(1, VOL_EDITORS, "TRACK", 1) > 1 Or InStr(1, VOL_EDITORS, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        txt_JHold_VolEditors.Focus()
                        Exit Sub
                    End If
                    VOL_EDITORS = TrimAll(VOL_EDITORS)
                    'check unwanted characters
                    c = 0
                    counter7 = 0
                    For iloop = 1 To Len(VOL_EDITORS)
                        strcurrentchar = Mid(VOL_EDITORS, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter7 = 1
                            End If
                        End If
                    Next
                    If counter7 = 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        txt_JHold_VolEditors.Focus()
                        Exit Sub
                    End If
                Else
                    VOL_EDITORS = ""
                End If

                'validation for VOL_TITLE
                Dim VOL_TITLE As Object = Nothing
                If txt_JHold_VolTitle.Text <> "" Then
                    VOL_TITLE = TrimAll(txt_JHold_VolTitle.Text)
                    VOL_TITLE = RemoveQuotes(VOL_TITLE)
                    If VOL_TITLE.Length > 250 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        txt_JHold_VolTitle.Focus()
                        Exit Sub
                    End If

                    VOL_TITLE = " " & VOL_TITLE & " "
                    If InStr(1, VOL_TITLE, "CREATE", 1) > 0 Or InStr(1, VOL_TITLE, "DELETE", 1) > 0 Or InStr(1, VOL_TITLE, "DROP", 1) > 0 Or InStr(1, VOL_TITLE, "INSERT", 1) > 1 Or InStr(1, VOL_TITLE, "TRACK", 1) > 1 Or InStr(1, VOL_TITLE, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Input is not Valid!"
                        txt_JHold_VolTitle.Focus()
                        Exit Sub
                    End If
                    VOL_TITLE = TrimAll(VOL_TITLE)
                    'check unwanted characters
                    c = 0
                    counter8 = 0
                    For iloop = 1 To Len(VOL_TITLE)
                        strcurrentchar = Mid(VOL_TITLE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter8 = 1
                            End If
                        End If
                    Next
                    If counter8 = 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        txt_JHold_VolTitle.Focus()
                        Exit Sub
                    End If
                Else
                    VOL_TITLE = ""
                End If

                'validation for CLASS NO
                Dim CLASS_NO As Object = Nothing
                If txt_JHold_ClassNo.Text <> "" Then
                    CLASS_NO = TrimX(txt_JHold_ClassNo.Text)
                    CLASS_NO = RemoveQuotes(CLASS_NO)
                    If CLASS_NO.Length > 100 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        txt_JHold_ClassNo.Focus()
                        Exit Sub
                    End If

                    CLASS_NO = " " & CLASS_NO & " "
                    If InStr(1, CLASS_NO, "CREATE", 1) > 0 Or InStr(1, CLASS_NO, "DELETE", 1) > 0 Or InStr(1, CLASS_NO, "DROP", 1) > 0 Or InStr(1, CLASS_NO, "INSERT", 1) > 1 Or InStr(1, CLASS_NO, "TRACK", 1) > 1 Or InStr(1, CLASS_NO, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Lbl_Error.Text = "Do not use reserved Words!"
                        txt_JHold_ClassNo.Focus()
                        Exit Sub
                    End If
                    CLASS_NO = TrimX(CLASS_NO)
                    'check unwanted characters
                    c = 0
                    counter9 = 0
                    For iloop = 1 To Len(CLASS_NO)
                        strcurrentchar = Mid(CLASS_NO, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter9 = 1
                            End If
                        End If
                    Next
                    If counter9 = 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        txt_JHold_ClassNo.Focus()
                        Exit Sub
                    End If
                Else
                    CLASS_NO = ""
                End If

                'validation for Book NO
                Dim BOOK_NO As Object = Nothing
                If txt_JHold_BookNo.Text <> "" Then
                    BOOK_NO = TrimAll(UCase(txt_JHold_BookNo.Text))
                    BOOK_NO = RemoveQuotes(BOOK_NO)
                    If BOOK_NO.Length > 50 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        txt_JHold_BookNo.Focus()
                        Exit Sub
                    End If

                    BOOK_NO = " " & BOOK_NO & " "
                    If InStr(1, BOOK_NO, "CREATE", 1) > 0 Or InStr(1, BOOK_NO, "DELETE", 1) > 0 Or InStr(1, BOOK_NO, "DROP", 1) > 0 Or InStr(1, BOOK_NO, "INSERT", 1) > 1 Or InStr(1, BOOK_NO, "TRACK", 1) > 1 Or InStr(1, BOOK_NO, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Do not use reserved Words!"
                        txt_JHold_BookNo.Focus()
                        Exit Sub
                    End If
                    BOOK_NO = TrimAll(BOOK_NO)
                    'check unwanted characters
                    c = 0
                    counter10 = 0
                    For iloop = 1 To Len(BOOK_NO)
                        strcurrentchar = Mid(BOOK_NO, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter10 = 1
                            End If
                        End If
                    Next
                    If counter10 = 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        txt_JHold_BookNo.Focus()
                        Exit Sub
                    End If
                Else
                    BOOK_NO = ""
                End If

                'validation for PAGES
                Dim PAGINATION As Object = Nothing
                If txt_JHold_Pagination.Text <> "" Then
                    PAGINATION = TrimAll(txt_JHold_Pagination.Text)
                    PAGINATION = RemoveQuotes(PAGINATION)
                    If PAGINATION.Length > 50 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        txt_JHold_Pagination.Focus()
                        Exit Sub
                    End If

                    PAGINATION = " " & PAGINATION & " "
                    If InStr(1, PAGINATION, "CREATE", 1) > 0 Or InStr(1, PAGINATION, "DELETE", 1) > 0 Or InStr(1, PAGINATION, "DROP", 1) > 0 Or InStr(1, PAGINATION, "INSERT", 1) > 1 Or InStr(1, PAGINATION, "TRACK", 1) > 1 Or InStr(1, PAGINATION, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Do not use reserved Words!"
                        txt_JHold_Pagination.Focus()
                        Exit Sub
                    End If
                    PAGINATION = TrimAll(PAGINATION)
                    'check unwanted characters
                    c = 0
                    counter11 = 0
                    For iloop = 1 To Len(PAGINATION)
                        strcurrentchar = Mid(PAGINATION, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter11 = 1
                            End If
                        End If
                    Next
                    If counter11 = 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        txt_JHold_Pagination.Focus()
                        Exit Sub
                    End If
                Else
                    PAGINATION = ""
                End If

                'validation for COLLECTION TYPE
                Dim COLLECTION_TYPE As Object = Nothing
                If DDL_CollectionType.Text <> "" Then
                    COLLECTION_TYPE = DDL_CollectionType.SelectedValue
                    COLLECTION_TYPE = RemoveQuotes(COLLECTION_TYPE)
                    If COLLECTION_TYPE.Length > 2 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        DDL_CollectionType.Focus()
                        Exit Sub
                    End If

                    COLLECTION_TYPE = " " & COLLECTION_TYPE & " "
                    If InStr(1, COLLECTION_TYPE, "CREATE", 1) > 0 Or InStr(1, COLLECTION_TYPE, "DELETE", 1) > 0 Or InStr(1, COLLECTION_TYPE, "DROP", 1) > 0 Or InStr(1, COLLECTION_TYPE, "INSERT", 1) > 1 Or InStr(1, COLLECTION_TYPE, "TRACK", 1) > 1 Or InStr(1, COLLECTION_TYPE, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        DDL_CollectionType.Focus()
                        Exit Sub
                    End If
                    COLLECTION_TYPE = TrimX(COLLECTION_TYPE)
                    'check unwanted characters
                    c = 0
                    counter12 = 0
                    For iloop = 1 To Len(COLLECTION_TYPE)
                        strcurrentchar = Mid(COLLECTION_TYPE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter12 = 1
                            End If
                        End If
                    Next
                    If counter12 = 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        DDL_CollectionType.Focus()
                        Exit Sub
                    End If
                Else
                    COLLECTION_TYPE = "C"
                End If

                'validation for STATUS
                Dim STA_CODE As Object = Nothing
                If DDL_Status.Text <> "" Then
                    STA_CODE = DDL_Status.SelectedValue
                    If STA_CODE = "2" Then
                        STA_CODE = "1"
                    End If
                Else
                    STA_CODE = "1"
                End If

                'validation for BINDING TYPE
                Dim BIND_CODE As Object = Nothing
                If DDL_Binding.Text <> "" Then
                    BIND_CODE = DDL_Binding.SelectedValue
                    BIND_CODE = RemoveQuotes(BIND_CODE)
                    If BIND_CODE.Length > 10 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        DDL_Binding.Focus()
                        Exit Sub
                    End If

                    BIND_CODE = " " & BIND_CODE & " "
                    If InStr(1, BIND_CODE, "CREATE", 1) > 0 Or InStr(1, BIND_CODE, "DELETE", 1) > 0 Or InStr(1, BIND_CODE, "DROP", 1) > 0 Or InStr(1, BIND_CODE, "INSERT", 1) > 1 Or InStr(1, BIND_CODE, "TRACK", 1) > 1 Or InStr(1, BIND_CODE, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        DDL_Binding.Focus()
                        Exit Sub
                    End If
                    BIND_CODE = TrimX(BIND_CODE)
                    'check unwanted characters
                    c = 0
                    counter13 = 0
                    For iloop = 1 To Len(BIND_CODE)
                        strcurrentchar = Mid(BIND_CODE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter13 = 1
                            End If
                        End If
                    Next
                    If counter13 = 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        DDL_Binding.Focus()
                        Exit Sub
                    End If
                Else
                    BIND_CODE = "U"
                End If

                'validation for PHYSICAL_LOCATION
                Dim PHYSICAL_LOCATION As Object = Nothing
                If txt_JHold_Location.Text <> "" Then
                    PHYSICAL_LOCATION = TrimAll(txt_JHold_Location.Text)
                    PHYSICAL_LOCATION = RemoveQuotes(PHYSICAL_LOCATION)
                    If PHYSICAL_LOCATION.Length > 50 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        txt_JHold_Location.Focus()
                        Exit Sub
                    End If

                    PHYSICAL_LOCATION = " " & PHYSICAL_LOCATION & " "
                    If InStr(1, PHYSICAL_LOCATION, "CREATE", 1) > 0 Or InStr(1, PHYSICAL_LOCATION, "DELETE", 1) > 0 Or InStr(1, PHYSICAL_LOCATION, "DROP", 1) > 0 Or InStr(1, PHYSICAL_LOCATION, "INSERT", 1) > 1 Or InStr(1, PHYSICAL_LOCATION, "TRACK", 1) > 1 Or InStr(1, PHYSICAL_LOCATION, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        txt_JHold_Location.Focus()
                        Exit Sub
                    End If
                    PHYSICAL_LOCATION = TrimAll(PHYSICAL_LOCATION)
                    'check unwanted characters
                    c = 0
                    counter14 = 0
                    For iloop = 1 To Len(PHYSICAL_LOCATION)
                        strcurrentchar = Mid(PHYSICAL_LOCATION, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter14 = 1
                            End If
                        End If
                    Next
                    If counter14 = 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        txt_JHold_Location.Focus()
                        Exit Sub
                    End If
                Else
                    PHYSICAL_LOCATION = ""
                End If

                'validation for SEC_CODE
                Dim SEC_CODE As Object = Nothing
                If DDL_Section.Text <> "" Then
                    SEC_CODE = Trim(DDL_Section.SelectedValue)
                    SEC_CODE = RemoveQuotes(SEC_CODE)
                    If SEC_CODE.Length > 10 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        DDL_Section.Focus()
                        Exit Sub
                    End If

                    SEC_CODE = " " & SEC_CODE & " "
                    If InStr(1, SEC_CODE, "CREATE", 1) > 0 Or InStr(1, SEC_CODE, "DELETE", 1) > 0 Or InStr(1, SEC_CODE, "DROP", 1) > 0 Or InStr(1, SEC_CODE, "INSERT", 1) > 1 Or InStr(1, SEC_CODE, "TRACK", 1) > 1 Or InStr(1, SEC_CODE, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        DDL_Section.Focus()
                        Exit Sub
                    End If
                    SEC_CODE = Trim(SEC_CODE)
                    'check unwanted characters
                    c = 0
                    counter15 = 0
                    For iloop = 1 To Len(SEC_CODE)
                        strcurrentchar = Mid(SEC_CODE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter15 = 1
                            End If
                        End If
                    Next
                    If counter15 = 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        DDL_Section.Focus()
                        Exit Sub
                    End If
                Else
                    SEC_CODE = ""
                End If

                'validation for REMARKS
                Dim REMARKS As Object = Nothing
                If txt_JHold_Remarks.Text <> "" Then
                    REMARKS = TrimAll(txt_JHold_Remarks.Text)
                    REMARKS = RemoveQuotes(REMARKS)
                    If REMARKS.Length > 250 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        txt_JHold_Remarks.Focus()
                        Exit Sub
                    End If

                    REMARKS = " " & REMARKS & " "
                    If InStr(1, REMARKS, "CREATE", 1) > 0 Or InStr(1, REMARKS, "DELETE", 1) > 0 Or InStr(1, REMARKS, "DROP", 1) > 0 Or InStr(1, REMARKS, "INSERT", 1) > 1 Or InStr(1, REMARKS, "TRACK", 1) > 1 Or InStr(1, REMARKS, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        txt_JHold_Remarks.Focus()
                        Exit Sub
                    End If
                    REMARKS = TrimAll(REMARKS)
                    'check unwanted characters
                    c = 0
                    counter16 = 0
                    For iloop = 1 To Len(REMARKS)
                        strcurrentchar = Mid(REMARKS, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter16 = 1
                            End If
                        End If
                    Next
                    If counter16 = 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        txt_JHold_Remarks.Focus()
                        Exit Sub
                    End If
                Else
                    REMARKS = ""
                End If

                'validation for MISSING_ISSUES
                Dim MISSING_ISSUES As Object = Nothing
                If txt_JHold_MissingIssues.Text <> "" Then
                    MISSING_ISSUES = TrimAll(txt_JHold_MissingIssues.Text)
                    MISSING_ISSUES = RemoveQuotes(MISSING_ISSUES)
                    If MISSING_ISSUES.Length > 100 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        txt_JHold_MissingIssues.Focus()
                        Exit Sub
                    End If

                    MISSING_ISSUES = " " & MISSING_ISSUES & " "
                    If InStr(1, MISSING_ISSUES, "CREATE", 1) > 0 Or InStr(1, MISSING_ISSUES, "DELETE", 1) > 0 Or InStr(1, MISSING_ISSUES, "DROP", 1) > 0 Or InStr(1, MISSING_ISSUES, "INSERT", 1) > 1 Or InStr(1, MISSING_ISSUES, "TRACK", 1) > 1 Or InStr(1, MISSING_ISSUES, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        txt_JHold_MissingIssues.Focus()
                        Exit Sub
                    End If
                    MISSING_ISSUES = TrimAll(MISSING_ISSUES)
                    'check unwanted characters
                    c = 0
                    counter17 = 0
                    For iloop = 1 To Len(MISSING_ISSUES)
                        strcurrentchar = Mid(MISSING_ISSUES, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter17 = 1
                            End If
                        End If
                    Next
                    If counter17 = 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        txt_JHold_MissingIssues.Focus()
                        Exit Sub
                    End If
                Else
                    MISSING_ISSUES = ""
                End If

                'validation for PERIOD
                Dim PERIOD As Object = Nothing
                If txt_JHold_Period.Text <> "" Then
                    PERIOD = TrimAll(txt_JHold_Period.Text)
                    PERIOD = RemoveQuotes(PERIOD)
                    If PERIOD.Length > 100 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        txt_JHold_Period.Focus()
                        Exit Sub
                    End If

                    PERIOD = " " & PERIOD & " "
                    If InStr(1, PERIOD, "CREATE", 1) > 0 Or InStr(1, PERIOD, "DELETE", 1) > 0 Or InStr(1, PERIOD, "DROP", 1) > 0 Or InStr(1, PERIOD, "INSERT", 1) > 1 Or InStr(1, PERIOD, "TRACK", 1) > 1 Or InStr(1, PERIOD, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        txt_JHold_Period.Focus()
                        Exit Sub
                    End If
                    PERIOD = TrimAll(PERIOD)
                    'check unwanted characters
                    c = 0
                    counter18 = 0
                    For iloop = 1 To Len(PERIOD)
                        strcurrentchar = Mid(PERIOD, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter18 = 1
                            End If
                        End If
                    Next
                    If counter18 = 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        txt_JHold_Period.Focus()
                        Exit Sub
                    End If
                Else
                    PERIOD = ""
                End If

                Dim USER_CODE As Object = Nothing
                USER_CODE = Session.Item("LoggedUser")

                Dim DATE_ADDED As Object = Nothing
                DATE_ADDED = Now.Date
                DATE_ADDED = Convert.ToDateTime(DATE_ADDED, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                Dim IP As Object = Nothing
                IP = Request.UserHostAddress.Trim

                'thisTransaction = SqlConn.BeginTransaction()
                If CAT_NO <> 0 Then
                    If SqlConn.State = 0 Then
                        SqlConn.Open()
                    End If
                    thisTransaction = SqlConn.BeginTransaction()

                    Dim intValue As Integer = 0
                    Dim objCommand As New SqlCommand
                    objCommand.Connection = SqlConn
                    objCommand.Transaction = thisTransaction
                    objCommand.CommandType = CommandType.Text
                    objCommand.CommandText = "INSERT INTO HOLDINGS (CAT_NO, ACCESSION_NO, ACCESSION_DATE, FORMAT_CODE, SHOW, ISSUEABLE, VOL_NO, JYEAR, VOL_EDITORS, VOL_TITLE, CLASS_NO, BOOK_NO, PAGINATION, COLLECTION_TYPE, STA_CODE, BIND_CODE, SEC_CODE, LIB_CODE, REMARKS, PHYSICAL_LOCATION, DATE_ADDED, USER_CODE, IP,ISSUE_NO, MISSING_ISSUES, PERIOD ) " & _
                                             " VALUES (@CAT_NO, @ACCESSION_NO, @ACCESSION_DATE, @FORMAT_CODE, @SHOW, @ISSUEABLE, @VOL_NO, @JYEAR, @VOL_EDITORS, @VOL_TITLE, @CLASS_NO, @BOOK_NO, @PAGINATION, @COLLECTION_TYPE, @STA_CODE, @BIND_CODE, @SEC_CODE, @LIB_CODE, @REMARKS, @PHYSICAL_LOCATION, @DATE_ADDED, @USER_CODE, @IP, @ISSUE_NO, @MISSING_ISSUES, @PERIOD );  " & _
                                             " SELECT SCOPE_IDENTITY()"

                    objCommand.Parameters.Add("@CAT_NO", SqlDbType.Int)
                    objCommand.Parameters("@CAT_NO").Value = CAT_NO

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
                    objCommand.Parameters.Add("@ISSUEABLE", SqlDbType.VarChar)
                    objCommand.Parameters("@ISSUEABLE").Value = ISSUABLE

                    If VOL_NO = "" Then VOL_NO = System.DBNull.Value
                    objCommand.Parameters.Add("@VOL_NO", SqlDbType.NVarChar)
                    objCommand.Parameters("@VOL_NO").Value = VOL_NO

                    objCommand.Parameters.Add("@ISSUE_NO", SqlDbType.NVarChar)
                    If ISSUE_NO = "" Then
                        objCommand.Parameters("@ISSUE_NO").Value = System.DBNull.Value
                    Else
                        objCommand.Parameters("@ISSUE_NO").Value = ISSUE_NO
                    End If

                    objCommand.Parameters.Add("@JYEAR", SqlDbType.VarChar)
                    If JYEAR = "" Then
                        objCommand.Parameters("@JYEAR").Value = System.DBNull.Value
                    Else
                        objCommand.Parameters("@JYEAR").Value = JYEAR
                    End If

                    objCommand.Parameters.Add("@VOL_EDITORS", SqlDbType.NVarChar)
                    If VOL_EDITORS = "" Then
                        objCommand.Parameters("@VOL_EDITORS").Value = System.DBNull.Value
                    Else
                        objCommand.Parameters("@VOL_EDITORS").Value = VOL_EDITORS
                    End If

                    objCommand.Parameters.Add("@VOL_TITLE", SqlDbType.NVarChar)
                    If VOL_TITLE = "" Then
                        objCommand.Parameters("@VOL_TITLE").Value = System.DBNull.Value
                    Else
                        objCommand.Parameters("@VOL_TITLE").Value = VOL_TITLE
                    End If

                    objCommand.Parameters.Add("@CLASS_NO", SqlDbType.NVarChar)
                    If CLASS_NO = "" Then
                        objCommand.Parameters("@CLASS_NO").Value = System.DBNull.Value
                    Else
                        objCommand.Parameters("@CLASS_NO").Value = CLASS_NO
                    End If

                    objCommand.Parameters.Add("@BOOK_NO", SqlDbType.NVarChar)
                    If BOOK_NO = "" Then
                        objCommand.Parameters("@BOOK_NO").Value = System.DBNull.Value
                    Else
                        objCommand.Parameters("@BOOK_NO").Value = BOOK_NO
                    End If

                    objCommand.Parameters.Add("@PAGINATION", SqlDbType.NVarChar)
                    If PAGINATION = "" Then
                        objCommand.Parameters("@PAGINATION").Value = System.DBNull.Value
                    Else
                        objCommand.Parameters("@PAGINATION").Value = PAGINATION
                    End If

                    If COLLECTION_TYPE = "" Then COLLECTION_TYPE = System.DBNull.Value
                    objCommand.Parameters.Add("@COLLECTION_TYPE", SqlDbType.VarChar)
                    objCommand.Parameters("@COLLECTION_TYPE").Value = COLLECTION_TYPE

                    If STA_CODE = "" Then STA_CODE = "1"
                    objCommand.Parameters.Add("@STA_CODE", SqlDbType.VarChar)
                    objCommand.Parameters("@STA_CODE").Value = STA_CODE

                    objCommand.Parameters.Add("@BIND_CODE", SqlDbType.VarChar)
                    If BIND_CODE = "" Then
                        objCommand.Parameters("@BIND_CODE").Value = System.DBNull.Value
                    Else
                        objCommand.Parameters("@BIND_CODE").Value = BIND_CODE
                    End If

                    objCommand.Parameters.Add("@SEC_CODE", SqlDbType.VarChar)
                    If SEC_CODE = "" Then
                        objCommand.Parameters("@SEC_CODE").Value = System.DBNull.Value
                    Else
                        objCommand.Parameters("@SEC_CODE").Value = SEC_CODE
                    End If

                    If LibCode = "" Then LibCode = System.DBNull.Value
                    objCommand.Parameters.Add("@LIB_CODE", SqlDbType.NVarChar)
                    objCommand.Parameters("@LIB_CODE").Value = LibCode

                    objCommand.Parameters.Add("@REMARKS", SqlDbType.NVarChar)
                    If REMARKS = "" Then
                        objCommand.Parameters("@REMARKS").Value = System.DBNull.Value
                    Else
                        objCommand.Parameters("@REMARKS").Value = REMARKS
                    End If

                    objCommand.Parameters.Add("@PHYSICAL_LOCATION", SqlDbType.NVarChar)
                    If PHYSICAL_LOCATION = "" Then
                        objCommand.Parameters("@PHYSICAL_LOCATION").Value = System.DBNull.Value
                    Else
                        objCommand.Parameters("@PHYSICAL_LOCATION").Value = PHYSICAL_LOCATION
                    End If

                    objCommand.Parameters.Add("@MISSING_ISSUES", SqlDbType.NVarChar)
                    If MISSING_ISSUES = "" Then
                        objCommand.Parameters("@MISSING_ISSUES").Value = System.DBNull.Value
                    Else
                        objCommand.Parameters("@MISSING_ISSUES").Value = MISSING_ISSUES
                    End If

                    objCommand.Parameters.Add("@PERIOD", SqlDbType.NVarChar)
                    If PERIOD = "" Then
                        objCommand.Parameters("@PERIOD").Value = System.DBNull.Value
                    Else
                        objCommand.Parameters("@PERIOD").Value = PERIOD
                    End If

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
                    Lbl_Error.Text = ""
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Record Added Successfully!');", True)
                    thisTransaction.Commit()
                    SqlConn.Close()
                    txt_JHold_AccessionNo.Text = ""
                End If
                PopulateLooseIssuesGrid()
            Catch q As SqlException
                thisTransaction.Rollback()
                Lbl_Error.Text = "Database Error -SAVE: " & (q.Message() & ", " & q.Number & ", " & q.GetType.ToString)
            Catch ex As Exception
                Lbl_Error.Text = "Error-SAVE: " & (ex.Message())
            Finally
                SqlConn.Close()
            End Try
        End If

    End Sub
    'get value of row from grid
    Private Sub Grid1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles Grid1.RowCommand
        Dim Command As SqlCommand = Nothing
        Dim dr As SqlDataReader = Nothing
        Try
            If e.CommandName = "Select" Then
                Label36.Text = ""
                Lbl_Error.Text = ""
                ClearFields()

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
                        Lbl_Error.Text = "Length of Input is not Proper!"
                        Exit Sub
                    End If
                    HOLD_ID = " " & HOLD_ID & " "
                    If InStr(1, HOLD_ID, " CREATE ", 1) > 0 Or InStr(1, HOLD_ID, " DELETE ", 1) > 0 Or InStr(1, HOLD_ID, " DROP ", 1) > 0 Or InStr(1, HOLD_ID, " INSERT ", 1) > 1 Or InStr(1, HOLD_ID, " TRACK ", 1) > 1 Or InStr(1, HOLD_ID, " TRACE ", 1) > 1 Then
                        Lbl_Error.Text = "Do not use reserve words... !"
                        Exit Sub
                    End If
                    HOLD_ID = TrimX(HOLD_ID)
                    'get record details from database
                    Dim SQL As String = Nothing
                    SQL = " SELECT *  FROM HOLDINGS WHERE (HOLD_ID = '" & Trim(HOLD_ID) & "') AND (LIB_CODE ='" & Trim(LibCode) & "') ; "
                    Command = New SqlCommand(SQL, SqlConn)
                    SqlConn.Open()
                    dr = Command.ExecuteReader(CommandBehavior.CloseConnection)
                    dr.Read()
                   
                    If dr.HasRows = True Then
                        If dr.Item("HOLD_ID").ToString <> "" Then
                            Label36.Text = dr.Item("HOLD_ID").ToString
                        Else
                            Label36.Text = ""
                        End If
                        If dr.Item("ACCESSION_NO").ToString <> "" Then
                            txt_JHold_AccessionNo.Text = dr.Item("ACCESSION_NO").ToString
                        Else
                            txt_JHold_AccessionNo.Text = ""
                        End If

                        If dr.Item("ACCESSION_DATE").ToString <> "" Then
                            txt_JHold_AccessionDate.Text = Format(dr.Item("ACCESSION_DATE"), "dd/MM/yyyy")
                        Else
                            txt_JHold_AccessionDate.Text = ""
                        End If

                        If dr.Item("VOL_NO").ToString <> "" Then
                            txt_JHold_VolNo.Text = dr.Item("VOL_NO").ToString
                        Else
                            txt_JHold_VolNo.Text = ""
                        End If

                        If dr.Item("ISSUE_NO").ToString <> "" Then
                            txt_JHold_IssueNo.Text = dr.Item("ISSUE_NO").ToString
                        Else
                            txt_JHold_IssueNo.Text = ""
                        End If

                        If dr.Item("JYEAR").ToString <> "" Then
                            txt_JHold_Year.Text = dr.Item("JYEAR").ToString
                        Else
                            txt_JHold_Year.Text = ""
                        End If

                        If dr.Item("VOL_TITLE").ToString <> "" Then
                            txt_JHold_VolTitle.Text = dr.Item("VOL_TITLE").ToString
                        Else
                            txt_JHold_VolTitle.Text = ""
                        End If


                        If dr.Item("VOL_EDITORS").ToString <> "" Then
                            txt_JHold_VolEditors.Text = dr.Item("VOL_EDITORS").ToString
                        Else
                            txt_JHold_VolEditors.Text = ""
                        End If

                        If dr.Item("CLASS_NO").ToString <> "" Then
                            txt_JHold_ClassNo.Text = dr.Item("CLASS_NO").ToString
                        Else
                            txt_JHold_ClassNo.Text = ""
                        End If

                        If dr.Item("BOOK_NO").ToString <> "" Then
                            txt_JHold_BookNo.Text = dr.Item("BOOK_NO").ToString
                        Else
                            txt_JHold_BookNo.Text = ""
                        End If

                        If dr.Item("PAGINATION").ToString <> "" Then
                            txt_JHold_Pagination.Text = dr.Item("PAGINATION").ToString
                        Else
                            txt_JHold_Pagination.Text = ""
                        End If

                        If dr.Item("COLLECTION_TYPE").ToString <> "" Then
                            DDL_CollectionType.SelectedValue = dr.Item("COLLECTION_TYPE").ToString
                        Else
                            DDL_CollectionType.ClearSelection()
                        End If

                        If dr.Item("PERIOD").ToString <> "" Then
                            txt_JHold_Period.Text = dr.Item("PERIOD").ToString
                        Else
                            txt_JHold_Period.Text = ""
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

                        If dr.Item("FORMAT_CODE").ToString <> "" Then
                            DDL_Format.SelectedValue = dr.Item("FORMAT_CODE").ToString
                        Else
                            DDL_Format.ClearSelection()
                        End If

                        If dr.Item("MISSING_ISSUES").ToString <> "" Then
                            txt_JHold_MissingIssues.Text = dr.Item("MISSING_ISSUES").ToString
                        Else
                            txt_JHold_MissingIssues.Text = ""
                        End If

                        If dr.Item("PHYSICAL_LOCATION").ToString <> "" Then
                            txt_JHold_Location.Text = dr.Item("PHYSICAL_LOCATION").ToString
                        Else
                            txt_JHold_Location.Text = ""
                        End If

                        If dr.Item("REMARKS").ToString <> "" Then
                            txt_JHold_Remarks.Text = dr.Item("REMARKS").ToString
                        Else
                            txt_JHold_Remarks.Text = ""
                        End If

                        'load DE Format
                        Me.JHold_Save_Bttn.Visible = False
                        Me.JHold_Update_Bttn.Visible = True
                        JHold_Delete_Bttn.Visible = True
                        dr.Close()
                    End If
                End If
            End If
        Catch s As Exception
            Lbl_Error.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'cancel button
    Protected Sub JHold_Cancel_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles JHold_Cancel_Bttn.Click
        ClearFields()
        Label36.Text = ""
        Me.JHold_Save_Bttn.Visible = True
        Me.JHold_Update_Bttn.Visible = False
        JHold_Delete_Bttn.Visible = False
    End Sub
    'update record
    Protected Sub JHold_Update_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles JHold_Update_Bttn.Click
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
                If Label36.Text = "" Then
                    Lbl_Error.Text = "Plz Select Title from Drop-Down!"
                    DDL_Titles.Focus()
                    Exit Sub
                End If

                'validation for HOLD_ID
                Dim HOLD_ID As Long = Nothing
                If Me.Label36.Text <> "" Then
                    HOLD_ID = Convert.ToInt16(Label36.Text)
                    HOLD_ID = RemoveQuotes(HOLD_ID)
                    If Not IsNumeric(HOLD_ID) = True Then
                        Lbl_Error.Text = "Error:Select Title from Drop-Down!"
                        DDL_Titles.Focus()
                        Exit Sub
                    End If
                    If Len(HOLD_ID) > 10 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        DDL_Titles.Focus()
                        Exit Sub
                    End If
                    HOLD_ID = " " & HOLD_ID & " "
                    If InStr(1, HOLD_ID, "CREATE", 1) > 0 Or InStr(1, HOLD_ID, "DELETE", 1) > 0 Or InStr(1, HOLD_ID, "DROP", 1) > 0 Or InStr(1, HOLD_ID, "INSERT", 1) > 1 Or InStr(1, HOLD_ID, "TRACK", 1) > 1 Or InStr(1, HOLD_ID, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        Exit Sub
                    End If
                    HOLD_ID = TrimX(HOLD_ID)
                    'check unwanted characters
                    c = 0
                    counter1 = 0
                    For iloop = 1 To Len(HOLD_ID.ToString)
                        strcurrentchar = Mid(HOLD_ID, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter1 = 1
                            End If
                        End If
                    Next
                    If counter1 = 1 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        DDL_Titles.Focus()
                        Exit Sub
                    End If
                Else
                    Lbl_Error.Text = "Error: Select Title from Drop-Down!"
                    DDL_Titles.Focus()
                    Exit Sub
                End If

                'Server Validation for accession no
                Dim ACCESSION_NO As Object = Nothing
                If txt_JHold_AccessionNo.Text <> "" Then
                    ACCESSION_NO = TrimX(UCase(txt_JHold_AccessionNo.Text))
                    ACCESSION_NO = RemoveQuotes(ACCESSION_NO)
                    If ACCESSION_NO.Length > 30 Then 'maximum length
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        txt_JHold_AccessionNo.Focus()
                        Exit Sub
                    End If

                    ACCESSION_NO = " " & ACCESSION_NO & " "
                    If InStr(1, ACCESSION_NO, "CREATE", 1) > 0 Or InStr(1, ACCESSION_NO, "DELETE", 1) > 0 Or InStr(1, ACCESSION_NO, "DROP", 1) > 0 Or InStr(1, ACCESSION_NO, "INSERT", 1) > 1 Or InStr(1, ACCESSION_NO, "TRACK", 1) > 1 Or InStr(1, ACCESSION_NO, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        Me.txt_JHold_AccessionNo.Focus()
                        Exit Sub
                    End If
                    ACCESSION_NO = TrimX(ACCESSION_NO)
                    'check unwanted characters
                    c = 0
                    counter2 = 0
                    For iloop = 1 To Len(ACCESSION_NO.ToString)
                        strcurrentchar = Mid(ACCESSION_NO, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^*+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter2 = 1
                            End If
                        End If
                    Next
                    If counter2 = 1 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        Me.txt_JHold_AccessionNo.Focus()
                        Exit Sub
                    End If

                    'check duplicate isbn
                    Dim str As Object = Nothing
                    Dim flag As Object = Nothing
                    str = "SELECT HOLD_ID FROM HOLDINGS WHERE (HOLD_ID <> ' " & Trim(HOLD_ID) & "') AND (ACCESSION_NO = '" & Trim(ACCESSION_NO) & "')  AND (LIB_CODE = '" & Trim(LibCode) & "')"
                    Dim cmd1 As New SqlCommand(str, SqlConn)
                    SqlConn.Open()
                    flag = cmd1.ExecuteScalar
                    SqlConn.Close()
                    If flag <> Nothing Then
                        Lbl_Error.Text = "This Accession No Already Exists ! "
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('This Accession No Already Exists!');", True)
                        Me.txt_JHold_AccessionNo.Focus()
                        Exit Sub
                    End If
                    SqlConn.Close()
                Else
                    Lbl_Error.Text = "Error: Input is not Valid !"
                    txt_JHold_AccessionNo.Focus()
                    Exit Sub
                End If

                'search accession date
                Dim ACCESSION_DATE As Object = Nothing
                If txt_JHold_AccessionDate.Text <> "" Then
                    ACCESSION_DATE = TrimX(txt_JHold_AccessionDate.Text)
                    ACCESSION_DATE = RemoveQuotes(ACCESSION_DATE)

                    If Len(ACCESSION_DATE) <> 10 Then
                        Lbl_Error.Text = " Input is not Valid..."
                        Me.txt_JHold_AccessionDate.Focus()
                        Exit Sub
                    End If
                    ACCESSION_DATE = " " & ACCESSION_DATE & " "
                    If InStr(1, ACCESSION_DATE, "CREATE", 1) > 0 Or InStr(1, ACCESSION_DATE, "DELETE", 1) > 0 Or InStr(1, ACCESSION_DATE, "DROP", 1) > 0 Or InStr(1, ACCESSION_DATE, "INSERT", 1) > 1 Or InStr(1, ACCESSION_DATE, "TRACK", 1) > 1 Or InStr(1, ACCESSION_DATE, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "  Input is not Valid... "
                        Me.txt_JHold_AccessionDate.Focus()
                        Exit Sub
                    End If
                    ACCESSION_DATE = TrimX(ACCESSION_DATE)
                    'check unwanted characters
                    c = 0
                    counter3 = 0
                    For iloop = 1 To Len(ACCESSION_DATE)
                        strcurrentchar = Mid(ACCESSION_DATE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter3 = 1
                            End If
                        End If
                    Next
                    If counter3 = 1 Then
                        Lbl_Error.Text = "data is not Valid... "
                        Me.txt_JHold_AccessionDate.Focus()
                        Exit Sub
                    End If
                    ACCESSION_DATE = Convert.ToDateTime(ACCESSION_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us
                Else
                    ACCESSION_DATE = Now.Date
                    ACCESSION_DATE = Convert.ToDateTime(ACCESSION_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us
                End If

                'validation for Format
                Dim FORMAT_CODE As Object = Nothing
                If DDL_Format.Text <> "" Then
                    FORMAT_CODE = DDL_Format.SelectedValue
                    FORMAT_CODE = RemoveQuotes(FORMAT_CODE)
                    If FORMAT_CODE.Length > 3 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        DDL_Format.Focus()
                        Exit Sub
                    End If

                    FORMAT_CODE = " " & FORMAT_CODE & " "
                    If InStr(1, FORMAT_CODE, "CREATE", 1) > 0 Or InStr(1, FORMAT_CODE, "DELETE", 1) > 0 Or InStr(1, FORMAT_CODE, "DROP", 1) > 0 Or InStr(1, FORMAT_CODE, "INSERT", 1) > 1 Or InStr(1, FORMAT_CODE, "TRACK", 1) > 1 Or InStr(1, FORMAT_CODE, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Do not use Reserve Words!"
                        DDL_Format.Focus()
                        Exit Sub
                    End If
                    FORMAT_CODE = TrimX(FORMAT_CODE)
                    'check unwanted characters
                    c = 0
                    counter4 = 0
                    For iloop = 1 To Len(FORMAT_CODE)
                        strcurrentchar = Mid(FORMAT_CODE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter4 = 1
                            End If
                        End If
                    Next
                    If counter4 = 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        DDL_Format.Focus()
                        Exit Sub
                    End If
                Else
                    FORMAT_CODE = "PT"
                End If

                'validation for VOL_NO
                Dim VOL_NO As Object = Nothing
                If txt_JHold_VolNo.Text <> "" Then
                    VOL_NO = TrimAll(txt_JHold_VolNo.Text)
                    VOL_NO = RemoveQuotes(VOL_NO)
                    If VOL_NO.Length > 30 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        txt_JHold_VolNo.Focus()
                        Exit Sub
                    End If

                    VOL_NO = " " & VOL_NO & " "
                    If InStr(1, VOL_NO, "CREATE", 1) > 0 Or InStr(1, VOL_NO, "DELETE", 1) > 0 Or InStr(1, VOL_NO, "DROP", 1) > 0 Or InStr(1, VOL_NO, "INSERT", 1) > 1 Or InStr(1, VOL_NO, "TRACK", 1) > 1 Or InStr(1, VOL_NO, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Do not use Reserve Word"
                        txt_JHold_VolNo.Focus()
                        Exit Sub
                    End If
                    VOL_NO = TrimAll(VOL_NO)
                    'check unwanted characters
                    c = 0
                    counter5 = 0
                    For iloop = 1 To Len(VOL_NO)
                        strcurrentchar = Mid(VOL_NO, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter5 = 1
                            End If
                        End If
                    Next
                    If counter5 = 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        txt_JHold_VolNo.Focus()
                        Exit Sub
                    End If
                Else
                    VOL_NO = ""
                End If

                'validation for ISSUE_NO
                Dim ISSUE_NO As Object = Nothing
                If txt_JHold_IssueNo.Text <> "" Then
                    ISSUE_NO = TrimAll(txt_JHold_IssueNo.Text)
                    ISSUE_NO = RemoveQuotes(ISSUE_NO)
                    If ISSUE_NO.Length > 30 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        txt_JHold_VolNo.Focus()
                        Exit Sub
                    End If

                    ISSUE_NO = " " & ISSUE_NO & " "
                    If InStr(1, ISSUE_NO, "CREATE", 1) > 0 Or InStr(1, ISSUE_NO, "DELETE", 1) > 0 Or InStr(1, ISSUE_NO, "DROP", 1) > 0 Or InStr(1, ISSUE_NO, "INSERT", 1) > 1 Or InStr(1, ISSUE_NO, "TRACK", 1) > 1 Or InStr(1, ISSUE_NO, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Do not use Reserve Word"
                        txt_JHold_IssueNo.Focus()
                        Exit Sub
                    End If
                    ISSUE_NO = TrimAll(ISSUE_NO)
                    'check unwanted characters
                    c = 0
                    counter6 = 0
                    For iloop = 1 To Len(ISSUE_NO)
                        strcurrentchar = Mid(ISSUE_NO, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter6 = 1
                            End If
                        End If
                    Next
                    If counter6 = 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        txt_JHold_IssueNo.Focus()
                        Exit Sub
                    End If
                Else
                    ISSUE_NO = ""
                End If

                'validation for JYEAR
                Dim JYEAR As Object = Nothing
                If Me.txt_JHold_Year.Text <> "" Then
                    JYEAR = TrimAll(txt_JHold_Year.Text)
                    JYEAR = RemoveQuotes(JYEAR)

                    If Len(JYEAR) > 50 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        txt_JHold_Year.Focus()
                        Exit Sub
                    End If
                    JYEAR = " " & JYEAR & " "
                    If InStr(1, JYEAR, "CREATE", 1) > 0 Or InStr(1, JYEAR, "DELETE", 1) > 0 Or InStr(1, JYEAR, "DROP", 1) > 0 Or InStr(1, JYEAR, "INSERT", 1) > 1 Or InStr(1, JYEAR, "TRACK", 1) > 1 Or InStr(1, JYEAR, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        txt_JHold_Year.Focus()
                        Exit Sub
                    End If
                    JYEAR = TrimAll(JYEAR)
                Else
                    JYEAR = ""
                End If

                'validation for VOL_EDITORS
                Dim VOL_EDITORS As Object = Nothing
                If txt_JHold_VolEditors.Text <> "" Then
                    VOL_EDITORS = TrimAll(txt_JHold_VolEditors.Text)
                    VOL_EDITORS = RemoveQuotes(VOL_EDITORS)
                    If VOL_EDITORS.Length > 400 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        txt_JHold_VolEditors.Focus()
                        Exit Sub
                    End If

                    VOL_EDITORS = " " & VOL_EDITORS & " "
                    If InStr(1, VOL_EDITORS, "CREATE", 1) > 0 Or InStr(1, VOL_EDITORS, "DELETE", 1) > 0 Or InStr(1, VOL_EDITORS, "DROP", 1) > 0 Or InStr(1, VOL_EDITORS, "INSERT", 1) > 1 Or InStr(1, VOL_EDITORS, "TRACK", 1) > 1 Or InStr(1, VOL_EDITORS, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        txt_JHold_VolEditors.Focus()
                        Exit Sub
                    End If
                    VOL_EDITORS = TrimAll(VOL_EDITORS)
                    'check unwanted characters
                    c = 0
                    counter7 = 0
                    For iloop = 1 To Len(VOL_EDITORS)
                        strcurrentchar = Mid(VOL_EDITORS, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter7 = 1
                            End If
                        End If
                    Next
                    If counter7 = 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        txt_JHold_VolEditors.Focus()
                        Exit Sub
                    End If
                Else
                    VOL_EDITORS = ""
                End If

                'validation for VOL_TITLE
                Dim VOL_TITLE As Object = Nothing
                If txt_JHold_VolTitle.Text <> "" Then
                    VOL_TITLE = TrimAll(txt_JHold_VolTitle.Text)
                    VOL_TITLE = RemoveQuotes(VOL_TITLE)
                    If VOL_TITLE.Length > 250 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        txt_JHold_VolTitle.Focus()
                        Exit Sub
                    End If

                    VOL_TITLE = " " & VOL_TITLE & " "
                    If InStr(1, VOL_TITLE, "CREATE", 1) > 0 Or InStr(1, VOL_TITLE, "DELETE", 1) > 0 Or InStr(1, VOL_TITLE, "DROP", 1) > 0 Or InStr(1, VOL_TITLE, "INSERT", 1) > 1 Or InStr(1, VOL_TITLE, "TRACK", 1) > 1 Or InStr(1, VOL_TITLE, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Input is not Valid!"
                        txt_JHold_VolTitle.Focus()
                        Exit Sub
                    End If
                    VOL_TITLE = TrimAll(VOL_TITLE)
                    'check unwanted characters
                    c = 0
                    counter8 = 0
                    For iloop = 1 To Len(VOL_TITLE)
                        strcurrentchar = Mid(VOL_TITLE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter8 = 1
                            End If
                        End If
                    Next
                    If counter8 = 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        txt_JHold_VolTitle.Focus()
                        Exit Sub
                    End If
                Else
                    VOL_TITLE = ""
                End If

                'validation for CLASS NO
                Dim CLASS_NO As Object = Nothing
                If txt_JHold_ClassNo.Text <> "" Then
                    CLASS_NO = TrimX(txt_JHold_ClassNo.Text)
                    CLASS_NO = RemoveQuotes(CLASS_NO)
                    If CLASS_NO.Length > 100 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        txt_JHold_ClassNo.Focus()
                        Exit Sub
                    End If

                    CLASS_NO = " " & CLASS_NO & " "
                    If InStr(1, CLASS_NO, "CREATE", 1) > 0 Or InStr(1, CLASS_NO, "DELETE", 1) > 0 Or InStr(1, CLASS_NO, "DROP", 1) > 0 Or InStr(1, CLASS_NO, "INSERT", 1) > 1 Or InStr(1, CLASS_NO, "TRACK", 1) > 1 Or InStr(1, CLASS_NO, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Lbl_Error.Text = "Do not use reserved Words!"
                        txt_JHold_ClassNo.Focus()
                        Exit Sub
                    End If
                    CLASS_NO = TrimX(CLASS_NO)
                    'check unwanted characters
                    c = 0
                    counter9 = 0
                    For iloop = 1 To Len(CLASS_NO)
                        strcurrentchar = Mid(CLASS_NO, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter9 = 1
                            End If
                        End If
                    Next
                    If counter9 = 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        txt_JHold_ClassNo.Focus()
                        Exit Sub
                    End If
                Else
                    CLASS_NO = ""
                End If

                'validation for Book NO
                Dim BOOK_NO As Object = Nothing
                If txt_JHold_BookNo.Text <> "" Then
                    BOOK_NO = TrimAll(UCase(txt_JHold_BookNo.Text))
                    BOOK_NO = RemoveQuotes(BOOK_NO)
                    If BOOK_NO.Length > 50 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        txt_JHold_BookNo.Focus()
                        Exit Sub
                    End If

                    BOOK_NO = " " & BOOK_NO & " "
                    If InStr(1, BOOK_NO, "CREATE", 1) > 0 Or InStr(1, BOOK_NO, "DELETE", 1) > 0 Or InStr(1, BOOK_NO, "DROP", 1) > 0 Or InStr(1, BOOK_NO, "INSERT", 1) > 1 Or InStr(1, BOOK_NO, "TRACK", 1) > 1 Or InStr(1, BOOK_NO, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Do not use reserved Words!"
                        txt_JHold_BookNo.Focus()
                        Exit Sub
                    End If
                    BOOK_NO = TrimAll(BOOK_NO)
                    'check unwanted characters
                    c = 0
                    counter10 = 0
                    For iloop = 1 To Len(BOOK_NO)
                        strcurrentchar = Mid(BOOK_NO, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter10 = 1
                            End If
                        End If
                    Next
                    If counter10 = 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        txt_JHold_BookNo.Focus()
                        Exit Sub
                    End If
                Else
                    BOOK_NO = ""
                End If

                'validation for PAGES
                Dim PAGINATION As Object = Nothing
                If txt_JHold_Pagination.Text <> "" Then
                    PAGINATION = TrimAll(txt_JHold_Pagination.Text)
                    PAGINATION = RemoveQuotes(PAGINATION)
                    If PAGINATION.Length > 50 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        txt_JHold_Pagination.Focus()
                        Exit Sub
                    End If

                    PAGINATION = " " & PAGINATION & " "
                    If InStr(1, PAGINATION, "CREATE", 1) > 0 Or InStr(1, PAGINATION, "DELETE", 1) > 0 Or InStr(1, PAGINATION, "DROP", 1) > 0 Or InStr(1, PAGINATION, "INSERT", 1) > 1 Or InStr(1, PAGINATION, "TRACK", 1) > 1 Or InStr(1, PAGINATION, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Do not use reserved Words!"
                        txt_JHold_Pagination.Focus()
                        Exit Sub
                    End If
                    PAGINATION = TrimAll(PAGINATION)
                    'check unwanted characters
                    c = 0
                    counter11 = 0
                    For iloop = 1 To Len(PAGINATION)
                        strcurrentchar = Mid(PAGINATION, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter11 = 1
                            End If
                        End If
                    Next
                    If counter11 = 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        txt_JHold_Pagination.Focus()
                        Exit Sub
                    End If
                Else
                    PAGINATION = ""
                End If

                'validation for COLLECTION TYPE
                Dim COLLECTION_TYPE As Object = Nothing
                If DDL_CollectionType.Text <> "" Then
                    COLLECTION_TYPE = DDL_CollectionType.SelectedValue
                    COLLECTION_TYPE = RemoveQuotes(COLLECTION_TYPE)
                    If COLLECTION_TYPE.Length > 2 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        DDL_CollectionType.Focus()
                        Exit Sub
                    End If

                    COLLECTION_TYPE = " " & COLLECTION_TYPE & " "
                    If InStr(1, COLLECTION_TYPE, "CREATE", 1) > 0 Or InStr(1, COLLECTION_TYPE, "DELETE", 1) > 0 Or InStr(1, COLLECTION_TYPE, "DROP", 1) > 0 Or InStr(1, COLLECTION_TYPE, "INSERT", 1) > 1 Or InStr(1, COLLECTION_TYPE, "TRACK", 1) > 1 Or InStr(1, COLLECTION_TYPE, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        DDL_CollectionType.Focus()
                        Exit Sub
                    End If
                    COLLECTION_TYPE = TrimX(COLLECTION_TYPE)
                    'check unwanted characters
                    c = 0
                    counter12 = 0
                    For iloop = 1 To Len(COLLECTION_TYPE)
                        strcurrentchar = Mid(COLLECTION_TYPE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter12 = 1
                            End If
                        End If
                    Next
                    If counter12 = 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        DDL_CollectionType.Focus()
                        Exit Sub
                    End If
                Else
                    COLLECTION_TYPE = "C"
                End If

                'validation for STATUS
                Dim STA_CODE As Object = Nothing
                If DDL_Status.Text <> "" Then
                    STA_CODE = DDL_Status.SelectedValue
                    If STA_CODE = "2" Then
                        STA_CODE = "1"
                    End If
                Else
                    STA_CODE = "1"
                End If

                'validation for BINDING TYPE
                Dim BIND_CODE As Object = Nothing
                If DDL_Binding.Text <> "" Then
                    BIND_CODE = DDL_Binding.SelectedValue
                    BIND_CODE = RemoveQuotes(BIND_CODE)
                    If BIND_CODE.Length > 10 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        DDL_Binding.Focus()
                        Exit Sub
                    End If

                    BIND_CODE = " " & BIND_CODE & " "
                    If InStr(1, BIND_CODE, "CREATE", 1) > 0 Or InStr(1, BIND_CODE, "DELETE", 1) > 0 Or InStr(1, BIND_CODE, "DROP", 1) > 0 Or InStr(1, BIND_CODE, "INSERT", 1) > 1 Or InStr(1, BIND_CODE, "TRACK", 1) > 1 Or InStr(1, BIND_CODE, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        DDL_Binding.Focus()
                        Exit Sub
                    End If
                    BIND_CODE = TrimX(BIND_CODE)
                    'check unwanted characters
                    c = 0
                    counter13 = 0
                    For iloop = 1 To Len(BIND_CODE)
                        strcurrentchar = Mid(BIND_CODE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter13 = 1
                            End If
                        End If
                    Next
                    If counter13 = 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        DDL_Binding.Focus()
                        Exit Sub
                    End If
                Else
                    BIND_CODE = "U"
                End If

                'validation for PHYSICAL_LOCATION
                Dim PHYSICAL_LOCATION As Object = Nothing
                If txt_JHold_Location.Text <> "" Then
                    PHYSICAL_LOCATION = TrimAll(txt_JHold_Location.Text)
                    PHYSICAL_LOCATION = RemoveQuotes(PHYSICAL_LOCATION)
                    If PHYSICAL_LOCATION.Length > 50 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        txt_JHold_Location.Focus()
                        Exit Sub
                    End If

                    PHYSICAL_LOCATION = " " & PHYSICAL_LOCATION & " "
                    If InStr(1, PHYSICAL_LOCATION, "CREATE", 1) > 0 Or InStr(1, PHYSICAL_LOCATION, "DELETE", 1) > 0 Or InStr(1, PHYSICAL_LOCATION, "DROP", 1) > 0 Or InStr(1, PHYSICAL_LOCATION, "INSERT", 1) > 1 Or InStr(1, PHYSICAL_LOCATION, "TRACK", 1) > 1 Or InStr(1, PHYSICAL_LOCATION, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        txt_JHold_Location.Focus()
                        Exit Sub
                    End If
                    PHYSICAL_LOCATION = TrimAll(PHYSICAL_LOCATION)
                    'check unwanted characters
                    c = 0
                    counter14 = 0
                    For iloop = 1 To Len(PHYSICAL_LOCATION)
                        strcurrentchar = Mid(PHYSICAL_LOCATION, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter14 = 1
                            End If
                        End If
                    Next
                    If counter14 = 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        txt_JHold_Location.Focus()
                        Exit Sub
                    End If
                Else
                    PHYSICAL_LOCATION = ""
                End If

                'validation for SEC_CODE
                Dim SEC_CODE As Object = Nothing
                If DDL_Section.Text <> "" Then
                    SEC_CODE = Trim(DDL_Section.SelectedValue)
                    SEC_CODE = RemoveQuotes(SEC_CODE)
                    If SEC_CODE.Length > 10 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        DDL_Section.Focus()
                        Exit Sub
                    End If

                    SEC_CODE = " " & SEC_CODE & " "
                    If InStr(1, SEC_CODE, "CREATE", 1) > 0 Or InStr(1, SEC_CODE, "DELETE", 1) > 0 Or InStr(1, SEC_CODE, "DROP", 1) > 0 Or InStr(1, SEC_CODE, "INSERT", 1) > 1 Or InStr(1, SEC_CODE, "TRACK", 1) > 1 Or InStr(1, SEC_CODE, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        DDL_Section.Focus()
                        Exit Sub
                    End If
                    SEC_CODE = Trim(SEC_CODE)
                    'check unwanted characters
                    c = 0
                    counter15 = 0
                    For iloop = 1 To Len(SEC_CODE)
                        strcurrentchar = Mid(SEC_CODE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter15 = 1
                            End If
                        End If
                    Next
                    If counter15 = 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        DDL_Section.Focus()
                        Exit Sub
                    End If
                Else
                    SEC_CODE = ""
                End If

                'validation for REMARKS
                Dim REMARKS As Object = Nothing
                If txt_JHold_Remarks.Text <> "" Then
                    REMARKS = TrimAll(txt_JHold_Remarks.Text)
                    REMARKS = RemoveQuotes(REMARKS)
                    If REMARKS.Length > 250 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        txt_JHold_Remarks.Focus()
                        Exit Sub
                    End If

                    REMARKS = " " & REMARKS & " "
                    If InStr(1, REMARKS, "CREATE", 1) > 0 Or InStr(1, REMARKS, "DELETE", 1) > 0 Or InStr(1, REMARKS, "DROP", 1) > 0 Or InStr(1, REMARKS, "INSERT", 1) > 1 Or InStr(1, REMARKS, "TRACK", 1) > 1 Or InStr(1, REMARKS, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        txt_JHold_Remarks.Focus()
                        Exit Sub
                    End If
                    REMARKS = TrimAll(REMARKS)
                    'check unwanted characters
                    c = 0
                    counter16 = 0
                    For iloop = 1 To Len(REMARKS)
                        strcurrentchar = Mid(REMARKS, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter16 = 1
                            End If
                        End If
                    Next
                    If counter16 = 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        txt_JHold_Remarks.Focus()
                        Exit Sub
                    End If
                Else
                    REMARKS = ""
                End If

                'validation for MISSING_ISSUES
                Dim MISSING_ISSUES As Object = Nothing
                If txt_JHold_MissingIssues.Text <> "" Then
                    MISSING_ISSUES = TrimAll(txt_JHold_MissingIssues.Text)
                    MISSING_ISSUES = RemoveQuotes(MISSING_ISSUES)
                    If MISSING_ISSUES.Length > 100 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        txt_JHold_MissingIssues.Focus()
                        Exit Sub
                    End If

                    MISSING_ISSUES = " " & MISSING_ISSUES & " "
                    If InStr(1, MISSING_ISSUES, "CREATE", 1) > 0 Or InStr(1, MISSING_ISSUES, "DELETE", 1) > 0 Or InStr(1, MISSING_ISSUES, "DROP", 1) > 0 Or InStr(1, MISSING_ISSUES, "INSERT", 1) > 1 Or InStr(1, MISSING_ISSUES, "TRACK", 1) > 1 Or InStr(1, MISSING_ISSUES, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        txt_JHold_MissingIssues.Focus()
                        Exit Sub
                    End If
                    MISSING_ISSUES = TrimAll(MISSING_ISSUES)
                    'check unwanted characters
                    c = 0
                    counter17 = 0
                    For iloop = 1 To Len(MISSING_ISSUES)
                        strcurrentchar = Mid(MISSING_ISSUES, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter17 = 1
                            End If
                        End If
                    Next
                    If counter17 = 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        txt_JHold_MissingIssues.Focus()
                        Exit Sub
                    End If
                Else
                    MISSING_ISSUES = ""
                End If

                'validation for PERIOD
                Dim PERIOD As Object = Nothing
                If txt_JHold_Period.Text <> "" Then
                    PERIOD = TrimAll(txt_JHold_Period.Text)
                    PERIOD = RemoveQuotes(PERIOD)
                    If PERIOD.Length > 100 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        txt_JHold_Period.Focus()
                        Exit Sub
                    End If

                    PERIOD = " " & PERIOD & " "
                    If InStr(1, PERIOD, "CREATE", 1) > 0 Or InStr(1, PERIOD, "DELETE", 1) > 0 Or InStr(1, PERIOD, "DROP", 1) > 0 Or InStr(1, PERIOD, "INSERT", 1) > 1 Or InStr(1, PERIOD, "TRACK", 1) > 1 Or InStr(1, PERIOD, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        txt_JHold_Period.Focus()
                        Exit Sub
                    End If
                    PERIOD = TrimAll(PERIOD)
                    'check unwanted characters
                    c = 0
                    counter18 = 0
                    For iloop = 1 To Len(PERIOD)
                        strcurrentchar = Mid(PERIOD, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter18 = 1
                            End If
                        End If
                    Next
                    If counter18 = 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        txt_JHold_Period.Focus()
                        Exit Sub
                    End If
                Else
                    PERIOD = ""
                End If

                Dim USER_CODE As Object = Nothing
                USER_CODE = Session.Item("LoggedUser")

                Dim DATE_MODIFIED As Object = Nothing
                DATE_MODIFIED = Now.Date
                DATE_MODIFIED = Convert.ToDateTime(DATE_MODIFIED, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                Dim IP As Object = Nothing
                IP = Request.UserHostAddress.Trim
                If Label36.Text <> "" Then
                    If SqlConn.State = 0 Then
                        SqlConn.Open()
                    End If

                    SQL = "SELECT * FROM HOLDINGS WHERE (HOLD_ID='" & Trim(HOLD_ID) & "') AND (LIB_CODE ='" & Trim(LibCode) & "'); "

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

                        If Not String.IsNullOrEmpty(FORMAT_CODE) Then
                            ds.Tables("HOLD").Rows(0)("FORMAT_CODE") = FORMAT_CODE.Trim
                        Else
                            ds.Tables("HOLD").Rows(0)("FORMAT_CODE") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(VOL_NO) Then
                            ds.Tables("HOLD").Rows(0)("VOL_NO") = VOL_NO.Trim
                        Else
                            ds.Tables("HOLD").Rows(0)("VOL_NO") = System.DBNull.Value
                        End If


                        If Not String.IsNullOrEmpty(JYEAR) Then
                            ds.Tables("HOLD").Rows(0)("JYEAR") = JYEAR.ToString.Trim
                        Else
                            ds.Tables("HOLD").Rows(0)("JYEAR") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(PERIOD) Then
                            ds.Tables("HOLD").Rows(0)("PERIOD") = PERIOD.ToString.Trim
                        Else
                            ds.Tables("HOLD").Rows(0)("PERIOD") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(MISSING_ISSUES) Then
                            ds.Tables("HOLD").Rows(0)("MISSING_ISSUES") = MISSING_ISSUES.ToString.Trim
                        Else
                            ds.Tables("HOLD").Rows(0)("MISSING_ISSUES") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(VOL_EDITORS) Then
                            ds.Tables("HOLD").Rows(0)("VOL_EDITORS") = VOL_EDITORS.Trim
                        Else
                            ds.Tables("HOLD").Rows(0)("VOL_EDITORS") = System.DBNull.Value
                        End If
                   
                        If Not String.IsNullOrEmpty(VOL_TITLE) Then
                            ds.Tables("HOLD").Rows(0)("VOL_TITLE") = VOL_TITLE.Trim
                        Else
                            ds.Tables("HOLD").Rows(0)("VOL_TITLE") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(CLASS_NO) Then
                            ds.Tables("HOLD").Rows(0)("CLASS_NO") = CLASS_NO.Trim
                        Else
                            ds.Tables("HOLD").Rows(0)("CLASS_NO") = System.DBNull.Value
                        End If
                    
                        If Not String.IsNullOrEmpty(BOOK_NO) Then
                            ds.Tables("HOLD").Rows(0)("BOOK_NO") = BOOK_NO.Trim
                        Else
                            ds.Tables("HOLD").Rows(0)("BOOK_NO") = System.DBNull.Value
                        End If
                  
                        If Not String.IsNullOrEmpty(PAGINATION) Then
                            ds.Tables("HOLD").Rows(0)("PAGINATION") = PAGINATION.Trim
                        Else
                            ds.Tables("HOLD").Rows(0)("PAGINATION") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(COLLECTION_TYPE) Then
                            ds.Tables("HOLD").Rows(0)("COLLECTION_TYPE") = COLLECTION_TYPE.Trim
                        Else
                            ds.Tables("HOLD").Rows(0)("COLLECTION_TYPE") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(STA_CODE) Then
                            ds.Tables("HOLD").Rows(0)("STA_CODE") = STA_CODE.Trim
                        Else
                            ds.Tables("HOLD").Rows(0)("STA_CODE") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(BIND_CODE) Then
                            ds.Tables("HOLD").Rows(0)("BIND_CODE") = BIND_CODE.Trim
                        Else
                            ds.Tables("HOLD").Rows(0)("BIND_CODE") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(SEC_CODE) Then
                            ds.Tables("HOLD").Rows(0)("SEC_CODE") = SEC_CODE.Trim
                        Else
                            ds.Tables("HOLD").Rows(0)("SEC_CODE") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(REMARKS) Then
                            ds.Tables("HOLD").Rows(0)("REMARKS") = REMARKS.Trim
                        Else
                            ds.Tables("HOLD").Rows(0)("REMARKS") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(PHYSICAL_LOCATION) Then
                            ds.Tables("HOLD").Rows(0)("PHYSICAL_LOCATION") = PHYSICAL_LOCATION.Trim
                        Else
                            ds.Tables("HOLD").Rows(0)("PHYSICAL_LOCATION") = System.DBNull.Value
                        End If

                        ds.Tables("HOLD").Rows(0)("UPDATED_BY") = UserCode.Trim
                        ds.Tables("HOLD").Rows(0)("DATE_MODIFIED") = Now.Date
                        ds.Tables("HOLD").Rows(0)("IP") = Request.UserHostAddress.Trim

                        thisTransaction = SqlConn.BeginTransaction()
                        da.SelectCommand.Transaction = thisTransaction
                        da.Update(ds, "HOLD")
                        thisTransaction.Commit()
                        SqlConn.Close()
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Record Added Successfully!');", True)
                        ClearFields()
                        Label36.Text = ""
                        JHold_Save_Bttn.Visible = True
                        JHold_Update_Bttn.Visible = False
                        JHold_Delete_Bttn.Visible = False
                    Else
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Record Update  - Please Contact System Administrator... ');", True)
                        Lbl_Error.Text = "Record Not Updated! "
                    End If
                End If
                PopulateLooseIssuesGrid()
            Catch q As SqlException
                thisTransaction.Rollback()
                Lbl_Error.Text = "Database Error -UPDATE: " & (q.Message() & ", " & q.Number & ", " & q.GetType.ToString)
            Catch ex As Exception
                Lbl_Error.Text = "Error-UPDATE: " & (ex.Message())
            Finally
                SqlConn.Close()
            End Try
        End If
    End Sub
    'Delete Selecte Holdings Records
    Protected Sub JHold_Delete_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles JHold_Delete_Bttn.Click
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing
        Dim dt As DataTable = Nothing
        Try
            Dim counter1 As Integer = Nothing
            Dim iloop As Integer
            Dim strcurrentchar As Object
            Dim c As Integer

            'validation for HOLD_ID
            Dim HOLD_ID As Long = Nothing
            If Me.Label36.Text <> "" Then
                HOLD_ID = Convert.ToInt16(Label36.Text)
                HOLD_ID = RemoveQuotes(HOLD_ID)
                If Not IsNumeric(HOLD_ID) = True Then
                    Lbl_Error.Text = "Error:Select Title from Drop-Down!"
                    DDL_Titles.Focus()
                    Exit Sub
                End If
                If Len(HOLD_ID) > 10 Then
                    Lbl_Error.Text = "Error: Input is not Valid !"
                    DDL_Titles.Focus()
                    Exit Sub
                End If
                HOLD_ID = " " & HOLD_ID & " "
                If InStr(1, HOLD_ID, "CREATE", 1) > 0 Or InStr(1, HOLD_ID, "DELETE", 1) > 0 Or InStr(1, HOLD_ID, "DROP", 1) > 0 Or InStr(1, HOLD_ID, "INSERT", 1) > 1 Or InStr(1, HOLD_ID, "TRACK", 1) > 1 Or InStr(1, HOLD_ID, "TRACE", 1) > 1 Then
                    Lbl_Error.Text = "Error: Input is not Valid !"
                    Exit Sub
                End If
                HOLD_ID = TrimX(HOLD_ID)
                'check unwanted characters
                c = 0
                counter1 = 0
                For iloop = 1 To Len(HOLD_ID.ToString)
                    strcurrentchar = Mid(HOLD_ID, iloop, 1)
                    If c = 0 Then
                        If Not InStr("'<>=%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter1 = 1
                        End If
                    End If
                Next
                If counter1 = 1 Then
                    Lbl_Error.Text = "Error: Input is not Valid !"
                    DDL_Titles.Focus()
                    Exit Sub
                End If
            Else
                Lbl_Error.Text = "Error: Select Title from Drop-Down!"
                DDL_Titles.Focus()
                Exit Sub
            End If

            Dim str As Object = Nothing
            str = "SELECT HOLD_ID, CAT_NO, STA_CODE FROM HOLDINGS WHERE (HOLD_ID = '" & Trim(HOLD_ID) & "')  AND (LIB_CODE = '" & Trim(LibCode) & "')"
            Dim cmd1 As New SqlCommand(str, SqlConn)
            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(str, SqlConn)
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            da.Fill(ds)

            dt = ds.Tables(0).Copy
            SqlConn.Close()

            Dim CAT_NO As Long = Nothing
            If dt.Rows.Count <> 0 Then
                If dt.Rows(0).Item("CAT_NO").ToString <> "" Then
                    CAT_NO = dt.Rows(0).Item("CAT_NO").ToString
                Else
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('There is no record to delete!');", True)
                    Exit Sub
                End If

                If dt.Rows(0).Item("STA_CODE").ToString <> "" Then
                    If dt.Rows(0).Item("STA_CODE").ToString = "2" Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('The document is Issued, can not be deleted!');", True)
                        Exit Sub
                    End If
                End If
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('There is no record to delete!');", True)
                Exit Sub
            End If

            dt = Nothing

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
            objCommand.Parameters("@HOLD_ID").Value = HOLD_ID

            objCommand.ExecuteNonQuery()

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
            ClearFields()
            Label36.Text = ""
            JHold_Save_Bttn.Visible = True
            JHold_Update_Bttn.Visible = False
            JHold_Delete_Bttn.Visible = False
            PopulateLooseIssuesGrid()
        Catch q As SqlException
            thisTransaction.Rollback()
            Lbl_Error.Text = "Database Error -UPDATE: " & (q.Message() & ", " & q.Number & ", " & q.GetType.ToString)
        Catch ex As Exception
            Lbl_Error.Text = "Error-UPDATE: " & (ex.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'delete all selected records from grid
    Protected Sub JHold_DeleteAll_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles JHold_DeleteAll_Bttn.Click
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing
        Dim dt As DataTable = Nothing
        Try
            For Each row As GridViewRow In Grid1.Rows
                Dim cb As CheckBox = DirectCast(row.FindControl("cbd"), CheckBox)
                If cb IsNot Nothing AndAlso cb.Checked = True Then
                    Dim HOLD_ID As Integer = Convert.ToInt32(Grid1.DataKeys(row.RowIndex).Value)

                    Dim counter1 As Integer = Nothing
                    Dim iloop As Integer
                    Dim strcurrentchar As Object
                    Dim c As Integer

                    'validation for HOLD_ID
                    HOLD_ID = RemoveQuotes(HOLD_ID)
                    If Not IsNumeric(HOLD_ID) = True Then
                        Lbl_Error.Text = "Error:Select Title from Drop-Down!"
                        DDL_Titles.Focus()
                        Continue For
                    End If
                    If Len(HOLD_ID) > 10 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        DDL_Titles.Focus()
                        Continue For
                    End If
                    HOLD_ID = " " & HOLD_ID & " "
                    If InStr(1, HOLD_ID, "CREATE", 1) > 0 Or InStr(1, HOLD_ID, "DELETE", 1) > 0 Or InStr(1, HOLD_ID, "DROP", 1) > 0 Or InStr(1, HOLD_ID, "INSERT", 1) > 1 Or InStr(1, HOLD_ID, "TRACK", 1) > 1 Or InStr(1, HOLD_ID, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        Continue For
                    End If
                    HOLD_ID = TrimX(HOLD_ID)
                    'check unwanted characters
                    c = 0
                    counter1 = 0
                    For iloop = 1 To Len(HOLD_ID.ToString)
                        strcurrentchar = Mid(HOLD_ID, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter1 = 1
                            End If
                        End If
                    Next
                    If counter1 = 1 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        DDL_Titles.Focus()
                        Continue For
                    End If

                Dim str As Object = Nothing
                str = "SELECT HOLD_ID, CAT_NO, STA_CODE FROM HOLDINGS WHERE (HOLD_ID = '" & Trim(HOLD_ID) & "')  AND (LIB_CODE = '" & Trim(LibCode) & "')"
                Dim cmd1 As New SqlCommand(str, SqlConn)
                Dim ds As New DataSet
                Dim da As New SqlDataAdapter(str, SqlConn)
                If SqlConn.State = 0 Then
                    SqlConn.Open()
                End If
                da.Fill(ds)

                dt = ds.Tables(0).Copy
                SqlConn.Close()

                Dim CAT_NO As Long = Nothing
                If dt.Rows.Count <> 0 Then
                    If dt.Rows(0).Item("CAT_NO").ToString <> "" Then
                        CAT_NO = dt.Rows(0).Item("CAT_NO").ToString
                    Else
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('There is no record to delete!');", True)
                            Continue For
                    End If

                    If dt.Rows(0).Item("STA_CODE").ToString <> "" Then
                        If dt.Rows(0).Item("STA_CODE").ToString = "2" Then
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('The document is Issued, can not be deleted!');", True)
                                Continue For
                        End If
                    End If
                Else
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('There is no record to delete!');", True)
                        Continue For
                End If

                dt = Nothing

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
                    objCommand.Parameters("@HOLD_ID").Value = HOLD_ID

                    objCommand.ExecuteNonQuery()

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
                    PopulateLooseIssuesGrid()
                End If
            Next
        Catch q As SqlException
            thisTransaction.Rollback()
            Lbl_Error.Text = "Database Error -UPDATE: " & (q.Message() & ", " & q.Number & ", " & q.GetType.ToString)
        Catch ex As Exception
            Lbl_Error.Text = "Error-UPDATE: " & (ex.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
End Class