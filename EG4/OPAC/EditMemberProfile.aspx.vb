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
Imports EG4.PopulateCombo
Imports EG4.CaptchaImage
Imports CDO
Imports System.Net.Mail
Public Class EditMemberProfile
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session.Item("IsThisU") <> Nothing Then
                If Not SConnection() = True Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Database Connection is Lost mm!');", True)
                Else
                    txt_Mem_MemID.Enabled = False
                    If Session.Item("LoggedMemID") <> 0 Then
                        txt_Mem_MemID.Text = Session.Item("LoggedMemID")
                    Else
                        txt_Mem_MemID.Text = ""
                    End If

                    If Page.IsPostBack = False Then
                        PopulateSubjects()
                        GetMemberDetails()
                    End If
                    Me.txt_Mem_Name.Focus()
                End If
            Else
                Response.Redirect("~/OPAC/Default.aspx", False)
            End If
        Catch ex As Exception
            Lbl_Error.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    Private Sub EditUserProfile_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Me.txt_Mem_Name.Focus()
    End Sub
    Public Sub GetMemberDetails()
        Dim Command As SqlCommand = Nothing
        Dim drUSERS As SqlDataReader = Nothing
        Try
            Dim myEdMemID As Integer = Nothing
            myEdMemID = Session.Item("LoggedMemID")

            Dim iloop As Integer
            Dim strcurrentchar As Object
            Dim c As Integer
            Dim counter1 As Integer

            myEdMemID = TrimX(myEdMemID)

            If myEdMemID <> 0 Then
                myEdMemID = RemoveQuotes(myEdMemID)
                If myEdMemID.ToString.Length > 12 Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert(' Length of Member ID is not Proper... ');", True)
                    Me.txt_Mem_Name.Focus()
                    Exit Sub
                End If

                If IsNumeric(myEdMemID) = False Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert(' Type of Member ID is not Proper... ');", True)
                    Me.txt_Mem_Name.Focus()
                    Exit Sub
                End If


                myEdMemID = " " & myEdMemID & " "
                If InStr(1, myEdMemID, " CREATE ", 1) > 0 Or InStr(1, myEdMemID, " DELETE ", 1) > 0 Or InStr(1, myEdMemID, " DROP ", 1) > 0 Or InStr(1, myEdMemID, " INSERT ", 1) > 1 Or InStr(1, myEdMemID, " TRACK ", 1) > 1 Or InStr(1, myEdMemID, " TRACE ", 1) > 1 Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert(' Do not use reserve words... ');", True)
                    Me.txt_Mem_Name.Focus()
                    Exit Sub
                End If
                myEdMemID = TrimX(myEdMemID)
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert(' Enter User Code... ');", True)
                Me.txt_Mem_MemID.Focus()
                Exit Sub
            End If
            'check unwanted characters
            c = 0
            counter1 = 0
            For iloop = 1 To Len(myEdMemID.ToString)
                strcurrentchar = Mid(myEdMemID, iloop, 1)
                If c = 0 Then
                    If Not InStr("';~!@#$^&*-_=+|[]{}?,<>=()%""", strcurrentchar) <= 0 Then
                        c = c + 1
                        counter1 = 1
                    End If
                End If
            Next
            If counter1 = 1 Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert(' Do Not Use Un-wanted Characters... ');", True)
                Me.txt_Mem_MemID.Focus()
                Exit Sub
            End If

            'get record details from database
            Dim SQL As String = Nothing
            SQL = " SELECT *  FROM MEMBERSHIPS WHERE (MEM_ID = '" & TrimX(myEdMemID) & "'); "
            Command = New SqlCommand(SQL, SqlConn)
            SqlConn.Open()
            drUSERS = Command.ExecuteReader(CommandBehavior.CloseConnection)
            drUSERS.Read()
            If drUSERS.HasRows = True Then
                If drUSERS.Item("MEM_NAME").ToString <> "" Then
                    txt_Mem_Name.Text = drUSERS.Item("MEM_NAME").ToString
                Else
                    txt_Mem_Name.Text = ""
                End If
                If drUSERS.Item("MEM_DOB").ToString <> "" Then
                    txt_Mem_DoB.Text = Format(drUSERS.Item("MEM_DOB"), "dd/MM/yyyy")
                Else
                    txt_Mem_DoB.Text = ""
                End If
                If drUSERS.Item("MEM_TELEPHONE").ToString <> "" Then
                    txt_Mem_Phone.Text = drUSERS.Item("MEM_TELEPHONE").ToString
                Else
                    txt_Mem_Phone.Text = ""
                End If
                If drUSERS.Item("MEM_MOBILE").ToString <> "" Then
                    txt_Mem_Mobile.Text = drUSERS.Item("MEM_MOBILE").ToString
                Else
                    txt_Mem_Mobile.Text = ""
                End If
                If drUSERS.Item("MEM_EMAIL").ToString <> "" Then
                    txt_Mem_Email.Text = drUSERS.Item("MEM_EMAIL").ToString
                Else
                    txt_Mem_Email.Text = ""
                End If

                If drUSERS.Item("MEM_Q").ToString <> "" Then
                    DropDownList1.SelectedValue = drUSERS.Item("MEM_Q").ToString
                Else
                    DropDownList1.Text = ""
                End If

                If drUSERS.Item("MEM_ANS").ToString <> "" Then
                    txt_Mem_Ans.Text = drUSERS.Item("MEM_ANS").ToString
                Else
                    txt_Mem_Ans.Text = ""
                End If

                If drUSERS.Item("SUB_ID").ToString <> "" Then
                    DDL_Subjects.SelectedValue = drUSERS.Item("SUB_ID").ToString
                Else
                    DDL_Subjects.ClearSelection()
                End If

                If drUSERS.Item("KEYWORDS").ToString <> "" Then
                    txt_Mem_Keywords.Text = drUSERS.Item("KEYWORDS").ToString
                Else
                    txt_Mem_Keywords.Text = ""
                End If

                If drUSERS.Item("MEM_RES_ADD").ToString <> "" Then
                    txt_Mem_ResAdd.Text = drUSERS.Item("MEM_RES_ADD").ToString
                Else
                    txt_Mem_ResAdd.Text = ""
                End If

                If drUSERS.Item("MEM_REMARKS").ToString <> "" Then
                    txt_Mem_Remarks.Text = drUSERS.Item("MEM_REMARKS").ToString
                Else
                    txt_Mem_Remarks.Text = ""
                End If

                If drUSERS.Item("PHOTO").ToString <> "" Then
                    Dim strURL As String = "Mem_GetPhoto.aspx?MEM_ID=" & txt_Mem_MemID.Text & ""
                    Image21.ImageUrl = strURL
                Else
                    Image21.Visible = False
                End If

            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert(' No Record to Edit... ');", True)
                Exit Sub
            End If
              
        Catch s As Exception
            Lbl_Error.Text = "Error: " & (s.Message())
        Finally
            Command.Dispose()
            drUSERS.Close()
            SqlConn.Close()
        End Try
    End Sub
    Public Sub ClearFields()
        Me.txt_Mem_ResAdd.Text = ""
        Me.txt_Mem_Email.Text = ""
        Me.txt_Mem_Mobile.Text = ""
        txt_Mem_Phone.Text = ""
        txt_Mem_Name.Text = ""
        txt_Mem_DoB.Text = ""
        txt_Mem_Remarks.Text = ""
        DropDownList1.ClearSelection()
        txt_Mem_Ans.Text = ""
        Image21.ImageUrl = Nothing
        DDL_Subjects.ClearSelection()
        txt_Mem_Keywords.Text = ""
    End Sub
    Public Sub PopulateSubjects()
        Me.DDL_Subjects.DataSource = GetSubjectList()
        Me.DDL_Subjects.DataTextField = "SUB_NAME"
        Me.DDL_Subjects.DataValueField = "SUB_ID"
        Me.DDL_Subjects.DataBind()
        DDL_Subjects.Items.Insert(0, "")
    End Sub
    Protected Sub Cancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Cancel.Click
        ClearFields()
    End Sub
    'update Record
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
                Dim counter1, counter2, counter3, counter4, counter5, counter6, counter7, counter8, counter9, counter10 As Integer
                '****************************************************************************************************
                'validation for cat ID
                Dim MEM_ID As Integer = Nothing
                If txt_Mem_MemID.Text <> "" Then
                    MEM_ID = TrimX(txt_Mem_MemID.Text)
                    MEM_ID = RemoveQuotes(MEM_ID)

                    If Len(MEM_ID).ToString > 10 Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert(' Length of Input is not Proper... ');", True)
                        Exit Sub
                    End If
                    If Not IsNumeric(MEM_ID) = True Then
                        Exit Sub
                    End If

                    MEM_ID = " " & MEM_ID & " "
                    If InStr(1, MEM_ID, " CREATE ", 1) > 0 Or InStr(1, MEM_ID, " DELETE ", 1) > 0 Or InStr(1, MEM_ID, " DROP ", 1) > 0 Or InStr(1, MEM_ID, " INSERT ", 1) > 1 Or InStr(1, MEM_ID, " TRACK ", 1) > 1 Or InStr(1, MEM_ID, " TRACE ", 1) > 1 Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert(' Do not use reserve words... ');", True)
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
                        Exit Sub
                    End If
                Else
                    Exit Sub
                End If

                'validation 
                Dim MEM_NAME As Object = Nothing
                If Me.txt_Mem_Name.Text <> "" Then
                    MEM_NAME = TrimAll(txt_Mem_Name.Text)
                    MEM_NAME = RemoveQuotes(MEM_NAME)
                    If MEM_NAME.Length > 150 Then
                        txt_Mem_Name.Focus()
                        Exit Sub
                    End If
                    MEM_NAME = " " & MEM_NAME & " "
                    If InStr(1, MEM_NAME, "CREATE", 1) > 0 Or InStr(1, MEM_NAME, "DELETE", 1) > 0 Or InStr(1, MEM_NAME, "DROP", 1) > 0 Or InStr(1, MEM_NAME, "INSERT", 1) > 1 Or InStr(1, MEM_NAME, "TRACK", 1) > 1 Or InStr(1, MEM_NAME, "TRACE", 1) > 1 Then
                        txt_Mem_Name.Focus()
                        Exit Sub
                    End If
                    MEM_NAME = TrimAll(MEM_NAME)
                Else
                    txt_Mem_Name.Focus()
                    Exit Sub
                End If

                'validation 
                Dim MEM_RES_ADD As Object = Nothing
                If Me.txt_Mem_ResAdd.Text <> "" Then
                    MEM_RES_ADD = TrimAll(txt_Mem_ResAdd.Text)
                    MEM_RES_ADD = RemoveQuotes(MEM_RES_ADD)
                    If MEM_RES_ADD.Length > 250 Then
                        txt_Mem_ResAdd.Focus()
                        Exit Sub
                    End If
                    MEM_RES_ADD = " " & MEM_RES_ADD & " "
                    If InStr(1, MEM_RES_ADD, "CREATE", 1) > 0 Or InStr(1, MEM_RES_ADD, "DELETE", 1) > 0 Or InStr(1, MEM_RES_ADD, "DROP", 1) > 0 Or InStr(1, MEM_RES_ADD, "INSERT", 1) > 1 Or InStr(1, MEM_RES_ADD, "TRACK", 1) > 1 Or InStr(1, MEM_RES_ADD, "TRACE", 1) > 1 Then
                        txt_Mem_ResAdd.Focus()
                        Exit Sub
                    End If
                    MEM_RES_ADD = TrimAll(MEM_RES_ADD)
                Else
                    MEM_RES_ADD = Nothing
                End If

                'validation 
                Dim MEM_EMAIL As Object = Nothing
                If Me.txt_Mem_Email.Text <> "" Then
                    MEM_EMAIL = Trim(txt_Mem_Email.Text)
                    MEM_EMAIL = RemoveQuotes(MEM_EMAIL)
                    If MEM_EMAIL.Length > 100 Then
                        txt_Mem_Email.Focus()
                        Exit Sub
                    End If
                    MEM_EMAIL = " " & MEM_EMAIL & " "
                    If InStr(1, MEM_EMAIL, "CREATE", 1) > 0 Or InStr(1, MEM_EMAIL, "DELETE", 1) > 0 Or InStr(1, MEM_EMAIL, "DROP", 1) > 0 Or InStr(1, MEM_EMAIL, "INSERT", 1) > 1 Or InStr(1, MEM_EMAIL, "TRACK", 1) > 1 Or InStr(1, MEM_EMAIL, "TRACE", 1) > 1 Then
                        txt_Mem_Email.Focus()
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
                        txt_Mem_Phone.Focus()
                        Exit Sub
                    End If
                    MEM_TELEPHONE = " " & MEM_TELEPHONE & " "
                    If InStr(1, MEM_TELEPHONE, "CREATE", 1) > 0 Or InStr(1, MEM_TELEPHONE, "DELETE", 1) > 0 Or InStr(1, MEM_TELEPHONE, "DROP", 1) > 0 Or InStr(1, MEM_TELEPHONE, "INSERT", 1) > 1 Or InStr(1, MEM_TELEPHONE, "TRACK", 1) > 1 Or InStr(1, MEM_TELEPHONE, "TRACE", 1) > 1 Then
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
                        txt_Mem_Mobile.Focus()
                        Exit Sub
                    End If
                    MEM_MOBILE = " " & MEM_MOBILE & " "
                    If InStr(1, MEM_MOBILE, "CREATE", 1) > 0 Or InStr(1, MEM_MOBILE, "DELETE", 1) > 0 Or InStr(1, MEM_MOBILE, "DROP", 1) > 0 Or InStr(1, MEM_MOBILE, "INSERT", 1) > 1 Or InStr(1, MEM_MOBILE, "TRACK", 1) > 1 Or InStr(1, MEM_MOBILE, "TRACE", 1) > 1 Then
                        txt_Mem_Mobile.Focus()
                        Exit Sub
                    End If
                    MEM_MOBILE = TrimX(MEM_MOBILE)
                Else
                    MEM_MOBILE = Nothing
                End If

                'validation 
                Dim MEM_REMARKS As Object = Nothing
                If Me.txt_Mem_Remarks.Text <> "" Then
                    MEM_REMARKS = Trim(txt_Mem_Remarks.Text)
                    MEM_REMARKS = RemoveQuotes(MEM_REMARKS)
                    If MEM_REMARKS.Length > 50 Then
                        txt_Mem_Remarks.Focus()
                        Exit Sub
                    End If
                    MEM_REMARKS = " " & MEM_REMARKS & " "
                    If InStr(1, MEM_REMARKS, "CREATE", 1) > 0 Or InStr(1, MEM_REMARKS, "DELETE", 1) > 0 Or InStr(1, MEM_REMARKS, "DROP", 1) > 0 Or InStr(1, MEM_REMARKS, "INSERT", 1) > 1 Or InStr(1, MEM_REMARKS, "TRACK", 1) > 1 Or InStr(1, MEM_REMARKS, "TRACE", 1) > 1 Then
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
                        DDL_Subjects.Focus()
                        Exit Sub
                    End If

                    If IsNumeric(SUB_ID) = False Then
                        DDL_Subjects.Focus()
                        Exit Sub
                    End If

                    SUB_ID = " " & SUB_ID & " "
                    If InStr(1, SUB_ID, "CREATE", 1) > 0 Or InStr(1, SUB_ID, "DELETE", 1) > 0 Or InStr(1, SUB_ID, "DROP", 1) > 0 Or InStr(1, SUB_ID, "INSERT", 1) > 1 Or InStr(1, SUB_ID, "TRACK", 1) > 1 Or InStr(1, SUB_ID, "TRACE", 1) > 1 Then
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
                        txt_Mem_Keywords.Focus()
                        Exit Sub
                    End If
                    KEYWORDS = " " & KEYWORDS & " "
                    If InStr(1, KEYWORDS, "CREATE", 1) > 0 Or InStr(1, KEYWORDS, "DELETE", 1) > 0 Or InStr(1, KEYWORDS, "DROP", 1) > 0 Or InStr(1, KEYWORDS, "INSERT", 1) > 1 Or InStr(1, KEYWORDS, "TRACK", 1) > 1 Or InStr(1, KEYWORDS, "TRACE", 1) > 1 Then
                        txt_Mem_Keywords.Focus()
                        Exit Sub
                    End If
                    KEYWORDS = TrimAll(KEYWORDS)
                Else
                    KEYWORDS = Nothing
                End If


                'validation 
                Dim MEM_DOB As Object = Nothing
                If Me.txt_Mem_DoB.Text <> "" Then
                    MEM_DOB = Trim(txt_Mem_DoB.Text)
                    MEM_DOB = RemoveQuotes(MEM_DOB)
                    MEM_DOB = Convert.ToDateTime(MEM_DOB, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy")
                    If MEM_DOB.Length > 12 Then
                        txt_Mem_DoB.Focus()
                        Exit Sub
                    End If
                    MEM_DOB = " " & MEM_DOB & " "
                    If InStr(1, MEM_DOB, "CREATE", 1) > 0 Or InStr(1, MEM_DOB, "DELETE", 1) > 0 Or InStr(1, MEM_DOB, "DROP", 1) > 0 Or InStr(1, MEM_DOB, "INSERT", 1) > 1 Or InStr(1, MEM_DOB, "TRACK", 1) > 1 Or InStr(1, MEM_DOB, "TRACE", 1) > 1 Then
                        txt_Mem_DoB.Focus()
                        Exit Sub
                    End If
                    MEM_DOB = TrimX(MEM_DOB)
                Else
                    MEM_DOB = Nothing
                End If

                'upload content file
                Dim arrContent1 As Byte()
                Dim FileType As String = Nothing
                Dim FileExtension As String = Nothing
                Dim intLength1 As Integer = 0
                If FileUpload13.FileName = "" Then
                    '    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Select Proper file... ');", True)
                    '    Me.FileUpload12.Focus()
                    '    Exit Sub
                Else
                    Dim ContfileName As String = FileUpload13.PostedFile.FileName
                    FileExtension = ContfileName.Substring(ContfileName.LastIndexOf("."))
                    FileExtension = FileExtension.ToLower
                    FileType = FileUpload13.PostedFile.ContentType

                    intLength1 = Convert.ToInt32(FileUpload13.PostedFile.InputStream.Length)
                    ReDim arrContent1(intLength1)
                    FileUpload13.PostedFile.InputStream.Read(arrContent1, 0, intLength1)

                    If intLength1 > 25000 Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Image Size musbe be below 25 KB');", True)
                        Exit Sub
                    End If
                    'Image1.ImageUrl = FileUpload1.PostedFile.FileName '"~/Images/1.png"
                End If

                Dim DATE_MODIFIED As Object = Nothing
                DATE_MODIFIED = Now.Date
                DATE_MODIFIED = Convert.ToDateTime(DATE_MODIFIED, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                Dim IP As Object = Nothing
                IP = Request.UserHostAddress.Trim

                'UPDATE THE PROFILE  
                If txt_Mem_MemID.Text <> "" Then
                    SQL = "SELECT * FROM MEMBERSHIPS WHERE (MEM_ID='" & Trim(MEM_ID) & "')"
                    SqlConn.Open()
                    da = New SqlDataAdapter(SQL, SqlConn)
                    cb = New SqlCommandBuilder(da)
                    da.Fill(ds, "MEM")
                    If ds.Tables("MEM").Rows.Count <> 0 Then
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

                        If Not String.IsNullOrEmpty(MEM_DOB) Then
                            ds.Tables("MEM").Rows(0)("MEM_DOB") = MEM_DOB.Trim
                        Else
                            ds.Tables("MEM").Rows(0)("MEM_DOB") = System.DBNull.Value
                        End If

                        If FileUpload13.FileName <> "" Then
                            ds.Tables("MEM").Rows(0)("PHOTO") = arrContent1
                        End If

                        ds.Tables("MEM").Rows(0)("MEMB_DATE") = DATE_MODIFIED
                        ds.Tables("MEM").Rows(0)("MEMB_IP") = IP

                        thisTransaction = SqlConn.BeginTransaction()
                        da.SelectCommand.Transaction = thisTransaction
                        da.Update(ds, "MEM")

                        thisTransaction.Commit()

                        ClearFields()
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert(' Record Updated Successfully!');", True)
                    Else
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert(' Record Updation not done  - Please Contact System Administrator');", True)
                    End If
                End If
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert(' Record Not Selected');", True)
            End If
           
            SqlConn.Close()
        Catch q As SqlException
            thisTransaction.Rollback()
            Lbl_Error.Text = "Error: in Updating Record!"
        Catch s As Exception
            Lbl_Error.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
End Class