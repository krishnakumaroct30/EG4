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

Public Class Subscriptions
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
                        Me.Subs_Save_Bttn.Visible = True
                        Subs_GetData_Bttn.Visible = True
                        Subs_Cancel_Bttn.Visible = True
                        Subs_Update_Bttn.Visible = False
                        Subs_Delete_Bttn.Visible = False
                        Label15.Text = "Enter Data and Press SAVE Button to save the record.."
                        Label6.Text = ""
                        SearchTitle()
                        PopulateFrequencies()
                    End If
                    Dim CreateSUserButton As Object 'System.Web.UI.WebControls.Button
                    CreateSUserButton = Master.FindControl("Accordion1").FindControl("SerPane").FindControl("Ser_Subscription_Bttn")
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
                    Label14.Text = "Error:  Input is not Valid!"
                    Me.txt_Search_SearchString.Focus()
                    Exit Sub
                End If
                mySearchString = " " & mySearchString & " "
                If InStr(1, mySearchString, "CREATE", 1) > 0 Or InStr(1, mySearchString, "DELETE", 1) > 0 Or InStr(1, mySearchString, "DROP", 1) > 0 Or InStr(1, mySearchString, "INSERT", 1) > 1 Or InStr(1, mySearchString, "TRACK", 1) > 1 Or InStr(1, mySearchString, "TRACE", 1) > 1 Then
                    Label14.Text = "Error:  Input is not Valid !"
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
                    Label14.Text = "Error: data is not Valid !"
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
                    Label14.Text = "Error: Input is not Valid !"
                    Me.DropDownList5.Focus()
                    Exit Sub
                End If
                myfield = " " & myfield & " "
                If InStr(1, myfield, "CREATE", 1) > 0 Or InStr(1, myfield, "DELETE", 1) > 0 Or InStr(1, myfield, "DROP", 1) > 0 Or InStr(1, myfield, "INSERT", 1) > 1 Or InStr(1, myfield, "TRACK", 1) > 1 Or InStr(1, myfield, "TRACE", 1) > 1 Then
                    Label14.Text = "Error: Input is not Valid !"
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
                    Label14.Text = "Error: Input is not Valid !"
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
                    Label14.Text = "Error: Input is not Valid !"
                    Me.DropDownList6.Focus()
                    Exit Sub
                End If
                myBoolean = " " & myBoolean & " "
                If InStr(1, myBoolean, "CREATE", 1) > 0 Or InStr(1, myBoolean, "DELETE", 1) > 0 Or InStr(1, myBoolean, "DROP", 1) > 0 Or InStr(1, myBoolean, "INSERT", 1) > 1 Or InStr(1, myBoolean, "TRACK", 1) > 1 Or InStr(1, myBoolean, "TRACE", 1) > 1 Then
                    Label14.Text = "Error: Input is not Valid !"
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
                    Label14.Text = "Error: Input is not Valid !"
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
                SQL = "select CAT_NO, TITLE from CATS WHERE (CAT_NO IS NOT NULL) AND (BIB_CODE ='S') ORDER BY TITLE "
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
                Label33.Text = "Total Record(s): 0 "
            Else
                RecordCount = dtSearch.Rows.Count
                Me.DDL_Titles.DataSource = dtSearch
                Me.DDL_Titles.DataTextField = "TITLE"
                Me.DDL_Titles.DataValueField = "CAT_NO"
                Me.DDL_Titles.DataBind()
                DDL_Titles.Items.Insert(0, "")
                Label33.Text = "Total Record(s): " & RecordCount
                DDL_Titles.Focus()
            End If
            ViewState("dt") = dtSearch
        Catch s As Exception
            Label14.Text = "Error: " & (s.Message())
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

            'clear subs fields
            Label9.Text = ""
            Label10.Text = ""
            Label26.Text = ""
            Label28.Text = ""
            Label25.Text = ""
            Label11.Text = ""
            Label12.Text = ""
            Label27.Text = ""
            Label29.Text = ""
            Label30.Text = ""
            Label31.Text = ""
            Label32.Text = ""
            Label13.Text = ""
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
            PopulateSubscriptionYears()
        Catch ex As Exception
            Label14.Text = "Error: " & (ex.Message())
        Finally
            SqlConn.Close()
            DDL_Titles.Focus()
        End Try
    End Sub
    'populate Purchasing records of selected Joruanl in drop-down
    Public Sub PopulateSubscriptionYears()
        Dim Sel As String = "SELECT DISTINCT SUBS_YEAR FROM ACQUISITIONS WHERE (LIB_CODE ='" & (LibCode) & "') AND  (SUBS_YEAR IS NOT NULL)  AND (CAT_NO = '" & Trim(Label19.Text) & "') ORDER BY SUBS_YEAR DESC"
        Dim cmd As New SqlCommand(Sel, SqlConn)
        Dim rdr As SqlDataReader = Nothing
        Try
            Label14.Text = ""
            If DDL_Titles.Text <> "" Then
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
                    Label14.Text = "No Acquisition Record Exists for This Serial!"
                End If
                SqlConn.Close()
                DDL_SubscriptionYears.Focus()
            Else
                Me.DDL_SubscriptionYears.DataSource = Nothing
                Me.DDL_SubscriptionYears.DataBind()
                DDL_SubscriptionYears.Items.Clear()
            End If
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
    'display purchasing record of selected year
    Protected Sub DDL_SubscriptionYears_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDL_SubscriptionYears.SelectedIndexChanged
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
                        SQL = "SELECT ACQUISITIONS.*, VENDORS.VEND_NAME FROM ACQUISITIONS LEFT OUTER JOIN VENDORS ON ACQUISITIONS.VEND_ID = VENDORS.VEND_ID WHERE (ACQUISITIONS.CAT_NO = '" & Trim(Label19.Text) & "') AND (ACQUISITIONS.LIB_CODE = '" & Trim(LibCode) & "') AND (ACQUISITIONS.SUBS_YEAR = '" & Trim(DDL_SubscriptionYears.SelectedValue) & "') "
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
                        If dt.Rows(0).Item("ACQ_ID").ToString <> "" Then
                            Label9.Text = dt.Rows(0).Item("ACQ_ID").ToString
                        Else
                            Label9.Text = ""
                        End If

                        If dt.Rows(0).Item("ACQMODE_CODE").ToString <> "" Then
                            Label26.Text = dt.Rows(0).Item("ACQMODE_CODE").ToString
                        Else
                            Label26.Text = ""
                        End If

                        If dt.Rows(0).Item("COM_CODE").ToString <> "" Then
                            Label28.Text = dt.Rows(0).Item("COM_CODE").ToString
                        Else
                            Label28.Text = dt.Rows(0).Item("COM_CODE").ToString
                        End If

                        If dt.Rows(0).Item("APP_NO").ToString <> "" Then
                            Label10.Text = dt.Rows(0).Item("APP_NO").ToString
                        Else
                            Label10.Text = ""
                        End If

                        If dt.Rows(0).Item("APP_DATE").ToString <> "" Then
                            Label25.Text = Format(dt.Rows(0).Item("APP_DATE"), "dd/MM/yyyy")
                        Else
                            Label25.Text = ""
                        End If

                        If dt.Rows(0).Item("SUBS_YEAR").ToString <> "" Then
                            Label11.Text = dt.Rows(0).Item("SUBS_YEAR").ToString
                        Else
                            Label11.Text = ""
                        End If

                        If dt.Rows(0).Item("ORDER_NO").ToString <> "" Then
                            Label12.Text = dt.Rows(0).Item("ORDER_NO").ToString
                        Else
                            Label12.Text = ""
                        End If

                        If dt.Rows(0).Item("ORDER_DATE").ToString <> "" Then
                            Label27.Text = Format(dt.Rows(0).Item("ORDER_DATE"), "dd/MM/yyyy")
                        Else
                            Label27.Text = ""
                        End If

                        If dt.Rows(0).Item("ITEM_PRICE").ToString <> "" Then
                            Label30.Text = dt.Rows(0).Item("CUR_CODE").ToString & ": " & dt.Rows(0).Item("ITEM_PRICE").ToString
                        Else
                            Label30.Text = ""
                        End If

                        If dt.Rows(0).Item("PROCESS_STATUS").ToString <> "" Then
                            Label31.Text = dt.Rows(0).Item("PROCESS_STATUS").ToString
                        Else
                            Label31.Text = ""
                        End If

                        If dt.Rows(0).Item("VEND_NAME").ToString <> "" Then
                            Label13.Text = dt.Rows(0).Item("VEND_NAME").ToString
                        Else
                            Label13.Text = ""
                        End If

                        If dt.Rows(0).Item("COPY_ORDERED").ToString <> "" Then
                            Label29.Text = dt.Rows(0).Item("COPY_ORDERED").ToString
                        Else
                            Label29.Text = ""
                        End If

                        If dt.Rows(0).Item("NOTE").ToString <> "" Then
                            Label32.Text = dt.Rows(0).Item("NOTE").ToString
                        Else
                            Label32.Text = ""
                        End If
                        SqlConn.Close()

                        'Display Susb Record
                        DisplaySubsRecord()
                    Else
                        Label9.Text = ""
                        Label10.Text = ""
                        Label26.Text = ""
                        Label28.Text = ""
                        Label25.Text = ""
                        Label11.Text = ""
                        Label12.Text = ""
                        Label27.Text = ""
                        Label29.Text = ""
                        Label30.Text = ""
                        Label31.Text = ""
                        Label32.Text = ""
                        Label13.Text = ""

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
                    Label9.Text = ""
                    Label10.Text = ""
                    Label26.Text = ""
                    Label28.Text = ""
                    Label25.Text = ""
                    Label11.Text = ""
                    Label12.Text = ""
                    Label27.Text = ""
                    Label29.Text = ""
                    Label30.Text = ""
                    Label31.Text = ""
                    Label32.Text = ""
                    Label13.Text = ""

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
            End If
        Catch ex As Exception
            Label14.Text = "Error: " & (ex.Message())
        Finally
            SqlConn.Close()
            DDL_SubscriptionYears.Focus()
        End Try
    End Sub
    Public Sub DisplaySubsRecord()
        Dim dt As New DataTable
        Try
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
                        Subs_Save_Bttn.Visible = False
                        Subs_GetData_Bttn.Visible = False
                        Subs_Update_Bttn.Visible = True
                        Subs_Delete_Bttn.Visible = True
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
                        Subs_Save_Bttn.Visible = True
                        Subs_GetData_Bttn.Visible = True
                        Subs_Update_Bttn.Visible = False
                        Subs_Delete_Bttn.Visible = False
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
                    Subs_Save_Bttn.Visible = True
                    Subs_GetData_Bttn.Visible = True
                    Subs_Update_Bttn.Visible = False
                    Subs_Delete_Bttn.Visible = False
                End If

            End If
        Catch ex As Exception
            Label14.Text = "Error: " & (ex.Message())
        Finally
            SqlConn.Close()
            DDL_SubscriptionYears.Focus()
        End Try
    End Sub
    Public Sub PopulateFrequencies()
        Me.DDL_Frequencies.DataTextField = "FREQ_NAME"
        Me.DDL_Frequencies.DataValueField = "FREQ_CODE"
        Me.DDL_Frequencies.DataSource = GetFreqList()
        Me.DDL_Frequencies.DataBind()
        DDL_Frequencies.Items.Insert(0, "")
    End Sub
    'save record
    Protected Sub App_Save_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Subs_Save_Bttn.Click
        If IsPostBack = True Then
            'check unwanted characters
            Dim iloop As Integer
            Dim strcurrentchar As Object
            Dim c As Integer
            Dim counter1, counter2, counter3, counter4, counter5, counter6, counter7, counter8, counter9 As Integer
            Dim counter10, counter11, counter12, counter13, counter14, counter15, counter16, counter17, counter18 As Integer
            Dim thisTransaction As SqlClient.SqlTransaction = Nothing
            Try
                If DDL_Titles.Text = "" Then
                    Label6.Text = "Plz Select Title from Drop-Down!"
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Plz Select Title from Drop-Down!');", True)
                    Label15.Text = ""
                    DDL_Titles.Focus()
                    Exit Sub
                End If

                If Label31.Text <> "Ordered" Then
                    Label6.Text = "Title Has not been Ordered, First Place the Order of the Title!"
                    Label15.Text = ""
                    DDL_Titles.Focus()
                    Exit Sub
                End If

                'validation for cat_no
                Dim CAT_NO As Integer = Nothing
                If Me.DDL_Titles.Text <> "" Then
                    CAT_NO = Trim(DDL_Titles.SelectedValue)
                    CAT_NO = RemoveQuotes(CAT_NO)
                    If CAT_NO.ToString.Length > 5 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_Titles.Focus()
                        Exit Sub
                    End If
                    CAT_NO = " " & CAT_NO & " "
                    If InStr(1, CAT_NO, "CREATE", 1) > 0 Or InStr(1, CAT_NO, "DELETE", 1) > 0 Or InStr(1, CAT_NO, "DROP", 1) > 0 Or InStr(1, CAT_NO, "INSERT", 1) > 1 Or InStr(1, CAT_NO, "TRACK", 1) > 1 Or InStr(1, CAT_NO, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_IssuesPerYear.Focus()
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
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_Titles.Focus()
                        Exit Sub
                    End If
                Else
                    Label6.Text = "Error: Plz Select Title from Drop-Down !"
                    Label15.Text = ""
                    DDL_Titles.Focus()
                    Exit Sub
                End If

                'validation for cat_no
                Dim ACQ_ID As Integer = Nothing
                If Me.Label9.Text <> "" Then
                    ACQ_ID = Trim(Label9.Text)
                    ACQ_ID = RemoveQuotes(ACQ_ID)
                    If ACQ_ID.ToString.Length > 5 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_SubscriptionYears.Focus()
                        Exit Sub
                    End If
                    ACQ_ID = " " & ACQ_ID & " "
                    If InStr(1, ACQ_ID, "CREATE", 1) > 0 Or InStr(1, ACQ_ID, "DELETE", 1) > 0 Or InStr(1, ACQ_ID, "DROP", 1) > 0 Or InStr(1, ACQ_ID, "INSERT", 1) > 1 Or InStr(1, ACQ_ID, "TRACK", 1) > 1 Or InStr(1, ACQ_ID, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_SubscriptionYears.Focus()
                        Exit Sub
                    End If
                    ACQ_ID = TrimX(ACQ_ID)
                    'check unwanted characters
                    c = 0
                    counter2 = 0
                    For iloop = 1 To Len(ACQ_ID.ToString)
                        strcurrentchar = Mid(ACQ_ID, iloop, 1)
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
                        DDL_SubscriptionYears.Focus()
                        Exit Sub
                    End If
                Else
                    Label6.Text = "Error: Plz Select Title from Drop-Down !"
                    Label15.Text = ""
                    DDL_SubscriptionYears.Focus()
                    Exit Sub
                End If

                'validation for Subscription eyar No
                Dim SUBS_YEAR As Integer = Nothing
                If Me.DDL_SubscriptionYears.Text <> "" Then
                    SUBS_YEAR = TrimX(DDL_SubscriptionYears.SelectedValue)
                    SUBS_YEAR = RemoveQuotes(SUBS_YEAR)
                    If SUBS_YEAR.ToString.Length > 5 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_SubscriptionYears.Focus()
                        Exit Sub
                    End If
                    SUBS_YEAR = " " & SUBS_YEAR & " "
                    If InStr(1, SUBS_YEAR, "CREATE", 1) > 0 Or InStr(1, SUBS_YEAR, "DELETE", 1) > 0 Or InStr(1, SUBS_YEAR, "DROP", 1) > 0 Or InStr(1, SUBS_YEAR, "INSERT", 1) > 1 Or InStr(1, SUBS_YEAR, "TRACK", 1) > 1 Or InStr(1, SUBS_YEAR, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_SubscriptionYears.Focus()
                        Exit Sub
                    End If
                    SUBS_YEAR = TrimX(SUBS_YEAR)
                    'check unwanted characters
                    c = 0
                    counter3 = 0
                    For iloop = 1 To Len(SUBS_YEAR.ToString)
                        strcurrentchar = Mid(SUBS_YEAR, iloop, 1)
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
                        DDL_SubscriptionYears.Focus()
                        Exit Sub
                    End If
                Else
                    Label6.Text = "Error: Plz Select Title from Drop-Down !"
                    Label15.Text = ""
                    DDL_SubscriptionYears.Focus()
                    Exit Sub
                End If

                'check this app no for same year
                Dim str2 As Object = Nothing
                Dim flag2 As Object = Nothing
                str2 = "SELECT SUBS_ID FROM SUBSCRIPTIONS WHERE (CAT_NO ='" & Trim(CAT_NO) & "') AND (SUBS_YEAR='" & Trim(SUBS_YEAR) & "') AND (LIB_CODE ='" & Trim(LibCode) & "')"
                Dim cmd2 As New SqlCommand(str2, SqlConn)
                SqlConn.Open()
                flag2 = cmd2.ExecuteScalar
                SqlConn.Close()
                If flag2 <> Nothing Then
                    Label6.Text = "Subscription Record for this  Year Already Exists!"
                    Label15.Text = ""
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Subscription Record For This Year Already Exists!');", True)
                    DDL_SubscriptionYears.Focus()
                    Exit Sub
                End If

                'validation for VOL_YEAR
                Dim VOL_YEAR As Integer = Nothing
                If Me.txt_Subs_VolPerYear.Text <> "" Then
                    VOL_YEAR = Convert.ToInt16(TrimX(txt_Subs_VolPerYear.Text))
                    VOL_YEAR = RemoveQuotes(VOL_YEAR)
                    If Not IsNumeric(VOL_YEAR) = True Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_VolPerYear.Focus()
                        Exit Sub
                    End If
                    If Len(VOL_YEAR) > 5 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_VolPerYear.Focus()
                        Exit Sub
                    End If
                    VOL_YEAR = " " & VOL_YEAR & " "
                    If InStr(1, VOL_YEAR, "CREATE", 1) > 0 Or InStr(1, VOL_YEAR, "DELETE", 1) > 0 Or InStr(1, VOL_YEAR, "DROP", 1) > 0 Or InStr(1, VOL_YEAR, "INSERT", 1) > 1 Or InStr(1, VOL_YEAR, "TRACK", 1) > 1 Or InStr(1, VOL_YEAR, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_VolPerYear.Focus()
                        Exit Sub
                    End If
                    VOL_YEAR = TrimX(VOL_YEAR)
                    'check unwanted characters
                    c = 0
                    counter4 = 0
                    For iloop = 1 To Len(VOL_YEAR.ToString)
                        strcurrentchar = Mid(VOL_YEAR, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter4 = 1
                            End If
                        End If
                    Next
                    If counter4 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_VolPerYear.Focus()
                        Exit Sub
                    End If
                Else
                    VOL_YEAR = Nothing
                End If

                'validation for ISSUE_YEAR
                Dim ISSUE_YEAR As Integer = Nothing
                If Me.txt_Subs_IssuesPerYear.Text <> "" Then
                    ISSUE_YEAR = Convert.ToInt16(TrimX(txt_Subs_IssuesPerYear.Text))
                    ISSUE_YEAR = RemoveQuotes(ISSUE_YEAR)
                    If Not IsNumeric(ISSUE_YEAR) = True Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_IssuesPerYear.Focus()
                        Exit Sub
                    End If
                    If Len(ISSUE_YEAR) > 5 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_IssuesPerYear.Focus()
                        Exit Sub
                    End If
                    ISSUE_YEAR = " " & ISSUE_YEAR & " "
                    If InStr(1, ISSUE_YEAR, "CREATE", 1) > 0 Or InStr(1, ISSUE_YEAR, "DELETE", 1) > 0 Or InStr(1, ISSUE_YEAR, "DROP", 1) > 0 Or InStr(1, ISSUE_YEAR, "INSERT", 1) > 1 Or InStr(1, ISSUE_YEAR, "TRACK", 1) > 1 Or InStr(1, ISSUE_YEAR, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_IssuesPerYear.Focus()
                        Exit Sub
                    End If
                    ISSUE_YEAR = TrimX(ISSUE_YEAR)
                    'check unwanted characters
                    c = 0
                    counter5 = 0
                    For iloop = 1 To Len(ISSUE_YEAR.ToString)
                        strcurrentchar = Mid(ISSUE_YEAR, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter5 = 1
                            End If
                        End If
                    Next
                    If counter5 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_IssuesPerYear.Focus()
                        Exit Sub
                    End If
                Else
                    Label6.Text = "Error: Input is not Valid !"
                    Label15.Text = ""
                    txt_Subs_IssuesPerYear.Focus()
                    Exit Sub
                End If

                'validation for ISSUE_VOL
                Dim ISSUE_VOL As Integer = Nothing
                If Me.txt_Subs_IssuesPerVol.Text <> "" Then
                    ISSUE_VOL = Convert.ToInt16(TrimX(txt_Subs_IssuesPerVol.Text))
                    ISSUE_VOL = RemoveQuotes(ISSUE_VOL)
                    If Not IsNumeric(ISSUE_VOL) = True Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_IssuesPerVol.Focus()
                        Exit Sub
                    End If
                    If Len(ISSUE_VOL) > 5 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_IssuesPerVol.Focus()
                        Exit Sub
                    End If
                    ISSUE_VOL = " " & ISSUE_VOL & " "
                    If InStr(1, ISSUE_VOL, "CREATE", 1) > 0 Or InStr(1, ISSUE_VOL, "DELETE", 1) > 0 Or InStr(1, ISSUE_VOL, "DROP", 1) > 0 Or InStr(1, ISSUE_VOL, "INSERT", 1) > 1 Or InStr(1, ISSUE_VOL, "TRACK", 1) > 1 Or InStr(1, ISSUE_VOL, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_IssuesPerVol.Focus()
                        Exit Sub
                    End If
                    ISSUE_VOL = TrimX(ISSUE_VOL)
                    'check unwanted characters
                    c = 0
                    counter6 = 0
                    For iloop = 1 To Len(ISSUE_VOL.ToString)
                        strcurrentchar = Mid(ISSUE_VOL, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter6 = 1
                            End If
                        End If
                    Next
                    If counter6 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_IssuesPerVol.Focus()
                        Exit Sub
                    End If
                Else
                    ISSUE_VOL = Nothing
                End If

                'validation for S_VOL
                Dim S_VOL As Integer = Nothing
                If Me.txt_Subs_StartVolNo.Text <> "" Then
                    S_VOL = Convert.ToInt16(TrimX(txt_Subs_StartVolNo.Text))
                    S_VOL = RemoveQuotes(S_VOL)
                    If Not IsNumeric(S_VOL) = True Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_StartVolNo.Focus()
                        Exit Sub
                    End If
                    If Len(S_VOL) > 5 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_StartVolNo.Focus()
                        Exit Sub
                    End If
                    S_VOL = " " & S_VOL & " "
                    If InStr(1, S_VOL, "CREATE", 1) > 0 Or InStr(1, S_VOL, "DELETE", 1) > 0 Or InStr(1, S_VOL, "DROP", 1) > 0 Or InStr(1, S_VOL, "INSERT", 1) > 1 Or InStr(1, S_VOL, "TRACK", 1) > 1 Or InStr(1, S_VOL, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_StartVolNo.Focus()
                        Exit Sub
                    End If
                    S_VOL = TrimX(S_VOL)
                    'check unwanted characters
                    c = 0
                    counter7 = 0
                    For iloop = 1 To Len(S_VOL.ToString)
                        strcurrentchar = Mid(S_VOL, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter7 = 1
                            End If
                        End If
                    Next
                    If counter7 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_StartVolNo.Focus()
                        Exit Sub
                    End If
                Else
                    S_VOL = Nothing
                End If

                'validation for S_ISSUE
                Dim S_ISSUE As Integer = Nothing
                If Me.txt_Subs_StartIssueNo.Text <> "" Then
                    S_ISSUE = Convert.ToInt16(TrimX(txt_Subs_StartIssueNo.Text))
                    S_ISSUE = RemoveQuotes(S_ISSUE)
                    If Not IsNumeric(S_ISSUE) = True Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_StartIssueNo.Focus()
                        Exit Sub
                    End If
                    If Len(S_ISSUE) > 5 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_StartIssueNo.Focus()
                        Exit Sub
                    End If
                    S_ISSUE = " " & S_ISSUE & " "
                    If InStr(1, S_ISSUE, "CREATE", 1) > 0 Or InStr(1, S_ISSUE, "DELETE", 1) > 0 Or InStr(1, S_ISSUE, "DROP", 1) > 0 Or InStr(1, S_ISSUE, "INSERT", 1) > 1 Or InStr(1, S_ISSUE, "TRACK", 1) > 1 Or InStr(1, S_ISSUE, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_StartIssueNo.Focus()
                        Exit Sub
                    End If
                    S_ISSUE = TrimX(S_ISSUE)
                    'check unwanted characters
                    c = 0
                    counter8 = 0
                    For iloop = 1 To Len(S_ISSUE.ToString)
                        strcurrentchar = Mid(S_ISSUE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter8 = 1
                            End If
                        End If
                    Next
                    If counter8 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_StartIssueNo.Focus()
                        Exit Sub
                    End If
                Else
                    S_ISSUE = Nothing
                End If

                'search start date
                Dim S_DATE As Object = Nothing
                If txt_Subs_SDate.Text <> "" Then
                    S_DATE = TrimX(txt_Subs_SDate.Text)
                    S_DATE = Convert.ToDateTime(S_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                    If Len(S_DATE) > 12 Then
                        Label6.Text = " Input is not Valid..."
                        Me.txt_Subs_SDate.Focus()
                        Exit Sub
                    End If
                    S_DATE = " " & S_DATE & " "
                    If InStr(1, S_DATE, "CREATE", 1) > 0 Or InStr(1, S_DATE, "DELETE", 1) > 0 Or InStr(1, S_DATE, "DROP", 1) > 0 Or InStr(1, S_DATE, "INSERT", 1) > 1 Or InStr(1, S_DATE, "TRACK", 1) > 1 Or InStr(1, S_DATE, "TRACE", 1) > 1 Then
                        Label6.Text = "  Input is not Valid... "
                        Me.txt_Subs_SDate.Focus()
                        Exit Sub
                    End If
                    S_DATE = TrimX(S_DATE)
                    'check unwanted characters
                    c = 0
                    counter9 = 0
                    For iloop = 1 To Len(S_DATE)
                        strcurrentchar = Mid(S_DATE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter9 = 1
                            End If
                        End If
                    Next
                    If counter9 = 1 Then
                        Label6.Text = "data is not Valid... "
                        Me.txt_Subs_SDate.Focus()
                        Exit Sub
                    End If
                Else
                    Label6.Text = "Plz Enter Subscription Start Date!"
                    Me.txt_Subs_SDate.Focus()
                    Exit Sub
                End If

                'search End date
                Dim E_DATE As Object = Nothing
                If txt_Subs_EDate.Text <> "" Then
                    E_DATE = TrimX(txt_Subs_EDate.Text)
                    E_DATE = Convert.ToDateTime(E_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                    If Len(E_DATE) > 12 Then
                        Label6.Text = " Input is not Valid..."
                        Me.txt_Subs_EDate.Focus()
                        Exit Sub
                    End If
                    E_DATE = " " & E_DATE & " "
                    If InStr(1, E_DATE, "CREATE", 1) > 0 Or InStr(1, E_DATE, "DELETE", 1) > 0 Or InStr(1, E_DATE, "DROP", 1) > 0 Or InStr(1, E_DATE, "INSERT", 1) > 1 Or InStr(1, E_DATE, "TRACK", 1) > 1 Or InStr(1, E_DATE, "TRACE", 1) > 1 Then
                        Label6.Text = "  Input is not Valid... "
                        Me.txt_Subs_EDate.Focus()
                        Exit Sub
                    End If
                    E_DATE = TrimX(E_DATE)
                    'check unwanted characters
                    c = 0
                    counter10 = 0
                    For iloop = 1 To Len(E_DATE)
                        strcurrentchar = Mid(E_DATE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter10 = 1
                            End If
                        End If
                    Next
                    If counter10 = 1 Then
                        Label6.Text = "data is not Valid... "
                        Me.txt_Subs_EDate.Focus()
                        Exit Sub
                    End If
                Else
                    Label6.Text = "Plz Enter Subscription End Date!"
                    Me.txt_Subs_EDate.Focus()
                    Exit Sub
                End If

                'search first issue  date
                Dim F_ISSUE_DATE As Object = Nothing
                If txt_Subs_FisrtIssueDate.Text <> "" Then
                    F_ISSUE_DATE = TrimX(txt_Subs_FisrtIssueDate.Text)
                    F_ISSUE_DATE = Convert.ToDateTime(F_ISSUE_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                    If Len(F_ISSUE_DATE) > 12 Then
                        Label6.Text = " Input is not Valid..."
                        Me.txt_Subs_FisrtIssueDate.Focus()
                        Exit Sub
                    End If
                    F_ISSUE_DATE = " " & F_ISSUE_DATE & " "
                    If InStr(1, F_ISSUE_DATE, "CREATE", 1) > 0 Or InStr(1, F_ISSUE_DATE, "DELETE", 1) > 0 Or InStr(1, F_ISSUE_DATE, "DROP", 1) > 0 Or InStr(1, F_ISSUE_DATE, "INSERT", 1) > 1 Or InStr(1, F_ISSUE_DATE, "TRACK", 1) > 1 Or InStr(1, F_ISSUE_DATE, "TRACE", 1) > 1 Then
                        Label6.Text = "  Input is not Valid... "
                        Me.txt_Subs_FisrtIssueDate.Focus()
                        Exit Sub
                    End If
                    F_ISSUE_DATE = TrimX(F_ISSUE_DATE)
                    'check unwanted characters
                    c = 0
                    counter11 = 0
                    For iloop = 1 To Len(F_ISSUE_DATE)
                        strcurrentchar = Mid(F_ISSUE_DATE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter11 = 1
                            End If
                        End If
                    Next
                    If counter11 = 1 Then
                        Label6.Text = "data is not Valid... "
                        Me.txt_Subs_FisrtIssueDate.Focus()
                        Exit Sub
                    End If
                Else
                    Label6.Text = "Plz Enter Date of First Issue!"
                    Me.txt_Subs_FisrtIssueDate.Focus()
                    Exit Sub
                End If

                'validation for GRACE_DAYS
                Dim GRACE_DAYS As Integer = Nothing
                If Me.txt_Subs_GraceDays.Text <> "" Then
                    GRACE_DAYS = Convert.ToInt16(TrimX(txt_Subs_GraceDays.Text))
                    GRACE_DAYS = RemoveQuotes(GRACE_DAYS)
                    If Not IsNumeric(GRACE_DAYS) = True Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_GraceDays.Focus()
                        Exit Sub
                    End If
                    If Len(GRACE_DAYS) > 5 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_GraceDays.Focus()
                        Exit Sub
                    End If
                    GRACE_DAYS = " " & GRACE_DAYS & " "
                    If InStr(1, GRACE_DAYS, "CREATE", 1) > 0 Or InStr(1, GRACE_DAYS, "DELETE", 1) > 0 Or InStr(1, GRACE_DAYS, "DROP", 1) > 0 Or InStr(1, GRACE_DAYS, "INSERT", 1) > 1 Or InStr(1, GRACE_DAYS, "TRACK", 1) > 1 Or InStr(1, GRACE_DAYS, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_GraceDays.Focus()
                        Exit Sub
                    End If
                    GRACE_DAYS = TrimX(GRACE_DAYS)
                    'check unwanted characters
                    c = 0
                    counter12 = 0
                    For iloop = 1 To Len(GRACE_DAYS.ToString)
                        strcurrentchar = Mid(GRACE_DAYS, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter12 = 1
                            End If
                        End If
                    Next
                    If counter12 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_GraceDays.Focus()
                        Exit Sub
                    End If
                Else
                    GRACE_DAYS = Nothing
                End If

                'validation for COPY
                Dim COPY As Integer = Nothing
                If Me.Label29.Text <> "" Then
                    COPY = Convert.ToInt16(TrimX(Label29.Text))
                    COPY = RemoveQuotes(COPY)
                    If Not IsNumeric(COPY) = True Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        Exit Sub
                    End If
                    If Len(COPY) > 5 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        Exit Sub
                    End If
                    COPY = " " & COPY & " "
                    If InStr(1, COPY, "CREATE", 1) > 0 Or InStr(1, COPY, "DELETE", 1) > 0 Or InStr(1, COPY, "DROP", 1) > 0 Or InStr(1, COPY, "INSERT", 1) > 1 Or InStr(1, COPY, "TRACK", 1) > 1 Or InStr(1, COPY, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        Exit Sub
                    End If
                    COPY = TrimX(COPY)
                    'check unwanted characters
                    c = 0
                    counter13 = 0
                    For iloop = 1 To Len(COPY.ToString)
                        strcurrentchar = Mid(COPY, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter13 = 1
                            End If
                        End If
                    Next
                    If counter13 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        Exit Sub
                    End If
                Else
                    Label6.Text = "Error: Copy Ordered is Not Entered !"
                    Label15.Text = ""
                    Exit Sub
                End If


                'validation for ISSUE_CONTINUE
                Dim ISSUE_CONTINUE As Object = Nothing
                If DDL_Continue.Text <> "" Then
                    ISSUE_CONTINUE = DDL_Continue.SelectedValue
                    ISSUE_CONTINUE = RemoveQuotes(ISSUE_CONTINUE)
                    If ISSUE_CONTINUE.Length > 2 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_Continue.Focus()
                        Exit Sub
                    End If
                    ISSUE_CONTINUE = " " & ISSUE_CONTINUE & " "
                    If InStr(1, ISSUE_CONTINUE, "CREATE", 1) > 0 Or InStr(1, ISSUE_CONTINUE, "DELETE", 1) > 0 Or InStr(1, ISSUE_CONTINUE, "DROP", 1) > 0 Or InStr(1, ISSUE_CONTINUE, "INSERT", 1) > 1 Or InStr(1, ISSUE_CONTINUE, "TRACK", 1) > 1 Or InStr(1, ISSUE_CONTINUE, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_Continue.Focus()
                        Exit Sub
                    End If
                    ISSUE_CONTINUE = TrimX(ISSUE_CONTINUE)
                    'check unwanted characters
                    c = 0
                    counter14 = 0
                    For iloop = 1 To Len(ISSUE_CONTINUE)
                        strcurrentchar = Mid(ISSUE_CONTINUE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter14 = 1
                            End If
                        End If
                    Next
                    If counter14 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_Continue.Focus()
                        Exit Sub
                    End If
                Else
                    ISSUE_CONTINUE = "N"
                End If

                'validation for SUBS_NO
                Dim SUBS_NO As Object = Nothing
                If txt_Subs_SubsNo.Text <> "" Then
                    SUBS_NO = txt_Subs_SubsNo.Text
                    SUBS_NO = RemoveQuotes(SUBS_NO)
                    If SUBS_NO.Length > 100 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_SubsNo.Focus()
                        Exit Sub
                    End If
                    SUBS_NO = " " & SUBS_NO & " "
                    If InStr(1, SUBS_NO, "CREATE", 1) > 0 Or InStr(1, SUBS_NO, "DELETE", 1) > 0 Or InStr(1, SUBS_NO, "DROP", 1) > 0 Or InStr(1, SUBS_NO, "INSERT", 1) > 1 Or InStr(1, SUBS_NO, "TRACK", 1) > 1 Or InStr(1, SUBS_NO, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_SubsNo.Focus()
                        Exit Sub
                    End If
                    SUBS_NO = TrimX(SUBS_NO)
                    'check unwanted characters
                    c = 0
                    counter15 = 0
                    For iloop = 1 To Len(SUBS_NO)
                        strcurrentchar = Mid(SUBS_NO, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter15 = 1
                            End If
                        End If
                    Next
                    If counter15 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_SubsNo.Focus()
                        Exit Sub
                    End If
                Else
                    SUBS_NO = ""
                End If

                'Server validation for  : txt_Cat_Remarks
                Dim REMARKS As Object = Nothing
                If txt_Subs_Remarks.Text <> "" Then
                    REMARKS = TrimAll(txt_Subs_Remarks.Text)
                    If REMARKS.Length > 250 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Subs_Remarks.Focus()
                        Exit Sub
                    End If

                    REMARKS = " " & REMARKS & " "
                    If InStr(1, REMARKS, "CREATE", 1) > 0 Or InStr(1, REMARKS, "DELETE", 1) > 0 Or InStr(1, REMARKS, "DROP", 1) > 0 Or InStr(1, REMARKS, "INSERT", 1) > 1 Or InStr(1, REMARKS, "TRACK", 1) > 1 Or InStr(1, REMARKS, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Subs_Remarks.Focus()
                        Exit Sub
                    End If
                    REMARKS = TrimAll(REMARKS)
                    'check unwanted characters
                    c = 0
                    counter16 = 0
                    For iloop = 1 To Len(REMARKS.ToString)
                        strcurrentchar = Mid(REMARKS, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter16 = 1
                            End If
                        End If
                    Next
                    If counter16 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Subs_Remarks.Focus()
                        Exit Sub
                    End If
                Else
                    REMARKS = ""
                End If

                'Server validation for  : FREQ_CODE
                Dim FREQ_CODE As Object = Nothing
                If DDL_Frequencies.Text <> "" Then
                    FREQ_CODE = TrimAll(DDL_Frequencies.Text)
                    If FREQ_CODE.Length > 3 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        DDL_Frequencies.Focus()
                        Exit Sub
                    End If

                    FREQ_CODE = " " & FREQ_CODE & " "
                    If InStr(1, FREQ_CODE, "CREATE", 1) > 0 Or InStr(1, FREQ_CODE, "DELETE", 1) > 0 Or InStr(1, FREQ_CODE, "DROP", 1) > 0 Or InStr(1, FREQ_CODE, "INSERT", 1) > 1 Or InStr(1, FREQ_CODE, "TRACK", 1) > 1 Or InStr(1, FREQ_CODE, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.DDL_Frequencies.Focus()
                        Exit Sub
                    End If
                    FREQ_CODE = TrimX(FREQ_CODE)
                    'check unwanted characters
                    c = 0
                    counter17 = 0
                    For iloop = 1 To Len(FREQ_CODE.ToString)
                        strcurrentchar = Mid(FREQ_CODE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter17 = 1
                            End If
                        End If
                    Next
                    If counter17 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.DDL_Frequencies.Focus()
                        Exit Sub
                    End If
                Else
                    Label6.Text = " Do Not use un-wanted Characters... "
                    Label15.Text = ""
                    Me.DDL_Frequencies.Focus()
                    Exit Sub
                End If

                'Server validation for  : LOCATION
                Dim LOCATION As Object = Nothing
                If txt_Subs_Location.Text <> "" Then
                    LOCATION = TrimAll(txt_Subs_Location.Text)
                    If LOCATION.Length > 50 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Subs_Location.Focus()
                        Exit Sub
                    End If

                    LOCATION = " " & LOCATION & " "
                    If InStr(1, LOCATION, "CREATE", 1) > 0 Or InStr(1, LOCATION, "DELETE", 1) > 0 Or InStr(1, LOCATION, "DROP", 1) > 0 Or InStr(1, LOCATION, "INSERT", 1) > 1 Or InStr(1, LOCATION, "TRACK", 1) > 1 Or InStr(1, LOCATION, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Subs_Location.Focus()
                        Exit Sub
                    End If
                    LOCATION = TrimAll(LOCATION)
                    'check unwanted characters
                    c = 0
                    counter18 = 0
                    For iloop = 1 To Len(LOCATION.ToString)
                        strcurrentchar = Mid(LOCATION, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter18 = 1
                            End If
                        End If
                    Next
                    If counter18 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Subs_Location.Focus()
                        Exit Sub
                    End If
                Else
                    LOCATION = ""
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
                objCommand.CommandText = "INSERT INTO SUBSCRIPTIONS (CAT_NO, ACQ_ID, SUBS_YEAR, VOL_YEAR, ISSUE_YEAR, ISSUE_VOL, S_VOL, S_ISSUE, S_DATE, E_DATE, F_ISSUE_DATE, GRACE_DAYS, COPY, ISSUE_CONTINUE, SUBS_NO, REMARKS, FREQ_CODE, LOCATION, DATE_ADDED, USER_CODE, LIB_CODE,IP) " & _
                                 " VALUES (@CAT_NO, @ACQ_ID, @SUBS_YEAR, @VOL_YEAR, @ISSUE_YEAR, @ISSUE_VOL, @S_VOL, @S_ISSUE, @S_DATE, @E_DATE, @F_ISSUE_DATE, @GRACE_DAYS, @COPY, @ISSUE_CONTINUE, @SUBS_NO, @REMARKS, @FREQ_CODE, @LOCATION, @DATE_ADDED, @USER_CODE, @LIB_CODE, @IP);  " & _
                                 " SELECT SCOPE_IDENTITY()"

                objCommand.Parameters.Add("@CAT_NO", SqlDbType.Int)
                objCommand.Parameters("@CAT_NO").Value = CAT_NO

                objCommand.Parameters.Add("@ACQ_ID", SqlDbType.Int)
                objCommand.Parameters("@ACQ_ID").Value = ACQ_ID

                objCommand.Parameters.Add("@SUBS_YEAR", SqlDbType.Int)
                objCommand.Parameters("@SUBS_YEAR").Value = SUBS_YEAR

                objCommand.Parameters.Add("@VOL_YEAR", SqlDbType.Int)
                If VOL_YEAR = 0 Then
                    objCommand.Parameters("@VOL_YEAR").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@VOL_YEAR").Value = VOL_YEAR
                End If

                objCommand.Parameters.Add("@ISSUE_YEAR", SqlDbType.Int)
                objCommand.Parameters("@ISSUE_YEAR").Value = ISSUE_YEAR

                objCommand.Parameters.Add("@ISSUE_VOL", SqlDbType.Int)
                If ISSUE_VOL = 0 Then
                    objCommand.Parameters("@ISSUE_VOL").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@ISSUE_VOL").Value = ISSUE_VOL
                End If

                objCommand.Parameters.Add("@S_VOL", SqlDbType.Int)
                If S_VOL = 0 Then
                    objCommand.Parameters("@S_VOL").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@S_VOL").Value = S_VOL
                End If

                objCommand.Parameters.Add("@S_ISSUE", SqlDbType.Int)
                If S_ISSUE = 0 Then
                    objCommand.Parameters("@S_ISSUE").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@S_ISSUE").Value = S_ISSUE
                End If

                objCommand.Parameters.Add("@S_DATE", SqlDbType.DateTime)
                objCommand.Parameters("@S_DATE").Value = S_DATE

                objCommand.Parameters.Add("@E_DATE", SqlDbType.DateTime)
                objCommand.Parameters("@E_DATE").Value = E_DATE

                objCommand.Parameters.Add("@F_ISSUE_DATE", SqlDbType.DateTime)
                objCommand.Parameters("@F_ISSUE_DATE").Value = F_ISSUE_DATE

                objCommand.Parameters.Add("@GRACE_DAYS", SqlDbType.Int)
                If GRACE_DAYS = 0 Then
                    objCommand.Parameters("@GRACE_DAYS").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@GRACE_DAYS").Value = GRACE_DAYS
                End If

                objCommand.Parameters.Add("@ISSUE_CONTINUE", SqlDbType.VarChar)
                If ISSUE_CONTINUE = "" Then
                    objCommand.Parameters("@ISSUE_CONTINUE").Value = "N"
                Else
                    objCommand.Parameters("@ISSUE_CONTINUE").Value = ISSUE_CONTINUE
                End If

                objCommand.Parameters.Add("@COPY", SqlDbType.Int)
                objCommand.Parameters("@COPY").Value = COPY

                objCommand.Parameters.Add("@SUBS_NO", SqlDbType.NVarChar)
                If SUBS_NO = "" Then
                    objCommand.Parameters("@SUBS_NO").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@SUBS_NO").Value = SUBS_NO
                End If

                objCommand.Parameters.Add("@REMARKS", SqlDbType.NVarChar)
                If REMARKS = "" Then
                    objCommand.Parameters("@REMARKS").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@REMARKS").Value = REMARKS
                End If

                objCommand.Parameters.Add("@LOCATION", SqlDbType.NVarChar)
                If LOCATION = "" Then
                    objCommand.Parameters("@LOCATION").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@LOCATION").Value = LOCATION
                End If

                objCommand.Parameters.Add("@FREQ_CODE", SqlDbType.NVarChar)
                objCommand.Parameters("@FREQ_CODE").Value = FREQ_CODE

                If LIB_CODE = "" Then LIB_CODE = System.DBNull.Value
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

                Dim dr As SqlDataReader
                dr = objCommand.ExecuteReader()
                If dr.Read Then
                    intValue = dr.GetValue(0)
                End If
                dr.Close()

                thisTransaction.Commit()
                SqlConn.Close()

                Label15.Text = "Record Added Successfully! " & "Acq ID: " & intValue.ToString
                Label6.Text = ""

                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Record Added Successfully!');", True)
                SUBS_YEAR = DDL_SubscriptionYears.SelectedValue
                DDL_SubscriptionYears.ClearSelection()
                DDL_SubscriptionYears.SelectedValue = SUBS_YEAR
                DDL_SubscriptionYears_SelectedIndexChanged(sender, e)

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

        Subs_Save_Bttn.Visible = True
        Subs_GetData_Bttn.Visible = True
        Subs_Update_Bttn.Visible = False
        Subs_Delete_Bttn.Visible = False
        Label15.Text = "Enter Data and Press SAVE Button to Save New Record!"
        Label6.Text = ""
    End Sub
    Protected Sub App_Cancel_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Subs_Cancel_Bttn.Click
        Subs_Save_Bttn.Visible = True
        Subs_GetData_Bttn.Visible = True
        Subs_Update_Bttn.Visible = False
        Label15.Text = "Enter Data and Press SAVE Button to save the record.."
        Label6.Text = ""
        ClearFields()
    End Sub
    'update record
    Protected Sub Subs_Update_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Subs_Update_Bttn.Click
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
                    Label6.Text = "Plz Select Title from Drop-Down!"
                    Label15.Text = ""
                    DDL_Titles.Focus()
                    Exit Sub
                End If

                Dim SUBS_ID As Integer = Nothing
                If Me.Label34.Text <> "" Then
                    SUBS_ID = Trim(Label34.Text)
                    SUBS_ID = RemoveQuotes(SUBS_ID)
                    If SUBS_ID.ToString.Length > 5 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_Titles.Focus()
                        Exit Sub
                    End If
                    SUBS_ID = " " & SUBS_ID & " "
                    If InStr(1, SUBS_ID, "CREATE", 1) > 0 Or InStr(1, SUBS_ID, "DELETE", 1) > 0 Or InStr(1, SUBS_ID, "DROP", 1) > 0 Or InStr(1, SUBS_ID, "INSERT", 1) > 1 Or InStr(1, SUBS_ID, "TRACK", 1) > 1 Or InStr(1, SUBS_ID, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_Titles.Focus()
                        Exit Sub
                    End If
                    SUBS_ID = TrimX(SUBS_ID)
                    'check unwanted characters
                    c = 0
                    counter1 = 0
                    For iloop = 1 To Len(SUBS_ID.ToString)
                        strcurrentchar = Mid(SUBS_ID, iloop, 1)
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
                        DDL_Titles.Focus()
                        Exit Sub
                    End If
                Else
                    Label6.Text = "Error: Plz Select Title from Drop-Down !"
                    Label15.Text = ""
                    DDL_Titles.Focus()
                    Exit Sub
                End If

                'validation for VOL_YEAR
                Dim VOL_YEAR As Integer = Nothing
                If Me.txt_Subs_VolPerYear.Text <> "" Then
                    VOL_YEAR = Convert.ToInt16(TrimX(txt_Subs_VolPerYear.Text))
                    VOL_YEAR = RemoveQuotes(VOL_YEAR)
                    If Not IsNumeric(VOL_YEAR) = True Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_VolPerYear.Focus()
                        Exit Sub
                    End If
                    If Len(VOL_YEAR) > 5 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_VolPerYear.Focus()
                        Exit Sub
                    End If
                    VOL_YEAR = " " & VOL_YEAR & " "
                    If InStr(1, VOL_YEAR, "CREATE", 1) > 0 Or InStr(1, VOL_YEAR, "DELETE", 1) > 0 Or InStr(1, VOL_YEAR, "DROP", 1) > 0 Or InStr(1, VOL_YEAR, "INSERT", 1) > 1 Or InStr(1, VOL_YEAR, "TRACK", 1) > 1 Or InStr(1, VOL_YEAR, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_VolPerYear.Focus()
                        Exit Sub
                    End If
                    VOL_YEAR = TrimX(VOL_YEAR)
                    'check unwanted characters
                    c = 0
                    counter4 = 0
                    For iloop = 1 To Len(VOL_YEAR.ToString)
                        strcurrentchar = Mid(VOL_YEAR, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter4 = 1
                            End If
                        End If
                    Next
                    If counter4 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_VolPerYear.Focus()
                        Exit Sub
                    End If
                Else
                    VOL_YEAR = Nothing
                End If

                'validation for ISSUE_YEAR
                Dim ISSUE_YEAR As Integer = Nothing
                If Me.txt_Subs_IssuesPerYear.Text <> "" Then
                    ISSUE_YEAR = Convert.ToInt16(TrimX(txt_Subs_IssuesPerYear.Text))
                    ISSUE_YEAR = RemoveQuotes(ISSUE_YEAR)
                    If Not IsNumeric(ISSUE_YEAR) = True Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_IssuesPerYear.Focus()
                        Exit Sub
                    End If
                    If Len(ISSUE_YEAR) > 5 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_IssuesPerYear.Focus()
                        Exit Sub
                    End If
                    ISSUE_YEAR = " " & ISSUE_YEAR & " "
                    If InStr(1, ISSUE_YEAR, "CREATE", 1) > 0 Or InStr(1, ISSUE_YEAR, "DELETE", 1) > 0 Or InStr(1, ISSUE_YEAR, "DROP", 1) > 0 Or InStr(1, ISSUE_YEAR, "INSERT", 1) > 1 Or InStr(1, ISSUE_YEAR, "TRACK", 1) > 1 Or InStr(1, ISSUE_YEAR, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_IssuesPerYear.Focus()
                        Exit Sub
                    End If
                    ISSUE_YEAR = TrimX(ISSUE_YEAR)
                    'check unwanted characters
                    c = 0
                    counter5 = 0
                    For iloop = 1 To Len(ISSUE_YEAR.ToString)
                        strcurrentchar = Mid(ISSUE_YEAR, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter5 = 1
                            End If
                        End If
                    Next
                    If counter5 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_IssuesPerYear.Focus()
                        Exit Sub
                    End If
                Else
                    Label6.Text = "Error: Input is not Valid !"
                    Label15.Text = ""
                    txt_Subs_IssuesPerYear.Focus()
                    Exit Sub
                End If

                'validation for ISSUE_VOL
                Dim ISSUE_VOL As Integer = Nothing
                If Me.txt_Subs_IssuesPerVol.Text <> "" Then
                    ISSUE_VOL = Convert.ToInt16(TrimX(txt_Subs_IssuesPerVol.Text))
                    ISSUE_VOL = RemoveQuotes(ISSUE_VOL)
                    If Not IsNumeric(ISSUE_VOL) = True Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_IssuesPerVol.Focus()
                        Exit Sub
                    End If
                    If Len(ISSUE_VOL) > 5 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_IssuesPerVol.Focus()
                        Exit Sub
                    End If
                    ISSUE_VOL = " " & ISSUE_VOL & " "
                    If InStr(1, ISSUE_VOL, "CREATE", 1) > 0 Or InStr(1, ISSUE_VOL, "DELETE", 1) > 0 Or InStr(1, ISSUE_VOL, "DROP", 1) > 0 Or InStr(1, ISSUE_VOL, "INSERT", 1) > 1 Or InStr(1, ISSUE_VOL, "TRACK", 1) > 1 Or InStr(1, ISSUE_VOL, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_IssuesPerVol.Focus()
                        Exit Sub
                    End If
                    ISSUE_VOL = TrimX(ISSUE_VOL)
                    'check unwanted characters
                    c = 0
                    counter6 = 0
                    For iloop = 1 To Len(ISSUE_VOL.ToString)
                        strcurrentchar = Mid(ISSUE_VOL, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter6 = 1
                            End If
                        End If
                    Next
                    If counter6 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_IssuesPerVol.Focus()
                        Exit Sub
                    End If
                Else
                    ISSUE_VOL = Nothing
                End If

                'validation for S_VOL
                Dim S_VOL As Integer = Nothing
                If Me.txt_Subs_StartVolNo.Text <> "" Then
                    S_VOL = Convert.ToInt16(TrimX(txt_Subs_StartVolNo.Text))
                    S_VOL = RemoveQuotes(S_VOL)
                    If Not IsNumeric(S_VOL) = True Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_StartVolNo.Focus()
                        Exit Sub
                    End If
                    If Len(S_VOL) > 5 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_StartVolNo.Focus()
                        Exit Sub
                    End If
                    S_VOL = " " & S_VOL & " "
                    If InStr(1, S_VOL, "CREATE", 1) > 0 Or InStr(1, S_VOL, "DELETE", 1) > 0 Or InStr(1, S_VOL, "DROP", 1) > 0 Or InStr(1, S_VOL, "INSERT", 1) > 1 Or InStr(1, S_VOL, "TRACK", 1) > 1 Or InStr(1, S_VOL, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_StartVolNo.Focus()
                        Exit Sub
                    End If
                    S_VOL = TrimX(S_VOL)
                    'check unwanted characters
                    c = 0
                    counter7 = 0
                    For iloop = 1 To Len(S_VOL.ToString)
                        strcurrentchar = Mid(S_VOL, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter7 = 1
                            End If
                        End If
                    Next
                    If counter7 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_StartVolNo.Focus()
                        Exit Sub
                    End If
                Else
                    S_VOL = Nothing
                End If

                'validation for S_ISSUE
                Dim S_ISSUE As Integer = Nothing
                If Me.txt_Subs_StartIssueNo.Text <> "" Then
                    S_ISSUE = Convert.ToInt16(TrimX(txt_Subs_StartIssueNo.Text))
                    S_ISSUE = RemoveQuotes(S_ISSUE)
                    If Not IsNumeric(S_ISSUE) = True Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_StartIssueNo.Focus()
                        Exit Sub
                    End If
                    If Len(S_ISSUE) > 5 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_StartIssueNo.Focus()
                        Exit Sub
                    End If
                    S_ISSUE = " " & S_ISSUE & " "
                    If InStr(1, S_ISSUE, "CREATE", 1) > 0 Or InStr(1, S_ISSUE, "DELETE", 1) > 0 Or InStr(1, S_ISSUE, "DROP", 1) > 0 Or InStr(1, S_ISSUE, "INSERT", 1) > 1 Or InStr(1, S_ISSUE, "TRACK", 1) > 1 Or InStr(1, S_ISSUE, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_StartIssueNo.Focus()
                        Exit Sub
                    End If
                    S_ISSUE = TrimX(S_ISSUE)
                    'check unwanted characters
                    c = 0
                    counter8 = 0
                    For iloop = 1 To Len(S_ISSUE.ToString)
                        strcurrentchar = Mid(S_ISSUE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter8 = 1
                            End If
                        End If
                    Next
                    If counter8 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_StartIssueNo.Focus()
                        Exit Sub
                    End If
                Else
                    S_ISSUE = Nothing
                End If

                'search start date
                Dim S_DATE As Object = Nothing
                If txt_Subs_SDate.Text <> "" Then
                    S_DATE = TrimX(txt_Subs_SDate.Text)
                    If S_DATE.ToString.Length <> 10 Then
                        Label6.Text = " Input is not Valid..."
                        Me.txt_Subs_SDate.Focus()
                        Exit Sub
                    End If
                    S_DATE = " " & S_DATE & " "
                    If InStr(1, S_DATE, "CREATE", 1) > 0 Or InStr(1, S_DATE, "DELETE", 1) > 0 Or InStr(1, S_DATE, "DROP", 1) > 0 Or InStr(1, S_DATE, "INSERT", 1) > 1 Or InStr(1, S_DATE, "TRACK", 1) > 1 Or InStr(1, S_DATE, "TRACE", 1) > 1 Then
                        Label6.Text = "  Input is not Valid... "
                        Me.txt_Subs_SDate.Focus()
                        Exit Sub
                    End If
                    S_DATE = TrimX(S_DATE)
                    'check unwanted characters
                    c = 0
                    counter9 = 0
                    For iloop = 1 To Len(S_DATE)
                        strcurrentchar = Mid(S_DATE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter9 = 1
                            End If
                        End If
                    Next
                    If counter9 = 1 Then
                        Label6.Text = "data is not Valid... "
                        Me.txt_Subs_SDate.Focus()
                        Exit Sub
                    End If
                    S_DATE = Convert.ToDateTime(S_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us
                   
                Else
                    Label6.Text = "Plz Enter Subscription Start Date!"
                    Me.txt_Subs_SDate.Focus()
                    Exit Sub
                End If

                'search End date
                Dim E_DATE As Object = Nothing
                If txt_Subs_EDate.Text <> "" Then
                    E_DATE = TrimX(txt_Subs_EDate.Text)
                    If Len(E_DATE) <> 10 Then
                        Label6.Text = " Input is not Valid..."
                        Me.txt_Subs_EDate.Focus()
                        Exit Sub
                    End If
                    E_DATE = Convert.ToDateTime(E_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                    E_DATE = " " & E_DATE & " "
                    If InStr(1, E_DATE, "CREATE", 1) > 0 Or InStr(1, E_DATE, "DELETE", 1) > 0 Or InStr(1, E_DATE, "DROP", 1) > 0 Or InStr(1, E_DATE, "INSERT", 1) > 1 Or InStr(1, E_DATE, "TRACK", 1) > 1 Or InStr(1, E_DATE, "TRACE", 1) > 1 Then
                        Label6.Text = "  Input is not Valid... "
                        Me.txt_Subs_EDate.Focus()
                        Exit Sub
                    End If
                    E_DATE = TrimX(E_DATE)
                    'check unwanted characters
                    c = 0
                    counter10 = 0
                    For iloop = 1 To Len(E_DATE)
                        strcurrentchar = Mid(E_DATE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter10 = 1
                            End If
                        End If
                    Next
                    If counter10 = 1 Then
                        Label6.Text = "data is not Valid... "
                        Me.txt_Subs_EDate.Focus()
                        Exit Sub
                    End If
                Else
                    Label6.Text = "Plz Enter Subscription End Date!"
                    Me.txt_Subs_EDate.Focus()
                    Exit Sub
                End If

                'search first issue  date
                Dim F_ISSUE_DATE As Object = Nothing
                If txt_Subs_FisrtIssueDate.Text <> "" Then
                    F_ISSUE_DATE = TrimX(txt_Subs_FisrtIssueDate.Text)
                    F_ISSUE_DATE = Convert.ToDateTime(F_ISSUE_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                    If Len(F_ISSUE_DATE) > 12 Then
                        Label6.Text = " Input is not Valid..."
                        Me.txt_Subs_FisrtIssueDate.Focus()
                        Exit Sub
                    End If
                    F_ISSUE_DATE = " " & F_ISSUE_DATE & " "
                    If InStr(1, F_ISSUE_DATE, "CREATE", 1) > 0 Or InStr(1, F_ISSUE_DATE, "DELETE", 1) > 0 Or InStr(1, F_ISSUE_DATE, "DROP", 1) > 0 Or InStr(1, F_ISSUE_DATE, "INSERT", 1) > 1 Or InStr(1, F_ISSUE_DATE, "TRACK", 1) > 1 Or InStr(1, F_ISSUE_DATE, "TRACE", 1) > 1 Then
                        Label6.Text = "  Input is not Valid... "
                        Me.txt_Subs_FisrtIssueDate.Focus()
                        Exit Sub
                    End If
                    F_ISSUE_DATE = TrimX(F_ISSUE_DATE)
                    'check unwanted characters
                    c = 0
                    counter11 = 0
                    For iloop = 1 To Len(F_ISSUE_DATE)
                        strcurrentchar = Mid(F_ISSUE_DATE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter11 = 1
                            End If
                        End If
                    Next
                    If counter11 = 1 Then
                        Label6.Text = "data is not Valid... "
                        Me.txt_Subs_FisrtIssueDate.Focus()
                        Exit Sub
                    End If
                Else
                    Label6.Text = "Plz Enter Date of First Issue!"
                    Me.txt_Subs_FisrtIssueDate.Focus()
                    Exit Sub
                End If

                'validation for GRACE_DAYS
                Dim GRACE_DAYS As Integer = Nothing
                If Me.txt_Subs_GraceDays.Text <> "" Then
                    GRACE_DAYS = Convert.ToInt16(TrimX(txt_Subs_GraceDays.Text))
                    GRACE_DAYS = RemoveQuotes(GRACE_DAYS)
                    If Not IsNumeric(GRACE_DAYS) = True Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_GraceDays.Focus()
                        Exit Sub
                    End If
                    If Len(GRACE_DAYS) > 5 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_GraceDays.Focus()
                        Exit Sub
                    End If
                    GRACE_DAYS = " " & GRACE_DAYS & " "
                    If InStr(1, GRACE_DAYS, "CREATE", 1) > 0 Or InStr(1, GRACE_DAYS, "DELETE", 1) > 0 Or InStr(1, GRACE_DAYS, "DROP", 1) > 0 Or InStr(1, GRACE_DAYS, "INSERT", 1) > 1 Or InStr(1, GRACE_DAYS, "TRACK", 1) > 1 Or InStr(1, GRACE_DAYS, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_GraceDays.Focus()
                        Exit Sub
                    End If
                    GRACE_DAYS = TrimX(GRACE_DAYS)
                    'check unwanted characters
                    c = 0
                    counter12 = 0
                    For iloop = 1 To Len(GRACE_DAYS.ToString)
                        strcurrentchar = Mid(GRACE_DAYS, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter12 = 1
                            End If
                        End If
                    Next
                    If counter12 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_GraceDays.Focus()
                        Exit Sub
                    End If
                Else
                    GRACE_DAYS = Nothing
                End If

                'validation for ISSUE_CONTINUE
                Dim ISSUE_CONTINUE As Object = Nothing
                If DDL_Continue.Text <> "" Then
                    ISSUE_CONTINUE = DDL_Continue.SelectedValue
                    ISSUE_CONTINUE = RemoveQuotes(ISSUE_CONTINUE)
                    If ISSUE_CONTINUE.Length > 2 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_Continue.Focus()
                        Exit Sub
                    End If
                    ISSUE_CONTINUE = " " & ISSUE_CONTINUE & " "
                    If InStr(1, ISSUE_CONTINUE, "CREATE", 1) > 0 Or InStr(1, ISSUE_CONTINUE, "DELETE", 1) > 0 Or InStr(1, ISSUE_CONTINUE, "DROP", 1) > 0 Or InStr(1, ISSUE_CONTINUE, "INSERT", 1) > 1 Or InStr(1, ISSUE_CONTINUE, "TRACK", 1) > 1 Or InStr(1, ISSUE_CONTINUE, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_Continue.Focus()
                        Exit Sub
                    End If
                    ISSUE_CONTINUE = TrimX(ISSUE_CONTINUE)
                    'check unwanted characters
                    c = 0
                    counter14 = 0
                    For iloop = 1 To Len(ISSUE_CONTINUE)
                        strcurrentchar = Mid(ISSUE_CONTINUE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter14 = 1
                            End If
                        End If
                    Next
                    If counter14 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_Continue.Focus()
                        Exit Sub
                    End If
                Else
                    ISSUE_CONTINUE = "N"
                End If

                'validation for SUBS_NO
                Dim SUBS_NO As Object = Nothing
                If txt_Subs_SubsNo.Text <> "" Then
                    SUBS_NO = txt_Subs_SubsNo.Text
                    SUBS_NO = RemoveQuotes(SUBS_NO)
                    If SUBS_NO.Length > 100 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_SubsNo.Focus()
                        Exit Sub
                    End If
                    SUBS_NO = " " & SUBS_NO & " "
                    If InStr(1, SUBS_NO, "CREATE", 1) > 0 Or InStr(1, SUBS_NO, "DELETE", 1) > 0 Or InStr(1, SUBS_NO, "DROP", 1) > 0 Or InStr(1, SUBS_NO, "INSERT", 1) > 1 Or InStr(1, SUBS_NO, "TRACK", 1) > 1 Or InStr(1, SUBS_NO, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_SubsNo.Focus()
                        Exit Sub
                    End If
                    SUBS_NO = TrimX(SUBS_NO)
                    'check unwanted characters
                    c = 0
                    counter15 = 0
                    For iloop = 1 To Len(SUBS_NO)
                        strcurrentchar = Mid(SUBS_NO, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter15 = 1
                            End If
                        End If
                    Next
                    If counter15 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Subs_SubsNo.Focus()
                        Exit Sub
                    End If
                Else
                    SUBS_NO = ""
                End If

                'Server validation for  : txt_Cat_Remarks
                Dim REMARKS As Object = Nothing
                If txt_Subs_Remarks.Text <> "" Then
                    REMARKS = TrimAll(txt_Subs_Remarks.Text)
                    If REMARKS.Length > 250 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Subs_Remarks.Focus()
                        Exit Sub
                    End If

                    REMARKS = " " & REMARKS & " "
                    If InStr(1, REMARKS, "CREATE", 1) > 0 Or InStr(1, REMARKS, "DELETE", 1) > 0 Or InStr(1, REMARKS, "DROP", 1) > 0 Or InStr(1, REMARKS, "INSERT", 1) > 1 Or InStr(1, REMARKS, "TRACK", 1) > 1 Or InStr(1, REMARKS, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Subs_Remarks.Focus()
                        Exit Sub
                    End If
                    REMARKS = TrimAll(REMARKS)
                    'check unwanted characters
                    c = 0
                    counter16 = 0
                    For iloop = 1 To Len(REMARKS.ToString)
                        strcurrentchar = Mid(REMARKS, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter16 = 1
                            End If
                        End If
                    Next
                    If counter16 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Subs_Remarks.Focus()
                        Exit Sub
                    End If
                Else
                    REMARKS = ""
                End If

                'Server validation for  : FREQ_CODE
                Dim FREQ_CODE As Object = Nothing
                If DDL_Frequencies.Text <> "" Then
                    FREQ_CODE = TrimAll(DDL_Frequencies.Text)
                    If FREQ_CODE.Length > 3 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        DDL_Frequencies.Focus()
                        Exit Sub
                    End If

                    FREQ_CODE = " " & FREQ_CODE & " "
                    If InStr(1, FREQ_CODE, "CREATE", 1) > 0 Or InStr(1, FREQ_CODE, "DELETE", 1) > 0 Or InStr(1, FREQ_CODE, "DROP", 1) > 0 Or InStr(1, FREQ_CODE, "INSERT", 1) > 1 Or InStr(1, FREQ_CODE, "TRACK", 1) > 1 Or InStr(1, FREQ_CODE, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.DDL_Frequencies.Focus()
                        Exit Sub
                    End If
                    FREQ_CODE = TrimX(FREQ_CODE)
                    'check unwanted characters
                    c = 0
                    counter17 = 0
                    For iloop = 1 To Len(FREQ_CODE.ToString)
                        strcurrentchar = Mid(FREQ_CODE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter17 = 1
                            End If
                        End If
                    Next
                    If counter17 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.DDL_Frequencies.Focus()
                        Exit Sub
                    End If
                Else
                    Label6.Text = " Do Not use un-wanted Characters... "
                    Label15.Text = ""
                    Me.DDL_Frequencies.Focus()
                    Exit Sub
                End If

                'Server validation for  : LOCATION
                Dim LOCATION As Object = Nothing
                If txt_Subs_Location.Text <> "" Then
                    LOCATION = TrimAll(txt_Subs_Location.Text)
                    If LOCATION.Length > 50 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Subs_Location.Focus()
                        Exit Sub
                    End If

                    LOCATION = " " & LOCATION & " "
                    If InStr(1, LOCATION, "CREATE", 1) > 0 Or InStr(1, LOCATION, "DELETE", 1) > 0 Or InStr(1, LOCATION, "DROP", 1) > 0 Or InStr(1, LOCATION, "INSERT", 1) > 1 Or InStr(1, LOCATION, "TRACK", 1) > 1 Or InStr(1, LOCATION, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Subs_Location.Focus()
                        Exit Sub
                    End If
                    LOCATION = TrimAll(LOCATION)
                    'check unwanted characters
                    c = 0
                    counter18 = 0
                    For iloop = 1 To Len(LOCATION.ToString)
                        strcurrentchar = Mid(LOCATION, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter18 = 1
                            End If
                        End If
                    Next
                    If counter18 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Subs_Location.Focus()
                        Exit Sub
                    End If
                Else
                    LOCATION = ""
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


                Dim IP As Object = Nothing
                IP = Request.UserHostAddress.Trim

                'UPDATE THE Record  
                If Label34.Text <> "" Then
                    SQL = "SELECT * FROM SUBSCRIPTIONS WHERE (SUBS_ID='" & Trim(SUBS_ID) & "')"
                    SqlConn.Open()
                    da = New SqlDataAdapter(SQL, SqlConn)
                    cb = New SqlCommandBuilder(da)
                    da.Fill(ds, "SUBS")
                    If ds.Tables("SUBS").Rows.Count <> 0 Then
                        If Not VOL_YEAR = Nothing Then
                            ds.Tables("SUBS").Rows(0)("VOL_YEAR") = VOL_YEAR
                        Else
                            ds.Tables("SUBS").Rows(0)("VOL_YEAR") = System.DBNull.Value
                        End If

                        If Not ISSUE_YEAR = Nothing Then
                            ds.Tables("SUBS").Rows(0)("ISSUE_YEAR") = ISSUE_YEAR
                        Else
                            ds.Tables("SUBS").Rows(0)("ISSUE_YEAR") = System.DBNull.Value
                        End If

                        If Not ISSUE_VOL = Nothing Then
                            ds.Tables("SUBS").Rows(0)("ISSUE_VOL") = ISSUE_VOL
                        Else
                            ds.Tables("SUBS").Rows(0)("ISSUE_VOL") = System.DBNull.Value
                        End If

                        If Not S_VOL = Nothing Then
                            ds.Tables("SUBS").Rows(0)("S_VOL") = S_VOL
                        Else
                            ds.Tables("SUBS").Rows(0)("S_VOL") = System.DBNull.Value
                        End If

                        If Not S_ISSUE = Nothing Then
                            ds.Tables("SUBS").Rows(0)("S_ISSUE") = S_ISSUE
                        Else
                            ds.Tables("SUBS").Rows(0)("S_ISSUE") = System.DBNull.Value
                        End If

                        If Not S_DATE = Nothing Then
                            ds.Tables("SUBS").Rows(0)("S_DATE") = S_DATE
                        Else
                            ds.Tables("SUBS").Rows(0)("S_DATE") = System.DBNull.Value
                        End If

                        If Not E_DATE = Nothing Then
                            ds.Tables("SUBS").Rows(0)("E_DATE") = E_DATE
                        Else
                            ds.Tables("SUBS").Rows(0)("E_DATE") = System.DBNull.Value
                        End If

                        If Not F_ISSUE_DATE = Nothing Then
                            ds.Tables("SUBS").Rows(0)("F_ISSUE_DATE") = F_ISSUE_DATE
                        Else
                            ds.Tables("SUBS").Rows(0)("F_ISSUE_DATE") = System.DBNull.Value
                        End If

                        If Not GRACE_DAYS = Nothing Then
                            ds.Tables("SUBS").Rows(0)("GRACE_DAYS") = GRACE_DAYS
                        Else
                            ds.Tables("SUBS").Rows(0)("GRACE_DAYS") = System.DBNull.Value
                        End If

                        If Not ISSUE_CONTINUE = Nothing Then
                            ds.Tables("SUBS").Rows(0)("ISSUE_CONTINUE") = ISSUE_CONTINUE
                        Else
                            ds.Tables("SUBS").Rows(0)("ISSUE_CONTINUE") = System.DBNull.Value
                        End If

                        If Not SUBS_NO = Nothing Then
                            ds.Tables("SUBS").Rows(0)("SUBS_NO") = SUBS_NO
                        Else
                            ds.Tables("SUBS").Rows(0)("SUBS_NO") = System.DBNull.Value
                        End If

                        If Not REMARKS = Nothing Then
                            ds.Tables("SUBS").Rows(0)("REMARKS") = REMARKS
                        Else
                            ds.Tables("SUBS").Rows(0)("REMARKS") = System.DBNull.Value
                        End If

                        If Not FREQ_CODE = Nothing Then
                            ds.Tables("SUBS").Rows(0)("FREQ_CODE") = FREQ_CODE
                        Else
                            ds.Tables("SUBS").Rows(0)("FREQ_CODE") = System.DBNull.Value
                        End If

                        If Not LOCATION = Nothing Then
                            ds.Tables("SUBS").Rows(0)("LOCATION") = LOCATION
                        Else
                            ds.Tables("SUBS").Rows(0)("LOCATION") = System.DBNull.Value
                        End If

                        ds.Tables("SUBS").Rows(0)("UPDATED_BY") = UserCode.Trim
                        ds.Tables("SUBS").Rows(0)("DATE_MODIFIED") = Now.Date
                        ds.Tables("SUBS").Rows(0)("IP") = Request.UserHostAddress.Trim

                        thisTransaction = SqlConn.BeginTransaction()
                        da.SelectCommand.Transaction = thisTransaction
                        da.Update(ds, "SUBS")
                        thisTransaction.Commit()
                       
                        Label15.Text = "User Record Updated Successfully"
                        Label6.Text = ""
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Record Updated Successfully!');", True)

                        SqlConn.Close()
                        ClearFields()

                        Dim SUBS_YEAR As Integer = Nothing
                        SUBS_YEAR = DDL_SubscriptionYears.SelectedValue
                        DDL_SubscriptionYears.ClearSelection()
                        DDL_SubscriptionYears.SelectedValue = SUBS_YEAR
                        DDL_SubscriptionYears_SelectedIndexChanged(sender, e)
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
        Catch q As SqlException
            thisTransaction.Rollback()
            Label6.Text = "Error: " & (q.Message())
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try

    End Sub
    'delete record    
    Protected Sub Subs_Delete_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Subs_Delete_Bttn.Click
        Try
            If Label34.Text <> "" Then
                Dim DelSUBS_ID As Integer = Nothing
                DelSUBS_ID = Trim(Label34.Text)

                'check cat record in acq and holdigns table
                Dim str As Object = Nothing
                Dim flag As Object = Nothing
                str = "SELECT ISS_ID FROM LOOSE_ISSUES WHERE (SUBS_ID ='" & Trim(DelSUBS_ID) & "') "
                Dim cmd As New SqlCommand(str, SqlConn)
                If SqlConn.State = 0 Then
                    SqlConn.Open()
                End If
                flag = cmd.ExecuteScalar
                If flag <> Nothing Then
                    Label6.Text = "Alert: You have generated Loose Issues Schedule for this Title, First Delete Loose Issues Schedule of this title from Schedule Manager!"
                    Label15.Text = ""
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Alert: You have generated Loose Issues Schedule for this Title, First Delete Loose Issues Schedule of this title from Schedule Manager!');", True)
                    SqlConn.Close()
                    Exit Sub
                End If
                SqlConn.Close()

               

                'delete Record
                If flag = Nothing Then
                    'delete Subscription record
                    Dim SQL As String = Nothing
                    SQL = "DELETE FROM SUBSCRIPTIONS WHERE (SUBS_ID ='" & Trim(DelSUBS_ID) & "') "
                    SqlConn.Open()
                    Dim objCommand As New SqlCommand(SQL, SqlConn)
                    Dim da As New SqlDataAdapter(objCommand)
                    Dim ds As New DataSet
                    da.Fill(ds)
                    SqlConn.Close()
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Record deleted Successfully!');", True)

                    Label15.Text = "Record deleted Successfully!"
                    Label6.Text = ""
                    ClearFields()
                    Me.Subs_Save_Bttn.Visible = True
                    Subs_GetData_Bttn.Visible = True
                    Me.Subs_Update_Bttn.Visible = False
                    Me.Subs_Delete_Bttn.Visible = False
                End If
            End If
        Catch q As SqlException
            Label6.Text = "Database Error -SAVE: " & (q.Message() & ", " & q.Number & ", " & q.GetType.ToString)
            Label15.Text = ""
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
            Label15.Text = ""
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'get subs data from prev year  
    Protected Sub Subs_GetData_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Subs_GetData_Bttn.Click
        Dim dt As New DataTable
        Try
            Dim myCatNo As Integer = Nothing
            Dim SUBS_YEAR As Integer = Nothing
            If DDL_Titles.Text <> "" Then
                myCatNo = DDL_Titles.SelectedValue

                If DDL_SubscriptionYears.Text <> "" Then
                    SUBS_YEAR = DDL_SubscriptionYears.SelectedValue
                    SUBS_YEAR = SUBS_YEAR - 1

                    Dim SQL As String = Nothing
                    If myCatNo <> 0 And SUBS_YEAR <> 0 Then
                        SQL = "SELECT * FROM SUBSCRIPTIONS WHERE (LIB_CODE = '" & Trim(LibCode) & "') AND (CAT_NO ='" & Trim(DDL_Titles.SelectedValue) & "') AND (SUBS_YEAR = '" & Trim(SUBS_YEAR) & "') "
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

                        If dt.Rows(0).Item("S_DATE").ToString <> "" Then
                            txt_Subs_SDate.Text = Microsoft.VisualBasic.Format(DateAdd(Microsoft.VisualBasic.DateInterval.Year, 1, dt.Rows(0).Item("S_DATE")), "dd/MM/yyyy") 'Format(dt.Rows(0).Item("S_DATE"), "dd/MM/yyyy")
                        Else
                            txt_Subs_SDate.Text = ""
                        End If

                        If dt.Rows(0).Item("E_DATE").ToString <> "" Then
                            txt_Subs_EDate.Text = Microsoft.VisualBasic.Format(DateAdd(Microsoft.VisualBasic.DateInterval.Year, 1, dt.Rows(0).Item("E_DATE")), "dd/MM/yyyy") 'Format(dt.Rows(0).Item("E_DATE"), "dd/MM/yyyy")
                        Else
                            txt_Subs_EDate.Text = ""
                        End If

                        If dt.Rows(0).Item("F_ISSUE_DATE").ToString <> "" Then
                            txt_Subs_FisrtIssueDate.Text = Microsoft.VisualBasic.Format(DateAdd(Microsoft.VisualBasic.DateInterval.Year, 1, dt.Rows(0).Item("F_ISSUE_DATE")), "dd/MM/yyyy") 'Format(dt.Rows(0).Item("F_ISSUE_DATE"), "dd/MM/yyyy")
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
                            txt_Subs_StartVolNo.Text = Convert.ToString(dt.Rows(0).Item("S_VOL") + dt.Rows(0).Item("VOL_YEAR"))
                        Else
                            txt_Subs_StartVolNo.Text = ""
                        End If

                        If dt.Rows(0).Item("ISSUE_CONTINUE").ToString <> "" Then
                            If dt.Rows(0).Item("S_ISSUE").ToString <> "" Then
                                If dt.Rows(0).Item("ISSUE_CONTINUE").ToString = "Y" Or dt.Rows(0).Item("ISSUE_CONTINUE").ToString = "y" Then
                                    txt_Subs_StartIssueNo.Text = dt.Rows(0).Item("S_ISSUE").ToString + dt.Rows(0).Item("ISSUE_YEAR").ToString
                                Else
                                    txt_Subs_StartIssueNo.Text = dt.Rows(0).Item("S_ISSUE").ToString
                                End If
                            Else
                                txt_Subs_StartIssueNo.Text = ""
                            End If
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
                    End If
                End If
            End If
        Catch ex As Exception
            Label14.Text = "Error: " & (ex.Message())
        Finally
            SqlConn.Close()
            DDL_SubscriptionYears.Focus()
        End Try
    End Sub
End Class