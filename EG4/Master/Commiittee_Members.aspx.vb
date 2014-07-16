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
Imports EG4.PopulateCombo
Public Class Commiittee_Members
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
                        PopulateComiitees()
                    End If

                    Me.txt_ComMember_Name.Focus()
                End If
                Dim CreateSUserButton As Object 'System.Web.UI.WebControls.Button
                CreateSUserButton = Master.FindControl("Accordion1").FindControl("MasterPane").FindControl("M_CommitteeMembers_Bttn")
                CreateSUserButton.ForeColor = Drawing.Color.Red
                myPaneName = "MasterPane" ' paneSelectedIndex = 0
            Else
                Response.Redirect("~/Default.aspx", False)
            End If
            LibCode = Session.Item("LoggedLibcode")
        Catch ex As Exception
            Label6.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    Private Sub Commiittee_Members_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Me.txt_ComMember_Name.Focus()
    End Sub
    'populate approval no
    Public Sub PopulateComiitees()
        Dim Sel As String = "SELECT COM_CODE, COM_NAME FROM COMMITTEES WHERE (LIB_CODE = '" & Trim(LibCode) & "') ORDER BY COM_NAME"
        Dim cmd As New SqlCommand(Sel, SqlConn)
        Dim rdr As SqlDataReader = Nothing
        Try
            SqlConn.Open()
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            If rdr.HasRows = True Then
                Me.DDL_Committees.DataTextField = "COM_NAME"
                Me.DDL_Committees.DataValueField = "COM_CODE"
                Me.DDL_Committees.DataSource = rdr
                Me.DDL_Committees.DataBind()
                DDL_Committees.Items.Insert(0, "")
            Else
                Me.DDL_Committees.DataSource = Nothing
                Me.DDL_Committees.DataBind()
            End If
        Catch ex As System.Exception
            If rdr IsNot Nothing Then
                rdr.Close()
            Else
                SqlConn.Close()
            End If
            Label6.Text = ex.Message.ToString()
        End Try
      
    End Sub
    'save user account
    Protected Sub Save_Click(ByVal sender As Object, ByVal e As EventArgs) Handles bttn_Save.Click
        If IsPostBack = True Then
            'check unwanted characters
            Dim iloop As Integer
            Dim strcurrentchar As Object
            Dim c As Integer
            Dim counter1, counter2, counter3, counter4, Counter5, counter6, Counter7, counter8, counter9, counter10 As Integer
            Dim thisTransaction As SqlClient.SqlTransaction = Nothing
            Try
                'Server Validation for Full Name
                Dim Names As String = Nothing
                Names = TrimAll(txt_ComMember_Name.Text)
                If String.IsNullOrEmpty(Names) Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Enter Full Name... ');", True)
                    Label6.Text = "Please Enter Full Name !"
                    Me.txt_ComMember_Name.Focus()
                    Exit Sub
                End If
                Names = RemoveQuotes(Names)
                If Names.Length > 250 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Name must be of Proper Length.. ');", True)
                    Label6.Text = "Name must be of Proper Length !"
                    txt_ComMember_Name.Focus()
                    Exit Sub
                End If
                Names = " " & Names & " "
                If InStr(1, Names, "CREATE", 1) > 0 Or InStr(1, Names, "DELETE", 1) > 0 Or InStr(1, Names, "DROP", 1) > 0 Or InStr(1, Names, "INSERT", 1) > 1 Or InStr(1, Names, "TRACK", 1) > 1 Or InStr(1, Names, "TRACE", 1) > 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Reserve Word... ');", True)
                    Label6.Text = "Do Not Use Reserve Word !"
                    txt_ComMember_Name.Focus()
                    Exit Sub
                End If
                Names = TrimAll(Names)
                c = 0
                counter1 = 0
                For iloop = 1 To Len(Names)
                    strcurrentchar = Mid(Names, iloop, 1)
                    If c = 0 Then
                        If Not InStr("'~!@#$^&*=+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter1 = 1
                        End If
                    End If
                Next
                If counter1 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Do Not Use Un-Wated Characters... ');", True)
                    Label6.Text = "Do Not Use Un-Wated Characters !"
                    txt_ComMember_Name.Focus()
                    Exit Sub
                End If

                '****************************************************************************************
                'Server Validation for Designation
                Dim Designation As String = Nothing
                Designation = TrimAll(txt_ComMember_Desig.Text)
                If Not String.IsNullOrEmpty(Designation) Then
                    Designation = RemoveQuotes(Designation)
                    If Designation.Length > 250 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not of proper Length... ');", True)
                        Label6.Text = "Input is not of proper Length !"
                        Me.txt_ComMember_Desig.Focus()
                        Exit Sub
                    End If
                    Designation = " " & Designation & " "
                    If InStr(1, Designation, "CREATE", 1) > 0 Or InStr(1, Designation, "DELETE", 1) > 0 Or InStr(1, Designation, "DROP", 1) > 0 Or InStr(1, Designation, "INSERT", 1) > 1 Or InStr(1, Designation, "TRACK", 1) > 1 Or InStr(1, Designation, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not use Reserve Words... ');", True)
                        Label6.Text = "Do Not use Reserve Words"
                        Me.txt_ComMember_Desig.Focus()
                        Exit Sub
                    End If
                    Designation = TrimAll(Designation)
                    'check unwanted characters
                    c = 0
                    counter2 = 0
                    For iloop = 1 To Len(Designation)
                        strcurrentchar = Mid(Designation, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter2 = 1
                            End If
                        End If
                    Next
                    If counter2 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not use un-wanted Characters... ');", True)
                        Label6.Text = "Do Not use un-wanted Characters"
                        Me.txt_ComMember_Desig.Focus()
                        Exit Sub
                    End If
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Please Enter proper Designation... ');", True)
                    Label6.Text = " Please Enter proper Designation"
                    Me.txt_ComMember_Desig.Focus()
                    Exit Sub
                End If

                '****************************************************************************************
                'Server Validation for Designation
                Dim Rank As Integer = 0
                If DropDownList_Rank.Text <> "" Then
                    Rank = Me.DropDownList_Rank.SelectedValue
                    If Not String.IsNullOrEmpty(Rank) Then
                        Rank = RemoveQuotes(Rank)
                        If Len(Rank.ToString) > 2 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not of proper Length... ');", True)
                            Label6.Text = " Input is not of proper Length"
                            Me.DropDownList_Rank.Focus()
                            Exit Sub
                        End If
                        Rank = " " & Rank & " "
                        If InStr(1, Rank, "CREATE", 1) > 0 Or InStr(1, Rank, "DELETE", 1) > 0 Or InStr(1, Rank, "DROP", 1) > 0 Or InStr(1, Rank, "INSERT", 1) > 1 Or InStr(1, Rank, "TRACK", 1) > 1 Or InStr(1, Rank, "TRACE", 1) > 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not use Reserve Words... ');", True)
                            Label6.Text = "Do Not use Reserve Words"
                            Me.DropDownList_Rank.Focus()
                            Exit Sub
                        End If
                        Rank = TrimAll(Rank)
                        'check unwanted characters
                        c = 0
                        counter3 = 0
                        For iloop = 1 To Len(Rank.ToString)
                            strcurrentchar = Mid(Rank, iloop, 1)
                            If c = 0 Then
                                If Not InStr("'~!@#$^&*=+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter3 = 1
                                End If
                            End If
                        Next
                        If counter3 = 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not use un-wanted Characters... ');", True)
                            Label6.Text = "Do Not use un-wanted Characters"
                            Me.DropDownList_Rank.Focus()
                            Exit Sub
                        End If
                    Else
                        Rank = 2
                    End If
                Else
                    Rank = 2
                End If

                '********************************************************************************************************
                'Server Validation for Phone Number
                Dim Phone As String = Nothing
                Phone = TrimAll(txt_ComMember_Phone.Text)
                If Not String.IsNullOrEmpty(Phone) Then
                    Phone = RemoveQuotes(Phone)
                    If Phone.Length > 250 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Label6.Text = "Input is not Valid"
                        Me.txt_ComMember_Phone.Focus()
                        Exit Sub
                    End If
                    Phone = " " & Phone & " "
                    If InStr(1, Phone, "CREATE", 1) > 0 Or InStr(1, Phone, "DELETE", 1) > 0 Or InStr(1, Phone, "DROP", 1) > 0 Or InStr(1, Phone, "INSERT", 1) > 1 Or InStr(1, Phone, "TRACK", 1) > 1 Or InStr(1, Phone, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Label6.Text = "Input is not Valid"
                        Me.txt_ComMember_Phone.Focus()
                        Exit Sub
                    End If
                    Phone = TrimAll(Phone)

                    'check unwanted characters
                    c = 0
                    counter4 = 0
                    For iloop = 1 To Len(Phone)
                        strcurrentchar = Mid(Phone, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter4 = 1
                            End If
                        End If
                    Next
                    If counter4 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Label6.Text = "Input is not Valid"
                        Me.txt_ComMember_Phone.Focus()
                        Exit Sub
                    End If
                Else
                    Phone = String.Empty
                End If

                '*******************************************************************************************************
                'Server Validation for Mobile Number
                Dim Mobile As String = Nothing
                Mobile = TrimAll(txt_ComMember_Mobile.Text)
                If Not String.IsNullOrEmpty(Mobile) Then
                    Mobile = RemoveQuotes(Mobile)
                    If Mobile.Length > 150 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Label6.Text = "Input is not Valid"
                        Me.txt_ComMember_Mobile.Focus()
                        Exit Sub
                    End If
                    Mobile = " " & Mobile & " "
                    If InStr(1, Mobile, "CREATE", 1) > 0 Or InStr(1, Mobile, "DELETE", 1) > 0 Or InStr(1, Mobile, "DROP", 1) > 0 Or InStr(1, Mobile, "INSERT", 1) > 1 Or InStr(1, Mobile, "TRACK", 1) > 1 Or InStr(1, Mobile, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Label6.Text = "Input is not Valid"
                        Me.txt_ComMember_Mobile.Focus()
                        Exit Sub
                    End If
                    Mobile = TrimAll(Mobile)
                    'check unwanted characters
                    c = 0
                    Counter5 = 0
                    For iloop = 1 To Len(Mobile)
                        strcurrentchar = Mid(Mobile, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                Counter5 = 1
                            End If
                        End If
                    Next
                    If Counter5 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Label6.Text = "Input is not Valid"
                        Me.txt_ComMember_Mobile.Focus()
                        Exit Sub
                    End If
                Else
                    Mobile = String.Empty
                End If

                '****************************************************************************************
                'Server Validation for E-Mail Address
                Dim email As String = Nothing
                email = TrimX(txt_ComMember_Email.Text)
                If Not String.IsNullOrEmpty(email) Then
                    email = RemoveQuotes(email)
                    If email.Length > 150 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Label6.Text = "Input is not Valid"
                        Me.txt_ComMember_Email.Focus()
                        Exit Sub
                    End If
                    email = " " & email & " "
                    If InStr(1, email, "CREATE", 1) > 0 Or InStr(1, email, "DELETE", 1) > 0 Or InStr(1, email, "DROP", 1) > 0 Or InStr(1, email, "INSERT", 1) > 1 Or InStr(1, email, "TRACK", 1) > 1 Or InStr(1, email, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Label6.Text = "Input is not Valid"
                        Me.txt_ComMember_Email.Focus()
                        Exit Sub
                    End If
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                    Label6.Text = "Input is not Valid"
                    Label6.Text = "Input is not Valid"
                    Me.txt_ComMember_Email.Focus()
                    Exit Sub
                End If
                email = TrimX(email)
                'check unwanted characters
                c = 0
                counter6 = 0
                For iloop = 1 To Len(email)
                    strcurrentchar = Mid(email, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';~!#$^&*|[]{}?<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter6 = 1
                        End If
                    End If
                Next
                If counter6 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                    Label6.Text = "Input is not Valid"
                    Me.txt_ComMember_Email.Focus()
                    Exit Sub
                End If

                '****************************************************************************************88
                'qualification
                Dim myQual As String = Nothing
                myQual = TrimAll(Me.txt_ComMember_Qual.Text)
                If Not String.IsNullOrEmpty(myQual) Then
                    myQual = RemoveQuotes(myQual)
                    If myQual.Length > 1000 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not of Proper Length.. ');", True)
                        Label6.Text = "Input is not Valid"
                        Me.txt_ComMember_Qual.Focus()
                        Exit Sub
                    End If
                    myQual = " " & myQual & " "
                    If InStr(1, myQual, "CREATE", 1) > 0 Or InStr(1, myQual, "DELETE", 1) > 0 Or InStr(1, myQual, "DROP", 1) > 0 Or InStr(1, myQual, "INSERT", 1) > 1 Or InStr(1, myQual, "TRACK", 1) > 1 Or InStr(1, myQual, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Do Not Use Reserve Words... ');", True)
                        Label6.Text = "Input is not Valid"
                        Me.txt_ComMember_Qual.Focus()
                        Exit Sub
                    End If
                    myQual = TrimAll(myQual)
                    'check unwanted characters
                    c = 0
                    Counter7 = 0
                    For iloop = 1 To Len(myQual)
                        strcurrentchar = Mid(myQual, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                Counter7 = 1
                            End If
                        End If
                    Next
                    If Counter7 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Un-wanted Characters... ');", True)
                        Label6.Text = "Input is not Valid"
                        Me.txt_ComMember_Qual.Focus()
                        Exit Sub
                    End If
                Else
                    myQual = String.Empty
                End If

                '****************************************************************************************88
                'Responsibilities
                Dim myResp As String = Nothing
                myResp = TrimAll(Me.txt_ComMember_Resp.Text)
                If Not String.IsNullOrEmpty(myResp) Then
                    myResp = RemoveQuotes(myResp)
                    If myQual.Length > 1000 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not of Proper Length.. ');", True)
                        Label6.Text = "Input is not Valid"
                        Me.txt_ComMember_Resp.Focus()
                        Exit Sub
                    End If
                    myResp = " " & myResp & " "
                    If InStr(1, myResp, "CREATE", 1) > 0 Or InStr(1, myResp, "DELETE", 1) > 0 Or InStr(1, myResp, "DROP", 1) > 0 Or InStr(1, myResp, "INSERT", 1) > 1 Or InStr(1, myResp, "TRACK", 1) > 1 Or InStr(1, myResp, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Do Not Use Reserve Words... ');", True)
                        Label6.Text = "Input is not Valid"
                        Me.txt_ComMember_Resp.Focus()
                        Exit Sub
                    End If
                    myResp = TrimAll(myResp)
                    'check unwanted characters
                    c = 0
                    counter8 = 0
                    For iloop = 1 To Len(myResp)
                        strcurrentchar = Mid(myResp, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter8 = 1
                            End If
                        End If
                    Next
                    If counter8 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Un-wanted Characters... ');", True)
                        Label6.Text = "Input is not Valid"
                        Me.txt_ComMember_Resp.Focus()
                        Exit Sub
                    End If
                Else
                    myResp = String.Empty
                End If

                '****************************************************************************************88
                'remareks
                Dim myRemarks As String = Nothing
                myRemarks = TrimAll(Me.txt_ComMember_Remarks.Text)
                If Not String.IsNullOrEmpty(myRemarks) Then
                    myRemarks = RemoveQuotes(myRemarks)
                    If myRemarks.Length > 250 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Label6.Text = "Input is not Valid"
                        Me.txt_ComMember_Remarks.Focus()
                        Exit Sub
                    End If
                    myRemarks = " " & myRemarks & " "
                    If InStr(1, myRemarks, "CREATE", 1) > 0 Or InStr(1, myRemarks, "DELETE", 1) > 0 Or InStr(1, myRemarks, "DROP", 1) > 0 Or InStr(1, myRemarks, "INSERT", 1) > 1 Or InStr(1, myRemarks, "TRACK", 1) > 1 Or InStr(1, myRemarks, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Label6.Text = "Input is not Valid"
                        Me.txt_ComMember_Remarks.Focus()
                        Exit Sub
                    End If
                    myRemarks = TrimAll(myRemarks)
                    'check unwanted characters
                    c = 0
                    counter9 = 0
                    For iloop = 1 To Len(myRemarks)
                        strcurrentchar = Mid(myRemarks, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter9 = 1
                            End If
                        End If
                    Next
                    If counter9 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Label6.Text = "Input is not Valid"
                        Me.txt_ComMember_Remarks.Focus()
                        Exit Sub
                    End If
                Else
                    myRemarks = String.Empty
                End If

                'com code3
                Dim COM_CODE As Object = Nothing
                If Me.DDL_Committees.Text <> "" Then
                    COM_CODE = Trim(DDL_Committees.Text)
                    COM_CODE = RemoveQuotes(COM_CODE)
                    If COM_CODE.Length > 15 Then
                        Label6.Text = "Error: Input is not Valid !"
                        DDL_Committees.Focus()
                        Exit Sub
                    End If
                    COM_CODE = " " & COM_CODE & " "
                    If InStr(1, COM_CODE, "CREATE", 1) > 0 Or InStr(1, COM_CODE, "DELETE", 1) > 0 Or InStr(1, COM_CODE, "DROP", 1) > 0 Or InStr(1, COM_CODE, "INSERT", 1) > 1 Or InStr(1, COM_CODE, "TRACK", 1) > 1 Or InStr(1, COM_CODE, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        DDL_Committees.Focus()
                        Exit Sub
                    End If
                    COM_CODE = TrimX(UCase(COM_CODE))
                    'check unwanted characters
                    c = 0
                    counter2 = 0
                    For iloop = 1 To Len(COM_CODE)
                        strcurrentchar = Mid(COM_CODE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter2 = 1
                            End If
                        End If
                    Next
                    If counter2 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        DDL_Committees.Focus()
                        Exit Sub
                    End If
                Else
                    Label6.Text = "Plz select the Committee from Drop-Down !"
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Plz select the Committee from Drop-Down !');", True)
                    DDL_Committees.Focus()
                    Exit Sub
                End If

                'upload staff photo
                Dim arrContent2 As Byte()
                Dim intLength2 As Integer = 0

                If FileUpload13.FileName = "" Then
                    '    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Select Proper file... ');", True)
                    '    Me.FileUpload12.Focus()
                    '    Exit Sub
                Else
                    Dim fileName As String = FileUpload13.PostedFile.FileName
                    Dim ext As String = fileName.Substring(fileName.LastIndexOf("."))
                    ext = ext.ToLower
                    Dim imgType = FileUpload13.PostedFile.ContentType

                    If ext = ".jpg" Then

                    ElseIf ext = ".bmp" Then

                    ElseIf ext = ".gif" Then

                    ElseIf ext = "jpg" Then

                    ElseIf ext = "bmp" Then

                    ElseIf ext = "gif" Then

                    Else
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Select Only gif, bmp, or jpg format files supported... ');", True)
                        Me.FileUpload13.Focus()
                        Exit Sub
                    End If
                    intLength2 = Convert.ToInt32(FileUpload13.PostedFile.InputStream.Length)
                    ReDim arrContent2(intLength2)
                    If intLength2 > 6000 Then
                        Label6.Text = "Error: Photo Size is Bigger than 6 KB"
                        Exit Sub
                    End If

                    FileUpload13.PostedFile.InputStream.Read(arrContent2, 0, intLength2)

                End If

                'INSERT THE RECORD IN TO THE DATABASE
                Dim SQL As String
                Dim Cmd As SqlCommand
                Dim da As SqlDataAdapter
                Dim ds As New DataSet
                Dim CB As SqlCommandBuilder
                Dim dtrow As DataRow
                SQL = "SELECT * FROM COMMITTEE_MEMBERS WHERE (COMMEM_ID = '00')"
                Cmd = New SqlCommand(SQL, SqlConn)
                da = New SqlDataAdapter(Cmd)
                CB = New SqlCommandBuilder(da)
                SqlConn.Open()
                da.Fill(ds, "TEAM")
                dtrow = ds.Tables("TEAM").NewRow

                If Not String.IsNullOrEmpty(Names) Then
                    dtrow("MEMBER_NAME") = Names.Trim
                End If

                If Not String.IsNullOrEmpty(Designation) Then
                    dtrow("MEMBER_DESIG") = Designation.Trim
                End If

                If Not String.IsNullOrEmpty(Rank) Then
                    dtrow("MEMBER_RANK") = Rank
                Else
                    dtrow("MEMBER_RANK") = System.DBNull.Value
                End If
                If Not String.IsNullOrEmpty(myQual) Then
                    dtrow("MEMBER_QUL") = myQual.Trim
                Else
                    dtrow("MEMBER_QUL") = System.DBNull.Value
                End If
                If Not String.IsNullOrEmpty(myResp) Then
                    dtrow("MEMBER_RESP") = myResp.Trim
                Else
                    dtrow("MEMBER_RESP") = System.DBNull.Value
                End If
                If Not String.IsNullOrEmpty(Mobile) Then
                    dtrow("MEMBER_MOBILE") = Mobile.Trim
                Else
                    dtrow("MEMBER_MOBILE") = System.DBNull.Value
                End If

                If Not String.IsNullOrEmpty(email) Then
                    dtrow("MEMBER_EMAIL") = email.Trim
                Else
                    dtrow("MEMBER_EMAIL") = System.DBNull.Value
                End If
                If Not String.IsNullOrEmpty(Phone) Then
                    dtrow("MEMBER_PHONE") = Phone.Trim
                Else
                    dtrow("MEMBER_PHONE") = System.DBNull.Value
                End If
                If Not String.IsNullOrEmpty(myRemarks) Then
                    dtrow("MEMBER_REMARKS") = myRemarks.Trim
                Else
                    dtrow("MEMBER_REMARKS") = System.DBNull.Value
                End If
                If Not String.IsNullOrEmpty(COM_CODE) Then
                    dtrow("COM_CODE") = COM_CODE.Trim
                Else
                    dtrow("COM_CODE") = System.DBNull.Value
                End If
                dtrow("LIB_CODE") = LibCode
                dtrow("USER_CODE") = Session.Item("LoggedUser")
                dtrow("DATE_ADDED") = Now.Date
                dtrow("IP") = Request.UserHostAddress.Trim
                If FileUpload13.FileName <> "" Then
                    dtrow("MEMBER_PHOTO") = arrContent2
                End If

                ds.Tables("TEAM").Rows.Add(dtrow)

                thisTransaction = SqlConn.BeginTransaction()
                da.SelectCommand.Transaction = thisTransaction
                da.Update(ds, "TEAM")
                thisTransaction.Commit()
                ClearFields()
                ' mailpwd()
                Names = Nothing
                Designation = Nothing
                myQual = Nothing
                myResp = Nothing
                Phone = Nothing
                Mobile = Nothing
                email = Nothing
                myRemarks = Nothing

                ds.Dispose()
                Label6.Text = "Library User Added Successfully!"
                bttn_Save.Visible = True
                bttn_Update.Visible = False
                Search_Bttn_Click(sender, e)
            Catch q As SqlException
                thisTransaction.Rollback()
            Catch ex As Exception
                Label6.Text = "Error: " & (ex.Message())
            Finally
                SqlConn.Close()
                Label7.Text = ""
            End Try
        End If
    End Sub
    Public Sub ClearFields()
        Me.txt_ComMember_Name.Text = ""
        Me.txt_ComMember_Desig.Text = ""
        Me.txt_ComMember_Email.Text = ""
        Me.txt_ComMember_Mobile.Text = ""
        txt_ComMember_Resp.Text = ""
        txt_ComMember_Qual.Text = ""
        txt_ComMember_Phone.Text = ""
        txt_ComMember_Remarks.Text = ""
        txt_ComMember_Name.Enabled = True
        Label6.Text = ""
        DDL_Committees.Text = ""
    End Sub
    Public Sub Cancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Cancel.Click
        bttn_Save.Visible = True
        bttn_Update.Visible = False
        Label6.Text = "Enter Data and Press SAVE Button to save the record.."
        Label7.Text = ""
        ClearFields()
    End Sub
    'Populate the users in grid     'search users
    Public Sub Search_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Search_Bttn.Click
        Dim dtUsers As DataTable = Nothing
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
                    Label6.Text = "Input is not Valid"
                    Me.txt_Search.Focus()
                    Exit Sub
                End If
                mySearchString = " " & mySearchString & " "
                If InStr(1, mySearchString, "CREATE", 1) > 0 Or InStr(1, mySearchString, "DELETE", 1) > 0 Or InStr(1, mySearchString, "DROP", 1) > 0 Or InStr(1, mySearchString, "INSERT", 1) > 1 Or InStr(1, mySearchString, "TRACK", 1) > 1 Or InStr(1, mySearchString, "TRACE", 1) > 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                    Label6.Text = "Input is not Valid"
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
                    Label6.Text = "Input is not Valid"
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
                    Label6.Text = "Input is not Valid"
                    Me.DropDownList2.Focus()
                    Exit Sub
                End If
                myfield = " " & myfield & " "
                If InStr(1, myfield, "CREATE", 1) > 0 Or InStr(1, myfield, "DELETE", 1) > 0 Or InStr(1, myfield, "DROP", 1) > 0 Or InStr(1, myfield, "INSERT", 1) > 1 Or InStr(1, myfield, "TRACK", 1) > 1 Or InStr(1, myfield, "TRACE", 1) > 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                    Label6.Text = "Input is not Valid"
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
                    Label6.Text = "Input is not Valid"
                    DropDownList2.Focus()
                    Exit Sub
                End If
            Else
                myfield = "NAME"
            End If

            'Boolean Operator validation
            Dim myBoolean As String = Nothing
            If DropDownList3.Text <> "" Then
                myBoolean = TrimAll(DropDownList3.SelectedValue)
                myBoolean = RemoveQuotes(myBoolean)
                If myBoolean.Length > 20 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                    Label6.Text = "Input is not Valid"
                    Me.DropDownList3.Focus()
                    Exit Sub
                End If
                myBoolean = " " & myBoolean & " "
                If InStr(1, myBoolean, "CREATE", 1) > 0 Or InStr(1, myBoolean, "DELETE", 1) > 0 Or InStr(1, myBoolean, "DROP", 1) > 0 Or InStr(1, myBoolean, "INSERT", 1) > 1 Or InStr(1, myBoolean, "TRACK", 1) > 1 Or InStr(1, myBoolean, "TRACE", 1) > 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                    Label6.Text = "Input is not Valid"
                    Me.DropDownList3.Focus()
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
                    Label6.Text = "Input is not Valid"
                    DropDownList3.Focus()
                    Exit Sub
                End If
            Else
                myBoolean = "AND"
            End If


            Dim SQL As String = Nothing
            SQL = "SELECT COMMEM_ID, MEMBER_NAME, MEMBER_DESIG, MEMBER_RANK, MEMBER_QUL, MEMBER_RESP, MEMBER_REMARKS FROM COMMITTEE_MEMBERS WHERE  (LIB_CODE='" & Trim(LibCode) & "')  "

            If txt_Search.Text <> "" Then
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

            SQL = SQL & " ORDER BY MEMBER_NAME ASC "

            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            da.Fill(ds)

            dtUsers = ds.Tables(0).Copy
            Dim RecordCount As Long = 0
            If dtUsers.Rows.Count = 0 Then
                Me.Grid1_LibTeam.DataSource = Nothing
                'Grid1_LibTeam.Columns.Clear()
                Grid1_LibTeam.DataBind()
                Label1.Text = "Total Record(s): 0 "
                Delete_Bttn.Enabled = False
            Else
                Grid1_LibTeam.Visible = True
                RecordCount = dtUsers.Rows.Count
                Grid1_LibTeam.DataSource = dtUsers
                Grid1_LibTeam.DataBind()
                Label1.Text = "Total Record(s): " & RecordCount
                Delete_Bttn.Enabled = True
            End If
            ViewState("dt") = dtUsers
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
    Protected Sub Grid1_LibTeam_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles Grid1_LibTeam.PageIndexChanging
        Try
            'rebind datagrid
            Grid1_LibTeam.DataSource = ViewState("dt") 'temp
            Grid1_LibTeam.PageIndex = e.NewPageIndex
            index = e.NewPageIndex * Grid1_LibTeam.PageSize
            Grid1_LibTeam.DataBind()
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
    Protected Sub Grid1_LibTeam_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles Grid1_LibTeam.Sorting
        Dim temp As DataTable = CType(ViewState("dt"), DataTable)
        GridViewSortExpression = e.SortExpression
        Grid1_LibTeam.DataSource = temp
        Dim pageIndex As Integer = Grid1_LibTeam.PageIndex
        Grid1_LibTeam.DataSource = SortDataTable(Grid1_LibTeam.DataSource, False)
        Grid1_LibTeam.DataBind()
        Grid1_LibTeam.PageIndex = pageIndex
    End Sub
    Protected Sub Grid1_LibTeam_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Grid1_LibTeam.RowDataBound
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
    Private Sub Grid1_LibTeam_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles Grid1_LibTeam.RowCommand
        Dim Command As SqlCommand = Nothing
        Dim dr As SqlDataReader = Nothing
        Try
            If e.CommandName = "Select" Then
                Dim myRowID, TeamID As Integer
                myRowID = e.CommandArgument.ToString()
                TeamID = Grid1_LibTeam.DataKeys(myRowID).Value
                If Not String.IsNullOrEmpty(TeamID) And TeamID <> 0 Then
                    Label7.Text = TeamID
                    Dim iloop As Integer
                    Dim strcurrentchar As Object
                    Dim c As Integer
                    Dim counter1 As Integer

                    TeamID = TrimX(TeamID)
                    TeamID = UCase(TeamID)

                    TeamID = RemoveQuotes(TeamID)
                    If Len(TeamID).ToString > 10 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Length of Input is not Proper... ');", True)
                        Label6.Text = "Input is not Valid"
                        Exit Sub
                    End If
                    TeamID = " " & TeamID & " "
                    If InStr(1, TeamID, " CREATE ", 1) > 0 Or InStr(1, TeamID, " DELETE ", 1) > 0 Or InStr(1, TeamID, " DROP ", 1) > 0 Or InStr(1, TeamID, " INSERT ", 1) > 1 Or InStr(1, TeamID, " TRACK ", 1) > 1 Or InStr(1, TeamID, " TRACE ", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do not use reserve words... ');", True)
                        Label6.Text = "Input is not Valid"
                        Exit Sub
                    End If
                    TeamID = TrimX(TeamID)
                    'get record details from database
                    Dim SQL As String = Nothing
                    SQL = " SELECT *  FROM COMMITTEE_MEMBERS WHERE (COMMEM_ID = '" & Trim(TeamID) & "') "
                    Command = New SqlCommand(SQL, SqlConn)
                    SqlConn.Open()
                    dr = Command.ExecuteReader(CommandBehavior.CloseConnection)
                    dr.Read()
                    If dr.HasRows = True Then
                        If dr.Item("MEMBER_NAME").ToString <> "" Then
                            txt_ComMember_Name.Text = dr.Item("MEMBER_NAME").ToString
                        Else
                            txt_ComMember_Name.Text = ""
                        End If
                        If dr.Item("MEMBER_DESIG").ToString <> "" Then
                            txt_ComMember_Desig.Text = dr.Item("MEMBER_DESIG").ToString
                        Else
                            txt_ComMember_Desig.Text = ""
                        End If
                        If dr.Item("MEMBER_RANK").ToString <> "" Then
                            DropDownList_Rank.SelectedValue = dr.Item("MEMBER_RANK").ToString
                        Else
                            DropDownList_Rank.Text = ""
                        End If
                        If dr.Item("MEMBER_PHONE").ToString <> "" Then
                            txt_ComMember_Phone.Text = dr.Item("MEMBER_PHONE").ToString
                        Else
                            txt_ComMember_Phone.Text = ""
                        End If
                        If dr.Item("MEMBER_MOBILE").ToString <> "" Then
                            txt_ComMember_Mobile.Text = dr.Item("MEMBER_MOBILE").ToString
                        Else
                            txt_ComMember_Mobile.Text = ""
                        End If
                        If dr.Item("MEMBER_EMAIL").ToString <> "" Then
                            txt_ComMember_Email.Text = dr.Item("MEMBER_EMAIL").ToString
                        Else
                            txt_ComMember_Email.Text = ""
                        End If
                        If dr.Item("MEMBER_QUL").ToString <> "" Then
                            txt_ComMember_Qual.Text = dr.Item("MEMBER_QUL").ToString
                        Else
                            txt_ComMember_Qual.Text = ""
                        End If
                        If dr.Item("MEMBER_RESP").ToString <> "" Then
                            txt_ComMember_Resp.Text = dr.Item("MEMBER_RESP").ToString
                        Else
                            txt_ComMember_Resp.Text = ""
                        End If
                        If dr.Item("MEMBER_REMARKS").ToString <> "" Then
                            txt_ComMember_Remarks.Text = dr.Item("MEMBER_REMARKS").ToString
                        Else
                            txt_ComMember_Remarks.Text = ""
                        End If
                        If dr.Item("COM_CODE").ToString <> "" Then
                            DDL_Committees.SelectedValue = dr.Item("COM_CODE").ToString
                        Else
                            DDL_Committees.Text = ""
                        End If
                        If dr.Item("MEMBER_PHOTO").ToString <> "" Then
                            Dim strURL As String = "~/Master/CommitteeMembers_GetPhoto.aspx?TEAM_ID=" & TeamID & ""
                            Image21.Visible = True
                            Image21.ImageUrl = strURL
                        Else
                            Image21.Visible = False
                        End If
                        bttn_Save.Visible = False
                        bttn_Update.Visible = True
                        Label6.Text = "Press UPDATE Button to save the Changes if any.."
                        dr.Close()
                    Else
                        Label7.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' No Record to Edit... ');", True)
                        Label6.Text = "No Record to Edit"
                    End If
                Else
                    Label7.Text = ""
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' No Record to Edit... ');", True)
                    Label6.Text = "No Record to Edit"
                End If
            End If
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
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
                'Server Validation for Lib Code
                Dim iloop As Integer
                Dim strcurrentchar As Object
                Dim c As Integer
                Dim counter1, counter2, counter3, counter4, Counter5, counter6, counter7, counter8, counter9, counter10 As Integer

                'Server Validation for Full Name
                Dim Names As String = Nothing
                Names = TrimAll(txt_ComMember_Name.Text)
                If String.IsNullOrEmpty(Names) Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Enter Full Name... ');", True)
                    Label6.Text = "Please Enter Full Name !"
                    Me.txt_ComMember_Name.Focus()
                    Exit Sub
                End If
                Names = RemoveQuotes(Names)
                If Names.Length > 250 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Name must be of Proper Length.. ');", True)
                    Label6.Text = "Name must be of Proper Length !"
                    txt_ComMember_Name.Focus()
                    Exit Sub
                End If
                Names = " " & Names & " "
                If InStr(1, Names, "CREATE", 1) > 0 Or InStr(1, Names, "DELETE", 1) > 0 Or InStr(1, Names, "DROP", 1) > 0 Or InStr(1, Names, "INSERT", 1) > 1 Or InStr(1, Names, "TRACK", 1) > 1 Or InStr(1, Names, "TRACE", 1) > 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Reserve Word... ');", True)
                    Label6.Text = "Do Not Use Reserve Word !"
                    txt_ComMember_Name.Focus()
                    Exit Sub
                End If
                Names = TrimAll(Names)
                c = 0
                counter1 = 0
                For iloop = 1 To Len(Names)
                    strcurrentchar = Mid(Names, iloop, 1)
                    If c = 0 Then
                        If Not InStr("'~!@#$^&*=+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter1 = 1
                        End If
                    End If
                Next
                If counter1 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Do Not Use Un-Wated Characters... ');", True)
                    Label6.Text = "Do Not Use Un-Wated Characters !"
                    txt_ComMember_Name.Focus()
                    Exit Sub
                End If

                '****************************************************************************************
                'Server Validation for Designation
                Dim Designation As String = Nothing
                Designation = TrimAll(txt_ComMember_Desig.Text)
                If Not String.IsNullOrEmpty(Designation) Then
                    Designation = RemoveQuotes(Designation)
                    If Designation.Length > 250 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not of proper Length... ');", True)
                        Label6.Text = "Input is not of proper Length !"
                        Me.txt_ComMember_Desig.Focus()
                        Exit Sub
                    End If
                    Designation = " " & Designation & " "
                    If InStr(1, Designation, "CREATE", 1) > 0 Or InStr(1, Designation, "DELETE", 1) > 0 Or InStr(1, Designation, "DROP", 1) > 0 Or InStr(1, Designation, "INSERT", 1) > 1 Or InStr(1, Designation, "TRACK", 1) > 1 Or InStr(1, Designation, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not use Reserve Words... ');", True)
                        Label6.Text = "Do Not use Reserve Words"
                        Me.txt_ComMember_Desig.Focus()
                        Exit Sub
                    End If
                    Designation = TrimAll(Designation)
                    'check unwanted characters
                    c = 0
                    counter2 = 0
                    For iloop = 1 To Len(Designation)
                        strcurrentchar = Mid(Designation, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter2 = 1
                            End If
                        End If
                    Next
                    If counter2 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not use un-wanted Characters... ');", True)
                        Label6.Text = "Do Not use un-wanted Characters"
                        Me.txt_ComMember_Desig.Focus()
                        Exit Sub
                    End If
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Please Enter proper Designation... ');", True)
                    Label6.Text = " Please Enter proper Designation"
                    Me.txt_ComMember_Desig.Focus()
                    Exit Sub
                End If

                '****************************************************************************************
                'Server Validation for Designation
                Dim Rank As Integer = 0
                If DropDownList_Rank.Text <> "" Then
                    Rank = Me.DropDownList_Rank.SelectedValue
                    If Not String.IsNullOrEmpty(Rank) Then
                        Rank = RemoveQuotes(Rank)
                        If Len(Rank.ToString) > 2 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not of proper Length... ');", True)
                            Label6.Text = " Input is not of proper Length"
                            Me.DropDownList_Rank.Focus()
                            Exit Sub
                        End If
                        Rank = " " & Rank & " "
                        If InStr(1, Rank, "CREATE", 1) > 0 Or InStr(1, Rank, "DELETE", 1) > 0 Or InStr(1, Rank, "DROP", 1) > 0 Or InStr(1, Rank, "INSERT", 1) > 1 Or InStr(1, Rank, "TRACK", 1) > 1 Or InStr(1, Rank, "TRACE", 1) > 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not use Reserve Words... ');", True)
                            Label6.Text = "Do Not use Reserve Words"
                            Me.DropDownList_Rank.Focus()
                            Exit Sub
                        End If
                        Rank = TrimAll(Rank)
                        'check unwanted characters
                        c = 0
                        counter3 = 0
                        For iloop = 1 To Len(Rank.ToString)
                            strcurrentchar = Mid(Rank, iloop, 1)
                            If c = 0 Then
                                If Not InStr("'~!@#$^&*=+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter3 = 1
                                End If
                            End If
                        Next
                        If counter3 = 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not use un-wanted Characters... ');", True)
                            Label6.Text = "Do Not use un-wanted Characters"
                            Me.DropDownList_Rank.Focus()
                            Exit Sub
                        End If
                    Else
                        Rank = 2
                    End If
                Else
                    Rank = 2
                End If

                '********************************************************************************************************
                'Server Validation for Phone Number
                Dim Phone As String = Nothing
                Phone = TrimAll(txt_ComMember_Phone.Text)
                If Not String.IsNullOrEmpty(Phone) Then
                    Phone = RemoveQuotes(Phone)
                    If Phone.Length > 250 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Label6.Text = "Input is not Valid"
                        Me.txt_ComMember_Phone.Focus()
                        Exit Sub
                    End If
                    Phone = " " & Phone & " "
                    If InStr(1, Phone, "CREATE", 1) > 0 Or InStr(1, Phone, "DELETE", 1) > 0 Or InStr(1, Phone, "DROP", 1) > 0 Or InStr(1, Phone, "INSERT", 1) > 1 Or InStr(1, Phone, "TRACK", 1) > 1 Or InStr(1, Phone, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Label6.Text = "Input is not Valid"
                        Me.txt_ComMember_Phone.Focus()
                        Exit Sub
                    End If
                    Phone = TrimAll(Phone)

                    'check unwanted characters
                    c = 0
                    counter4 = 0
                    For iloop = 1 To Len(Phone)
                        strcurrentchar = Mid(Phone, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter4 = 1
                            End If
                        End If
                    Next
                    If counter4 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Label6.Text = "Input is not Valid"
                        Me.txt_ComMember_Phone.Focus()
                        Exit Sub
                    End If
                Else
                    Phone = String.Empty
                End If

                '*******************************************************************************************************
                'Server Validation for Mobile Number
                Dim Mobile As String = Nothing
                Mobile = TrimAll(txt_ComMember_Mobile.Text)
                If Not String.IsNullOrEmpty(Mobile) Then
                    Mobile = RemoveQuotes(Mobile)
                    If Mobile.Length > 150 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Label6.Text = "Input is not Valid"
                        Me.txt_ComMember_Mobile.Focus()
                        Exit Sub
                    End If
                    Mobile = " " & Mobile & " "
                    If InStr(1, Mobile, "CREATE", 1) > 0 Or InStr(1, Mobile, "DELETE", 1) > 0 Or InStr(1, Mobile, "DROP", 1) > 0 Or InStr(1, Mobile, "INSERT", 1) > 1 Or InStr(1, Mobile, "TRACK", 1) > 1 Or InStr(1, Mobile, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Label6.Text = "Input is not Valid"
                        Me.txt_ComMember_Mobile.Focus()
                        Exit Sub
                    End If
                    Mobile = TrimAll(Mobile)
                    'check unwanted characters
                    c = 0
                    Counter5 = 0
                    For iloop = 1 To Len(Mobile)
                        strcurrentchar = Mid(Mobile, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                Counter5 = 1
                            End If
                        End If
                    Next
                    If Counter5 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Label6.Text = "Input is not Valid"
                        Me.txt_ComMember_Mobile.Focus()
                        Exit Sub
                    End If
                Else
                    Mobile = String.Empty
                End If

                '****************************************************************************************
                'Server Validation for E-Mail Address
                Dim email As String = Nothing
                email = TrimX(txt_ComMember_Email.Text)
                If Not String.IsNullOrEmpty(email) Then
                    email = RemoveQuotes(email)
                    If email.Length > 150 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Label6.Text = "Input is not Valid"
                        Me.txt_ComMember_Email.Focus()
                        Exit Sub
                    End If
                    email = " " & email & " "
                    If InStr(1, email, "CREATE", 1) > 0 Or InStr(1, email, "DELETE", 1) > 0 Or InStr(1, email, "DROP", 1) > 0 Or InStr(1, email, "INSERT", 1) > 1 Or InStr(1, email, "TRACK", 1) > 1 Or InStr(1, email, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Label6.Text = "Input is not Valid"
                        Me.txt_ComMember_Email.Focus()
                        Exit Sub
                    End If
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                    Label6.Text = "Input is not Valid"
                    Label6.Text = "Input is not Valid"
                    Me.txt_ComMember_Email.Focus()
                    Exit Sub
                End If
                email = TrimX(email)
                'check unwanted characters
                c = 0
                counter6 = 0
                For iloop = 1 To Len(email)
                    strcurrentchar = Mid(email, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';~!#$^&*|[]{}?<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter6 = 1
                        End If
                    End If
                Next
                If counter6 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                    Label6.Text = "Input is not Valid"
                    Me.txt_ComMember_Email.Focus()
                    Exit Sub
                End If

                '****************************************************************************************88
                'qualification
                Dim myQual As String = Nothing
                myQual = TrimAll(Me.txt_ComMember_Qual.Text)
                If Not String.IsNullOrEmpty(myQual) Then
                    myQual = RemoveQuotes(myQual)
                    If myQual.Length > 1000 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not of Proper Length.. ');", True)
                        Label6.Text = "Input is not Valid"
                        Me.txt_ComMember_Qual.Focus()
                        Exit Sub
                    End If
                    myQual = " " & myQual & " "
                    If InStr(1, myQual, "CREATE", 1) > 0 Or InStr(1, myQual, "DELETE", 1) > 0 Or InStr(1, myQual, "DROP", 1) > 0 Or InStr(1, myQual, "INSERT", 1) > 1 Or InStr(1, myQual, "TRACK", 1) > 1 Or InStr(1, myQual, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Do Not Use Reserve Words... ');", True)
                        Label6.Text = "Input is not Valid"
                        Me.txt_ComMember_Qual.Focus()
                        Exit Sub
                    End If
                    myQual = TrimAll(myQual)
                    'check unwanted characters
                    c = 0
                    counter7 = 0
                    For iloop = 1 To Len(myQual)
                        strcurrentchar = Mid(myQual, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter7 = 1
                            End If
                        End If
                    Next
                    If counter7 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Un-wanted Characters... ');", True)
                        Label6.Text = "Input is not Valid"
                        Me.txt_ComMember_Qual.Focus()
                        Exit Sub
                    End If
                Else
                    myQual = String.Empty
                End If

                '****************************************************************************************88
                'Responsibilities
                Dim myResp As String = Nothing
                myResp = TrimAll(Me.txt_ComMember_Resp.Text)
                If Not String.IsNullOrEmpty(myResp) Then
                    myResp = RemoveQuotes(myResp)
                    If myQual.Length > 1000 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not of Proper Length.. ');", True)
                        Label6.Text = "Input is not Valid"
                        Me.txt_ComMember_Resp.Focus()
                        Exit Sub
                    End If
                    myResp = " " & myResp & " "
                    If InStr(1, myResp, "CREATE", 1) > 0 Or InStr(1, myResp, "DELETE", 1) > 0 Or InStr(1, myResp, "DROP", 1) > 0 Or InStr(1, myResp, "INSERT", 1) > 1 Or InStr(1, myResp, "TRACK", 1) > 1 Or InStr(1, myResp, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Do Not Use Reserve Words... ');", True)
                        Label6.Text = "Input is not Valid"
                        Me.txt_ComMember_Resp.Focus()
                        Exit Sub
                    End If
                    myResp = TrimAll(myResp)
                    'check unwanted characters
                    c = 0
                    counter8 = 0
                    For iloop = 1 To Len(myResp)
                        strcurrentchar = Mid(myResp, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter8 = 1
                            End If
                        End If
                    Next
                    If counter8 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Un-wanted Characters... ');", True)
                        Label6.Text = "Input is not Valid"
                        Me.txt_ComMember_Resp.Focus()
                        Exit Sub
                    End If
                Else
                    myResp = String.Empty
                End If

                '****************************************************************************************88
                'remareks
                Dim myRemarks As String = Nothing
                myRemarks = TrimAll(Me.txt_ComMember_Remarks.Text)
                If Not String.IsNullOrEmpty(myRemarks) Then
                    myRemarks = RemoveQuotes(myRemarks)
                    If myRemarks.Length > 250 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Label6.Text = "Input is not Valid"
                        Me.txt_ComMember_Remarks.Focus()
                        Exit Sub
                    End If
                    myRemarks = " " & myRemarks & " "
                    If InStr(1, myRemarks, "CREATE", 1) > 0 Or InStr(1, myRemarks, "DELETE", 1) > 0 Or InStr(1, myRemarks, "DROP", 1) > 0 Or InStr(1, myRemarks, "INSERT", 1) > 1 Or InStr(1, myRemarks, "TRACK", 1) > 1 Or InStr(1, myRemarks, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Label6.Text = "Input is not Valid"
                        Me.txt_ComMember_Remarks.Focus()
                        Exit Sub
                    End If
                    myRemarks = TrimAll(myRemarks)
                    'check unwanted characters
                    c = 0
                    counter9 = 0
                    For iloop = 1 To Len(myRemarks)
                        strcurrentchar = Mid(myRemarks, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter9 = 1
                            End If
                        End If
                    Next
                    If counter9 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Label6.Text = "Input is not Valid"
                        Me.txt_ComMember_Remarks.Focus()
                        Exit Sub
                    End If
                Else
                    myRemarks = String.Empty
                End If

                'com code3
                Dim COM_CODE As Object = Nothing
                If Me.DDL_Committees.Text <> "" Then
                    COM_CODE = Trim(DDL_Committees.Text)
                    COM_CODE = RemoveQuotes(COM_CODE)
                    If COM_CODE.Length > 15 Then
                        Label6.Text = "Error: Input is not Valid !"
                        DDL_Committees.Focus()
                        Exit Sub
                    End If
                    COM_CODE = " " & COM_CODE & " "
                    If InStr(1, COM_CODE, "CREATE", 1) > 0 Or InStr(1, COM_CODE, "DELETE", 1) > 0 Or InStr(1, COM_CODE, "DROP", 1) > 0 Or InStr(1, COM_CODE, "INSERT", 1) > 1 Or InStr(1, COM_CODE, "TRACK", 1) > 1 Or InStr(1, COM_CODE, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        DDL_Committees.Focus()
                        Exit Sub
                    End If
                    COM_CODE = TrimX(UCase(COM_CODE))
                    'check unwanted characters
                    c = 0
                    counter2 = 0
                    For iloop = 1 To Len(COM_CODE)
                        strcurrentchar = Mid(COM_CODE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter2 = 1
                            End If
                        End If
                    Next
                    If counter2 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        DDL_Committees.Focus()
                        Exit Sub
                    End If
                Else
                    Label6.Text = "Plz select the Committee from Drop-Down !"
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Plz select the Committee from Drop-Down !');", True)
                    DDL_Committees.Focus()
                    Exit Sub
                End If
                'upload staff photo
                Dim arrContent2 As Byte()
                Dim intLength2 As Integer = 0

                If FileUpload13.FileName = "" Then
                    '    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Select Proper file... ');", True)
                    '    Me.FileUpload12.Focus()
                    '    Exit Sub
                Else
                    Dim fileName As String = FileUpload13.PostedFile.FileName
                    Dim ext As String = fileName.Substring(fileName.LastIndexOf("."))
                    ext = ext.ToLower
                    Dim imgType = FileUpload13.PostedFile.ContentType

                    If ext = ".jpg" Then

                    ElseIf ext = ".bmp" Then

                    ElseIf ext = ".gif" Then

                    ElseIf ext = "jpg" Then

                    ElseIf ext = "bmp" Then

                    ElseIf ext = "gif" Then

                    Else
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Select Only gif, bmp, or jpg format files supported... ');", True)
                        Label6.Text = "Error: Please Select Only gif, bmp, or jpg format files supported"
                        Me.FileUpload13.Focus()
                        Exit Sub
                    End If
                    intLength2 = Convert.ToInt32(FileUpload13.PostedFile.InputStream.Length)
                    ReDim arrContent2(intLength2)
                    If intLength2 > 6000 Then
                        Label6.Text = "Error: Photo Size is Bigger than 6 KB"
                        Exit Sub
                    End If

                    FileUpload13.PostedFile.InputStream.Read(arrContent2, 0, intLength2)

                End If


                '****************************************************************************************************
                'UPDATE THE LIBRARY PROFILE  
                If Label7.Text <> "" Then
                    SQL = "SELECT * FROM COMMITTEE_MEMBERS WHERE (COMMEM_ID='" & Trim(Label7.Text) & "')"
                    SqlConn.Open()
                    da = New SqlDataAdapter(SQL, SqlConn)
                    cb = New SqlCommandBuilder(da)
                    da.Fill(ds, "USERS")
                    If ds.Tables("USERS").Rows.Count <> 0 Then
                        If Not String.IsNullOrEmpty(Names) Then
                            ds.Tables("USERS").Rows(0)("MEMBER_NAME") = Names.Trim
                        Else
                            ds.Tables("USERS").Rows(0)("MEMBER_NAME") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(Designation) Then
                            ds.Tables("USERS").Rows(0)("MEMBER_DESIG") = Designation.Trim
                        Else
                            ds.Tables("USERS").Rows(0)("MEMBER_DESIG") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(Rank) Then
                            ds.Tables("USERS").Rows(0)("MEMBER_RANK") = Rank
                        Else
                            ds.Tables("USERS").Rows(0)("MEMBER_RANK") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(myQual) Then
                            ds.Tables("USERS").Rows(0)("MEMBER_QUL") = myQual.Trim
                        Else
                            ds.Tables("USERS").Rows(0)("MEMBER_QUL") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(myResp) Then
                            ds.Tables("USERS").Rows(0)("MEMBER_RESP") = myResp.Trim
                        Else
                            ds.Tables("USERS").Rows(0)("MEMBER_RESP") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(myRemarks) Then
                            ds.Tables("USERS").Rows(0)("MEMBER_REMARKS") = myRemarks.ToString.Trim
                        Else
                            ds.Tables("USERS").Rows(0)("MEMBER_REMARKS") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(Phone) Then
                            ds.Tables("USERS").Rows(0)("MEMBER_PHONE") = Phone.Trim
                        Else
                            ds.Tables("USERS").Rows(0)("MEMBER_PHONE") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(Mobile) Then
                            ds.Tables("USERS").Rows(0)("MEMBER_MOBILE") = Mobile.Trim
                        Else
                            ds.Tables("USERS").Rows(0)("MEMBER_MOBILE") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(email) Then
                            ds.Tables("USERS").Rows(0)("MEMBER_EMAIL") = email.Trim
                        Else
                            ds.Tables("USERS").Rows(0)("MEMBER_EMAIL") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(COM_CODE) Then
                            ds.Tables("USERS").Rows(0)("COM_CODE") = COM_CODE
                        Else
                            ds.Tables("USERS").Rows(0)("COM_CODE") = System.DBNull.Value
                        End If
                        If FileUpload13.FileName <> "" Then
                            ds.Tables("USERS").Rows(0)("MEMBER_PHOTO") = arrContent2
                        End If
                        ds.Tables("USERS").Rows(0)("UPDATED_BY") = UserCode.Trim
                        ds.Tables("USERS").Rows(0)("DATE_MODIFIED") = Now.Date
                        ds.Tables("USERS").Rows(0)("IP") = Request.UserHostAddress.Trim

                        thisTransaction = SqlConn.BeginTransaction()
                        da.SelectCommand.Transaction = thisTransaction
                        da.Update(ds, "USERS")
                        thisTransaction.Commit()
                        Names = Nothing
                        Designation = Nothing
                        Phone = Nothing
                        Mobile = Nothing
                        email = Nothing
                        myRemarks = Nothing
                        Image21.ImageUrl = ""
                        Label6.Visible = True
                        Label6.Text = "User Record Updated Successfully"
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' User Profile Updated Successfully... ');", True)
                        ClearFields()
                    Else
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Record Update  - Please Contact System Administrator... ');", True)
                        Label6.Text = "Error: Record Update  - Please Contact System Administrator"
                        Exit Sub
                    End If
                End If
            Else
                'record not selected
                ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Record Not Selected... ');", True)
                Label6.Text = "Error: Record Not Selected"
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
            For Each row As GridViewRow In Grid1_LibTeam.Rows
                Dim cb As CheckBox = DirectCast(row.FindControl("cbd"), CheckBox)
                If cb IsNot Nothing AndAlso cb.Checked = True Then
                    Dim LogID As Integer = Convert.ToInt32(Grid1_LibTeam.DataKeys(row.RowIndex).Value)
                    'get cat record
                    Dim SQL As String = Nothing
                    SQL = "DELETE FROM COMMITTEE_MEMBERS WHERE (COMMEM_ID ='" & Trim(LogID) & "') "
                    Dim objCommand As New SqlCommand(SQL, SqlConn)
                    Dim da As New SqlDataAdapter(objCommand)
                    Dim ds As New DataSet
                    da.Fill(ds)
                End If
            Next
            Dim s As Object = Nothing
            Dim e1 As EventArgs = Nothing
            Search_Bttn_Click(s, e1)
            Label6.Text = "Record(s) Deleted Successfully!"
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
End Class