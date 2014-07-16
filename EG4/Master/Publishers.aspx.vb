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
Public Class Publishers
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
                        PopulateCountry()
                        Label6.Text = "Enter Data and Press SAVE Button to save the record.."
                    End If
                End If
                Dim CreateSUserButton As Object 'System.Web.UI.WebControls.Button
                CreateSUserButton = Master.FindControl("Accordion1").FindControl("MasterPane").FindControl("M_Pub_Bttn")
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
    Private Sub Publishers_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        txt_Pub_Name.Focus()
    End Sub
    'populate bib levels
    Public Sub PopulateCountry()
        Dim Command As SqlCommand = Nothing
        Dim dt As DataTable = Nothing
        Try
            Command = New SqlCommand("SELECT CON_ID, CON_CODE, CON_NAME FROM COUNTRIES ORDER BY CON_NAME ", SqlConn)
            SqlConn.Open()
            Dim da As New SqlDataAdapter(Command)
            Dim ds As New DataSet
            Dim RecordCount As Long = 0
            da.Fill(ds)

            dt = ds.Tables(0).Copy

            Dim Dr As DataRow
            Dr = dt.NewRow
            Dr("CON_CODE") = ""
            Dr("CON_NAME") = ""
            dt.Rows.InsertAt(Dr, 0)

            If dt.Rows.Count = 0 Then
                Me.DropDownList_Country.DataSource = Nothing
            Else
                Me.DropDownList_Country.DataSource = dt
                Me.DropDownList_Country.DataTextField = "CON_NAME"
                Me.DropDownList_Country.DataValueField = "CON_CODE"
                Me.DropDownList_Country.DataBind()
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
    'save data
    Public Sub Save_Click(ByVal sender As Object, ByVal e As EventArgs) Handles bttn_Save.Click
        If IsPostBack = True Then
            'check unwanted characters
            Dim iloop As Integer
            Dim strcurrentchar As Object
            Dim c As Integer
            Dim counter1, counter2, counter3, counter4, counter5, counter6, counter7, counter8, counter9, counter10 As Integer
            Dim thisTransaction As SqlClient.SqlTransaction = Nothing
            Try
                'Server Validation for  name
                Dim Name As String = Nothing
                If txt_Pub_Name.Text <> "" Then
                    Name = TrimAll(txt_Pub_Name.Text)
                    Name = RemoveQuotes(Name)
                    If Name.Length > 250 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        txt_Pub_Name.Focus()
                        Exit Sub
                    End If
                    Name = " " & Name & " "
                    If InStr(1, Name, "CREATE", 1) > 0 Or InStr(1, Name, "DELETE", 1) > 0 Or InStr(1, Name, "DROP", 1) > 0 Or InStr(1, Name, "INSERT", 1) > 1 Or InStr(1, Name, "TRACK", 1) > 1 Or InStr(1, Name, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Me.txt_Pub_Name.Focus()
                        Exit Sub
                    End If
                    Name = TrimAll(Name)
                    'check unwanted characters
                    c = 0
                    counter1 = 0
                    For iloop = 1 To Len(Name.ToString)
                        strcurrentchar = Mid(Name, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^*+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter1 = 1
                            End If
                        End If
                    Next
                    If counter1 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Me.txt_Pub_Name.Focus()
                        Exit Sub
                    End If

                    'Check Duplicate User Code
                    Dim str As Object = Nothing
                    Dim flag As Object = Nothing
                    str = "SELECT PUB_ID FROM PUBLISHERS WHERE (PUB_NAME = '" & Trim(Name) & "' ) "
                    Dim cmd1 As New SqlCommand(str, SqlConn)
                    SqlConn.Open()
                    flag = cmd1.ExecuteScalar
                    If flag <> Nothing Then
                        Label6.Text = " Publisher Name Already Exists ! "
                        Me.txt_Pub_Name.Focus()
                        Exit Sub
                    End If
                    SqlConn.Close()
                Else
                    Label6.Text = " Please Enter Data in this field... "
                    Me.txt_Pub_Name.Focus()
                    Exit Sub
                End If
                '****************************************************************************************
                'Server Validation for  place
                Dim myPlace As String = Nothing
                If txt_Pub_Place.Text <> "" Then
                    myPlace = TrimAll(txt_Pub_Place.Text)
                    myPlace = RemoveQuotes(myPlace)
                    If myPlace.Length > 100 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        txt_Pub_Place.Focus()
                        Exit Sub
                    End If
                    myPlace = " " & myPlace & " "
                    If InStr(1, myPlace, "CREATE", 1) > 0 Or InStr(1, myPlace, "DELETE", 1) > 0 Or InStr(1, myPlace, "DROP", 1) > 0 Or InStr(1, myPlace, "INSERT", 1) > 1 Or InStr(1, myPlace, "TRACK", 1) > 1 Or InStr(1, myPlace, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Me.txt_Pub_Place.Focus()
                        Exit Sub
                    End If
                    myPlace = TrimAll(myPlace)
                    'check unwanted characters
                    c = 0
                    counter2 = 0
                    For iloop = 1 To Len(myPlace.ToString)
                        strcurrentchar = Mid(myPlace, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^*+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter2 = 1
                            End If
                        End If
                    Next
                    If counter2 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Me.txt_Pub_Place.Focus()
                        Exit Sub
                    End If
                Else
                    Label6.Text = " Please Enter Data in this field... "
                    Me.txt_Pub_Place.Focus()
                    Exit Sub
                End If

                '****************************************************************************************
                'Server Validation for  place
                Dim myAdd As String = Nothing
                If txt_Pub_Address.Text <> "" Then
                    myAdd = TrimAll(txt_Pub_Address.Text)
                    myAdd = RemoveQuotes(myAdd)
                    If myAdd.Length > 250 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        txt_Pub_Address.Focus()
                        Exit Sub
                    End If
                    myAdd = " " & myAdd & " "
                    If InStr(1, myAdd, "CREATE", 1) > 0 Or InStr(1, myAdd, "DELETE", 1) > 0 Or InStr(1, myAdd, "DROP", 1) > 0 Or InStr(1, myAdd, "INSERT", 1) > 1 Or InStr(1, myAdd, "TRACK", 1) > 1 Or InStr(1, myAdd, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Me.txt_Pub_Address.Focus()
                        Exit Sub
                    End If
                    myAdd = TrimAll(myAdd)
                    'check unwanted characters
                    c = 0
                    counter3 = 0
                    For iloop = 1 To Len(myAdd.ToString)
                        strcurrentchar = Mid(myAdd, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^*+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter3 = 1
                            End If
                        End If
                    Next
                    If counter3 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Me.txt_Pub_Address.Focus()
                        Exit Sub
                    End If
                Else
                    myAdd = ""
                End If

                '****************************************************************************************
                'Server Validation for E-Mail Address
                Dim email As String
                email = TrimX(txt_Pub_Email.Text)
                If Not String.IsNullOrEmpty(email) Then
                    email = RemoveQuotes(email)
                    If email.Length > 200 Then
                        Label6.Text = " Input is not Valid.."
                        Me.txt_Pub_Email.Focus()
                        Exit Sub
                    End If
                    email = " " & email & " "
                    If InStr(1, email, "CREATE", 1) > 0 Or InStr(1, email, "DELETE", 1) > 0 Or InStr(1, email, "DROP", 1) > 0 Or InStr(1, email, "INSERT", 1) > 1 Or InStr(1, email, "TRACK", 1) > 1 Or InStr(1, email, "TRACE", 1) > 1 Then
                        Label6.Text = "  Input is not Valid... "
                        Me.txt_Pub_Email.Focus()
                        Exit Sub
                    End If
                    email = TrimX(email)
                    'check unwanted characters
                    c = 0
                    counter4 = 0
                    For iloop = 1 To Len(email)
                        strcurrentchar = Mid(email, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';~!#$^&*|[]{}?<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter4 = 1
                            End If
                        End If
                    Next
                    If counter4 = 1 Then
                        Label6.Text = "  Input is not Valid..."
                        Me.txt_Pub_Email.Focus()
                        Exit Sub
                    End If
                Else
                    email = ""
                End If
                '****************************************************************************************88
                'remareks
                Dim myRemarks As String = Nothing
                myRemarks = TrimAll(Me.txt_Pub_Remarks.Text)
                If Not String.IsNullOrEmpty(myRemarks) Then
                    myRemarks = RemoveQuotes(myRemarks)
                    If myRemarks.Length > 250 Then
                        Label6.Text = " Input is not Valid... "
                        Me.txt_Pub_Remarks.Focus()
                        Exit Sub
                    End If
                    myRemarks = TrimAll(myRemarks)
                    'check unwanted characters
                    c = 0
                    counter5 = 0
                    For iloop = 1 To Len(myRemarks)
                        strcurrentchar = Mid(myRemarks, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter5 = 1
                            End If
                        End If
                    Next
                    If counter5 = 1 Then
                        Label6.Text = " Input is not Valid..."
                        Me.txt_Pub_Remarks.Focus()
                        Exit Sub
                    End If
                Else
                    myRemarks = String.Empty
                End If

                '****************************************************************************************88
                'phone
                Dim myPhone As String
                myPhone = TrimAll(txt_Pub_Phone.Text)
                If Not String.IsNullOrEmpty(myPhone) Then
                    myPhone = RemoveQuotes(myPhone)
                    If myPhone.Length > 100 Then
                        Label6.Text = " Input is not Valid.."
                        Me.txt_Pub_Phone.Focus()
                        Exit Sub
                    End If
                    myPhone = " " & myPhone & " "
                    If InStr(1, myPhone, "CREATE", 1) > 0 Or InStr(1, myPhone, "DELETE", 1) > 0 Or InStr(1, myPhone, "DROP", 1) > 0 Or InStr(1, myPhone, "INSERT", 1) > 1 Or InStr(1, myPhone, "TRACK", 1) > 1 Or InStr(1, myPhone, "TRACE", 1) > 1 Then
                        Label6.Text = "  Input is not Valid... "
                        Me.txt_Pub_Phone.Focus()
                        Exit Sub
                    End If
                    myPhone = TrimX(myPhone)
                    'check unwanted characters
                    c = 0
                    counter6 = 0
                    For iloop = 1 To Len(myPhone)
                        strcurrentchar = Mid(myPhone, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'!#$^&*|[]{}?<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter6 = 1
                            End If
                        End If
                    Next
                    If counter6 = 1 Then
                        Label6.Text = "  Input is not Valid..."
                        Me.txt_Pub_Phone.Focus()
                        Exit Sub
                    End If
                Else
                    myPhone = ""
                End If

                '****************************************************************************************88
                'phone
                Dim myFax As String
                myFax = TrimAll(txt_Pub_Fax.Text)
                If Not String.IsNullOrEmpty(myFax) Then
                    myFax = RemoveQuotes(myFax)
                    If myFax.Length > 100 Then
                        Label6.Text = " Input is not Valid.."
                        Me.txt_Pub_Fax.Focus()
                        Exit Sub
                    End If
                    myFax = " " & myFax & " "
                    If InStr(1, myFax, "CREATE", 1) > 0 Or InStr(1, myFax, "DELETE", 1) > 0 Or InStr(1, myFax, "DROP", 1) > 0 Or InStr(1, myFax, "INSERT", 1) > 1 Or InStr(1, myFax, "TRACK", 1) > 1 Or InStr(1, myFax, "TRACE", 1) > 1 Then
                        Label6.Text = "  Input is not Valid... "
                        Me.txt_Pub_Fax.Focus()
                        Exit Sub
                    End If
                    myFax = TrimAll(myFax)
                    'check unwanted characters
                    c = 0
                    counter7 = 0
                    For iloop = 1 To Len(myFax)
                        strcurrentchar = Mid(myFax, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'!#$^&*|[]{}?<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter7 = 1
                            End If
                        End If
                    Next
                    If counter7 = 1 Then
                        Label6.Text = "  Input is not Valid..."
                        Me.txt_Pub_Fax.Focus()
                        Exit Sub
                    End If
                Else
                    myFax = ""
                End If

                '****************************************************************************************
                'Server Validation for E-Mail Address
                Dim url As String = Nothing
                url = TrimX(txt_Pub_Url.Text)
                If Not String.IsNullOrEmpty(url) Then
                    url = RemoveQuotes(url)
                    If url.Length > 250 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Me.txt_Pub_Url.Focus()
                        Exit Sub
                    End If
                    url = " " & url & " "
                    If InStr(1, url, "CREATE", 1) > 0 Or InStr(1, url, "DELETE", 1) > 0 Or InStr(1, url, "DROP", 1) > 0 Or InStr(1, url, "INSERT", 1) > 1 Or InStr(1, url, "TRACK", 1) > 1 Or InStr(1, url, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Pub_Url.Focus()
                        Exit Sub
                    End If
                    url = TrimX(url)
                    'check unwanted characters
                    c = 0
                    counter8 = 0
                    For iloop = 1 To Len(url)
                        strcurrentchar = Mid(url, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';!#$^&*|[]{}?<>()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter8 = 1
                            End If
                        End If
                    Next
                    If counter8 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Pub_Url.Focus()
                        Exit Sub
                    End If
                    If InStr(url, "http://") = 0 Then
                        url = "http://" & url
                    End If
                Else
                    url = ""
                End If

                '****************************************************************************************
                'Server Validation for Econtact person
                Dim myContact As String = Nothing
                myContact = TrimX(txt_Pub_Contact.Text)
                If Not String.IsNullOrEmpty(myContact) Then
                    myContact = RemoveQuotes(myContact)
                    If myContact.Length > 250 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Me.txt_Pub_Contact.Focus()
                        Exit Sub
                    End If
                    myContact = " " & myContact & " "
                    If InStr(1, myContact, "CREATE", 1) > 0 Or InStr(1, myContact, "DELETE", 1) > 0 Or InStr(1, myContact, "DROP", 1) > 0 Or InStr(1, myContact, "INSERT", 1) > 1 Or InStr(1, myContact, "TRACK", 1) > 1 Or InStr(1, myContact, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Pub_Contact.Focus()
                        Exit Sub
                    End If
                    myContact = TrimAll(myContact)
                    'check unwanted characters
                    c = 0
                    counter9 = 0
                    For iloop = 1 To Len(myContact)
                        strcurrentchar = Mid(myContact, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'!#$^&*|[]{}?<>()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter9 = 1
                            End If
                        End If
                    Next
                    If counter9 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Pub_Contact.Focus()
                        Exit Sub
                    End If
                Else
                    myContact = ""
                End If

                '**************************************************************************************
                'Server Validation for Classification Scheme
                Dim Country As String = Nothing
                Country = DropDownList_Country.SelectedValue
                If Not String.IsNullOrEmpty(Country) Then
                    Country = RemoveQuotes(Country)
                    If Country.Length > 4 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input not Valid... ');", True)
                        DropDownList_Country.Focus()
                        Exit Sub
                    End If
                    If InStr(1, Country, "CREATE", 1) > 0 Or InStr(1, Country, "DELETE", 1) > 0 Or InStr(1, Country, "DROP", 1) > 0 Or InStr(1, Country, "INSERT", 1) > 1 Or InStr(1, Country, "TRACK", 1) > 1 Or InStr(1, Country, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        DropDownList_Country.Focus()
                        Exit Sub
                    End If
                    Country = TrimAll(Country)
                    'check unwanted characters
                    c = 0
                    counter10 = 0
                    For iloop = 1 To Len(Country)
                        strcurrentchar = Mid(Country, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter10 = 1
                            End If
                        End If
                    Next
                    If counter10 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                        DropDownList_Country.Focus()
                        Exit Sub
                    End If
                Else
                    Country = "IND"
                End If

                'INSERT THE RECORD IN TO THE DATABASE
                Dim SQL As String
                Dim Cmd As SqlCommand
                Dim da As SqlDataAdapter
                Dim ds As New DataSet
                Dim CB As SqlCommandBuilder
                Dim dtrow As DataRow
                SQL = "SELECT * FROM PUBLISHERS WHERE (PUB_ID = '00')"
                Cmd = New SqlCommand(SQL, SqlConn)
                da = New SqlDataAdapter(Cmd)
                CB = New SqlCommandBuilder(da)
                SqlConn.Open()
                da.Fill(ds, "PUBLISHERS")
                dtrow = ds.Tables("PUBLISHERS").NewRow

                If Not String.IsNullOrEmpty(Name) Then
                    dtrow("PUB_NAME") = Name.Trim
                End If
                If Not String.IsNullOrEmpty(myPlace) Then
                    dtrow("PUB_PLACE") = myPlace.Trim
                Else
                    dtrow("PUB_PLACE") = System.DBNull.Value
                End If
                If Not String.IsNullOrEmpty(Country) Then
                    dtrow("CON_CODE") = Country.Trim
                Else
                    dtrow("CON_CODE") = System.DBNull.Value
                End If

                If Not String.IsNullOrEmpty(myAdd) Then
                    dtrow("PUB_ADDRESS") = myAdd.Trim
                Else
                    dtrow("PUB_ADDRESS") = System.DBNull.Value
                End If

                If Not String.IsNullOrEmpty(myFax) Then
                    dtrow("PUB_FAX") = myFax.Trim
                Else
                    dtrow("PUB_FAX") = System.DBNull.Value
                End If

                If Not String.IsNullOrEmpty(myPhone) Then
                    dtrow("PUB_PHONE") = myPhone.Trim
                Else
                    dtrow("PUB_PHONE") = System.DBNull.Value
                End If

                If Not String.IsNullOrEmpty(email) Then
                    dtrow("PUB_EMAIL") = email.Trim
                Else
                    dtrow("PUB_EMAIL") = System.DBNull.Value
                End If

                If Not String.IsNullOrEmpty(url) Then
                    dtrow("PUB_URL") = url.Trim
                Else
                    dtrow("PUB_URL") = System.DBNull.Value
                End If

                If Not String.IsNullOrEmpty(myRemarks) Then
                    dtrow("REMARKS") = myRemarks.Trim
                Else
                    dtrow("REMARKS") = System.DBNull.Value
                End If

                If Not String.IsNullOrEmpty(myContact) Then
                    dtrow("CONTACT_PERSON") = myContact.Trim
                Else
                    dtrow("CONTACT_PERSON") = System.DBNull.Value
                End If

                dtrow("LIB_CODE") = LibCode
                dtrow("USER_CODE") = Session.Item("LoggedUser")
                dtrow("DATE_ADDED") = Now.Date
                dtrow("IP") = Request.UserHostAddress.Trim

                ds.Tables("PUBLISHERS").Rows.Add(dtrow)

                thisTransaction = SqlConn.BeginTransaction()
                da.SelectCommand.Transaction = thisTransaction
                da.Update(ds, "PUBLISHERS")
                thisTransaction.Commit()
                ' mailpwd()
                Name = Nothing
                myPlace = Nothing
                Country = Nothing
                myAdd = Nothing
                email = Nothing
                myRemarks = Nothing
                myFax = Nothing
                myPhone = Nothing
                url = Nothing
                myContact = Nothing

                ds.Dispose()
                Label6.Text = "Record Added Successfully!"
                bttn_Save.Visible = True
                bttn_Update.Visible = False
                ClearFields()
                Search_Bttn_Click(sender, e)
                UpdatePanel1.Update()
            Catch q As SqlException
                thisTransaction.Rollback()
            Catch ex As Exception
                Label6.Text = "Error: " & (ex.Message())
            Finally
                SqlConn.Close()
            End Try
        End If
    End Sub
    'this function is called from other pages for adding new publishers
    Public Function PUB_SAVE(ByVal myPubName As Object, Optional ByVal myPubPlace As Object = Nothing, Optional ByVal myConCode As Object = Nothing, Optional ByVal myLibCode As Object = Nothing)
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing
        Try
            SqlConn.Open()
            thisTransaction = SqlConn.BeginTransaction()
            Dim intValue As Integer = 0
            Dim objCommand As New SqlCommand
            objCommand.Connection = SqlConn
            objCommand.Transaction = thisTransaction
            objCommand.CommandType = CommandType.Text
            objCommand.CommandText = "INSERT INTO PUBLISHERS (PUB_NAME, PUB_PLACE, CON_CODE, LIB_CODE) " & _
                                     " VALUES (@myPubName, @myPubPlace, @myConCode, @myLibCode); " & _
                                     " SELECT SCOPE_IDENTITY()"

            If myPubName = "" Then myPubName = System.DBNull.Value
            objCommand.Parameters.Add("@myPubName", SqlDbType.NVarChar)
            objCommand.Parameters("@myPubName").Value = myPubName

            If myPubPlace = "" Then myPubPlace = System.DBNull.Value
            objCommand.Parameters.Add("@myPubPlace", SqlDbType.NVarChar)
            objCommand.Parameters("@myPubPlace").Value = myPubPlace

            If myConCode = "" Then myConCode = System.DBNull.Value
            objCommand.Parameters.Add("@myConCode", SqlDbType.VarChar)
            objCommand.Parameters("@myConCode").Value = myConCode

            If LibCode = "" Then LibCode = System.DBNull.Value
            objCommand.Parameters.Add("@myLibCode", SqlDbType.NVarChar)
            objCommand.Parameters("@myLibCode").Value = LibCode

            Dim dr As SqlDataReader
            dr = objCommand.ExecuteReader()
            If dr.Read Then
                intValue = dr.GetValue(0)
            End If
            dr.Close()
            thisTransaction.Commit()
            SqlConn.Close()
        Catch q As SqlException
            thisTransaction.Rollback()
        Catch ex As Exception
            Label6.Text = "Error: " & (ex.Message())
        Finally
            SqlConn.Close()
        End Try
    End Function

    Public Sub Cancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Cancel.Click
        Label7.Text = ""
        bttn_Save.Visible = True
        bttn_Update.Visible = False
        Label6.Text = "Enter Data and Press SAVE Button to save the record.."
        ClearFields()
    End Sub
    Public Sub ClearFields()
        txt_Pub_Name.Text = ""
        txt_Pub_Place.Text = ""
        txt_Pub_Address.Text = ""
        txt_Pub_Email.Text = ""
        txt_Pub_Remarks.Text = ""
        txt_Pub_Fax.Text = ""
        txt_Pub_Email.Text = ""
        txt_Pub_Phone.Text = ""
        txt_Pub_Contact.Text = ""
        txt_Pub_Url.Text = ""
        DropDownList_Country.Text = ""
    End Sub
    'Populate the users in grid     'search users
    Protected Sub Search_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Search_Bttn.Click
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
            If DropDownList2.Text <> "" Then
                myfield = TrimAll(DropDownList2.SelectedValue)
                myfield = RemoveQuotes(myfield)
                If myfield.Length > 50 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                    Me.DropDownList2.Focus()
                    Exit Sub
                End If
                myfield = " " & myfield & " "
                If InStr(1, myfield, "CREATE", 1) > 0 Or InStr(1, myfield, "DELETE", 1) > 0 Or InStr(1, myfield, "DROP", 1) > 0 Or InStr(1, myfield, "INSERT", 1) > 1 Or InStr(1, myfield, "TRACK", 1) > 1 Or InStr(1, myfield, "TRACE", 1) > 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
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
                    DropDownList2.Focus()
                    Exit Sub
                End If
            Else
                myfield = "PUB_NAME"
            End If

            'Boolean Operator validation
            Dim myBoolean As String = Nothing
            If DropDownList3.Text <> "" Then
                myBoolean = TrimAll(DropDownList3.SelectedValue)
                myBoolean = RemoveQuotes(myBoolean)
                If myBoolean.Length > 20 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                    Me.DropDownList3.Focus()
                    Exit Sub
                End If
                myBoolean = " " & myBoolean & " "
                If InStr(1, myBoolean, "CREATE", 1) > 0 Or InStr(1, myBoolean, "DELETE", 1) > 0 Or InStr(1, myBoolean, "DROP", 1) > 0 Or InStr(1, myBoolean, "INSERT", 1) > 1 Or InStr(1, myBoolean, "TRACK", 1) > 1 Or InStr(1, myBoolean, "TRACE", 1) > 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
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
                    DropDownList3.Focus()
                    Exit Sub
                End If
            Else
                myBoolean = "AND"
            End If

            Dim SQL As String = Nothing
            SQL = "SELECT * FROM PUBLISHERS "

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
            Else
                SQL = SQL
            End If

            SQL = SQL & " ORDER BY PUB_NAME ASC "

            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            da.Fill(ds)

            dtUsers = ds.Tables(0).Copy
            Dim RecordCount As Long = 0
            If dtUsers.Rows.Count = 0 Then
                Me.Grid_Pub.DataSource = Nothing
                Grid_Pub.DataBind()
                Delete_Bttn.Enabled = False
                Label1.Text = "Total Record(s): " & Grid_Pub.Rows.Count
            Else
                Grid_Pub.Visible = True
                RecordCount = dtUsers.Rows.Count
                Grid_Pub.DataSource = dtUsers
                Grid_Pub.DataBind()
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
    Protected Sub Grid_Pub_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles Grid_Pub.PageIndexChanging
        Try
            'rebind datagrid
            Grid_Pub.DataSource = ViewState("dt") 'temp
            Grid_Pub.PageIndex = e.NewPageIndex
            index = e.NewPageIndex * Grid_Pub.PageSize
            Grid_Pub.DataBind()

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
    Protected Sub Grid_Pub_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles Grid_Pub.Sorting
        Dim temp As DataTable = CType(ViewState("dt"), DataTable)
        GridViewSortExpression = e.SortExpression
        Grid_Pub.DataSource = temp
        Dim pageIndex As Integer = Grid_Pub.PageIndex
        Grid_Pub.DataSource = SortDataTable(Grid_Pub.DataSource, False)
        Grid_Pub.DataBind()
        Grid_Pub.PageIndex = pageIndex
        UpdatePanel1.Update()
    End Sub
    Protected Sub Grid_Pub_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Grid_Pub.RowDataBound
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
    Private Sub Grid_Pub_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles Grid_Pub.RowCommand
        Dim Command As SqlCommand = Nothing
        Dim dr As SqlDataReader = Nothing
        Try
            If e.CommandName = "Select" Then
                Dim myRowID, PubID As Integer
                myRowID = e.CommandArgument.ToString()

                If Grid_Pub.Rows(myRowID).Cells(5).Text <> "" Then
                    PubID = Grid_Pub.Rows(myRowID).Cells(5).Text
                    Label7.Text = PubID
                Else
                    PubID = ""
                    Label7.Text = ""
                End If

                Dim iloop As Integer
                Dim strcurrentchar As Object
                Dim c As Integer
                Dim counter1 As Integer

                PubID = TrimX(PubID)
                PubID = UCase(PubID)
                If Not String.IsNullOrEmpty(PubID) Then
                    PubID = RemoveQuotes(PubID)
                    If Len(PubID).ToString > 10 Then
                        Label6.Text = "Length of Input is not Proper... "
                        Exit Sub
                    End If
                    PubID = " " & PubID & " "
                    If InStr(1, PubID, " CREATE ", 1) > 0 Or InStr(1, PubID, " DELETE ", 1) > 0 Or InStr(1, PubID, " DROP ", 1) > 0 Or InStr(1, PubID, " INSERT ", 1) > 1 Or InStr(1, PubID, " TRACK ", 1) > 1 Or InStr(1, PubID, " TRACE ", 1) > 1 Then
                        Label6.Text = "Do not use reserve words..."
                        Exit Sub
                    End If
                    PubID = TrimX(PubID)
                Else
                    Label6.Text = "Enter committee Code... "
                    Exit Sub
                End If

                'get record details from database
                Dim SQL As String = Nothing
                SQL = " SELECT *  FROM PUBLISHERS WHERE (PUB_ID = '" & Trim(PubID) & "') "
                Command = New SqlCommand(SQL, SqlConn)
                SqlConn.Open()
                dr = Command.ExecuteReader(CommandBehavior.CloseConnection)
                dr.Read()
                ClearFields()
                If dr.HasRows = True Then
                    If dr.Item("PUB_ID").ToString <> "" Then
                        Me.Label7.Text = dr.Item("PUB_ID").ToString
                    Else
                        Me.Label7.Text = ""
                    End If
                    If dr.Item("PUB_NAME").ToString <> "" Then
                        Me.txt_Pub_Name.Text = dr.Item("PUB_NAME").ToString
                    Else
                        Me.txt_Pub_Name.Text = ""
                    End If
                    If dr.Item("PUB_PLACE").ToString <> "" Then
                        txt_Pub_Place.Text = dr.Item("PUB_PLACE").ToString
                    Else
                        txt_Pub_Place.Text = ""
                    End If
                    If dr.Item("PUB_ADDRESS").ToString <> "" Then
                        Me.txt_Pub_Address.Text = dr.Item("PUB_ADDRESS").ToString
                    Else
                        Me.txt_Pub_Address.Text = ""
                    End If
                    If dr.Item("PUB_EMAIL").ToString <> "" Then
                        txt_Pub_Email.Text = dr.Item("PUB_EMAIL").ToString
                    Else
                        txt_Pub_Email.Text = ""
                    End If
                    If dr.Item("PUB_FAX").ToString <> "" Then
                        txt_Pub_Fax.Text = dr.Item("PUB_FAX").ToString
                    Else
                        txt_Pub_Fax.Text = ""
                    End If
                    If dr.Item("PUB_URL").ToString <> "" Then
                        txt_Pub_Url.Text = dr.Item("PUB_URL").ToString
                    Else
                        txt_Pub_Url.Text = ""
                    End If
                    If dr.Item("REMARKS").ToString <> "" Then
                        Me.txt_Pub_Remarks.Text = dr.Item("REMARKS").ToString
                    Else
                        Me.txt_Pub_Remarks.Text = ""
                    End If
                    If dr.Item("CONTACT_PERSON").ToString <> "" Then
                        Me.txt_Pub_Contact.Text = dr.Item("CONTACT_PERSON").ToString
                    Else
                        Me.txt_Pub_Contact.Text = ""
                    End If

                    If dr.Item("CON_CODE").ToString <> "" Then
                        Me.DropDownList_Country.SelectedValue = dr.Item("CON_CODE").ToString
                    Else
                        Me.DropDownList_Country.Text = ""
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
    Public Sub mailpwd()
        'Dim msg As New CDO.Message()
        'Dim iConfg As CDO.IConfiguration
        'iConfg = msg.Configuration
        'msg.From = "rkmatoria@nic.in"
        'msg.[To] = email
        'msg.Subject = "e-Granthalaya Admin User Account..."
        'Dim txtMsg As String = "<html><body>Dear User: " + usercode + " Your Account created sucessfully!..</body></html>"
        'msg.HTMLBody = txtMsg
        'msg.Send()
        'ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Mail Sent to your E-Mail');", True)

        'Dim message As System.Net.Mail.MailMessage = New System.Net.Mail.MailMessage("rkmatoria@nic.in", "rkmatoria@nic.in")
        'message.Subject = "Test"
        'message.Body = "xxxx"

        'Dim smtp As System.Net.Mail.SmtpClient = New System.Net.Mail.SmtpClient("mail.nic.in", "465")
        'smtp.EnableSsl = True
        'smtp.Credentials = New Net.NetworkCredential("rkmatoria", "Rkmatoria$123")
        'smtp.Send(message)
    End Sub
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

                'Server Validation for  name
                Dim Name As String = Nothing
                If txt_Pub_Name.Text <> "" Then
                    Name = TrimAll(txt_Pub_Name.Text)
                    Name = RemoveQuotes(Name)
                    If Name.Length > 250 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        txt_Pub_Name.Focus()
                        Exit Sub
                    End If
                    Name = " " & Name & " "
                    If InStr(1, Name, "CREATE", 1) > 0 Or InStr(1, Name, "DELETE", 1) > 0 Or InStr(1, Name, "DROP", 1) > 0 Or InStr(1, Name, "INSERT", 1) > 1 Or InStr(1, Name, "TRACK", 1) > 1 Or InStr(1, Name, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Me.txt_Pub_Name.Focus()
                        Exit Sub
                    End If
                    Name = TrimAll(Name)
                    'check unwanted characters
                    c = 0
                    counter1 = 0
                    For iloop = 1 To Len(Name.ToString)
                        strcurrentchar = Mid(Name, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^*+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter1 = 1
                            End If
                        End If
                    Next
                    If counter1 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Me.txt_Pub_Name.Focus()
                        Exit Sub
                    End If

                    'Check Duplicate User Code
                    Dim str As Object = Nothing
                    Dim flag As Object = Nothing
                    str = "SELECT PUB_ID FROM PUBLISHERS WHERE (PUB_ID <>'" & Trim(Label7.Text) & "' AND PUB_NAME = '" & Trim(Name) & "' ) "
                    Dim cmd1 As New SqlCommand(str, SqlConn)
                    SqlConn.Open()
                    flag = cmd1.ExecuteScalar
                    If flag <> Nothing Then
                        Label6.Text = " Publisher Name Already Exists ! "
                        Me.txt_Pub_Name.Focus()
                        Exit Sub
                    End If
                    SqlConn.Close()
                Else
                    Label6.Text = " Please Enter Data in this field... "
                    Me.txt_Pub_Name.Focus()
                    Exit Sub
                End If
                '****************************************************************************************
                'Server Validation for  place
                Dim myPlace As String = Nothing
                If txt_Pub_Place.Text <> "" Then
                    myPlace = TrimAll(txt_Pub_Place.Text)
                    myPlace = RemoveQuotes(myPlace)
                    If myPlace.Length > 100 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        txt_Pub_Place.Focus()
                        Exit Sub
                    End If
                    myPlace = " " & myPlace & " "
                    If InStr(1, myPlace, "CREATE", 1) > 0 Or InStr(1, myPlace, "DELETE", 1) > 0 Or InStr(1, myPlace, "DROP", 1) > 0 Or InStr(1, myPlace, "INSERT", 1) > 1 Or InStr(1, myPlace, "TRACK", 1) > 1 Or InStr(1, myPlace, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Me.txt_Pub_Place.Focus()
                        Exit Sub
                    End If
                    myPlace = TrimAll(myPlace)
                    'check unwanted characters
                    c = 0
                    counter2 = 0
                    For iloop = 1 To Len(myPlace.ToString)
                        strcurrentchar = Mid(myPlace, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^*+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter2 = 1
                            End If
                        End If
                    Next
                    If counter2 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Me.txt_Pub_Place.Focus()
                        Exit Sub
                    End If
                Else
                    Label6.Text = " Please Enter Data in this field... "
                    Me.txt_Pub_Place.Focus()
                    Exit Sub
                End If

                '****************************************************************************************
                'Server Validation for  place
                Dim myAdd As String = Nothing
                If txt_Pub_Address.Text <> "" Then
                    myAdd = TrimAll(txt_Pub_Address.Text)
                    myAdd = RemoveQuotes(myAdd)
                    If myAdd.Length > 250 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        txt_Pub_Address.Focus()
                        Exit Sub
                    End If
                    myAdd = " " & myAdd & " "
                    If InStr(1, myAdd, "CREATE", 1) > 0 Or InStr(1, myAdd, "DELETE", 1) > 0 Or InStr(1, myAdd, "DROP", 1) > 0 Or InStr(1, myAdd, "INSERT", 1) > 1 Or InStr(1, myAdd, "TRACK", 1) > 1 Or InStr(1, myAdd, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Me.txt_Pub_Address.Focus()
                        Exit Sub
                    End If
                    myAdd = TrimAll(myAdd)
                    'check unwanted characters
                    c = 0
                    counter3 = 0
                    For iloop = 1 To Len(myAdd.ToString)
                        strcurrentchar = Mid(myAdd, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^*+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter3 = 1
                            End If
                        End If
                    Next
                    If counter3 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Me.txt_Pub_Address.Focus()
                        Exit Sub
                    End If
                Else
                    myAdd = ""
                End If

                '****************************************************************************************
                'Server Validation for E-Mail Address
                Dim email As String
                email = TrimX(txt_Pub_Email.Text)
                If Not String.IsNullOrEmpty(email) Then
                    email = RemoveQuotes(email)
                    If email.Length > 200 Then
                        Label6.Text = " Input is not Valid.."
                        Me.txt_Pub_Email.Focus()
                        Exit Sub
                    End If
                    email = " " & email & " "
                    If InStr(1, email, "CREATE", 1) > 0 Or InStr(1, email, "DELETE", 1) > 0 Or InStr(1, email, "DROP", 1) > 0 Or InStr(1, email, "INSERT", 1) > 1 Or InStr(1, email, "TRACK", 1) > 1 Or InStr(1, email, "TRACE", 1) > 1 Then
                        Label6.Text = "  Input is not Valid... "
                        Me.txt_Pub_Email.Focus()
                        Exit Sub
                    End If
                    email = TrimX(email)
                    'check unwanted characters
                    c = 0
                    counter4 = 0
                    For iloop = 1 To Len(email)
                        strcurrentchar = Mid(email, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';~!#$^&*|[]{}?<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter4 = 1
                            End If
                        End If
                    Next
                    If counter4 = 1 Then
                        Label6.Text = "  Input is not Valid..."
                        Me.txt_Pub_Email.Focus()
                        Exit Sub
                    End If
                Else
                    email = ""
                End If
                '****************************************************************************************88
                'remareks
                Dim myRemarks As String = Nothing
                myRemarks = TrimAll(Me.txt_Pub_Remarks.Text)
                If Not String.IsNullOrEmpty(myRemarks) Then
                    myRemarks = RemoveQuotes(myRemarks)
                    If myRemarks.Length > 250 Then
                        Label6.Text = " Input is not Valid... "
                        Me.txt_Pub_Remarks.Focus()
                        Exit Sub
                    End If
                    myRemarks = TrimAll(myRemarks)
                    'check unwanted characters
                    c = 0
                    Counter5 = 0
                    For iloop = 1 To Len(myRemarks)
                        strcurrentchar = Mid(myRemarks, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*|[]{}?<>%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                Counter5 = 1
                            End If
                        End If
                    Next
                    If Counter5 = 1 Then
                        Label6.Text = " Input is not Valid..."
                        Me.txt_Pub_Remarks.Focus()
                        Exit Sub
                    End If
                Else
                    myRemarks = String.Empty
                End If

                '****************************************************************************************88
                'phone
                Dim myPhone As String
                myPhone = TrimAll(txt_Pub_Phone.Text)
                If Not String.IsNullOrEmpty(myPhone) Then
                    myPhone = RemoveQuotes(myPhone)
                    If myPhone.Length > 100 Then
                        Label6.Text = " Input is not Valid.."
                        Me.txt_Pub_Phone.Focus()
                        Exit Sub
                    End If
                    myPhone = " " & myPhone & " "
                    If InStr(1, myPhone, "CREATE", 1) > 0 Or InStr(1, myPhone, "DELETE", 1) > 0 Or InStr(1, myPhone, "DROP", 1) > 0 Or InStr(1, myPhone, "INSERT", 1) > 1 Or InStr(1, myPhone, "TRACK", 1) > 1 Or InStr(1, myPhone, "TRACE", 1) > 1 Then
                        Label6.Text = "  Input is not Valid... "
                        Me.txt_Pub_Phone.Focus()
                        Exit Sub
                    End If
                    myPhone = TrimX(myPhone)
                    'check unwanted characters
                    c = 0
                    counter6 = 0
                    For iloop = 1 To Len(myPhone)
                        strcurrentchar = Mid(myPhone, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'!#$^&*|[]{}?<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter6 = 1
                            End If
                        End If
                    Next
                    If counter6 = 1 Then
                        Label6.Text = "  Input is not Valid..."
                        Me.txt_Pub_Phone.Focus()
                        Exit Sub
                    End If
                Else
                    myPhone = ""
                End If

                '****************************************************************************************88
                'phone
                Dim myFax As String
                myFax = TrimAll(txt_Pub_Fax.Text)
                If Not String.IsNullOrEmpty(myFax) Then
                    myFax = RemoveQuotes(myFax)
                    If myFax.Length > 100 Then
                        Label6.Text = " Input is not Valid.."
                        Me.txt_Pub_Fax.Focus()
                        Exit Sub
                    End If
                    myFax = " " & myFax & " "
                    If InStr(1, myFax, "CREATE", 1) > 0 Or InStr(1, myFax, "DELETE", 1) > 0 Or InStr(1, myFax, "DROP", 1) > 0 Or InStr(1, myFax, "INSERT", 1) > 1 Or InStr(1, myFax, "TRACK", 1) > 1 Or InStr(1, myFax, "TRACE", 1) > 1 Then
                        Label6.Text = "  Input is not Valid... "
                        Me.txt_Pub_Fax.Focus()
                        Exit Sub
                    End If
                    myFax = TrimAll(myFax)
                    'check unwanted characters
                    c = 0
                    counter7 = 0
                    For iloop = 1 To Len(myFax)
                        strcurrentchar = Mid(myFax, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'!#$^&*|[]{}?<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter7 = 1
                            End If
                        End If
                    Next
                    If counter7 = 1 Then
                        Label6.Text = "  Input is not Valid..."
                        Me.txt_Pub_Fax.Focus()
                        Exit Sub
                    End If
                Else
                    myFax = ""
                End If

                '****************************************************************************************
                'Server Validation for E-Mail Address
                Dim url As String = Nothing
                url = TrimX(txt_Pub_Url.Text)
                If Not String.IsNullOrEmpty(url) Then
                    url = RemoveQuotes(url)
                    If url.Length > 250 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Me.txt_Pub_Url.Focus()
                        Exit Sub
                    End If
                    url = " " & url & " "
                    If InStr(1, url, "CREATE", 1) > 0 Or InStr(1, url, "DELETE", 1) > 0 Or InStr(1, url, "DROP", 1) > 0 Or InStr(1, url, "INSERT", 1) > 1 Or InStr(1, url, "TRACK", 1) > 1 Or InStr(1, url, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Pub_Url.Focus()
                        Exit Sub
                    End If
                    url = TrimX(url)
                    'check unwanted characters
                    c = 0
                    counter8 = 0
                    For iloop = 1 To Len(url)
                        strcurrentchar = Mid(url, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';!#$^&*|[]{}?<>()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter8 = 1
                            End If
                        End If
                    Next
                    If counter8 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Pub_Url.Focus()
                        Exit Sub
                    End If
                    If InStr(url, "http://") = 0 Then
                        url = "http://" & url
                    End If
                Else
                    url = ""
                End If

                '****************************************************************************************
                'Server Validation for Econtact person
                Dim myContact As String = Nothing
                myContact = TrimX(txt_Pub_Contact.Text)
                If Not String.IsNullOrEmpty(myContact) Then
                    myContact = RemoveQuotes(myContact)
                    If myContact.Length > 250 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        Me.txt_Pub_Contact.Focus()
                        Exit Sub
                    End If
                    myContact = " " & myContact & " "
                    If InStr(1, myContact, "CREATE", 1) > 0 Or InStr(1, myContact, "DELETE", 1) > 0 Or InStr(1, myContact, "DROP", 1) > 0 Or InStr(1, myContact, "INSERT", 1) > 1 Or InStr(1, myContact, "TRACK", 1) > 1 Or InStr(1, myContact, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Pub_Contact.Focus()
                        Exit Sub
                    End If
                    myContact = TrimAll(myContact)
                    'check unwanted characters
                    c = 0
                    counter9 = 0
                    For iloop = 1 To Len(myContact)
                        strcurrentchar = Mid(myContact, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'!#$^&*|[]{}?<>()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter9 = 1
                            End If
                        End If
                    Next
                    If counter9 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('  Input is not Valid... ');", True)
                        Me.txt_Pub_Contact.Focus()
                        Exit Sub
                    End If
                Else
                    myContact = ""
                End If

                '**************************************************************************************
                'Server Validation for Classification Scheme
                Dim Country As String = Nothing
                Country = DropDownList_Country.SelectedValue
                If Not String.IsNullOrEmpty(Country) Then
                    Country = RemoveQuotes(Country)
                    If Country.Length > 4 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input not Valid... ');", True)
                        DropDownList_Country.Focus()
                        Exit Sub
                    End If
                    If InStr(1, Country, "CREATE", 1) > 0 Or InStr(1, Country, "DELETE", 1) > 0 Or InStr(1, Country, "DROP", 1) > 0 Or InStr(1, Country, "INSERT", 1) > 1 Or InStr(1, Country, "TRACK", 1) > 1 Or InStr(1, Country, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        DropDownList_Country.Focus()
                        Exit Sub
                    End If
                    Country = TrimAll(Country)
                    'check unwanted characters
                    c = 0
                    counter10 = 0
                    For iloop = 1 To Len(Country)
                        strcurrentchar = Mid(Country, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter10 = 1
                            End If
                        End If
                    Next
                    If counter10 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                        DropDownList_Country.Focus()
                        Exit Sub
                    End If
                Else
                    Country = "IND"
                End If

                '****************************************************************************************************
                'UPDATE THE LIBRARY PROFILE  
                If Label7.Text <> "" Then
                    SQL = "SELECT * FROM PUBLISHERS WHERE (PUB_ID='" & Trim(Label7.Text) & "')"
                    SqlConn.Open()
                    da = New SqlDataAdapter(SQL, SqlConn)
                    cb = New SqlCommandBuilder(da)
                    da.Fill(ds, "PUBLISHERS")
                    If ds.Tables("PUBLISHERS").Rows.Count <> 0 Then
                        If Not String.IsNullOrEmpty(Name) Then
                            ds.Tables("PUBLISHERS").Rows(0)("PUB_NAME") = Name.Trim
                        Else
                            ds.Tables("PUBLISHERS").Rows(0)("PUB_NAME") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(myPlace) Then
                            ds.Tables("PUBLISHERS").Rows(0)("PUB_PLACE") = myPlace.Trim
                        Else
                            ds.Tables("PUBLISHERS").Rows(0)("PUB_PLACE") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(Country) Then
                            ds.Tables("PUBLISHERS").Rows(0)("CON_CODE") = Country
                        Else
                            ds.Tables("PUBLISHERS").Rows(0)("CON_CODE") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(myAdd) Then
                            ds.Tables("PUBLISHERS").Rows(0)("PUB_ADDRESS") = myAdd.Trim
                        Else
                            ds.Tables("PUBLISHERS").Rows(0)("PUB_ADDRESS") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(email) Then
                            ds.Tables("PUBLISHERS").Rows(0)("PUB_EMAIL") = email.Trim
                        Else
                            ds.Tables("PUBLISHERS").Rows(0)("PUB_EMAIL") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(myFax) Then
                            ds.Tables("PUBLISHERS").Rows(0)("PUB_FAX") = myFax.ToString.Trim
                        Else
                            ds.Tables("PUBLISHERS").Rows(0)("PUB_FAX") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(myPhone) Then
                            ds.Tables("PUBLISHERS").Rows(0)("PUB_PHONE") = myPhone.ToString.Trim
                        Else
                            ds.Tables("PUBLISHERS").Rows(0)("PUB_PHONE") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(url) Then
                            ds.Tables("PUBLISHERS").Rows(0)("PUB_URL") = url.ToString.Trim
                        Else
                            ds.Tables("PUBLISHERS").Rows(0)("PUB_URL") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(myRemarks) Then
                            ds.Tables("PUBLISHERS").Rows(0)("REMARKS") = myRemarks.ToString.Trim
                        Else
                            ds.Tables("PUBLISHERS").Rows(0)("REMARKS") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(myContact) Then
                            ds.Tables("PUBLISHERS").Rows(0)("CONTACT_PERSON") = myContact.ToString.Trim
                        Else
                            ds.Tables("PUBLISHERS").Rows(0)("CONTACT_PERSON") = System.DBNull.Value
                        End If
                        ds.Tables("PUBLISHERS").Rows(0)("UPDATED_BY") = UserCode.Trim
                        ds.Tables("PUBLISHERS").Rows(0)("DATE_MODIFIED") = Now.Date
                        ds.Tables("PUBLISHERS").Rows(0)("IP") = Request.UserHostAddress.Trim

                        thisTransaction = SqlConn.BeginTransaction()
                        da.SelectCommand.Transaction = thisTransaction
                        da.Update(ds, "PUBLISHERS")
                        thisTransaction.Commit()

                        Label6.Visible = True
                        Label6.Text = "Record Updated Successfully"
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
            UpdatePanel1.Update()
            Label7.Text = ""
            Me.bttn_Save.Visible = True
            Me.bttn_Update.Visible = False
        Catch q As SqlException
            thisTransaction.Rollback()
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'delete selected rows
    Protected Sub Delete_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Delete_Bttn.Click
        Try
            For i = 0 To Grid_Pub.Rows.Count - 1
                If DirectCast(Grid_Pub.Rows(i).Cells(0).FindControl("cbd"), CheckBox).Checked Then
                    Dim PubID As Integer = Nothing
                    PubID = Grid_Pub.Rows(i).Cells(5).Text
                    'chk for foreign reference in ACQ tble
                    Dim str As Object = Nothing
                    Dim flag As Object = Nothing
                    str = "SELECT CAT_NO FROM CATS WHERE (PUB_ID ='" & Trim(PubID) & "')"
                    Dim cmd1 As New SqlCommand(str, SqlConn)
                    SqlConn.Open()
                    flag = cmd1.ExecuteScalar
                    SqlConn.Close()
                    If flag <> Nothing Then
                        Label6.Text = "Publisher reference saved..in CATS Table, can not be deleted)"
                    Else
                        'get cat record
                        Dim SQL As String = Nothing
                        SQL = "DELETE FROM PUBLISHERS WHERE (PUB_ID ='" & Trim(PubID) & "') "
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
            UpdatePanel1.Update()
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
End Class