Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Partial Class Feedback
    Inherits System.Web.UI.Page

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session.Item("IsThisU") <> Nothing Then
                If Not SConnection() = True Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Database Connection is lost..Try Again !');", True)
                Else
                    TxtName.Focus()
                End If
            End If
        Catch s As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Page Load Error - Feedback ..');", True)
        End Try
    End Sub
    Protected Sub Submit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Submit.Click
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing
        Try
            'check unwanted characters
            Dim iloop As Integer
            Dim strcurrentchar As Object
            Dim c As Integer
            Dim counter1, counter2, counter3, counter4, counter5, counter6, counter7, counter8, counter9, counter10, counter11 As Integer

            Dim loginCAPTCHA As WebControlCaptcha.CaptchaControl = Me.CAPTCHA

            If loginCAPTCHA.UserValidated = False Then
                ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Captcha Value is not Valid!');", True)
                Me.CAPTCHA.Focus()
                Exit Sub
            End If

            Dim Names As String = Nothing
            If TxtName.Text <> "" Then
                Names = TrimAll(TxtName.Text)
                Names = RemoveQuotes(Names)
                If Names.Length > 200 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Name must be of Proper Length... ');", True)
                    TxtName.Focus()
                    Exit Sub
                End If
                Names = " " & Names & " "
                If InStr(1, Names, "CREATE", 1) > 0 Or InStr(1, Names, "DELETE", 1) > 0 Or InStr(1, Names, "DROP", 1) > 0 Or InStr(1, Names, "INSERT", 1) > 1 Or InStr(1, Names, "TRACK", 1) > 1 Or InStr(1, Names, "TRACE", 1) > 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Reserve Word... ');", True)
                    TxtName.Focus()
                    Exit Sub
                End If

                Names = TrimAll(Names)

                c = 0
                counter1 = 0
                For iloop = 1 To Len(Names)
                    strcurrentchar = Mid(Names, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter1 = 1
                        End If
                    End If
                Next
                If counter1 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Un-Wated Characters... ');", True)
                    TxtName.Focus()
                    Exit Sub
                End If

            Else
                ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Name is Mandatory!');", True)
                TxtName.Focus()
                Exit Sub
            End If
            '****************************************************************************************
            'Server Validation for Designation
            Dim Desig As String = Nothing
            If TxtDesig.Text <> "" Then
                Desig = TrimAll(TxtDesig.Text)
                Desig = RemoveQuotes(Desig)
                If Desig.Length > 150 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Length of Designation is not Proper... ');", True)
                    Me.TxtDesig.Focus()
                    Exit Sub
                End If
                Desig = " " & Desig & " "
                If InStr(1, Desig, "CREATE", 1) > 0 Or InStr(1, Desig, "DELETE", 1) > 0 Or InStr(1, Desig, "DROP", 1) > 0 Or InStr(1, Desig, "INSERT", 1) > 1 Or InStr(1, Desig, "TRACK", 1) > 1 Or InStr(1, Desig, "TRACE", 1) > 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do not use reserve words... ');", True)
                    Me.TxtDesig.Focus()
                    Exit Sub
                End If

                Desig = TrimAll(Desig)
                'check unwanted characters
                c = 0
                counter2 = 0
                For iloop = 1 To Len(Desig)
                    strcurrentchar = Mid(Desig, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter2 = 1
                        End If
                    End If
                Next

                If counter2 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Un-wanted Characters... ');", True)
                    Me.TxtDesig.Focus()
                    Exit Sub
                End If
            Else
                Desig = Nothing
            End If


            '****************************************************************************************
            'Server Validation for Organization
            Dim Organization As String = Nothing
            If TxtOrg.Text <> "" Then
                Organization = TrimAll(TxtOrg.Text)
                Organization = RemoveQuotes(Organization)
                If Organization.Length > 250 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Length of Org is not proper... ');", True)
                    Me.TxtOrg.Focus()
                    Exit Sub
                End If

                Organization = " " & Organization & " "
                If InStr(1, Organization, "CREATE", 1) > 0 Or InStr(1, Organization, "DELETE", 1) > 0 Or InStr(1, Organization, "DROP", 1) > 0 Or InStr(1, Organization, "INSERT", 1) > 1 Or InStr(1, Organization, "TRACK", 1) > 1 Or InStr(1, Organization, "TRACE", 1) > 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Reserve Words!');", True)
                    Me.TxtOrg.Focus()
                    Exit Sub
                End If
                Organization = TrimAll(Organization)
                'check unwanted characters
                c = 0
                counter3 = 0
                For iloop = 1 To Len(Organization)
                    strcurrentchar = Mid(Organization, iloop, 1)
                    If c = 0 Then
                        If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter3 = 1
                        End If
                    End If
                Next
                If counter3 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not un-wanted Characters!');", True)
                    Me.TxtOrg.Focus()
                    Exit Sub
                End If
            Else
                ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Organization Name is mandatory!');", True)
                Me.TxtOrg.Focus()
                Exit Sub
            End If

            '****************************************************************************************
            'Server Validation for Office Address
            Dim OfficeAdd As String = Nothing
            If TxtOfficeAdd.Text <> "" Then
                OfficeAdd = TrimAll(TxtOfficeAdd.Text)
                OfficeAdd = RemoveQuotes(OfficeAdd)
                If OfficeAdd.Length > 250 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Off. Address Length is not proper!');", True)
                    Me.TxtOfficeAdd.Focus()
                    Exit Sub
                End If

                OfficeAdd = " " & OfficeAdd & " "
                If InStr(1, OfficeAdd, "CREATE", 1) > 0 Or InStr(1, OfficeAdd, "DELETE", 1) > 0 Or InStr(1, OfficeAdd, "DROP", 1) > 0 Or InStr(1, OfficeAdd, "INSERT", 1) > 1 Or InStr(1, OfficeAdd, "TRACK", 1) > 1 Or InStr(1, OfficeAdd, "TRACE", 1) > 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Reserve Words!');", True)
                    Me.TxtOfficeAdd.Focus()
                    Exit Sub
                End If
                OfficeAdd = TrimAll(OfficeAdd)
                'check unwanted characters
                c = 0
                counter4 = 0
                For iloop = 1 To Len(OfficeAdd)
                    strcurrentchar = Mid(OfficeAdd, iloop, 1)
                    If c = 0 Then
                        If Not InStr("'<>=%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter4 = 1
                        End If
                    End If
                Next
                If counter4 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Un-Wanted Characters!');", True)
                    Me.TxtOfficeAdd.Focus()
                    Exit Sub
                End If
            Else
                Organization = Nothing
            End If

            '****************************************************************************************
            'Server Validation for Residential Address
            Dim ResAdd As String = Nothing
            If TxtResidentialAdd.Text <> "" Then
                ResAdd = TrimAll(TxtResidentialAdd.Text)
                ResAdd = RemoveQuotes(ResAdd)
                If ResAdd.Length > 250 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Res.Add length is not Proper!');", True)
                    Me.TxtResidentialAdd.Focus()
                    Exit Sub
                End If
                ResAdd = " " & ResAdd & " "
                If InStr(1, ResAdd, "CREATE", 1) > 0 Or InStr(1, ResAdd, "DELETE", 1) > 0 Or InStr(1, ResAdd, "DROP", 1) > 0 Or InStr(1, ResAdd, "INSERT", 1) > 1 Or InStr(1, ResAdd, "TRACK", 1) > 1 Or InStr(1, ResAdd, "TRACE", 1) > 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Reserve Words!');", True)
                    Me.TxtResidentialAdd.Focus()
                    Exit Sub
                End If
                ResAdd = TrimAll(ResAdd)
                'check unwanted characters
                c = 0
                counter5 = 0
                For iloop = 1 To Len(ResAdd)
                    strcurrentchar = Mid(ResAdd, iloop, 1)
                    If c = 0 Then
                        If Not InStr("'<>=%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter5 = 1
                        End If
                    End If
                Next
                If counter5 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Un-Wanted Characters!');", True)
                    Me.TxtResidentialAdd.Focus()
                    Exit Sub
                End If
            Else
                ResAdd = String.Empty
            End If

            '****************************************************************************************
            'Server Validation for Office Phone Number
            Dim OfficePhone As String = Nothing
            If TxtOfficePhone.Text <> "" Then
                OfficePhone = TrimAll(TxtOfficePhone.Text)
                OfficePhone = RemoveQuotes(OfficePhone)
                If OfficePhone.Length > 150 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input Length is not Proper... ');", True)
                    Me.TxtOfficePhone.Focus()
                    Exit Sub
                End If
                OfficePhone = " " & OfficePhone & " "
                If InStr(1, OfficePhone, "CREATE", 1) > 0 Or InStr(1, OfficePhone, "DELETE", 1) > 0 Or InStr(1, OfficePhone, "DROP", 1) > 0 Or InStr(1, OfficePhone, "INSERT", 1) > 1 Or InStr(1, OfficePhone, "TRACK", 1) > 1 Or InStr(1, OfficePhone, "TRACE", 1) > 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                    Me.TxtOfficePhone.Focus()
                    Exit Sub
                End If
                OfficePhone = TrimAll(OfficePhone)
                'check unwanted characters
                c = 0
                counter6 = 0
                For iloop = 1 To Len(OfficePhone)
                    strcurrentchar = Mid(OfficePhone, iloop, 1)
                    If c = 0 Then
                        If Not InStr("'<>=%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter6 = 1
                        End If
                    End If
                Next
                If counter6 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Un-Wanted Words!');", True)
                    Me.TxtOfficePhone.Focus()
                    Exit Sub
                End If
            Else
                OfficePhone = String.Empty
            End If


            '****************************************************************************************
            'Server Validation for Residential Phone Number
            Dim ResPhone As String = Nothing
            If TxtResidentialPhone.Text <> "" Then
                ResPhone = TrimAll(TxtResidentialPhone.Text)
                ResPhone = RemoveQuotes(ResPhone)
                If ResPhone.Length > 150 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input Length is not Proper!');", True)
                    Me.TxtResidentialPhone.Focus()
                    Exit Sub
                End If
                ResPhone = " " & ResPhone & " "
                If InStr(1, ResPhone, "CREATE", 1) > 0 Or InStr(1, ResPhone, "DELETE", 1) > 0 Or InStr(1, ResPhone, "DROP", 1) > 0 Or InStr(1, ResPhone, "INSERT", 1) > 1 Or InStr(1, ResPhone, "TRACK", 1) > 1 Or InStr(1, ResPhone, "TRACE", 1) > 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Reserve Words!');", True)
                    Me.TxtResidentialPhone.Focus()
                    Exit Sub
                End If
                ResPhone = TrimAll(ResPhone)
                'check unwanted characters
                c = 0
                counter7 = 0
                For iloop = 1 To Len(ResPhone)
                    strcurrentchar = Mid(ResPhone, iloop, 1)
                    If c = 0 Then
                        If Not InStr("'<>=%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter7 = 1
                        End If
                    End If
                Next
                If counter7 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                    Me.TxtResidentialPhone.Focus()
                    Exit Sub
                End If
            Else
                ResPhone = String.Empty
            End If

            '****************************************************************************************
            'Server Validation for Mobile Number
            Dim Mobno As String = Nothing
            If TxtMobileNo.Text <> "" Then
                Mobno = TrimAll(TxtMobileNo.Text)
                Mobno = RemoveQuotes(Mobno)
                If Mobno.Length > 150 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input Length is not Proper!');", True)
                    Me.TxtMobileNo.Focus()
                    Exit Sub
                End If
                Mobno = " " & Mobno & " "
                If InStr(1, Mobno, "CREATE", 1) > 0 Or InStr(1, Mobno, "DELETE", 1) > 0 Or InStr(1, Mobno, "DROP", 1) > 0 Or InStr(1, Mobno, "INSERT", 1) > 1 Or InStr(1, Mobno, "TRACK", 1) > 1 Or InStr(1, Mobno, "TRACE", 1) > 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Reserve Words!');", True)
                    Me.TxtMobileNo.Focus()
                    Exit Sub
                End If
                Mobno = TrimAll(Mobno)
                'check unwanted characters
                c = 0
                counter8 = 0
                For iloop = 1 To Len(Mobno)
                    strcurrentchar = Mid(Mobno, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';<>=%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter8 = 1
                        End If
                    End If
                Next
                If counter8 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                    Me.TxtMobileNo.Focus()
                    Exit Sub
                End If
            Else
                Mobno = String.Empty
            End If

            '****************************************************************************************
            'Server Validation for E-Mail Address
            Dim email As String = Nothing
            If TxtEmailID.Text <> "" Then
                email = TrimX(TxtEmailID.Text)
                email = RemoveQuotes(email)
                If email.Length > 250 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input Length is not Proper!');", True)
                    Me.TxtMobileNo.Focus()
                    Exit Sub
                End If
                email = " " & email & " "
                If InStr(1, email, "CREATE", 1) > 0 Or InStr(1, email, "DELETE", 1) > 0 Or InStr(1, email, "DROP", 1) > 0 Or InStr(1, email, "INSERT", 1) > 1 Or InStr(1, email, "TRACK", 1) > 1 Or InStr(1, email, "TRACE", 1) > 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do Not Use Reserve Words!');", True)
                    Me.TxtMobileNo.Focus()
                    Exit Sub
                End If
                email = TrimX(email)
                'check unwanted characters
                c = 0
                counter9 = 0
                For iloop = 1 To Len(email)
                    strcurrentchar = Mid(email, iloop, 1)
                    If c = 0 Then
                        If Not InStr("'<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter9 = 1
                        End If
                    End If
                Next
                If counter9 = 1 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                    Me.TxtMobileNo.Focus()
                    Exit Sub
                End If
            Else
                ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Email is mandatory!');", True)
                Me.TxtMobileNo.Focus()
                Exit Sub
            End If

            '****************************************************************************************
            'Server Validation for COMMENTS
            Dim Comments As Object = Nothing
            If TxtComments.Text <> "" Then
                Comments = TrimAll(TxtComments.Text)
                Comments = RemoveQuotes(Comments)
                If Comments.ToString.Length > 1001 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input Length is not Proper!');", True)
                    Me.TxtComments.Focus()
                    Exit Sub
                End If

            Else
                ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Plz Enter your Comments!');", True)
                Me.TxtComments.Focus()
                Exit Sub
            End If

            '****************************************************************************************
            'Server Validation for IP ADDRESS
            Dim IP As String
            IP = Request.UserHostAddress

            '**************************************************************************************************
            'INSERT THE RECORD IN TO THE DATABASE
            Dim SQL As String
            Dim Cmd As SqlCommand
            Dim da As SqlDataAdapter
            Dim ds As New DataSet
            Dim CB As SqlCommandBuilder
            Dim dtrow As DataRow
            SQL = "SELECT * FROM FEEDBACKS WHERE (ID = '0')"
            Cmd = New SqlCommand(SQL, SqlConn)
            da = New SqlDataAdapter(Cmd)
            CB = New SqlCommandBuilder(da)
            da.Fill(ds, "Feedbacks")
            dtrow = ds.Tables("FeedBacks").NewRow
            If Names <> "" Then
                dtrow("NAME") = Names.Trim
            End If

            If Not String.IsNullOrEmpty(Desig) Then
                dtrow("Designation") = Desig.Trim
            Else
                dtrow("Designation") = System.DBNull.Value
            End If

            If Not String.IsNullOrEmpty(Organization) Then
                dtrow("ORGANIZATION") = Organization.Trim
            Else
                dtrow("ORGANIZATION") = System.DBNull.Value
            End If

            If Not String.IsNullOrEmpty(OfficeAdd) Then
                dtrow("ADD_OFF") = OfficeAdd.Trim
            Else
                dtrow("ADD_OFF") = System.DBNull.Value
            End If

            If Not String.IsNullOrEmpty(ResAdd) Then
                dtrow("ADD_RES") = ResAdd.Trim
            Else
                dtrow("ADD_RES") = System.DBNull.Value
            End If

            If Not String.IsNullOrEmpty(OfficePhone) Then
                dtrow("PHONE_O") = OfficePhone.Trim
            Else
                dtrow("PHONE_O") = System.DBNull.Value
            End If

            If Not String.IsNullOrEmpty(ResPhone) Then
                dtrow("PHONE_R") = ResPhone.Trim
            Else
                dtrow("PHONE_R") = System.DBNull.Value
            End If

            If Not String.IsNullOrEmpty(Mobno) Then
                dtrow("MOBILE") = Mobno.Trim
            Else
                dtrow("MOBILE") = System.DBNull.Value
            End If

            If Not String.IsNullOrEmpty(email) Then
                dtrow("EMAIL") = email.Trim
            Else
                dtrow("EMAIL") = System.DBNull.Value
            End If

            If Not String.IsNullOrEmpty(Comments) Then
                dtrow("COMMENTS") = Comments.Trim
            Else
                dtrow("COMMENTS") = System.DBNull.Value
            End If

            dtrow("DATE_ADDED") = Now.ToShortDateString
            dtrow("TIME_ADDED") = Now.ToLongTimeString
            dtrow("IP") = IP.Trim
            dtrow("SHOW") = "N"
            ds.Tables("FeedBacks").Rows.Add(dtrow)

            SqlConn.Open()

            thisTransaction = SqlConn.BeginTransaction()
            da.SelectCommand.Transaction = thisTransaction
            da.Update(ds, "Feedbacks")
            thisTransaction.Commit()
            ClearControl()
            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Thanks for Submitting Feedback... ');", True)
            Me.TxtName.Focus()
        Catch q As SqlException
            thisTransaction.Rollback()
        Catch s As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' there is error in saving record ..');", True)
        Finally
            SqlConn.Close()
        End Try
    End Sub
    Public Sub ClearControl()
        TxtName.Text = String.Empty
        TxtDesig.Text = String.Empty
        TxtOrg.Text = String.Empty
        TxtOfficeAdd.Text = String.Empty
        TxtResidentialAdd.Text = String.Empty
        TxtOfficePhone.Text = String.Empty
        TxtResidentialPhone.Text = String.Empty
        TxtMobileNo.Text = String.Empty
        TxtEmailID.Text = String.Empty
        TxtComments.Text = String.Empty
    End Sub
    Protected Sub Reset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Reset.Click
        ClearControl()
        Me.TxtName.Focus()
    End Sub




End Class
