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

Public Class JApprovals
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
                        Me.App_Save_Bttn.Visible = True
                        App_Cancel_Bttn.Visible = True
                        App_Update_Bttn.Visible = False
                        App_DeleteAll_Bttn.Visible = False
                        Label15.Text = "Enter Data and Press SAVE Button to save the record.."
                        Label6.Text = ""
                        SearchTitle()
                        PopulateAcqModes()
                        PopulateLetters()
                        DDL_AcqModes.SelectedValue = "P"
                        PopulateCurrencies()
                        DDL_Currencies.SelectedValue = "INR"
                    End If
                    Dim CreateSUserButton As Object 'System.Web.UI.WebControls.Button
                    CreateSUserButton = Master.FindControl("Accordion1").FindControl("SerPane").FindControl("Ser_Approval_Bttn")
                    CreateSUserButton.ForeColor = Drawing.Color.Red
                    myPaneName = "SerPane"
                End If
            Else
                Response.Redirect("~/Default.aspx", False)
            End If
            LibCode = Session.Item("LoggedLibcode")
        Catch ex As Exception
            Label6.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    Private Sub Approvals_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        If MultiView1.ActiveViewIndex = 0 Then
            DDL_Titles.Focus()
        End If
    End Sub
    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As MenuEventArgs) Handles Menu1.MenuItemClick
        MultiView1.ActiveViewIndex = Int32.Parse(e.Item.Value)
        If MultiView1.ActiveViewIndex = 0 Then
            Menu1.Items(0).ImageUrl = "~/Images/button1up.png"
            Menu1.Items(1).ImageUrl = "~/Images/button2over.png"
            Menu1.Items(2).ImageUrl = "~/Images/button3over.png"
            DDL_Titles.ClearSelection()
            DDL_Titles_SelectedIndexChanged(sender, e)
            DDL_Titles.Focus()
        End If
        If MultiView1.ActiveViewIndex = 1 Then 'generate approval
            Menu1.Items(0).ImageUrl = "~/Images/button1over.png"
            Menu1.Items(1).ImageUrl = "~/Images/button2up.png"
            Menu1.Items(2).ImageUrl = "~/Images/button3over.png"
            PopulateAppNo()
            Grid2.DataSource = Nothing
            Grid2.DataBind()
            PopulateComiitees()
            DDL_Approvals.Focus()
        End If
        If MultiView1.ActiveViewIndex = 2 Then
            Menu1.Items(0).ImageUrl = "~/Images/button1over.png"
            Menu1.Items(1).ImageUrl = "~/Images/button2over.png"
            Menu1.Items(2).ImageUrl = "~/Images/button3up.png"
            GetApprovalList3()
            PopulateComiitees3()
            Grid3.DataSource = Nothing
            Grid3.DataBind()
            DDL_Approval3.Focus()
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
    'Search Catalog
    Public Sub Search_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Search_Bttn.Click
        SearchTitle()
    End Sub
    Public Sub SearchTitle()
        Dim dtSearch As DataTable = Nothing
        Try
            Dim c, counter1, counter2, counter3, counter4, counter5 As Integer
            Dim iloop As Integer
            Dim strcurrentchar As Object

            'search string validation
            Dim mySearchString As Object = Nothing
            If txt_Search_SearchString.Text <> "" Then
                mySearchString = TrimAll(txt_Search_SearchString.Text)
                mySearchString = RemoveQuotes(mySearchString)
                If mySearchString.Length > 250 Then
                    Label6.Text = "Error:  Input is not Valid!"
                    Label15.Text = ""
                    Me.txt_Search_SearchString.Focus()
                    Exit Sub
                End If
                mySearchString = " " & mySearchString & " "
                If InStr(1, mySearchString, "CREATE", 1) > 0 Or InStr(1, mySearchString, "DELETE", 1) > 0 Or InStr(1, mySearchString, "DROP", 1) > 0 Or InStr(1, mySearchString, "INSERT", 1) > 1 Or InStr(1, mySearchString, "TRACK", 1) > 1 Or InStr(1, mySearchString, "TRACE", 1) > 1 Then
                    Label6.Text = "Error:  Input is not Valid !"
                    Label15.Text = ""
                    Me.txt_Search_SearchString.Focus()
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
                    Label6.Text = "Error: data is not Valid !"
                    Label15.Text = ""
                    Me.txt_Search_SearchString.Focus()
                    Exit Sub
                End If
            Else
                mySearchString = String.Empty
            End If

            'Field Name validation
            Dim myfield As String = Nothing
            If DropDownList5.Text <> "" Then
                myfield = TrimAll(DropDownList5.SelectedValue)
                myfield = RemoveQuotes(myfield)
                If myfield.Length > 50 Then
                    Label6.Text = "Error: Input is not Valid !"
                    Label15.Text = ""
                    Me.DropDownList5.Focus()
                    Exit Sub
                End If
                myfield = " " & myfield & " "
                If InStr(1, myfield, "CREATE", 1) > 0 Or InStr(1, myfield, "DELETE", 1) > 0 Or InStr(1, myfield, "DROP", 1) > 0 Or InStr(1, myfield, "INSERT", 1) > 1 Or InStr(1, myfield, "TRACK", 1) > 1 Or InStr(1, myfield, "TRACE", 1) > 1 Then
                    Label6.Text = "Error: Input is not Valid !"
                    Label15.Text = ""
                    Me.DropDownList5.Focus()
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
                    Label6.Text = "Error: Input is not Valid !"
                    Label15.Text = ""
                    DropDownList5.Focus()
                    Exit Sub
                End If
            Else
                myfield = "CAT_NO"
            End If


            'Boolean Operator validation
            Dim myBoolean As String = Nothing
            If DropDownList6.Text <> "" Then
                myBoolean = TrimAll(DropDownList6.SelectedValue)
                myBoolean = RemoveQuotes(myBoolean)
                If myBoolean.Length > 20 Then
                    Label6.Text = "Error: Input is not Valid !"
                    Label15.Text = ""
                    Me.DropDownList6.Focus()
                    Exit Sub
                End If
                myBoolean = " " & myBoolean & " "
                If InStr(1, myBoolean, "CREATE", 1) > 0 Or InStr(1, myBoolean, "DELETE", 1) > 0 Or InStr(1, myBoolean, "DROP", 1) > 0 Or InStr(1, myBoolean, "INSERT", 1) > 1 Or InStr(1, myBoolean, "TRACK", 1) > 1 Or InStr(1, myBoolean, "TRACE", 1) > 1 Then
                    Label6.Text = "Error: Input is not Valid !"
                    Label15.Text = ""
                    Me.DropDownList6.Focus()
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
                    Label6.Text = "Error: Input is not Valid !"
                    Label15.Text = ""
                    DropDownList6.Focus()
                    Exit Sub
                End If
            Else
                myBoolean = "AND"
            End If

            '**********************************************************************************
            Dim SQL As String = Nothing

            If txt_Search_SearchString.Text <> "" Then
                If myfield = "CAT_NO" Then
                    If IsNumeric(mySearchString) = False Then
                        Label6.Text = "Cat Number value must be Numeric Only!"
                        Label15.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Cat No must be Numeric Only ');", True)
                        Exit Sub
                    Else
                        SQL = "SELECT *  FROM CATS WHERE (CAT_NO IS NOT NULL) AND (BIB_CODE ='S') AND (CAT_NO = '" & Trim(mySearchString) & "') "
                    End If
                End If

                If myfield = "ACCESSION_NO" Then
                    SQL = "SELECT *  FROM CATS where BIB_CODE ='S' AND CAT_NO = (SELECT CAT_NO FROM HOLDINGS WHERE (LIB_CODE ='" & Trim(LibCode) & "'  AND ACCESSION_NO ='" & mySearchString & "'))"
                End If

                If myfield.ToString <> "CAT_NO" And myfield.ToString <> "ACCESSION_NO" Then
                    SQL = "SELECT CAT_NO, TITLE FROM CATS_AUTHORS_VIEW WHERE (CAT_NO IS NOT NULL) AND (BIB_CODE ='S') "
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
            End If
            If SQL <> "" Then
                SQL = SQL & " ORDER BY TITLE ASC "
            Else
                SQL = "select  CAT_NO, TITLE from CATS WHERE (CAT_NO IS NOT NULL) AND (BIB_CODE ='S') ORDER BY TITLE "
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
                Label1.Text = "Total Record(s): 0 "
            Else
                RecordCount = dtSearch.Rows.Count
                Me.DDL_Titles.DataSource = dtSearch
                Me.DDL_Titles.DataTextField = "TITLE"
                Me.DDL_Titles.DataValueField = "CAT_NO"
                Me.DDL_Titles.DataBind()
                DDL_Titles.Items.Insert(0, "")
                Label1.Text = "Total Record(s): " & RecordCount
                DDL_Titles.Focus()
            End If
            ViewState("dt") = dtSearch
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
            Label15.Text = ""
        Finally
            SqlConn.Close()
            DDL_Titles.Focus()
        End Try
    End Sub
    'display Cat Record   
    Public Sub txt_Search_SearchString_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txt_Search_SearchString.TextChanged
        Me.SearchTitle()
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
            PopulateGrid()
        Catch ex As Exception
            Label6.Text = "Error: " & (ex.Message())
            Label15.Text = ""
        Finally
            dt.Dispose()
            SqlConn.Close()
            DDL_Titles.Focus()
        End Try
    End Sub
    Public Sub PopulateAcqModes()
        Me.DDL_AcqModes.DataTextField = "ACQMODE_NAME"
        Me.DDL_AcqModes.DataValueField = "ACQMODE_CODE"
        Me.DDL_AcqModes.DataSource = GetAcqModesList()
        Me.DDL_AcqModes.DataBind()
    End Sub
    Public Sub PopulateCurrencies()
        Me.DDL_Currencies.DataSource = GetCurrenciesList()
        Me.DDL_Currencies.DataTextField = "CUR_NAME"
        Me.DDL_Currencies.DataValueField = "CUR_CODE"
        Me.DDL_Currencies.DataBind()
        DDL_Currencies.Items.Insert(0, "")
    End Sub
    <System.Web.Script.Services.ScriptMethod(), _
