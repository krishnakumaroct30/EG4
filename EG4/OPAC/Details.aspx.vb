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
Public Class Details
    Inherits System.Web.UI.Page
    Dim HOLD_LIB_CODE As String = Nothing
    Dim dtLibrary As New DataTable
    Dim dtFull As New DataTable
    Dim dtHold As New DataTable
    Dim dtArticles As New DataTable
    Dim Count As Integer = Nothing
    Dim strFind As Object
    Public drLibrary1 As SqlDataReader
    Public DETAILSX As Object
    Dim index As Integer = 0
    Dim RecordCount As Integer
    Public Line As Object = Nothing
    Public Srno As Integer = 0
    Public myCatNo As Integer = 0
    Public myDispCatNo As Object
    Public BIB_CODE, MAT_CODE As String
    Public myTitle, myAuthor, myLang, myDocType, myISBN, myVarTitle, myCorpAuthor, myEditors, myTr, myIllus, myEdition, myPub As String
    Public mySeries, mySub, myKeyword, myURL, myCountry, myConf, myDateAdded As Object
    Public myAcc, myClassNo, myBook, myVol, myPages, myLibCode, myHoldLibCode As Object
    Public myHoldDateAdded, myAccDate As Date
    Public myTags As Object = Nothing
    Public Leader, my001, my003, my005, my008, my020, my040, my080, my088, my100, my110, my111, my130, my245, my246, my250, my260, my300, my490, my500, my520, my650, my700, my710, my711, my800, my830, my850, my852, my856 As Object
    Public COMPILER, NOTE, MULTI_VOL, TOTAL_VOL, SP_NO, SP_VERSION, REPORT_NO, MANUAL_NO, MANUAL_VER, PATENT_INVENTOR, PATENTEE, PATENT_NO, REPRINTS, CONF_FROM, CONF_TO, REVISED_BY, COMMENTATORS, SCHOLAR_NAME, SCHOLAR_DEPT, GUIDE_NAME, GUIDE_DEPT, DEGREE_NAME, COMMENTS, SP_TCSC, SP_UPDATES, SP_AMMENDMENTS, SP_ISSUE_BODY, PRODUCER, DESIGNER, MANUFACTURER, MATERIALS, TECHNIQ, WORK_CATEGORY, WORK_TYPE, CREATOR, ROLE_OF_CREATOR, RELATED_WORK, LITERARY_FORM, SOURCE, PHOTOGRAPHER, NATIONALITY, CHAIRMAN, GOVERNMENT, ACT_NO As String
    Public SP_REAFFIRM_YEAR, SP_WITHDRAW_YEAR, PRODUCTION_YEAR, ACT_YEAR As Integer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not SConnection() = True Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Database Connection is lost..Try Again !');", True)
            Else
                If Session("LoggedLibCode") <> "" Then
                    HOLD_LIB_CODE = Session("LoggedLibCode")
                Else
                    HOLD_LIB_CODE = ""
                End If

                DIV_CH.Visible = False
                DIV_MICRO.Visible = False
                If Request.QueryString("ctr").ToString <> "" Then
                    myCatNo = Request.QueryString("ctr")
                    GetTitleDetails()
                    DETAILSX = Trim(Session("DETAILS"))
                    Do While InStr(1, DETAILSX, "  ")
                        DETAILSX = Replace(DETAILSX, "  ", " ")
                    Loop
                End If
                Table1.Visible = False
            End If

        Catch ex As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Connection is not established... ');", True)
        End Try
    End Sub
    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As MenuEventArgs) Handles Menu1.MenuItemClick
        MultiView1.ActiveViewIndex = Int32.Parse(e.Item.Value)
        If MultiView1.ActiveViewIndex = 0 Then
            Label1.Text = "Brief View"
            Menu1.Items(0).ImageUrl = "~/Images/brup.png"
            Menu1.Items(1).ImageUrl = "~/Images/fvover.png"
            Menu1.Items(2).ImageUrl = "~/Images/mvover.png"
            Response.Write(Title)
        End If
        If MultiView1.ActiveViewIndex = 1 Then
            Label1.Text = "Full View"
            Menu1.Items(0).ImageUrl = "~/Images/brover.png"
            Menu1.Items(1).ImageUrl = "~/Images/fvup.png"
            Menu1.Items(2).ImageUrl = "~/Images/mvover.png"
        End If
        If MultiView1.ActiveViewIndex = 2 Then
            Label1.Text = "MARC View"
            Menu1.Items(0).ImageUrl = "~/Images/brover.png"
            Menu1.Items(1).ImageUrl = "~/Images/fvover.png"
            Menu1.Items(2).ImageUrl = "~/Images/mvup.png"
        End If
    End Sub
    Public Sub GetTitleDetails()
        Try
            If myCatNo <> 0 Then
                'server validation controls for Details            
                myCatNo = RemoveQuotes(myCatNo)

                If IsNumeric(myCatNo) = False Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do not use un-wanted input..pl try again ');", True)
                    Exit Sub
                End If
                If Len(myCatNo) >= 15 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Invalid SearchString... ');", True)
                    Exit Sub
                End If
                'check for reserve words
                If ((InStr(1, myCatNo, "CREATE", 1) > 0) Or (InStr(1, myCatNo, "DELETE", 1) > 0) Or (InStr(1, myCatNo, "DROP", 1) > 0) Or (InStr(1, myCatNo, "track", 1) > 0) Or (InStr(1, myCatNo, "trace", 1) > 0)) Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' SearchString ERROR... ');", True)
                    Exit Sub
                End If

                'check for unwanted characters
                Dim iloop As Integer
                Dim strcurrentchar As Object
                Dim c As Integer
                Dim counter1 As Integer
                c = 0
                counter1 = 0

                For iloop = 1 To myCatNo.ToString.Length
                    strcurrentchar = Mid(myCatNo, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter1 = 1
                        End If
                    End If
                Next

                If counter1 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('x Unwanted characters in SearchString... ');", True)
                    Exit Sub
                End If

                Dim ds As New DataSet
                Dim SQL As String = Nothing
                SQL = "SELECT * FROM CATS_AUTHORS_VIEW WHERE (CAT_NO ='" & Trim(myCatNo) & "') "
                Dim da As New SqlDataAdapter(SQL, SqlConn)
                da.Fill(ds)

                dtLibrary = ds.Tables(0).Copy

                If dtLibrary.Rows.Count = 0 Then
                    Exit Sub
                Else
                    myDispCatNo = dtLibrary.Rows(0).Item("CAT_NO").ToString

                    If dtLibrary.Rows(0).Item("LANG_CODE").ToString <> "" Then
                        myLang = dtLibrary.Rows(0).Item("LANG_CODE").ToString
                    Else
                        myLang = ""
                    End If

                    If dtLibrary.Rows(0).Item("BIB_CODE").ToString <> "" Then
                        BIB_CODE = dtLibrary.Rows(0).Item("BIB_CODE").ToString
                    Else
                        BIB_CODE = ""
                    End If

                    If dtLibrary.Rows(0).Item("MAT_CODE").ToString <> "" Then
                        MAT_CODE = dtLibrary.Rows(0).Item("MAT_CODE").ToString
                    Else
                        MAT_CODE = ""
                    End If

                    If dtLibrary.Rows(0).Item("DOC_TYPE_CODE").ToString <> "" Then
                        myDocType = dtLibrary.Rows(0).Item("DOC_TYPE_CODE").ToString
                    Else
                        myDocType = ""
                    End If

                    If dtLibrary.Rows(0).Item("STANDARD_NO").ToString <> "" Then
                        myISBN = dtLibrary.Rows(0).Item("STANDARD_NO").ToString
                    Else
                        myISBN = ""
                    End If

                    If dtLibrary.Rows(0).Item("SUB_TITLE").ToString <> "" Then
                        myTitle = dtLibrary.Rows(0).Item("TITLE").ToString & ": " & dtLibrary.Rows(0).Item("SUB_TITLE").ToString
                    Else
                        myTitle = dtLibrary.Rows(0).Item("TITLE").ToString
                    End If

                    If dtLibrary.Rows(0).Item("VAR_TITLE").ToString <> "" Then
                        myVarTitle = dtLibrary.Rows(0).Item("VAR_TITLE").ToString
                    Else
                        myVarTitle = ""
                    End If

                    If dtLibrary.Rows(0).Item("AUTHOR1").ToString <> "" And dtLibrary.Rows(0).Item("AUTHOR2").ToString <> "" And dtLibrary.Rows(0).Item("AUTHOR3").ToString <> "" Then
                        myAuthor = dtLibrary.Rows(0).Item("AUTHOR1").ToString & "; " & dtLibrary.Rows(0).Item("AUTHOR2").ToString & " and " & dtLibrary.Rows(0).Item("AUTHOR3").ToString
                    End If
                    If dtLibrary.Rows(0).Item("AUTHOR1").ToString <> "" And dtLibrary.Rows(0).Item("AUTHOR2").ToString <> "" And dtLibrary.Rows(0).Item("AUTHOR3").ToString = "" Then
                        myAuthor = dtLibrary.Rows(0).Item("AUTHOR1").ToString & " and " & dtLibrary.Rows(0).Item("AUTHOR2").ToString
                    End If
                    If dtLibrary.Rows(0).Item("AUTHOR1").ToString <> "" And dtLibrary.Rows(0).Item("AUTHOR2").ToString = "" And dtLibrary.Rows(0).Item("AUTHOR3").ToString = "" Then
                        myAuthor = dtLibrary.Rows(0).Item("AUTHOR1").ToString
                    End If

                    If dtLibrary.Rows(0).Item("CORPORATE_AUTHOR").ToString <> "" Then
                        myCorpAuthor = dtLibrary.Rows(0).Item("CORPORATE_AUTHOR").ToString
                    Else
                        myCorpAuthor = ""
                    End If

                    If dtLibrary.Rows(0).Item("EDITOR").ToString <> "" Then
                        myEditors = dtLibrary.Rows(0).Item("EDITOR").ToString
                    Else
                        myEditors = ""
                    End If

                    If dtLibrary.Rows(0).Item("TRANSLATOR").ToString <> "" Then
                        myTr = dtLibrary.Rows(0).Item("TRANSLATOR").ToString
                    Else
                        myTr = ""
                    End If

                    If dtLibrary.Rows(0).Item("ILLUSTRATOR").ToString <> "" Then
                        myIllus = dtLibrary.Rows(0).Item("ILLUSTRATOR").ToString
                    Else
                        myIllus = ""
                    End If

                    If dtLibrary.Rows(0).Item("COMPILER").ToString <> "" Then
                        COMPILER = dtLibrary.Rows(0).Item("COMPILER").ToString
                    Else
                        COMPILER = ""
                    End If

                    If dtLibrary.Rows(0).Item("EDITION").ToString <> "" Then
                        myEdition = dtLibrary.Rows(0).Item("EDITION").ToString
                    Else
                        myEdition = ""
                    End If

                    If dtLibrary.Rows(0).Item("PLACE_OF_PUB").ToString <> "" And dtLibrary.Rows(0).Item("PUB_NAME").ToString <> "" And dtLibrary.Rows(0).Item("YEAR_OF_PUB").ToString <> "" Then
                        myPub = dtLibrary.Rows(0).Item("PLACE_OF_PUB").ToString & "; " & dtLibrary.Rows(0).Item("PUB_NAME").ToString & "; " & dtLibrary.Rows(0).Item("YEAR_OF_PUB").ToString
                    End If

                    If dtLibrary.Rows(0).Item("PLACE_OF_PUB").ToString <> "" And dtLibrary.Rows(0).Item("PUB_NAME").ToString <> "" And dtLibrary.Rows(0).Item("YEAR_OF_PUB").ToString = "" Then
                        myPub = dtLibrary.Rows(0).Item("PLACE_OF_PUB").ToString & "; " & dtLibrary.Rows(0).Item("PUB_NAME").ToString
                    End If

                    If dtLibrary.Rows(0).Item("PLACE_OF_PUB").ToString = "" And dtLibrary.Rows(0).Item("PUB_NAME").ToString <> "" And dtLibrary.Rows(0).Item("YEAR_OF_PUB").ToString <> "" Then
                        myPub = dtLibrary.Rows(0).Item("PUB_NAME").ToString & "; " & dtLibrary.Rows(0).Item("YEAR_OF_PUB").ToString
                    End If
                    If dtLibrary.Rows(0).Item("PLACE_OF_PUB").ToString = "" And dtLibrary.Rows(0).Item("PUB_NAME").ToString <> "" And dtLibrary.Rows(0).Item("YEAR_OF_PUB").ToString = "" Then
                        myPub = dtLibrary.Rows(0).Item("PUB_NAME").ToString
                    End If

                    If dtLibrary.Rows(0).Item("SERIES_TITLE").ToString <> "" And dtLibrary.Rows(0).Item("SERIES_EDITOR").ToString <> "" Then
                        mySeries = dtLibrary.Rows(0).Item("SERIES_TITLE").ToString & " / Ed By " & dtLibrary.Rows(0).Item("SERIES_EDITOR").ToString
                    End If
                    If dtLibrary.Rows(0).Item("SERIES_TITLE").ToString <> "" And dtLibrary.Rows(0).Item("SERIES_EDITOR").ToString = "" Then
                        mySeries = dtLibrary.Rows(0).Item("SERIES_TITLE").ToString
                    End If

                    If dtLibrary.Rows(0).Item("NOTE").ToString <> "" Then
                        NOTE = dtLibrary.Rows(0).Item("NOTE").ToString
                    Else
                        NOTE = ""
                    End If

                    If dtLibrary.Rows(0).Item("KEYWORDS").ToString <> "" Then
                        myKeyword = dtLibrary.Rows(0).Item("KEYWORDS").ToString
                    Else
                        myKeyword = ""
                    End If

                    If dtLibrary.Rows(0).Item("MULTI_VOL").ToString <> "" Then
                        MULTI_VOL = dtLibrary.Rows(0).Item("MULTI_VOL").ToString
                    Else
                        MULTI_VOL = ""
                    End If

                    If dtLibrary.Rows(0).Item("TOTAL_VOL").ToString <> "" Then
                        TOTAL_VOL = dtLibrary.Rows(0).Item("TOTAL_VOL").ToString
                    Else
                        TOTAL_VOL = ""
                    End If

                    If dtLibrary.Rows(0).Item("SP_NO").ToString <> "" Then
                        SP_NO = dtLibrary.Rows(0).Item("SP_NO").ToString
                    Else
                        SP_NO = ""
                    End If

                    If dtLibrary.Rows(0).Item("SP_VERSION").ToString <> "" Then
                        SP_VERSION = dtLibrary.Rows(0).Item("SP_VERSION").ToString
                    Else
                        SP_VERSION = ""
                    End If

                    If dtLibrary.Rows(0).Item("REPORT_NO").ToString <> "" Then
                        REPORT_NO = dtLibrary.Rows(0).Item("REPORT_NO").ToString
                    Else
                        REPORT_NO = ""
                    End If

                    If dtLibrary.Rows(0).Item("MANUAL_NO").ToString <> "" Then
                        MANUAL_NO = dtLibrary.Rows(0).Item("MANUAL_NO").ToString
                    Else
                        MANUAL_NO = ""
                    End If

                    If dtLibrary.Rows(0).Item("MANUAL_VER").ToString <> "" Then
                        MANUAL_VER = dtLibrary.Rows(0).Item("MANUAL_VER").ToString
                    Else
                        MANUAL_VER = ""
                    End If

                    If dtLibrary.Rows(0).Item("PATENT_INVENTOR").ToString <> "" Then
                        PATENT_INVENTOR = dtLibrary.Rows(0).Item("PATENT_INVENTOR").ToString
                    Else
                        PATENT_INVENTOR = ""
                    End If

                    If dtLibrary.Rows(0).Item("PATENTEE").ToString <> "" Then
                        PATENTEE = dtLibrary.Rows(0).Item("PATENTEE").ToString
                    Else
                        PATENTEE = ""
                    End If

                    If dtLibrary.Rows(0).Item("PATENT_NO").ToString <> "" Then
                        PATENT_NO = dtLibrary.Rows(0).Item("PATENT_NO").ToString
                    Else
                        PATENT_NO = ""
                    End If

                    If dtLibrary.Rows(0).Item("REPRINTS").ToString <> "" Then
                        REPRINTS = dtLibrary.Rows(0).Item("REPRINTS").ToString
                    Else
                        REPRINTS = ""
                    End If

                    If dtLibrary.Rows(0).Item("CONF_FROM").ToString <> "" Then
                        CONF_FROM = dtLibrary.Rows(0).Item("CONF_FROM").ToString
                    Else
                        CONF_FROM = ""
                    End If

                    If dtLibrary.Rows(0).Item("CONF_TO").ToString <> "" Then
                        CONF_TO = dtLibrary.Rows(0).Item("CONF_TO").ToString
                    Else
                        CONF_TO = ""
                    End If

                    If dtLibrary.Rows(0).Item("REVISED_BY").ToString <> "" Then
                        REVISED_BY = dtLibrary.Rows(0).Item("REVISED_BY").ToString
                    Else
                        REVISED_BY = ""
                    End If

                    If dtLibrary.Rows(0).Item("COMMENTATORS").ToString <> "" Then
                        COMMENTATORS = dtLibrary.Rows(0).Item("COMMENTATORS").ToString
                    Else
                        COMMENTATORS = ""
                    End If

                    If dtLibrary.Rows(0).Item("KEYWORDS").ToString <> "" Then
                        myKeyword = dtLibrary.Rows(0).Item("KEYWORDS").ToString
                    Else
                        myKeyword = ""
                    End If

                    If dtLibrary.Rows(0).Item("SUB_NAME").ToString <> "" Then
                        mySub = dtLibrary.Rows(0).Item("SUB_NAME").ToString
                    Else
                        mySub = ""
                    End If

                    If dtLibrary.Rows(0).Item("URL").ToString <> "" Then
                        myURL = dtLibrary.Rows(0).Item("URL").ToString
                    Else
                        myURL = ""
                    End If

                    If dtLibrary.Rows(0).Item("CON_CODE").ToString <> "" Then
                        myCountry = dtLibrary.Rows(0).Item("CON_CODE").ToString
                    Else
                        myCountry = ""
                    End If

                    If dtLibrary.Rows(0).Item("CONF_NAME").ToString <> "" Then
                        myConf = dtLibrary.Rows(0).Item("CONF_NAME").ToString
                    Else
                        myConf = ""
                    End If

                    If dtLibrary.Rows(0).Item("DATE_ADDED").ToString <> "" Then
                        myDateAdded = dtLibrary.Rows(0).Item("DATE_ADDED").ToString
                    Else
                        myDateAdded = ""
                    End If

                    If dtLibrary.Rows(0).Item("TAGS").ToString <> "" Then
                        myTags = dtLibrary.Rows(0).Item("TAGS").ToString
                    Else
                        myTags = ""
                    End If

                    If dtLibrary.Rows(0).Item("SCHOLAR_NAME").ToString <> "" Then
                        SCHOLAR_NAME = dtLibrary.Rows(0).Item("SCHOLAR_NAME").ToString
                    Else
                        SCHOLAR_NAME = ""
                    End If

                    If dtLibrary.Rows(0).Item("SCHOLAR_DEPT").ToString <> "" Then
                        SCHOLAR_DEPT = dtLibrary.Rows(0).Item("SCHOLAR_DEPT").ToString
                    Else
                        SCHOLAR_DEPT = ""
                    End If

                    If dtLibrary.Rows(0).Item("GUIDE_NAME").ToString <> "" Then
                        GUIDE_NAME = dtLibrary.Rows(0).Item("GUIDE_NAME").ToString
                    Else
                        GUIDE_NAME = ""
                    End If

                    If dtLibrary.Rows(0).Item("GUIDE_DEPT").ToString <> "" Then
                        GUIDE_DEPT = dtLibrary.Rows(0).Item("GUIDE_DEPT").ToString
                    Else
                        GUIDE_DEPT = ""
                    End If

                    If dtLibrary.Rows(0).Item("DEGREE_NAME").ToString <> "" Then
                        DEGREE_NAME = dtLibrary.Rows(0).Item("DEGREE_NAME").ToString
                    Else
                        DEGREE_NAME = ""
                    End If

                    If dtLibrary.Rows(0).Item("COMMENTS").ToString <> "" Then
                        COMMENTS = dtLibrary.Rows(0).Item("COMMENTS").ToString
                    Else
                        COMMENTS = ""
                    End If

                    If dtLibrary.Rows(0).Item("SP_REAFFIRM_YEAR").ToString <> "" Then
                        SP_REAFFIRM_YEAR = dtLibrary.Rows(0).Item("SP_REAFFIRM_YEAR").ToString
                    Else
                        SP_REAFFIRM_YEAR = Nothing
                    End If

                    If dtLibrary.Rows(0).Item("SP_TCSC").ToString <> "" Then
                        SP_TCSC = dtLibrary.Rows(0).Item("SP_TCSC").ToString
                    Else
                        SP_TCSC = ""
                    End If

                    If dtLibrary.Rows(0).Item("SP_UPDATES").ToString <> "" Then
                        SP_UPDATES = dtLibrary.Rows(0).Item("SP_UPDATES").ToString
                    Else
                        SP_UPDATES = ""
                    End If

                    If dtLibrary.Rows(0).Item("SP_WITHDRAW_YEAR").ToString <> "" Then
                        SP_WITHDRAW_YEAR = dtLibrary.Rows(0).Item("SP_WITHDRAW_YEAR").ToString
                    Else
                        SP_WITHDRAW_YEAR = Nothing
                    End If

                    If dtLibrary.Rows(0).Item("SP_AMMENDMENTS").ToString <> "" Then
                        SP_AMMENDMENTS = dtLibrary.Rows(0).Item("SP_AMMENDMENTS").ToString
                    Else
                        SP_AMMENDMENTS = ""
                    End If

                    If dtLibrary.Rows(0).Item("SP_ISSUE_BODY").ToString <> "" Then
                        SP_ISSUE_BODY = dtLibrary.Rows(0).Item("SP_ISSUE_BODY").ToString
                    Else
                        SP_ISSUE_BODY = ""
                    End If

                    If dtLibrary.Rows(0).Item("PRODUCER").ToString <> "" Then
                        PRODUCER = dtLibrary.Rows(0).Item("PRODUCER").ToString
                    Else
                        PRODUCER = ""
                    End If

                    If dtLibrary.Rows(0).Item("DESIGNER").ToString <> "" Then
                        DESIGNER = dtLibrary.Rows(0).Item("DESIGNER").ToString
                    Else
                        DESIGNER = ""
                    End If

                    If dtLibrary.Rows(0).Item("MANUFACTURER").ToString <> "" Then
                        MANUFACTURER = dtLibrary.Rows(0).Item("MANUFACTURER").ToString
                    Else
                        MANUFACTURER = ""
                    End If

                    If dtLibrary.Rows(0).Item("MATERIALS").ToString <> "" Then
                        MATERIALS = dtLibrary.Rows(0).Item("MATERIALS").ToString
                    Else
                        MATERIALS = ""
                    End If

                    If dtLibrary.Rows(0).Item("TECHNIQ").ToString <> "" Then
                        TECHNIQ = dtLibrary.Rows(0).Item("TECHNIQ").ToString
                    Else
                        TECHNIQ = ""
                    End If

                    If dtLibrary.Rows(0).Item("WORK_CATEGORY").ToString <> "" Then
                        WORK_CATEGORY = dtLibrary.Rows(0).Item("WORK_CATEGORY").ToString
                    Else
                        WORK_CATEGORY = ""
                    End If

                    If dtLibrary.Rows(0).Item("WORK_TYPE").ToString <> "" Then
                        WORK_TYPE = dtLibrary.Rows(0).Item("WORK_TYPE").ToString
                    Else
                        WORK_TYPE = ""
                    End If

                    If dtLibrary.Rows(0).Item("CREATOR").ToString <> "" Then
                        CREATOR = dtLibrary.Rows(0).Item("CREATOR").ToString
                    Else
                        CREATOR = ""
                    End If

                    If dtLibrary.Rows(0).Item("ROLE_OF_CREATOR").ToString <> "" Then
                        ROLE_OF_CREATOR = dtLibrary.Rows(0).Item("ROLE_OF_CREATOR").ToString
                    Else
                        ROLE_OF_CREATOR = ""
                    End If


                    If dtLibrary.Rows(0).Item("RELATED_WORK").ToString <> "" Then
                        RELATED_WORK = dtLibrary.Rows(0).Item("RELATED_WORK").ToString
                    Else
                        RELATED_WORK = ""
                    End If

                    If dtLibrary.Rows(0).Item("LITERARY_FORM").ToString <> "" Then
                        LITERARY_FORM = dtLibrary.Rows(0).Item("LITERARY_FORM").ToString
                    Else
                        LITERARY_FORM = ""
                    End If

                    If dtLibrary.Rows(0).Item("SOURCE").ToString <> "" Then
                        SOURCE = dtLibrary.Rows(0).Item("SOURCE").ToString
                    Else
                        SOURCE = ""
                    End If

                    If dtLibrary.Rows(0).Item("PHOTOGRAPHER").ToString <> "" Then
                        PHOTOGRAPHER = dtLibrary.Rows(0).Item("PHOTOGRAPHER").ToString
                    Else
                        PHOTOGRAPHER = ""
                    End If

                    If dtLibrary.Rows(0).Item("PRODUCTION_YEAR").ToString <> "" Then
                        PRODUCTION_YEAR = dtLibrary.Rows(0).Item("PRODUCTION_YEAR").ToString
                    Else
                        PRODUCTION_YEAR = Nothing
                    End If

                    If dtLibrary.Rows(0).Item("NATIONALITY").ToString <> "" Then
                        NATIONALITY = dtLibrary.Rows(0).Item("NATIONALITY").ToString
                    Else
                        NATIONALITY = ""
                    End If

                    If dtLibrary.Rows(0).Item("CHAIRMAN").ToString <> "" Then
                        CHAIRMAN = dtLibrary.Rows(0).Item("CHAIRMAN").ToString
                    Else
                        CHAIRMAN = ""
                    End If

                    If dtLibrary.Rows(0).Item("GOVERNMENT").ToString <> "" Then
                        GOVERNMENT = dtLibrary.Rows(0).Item("GOVERNMENT").ToString
                    Else
                        GOVERNMENT = ""
                    End If

                    If dtLibrary.Rows(0).Item("ACT_NO").ToString <> "" Then
                        ACT_NO = dtLibrary.Rows(0).Item("ACT_NO").ToString
                    Else
                        ACT_NO = ""
                    End If

                    If dtLibrary.Rows(0).Item("ACT_YEAR").ToString <> "" Then
                        ACT_YEAR = dtLibrary.Rows(0).Item("ACT_YEAR").ToString
                    Else
                        ACT_YEAR = Nothing
                    End If
                    If dtLibrary.Rows(0).Item("PHOTO").ToString <> "" Then
                        Dim strURL As String = "~/Acquisition/Cats_GetPhoto.aspx?CAT_NO=" & dtLibrary.Rows(0).Item("CAT_NO").ToString & ""
                        Image1.ImageUrl = strURL
                        Image1.Visible = True
                    Else
                        Image1.Visible = True
                    End If
                End If
            End If
            SqlConn.Close()
            GetHoldingsData()
            GetMicroDocuments()
        Catch s As Exception
            'ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' there is error ..');", True)
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'get holdign data
    Public Sub GetHoldingsData()
        Try
            If myCatNo <> 0 Then
                Dim ds As New DataSet
                Dim SQL As String = Nothing
                If HOLD_LIB_CODE = "" Then
                    SQL = "SELECT HOLDINGS.*, BOOKSTATUS.STA_NAME FROM HOLDINGS  LEFT OUTER JOIN BOOKSTATUS ON HOLDINGS.STA_CODE = BOOKSTATUS.STA_CODE WHERE (CAT_NO ='" & Trim(myCatNo) & "') ORDER BY HOLDINGS.LIB_CODE; "
                Else
                    SQL = "SELECT HOLDINGS.*, BOOKSTATUS.STA_NAME FROM HOLDINGS  LEFT OUTER JOIN BOOKSTATUS ON HOLDINGS.STA_CODE = BOOKSTATUS.STA_CODE WHERE (CAT_NO ='" & Trim(myCatNo) & "') AND (HOLDINGS.LIB_CODE ='" & Trim(HOLD_LIB_CODE) & "') ORDER BY HOLD_ID; "
                End If

                Dim da As New SqlDataAdapter(SQL, SqlConn)
                da.Fill(ds)
                dtHold = ds.Tables(0).Copy
                If dtHold.Rows.Count = 0 Then
                    GridView1.DataSource = Nothing
                    GridView1.DataBind()
                    da.Dispose()
                    ds.Dispose()
                Else
                    ExportToMarcDisplayFormat_BOOKS()
                    RecordCount = dtHold.Rows.Count
                    GridView1.DataSource = dtHold
                    GridView1.DataBind()
                    GridView1.Caption = "Total Records : " & RecordCount
                    da.Dispose()
                    ds.Dispose()
                End If
            End If
                ViewState("dtHold") = dtHold
                UpdatePanel1.Update()
                SqlConn.Close()
        Catch s As Exception
            'ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' there is error ..');", True)
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
            'rebind datagrid
            GridView1.DataSource = ViewState("dtHold") 'temp
            GridView1.PageIndex = e.NewPageIndex
            index = e.NewPageIndex * GridView1.PageSize
            GridView1.DataBind()
        Catch s As Exception
            ' Label8.Text = "Error - Populate Budgets:  " & (s.Message())
            ' Label6.Text = ""
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
        UpdatePanel1.Update()
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
    Protected Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        Dim temp As DataTable = CType(ViewState("dtHold"), DataTable)
        GridViewSortExpression = e.SortExpression
        GridView1.DataSource = temp
        Dim pageIndex As Integer = GridView1.PageIndex
        GridView1.DataSource = SortDataTable(GridView1.DataSource, False)
        GridView1.DataBind()
        GridView1.PageIndex = pageIndex
        UpdatePanel1.Update()
    End Sub
    Private Shared Function t() As Object
        Throw New NotImplementedException
    End Function
    'get value of row from gridview1 to publish Circulation History
    Private Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        Dim ds As New DataSet
        Dim dtCirHist As DataTable
        Try
            Dim iloop As Integer
            Dim strcurrentchar As Object
            Dim c As Integer
            Dim counter1, counter2 As Integer

            'publish cir history
            If e.CommandName = "Select" Then
                Dim myRowID, HOLD_ID As Integer
                myRowID = e.CommandArgument.ToString()
                HOLD_ID = GridView1.DataKeys(myRowID).Value

                If Not String.IsNullOrEmpty(HOLD_ID) And HOLD_ID <> 0 Then
                    HOLD_ID = TrimX(HOLD_ID)
                    HOLD_ID = RemoveQuotes(HOLD_ID)
                    If Len(HOLD_ID).ToString > 10 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Length of Input is not Proper... ');", True)
                        Exit Sub
                    End If
                    If IsNumeric(HOLD_ID) = False Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Enter Valid Data Only!');", True)
                        Exit Sub
                    End If

                    HOLD_ID = " " & HOLD_ID & " "
                    If InStr(1, HOLD_ID, " CREATE ", 1) > 0 Or InStr(1, HOLD_ID, " DELETE ", 1) > 0 Or InStr(1, HOLD_ID, " DROP ", 1) > 0 Or InStr(1, HOLD_ID, " INSERT ", 1) > 1 Or InStr(1, HOLD_ID, " TRACK ", 1) > 1 Or InStr(1, HOLD_ID, " TRACE ", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do not use reserve words... ');", True)
                        Exit Sub
                    End If
                    HOLD_ID = TrimX(HOLD_ID)

                    Dim SelectedLibCode As String = Nothing
                    SelectedLibCode = GridView1.Rows(myRowID).Cells(8).Text
                    If SelectedLibCode <> "" Then
                        SelectedLibCode = RemoveQuotes(SelectedLibCode)

                        If Len(SelectedLibCode) >= 15 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' LIB CODE ERROR... ');", True)
                            Exit Sub
                        End If
                        'check for reserve words
                        If ((InStr(1, SelectedLibCode, "CREATE", 1) > 0) Or (InStr(1, SelectedLibCode, "DELETE", 1) > 0) Or (InStr(1, SelectedLibCode, "DROP", 1) > 0) Or (InStr(1, SelectedLibCode, "track", 1) > 0) Or (InStr(1, SelectedLibCode, "trace", 1) > 0)) Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' INVALID LIBCODE... ');", True)
                            Exit Sub
                        End If

                        'check for unwanted characters
                        c = 0
                        counter1 = 0
                        For iloop = 1 To Len(SelectedLibCode)
                            strcurrentchar = Mid(SelectedLibCode, iloop, 1)
                            If c = 0 Then
                                If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter1 = 1
                                End If
                            End If
                        Next
                        If counter1 = 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Unwanted Characters in LIBCODE... ');", True)
                            Exit Sub
                        End If
                    End If

                    'get record details from database
                    Dim SQL As String = Nothing
                    SQL = "SELECT CIRCULATION_TRANSACTIONS_VIEW.CIR_ID, MEM_NAME, MEM_NO, Accession = CASE (isnull(accession_no,'')) WHen ''  THEN cast(copy_id as varchar(20))" & _
                           " ELSE accession_no END, Title = CASE (isnull(Title,'')) WHen ''  THEN journal ELSE title END, convert(char(10),ISSUE_DATE,103) as ISSUE_DATE,ISSUE_TIME,convert(char(10),DUE_DATE,103) as DUE_DATE,convert(char(10),RETURN_DATE,103) as RETURN_DATE,RETURN_TIME," & _
                           " FINE_COLLECTED, convert(char(10),RESERVE_DATE,103) as RESERVE_DATE, convert(char(10),RENEW_DATE,103) as RENEW_DATE, STATUS, VOL_NO, CIR_LIB_CODE  FROM CIRCULATION_TRANSACTIONS_VIEW WHERE (CIR_LIB_CODE = '" & Trim(SelectedLibCode) & "') and (HOLD_ID ='" & Trim(HOLD_ID) & "') "

                    SqlConn.Open()
                    Dim da As New SqlDataAdapter(SQL, SqlConn)
                    da.Fill(ds)
                    dtCirHist = ds.Tables(0).Copy
                    If dtCirHist.Rows.Count = 0 Then
                        GridView2.DataSource = Nothing
                        GridView2.DataBind()
                        da.Dispose()
                        ds.Dispose()
                        DIV_CH.Visible = False
                    Else
                        RecordCount = dtCirHist.Rows.Count
                        GridView2.DataSource = dtCirHist
                        GridView2.DataBind()
                        GridView2.Caption = "Total Records : " & RecordCount
                        da.Dispose()
                        ds.Dispose()
                        DIV_CH.Visible = True
                    End If
                End If
                ViewState("dtCirHist") = dtCirHist
                SqlConn.Close()
            Else
                GridView2.DataSource = Nothing
                GridView2.DataBind()
            End If

            If e.CommandName = "View" Then
                DIV_CH.Visible = False
                Dim myRowID, HOLD_ID As Integer
                myRowID = e.CommandArgument.ToString()

                Dim SelectedLibCode As String = Nothing
                SelectedLibCode = GridView1.Rows(myRowID).Cells(8).Text
                If SelectedLibCode <> "" Then
                    'server validation controls for Details            
                    SelectedLibCode = RemoveQuotes(SelectedLibCode)

                    If Len(SelectedLibCode) >= 15 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' LIB CODE ERROR... ');", True)
                        Exit Sub
                    End If
                    'check for reserve words
                    If ((InStr(1, SelectedLibCode, "CREATE", 1) > 0) Or (InStr(1, SelectedLibCode, "DELETE", 1) > 0) Or (InStr(1, SelectedLibCode, "DROP", 1) > 0) Or (InStr(1, SelectedLibCode, "track", 1) > 0) Or (InStr(1, SelectedLibCode, "trace", 1) > 0)) Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' INVALID LIBCODE... ');", True)
                        Exit Sub
                    End If

                    'check for unwanted characters
                    c = 0
                    counter1 = 0
                    For iloop = 1 To Len(SelectedLibCode)
                        strcurrentchar = Mid(SelectedLibCode, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter1 = 1
                            End If
                        End If
                    Next
                    If counter1 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Unwanted Characters in LIBCODE... ');", True)
                        Exit Sub
                    End If

                    Dim ds2 As New DataSet
                    Dim SQL2 As String = Nothing
                    SQL2 = "SELECT * FROM LIBRARIES WHERE (LIB_CODE ='" & Trim(SelectedLibCode) & "') "
                    Dim da As New SqlDataAdapter(SQL2, SqlConn)
                    da.Fill(ds2)

                    dtLibrary = ds2.Tables(0).Copy

                    If dtLibrary.Rows.Count = 0 Then
                        Table1.Visible = False
                    Else
                        Dim myTitleDetails As String
                        myTitleDetails = "Title Details: " & myTitle + " " + myAuthor + " " + myLang + " " + myDocType + " " + myISBN + " " + myVarTitle + " " + myCorpAuthor + " " + myEditors + " " + myTr + " " + myIllus + " " + myEdition + " " + myPub
                        Table1.Visible = True
                        TextBox2.Text = dtLibrary.Rows(0).Item("LIB_EMAIL").ToString
                        TextBox6.Text = "Request for Book as detailed below "
                        TextBox7.Text = myTitleDetails & vbCrLf & vbCrLf & vbCrLf & vbCrLf & "Requested By: " & Session.Item("LoggedUser")
                        TextBox3.Text = Session.Item("LoggedEmail")
                    End If
                End If
                SqlConn.Close()
            End If

        Catch s As Exception

        Finally
            SqlConn.Close()
        End Try
    End Sub 'Grid1_ItemCommand
    Public Sub ExportToMarcDisplayFormat_BOOKS()
        Try
            If dtLibrary.Rows.Count <> 0 Then
                Line = ""
                '000 leader, 24 chr, 00-23 
                Line = Line & " " & "00000" '00-04: record length will be calculated by system
                Line = Line & "n" '05:Record Status: always 'n'=new
                Line = Line & "a" '06:type of record:  a=lanuage materia
                If dtLibrary.Rows(0).Item("BIB_CODE").ToString = "M" Then ' 07: bib level
                    Line = Line & "m"
                Else
                    Line = Line & "s"
                End If
                Line = Line & "\" ' 08: Type of control: blank(#) for not specified
                Line = Line & "a" '09: Character coding scheme: a=UCS/UNICODE
                Line = Line & "2" '10: indicator count: always 2
                Line = Line & "2" '11: subfield code count: always 2
                Line = Line & "00000" '12-16: base address of data:Calcualted by computer for each record
                Line = Line & "7" '17: Encoding level: 7=minimal level
                Line = Line & "a" '18: Descriptive cataloging form: a=AACR2
                Line = Line & "\" '19: Multipart Resource Record level: #=not applicable
                Line = Line & "4" '20: lenght of the-length-of-field position: always 4
                Line = Line & "5" '21: Length of the starting character position: always 5
                Line = Line & "0" '22: length of the implementation defined portion: always 0
                Line = Line & "0" & vbCrLf '23: undefined: always 0
                Leader = Line
                Line = ""
                '001: Control Number , max 12 chr A/N Var12 (Cat No)
                Line = Line & " " & dtLibrary.Rows(0).Item("CAT_NO").ToString & vbCrLf
                my001 = Line
                Line = ""

                '003: Control Number Identification : In-DelNIC : Alpha/Var 8 max chr
                Line = Line & " " & dtHold.Rows(0).Item("LIB_CODE").ToString & vbCrLf
                my003 = Line
                Line = ""

                '005: Date and Time of the latest Transaction : 16chr long, yyyymmddhhuuss
                If dtHold.Rows(0).Item("ACCESSION_DATE").ToString <> "" Then
                    Line = Line & " " & Format(dtHold.Rows(0).Item("ACCESSION_DATE"), "yyyyMMdd") + "000000.0" & vbCrLf
                Else
                    Line = Line & " " & Format(Date.Today, "yyyyMMdd") + "000000.0" & vbCrLf
                End If
                my005 = Line
                Line = ""

                '008 40 chr fixed length data, 00-39 (0-17 and 35-39 are common for all materials, 18-34 are separate for group of materiasl
                '008/00-05 Date of creation in yyMMdd format
                If dtHold.Rows(0).Item("ACCESSION_DATE").ToString <> "" Then
                    Line = Line & " " & Format(dtHold.Rows(0).Item("ACCESSION_DATE"), "yyMMdd")
                Else
                    Line = Line & " " & Format(Today.Date, "yyMMdd")
                End If
                '008/06: 
                Line = Line & "s" '06 type of dates s=1 single dates
                '008/07-10 :Date 1 (Start date for serials)
                If dtLibrary.Rows(0).Item("YEAR_OF_PUB").ToString <> "" Then
                    Line = Line & Trim(dtLibrary.Rows(0).Item("YEAR_OF_PUB").ToString)
                Else
                    Line = Line & Trim(Today.Year) '7-10 date 1
                End If
                '008/11-14 Date 2: blank in case s=single date (end date for Serials)
                Line = Line & "\\\\" '11-14 date2 = blank

                '008/15-17: MARC country Code, Country of Publication
                Dim myConcode As String = Nothing
                If dtLibrary.Rows(0).Item("CON_CODE").ToString <> "" Then
                    Select Case dtLibrary.Rows(0).Item("CON_CODE").ToString
                        Case "IND"
                            myConcode = "ii\"
                        Case "AUS"
                            myConcode = "at\"
                        Case "FRA"
                            myConcode = "fr\"
                        Case "GBR"
                            myConcode = "xxk"
                        Case "HKG"
                            myConcode = "cc\"
                        Case "JPN"
                            myConcode = "ja\"
                        Case "PAK"
                            myConcode = "pk\"
                        Case "RUS"
                            myConcode = "ru\"
                        Case "USA"
                            myConcode = "xxu"
                        Case Else
                            myConcode = "ii\"
                    End Select
                    Line = Line & myConcode
                Else
                    Line = Line & "xx\" '15-17 con code
                End If

                '18-34 are separate for books and serials
                '008/18-21 Illustration
                Line = Line & "\\\\" 'ill 18-21
                '008/22: Target Audience
                Line = Line & "\" 'Audience 22
                '008/23: Form of Item
                Line = Line & "d" 'Audience 23 d=large print
                '008/24-27: 
                Line = Line & "\\\\" '24-27 nature of contents
                '008/28: Govt publication
                Line = Line & "\" '28 Govt Publication
                '008/29: Conf Publication
                If dtLibrary.Rows(0).Item("CONF_NAME").ToString <> "" Then
                    Line = Line & "1" 'yes
                Else
                    Line = Line & "0" 'no
                End If
                '008/30: Festchrift
                Line = Line & "0" '30 not a festchrift
                '008/31: Index Y/N
                Line = Line & "0" '31 index present
                '008/32: undefined
                Line = Line & "\" '32 undefined
                '008/33: Literary Form
                Line = Line & "u" '33 literary form no u=unknown
                '008/34: Biography
                Line = Line & "\" '34 biography no

                '008/35-37 lang code
                If dtLibrary.Rows(0).Item("LANG_CODE").ToString <> "" Then
                    Line = Line & LCase(dtLibrary.Rows(0).Item("LANG_CODE").ToString)
                Else
                    Line = Line & "eng"
                End If
                '008/38: Modified Record
                Line = Line & "\" '38 record not modified
                '008/39: Cataloging source
                Line = Line & "d" & vbCrLf '39 cataloging agency u=unknown
                my008 = Line
                Line = ""
                '020 ISBN
                Dim myISBN As Object
                If dtLibrary.Rows(0).Item("STANDARD_NO").ToString <> "" Then
                    myISBN = RemoveQuotes(TrimX(Replace(RemoveQuotes(TrimX(dtLibrary.Rows(0).Item("STANDARD_NO").ToString())), "-", "")))
                    If Len(myISBN) = 10 Or Len(myISBN) = 13 Then
                        Line = Line & " " & "\\" & "$a" & Replace(TrimX(dtLibrary.Rows(0).Item("STANDARD_NO").ToString()), "-", "") & vbCrLf
                    Else
                        Line = Line & " " & "\\" & "$z" & Replace(TrimX(dtLibrary.Rows(0).Item("STANDARD_NO").ToString()), "-", "") & vbCrLf
                    End If
                End If
                my020 = Line
                Line = ""

                '040 cataloging agency
                Line = Line & " " & "\\" & "$a" & dtHold.Rows(0).Item("LIB_CODE").ToString & "$c" & dtHold.Rows(0).Item("LIB_CODE").ToString & vbCrLf
                my040 = Line
                Line = ""

                '080 UDC No
                'If RadioButton1.Checked = True Then 'for UDC
                If dtHold.Rows(0).Item("CLASS_NO").ToString <> "" Then
                    Line = Line & " " & "\\" & "$a" & TrimAll(dtHold.Rows(0).Item("CLASS_NO").ToString)
                    If dtHold.Rows(0).Item("BOOK_NO").ToString <> "" Then
                        Line = Line & "$b" & dtHold.Rows(0).Item("BOOK_NO").ToString
                    End If
                    Line = Line & vbCrLf
                End If
                'End If
                my080 = Line
                Line = ""

                ''082 DDC No
                ''If RadioButton2.Checked = True Then 'for DDC
                'If dtLibrary.Rows(0).Item("CLASS_NO").ToString <> "" Then
                '    Line = Line & "=082  " & "04" & "$a" & TrimAll(dtLibrary.Rows(0).Item("CLASS_NO").ToString)
                '    If dtLibrary.Rows(0).Item("BOOK_NO").ToString <> "" Then
                '        Line = Line & "$b" & dtLibrary.Rows(0).Item("BOOK_NO").ToString
                '    End If
                '    Line = Line & vbCrLf
                'End If
                'End If

                '088 Report Number Optional
                If dtLibrary.Rows(0).Item("REPORT_NO").ToString <> "" Then
                    Line = Line & " " & "\\" & "$a" & dtLibrary.Rows(0).Item("REPORT_NO").ToString & vbCrLf
                End If
                my088 = Line
                Line = ""

                '100 Personal Author for Main entry
                Dim myAuth As Object = Nothing
                If dtLibrary.Rows(0).Item("AUTHOR1").ToString <> "" Then
                    If InStr(dtLibrary.Rows(0).Item("AUTHOR1").ToString, ",") <> 0 Then
                        myAuth = TrimAll(Replace(dtLibrary.Rows(0).Item("AUTHOR1").ToString(), ",", ", "))
                        myAuth = TrimAll(Replace(myAuth, ".", " "))
                        If Trim(myAuth.ToString.Substring(Len(myAuth) - 1)) = ")" Then
                            Line = Line & " " & "1\" & "$a" & TrimAll(myAuth) & vbCrLf
                        Else
                            Line = Line & " " & "1\" & "$a" & TrimAll(myAuth) & "." & vbCrLf
                        End If
                    Else
                        myAuth = TrimAll(Replace(dtLibrary.Rows(0).Item("AUTHOR1").ToString(), ".", " "))
                        If Trim(myAuth.ToString.Substring(Len(myAuth) - 1)) = ")" Then
                            Line = Line & " " & "0\" & "$a" & TrimAll(myAuth) & vbCrLf
                        Else
                            Line = Line & " " & "0\" & "$a" & TrimAll(myAuth) & "." & vbCrLf
                        End If
                    End If
                End If
                my100 = Line
                Line = ""

                '110 Corporate Author for Main entry
                If dtLibrary.Rows(0).Item("AUTHOR1").ToString = "" And dtLibrary.Rows(0).Item("CORPORATE_AUTHOR").ToString <> "" Then
                    Line = Line & " " & "2\" & "$a" & TrimAll(dtLibrary.Rows(0).Item("CORPORATE_AUTHOR").ToString()) & "." & vbCrLf
                End If
                my110 = Line
                Line = ""

                '111 Conference for Main entry
                If dtLibrary.Rows(0).Item("AUTHOR1").ToString = "" And dtLibrary.Rows(0).Item("CORPORATE_AUTHOR").ToString = "" And dtLibrary.Rows(0).Item("CONF_NAME").ToString <> "" Then
                    Line = Line & " " & "2\" & "$a" & TrimAll(dtLibrary.Rows(0).Item("CONF_NAME").ToString()) & "." & vbCrLf
                End If
                my111 = Line
                Line = ""

                '130 UNIFORM TITLE for Main entry
                Dim OneThirty As Object = Nothing
                Dim my130Title As Object = Nothing
                If dtLibrary.Rows(0).Item("AUTHOR1").ToString = "" And dtLibrary.Rows(0).Item("CORPORATE_AUTHOR").ToString = "" And dtLibrary.Rows(0).Item("CONF_NAME").ToString = "" Then
                    my130Title = TrimAll(dtLibrary.Rows(0).Item("TITLE").ToString())
                    my130Title = TrimAll(Replace(my130Title, "a ", "", 1, 2))
                    my130Title = TrimAll(Replace(my130Title, "A ", "", 1, 2))
                    my130Title = TrimAll(Replace(my130Title, "an ", "", 1, 3))
                    my130Title = TrimAll(Replace(my130Title, "An ", "", 1, 3))
                    my130Title = TrimAll(Replace(my130Title, "AN ", "", 1, 3))
                    my130Title = TrimAll(Replace(my130Title, "aN ", "", 1, 3))
                    my130Title = TrimAll(Replace(my130Title, "The ", "", 1, 4))
                    my130Title = TrimAll(Replace(my130Title, "the ", "", 1, 4))
                    my130Title = TrimAll(Replace(my130Title, "THe ", "", 1, 4))
                    my130Title = TrimAll(Replace(my130Title, "THE ", "", 1, 4))
                    my130Title = TrimAll(Replace(my130Title, "tHe ", "", 1, 4))
                    my130Title = TrimAll(Replace(my130Title, "tHE ", "", 1, 4))
                    Line = Line & " " & "0\" & "$a" & my130Title
                    If dtLibrary.Rows(0).Item("LANG_CODE").ToString <> "" Then
                        Line = Line & ".$l" & TrimAll(dtLibrary.Rows(0).Item("LANG_CODE").ToString()) & "."
                    End If
                    Line = Line & vbCrLf
                    OneThirty = "YES"
                Else
                    OneThirty = "NO"
                End If
                my130Title = Line
                Line = ""

                '245 Main Title
                Dim mySplitAuthor1, SurName1, ForeName1, MyNewAuthor1 As Object
                If dtLibrary.Rows(0).Item("AUTHOR1").ToString <> "" Then
                    If InStr(dtLibrary.Rows(0).Item("AUTHOR1").ToString, ",") <> 0 Then
                        mySplitAuthor1 = TrimAll(dtLibrary.Rows(0).Item("AUTHOR1").ToString())
                        mySplitAuthor1 = Split(mySplitAuthor1, ",")
                        SurName1 = mySplitAuthor1(0)
                        ForeName1 = mySplitAuthor1(1)
                        MyNewAuthor1 = Trim(ForeName1) & " " & Trim(SurName1)
                    Else
                        MyNewAuthor1 = TrimAll(dtLibrary.Rows(0).Item("AUTHOR1").ToString())
                    End If
                End If

                Dim mySplitAuthor2, SurName2, ForeName2, MyNewAuthor2 As Object
                If dtLibrary.Rows(0).Item("AUTHOR2").ToString <> "" Then
                    If InStr(dtLibrary.Rows(0).Item("AUTHOR2").ToString, ",") <> 0 Then
                        mySplitAuthor2 = TrimAll(dtLibrary.Rows(0).Item("AUTHOR2").ToString())
                        mySplitAuthor2 = Split(mySplitAuthor2, ",")
                        SurName2 = mySplitAuthor2(0)
                        ForeName2 = mySplitAuthor2(1)
                        MyNewAuthor2 = Trim(ForeName2) & " " & Trim(SurName2)
                    Else
                        MyNewAuthor2 = TrimAll(dtLibrary.Rows(0).Item("AUTHOR2").ToString())
                    End If
                End If

                Dim mySplitAuthor3, SurName3, ForeName3, MyNewAuthor3 As Object
                If dtLibrary.Rows(0).Item("AUTHOR3").ToString <> "" Then
                    If InStr(dtLibrary.Rows(0).Item("AUTHOR3").ToString, ",") <> 0 Then
                        mySplitAuthor3 = TrimAll(dtLibrary.Rows(0).Item("AUTHOR3").ToString())
                        mySplitAuthor3 = Split(mySplitAuthor3, ",")
                        SurName3 = mySplitAuthor3(0)
                        ForeName3 = mySplitAuthor3(1)
                        MyNewAuthor3 = Trim(ForeName3) & " " & Trim(SurName3)
                    Else
                        MyNewAuthor3 = TrimAll(dtLibrary.Rows(0).Item("AUTHOR3").ToString())
                    End If
                End If

                Dim myTitle As String = Nothing
                Dim myRKM As Object = Nothing
                myTitle = Replace(TrimAll(dtLibrary.Rows(0).Item("TITLE").ToString()), ":", ",")
                myTitle = Replace(myTitle, "...", "--")
                myTitle = TrimAll(myTitle)

                If dtLibrary.Rows(0).Item("AUTHOR1").ToString <> "" Or dtLibrary.Rows(0).Item("CORPORATE_AUTHOR").ToString <> "" Or dtLibrary.Rows(0).Item("CONF_NAME").ToString <> "" Or OneThirty = "YES" Then
                    Line = Line & " " & "1" '  index the title as an added entry
                    ''count non-filing caharacters - omiited by RKM
                    If Len(myTitle) > 1 Then
                        If TrimAll(dtLibrary.Rows(0).Item("TITLE").ToString.Substring(0, 2)) = "a " Or TrimAll(dtLibrary.Rows(0).Item("TITLE").ToString.Substring(0, 2)) = "A " Then
                            Line = Line & "2"
                            myRKM = "RKM"
                        End If
                    End If
                    If Len(myTitle) > 2 Then
                        If TrimAll(dtLibrary.Rows(0).Item("TITLE").ToString.Substring(0, 3)) = "an " Or TrimAll(dtLibrary.Rows(0).Item("TITLE").ToString.Substring(0, 3)) = "An " Then
                            Line = Line & "3"
                            myRKM = "RKM"
                        End If
                    End If
                    If Len(myTitle) > 3 Then
                        If TrimAll(dtLibrary.Rows(0).Item("TITLE").ToString.Substring(0, 4)) = "The " Or TrimAll(dtLibrary.Rows(0).Item("TITLE").ToString.Substring(0, 4)) = "the " Or TrimAll(dtLibrary.Rows(0).Item("TITLE").ToString.Substring(0, 4)) = "THE " Then
                            Line = Line & "4"
                            myRKM = "RKM"
                        End If
                    End If
                    If myRKM <> "RKM" Then
                        Line = Line & "0"
                    End If
                    Line = Line & "$a" & myTitle
                    If dtLibrary.Rows(0).Item("SUB_TITLE").ToString <> "" Then
                        Line = Line & " :$b" & TrimAll(dtLibrary.Rows(0).Item("SUB_TITLE").ToString())
                    End If
                    If dtLibrary.Rows(0).Item("AUTHOR1").ToString <> "" And dtLibrary.Rows(0).Item("AUTHOR2").ToString <> "" And dtLibrary.Rows(0).Item("AUTHOR3").ToString <> "" Then
                        Line = Line & " /$cBy " & Trim(MyNewAuthor1) & ", " & MyNewAuthor2 & " and " & MyNewAuthor3
                        If dtLibrary.Rows(0).Item("EDITOR").ToString <> "" Then
                            Line = Line & "; Edited by " & dtLibrary.Rows(0).Item("EDITOR").ToString
                        End If
                        If dtLibrary.Rows(0).Item("TRANSLATOR").ToString <> "" Then
                            Line = Line & "; Tr. by " & dtLibrary.Rows(0).Item("TRANSLATOR").ToString
                        End If
                        If dtLibrary.Rows(0).Item("ILLUSTRATOR").ToString <> "" Then
                            Line = Line & "; Illus. by " & dtLibrary.Rows(0).Item("ILLUSTRATOR").ToString
                        End If
                    End If
                    If dtLibrary.Rows(0).Item("AUTHOR1").ToString <> "" And dtLibrary.Rows(0).Item("AUTHOR2").ToString <> "" And dtLibrary.Rows(0).Item("AUTHOR3").ToString = "" Then
                        Line = Line & " /$cBy " & Trim(MyNewAuthor1) & ", " & MyNewAuthor2
                        If dtLibrary.Rows(0).Item("EDITOR").ToString <> "" Then
                            Line = Line & "; edited by " & dtLibrary.Rows(0).Item("EDITOR").ToString
                        End If
                        If dtLibrary.Rows(0).Item("TRANSLATOR").ToString <> "" Then
                            Line = Line & "; Tr. by " & dtLibrary.Rows(0).Item("TRANSLATOR").ToString
                        End If
                        If dtLibrary.Rows(0).Item("ILLUSTRATOR").ToString <> "" Then
                            Line = Line & "; Illus. by " & dtLibrary.Rows(0).Item("ILLUSTRATOR").ToString
                        End If
                    End If
                    If dtLibrary.Rows(0).Item("AUTHOR1").ToString <> "" And dtLibrary.Rows(0).Item("AUTHOR2").ToString = "" And dtLibrary.Rows(0).Item("AUTHOR3").ToString = "" Then
                        Line = Line & " /$cBy " & Trim(MyNewAuthor1)
                        If dtLibrary.Rows(0).Item("EDITOR").ToString <> "" Then
                            Line = Line & "; edited by " & dtLibrary.Rows(0).Item("EDITOR").ToString
                        End If
                        If dtLibrary.Rows(0).Item("TRANSLATOR").ToString <> "" Then
                            Line = Line & "; Tr. by " & dtLibrary.Rows(0).Item("TRANSLATOR").ToString
                        End If
                        If dtLibrary.Rows(0).Item("ILLUSTRATOR").ToString <> "" Then
                            Line = Line & "; Illus. by " & dtLibrary.Rows(0).Item("ILLUSTRATOR").ToString
                        End If
                    End If
                    Line = Line & "." & vbCrLf
                Else
                    Line = Line & " " & "0"  ' do not Index title as an added entry
                    ''count non-filing caharacters - omiited by RKM
                    If TrimAll(dtLibrary.Rows(0).Item("TITLE").ToString.Substring(0, 2)) = "a " Or TrimAll(dtLibrary.Rows(0).Item("TITLE").ToString.Substring(0, 2)) = "A " Then
                        Line = Line & "2"
                        myRKM = "RKM"
                    End If
                    If TrimAll(dtLibrary.Rows(0).Item("TITLE").ToString.Substring(0, 3)) = "an " Or TrimAll(dtLibrary.Rows(0).Item("TITLE").ToString.Substring(0, 3)) = "An " Then
                        Line = Line & "3"
                        myRKM = "RKM"
                    End If
                    If TrimAll(dtLibrary.Rows(0).Item("TITLE").ToString.Substring(0, 4)) = "The " Or TrimAll(dtLibrary.Rows(0).Item("TITLE").ToString.Substring(0, 4)) = "the " Or TrimAll(dtLibrary.Rows(0).Item("TITLE").ToString.Substring(0, 4)) = "THE " Then
                        Line = Line & "4"
                        myRKM = "RKM"
                    End If
                    If myRKM <> "RKM" Then
                        Line = Line & "0"
                    End If
                    Line = Line & "$a" & myTitle
                    If dtLibrary.Rows(0).Item("SUB_TITLE").ToString <> "" Then
                        Line = Line & " :$b" & TrimAll(dtLibrary.Rows(0).Item("SUB_TITLE").ToString())
                    End If
                    If dtLibrary.Rows(0).Item("AUTHOR1").ToString <> "" And dtLibrary.Rows(0).Item("AUTHOR2").ToString <> "" And dtLibrary.Rows(0).Item("AUTHOR3").ToString <> "" Then
                        Line = Line & " /$cBy " & Trim(MyNewAuthor1) & ", " & MyNewAuthor2 & " and " & MyNewAuthor3
                        If dtLibrary.Rows(0).Item("EDITOR").ToString <> "" Then
                            Line = Line & "; Edited by " & dtLibrary.Rows(0).Item("EDITOR").ToString
                        End If
                        If dtLibrary.Rows(0).Item("TRANSLATOR").ToString <> "" Then
                            Line = Line & "; Tr. by " & dtLibrary.Rows(0).Item("TRANSLATOR").ToString
                        End If
                        If dtLibrary.Rows(0).Item("ILLUSTRATOR").ToString <> "" Then
                            Line = Line & "; Illus. by " & dtLibrary.Rows(0).Item("ILLUSTRATOR").ToString
                        End If
                    End If
                    If dtLibrary.Rows(0).Item("AUTHOR1").ToString <> "" And dtLibrary.Rows(0).Item("AUTHOR2").ToString <> "" And dtLibrary.Rows(0).Item("AUTHOR3").ToString = "" Then
                        Line = Line & " /$cBy " & Trim(MyNewAuthor1) & ", " & MyNewAuthor2
                        If dtLibrary.Rows(0).Item("EDITOR").ToString <> "" Then
                            Line = Line & "; edited by " & dtLibrary.Rows(0).Item("EDITOR").ToString
                        End If
                        If dtLibrary.Rows(0).Item("TRANSLATOR").ToString <> "" Then
                            Line = Line & "; Tr. by " & dtLibrary.Rows(0).Item("TRANSLATOR").ToString
                        End If
                        If dtLibrary.Rows(0).Item("ILLUSTRATOR").ToString <> "" Then
                            Line = Line & "; Illus. by " & dtLibrary.Rows(0).Item("ILLUSTRATOR").ToString
                        End If
                    End If
                    If dtLibrary.Rows(0).Item("AUTHOR1").ToString <> "" And dtLibrary.Rows(0).Item("AUTHOR2").ToString = "" And dtLibrary.Rows(0).Item("AUTHOR3").ToString = "" Then
                        Line = Line & " /$cBy " & Trim(MyNewAuthor1)
                        If dtLibrary.Rows(0).Item("EDITOR").ToString <> "" Then
                            Line = Line & "; edited by " & dtLibrary.Rows(0).Item("EDITOR").ToString
                        End If
                        If dtLibrary.Rows(0).Item("TRANSLATOR").ToString <> "" Then
                            Line = Line & "; Tr. by " & dtLibrary.Rows(0).Item("TRANSLATOR").ToString
                        End If
                        If dtLibrary.Rows(0).Item("ILLUSTRATOR").ToString <> "" Then
                            Line = Line & "; Illus. by " & dtLibrary.Rows(0).Item("ILLUSTRATOR").ToString
                        End If
                    End If
                    Line = Line & "." & vbCrLf
                End If

                my245 = Line
                Line = ""

                '246 VAR_TITLE
                If dtLibrary.Rows(0).Item("VAR_TITLE").ToString <> "" Then
                    Line = Line & " " & "33" & "$a" & TrimAll(dtLibrary.Rows(0).Item("VAR_TITLE").ToString()) & vbCrLf
                End If
                my246 = Line
                Line = ""

                '250 Edition Statement
                If dtLibrary.Rows(0).Item("EDITION").ToString <> "" Then
                    Dim myEd As Object
                    myEd = TrimAll(dtLibrary.Rows(0).Item("EDITION").ToString)
                    myEd = Replace(myEd, "Edition", "Ed")
                    myEd = Replace(myEd, "Edition.", "Ed")
                    myEd = Replace(myEd, "edition", "Ed")
                    myEd = Replace(myEd, "edition.", "Ed")
                    myEd = Replace(myEd, "First", "1st")
                    myEd = Replace(myEd, "first", "1st")
                    myEd = Replace(myEd, "Seventh", "7th")
                    myEd = Replace(myEd, ".", "")
                    Line = Line & " " & "\\" & "$a" & myEd & "." & vbCrLf
                Else
                    Line = Line & " " & "\\" & "$a" & "1st Ed." & vbCrLf
                End If
                my250 = Line
                Line = ""

                '260 Imprint Statement
                Line = Line & " " & "\\"
                If dtLibrary.Rows(0).Item("PLACE_OF_PUB").ToString <> "" Then
                    Line = Line & "$a" & TrimAll(dtLibrary.Rows(0).Item("PLACE_OF_PUB").ToString()) & " :"
                Else
                    Line = Line & "$a" & "[S.l.] :"
                End If
                Dim myPub As Object = Nothing
                If dtLibrary.Rows(0).Item("PUB_NAME").ToString <> "" Then
                    myPub = TrimAll(dtLibrary.Rows(0).Item("PUB_NAME").ToString())
                    myPub = TrimAll(Replace(myPub, "a ", "", 1, 2))
                    myPub = TrimAll(Replace(myPub, "A ", "", 1, 2))
                    myPub = TrimAll(Replace(myPub, "an ", "", 1, 3))
                    myPub = TrimAll(Replace(myPub, "An ", "", 1, 3))
                    myPub = TrimAll(Replace(myPub, "AN ", "", 1, 3))
                    myPub = TrimAll(Replace(myPub, "aN ", "", 1, 3))
                    myPub = TrimAll(Replace(myPub, "The ", "", 1, 4))
                    myPub = TrimAll(Replace(myPub, "the ", "", 1, 4))
                    myPub = TrimAll(Replace(myPub, "THe ", "", 1, 4))
                    myPub = TrimAll(Replace(myPub, "THE ", "", 1, 4))
                    myPub = TrimAll(Replace(myPub, "tHe ", "", 1, 4))
                    myPub = TrimAll(Replace(myPub, "tHE ", "", 1, 4))
                    Line = Line & "$b" & TrimAll(myPub) & ","
                Else
                    Line = Line & "$b" & "[S.l.],"
                End If
                If dtLibrary.Rows(0).Item("YEAR_OF_PUB").ToString <> "" Then
                    Line = Line & "$c" & TrimAll(dtLibrary.Rows(0).Item("YEAR_OF_PUB").ToString()) & "."
                Else
                    Line = Line & "$c[" & Today.Year & "?]"
                End If
                Line = Line & vbCrLf
                my260 = Line
                Line = ""

                '300 pagination
                If dtHold.Rows(0).Item("PAGINATION").ToString <> "" Then
                    Dim myPages As Object = Nothing
                    myPages = Replace(TrimAll(dtHold.Rows(0).Item("PAGINATION").ToString()), ";", ",")
                    Line = Line & " " & "\\"
                    If InStr(dtHold.Rows(0).Item("PAGINATION").ToString(), "p") = 0 Then
                        Line = Line & "$a" & TrimAll(myPages) + "p."
                    Else
                        Line = Line & "$a" & TrimAll(myPages) + "."
                    End If
                    Line = Line & " ;$c00 cm."
                End If
                Line = Line & vbCrLf
                my300 = Line
                Line = ""

                '490 Series Statement
                If dtLibrary.Rows(0).Item("SERIES_TITLE").ToString <> "" Then
                    Line = Line & " "
                    Line = Line & "1\"
                    Line = Line & "$a" & TrimAll(dtLibrary.Rows(0).Item("SERIES_TITLE").ToString()) & vbCrLf
                End If
                my490 = Line
                Line = ""

                '500 Note
                If dtLibrary.Rows(0).Item("NOTE").ToString <> "" Then
                    Dim my500 As Object
                    my500 = TrimAll(dtLibrary.Rows(0).Item("NOTE").ToString)
                    If my500.ToString <> "" Then
                        Line = Line & " " & "\\"
                        Line = Line & "$a" & TrimAll(dtLibrary.Rows(0).Item("NOTE").ToString()) & "." & vbCrLf
                    End If
                End If
                my500 = Line
                Line = ""

                '520 Summary/abstract
                If dtLibrary.Rows(0).Item("ABSTRACT").ToString <> "" Then
                    Line = Line & " " & "3\"
                    Line = Line & "$a" & TrimAll(dtLibrary.Rows(0).Item("ABSTRACT").ToString()) & "." & vbCrLf
                End If
                my520 = Line
                Line = ""

                '650 SUBJECT - TROPICAL
                If dtLibrary.Rows(0).Item("SUB_NAME").ToString <> "" Then
                    Line = Line & " " & "\4"
                    Line = Line & "$a" & TrimAll(dtLibrary.Rows(0).Item("SUB_NAME").ToString()) & "." & vbCrLf
                End If

                If dtLibrary.Rows(0).Item("KEYWORDS").ToString <> "" Then
                    If InStr(dtLibrary.Rows(0).Item("KEYWORDS").ToString, ";") <> 0 Then
                        Dim myKeyword As Object
                        myKeyword = TrimAll(dtLibrary.Rows(0).Item("KEYWORDS").ToString)
                        myKeyword = Split(myKeyword, ";")
                        Dim m As Integer
                        For m = 0 To UBound(myKeyword)
                            Line = Line & " " & "\4"
                            Line = Line & "$a" & TrimAll(myKeyword(m))
                            Line = Line & "." & vbCrLf
                        Next
                    Else
                        Line = Line & " " & "\4"
                        Line = Line & "$a" & TrimAll(dtLibrary.Rows(0).Item("KEYWORDS").ToString)
                        Line = Line & "." & vbCrLf
                    End If
                End If
                my650 = Line
                Line = ""

                '700 ADDED ENTRIES - Personal Name
                If dtLibrary.Rows(0).Item("AUTHOR2").ToString <> "" Or dtLibrary.Rows(0).Item("AUTHOR3").ToString <> "" Or dtLibrary.Rows(0).Item("EDITOR").ToString <> "" Or dtLibrary.Rows(0).Item("TRANSLATOR").ToString <> "" Or dtLibrary.Rows(0).Item("ILLUSTRATOR").ToString <> "" Then
                    If dtLibrary.Rows(0).Item("AUTHOR2").ToString <> "" Then
                        If InStr(dtLibrary.Rows(0).Item("AUTHOR2").ToString, ",") <> 0 Then
                            Line = Line & " " & "1\" & "$a" & TrimAll(Replace(dtLibrary.Rows(0).Item("AUTHOR2").ToString(), ".", " "))
                            Line = Line & ",$e" & "Jt.Author"
                        Else
                            Line = Line & " " & "0\" & "$a" & TrimAll(Replace(dtLibrary.Rows(0).Item("AUTHOR2").ToString(), ".", " "))
                            Line = Line & ",$e" & "Jt.Author"
                        End If
                        Line = Line & "." & vbCrLf
                    End If
                    If dtLibrary.Rows(0).Item("AUTHOR3").ToString <> "" Then
                        If InStr(dtLibrary.Rows(0).Item("AUTHOR3").ToString, ",") <> 0 Then
                            Line = Line & " " & "1\" & "$a" & TrimAll(Replace(dtLibrary.Rows(0).Item("AUTHOR3").ToString(), ".", " "))
                            Line = Line & ",$e" & "Jt.Author"
                        Else
                            Line = Line & " " & "0\" & "$a" & TrimAll(Replace(dtLibrary.Rows(0).Item("AUTHOR3").ToString(), ".", " "))
                            Line = Line & ",$e" & "Jt.Author"
                        End If
                        Line = Line & "." & vbCrLf
                    End If

                    If dtLibrary.Rows(0).Item("EDITOR").ToString <> "" Then
                        If InStr(dtLibrary.Rows(0).Item("EDITOR").ToString, ";") <> 0 Then
                            Dim myEditor As Object
                            myEditor = TrimAll(dtLibrary.Rows(0).Item("EDITOR").ToString)
                            myEditor = Split(myEditor, ";")
                            Dim m As Integer
                            For m = 0 To UBound(myEditor)
                                If InStr(myEditor(m), ",") <> 0 Then
                                    Line = Line & " " & "1\" & "$a" & TrimAll(Replace((myEditor(m)), ".", " "))
                                    Line = Line & ",$e" & "Ed." & vbCrLf
                                Else
                                    Line = Line & " " & "0\" & "$a" & TrimAll(Replace((myEditor(m)), ".", " "))
                                    Line = Line & ",$e" & "Ed." & vbCrLf
                                End If
                            Next
                        Else
                            If InStr(dtLibrary.Rows(0).Item("EDITOR").ToString, ",") <> 0 Then
                                Line = Line & " " & "1\" & "$a" & TrimAll(Replace(dtLibrary.Rows(0).Item("EDITOR").ToString(), ".", " "))
                                Line = Line & ",$e" & "Ed." & vbCrLf
                            Else
                                Line = Line & " " & "0\" & "$a" & TrimAll(Replace(dtLibrary.Rows(0).Item("EDITOR").ToString(), ".", " "))
                                Line = Line & ",$e" & "Ed." & vbCrLf
                            End If
                        End If
                    End If

                    If dtLibrary.Rows(0).Item("TRANSLATOR").ToString <> "" Then
                        If InStr(dtLibrary.Rows(0).Item("TRANSLATOR").ToString, ";") <> 0 Then
                            Dim myTr As Object
                            myTr = TrimAll(dtLibrary.Rows(0).Item("TRANSLATOR").ToString)
                            myTr = Split(myTr, ";")
                            Dim m As Integer
                            For m = 0 To UBound(myTr)
                                If InStr(myTr(m), ",") <> 0 Then
                                    Line = Line & " " & "1\" & "$a" & TrimAll(Replace((myTr(m)), ".", " "))
                                    Line = Line & ",$e" & "Tr." & vbCrLf
                                Else
                                    Line = Line & " " & "0\" & "$a" & TrimAll(Replace((myTr(m)), ".", " "))
                                    Line = Line & ",$e" & "Tr." & vbCrLf
                                End If
                            Next
                        Else
                            If InStr(dtLibrary.Rows(0).Item("TRANSLATOR").ToString, ",") <> 0 Then
                                Line = Line & " " & "1\" & "$a" & TrimAll(Replace(dtLibrary.Rows(0).Item("TRANSLATOR").ToString(), ".", " "))
                                Line = Line & ",$e" & "Tr." & vbCrLf
                            Else
                                Line = Line & " " & "0\" & "$a" & TrimAll(Replace(dtLibrary.Rows(0).Item("TRANSLATOR").ToString(), ".", " "))
                                Line = Line & ",$e" & "Tr." & vbCrLf
                            End If
                        End If
                    End If

                    If dtLibrary.Rows(0).Item("ILLUSTRATOR").ToString <> "" Then
                        If InStr(dtLibrary.Rows(0).Item("ILLUSTRATOR").ToString, ";") <> 0 Then
                            Dim myIllus As Object
                            myIllus = TrimAll(dtLibrary.Rows(0).Item("ILLUSTRATOR").ToString)
                            myIllus = Split(myIllus, ";")
                            Dim m As Integer
                            For m = 0 To UBound(myIllus)
                                If InStr(myIllus(m), ",") <> 0 Then
                                    Line = Line & " " & "1\" & "$a" & TrimAll(Replace((myIllus(m)), ".", " "))
                                    Line = Line & ",$e" & "Illus." & vbCrLf
                                Else
                                    Line = Line & " " & "0\" & "$a" & TrimAll(Replace((myIllus(m)), ".", " "))
                                    Line = Line & ",$e" & "Illus." & vbCrLf
                                End If
                            Next
                        Else
                            If InStr(dtLibrary.Rows(0).Item("ILLUSTRATOR").ToString, ",") <> 0 Then
                                Line = Line & " " & "1\" & "$a" & TrimAll(Replace(dtLibrary.Rows(0).Item("ILLUSTRATOR").ToString(), ".", " "))
                                Line = Line & ",$e" & "Illus." & vbCrLf
                            Else
                                Line = Line & " " & "0\" & "$a" & TrimAll(Replace(dtLibrary.Rows(0).Item("ILLUSTRATOR").ToString(), ".", " "))
                                Line = Line & ",$e" & "Illus." & vbCrLf
                            End If
                        End If
                    End If
                End If
                my700 = Line
                Line = ""

                '710 Added entry for Corporate Authro
                If dtLibrary.Rows(0).Item("AUTHOR1").ToString <> "" Then
                    If dtLibrary.Rows(0).Item("CORPORATE_AUTHOR").ToString <> "" Then
                        Line = Line & " " & "2\" & "$a" & TrimAll(dtLibrary.Rows(0).Item("CORPORATE_AUTHOR").ToString()) & "." & vbCrLf
                    End If
                End If
                my710 = Line
                Line = ""

                '711 Added entry for Conference / meeting
                If dtLibrary.Rows(0).Item("AUTHOR1").ToString <> "" Or dtLibrary.Rows(0).Item("CORPORATE_AUTHOR").ToString <> "" Then
                    If dtLibrary.Rows(0).Item("CONF_NAME").ToString <> "" Then
                        Line = Line & " " & "2\" & "$a" & TrimAll(dtLibrary.Rows(0).Item("CONF_NAME").ToString()) & "." & vbCrLf
                    End If
                End If
                my711 = Line
                Line = ""

                '800 Series Editor
                If dtLibrary.Rows(0).Item("SERIES_EDITOR").ToString <> "" Then
                    If InStr(dtLibrary.Rows(0).Item("SERIES_EDITOR").ToString, ";") <> 0 Then
                        Dim mySEditor As Object
                        mySEditor = TrimAll(dtLibrary.Rows(0).Item("SERIES_EDITOR").ToString)
                        mySEditor = Split(mySEditor, ";")
                        Dim m As Integer
                        For m = 0 To UBound(mySEditor)
                            If InStr(mySEditor(m), ",") <> 0 Then
                                Line = Line & " " & "1\" & "$a" & TrimAll(Replace((mySEditor(m)), ".", " "))
                                Line = Line & ".$t" & TrimAll(dtLibrary.Rows(0).Item("SERIES_TITLE").ToString()) & "." & vbCrLf
                            Else
                                Line = Line & " " & "0\" & "$a" & TrimAll(Replace((mySEditor(m)), ".", " "))
                                Line = Line & ".$t" & TrimAll(dtLibrary.Rows(0).Item("SERIES_TITLE").ToString()) & "." & vbCrLf
                            End If
                        Next
                    Else
                        If InStr(dtLibrary.Rows(0).Item("SERIES_EDITOR").ToString, ",") <> 0 Then
                            Line = Line & " " & "1\" & "$a" & TrimAll(Replace(dtLibrary.Rows(0).Item("SERIES_EDITOR").ToString(), ".", " "))
                            Line = Line & ".$t" & TrimAll(dtLibrary.Rows(0).Item("SERIES_TITLE").ToString()) & "." & vbCrLf
                        Else
                            Line = Line & " " & "0\" & "$a" & TrimAll(Replace(dtLibrary.Rows(0).Item("SERIES_EDITOR").ToString(), ".", " "))
                            Line = Line & ".$t" & TrimAll(dtLibrary.Rows(0).Item("SERIES_TITLE").ToString()) & "." & vbCrLf
                        End If
                    End If
                End If
                my800 = Line
                Line = ""

                '830 Add Series entry
                If dtLibrary.Rows(0).Item("SERIES_TITLE").ToString <> "" Then
                    Line = Line & " "
                    Line = Line & "\0"

                    Dim mySer As String = ""
                    mySer = TrimAll(dtLibrary.Rows(0).Item("SERIES_TITLE").ToString())
                    mySer = TrimAll(Replace(mySer, "a ", "", 1, 2))
                    mySer = TrimAll(Replace(mySer, "A ", "", 1, 2))
                    mySer = TrimAll(Replace(mySer, "an ", "", 1, 3))
                    mySer = TrimAll(Replace(mySer, "An ", "", 1, 3))
                    mySer = TrimAll(Replace(mySer, "AN ", "", 1, 3))
                    mySer = TrimAll(Replace(mySer, "aN ", "", 1, 3))
                    mySer = TrimAll(Replace(mySer, "The ", "", 1, 4))
                    mySer = TrimAll(Replace(mySer, "the ", "", 1, 4))
                    mySer = TrimAll(Replace(mySer, "THe ", "", 1, 4))
                    mySer = TrimAll(Replace(mySer, "THE ", "", 1, 4))
                    mySer = TrimAll(Replace(mySer, "tHe ", "", 1, 4))
                    mySer = TrimAll(Replace(mySer, "tHE ", "", 1, 4))
                    If Trim(mySer.Substring(mySer.Length - 1)) = ")" Then
                        Line = Line & "$a" & mySer & vbCrLf
                    Else
                        Line = Line & "$a" & mySer & "." & vbCrLf
                    End If
                End If
                my830 = Line
                Line = ""

                '850 Holding Institution                            
                Line = Line & " " & "\\" & "$a" & dtHold.Rows(0).Item("LIB_CODE").ToString & vbCrLf
                my850 = Line
                Line = ""

                '852 location on shelf  
                If dtHold.Rows(0).Item("PHYSICAL_LOCATION").ToString <> "" Then
                    Line = Line & " " & "\\" & "$a" & TrimAll(dtHold.Rows(0).Item("PHYSICAL_LOCATION").ToString) & vbCrLf
                End If
                my852 = Line
                Line = ""

                '856 URL  
                If dtLibrary.Rows(0).Item("URL").ToString <> "" Then
                    Line = Line & " " & "4\" & "$u" & TrimAll(dtLibrary.Rows(0).Item("URL").ToString) & vbCrLf
                End If
                my856 = Line
                Line = ""
            Else
                MsgBox("No record exists for conversion...")
            End If
        Catch s As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' there is error ..');", True)
        Finally

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
    'save TAGS field data 
    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        SaveData()
    End Sub
    Public Sub SaveData()
        '****************************************************************************************
        'Server Validation for Name
        Try

            Dim myNewTags As Object = Nothing
            myNewTags = RemoveQuotes(TrimAll(TextBox1.Text))
            If String.IsNullOrEmpty(myNewTags) Then
                ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Pleas Enter tags... ');", True)
                Me.TextBox1.Focus()
                Exit Sub
            End If
            myNewTags = RemoveQuotes(myNewTags)
            If myNewTags.Length > 101 Then
                ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Tags must be of Proper Length... ');", True)
                TextBox1.Focus()
                Exit Sub
            End If
            If InStr(1, myNewTags, "CREATE", 1) > 0 Or InStr(1, myNewTags, "DELETE", 1) > 0 Or InStr(1, myNewTags, "DROP", 1) > 0 Or InStr(1, myNewTags, "INSERT", 1) > 1 Or InStr(1, myNewTags, "TRACK", 1) > 1 Or InStr(1, myNewTags, "TRACE", 1) > 1 Then
                ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Reserve Word... ');", True)
                TextBox1.Focus()
                Exit Sub
            End If

            'check unwanted characters
            Dim iloop As Integer
            Dim strcurrentchar As Object
            Dim c As Integer
            Dim counter1 As Integer
            c = 0
            counter1 = 0
            For iloop = 1 To Len(myNewTags)
                strcurrentchar = Mid(myNewTags, iloop, 1)
                If c = 0 Then
                    If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                        c = c + 1
                        counter1 = 1
                    End If
                End If
            Next
            If counter1 = 1 Then
                ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Un-Wated Characters... ');", True)
                TextBox1.Focus()
                Exit Sub
            End If

            Dim myFinalTags As Object = Nothing
            If myTags <> "" Then
                myFinalTags = myTags + "; " + myNewTags
            Else
                myFinalTags = myNewTags
            End If

            myFinalTags = Replace(myFinalTags, ";", "; ")
            myFinalTags = Replace(myFinalTags, ";;", ";")
            myFinalTags = Replace(myFinalTags, "  ", " ")
            myFinalTags = RemoveQuotes(TrimAll(myFinalTags))

            'update THE RECORD IN TO THE DATABASE
            Dim SQL As String
            Dim Cmd As SqlCommand
            Dim da As SqlDataAdapter
            Dim ds As New DataSet
            Dim CB As SqlCommandBuilder
            SQL = "SELECT CAT_NO, TAGS FROM CATS WHERE (CAT_NO = '" & Trim(myCatNo) & "')"
            Cmd = New SqlCommand(SQL, SqlConn)
            da = New SqlDataAdapter(Cmd)
            CB = New SqlCommandBuilder(da)
            da.Fill(ds, "Cats")
            If Not String.IsNullOrEmpty(myFinalTags) Then
                ds.Tables("Cats").Rows(0)("TAGS") = myFinalTags.Trim
            Else
                ds.Tables("Cats").Rows(0)("TAGS") = System.DBNull.Value
            End If
            da.Update(ds, "Cats")

            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Thanks for Submitting Info... ');", True)
            Me.TextBox1.Focus()
        Catch s As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' there is error ..');", True)
        Finally

        End Try
    End Sub
    Protected Sub GridView2_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView2.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim CurrDate As Date = Nothing
            CurrDate = Now.Date
            CurrDate = Convert.ToDateTime(CurrDate, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert to indian 

            Dim DueDate As Object = Nothing
            If e.Row.Cells(7).Text <> "" Then
                If e.Row.Cells(7).Text = "Issued" Then
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
                End If
            End If




        End If
    End Sub
    'get micro-documents data
    Public Sub GetMicroDocuments()
        Try
            If myCatNo <> 0 Then
                Dim ds As New DataSet
                Dim SQL As String = Nothing
                If HOLD_LIB_CODE = "" Then
                    SQL = "SELECT * FROM ARTICLES WHERE (CAT_NO ='" & Trim(myCatNo) & "') ORDER BY LIB_CODE, ART_NO DESC; "
                Else
                    SQL = "SELECT * FROM ARTICLES WHERE (CAT_NO ='" & Trim(myCatNo) & "') AND (LIB_CODE = '" & Trim(HOLD_LIB_CODE) & "') ORDER BY ART_NO DESC; "
                End If

                Dim da As New SqlDataAdapter(SQL, SqlConn)
                da.Fill(ds)
                dtArticles = ds.Tables(0).Copy
                If dtArticles.Rows.Count = 0 Then
                    GridView3.DataSource = Nothing
                    GridView3.DataBind()
                    da.Dispose()
                    ds.Dispose()
                    DIV_MICRO.Visible = False
                Else
                    RecordCount = dtHold.Rows.Count
                    GridView3.DataSource = dtArticles
                    GridView3.DataBind()
                    GridView3.Caption = "Total Records : " & RecordCount
                    da.Dispose()
                    ds.Dispose()
                    DIV_MICRO.Visible = True
                End If
            End If
                ViewState("dtArticles") = dtArticles
                SqlConn.Close()
        Catch s As Exception
            'ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' there is error ..');", True)
        Finally
            SqlConn.Close()
        End Try
    End Sub

    Protected Sub GridView3_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView3.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim hl As HyperLink = e.Row.FindControl("HyperLink1")
            If e.Row.Cells(2).Text <> "" Then
                Dim FilePath As String
                Dim ART_NO As Integer = Nothing
                ART_NO = e.Row.Cells(2).Text
                FilePath = "~/DFILES/" & e.Row.Cells(3).Text & "/Articles/" & ART_NO & "/"
                FilePath = Replace(FilePath, "\", "/")
                FilePath = Replace(FilePath, "//", "/")
                If System.IO.Directory.Exists(Server.MapPath(FilePath)) = True Then
                    Dim myDir As DirectoryInfo = New DirectoryInfo(Server.MapPath(FilePath))

                    If (myDir.EnumerateFiles().Any()) Then
                        Dim files() As String = IO.Directory.GetFiles(Server.MapPath(FilePath))
                        For Each sFile As String In files
                            Dim FileName As String = System.IO.Path.GetFileName(sFile) ' Text &= IO.File.ReadAllText(sFile)
                            Dim strURL As String = "~/DFILES/" & e.Row.Cells(3).Text & "/Articles/" & ART_NO & "/" & FileName '"AM51357293_2014-03-10_16-48-22.pdf"
                            hl.Visible = True
                            hl.NavigateUrl = strURL
                            hl.Target = "_new"
                            ' hl.ImageUrl = "~/Images/pdf.ico"

                            Dim FileExtn As String = Nothing
                            FileExtn = System.IO.Path.GetExtension(FileName.ToString())
                            If LCase(FileExtn) = ".bmp" Or FileExtn = ".emf" Or FileExtn = ".exif" Or FileExtn = ".gif" Or FileExtn = ".icon" Or FileExtn = ".jpeg" Or FileExtn = ".jpg" Or FileExtn = ".png" Or FileExtn = ".tiff" Or FileExtn = ".tif" Or FileExtn = ".wmf" Then
                                hl.ImageUrl = "~/Images/btn_edit.gif"
                            ElseIf LCase(FileExtn) = ".htm" Or LCase(FileExtn) = ".html" Then
                                hl.ImageUrl = "~/Images/ie.bmp"
                            ElseIf LCase(FileExtn) = ".doc" Or LCase(FileExtn) = ".docx" Then
                                hl.ImageUrl = "~/Images/word.png"
                            Else
                                hl.ImageUrl = "~/Images/pdf.png"
                            End If

                          



                        Next
                    Else
                        hl.Visible = False
                    End If
                Else
                    hl.Visible = False
                End If
            End If

            
        End If
    End Sub
End Class