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
Public Class Subjects
    Inherits System.Web.UI.Page
    Dim index As Integer = 0
    Public Srno As Integer = 0
    Dim myClassScheme As String = Nothing
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session.Item("IsThisU") <> Nothing Then
                If Not SConnection() = True Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Database Connection is lost..Try Again !');", True)
                Else
                    If Page.IsPostBack = False Then
                        Me.bttn_Save.Visible = True
                        Me.bttn_Update.Visible = False
                        PopulateSubjects()
                        Label6.Text = "Enter Data and Press SAVE Button to save the record.."
                    End If
                End If
                Dim CreateSUserButton As Object 'System.Web.UI.WebControls.Button
                CreateSUserButton = Master.FindControl("Accordion1").FindControl("MasterPane").FindControl("M_Subjects_Bttn")
                CreateSUserButton.ForeColor = Drawing.Color.Red
                myPaneName = "MasterPane" 'paneSelectedIndex = 1
            Else
                Response.Redirect("~/Default.aspx", False)
            End If
            LibCode = Session.Item("LoggedLibcode")
        Catch ex As Exception
            Label6.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    Private Sub Subject_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        txt_Sub_Name.Focus()
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
                '****************************************************************************************
                'Server Validation for section name
                Dim Name As String = Nothing
                If txt_Sub_Name.Text <> "" Then
                    Name = TrimAll(UCase(txt_Sub_Name.Text))
                    Name = RemoveQuotes(Name)
                    If Name.Length > 100 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        txt_Sub_Name.Focus()
                        Exit Sub
                    End If
                    Name = " " & Name & " "
                    If InStr(1, Name, "CREATE", 1) > 0 Or InStr(1, Name, "DELETE", 1) > 0 Or InStr(1, Name, "DROP", 1) > 0 Or InStr(1, Name, "INSERT", 1) > 1 Or InStr(1, Name, "TRACK", 1) > 1 Or InStr(1, Name, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Me.txt_Sub_Name.Focus()
                        Exit Sub
                    End If
                    Name = TrimAll(Name)
                    'check unwanted characters
                    c = 0
                    counter1 = 0
                    For iloop = 1 To Len(Name.ToString)
                        strcurrentchar = Mid(Name, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter1 = 1
                            End If
                        End If
                    Next
                    If counter1 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Me.txt_Sub_Name.Focus()
                        Exit Sub
                    End If
                    'Check Duplicate comm Code
                    Dim str As Object = Nothing
                    Dim flag As Object = Nothing
                    str = "SELECT SUB_ID FROM SUBJECTS WHERE (SUB_NAME ='" & Trim(Name) & "')"
                    Dim cmd1 As New SqlCommand(str, SqlConn)
                    SqlConn.Open()
                    flag = cmd1.ExecuteScalar
                    If flag <> Nothing Then
                        Label6.Text = "Code Already exists..Enter another code)"
                        Me.txt_Sub_Name.Focus()
                        Exit Sub
                    End If
                    SqlConn.Close()
                Else
                    Label6.Text = " Plz Enter Subject Heading! "
                    Me.txt_Sub_Name.Focus()
                    Exit Sub
                End If

                '****************************************************************************************88
                'class no
                Dim myClassNo As String = Nothing
                myClassNo = TrimAll(Me.txt_Sub_ClassNo.Text)
                If Not String.IsNullOrEmpty(myClassNo) Then
                    myClassNo = RemoveQuotes(myClassNo)
                    If myClassNo.Length > 50 Then
                        Label6.Text = " Input is not Valid... "
                        Me.txt_Sub_ClassNo.Focus()
                        Exit Sub
                    End If
                    myClassNo = TrimAll(myClassNo)
                    'check unwanted characters
                    c = 0
                    counter2 = 0
                    For iloop = 1 To Len(myClassNo)
                        strcurrentchar = Mid(myClassNo, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter2 = 1
                            End If
                        End If
                    Next
                    If counter2 = 1 Then
                        Label6.Text = " Input is not Valid..."
                        Me.txt_Sub_ClassNo.Focus()
                        Exit Sub
                    End If
                Else
                    myClassNo = String.Empty
                End If

                '****************************************************************************************88
                'class no
                Dim myKeywords As String = Nothing
                myKeywords = TrimAll(Me.txt_Sub_Keywords.Text)
                If Not String.IsNullOrEmpty(myKeywords) Then
                    myKeywords = RemoveQuotes(myKeywords)
                    If myKeywords.Length > 400 Then
                        Label6.Text = " Input is not Valid... "
                        Me.txt_Sub_Keywords.Focus()
                        Exit Sub
                    End If
                    myKeywords = TrimAll(myKeywords)
                    'check unwanted characters
                    c = 0
                    counter3 = 0
                    For iloop = 1 To Len(myKeywords)
                        strcurrentchar = Mid(myKeywords, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter3 = 1
                            End If
                        End If
                    Next
                    If counter3 = 1 Then
                        Label6.Text = " Input is not Valid..."
                        Me.txt_Sub_Keywords.Focus()
                        Exit Sub
                    End If
                Else
                    myKeywords = String.Empty
                End If

                '****************************************************************************************88
                'parent subject ID
                Dim myPSubID As Integer = Nothing
                If DropDownList_Sub.Text <> "" Then
                    myPSubID = TrimAll(Me.DropDownList_Sub.SelectedValue)
                    myPSubID = RemoveQuotes(myPSubID)
                    If myPSubID.ToString.Length > 10 Then
                        Label6.Text = " Input is not Valid... "
                        Me.DropDownList_Sub.Focus()
                        Exit Sub
                    End If
                    myPSubID = TrimAll(myPSubID)
                    'check unwanted characters
                    c = 0
                    counter4 = 0
                    For iloop = 1 To myPSubID.ToString.Length
                        strcurrentchar = Mid(myPSubID, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter4 = 1
                            End If
                        End If
                    Next
                    If counter4 = 1 Then
                        Label6.Text = " Input is not Valid..."
                        Me.DropDownList_Sub.Focus()
                        Exit Sub
                    End If
                Else
                    myPSubID = 0
                End If

                'INSERT THE RECORD IN TO THE DATABASE
                Dim SQL As String
                Dim Cmd As SqlCommand
                Dim da As SqlDataAdapter
                Dim ds As New DataSet
                Dim CB As SqlCommandBuilder
                Dim dtrow As DataRow
                SQL = "SELECT * FROM SUBJECTS WHERE (SUB_ID = '00')"
                Cmd = New SqlCommand(SQL, SqlConn)
                da = New SqlDataAdapter(Cmd)
                CB = New SqlCommandBuilder(da)
                SqlConn.Open()
                da.Fill(ds, "SUBJECTS")
                dtrow = ds.Tables("SUBJECTS").NewRow

                If Not String.IsNullOrEmpty(Name) Then
                    dtrow("SUB_NAME") = Name.Trim
                End If

                If Not String.IsNullOrEmpty(myClassNo) Then
                    dtrow("CLASS_NO") = myClassNo.Trim
                Else
                    dtrow("CLASS_NO") = System.DBNull.Value
                End If
                If Not String.IsNullOrEmpty(myKeywords) Then
                    dtrow("SUB_KEYWORDS") = myKeywords.Trim
                Else
                    dtrow("SUB_KEYWORDS") = System.DBNull.Value
                End If

                If myPSubID = 0 Then
                    dtrow("P_SUB_ID") = System.DBNull.Value
                Else
                    dtrow("P_SUB_ID") = myPSubID
                End If

                dtrow("LIB_CODE") = LibCode
                dtrow("USER_CODE") = Session.Item("LoggedUser")
                dtrow("DATE_ADDED") = Now.Date
                dtrow("IP") = Request.UserHostAddress.Trim

                ds.Tables("SUBJECTS").Rows.Add(dtrow)

                thisTransaction = SqlConn.BeginTransaction()
                da.SelectCommand.Transaction = thisTransaction
                da.Update(ds, "SUBJECTS")
                thisTransaction.Commit()
                ClearFields()
                myClassNo = Nothing
                Name = Nothing
                myKeywords = Nothing
                myPSubID = Nothing

                ds.Dispose()
                Label6.Text = "Record Added Successfully!"
                bttn_Save.Visible = True
                bttn_Update.Visible = False
                ClearFields()
                Search_Bttn_Click(sender, e)
                PopulateSubjects()
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
        txt_Sub_Name.Text = ""
        txt_Sub_ClassNo.Text = ""
        txt_Sub_Keywords.Text = ""
        DropDownList_Sub.Enabled = True
    End Sub
    'populate subjects
    Public Sub PopulateSubjects()
        Dim Command As SqlCommand = Nothing
        Dim dt As DataTable = Nothing
        Try
            Command = New SqlCommand("SELECT SUB_ID, SUB_NAME FROM SUBJECTS ORDER BY SUB_NAME ", SqlConn)
            SqlConn.Open()
            Dim da As New SqlDataAdapter(Command)
            Dim ds As New DataSet
            Dim RecordCount As Long = 0
            da.Fill(ds)

            dt = ds.Tables(0).Copy

            Dim Dr As DataRow
            Dr = dt.NewRow
            Dr("SUB_ID") = System.DBNull.Value
            Dr("SUB_NAME") = ""
            dt.Rows.InsertAt(Dr, 0)
            If dt.Rows.Count = 0 Then
                Me.DropDownList_Sub.DataSource = Nothing
            Else
                Me.DropDownList_Sub.DataSource = dt
                Me.DropDownList_Sub.DataTextField = "SUB_NAME"
                Me.DropDownList_Sub.DataValueField = "SUB_ID"
                Me.DropDownList_Sub.DataBind()
            End If
            dt.Dispose()
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
        Finally
            Command.Dispose()
            dt.Dispose()
            SqlConn.Close()
        End Try
    End Sub
    'Populate the users in grid     'search users
    Protected Sub Search_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Search_Bttn.Click
        Dim dt As DataTable = Nothing
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
            If DropDownList1.Text <> "" Then
                myfield = TrimAll(DropDownList1.SelectedValue)
                myfield = RemoveQuotes(myfield)
                If myfield.Length > 50 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                    Me.DropDownList1.Focus()
                    Exit Sub
                End If
                myfield = " " & myfield & " "
                If InStr(1, myfield, "CREATE", 1) > 0 Or InStr(1, myfield, "DELETE", 1) > 0 Or InStr(1, myfield, "DROP", 1) > 0 Or InStr(1, myfield, "INSERT", 1) > 1 Or InStr(1, myfield, "TRACK", 1) > 1 Or InStr(1, myfield, "TRACE", 1) > 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
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
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' data is not Valid... ');", True)
                    DropDownList1.Focus()
                    Exit Sub
                End If
            Else
                myfield = "SUB_NAME"
            End If

            'Boolean Operator validation
            Dim myBoolean As String = Nothing
            If DropDownList2.Text <> "" Then
                myBoolean = TrimAll(DropDownList2.SelectedValue)
                myBoolean = RemoveQuotes(myBoolean)
                If myBoolean.Length > 20 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                    Me.DropDownList2.Focus()
                    Exit Sub
                End If
                myBoolean = " " & myBoolean & " "
                If InStr(1, myBoolean, "CREATE", 1) > 0 Or InStr(1, myBoolean, "DELETE", 1) > 0 Or InStr(1, myBoolean, "DROP", 1) > 0 Or InStr(1, myBoolean, "INSERT", 1) > 1 Or InStr(1, myBoolean, "TRACK", 1) > 1 Or InStr(1, myBoolean, "TRACE", 1) > 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
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
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' data is not Valid... ');", True)
                    DropDownList2.Focus()
                    Exit Sub
                End If
            Else
                myBoolean = "AND"
            End If

            'order by validation
            Dim OrderBy As String = Nothing
            If DropDownList3.Text <> "" Then
                OrderBy = TrimAll(DropDownList3.SelectedValue)
                OrderBy = RemoveQuotes(OrderBy)
                If OrderBy.Length > 20 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                    Me.DropDownList3.Focus()
                    Exit Sub
                End If
                OrderBy = " " & OrderBy & " "
                If InStr(1, OrderBy, "CREATE", 1) > 0 Or InStr(1, OrderBy, "DELETE", 1) > 0 Or InStr(1, OrderBy, "DROP", 1) > 0 Or InStr(1, OrderBy, "INSERT", 1) > 1 Or InStr(1, OrderBy, "TRACK", 1) > 1 Or InStr(1, OrderBy, "TRACE", 1) > 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                    Me.DropDownList3.Focus()
                    Exit Sub
                End If
                OrderBy = TrimAll(OrderBy)
                'check unwanted characters
                c = 0
                counter4 = 0
                For iloop = 1 To Len(OrderBy)
                    strcurrentchar = Mid(OrderBy, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter4 = 1
                        End If
                    End If
                Next
                If counter4 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' data is not Valid... ');", True)
                    DropDownList3.Focus()
                    Exit Sub
                End If
            Else
                OrderBy = "SUB_NAME"
            End If

            'sort by validation
            Dim SortBy As String = Nothing
            If DropDownList4.Text <> "" Then
                SortBy = TrimAll(DropDownList4.SelectedValue)
                SortBy = RemoveQuotes(SortBy)
                If SortBy.Length > 20 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                    Me.DropDownList4.Focus()
                    Exit Sub
                End If
                SortBy = " " & SortBy & " "
                If InStr(1, SortBy, "CREATE", 1) > 0 Or InStr(1, SortBy, "DELETE", 1) > 0 Or InStr(1, SortBy, "DROP", 1) > 0 Or InStr(1, SortBy, "INSERT", 1) > 1 Or InStr(1, SortBy, "TRACK", 1) > 1 Or InStr(1, SortBy, "TRACE", 1) > 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                    Me.DropDownList4.Focus()
                    Exit Sub
                End If
                SortBy = TrimAll(SortBy)
                'check unwanted characters
                c = 0
                counter4 = 0
                For iloop = 1 To Len(SortBy)
                    strcurrentchar = Mid(SortBy, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter4 = 1
                        End If
                    End If
                Next
                If counter4 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' data is not Valid... ');", True)
                    DropDownList4.Focus()
                    Exit Sub
                End If
            Else
                SortBy = "ASC"
            End If

            Dim SQL As String = Nothing
            SQL = "SELECT * FROM SUBJECTS "

            If txt_Search.Text <> "" Then
                If myBoolean = "LIKE" Then
                    SQL = SQL & " WHERE (" & myfield & " LIKE N'%" & Trim(mySearchString) & "%') "
                End If
                If myBoolean = "SW" Then
                    SQL = SQL & " WHERE (" & myfield & " LIKE N'" & Trim(mySearchString) & "%') "
                End If
                If myBoolean = "EW" Then
                    SQL = SQL & " WHERE (" & myfield & " LIKE N'%" & Trim(mySearchString) & "') "
                End If
                If myBoolean = "AND" Then
                    Dim h As Integer
                    Dim myNewSearchString As Object
                    myNewSearchString = Split(mySearchString, " ")
                    SQL = SQL & " WHERE (" & myfield & " LIKE N'%" & Trim(myNewSearchString(0)) & "%' "
                    For h = 1 To UBound(myNewSearchString)
                        SQL = SQL & " AND " & myfield & " LIKE N'%" & Trim(myNewSearchString(h)) & "%'"
                    Next
                    SQL = SQL & ")"
                End If
                If myBoolean = "OR" Then
                    Dim h As Integer
                    Dim myNewSearchString As Object
                    myNewSearchString = Split(mySearchString, " ")
                    SQL = SQL & " WHERE (" & myfield & " LIKE N'%" & Trim(myNewSearchString(0)) & "%' "
                    For h = 1 To UBound(myNewSearchString)
                        SQL = SQL & " OR " & myfield & " LIKE N'%" & Trim(myNewSearchString(h)) & "%' "
                    Next
                    SQL = SQL & ")"
                End If
            End If

            SQL = SQL & " ORDER BY " & OrderBy & " " & SortBy

            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            da.Fill(ds)

            dt = ds.Tables(0).Copy
            Dim RecordCount As Long = 0
            If dt.Rows.Count = 0 Then
                Me.Grid_Subject.DataSource = Nothing
                Grid_Subject.DataBind()
                Label1.Text = "Total Record(s): 0 "
                Delete_Bttn.Enabled = False
            Else
                Grid_Subject.Visible = True
                RecordCount = dt.Rows.Count
                Grid_Subject.DataSource = dt
                Grid_Subject.DataBind()
                Label1.Text = "Total Record(s): " & RecordCount
                Delete_Bttn.Enabled = True
            End If
            ViewState("dt") = dt
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
    Protected Sub Grid1_Subject_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles Grid_Subject.PageIndexChanging
        Try
            'rebind datagrid
            Grid_Subject.DataSource = ViewState("dt") 'temp
            Grid_Subject.PageIndex = e.NewPageIndex
            index = e.NewPageIndex * Grid_Subject.PageSize
            Grid_Subject.DataBind()
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
    Protected Sub Grid_Subject_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles Grid_Subject.Sorting
        Dim temp As DataTable = CType(ViewState("dt"), DataTable)
        GridViewSortExpression = e.SortExpression
        Grid_Subject.DataSource = temp
        Dim pageIndex As Integer = Grid_Subject.PageIndex
        Grid_Subject.DataSource = SortDataTable(Grid_Subject.DataSource, False)
        Grid_Subject.DataBind()
        Grid_Subject.PageIndex = pageIndex
        UpdatePanel1.Update()
    End Sub
    Protected Sub Grid_Subject_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Grid_Subject.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Lblsr As Label = e.Row.FindControl("lblsr")
            Srno = index + 1
            If Lblsr Is Nothing = False Then
                Lblsr.Text = Srno
                index += 1
            End If
            e.Row.Attributes("onmouseover") = "this.style.cursor='hand';this.style.textDecoration='bold';this.style.background='#FFDFDF'"
            e.Row.Attributes("onmouseout") = "this.style.textDecoration='none';this.style.background='none'"
            'e.Row.Attributes("onclick") = ClientScript.GetPostBackClientHyperlink(Me, "Select$" & Convert.ToString(e.Row.RowIndex))
        End If
    End Sub
    Private Shared Function t() As Object
        Throw New NotImplementedException
    End Function
    'get value of row from grid
    Private Sub Grid_Subject_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles Grid_Subject.RowCommand
        Dim Command As SqlCommand = Nothing
        Dim dr As SqlDataReader = Nothing
        Try
            If e.CommandName = "Select" Then
                Dim myRowID, SubID As Integer
                myRowID = e.CommandArgument.ToString()

                If Grid_Subject.Rows(myRowID).Cells(5).Text <> "" Then
                    SubID = Grid_Subject.Rows(myRowID).Cells(5).Text
                    Label7.Text = SubID
                Else
                    SubID = ""
                    Label7.Text = ""
                End If

                Dim iloop As Integer
                Dim strcurrentchar As Object
                Dim c As Integer
                Dim counter1 As Integer

                SubID = TrimX(SubID)
                If Not String.IsNullOrEmpty(SubID) Then
                    SubID = RemoveQuotes(SubID)
                    If Len(SubID).ToString > 10 Then
                        Label6.Text = "Length of Input is not Proper... "
                        Exit Sub
                    End If
                    SubID = " " & SubID & " "
                    If InStr(1, SubID, " CREATE ", 1) > 0 Or InStr(1, SubID, " DELETE ", 1) > 0 Or InStr(1, SubID, " DROP ", 1) > 0 Or InStr(1, SubID, " INSERT ", 1) > 1 Or InStr(1, SubID, " TRACK ", 1) > 1 Or InStr(1, SubID, " TRACE ", 1) > 1 Then
                        Label6.Text = "Do not use reserve words..."
                        Exit Sub
                    End If
                    SubID = TrimX(SubID)
                Else
                    Label6.Text = "Select Record... "
                    Exit Sub
                End If

                'get record details from database
                Dim SQL As String = Nothing
                SQL = " SELECT *  FROM SUBJECTS WHERE (SUB_ID = '" & Trim(SubID) & "') "
                Command = New SqlCommand(SQL, SqlConn)
                SqlConn.Open()
                dr = Command.ExecuteReader(CommandBehavior.CloseConnection)
                dr.Read()
                ClearFields()
                If dr.HasRows = True Then
                    If dr.Item("SUB_NAME").ToString <> "" Then
                        Me.txt_Sub_Name.Text = dr.Item("SUB_NAME").ToString
                    Else
                        txt_Sub_Name.Text = ""
                    End If
                    If dr.Item("CLASS_NO").ToString <> "" Then
                        Me.txt_Sub_ClassNo.Text = dr.Item("CLASS_NO").ToString
                    Else
                        Me.txt_Sub_ClassNo.Text = ""
                    End If

                    If dr.Item("SUB_KEYWORDS").ToString <> "" Then
                        Me.txt_Sub_Keywords.Text = dr.Item("SUB_KEYWORDS").ToString
                    Else
                        Me.txt_Sub_Keywords.Text = ""
                    End If
                    If dr.Item("P_SUB_ID").ToString <> "" Then
                        DropDownList_Sub.SelectedValue = dr.Item("P_SUB_ID")
                    Else
                        Me.DropDownList_Sub.Text = ""
                    End If

                    bttn_Save.Visible = False
                    bttn_Update.Visible = True
                    Label6.Text = "Press UPDATE Button to save the Changes if any.."
                    dr.Close()
                    SqlConn.Close()
                Else
                    Label6.Text = "No Record to Edit... "
                    Exit Sub
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
                Dim counter1, counter2, counter3, counter4 As Integer

                'Server Validation for section name
                Dim Name As String = Nothing
                If txt_Sub_Name.Text <> "" Then
                    Name = TrimAll(UCase(txt_Sub_Name.Text))
                    Name = RemoveQuotes(Name)
                    If Name.Length > 100 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        txt_Sub_Name.Focus()
                        Exit Sub
                    End If
                    Name = " " & Name & " "
                    If InStr(1, Name, "CREATE", 1) > 0 Or InStr(1, Name, "DELETE", 1) > 0 Or InStr(1, Name, "DROP", 1) > 0 Or InStr(1, Name, "INSERT", 1) > 1 Or InStr(1, Name, "TRACK", 1) > 1 Or InStr(1, Name, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Me.txt_Sub_Name.Focus()
                        Exit Sub
                    End If
                    Name = TrimAll(Name)
                    'check unwanted characters
                    c = 0
                    counter1 = 0
                    For iloop = 1 To Len(Name.ToString)
                        strcurrentchar = Mid(Name, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter1 = 1
                            End If
                        End If
                    Next
                    If counter1 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Me.txt_Sub_Name.Focus()
                        Exit Sub
                    End If
                    'Check Duplicate comm Code
                    Dim str As Object = Nothing
                    Dim flag As Object = Nothing
                    'str = "SELECT VEND_ID FROM VENDORS WHERE (VEND_ID <>'" & Trim(Label7.Text) & "' AND VEND_NAME = '" & Trim(Name) & "' ) "
                    str = "SELECT SUB_ID FROM SUBJECTS WHERE (SUB_ID <> '" & Trim(Label7.Text) & "' AND SUB_NAME ='" & Trim(Name) & "')"
                    Dim cmd1 As New SqlCommand(str, SqlConn)
                    SqlConn.Open()
                    flag = cmd1.ExecuteScalar
                    If flag <> Nothing Then
                        Label6.Text = "Code Already exists..Enter another code)"
                        Me.txt_Sub_Name.Focus()
                        Exit Sub
                    End If
                    SqlConn.Close()
                Else
                    Label6.Text = " Plz Enter Subject Heading! "
                    Me.txt_Sub_Name.Focus()
                    Exit Sub
                End If

                '****************************************************************************************88
                'class no
                Dim myClassNo As String = Nothing
                myClassNo = TrimAll(Me.txt_Sub_ClassNo.Text)
                If Not String.IsNullOrEmpty(myClassNo) Then
                    myClassNo = RemoveQuotes(myClassNo)
                    If myClassNo.Length > 50 Then
                        Label6.Text = " Input is not Valid... "
                        Me.txt_Sub_ClassNo.Focus()
                        Exit Sub
                    End If
                    myClassNo = TrimAll(myClassNo)
                    'check unwanted characters
                    c = 0
                    counter2 = 0
                    For iloop = 1 To Len(myClassNo)
                        strcurrentchar = Mid(myClassNo, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter2 = 1
                            End If
                        End If
                    Next
                    If counter2 = 1 Then
                        Label6.Text = " Input is not Valid..."
                        Me.txt_Sub_ClassNo.Focus()
                        Exit Sub
                    End If
                Else
                    myClassNo = String.Empty
                End If

                '****************************************************************************************88
                'class no
                Dim myKeywords As String = Nothing
                myKeywords = TrimAll(Me.txt_Sub_Keywords.Text)
                If Not String.IsNullOrEmpty(myKeywords) Then
                    myKeywords = RemoveQuotes(myKeywords)
                    If myKeywords.Length > 400 Then
                        Label6.Text = " Input is not Valid... "
                        Me.txt_Sub_Keywords.Focus()
                        Exit Sub
                    End If
                    myKeywords = TrimAll(myKeywords)
                    'check unwanted characters
                    c = 0
                    counter3 = 0
                    For iloop = 1 To Len(myKeywords)
                        strcurrentchar = Mid(myKeywords, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter3 = 1
                            End If
                        End If
                    Next
                    If counter3 = 1 Then
                        Label6.Text = " Input is not Valid..."
                        Me.txt_Sub_Keywords.Focus()
                        Exit Sub
                    End If
                Else
                    myKeywords = String.Empty
                End If

                '****************************************************************************************88
                'parent subject ID
                Dim myPSubID As Integer = Nothing
                If DropDownList_Sub.Text <> "" Then
                    myPSubID = TrimAll(Me.DropDownList_Sub.SelectedValue)
                    myPSubID = RemoveQuotes(myPSubID)
                    If myPSubID.ToString.Length > 10 Then
                        Label6.Text = " Input is not Valid... "
                        Me.DropDownList_Sub.Focus()
                        Exit Sub
                    End If
                    myPSubID = TrimAll(myPSubID)
                    'check unwanted characters
                    c = 0
                    counter4 = 0
                    For iloop = 1 To myPSubID.ToString.Length
                        strcurrentchar = Mid(myPSubID, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter4 = 1
                            End If
                        End If
                    Next
                    If counter4 = 1 Then
                        Label6.Text = " Input is not Valid..."
                        Me.DropDownList_Sub.Focus()
                        Exit Sub
                    End If
                Else
                    myPSubID = 0
                End If

                '****************************************************************************************************
                'UPDATE THE LIBRARY PROFILE  
                If Label7.Text <> "" Then
                    SQL = "SELECT * FROM SUBJECTS WHERE (SUB_ID='" & Trim(Label7.Text) & "')"
                    SqlConn.Open()
                    da = New SqlDataAdapter(SQL, SqlConn)
                    cb = New SqlCommandBuilder(da)
                    da.Fill(ds, "SUBJECTS")
                    If ds.Tables("SUBJECTS").Rows.Count <> 0 Then
                        If Not String.IsNullOrEmpty(Name) Then
                            ds.Tables("SUBJECTS").Rows(0)("SUB_NAME") = Name.Trim
                        Else
                            ds.Tables("SUBJECTS").Rows(0)("SUB_NAME") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(myClassNo) Then
                            ds.Tables("SUBJECTS").Rows(0)("CLASS_NO") = myClassNo.Trim
                        Else
                            ds.Tables("SUBJECTS").Rows(0)("CLASS_NO") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(myKeywords) Then
                            ds.Tables("SUBJECTS").Rows(0)("SUB_KEYWORDS") = myKeywords.Trim
                        Else
                            ds.Tables("SUBJECTS").Rows(0)("SUB_KEYWORDS") = System.DBNull.Value
                        End If
                        If myPSubID = 0 Then
                            ds.Tables("SUBJECTS").Rows(0)("P_SUB_ID") = System.DBNull.Value
                        Else
                            ds.Tables("SUBJECTS").Rows(0)("P_SUB_ID") = myPSubID
                        End If

                        ds.Tables("SUBJECTS").Rows(0)("UPDATED_BY") = UserCode.Trim
                        ds.Tables("SUBJECTS").Rows(0)("DATE_MODIFIED") = Now.Date
                        ds.Tables("SUBJECTS").Rows(0)("IP") = Request.UserHostAddress.Trim

                        thisTransaction = SqlConn.BeginTransaction()
                        da.SelectCommand.Transaction = thisTransaction
                        da.Update(ds, "SUBJECTS")
                        thisTransaction.Commit()

                        Label6.Visible = True
                        Label6.Text = "User Record Updated Successfully"
                        ClearFields()
                    Else
                        Label6.Text = "Record Not Updated  - Please Contact System Administrator... "
                        Exit Sub
                    End If
                End If
            Else
                'record not selected
                Label6.Text = "Record Not Selected..."
                Exit Sub
            End If
            SqlConn.Close()
            ClearFields()
            Search_Bttn_Click(sender, e)
            PopulateSubjects()
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
            For i = 0 To Grid_Subject.Rows.Count - 1
                If DirectCast(Grid_Subject.Rows(i).Cells(0).FindControl("cbd"), CheckBox).Checked Then
                    Dim SubID As Integer = Nothing
                    SubID = Grid_Subject.Rows(i).Cells(5).Text
                    'chk for foreign reference in ACQ tble
                    Dim str As Object = Nothing
                    Dim flag As Object = Nothing
                    str = "SELECT CAT_NO FROM CATS WHERE (SUB_ID ='" & Trim(SubID) & "')"
                    Dim cmd1 As New SqlCommand(str, SqlConn)
                    SqlConn.Open()
                    flag = cmd1.ExecuteScalar
                    SqlConn.Close()
                    If flag <> Nothing Then
                        Label6.Text = "Subject reference saved..in CATS Table, can not be deleted)"
                    Else
                        'get cat record
                        Dim SQL As String = Nothing
                        SQL = "DELETE FROM SUBJECTS WHERE (SUB_ID ='" & Trim(SubID) & "') "
                        SqlConn.Open()
                        Dim objCommand As New SqlCommand(SQL, SqlConn)
                        Dim da As New SqlDataAdapter(objCommand)
                        Dim ds As New DataSet
                        da.Fill(ds)
                        SqlConn.Close()
                    End If
                End If
            Next
            Search_Bttn_Click(sender, e)
            PopulateSubjects()
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub

   
End Class