System.Web.Services.WebMethod()> _
    Public Shared Function SearchAppNo(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim sdr As SqlDataReader
        Dim cmd As SqlCommand = New SqlCommand
        cmd.CommandText = "select DISTINCT APP_NO from ACQUISITIONS where (APP_NO like '" + prefixText + "%') and (PROCESS_STATUS ='Requested') AND (LIB_CODE ='" & LibCode & "')"
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
    'save record
    Protected Sub App_Save_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles App_Save_Bttn.Click
        If IsPostBack = True Then
            'check unwanted characters
            Dim iloop As Integer
            Dim strcurrentchar As Object
            Dim c As Integer
            Dim counter1, counter2, counter3, counter4, counter5, counter6, counter7, counter8, counter9 As Integer
            Dim thisTransaction As SqlClient.SqlTransaction = Nothing
            Try
                If DDL_Titles.Text = "" Then
                    Label6.Text = "Plz Select Title from Drop-Down!"
                    Label15.Text = ""
                    DDL_Titles.Focus()
                    Exit Sub
                End If

                'validation for App No
                Dim APP_NO As Object = Nothing
                If Me.txt_Acq_AppNo.Text <> "" Then
                    APP_NO = TrimX(txt_Acq_AppNo.Text)
                    APP_NO = RemoveQuotes(APP_NO)
                    If APP_NO.Length > 50 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Acq_AppNo.Focus()
                        Exit Sub
                    End If
                    APP_NO = " " & APP_NO & " "
                    If InStr(1, APP_NO, "CREATE", 1) > 0 Or InStr(1, APP_NO, "DELETE", 1) > 0 Or InStr(1, APP_NO, "DROP", 1) > 0 Or InStr(1, APP_NO, "INSERT", 1) > 1 Or InStr(1, APP_NO, "TRACK", 1) > 1 Or InStr(1, APP_NO, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Acq_AppNo.Focus()
                        Exit Sub
                    End If
                    APP_NO = TrimX(UCase(APP_NO))
                    'check unwanted characters
                    c = 0
                    counter1 = 0
                    For iloop = 1 To Len(APP_NO)
                        strcurrentchar = Mid(APP_NO, iloop, 1)
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
                        txt_Acq_AppNo.Focus()
                        Exit Sub
                    End If

                    'check this app no status
                    Dim str As Object = Nothing
                    Dim flag As Object = Nothing
                    str = "SELECT APP_NO FROM ACQUISITIONS WHERE (APP_NO ='" & Trim(APP_NO) & "') AND (PROCESS_STATUS<>'Requested') AND (LIB_CODE ='" & Trim(LibCode) & "')"
                    Dim cmd1 As New SqlCommand(str, SqlConn)
                    SqlConn.Open()
                    flag = cmd1.ExecuteScalar
                    SqlConn.Close()
                    If flag <> Nothing Then
                        Label6.Text = "This Approval No has already been processed, Use another one !"
                        Label15.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('This Approval No has already been processed, Use another one !');", True)
                        txt_Acq_AppNo.Focus()
                        Exit Sub
                    End If
                Else
                    Label6.Text = "Plz Enter the Approval Number !"
                    Label15.Text = ""
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Plz Enter the Approval Number !');", True)
                    txt_Acq_AppNo.Focus()
                    Exit Sub
                End If

                'validation for Subscription eyar No
                Dim SUBS_YEAR As Integer = Nothing
                If Me.txt_Acq_SubsYear.Text <> "" Then
                    SUBS_YEAR = TrimX(txt_Acq_SubsYear.Text)
                    SUBS_YEAR = RemoveQuotes(SUBS_YEAR)
                    If SUBS_YEAR.ToString.Length > 5 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Acq_SubsYear.Focus()
                        Exit Sub
                    End If
                    SUBS_YEAR = " " & SUBS_YEAR & " "
                    If InStr(1, SUBS_YEAR, "CREATE", 1) > 0 Or InStr(1, SUBS_YEAR, "DELETE", 1) > 0 Or InStr(1, SUBS_YEAR, "DROP", 1) > 0 Or InStr(1, SUBS_YEAR, "INSERT", 1) > 1 Or InStr(1, SUBS_YEAR, "TRACK", 1) > 1 Or InStr(1, SUBS_YEAR, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Acq_SubsYear.Focus()
                        Exit Sub
                    End If
                    SUBS_YEAR = TrimX(SUBS_YEAR)
                    'check unwanted characters
                    c = 0
                    counter2 = 0
                    For iloop = 1 To Len(SUBS_YEAR)
                        strcurrentchar = Mid(SUBS_YEAR, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter2 = 1
                            End If
                        End If
                    Next
                    If counter2 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Acq_SubsYear.Focus()
                        Exit Sub
                    End If
                Else
                    SUBS_YEAR = Now.Year
                End If

                'check this app no for same year
                Dim str2 As Object = Nothing
                Dim flag2 As Object = Nothing
                str2 = "SELECT APP_NO FROM ACQUISITIONS WHERE (CAT_NO ='" & Trim(Label19.Text) & "') AND (SUBS_YEAR='" & Trim(SUBS_YEAR) & "') AND (LIB_CODE ='" & Trim(LibCode) & "')"
                Dim cmd2 As New SqlCommand(str2, SqlConn)
                SqlConn.Open()
                flag2 = cmd2.ExecuteScalar
                SqlConn.Close()
                If flag2 <> Nothing Then
                    Label6.Text = "Acqusition Record for this Subscription Year Already Exists!"
                    Label15.Text = ""
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Acqusition Record for this Subscription Year Already Exists!');", True)
                    txt_Acq_AppNo.Focus()
                    Exit Sub
                End If

                'validation for copy
                Dim COPY_PROPOSED As Integer = Nothing
                If Me.txt_Acq_Copy.Text <> "" Then
                    COPY_PROPOSED = Convert.ToInt16(TrimX(txt_Acq_Copy.Text))
                    COPY_PROPOSED = RemoveQuotes(COPY_PROPOSED)
                    If Not IsNumeric(COPY_PROPOSED) = True Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Acq_Copy.Focus()
                        Exit Sub
                    End If
                    If Len(COPY_PROPOSED) > 5 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Acq_Copy.Focus()
                        Exit Sub
                    End If
                    COPY_PROPOSED = " " & COPY_PROPOSED & " "
                    If InStr(1, COPY_PROPOSED, "CREATE", 1) > 0 Or InStr(1, COPY_PROPOSED, "DELETE", 1) > 0 Or InStr(1, COPY_PROPOSED, "DROP", 1) > 0 Or InStr(1, COPY_PROPOSED, "INSERT", 1) > 1 Or InStr(1, COPY_PROPOSED, "TRACK", 1) > 1 Or InStr(1, COPY_PROPOSED, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Acq_Copy.Focus()
                        Exit Sub
                    End If
                    COPY_PROPOSED = TrimAll(COPY_PROPOSED)
                    'check unwanted characters
                    c = 0
                    counter3 = 0
                    For iloop = 1 To Len(COPY_PROPOSED.ToString)
                        strcurrentchar = Mid(COPY_PROPOSED, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter3 = 1
                            End If
                        End If
                    Next
                    If counter3 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Acq_Copy.Focus()
                        Exit Sub
                    End If
                Else
                    COPY_PROPOSED = 1
                End If

                'validation for Currency code
                Dim CUR_CODE As Object = Nothing
                CUR_CODE = DDL_Currencies.SelectedValue
                If Not String.IsNullOrEmpty(CUR_CODE) Then
                    CUR_CODE = RemoveQuotes(CUR_CODE)
                    If CUR_CODE.Length > 4 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_Currencies.Focus()
                        Exit Sub
                    End If
                    CUR_CODE = " " & CUR_CODE & " "
                    If InStr(1, CUR_CODE, "CREATE", 1) > 0 Or InStr(1, CUR_CODE, "DELETE", 1) > 0 Or InStr(1, CUR_CODE, "DROP", 1) > 0 Or InStr(1, CUR_CODE, "INSERT", 1) > 1 Or InStr(1, CUR_CODE, "TRACK", 1) > 1 Or InStr(1, CUR_CODE, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_Currencies.Focus()
                        Exit Sub
                    End If
                    CUR_CODE = TrimX(CUR_CODE)
                    'check unwanted characters
                    c = 0
                    counter4 = 0
                    For iloop = 1 To Len(CUR_CODE)
                        strcurrentchar = Mid(CUR_CODE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter4 = 1
                            End If
                        End If
                    Next
                    If counter4 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_Currencies.Focus()
                        Exit Sub
                    End If
                Else
                    CUR_CODE = "INR"
                End If

                'validation for Currency code
                Dim ITEM_PRICE As Object = Nothing
                ITEM_PRICE = TrimX(txt_Acq_ItemPrice.Text)
                If Not String.IsNullOrEmpty(ITEM_PRICE) Then
                    ITEM_PRICE = RemoveQuotes(ITEM_PRICE)
                    If ITEM_PRICE.Length > 10 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Acq_ItemPrice.Focus()
                        Exit Sub
                    End If
                    ITEM_PRICE = " " & ITEM_PRICE & " "
                    If InStr(1, ITEM_PRICE, "CREATE", 1) > 0 Or InStr(1, ITEM_PRICE, "DELETE", 1) > 0 Or InStr(1, ITEM_PRICE, "DROP", 1) > 0 Or InStr(1, ITEM_PRICE, "INSERT", 1) > 1 Or InStr(1, ITEM_PRICE, "TRACK", 1) > 1 Or InStr(1, ITEM_PRICE, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Acq_ItemPrice.Focus()
                        Exit Sub
                    End If
                    ITEM_PRICE = TrimX(ITEM_PRICE)
                    'check unwanted characters
                    c = 0
                    counter5 = 0
                    For iloop = 1 To Len(ITEM_PRICE)
                        strcurrentchar = Mid(ITEM_PRICE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter5 = 1
                            End If
                        End If
                    Next
                    If counter5 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Acq_ItemPrice.Focus()
                        Exit Sub
                    End If
                Else
                    Label6.Text = "Plz Enter the Item Price !"
                    Label15.Text = ""
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Plz Enter the Item Price !');", True)
                    txt_Acq_ItemPrice.Focus()
                    Exit Sub
                End If

                'validation for Currency code
                Dim ACQMODE_CODE As Object = Nothing
                ACQMODE_CODE = DDL_AcqModes.SelectedValue
                If Not String.IsNullOrEmpty(ACQMODE_CODE) Then
                    ACQMODE_CODE = RemoveQuotes(ACQMODE_CODE)
                    If ACQMODE_CODE.Length > 2 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_AcqModes.Focus()
                        Exit Sub
                    End If
                    ACQMODE_CODE = " " & ACQMODE_CODE & " "
                    If InStr(1, ACQMODE_CODE, "CREATE", 1) > 0 Or InStr(1, ACQMODE_CODE, "DELETE", 1) > 0 Or InStr(1, ACQMODE_CODE, "DROP", 1) > 0 Or InStr(1, ACQMODE_CODE, "INSERT", 1) > 1 Or InStr(1, ACQMODE_CODE, "TRACK", 1) > 1 Or InStr(1, ACQMODE_CODE, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_AcqModes.Focus()
                        Exit Sub
                    End If
                    ACQMODE_CODE = TrimX(ACQMODE_CODE)
                    'check unwanted characters
                    c = 0
                    counter6 = 0
                    For iloop = 1 To Len(ACQMODE_CODE)
                        strcurrentchar = Mid(ACQMODE_CODE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter6 = 1
                            End If
                        End If
                    Next
                    If counter6 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_AcqModes.Focus()
                        Exit Sub
                    End If
                Else
                    ACQMODE_CODE = "P"
                End If

                'validation for reccommednd by
                Dim RECOMMENDED_BY As Object = Nothing
                If Me.txt_Acq_RecommendedBy.Text <> "" Then
                    RECOMMENDED_BY = TrimAll(txt_Acq_RecommendedBy.Text)
                    RECOMMENDED_BY = RemoveQuotes(RECOMMENDED_BY)
                    If RECOMMENDED_BY.Length > 100 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Acq_RecommendedBy.Focus()
                        Exit Sub
                    End If
                    RECOMMENDED_BY = " " & RECOMMENDED_BY & " "
                    If InStr(1, RECOMMENDED_BY, "CREATE", 1) > 0 Or InStr(1, RECOMMENDED_BY, "DELETE", 1) > 0 Or InStr(1, RECOMMENDED_BY, "DROP", 1) > 0 Or InStr(1, RECOMMENDED_BY, "INSERT", 1) > 1 Or InStr(1, RECOMMENDED_BY, "TRACK", 1) > 1 Or InStr(1, RECOMMENDED_BY, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Acq_RecommendedBy.Focus()
                        Exit Sub
                    End If
                    RECOMMENDED_BY = TrimAll(RECOMMENDED_BY)
                    'check unwanted characters
                    c = 0
                    counter7 = 0
                    For iloop = 1 To Len(RECOMMENDED_BY)
                        strcurrentchar = Mid(RECOMMENDED_BY, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter7 = 1
                            End If
                        End If
                    Next
                    If counter7 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Acq_RecommendedBy.Focus()
                        Exit Sub
                    End If
                Else
                    RECOMMENDED_BY = ""
                End If

                'search start date
                Dim RECOMMENDED_DATE As Object = Nothing
                If txt_Acq_RecommendedDate.Text <> "" Then
                    RECOMMENDED_DATE = TrimX(txt_Acq_RecommendedDate.Text)
                    RECOMMENDED_DATE = RemoveQuotes(RECOMMENDED_DATE)
                    RECOMMENDED_DATE = Convert.ToDateTime(RECOMMENDED_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                    If Len(RECOMMENDED_DATE) > 12 Then
                        Label6.Text = " Input is not Valid..."
                        Me.txt_Acq_RecommendedDate.Focus()
                        Exit Sub
                    End If
                    RECOMMENDED_DATE = " " & RECOMMENDED_DATE & " "
                    If InStr(1, RECOMMENDED_DATE, "CREATE", 1) > 0 Or InStr(1, RECOMMENDED_DATE, "DELETE", 1) > 0 Or InStr(1, RECOMMENDED_DATE, "DROP", 1) > 0 Or InStr(1, RECOMMENDED_DATE, "INSERT", 1) > 1 Or InStr(1, RECOMMENDED_DATE, "TRACK", 1) > 1 Or InStr(1, RECOMMENDED_DATE, "TRACE", 1) > 1 Then
                        Label6.Text = "  Input is not Valid... "
                        Me.txt_Acq_RecommendedDate.Focus()
                        Exit Sub
                    End If
                    RECOMMENDED_DATE = TrimX(RECOMMENDED_DATE)
                    'check unwanted characters
                    c = 0
                    counter8 = 0
                    For iloop = 1 To Len(RECOMMENDED_DATE)
                        strcurrentchar = Mid(RECOMMENDED_DATE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter8 = 1
                            End If
                        End If
                    Next
                    If counter8 = 1 Then
                        Label6.Text = "data is not Valid... "
                        Me.txt_Acq_RecommendedDate.Focus()
                        Exit Sub
                    End If
                Else
                    RECOMMENDED_DATE = ""
                End If

                If RECOMMENDED_BY <> "" And RECOMMENDED_DATE = "" Then
                    RECOMMENDED_DATE = Now.Date
                End If

                'Server validation for  : txt_Cat_Remarks
                Dim NOTE As Object = Nothing
                If txt_Acq_Remarks.Text <> "" Then
                    NOTE = TrimAll(txt_Acq_Remarks.Text)
                    NOTE = RemoveQuotes(NOTE)
                    If NOTE.Length > 500 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Acq_Remarks.Focus()
                        Exit Sub
                    End If

                    NOTE = " " & NOTE & " "
                    If InStr(1, NOTE, "CREATE", 1) > 0 Or InStr(1, NOTE, "DELETE", 1) > 0 Or InStr(1, NOTE, "DROP", 1) > 0 Or InStr(1, NOTE, "INSERT", 1) > 1 Or InStr(1, NOTE, "TRACK", 1) > 1 Or InStr(1, NOTE, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Acq_Remarks.Focus()
                        Exit Sub
                    End If
                    NOTE = TrimAll(NOTE)
                    'check unwanted characters
                    c = 0
                    counter9 = 0
                    For iloop = 1 To Len(NOTE.ToString)
                        strcurrentchar = Mid(NOTE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter9 = 1
                            End If
                        End If
                    Next
                    If counter9 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Acq_Remarks.Focus()
                        Exit Sub
                    End If
                Else
                    NOTE = ""
                End If

                Dim CAT_NO As Integer = Nothing
                If Label19.Text <> "" Then
                    CAT_NO = Trim(Label19.Text)
                Else
                    Label6.Text = "Title Record not selected  "
                    Label15.Text = ""
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Plz Select Title from Drop-Down and press ENTER !');", True)
                    Me.DDL_Titles.Focus()
                    Exit Sub
                End If

                Dim LIB_CODE As Object = Nothing
                If Session.Item("LoggedLibcode") <> "" Then
                    LIB_CODE = Session.Item("LoggedLibcode")
                Else
                    Label6.Text = "No Library Code Exists..Login Again  "
                    Label15.Text = ""
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('No Library Code Exists..Login Again !');", True)
                    Exit Sub
                End If

                Dim USER_CODE As Object = Nothing
                USER_CODE = Session.Item("LoggedUser")

                Dim DATE_ADDED As Object = Nothing
                DATE_ADDED = Now.Date
                DATE_ADDED = Convert.ToDateTime(DATE_ADDED, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                Dim PROCESS_STATUS As Object = Nothing
                PROCESS_STATUS = "Requested"

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
                objCommand.CommandText = "INSERT INTO ACQUISITIONS (CAT_NO, LIB_CODE, APP_NO, SUBS_YEAR, COPY_PROPOSED, CUR_CODE, ITEM_PRICE, RECOMMENDED_BY, RECOMMENDED_DATE, NOTE, PROCESS_STATUS, DATE_ADDED, USER_CODE, ACQMODE_CODE,IP) " & _
                                 " VALUES (@CAT_NO, @LIB_CODE, @APP_NO, @SUBS_YEAR, @COPY_PROPOSED, @CUR_CODE, @ITEM_PRICE, @RECOMMENDED_BY, @RECOMMENDED_DATE, @NOTE, @PROCESS_STATUS, @DATE_ADDED, @USER_CODE, @ACQMODE_CODE,@IP);  " & _
                                 " SELECT SCOPE_IDENTITY()"

                objCommand.Parameters.Add("@CAT_NO", SqlDbType.Int)
                objCommand.Parameters("@CAT_NO").Value = CAT_NO

                If LIB_CODE = "" Then LIB_CODE = System.DBNull.Value
                objCommand.Parameters.Add("@LIB_CODE", SqlDbType.NVarChar)
                objCommand.Parameters("@LIB_CODE").Value = LIB_CODE

                If APP_NO = "" Then APP_NO = System.DBNull.Value
                objCommand.Parameters.Add("@APP_NO", SqlDbType.NVarChar)
                objCommand.Parameters("@APP_NO").Value = APP_NO

                objCommand.Parameters.Add("@SUBS_YEAR", SqlDbType.Int)
                If SUBS_YEAR = 0 Then
                    objCommand.Parameters("@SUBS_YEAR").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@SUBS_YEAR").Value = SUBS_YEAR
                End If


                objCommand.Parameters.Add("@COPY_PROPOSED", SqlDbType.Int)
                objCommand.Parameters("@COPY_PROPOSED").Value = COPY_PROPOSED

                If CUR_CODE = "" Then CUR_CODE = System.DBNull.Value
                objCommand.Parameters.Add("@CUR_CODE", SqlDbType.NVarChar)
                objCommand.Parameters("@CUR_CODE").Value = CUR_CODE

                If ITEM_PRICE = "" Then ITEM_PRICE = System.DBNull.Value
                objCommand.Parameters.Add("@ITEM_PRICE", SqlDbType.Decimal)
                objCommand.Parameters("@ITEM_PRICE").Value = ITEM_PRICE

                objCommand.Parameters.Add("@RECOMMENDED_BY", SqlDbType.NVarChar)
                If RECOMMENDED_BY = "" Then
                    objCommand.Parameters("@RECOMMENDED_BY").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@RECOMMENDED_BY").Value = RECOMMENDED_BY
                End If

                objCommand.Parameters.Add("@RECOMMENDED_DATE", SqlDbType.DateTime)
                If RECOMMENDED_DATE = "" Then
                    objCommand.Parameters("@RECOMMENDED_DATE").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@RECOMMENDED_DATE").Value = RECOMMENDED_DATE
                End If

                objCommand.Parameters.Add("@NOTE", SqlDbType.NVarChar)
                If NOTE = "" Then
                    objCommand.Parameters("@NOTE").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@NOTE").Value = NOTE
                End If


                If PROCESS_STATUS = "" Then PROCESS_STATUS = System.DBNull.Value
                objCommand.Parameters.Add("@PROCESS_STATUS", SqlDbType.VarChar)
                objCommand.Parameters("@PROCESS_STATUS").Value = PROCESS_STATUS

                If DATE_ADDED = "" Then DATE_ADDED = System.DBNull.Value
                objCommand.Parameters.Add("@DATE_ADDED", SqlDbType.DateTime)
                objCommand.Parameters("@DATE_ADDED").Value = DATE_ADDED

                If USER_CODE = "" Then USER_CODE = System.DBNull.Value
                objCommand.Parameters.Add("@USER_CODE", SqlDbType.NVarChar)
                objCommand.Parameters("@USER_CODE").Value = USER_CODE

                If IP = "" Then IP = System.DBNull.Value
                objCommand.Parameters.Add("@IP", SqlDbType.VarChar)
                objCommand.Parameters("@IP").Value = IP

                If ACQMODE_CODE = "" Then ACQMODE_CODE = System.DBNull.Value
                objCommand.Parameters.Add("@ACQMODE_CODE", SqlDbType.VarChar)
                objCommand.Parameters("@ACQMODE_CODE").Value = ACQMODE_CODE

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
                    objCommand4.CommandText = "UPDATE CATS SET CAT_LEVEL ='Partial' WHERE (CAT_NO = @CAT_NO AND CAT_LEVEL = 'New')"

                    objCommand4.Parameters.Add("@CAT_NO", SqlDbType.Int)
                    objCommand4.Parameters("@CAT_NO").Value = CAT_NO

                    Dim dr4 As SqlDataReader
                    dr4 = objCommand4.ExecuteReader()
                    dr4.Close()
                End If

                thisTransaction.Commit()
                SqlConn.Close()

                Label15.Text = "Record Added Successfully! " & "Acq ID: " & intValue.ToString
                Label6.Text = ""
                Me.App_Save_Bttn.Visible = True
                App_Update_Bttn.Visible = False

                txt_Acq_ItemPrice.Text = ""
                txt_Acq_RecommendedBy.Text = ""
                txt_Acq_Remarks.Text = ""
                txt_Acq_RecommendedDate.Text = ""
                PopulateGrid()
                DDL_Titles.Focus()
            Catch q As SqlException
                thisTransaction.Rollback()
                Label6.Text = "Database Error -SAVE: " & (q.Message() & ", " & q.Number & ", " & q.GetType.ToString)
            Catch ex As Exception
                Label6.Text = "Error-SAVE: " & (ex.Message())
                Label15.Text = ""
            Finally
                SqlConn.Close()
            End Try
        End If
    End Sub
    Public Sub ClearFields()
        txt_Acq_Copy.Text = ""
        txt_Acq_ItemPrice.Text = ""
        txt_Acq_RecommendedBy.Text = ""
        txt_Acq_Remarks.Text = ""
        txt_Acq_SubsYear.Text = ""
        txt_Acq_RecommendedDate.Text = ""
    End Sub
    'populate acq records of a title
    Public Sub PopulateGrid()
        Dim dtSearch As DataTable = Nothing
        Try
            '**********************************************************************************
            If DDL_Titles.Text <> "" Then
                Dim SQL As String = Nothing
                SQL = "SELECT ACQ_ID, APP_NO, APP_DATE, VOL_NO, PROCESS_STATUS, COPY_PROPOSED, CUR_CODE, ITEM_PRICE, SUBS_YEAR FROM ACQUISITIONS WHERE (CAT_NO IS NOT NULL) AND (CAT_NO = '" & Trim(Label19.Text) & "') AND (LIB_CODE = '" & Trim(LibCode) & "')  AND (PROCESS_STATUS = 'Requested' OR PROCESS_STATUS = 'Rejected') ORDER BY ACQ_ID DESC"

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
                    Label24.Text = "Total Record(s): 0 "
                    App_DeleteAll_Bttn.Visible = False
                    App_Save_Bttn.Visible = True
                    App_Update_Bttn.Visible = False
                Else
                    Grid1.Visible = True
                    RecordCount = dtSearch.Rows.Count
                    Grid1.DataSource = dtSearch
                    Grid1.DataBind()
                    Label24.Text = "Total Record(s): " & RecordCount
                    App_DeleteAll_Bttn.Visible = True
                    App_Save_Bttn.Visible = True
                    App_Update_Bttn.Visible = False
                End If
                ViewState("dt") = dtSearch
            Else
                Me.Grid1.DataSource = Nothing
                Grid1.DataBind()
                App_DeleteAll_Bttn.Visible = False
                App_Update_Bttn.Visible = False
                App_Save_Bttn.Visible = True
                Label24.Text = "Total Record(s): 0 "
            End If

        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
            Label15.Text = ""
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
            Label6.Text = "Error:  there is error in page index !"
            Label15.Text = ""
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
            e.Row.Attributes("onmouseover") = "this.style.cursor='hand';this.style.textDecoration='bold';this.style.background='#FFDFDF'"
            'e.Row.Attributes("onmouseout") = "this.style.textDecoration='none';this.style.background='none'"
            'e.Row.Attributes("onclick") = ClientScript.GetPostBackClientHyperlink(Me, "Edit$" & Convert.ToString(e.Row.RowIndex))
        End If
    End Sub
    Private Shared Function t() As Object
        Throw New NotImplementedException
    End Function
    'get value of row from grid
    Private Sub Grid1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles Grid1.RowCommand
        Dim Command As SqlCommand = Nothing
        Dim dr As SqlDataReader = Nothing
        Try
            If e.CommandName = "Select" Then
                Dim myRowID, AcqID As Integer
                myRowID = e.CommandArgument.ToString()
                AcqID = Grid1.DataKeys(myRowID).Value
                If Not String.IsNullOrEmpty(AcqID) And AcqID <> 0 Then
                    Label4.Text = AcqID
                    Dim iloop As Integer
                    Dim strcurrentchar As Object
                    Dim c As Integer
                    Dim counter1 As Integer

                    AcqID = TrimX(AcqID)
                    AcqID = UCase(AcqID)

                    AcqID = RemoveQuotes(AcqID)
                    If Len(AcqID).ToString > 10 Then
                        Label6.Text = "Length of Input is not Proper!"
                        Label15.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Length of Input is not Proper... ');", True)
                        Exit Sub
                    End If
                    AcqID = " " & AcqID & " "
                    If InStr(1, AcqID, " CREATE ", 1) > 0 Or InStr(1, AcqID, " DELETE ", 1) > 0 Or InStr(1, AcqID, " DROP ", 1) > 0 Or InStr(1, AcqID, " INSERT ", 1) > 1 Or InStr(1, AcqID, " TRACK ", 1) > 1 Or InStr(1, AcqID, " TRACE ", 1) > 1 Then
                        Label6.Text = "Do not use reserve words... !"
                        Label15.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do not use reserve words... ');", True)
                        Exit Sub
                    End If
                    AcqID = TrimX(AcqID)
                    'get record details from database
                    Dim SQL As String = Nothing
                    SQL = " SELECT *  FROM ACQUISITIONS WHERE (ACQ_ID = '" & Trim(AcqID) & "') "
                    Command = New SqlCommand(SQL, SqlConn)
                    SqlConn.Open()
                    dr = Command.ExecuteReader(CommandBehavior.CloseConnection)
                    dr.Read()
                    If dr.HasRows = True Then
                        If dr.Item("APP_NO").ToString <> "" Then
                            txt_Acq_AppNo.Text = dr.Item("APP_NO").ToString
                        Else
                            txt_Acq_AppNo.Text = ""
                        End If
                        If dr.Item("SUBS_YEAR").ToString <> "" Then
                            txt_Acq_SubsYear.Text = dr.Item("SUBS_YEAR").ToString
                        Else
                            txt_Acq_SubsYear.Text = ""
                        End If
                        If dr.Item("COPY_PROPOSED").ToString <> "" Then
                            txt_Acq_Copy.Text = dr.Item("COPY_PROPOSED").ToString
                        Else
                            txt_Acq_Copy.Text = ""
                        End If
                        If dr.Item("CUR_CODE").ToString <> "" Then
                            DDL_Currencies.SelectedValue = dr.Item("CUR_CODE").ToString
                        Else
                            DDL_Currencies.Text = ""
                        End If
                        If dr.Item("ITEM_PRICE").ToString <> "" Then
                            txt_Acq_ItemPrice.Text = dr.Item("ITEM_PRICE").ToString
                        Else
                            txt_Acq_ItemPrice.Text = ""
                        End If
                        If dr.Item("ACQMODE_CODE").ToString <> "" Then
                            DDL_AcqModes.SelectedValue = dr.Item("ACQMODE_CODE").ToString
                        Else
                            DDL_AcqModes.Text = ""
                        End If
                        If dr.Item("RECOMMENDED_BY").ToString <> "" Then
                            txt_Acq_RecommendedBy.Text = dr.Item("RECOMMENDED_BY").ToString
                        Else
                            txt_Acq_RecommendedBy.Text = ""
                        End If
                        If dr.Item("RECOMMENDED_DATE").ToString <> "" Then
                            txt_Acq_RecommendedDate.Text = Format(dr.Item("RECOMMENDED_DATE"), "dd/MM/yyyy").ToString
                        Else
                            txt_Acq_RecommendedDate.Text = ""
                        End If
                        If dr.Item("NOTE").ToString <> "" Then
                            txt_Acq_Remarks.Text = dr.Item("NOTE").ToString
                        Else
                            txt_Acq_Remarks.Text = ""
                        End If
                        App_Update_Bttn.Visible = True
                        App_Update_Bttn.Enabled = True
                        App_Save_Bttn.Visible = False
                        Label6.Text = ""
                        Label15.Text = "Press UPDATE Button to save the Changes if any.."
                        dr.Close()
                    Else
                        App_Update_Bttn.Visible = False
                        App_Update_Bttn.Enabled = False
                        App_Save_Bttn.Visible = True
                        Label4.Text = ""
                        Label6.Text = "Record Not Selected to Edit"
                        Label15.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' No Record to Edit... ');", True)
                    End If
                Else
                    App_Update_Bttn.Visible = False
                    App_Update_Bttn.Enabled = False
                    App_Save_Bttn.Visible = True
                    Label4.Text = ""
                    Label6.Text = "Record Not Selected to Edit"
                    Label15.Text = ""
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' No Record to Edit... ');", True)
                End If
            End If
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
            Label15.Text = ""
            Label4.Text = ""
            App_Update_Bttn.Visible = False
            App_Update_Bttn.Enabled = False
            App_Save_Bttn.Visible = True
        Finally
            SqlConn.Close()
            txt_Acq_AppNo.Focus()
        End Try
    End Sub 'Grid1_ItemCommand

    Protected Sub App_Cancel_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles App_Cancel_Bttn.Click
        Me.txt_Acq_AppNo.Text = ""
        Label4.Text = ""
        App_Save_Bttn.Visible = True
        App_Update_Bttn.Visible = False
        Label15.Text = "Enter Data and Press SAVE Button to save the record.."
        Label6.Text = ""
        ClearFields()
    End Sub
    'update record
    Protected Sub App_Update_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles App_Update_Bttn.Click
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
                Dim counter1, counter2, counter3, counter4, counter5, counter6, counter7, counter8, counter9 As Integer
                '****************************************************************************************************
                'validation for App No
                Dim APP_NO As Object = Nothing
                If Me.txt_Acq_AppNo.Text <> "" Then
                    APP_NO = TrimX(txt_Acq_AppNo.Text)
                    APP_NO = RemoveQuotes(APP_NO)
                    If APP_NO.Length > 50 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Acq_AppNo.Focus()
                        Exit Sub
                    End If
                    APP_NO = " " & APP_NO & " "
                    If InStr(1, APP_NO, "CREATE", 1) > 0 Or InStr(1, APP_NO, "DELETE", 1) > 0 Or InStr(1, APP_NO, "DROP", 1) > 0 Or InStr(1, APP_NO, "INSERT", 1) > 1 Or InStr(1, APP_NO, "TRACK", 1) > 1 Or InStr(1, APP_NO, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Acq_AppNo.Focus()
                        Exit Sub
                    End If
                    APP_NO = TrimX(UCase(APP_NO))
                    'check unwanted characters
                    c = 0
                    counter1 = 0
                    For iloop = 1 To Len(APP_NO)
                        strcurrentchar = Mid(APP_NO, iloop, 1)
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
                        txt_Acq_AppNo.Focus()
                        Exit Sub
                    End If

                    'check this app no status
                    Dim str As Object = Nothing
                    Dim flag As Object = Nothing
                    str = "SELECT APP_NO FROM ACQUISITIONS WHERE (APP_NO ='" & Trim(APP_NO) & "') AND (PROCESS_STATUS<>'Requested') AND (LIB_CODE ='" & Trim(LibCode) & "')"
                    Dim cmd1 As New SqlCommand(str, SqlConn)
                    SqlConn.Open()
                    flag = cmd1.ExecuteScalar
                    If flag <> Nothing Then
                        Label6.Text = "This Approval No has already been processed, Use another one !"
                        Label15.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('This Approval No has already been processed, Use another one !');", True)
                        txt_Acq_AppNo.Focus()
                        Exit Sub
                    End If
                    SqlConn.Close()
                Else
                    Label6.Text = "Plz Enter the Approval Number !"
                    Label15.Text = ""
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Plz Enter the Approval Number !');", True)
                    txt_Acq_AppNo.Focus()
                    Exit Sub
                End If

                'validation for Subscription eyar No
                Dim SUBS_YEAR As Integer = Nothing
                If Me.txt_Acq_SubsYear.Text <> "" Then
                    SUBS_YEAR = TrimX(txt_Acq_SubsYear.Text)
                    SUBS_YEAR = RemoveQuotes(SUBS_YEAR)
                    If SUBS_YEAR.ToString.Length > 5 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Acq_SubsYear.Focus()
                        Exit Sub
                    End If
                    SUBS_YEAR = " " & SUBS_YEAR & " "
                    If InStr(1, SUBS_YEAR, "CREATE", 1) > 0 Or InStr(1, SUBS_YEAR, "DELETE", 1) > 0 Or InStr(1, SUBS_YEAR, "DROP", 1) > 0 Or InStr(1, SUBS_YEAR, "INSERT", 1) > 1 Or InStr(1, SUBS_YEAR, "TRACK", 1) > 1 Or InStr(1, SUBS_YEAR, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Acq_SubsYear.Focus()
                        Exit Sub
                    End If
                    SUBS_YEAR = TrimX(SUBS_YEAR)
                    'check unwanted characters
                    c = 0
                    counter2 = 0
                    For iloop = 1 To Len(SUBS_YEAR)
                        strcurrentchar = Mid(SUBS_YEAR, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter2 = 1
                            End If
                        End If
                    Next
                    If counter2 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Acq_SubsYear.Focus()
                        Exit Sub
                    End If
                Else
                    SUBS_YEAR = Nothing
                End If

                'check this app no for same year
                Dim str2 As Object = Nothing
                Dim flag2 As Object = Nothing
                str2 = "SELECT APP_NO FROM ACQUISITIONS WHERE (ACQ_ID<>'" & Trim(Label4.Text) & "') AND (CAT_NO ='" & Trim(Label19.Text) & "') AND (SUBS_YEAR='" & Trim(SUBS_YEAR) & "') AND (LIB_CODE ='" & Trim(LibCode) & "')"
                Dim cmd2 As New SqlCommand(str2, SqlConn)
                SqlConn.Open()
                flag2 = cmd2.ExecuteScalar
                SqlConn.Close()
                If flag2 <> Nothing Then
                    Label6.Text = "Acqusition Record for this Subscription Year Already Exists!"
                    Label15.Text = ""
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Acqusition Record for this Subscription Year Already Exists!');", True)
                    txt_Acq_AppNo.Focus()
                    Exit Sub
                End If

                'validation for copy
                Dim COPY_PROPOSED As Integer = Nothing
                If Me.txt_Acq_Copy.Text <> "" Then
                    COPY_PROPOSED = Convert.ToInt16(TrimX(txt_Acq_Copy.Text))
                    COPY_PROPOSED = RemoveQuotes(COPY_PROPOSED)
                    If Not IsNumeric(COPY_PROPOSED) = True Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Acq_Copy.Focus()
                        Exit Sub
                    End If
                    If Len(COPY_PROPOSED) > 5 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Acq_Copy.Focus()
                        Exit Sub
                    End If
                    COPY_PROPOSED = " " & COPY_PROPOSED & " "
                    If InStr(1, COPY_PROPOSED, "CREATE", 1) > 0 Or InStr(1, COPY_PROPOSED, "DELETE", 1) > 0 Or InStr(1, COPY_PROPOSED, "DROP", 1) > 0 Or InStr(1, COPY_PROPOSED, "INSERT", 1) > 1 Or InStr(1, COPY_PROPOSED, "TRACK", 1) > 1 Or InStr(1, COPY_PROPOSED, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Acq_Copy.Focus()
                        Exit Sub
                    End If
                    COPY_PROPOSED = TrimAll(COPY_PROPOSED)
                    'check unwanted characters
                    c = 0
                    counter3 = 0
                    For iloop = 1 To Len(COPY_PROPOSED.ToString)
                        strcurrentchar = Mid(COPY_PROPOSED, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter3 = 1
                            End If
                        End If
                    Next
                    If counter3 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Acq_Copy.Focus()
                        Exit Sub
                    End If
                Else
                    COPY_PROPOSED = 1
                End If

                'validation for Currency code
                Dim CUR_CODE As Object = Nothing
                CUR_CODE = DDL_Currencies.SelectedValue
                If Not String.IsNullOrEmpty(CUR_CODE) Then
                    CUR_CODE = RemoveQuotes(CUR_CODE)
                    If CUR_CODE.Length > 4 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_Currencies.Focus()
                        Exit Sub
                    End If
                    CUR_CODE = " " & CUR_CODE & " "
                    If InStr(1, CUR_CODE, "CREATE", 1) > 0 Or InStr(1, CUR_CODE, "DELETE", 1) > 0 Or InStr(1, CUR_CODE, "DROP", 1) > 0 Or InStr(1, CUR_CODE, "INSERT", 1) > 1 Or InStr(1, CUR_CODE, "TRACK", 1) > 1 Or InStr(1, CUR_CODE, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_Currencies.Focus()
                        Exit Sub
                    End If
                    CUR_CODE = TrimX(CUR_CODE)
                    'check unwanted characters
                    c = 0
                    counter4 = 0
                    For iloop = 1 To Len(CUR_CODE)
                        strcurrentchar = Mid(CUR_CODE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter4 = 1
                            End If
                        End If
                    Next
                    If counter4 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_Currencies.Focus()
                        Exit Sub
                    End If
                Else
                    CUR_CODE = "INR"
                End If

                'validation for Currency code
                Dim ITEM_PRICE As Object = Nothing
                ITEM_PRICE = TrimX(txt_Acq_ItemPrice.Text)
                If Not String.IsNullOrEmpty(ITEM_PRICE) Then
                    ITEM_PRICE = RemoveQuotes(ITEM_PRICE)
                    If ITEM_PRICE.Length > 10 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Acq_ItemPrice.Focus()
                        Exit Sub
                    End If
                    ITEM_PRICE = " " & ITEM_PRICE & " "
                    If InStr(1, ITEM_PRICE, "CREATE", 1) > 0 Or InStr(1, ITEM_PRICE, "DELETE", 1) > 0 Or InStr(1, ITEM_PRICE, "DROP", 1) > 0 Or InStr(1, ITEM_PRICE, "INSERT", 1) > 1 Or InStr(1, ITEM_PRICE, "TRACK", 1) > 1 Or InStr(1, ITEM_PRICE, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Acq_ItemPrice.Focus()
                        Exit Sub
                    End If
                    ITEM_PRICE = TrimX(ITEM_PRICE)
                    'check unwanted characters
                    c = 0
                    counter5 = 0
                    For iloop = 1 To Len(ITEM_PRICE)
                        strcurrentchar = Mid(ITEM_PRICE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter5 = 1
                            End If
                        End If
                    Next
                    If counter5 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Acq_ItemPrice.Focus()
                        Exit Sub
                    End If
                Else
                    Label6.Text = "Plz Enter the Item Price !"
                    Label15.Text = ""
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Plz Enter the Item Price !');", True)
                    txt_Acq_ItemPrice.Focus()
                    Exit Sub
                End If

                'validation for Currency code
                Dim ACQMODE_CODE As Object = Nothing
                ACQMODE_CODE = DDL_AcqModes.SelectedValue
                If Not String.IsNullOrEmpty(ACQMODE_CODE) Then
                    ACQMODE_CODE = RemoveQuotes(ACQMODE_CODE)
                    If ACQMODE_CODE.Length > 2 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_AcqModes.Focus()
                        Exit Sub
                    End If
                    ACQMODE_CODE = " " & ACQMODE_CODE & " "
                    If InStr(1, ACQMODE_CODE, "CREATE", 1) > 0 Or InStr(1, ACQMODE_CODE, "DELETE", 1) > 0 Or InStr(1, ACQMODE_CODE, "DROP", 1) > 0 Or InStr(1, ACQMODE_CODE, "INSERT", 1) > 1 Or InStr(1, ACQMODE_CODE, "TRACK", 1) > 1 Or InStr(1, ACQMODE_CODE, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_AcqModes.Focus()
                        Exit Sub
                    End If
                    ACQMODE_CODE = TrimX(ACQMODE_CODE)
                    'check unwanted characters
                    c = 0
                    counter6 = 0
                    For iloop = 1 To Len(ACQMODE_CODE)
                        strcurrentchar = Mid(ACQMODE_CODE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter6 = 1
                            End If
                        End If
                    Next
                    If counter6 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_AcqModes.Focus()
                        Exit Sub
                    End If
                Else
                    ACQMODE_CODE = "P"
                End If

                'validation for reccommednd by
                Dim RECOMMENDED_BY As Object = Nothing
                If Me.txt_Acq_RecommendedBy.Text <> "" Then
                    RECOMMENDED_BY = TrimAll(txt_Acq_RecommendedBy.Text)
                    RECOMMENDED_BY = RemoveQuotes(RECOMMENDED_BY)
                    If RECOMMENDED_BY.Length > 100 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Acq_RecommendedBy.Focus()
                        Exit Sub
                    End If
                    RECOMMENDED_BY = " " & RECOMMENDED_BY & " "
                    If InStr(1, RECOMMENDED_BY, "CREATE", 1) > 0 Or InStr(1, RECOMMENDED_BY, "DELETE", 1) > 0 Or InStr(1, RECOMMENDED_BY, "DROP", 1) > 0 Or InStr(1, RECOMMENDED_BY, "INSERT", 1) > 1 Or InStr(1, RECOMMENDED_BY, "TRACK", 1) > 1 Or InStr(1, RECOMMENDED_BY, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Acq_RecommendedBy.Focus()
                        Exit Sub
                    End If
                    RECOMMENDED_BY = TrimAll(RECOMMENDED_BY)
                    'check unwanted characters
                    c = 0
                    counter7 = 0
                    For iloop = 1 To Len(RECOMMENDED_BY)
                        strcurrentchar = Mid(RECOMMENDED_BY, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter7 = 1
                            End If
                        End If
                    Next
                    If counter7 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Acq_RecommendedBy.Focus()
                        Exit Sub
                    End If
                Else
                    RECOMMENDED_BY = ""
                End If

                'search start date
                Dim RECOMMENDED_DATE As Object = Nothing
                If txt_Acq_RecommendedDate.Text <> "" Then
                    RECOMMENDED_DATE = TrimX(txt_Acq_RecommendedDate.Text)
                    RECOMMENDED_DATE = RemoveQuotes(RECOMMENDED_DATE)
                    RECOMMENDED_DATE = Convert.ToDateTime(RECOMMENDED_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                    If Len(RECOMMENDED_DATE) > 12 Then
                        Label6.Text = " Input is not Valid..."
                        Me.txt_Acq_RecommendedDate.Focus()
                        Exit Sub
                    End If
                    RECOMMENDED_DATE = " " & RECOMMENDED_DATE & " "
                    If InStr(1, RECOMMENDED_DATE, "CREATE", 1) > 0 Or InStr(1, RECOMMENDED_DATE, "DELETE", 1) > 0 Or InStr(1, RECOMMENDED_DATE, "DROP", 1) > 0 Or InStr(1, RECOMMENDED_DATE, "INSERT", 1) > 1 Or InStr(1, RECOMMENDED_DATE, "TRACK", 1) > 1 Or InStr(1, RECOMMENDED_DATE, "TRACE", 1) > 1 Then
                        Label6.Text = "  Input is not Valid... "
                        Me.txt_Acq_RecommendedDate.Focus()
                        Exit Sub
                    End If
                    RECOMMENDED_DATE = TrimX(RECOMMENDED_DATE)
                    'check unwanted characters
                    c = 0
                    counter8 = 0
                    For iloop = 1 To Len(RECOMMENDED_DATE)
                        strcurrentchar = Mid(RECOMMENDED_DATE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter8 = 1
                            End If
                        End If
                    Next
                    If counter8 = 1 Then
                        Label6.Text = "data is not Valid... "
                        Me.txt_Acq_RecommendedDate.Focus()
                        Exit Sub
                    End If
                Else
                    RECOMMENDED_DATE = ""
                End If

                If RECOMMENDED_BY <> "" And RECOMMENDED_DATE = "" Then
                    RECOMMENDED_DATE = Now.Date
                End If

                'Server validation for  : txt_Cat_Remarks
                Dim NOTE As Object = Nothing
                If txt_Acq_Remarks.Text <> "" Then
                    NOTE = TrimAll(txt_Acq_Remarks.Text)
                    NOTE = RemoveQuotes(NOTE)
                    If NOTE.Length > 500 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Acq_Remarks.Focus()
                        Exit Sub
                    End If

                    NOTE = " " & NOTE & " "
                    If InStr(1, NOTE, "CREATE", 1) > 0 Or InStr(1, NOTE, "DELETE", 1) > 0 Or InStr(1, NOTE, "DROP", 1) > 0 Or InStr(1, NOTE, "INSERT", 1) > 1 Or InStr(1, NOTE, "TRACK", 1) > 1 Or InStr(1, NOTE, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Acq_Remarks.Focus()
                        Exit Sub
                    End If
                    NOTE = TrimAll(NOTE)
                    'check unwanted characters
                    c = 0
                    counter9 = 0
                    For iloop = 1 To Len(NOTE.ToString)
                        strcurrentchar = Mid(NOTE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter9 = 1
                            End If
                        End If
                    Next
                    If counter9 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Acq_Remarks.Focus()
                        Exit Sub
                    End If
                Else
                    NOTE = ""
                End If

                Dim LIB_CODE As Object = Nothing
                If Session.Item("LoggedLibcode") <> "" Then
                    LIB_CODE = Session.Item("LoggedLibcode")
                Else
                    Label6.Text = "No Library Code Exists..Login Again  "
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

                'UPDATE THE LIBRARY PROFILE  
                If Label4.Text <> "" Then
                    SQL = "SELECT * FROM ACQUISITIONS WHERE (ACQ_ID='" & Trim(Label4.Text) & "')"
                    SqlConn.Open()
                    da = New SqlDataAdapter(SQL, SqlConn)
                    cb = New SqlCommandBuilder(da)
                    da.Fill(ds, "ACQ")
                    If ds.Tables("ACQ").Rows.Count <> 0 Then
                        If Not String.IsNullOrEmpty(APP_NO) Then
                            ds.Tables("ACQ").Rows(0)("APP_NO") = APP_NO.Trim
                        Else
                            ds.Tables("ACQ").Rows(0)("APP_NO") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(SUBS_YEAR) Then
                            ds.Tables("ACQ").Rows(0)("SUBS_YEAR") = SUBS_YEAR
                        Else
                            ds.Tables("ACQ").Rows(0)("SUBS_YEAR") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(COPY_PROPOSED) Then
                            ds.Tables("ACQ").Rows(0)("COPY_PROPOSED") = COPY_PROPOSED
                        Else
                            ds.Tables("ACQ").Rows(0)("COPY_PROPOSED") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(CUR_CODE) Then
                            ds.Tables("ACQ").Rows(0)("CUR_CODE") = CUR_CODE.Trim
                        Else
                            ds.Tables("ACQ").Rows(0)("CUR_CODE") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(ITEM_PRICE) Then
                            ds.Tables("ACQ").Rows(0)("ITEM_PRICE") = ITEM_PRICE.Trim
                        Else
                            ds.Tables("ACQ").Rows(0)("ITEM_PRICE") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(ACQMODE_CODE) Then
                            ds.Tables("ACQ").Rows(0)("ACQMODE_CODE") = ACQMODE_CODE.Trim
                        Else
                            ds.Tables("ACQ").Rows(0)("ACQMODE_CODE") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(RECOMMENDED_BY) Then
                            ds.Tables("ACQ").Rows(0)("RECOMMENDED_BY") = RECOMMENDED_BY.Trim
                        Else
                            ds.Tables("ACQ").Rows(0)("RECOMMENDED_BY") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(RECOMMENDED_DATE) Then
                            ds.Tables("ACQ").Rows(0)("RECOMMENDED_DATE") = RECOMMENDED_DATE.Trim
                        Else
                            ds.Tables("ACQ").Rows(0)("RECOMMENDED_DATE") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(NOTE) Then
                            ds.Tables("ACQ").Rows(0)("NOTE") = NOTE.ToString.Trim
                        Else
                            ds.Tables("ACQ").Rows(0)("NOTE") = System.DBNull.Value
                        End If

                        ds.Tables("ACQ").Rows(0)("UPDATED_BY") = USER_CODE
                        ds.Tables("ACQ").Rows(0)("DATE_MODIFIED") = DATE_MODIFIED
                        ds.Tables("ACQ").Rows(0)("IP") = IP

                        thisTransaction = SqlConn.BeginTransaction()
                        da.SelectCommand.Transaction = thisTransaction
                        da.Update(ds, "ACQ")
                        thisTransaction.Commit()
                        Label6.Text = ""
                        Label15.Text = "Record Updated Successfully"
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' User Profile Updated Successfully... ');", True)
                        ClearFields()
                        PopulateGrid()
                    Else
                        Label6.Text = "Record Updation   - Please Contact System Administrator"
                        Label15.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Record Updation   - Please Contact System Administrator... ');", True)
                    End If
                End If
            Else
                'record not selected
                Label6.Text = "Record Not Selected..."
                Label15.Text = ""
                ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Record Not Selected... ');", True)
            End If
            SqlConn.Close()
            'Search_Bttn_Click(sender, e)
            ClearFields()
        Catch q As SqlException
            thisTransaction.Rollback()
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
            Label15.Text = ""
        Finally
            SqlConn.Close()
            Label4.Text = ""
            ClearFields()
            Me.App_Save_Bttn.Visible = True
            Me.App_Update_Bttn.Visible = False
        End Try
    End Sub
    'delete selected rows
    Protected Sub App_DeleteAll_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles App_DeleteAll_Bttn.Click
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing
        Try
            For Each row As GridViewRow In Grid1.Rows
                Dim cb As CheckBox = DirectCast(row.FindControl("cbd"), CheckBox)
                If cb IsNot Nothing AndAlso cb.Checked = True Then
                    Dim ID As Integer = Convert.ToInt32(Grid1.DataKeys(row.RowIndex).Value)

                    If SqlConn.State = 0 Then
                        SqlConn.Open()
                    End If

                    thisTransaction = SqlConn.BeginTransaction()
                    Dim objCommand As New SqlCommand
                    objCommand.Connection = SqlConn
                    objCommand.Transaction = thisTransaction
                    objCommand.CommandType = CommandType.Text
                    objCommand.CommandText = "DELETE FROM ACQUISITIONS WHERE (ACQ_ID =@ACQ_ID) " '"DELETE FROM  HOLDINGS WHERE HOLD_ID = @HOLD_ID "

                    objCommand.Parameters.Add("@ACQ_ID", SqlDbType.Int)
                    objCommand.Parameters("@ACQ_ID").Value = ID

                    objCommand.ExecuteNonQuery()

                    'count acq record of this cat no
                    Dim objCommand3 As New SqlCommand
                    objCommand3.Connection = SqlConn
                    objCommand3.Transaction = thisTransaction
                    objCommand3.CommandType = CommandType.Text
                    objCommand3.CommandText = "SELECT COUNT(*) FROM ACQUISITIONS WHERE CAT_NO = @CAT_NO"

                    objCommand3.Parameters.Add("@CAT_NO", SqlDbType.Int)
                    objCommand3.Parameters("@CAT_NO").Value = Label19.Text

                    Dim intValue As Long = 0
                    Dim dr As SqlDataReader
                    dr = objCommand3.ExecuteReader()
                    If dr.Read Then
                        intValue = dr.GetValue(0)
                    End If
                    dr.Close()

                    ''update cat table for cat_level
                    If intValue = 0 Then
                        Dim objCommand4 As New SqlCommand
                        objCommand4.Connection = SqlConn
                        objCommand4.Transaction = thisTransaction
                        objCommand4.CommandType = CommandType.Text
                        objCommand4.CommandText = "UPDATE CATS SET CAT_LEVEL = 'New'  WHERE (CAT_NO = @CAT_NO)"

                        objCommand4.Parameters.Add("@CAT_NO", SqlDbType.Int)
                        objCommand4.Parameters("@CAT_NO").Value = Label19.Text

                        Dim dr4 As SqlDataReader
                        dr4 = objCommand4.ExecuteReader()
                        dr4.Close()
                    End If
                    thisTransaction.Commit()
                    SqlConn.Close()
                End If
            Next
            Dim s As Object = Nothing
            Dim e1 As EventArgs = Nothing
            PopulateGrid()
        Catch s As Exception
            thisTransaction.Rollback()
            Label15.Text = ""
            Label6.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    '****************************************************************************************************************\
    '*******************************************************************************************************************
    'GENERATE APPROVAL STARTS FROM HERE
    'populate approval no
    Public Sub PopulateAppNo()
        Me.DDL_Approvals.DataTextField = "APP_NO"
        Me.DDL_Approvals.DataValueField = "APP_NO"
        Me.DDL_Approvals.DataSource = GetApprovalList(LibCode)
        Me.DDL_Approvals.DataBind()
        DDL_Approvals.Items.Insert(0, "")
    End Sub
    'populate approval no
    Public Sub PopulateComiitees()
        Me.DDL_Committees.DataTextField = "COM_NAME"
        Me.DDL_Committees.DataValueField = "COM_CODE"
        Me.DDL_Committees.DataSource = GetCommitteeList(LibCode)
        Me.DDL_Committees.DataBind()
        DDL_Committees.Items.Insert(0, "")
    End Sub
    'display app date/comittee and fill title details
    Protected Sub DDL_Approvals_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDL_Approvals.SelectedIndexChanged
        Dim dt As DataTable
        Try
            Dim APP_NO As Object = Nothing
            If DDL_Approvals.Text <> "" Then
                APP_NO = DDL_Approvals.SelectedValue
                Dim SQL As String = Nothing
                SQL = "SELECT APP_DATE, COM_CODE  FROM ACQUISITIONS WHERE (APP_NO = '" & Trim(APP_NO) & "') AND (LIB_CODE ='" & Trim(LibCode) & "')"
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
                        txt_Acq_AppDate.Text = Format(dt.Rows(0).Item("APP_DATE"), "dd/MM/yyyy").ToString
                    Else
                        txt_Acq_AppDate.Text = ""
                    End If

                    If dt.Rows(0).Item("COM_CODE").ToString <> "" Then
                        DDL_Committees.SelectedValue = dt.Rows(0).Item("COM_CODE").ToString
                    Else
                        DDL_Committees.Text = ""
                    End If
                Else
                    txt_Acq_AppDate.Text = ""
                    DDL_Committees.Text = ""
                End If
            End If
            PopulateAppGrid(APP_NO)
        Catch s As Exception
            Label7.Text = ""
            Label20.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'populate titles in the Grid
    Public Sub PopulateAppGrid(ByVal APP_NO As Object)
        Dim dtSearch As DataTable = Nothing
        Try
            If APP_NO <> "" Then
                '**********************************************************************************
                Dim SQL As String = Nothing
                SQL = "SELECT ACQ_ID, APP_NO, APP_DATE, SUBS_YEAR, PROCESS_STATUS, COPY_PROPOSED, CUR_CODE, ITEM_PRICE, CAT_NO, COM_CODE, TITLE , SUB_TITLE, AUTHOR1, AUTHOR2, AUTHOR3, EDITION, PLACE_OF_PUB, YEAR_OF_PUB,PUB_PLACE,  STANDARD_NO, PUB_NAME, CORPORATE_AUTHOR, EDITOR FROM APPROVAL_VIEW2 WHERE (APP_NO = '" & APP_NO & "')  AND (PROCESS_STATUS = 'Requested' or PROCESS_STATUS = 'Sent For Approval') AND (LIB_CODE ='" & (LibCode) & "') AND  (BIB_CODE = 'S') ORDER BY ACQ_ID DESC"

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
                    Me.App_Print_Bttn.Visible = False
                Else
                    Grid2.Visible = True
                    RecordCount = dtSearch.Rows.Count
                    Grid2.DataSource = dtSearch
                    Grid2.DataBind()
                    Label9.Text = "Total Record(s): " & RecordCount
                    Me.App_Print_Bttn.Visible = True
                End If
                ViewState("dt") = dtSearch
            Else
                Me.Grid2.DataSource = Nothing
                Grid2.DataBind()
                Label9.Text = "Total Record(s): 0 "
                Me.App_Print_Bttn.Visible = False
            End If
        Catch s As Exception
            Label20.Text = "Error: " & (s.Message())
            Label7.Text = ""
        Finally
            SqlConn.Close()
            DDL_Approvals.Focus()
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
    'process the approval
    Protected Sub App_Process_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles App_Process_Bttn.Click
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
                Dim counter1, counter2, counter3 As Integer
                '****************************************************************************************************
                'validation for App No
                Dim APP_NO As Object = Nothing
                If Me.DDL_Approvals.Text <> "" Then
                    APP_NO = Trim(DDL_Approvals.Text)
                    APP_NO = RemoveQuotes(APP_NO)
                    If APP_NO.Length > 50 Then
                        Label20.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        DDL_Approvals.Focus()
                        Exit Sub
                    End If
                    APP_NO = " " & APP_NO & " "
                    If InStr(1, APP_NO, "CREATE", 1) > 0 Or InStr(1, APP_NO, "DELETE", 1) > 0 Or InStr(1, APP_NO, "DROP", 1) > 0 Or InStr(1, APP_NO, "INSERT", 1) > 1 Or InStr(1, APP_NO, "TRACK", 1) > 1 Or InStr(1, APP_NO, "TRACE", 1) > 1 Then
                        Label20.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        DDL_Approvals.Focus()
                        Exit Sub
                    End If
                    APP_NO = TrimX(UCase(APP_NO))
                    'check unwanted characters
                    c = 0
                    counter1 = 0
                    For iloop = 1 To Len(APP_NO)
                        strcurrentchar = Mid(APP_NO, iloop, 1)
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
                        DDL_Approvals.Focus()
                        Exit Sub
                    End If
                Else
                    Label20.Text = "Plz select the Approval Number from Drop-Down !"
                    Label7.Text = ""
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Plz select the Approval Number from Drop-Down !');", True)
                    DDL_Approvals.Focus()
                    Exit Sub
                End If

                'com code3
                Dim COM_CODE As Object = Nothing
                If Me.DDL_Committees.Text <> "" Then
                    COM_CODE = Trim(DDL_Committees.Text)
                    COM_CODE = RemoveQuotes(COM_CODE)
                    If COM_CODE.Length > 15 Then
                        Label20.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        DDL_Committees.Focus()
                        Exit Sub
                    End If
                    COM_CODE = " " & COM_CODE & " "
                    If InStr(1, COM_CODE, "CREATE", 1) > 0 Or InStr(1, COM_CODE, "DELETE", 1) > 0 Or InStr(1, COM_CODE, "DROP", 1) > 0 Or InStr(1, COM_CODE, "INSERT", 1) > 1 Or InStr(1, COM_CODE, "TRACK", 1) > 1 Or InStr(1, COM_CODE, "TRACE", 1) > 1 Then
                        Label20.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        DDL_Committees.Focus()
                        Exit Sub
                    End If
                    COM_CODE = TrimX(UCase(COM_CODE))
                    'check unwanted characters
                    c = 0
                    counter2 = 0
                    For iloop = 1 To Len(COM_CODE)
                        strcurrentchar = Mid(COM_CODE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter2 = 1
                            End If
                        End If
                    Next
                    If counter2 = 1 Then
                        Label20.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        DDL_Committees.Focus()
                        Exit Sub
                    End If
                Else
                    Label20.Text = "Plz select the Committee from Drop-Down !"
                    Label7.Text = ""
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Plz select the Committee from Drop-Down !');", True)
                    DDL_Committees.Focus()
                    Exit Sub
                End If

                'search start date
                Dim APP_DATE As Object = Nothing
                If txt_Acq_AppDate.Text <> "" Then
                    APP_DATE = TrimX(txt_Acq_AppDate.Text)
                    APP_DATE = RemoveQuotes(APP_DATE)
                    APP_DATE = Convert.ToDateTime(APP_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                    If Len(APP_DATE) > 12 Then
                        Label20.Text = " Input is not Valid..."
                        Label7.Text = ""
                        Me.txt_Acq_AppDate.Focus()
                        Exit Sub
                    End If
                    APP_DATE = " " & APP_DATE & " "
                    If InStr(1, APP_DATE, "CREATE", 1) > 0 Or InStr(1, APP_DATE, "DELETE", 1) > 0 Or InStr(1, APP_DATE, "DROP", 1) > 0 Or InStr(1, APP_DATE, "INSERT", 1) > 1 Or InStr(1, APP_DATE, "TRACK", 1) > 1 Or InStr(1, APP_DATE, "TRACE", 1) > 1 Then
                        Label20.Text = "  Input is not Valid... "
                        Label7.Text = ""
                        Me.txt_Acq_AppDate.Focus()
                        Exit Sub
                    End If
                    APP_DATE = TrimX(APP_DATE)
                    'check unwanted characters
                    c = 0
                    counter3 = 0
                    For iloop = 1 To Len(APP_DATE)
                        strcurrentchar = Mid(APP_DATE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter3 = 1
                            End If
                        End If
                    Next
                    If counter3 = 1 Then
                        Label20.Text = "data is not Valid... "
                        Label7.Text = ""
                        Me.txt_Acq_AppDate.Focus()
                        Exit Sub
                    End If
                Else
                    APP_DATE = Now.Date
                    APP_DATE = Convert.ToDateTime(APP_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us
                End If

                Dim USER_CODE As Object = Nothing
                USER_CODE = Session.Item("LoggedUser")

                Dim LIB_CODE As Object = Nothing
                LIB_CODE = LibCode

                Dim PROCESS_STATUS As Object = Nothing
                PROCESS_STATUS = "Sent For Approval"

                Dim DATE_MODIFIED As Object = Nothing
                DATE_MODIFIED = Now.Date
                DATE_MODIFIED = Convert.ToDateTime(DATE_MODIFIED, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                Dim IP As Object = Nothing
                IP = Request.UserHostAddress.Trim

                If SqlConn.State = 0 Then
                    SqlConn.Open()
                End If

                Dim objCommand As New SqlCommand
                objCommand.Connection = SqlConn
                thisTransaction = SqlConn.BeginTransaction()
                objCommand.Transaction = thisTransaction
                objCommand.CommandType = CommandType.Text
                objCommand.CommandText = "UPDATE ACQUISITIONS SET APP_DATE=@APP_DATE, PROCESS_STATUS=@PROCESS_STATUS, COM_CODE=@COM_CODE, USER_CODE=@USER_CODE, DATE_MODIFIED=@DATE_MODIFIED, UPDATED_BY=@USER_CODE, IP=@IP FROM ACQUISITIONS WHERE APP_NO = @APP_NO  AND PROCESS_STATUS = 'Requested'  AND LIB_CODE =@LIB_CODE "

                If APP_NO = "" Then APP_NO = System.DBNull.Value
                objCommand.Parameters.Add("@APP_NO", SqlDbType.NVarChar)
                objCommand.Parameters("@APP_NO").Value = APP_NO

                If APP_DATE = "" Then APP_DATE = System.DBNull.Value
                objCommand.Parameters.Add("@APP_DATE", SqlDbType.DateTime)
                objCommand.Parameters("@APP_DATE").Value = APP_DATE

                If PROCESS_STATUS = "" Then PROCESS_STATUS = System.DBNull.Value
                objCommand.Parameters.Add("@PROCESS_STATUS", SqlDbType.VarChar)
                objCommand.Parameters("@PROCESS_STATUS").Value = PROCESS_STATUS

                If COM_CODE = "" Then COM_CODE = System.DBNull.Value
                objCommand.Parameters.Add("@COM_CODE", SqlDbType.VarChar)
                objCommand.Parameters("@COM_CODE").Value = COM_CODE

                If USER_CODE = "" Then USER_CODE = System.DBNull.Value
                objCommand.Parameters.Add("@USER_CODE", SqlDbType.NVarChar)
                objCommand.Parameters("@USER_CODE").Value = USER_CODE

                If DATE_MODIFIED = "" Then DATE_MODIFIED = System.DBNull.Value
                objCommand.Parameters.Add("@DATE_MODIFIED", SqlDbType.DateTime)
                objCommand.Parameters("@DATE_MODIFIED").Value = DATE_MODIFIED

                If USER_CODE = "" Then USER_CODE = System.DBNull.Value
                objCommand.Parameters.Add("@UPDATED_BY", SqlDbType.NVarChar)
                objCommand.Parameters("@UPDATED_BY").Value = USER_CODE

                If IP = "" Then IP = System.DBNull.Value
                objCommand.Parameters.Add("@IP", SqlDbType.VarChar)
                objCommand.Parameters("@IP").Value = IP

                If LIB_CODE = "" Then LIB_CODE = System.DBNull.Value
                objCommand.Parameters.Add("@LIB_CODE", SqlDbType.NVarChar)
                objCommand.Parameters("@LIB_CODE").Value = LIB_CODE

                objCommand.ExecuteNonQuery()
                thisTransaction.Commit()

                Label20.Text = ""
                Label7.Text = "Record(s) Updated Successfully"
                ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Record(s) Updated Successfully... ');", True)
            Else
                Label20.Text = "Record Updation   - Please Contact System Administrator"
                Label17.Text = ""
                ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Record Updation   - Please Contact System Administrator... ');", True)
            End If
            'End If
            SqlConn.Close()
            PopulateAppGrid(DDL_Approvals.SelectedValue)
        Catch q As SqlException
            thisTransaction.Rollback()
        Catch s As Exception
            Label20.Text = "Error: " & (s.Message())
            Label7.Text = ""
        Finally
            SqlConn.Close()

        End Try
    End Sub

    '************************************************************************************************************8
    '***************************************************************************************************************
    'UPDATE APPROVAL
    'publish approval for UPDATE APPROVAL TAB
    Public Sub GetApprovalList3()
        Dim Sel As String = "SELECT DISTINCT APP_NO FROM APPROVAL_VIEW2 WHERE (PROCESS_STATUS = 'Approved' or PROCESS_STATUS = 'Sent For Approval' OR PROCESS_STATUS = 'Rejected') AND (LIB_CODE ='" & (LibCode) & "') AND  (BIB_CODE = 'S')  ORDER BY APP_NO DESC"
        Dim cmd As New SqlCommand(Sel, SqlConn)
        Dim rdr As SqlDataReader = Nothing
        Try
            SqlConn.Open()
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            If rdr.HasRows = True Then
                Me.DDL_Approval3.DataTextField = "APP_NO"
                Me.DDL_Approval3.DataValueField = "APP_NO"
                Me.DDL_Approval3.DataSource = rdr
                Me.DDL_Approval3.DataBind()
                DDL_Approval3.Items.Insert(0, "")
            Else
                Me.DDL_Approval3.DataSource = rdr
                Me.DDL_Approval3.DataBind()
            End If
            SqlConn.Close()
        Catch ex As System.Exception
            If rdr IsNot Nothing Then
                rdr.Close()
            Else
                SqlConn.Close()
            End If
            ex.Message.ToString()
        End Try
    End Sub
    'populate approval no
    Public Sub PopulateComiitees3()
        Me.DDL_Committees3.DataTextField = "COM_NAME"
        Me.DDL_Committees3.DataValueField = "COM_CODE"
        Me.DDL_Committees3.DataSource = GetCommitteeList(LibCode)
        Me.DDL_Committees3.DataBind()
        DDL_Committees3.Items.Insert(0, "")
    End Sub
    'display app date/comittee and fill title details
    Protected Sub DDL_Approval3_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDL_Approval3.SelectedIndexChanged
        Dim dt As DataTable
        Try
            Dim APP_NO As Object = Nothing
            If DDL_Approval3.Text <> "" Then
                APP_NO = DDL_Approval3.SelectedValue
                Dim SQL As String = Nothing
                SQL = "SELECT ACQ_ID, APP_NO, APP_DATE, COM_CODE, SUBS_YEAR, PROCESS_STATUS, COPY_PROPOSED, COPY_APPROVED  FROM ACQUISITIONS WHERE (APP_NO = '" & Trim(APP_NO) & "') AND (LIB_CODE ='" & (LibCode) & "')  "
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
                        txt_App_AppDate3.Text = Format(dt.Rows(0).Item("APP_DATE"), "dd/MM/yyyy").ToString
                    Else
                        txt_App_AppDate3.Text = ""
                    End If

                    If dt.Rows(0).Item("COM_CODE").ToString <> "" Then
                        DDL_Committees3.SelectedValue = dt.Rows(0).Item("COM_CODE").ToString
                    Else
                        DDL_Committees3.Text = ""
                    End If
                Else
                    txt_Acq_AppDate.Text = ""
                    DDL_Committees.Text = ""
                End If
            End If
            PopulateAppGrid3(APP_NO)
        Catch s As Exception
            Label7.Text = ""
            Label20.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'populate titles in the Grid
    Public Sub PopulateAppGrid3(ByVal APP_NO As Object)
        Dim dtSearch As DataTable = Nothing
        Try
            If APP_NO <> "" Then
                '**********************************************************************************
                Dim SQL As String = Nothing
                SQL = "SELECT ACQ_ID, APP_NO, APP_DATE, VOL_NO, PROCESS_STATUS, COPY_PROPOSED, COPY_APPROVED, CUR_CODE, ITEM_PRICE, CAT_NO, COM_CODE, SUBS_YEAR, TITLE FROM APPROVAL_VIEW2 WHERE (APP_NO = '" & APP_NO & "')  AND (PROCESS_STATUS = 'Approved' or PROCESS_STATUS = 'Sent For Approval' OR PROCESS_STATUS = 'Rejected') AND (LIB_CODE ='" & (LibCode) & "') AND  (BIB_CODE = 'S') ORDER BY ACQ_ID DESC"

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
                    Label13.Text = "Total Record(s): 0 "
                    App_UpdateApproval_Bttn.Visible = False
                Else
                    Grid3.Visible = True
                    RecordCount = dtSearch.Rows.Count
                    Grid3.DataSource = dtSearch
                    Grid3.DataBind()
                    Label13.Text = "Total Record(s): " & RecordCount
                    App_UpdateApproval_Bttn.Visible = True
                End If
                ViewState("dt") = dtSearch
                Label14.Text = "HELP: Check the Record if Approved, Un-Check the Record if Rejected"
                Label21.Text = ""
            Else
                Me.Grid3.DataSource = Nothing
                Grid3.DataBind()
                Label14.Text = ""
                Label21.Text = "No Record(s) found!"
            End If
        Catch s As Exception
            Label12.Text = "Error: " & (s.Message())
            Label11.Text = ""
            Label13.Text = ""
        Finally
            SqlConn.Close()
            DDL_Approval3.Focus()
        End Try
    End Sub
    'grid view page index changing event
    Protected Sub Grid3_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles Grid3.PageIndexChanging
        Try
            'rebind datagrid
            Grid3.DataSource = ViewState("dt") 'temp
            Grid3.PageIndex = e.NewPageIndex
            index = e.NewPageIndex * Grid3.PageSize
            Grid3.DataBind()
        Catch s As Exception
            Label12.Text = "Error:  there is error in page index !"
            Label11.Text = ""
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
            Dim Lblsr As Label = e.Row.FindControl("lblsr")
            Srno = index + 1
            If Lblsr Is Nothing = False Then
                Lblsr.Text = Srno
                index += 1
            End If

            'fill copy proposed in copy appd text box when it is blank
            Dim txtsn As TextBox = DirectCast(e.Row.FindControl("txt_App_CopyAppd"), TextBox)
            If txtsn.Text = "" Then
                If e.Row.Cells(3).Text <> "Rejected" Then
                    txtsn.Text = e.Row.Cells(4).Text
                End If
            End If
        End If
    End Sub
    'update selected Record from Grid (appd/rejected)
    Protected Sub App_UpdateApproval_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles App_UpdateApproval_Bttn.Click
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing
        Try
            If DDL_Approval3.SelectedValue <> "" Then
                If Grid3.Rows.Count <> 0 Then
                    For Each row As GridViewRow In Grid3.Rows
                        If row.RowType = DataControlRowType.DataRow Then
                            Dim cb As CheckBox = DirectCast(row.FindControl("cbd"), CheckBox)
                            Dim ID As Integer = Convert.ToInt32(Grid3.DataKeys(row.RowIndex).Value)
                            Dim CopyPpsd As Int32 = Nothing
                            If Grid3.Rows(row.RowIndex).Cells(4).Text <> "" Then
                                CopyPpsd = Convert.ToInt32(Grid3.Rows(row.RowIndex).Cells(4).Text)
                            Else
                                Label14.Text = ""
                                Label21.Text = "Copy Proposed Field is Blank!"
                                ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Copy Proposed Field is Blank!');", True)
                                Exit Sub
                            End If

                            Dim txtsn As TextBox = DirectCast(row.FindControl("txt_App_CopyAppd"), TextBox)
                            Dim CopyAppd As Int32 = Nothing
                            If txtsn.Text <> "" Then
                                CopyAppd = Convert.ToInt32(txtsn.Text)
                            Else
                                CopyAppd = 0
                            End If

                            If cb IsNot Nothing AndAlso cb.Checked = True Then 'approved
                                If txtsn.Text = "" Then
                                    Label14.Text = ""
                                    Label21.Text = "Plz Enter the No of Copies Approved!"
                                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Plz Enter the No of Copies Approved!');", True)
                                    txtsn.Enabled = True
                                    chckchkbox()
                                    txtsn.Focus()
                                    Exit Sub
                                Else
                                    If CShort(CopyAppd) > CShort(CopyPpsd) Then
                                        Label14.Text = ""
                                        Label21.Text = "No of Copies Approved should be equal or less then copies Proposed!"
                                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('No of Copies Approved should be equal or less then copies Proposed!');", True)
                                        txtsn.Enabled = True
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
                            SQL = "SELECT ACQ_ID, PROCESS_STATUS, COPY_APPROVED, DATE_MODIFIED, UPDATED_BY, IP FROM ACQUISITIONS WHERE (ACQ_ID = '" & ID & "')  AND (PROCESS_STATUS = 'Approved' or PROCESS_STATUS = 'Sent For Approval' or PROCESS_STATUS = 'Rejected') AND (LIB_CODE ='" & (LibCode) & "') "

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
                                    ds.Tables("APP").Rows(0)("PROCESS_STATUS") = "Approved"
                                    ds.Tables("APP").Rows(0)("COPY_APPROVED") = CopyAppd
                                Else
                                    ds.Tables("APP").Rows(0)("PROCESS_STATUS") = "Rejected"
                                    ds.Tables("APP").Rows(0)("COPY_APPROVED") = System.DBNull.Value
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
                    Label14.Text = "Record(s) Updated Successfully!"
                    Label21.Text = ""
                    PopulateAppGrid3(DDL_Approval3.SelectedValue)
                    DDL_Approval3.Focus()
                Else
                    Label14.Text = ""
                    Label21.Text = "Select Approval No from Drop-Down!"
                    DDL_Approval3.Focus()
                End If
            Else
                Label14.Text = ""
                Label21.Text = "Select Approval No from Drop-Down!"
                DDL_Approval3.Focus()
            End If
        Catch q As SqlException
            thisTransaction.Rollback()
        Catch s As Exception
            Label14.Text = ""
            Label21.Text = "Error: " & (s.Message())
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

                Dim txtsn As TextBox = DirectCast(row.FindControl("txt_App_CopyAppd"), TextBox)
                If cb IsNot Nothing AndAlso cb.Checked = True Then
                    txtsn.Enabled = True
                Else
                    txtsn.Enabled = False
                End If
            End If
        Next
    End Sub

    'load report
    Protected Sub App_Print_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles App_Print_Bttn.Click
        Try
            If DDL_PrintFormats.Text = "" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Plz Select Value from Drop-Down!');", True)
                DDL_PrintFormats.Focus()
                Exit Sub
            End If

            Dim Reportdoc As New ReportDocument
            Reportdoc = Report_Load_Letter()

            If DDL_PrintFormats.SelectedValue = "PDF" Then
                Reportdoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "Report_Approval_Books_" + Format(DateTime.Now, "ddMMyyyy"))
            End If
            If DDL_PrintFormats.SelectedValue = "DOC" Then
                Reportdoc.ExportToHttpResponse(ExportFormatType.WordForWindows, Response, True, "Report_Approval_Books_" + Format(DateTime.Now, "ddMMyyyy"))
            End If
        Catch ex As Exception
            Label20.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    'print approval form
    Public Function Report_Load_Letter() As ReportDocument
        Dim dtMess As DataTable = Nothing
        Dim dt As DataTable = Nothing
        Dim Rpt As New ReportDocument
        Try
            Dim APP_NO As Object = Nothing
            If DDL_Approvals.Text <> "" Then
                APP_NO = DDL_Approvals.SelectedValue
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Plz Select Value from Drop-Down!');", True)
                DDL_Approvals.Focus()
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

            dt = ViewState("dt")
            If dt.Rows.Count <> 0 Then

                Rpt.Load(Server.MapPath("~/Reports/Journals_Approval_Report.rpt"))
                Rpt.SetDataSource(dt)
                Rpt.Refresh()

                Dim FileNoText As CrystalReports.Engine.TextObject
                If myFileNo <> "" Then
                    FileNoText = Rpt.ReportDefinition.Sections("ReportHeaderSection1").ReportObjects.Item("Text13")
                    FileNoText.Text = myFileNo
                Else
                    'Rpt.ReportDefinition.Sections("ReportHeaderSection2").SectionFormat.EnableSuppress = True
                End If

                Dim AppNoText As CrystalReports.Engine.TextObject
                If APP_NO <> "" Then
                    AppNoText = Rpt.ReportDefinition.Sections("ReportHeaderSection2").ReportObjects.Item("Text7")
                    AppNoText.Text = APP_NO
                Else
                    Rpt.ReportDefinition.Sections("ReportHeaderSection2").SectionFormat.EnableSuppress = True
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