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

Public Class Members
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
                        Cat_Save_Bttn.Visible = True
                        Cat_Cancel_Bttn.Visible = True
                        Cat_Update_Bttn.Visible = False
                        cat_DeleteAll_Bttn.Visible = False
                        Label15.Text = "Enter Data and Press SAVE Button to save the record.."
                        Label6.Text = ""
                        PopulateCategories()
                        DDL_FineSystem.SelectedValue = "N"
                        Me.DDL_FineSystem_SelectedIndexChanged(sender, e)
                        PopulateSubjects()
                        Me.Mem_Save_Bttn.Visible = True
                        Mem_Update_Bttn.Visible = False
                    End If
                    Dim CreateSUserButton As Object 'System.Web.UI.WebControls.Button
                    CreateSUserButton = Master.FindControl("Accordion1").FindControl("CirPane").FindControl("Cir_Members_Bttn")
                    CreateSUserButton.ForeColor = Drawing.Color.Red
                    myPaneName = "CirPane"
                End If
            Else
                Response.Redirect("~/Default.aspx", False)
            End If
            LibCode = Session.Item("LoggedLibcode")
        Catch ex As Exception
            Label6.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As MenuEventArgs) Handles Menu1.MenuItemClick
        MultiView1.ActiveViewIndex = Int32.Parse(e.Item.Value)
        If MultiView1.ActiveViewIndex = 0 Then
            Menu1.Items(0).ImageUrl = "~/Images/CategoriesUP.png"
            Menu1.Items(1).ImageUrl = "~/Images/Sub-CategoriesOver.png"
            Menu1.Items(2).ImageUrl = "~/Images/RegistrationOver.png"
            PopulateCategories()
            txt_Cir_Category.Focus()
        End If
        If MultiView1.ActiveViewIndex = 1 Then
            Menu1.Items(0).ImageUrl = "~/Images/CategoriesOver.png"
            Menu1.Items(1).ImageUrl = "~/Images/Sub-CategoriesUP.png"
            Menu1.Items(2).ImageUrl = "~/Images/RegistrationOver.png"
            PopulateSubCategories()
            txt_Cir_SubCatName.Focus()
        End If
        If MultiView1.ActiveViewIndex = 2 Then
            Menu1.Items(0).ImageUrl = "~/Images/CategoriesOver.png"
            Menu1.Items(1).ImageUrl = "~/Images/Sub-CategoriesOver.png"
            Menu1.Items(2).ImageUrl = "~/Images/RegistrationUP.png"
            PopulateAllCategories()
            PopulateAllSubCategories()
            PopulateAllMemberNoSearch()
            PopulateAllMembersSearch()
            txt_Mem_MemNo.Focus()
        End If
    End Sub
    Private Sub Members_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        If MultiView1.ActiveViewIndex = 0 Then
            txt_Cir_Category.Focus()
        End If
        If MultiView1.ActiveViewIndex = 1 Then
            txt_Cir_SubCatName.Focus()
        End If
        If MultiView1.ActiveViewIndex = 2 Then
            txt_Mem_MemNo.Focus()
        End If
    End Sub
    Public Sub PopulateCategories()
        Dim dtSearch As DataTable = Nothing
        Try
            Dim SQL As String = Nothing
            SQL = "SELECT * FROM CATEGORIES WHERE (LIB_CODE = '" & Trim(LibCode) & "') ORDER BY CAT_NAME"
            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            da.Fill(ds)

            dtSearch = ds.Tables(0).Copy
            Dim RecordCount As Long = 0
            If dtSearch.Rows.Count = 0 Then
                Grid1.DataSource = Nothing
                Grid1.DataBind()
                Cat_DeleteAll_Bttn.Visible = False
                Label1.Text = "Total Record(s): 0 "
            Else
                RecordCount = dtSearch.Rows.Count
                Me.Grid1.DataSource = dtSearch
                Me.Grid1.DataBind()
                Cat_DeleteAll_Bttn.Visible = True
                Cat_DeleteAll_Bttn.Enabled = True
                Label1.Text = "Total Record(s): " & RecordCount
            End If
            ViewState("dt") = dtSearch
            UpdatePanel1.Update()
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
    'get value of row from grid
    Private Sub Grid1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles Grid1.RowCommand
        Dim Command As SqlCommand = Nothing
        Dim dr As SqlDataReader = Nothing
        Try
            If e.CommandName = "Select" Then
                Dim myRowID, CAT_ID As Integer
                myRowID = e.CommandArgument.ToString()
                CAT_ID = Grid1.DataKeys(myRowID).Value
                If Not String.IsNullOrEmpty(CAT_ID) And CAT_ID <> 0 Then
                    Label23.Text = CAT_ID
                    Dim iloop As Integer
                    Dim strcurrentchar As Object
                    Dim c As Integer
                    Dim counter1 As Integer

                    CAT_ID = TrimX(CAT_ID)
                    CAT_ID = RemoveQuotes(CAT_ID)

                    If Len(CAT_ID).ToString > 10 Then
                        Label6.Text = "Length of Input is not Proper!"
                        Label15.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Length of Input is not Proper... ');", True)
                        Exit Sub
                    End If
                    CAT_ID = " " & CAT_ID & " "
                    If InStr(1, CAT_ID, " CREATE ", 1) > 0 Or InStr(1, CAT_ID, " DELETE ", 1) > 0 Or InStr(1, CAT_ID, " DROP ", 1) > 0 Or InStr(1, CAT_ID, " INSERT ", 1) > 1 Or InStr(1, CAT_ID, " TRACK ", 1) > 1 Or InStr(1, CAT_ID, " TRACE ", 1) > 1 Then
                        Label6.Text = "Do not use reserve words... !"
                        Label15.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do not use reserve words... ');", True)
                        Exit Sub
                    End If
                    CAT_ID = TrimX(CAT_ID)
                    'get record details from database
                    Dim SQL As String = Nothing
                    SQL = " SELECT *  FROM CATEGORIES WHERE (CAT_ID = '" & Trim(CAT_ID) & "') "
                    Command = New SqlCommand(SQL, SqlConn)
                    SqlConn.Open()
                    dr = Command.ExecuteReader(CommandBehavior.CloseConnection)
                    dr.Read()

                    If dr.HasRows = True Then
                        If dr.Item("CAT_NAME").ToString <> "" Then
                            txt_Cir_Category.Text = dr.Item("CAT_NAME").ToString
                        Else
                            txt_Cir_Category.Text = ""
                        End If

                        If dr.Item("CAT_DESC").ToString <> "" Then
                            txt_Cat_Remarks.Text = dr.Item("CAT_DESC").ToString
                        Else
                            txt_Cat_Remarks.Text = ""
                        End If
                       
                        Cat_Update_Bttn.Visible = True
                        Cat_Update_Bttn.Enabled = True
                        Cat_Save_Bttn.Visible = False
                        Label6.Text = ""
                        Label15.Text = "Press UPDATE Button to save the Changes if any.."
                        dr.Close()
                    Else
                        Cat_Update_Bttn.Visible = False
                        Cat_Update_Bttn.Enabled = False
                        Cat_Save_Bttn.Visible = True
                        Label23.Text = ""
                        Label6.Text = "Record Not Selected to Edit"
                        Label15.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' No Record to Edit... ');", True)
                    End If
                Else
                    Cat_Update_Bttn.Visible = False
                    Cat_Update_Bttn.Enabled = False
                    Cat_Save_Bttn.Visible = True
                    Label23.Text = ""
                    Label6.Text = "Record Not Selected to Edit"
                    Label15.Text = ""
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' No Record to Edit... ');", True)
                End If
            End If
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
            Label15.Text = ""
            Label23.Text = ""
            Cat_Update_Bttn.Visible = False
            Cat_Update_Bttn.Enabled = False
            Cat_Save_Bttn.Visible = True
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'save record
    Protected Sub Cat_Save_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Cat_Save_Bttn.Click
        If IsPostBack = True Then
            'check unwanted characters
            Dim iloop As Integer
            Dim strcurrentchar As Object
            Dim c As Integer
            Dim counter1, counter2, counter3, counter4, counter5, counter6, counter7, counter8, counter9 As Integer
            Dim thisTransaction As SqlClient.SqlTransaction = Nothing
            Try
                'validation for App No
                Dim CAT_NAME As Object = Nothing
                If Me.txt_Cir_Category.Text <> "" Then
                    CAT_NAME = TrimAll(txt_Cir_Category.Text)
                    CAT_NAME = RemoveQuotes(CAT_NAME)
                    If CAT_NAME.Length > 50 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Cir_Category.Focus()
                        Exit Sub
                    End If
                    CAT_NAME = " " & CAT_NAME & " "
                    If InStr(1, CAT_NAME, "CREATE", 1) > 0 Or InStr(1, CAT_NAME, "DELETE", 1) > 0 Or InStr(1, CAT_NAME, "DROP", 1) > 0 Or InStr(1, CAT_NAME, "INSERT", 1) > 1 Or InStr(1, CAT_NAME, "TRACK", 1) > 1 Or InStr(1, CAT_NAME, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Cir_Category.Focus()
                        Exit Sub
                    End If
                    CAT_NAME = TrimAll(CAT_NAME)
                   
                    'check duplicate category
                    Dim str As Object = Nothing
                    Dim flag As Object = Nothing
                    str = "SELECT CAT_NAME FROM CATEGORIES WHERE (CAT_NAME ='" & Trim(CAT_NAME) & "') AND (LIB_CODE ='" & Trim(LibCode) & "')"
                    Dim cmd1 As New SqlCommand(str, SqlConn)
                    SqlConn.Open()
                    flag = cmd1.ExecuteScalar
                    If flag <> Nothing Then
                        Label6.Text = "This Member Category already exists !"
                        Label15.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('This Approval No has already been processed, Use another one !');", True)
                        txt_Cir_Category.Focus()
                        Exit Sub
                    End If
                    SqlConn.Close()
                Else
                    Label6.Text = "Plz Enter Category Name!"
                    Label15.Text = ""
                    txt_Cir_Category.Focus()
                    Exit Sub
                End If

                'Server validation for  : txt_Cat_Remarks
                Dim CAT_DESC As Object = Nothing
                If txt_Cat_Remarks.Text <> "" Then
                    CAT_DESC = TrimAll(txt_Cat_Remarks.Text)
                    CAT_DESC = RemoveQuotes(CAT_DESC)
                    If CAT_DESC.Length > 250 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Remarks.Focus()
                        Exit Sub
                    End If

                    CAT_DESC = " " & CAT_DESC & " "
                    If InStr(1, CAT_DESC, "CREATE", 1) > 0 Or InStr(1, CAT_DESC, "DELETE", 1) > 0 Or InStr(1, CAT_DESC, "DROP", 1) > 0 Or InStr(1, CAT_DESC, "INSERT", 1) > 1 Or InStr(1, CAT_DESC, "TRACK", 1) > 1 Or InStr(1, CAT_DESC, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_Remarks.Focus()
                        Exit Sub
                    End If
                    CAT_DESC = TrimAll(CAT_DESC)
                Else
                    CAT_DESC = ""
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
                objCommand.CommandText = "INSERT INTO CATEGORIES (CAT_NAME, CAT_DESC, DATE_ADDED, USER_CODE, LIB_CODE, IP) " & _
                                         " VALUES (@CAT_NAME, @CAT_DESC, @DATE_ADDED, @USER_CODE, @LIB_CODE,@IP); "

                If CAT_NAME = "" Then CAT_NAME = System.DBNull.Value
                objCommand.Parameters.Add("@CAT_NAME", SqlDbType.NVarChar)
                objCommand.Parameters("@CAT_NAME").Value = CAT_NAME

                objCommand.Parameters.Add("@CAT_DESC", SqlDbType.NVarChar)
                If CAT_DESC = "" Then
                    objCommand.Parameters("@CAT_DESC").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@CAT_DESC").Value = CAT_DESC
                End If

                If DATE_ADDED = "" Then DATE_ADDED = System.DBNull.Value
                objCommand.Parameters.Add("@DATE_ADDED", SqlDbType.DateTime)
                objCommand.Parameters("@DATE_ADDED").Value = DATE_ADDED

                If USER_CODE = "" Then USER_CODE = System.DBNull.Value
                objCommand.Parameters.Add("@USER_CODE", SqlDbType.NVarChar)
                objCommand.Parameters("@USER_CODE").Value = USER_CODE

                If LIB_CODE = "" Then LIB_CODE = System.DBNull.Value
                objCommand.Parameters.Add("@LIB_CODE", SqlDbType.NVarChar)
                objCommand.Parameters("@LIB_CODE").Value = LIB_CODE

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

                Label15.Text = "Record Added Successfully! "
                Label6.Text = ""
                Me.Cat_Save_Bttn.Visible = True
                Cat_Update_Bttn.Visible = False
                txt_Cir_Category.Text = ""
                txt_Cat_Remarks.Text = ""
                PopulateCategories()
                txt_Cir_Category.Focus()
            Catch q As SqlException
                thisTransaction.Rollback()
                Label6.Text = "Database Error -SAVE: " & (q.Message() & ", " & q.Number & ", " & q.GetType.ToString)
                Label15.Text = ""
            Catch ex As Exception
                Label6.Text = "Error-SAVE: " & (ex.Message())
                Label15.Text = ""
            Finally
                SqlConn.Close()
            End Try
        End If
    End Sub
    <System.Web.Script.Services.ScriptMethod(), _
System.Web.Services.WebMethod()> _
    Public Shared Function SearchCategories(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim sdr As SqlDataReader
        Dim cmd As SqlCommand = New SqlCommand
        cmd.CommandText = "select DISTINCT CAT_NAME from CATEGORIES where (CAT_NAME like '" + prefixText + "%') AND (LIB_CODE ='" & LibCode & "')"
        cmd.Connection = SqlConn
        Try
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            Dim categories As List(Of String) = New List(Of String)
            sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            While sdr.Read
                categories.Add(sdr("CAT_NAME").ToString)
            End While
            Return categories
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
    'delete selected rows
    Protected Sub Cat_DeleteAll_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Cat_DeleteAll_Bttn.Click
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing
        Try
            For Each row As GridViewRow In Grid1.Rows
                Dim cb As CheckBox = DirectCast(row.FindControl("cbd"), CheckBox)
                If cb IsNot Nothing AndAlso cb.Checked = True Then
                    Dim ID As Integer = Convert.ToInt32(Grid1.DataKeys(row.RowIndex).Value)

                    Dim LIB_CODE As Object = Nothing
                    LIB_CODE = LibCode

                    'check duplicate category
                    Dim str As Object = Nothing
                    Dim flag As Object = Nothing
                    str = "SELECT CAT_ID FROM MEMBERSHIPS WHERE (CAT_ID ='" & Trim(ID) & "')"
                    Dim cmd1 As New SqlCommand(str, SqlConn)
                    SqlConn.Open()
                    flag = cmd1.ExecuteScalar
                    SqlConn.Close()
                    If flag <> Nothing Then
                        Label6.Text = "This Member Category can not be deleted - it is saved with Member Records !"
                        Label15.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('This Member Category can not be deleted - it is saved with Member Records !');", True)
                    Else
                        If SqlConn.State = 0 Then
                            SqlConn.Open()
                        End If
                        thisTransaction = SqlConn.BeginTransaction()
                        Dim objCommand As New SqlCommand
                        objCommand.Connection = SqlConn
                        objCommand.Transaction = thisTransaction
                        objCommand.CommandType = CommandType.Text
                        objCommand.CommandText = "DELETE FROM CATEGORIES WHERE (CAT_ID =@CAT_ID and LIB_CODE =@LIB_CODE) "

                        objCommand.Parameters.Add("@CAT_ID", SqlDbType.Int)
                        objCommand.Parameters("@CAT_ID").Value = ID

                        If LIB_CODE = "" Then LIB_CODE = System.DBNull.Value
                        objCommand.Parameters.Add("@LIB_CODE", SqlDbType.NVarChar)
                        objCommand.Parameters("@LIB_CODE").Value = LIB_CODE

                        objCommand.ExecuteNonQuery()

                    End If

                    thisTransaction.Commit()
                    SqlConn.Close()
                End If
            Next
            Dim s As Object = Nothing
            Dim e1 As EventArgs = Nothing
            PopulateCategories()
        Catch s As Exception
            thisTransaction.Rollback()
            Label15.Text = ""
            Label6.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    Protected Sub Cat_Cancel_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Cat_Cancel_Bttn.Click
        Me.txt_Cir_Category.Text = ""
        txt_Cat_Remarks.Text = ""
        Label23.Text = ""
        Cat_Save_Bttn.Visible = True
        Cat_Save_Bttn.Enabled = True
        Cat_Update_Bttn.Visible = False
        Label15.Text = "Enter Data and Press SAVE Button to save the record.."
        Label6.Text = ""
    End Sub
    'update record
    Protected Sub Cat_Update_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Cat_Update_Bttn.Click
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
                Dim counter1, counter2, counter3, counter4, counter5, counter6, counter7, counter8, counter9 As Integer
                '****************************************************************************************************
                'validation for cat ID
                Dim CAT_ID As Integer = Nothing
                If Label23.Text <> "" Then
                    CAT_ID = TrimX(Label23.Text)
                    CAT_ID = RemoveQuotes(CAT_ID)

                    If Len(CAT_ID).ToString > 10 Then
                        Label6.Text = "Length of Input is not Proper!"
                        Label15.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Length of Input is not Proper... ');", True)
                        Exit Sub
                    End If
                    If Not IsNumeric(CAT_ID) = True Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        Exit Sub
                    End If

                    CAT_ID = " " & CAT_ID & " "
                    If InStr(1, CAT_ID, " CREATE ", 1) > 0 Or InStr(1, CAT_ID, " DELETE ", 1) > 0 Or InStr(1, CAT_ID, " DROP ", 1) > 0 Or InStr(1, CAT_ID, " INSERT ", 1) > 1 Or InStr(1, CAT_ID, " TRACK ", 1) > 1 Or InStr(1, CAT_ID, " TRACE ", 1) > 1 Then
                        Label6.Text = "Do not use reserve words... !"
                        Label15.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do not use reserve words... ');", True)
                        Exit Sub
                    End If
                    CAT_ID = TrimX(CAT_ID)

                    'check unwanted characters
                    c = 0
                    counter1 = 0
                    For iloop = 1 To Len(CAT_ID.ToString)
                        strcurrentchar = Mid(CAT_ID, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqratuvwxyz;<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter1 = 1
                            End If
                        End If
                    Next
                    If counter1 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        Exit Sub
                    End If
                Else
                    Label6.Text = "Select Record to Update !"
                    Label15.Text = ""
                    Exit Sub
                End If

                'validation for App No
                Dim CAT_NAME As Object = Nothing
                If Me.txt_Cir_Category.Text <> "" Then
                    CAT_NAME = TrimAll(txt_Cir_Category.Text)
                    CAT_NAME = RemoveQuotes(CAT_NAME)
                    If CAT_NAME.Length > 50 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Cir_Category.Focus()
                        Exit Sub
                    End If
                    CAT_NAME = " " & CAT_NAME & " "
                    If InStr(1, CAT_NAME, "CREATE", 1) > 0 Or InStr(1, CAT_NAME, "DELETE", 1) > 0 Or InStr(1, CAT_NAME, "DROP", 1) > 0 Or InStr(1, CAT_NAME, "INSERT", 1) > 1 Or InStr(1, CAT_NAME, "TRACK", 1) > 1 Or InStr(1, CAT_NAME, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Cir_Category.Focus()
                        Exit Sub
                    End If
                    CAT_NAME = TrimAll(CAT_NAME)


                    'check duplicate category
                    Dim str As Object = Nothing
                    Dim flag As Object = Nothing
                    str = "SELECT CAT_NAME FROM CATEGORIES WHERE (CAT_NAME ='" & Trim(CAT_NAME) & "') AND (CAT_ID <>'" & Trim(CAT_ID) & "')"
                    Dim cmd1 As New SqlCommand(str, SqlConn)
                    SqlConn.Open()
                    flag = cmd1.ExecuteScalar
                    If flag <> Nothing Then
                        Label6.Text = "This Member Category already exists !"
                        Label15.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('This Approval No has already been processed, Use another one !');", True)
                        txt_Cir_Category.Focus()
                        Exit Sub
                    End If
                    SqlConn.Close()
                Else
                    Label6.Text = "Plz Enter the Approval Number !"
                    Label15.Text = ""
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Plz Enter the Approval Number !');", True)
                    txt_Cir_Category.Focus()
                    Exit Sub
                End If

                'Server validation for  : txt_Cat_Remarks
                Dim CAT_DESC As Object = Nothing
                If txt_Cat_Remarks.Text <> "" Then
                    CAT_DESC = TrimAll(txt_Cat_Remarks.Text)
                    CAT_DESC = RemoveQuotes(CAT_DESC)
                    If CAT_DESC.Length > 250 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Remarks.Focus()
                        Exit Sub
                    End If

                    CAT_DESC = " " & CAT_DESC & " "
                    If InStr(1, CAT_DESC, "CREATE", 1) > 0 Or InStr(1, CAT_DESC, "DELETE", 1) > 0 Or InStr(1, CAT_DESC, "DROP", 1) > 0 Or InStr(1, CAT_DESC, "INSERT", 1) > 1 Or InStr(1, CAT_DESC, "TRACK", 1) > 1 Or InStr(1, CAT_DESC, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_Remarks.Focus()
                        Exit Sub
                    End If
                    CAT_DESC = TrimAll(CAT_DESC)
                Else
                    CAT_DESC = ""
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
                If Label23.Text <> "" Then
                    SQL = "SELECT * FROM CATEGORIES WHERE (CAT_ID='" & Trim(CAT_ID) & "')"
                    SqlConn.Open()
                    da = New SqlDataAdapter(SQL, SqlConn)
                    cb = New SqlCommandBuilder(da)
                    da.Fill(ds, "CAT")
                    If ds.Tables("CAT").Rows.Count <> 0 Then
                        If Not String.IsNullOrEmpty(CAT_NAME) Then
                            ds.Tables("CAT").Rows(0)("CAT_NAME") = CAT_NAME.Trim
                        Else
                            ds.Tables("CAT").Rows(0)("CAT_NAME") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(CAT_DESC) Then
                            ds.Tables("CAT").Rows(0)("CAT_DESC") = CAT_DESC.Trim
                        Else
                            ds.Tables("CAT").Rows(0)("CAT_DESC") = System.DBNull.Value
                        End If

                        ds.Tables("CAT").Rows(0)("UPDATED_BY") = USER_CODE
                        ds.Tables("CAT").Rows(0)("DATE_MODIFIED") = DATE_MODIFIED
                        ds.Tables("CAT").Rows(0)("IP") = IP

                        thisTransaction = SqlConn.BeginTransaction()
                        da.SelectCommand.Transaction = thisTransaction
                        da.Update(ds, "CAT")
                        thisTransaction.Commit()
                        Label6.Text = ""
                        Label15.Text = "Record Updated Successfully!"
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Record Updated Successfully!');", True)
                        txt_Cir_Category.Text = ""
                        txt_Cat_Remarks.Text = ""
                        Cat_Save_Bttn.Visible = True
                        Cat_Save_Bttn.Enabled = True
                        Cat_Update_Bttn.Visible = False
                        Label23.Text = ""
                        PopulateCategories()
                    Else
                        txt_Cir_Category.Text = ""
                        txt_Cat_Remarks.Text = ""
                        Label6.Text = "Record Updation not done  - Please Contact System Administrator"
                        Label15.Text = ""
                    End If
                End If
            Else
                'record not selected
                txt_Cir_Category.Text = ""
                txt_Cat_Remarks.Text = ""
                Label6.Text = "Record Not Selected..."
                Label15.Text = ""
            End If
                SqlConn.Close()
        Catch q As SqlException
            thisTransaction.Rollback()
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
            Label15.Text = ""
        Finally
            SqlConn.Close()
        End Try
    End Sub










    '***********************************************************************
    'MEMBER SUB-CATEGORIES
    Public Sub PopulateSubCategories()
        Dim dtSearch As DataTable = Nothing
        Try
            Dim SQL As String = Nothing
            SQL = "SELECT * FROM SUB_CATEGORIES WHERE (LIB_CODE = '" & Trim(LibCode) & "') ORDER BY SUBCAT_NAME"
            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            da.Fill(ds)

            dtSearch = ds.Tables(0).Copy
            Dim RecordCount As Long = 0
            If dtSearch.Rows.Count = 0 Then
                Grid2.DataSource = Nothing
                Grid2.DataBind()
                SubCat_DeleteAll_Bttn.Visible = False
                Label4.Text = "Total Record(s): 0 "
            Else
                RecordCount = dtSearch.Rows.Count
                Me.Grid2.DataSource = dtSearch
                Me.Grid2.DataBind()
                SubCat_DeleteAll_Bttn.Visible = True
                SubCat_DeleteAll_Bttn.Enabled = True
                Label4.Text = "Total Record(s): " & RecordCount
            End If
            ViewState("dt") = dtSearch
            UpdatePanel4.Update()
            SubCat_Save_Bttn.Visible = True
            SubCat_Save_Bttn.Enabled = True
            SubCat_Update_Bttn.Visible = False
            Label5.Text = ""
            Label7.Text = "Enter Data and Press SAVE Button to Save the Record!"
        Catch s As Exception
            Label5.Text = "Error: " & (s.Message())
            Label7.Text = ""
        Finally
            SqlConn.Close()
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
            Label5.Text = "Error:  there is error in page index !"
            Label7.Text = ""
        End Try
    End Sub
    'gridview sorting event
    Protected Sub Grid2_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles Grid2.Sorting
        Dim temp As DataTable = CType(ViewState("dt"), DataTable)
        GridViewSortExpression = e.SortExpression
        Grid1.DataSource = temp
        Dim pageIndex As Integer = Grid2.PageIndex
        Grid2.DataSource = SortDataTable(Grid2.DataSource, False)
        Grid2.DataBind()
        Grid2.PageIndex = pageIndex
    End Sub
    <System.Web.Script.Services.ScriptMethod(), _
System.Web.Services.WebMethod()> _
    Public Shared Function SearchSubCategories(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim sdr As SqlDataReader
        Dim cmd As SqlCommand = New SqlCommand
        cmd.CommandText = "select DISTINCT SUBCAT_NAME from SUB_CATEGORIES where (SUBCAT_NAME like '" + prefixText + "%') AND (LIB_CODE ='" & LibCode & "')"
        cmd.Connection = SqlConn
        Try
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            Dim Subcategories As List(Of String) = New List(Of String)
            sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            While sdr.Read
                Subcategories.Add(sdr("SUBCAT_NAME").ToString)
            End While
            Return Subcategories
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
    'save subcategories
    Protected Sub SubCat_Save_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SubCat_Save_Bttn.Click
        If IsPostBack = True Then
            'check unwanted characters
            Dim iloop As Integer
            Dim strcurrentchar As Object
            Dim c As Integer
            Dim counter1, counter2, counter3, counter4, counter5, counter6, counter7, counter8, counter9 As Integer
            Dim thisTransaction As SqlClient.SqlTransaction = Nothing
            Try
                'validation for App No
                Dim SUBCAT_NAME As Object = Nothing
                If Me.txt_Cir_SubCatName.Text <> "" Then
                    SUBCAT_NAME = TrimAll(txt_Cir_SubCatName.Text)
                    SUBCAT_NAME = RemoveQuotes(SUBCAT_NAME)
                    If SUBCAT_NAME.Length > 200 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Cir_SubCatName.Focus()
                        Exit Sub
                    End If
                    SUBCAT_NAME = " " & SUBCAT_NAME & " "
                    If InStr(1, SUBCAT_NAME, "CREATE", 1) > 0 Or InStr(1, SUBCAT_NAME, "DELETE", 1) > 0 Or InStr(1, SUBCAT_NAME, "DROP", 1) > 0 Or InStr(1, SUBCAT_NAME, "INSERT", 1) > 1 Or InStr(1, SUBCAT_NAME, "TRACK", 1) > 1 Or InStr(1, SUBCAT_NAME, "TRACE", 1) > 1 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Cir_SubCatName.Focus()
                        Exit Sub
                    End If
                    SUBCAT_NAME = TrimAll(SUBCAT_NAME)

                    'check duplicate category
                    Dim str As Object = Nothing
                    Dim flag As Object = Nothing
                    str = "SELECT SUBCAT_NAME FROM SUB_CATEGORIES WHERE (SUBCAT_NAME ='" & Trim(SUBCAT_NAME) & "') AND (LIB_CODE ='" & Trim(LibCode) & "')"
                    Dim cmd1 As New SqlCommand(str, SqlConn)
                    SqlConn.Open()
                    flag = cmd1.ExecuteScalar
                    If flag <> Nothing Then
                        Label5.Text = "This Sub Category already exists !"
                        Label7.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('This Approval No has already been processed, Use another one !');", True)
                        txt_Cir_SubCatName.Focus()
                        Exit Sub
                    End If
                    SqlConn.Close()
                Else
                    Label5.Text = "Plz Enter the Sub Category Name !"
                    Label7.Text = ""
                    txt_Cir_SubCatName.Focus()
                    Exit Sub
                End If

                'validation for Fine System
                Dim FINE_SYSTEM As Object = Nothing
                If Me.DDL_FineSystem.Text <> "" Then
                    FINE_SYSTEM = Trim(DDL_FineSystem.Text)
                    FINE_SYSTEM = RemoveQuotes(FINE_SYSTEM)
                    If FINE_SYSTEM.Length > 2 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        DDL_FineSystem.Focus()
                        Exit Sub
                    End If
                    FINE_SYSTEM = " " & FINE_SYSTEM & " "
                    If InStr(1, FINE_SYSTEM, "CREATE", 1) > 0 Or InStr(1, FINE_SYSTEM, "DELETE", 1) > 0 Or InStr(1, FINE_SYSTEM, "DROP", 1) > 0 Or InStr(1, FINE_SYSTEM, "INSERT", 1) > 1 Or InStr(1, FINE_SYSTEM, "TRACK", 1) > 1 Or InStr(1, FINE_SYSTEM, "TRACE", 1) > 1 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        DDL_FineSystem.Focus()
                        Exit Sub
                    End If
                    FINE_SYSTEM = TrimX(FINE_SYSTEM)
                Else
                    FINE_SYSTEM = "N"
                End If

                'Server validation for  : txt_Cat_Remarks
                Dim SUBCAT_DESC As Object = Nothing
                If txt_Cir_SubCatRemarks.Text <> "" Then
                    SUBCAT_DESC = TrimAll(txt_Cir_SubCatRemarks.Text)
                    SUBCAT_DESC = RemoveQuotes(SUBCAT_DESC)
                    If SUBCAT_DESC.Length > 250 Then 'maximum length
                        Label5.Text = " Data must be of Proper Length.. "
                        Label7.Text = ""
                        txt_Cir_SubCatRemarks.Focus()
                        Exit Sub
                    End If

                    SUBCAT_DESC = " " & SUBCAT_DESC & " "
                    If InStr(1, SUBCAT_DESC, "CREATE", 1) > 0 Or InStr(1, SUBCAT_DESC, "DELETE", 1) > 0 Or InStr(1, SUBCAT_DESC, "DROP", 1) > 0 Or InStr(1, SUBCAT_DESC, "INSERT", 1) > 1 Or InStr(1, SUBCAT_DESC, "TRACK", 1) > 1 Or InStr(1, SUBCAT_DESC, "TRACE", 1) > 1 Then
                        Label5.Text = " Do Not use Reserve Words... "
                        Label7.Text = ""
                        Me.txt_Cir_SubCatRemarks.Focus()
                        Exit Sub
                    End If
                    SUBCAT_DESC = TrimAll(SUBCAT_DESC)
                Else
                    SUBCAT_DESC = ""
                End If


                'FINE1_GAP
                Dim FINE1_GAP As Integer = Nothing
                If txt_Cir_Gap1.Visible = True Then
                    If Me.txt_Cir_Gap1.Text <> "" Then
                        FINE1_GAP = Convert.ToInt16(TrimX(txt_Cir_Gap1.Text))
                        FINE1_GAP = RemoveQuotes(FINE1_GAP)
                        If Not IsNumeric(FINE1_GAP) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Cir_Gap1.Focus()
                            Exit Sub
                        End If
                        If Len(FINE1_GAP) > 5 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Cir_Gap1.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE1_GAP = Nothing
                    End If
                End If

                'Books Entitlement
                Dim ENTITLEMENT_BOOKS As Integer = Nothing
                If Me.txt_Ent_Books.Text <> "" Then
                    ENTITLEMENT_BOOKS = Convert.ToInt16(TrimX(txt_Ent_Books.Text))
                    ENTITLEMENT_BOOKS = RemoveQuotes(ENTITLEMENT_BOOKS)
                    If Not IsNumeric(ENTITLEMENT_BOOKS) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_Books.Focus()
                        Exit Sub
                    End If
                    If Len(ENTITLEMENT_BOOKS) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_Books.Focus()
                        Exit Sub
                    End If
                Else
                    ENTITLEMENT_BOOKS = Nothing
                End If

                'Manuals Entitlement
                Dim ENTITLEMENT_MANUALS As Integer = Nothing
                If Me.txt_Ent_Manuals.Text <> "" Then
                    ENTITLEMENT_MANUALS = Convert.ToInt16(TrimX(txt_Ent_Manuals.Text))
                    ENTITLEMENT_MANUALS = RemoveQuotes(ENTITLEMENT_MANUALS)
                    If Not IsNumeric(ENTITLEMENT_MANUALS) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_Manuals.Focus()
                        Exit Sub
                    End If
                    If Len(ENTITLEMENT_MANUALS) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_Manuals.Focus()
                        Exit Sub
                    End If
                Else
                    ENTITLEMENT_MANUALS = Nothing
                End If


                'PATENTS Entitlement
                Dim ENTITLEMENT_PATENTS As Integer = Nothing
                If Me.txt_Ent_Patents.Text <> "" Then
                    ENTITLEMENT_PATENTS = Convert.ToInt16(TrimX(txt_Ent_Patents.Text))
                    ENTITLEMENT_PATENTS = RemoveQuotes(ENTITLEMENT_PATENTS)
                    If Not IsNumeric(ENTITLEMENT_PATENTS) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_Patents.Focus()
                        Exit Sub
                    End If
                    If Len(ENTITLEMENT_PATENTS) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_Patents.Focus()
                        Exit Sub
                    End If
                Else
                    ENTITLEMENT_PATENTS = Nothing
                End If

                'REPORTS Entitlement
                Dim ENTITLEMENT_REPORTS As Integer = Nothing
                If Me.txt_Ent_Reports.Text <> "" Then
                    ENTITLEMENT_REPORTS = Convert.ToInt16(TrimX(txt_Ent_Reports.Text))
                    ENTITLEMENT_REPORTS = RemoveQuotes(ENTITLEMENT_REPORTS)
                    If Not IsNumeric(ENTITLEMENT_REPORTS) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_Reports.Focus()
                        Exit Sub
                    End If
                    If Len(ENTITLEMENT_REPORTS) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_Reports.Focus()
                        Exit Sub
                    End If
                Else
                    ENTITLEMENT_REPORTS = Nothing
                End If

                'STANDARDS Entitlement
                Dim ENTITLEMENT_STANDARDS As Integer = Nothing
                If Me.txt_Ent_Standards.Text <> "" Then
                    ENTITLEMENT_STANDARDS = Convert.ToInt16(TrimX(txt_Ent_Standards.Text))
                    ENTITLEMENT_STANDARDS = RemoveQuotes(ENTITLEMENT_STANDARDS)
                    If Not IsNumeric(ENTITLEMENT_STANDARDS) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_Standards.Focus()
                        Exit Sub
                    End If
                    If Len(ENTITLEMENT_STANDARDS) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_Standards.Focus()
                        Exit Sub
                    End If
                Else
                    ENTITLEMENT_STANDARDS = Nothing
                End If

                'LOOSE Entitlement
                Dim ENTITLEMENT_LOOSE As Integer = Nothing
                If Me.txt_Ent_LooseIssues.Text <> "" Then
                    ENTITLEMENT_LOOSE = Convert.ToInt16(TrimX(txt_Ent_LooseIssues.Text))
                    ENTITLEMENT_LOOSE = RemoveQuotes(ENTITLEMENT_LOOSE)
                    If Not IsNumeric(ENTITLEMENT_LOOSE) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_LooseIssues.Focus()
                        Exit Sub
                    End If
                    If Len(ENTITLEMENT_LOOSE) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_LooseIssues.Focus()
                        Exit Sub
                    End If
                Else
                    ENTITLEMENT_LOOSE = Nothing
                End If

                'BOUNDJ Entitlement
                Dim ENTITLEMENT_BOUNDJ As Integer = Nothing
                If Me.txt_Ent_BoundJ.Text <> "" Then
                    ENTITLEMENT_BOUNDJ = Convert.ToInt16(TrimX(txt_Ent_BoundJ.Text))
                    ENTITLEMENT_BOUNDJ = RemoveQuotes(ENTITLEMENT_BOUNDJ)
                    If Not IsNumeric(ENTITLEMENT_BOUNDJ) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_BoundJ.Focus()
                        Exit Sub
                    End If
                    If Len(ENTITLEMENT_BOUNDJ) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_BoundJ.Focus()
                        Exit Sub
                    End If
                Else
                    ENTITLEMENT_BOUNDJ = Nothing
                End If

                'AV Entitlement
                Dim ENTITLEMENT_AV As Integer = Nothing
                If Me.txt_Ent_AV.Text <> "" Then
                    ENTITLEMENT_AV = Convert.ToInt16(TrimX(txt_Ent_AV.Text))
                    ENTITLEMENT_AV = RemoveQuotes(ENTITLEMENT_AV)
                    If Not IsNumeric(ENTITLEMENT_AV) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_AV.Focus()
                        Exit Sub
                    End If
                    If Len(ENTITLEMENT_AV) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_AV.Focus()
                        Exit Sub
                    End If
                Else
                    ENTITLEMENT_AV = Nothing
                End If

                'CARTOGRAPHIC Entitlement
                Dim ENTITLEMENT_CARTOGRAPHIC As Integer = Nothing
                If Me.txt_Ent_Cartographic.Text <> "" Then
                    ENTITLEMENT_CARTOGRAPHIC = Convert.ToInt16(TrimX(txt_Ent_Cartographic.Text))
                    ENTITLEMENT_CARTOGRAPHIC = RemoveQuotes(ENTITLEMENT_CARTOGRAPHIC)
                    If Not IsNumeric(ENTITLEMENT_CARTOGRAPHIC) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_Cartographic.Focus()
                        Exit Sub
                    End If
                    If Len(ENTITLEMENT_CARTOGRAPHIC) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_Cartographic.Focus()
                        Exit Sub
                    End If
                Else
                    ENTITLEMENT_CARTOGRAPHIC = Nothing
                End If

                'MANUSCRIPTS Entitlement
                Dim ENTITLEMENT_MANUSCRIPTS As Integer = Nothing
                If Me.txt_Ent_Manuscripts.Text <> "" Then
                    ENTITLEMENT_MANUSCRIPTS = Convert.ToInt16(TrimX(txt_Ent_Manuscripts.Text))
                    ENTITLEMENT_MANUSCRIPTS = RemoveQuotes(ENTITLEMENT_MANUSCRIPTS)
                    If Not IsNumeric(ENTITLEMENT_MANUSCRIPTS) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_Manuscripts.Focus()
                        Exit Sub
                    End If
                    If Len(ENTITLEMENT_MANUSCRIPTS) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_Manuscripts.Focus()
                        Exit Sub
                    End If
                Else
                    ENTITLEMENT_MANUSCRIPTS = Nothing
                End If

                'BBGENERAL Entitlement
                Dim ENTITLEMENT_BBGENERAL As Integer = Nothing
                If Me.txt_Ent_BBGeneral.Text <> "" Then
                    ENTITLEMENT_BBGENERAL = Convert.ToInt16(TrimX(txt_Ent_BBGeneral.Text))
                    ENTITLEMENT_BBGENERAL = RemoveQuotes(ENTITLEMENT_BBGENERAL)
                    If Not IsNumeric(ENTITLEMENT_BBGENERAL) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_BBGeneral.Focus()
                        Exit Sub
                    End If
                    If Len(ENTITLEMENT_BBGENERAL) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_BBGeneral.Focus()
                        Exit Sub
                    End If
                Else
                    ENTITLEMENT_BBGENERAL = Nothing
                End If

                'BBRESERVE Entitlement
                Dim ENTITLEMENT_BBRESERVE As Integer = Nothing
                If Me.txt_Ent_BBReserve.Text <> "" Then
                    ENTITLEMENT_BBRESERVE = Convert.ToInt16(TrimX(txt_Ent_BBReserve.Text))
                    ENTITLEMENT_BBRESERVE = RemoveQuotes(ENTITLEMENT_BBRESERVE)
                    If Not IsNumeric(ENTITLEMENT_BBRESERVE) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_BBReserve.Focus()
                        Exit Sub
                    End If
                    If Len(ENTITLEMENT_BBRESERVE) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_BBReserve.Focus()
                        Exit Sub
                    End If
                Else
                    ENTITLEMENT_BBRESERVE = Nothing
                End If

                'Non-Returanble Entitlement
                Dim ENTITLEMENT_NONRETURNABLE As Integer = Nothing
                If Me.txt_Ent_NonReturnable.Text <> "" Then
                    ENTITLEMENT_NONRETURNABLE = Convert.ToInt16(TrimX(txt_Ent_NonReturnable.Text))
                    ENTITLEMENT_NONRETURNABLE = RemoveQuotes(ENTITLEMENT_NONRETURNABLE)
                    If Not IsNumeric(ENTITLEMENT_NONRETURNABLE) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_NonReturnable.Focus()
                        Exit Sub
                    End If
                    If Len(ENTITLEMENT_NONRETURNABLE) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_NonReturnable.Focus()
                        Exit Sub
                    End If
                Else
                    ENTITLEMENT_NONRETURNABLE = Nothing
                End If



                'Books DueDays
                Dim DUEDAYS_BOOKS As Integer = Nothing
                If Me.txt_DueDays_Books.Text <> "" Then
                    DUEDAYS_BOOKS = Convert.ToInt16(TrimX(txt_DueDays_Books.Text))
                    DUEDAYS_BOOKS = RemoveQuotes(DUEDAYS_BOOKS)
                    If Not IsNumeric(DUEDAYS_BOOKS) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_Books.Focus()
                        Exit Sub
                    End If
                    If Len(DUEDAYS_BOOKS) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_Books.Focus()
                        Exit Sub
                    End If
                Else
                    DUEDAYS_BOOKS = Nothing
                End If

                'Manuals DueDays
                Dim DUEDAYS_MANUALS As Integer = Nothing
                If Me.txt_DueDays_Manuals.Text <> "" Then
                    DUEDAYS_MANUALS = Convert.ToInt16(TrimX(txt_DueDays_Manuals.Text))
                    DUEDAYS_MANUALS = RemoveQuotes(DUEDAYS_MANUALS)
                    If Not IsNumeric(DUEDAYS_MANUALS) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_Manuals.Focus()
                        Exit Sub
                    End If
                    If Len(DUEDAYS_MANUALS) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_Manuals.Focus()
                        Exit Sub
                    End If
                Else
                    DUEDAYS_MANUALS = Nothing
                End If


                'PATENTS DueDays
                Dim DUEDAYS_PATENTS As Integer = Nothing
                If Me.txt_DueDays_Patents.Text <> "" Then
                    DUEDAYS_PATENTS = Convert.ToInt16(TrimX(txt_DueDays_Patents.Text))
                    DUEDAYS_PATENTS = RemoveQuotes(DUEDAYS_PATENTS)
                    If Not IsNumeric(DUEDAYS_PATENTS) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_Patents.Focus()
                        Exit Sub
                    End If
                    If Len(DUEDAYS_PATENTS) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_Patents.Focus()
                        Exit Sub
                    End If
                Else
                    DUEDAYS_PATENTS = Nothing
                End If

                'REPORTS DueDays
                Dim DUEDAYS_REPORTS As Integer = Nothing
                If Me.txt_DueDays_Reports.Text <> "" Then
                    DUEDAYS_REPORTS = Convert.ToInt16(TrimX(txt_DueDays_Reports.Text))
                    DUEDAYS_REPORTS = RemoveQuotes(DUEDAYS_REPORTS)
                    If Not IsNumeric(DUEDAYS_REPORTS) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_Reports.Focus()
                        Exit Sub
                    End If
                    If Len(DUEDAYS_REPORTS) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_Reports.Focus()
                        Exit Sub
                    End If
                Else
                    DUEDAYS_REPORTS = Nothing
                End If

                'STANDARDS DueDays
                Dim DUEDAYS_STANDARDS As Integer = Nothing
                If Me.txt_DueDays_Standards.Text <> "" Then
                    DUEDAYS_STANDARDS = Convert.ToInt16(TrimX(txt_DueDays_Standards.Text))
                    DUEDAYS_STANDARDS = RemoveQuotes(DUEDAYS_STANDARDS)
                    If Not IsNumeric(DUEDAYS_STANDARDS) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_Standards.Focus()
                        Exit Sub
                    End If
                    If Len(DUEDAYS_STANDARDS) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_Standards.Focus()
                        Exit Sub
                    End If
                Else
                    DUEDAYS_STANDARDS = Nothing
                End If

                'LOOSE DueDays
                Dim DUEDAYS_LOOSE As Integer = Nothing
                If Me.txt_DueDays_Loose.Text <> "" Then
                    DUEDAYS_LOOSE = Convert.ToInt16(TrimX(txt_DueDays_Loose.Text))
                    DUEDAYS_LOOSE = RemoveQuotes(DUEDAYS_LOOSE)
                    If Not IsNumeric(DUEDAYS_LOOSE) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_Loose.Focus()
                        Exit Sub
                    End If
                    If Len(DUEDAYS_LOOSE) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_Loose.Focus()
                        Exit Sub
                    End If
                Else
                    DUEDAYS_LOOSE = Nothing
                End If

                'BOUNDJ DueDays
                Dim DUEDAYS_BOUNDJ As Integer = Nothing
                If Me.txt_DueDays_BoundJ.Text <> "" Then
                    DUEDAYS_BOUNDJ = Convert.ToInt16(TrimX(txt_DueDays_BoundJ.Text))
                    DUEDAYS_BOUNDJ = RemoveQuotes(DUEDAYS_BOUNDJ)
                    If Not IsNumeric(DUEDAYS_BOUNDJ) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_BoundJ.Focus()
                        Exit Sub
                    End If
                    If Len(DUEDAYS_BOUNDJ) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_BoundJ.Focus()
                        Exit Sub
                    End If
                Else
                    DUEDAYS_BOUNDJ = Nothing
                End If

                'AV DueDays
                Dim DUEDAYS_AV As Integer = Nothing
                If Me.txt_DueDays_AV.Text <> "" Then
                    DUEDAYS_AV = Convert.ToInt16(TrimX(txt_DueDays_AV.Text))
                    DUEDAYS_AV = RemoveQuotes(DUEDAYS_AV)
                    If Not IsNumeric(DUEDAYS_AV) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_AV.Focus()
                        Exit Sub
                    End If
                    If Len(DUEDAYS_AV) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_AV.Focus()
                        Exit Sub
                    End If
                Else
                    DUEDAYS_AV = Nothing
                End If

                'CARTOGRAPHIC DueDays
                Dim DUEDAYS_CARTOGRAPHIC As Integer = Nothing
                If Me.txt_DueDays_Cartographic.Text <> "" Then
                    DUEDAYS_CARTOGRAPHIC = Convert.ToInt16(TrimX(txt_DueDays_Cartographic.Text))
                    DUEDAYS_CARTOGRAPHIC = RemoveQuotes(DUEDAYS_CARTOGRAPHIC)
                    If Not IsNumeric(DUEDAYS_CARTOGRAPHIC) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_Cartographic.Focus()
                        Exit Sub
                    End If
                    If Len(DUEDAYS_CARTOGRAPHIC) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_Cartographic.Focus()
                        Exit Sub
                    End If
                Else
                    DUEDAYS_CARTOGRAPHIC = Nothing
                End If

                'MANUSCRIPTS DueDays
                Dim DUEDAYS_MANUSCRIPTS As Integer = Nothing
                If Me.txt_DueDays_Manuscripts.Text <> "" Then
                    DUEDAYS_MANUSCRIPTS = Convert.ToInt16(TrimX(txt_DueDays_Manuscripts.Text))
                    DUEDAYS_MANUSCRIPTS = RemoveQuotes(DUEDAYS_MANUSCRIPTS)
                    If Not IsNumeric(DUEDAYS_MANUSCRIPTS) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_Manuscripts.Focus()
                        Exit Sub
                    End If
                    If Len(DUEDAYS_MANUSCRIPTS) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_Manuscripts.Focus()
                        Exit Sub
                    End If
                Else
                    DUEDAYS_MANUSCRIPTS = Nothing
                End If

                'BBGENERAL DueDays
                Dim DUEDAYS_BBGENERAL As Integer = Nothing
                If Me.txt_DueDays_BBGeneral.Text <> "" Then
                    DUEDAYS_BBGENERAL = Convert.ToInt16(TrimX(txt_DueDays_BBGeneral.Text))
                    DUEDAYS_BBGENERAL = RemoveQuotes(DUEDAYS_BBGENERAL)
                    If Not IsNumeric(DUEDAYS_BBGENERAL) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_BBGeneral.Focus()
                        Exit Sub
                    End If
                    If Len(DUEDAYS_BBGENERAL) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_BBGeneral.Focus()
                        Exit Sub
                    End If
                Else
                    DUEDAYS_BBGENERAL = Nothing
                End If

                'BBRESERVE DueDays
                Dim DUEDAYS_BBRESERVE As Integer = Nothing
                If Me.txt_DueDays_BBReserve.Text <> "" Then
                    DUEDAYS_BBRESERVE = Convert.ToInt16(TrimX(txt_DueDays_BBReserve.Text))
                    DUEDAYS_BBRESERVE = RemoveQuotes(DUEDAYS_BBRESERVE)
                    If Not IsNumeric(DUEDAYS_BBRESERVE) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_BBReserve.Focus()
                        Exit Sub
                    End If
                    If Len(DUEDAYS_BBRESERVE) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_BBReserve.Focus()
                        Exit Sub
                    End If
                Else
                    DUEDAYS_BBRESERVE = Nothing
                End If

                'NONRETURNABLE DueDays
                Dim DUEDAYS_NONRETURNABLE As Integer = Nothing
                If Me.txt_DueDays_NonReturnable.Text <> "" Then
                    DUEDAYS_NONRETURNABLE = Convert.ToInt16(TrimX(txt_DueDays_NonReturnable.Text))
                    DUEDAYS_NONRETURNABLE = RemoveQuotes(DUEDAYS_NONRETURNABLE)
                    If Not IsNumeric(DUEDAYS_NONRETURNABLE) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_NonReturnable.Focus()
                        Exit Sub
                    End If
                    If Len(DUEDAYS_NONRETURNABLE) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_NonReturnable.Focus()
                        Exit Sub
                    End If
                Else
                    DUEDAYS_NONRETURNABLE = Nothing
                End If









                'FINE1_BOOKS
                Dim FINE1_BOOKS As Decimal = Nothing
                If txt_Fine1_Books.Visible = True Then
                    If txt_Fine1_Books.Text <> "" Then
                        FINE1_BOOKS = TrimX(txt_Fine1_Books.Text)
                        FINE1_BOOKS = RemoveQuotes(FINE1_BOOKS)
                        If FINE1_BOOKS.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_Books.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE1_BOOKS) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_Books.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE1_BOOKS = Nothing
                    End If
                End If

                'FINE1_MANUALS
                Dim FINE1_MANUALS As Decimal = Nothing
                If txt_Fine1_Manuals.Visible = True Then
                    If txt_Fine1_Manuals.Text <> "" Then
                        FINE1_MANUALS = TrimX(txt_Fine1_Manuals.Text)
                        FINE1_MANUALS = RemoveQuotes(FINE1_MANUALS)
                        If FINE1_MANUALS.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_Manuals.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE1_MANUALS) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_Manuals.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE1_MANUALS = Nothing
                    End If
                End If

                'FINE1_PATENTS
                Dim FINE1_PATENTS As Decimal = Nothing
                If txt_Fine1_Patents.Visible = True Then
                    If txt_Fine1_Patents.Text <> "" Then
                        FINE1_PATENTS = TrimX(txt_Fine1_Patents.Text)
                        FINE1_PATENTS = RemoveQuotes(FINE1_PATENTS)
                        If FINE1_PATENTS.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_Patents.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE1_PATENTS) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_Patents.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE1_PATENTS = Nothing
                    End If
                End If

                'FINE1_REPORTS
                Dim FINE1_REPORTS As Decimal = Nothing
                If txt_Fine1_Reports.Visible = True Then
                    If txt_Fine1_Reports.Text <> "" Then
                        FINE1_REPORTS = TrimX(txt_Fine1_Reports.Text)
                        FINE1_REPORTS = RemoveQuotes(FINE1_REPORTS)
                        If FINE1_REPORTS.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_Reports.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE1_REPORTS) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_Reports.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE1_REPORTS = Nothing
                    End If
                End If

                'FINE1_STANDARDS
                Dim FINE1_STANDARDS As Decimal = Nothing
                If txt_Fine1_Standards.Visible = True Then
                    If txt_Fine1_Standards.Text <> "" Then
                        FINE1_STANDARDS = TrimX(txt_Fine1_Standards.Text)
                        FINE1_STANDARDS = RemoveQuotes(FINE1_STANDARDS)
                        If FINE1_STANDARDS.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_Standards.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE1_STANDARDS) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_Standards.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE1_STANDARDS = Nothing
                    End If
                End If

                'FINE1_LOOSE
                Dim FINE1_LOOSE As Decimal = Nothing
                If txt_Fine1_Loose.Visible = True Then
                    If txt_Fine1_Loose.Text <> "" Then
                        FINE1_LOOSE = TrimX(txt_Fine1_Loose.Text)
                        FINE1_LOOSE = RemoveQuotes(FINE1_LOOSE)
                        If FINE1_LOOSE.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_Loose.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE1_LOOSE) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_Loose.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE1_LOOSE = Nothing
                    End If
                End If

                'FINE1_BOUNDJ
                Dim FINE1_BOUNDJ As Decimal = Nothing
                If txt_Fine1_BoundJ.Visible = True Then
                    If txt_Fine1_BoundJ.Text <> "" Then
                        FINE1_BOUNDJ = TrimX(txt_Fine1_BoundJ.Text)
                        FINE1_BOUNDJ = RemoveQuotes(FINE1_BOUNDJ)
                        If FINE1_LOOSE.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_BoundJ.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE1_BOUNDJ) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_BoundJ.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE1_BOUNDJ = Nothing
                    End If
                End If

                'FINE1_AV
                Dim FINE1_AV As Decimal = Nothing
                If txt_Fine1_AV.Visible = True Then
                    If txt_Fine1_AV.Text <> "" Then
                        FINE1_AV = TrimX(txt_Fine1_AV.Text)
                        FINE1_AV = RemoveQuotes(FINE1_AV)
                        If FINE1_LOOSE.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_AV.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE1_AV) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_AV.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE1_AV = Nothing
                    End If
                End If

                'FINE1_CARTOGRAPHIC
                Dim FINE1_CARTOGRAPHIC As Decimal = Nothing
                If txt_Fine1_AV.Visible = True Then
                    If txt_Fine1_Cartographic.Text <> "" Then
                        FINE1_CARTOGRAPHIC = TrimX(txt_Fine1_Cartographic.Text)
                        FINE1_CARTOGRAPHIC = RemoveQuotes(FINE1_CARTOGRAPHIC)
                        If FINE1_CARTOGRAPHIC.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_Cartographic.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE1_CARTOGRAPHIC) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_Cartographic.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE1_CARTOGRAPHIC = Nothing
                    End If
                End If

                'FINE1_MANUSCRIPTS
                Dim FINE1_MANUSCRIPTS As Decimal = Nothing
                If txt_Fine1_Manuscripts.Visible = True Then
                    If txt_Fine1_Manuscripts.Text <> "" Then
                        FINE1_MANUSCRIPTS = TrimX(txt_Fine1_Manuscripts.Text)
                        FINE1_MANUSCRIPTS = RemoveQuotes(FINE1_MANUSCRIPTS)
                        If FINE1_MANUSCRIPTS.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_Manuscripts.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE1_MANUSCRIPTS) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_Manuscripts.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE1_MANUSCRIPTS = Nothing
                    End If
                End If

                'FINE1_BBGENERAL
                Dim FINE1_BBGENERAL As Decimal = Nothing
                If txt_Fine1_BBGeneral.Visible = True Then
                    If txt_Fine1_BBGeneral.Text <> "" Then
                        FINE1_BBGENERAL = TrimX(txt_Fine1_BBGeneral.Text)
                        FINE1_BBGENERAL = RemoveQuotes(FINE1_BBGENERAL)
                        If FINE1_BBGENERAL.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_BBGeneral.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE1_BBGENERAL) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_BBGeneral.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE1_BBGENERAL = Nothing
                    End If
                End If

                'FINE1_BBRESERVE
                Dim FINE1_BBRESERVE As Decimal = Nothing
                If txt_Fine1_BBReserve.Visible = True Then
                    If txt_Fine1_BBReserve.Text <> "" Then
                        FINE1_BBRESERVE = TrimX(txt_Fine1_BBReserve.Text)
                        FINE1_BBRESERVE = RemoveQuotes(FINE1_BBRESERVE)
                        If FINE1_BBGENERAL.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_BBReserve.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE1_BBRESERVE) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_BBReserve.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE1_BBRESERVE = Nothing
                    End If
                End If




                'FINE2_BOOKS
                Dim FINE2_BOOKS As Decimal = Nothing
                If txt_Fine2_Books.Visible = True Then
                    If txt_Fine2_Books.Text <> "" Then
                        FINE2_BOOKS = TrimX(txt_Fine2_Books.Text)
                        FINE2_BOOKS = RemoveQuotes(FINE2_BOOKS)
                        If FINE2_BOOKS.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_Books.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE2_BOOKS) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_Books.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE2_BOOKS = Nothing
                    End If
                End If

                'FINE2_MANUALS
                Dim FINE2_MANUALS As Decimal = Nothing
                If txt_Fine2_Manuals.Visible = True Then
                    If txt_Fine2_Manuals.Text <> "" Then
                        FINE2_MANUALS = TrimX(txt_Fine2_Manuals.Text)
                        FINE2_MANUALS = RemoveQuotes(FINE2_MANUALS)
                        If FINE2_MANUALS.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_Manuals.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE2_MANUALS) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_Manuals.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE2_MANUALS = Nothing
                    End If
                End If

                'FINE2_PATENTS
                Dim FINE2_PATENTS As Decimal = Nothing
                If txt_Fine2_Patents.Visible = True Then
                    If txt_Fine2_Patents.Text <> "" Then
                        FINE2_PATENTS = TrimX(txt_Fine2_Patents.Text)
                        FINE2_PATENTS = RemoveQuotes(FINE2_PATENTS)
                        If FINE2_PATENTS.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_Patents.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE2_PATENTS) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_Patents.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE2_PATENTS = Nothing
                    End If
                End If

                'FINE2_REPORTS
                Dim FINE2_REPORTS As Decimal = Nothing
                If txt_Fine2_Reports.Visible = True Then
                    If txt_Fine2_Reports.Text <> "" Then
                        FINE2_REPORTS = TrimX(txt_Fine2_Reports.Text)
                        FINE2_REPORTS = RemoveQuotes(FINE2_REPORTS)
                        If FINE2_REPORTS.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_Reports.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE2_REPORTS) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_Reports.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE2_REPORTS = Nothing
                    End If
                End If

                'FINE2_STANDARDS
                Dim FINE2_STANDARDS As Decimal = Nothing
                If txt_Fine2_Standards.Visible = True Then
                    If txt_Fine2_Standards.Text <> "" Then
                        FINE2_STANDARDS = TrimX(txt_Fine2_Standards.Text)
                        FINE2_STANDARDS = RemoveQuotes(FINE2_STANDARDS)
                        If FINE2_STANDARDS.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_Standards.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE2_STANDARDS) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_Standards.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE2_STANDARDS = Nothing
                    End If
                End If

                'FINE2_LOOSE
                Dim FINE2_LOOSE As Decimal = Nothing
                If txt_Fine2_Loose.Visible = True Then
                    If txt_Fine2_Loose.Text <> "" Then
                        FINE2_LOOSE = TrimX(txt_Fine2_Loose.Text)
                        FINE2_LOOSE = RemoveQuotes(FINE2_LOOSE)
                        If FINE2_LOOSE.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_Loose.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE2_LOOSE) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_Loose.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE2_LOOSE = Nothing
                    End If
                End If

                'FINE2_BOUNDJ
                Dim FINE2_BOUNDJ As Decimal = Nothing
                If txt_Fine2_BoundJ.Visible = True Then
                    If txt_Fine2_BoundJ.Text <> "" Then
                        FINE2_BOUNDJ = TrimX(txt_Fine2_BoundJ.Text)
                        FINE2_BOUNDJ = RemoveQuotes(FINE2_BOUNDJ)
                        If FINE2_LOOSE.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_BoundJ.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE2_BOUNDJ) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_BoundJ.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE2_BOUNDJ = Nothing
                    End If
                End If

                'FINE2_AV
                Dim FINE2_AV As Decimal = Nothing
                If txt_Fine2_AV.Visible = True Then
                    If txt_Fine2_AV.Text <> "" Then
                        FINE2_AV = TrimX(txt_Fine2_AV.Text)
                        FINE2_AV = RemoveQuotes(FINE2_AV)
                        If FINE2_LOOSE.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_AV.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE2_AV) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_AV.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE2_AV = Nothing
                    End If
                End If

                'FINE2_CARTOGRAPHIC
                Dim FINE2_CARTOGRAPHIC As Decimal = Nothing
                If txt_Fine2_Cartographic.Visible = True Then
                    If txt_Fine2_Cartographic.Text <> "" Then
                        FINE2_CARTOGRAPHIC = TrimX(txt_Fine2_Cartographic.Text)
                        FINE2_CARTOGRAPHIC = RemoveQuotes(FINE2_CARTOGRAPHIC)
                        If FINE2_CARTOGRAPHIC.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_Cartographic.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE2_CARTOGRAPHIC) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_Cartographic.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE2_CARTOGRAPHIC = Nothing
                    End If
                End If

                'FINE2_MANUSCRIPTS
                Dim FINE2_MANUSCRIPTS As Decimal = Nothing
                If txt_Fine2_Manuscripts.Visible = True Then
                    If txt_Fine2_Manuscripts.Text <> "" Then
                        FINE2_MANUSCRIPTS = TrimX(txt_Fine2_Manuscripts.Text)
                        FINE2_MANUSCRIPTS = RemoveQuotes(FINE2_MANUSCRIPTS)
                        If FINE2_MANUSCRIPTS.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_Manuscripts.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE2_MANUSCRIPTS) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_Manuscripts.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE2_MANUSCRIPTS = Nothing
                    End If
                End If

                'FINE2_BBGENERAL
                Dim FINE2_BBGENERAL As Decimal = Nothing
                If txt_Fine2_BBGeneral.Visible = True Then
                    If txt_Fine2_BBGeneral.Text <> "" Then
                        FINE2_BBGENERAL = TrimX(txt_Fine2_BBGeneral.Text)
                        FINE2_BBGENERAL = RemoveQuotes(FINE2_BBGENERAL)
                        If FINE2_BBGENERAL.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_BBGeneral.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE2_BBGENERAL) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_BBGeneral.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE2_BBGENERAL = Nothing
                    End If
                End If

                'FINE2_BBRESERVE
                Dim FINE2_BBRESERVE As Decimal = Nothing
                If txt_Fine2_BBReserve.Visible = True Then
                    If txt_Fine2_BBReserve.Text <> "" Then
                        FINE2_BBRESERVE = TrimX(txt_Fine2_BBReserve.Text)
                        FINE2_BBRESERVE = RemoveQuotes(FINE2_BBRESERVE)
                        If FINE2_BBGENERAL.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_BBReserve.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE2_BBRESERVE) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_BBReserve.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE2_BBRESERVE = Nothing
                    End If
                End If






                Dim LIB_CODE As Object = Nothing
                If Session.Item("LoggedLibcode") <> "" Then
                    LIB_CODE = Session.Item("LoggedLibcode")
                Else
                    Label5.Text = "No Library Code Exists..Login Again  "
                    Label7.Text = ""
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
                objCommand.CommandText = "INSERT INTO SUB_CATEGORIES (SUBCAT_NAME, SUBCAT_DESC, FINE_SYSTEM, ENTITLEMENT_LOOSE,ENTITLEMENT_BOOKS, ENTITLEMENT_BOUNDJ, ENTITLEMENT_MANUALS, ENTITLEMENT_PATENTS, ENTITLEMENT_REPORTS, ENTITLEMENT_STANDARDS,ENTITLEMENT_AV,ENTITLEMENT_CARTOGRAPHIC,ENTITLEMENT_MANUSCRIPTS,ENTITLEMENT_BBGENERAL, ENTITLEMENT_BBRESERVE, ENTITLEMENT_NONRETURNABLE, DUEDAYS_LOOSE,DUEDAYS_BOOKS, DUEDAYS_BOUNDJ, DUEDAYS_MANUALS, DUEDAYS_PATENTS, DUEDAYS_REPORTS, DUEDAYS_STANDARDS,DUEDAYS_AV,DUEDAYS_CARTOGRAPHIC,DUEDAYS_MANUSCRIPTS,DUEDAYS_BBGENERAL, DUEDAYS_BBRESERVE, DUEDAYS_NONRETURNABLE, FINE1_GAP, FINE1_LOOSE,FINE1_BOOKS, FINE1_BOUNDJ, FINE1_MANUALS, FINE1_PATENTS, FINE1_REPORTS, FINE1_STANDARDS,FINE1_AV,FINE1_CARTOGRAPHIC,FINE1_MANUSCRIPTS,FINE1_BBGENERAL, FINE1_BBRESERVE, FINE2_LOOSE,FINE2_BOOKS, FINE2_BOUNDJ, FINE2_MANUALS, FINE2_PATENTS, FINE2_REPORTS, FINE2_STANDARDS,FINE2_AV,FINE2_CARTOGRAPHIC,FINE2_MANUSCRIPTS,FINE2_BBGENERAL, FINE2_BBRESERVE, DATE_ADDED, USER_CODE, LIB_CODE, IP) " & _
                                         " VALUES (@SUBCAT_NAME, @SUBCAT_DESC, @FINE_SYSTEM, @ENTITLEMENT_LOOSE, @ENTITLEMENT_BOOKS, @ENTITLEMENT_BOUNDJ, @ENTITLEMENT_MANUALS, @ENTITLEMENT_PATENTS, @ENTITLEMENT_REPORTS, @ENTITLEMENT_STANDARDS, @ENTITLEMENT_AV, @ENTITLEMENT_CARTOGRAPHIC, @ENTITLEMENT_MANUSCRIPTS, @ENTITLEMENT_BBGENERAL, @ENTITLEMENT_BBRESERVE, @ENTITLEMENT_NONRETURNABLE, @DUEDAYS_LOOSE, @DUEDAYS_BOOKS, @DUEDAYS_BOUNDJ, @DUEDAYS_MANUALS, @DUEDAYS_PATENTS, @DUEDAYS_REPORTS, @DUEDAYS_STANDARDS, @DUEDAYS_AV, @DUEDAYS_CARTOGRAPHIC, @DUEDAYS_MANUSCRIPTS, @DUEDAYS_BBGENERAL, @DUEDAYS_BBRESERVE, @DUEDAYS_NONRETURNABLE, @FINE1_GAP, @FINE1_LOOSE, @FINE1_BOOKS, @FINE1_BOUNDJ, @FINE1_MANUALS, @FINE1_PATENTS, @FINE1_REPORTS, @FINE1_STANDARDS, @FINE1_AV, @FINE1_CARTOGRAPHIC, @FINE1_MANUSCRIPTS, @FINE1_BBGENERAL, @FINE1_BBRESERVE, @FINE2_LOOSE, @FINE2_BOOKS, @FINE2_BOUNDJ, @FINE2_MANUALS, @FINE2_PATENTS, @FINE2_REPORTS, @FINE2_STANDARDS, @FINE2_AV, @FINE2_CARTOGRAPHIC, @FINE2_MANUSCRIPTS, @FINE2_BBGENERAL, @FINE2_BBRESERVE, @DATE_ADDED, @USER_CODE, @LIB_CODE, @IP); "

                If SUBCAT_NAME = "" Then SUBCAT_NAME = System.DBNull.Value
                objCommand.Parameters.Add("@SUBCAT_NAME", SqlDbType.NVarChar)
                objCommand.Parameters("@SUBCAT_NAME").Value = SUBCAT_NAME

                objCommand.Parameters.Add("@SUBCAT_DESC", SqlDbType.NVarChar)
                If SUBCAT_DESC = "" Then
                    objCommand.Parameters("@SUBCAT_DESC").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@SUBCAT_DESC").Value = SUBCAT_DESC
                End If

                If FINE_SYSTEM = "" Then FINE_SYSTEM = "N"
                objCommand.Parameters.Add("@FINE_SYSTEM", SqlDbType.NVarChar)
                objCommand.Parameters("@FINE_SYSTEM").Value = FINE_SYSTEM

                objCommand.Parameters.Add("@ENTITLEMENT_LOOSE", SqlDbType.Int)
                If ENTITLEMENT_LOOSE = 0 Then
                    objCommand.Parameters("@ENTITLEMENT_LOOSE").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@ENTITLEMENT_LOOSE").Value = ENTITLEMENT_LOOSE
                End If

                objCommand.Parameters.Add("@ENTITLEMENT_BOOKS", SqlDbType.Int)
                If ENTITLEMENT_BOOKS = 0 Then
                    objCommand.Parameters("@ENTITLEMENT_BOOKS").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@ENTITLEMENT_BOOKS").Value = ENTITLEMENT_BOOKS
                End If

                objCommand.Parameters.Add("@ENTITLEMENT_BOUNDJ", SqlDbType.Int)
                If ENTITLEMENT_BOUNDJ = 0 Then
                    objCommand.Parameters("@ENTITLEMENT_BOUNDJ").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@ENTITLEMENT_BOUNDJ").Value = ENTITLEMENT_BOUNDJ
                End If

                objCommand.Parameters.Add("@ENTITLEMENT_MANUALS", SqlDbType.Int)
                If ENTITLEMENT_MANUALS = 0 Then
                    objCommand.Parameters("@ENTITLEMENT_MANUALS").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@ENTITLEMENT_MANUALS").Value = ENTITLEMENT_MANUALS
                End If

                objCommand.Parameters.Add("@ENTITLEMENT_PATENTS", SqlDbType.Int)
                If ENTITLEMENT_PATENTS = 0 Then
                    objCommand.Parameters("@ENTITLEMENT_PATENTS").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@ENTITLEMENT_PATENTS").Value = ENTITLEMENT_PATENTS
                End If

                objCommand.Parameters.Add("@ENTITLEMENT_REPORTS", SqlDbType.Int)
                If ENTITLEMENT_REPORTS = 0 Then
                    objCommand.Parameters("@ENTITLEMENT_REPORTS").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@ENTITLEMENT_REPORTS").Value = ENTITLEMENT_REPORTS
                End If

                objCommand.Parameters.Add("@ENTITLEMENT_STANDARDS", SqlDbType.Int)
                If ENTITLEMENT_STANDARDS = 0 Then
                    objCommand.Parameters("@ENTITLEMENT_STANDARDS").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@ENTITLEMENT_STANDARDS").Value = ENTITLEMENT_STANDARDS
                End If

                objCommand.Parameters.Add("@ENTITLEMENT_AV", SqlDbType.Int)
                If ENTITLEMENT_AV = 0 Then
                    objCommand.Parameters("@ENTITLEMENT_AV").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@ENTITLEMENT_AV").Value = ENTITLEMENT_AV
                End If '

                objCommand.Parameters.Add("@ENTITLEMENT_CARTOGRAPHIC", SqlDbType.Int)
                If ENTITLEMENT_CARTOGRAPHIC = 0 Then
                    objCommand.Parameters("@ENTITLEMENT_CARTOGRAPHIC").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@ENTITLEMENT_CARTOGRAPHIC").Value = ENTITLEMENT_CARTOGRAPHIC
                End If

                objCommand.Parameters.Add("@ENTITLEMENT_MANUSCRIPTS", SqlDbType.Int)
                If ENTITLEMENT_MANUSCRIPTS = 0 Then
                    objCommand.Parameters("@ENTITLEMENT_MANUSCRIPTS").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@ENTITLEMENT_MANUSCRIPTS").Value = ENTITLEMENT_MANUSCRIPTS
                End If

                objCommand.Parameters.Add("@ENTITLEMENT_BBGENERAL", SqlDbType.Int)
                If ENTITLEMENT_BBGENERAL = 0 Then
                    objCommand.Parameters("@ENTITLEMENT_BBGENERAL").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@ENTITLEMENT_BBGENERAL").Value = ENTITLEMENT_BBGENERAL
                End If

                objCommand.Parameters.Add("@ENTITLEMENT_BBRESERVE", SqlDbType.Int)
                If ENTITLEMENT_BBRESERVE = 0 Then
                    objCommand.Parameters("@ENTITLEMENT_BBRESERVE").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@ENTITLEMENT_BBRESERVE").Value = ENTITLEMENT_BBRESERVE
                End If

                objCommand.Parameters.Add("@ENTITLEMENT_NONRETURNABLE", SqlDbType.Int)
                If ENTITLEMENT_NONRETURNABLE = 0 Then
                    objCommand.Parameters("@ENTITLEMENT_NONRETURNABLE").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@ENTITLEMENT_NONRETURNABLE").Value = ENTITLEMENT_NONRETURNABLE
                End If




                objCommand.Parameters.Add("@DUEDAYS_LOOSE", SqlDbType.Int)
                If DUEDAYS_LOOSE = 0 Then
                    objCommand.Parameters("@DUEDAYS_LOOSE").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@DUEDAYS_LOOSE").Value = DUEDAYS_LOOSE
                End If

                objCommand.Parameters.Add("@DUEDAYS_BOOKS", SqlDbType.Int)
                If DUEDAYS_BOOKS = 0 Then
                    objCommand.Parameters("@DUEDAYS_BOOKS").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@DUEDAYS_BOOKS").Value = DUEDAYS_BOOKS
                End If

                objCommand.Parameters.Add("@DUEDAYS_BOUNDJ", SqlDbType.Int)
                If DUEDAYS_BOUNDJ = 0 Then
                    objCommand.Parameters("@DUEDAYS_BOUNDJ").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@DUEDAYS_BOUNDJ").Value = DUEDAYS_BOUNDJ
                End If

                objCommand.Parameters.Add("@DUEDAYS_MANUALS", SqlDbType.Int)
                If DUEDAYS_MANUALS = 0 Then
                    objCommand.Parameters("@DUEDAYS_MANUALS").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@DUEDAYS_MANUALS").Value = DUEDAYS_MANUALS
                End If

                objCommand.Parameters.Add("@DUEDAYS_PATENTS", SqlDbType.Int)
                If DUEDAYS_PATENTS = 0 Then
                    objCommand.Parameters("@DUEDAYS_PATENTS").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@DUEDAYS_PATENTS").Value = DUEDAYS_PATENTS
                End If

                objCommand.Parameters.Add("@DUEDAYS_REPORTS", SqlDbType.Int)
                If DUEDAYS_REPORTS = 0 Then
                    objCommand.Parameters("@DUEDAYS_REPORTS").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@DUEDAYS_REPORTS").Value = DUEDAYS_REPORTS
                End If

                objCommand.Parameters.Add("@DUEDAYS_STANDARDS", SqlDbType.Int)
                If DUEDAYS_STANDARDS = 0 Then
                    objCommand.Parameters("@DUEDAYS_STANDARDS").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@DUEDAYS_STANDARDS").Value = DUEDAYS_STANDARDS
                End If

                objCommand.Parameters.Add("@DUEDAYS_AV", SqlDbType.Int)
                If DUEDAYS_AV = 0 Then
                    objCommand.Parameters("@DUEDAYS_AV").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@DUEDAYS_AV").Value = DUEDAYS_AV
                End If '

                objCommand.Parameters.Add("@DUEDAYS_CARTOGRAPHIC", SqlDbType.Int)
                If DUEDAYS_CARTOGRAPHIC = 0 Then
                    objCommand.Parameters("@DUEDAYS_CARTOGRAPHIC").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@DUEDAYS_CARTOGRAPHIC").Value = DUEDAYS_CARTOGRAPHIC
                End If

                objCommand.Parameters.Add("@DUEDAYS_MANUSCRIPTS", SqlDbType.Int)
                If DUEDAYS_MANUSCRIPTS = 0 Then
                    objCommand.Parameters("@DUEDAYS_MANUSCRIPTS").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@DUEDAYS_MANUSCRIPTS").Value = DUEDAYS_MANUSCRIPTS
                End If

                objCommand.Parameters.Add("@DUEDAYS_BBGENERAL", SqlDbType.Int)
                If DUEDAYS_BBGENERAL = 0 Then
                    objCommand.Parameters("@DUEDAYS_BBGENERAL").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@DUEDAYS_BBGENERAL").Value = DUEDAYS_BBGENERAL
                End If

                objCommand.Parameters.Add("@DUEDAYS_BBRESERVE", SqlDbType.Int)
                If DUEDAYS_BBRESERVE = 0 Then
                    objCommand.Parameters("@DUEDAYS_BBRESERVE").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@DUEDAYS_BBRESERVE").Value = ENTITLEMENT_BBRESERVE
                End If

                objCommand.Parameters.Add("@DUEDAYS_NONRETURNABLE", SqlDbType.Int)
                If DUEDAYS_NONRETURNABLE = 0 Then
                    objCommand.Parameters("@DUEDAYS_NONRETURNABLE").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@DUEDAYS_NONRETURNABLE").Value = DUEDAYS_NONRETURNABLE
                End If

                objCommand.Parameters.Add("@FINE1_GAP", SqlDbType.Int)
                If FINE1_GAP = 0 Then
                    objCommand.Parameters("@FINE1_GAP").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@FINE1_GAP").Value = FINE1_GAP
                End If





                objCommand.Parameters.Add("@FINE1_LOOSE", SqlDbType.Decimal)
                If FINE1_LOOSE = 0 Then
                    objCommand.Parameters("@FINE1_LOOSE").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@FINE1_LOOSE").Value = FINE1_LOOSE
                End If

                objCommand.Parameters.Add("@FINE1_BOOKS", SqlDbType.Decimal)
                If FINE1_BOOKS = 0 Then
                    objCommand.Parameters("@FINE1_BOOKS").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@FINE1_BOOKS").Value = FINE1_BOOKS
                End If

                objCommand.Parameters.Add("@FINE1_BOUNDJ", SqlDbType.Decimal)
                If FINE1_BOUNDJ = 0 Then
                    objCommand.Parameters("@FINE1_BOUNDJ").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@FINE1_BOUNDJ").Value = FINE1_BOUNDJ
                End If

                objCommand.Parameters.Add("@FINE1_MANUALS", SqlDbType.Decimal)
                If FINE1_MANUALS = 0 Then
                    objCommand.Parameters("@FINE1_MANUALS").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@FINE1_MANUALS").Value = FINE1_MANUALS
                End If

                objCommand.Parameters.Add("@FINE1_PATENTS", SqlDbType.Decimal)
                If FINE1_PATENTS = 0 Then
                    objCommand.Parameters("@FINE1_PATENTS").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@FINE1_PATENTS").Value = FINE1_PATENTS
                End If

                objCommand.Parameters.Add("@FINE1_REPORTS", SqlDbType.Decimal)
                If FINE1_REPORTS = 0 Then
                    objCommand.Parameters("@FINE1_REPORTS").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@FINE1_REPORTS").Value = FINE1_REPORTS
                End If

                objCommand.Parameters.Add("@FINE1_STANDARDS", SqlDbType.Decimal)
                If FINE1_STANDARDS = 0 Then
                    objCommand.Parameters("@FINE1_STANDARDS").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@FINE1_STANDARDS").Value = FINE1_STANDARDS
                End If

                objCommand.Parameters.Add("@FINE1_AV", SqlDbType.Decimal)
                If FINE1_AV = 0 Then
                    objCommand.Parameters("@FINE1_AV").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@FINE1_AV").Value = FINE1_AV
                End If '

                objCommand.Parameters.Add("@FINE1_CARTOGRAPHIC", SqlDbType.Decimal)
                If FINE1_CARTOGRAPHIC = 0 Then
                    objCommand.Parameters("@FINE1_CARTOGRAPHIC").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@FINE1_CARTOGRAPHIC").Value = FINE1_CARTOGRAPHIC
                End If

                objCommand.Parameters.Add("@FINE1_MANUSCRIPTS", SqlDbType.Decimal)
                If FINE1_MANUSCRIPTS = 0 Then
                    objCommand.Parameters("@FINE1_MANUSCRIPTS").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@FINE1_MANUSCRIPTS").Value = FINE1_MANUSCRIPTS
                End If

                objCommand.Parameters.Add("@FINE1_BBGENERAL", SqlDbType.Decimal)
                If FINE1_BBGENERAL = 0 Then
                    objCommand.Parameters("@FINE1_BBGENERAL").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@FINE1_BBGENERAL").Value = FINE1_BBGENERAL
                End If

                objCommand.Parameters.Add("@FINE1_BBRESERVE", SqlDbType.Decimal)
                If FINE1_BBRESERVE = 0 Then
                    objCommand.Parameters("@FINE1_BBRESERVE").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@FINE1_BBRESERVE").Value = FINE1_BBRESERVE
                End If





                objCommand.Parameters.Add("@FINE2_LOOSE", SqlDbType.Decimal)
                If FINE2_LOOSE = 0 Then
                    objCommand.Parameters("@FINE2_LOOSE").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@FINE2_LOOSE").Value = FINE2_LOOSE
                End If

                objCommand.Parameters.Add("@FINE2_BOOKS", SqlDbType.Decimal)
                If FINE2_BOOKS = 0 Then
                    objCommand.Parameters("@FINE2_BOOKS").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@FINE2_BOOKS").Value = FINE2_BOOKS
                End If

                objCommand.Parameters.Add("@FINE2_BOUNDJ", SqlDbType.Decimal)
                If FINE2_BOUNDJ = 0 Then
                    objCommand.Parameters("@FINE2_BOUNDJ").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@FINE2_BOUNDJ").Value = FINE2_BOUNDJ
                End If

                objCommand.Parameters.Add("@FINE2_MANUALS", SqlDbType.Decimal)
                If FINE2_MANUALS = 0 Then
                    objCommand.Parameters("@FINE2_MANUALS").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@FINE2_MANUALS").Value = FINE2_MANUALS
                End If

                objCommand.Parameters.Add("@FINE2_PATENTS", SqlDbType.Decimal)
                If FINE2_PATENTS = 0 Then
                    objCommand.Parameters("@FINE2_PATENTS").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@FINE2_PATENTS").Value = FINE2_PATENTS
                End If

                objCommand.Parameters.Add("@FINE2_REPORTS", SqlDbType.Decimal)
                If FINE2_REPORTS = 0 Then
                    objCommand.Parameters("@FINE2_REPORTS").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@FINE2_REPORTS").Value = FINE2_REPORTS
                End If

                objCommand.Parameters.Add("@FINE2_STANDARDS", SqlDbType.Decimal)
                If FINE2_STANDARDS = 0 Then
                    objCommand.Parameters("@FINE2_STANDARDS").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@FINE2_STANDARDS").Value = FINE2_STANDARDS
                End If

                objCommand.Parameters.Add("@FINE2_AV", SqlDbType.Decimal)
                If FINE2_AV = 0 Then
                    objCommand.Parameters("@FINE2_AV").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@FINE2_AV").Value = FINE2_AV
                End If '

                objCommand.Parameters.Add("@FINE2_CARTOGRAPHIC", SqlDbType.Decimal)
                If FINE2_CARTOGRAPHIC = 0 Then
                    objCommand.Parameters("@FINE2_CARTOGRAPHIC").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@FINE2_CARTOGRAPHIC").Value = FINE2_CARTOGRAPHIC
                End If

                objCommand.Parameters.Add("@FINE2_MANUSCRIPTS", SqlDbType.Decimal)
                If FINE2_MANUSCRIPTS = 0 Then
                    objCommand.Parameters("@FINE2_MANUSCRIPTS").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@FINE2_MANUSCRIPTS").Value = FINE2_MANUSCRIPTS
                End If

                objCommand.Parameters.Add("@FINE2_BBGENERAL", SqlDbType.Decimal)
                If FINE2_BBGENERAL = 0 Then
                    objCommand.Parameters("@FINE2_BBGENERAL").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@FINE2_BBGENERAL").Value = FINE2_BBGENERAL
                End If

                objCommand.Parameters.Add("@FINE2_BBRESERVE", SqlDbType.Decimal)
                If FINE2_BBRESERVE = 0 Then
                    objCommand.Parameters("@FINE2_BBRESERVE").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@FINE2_BBRESERVE").Value = FINE2_BBRESERVE
                End If










                If DATE_ADDED = "" Then DATE_ADDED = System.DBNull.Value
                objCommand.Parameters.Add("@DATE_ADDED", SqlDbType.DateTime)
                objCommand.Parameters("@DATE_ADDED").Value = DATE_ADDED

                If USER_CODE = "" Then USER_CODE = System.DBNull.Value
                objCommand.Parameters.Add("@USER_CODE", SqlDbType.NVarChar)
                objCommand.Parameters("@USER_CODE").Value = USER_CODE

                If LIB_CODE = "" Then LIB_CODE = System.DBNull.Value
                objCommand.Parameters.Add("@LIB_CODE", SqlDbType.NVarChar)
                objCommand.Parameters("@LIB_CODE").Value = LIB_CODE

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

                Label7.Text = "Record Added Successfully! "
                Label5.Text = ""
                Me.SubCat_Save_Bttn.Visible = True
                Me.SubCat_Save_Bttn.Enabled = True
                SubCat_Update_Bttn.Visible = False
                ClearSubCatFields()
                PopulateSubCategories()
                txt_Cir_SubCatName.Focus()
            Catch q As SqlException
                thisTransaction.Rollback()
                Label5.Text = "Database Error -SAVE: " & (q.Message() & ", " & q.Number & ", " & q.GetType.ToString)
                Label7.Text = ""
            Catch ex As Exception
                Label5.Text = "Error-SAVE: " & (ex.Message())
                Label7.Text = ""
            Finally
                SqlConn.Close()
            End Try
        End If
    End Sub
    'clear fields
    Public Sub ClearSubCatFields()
        txt_Ent_LooseIssues.Text = ""
        txt_Ent_Books.Text = ""
        txt_Ent_BoundJ.Text = ""
        txt_Ent_Manuals.Text = ""
        txt_Ent_Patents.Text = ""
        txt_Ent_Reports.Text = ""
        txt_Ent_Standards.Text = ""
        txt_Ent_AV.Text = ""
        txt_Ent_Cartographic.Text = ""
        txt_Ent_Manuscripts.Text = ""
        txt_Ent_BBGeneral.Text = ""
        txt_Ent_BBReserve.Text = ""
        txt_Ent_NonReturnable.Text = ""


        txt_DueDays_Loose.Text = ""
        txt_DueDays_Books.Text = ""
        txt_DueDays_BoundJ.Text = ""
        txt_DueDays_Manuals.Text = ""
        txt_DueDays_Patents.Text = ""
        txt_DueDays_Reports.Text = ""
        txt_DueDays_Standards.Text = ""
        txt_DueDays_AV.Text = ""
        txt_DueDays_Cartographic.Text = ""
        txt_DueDays_Manuscripts.Text = ""
        txt_DueDays_BBGeneral.Text = ""
        txt_DueDays_BBReserve.Text = ""
        txt_DueDays_NonReturnable.Text = ""

        txt_Cir_Gap1.Text = ""

        txt_Fine1_Loose.Text = ""
        txt_Fine1_Books.Text = ""
        txt_Fine1_BoundJ.Text = ""
        txt_Fine1_Manuals.Text = ""
        txt_Fine1_Patents.Text = ""
        txt_Fine1_Reports.Text = ""
        txt_Fine1_Standards.Text = ""
        txt_Fine1_AV.Text = ""
        txt_Fine1_Cartographic.Text = ""
        txt_Fine1_Manuscripts.Text = ""
        txt_Fine1_BBGeneral.Text = ""
        txt_Fine1_BBReserve.Text = ""

        txt_Fine2_Loose.Text = ""
        txt_Fine2_Books.Text = ""
        txt_Fine2_BoundJ.Text = ""
        txt_Fine2_Manuals.Text = ""
        txt_Fine2_Patents.Text = ""
        txt_Fine2_Reports.Text = ""
        txt_Fine2_Standards.Text = ""
        txt_Fine2_AV.Text = ""
        txt_Fine2_Cartographic.Text = ""
        txt_Fine2_Manuscripts.Text = ""
        txt_Fine2_BBGeneral.Text = ""
        txt_Fine2_BBReserve.Text = ""

    End Sub
    'select fine system
    Public Sub DDL_FineSystem_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDL_FineSystem.SelectedIndexChanged
        If DDL_FineSystem.Text <> "" Then
            If DDL_FineSystem.SelectedValue = "F" Then
                Label24.Text = "You have selected FLAT FINE SYSTEM - same rates of Fine will be applied"
                txt_Cir_Gap1.Text = ""
                txt_Fine1_Loose.Text = ""
                txt_Fine1_Books.Text = ""
                txt_Fine1_BoundJ.Text = ""
                txt_Fine1_Manuals.Text = ""
                txt_Fine1_Patents.Text = ""
                txt_Fine1_Reports.Text = ""
                txt_Fine1_Standards.Text = ""
                txt_Fine1_AV.Text = ""
                txt_Fine1_Cartographic.Text = ""
                txt_Fine1_Manuscripts.Text = ""
                txt_Fine1_BBGeneral.Text = ""
                txt_Fine1_BBReserve.Text = ""

                txt_Cir_Gap1.Visible = False
                txt_Fine1_Loose.Visible = False
                txt_Fine1_Books.Visible = False
                txt_Fine1_BoundJ.Visible = False
                txt_Fine1_Manuals.Visible = False
                txt_Fine1_Patents.Visible = False
                txt_Fine1_Reports.Visible = False
                txt_Fine1_Standards.Visible = False
                txt_Fine1_AV.Visible = False
                txt_Fine1_Cartographic.Visible = False
                txt_Fine1_Manuscripts.Visible = False
                txt_Fine1_BBGeneral.Visible = False
                txt_Fine1_BBReserve.Visible = False

                txt_Fine2_Loose.Visible = True
                txt_Fine2_Books.Visible = True
                txt_Fine2_BoundJ.Visible = True
                txt_Fine2_Manuals.Visible = True
                txt_Fine2_Patents.Visible = True
                txt_Fine2_Reports.Visible = True
                txt_Fine2_Standards.Visible = True
                txt_Fine2_AV.Visible = True
                txt_Fine2_Cartographic.Visible = True
                txt_Fine2_Manuscripts.Visible = True
                txt_Fine2_BBGeneral.Visible = True
                txt_Fine2_BBReserve.Visible = True
            End If

            If DDL_FineSystem.SelectedValue = "N" Then
                Label24.Text = "You have selected NO FINE Option - No Fine will be applied"
                txt_Cir_Gap1.Text = ""

                txt_Fine1_Loose.Text = ""
                txt_Fine1_Books.Text = ""
                txt_Fine1_BoundJ.Text = ""
                txt_Fine1_Manuals.Text = ""
                txt_Fine1_Patents.Text = ""
                txt_Fine1_Reports.Text = ""
                txt_Fine1_Standards.Text = ""
                txt_Fine1_AV.Text = ""
                txt_Fine1_Cartographic.Text = ""
                txt_Fine1_Manuscripts.Text = ""
                txt_Fine1_BBGeneral.Text = ""
                txt_Fine1_BBReserve.Text = ""

                txt_Fine2_Loose.Text = ""
                txt_Fine2_Books.Text = ""
                txt_Fine2_BoundJ.Text = ""
                txt_Fine2_Manuals.Text = ""
                txt_Fine2_Patents.Text = ""
                txt_Fine2_Reports.Text = ""
                txt_Fine2_Standards.Text = ""
                txt_Fine2_AV.Text = ""
                txt_Fine2_Cartographic.Text = ""
                txt_Fine2_Manuscripts.Text = ""
                txt_Fine2_BBGeneral.Text = ""
                txt_Fine2_BBReserve.Text = ""

                txt_Cir_Gap1.Visible = False
                txt_Fine1_Loose.Visible = False
                txt_Fine1_Books.Visible = False
                txt_Fine1_BoundJ.Visible = False
                txt_Fine1_Manuals.Visible = False
                txt_Fine1_Patents.Visible = False
                txt_Fine1_Reports.Visible = False
                txt_Fine1_Standards.Visible = False
                txt_Fine1_AV.Visible = False
                txt_Fine1_Cartographic.Visible = False
                txt_Fine1_Manuscripts.Visible = False
                txt_Fine1_BBGeneral.Visible = False
                txt_Fine1_BBReserve.Visible = False

                txt_Fine2_Loose.Visible = False
                txt_Fine2_Books.Visible = False
                txt_Fine2_BoundJ.Visible = False
                txt_Fine2_Manuals.Visible = False
                txt_Fine2_Patents.Visible = False
                txt_Fine2_Reports.Visible = False
                txt_Fine2_Standards.Visible = False
                txt_Fine2_AV.Visible = False
                txt_Fine2_Cartographic.Visible = False
                txt_Fine2_Manuscripts.Visible = False
                txt_Fine2_BBGeneral.Visible = False
                txt_Fine2_BBReserve.Visible = False

            End If
            If DDL_FineSystem.SelectedValue = "V" Then
                Label24.Text = "You have selected VARIABLE FINE Option - Separate Fine Rate for different Documents will be applied"
                txt_Cir_Gap1.Text = ""

                txt_Fine1_Loose.Text = ""
                txt_Fine1_Books.Text = ""
                txt_Fine1_BoundJ.Text = ""
                txt_Fine1_Manuals.Text = ""
                txt_Fine1_Patents.Text = ""
                txt_Fine1_Reports.Text = ""
                txt_Fine1_Standards.Text = ""
                txt_Fine1_AV.Text = ""
                txt_Fine1_Cartographic.Text = ""
                txt_Fine1_Manuscripts.Text = ""
                txt_Fine1_BBGeneral.Text = ""
                txt_Fine1_BBReserve.Text = ""

                txt_Fine2_Loose.Text = ""
                txt_Fine2_Books.Text = ""
                txt_Fine2_BoundJ.Text = ""
                txt_Fine2_Manuals.Text = ""
                txt_Fine2_Patents.Text = ""
                txt_Fine2_Reports.Text = ""
                txt_Fine2_Standards.Text = ""
                txt_Fine2_AV.Text = ""
                txt_Fine2_Cartographic.Text = ""
                txt_Fine2_Manuscripts.Text = ""
                txt_Fine2_BBGeneral.Text = ""
                txt_Fine2_BBReserve.Text = ""

                txt_Cir_Gap1.Visible = True
                txt_Fine1_Loose.Visible = True
                txt_Fine1_Books.Visible = True
                txt_Fine1_BoundJ.Visible = True
                txt_Fine1_Manuals.Visible = True
                txt_Fine1_Patents.Visible = True
                txt_Fine1_Reports.Visible = True
                txt_Fine1_Standards.Visible = True
                txt_Fine1_AV.Visible = True
                txt_Fine1_Cartographic.Visible = True
                txt_Fine1_Manuscripts.Visible = True
                txt_Fine1_BBGeneral.Visible = True
                txt_Fine1_BBReserve.Visible = True

                txt_Fine2_Loose.Visible = True
                txt_Fine2_Books.Visible = True
                txt_Fine2_BoundJ.Visible = True
                txt_Fine2_Manuals.Visible = True
                txt_Fine2_Patents.Visible = True
                txt_Fine2_Reports.Visible = True
                txt_Fine2_Standards.Visible = True
                txt_Fine2_AV.Visible = True
                txt_Fine2_Cartographic.Visible = True
                txt_Fine2_Manuscripts.Visible = True
                txt_Fine2_BBGeneral.Visible = True
                txt_Fine2_BBReserve.Visible = True
            End If
            DDL_FineSystem.Focus()
        End If
    End Sub
    'get display sub cat 
    'get value of row from grid
    Private Sub Grid2_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles Grid2.RowCommand
        Dim Command As SqlCommand = Nothing
        Dim dr As SqlDataReader = Nothing
        Try
            If e.CommandName = "Select" Then
                Dim myRowID, SUBCAT_ID As Integer
                myRowID = e.CommandArgument.ToString()
                SUBCAT_ID = Grid2.DataKeys(myRowID).Value
                If Not String.IsNullOrEmpty(SUBCAT_ID) And SUBCAT_ID <> 0 Then
                    Label8.Text = SUBCAT_ID
                    Dim iloop As Integer
                    Dim strcurrentchar As Object
                    Dim c As Integer
                    Dim counter1 As Integer

                    SUBCAT_ID = TrimX(SUBCAT_ID)
                    SUBCAT_ID = RemoveQuotes(SUBCAT_ID)

                    If Len(SUBCAT_ID).ToString > 10 Then
                        Label5.Text = "Length of Input is not Proper!"
                        Label7.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Length of Input is not Proper... ');", True)
                        Exit Sub
                    End If
                    SUBCAT_ID = " " & SUBCAT_ID & " "
                    If InStr(1, SUBCAT_ID, " CREATE ", 1) > 0 Or InStr(1, SUBCAT_ID, " DELETE ", 1) > 0 Or InStr(1, SUBCAT_ID, " DROP ", 1) > 0 Or InStr(1, SUBCAT_ID, " INSERT ", 1) > 1 Or InStr(1, SUBCAT_ID, " TRACK ", 1) > 1 Or InStr(1, SUBCAT_ID, " TRACE ", 1) > 1 Then
                        Label5.Text = "Do not use reserve words... !"
                        Label7.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do not use reserve words... ');", True)
                        Exit Sub
                    End If
                    SUBCAT_ID = TrimX(SUBCAT_ID)
                    'get record details from database
                    Dim SQL As String = Nothing
                    SQL = " SELECT *  FROM SUB_CATEGORIES WHERE (SUBCAT_ID = '" & Trim(SUBCAT_ID) & "') "
                    Command = New SqlCommand(SQL, SqlConn)
                    SqlConn.Open()
                    dr = Command.ExecuteReader(CommandBehavior.CloseConnection)
                    dr.Read()

                    If dr.HasRows = True Then
                        If dr.Item("SUBCAT_NAME").ToString <> "" Then
                            txt_Cir_SubCatName.Text = dr.Item("SUBCAT_NAME").ToString
                        Else
                            txt_Cir_SubCatName.Text = ""
                        End If

                        If dr.Item("SUBCAT_DESC").ToString <> "" Then
                            txt_Cir_SubCatRemarks.Text = dr.Item("SUBCAT_DESC").ToString
                        Else
                            txt_Cir_SubCatRemarks.Text = ""
                        End If

                        If dr.Item("FINE_SYSTEM").ToString <> "" Then
                            DDL_FineSystem.SelectedValue = dr.Item("FINE_SYSTEM").ToString
                            If dr.Item("FINE_SYSTEM").ToString = "F" Then
                                txt_Cir_Gap1.Visible = False

                                txt_Fine2_Loose.Visible = True
                                txt_Fine2_Books.Visible = True
                                txt_Fine2_BoundJ.Visible = True
                                txt_Fine2_Manuals.Visible = True
                                txt_Fine2_Patents.Visible = True
                                txt_Fine2_Reports.Visible = True
                                txt_Fine2_Standards.Visible = True
                                txt_Fine2_AV.Visible = True
                                txt_Fine2_Cartographic.Visible = True
                                txt_Fine2_Manuscripts.Visible = True
                                txt_Fine2_BBGeneral.Visible = True
                                txt_Fine2_BBReserve.Visible = True

                                txt_Cir_Gap1.Visible = False
                                txt_Cir_Gap1.Visible = False
                                txt_Fine1_Loose.Visible = False
                                txt_Fine1_Books.Visible = False
                                txt_Fine1_BoundJ.Visible = False
                                txt_Fine1_Manuals.Visible = False
                                txt_Fine1_Patents.Visible = False
                                txt_Fine1_Reports.Visible = False
                                txt_Fine1_Standards.Visible = False
                                txt_Fine1_AV.Visible = False
                                txt_Fine1_Cartographic.Visible = False
                                txt_Fine1_Manuscripts.Visible = False
                                txt_Fine1_BBGeneral.Visible = False
                                txt_Fine1_BBReserve.Visible = False

                            End If
                            If dr.Item("FINE_SYSTEM").ToString = "V" Then
                                txt_Cir_Gap1.Visible = True
                                txt_Cir_Gap1.Visible = True
                                txt_Fine1_Loose.Visible = True
                                txt_Fine1_Books.Visible = True
                                txt_Fine1_BoundJ.Visible = True
                                txt_Fine1_Manuals.Visible = True
                                txt_Fine1_Patents.Visible = True
                                txt_Fine1_Reports.Visible = True
                                txt_Fine1_Standards.Visible = True
                                txt_Fine1_AV.Visible = True
                                txt_Fine1_Cartographic.Visible = True
                                txt_Fine1_Manuscripts.Visible = True
                                txt_Fine1_BBGeneral.Visible = True
                                txt_Fine1_BBReserve.Visible = True

                                txt_Fine2_Loose.Visible = True
                                txt_Fine2_Books.Visible = True
                                txt_Fine2_BoundJ.Visible = True
                                txt_Fine2_Manuals.Visible = True
                                txt_Fine2_Patents.Visible = True
                                txt_Fine2_Reports.Visible = True
                                txt_Fine2_Standards.Visible = True
                                txt_Fine2_AV.Visible = True
                                txt_Fine2_Cartographic.Visible = True
                                txt_Fine2_Manuscripts.Visible = True
                                txt_Fine2_BBGeneral.Visible = True
                                txt_Fine2_BBReserve.Visible = True
                            End If
                            If dr.Item("FINE_SYSTEM").ToString = "N" Then

                                txt_Cir_Gap1.Visible = False
                                txt_Cir_Gap1.Visible = False
                                txt_Fine1_Loose.Visible = False
                                txt_Fine1_Books.Visible = False
                                txt_Fine1_BoundJ.Visible = False
                                txt_Fine1_Manuals.Visible = False
                                txt_Fine1_Patents.Visible = False
                                txt_Fine1_Reports.Visible = False
                                txt_Fine1_Standards.Visible = False
                                txt_Fine1_AV.Visible = False
                                txt_Fine1_Cartographic.Visible = False
                                txt_Fine1_Manuscripts.Visible = False
                                txt_Fine1_BBGeneral.Visible = False
                                txt_Fine1_BBReserve.Visible = False

                                txt_Fine2_Loose.Visible = False
                                txt_Fine2_Books.Visible = False
                                txt_Fine2_BoundJ.Visible = False
                                txt_Fine2_Manuals.Visible = False
                                txt_Fine2_Patents.Visible = False
                                txt_Fine2_Reports.Visible = False
                                txt_Fine2_Standards.Visible = False
                                txt_Fine2_AV.Visible = False
                                txt_Fine2_Cartographic.Visible = False
                                txt_Fine2_Manuscripts.Visible = False
                                txt_Fine2_BBGeneral.Visible = False
                                txt_Fine2_BBReserve.Visible = False
                            End If
                        Else
                            DDL_FineSystem.ClearSelection()
                        End If

                        If dr.Item("FINE1_GAP").ToString <> "" Then
                            txt_Cir_Gap1.Text = dr.Item("FINE1_GAP").ToString
                        Else
                            txt_Cir_Gap1.Text = ""
                        End If

                        If dr.Item("ENTITLEMENT_BOOKS").ToString <> "" Then
                            txt_Ent_Books.Text = dr.Item("ENTITLEMENT_BOOKS").ToString
                        Else
                            txt_Ent_Books.Text = ""
                        End If

                        If dr.Item("ENTITLEMENT_MANUALS").ToString <> "" Then
                            txt_Ent_Manuals.Text = dr.Item("ENTITLEMENT_MANUALS").ToString
                        Else
                            txt_Ent_Manuals.Text = ""
                        End If

                        If dr.Item("ENTITLEMENT_PATENTS").ToString <> "" Then
                            txt_Ent_Patents.Text = dr.Item("ENTITLEMENT_PATENTS").ToString
                        Else
                            txt_Ent_Patents.Text = ""
                        End If

                        If dr.Item("ENTITLEMENT_REPORTS").ToString <> "" Then
                            txt_Ent_Reports.Text = dr.Item("ENTITLEMENT_REPORTS").ToString
                        Else
                            txt_Ent_Reports.Text = ""
                        End If

                        If dr.Item("ENTITLEMENT_STANDARDS").ToString <> "" Then
                            txt_Ent_Standards.Text = dr.Item("ENTITLEMENT_STANDARDS").ToString
                        Else
                            txt_Ent_Standards.Text = ""
                        End If

                        If dr.Item("ENTITLEMENT_LOOSE").ToString <> "" Then
                            txt_Ent_LooseIssues.Text = dr.Item("ENTITLEMENT_LOOSE").ToString
                        Else
                            txt_Ent_LooseIssues.Text = ""
                        End If

                        If dr.Item("ENTITLEMENT_BOUNDJ").ToString <> "" Then
                            txt_Ent_BoundJ.Text = dr.Item("ENTITLEMENT_BOUNDJ").ToString
                        Else
                            txt_Ent_BoundJ.Text = ""
                        End If

                        If dr.Item("ENTITLEMENT_AV").ToString <> "" Then
                            txt_Ent_AV.Text = dr.Item("ENTITLEMENT_AV").ToString
                        Else
                            txt_Ent_AV.Text = ""
                        End If

                        If dr.Item("ENTITLEMENT_CARTOGRAPHIC").ToString <> "" Then
                            txt_Ent_Cartographic.Text = dr.Item("ENTITLEMENT_CARTOGRAPHIC").ToString
                        Else
                            txt_Ent_Cartographic.Text = ""
                        End If

                        If dr.Item("ENTITLEMENT_MANUSCRIPTS").ToString <> "" Then
                            txt_Ent_Manuscripts.Text = dr.Item("ENTITLEMENT_MANUSCRIPTS").ToString
                        Else
                            txt_Ent_Manuscripts.Text = ""
                        End If

                        If dr.Item("ENTITLEMENT_BBGENERAL").ToString <> "" Then
                            txt_Ent_BBGeneral.Text = dr.Item("ENTITLEMENT_BBGENERAL").ToString
                        Else
                            txt_Ent_BBGeneral.Text = ""
                        End If

                        If dr.Item("ENTITLEMENT_BBRESERVE").ToString <> "" Then
                            txt_Ent_BBReserve.Text = dr.Item("ENTITLEMENT_BBRESERVE").ToString
                        Else
                            txt_Ent_BBReserve.Text = ""
                        End If

                        If dr.Item("ENTITLEMENT_NONRETURNABLE").ToString <> "" Then
                            txt_Ent_NonReturnable.Text = dr.Item("ENTITLEMENT_NONRETURNABLE").ToString
                        Else
                            txt_Ent_NonReturnable.Text = ""
                        End If







                        If dr.Item("DUEDAYS_BOOKS").ToString <> "" Then
                            txt_DueDays_Books.Text = dr.Item("DUEDAYS_BOOKS").ToString
                        Else
                            txt_DueDays_Books.Text = ""
                        End If

                        If dr.Item("DUEDAYS_MANUALS").ToString <> "" Then
                            txt_DueDays_Manuals.Text = dr.Item("DUEDAYS_MANUALS").ToString
                        Else
                            txt_DueDays_Manuals.Text = ""
                        End If

                        If dr.Item("DUEDAYS_PATENTS").ToString <> "" Then
                            txt_DueDays_Patents.Text = dr.Item("DUEDAYS_PATENTS").ToString
                        Else
                            txt_DueDays_Patents.Text = ""
                        End If

                        If dr.Item("DUEDAYS_REPORTS").ToString <> "" Then
                            txt_DueDays_Reports.Text = dr.Item("DUEDAYS_REPORTS").ToString
                        Else
                            txt_DueDays_Reports.Text = ""
                        End If

                        If dr.Item("DUEDAYS_STANDARDS").ToString <> "" Then
                            txt_DueDays_Standards.Text = dr.Item("DUEDAYS_STANDARDS").ToString
                        Else
                            txt_DueDays_Standards.Text = ""
                        End If

                        If dr.Item("DUEDAYS_LOOSE").ToString <> "" Then
                            txt_DueDays_Loose.Text = dr.Item("DUEDAYS_LOOSE").ToString
                        Else
                            txt_DueDays_Loose.Text = ""
                        End If

                        If dr.Item("DUEDAYS_BOUNDJ").ToString <> "" Then
                            txt_DueDays_BoundJ.Text = dr.Item("DUEDAYS_BOUNDJ").ToString
                        Else
                            txt_DueDays_BoundJ.Text = ""
                        End If

                        If dr.Item("DUEDAYS_AV").ToString <> "" Then
                            txt_DueDays_AV.Text = dr.Item("DUEDAYS_AV").ToString
                        Else
                            txt_DueDays_AV.Text = ""
                        End If

                        If dr.Item("DUEDAYS_CARTOGRAPHIC").ToString <> "" Then
                            txt_DueDays_Cartographic.Text = dr.Item("DUEDAYS_CARTOGRAPHIC").ToString
                        Else
                            txt_DueDays_Cartographic.Text = ""
                        End If

                        If dr.Item("DUEDAYS_MANUSCRIPTS").ToString <> "" Then
                            txt_DueDays_Manuscripts.Text = dr.Item("DUEDAYS_MANUSCRIPTS").ToString
                        Else
                            txt_DueDays_Manuscripts.Text = ""
                        End If

                        If dr.Item("DUEDAYS_BBGENERAL").ToString <> "" Then
                            txt_DueDays_BBGeneral.Text = dr.Item("DUEDAYS_BBGENERAL").ToString
                        Else
                            txt_DueDays_BBGeneral.Text = ""
                        End If

                        If dr.Item("DUEDAYS_BBRESERVE").ToString <> "" Then
                            txt_DueDays_BBReserve.Text = dr.Item("DUEDAYS_BBRESERVE").ToString
                        Else
                            txt_DueDays_BBReserve.Text = ""
                        End If

                        If dr.Item("DUEDAYS_NONRETURNABLE").ToString <> "" Then
                            txt_DueDays_NonReturnable.Text = dr.Item("DUEDAYS_NONRETURNABLE").ToString
                        Else
                            txt_DueDays_NonReturnable.Text = ""
                        End If









                        If dr.Item("FINE1_BOOKS").ToString <> "" Then
                            txt_Fine1_Books.Text = dr.Item("FINE1_BOOKS").ToString
                        Else
                            txt_Fine1_Books.Text = ""
                        End If

                        If dr.Item("FINE1_MANUALS").ToString <> "" Then
                            txt_Fine1_Manuals.Text = dr.Item("FINE1_MANUALS").ToString
                        Else
                            txt_Fine1_Manuals.Text = ""
                        End If

                        If dr.Item("FINE1_PATENTS").ToString <> "" Then
                            txt_Fine1_Patents.Text = dr.Item("FINE1_PATENTS").ToString
                        Else
                            txt_Fine1_Patents.Text = ""
                        End If

                        If dr.Item("FINE1_REPORTS").ToString <> "" Then
                            txt_Fine1_Reports.Text = dr.Item("FINE1_REPORTS").ToString
                        Else
                            txt_Fine1_Reports.Text = ""
                        End If

                        If dr.Item("FINE1_STANDARDS").ToString <> "" Then
                            txt_Fine1_Standards.Text = dr.Item("FINE1_STANDARDS").ToString
                        Else
                            txt_Fine1_Standards.Text = ""
                        End If

                        If dr.Item("FINE1_LOOSE").ToString <> "" Then
                            txt_Fine1_Loose.Text = dr.Item("FINE1_LOOSE").ToString
                        Else
                            txt_Fine1_Loose.Text = ""
                        End If

                        If dr.Item("FINE1_BOUNDJ").ToString <> "" Then
                            txt_Fine1_BoundJ.Text = dr.Item("FINE1_BOUNDJ").ToString
                        Else
                            txt_Fine1_BoundJ.Text = ""
                        End If

                        If dr.Item("FINE1_AV").ToString <> "" Then
                            txt_Fine1_AV.Text = dr.Item("FINE1_AV").ToString
                        Else
                            txt_Fine1_AV.Text = ""
                        End If

                        If dr.Item("FINE1_CARTOGRAPHIC").ToString <> "" Then
                            txt_Fine1_Cartographic.Text = dr.Item("FINE1_CARTOGRAPHIC").ToString
                        Else
                            txt_Fine1_Cartographic.Text = ""
                        End If

                        If dr.Item("FINE1_MANUSCRIPTS").ToString <> "" Then
                            txt_Fine1_Manuscripts.Text = dr.Item("FINE1_MANUSCRIPTS").ToString
                        Else
                            txt_Fine1_Manuscripts.Text = ""
                        End If

                        If dr.Item("FINE1_BBGENERAL").ToString <> "" Then
                            txt_Fine1_BBGeneral.Text = dr.Item("FINE1_BBGENERAL").ToString
                        Else
                            txt_Fine1_BBGeneral.Text = ""
                        End If

                        If dr.Item("FINE1_BBRESERVE").ToString <> "" Then
                            txt_Fine1_BBReserve.Text = dr.Item("FINE1_BBRESERVE").ToString
                        Else
                            txt_Fine1_BBReserve.Text = ""
                        End If







                        If dr.Item("FINE2_BOOKS").ToString <> "" Then
                            txt_Fine2_Books.Text = dr.Item("FINE2_BOOKS").ToString
                        Else
                            txt_Fine2_Books.Text = ""
                        End If

                        If dr.Item("FINE2_MANUALS").ToString <> "" Then
                            txt_Fine2_Manuals.Text = dr.Item("FINE2_MANUALS").ToString
                        Else
                            txt_Fine2_Manuals.Text = ""
                        End If

                        If dr.Item("FINE2_PATENTS").ToString <> "" Then
                            txt_Fine2_Patents.Text = dr.Item("FINE2_PATENTS").ToString
                        Else
                            txt_Fine2_Patents.Text = ""
                        End If

                        If dr.Item("FINE2_REPORTS").ToString <> "" Then
                            txt_Fine2_Reports.Text = dr.Item("FINE2_REPORTS").ToString
                        Else
                            txt_Fine2_Reports.Text = ""
                        End If

                        If dr.Item("FINE2_STANDARDS").ToString <> "" Then
                            txt_Fine2_Standards.Text = dr.Item("FINE2_STANDARDS").ToString
                        Else
                            txt_Fine2_Standards.Text = ""
                        End If

                        If dr.Item("FINE2_LOOSE").ToString <> "" Then
                            txt_Fine2_Loose.Text = dr.Item("FINE2_LOOSE").ToString
                        Else
                            txt_Fine2_Loose.Text = ""
                        End If

                        If dr.Item("FINE2_BOUNDJ").ToString <> "" Then
                            txt_Fine2_BoundJ.Text = dr.Item("FINE2_BOUNDJ").ToString
                        Else
                            txt_Fine2_BoundJ.Text = ""
                        End If

                        If dr.Item("FINE2_AV").ToString <> "" Then
                            txt_Fine2_AV.Text = dr.Item("FINE2_AV").ToString
                        Else
                            txt_Fine2_AV.Text = ""
                        End If

                        If dr.Item("FINE2_CARTOGRAPHIC").ToString <> "" Then
                            txt_Fine2_Cartographic.Text = dr.Item("FINE2_CARTOGRAPHIC").ToString
                        Else
                            txt_Fine2_Cartographic.Text = ""
                        End If

                        If dr.Item("FINE2_MANUSCRIPTS").ToString <> "" Then
                            txt_Fine2_Manuscripts.Text = dr.Item("FINE2_MANUSCRIPTS").ToString
                        Else
                            txt_Fine2_Manuscripts.Text = ""
                        End If

                        If dr.Item("FINE2_BBGENERAL").ToString <> "" Then
                            txt_Fine2_BBGeneral.Text = dr.Item("FINE2_BBGENERAL").ToString
                        Else
                            txt_Fine2_BBGeneral.Text = ""
                        End If

                        If dr.Item("FINE2_BBRESERVE").ToString <> "" Then
                            txt_Fine2_BBReserve.Text = dr.Item("FINE2_BBRESERVE").ToString
                        Else
                            txt_Fine2_BBReserve.Text = ""
                        End If

                        SubCat_Update_Bttn.Visible = True
                        SubCat_Update_Bttn.Enabled = True
                        SubCat_Save_Bttn.Visible = False
                        Label5.Text = ""
                        Label7.Text = "Press UPDATE Button to save the Changes if any.."
                        dr.Close()
                    Else
                        SubCat_Update_Bttn.Visible = False
                        SubCat_Update_Bttn.Enabled = False
                        SubCat_Save_Bttn.Visible = True
                        Label8.Text = ""
                        Label5.Text = "Record Not Selected to Edit"
                        Label7.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' No Record to Edit... ');", True)
                    End If
                Else
                    SubCat_Update_Bttn.Visible = False
                    SubCat_Update_Bttn.Enabled = False
                    SubCat_Save_Bttn.Visible = True
                    Label8.Text = ""
                    Label5.Text = "Record Not Selected to Edit"
                    Label7.Text = ""
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' No Record to Edit... ');", True)
                End If
            End If
        Catch s As Exception
            Label5.Text = "Error: " & (s.Message())
            Label7.Text = ""
            Label8.Text = ""
            SubCat_Update_Bttn.Visible = False
            SubCat_Update_Bttn.Enabled = False
            SubCat_Save_Bttn.Visible = True
        Finally
            SqlConn.Close()
        End Try
    End Sub
    Protected Sub SubCat_Cancel_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SubCat_Cancel_Bttn.Click
        DDL_FineSystem.ClearSelection()
        DDL_FineSystem.SelectedValue = "N"
        DDL_FineSystem_SelectedIndexChanged(sender, e)

        txt_Cir_Gap1.Text = ""

        txt_Fine1_Loose.Text = ""
        txt_Fine1_Books.Text = ""
        txt_Fine1_BoundJ.Text = ""
        txt_Fine1_Manuals.Text = ""
        txt_Fine1_Patents.Text = ""
        txt_Fine1_Reports.Text = ""
        txt_Fine1_Standards.Text = ""
        txt_Fine1_AV.Text = ""
        txt_Fine1_Cartographic.Text = ""
        txt_Fine1_Manuscripts.Text = ""
        txt_Fine1_BBGeneral.Text = ""
        txt_Fine1_BBReserve.Text = ""

        txt_Fine2_Loose.Text = ""
        txt_Fine2_Books.Text = ""
        txt_Fine2_BoundJ.Text = ""
        txt_Fine2_Manuals.Text = ""
        txt_Fine2_Patents.Text = ""
        txt_Fine2_Reports.Text = ""
        txt_Fine2_Standards.Text = ""
        txt_Fine2_AV.Text = ""
        txt_Fine2_Cartographic.Text = ""
        txt_Fine2_Manuscripts.Text = ""
        txt_Fine2_BBGeneral.Text = ""
        txt_Fine2_BBReserve.Text = ""

        txt_Ent_Books.Text = ""
        txt_Ent_Manuals.Text = ""
        txt_Ent_Patents.Text = ""
        txt_Ent_Reports.Text = ""
        txt_Ent_Standards.Text = ""
        txt_Ent_LooseIssues.Text = ""
        txt_Ent_BoundJ.Text = ""
        txt_Ent_Cartographic.Text = ""
        txt_Ent_Manuscripts.Text = ""
        txt_Ent_BBGeneral.Text = ""
        txt_Ent_BBReserve.Text = ""
        txt_Ent_AV.Text = ""
        txt_Ent_NonReturnable.Text = ""

        txt_DueDays_Books.Text = ""
        txt_DueDays_Manuals.Text = ""
        txt_DueDays_Patents.Text = ""
        txt_DueDays_Reports.Text = ""
        txt_DueDays_Standards.Text = ""
        txt_DueDays_Loose.Text = ""
        txt_DueDays_BoundJ.Text = ""
        txt_DueDays_Cartographic.Text = ""
        txt_DueDays_Manuscripts.Text = ""
        txt_DueDays_BBGeneral.Text = ""
        txt_DueDays_BBReserve.Text = ""
        txt_Cir_SubCatName.Text = ""
        txt_Cir_SubCatRemarks.Text = ""
        txt_DueDays_AV.Text = ""
        txt_DueDays_NonReturnable.Text = ""

        Label8.Text = ""
        SubCat_Save_Bttn.Visible = True
        SubCat_Update_Bttn.Visible = False
    End Sub
    'update sub-cat records
    Protected Sub SubCat_Update_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SubCat_Update_Bttn.Click
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
                Dim counter1, counter2, counter3, counter4, counter5, counter6, counter7, counter8, counter9, counter10, counter11, counter12, counter13, counter14, counter15 As Integer
                '****************************************************************************************************
                'Server Validation for BUDG_HEAD
                Dim SUBCAT_ID As Integer = Nothing
                If Label8.Text <> "" Then
                    SUBCAT_ID = Trim(Label8.Text)
                    SUBCAT_ID = RemoveQuotes(SUBCAT_ID)

                    If IsNumeric(SUBCAT_ID) = False Then
                        Label5.Text = "Plz Select Record to Edit!"
                        Label7.Text = ""
                        Exit Sub
                    End If

                    If SUBCAT_ID.ToString.Length > 10 Then
                        Label5.Text = "Plz Enter Data with Proper Length!"
                        Label7.Text = ""
                        Exit Sub
                    End If

                    SUBCAT_ID = " " & SUBCAT_ID & " "
                    If InStr(1, SUBCAT_ID, "CREATE", 1) > 0 Or InStr(1, SUBCAT_ID, "DELETE", 1) > 0 Or InStr(1, SUBCAT_ID, "DROP", 1) > 0 Or InStr(1, SUBCAT_ID, "INSERT", 1) > 1 Or InStr(1, SUBCAT_ID, "TRACK", 1) > 1 Or InStr(1, SUBCAT_ID, "TRACE", 1) > 1 Then
                        Label5.Text = "Do Not Use Reserve Words!"
                        Label7.Text = ""
                        Exit Sub
                    End If

                    SUBCAT_ID = TrimX(SUBCAT_ID)

                    c = 0
                    counter1 = 0
                    For iloop = 1 To Len(SUBCAT_ID.ToString)
                        strcurrentchar = Mid(SUBCAT_ID, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter1 = 1
                            End If
                        End If
                    Next

                    If counter1 = 1 Then
                        Label5.Text = "Do Not Use Un-Wanted Characters!"
                        Label7.Text = ""
                        Exit Sub
                    End If
                Else
                    Label5.Text = "Plz Select Record Record for Edit!"
                    Label7.Text = ""
                    Exit Sub
                End If

                'validation for App No
                Dim SUBCAT_NAME As Object = Nothing
                If Me.txt_Cir_SubCatName.Text <> "" Then
                    SUBCAT_NAME = TrimAll(txt_Cir_SubCatName.Text)
                    SUBCAT_NAME = RemoveQuotes(SUBCAT_NAME)
                    If SUBCAT_NAME.Length > 200 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Cir_SubCatName.Focus()
                        Exit Sub
                    End If
                    SUBCAT_NAME = " " & SUBCAT_NAME & " "
                    If InStr(1, SUBCAT_NAME, "CREATE", 1) > 0 Or InStr(1, SUBCAT_NAME, "DELETE", 1) > 0 Or InStr(1, SUBCAT_NAME, "DROP", 1) > 0 Or InStr(1, SUBCAT_NAME, "INSERT", 1) > 1 Or InStr(1, SUBCAT_NAME, "TRACK", 1) > 1 Or InStr(1, SUBCAT_NAME, "TRACE", 1) > 1 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Cir_SubCatName.Focus()
                        Exit Sub
                    End If
                    SUBCAT_NAME = TrimAll(SUBCAT_NAME)

                    'check duplicate category
                    Dim str As Object = Nothing
                    Dim flag As Object = Nothing
                    str = "SELECT SUBCAT_NAME FROM SUB_CATEGORIES WHERE (SUBCAT_NAME ='" & Trim(SUBCAT_NAME) & "') AND (LIB_CODE ='" & Trim(LibCode) & "') AND (SUBCAT_ID <> '" & Trim(SUBCAT_ID) & "')"
                    Dim cmd1 As New SqlCommand(str, SqlConn)
                    SqlConn.Open()
                    flag = cmd1.ExecuteScalar
                    If flag <> Nothing Then
                        Label5.Text = "This Sub Category already exists !"
                        Label7.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('This Approval No has already been processed, Use another one !');", True)
                        txt_Cir_SubCatName.Focus()
                        Exit Sub
                    End If
                    SqlConn.Close()
                Else
                    Label5.Text = "Plz Enter the Sub Category Name !"
                    Label7.Text = ""
                    txt_Cir_SubCatName.Focus()
                    Exit Sub
                End If

                'validation for Fine System
                Dim FINE_SYSTEM As Object = Nothing
                If Me.DDL_FineSystem.Text <> "" Then
                    FINE_SYSTEM = Trim(DDL_FineSystem.Text)
                    FINE_SYSTEM = RemoveQuotes(FINE_SYSTEM)
                    If FINE_SYSTEM.Length > 2 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        DDL_FineSystem.Focus()
                        Exit Sub
                    End If
                    FINE_SYSTEM = " " & FINE_SYSTEM & " "
                    If InStr(1, FINE_SYSTEM, "CREATE", 1) > 0 Or InStr(1, FINE_SYSTEM, "DELETE", 1) > 0 Or InStr(1, FINE_SYSTEM, "DROP", 1) > 0 Or InStr(1, FINE_SYSTEM, "INSERT", 1) > 1 Or InStr(1, FINE_SYSTEM, "TRACK", 1) > 1 Or InStr(1, FINE_SYSTEM, "TRACE", 1) > 1 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        DDL_FineSystem.Focus()
                        Exit Sub
                    End If
                    FINE_SYSTEM = TrimX(FINE_SYSTEM)
                Else
                    FINE_SYSTEM = "N"
                End If

                'Server validation for  : txt_Cat_Remarks
                Dim SUBCAT_DESC As Object = Nothing
                If txt_Cir_SubCatRemarks.Text <> "" Then
                    SUBCAT_DESC = TrimAll(txt_Cir_SubCatRemarks.Text)
                    SUBCAT_DESC = RemoveQuotes(SUBCAT_DESC)
                    If SUBCAT_DESC.Length > 250 Then 'maximum length
                        Label5.Text = " Data must be of Proper Length.. "
                        Label7.Text = ""
                        txt_Cir_SubCatRemarks.Focus()
                        Exit Sub
                    End If

                    SUBCAT_DESC = " " & SUBCAT_DESC & " "
                    If InStr(1, SUBCAT_DESC, "CREATE", 1) > 0 Or InStr(1, SUBCAT_DESC, "DELETE", 1) > 0 Or InStr(1, SUBCAT_DESC, "DROP", 1) > 0 Or InStr(1, SUBCAT_DESC, "INSERT", 1) > 1 Or InStr(1, SUBCAT_DESC, "TRACK", 1) > 1 Or InStr(1, SUBCAT_DESC, "TRACE", 1) > 1 Then
                        Label5.Text = " Do Not use Reserve Words... "
                        Label7.Text = ""
                        Me.txt_Cir_SubCatRemarks.Focus()
                        Exit Sub
                    End If
                    SUBCAT_DESC = TrimAll(SUBCAT_DESC)
                Else
                    SUBCAT_DESC = ""
                End If


                'FINE1_GAP
                Dim FINE1_GAP As Integer = Nothing
                If txt_Cir_Gap1.Visible = True Then
                    If Me.txt_Cir_Gap1.Text <> "" Then
                        FINE1_GAP = Convert.ToInt16(TrimX(txt_Cir_Gap1.Text))
                        FINE1_GAP = RemoveQuotes(FINE1_GAP)
                        If Not IsNumeric(FINE1_GAP) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Cir_Gap1.Focus()
                            Exit Sub
                        End If
                        If Len(FINE1_GAP) > 5 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Cir_Gap1.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE1_GAP = Nothing
                    End If
                End If

                'Books Entitlement
                Dim ENTITLEMENT_BOOKS As Integer = Nothing
                If Me.txt_Ent_Books.Text <> "" Then
                    ENTITLEMENT_BOOKS = Convert.ToInt16(TrimX(txt_Ent_Books.Text))
                    ENTITLEMENT_BOOKS = RemoveQuotes(ENTITLEMENT_BOOKS)
                    If Not IsNumeric(ENTITLEMENT_BOOKS) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_Books.Focus()
                        Exit Sub
                    End If
                    If Len(ENTITLEMENT_BOOKS) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_Books.Focus()
                        Exit Sub
                    End If
                Else
                    ENTITLEMENT_BOOKS = Nothing
                End If

                'Manuals Entitlement
                Dim ENTITLEMENT_MANUALS As Integer = Nothing
                If Me.txt_Ent_Manuals.Text <> "" Then
                    ENTITLEMENT_MANUALS = Convert.ToInt16(TrimX(txt_Ent_Manuals.Text))
                    ENTITLEMENT_MANUALS = RemoveQuotes(ENTITLEMENT_MANUALS)
                    If Not IsNumeric(ENTITLEMENT_MANUALS) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_Manuals.Focus()
                        Exit Sub
                    End If
                    If Len(ENTITLEMENT_MANUALS) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_Manuals.Focus()
                        Exit Sub
                    End If
                Else
                    ENTITLEMENT_MANUALS = Nothing
                End If


                'PATENTS Entitlement
                Dim ENTITLEMENT_PATENTS As Integer = Nothing
                If Me.txt_Ent_Patents.Text <> "" Then
                    ENTITLEMENT_PATENTS = Convert.ToInt16(TrimX(txt_Ent_Patents.Text))
                    ENTITLEMENT_PATENTS = RemoveQuotes(ENTITLEMENT_PATENTS)
                    If Not IsNumeric(ENTITLEMENT_PATENTS) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_Patents.Focus()
                        Exit Sub
                    End If
                    If Len(ENTITLEMENT_PATENTS) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_Patents.Focus()
                        Exit Sub
                    End If
                Else
                    ENTITLEMENT_PATENTS = Nothing
                End If

                'REPORTS Entitlement
                Dim ENTITLEMENT_REPORTS As Integer = Nothing
                If Me.txt_Ent_Reports.Text <> "" Then
                    ENTITLEMENT_REPORTS = Convert.ToInt16(TrimX(txt_Ent_Reports.Text))
                    ENTITLEMENT_REPORTS = RemoveQuotes(ENTITLEMENT_REPORTS)
                    If Not IsNumeric(ENTITLEMENT_REPORTS) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_Reports.Focus()
                        Exit Sub
                    End If
                    If Len(ENTITLEMENT_REPORTS) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_Reports.Focus()
                        Exit Sub
                    End If
                Else
                    ENTITLEMENT_REPORTS = Nothing
                End If

                'STANDARDS Entitlement
                Dim ENTITLEMENT_STANDARDS As Integer = Nothing
                If Me.txt_Ent_Standards.Text <> "" Then
                    ENTITLEMENT_STANDARDS = Convert.ToInt16(TrimX(txt_Ent_Standards.Text))
                    ENTITLEMENT_STANDARDS = RemoveQuotes(ENTITLEMENT_STANDARDS)
                    If Not IsNumeric(ENTITLEMENT_STANDARDS) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_Standards.Focus()
                        Exit Sub
                    End If
                    If Len(ENTITLEMENT_STANDARDS) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_Standards.Focus()
                        Exit Sub
                    End If
                Else
                    ENTITLEMENT_STANDARDS = Nothing
                End If

                'LOOSE Entitlement
                Dim ENTITLEMENT_LOOSE As Integer = Nothing
                If Me.txt_Ent_LooseIssues.Text <> "" Then
                    ENTITLEMENT_LOOSE = Convert.ToInt16(TrimX(txt_Ent_LooseIssues.Text))
                    ENTITLEMENT_LOOSE = RemoveQuotes(ENTITLEMENT_LOOSE)
                    If Not IsNumeric(ENTITLEMENT_LOOSE) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_LooseIssues.Focus()
                        Exit Sub
                    End If
                    If Len(ENTITLEMENT_LOOSE) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_LooseIssues.Focus()
                        Exit Sub
                    End If
                Else
                    ENTITLEMENT_LOOSE = Nothing
                End If

                'BOUNDJ Entitlement
                Dim ENTITLEMENT_BOUNDJ As Integer = Nothing
                If Me.txt_Ent_BoundJ.Text <> "" Then
                    ENTITLEMENT_BOUNDJ = Convert.ToInt16(TrimX(txt_Ent_BoundJ.Text))
                    ENTITLEMENT_BOUNDJ = RemoveQuotes(ENTITLEMENT_BOUNDJ)
                    If Not IsNumeric(ENTITLEMENT_BOUNDJ) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_BoundJ.Focus()
                        Exit Sub
                    End If
                    If Len(ENTITLEMENT_BOUNDJ) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_BoundJ.Focus()
                        Exit Sub
                    End If
                Else
                    ENTITLEMENT_BOUNDJ = Nothing
                End If

                'AV Entitlement
                Dim ENTITLEMENT_AV As Integer = Nothing
                If Me.txt_Ent_AV.Text <> "" Then
                    ENTITLEMENT_AV = Convert.ToInt16(TrimX(txt_Ent_AV.Text))
                    ENTITLEMENT_AV = RemoveQuotes(ENTITLEMENT_AV)
                    If Not IsNumeric(ENTITLEMENT_AV) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_AV.Focus()
                        Exit Sub
                    End If
                    If Len(ENTITLEMENT_AV) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_AV.Focus()
                        Exit Sub
                    End If
                Else
                    ENTITLEMENT_AV = Nothing
                End If

                'CARTOGRAPHIC Entitlement
                Dim ENTITLEMENT_CARTOGRAPHIC As Integer = Nothing
                If Me.txt_Ent_Cartographic.Text <> "" Then
                    ENTITLEMENT_CARTOGRAPHIC = Convert.ToInt16(TrimX(txt_Ent_Cartographic.Text))
                    ENTITLEMENT_CARTOGRAPHIC = RemoveQuotes(ENTITLEMENT_CARTOGRAPHIC)
                    If Not IsNumeric(ENTITLEMENT_CARTOGRAPHIC) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_Cartographic.Focus()
                        Exit Sub
                    End If
                    If Len(ENTITLEMENT_CARTOGRAPHIC) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_Cartographic.Focus()
                        Exit Sub
                    End If
                Else
                    ENTITLEMENT_CARTOGRAPHIC = Nothing
                End If

                'MANUSCRIPTS Entitlement
                Dim ENTITLEMENT_MANUSCRIPTS As Integer = Nothing
                If Me.txt_Ent_Manuscripts.Text <> "" Then
                    ENTITLEMENT_MANUSCRIPTS = Convert.ToInt16(TrimX(txt_Ent_Manuscripts.Text))
                    ENTITLEMENT_MANUSCRIPTS = RemoveQuotes(ENTITLEMENT_MANUSCRIPTS)
                    If Not IsNumeric(ENTITLEMENT_MANUSCRIPTS) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_Manuscripts.Focus()
                        Exit Sub
                    End If
                    If Len(ENTITLEMENT_MANUSCRIPTS) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_Manuscripts.Focus()
                        Exit Sub
                    End If
                Else
                    ENTITLEMENT_MANUSCRIPTS = Nothing
                End If

                'BBGENERAL Entitlement
                Dim ENTITLEMENT_BBGENERAL As Integer = Nothing
                If Me.txt_Ent_BBGeneral.Text <> "" Then
                    ENTITLEMENT_BBGENERAL = Convert.ToInt16(TrimX(txt_Ent_BBGeneral.Text))
                    ENTITLEMENT_BBGENERAL = RemoveQuotes(ENTITLEMENT_BBGENERAL)
                    If Not IsNumeric(ENTITLEMENT_BBGENERAL) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_BBGeneral.Focus()
                        Exit Sub
                    End If
                    If Len(ENTITLEMENT_BBGENERAL) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_BBGeneral.Focus()
                        Exit Sub
                    End If
                Else
                    ENTITLEMENT_BBGENERAL = Nothing
                End If

                'BBRESERVE Entitlement
                Dim ENTITLEMENT_BBRESERVE As Integer = Nothing
                If Me.txt_Ent_BBReserve.Text <> "" Then
                    ENTITLEMENT_BBRESERVE = Convert.ToInt16(TrimX(txt_Ent_BBReserve.Text))
                    ENTITLEMENT_BBRESERVE = RemoveQuotes(ENTITLEMENT_BBRESERVE)
                    If Not IsNumeric(ENTITLEMENT_BBRESERVE) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_BBReserve.Focus()
                        Exit Sub
                    End If
                    If Len(ENTITLEMENT_BBRESERVE) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_BBReserve.Focus()
                        Exit Sub
                    End If
                Else
                    ENTITLEMENT_BBRESERVE = Nothing
                End If

                'Non-Returanble Entitlement
                Dim ENTITLEMENT_NONRETURNABLE As Integer = Nothing
                If Me.txt_Ent_NonReturnable.Text <> "" Then
                    ENTITLEMENT_NONRETURNABLE = Convert.ToInt16(TrimX(txt_Ent_NonReturnable.Text))
                    ENTITLEMENT_NONRETURNABLE = RemoveQuotes(ENTITLEMENT_NONRETURNABLE)
                    If Not IsNumeric(ENTITLEMENT_NONRETURNABLE) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_NonReturnable.Focus()
                        Exit Sub
                    End If
                    If Len(ENTITLEMENT_NONRETURNABLE) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_Ent_NonReturnable.Focus()
                        Exit Sub
                    End If
                Else
                    ENTITLEMENT_NONRETURNABLE = Nothing
                End If




                'Books DueDays
                Dim DUEDAYS_BOOKS As Integer = Nothing
                If Me.txt_DueDays_Books.Text <> "" Then
                    DUEDAYS_BOOKS = Convert.ToInt16(TrimX(txt_DueDays_Books.Text))
                    DUEDAYS_BOOKS = RemoveQuotes(DUEDAYS_BOOKS)
                    If Not IsNumeric(DUEDAYS_BOOKS) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_Books.Focus()
                        Exit Sub
                    End If
                    If Len(DUEDAYS_BOOKS) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_Books.Focus()
                        Exit Sub
                    End If
                Else
                    DUEDAYS_BOOKS = Nothing
                End If

                'Manuals DueDays
                Dim DUEDAYS_MANUALS As Integer = Nothing
                If Me.txt_DueDays_Manuals.Text <> "" Then
                    DUEDAYS_MANUALS = Convert.ToInt16(TrimX(txt_DueDays_Manuals.Text))
                    DUEDAYS_MANUALS = RemoveQuotes(DUEDAYS_MANUALS)
                    If Not IsNumeric(DUEDAYS_MANUALS) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_Manuals.Focus()
                        Exit Sub
                    End If
                    If Len(DUEDAYS_MANUALS) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_Manuals.Focus()
                        Exit Sub
                    End If
                Else
                    DUEDAYS_MANUALS = Nothing
                End If


                'PATENTS DueDays
                Dim DUEDAYS_PATENTS As Integer = Nothing
                If Me.txt_DueDays_Patents.Text <> "" Then
                    DUEDAYS_PATENTS = Convert.ToInt16(TrimX(txt_DueDays_Patents.Text))
                    DUEDAYS_PATENTS = RemoveQuotes(DUEDAYS_PATENTS)
                    If Not IsNumeric(DUEDAYS_PATENTS) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_Patents.Focus()
                        Exit Sub
                    End If
                    If Len(DUEDAYS_PATENTS) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_Patents.Focus()
                        Exit Sub
                    End If
                Else
                    DUEDAYS_PATENTS = Nothing
                End If

                'REPORTS DueDays
                Dim DUEDAYS_REPORTS As Integer = Nothing
                If Me.txt_DueDays_Reports.Text <> "" Then
                    DUEDAYS_REPORTS = Convert.ToInt16(TrimX(txt_DueDays_Reports.Text))
                    DUEDAYS_REPORTS = RemoveQuotes(DUEDAYS_REPORTS)
                    If Not IsNumeric(DUEDAYS_REPORTS) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_Reports.Focus()
                        Exit Sub
                    End If
                    If Len(DUEDAYS_REPORTS) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_Reports.Focus()
                        Exit Sub
                    End If
                Else
                    DUEDAYS_REPORTS = Nothing
                End If

                'STANDARDS DueDays
                Dim DUEDAYS_STANDARDS As Integer = Nothing
                If Me.txt_DueDays_Standards.Text <> "" Then
                    DUEDAYS_STANDARDS = Convert.ToInt16(TrimX(txt_DueDays_Standards.Text))
                    DUEDAYS_STANDARDS = RemoveQuotes(DUEDAYS_STANDARDS)
                    If Not IsNumeric(DUEDAYS_STANDARDS) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_Standards.Focus()
                        Exit Sub
                    End If
                    If Len(DUEDAYS_STANDARDS) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_Standards.Focus()
                        Exit Sub
                    End If
                Else
                    DUEDAYS_STANDARDS = Nothing
                End If

                'LOOSE DueDays
                Dim DUEDAYS_LOOSE As Integer = Nothing
                If Me.txt_DueDays_Loose.Text <> "" Then
                    DUEDAYS_LOOSE = Convert.ToInt16(TrimX(txt_DueDays_Loose.Text))
                    DUEDAYS_LOOSE = RemoveQuotes(DUEDAYS_LOOSE)
                    If Not IsNumeric(DUEDAYS_LOOSE) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_Loose.Focus()
                        Exit Sub
                    End If
                    If Len(DUEDAYS_LOOSE) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_Loose.Focus()
                        Exit Sub
                    End If
                Else
                    DUEDAYS_LOOSE = Nothing
                End If

                'BOUNDJ DueDays
                Dim DUEDAYS_BOUNDJ As Integer = Nothing
                If Me.txt_DueDays_BoundJ.Text <> "" Then
                    DUEDAYS_BOUNDJ = Convert.ToInt16(TrimX(txt_DueDays_BoundJ.Text))
                    DUEDAYS_BOUNDJ = RemoveQuotes(DUEDAYS_BOUNDJ)
                    If Not IsNumeric(DUEDAYS_BOUNDJ) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_BoundJ.Focus()
                        Exit Sub
                    End If
                    If Len(DUEDAYS_BOUNDJ) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_BoundJ.Focus()
                        Exit Sub
                    End If
                Else
                    DUEDAYS_BOUNDJ = Nothing
                End If

                'AV DueDays
                Dim DUEDAYS_AV As Integer = Nothing
                If Me.txt_DueDays_AV.Text <> "" Then
                    DUEDAYS_AV = Convert.ToInt16(TrimX(txt_DueDays_AV.Text))
                    DUEDAYS_AV = RemoveQuotes(DUEDAYS_AV)
                    If Not IsNumeric(DUEDAYS_AV) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_AV.Focus()
                        Exit Sub
                    End If
                    If Len(DUEDAYS_AV) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_AV.Focus()
                        Exit Sub
                    End If
                Else
                    DUEDAYS_AV = Nothing
                End If

                'CARTOGRAPHIC DueDays
                Dim DUEDAYS_CARTOGRAPHIC As Integer = Nothing
                If Me.txt_DueDays_Cartographic.Text <> "" Then
                    DUEDAYS_CARTOGRAPHIC = Convert.ToInt16(TrimX(txt_DueDays_Cartographic.Text))
                    DUEDAYS_CARTOGRAPHIC = RemoveQuotes(DUEDAYS_CARTOGRAPHIC)
                    If Not IsNumeric(DUEDAYS_CARTOGRAPHIC) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_Cartographic.Focus()
                        Exit Sub
                    End If
                    If Len(DUEDAYS_CARTOGRAPHIC) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_Cartographic.Focus()
                        Exit Sub
                    End If
                Else
                    DUEDAYS_CARTOGRAPHIC = Nothing
                End If

                'MANUSCRIPTS DueDays
                Dim DUEDAYS_MANUSCRIPTS As Integer = Nothing
                If Me.txt_DueDays_Manuscripts.Text <> "" Then
                    DUEDAYS_MANUSCRIPTS = Convert.ToInt16(TrimX(txt_DueDays_Manuscripts.Text))
                    DUEDAYS_MANUSCRIPTS = RemoveQuotes(DUEDAYS_MANUSCRIPTS)
                    If Not IsNumeric(DUEDAYS_MANUSCRIPTS) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_Manuscripts.Focus()
                        Exit Sub
                    End If
                    If Len(DUEDAYS_MANUSCRIPTS) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_Manuscripts.Focus()
                        Exit Sub
                    End If
                Else
                    DUEDAYS_MANUSCRIPTS = Nothing
                End If

                'BBGENERAL DueDays
                Dim DUEDAYS_BBGENERAL As Integer = Nothing
                If Me.txt_DueDays_BBGeneral.Text <> "" Then
                    DUEDAYS_BBGENERAL = Convert.ToInt16(TrimX(txt_DueDays_BBGeneral.Text))
                    DUEDAYS_BBGENERAL = RemoveQuotes(DUEDAYS_BBGENERAL)
                    If Not IsNumeric(DUEDAYS_BBGENERAL) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_BBGeneral.Focus()
                        Exit Sub
                    End If
                    If Len(DUEDAYS_BBGENERAL) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_BBGeneral.Focus()
                        Exit Sub
                    End If
                Else
                    DUEDAYS_BBGENERAL = Nothing
                End If

                'BBRESERVE DueDays
                Dim DUEDAYS_BBRESERVE As Integer = Nothing
                If Me.txt_DueDays_BBReserve.Text <> "" Then
                    DUEDAYS_BBRESERVE = Convert.ToInt16(TrimX(txt_DueDays_BBReserve.Text))
                    DUEDAYS_BBRESERVE = RemoveQuotes(DUEDAYS_BBRESERVE)
                    If Not IsNumeric(DUEDAYS_BBRESERVE) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_BBReserve.Focus()
                        Exit Sub
                    End If
                    If Len(DUEDAYS_BBRESERVE) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_BBReserve.Focus()
                        Exit Sub
                    End If
                Else
                    DUEDAYS_BBRESERVE = Nothing
                End If


                'NONRETURNABLE DueDays
                Dim DUEDAYS_NONRETURNABLE As Integer = Nothing
                If Me.txt_DueDays_NonReturnable.Text <> "" Then
                    DUEDAYS_NONRETURNABLE = Convert.ToInt16(TrimX(txt_DueDays_NonReturnable.Text))
                    DUEDAYS_NONRETURNABLE = RemoveQuotes(DUEDAYS_NONRETURNABLE)
                    If Not IsNumeric(DUEDAYS_NONRETURNABLE) = True Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_NonReturnable.Focus()
                        Exit Sub
                    End If
                    If Len(DUEDAYS_NONRETURNABLE) > 5 Then
                        Label5.Text = "Error: Input is not Valid !"
                        Label7.Text = ""
                        txt_DueDays_NonReturnable.Focus()
                        Exit Sub
                    End If
                Else
                    DUEDAYS_NONRETURNABLE = Nothing
                End If







                'FINE1_BOOKS
                Dim FINE1_BOOKS As Decimal = Nothing
                If txt_Fine1_Books.Visible = True Then
                    If txt_Fine1_Books.Text <> "" Then
                        FINE1_BOOKS = TrimX(txt_Fine1_Books.Text)
                        FINE1_BOOKS = RemoveQuotes(FINE1_BOOKS)
                        If FINE1_BOOKS.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_Books.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE1_BOOKS) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_Books.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE1_BOOKS = Nothing
                    End If
                End If

                'FINE1_MANUALS
                Dim FINE1_MANUALS As Decimal = Nothing
                If txt_Fine1_Manuals.Visible = True Then
                    If txt_Fine1_Manuals.Text <> "" Then
                        FINE1_MANUALS = TrimX(txt_Fine1_Manuals.Text)
                        FINE1_MANUALS = RemoveQuotes(FINE1_MANUALS)
                        If FINE1_MANUALS.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_Manuals.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE1_MANUALS) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_Manuals.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE1_MANUALS = Nothing
                    End If
                End If

                'FINE1_PATENTS
                Dim FINE1_PATENTS As Decimal = Nothing
                If txt_Fine1_Patents.Visible = True Then
                    If txt_Fine1_Patents.Text <> "" Then
                        FINE1_PATENTS = TrimX(txt_Fine1_Patents.Text)
                        FINE1_PATENTS = RemoveQuotes(FINE1_PATENTS)
                        If FINE1_PATENTS.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_Patents.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE1_PATENTS) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_Patents.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE1_PATENTS = Nothing
                    End If
                End If

                'FINE1_REPORTS
                Dim FINE1_REPORTS As Decimal = Nothing
                If txt_Fine1_Reports.Visible = True Then
                    If txt_Fine1_Reports.Text <> "" Then
                        FINE1_REPORTS = TrimX(txt_Fine1_Reports.Text)
                        FINE1_REPORTS = RemoveQuotes(FINE1_REPORTS)
                        If FINE1_REPORTS.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_Reports.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE1_REPORTS) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_Reports.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE1_REPORTS = Nothing
                    End If
                End If

                'FINE1_STANDARDS
                Dim FINE1_STANDARDS As Decimal = Nothing
                If txt_Fine1_Standards.Visible = True Then
                    If txt_Fine1_Standards.Text <> "" Then
                        FINE1_STANDARDS = TrimX(txt_Fine1_Standards.Text)
                        FINE1_STANDARDS = RemoveQuotes(FINE1_STANDARDS)
                        If FINE1_STANDARDS.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_Standards.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE1_STANDARDS) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_Standards.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE1_STANDARDS = Nothing
                    End If
                End If

                'FINE1_LOOSE
                Dim FINE1_LOOSE As Decimal = Nothing
                If txt_Fine1_Loose.Visible = True Then
                    If txt_Fine1_Loose.Text <> "" Then
                        FINE1_LOOSE = TrimX(txt_Fine1_Loose.Text)
                        FINE1_LOOSE = RemoveQuotes(FINE1_LOOSE)
                        If FINE1_LOOSE.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_Loose.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE1_LOOSE) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_Loose.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE1_LOOSE = Nothing
                    End If
                End If

                'FINE1_BOUNDJ
                Dim FINE1_BOUNDJ As Decimal = Nothing
                If txt_Fine1_BoundJ.Visible = True Then
                    If txt_Fine1_BoundJ.Text <> "" Then
                        FINE1_BOUNDJ = TrimX(txt_Fine1_BoundJ.Text)
                        FINE1_BOUNDJ = RemoveQuotes(FINE1_BOUNDJ)
                        If FINE1_LOOSE.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_BoundJ.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE1_BOUNDJ) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_BoundJ.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE1_BOUNDJ = Nothing
                    End If
                End If

                'FINE1_AV
                Dim FINE1_AV As Decimal = Nothing
                If txt_Fine1_AV.Visible = True Then
                    If txt_Fine1_AV.Text <> "" Then
                        FINE1_AV = TrimX(txt_Fine1_AV.Text)
                        FINE1_AV = RemoveQuotes(FINE1_AV)
                        If FINE1_LOOSE.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_AV.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE1_AV) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_AV.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE1_AV = Nothing
                    End If
                End If

                'FINE1_CARTOGRAPHIC
                Dim FINE1_CARTOGRAPHIC As Decimal = Nothing
                If txt_Fine1_AV.Visible = True Then
                    If txt_Fine1_Cartographic.Text <> "" Then
                        FINE1_CARTOGRAPHIC = TrimX(txt_Fine1_Cartographic.Text)
                        FINE1_CARTOGRAPHIC = RemoveQuotes(FINE1_CARTOGRAPHIC)
                        If FINE1_CARTOGRAPHIC.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_Cartographic.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE1_CARTOGRAPHIC) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_Cartographic.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE1_CARTOGRAPHIC = Nothing
                    End If
                End If

                'FINE1_MANUSCRIPTS
                Dim FINE1_MANUSCRIPTS As Decimal = Nothing
                If txt_Fine1_Manuscripts.Visible = True Then
                    If txt_Fine1_Manuscripts.Text <> "" Then
                        FINE1_MANUSCRIPTS = TrimX(txt_Fine1_Manuscripts.Text)
                        FINE1_MANUSCRIPTS = RemoveQuotes(FINE1_MANUSCRIPTS)
                        If FINE1_MANUSCRIPTS.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_Manuscripts.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE1_MANUSCRIPTS) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_Manuscripts.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE1_MANUSCRIPTS = Nothing
                    End If
                End If

                'FINE1_BBGENERAL
                Dim FINE1_BBGENERAL As Decimal = Nothing
                If txt_Fine1_BBGeneral.Visible = True Then
                    If txt_Fine1_BBGeneral.Text <> "" Then
                        FINE1_BBGENERAL = TrimX(txt_Fine1_BBGeneral.Text)
                        FINE1_BBGENERAL = RemoveQuotes(FINE1_BBGENERAL)
                        If FINE1_BBGENERAL.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_BBGeneral.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE1_BBGENERAL) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_BBGeneral.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE1_BBGENERAL = Nothing
                    End If
                End If

                'FINE1_BBRESERVE
                Dim FINE1_BBRESERVE As Decimal = Nothing
                If txt_Fine1_BBReserve.Visible = True Then
                    If txt_Fine1_BBReserve.Text <> "" Then
                        FINE1_BBRESERVE = TrimX(txt_Fine1_BBReserve.Text)
                        FINE1_BBRESERVE = RemoveQuotes(FINE1_BBRESERVE)
                        If FINE1_BBGENERAL.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_BBReserve.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE1_BBRESERVE) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine1_BBReserve.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE1_BBRESERVE = Nothing
                    End If
                End If




                'FINE2_BOOKS
                Dim FINE2_BOOKS As Decimal = Nothing
                If txt_Fine2_Books.Visible = True Then
                    If txt_Fine2_Books.Text <> "" Then
                        FINE2_BOOKS = TrimX(txt_Fine2_Books.Text)
                        FINE2_BOOKS = RemoveQuotes(FINE2_BOOKS)
                        If FINE2_BOOKS.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_Books.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE2_BOOKS) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_Books.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE2_BOOKS = Nothing
                    End If
                End If

                'FINE2_MANUALS
                Dim FINE2_MANUALS As Decimal = Nothing
                If txt_Fine2_Manuals.Visible = True Then
                    If txt_Fine2_Manuals.Text <> "" Then
                        FINE2_MANUALS = TrimX(txt_Fine2_Manuals.Text)
                        FINE2_MANUALS = RemoveQuotes(FINE2_MANUALS)
                        If FINE2_MANUALS.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_Manuals.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE2_MANUALS) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_Manuals.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE2_MANUALS = Nothing
                    End If
                End If

                'FINE2_PATENTS
                Dim FINE2_PATENTS As Decimal = Nothing
                If txt_Fine2_Patents.Visible = True Then
                    If txt_Fine2_Patents.Text <> "" Then
                        FINE2_PATENTS = TrimX(txt_Fine2_Patents.Text)
                        FINE2_PATENTS = RemoveQuotes(FINE2_PATENTS)
                        If FINE2_PATENTS.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_Patents.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE2_PATENTS) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_Patents.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE2_PATENTS = Nothing
                    End If
                End If

                'FINE2_REPORTS
                Dim FINE2_REPORTS As Decimal = Nothing
                If txt_Fine2_Reports.Visible = True Then
                    If txt_Fine2_Reports.Text <> "" Then
                        FINE2_REPORTS = TrimX(txt_Fine2_Reports.Text)
                        FINE2_REPORTS = RemoveQuotes(FINE2_REPORTS)
                        If FINE2_REPORTS.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_Reports.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE2_REPORTS) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_Reports.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE2_REPORTS = Nothing
                    End If
                End If

                'FINE2_STANDARDS
                Dim FINE2_STANDARDS As Decimal = Nothing
                If txt_Fine2_Standards.Visible = True Then
                    If txt_Fine2_Standards.Text <> "" Then
                        FINE2_STANDARDS = TrimX(txt_Fine2_Standards.Text)
                        FINE2_STANDARDS = RemoveQuotes(FINE2_STANDARDS)
                        If FINE2_STANDARDS.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_Standards.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE2_STANDARDS) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_Standards.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE2_STANDARDS = Nothing
                    End If
                End If

                'FINE2_LOOSE
                Dim FINE2_LOOSE As Decimal = Nothing
                If txt_Fine2_Loose.Visible = True Then
                    If txt_Fine2_Loose.Text <> "" Then
                        FINE2_LOOSE = TrimX(txt_Fine2_Loose.Text)
                        FINE2_LOOSE = RemoveQuotes(FINE2_LOOSE)
                        If FINE2_LOOSE.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_Loose.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE2_LOOSE) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_Loose.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE2_LOOSE = Nothing
                    End If
                End If

                'FINE2_BOUNDJ
                Dim FINE2_BOUNDJ As Decimal = Nothing
                If txt_Fine2_BoundJ.Visible = True Then
                    If txt_Fine2_BoundJ.Text <> "" Then
                        FINE2_BOUNDJ = TrimX(txt_Fine2_BoundJ.Text)
                        FINE2_BOUNDJ = RemoveQuotes(FINE2_BOUNDJ)
                        If FINE2_LOOSE.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_BoundJ.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE2_BOUNDJ) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_BoundJ.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE2_BOUNDJ = Nothing
                    End If
                End If

                'FINE2_AV
                Dim FINE2_AV As Decimal = Nothing
                If txt_Fine2_AV.Visible = True Then
                    If txt_Fine2_AV.Text <> "" Then
                        FINE2_AV = TrimX(txt_Fine2_AV.Text)
                        FINE2_AV = RemoveQuotes(FINE2_AV)
                        If FINE2_LOOSE.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_AV.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE2_AV) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_AV.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE2_AV = Nothing
                    End If
                End If

                'FINE2_CARTOGRAPHIC
                Dim FINE2_CARTOGRAPHIC As Decimal = Nothing
                If txt_Fine2_Cartographic.Visible = True Then
                    If txt_Fine2_Cartographic.Text <> "" Then
                        FINE2_CARTOGRAPHIC = TrimX(txt_Fine2_Cartographic.Text)
                        FINE2_CARTOGRAPHIC = RemoveQuotes(FINE2_CARTOGRAPHIC)
                        If FINE2_CARTOGRAPHIC.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_Cartographic.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE2_CARTOGRAPHIC) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_Cartographic.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE2_CARTOGRAPHIC = Nothing
                    End If
                End If

                'FINE2_MANUSCRIPTS
                Dim FINE2_MANUSCRIPTS As Decimal = Nothing
                If txt_Fine2_Manuscripts.Visible = True Then
                    If txt_Fine2_Manuscripts.Text <> "" Then
                        FINE2_MANUSCRIPTS = TrimX(txt_Fine2_Manuscripts.Text)
                        FINE2_MANUSCRIPTS = RemoveQuotes(FINE2_MANUSCRIPTS)
                        If FINE2_MANUSCRIPTS.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_Manuscripts.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE2_MANUSCRIPTS) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_Manuscripts.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE2_MANUSCRIPTS = Nothing
                    End If
                End If

                'FINE2_BBGENERAL
                Dim FINE2_BBGENERAL As Decimal = Nothing
                If txt_Fine2_BBGeneral.Visible = True Then
                    If txt_Fine2_BBGeneral.Text <> "" Then
                        FINE2_BBGENERAL = TrimX(txt_Fine2_BBGeneral.Text)
                        FINE2_BBGENERAL = RemoveQuotes(FINE2_BBGENERAL)
                        If FINE2_BBGENERAL.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_BBGeneral.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE2_BBGENERAL) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_BBGeneral.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE2_BBGENERAL = Nothing
                    End If
                End If

                'FINE2_BBRESERVE
                Dim FINE2_BBRESERVE As Decimal = Nothing
                If txt_Fine2_BBReserve.Visible = True Then
                    If txt_Fine2_BBReserve.Text <> "" Then
                        FINE2_BBRESERVE = TrimX(txt_Fine2_BBReserve.Text)
                        FINE2_BBRESERVE = RemoveQuotes(FINE2_BBRESERVE)
                        If FINE2_BBGENERAL.ToString.Length > 10 Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_BBReserve.Focus()
                            Exit Sub
                        End If
                        If Not IsNumeric(FINE2_BBRESERVE) = True Then
                            Label5.Text = "Error: Input is not Valid !"
                            Label7.Text = ""
                            txt_Fine2_BBReserve.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE2_BBRESERVE = Nothing
                    End If
                End If



               


                Dim LIB_CODE As Object = Nothing
                If Session.Item("LoggedLibcode") <> "" Then
                    LIB_CODE = Session.Item("LoggedLibcode")
                Else
                    Label5.Text = "No Library Code Exists..Login Again  "
                    Label7.Text = ""
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
                    SQL = "SELECT * FROM SUB_CATEGORIES WHERE (SUBCAT_ID='" & Trim(SUBCAT_ID) & "')"
                    SqlConn.Open()
                    da = New SqlDataAdapter(SQL, SqlConn)
                    cb = New SqlCommandBuilder(da)
                    da.Fill(ds, "SUB_CATEGORIES")
                    If ds.Tables("SUB_CATEGORIES").Rows.Count <> 0 Then
                        If Not String.IsNullOrEmpty(SUBCAT_NAME) Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("SUBCAT_NAME") = SUBCAT_NAME.Trim
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("SUBCAT_NAME") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(SUBCAT_DESC) Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("SUBCAT_DESC") = SUBCAT_DESC.Trim
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("SUBCAT_DESC") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(FINE_SYSTEM) Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE_SYSTEM") = FINE_SYSTEM.Trim
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE_SYSTEM") = System.DBNull.Value
                        End If

                        If ENTITLEMENT_LOOSE <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("ENTITLEMENT_LOOSE") = ENTITLEMENT_LOOSE
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("ENTITLEMENT_LOOSE") = System.DBNull.Value
                        End If

                        If ENTITLEMENT_BOOKS <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("ENTITLEMENT_BOOKS") = ENTITLEMENT_BOOKS
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("ENTITLEMENT_BOOKS") = System.DBNull.Value
                        End If


                        If ENTITLEMENT_BOUNDJ <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("ENTITLEMENT_BOUNDJ") = ENTITLEMENT_BOUNDJ
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("ENTITLEMENT_BOUNDJ") = System.DBNull.Value
                        End If


                        If ENTITLEMENT_MANUALS <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("ENTITLEMENT_MANUALS") = ENTITLEMENT_MANUALS
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("ENTITLEMENT_MANUALS") = System.DBNull.Value
                        End If


                        If ENTITLEMENT_PATENTS <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("ENTITLEMENT_PATENTS") = ENTITLEMENT_PATENTS
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("ENTITLEMENT_PATENTS") = System.DBNull.Value
                        End If


                        If ENTITLEMENT_REPORTS <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("ENTITLEMENT_REPORTS") = ENTITLEMENT_REPORTS
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("ENTITLEMENT_REPORTS") = System.DBNull.Value
                        End If

                        If ENTITLEMENT_STANDARDS <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("ENTITLEMENT_STANDARDS") = ENTITLEMENT_STANDARDS
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("ENTITLEMENT_STANDARDS") = System.DBNull.Value
                        End If

                        If ENTITLEMENT_AV <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("ENTITLEMENT_AV") = ENTITLEMENT_AV
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("ENTITLEMENT_AV") = System.DBNull.Value
                        End If

                        If ENTITLEMENT_CARTOGRAPHIC <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("ENTITLEMENT_CARTOGRAPHIC") = ENTITLEMENT_CARTOGRAPHIC
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("ENTITLEMENT_CARTOGRAPHIC") = System.DBNull.Value
                        End If

                        If ENTITLEMENT_MANUSCRIPTS <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("ENTITLEMENT_MANUSCRIPTS") = ENTITLEMENT_MANUSCRIPTS
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("ENTITLEMENT_MANUSCRIPTS") = System.DBNull.Value
                        End If

                        If ENTITLEMENT_BBGENERAL <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("ENTITLEMENT_BBGENERAL") = ENTITLEMENT_BBGENERAL
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("ENTITLEMENT_BBGENERAL") = System.DBNull.Value
                        End If

                        If ENTITLEMENT_BBRESERVE <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("ENTITLEMENT_BBRESERVE") = ENTITLEMENT_BBRESERVE
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("ENTITLEMENT_BBRESERVE") = System.DBNull.Value
                        End If

                        If ENTITLEMENT_NONRETURNABLE <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("ENTITLEMENT_NONRETURNABLE") = ENTITLEMENT_NONRETURNABLE
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("ENTITLEMENT_NONRETURNABLE") = System.DBNull.Value
                        End If



                        If DUEDAYS_LOOSE <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("DUEDAYS_LOOSE") = DUEDAYS_LOOSE
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("DUEDAYS_LOOSE") = System.DBNull.Value
                        End If

                        If DUEDAYS_BOOKS <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("DUEDAYS_BOOKS") = DUEDAYS_BOOKS
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("DUEDAYS_BOOKS") = System.DBNull.Value
                        End If


                        If DUEDAYS_BOUNDJ <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("DUEDAYS_BOUNDJ") = DUEDAYS_BOUNDJ
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("DUEDAYS_BOUNDJ") = System.DBNull.Value
                        End If


                        If DUEDAYS_MANUALS <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("DUEDAYS_MANUALS") = DUEDAYS_MANUALS
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("DUEDAYS_MANUALS") = System.DBNull.Value
                        End If


                        If DUEDAYS_PATENTS <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("DUEDAYS_PATENTS") = DUEDAYS_PATENTS
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("DUEDAYS_PATENTS") = System.DBNull.Value
                        End If


                        If DUEDAYS_REPORTS <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("DUEDAYS_REPORTS") = DUEDAYS_REPORTS
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("DUEDAYS_REPORTS") = System.DBNull.Value
                        End If

                        If DUEDAYS_STANDARDS <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("DUEDAYS_STANDARDS") = DUEDAYS_STANDARDS
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("DUEDAYS_STANDARDS") = System.DBNull.Value
                        End If

                        If DUEDAYS_AV <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("DUEDAYS_AV") = DUEDAYS_AV
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("DUEDAYS_AV") = System.DBNull.Value
                        End If

                        If DUEDAYS_CARTOGRAPHIC <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("DUEDAYS_CARTOGRAPHIC") = DUEDAYS_CARTOGRAPHIC
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("DUEDAYS_CARTOGRAPHIC") = System.DBNull.Value
                        End If

                        If DUEDAYS_MANUSCRIPTS <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("DUEDAYS_MANUSCRIPTS") = DUEDAYS_MANUSCRIPTS
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("DUEDAYS_MANUSCRIPTS") = System.DBNull.Value
                        End If

                        If DUEDAYS_BBGENERAL <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("DUEDAYS_BBGENERAL") = DUEDAYS_BBGENERAL
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("DUEDAYS_BBGENERAL") = System.DBNull.Value
                        End If

                        If DUEDAYS_BBRESERVE <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("DUEDAYS_BBRESERVE") = DUEDAYS_BBRESERVE
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("DUEDAYS_BBRESERVE") = System.DBNull.Value
                        End If

                        If DUEDAYS_NONRETURNABLE <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("DUEDAYS_NONRETURNABLE") = DUEDAYS_NONRETURNABLE
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("DUEDAYS_NONRETURNABLE") = System.DBNull.Value
                        End If

                        If FINE1_GAP <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE1_GAP") = FINE1_GAP
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE1_GAP") = System.DBNull.Value
                        End If

                        If FINE1_LOOSE <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE1_LOOSE") = FINE1_LOOSE
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE1_LOOSE") = System.DBNull.Value
                        End If

                        If FINE1_BOOKS <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE1_BOOKS") = FINE1_BOOKS
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE1_BOOKS") = System.DBNull.Value
                        End If


                        If FINE1_BOUNDJ <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE1_BOUNDJ") = FINE1_BOUNDJ
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE1_BOUNDJ") = System.DBNull.Value
                        End If


                        If FINE1_MANUALS <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE1_MANUALS") = FINE1_MANUALS
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE1_MANUALS") = System.DBNull.Value
                        End If


                        If FINE1_PATENTS <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE1_PATENTS") = FINE1_PATENTS
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE1_PATENTS") = System.DBNull.Value
                        End If


                        If FINE1_REPORTS <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE1_REPORTS") = FINE1_REPORTS
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE1_REPORTS") = System.DBNull.Value
                        End If

                        If FINE1_STANDARDS <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE1_STANDARDS") = FINE1_STANDARDS
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE1_STANDARDS") = System.DBNull.Value
                        End If

                        If FINE1_AV <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE1_AV") = FINE1_AV
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE1_AV") = System.DBNull.Value
                        End If

                        If FINE1_CARTOGRAPHIC <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE1_CARTOGRAPHIC") = FINE1_CARTOGRAPHIC
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE1_CARTOGRAPHIC") = System.DBNull.Value
                        End If

                        If FINE1_MANUSCRIPTS <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE1_MANUSCRIPTS") = FINE1_MANUSCRIPTS
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE1_MANUSCRIPTS") = System.DBNull.Value
                        End If

                        If FINE1_BBGENERAL <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE1_BBGENERAL") = FINE1_BBGENERAL
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE1_BBGENERAL") = System.DBNull.Value
                        End If

                        If FINE1_BBRESERVE <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE1_BBRESERVE") = FINE1_BBRESERVE
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE1_BBRESERVE") = System.DBNull.Value
                        End If



                        If FINE2_LOOSE <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE2_LOOSE") = FINE2_LOOSE
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE2_LOOSE") = System.DBNull.Value
                        End If

                        If FINE2_BOOKS <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE2_BOOKS") = FINE2_BOOKS
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE2_BOOKS") = System.DBNull.Value
                        End If


                        If FINE2_BOUNDJ <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE2_BOUNDJ") = FINE2_BOUNDJ
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE2_BOUNDJ") = System.DBNull.Value
                        End If


                        If FINE2_MANUALS <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE2_MANUALS") = FINE2_MANUALS
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE2_MANUALS") = System.DBNull.Value
                        End If


                        If FINE2_PATENTS <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE2_PATENTS") = FINE2_PATENTS
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE2_PATENTS") = System.DBNull.Value
                        End If


                        If FINE2_REPORTS <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE2_REPORTS") = FINE2_REPORTS
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE2_REPORTS") = System.DBNull.Value
                        End If

                        If FINE2_STANDARDS <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE2_STANDARDS") = FINE2_STANDARDS
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE2_STANDARDS") = System.DBNull.Value
                        End If

                        If FINE2_AV <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE2_AV") = FINE2_AV
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE2_AV") = System.DBNull.Value
                        End If

                        If FINE2_CARTOGRAPHIC <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE2_CARTOGRAPHIC") = FINE2_CARTOGRAPHIC
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE2_CARTOGRAPHIC") = System.DBNull.Value
                        End If

                        If FINE2_MANUSCRIPTS <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE2_MANUSCRIPTS") = FINE2_MANUSCRIPTS
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE2_MANUSCRIPTS") = System.DBNull.Value
                        End If

                        If FINE2_BBGENERAL <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE2_BBGENERAL") = FINE2_BBGENERAL
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE2_BBGENERAL") = System.DBNull.Value
                        End If

                        If FINE2_BBRESERVE <> 0 Then
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE2_BBRESERVE") = FINE2_BBRESERVE
                        Else
                            ds.Tables("SUB_CATEGORIES").Rows(0)("FINE2_BBRESERVE") = System.DBNull.Value
                        End If


                        

                        ds.Tables("SUB_CATEGORIES").Rows(0)("UPDATED_BY") = USER_CODE
                        ds.Tables("SUB_CATEGORIES").Rows(0)("DATE_MODIFIED") = DATE_MODIFIED
                        ds.Tables("SUB_CATEGORIES").Rows(0)("IP") = IP

                        thisTransaction = SqlConn.BeginTransaction()
                        da.SelectCommand.Transaction = thisTransaction
                        da.Update(ds, "SUB_CATEGORIES")
                        thisTransaction.Commit()


                        Me.SubCat_Save_Bttn.Visible = True
                        Me.SubCat_Save_Bttn.Enabled = True
                        SubCat_Update_Bttn.Visible = False
                        ClearSubCatFields()
                        PopulateSubCategories()
                        txt_Cir_SubCatName.Focus()

                        Label7.Text = "Record Updated Successfully"
                        Label5.Text = ""


                       
                    Else
                        Label5.Text = "Error in Record Updation!"
                        Label7.Text = ""
                    End If
                End If
            Else
                'record not selected
                Label5.Text = "Record not Selected for Edit!"
                Label7.Text = ""
            End If
            SqlConn.Close()
        Catch q As SqlException
            thisTransaction.Rollback()
        Catch s As Exception
            Label5.Text = "Error: " & (s.Message())
            Label7.Text = ""
        Finally
            SqlConn.Close()
            Label8.Text = ""
        End Try
    End Sub
    'delete sub-categories
    Protected Sub SubCat_DeleteAll_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SubCat_DeleteAll_Bttn.Click
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing
        Try
            For Each row As GridViewRow In Grid2.Rows
                Dim cb As CheckBox = DirectCast(row.FindControl("cbd"), CheckBox)
                If cb IsNot Nothing AndAlso cb.Checked = True Then
                    Dim ID As Integer = Convert.ToInt32(Grid2.DataKeys(row.RowIndex).Value)

                    Dim LIB_CODE As Object = Nothing
                    LIB_CODE = LibCode

                    'check duplicate category
                    Dim str As Object = Nothing
                    Dim flag As Object = Nothing
                    str = "SELECT SUBCAT_ID FROM MEMBERSHIPS WHERE (SUBCAT_ID ='" & Trim(ID) & "')"
                    Dim cmd1 As New SqlCommand(str, SqlConn)
                    SqlConn.Open()
                    flag = cmd1.ExecuteScalar
                    SqlConn.Close()
                    If flag <> Nothing Then
                        Label6.Text = "This Member Sub Category can not be deleted - it is saved with Member Records !"
                        Label15.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('This Member Sub Category can not be deleted - it is saved with Member Records !');", True)
                    Else
                        If SqlConn.State = 0 Then
                            SqlConn.Open()
                        End If
                        thisTransaction = SqlConn.BeginTransaction()
                        Dim objCommand As New SqlCommand
                        objCommand.Connection = SqlConn
                        objCommand.Transaction = thisTransaction
                        objCommand.CommandType = CommandType.Text
                        objCommand.CommandText = "DELETE FROM SUB_CATEGORIES WHERE (SUBCAT_ID =@sUBCAT_ID and LIB_CODE =@LIB_CODE) "

                        objCommand.Parameters.Add("@SUBCAT_ID", SqlDbType.Int)
                        objCommand.Parameters("@SUBCAT_ID").Value = ID

                        If LIB_CODE = "" Then LIB_CODE = System.DBNull.Value
                        objCommand.Parameters.Add("@LIB_CODE", SqlDbType.NVarChar)
                        objCommand.Parameters("@LIB_CODE").Value = LIB_CODE

                        objCommand.ExecuteNonQuery()

                    End If

                    thisTransaction.Commit()
                    SqlConn.Close()
                End If
            Next
            Dim s As Object = Nothing
            Dim e1 As EventArgs = Nothing
            PopulateSubCategories()
        Catch s As Exception
            thisTransaction.Rollback()
            Label15.Text = ""
            Label6.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub

    'MEMBER S
    '*******************************************************************
    'populate Categories in combo
    Public Sub PopulateAllCategories()
        Dim dtSearch As DataTable = Nothing
        Try
            Dim SQL As String = Nothing
            SQL = "SELECT CAT_ID, CAT_NAME FROM CATEGORIES WHERE (LIB_CODE = '" & Trim(LibCode) & "') ORDER BY CAT_NAME"
            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            da.Fill(ds)

            dtSearch = ds.Tables(0).Copy
            Dim RecordCount As Long = 0
            If dtSearch.Rows.Count = 0 Then
                DDL_Categories.DataSource = Nothing
                DDL_Categories.DataBind()
                DDL_AllCategories.DataSource = Nothing
                DDL_AllCategories.DataBind()
            Else
                RecordCount = dtSearch.Rows.Count
                Me.DDL_Categories.DataTextField = "CAT_NAME"
                Me.DDL_Categories.DataValueField = "CAT_ID"
                Me.DDL_Categories.DataSource = dtSearch
                Me.DDL_Categories.DataBind()
                DDL_Categories.Items.Insert(0, "")

                Me.DDL_AllCategories.DataTextField = "CAT_NAME"
                Me.DDL_AllCategories.DataValueField = "CAT_NAME"
                Me.DDL_AllCategories.DataSource = dtSearch
                Me.DDL_AllCategories.DataBind()
                DDL_AllCategories.Items.Insert(0, "")
            End If
        Catch s As Exception
            Label12.Text = "Error: " & (s.Message())
            Label11.Text = ""
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'populate Categories in combo
    Public Sub PopulateAllSubCategories()
        Dim dtSearch As DataTable = Nothing
        Try
            Dim SQL As String = Nothing
            SQL = "SELECT SUBCAT_ID, SUBCAT_NAME FROM SUB_CATEGORIES WHERE (LIB_CODE = '" & Trim(LibCode) & "') ORDER BY SUBCAT_NAME"
            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            da.Fill(ds)

            dtSearch = ds.Tables(0).Copy
            Dim RecordCount As Long = 0
            If dtSearch.Rows.Count = 0 Then
                DDL_Categories.DataSource = Nothing
                DDL_Categories.DataBind()

                DDL_AllCategories.DataSource = Nothing
                DDL_AllCategories.DataBind()

                DDL_NewSubCategories.DataSource = Nothing
                DDL_NewSubCategories.DataBind()
            Else
                RecordCount = dtSearch.Rows.Count
                Me.DDL_SubCategories.DataTextField = "SUBCAT_NAME"
                Me.DDL_SubCategories.DataValueField = "SUBCAT_ID"
                Me.DDL_SubCategories.DataSource = dtSearch
                Me.DDL_SubCategories.DataBind()
                DDL_SubCategories.Items.Insert(0, "")

                Me.DDL_AllSubCategories.DataTextField = "SUBCAT_NAME"
                Me.DDL_AllSubCategories.DataValueField = "SUBCAT_NAME"
                Me.DDL_AllSubCategories.DataSource = dtSearch
                Me.DDL_AllSubCategories.DataBind()
                DDL_AllSubCategories.Items.Insert(0, "")

                Me.DDL_NewSubCategories.DataTextField = "SUBCAT_NAME"
                Me.DDL_NewSubCategories.DataValueField = "SUBCAT_ID"
                Me.DDL_NewSubCategories.DataSource = dtSearch
                Me.DDL_NewSubCategories.DataBind()
                DDL_NewSubCategories.Items.Insert(0, "")
            End If
        Catch s As Exception
            Label12.Text = "Error: " & (s.Message())
            Label11.Text = ""
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'populate Categories in combo
    Public Sub PopulateAllMembersSearch()
        Dim dtSearch As DataTable = Nothing
        Try
            Dim SQL As String = Nothing
            SQL = "SELECT DISTINCT MEM_NAME FROM MEMBERSHIPS WHERE (LIB_CODE = '" & Trim(LibCode) & "') ORDER BY MEM_NAME"
            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            da.Fill(ds)

            dtSearch = ds.Tables(0).Copy
            Dim RecordCount As Long = 0
            If dtSearch.Rows.Count = 0 Then
                DDL_MemName.DataSource = Nothing
                DDL_MemName.DataBind()
            Else
                RecordCount = dtSearch.Rows.Count
                Me.DDL_MemName.DataTextField = "MEM_NAME"
                Me.DDL_MemName.DataValueField = "MEM_NAME"
                Me.DDL_MemName.DataSource = dtSearch
                Me.DDL_MemName.DataBind()
                DDL_MemName.Items.Insert(0, "")
            End If
        Catch s As Exception
            Label12.Text = "Error: " & (s.Message())
            Label11.Text = ""
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'populate Categories in combo
    Public Sub PopulateAllMemberNoSearch()
        Dim dtSearch As DataTable = Nothing
        Try
            Dim SQL As String = Nothing
            SQL = "SELECT MEM_NO FROM MEMBERSHIPS WHERE (LIB_CODE = '" & Trim(LibCode) & "')  ORDER BY CASE WHEN LEFT(MEM_NO,1) LIKE  '[A-Za-z]'  THEN dbo.Sort_AlphaNumeric(MEM_NO, 'A-Za-z') END ASC, CAST(dbo.Sort_AlphaNumeric(MEM_NO, '0-9') AS float) ASC"
            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            da.Fill(ds)

            dtSearch = ds.Tables(0).Copy
            Dim RecordCount As Long = 0
            If dtSearch.Rows.Count = 0 Then
                DDL_MemNo.DataSource = Nothing
                DDL_MemNo.DataBind()
            Else
                RecordCount = dtSearch.Rows.Count
                Me.DDL_MemNo.DataTextField = "MEM_NO"
                Me.DDL_MemNo.DataValueField = "MEM_NO"
                Me.DDL_MemNo.DataSource = dtSearch
                Me.DDL_MemNo.DataBind()
                DDL_MemNo.Items.Insert(0, "")
            End If
        Catch s As Exception
            Label12.Text = "Error: " & (s.Message())
            Label11.Text = ""
        Finally
            SqlConn.Close()
        End Try
    End Sub
    Public Sub PopulateSubjects()
        Me.DDL_Subjects.DataSource = GetSubjectList()
        Me.DDL_Subjects.DataTextField = "SUB_NAME"
        Me.DDL_Subjects.DataValueField = "SUB_ID"
        Me.DDL_Subjects.DataBind()
        DDL_Subjects.Items.Insert(0, "")
    End Sub
    Public Sub PopulateMembersGrid()
        Dim dtSearch As DataTable = Nothing
        Try

            Dim myField As String = Nothing
            If DDL_MemNo.Text <> "" Then
                myField = "MEMBERSHIPS.MEM_NO"
            End If

            If DDL_MemName.Text <> "" Then
                myField = "MEMBERSHIPS.MEM_NAME"
            End If

            If DDL_AllCategories.Text <> "" Then
                myField = "CATEGORIES.CAT_NAME"
            End If
            If DDL_AllSubCategories.Text <> "" Then
                myField = "SUB_CATEGORIES.SUBCAT_NAME"
            End If

            If DDL_Status2.Text <> "" Then
                myField = "MEMBERSHIPS.MEM_STATUS"
            End If

            Dim SearchString = Nothing
            If DDL_MemNo.Text <> "" Then
                SearchString = DDL_MemNo.SelectedValue
            End If

            If DDL_MemName.Text <> "" Then
                SearchString = DDL_MemName.SelectedValue
            End If

            If DDL_AllCategories.Text <> "" Then
                SearchString = DDL_AllCategories.SelectedValue
            End If
            If DDL_AllSubCategories.Text <> "" Then
                SearchString = DDL_AllSubCategories.SelectedValue
            End If

            If DDL_Status2.Text <> "" Then
                SearchString = DDL_Status2.SelectedValue
            End If


            Dim SQL As String = Nothing
            If SearchString <> "" Then
                SQL = "SELECT MEMBERSHIPS.MEM_ID, MEMBERSHIPS.MEM_NO, MEMBERSHIPS.MEM_NAME, CATEGORIES.CAT_NAME, SUB_CATEGORIES.SUBCAT_NAME, MEMBERSHIPS.LIB_CODE FROM MEMBERSHIPS INNER JOIN CATEGORIES ON MEMBERSHIPS.CAT_ID = CATEGORIES.CAT_ID INNER JOIN SUB_CATEGORIES ON MEMBERSHIPS.SUBCAT_ID = SUB_CATEGORIES.SUBCAT_ID WHERE (MEMBERSHIPS.LIB_CODE = '" & Trim(LibCode) & "') AND (" & myField & " = '" & Trim(SearchString) & "') ORDER BY MEMBERSHIPS.MEM_NAME"
            Else
                SQL = "SELECT MEMBERSHIPS.MEM_ID, MEMBERSHIPS.MEM_NO, MEMBERSHIPS.MEM_NAME, CATEGORIES.CAT_NAME, SUB_CATEGORIES.SUBCAT_NAME, MEMBERSHIPS.LIB_CODE FROM MEMBERSHIPS INNER JOIN CATEGORIES ON MEMBERSHIPS.CAT_ID = CATEGORIES.CAT_ID INNER JOIN SUB_CATEGORIES ON MEMBERSHIPS.SUBCAT_ID = SUB_CATEGORIES.SUBCAT_ID WHERE (MEMBERSHIPS.LIB_CODE = '" & Trim(LibCode) & "') ORDER BY MEMBERSHIPS.MEM_NAME"
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
                Grid3.DataSource = Nothing
                Grid3.DataBind()
                Mem_Delete_All.Visible = False
                Mem_DeletedPhoto_All.Visible = False
                Mem_ChangeSubCatergory_Bttn.Visible = False
                Label16.Text = "Total Record(s): 0 "
            Else
                RecordCount = dtSearch.Rows.Count
                Me.Grid3.DataSource = dtSearch
                Me.Grid3.DataBind()
                Mem_Delete_All.Visible = True
                Mem_Delete_All.Enabled = True
                Mem_DeletedPhoto_All.Visible = True
                Mem_DeletedPhoto_All.Enabled = True
                Mem_ChangeSubCatergory_Bttn.Visible = True
                Mem_ChangeSubCatergory_Bttn.Enabled = True
                Label16.Text = "Total Record(s): " & RecordCount
            End If
            ViewState("dt") = dtSearch
            UpdatePanel5.Update()

            Label12.Text = ""
            Label11.Text = "Enter Data and Press SAVE Button to Save the Record!"
        Catch s As Exception
            Label12.Text = "Error: " & (s.Message())
            Label11.Text = ""
        Finally
            SqlConn.Close()
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
    'save member record
    Protected Sub Mem_Save_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Mem_Save_Bttn.Click
        If IsPostBack = True Then
            'check unwanted characters
            Dim iloop As Integer
            Dim strcurrentchar As Object
            Dim c As Integer
            Dim counter1, counter2, counter3, counter4, Counter5, counter6, counter7, counter8, counter9, counter10, counter11, counter12 As Integer

            Dim thisTransaction As SqlClient.SqlTransaction = Nothing
            Try
                'validation 
                Dim MEM_NO As Object = Nothing
                If Me.txt_Mem_MemNo.Text <> "" Then
                    MEM_NO = TrimX(txt_Mem_MemNo.Text)
                    MEM_NO = RemoveQuotes(MEM_NO)
                    If MEM_NO.Length > 50 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_MemNo.Focus()
                        Exit Sub
                    End If
                    MEM_NO = " " & MEM_NO & " "
                    If InStr(1, MEM_NO, "CREATE", 1) > 0 Or InStr(1, MEM_NO, "DELETE", 1) > 0 Or InStr(1, MEM_NO, "DROP", 1) > 0 Or InStr(1, MEM_NO, "INSERT", 1) > 1 Or InStr(1, MEM_NO, "TRACK", 1) > 1 Or InStr(1, MEM_NO, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_MemNo.Focus()
                        Exit Sub
                    End If
                    MEM_NO = TrimAll(MEM_NO)

                    'check duplicate category
                    Dim str As Object = Nothing
                    Dim flag As Object = Nothing
                    str = "SELECT MEM_ID FROM MEMBERSHIPS WHERE (MEM_NO ='" & Trim(MEM_NO) & "') AND (LIB_CODE ='" & Trim(LibCode) & "')"
                    Dim cmd1 As New SqlCommand(str, SqlConn)
                    SqlConn.Open()
                    flag = cmd1.ExecuteScalar
                    SqlConn.Close()
                    If flag <> Nothing Then
                        Label12.Text = "This Member No already exists !"
                        Label11.Text = ""
                        txt_Mem_MemNo.Focus()
                        Exit Sub
                    End If
                Else
                    Label12.Text = "Plz Enter Member No!"
                    Label11.Text = ""
                    txt_Mem_MemNo.Focus()
                    Exit Sub
                End If

                'validation 
                Dim MEM_NAME As Object = Nothing
                If Me.txt_Mem_MemName.Text <> "" Then
                    MEM_NAME = TrimAll(txt_Mem_MemName.Text)
                    MEM_NAME = RemoveQuotes(MEM_NAME)
                    If MEM_NO.Length > 150 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_MemName.Focus()
                        Exit Sub
                    End If
                    MEM_NAME = " " & MEM_NAME & " "
                    If InStr(1, MEM_NAME, "CREATE", 1) > 0 Or InStr(1, MEM_NAME, "DELETE", 1) > 0 Or InStr(1, MEM_NAME, "DROP", 1) > 0 Or InStr(1, MEM_NAME, "INSERT", 1) > 1 Or InStr(1, MEM_NAME, "TRACK", 1) > 1 Or InStr(1, MEM_NAME, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_MemName.Focus()
                        Exit Sub
                    End If
                    MEM_NAME = TrimAll(MEM_NAME)
                Else
                    Label12.Text = "Plz Enter Member No!"
                    Label11.Text = ""
                    txt_Mem_MemName.Focus()
                    Exit Sub
                End If

                'validation 
                Dim MEM_RES_ADD As Object = Nothing
                If Me.txt_Mem_ResAdd.Text <> "" Then
                    MEM_RES_ADD = TrimAll(txt_Mem_ResAdd.Text)
                    MEM_RES_ADD = RemoveQuotes(MEM_RES_ADD)
                    If MEM_RES_ADD.Length > 250 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_ResAdd.Focus()
                        Exit Sub
                    End If
                    MEM_RES_ADD = " " & MEM_RES_ADD & " "
                    If InStr(1, MEM_RES_ADD, "CREATE", 1) > 0 Or InStr(1, MEM_RES_ADD, "DELETE", 1) > 0 Or InStr(1, MEM_RES_ADD, "DROP", 1) > 0 Or InStr(1, MEM_RES_ADD, "INSERT", 1) > 1 Or InStr(1, MEM_RES_ADD, "TRACK", 1) > 1 Or InStr(1, MEM_RES_ADD, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_ResAdd.Focus()
                        Exit Sub
                    End If
                    MEM_RES_ADD = TrimAll(MEM_RES_ADD)
                Else
                    MEM_RES_ADD = Nothing
                End If

                'validation 
                Dim MEM_OFF_ADD As Object = Nothing
                If Me.txt_Mem_ResAdd.Text <> "" Then
                    MEM_OFF_ADD = TrimAll(txt_Mem_ResAdd.Text)
                    MEM_OFF_ADD = RemoveQuotes(MEM_OFF_ADD)
                    If MEM_OFF_ADD.Length > 250 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_ResAdd.Focus()
                        Exit Sub
                    End If
                    MEM_RES_ADD = " " & MEM_RES_ADD & " "
                    If InStr(1, MEM_OFF_ADD, "CREATE", 1) > 0 Or InStr(1, MEM_OFF_ADD, "DELETE", 1) > 0 Or InStr(1, MEM_OFF_ADD, "DROP", 1) > 0 Or InStr(1, MEM_OFF_ADD, "INSERT", 1) > 1 Or InStr(1, MEM_OFF_ADD, "TRACK", 1) > 1 Or InStr(1, MEM_OFF_ADD, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_ResAdd.Focus()
                        Exit Sub
                    End If
                    MEM_OFF_ADD = TrimAll(MEM_OFF_ADD)
                Else
                    MEM_OFF_ADD = Nothing
                End If

                'validation 
                Dim MEM_GENDER As Object = Nothing
                If Me.DDL_Gender.Text <> "" Then
                    MEM_GENDER = Trim(DDL_Gender.SelectedValue)
                    MEM_GENDER = RemoveQuotes(MEM_GENDER)
                    If MEM_GENDER.Length > 2 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        DDL_Gender.Focus()
                        Exit Sub
                    End If
                    MEM_GENDER = " " & MEM_GENDER & " "
                    If InStr(1, MEM_GENDER, "CREATE", 1) > 0 Or InStr(1, MEM_GENDER, "DELETE", 1) > 0 Or InStr(1, MEM_GENDER, "DROP", 1) > 0 Or InStr(1, MEM_GENDER, "INSERT", 1) > 1 Or InStr(1, MEM_GENDER, "TRACK", 1) > 1 Or InStr(1, MEM_GENDER, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        DDL_Gender.Focus()
                        Exit Sub
                    End If
                    MEM_GENDER = TrimX(MEM_GENDER)
                Else
                    MEM_GENDER = "M"
                End If

                'validation 
                Dim CAT_ID As Integer = Nothing
                If Me.DDL_Categories.Text <> "" Then
                    CAT_ID = Trim(DDL_Categories.SelectedValue)
                    CAT_ID = RemoveQuotes(CAT_ID)
                    If CAT_ID.ToString.Length > 10 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        DDL_Categories.Focus()
                        Exit Sub
                    End If

                    If IsNumeric(CAT_ID) = False Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        DDL_Categories.Focus()
                        Exit Sub
                    End If

                    CAT_ID = " " & CAT_ID & " "
                    If InStr(1, CAT_ID, "CREATE", 1) > 0 Or InStr(1, CAT_ID, "DELETE", 1) > 0 Or InStr(1, CAT_ID, "DROP", 1) > 0 Or InStr(1, CAT_ID, "INSERT", 1) > 1 Or InStr(1, CAT_ID, "TRACK", 1) > 1 Or InStr(1, CAT_ID, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        DDL_Categories.Focus()
                        Exit Sub
                    End If
                    CAT_ID = TrimX(CAT_ID)
                Else
                    Label12.Text = "Plz Select Member Category!"
                    Label11.Text = ""
                    DDL_Categories.Focus()
                    Exit Sub
                End If

                'validation 
                Dim SUBCAT_ID As Integer = Nothing
                If Me.DDL_SubCategories.Text <> "" Then
                    SUBCAT_ID = Trim(DDL_SubCategories.SelectedValue)
                    SUBCAT_ID = RemoveQuotes(SUBCAT_ID)
                    If SUBCAT_ID.ToString.Length > 10 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        DDL_SubCategories.Focus()
                        Exit Sub
                    End If

                    If IsNumeric(SUBCAT_ID) = False Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        DDL_SubCategories.Focus()
                        Exit Sub
                    End If

                    SUBCAT_ID = " " & SUBCAT_ID & " "
                    If InStr(1, SUBCAT_ID, "CREATE", 1) > 0 Or InStr(1, SUBCAT_ID, "DELETE", 1) > 0 Or InStr(1, SUBCAT_ID, "DROP", 1) > 0 Or InStr(1, SUBCAT_ID, "INSERT", 1) > 1 Or InStr(1, SUBCAT_ID, "TRACK", 1) > 1 Or InStr(1, SUBCAT_ID, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        DDL_SubCategories.Focus()
                        Exit Sub
                    End If
                    SUBCAT_ID = TrimX(SUBCAT_ID)
                Else
                    Label12.Text = "Plz Select Member Category!"
                    Label11.Text = ""
                    DDL_SubCategories.Focus()
                    Exit Sub
                End If

                'validation 
                Dim MEM_EMAIL As Object = Nothing
                If Me.txt_Mem_Mail.Text <> "" Then
                    MEM_EMAIL = Trim(txt_Mem_Mail.Text)
                    MEM_EMAIL = RemoveQuotes(MEM_EMAIL)
                    If MEM_EMAIL.Length > 100 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_Mail.Focus()
                        Exit Sub
                    End If
                    MEM_EMAIL = " " & MEM_EMAIL & " "
                    If InStr(1, MEM_EMAIL, "CREATE", 1) > 0 Or InStr(1, MEM_EMAIL, "DELETE", 1) > 0 Or InStr(1, MEM_EMAIL, "DROP", 1) > 0 Or InStr(1, MEM_EMAIL, "INSERT", 1) > 1 Or InStr(1, MEM_EMAIL, "TRACK", 1) > 1 Or InStr(1, MEM_EMAIL, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_Mail.Focus()
                        Exit Sub
                    End If
                    MEM_EMAIL = TrimX(MEM_EMAIL)
                Else
                    MEM_EMAIL = Nothing
                End If

                'validation 
                Dim MEM_TELEPHONE As Object = Nothing
                If Me.txt_Mem_Phone.Text <> "" Then
                    MEM_TELEPHONE = Trim(txt_Mem_Phone.Text)
                    MEM_TELEPHONE = RemoveQuotes(MEM_TELEPHONE)
                    If MEM_TELEPHONE.Length > 50 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_Phone.Focus()
                        Exit Sub
                    End If
                    MEM_TELEPHONE = " " & MEM_TELEPHONE & " "
                    If InStr(1, MEM_TELEPHONE, "CREATE", 1) > 0 Or InStr(1, MEM_TELEPHONE, "DELETE", 1) > 0 Or InStr(1, MEM_TELEPHONE, "DROP", 1) > 0 Or InStr(1, MEM_TELEPHONE, "INSERT", 1) > 1 Or InStr(1, MEM_TELEPHONE, "TRACK", 1) > 1 Or InStr(1, MEM_TELEPHONE, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_Phone.Focus()
                        Exit Sub
                    End If
                    MEM_TELEPHONE = TrimX(MEM_TELEPHONE)
                Else
                    MEM_TELEPHONE = Nothing
                End If

                'validation 
                Dim MEM_MOBILE As Object = Nothing
                If Me.txt_Mem_Mobile.Text <> "" Then
                    MEM_MOBILE = Trim(txt_Mem_Mobile.Text)
                    MEM_MOBILE = RemoveQuotes(MEM_MOBILE)
                    If MEM_MOBILE.Length > 50 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_Mobile.Focus()
                        Exit Sub
                    End If
                    MEM_MOBILE = " " & MEM_MOBILE & " "
                    If InStr(1, MEM_MOBILE, "CREATE", 1) > 0 Or InStr(1, MEM_MOBILE, "DELETE", 1) > 0 Or InStr(1, MEM_MOBILE, "DROP", 1) > 0 Or InStr(1, MEM_MOBILE, "INSERT", 1) > 1 Or InStr(1, MEM_MOBILE, "TRACK", 1) > 1 Or InStr(1, MEM_MOBILE, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_Mobile.Focus()
                        Exit Sub
                    End If
                    MEM_MOBILE = TrimX(MEM_MOBILE)
                Else
                    MEM_MOBILE = Nothing
                End If

                'validation 
                Dim MEM_OVERRIDE As Object = Nothing
                If Me.DDL_OverRide.Text <> "" Then
                    MEM_OVERRIDE = Trim(DDL_OverRide.SelectedValue)
                    MEM_OVERRIDE = RemoveQuotes(MEM_OVERRIDE)
                    If MEM_OVERRIDE.Length > 50 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        DDL_OverRide.Focus()
                        Exit Sub
                    End If
                    MEM_OVERRIDE = " " & MEM_OVERRIDE & " "
                    If InStr(1, MEM_OVERRIDE, "CREATE", 1) > 0 Or InStr(1, MEM_OVERRIDE, "DELETE", 1) > 0 Or InStr(1, MEM_OVERRIDE, "DROP", 1) > 0 Or InStr(1, MEM_OVERRIDE, "INSERT", 1) > 1 Or InStr(1, MEM_OVERRIDE, "TRACK", 1) > 1 Or InStr(1, MEM_OVERRIDE, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        DDL_OverRide.Focus()
                        Exit Sub
                    End If
                    MEM_OVERRIDE = TrimX(MEM_OVERRIDE)
                Else
                    MEM_OVERRIDE = "N"
                End If

                'validation 
                Dim MEM_ADM_DATE As Object = Nothing
                If Me.txt_Mem_AdmissionDate.Text <> "" Then
                    MEM_ADM_DATE = Trim(txt_Mem_AdmissionDate.Text)
                    MEM_ADM_DATE = RemoveQuotes(MEM_ADM_DATE)
                    MEM_ADM_DATE = Convert.ToDateTime(MEM_ADM_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy")
                    If MEM_ADM_DATE.Length > 12 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_AdmissionDate.Focus()
                        Exit Sub
                    End If
                    MEM_ADM_DATE = " " & MEM_ADM_DATE & " "
                    If InStr(1, MEM_ADM_DATE, "CREATE", 1) > 0 Or InStr(1, MEM_ADM_DATE, "DELETE", 1) > 0 Or InStr(1, MEM_ADM_DATE, "DROP", 1) > 0 Or InStr(1, MEM_ADM_DATE, "INSERT", 1) > 1 Or InStr(1, MEM_ADM_DATE, "TRACK", 1) > 1 Or InStr(1, MEM_ADM_DATE, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_AdmissionDate.Focus()
                        Exit Sub
                    End If
                    MEM_ADM_DATE = TrimX(MEM_ADM_DATE)
                Else
                    MEM_ADM_DATE = Convert.ToDateTime(Today.Date, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy")
                End If

                'validation 
                Dim MEM_CLOSE_DATE As Object = Nothing
                If Me.txt_Mem_ClosingDate.Text <> "" Then
                    MEM_CLOSE_DATE = Trim(txt_Mem_ClosingDate.Text)
                    MEM_CLOSE_DATE = RemoveQuotes(MEM_CLOSE_DATE)
                    MEM_CLOSE_DATE = Convert.ToDateTime(MEM_CLOSE_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy")
                    If MEM_ADM_DATE.Length > 12 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_ClosingDate.Focus()
                        Exit Sub
                    End If
                    MEM_CLOSE_DATE = " " & MEM_CLOSE_DATE & " "
                    If InStr(1, MEM_CLOSE_DATE, "CREATE", 1) > 0 Or InStr(1, MEM_CLOSE_DATE, "DELETE", 1) > 0 Or InStr(1, MEM_CLOSE_DATE, "DROP", 1) > 0 Or InStr(1, MEM_CLOSE_DATE, "INSERT", 1) > 1 Or InStr(1, MEM_CLOSE_DATE, "TRACK", 1) > 1 Or InStr(1, MEM_CLOSE_DATE, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_ClosingDate.Focus()
                        Exit Sub
                    End If
                    MEM_CLOSE_DATE = TrimX(MEM_CLOSE_DATE)
                Else
                    MEM_CLOSE_DATE = Convert.ToDateTime(Today.Date, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy")
                End If

                'validation 
                Dim MEM_REMARKS As Object = Nothing
                If Me.txt_Mem_Remarks.Text <> "" Then
                    MEM_REMARKS = Trim(txt_Mem_Remarks.Text)
                    MEM_REMARKS = RemoveQuotes(MEM_REMARKS)
                    If MEM_REMARKS.Length > 50 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_Remarks.Focus()
                        Exit Sub
                    End If
                    MEM_REMARKS = " " & MEM_REMARKS & " "
                    If InStr(1, MEM_REMARKS, "CREATE", 1) > 0 Or InStr(1, MEM_REMARKS, "DELETE", 1) > 0 Or InStr(1, MEM_REMARKS, "DROP", 1) > 0 Or InStr(1, MEM_REMARKS, "INSERT", 1) > 1 Or InStr(1, MEM_REMARKS, "TRACK", 1) > 1 Or InStr(1, MEM_REMARKS, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_Remarks.Focus()
                        Exit Sub
                    End If
                    MEM_REMARKS = TrimAll(MEM_REMARKS)
                Else
                    MEM_REMARKS = Nothing
                End If

                'validation 
                Dim SUB_ID As Integer = Nothing
                If Me.DDL_Subjects.Text <> "" Then
                    SUB_ID = Trim(DDL_Subjects.SelectedValue)
                    SUB_ID = RemoveQuotes(SUB_ID)
                    If SUB_ID.ToString.Length > 5 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        DDL_Subjects.Focus()
                        Exit Sub
                    End If

                    If IsNumeric(SUB_ID) = False Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        DDL_Subjects.Focus()
                        Exit Sub
                    End If

                    SUB_ID = " " & SUB_ID & " "
                    If InStr(1, SUB_ID, "CREATE", 1) > 0 Or InStr(1, SUB_ID, "DELETE", 1) > 0 Or InStr(1, SUB_ID, "DROP", 1) > 0 Or InStr(1, SUB_ID, "INSERT", 1) > 1 Or InStr(1, SUB_ID, "TRACK", 1) > 1 Or InStr(1, SUB_ID, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        DDL_Subjects.Focus()
                        Exit Sub
                    End If
                    SUB_ID = TrimX(SUB_ID)
                Else
                    SUB_ID = Nothing
                End If

                'validation 
                Dim KEYWORDS As Object = Nothing
                If Me.txt_Mem_Keywords.Text <> "" Then
                    KEYWORDS = Trim(txt_Mem_Keywords.Text)
                    KEYWORDS = RemoveQuotes(KEYWORDS)
                    If KEYWORDS.Length > 250 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_Keywords.Focus()
                        Exit Sub
                    End If
                    KEYWORDS = " " & KEYWORDS & " "
                    If InStr(1, KEYWORDS, "CREATE", 1) > 0 Or InStr(1, KEYWORDS, "DELETE", 1) > 0 Or InStr(1, KEYWORDS, "DROP", 1) > 0 Or InStr(1, KEYWORDS, "INSERT", 1) > 1 Or InStr(1, KEYWORDS, "TRACK", 1) > 1 Or InStr(1, KEYWORDS, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_Keywords.Focus()
                        Exit Sub
                    End If
                    KEYWORDS = TrimAll(KEYWORDS)
                Else
                    KEYWORDS = Nothing
                End If

                'validation 
                Dim MEM_STATUS As Object = Nothing
                If Me.DDL_Status.Text <> "" Then
                    MEM_STATUS = Trim(DDL_Status.SelectedValue)
                    MEM_STATUS = RemoveQuotes(MEM_STATUS)
                    If MEM_STATUS.ToString.Length > 5 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        DDL_Status.Focus()
                        Exit Sub
                    End If
                    MEM_STATUS = " " & MEM_STATUS & " "
                    If InStr(1, MEM_STATUS, "CREATE", 1) > 0 Or InStr(1, MEM_STATUS, "DELETE", 1) > 0 Or InStr(1, MEM_STATUS, "DROP", 1) > 0 Or InStr(1, MEM_STATUS, "INSERT", 1) > 1 Or InStr(1, MEM_STATUS, "TRACK", 1) > 1 Or InStr(1, MEM_STATUS, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        DDL_Status.Focus()
                        Exit Sub
                    End If
                    MEM_STATUS = TrimX(MEM_STATUS)
                Else
                    MEM_STATUS = "CU"
                End If

                'validation 
                Dim NO_DUE_DATE As Object = Nothing
                If Me.txt_Mem_NoDueDate.Text <> "" Then
                    NO_DUE_DATE = Trim(txt_Mem_NoDueDate.Text)
                    NO_DUE_DATE = RemoveQuotes(NO_DUE_DATE)
                    NO_DUE_DATE = Convert.ToDateTime(NO_DUE_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy")
                    If NO_DUE_DATE.Length > 12 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_NoDueDate.Focus()
                        Exit Sub
                    End If
                    NO_DUE_DATE = " " & NO_DUE_DATE & " "
                    If InStr(1, NO_DUE_DATE, "CREATE", 1) > 0 Or InStr(1, NO_DUE_DATE, "DELETE", 1) > 0 Or InStr(1, NO_DUE_DATE, "DROP", 1) > 0 Or InStr(1, NO_DUE_DATE, "INSERT", 1) > 1 Or InStr(1, NO_DUE_DATE, "TRACK", 1) > 1 Or InStr(1, NO_DUE_DATE, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_NoDueDate.Focus()
                        Exit Sub
                    End If
                    NO_DUE_DATE = TrimX(NO_DUE_DATE)
                Else
                    NO_DUE_DATE = Nothing
                End If

                'validation 
                Dim MEM_DOB As Object = Nothing
                If Me.txt_Mem_DoB.Text <> "" Then
                    MEM_DOB = Trim(txt_Mem_DoB.Text)
                    MEM_DOB = RemoveQuotes(MEM_DOB)
                    MEM_DOB = Convert.ToDateTime(MEM_DOB, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy")
                    If MEM_DOB.Length > 12 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_DoB.Focus()
                        Exit Sub
                    End If
                    MEM_DOB = " " & MEM_DOB & " "
                    If InStr(1, MEM_DOB, "CREATE", 1) > 0 Or InStr(1, MEM_DOB, "DELETE", 1) > 0 Or InStr(1, MEM_DOB, "DROP", 1) > 0 Or InStr(1, MEM_DOB, "INSERT", 1) > 1 Or InStr(1, MEM_DOB, "TRACK", 1) > 1 Or InStr(1, MEM_DOB, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_DoB.Focus()
                        Exit Sub
                    End If
                    MEM_DOB = TrimX(MEM_DOB)
                Else
                    MEM_DOB = Nothing
                End If


                'validation 
                Dim SURITY_NAME As Object = Nothing
                If Me.txt_Mem_SurityName.Text <> "" Then
                    SURITY_NAME = TrimAll(txt_Mem_SurityName.Text)
                    SURITY_NAME = RemoveQuotes(SURITY_NAME)
                    If SURITY_NAME.Length >= 150 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_SurityName.Focus()
                        Exit Sub
                    End If
                    SURITY_NAME = " " & SURITY_NAME & " "
                    If InStr(1, SURITY_NAME, "CREATE", 1) > 0 Or InStr(1, SURITY_NAME, "DELETE", 1) > 0 Or InStr(1, SURITY_NAME, "DROP", 1) > 0 Or InStr(1, SURITY_NAME, "INSERT", 1) > 1 Or InStr(1, SURITY_NAME, "TRACK", 1) > 1 Or InStr(1, SURITY_NAME, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_SurityName.Focus()
                        Exit Sub
                    End If
                    SURITY_NAME = TrimAll(SURITY_NAME)
                Else
                    SURITY_NAME = Nothing
                End If

                'validation 
                Dim FATHER_NAME As Object = Nothing
                If Me.txt_Mem_FatherName.Text <> "" Then
                    FATHER_NAME = TrimAll(txt_Mem_FatherName.Text)
                    FATHER_NAME = RemoveQuotes(FATHER_NAME)
                    If FATHER_NAME.Length >= 150 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_FatherName.Focus()
                        Exit Sub
                    End If
                    FATHER_NAME = " " & FATHER_NAME & " "
                    If InStr(1, FATHER_NAME, "CREATE", 1) > 0 Or InStr(1, FATHER_NAME, "DELETE", 1) > 0 Or InStr(1, FATHER_NAME, "DROP", 1) > 0 Or InStr(1, FATHER_NAME, "INSERT", 1) > 1 Or InStr(1, FATHER_NAME, "TRACK", 1) > 1 Or InStr(1, FATHER_NAME, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_FatherName.Focus()
                        Exit Sub
                    End If
                    FATHER_NAME = TrimAll(FATHER_NAME)
                Else
                    FATHER_NAME = Nothing
                End If

                'validation 
                Dim PROFESSION As Object = Nothing
                If Me.txt_Mem_Profession.Text <> "" Then
                    PROFESSION = TrimAll(txt_Mem_Profession.Text)
                    PROFESSION = RemoveQuotes(PROFESSION)
                    If PROFESSION.Length >= 150 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_Profession.Focus()
                        Exit Sub
                    End If
                    PROFESSION = " " & PROFESSION & " "
                    If InStr(1, PROFESSION, "CREATE", 1) > 0 Or InStr(1, PROFESSION, "DELETE", 1) > 0 Or InStr(1, PROFESSION, "DROP", 1) > 0 Or InStr(1, PROFESSION, "INSERT", 1) > 1 Or InStr(1, PROFESSION, "TRACK", 1) > 1 Or InStr(1, PROFESSION, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_Profession.Focus()
                        Exit Sub
                    End If
                    PROFESSION = TrimAll(PROFESSION)
                Else
                    PROFESSION = Nothing
                End If

                'validation 
                Dim QUALIFICATION As Object = Nothing
                If Me.txt_Mem_Qualification.Text <> "" Then
                    QUALIFICATION = TrimAll(txt_Mem_Qualification.Text)
                    QUALIFICATION = RemoveQuotes(QUALIFICATION)
                    If QUALIFICATION.Length >= 150 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_Qualification.Focus()
                        Exit Sub
                    End If
                    QUALIFICATION = " " & QUALIFICATION & " "
                    If InStr(1, QUALIFICATION, "CREATE", 1) > 0 Or InStr(1, QUALIFICATION, "DELETE", 1) > 0 Or InStr(1, QUALIFICATION, "DROP", 1) > 0 Or InStr(1, QUALIFICATION, "INSERT", 1) > 1 Or InStr(1, QUALIFICATION, "TRACK", 1) > 1 Or InStr(1, QUALIFICATION, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_Qualification.Focus()
                        Exit Sub
                    End If
                    QUALIFICATION = TrimAll(QUALIFICATION)
                Else
                    QUALIFICATION = Nothing
                End If

                'validation 
                Dim SEND_REMINDER As Object = Nothing
                If Me.DDL_Reminder.Text <> "" Then
                    SEND_REMINDER = Trim(DDL_Reminder.SelectedValue)
                    SEND_REMINDER = RemoveQuotes(SEND_REMINDER)
                    If SEND_REMINDER.ToString.Length > 2 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        DDL_Reminder.Focus()
                        Exit Sub
                    End If
                    SEND_REMINDER = " " & SEND_REMINDER & " "
                    If InStr(1, SEND_REMINDER, "CREATE", 1) > 0 Or InStr(1, SEND_REMINDER, "DELETE", 1) > 0 Or InStr(1, SEND_REMINDER, "DROP", 1) > 0 Or InStr(1, SEND_REMINDER, "INSERT", 1) > 1 Or InStr(1, SEND_REMINDER, "TRACK", 1) > 1 Or InStr(1, SEND_REMINDER, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        DDL_Reminder.Focus()
                        Exit Sub
                    End If
                    SEND_REMINDER = TrimX(SEND_REMINDER)
                Else
                    SEND_REMINDER = "Y"
                End If


                'SERVER VALIDATION FOR PASSWORD
                '*******************************************************************************************************
                Dim Password As Object = Nothing
                If CheckBox2.Checked = True Then
                    Dim Hashed As Object = Nothing
                    Hashed = HashPass2.Value 'Request.Form("HashPass")
                    Password = TrimX(Hashed)
                    If Not String.IsNullOrEmpty(Password) Then
                        Password = RemoveQuotes(Password)
                        If Password.Length > 72 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Length of password is not Proper... ');", True)
                            Me.txt_UserPass.Focus()
                            Exit Sub
                        End If
                        If InStr(1, Password, "CREATE", 1) > 0 Or InStr(1, Password, "DELETE", 1) > 0 Or InStr(1, Password, "DROP", 1) > 0 Or InStr(1, Password, "INSERT", 1) > 1 Or InStr(1, Password, "TRACK", 1) > 1 Or InStr(1, Password, "TRACE", 1) > 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Do not use reserve words... ');", True)
                            Me.txt_UserPass.Focus()
                            Exit Sub
                        End If
                    Else
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Enter Password... ');", True)
                        Me.txt_UserPass.Focus()
                        Exit Sub
                    End If
                    'check unwanted characters
                    c = 0
                    counter10 = 0
                    For iloop = 1 To Len(Password)
                        strcurrentchar = Mid(Password, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter10 = 1
                            End If
                        End If
                    Next
                    If counter10 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Do Not Use Un-wanted Characters... ');", True)
                        Me.txt_UserPass.Focus()
                        Exit Sub
                    End If
                End If


                'upload content file
                Dim arrContent1 As Byte()
                Dim FileType As String = Nothing
                Dim FileExtension As String = Nothing
                Dim intLength1 As Integer = 0
                If FileUpload1.FileName = "" Then
                    '    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Select Proper file... ');", True)
                    '    Me.FileUpload12.Focus()
                    '    Exit Sub
                Else
                    Dim ContfileName As String = FileUpload1.PostedFile.FileName
                    FileExtension = ContfileName.Substring(ContfileName.LastIndexOf("."))
                    FileExtension = FileExtension.ToLower
                    FileType = FileUpload1.PostedFile.ContentType

                    intLength1 = Convert.ToInt32(FileUpload1.PostedFile.InputStream.Length)
                    ReDim arrContent1(intLength1)
                    FileUpload1.PostedFile.InputStream.Read(arrContent1, 0, intLength1)

                    If intLength1 > 15000 Then
                        Label6.Text = "Error: Photo Size is Bigger than 15 KB"
                        Label15.Text = ""
                        Exit Sub
                    End If
                    'Image1.ImageUrl = FileUpload1.PostedFile.FileName '"~/Images/1.png"
                End If


                Dim LIB_CODE As Object = Nothing
                If Session.Item("LoggedLibcode") <> "" Then
                    LIB_CODE = Session.Item("LoggedLibcode")
                Else
                    Label5.Text = "No Library Code Exists..Login Again  "
                    Label7.Text = ""
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
                objCommand.CommandText = "INSERT INTO MEMBERSHIPS (MEM_NO, MEM_NAME, MEM_RES_ADD, MEM_OFF_ADD, MEM_GENDER, CAT_ID, SUBCAT_ID, MEM_EMAIL, MEM_TELEPHONE, MEM_MOBILE, MEM_OVERRIDE, MEM_ADM_DATE, MEM_CLOSE_DATE, MEM_REMARKS, SUB_ID, KEYWORDS, MEM_STATUS, NO_DUE_DATE, MEM_DOB, SURITY_NAME, FATHER_NAME, PROFESSION, QUALIFICATION, SEND_REMINDER, PHOTO, DATE_ADDED, USER_CODE, LIB_CODE, IP, MEMB_PASSWORD) " & _
                                         " VALUES (@MEM_NO, @MEM_NAME, @MEM_RES_ADD, @MEM_OFF_ADD, @MEM_GENDER, @CAT_ID, @SUBCAT_ID, @MEM_EMAIL, @MEM_TELEPHONE, @MEM_MOBILE, @MEM_OVERRIDE, @MEM_ADM_DATE, @MEM_CLOSE_DATE, @MEM_REMARKS, @SUB_ID, @KEYWORDS, @MEM_STATUS, @NO_DUE_DATE, @MEM_DOB, @SURITY_NAME, @FATHER_NAME, @PROFESSION, @QUALIFICATION, @SEND_REMINDER, @PHOTO, @DATE_ADDED, @USER_CODE, @LIB_CODE, @IP, @MEMB_PASSWORD); "

                If MEM_NO = "" Then MEM_NO = System.DBNull.Value
                objCommand.Parameters.Add("@MEM_NO", SqlDbType.NVarChar)
                objCommand.Parameters("@MEM_NO").Value = MEM_NO

                If MEM_NAME = "" Then MEM_NAME = System.DBNull.Value
                objCommand.Parameters.Add("@MEM_NAME", SqlDbType.NVarChar)
                objCommand.Parameters("@MEM_NAME").Value = MEM_NAME

                objCommand.Parameters.Add("@MEM_RES_ADD", SqlDbType.NVarChar)
                If MEM_RES_ADD = "" Then
                    objCommand.Parameters("@MEM_RES_ADD").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@MEM_RES_ADD").Value = MEM_RES_ADD
                End If

                objCommand.Parameters.Add("@MEM_OFF_ADD", SqlDbType.NVarChar)
                If MEM_OFF_ADD = "" Then
                    objCommand.Parameters("@MEM_OFF_ADD").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@MEM_OFF_ADD").Value = MEM_OFF_ADD
                End If

                objCommand.Parameters.Add("@MEM_GENDER", SqlDbType.NVarChar)
                If MEM_GENDER = "" Then
                    objCommand.Parameters("@MEM_GENDER").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@MEM_GENDER").Value = MEM_GENDER
                End If

                objCommand.Parameters.Add("@CAT_ID", SqlDbType.Int)
                If CAT_ID = 0 Then
                    objCommand.Parameters("@CAT_ID").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@CAT_ID").Value = CAT_ID
                End If

                objCommand.Parameters.Add("@SUBCAT_ID", SqlDbType.Int)
                If SUBCAT_ID = 0 Then
                    objCommand.Parameters("@SUBCAT_ID").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@SUBCAT_ID").Value = SUBCAT_ID
                End If

                objCommand.Parameters.Add("@MEM_EMAIL", SqlDbType.NVarChar)
                If MEM_EMAIL = "" Then
                    objCommand.Parameters("@MEM_EMAIL").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@MEM_EMAIL").Value = MEM_EMAIL
                End If

                objCommand.Parameters.Add("@MEM_TELEPHONE", SqlDbType.NVarChar)
                If MEM_TELEPHONE = "" Then
                    objCommand.Parameters("@MEM_TELEPHONE").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@MEM_TELEPHONE").Value = MEM_TELEPHONE
                End If

                objCommand.Parameters.Add("@MEM_MOBILE", SqlDbType.NVarChar)
                If MEM_MOBILE = "" Then
                    objCommand.Parameters("@MEM_MOBILE").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@MEM_MOBILE").Value = MEM_MOBILE
                End If

                objCommand.Parameters.Add("@MEM_OVERRIDE", SqlDbType.NVarChar)
                If MEM_OVERRIDE = "" Then
                    objCommand.Parameters("@MEM_OVERRIDE").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@MEM_OVERRIDE").Value = MEM_OVERRIDE
                End If

                objCommand.Parameters.Add("@MEM_ADM_DATE", SqlDbType.DateTime)
                If MEM_ADM_DATE = "" Then
                    objCommand.Parameters("@MEM_ADM_DATE").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@MEM_ADM_DATE").Value = MEM_ADM_DATE
                End If

                objCommand.Parameters.Add("@MEM_CLOSE_DATE", SqlDbType.DateTime)
                If MEM_CLOSE_DATE = "" Then
                    objCommand.Parameters("@MEM_CLOSE_DATE").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@MEM_CLOSE_DATE").Value = MEM_CLOSE_DATE
                End If

                objCommand.Parameters.Add("@MEM_REMARKS", SqlDbType.NVarChar)
                If MEM_REMARKS = "" Then
                    objCommand.Parameters("@MEM_REMARKS").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@MEM_REMARKS").Value = MEM_REMARKS
                End If

                objCommand.Parameters.Add("@SUB_ID", SqlDbType.Int)
                If SUB_ID = 0 Then
                    objCommand.Parameters("@SUB_ID").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@SUB_ID").Value = SUB_ID
                End If

                objCommand.Parameters.Add("@KEYWORDS", SqlDbType.NVarChar)
                If KEYWORDS = "" Then
                    objCommand.Parameters("@KEYWORDS").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@KEYWORDS").Value = KEYWORDS
                End If

                objCommand.Parameters.Add("@MEM_STATUS", SqlDbType.NVarChar)
                If MEM_STATUS = "" Then
                    objCommand.Parameters("@MEM_STATUS").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@MEM_STATUS").Value = MEM_STATUS
                End If

                objCommand.Parameters.Add("@MEM_DOB", SqlDbType.DateTime)
                If MEM_DOB = "" Then
                    objCommand.Parameters("@MEM_DOB").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@MEM_DOB").Value = MEM_DOB
                End If

                objCommand.Parameters.Add("@NO_DUE_DATE", SqlDbType.DateTime)
                If NO_DUE_DATE = "" Then
                    objCommand.Parameters("@NO_DUE_DATE").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@NO_DUE_DATE").Value = NO_DUE_DATE
                End If

                objCommand.Parameters.Add("@SURITY_NAME", SqlDbType.NVarChar)
                If SURITY_NAME = "" Then
                    objCommand.Parameters("@SURITY_NAME").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@SURITY_NAME").Value = SURITY_NAME
                End If

                objCommand.Parameters.Add("@FATHER_NAME", SqlDbType.NVarChar)
                If FATHER_NAME = "" Then
                    objCommand.Parameters("@FATHER_NAME").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@FATHER_NAME").Value = FATHER_NAME
                End If

                objCommand.Parameters.Add("@PROFESSION", SqlDbType.NVarChar)
                If PROFESSION = "" Then
                    objCommand.Parameters("@PROFESSION").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@PROFESSION").Value = PROFESSION
                End If

                objCommand.Parameters.Add("@QUALIFICATION", SqlDbType.NVarChar)
                If QUALIFICATION = "" Then
                    objCommand.Parameters("@QUALIFICATION").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@QUALIFICATION").Value = QUALIFICATION
                End If

                objCommand.Parameters.Add("@SEND_REMINDER", SqlDbType.NVarChar)
                If SEND_REMINDER = "" Then
                    objCommand.Parameters("@SEND_REMINDER").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@SEND_REMINDER").Value = SEND_REMINDER
                End If

                objCommand.Parameters.Add("@MEMB_PASSWORD", SqlDbType.NVarChar)
                If CheckBox2.Checked = True Then
                    If Password <> "" Then
                        objCommand.Parameters("@MEMB_PASSWORD").Value = Password
                    Else
                        objCommand.Parameters("@MEMB_PASSWORD").Value = System.DBNull.Value
                    End If
                Else
                    objCommand.Parameters("@MEMB_PASSWORD").Value = System.DBNull.Value
                End If

                objCommand.Parameters.Add("@PHOTO", SqlDbType.Image)
                If FileUpload1.FileName = "" Then
                    objCommand.Parameters("@PHOTO").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@PHOTO").Value = arrContent1
                End If

                If DATE_ADDED = "" Then DATE_ADDED = System.DBNull.Value
                objCommand.Parameters.Add("@DATE_ADDED", SqlDbType.DateTime)
                objCommand.Parameters("@DATE_ADDED").Value = DATE_ADDED

                If USER_CODE = "" Then USER_CODE = System.DBNull.Value
                objCommand.Parameters.Add("@USER_CODE", SqlDbType.NVarChar)
                objCommand.Parameters("@USER_CODE").Value = USER_CODE

                If LIB_CODE = "" Then LIB_CODE = System.DBNull.Value
                objCommand.Parameters.Add("@LIB_CODE", SqlDbType.NVarChar)
                objCommand.Parameters("@LIB_CODE").Value = LIB_CODE

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

                ClearMemFields()

                Label11.Text = "Record Added Successfully! "
                Label12.Text = ""
                Me.Mem_Save_Bttn.Visible = True
                Me.Mem_Save_Bttn.Enabled = True
                Mem_Update_Bttn.Visible = False
                PopulateMembersGrid()
                txt_Mem_MemNo.Focus()
            Catch q As SqlException
                thisTransaction.Rollback()
                Label12.Text = "Members Error -SAVE: " & (q.Message() & ", " & q.Number & ", " & q.GetType.ToString)
                Label11.Text = ""
            Catch ex As Exception
                Label12.Text = "Error-SAVE: " & (ex.Message())
                Label11.Text = ""
            Finally
                SqlConn.Close()
            End Try
        End If
    End Sub
    Public Sub ClearMemFields()
        Label9.Text = ""
        txt_Mem_MemNo.Text = ""
        txt_Mem_MemName.Text = ""
        DDL_Gender.ClearSelection()
        txt_Mem_OffAdd.Text = ""
        txt_Mem_ResAdd.Text = ""
        txt_Mem_Mail.Text = ""
        txt_Mem_Phone.Text = ""
        txt_Mem_Mobile.Text = ""
        DDL_Categories.ClearSelection()
        DDL_SubCategories.ClearSelection()
        DDL_OverRide.ClearSelection()
        txt_Mem_AdmissionDate.Text = ""
        txt_Mem_ClosingDate.Text = ""
        DDL_Subjects.ClearSelection()
        txt_Mem_Keywords.Text = ""
        txt_Mem_Remarks.Text = ""
        DDL_Status.Text = ""
        txt_Mem_NoDueDate.Text = ""
        txt_Mem_DoB.Text = ""
        DDL_Reminder.ClearSelection()
        txt_Mem_FatherName.Text = ""
        txt_Mem_SurityName.Text = ""
        txt_Mem_Profession.Text = ""
        txt_Mem_Qualification.Text = ""
        CheckBox1.Checked = False
        Image1.ImageUrl = Nothing
    End Sub
    Protected Sub Mem_Cancel_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Mem_Cancel_Bttn.Click
        ClearMemFields()
        Mem_Save_Bttn.Visible = True
        Mem_Update_Bttn.Visible = False
    End Sub

    <System.Web.Script.Services.ScriptMethod(), _
System.Web.Services.WebMethod()> _
    Public Shared Function SearchMemNo(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim sdr As SqlDataReader
        Dim cmd As SqlCommand = New SqlCommand
        cmd.CommandText = "select DISTINCT MEM_NO from MEMBERSHIPS where (MEM_NO like '" + prefixText + "%') AND (LIB_CODE ='" & LibCode & "')"
        cmd.Connection = SqlConn
        Try
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            Dim memNo As List(Of String) = New List(Of String)
            sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            While sdr.Read
                memNo.Add(sdr("MEM_NO").ToString)
            End While
            Return memNo
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

    <System.Web.Script.Services.ScriptMethod(), _
System.Web.Services.WebMethod()> _
    Public Shared Function SearchMemName(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim sdr As SqlDataReader
        Dim cmd As SqlCommand = New SqlCommand
        cmd.CommandText = "select DISTINCT MEM_NAME from MEMBERSHIPS where (MEM_NAME like '" + prefixText + "%') AND (LIB_CODE ='" & LibCode & "')"
        cmd.Connection = SqlConn
        Try
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            Dim memName As List(Of String) = New List(Of String)
            sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            While sdr.Read
                memName.Add(sdr("MEM_NAME").ToString)
            End While
            Return memName
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
    'search members in grid
    Private Sub Search_Bttn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Search_Bttn.Click
        PopulateMembersGrid()
    End Sub
    Private Sub DDL_MemNo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDL_MemNo.SelectedIndexChanged
        DDL_MemName.ClearSelection()
        DDL_AllCategories.ClearSelection()
        DDL_AllSubCategories.ClearSelection()
        DDL_Status2.ClearSelection()
        DDL_MemNo.Focus()
    End Sub
    Private Sub DDL_MemName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDL_MemName.SelectedIndexChanged
        DDL_MemNo.ClearSelection()
        DDL_AllCategories.ClearSelection()
        DDL_AllSubCategories.ClearSelection()
        DDL_Status2.ClearSelection()
        DDL_MemName.Focus()
    End Sub
    Private Sub DDL_AllCategories_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDL_AllCategories.SelectedIndexChanged
        DDL_MemNo.ClearSelection()
        DDL_MemName.ClearSelection()
        DDL_AllSubCategories.ClearSelection()
        DDL_Status2.ClearSelection()
        DDL_AllCategories.Focus()
    End Sub
    Private Sub DDL_AllSubCategories_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDL_AllSubCategories.SelectedIndexChanged
        DDL_MemNo.ClearSelection()
        DDL_MemName.ClearSelection()
        DDL_AllCategories.ClearSelection()
        DDL_Status2.ClearSelection()
        DDL_AllSubCategories.Focus()
    End Sub
    Private Sub DDL_Status2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDL_Status2.SelectedIndexChanged
        DDL_MemNo.ClearSelection()
        DDL_MemName.ClearSelection()
        DDL_AllCategories.ClearSelection()
        DDL_AllSubCategories.ClearSelection()
        DDL_Status2.Focus()
    End Sub
    'get value of row from grid
    Private Sub Grid3_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles Grid3.RowCommand
        Dim Command As SqlCommand = Nothing
        Dim dr As SqlDataReader = Nothing
        Try
            If e.CommandName = "Select" Then
                Dim myRowID, MEM_ID As Integer
                myRowID = e.CommandArgument.ToString()
                MEM_ID = Grid3.DataKeys(myRowID).Value
                If Not String.IsNullOrEmpty(MEM_ID) And MEM_ID <> 0 Then
                    Label9.Text = MEM_ID
                    Dim iloop As Integer
                    Dim strcurrentchar As Object
                    Dim c As Integer
                    Dim counter1 As Integer

                    MEM_ID = TrimX(MEM_ID)
                    MEM_ID = RemoveQuotes(MEM_ID)

                    If IsNumeric(MEM_ID) = False Then
                        Label12.Text = "Length of Input is not Proper!"
                        Label11.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Length of Input is not Proper... ');", True)
                        Exit Sub
                    End If

                    If Len(MEM_ID).ToString > 10 Then
                        Label12.Text = "Length of Input is not Proper!"
                        Label11.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Length of Input is not Proper... ');", True)
                        Exit Sub
                    End If
                    MEM_ID = " " & MEM_ID & " "
                    If InStr(1, MEM_ID, " CREATE ", 1) > 0 Or InStr(1, MEM_ID, " DELETE ", 1) > 0 Or InStr(1, MEM_ID, " DROP ", 1) > 0 Or InStr(1, MEM_ID, " INSERT ", 1) > 1 Or InStr(1, MEM_ID, " TRACK ", 1) > 1 Or InStr(1, MEM_ID, " TRACE ", 1) > 1 Then
                        Label12.Text = "Do not use reserve words... !"
                        Label11.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do not use reserve words... ');", True)
                        Exit Sub
                    End If
                    MEM_ID = TrimX(MEM_ID)
                    'get record details from database
                    Dim SQL As String = Nothing
                    SQL = " SELECT *  FROM MEMBERSHIPS WHERE (MEM_ID = '" & Trim(MEM_ID) & "') "
                    Command = New SqlCommand(SQL, SqlConn)
                    SqlConn.Open()
                    dr = Command.ExecuteReader(CommandBehavior.CloseConnection)
                    dr.Read()

                    If dr.HasRows = True Then
                        If dr.Item("MEM_NO").ToString <> "" Then
                            txt_Mem_MemNo.Text = dr.Item("MEM_NO").ToString
                        Else
                            txt_Mem_MemNo.Text = ""
                        End If

                        If dr.Item("MEM_NAME").ToString <> "" Then
                            txt_Mem_MemName.Text = dr.Item("MEM_NAME").ToString
                        Else
                            txt_Mem_MemName.Text = ""
                        End If

                        If dr.Item("MEM_RES_ADD").ToString <> "" Then
                            txt_Mem_ResAdd.Text = dr.Item("MEM_RES_ADD").ToString
                        Else
                            txt_Mem_ResAdd.Text = ""
                        End If

                        If dr.Item("MEM_OFF_ADD").ToString <> "" Then
                            txt_Mem_OffAdd.Text = dr.Item("MEM_OFF_ADD").ToString
                        Else
                            txt_Mem_OffAdd.Text = ""
                        End If

                        If dr.Item("MEM_GENDER").ToString <> "" Then
                            DDL_Gender.SelectedValue = dr.Item("MEM_GENDER").ToString
                        Else
                            DDL_Gender.SelectedValue = "M"
                        End If

                        If dr.Item("CAT_ID").ToString <> "" Then
                            DDL_Categories.SelectedValue = dr.Item("CAT_ID").ToString
                        Else
                            DDL_Categories.Text = ""
                        End If

                        If dr.Item("SUBCAT_ID").ToString <> "" Then
                            DDL_SubCategories.SelectedValue = dr.Item("SUBCAT_ID").ToString
                        Else
                            DDL_SubCategories.Text = ""
                        End If

                        If dr.Item("MEM_EMAIL").ToString <> "" Then
                            txt_Mem_Mail.Text = dr.Item("MEM_EMAIL").ToString
                        Else
                            txt_Mem_Mail.Text = ""
                        End If

                        If dr.Item("MEM_TELEPHONE").ToString <> "" Then
                            txt_Mem_Phone.Text = dr.Item("MEM_TELEPHONE").ToString
                        Else
                            txt_Mem_Phone.Text = ""
                        End If

                        If dr.Item("MEM_MOBILE").ToString <> "" Then
                            txt_Mem_Mobile.Text = dr.Item("MEM_MOBILE").ToString
                        Else
                            txt_Mem_Mobile.Text = ""
                        End If

                        If dr.Item("MEM_OVERRIDE").ToString <> "" Then
                            DDL_OverRide.SelectedValue = dr.Item("MEM_OVERRIDE").ToString
                        Else
                            DDL_OverRide.Text = ""
                        End If

                        If dr.Item("MEM_ADM_DATE").ToString <> "" Then
                            txt_Mem_AdmissionDate.Text = Format(dr.Item("MEM_ADM_DATE"), "dd/MM/yyyy")
                        Else
                            txt_Mem_AdmissionDate.Text = ""
                        End If

                        If dr.Item("MEM_CLOSE_DATE").ToString <> "" Then
                            txt_Mem_ClosingDate.Text = Format(dr.Item("MEM_CLOSE_DATE"), "dd/MM/yyyy")
                        Else
                            txt_Mem_ClosingDate.Text = ""
                        End If

                        If dr.Item("MEM_REMARKS").ToString <> "" Then
                            txt_Mem_Remarks.Text = dr.Item("MEM_REMARKS").ToString
                        Else
                            txt_Mem_Remarks.Text = ""
                        End If

                        If dr.Item("SUB_ID").ToString <> "" Then
                            DDL_Subjects.SelectedValue = dr.Item("SUB_ID").ToString
                        Else
                            DDL_Subjects.Text = ""
                        End If

                        If dr.Item("KEYWORDS").ToString <> "" Then
                            txt_Mem_Keywords.Text = dr.Item("KEYWORDS").ToString
                        Else
                            txt_Mem_Keywords.Text = ""
                        End If

                        If dr.Item("MEM_STATUS").ToString <> "" Then
                            DDL_Status.SelectedValue = dr.Item("MEM_STATUS").ToString
                        Else
                            DDL_Status.Text = ""
                        End If

                        If dr.Item("NO_DUE_DATE").ToString <> "" Then
                            txt_Mem_NoDueDate.Text = Format(dr.Item("NO_DUE_DATE"), "dd/MM/yyyy")
                        Else
                            txt_Mem_NoDueDate.Text = ""
                        End If

                        If dr.Item("MEM_DOB").ToString <> "" Then
                            txt_Mem_DoB.Text = Format(dr.Item("MEM_DOB"), "dd/MM/yyyy")
                        Else
                            txt_Mem_DoB.Text = ""
                        End If

                        If dr.Item("SURITY_NAME").ToString <> "" Then
                            txt_Mem_SurityName.Text = dr.Item("SURITY_NAME").ToString
                        Else
                            txt_Mem_SurityName.Text = ""
                        End If

                        If dr.Item("FATHER_NAME").ToString <> "" Then
                            txt_Mem_FatherName.Text = dr.Item("FATHER_NAME").ToString
                        Else
                            txt_Mem_FatherName.Text = ""
                        End If

                        If dr.Item("PROFESSION").ToString <> "" Then
                            txt_Mem_Profession.Text = dr.Item("PROFESSION").ToString
                        Else
                            txt_Mem_Profession.Text = ""
                        End If

                        If dr.Item("QUALIFICATION").ToString <> "" Then
                            txt_Mem_Qualification.Text = dr.Item("QUALIFICATION").ToString
                        Else
                            txt_Mem_Qualification.Text = ""
                        End If

                        If dr.Item("SEND_REMINDER").ToString <> "" Then
                            DDL_Reminder.SelectedValue = dr.Item("SEND_REMINDER").ToString
                        Else
                            DDL_Reminder.ClearSelection()
                        End If

                        If dr.Item("PHOTO").ToString <> "" Then
                            Dim strURL As String = "~/Circulation/Member_GetPhoto.aspx?MEM_ID=" & Trim(MEM_ID) & ""
                            Image1.ImageUrl = strURL
                            Image1.Visible = True
                            CheckBox1.Visible = True
                        Else
                            Image1.Visible = False
                            CheckBox1.Visible = False
                        End If
                        Mem_Update_Bttn.Visible = True
                        Mem_Update_Bttn.Enabled = True
                        Mem_Save_Bttn.Visible = False
                        Label12.Text = ""
                        Label11.Text = "Press UPDATE Button to save the Changes if any.."
                        dr.Close()
                    Else
                        Mem_Update_Bttn.Visible = True
                        Mem_Update_Bttn.Enabled = True
                        Mem_Save_Bttn.Visible = False
                        Label9.Text = ""
                        Label12.Text = "Record Not Selected to Edit"
                        Label11.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' No Record to Edit... ');", True)
                    End If
                Else
                    Mem_Update_Bttn.Visible = True
                    Mem_Update_Bttn.Enabled = True
                    Mem_Save_Bttn.Visible = False
                    Label9.Text = ""
                    Label12.Text = "Record Not Selected to Edit"
                    Label11.Text = ""
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' No Record to Edit... ');", True)
                End If
            End If
        Catch s As Exception
            Label2.Text = "Error: " & (s.Message())
            Label11.Text = ""
            Label9.Text = ""
            Mem_Update_Bttn.Visible = True
            Mem_Update_Bttn.Enabled = True
            Mem_Save_Bttn.Visible = False
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'update member records
    Protected Sub Mem_Update_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Mem_Update_Bttn.Click
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
                Dim counter1, counter2, counter3, counter4, counter5, counter6, counter7, counter8, counter9, counter10 As Integer
                '****************************************************************************************************
                'validation for cat ID
                Dim MEM_ID As Integer = Nothing
                If Label9.Text <> "" Then
                    MEM_ID = TrimX(Label9.Text)
                    MEM_ID = RemoveQuotes(MEM_ID)

                    If Len(MEM_ID).ToString > 10 Then
                        Label12.Text = "Length of Input is not Proper!"
                        Label11.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Length of Input is not Proper... ');", True)
                        Exit Sub
                    End If
                    If Not IsNumeric(MEM_ID) = True Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        Exit Sub
                    End If

                    MEM_ID = " " & MEM_ID & " "
                    If InStr(1, MEM_ID, " CREATE ", 1) > 0 Or InStr(1, MEM_ID, " DELETE ", 1) > 0 Or InStr(1, MEM_ID, " DROP ", 1) > 0 Or InStr(1, MEM_ID, " INSERT ", 1) > 1 Or InStr(1, MEM_ID, " TRACK ", 1) > 1 Or InStr(1, MEM_ID, " TRACE ", 1) > 1 Then
                        Label12.Text = "Do not use reserve words... !"
                        Label11.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do not use reserve words... ');", True)
                        Exit Sub
                    End If
                    MEM_ID = TrimX(MEM_ID)

                    'check unwanted characters
                    c = 0
                    counter1 = 0
                    For iloop = 1 To Len(MEM_ID.ToString)
                        strcurrentchar = Mid(MEM_ID, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqratuvwxyz;<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter1 = 1
                            End If
                        End If
                    Next
                    If counter1 = 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        Exit Sub
                    End If
                Else
                    Label12.Text = "Select Record to Update !"
                    Label11.Text = ""
                    Exit Sub
                End If

                'validation 
                Dim MEM_NO As Object = Nothing
                If Me.txt_Mem_MemNo.Text <> "" Then
                    MEM_NO = TrimX(txt_Mem_MemNo.Text)
                    MEM_NO = RemoveQuotes(MEM_NO)
                    If MEM_NO.Length > 50 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_MemNo.Focus()
                        Exit Sub
                    End If
                    MEM_NO = " " & MEM_NO & " "
                    If InStr(1, MEM_NO, "CREATE", 1) > 0 Or InStr(1, MEM_NO, "DELETE", 1) > 0 Or InStr(1, MEM_NO, "DROP", 1) > 0 Or InStr(1, MEM_NO, "INSERT", 1) > 1 Or InStr(1, MEM_NO, "TRACK", 1) > 1 Or InStr(1, MEM_NO, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_MemNo.Focus()
                        Exit Sub
                    End If
                    MEM_NO = TrimAll(MEM_NO)

                    'check duplicate category
                    Dim str As Object = Nothing
                    Dim flag As Object = Nothing
                    str = "SELECT MEM_ID FROM MEMBERSHIPS WHERE (MEM_NO ='" & Trim(MEM_NO) & "') AND (MEM_ID<> '" & Trim(MEM_ID) & "') AND (LIB_CODE ='" & Trim(LibCode) & "')"
                    Dim cmd1 As New SqlCommand(str, SqlConn)
                    SqlConn.Open()
                    flag = cmd1.ExecuteScalar
                    SqlConn.Close()
                    If flag <> Nothing Then
                        Label12.Text = "This Member No already exists !"
                        Label11.Text = ""
                        txt_Mem_MemNo.Focus()
                        Exit Sub
                    End If
                Else
                    Label12.Text = "Plz Enter Member No!"
                    Label11.Text = ""
                    txt_Mem_MemNo.Focus()
                    Exit Sub
                End If

                'validation 
                Dim MEM_NAME As Object = Nothing
                If Me.txt_Mem_MemName.Text <> "" Then
                    MEM_NAME = TrimAll(txt_Mem_MemName.Text)
                    MEM_NAME = RemoveQuotes(MEM_NAME)
                    If MEM_NAME.Length > 150 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_MemName.Focus()
                        Exit Sub
                    End If
                    MEM_NAME = " " & MEM_NAME & " "
                    If InStr(1, MEM_NAME, "CREATE", 1) > 0 Or InStr(1, MEM_NAME, "DELETE", 1) > 0 Or InStr(1, MEM_NAME, "DROP", 1) > 0 Or InStr(1, MEM_NAME, "INSERT", 1) > 1 Or InStr(1, MEM_NAME, "TRACK", 1) > 1 Or InStr(1, MEM_NAME, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_MemName.Focus()
                        Exit Sub
                    End If
                    MEM_NAME = TrimAll(MEM_NAME)
                Else
                    Label12.Text = "Plz Enter Member No!"
                    Label11.Text = ""
                    txt_Mem_MemName.Focus()
                    Exit Sub
                End If

                'validation 
                Dim MEM_RES_ADD As Object = Nothing
                If Me.txt_Mem_ResAdd.Text <> "" Then
                    MEM_RES_ADD = TrimAll(txt_Mem_ResAdd.Text)
                    MEM_RES_ADD = RemoveQuotes(MEM_RES_ADD)
                    If MEM_RES_ADD.Length > 250 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_ResAdd.Focus()
                        Exit Sub
                    End If
                    MEM_RES_ADD = " " & MEM_RES_ADD & " "
                    If InStr(1, MEM_RES_ADD, "CREATE", 1) > 0 Or InStr(1, MEM_RES_ADD, "DELETE", 1) > 0 Or InStr(1, MEM_RES_ADD, "DROP", 1) > 0 Or InStr(1, MEM_RES_ADD, "INSERT", 1) > 1 Or InStr(1, MEM_RES_ADD, "TRACK", 1) > 1 Or InStr(1, MEM_RES_ADD, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_ResAdd.Focus()
                        Exit Sub
                    End If
                    MEM_RES_ADD = TrimAll(MEM_RES_ADD)
                Else
                    MEM_RES_ADD = Nothing
                End If

                'validation 
                Dim MEM_OFF_ADD As Object = Nothing
                If Me.txt_Mem_OffAdd.Text <> "" Then
                    MEM_OFF_ADD = TrimAll(txt_Mem_OffAdd.Text)
                    MEM_OFF_ADD = RemoveQuotes(MEM_OFF_ADD)
                    If MEM_OFF_ADD.Length > 250 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_OffAdd.Focus()
                        Exit Sub
                    End If
                    MEM_RES_ADD = " " & MEM_RES_ADD & " "
                    If InStr(1, MEM_OFF_ADD, "CREATE", 1) > 0 Or InStr(1, MEM_OFF_ADD, "DELETE", 1) > 0 Or InStr(1, MEM_OFF_ADD, "DROP", 1) > 0 Or InStr(1, MEM_OFF_ADD, "INSERT", 1) > 1 Or InStr(1, MEM_OFF_ADD, "TRACK", 1) > 1 Or InStr(1, MEM_OFF_ADD, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_OffAdd.Focus()
                        Exit Sub
                    End If
                    MEM_OFF_ADD = TrimAll(MEM_OFF_ADD)
                Else
                    MEM_OFF_ADD = Nothing
                End If

                'validation 
                Dim MEM_GENDER As Object = Nothing
                If Me.DDL_Gender.Text <> "" Then
                    MEM_GENDER = Trim(DDL_Gender.SelectedValue)
                    MEM_GENDER = RemoveQuotes(MEM_GENDER)
                    If MEM_GENDER.Length > 2 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        DDL_Gender.Focus()
                        Exit Sub
                    End If
                    MEM_GENDER = " " & MEM_GENDER & " "
                    If InStr(1, MEM_GENDER, "CREATE", 1) > 0 Or InStr(1, MEM_GENDER, "DELETE", 1) > 0 Or InStr(1, MEM_GENDER, "DROP", 1) > 0 Or InStr(1, MEM_GENDER, "INSERT", 1) > 1 Or InStr(1, MEM_GENDER, "TRACK", 1) > 1 Or InStr(1, MEM_GENDER, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        DDL_Gender.Focus()
                        Exit Sub
                    End If
                    MEM_GENDER = TrimX(MEM_GENDER)
                Else
                    MEM_GENDER = "M"
                End If

                'validation 
                Dim CAT_ID As Integer = Nothing
                If Me.DDL_Categories.Text <> "" Then
                    CAT_ID = Trim(DDL_Categories.SelectedValue)
                    CAT_ID = RemoveQuotes(CAT_ID)
                    If CAT_ID.ToString.Length > 10 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        DDL_Categories.Focus()
                        Exit Sub
                    End If

                    If IsNumeric(CAT_ID) = False Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        DDL_Categories.Focus()
                        Exit Sub
                    End If

                    CAT_ID = " " & CAT_ID & " "
                    If InStr(1, CAT_ID, "CREATE", 1) > 0 Or InStr(1, CAT_ID, "DELETE", 1) > 0 Or InStr(1, CAT_ID, "DROP", 1) > 0 Or InStr(1, CAT_ID, "INSERT", 1) > 1 Or InStr(1, CAT_ID, "TRACK", 1) > 1 Or InStr(1, CAT_ID, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        DDL_Categories.Focus()
                        Exit Sub
                    End If
                    CAT_ID = TrimX(CAT_ID)
                Else
                    Label12.Text = "Plz Select Member Category!"
                    Label11.Text = ""
                    DDL_Categories.Focus()
                    Exit Sub
                End If

                'validation 
                Dim SUBCAT_ID As Integer = Nothing
                If Me.DDL_SubCategories.Text <> "" Then
                    SUBCAT_ID = Trim(DDL_SubCategories.SelectedValue)
                    SUBCAT_ID = RemoveQuotes(SUBCAT_ID)
                    If SUBCAT_ID.ToString.Length > 10 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        DDL_SubCategories.Focus()
                        Exit Sub
                    End If

                    If IsNumeric(SUBCAT_ID) = False Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        DDL_SubCategories.Focus()
                        Exit Sub
                    End If

                    SUBCAT_ID = " " & SUBCAT_ID & " "
                    If InStr(1, SUBCAT_ID, "CREATE", 1) > 0 Or InStr(1, SUBCAT_ID, "DELETE", 1) > 0 Or InStr(1, SUBCAT_ID, "DROP", 1) > 0 Or InStr(1, SUBCAT_ID, "INSERT", 1) > 1 Or InStr(1, SUBCAT_ID, "TRACK", 1) > 1 Or InStr(1, SUBCAT_ID, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        DDL_SubCategories.Focus()
                        Exit Sub
                    End If
                    SUBCAT_ID = TrimX(SUBCAT_ID)
                Else
                    Label12.Text = "Plz Select Member Category!"
                    Label11.Text = ""
                    DDL_SubCategories.Focus()
                    Exit Sub
                End If

                'validation 
                Dim MEM_EMAIL As Object = Nothing
                If Me.txt_Mem_Mail.Text <> "" Then
                    MEM_EMAIL = Trim(txt_Mem_Mail.Text)
                    MEM_EMAIL = RemoveQuotes(MEM_EMAIL)
                    If MEM_EMAIL.Length > 100 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_Mail.Focus()
                        Exit Sub
                    End If
                    MEM_EMAIL = " " & MEM_EMAIL & " "
                    If InStr(1, MEM_EMAIL, "CREATE", 1) > 0 Or InStr(1, MEM_EMAIL, "DELETE", 1) > 0 Or InStr(1, MEM_EMAIL, "DROP", 1) > 0 Or InStr(1, MEM_EMAIL, "INSERT", 1) > 1 Or InStr(1, MEM_EMAIL, "TRACK", 1) > 1 Or InStr(1, MEM_EMAIL, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_Mail.Focus()
                        Exit Sub
                    End If
                    MEM_EMAIL = TrimX(MEM_EMAIL)
                Else
                    MEM_EMAIL = Nothing
                End If

                'validation 
                Dim MEM_TELEPHONE As Object = Nothing
                If Me.txt_Mem_Phone.Text <> "" Then
                    MEM_TELEPHONE = Trim(txt_Mem_Phone.Text)
                    MEM_TELEPHONE = RemoveQuotes(MEM_TELEPHONE)
                    If MEM_TELEPHONE.Length > 50 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_Phone.Focus()
                        Exit Sub
                    End If
                    MEM_TELEPHONE = " " & MEM_TELEPHONE & " "
                    If InStr(1, MEM_TELEPHONE, "CREATE", 1) > 0 Or InStr(1, MEM_TELEPHONE, "DELETE", 1) > 0 Or InStr(1, MEM_TELEPHONE, "DROP", 1) > 0 Or InStr(1, MEM_TELEPHONE, "INSERT", 1) > 1 Or InStr(1, MEM_TELEPHONE, "TRACK", 1) > 1 Or InStr(1, MEM_TELEPHONE, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_Phone.Focus()
                        Exit Sub
                    End If
                    MEM_TELEPHONE = TrimX(MEM_TELEPHONE)
                Else
                    MEM_TELEPHONE = Nothing
                End If

                'validation 
                Dim MEM_MOBILE As Object = Nothing
                If Me.txt_Mem_Mobile.Text <> "" Then
                    MEM_MOBILE = Trim(txt_Mem_Mobile.Text)
                    MEM_MOBILE = RemoveQuotes(MEM_MOBILE)
                    If MEM_MOBILE.Length > 50 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_Mobile.Focus()
                        Exit Sub
                    End If
                    MEM_MOBILE = " " & MEM_MOBILE & " "
                    If InStr(1, MEM_MOBILE, "CREATE", 1) > 0 Or InStr(1, MEM_MOBILE, "DELETE", 1) > 0 Or InStr(1, MEM_MOBILE, "DROP", 1) > 0 Or InStr(1, MEM_MOBILE, "INSERT", 1) > 1 Or InStr(1, MEM_MOBILE, "TRACK", 1) > 1 Or InStr(1, MEM_MOBILE, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_Mobile.Focus()
                        Exit Sub
                    End If
                    MEM_MOBILE = TrimX(MEM_MOBILE)
                Else
                    MEM_MOBILE = Nothing
                End If

                'validation 
                Dim MEM_OVERRIDE As Object = Nothing
                If Me.DDL_OverRide.Text <> "" Then
                    MEM_OVERRIDE = Trim(DDL_OverRide.SelectedValue)
                    MEM_OVERRIDE = RemoveQuotes(MEM_OVERRIDE)
                    If MEM_OVERRIDE.Length > 50 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        DDL_OverRide.Focus()
                        Exit Sub
                    End If
                    MEM_OVERRIDE = " " & MEM_OVERRIDE & " "
                    If InStr(1, MEM_OVERRIDE, "CREATE", 1) > 0 Or InStr(1, MEM_OVERRIDE, "DELETE", 1) > 0 Or InStr(1, MEM_OVERRIDE, "DROP", 1) > 0 Or InStr(1, MEM_OVERRIDE, "INSERT", 1) > 1 Or InStr(1, MEM_OVERRIDE, "TRACK", 1) > 1 Or InStr(1, MEM_OVERRIDE, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        DDL_OverRide.Focus()
                        Exit Sub
                    End If
                    MEM_OVERRIDE = TrimX(MEM_OVERRIDE)
                Else
                    MEM_OVERRIDE = "N"
                End If

                'validation 
                Dim MEM_ADM_DATE As Object = Nothing
                If Me.txt_Mem_AdmissionDate.Text <> "" Then
                    MEM_ADM_DATE = Trim(txt_Mem_AdmissionDate.Text)
                    MEM_ADM_DATE = RemoveQuotes(MEM_ADM_DATE)
                    MEM_ADM_DATE = Convert.ToDateTime(MEM_ADM_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy")
                    If MEM_ADM_DATE.Length > 12 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_AdmissionDate.Focus()
                        Exit Sub
                    End If
                    MEM_ADM_DATE = " " & MEM_ADM_DATE & " "
                    If InStr(1, MEM_ADM_DATE, "CREATE", 1) > 0 Or InStr(1, MEM_ADM_DATE, "DELETE", 1) > 0 Or InStr(1, MEM_ADM_DATE, "DROP", 1) > 0 Or InStr(1, MEM_ADM_DATE, "INSERT", 1) > 1 Or InStr(1, MEM_ADM_DATE, "TRACK", 1) > 1 Or InStr(1, MEM_ADM_DATE, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_AdmissionDate.Focus()
                        Exit Sub
                    End If
                    MEM_ADM_DATE = TrimX(MEM_ADM_DATE)
                Else
                    MEM_ADM_DATE = Convert.ToDateTime(Today.Date, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy")
                End If

                'validation 
                Dim MEM_CLOSE_DATE As Object = Nothing
                If Me.txt_Mem_ClosingDate.Text <> "" Then
                    MEM_CLOSE_DATE = Trim(txt_Mem_ClosingDate.Text)
                    MEM_CLOSE_DATE = RemoveQuotes(MEM_CLOSE_DATE)
                    MEM_CLOSE_DATE = Convert.ToDateTime(MEM_CLOSE_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy")
                    If MEM_CLOSE_DATE.Length > 12 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_ClosingDate.Focus()
                        Exit Sub
                    End If
                    MEM_CLOSE_DATE = " " & MEM_CLOSE_DATE & " "
                    If InStr(1, MEM_CLOSE_DATE, "CREATE", 1) > 0 Or InStr(1, MEM_CLOSE_DATE, "DELETE", 1) > 0 Or InStr(1, MEM_CLOSE_DATE, "DROP", 1) > 0 Or InStr(1, MEM_CLOSE_DATE, "INSERT", 1) > 1 Or InStr(1, MEM_CLOSE_DATE, "TRACK", 1) > 1 Or InStr(1, MEM_CLOSE_DATE, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_ClosingDate.Focus()
                        Exit Sub
                    End If
                    MEM_CLOSE_DATE = TrimX(MEM_CLOSE_DATE)
                Else
                    MEM_CLOSE_DATE = Convert.ToDateTime(Today.Date, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy")
                End If

                'validation 
                Dim MEM_REMARKS As Object = Nothing
                If Me.txt_Mem_Remarks.Text <> "" Then
                    MEM_REMARKS = Trim(txt_Mem_Remarks.Text)
                    MEM_REMARKS = RemoveQuotes(MEM_REMARKS)
                    If MEM_REMARKS.Length > 50 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_Remarks.Focus()
                        Exit Sub
                    End If
                    MEM_REMARKS = " " & MEM_REMARKS & " "
                    If InStr(1, MEM_REMARKS, "CREATE", 1) > 0 Or InStr(1, MEM_REMARKS, "DELETE", 1) > 0 Or InStr(1, MEM_REMARKS, "DROP", 1) > 0 Or InStr(1, MEM_REMARKS, "INSERT", 1) > 1 Or InStr(1, MEM_REMARKS, "TRACK", 1) > 1 Or InStr(1, MEM_REMARKS, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_Remarks.Focus()
                        Exit Sub
                    End If
                    MEM_REMARKS = TrimAll(MEM_REMARKS)
                Else
                    MEM_REMARKS = Nothing
                End If

                'validation 
                Dim SUB_ID As Integer = Nothing
                If Me.DDL_Subjects.Text <> "" Then
                    SUB_ID = Trim(DDL_Subjects.SelectedValue)
                    SUB_ID = RemoveQuotes(SUB_ID)
                    If SUB_ID.ToString.Length > 5 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        DDL_Subjects.Focus()
                        Exit Sub
                    End If

                    If IsNumeric(SUB_ID) = False Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        DDL_Subjects.Focus()
                        Exit Sub
                    End If

                    SUB_ID = " " & SUB_ID & " "
                    If InStr(1, SUB_ID, "CREATE", 1) > 0 Or InStr(1, SUB_ID, "DELETE", 1) > 0 Or InStr(1, SUB_ID, "DROP", 1) > 0 Or InStr(1, SUB_ID, "INSERT", 1) > 1 Or InStr(1, SUB_ID, "TRACK", 1) > 1 Or InStr(1, SUB_ID, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        DDL_Subjects.Focus()
                        Exit Sub
                    End If
                    SUB_ID = TrimX(SUB_ID)
                Else
                    SUB_ID = Nothing
                End If

                'validation 
                Dim KEYWORDS As Object = Nothing
                If Me.txt_Mem_Keywords.Text <> "" Then
                    KEYWORDS = Trim(txt_Mem_Keywords.Text)
                    KEYWORDS = RemoveQuotes(KEYWORDS)
                    If KEYWORDS.Length > 250 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_Keywords.Focus()
                        Exit Sub
                    End If
                    KEYWORDS = " " & KEYWORDS & " "
                    If InStr(1, KEYWORDS, "CREATE", 1) > 0 Or InStr(1, KEYWORDS, "DELETE", 1) > 0 Or InStr(1, KEYWORDS, "DROP", 1) > 0 Or InStr(1, KEYWORDS, "INSERT", 1) > 1 Or InStr(1, KEYWORDS, "TRACK", 1) > 1 Or InStr(1, KEYWORDS, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_Keywords.Focus()
                        Exit Sub
                    End If
                    KEYWORDS = TrimAll(KEYWORDS)
                Else
                    KEYWORDS = Nothing
                End If


                'validation 
                Dim MEM_STATUS As Object = Nothing
                If Me.DDL_Status.Text <> "" Then
                    MEM_STATUS = Trim(DDL_Status.SelectedValue)
                    MEM_STATUS = RemoveQuotes(MEM_STATUS)
                    If MEM_STATUS.ToString.Length > 5 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        DDL_Status.Focus()
                        Exit Sub
                    End If
                    MEM_STATUS = " " & MEM_STATUS & " "
                    If InStr(1, MEM_STATUS, "CREATE", 1) > 0 Or InStr(1, MEM_STATUS, "DELETE", 1) > 0 Or InStr(1, MEM_STATUS, "DROP", 1) > 0 Or InStr(1, MEM_STATUS, "INSERT", 1) > 1 Or InStr(1, MEM_STATUS, "TRACK", 1) > 1 Or InStr(1, MEM_STATUS, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        DDL_Status.Focus()
                        Exit Sub
                    End If
                    MEM_STATUS = TrimX(MEM_STATUS)
                Else
                    MEM_STATUS = "CU"
                End If

                'validation 
                Dim MEM_DOB As Object = Nothing
                If Me.txt_Mem_DoB.Text <> "" Then
                    MEM_DOB = Trim(txt_Mem_DoB.Text)
                    MEM_DOB = RemoveQuotes(MEM_DOB)
                    MEM_DOB = Convert.ToDateTime(MEM_DOB, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy")
                    If MEM_DOB.Length > 12 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_DoB.Focus()
                        Exit Sub
                    End If
                    MEM_DOB = " " & MEM_DOB & " "
                    If InStr(1, MEM_DOB, "CREATE", 1) > 0 Or InStr(1, MEM_DOB, "DELETE", 1) > 0 Or InStr(1, MEM_DOB, "DROP", 1) > 0 Or InStr(1, MEM_DOB, "INSERT", 1) > 1 Or InStr(1, MEM_DOB, "TRACK", 1) > 1 Or InStr(1, MEM_DOB, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_DoB.Focus()
                        Exit Sub
                    End If
                    MEM_DOB = TrimX(MEM_DOB)
                Else
                    MEM_DOB = Nothing
                End If


                'validation 
                Dim SURITY_NAME As Object = Nothing
                If Me.txt_Mem_SurityName.Text <> "" Then
                    SURITY_NAME = TrimAll(txt_Mem_SurityName.Text)
                    SURITY_NAME = RemoveQuotes(SURITY_NAME)
                    If SURITY_NAME.Length >= 150 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_SurityName.Focus()
                        Exit Sub
                    End If
                    SURITY_NAME = " " & SURITY_NAME & " "
                    If InStr(1, SURITY_NAME, "CREATE", 1) > 0 Or InStr(1, SURITY_NAME, "DELETE", 1) > 0 Or InStr(1, SURITY_NAME, "DROP", 1) > 0 Or InStr(1, SURITY_NAME, "INSERT", 1) > 1 Or InStr(1, SURITY_NAME, "TRACK", 1) > 1 Or InStr(1, SURITY_NAME, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_SurityName.Focus()
                        Exit Sub
                    End If
                    SURITY_NAME = TrimAll(SURITY_NAME)
                Else
                    SURITY_NAME = Nothing
                End If

                'validation 
                Dim FATHER_NAME As Object = Nothing
                If Me.txt_Mem_FatherName.Text <> "" Then
                    FATHER_NAME = TrimAll(txt_Mem_FatherName.Text)
                    FATHER_NAME = RemoveQuotes(FATHER_NAME)
                    If FATHER_NAME.Length >= 150 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_FatherName.Focus()
                        Exit Sub
                    End If
                    FATHER_NAME = " " & FATHER_NAME & " "
                    If InStr(1, FATHER_NAME, "CREATE", 1) > 0 Or InStr(1, FATHER_NAME, "DELETE", 1) > 0 Or InStr(1, FATHER_NAME, "DROP", 1) > 0 Or InStr(1, FATHER_NAME, "INSERT", 1) > 1 Or InStr(1, FATHER_NAME, "TRACK", 1) > 1 Or InStr(1, FATHER_NAME, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_FatherName.Focus()
                        Exit Sub
                    End If
                    FATHER_NAME = TrimAll(FATHER_NAME)
                Else
                    FATHER_NAME = Nothing
                End If

                'validation 
                Dim PROFESSION As Object = Nothing
                If Me.txt_Mem_Profession.Text <> "" Then
                    PROFESSION = TrimAll(txt_Mem_Profession.Text)
                    PROFESSION = RemoveQuotes(PROFESSION)
                    If PROFESSION.Length >= 150 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_Profession.Focus()
                        Exit Sub
                    End If
                    PROFESSION = " " & PROFESSION & " "
                    If InStr(1, PROFESSION, "CREATE", 1) > 0 Or InStr(1, PROFESSION, "DELETE", 1) > 0 Or InStr(1, PROFESSION, "DROP", 1) > 0 Or InStr(1, PROFESSION, "INSERT", 1) > 1 Or InStr(1, PROFESSION, "TRACK", 1) > 1 Or InStr(1, PROFESSION, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_Profession.Focus()
                        Exit Sub
                    End If
                    PROFESSION = TrimAll(PROFESSION)
                Else
                    PROFESSION = Nothing
                End If

                'validation 
                Dim QUALIFICATION As Object = Nothing
                If Me.txt_Mem_Qualification.Text <> "" Then
                    QUALIFICATION = TrimAll(txt_Mem_Qualification.Text)
                    QUALIFICATION = RemoveQuotes(QUALIFICATION)
                    If QUALIFICATION.Length >= 150 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_Qualification.Focus()
                        Exit Sub
                    End If
                    QUALIFICATION = " " & QUALIFICATION & " "
                    If InStr(1, QUALIFICATION, "CREATE", 1) > 0 Or InStr(1, QUALIFICATION, "DELETE", 1) > 0 Or InStr(1, QUALIFICATION, "DROP", 1) > 0 Or InStr(1, QUALIFICATION, "INSERT", 1) > 1 Or InStr(1, QUALIFICATION, "TRACK", 1) > 1 Or InStr(1, QUALIFICATION, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_Qualification.Focus()
                        Exit Sub
                    End If
                    QUALIFICATION = TrimAll(QUALIFICATION)
                Else
                    QUALIFICATION = Nothing
                End If

                'validation 
                Dim SEND_REMINDER As Object = Nothing
                If Me.DDL_Reminder.Text <> "" Then
                    SEND_REMINDER = Trim(DDL_Reminder.SelectedValue)
                    SEND_REMINDER = RemoveQuotes(SEND_REMINDER)
                    If SEND_REMINDER.ToString.Length > 2 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        DDL_Reminder.Focus()
                        Exit Sub
                    End If
                    SEND_REMINDER = " " & SEND_REMINDER & " "
                    If InStr(1, SEND_REMINDER, "CREATE", 1) > 0 Or InStr(1, SEND_REMINDER, "DELETE", 1) > 0 Or InStr(1, SEND_REMINDER, "DROP", 1) > 0 Or InStr(1, SEND_REMINDER, "INSERT", 1) > 1 Or InStr(1, SEND_REMINDER, "TRACK", 1) > 1 Or InStr(1, SEND_REMINDER, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        DDL_Reminder.Focus()
                        Exit Sub
                    End If
                    SEND_REMINDER = TrimX(SEND_REMINDER)
                Else
                    SEND_REMINDER = "Y"
                End If

                'SERVER VALIDATION FOR PASSWORD
                '*******************************************************************************************************
                Dim Password As Object = Nothing
                If CheckBox2.Checked = True Then
                    Dim Hashed As Object = Nothing
                    Hashed = HashPass2.Value 'Request.Form("HashPass")
                    Password = TrimX(Hashed)
                    If Not String.IsNullOrEmpty(Password) Then
                        Password = RemoveQuotes(Password)
                        If Password.Length > 72 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Length of password is not Proper... ');", True)
                            Me.txt_UserPass.Focus()
                            Exit Sub
                        End If
                        If InStr(1, Password, "CREATE", 1) > 0 Or InStr(1, Password, "DELETE", 1) > 0 Or InStr(1, Password, "DROP", 1) > 0 Or InStr(1, Password, "INSERT", 1) > 1 Or InStr(1, Password, "TRACK", 1) > 1 Or InStr(1, Password, "TRACE", 1) > 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Do not use reserve words... ');", True)
                            Me.txt_UserPass.Focus()
                            Exit Sub
                        End If
                    Else
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Enter Password... ');", True)
                        Me.txt_UserPass.Focus()
                        Exit Sub
                    End If
                    'check unwanted characters
                    c = 0
                    counter10 = 0
                    For iloop = 1 To Len(Password)
                        strcurrentchar = Mid(Password, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter10 = 1
                            End If
                        End If
                    Next
                    If counter10 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Do Not Use Un-wanted Characters... ');", True)
                        Me.txt_UserPass.Focus()
                        Exit Sub
                    End If
                End If


                'validation 
                Dim NO_DUE_DATE As Object = Nothing
                If Me.txt_Mem_NoDueDate.Text <> "" Then
                    NO_DUE_DATE = Trim(txt_Mem_NoDueDate.Text)
                    NO_DUE_DATE = RemoveQuotes(NO_DUE_DATE)
                    NO_DUE_DATE = Convert.ToDateTime(NO_DUE_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy")
                    If NO_DUE_DATE.Length > 12 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_NoDueDate.Focus()
                        Exit Sub
                    End If
                    NO_DUE_DATE = " " & NO_DUE_DATE & " "
                    If InStr(1, NO_DUE_DATE, "CREATE", 1) > 0 Or InStr(1, NO_DUE_DATE, "DELETE", 1) > 0 Or InStr(1, NO_DUE_DATE, "DROP", 1) > 0 Or InStr(1, NO_DUE_DATE, "INSERT", 1) > 1 Or InStr(1, NO_DUE_DATE, "TRACK", 1) > 1 Or InStr(1, NO_DUE_DATE, "TRACE", 1) > 1 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        txt_Mem_NoDueDate.Focus()
                        Exit Sub
                    End If
                    NO_DUE_DATE = TrimX(NO_DUE_DATE)
                Else
                    NO_DUE_DATE = Nothing
                End If



                If NO_DUE_DATE <> "" Then
                    MEM_STATUS = "CL"
                    SEND_REMINDER = "N"
                End If

                'upload content file
                Dim arrContent1 As Byte()
                Dim FileType As String = Nothing
                Dim FileExtension As String = Nothing
                Dim intLength1 As Integer = 0
                If FileUpload1.FileName = "" Then
                    '    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Select Proper file... ');", True)
                    '    Me.FileUpload12.Focus()
                    '    Exit Sub
                Else
                    Dim ContfileName As String = FileUpload1.PostedFile.FileName
                    FileExtension = ContfileName.Substring(ContfileName.LastIndexOf("."))
                    FileExtension = FileExtension.ToLower
                    FileType = FileUpload1.PostedFile.ContentType

                    intLength1 = Convert.ToInt32(FileUpload1.PostedFile.InputStream.Length)
                    ReDim arrContent1(intLength1)
                    FileUpload1.PostedFile.InputStream.Read(arrContent1, 0, intLength1)

                    If intLength1 > 15000 Then
                        Label12.Text = "Error: Photo Size is Bigger than 15 KB"
                        Label11.Text = ""
                        Exit Sub
                    End If
                    'Image1.ImageUrl = FileUpload1.PostedFile.FileName '"~/Images/1.png"
                End If





                Dim LIB_CODE As Object = Nothing
                If Session.Item("LoggedLibcode") <> "" Then
                    LIB_CODE = Session.Item("LoggedLibcode")
                Else
                    Label12.Text = "No Library Code Exists..Login Again  "
                    Label11.Text = ""
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
                If Label9.Text <> "" Then
                    SQL = "SELECT * FROM MEMBERSHIPS WHERE (MEM_ID='" & Trim(MEM_ID) & "')"
                    SqlConn.Open()
                    da = New SqlDataAdapter(SQL, SqlConn)
                    cb = New SqlCommandBuilder(da)
                    da.Fill(ds, "MEM")
                    If ds.Tables("MEM").Rows.Count <> 0 Then
                        If Not String.IsNullOrEmpty(MEM_NO) Then
                            ds.Tables("MEM").Rows(0)("MEM_NO") = MEM_NO.Trim
                        Else
                            ds.Tables("MEM").Rows(0)("MEM_NO") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(MEM_NAME) Then
                            ds.Tables("MEM").Rows(0)("MEM_NAME") = MEM_NAME.Trim
                        Else
                            ds.Tables("MEM").Rows(0)("MEM_NAME") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(MEM_RES_ADD) Then
                            ds.Tables("MEM").Rows(0)("MEM_RES_ADD") = MEM_RES_ADD.Trim
                        Else
                            ds.Tables("MEM").Rows(0)("MEM_RES_ADD") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(MEM_OFF_ADD) Then
                            ds.Tables("MEM").Rows(0)("MEM_OFF_ADD") = MEM_OFF_ADD.Trim
                        Else
                            ds.Tables("MEM").Rows(0)("MEM_OFF_ADD") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(MEM_GENDER) Then
                            ds.Tables("MEM").Rows(0)("MEM_GENDER") = MEM_GENDER.Trim
                        Else
                            ds.Tables("MEM").Rows(0)("MEM_GENDER") = System.DBNull.Value
                        End If

                        If CAT_ID <> 0 Then
                            ds.Tables("MEM").Rows(0)("CAT_ID") = CAT_ID
                        Else
                            ds.Tables("MEM").Rows(0)("CAT_ID") = System.DBNull.Value
                        End If

                        If SUBCAT_ID <> 0 Then
                            ds.Tables("MEM").Rows(0)("SUBCAT_ID") = SUBCAT_ID
                        Else
                            ds.Tables("MEM").Rows(0)("SUBCAT_ID") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(MEM_EMAIL) Then
                            ds.Tables("MEM").Rows(0)("MEM_EMAIL") = MEM_EMAIL.Trim
                        Else
                            ds.Tables("MEM").Rows(0)("MEM_EMAIL") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(MEM_TELEPHONE) Then
                            ds.Tables("MEM").Rows(0)("MEM_TELEPHONE") = MEM_TELEPHONE.Trim
                        Else
                            ds.Tables("MEM").Rows(0)("MEM_TELEPHONE") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(MEM_MOBILE) Then
                            ds.Tables("MEM").Rows(0)("MEM_MOBILE") = MEM_MOBILE.Trim
                        Else
                            ds.Tables("MEM").Rows(0)("MEM_MOBILE") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(MEM_OVERRIDE) Then
                            ds.Tables("MEM").Rows(0)("MEM_OVERRIDE") = MEM_OVERRIDE
                        Else
                            ds.Tables("MEM").Rows(0)("MEM_OVERRIDE") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(MEM_ADM_DATE) Then
                            ds.Tables("MEM").Rows(0)("MEM_ADM_DATE") = MEM_ADM_DATE.Trim
                        Else
                            ds.Tables("MEM").Rows(0)("MEM_ADM_DATE") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(MEM_CLOSE_DATE) Then
                            ds.Tables("MEM").Rows(0)("MEM_CLOSE_DATE") = MEM_CLOSE_DATE.Trim
                        Else
                            ds.Tables("MEM").Rows(0)("MEM_CLOSE_DATE") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(MEM_REMARKS) Then
                            ds.Tables("MEM").Rows(0)("MEM_REMARKS") = MEM_REMARKS.Trim
                        Else
                            ds.Tables("MEM").Rows(0)("MEM_REMARKS") = System.DBNull.Value
                        End If

                        If SUB_ID <> 0 Then
                            ds.Tables("MEM").Rows(0)("SUB_ID") = SUB_ID
                        Else
                            ds.Tables("MEM").Rows(0)("SUB_ID") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(KEYWORDS) Then
                            ds.Tables("MEM").Rows(0)("KEYWORDS") = KEYWORDS.Trim
                        Else
                            ds.Tables("MEM").Rows(0)("KEYWORDS") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(MEM_STATUS) Then
                            ds.Tables("MEM").Rows(0)("MEM_STATUS") = MEM_STATUS.Trim
                        Else
                            ds.Tables("MEM").Rows(0)("MEM_STATUS") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(NO_DUE_DATE) Then
                            ds.Tables("MEM").Rows(0)("NO_DUE_DATE") = NO_DUE_DATE.Trim
                        Else
                            ds.Tables("MEM").Rows(0)("NO_DUE_DATE") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(MEM_DOB) Then
                            ds.Tables("MEM").Rows(0)("MEM_DOB") = MEM_DOB.Trim
                        Else
                            ds.Tables("MEM").Rows(0)("MEM_DOB") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(SURITY_NAME) Then
                            ds.Tables("MEM").Rows(0)("SURITY_NAME") = SURITY_NAME.Trim
                        Else
                            ds.Tables("MEM").Rows(0)("SURITY_NAME") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(FATHER_NAME) Then
                            ds.Tables("MEM").Rows(0)("FATHER_NAME") = FATHER_NAME.Trim
                        Else
                            ds.Tables("MEM").Rows(0)("FATHER_NAME") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(PROFESSION) Then
                            ds.Tables("MEM").Rows(0)("PROFESSION") = PROFESSION.Trim
                        Else
                            ds.Tables("MEM").Rows(0)("PROFESSION") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(QUALIFICATION) Then
                            ds.Tables("MEM").Rows(0)("QUALIFICATION") = QUALIFICATION.Trim
                        Else
                            ds.Tables("MEM").Rows(0)("QUALIFICATION") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(SEND_REMINDER) Then
                            ds.Tables("MEM").Rows(0)("SEND_REMINDER") = SEND_REMINDER.Trim
                        Else
                            ds.Tables("MEM").Rows(0)("SEND_REMINDER") = System.DBNull.Value
                        End If

                        If CheckBox2.Checked = True Then
                            If Not String.IsNullOrEmpty(Password) Then
                                ds.Tables("MEM").Rows(0)("MEMB_PASSWORD") = Password.Trim
                            End If
                        End If

                        If FileUpload1.FileName <> "" Then
                            ds.Tables("MEM").Rows(0)("PHOTO") = arrContent1
                        Else
                            If CheckBox1.Checked = True Then
                                ds.Tables("MEM").Rows(0)("PHOTO") = System.DBNull.Value
                            End If
                        End If

                        ds.Tables("MEM").Rows(0)("UPDATED_BY") = USER_CODE
                        ds.Tables("MEM").Rows(0)("DATE_MODIFIED") = DATE_MODIFIED
                        ds.Tables("MEM").Rows(0)("IP") = IP

                        thisTransaction = SqlConn.BeginTransaction()
                        da.SelectCommand.Transaction = thisTransaction
                        da.Update(ds, "MEM")

                        thisTransaction.Commit()
                        Label12.Text = ""
                        Label11.Text = "Record Updated Successfully!"

                        Mem_Save_Bttn.Visible = True
                        Mem_Save_Bttn.Enabled = True
                        Mem_Update_Bttn.Visible = False

                        Label9.Text = ""
                        ClearMemFields()
                        PopulateMembersGrid()
                    Else
                        Label12.Text = "Record Updation not done  - Please Contact System Administrator"
                        Label11.Text = ""
                    End If
                    End If
            Else
                Label12.Text = "Record Not Selected..."
                Label11.Text = ""
            End If
            SqlConn.Close()
        Catch q As SqlException
            thisTransaction.Rollback()
            Label12.Text = "Error: in Updating Record!"
        Catch s As Exception
            Label12.Text = "Error: " & (s.Message())
            Label11.Text = ""
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'delete cover photo from datbase in cat record
    Private Sub Mem_DeletedPhoto_All_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Mem_DeletedPhoto_All.Click
        Dim SQL As String = Nothing
        Dim da As SqlDataAdapter
        Dim ds As New DataSet
        Dim dt As New DataTable
        Dim thisTransaction As SqlClient.SqlTransaction
        If SqlConn.State = 0 Then
            SqlConn.Open()
        End If
        Try
            If Grid3.Rows.Count <> 0 Then
                For Each row As GridViewRow In Grid3.Rows
                    If row.RowType = DataControlRowType.DataRow Then
                        Dim cb As CheckBox = DirectCast(row.FindControl("cbd"), CheckBox)
                        If cb IsNot Nothing AndAlso cb.Checked = True Then
                            Dim MEM_ID As Integer = Convert.ToInt32(Grid3.DataKeys(row.RowIndex).Value)

                            'UPDATE THE LIBRARY PROFILE  
                            If MEM_ID <> 0 Then
                                thisTransaction = SqlConn.BeginTransaction()
                                Dim intValue As Integer = 0
                                Dim objCommand As New SqlCommand
                                objCommand.Connection = SqlConn
                                objCommand.Transaction = thisTransaction
                                objCommand.CommandType = CommandType.Text
                                objCommand.CommandText = "UPDATE MEMBERSHIPS SET DATE_MODIFIED =@DateModified, UPDATED_BY=@USER_CODE, IP=@myIP, PHOTO=@PHOTO WHERE MEM_ID = @MEM_ID"

                                objCommand.Parameters.Add("@MEM_ID", SqlDbType.Int)
                                objCommand.Parameters("@MEM_ID").Value = MEM_ID

                                objCommand.Parameters.Add("@DateModified", SqlDbType.DateTime)
                                objCommand.Parameters("@DateModified").Value = Now.Date

                                objCommand.Parameters.Add("@USER_CODE", SqlDbType.NVarChar)
                                objCommand.Parameters("@USER_CODE").Value = UserCode

                                objCommand.Parameters.Add("@myIP", SqlDbType.NVarChar)
                                objCommand.Parameters("@myIP").Value = Request.UserHostAddress.Trim

                                objCommand.Parameters.Add("@PHOTO", SqlDbType.Image)
                                objCommand.Parameters("@PHOTO").Value = System.DBNull.Value

                                objCommand.ExecuteNonQuery()

                                thisTransaction.Commit()
                                Label6.Text = ""
                                Label15.Text = "Cover Photo Deleted from Database!"
                                ' DirectCast(Grid3.Rows(row).Cells(0).FindControl("cbd"), CheckBox).Checked = False
                                cb.Checked = False
                            End If
                        End If
                    End If
                Next
            End If

        Catch q As SqlException
            thisTransaction.Rollback()
            Label12.Text = "Database Error -SAVE: " & (q.Message() & ", " & q.Number & ", " & q.GetType.ToString)
            Label11.Text = ""
        Catch ex As Exception
            Label12.Text = "Error: " & (ex.Message())
            Label11.Text = ""
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'delete selected member records
    Protected Sub Mem_Delete_All_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Mem_Delete_All.Click
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing
        Try
            For Each row As GridViewRow In Grid3.Rows
                Dim cb As CheckBox = DirectCast(row.FindControl("cbd"), CheckBox)
                If cb IsNot Nothing AndAlso cb.Checked = True Then
                    Dim MEM_ID As Integer = Convert.ToInt32(Grid3.DataKeys(row.RowIndex).Value)

                    Dim LIB_CODE As Object = Nothing
                    LIB_CODE = LibCode

                    'check reference in CIRCULATION table
                    Dim str As Object = Nothing
                    Dim flag As Object = Nothing
                    str = "SELECT MEM_ID FROM CIRCULATION WHERE (MEM_ID ='" & Trim(MEM_ID) & "')"
                    Dim cmd1 As New SqlCommand(str, SqlConn)
                    SqlConn.Open()
                    flag = cmd1.ExecuteScalar
                    SqlConn.Close()
                    If flag <> Nothing Then
                        Label6.Text = "There are transactions pending in the member name in Circulation Table..so first you must delete all the transactions of this member in TRANSACTION Form !"
                        Label15.Text = ""
                        ' ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('This Member Category can not be deleted - it is saved with Member Records !');", True)
                    Else
                        If SqlConn.State = 0 Then
                            SqlConn.Open()
                        End If
                        thisTransaction = SqlConn.BeginTransaction()
                        Dim objCommand As New SqlCommand
                        objCommand.Connection = SqlConn
                        objCommand.Transaction = thisTransaction
                        objCommand.CommandType = CommandType.Text
                        objCommand.CommandText = "DELETE FROM MEMBERSHIPS WHERE (MEM_ID =@MEM_ID and LIB_CODE =@LIB_CODE) "

                        objCommand.Parameters.Add("@MEM_ID", SqlDbType.Int)
                        objCommand.Parameters("@MEM_ID").Value = MEM_ID

                        If LIB_CODE = "" Then LIB_CODE = System.DBNull.Value
                        objCommand.Parameters.Add("@LIB_CODE", SqlDbType.NVarChar)
                        objCommand.Parameters("@LIB_CODE").Value = LIB_CODE

                        objCommand.ExecuteNonQuery()

                    End If

                    thisTransaction.Commit()
                    SqlConn.Close()
                End If
            Next
            Dim s As Object = Nothing
            Dim e1 As EventArgs = Nothing
            PopulateMembersGrid()
        Catch s As Exception
            thisTransaction.Rollback()
            Label11.Text = ""
            Label12.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'change class of selected members
    Protected Sub Mem_ChangeSubCatergory_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Mem_ChangeSubCatergory_Bttn.Click
        Dim SQL As String = Nothing
        Dim da As SqlDataAdapter
        Dim ds As New DataSet
        Dim dt As New DataTable
        Dim thisTransaction As SqlClient.SqlTransaction
        If SqlConn.State = 0 Then
            SqlConn.Open()
        End If
        Try
            If Grid3.Rows.Count <> 0 Then

                Dim NEW_SUBCAT_ID As Integer = Nothing
                If DDL_NewSubCategories.Text <> "" Then
                    NEW_SUBCAT_ID = Trim(DDL_NewSubCategories.SelectedValue)
                    NEW_SUBCAT_ID = RemoveQuotes(NEW_SUBCAT_ID)
                    If NEW_SUBCAT_ID.ToString.Length > 10 Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        DDL_NewSubCategories.Focus()
                        Exit Sub
                    End If

                    If IsNumeric(NEW_SUBCAT_ID) = False Then
                        Label12.Text = "Error: Input is not Valid !"
                        Label11.Text = ""
                        DDL_NewSubCategories.Focus()
                        Exit Sub
                    End If
                Else
                    Label12.Text = "Plz Select Member Sub Category!"
                    Label11.Text = ""
                    DDL_NewSubCategories.Focus()
                    Exit Sub
                End If


                For Each row As GridViewRow In Grid3.Rows
                    If row.RowType = DataControlRowType.DataRow Then
                        Dim cb As CheckBox = DirectCast(row.FindControl("cbd"), CheckBox)
                        If cb IsNot Nothing AndAlso cb.Checked = True Then
                            Dim MEM_ID As Integer = Convert.ToInt32(Grid3.DataKeys(row.RowIndex).Value)

                            'UPDATE   
                            If MEM_ID <> 0 Then
                                thisTransaction = SqlConn.BeginTransaction()
                                Dim intValue As Integer = 0
                                Dim objCommand As New SqlCommand
                                objCommand.Connection = SqlConn
                                objCommand.Transaction = thisTransaction
                                objCommand.CommandType = CommandType.Text
                                objCommand.CommandText = "UPDATE MEMBERSHIPS SET DATE_MODIFIED =@DateModified, UPDATED_BY=@USER_CODE, IP=@myIP, SUBCAT_ID=@SUBCAT_ID WHERE MEM_ID = @MEM_ID"

                                objCommand.Parameters.Add("@MEM_ID", SqlDbType.Int)
                                objCommand.Parameters("@MEM_ID").Value = MEM_ID

                                objCommand.Parameters.Add("@DateModified", SqlDbType.DateTime)
                                objCommand.Parameters("@DateModified").Value = Now.Date

                                objCommand.Parameters.Add("@USER_CODE", SqlDbType.NVarChar)
                                objCommand.Parameters("@USER_CODE").Value = UserCode

                                objCommand.Parameters.Add("@myIP", SqlDbType.NVarChar)
                                objCommand.Parameters("@myIP").Value = Request.UserHostAddress.Trim

                                objCommand.Parameters.Add("@SUBCAT_ID", SqlDbType.Int)
                                objCommand.Parameters("@SUBCAT_ID").Value = NEW_SUBCAT_ID

                                objCommand.ExecuteNonQuery()

                                thisTransaction.Commit()
                                Label6.Text = ""
                                Label15.Text = "Member Records Updated!"
                                cb.Checked = False
                            End If
                        End If
                    End If
                Next
            End If
            Dim s As Object = Nothing
            Dim e1 As EventArgs = Nothing
            PopulateMembersGrid()
        Catch q As SqlException
            thisTransaction.Rollback()
            Label12.Text = "Database Error -SAVE: " & (q.Message() & ", " & q.Number & ", " & q.GetType.ToString)
            Label11.Text = ""
        Catch ex As Exception
            Label12.Text = "Error: " & (ex.Message())
            Label11.Text = ""
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'enable/disable password text boxes   
    Protected Sub CheckBox2_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles CheckBox2.CheckedChanged
        If CheckBox2.Checked = True Then
            txt_UserPass.Enabled = True
            txt_UserRePass.Enabled = True
            txt_UserPass.Focus()
        Else
            txt_UserPass.Enabled = False
            txt_UserRePass.Enabled = False
        End If
    End Sub
End Class