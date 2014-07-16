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
Public Class DatabaseLogs
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
                        Grid1_Logs.DataSource = Nothing
                        Grid1_Logs.Dispose()
                        GetUsers()
                        Delete_Bttn.Enabled = False
                    End If

                    Dim CreateSUserButton As Object 'System.Web.UI.WebControls.Button
                    CreateSUserButton = Master.FindControl("Accordion1").FindControl("DBAdminPane").FindControl("Database_Log_Bttn")
                    CreateSUserButton.ForeColor = Drawing.Color.Red
                    myPaneName = "DBAdminPane" 'paneSelectedIndex = 0
                End If
            Else
                Response.Redirect("~/Default.aspx", False)
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Page Load - Contact Administrator !');", True)
        End Try
    End Sub
    Public Sub GetUsers()
        Dim Command As SqlCommand = Nothing
        Dim dt As DataTable = Nothing
        Try
            Command = New SqlCommand("SELECT  USER_CODE, USER_NAME FROM USERS ORDER BY USER_NAME ", SqlConn)
            SqlConn.Open()
            Dim da As New SqlDataAdapter(Command)
            Dim ds As New DataSet
            Dim RecordCount As Long = 0
            da.Fill(ds)

            dt = ds.Tables(0).Copy

            Dim Dr As DataRow
            Dr = dt.NewRow
            Dr("USER_CODE") = ""
            Dr("USER_NAME") = ""
            dt.Rows.InsertAt(Dr, 0)

            If dt.Rows.Count = 0 Then
                Me.DD_UserCode.DataSource = Nothing
            Else
                Me.DD_UserCode.DataSource = dt
                Me.DD_UserCode.DataTextField = "USER_NAME"
                Me.DD_UserCode.DataValueField = "USER_CODE"
                Me.DD_UserCode.DataBind()
            End If
            dt.Dispose()
        Catch s As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Populate libraries DataCombo - Contact Administrator !');", True)
        Finally
            Command.Dispose()
            dt.Dispose()
            SqlConn.Close()
        End Try
    End Sub
    'search logs
    Public Sub Search_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Search_Bttn.Click
        Dim dtLogs As DataTable = Nothing
        Dim Check As Integer

        Try
            Dim c, counter1, counter2, counter3, counter4, Counter5 As Integer
            Dim iloop As Integer
            Dim strcurrentchar As Object

            If TextBox1.Text <> "" And TextBox2.Text = "" Then
                ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Plz enter the To Date... ');", True)
                Me.TextBox2.Focus()
                Exit Sub
            End If
            If TextBox1.Text = "" And TextBox2.Text <> "" Then
                ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Plz enter the From Date... ');", True)
                Me.TextBox1.Focus()
                Exit Sub
            End If

            'search start date
            Dim DateFrom As Object = Nothing
            If TextBox1.Text <> "" Then
                DateFrom = TrimAll(TextBox1.Text)
                DateFrom = RemoveQuotes(DateFrom)
                DateFrom = Convert.ToDateTime(DateFrom, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                If DateFrom.Length > 12 Then
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
                'Check = ValidateDate(TextBox1.Text)
                'If Check = 1 Then
                '    MsgBox("Please enter the date in dd/mm/yyyy format")
                '    TextBox1.Focus()
                '    Exit Sub
                'Else
                '    Check = 0
                '    DateFrom = TrimX(TextBox1.Text)
                'End If
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
                'Check = ValidateDate(TextBox1.Text)
                'If Check = 1 Then
                '    MsgBox("Please enter the date in dd/mm/yyyy format")
                '    TextBox2.Focus()
                '    Exit Sub
                'Else
                '    Check = 0
                '    DateTo = TrimX(TextBox2.Text)
                'End If
            Else
                DateTo = String.Empty
            End If

            'User code Name validation
            Dim UserCd As String = Nothing
            If DD_UserCode.Text <> "" Then
                UserCd = TrimX(DD_UserCode.SelectedValue)
                UserCd = RemoveQuotes(UserCd)
                If UserCd.Length > 12 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                    Me.DD_UserCode.Focus()
                    Exit Sub
                End If
                UserCd = " " & UserCd & " "
                If InStr(1, UserCd, "CREATE", 1) > 0 Or InStr(1, UserCd, "DELETE", 1) > 0 Or InStr(1, UserCd, "DROP", 1) > 0 Or InStr(1, UserCd, "INSERT", 1) > 1 Or InStr(1, UserCd, "TRACK", 1) > 1 Or InStr(1, UserCd, "TRACE", 1) > 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                    Me.DD_UserCode.Focus()
                    Exit Sub
                End If
                UserCd = TrimX(UserCd)
                'check unwanted characters
                c = 0
                counter3 = 0
                For iloop = 1 To Len(UserCd)
                    strcurrentchar = Mid(UserCd, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter3 = 1
                        End If
                    End If
                Next
                If counter3 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' data is not Valid... ');", True)
                    DD_UserCode.Focus()
                    Exit Sub
                End If
            Else
                UserCd = ""
            End If

            'Boolean Operator validation
            Dim myAction As String = Nothing
            If DD_Action.Text <> "" Then
                myAction = TrimAll(DD_Action.SelectedValue)
                myAction = RemoveQuotes(myAction)
                If myAction.Length > 20 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                    Me.DD_Action.Focus()
                    Exit Sub
                End If
                myAction = " " & myAction & " "
                If InStr(1, myAction, "CREATE", 1) > 0 Or InStr(1, myAction, "DELETE", 1) > 0 Or InStr(1, myAction, "DROP", 1) > 0 Or InStr(1, myAction, "INSERT", 1) > 1 Or InStr(1, myAction, "TRACK", 1) > 1 Or InStr(1, myAction, "TRACE", 1) > 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                    Me.DD_Action.Focus()
                    Exit Sub
                End If
                myAction = TrimAll(myAction)
                'check unwanted characters
                c = 0
                counter4 = 0
                For iloop = 1 To Len(myAction)
                    strcurrentchar = Mid(myAction, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter4 = 1
                        End If
                    End If
                Next
                If counter4 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' data is not Valid... ');", True)
                    DD_Action.Focus()
                    Exit Sub
                End If
            Else
                myAction = "ALL"
            End If

            'UI type
            Dim myUIType As String = Nothing
            If RadioButton1.Checked = True Then
                myUIType = "DataEntry"
            Else
                myUIType = "OPAC"
            End If
            If myUIType <> "" Then
                myUIType = RemoveQuotes(myUIType)
                If myUIType.Length > 12 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                    Exit Sub
                End If
                myUIType = " " & myUIType & " "
                If InStr(1, myUIType, "CREATE", 1) > 0 Or InStr(1, myUIType, "DELETE", 1) > 0 Or InStr(1, myUIType, "DROP", 1) > 0 Or InStr(1, myUIType, "INSERT", 1) > 1 Or InStr(1, myUIType, "TRACK", 1) > 1 Or InStr(1, myUIType, "TRACE", 1) > 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                    Exit Sub
                End If
                myUIType = TrimX(myUIType)
                'check unwanted characters
                c = 0
                counter5 = 0
                For iloop = 1 To Len(myUIType)
                    strcurrentchar = Mid(myUIType, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter5 = 1
                        End If
                    End If
                Next
                If counter5 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' data is not Valid... ');", True)
                    Exit Sub
                End If
            Else
                myUIType = "DataEntry"
            End If

            Dim SQL As String = Nothing
            SQL = "SELECT USER_LOG_ID, USER_CODE, IP, LOGIN_DATE,  convert(varchar,LOGIN_TIME,108) as LOGIN_TIME, PAGE_VISITED, SUCCESS, ACTION, UI_TYPE, REMARKS, LOGOUT_DATE, convert(varchar,LOGOUT_TIME,108) as LOGOUT_TIME FROM USER_LOGS "
            SQL = SQL & " WHERE (UI_TYPE ='" & Trim(myUIType) & "')"

            If DateFrom <> "" And DateTo <> "" And UserCd <> "" Then
                SQL = SQL & " AND (LOGIN_DATE >= '" & Trim(DateFrom) & "' and LOGIN_DATE <= '" & Trim(DateTo) & "') AND (USER_CODE ='" & Trim(UserCd) & "')"
            End If

            If DateFrom <> "" And DateTo <> "" And UserCd = "" Then
                SQL = SQL & " AND (LOGIN_DATE >= '" & Trim(DateFrom) & "' and LOGIN_DATE <= '" & Trim(DateTo) & "') "
            End If

            If DateFrom = "" And DateTo = "" And UserCd <> "" Then
                SQL = SQL & " AND  (USER_CODE ='" & Trim(UserCd) & "')"
            End If

            If myAction = "LoggedIn" Or myAction = "Failure" Then
                SQL = SQL & " AND  (ACTION ='" & Trim(myAction) & "')"
            End If

            SQL = SQL & "  ORDER BY LOGIN_DATE DESC, LOGIN_TIME DESC "

            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            da.Fill(ds)

            dtLogs = ds.Tables(0).Copy
            Dim RecordCount As Long = 0
            If dtLogs.Rows.Count = 0 Then
                Me.Grid1_Logs.DataSource = Nothing
                Delete_Bttn.Enabled = False
            Else
                RecordCount = dtLogs.Rows.Count
                Grid1_Logs.AutoGenerateColumns = False
                Grid1_Logs.DataSource = dtLogs
                Grid1_Logs.DataBind()
                Label1.Text = "Total Record(s): " & RecordCount
                Grid1_Logs.ToolTip = "Total Record(s): " & RecordCount
                Delete_Bttn.Enabled = True
            End If
            ViewState("dt") = dtLogs
        Catch s As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Error -Search Logs: Contact Administrator !');", True)
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
    Protected Sub Grid1_Logs_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles Grid1_Logs.PageIndexChanging
        Try
            'rebind datagrid
            Grid1_Logs.DataSource = ViewState("dt") 'temp
            Grid1_Logs.PageIndex = e.NewPageIndex
            index = e.NewPageIndex * Grid1_Logs.PageSize
            Grid1_Logs.DataBind()
        Catch s As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' there is error in page index..');", True)
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
    Protected Sub Grid1_Logs_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles Grid1_Logs.Sorting
        Dim temp As DataTable = CType(ViewState("dt"), DataTable)
        GridViewSortExpression = e.SortExpression
        Grid1_Logs.DataSource = temp
        Dim pageIndex As Integer = Grid1_Logs.PageIndex
        Grid1_Logs.DataSource = SortDataTable(Grid1_Logs.DataSource, False)
        Grid1_Logs.DataBind()
        Grid1_Logs.PageIndex = pageIndex
    End Sub
    Protected Sub Grid1_Logs_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Grid1_Logs.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Lblsr As Label = e.Row.FindControl("lblsr")
            Srno = index + 1
            Lblsr.Text = Srno
            index += 1

            'Dim SearchText As String = ViewState("mySearchString")
            'If e.Row.Cells(2).Text.Contains(SearchText) Then
            'e.Row.Cells(1).Text = Highlight(e.Row.Cells(1).Text, SearchText, "<span style=""color:red"">", "</span>")
            'End If
        End If
    End Sub
    Private Shared Function t() As Object
        Throw New NotImplementedException
    End Function
    'delete selected rows
    Protected Sub Delete_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Delete_Bttn.Click
        Try
            For Each row As GridViewRow In Grid1_Logs.Rows
                Dim cb As CheckBox = DirectCast(row.FindControl("cbd"), CheckBox)
                If cb IsNot Nothing AndAlso cb.Checked = True Then
                    Dim LogID As Integer = Convert.ToInt32(Grid1_Logs.DataKeys(row.RowIndex).Value)

                    'get cat record
                    Dim SQL As String = Nothing
                    SQL = "DELETE FROM USER_LOGS WHERE (USER_LOG_ID ='" & Trim(LogID) & "') "
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
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Error Delete Logs: Contact Administrator !');", True)
        Finally
            SqlConn.Close()
        End Try
    End Sub




End Class