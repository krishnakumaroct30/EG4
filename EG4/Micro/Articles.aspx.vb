﻿Imports System.Data
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
Imports System.Drawing
Imports System.Drawing.Bitmap
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports EG4.PopulateCombo
Public Class Articles
    Inherits System.Web.UI.Page
    Dim index As Integer = 0
    Public Srno As Integer = 0
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session.Item("IsThisU") <> Nothing Then
                If Not SConnection() = True Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Database Connection is Lost!');", True)
                Else
                    If Page.IsPostBack = False Then
                        Me.Art_Save_Bttn.Visible = True
                        Me.Art_Update_Bttn.Visible = False
                        RadioButton1.Checked = True
                        CreateRootFolder()
                        SearchTitle()
                        PopulateSubjects()
                    End If
                End If
                Dim CreateSUserButton As Object 'System.Web.UI.WebControls.Button
                CreateSUserButton = Master.FindControl("Accordion1").FindControl("ArtPane").FindControl("Art_Indexing_Bttn")
                CreateSUserButton.ForeColor = Drawing.Color.Red
                'paneSelectedIndex = 1
                myPaneName = "ArtPane"

            Else
                Response.Redirect("~/Default.aspx", False)
            End If
            LibCode = Session.Item("LoggedLibcode")
        Catch ex As Exception
            Lbl_Error.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    'create LIBCODE as photo folder if not exist
    Public Sub CreateRootFolder() 'Libcode
        Try
            Dim pathToCreate As String = "~/DFILES/" & Trim(LibCode) & "/Articles"
            ' folder exist message
            If Not Directory.Exists(Server.MapPath(pathToCreate)) Then
                Directory.CreateDirectory(Server.MapPath(pathToCreate))
            End If
        Catch ex As Exception
            Label4.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    Public Sub SearchTitle()
        Dim dtSearch As DataTable = Nothing
        Try
            Dim SQL As String = Nothing
            If RadioButton1.Checked = True Then
                SQL = "SELECT CAT_NO, TITLE  FROM CATS WHERE (CAT_NO IS NOT NULL) AND (BIB_CODE ='S') AND (MAT_CODE ='P') ORDER BY TITLE"
            Else
                SQL = "SELECT CAT_NO, TITLE  FROM CATS WHERE (CAT_NO IS NOT NULL) AND (BIB_CODE ='M') ORDER BY TITLE"
            End If
           
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
                Label24.Text = "Total Record(s): 0 "
            Else
                RecordCount = dtSearch.Rows.Count
                Me.DDL_Titles.DataSource = dtSearch
                Me.DDL_Titles.DataTextField = "TITLE"
                Me.DDL_Titles.DataValueField = "CAT_NO"
                Me.DDL_Titles.DataBind()
                DDL_Titles.Items.Insert(0, "")
                Label24.Text = "Total Record(s): " & RecordCount
                DDL_Titles.Focus()
            End If
            ViewState("dt") = dtSearch
        Catch s As Exception
            Lbl_Error.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'select serials
    Protected Sub RadioButton1_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles RadioButton1.CheckedChanged
        SearchTitle()
    End Sub
    'select books
    Protected Sub RadioButton2_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles RadioButton2.CheckedChanged
        SearchTitle()
    End Sub
    'Search articles
    Public Sub Search_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Search_Bttn.Click
        Dim dtSearch As DataTable = Nothing
        Try
            Dim c, counter1, counter2, counter3, counter4, counter5 As Integer
            Dim iloop As Integer
            Dim strcurrentchar As Object

            'search string validation
            Dim mySearchString As Object = Nothing
            If txt_Search.Text <> "" Then
                mySearchString = TrimAll(txt_Search.Text)
                mySearchString = RemoveQuotes(mySearchString)
                If mySearchString.Length > 250 Then
                    Lbl_Error.Text = "Error:  Input is not Valid!"
                    Me.txt_Search.Focus()
                    Exit Sub
                End If
                mySearchString = " " & mySearchString & " "
                If InStr(1, mySearchString, "CREATE", 1) > 0 Or InStr(1, mySearchString, "DELETE", 1) > 0 Or InStr(1, mySearchString, "DROP", 1) > 0 Or InStr(1, mySearchString, "INSERT", 1) > 1 Or InStr(1, mySearchString, "TRACK", 1) > 1 Or InStr(1, mySearchString, "TRACE", 1) > 1 Then
                    Lbl_Error.Text = "Error:  Input is not Valid !"
                    Me.txt_Search.Focus()
                    Exit Sub
                End If
                mySearchString = TrimAll(mySearchString)
                'check unwanted characters
                c = 0
                counter1 = 0
                For iloop = 1 To Len(mySearchString)
                    strcurrentchar = Mid(mySearchString, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter1 = 1
                        End If
                    End If
                Next
                If counter1 = 1 Then
                    Lbl_Error.Text = "Error: data is not Valid !"
                    Me.txt_Search.Focus()
                    Exit Sub
                End If
            Else
                mySearchString = String.Empty
            End If

            'Field Name validation
            Dim myfield As String = Nothing
            If DropDownList1.Text <> "" Then
                myfield = TrimAll(DropDownList1.SelectedValue)
                myfield = RemoveQuotes(myfield)
                If myfield.Length > 50 Then
                    Lbl_Error.Text = "Error: Input is not Valid !"
                    Me.DropDownList1.Focus()
                    Exit Sub
                End If
                myfield = " " & myfield & " "
                If InStr(1, myfield, "CREATE", 1) > 0 Or InStr(1, myfield, "DELETE", 1) > 0 Or InStr(1, myfield, "DROP", 1) > 0 Or InStr(1, myfield, "INSERT", 1) > 1 Or InStr(1, myfield, "TRACK", 1) > 1 Or InStr(1, myfield, "TRACE", 1) > 1 Then
                    Lbl_Error.Text = "Error: Input is not Valid !"
                    Me.DropDownList1.Focus()
                    Exit Sub
                End If
                myfield = TrimAll(myfield)
                'check unwanted characters
                c = 0
                counter2 = 0
                For iloop = 1 To Len(myfield)
                    strcurrentchar = Mid(myfield, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter2 = 1
                        End If
                    End If
                Next
                If counter2 = 1 Then
                    Lbl_Error.Text = "Error: Input is not Valid !"
                    DropDownList1.Focus()
                    Exit Sub
                End If
            Else
                myfield = "ART_TITLE"
            End If


            'Boolean Operator validation
            Dim myBoolean As String = Nothing
            If DropDownList2.Text <> "" Then
                myBoolean = TrimAll(DropDownList2.SelectedValue)
                myBoolean = RemoveQuotes(myBoolean)
                If myBoolean.Length > 20 Then
                    Lbl_Error.Text = "Error: Input is not Valid !"
                    Me.DropDownList2.Focus()
                    Exit Sub
                End If
                myBoolean = " " & myBoolean & " "
                If InStr(1, myBoolean, "CREATE", 1) > 0 Or InStr(1, myBoolean, "DELETE", 1) > 0 Or InStr(1, myBoolean, "DROP", 1) > 0 Or InStr(1, myBoolean, "INSERT", 1) > 1 Or InStr(1, myBoolean, "TRACK", 1) > 1 Or InStr(1, myBoolean, "TRACE", 1) > 1 Then
                    Lbl_Error.Text = "Error: Input is not Valid !"
                    Me.DropDownList2.Focus()
                    Exit Sub
                End If
                myBoolean = TrimAll(myBoolean)
                'check unwanted characters
                c = 0
                counter3 = 0
                For iloop = 1 To Len(myBoolean)
                    strcurrentchar = Mid(myBoolean, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter3 = 1
                        End If
                    End If
                Next
                If counter3 = 1 Then
                    Lbl_Error.Text = "Error: Input is not Valid !"
                    DropDownList2.Focus()
                    Exit Sub
                End If
            Else
                myBoolean = "AND"
            End If

            'Order by validation
            Dim myOrderBy As String = Nothing
            If DropDownList3.Text <> "" Then
                myOrderBy = TrimAll(DropDownList3.SelectedValue)
                myOrderBy = RemoveQuotes(myOrderBy)
                If myOrderBy.Length > 20 Then
                    Lbl_Error.Text = "Error: Input is not Valid !"
                    Me.DropDownList3.Focus()
                    Exit Sub
                End If
                myOrderBy = " " & myOrderBy & " "
                If InStr(1, myOrderBy, "CREATE", 1) > 0 Or InStr(1, myOrderBy, "DELETE", 1) > 0 Or InStr(1, myOrderBy, "DROP", 1) > 0 Or InStr(1, myOrderBy, "INSERT", 1) > 1 Or InStr(1, myOrderBy, "TRACK", 1) > 1 Or InStr(1, myOrderBy, "TRACE", 1) > 1 Then
                    Lbl_Error.Text = "Error: Input is not Valid !"
                    Me.DropDownList3.Focus()
                    Exit Sub
                End If
                myOrderBy = TrimAll(myOrderBy)
                'check unwanted characters
                c = 0
                counter4 = 0
                For iloop = 1 To Len(myOrderBy)
                    strcurrentchar = Mid(myOrderBy, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter4 = 1
                        End If
                    End If
                Next
                If counter4 = 1 Then
                    Lbl_Error.Text = "Error: Input is not Valid !"
                    DropDownList3.Focus()
                    Exit Sub
                End If
            Else
                myOrderBy = "ART_TITLE"
            End If

            'Sort by validation
            Dim mySortBy As String = Nothing
            If DropDownList4.Text <> "" Then
                mySortBy = TrimAll(DropDownList4.SelectedValue)
                mySortBy = RemoveQuotes(mySortBy)
                If mySortBy.Length > 5 Then
                    Lbl_Error.Text = "Error: Input is not Valid !"
                    Me.DropDownList4.Focus()
                    Exit Sub
                End If
                mySortBy = " " & mySortBy & " "
                If InStr(1, mySortBy, "CREATE", 1) > 0 Or InStr(1, mySortBy, "DELETE", 1) > 0 Or InStr(1, mySortBy, "DROP", 1) > 0 Or InStr(1, mySortBy, "INSERT", 1) > 1 Or InStr(1, mySortBy, "TRACK", 1) > 1 Or InStr(1, mySortBy, "TRACE", 1) > 1 Then
                    Lbl_Error.Text = "Error: Input is not Valid !"
                    Me.DropDownList4.Focus()
                    Exit Sub
                End If
                mySortBy = TrimAll(mySortBy)
                'check unwanted characters
                c = 0
                counter5 = 0
                For iloop = 1 To Len(mySortBy)
                    strcurrentchar = Mid(mySortBy, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter5 = 1
                        End If
                    End If
                Next
                If counter5 = 1 Then
                    Lbl_Error.Text = "Error: Input is not Valid !"
                    DropDownList4.Focus()
                    Exit Sub
                End If
            Else
                mySortBy = "ASC"
            End If

            Dim ART_TYPE As Object = Nothing
            If DropDownList5.Text <> "" Then
                ART_TYPE = DropDownList5.SelectedValue
            Else
                ART_TYPE = "ALL"
            End If

            '**********************************************************************************
            Dim SQL As String = Nothing
            SQL = "SELECT ART_NO, ART_TITLE, TITLE, VOL, ISSUE, PERIOD, ART_YEAR, AUTHORS FROM ARTICLES_VIEW WHERE (ART_NO IS NOT NULL) "

            If txt_Search.Text <> "" Then
                If myBoolean = "LIKE" Then
                    SQL = SQL & " AND (" & myfield & " LIKE N'%" & Trim(mySearchString) & "%') "
                End If
                If myBoolean = "SW" Then
                    SQL = SQL & " AND (" & myfield & " LIKE N'" & Trim(mySearchString) & "%') "
                End If
                If myBoolean = "EW" Then
                    SQL = SQL & " AND (" & myfield & " LIKE N'%" & Trim(mySearchString) & "') "
                End If
                If myBoolean = "AND" Then
                    Dim h As Integer
                    Dim myNewSearchString As Object
                    myNewSearchString = Split(mySearchString, " ")
                    SQL = SQL & " AND (" & myfield & " LIKE N'%" & Trim(myNewSearchString(0)) & "%' "
                    For h = 1 To UBound(myNewSearchString)
                        SQL = SQL & " AND " & myfield & " LIKE N'%" & Trim(myNewSearchString(h)) & "%'"
                    Next
                    SQL = SQL & ")"
                End If
                If myBoolean = "OR" Then
                    Dim h As Integer
                    Dim myNewSearchString As Object
                    myNewSearchString = Split(mySearchString, " ")
                    SQL = SQL & " AND (" & myfield & " LIKE N'%" & Trim(myNewSearchString(0)) & "%' "
                    For h = 1 To UBound(myNewSearchString)
                        SQL = SQL & " OR " & myfield & " LIKE N'%" & Trim(myNewSearchString(h)) & "%' "
                    Next
                    SQL = SQL & ")"
                End If
            End If

            If ART_TYPE = "ALL" Then

            Else
                SQL = SQL & " AND (ART_TYPE = '" & Trim(ART_TYPE) & "')"
            End If

            SQL = SQL & " ORDER BY " & myOrderBy & " " & mySortBy & " "

            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            da.Fill(ds)

            dtSearch = ds.Tables(0).Copy
            Dim RecordCount As Long = 0
            If dtSearch.Rows.Count = 0 Then
                Me.Grid1_Search.DataSource = Nothing
                Grid1_Search.DataBind()
                Label1.Text = "Total Record(s): 0 "
                Delete_Bttn.Visible = False
                Delete_Bttn.Enabled = False
            Else
                Grid1_Search.Visible = True
                RecordCount = dtSearch.Rows.Count
                Grid1_Search.DataSource = dtSearch
                Grid1_Search.DataBind()
                Label1.Text = "Total Record(s): " & RecordCount
                Delete_Bttn.Visible = True
                Delete_Bttn.Enabled = True
            End If
            ViewState("dt") = dtSearch
            UpdatePanel1.Update()
        Catch s As Exception
            Lbl_Error.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'grid view page index changing event
    Protected Sub Grid1_Search_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles Grid1_Search.PageIndexChanging
        Try
            'rebind datagrid
            Grid1_Search.DataSource = ViewState("dt") 'temp
            Grid1_Search.PageIndex = e.NewPageIndex
            index = e.NewPageIndex * Grid1_Search.PageSize
            Grid1_Search.DataBind()
        Catch s As Exception
            Lbl_Error.Text = "Error:  there is error in page index !"
        End Try
    End Sub
    'gridview sorting event
    Protected Sub Grid1_Search_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles Grid1_Search.Sorting
        Dim temp As DataTable = CType(ViewState("dt"), DataTable)
        GridViewSortExpression = e.SortExpression
        Grid1_Search.DataSource = temp
        Dim pageIndex As Integer = Grid1_Search.PageIndex
        Grid1_Search.DataSource = SortDataTable(Grid1_Search.DataSource, False)
        Grid1_Search.DataBind()
        Grid1_Search.PageIndex = pageIndex
    End Sub
    'get value of row from grid
    Private Sub Grid1_Search_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles Grid1_Search.RowCommand
        Dim Command As SqlCommand = Nothing
        Dim dr As SqlDataReader = Nothing
        Try
            If e.CommandName = "Select" Then
                Dim myRowID, ART_NO As Integer
                myRowID = e.CommandArgument.ToString()
                ART_NO = Grid1_Search.DataKeys(myRowID).Value
                If Not String.IsNullOrEmpty(ART_NO) And ART_NO <> 0 Then
                    Label26.Text = ART_NO
                    Dim iloop As Integer
                    Dim strcurrentchar As Object
                    Dim c As Integer
                    Dim counter1 As Integer

                    ART_NO = TrimX(ART_NO)
                    ART_NO = RemoveQuotes(ART_NO)

                    If Len(ART_NO).ToString > 10 Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Length of Input is not Proper!');", True)
                        Exit Sub
                    End If

                    If Not IsNumeric(ART_NO.ToString) = True Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Input type is not Proper!');", True)
                        Exit Sub
                    End If

                    ART_NO = " " & ART_NO & " "
                    If InStr(1, ART_NO, " CREATE ", 1) > 0 Or InStr(1, ART_NO, " DELETE ", 1) > 0 Or InStr(1, ART_NO, " DROP ", 1) > 0 Or InStr(1, ART_NO, " INSERT ", 1) > 1 Or InStr(1, ART_NO, " TRACK ", 1) > 1 Or InStr(1, ART_NO, " TRACE ", 1) > 1 Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Do Not Use Reserve Words!');", True)
                        Exit Sub
                    End If
                    ART_NO = TrimX(ART_NO)

                    ClearFields()
                    'get record details from database
                    Dim SQL As String = Nothing
                    SQL = " SELECT *  FROM ARTICLES WHERE (ART_NO = '" & Trim(ART_NO) & "') "
                    Command = New SqlCommand(SQL, SqlConn)
                    SqlConn.Open()
                    dr = Command.ExecuteReader(CommandBehavior.CloseConnection)
                    dr.Read()
                    If dr.HasRows = True Then
                        DDL_Titles.ClearSelection()
                        If dr.Item("TITLE").ToString <> "" Then
                            txt_Art_Title.Text = dr.Item("TITLE").ToString
                        Else
                            txt_Art_Title.Text = ""
                        End If

                        If dr.Item("SUB_TITLE").ToString <> "" Then
                            txt_Art_SubTitle.Text = dr.Item("SUB_TITLE").ToString
                        Else
                            txt_Art_SubTitle.Text = ""
                        End If

                        If dr.Item("AUTHORS").ToString <> "" Then
                            txt_Art_Authors.Text = dr.Item("AUTHORS").ToString
                        Else
                            txt_Art_Authors.Text = ""
                        End If

                        If dr.Item("ABSTRACT").ToString <> "" Then
                            txt_Art_Abstract.Text = dr.Item("ABSTRACT").ToString
                        Else
                            txt_Art_Abstract.Text = ""
                        End If

                        If dr.Item("VOL").ToString <> "" Then
                            txt_Art_VolNo.Text = dr.Item("VOL").ToString
                        Else
                            txt_Art_VolNo.Text = ""
                        End If

                        If dr.Item("ISSUE").ToString <> "" Then
                            txt_Art_IssueNo.Text = dr.Item("ISSUE").ToString
                        Else
                            txt_Art_IssueNo.Text = ""
                        End If

                        If dr.Item("PERIOD").ToString <> "" Then
                            txt_Art_Period.Text = dr.Item("PERIOD").ToString
                        Else
                            txt_Art_Period.Text = ""
                        End If

                        If dr.Item("ART_YEAR").ToString <> "" Then
                            txt_Art_Year.Text = dr.Item("ART_YEAR").ToString
                        Else
                            txt_Art_Year.Text = ""
                        End If

                        If dr.Item("PAGE").ToString <> "" Then
                            txt_Art_Page.Text = dr.Item("PAGE").ToString
                        Else
                            txt_Art_Page.Text = ""
                        End If

                        If dr.Item("URL").ToString <> "" Then
                            txt_Art_URL.Text = dr.Item("URL").ToString
                        Else
                            txt_Art_URL.Text = ""
                        End If

                        If dr.Item("KEYWORDS").ToString <> "" Then
                            txt_Art_Keywords.Text = dr.Item("KEYWORDS").ToString
                        Else
                            txt_Art_Keywords.Text = ""
                        End If

                        If dr.Item("SUB_ID").ToString <> "" Then
                            DDL_Subjects.SelectedValue = dr.Item("SUB_ID").ToString
                        Else
                            DDL_Subjects.ClearSelection()
                        End If

                        If dr.Item("REMARKS").ToString <> "" Then
                            txt_Art_Remarks.Text = dr.Item("REMARKS").ToString
                        Else
                            txt_Art_Remarks.Text = ""
                        End If

                        If dr.Item("ART_TYPE").ToString <> "" Then
                            DDL_Type.SelectedValue = dr.Item("ART_TYPE").ToString
                        Else
                            DDL_Type.ClearSelection()
                        End If

                        If dr.Item("CHAP_NO").ToString <> "" Then
                            txt_Art_ChapterNo.Text = dr.Item("CHAP_NO").ToString
                        Else
                            txt_Art_ChapterNo.Text = ""
                        End If

                        Dim FilePath As String
                        FilePath = "~/DFILES/" & Session.Item("LoggedLibcode") & "/Articles/" & ART_NO & "/"

                        FilePath = Replace(FilePath, "\", "/")
                        FilePath = Replace(FilePath, "//", "/")
                        If System.IO.Directory.Exists(Server.MapPath(FilePath)) = True Then
                            Dim myDir As DirectoryInfo = New DirectoryInfo(Server.MapPath(FilePath))

                            If (myDir.EnumerateFiles().Any()) Then
                                Dim files() As String = IO.Directory.GetFiles(Server.MapPath(FilePath))
                                For Each sFile As String In files
                                    Dim FileName As String = System.IO.Path.GetFileName(sFile) ' Text &= IO.File.ReadAllText(sFile)
                                    Dim strURL As String = "~/DFILES/" & Session.Item("LoggedLibcode") & "/Articles/" & ART_NO & "/" & FileName '"AM51357293_2014-03-10_16-48-22.pdf"
                                    HyperLink1.Visible = True
                                    HyperLink1.NavigateUrl = strURL
                                    HyperLink1.Target = "_new"
                                    CheckBox3.Visible = True
                                    Label27.Text = Trim(FileName)
                                Next
                            Else
                                HyperLink1.Visible = False
                                CheckBox3.Visible = False
                            End If
                        Else
                            HyperLink1.Visible = False
                            CheckBox3.Visible = False
                        End If

                        Art_Save_Bttn.Visible = False
                        Art_Update_Bttn.Visible = True
                        Art_Delete_Bttn.Visible = True
                        dr.Close()
                    Else
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Record No Selected for Edit!');", True)
                    End If
                Else
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Record No Selected for Edit!');", True)
                End If
            End If
            txt_Art_Title.Focus()
        Catch s As Exception
            Lbl_Error.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'load / display fields
    Protected Sub DDL_Titles_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDL_Titles.SelectedIndexChanged
        Dim dt As New DataTable
        Try
            Dim myCatNo As Integer = Nothing
            If DDL_Titles.Text <> "" Then
                myCatNo = DDL_Titles.SelectedValue

                Dim SQL As String = Nothing
                If myCatNo <> 0 Then
                    SQL = "SELECT CATS.CAT_NO, CATS.TITLE, CATS.SUB_TITLE, CATS.EDITOR,  CATS.AUTHOR1,  CATS.AUTHOR2,  CATS.AUTHOR3, CATS.CORPORATE_AUTHOR, CATS.PHOTO, PUBLISHERS.PUB_NAME, CATS.PLACE_OF_PUB FROM CATS  LEFT OUTER JOIN PUBLISHERS ON CATS.PUB_ID = PUBLISHERS.PUB_ID WHERE (CATS.CAT_NO = '" & Trim(myCatNo) & "') "
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

                    'authors
                    If dt.Rows(0).Item("AUTHOR1").ToString <> "" Then
                        If dt.Rows(0).Item("AUTHOR2").ToString <> "" Then
                            If dt.Rows(0).Item("AUTHOR3").ToString <> "" Then
                                Label3.Text = dt.Rows(0).Item("AUTHOR1").ToString & "; " & dt.Rows(0).Item("AUTHOR2").ToString & " and  " & dt.Rows(0).Item("AUTHOR3").ToString
                            Else
                                Label3.Text = dt.Rows(0).Item("AUTHOR1").ToString & " and " & dt.Rows(0).Item("AUTHOR2").ToString
                            End If
                        Else
                            Label3.Text = dt.Rows(0).Item("AUTHOR1").ToString
                        End If
                    Else
                        Label3.Text = ""
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
            PopulateArticlesGrid()
        Catch ex As Exception
            Lbl_Error.Text = "Error: " & (ex.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    Public Sub PopulateSubjects()
        Me.DDL_Subjects.DataSource = GetSubjectList()
        Me.DDL_Subjects.DataTextField = "SUB_NAME"
        Me.DDL_Subjects.DataValueField = "SUB_ID"
        Me.DDL_Subjects.DataBind()
        DDL_Subjects.Items.Insert(0, "")
    End Sub
    'save record    
    Protected Sub Art_Save_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Art_Save_Bttn.Click
        If IsPostBack = True Then
            'check unwanted characters
            Dim iloop As Integer
            Dim strcurrentchar As Object
            Dim c As Integer
            Dim counter1, counter2, counter3, counter4, counter5, counter6, counter7, counter8, counter9, counter10, counter11, counter12, counter13, counter14, counter15, counter16, counter17, counter18, counter19 As Integer
            Dim counter20, counter21, counter22, counter23, counter24, counter25, counter26, counter27, counter28, counter29, counter30, counter31, counter32, counter33, counter34, counter35, counter36, counter37, counter38, counter39 As Integer
            Dim thisTransaction As SqlClient.SqlTransaction = Nothing
            Try
                If DDL_Titles.Text = "" Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Plz Select the Title from Drop-Downn!');", True)
                    DDL_Titles.Focus()
                    Exit Sub
                End If

                'validation for cat_no
                Dim CAT_NO As Integer = Nothing
                If Me.DDL_Titles.Text <> "" Then
                    CAT_NO = Trim(DDL_Titles.SelectedValue)
                    CAT_NO = RemoveQuotes(CAT_NO)

                    If Not IsNumeric(CAT_NO.ToString) Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        DDL_Titles.Focus()
                        Exit Sub
                    End If

                    If CAT_NO.ToString.Length > 5 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        DDL_Titles.Focus()
                        Exit Sub
                    End If
                    CAT_NO = " " & CAT_NO & " "
                    If InStr(1, CAT_NO, "CREATE", 1) > 0 Or InStr(1, CAT_NO, "DELETE", 1) > 0 Or InStr(1, CAT_NO, "DROP", 1) > 0 Or InStr(1, CAT_NO, "INSERT", 1) > 1 Or InStr(1, CAT_NO, "TRACK", 1) > 1 Or InStr(1, CAT_NO, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        DDL_Titles.Focus()
                        Exit Sub
                    End If
                    CAT_NO = TrimX(CAT_NO)
                    'check unwanted characters
                    c = 0
                    counter1 = 0
                    For iloop = 1 To Len(CAT_NO.ToString)
                        strcurrentchar = Mid(CAT_NO, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=%""", strcurrentchar) <= 0 Then
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
                    Lbl_Error.Text = "Error: Plz Select Title from Drop-Down !"
                    DDL_Titles.Focus()
                    Exit Sub
                End If

                'validation for ART_TYPE
                Dim ART_TYPE As Object = Nothing
                If Me.DDL_Type.Text <> "" Then
                    ART_TYPE = Trim(DDL_Type.SelectedValue)
                    ART_TYPE = RemoveQuotes(ART_TYPE)

                    If ART_TYPE.ToString.Length > 5 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        DDL_Type.Focus()
                        Exit Sub
                    End If
                    ART_TYPE = " " & ART_TYPE & " "
                    If InStr(1, ART_TYPE, "CREATE", 1) > 0 Or InStr(1, ART_TYPE, "DELETE", 1) > 0 Or InStr(1, ART_TYPE, "DROP", 1) > 0 Or InStr(1, ART_TYPE, "INSERT", 1) > 1 Or InStr(1, ART_TYPE, "TRACK", 1) > 1 Or InStr(1, ART_TYPE, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        DDL_Type.Focus()
                        Exit Sub
                    End If
                    ART_TYPE = TrimX(ART_TYPE)
                    'check unwanted characters
                    c = 0
                    counter2 = 0
                    For iloop = 1 To Len(ART_TYPE.ToString)
                        strcurrentchar = Mid(ART_TYPE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter2 = 1
                            End If
                        End If
                    Next
                    If counter2 = 1 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        DDL_Type.Focus()
                        Exit Sub
                    End If
                Else
                    ART_TYPE = "A"
                End If

                'validation for VOL_NO
                Dim VOL As Object = Nothing
                If txt_Art_VolNo.Text <> "" Then
                    VOL = TrimAll(txt_Art_VolNo.Text)
                    VOL = RemoveQuotes(VOL)
                    If VOL.Length > 50 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        txt_Art_VolNo.Focus()
                        Exit Sub
                    End If

                    VOL = " " & VOL & " "
                    If InStr(1, VOL, "CREATE", 1) > 0 Or InStr(1, VOL, "DELETE", 1) > 0 Or InStr(1, VOL, "DROP", 1) > 0 Or InStr(1, VOL, "INSERT", 1) > 1 Or InStr(1, VOL, "TRACK", 1) > 1 Or InStr(1, VOL, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Do not use Reserve Word"
                        txt_Art_VolNo.Focus()
                        Exit Sub
                    End If
                    VOL = TrimAll(VOL)
                    'check unwanted characters
                    c = 0
                    counter3 = 0
                    For iloop = 1 To Len(VOL)
                        strcurrentchar = Mid(VOL, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter3 = 1
                            End If
                        End If
                    Next
                    If counter3 = 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        txt_Art_VolNo.Focus()
                        Exit Sub
                    End If
                Else
                    VOL = ""
                End If

                'validation for ISSUE_NO
                Dim ISSUE As Object = Nothing
                If txt_Art_IssueNo.Text <> "" Then
                    ISSUE = TrimAll(txt_Art_IssueNo.Text)
                    ISSUE = RemoveQuotes(ISSUE)
                    If ISSUE.Length > 30 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        txt_Art_IssueNo.Focus()
                        Exit Sub
                    End If

                    ISSUE = " " & ISSUE & " "
                    If InStr(1, ISSUE, "CREATE", 1) > 0 Or InStr(1, ISSUE, "DELETE", 1) > 0 Or InStr(1, ISSUE, "DROP", 1) > 0 Or InStr(1, ISSUE, "INSERT", 1) > 1 Or InStr(1, ISSUE, "TRACK", 1) > 1 Or InStr(1, ISSUE, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Do not use Reserve Word"
                        txt_Art_IssueNo.Focus()
                        Exit Sub
                    End If
                    ISSUE = TrimAll(ISSUE)
                    'check unwanted characters
                    c = 0
                    counter4 = 0
                    For iloop = 1 To Len(ISSUE)
                        strcurrentchar = Mid(ISSUE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter4 = 1
                            End If
                        End If
                    Next
                    If counter4 = 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        txt_Art_IssueNo.Focus()
                        Exit Sub
                    End If
                Else
                    ISSUE = ""
                End If

                'validation for PERIOD
                Dim PERIOD As Object = Nothing
                If txt_Art_Period.Text <> "" Then
                    PERIOD = TrimAll(txt_Art_Period.Text)
                    PERIOD = RemoveQuotes(PERIOD)
                    If PERIOD.Length > 50 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        txt_Art_Period.Focus()
                        Exit Sub
                    End If

                    PERIOD = " " & PERIOD & " "
                    If InStr(1, PERIOD, "CREATE", 1) > 0 Or InStr(1, PERIOD, "DELETE", 1) > 0 Or InStr(1, PERIOD, "DROP", 1) > 0 Or InStr(1, PERIOD, "INSERT", 1) > 1 Or InStr(1, PERIOD, "TRACK", 1) > 1 Or InStr(1, PERIOD, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        txt_Art_Period.Focus()
                        Exit Sub
                    End If
                    PERIOD = TrimAll(PERIOD)
                    'check unwanted characters
                    c = 0
                    counter5 = 0
                    For iloop = 1 To Len(PERIOD)
                        strcurrentchar = Mid(PERIOD, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter5 = 1
                            End If
                        End If
                    Next
                    If counter5 = 1 Then
                        Lbl_Error.Text = " Input  is not Valid!"
                        txt_Art_Period.Focus()
                        Exit Sub
                    End If
                Else
                    PERIOD = ""
                End If

                'validation for ART_YEAR
                Dim ART_YEAR As Object = Nothing
                If Me.txt_Art_Year.Text <> "" Then
                    ART_YEAR = TrimAll(txt_Art_Year.Text)
                    ART_YEAR = RemoveQuotes(ART_YEAR)

                    If Len(ART_YEAR) <> 4 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        txt_Art_Year.Focus()
                        Exit Sub
                    End If
                    ART_YEAR = " " & ART_YEAR & " "
                    If InStr(1, ART_YEAR, "CREATE", 1) > 0 Or InStr(1, ART_YEAR, "DELETE", 1) > 0 Or InStr(1, ART_YEAR, "DROP", 1) > 0 Or InStr(1, ART_YEAR, "INSERT", 1) > 1 Or InStr(1, ART_YEAR, "TRACK", 1) > 1 Or InStr(1, ART_YEAR, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        txt_Art_Year.Focus()
                        Exit Sub
                    End If
                    ART_YEAR = TrimAll(ART_YEAR)
                Else
                    ART_YEAR = Now.Year
                End If

                'validation for CHAP_NO
                Dim CHAP_NO As Object = Nothing
                If Me.txt_Art_ChapterNo.Text <> "" Then
                    CHAP_NO = TrimAll(txt_Art_ChapterNo.Text)
                    CHAP_NO = RemoveQuotes(CHAP_NO)

                    If Len(CHAP_NO) > 10 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        txt_Art_ChapterNo.Focus()
                        Exit Sub
                    End If
                    CHAP_NO = " " & CHAP_NO & " "
                    If InStr(1, CHAP_NO, "CREATE", 1) > 0 Or InStr(1, CHAP_NO, "DELETE", 1) > 0 Or InStr(1, CHAP_NO, "DROP", 1) > 0 Or InStr(1, CHAP_NO, "INSERT", 1) > 1 Or InStr(1, CHAP_NO, "TRACK", 1) > 1 Or InStr(1, CHAP_NO, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        txt_Art_ChapterNo.Focus()
                        Exit Sub
                    End If
                    CHAP_NO = TrimAll(CHAP_NO)
                Else
                    CHAP_NO = Nothing
                End If

                Dim SHOW As Object = Nothing
                If Me.DDL_Show.Text <> "" Then
                    SHOW = TrimAll(DDL_Show.SelectedValue)
                    SHOW = RemoveQuotes(SHOW)

                    If Len(SHOW) > 2 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        DDL_Show.Focus()
                        Exit Sub
                    End If
                    SHOW = " " & SHOW & " "
                    If InStr(1, SHOW, "CREATE", 1) > 0 Or InStr(1, SHOW, "DELETE", 1) > 0 Or InStr(1, SHOW, "DROP", 1) > 0 Or InStr(1, SHOW, "INSERT", 1) > 1 Or InStr(1, SHOW, "TRACK", 1) > 1 Or InStr(1, SHOW, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        DDL_Show.Focus()
                        Exit Sub
                    End If
                    SHOW = TrimAll(SHOW)
                Else
                    SHOW = "Y"
                End If

                'Server validation for  : txt_Cat_Title
                Dim TITLE As Object = Nothing
                If txt_Art_Title.Text <> "" Then
                    TITLE = TrimAll(txt_Art_Title.Text)
                    TITLE = RemoveQuotes(TITLE)
                    If TITLE.Length > 350 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        txt_Art_Title.Focus()
                        Exit Sub
                    End If

                    TITLE = " " & TITLE & " "
                    If InStr(1, TITLE, "CREATE", 1) > 0 Or InStr(1, TITLE, "DELETE", 1) > 0 Or InStr(1, TITLE, "DROP", 1) > 0 Or InStr(1, TITLE, "INSERT", 1) > 1 Or InStr(1, TITLE, "TRACK", 1) > 1 Or InStr(1, TITLE, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = " Do Not use Reserve Words... "
                        Me.txt_Art_Title.Focus()
                        Exit Sub
                    End If
                    TITLE = TrimAll(TITLE)
                    'check unwanted characters
                    c = 0
                    counter6 = 0
                    For iloop = 1 To Len(TITLE.ToString)
                        strcurrentchar = Mid(TITLE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!+|""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter6 = 1
                            End If
                        End If
                    Next
                    If counter6 = 1 Then
                        txt_Art_Title.Text = " Do Not use un-wanted Characters... "
                        Me.txt_Art_Title.Focus()
                        Exit Sub
                    End If
                Else
                    Lbl_Error.Text = " Plz Enter Title!"
                    Me.txt_Art_Title.Focus()
                    Exit Sub
                End If

                'Server validation for  : txt_Cat_Title
                Dim SUB_TITLE As Object = Nothing
                If txt_Art_SubTitle.Text <> "" Then
                    SUB_TITLE = TrimAll(txt_Art_SubTitle.Text)
                    SUB_TITLE = RemoveQuotes(SUB_TITLE)
                    If SUB_TITLE.Length > 350 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        txt_Art_SubTitle.Focus()
                        Exit Sub
                    End If

                    SUB_TITLE = " " & SUB_TITLE & " "
                    If InStr(1, SUB_TITLE, "CREATE", 1) > 0 Or InStr(1, SUB_TITLE, "DELETE", 1) > 0 Or InStr(1, SUB_TITLE, "DROP", 1) > 0 Or InStr(1, SUB_TITLE, "INSERT", 1) > 1 Or InStr(1, SUB_TITLE, "TRACK", 1) > 1 Or InStr(1, SUB_TITLE, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = " Do Not use Reserve Words... "
                        Me.txt_Art_SubTitle.Focus()
                        Exit Sub
                    End If
                    SUB_TITLE = TrimAll(SUB_TITLE)
                    'check unwanted characters
                    c = 0
                    counter7 = 0
                    For iloop = 1 To Len(SUB_TITLE.ToString)
                        strcurrentchar = Mid(SUB_TITLE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!+|""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter7 = 1
                            End If
                        End If
                    Next
                    If counter7 = 1 Then
                        Lbl_Error.Text = " Do Not use un-wanted Characters... "
                        Me.txt_Art_SubTitle.Focus()
                        Exit Sub
                    End If
                Else
                    SUB_TITLE = Nothing
                End If

                'Server validation for  : AUTHORS
                Dim AUTHORS As Object = Nothing
                If txt_Art_Authors.Text <> "" Then
                    AUTHORS = TrimAll(txt_Art_Authors.Text)
                    AUTHORS = RemoveQuotes(AUTHORS)
                    If AUTHORS.Length > 250 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        txt_Art_Authors.Focus()
                        Exit Sub
                    End If

                    AUTHORS = " " & AUTHORS & " "
                    If InStr(1, AUTHORS, "CREATE", 1) > 0 Or InStr(1, AUTHORS, "DELETE", 1) > 0 Or InStr(1, AUTHORS, "DROP", 1) > 0 Or InStr(1, AUTHORS, "INSERT", 1) > 1 Or InStr(1, AUTHORS, "TRACK", 1) > 1 Or InStr(1, AUTHORS, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = " Do Not use Reserve Words... "
                        Me.txt_Art_Authors.Focus()
                        Exit Sub
                    End If
                    AUTHORS = TrimAll(AUTHORS)
                    'check unwanted characters
                    c = 0
                    counter8 = 0
                    For iloop = 1 To Len(AUTHORS.ToString)
                        strcurrentchar = Mid(AUTHORS, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!+|""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter8 = 1
                            End If
                        End If
                    Next
                    If counter8 = 1 Then
                        Lbl_Error.Text = " Do Not use un-wanted Characters... "
                        Me.txt_Art_Authors.Focus()
                        Exit Sub
                    End If
                Else
                    AUTHORS = Nothing
                End If

                'Server validation for  : ABSTRACT
                Dim ABSTRACT As Object = Nothing
                If txt_Art_Abstract.Text <> "" Then
                    ABSTRACT = TrimAll(txt_Art_Abstract.Text)
                    ABSTRACT = RemoveQuotes(ABSTRACT)
                    If ABSTRACT.Length > 4000 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        txt_Art_Abstract.Focus()
                        Exit Sub
                    End If
                    ABSTRACT = TrimAll(ABSTRACT)
                Else
                    ABSTRACT = Nothing
                End If

                'validation for DDL_Subjects
                Dim SUB_ID As Integer = Nothing
                If DDL_Subjects.Text <> "" Then
                    SUB_ID = DDL_Subjects.SelectedValue
                    SUB_ID = RemoveQuotes(SUB_ID)
                    If Len(SUB_ID) > 10 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        DDL_Subjects.Focus()
                        Exit Sub
                    End If

                    If Not IsNumeric(SUB_ID) = True Then 'maximum length
                        Lbl_Error.Text = " Data must be Numeric Only.. "
                        DDL_Subjects.Focus()
                        Exit Sub
                    End If
                Else
                    SUB_ID = Nothing
                End If

                'Server validation for  : txt_Cat_Keywords
                Dim KEYWORDS As Object = Nothing
                If txt_Art_Keywords.Text <> "" Then
                    KEYWORDS = TrimAll(txt_Art_Keywords.Text)
                    KEYWORDS = RemoveQuotes(KEYWORDS)
                    If KEYWORDS.Length > 350 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        txt_Art_Keywords.Focus()
                        Exit Sub
                    End If
                    KEYWORDS = " " & KEYWORDS & " "
                    If InStr(1, KEYWORDS, "CREATE", 1) > 0 Or InStr(1, KEYWORDS, "DELETE", 1) > 0 Or InStr(1, KEYWORDS, "DROP", 1) > 0 Or InStr(1, KEYWORDS, "INSERT", 1) > 1 Or InStr(1, KEYWORDS, "TRACK", 1) > 1 Or InStr(1, KEYWORDS, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = " Do Not use Reserve Words... "
                        Me.txt_Art_Keywords.Focus()
                        Exit Sub
                    End If
                    KEYWORDS = TrimAll(KEYWORDS)
                    'check unwanted characters
                    c = 0
                    counter9 = 0
                    For iloop = 1 To Len(KEYWORDS.ToString)
                        strcurrentchar = Mid(KEYWORDS, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter9 = 1
                            End If
                        End If
                    Next
                    If counter9 = 1 Then
                        Lbl_Error.Text = " Do Not use un-wanted Characters... "
                        Me.txt_Art_Keywords.Focus()
                        Exit Sub
                    End If
                    KEYWORDS = UCase(KEYWORDS)
                Else
                    KEYWORDS = ""
                End If

                'Server validation for  : PAGE
                Dim PAGE As Object = Nothing
                If txt_Art_Page.Text <> "" Then
                    PAGE = TrimAll(txt_Art_Page.Text)
                    PAGE = RemoveQuotes(PAGE)
                    If PAGE.Length > 50 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        txt_Art_Page.Focus()
                        Exit Sub
                    End If
                    PAGE = " " & PAGE & " "
                    If InStr(1, PAGE, "CREATE", 1) > 0 Or InStr(1, PAGE, "DELETE", 1) > 0 Or InStr(1, PAGE, "DROP", 1) > 0 Or InStr(1, PAGE, "INSERT", 1) > 1 Or InStr(1, PAGE, "TRACK", 1) > 1 Or InStr(1, PAGE, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = " Do Not use Reserve Words... "
                        Me.txt_Art_Page.Focus()
                        Exit Sub
                    End If
                    PAGE = TrimAll(PAGE)
                    'check unwanted characters
                    c = 0
                    counter10 = 0
                    For iloop = 1 To Len(PAGE.ToString)
                        strcurrentchar = Mid(PAGE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter10 = 1
                            End If
                        End If
                    Next
                    If counter10 = 1 Then
                        Lbl_Error.Text = " Do Not use un-wanted Characters... "
                        Me.txt_Art_Page.Focus()
                        Exit Sub
                    End If
                    PAGE = TrimAll(PAGE)
                Else
                    PAGE = ""
                End If

                'Server validation for  : REMARKS
                Dim REMARKS As Object = Nothing
                If txt_Art_Remarks.Text <> "" Then
                    REMARKS = TrimAll(txt_Art_Remarks.Text)
                    REMARKS = RemoveQuotes(REMARKS)
                    If REMARKS.Length > 250 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        txt_Art_Remarks.Focus()
                        Exit Sub
                    End If

                    REMARKS = " " & REMARKS & " "
                    If InStr(1, REMARKS, "CREATE", 1) > 0 Or InStr(1, REMARKS, "DELETE", 1) > 0 Or InStr(1, REMARKS, "DROP", 1) > 0 Or InStr(1, REMARKS, "INSERT", 1) > 1 Or InStr(1, REMARKS, "TRACK", 1) > 1 Or InStr(1, REMARKS, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = " Do Not use Reserve Words... "
                        Me.txt_Art_Remarks.Focus()
                        Exit Sub
                    End If
                    REMARKS = TrimAll(REMARKS)
                Else
                    REMARKS = ""
                End If

                'Server validation for  : URL
                Dim URL As Object = Nothing
                If txt_Art_URL.Text <> "" Then
                    URL = TrimAll(txt_Art_URL.Text)
                    URL = RemoveQuotes(URL)
                    If URL.Length > 250 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        txt_Art_URL.Focus()
                        Exit Sub
                    End If

                    URL = " " & URL & " "
                    If InStr(1, URL, "CREATE", 1) > 0 Or InStr(1, URL, "DELETE", 1) > 0 Or InStr(1, URL, "DROP", 1) > 0 Or InStr(1, URL, "INSERT", 1) > 1 Or InStr(1, URL, "TRACK", 1) > 1 Or InStr(1, URL, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = " Do Not use Reserve Words... "
                        Me.txt_Art_URL.Focus()
                        Exit Sub
                    End If
                    URL = TrimAll(URL)
                    If InStr(URL, "http://") <> 0 Then
                        URL = "http://" & URL
                    End If
                Else
                    URL = ""
                End If

                Dim USER_CODE As Object = Nothing
                USER_CODE = Session.Item("LoggedUser")

                Dim DATE_ADDED As Object = Nothing
                DATE_ADDED = Now.Date
                DATE_ADDED = Convert.ToDateTime(DATE_ADDED, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                Dim FULL_TEXT As Object = Nothing
                FULL_TEXT = "Y"

                Dim LIB_CODE As Object = Nothing
                LIB_CODE = LibCode

                Dim IP As Object = Nothing
                IP = Request.UserHostAddress.Trim

                If SqlConn.State = 0 Then
                    SqlConn.Open()
                End If

                thisTransaction = SqlConn.BeginTransaction()
                Dim intValue As Integer = 0
                Dim objCommand As New SqlCommand
                objCommand.Connection = SqlConn
                objCommand.Transaction = thisTransaction
                objCommand.CommandType = CommandType.Text
                objCommand.CommandText = "INSERT INTO ARTICLES (CAT_NO, ART_TYPE, VOL, ISSUE, PERIOD, ART_YEAR, CHAP_NO, SHOW, TITLE, SUB_TITLE, AUTHORS, ABSTRACT, SUB_ID, KEYWORDS, PAGE, URL, IP, FULL_TEXT, REMARKS, DATE_ADDED, USER_CODE, LIB_CODE) " & _
                                         " VALUES (@CAT_NO, @ART_TYPE, @VOL, @ISSUE, @PERIOD, @ART_YEAR, @CHAP_NO, @SHOW, @TITLE, @SUB_TITLE, @AUTHORS, @ABSTRACT, @SUB_ID, @KEYWORDS, @PAGE, @URL, @IP, @FULL_TEXT, @REMARKS, @DATE_ADDED, @USER_CODE, @LIB_CODE); " & _
                                         " SELECT SCOPE_IDENTITY()"

                objCommand.Parameters.Add("@CAT_NO", SqlDbType.Int)
                If CAT_NO <> 0 Then
                    objCommand.Parameters("@CAT_NO").Value = CAT_NO
                Else
                    objCommand.Parameters("@CAT_NO").Value = System.DBNull.Value
                End If

                objCommand.Parameters.Add("@ART_TYPE", SqlDbType.NVarChar)
                If ART_TYPE <> "" Then
                    objCommand.Parameters("@ART_TYPE").Value = ART_TYPE
                Else
                    objCommand.Parameters("@ART_TYPE").Value = "A"
                End If

                objCommand.Parameters.Add("@VOL", SqlDbType.NVarChar)
                If VOL <> "" Then
                    objCommand.Parameters("@VOL").Value = VOL
                Else
                    objCommand.Parameters("@VOL").Value = System.DBNull.Value
                End If

                objCommand.Parameters.Add("@ISSUE", SqlDbType.NVarChar)
                If ISSUE <> "" Then
                    objCommand.Parameters("@ISSUE").Value = ISSUE
                Else
                    objCommand.Parameters("@ISSUE").Value = System.DBNull.Value
                End If

                objCommand.Parameters.Add("@PERIOD", SqlDbType.NVarChar)
                If PERIOD <> "" Then
                    objCommand.Parameters("@PERIOD").Value = PERIOD
                Else
                    objCommand.Parameters("@PERIOD").Value = System.DBNull.Value
                End If

                objCommand.Parameters.Add("@ART_YEAR", SqlDbType.Int)
                If ART_YEAR <> 0 Then
                    objCommand.Parameters("@ART_YEAR").Value = ART_YEAR
                Else
                    objCommand.Parameters("@ART_YEAR").Value = System.DBNull.Value
                End If

                objCommand.Parameters.Add("@CHAP_NO", SqlDbType.NVarChar)
                If CHAP_NO <> "" Then
                    objCommand.Parameters("@CHAP_NO").Value = CHAP_NO
                Else
                    objCommand.Parameters("@CHAP_NO").Value = System.DBNull.Value
                End If

                objCommand.Parameters.Add("@SHOW", SqlDbType.NVarChar)
                If SHOW <> "" Then
                    objCommand.Parameters("@SHOW").Value = SHOW
                Else
                    objCommand.Parameters("@SHOW").Value = System.DBNull.Value
                End If

                objCommand.Parameters.Add("@SUB_TITLE", SqlDbType.NVarChar)
                If SUB_TITLE <> "" Then
                    objCommand.Parameters("@SUB_TITLE").Value = SUB_TITLE
                Else
                    objCommand.Parameters("@SUB_TITLE").Value = System.DBNull.Value
                End If

                objCommand.Parameters.Add("@TITLE", SqlDbType.NVarChar)
                If TITLE <> "" Then
                    objCommand.Parameters("@TITLE").Value = TITLE
                Else
                    objCommand.Parameters("@TITLE").Value = System.DBNull.Value
                End If

                objCommand.Parameters.Add("@AUTHORS", SqlDbType.NVarChar)
                If AUTHORS <> "" Then
                    objCommand.Parameters("@AUTHORS").Value = AUTHORS
                Else
                    objCommand.Parameters("@AUTHORS").Value = System.DBNull.Value
                End If

                objCommand.Parameters.Add("@ABSTRACT", SqlDbType.NVarChar)
                If ABSTRACT <> "" Then
                    objCommand.Parameters("@ABSTRACT").Value = ABSTRACT
                Else
                    objCommand.Parameters("@ABSTRACT").Value = System.DBNull.Value
                End If

                objCommand.Parameters.Add("@SUB_ID", SqlDbType.Int)
                If SUB_ID <> 0 Then
                    objCommand.Parameters("@SUB_ID").Value = SUB_ID
                Else
                    objCommand.Parameters("@SUB_ID").Value = System.DBNull.Value
                End If

                objCommand.Parameters.Add("@KEYWORDS", SqlDbType.NVarChar)
                If KEYWORDS <> "" Then
                    objCommand.Parameters("@KEYWORDS").Value = KEYWORDS
                Else
                    objCommand.Parameters("@KEYWORDS").Value = System.DBNull.Value
                End If

                objCommand.Parameters.Add("@PAGE", SqlDbType.NVarChar)
                If PAGE <> "" Then
                    objCommand.Parameters("@PAGE").Value = PAGE
                Else
                    objCommand.Parameters("@PAGE").Value = System.DBNull.Value
                End If

                objCommand.Parameters.Add("@URL", SqlDbType.NVarChar)
                If URL <> "" Then
                    objCommand.Parameters("@URL").Value = URL
                Else
                    objCommand.Parameters("@URL").Value = System.DBNull.Value
                End If

                objCommand.Parameters.Add("@IP", SqlDbType.NVarChar)
                If IP <> "" Then
                    objCommand.Parameters("@IP").Value = IP
                Else
                    objCommand.Parameters("@IP").Value = System.DBNull.Value
                End If

                objCommand.Parameters.Add("@FULL_TEXT", SqlDbType.VarChar)
                If FileUpload1.FileName <> "" Then
                    objCommand.Parameters("@FULL_TEXT").Value = "Y"
                Else
                    objCommand.Parameters("@FULL_TEXT").Value = "N"
                End If

                objCommand.Parameters.Add("@REMARKS", SqlDbType.NVarChar)
                If REMARKS <> "" Then
                    objCommand.Parameters("@REMARKS").Value = REMARKS
                Else
                    objCommand.Parameters("@REMARKS").Value = System.DBNull.Value
                End If

                objCommand.Parameters.Add("@DATE_ADDED", SqlDbType.DateTime)
                If DATE_ADDED <> "" Then
                    objCommand.Parameters("@DATE_ADDED").Value = DATE_ADDED
                Else
                    objCommand.Parameters("@DATE_ADDED").Value = System.DBNull.Value
                End If

                objCommand.Parameters.Add("@USER_CODE", SqlDbType.NVarChar)
                If USER_CODE <> "" Then
                    objCommand.Parameters("@USER_CODE").Value = USER_CODE
                Else
                    objCommand.Parameters("@USER_CODE").Value = System.DBNull.Value
                End If

                objCommand.Parameters.Add("@LIB_CODE", SqlDbType.NVarChar)
                If LIB_CODE <> "" Then
                    objCommand.Parameters("@LIB_CODE").Value = LIB_CODE
                Else
                    objCommand.Parameters("@LIB_CODE").Value = System.DBNull.Value
                End If

                Dim dr As SqlDataReader
                dr = objCommand.ExecuteReader()
                If dr.Read Then
                    intValue = dr.GetValue(0)
                End If
                dr.Close()

                thisTransaction.Commit()
                SqlConn.Close()

                Dim ImagePAth As String = Nothing
                If FileUpload1.FileName <> "" Then
                    ImagePAth = "~/DFILES/" & Session.Item("LoggedLibcode") & "/Articles/" & intValue & "/"
                    ImagePAth = Replace(ImagePAth, "\", "/")
                    ImagePAth = Replace(ImagePAth, "//", "/")
                    If Not Directory.Exists(Server.MapPath(ImagePAth)) Then
                        Directory.CreateDirectory(Server.MapPath(ImagePAth))
                    End If

                    If (FileUpload1.PostedFile IsNot Nothing) AndAlso (FileUpload1.PostedFile.ContentLength > 0) Then
                        Dim fn As String = System.IO.Path.GetFileName(FileUpload1.PostedFile.FileName)
                        Dim SaveLocation As String = Server.MapPath(ImagePAth) & "\" & fn
                        FileUpload1.PostedFile.SaveAs(SaveLocation)
                    End If
                End If

                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Record Added Sucessfully!');", True)
                Lbl_Error.Text = ""
                Me.Art_Save_Bttn.Visible = True
                Art_Update_Bttn.Visible = False
                Art_Delete_Bttn.Visible = False

                txt_Art_Abstract.Text = ""
                txt_Art_Authors.Text = ""
                txt_Art_Keywords.Text = ""
                txt_Art_Page.Text = ""
                txt_Art_Remarks.Text = ""
                txt_Art_SubTitle.Text = ""
                txt_Art_Title.Text = ""
                txt_Art_URL.Text = ""

                PopulateArticlesGrid()
                txt_Art_Title.Focus()
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
    Public Sub ClearFields()
        txt_Art_Abstract.Text = ""
        txt_Art_Authors.Text = ""
        txt_Art_ChapterNo.Text = ""
        txt_Art_IssueNo.Text = ""
        txt_Art_Keywords.Text = ""
        txt_Art_Page.Text = ""
        txt_Art_Period.Text = ""
        txt_Art_Remarks.Text = ""
        txt_Art_SubTitle.Text = ""
        txt_Art_Title.Text = ""
        txt_Art_URL.Text = ""
        txt_Art_VolNo.Text = ""
        txt_Art_Year.Text = ""
        DDL_Subjects.ClearSelection()

        HyperLink1.NavigateUrl = Nothing
        HyperLink1.Visible = False
        CheckBox3.Text = ""
        CheckBox3.Checked = False
        CheckBox3.Visible = False

        Label27.Text = ""
    End Sub
    'fill Grid with Approved Acq Records
    Public Sub PopulateArticlesGrid()
        Dim dtSearch As DataTable = Nothing
        Try
            If DDL_Titles.Text <> "" Then
                If DDL_Titles.Text <> "" Then

                    Dim SQL As String = Nothing
                    SQL = "SELECT ARTICLES.* , CATS.LANG_CODE, CATS.BIB_CODE, CATS.MAT_CODE, CATS.DOC_TYPE_CODE, CATS.STANDARD_NO, CATS.TITLE AS SOURCE_TITLE, CATS.SUB_TITLE AS SOURCE_SUB_TITLE, CATS.CORPORATE_AUTHOR, CATS.EDITOR, CATS.PLACE_OF_PUB FROM ARTICLES LEFT OUTER JOIN CATS ON ARTICLES.CAT_NO = CATS.CAT_NO WHERE (ARTICLES.LIB_CODE = '" & Trim(LibCode) & "') AND (CATS.CAT_NO = '" & DDL_Titles.SelectedValue & "'); "
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
                        Label25.Text = "Total Record(s): 0 "
                        Art_DeleteAll_Bttn.Visible = False
                    Else
                        Grid1.Visible = True
                        RecordCount = dtSearch.Rows.Count
                        Grid1.DataSource = dtSearch
                        Grid1.DataBind()
                        Label25.Text = "Total Record(s): " & RecordCount
                        Art_DeleteAll_Bttn.Visible = True
                    End If
                    ViewState("dt") = dtSearch
                Else
                    Me.Grid1.DataSource = Nothing
                    Grid1.DataBind()
                    Label25.Text = "Total Record(s): 0 "
                    Art_DeleteAll_Bttn.Visible = False
                End If
            Else
                Me.Grid1.DataSource = Nothing
                Grid1.DataBind()
                Label25.Text = "Total Record(s): 0 "
                Art_DeleteAll_Bttn.Visible = False
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
    'get value of row from grid
    Private Sub Grid1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles Grid1.RowCommand
        Dim Command As SqlCommand = Nothing
        Dim dr As SqlDataReader = Nothing
        Try
            If e.CommandName = "Select" Then
                Dim myRowID, ART_NO As Integer
                myRowID = e.CommandArgument.ToString()
                ART_NO = Grid1.DataKeys(myRowID).Value
                If Not String.IsNullOrEmpty(ART_NO) And ART_NO <> 0 Then
                    Label26.Text = ART_NO
                    Dim iloop As Integer
                    Dim strcurrentchar As Object
                    Dim c As Integer
                    Dim counter1 As Integer

                    ART_NO = TrimX(ART_NO)
                    ART_NO = RemoveQuotes(ART_NO)

                    If Len(ART_NO).ToString > 10 Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Length of Input is not Proper!');", True)
                        Exit Sub
                    End If

                    If Not IsNumeric(ART_NO.ToString) = True Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Input type is not Proper!');", True)
                        Exit Sub
                    End If

                    ART_NO = " " & ART_NO & " "
                    If InStr(1, ART_NO, " CREATE ", 1) > 0 Or InStr(1, ART_NO, " DELETE ", 1) > 0 Or InStr(1, ART_NO, " DROP ", 1) > 0 Or InStr(1, ART_NO, " INSERT ", 1) > 1 Or InStr(1, ART_NO, " TRACK ", 1) > 1 Or InStr(1, ART_NO, " TRACE ", 1) > 1 Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Do Not Use Reserve Words!');", True)
                        Exit Sub
                    End If
                    ART_NO = TrimX(ART_NO)

                    ClearFields()
                    'get record details from database
                    Dim SQL As String = Nothing
                    SQL = " SELECT *  FROM ARTICLES WHERE (ART_NO = '" & Trim(ART_NO) & "') "
                    Command = New SqlCommand(SQL, SqlConn)
                    SqlConn.Open()
                    dr = Command.ExecuteReader(CommandBehavior.CloseConnection)
                    dr.Read()
                    If dr.HasRows = True Then
                        If dr.Item("TITLE").ToString <> "" Then
                            txt_Art_Title.Text = dr.Item("TITLE").ToString
                        Else
                            txt_Art_Title.Text = ""
                        End If

                        If dr.Item("SUB_TITLE").ToString <> "" Then
                            txt_Art_SubTitle.Text = dr.Item("SUB_TITLE").ToString
                        Else
                            txt_Art_SubTitle.Text = ""
                        End If

                        If dr.Item("AUTHORS").ToString <> "" Then
                            txt_Art_Authors.Text = dr.Item("AUTHORS").ToString
                        Else
                            txt_Art_Authors.Text = ""
                        End If

                        If dr.Item("ABSTRACT").ToString <> "" Then
                            txt_Art_Abstract.Text = dr.Item("ABSTRACT").ToString
                        Else
                            txt_Art_Abstract.Text = ""
                        End If

                        If dr.Item("VOL").ToString <> "" Then
                            txt_Art_VolNo.Text = dr.Item("VOL").ToString
                        Else
                            txt_Art_VolNo.Text = ""
                        End If

                        If dr.Item("ISSUE").ToString <> "" Then
                            txt_Art_IssueNo.Text = dr.Item("ISSUE").ToString
                        Else
                            txt_Art_IssueNo.Text = ""
                        End If

                        If dr.Item("PERIOD").ToString <> "" Then
                            txt_Art_Period.Text = dr.Item("PERIOD").ToString
                        Else
                            txt_Art_Period.Text = ""
                        End If

                        If dr.Item("ART_YEAR").ToString <> "" Then
                            txt_Art_Year.Text = dr.Item("ART_YEAR").ToString
                        Else
                            txt_Art_Year.Text = ""
                        End If

                        If dr.Item("PAGE").ToString <> "" Then
                            txt_Art_Page.Text = dr.Item("PAGE").ToString
                        Else
                            txt_Art_Page.Text = ""
                        End If

                        If dr.Item("URL").ToString <> "" Then
                            txt_Art_URL.Text = dr.Item("URL").ToString
                        Else
                            txt_Art_URL.Text = ""
                        End If

                        If dr.Item("KEYWORDS").ToString <> "" Then
                            txt_Art_Keywords.Text = dr.Item("KEYWORDS").ToString
                        Else
                            txt_Art_Keywords.Text = ""
                        End If

                        If dr.Item("SUB_ID").ToString <> "" Then
                            DDL_Subjects.SelectedValue = dr.Item("SUB_ID").ToString
                        Else
                            DDL_Subjects.ClearSelection()
                        End If

                        If dr.Item("REMARKS").ToString <> "" Then
                            txt_Art_Remarks.Text = dr.Item("REMARKS").ToString
                        Else
                            txt_Art_Remarks.Text = ""
                        End If

                        If dr.Item("ART_TYPE").ToString <> "" Then
                            DDL_Type.SelectedValue = dr.Item("ART_TYPE").ToString
                        Else
                            DDL_Type.ClearSelection()
                        End If

                        If dr.Item("CHAP_NO").ToString <> "" Then
                            txt_Art_ChapterNo.Text = dr.Item("CHAP_NO").ToString
                        Else
                            txt_Art_ChapterNo.Text = ""
                        End If

                        Dim FilePath As String
                        FilePath = "~/DFILES/" & Session.Item("LoggedLibcode") & "/Articles/" & ART_NO & "/"

                        FilePath = Replace(FilePath, "\", "/")
                        FilePath = Replace(FilePath, "//", "/")
                        If System.IO.Directory.Exists(Server.MapPath(FilePath)) = True Then
                            Dim myDir As DirectoryInfo = New DirectoryInfo(Server.MapPath(FilePath))

                            If (myDir.EnumerateFiles().Any()) Then
                                Dim files() As String = IO.Directory.GetFiles(Server.MapPath(FilePath))
                                For Each sFile As String In files
                                    Dim FileName As String = System.IO.Path.GetFileName(sFile) ' Text &= IO.File.ReadAllText(sFile)
                                    Dim strURL As String = "~/DFILES/" & Session.Item("LoggedLibcode") & "/Articles/" & ART_NO & "/" & FileName '"AM51357293_2014-03-10_16-48-22.pdf"
                                    HyperLink1.Visible = True
                                    HyperLink1.NavigateUrl = strURL
                                    HyperLink1.Target = "_new"
                                    CheckBox3.Visible = True
                                    Label27.Text = Trim(FileName)
                                Next
                            Else
                                HyperLink1.Visible = False
                                CheckBox3.Visible = False
                            End If
                        Else
                            HyperLink1.Visible = False
                            CheckBox3.Visible = False
                        End If

                        Art_Save_Bttn.Visible = False
                        Art_Update_Bttn.Visible = True
                        Art_Delete_Bttn.Visible = True
                        dr.Close()
                    Else
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Record No Selected for Edit!');", True)
                    End If
                Else
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Record No Selected for Edit!');", True)
                End If
            End If
            txt_Art_Title.Focus()
        Catch s As Exception
            Lbl_Error.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub 'Grid1_ItemCommand

    'cacel click
    Protected Sub Art_Cancel_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Art_Cancel_Bttn.Click
        ClearFields()
        Label26.Text = ""
        Art_Save_Bttn.Visible = True
        Art_Update_Bttn.Visible = False
        Art_Delete_Bttn.Visible = False
    End Sub
    'update records
    Protected Sub Art_Update_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Art_Update_Bttn.Click
        Dim SQL As String = Nothing
        Dim da As SqlDataAdapter
        Dim cb As SqlCommandBuilder
        Dim ds As New DataSet
        Dim dt As New DataTable
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing
        Try
            If IsPostBack = True Then
                'Server Validation for Lib Code
                Dim iloop As Integer
                Dim strcurrentchar As Object
                Dim c As Integer
                Dim counter1, counter2, counter3, counter4, Counter5, counter6, counter7, counter8, counter9, counter10 As Integer

                If Label26.Text <> "" Then
                    'validation for ART_TYPE
                    Dim ART_TYPE As Object = Nothing
                    If Me.DDL_Type.Text <> "" Then
                        ART_TYPE = Trim(DDL_Type.SelectedValue)
                        ART_TYPE = RemoveQuotes(ART_TYPE)

                        If ART_TYPE.ToString.Length > 5 Then
                            Lbl_Error.Text = "Error: Input is not Valid !"
                            DDL_Type.Focus()
                            Exit Sub
                        End If
                        ART_TYPE = " " & ART_TYPE & " "
                        If InStr(1, ART_TYPE, "CREATE", 1) > 0 Or InStr(1, ART_TYPE, "DELETE", 1) > 0 Or InStr(1, ART_TYPE, "DROP", 1) > 0 Or InStr(1, ART_TYPE, "INSERT", 1) > 1 Or InStr(1, ART_TYPE, "TRACK", 1) > 1 Or InStr(1, ART_TYPE, "TRACE", 1) > 1 Then
                            Lbl_Error.Text = "Error: Input is not Valid !"
                            DDL_Type.Focus()
                            Exit Sub
                        End If
                        ART_TYPE = TrimX(ART_TYPE)
                        'check unwanted characters
                        c = 0
                        counter2 = 0
                        For iloop = 1 To Len(ART_TYPE.ToString)
                            strcurrentchar = Mid(ART_TYPE, iloop, 1)
                            If c = 0 Then
                                If Not InStr("';<>=%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter2 = 1
                                End If
                            End If
                        Next
                        If counter2 = 1 Then
                            Lbl_Error.Text = "Error: Input is not Valid !"
                            DDL_Type.Focus()
                            Exit Sub
                        End If
                    Else
                        ART_TYPE = "A"
                    End If

                    'validation for VOL_NO
                    Dim VOL As Object = Nothing
                    If txt_Art_VolNo.Text <> "" Then
                        VOL = TrimAll(txt_Art_VolNo.Text)
                        VOL = RemoveQuotes(VOL)
                        If VOL.Length > 50 Then 'maximum length
                            Lbl_Error.Text = " Data must be of Proper Length.. "
                            txt_Art_VolNo.Focus()
                            Exit Sub
                        End If

                        VOL = " " & VOL & " "
                        If InStr(1, VOL, "CREATE", 1) > 0 Or InStr(1, VOL, "DELETE", 1) > 0 Or InStr(1, VOL, "DROP", 1) > 0 Or InStr(1, VOL, "INSERT", 1) > 1 Or InStr(1, VOL, "TRACK", 1) > 1 Or InStr(1, VOL, "TRACE", 1) > 1 Then
                            Lbl_Error.Text = "Do not use Reserve Word"
                            txt_Art_VolNo.Focus()
                            Exit Sub
                        End If
                        VOL = TrimAll(VOL)
                        'check unwanted characters
                        c = 0
                        counter3 = 0
                        For iloop = 1 To Len(VOL)
                            strcurrentchar = Mid(VOL, iloop, 1)
                            If c = 0 Then
                                If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter3 = 1
                                End If
                            End If
                        Next
                        If counter3 = 1 Then
                            Lbl_Error.Text = " Input  is not Valid!"
                            txt_Art_VolNo.Focus()
                            Exit Sub
                        End If
                    Else
                        VOL = ""
                    End If

                    'validation for ISSUE_NO
                    Dim ISSUE As Object = Nothing
                    If txt_Art_IssueNo.Text <> "" Then
                        ISSUE = TrimAll(txt_Art_IssueNo.Text)
                        ISSUE = RemoveQuotes(ISSUE)
                        If ISSUE.Length > 30 Then 'maximum length
                            Lbl_Error.Text = " Data must be of Proper Length.. "
                            txt_Art_IssueNo.Focus()
                            Exit Sub
                        End If

                        ISSUE = " " & ISSUE & " "
                        If InStr(1, ISSUE, "CREATE", 1) > 0 Or InStr(1, ISSUE, "DELETE", 1) > 0 Or InStr(1, ISSUE, "DROP", 1) > 0 Or InStr(1, ISSUE, "INSERT", 1) > 1 Or InStr(1, ISSUE, "TRACK", 1) > 1 Or InStr(1, ISSUE, "TRACE", 1) > 1 Then
                            Lbl_Error.Text = "Do not use Reserve Word"
                            txt_Art_IssueNo.Focus()
                            Exit Sub
                        End If
                        ISSUE = TrimAll(ISSUE)
                        'check unwanted characters
                        c = 0
                        counter4 = 0
                        For iloop = 1 To Len(ISSUE)
                            strcurrentchar = Mid(ISSUE, iloop, 1)
                            If c = 0 Then
                                If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter4 = 1
                                End If
                            End If
                        Next
                        If counter4 = 1 Then
                            Lbl_Error.Text = " Input  is not Valid!"
                            txt_Art_IssueNo.Focus()
                            Exit Sub
                        End If
                    Else
                        ISSUE = ""
                    End If

                    'validation for PERIOD
                    Dim PERIOD As Object = Nothing
                    If txt_Art_Period.Text <> "" Then
                        PERIOD = TrimAll(txt_Art_Period.Text)
                        PERIOD = RemoveQuotes(PERIOD)
                        If PERIOD.Length > 50 Then 'maximum length
                            Lbl_Error.Text = " Data must be of Proper Length.. "
                            txt_Art_Period.Focus()
                            Exit Sub
                        End If

                        PERIOD = " " & PERIOD & " "
                        If InStr(1, PERIOD, "CREATE", 1) > 0 Or InStr(1, PERIOD, "DELETE", 1) > 0 Or InStr(1, PERIOD, "DROP", 1) > 0 Or InStr(1, PERIOD, "INSERT", 1) > 1 Or InStr(1, PERIOD, "TRACK", 1) > 1 Or InStr(1, PERIOD, "TRACE", 1) > 1 Then
                            Lbl_Error.Text = " Input  is not Valid!"
                            txt_Art_Period.Focus()
                            Exit Sub
                        End If
                        PERIOD = TrimAll(PERIOD)
                        'check unwanted characters
                        c = 0
                        Counter5 = 0
                        For iloop = 1 To Len(PERIOD)
                            strcurrentchar = Mid(PERIOD, iloop, 1)
                            If c = 0 Then
                                If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    Counter5 = 1
                                End If
                            End If
                        Next
                        If Counter5 = 1 Then
                            Lbl_Error.Text = " Input  is not Valid!"
                            txt_Art_Period.Focus()
                            Exit Sub
                        End If
                    Else
                        PERIOD = ""
                    End If

                    'validation for ART_YEAR
                    Dim ART_YEAR As Object = Nothing
                    If Me.txt_Art_Year.Text <> "" Then
                        ART_YEAR = TrimAll(txt_Art_Year.Text)
                        ART_YEAR = RemoveQuotes(ART_YEAR)

                        If Len(ART_YEAR) <> 4 Then
                            Lbl_Error.Text = "Error: Input is not Valid !"
                            txt_Art_Year.Focus()
                            Exit Sub
                        End If
                        ART_YEAR = " " & ART_YEAR & " "
                        If InStr(1, ART_YEAR, "CREATE", 1) > 0 Or InStr(1, ART_YEAR, "DELETE", 1) > 0 Or InStr(1, ART_YEAR, "DROP", 1) > 0 Or InStr(1, ART_YEAR, "INSERT", 1) > 1 Or InStr(1, ART_YEAR, "TRACK", 1) > 1 Or InStr(1, ART_YEAR, "TRACE", 1) > 1 Then
                            Lbl_Error.Text = "Error: Input is not Valid !"
                            txt_Art_Year.Focus()
                            Exit Sub
                        End If
                        ART_YEAR = TrimAll(ART_YEAR)
                    Else
                        ART_YEAR = Now.Year
                    End If

                    'validation for CHAP_NO
                    Dim CHAP_NO As Object = Nothing
                    If Me.txt_Art_ChapterNo.Text <> "" Then
                        CHAP_NO = TrimAll(txt_Art_ChapterNo.Text)
                        CHAP_NO = RemoveQuotes(CHAP_NO)

                        If Len(CHAP_NO) > 10 Then
                            Lbl_Error.Text = "Error: Input is not Valid !"
                            txt_Art_ChapterNo.Focus()
                            Exit Sub
                        End If
                        CHAP_NO = " " & CHAP_NO & " "
                        If InStr(1, CHAP_NO, "CREATE", 1) > 0 Or InStr(1, CHAP_NO, "DELETE", 1) > 0 Or InStr(1, CHAP_NO, "DROP", 1) > 0 Or InStr(1, CHAP_NO, "INSERT", 1) > 1 Or InStr(1, CHAP_NO, "TRACK", 1) > 1 Or InStr(1, CHAP_NO, "TRACE", 1) > 1 Then
                            Lbl_Error.Text = "Error: Input is not Valid !"
                            txt_Art_ChapterNo.Focus()
                            Exit Sub
                        End If
                        CHAP_NO = TrimAll(CHAP_NO)
                    Else
                        CHAP_NO = Nothing
                    End If

                    Dim SHOW As Object = Nothing
                    If Me.DDL_Show.Text <> "" Then
                        SHOW = TrimAll(DDL_Show.SelectedValue)
                        SHOW = RemoveQuotes(SHOW)

                        If Len(SHOW) > 2 Then
                            Lbl_Error.Text = "Error: Input is not Valid !"
                            DDL_Show.Focus()
                            Exit Sub
                        End If
                        SHOW = " " & SHOW & " "
                        If InStr(1, SHOW, "CREATE", 1) > 0 Or InStr(1, SHOW, "DELETE", 1) > 0 Or InStr(1, SHOW, "DROP", 1) > 0 Or InStr(1, SHOW, "INSERT", 1) > 1 Or InStr(1, SHOW, "TRACK", 1) > 1 Or InStr(1, SHOW, "TRACE", 1) > 1 Then
                            Lbl_Error.Text = "Error: Input is not Valid !"
                            DDL_Show.Focus()
                            Exit Sub
                        End If
                        SHOW = TrimAll(SHOW)
                    Else
                        SHOW = "Y"
                    End If

                    'Server validation for  : txt_Cat_Title
                    Dim TITLE As Object = Nothing
                    If txt_Art_Title.Text <> "" Then
                        TITLE = TrimAll(txt_Art_Title.Text)
                        TITLE = RemoveQuotes(TITLE)
                        If TITLE.Length > 350 Then 'maximum length
                            Lbl_Error.Text = " Data must be of Proper Length.. "
                            txt_Art_Title.Focus()
                            Exit Sub
                        End If

                        TITLE = " " & TITLE & " "
                        If InStr(1, TITLE, "CREATE", 1) > 0 Or InStr(1, TITLE, "DELETE", 1) > 0 Or InStr(1, TITLE, "DROP", 1) > 0 Or InStr(1, TITLE, "INSERT", 1) > 1 Or InStr(1, TITLE, "TRACK", 1) > 1 Or InStr(1, TITLE, "TRACE", 1) > 1 Then
                            Lbl_Error.Text = " Do Not use Reserve Words... "
                            Me.txt_Art_Title.Focus()
                            Exit Sub
                        End If
                        TITLE = TrimAll(TITLE)
                        'check unwanted characters
                        c = 0
                        counter6 = 0
                        For iloop = 1 To Len(TITLE.ToString)
                            strcurrentchar = Mid(TITLE, iloop, 1)
                            If c = 0 Then
                                If Not InStr("'~!+|""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter6 = 1
                                End If
                            End If
                        Next
                        If counter6 = 1 Then
                            txt_Art_Title.Text = " Do Not use un-wanted Characters... "
                            Me.txt_Art_Title.Focus()
                            Exit Sub
                        End If
                    Else
                        Lbl_Error.Text = " Plz Enter Title!"
                        Me.txt_Art_Title.Focus()
                        Exit Sub
                    End If

                    'Server validation for  : txt_Cat_Title
                    Dim SUB_TITLE As Object = Nothing
                    If txt_Art_SubTitle.Text <> "" Then
                        SUB_TITLE = TrimAll(txt_Art_SubTitle.Text)
                        SUB_TITLE = RemoveQuotes(SUB_TITLE)
                        If SUB_TITLE.Length > 350 Then 'maximum length
                            Lbl_Error.Text = " Data must be of Proper Length.. "
                            txt_Art_SubTitle.Focus()
                            Exit Sub
                        End If

                        SUB_TITLE = " " & SUB_TITLE & " "
                        If InStr(1, SUB_TITLE, "CREATE", 1) > 0 Or InStr(1, SUB_TITLE, "DELETE", 1) > 0 Or InStr(1, SUB_TITLE, "DROP", 1) > 0 Or InStr(1, SUB_TITLE, "INSERT", 1) > 1 Or InStr(1, SUB_TITLE, "TRACK", 1) > 1 Or InStr(1, SUB_TITLE, "TRACE", 1) > 1 Then
                            Lbl_Error.Text = " Do Not use Reserve Words... "
                            Me.txt_Art_SubTitle.Focus()
                            Exit Sub
                        End If
                        SUB_TITLE = TrimAll(SUB_TITLE)
                        'check unwanted characters
                        c = 0
                        counter7 = 0
                        For iloop = 1 To Len(SUB_TITLE.ToString)
                            strcurrentchar = Mid(SUB_TITLE, iloop, 1)
                            If c = 0 Then
                                If Not InStr("'~!+|""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter7 = 1
                                End If
                            End If
                        Next
                        If counter7 = 1 Then
                            Lbl_Error.Text = " Do Not use un-wanted Characters... "
                            Me.txt_Art_SubTitle.Focus()
                            Exit Sub
                        End If
                    Else
                        SUB_TITLE = Nothing
                    End If

                    'Server validation for  : AUTHORS
                    Dim AUTHORS As Object = Nothing
                    If txt_Art_Authors.Text <> "" Then
                        AUTHORS = TrimAll(txt_Art_Authors.Text)
                        AUTHORS = RemoveQuotes(AUTHORS)
                        If AUTHORS.Length > 250 Then 'maximum length
                            Lbl_Error.Text = " Data must be of Proper Length.. "
                            txt_Art_Authors.Focus()
                            Exit Sub
                        End If

                        AUTHORS = " " & AUTHORS & " "
                        If InStr(1, AUTHORS, "CREATE", 1) > 0 Or InStr(1, AUTHORS, "DELETE", 1) > 0 Or InStr(1, AUTHORS, "DROP", 1) > 0 Or InStr(1, AUTHORS, "INSERT", 1) > 1 Or InStr(1, AUTHORS, "TRACK", 1) > 1 Or InStr(1, AUTHORS, "TRACE", 1) > 1 Then
                            Lbl_Error.Text = " Do Not use Reserve Words... "
                            Me.txt_Art_Authors.Focus()
                            Exit Sub
                        End If
                        AUTHORS = TrimAll(AUTHORS)
                        'check unwanted characters
                        c = 0
                        counter8 = 0
                        For iloop = 1 To Len(AUTHORS.ToString)
                            strcurrentchar = Mid(AUTHORS, iloop, 1)
                            If c = 0 Then
                                If Not InStr("'~!+|""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter8 = 1
                                End If
                            End If
                        Next
                        If counter8 = 1 Then
                            Lbl_Error.Text = " Do Not use un-wanted Characters... "
                            Me.txt_Art_Authors.Focus()
                            Exit Sub
                        End If
                    Else
                        AUTHORS = Nothing
                    End If

                    'Server validation for  : ABSTRACT
                    Dim ABSTRACT As Object = Nothing
                    If txt_Art_Abstract.Text <> "" Then
                        ABSTRACT = TrimAll(txt_Art_Abstract.Text)
                        ABSTRACT = RemoveQuotes(ABSTRACT)
                        If ABSTRACT.Length > 4000 Then 'maximum length
                            Lbl_Error.Text = " Data must be of Proper Length.. "
                            txt_Art_Abstract.Focus()
                            Exit Sub
                        End If
                        ABSTRACT = TrimAll(ABSTRACT)
                    Else
                        ABSTRACT = Nothing
                    End If

                    'validation for DDL_Subjects
                    Dim SUB_ID As Integer = Nothing
                    If DDL_Subjects.Text <> "" Then
                        SUB_ID = DDL_Subjects.SelectedValue
                        SUB_ID = RemoveQuotes(SUB_ID)
                        If Len(SUB_ID) > 10 Then 'maximum length
                            Lbl_Error.Text = " Data must be of Proper Length.. "
                            DDL_Subjects.Focus()
                            Exit Sub
                        End If

                        If Not IsNumeric(SUB_ID) = True Then 'maximum length
                            Lbl_Error.Text = " Data must be Numeric Only.. "
                            DDL_Subjects.Focus()
                            Exit Sub
                        End If
                    Else
                        SUB_ID = Nothing
                    End If

                    'Server validation for  : txt_Cat_Keywords
                    Dim KEYWORDS As Object = Nothing
                    If txt_Art_Keywords.Text <> "" Then
                        KEYWORDS = TrimAll(txt_Art_Keywords.Text)
                        KEYWORDS = RemoveQuotes(KEYWORDS)
                        If KEYWORDS.Length > 350 Then 'maximum length
                            Lbl_Error.Text = " Data must be of Proper Length.. "
                            txt_Art_Keywords.Focus()
                            Exit Sub
                        End If
                        KEYWORDS = " " & KEYWORDS & " "
                        If InStr(1, KEYWORDS, "CREATE", 1) > 0 Or InStr(1, KEYWORDS, "DELETE", 1) > 0 Or InStr(1, KEYWORDS, "DROP", 1) > 0 Or InStr(1, KEYWORDS, "INSERT", 1) > 1 Or InStr(1, KEYWORDS, "TRACK", 1) > 1 Or InStr(1, KEYWORDS, "TRACE", 1) > 1 Then
                            Lbl_Error.Text = " Do Not use Reserve Words... "
                            Me.txt_Art_Keywords.Focus()
                            Exit Sub
                        End If
                        KEYWORDS = TrimAll(KEYWORDS)
                        'check unwanted characters
                        c = 0
                        counter9 = 0
                        For iloop = 1 To Len(KEYWORDS.ToString)
                            strcurrentchar = Mid(KEYWORDS, iloop, 1)
                            If c = 0 Then
                                If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter9 = 1
                                End If
                            End If
                        Next
                        If counter9 = 1 Then
                            Lbl_Error.Text = " Do Not use un-wanted Characters... "
                            Me.txt_Art_Keywords.Focus()
                            Exit Sub
                        End If
                        KEYWORDS = UCase(KEYWORDS)
                    Else
                        KEYWORDS = ""
                    End If

                    'Server validation for  : PAGE
                    Dim PAGE As Object = Nothing
                    If txt_Art_Page.Text <> "" Then
                        PAGE = TrimAll(txt_Art_Page.Text)
                        PAGE = RemoveQuotes(PAGE)
                        If PAGE.Length > 50 Then 'maximum length
                            Lbl_Error.Text = " Data must be of Proper Length.. "
                            txt_Art_Page.Focus()
                            Exit Sub
                        End If
                        PAGE = " " & PAGE & " "
                        If InStr(1, PAGE, "CREATE", 1) > 0 Or InStr(1, PAGE, "DELETE", 1) > 0 Or InStr(1, PAGE, "DROP", 1) > 0 Or InStr(1, PAGE, "INSERT", 1) > 1 Or InStr(1, PAGE, "TRACK", 1) > 1 Or InStr(1, PAGE, "TRACE", 1) > 1 Then
                            Lbl_Error.Text = " Do Not use Reserve Words... "
                            Me.txt_Art_Page.Focus()
                            Exit Sub
                        End If
                        PAGE = TrimAll(PAGE)
                        'check unwanted characters
                        c = 0
                        counter10 = 0
                        For iloop = 1 To Len(PAGE.ToString)
                            strcurrentchar = Mid(PAGE, iloop, 1)
                            If c = 0 Then
                                If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter10 = 1
                                End If
                            End If
                        Next
                        If counter10 = 1 Then
                            Lbl_Error.Text = " Do Not use un-wanted Characters... "
                            Me.txt_Art_Page.Focus()
                            Exit Sub
                        End If
                        PAGE = TrimAll(PAGE)
                    Else
                        PAGE = ""
                    End If

                    'Server validation for  : REMARKS
                    Dim REMARKS As Object = Nothing
                    If txt_Art_Remarks.Text <> "" Then
                        REMARKS = TrimAll(txt_Art_Remarks.Text)
                        REMARKS = RemoveQuotes(REMARKS)
                        If REMARKS.Length > 250 Then 'maximum length
                            Lbl_Error.Text = " Data must be of Proper Length.. "
                            txt_Art_Remarks.Focus()
                            Exit Sub
                        End If

                        REMARKS = " " & REMARKS & " "
                        If InStr(1, REMARKS, "CREATE", 1) > 0 Or InStr(1, REMARKS, "DELETE", 1) > 0 Or InStr(1, REMARKS, "DROP", 1) > 0 Or InStr(1, REMARKS, "INSERT", 1) > 1 Or InStr(1, REMARKS, "TRACK", 1) > 1 Or InStr(1, REMARKS, "TRACE", 1) > 1 Then
                            Lbl_Error.Text = " Do Not use Reserve Words... "
                            Me.txt_Art_Remarks.Focus()
                            Exit Sub
                        End If
                        REMARKS = TrimAll(REMARKS)
                    Else
                        REMARKS = ""
                    End If

                    'Server validation for  : URL
                    Dim URL As Object = Nothing
                    If txt_Art_URL.Text <> "" Then
                        URL = TrimAll(txt_Art_URL.Text)
                        URL = RemoveQuotes(URL)
                        If URL.Length > 250 Then 'maximum length
                            Lbl_Error.Text = " Data must be of Proper Length.. "
                            txt_Art_URL.Focus()
                            Exit Sub
                        End If

                        URL = " " & URL & " "
                        If InStr(1, URL, "CREATE", 1) > 0 Or InStr(1, URL, "DELETE", 1) > 0 Or InStr(1, URL, "DROP", 1) > 0 Or InStr(1, URL, "INSERT", 1) > 1 Or InStr(1, URL, "TRACK", 1) > 1 Or InStr(1, URL, "TRACE", 1) > 1 Then
                            Lbl_Error.Text = " Do Not use Reserve Words... "
                            Me.txt_Art_URL.Focus()
                            Exit Sub
                        End If
                        URL = TrimAll(URL)
                        If InStr(URL, "http://") <> 0 Then
                            URL = "http://" & URL
                        End If
                    Else
                        URL = ""
                    End If


                    Dim FULL_TEXT As Object = Nothing
                    'delete file from folder
                    If CheckBox3.Checked = True Then
                        Dim FilePath As String
                        FilePath = "~/DFILES/" & Session.Item("LoggedLibcode") & "/Articles/" & Trim(Label26.Text) & "/"

                        FilePath = Replace(FilePath, "\", "/")
                        FilePath = Replace(FilePath, "//", "/")
                        If System.IO.Directory.Exists(Server.MapPath(FilePath)) = True Then
                            Dim myDir As DirectoryInfo = New DirectoryInfo(Server.MapPath(FilePath))

                            If (myDir.EnumerateFiles().Any()) Then
                                If File.Exists(Server.MapPath(FilePath & "/" & Label27.Text)) = True Then
                                    File.Delete(Server.MapPath(FilePath & "/" & Label27.Text))
                                End If
                            End If

                            If Not (myDir.EnumerateFiles().Any()) Then
                                Directory.Delete(Server.MapPath(FilePath))
                                FULL_TEXT = "N"
                            Else
                                FULL_TEXT = "Y"
                            End If
                        Else
                            FULL_TEXT = "N"
                        End If
                    Else
                        Dim FilePath As String
                        FilePath = "~/DFILES/" & Session.Item("LoggedLibcode") & "/Articles/" & Trim(Label26.Text) & "/"
                        FilePath = Replace(FilePath, "\", "/")
                        FilePath = Replace(FilePath, "//", "/")
                        If System.IO.Directory.Exists(Server.MapPath(FilePath)) = True Then
                            Dim myDir As DirectoryInfo = New DirectoryInfo(Server.MapPath(FilePath))
                            If (myDir.EnumerateFiles().Any()) Then
                                FULL_TEXT = "Y"
                            Else
                                FULL_TEXT = "N"
                            End If
                        Else
                            FULL_TEXT = "N"
                        End If
                    End If

                    If FileUpload1.FileName <> "" Then
                        FULL_TEXT = "Y"
                    End If

                    SQL = "SELECT * FROM ARTICLES WHERE (ART_NO='" & Trim(Label26.Text) & "')"
                    SqlConn.Open()
                    da = New SqlDataAdapter(SQL, SqlConn)
                    cb = New SqlCommandBuilder(da)
                    da.Fill(ds, "ARTICLES")
                    If ds.Tables("ARTICLES").Rows.Count <> 0 Then

                        If TITLE <> "" Then
                            ds.Tables("ARTICLES").Rows(0)("TITLE") = TITLE
                        Else
                            ds.Tables("ARTICLES").Rows(0)("TITLE") = System.DBNull.Value
                        End If

                        If SUB_TITLE <> "" Then
                            ds.Tables("ARTICLES").Rows(0)("SUB_TITLE") = SUB_TITLE
                        Else
                            ds.Tables("ARTICLES").Rows(0)("SUB_TITLE") = System.DBNull.Value
                        End If

                        If AUTHORS <> "" Then
                            ds.Tables("ARTICLES").Rows(0)("AUTHORS") = AUTHORS
                        Else
                            ds.Tables("ARTICLES").Rows(0)("AUTHORS") = System.DBNull.Value
                        End If

                        If ABSTRACT <> "" Then
                            ds.Tables("ARTICLES").Rows(0)("ABSTRACT") = ABSTRACT
                        Else
                            ds.Tables("ARTICLES").Rows(0)("ABSTRACT") = System.DBNull.Value
                        End If

                        If VOL <> "" Then
                            ds.Tables("ARTICLES").Rows(0)("VOL") = VOL
                        Else
                            ds.Tables("ARTICLES").Rows(0)("VOL") = System.DBNull.Value
                        End If

                        If ISSUE <> "" Then
                            ds.Tables("ARTICLES").Rows(0)("ISSUE") = ISSUE
                        Else
                            ds.Tables("ARTICLES").Rows(0)("ISSUE") = System.DBNull.Value
                        End If

                        If PERIOD <> "" Then
                            ds.Tables("ARTICLES").Rows(0)("PERIOD") = PERIOD
                        Else
                            ds.Tables("ARTICLES").Rows(0)("PERIOD") = System.DBNull.Value
                        End If

                        If ART_YEAR <> 0 Then
                            ds.Tables("ARTICLES").Rows(0)("ART_YEAR") = ART_YEAR
                        Else
                            ds.Tables("ARTICLES").Rows(0)("ART_YEAR") = System.DBNull.Value
                        End If

                        If PAGE <> "" Then
                            ds.Tables("ARTICLES").Rows(0)("PAGE") = PAGE
                        Else
                            ds.Tables("ARTICLES").Rows(0)("PAGE") = System.DBNull.Value
                        End If

                        If URL <> "" Then
                            ds.Tables("ARTICLES").Rows(0)("URL") = URL
                        Else
                            ds.Tables("ARTICLES").Rows(0)("URL") = System.DBNull.Value
                        End If

                        If KEYWORDS <> "" Then
                            ds.Tables("ARTICLES").Rows(0)("KEYWORDS") = KEYWORDS
                        Else
                            ds.Tables("ARTICLES").Rows(0)("KEYWORDS") = System.DBNull.Value
                        End If

                        If SUB_ID <> 0 Then
                            ds.Tables("ARTICLES").Rows(0)("SUB_ID") = SUB_ID
                        Else
                            ds.Tables("ARTICLES").Rows(0)("SUB_ID") = System.DBNull.Value
                        End If

                        If REMARKS <> "" Then
                            ds.Tables("ARTICLES").Rows(0)("REMARKS") = REMARKS
                        Else
                            ds.Tables("ARTICLES").Rows(0)("REMARKS") = System.DBNull.Value
                        End If

                        If ART_TYPE <> "" Then
                            ds.Tables("ARTICLES").Rows(0)("ART_TYPE") = ART_TYPE
                        Else
                            ds.Tables("ARTICLES").Rows(0)("ART_TYPE") = System.DBNull.Value
                        End If

                        If SHOW <> "" Then
                            ds.Tables("ARTICLES").Rows(0)("SHOW") = SHOW
                        Else
                            ds.Tables("ARTICLES").Rows(0)("SHOW") = System.DBNull.Value
                        End If

                        If CHAP_NO <> "" Then
                            ds.Tables("ARTICLES").Rows(0)("CHAP_NO") = CHAP_NO
                        Else
                            ds.Tables("ARTICLES").Rows(0)("CHAP_NO") = System.DBNull.Value
                        End If

                        If FULL_TEXT <> "" Then
                            ds.Tables("ARTICLES").Rows(0)("FULL_TEXT") = FULL_TEXT
                        Else
                            ds.Tables("ARTICLES").Rows(0)("FULL_TEXT") = "N"
                        End If

                    ds.Tables("ARTICLES").Rows(0)("UPDATED_BY") = UserCode.Trim
                    ds.Tables("ARTICLES").Rows(0)("DATE_MODIFIED") = Now.Date
                    ds.Tables("ARTICLES").Rows(0)("IP") = Request.UserHostAddress.Trim

                    thisTransaction = SqlConn.BeginTransaction()
                    da.SelectCommand.Transaction = thisTransaction
                    da.Update(ds, "ARTICLES")
                    thisTransaction.Commit()

                    'upload file
                    Dim ImagePAth As String = Nothing
                    If FileUpload1.FileName <> "" Then
                        ImagePAth = "~/DFILES/" & Session.Item("LoggedLibcode") & "/Articles/" & Label26.Text & "/"
                        ImagePAth = Replace(ImagePAth, "\", "/")
                        ImagePAth = Replace(ImagePAth, "//", "/")
                        If Not Directory.Exists(Server.MapPath(ImagePAth)) Then
                            Directory.CreateDirectory(Server.MapPath(ImagePAth))
                        End If

                        If (FileUpload1.PostedFile IsNot Nothing) AndAlso (FileUpload1.PostedFile.ContentLength > 0) Then
                            Dim fn As String = System.IO.Path.GetFileName(FileUpload1.PostedFile.FileName)
                            Dim SaveLocation As String = Server.MapPath(ImagePAth) & "\" & fn
                            FileUpload1.PostedFile.SaveAs(SaveLocation)
                        End If
                    End If

                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Record updated Sucessfulyy!');", True)
                    ClearFields()
                Else
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Error in Record Updation!');", True)
                    Exit Sub
                End If
            End If
            Else
            'record not selected
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Record Not Selected for Edit!');", True)
            Exit Sub
            End If
            SqlConn.Close()
            PopulateArticlesGrid()
            ClearFields()
            Label26.Text = ""
            Art_Save_Bttn.Visible = True
            Art_Update_Bttn.Visible = False
            Art_Delete_Bttn.Visible = False
        Catch q As SqlException
            thisTransaction.Rollback()
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'delete record
    Protected Sub Art_Delete_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Art_Delete_Bttn.Click
        Try
            If Label26.Text <> "" Then
                Dim ART_NO As Object = Nothing
                ART_NO = Trim(Label26.Text)

                Dim FilePath As String
                FilePath = "~/DFILES/" & Session.Item("LoggedLibcode") & "/Articles/" & ART_NO & "/"

                FilePath = Replace(FilePath, "\", "/")
                FilePath = Replace(FilePath, "//", "/")
                If System.IO.Directory.Exists(Server.MapPath(FilePath)) = True Then
                    Dim myDir As DirectoryInfo = New DirectoryInfo(Server.MapPath(FilePath))

                    If (myDir.EnumerateFiles().Any()) Then
                        Dim files() As String = IO.Directory.GetFiles(Server.MapPath(FilePath))
                        For Each sFile As String In files
                            Dim FileName As String = System.IO.Path.GetFileName(sFile)
                            Dim myDelPath As String = Nothing
                            myDelPath = FilePath & FileName
                            If File.Exists(Server.MapPath(myDelPath)) = True Then
                                File.Delete(Server.MapPath(myDelPath))
                            End If
                        Next
                    End If
                    Directory.Delete(Server.MapPath(FilePath))
                End If

                'delete Record
                Dim SQL As String = Nothing
                SQL = "DELETE FROM ARTICLES WHERE (ART_NO ='" & Trim(ART_NO) & "') "
                Dim objCommand As New SqlCommand(SQL, SqlConn)
                Dim da As New SqlDataAdapter(objCommand)
                Dim ds As New DataSet
                da.Fill(ds)
                ClearFields()
                Me.Art_Save_Bttn.Visible = True
                Me.Art_Update_Bttn.Visible = False
                Me.Art_Delete_Bttn.Visible = False
            End If
            PopulateArticlesGrid()
        Catch q As SqlException
            Lbl_Error.Text = "Database Error -SAVE: " & (q.Message() & ", " & q.Number & ", " & q.GetType.ToString)
        Catch s As Exception
            Lbl_Error.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'delete selected records
    Protected Sub Art_DeleteAll_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Art_DeleteAll_Bttn.Click
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing
        Dim dt As DataTable = Nothing
        Try
            For Each row As GridViewRow In Grid1.Rows
                Dim cb As CheckBox = DirectCast(row.FindControl("cbd"), CheckBox)
                If cb IsNot Nothing AndAlso cb.Checked = True Then
                    Dim ART_NO As Integer = Convert.ToInt32(Grid1.DataKeys(row.RowIndex).Value)

                    Dim counter1 As Integer = Nothing
                    Dim iloop As Integer
                    Dim strcurrentchar As Object
                    Dim c As Integer

                    'validation for HOLD_ID
                    ART_NO = RemoveQuotes(ART_NO)
                    If Not IsNumeric(ART_NO) = True Then
                        Lbl_Error.Text = "Error:Select Title from Drop-Down!"
                        DDL_Titles.Focus()
                        Continue For
                    End If
                    If Len(ART_NO) > 10 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        DDL_Titles.Focus()
                        Continue For
                    End If
                    ART_NO = " " & ART_NO & " "
                    If InStr(1, ART_NO, "CREATE", 1) > 0 Or InStr(1, ART_NO, "DELETE", 1) > 0 Or InStr(1, ART_NO, "DROP", 1) > 0 Or InStr(1, ART_NO, "INSERT", 1) > 1 Or InStr(1, ART_NO, "TRACK", 1) > 1 Or InStr(1, ART_NO, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        Continue For
                    End If
                    ART_NO = TrimX(ART_NO)
                    'check unwanted characters
                    c = 0
                    counter1 = 0
                    For iloop = 1 To Len(ART_NO.ToString)
                        strcurrentchar = Mid(ART_NO, iloop, 1)
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

                    
                    If SqlConn.State = 0 Then
                        SqlConn.Open()
                    End If

                    thisTransaction = SqlConn.BeginTransaction()
                    Dim objCommand As New SqlCommand
                    objCommand.Connection = SqlConn
                    objCommand.Transaction = thisTransaction
                    objCommand.CommandType = CommandType.Text
                    objCommand.CommandText = "DELETE FROM  ARTICLES WHERE ART_NO = @ART_NO; "

                    objCommand.Parameters.Add("@ART_NO", SqlDbType.Int)
                    objCommand.Parameters("@ART_NO").Value = ART_NO

                    objCommand.ExecuteNonQuery()

                    thisTransaction.Commit()
                    SqlConn.Close()


                    'delete digital file
                    Dim FilePath As String
                    FilePath = "~/DFILES/" & Session.Item("LoggedLibcode") & "/Articles/" & ART_NO & "/"

                    FilePath = Replace(FilePath, "\", "/")
                    FilePath = Replace(FilePath, "//", "/")
                    If System.IO.Directory.Exists(Server.MapPath(FilePath)) = True Then
                        Dim myDir As DirectoryInfo = New DirectoryInfo(Server.MapPath(FilePath))

                        If (myDir.EnumerateFiles().Any()) Then
                            Dim files() As String = IO.Directory.GetFiles(Server.MapPath(FilePath))
                            For Each sFile As String In files
                                Dim FileName As String = System.IO.Path.GetFileName(sFile)
                                Dim myDelPath As String = Nothing
                                myDelPath = FilePath & FileName
                                If File.Exists(Server.MapPath(myDelPath)) = True Then
                                    File.Delete(Server.MapPath(myDelPath))
                                End If
                            Next
                        End If
                        Directory.Delete(Server.MapPath(FilePath))
                    End If
                End If
            Next
            PopulateArticlesGrid()
        Catch q As SqlException
            thisTransaction.Rollback()
            Lbl_Error.Text = "Database Error -UPDATE: " & (q.Message() & ", " & q.Number & ", " & q.GetType.ToString)
        Catch ex As Exception
            Lbl_Error.Text = "Error-UPDATE: " & (ex.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'delete selected records from above grid
    Protected Sub Delete_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Delete_Bttn.Click
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing
        Dim dt As DataTable = Nothing
        Try
            For Each row As GridViewRow In Grid1_Search.Rows
                Dim cb As CheckBox = DirectCast(row.FindControl("cbd"), CheckBox)
                If cb IsNot Nothing AndAlso cb.Checked = True Then
                    Dim ART_NO As Integer = Convert.ToInt32(Grid1_Search.DataKeys(row.RowIndex).Value)

                    Dim counter1 As Integer = Nothing
                    Dim iloop As Integer
                    Dim strcurrentchar As Object
                    Dim c As Integer

                    'validation for HOLD_ID
                    ART_NO = RemoveQuotes(ART_NO)
                    If Not IsNumeric(ART_NO) = True Then
                        Lbl_Error.Text = "Error:Select Title from Drop-Down!"
                        DDL_Titles.Focus()
                        Continue For
                    End If
                    If Len(ART_NO) > 10 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        DDL_Titles.Focus()
                        Continue For
                    End If
                    ART_NO = " " & ART_NO & " "
                    If InStr(1, ART_NO, "CREATE", 1) > 0 Or InStr(1, ART_NO, "DELETE", 1) > 0 Or InStr(1, ART_NO, "DROP", 1) > 0 Or InStr(1, ART_NO, "INSERT", 1) > 1 Or InStr(1, ART_NO, "TRACK", 1) > 1 Or InStr(1, ART_NO, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        Continue For
                    End If
                    ART_NO = TrimX(ART_NO)
                    'check unwanted characters
                    c = 0
                    counter1 = 0
                    For iloop = 1 To Len(ART_NO.ToString)
                        strcurrentchar = Mid(ART_NO, iloop, 1)
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


                    If SqlConn.State = 0 Then
                        SqlConn.Open()
                    End If

                    thisTransaction = SqlConn.BeginTransaction()
                    Dim objCommand As New SqlCommand
                    objCommand.Connection = SqlConn
                    objCommand.Transaction = thisTransaction
                    objCommand.CommandType = CommandType.Text
                    objCommand.CommandText = "DELETE FROM  ARTICLES WHERE ART_NO = @ART_NO; "

                    objCommand.Parameters.Add("@ART_NO", SqlDbType.Int)
                    objCommand.Parameters("@ART_NO").Value = ART_NO

                    objCommand.ExecuteNonQuery()

                    thisTransaction.Commit()
                    SqlConn.Close()


                    'delete digital file
                    Dim FilePath As String
                    FilePath = "~/DFILES/" & Session.Item("LoggedLibcode") & "/Articles/" & ART_NO & "/"

                    FilePath = Replace(FilePath, "\", "/")
                    FilePath = Replace(FilePath, "//", "/")
                    If System.IO.Directory.Exists(Server.MapPath(FilePath)) = True Then
                        Dim myDir As DirectoryInfo = New DirectoryInfo(Server.MapPath(FilePath))

                        If (myDir.EnumerateFiles().Any()) Then
                            Dim files() As String = IO.Directory.GetFiles(Server.MapPath(FilePath))
                            For Each sFile As String In files
                                Dim FileName As String = System.IO.Path.GetFileName(sFile)
                                Dim myDelPath As String = Nothing
                                myDelPath = FilePath & FileName
                                If File.Exists(Server.MapPath(myDelPath)) = True Then
                                    File.Delete(Server.MapPath(myDelPath))
                                End If
                            Next
                        End If
                        Directory.Delete(Server.MapPath(FilePath))
                    End If
                End If
            Next
            Search_Bttn_Click(sender, e)
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