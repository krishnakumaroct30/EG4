Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Security.Cryptography
Imports System.Globalization

Module Module1
    Public SqlConnString As String
    Public SqlConn As SqlConnection
    Public UserCode As String = Nothing
    Public LibCode As Object = Nothing
    Public paneSelectedIndex As Integer = Nothing
    Public myPaneName As Object = Nothing
    Public AcqRetro As Object = Nothing
   

    Public RKLibraryParent As String = Nothing
    Public RKLibraryAddress As String = Nothing
    Public RKLibraryName As String = Nothing
    Public RKTreeviewValue As String = Nothing

    Public Function SConnection() As Boolean
        Try
            SqlConnString = System.Configuration.ConfigurationManager.AppSettings("dbConnString")
            SqlConn = New SqlClient.SqlConnection(SqlConnString)
            SqlConn.Open()
            If SqlConn.State = ConnectionState.Closed Then
                SConnection = False
            Else
                SConnection = True
                SqlConn.Close()
            End If
        Catch ex As SqlException
            SConnection = False
        Finally
            SqlConn.Close()
        End Try
    End Function
    Public str11 As String
    Function RemoveQuotes(ByVal s As Object) As Object
        Dim x, s2 As Object
        s = Trim(s)
        If Len(s) = 0 Then
            RemoveQuotes = ""
            Exit Function
        End If
        s2 = ""
        For x = 1 To Len(s)
            If Mid(s, x, 1) = "'" Then
                s2 = s2 & ""
            Else
                s2 = s2 & Mid(s, x, 1)
            End If
        Next
        RemoveQuotes = s2
    End Function
    'Trim extra spaces between words
    Function TrimAll(ByVal TextIN As String) As String
        TrimAll = Trim(TextIN)
        While InStr(TrimAll, New String(" ", 2)) > 0
            TrimAll = Replace(TrimAll, New String(" ", 2), " ")
        End While
    End Function
    'Trim all spaces between words
    Function TrimX(ByVal TextIN As String) As String
        TrimX = Trim(TextIN)
        While InStr(TrimX, New String(" ", 1)) > 0
            TrimX = Replace(TrimX, New String(" ", 1), "")
        End While
    End Function
    Public Function RandomString(ByVal l) As String
        Dim value = Nothing, i, r
        Randomize()
        For i = 0 To l
            r = Int(Rnd() * 62)
            If r < 10 Then
                r = r + 48
            ElseIf r < 36 Then
                r = (r - 10) + 65
            Else
                r = (r - 10 - 26) + 97
            End If
            value = value & Chr(r)
        Next
        RandomString = value
    End Function
    Function getMD5Hash(ByVal strToHash As String) As String
        Dim md5Obj As New System.Security.Cryptography.MD5CryptoServiceProvider
        Dim bytesToHash() As Byte = System.Text.Encoding.ASCII.GetBytes(strToHash)

        bytesToHash = md5Obj.ComputeHash(bytesToHash)

        Dim strResult As String = ""

        For Each b As Byte In bytesToHash
            strResult += b.ToString("x2")
        Next

        Return strResult
    End Function

    'Validate DATE to DD/MM/YYYY Style
    Function ValidateDate(ByRef datevalue_Renamed As Object) As Short

        'On Error GoTo ErrorHandler

        Dim strYYYY As Object
        Dim strMM As Object
        Dim strDD As Object
        strDD = ""
        strMM = ""
        strYYYY = ""
        Dim MonthPosition As Short
        Dim position, Position2 As Object
        Dim Position3 As Short
        If (Val(datevalue_Renamed) = 0) Or (InStr(1, datevalue_Renamed, "_")) Then
            MsgBox("Please enter a valid date only")
            ValidateDate = 1
            Exit Function
        Else
            position = InStr(1, datevalue_Renamed, "/")
            Position2 = InStr(4, datevalue_Renamed, "/")
            Position3 = InStr(7, datevalue_Renamed, "/")

            If (position = 0) Or (position = 1) Or (position = 2) Or (Position2 = 4) Or (Position2 = 5) Or (Position3 = 7) Then
                'If Not ((position = 3) And (Position2 = 6)) Then
                MsgBox("please enter a valid date")
                ValidateDate = 1
                Exit Function
            Else
                strDD = Left(datevalue_Renamed, position - 1)
                strDD = CShort(strDD)
                MonthPosition = InStr(position + 1, datevalue_Renamed, "/")
                If MonthPosition = 0 Then
                    MsgBox("please enter a valid date")
                    ValidateDate = 1
                    Exit Function
                Else
                    strMM = Mid(datevalue_Renamed, position + 1, MonthPosition - (position + 1))
                    strMM = CShort(strMM)
                    strYYYY = Right(datevalue_Renamed, Len(datevalue_Renamed) - MonthPosition)
                    If strYYYY = "" Or InStr(1, strYYYY, "/") Then
                        MsgBox("Please Enter Valid year")
                        ValidateDate = 1
                        Exit Function
                    Else
                        If strDD > 31 Then
                            MsgBox("please enter a valid day")
                            ValidateDate = 1
                            Exit Function
                        Else
                            If strMM > 12 Then
                                MsgBox("please enter a valid month")
                                ValidateDate = 1
                                Exit Function
                            Else
                                If Len(strYYYY) <> 4 Then
                                    MsgBox("please enter a valid year")
                                    ValidateDate = 1
                                    Exit Function
                                Else
                                    If ((strMM = 1) Or (strMM = 3) Or (strMM = 5) Or (strMM = 7) Or (strMM = 8) Or (strMM = 10) Or (strMM = 12)) And (strDD > 31) Then
                                        MsgBox("please enter a valid date")
                                        ValidateDate = 1
                                    End If

                                    If ((strMM = 4) Or (strMM = 6) Or (strMM = 9) Or (strMM = 11)) And (strDD > 30) Then
                                        MsgBox("please enter a valid date")
                                        ValidateDate = 1
                                    End If

                                    If strMM = 2 And (strYYYY Mod 4) <> 0 And strDD > 28 Then
                                        MsgBox("please enter a valid date")
                                        ValidateDate = 1
                                    End If

                                    If strMM = 2 And (strYYYY Mod 4) = 0 And strDD > 29 Then
                                        MsgBox("please enter a valid date")
                                        ValidateDate = 1
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        End If
    End Function

    Public Function datechk(ByVal datestring As String)
        Try

            Dim val As String = ""
            val = datestring

            val = Replace(Trim(val.ToString), ".", "/")
            val = Replace(Trim(val.ToString), "-", "/")
            val = Replace(Trim(val.ToString), "_", "/")
            val = Replace(Trim(val.ToString), "\", "/")

            Dim dat() As String = {"d", "dd", "ddd", "dddd"}
            Dim mon() As String = {"M", "MM", "MMM", "MMMM"}
            Dim yr() As String = {"y", "yy", "yyy", "yyyy", "yyyyy"}

            Dim d, m, y, flag As Integer
            d = 0
            m = 0
            y = 0
            flag = 0

            Dim dc As DateTime

            'dMy
            For d = 0 To dat.Length - 1
                For m = 0 To mon.Length - 1
                    For y = 0 To yr.Length - 1
                        Dim dmy As String = ""
                        dmy = dat(d) + "/" + mon(m) + "/" + yr(y)
                        If Date.TryParseExact(val.ToString, dmy, New Globalization.CultureInfo("en-US"), DateTimeStyles.None, dc) Then
                            val = dc
                            flag = 1
                            Return val
                            Exit Function
                        End If
                    Next
                Next
            Next

            'dyM
            If flag <> 1 Then
                For d = 0 To dat.Length - 1
                    For y = 0 To yr.Length - 1
                        For m = 0 To mon.Length - 1
                            Dim dmy As String = ""
                            dmy = dat(d) + "/" + yr(y) + "/" + mon(m)
                            If Date.TryParseExact(val.ToString, dmy, New Globalization.CultureInfo("en-US"), DateTimeStyles.None, dc) Then
                                val = dc
                                flag = 1
                                Return val
                                Exit Function
                            End If
                        Next
                    Next
                Next
            End If

            'Mdy
            If flag <> 1 Then
                For m = 0 To mon.Length - 1
                    For d = 0 To dat.Length - 1
                        For y = 0 To yr.Length - 1
                            Dim dmy As String = ""
                            dmy = mon(m) + "/" + dat(d) + "/" + yr(y)
                            If Date.TryParseExact(val.ToString, dmy, New Globalization.CultureInfo("en-US"), DateTimeStyles.None, dc) Then
                                val = dc
                                flag = 1
                                Return val
                                Exit Function
                            End If
                        Next
                    Next
                Next
            End If

            'Myd
            If flag <> 1 Then
                For m = 0 To mon.Length - 1
                    For y = 0 To yr.Length - 1
                        For d = 0 To dat.Length - 1
                            Dim dmy As String = ""
                            dmy = mon(m) + "/" + yr(y) + "/" + dat(d)
                            If Date.TryParseExact(val.ToString, dmy, New Globalization.CultureInfo("en-US"), DateTimeStyles.None, dc) Then
                                val = dc
                                flag = 1
                                Return val
                                Exit Function
                            End If
                        Next
                    Next
                Next
            End If

            'ydM
            If flag <> 1 Then
                For y = 0 To yr.Length - 1
                    For d = 0 To dat.Length - 1
                        For m = 0 To mon.Length - 1
                            Dim dmy As String = ""
                            dmy = yr(y) + "/" + dat(d) + "/" + mon(m)
                            If Date.TryParseExact(val.ToString, dmy, New Globalization.CultureInfo("en-US"), DateTimeStyles.None, dc) Then
                                val = dc
                                flag = 1
                                Return val
                                Exit Function
                            End If
                        Next
                    Next
                Next
            End If

            'yMd
            If flag <> 1 Then
                For y = 0 To yr.Length - 1
                    For m = 0 To mon.Length - 1
                        For d = 0 To dat.Length - 1
                            Dim dmy As String = ""
                            dmy = yr(y) + "/" + mon(m) + "/" + dat(d)
                            If Date.TryParseExact(val.ToString, dmy, New Globalization.CultureInfo("en-US"), DateTimeStyles.None, dc) Then
                                val = dc
                                flag = 1
                                Return val
                                Exit Function
                            End If
                        Next
                    Next
                Next
            End If
            If flag = 0 Then
                val = Now
            End If
            Return val
        Catch ex As Exception
            MsgBox(ex.Message)
            Return Now
        End Try
        'Dim dat1 As Date
        'dat1 = Date.Parse(val.ToString)
        'val = dat1.ToString("MM/dd/yyyy")
        'Dim [date] As String = val.ToString  'format is MM/dd/yyyy

        'Dim dateTimeFormatterProvider As System.Globalization.DateTimeFormatInfo = TryCast(System.Globalization.DateTimeFormatInfo.CurrentInfo.Clone(), System.Globalization.DateTimeFormatInfo)

        'dateTimeFormatterProvider.ShortDatePattern = "dd/MM/yyyy"

        ' ''source date format

        'Dim dateTime As DateTime = Date.Parse([date], dateTimeFormatterProvider)

        'Dim formatted As String = dateTime.ToString("MM/dd/yyyy")
        'val = formatted

        'Dim nd1 As String
        'Dim nd2 As Date
        'nd2 = DateTime.Parse(Trim(val.ToString), Globalization.CultureInfo.CreateSpecificCulture("en-CA"))
        'nd1 = nd2.ToString("MM/dd/yyyy")

        '    Dim dateString As String = Trim(Val.ToString)
        '    'Dim formats As String = "MM/dd/yyyy"
        '    Dim dateValue As DateTime

        '    If Date.TryParseExact(dateString, "MM/dd/yyyy", New Globalization.CultureInfo("en-US"), DateTimeStyles.None, dateValue) Then
        '        Val = dateValue
        '        cdr("DATE_ADDED") = Val()
        '    ElseIf Date.TryParseExact(dateString, "M/d/yyyy", New Globalization.CultureInfo("en-US"), DateTimeStyles.None, dateValue) Then
        '        Val = dateValue
        '        cdr("DATE_ADDED") = Val()
        '    ElseIf Date.TryParseExact(dateString, "M/d/yy", New Globalization.CultureInfo("en-US"), DateTimeStyles.None, dateValue) Then
        '        Val = dateValue
        '        cdr("DATE_ADDED") = Val()
        '    ElseIf Date.TryParseExact(dateString, "MM/dd/yy", New Globalization.CultureInfo("en-US"), DateTimeStyles.None, dateValue) Then
        '        Val = dateValue
        '        cdr("DATE_ADDED") = Val()
        '    ElseIf Date.TryParseExact(dateString, "dd/MMM/yy", New Globalization.CultureInfo("en-US"), DateTimeStyles.None, dateValue) Then
        '        Val = dateValue
        '        cdr("DATE_ADDED") = Val()
        '    ElseIf Date.TryParseExact(dateString, "yyyy/MM/dd", New Globalization.CultureInfo("en-US"), DateTimeStyles.None, dateValue) Then
        '        Val = dateValue
        '        cdr("DATE_ADDED") = Val()
        '    ElseIf Date.TryParseExact(dateString, "yy/MM/dd", New Globalization.CultureInfo("en-US"), DateTimeStyles.None, dateValue) Then
        '        Val = dateValue
        '        cdr("DATE_ADDED") = Val()
        '    Else
        '        Dim nd1 As String
        '        Dim nd2 As Date
        '        nd2 = DateTime.Parse(Trim(Val.ToString), Globalization.CultureInfo.CreateSpecificCulture("en-CA"))
        '        nd1 = nd2.ToString("MM/dd/yyyy")
        '        Val = nd1
        '        cdr("DATE_ADDED") = Val()
        '    End If

        'Else
        '    cdr("DATE_ADDED") = Now
        'End If

        '                        Else
        'cdr("DATE_ADDED") = Now
        '                        End If
    End Function

End Module
