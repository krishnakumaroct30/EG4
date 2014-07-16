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
Public Class OPACDefault
    Inherits System.Web.UI.Page
    Dim mySearchString As String = Nothing
    Dim index As Integer = 0
    Public dt As New DataTable("temp")
    Dim dtLang As New DataTable
    Dim dtYear As New DataTable
    Dim dtDocument As New DataTable
    Dim dtCountry As New DataTable
    Dim dtSubject As New DataTable
    Dim dtPlace As New DataTable
    Dim dtPub As New DataTable
    Dim strFind As Object
    Public DETAILS As Object = Nothing
    Dim RecordCount As Integer = Nothing
    Dim OPACLibCode As String = Nothing
    Public myNews As StringBuilder = New StringBuilder
    Public strData As String = Nothing
    Dim j As Integer = Nothing
    Dim myTreeValue As String = Nothing
    Dim myLibIntro As String = Nothing
    Dim myClusterIntro As String = Nothing
    Dim LIB_EMAIL As String = Nothing
    Dim LIB_FAX As String = Nothing
    Dim LIB_PHONE As String = Nothing
    Dim CONTACT_PERSON As String = Nothing
    Dim PARENT_ORG As String = Nothing
    Dim CLUSTER_ADDRESS As String = Nothing
    Dim CLUSTER_NAME2 As String = Nothing
    Dim CLUSTER_NAME As String = Nothing

    Dim LIB_NAME2 As String = Nothing
    Dim LIB_CITY2 As String = Nothing
    Dim PARENT_BODY2 As String = Nothing
    Dim LIB_ADD2 As String = Nothing
    Dim LIB_INCHARGE2 As String = Nothing
    Dim LIBRARY_NAME As String = Nothing
    Dim LIBRARY_ADD As String = Nothing
    Dim myIntroMaster As String = Nothing

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not SConnection() = True Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Database Connection is Lost mm!');", True)
            Else
                If Page.IsPostBack = False Then
                    ClusterStatistics()
                    TR_LANG.Visible = False
                    TR_YEAR.Visible = False
                    TR_DOC.Visible = False
                    TR_COUNTRY.Visible = False
                    TR_SUB.Visible = False
                    TR_PLACE.Visible = False
                    TR_PUB.Visible = False
                    TR_TRANSACTIONS.Visible = False

                    If Session("LoggedLibCode") <> "" Then
                        OPACLibCode = Session("LoggedLibCode")
                        GetLibraryDetails()
                        Dim myParam As String = Nothing
                        myParam = Request.QueryString("PARAM")
                        If myParam <> "" Then
                            PublishmyTransactions()
                        End If
                    Else
                        OPACLibCode = ""
                        GetClusterDetails()
                    End If
                    PopulateHoldingsGrid()
                    PopulateArticlesGrid()
                    PopulateLooseIssuesGrid()

                    myTreeValue = RKTreeviewValue
                    If myTreeValue <> "" Then
                        If myTreeValue = "About" Then
                            Label5.Text = "About Us"
                            Lbl_Contact.Text = ""
                            TR_NEWS.Visible = True
                            TR_EG_INTRO.Visible = True
                            Label3.Visible = True
                            TR_TRANSACTIONS.Visible = False
                            TR_CLUSTER_INTRO.Visible = True
                            GridView1.DataSource = Nothing
                            GridView1.DataBind()

                            CheckBoxList1.DataSource = Nothing
                            CheckBoxList1.DataBind()
                            CheckBoxList1.Items.Clear()

                            CheckBoxList2.DataSource = Nothing
                            CheckBoxList2.DataBind()
                            CheckBoxList2.Items.Clear()

                            CheckBoxList3.DataSource = Nothing
                            CheckBoxList3.DataBind()
                            CheckBoxList3.Items.Clear()

                            CheckBoxList4.DataSource = Nothing
                            CheckBoxList4.DataBind()
                            CheckBoxList4.Items.Clear()

                            CheckBoxList5.DataSource = Nothing
                            CheckBoxList5.DataBind()
                            CheckBoxList5.Items.Clear()

                            CheckBoxList6.DataSource = Nothing
                            CheckBoxList6.DataBind()
                            CheckBoxList6.Items.Clear()

                            CheckBoxList7.DataSource = Nothing
                            CheckBoxList7.DataBind()
                            CheckBoxList7.Items.Clear()

                            TR_DOC.Visible = False
                            TR_LANG.Visible = False
                            TR_YEAR.Visible = False
                            TR_COUNTRY.Visible = False
                            TR_SUB.Visible = False
                            TR_PLACE.Visible = False
                            TR_PUB.Visible = False
                            TR_RB.Visible = False
                            RKTreeviewValue = ""
                        End If
                        If myTreeValue = "Contact" Then
                            Label5.Text = "Contact Us"
                            Label1.Text = ""
                            If Session.Item("LoggedMemberNo") <> "" And Session("LoggedLibCode") <> "" Then
                                Lbl_Contact.Text = Replace("Contact Us: " & vbCrLf & vbCrLf & CONTACT_PERSON & vbCrLf & LIBRARY_NAME & vbCrLf & PARENT_ORG & vbCrLf & LIBRARY_ADD & vbCrLf & "Phone: " & LIB_PHONE & vbCrLf & "Fax: " & LIB_FAX & vbCrLf & "Email: " & LIB_EMAIL & vbCrLf & vbCrLf & LIB_INCHARGE2 & vbCrLf & LIB_NAME2 & vbCrLf & PARENT_BODY2 & vbCrLf & LIB_ADD2 & vbCrLf & "Phone: " & LIB_PHONE & vbCrLf & "Fax: " & LIB_FAX & vbCrLf & "Email: " & LIB_EMAIL & vbCrLf & vbCrLf, Environment.NewLine, "<br />")
                            Else
                                Lbl_Contact.Text = Replace("Contact Us: " & vbCrLf & CLUSTER_NAME2 & vbCrLf & CLUSTER_ADDRESS & vbCrLf, Environment.NewLine, "<br />")
                            End If
                            Label1.Visible = True

                            TR_NEWS.Visible = True
                            TR_EG_INTRO.Visible = True
                            Label3.Visible = True
                            TR_TRANSACTIONS.Visible = False
                            TR_CLUSTER_INTRO.Visible = True
                            GridView1.DataSource = Nothing
                            GridView1.DataBind()

                            CheckBoxList1.DataSource = Nothing
                            CheckBoxList1.DataBind()
                            CheckBoxList1.Items.Clear()

                            CheckBoxList2.DataSource = Nothing
                            CheckBoxList2.DataBind()
                            CheckBoxList2.Items.Clear()

                            CheckBoxList3.DataSource = Nothing
                            CheckBoxList3.DataBind()
                            CheckBoxList3.Items.Clear()

                            CheckBoxList4.DataSource = Nothing
                            CheckBoxList4.DataBind()
                            CheckBoxList4.Items.Clear()

                            CheckBoxList5.DataSource = Nothing
                            CheckBoxList5.DataBind()
                            CheckBoxList5.Items.Clear()

                            CheckBoxList6.DataSource = Nothing
                            CheckBoxList6.DataBind()
                            CheckBoxList6.Items.Clear()

                            CheckBoxList7.DataSource = Nothing
                            CheckBoxList7.DataBind()
                            CheckBoxList7.Items.Clear()

                            TR_DOC.Visible = False
                            TR_LANG.Visible = False
                            TR_YEAR.Visible = False
                            TR_COUNTRY.Visible = False
                            TR_SUB.Visible = False
                            TR_PLACE.Visible = False
                            TR_PUB.Visible = False
                            TR_RB.Visible = False
                            RKTreeviewValue = ""
                        End If
                    End If






                End If
                    GetLatestNews()
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Sub ClusterStatistics()
        Dim dtCLStat As DataTable = Nothing
        Try
            Dim ds As New DataSet
            Dim SQL As String = Nothing
            SQL = "SELECT LIB_CODE,LIB_NAME, PARENT_BODY, LIB_ADDRESS, LIB_EMAIL, LIB_PHONE, (SELECT COUNT(*) FROM HOLDINGS WHERE HOLDINGS.LIB_CODE = LIBRARIES.LIB_CODE) As HOLD_COUNT,(SELECT COUNT(*)FROM MEMBERSHIPS WHERE MEMBERSHIPS.LIB_CODE = LIBRARIES.LIB_CODE) As MEMBER_COUNT,(SELECT COUNT(*) FROM CIRCULATION WHERE CIRCULATION.LIB_CODE = LIBRARIES.LIB_CODE) As CIR_COUNT FROM LIBRARIES; "
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            da.Fill(ds)

            dtCLStat = ds.Tables(0).Copy
            Dim RecordCount As Long = 0
            If dtCLStat.Rows.Count = 0 Then
                Me.Grid1_Stat.DataSource = Nothing
                Grid1_Stat.DataBind()
                Grid1_Stat.Enabled = False
            Else
                Grid1_Stat.Visible = True
                RecordCount = dtCLStat.Rows.Count
                Grid1_Stat.DataSource = dtCLStat
                Grid1_Stat.DataBind()
            End If
            ViewState("dt") = dtCLStat
        Catch s As Exception

        Finally
            SqlConn.Close()
        End Try
    End Sub
    Public Sub GetClusterDetails()
        Dim dtCluster As DataTable = Nothing
        Try
            Dim ds As New DataSet
            Dim SQL As String = Nothing
            SQL = "SELECT * FROM CLUSTER; "
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            da.Fill(ds)

            If ds.Tables.Count > 0 Then
                dtCluster = ds.Tables(0)
            Else
                Exit Sub
            End If

            If dtCluster.Rows.Count <> 0 Then
                If dtCluster.Rows(0).Item("NAME").ToString <> "" Then
                    CLUSTER_NAME = dtCluster.Rows(0).Item("NAME").ToString
                    CLUSTER_NAME2 = dtCluster.Rows(0).Item("NAME").ToString
                Else
                    CLUSTER_NAME = ""
                    CLUSTER_NAME2 = ""
                End If

                If dtCluster.Rows(0).Item("REMARKS").ToString <> "" Then
                    TR_CLUSTER_INTRO.Visible = True
                    myClusterIntro = dtCluster.Rows(0).Item("REMARKS").ToString.Replace(Environment.NewLine, "<br />")

                    'If myClusterIntro.Length > 2000 Then ' to publish limited characters
                    'If InStr(myClusterIntro, "<br />") > 0 Then
                    '    Dim my1 As Integer = Nothing
                    '    my1 = myClusterIntro.IndexOf("<br />", 1500)
                    '    myClusterIntro = myClusterIntro.Substring(0, my1)
                    '    my1 = Nothing
                    'End If
                    'End If
                    Label1.Text = myClusterIntro
                Else
                    TR_CLUSTER_INTRO.Visible = False
                End If
                If Session.Item("LoggedMemberNo") <> "" And Session("LoggedLibCode") <> "" Then
                    CLUSTER_ADDRESS = ""
                Else
                    CLUSTER_NAME = ""
                    CLUSTER_ADDRESS = dtCluster.Rows(0).Item("ADDRESS").ToString
                End If
            Else
                TR_CLUSTER_INTRO.Visible = False
            End If

        Catch s As Exception

        Finally
            SqlConn.Close()
        End Try
    End Sub
    Public Sub GetLibraryDetails()
        Dim dtLibrary As DataTable = Nothing
        Try
            If OPACLibCode <> "" Then
                Dim ds As New DataSet
                Dim SQL As String = Nothing
                SQL = "SELECT * FROM LIBRARIES WHERE (LIB_CODE ='" & Trim(OPACLibCode) & "'); "
                Dim da As New SqlDataAdapter(SQL, SqlConn)
                da.Fill(ds)

                If ds.Tables.Count > 0 Then
                    dtLibrary = ds.Tables(0)
                Else
                    Exit Sub
                End If

                If dtLibrary.Rows.Count <> 0 Then
                    If dtLibrary.Rows(0).Item("LIB_INTRO").ToString <> "" Then
                        TR_CLUSTER_INTRO.Visible = True
                        myLibIntro = dtLibrary.Rows(0).Item("LIB_INTRO").ToString.Replace(Environment.NewLine, "<br />")

                        'If myLibIntro.Length > 2000 Then
                        '    If InStr(myLibIntro, "<br />") > 0 Then
                        '        Dim my1 As Integer = Nothing
                        '        my1 = myLibIntro.IndexOf("<br />", 1500)
                        '        myLibIntro = myLibIntro.Substring(0, my1)
                        '        my1 = Nothing
                        '    End If
                        'End If
                        Label1.Text = myLibIntro
                    Else
                        TR_CLUSTER_INTRO.Visible = False
                    End If
                    LIBRARY_NAME = dtLibrary.Rows(0).Item("LIB_NAME").ToString

                    If dtLibrary.Rows(0).Item("LIB_INCHARGE").ToString <> "" Then
                        CONTACT_PERSON = dtLibrary.Rows(0).Item("LIB_INCHARGE").ToString
                    Else
                        CONTACT_PERSON = ""
                    End If

                    If dtLibrary.Rows(0).Item("PARENT_BODY").ToString <> "" Then
                        PARENT_ORG = dtLibrary.Rows(0).Item("PARENT_BODY").ToString
                    Else
                        PARENT_ORG = ""
                    End If
                    If dtLibrary.Rows(0).Item("LIB_ADDRESS").ToString <> "" Then
                        LIBRARY_ADD = dtLibrary.Rows(0).Item("LIB_ADDRESS").ToString
                    Else
                        LIBRARY_ADD = ""
                    End If

                    If dtLibrary.Rows(0).Item("LIB_FAX").ToString <> "" Then
                        LIB_FAX = dtLibrary.Rows(0).Item("LIB_FAX").ToString
                    Else
                        LIB_FAX = ""
                    End If

                    If dtLibrary.Rows(0).Item("LIB_PHONE").ToString <> "" Then
                        LIB_PHONE = dtLibrary.Rows(0).Item("LIB_PHONE").ToString
                    Else
                        LIB_PHONE = ""
                    End If

                    If dtLibrary.Rows(0).Item("LIB_EMAIL").ToString <> "" Then
                        LIB_EMAIL = dtLibrary.Rows(0).Item("LIB_EMAIL").ToString
                    Else
                        LIB_EMAIL = ""
                    End If

                    If dtLibrary.Rows(0).Item("LIB_INCHARGE2").ToString <> "" Then
                        LIB_INCHARGE2 = dtLibrary.Rows(0).Item("LIB_INCHARGE2").ToString
                    Else
                        LIB_INCHARGE2 = ""
                    End If

                    If dtLibrary.Rows(0).Item("LIB_NAME2").ToString <> "" Then
                        LIB_NAME2 = dtLibrary.Rows(0).Item("LIB_NAME2").ToString
                    Else
                        LIB_NAME2 = ""
                    End If

                    If dtLibrary.Rows(0).Item("PARENT_BODY2").ToString <> "" Then
                        PARENT_BODY2 = dtLibrary.Rows(0).Item("PARENT_BODY2").ToString
                    Else
                        PARENT_BODY2 = ""
                    End If

                    If dtLibrary.Rows(0).Item("LIB_ADDRESS2").ToString <> "" Then
                        LIB_ADD2 = dtLibrary.Rows(0).Item("LIB_ADDRESS2").ToString
                    Else
                        LIB_ADD2 = ""
                    End If

                Else
                    TR_CLUSTER_INTRO.Visible = False
                End If
            End If
        Catch s As Exception

        Finally
            SqlConn.Close()
        End Try
    End Sub
    'Basic Search
    Protected Sub Basic_Search_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Basic_Search_Bttn.Click
        If TextBox1.Text <> "" Then
            GetResults()
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Plz Enter Search Text !');", True)
            TextBox1.Focus()
            Exit Sub
        End If
    End Sub
    'press search button event
    Public Sub GetResults()
        Dim RecordCount As Integer = Nothing
        Try
            'server validation controls for Details
            If TextBox1.Text = "" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Plz Enter Search Text !');", True)
                TextBox1.Focus()
                Exit Sub
            Else
                mySearchString = TrimAll(TextBox1.Text)
                'to remove space between words
                Do While InStr(1, mySearchString, "  ")
                    mySearchString = Replace(mySearchString, "  ", " ")
                Loop
                mySearchString = RemoveQuotes(mySearchString)
                Session("DETAILS") = mySearchString

                If mySearchString.Length >= 250 Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert(' SearchString id not of Proper Length... ');", True)
                    TextBox1.Focus()
                    Exit Sub
                End If

                'check for reserve words
                If ((InStr(1, mySearchString, "CREATE", 1) > 0) Or (InStr(1, mySearchString, "DELETE", 1) > 0) Or (InStr(1, mySearchString, "DROP", 1) > 0) Or (InStr(1, mySearchString, "track", 1) > 0) Or (InStr(1, mySearchString, "trace", 1) > 0)) Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do not user Reserve Words...');", True)
                    TextBox1.Focus()
                    Exit Sub
                End If

                'check for unwanted characters
                Dim iloop As Integer
                Dim strcurrentchar As Object
                Dim c As Integer
                Dim counter1 As Integer
                c = 0
                counter1 = 0
                For iloop = 1 To Len(mySearchString)
                    strcurrentchar = Mid(mySearchString, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter1 = 1
                        End If
                    End If
                Next
                If counter1 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do not use un-wanted characters..... ');", True)
                    TextBox1.Focus()
                    Exit Sub
                End If
            End If

            'split the search string if more than one word
            Dim SQL As String = Nothing
            Dim str1 As Object
            SQL = "SELECT CAT_NO, TITLE = CASE(ISNULL (SUB_TITLE,'')) WHEN '' THEN TITLE ELSE (TITLE +': '+SUB_TITLE) END,  LANG_CODE, BIB_CODE, MAT_CODE, DOC_TYPE_CODE, PLACE_OF_PUB, YEAR_OF_PUB, CON_CODE, SUB_NAME, PUB_NAME, AUTHOR1, AUTHOR2, AUTHOR3 FROM CATS_AUTHORS_VIEW WHERE  (BIB_CODE ='M') AND (CAT_LEVEL ='Full') "
            If InStr(mySearchString, " ") <> 0 Then
                str1 = Split(mySearchString, " ")
                SQL = SQL & " AND ((TITLE  like N'%" & Trim(str1(0)) & "%' or SUB_TITLE like N'%" & Trim(str1(0)) & "%' or SERIES_TITLE like N'%" & Trim(str1(0)) & "%' or KEYWORDS like N'%" & Trim(str1(0)) & "%' or TAGS like N'%" & Trim(str1(0)) & "%') "
                For I = 1 To str1.Length - 1
                    SQL = SQL & " AND (TITLE  like N'%" & str1(I) & "%' OR SUB_TITLE  like N'%" & str1(I) & "%' OR SERIES_TITLE  like N'%" & str1(I) & "%' OR  KEYWORDS like N'%" & Trim(str1(I)) & "%' OR TAGS like N'%" & Trim(str1(I)) & "%')"
                Next
                SQL = SQL & ")"

            Else
                str1 = Trim(mySearchString)
                SQL = SQL & " AND  (TITLE  like N'%" & Trim(str1) & "%' or SUB_TITLE like N'%" & Trim(str1) & "%' or SERIES_TITLE like N'%" & Trim(str1) & "%' or KEYWORDS like N'%" & Trim(str1) & "%' or TAGS like N'%" & Trim(str1) & "%') "
            End If

            If Session("LoggedLibCode") <> "" Then
                SQL = SQL & " AND (CAT_NO IN (SELECT CAT_NO FROM HOLDINGS WHERE (LIB_CODE = N'" & Trim(Session("LoggedLibCode")) & "'))) "
            End If

            SQL = SQL & " ORDER BY TITLE "

            SqlConn.Open()

            Dim objCommand As New SqlCommand(SQL, SqlConn)
            Dim da As New SqlDataAdapter(objCommand)
            Dim ds As New DataSet

            da.Fill(ds)
            If ds.Tables.Count > 0 Then
                dt = ds.Tables(0)
            Else
                Exit Sub
            End If

            If dt.Rows.Count > 0 Then
                RecordCount = dt.Rows.Count
                GridView1.DataSource = dt
                GridView1.DataBind()
                GridView1.Caption = "Total Records : " & RecordCount

                Label3.Text = "Filter Records"
                Label5.Text = "Basic Search"
                TR_CLUSTER_INTRO.Visible = False
                TR_EG_INTRO.Visible = False
                TR_TRANSACTIONS.Visible = False

                SelectDistinctLang()
                Languages()
                SelectDistinctYear()
                Years()
                SelectDistinctdocument()
                Documents()
                SelectDistinctCountry()
                Countries()
                SelectDistinctSubject()
                Subjects()
                SelectDistinctPlace()
                Places()
                SelectDistinctPublisher()
                Publishers()
                TR_NEWS.Visible = False
                TR_RB.Visible = False 'recent arrivals
            Else
                TR_CLUSTER_INTRO.Visible = True
                TR_EG_INTRO.Visible = True
                TR_RB.Visible = True 'recent arrivals
                TR_TRANSACTIONS.Visible = False

                GridView1.DataSource = Nothing
                GridView1.DataBind()
                CheckBoxList1.ClearSelection()
                CheckBoxList2.ClearSelection()
                CheckBoxList3.ClearSelection()
                CheckBoxList4.ClearSelection()
                CheckBoxList5.ClearSelection()
                CheckBoxList6.ClearSelection()
                CheckBoxList7.ClearSelection()

                CheckBoxList1.Items.Clear()
                CheckBoxList2.Items.Clear()
                CheckBoxList3.Items.Clear()
                CheckBoxList4.Items.Clear()
                CheckBoxList5.Items.Clear()
                CheckBoxList6.Items.Clear()
                CheckBoxList7.Items.Clear()

                TR_LANG.Visible = False
                TR_YEAR.Visible = False
                TR_DOC.Visible = False
                TR_COUNTRY.Visible = False
                TR_SUB.Visible = False
                TR_PLACE.Visible = False
                TR_PUB.Visible = False

                TR_NEWS.Visible = True
                TextBox1.Focus()
            End If
            ViewState("dt") = dt
        Catch s As Exception
           
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
    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        Try
            Dim SelLangCode As Object
            Dim mySelection As Object
            mySelection = ""
            For z = 0 To CheckBoxList1.Items.Count - 1
                If CheckBoxList1.Items(z).Selected = True Then
                    SelLangCode = CheckBoxList1.Items(z).Value
                    If Trim(mySelection.ToString) <> String.Empty Then
                        mySelection = mySelection & " OR "
                    End If
                    mySelection = mySelection & " LANG_CODE = '" & SelLangCode & "'"
                End If
            Next

            'year
            If mySelection = "" Then
                Dim SelYear As Object
                mySelection = ""
                For z = 0 To CheckBoxList2.Items.Count - 1
                    If CheckBoxList2.Items(z).Selected = True Then
                        SelYear = CheckBoxList2.Items(z).Value
                        If Trim(mySelection.ToString) <> String.Empty Then
                            mySelection = mySelection & " OR "
                        End If
                        mySelection = mySelection & " YEAR_OF_PUB = '" & SelYear & "'"
                    End If
                Next
            End If

            'Document
            If mySelection = "" Then
                Dim Seldoc As Object
                mySelection = ""
                For z = 0 To CheckBoxList3.Items.Count - 1
                    If CheckBoxList3.Items(z).Selected = True Then
                        Seldoc = CheckBoxList3.Items(z).Value
                        If Trim(mySelection.ToString) <> String.Empty Then
                            mySelection = mySelection & " OR "
                        End If
                        mySelection = mySelection & " DOC_TYPE_CODE = '" & Seldoc & "'"
                    End If
                Next
            End If

            'Country
            If mySelection = "" Then
                Dim Selcon As Object
                mySelection = ""
                For z = 0 To CheckBoxList4.Items.Count - 1
                    If CheckBoxList4.Items(z).Selected = True Then
                        Selcon = CheckBoxList4.Items(z).Value
                        If Trim(mySelection.ToString) <> String.Empty Then
                            mySelection = mySelection & " OR "
                        End If
                        mySelection = mySelection & " CON_CODE = '" & Selcon & "'"
                    End If
                Next
            End If

            'Subject
            If mySelection = "" Then
                Dim Selsub As Object
                mySelection = ""
                For z = 0 To CheckBoxList5.Items.Count - 1
                    If CheckBoxList5.Items(z).Selected = True Then
                        Selsub = CheckBoxList5.Items(z).Value
                        If Trim(mySelection.ToString) <> String.Empty Then
                            mySelection = mySelection & " OR "
                        End If
                        mySelection = mySelection & " SUB_NAME = '" & Selsub & "'"
                    End If
                Next
            End If

            'Publisher
            If mySelection = "" Then
                Dim Selpub As Object
                mySelection = ""
                For z = 0 To CheckBoxList7.Items.Count - 1
                    If CheckBoxList7.Items(z).Selected = True Then
                        Selpub = CheckBoxList7.Items(z).Value
                        If Trim(mySelection.ToString) <> String.Empty Then
                            mySelection = mySelection & " OR "
                        End If
                        mySelection = mySelection & " SUB_NAME = '" & Selpub & "'"
                    End If
                Next
            End If

            If mySelection <> "" Then
                ViewState("dt").DefaultView.RowFilter = mySelection
                ViewState("Test") = ViewState("dt")
            Else
                ViewState("Test") = ViewState("dt")
            End If

            'rebind datagrid
            GridView1.DataSource = ViewState("Test") 'temp
            GridView1.PageIndex = e.NewPageIndex
            index = e.NewPageIndex * GridView1.PageSize
            GridView1.DataBind()
            GridView1.Visible = True
        Catch s As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' there is error ..aa');", True)
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
    Protected Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        Dim temp As DataTable = CType(ViewState("dt"), DataTable)
        GridViewSortExpression = e.SortExpression
        GridView1.DataSource = temp
        Dim pageIndex As Integer = GridView1.PageIndex
        GridView1.DataSource = SortDataTable(GridView1.DataSource, False)
        GridView1.DataBind()
        GridView1.PageIndex = pageIndex
    End Sub
    'filter distinct Lang  from dt table
    Public Function SelectDistinctLang() As DataTable
        Try
            Dim ds1 As DataSet
            dtLang.Columns.Add("LANG_CODE", dt.Columns("LANG_CODE").DataType)
            Dim dr As DataRow, LastValue As Object
            For Each dr In dt.Select("", "LANG_CODE ASC")
                If LastValue Is Nothing OrElse Not ColumnEqual(LastValue, dr("LANG_CODE")) Then
                    LastValue = dr("LANG_CODE")
                    dtLang.Rows.Add(New Object() {LastValue})
                End If
            Next
            If Not ds1 Is Nothing Then ds1.Tables.Add(dtLang)
            Return dtLang
        Catch s As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' there is error .._Language');", True)
        End Try
    End Function
    'bind language list
    Public Sub Languages()
        Try
            CheckBoxList1.DataSource = dtLang
            CheckBoxList1.DataTextField = "LANG_CODE"
            CheckBoxList1.DataValueField = "LANG_CODE"
            CheckBoxList1.DataBind()
            For z = 0 To dtLang.Rows.Count - 1
                CheckBoxList1.Items(z).Selected = False
            Next
            TR_LANG.Visible = True
        Catch s As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' there is error in _Language');", True)
        End Try
    End Sub
    'filter distinct Year  from dt table
    Public Function SelectDistinctYear() As DataTable
        Try
            Dim ds1 As DataSet
            dtYear.Columns.Add("YEAR_OF_PUB", dt.Columns("YEAR_OF_PUB").DataType)
            Dim dr As DataRow, LastValue As Object

            For Each dr In dt.Select("", "YEAR_OF_PUB DESC")
                If LastValue Is Nothing OrElse Not ColumnEqual(LastValue, dr("YEAR_OF_PUB")) Then
                    LastValue = dr("YEAR_OF_PUB")
                    If Trim(LastValue.ToString) <> String.Empty Then
                        dtYear.Rows.Add(New Object() {LastValue})
                        'Else
                        'dtYear.Rows.Add(New Object() {"0000"})
                    End If
                End If
            Next
            If Not ds1 Is Nothing Then ds1.Tables.Add(dtYear)
            Return dtYear
        Catch s As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' there is error ..Year');", True)
        End Try
    End Function
    'bind year lsit
    Public Sub Years()
        Try
            CheckBoxList2.DataSource = dtYear
            CheckBoxList2.DataTextField = "YEAR_OF_PUB"
            CheckBoxList2.DataValueField = "YEAR_OF_PUB"
            CheckBoxList2.DataBind()
            For z = 0 To dtYear.Rows.Count - 1
                CheckBoxList2.Items(z).Selected = False
            Next
            TR_YEAR.Visible = True
        Catch s As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' there is error ..Year');", True)
        End Try
    End Sub
    'get document name
    Public Function getdocument(ByVal value As Object) As String
        Dim fn As String = ""
        Select Case value
            Case "AB"
                fn = "Autobiography"
            Case "AI"
                fn = "Abstracting and Indexing Services"
            Case "AM"
                fn = "Almanacs"
            Case "AN"
                fn = "Annuals (General)"
            Case "AR"
                fn = "Annual Reports"
            Case "AT"
                fn = "Atlas"
            Case "BB"
                fn = "Bibliographies"
            Case "BK"
                fn = "General Books"
            Case "CB"
                fn = "Composite Books"
            Case "CP"
                fn = "Conference Proceedings"
            Case "DR"
                fn = "Directories"
            Case "DS"
                fn = "Dissertations"
            Case "DT"
                fn = "Dictionaries"
            Case "EB"
                fn = "Edited Books"
            Case "EN"
                fn = "Encyclopedias"
            Case "GB"
                fn = "Government Publications"
            Case "GR"
                fn = "General Reports"
            Case "HB"
                fn = "Hand Books"
            Case "JR"
                fn = "Journals"
            Case "MG"
                fn = "Magazines"
            Case "MN"
                fn = "Manuals"
            Case "MP"
                fn = "Maps"
            Case "MV"
                fn = "Multi-Volume Books"
            Case "NL"
                fn = "Newsletters"
            Case "NP"
                fn = "Newspapers"
            Case "PE"
                fn = "Patents"
            Case "PR"
                fn = "Project Reports"
            Case "RR"
                fn = "Research Reports"
            Case "RT"
                fn = "Reprints"
            Case "ST"
                fn = "Standards"
            Case "TH"
                fn = "Thesaureses"
            Case "TR"
                fn = "Technical Reports"
            Case "TS"
                fn = "Theses"
            Case "YB"
                fn = "Year Books"
            Case Else
                fn = "General Books"
        End Select
        Return fn
    End Function
    'filter distinct document type  from dt table
    Public Function SelectDistinctdocument() As DataTable
        Try
            Dim ds1 As DataSet
            dtDocument.Columns.Add("DOC_TYPE_CODE", dt.Columns("DOC_TYPE_CODE").DataType)
            dtDocument.Columns.Add("DOC_TYPE_NAME", Type.GetType("System.String"))
            Dim dr As DataRow, LastValue As Object
            For Each dr In dt.Select("", "DOC_TYPE_CODE ASC")
                If LastValue Is Nothing OrElse Not ColumnEqual(LastValue, dr("DOC_TYPE_CODE")) Then
                    LastValue = dr("DOC_TYPE_CODE")
                    Dim dn As String
                    dn = getdocument(LastValue)
                    dtDocument.Rows.Add(LastValue, dn)
                End If
            Next
            If Not ds1 Is Nothing Then ds1.Tables.Add(dtDocument)
            Return dtDocument
        Catch s As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' there is error ..oo');", True)
        End Try
    End Function
    'bind Document lsit
    Public Sub Documents()
        Try
            CheckBoxList3.DataSource = dtDocument
            CheckBoxList3.DataTextField = "DOC_TYPE_NAME"
            CheckBoxList3.DataValueField = "DOC_TYPE_CODE"
            CheckBoxList3.DataBind()
            For z = 0 To dtDocument.Rows.Count - 1
                CheckBoxList3.Items(z).Selected = False
            Next
            TR_DOC.Visible = True
        Catch s As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' there is error ..pp');", True)
        End Try
    End Sub
    'filter distinct Countries  from dt table
    Public Function SelectDistinctCountry() As DataTable
        Try
            Dim ds1 As DataSet
            dtCountry.Columns.Add("CON_CODE", dt.Columns("CON_CODE").DataType)
            Dim dr As DataRow, LastValue As Object
            For Each dr In dt.Select("", "CON_CODE ASC")
                If LastValue Is Nothing OrElse Not ColumnEqual(LastValue, dr("CON_CODE")) Then
                    LastValue = dr("CON_CODE")
                    If Trim(LastValue.ToString) <> String.Empty Then
                        dtCountry.Rows.Add(New Object() {LastValue})
                        'Else
                        'dtCountry.Rows.Add(New Object() {"NA"})
                    End If
                End If
            Next
            If Not ds1 Is Nothing Then ds1.Tables.Add(dtCountry)
            Return dtCountry
        Catch s As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' there is error ..qq');", True)
        End Try
    End Function
    'bind Countries list
    Public Sub Countries()
        Try
            CheckBoxList4.DataSource = dtCountry
            CheckBoxList4.DataTextField = "CON_CODE"
            CheckBoxList4.DataValueField = "CON_CODE"
            CheckBoxList4.DataBind()
            For z = 0 To dtCountry.Rows.Count - 1
                CheckBoxList4.Items(z).Selected = False
            Next
            TR_COUNTRY.Visible = True
        Catch s As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' there is error country..rr');", True)
        End Try
    End Sub
    'filter distinct Subjects  from dt table
    Public Function SelectDistinctSubject() As DataTable
        Try
            Dim ds1 As DataSet
            dtSubject.Columns.Add("SUB_NAME", dt.Columns("SUB_NAME").DataType)
            Dim dr As DataRow, LastValue As Object
            For Each dr In dt.Select("", "SUB_NAME ASC")
                If LastValue Is Nothing OrElse Not ColumnEqual(LastValue, dr("SUB_NAME")) Then
                    LastValue = dr("SUB_NAME")
                    If Trim(LastValue.ToString) <> String.Empty Then
                        dtSubject.Rows.Add(New Object() {LastValue})
                        'Else
                        'dtSubject.Rows.Add(New Object() {"Not Available"})
                    End If

                End If
            Next
            If Not ds1 Is Nothing Then ds1.Tables.Add(dtSubject)
            Return dtSubject
        Catch s As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' there is error .subject.ss');", True)
        End Try
    End Function
    'bind Subjects list
    Public Sub Subjects()
        Try
            CheckBoxList5.DataSource = dtSubject
            CheckBoxList5.DataTextField = "SUB_NAME"
            CheckBoxList5.DataValueField = "SUB_NAME"
            CheckBoxList5.DataBind()
            For z = 0 To dtSubject.Rows.Count - 1
                CheckBoxList5.Items(z).Selected = False
            Next
            TR_SUB.Visible = True
        Catch s As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' there is error .subject.tt');", True)
        End Try
    End Sub
    'filter distinct Subjects  from dt table
    Public Function SelectDistinctPlace() As DataTable
        Try
            Dim ds1 As DataSet
            dtPlace.Columns.Add("PLACE_OF_PUB", dt.Columns("PLACE_OF_PUB").DataType)
            Dim dr As DataRow, LastValue As Object
            For Each dr In dt.Select("", "PLACE_OF_PUB ASC")
                If LastValue Is Nothing OrElse Not ColumnEqual(LastValue, dr("PLACE_OF_PUB")) Then
                    LastValue = dr("PLACE_OF_PUB")
                    If Trim(LastValue.ToString) <> String.Empty Then
                        dtPlace.Rows.Add(New Object() {LastValue})
                        'Else
                        'dtSubject.Rows.Add(New Object() {"Not Available"})
                    End If

                End If
            Next
            If Not ds1 Is Nothing Then ds1.Tables.Add(dtPlace)
            Return dtPlace
        Catch s As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' there is error .Place.ss');", True)
        End Try
    End Function
    'bind Subjects list
    Public Sub Places()
        Try
            CheckBoxList6.DataSource = dtPlace
            CheckBoxList6.DataTextField = "PLACE_OF_PUB"
            CheckBoxList6.DataValueField = "PLACE_OF_PUB"
            CheckBoxList6.DataBind()
            For z = 0 To dtPlace.Rows.Count - 1
                CheckBoxList6.Items(z).Selected = False
            Next
            TR_PLACE.Visible = True
        Catch s As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' there is error .Place.tt');", True)
        End Try
    End Sub
    'filter distinct Subjects  from dt table
    Public Function SelectDistinctPublisher() As DataTable
        Try
            Dim ds1 As DataSet
            dtPub.Columns.Add("PUB_NAME", dt.Columns("PUB_NAME").DataType)
            Dim dr As DataRow, LastValue As Object
            For Each dr In dt.Select("", "PUB_NAME ASC")
                If LastValue Is Nothing OrElse Not ColumnEqual(LastValue, dr("PUB_NAME")) Then
                    LastValue = dr("PUB_NAME")
                    If Trim(LastValue.ToString) <> String.Empty Then
                        dtPub.Rows.Add(New Object() {LastValue})
                        'Else
                        'dtSubject.Rows.Add(New Object() {"Not Available"})
                    End If

                End If
            Next
            If Not ds1 Is Nothing Then ds1.Tables.Add(dtPub)
            Return dtPub
        Catch s As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' there is error .Place.ss');", True)
        End Try
    End Function
    'bind Subjects list
    Public Sub Publishers()
        Try
            CheckBoxList7.DataSource = dtPub
            CheckBoxList7.DataTextField = "PUB_NAME"
            CheckBoxList7.DataValueField = "PUB_NAME"
            CheckBoxList7.DataBind()
            For z = 0 To dtPub.Rows.Count - 1
                CheckBoxList7.Items(z).Selected = False
            Next
            TR_PUB.Visible = True
        Catch s As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' there is error .Place.tt');", True)
        End Try
    End Sub
    Function Highlight(ByVal strText As Object, ByVal DETAILS As Object, ByVal strBefore As Object, ByVal strAfter As Object) As String
        Dim i, j As Integer
        Dim strSearch As Object
        Dim hx, hx1 As Object
        hx = ""
        hx1 = ""
        strSearch = Split(Trim(DETAILS), " ")
        Highlight = strText
        hx = Split(Trim(Highlight), " ")
        For i = 0 To (UBound(hx))
            Dim flag = 0
            For j = 0 To (UBound(strSearch))
                If UCase(hx(i)) <> "AND" Or UCase(hx(i)) <> "OR" Or UCase(hx(i)) <> "NOT" Then
                    If UCase(hx(i)).ToString.Contains(UCase(strSearch(j)).ToString) = True Then
                        hx1 = hx1 + strBefore + hx(i) + " " + strAfter
                        flag = 1
                    End If
                End If
            Next
            If flag = 0 Then
                hx1 = hx1 + hx(i) + " "
            End If
        Next
        Highlight = Trim(hx1)
    End Function
    Function Highlight1(ByVal strText As Object, ByVal DETAILS As Object, ByVal strBefore As Object, ByVal strAfter As Object) As String
        Dim nPos As Object
        Dim nLen As Integer
        Dim nLenAll As Integer

        nLen = Len(DETAILS)
        nLenAll = nLen + Len(strBefore) + Len(strAfter) + 1
        Highlight1 = strText
        strFind = DETAILS

        If nLen > 0 And Len(Highlight1) > 0 Then

            nPos = InStr(1, Highlight1, strFind, 1)

            Do While nPos > 0
                Highlight1 = Left(Highlight1, nPos - 1) & _
                    strBefore & Mid(Highlight1, nPos, nLen) & strAfter & _
                                Mid(Highlight1, nPos + nLen)
                nPos = InStr(nPos + nLenAll, Highlight1, DETAILS, 1)
            Loop
        End If
    End Function
    'checklist1 event lang
    Protected Sub CheckBoxList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxList1.SelectedIndexChanged
        Try
            'check another checklistboxes
            For z = 0 To CheckBoxList2.Items.Count - 1
                CheckBoxList2.Items(z).Selected = False
            Next
            For z = 0 To CheckBoxList3.Items.Count - 1
                CheckBoxList3.Items(z).Selected = False
            Next
            For z = 0 To CheckBoxList4.Items.Count - 1
                CheckBoxList4.Items(z).Selected = False
            Next
            For z = 0 To CheckBoxList5.Items.Count - 1
                CheckBoxList5.Items(z).Selected = False
            Next
            For z = 0 To CheckBoxList6.Items.Count - 1
                CheckBoxList6.Items(z).Selected = False
            Next
            For z = 0 To CheckBoxList7.Items.Count - 1
                CheckBoxList7.Items(z).Selected = False
            Next

            Dim SelLangCode As Object
            Dim mySelection As Object
            mySelection = ""
            For z = 0 To CheckBoxList1.Items.Count - 1
                If CheckBoxList1.Items(z).Selected = True Then
                    SelLangCode = CheckBoxList1.Items(z).Value
                    If Trim(mySelection.ToString) <> String.Empty Then
                        mySelection = mySelection & " OR "
                    End If
                    mySelection = mySelection & " LANG_CODE = '" & SelLangCode & "'"
                End If
            Next

            If mySelection <> "" Then
                ViewState("dt").DefaultView.RowFilter = mySelection
                ViewState("Test") = ViewState("dt")

                GridView1.DataSource = ViewState("Test")
                GridView1.DataBind()
                RecordCount = ViewState("Test").DefaultView.Count
                GridView1.Caption = "Total Records : " & RecordCount
            Else
                GridView1.DataSource = ViewState("dt")
                GridView1.DataBind()
                RecordCount = ViewState("dt").DefaultView.Count
                GridView1.Caption = "Total Records : " & RecordCount
            End If
            Label3.Text = "Filter Records"
        Catch s As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' there is error ..lang');", True)
        End Try
    End Sub
    'checklist2 event (year)
    Protected Sub CheckBoxList2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxList2.SelectedIndexChanged
        Try
            'check all lang
            For z = 0 To CheckBoxList1.Items.Count - 1
                CheckBoxList1.Items(z).Selected = False
            Next
            For z = 0 To CheckBoxList3.Items.Count - 1
                CheckBoxList3.Items(z).Selected = False
            Next
            For z = 0 To CheckBoxList4.Items.Count - 1
                CheckBoxList4.Items(z).Selected = False
            Next
            For z = 0 To CheckBoxList5.Items.Count - 1
                CheckBoxList5.Items(z).Selected = False
            Next
            For z = 0 To CheckBoxList6.Items.Count - 1
                CheckBoxList6.Items(z).Selected = False
            Next
            For z = 0 To CheckBoxList7.Items.Count - 1
                CheckBoxList7.Items(z).Selected = False
            Next

            Dim SelYear As Object
            Dim mySelection As Object
            mySelection = ""
            For z = 0 To CheckBoxList2.Items.Count - 1
                If CheckBoxList2.Items(z).Selected = True Then
                    SelYear = CheckBoxList2.Items(z).Value
                    If Trim(mySelection.ToString) <> String.Empty Then
                        mySelection = mySelection & " OR "
                    End If
                    mySelection = mySelection & " YEAR_OF_PUB = '" & SelYear & "'"
                End If
            Next

            If mySelection <> "" Then
                ViewState("dt").DefaultView.RowFilter = mySelection
                ViewState("Test") = ViewState("dt")

                GridView1.DataSource = ViewState("Test")
                GridView1.DataBind()
                RecordCount = ViewState("Test").DefaultView.Count
                GridView1.Caption = "Total Records : " & RecordCount
            Else
                GridView1.DataSource = ViewState("dt")
                GridView1.DataBind()
                RecordCount = ViewState("dt").DefaultView.Count
                GridView1.Caption = "Total Records : " & RecordCount
            End If
            Label3.Text = "Filter Records"
        Catch s As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' there is error ..cc');", True)
        End Try
    End Sub
    'checklist3 event (document)
    Protected Sub CheckBoxList3_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxList3.SelectedIndexChanged
        Try
            'check all lang
            For z = 0 To CheckBoxList1.Items.Count - 1
                CheckBoxList1.Items(z).Selected = False
            Next
            For z = 0 To CheckBoxList2.Items.Count - 1
                CheckBoxList2.Items(z).Selected = False
            Next
            For z = 0 To CheckBoxList4.Items.Count - 1
                CheckBoxList4.Items(z).Selected = False
            Next
            For z = 0 To CheckBoxList5.Items.Count - 1
                CheckBoxList5.Items(z).Selected = False
            Next
            For z = 0 To CheckBoxList6.Items.Count - 1
                CheckBoxList6.Items(z).Selected = False
            Next
            For z = 0 To CheckBoxList7.Items.Count - 1
                CheckBoxList7.Items(z).Selected = False
            Next

            Dim SelDocument As Object
            Dim mySelection As Object
            mySelection = ""
            For x = 0 To CheckBoxList3.Items.Count - 1
                If CheckBoxList3.Items(x).Selected = True Then
                    SelDocument = CheckBoxList3.Items(x).Value
                    If Trim(mySelection.ToString) <> String.Empty Then
                        mySelection = mySelection & " OR "
                    End If
                    mySelection = mySelection & " DOC_TYPE_CODE = '" & SelDocument & "'"
                End If
            Next

            If mySelection <> "" Then
                ViewState("dt").DefaultView.RowFilter = mySelection
                ViewState("Test") = ViewState("dt")

                GridView1.DataSource = ViewState("Test")
                GridView1.DataBind()
                RecordCount = ViewState("Test").DefaultView.Count
                GridView1.Caption = "Total Records : " & RecordCount
            Else
                GridView1.DataSource = ViewState("dt")
                GridView1.DataBind()
                RecordCount = ViewState("dt").DefaultView.Count
                GridView1.Caption = "Total Records : " & RecordCount
            End If
            Label3.Text = "Filter Records"
        Catch s As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' there is error ..dd');", True)
        End Try
    End Sub
    'checklist5 event (Country type)
    Protected Sub CheckBoxList4_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxList4.SelectedIndexChanged
        Try
            'check all Lang
            For z = 0 To CheckBoxList1.Items.Count - 1
                CheckBoxList1.Items(z).Selected = False
            Next
            For z = 0 To CheckBoxList2.Items.Count - 1
                CheckBoxList2.Items(z).Selected = False
            Next
            For z = 0 To CheckBoxList3.Items.Count - 1
                CheckBoxList3.Items(z).Selected = False
            Next
            For z = 0 To CheckBoxList5.Items.Count - 1
                CheckBoxList5.Items(z).Selected = False
            Next
            For z = 0 To CheckBoxList6.Items.Count - 1
                CheckBoxList6.Items(z).Selected = False
            Next
            For z = 0 To CheckBoxList7.Items.Count - 1
                CheckBoxList7.Items(z).Selected = False
            Next

            Dim SelCountry As Object
            Dim mySelection As Object
            mySelection = ""
            For x = 0 To CheckBoxList4.Items.Count - 1
                If CheckBoxList4.Items(x).Selected = True Then
                    SelCountry = CheckBoxList4.Items(x).Value
                    If SelCountry = "NA" Then
                        SelCountry = ""
                    End If
                    If Trim(mySelection.ToString) <> String.Empty Then
                        mySelection = mySelection & " OR "
                    End If
                    mySelection = mySelection & " CON_CODE = '" & SelCountry & "'"
                End If
            Next

            If mySelection <> "" Then
                ViewState("dt").DefaultView.RowFilter = mySelection
                ViewState("Test") = ViewState("dt")

                GridView1.DataSource = ViewState("Test")
                GridView1.DataBind()
                RecordCount = ViewState("Test").DefaultView.Count
                GridView1.Caption = "Total Records : " & RecordCount
            Else
                GridView1.DataSource = ViewState("dt")
                GridView1.DataBind()
                RecordCount = ViewState("dt").DefaultView.Count
                GridView1.Caption = "Total Records : " & RecordCount
            End If
            Label3.Text = "Filter Records"
        Catch s As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' there is an error Country..');", True)
        End Try
    End Sub
    'checklist5 event (Subject type)
    Protected Sub CheckBoxList5_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxList5.SelectedIndexChanged
        Try
            'check all lang
            For z = 0 To CheckBoxList1.Items.Count - 1
                CheckBoxList1.Items(z).Selected = False
            Next
            For z = 0 To CheckBoxList2.Items.Count - 1
                CheckBoxList2.Items(z).Selected = False
            Next
            For z = 0 To CheckBoxList3.Items.Count - 1
                CheckBoxList3.Items(z).Selected = False
            Next
            For z = 0 To CheckBoxList4.Items.Count - 1
                CheckBoxList4.Items(z).Selected = False
            Next
            For z = 0 To CheckBoxList6.Items.Count - 1
                CheckBoxList6.Items(z).Selected = False
            Next
            For z = 0 To CheckBoxList7.Items.Count - 1
                CheckBoxList7.Items(z).Selected = False
            Next


            Dim SelSubject As Object
            Dim mySelection As Object
            mySelection = ""
            For x = 0 To CheckBoxList5.Items.Count - 1
                If CheckBoxList5.Items(x).Selected = True Then
                    SelSubject = CheckBoxList5.Items(x).Value
                    If Trim(mySelection.ToString) <> String.Empty Then
                        mySelection = mySelection & " OR "
                    End If
                    mySelection = mySelection & " SUB_NAME = '" & SelSubject & "'"
                End If
            Next

            If mySelection <> "" Then
                ViewState("dt").DefaultView.RowFilter = mySelection
                ViewState("Test") = ViewState("dt")

                GridView1.DataSource = ViewState("Test")
                GridView1.DataBind()
                RecordCount = ViewState("Test").DefaultView.Count
                GridView1.Caption = "Total Records : " & RecordCount
            Else
                GridView1.DataSource = ViewState("dt")
                GridView1.DataBind()
                RecordCount = ViewState("dt").DefaultView.Count
                GridView1.Caption = "Total Records : " & RecordCount
            End If
            Label3.Text = "Filter Records"
        Catch s As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' there is an error ..');", True)
        End Try
    End Sub
    'checklist6 event (Place wise)
    Protected Sub CheckBoxList6_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxList6.SelectedIndexChanged
        Try
            'check all lang
            For z = 0 To CheckBoxList1.Items.Count - 1
                CheckBoxList1.Items(z).Selected = False
            Next
            For z = 0 To CheckBoxList2.Items.Count - 1
                CheckBoxList2.Items(z).Selected = False
            Next
            For z = 0 To CheckBoxList3.Items.Count - 1
                CheckBoxList3.Items(z).Selected = False
            Next
            For z = 0 To CheckBoxList4.Items.Count - 1
                CheckBoxList4.Items(z).Selected = False
            Next
            For z = 0 To CheckBoxList5.Items.Count - 1
                CheckBoxList5.Items(z).Selected = False
            Next
            For z = 0 To CheckBoxList7.Items.Count - 1
                CheckBoxList7.Items(z).Selected = False
            Next


            Dim SelPlace As Object
            Dim mySelection As Object
            mySelection = ""
            For x = 0 To CheckBoxList6.Items.Count - 1
                If CheckBoxList6.Items(x).Selected = True Then
                    SelPlace = CheckBoxList6.Items(x).Value
                    If Trim(mySelection.ToString) <> String.Empty Then
                        mySelection = mySelection & " OR "
                    End If
                    mySelection = mySelection & " PLACE_OF_PUB = '" & SelPlace & "'"
                End If
            Next

            If mySelection <> "" Then
                ViewState("dt").DefaultView.RowFilter = mySelection
                ViewState("Test") = ViewState("dt")

                GridView1.DataSource = ViewState("Test")
                GridView1.DataBind()
                RecordCount = ViewState("Test").DefaultView.Count
                GridView1.Caption = "Total Records : " & RecordCount
            Else
                GridView1.DataSource = ViewState("dt")
                GridView1.DataBind()
                RecordCount = ViewState("dt").DefaultView.Count
                GridView1.Caption = "Total Records : " & RecordCount
            End If
            Label3.Text = "Filter Records"
        Catch s As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' there is an error ...Place..');", True)
        End Try
    End Sub
    'checklist7 event (publiser wise)
    Protected Sub CheckBoxList7_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxList7.SelectedIndexChanged
        Try
            'check all lang
            For z = 0 To CheckBoxList1.Items.Count - 1
                CheckBoxList1.Items(z).Selected = False
            Next
            For z = 0 To CheckBoxList2.Items.Count - 1
                CheckBoxList2.Items(z).Selected = False
            Next
            For z = 0 To CheckBoxList3.Items.Count - 1
                CheckBoxList3.Items(z).Selected = False
            Next
            For z = 0 To CheckBoxList4.Items.Count - 1
                CheckBoxList4.Items(z).Selected = False
            Next
            For z = 0 To CheckBoxList5.Items.Count - 1
                CheckBoxList5.Items(z).Selected = False
            Next
            For z = 0 To CheckBoxList6.Items.Count - 1
                CheckBoxList6.Items(z).Selected = False
            Next


            Dim SelPublisher As Object
            Dim mySelection As Object
            mySelection = ""
            For x = 0 To CheckBoxList7.Items.Count - 1
                If CheckBoxList7.Items(x).Selected = True Then
                    SelPublisher = CheckBoxList7.Items(x).Value
                    If Trim(mySelection.ToString) <> String.Empty Then
                        mySelection = mySelection & " OR "
                    End If
                    mySelection = mySelection & " PUB_NAME = '" & SelPublisher & "'"
                End If
            Next

            If mySelection <> "" Then
                ViewState("dt").DefaultView.RowFilter = mySelection
                ViewState("Test") = ViewState("dt")

                GridView1.DataSource = ViewState("Test")
                GridView1.DataBind()
                RecordCount = ViewState("Test").DefaultView.Count
                GridView1.Caption = "Total Records : " & RecordCount
            Else
                GridView1.DataSource = ViewState("dt")
                GridView1.DataBind()
                RecordCount = ViewState("dt").DefaultView.Count
                GridView1.Caption = "Total Records : " & RecordCount
            End If
            Label3.Text = "Filter Records"
        Catch s As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' there is an error ...Place..');", True)
        End Try
    End Sub
    Private Shared Function t() As Object
        Throw New NotImplementedException
    End Function
    Public Sub GetLatestNews()
        Dim dtNews As New DataTable
        Try
            Dim SQL As String = Nothing
            If Session("LoggedLibCode") <> "" Then
                SQL = "SELECT TOP 20 ARTICLES.ART_NO, ARTICLES.TITLE, ARTICLES.SUB_TITLE, ARTICLES.NEWS_DATE, ARTICLES.FULL_TEXT, ARTICLES.FILE_TYPE, ARTICLES.ABSTRACT, ARTICLES.URL, ARTICLES.LIB_CODE, CATS.TITLE AS SOURCE_TITLE  FROM  ARTICLES LEFT OUTER JOIN  CATS ON ARTICLES.CAT_NO = CATS.CAT_NO WHERE (ART_TYPE = 'N')  AND (ARTICLES.LIB_CODE ='" & Trim(Session("LoggedLibCode")) & "') ORDER BY NEWS_DATE DESC "
            Else
                SQL = "SELECT TOP 50 ARTICLES.ART_NO, ARTICLES.TITLE, ARTICLES.SUB_TITLE, ARTICLES.NEWS_DATE, ARTICLES.FULL_TEXT, ARTICLES.FILE_TYPE, ARTICLES.ABSTRACT, ARTICLES.URL, ARTICLES.LIB_CODE, CATS.TITLE AS SOURCE_TITLE  FROM  ARTICLES LEFT OUTER JOIN  CATS ON ARTICLES.CAT_NO = CATS.CAT_NO WHERE (ART_TYPE = 'N') ORDER BY NEWS_DATE DESC "
            End If

            If Not SqlConn.State = ConnectionState.Open Then
                SqlConn.Open()
            End If

            Dim CommandRecent As System.Data.SqlClient.SqlCommand
            Dim drRecent As System.Data.SqlClient.SqlDataReader
            CommandRecent = New System.Data.SqlClient.SqlCommand(SQL, SqlConn)

            drRecent = CommandRecent.ExecuteReader

            If (drRecent.HasRows) Then
                Label3.Text = "Latest News"
                Dim i As Integer = 1
                strData = "<table  width = " & Chr(34) & "100%" & Chr(34) & ">"
                Do While drRecent.Read()
                    Dim myImage As String = Nothing
                    Dim ART_NO As Integer = Nothing
                    ART_NO = drRecent("ART_NO").ToString
                    If Not (IsDBNull(drRecent("TITLE")) Or drRecent("TITLE").Equals("")) Then
                        Dim strURL As String = Nothing
                        Dim FilePath As String = Nothing

                        FilePath = "~/DFILES/" & drRecent("LIB_CODE").ToString & "/News/" & ART_NO & "/"
                        FilePath = Replace(FilePath, "\", "/")
                        FilePath = Replace(FilePath, "//", "/")

                        If System.IO.Directory.Exists(Server.MapPath(FilePath)) = True Then
                            Dim myDir As DirectoryInfo = New DirectoryInfo(Server.MapPath(FilePath))

                            If (myDir.EnumerateFiles().Any()) Then
                                Dim files() As String = IO.Directory.GetFiles(Server.MapPath(FilePath))
                                For Each sFile As String In files
                                    Dim FileName As String = System.IO.Path.GetFileName(sFile) ' Text &= IO.File.ReadAllText(sFile)
                                    strURL = "/DFILES/NICLIBHQ/News/" & ART_NO & "/" & FileName '"AM51357293_2014-03-10_16-48-22.pdf"

                                    Dim FileExtn As String = Nothing
                                    FileExtn = System.IO.Path.GetExtension(FileName.ToString())
                                    If LCase(FileExtn) = ".bmp" Or FileExtn = ".emf" Or FileExtn = ".exif" Or FileExtn = ".gif" Or FileExtn = ".icon" Or FileExtn = ".jpeg" Or FileExtn = ".jpg" Or FileExtn = ".png" Or FileExtn = ".tiff" Or FileExtn = ".tif" Or FileExtn = ".wmf" Then
                                        myImage = "btn_edit.gif'"
                                    ElseIf LCase(FileExtn) = ".htm" Or LCase(FileExtn) = ".html" Then
                                        myImage = "ie.bmp"
                                    ElseIf LCase(FileExtn) = ".doc" Or LCase(FileExtn) = ".docx" Then
                                        myImage = "word.png"
                                    Else
                                        myImage = "pdf.png"
                                    End If
                                Next
                            End If
                        End If

                        If Not (IsDBNull(drRecent("URL")) Or drRecent("URL").Equals("")) Then
                            If strURL <> Nothing Then
                                strData += "<tr><td bgcolor=" & Chr(34) & "#CCCCCC" & Chr(34) & " width=" & Chr(34) & "5%" & Chr(34) & "><a target=" & Chr(34) & "_new" & Chr(34) & "href ='" & strURL & "'><img src='../Images/" & myImage & "'""></img></a></td>"
                            Else
                                strData += "<tr><td bgcolor=" & Chr(34) & "#CCCCCC" & Chr(34) & " width=" & Chr(34) & "5%" & Chr(34) & "></td>"
                            End If
                            strData += "<td bgcolor=" & Chr(34) & "#CCCCCC" & Chr(34) & "><b><a target=" & Chr(34) & "_new" & Chr(34) & " href=" & Chr(34) & drRecent("URL") & Chr(34) & ">" & drRecent("TITLE") & "- " & drRecent("SUB_TITLE") & "</a></b><b>" & FormatDateTime(drRecent("NEWS_DATE"), DateFormat.LongDate) & "</b><b>; -  " & drRecent("SOURCE_TITLE") & "</b></td></tr>"
                        Else
                            If strURL <> Nothing Then
                                strData += "<tr><td bgcolor=" & Chr(34) & "#CCCCCC" & Chr(34) & " width=" & Chr(34) & "5%" & Chr(34) & "><a target=" & Chr(34) & "_new" & Chr(34) & "href ='" & strURL & "'><img src='../Images/" & myImage & "'""></img></a></td>"
                            Else
                                strData += "<tr><td bgcolor=" & Chr(34) & "#CCCCCC" & Chr(34) & " width=" & Chr(34) & "5%" & Chr(34) & "></td>"
                            End If
                            strData += "<td style='background-color:#E0E0E0;text-align: left;width:100%'><b><Font Color='blue'>" & drRecent("TITLE") & "- " & drRecent("SUB_TITLE") & "</b></font>" & "  " & FormatDateTime(drRecent("NEWS_DATE"), DateFormat.LongDate) & "; -  " & drRecent("SOURCE_TITLE") & "</td></tr>"
                        End If
                    End If
                    j += 1
                Loop
                strData += "</Table>"
            Else
                Label3.Text = ""
            End If
            drRecent.Close()
        Catch ex As Exception

        Finally
            SqlConn.Close()
        End Try
    End Sub

    'fill Grid with holdings
    Public Sub PopulateHoldingsGrid()
        Dim dtSearch As DataTable = Nothing
        Try
            Dim SQL As String = Nothing
            If Session("LoggedLibCode") <> "" Then
                SQL = "SELECT H.HOLD_ID, H.ACCESSION_NO, H.ACCESSION_DATE,  H.CAT_NO, H.STANDARD_NO, TITLE = CASE(ISNULL (SUB_TITLE,'')) WHEN '' THEN TITLE ELSE (TITLE +': '+SUB_TITLE) END, H.YEAR_OF_PUB, B.STA_NAME, H.LIB_CODE  FROM  HOLDINGS_CATS_AUTHORS_VIEW H LEFT OUTER JOIN BOOKSTATUS B ON H.STA_CODE = B.STA_CODE WHERE (BIB_CODE = 'M') AND (CAT_LEVEL = 'Full') AND (H.LIB_CODE = '" & Trim(Session("LoggedLibCode")) & "')  AND (H.DATE_ADDED >= getdate()-30  AND H.DATE_ADDED<=GetDate())  ORDER BY H.DATE_ADDED DESC"
            Else
                SQL = "SELECT H.HOLD_ID, H.ACCESSION_NO, H.ACCESSION_DATE,  H.CAT_NO, H.STANDARD_NO, TITLE = CASE(ISNULL (SUB_TITLE,'')) WHEN '' THEN TITLE ELSE (TITLE +': '+SUB_TITLE) END, H.YEAR_OF_PUB, B.STA_NAME, H.LIB_CODE  FROM  HOLDINGS_CATS_AUTHORS_VIEW H LEFT OUTER JOIN BOOKSTATUS B ON H.STA_CODE = B.STA_CODE WHERE (BIB_CODE = 'M') AND (CAT_LEVEL = 'Full')  AND (H.DATE_ADDED >= getdate()-30  AND H.DATE_ADDED<=GetDate())  ORDER BY H.DATE_ADDED DESC"
            End If
            'AND (H.DATE_ADDED >= DATEADD(month,-1,GETDATE())   AND H.DATE_ADDED<=GetDate())  ORDER BY H.DATE_ADDED DESC


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
            Else
                Grid1.Visible = True
                RecordCount = dtSearch.Rows.Count
                Grid1.DataSource = dtSearch
                Grid1.DataBind()
            End If
            ViewState("dt") = dtSearch


        Catch s As Exception

        Finally
            SqlConn.Close()
        End Try
    End Sub
    'grid view page index changing event
    Protected Sub Grid1_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles Grid1.PageIndexChanging
        'rebind datagrid
        Grid1.DataSource = ViewState("dt") 'temp
        Grid1.PageIndex = e.NewPageIndex
        index = e.NewPageIndex * Grid1.PageSize
        Grid1.DataBind()
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

    'fill Grid with holdings
    Public Sub PopulateArticlesGrid()
        Dim dtSearch As DataTable = Nothing
        Try
            Dim SQL As String = Nothing
            If Session("LoggedLibCode") <> "" Then
                SQL = "SELECT ART_NO, ART_TITLE = CASE(ISNULL (SUB_TITLE,'')) WHEN '' THEN ART_TITLE ELSE (ART_TITLE +': '+SUB_TITLE) END, TITLE as SOURCE, LIB_CODE, ART_TYPE, AUTHORS, VOL, ISSUE, PERIOD, PAGE FROM  ARTICLES_VIEW WHERE (LIB_CODE = '" & Trim(Session("LoggedLibCode")) & "') AND  (ART_TYPE <> 'N') AND (DATE_ADDED >= getdate()-90  AND DATE_ADDED<=GetDate())  ORDER BY DATE_ADDED DESC"
            Else
                SQL = "SELECT ART_NO, ART_TITLE = CASE(ISNULL (SUB_TITLE,'')) WHEN '' THEN ART_TITLE ELSE (ART_TITLE +': '+SUB_TITLE) END, TITLE as SOURCE, LIB_CODE, ART_TYPE, AUTHORS, VOL, ISSUE, PERIOD, PAGE FROM  ARTICLES_VIEW WHERE (ART_TYPE <> 'N') AND (DATE_ADDED >= getdate()-90  AND DATE_ADDED<=GetDate())  ORDER BY DATE_ADDED DESC"
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
                Me.Grid2.DataSource = Nothing
                Grid2.DataBind()
            Else
                Grid2.Visible = True
                RecordCount = dtSearch.Rows.Count
                Grid2.DataSource = dtSearch
                Grid2.DataBind()
            End If
            ViewState("dtArticles") = dtSearch


        Catch s As Exception

        Finally
            SqlConn.Close()
        End Try
    End Sub
    'grid view page index changing event
    Protected Sub Grid2_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles Grid2.PageIndexChanging
        'rebind datagrid
        Grid2.DataSource = ViewState("dtArticles") 'temp
        Grid2.PageIndex = e.NewPageIndex
        index = e.NewPageIndex * Grid2.PageSize
        Grid2.DataBind()
    End Sub
    'gridview sorting event
    Protected Sub Grid2_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles Grid2.Sorting
        Dim temp As DataTable = CType(ViewState("dtArticles"), DataTable)
        GridViewSortExpression = e.SortExpression
        Grid2.DataSource = temp
        Dim pageIndex As Integer = Grid2.PageIndex
        Grid2.DataSource = SortDataTable(Grid2.DataSource, False)
        Grid2.DataBind()
        Grid2.PageIndex = pageIndex
    End Sub
    Public Sub PopulateLooseIssuesGrid()
        Dim dtSearch As DataTable = Nothing
        Try

            Dim SQL As String = Nothing
            If Session("LoggedLibCode") <> "" Then
                SQL = "SELECT COPY_ID, TITLE , VOL_NO, ISSUE_NO, PART_NO, ISS_DATE, LOOSE_ISSUE_LIB_CODE, BOOKSTATUS.STA_NAME FROM CATS_LOOSE_ISSUES_COPIES_VIEW LEFT OUTER JOIN BOOKSTATUS ON CATS_LOOSE_ISSUES_COPIES_VIEW.STA_CODE = BOOKSTATUS.STA_CODE WHERE (LOOSE_ISSUE_LIB_CODE = '" & Trim(Session("LoggedLibCode")) & "') AND (CATS_LOOSE_ISSUES_COPIES_VIEW.DATE_ADDED >= getdate()-180  AND CATS_LOOSE_ISSUES_COPIES_VIEW.DATE_ADDED<=GetDate())  ORDER BY CATS_LOOSE_ISSUES_COPIES_VIEW.DATE_ADDED DESC"
            Else
                SQL = "SELECT COPY_ID, TITLE , VOL_NO, ISSUE_NO, PART_NO, ISS_DATE, LOOSE_ISSUE_LIB_CODE, BOOKSTATUS.STA_NAME FROM CATS_LOOSE_ISSUES_COPIES_VIEW LEFT OUTER JOIN BOOKSTATUS ON CATS_LOOSE_ISSUES_COPIES_VIEW.STA_CODE = BOOKSTATUS.STA_CODE WHERE (CATS_LOOSE_ISSUES_COPIES_VIEW.DATE_ADDED >= getdate()-180  AND CATS_LOOSE_ISSUES_COPIES_VIEW.DATE_ADDED<=GetDate())  ORDER BY CATS_LOOSE_ISSUES_COPIES_VIEW.DATE_ADDED DESC"
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
                Me.Grid3.DataSource = Nothing
                Grid3.DataBind()
            Else
                Grid3.Visible = True
                RecordCount = dtSearch.Rows.Count
                Grid3.DataSource = dtSearch
                Grid3.DataBind()
            End If
            ViewState("dtLoose") = dtSearch


        Catch s As Exception

        Finally
            SqlConn.Close()
        End Try
    End Sub
    'grid view page index changing event
    Protected Sub Grid3_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles Grid3.PageIndexChanging
        'rebind datagrid
        Grid3.DataSource = ViewState("dtLoose") 'temp
        Grid3.PageIndex = e.NewPageIndex
        index = e.NewPageIndex * Grid3.PageSize
        Grid3.DataBind()
    End Sub
    'gridview sorting event
    Protected Sub Grid3_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles Grid3.Sorting
        Dim temp As DataTable = CType(ViewState("dtLoose"), DataTable)
        GridViewSortExpression = e.SortExpression
        Grid3.DataSource = temp
        Dim pageIndex As Integer = Grid3.PageIndex
        Grid3.DataSource = SortDataTable(Grid3.DataSource, False)
        Grid3.DataBind()
        Grid3.PageIndex = pageIndex
    End Sub
    Protected Sub Book_Reserve_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Book_Reserve_Bttn.Click
        Dim thisTransaction As SqlClient.SqlTransaction
        Try
            If IsPostBack = True Then
                Dim iloop As Integer
                Dim strcurrentchar As Object
                Dim c As Integer
                Dim counter1 As Integer

                For Each row As GridViewRow In Grid1.Rows
                    If row.RowType = DataControlRowType.DataRow Then
                        Dim cb As CheckBox = DirectCast(row.FindControl("cbd"), CheckBox)
                        If cb IsNot Nothing AndAlso cb.Checked = True Then

                            Dim HOLD_ID As Integer = Nothing
                            HOLD_ID = Convert.ToInt32(Grid1.DataKeys(row.RowIndex).Value)
                            If IsNumeric(HOLD_ID) = False Then
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Error: Item ID is not Proper!');", True)
                                Exit Sub
                            End If

                            If Len(HOLD_ID) > 10 Then
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Error: Item ID is not Proper!');", True)
                                Exit Sub
                            End If

                            HOLD_ID = " " & HOLD_ID & " "
                            If InStr(1, HOLD_ID, "CREATE", 1) > 0 Or InStr(1, HOLD_ID, "DELETE", 1) > 0 Or InStr(1, HOLD_ID, "DROP", 1) > 0 Or InStr(1, HOLD_ID, "INSERT", 1) > 1 Or InStr(1, HOLD_ID, "TRACK", 1) > 1 Or InStr(1, HOLD_ID, "TRACE", 1) > 1 Then
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Error: Input is not Valid !');", True)
                                Exit Sub
                            End If

                            HOLD_ID = TrimX(HOLD_ID)
                            'check unwanted characters
                            c = 0
                            counter1 = 0
                            For iloop = 1 To Len(HOLD_ID.ToString)
                                strcurrentchar = Mid(HOLD_ID, iloop, 1)
                                If c = 0 Then
                                    If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                        c = c + 1
                                        counter1 = 1
                                    End If
                                End If
                            Next
                            If counter1 = 1 Then
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Error: Input is not Valid !');", True)
                                Exit Sub
                            End If

                            'check holding status of this book
                            Dim str As Object = Nothing
                            Dim flag As Object = Nothing
                            str = "SELECT HOLD_ID FROM HOLDINGS WHERE (HOLD_ID ='" & Trim(HOLD_ID) & "') AND (STA_CODE ='2') AND (LIB_CODE ='" & Trim(Session("LoggedLibCode")) & "')"
                            Dim cmd1 As New SqlCommand(str, SqlConn)
                            SqlConn.Open()
                            flag = cmd1.ExecuteScalar
                            SqlConn.Close()


                            If flag <> Nothing Then ' if books is issued
                                Dim str2 As Object = Nothing
                                Dim flag2 As Object = Nothing
                                str2 = "SELECT CIR_ID FROM CIRCULATION WHERE (HOLD_ID ='" & Trim(HOLD_ID) & "' AND STATUS ='Issued' AND MEM_ID = '" & Trim(Session.Item("LoggedMemID")) & "' AND LIB_CODE ='" & Trim(Session("LoggedLibCode")) & "')"
                                Dim cmd2 As New SqlCommand(str2, SqlConn)
                                SqlConn.Open()
                                flag2 = cmd2.ExecuteScalar
                                SqlConn.Close()
                                If flag2 = Nothing Then
                                    Dim str3 As Object = Nothing
                                    Dim flag3 As Object = Nothing
                                    str3 = "SELECT CIR_ID FROM CIRCULATION WHERE (HOLD_ID ='" & Trim(HOLD_ID) & "' AND STATUS ='Reserved' AND MEM_ID = '" & Trim(Session.Item("LoggedMemID")) & "' AND LIB_CODE ='" & Trim(Session("LoggedLibCode")) & "')"
                                    Dim cmd3 As New SqlCommand(str3, SqlConn)
                                    SqlConn.Open()
                                    flag3 = cmd3.ExecuteScalar
                                    SqlConn.Close()
                                    If flag3 = Nothing Then 'if book is reservable to this membre

                                        Dim RESERVE_DATE As Object = Nothing
                                        RESERVE_DATE = Now.Date
                                        RESERVE_DATE = RemoveQuotes(RESERVE_DATE)
                                        RESERVE_DATE = datechk(RESERVE_DATE)
                                        RESERVE_DATE = Date.Parse(RESERVE_DATE, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))

                                        'Reserve Time
                                        Dim RESERVE_TIME As Object = Nothing
                                        RESERVE_TIME = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")


                                        Dim STATUS As String = Nothing
                                        STATUS = "Reserved"

                                        Dim DATE_ADDED As Object = Nothing
                                        DATE_ADDED = Now.Date
                                        DATE_ADDED = datechk(DATE_ADDED)
                                        DATE_ADDED = Date.Parse(DATE_ADDED, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))

                                        Dim MEMB_IP As Object = Nothing
                                        MEMB_IP = Request.UserHostAddress.Trim

                                        'UPDATE   
                                        If Session.Item("LoggedMemID") <> 0 Then
                                            If SqlConn.State = 0 Then
                                                SqlConn.Open()
                                            End If
                                            thisTransaction = SqlConn.BeginTransaction()

                                            Dim intValue As Integer = 0
                                            Dim objCommand As New SqlCommand
                                            objCommand.Connection = SqlConn
                                            objCommand.Transaction = thisTransaction
                                            objCommand.CommandType = CommandType.Text
                                            objCommand.CommandText = "INSERT INTO CIRCULATION (MEM_ID, HOLD_ID, RESERVE_DATE, RESERVE_TIME, STATUS, DATE_ADDED, LIB_CODE, IP) " & _
                                                                     " VALUES (@MEM_ID, @HOLD_ID, @RESERVE_DATE, @RESERVE_TIME, @STATUS, @DATE_ADDED, @LIB_CODE, @IP); " & _
                                                                     " SELECT SCOPE_IDENTITY();"

                                            objCommand.Parameters.Add("@MEM_ID", SqlDbType.Int)
                                            If Session.Item("LoggedMemID") = 0 Then
                                                objCommand.Parameters("@MEM_ID").Value = System.DBNull.Value
                                            Else
                                                objCommand.Parameters("@MEM_ID").Value = Session.Item("LoggedMemID") <> 0
                                            End If

                                            objCommand.Parameters.Add("@HOLD_ID", SqlDbType.Int)
                                            If HOLD_ID = 0 Then
                                                objCommand.Parameters("@HOLD_ID").Value = System.DBNull.Value
                                            Else
                                                objCommand.Parameters("@HOLD_ID").Value = HOLD_ID
                                            End If

                                            objCommand.Parameters.Add("@RESERVE_DATE", SqlDbType.DateTime)
                                            If RESERVE_DATE = Nothing Then
                                                objCommand.Parameters("@RESERVE_DATE").Value = System.DBNull.Value
                                            Else
                                                objCommand.Parameters("@RESERVE_DATE").Value = RESERVE_DATE
                                            End If

                                            objCommand.Parameters.Add("@RESERVE_TIME", SqlDbType.DateTime)
                                            If RESERVE_TIME = Nothing Then
                                                objCommand.Parameters("@RESERVE_TIME").Value = System.DBNull.Value
                                            Else
                                                objCommand.Parameters("@RESERVE_TIME").Value = RESERVE_TIME
                                            End If

                                            objCommand.Parameters.Add("@STATUS", SqlDbType.VarChar)
                                            If STATUS = "" Then
                                                objCommand.Parameters("@STATUS").Value = System.DBNull.Value
                                            Else
                                                objCommand.Parameters("@STATUS").Value = STATUS
                                            End If

                                            objCommand.Parameters.Add("@DATE_ADDED", SqlDbType.DateTime)
                                            If DATE_ADDED = Nothing Then
                                                objCommand.Parameters("@DATE_ADDED").Value = System.DBNull.Value
                                            Else
                                                objCommand.Parameters("@DATE_ADDED").Value = DATE_ADDED
                                            End If

                                            objCommand.Parameters.Add("@IP", SqlDbType.VarChar)
                                            If MEMB_IP = "" Then
                                                objCommand.Parameters("@IP").Value = System.DBNull.Value
                                            Else
                                                objCommand.Parameters("@IP").Value = MEMB_IP
                                            End If

                                            objCommand.Parameters.Add("@LIB_CODE", SqlDbType.NVarChar)
                                            objCommand.Parameters("@LIB_CODE").Value = Session("LoggedLibCode")


                                            Dim dr As SqlDataReader
                                            dr = objCommand.ExecuteReader()
                                            If dr.Read Then
                                                intValue = dr.GetValue(0)
                                            End If
                                            dr.Close()

                                            thisTransaction.Commit()
                                            SqlConn.Close()
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                Next
            End If


        Catch q As SqlException
            thisTransaction.Rollback()
        Catch ex As Exception
        Finally
            SqlConn.Close()
        End Try


    End Sub
    'reserve loose issues
    Protected Sub Loose_Reserve_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Loose_Reserve_Bttn.Click
        Dim thisTransaction As SqlClient.SqlTransaction
        Try
            If IsPostBack = True Then
                Dim iloop As Integer
                Dim strcurrentchar As Object
                Dim c As Integer
                Dim counter1 As Integer

                For Each row As GridViewRow In Grid3.Rows
                    If row.RowType = DataControlRowType.DataRow Then
                        Dim cb As CheckBox = DirectCast(row.FindControl("cbd"), CheckBox)
                        If cb IsNot Nothing AndAlso cb.Checked = True Then

                            Dim COPY_ID As Integer = Nothing
                            COPY_ID = Convert.ToInt32(Grid3.DataKeys(row.RowIndex).Value)
                            If IsNumeric(COPY_ID) = False Then
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Error: Item ID is not Proper!');", True)
                                Exit Sub
                            End If

                            If Len(COPY_ID) > 10 Then
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Error: Item ID is not Proper!');", True)
                                Exit Sub
                            End If

                            COPY_ID = " " & COPY_ID & " "
                            If InStr(1, COPY_ID, "CREATE", 1) > 0 Or InStr(1, COPY_ID, "DELETE", 1) > 0 Or InStr(1, COPY_ID, "DROP", 1) > 0 Or InStr(1, COPY_ID, "INSERT", 1) > 1 Or InStr(1, COPY_ID, "TRACK", 1) > 1 Or InStr(1, COPY_ID, "TRACE", 1) > 1 Then
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Error: Input is not Valid !');", True)
                                Exit Sub
                            End If

                            COPY_ID = TrimX(COPY_ID)
                            'check unwanted characters
                            c = 0
                            counter1 = 0
                            For iloop = 1 To Len(COPY_ID.ToString)
                                strcurrentchar = Mid(COPY_ID, iloop, 1)
                                If c = 0 Then
                                    If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                        c = c + 1
                                        counter1 = 1
                                    End If
                                End If
                            Next
                            If counter1 = 1 Then
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Error: Input is not Valid !');", True)
                                Exit Sub
                            End If

                            'check holding status of this book
                            Dim str As Object = Nothing
                            Dim flag As Object = Nothing
                            str = "SELECT COPY_ID FROM LOOSE_ISSUES_COPIES WHERE (COPY_ID ='" & Trim(COPY_ID) & "') AND (STA_CODE ='2') AND  (LIB_CODE ='" & Trim(Session("LoggedLibCode")) & "')"
                            Dim cmd1 As New SqlCommand(str, SqlConn)
                            SqlConn.Open()
                            flag = cmd1.ExecuteScalar
                            SqlConn.Close()


                            If flag <> Nothing Then ' if copy id  is issued
                                Dim str2 As Object = Nothing
                                Dim flag2 As Object = Nothing
                                str2 = "SELECT CIR_ID FROM CIRCULATION WHERE (COPY_ID ='" & Trim(COPY_ID) & "' AND STATUS ='Issued'  AND  MEM_ID = '" & Trim(Session.Item("LoggedMemID")) & "' AND LIB_CODE ='" & Trim(Session("LoggedLibCode")) & "')"
                                Dim cmd2 As New SqlCommand(str2, SqlConn)
                                SqlConn.Open()
                                flag2 = cmd2.ExecuteScalar
                                SqlConn.Close()
                                If flag2 = Nothing Then
                                    Dim str3 As Object = Nothing
                                    Dim flag3 As Object = Nothing
                                    str3 = "SELECT CIR_ID FROM CIRCULATION WHERE (COPY_ID ='" & Trim(COPY_ID) & "' AND STATUS ='Reserved' AND MEM_ID = '" & Trim(Session.Item("LoggedMemID")) & "' AND LIB_CODE ='" & Trim(Session("LoggedLibCode")) & "')"
                                    Dim cmd3 As New SqlCommand(str3, SqlConn)
                                    SqlConn.Open()
                                    flag3 = cmd3.ExecuteScalar
                                    SqlConn.Close()
                                    If flag3 = Nothing Then 'if book is reservable to this membre

                                        Dim RESERVE_DATE As Object = Nothing
                                        RESERVE_DATE = Now.Date
                                        RESERVE_DATE = RemoveQuotes(RESERVE_DATE)
                                        RESERVE_DATE = datechk(RESERVE_DATE)
                                        RESERVE_DATE = Date.Parse(RESERVE_DATE, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))

                                        'Reserve Time
                                        Dim RESERVE_TIME As Object = Nothing
                                        RESERVE_TIME = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")


                                        Dim STATUS As String = Nothing
                                        STATUS = "Reserved"

                                        Dim DATE_ADDED As Object = Nothing
                                        DATE_ADDED = Now.Date
                                        DATE_ADDED = datechk(DATE_ADDED)
                                        DATE_ADDED = Date.Parse(DATE_ADDED, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))

                                        Dim MEMB_IP As Object = Nothing
                                        MEMB_IP = Request.UserHostAddress.Trim

                                        'UPDATE   
                                        If Session.Item("LoggedMemID") <> 0 Then
                                            If SqlConn.State = 0 Then
                                                SqlConn.Open()
                                            End If
                                            thisTransaction = SqlConn.BeginTransaction()

                                            Dim intValue As Integer = 0
                                            Dim objCommand As New SqlCommand
                                            objCommand.Connection = SqlConn
                                            objCommand.Transaction = thisTransaction
                                            objCommand.CommandType = CommandType.Text
                                            objCommand.CommandText = "INSERT INTO CIRCULATION (MEM_ID, COPY_ID, RESERVE_DATE, RESERVE_TIME, STATUS, DATE_ADDED, LIB_CODE, IP) " & _
                                                                     " VALUES (@MEM_ID, @COPY_ID, @RESERVE_DATE, @RESERVE_TIME, @STATUS, @DATE_ADDED, @LIB_CODE, @IP); " & _
                                                                     " SELECT SCOPE_IDENTITY();"

                                            objCommand.Parameters.Add("@MEM_ID", SqlDbType.Int)
                                            If Session.Item("LoggedMemID") = 0 Then
                                                objCommand.Parameters("@MEM_ID").Value = System.DBNull.Value
                                            Else
                                                objCommand.Parameters("@MEM_ID").Value = Session.Item("LoggedMemID") <> 0
                                            End If

                                            objCommand.Parameters.Add("@COPY_ID", SqlDbType.Int)
                                            If COPY_ID = 0 Then
                                                objCommand.Parameters("@COPY_ID").Value = System.DBNull.Value
                                            Else
                                                objCommand.Parameters("@COPY_ID").Value = COPY_ID
                                            End If

                                            objCommand.Parameters.Add("@RESERVE_DATE", SqlDbType.DateTime)
                                            If RESERVE_DATE = Nothing Then
                                                objCommand.Parameters("@RESERVE_DATE").Value = System.DBNull.Value
                                            Else
                                                objCommand.Parameters("@RESERVE_DATE").Value = RESERVE_DATE
                                            End If

                                            objCommand.Parameters.Add("@RESERVE_TIME", SqlDbType.DateTime)
                                            If RESERVE_TIME = Nothing Then
                                                objCommand.Parameters("@RESERVE_TIME").Value = System.DBNull.Value
                                            Else
                                                objCommand.Parameters("@RESERVE_TIME").Value = RESERVE_TIME
                                            End If

                                            objCommand.Parameters.Add("@STATUS", SqlDbType.VarChar)
                                            If STATUS = "" Then
                                                objCommand.Parameters("@STATUS").Value = System.DBNull.Value
                                            Else
                                                objCommand.Parameters("@STATUS").Value = STATUS
                                            End If

                                            objCommand.Parameters.Add("@DATE_ADDED", SqlDbType.DateTime)
                                            If DATE_ADDED = Nothing Then
                                                objCommand.Parameters("@DATE_ADDED").Value = System.DBNull.Value
                                            Else
                                                objCommand.Parameters("@DATE_ADDED").Value = DATE_ADDED
                                            End If

                                            objCommand.Parameters.Add("@IP", SqlDbType.VarChar)
                                            If MEMB_IP = "" Then
                                                objCommand.Parameters("@IP").Value = System.DBNull.Value
                                            Else
                                                objCommand.Parameters("@IP").Value = MEMB_IP
                                            End If

                                            objCommand.Parameters.Add("@LIB_CODE", SqlDbType.NVarChar)
                                            objCommand.Parameters("@LIB_CODE").Value = Session("LoggedLibCode")


                                            Dim dr As SqlDataReader
                                            dr = objCommand.ExecuteReader()
                                            If dr.Read Then
                                                intValue = dr.GetValue(0)
                                            End If
                                            dr.Close()

                                            thisTransaction.Commit()
                                            SqlConn.Close()
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                Next
            End If


        Catch q As SqlException
            thisTransaction.Rollback()
        Catch ex As Exception
        Finally
            SqlConn.Close()
        End Try
    End Sub
    
    Public Sub PublishmyTransactions()
        Dim dtSearch As DataTable = Nothing
        Try
            If Session.Item("LoggedMemID") <> 0 Then
                Dim iloop As Integer
                Dim strcurrentchar As Object
                Dim c As Integer
                Dim counter1, counter2, counter3, counter4, counter5, counter6, counter7, counter8, counter9, counter10, counter11, counter12, counter13, counter14, counter15, counter16, counter17, counter18, counter19 As Integer

                Dim MEM_ID As Integer = Nothing

                Dim SQL As String = Nothing
                SQL = "SELECT CIRCULATION_TRANSACTIONS_VIEW.CIR_ID,MEM_NAME, MEM_ID, MEM_NO, Accession = CASE (isnull(accession_no,'')) WHen ''  THEN cast(copy_id as varchar(20))" & _
                       " ELSE accession_no END, Title = CASE (isnull(Title,'')) WHen ''  THEN journal ELSE title END, convert(char(10),ISSUE_DATE,103) as ISSUE_DATE,ISSUE_TIME,convert(char(10),DUE_DATE,103) as DUE_DATE,convert(char(10),RETURN_DATE,103) as RETURN_DATE,RETURN_TIME," & _
                       " FINE_COLLECTED, convert(char(10),RESERVE_DATE,103) as RESERVE_DATE, convert(char(10),RENEW_DATE,103) as RENEW_DATE, STATUS, VOL_NO, CIR_LIB_CODE  FROM CIRCULATION_TRANSACTIONS_VIEW WHERE (CIR_LIB_CODE = '" & Trim(Session("LoggedLibCode")) & "') AND (CIRCULATION_TRANSACTIONS_VIEW.MEM_ID = '" & Trim(Session.Item("LoggedMemID")) & "') ORDER BY STATUS ASC, DUE_DATE DESC "

                Dim ds As New DataSet
                Dim da As New SqlDataAdapter(SQL, SqlConn)
                If SqlConn.State = 0 Then
                    SqlConn.Open()
                End If
                da.Fill(ds)

                dtSearch = ds.Tables(0).Copy

                If dtSearch.Rows.Count = 0 Then
                    Grid4.DataSource = Nothing
                    Grid4.DataBind()
                    TR_TRANSACTIONS.Visible = False
                Else
                    RecordCount = dtSearch.Rows.Count
                    Me.Grid4.DataSource = dtSearch
                    Me.Grid4.DataBind()
                    TR_TRANSACTIONS.Visible = True
                    Grid4.Visible = True
                    TR_RB.Visible = False
                    Label5.Text = "My Circulation Transactions"
                End If
                ViewState("dtTrans") = dtSearch
            End If
        Catch s As Exception

        Finally
            SqlConn.Close()
        End Try
    End Sub
    Protected Sub Grid4_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Grid4.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim CurrDate As Date = Nothing
            CurrDate = Now.Date
            CurrDate = Convert.ToDateTime(CurrDate, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert to indian 

            Dim DueDate As Object = Nothing
            ' If e.Row.Cells(7).Text <> "" Then
            If e.Row.Cells(9).Text = "Issued" Then
                If e.Row.Cells(5).Text <> "" Then
                    DueDate = e.Row.Cells(5).Text
                    Dim myDateArray As Object = Nothing
                    Dim strYYYY As Object = Nothing
                    Dim strMM As Object = Nothing
                    Dim strDD As Object = Nothing
                    myDateArray = Split(DueDate, "/")
                    strDD = myDateArray(0)
                    strMM = myDateArray(1)
                    strYYYY = myDateArray(2)

                    Dim myNewDueDate As Date = Nothing
                    myNewDueDate = strMM & "/" & strDD & "/" & strYYYY

                    If myNewDueDate <= CurrDate Then
                        e.Row.ForeColor = Drawing.Color.Red
                    Else
                        e.Row.ForeColor = Drawing.Color.DarkGreen
                    End If
                Else
                    e.Row.ForeColor = Drawing.Color.Brown
                End If
            Else
                If e.Row.Cells(9).Text = "Reserved" Then
                    e.Row.ForeColor = Drawing.Color.Brown
                End If
            End If



        End If
    End Sub
    'gridview sorting event
    Protected Sub Grid4_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles Grid4.Sorting
        Dim temp As DataTable = CType(ViewState("dtTrans"), DataTable)
        GridViewSortExpression = e.SortExpression
        Grid4.DataSource = temp
        Dim pageIndex As Integer = Grid4.PageIndex
        Grid4.DataSource = SortDataTable(Grid4.DataSource, False)
        Grid4.DataBind()
        Grid4.PageIndex = pageIndex
    End Sub
    'grid view page index changing event
    Protected Sub Grid4_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles Grid4.PageIndexChanging
        'rebind datagrid
        Grid4.DataSource = ViewState("dtTrans") 'temp
        Grid4.PageIndex = e.NewPageIndex
        index = e.NewPageIndex * Grid4.PageSize
        Grid4.DataBind()
    End Sub
    
   
End Class
