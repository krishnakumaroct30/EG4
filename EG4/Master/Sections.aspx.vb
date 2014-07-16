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
Public Class Sections
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
                CreateSUserButton = Master.FindControl("Accordion1").FindControl("MasterPane").FindControl("M_Section_Bttn")
                CreateSUserButton.ForeColor = Drawing.Color.Red
                myPaneName = "MasterPane" ' paneSelectedIndex = 1
            Else
                Response.Redirect("~/Default.aspx", False)
            End If
            LibCode = Session.Item("LoggedLibcode")
        Catch ex As Exception
            Label6.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    Private Sub SECTIONS_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        txt_Sec_Code.Focus()
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
                Code = TrimX(UCase(txt_Sec_Code.Text))
                If String.IsNullOrEmpty(Code) Then
                    Label6.Text = "Plz Enter Data in this Field !"
                    Me.txt_Sec_Code.Focus()
                    Exit Sub
                End If
                Code = RemoveQuotes(Code)
                If Code.Length > 5 Then 'maximum length
                    Label6.Text = "Data must be of proper Length !"
                    txt_Sec_Code.Focus()
                    Exit Sub
                End If
                If Code.Length < 1 Then ' minimum length
                    Label6.Text = "Data must be or proper Length"
                    txt_Sec_Code.Focus()
                    Exit Sub
                End If
                Code = " " & Code & " "
                If InStr(1, Code, "CREATE", 1) > 0 Or InStr(1, Code, "DELETE", 1) > 0 Or InStr(1, Code, "DROP", 1) > 0 Or InStr(1, Code, "INSERT", 1) > 1 Or InStr(1, Code, "TRACK", 1) > 1 Or InStr(1, Code, "TRACE", 1) > 1 Then
                    Label6.Text = "Do Not use Reserve Words..."
                    Me.txt_Sec_Code.Focus()
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
                    Me.txt_Sec_Code.Focus()
                    Exit Sub
                End If

                'Check Duplicate comm Code
                Dim str As Object = Nothing
                Dim flag As Object = Nothing
                str = "SELECT SEC_ID FROM SECTIONS WHERE (SEC_CODE ='" & Trim(Code) & "') AND (LIB_CODE = '" & Trim(LibCode) & "')"
                Dim cmd1 As New SqlCommand(str, SqlConn)
                SqlConn.Open()
                flag = cmd1.ExecuteScalar
                If flag <> Nothing Then
                    Label6.Text = "Code Already exists..Enter another code)"
                    Me.txt_Sec_Code.Focus()
                    Exit Sub
                End If
                SqlConn.Close()

                '****************************************************************************************
                'Server Validation for section name
                Dim Name As String = Nothing
                Name = TrimAll(txt_Sec_Name.Text)
                If String.IsNullOrEmpty(Name) Then
                    Label6.Text = " Please Enter Data in this field... "
                    Me.txt_Sec_Name.Focus()
                    Exit Sub
                End If
                Name = RemoveQuotes(Name)
                If Name.Length > 100 Then 'maximum length
                    Label6.Text = " Data must be of Proper Length.. "
                    txt_Sec_Name.Focus()
                    Exit Sub
                End If
                Name = " " & Name & " "
                If InStr(1, Name, "CREATE", 1) > 0 Or InStr(1, Name, "DELETE", 1) > 0 Or InStr(1, Name, "DROP", 1) > 0 Or InStr(1, Name, "INSERT", 1) > 1 Or InStr(1, Name, "TRACK", 1) > 1 Or InStr(1, Name, "TRACE", 1) > 1 Then
                    Label6.Text = " Do Not use Reserve Words... "
                    Me.txt_Sec_Name.Focus()
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
                    Me.txt_Sec_Name.Focus()
                    Exit Sub
                End If


                '****************************************************************************************88
                'remareks
                Dim myRemarks As String = Nothing
                myRemarks = TrimAll(Me.txt_Sec_Remarks.Text)
                If Not String.IsNullOrEmpty(myRemarks) Then
                    myRemarks = RemoveQuotes(myRemarks)
                    If myRemarks.Length > 250 Then
                        Label6.Text = " Input is not Valid... "
                        Me.txt_Sec_Remarks.Focus()
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
                        Me.txt_Sec_Remarks.Focus()
                        Exit Sub
                    End If
                Else
                    myRemarks = String.Empty
                End If

                'INSERT THE RECORD IN TO THE DATABASE
                Dim SQL As String
                Dim Cmd As SqlCommand
                Dim da As SqlDataAdapter
                Dim ds As New DataSet
                Dim CB As SqlCommandBuilder
                Dim dtrow As DataRow
                SQL = "SELECT * FROM SECTIONS WHERE (SEC_ID = '00')"
                Cmd = New SqlCommand(SQL, SqlConn)
                da = New SqlDataAdapter(Cmd)
                CB = New SqlCommandBuilder(da)
                SqlConn.Open()
                da.Fill(ds, "SECTIONS")
                dtrow = ds.Tables("SECTIONS").NewRow

                If Not String.IsNullOrEmpty(Code) Then
                    dtrow("SEC_CODE") = Code.Trim
                End If

                If Not String.IsNullOrEmpty(Name) Then
                    dtrow("SEC_NAME") = Name.Trim
                End If

                If Not String.IsNullOrEmpty(myRemarks) Then
                    dtrow("SEC_DESC") = myRemarks.Trim
                Else
                    dtrow("SEC_DESC") = System.DBNull.Value
                End If

                dtrow("LIB_CODE") = LibCode
                dtrow("USER_CODE") = Session.Item("LoggedUser")
                dtrow("DATE_ADDED") = Now.Date
                dtrow("IP") = Request.UserHostAddress.Trim

                ds.Tables("SECTIONS").Rows.Add(dtrow)

                thisTransaction = SqlConn.BeginTransaction()
                da.SelectCommand.Transaction = thisTransaction
                da.Update(ds, "SECTIONS")
                thisTransaction.Commit()
                ClearFields()
                Code = Nothing
                Name = Nothing
                myRemarks = Nothing
               
                ds.Dispose()
                Label6.Text = "Record Added Successfully!"
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
        txt_Sec_Code.Text = ""
        txt_Sec_Name.Text = ""
        txt_Sec_Remarks.Text = ""
        txt_Sec_Code.Enabled = True
    End Sub
    'Populate the users in grid     'search users
    Public Sub PopulateGrid()

        Dim dt As DataTable = Nothing
        Try
            Dim SQL As String = Nothing
            SQL = "SELECT * FROM SECTIONS WHERE  (LIB_CODE='" & Trim(LibCode) & "') ORDER BY SEC_NAME  "

            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            da.Fill(ds)

            dt = ds.Tables(0).Copy
            Dim RecordCount As Long = 0
            If dt.Rows.Count = 0 Then
                Me.Grid_Section.DataSource = Nothing
                Grid_Section.DataBind()
                Label1.Text = "Total Record(s): 0 "
                Delete_Bttn.Enabled = False
            Else
                Grid_Section.Visible = True
                RecordCount = dt.Rows.Count
                Grid_Section.DataSource = dt
                Grid_Section.DataBind()
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
    Protected Sub Grid1_Section_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles Grid_Section.PageIndexChanging
        Try
            'rebind datagrid
            Grid_Section.DataSource = ViewState("dt") 'temp
            Grid_Section.PageIndex = e.NewPageIndex
            index = e.NewPageIndex * Grid_Section.PageSize
            Grid_Section.DataBind()
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
    Protected Sub Grid_Section_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles Grid_Section.Sorting
        Dim temp As DataTable = CType(ViewState("dt"), DataTable)
        GridViewSortExpression = e.SortExpression
        Grid_Section.DataSource = temp
        Dim pageIndex As Integer = Grid_Section.PageIndex
        Grid_Section.DataSource = SortDataTable(Grid_Section.DataSource, False)
        Grid_Section.DataBind()
        Grid_Section.PageIndex = pageIndex
        UpdatePanel1.Update()
    End Sub
    Protected Sub Grid_Section_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Grid_Section.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Lblsr As Label = e.Row.FindControl("lblsr")
            Srno = index + 1
            If Lblsr Is Nothing = False Then
                Lblsr.Text = Srno
                index += 1
            End If
            e.Row.Attributes("onmouseover") = "this.style.cursor='hand';this.style.textDecoration='bold';this.style.background='#FFDFDF'"
            e.Row.Attributes("onmouseout") = "this.style.textDecoration='none';this.style.background='none'"
            ' e.Row.Attributes("onclick") = ClientScript.GetPostBackClientHyperlink(Me, "Select$" & Convert.ToString(e.Row.RowIndex))
        End If
    End Sub
    Private Shared Function t() As Object
        Throw New NotImplementedException
    End Function
    'get value of row from grid
    Private Sub Grid_Section_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles Grid_Section.RowCommand
        Dim Command As SqlCommand = Nothing
        Dim dr As SqlDataReader = Nothing
        Try
            If e.CommandName = "Select" Then
                Dim myRowID, SecID As Integer
                myRowID = e.CommandArgument.ToString()

                If Grid_Section.Rows(myRowID).Cells(5).Text <> "" Then
                    SecID = Grid_Section.Rows(myRowID).Cells(5).Text
                    Label7.Text = SecID
                Else
                    SecID = ""
                    Label7.Text = ""
                End If

                Dim iloop As Integer
                Dim strcurrentchar As Object
                Dim c As Integer
                Dim counter1 As Integer

                SecID = TrimX(SecID)
                If Not String.IsNullOrEmpty(SecID) Then
                    SecID = RemoveQuotes(SecID)
                    If Len(SecID).ToString > 10 Then
                        Label6.Text = "Length of Input is not Proper... "
                        Exit Sub
                    End If
                    SecID = " " & SecID & " "
                    If InStr(1, SecID, " CREATE ", 1) > 0 Or InStr(1, SecID, " DELETE ", 1) > 0 Or InStr(1, SecID, " DROP ", 1) > 0 Or InStr(1, SecID, " INSERT ", 1) > 1 Or InStr(1, SecID, " TRACK ", 1) > 1 Or InStr(1, SecID, " TRACE ", 1) > 1 Then
                        Label6.Text = "Do not use reserve words..."
                        Exit Sub
                    End If
                    SecID = TrimX(SecID)
                Else
                    Label6.Text = "Select Record... "
                    Exit Sub
                End If

                'get record details from database
                Dim SQL As String = Nothing
                SQL = " SELECT *  FROM SECTIONS WHERE (SEC_ID = '" & Trim(SecID) & "') "
                Command = New SqlCommand(SQL, SqlConn)
                SqlConn.Open()
                dr = Command.ExecuteReader(CommandBehavior.CloseConnection)
                dr.Read()
                ClearFields()
                txt_Sec_Code.Enabled = False
                If dr.HasRows = True Then
                    If dr.Item("SEC_CODE").ToString <> "" Then
                        Me.txt_Sec_Code.Text = dr.Item("SEC_CODE").ToString
                    Else
                        txt_Sec_Code.Text = ""
                    End If
                    If dr.Item("SEC_NAME").ToString <> "" Then
                        Me.txt_Sec_Name.Text = dr.Item("SEC_NAME").ToString
                    Else
                        Me.txt_Sec_Name.Text = ""
                    End If
                  
                    If dr.Item("SEC_DESC").ToString <> "" Then
                        Me.txt_Sec_Remarks.Text = dr.Item("SEC_DESC").ToString
                    Else
                        Me.txt_Sec_Remarks.Text = ""
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
                Name = TrimAll(txt_Sec_Name.Text)
                If String.IsNullOrEmpty(Name) Then
                    Label6.Text = " Please Enter Data in this field... "
                    Me.txt_Sec_Name.Focus()
                    Exit Sub
                End If
                Name = RemoveQuotes(Name)
                If Name.Length > 100 Then 'maximum length
                    Label6.Text = " Data must be of Proper Length.. "
                    txt_Sec_Name.Focus()
                    Exit Sub
                End If
                Name = " " & Name & " "
                If InStr(1, Name, "CREATE", 1) > 0 Or InStr(1, Name, "DELETE", 1) > 0 Or InStr(1, Name, "DROP", 1) > 0 Or InStr(1, Name, "INSERT", 1) > 1 Or InStr(1, Name, "TRACK", 1) > 1 Or InStr(1, Name, "TRACE", 1) > 1 Then
                    Label6.Text = " Do Not use Reserve Words... "
                    Me.txt_Sec_Name.Focus()
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
                    Me.txt_Sec_Name.Focus()
                    Exit Sub
                End If

                '****************************************************************************************88
                'remareks
                Dim myRemarks As String = Nothing
                myRemarks = TrimAll(Me.txt_Sec_Remarks.Text)
                If Not String.IsNullOrEmpty(myRemarks) Then
                    myRemarks = RemoveQuotes(myRemarks)
                    If myRemarks.Length > 250 Then
                        Label6.Text = " Input is not Valid... "
                        Me.txt_Sec_Remarks.Focus()
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
                        Me.txt_Sec_Remarks.Focus()
                        Exit Sub
                    End If
                Else
                    myRemarks = String.Empty
                End If

                '****************************************************************************************************
                'UPDATE THE LIBRARY PROFILE  
                If Label7.Text <> "" Then
                    SQL = "SELECT * FROM SECTIONS WHERE (SEC_ID='" & Trim(Label7.Text) & "')"
                    SqlConn.Open()
                    da = New SqlDataAdapter(SQL, SqlConn)
                    cb = New SqlCommandBuilder(da)
                    da.Fill(ds, "SECTIONS")
                    If ds.Tables("SECTIONS").Rows.Count <> 0 Then
                        If Not String.IsNullOrEmpty(Name) Then
                            ds.Tables("SECTIONS").Rows(0)("SEC_NAME") = Name.Trim
                        Else
                            ds.Tables("SECTIONS").Rows(0)("SEC_NAME") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(myRemarks) Then
                            ds.Tables("SECTIONS").Rows(0)("SEC_DESC") = myRemarks.Trim
                        Else
                            ds.Tables("SECTIONS").Rows(0)("SEC_DESC") = System.DBNull.Value
                        End If
                        
                        ds.Tables("SECTIONS").Rows(0)("UPDATED_BY") = UserCode.Trim
                        ds.Tables("SECTIONS").Rows(0)("DATE_MODIFIED") = Now.Date
                        ds.Tables("SECTIONS").Rows(0)("IP") = Request.UserHostAddress.Trim

                        thisTransaction = SqlConn.BeginTransaction()
                        da.SelectCommand.Transaction = thisTransaction
                        da.Update(ds, "SECTIONS")
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
            For i = 0 To Grid_Section.Rows.Count - 1
                If DirectCast(Grid_Section.Rows(i).Cells(0).FindControl("cbd"), CheckBox).Checked Then
                    Dim SecCode As Object = Nothing
                    SecCode = Grid_Section.Rows(i).Cells(2).Text
                    'chk for foreign reference in ACQ tble
                    Dim str As Object = Nothing
                    Dim flag As Object = Nothing
                    str = "SELECT HOLD_ID FROM HOLDINGS WHERE (SEC_CODE ='" & Trim(SecCode) & "') AND (LIB_CODE = '" & Trim(LibCode) & "')"
                    Dim cmd1 As New SqlCommand(str, SqlConn)
                    SqlConn.Open()
                    flag = cmd1.ExecuteScalar
                    SqlConn.Close()
                    If flag <> Nothing Then
                        Label6.Text = "Section reference saved..in Holdings Table, can not be deleted)"
                        Me.txt_Sec_Code.Focus()
                    Else
                        Dim SEC_ID As Integer = Convert.ToInt32(Grid_Section.Rows(i).Cells(5).Text)
                        'get cat record
                        Dim SQL As String = Nothing
                        SQL = "DELETE FROM SECTIONS WHERE (SEC_ID ='" & Trim(SEC_ID) & "') "
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