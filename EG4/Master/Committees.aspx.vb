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
Public Class Committees
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
                        PopulateGrid()
                        Label6.Text = "Enter Data and Press SAVE Button to save the record.."
                    End If
                End If
                Dim CreateSUserButton As Object 'System.Web.UI.WebControls.Button
                CreateSUserButton = Master.FindControl("Accordion1").FindControl("MasterPane").FindControl("M_Committees_Bttn")
                CreateSUserButton.ForeColor = Drawing.Color.Red
                'paneSelectedIndex = 1
                myPaneName = "MasterPane"

            Else
                Response.Redirect("~/Default.aspx", False)
            End If
            LibCode = Session.Item("LoggedLibcode")
        Catch ex As Exception
            Label6.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    Private Sub Committees_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        txt_Com_Code.Focus()
    End Sub
    'save user account
    Protected Sub Save_Click(ByVal sender As Object, ByVal e As EventArgs) Handles bttn_Save.Click
        If IsPostBack = True Then
            'check unwanted characters
            Dim iloop As Integer
            Dim strcurrentchar As Object
            Dim c As Integer
            Dim counter1, counter2, counter3, counter4, counter5, counter6, counter7, counter8 As Integer
            Dim thisTransaction As SqlClient.SqlTransaction = Nothing
            Try
                'Server Validation for committee code
                Dim Code As String = Nothing
                Code = TrimX(UCase(txt_Com_Code.Text))
                If String.IsNullOrEmpty(Code) Then
                    'ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Enter Data in this field... ');", True)
                    Label6.Text = "Plz Enter Data in this Field !"
                    Me.txt_Com_Code.Focus()
                    Exit Sub
                End If
                Code = RemoveQuotes(Code)
                If Code.Length > 10 Then 'maximum length
                    'ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Data must be of Proper Length.. ');", True)
                    Label6.Text = "Data must be of proper Length !"
                    txt_Com_Code.Focus()
                    Exit Sub
                End If
                If Code.Length < 2 Then ' minimum length
                    'ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Data must be of Proper Length.. ');", True)
                    Label6.Text = "Data must be or proper Length"
                    txt_Com_Code.Focus()
                    Exit Sub
                End If
                Code = " " & Code & " "
                If InStr(1, Code, "CREATE", 1) > 0 Or InStr(1, Code, "DELETE", 1) > 0 Or InStr(1, Code, "DROP", 1) > 0 Or InStr(1, Code, "INSERT", 1) > 1 Or InStr(1, Code, "TRACK", 1) > 1 Or InStr(1, Code, "TRACE", 1) > 1 Then
                    Label6.Text = "Do Not use Reserve Words..."
                    Me.txt_Com_Code.Focus()
                    Exit Sub
                End If
                Code = TrimX(Code)
                'check unwanted characters
                c = 0
                counter1 = 0
                For iloop = 1 To Len(Code.ToString)
                    strcurrentchar = Mid(Code, iloop, 1)
                    If c = 0 Then
                        If InStr("123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter1 = 1
                        End If
                    End If
                Next
                If counter1 = 1 Then
                    Label6.Text = " Do Not use un-wanted Characters..."
                    Me.txt_Com_Code.Focus()
                    Exit Sub
                End If

                'Check Duplicate comm Code
                Dim str As Object = Nothing
                Dim flag As Object = Nothing
                str = "SELECT COM_ID FROM COMMITTEES WHERE (COM_CODE ='" & Trim(Code) & "') AND  (LIB_CODE = '" & Trim(LibCode) & "')"
                Dim cmd1 As New SqlCommand(str, SqlConn)
                SqlConn.Open()
                flag = cmd1.ExecuteScalar
                If flag <> Nothing Then
                    'ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' User Code Already Exists, Please try to enter other User Code... ');", True)
                    Label6.Text = "Committee Code Already exists..Enter another code)"
                    Me.txt_Com_Code.Focus()
                    Exit Sub
                End If
                SqlConn.Close()
                '********************************************************************************************************************

                '****************************************************************************************
                'Server Validation for committee name
                Dim Name As String = Nothing
                Name = TrimAll(txt_Com_Name.Text)
                If String.IsNullOrEmpty(Name) Then
                    Label6.Text = " Please Enter Data in this field... "
                    Me.txt_Com_Name.Focus()
                    Exit Sub
                End If
                Name = RemoveQuotes(Name)
                If Name.Length > 250 Then 'maximum length
                    Label6.Text = " Data must be of Proper Length.. "
                    txt_Com_Name.Focus()
                    Exit Sub
                End If
                Name = " " & Name & " "
                If InStr(1, Name, "CREATE", 1) > 0 Or InStr(1, Name, "DELETE", 1) > 0 Or InStr(1, Name, "DROP", 1) > 0 Or InStr(1, Name, "INSERT", 1) > 1 Or InStr(1, Name, "TRACK", 1) > 1 Or InStr(1, Name, "TRACE", 1) > 1 Then
                    Label6.Text = " Do Not use Reserve Words... "
                    Me.txt_Com_Name.Focus()
                    Exit Sub
                End If
                Name = TrimAll(Name)
                'check unwanted characters
                c = 0
                counter2 = 0
                For iloop = 1 To Len(Name.ToString)
                    strcurrentchar = Mid(Name, iloop, 1)
                    If c = 0 Then
                        If Not InStr("'~!@#$^&*+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter2 = 1
                        End If
                    End If
                Next
                If counter2 = 1 Then
                    Label6.Text = " Do Not use un-wanted Characters... "
                    Me.txt_Com_Name.Focus()
                    Exit Sub
                End If

                '****************************************************************************************
                'Server Validation for committee chairman
                Dim Chairman As String = Nothing
                Chairman = TrimAll(txt_Com_Chairman.Text)
                If String.IsNullOrEmpty(Chairman) Then
                    Label6.Text = " Please Enter Data in this field... "
                    Me.txt_Com_Chairman.Focus()
                    Exit Sub
                End If
                Chairman = RemoveQuotes(Chairman)
                If Chairman.Length > 100 Then 'maximum length
                    Label6.Text = " Data must be of Proper Length.. "
                    txt_Com_Chairman.Focus()
                    Exit Sub
                End If
                Chairman = " " & Chairman & " "
                If InStr(1, Chairman, "CREATE", 1) > 0 Or InStr(1, Chairman, "DELETE", 1) > 0 Or InStr(1, Chairman, "DROP", 1) > 0 Or InStr(1, Chairman, "INSERT", 1) > 1 Or InStr(1, Chairman, "TRACK", 1) > 1 Or InStr(1, Chairman, "TRACE", 1) > 1 Then
                    Label6.Text = " Do Not use Reserve Words..."
                    Me.txt_Com_Chairman.Focus()
                    Exit Sub
                End If
                Chairman = TrimAll(Chairman)
                'check unwanted characters
                c = 0
                counter3 = 0
                For iloop = 1 To Len(Chairman.ToString)
                    strcurrentchar = Mid(Chairman, iloop, 1)
                    If c = 0 Then
                        If Not InStr("'~!@#$^&*+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter3 = 1
                        End If
                    End If
                Next
                If counter3 = 1 Then
                    Label6.Text = " Do Not use un-wanted Characters..."
                    Me.txt_Com_Chairman.Focus()
                    Exit Sub
                End If

                '****************************************************************************************
                'Server Validation for committee members
                Dim Member As String = Nothing
                Member = TrimAll(txt_Com_Members.Text)
                If String.IsNullOrEmpty(Member) Then
                    Label6.Text = " Please Enter Data in this field... "
                    Me.txt_Com_Members.Focus()
                    Exit Sub
                End If
                Member = RemoveQuotes(Member)
                If Chairman.Length > 250 Then 'maximum length
                    Label6.Text = " Data must be of Proper Length.. "
                    txt_Com_Members.Focus()
                    Exit Sub
                End If
                Member = " " & Member & " "
                If InStr(1, Member, "CREATE", 1) > 0 Or InStr(1, Member, "DELETE", 1) > 0 Or InStr(1, Member, "DROP", 1) > 0 Or InStr(1, Member, "INSERT", 1) > 1 Or InStr(1, Member, "TRACK", 1) > 1 Or InStr(1, Member, "TRACE", 1) > 1 Then
                    Label6.Text = " Do Not use Reserve Words... "
                    Me.txt_Com_Members.Focus()
                    Exit Sub
                End If
                Member = TrimAll(Member)
                'check unwanted characters
                c = 0
                counter4 = 0
                For iloop = 1 To Len(Member.ToString)
                    strcurrentchar = Mid(Member, iloop, 1)
                    If c = 0 Then
                        If Not InStr("'~!@#$^&*+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter4 = 1
                        End If
                    End If
                Next
                If counter4 = 1 Then
                    Label6.Text = " Do Not use un-wanted Characters... "
                    Me.txt_Com_Members.Focus()
                    Exit Sub
                End If

                '****************************************************************************************
                'Server Validation for E-Mail Address
                Dim email As String
                email = TrimX(txt_Com_Email.Text)
                If Not String.IsNullOrEmpty(email) Then
                    email = RemoveQuotes(email)
                    If email.Length > 200 Then
                        Label6.Text = " Input is not Valid.."
                        Me.txt_Com_Email.Focus()
                        Exit Sub
                    End If
                    email = " " & email & " "
                    If InStr(1, email, "CREATE", 1) > 0 Or InStr(1, email, "DELETE", 1) > 0 Or InStr(1, email, "DROP", 1) > 0 Or InStr(1, email, "INSERT", 1) > 1 Or InStr(1, email, "TRACK", 1) > 1 Or InStr(1, email, "TRACE", 1) > 1 Then
                        Label6.Text = "  Input is not Valid... "
                        Me.txt_Com_Email.Focus()
                        Exit Sub
                    End If
                    email = TrimX(email)
                    'check unwanted characters
                    c = 0
                    counter5 = 0
                    For iloop = 1 To Len(email)
                        strcurrentchar = Mid(email, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';~!#$^&*|[]{}?<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter5 = 1
                            End If
                        End If
                    Next
                    If counter5 = 1 Then
                        Label6.Text = "  Input is not Valid..."
                        Me.txt_Com_Email.Focus()
                        Exit Sub
                    End If

                Else
                    email = ""
                End If
                '****************************************************************************************88
                'remareks
                Dim myRemarks As String = Nothing
                myRemarks = TrimAll(Me.txt_Com_Remarks.Text)
                If Not String.IsNullOrEmpty(myRemarks) Then
                    myRemarks = RemoveQuotes(myRemarks)
                    If myRemarks.Length > 250 Then
                        Label6.Text = " Input is not Valid... "
                        Me.txt_Com_Remarks.Focus()
                        Exit Sub
                    End If
                    myRemarks = TrimAll(myRemarks)
                    'check unwanted characters
                    c = 0
                    counter6 = 0
                    For iloop = 1 To Len(myRemarks)
                        strcurrentchar = Mid(myRemarks, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter6 = 1
                            End If
                        End If
                    Next
                    If counter6 = 1 Then
                        Label6.Text = " Input is not Valid..."
                        Me.txt_Com_Remarks.Focus()
                        Exit Sub
                    End If
                Else
                    myRemarks = String.Empty
                End If

                'search start date
                Dim SDate As Object = Nothing
                If txt_Com_SDate.Text <> "" Then
                    SDate = TrimX(txt_Com_SDate.Text)
                    SDate = RemoveQuotes(SDate)
                    SDate = Convert.ToDateTime(SDate, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us
                    'DateFrom = Convert.ToDateTime(DateFrom, System.Globalization.CultureInfo.GetCultureInfo("en-US")).ToString("MM/dd/yyyy") ' convert from us to india
                    ' DateFrom = Convert.ToDateTime(DateTime.Now, System.Globalization.CultureInfo.CurrentCulture).ToString("MM/dd/yyyy hh:MM:ss")
                    'DateFrom = Convert.ToDateTime(DateFrom, System.Globalization.CultureInfo.CurrentCulture).ToString("MM/dd/yyyy hh:MM:ss")

                    If Len(SDate) > 12 Then
                        Label6.Text = " Input is not Valid..."
                        Me.txt_Com_SDate.Focus()
                        Exit Sub
                    End If
                    SDate = " " & SDate & " "
                    If InStr(1, SDate, "CREATE", 1) > 0 Or InStr(1, SDate, "DELETE", 1) > 0 Or InStr(1, SDate, "DROP", 1) > 0 Or InStr(1, SDate, "INSERT", 1) > 1 Or InStr(1, SDate, "TRACK", 1) > 1 Or InStr(1, SDate, "TRACE", 1) > 1 Then
                        Label6.Text = "  Input is not Valid... "
                        Me.txt_Com_SDate.Focus()
                        Exit Sub
                    End If
                    SDate = TrimX(SDate)
                    'check unwanted characters
                    c = 0
                    counter7 = 0
                    For iloop = 1 To Len(SDate)
                        strcurrentchar = Mid(SDate, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter7 = 1
                            End If
                        End If
                    Next
                    If counter7 = 1 Then
                        Label6.Text = "data is not Valid... "
                        Me.txt_Com_SDate.Focus()
                        Exit Sub
                    End If
                Else
                    SDate = String.Empty
                End If

                'search end date
                Dim EDate As Object = Nothing
                If txt_Com_EDate.Text <> "" Then
                    EDate = TrimX(txt_Com_EDate.Text)
                    EDate = RemoveQuotes(EDate)
                    EDate = Convert.ToDateTime(EDate, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                    If EDate.Length > 12 Then
                        Label6.Text = "Input is not Valid... "
                        Me.txt_Com_EDate.Focus()
                        Exit Sub
                    End If
                    EDate = " " & EDate & " "
                    If InStr(1, EDate, "CREATE", 1) > 0 Or InStr(1, EDate, "DELETE", 1) > 0 Or InStr(1, EDate, "DROP", 1) > 0 Or InStr(1, EDate, "INSERT", 1) > 1 Or InStr(1, EDate, "TRACK", 1) > 1 Or InStr(1, EDate, "TRACE", 1) > 1 Then
                        Label6.Text = " Input is not Valid..."
                        Me.txt_Com_EDate.Focus()
                        Exit Sub
                    End If
                    EDate = TrimX(EDate)
                    'check unwanted characters
                    c = 0
                    counter8 = 0
                    For iloop = 1 To Len(EDate)
                        strcurrentchar = Mid(EDate, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter8 = 1
                            End If
                        End If
                    Next
                    If counter8 = 1 Then
                        Label6.Text = " data is not Valid..."
                        Me.txt_Com_EDate.Focus()
                        Exit Sub
                    End If
                Else
                    EDate = String.Empty
                End If


                'INSERT THE RECORD IN TO THE DATABASE
                Dim SQL As String
                Dim Cmd As SqlCommand
                Dim da As SqlDataAdapter
                Dim ds As New DataSet
                Dim CB As SqlCommandBuilder
                Dim dtrow As DataRow
                SQL = "SELECT * FROM COMMITTEES WHERE (COM_ID = '00')"
                Cmd = New SqlCommand(SQL, SqlConn)
                da = New SqlDataAdapter(Cmd)
                CB = New SqlCommandBuilder(da)
                SqlConn.Open()
                da.Fill(ds, "COMMITTEES")
                dtrow = ds.Tables("COMMITTEES").NewRow

                If Not String.IsNullOrEmpty(Code) Then
                    dtrow("COM_CODE") = Code.Trim
                End If

                If Not String.IsNullOrEmpty(Name) Then
                    dtrow("COM_NAME") = Name.Trim
                End If
                If Not String.IsNullOrEmpty(Chairman) Then
                    dtrow("COM_CHAIRMAN") = Chairman
                End If
                If Not String.IsNullOrEmpty(Member) Then
                    dtrow("COM_MEMB") = Member
                Else
                    dtrow("COM_MEMB") = System.DBNull.Value
                End If
                If Not String.IsNullOrEmpty(email) Then
                    dtrow("COM_EMAIL") = email.Trim
                Else
                    dtrow("COM_EMAIL") = System.DBNull.Value
                End If
                If Not String.IsNullOrEmpty(myRemarks) Then
                    dtrow("COM_DESC") = myRemarks.Trim
                Else
                    dtrow("COM_DESC") = System.DBNull.Value
                End If
                If Not String.IsNullOrEmpty(SDate) Then
                    dtrow("START_DATE") = SDate.Trim
                Else
                    dtrow("START_DATE") = System.DBNull.Value
                End If
                If Not String.IsNullOrEmpty(EDate) Then
                    dtrow("END_DATE") = EDate.Trim
                Else
                    dtrow("END_DATE") = System.DBNull.Value
                End If
               

                dtrow("LIB_CODE") = LibCode
                dtrow("USER_CODE") = Session.Item("LoggedUser")
                dtrow("DATE_ADDED") = Now.Date
                dtrow("IP") = Request.UserHostAddress.Trim

                ds.Tables("COMMITTEES").Rows.Add(dtrow)

                thisTransaction = SqlConn.BeginTransaction()
                da.SelectCommand.Transaction = thisTransaction
                da.Update(ds, "COMMITTEES")
                thisTransaction.Commit()
                ClearFields()
                ' mailpwd()
                Code = Nothing
                Name = Nothing
                Chairman = Nothing
                Member = Nothing
                email = Nothing
                myRemarks = Nothing
                SDate = Nothing
                EDate = Nothing

                ds.Dispose()
                Label6.Text = "Committee Record Added Successfully!"
                bttn_Save.Visible = True
                bttn_Update.Visible = False
                ClearFields()
                PopulateGrid()
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
        txt_Com_Code.Text = ""
        txt_Com_Name.Text = ""
        txt_Com_Chairman.Text = ""
        txt_Com_Members.Text = ""
        txt_Com_Email.Text = ""
        txt_Com_Remarks.Text = ""
        txt_Com_SDate.Text = ""
        txt_Com_EDate.Text = ""
        txt_Com_Code.Enabled = True
    End Sub
    'Populate the users in grid     'search users
    Public Sub PopulateGrid()

        Dim dt As DataTable = Nothing
        Try
            Dim SQL As String = Nothing
            SQL = "SELECT * FROM COMMITTEES WHERE  (LIB_CODE='" & Trim(LibCode) & "') ORDER BY COM_NAME  "

            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            da.Fill(ds)

            dt = ds.Tables(0).Copy
            Dim RecordCount As Long = 0
            If dt.Rows.Count = 0 Then
                Me.Grid_Committee.DataSource = Nothing
                Grid_Committee.DataBind()
                Label1.Text = "Total Record(s): 0 "
                Delete_Bttn.Enabled = False
            Else
                Grid_Committee.Visible = True
                RecordCount = dt.Rows.Count
                Grid_Committee.DataSource = dt
                Grid_Committee.DataBind()
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
    Protected Sub Grid1_LibTeam_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles Grid_Committee.PageIndexChanging
        Try
            'rebind datagrid
            Grid_Committee.DataSource = ViewState("dt") 'temp
            Grid_Committee.PageIndex = e.NewPageIndex
            index = e.NewPageIndex * Grid_Committee.PageSize
            Grid_Committee.DataBind()

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
    Protected Sub Grid_Committee_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles Grid_Committee.Sorting
        Dim temp As DataTable = CType(ViewState("dt"), DataTable)
        GridViewSortExpression = e.SortExpression
        Grid_Committee.DataSource = temp
        Dim pageIndex As Integer = Grid_Committee.PageIndex
        Grid_Committee.DataSource = SortDataTable(Grid_Committee.DataSource, False)
        Grid_Committee.DataBind()
        Grid_Committee.PageIndex = pageIndex
        UpdatePanel1.Update()
    End Sub
    Protected Sub Grid_Committee_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Grid_Committee.RowDataBound
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
            'Dim SearchText As String = ViewState("mySearchString")
            'If e.Row.Cells(2).Text.Contains(SearchText) Then
            'e.Row.Cells(1).Text = Highlight(e.Row.Cells(1).Text, SearchText, "<span style=""color:red"">", "</span>")
            'End If
        End If
    End Sub
    Private Shared Function t() As Object
        Throw New NotImplementedException
    End Function
    'get value of row from grid
    Private Sub Grid_Committee_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles Grid_Committee.RowCommand
        Dim Command As SqlCommand = Nothing
        Dim dr As SqlDataReader = Nothing
        Try
            If e.CommandName = "Select" Then
                Dim myRowID, ComID As Integer
                myRowID = e.CommandArgument.ToString()

                If Grid_Committee.Rows(myRowID).Cells(6).Text <> "" Then
                    ComID = Grid_Committee.Rows(myRowID).Cells(6).Text
                    Label7.Text = ComID
                Else
                    ComID = ""
                    Label7.Text = ""
                End If

                Dim iloop As Integer
                Dim strcurrentchar As Object
                Dim c As Integer
                Dim counter1 As Integer

                ComID = TrimX(ComID)
                ComID = UCase(ComID)
                If Not String.IsNullOrEmpty(ComID) Then
                    ComID = RemoveQuotes(ComID)
                    If Len(ComID).ToString > 10 Then
                        Label6.Text = "Length of Input is not Proper... "
                        Exit Sub
                    End If
                    ComID = " " & ComID & " "
                    If InStr(1, ComID, " CREATE ", 1) > 0 Or InStr(1, ComID, " DELETE ", 1) > 0 Or InStr(1, ComID, " DROP ", 1) > 0 Or InStr(1, ComID, " INSERT ", 1) > 1 Or InStr(1, ComID, " TRACK ", 1) > 1 Or InStr(1, ComID, " TRACE ", 1) > 1 Then
                        Label6.Text = "Do not use reserve words..."
                        Exit Sub
                    End If
                    ComID = TrimX(ComID)
                Else
                    Label6.Text = "Enter committee Code... "
                    Exit Sub
                End If

                'get record details from database
                Dim SQL As String = Nothing
                SQL = " SELECT *  FROM COMMITTEES WHERE (COM_ID = '" & Trim(ComID) & "') "
                Command = New SqlCommand(SQL, SqlConn)
                SqlConn.Open()
                dr = Command.ExecuteReader(CommandBehavior.CloseConnection)
                dr.Read()
                ClearFields()
                txt_Com_Code.Enabled = False
                If dr.HasRows = True Then
                    If dr.Item("COM_CODE").ToString <> "" Then
                        Me.txt_Com_Code.Text = dr.Item("COM_CODE").ToString
                    Else
                        txt_Com_Code.Text = ""
                    End If
                    If dr.Item("COM_NAME").ToString <> "" Then
                        Me.txt_Com_Name.Text = dr.Item("COM_NAME").ToString
                    Else
                        Me.txt_Com_Name.Text = ""
                    End If
                    If dr.Item("COM_CHAIRMAN").ToString <> "" Then
                        txt_Com_Chairman.Text = dr.Item("COM_CHAIRMAN").ToString
                    Else
                        txt_Com_Chairman.Text = ""
                    End If
                    If dr.Item("COM_MEMB").ToString <> "" Then
                        Me.txt_Com_Members.Text = dr.Item("COM_MEMB").ToString
                    Else
                        Me.txt_Com_Members.Text = ""
                    End If
                    If dr.Item("COM_EMAIL").ToString <> "" Then
                        txt_Com_Email.Text = dr.Item("COM_EMAIL").ToString
                    Else
                        txt_Com_Email.Text = ""
                    End If
                    If dr.Item("COM_DESC").ToString <> "" Then
                        Me.txt_Com_Remarks.Text = dr.Item("COM_DESC").ToString
                    Else
                        Me.txt_Com_Remarks.Text = ""
                    End If
                    If dr.Item("START_DATE").ToString <> "" Then
                        Me.txt_Com_SDate.Text = Format(dr.Item("START_DATE"), "dd/MM/yyyy")
                    Else
                        Me.txt_Com_SDate.Text = ""
                    End If
                    If dr.Item("END_DATE").ToString <> "" Then
                        Me.txt_Com_EDate.Text = Format(dr.Item("END_DATE"), "dd/MM/yyyy")
                    Else
                        Me.txt_Com_EDate.Text = ""
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
                Dim counter1, counter2, counter3, counter4, Counter5, counter6, counter7, counter8, counter9, counter10 As Integer

                'Server Validation for committee name
                Dim Name As String = Nothing
                Name = TrimAll(txt_Com_Name.Text)
                If String.IsNullOrEmpty(Name) Then
                    Label6.Text = " Please Enter Data in this field... "
                    Me.txt_Com_Name.Focus()
                    Exit Sub
                End If
                Name = RemoveQuotes(Name)
                If Name.Length > 250 Then 'maximum length
                    Label6.Text = " Data must be of Proper Length.. "
                    txt_Com_Name.Focus()
                    Exit Sub
                End If
                Name = " " & Name & " "
                If InStr(1, Name, "CREATE", 1) > 0 Or InStr(1, Name, "DELETE", 1) > 0 Or InStr(1, Name, "DROP", 1) > 0 Or InStr(1, Name, "INSERT", 1) > 1 Or InStr(1, Name, "TRACK", 1) > 1 Or InStr(1, Name, "TRACE", 1) > 1 Then
                    Label6.Text = " Do Not use Reserve Words... "
                    Me.txt_Com_Name.Focus()
                    Exit Sub
                End If
                Name = TrimAll(Name)
                'check unwanted characters
                c = 0
                counter2 = 0
                For iloop = 1 To Len(Name.ToString)
                    strcurrentchar = Mid(Name, iloop, 1)
                    If c = 0 Then
                        If Not InStr("'~!@#$^&*+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter2 = 1
                        End If
                    End If
                Next
                If counter2 = 1 Then
                    Label6.Text = " Do Not use un-wanted Characters... "
                    Me.txt_Com_Name.Focus()
                    Exit Sub
                End If

                '****************************************************************************************
                'Server Validation for committee chairman
                Dim Chairman As String = Nothing
                Chairman = TrimAll(txt_Com_Chairman.Text)
                If String.IsNullOrEmpty(Chairman) Then
                    Label6.Text = " Please Enter Data in this field... "
                    Me.txt_Com_Chairman.Focus()
                    Exit Sub
                End If
                Chairman = RemoveQuotes(Chairman)
                If Chairman.Length > 100 Then 'maximum length
                    Label6.Text = " Data must be of Proper Length.. "
                    txt_Com_Chairman.Focus()
                    Exit Sub
                End If
                Chairman = " " & Chairman & " "
                If InStr(1, Chairman, "CREATE", 1) > 0 Or InStr(1, Chairman, "DELETE", 1) > 0 Or InStr(1, Chairman, "DROP", 1) > 0 Or InStr(1, Chairman, "INSERT", 1) > 1 Or InStr(1, Chairman, "TRACK", 1) > 1 Or InStr(1, Chairman, "TRACE", 1) > 1 Then
                    Label6.Text = " Do Not use Reserve Words..."
                    Me.txt_Com_Chairman.Focus()
                    Exit Sub
                End If
                Chairman = TrimAll(Chairman)
                'check unwanted characters
                c = 0
                counter3 = 0
                For iloop = 1 To Len(Chairman.ToString)
                    strcurrentchar = Mid(Chairman, iloop, 1)
                    If c = 0 Then
                        If Not InStr("'~!@#$^&*+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter3 = 1
                        End If
                    End If
                Next
                If counter3 = 1 Then
                    Label6.Text = " Do Not use un-wanted Characters..."
                    Me.txt_Com_Chairman.Focus()
                    Exit Sub
                End If

                '****************************************************************************************
                'Server Validation for committee members
                Dim Member As String = Nothing
                Member = TrimAll(txt_Com_Members.Text)
                If String.IsNullOrEmpty(Member) Then
                    Label6.Text = " Please Enter Data in this field... "
                    Me.txt_Com_Members.Focus()
                    Exit Sub
                End If
                Member = RemoveQuotes(Member)
                If Chairman.Length > 250 Then 'maximum length
                    Label6.Text = " Data must be of Proper Length.. "
                    txt_Com_Members.Focus()
                    Exit Sub
                End If
                Member = " " & Member & " "
                If InStr(1, Member, "CREATE", 1) > 0 Or InStr(1, Member, "DELETE", 1) > 0 Or InStr(1, Member, "DROP", 1) > 0 Or InStr(1, Member, "INSERT", 1) > 1 Or InStr(1, Member, "TRACK", 1) > 1 Or InStr(1, Member, "TRACE", 1) > 1 Then
                    Label6.Text = " Do Not use Reserve Words... "
                    Me.txt_Com_Members.Focus()
                    Exit Sub
                End If
                Member = TrimAll(Member)
                'check unwanted characters
                c = 0
                counter4 = 0
                For iloop = 1 To Len(Member.ToString)
                    strcurrentchar = Mid(Member, iloop, 1)
                    If c = 0 Then
                        If Not InStr("'~!@#$^&*+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter4 = 1
                        End If
                    End If
                Next
                If counter4 = 1 Then
                    Label6.Text = " Do Not use un-wanted Characters... "
                    Me.txt_Com_Members.Focus()
                    Exit Sub
                End If

                '****************************************************************************************
                'Server Validation for E-Mail Address
                Dim email As String
                email = TrimX(txt_Com_Email.Text)
                If Not String.IsNullOrEmpty(email) Then
                    email = RemoveQuotes(email)
                    If email.Length > 200 Then
                        Label6.Text = " Input is not Valid.."
                        Me.txt_Com_Email.Focus()
                        Exit Sub
                    End If
                    email = " " & email & " "
                    If InStr(1, email, "CREATE", 1) > 0 Or InStr(1, email, "DELETE", 1) > 0 Or InStr(1, email, "DROP", 1) > 0 Or InStr(1, email, "INSERT", 1) > 1 Or InStr(1, email, "TRACK", 1) > 1 Or InStr(1, email, "TRACE", 1) > 1 Then
                        Label6.Text = "  Input is not Valid... "
                        Me.txt_Com_Email.Focus()
                        Exit Sub
                    End If
                    email = TrimX(email)
                    'check unwanted characters
                    c = 0
                    Counter5 = 0
                    For iloop = 1 To Len(email)
                        strcurrentchar = Mid(email, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';~!#$^&*|[]{}?<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                Counter5 = 1
                            End If
                        End If
                    Next
                    If Counter5 = 1 Then
                        Label6.Text = "  Input is not Valid..."
                        Me.txt_Com_Email.Focus()
                        Exit Sub
                    End If

                Else
                    email = ""
                End If
                '****************************************************************************************88
                'remareks
                Dim myRemarks As String = Nothing
                myRemarks = TrimAll(Me.txt_Com_Remarks.Text)
                If Not String.IsNullOrEmpty(myRemarks) Then
                    myRemarks = RemoveQuotes(myRemarks)
                    If myRemarks.Length > 250 Then
                        Label6.Text = " Input is not Valid... "
                        Me.txt_Com_Remarks.Focus()
                        Exit Sub
                    End If
                    myRemarks = TrimAll(myRemarks)
                    'check unwanted characters
                    c = 0
                    counter6 = 0
                    For iloop = 1 To Len(myRemarks)
                        strcurrentchar = Mid(myRemarks, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter6 = 1
                            End If
                        End If
                    Next
                    If counter6 = 1 Then
                        Label6.Text = " Input is not Valid..."
                        Me.txt_Com_Remarks.Focus()
                        Exit Sub
                    End If
                Else
                    myRemarks = String.Empty
                End If

                'search start date
                Dim SDate As Object = Nothing
                If txt_Com_SDate.Text <> "" Then
                    SDate = TrimAll(txt_Com_SDate.Text)
                    SDate = RemoveQuotes(SDate)
                    SDate = Convert.ToDateTime(SDate, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us
                    'DateFrom = Convert.ToDateTime(DateFrom, System.Globalization.CultureInfo.GetCultureInfo("en-US")).ToString("MM/dd/yyyy") ' convert from us to india
                    ' DateFrom = Convert.ToDateTime(DateTime.Now, System.Globalization.CultureInfo.CurrentCulture).ToString("MM/dd/yyyy hh:MM:ss")
                    'DateFrom = Convert.ToDateTime(DateFrom, System.Globalization.CultureInfo.CurrentCulture).ToString("MM/dd/yyyy hh:MM:ss")

                    If Len(SDate) > 12 Then
                        Label6.Text = " Input is not Valid..."
                        Me.txt_Com_SDate.Focus()
                        Exit Sub
                    End If
                    SDate = " " & SDate & " "
                    If InStr(1, SDate, "CREATE", 1) > 0 Or InStr(1, SDate, "DELETE", 1) > 0 Or InStr(1, SDate, "DROP", 1) > 0 Or InStr(1, SDate, "INSERT", 1) > 1 Or InStr(1, SDate, "TRACK", 1) > 1 Or InStr(1, SDate, "TRACE", 1) > 1 Then
                        Label6.Text = "  Input is not Valid... "
                        Me.txt_Com_SDate.Focus()
                        Exit Sub
                    End If
                    SDate = TrimX(SDate)
                    'check unwanted characters
                    c = 0
                    counter7 = 0
                    For iloop = 1 To Len(SDate)
                        strcurrentchar = Mid(SDate, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter7 = 1
                            End If
                        End If
                    Next
                    If counter7 = 1 Then
                        Label6.Text = "data is not Valid... "
                        Me.txt_Com_SDate.Focus()
                        Exit Sub
                    End If
                Else
                    SDate = String.Empty
                End If

                'search end date
                Dim EDate As Object = Nothing
                If txt_Com_EDate.Text <> "" Then
                    EDate = TrimX(txt_Com_EDate.Text)
                    EDate = RemoveQuotes(EDate)
                    EDate = Convert.ToDateTime(EDate, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                    If EDate.Length > 12 Then
                        Label6.Text = "Input is not Valid... "
                        Me.txt_Com_EDate.Focus()
                        Exit Sub
                    End If
                    EDate = " " & EDate & " "
                    If InStr(1, EDate, "CREATE", 1) > 0 Or InStr(1, EDate, "DELETE", 1) > 0 Or InStr(1, EDate, "DROP", 1) > 0 Or InStr(1, EDate, "INSERT", 1) > 1 Or InStr(1, EDate, "TRACK", 1) > 1 Or InStr(1, EDate, "TRACE", 1) > 1 Then
                        Label6.Text = " Input is not Valid..."
                        Me.txt_Com_EDate.Focus()
                        Exit Sub
                    End If
                    EDate = TrimX(EDate)
                    'check unwanted characters
                    c = 0
                    counter8 = 0
                    For iloop = 1 To Len(EDate)
                        strcurrentchar = Mid(EDate, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter8 = 1
                            End If
                        End If
                    Next
                    If counter8 = 1 Then
                        Label6.Text = " data is not Valid..."
                        Me.txt_Com_EDate.Focus()
                        Exit Sub
                    End If
                Else
                    EDate = String.Empty
                End If


                '****************************************************************************************************
                'UPDATE THE LIBRARY PROFILE  
                If Label7.Text <> "" Then
                    SQL = "SELECT * FROM COMMITTEES WHERE (COM_ID='" & Trim(Label7.Text) & "')"
                    SqlConn.Open()
                    da = New SqlDataAdapter(SQL, SqlConn)
                    cb = New SqlCommandBuilder(da)
                    da.Fill(ds, "COMMITTEES")
                    If ds.Tables("COMMITTEES").Rows.Count <> 0 Then
                        If Not String.IsNullOrEmpty(Name) Then
                            ds.Tables("COMMITTEES").Rows(0)("COM_NAME") = Name.Trim
                        Else
                            ds.Tables("COMMITTEES").Rows(0)("COM_NAME") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(Chairman) Then
                            ds.Tables("COMMITTEES").Rows(0)("COM_CHAIRMAN") = Chairman.Trim
                        Else
                            ds.Tables("COMMITTEES").Rows(0)("COM_CHAIRMAN") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(Member) Then
                            ds.Tables("COMMITTEES").Rows(0)("COM_MEMB") = Member
                        Else
                            ds.Tables("COMMITTEES").Rows(0)("COM_MEMB") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(myRemarks) Then
                            ds.Tables("COMMITTEES").Rows(0)("COM_DESC") = myRemarks.Trim
                        Else
                            ds.Tables("COMMITTEES").Rows(0)("COM_DESC") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(email) Then
                            ds.Tables("COMMITTEES").Rows(0)("COM_EMAIL") = email.Trim
                        Else
                            ds.Tables("COMMITTEES").Rows(0)("COM_EMAIL") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(SDate) Then
                            ds.Tables("COMMITTEES").Rows(0)("START_DATE") = SDate.ToString.Trim
                        Else
                            ds.Tables("COMMITTEES").Rows(0)("START_DATE") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(EDate) Then
                            ds.Tables("COMMITTEES").Rows(0)("END_DATE") = EDate.ToString.Trim
                        Else
                            ds.Tables("COMMITTEES").Rows(0)("END_DATE") = System.DBNull.Value
                        End If
                        ds.Tables("COMMITTEES").Rows(0)("UPDATED_BY") = UserCode.Trim
                        ds.Tables("COMMITTEES").Rows(0)("DATE_MODIFIED") = Now.Date
                        ds.Tables("COMMITTEES").Rows(0)("IP") = Request.UserHostAddress.Trim

                        thisTransaction = SqlConn.BeginTransaction()
                        da.SelectCommand.Transaction = thisTransaction
                        da.Update(ds, "COMMITTEES")
                        thisTransaction.Commit()

                        Label6.Visible = True
                        Label6.Text = "User Record Updated Successfully"
                        Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Record updated Successfully !');", True)
                        ClearFields()
                    Else
                        Label6.Text = "Committee Not Updated  - Please Contact System Administrator... "
                        Exit Sub
                    End If
                End If
            Else
                'record not selected
                Label6.Text = "Record Not Selected..."
                Exit Sub
            End If
            SqlConn.Close()
            Me.PopulateGrid()
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
            For i = 0 To Grid_Committee.Rows.Count - 1
                If DirectCast(Grid_Committee.Rows(i).Cells(0).FindControl("cbd"), CheckBox).Checked Then
                    Dim ComCode As Object = Nothing
                    ComCode = Grid_Committee.Rows(i).Cells(2).Text
                    'chk for foreign reference in ACQ tble
                    Dim str As Object = Nothing
                    Dim flag As Object = Nothing
                    str = "SELECT COM_CODE FROM ACQUISITIONS WHERE (COM_CODE ='" & Trim(ComCode) & "')  AND (LIB_CODE = '" & Trim(LibCode) & "')"
                    Dim cmd1 As New SqlCommand(str, SqlConn)
                    SqlConn.Open()
                    flag = cmd1.ExecuteScalar
                    SqlConn.Close()
                    If flag <> Nothing Then
                        Label6.Text = "Committee reference saved..in Acquisition Table, can not be deleted)"
                        Me.txt_Com_Code.Focus()
                    Else
                        Dim COM_ID As Integer = Convert.ToInt32(Grid_Committee.Rows(i).Cells(6).Text)
                        'get cat record
                        Dim SQL As String = Nothing
                        SQL = "DELETE FROM COMMITTEES WHERE (COM_ID ='" & Trim(COM_ID) & "') "
                        SqlConn.Open()
                        Dim objCommand As New SqlCommand(SQL, SqlConn)
                        Dim da As New SqlDataAdapter(objCommand)
                        Dim ds As New DataSet
                        da.Fill(ds)
                        SqlConn.Close()
                    End If
                End If
            Next
            PopulateGrid()
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
End Class