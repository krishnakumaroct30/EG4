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
Public Class Holidays
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
                        Label6.Text = "Select / De-Select Date from Calendar and Press SAVE Button to save the record(s).."
                        PopulateYears()
                    End If
                End If
                Dim CreateSUserButton As Object 'System.Web.UI.WebControls.Button
                CreateSUserButton = Master.FindControl("Accordion1").FindControl("MasterPane").FindControl("M_Holidays_Bttn")
                CreateSUserButton.ForeColor = Drawing.Color.Red
                myPaneName = "MasterPane" ' paneSelectedIndex = 1
            Else
                Response.Redirect("~/Default.aspx", False)
            End If
            LibCode = Session.Item("LoggedLibcode")
            If Page.IsPostBack AndAlso Calendar1.SelectedDates.Count = 1 Then
                Calendar1.SelectedDates.Clear()
            End If

        Catch ex As Exception
            Label6.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    Private Sub Holidays_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        DropDownList_Year.Focus()
    End Sub
    'fill calander
    Protected Sub Calendar1_DayRender(ByVal sender As Object, ByVal e As DayRenderEventArgs)
        If e.Day.IsOtherMonth = True Then
            e.Cell.ForeColor = Drawing.Color.Black
            e.Cell.Enabled = False
        Else
            Dim Command As SqlCommand = Nothing
            Dim dt As DataTable = Nothing
            Command = New SqlCommand("SELECT DISTINCT HOLI_YEAR, HOLI_DATE FROM HOLIDAYS WHERE (LIB_CODE = '" & Trim(LibCode) & "') AND (HOLI_DATE= '" & Trim(e.Day.Date) & "') ", SqlConn)

            SqlConn.Open()
            Dim da As New SqlDataAdapter(Command)
            Dim ds As New DataSet
            Dim RecordCount As Long = 0
            da.Fill(ds)

            dt = ds.Tables(0).Copy
            If dt.Rows.Count <> 0 Then
                e.Cell.ForeColor = Drawing.Color.Tomato
                e.Cell.Enabled = False

                Dim CheckBox1 As New CheckBox()
                CheckBox1.Checked = True
                CheckBox1.Width = 25
                CheckBox1.Enabled = False
                e.Cell.Controls.AddAt(1, CheckBox1)
                e.Cell.Font.Size = FontUnit.XLarge
                e.Cell.Font.Bold = True
                e.Cell.ForeColor = Drawing.Color.Blue
            End If
            dt.Dispose()
            SqlConn.Close()
        End If



            If e.Day.IsSelected Then
                If e.Day.IsOtherMonth = False Then
                    Dim CheckBox1 As New CheckBox()
                    CheckBox1.Checked = True
                    CheckBox1.Width = 25
                    CheckBox1.Enabled = False
                    e.Cell.Controls.AddAt(1, CheckBox1)
                    e.Cell.Font.Size = FontUnit.XLarge
                    e.Cell.Font.Bold = True
                    e.Cell.ForeColor = Drawing.Color.Blue
                End If
            End If

           
    End Sub
    Protected Sub Calendar1_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim li As New ListItem()
        li.Text = Calendar1.SelectedDate.ToShortDateString()

        Dim itemCounter As Integer = 0
        For Each litem As ListItem In CheckBoxList2.Items
            If litem.Text = li.Text Then
                itemCounter += 1
            End If
        Next

        If itemCounter > 0 Then
            CheckBoxList2.Items.Remove(li)
        Else
            CheckBoxList2.Items.Add(li)
        End If


        Calendar1.SelectedDates.Clear()
        Dim dates As SelectedDatesCollection = Calendar1.SelectedDates

        For Each litem As ListItem In CheckBoxList2.Items
            Dim [date] As DateTime = Convert.ToDateTime(litem.Text)
            dates.Add([date])
            litem.Selected = True
            litem.Enabled = False
        Next
        ' End If
    End Sub
    'hoq to select a date in calnder
    Protected Sub Save_Click(ByVal sender As Object, ByVal e As EventArgs) Handles bttn_Save.Click
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing
        Try
            If IsPostBack = True Then
                Dim myArray As New ArrayList()
                For Each item As ListItem In CheckBoxList2.Items
                    If item.Selected = True Then
                        'myArray.Add(item)

                        'save record in table
                        Dim iloop As Integer
                        Dim strcurrentchar As Object
                        Dim c As Integer
                        Dim counter1 As Integer
                        'Dim thisTransaction As SqlClient.SqlTransaction = Nothing

                        Dim myHoliDate As Date = Nothing
                        If item.ToString <> "" Then
                            myHoliDate = item.ToString

                            If Len(myHoliDate) > 12 Then
                                Label6.Text = " Input is not Valid..."
                                Exit Sub
                            End If
                            myHoliDate = " " & myHoliDate & " "
                            If InStr(1, myHoliDate, "CREATE", 1) > 0 Or InStr(1, myHoliDate, "DELETE", 1) > 0 Or InStr(1, myHoliDate, "DROP", 1) > 0 Or InStr(1, myHoliDate, "INSERT", 1) > 1 Or InStr(1, myHoliDate, "TRACK", 1) > 1 Or InStr(1, myHoliDate, "TRACE", 1) > 1 Then
                                Label6.Text = "  Input is not Valid... "
                                Exit Sub
                            End If
                            myHoliDate = TrimX(myHoliDate)
                            'check unwanted characters
                            c = 0
                            counter1 = 0
                            For iloop = 1 To Len(myHoliDate)
                                strcurrentchar = Mid(myHoliDate, iloop, 1)
                                If c = 0 Then
                                    If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                        c = c + 1
                                        counter1 = 1
                                    End If
                                End If
                            Next
                            If counter1 = 1 Then
                                Label6.Text = "data is not Valid... "
                                Exit Sub
                            End If
                        Else
                            myHoliDate = String.Empty
                        End If

                        Dim myYear As Integer = Nothing
                        If Not IsNothing(myHoliDate) Then
                            myYear = myHoliDate.Year
                        Else
                            myYear = Today.Year
                        End If

                        'check duplicate date
                        Dim str As Object = Nothing
                        Dim flag As Object = Nothing
                        str = "SELECT HOLI_ID FROM HOLIDAYS WHERE (LIB_CODE = '" & Trim(LibCode) & "' AND HOLI_DATE ='" & Trim(myHoliDate) & "')"
                        Dim cmd1 As New SqlCommand(str, SqlConn)
                        SqlConn.Open()
                        flag = cmd1.ExecuteScalar
                        If flag <> Nothing Then
                            SqlConn.Close()
                            Continue For 'Exit Sub
                        End If
                        SqlConn.Close()


                        'INSERT THE RECORD IN TO THE DATABASE
                        Dim SQL As String
                        Dim Cmd As SqlCommand
                        Dim da As SqlDataAdapter
                        Dim ds As New DataSet
                        Dim CB As SqlCommandBuilder
                        Dim dtrow As DataRow
                        SQL = "SELECT * FROM HOLIDAYS WHERE (HOLI_ID = '00')"
                        Cmd = New SqlCommand(SQL, SqlConn)
                        da = New SqlDataAdapter(Cmd)
                        CB = New SqlCommandBuilder(da)
                        SqlConn.Open()

                        da.Fill(ds, "HOLIDAYS")
                        dtrow = ds.Tables("HOLIDAYS").NewRow
                        If Not String.IsNullOrEmpty(myHoliDate) Then
                            dtrow("HOLI_DATE") = myHoliDate
                        Else
                            dtrow("HOLI_DATE") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(myYear) Then
                            dtrow("HOLI_YEAR") = myYear
                        Else
                            dtrow("HOLI_YEAR") = System.DBNull.Value
                        End If
                        dtrow("LIB_CODE") = LibCode
                        dtrow("USER_CODE") = Session.Item("LoggedUser")
                        dtrow("DATE_ADDED") = Now.Date
                        dtrow("IP") = Request.UserHostAddress.Trim

                        ds.Tables("HOLIDAYS").Rows.Add(dtrow)

                        thisTransaction = SqlConn.BeginTransaction()
                        da.SelectCommand.Transaction = thisTransaction
                        da.Update(ds, "HOLIDAYS")
                        thisTransaction.Commit()

                        myHoliDate = Nothing
                        ds.Dispose()
                        SqlConn.Close()
                        Label6.Text = "Record(s) Added Successfully!"
                    Else
                        Label6.Text = "No Dates Selected to Added!"
                    End If
                Next
                Calendar1.SelectedDates.Clear()
                CheckBoxList2.Items.Clear()
                bttn_Save.Visible = True
                bttn_Update.Visible = False
            End If
            UpdatePanel1.Update()
        Catch q As SqlException
            thisTransaction.Rollback()
        Catch ex As Exception
            Label6.Text = ex.Message
        Finally
            SqlConn.Close()
            PopulateGrid()
        End Try
    End Sub
    'populate bib levels
    Public Sub PopulateYears()
        Dim Command As SqlCommand = Nothing
        Dim dt As DataTable = Nothing
        Try
            Command = New SqlCommand("SELECT DISTINCT HOLI_YEAR FROM HOLIDAYS WHERE (LIB_CODE = '" & Trim(LibCode) & "')  ORDER BY HOLI_YEAR DESC ", SqlConn)
            SqlConn.Open()
            Dim da As New SqlDataAdapter(Command)
            Dim ds As New DataSet
            Dim RecordCount As Long = 0
            da.Fill(ds)

            dt = ds.Tables(0).Copy

            Dim Dr As DataRow
            Dr = dt.NewRow
            'dt.Rows.InsertAt(Dr, 0)

            If dt.Rows.Count = 0 Then
                Me.DropDownList_Year.DataSource = Nothing
            Else
                Me.DropDownList_Year.DataSource = dt
                Me.DropDownList_Year.DataTextField = "HOLI_YEAR"
                Me.DropDownList_Year.DataValueField = "HOLI_YEAR"
                Me.DropDownList_Year.DataBind()
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
    Public Sub Cancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Cancel.Click
        bttn_Save.Visible = True
        bttn_Update.Visible = False
        Label6.Text = "Select / De-Select Date from Calendar and Press SAVE Button to save the record(s).."
    End Sub
    Private Sub DropDownList_Year_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownList_Year.SelectedIndexChanged
        PopulateGrid()
    End Sub
    'Populate the record in grid     'search users
    Public Sub PopulateGrid()
        Dim dt As DataTable = Nothing
        Try
            Dim iloop As Integer
            Dim strcurrentchar As Object
            Dim c As Integer
            Dim counter1 As Integer

            Dim mySelectedYear As Integer = Nothing
            If DropDownList_Year.Text <> "" Then
                mySelectedYear = DropDownList_Year.SelectedValue

                If Len(mySelectedYear) > 4 Then
                    Label6.Text = " Input is not Valid..."
                    Exit Sub
                End If
                mySelectedYear = " " & mySelectedYear & " "
                If InStr(1, mySelectedYear, "CREATE", 1) > 0 Or InStr(1, mySelectedYear, "DELETE", 1) > 0 Or InStr(1, mySelectedYear, "DROP", 1) > 0 Or InStr(1, mySelectedYear, "INSERT", 1) > 1 Or InStr(1, mySelectedYear, "TRACK", 1) > 1 Or InStr(1, mySelectedYear, "TRACE", 1) > 1 Then
                    Label6.Text = "  Input is not Valid... "
                    Exit Sub
                End If
                mySelectedYear = TrimX(mySelectedYear)
                'check unwanted characters
                c = 0
                counter1 = 0
                For iloop = 1 To Len(mySelectedYear)
                    strcurrentchar = Mid(mySelectedYear, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter1 = 1
                        End If
                    End If
                Next
                If counter1 = 1 Then
                    Label6.Text = "data is not Valid... "
                    Exit Sub
                End If
            Else
                mySelectedYear = 0
            End If

            Dim SQL As String = Nothing
            If mySelectedYear = 0 Then
                SQL = "SELECT * FROM HOLIDAYS WHERE  (LIB_CODE='" & Trim(LibCode) & "') ORDER BY HOLI_YEAR DESC, HOLI_DATE ASC  "
            Else
                SQL = "SELECT * FROM HOLIDAYS WHERE  (LIB_CODE='" & Trim(LibCode) & "' AND HOLI_YEAR = '" & Trim(mySelectedYear) & "') ORDER BY HOLI_YEAR DESC, HOLI_DATE ASC  "
            End If
            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            da.Fill(ds)

            dt = ds.Tables(0).Copy
            Dim RecordCount As Long = 0
            If dt.Rows.Count = 0 Then
                Me.Grid_Holidays.DataSource = Nothing
                Grid_Holidays.DataBind()
                Label1.Text = "Total Record(s): 0 "
                Delete_Bttn.Enabled = False
            Else
                Grid_Holidays.Visible = True
                RecordCount = dt.Rows.Count
                Grid_Holidays.DataSource = dt
                Grid_Holidays.DataBind()
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
    Protected Sub Grid1_Holidays_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles Grid_Holidays.PageIndexChanging
        Try
            'rebind datagrid
            Grid_Holidays.DataSource = ViewState("dt") 'temp
            Grid_Holidays.PageIndex = e.NewPageIndex
            index = e.NewPageIndex * Grid_Holidays.PageSize
            Grid_Holidays.DataBind()

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
    Protected Sub Grid_Holidays_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles Grid_Holidays.Sorting
        Dim temp As DataTable = CType(ViewState("dt"), DataTable)
        GridViewSortExpression = e.SortExpression
        Grid_Holidays.DataSource = temp
        Dim pageIndex As Integer = Grid_Holidays.PageIndex
        Grid_Holidays.DataSource = SortDataTable(Grid_Holidays.DataSource, False)
        Grid_Holidays.DataBind()
        Grid_Holidays.PageIndex = pageIndex
        UpdatePanel1.Update()
    End Sub
    Protected Sub Grid_Holidays_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Grid_Holidays.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Lblsr As Label = e.Row.FindControl("lblsr")
            Srno = index + 1
            If Lblsr Is Nothing = False Then
                Lblsr.Text = Srno
                index += 1
            End If
            e.Row.Attributes("onmouseover") = "this.style.cursor='hand';this.style.textDecoration='bold';this.style.background='#FFDFDF'"
            e.Row.Attributes("onmouseout") = "this.style.textDecoration='none';this.style.background='none'"
            e.Row.Attributes("onclick") = ClientScript.GetPostBackClientHyperlink(Me, "Select$" & Convert.ToString(e.Row.RowIndex))
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
            For Each row As GridViewRow In Grid_Holidays.Rows
                Dim cb As CheckBox = DirectCast(row.FindControl("cbd"), CheckBox)
                If cb IsNot Nothing AndAlso cb.Checked = True Then
                    Dim HoliID As Integer = Convert.ToInt32(Grid_Holidays.DataKeys(row.RowIndex).Value)

                    'get cat record
                    Dim SQL As String = Nothing
                    SQL = "DELETE FROM HOLIDAYS WHERE (HOLI_ID ='" & Trim(HoliID) & "') "
                    Dim objCommand As New SqlCommand(SQL, SqlConn)
                    Dim da As New SqlDataAdapter(objCommand)
                    Dim ds As New DataSet
                    da.Fill(ds)
                End If
            Next
            Dim s As Object = Nothing
            Dim e1 As EventArgs = Nothing
            PopulateGrid()
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub


End Class