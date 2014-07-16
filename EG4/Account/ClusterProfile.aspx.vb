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
Public Class ClusterProfile
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session.Item("IsThisU") <> Nothing Then
                If Not SConnection() = True Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Database Connection is lost..Try Again !');", True)
                Else
                    If Page.IsPostBack = False Then
                        GetClusterDetails()
                    End If
                    Me.txt_Cluster_Name.Focus()
                End If
                Dim CreateSUserButton As Object 'System.Web.UI.WebControls.Button
                CreateSUserButton = Master.FindControl("Accordion1").FindControl("DBAdminPane").FindControl("Adm_Cluster_Bttn")
                CreateSUserButton.ForeColor = Drawing.Color.Red
                myPaneName = "DBAdminPane" 'paneSelectedIndex = 0
            Else
                Response.Redirect("~/Default.aspx", False)
            End If
        Catch ex As Exception
            Label6.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    Private Sub ClusterProfile_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Me.txt_Cluster_Name.Focus()
    End Sub
    Public Sub GetClusterDetails()
        Dim Command As SqlCommand = Nothing
        Dim drCluster As SqlDataReader = Nothing
        Try
            'get record details from database
            Dim SQL As String = Nothing
            SQL = " SELECT *  FROM CLUSTER  "
            Command = New SqlCommand(SQL, SqlConn)
            SqlConn.Open()
            drCluster = Command.ExecuteReader(CommandBehavior.CloseConnection)
            drCluster.Read()
            If drCluster.HasRows = True Then
                If drCluster.Item("NAME").ToString <> "" Then
                    txt_Cluster_Name.Text = drCluster.Item("NAME").ToString
                Else
                    txt_Cluster_Name.Text = ""
                End If
                If drCluster.Item("ADDRESS").ToString <> "" Then
                    txt_Cluster_Add.Text = drCluster.Item("ADDRESS").ToString
                Else
                    txt_Cluster_Add.Text = ""
                End If
                If drCluster.Item("URL").ToString <> "" Then
                    txt_Cluster_URL.Text = drCluster.Item("URL").ToString
                Else
                    txt_Cluster_URL.Text = ""
                End If
                If drCluster.Item("REMARKS").ToString <> "" Then
                    TextArea1.InnerText = drCluster.Item("REMARKS").ToString '.Replace(Environment.NewLine, "<br />")
                Else
                    TextArea1.InnerText = ""
                End If
               
                If drCluster.Item("LOGO").ToString <> "" Then
                    Dim strURL As String = "~/Account/Cluster_GetLogo.aspx" ' & txt_Cluster_Name.Text & ""
                    Image21.ImageUrl = strURL
                Else
                    Image21.Visible = False
                End If
                Me.bttn_Save.Visible = False
                bttn_Update.Visible = True
            Else
                Me.bttn_Save.Visible = True
                bttn_Update.Visible = False
            End If
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
        Finally
            Command.Dispose()
            drCluster.Close()
            SqlConn.Close()
        End Try
    End Sub
    Public Sub ClearFields()
        Me.txt_Cluster_Name.Text = ""
        Me.txt_Cluster_Add.Text = ""
        Me.txt_Cluster_URL.Text = ""
        TextArea1.InnerText = ""
    End Sub
    Protected Sub Cancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Cancel.Click
        ClearFields()
    End Sub
    'update Record
    Protected Sub bttn_Update_Click(ByVal sender As Object, ByVal e As EventArgs) Handles bttn_Update.Click
        Dim SQL As String = Nothing
        Dim da As SqlDataAdapter = Nothing
        Dim cb As SqlCommandBuilder = Nothing
        Dim ds As New DataSet
        Dim dt As New DataTable
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing
        Try
            If IsPostBack = True Then
                'Server Validation for Lib Code
                Dim iloop As Integer
                Dim strcurrentchar As Object
                Dim c As Integer
                Dim counter1, counter2, counter3, counter4, Counter5, counter6, counter7, counter8, counter9 As Integer

                'Server Validation for User Code
                Dim newClusterName As String = Nothing
                newClusterName = TrimAll(txt_Cluster_Name.Text)
                If Not String.IsNullOrEmpty(newClusterName) Then
                    newClusterName = RemoveQuotes(newClusterName)
                    If newClusterName.Length > 750 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Length of Name is not Proper... ');", True)
                        Me.txt_Cluster_Name.Focus()
                        Exit Sub
                    End If
                    newClusterName = " " & newClusterName & " "
                    If InStr(1, newClusterName, " CREATE ", 1) > 0 Or InStr(1, newClusterName, " DELETE ", 1) > 0 Or InStr(1, newClusterName, " DROP ", 1) > 0 Or InStr(1, newClusterName, " INSERT ", 1) > 1 Or InStr(1, newClusterName, " TRACK ", 1) > 1 Or InStr(1, newClusterName, " TRACE ", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do not use reserve words... ');", True)
                        Me.txt_Cluster_Name.Focus()
                        Exit Sub
                    End If
                    newClusterName = TrimAll(newClusterName)
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Enter Cluster Name.. ');", True)
                    Me.txt_Cluster_Name.Focus()
                    Exit Sub
                End If
                'check unwanted characters
                c = 0
                counter1 = 0
                For iloop = 1 To Len(newClusterName)
                    strcurrentchar = Mid(newClusterName, iloop, 1)
                    If c = 0 Then
                        If Not InStr("'~!@#$^&*=+|[]{}?<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter1 = 1
                        End If
                    End If
                Next
                If counter1 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Un-wanted Characters... ');", True)
                    Me.txt_Cluster_Name.Focus()
                    Exit Sub
                End If

                'Server Validation for Full Name
                Dim URL As String = Nothing
                URL = TrimAll(txt_Cluster_URL.Text)
                If Not String.IsNullOrEmpty(URL) Then
                    URL = RemoveQuotes(URL)
                    If URL.Length > 450 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' URL must be of Proper Length.. ');", True)
                        txt_Cluster_URL.Focus()
                        Exit Sub
                    End If
                    URL = " " & URL & " "
                    If InStr(1, URL, "CREATE", 1) > 0 Or InStr(1, URL, "DELETE", 1) > 0 Or InStr(1, URL, "DROP", 1) > 0 Or InStr(1, URL, "INSERT", 1) > 1 Or InStr(1, URL, "TRACK", 1) > 1 Or InStr(1, URL, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Reserve Word... ');", True)
                        txt_Cluster_URL.Focus()
                        Exit Sub
                    End If
                    URL = TrimX(URL)
                    c = 0
                    counter2 = 0
                    For iloop = 1 To Len(URL)
                        strcurrentchar = Mid(URL, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'!@#$^&*|[]{}<>()""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter2 = 1
                            End If
                        End If
                    Next
                Else
                    URL = ""
                End If
                If counter2 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Do Not Use Un-Wated Characters... ');", True)
                    txt_Cluster_URL.Focus()
                    Exit Sub
                End If

                '****************************************************************************************
                'Server Validation for Designation
                Dim Address As String = Nothing
                Address = TrimAll(txt_Cluster_Add.Text)
                If Not String.IsNullOrEmpty(Address) Then
                    Address = RemoveQuotes(Address)
                    If Address.Length > 250 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not proper... ');", True)
                        Me.txt_Cluster_Add.Focus()
                        Exit Sub
                    End If
                    Address = " " & Address & " "
                    If InStr(1, Address, "CREATE", 1) > 0 Or InStr(1, Address, "DELETE", 1) > 0 Or InStr(1, Address, "DROP", 1) > 0 Or InStr(1, Address, "INSERT", 1) > 1 Or InStr(1, Address, "TRACK", 1) > 1 Or InStr(1, Address, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not proper... ');", True)
                        Me.txt_Cluster_Add.Focus()
                        Exit Sub
                    End If
                    Address = TrimAll(Address)
                    'check unwanted characters
                    c = 0
                    counter3 = 0
                    For iloop = 1 To Len(Address)
                        strcurrentchar = Mid(Address, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter3 = 1
                            End If
                        End If
                    Next
                    If counter3 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not proper... ');", True)
                        Me.txt_Cluster_Name.Focus()
                        Exit Sub
                    End If
                Else
                    Address = ""
                End If
                
                '****************************************************************************************88
                'remareks
                Dim myRemarks As String = Nothing
                If TextArea1.InnerText <> "" Then
                    myRemarks = Trim(TextArea1.InnerText)
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Plz Enter a brief Introduction of Cluster!');", True)
                    Me.TextArea1.Focus()
                    Exit Sub
                End If

                'upload user photo
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

                    FileUpload13.PostedFile.InputStream.Read(arrContent2, 0, intLength2)

                End If

                '****************************************************************************************************
                'UPDATE THE LIBRARY PROFILE   
                SQL = "SELECT * FROM CLUSTER "
                SqlConn.Open()
                da = New SqlDataAdapter(SQL, SqlConn)
                cb = New SqlCommandBuilder(da)
                da.Fill(ds, "CLUSTER")
                If ds.Tables("CLUSTER").Rows.Count <> 0 Then

                    If Not String.IsNullOrEmpty(newClusterName) Then
                        ds.Tables("CLUSTER").Rows(0)("NAME") = newClusterName
                    Else
                        ds.Tables("CLUSTER").Rows(0)("NAME") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(URL) Then
                        ds.Tables("CLUSTER").Rows(0)("url") = URL.Trim
                    Else
                        ds.Tables("CLUSTER").Rows(0)("url") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(Address) Then
                        ds.Tables("CLUSTER").Rows(0)("ADDRESS") = Address.Trim
                    Else
                        ds.Tables("CLUSTER").Rows(0)("ADDRESS") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(myRemarks) Then
                        ds.Tables("CLUSTER").Rows(0)("REMARKS") = myRemarks
                    Else
                        ds.Tables("CLUSTER").Rows(0)("REMARKS") = System.DBNull.Value
                    End If

                    If FileUpload13.FileName <> "" Then
                        ds.Tables("CLUSTER").Rows(0)("LOGO") = arrContent2
                    End If


                    thisTransaction = SqlConn.BeginTransaction()
                    da.SelectCommand.Transaction = thisTransaction
                    da.Update(ds, "CLUSTER")
                    thisTransaction.Commit()

                    newClusterName = Nothing
                    Address = Nothing
                    URL = Nothing
                    myRemarks = Nothing
                    Label6.Visible = True
                    Label6.Text = "Cluster Details Updated Successfully"
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Cluster Detail Updated Successfully... ');", True)
                    ClearFields()

                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Clsuter Details Update  - Please Contact System Administrator... ');", True)
                    Exit Sub
                End If
            End If
            SqlConn.Close()
            GetClusterDetails()
        Catch q As SqlException
            thisTransaction.Rollback()
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'save records
    Protected Sub bttn_Save_Click(ByVal sender As Object, ByVal e As EventArgs) Handles bttn_Save.Click
        If IsPostBack = True Then
            'check unwanted characters
            Dim iloop As Integer
            Dim strcurrentchar As Object
            Dim c As Integer
            Dim counter1, counter2, counter3, counter4, Counter5, counter6, Counter7, counter8, counter9, counter10 As Integer
            Dim thisTransaction As SqlClient.SqlTransaction = Nothing
            Try
                If IsPostBack = True Then
                    'Server Validation for User Code
                    Dim newClusterName As String = Nothing
                    newClusterName = TrimAll(txt_Cluster_Name.Text)
                    If Not String.IsNullOrEmpty(newClusterName) Then
                        newClusterName = RemoveQuotes(newClusterName)
                        If newClusterName.Length > 750 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Length of Name is not Proper... ');", True)
                            Me.txt_Cluster_Name.Focus()
                            Exit Sub
                        End If
                        newClusterName = " " & newClusterName & " "
                        If InStr(1, newClusterName, " CREATE ", 1) > 0 Or InStr(1, newClusterName, " DELETE ", 1) > 0 Or InStr(1, newClusterName, " DROP ", 1) > 0 Or InStr(1, newClusterName, " INSERT ", 1) > 1 Or InStr(1, newClusterName, " TRACK ", 1) > 1 Or InStr(1, newClusterName, " TRACE ", 1) > 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do not use reserve words... ');", True)
                            Me.txt_Cluster_Name.Focus()
                            Exit Sub
                        End If
                        newClusterName = TrimAll(newClusterName)
                    Else
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Enter Cluster Name.. ');", True)
                        Me.txt_Cluster_Name.Focus()
                        Exit Sub
                    End If
                    'check unwanted characters
                    c = 0
                    counter1 = 0
                    For iloop = 1 To Len(newClusterName)
                        strcurrentchar = Mid(newClusterName, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter1 = 1
                            End If
                        End If
                    Next
                    If counter1 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Un-wanted Characters... ');", True)
                        Me.txt_Cluster_Name.Focus()
                        Exit Sub
                    End If

                    'Server Validation for Full Name
                    Dim URL As String = Nothing
                    URL = TrimAll(txt_Cluster_URL.Text)
                    If Not String.IsNullOrEmpty(URL) Then
                    URL = RemoveQuotes(URL)
                    If URL.Length > 450 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' URL must be of Proper Length.. ');", True)
                        txt_Cluster_URL.Focus()
                        Exit Sub
                    End If
                    URL = " " & URL & " "
                    If InStr(1, URL, "CREATE", 1) > 0 Or InStr(1, URL, "DELETE", 1) > 0 Or InStr(1, URL, "DROP", 1) > 0 Or InStr(1, URL, "INSERT", 1) > 1 Or InStr(1, URL, "TRACK", 1) > 1 Or InStr(1, URL, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Reserve Word... ');", True)
                        txt_Cluster_URL.Focus()
                        Exit Sub
                    End If
                    URL = TrimX(URL)
                    c = 0
                    counter2 = 0
                    For iloop = 1 To Len(URL)
                        strcurrentchar = Mid(URL, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'!@#$^&*|[]{}<>()""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter2 = 1
                            End If
                        End If
                        Next
                    Else
                        URL = ""
                    End If
                    If counter2 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Do Not Use Un-Wated Characters... ');", True)
                        txt_Cluster_URL.Focus()
                        Exit Sub
                    End If

                    '****************************************************************************************
                    'Server Validation for address
                    Dim Address As String = Nothing
                    Address = TrimAll(txt_Cluster_Add.Text)
                    If Not String.IsNullOrEmpty(Address) Then
                        Address = RemoveQuotes(Address)
                        If Address.Length > 250 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not proper... ');", True)
                            Me.txt_Cluster_Add.Focus()
                            Exit Sub
                        End If
                        Address = " " & Address & " "
                        If InStr(1, Address, "CREATE", 1) > 0 Or InStr(1, Address, "DELETE", 1) > 0 Or InStr(1, Address, "DROP", 1) > 0 Or InStr(1, Address, "INSERT", 1) > 1 Or InStr(1, Address, "TRACK", 1) > 1 Or InStr(1, Address, "TRACE", 1) > 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not proper... ');", True)
                            Me.txt_Cluster_Add.Focus()
                            Exit Sub
                        End If
                        Address = TrimAll(Address)
                        'check unwanted characters
                        c = 0
                        counter3 = 0
                        For iloop = 1 To Len(Address)
                            strcurrentchar = Mid(Address, iloop, 1)
                            If c = 0 Then
                                If Not InStr("'~!@#$^&*=+|[]{}?<>=()%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter3 = 1
                                End If
                            End If
                        Next
                        If counter3 = 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not proper... ');", True)
                            Me.txt_Cluster_Name.Focus()
                            Exit Sub
                        End If

                    Else
                        Address = ""
                    End If
                   


                    '****************************************************************************************88
                    'remareks
                    Dim myRemarks As String = Nothing
                    If TextArea1.InnerText <> "" Then
                        myRemarks = Trim(TextArea1.InnerText)
                    Else
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Plz Enter a brief Introduction of Cluster!');", True)
                        Me.TextArea1.Focus()
                        Exit Sub
                    End If


                    'upload user photo
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

                        FileUpload13.PostedFile.InputStream.Read(arrContent2, 0, intLength2)

                    End If

                    '**************************************************************************************************
                    'Check Duplicate User Code
                    Dim str2 As Object = Nothing
                    Dim flag2 As Object = Nothing
                    Dim STR As String = Nothing
                    STR = "SELECT  ID FROM CLUSTER "
                    Dim cmd2 As New SqlCommand(STR, SqlConn)
                    SqlConn.Open()
                    flag2 = cmd2.ExecuteScalar
                    If flag2 <> Nothing Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Cluster Record Already Exists, Please abort... ');", True)
                        Me.txt_Cluster_Name.Focus()
                        Exit Sub
                    End If
                    SqlConn.Close()

                    'INSERT THE RECORD IN TO THE DATABASE
                    Dim SQL As String
                    Dim Cmd As SqlCommand
                    Dim da As SqlDataAdapter
                    Dim ds As New DataSet
                    Dim CB As SqlCommandBuilder
                    Dim dtrow As DataRow
                    SQL = "SELECT * FROM CLUSTER "
                    Cmd = New SqlCommand(SQL, SqlConn)
                    da = New SqlDataAdapter(Cmd)
                    CB = New SqlCommandBuilder(da)
                    SqlConn.Open()
                    da.Fill(ds, "CLUSTER")
                    dtrow = ds.Tables("CLUSTER").NewRow
                    If Not String.IsNullOrEmpty(newClusterName) Then
                        dtrow("NAME") = newClusterName.Trim
                    End If
                    If Not String.IsNullOrEmpty(Address) Then
                        dtrow("ADDRESS") = Address.Trim
                    End If
                    If Not String.IsNullOrEmpty(URL) Then
                        dtrow("URL") = URL.Trim
                    End If
                    If Not String.IsNullOrEmpty(myRemarks) Then
                        dtrow("REMARKS") = myRemarks
                    Else
                        dtrow("REMARKS") = System.DBNull.Value
                    End If
                    If FileUpload13.FileName <> "" Then
                        dtrow("LOGO") = arrContent2
                    Else
                        dtrow("LOGO") = System.DBNull.Value
                    End If

                    ds.Tables("CLUSTER").Rows.Add(dtrow)

                    thisTransaction = SqlConn.BeginTransaction()
                    da.SelectCommand.Transaction = thisTransaction


                    da.Update(ds, "CLUSTER")
                    thisTransaction.Commit()
                    ClearFields()

                    ds.Dispose()
                    Alert.Show("Cluster Account Created/updated Sucessfully !")
                    'Dim url = "../Default.aspx"
                    'ClientScript.RegisterStartupScript(Me.GetType(), "callfunction", "alert('Admin User Registered Sucessfully !  ');window.location.href = '" + url + "';", True)
                End If
                SqlConn.Close()
                GetClusterDetails()
            Catch q As SqlException
                thisTransaction.Rollback()
            Catch ex As Exception
                Label6.Text = "Error: " & (ex.Message())
            Finally
                SqlConn.Close()
            End Try
        End If

    End Sub
End Class