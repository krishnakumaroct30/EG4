Imports System.Data
Imports System.Data.SqlClient
Imports AjaxControlToolkit
Imports System.IO
Imports System.Xml
Imports System.Web
Imports System.Web.UI
Imports Microsoft.VisualBasic
Imports System.Configuration
Imports System.ComponentModel
Imports EG4.Module1
Public Class PopulateCombo

    Private Shared _viewState As SqlDataReader

    Private Shared Property ViewState(ByVal p1 As String) As SqlDataReader
        Get
            Return _viewState
        End Get
        Set(ByVal value As SqlDataReader)
            _viewState = value
        End Set
    End Property

    Public Shared Function GetPublisherList() As IEnumerable
        Dim Sel As String = "SELECT PUB_ID, PUB_NAME FROM PUBLISHERS ORDER BY PUB_NAME"
        Dim cmd As New SqlCommand(Sel, SqlConn)
        Dim rdr As SqlDataReader = Nothing
        Try
            SqlConn.Open()
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            Return (rdr)
        Catch ex As System.Exception
            If rdr IsNot Nothing Then
                rdr.Close()
            Else
                SqlConn.Close()
            End If
            ex.Message.ToString()
        End Try
    End Function
    Public Shared Function GetVendorList() As IEnumerable
        Dim Sel As String = "SELECT VEND_ID, VEND_NAME FROM VENDORS ORDER BY VEND_NAME"
        Dim cmd As New SqlCommand(Sel, SqlConn)
        Dim rdr As SqlDataReader = Nothing
        Try
            SqlConn.Open()
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            Return (rdr)
        Catch ex As System.Exception
            If rdr IsNot Nothing Then
                rdr.Close()
            Else
                SqlConn.Close()
            End If
            ex.Message.ToString()
        End Try
    End Function
    Public Shared Function GetBibLevelist() As IEnumerable
        Dim Sel As String = "SELECT  BIB_CODE, BIB_NAME FROM BIB_LEVELS ORDER BY BIB_NAME "
        Dim cmd As New SqlCommand(Sel, SqlConn)
        Dim rdr As SqlDataReader = Nothing
        Try
            SqlConn.Open()
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            Return (rdr)
        Catch ex As System.Exception
            If rdr IsNot Nothing Then
                rdr.Close()
            Else
                SqlConn.Close()
            End If
            ex.Message.ToString()
        End Try
    End Function
    Public Shared Function GetMaterialsList() As IEnumerable
        Dim Sel As String = "SELECT  MAT_CODE, MAT_NAME FROM MATERIALS ORDER BY MAT_NAME "
        Dim cmd As New SqlCommand(Sel, SqlConn)
        Dim rdr As SqlDataReader = Nothing
        Try
            SqlConn.Open()
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            Return (rdr)
        Catch ex As System.Exception
            If rdr IsNot Nothing Then
                rdr.Close()
            Else
                SqlConn.Close()
            End If
            ex.Message.ToString()
        End Try
    End Function
    Public Shared Function GetDocTypeList() As IEnumerable
        Dim Sel As String = "SELECT  DOC_TYPE_CODE, DOC_TYPE_NAME FROM DOC_TYPES ORDER BY DOC_TYPE_NAME "
        Dim cmd As New SqlCommand(Sel, SqlConn)
        Dim rdr As SqlDataReader = Nothing
        Try
            SqlConn.Open()
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            Return (rdr)
        Catch ex As System.Exception
            If rdr IsNot Nothing Then
                rdr.Close()
            Else
                SqlConn.Close()
            End If
            ex.Message.ToString()
        End Try
    End Function

    Public Shared Function GetLanguageList() As IEnumerable
        Dim Sel As String = "SELECT  LANG_CODE, LANG_NAME FROM LANGUAGES ORDER BY LANG_NAME "
        Dim cmd As New SqlCommand(Sel, SqlConn)
        Dim rdr As SqlDataReader = Nothing
        Try
            SqlConn.Open()
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            Return (rdr)
        Catch ex As System.Exception
            If rdr IsNot Nothing Then
                rdr.Close()
            Else
                SqlConn.Close()
            End If
            ex.Message.ToString()
        End Try
    End Function
    Public Shared Function GetCountryList() As IEnumerable
        Dim Sel As String = "SELECT  CON_CODE, CON_NAME FROM COUNTRIES ORDER BY CON_NAME "
        Dim cmd As New SqlCommand(Sel, SqlConn)
        Dim rdr As SqlDataReader = Nothing
        Try
            SqlConn.Open()
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            Return (rdr)
        Catch ex As System.Exception
            If rdr IsNot Nothing Then
                rdr.Close()
            Else
                SqlConn.Close()
            End If
            ex.Message.ToString()
        End Try
    End Function
    Public Shared Function GetSubjectList() As IEnumerable
        Dim Sel As String = "SELECT  SUB_ID, SUB_NAME, SUB_KEYWORDS FROM SUBJECTS ORDER BY SUB_NAME "
        Dim cmd As New SqlCommand(Sel, SqlConn)
        Dim rdr As SqlDataReader = Nothing
        Try
            SqlConn.Open()
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            Return (rdr)
        Catch ex As System.Exception
            If rdr IsNot Nothing Then
                rdr.Close()
            Else
                SqlConn.Close()
            End If
            ex.Message.ToString()
        End Try
    End Function
    Public Shared Function GetCurrenciesList() As IEnumerable
        Dim Sel As String = "SELECT  CUR_CODE, CUR_NAME FROM CURRENCIES ORDER BY CUR_NAME "
        Dim cmd As New SqlCommand(Sel, SqlConn)
        Dim rdr As SqlDataReader = Nothing
        Try
            SqlConn.Open()
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            Return (rdr)
        Catch ex As System.Exception
            If rdr IsNot Nothing Then
                rdr.Close()
            Else
                SqlConn.Close()
            End If
            ex.Message.ToString()
        End Try
    End Function
    Public Shared Function GetFormatList() As IEnumerable
        Dim Sel As String = "SELECT  FORMAT_CODE, FORMAT_NAME FROM PHYSICAL_FORMATS ORDER BY FORMAT_NAME "
        Dim cmd As New SqlCommand(Sel, SqlConn)
        Dim rdr As SqlDataReader = Nothing
        Try
            SqlConn.Open()
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            Return (rdr)
        Catch ex As System.Exception
            If rdr IsNot Nothing Then
                rdr.Close()
            Else
                SqlConn.Close()
            End If
            ex.Message.ToString()
        End Try
    End Function
    Public Shared Function GetCopyStatusList() As IEnumerable
        Dim Sel As String = "SELECT  STA_CODE, STA_NAME FROM BOOKSTATUS ORDER BY STA_NAME "
        Dim cmd As New SqlCommand(Sel, SqlConn)
        Dim rdr As SqlDataReader = Nothing
        Try
            SqlConn.Open()
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            Return (rdr)
        Catch ex As System.Exception
            If rdr IsNot Nothing Then
                rdr.Close()
            Else
                SqlConn.Close()
            End If
            ex.Message.ToString()
        End Try
    End Function
    Public Shared Function GetBindingList() As IEnumerable
        Dim Sel As String = "SELECT  BIND_CODE, BIND_NAME FROM BINDINGS ORDER BY BIND_NAME "
        Dim cmd As New SqlCommand(Sel, SqlConn)
        Dim rdr As SqlDataReader = Nothing
        Try
            SqlConn.Open()
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            Return (rdr)
        Catch ex As System.Exception
            If rdr IsNot Nothing Then
                rdr.Close()
            Else
                SqlConn.Close()
            End If
            ex.Message.ToString()
        End Try
    End Function
    Public Shared Function GetAccMaterialsList() As IEnumerable
        Dim Sel As String = "SELECT  ACC_MAT_CODE, ACC_MAT_NAME FROM ACC_MATERIALS ORDER BY ACC_MAT_NAME "
        Dim cmd As New SqlCommand(Sel, SqlConn)
        Dim rdr As SqlDataReader = Nothing
        Try
            SqlConn.Open()
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            Return (rdr)
        Catch ex As System.Exception
            If rdr IsNot Nothing Then
                rdr.Close()
            Else
                SqlConn.Close()
            End If
            ex.Message.ToString()
        End Try
    End Function
    Public Shared Function GetAcqModesList() As IEnumerable
        Dim Sel As String = "SELECT  ACQMODE_CODE, ACQMODE_NAME FROM ACQMODES ORDER BY ACQMODE_NAME "
        Dim cmd As New SqlCommand(Sel, SqlConn)
        Dim rdr As SqlDataReader = Nothing
        Try
            SqlConn.Open()
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            Return (rdr)
        Catch ex As System.Exception
            If rdr IsNot Nothing Then
                rdr.Close()
            Else
                SqlConn.Close()
            End If
            ex.Message.ToString()
        End Try
    End Function
    Public Shared Function GetFreqList() As IEnumerable
        Dim Sel As String = "SELECT  FREQ_CODE, FREQ_NAME FROM FREQUENCIES ORDER BY FREQ_NAME "
        Dim cmd As New SqlCommand(Sel, SqlConn)
        Dim rdr As SqlDataReader = Nothing
        Try
            SqlConn.Open()
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            Return (rdr)
        Catch ex As System.Exception
            If rdr IsNot Nothing Then
                rdr.Close()
            Else
                SqlConn.Close()
            End If
            ex.Message.ToString()
        End Try
    End Function
    'publish approval list for ACQ_Manage Approval - 2nd TAB
    Public Shared Function GetApprovalList(ByVal LibCode As Object) As IEnumerable
        Dim Sel As String = "SELECT DISTINCT APP_NO FROM APPROVAL_VIEW2 WHERE (PROCESS_STATUS = 'Requested' or PROCESS_STATUS = 'Sent For Approval') AND (LIB_CODE ='" & (LibCode) & "') AND  (BIB_CODE = 'S')  ORDER BY APP_NO DESC"
        Dim cmd As New SqlCommand(Sel, SqlConn)
        Dim rdr As SqlDataReader = Nothing
        Try
            SqlConn.Open()
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            Return (rdr)
        Catch ex As System.Exception
            If rdr IsNot Nothing Then
                rdr.Close()
            Else
                SqlConn.Close()
            End If
            ex.Message.ToString()
        End Try
    End Function
    'publish approval list for ACQ_Manage Approval - 2nd TAB for Books
    Public Shared Function GetApprovalList2(ByVal LibCode As Object) As IEnumerable
        Dim Sel As String = "SELECT DISTINCT APP_NO FROM APPROVAL_VIEW2 WHERE (PROCESS_STATUS = 'Requested' or PROCESS_STATUS = 'Sent For Approval') AND (LIB_CODE ='" & (LibCode) & "') AND  (BIB_CODE = 'M')  ORDER BY APP_NO DESC"
        Dim cmd As New SqlCommand(Sel, SqlConn)
        Dim rdr As SqlDataReader = Nothing
        Try
            SqlConn.Open()
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            Return (rdr)
        Catch ex As System.Exception
            If rdr IsNot Nothing Then
                rdr.Close()
            Else
                SqlConn.Close()
            End If
            ex.Message.ToString()
        End Try
    End Function

    Public Shared Function GetCommitteeList(ByVal LibCode As Object) As IEnumerable
        Dim Sel As String = "SELECT COM_CODE, COM_NAME FROM COMMITTEES WHERE  (LIB_CODE ='" & (LibCode) & "') "
        Dim cmd As New SqlCommand(Sel, SqlConn)
        Dim rdr As SqlDataReader = Nothing
        Try
            SqlConn.Open()
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            Return (rdr)
        Catch ex As System.Exception
            If rdr IsNot Nothing Then
                rdr.Close()
            Else
                SqlConn.Close()
            End If
            ex.Message.ToString()
        End Try
    End Function
    <System.Web.Script.Services.ScriptMethod(), _
 System.Web.Services.WebMethod()> _
    Public Shared Function SearchTitle(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim sdr As SqlDataReader
        Dim cmd As SqlCommand = New SqlCommand
        cmd.CommandText = "select DISTINCT Title from cats where Title like '" + prefixText + "%'"
        cmd.Connection = SqlConn
        Try
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            Dim titles As List(Of String) = New List(Of String)
            sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            While sdr.Read
                titles.Add(sdr("title").ToString)
            End While
            Return titles
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
    Public Shared Function SearchSerialTitle(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim sdr As SqlDataReader
        Dim cmd As SqlCommand = New SqlCommand
        cmd.CommandText = "select DISTINCT Title from cats where (Title like '" + prefixText + "%') AND (BIB_CODE ='S')"
        cmd.Connection = SqlConn
        Try
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            Dim titles As List(Of String) = New List(Of String)
            sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            While sdr.Read
                titles.Add(sdr("title").ToString)
            End While
            Return titles
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
    Public Shared Function SearchAuthor(ByVal prefixText As String, ByVal count As Integer) As List(Of String)

        Dim cmd As SqlCommand = New SqlCommand
        Dim sdr As SqlDataReader
        Try
            cmd.CommandText = "SELECT AUTHOR FROM (SELECT AUTHOR1 FROM CATS UNION SELECT AUTHOR2 FROM CATS UNION SELECT AUTHOR3 FROM CATS) AS DistinctCodes (AUTHOR) WHERE AUTHOR IS NOT NULL AND AUTHOR like '" + prefixText + "%' "
            cmd.Connection = SqlConn
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            Dim authors As List(Of String) = New List(Of String)
            sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            While sdr.Read
                authors.Add(sdr("AUTHOR").ToString)
            End While
            Return authors
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
    Public Shared Function SearchSubTitle(ByVal prefixText As String, ByVal count As Integer) As List(Of String)

        Dim cmd As SqlCommand = New SqlCommand
        Dim sdr As SqlDataReader
        Try
            cmd.CommandText = "select DISTINCT SUB_TITLE from cats where SUB_TITLE like '" + prefixText + "%'"
            cmd.Connection = SqlConn
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            Dim subtitle As List(Of String) = New List(Of String)
            sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            While sdr.Read
                subtitle.Add(sdr("SUB_TITLE").ToString)
            End While
            Return subtitle
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
    Public Shared Function SearchConfName(ByVal prefixText As String, ByVal count As Integer) As List(Of String)

        Dim cmd As SqlCommand = New SqlCommand
        Dim sdr As SqlDataReader
        Try
            cmd.CommandText = "select DISTINCT CONF_NAME from cats where CONF_NAME like '" + prefixText + "%'"
            cmd.Connection = SqlConn
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            Dim confname As List(Of String) = New List(Of String)
            sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            While sdr.Read
                confname.Add(sdr("CONF_NAME").ToString)
            End While
            Return confname
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
    Public Shared Function SearchEdition(ByVal prefixText As String, ByVal count As Integer) As List(Of String)

        Dim cmd As SqlCommand = New SqlCommand
        Dim sdr As SqlDataReader
        Try
            cmd.CommandText = "select DISTINCT EDITION from cats where EDITION like '" + prefixText + "%'"
            cmd.Connection = SqlConn
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            Dim edition As List(Of String) = New List(Of String)
            sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            While sdr.Read
                edition.Add(sdr("EDITION").ToString)
            End While
            Return edition
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
    Public Shared Function SearchPlace(ByVal prefixText As String, ByVal count As Integer) As List(Of String)

        Dim cmd As SqlCommand = New SqlCommand
        Dim sdr As SqlDataReader
        Try
            cmd.CommandText = "select DISTINCT PLACE_OF_PUB from cats where PLACE_OF_PUB like '" + prefixText + "%'"
            cmd.Connection = SqlConn
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            Dim place As List(Of String) = New List(Of String)
            sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            While sdr.Read
                place.Add(sdr("PLACE_OF_PUB").ToString)
            End While
            Return place
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
    Public Shared Function SearchYear(ByVal prefixText As String, ByVal count As Integer) As List(Of String)

        Dim cmd As SqlCommand = New SqlCommand
        Dim sdr As SqlDataReader
        Try
            cmd.CommandText = "select DISTINCT YEAR_OF_PUB from cats where YEAR_OF_PUB like '" + prefixText + "%'"
            cmd.Connection = SqlConn
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            Dim year As List(Of String) = New List(Of String)
            sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            While sdr.Read
                year.Add(sdr("YEAR_OF_PUB").ToString)
            End While
            Return year
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
    Public Shared Function SearchSeries(ByVal prefixText As String, ByVal count As Integer) As List(Of String)

        Dim cmd As SqlCommand = New SqlCommand
        Dim sdr As SqlDataReader
        Try
            cmd.CommandText = "select DISTINCT SERIES_TITLE from cats where SERIES_TITLE like '" + prefixText + "%'"
            cmd.Connection = SqlConn
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            Dim series As List(Of String) = New List(Of String)
            sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            While sdr.Read
                series.Add(sdr("SERIES_TITLE").ToString)
            End While
            Return series
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
    Public Shared Function SearchISBN(ByVal prefixText As String, ByVal count As Integer) As List(Of String)

        Dim cmd As SqlCommand = New SqlCommand
        Dim sdr As SqlDataReader
        Try
            cmd.CommandText = "select DISTINCT STANDARD_NO from cats where STANDARD_NO like '" + prefixText + "%'"
            cmd.Connection = SqlConn
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            Dim ISBN As List(Of String) = New List(Of String)
            sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            While sdr.Read
                ISBN.Add(sdr("STANDARD_NO").ToString)
            End While
            Return ISBN
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
    Public Shared Function SearchManualNo(ByVal prefixText As String, ByVal count As Integer) As List(Of String)

        Dim cmd As SqlCommand = New SqlCommand
        Dim sdr As SqlDataReader
        Try
            cmd.CommandText = "select DISTINCT MANUAL_NO from cats where MANUAL_NO like '" + prefixText + "%'"
            cmd.Connection = SqlConn
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            Dim Manual As List(Of String) = New List(Of String)
            sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            While sdr.Read
                Manual.Add(sdr("MANUAL_NO").ToString)
            End While
            Return Manual
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
    Public Shared Function SearchReportNo(ByVal prefixText As String, ByVal count As Integer) As List(Of String)

        Dim cmd As SqlCommand = New SqlCommand
        Dim sdr As SqlDataReader
        Try
            cmd.CommandText = "select DISTINCT REPORT_NO from cats where REPORT_NO like '" + prefixText + "%'"
            cmd.Connection = SqlConn
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            Dim Report As List(Of String) = New List(Of String)
            sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            While sdr.Read
                Report.Add(sdr("REPORT_NO").ToString)
            End While
            Return Report
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
    Public Shared Function SearchActNo(ByVal prefixText As String, ByVal count As Integer) As List(Of String)

        Dim cmd As SqlCommand = New SqlCommand
        Dim sdr As SqlDataReader
        Try
            cmd.CommandText = "select DISTINCT ACT_NO from cats where ACT_NO like '" + prefixText + "%'"
            cmd.Connection = SqlConn
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            Dim Act As List(Of String) = New List(Of String)
            sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            While sdr.Read
                Act.Add(sdr("ACT_NO").ToString)
            End While
            Return Act
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
    Public Shared Function SearchSPNo(ByVal prefixText As String, ByVal count As Integer) As List(Of String)

        Dim cmd As SqlCommand = New SqlCommand
        Dim sdr As SqlDataReader
        Try
            cmd.CommandText = "select DISTINCT SP_NO from cats where SP_NO like '" + prefixText + "%'"
            cmd.Connection = SqlConn
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            Dim SP As List(Of String) = New List(Of String)
            sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            While sdr.Read
                SP.Add(sdr("SP_NO").ToString)
            End While
            Return SP
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
    Public Shared Function SearchAppNo(ByVal prefixText As String, ByVal count As Integer, ByVal LIB_CODE As Object) As List(Of String)
        Dim sdr As SqlDataReader
        Dim cmd As SqlCommand = New SqlCommand
        cmd.CommandText = "select DISTINCT APP_NO from ACQUISITIONS where (LIB_CODE ='" & LIB_CODE & "') AND (PROCESS_STATUS ='Requested') AND (APP_NO like '" + prefixText + "%')"
        cmd.Connection = SqlConn
        Try
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            Dim appno As List(Of String) = New List(Of String)
            sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            While sdr.Read
                appno.Add(sdr("APP_NO").ToString)
            End While
            Return appno
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
    'search Accession No
    <System.Web.Script.Services.ScriptMethod(), _
System.Web.Services.WebMethod()> _
    Public Shared Function SearchAccNo(ByVal prefixText As String, ByVal count As Integer, ByVal LIB_CODE As Object) As List(Of String)
        Dim sdr As SqlDataReader
        Dim cmd As SqlCommand = New SqlCommand
        cmd.CommandText = "SELECT DISTINCT ACCESSION_NO from HOLDINGS WHERE (LIB_CODE ='" & LIB_CODE & "')  AND (ACCESSION_NO like '" + prefixText + "%')"
        cmd.Connection = SqlConn
        Try
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            Dim accno As List(Of String) = New List(Of String)
            sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            While sdr.Read
                accno.Add(sdr("ACCESSION_NO").ToString)
            End While
            Return accno
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
    'search class No
    <System.Web.Script.Services.ScriptMethod(), _
System.Web.Services.WebMethod()> _
    Public Shared Function SearchClassNo(ByVal prefixText As String, ByVal count As Integer, ByVal LIB_CODE As Object) As List(Of String)
        Dim sdr As SqlDataReader
        Dim cmd As SqlCommand = New SqlCommand
        cmd.CommandText = "SELECT DISTINCT CLASS_NO from HOLDINGS WHERE (LIB_CODE ='" & LIB_CODE & "')  AND (CLASS_NO like '" + prefixText + "%')"
        cmd.Connection = SqlConn
        Try
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            Dim classNo As List(Of String) = New List(Of String)
            sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            While sdr.Read
                classNo.Add(sdr("CLASS_NO").ToString)
            End While
            Return classNo
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
    Public Shared Function SearchDiscount(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim sdr As SqlDataReader
        Dim cmd As SqlCommand = New SqlCommand
        cmd.CommandText = "select DISTINCT DISCOUNT from BILLS where DISCOUNT like '" + prefixText + "%'"
        cmd.Connection = SqlConn
        Try
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            Dim Discount As List(Of String) = New List(Of String)
            sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            While sdr.Read
                Discount.Add(sdr("DISCOUNT").ToString)
            End While
            Return Discount
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
    Public Shared Function SearchBills(ByVal prefixText As String, ByVal count As Integer, ByVal LibCode As Object) As List(Of String)
        Dim sdr As SqlDataReader
        Dim cmd As SqlCommand = New SqlCommand
        cmd.CommandText = "select DISTINCT INV_NO from BILLS where (INV_NO like '" + prefixText + "%') AND (LIB_CODE = '" & Trim(LibCode) & "')"
        cmd.Connection = SqlConn
        Try
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            Dim BillNo As List(Of String) = New List(Of String)
            sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            While sdr.Read
                BillNo.Add(sdr("INV_NO").ToString)
            End While
            Return BillNo
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
    'publish Leter Templates
    Public Shared Function GetLetters(ByVal LibCode As Object) As IEnumerable
        Dim Sel As String = "SELECT MESS_ID, FORM_NAME FROM MESSAGES WHERE (LIB_CODE ='" & (LibCode) & "') ORDER BY FORM_NAME ASC; "
        Dim cmd As New SqlCommand(Sel, SqlConn)
        Dim rdr As SqlDataReader = Nothing
        Try
            SqlConn.Open()
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            Return (rdr)
        Catch ex As System.Exception
            If rdr IsNot Nothing Then
                rdr.Close()
            Else
                SqlConn.Close()
            End If
            ex.Message.ToString()
        End Try
    End Function
    'publish member cir trans in opac
    Public Shared Function GetBindGrid4(ByVal LibMCode As Object, ByVal MemMID As Integer) As IEnumerable
        Dim Sel As String = "SELECT CIRCULATION_TRANSACTIONS_VIEW.CIR_ID,MEM_NAME, MEM_ID, MEM_NO, Accession = CASE (isnull(accession_no,'')) WHen ''  THEN cast(copy_id as varchar(20))" & _
                       " ELSE accession_no END, Title = CASE (isnull(Title,'')) WHen ''  THEN journal ELSE title END, convert(char(10),ISSUE_DATE,103) as ISSUE_DATE,ISSUE_TIME,convert(char(10),DUE_DATE,103) as DUE_DATE,convert(char(10),RETURN_DATE,103) as RETURN_DATE,RETURN_TIME," & _
                       " FINE_COLLECTED, convert(char(10),RESERVE_DATE,103) as RESERVE_DATE, convert(char(10),RENEW_DATE,103) as RENEW_DATE, STATUS, VOL_NO, CIR_LIB_CODE  FROM CIRCULATION_TRANSACTIONS_VIEW WHERE (CIR_LIB_CODE = '" & Trim(LibMCode) & "') AND (CIRCULATION_TRANSACTIONS_VIEW.MEM_ID = '" & Trim(MemMID) & "') "

        Dim cmd As New SqlCommand(Sel, SqlConn)
        Dim rdr As SqlDataReader = Nothing
        Try
            SqlConn.Open()
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            Return (rdr)
            ViewState("dt") = rdr
        Catch ex As System.Exception
            If rdr IsNot Nothing Then
                rdr.Close()
            Else
                SqlConn.Close()
            End If
            ex.Message.ToString()
        End Try
    End Function
End Class
