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
Public Class Letters
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
                CreateSUserButton = Master.FindControl("Accordion1").FindControl("MasterPane").FindControl("M_Letter_Bttn")
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
    Private Sub Letters_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        txt_Form_Name.Focus()
    End Sub
    'save user account
    Protected Sub Save_Click(ByVal sender As Object, ByVal e As EventArgs) Handles bttn_Save.Click
        If IsPostBack = True Then
            'check unwanted characters
            Dim iloop As Integer
            Dim strcurrentchar As Object
            Dim c As Integer
            Dim counter1, counter2, counter3, counter4, counter5, counter6, counter7 As Integer
            Dim thisTransaction As SqlClient.SqlTransaction = Nothing
            Try
                '****************************************************************************************
                'Server Validation for form name
                Dim Name As String = Nothing
                If txt_Form_Name.Text <> "" Then
                    Name = TrimAll(UCase(txt_Form_Name.Text))
                    Name = RemoveQuotes(Name)
                    If Name.Length > 50 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        txt_Form_Name.Focus()
                        Exit Sub
                    End If
                    Name = " " & Name & " "
                    If InStr(1, Name, "CREATE", 1) > 0 Or InStr(1, Name, "DELETE", 1) > 0 Or InStr(1, Name, "DROP", 1) > 0 Or InStr(1, Name, "INSERT", 1) > 1 Or InStr(1, Name, "TRACK", 1) > 1 Or InStr(1, Name, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Me.txt_Form_Name.Focus()
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
                        Me.txt_Form_Name.Focus()
                        Exit Sub
                    End If
                    Name = UCase(Name)
                    'Check Duplicate form name
                    Dim str As Object = Nothing
                    Dim flag As Object = Nothing
                    str = "SELECT MESS_ID FROM MESSAGES WHERE (LIB_CODE = '" & Trim(LibCode) & "' AND FORM_NAME ='" & Trim(Name) & "')"
                    Dim cmd1 As New SqlCommand(str, SqlConn)
                    SqlConn.Open()
                    flag = cmd1.ExecuteScalar
                    If flag <> Nothing Then
                        Label6.Text = "Form Already exists..Enter another form"
                        Me.txt_Form_Name.Focus()
                        Exit Sub
                    End If
                    SqlConn.Close()
                Else
                    Label6.Text = " Plz Enter Form Name! "
                    Me.txt_Form_Name.Focus()
                    Exit Sub
                End If

                '****************************************************************************************88
                'File No
                Dim myFileNo As String = Nothing
                If txt_Form_FileNo.Text <> "" Then
                    myFileNo = TrimAll(Me.txt_Form_FileNo.Text)
                    myFileNo = RemoveQuotes(myFileNo)
                    If myFileNo.Length > 200 Then
                        Label6.Text = " Input is not Valid... "
                        Me.txt_Form_FileNo.Focus()
                        Exit Sub
                    End If
                    myFileNo = " " & myFileNo & " "
                    If InStr(1, myFileNo, "CREATE", 1) > 0 Or InStr(1, myFileNo, "DELETE", 1) > 0 Or InStr(1, myFileNo, "DROP", 1) > 0 Or InStr(1, myFileNo, "INSERT", 1) > 1 Or InStr(1, myFileNo, "TRACK", 1) > 1 Or InStr(1, myFileNo, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Me.txt_Form_FileNo.Focus()
                        Exit Sub
                    End If
                    myFileNo = TrimX(myFileNo)
                    'check unwanted characters
                    c = 0
                    counter2 = 0
                    For iloop = 1 To Len(myFileNo)
                        strcurrentchar = Mid(myFileNo, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter2 = 1
                            End If
                        End If
                    Next
                    myFileNo = UCase(myFileNo)
                    If counter2 = 1 Then
                        Label6.Text = " Input is not Valid..."
                        Me.txt_Form_FileNo.Focus()
                        Exit Sub
                    End If
                Else
                    myFileNo = String.Empty
                End If

                '****************************************************************************************88
                'letter subject
                Dim mySubject As String = Nothing
                If txt_Form_Subject.Text <> "" Then
                    mySubject = TrimAll(Me.txt_Form_Subject.Text)
                    mySubject = RemoveQuotes(mySubject)
                    If mySubject.Length > 250 Then
                        Label6.Text = " Input is not Valid... "
                        Me.txt_Form_Subject.Focus()
                        Exit Sub
                    End If
                    mySubject = " " & mySubject & " "
                    If InStr(1, mySubject, "CREATE", 1) > 0 Or InStr(1, mySubject, "DELETE", 1) > 0 Or InStr(1, mySubject, "DROP", 1) > 0 Or InStr(1, mySubject, "INSERT", 1) > 1 Or InStr(1, mySubject, "TRACK", 1) > 1 Or InStr(1, mySubject, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Me.txt_Form_Subject.Focus()
                        Exit Sub
                    End If
                    mySubject = TrimAll(mySubject)
                    'check unwanted characters
                    c = 0
                    counter3 = 0
                    For iloop = 1 To Len(mySubject)
                        strcurrentchar = Mid(mySubject, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter3 = 1
                            End If
                        End If
                    Next
                    If counter3 = 1 Then
                        Label6.Text = " Input is not Valid..."
                        Me.txt_Form_Subject.Focus()
                        Exit Sub
                    End If
                Else
                    Label6.Text = " Plz Enter Letter Subect! "
                    Me.txt_Form_Subject.Focus()
                    Exit Sub
                End If

                '****************************************************************************************88
                'top message
                Dim myTopMsg As Object = Nothing
                If txt_Form_TopMsg.text <> "" Then
                    myTopMsg = TrimAll(Me.txt_Form_TopMsg.Text)
                    myTopMsg = RemoveQuotes(myTopMsg)
                    If myTopMsg.ToString.Length > 2000 Then
                        Label6.Text = " Input is not Valid... "
                        Me.txt_Form_TopMsg.Focus()
                        Exit Sub
                    End If
                    myTopMsg = TrimAll(myTopMsg)
                    'check unwanted characters
                    c = 0
                    counter4 = 0
                    For iloop = 1 To myTopMsg.ToString.Length
                        strcurrentchar = Mid(myTopMsg, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter4 = 1
                            End If
                        End If
                    Next
                    If counter4 = 1 Then
                        Label6.Text = " Input is not Valid..."
                        Me.txt_Form_TopMsg.Focus()
                        Exit Sub
                    End If
                Else
                    Label6.Text = " Plz Enter Top Message of the Letter! "
                    Me.txt_Form_TopMsg.Focus()
                    Exit Sub
                End If

                '****************************************************************************************88
                'Bottom message
                Dim myBottomMsg As Object = Nothing
                If txt_Form_TopMsg.Text <> "" Then
                    myBottomMsg = TrimAll(Me.txt_Form_BottomMsg.Text)
                    myBottomMsg = RemoveQuotes(myBottomMsg)
                    If myBottomMsg.ToString.Length > 2000 Then
                        Label6.Text = " Input is not Valid... "
                        Me.txt_Form_BottomMsg.Focus()
                        Exit Sub
                    End If
                    myBottomMsg = TrimAll(myBottomMsg)
                    'check unwanted characters
                    c = 0
                    counter5 = 0
                    For iloop = 1 To myBottomMsg.ToString.Length
                        strcurrentchar = Mid(myBottomMsg, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter5 = 1
                            End If
                        End If
                    Next
                    If counter5 = 1 Then
                        Label6.Text = " Input is not Valid..."
                        Me.txt_Form_BottomMsg.Focus()
                        Exit Sub
                    End If
                Else
                    Label6.Text = " Plz Enter bottom Message of the Letter! "
                    Me.txt_Form_BottomMsg.Focus()
                    Exit Sub
                End If

                '****************************************************************************************88
                'sign authority
                Dim mySign As Object = Nothing
                If txt_Form_Sign.Text <> "" Then
                    mySign = TrimAll(Me.txt_Form_Sign.Text)
                    mySign = RemoveQuotes(mySign)
                    If mySign.ToString.Length > 100 Then
                        Label6.Text = " Input is not Valid... "
                        Me.txt_Form_Sign.Focus()
                        Exit Sub
                    End If
                    mySign = " " & mySign & " "
                    If InStr(1, mySign, "CREATE", 1) > 0 Or InStr(1, mySign, "DELETE", 1) > 0 Or InStr(1, mySign, "DROP", 1) > 0 Or InStr(1, mySign, "INSERT", 1) > 1 Or InStr(1, mySign, "TRACK", 1) > 1 Or InStr(1, mySign, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Me.txt_Form_Sign.Focus()
                        Exit Sub
                    End If
                    mySign = TrimAll(mySign)
                    'check unwanted characters
                    c = 0
                    counter6 = 0
                    For iloop = 1 To mySign.ToString.Length
                        strcurrentchar = Mid(mySign, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter6 = 1
                            End If
                        End If
                    Next
                    If counter6 = 1 Then
                        Label6.Text = " Input is not Valid..."
                        Me.txt_Form_Sign.Focus()
                        Exit Sub
                    End If
                Else
                    Label6.Text = " Plz Enter bottom Message of the Letter! "
                    Me.txt_Form_Sign.Focus()
                    Exit Sub
                End If

                '****************************************************************************************88
                'Remarks 
                Dim myRemarks As Object = Nothing
                If txt_Form_Remarks.Text <> "" Then
                    myRemarks = TrimAll(Me.txt_Form_Remarks.Text)
                    myRemarks = RemoveQuotes(myRemarks)
                    If myRemarks.ToString.Length > 250 Then
                        Label6.Text = " Input is not Valid... "
                        Me.txt_Form_Remarks.Focus()
                        Exit Sub
                    End If
                    myRemarks = TrimAll(myRemarks)
                    'check unwanted characters
                    c = 0
                    counter7 = 0
                    For iloop = 1 To myRemarks.ToString.Length
                        strcurrentchar = Mid(myRemarks, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter7 = 1
                            End If
                        End If
                    Next
                    If counter7 = 1 Then
                        Label6.Text = " Input is not Valid..."
                        Me.txt_Form_Remarks.Focus()
                        Exit Sub
                    End If
                Else
                    myRemarks = ""
                End If

                'INSERT THE RECORD IN TO THE DATABASE
                Dim SQL As String
                Dim Cmd As SqlCommand
                Dim da As SqlDataAdapter
                Dim ds As New DataSet
                Dim CB As SqlCommandBuilder
                Dim dtrow As DataRow
                SQL = "SELECT * FROM MESSAGES WHERE (MESS_ID = '00')"
                Cmd = New SqlCommand(SQL, SqlConn)
                da = New SqlDataAdapter(Cmd)
                CB = New SqlCommandBuilder(da)
                SqlConn.Open()
                da.Fill(ds, "MESSAGES")
                dtrow = ds.Tables("MESSAGES").NewRow

                If Not String.IsNullOrEmpty(Name) Then
                    dtrow("FORM_NAME") = Name.Trim
                End If

                If Not String.IsNullOrEmpty(myFileNo) Then
                    dtrow("FILE_NO") = myFileNo.Trim
                Else
                    dtrow("FILE_NO") = System.DBNull.Value
                End If
                If Not String.IsNullOrEmpty(mySubject) Then
                    dtrow("SUBJECT") = mySubject.Trim
                Else
                    dtrow("SUBJECT") = System.DBNull.Value
                End If

                If Not String.IsNullOrEmpty(myTopMsg) Then
                    dtrow("TOP_MESSAGE") = myTopMsg
                Else
                    dtrow("TOP_MESSAGE") = System.DBNull.Value
                End If

                If Not String.IsNullOrEmpty(myBottomMsg) Then
                    dtrow("BOTTOM_MESSAGE") = myBottomMsg
                Else
                    dtrow("BOTTOM_MESSAGE") = System.DBNull.Value
                End If
                If Not String.IsNullOrEmpty(mySign) Then
                    dtrow("SIGN_AUTHORITY") = mySign
                Else
                    dtrow("SIGN_AUTHORITY") = System.DBNull.Value
                End If
                If Not String.IsNullOrEmpty(myRemarks) Then
                    dtrow("REMARKS") = myRemarks
                Else
                    dtrow("REMARKS") = System.DBNull.Value
                End If

                dtrow("LIB_CODE") = LibCode
                dtrow("USER_CODE") = Session.Item("LoggedUser")
                dtrow("DATE_ADDED") = Now.Date
                dtrow("IP") = Request.UserHostAddress.Trim

                ds.Tables("MESSAGES").Rows.Add(dtrow)

                thisTransaction = SqlConn.BeginTransaction()
                da.SelectCommand.Transaction = thisTransaction
                da.Update(ds, "MESSAGES")
                thisTransaction.Commit()
                ClearFields()
                Name = Nothing
                myFileNo = Nothing
                mySubject = Nothing
                myBottomMsg = Nothing
                myTopMsg = Nothing
                myRemarks = Nothing
                mySign = Nothing

                ds.Dispose()
                Label6.Text = "Record Added Successfully!"
                bttn_Save.Visible = True
                bttn_Update.Visible = False
                ClearFields()
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
        txt_Form_Name.Text = ""
        txt_Form_FileNo.Text = ""
        txt_Form_Subject.Text = ""
        txt_Form_TopMsg.Text = ""
        txt_Form_BottomMsg.Text = ""
        txt_Form_Sign.Text = ""
        txt_Form_Remarks.Text = ""
    End Sub
    ''Populate the users in grid     'search users
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
                myfield = "FORM_NAME"
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
                OrderBy = "FORM_NAME"
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
            SQL = "SELECT * FROM MESSAGES WHERE (LIB_CODE = '" & Trim(LibCode) & "')  "

            If txt_Search.Text <> "" Then
                If myBoolean = "LIKE" Then
                    SQL = SQL & " AND  (" & myfield & " LIKE N'%" & Trim(mySearchString) & "%') "
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

            SQL = SQL & " ORDER BY " & OrderBy & " " & SortBy

            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            da.Fill(ds)

            dt = ds.Tables(0).Copy
            Dim RecordCount As Long = 0
            If dt.Rows.Count = 0 Then
                Me.Grid_Letters.DataSource = Nothing
                Grid_Letters.DataBind()
                Label1.Text = "Total Record(s): 0 "
                Delete_Bttn.Enabled = False
            Else
                Grid_Letters.Visible = True
                RecordCount = dt.Rows.Count
                Grid_Letters.DataSource = dt
                Grid_Letters.DataBind()
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
    Protected Sub Grid1_Letters_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles Grid_Letters.PageIndexChanging
        Try
            'rebind datagrid
            Grid_Letters.DataSource = ViewState("dt") 'temp
            Grid_Letters.PageIndex = e.NewPageIndex
            index = e.NewPageIndex * Grid_Letters.PageSize
            Grid_Letters.DataBind()
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
    Protected Sub Grid_Letters_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles Grid_Letters.Sorting
        Dim temp As DataTable = CType(ViewState("dt"), DataTable)
        GridViewSortExpression = e.SortExpression
        Grid_Letters.DataSource = temp
        Dim pageIndex As Integer = Grid_Letters.PageIndex
        Grid_Letters.DataSource = SortDataTable(Grid_Letters.DataSource, False)
        Grid_Letters.DataBind()
        Grid_Letters.PageIndex = pageIndex
        UpdatePanel1.Update()
    End Sub
    Protected Sub Grid_Letters_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Grid_Letters.RowDataBound
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
    Private Sub Grid_Letters_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles Grid_Letters.RowCommand
        Dim Command As SqlCommand = Nothing
        Dim dr As SqlDataReader = Nothing
        Try
            If e.CommandName = "Select" Then
                Dim myRowID, MesID As Integer
                myRowID = e.CommandArgument.ToString()

                If Grid_Letters.Rows(myRowID).Cells(5).Text <> "" Then
                    MesID = Grid_Letters.Rows(myRowID).Cells(5).Text
                    Label7.Text = MesID
                Else
                    MesID = ""
                    Label7.Text = ""
                End If

                Dim iloop As Integer
                Dim strcurrentchar As Object
                Dim c As Integer
                Dim counter1 As Integer

                MesID = TrimX(MesID)
                If Not String.IsNullOrEmpty(MesID) Then
                    MesID = RemoveQuotes(MesID)
                    If Len(MesID).ToString > 10 Then
                        Label6.Text = "Length of Input is not Proper... "
                        Exit Sub
                    End If
                    MesID = " " & MesID & " "
                    If InStr(1, MesID, " CREATE ", 1) > 0 Or InStr(1, MesID, " DELETE ", 1) > 0 Or InStr(1, MesID, " DROP ", 1) > 0 Or InStr(1, MesID, " INSERT ", 1) > 1 Or InStr(1, MesID, " TRACK ", 1) > 1 Or InStr(1, MesID, " TRACE ", 1) > 1 Then
                        Label6.Text = "Do not use reserve words..."
                        Exit Sub
                    End If
                    MesID = TrimX(MesID)
                Else
                    Label6.Text = "Select Record... "
                    Exit Sub
                End If

                'get record details from database
                Dim SQL As String = Nothing
                SQL = " SELECT *  FROM MESSAGES WHERE (MESS_ID = '" & Trim(MesID) & "') "
                Command = New SqlCommand(SQL, SqlConn)
                SqlConn.Open()
                dr = Command.ExecuteReader(CommandBehavior.CloseConnection)
                dr.Read()
                ClearFields()
                If dr.HasRows = True Then
                    If dr.Item("FILE_NO").ToString <> "" Then
                        Me.txt_Form_FileNo.Text = dr.Item("FILE_NO").ToString
                    Else
                        txt_Form_FileNo.Text = ""
                    End If
                    If dr.Item("FORM_NAME").ToString <> "" Then
                        Me.txt_Form_Name.Text = dr.Item("FORM_NAME").ToString
                    Else
                        txt_Form_Name.Text = ""
                    End If
                    If dr.Item("SUBJECT").ToString <> "" Then
                        Me.txt_Form_Subject.Text = dr.Item("SUBJECT").ToString
                    Else
                        Me.txt_Form_Subject.Text = ""
                    End If

                    If dr.Item("TOP_MESSAGE").ToString <> "" Then
                        Me.txt_Form_TopMsg.Text = dr.Item("TOP_MESSAGE").ToString
                    Else
                        Me.txt_Form_TopMsg.Text = ""
                    End If
                    If dr.Item("BOTTOM_MESSAGE").ToString <> "" Then
                        txt_Form_BottomMsg.Text = dr.Item("BOTTOM_MESSAGE")
                    Else
                        Me.txt_Form_BottomMsg.Text = ""
                    End If
                    If dr.Item("SIGN_AUTHORITY").ToString <> "" Then
                        txt_Form_Sign.Text = dr.Item("SIGN_AUTHORITY")
                    Else
                        Me.txt_Form_Sign.Text = ""
                    End If
                    If dr.Item("REMARKS").ToString <> "" Then
                        txt_Form_Remarks.Text = dr.Item("REMARKS")
                    Else
                        Me.txt_Form_Remarks.Text = ""
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
                Dim counter1, counter2, counter3, counter4, counter5, counter6, counter7 As Integer

                '****************************************************************************************
                'Server Validation for form name
                Dim Name As String = Nothing
                If txt_Form_Name.Text <> "" Then
                    Name = TrimAll(UCase(txt_Form_Name.Text))
                    Name = RemoveQuotes(Name)
                    If Name.Length > 50 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        txt_Form_Name.Focus()
                        Exit Sub
                    End If
                    Name = " " & Name & " "
                    If InStr(1, Name, "CREATE", 1) > 0 Or InStr(1, Name, "DELETE", 1) > 0 Or InStr(1, Name, "DROP", 1) > 0 Or InStr(1, Name, "INSERT", 1) > 1 Or InStr(1, Name, "TRACK", 1) > 1 Or InStr(1, Name, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Me.txt_Form_Name.Focus()
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
                        Me.txt_Form_Name.Focus()
                        Exit Sub
                    End If
                    'Check Duplicate comm Code
                    Dim str As Object = Nothing
                    Dim flag As Object = Nothing
                    str = "SELECT MESS_ID FROM MESSAGES WHERE (LIB_CODE = '" & Trim(LibCode) & "' AND MESS_ID<>'" & Trim(Label7.Text) & "' AND FORM_NAME ='" & Trim(Name) & "')"
                    Dim cmd1 As New SqlCommand(str, SqlConn)
                    SqlConn.Open()
                    flag = cmd1.ExecuteScalar
                    If flag <> Nothing Then
                        Label6.Text = "Form Already exists..Enter another form"
                        Me.txt_Form_Name.Focus()
                        Exit Sub
                    End If
                    SqlConn.Close()
                Else
                    Label6.Text = " Plz Enter Form Name! "
                    Me.txt_Form_Name.Focus()
                    Exit Sub
                End If

                '****************************************************************************************88
                'File No
                Dim myFileNo As String = Nothing
                If txt_Form_FileNo.Text <> "" Then
                    myFileNo = TrimAll(Me.txt_Form_FileNo.Text)
                    myFileNo = RemoveQuotes(myFileNo)
                    If myFileNo.Length > 200 Then
                        Label6.Text = " Input is not Valid... "
                        Me.txt_Form_FileNo.Focus()
                        Exit Sub
                    End If
                    myFileNo = " " & myFileNo & " "
                    If InStr(1, myFileNo, "CREATE", 1) > 0 Or InStr(1, myFileNo, "DELETE", 1) > 0 Or InStr(1, myFileNo, "DROP", 1) > 0 Or InStr(1, myFileNo, "INSERT", 1) > 1 Or InStr(1, myFileNo, "TRACK", 1) > 1 Or InStr(1, myFileNo, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Me.txt_Form_FileNo.Focus()
                        Exit Sub
                    End If
                    myFileNo = TrimX(myFileNo)
                    'check unwanted characters
                    c = 0
                    counter2 = 0
                    For iloop = 1 To Len(myFileNo)
                        strcurrentchar = Mid(myFileNo, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter2 = 1
                            End If
                        End If
                    Next
                    If counter2 = 1 Then
                        Label6.Text = " Input is not Valid..."
                        Me.txt_Form_FileNo.Focus()
                        Exit Sub
                    End If
                Else
                    myFileNo = String.Empty
                End If

                '****************************************************************************************88
                'letter subject
                Dim mySubject As String = Nothing
                If txt_Form_Subject.Text <> "" Then
                    mySubject = TrimAll(Me.txt_Form_Subject.Text)
                    mySubject = RemoveQuotes(mySubject)
                    If mySubject.Length > 250 Then
                        Label6.Text = " Input is not Valid... "
                        Me.txt_Form_Subject.Focus()
                        Exit Sub
                    End If
                    mySubject = " " & mySubject & " "
                    If InStr(1, mySubject, "CREATE", 1) > 0 Or InStr(1, mySubject, "DELETE", 1) > 0 Or InStr(1, mySubject, "DROP", 1) > 0 Or InStr(1, mySubject, "INSERT", 1) > 1 Or InStr(1, mySubject, "TRACK", 1) > 1 Or InStr(1, mySubject, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Me.txt_Form_Subject.Focus()
                        Exit Sub
                    End If
                    mySubject = TrimAll(mySubject)
                    'check unwanted characters
                    c = 0
                    counter3 = 0
                    For iloop = 1 To Len(mySubject)
                        strcurrentchar = Mid(mySubject, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter3 = 1
                            End If
                        End If
                    Next
                    If counter3 = 1 Then
                        Label6.Text = " Input is not Valid..."
                        Me.txt_Form_Subject.Focus()
                        Exit Sub
                    End If
                Else
                    Label6.Text = " Plz Enter Letter Subect! "
                    Me.txt_Form_Subject.Focus()
                    Exit Sub
                End If

                '****************************************************************************************88
                'top message
                Dim myTopMsg As Object = Nothing
                If txt_Form_TopMsg.Text <> "" Then
                    myTopMsg = TrimAll(Me.txt_Form_TopMsg.Text)
                    myTopMsg = RemoveQuotes(myTopMsg)
                    If myTopMsg.ToString.Length > 2000 Then
                        Label6.Text = " Input is not Valid... "
                        Me.txt_Form_TopMsg.Focus()
                        Exit Sub
                    End If
                    myTopMsg = TrimAll(myTopMsg)
                    'check unwanted characters
                    c = 0
                    counter4 = 0
                    For iloop = 1 To myTopMsg.ToString.Length
                        strcurrentchar = Mid(myTopMsg, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter4 = 1
                            End If
                        End If
                    Next
                    If counter4 = 1 Then
                        Label6.Text = " Input is not Valid..."
                        Me.txt_Form_TopMsg.Focus()
                        Exit Sub
                    End If
                Else
                    Label6.Text = " Plz Enter Top Message of the Letter! "
                    Me.txt_Form_TopMsg.Focus()
                    Exit Sub
                End If

                '****************************************************************************************88
                'Bottom message
                Dim myBottomMsg As Object = Nothing
                If txt_Form_TopMsg.Text <> "" Then
                    myBottomMsg = TrimAll(Me.txt_Form_BottomMsg.Text)
                    myBottomMsg = RemoveQuotes(myBottomMsg)
                    If myBottomMsg.ToString.Length > 2000 Then
                        Label6.Text = " Input is not Valid... "
                        Me.txt_Form_BottomMsg.Focus()
                        Exit Sub
                    End If
                    myBottomMsg = TrimAll(myBottomMsg)
                    'check unwanted characters
                    c = 0
                    counter5 = 0
                    For iloop = 1 To myBottomMsg.ToString.Length
                        strcurrentchar = Mid(myBottomMsg, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter5 = 1
                            End If
                        End If
                    Next
                    If counter5 = 1 Then
                        Label6.Text = " Input is not Valid..."
                        Me.txt_Form_BottomMsg.Focus()
                        Exit Sub
                    End If
                Else
                    Label6.Text = " Plz Enter bottom Message of the Letter! "
                    Me.txt_Form_BottomMsg.Focus()
                    Exit Sub
                End If

                '****************************************************************************************88
                'sign authority
                Dim mySign As Object = Nothing
                If txt_Form_Sign.Text <> "" Then
                    mySign = TrimAll(Me.txt_Form_Sign.Text)
                    mySign = RemoveQuotes(mySign)
                    If mySign.ToString.Length > 100 Then
                        Label6.Text = " Input is not Valid... "
                        Me.txt_Form_Sign.Focus()
                        Exit Sub
                    End If
                    mySign = " " & mySign & " "
                    If InStr(1, mySign, "CREATE", 1) > 0 Or InStr(1, mySign, "DELETE", 1) > 0 Or InStr(1, mySign, "DROP", 1) > 0 Or InStr(1, mySign, "INSERT", 1) > 1 Or InStr(1, mySign, "TRACK", 1) > 1 Or InStr(1, mySign, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Me.txt_Form_Sign.Focus()
                        Exit Sub
                    End If
                    mySign = TrimAll(mySign)
                    'check unwanted characters
                    c = 0
                    counter6 = 0
                    For iloop = 1 To mySign.ToString.Length
                        strcurrentchar = Mid(mySign, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter6 = 1
                            End If
                        End If
                    Next
                    If counter6 = 1 Then
                        Label6.Text = " Input is not Valid..."
                        Me.txt_Form_Sign.Focus()
                        Exit Sub
                    End If
                Else
                    Label6.Text = " Plz Enter bottom Message of the Letter! "
                    Me.txt_Form_Sign.Focus()
                    Exit Sub
                End If

                '****************************************************************************************88
                'Remarks 
                Dim myRemarks As Object = Nothing
                If txt_Form_Remarks.Text <> "" Then
                    myRemarks = TrimAll(Me.txt_Form_Remarks.Text)
                    myRemarks = RemoveQuotes(myRemarks)
                    If myRemarks.ToString.Length > 250 Then
                        Label6.Text = " Input is not Valid... "
                        Me.txt_Form_Remarks.Focus()
                        Exit Sub
                    End If
                    myRemarks = TrimAll(myRemarks)
                    'check unwanted characters
                    c = 0
                    counter7 = 0
                    For iloop = 1 To myRemarks.ToString.Length
                        strcurrentchar = Mid(myRemarks, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter7 = 1
                            End If
                        End If
                    Next
                    If counter7 = 1 Then
                        Label6.Text = " Input is not Valid..."
                        Me.txt_Form_Remarks.Focus()
                        Exit Sub
                    End If
                Else
                    myRemarks = ""
                End If


                '****************************************************************************************************
                'UPDATE THE LIBRARY PROFILE  
                If Label7.Text <> "" Then
                    SQL = "SELECT * FROM MESSAGES WHERE (MESS_ID='" & Trim(Label7.Text) & "')"
                    SqlConn.Open()
                    da = New SqlDataAdapter(SQL, SqlConn)
                    cb = New SqlCommandBuilder(da)
                    da.Fill(ds, "MESSAGES")
                    If ds.Tables("MESSAGES").Rows.Count <> 0 Then
                        If Not String.IsNullOrEmpty(Name) Then
                            ds.Tables("MESSAGES").Rows(0)("FORM_NAME") = Name.Trim
                        Else
                            ds.Tables("MESSAGES").Rows(0)("FORM_NAME") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(myFileNo) Then
                            ds.Tables("MESSAGES").Rows(0)("FILE_NO") = myFileNo.Trim
                        Else
                            ds.Tables("MESSAGES").Rows(0)("FILE_NO") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(mySubject) Then
                            ds.Tables("MESSAGES").Rows(0)("SUBJECT") = mySubject.Trim
                        Else
                            ds.Tables("MESSAGES").Rows(0)("SUBJECT") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(myTopMsg) Then
                            ds.Tables("MESSAGES").Rows(0)("TOP_MESSAGE") = myTopMsg
                        Else
                            ds.Tables("MESSAGES").Rows(0)("TOP_MESSAGE") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(myBottomMsg) Then
                            ds.Tables("MESSAGES").Rows(0)("BOTTOM_MESSAGE") = myBottomMsg
                        Else
                            ds.Tables("MESSAGES").Rows(0)("BOTTOM_MESSAGE") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(mySign) Then
                            ds.Tables("MESSAGES").Rows(0)("SIGN_AUTHORITY") = mySign
                        Else
                            ds.Tables("MESSAGES").Rows(0)("SIGN_AUTHORITY") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(myRemarks) Then
                            ds.Tables("MESSAGES").Rows(0)("REMARKS") = myRemarks
                        Else
                            ds.Tables("MESSAGES").Rows(0)("REMARKS") = System.DBNull.Value
                        End If

                        ds.Tables("MESSAGES").Rows(0)("UPDATED_BY") = UserCode.Trim
                        ds.Tables("MESSAGES").Rows(0)("DATE_MODIFIED") = Now.Date
                        ds.Tables("MESSAGES").Rows(0)("IP") = Request.UserHostAddress.Trim

                        thisTransaction = SqlConn.BeginTransaction()
                        da.SelectCommand.Transaction = thisTransaction
                        da.Update(ds, "MESSAGES")
                        thisTransaction.Commit()

                        Name = Nothing
                        myFileNo = Nothing
                        mySubject = Nothing
                        myBottomMsg = Nothing
                        myTopMsg = Nothing
                        myRemarks = Nothing
                        mySign = Nothing
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
            For i = 0 To Grid_Letters.Rows.Count - 1
                If DirectCast(Grid_Letters.Rows(i).Cells(0).FindControl("cbd"), CheckBox).Checked Then
                    Dim MessID As Integer = Nothing
                    MessID = Grid_Letters.Rows(i).Cells(5).Text
                    'get cat record
                    Dim SQL As String = Nothing
                    SQL = "DELETE FROM MESSAGES WHERE (MESS_ID ='" & Trim(MessID) & "') "
                    SqlConn.Open()
                    Dim objCommand As New SqlCommand(SQL, SqlConn)
                    Dim da As New SqlDataAdapter(objCommand)
                    Dim ds As New DataSet
                    da.Fill(ds)
                    SqlConn.Close()
                End If
            Next
            Search_Bttn_Click(sender, e)
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
End Class