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
Public Class DEFormats
    Inherits System.Web.UI.Page
    Dim ID As String = ""
    Dim CODE As String = ""
    Dim NAME As String = ""
    Dim DESC As String = ""
    Dim MYTABLE As String = ""
    Dim index As Integer = 0
    Public Srno As Integer = 0
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session.Item("IsThisU") <> Nothing Then
                If Not SConnection() = True Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Database Connection is lost..Try Again !');", True)
                Else
                    If Page.IsPostBack = False Then
                        PopulateDocumentTypes()
                        PopulateNodes()
                    End If
                    Dim CreateSUserButton As Object
                    CreateSUserButton = Master.FindControl("Accordion1").FindControl("LibAdminPane").FindControl("Lib_DEFormat_Bttn")
                    CreateSUserButton.ForeColor = Drawing.Color.Red
                    myPaneName = "LibAdminPane"  'paneSelectedIndex = 0
                End If
            Else
                Response.Redirect("~/Default.aspx", False)
            End If
            LibCode = Session.Item("LoggedLibcode")
        Catch ex As Exception
            Label6.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    Private Sub SystemData_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        DropDownList1.Focus()
    End Sub
    'populate materials type
    Public Sub PopulateDocumentTypes()
        Dim Command As SqlCommand = Nothing
        Dim dt As DataTable = Nothing
        Try
            Command = New SqlCommand("SELECT DOC_TYPE_ID, DOC_TYPE_CODE, DOC_TYPE_NAME FROM DOC_TYPES ORDER BY DOC_TYPE_NAME ", SqlConn)
            SqlConn.Open()
            Dim da As New SqlDataAdapter(Command)
            Dim ds As New DataSet
            Dim RecordCount As Long = 0
            da.Fill(ds)

            dt = ds.Tables(0).Copy

            Dim Dr As DataRow
            Dr = dt.NewRow
            Dr("DOC_TYPE_CODE") = ""
            Dr("DOC_TYPE_NAME") = ""
            dt.Rows.InsertAt(Dr, 0)

            If dt.Rows.Count = 0 Then
                Me.DropDownList1.DataSource = Nothing
            Else
                Me.DropDownList1.DataSource = dt
                Me.DropDownList1.DataTextField = "DOC_TYPE_NAME"
                Me.DropDownList1.DataValueField = "DOC_TYPE_CODE"
                Me.DropDownList1.DataBind()
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
    Public Sub PopulateNodes()
        If DropDownList1.Text <> "" Then
            Label10.Text = ""
            If DropDownList1.SelectedValue = "AM" Or DropDownList1.SelectedValue = "AN" Or DropDownList1.SelectedValue = "AB" Or DropDownList1.SelectedValue = "BB" Or DropDownList1.SelectedValue = "CB" Or DropDownList1.SelectedValue = "CP" Or DropDownList1.SelectedValue = "DT" Or DropDownList1.SelectedValue = "DR" Or DropDownList1.SelectedValue = "EB" Or DropDownList1.SelectedValue = "EN" Or DropDownList1.SelectedValue = "BK" Or DropDownList1.SelectedValue = "GB" Or DropDownList1.SelectedValue = "HB" Or DropDownList1.SelectedValue = "TB" Or DropDownList1.SelectedValue = "TH" Or DropDownList1.SelectedValue = "YB" Or DropDownList1.SelectedValue = "BA" Or DropDownList1.SelectedValue = "IC" Or DropDownList1.SelectedValue = "LA" Or DropDownList1.SelectedValue = "PB" Or DropDownList1.SelectedValue = "PD" Or DropDownList1.SelectedValue = "SC" Then  'books and related documentsw
                TR_General.Visible = True 'for monographs/Books
                TR_Reports.Visible = False
                TR_Standards.Visible = False
                TR_Manuals.Visible = False
                TR_Patents.Visible = False
                TR_NBM.Visible = False
                TR_ISBN.Visible = True
                TR_Titles.Visible = True
                TR_Authors.Visible = True
                TR_CorpAuthors.Visible = True
                TR_Edition.Visible = True
                TR_Imprint.Visible = True
                TR_Series.Visible = True
                TR_Notes.Visible = True
                TR_Abstracts.Visible = True
                TR_Others.Visible = True
                TR_Holdings.Visible = True
            End If
            'non-book materials
            If DropDownList1.SelectedValue = "TX" Or DropDownList1.SelectedValue = "XM" Or DropDownList1.SelectedValue = "XW" Or DropDownList1.SelectedValue = "SL" Or DropDownList1.SelectedValue = "SR" Or DropDownList1.SelectedValue = "VR" Or DropDownList1.SelectedValue = "CD" Or DropDownList1.SelectedValue = "FC" Or DropDownList1.SelectedValue = "IM" Or DropDownList1.SelectedValue = "PH" Or DropDownList1.SelectedValue = "LR" Or DropDownList1.SelectedValue = "CR" Then
                TR_General.Visible = True
                TR_Reports.Visible = False
                TR_Standards.Visible = False
                TR_Manuals.Visible = False
                TR_Patents.Visible = False
                TR_NBM.Visible = True
                TR_ISBN.Visible = False
                TR_Titles.Visible = True
                TR_Authors.Visible = True
                TR_CorpAuthors.Visible = True
                TR_Edition.Visible = True
                TR_Imprint.Visible = True
                TR_Series.Visible = True
                TR_Notes.Visible = True
                TR_Abstracts.Visible = True
                TR_Others.Visible = True
                TR_Holdings.Visible = True
            End If

            If DropDownList1.SelectedValue = "PE" Then 'for Patents
                TR_General.Visible = True
                TR_Reports.Visible = False
                TR_Standards.Visible = False
                TR_Manuals.Visible = False
                TR_Patents.Visible = True
                TR_NBM.Visible = False
                TR_ISBN.Visible = True
                TR_Titles.Visible = True
                TR_Authors.Visible = True
                TR_CorpAuthors.Visible = True
                TR_Edition.Visible = True
                TR_Imprint.Visible = True
                TR_Series.Visible = True
                TR_Notes.Visible = True
                TR_Abstracts.Visible = True
                TR_Others.Visible = True
                TR_Holdings.Visible = True
            End If
            If DropDownList1.SelectedValue = "MN" Then 'for Manuals
                TR_General.Visible = True
                TR_Reports.Visible = False
                TR_Standards.Visible = False
                TR_Manuals.Visible = True
                TR_Patents.Visible = False
                TR_NBM.Visible = False
                TR_ISBN.Visible = True
                TR_Titles.Visible = True
                TR_Authors.Visible = True
                TR_CorpAuthors.Visible = True
                TR_Edition.Visible = True
                TR_Imprint.Visible = True
                TR_Series.Visible = True
                TR_Notes.Visible = True
                TR_Abstracts.Visible = True
                TR_Others.Visible = True
                TR_Holdings.Visible = True
            End If
            If DropDownList1.SelectedValue = "ST" Then 'for standards
                TR_General.Visible = True
                TR_Reports.Visible = False
                TR_Standards.Visible = True
                TR_Manuals.Visible = False
                TR_Patents.Visible = False
                TR_NBM.Visible = False
                TR_ISBN.Visible = True
                TR_Titles.Visible = True
                TR_Authors.Visible = True
                TR_CorpAuthors.Visible = True
                TR_Edition.Visible = True
                TR_Imprint.Visible = True
                TR_Series.Visible = True
                TR_Notes.Visible = True
                TR_Abstracts.Visible = True
                TR_Others.Visible = True
                TR_Holdings.Visible = True
            End If
            'cartographic
            If DropDownList1.SelectedValue = "AT" Or DropDownList1.SelectedValue = "MP" Or DropDownList1.SelectedValue = "GL" Or DropDownList1.SelectedValue = "ML" Then 'for cartographic
                TR_General.Visible = True
                TR_Reports.Visible = False
                TR_Standards.Visible = False
                TR_Manuals.Visible = False
                TR_Patents.Visible = False
                TR_NBM.Visible = True
                TR_ISBN.Visible = True
                TR_Titles.Visible = True
                TR_Authors.Visible = True
                TR_CorpAuthors.Visible = True
                TR_Edition.Visible = True
                TR_Imprint.Visible = True
                TR_Series.Visible = True
                TR_Notes.Visible = True
                TR_Abstracts.Visible = True
                TR_Others.Visible = True
                TR_Holdings.Visible = True
            End If
            If DropDownList1.SelectedValue = "NL" Or DropDownList1.SelectedValue = "NP" Then 'for Newspapers
                TR_General.Visible = True
                TR_Reports.Visible = False
                TR_Standards.Visible = False
                TR_Manuals.Visible = False
                TR_Patents.Visible = False
                TR_NBM.Visible = False
                TR_ISBN.Visible = True
                TR_Titles.Visible = True
                TR_Authors.Visible = True
                TR_CorpAuthors.Visible = True
                TR_Edition.Visible = True
                TR_Imprint.Visible = True
                TR_Series.Visible = True
                TR_Notes.Visible = True
                TR_Abstracts.Visible = True
                TR_Others.Visible = True
                TR_Holdings.Visible = True
            End If
            If DropDownList1.SelectedValue = "AI" Or DropDownList1.SelectedValue = "JR" Or DropDownList1.SelectedValue = "MG" Then 'for Periodicals
                TR_General.Visible = True
                TR_Reports.Visible = False
                TR_Standards.Visible = False
                TR_Manuals.Visible = False
                TR_Patents.Visible = False
                TR_NBM.Visible = False
                TR_ISBN.Visible = True
                TR_Titles.Visible = True
                TR_Authors.Visible = True
                TR_CorpAuthors.Visible = True
                TR_Edition.Visible = True
                TR_Imprint.Visible = True
                TR_Series.Visible = True
                TR_Notes.Visible = True
                TR_Abstracts.Visible = True
                TR_Others.Visible = True
                TR_Holdings.Visible = True
            End If

            'reports
            If DropDownList1.SelectedValue = "CR" Or DropDownList1.SelectedValue = "GR" Or DropDownList1.SelectedValue = "LR" Or DropDownList1.SelectedValue = "PR" Or DropDownList1.SelectedValue = "RR" Or DropDownList1.SelectedValue = "TR" Or DropDownList1.SelectedValue = "TS" Or DropDownList1.SelectedValue = "DS" Or DropDownList1.SelectedValue = "CR" Or DropDownList1.SelectedValue = "LR" Then 'for Reports
                TR_General.Visible = True
                TR_Reports.Visible = True
                TR_Standards.Visible = False
                TR_Manuals.Visible = False
                TR_Patents.Visible = False
                TR_NBM.Visible = False
                TR_ISBN.Visible = True
                TR_Titles.Visible = True
                TR_Authors.Visible = True
                TR_CorpAuthors.Visible = True
                TR_Edition.Visible = True
                TR_Imprint.Visible = True
                TR_Series.Visible = True
                TR_Notes.Visible = True
                TR_Abstracts.Visible = True
                TR_Others.Visible = True
                TR_Holdings.Visible = True
            End If
            'If DropDownList1.SelectedValue = "V" Then 'for Audio-Visuals
            '    TR_General.Visible = True
            '    TR_Reports.Visible = False
            '    TR_Standards.Visible = False
            '    TR_Manuals.Visible = False
            '    TR_Patents.Visible = False
            '    TR_ISBN.Visible = True
            '    TR_Titles.Visible = True
            '    TR_Authors.Visible = True
            '    TR_CorpAuthors.Visible = True
            '    TR_Edition.Visible = True
            '    TR_Imprint.Visible = True
            '    TR_Series.Visible = True
            '    TR_Notes.Visible = True
            '    TR_Abstracts.Visible = True
            '    TR_Others.Visible = True
            '    TR_Holdings.Visible = True
            'End If
            If DropDownList1.SelectedValue = "X" Then 'for Manuscripts
                TR_General.Visible = True
                TR_Reports.Visible = False
                TR_Standards.Visible = False
                TR_Manuals.Visible = False
                TR_Patents.Visible = False
                TR_NBM.Visible = True
                TR_ISBN.Visible = True
                TR_Titles.Visible = True
                TR_Authors.Visible = True
                TR_CorpAuthors.Visible = True
                TR_Edition.Visible = True
                TR_Imprint.Visible = True
                TR_Series.Visible = True
                TR_Notes.Visible = True
                TR_Abstracts.Visible = True
                TR_Others.Visible = True
                TR_Holdings.Visible = True
            End If
        Else
            TR_General.Visible = True
            TR_Reports.Visible = True
            TR_Standards.Visible = True
            TR_Manuals.Visible = True
            TR_Patents.Visible = True
            TR_NBM.Visible = True
            TR_ISBN.Visible = True
            TR_Titles.Visible = True
            TR_Authors.Visible = True
            TR_CorpAuthors.Visible = True
            TR_Edition.Visible = True
            TR_Imprint.Visible = True
            TR_Series.Visible = True
            TR_Notes.Visible = True
            TR_Abstracts.Visible = True
            TR_Others.Visible = True
            TR_Holdings.Visible = True
        End If
        'chk duplecate format hide/show bttn
        UnCheckAll()
        CheckSavedFormats()
        UpdatePanel2.Update()
        UpdatePanel1.Update()
    End Sub
    'populate materials type
    Public Sub DropDownList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownList1.SelectedIndexChanged
        PopulateNodes()
    End Sub
    Public Sub UnCheckAll()
        If TR_Reports.Visible = True Then
            For Each tn As TreeNode In TreeView_Reports.Nodes
                If tn.Value = "REPORTS" Then
                    If tn.ChildNodes.Count > 0 Then
                        For Each cTn As TreeNode In tn.ChildNodes
                            cTn.Checked = False
                        Next
                    End If
                End If
            Next
        End If
        If TR_ISBN.Visible = True Then
            For Each tn As TreeNode In TreeView_ISBN.Nodes
                If tn.Value = "STD_NO" Then
                    If tn.ChildNodes.Count > 0 Then
                        For Each cTn As TreeNode In tn.ChildNodes
                            cTn.Checked = False
                        Next
                    End If
                End If
            Next
        End If

        If TR_Manuals.Visible = True Then
            For Each tn As TreeNode In TreeView_Manuals.Nodes
                If tn.Value = "MANUALS" Then
                    If tn.ChildNodes.Count > 0 Then
                        For Each cTn As TreeNode In tn.ChildNodes
                            cTn.Checked = False
                        Next
                    End If
                End If
            Next
        End If
        If TR_Patents.Visible = True Then
            For Each tn As TreeNode In TreeView_Patents.Nodes
                If tn.Value = "PATENTS" Then
                    If tn.ChildNodes.Count > 0 Then
                        For Each cTn As TreeNode In tn.ChildNodes
                            cTn.Checked = False
                        Next
                    End If
                End If
            Next
        End If
        If TR_Standards.Visible = True Then
            For Each tn As TreeNode In TreeView_SP.Nodes
                If tn.Value = "Standard Specification" Then
                    If tn.ChildNodes.Count > 0 Then
                        For Each cTn As TreeNode In tn.ChildNodes
                            cTn.Checked = False
                        Next
                    End If
                End If
            Next
        End If
        If TR_NBM.Visible = True Then
            For Each tn As TreeNode In TreeView_NBM.Nodes
                If tn.Value = "NBM" Then
                    If tn.ChildNodes.Count > 0 Then
                        For Each cTn As TreeNode In tn.ChildNodes
                            cTn.Checked = False
                        Next
                    End If
                End If
            Next
        End If

        If TR_Titles.Visible = True Then
            For Each tn As TreeNode In TreeView_Titles.Nodes
                If tn.Value = "TITLE_STATEMENTS" Then
                    If tn.ChildNodes.Count > 0 Then
                        For Each cTn As TreeNode In tn.ChildNodes
                            cTn.Checked = False
                        Next
                    End If
                End If
            Next
        End If
        If TR_Authors.Visible = True Then
            For Each tn As TreeNode In TreeView_Authors.Nodes
                If tn.Value = "RESPONSIBILITIES" Then
                    If tn.ChildNodes.Count > 0 Then
                        For Each cTn As TreeNode In tn.ChildNodes
                            cTn.Checked = False
                        Next
                    End If
                End If
            Next
        End If
        If TR_CorpAuthors.Visible = True Then
            For Each tn As TreeNode In TreeView_CorpAuthor.Nodes
                If tn.Value = "Corporate Author" Then
                    If tn.ChildNodes.Count > 0 Then
                        For Each cTn As TreeNode In tn.ChildNodes
                            cTn.Checked = False
                        Next
                    End If
                End If
            Next
        End If
        If TR_Edition.Visible = True Then
            For Each tn As TreeNode In TreeView_Edition.Nodes
                If tn.Value = "Edition Statement" Then
                    If tn.ChildNodes.Count > 0 Then
                        For Each cTn As TreeNode In tn.ChildNodes
                            cTn.Checked = False
                        Next
                    End If
                End If
            Next
        End If
        If TR_Imprint.Visible = True Then
            For Each tn As TreeNode In TreeView_Imprint.Nodes
                If tn.Value = "IMPRINT" Then
                    If tn.ChildNodes.Count > 0 Then
                        For Each cTn As TreeNode In tn.ChildNodes
                            cTn.Checked = False
                        Next
                    End If
                End If
            Next
        End If
        If TR_Series.Visible = True Then
            For Each tn As TreeNode In TreeView_Sereis.Nodes
                If tn.Value = "Series Statement" Then
                    If tn.ChildNodes.Count > 0 Then
                        For Each cTn As TreeNode In tn.ChildNodes
                            cTn.Checked = False
                        Next
                    End If
                End If
            Next
        End If
        If TR_Notes.Visible = True Then
            For Each tn As TreeNode In TreeView_Note.Nodes
                If tn.Value = "Note Area" Then
                    If tn.ChildNodes.Count > 0 Then
                        For Each cTn As TreeNode In tn.ChildNodes
                            cTn.Checked = False
                        Next
                    End If
                End If
            Next
        End If
        If TR_Abstracts.Visible = True Then
            For Each tn As TreeNode In TreeView_Subject.Nodes
                If tn.Value = "Subjet and Keywords" Then
                    If tn.ChildNodes.Count > 0 Then
                        For Each cTn As TreeNode In tn.ChildNodes
                            cTn.Checked = False
                        Next
                    End If
                End If
            Next
        End If
        If TR_Others.Visible = True Then
            For Each tn As TreeNode In TreeView_Other.Nodes
                If tn.Value = "OTHERS" Then
                    If tn.ChildNodes.Count > 0 Then
                        For Each cTn As TreeNode In tn.ChildNodes
                            cTn.Checked = False
                        Next
                    End If
                End If
            Next
        End If

        If TR_Holdings.Visible = True Then
            For Each tn As TreeNode In TreeView_Holdings.Nodes
                If tn.Value = "HOLDINGS" Then
                    If tn.ChildNodes.Count > 0 Then
                        For Each cTn As TreeNode In tn.ChildNodes
                            cTn.Checked = False
                        Next
                    End If
                End If
            Next
        End If

    End Sub
    Public Sub CheckSavedFormats()
        'check unwanted characters
        Dim iloop As Integer
        Dim strcurrentchar As Object
        Dim c As Integer
        Dim counter1 As Integer
        '********************************************************************************************************
        'doc code
        Dim myDocCode As String = Nothing
        If DropDownList1.Text <> "" Then
            myDocCode = Trim(DropDownList1.SelectedValue)
            myDocCode = RemoveQuotes(myDocCode)
            If myDocCode.Length > 4 Then
                Label6.Text = "Input is not of proper length... "
                Me.DropDownList1.Focus()
                Exit Sub
            End If
            myDocCode = " " & myDocCode & " "
            If InStr(1, myDocCode, "CREATE", 1) > 0 Or InStr(1, myDocCode, "DELETE", 1) > 0 Or InStr(1, myDocCode, "DROP", 1) > 0 Or InStr(1, myDocCode, "INSERT", 1) > 1 Or InStr(1, myDocCode, "TRACK", 1) > 1 Or InStr(1, myDocCode, "TRACE", 1) > 1 Then
                Label6.Text = " Do not use Reserve Words"
                Me.DropDownList1.Focus()
                Exit Sub
            End If
            myDocCode = TrimAll(myDocCode)
            'check unwanted characters
            c = 0
            counter1 = 0
            For iloop = 1 To Len(myDocCode)
                strcurrentchar = Mid(myDocCode, iloop, 1)
                If c = 0 Then
                    If Not InStr("';~!@#$^&*-_=+|[]{}?,<>=()%""", strcurrentchar) <= 0 Then
                        c = c + 1
                        counter1 = 1
                    End If
                End If
            Next
            If counter1 = 1 Then
                Label6.Text = "Do not use un-wanted letters..."
                Me.DropDownList1.Focus()
                Exit Sub
            End If
            'Check Duplicate format
            Dim str As Object = Nothing
            Dim flag As Object = Nothing
            str = "SELECT DEF_ID, CAT_FORMAT, HOLD_FORMAT FROM DEFORMATS WHERE (LIB_CODE = '" & Trim(LibCode) & "'  AND DOC_TYPE_CODE = '" & Trim(myDocCode) & "') "
            Dim cmd1 As New SqlCommand(str, SqlConn)
            SqlConn.Open()
            flag = cmd1.ExecuteScalar
            If flag <> Nothing Then
                Me.bttn_Save.Visible = False
                Me.bttn_Delete.Visible = True
                Me.bttn_Update.Visible = True
            Else
                Me.bttn_Save.Visible = True
                Me.bttn_Delete.Visible = False
                Me.bttn_Update.Visible = False
            End If
            SqlConn.Close()
        Else
            Me.bttn_Save.Visible = False
            Me.bttn_Delete.Visible = False
            Me.bttn_Update.Visible = False
        End If

        'bind the format with drop-downs in loop
        Dim dr As SqlDataReader = Nothing
        Dim Command As SqlCommand = Nothing
        Dim myCatFields As Object = Nothing
        Dim myHoldFields As Object = Nothing

        Dim SQL As String = Nothing
        SQL = "SELECT *  FROM DEFORMATS WHERE (LIB_CODE = '" & Trim(LibCode) & "'  AND DOC_TYPE_CODE = '" & Trim(myDocCode) & "') "
        Command = New SqlCommand(SQL, SqlConn)
        SqlConn.Open()
        dr = Command.ExecuteReader(CommandBehavior.CloseConnection)
        dr.Read()

        If dr.HasRows = True Then
            If dr.Item("CAT_FORMAT").ToString <> "" Then
                myCatFields = dr.Item("CAT_FORMAT")
                If dr.Item("DEF_ID").ToString <> "" Then
                    Label10.Text = dr.Item("DEF_ID")
                Else
                    Label10.Text = ""
                End If

                Dim myNewCatFields As Object = Nothing
                myNewCatFields = Split(myCatFields, ";")
                Dim i As Integer = 0
                For i = 0 To myNewCatFields.Length - 1
                    Dim myDisplayCatFlds As String
                    myDisplayCatFlds = myNewCatFields(i)

                    If TR_Reports.Visible = True Then
                        For Each tn As TreeNode In TreeView_Reports.Nodes
                            If tn.Value = "REPORTS" Then
                                If tn.ChildNodes.Count > 0 Then
                                    For Each cTn As TreeNode In tn.ChildNodes
                                        If cTn.Value = myDisplayCatFlds Then
                                            cTn.Checked = True
                                        End If
                                    Next
                                End If
                            End If
                        Next
                    End If
                    If TR_ISBN.Visible = True Then
                        For Each tn As TreeNode In TreeView_ISBN.Nodes
                            If tn.Value = "STD_NO" Then
                                If tn.ChildNodes.Count > 0 Then
                                    For Each cTn As TreeNode In tn.ChildNodes
                                        If cTn.Value = myDisplayCatFlds Then
                                            cTn.Checked = True
                                        End If
                                    Next
                                End If
                            End If
                        Next
                    End If

                    If TR_Manuals.Visible = True Then
                        For Each tn As TreeNode In TreeView_Manuals.Nodes
                            If tn.Value = "MANUALS" Then
                                If tn.ChildNodes.Count > 0 Then
                                    For Each cTn As TreeNode In tn.ChildNodes
                                        If cTn.Value = myDisplayCatFlds Then
                                            cTn.Checked = True
                                        End If
                                    Next
                                End If
                            End If
                        Next
                    End If
                    If TR_Patents.Visible = True Then
                        For Each tn As TreeNode In TreeView_Patents.Nodes
                            If tn.Value = "PATENTS" Then
                                If tn.ChildNodes.Count > 0 Then
                                    For Each cTn As TreeNode In tn.ChildNodes
                                        If cTn.Value = myDisplayCatFlds Then
                                            cTn.Checked = True
                                        End If
                                    Next
                                End If
                            End If
                        Next
                    End If
                    If TR_Standards.Visible = True Then
                        For Each tn As TreeNode In TreeView_SP.Nodes
                            If tn.Value = "Standard Specification" Then
                                If tn.ChildNodes.Count > 0 Then
                                    For Each cTn As TreeNode In tn.ChildNodes
                                        If cTn.Value = myDisplayCatFlds Then
                                            cTn.Checked = True
                                        End If
                                    Next
                                End If
                            End If
                        Next
                    End If

                    If TR_NBM.Visible = True Then
                        For Each tn As TreeNode In TreeView_NBM.Nodes
                            If tn.Value = "NBM" Then
                                If tn.ChildNodes.Count > 0 Then
                                    For Each cTn As TreeNode In tn.ChildNodes
                                        If cTn.Value = myDisplayCatFlds Then
                                            cTn.Checked = True
                                        End If
                                    Next
                                End If
                            End If
                        Next
                    End If

                    If TR_Titles.Visible = True Then
                        For Each tn As TreeNode In TreeView_Titles.Nodes
                            If tn.Value = "TITLE_STATEMENTS" Then
                                If tn.ChildNodes.Count > 0 Then
                                    For Each cTn As TreeNode In tn.ChildNodes
                                        If cTn.Value = myDisplayCatFlds Then
                                            cTn.Checked = True
                                        End If
                                    Next
                                End If
                            End If
                        Next
                    End If


                    If TR_Authors.Visible = True Then
                        For Each tn As TreeNode In TreeView_Authors.Nodes
                            If tn.Value = "RESPONSIBILITIES" Then
                                If tn.ChildNodes.Count > 0 Then
                                    For Each cTn As TreeNode In tn.ChildNodes
                                        If cTn.Value = myDisplayCatFlds Then
                                            cTn.Checked = True
                                        End If
                                    Next
                                End If
                            End If
                        Next
                    End If
                    If TR_CorpAuthors.Visible = True Then
                        For Each tn As TreeNode In TreeView_CorpAuthor.Nodes
                            If tn.Value = "Corporate Author" Then
                                If tn.ChildNodes.Count > 0 Then
                                    For Each cTn As TreeNode In tn.ChildNodes
                                        If cTn.Value = myDisplayCatFlds Then
                                            cTn.Checked = True
                                        End If
                                    Next
                                End If
                            End If
                        Next
                    End If
                    If TR_Edition.Visible = True Then
                        For Each tn As TreeNode In TreeView_Edition.Nodes
                            If tn.Value = "Edition Statement" Then
                                If tn.ChildNodes.Count > 0 Then
                                    For Each cTn As TreeNode In tn.ChildNodes
                                        If cTn.Value = myDisplayCatFlds Then
                                            cTn.Checked = True
                                        End If
                                    Next
                                End If
                            End If
                        Next
                    End If
                    If TR_Imprint.Visible = True Then
                        For Each tn As TreeNode In TreeView_Imprint.Nodes
                            If tn.Value = "IMPRINT" Then
                                If tn.ChildNodes.Count > 0 Then
                                    For Each cTn As TreeNode In tn.ChildNodes
                                        If cTn.Value = myDisplayCatFlds Then
                                            cTn.Checked = True
                                        End If
                                    Next
                                End If
                            End If
                        Next
                    End If
                    If TR_Series.Visible = True Then
                        For Each tn As TreeNode In TreeView_Sereis.Nodes
                            If tn.Value = "Series Statement" Then
                                If tn.ChildNodes.Count > 0 Then
                                    For Each cTn As TreeNode In tn.ChildNodes
                                        If cTn.Value = myDisplayCatFlds Then
                                            cTn.Checked = True
                                        End If
                                    Next
                                End If
                            End If
                        Next
                    End If
                    If TR_Notes.Visible = True Then
                        For Each tn As TreeNode In TreeView_Note.Nodes
                            If tn.Value = "Note Area" Then
                                If tn.ChildNodes.Count > 0 Then
                                    For Each cTn As TreeNode In tn.ChildNodes
                                        If cTn.Value = myDisplayCatFlds Then
                                            cTn.Checked = True
                                        End If
                                    Next
                                End If
                            End If
                        Next
                    End If
                    If TR_Abstracts.Visible = True Then
                        For Each tn As TreeNode In TreeView_Subject.Nodes
                            If tn.Value = "Subjet and Keywords" Then
                                If tn.ChildNodes.Count > 0 Then
                                    For Each cTn As TreeNode In tn.ChildNodes
                                        If cTn.Value = myDisplayCatFlds Then
                                            cTn.Checked = True
                                        End If
                                    Next
                                End If
                            End If
                        Next
                    End If
                    If TR_Others.Visible = True Then
                        For Each tn As TreeNode In TreeView_Other.Nodes
                            If tn.Value = "OTHERS" Then
                                If tn.ChildNodes.Count > 0 Then
                                    For Each cTn As TreeNode In tn.ChildNodes
                                        If cTn.Value = myDisplayCatFlds Then
                                            cTn.Checked = True
                                        End If
                                    Next
                                End If
                            End If
                        Next
                    End If

                Next
            End If
                If dr.Item("HOLD_FORMAT").ToString <> "" Then
                    myHoldFields = dr.Item("HOLD_FORMAT")

                    Dim myNewHoldFields As Object = Nothing
                    myNewHoldFields = Split(myHoldFields, ";")
                    Dim i As Integer = 0
                    For i = 0 To myNewHoldFields.Length - 1
                        Dim myDisplayHoldFlds As String
                        myDisplayHoldFlds = myNewHoldFields(i)

                        If TR_Holdings.Visible = True Then
                            For Each tn As TreeNode In TreeView_Holdings.Nodes
                                If tn.Value = "HOLDINGS" Then
                                    If tn.ChildNodes.Count > 0 Then
                                        For Each cTn As TreeNode In tn.ChildNodes
                                            If cTn.Value = myDisplayHoldFlds Then
                                                cTn.Checked = True
                                            End If
                                        Next
                                    End If
                                End If
                            Next
                        End If


                    Next
            End If
        End If
        SqlConn.Close()
        UpdatePanel2.Update()
        UpdatePanel1.Update()
    End Sub
    'save record
    Protected Sub Save_Click(ByVal sender As Object, ByVal e As EventArgs) Handles bttn_Save.Click
        If IsPostBack = True Then
            'check unwanted characters
            Dim iloop As Integer
            Dim strcurrentchar As Object
            Dim c As Integer
            Dim counter1, counter2, counter3, counter4, Counter5, Counter6 As Integer
            Dim thisTransaction As SqlClient.SqlTransaction = Nothing
            Try
                Dim myCatString As String = Nothing
                For Each tn As TreeNode In TreeView_General.Nodes
                    If tn.Value = "General Fields" Then
                        If tn.ChildNodes.Count > 0 Then
                            For Each cTn As TreeNode In tn.ChildNodes
                                If cTn.Checked = True Then
                                    If myCatString = Nothing Then
                                        myCatString = cTn.Value
                                    Else
                                        myCatString = myCatString + ";" + cTn.Value
                                    End If
                                End If
                            Next
                        End If
                    End If
                Next

                If TR_Reports.Visible = True Then
                    For Each tn As TreeNode In TreeView_Reports.Nodes
                        If tn.Value = "REPORTS" Then
                            If tn.ChildNodes.Count > 0 Then
                                For Each cTn As TreeNode In tn.ChildNodes
                                    If cTn.Value = "REPORT_NO" Then
                                        cTn.Checked = True
                                    End If
                                    If cTn.Checked = True Then
                                        If myCatString = Nothing Then
                                            myCatString = cTn.Value
                                        Else
                                            myCatString = myCatString + ";" + cTn.Value
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next
                End If

                If TR_ISBN.Visible = True Then
                    For Each tn As TreeNode In TreeView_ISBN.Nodes
                        If tn.Value = "STD_NO" Then
                            If tn.ChildNodes.Count > 0 Then
                                For Each cTn As TreeNode In tn.ChildNodes
                                    If cTn.Checked = True Then
                                        If myCatString = Nothing Then
                                            myCatString = cTn.Value
                                        Else
                                            myCatString = myCatString + ";" + cTn.Value
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next
                End If

                If TR_Manuals.Visible = True Then
                    For Each tn As TreeNode In TreeView_Manuals.Nodes
                        If tn.Value = "MANUALS" Then
                            If tn.ChildNodes.Count > 0 Then
                                For Each cTn As TreeNode In tn.ChildNodes
                                    If cTn.Value = "MANUAL_NO" Then
                                        cTn.Checked = True
                                    End If
                                    If cTn.Checked = True Then
                                        If myCatString = Nothing Then
                                            myCatString = cTn.Value
                                        Else
                                            myCatString = myCatString + ";" + cTn.Value
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next
                End If
                If TR_Patents.Visible = True Then
                    For Each tn As TreeNode In TreeView_Patents.Nodes
                        If tn.Value = "PATENTS" Then
                            If tn.ChildNodes.Count > 0 Then
                                For Each cTn As TreeNode In tn.ChildNodes
                                    If cTn.Value = "PATENT_NO" Or cTn.Value = "PATENTEE" Or cTn.Value = "PATENT_INVENTOR" Then
                                        cTn.Checked = True
                                    End If
                                    If cTn.Checked = True Then
                                        If myCatString = Nothing Then
                                            myCatString = cTn.Value
                                        Else
                                            myCatString = myCatString + ";" + cTn.Value
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next
                End If
                If TR_Standards.Visible = True Then
                    For Each tn As TreeNode In TreeView_SP.Nodes
                        If tn.Value = "Standard Specification" Then
                            If tn.ChildNodes.Count > 0 Then
                                For Each cTn As TreeNode In tn.ChildNodes
                                    If cTn.Value = "SP_NO" Then
                                        cTn.Checked = True
                                    End If
                                    If cTn.Checked = True Then
                                        If myCatString = Nothing Then
                                            myCatString = cTn.Value
                                        Else
                                            myCatString = myCatString + ";" + cTn.Value
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next
                End If

                If TR_NBM.Visible = True Then
                    For Each tn As TreeNode In TreeView_NBM.Nodes
                        If tn.Value = "NBM" Then
                            If tn.ChildNodes.Count > 0 Then
                                For Each cTn As TreeNode In tn.ChildNodes
                                    If cTn.Checked = True Then
                                        If myCatString = Nothing Then
                                            myCatString = cTn.Value
                                        Else
                                            myCatString = myCatString + ";" + cTn.Value
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next
                End If

                If TR_Titles.Visible = True Then
                    For Each tn As TreeNode In TreeView_Titles.Nodes
                        If tn.Value = "TITLE_STATEMENTS" Then
                            If tn.ChildNodes.Count > 0 Then
                                For Each cTn As TreeNode In tn.ChildNodes
                                    If cTn.Value = "TITLE" Then
                                        cTn.Checked = True
                                    End If
                                    If cTn.Checked = True Then
                                        If myCatString = Nothing Then
                                            myCatString = cTn.Value
                                        Else
                                            myCatString = myCatString + ";" + cTn.Value
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next
                End If


                If TR_Authors.Visible = True Then
                    For Each tn As TreeNode In TreeView_Authors.Nodes
                        If tn.Value = "RESPONSIBILITIES" Then
                            If tn.ChildNodes.Count > 0 Then
                                For Each cTn As TreeNode In tn.ChildNodes
                                    If cTn.Checked = True Then
                                        If myCatString = Nothing Then
                                            myCatString = cTn.Value
                                        Else
                                            myCatString = myCatString + ";" + cTn.Value
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next
                End If
                If TR_CorpAuthors.Visible = True Then
                    For Each tn As TreeNode In TreeView_CorpAuthor.Nodes
                        If tn.Value = "Corporate Author" Then
                            If tn.ChildNodes.Count > 0 Then
                                For Each cTn As TreeNode In tn.ChildNodes
                                    If cTn.Checked = True Then
                                        If myCatString = Nothing Then
                                            myCatString = cTn.Value
                                        Else
                                            myCatString = myCatString + ";" + cTn.Value
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next
                End If
                If TR_Edition.Visible = True Then
                    For Each tn As TreeNode In TreeView_Edition.Nodes
                        If tn.Value = "Edition Statement" Then
                            If tn.ChildNodes.Count > 0 Then
                                For Each cTn As TreeNode In tn.ChildNodes
                                    If cTn.Checked = True Then
                                        If myCatString = Nothing Then
                                            myCatString = cTn.Value
                                        Else
                                            myCatString = myCatString + ";" + cTn.Value
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next
                End If
                If TR_Imprint.Visible = True Then
                    For Each tn As TreeNode In TreeView_Imprint.Nodes
                        If tn.Value = "IMPRINT" Then
                            If tn.ChildNodes.Count > 0 Then
                                For Each cTn As TreeNode In tn.ChildNodes
                                    'If cTn.Value = "PUB_ID" Or cTn.Value = "PLACE_OF_PUB" Or cTn.Value = "YEAR_OF_PUB" Then
                                    '    cTn.Checked = True
                                    'End If
                                    If cTn.Checked = True Then
                                        If myCatString = Nothing Then
                                            myCatString = cTn.Value
                                        Else
                                            myCatString = myCatString + ";" + cTn.Value
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next
                End If
                If TR_Series.Visible = True Then
                    For Each tn As TreeNode In TreeView_Sereis.Nodes
                        If tn.Value = "Series Statement" Then
                            If tn.ChildNodes.Count > 0 Then
                                For Each cTn As TreeNode In tn.ChildNodes
                                    If cTn.Checked = True Then
                                        If myCatString = Nothing Then
                                            myCatString = cTn.Value
                                        Else
                                            myCatString = myCatString + ";" + cTn.Value
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next
                End If
                If TR_Notes.Visible = True Then
                    For Each tn As TreeNode In TreeView_Note.Nodes
                        If tn.Value = "Note Area" Then
                            If tn.ChildNodes.Count > 0 Then
                                For Each cTn As TreeNode In tn.ChildNodes
                                    If cTn.Value = "MULTI_VOL" Then
                                        cTn.Checked = True
                                    End If
                                    If cTn.Checked = True Then
                                        If myCatString = Nothing Then
                                            myCatString = cTn.Value
                                        Else
                                            myCatString = myCatString + ";" + cTn.Value
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next
                End If
                If TR_Abstracts.Visible = True Then
                    For Each tn As TreeNode In TreeView_Subject.Nodes
                        If tn.Value = "Subjet and Keywords" Then
                            If tn.ChildNodes.Count > 0 Then
                                For Each cTn As TreeNode In tn.ChildNodes
                                    If cTn.Checked = True Then
                                        If myCatString = Nothing Then
                                            myCatString = cTn.Value
                                        Else
                                            myCatString = myCatString + ";" + cTn.Value
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next
                End If
                If TR_Others.Visible = True Then
                    For Each tn As TreeNode In TreeView_Other.Nodes
                        If tn.Value = "OTHERS" Then
                            If tn.ChildNodes.Count > 0 Then
                                For Each cTn As TreeNode In tn.ChildNodes
                                    If cTn.Value = "CON_CODE" Then
                                        cTn.Checked = True
                                    End If
                                    If cTn.Checked = True Then
                                        If myCatString = Nothing Then
                                            myCatString = cTn.Value
                                        Else
                                            myCatString = myCatString + ";" + cTn.Value
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next
                End If

                'holdings format
                Dim myHoldString As String = Nothing               
                If TR_Holdings.Visible = True Then
                    For Each tn As TreeNode In TreeView_Holdings.Nodes
                        If tn.Value = "HOLDINGS" Then
                            If tn.ChildNodes.Count > 0 Then
                                For Each cTn As TreeNode In tn.ChildNodes
                                    If cTn.Value = "ACCESSION_NO" Or cTn.Value = "ACCESSION_DATE" Or cTn.Value = "STA_CODE" Then
                                        cTn.Checked = True
                                    End If

                                    If cTn.Checked = True Then
                                        If myHoldString = Nothing Then
                                            myHoldString = cTn.Value
                                        Else
                                            myHoldString = myHoldString + ";" + cTn.Value
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next
                End If

                If Not String.IsNullOrEmpty(myCatString) Then
                    myCatString = " " & myCatString & " "
                    If InStr(1, myCatString, " CREATE ", 1) > 0 Or InStr(1, myCatString, " DELETE ", 1) > 0 Or InStr(1, myCatString, " DROP ", 1) > 0 Or InStr(1, myCatString, " INSERT ", 1) > 1 Or InStr(1, myCatString, " TRACK ", 1) > 1 Or InStr(1, myCatString, " TRACE ", 1) > 1 Then
                        Label6.Text = " Do not use Reserve Words"
                        Exit Sub
                    End If
                    myCatString = TrimX(myCatString)
                Else
                    Label6.Text = "You Selected Nothing"
                    Exit Sub
                End If

                ' check unwanted characters
                c = 0
                counter1 = 0
                For iloop = 1 To Len(myCatString)
                    strcurrentchar = Mid(myCatString, iloop, 1)
                    If c = 0 Then
                        If Not InStr("'~!@#$^&*=+|[]{}?<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter1 = 1
                        End If
                    End If
                Next
                If counter1 = 1 Then
                    Label6.Text = "Do Not Use Un-wanted Characters... "
                    Exit Sub
                End If

                '********************************************************************************************************
                'doc code
                Dim myDocCode As String = Nothing
                If DropDownList1.Text <> "" Then
                    myDocCode = Trim(DropDownList1.SelectedValue)
                    myDocCode = RemoveQuotes(myDocCode)
                    If myDocCode.Length > 4 Then
                        Label6.Text = "Input is not of proper length... "
                        Me.DropDownList1.Focus()
                        Exit Sub
                    End If
                    myDocCode = " " & myDocCode & " "
                    If InStr(1, myDocCode, "CREATE", 1) > 0 Or InStr(1, myDocCode, "DELETE", 1) > 0 Or InStr(1, myDocCode, "DROP", 1) > 0 Or InStr(1, myDocCode, "INSERT", 1) > 1 Or InStr(1, myDocCode, "TRACK", 1) > 1 Or InStr(1, myDocCode, "TRACE", 1) > 1 Then
                        Label6.Text = " Do not use Reserve Words"
                        Me.DropDownList1.Focus()
                        Exit Sub
                    End If
                    myDocCode = TrimAll(myDocCode)
                    'check unwanted characters
                    c = 0
                    counter2 = 0
                    For iloop = 1 To Len(myDocCode)
                        strcurrentchar = Mid(myDocCode, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter2 = 1
                            End If
                        End If
                    Next
                    If counter2 = 1 Then
                        Label6.Text = "Do not use un-wanted letters..."
                        Me.DropDownList1.Focus()
                        Exit Sub
                    End If
                Else
                    Label6.Text = "Plz Select document from Drop-DownList..."
                    Me.DropDownList1.Focus()
                    Exit Sub
                End If

                '********************************************************************************************************
                'doc name
                Dim myDocName As String = Nothing
                If DropDownList1.Text <> "" Then
                    myDocName = Trim(DropDownList1.SelectedItem.Text)
                    myDocName = RemoveQuotes(myDocName)
                    If myDocName.Length > 100 Then
                        Label6.Text = "Input is not of proper length... "
                        Me.DropDownList1.Focus()
                        Exit Sub
                    End If
                    myDocName = " " & myDocName & " "
                    If InStr(1, myDocName, "CREATE", 1) > 0 Or InStr(1, myDocName, "DELETE", 1) > 0 Or InStr(1, myDocName, "DROP", 1) > 0 Or InStr(1, myDocName, "INSERT", 1) > 1 Or InStr(1, myDocName, "TRACK", 1) > 1 Or InStr(1, myDocName, "TRACE", 1) > 1 Then
                        Label6.Text = " Do not use Reserve Words"
                        Me.DropDownList1.Focus()
                        Exit Sub
                    End If
                    myDocName = TrimAll(myDocName)
                    'check unwanted characters
                    c = 0
                    counter3 = 0
                    For iloop = 1 To Len(myDocName)
                        strcurrentchar = Mid(myDocName, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter3 = 1
                            End If
                        End If
                    Next
                    If counter3 = 1 Then
                        Label6.Text = "Do not use un-wanted letters... "
                        Me.DropDownList1.Focus()
                        Exit Sub
                    End If
                Else
                    Label6.Text = "Plz Select document type from Drop-DownList... "
                    Me.DropDownList1.Focus()
                    Exit Sub
                End If

                'Check Duplicate format
                Dim str As Object = Nothing
                Dim flag As Object = Nothing
                str = "SELECT DEF_ID FROM DEFORMATS WHERE (LIB_CODE = '" & Trim(LibCode) & "'  AND DOC_TYPE_CODE = '" & Trim(myDocCode) & "') "
                Dim cmd1 As New SqlCommand(str, SqlConn)
                SqlConn.Open()
                flag = cmd1.ExecuteScalar
                If flag <> Nothing Then
                    Label6.Text = "Document Format already exists.. "
                    Exit Sub
                End If
                SqlConn.Close()
                '**********************************************************

                'INSERT THE RECORD IN TO THE DATABASE
                Dim SQL As String
                Dim Cmd As SqlCommand
                Dim da As SqlDataAdapter
                Dim ds As New DataSet
                Dim CB As SqlCommandBuilder
                Dim dtrow As DataRow
                SQL = "SELECT * FROM DEFORMATS WHERE (DEF_ID = '00')"
                Cmd = New SqlCommand(SQL, SqlConn)
                da = New SqlDataAdapter(Cmd)
                CB = New SqlCommandBuilder(da)
                SqlConn.Open()
                da.Fill(ds, "STable")
                dtrow = ds.Tables("STable").NewRow

                If Not String.IsNullOrEmpty(myDocCode) Then
                    dtrow("DOC_TYPE_CODE") = myDocCode.Trim
                End If
                If Not String.IsNullOrEmpty(myDocName) Then
                    dtrow("DOC_TYPE_NAME") = myDocName.Trim
                End If
                If Not String.IsNullOrEmpty(myCatString) Then
                    dtrow("CAT_FORMAT") = myCatString.Trim
                End If
                If Not String.IsNullOrEmpty(myHoldString) Then
                    dtrow("HOLD_FORMAT") = myHoldString.Trim
                End If
                dtrow("DATE_ADDED") = Now.ToShortDateString
                dtrow("USER_CODE") = Session.Item("LoggedUser")
                dtrow("LIB_CODE") = Session.Item("LoggedLibcode")
                dtrow("IP") = Request.UserHostAddress.Trim

                ds.Tables("STable").Rows.Add(dtrow)
                thisTransaction = SqlConn.BeginTransaction()
                da.SelectCommand.Transaction = thisTransaction
                da.Update(ds, "STable")
                thisTransaction.Commit()
                ds.Dispose()
                Label6.Text = "Record Added Successfully!"
            Catch q As SqlException
                thisTransaction.Rollback()
            Catch ex As Exception
                Label6.Text = "Error: " & (ex.Message())
            Finally
                SqlConn.Close()
            End Try
        End If
    End Sub
    ''update the record
    Protected Sub bttn_Update_Click(ByVal sender As Object, ByVal e As EventArgs) Handles bttn_Update.Click
        Dim SQL As String = Nothing
        Dim da As SqlDataAdapter
        Dim cb As SqlCommandBuilder
        Dim ds As New DataSet
        Dim dt As New DataTable
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing
        Try
            If IsPostBack = True Then
                'check unwanted characters
                Dim iloop As Integer
                Dim strcurrentchar As Object
                Dim c As Integer
                Dim counter1, counter2, counter3 As Integer

                Dim myCatString As String = Nothing
                For Each tn As TreeNode In TreeView_General.Nodes
                    If tn.Value = "General Fields" Then
                        If tn.ChildNodes.Count > 0 Then
                            For Each cTn As TreeNode In tn.ChildNodes
                                If cTn.Checked = True Then
                                    If myCatString = Nothing Then
                                        myCatString = cTn.Value
                                    Else
                                        myCatString = myCatString + ";" + cTn.Value
                                    End If
                                End If
                            Next
                        End If
                    End If
                Next

                If TR_Reports.Visible = True Then
                    For Each tn As TreeNode In TreeView_Reports.Nodes
                        If tn.Value = "REPORTS" Then
                            If tn.ChildNodes.Count > 0 Then
                                For Each cTn As TreeNode In tn.ChildNodes
                                    If cTn.Value = "REPORT_NO" Then
                                        cTn.Checked = True
                                    End If
                                    If cTn.Checked = True Then
                                        If myCatString = Nothing Then
                                            myCatString = cTn.Value
                                        Else
                                            myCatString = myCatString + ";" + cTn.Value
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next
                End If

                If TR_ISBN.Visible = True Then
                    For Each tn As TreeNode In TreeView_ISBN.Nodes
                        If tn.Value = "STD_NO" Then
                            If tn.ChildNodes.Count > 0 Then
                                For Each cTn As TreeNode In tn.ChildNodes
                                    If cTn.Checked = True Then
                                        If myCatString = Nothing Then
                                            myCatString = cTn.Value
                                        Else
                                            myCatString = myCatString + ";" + cTn.Value
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next
                End If

                If TR_Manuals.Visible = True Then
                    For Each tn As TreeNode In TreeView_Manuals.Nodes
                        If tn.Value = "MANUALS" Then
                            If tn.ChildNodes.Count > 0 Then
                                For Each cTn As TreeNode In tn.ChildNodes
                                    If cTn.Value = "MANUAL_NO" Then
                                        cTn.Checked = True
                                    End If
                                    If cTn.Checked = True Then
                                        If myCatString = Nothing Then
                                            myCatString = cTn.Value
                                        Else
                                            myCatString = myCatString + ";" + cTn.Value
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next
                End If
                If TR_Patents.Visible = True Then
                    For Each tn As TreeNode In TreeView_Patents.Nodes
                        If tn.Value = "PATENTS" Then
                            If tn.ChildNodes.Count > 0 Then
                                For Each cTn As TreeNode In tn.ChildNodes
                                    If cTn.Value = "PATENT_NO" Or cTn.Value = "PATENTEE" Or cTn.Value = "PATENT_INVENTOR" Then
                                        cTn.Checked = True
                                    End If
                                    If cTn.Checked = True Then
                                        If myCatString = Nothing Then
                                            myCatString = cTn.Value
                                        Else
                                            myCatString = myCatString + ";" + cTn.Value
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next
                End If
                If TR_Standards.Visible = True Then
                    For Each tn As TreeNode In TreeView_SP.Nodes
                        If tn.Value = "Standard Specification" Then
                            If tn.ChildNodes.Count > 0 Then
                                For Each cTn As TreeNode In tn.ChildNodes
                                    If cTn.Value = "SP_NO" Then
                                        cTn.Checked = True
                                    End If
                                    If cTn.Checked = True Then
                                        If myCatString = Nothing Then
                                            myCatString = cTn.Value
                                        Else
                                            myCatString = myCatString + ";" + cTn.Value
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next
                End If

                 If TR_NBM.Visible = True Then
                    For Each tn As TreeNode In TreeView_NBM.Nodes
                        If tn.Value = "NBM" Then
                            If tn.ChildNodes.Count > 0 Then
                                For Each cTn As TreeNode In tn.ChildNodes
                                    If cTn.Checked = True Then
                                        If myCatString = Nothing Then
                                            myCatString = cTn.Value
                                        Else
                                            myCatString = myCatString + ";" + cTn.Value
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next
                End If

                If TR_Titles.Visible = True Then
                    For Each tn As TreeNode In TreeView_Titles.Nodes
                        If tn.Value = "TITLE_STATEMENTS" Then
                            If tn.ChildNodes.Count > 0 Then
                                For Each cTn As TreeNode In tn.ChildNodes
                                    If cTn.Value = "TITLE" Then
                                        cTn.Checked = True
                                    End If
                                    If cTn.Checked = True Then
                                        If myCatString = Nothing Then
                                            myCatString = cTn.Value
                                        Else
                                            myCatString = myCatString + ";" + cTn.Value
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next
                End If


                If TR_Authors.Visible = True Then
                    For Each tn As TreeNode In TreeView_Authors.Nodes
                        If tn.Value = "RESPONSIBILITIES" Then
                            If tn.ChildNodes.Count > 0 Then
                                For Each cTn As TreeNode In tn.ChildNodes
                                    If cTn.Checked = True Then
                                        If myCatString = Nothing Then
                                            myCatString = cTn.Value
                                        Else
                                            myCatString = myCatString + ";" + cTn.Value
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next
                End If
                If TR_CorpAuthors.Visible = True Then
                    For Each tn As TreeNode In TreeView_CorpAuthor.Nodes
                        If tn.Value = "Corporate Author" Then
                            If tn.ChildNodes.Count > 0 Then
                                For Each cTn As TreeNode In tn.ChildNodes
                                    If cTn.Checked = True Then
                                        If myCatString = Nothing Then
                                            myCatString = cTn.Value
                                        Else
                                            myCatString = myCatString + ";" + cTn.Value
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next
                End If
                If TR_Edition.Visible = True Then
                    For Each tn As TreeNode In TreeView_Edition.Nodes
                        If tn.Value = "Edition Statement" Then
                            If tn.ChildNodes.Count > 0 Then
                                For Each cTn As TreeNode In tn.ChildNodes
                                    If cTn.Checked = True Then
                                        If myCatString = Nothing Then
                                            myCatString = cTn.Value
                                        Else
                                            myCatString = myCatString + ";" + cTn.Value
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next
                End If
                If TR_Imprint.Visible = True Then
                    For Each tn As TreeNode In TreeView_Imprint.Nodes
                        If tn.Value = "IMPRINT" Then
                            If tn.ChildNodes.Count > 0 Then
                                For Each cTn As TreeNode In tn.ChildNodes
                                    'If cTn.Value = "PUB_ID" Or cTn.Value = "PLACE_OF_PUB" Then
                                    '    cTn.Checked = True
                                    'End If
                                    If cTn.Checked = True Then
                                        If myCatString = Nothing Then
                                            myCatString = cTn.Value
                                        Else
                                            myCatString = myCatString + ";" + cTn.Value
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next
                End If
                If TR_Series.Visible = True Then
                    For Each tn As TreeNode In TreeView_Sereis.Nodes
                        If tn.Value = "Series Statement" Then
                            If tn.ChildNodes.Count > 0 Then
                                For Each cTn As TreeNode In tn.ChildNodes
                                    If cTn.Checked = True Then
                                        If myCatString = Nothing Then
                                            myCatString = cTn.Value
                                        Else
                                            myCatString = myCatString + ";" + cTn.Value
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next
                End If
                If TR_Notes.Visible = True Then
                    For Each tn As TreeNode In TreeView_Note.Nodes
                        If tn.Value = "Note Area" Then
                            If tn.ChildNodes.Count > 0 Then
                                For Each cTn As TreeNode In tn.ChildNodes
                                    If cTn.Value = "MULTI_VOL" Then
                                        cTn.Checked = True
                                    End If
                                    If cTn.Checked = True Then
                                        If myCatString = Nothing Then
                                            myCatString = cTn.Value
                                        Else
                                            myCatString = myCatString + ";" + cTn.Value
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next
                End If
                If TR_Abstracts.Visible = True Then
                    For Each tn As TreeNode In TreeView_Subject.Nodes
                        If tn.Value = "Subjet and Keywords" Then
                            If tn.ChildNodes.Count > 0 Then
                                For Each cTn As TreeNode In tn.ChildNodes
                                    If cTn.Checked = True Then
                                        If myCatString = Nothing Then
                                            myCatString = cTn.Value
                                        Else
                                            myCatString = myCatString + ";" + cTn.Value
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next
                End If
                If TR_Others.Visible = True Then
                    For Each tn As TreeNode In TreeView_Other.Nodes
                        If tn.Value = "OTHERS" Then
                            If tn.ChildNodes.Count > 0 Then
                                For Each cTn As TreeNode In tn.ChildNodes
                                    If cTn.Value = "CON_CODE" Then
                                        cTn.Checked = True
                                    End If
                                    If cTn.Checked = True Then
                                        If myCatString = Nothing Then
                                            myCatString = cTn.Value
                                        Else
                                            myCatString = myCatString + ";" + cTn.Value
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next
                End If

                'holdings format
                Dim myHoldString As String = Nothing
                If TR_Holdings.Visible = True Then
                    For Each tn As TreeNode In TreeView_Holdings.Nodes
                        If tn.Value = "HOLDINGS" Then
                            If tn.ChildNodes.Count > 0 Then
                                For Each cTn As TreeNode In tn.ChildNodes
                                    If cTn.Value = "ACCESSION_NO" Or cTn.Value = "ACCESSION_DATE" Then
                                        cTn.Checked = True
                                    End If

                                    If cTn.Checked = True Then
                                        If myHoldString = Nothing Then
                                            myHoldString = cTn.Value
                                        Else
                                            myHoldString = myHoldString + ";" + cTn.Value
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next
                End If

                If Not String.IsNullOrEmpty(myCatString) Then
                    myCatString = " " & myCatString & " "
                    If InStr(1, myCatString, " CREATE ", 1) > 0 Or InStr(1, myCatString, " DELETE ", 1) > 0 Or InStr(1, myCatString, " DROP ", 1) > 0 Or InStr(1, myCatString, " INSERT ", 1) > 1 Or InStr(1, myCatString, " TRACK ", 1) > 1 Or InStr(1, myCatString, " TRACE ", 1) > 1 Then
                        Label6.Text = " Do not use Reserve Words"
                        Exit Sub
                    End If
                    myCatString = TrimX(myCatString)
                Else
                    Label6.Text = "You Selected Nothing"
                    Exit Sub
                End If

                ' check unwanted characters
                c = 0
                counter1 = 0
                For iloop = 1 To Len(myCatString)
                    strcurrentchar = Mid(myCatString, iloop, 1)
                    If c = 0 Then
                        If Not InStr("'~!@#$^&*=+|[]{}?<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter1 = 1
                        End If
                    End If
                Next
                If counter1 = 1 Then
                    Label6.Text = "Do Not Use Un-wanted Characters... "
                    Exit Sub
                End If


                '****************************************************************************************************
                'UPDATE THE format PROFILE   
                SQL = "SELECT * FROM DEFORMATS WHERE (DEF_ID = '" & Trim(Label10.Text) & "')"
                SqlConn.Open()
                da = New SqlDataAdapter(SQL, SqlConn)
                cb = New SqlCommandBuilder(da)
                da.Fill(ds, "DEFORMATS")
                If ds.Tables("DEFORMATS").Rows.Count <> 0 Then
                    If Not String.IsNullOrEmpty(myCatString) Then
                        ds.Tables("DEFORMATS").Rows(0)("CAT_FORMAT") = myCatString.Trim
                    Else
                        ds.Tables("DEFORMATS").Rows(0)("CAT_FORMAT") = System.DBNull.Value
                    End If
                    If Not String.IsNullOrEmpty(myHoldString) Then
                        ds.Tables("DEFORMATS").Rows(0)("HOLD_FORMAT") = myHoldString.Trim
                    Else
                        ds.Tables("DEFORMATS").Rows(0)("HOLD_FORMAT") = System.DBNull.Value
                    End If
                   
                    ds.Tables("DEFORMATS").Rows(0)("DATE_MODIFIED") = Now.Date
                    ds.Tables("DEFORMATS").Rows(0)("USER_CODE") = Session.Item("LoggedUser")
                    ds.Tables("DEFORMATS").Rows(0)("LIB_CODE") = Session.Item("LoggedLibcode")

                    thisTransaction = SqlConn.BeginTransaction()
                    da.SelectCommand.Transaction = thisTransaction
                    da.Update(ds, "DEFORMATS")
                    thisTransaction.Commit()
                    myCatString = Nothing
                    myHoldString = Nothing
                    Label6.Visible = True
                    Label6.Text = "Record Updated Successfully"
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' System Record Updated Successfully... ');", True)
                Else
                    Label6.Text = "Record Not Updated  - Please Contact System Administrator ! "
                End If
            End If
            SqlConn.Close()
        Catch q As SqlException
            thisTransaction.Rollback()
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'delete format
    Protected Sub bttn_Delete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles bttn_Delete.Click
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing
        Try
            If IsPostBack = True Then
                If Label10.Text <> "" Then
                    ' Dim SQL As String = Nothing
                    ' SQL = "DELETE FROM DEFORMATS WHERE (DEF_ID ='" & Trim(Label10.Text) & "') "
                    If SqlConn.State = 0 Then
                        SqlConn.Open()
                    End If
                    thisTransaction = SqlConn.BeginTransaction()

                    Dim objCommand As New SqlCommand
                    objCommand.Connection = SqlConn
                    objCommand.Transaction = thisTransaction
                    objCommand.CommandType = CommandType.Text
                    objCommand.CommandText = "DELETE FROM DEFORMATS WHERE (DEF_ID =@myID) "

                    objCommand.Parameters.Add("@myID", SqlDbType.Int)
                    objCommand.Parameters("@myID").Value = Label10.Text

                    objCommand.ExecuteNonQuery()
                    thisTransaction.Commit()
                    ' PopulateNodes()
                    Label6.Text = "Record Deleted Successfully!"
                Else
                    Label6.Text = "No Record Exists!"
                    Exit Sub
                End If
            End If
        Catch q As SqlException
            thisTransaction.Rollback()
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub

    Protected Sub TreeView_Holdings_SelectedNodeChanged(ByVal sender As Object, ByVal e As EventArgs) Handles TreeView_Holdings.SelectedNodeChanged

    End Sub
End Class