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
Public Class Notices
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
                        Label6.Text = "Enter Data and Press SAVE Button to save the record.."
                    End If
                End If
                Dim CreateSUserButton As Object 'System.Web.UI.WebControls.Button
                CreateSUserButton = Master.FindControl("Accordion1").FindControl("LibAdminPane").FindControl("Lib_Notice_Bttn")
                CreateSUserButton.ForeColor = Drawing.Color.Red
                myPaneName = "LibAdminPane" 'paneSelectedIndex = 0
            Else
                Response.Redirect("~/Default.aspx", False)
            End If
            LibCode = Session.Item("LoggedLibcode")
        Catch ex As Exception
            Label6.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    Private Sub Notices_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        txt_Msg_Heading.Focus()
    End Sub
    'save user account
    Protected Sub Save_Click(ByVal sender As Object, ByVal e As EventArgs) Handles bttn_Save.Click
        If IsPostBack = True Then
            'check unwanted characters
            Dim iloop As Integer
            Dim strcurrentchar As Object
            Dim c As Integer
            Dim counter1, counter2, counter3, counter4 As Integer
            Dim thisTransaction As SqlClient.SqlTransaction = Nothing
            Try
                'Server Validation for Full Name
                Dim Heading As String = Nothing
                Heading = TrimAll(txt_Msg_Heading.Text)
                If String.IsNullOrEmpty(Heading) Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Enter Heading of the Message... ');", True)
                    Me.txt_Msg_Heading.Focus()
                    Exit Sub
                End If
                Heading = RemoveQuotes(Heading)
                If Heading.Length > 550 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Message must be of Proper Length.. ');", True)
                    txt_Msg_Heading.Focus()
                    Exit Sub
                End If
                '****************************************************************************************
                'Server Validation for Designation
                Dim Message As String = Nothing
                Message = TrimAll(txt_Msg_Message.Text)
                Message = RemoveQuotes(Message)
                If String.IsNullOrEmpty(Message) Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Enter Message/Notice... ');", True)
                    Me.txt_Msg_Message.Focus()
                    Exit Sub
                End If
                If Not String.IsNullOrEmpty(Message) Then
                    If Message.Length > 3000 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not of proper Length... ');", True)
                        Me.txt_Msg_Message.Focus()
                        Exit Sub
                    End If
                End If
                '****************************************************************************************
                'Server Validation for target
                Dim Target As Object = Nothing
                If DropDownList1_Target.Text <> "" Then
                    Target = TrimAll(Me.DropDownList1_Target.Text)
                    If Len(Target.ToString) > 2 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Plz Select Value from Dropdown... ');", True)
                        Me.DropDownList1_Target.Focus()
                        Exit Sub
                    End If
                    Target = " " & Target & " "
                    If InStr(1, Target, "CREATE", 1) > 0 Or InStr(1, Target, "DELETE", 1) > 0 Or InStr(1, Target, "DROP", 1) > 0 Or InStr(1, Target, "INSERT", 1) > 1 Or InStr(1, Target, "TRACK", 1) > 1 Or InStr(1, Target, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not use Reserve Words... ');", True)
                        Me.DropDownList1_Target.Focus()
                        Exit Sub
                    End If
                    Target = TrimAll(Target)
                    'check unwanted characters
                    c = 0
                    counter1 = 0
                    For iloop = 1 To Len(Target.ToString)
                        strcurrentchar = Mid(Target, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter1 = 1
                            End If
                        End If
                    Next
                    If counter1 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not use un-wanted Characters... ');", True)
                        Me.DropDownList1_Target.Focus()
                        Exit Sub
                    End If
                Else
                    Target = "M"
                End If

                'Server Validation for target
                Dim Show As Object = Nothing
                If DropDownList1_Show.Text <> "" Then
                    Show = TrimAll(Me.DropDownList1_Show.Text)
                    If Len(Show.ToString) > 2 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Plz Select Value from Dropdown... ');", True)
                        Me.DropDownList1_Show.Focus()
                        Exit Sub
                    End If
                    Show = " " & Show & " "
                    If InStr(1, Show, "CREATE", 1) > 0 Or InStr(1, Show, "DELETE", 1) > 0 Or InStr(1, Show, "DROP", 1) > 0 Or InStr(1, Show, "INSERT", 1) > 1 Or InStr(1, Show, "TRACK", 1) > 1 Or InStr(1, Show, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not use Reserve Words... ');", True)
                        Me.DropDownList1_Show.Focus()
                        Exit Sub
                    End If
                    Show = TrimAll(Show)
                    'check unwanted characters
                    c = 0
                    counter2 = 0
                    For iloop = 1 To Len(Show.ToString)
                        strcurrentchar = Mid(Show, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter2 = 1
                            End If
                        End If
                    Next
                    If counter2 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not use un-wanted Characters... ');", True)
                        Me.DropDownList1_Show.Focus()
                        Exit Sub
                    End If
                Else
                    Target = "Y"
                End If
                '****************************************************************************************88
                'remareks
                Dim myRemarks As String = Nothing
                myRemarks = TrimAll(Me.txt_Msg_Remarks.Text)
                If Not String.IsNullOrEmpty(myRemarks) Then
                    myRemarks = RemoveQuotes(myRemarks)
                    If myRemarks.Length > 250 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Msg_Remarks.Focus()
                        Exit Sub
                    End If
                    myRemarks = " " & myRemarks & " "
                    If InStr(1, myRemarks, "CREATE", 1) > 0 Or InStr(1, myRemarks, "DELETE", 1) > 0 Or InStr(1, myRemarks, "DROP", 1) > 0 Or InStr(1, myRemarks, "INSERT", 1) > 1 Or InStr(1, myRemarks, "TRACK", 1) > 1 Or InStr(1, myRemarks, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Msg_Remarks.Focus()
                        Exit Sub
                    End If
                    myRemarks = TrimAll(myRemarks)
                    'check unwanted characters
                    c = 0
                    counter3 = 0
                    For iloop = 1 To Len(myRemarks)
                        strcurrentchar = Mid(myRemarks, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter3 = 1
                            End If
                        End If
                    Next
                    If counter3 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Me.txt_Msg_Remarks.Focus()
                        Exit Sub
                    End If
                Else
                    myRemarks = String.Empty
                End If

                'submitted by
                Dim SubmittedBy As String = Nothing
                SubmittedBy = TrimAll(Me.txt_Msg_Submittedby.Text)
                If Not String.IsNullOrEmpty(SubmittedBy) Then
                    SubmittedBy = RemoveQuotes(SubmittedBy)
                    If SubmittedBy.Length > 250 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Msg_Submittedby.Focus()
                        Exit Sub
                    End If
                    SubmittedBy = " " & SubmittedBy & " "
                    If InStr(1, SubmittedBy, "CREATE", 1) > 0 Or InStr(1, SubmittedBy, "DELETE", 1) > 0 Or InStr(1, SubmittedBy, "DROP", 1) > 0 Or InStr(1, SubmittedBy, "INSERT", 1) > 1 Or InStr(1, SubmittedBy, "TRACK", 1) > 1 Or InStr(1, SubmittedBy, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Msg_Submittedby.Focus()
                        Exit Sub
                    End If
                    SubmittedBy = TrimAll(SubmittedBy)
                    'check unwanted characters
                    c = 0
                    counter4 = 0
                    For iloop = 1 To Len(SubmittedBy)
                        strcurrentchar = Mid(SubmittedBy, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter4 = 1
                            End If
                        End If
                    Next
                    If counter4 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Me.txt_Msg_Submittedby.Focus()
                        Exit Sub
                    End If
                Else
                    SubmittedBy = String.Empty
                End If


                'INSERT THE RECORD IN TO THE DATABASE
                Dim SQL As String
                Dim Cmd As SqlCommand
                Dim da As SqlDataAdapter
                Dim ds As New DataSet
                Dim CB As SqlCommandBuilder
                Dim dtrow As DataRow
                SQL = "SELECT * FROM NOTICES WHERE (NOT_ID = '00')"
                Cmd = New SqlCommand(SQL, SqlConn)
                da = New SqlDataAdapter(Cmd)
                CB = New SqlCommandBuilder(da)
                SqlConn.Open()
                da.Fill(ds, "NOTICES")
                dtrow = ds.Tables("NOTICES").NewRow

                If Not String.IsNullOrEmpty(Heading) Then
                    dtrow("HEADING") = Heading.Trim
                End If

                If Not String.IsNullOrEmpty(Message) Then
                    dtrow("MESSAGE") = Message.Trim
                End If
                If Not String.IsNullOrEmpty(Target) Then
                    dtrow("TARGET") = Target
                End If
                If Not String.IsNullOrEmpty(Show) Then
                    dtrow("SHOW") = Show
                End If
                If Not String.IsNullOrEmpty(SubmittedBy) Then
                    dtrow("SUBMITTED_BY") = SubmittedBy.Trim
                Else
                    dtrow("SUBMITTED_BY") = System.DBNull.Value
                End If
                If Not String.IsNullOrEmpty(myRemarks) Then
                    dtrow("REMARKS") = myRemarks.Trim
                Else
                    dtrow("REMARKS") = System.DBNull.Value
                End If
                dtrow("LIB_CODE") = LibCode
                dtrow("USER_CODE") = Session.Item("LoggedUser")
                dtrow("DATE_ADDED") = Now.Date
                dtrow("IP") = Request.UserHostAddress.Trim
               
                ds.Tables("NOTICES").Rows.Add(dtrow)

                thisTransaction = SqlConn.BeginTransaction()
                da.SelectCommand.Transaction = thisTransaction
                da.Update(ds, "NOTICES")
                thisTransaction.Commit()
                ClearFields()
                ' mailpwd()
                Heading = Nothing
                Message = Nothing
                Target = Nothing
                Show = Nothing
                SubmittedBy = Nothing
                myRemarks = Nothing

                ds.Dispose()
                Label6.Text = "Message Added Successfully!"
                bttn_Save.Visible = True
                bttn_Update.Visible = False
                Search_Bttn_Click(sender, e)
            Catch q As SqlException
                thisTransaction.Rollback()
            Catch ex As Exception
                Label6.Text = "Error: " & (ex.Message())
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
        txt_Msg_Heading.Text = ""
        txt_Msg_Message.Text = ""
        txt_Msg_Remarks.Text = ""
        txt_Msg_Submittedby.Text = ""
        DropDownList1_Show.Text = ""
        DropDownList1_Target.Text = ""
    End Sub
    'Populate the users in grid     'search users
    Public Sub Search_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Search_Bttn.Click
        Dim dtNotices As DataTable = Nothing
        Dim Check As Integer
        Try
            Dim c, counter1, counter2, counter3, counter4 As Integer
            Dim iloop As Integer
            Dim strcurrentchar As Object

            'search string validation
            Dim mySearchString As Object = Nothing
            If txt_Search.Text <> "" Then
                mySearchString = TrimAll(txt_Search.Text)
                mySearchString = RemoveQuotes(mySearchString)
                If mySearchString.Length > 250 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                    Me.txt_Search.Focus()
                    Exit Sub
                End If
                mySearchString = " " & mySearchString & " "
                If InStr(1, mySearchString, "CREATE", 1) > 0 Or InStr(1, mySearchString, "DELETE", 1) > 0 Or InStr(1, mySearchString, "DROP", 1) > 0 Or InStr(1, mySearchString, "INSERT", 1) > 1 Or InStr(1, mySearchString, "TRACK", 1) > 1 Or InStr(1, mySearchString, "TRACE", 1) > 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
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
                        If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter1 = 1
                        End If
                    End If
                Next
                If counter1 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' data is not Valid... ');", True)
                    Me.txt_Search.Focus()
                    Exit Sub
                End If
            Else
                mySearchString = String.Empty
            End If

            'Field Name validation
            Dim myfield As String = Nothing
            If DropDownList2.Text <> "" Then
                myfield = TrimAll(DropDownList2.SelectedValue)
                myfield = RemoveQuotes(myfield)
                If myfield.Length > 50 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                    Me.DropDownList2.Focus()
                    Exit Sub
                End If
                myfield = " " & myfield & " "
                If InStr(1, myfield, "CREATE", 1) > 0 Or InStr(1, myfield, "DELETE", 1) > 0 Or InStr(1, myfield, "DROP", 1) > 0 Or InStr(1, myfield, "INSERT", 1) > 1 Or InStr(1, myfield, "TRACK", 1) > 1 Or InStr(1, myfield, "TRACE", 1) > 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                    Me.DropDownList2.Focus()
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
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' data is not Valid... ');", True)
                    DropDownList2.Focus()
                    Exit Sub
                End If
            Else
                myfield = "HEADING"
            End If

            'target string validation
            Dim myTarget As Object = Nothing
            If DropDownList4.Text <> "" Then
                myTarget = DropDownList4.SelectedValue
                myTarget = RemoveQuotes(myTarget)
                If myTarget.Length > 5 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                    Me.DropDownList4.Focus()
                    Exit Sub
                End If
                myTarget = " " & myTarget & " "
                If InStr(1, myTarget, "CREATE", 1) > 0 Or InStr(1, myTarget, "DELETE", 1) > 0 Or InStr(1, myTarget, "DROP", 1) > 0 Or InStr(1, myTarget, "INSERT", 1) > 1 Or InStr(1, myTarget, "TRACK", 1) > 1 Or InStr(1, myTarget, "TRACE", 1) > 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                    Me.DropDownList4.Focus()
                    Exit Sub
                End If
                myTarget = TrimAll(myTarget)
                'check unwanted characters
                c = 0
                counter1 = 0
                For iloop = 1 To Len(myTarget)
                    strcurrentchar = Mid(myTarget, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter1 = 1
                        End If
                    End If
                Next
                If counter1 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' data is not Valid... ');", True)
                    Me.DropDownList4.Focus()
                    Exit Sub
                End If
            Else
                myTarget = "ALL"
            End If


            'search start date
            Dim DateFrom As Object = Nothing
            If TextBox1.Text <> "" Then
                DateFrom = TrimAll(TextBox1.Text)
                DateFrom = RemoveQuotes(DateFrom)
                DateFrom = Convert.ToDateTime(DateFrom, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us
                'DateFrom = Convert.ToDateTime(DateFrom, System.Globalization.CultureInfo.GetCultureInfo("en-US")).ToString("MM/dd/yyyy") ' convert from us to india
                ' DateFrom = Convert.ToDateTime(DateTime.Now, System.Globalization.CultureInfo.CurrentCulture).ToString("MM/dd/yyyy hh:MM:ss")
                'DateFrom = Convert.ToDateTime(DateFrom, System.Globalization.CultureInfo.CurrentCulture).ToString("MM/dd/yyyy hh:MM:ss")


                If Len(DateFrom) > 12 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                    Me.TextBox1.Focus()
                    Exit Sub
                End If
                DateFrom = " " & DateFrom & " "
                If InStr(1, DateFrom, "CREATE", 1) > 0 Or InStr(1, DateFrom, "DELETE", 1) > 0 Or InStr(1, DateFrom, "DROP", 1) > 0 Or InStr(1, DateFrom, "INSERT", 1) > 1 Or InStr(1, DateFrom, "TRACK", 1) > 1 Or InStr(1, DateFrom, "TRACE", 1) > 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                    Me.TextBox1.Focus()
                    Exit Sub
                End If
                DateFrom = TrimX(DateFrom)
                'check unwanted characters
                c = 0
                counter1 = 0
                For iloop = 1 To Len(DateFrom)
                    strcurrentchar = Mid(DateFrom, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter1 = 1
                        End If
                    End If
                Next
                If counter1 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' data is not Valid... ');", True)
                    Me.TextBox1.Focus()
                    Exit Sub
                End If
            Else
                DateFrom = String.Empty
            End If

            'search end date
            Dim DateTo As Object = Nothing
            If TextBox2.Text <> "" Then
                DateTo = TrimX(TextBox2.Text)
                DateTo = RemoveQuotes(DateTo)
                DateTo = Convert.ToDateTime(DateTo, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                If DateTo.Length > 12 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                    Me.TextBox2.Focus()
                    Exit Sub
                End If
                DateTo = " " & DateTo & " "
                If InStr(1, DateTo, "CREATE", 1) > 0 Or InStr(1, DateTo, "DELETE", 1) > 0 Or InStr(1, DateTo, "DROP", 1) > 0 Or InStr(1, DateTo, "INSERT", 1) > 1 Or InStr(1, DateTo, "TRACK", 1) > 1 Or InStr(1, DateTo, "TRACE", 1) > 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                    Me.TextBox2.Focus()
                    Exit Sub
                End If
                DateTo = TrimX(DateTo)
                'check unwanted characters
                c = 0
                counter2 = 0
                For iloop = 1 To Len(DateTo)
                    strcurrentchar = Mid(DateTo, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter2 = 1
                        End If
                    End If
                Next
                If counter2 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' data is not Valid... ');", True)
                    Me.TextBox2.Focus()
                    Exit Sub
                End If
            Else
                DateTo = String.Empty
            End If


            Dim SQL As String = Nothing
            SQL = "SELECT * FROM NOTICES WHERE  (LIB_CODE='" & Trim(LibCode) & "')  "

            Dim h As Integer
            If txt_Search.Text <> "" Then
                Dim myNewSearchString As Object = Nothing
                myNewSearchString = Split(mySearchString, " ")
                SQL = SQL & " AND (" & myfield & " LIKE N'%" & Trim(myNewSearchString(0)) & "%' "
                For h = 1 To UBound(myNewSearchString)
                    SQL = SQL & " AND " & myfield & " LIKE N'%" & Trim(myNewSearchString(h)) & "%'"
                Next
                SQL = SQL & ")"
            End If

            If Not myTarget = "ALL" Then
                SQL = SQL & " AND (TARGET = '" & Trim(myTarget) & "')  "
            End If

            If DateFrom <> "" And DateTo <> "" Then
                SQL = SQL & " AND (DATE_ADDED >= '" & Trim(DateFrom) & "' and DATE_ADDED <= '" & Trim(DateTo) & "') "
            End If


            SQL = SQL & " ORDER BY DATE_ADDED ASC "

            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            da.Fill(ds)

            dtNotices = ds.Tables(0).Copy
            Dim RecordCount As Long = 0
            If dtNotices.Rows.Count = 0 Then
                Me.Grid_Notices.DataSource = Nothing
                Grid_Notices.DataBind()
                Label1.Text = "Total Record(s): 0 "
                Delete_Bttn.Enabled = False
            Else
                Grid_Notices.Visible = True
                RecordCount = dtNotices.Rows.Count
                Grid_Notices.DataSource = dtNotices
                Grid_Notices.DataBind()
                Label1.Text = "Total Record(s): " & RecordCount
                Delete_Bttn.Enabled = True
            End If
            ViewState("dt") = dtNotices
            UpdatePanel1.Update()
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
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
    Protected Sub Grid1_LibTeam_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles Grid_Notices.PageIndexChanging
        Try
            'rebind datagrid
            Grid_Notices.DataSource = ViewState("dt") 'temp
            Grid_Notices.PageIndex = e.NewPageIndex
            index = e.NewPageIndex * Grid_Notices.PageSize
            Grid_Notices.DataBind()
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
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
    Protected Sub Grid1_LibTeam_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles Grid_Notices.Sorting
        Dim temp As DataTable = CType(ViewState("dt"), DataTable)
        GridViewSortExpression = e.SortExpression
        Grid_Notices.DataSource = temp
        Dim pageIndex As Integer = Grid_Notices.PageIndex
        Grid_Notices.DataSource = SortDataTable(Grid_Notices.DataSource, False)
        Grid_Notices.DataBind()
        Grid_Notices.PageIndex = pageIndex
        UpdatePanel1.Update()
    End Sub
    Protected Sub Grid1_LibTeam_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Grid_Notices.RowDataBound
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
    Private Sub Grid_Notices_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles Grid_Notices.RowCommand
        Dim Command As SqlCommand = Nothing
        Dim drNOTICES As SqlDataReader = Nothing
        Try
            If e.CommandName = "Select" Then
                Dim myRowID, NotID As Integer
                myRowID = e.CommandArgument.ToString()
                NotID = Grid_Notices.DataKeys(myRowID).Value

                If Not String.IsNullOrEmpty(NotID) And NotID <> 0 Then
                    Label7.Text = NotID
                    Dim iloop As Integer
                    Dim strcurrentchar As Object
                    Dim c As Integer
                    Dim counter1 As Integer

                    NotID = TrimX(NotID)
                    NotID = UCase(NotID)
                    NotID = RemoveQuotes(NotID)
                    If Len(NotID).ToString > 10 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Length of Input is not Proper... ');", True)
                        Exit Sub
                    End If
                    NotID = " " & NotID & " "
                    If InStr(1, NotID, " CREATE ", 1) > 0 Or InStr(1, NotID, " DELETE ", 1) > 0 Or InStr(1, NotID, " DROP ", 1) > 0 Or InStr(1, NotID, " INSERT ", 1) > 1 Or InStr(1, NotID, " TRACK ", 1) > 1 Or InStr(1, NotID, " TRACE ", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do not use reserve words... ');", True)
                        Exit Sub
                    End If
                    NotID = TrimX(NotID)

                    'get record details from database
                    Dim SQL As String = Nothing
                    SQL = " SELECT *  FROM NOTICES WHERE (NOT_ID = '" & Trim(NotID) & "') "
                    Command = New SqlCommand(SQL, SqlConn)
                    SqlConn.Open()
                    drNOTICES = Command.ExecuteReader(CommandBehavior.CloseConnection)
                    drNOTICES.Read()
                    If drNOTICES.HasRows = True Then
                        If drNOTICES.Item("HEADING").ToString <> "" Then
                            Me.txt_Msg_Heading.Text = drNOTICES.Item("HEADING").ToString
                        Else
                            txt_Msg_Heading.Text = ""
                        End If
                        If drNOTICES.Item("MESSAGE").ToString <> "" Then
                            Me.txt_Msg_Message.Text = drNOTICES.Item("MESSAGE").ToString
                        Else
                            Me.txt_Msg_Message.Text = ""
                        End If
                        If drNOTICES.Item("SHOW").ToString <> "" Then
                            DropDownList1_Show.SelectedValue = drNOTICES.Item("SHOW").ToString
                        Else
                            DropDownList1_Show.Text = ""
                        End If
                        If drNOTICES.Item("REMARKS").ToString <> "" Then
                            Me.txt_Msg_Remarks.Text = drNOTICES.Item("REMARKS").ToString
                        Else
                            Me.txt_Msg_Remarks.Text = ""
                        End If
                        If drNOTICES.Item("TARGET").ToString <> "" Then
                            DropDownList1_Target.SelectedValue = drNOTICES.Item("TARGET").ToString
                        Else
                            DropDownList1_Target.Text = ""
                        End If
                        If drNOTICES.Item("SUBMITTED_BY").ToString <> "" Then
                            Me.txt_Msg_Submittedby.Text = drNOTICES.Item("SUBMITTED_BY").ToString
                        Else
                            Me.txt_Msg_Submittedby.Text = ""
                        End If

                        bttn_Save.Visible = False
                        bttn_Update.Visible = True
                        Label6.Text = "Press UPDATE Button to save the Changes if any.."
                        drNOTICES.Close()
                        SqlConn.Close()
                    Else
                        Label7.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' No Input is There... ');", True)
                    End If
                Else
                    Label7.Text = ""
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' No Record to Edit... ');", True)
                End If
            End If
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
        Finally

        End Try
    End Sub 'Grid1_ItemCommand
    'update record
    Protected Sub bttn_Update_Click(ByVal sender As Object, ByVal e As EventArgs) Handles bttn_Update.Click
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
                Dim counter1, counter2, counter3, counter4, Counter5, counter6, counter7, counter8, counter9, counter10 As Integer

                'Server Validation for Full Name
                Dim Heading As String = Nothing
                Heading = TrimAll(txt_Msg_Heading.Text)
                If String.IsNullOrEmpty(Heading) Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Enter Heading of the Message... ');", True)
                    Me.txt_Msg_Heading.Focus()
                    Exit Sub
                End If
                Heading = RemoveQuotes(Heading)
                If Heading.Length > 550 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Message must be of Proper Length.. ');", True)
                    txt_Msg_Heading.Focus()
                    Exit Sub
                End If
                '****************************************************************************************
                'Server Validation for Designation
                Dim Message As String = Nothing
                Message = TrimAll(txt_Msg_Message.Text)
                Message = RemoveQuotes(Message)
                If String.IsNullOrEmpty(Message) Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Enter Message/Notice... ');", True)
                    Me.txt_Msg_Message.Focus()
                    Exit Sub
                End If
                If Not String.IsNullOrEmpty(Message) Then
                    If Message.Length > 3000 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not of proper Length... ');", True)
                        Me.txt_Msg_Message.Focus()
                        Exit Sub
                    End If
                End If
                '****************************************************************************************
                'Server Validation for target
                Dim Target As Object = Nothing
                If DropDownList1_Target.Text <> "" Then
                    Target = TrimAll(Me.DropDownList1_Target.Text)
                    If Len(Target.ToString) > 2 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Plz Select Value from Dropdown... ');", True)
                        Me.DropDownList1_Target.Focus()
                        Exit Sub
                    End If
                    Target = " " & Target & " "
                    If InStr(1, Target, "CREATE", 1) > 0 Or InStr(1, Target, "DELETE", 1) > 0 Or InStr(1, Target, "DROP", 1) > 0 Or InStr(1, Target, "INSERT", 1) > 1 Or InStr(1, Target, "TRACK", 1) > 1 Or InStr(1, Target, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not use Reserve Words... ');", True)
                        Me.DropDownList1_Target.Focus()
                        Exit Sub
                    End If
                    Target = TrimAll(Target)
                    'check unwanted characters
                    c = 0
                    counter1 = 0
                    For iloop = 1 To Len(Target.ToString)
                        strcurrentchar = Mid(Target, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter1 = 1
                            End If
                        End If
                    Next
                    If counter1 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not use un-wanted Characters... ');", True)
                        Me.DropDownList1_Target.Focus()
                        Exit Sub
                    End If
                Else
                    Target = "M"
                End If

                'Server Validation for target
                Dim Show As Object = Nothing
                If DropDownList1_Show.Text <> "" Then
                    Show = TrimAll(Me.DropDownList1_Show.Text)
                    If Len(Show.ToString) > 2 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Plz Select Value from Dropdown... ');", True)
                        Me.DropDownList1_Show.Focus()
                        Exit Sub
                    End If
                    Show = " " & Show & " "
                    If InStr(1, Show, "CREATE", 1) > 0 Or InStr(1, Show, "DELETE", 1) > 0 Or InStr(1, Show, "DROP", 1) > 0 Or InStr(1, Show, "INSERT", 1) > 1 Or InStr(1, Show, "TRACK", 1) > 1 Or InStr(1, Show, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not use Reserve Words... ');", True)
                        Me.DropDownList1_Show.Focus()
                        Exit Sub
                    End If
                    Show = TrimAll(Show)
                    'check unwanted characters
                    c = 0
                    counter2 = 0
                    For iloop = 1 To Len(Show.ToString)
                        strcurrentchar = Mid(Show, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter2 = 1
                            End If
                        End If
                    Next
                    If counter2 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not use un-wanted Characters... ');", True)
                        Me.DropDownList1_Show.Focus()
                        Exit Sub
                    End If
                Else
                    Target = "Y"
                End If
                '****************************************************************************************88
                'remareks
                Dim myRemarks As String = Nothing
                myRemarks = TrimAll(Me.txt_Msg_Remarks.Text)
                If Not String.IsNullOrEmpty(myRemarks) Then
                    myRemarks = RemoveQuotes(myRemarks)
                    If myRemarks.Length > 250 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Msg_Remarks.Focus()
                        Exit Sub
                    End If
                    myRemarks = " " & myRemarks & " "
                    If InStr(1, myRemarks, "CREATE", 1) > 0 Or InStr(1, myRemarks, "DELETE", 1) > 0 Or InStr(1, myRemarks, "DROP", 1) > 0 Or InStr(1, myRemarks, "INSERT", 1) > 1 Or InStr(1, myRemarks, "TRACK", 1) > 1 Or InStr(1, myRemarks, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Msg_Remarks.Focus()
                        Exit Sub
                    End If
                    myRemarks = TrimAll(myRemarks)
                    'check unwanted characters
                    c = 0
                    counter3 = 0
                    For iloop = 1 To Len(myRemarks)
                        strcurrentchar = Mid(myRemarks, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter3 = 1
                            End If
                        End If
                    Next
                    If counter3 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Me.txt_Msg_Remarks.Focus()
                        Exit Sub
                    End If
                Else
                    myRemarks = String.Empty
                End If

                'submitted by
                Dim SubmittedBy As String = Nothing
                SubmittedBy = TrimAll(Me.txt_Msg_Submittedby.Text)
                If Not String.IsNullOrEmpty(SubmittedBy) Then
                    SubmittedBy = RemoveQuotes(SubmittedBy)
                    If SubmittedBy.Length > 250 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Msg_Submittedby.Focus()
                        Exit Sub
                    End If
                    SubmittedBy = " " & SubmittedBy & " "
                    If InStr(1, SubmittedBy, "CREATE", 1) > 0 Or InStr(1, SubmittedBy, "DELETE", 1) > 0 Or InStr(1, SubmittedBy, "DROP", 1) > 0 Or InStr(1, SubmittedBy, "INSERT", 1) > 1 Or InStr(1, SubmittedBy, "TRACK", 1) > 1 Or InStr(1, SubmittedBy, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Msg_Submittedby.Focus()
                        Exit Sub
                    End If
                    SubmittedBy = TrimAll(SubmittedBy)
                    'check unwanted characters
                    c = 0
                    counter4 = 0
                    For iloop = 1 To Len(SubmittedBy)
                        strcurrentchar = Mid(SubmittedBy, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter4 = 1
                            End If
                        End If
                    Next
                    If counter4 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Me.txt_Msg_Submittedby.Focus()
                        Exit Sub
                    End If
                Else
                    SubmittedBy = String.Empty
                End If

                '****************************************************************************************************
                'UPDATE THE LIBRARY PROFILE  
                If Label7.Text <> "" Then
                    SQL = "SELECT * FROM NOTICES WHERE (NOT_ID='" & Trim(Label7.Text) & "')"
                    SqlConn.Open()
                    da = New SqlDataAdapter(SQL, SqlConn)
                    cb = New SqlCommandBuilder(da)
                    da.Fill(ds, "NOTICES")
                    If ds.Tables("NOTICES").Rows.Count <> 0 Then
                        If Not String.IsNullOrEmpty(Heading) Then
                            ds.Tables("NOTICES").Rows(0)("HEADING") = Heading.Trim
                        Else
                            ds.Tables("NOTICES").Rows(0)("HEADING") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(Message) Then
                            ds.Tables("NOTICES").Rows(0)("MESSAGE") = Message.Trim
                        Else
                            ds.Tables("NOTICES").Rows(0)("MESSAGE") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(Show) Then
                            ds.Tables("NOTICES").Rows(0)("SHOW") = Show
                        Else
                            ds.Tables("NOTICES").Rows(0)("SHOW") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(myRemarks) Then
                            ds.Tables("NOTICES").Rows(0)("REMARKS") = myRemarks.Trim
                        Else
                            ds.Tables("NOTICES").Rows(0)("REMARKS") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(Target) Then
                            ds.Tables("NOTICES").Rows(0)("TARGET") = Target.Trim
                        Else
                            ds.Tables("NOTICES").Rows(0)("TARGET") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(SubmittedBy) Then
                            ds.Tables("NOTICES").Rows(0)("SUBMITTED_BY") = SubmittedBy.ToString.Trim
                        Else
                            ds.Tables("NOTICES").Rows(0)("SUBMITTED_BY") = System.DBNull.Value
                        End If
                        ds.Tables("NOTICES").Rows(0)("UPDATED_BY") = UserCode.Trim
                        ds.Tables("NOTICES").Rows(0)("DATE_MODIFIED") = Now.Date
                        ds.Tables("NOTICES").Rows(0)("IP") = Request.UserHostAddress.Trim

                        thisTransaction = SqlConn.BeginTransaction()
                        da.SelectCommand.Transaction = thisTransaction
                        da.Update(ds, "NOTICES")
                        thisTransaction.Commit()
                        Heading = Nothing
                        Message = Nothing
                        Show = Nothing
                        myRemarks = Nothing
                        Target = Nothing
                        SubmittedBy = Nothing
                        Label6.Visible = True
                        Label6.Text = "User Record Updated Successfully"
                        ClearFields()
                    Else
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('User Profile Update  - Please Contact System Administrator... ');", True)
                        Exit Sub
                    End If
                End If
            Else
                'record not selected
                ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Record Not Selected... ');", True)
                Exit Sub
            End If
            SqlConn.Close()
            Search_Bttn_Click(sender, e)
            ClearFields()
        Catch q As SqlException
            thisTransaction.Rollback()
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
            Label7.Text = ""
            ClearFields()
            Me.bttn_Save.Visible = True
            Me.bttn_Update.Visible = False
        End Try
    End Sub
    'delete selected rows
    Protected Sub Delete_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Delete_Bttn.Click
        Try
            For Each row As GridViewRow In Grid_Notices.Rows
                Dim cb As CheckBox = DirectCast(row.FindControl("cbd"), CheckBox)
                If cb IsNot Nothing AndAlso cb.Checked = True Then
                    Dim NotID As Integer = Convert.ToInt32(Grid_Notices.DataKeys(row.RowIndex).Value)
                    'get cat record
                    Dim SQL As String = Nothing
                    SQL = "DELETE FROM NOTICES WHERE (NOT_ID ='" & Trim(NotID) & "') "
                    Dim objCommand As New SqlCommand(SQL, SqlConn)
                    Dim da As New SqlDataAdapter(objCommand)
                    Dim ds As New DataSet
                    da.Fill(ds)
                End If
            Next
            Dim s As Object = Nothing
            Dim e1 As EventArgs = Nothing
            Search_Bttn_Click(s, e1)
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
End Class