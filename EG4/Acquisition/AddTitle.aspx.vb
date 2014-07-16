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

Public Class AddTitle
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
                        GetUserSettings()
                        PopulateBibCodes()
                        DDL_Bib_Level.SelectedValue = "M"
                        Me.DDL_Bib_Level_SelectedIndexChanged(sender, e)
                        DDL_Mat_Type.SelectedValue = "B"
                        DDL_Mat_Type_SelectedIndexChanged(sender, e)
                        DDL_Doc_Type.SelectedValue = "BK"
                        DDL_Doc_Type_SelectedIndexChanged(sender, e)
                        PopulateLanguages()
                        DDL_Lang.SelectedValue = "ENG"
                        PopulateCountries()
                        DDL_Countries.SelectedValue = ""
                        PopulatePub()
                        PopulateSubjects()

                        DDL_Government.ClearSelection()
                        Me.Acq_Save_Bttn.Visible = True
                        Acq_Cancel_Bttn.Visible = True
                        Acq_Delete_Bttn.Visible = False
                        Acq_Update_Bttn.Visible = False
                        Acq_SaveNew_Bttn.Visible = False
                        Label15.Text = "Enter Data and Press SAVE Button to save the record.."
                        Label6.Text = ""

                        If Acq_Update_Bttn.Visible = False Then
                            txt_Cat_Year.Text = Now.Year
                        Else
                            txt_Display_Value.Focus()
                        End If
                    End If
                    Dim CreateSUserButton As Object 'System.Web.UI.WebControls.Button
                    CreateSUserButton = Master.FindControl("Accordion1").FindControl("AcqPane").FindControl("Acq_AddTitle_Bttn")
                    CreateSUserButton.ForeColor = Drawing.Color.Red
                    myPaneName = "AcqPane"
                End If
            Else
                Response.Redirect("~/Default.aspx", False)
            End If
            LibCode = Session.Item("LoggedLibcode")
        Catch ex As Exception
            Label6.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    Public Sub PopulateBibCodes()
        Me.DDL_Bib_Level.DataTextField = "BIB_NAME"
        Me.DDL_Bib_Level.DataValueField = "BIB_CODE"
        Me.DDL_Bib_Level.DataSource = GetBibLevelist()
        Me.DDL_Bib_Level.DataBind()
    End Sub
    'select materials type on selection of bib levels
    Protected Sub DDL_Bib_Level_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDL_Bib_Level.SelectedIndexChanged
        Dim Command As SqlCommand = Nothing
        Dim dt As DataTable = Nothing
        Try
            Dim myBibName As Object = Nothing
            If DDL_Bib_Level.Text <> "" Then
                myBibName = DDL_Bib_Level.SelectedValue
            Else
                myBibName = ""
            End If

            If myBibName = "S" Then
                Label6.Text = "This Form can not be used to Add SERIALS - use SERIALS Module for such materials.)"
                Label15.Text = ""
                DDL_Mat_Type.Items.Clear()
                DDL_Doc_Type.Items.Clear()
                DDL_Bib_Level.Focus()
                Exit Sub
            End If
                Command = New SqlCommand("SELECT  MAT_CODE, MAT_NAME FROM MATERIALS WHERE (BIB_CODE = '" & Trim(myBibName) & "') ORDER BY MAT_NAME ", SqlConn)
                If SqlConn.State = 0 Then
                    SqlConn.Open()
                End If
                Dim da As New SqlDataAdapter(Command)
                Dim ds As New DataSet
                Dim RecordCount As Long = 0
                da.Fill(ds)

                dt = ds.Tables(0).Copy

                Dim Dr As DataRow
                Dr = dt.NewRow
                Dr("MAT_CODE") = ""
                Dr("MAT_NAME") = ""
                dt.Rows.InsertAt(Dr, 0)

                If dt.Rows.Count = 0 Then
                    Me.DDL_Mat_Type.DataSource = Nothing
                Else
                    Me.DDL_Mat_Type.DataSource = dt
                    Me.DDL_Mat_Type.DataTextField = "MAT_NAME"
                    Me.DDL_Mat_Type.DataValueField = "MAT_CODE"
                    Me.DDL_Mat_Type.DataBind()
                End If
                dt.Dispose()
            SqlConn.Close()
            DDL_Doc_Type.Items.Clear()
            DDL_Bib_Level.Focus()
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
            Label15.Text = ""
        Finally
            SqlConn.Close()
        End Try

    End Sub
    'select doc type on selection of materials
    Public Sub DDL_Mat_Type_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDL_Mat_Type.SelectedIndexChanged
        Dim Command As SqlCommand = Nothing
        Dim dt As DataTable = Nothing
        Try
            Dim myMatName As Object = Nothing
            If DDL_Mat_Type.Text <> "" Then
                myMatName = DDL_Mat_Type.SelectedValue
            Else
                myMatName = ""
            End If
            Command = New SqlCommand("SELECT  DOC_TYPE_CODE, DOC_TYPE_NAME FROM DOC_TYPES WHERE (MAT_CODE = '" & Trim(myMatName) & "') ORDER BY DOC_TYPE_NAME ", SqlConn)
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
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
                Me.DDL_Doc_Type.DataSource = Nothing
            Else
                Me.DDL_Doc_Type.DataSource = dt
                Me.DDL_Doc_Type.DataTextField = "DOC_TYPE_NAME"
                Me.DDL_Doc_Type.DataValueField = "DOC_TYPE_CODE"
                Me.DDL_Doc_Type.DataBind()
            End If
            dt.Dispose()
            SqlConn.Close()
            DDL_Mat_Type.Focus()
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
            Label15.Text = ""
        Finally
            Command.Dispose()
            dt.Dispose()
            SqlConn.Close()
        End Try

    End Sub
    'populate lang
    Public Sub PopulateLanguages()
        Me.DDL_Lang.DataSource = GetLanguageList()
        Me.DDL_Lang.DataTextField = "LANG_NAME"
        Me.DDL_Lang.DataValueField = "LANG_CODE"
        Me.DDL_Lang.DataBind()
    End Sub
    Public Sub PopulateCountries()
        Me.DDL_Countries.DataSource = GetCountryList()
        Me.DDL_Countries.DataTextField = "CON_NAME"
        Me.DDL_Countries.DataValueField = "CON_CODE"
        Me.DDL_Countries.DataBind()
        DDL_Countries.Items.Insert(0, "")
    End Sub
    Public Sub PopulatePub()
        Pub_ComboBox.DataTextField = "PUB_NAME"
        Pub_ComboBox.DataValueField = "PUB_ID"
        Pub_ComboBox.DataSource = GetPublisherList()
        Pub_ComboBox.DataBind()
        Pub_ComboBox.Items.Insert(0, "")
    End Sub
    Public Sub PopulateSubjects()
        Me.DDL_Subjects.DataSource = GetSubjectList()
        Me.DDL_Subjects.DataTextField = "SUB_NAME"
        Me.DDL_Subjects.DataValueField = "SUB_ID"
        Me.DDL_Subjects.DataBind()
        DDL_Subjects.Items.Insert(0, "")
    End Sub
    'get place country on selection of publisher
    Protected Sub Pub_ComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Pub_ComboBox.SelectedIndexChanged
        Dim Command As SqlCommand = Nothing
        Dim dt As DataTable = Nothing
        If Me.Acq_Save_Bttn.Visible = True Then
            Try
                Dim myPubID As Object = Nothing
                If Pub_ComboBox.Text <> "" Then
                    myPubID = Pub_ComboBox.SelectedValue
                    Command = New SqlCommand("SELECT  PUB_PLACE, CON_CODE FROM PUBLISHERS WHERE (PUB_ID = '" & Trim(myPubID) & "') ", SqlConn)
                    If SqlConn.State = 0 Then
                        SqlConn.Open()
                    End If
                    Dim da As New SqlDataAdapter(Command)
                    Dim ds As New DataSet
                    Dim RecordCount As Long = 0
                    da.Fill(ds)
                    dt = ds.Tables(0).Copy
                    If dt.Rows.Count <> 0 Then
                        txt_Cat_Place.Text = dt.Rows(0).Item("PUB_PLACE").ToString
                        DDL_Countries.SelectedValue = dt.Rows(0).Item("CON_CODE")
                    Else
                        txt_Cat_Place.Text = ""
                    End If
                    dt.Dispose()
                    SqlConn.Close()
                Else
                    myPubID = 0
                End If
                Pub_ComboBox.Focus()
            Catch s As Exception
                Label6.Text = "Error: " & (s.Message())
                Label15.Text = ""
            Finally
                SqlConn.Close()
            End Try
        Else
            Pub_ComboBox.Focus()
        End If
    End Sub
    'get keywords on selection of subject
    Protected Sub DDL_Subjects_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDL_Subjects.SelectedIndexChanged
        Dim Command As SqlCommand = Nothing
        Dim dt As DataTable = Nothing
        If Me.Acq_Save_Bttn.Visible = True Then
            Try
                Dim mySubID As Object = Nothing
                If DDL_Subjects.Text <> "" Then
                    mySubID = DDL_Subjects.SelectedValue
                    Command = New SqlCommand("SELECT SUB_KEYWORDS FROM SUBJECTS WHERE (SUB_ID = '" & Trim(mySubID) & "') ", SqlConn)
                    If SqlConn.State = 0 Then
                        SqlConn.Open()
                    End If
                    Dim da As New SqlDataAdapter(Command)
                    Dim ds As New DataSet
                    Dim RecordCount As Long = 0
                    da.Fill(ds)
                    dt = ds.Tables(0).Copy

                    If dt.Rows.Count <> 0 Then
                        txt_Cat_Keywords.Text = dt.Rows(0).Item("SUB_KEYWORDS").ToString
                    Else
                        txt_Cat_Keywords.Text = ""
                    End If
                    dt.Dispose()
                    SqlConn.Close()
                Else
                    mySubID = 0
                End If
                DDL_Subjects.Focus()
            Catch s As Exception
                Label6.Text = "Error: " & (s.Message())
                Label15.Text = ""
            Finally
                SqlConn.Close()
            End Try
        Else
            DDL_Subjects.Focus()
        End If
    End Sub
    'load / display fields
    Protected Sub DDL_Doc_Type_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDL_Doc_Type.SelectedIndexChanged
        LoadDeFormats()
    End Sub
    Public Sub LoadDeFormats()
        Dim Command As SqlCommand = Nothing
        Dim dt As DataTable = Nothing
        Try
            Dim myCatFields As Object = Nothing
            Dim myHoldFields As Object = Nothing

            Dim myDocName As Object = Nothing
            If DDL_Doc_Type.Text <> "" Then
                myDocName = DDL_Doc_Type.SelectedValue
            Else
                Exit Sub
            End If

            Dim SQL As String = Nothing
            SQL = "SELECT DEF_ID, CAT_FORMAT  FROM DEFORMATS WHERE (LIB_CODE = '" & Trim(LibCode) & "'  AND DOC_TYPE_CODE = '" & Trim(myDocName) & "') "
            Command = New SqlCommand(SQL, SqlConn)
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            Dim da As New SqlDataAdapter(Command)
            Dim ds As New DataSet
            Dim RecordCount As Long = 0
            da.Fill(ds)
            dt = ds.Tables(0).Copy

            If dt.Rows.Count <> 0 Then
                If dt.Rows(0).Item("CAT_FORMAT").ToString <> "" Then
                    myCatFields = dt.Rows(0).Item("CAT_FORMAT")
                    If myCatFields <> "" Then
                        If InStr(myCatFields, "STANDARD_NO") <> 0 Then
                            TR_ISBN.Visible = True
                        Else
                            TR_ISBN.Visible = False
                        End If
                        If InStr(myCatFields, "SP_NO") <> 0 Then
                            TR_SPNo.Visible = True
                        Else
                            TR_SPNo.Visible = False
                        End If

                        If InStr(myCatFields, "SP_TCSC") <> 0 Then
                            TR_SP_TCSC.Visible = True
                        Else
                            TR_SP_TCSC.Visible = False
                        End If
                        If InStr(myCatFields, "SP_UPDATES") <> 0 Then
                            TR_SP_UPDATES.Visible = True
                        Else
                            TR_SP_UPDATES.Visible = False
                        End If
                        If InStr(myCatFields, "SP_AMMENDMENTS") <> 0 Then
                            TR_SP_AMMENDMENTS.Visible = True
                        Else
                            TR_SP_AMMENDMENTS.Visible = False
                        End If

                        If InStr(myCatFields, "SP_ISSUE_BODY") <> 0 Then
                            TR_SP_ISSUE_BODY.Visible = True
                        Else
                            TR_SP_ISSUE_BODY.Visible = False
                        End If


                        If InStr(myCatFields, "MANUAL_NO") <> 0 Then
                            TR_ManualNo.Visible = True
                        Else
                            TR_ManualNo.Visible = False
                        End If

                        If InStr(myCatFields, "REPORT_NO") <> 0 Then
                            TR_ReportNo.Visible = True
                        Else
                            TR_ReportNo.Visible = False
                        End If

                        If InStr(myCatFields, "SUB_TITLE") <> 0 Then
                            TR_SubTitle.Visible = True
                        Else
                            TR_SubTitle.Visible = False
                        End If
                        If InStr(myCatFields, "VAR_TITLE") <> 0 Then
                            TR_VarTitle.Visible = True
                        Else
                            TR_VarTitle.Visible = False
                        End If
                        If InStr(myCatFields, "SCHOLAR_NAME") <> 0 Then
                            TR_ScholarName.Visible = True
                        Else
                            TR_ScholarName.Visible = False
                        End If
                        If InStr(myCatFields, "SCHOLAR_DEPT") <> 0 Then
                            TR_ScholarDepartment.Visible = True
                        Else
                            TR_ScholarDepartment.Visible = False
                        End If
                        If InStr(myCatFields, "GUIDE_NAME") <> 0 Then
                            TR_GuideName.Visible = True
                        Else
                            TR_GuideName.Visible = False
                        End If
                        If InStr(myCatFields, "GUIDE_DEPT") <> 0 Then
                            TR_GuideDepartment.Visible = True
                        Else
                            TR_GuideDepartment.Visible = False
                        End If
                        If InStr(myCatFields, "DEGREE_NAME") <> 0 Then
                            TR_DegreeName.Visible = True
                        Else
                            TR_DegreeName.Visible = False
                        End If
                        If InStr(myCatFields, "SP_VERSION") <> 0 Then
                            TR_SPRevision.Visible = True
                        Else
                            TR_SPRevision.Visible = False
                        End If
                        If InStr(myCatFields, "MANUAL_VER") <> 0 Then
                            TR_ManualRev.Visible = True
                        Else
                            TR_ManualRev.Visible = False
                        End If
                        If InStr(myCatFields, "PATENT_NO") <> 0 Then
                            TR_PatentNo.Visible = True
                        Else
                            TR_PatentNo.Visible = False
                        End If
                        If InStr(myCatFields, "PATENTEE") <> 0 Then
                            TR_Patentee.Visible = True
                        Else
                            TR_Patentee.Visible = False
                        End If
                        If InStr(myCatFields, "PATENT_INVENTOR") <> 0 Then
                            TR_PatentInventor.Visible = True
                        Else
                            TR_PatentInventor.Visible = False
                        End If
                        If InStr(myCatFields, "CONF_NAME") <> 0 Then
                            TR_ConfName.Visible = True
                        Else
                            TR_ConfName.Visible = False
                        End If
                        If InStr(myCatFields, "CONF_NAME") <> 0 Then
                            TR_ConfDetails.Visible = True
                        Else
                            TR_ConfDetails.Visible = False
                        End If
                        If InStr(myCatFields, "AUTHOR1") <> 0 Then
                            TR_Author.Visible = True
                        Else
                            TR_Author.Visible = False
                        End If
                        If InStr(myCatFields, "EDITOR") <> 0 Then
                            TR_Editor.Visible = True
                        Else
                            TR_Editor.Visible = False
                        End If
                        If InStr(myCatFields, "TRANSLATOR") <> 0 Then
                            TR_Translator.Visible = True
                        Else
                            TR_Translator.Visible = False
                        End If
                        If InStr(myCatFields, "ILLUSTRATOR") <> 0 Then
                            TR_Illustrator.Visible = True
                        Else
                            TR_Illustrator.Visible = False
                        End If
                        If InStr(myCatFields, "COMPILER") <> 0 Then
                            TR_Compiler.Visible = True
                        Else
                            TR_Compiler.Visible = False
                        End If
                        If InStr(myCatFields, "COMMENTATORS") <> 0 Then
                            TR_Commentator.Visible = True
                        Else
                            TR_Commentator.Visible = False
                        End If
                        If InStr(myCatFields, "REVISED_BY") <> 0 Then
                            TR_RevisedBy.Visible = True
                        Else
                            TR_RevisedBy.Visible = False
                        End If
                        If InStr(myCatFields, "CORPORATE_AUTHOR") <> 0 Then
                            TR_CorpAuthor.Visible = True
                        Else
                            TR_CorpAuthor.Visible = False
                        End If
                        If InStr(myCatFields, "EDITION") <> 0 Then
                            TR_Edition.Visible = True
                        Else
                            TR_Edition.Visible = False
                        End If
                        If InStr(myCatFields, "REPRINTS") <> 0 Then
                            TR_Reprint.Visible = True
                        Else
                            TR_Reprint.Visible = False
                        End If
                        If InStr(myCatFields, "PUB_ID") <> 0 Then
                            TR_Publisher.Visible = True
                        Else
                            TR_Publisher.Visible = False
                        End If
                        If InStr(myCatFields, "PLACE_OF_PUB") <> 0 Then
                            TR_Place.Visible = True
                        Else
                            TR_Place.Visible = False
                        End If
                        If InStr(myCatFields, "YEAR_OF_PUB") <> 0 Then
                            TR_Year.Visible = True
                        Else
                            TR_Year.Visible = False
                        End If
                        If InStr(myCatFields, "SERIES_TITLE") <> 0 Then
                            TR_Series.Visible = True
                        Else
                            TR_Series.Visible = False
                        End If
                        If InStr(myCatFields, "SERIES_EDITOR") <> 0 Then
                            TR_SeriesEditor.Visible = True
                        Else
                            TR_SeriesEditor.Visible = False
                        End If
                        If InStr(myCatFields, "NOTE") <> 0 Then
                            TR_Note.Visible = True
                        Else
                            TR_Note.Visible = False
                        End If
                        If InStr(myCatFields, "REMARKS") <> 0 Then
                            TR_Remarks.Visible = True
                        Else
                            TR_Remarks.Visible = False
                        End If
                        If InStr(myCatFields, "URL") <> 0 Then
                            TR_URL.Visible = True
                        Else
                            TR_URL.Visible = False
                        End If
                        If InStr(myCatFields, "COMMENTS") <> 0 Then
                            TR_Comments.Visible = True
                        Else
                            TR_Comments.Visible = False
                        End If
                        If InStr(myCatFields, "SUB_ID") <> 0 Then
                            TR_Subject.Visible = True
                        Else
                            TR_Subject.Visible = False
                        End If
                        If InStr(myCatFields, "KEYWORDS") <> 0 Then
                            TR_Keyword.Visible = True
                        Else
                            TR_Keyword.Visible = False
                        End If
                        If InStr(myCatFields, "TR_FROM") <> 0 Then
                            TR_TranslatedFrom.Visible = True
                        Else
                            TR_TranslatedFrom.Visible = False
                        End If
                        If InStr(myCatFields, "ABSTRACT") <> 0 Then
                            TR_Absract.Visible = True
                        Else
                            TR_Absract.Visible = False
                        End If
                        If InStr(myCatFields, "REFERENCE_NO") <> 0 Then
                            TR_ReferenceNo.Visible = True
                        Else
                            TR_ReferenceNo.Visible = False
                        End If

                        If InStr(myCatFields, "PRODUCER") <> 0 Then
                            TR_Producer.Visible = True
                        Else
                            TR_Producer.Visible = False
                        End If

                        If InStr(myCatFields, "DESIGNER") <> 0 Then
                            TR_Designer.Visible = True
                        Else
                            TR_Designer.Visible = False
                        End If

                        If InStr(myCatFields, "MANUFACTURER") <> 0 Then
                            TR_Manufacturer.Visible = True
                        Else
                            TR_Manufacturer.Visible = False
                        End If

                        If InStr(myCatFields, "CREATOR") <> 0 Then
                            TR_Creator.Visible = True
                        Else
                            TR_Creator.Visible = False
                        End If

                        If InStr(myCatFields, "MATERIALS") <> 0 Then
                            TR_Materials.Visible = True
                        Else
                            TR_Materials.Visible = False
                        End If

                        If InStr(myCatFields, "WORK_CATEGORY") <> 0 Then
                            TR_Work.Visible = True
                        Else
                            TR_Work.Visible = False
                        End If

                        If InStr(myCatFields, "RELATED_WORK") <> 0 Then
                            TR_RelatedWork.Visible = True
                        Else
                            TR_RelatedWork.Visible = False
                        End If

                        If InStr(myCatFields, "SOURCE") <> 0 Then
                            TR_Source.Visible = True
                        Else
                            TR_Source.Visible = False
                        End If

                        If InStr(myCatFields, "PHOTOGRAPHER") <> 0 Then
                            TR_Photographer.Visible = True
                        Else
                            TR_Photographer.Visible = False
                        End If

                        If InStr(myCatFields, "NATIONALITY") <> 0 Then
                            TR_Nationality.Visible = True
                        Else
                            TR_Nationality.Visible = False
                        End If

                        If InStr(myCatFields, "ACT_NO") <> 0 Then
                            TR_ACT.Visible = True
                        Else
                            TR_ACT.Visible = False
                        End If

                        If InStr(myCatFields, "CHAIRMAN") <> 0 Then
                            TR_CHAIRMAN.Visible = True
                        Else
                            TR_CHAIRMAN.Visible = False
                        End If

                        If InStr(myCatFields, "GOVERNMENT") <> 0 Then
                            TR_GOVERNMENT.Visible = True
                        Else
                            TR_GOVERNMENT.Visible = False
                        End If

                        Label15.Text = "Enter Data and Press SAVE Button to save the record.."
                        Label6.Text = ""
                    Else
                        Label6.Text = "Data Entry Format is not proper..Create Data Entry Format for this Document Type in Library Administrator Module"
                        Label15.Text = ""
                    End If
                Else
                    Label6.Text = "Data Entry Format has not been added..Create Data Entry Format for this Document Type in Library Administrator Module"
                    Label15.Text = ""
                End If
            Else
                Label6.Text = "Data Entry Format has not been added..Create Data Entry Format for this Document Type in Library Administrator Module"
                Label15.Text = ""
                TR_ISBN.Visible = False
                TR_SPNo.Visible = False
                TR_SP_TCSC.Visible = False
                TR_SP_AMMENDMENTS.Visible = False
                TR_SP_UPDATES.Visible = False
                TR_SP_ISSUE_BODY.Visible = False
                TR_ManualNo.Visible = False
                TR_ReportNo.Visible = False
                TR_SubTitle.Visible = False
                TR_VarTitle.Visible = False
                TR_ScholarName.Visible = False
                TR_ScholarDepartment.Visible = False
                TR_GuideName.Visible = False
                TR_GuideDepartment.Visible = False
                TR_DegreeName.Visible = False
                TR_SPRevision.Visible = False
                TR_ManualRev.Visible = False
                TR_PatentNo.Visible = False
                TR_Patentee.Visible = False
                TR_PatentInventor.Visible = False
                TR_ConfName.Visible = False
                TR_ConfDetails.Visible = False
                TR_Author.Visible = False
                TR_Editor.Visible = False
                TR_Translator.Visible = False
                TR_Illustrator.Visible = False
                TR_Compiler.Visible = False
                TR_Commentator.Visible = False
                TR_RevisedBy.Visible = False
                TR_CorpAuthor.Visible = False
                TR_Edition.Visible = False
                TR_Reprint.Visible = False
                TR_Publisher.Visible = False
                TR_Place.Visible = False
                TR_Year.Visible = False
                TR_Series.Visible = False
                TR_SeriesEditor.Visible = False
                TR_Note.Visible = False
                TR_Remarks.Visible = False
                TR_URL.Visible = False
                TR_Comments.Visible = False
                TR_Subject.Visible = False
                TR_Keyword.Visible = False
                TR_TranslatedFrom.Visible = False
                TR_Absract.Visible = False
                TR_ReferenceNo.Visible = False
                TR_Producer.Visible = False
                TR_Designer.Visible = False
                TR_Manufacturer.Visible = False
                TR_Creator.Visible = False
                TR_Materials.Visible = False
                TR_Work.Visible = False
                TR_RelatedWork.Visible = False
                TR_Source.Visible = False
                TR_Photographer.Visible = False
                TR_Nationality.Visible = False
            End If
            DDL_Doc_Type.Focus()
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
            Label15.Text = ""
        Finally
            Command.Dispose()
            dt.Dispose()
            SqlConn.Close()
        End Try

    End Sub
    Public Sub GetUserSettings()
        Dim Command As SqlCommand = Nothing
        Dim drUSERS As SqlDataReader = Nothing
        Try
            'get record details from database
            Dim SQL As String = Nothing
            SQL = "SELECT DOWNLOAD_CATS  FROM USERS WHERE (USER_CODE = '" & TrimX(UserCode) & "') "
            Command = New SqlCommand(SQL, SqlConn)
            SqlConn.Open()
            drUSERS = Command.ExecuteReader(CommandBehavior.CloseConnection)
            drUSERS.Read()
            If drUSERS.HasRows = True Then
                If drUSERS.Item("DOWNLOAD_CATS").ToString <> "" And drUSERS.Item("DOWNLOAD_CATS").ToString = "Y" Then
                    CheckBox2.Checked = True
                Else
                    CheckBox2.Checked = False
                End If
            End If
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
            If drUSERS IsNot Nothing Then
                drUSERS.Close()
            Else
                SqlConn.Close()
            End If
        Finally
            Command.Dispose()
            SqlConn.Close()
        End Try
    End Sub
    'Search Catalog
    Public Sub Search_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Search_Bttn.Click
        Dim dtSearch As DataTable = Nothing
        Try
            Dim c, counter1, counter2, counter3, counter4, counter5 As Integer
            Dim iloop As Integer
            Dim strcurrentchar As Object

            'search string validation
            Dim mySearchString As Object = Nothing
            If txt_Search.Text <> "" Then
                mySearchString = TrimAll(txt_Search.Text)
                mySearchString = RemoveQuotes(mySearchString)
                If mySearchString.Length > 250 Then
                    Label6.Text = "Error:  Input is not Valid!"
                    Label15.Text = ""
                    Me.txt_Search.Focus()
                    Exit Sub
                End If
                mySearchString = " " & mySearchString & " "
                If InStr(1, mySearchString, "CREATE", 1) > 0 Or InStr(1, mySearchString, "DELETE", 1) > 0 Or InStr(1, mySearchString, "DROP", 1) > 0 Or InStr(1, mySearchString, "INSERT", 1) > 1 Or InStr(1, mySearchString, "TRACK", 1) > 1 Or InStr(1, mySearchString, "TRACE", 1) > 1 Then
                    Label6.Text = "Error:  Input is not Valid !"
                    Label15.Text = ""
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
                        If Not InStr("';""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter1 = 1
                        End If
                    End If
                Next
                If counter1 = 1 Then
                    Label6.Text = "Error: data is not Valid !"
                    Label15.Text = ""
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
                    Label6.Text = "Error: Input is not Valid !"
                    Label15.Text = ""
                    Me.DropDownList1.Focus()
                    Exit Sub
                End If
                myfield = " " & myfield & " "
                If InStr(1, myfield, "CREATE", 1) > 0 Or InStr(1, myfield, "DELETE", 1) > 0 Or InStr(1, myfield, "DROP", 1) > 0 Or InStr(1, myfield, "INSERT", 1) > 1 Or InStr(1, myfield, "TRACK", 1) > 1 Or InStr(1, myfield, "TRACE", 1) > 1 Then
                    Label6.Text = "Error: Input is not Valid !"
                    Label15.Text = ""
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
                    Label6.Text = "Error: Input is not Valid !"
                    Label15.Text = ""
                    DropDownList1.Focus()
                    Exit Sub
                End If
            Else
                myfield = "TITLE"
            End If


            'Boolean Operator validation
            Dim myBoolean As String = Nothing
            If DropDownList2.Text <> "" Then
                myBoolean = TrimAll(DropDownList2.SelectedValue)
                myBoolean = RemoveQuotes(myBoolean)
                If myBoolean.Length > 20 Then
                    Label6.Text = "Error: Input is not Valid !"
                    Label15.Text = ""
                    Me.DropDownList2.Focus()
                    Exit Sub
                End If
                myBoolean = " " & myBoolean & " "
                If InStr(1, myBoolean, "CREATE", 1) > 0 Or InStr(1, myBoolean, "DELETE", 1) > 0 Or InStr(1, myBoolean, "DROP", 1) > 0 Or InStr(1, myBoolean, "INSERT", 1) > 1 Or InStr(1, myBoolean, "TRACK", 1) > 1 Or InStr(1, myBoolean, "TRACE", 1) > 1 Then
                    Label6.Text = "Error: Input is not Valid !"
                    Label15.Text = ""
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
                    Label6.Text = "Error: Input is not Valid !"
                    Label15.Text = ""
                    DropDownList2.Focus()
                    Exit Sub
                End If
            Else
                myBoolean = "AND"
            End If

            'Order by validation
            Dim myOrderBy As String = Nothing
            If DropDownList3.Text <> "" Then
                myOrderBy = TrimAll(DropDownList3.SelectedValue)
                myOrderBy = RemoveQuotes(myOrderBy)
                If myOrderBy.Length > 20 Then
                    Label6.Text = "Error: Input is not Valid !"
                    Label15.Text = ""
                    Me.DropDownList3.Focus()
                    Exit Sub
                End If
                myOrderBy = " " & myOrderBy & " "
                If InStr(1, myOrderBy, "CREATE", 1) > 0 Or InStr(1, myOrderBy, "DELETE", 1) > 0 Or InStr(1, myOrderBy, "DROP", 1) > 0 Or InStr(1, myOrderBy, "INSERT", 1) > 1 Or InStr(1, myOrderBy, "TRACK", 1) > 1 Or InStr(1, myOrderBy, "TRACE", 1) > 1 Then
                    Label6.Text = "Error: Input is not Valid !"
                    Label15.Text = ""
                    Me.DropDownList3.Focus()
                    Exit Sub
                End If
                myOrderBy = TrimAll(myOrderBy)
                'check unwanted characters
                c = 0
                counter4 = 0
                For iloop = 1 To Len(myOrderBy)
                    strcurrentchar = Mid(myOrderBy, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter4 = 1
                        End If
                    End If
                Next
                If counter4 = 1 Then
                    Label6.Text = "Error: Input is not Valid !"
                    Label15.Text = ""
                    DropDownList3.Focus()
                    Exit Sub
                End If
            Else
                myOrderBy = "TITLE"
            End If

            'Sort by validation
            Dim mySortBy As String = Nothing
            If DropDownList4.Text <> "" Then
                mySortBy = TrimAll(DropDownList4.SelectedValue)
                mySortBy = RemoveQuotes(mySortBy)
                If mySortBy.Length > 5 Then
                    Label6.Text = "Error: Input is not Valid !"
                    Label15.Text = ""
                    Me.DropDownList4.Focus()
                    Exit Sub
                End If
                mySortBy = " " & mySortBy & " "
                If InStr(1, mySortBy, "CREATE", 1) > 0 Or InStr(1, mySortBy, "DELETE", 1) > 0 Or InStr(1, mySortBy, "DROP", 1) > 0 Or InStr(1, mySortBy, "INSERT", 1) > 1 Or InStr(1, mySortBy, "TRACK", 1) > 1 Or InStr(1, mySortBy, "TRACE", 1) > 1 Then
                    Label6.Text = "Error: Input is not Valid !" & Label15.Text = ""
                    Me.DropDownList4.Focus()
                    Exit Sub
                End If
                mySortBy = TrimAll(mySortBy)
                'check unwanted characters
                c = 0
                counter5 = 0
                For iloop = 1 To Len(mySortBy)
                    strcurrentchar = Mid(mySortBy, iloop, 1)
                    If c = 0 Then
                        If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                            c = c + 1
                            counter5 = 1
                        End If
                    End If
                Next
                If counter5 = 1 Then
                    Label6.Text = "Error: Input is not Valid !"
                    Label15.Text = ""
                    DropDownList4.Focus()
                    Exit Sub
                End If
            Else
                mySortBy = "ASC"
            End If


            '**********************************************************************************
            Dim SQL As String = Nothing
            SQL = "SELECT CAT_NO, TITLE FROM CATS_AUTHORS_VIEW WHERE (CAT_NO IS NOT NULL) AND (BIB_CODE <>'S') "

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

            SQL = SQL & " ORDER BY " & myOrderBy & " " & mySortBy & " "

            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            da.Fill(ds)

            dtSearch = ds.Tables(0).Copy
            Dim RecordCount As Long = 0
            If dtSearch.Rows.Count = 0 Then
                Me.Grid1_Search.DataSource = Nothing
                'Grid1_LibTeam.Columns.Clear()
                Grid1_Search.DataBind()
                Label1.Text = "Total Record(s): 0 "
                Delete_Bttn.Enabled = False
                Delete_Photo_Bttn.Enabled = False
            Else
                Grid1_Search.Visible = True
                RecordCount = dtSearch.Rows.Count
                Grid1_Search.DataSource = dtSearch
                Grid1_Search.DataBind()
                Label1.Text = "Total Record(s): " & RecordCount
                Delete_Bttn.Enabled = True
                Delete_Photo_Bttn.Enabled = True
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
    Protected Sub Grid1_Search_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles Grid1_Search.PageIndexChanging
        Try
            'rebind datagrid
            Grid1_Search.DataSource = ViewState("dt") 'temp
            Grid1_Search.PageIndex = e.NewPageIndex
            index = e.NewPageIndex * Grid1_Search.PageSize
            Grid1_Search.DataBind()
        Catch s As Exception
            Label6.Text = "Error:  there is error in page index !"
            Label15.Text = ""
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
    Protected Sub Grid1_Search_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles Grid1_Search.Sorting
        Dim temp As DataTable = CType(ViewState("dt"), DataTable)
        GridViewSortExpression = e.SortExpression
        Grid1_Search.DataSource = temp
        Dim pageIndex As Integer = Grid1_Search.PageIndex
        Grid1_Search.DataSource = SortDataTable(Grid1_Search.DataSource, False)
        Grid1_Search.DataBind()
        Grid1_Search.PageIndex = pageIndex
    End Sub
    Protected Sub Grid1_Search_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Grid1_Search.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Lblsr As Label = e.Row.FindControl("lblsr")
            Srno = index + 1
            If Lblsr Is Nothing = False Then
                Lblsr.Text = Srno
                index += 1
            End If
            e.Row.Attributes("onmouseover") = "this.style.cursor='hand';this.style.textDecoration='bold';this.style.background='#FFDFDF'"
            e.Row.Attributes("onmouseout") = "this.style.textDecoration='none';this.style.background='none'"
            'e.Row.Attributes("onclick") = ClientScript.GetPostBackClientHyperlink(Me, "Edit$" & Convert.ToString(e.Row.RowIndex))
        End If
    End Sub
    Private Shared Function t() As Object
        Throw New NotImplementedException
    End Function
    'get value of row from grid
    Private Sub Grid1_Search_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles Grid1_Search.RowCommand
        If e.CommandName = "Select" Then
            Dim myRowID, ID As Integer
            myRowID = e.CommandArgument.ToString()
            If Grid1_Search.Rows(myRowID).Cells(3).Text <> "" Then
                ID = Grid1_Search.Rows(myRowID).Cells(3).Text
                txt_Display_Value.Text = ID
                DDL_Display.SelectedValue = "CAT_NO"
                Me.DisplayRecord()
            Else
                Exit Sub
            End If
        Else
            Label6.Text = "Display Error: No Record Selected!"
            Label15.Text = ""
        End If
        DDL_Bib_Level.Focus()
    End Sub 'Grid1_ItemCommand
    'save record
    Public Sub Acq_Save_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Acq_Save_Bttn.Click

        If IsPostBack = True Then
            'check unwanted characters
            Dim iloop As Integer
            Dim strcurrentchar As Object
            Dim c As Integer
            Dim counter1, counter2, counter3, counter4, counter5, counter6, counter7, counter8, counter9, counter10, counter11, counter12, counter13, counter14, counter15, counter16, counter17, counter18, counter19 As Integer
            Dim counter20, counter21, counter22, counter23, counter24, counter25, counter26, counter27, counter28, counter29, counter30, counter31, counter32, counter33, counter34, counter35, counter36, counter37, counter38, counter39 As Integer
            Dim counter40, counter41, counter42, counter43, counter44, counter45, counter46, counter47, counter48, counter49, counter50, counter51, counter52, counter53, counter54 As Integer
            Dim counter55, counter56, counter57, counter58, counter59, counter60, counter61, counter62, counter63, counter64, counter65, counter66, counter67, counter68, counter69, counter70 As Integer
            Dim counter71, counter72, counter73, counter74, counter75 As Integer
            Dim thisTransaction As SqlClient.SqlTransaction = Nothing
            Try
                'validation for DDL_Bib_Level
                Dim myBibLevel As Object = Nothing
                myBibLevel = DDL_Bib_Level.SelectedValue
                If Not String.IsNullOrEmpty(myBibLevel) Then
                    myBibLevel = RemoveQuotes(myBibLevel)
                    If myBibLevel.Length <> 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_Bib_Level.Focus()
                        Exit Sub
                    End If
                    myBibLevel = " " & myBibLevel & " "
                    If InStr(1, myBibLevel, "CREATE", 1) > 0 Or InStr(1, myBibLevel, "DELETE", 1) > 0 Or InStr(1, myBibLevel, "DROP", 1) > 0 Or InStr(1, myBibLevel, "INSERT", 1) > 1 Or InStr(1, myBibLevel, "TRACK", 1) > 1 Or InStr(1, myBibLevel, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_Bib_Level.Focus()
                        Exit Sub
                    End If
                    myBibLevel = TrimX(myBibLevel)
                    'check unwanted characters
                    c = 0
                    counter1 = 0
                    For iloop = 1 To Len(myBibLevel)
                        strcurrentchar = Mid(myBibLevel, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter1 = 1
                            End If
                        End If
                    Next
                    If counter1 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_Bib_Level.Focus()
                        Exit Sub
                    End If
                Else
                    myBibLevel = "M"
                End If

                'validation for DDL_Mat_Type
                Dim myMatType As Object = Nothing
                myMatType = DDL_Mat_Type.SelectedValue
                If Not String.IsNullOrEmpty(myMatType) Then
                    myMatType = RemoveQuotes(myMatType)
                    If myMatType.Length <> 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_Mat_Type.Focus()
                        Exit Sub
                    End If
                    myMatType = " " & myMatType & " "
                    If InStr(1, myMatType, "CREATE", 1) > 0 Or InStr(1, myMatType, "DELETE", 1) > 0 Or InStr(1, myMatType, "DROP", 1) > 0 Or InStr(1, myMatType, "INSERT", 1) > 1 Or InStr(1, myMatType, "TRACK", 1) > 1 Or InStr(1, myMatType, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_Mat_Type.Focus()
                        Exit Sub
                    End If
                    myMatType = TrimX(myMatType)
                    'check unwanted characters
                    c = 0
                    counter2 = 0
                    For iloop = 1 To Len(myMatType)
                        strcurrentchar = Mid(myBibLevel, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter2 = 1
                            End If
                        End If
                    Next
                    If counter2 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_Mat_Type.Focus()
                        Exit Sub
                    End If
                Else
                    myMatType = "B"
                End If

                'validation for document Type
                Dim myDocType As Object = Nothing
                myDocType = DDL_Doc_Type.SelectedValue
                If Not String.IsNullOrEmpty(myDocType) Then
                    myDocType = RemoveQuotes(myDocType)
                    If myDocType.Length <> 2 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_Doc_Type.Focus()
                        Exit Sub
                    End If
                    myDocType = " " & myDocType & " "
                    If InStr(1, myDocType, "CREATE", 1) > 0 Or InStr(1, myDocType, "DELETE", 1) > 0 Or InStr(1, myDocType, "DROP", 1) > 0 Or InStr(1, myDocType, "INSERT", 1) > 1 Or InStr(1, myDocType, "TRACK", 1) > 1 Or InStr(1, myDocType, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_Doc_Type.Focus()
                        Exit Sub
                    End If
                    myDocType = TrimX(myDocType)
                    'check unwanted characters
                    c = 0
                    counter3 = 0
                    For iloop = 1 To Len(myDocType)
                        strcurrentchar = Mid(myDocType, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter3 = 1
                            End If
                        End If
                    Next
                    If counter3 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_Doc_Type.Focus()
                        Exit Sub
                    End If
                Else
                    myDocType = "BK"
                End If

                'validation for Lang DDL_Lang
                Dim myLangCode As Object = Nothing
                myLangCode = DDL_Lang.SelectedValue
                If Not String.IsNullOrEmpty(myLangCode) Then
                    myLangCode = RemoveQuotes(myLangCode)
                    If myLangCode.Length <> 3 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_Lang.Focus()
                        Exit Sub
                    End If
                    myLangCode = " " & myLangCode & " "
                    If InStr(1, myLangCode, "CREATE", 1) > 0 Or InStr(1, myLangCode, "DELETE", 1) > 0 Or InStr(1, myLangCode, "DROP", 1) > 0 Or InStr(1, myLangCode, "INSERT", 1) > 1 Or InStr(1, myLangCode, "TRACK", 1) > 1 Or InStr(1, myLangCode, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_Lang.Focus()
                        Exit Sub
                    End If
                    myLangCode = TrimX(myLangCode)
                    'check unwanted characters
                    c = 0
                    counter4 = 0
                    For iloop = 1 To Len(myLangCode)
                        strcurrentchar = Mid(myLangCode, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter4 = 1
                            End If
                        End If
                    Next
                    If counter4 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_Lang.Focus()
                        Exit Sub
                    End If
                Else
                    myLangCode = "ENG"
                End If

                'DDL_YesNo for Multi-Vol Y/N
                Dim myYesNo As Object = Nothing
                myYesNo = DDL_YesNo.SelectedValue
                If Not String.IsNullOrEmpty(myYesNo) Then
                    myYesNo = RemoveQuotes(myYesNo)
                    If myYesNo.Length <> 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_YesNo.Focus()
                        Exit Sub
                    End If
                    myYesNo = " " & myYesNo & " "
                    If InStr(1, myYesNo, "CREATE", 1) > 0 Or InStr(1, myYesNo, "DELETE", 1) > 0 Or InStr(1, myYesNo, "DROP", 1) > 0 Or InStr(1, myYesNo, "INSERT", 1) > 1 Or InStr(1, myYesNo, "TRACK", 1) > 1 Or InStr(1, myYesNo, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_YesNo.Focus()
                        Exit Sub
                    End If
                    myYesNo = TrimX(myYesNo)
                    'check unwanted characters
                    c = 0
                    counter5 = 0
                    For iloop = 1 To Len(myYesNo)
                        strcurrentchar = Mid(myYesNo, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter5 = 1
                            End If
                        End If
                    Next
                    If counter5 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_YesNo.Focus()
                        Exit Sub
                    End If
                Else
                    myYesNo = "N"
                End If


                'Server Validation for  total vol - numeric only
                Dim TotalVol As Integer = Nothing
                If myYesNo = "Y" Then
                    If txt_Cat_TotalVol.Text <> "" Then
                        TotalVol = TrimX(txt_Cat_TotalVol.Text)
                        TotalVol = RemoveQuotes(TotalVol)
                        If Len(TotalVol.ToString) > 5 Then 'maximum length
                            Label6.Text = " Data must be of Proper Length.. "
                            Label15.Text = ""
                            txt_Cat_TotalVol.Focus()
                            Exit Sub
                        End If

                        If Not IsNumeric(TotalVol) = True Then 'maximum length
                            Label6.Text = " Data must be Numeric Only.. "
                            Label15.Text = ""
                            txt_Cat_TotalVol.Focus()
                            Exit Sub
                        End If

                        TotalVol = " " & TotalVol & " "
                        If InStr(1, TotalVol, "CREATE", 1) > 0 Or InStr(1, TotalVol, "DELETE", 1) > 0 Or InStr(1, TotalVol, "DROP", 1) > 0 Or InStr(1, TotalVol, "INSERT", 1) > 1 Or InStr(1, TotalVol, "TRACK", 1) > 1 Or InStr(1, TotalVol, "TRACE", 1) > 1 Then
                            Label6.Text = " Do Not use Reserve Words... "
                            Label15.Text = ""
                            Me.txt_Cat_TotalVol.Focus()
                            Exit Sub
                        End If
                        TotalVol = TrimX(TotalVol)
                        'check unwanted characters
                        c = 0
                        counter6 = 0
                        For iloop = 1 To Len(TotalVol.ToString)
                            strcurrentchar = Mid(TotalVol, iloop, 1)
                            If c = 0 Then
                                If Not InStr("'~!@#$^*+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter6 = 1
                                End If
                            End If
                        Next
                        If counter6 = 1 Then
                            Label6.Text = " Do Not use un-wanted Characters... "
                            Label15.Text = ""
                            Me.txt_Cat_TotalVol.Focus()
                            Exit Sub
                        End If
                    Else
                        TotalVol = Nothing
                    End If
                End If

                'Server Validation for txt_Cat_ISBN
                Dim myISBN As Object = Nothing
                If txt_Cat_ISBN.Text <> "" Then
                    myISBN = TrimX(txt_Cat_ISBN.Text)
                    myISBN = RemoveQuotes(myISBN)
                    If myISBN.Length > 30 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_ISBN.Focus()
                        Exit Sub
                    End If

                    myISBN = " " & myISBN & " "
                    If InStr(1, myISBN, "CREATE", 1) > 0 Or InStr(1, myISBN, "DELETE", 1) > 0 Or InStr(1, myISBN, "DROP", 1) > 0 Or InStr(1, myISBN, "INSERT", 1) > 1 Or InStr(1, myISBN, "TRACK", 1) > 1 Or InStr(1, myISBN, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_ISBN.Focus()
                        Exit Sub
                    End If
                    myISBN = TrimX(myISBN)
                    'check unwanted characters
                    c = 0
                    counter7 = 0
                    For iloop = 1 To Len(myISBN.ToString)
                        strcurrentchar = Mid(myISBN, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^*+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter7 = 1
                            End If
                        End If
                    Next
                    If counter7 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_ISBN.Focus()
                        Exit Sub
                    End If

                    'check duplicate isbn
                    Dim str As Object = Nothing
                    Dim flag As Object = Nothing
                    str = "SELECT CAT_NO FROM CATS WHERE (replace(STANDARD_NO,'-','') = '" & Replace(myISBN.ToString, "-", "") & "' ) "
                    Dim cmd1 As New SqlCommand(str, SqlConn)
                    SqlConn.Open()
                    flag = cmd1.ExecuteScalar
                    If flag <> Nothing Then
                        Label6.Text = "This ISBN Already Exists ! "
                        Label15.Text = ""
                        Me.txt_Cat_ISBN.Focus()
                        Exit Sub
                    End If
                    SqlConn.Close()
                Else
                    myISBN = ""
                End If

                'Server validation for Standard Specification No : txt_Cat_SPNo
                Dim mySPNo As Object = Nothing
                If txt_Cat_SPNo.Text <> "" Then
                    mySPNo = TrimX(txt_Cat_SPNo.Text)
                    mySPNo = RemoveQuotes(mySPNo)
                    If mySPNo.Length > 100 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_SPNo.Focus()
                        Exit Sub
                    End If

                    mySPNo = " " & mySPNo & " "
                    If InStr(1, mySPNo, "CREATE", 1) > 0 Or InStr(1, mySPNo, "DELETE", 1) > 0 Or InStr(1, mySPNo, "DROP", 1) > 0 Or InStr(1, mySPNo, "INSERT", 1) > 1 Or InStr(1, mySPNo, "TRACK", 1) > 1 Or InStr(1, mySPNo, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_SPNo.Focus()
                        Exit Sub
                    End If
                    mySPNo = TrimX(mySPNo)
                    'check unwanted characters
                    c = 0
                    counter8 = 0
                    For iloop = 1 To Len(mySPNo.ToString)
                        strcurrentchar = Mid(mySPNo, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^*+|?<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter8 = 1
                            End If
                        End If
                    Next
                    If counter8 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_SPNo.Focus()
                        Exit Sub
                    End If
                Else
                    mySPNo = ""
                End If

                'Server validation for Standard Specification No : txt_Cat_ManualNo
                Dim myManualNo As Object = Nothing
                If txt_Cat_ManualNo.Text <> "" Then
                    myManualNo = TrimX(txt_Cat_ManualNo.Text)
                    myManualNo = RemoveQuotes(myManualNo)
                    If myManualNo.Length > 100 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_ManualNo.Focus()
                        Exit Sub
                    End If

                    myManualNo = " " & myManualNo & " "
                    If InStr(1, myManualNo, "CREATE", 1) > 0 Or InStr(1, myManualNo, "DELETE", 1) > 0 Or InStr(1, myManualNo, "DROP", 1) > 0 Or InStr(1, myManualNo, "INSERT", 1) > 1 Or InStr(1, myManualNo, "TRACK", 1) > 1 Or InStr(1, myManualNo, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_ManualNo.Focus()
                        Exit Sub
                    End If
                    myManualNo = TrimX(myManualNo)
                    'check unwanted characters
                    c = 0
                    counter9 = 0
                    For iloop = 1 To Len(myManualNo.ToString)
                        strcurrentchar = Mid(myManualNo, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^*+|?<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter9 = 1
                            End If
                        End If
                    Next
                    If counter9 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_ManualNo.Focus()
                        Exit Sub
                    End If
                Else
                    myManualNo = ""
                End If

                'Server validation for  : txt_Cat_ReportNo
                Dim myReportNo As Object = Nothing
                If txt_Cat_ReportNo.Text <> "" Then
                    myReportNo = TrimX(txt_Cat_ReportNo.Text)
                    myReportNo = RemoveQuotes(myReportNo)
                    If myReportNo.Length > 100 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_ReportNo.Focus()
                        Exit Sub
                    End If

                    myReportNo = " " & myReportNo & " "
                    If InStr(1, myReportNo, "CREATE", 1) > 0 Or InStr(1, myReportNo, "DELETE", 1) > 0 Or InStr(1, myReportNo, "DROP", 1) > 0 Or InStr(1, myReportNo, "INSERT", 1) > 1 Or InStr(1, myReportNo, "TRACK", 1) > 1 Or InStr(1, myReportNo, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_ReportNo.Focus()
                        Exit Sub
                    End If
                    myReportNo = TrimX(myReportNo)
                    'check unwanted characters
                    c = 0
                    counter10 = 0
                    For iloop = 1 To Len(myReportNo.ToString)
                        strcurrentchar = Mid(myReportNo, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^*+|?<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter10 = 1
                            End If
                        End If
                    Next
                    If counter10 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_ReportNo.Focus()
                        Exit Sub
                    End If
                Else
                    myReportNo = ""
                End If

                'Server validation for  : txt_Cat_Title
                Dim myTitle As Object = Nothing
                If txt_Cat_Title.Text <> "" Then
                    myTitle = TrimAll(txt_Cat_Title.Text)
                    myTitle = RemoveQuotes(myTitle)
                    If myTitle.Length > 400 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Title.Focus()
                        Exit Sub
                    End If

                    myTitle = " " & myTitle & " "
                    If InStr(1, myTitle, "CREATE", 1) > 0 Or InStr(1, myTitle, "DELETE", 1) > 0 Or InStr(1, myTitle, "DROP", 1) > 0 Or InStr(1, myTitle, "INSERT", 1) > 1 Or InStr(1, myTitle, "TRACK", 1) > 1 Or InStr(1, myTitle, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_Title.Focus()
                        Exit Sub
                    End If
                    myTitle = TrimAll(myTitle)
                    'check unwanted characters
                    c = 0
                    counter11 = 0
                    For iloop = 1 To Len(myTitle.ToString)
                        strcurrentchar = Mid(myTitle, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!+|""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter11 = 1
                            End If
                        End If
                    Next
                    If counter11 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_Title.Focus()
                        Exit Sub
                    End If
                Else
                    Label6.Text = " Plz Enter Data... "
                    Label15.Text = ""
                    Me.txt_Cat_Title.Focus()
                    Exit Sub
                End If

                'Server validation for  : txt_Cat_SubTitle
                Dim mySubTitle As Object = Nothing
                If txt_Cat_SubTitle.Text <> "" Then
                    mySubTitle = TrimAll(txt_Cat_SubTitle.Text)
                    mySubTitle = RemoveQuotes(mySubTitle)
                    If mySubTitle.Length > 400 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_SubTitle.Focus()
                        Exit Sub
                    End If

                    mySubTitle = " " & mySubTitle & " "
                    If InStr(1, mySubTitle, "CREATE", 1) > 0 Or InStr(1, mySubTitle, "DELETE", 1) > 0 Or InStr(1, mySubTitle, "DROP", 1) > 0 Or InStr(1, mySubTitle, "INSERT", 1) > 1 Or InStr(1, mySubTitle, "TRACK", 1) > 1 Or InStr(1, mySubTitle, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_SubTitle.Focus()
                        Exit Sub
                    End If
                    mySubTitle = TrimAll(mySubTitle)
                    'check unwanted characters
                    c = 0
                    counter12 = 0
                    For iloop = 1 To Len(mySubTitle.ToString)
                        strcurrentchar = Mid(mySubTitle, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter12 = 1
                            End If
                        End If
                    Next
                    If counter12 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_SubTitle.Focus()
                        Exit Sub
                    End If
                Else
                    mySubTitle = ""
                End If

                'Server validation for  : txt_Cat_VarTitle
                Dim myVarTitle As Object = Nothing
                If txt_Cat_VarTitle.Text <> "" Then
                    myVarTitle = TrimAll(txt_Cat_VarTitle.Text)
                    myVarTitle = RemoveQuotes(myVarTitle)
                    If myVarTitle.Length > 400 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_VarTitle.Focus()
                        Exit Sub
                    End If

                    myVarTitle = " " & myVarTitle & " "
                    If InStr(1, myVarTitle, "CREATE", 1) > 0 Or InStr(1, myVarTitle, "DELETE", 1) > 0 Or InStr(1, myVarTitle, "DROP", 1) > 0 Or InStr(1, myVarTitle, "INSERT", 1) > 1 Or InStr(1, myVarTitle, "TRACK", 1) > 1 Or InStr(1, myVarTitle, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_VarTitle.Focus()
                        Exit Sub
                    End If
                    myVarTitle = TrimAll(myVarTitle)
                    'check unwanted characters
                    c = 0
                    counter13 = 0
                    For iloop = 1 To Len(myVarTitle.ToString)
                        strcurrentchar = Mid(myVarTitle, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter13 = 1
                            End If
                        End If
                    Next
                    If counter13 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_VarTitle.Focus()
                        Exit Sub
                    End If
                Else
                    myVarTitle = ""
                End If

                'Server validation for  : txt_Cat_ScholarName
                Dim myScholarName As Object = Nothing
                If txt_Cat_ScholarName.Text <> "" Then
                    myScholarName = TrimAll(txt_Cat_ScholarName.Text)
                    myScholarName = RemoveQuotes(myScholarName)
                    If myScholarName.Length > 250 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_ScholarName.Focus()
                        Exit Sub
                    End If

                    myScholarName = " " & myScholarName & " "
                    If InStr(1, myScholarName, "CREATE", 1) > 0 Or InStr(1, myScholarName, "DELETE", 1) > 0 Or InStr(1, myScholarName, "DROP", 1) > 0 Or InStr(1, myScholarName, "INSERT", 1) > 1 Or InStr(1, myScholarName, "TRACK", 1) > 1 Or InStr(1, myScholarName, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_ScholarName.Focus()
                        Exit Sub
                    End If
                    myScholarName = TrimAll(myScholarName)
                    'check unwanted characters
                    c = 0
                    counter14 = 0
                    For iloop = 1 To Len(myScholarName.ToString)
                        strcurrentchar = Mid(myScholarName, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter14 = 1
                            End If
                        End If
                    Next
                    If counter14 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_ScholarName.Focus()
                        Exit Sub
                    End If
                Else
                    myScholarName = ""
                End If

                'Server validation for  : txt_Cat_ScholarDept
                Dim myScholarDept As Object = Nothing
                If txt_Cat_ScholarDept.Text <> "" Then
                    myScholarDept = TrimAll(txt_Cat_ScholarDept.Text)
                    myScholarDept = RemoveQuotes(myScholarDept)
                    If myScholarDept.Length > 300 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_ScholarDept.Focus()
                        Exit Sub
                    End If

                    myScholarDept = " " & myScholarDept & " "
                    If InStr(1, myScholarDept, "CREATE", 1) > 0 Or InStr(1, myScholarDept, "DELETE", 1) > 0 Or InStr(1, myScholarDept, "DROP", 1) > 0 Or InStr(1, myScholarDept, "INSERT", 1) > 1 Or InStr(1, myScholarDept, "TRACK", 1) > 1 Or InStr(1, myScholarDept, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_ScholarDept.Focus()
                        Exit Sub
                    End If
                    myScholarDept = TrimAll(myScholarDept)
                    'check unwanted characters
                    c = 0
                    counter15 = 0
                    For iloop = 1 To Len(myScholarDept.ToString)
                        strcurrentchar = Mid(myScholarDept, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter15 = 1
                            End If
                        End If
                    Next
                    If counter15 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_ScholarDept.Focus()
                        Exit Sub
                    End If
                Else
                    myScholarDept = ""
                End If

                'Server validation for  : txt_Cat_GuideName
                Dim myGuideName As Object = Nothing
                If txt_Cat_GuideName.Text <> "" Then
                    myGuideName = TrimAll(txt_Cat_GuideName.Text)
                    myGuideName = RemoveQuotes(myGuideName)
                    If myGuideName.Length > 250 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_GuideName.Focus()
                        Exit Sub
                    End If

                    myGuideName = " " & myGuideName & " "
                    If InStr(1, myGuideName, "CREATE", 1) > 0 Or InStr(1, myGuideName, "DELETE", 1) > 0 Or InStr(1, myGuideName, "DROP", 1) > 0 Or InStr(1, myGuideName, "INSERT", 1) > 1 Or InStr(1, myGuideName, "TRACK", 1) > 1 Or InStr(1, myGuideName, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_GuideName.Focus()
                        Exit Sub
                    End If
                    myGuideName = TrimAll(myGuideName)
                    'check unwanted characters
                    c = 0
                    counter16 = 0
                    For iloop = 1 To Len(myGuideName.ToString)
                        strcurrentchar = Mid(myGuideName, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter16 = 1
                            End If
                        End If
                    Next
                    If counter16 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_GuideName.Focus()
                        Exit Sub
                    End If
                Else
                    myGuideName = ""
                End If

                'Server validation for  : txt_Cat_GuideDept
                Dim myGuideDept As Object = Nothing
                If txt_Cat_GuideDept.Text <> "" Then
                    myGuideDept = TrimAll(txt_Cat_GuideDept.Text)
                    myGuideDept = RemoveQuotes(myGuideDept)
                    If myGuideDept.Length > 300 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_GuideDept.Focus()
                        Exit Sub
                    End If

                    myGuideDept = " " & myGuideDept & " "
                    If InStr(1, myGuideDept, "CREATE", 1) > 0 Or InStr(1, myGuideDept, "DELETE", 1) > 0 Or InStr(1, myGuideDept, "DROP", 1) > 0 Or InStr(1, myGuideDept, "INSERT", 1) > 1 Or InStr(1, myGuideDept, "TRACK", 1) > 1 Or InStr(1, myGuideDept, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_GuideDept.Focus()
                        Exit Sub
                    End If
                    myGuideDept = TrimAll(myGuideDept)
                    'check unwanted characters
                    c = 0
                    counter17 = 0
                    For iloop = 1 To Len(myGuideDept.ToString)
                        strcurrentchar = Mid(myGuideDept, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter17 = 1
                            End If
                        End If
                    Next
                    If counter17 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_GuideDept.Focus()
                        Exit Sub
                    End If
                Else
                    myGuideDept = ""
                End If

                'Server validation for  : txt_Cat_DegreeName
                Dim myDegreeName As Object = Nothing
                If txt_Cat_DegreeName.Text <> "" Then
                    myDegreeName = TrimAll(txt_Cat_DegreeName.Text)
                    myDegreeName = RemoveQuotes(myDegreeName)
                    If myDegreeName.Length > 250 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_DegreeName.Focus()
                        Exit Sub
                    End If

                    myDegreeName = " " & myDegreeName & " "
                    If InStr(1, myDegreeName, "CREATE", 1) > 0 Or InStr(1, myDegreeName, "DELETE", 1) > 0 Or InStr(1, myDegreeName, "DROP", 1) > 0 Or InStr(1, myDegreeName, "INSERT", 1) > 1 Or InStr(1, myDegreeName, "TRACK", 1) > 1 Or InStr(1, myDegreeName, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_DegreeName.Focus()
                        Exit Sub
                    End If
                    myDegreeName = TrimAll(myDegreeName)
                    'check unwanted characters
                    c = 0
                    counter18 = 0
                    For iloop = 1 To Len(myDegreeName.ToString)
                        strcurrentchar = Mid(myDegreeName, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter18 = 1
                            End If
                        End If
                    Next
                    If counter18 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_DegreeName.Focus()
                        Exit Sub
                    End If
                Else
                    myDegreeName = ""
                End If

                'Server validation for  : txt_Cat_SPRevision
                Dim mySPRev As Object = Nothing
                If txt_Cat_SPRevision.Text <> "" Then
                    mySPRev = TrimAll(txt_Cat_SPRevision.Text)
                    mySPRev = RemoveQuotes(mySPRev)
                    If mySPRev.Length > 100 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_SPRevision.Focus()
                        Exit Sub
                    End If

                    mySPRev = " " & mySPRev & " "
                    If InStr(1, mySPRev, "CREATE", 1) > 0 Or InStr(1, mySPRev, "DELETE", 1) > 0 Or InStr(1, mySPRev, "DROP", 1) > 0 Or InStr(1, mySPRev, "INSERT", 1) > 1 Or InStr(1, mySPRev, "TRACK", 1) > 1 Or InStr(1, mySPRev, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_SPRevision.Focus()
                        Exit Sub
                    End If
                    mySPRev = TrimAll(mySPRev)
                    'check unwanted characters
                    c = 0
                    counter19 = 0
                    For iloop = 1 To Len(mySPRev.ToString)
                        strcurrentchar = Mid(mySPRev, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter19 = 1
                            End If
                        End If
                    Next
                    If counter19 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_SPRevision.Focus()
                        Exit Sub
                    End If
                Else
                    mySPRev = ""
                End If

                'Server validation for  : txt_Cat_ManualRevision
                Dim myManualVer As Object = Nothing
                If txt_Cat_ManualRevision.Text <> "" Then
                    myManualVer = TrimAll(txt_Cat_ManualRevision.Text)
                    myManualVer = RemoveQuotes(myManualVer)
                    If myManualVer.Length > 50 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_ManualRevision.Focus()
                        Exit Sub
                    End If

                    myManualVer = " " & myManualVer & " "
                    If InStr(1, myManualVer, "CREATE", 1) > 0 Or InStr(1, myManualVer, "DELETE", 1) > 0 Or InStr(1, myManualVer, "DROP", 1) > 0 Or InStr(1, myManualVer, "INSERT", 1) > 1 Or InStr(1, myManualVer, "TRACK", 1) > 1 Or InStr(1, myManualVer, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_ManualRevision.Focus()
                        Exit Sub
                    End If
                    myManualVer = TrimAll(myManualVer)
                    'check unwanted characters
                    c = 0
                    counter20 = 0
                    For iloop = 1 To Len(myManualVer.ToString)
                        strcurrentchar = Mid(myManualVer, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter20 = 1
                            End If
                        End If
                    Next
                    If counter20 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_ManualRevision.Focus()
                        Exit Sub
                    End If
                Else
                    myManualVer = ""
                End If

                'Server validation for  : txt_Cat_PatentNo
                Dim myPatentNo As Object = Nothing
                If txt_Cat_PatentNo.Text <> "" Then
                    myPatentNo = TrimAll(txt_Cat_PatentNo.Text)
                    myPatentNo = RemoveQuotes(myPatentNo)
                    If myPatentNo.Length > 100 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_PatentNo.Focus()
                        Exit Sub
                    End If

                    myPatentNo = " " & myPatentNo & " "
                    If InStr(1, myPatentNo, "CREATE", 1) > 0 Or InStr(1, myPatentNo, "DELETE", 1) > 0 Or InStr(1, myPatentNo, "DROP", 1) > 0 Or InStr(1, myPatentNo, "INSERT", 1) > 1 Or InStr(1, myPatentNo, "TRACK", 1) > 1 Or InStr(1, myPatentNo, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_PatentNo.Focus()
                        Exit Sub
                    End If
                    myPatentNo = TrimAll(myPatentNo)
                    'check unwanted characters
                    c = 0
                    counter21 = 0
                    For iloop = 1 To Len(myPatentNo.ToString)
                        strcurrentchar = Mid(myPatentNo, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter21 = 1
                            End If
                        End If
                    Next
                    If counter21 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_PatentNo.Focus()
                        Exit Sub
                    End If
                Else
                    myPatentNo = ""
                End If


                'Server validation for  : txt_Cat_Patentee
                Dim myPatentee As Object = Nothing
                If txt_Cat_Patentee.Text <> "" Then
                    myPatentee = TrimAll(txt_Cat_Patentee.Text)
                    myPatentee = RemoveQuotes(myPatentee)
                    If myPatentee.Length > 150 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Patentee.Focus()
                        Exit Sub
                    End If

                    myPatentee = " " & myPatentee & " "
                    If InStr(1, myPatentee, "CREATE", 1) > 0 Or InStr(1, myPatentee, "DELETE", 1) > 0 Or InStr(1, myPatentee, "DROP", 1) > 0 Or InStr(1, myPatentee, "INSERT", 1) > 1 Or InStr(1, myPatentee, "TRACK", 1) > 1 Or InStr(1, myPatentee, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_Patentee.Focus()
                        Exit Sub
                    End If
                    myPatentee = TrimAll(myPatentee)
                    'check unwanted characters
                    c = 0
                    counter22 = 0
                    For iloop = 1 To Len(myPatentee.ToString)
                        strcurrentchar = Mid(myPatentee, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter22 = 1
                            End If
                        End If
                    Next
                    If counter22 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_Patentee.Focus()
                        Exit Sub
                    End If
                Else
                    myPatentee = ""
                End If


                'Server validation for  : txt_Cat_PatentInventor
                Dim myPatentInventor As Object = Nothing
                If txt_Cat_PatentInventor.Text <> "" Then
                    myPatentInventor = TrimAll(txt_Cat_PatentInventor.Text)
                    myPatentInventor = RemoveQuotes(myPatentInventor)
                    If myPatentInventor.Length > 250 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_PatentInventor.Focus()
                        Exit Sub
                    End If

                    myPatentInventor = " " & myPatentInventor & " "
                    If InStr(1, myPatentInventor, "CREATE", 1) > 0 Or InStr(1, myPatentInventor, "DELETE", 1) > 0 Or InStr(1, myPatentInventor, "DROP", 1) > 0 Or InStr(1, myPatentInventor, "INSERT", 1) > 1 Or InStr(1, myPatentInventor, "TRACK", 1) > 1 Or InStr(1, myPatentInventor, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_PatentInventor.Focus()
                        Exit Sub
                    End If
                    myPatentInventor = TrimAll(myPatentInventor)
                    'check unwanted characters
                    c = 0
                    counter23 = 0
                    For iloop = 1 To Len(myPatentInventor.ToString)
                        strcurrentchar = Mid(myPatentInventor, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter23 = 1
                            End If
                        End If
                    Next
                    If counter23 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_PatentInventor.Focus()
                        Exit Sub
                    End If
                Else
                    myPatentInventor = ""
                End If


                'Server validation for  : txt_Cat_ConfName
                Dim myConfName As Object = Nothing
                If txt_Cat_ConfName.Text <> "" Then
                    myConfName = TrimAll(txt_Cat_ConfName.Text)
                    myConfName = RemoveQuotes(myConfName)
                    If myConfName.Length > 500 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_ConfName.Focus()
                        Exit Sub
                    End If

                    myConfName = " " & myConfName & " "
                    If InStr(1, myConfName, "CREATE", 1) > 0 Or InStr(1, myConfName, "DELETE", 1) > 0 Or InStr(1, myConfName, "DROP", 1) > 0 Or InStr(1, myConfName, "INSERT", 1) > 1 Or InStr(1, myConfName, "TRACK", 1) > 1 Or InStr(1, myConfName, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_ConfName.Focus()
                        Exit Sub
                    End If
                    myConfName = TrimAll(myConfName)
                    'check unwanted characters
                    c = 0
                    counter24 = 0
                    For iloop = 1 To Len(myConfName.ToString)
                        strcurrentchar = Mid(myConfName, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter24 = 1
                            End If
                        End If
                    Next
                    If counter24 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_ConfName.Focus()
                        Exit Sub
                    End If
                Else
                    myConfName = ""
                End If


                'Server validation for  : txt_Cat_ConfName
                Dim myConfSDate As Object = Nothing
                If txt_Cat_SDate.Text <> "" Then
                    myConfSDate = TrimX(txt_Cat_SDate.Text)
                    myConfSDate = RemoveQuotes(myConfSDate)
                    myConfSDate = Convert.ToDateTime(myConfSDate, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us
                    If myConfSDate.Length > 12 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_SDate.Focus()
                        Exit Sub
                    End If

                    myConfSDate = " " & myConfSDate & " "
                    If InStr(1, myConfSDate, "CREATE", 1) > 0 Or InStr(1, myConfSDate, "DELETE", 1) > 0 Or InStr(1, myConfSDate, "DROP", 1) > 0 Or InStr(1, myConfSDate, "INSERT", 1) > 1 Or InStr(1, myConfSDate, "TRACK", 1) > 1 Or InStr(1, myConfSDate, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_SDate.Focus()
                        Exit Sub
                    End If
                    myConfSDate = TrimX(myConfSDate)
                    'check unwanted characters
                    c = 0
                    counter25 = 0
                    For iloop = 1 To Len(myConfSDate.ToString)
                        strcurrentchar = Mid(myConfSDate, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter25 = 1
                            End If
                        End If
                    Next
                    If counter25 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_SDate.Focus()
                        Exit Sub
                    End If
                Else
                    myConfSDate = ""
                End If

                'Server validation for  : txt_Cat_EDate
                Dim myConfEDate As Object = Nothing
                If txt_Cat_EDate.Text <> "" Then
                    myConfEDate = TrimX(txt_Cat_EDate.Text)
                    myConfEDate = RemoveQuotes(myConfEDate)
                    myConfEDate = Convert.ToDateTime(myConfEDate, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us
                    If myConfEDate.Length > 12 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_EDate.Focus()
                        Exit Sub
                    End If

                    myConfEDate = " " & myConfEDate & " "
                    If InStr(1, myConfEDate, "CREATE", 1) > 0 Or InStr(1, myConfEDate, "DELETE", 1) > 0 Or InStr(1, myConfEDate, "DROP", 1) > 0 Or InStr(1, myConfEDate, "INSERT", 1) > 1 Or InStr(1, myConfEDate, "TRACK", 1) > 1 Or InStr(1, myConfEDate, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_EDate.Focus()
                        Exit Sub
                    End If
                    myConfEDate = TrimX(myConfEDate)
                    'check unwanted characters
                    c = 0
                    counter26 = 0
                    For iloop = 1 To Len(myConfEDate.ToString)
                        strcurrentchar = Mid(myConfEDate, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter26 = 1
                            End If
                        End If
                    Next
                    If counter26 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_EDate.Focus()
                        Exit Sub
                    End If
                Else
                    myConfEDate = ""
                End If

                'Server validation for  : txt_Cat_ConfPlace
                Dim myConfPlace As Object = Nothing
                If txt_Cat_ConfPlace.Text <> "" Then
                    myConfPlace = TrimAll(txt_Cat_ConfPlace.Text)
                    myConfPlace = RemoveQuotes(myConfPlace)
                    If myConfPlace.Length > 100 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_ConfName.Focus()
                        Exit Sub
                    End If

                    myConfPlace = " " & myConfPlace & " "
                    If InStr(1, myConfPlace, "CREATE", 1) > 0 Or InStr(1, myConfPlace, "DELETE", 1) > 0 Or InStr(1, myConfPlace, "DROP", 1) > 0 Or InStr(1, myConfPlace, "INSERT", 1) > 1 Or InStr(1, myConfPlace, "TRACK", 1) > 1 Or InStr(1, myConfPlace, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_ConfPlace.Focus()
                        Exit Sub
                    End If
                    myConfPlace = TrimAll(myConfPlace)
                    'check unwanted characters
                    c = 0
                    counter27 = 0
                    For iloop = 1 To Len(myConfPlace.ToString)
                        strcurrentchar = Mid(myConfPlace, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter27 = 1
                            End If
                        End If
                    Next
                    If counter27 = 1 Then
                        Label6.Text = "Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_ConfPlace.Focus()
                        Exit Sub
                    End If
                Else
                    myConfPlace = ""
                End If

                'Server validation for  : txt_Cat_Author1
                Dim myAuthor1 As Object = Nothing
                If txt_Cat_Author1.Text <> "" Then
                    myAuthor1 = TrimAll(txt_Cat_Author1.Text)
                    myAuthor1 = RemoveQuotes(myAuthor1)
                    If myAuthor1.Length > 250 Then 'maximum length
                        Label6.Text = "Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Author1.Focus()
                        Exit Sub
                    End If

                    myAuthor1 = " " & myAuthor1 & " "
                    If InStr(1, myAuthor1, "CREATE", 1) > 0 Or InStr(1, myAuthor1, "DELETE", 1) > 0 Or InStr(1, myAuthor1, "DROP", 1) > 0 Or InStr(1, myAuthor1, "INSERT", 1) > 1 Or InStr(1, myAuthor1, "TRACK", 1) > 1 Or InStr(1, myAuthor1, "TRACE", 1) > 1 Then
                        Label6.Text = "Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_Author1.Focus()
                        Exit Sub
                    End If
                    myAuthor1 = TrimAll(myAuthor1)
                    'check unwanted characters
                    c = 0
                    counter28 = 0
                    For iloop = 1 To Len(myAuthor1.ToString)
                        strcurrentchar = Mid(myAuthor1, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter28 = 1
                            End If
                        End If
                    Next
                    myAuthor1 = TrimAll(Replace(myAuthor1, ".", " "))
                    myAuthor1 = TrimAll(Replace(myAuthor1, ",", ", "))
                    myAuthor1 = TrimAll(Replace(myAuthor1, ":", ", "))

                    If counter28 = 1 Then
                        Label6.Text = "Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_Author1.Focus()
                        Exit Sub
                    End If
                Else
                    myAuthor1 = ""
                End If

                'Server validation for  : txt_Cat_Author2
                Dim myAuthor2 As Object = Nothing
                If txt_Cat_Author2.Text <> "" Then
                    myAuthor2 = TrimAll(txt_Cat_Author2.Text)
                    myAuthor2 = RemoveQuotes(myAuthor2)
                    If myAuthor2.Length > 250 Then 'maximum length
                        Label6.Text = "Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Author2.Focus()
                        Exit Sub
                    End If

                    myAuthor2 = " " & myAuthor2 & " "
                    If InStr(1, myAuthor2, "CREATE", 1) > 0 Or InStr(1, myAuthor2, "DELETE", 1) > 0 Or InStr(1, myAuthor2, "DROP", 1) > 0 Or InStr(1, myAuthor2, "INSERT", 1) > 1 Or InStr(1, myAuthor2, "TRACK", 1) > 1 Or InStr(1, myAuthor2, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_Author2.Focus()
                        Exit Sub
                    End If
                    myAuthor2 = TrimAll(myAuthor2)
                    'check unwanted characters
                    c = 0
                    counter29 = 0
                    For iloop = 1 To Len(myAuthor2.ToString)
                        strcurrentchar = Mid(myAuthor2, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter29 = 1
                            End If
                        End If
                    Next
                    myAuthor2 = TrimAll(Replace(myAuthor2, ".", " "))
                    myAuthor2 = TrimAll(Replace(myAuthor2, ",", ", "))
                    myAuthor2 = TrimAll(Replace(myAuthor2, ":", ", "))
                    If counter29 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_Author2.Focus()
                        Exit Sub
                    End If
                Else
                    myAuthor2 = ""
                End If

                'Server validation for  : txt_Cat_Author2
                Dim myAuthor3 As Object = Nothing
                If txt_Cat_Author3.Text <> "" Then
                    myAuthor3 = TrimAll(txt_Cat_Author3.Text)
                    myAuthor3 = RemoveQuotes(myAuthor3)
                    If myAuthor3.Length > 250 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Author3.Focus()
                        Exit Sub
                    End If

                    myAuthor3 = " " & myAuthor3 & " "
                    If InStr(1, myAuthor3, "CREATE", 1) > 0 Or InStr(1, myAuthor3, "DELETE", 1) > 0 Or InStr(1, myAuthor3, "DROP", 1) > 0 Or InStr(1, myAuthor3, "INSERT", 1) > 1 Or InStr(1, myAuthor3, "TRACK", 1) > 1 Or InStr(1, myAuthor3, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_Author3.Focus()
                        Exit Sub
                    End If
                    myAuthor3 = TrimAll(myAuthor3)
                    'check unwanted characters
                    c = 0
                    counter30 = 0
                    For iloop = 1 To Len(myAuthor3.ToString)
                        strcurrentchar = Mid(myAuthor3, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter30 = 1
                            End If
                        End If
                    Next
                    myAuthor3 = TrimAll(Replace(myAuthor3, ".", " "))
                    myAuthor3 = TrimAll(Replace(myAuthor3, ",", ", "))
                    myAuthor3 = TrimAll(Replace(myAuthor3, ":", ", "))
                    If counter30 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_Author3.Focus()
                        Exit Sub
                    End If
                Else
                    myAuthor3 = ""
                End If

                'Server validation for  : txt_Cat_Editor
                Dim myEditor As Object = Nothing
                If txt_Cat_Editor.Text <> "" Then
                    myEditor = TrimAll(txt_Cat_Editor.Text)
                    myEditor = RemoveQuotes(myEditor)
                    If myEditor.Length > 500 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Editor.Focus()
                        Exit Sub
                    End If

                    myEditor = " " & myEditor & " "
                    If InStr(1, myEditor, "CREATE", 1) > 0 Or InStr(1, myEditor, "DELETE", 1) > 0 Or InStr(1, myEditor, "DROP", 1) > 0 Or InStr(1, myEditor, "INSERT", 1) > 1 Or InStr(1, myEditor, "TRACK", 1) > 1 Or InStr(1, myEditor, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_Editor.Focus()
                        Exit Sub
                    End If
                    myEditor = TrimAll(myEditor)
                    'check unwanted characters
                    c = 0
                    counter31 = 0
                    For iloop = 1 To Len(myEditor.ToString)
                        strcurrentchar = Mid(myEditor, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter31 = 1
                            End If
                        End If
                    Next
                    If counter31 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_Editor.Focus()
                        Exit Sub
                    End If
                Else
                    myEditor = ""
                End If

                'Server validation for  : txt_Cat_Editor
                Dim myTr As Object = Nothing
                If txt_Cat_Translator.Text <> "" Then
                    myTr = TrimAll(txt_Cat_Translator.Text)
                    myTr = RemoveQuotes(myTr)
                    If myTr.Length > 500 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Translator.Focus()
                        Exit Sub
                    End If

                    myTr = " " & myTr & " "
                    If InStr(1, myTr, "CREATE", 1) > 0 Or InStr(1, myTr, "DELETE", 1) > 0 Or InStr(1, myTr, "DROP", 1) > 0 Or InStr(1, myTr, "INSERT", 1) > 1 Or InStr(1, myTr, "TRACK", 1) > 1 Or InStr(1, myTr, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_Translator.Focus()
                        Exit Sub
                    End If
                    myTr = TrimAll(myTr)
                    'check unwanted characters
                    c = 0
                    counter32 = 0
                    For iloop = 1 To Len(myTr.ToString)
                        strcurrentchar = Mid(myTr, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter32 = 1
                            End If
                        End If
                    Next
                    If counter32 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_Translator.Focus()
                        Exit Sub
                    End If
                Else
                    myTr = ""
                End If

                'Server validation for  : txt_Cat_Illustrator
                Dim myIllus As Object = Nothing
                If txt_Cat_Illustrator.Text <> "" Then
                    myIllus = TrimAll(txt_Cat_Illustrator.Text)
                    myIllus = RemoveQuotes(myIllus)
                    If myIllus.Length > 500 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Illustrator.Focus()
                        Exit Sub
                    End If

                    myIllus = " " & myIllus & " "
                    If InStr(1, myIllus, "CREATE", 1) > 0 Or InStr(1, myIllus, "DELETE", 1) > 0 Or InStr(1, myIllus, "DROP", 1) > 0 Or InStr(1, myIllus, "INSERT", 1) > 1 Or InStr(1, myIllus, "TRACK", 1) > 1 Or InStr(1, myIllus, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_Illustrator.Focus()
                        Exit Sub
                    End If
                    myIllus = TrimAll(myIllus)
                    'check unwanted characters
                    c = 0
                    counter33 = 0
                    For iloop = 1 To Len(myIllus.ToString)
                        strcurrentchar = Mid(myIllus, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter33 = 1
                            End If
                        End If
                    Next
                    If counter33 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_Illustrator.Focus()
                        Exit Sub
                    End If
                Else
                    myIllus = ""
                End If

                'Server validation for  : txt_Cat_Compiler
                Dim myCompiler As Object = Nothing
                If txt_Cat_Compiler.Text <> "" Then
                    myCompiler = TrimAll(txt_Cat_Compiler.Text)
                    myCompiler = RemoveQuotes(myCompiler)
                    If myCompiler.Length > 500 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Compiler.Focus()
                        Exit Sub
                    End If

                    myCompiler = " " & myCompiler & " "
                    If InStr(1, myCompiler, "CREATE", 1) > 0 Or InStr(1, myCompiler, "DELETE", 1) > 0 Or InStr(1, myCompiler, "DROP", 1) > 0 Or InStr(1, myCompiler, "INSERT", 1) > 1 Or InStr(1, myCompiler, "TRACK", 1) > 1 Or InStr(1, myCompiler, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_Compiler.Focus()
                        Exit Sub
                    End If
                    myCompiler = TrimAll(myCompiler)
                    'check unwanted characters
                    c = 0
                    counter34 = 0
                    For iloop = 1 To Len(myCompiler.ToString)
                        strcurrentchar = Mid(myCompiler, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter34 = 1
                            End If
                        End If
                    Next
                    If counter34 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_Compiler.Focus()
                        Exit Sub
                    End If
                Else
                    myCompiler = ""
                End If

                'Server validation for  : txt_Cat_Commentator
                Dim myCommentator As Object = Nothing
                If txt_Cat_Commentator.Text <> "" Then
                    myCommentator = TrimAll(txt_Cat_Commentator.Text)
                    myCommentator = RemoveQuotes(myCommentator)
                    If myCommentator.Length > 500 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Commentator.Focus()
                        Exit Sub
                    End If

                    myCommentator = " " & myCommentator & " "
                    If InStr(1, myCommentator, "CREATE", 1) > 0 Or InStr(1, myCommentator, "DELETE", 1) > 0 Or InStr(1, myCommentator, "DROP", 1) > 0 Or InStr(1, myCommentator, "INSERT", 1) > 1 Or InStr(1, myCommentator, "TRACK", 1) > 1 Or InStr(1, myCommentator, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_Commentator.Focus()
                        Exit Sub
                    End If
                    myCommentator = TrimAll(myCommentator)
                    'check unwanted characters
                    c = 0
                    counter35 = 0
                    For iloop = 1 To Len(myCommentator.ToString)
                        strcurrentchar = Mid(myCommentator, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter35 = 1
                            End If
                        End If
                    Next
                    If counter35 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_Commentator.Focus()
                        Exit Sub
                    End If
                Else
                    myCommentator = ""
                End If

                'Server validation for  : txt_Cat_RevisedBy
                Dim myRevisedBy As Object = Nothing
                If txt_Cat_RevisedBy.Text <> "" Then
                    myRevisedBy = TrimAll(txt_Cat_RevisedBy.Text)
                    myRevisedBy = RemoveQuotes(myRevisedBy)
                    If myRevisedBy.Length > 255 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_RevisedBy.Focus()
                        Exit Sub
                    End If

                    myRevisedBy = " " & myRevisedBy & " "
                    If InStr(1, myRevisedBy, "CREATE", 1) > 0 Or InStr(1, myRevisedBy, "DELETE", 1) > 0 Or InStr(1, myRevisedBy, "DROP", 1) > 0 Or InStr(1, myRevisedBy, "INSERT", 1) > 1 Or InStr(1, myRevisedBy, "TRACK", 1) > 1 Or InStr(1, myRevisedBy, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_RevisedBy.Focus()
                        Exit Sub
                    End If
                    myRevisedBy = TrimAll(myRevisedBy)
                    'check unwanted characters
                    c = 0
                    counter36 = 0
                    For iloop = 1 To Len(myRevisedBy.ToString)
                        strcurrentchar = Mid(myRevisedBy, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter36 = 1
                            End If
                        End If
                    Next
                    If counter36 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_RevisedBy.Focus()
                        Exit Sub
                    End If
                Else
                    myRevisedBy = ""
                End If

                'Server validation for  : txt_Cat_CorpAuthor
                Dim myCorpAuthor As Object = Nothing
                If txt_Cat_CorpAuthor.Text <> "" Then
                    myCorpAuthor = TrimAll(txt_Cat_CorpAuthor.Text)
                    myCorpAuthor = RemoveQuotes(myCorpAuthor)
                    If myCorpAuthor.Length > 500 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_CorpAuthor.Focus()
                        Exit Sub
                    End If

                    myCorpAuthor = " " & myCorpAuthor & " "
                    If InStr(1, myCorpAuthor, "CREATE", 1) > 0 Or InStr(1, myCorpAuthor, "DELETE", 1) > 0 Or InStr(1, myCorpAuthor, "DROP", 1) > 0 Or InStr(1, myCorpAuthor, "INSERT", 1) > 1 Or InStr(1, myCorpAuthor, "TRACK", 1) > 1 Or InStr(1, myCorpAuthor, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_CorpAuthor.Focus()
                        Exit Sub
                    End If
                    myCorpAuthor = TrimAll(myCorpAuthor)
                    'check unwanted characters
                    c = 0
                    counter37 = 0
                    For iloop = 1 To Len(myCorpAuthor.ToString)
                        strcurrentchar = Mid(myCorpAuthor, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter37 = 1
                            End If
                        End If
                    Next
                    If counter37 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_CorpAuthor.Focus()
                        Exit Sub
                    End If
                Else
                    myCorpAuthor = ""
                End If

                'Server validation for  : txt_Cat_Edition
                Dim myEdition As Object = Nothing
                If txt_Cat_Edition.Text <> "" Then
                    myEdition = TrimAll(txt_Cat_Edition.Text)
                    myEdition = RemoveQuotes(myEdition)
                    myEdition = myEdition
                    If myEdition.Length > 500 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Edition.Focus()
                        Exit Sub
                    End If

                    myEdition = " " & myEdition & " "
                    If InStr(1, myEdition, "CREATE", 1) > 0 Or InStr(1, myEdition, "DELETE", 1) > 0 Or InStr(1, myEdition, "DROP", 1) > 0 Or InStr(1, myEdition, "INSERT", 1) > 1 Or InStr(1, myEdition, "TRACK", 1) > 1 Or InStr(1, myEdition, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_Edition.Focus()
                        Exit Sub
                    End If
                    myEdition = TrimAll(myEdition)
                    'check unwanted characters
                    c = 0
                    counter38 = 0
                    For iloop = 1 To Len(myEdition.ToString)
                        strcurrentchar = Mid(myEdition, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter38 = 1
                            End If
                        End If
                    Next
                    If counter38 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_Edition.Focus()
                        Exit Sub
                    End If
                    If InStr(myEdition, "edition") <> 0 Then
                        myEdition = LCase(TrimAll(Replace(myEdition, "edition", "")))
                    End If
                    If InStr(myEdition, "ed") <> 0 Then
                        myEdition = LCase(TrimAll(Replace(myEdition, "ed", "")))
                    End If
                    If InStr(myEdition, "ed.") <> 0 Then
                        myEdition = LCase(TrimAll(Replace(myEdition, "ed.", "")))
                    End If
                    If InStr(myEdition, "edition.") <> 0 Then
                        myEdition = LCase(TrimAll(Replace(myEdition, "edition.", "")))
                    End If
                Else
                    myEdition = ""
                End If


                'Server validation for  : txt_Cat_Reprint
                Dim myReprint As Object = Nothing
                If txt_Cat_Reprint.Text <> "" Then
                    myReprint = TrimAll(txt_Cat_Reprint.Text)
                    myReprint = RemoveQuotes(myReprint)
                    If myReprint.Length > 500 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Reprint.Focus()
                        Exit Sub
                    End If

                    myReprint = " " & myReprint & " "
                    If InStr(1, myReprint, "CREATE", 1) > 0 Or InStr(1, myReprint, "DELETE", 1) > 0 Or InStr(1, myReprint, "DROP", 1) > 0 Or InStr(1, myReprint, "INSERT", 1) > 1 Or InStr(1, myReprint, "TRACK", 1) > 1 Or InStr(1, myReprint, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_Reprint.Focus()
                        Exit Sub
                    End If
                    myReprint = TrimAll(myReprint)
                    'check unwanted characters
                    c = 0
                    counter39 = 0
                    For iloop = 1 To Len(myReprint.ToString)
                        strcurrentchar = Mid(myReprint, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter39 = 1
                            End If
                        End If
                    Next
                    If counter39 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_Reprint.Focus()
                        Exit Sub
                    End If
                Else
                    myReprint = ""
                End If

                'Server validation for  : txt_Cat_Place
                Dim myPubPlace As Object = Nothing
                If txt_Cat_Place.Text <> "" Then
                    myPubPlace = TrimAll(txt_Cat_Place.Text)
                    myPubPlace = RemoveQuotes(myPubPlace)
                    If myPubPlace.Length > 150 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Place.Focus()
                        Exit Sub
                    End If

                    myPubPlace = " " & myPubPlace & " "
                    If InStr(1, myPubPlace, "CREATE", 1) > 0 Or InStr(1, myPubPlace, "DELETE", 1) > 0 Or InStr(1, myPubPlace, "DROP", 1) > 0 Or InStr(1, myPubPlace, "INSERT", 1) > 1 Or InStr(1, myPubPlace, "TRACK", 1) > 1 Or InStr(1, myPubPlace, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_Place.Focus()
                        Exit Sub
                    End If
                    myPubPlace = TrimAll(myPubPlace)
                    'check unwanted characters
                    c = 0
                    counter41 = 0
                    For iloop = 1 To Len(myPubPlace.ToString)
                        strcurrentchar = Mid(myPubPlace, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter41 = 1
                            End If
                        End If
                    Next
                    If counter41 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_Place.Focus()
                        Exit Sub
                    End If
                Else
                    myPubPlace = ""
                End If

                'validation for Country Code
                Dim myConCode As Object = Nothing
                If DDL_Countries.Text <> "" Then
                    myConCode = DDL_Countries.SelectedValue
                    myConCode = RemoveQuotes(myConCode)
                    If myConCode.Length > 4 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        DDL_Countries.Focus()
                        Exit Sub
                    End If

                    myConCode = " " & myConCode & " "
                    If InStr(1, myConCode, "CREATE", 1) > 0 Or InStr(1, myConCode, "DELETE", 1) > 0 Or InStr(1, myConCode, "DROP", 1) > 0 Or InStr(1, myConCode, "INSERT", 1) > 1 Or InStr(1, myConCode, "TRACK", 1) > 1 Or InStr(1, myConCode, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        DDL_Countries.Focus()
                        Exit Sub
                    End If
                    myConCode = TrimX(myConCode)
                    'check unwanted characters
                    c = 0
                    counter42 = 0
                    For iloop = 1 To Len(myConCode)
                        strcurrentchar = Mid(myConCode, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter42 = 1
                            End If
                        End If
                    Next
                    If counter42 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                        DDL_Countries.Focus()
                        Exit Sub
                    End If
                Else
                    myConCode = ""
                End If

                'validation for Publisher
                Dim myPubID As Object = Nothing
                Dim PUB_NAME As Object = Nothing
                If Pub_ComboBox.Text <> "" Then
                    myPubID = Pub_ComboBox.SelectedValue
                    If Microsoft.VisualBasic.IsNumeric(myPubID) = False Then
                        PUB_NAME = Trim(Pub_ComboBox.Text)
                        If PUB_NAME <> "" Then
                            PUB_NAME = TrimAll(Replace(PUB_NAME, ".", " "))
                            PUB_NAME = TrimAll(Replace(PUB_NAME, ",", ", "))
                            PUB_NAME = TrimAll(Replace(PUB_NAME, ";", ", "))
                        End If

                        'Check Duplicate User Code
                        Dim str As Object = Nothing
                        Dim flag As Object = Nothing
                        str = "SELECT PUB_ID FROM PUBLISHERS WHERE (PUB_NAME ='" & Trim(PUB_NAME) & "') "
                        Dim cmd1 As New SqlCommand(str, SqlConn)
                        SqlConn.Open()
                        flag = cmd1.ExecuteScalar
                        SqlConn.Close()
                        If flag <> Nothing Then
                            myPubID = flag
                            Pub_ComboBox.Items.FindByText(PUB_NAME).Selected = True
                        Else
                            Dim PubForm As New Publishers
                            'save new pub in database
                            PubForm.PUB_SAVE(PUB_NAME, myPubPlace, myConCode, LibCode)
                            Me.PopulatePub()
                            Pub_ComboBox.Items.FindByText(PUB_NAME).Selected = True
                            myPubID = Pub_ComboBox.SelectedValue
                        End If
                    End If

                    If Not String.IsNullOrEmpty(myPubID) Then
                        myPubID = RemoveQuotes(myPubID)
                        If Not IsNumeric(myPubID) = True Then
                            Label6.Text = "Error: Input is not Valid !"
                            Label15.Text = ""
                            Pub_ComboBox.Focus()
                            Exit Sub
                        End If
                        myPubID = " " & myPubID & " "
                        If InStr(1, myPubID, "CREATE", 1) > 0 Or InStr(1, myPubID, "DELETE", 1) > 0 Or InStr(1, myPubID, "DROP", 1) > 0 Or InStr(1, myPubID, "INSERT", 1) > 1 Or InStr(1, myPubID, "TRACK", 1) > 1 Or InStr(1, myPubID, "TRACE", 1) > 1 Then
                            Label6.Text = "Error: Input is not Valid !"
                            Label15.Text = ""
                            Pub_ComboBox.Focus()
                            Exit Sub
                        End If
                        myPubID = TrimX(myPubID)
                        'check unwanted characters
                        c = 0
                        counter40 = 0
                        For iloop = 1 To Len(myPubID.ToString)
                            strcurrentchar = Mid(myPubID, iloop, 1)
                            If c = 0 Then
                                If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter40 = 1
                                End If
                            End If
                        Next
                        If counter40 = 1 Then
                            Label6.Text = "Error: Input is not Valid !"
                            Label15.Text = ""
                            Pub_ComboBox.Focus()
                            Exit Sub
                        End If
                    End If
                Else
                    'Label6.Text = "Error: Publisher is Required !"
                    'Label15.Text = ""
                    'Pub_ComboBox.Focus()
                    'Exit Sub
                    myPubID = 0
                End If

                'Server validation for  : txt_Cat_Place
                Dim myPubYear As Integer = Nothing
                If TR_Year.Visible = True Then
                    If txt_Cat_Year.Text <> "" Then
                        myPubYear = TrimX(txt_Cat_Year.Text)
                        myPubYear = RemoveQuotes(myPubYear)
                        If Not Len(myPubYear.ToString) = 4 Then 'maximum length
                            Label6.Text = " Data must be of Proper Length.. "
                            Label15.Text = ""
                            txt_Cat_Year.Focus()
                            Exit Sub
                        End If

                        myPubYear = " " & myPubYear & " "
                        If InStr(1, myPubYear, "CREATE", 1) > 0 Or InStr(1, myPubYear, "DELETE", 1) > 0 Or InStr(1, myPubYear, "DROP", 1) > 0 Or InStr(1, myPubYear, "INSERT", 1) > 1 Or InStr(1, myPubYear, "TRACK", 1) > 1 Or InStr(1, myPubYear, "TRACE", 1) > 1 Then
                            Label6.Text = " Do Not use Reserve Words... "
                            Label15.Text = ""
                            Me.txt_Cat_Year.Focus()
                            Exit Sub
                        End If
                        myPubYear = TrimX(myPubYear)
                        'check unwanted characters
                        c = 0
                        counter43 = 0
                        For iloop = 1 To Len(myPubYear.ToString)
                            strcurrentchar = Mid(myPubYear, iloop, 1)
                            If c = 0 Then
                                If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter43 = 1
                                End If
                            End If
                        Next
                        If counter43 = 1 Then
                            Label6.Text = " Do Not use un-wanted Characters... "
                            Label15.Text = ""
                            Me.txt_Cat_Year.Focus()
                            Exit Sub
                        End If
                    Else
                        myPubYear = 0
                    End If
                Else
                    myPubYear = 0
                End If

                'Server validation for  : txt_Cat_Place
                Dim mySeries As Object = Nothing
                If txt_Cat_Series.Text <> "" Then
                    mySeries = TrimAll(txt_Cat_Series.Text)
                    mySeries = RemoveQuotes(mySeries)
                    If mySeries.Length > 500 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Series.Focus()
                        Exit Sub
                    End If

                    mySeries = " " & mySeries & " "
                    If InStr(1, mySeries, "CREATE", 1) > 0 Or InStr(1, mySeries, "DELETE", 1) > 0 Or InStr(1, mySeries, "DROP", 1) > 0 Or InStr(1, mySeries, "INSERT", 1) > 1 Or InStr(1, mySeries, "TRACK", 1) > 1 Or InStr(1, mySeries, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_Series.Focus()
                        Exit Sub
                    End If
                    mySeries = TrimAll(mySeries)
                    'check unwanted characters
                    c = 0
                    counter44 = 0
                    For iloop = 1 To Len(mySeries.ToString)
                        strcurrentchar = Mid(mySeries, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter44 = 1
                            End If
                        End If
                    Next
                    If counter44 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_Series.Focus()
                        Exit Sub
                    End If
                Else
                    mySeries = ""
                End If

                'Server validation for  : txt_Cat_SeriesEditor
                Dim mySeriesEditor As Object = Nothing
                If txt_Cat_SeriesEditor.Text <> "" Then
                    mySeriesEditor = TrimAll(txt_Cat_SeriesEditor.Text)
                    mySeriesEditor = RemoveQuotes(mySeriesEditor)
                    If mySeriesEditor.Length > 500 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_SeriesEditor.Focus()
                        Exit Sub
                    End If

                    mySeriesEditor = " " & mySeriesEditor & " "
                    If InStr(1, mySeriesEditor, "CREATE", 1) > 0 Or InStr(1, mySeriesEditor, "DELETE", 1) > 0 Or InStr(1, mySeriesEditor, "DROP", 1) > 0 Or InStr(1, mySeriesEditor, "INSERT", 1) > 1 Or InStr(1, mySeriesEditor, "TRACK", 1) > 1 Or InStr(1, mySeriesEditor, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_SeriesEditor.Focus()
                        Exit Sub
                    End If
                    mySeriesEditor = TrimAll(mySeriesEditor)
                    'check unwanted characters
                    c = 0
                    counter45 = 0
                    For iloop = 1 To Len(mySeriesEditor.ToString)
                        strcurrentchar = Mid(mySeriesEditor, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter45 = 1
                            End If
                        End If
                    Next
                    If counter45 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_SeriesEditor.Focus()
                        Exit Sub
                    End If
                Else
                    mySeriesEditor = ""
                End If


                'Server validation for  : txt_Cat_Note
                Dim myNote As Object = Nothing
                If txt_Cat_Note.Text <> "" Then
                    myNote = TrimAll(txt_Cat_Note.Text)
                    myNote = RemoveQuotes(myNote)
                    If myNote.Length > 500 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Note.Focus()
                        Exit Sub
                    End If

                    myNote = " " & myNote & " "
                    If InStr(1, myNote, "CREATE", 1) > 0 Or InStr(1, myNote, "DELETE", 1) > 0 Or InStr(1, myNote, "DROP", 1) > 0 Or InStr(1, myNote, "INSERT", 1) > 1 Or InStr(1, myNote, "TRACK", 1) > 1 Or InStr(1, myNote, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_Note.Focus()
                        Exit Sub
                    End If
                    myNote = TrimAll(myNote)
                    'check unwanted characters
                    c = 0
                    counter46 = 0
                    For iloop = 1 To Len(myNote.ToString)
                        strcurrentchar = Mid(myNote, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter46 = 1
                            End If
                        End If
                    Next
                    If counter46 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_Note.Focus()
                        Exit Sub
                    End If
                Else
                    myNote = ""
                End If

                'Server validation for  : txt_Cat_Remarks
                Dim myRemarks As Object = Nothing
                If txt_Cat_Remarks.Text <> "" Then
                    myRemarks = TrimAll(txt_Cat_Remarks.Text)
                    myRemarks = RemoveQuotes(myRemarks)
                    If myRemarks.Length > 500 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Remarks.Focus()
                        Exit Sub
                    End If

                    myRemarks = " " & myRemarks & " "
                    If InStr(1, myRemarks, "CREATE", 1) > 0 Or InStr(1, myRemarks, "DELETE", 1) > 0 Or InStr(1, myRemarks, "DROP", 1) > 0 Or InStr(1, myRemarks, "INSERT", 1) > 1 Or InStr(1, myRemarks, "TRACK", 1) > 1 Or InStr(1, myRemarks, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_Remarks.Focus()
                        Exit Sub
                    End If
                    myRemarks = TrimAll(myRemarks)
                    'check unwanted characters
                    c = 0
                    counter47 = 0
                    For iloop = 1 To Len(myRemarks.ToString)
                        strcurrentchar = Mid(myRemarks, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter47 = 1
                            End If
                        End If
                    Next
                    If counter47 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_Remarks.Focus()
                        Exit Sub
                    End If
                Else
                    myRemarks = ""
                End If

                'Server validation for  : txt_Cat_URL
                Dim myURL As Object = Nothing
                If txt_Cat_URL.Text <> "" Then
                    myURL = TrimAll(txt_Cat_URL.Text)
                    myURL = RemoveQuotes(myURL)
                    If myURL.Length > 500 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_URL.Focus()
                        Exit Sub
                    End If

                    myURL = " " & myURL & " "
                    If InStr(1, myURL, "CREATE", 1) > 0 Or InStr(1, myURL, "DELETE", 1) > 0 Or InStr(1, myURL, "DROP", 1) > 0 Or InStr(1, myURL, "INSERT", 1) > 1 Or InStr(1, myURL, "TRACK", 1) > 1 Or InStr(1, myURL, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_URL.Focus()
                        Exit Sub
                    End If
                    myURL = TrimAll(myURL)
                    'check unwanted characters
                    c = 0
                    counter48 = 0
                    For iloop = 1 To Len(myURL.ToString)
                        strcurrentchar = Mid(myURL, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter48 = 1
                            End If
                        End If
                    Next
                    If InStr(myURL, "http://") <> 0 Then
                        myURL = "http://" & myURL
                    End If
                    If counter48 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_URL.Focus()
                        Exit Sub
                    End If
                Else
                    myURL = ""
                End If

                'Server validation for  : txt_Cat_Remarks
                Dim myComments As Object = Nothing
                If txt_Cat_Comments.Text <> "" Then
                    myComments = TrimAll(txt_Cat_Comments.Text)
                    myComments = RemoveQuotes(myComments)
                    If myComments.Length > 500 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_URL.Focus()
                        Exit Sub
                    End If
                    myComments = " " & myComments & " "
                    If InStr(1, myComments, "CREATE", 1) > 0 Or InStr(1, myComments, "DELETE", 1) > 0 Or InStr(1, myComments, "DROP", 1) > 0 Or InStr(1, myComments, "INSERT", 1) > 1 Or InStr(1, myComments, "TRACK", 1) > 1 Or InStr(1, myComments, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_URL.Focus()
                        Exit Sub
                    End If
                    myComments = TrimAll(myComments)
                    'check unwanted characters
                    c = 0
                    counter49 = 0
                    For iloop = 1 To Len(myComments.ToString)
                        strcurrentchar = Mid(myComments, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter49 = 1
                            End If
                        End If
                    Next
                    If counter49 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_Comments.Focus()
                        Exit Sub
                    End If
                Else
                    myComments = ""
                End If

                'validation for DDL_Subjects
                Dim mySubId As Integer = Nothing
                If DDL_Subjects.Text <> "" Then
                    mySubId = DDL_Subjects.SelectedValue
                    If Not String.IsNullOrEmpty(mySubId) Then
                        mySubId = RemoveQuotes(mySubId)
                        If Len(mySubId) > 10 Then 'maximum length
                            Label6.Text = " Data must be of Proper Length.. "
                            Label15.Text = ""
                            DDL_Subjects.Focus()
                            Exit Sub
                        End If

                        If Not IsNumeric(mySubId) = True Then 'maximum length
                            Label6.Text = " Data must be Numeric Only.. "
                            Label15.Text = ""
                            DDL_Subjects.Focus()
                            Exit Sub
                        End If

                        mySubId = " " & mySubId & " "
                        If InStr(1, mySubId, "CREATE", 1) > 0 Or InStr(1, mySubId, "DELETE", 1) > 0 Or InStr(1, mySubId, "DROP", 1) > 0 Or InStr(1, mySubId, "INSERT", 1) > 1 Or InStr(1, mySubId, "TRACK", 1) > 1 Or InStr(1, mySubId, "TRACE", 1) > 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                            DDL_Subjects.Focus()
                            Exit Sub
                        End If
                        mySubId = TrimX(mySubId)
                        'check unwanted characters
                        c = 0
                        counter50 = 0
                        For iloop = 1 To Len(mySubId.ToString)
                            strcurrentchar = Mid(mySubId, iloop, 1)
                            If c = 0 Then
                                If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter50 = 1
                                End If
                            End If
                        Next
                        If counter50 = 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                            DDL_Subjects.Focus()
                            Exit Sub
                        End If
                    Else
                        mySubId = Nothing
                    End If
                Else
                    mySubId = Nothing
                End If

                'Server validation for  : txt_Cat_Keywords
                Dim myKeywords As Object = Nothing
                If txt_Cat_Keywords.Text <> "" Then
                    myKeywords = TrimAll(txt_Cat_Keywords.Text)
                    myKeywords = RemoveQuotes(myKeywords)
                    If myKeywords.Length > 1000 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Keywords.Focus()
                        Exit Sub
                    End If
                    myKeywords = " " & myKeywords & " "
                    If InStr(1, myKeywords, "CREATE", 1) > 0 Or InStr(1, myKeywords, "DELETE", 1) > 0 Or InStr(1, myKeywords, "DROP", 1) > 0 Or InStr(1, myKeywords, "INSERT", 1) > 1 Or InStr(1, myKeywords, "TRACK", 1) > 1 Or InStr(1, myKeywords, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_Keywords.Focus()
                        Exit Sub
                    End If
                    myKeywords = TrimAll(myKeywords)
                    'check unwanted characters
                    c = 0
                    counter51 = 0
                    For iloop = 1 To Len(myKeywords.ToString)
                        strcurrentchar = Mid(myKeywords, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter51 = 1
                            End If
                        End If
                    Next
                    If counter51 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_Keywords.Focus()
                        Exit Sub
                    End If
                    myKeywords = UCase(myKeywords)
                Else
                    myKeywords = ""
                End If

                'Server validation for  : txt_Cat_TrFrom
                Dim myTrFrom As Object = Nothing
                If txt_Cat_TrFrom.Text <> "" Then
                    myTrFrom = TrimAll(txt_Cat_TrFrom.Text)
                    myTrFrom = RemoveQuotes(myTrFrom)
                    If myTrFrom.Length > 500 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_TrFrom.Focus()
                        Exit Sub
                    End If
                    myTrFrom = " " & myTrFrom & " "
                    If InStr(1, myTrFrom, "CREATE", 1) > 0 Or InStr(1, myTrFrom, "DELETE", 1) > 0 Or InStr(1, myTrFrom, "DROP", 1) > 0 Or InStr(1, myTrFrom, "INSERT", 1) > 1 Or InStr(1, myTrFrom, "TRACK", 1) > 1 Or InStr(1, myTrFrom, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_TrFrom.Focus()
                        Exit Sub
                    End If
                    myTrFrom = TrimAll(myTrFrom)
                    'check unwanted characters
                    c = 0
                    counter52 = 0
                    For iloop = 1 To Len(myTrFrom.ToString)
                        strcurrentchar = Mid(myTrFrom, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter52 = 1
                            End If
                        End If
                    Next
                    If counter52 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_TrFrom.Focus()
                        Exit Sub
                    End If
                Else
                    myTrFrom = ""
                End If

                'Server validation for  : txt_Cat_Abstract
                Dim myAbstract As Object = Nothing
                If txt_Cat_Abstract.Text <> "" Then
                    myAbstract = TrimAll(txt_Cat_Abstract.Text)
                    myAbstract = RemoveQuotes(myAbstract)
                    If myAbstract.Length > 4000 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Abstract.Focus()
                        Exit Sub
                    End If
                    myAbstract = " " & myAbstract & " "
                    If InStr(1, myAbstract, "CREATE", 1) > 0 Or InStr(1, myAbstract, "DELETE", 1) > 0 Or InStr(1, myAbstract, "DROP", 1) > 0 Or InStr(1, myAbstract, "INSERT", 1) > 1 Or InStr(1, myAbstract, "TRACK", 1) > 1 Or InStr(1, myAbstract, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_Abstract.Focus()
                        Exit Sub
                    End If
                    myAbstract = TrimAll(myAbstract)
                    'check unwanted characters
                    c = 0
                    counter53 = 0
                    For iloop = 1 To Len(myAbstract.ToString)
                        strcurrentchar = Mid(myAbstract, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter53 = 1
                            End If
                        End If
                    Next
                    If counter53 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_Abstract.Focus()
                        Exit Sub
                    End If
                Else
                    myAbstract = ""
                End If

                'Server validation for  : txt_Cat_ReferenceNo
                Dim myRefNo As Object = Nothing
                If txt_Cat_ReferenceNo.Text <> "" Then
                    myRefNo = TrimAll(txt_Cat_ReferenceNo.Text)
                    myRefNo = RemoveQuotes(myRefNo)
                    If myRefNo.Length > 500 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_ReferenceNo.Focus()
                        Exit Sub
                    End If
                    myRefNo = " " & myRefNo & " "
                    If InStr(1, myRefNo, "CREATE", 1) > 0 Or InStr(1, myRefNo, "DELETE", 1) > 0 Or InStr(1, myRefNo, "DROP", 1) > 0 Or InStr(1, myRefNo, "INSERT", 1) > 1 Or InStr(1, myRefNo, "TRACK", 1) > 1 Or InStr(1, myRefNo, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_ReferenceNo.Focus()
                        Exit Sub
                    End If
                    myRefNo = TrimAll(myRefNo)
                    'check unwanted characters
                    c = 0
                    counter54 = 0
                    For iloop = 1 To Len(myRefNo.ToString)
                        strcurrentchar = Mid(myRefNo, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter54 = 1
                            End If
                        End If
                    Next
                    If counter54 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_ReferenceNo.Focus()
                        Exit Sub
                    End If
                Else
                    myRefNo = ""
                End If

                'Server validation for  : SP Re-Affirmation year
                Dim myReaffirmYear As Integer = Nothing
                If txt_Cat_ReaffirmYear.Text <> "" Then
                    myReaffirmYear = TrimX(txt_Cat_ReaffirmYear.Text)
                    myReaffirmYear = RemoveQuotes(myReaffirmYear)
                    If Not Len(myReaffirmYear.ToString) = 4 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_ReaffirmYear.Focus()
                        Exit Sub
                    End If

                    myReaffirmYear = " " & myReaffirmYear & " "
                    If InStr(1, myReaffirmYear, "CREATE", 1) > 0 Or InStr(1, myReaffirmYear, "DELETE", 1) > 0 Or InStr(1, myReaffirmYear, "DROP", 1) > 0 Or InStr(1, myReaffirmYear, "INSERT", 1) > 1 Or InStr(1, myReaffirmYear, "TRACK", 1) > 1 Or InStr(1, myReaffirmYear, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_ReaffirmYear.Focus()
                        Exit Sub
                    End If
                    myReaffirmYear = TrimX(myReaffirmYear)
                    'check unwanted characters
                    c = 0
                    counter55 = 0
                    For iloop = 1 To Len(myReaffirmYear.ToString)
                        strcurrentchar = Mid(myReaffirmYear, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter55 = 1
                            End If
                        End If
                    Next
                    If counter55 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_ReaffirmYear.Focus()
                        Exit Sub
                    End If
                Else
                    myReaffirmYear = 0
                End If

                'Server validation for  : SP Re-txt_Cat_WithdrawYear
                Dim myWithdrawYear As Integer = Nothing
                If txt_Cat_WithdrawYear.Text <> "" Then
                    myWithdrawYear = TrimX(txt_Cat_WithdrawYear.Text)
                    myWithdrawYear = RemoveQuotes(myWithdrawYear)
                    If Not Len(myWithdrawYear.ToString) = 4 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_WithdrawYear.Focus()
                        Exit Sub
                    End If

                    myWithdrawYear = " " & myWithdrawYear & " "
                    If InStr(1, myWithdrawYear, "CREATE", 1) > 0 Or InStr(1, myWithdrawYear, "DELETE", 1) > 0 Or InStr(1, myWithdrawYear, "DROP", 1) > 0 Or InStr(1, myWithdrawYear, "INSERT", 1) > 1 Or InStr(1, myWithdrawYear, "TRACK", 1) > 1 Or InStr(1, myWithdrawYear, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_WithdrawYear.Focus()
                        Exit Sub
                    End If
                    myWithdrawYear = TrimX(myWithdrawYear)
                    'check unwanted characters
                    c = 0
                    counter56 = 0
                    For iloop = 1 To Len(myWithdrawYear.ToString)
                        strcurrentchar = Mid(myWithdrawYear, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter56 = 1
                            End If
                        End If
                    Next
                    If counter56 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_WithdrawYear.Focus()
                        Exit Sub
                    End If
                Else
                    myWithdrawYear = 0
                End If


                'Server validation for  : SP Technical Committee
                Dim mySPCommittee As Object = Nothing
                If txt_Cat_SPTCSC.Text <> "" Then
                    mySPCommittee = TrimAll(txt_Cat_SPTCSC.Text)
                    mySPCommittee = RemoveQuotes(mySPCommittee)
                    If mySPCommittee.Length > 250 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_SPTCSC.Focus()
                        Exit Sub
                    End If
                    mySPCommittee = " " & mySPCommittee & " "
                    If InStr(1, mySPCommittee, "CREATE", 1) > 0 Or InStr(1, mySPCommittee, "DELETE", 1) > 0 Or InStr(1, mySPCommittee, "DROP", 1) > 0 Or InStr(1, mySPCommittee, "INSERT", 1) > 1 Or InStr(1, mySPCommittee, "TRACK", 1) > 1 Or InStr(1, mySPCommittee, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_SPTCSC.Focus()
                        Exit Sub
                    End If
                    mySPCommittee = TrimAll(mySPCommittee)
                    'check unwanted characters
                    c = 0
                    counter57 = 0
                    For iloop = 1 To Len(mySPCommittee.ToString)
                        strcurrentchar = Mid(mySPCommittee, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter57 = 1
                            End If
                        End If
                    Next
                    If counter57 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_SPTCSC.Focus()
                        Exit Sub
                    End If
                Else
                    mySPCommittee = ""
                End If

                'Server validation for  : txt_Cat_SPUpdates
                Dim mySPUpdates As Object = Nothing
                If txt_Cat_SPUpdates.Text <> "" Then
                    mySPUpdates = TrimAll(txt_Cat_SPUpdates.Text)
                    mySPUpdates = RemoveQuotes(mySPUpdates)
                    If mySPUpdates.Length > 250 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_SPUpdates.Focus()
                        Exit Sub
                    End If
                    mySPUpdates = " " & mySPUpdates & " "
                    If InStr(1, mySPUpdates, "CREATE", 1) > 0 Or InStr(1, mySPUpdates, "DELETE", 1) > 0 Or InStr(1, mySPUpdates, "DROP", 1) > 0 Or InStr(1, mySPUpdates, "INSERT", 1) > 1 Or InStr(1, mySPUpdates, "TRACK", 1) > 1 Or InStr(1, mySPUpdates, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_SPUpdates.Focus()
                        Exit Sub
                    End If
                    mySPUpdates = TrimAll(mySPUpdates)
                    'check unwanted characters
                    c = 0
                    counter58 = 0
                    For iloop = 1 To Len(mySPUpdates.ToString)
                        strcurrentchar = Mid(mySPUpdates, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter58 = 1
                            End If
                        End If
                    Next
                    If counter58 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_SPUpdates.Focus()
                        Exit Sub
                    End If
                Else
                    mySPUpdates = ""
                End If

                'Server validation for  : txt_Cat_SPAmmendments
                Dim mySPAmmendments As Object = Nothing
                If txt_Cat_SPAmmendments.Text <> "" Then
                    mySPAmmendments = TrimAll(txt_Cat_SPAmmendments.Text)
                    mySPAmmendments = RemoveQuotes(mySPAmmendments)
                    If mySPAmmendments.Length > 250 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_SPAmmendments.Focus()
                        Exit Sub
                    End If
                    mySPAmmendments = " " & mySPAmmendments & " "
                    If InStr(1, mySPAmmendments, "CREATE", 1) > 0 Or InStr(1, mySPAmmendments, "DELETE", 1) > 0 Or InStr(1, mySPAmmendments, "DROP", 1) > 0 Or InStr(1, mySPAmmendments, "INSERT", 1) > 1 Or InStr(1, mySPAmmendments, "TRACK", 1) > 1 Or InStr(1, mySPAmmendments, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_SPAmmendments.Focus()
                        Exit Sub
                    End If
                    mySPAmmendments = TrimAll(mySPAmmendments)
                    'check unwanted characters
                    c = 0
                    counter59 = 0
                    For iloop = 1 To Len(mySPAmmendments.ToString)
                        strcurrentchar = Mid(mySPAmmendments, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter59 = 1
                            End If
                        End If
                    Next
                    If counter59 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_SPAmmendments.Focus()
                        Exit Sub
                    End If
                Else
                    mySPAmmendments = ""
                End If

                'Server validation for  : txt_Cat_SPAmmendments
                Dim mySPIssueBody As Object = Nothing
                If txt_Cat_SPIssueBody.Text <> "" Then
                    mySPIssueBody = TrimAll(txt_Cat_SPIssueBody.Text)
                    mySPIssueBody = RemoveQuotes(mySPIssueBody)
                    If mySPAmmendments.Length > 350 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_SPIssueBody.Focus()
                        Exit Sub
                    End If
                    mySPIssueBody = " " & mySPIssueBody & " "
                    If InStr(1, mySPIssueBody, "CREATE", 1) > 0 Or InStr(1, mySPIssueBody, "DELETE", 1) > 0 Or InStr(1, mySPIssueBody, "DROP", 1) > 0 Or InStr(1, mySPIssueBody, "INSERT", 1) > 1 Or InStr(1, mySPIssueBody, "TRACK", 1) > 1 Or InStr(1, mySPIssueBody, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_SPIssueBody.Focus()
                        Exit Sub
                    End If
                    mySPIssueBody = TrimAll(mySPIssueBody)
                    'check unwanted characters
                    c = 0
                    counter60 = 0
                    For iloop = 1 To Len(mySPIssueBody.ToString)
                        strcurrentchar = Mid(mySPIssueBody, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter60 = 1
                            End If
                        End If
                    Next
                    If counter60 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_SPIssueBody.Focus()
                        Exit Sub
                    End If
                Else
                    mySPIssueBody = ""
                End If

                'Server validation for  : txt_Cat_Producer            
                Dim myProducer As Object = Nothing
                If txt_Cat_Producer.Text <> "" Then
                    myProducer = TrimAll(txt_Cat_Producer.Text)
                    myProducer = RemoveQuotes(myProducer)
                    If myProducer.Length > 250 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Producer.Focus()
                        Exit Sub
                    End If
                Else
                    myProducer = ""
                End If

                'Server validation for  : txt_Cat_Designer            
                Dim myDesigner As Object = Nothing
                If txt_Cat_Designer.Text <> "" Then
                    myDesigner = TrimAll(txt_Cat_Designer.Text)
                    myDesigner = RemoveQuotes(myDesigner)
                    If myDesigner.Length > 150 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Designer.Focus()
                        Exit Sub
                    End If
                Else
                    myDesigner = ""
                End If

                'Server validation for  : txt_Cat_Manufacturer						            
                Dim myManufacturer As Object = Nothing
                If txt_Cat_Manufacturer.Text <> "" Then
                    myManufacturer = TrimAll(txt_Cat_Manufacturer.Text)
                    myManufacturer = RemoveQuotes(myManufacturer)
                    If myManufacturer.Length > 250 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Manufacturer.Focus()
                        Exit Sub
                    End If
                Else
                    myManufacturer = ""
                End If

                'Server validation for  : txt_Cat_Creater									            
                Dim myCreator As Object = Nothing
                If txt_Cat_Creater.Text <> "" Then
                    myCreator = TrimAll(txt_Cat_Creater.Text)
                    myCreator = RemoveQuotes(myCreator)
                    If myCreator.Length > 150 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Creater.Focus()
                        Exit Sub
                    End If
                Else
                    myCreator = ""
                End If

                'Server validation for  : txt_Cat_Materials											            
                Dim myMaterials As Object = Nothing
                If txt_Cat_Materials.Text <> "" Then
                    myMaterials = TrimAll(txt_Cat_Materials.Text)
                    myMaterials = RemoveQuotes(myMaterials)
                    If myMaterials.Length > 50 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Materials.Focus()
                        Exit Sub
                    End If
                Else
                    myMaterials = ""
                End If


                'Server validation for  : txt_Cat_WrokCategory													            
                Dim myWorkCategory As Object = Nothing
                If txt_Cat_WrokCategory.Text <> "" Then
                    myWorkCategory = TrimAll(txt_Cat_WrokCategory.Text)
                    myWorkCategory = RemoveQuotes(myWorkCategory)
                    If myWorkCategory.Length > 50 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_WrokCategory.Focus()
                        Exit Sub
                    End If
                Else
                    myWorkCategory = ""
                End If

                'Server validation for  : txt_Cat_RelatedWork																			            
                Dim myRelatedWork As Object = Nothing
                If txt_Cat_RelatedWork.Text <> "" Then
                    myRelatedWork = TrimAll(txt_Cat_RelatedWork.Text)
                    myRelatedWork = RemoveQuotes(myRelatedWork)
                    If myRelatedWork.Length > 250 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_RelatedWork.Focus()
                        Exit Sub
                    End If
                Else
                    myRelatedWork = ""
                End If

                'ser rver validation for  : txt_Cat_Source																										            
                Dim mySource As Object = Nothing
                If txt_Cat_Source.Text <> "" Then
                    mySource = TrimAll(txt_Cat_Source.Text)
                    mySource = RemoveQuotes(mySource)
                    If mySource.Length > 250 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Source.Focus()
                        Exit Sub
                    End If
                Else
                    mySource = ""
                End If

                'ser rver validation for  : txt_Cat_Photographer																																            
                Dim myPhotographer As Object = Nothing
                If txt_Cat_Photographer.Text <> "" Then
                    myPhotographer = TrimAll(txt_Cat_Photographer.Text)
                    myPhotographer = RemoveQuotes(myPhotographer)
                    If myPhotographer.Length > 250 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Photographer.Focus()
                        Exit Sub
                    End If
                Else
                    myPhotographer = ""
                End If

                'ser rver validation for  : txt_Cat_Nationality																																	            
                Dim myNationality As Object = Nothing
                If txt_Cat_Nationality.Text <> "" Then
                    myNationality = TrimAll(txt_Cat_Nationality.Text)
                    myNationality = RemoveQuotes(myNationality)
                    If myNationality.Length > 50 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Nationality.Focus()
                        Exit Sub
                    End If
                Else
                    myNationality = ""
                End If

                'ser rver validation for  : txt_Cat_Techniq																																	            
                Dim myTechniq As Object = Nothing
                If txt_Cat_Techniq.Text <> "" Then
                    myTechniq = TrimAll(txt_Cat_Techniq.Text)
                    myTechniq = RemoveQuotes(myTechniq)
                    If myTechniq.Length > 150 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Techniq.Focus()
                        Exit Sub
                    End If
                Else
                    myTechniq = ""
                End If

                'ser rver validation for  : txt_Cat_WorkType																																			            
                Dim myWorkType As Object = Nothing
                If txt_Cat_WorkType.Text <> "" Then
                    myWorkType = TrimAll(txt_Cat_WorkType.Text)
                    myWorkType = RemoveQuotes(myWorkType)
                    If myWorkType.Length > 50 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_WorkType.Focus()
                        Exit Sub
                    End If
                Else
                    myWorkType = ""
                End If

                'ser rver validation for  : txt_Cat_RoleofCreator																																					            
                Dim myRoleofCreator As Object = Nothing
                If txt_Cat_RoleofCreator.Text <> "" Then
                    myRoleofCreator = TrimAll(txt_Cat_RoleofCreator.Text)
                    myRoleofCreator = RemoveQuotes(myRoleofCreator)
                    If myRoleofCreator.Length > 50 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_RoleofCreator.Focus()
                        Exit Sub
                    End If
                Else
                    myRoleofCreator = ""
                End If

                'Server validation for  : SP Re-txt_Cat_ProductionYear		
                Dim myProductionYear As Integer = Nothing
                If txt_Cat_ProductionYear.Text <> "" Then
                    myProductionYear = TrimX(txt_Cat_ProductionYear.Text)
                    myProductionYear = RemoveQuotes(myProductionYear)
                    If Not Len(myProductionYear.ToString) = 4 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_ProductionYear.Focus()
                        Exit Sub
                    End If

                    myProductionYear = " " & myProductionYear & " "
                    If InStr(1, myProductionYear, "CREATE", 1) > 0 Or InStr(1, myProductionYear, "DELETE", 1) > 0 Or InStr(1, myProductionYear, "DROP", 1) > 0 Or InStr(1, myProductionYear, "INSERT", 1) > 1 Or InStr(1, myProductionYear, "TRACK", 1) > 1 Or InStr(1, myProductionYear, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_ProductionYear.Focus()
                        Exit Sub
                    End If
                    myProductionYear = TrimX(myProductionYear)
                    'check unwanted characters
                    c = 0
                    counter56 = 0
                    For iloop = 1 To Len(myProductionYear.ToString)
                        strcurrentchar = Mid(myProductionYear, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter56 = 1
                            End If
                        End If
                    Next
                    If counter56 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_ProductionYear.Focus()
                        Exit Sub
                    End If
                Else
                    myProductionYear = 0
                End If


                'validation for CHAIRMAN
                Dim CHAIRMAN As Object = Nothing
                If txt_Cat_Chairman.Text <> "" Then
                    CHAIRMAN = TrimAll(txt_Cat_Chairman.Text)
                    CHAIRMAN = RemoveQuotes(CHAIRMAN)
                    If CHAIRMAN.Length > 250 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Cat_Chairman.Focus()
                        Exit Sub
                    End If
                    CHAIRMAN = " " & CHAIRMAN & " "
                    If InStr(1, CHAIRMAN, "CREATE", 1) > 0 Or InStr(1, CHAIRMAN, "DELETE", 1) > 0 Or InStr(1, CHAIRMAN, "DROP", 1) > 0 Or InStr(1, CHAIRMAN, "INSERT", 1) > 1 Or InStr(1, CHAIRMAN, "TRACK", 1) > 1 Or InStr(1, CHAIRMAN, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Cat_Chairman.Focus()
                        Exit Sub
                    End If
                    CHAIRMAN = TrimAll(CHAIRMAN)
                    'check unwanted characters
                    c = 0
                    counter71 = 0
                    For iloop = 1 To Len(CHAIRMAN)
                        strcurrentchar = Mid(CHAIRMAN, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter71 = 1
                            End If
                        End If
                    Next
                    If counter71 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Cat_Chairman.Focus()
                        Exit Sub
                    End If
                Else
                    CHAIRMAN = ""
                End If


                'validation for GOVERNMENT
                Dim GOVERNMENT As Object = Nothing
                If DDL_Government.Text <> "" Then
                    GOVERNMENT = DDL_Government.SelectedValue
                    GOVERNMENT = RemoveQuotes(GOVERNMENT)
                    If GOVERNMENT.Length > 50 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_Government.Focus()
                        Exit Sub
                    End If
                    GOVERNMENT = " " & GOVERNMENT & " "
                    If InStr(1, GOVERNMENT, "CREATE", 1) > 0 Or InStr(1, GOVERNMENT, "DELETE", 1) > 0 Or InStr(1, GOVERNMENT, "DROP", 1) > 0 Or InStr(1, GOVERNMENT, "INSERT", 1) > 1 Or InStr(1, GOVERNMENT, "TRACK", 1) > 1 Or InStr(1, GOVERNMENT, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_Government.Focus()
                        Exit Sub
                    End If
                    GOVERNMENT = TrimAll(GOVERNMENT)
                    'check unwanted characters
                    c = 0
                    counter72 = 0
                    For iloop = 1 To Len(GOVERNMENT)
                        strcurrentchar = Mid(GOVERNMENT, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter72 = 1
                            End If
                        End If
                    Next
                    If counter72 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_Government.Focus()
                        Exit Sub
                    End If
                Else
                    GOVERNMENT = ""
                End If


                'validation for ACT_NO
                Dim ACT_NO As Object = Nothing
                If txt_Cat_ActNo.Text <> "" Then
                    ACT_NO = TrimAll(txt_Cat_ActNo.Text)
                    ACT_NO = RemoveQuotes(ACT_NO)
                    If ACT_NO.Length > 150 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Cat_ActNo.Focus()
                        Exit Sub
                    End If
                    ACT_NO = " " & ACT_NO & " "
                    If InStr(1, ACT_NO, "CREATE", 1) > 0 Or InStr(1, ACT_NO, "DELETE", 1) > 0 Or InStr(1, ACT_NO, "DROP", 1) > 0 Or InStr(1, ACT_NO, "INSERT", 1) > 1 Or InStr(1, ACT_NO, "TRACK", 1) > 1 Or InStr(1, ACT_NO, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Cat_ActNo.Focus()
                        Exit Sub
                    End If
                    ACT_NO = TrimAll(ACT_NO)
                    'check unwanted characters
                    c = 0
                    counter73 = 0
                    For iloop = 1 To Len(ACT_NO)
                        strcurrentchar = Mid(ACT_NO, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter73 = 1
                            End If
                        End If
                    Next
                    If counter73 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Cat_ActNo.Focus()
                        Exit Sub
                    End If
                Else
                    ACT_NO = ""
                End If

                'Server validation for  : ACT_YEAR		
                Dim ACT_YEAR As Integer = Nothing
                If txt_Cat_ActYear.Text <> "" Then
                    ACT_YEAR = TrimX(txt_Cat_ActYear.Text)
                    ACT_YEAR = RemoveQuotes(ACT_YEAR)
                    If Not Len(ACT_YEAR.ToString) = 4 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_ActYear.Focus()
                        Exit Sub
                    End If

                    ACT_YEAR = " " & ACT_YEAR & " "
                    If InStr(1, ACT_YEAR, "CREATE", 1) > 0 Or InStr(1, ACT_YEAR, "DELETE", 1) > 0 Or InStr(1, ACT_YEAR, "DROP", 1) > 0 Or InStr(1, ACT_YEAR, "INSERT", 1) > 1 Or InStr(1, ACT_YEAR, "TRACK", 1) > 1 Or InStr(1, ACT_YEAR, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_ActYear.Focus()
                        Exit Sub
                    End If
                    ACT_YEAR = TrimX(ACT_YEAR)
                    'check unwanted characters
                    c = 0
                    counter56 = 0
                    For iloop = 1 To Len(ACT_YEAR.ToString)
                        strcurrentchar = Mid(ACT_YEAR, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter56 = 1
                            End If
                        End If
                    Next
                    If counter56 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_ActYear.Focus()
                        Exit Sub
                    End If
                Else
                    ACT_YEAR = 0
                End If



                'upload library photo
                Dim arrContent2 As Byte()
                Dim intLength2Photo As Integer = 0
                If FileUpload1.FileName = "" Then
                    '    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Select Proper file... ');", True)
                    '    Me.FileUpload12.Focus()
                    '    Exit Sub
                Else
                    Dim fileName As String = FileUpload1.PostedFile.FileName
                    Dim ext As String = fileName.Substring(fileName.LastIndexOf("."))
                    ext = ext.ToLower
                    Dim imgType = FileUpload1.PostedFile.ContentType

                    If ext = ".jpg" Then

                    ElseIf ext = ".bmp" Then

                    ElseIf ext = ".gif" Then

                    ElseIf ext = "jpg" Then

                    ElseIf ext = "bmp" Then

                    ElseIf ext = "gif" Then

                    Else
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Select Only gif, bmp, or jpg format files supported... ');", True)
                        Me.FileUpload1.Focus()
                        Exit Sub
                    End If
                    intLength2Photo = Convert.ToInt32(FileUpload1.PostedFile.InputStream.Length)
                    ReDim arrContent2(intLength2Photo)
                    FileUpload1.PostedFile.InputStream.Read(arrContent2, 0, intLength2Photo)

                    If intLength2Photo > 6000 Then
                        Label6.Text = "Error: Photo Size is Bigger than 6 KB"
                        Label15.Text = ""
                        Exit Sub
                    End If
                    Image1.ImageUrl = FileUpload1.PostedFile.FileName '"~/Images/1.png"
                End If




                'upload content file
                Dim arrContent3 As Byte()
                Dim FileType As String = Nothing
                Dim FileExtension As String = Nothing
                Dim intLength3 As Integer = 0
                If FileUpload2.FileName = "" Then
                    '    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Select Proper file... ');", True)
                    '    Me.FileUpload12.Focus()
                    '    Exit Sub
                Else
                    Dim ContfileName As String = FileUpload2.PostedFile.FileName
                    FileExtension = ContfileName.Substring(ContfileName.LastIndexOf("."))
                    FileExtension = FileExtension.ToLower
                    FileType = FileUpload2.PostedFile.ContentType

                    intLength3 = Convert.ToInt32(FileUpload2.PostedFile.InputStream.Length)
                    ReDim arrContent3(intLength3)
                    FileUpload2.PostedFile.InputStream.Read(arrContent3, 0, intLength3)

                    If intLength3 > 90000 Then
                        Label6.Text = "Error: Photo Size is Bigger than 90 KB"
                        Label15.Text = ""
                        Exit Sub
                    End If
                    'Image1.ImageUrl = FileUpload1.PostedFile.FileName '"~/Images/1.png"
                End If




                Dim myUserCode As Object = Nothing
                myUserCode = Session.Item("LoggedUser")

                Dim myDateAdded As Object = Nothing
                myDateAdded = Now.Date
                myDateAdded = Convert.ToDateTime(myDateAdded, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                Dim myCatLevel As Object = Nothing
                myCatLevel = "New"

                Dim myIP As Object = Nothing
                myIP = Request.UserHostAddress.Trim

                'check title for duplicacy
                Dim str3 As Object = Nothing
                Dim flag3 As Object = Nothing
                If myAuthor1 = String.Empty And myAuthor2 = String.Empty And myAuthor3 = String.Empty Then
                    str3 = "select CAT_NO from CATS_AUTHORS_VIEW where (TITLE= N'" & myTitle & "') "
                ElseIf myAuthor1 <> String.Empty And myAuthor2 = String.Empty And myAuthor3 = String.Empty Then
                    str3 = "select CAT_NO from CATS_AUTHORS_VIEW where (TITLE = N'" & myTitle & "' and AUTHOR1='" & myAuthor1 & "')"
                ElseIf myAuthor1 <> String.Empty And myAuthor2 <> String.Empty And myAuthor3 = String.Empty Then
                    str3 = "select CAT_NO from CATS_AUTHORS_VIEW where (TITLE = N'" & myTitle & "' and AUTHOR1='" & myAuthor1 & "' and AUTHOR2='" & myAuthor2 & "')"
                ElseIf myAuthor1 <> String.Empty And myAuthor2 <> String.Empty And myAuthor3 <> String.Empty Then
                    str3 = "select CAT_NO from CATS_AUTHORS_VIEW where (TITLE = N'" & myTitle & "' and AUTHOR1='" & myAuthor1 & "' and AUTHOR2='" & myAuthor2 & "' and AUTHOR3='" & myAuthor3 & "')"
                End If
                If myEdition <> String.Empty Then
                    str3 = str3 & " AND (EDITION= N'" & myEdition & "') "
                End If

                Dim PUBNAMEX As Object = Nothing
                If Pub_ComboBox.Text <> "" Then
                    PUBNAMEX = Pub_ComboBox.SelectedItem.ToString
                Else
                    PUBNAMEX = ""
                End If
                If PUBNAMEX <> String.Empty Then
                    str3 = str3 & " AND (PUB_NAME= N'" & PUBNAMEX & "') "
                End If
                If myPubYear <> 0 Then
                    str3 = str3 & " AND (YEAR_OF_PUB= '" & myPubYear & "') "
                End If


                Dim cmd3 As New SqlCommand(str3, SqlConn)
                SqlConn.Open()
                flag3 = cmd3.ExecuteScalar
                SqlConn.Close()
                If flag3 <> Nothing Then
                    Label6.Text = "This TITLE Already Exists ! "
                    Label15.Text = ""
                    Me.txt_Cat_ISBN.Focus()
                    Exit Sub
                End If

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
                objCommand.CommandText = "INSERT INTO CATS (LANG_CODE, BIB_CODE, MAT_CODE, DOC_TYPE_CODE, STANDARD_NO, TITLE, SUB_TITLE, VAR_TITLE, AUTHOR1, AUTHOR2, AUTHOR3, CORPORATE_AUTHOR, EDITOR, TRANSLATOR, ILLUSTRATOR, COMPILER, EDITION, PLACE_OF_PUB, PUB_ID, YEAR_OF_PUB, SERIES_TITLE, SERIES_EDITOR, NOTE, REMARKS, SUB_ID, KEYWORDS, DATE_ADDED, USER_CODE, ABSTRACT, CAT_LEVEL, MULTI_VOL, TOTAL_VOL, SP_NO, SP_VERSION, REPORT_NO, MANUAL_NO, MANUAL_VER, REFERENCE_NO, URL, PATENT_INVENTOR, PATENTEE, PATENT_NO, REPRINTS, CON_CODE, CONF_NAME, CONF_FROM, CONF_TO, CONF_PLACE, TR_FROM, REVISED_BY, COMMENTATORS, SCHOLAR_NAME, SCHOLAR_DEPT, GUIDE_NAME, GUIDE_DEPT, DEGREE_NAME, COMMENTS, LIB_CODE, IP, PHOTO, SP_REAFFIRM_YEAR, SP_TCSC, SP_UPDATES, SP_WITHDRAW_YEAR, SP_AMMENDMENTS, SP_ISSUE_BODY, PRODUCER, DESIGNER, MANUFACTURER, CREATOR, MATERIALS, WORK_CATEGORY, RELATED_WORK, SOURCE, PHOTOGRAPHER, NATIONALITY, TECHNIQ, WORK_TYPE, ROLE_OF_CREATOR, PRODUCTION_YEAR, CHAIRMAN, GOVERNMENT, ACT_NO, ACT_YEAR,CONT_FILE,CONT_FILE_TYPE) " & _
                                         " VALUES (@myLangCode,@myBibCode,@myMatCode,@myDocCode,@myISBN,@myTitle,@mySubTitle,@myVarTitle,@myAuthor1,@myAuthor2,@myAuthor3,@myCorpAuthor,@myEditor,@myTranslator,@myIllustrator,@myCompiler,@myEdition,@myPubPlace,@myPubID,@myPubYear,@mySeriesTitle,@mySeriesEditor,@myNote,@myRemarks,@mySubID,@myKeywords,@myDateAdded,@myUserCode,@myAbstract,@myCatLevel,@myMultiVol,@myTotalVol,@mySpNo,@mySpVersion,@myReportNo,@myManualNo,@myManualVer,@myReferenceNo,@myURL,@myPatentInventor,@myPatentee,@myPatentNo,@myReprints,@myConCode,@myConfName,@myConfFrom,@myConfTo,@myConfPlace,@myTrFrom,@myRevisedBy,@myCommentators,@myScholarName,@myScholarDept,@myGuideName,@myGuideDept,@myDegreeName,@myComments,@myLibCode,@myIP,@myPhoto,@myReaffirmYear,@mySPCommittee,@mySPUpdates,@myWithdrawYear,@mySPAmmendments,@mySPIssueBody,@myProducer,@myDesigner,@myManufacturer,@myCreator,@myMaterials,@myWorkCategory,@myRelatedWork,@mySource,@myPhotographer,@myNationality,@myTechniq,@myWorkType,@myRoleofCreator,@myProductionYear, @CHAIRMAN, @GOVERNMENT, @ACT_NO, @ACT_YEAR,@CONT_FILE,@CONT_FILE_TYPE); " & _
                                         " SELECT SCOPE_IDENTITY()"

                If myLangCode = "" Then myLangCode = "ENG"
                objCommand.Parameters.Add("@myLangCode", SqlDbType.VarChar)
                objCommand.Parameters("@myLangCode").Value = myLangCode

                If myBibLevel = "" Then myBibLevel = "M"
                objCommand.Parameters.Add("@myBibCode", SqlDbType.VarChar)
                objCommand.Parameters("@myBibCode").Value = myBibLevel

                If myMatType = "" Then myMatType = "B"
                objCommand.Parameters.Add("@myMatCode", SqlDbType.VarChar)
                objCommand.Parameters("@myMatCode").Value = myMatType

                If myDocType = "" Then myDocType = "BK"
                objCommand.Parameters.Add("@myDocCode", SqlDbType.VarChar)
                objCommand.Parameters("@myDocCode").Value = myDocType

                If myISBN = "" Then myISBN = System.DBNull.Value
                objCommand.Parameters.Add("@myISBN", SqlDbType.VarChar)
                objCommand.Parameters("@myISBN").Value = myISBN

                If myTitle = "" Then myTitle = System.DBNull.Value
                objCommand.Parameters.Add("@myTitle", SqlDbType.NVarChar)
                objCommand.Parameters("@myTitle").Value = myTitle

                If mySubTitle = "" Then mySubTitle = System.DBNull.Value
                objCommand.Parameters.Add("@mySubTitle", SqlDbType.NVarChar)
                objCommand.Parameters("@mySubTitle").Value = mySubTitle

                If myVarTitle = "" Then myVarTitle = System.DBNull.Value
                objCommand.Parameters.Add("@myVarTitle", SqlDbType.NVarChar)
                objCommand.Parameters("@myVarTitle").Value = myVarTitle

                If myAuthor1 = "" Then myAuthor1 = System.DBNull.Value
                objCommand.Parameters.Add("@myAuthor1", SqlDbType.NVarChar)
                objCommand.Parameters("@myAuthor1").Value = myAuthor1

                If myAuthor2 = "" Then myAuthor2 = System.DBNull.Value
                objCommand.Parameters.Add("@myAuthor2", SqlDbType.NVarChar)
                objCommand.Parameters("@myAuthor2").Value = myAuthor2

                If myAuthor3 = "" Then myAuthor3 = System.DBNull.Value
                objCommand.Parameters.Add("@myAuthor3", SqlDbType.NVarChar)
                objCommand.Parameters("@myAuthor3").Value = myAuthor3

                If myCorpAuthor = "" Then myCorpAuthor = System.DBNull.Value
                objCommand.Parameters.Add("@myCorpAuthor", SqlDbType.NVarChar)
                objCommand.Parameters("@myCorpAuthor").Value = myCorpAuthor

                If myEditor = "" Then myEditor = System.DBNull.Value
                objCommand.Parameters.Add("@myEditor", SqlDbType.NVarChar)
                objCommand.Parameters("@myEditor").Value = myEditor

                If myTr = "" Then myTr = System.DBNull.Value
                objCommand.Parameters.Add("@myTranslator", SqlDbType.NVarChar)
                objCommand.Parameters("@myTranslator").Value = myTr

                If myIllus = "" Then myIllus = System.DBNull.Value
                objCommand.Parameters.Add("@myIllustrator", SqlDbType.NVarChar)
                objCommand.Parameters("@myIllustrator").Value = myIllus

                If myCompiler = "" Then myCompiler = System.DBNull.Value
                objCommand.Parameters.Add("@myCompiler", SqlDbType.NVarChar)
                objCommand.Parameters("@myCompiler").Value = myCompiler

                If myEdition = "" Then myEdition = System.DBNull.Value
                objCommand.Parameters.Add("@myEdition", SqlDbType.NVarChar)
                objCommand.Parameters("@myEdition").Value = myEdition

                If myPubPlace = "" Then myPubPlace = System.DBNull.Value
                objCommand.Parameters.Add("@myPubPlace", SqlDbType.NVarChar)
                objCommand.Parameters("@myPubPlace").Value = myPubPlace

                objCommand.Parameters.Add("@myPubID", SqlDbType.Int)
                If myPubID = 0 Then
                    objCommand.Parameters("@myPubID").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@myPubID").Value = myPubID
                End If

                objCommand.Parameters.Add("@myPubYear", SqlDbType.Int)
                If myPubYear = 0 Then
                    objCommand.Parameters("@myPubYear").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@myPubYear").Value = myPubYear
                End If

                If mySeries = "" Then mySeries = System.DBNull.Value
                objCommand.Parameters.Add("@mySeriesTitle", SqlDbType.NVarChar)
                objCommand.Parameters("@mySeriesTitle").Value = mySeries

                If mySeriesEditor = "" Then mySeriesEditor = System.DBNull.Value
                objCommand.Parameters.Add("@myseriesEditor", SqlDbType.NVarChar)
                objCommand.Parameters("@mySeriesEditor").Value = mySeriesEditor

                If myNote = "" Then myNote = System.DBNull.Value
                objCommand.Parameters.Add("@myNote", SqlDbType.NVarChar)
                objCommand.Parameters("@myNote").Value = myNote

                If myRemarks = "" Then myRemarks = System.DBNull.Value
                objCommand.Parameters.Add("@myRemarks", SqlDbType.NVarChar)
                objCommand.Parameters("@myRemarks").Value = myRemarks

                objCommand.Parameters.Add("@mySubID", SqlDbType.Int)
                If mySubId = 0 Then
                    objCommand.Parameters("@mySubID").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@mySubID").Value = mySubId
                End If

                If myKeywords = "" Then myKeywords = System.DBNull.Value
                objCommand.Parameters.Add("@myKeywords", SqlDbType.NVarChar)
                objCommand.Parameters("@myKeywords").Value = myKeywords

                If myDateAdded = "" Then myDateAdded = System.DBNull.Value
                objCommand.Parameters.Add("@myDateAdded", SqlDbType.DateTime)
                objCommand.Parameters("@myDateAdded").Value = myDateAdded

                If myUserCode = "" Then myUserCode = System.DBNull.Value
                objCommand.Parameters.Add("@myUserCode", SqlDbType.NVarChar)
                objCommand.Parameters("@myUserCode").Value = myUserCode

                If myAbstract = "" Then myAbstract = System.DBNull.Value
                objCommand.Parameters.Add("@myAbstract", SqlDbType.NVarChar)
                objCommand.Parameters("@myAbstract").Value = myAbstract

                If myCatLevel = "" Then myCatLevel = System.DBNull.Value
                objCommand.Parameters.Add("@myCatLevel", SqlDbType.VarChar)
                objCommand.Parameters("@myCatLevel").Value = myCatLevel

                If myYesNo = "" Then myYesNo = System.DBNull.Value
                objCommand.Parameters.Add("@myMultiVol", SqlDbType.VarChar)
                objCommand.Parameters("@myMultiVol").Value = myYesNo

                objCommand.Parameters.Add("@myTotalVol", SqlDbType.Int)
                If TotalVol = 0 Then
                    objCommand.Parameters("@myTotalVol").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@myTotalVol").Value = TotalVol
                End If

                If mySPNo = "" Then mySPNo = System.DBNull.Value
                objCommand.Parameters.Add("@mySpNo", SqlDbType.NVarChar)
                objCommand.Parameters("@mySpNo").Value = mySPNo

                If mySPRev = "" Then mySPRev = System.DBNull.Value
                objCommand.Parameters.Add("@mySpVersion", SqlDbType.NVarChar)
                objCommand.Parameters("@mySpVersion").Value = mySPRev

                If myReportNo = "" Then myReportNo = System.DBNull.Value
                objCommand.Parameters.Add("@myReportNo", SqlDbType.NVarChar)
                objCommand.Parameters("@myReportNo").Value = myReportNo

                If myManualNo = "" Then myManualNo = System.DBNull.Value
                objCommand.Parameters.Add("@myManualNo", SqlDbType.NVarChar)
                objCommand.Parameters("@myManualNo").Value = myManualNo

                If myManualVer = "" Then myManualVer = System.DBNull.Value
                objCommand.Parameters.Add("@myManualVer", SqlDbType.NVarChar)
                objCommand.Parameters("@myManualVer").Value = myManualVer

                If myRefNo = "" Then myRefNo = System.DBNull.Value
                objCommand.Parameters.Add("@myReferenceNo", SqlDbType.NVarChar)
                objCommand.Parameters("@myReferenceNo").Value = myRefNo

                If myURL = "" Then myURL = System.DBNull.Value
                objCommand.Parameters.Add("@myURL", SqlDbType.VarChar)
                objCommand.Parameters("@myURL").Value = myURL

                If myPatentInventor = "" Then myPatentInventor = System.DBNull.Value
                objCommand.Parameters.Add("@myPatentInventor", SqlDbType.NVarChar)
                objCommand.Parameters("@myPatentInventor").Value = myPatentInventor

                If myPatentee = "" Then myPatentee = System.DBNull.Value
                objCommand.Parameters.Add("@myPatentee", SqlDbType.NVarChar)
                objCommand.Parameters("@myPatentee").Value = myPatentee

                If myPatentNo = "" Then myPatentNo = System.DBNull.Value
                objCommand.Parameters.Add("@myPatentNo", SqlDbType.NVarChar)
                objCommand.Parameters("@myPatentNo").Value = myPatentNo

                If myReprint = "" Then myReprint = System.DBNull.Value
                objCommand.Parameters.Add("@myReprints", SqlDbType.NVarChar)
                objCommand.Parameters("@myReprints").Value = myReprint

                If myConCode = "" Then myConCode = System.DBNull.Value
                objCommand.Parameters.Add("@myConCode", SqlDbType.VarChar)
                objCommand.Parameters("@myConCode").Value = myConCode

                If myConfName = "" Then myConfName = System.DBNull.Value
                objCommand.Parameters.Add("@myConfName", SqlDbType.NVarChar)
                objCommand.Parameters("@myConfName").Value = myConfName

                If myConfSDate = "" Then myConfSDate = System.DBNull.Value
                objCommand.Parameters.Add("@myConfFrom", SqlDbType.DateTime)
                objCommand.Parameters("@myConfFrom").Value = myConfSDate

                If myConfEDate = "" Then myConfEDate = System.DBNull.Value
                objCommand.Parameters.Add("@myConfTo", SqlDbType.DateTime)
                objCommand.Parameters("@myConfTo").Value = myConfEDate

                If myConfPlace = "" Then myConfPlace = System.DBNull.Value
                objCommand.Parameters.Add("@myConfPlace", SqlDbType.NVarChar)
                objCommand.Parameters("@myConfPlace").Value = myConfPlace

                If myTrFrom = "" Then myTrFrom = System.DBNull.Value
                objCommand.Parameters.Add("@myTrFrom", SqlDbType.NVarChar)
                objCommand.Parameters("@myTrFrom").Value = myTrFrom

                If myRevisedBy = "" Then myRevisedBy = System.DBNull.Value
                objCommand.Parameters.Add("@myRevisedBy", SqlDbType.NVarChar)
                objCommand.Parameters("@myRevisedBy").Value = myRevisedBy

                If myCommentator = "" Then myCommentator = System.DBNull.Value
                objCommand.Parameters.Add("@myCommentators", SqlDbType.NVarChar)
                objCommand.Parameters("@myCommentators").Value = myCommentator

                If myScholarName = "" Then myScholarName = System.DBNull.Value
                objCommand.Parameters.Add("@myScholarName", SqlDbType.NVarChar)
                objCommand.Parameters("@myScholarName").Value = myScholarName

                If myScholarDept = "" Then myScholarDept = System.DBNull.Value
                objCommand.Parameters.Add("@myScholarDept", SqlDbType.NVarChar)
                objCommand.Parameters("@myScholarDept").Value = myScholarDept

                If myGuideName = "" Then myGuideName = System.DBNull.Value
                objCommand.Parameters.Add("@myGuideName", SqlDbType.NVarChar)
                objCommand.Parameters("@myGuideName").Value = myGuideName

                If myGuideDept = "" Then myGuideDept = System.DBNull.Value
                objCommand.Parameters.Add("@myGuideDept", SqlDbType.NVarChar)
                objCommand.Parameters("@myGuideDept").Value = myGuideDept

                If myDegreeName = "" Then myDegreeName = System.DBNull.Value
                objCommand.Parameters.Add("@myDegreeName", SqlDbType.NVarChar)
                objCommand.Parameters("@myDegreeName").Value = myDegreeName

                If myComments = "" Then myComments = System.DBNull.Value
                objCommand.Parameters.Add("@myComments", SqlDbType.NVarChar)
                objCommand.Parameters("@myComments").Value = myComments

                If LibCode = "" Then LibCode = System.DBNull.Value
                objCommand.Parameters.Add("@myLibCode", SqlDbType.NVarChar)
                objCommand.Parameters("@myLibCode").Value = LibCode


                objCommand.Parameters.Add("@myReaffirmYear", SqlDbType.Int)
                If myReaffirmYear = 0 Then
                    objCommand.Parameters("@myReaffirmYear").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@myReaffirmYear").Value = myReaffirmYear
                End If

                objCommand.Parameters.Add("@myWithdrawYear", SqlDbType.Int)
                If myWithdrawYear = 0 Then
                    objCommand.Parameters("@myWithdrawYear").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@myWithdrawYear").Value = myWithdrawYear
                End If

                If mySPCommittee = "" Then mySPCommittee = System.DBNull.Value
                objCommand.Parameters.Add("@mySPCommittee", SqlDbType.NVarChar)
                objCommand.Parameters("@mySPCommittee").Value = mySPCommittee

                If mySPUpdates = "" Then mySPUpdates = System.DBNull.Value
                objCommand.Parameters.Add("@mySPUpdates", SqlDbType.NVarChar)
                objCommand.Parameters("@mySPUpdates").Value = mySPUpdates

                If mySPAmmendments = "" Then mySPAmmendments = System.DBNull.Value
                objCommand.Parameters.Add("@mySPAmmendments", SqlDbType.NVarChar)
                objCommand.Parameters("@mySPAmmendments").Value = mySPAmmendments

                If mySPIssueBody = "" Then mySPIssueBody = System.DBNull.Value
                objCommand.Parameters.Add("@mySPIssueBody", SqlDbType.NVarChar)
                objCommand.Parameters("@mySPIssueBody").Value = mySPIssueBody

                If myProducer = "" Then myProducer = System.DBNull.Value
                objCommand.Parameters.Add("@myProducer", SqlDbType.NVarChar)
                objCommand.Parameters("@myProducer").Value = myProducer

                If myDesigner = "" Then myDesigner = System.DBNull.Value
                objCommand.Parameters.Add("@myDesigner", SqlDbType.NVarChar)
                objCommand.Parameters("@myDesigner").Value = myDesigner

                If myManufacturer = "" Then myManufacturer = System.DBNull.Value
                objCommand.Parameters.Add("@myManufacturer", SqlDbType.NVarChar)
                objCommand.Parameters("@myManufacturer").Value = myManufacturer

                If myCreator = "" Then myCreator = System.DBNull.Value
                objCommand.Parameters.Add("@myCreator", SqlDbType.NVarChar)
                objCommand.Parameters("@myCreator").Value = myCreator

                If myMaterials = "" Then myMaterials = System.DBNull.Value
                objCommand.Parameters.Add("@myMaterials", SqlDbType.NVarChar)
                objCommand.Parameters("@myMaterials").Value = myMaterials

                If myWorkCategory = "" Then myWorkCategory = System.DBNull.Value
                objCommand.Parameters.Add("@myWorkCategory", SqlDbType.NVarChar)
                objCommand.Parameters("@myWorkCategory").Value = myWorkCategory

                If myRelatedWork = "" Then myRelatedWork = System.DBNull.Value
                objCommand.Parameters.Add("@myRelatedWork", SqlDbType.NVarChar)
                objCommand.Parameters("@myRelatedWork").Value = myRelatedWork

                If mySource = "" Then mySource = System.DBNull.Value
                objCommand.Parameters.Add("@mySource", SqlDbType.NVarChar)
                objCommand.Parameters("@mySource").Value = mySource

                If myPhotographer = "" Then myPhotographer = System.DBNull.Value
                objCommand.Parameters.Add("@myPhotographer", SqlDbType.NVarChar)
                objCommand.Parameters("@myPhotographer").Value = myPhotographer

                If myNationality = "" Then myNationality = System.DBNull.Value
                objCommand.Parameters.Add("@myNationality", SqlDbType.NVarChar)
                objCommand.Parameters("@myNationality").Value = myNationality

                If myTechniq = "" Then myTechniq = System.DBNull.Value
                objCommand.Parameters.Add("@myTechniq", SqlDbType.NVarChar)
                objCommand.Parameters("@myTechniq").Value = myTechniq

                If myWorkType = "" Then myWorkType = System.DBNull.Value
                objCommand.Parameters.Add("@myWorkType", SqlDbType.NVarChar)
                objCommand.Parameters("@myWorkType").Value = myWorkType

                If myRoleofCreator = "" Then myRoleofCreator = System.DBNull.Value
                objCommand.Parameters.Add("@myRoleofCreator", SqlDbType.NVarChar)
                objCommand.Parameters("@myRoleofCreator").Value = myRoleofCreator

                objCommand.Parameters.Add("@myProductionYear", SqlDbType.Int)
                If myProductionYear = 0 Then
                    objCommand.Parameters("@myProductionYear").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@myProductionYear").Value = myProductionYear
                End If

                If CHAIRMAN = "" Then CHAIRMAN = System.DBNull.Value
                objCommand.Parameters.Add("@CHAIRMAN", SqlDbType.NVarChar)
                objCommand.Parameters("@CHAIRMAN").Value = CHAIRMAN

                If GOVERNMENT = "" Then GOVERNMENT = System.DBNull.Value
                objCommand.Parameters.Add("@GOVERNMENT", SqlDbType.NVarChar)
                objCommand.Parameters("@GOVERNMENT").Value = GOVERNMENT

                If ACT_NO = "" Then ACT_NO = System.DBNull.Value
                objCommand.Parameters.Add("@ACT_NO", SqlDbType.NVarChar)
                objCommand.Parameters("@ACT_NO").Value = ACT_NO

                objCommand.Parameters.Add("@ACT_YEAR", SqlDbType.Int)
                If ACT_YEAR = 0 Then
                    objCommand.Parameters("@ACT_YEAR").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@ACT_YEAR").Value = ACT_YEAR
                End If


                If myIP = "" Then myIP = System.DBNull.Value
                objCommand.Parameters.Add("@myIP", SqlDbType.NVarChar)
                objCommand.Parameters("@myIP").Value = myIP

                objCommand.Parameters.Add("@myPhoto", SqlDbType.Image)
                If FileUpload1.FileName = "" Then
                    objCommand.Parameters("@myPhoto").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@myPhoto").Value = arrContent2
                End If


                'contents file uploading
                objCommand.Parameters.Add("@CONT_FILE", SqlDbType.Image)
                If FileUpload2.FileName = "" Then
                    objCommand.Parameters("@CONT_FILE").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@CONT_FILE").Value = arrContent3
                End If

                'contents file uploading extension
                objCommand.Parameters.Add("@CONT_FILE_TYPE", SqlDbType.VarChar)
                If FileUpload2.FileName = "" Then
                    objCommand.Parameters("@CONT_FILE_TYPE").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@CONT_FILE_TYPE").Value = FileExtension
                End If


                Dim dr As SqlDataReader
                dr = objCommand.ExecuteReader()
                If dr.Read Then
                    intValue = dr.GetValue(0)
                End If
                dr.Close()

                thisTransaction.Commit()
                SqlConn.Close()

                Label15.Text = "Record Added Successfully! " & "Cat No: " & intValue.ToString
                Label6.Text = ""
                Me.Acq_Save_Bttn.Visible = True
                Acq_Update_Bttn.Visible = False
                Acq_SaveNew_Bttn.Visible = False
                ClearFields()
                UpdatePanel1.Update()
            Catch q As SqlException
                thisTransaction.Rollback()
                Label6.Text = "Database Error -SAVE: " & (q.Message() & ", " & q.Number & ", " & q.GetType.ToString)
            Catch ex As Exception
                Label6.Text = "Error-SAVE: " & (ex.Message())
                Label15.Text = ""
            Finally
                SqlConn.Close()
            End Try
        End If

    End Sub
    Public Sub ClearFields()
        txt_Cat_TotalVol.Text = ""
        txt_Cat_ISBN.Text = ""
        txt_Cat_SPNo.Text = ""
        txt_Cat_ManualNo.Text = ""
        txt_Cat_ReportNo.Text = ""
        txt_Cat_Title.Text = ""
        txt_Cat_SubTitle.Text = ""
        txt_Cat_VarTitle.Text = ""
        txt_Cat_ScholarName.Text = ""
        txt_Cat_ScholarDept.Text = ""
        txt_Cat_GuideName.Text = ""
        txt_Cat_GuideDept.Text = ""
        txt_Cat_DegreeName.Text = ""
        txt_Cat_SPRevision.Text = ""
        txt_Cat_ManualRevision.Text = ""
        txt_Cat_PatentNo.Text = ""
        txt_Cat_Patentee.Text = ""
        txt_Cat_PatentInventor.Text = ""
        txt_Cat_ConfName.Text = ""
        txt_Cat_SDate.Text = ""
        txt_Cat_EDate.Text = ""
        txt_Cat_ConfPlace.Text = ""
        txt_Cat_Author1.Text = ""
        txt_Cat_Author2.Text = ""
        txt_Cat_Author3.Text = ""
        txt_Cat_Editor.Text = ""
        txt_Cat_Translator.Text = ""
        txt_Cat_Illustrator.Text = ""
        txt_Cat_Compiler.Text = ""
        txt_Cat_Commentator.Text = ""
        txt_Cat_RevisedBy.Text = ""
        txt_Cat_CorpAuthor.Text = ""
        txt_Cat_Edition.Text = ""
        txt_Cat_Reprint.Text = ""
        Pub_ComboBox.ClearSelection()
        txt_Cat_Place.Text = ""
        DDL_Countries.Text = ""
        txt_Cat_Series.Text = ""
        txt_Cat_SeriesEditor.Text = ""
        txt_Cat_Note.Text = ""
        txt_Cat_Remarks.Text = ""
        txt_Cat_URL.Text = ""
        txt_Cat_Comments.Text = ""
        DDL_Subjects.ClearSelection()
        txt_Cat_Keywords.Text = ""
        txt_Cat_TrFrom.Text = ""
        txt_Cat_Abstract.Text = ""
        txt_Cat_ReferenceNo.Text = ""
        txt_Cat_SPAmmendments.Text = ""
        txt_Cat_SPTCSC.Text = ""
        txt_Cat_SPUpdates.Text = ""
        txt_Cat_ReaffirmYear.Text = ""
        txt_Cat_WithdrawYear.Text = ""
        txt_Cat_SPIssueBody.Text = ""
        Me.Label7.Text = ""
        txt_Display_Value.Text = ""
        Image1.ImageUrl = ""
        CheckBox1.Visible = False


        txt_Cat_Producer.Text = ""
        txt_Cat_ProductionYear.Text = ""
        txt_Cat_Designer.Text = ""
        txt_Cat_Manufacturer.Text = ""
        txt_Cat_Creater.Text = ""
        txt_Cat_RoleofCreator.Text = ""
        txt_Cat_Materials.Text = ""
        txt_Cat_Techniq.Text = ""
        txt_Cat_WrokCategory.Text = ""
        txt_Cat_WorkType.Text = ""
        txt_Cat_RelatedWork.Text = ""
        txt_Cat_Source.Text = ""
        txt_Cat_Photographer.Text = ""
        txt_Cat_Nationality.Text = ""

        txt_Cat_Chairman.Text = ""
        DDL_Government.ClearSelection()
        txt_Cat_ActNo.Text = ""
        txt_Cat_ActYear.Text = ""

    End Sub
    'enable/disable Total vol text box
    Protected Sub DDL_YesNo_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDL_YesNo.TextChanged
        If DDL_YesNo.Text = "N" Then
            txt_Cat_TotalVol.Enabled = False
        Else
            txt_Cat_TotalVol.Enabled = True
        End If
        DDL_YesNo.Focus()
    End Sub
    'display record
    Public Function DisplayRecord()
        Dim dt As New DataTable
        Try
            Dim iloop As Integer
            Dim strcurrentchar As Object
            Dim c As Integer
            Dim counter1 As Integer

            Dim myDisplayValue As Object = Nothing
            Dim myDisplayField As Object = Nothing
            If Trim(txt_Display_Value.Text) <> "" Then
                myDisplayValue = TrimAll(txt_Display_Value.Text)
                myDisplayValue = RemoveQuotes(myDisplayValue)

                If Trim(DDL_Display.Text) <> "" Then
                    myDisplayField = DDL_Display.SelectedValue
                    myDisplayField = RemoveQuotes(myDisplayField)
                Else
                    myDisplayField = "CAT_NO"
                End If
            Else
                ClearFields()
                Exit Function
            End If

            'get record details from database
            Dim SQL As String = Nothing
            If myDisplayField = "CAT_NO" Then
                If IsNumeric(myDisplayValue) = False Then
                    Label6.Text = "Cat Number value must be Numeric Only!"
                    Label15.Text = ""
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Cat No must be Numeric Only ');", True)
                    Exit Function
                Else
                    SQL = "SELECT *  FROM CATS WHERE (CAT_NO = '" & Trim(myDisplayValue) & "') "
                End If
            End If
            If myDisplayField = "STANDARD_NO" Then
                SQL = "SELECT *  FROM CATS WHERE (STANDARD_NO = '" & Trim(myDisplayValue) & "') "
            End If
            If myDisplayField = "ACCESSION_NO" Then
                SQL = "SELECT *  FROM CATS where CAT_NO = (SELECT CAT_NO FROM HOLDINGS WHERE (LIB_CODE ='" & Trim(LibCode) & "'  AND ACCESSION_NO ='" & myDisplayValue & "'))"
            End If
            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            da.Fill(ds)

            dt = ds.Tables(0).Copy
            ClearFields()

            If dt.Rows.Count <> 0 Then
                If dt.Rows(0).Item("BIB_CODE").ToString <> "" Then
                    Me.DDL_Bib_Level.SelectedValue = dt.Rows(0).Item("BIB_CODE").ToString
                Else
                    DDL_Bib_Level.SelectedValue = "M"
                End If
                If dt.Rows(0).Item("MAT_CODE").ToString <> "" Then
                    Dim sender As Object = Nothing
                    Dim e As Object = Nothing
                    DDL_Bib_Level_SelectedIndexChanged(sender, e)
                    Me.DDL_Mat_Type.SelectedValue = dt.Rows(0).Item("MAT_CODE").ToString
                Else
                    DDL_Mat_Type.SelectedValue = "B"
                End If
                If dt.Rows(0).Item("DOC_TYPE_CODE").ToString <> "" Then
                    Dim sender As Object = Nothing
                    Dim e As Object = Nothing
                    DDL_Mat_Type_SelectedIndexChanged(sender, e)
                    Me.DDL_Doc_Type.SelectedValue = dt.Rows(0).Item("DOC_TYPE_CODE").ToString
                    LoadDeFormats()
                Else
                    DDL_Doc_Type.SelectedValue = "BK"
                End If

                If dt.Rows(0).Item("LANG_CODE").ToString <> "" Then
                    Me.DDL_Lang.SelectedValue = dt.Rows(0).Item("LANG_CODE").ToString
                Else
                    DDL_Lang.SelectedValue = "ENG"
                End If
                If dt.Rows(0).Item("MULTI_VOL").ToString <> "" Then
                    Me.DDL_YesNo.SelectedValue = dt.Rows(0).Item("MULTI_VOL").ToString
                Else
                    DDL_YesNo.ClearSelection() '
                End If
                If dt.Rows(0).Item("MULTI_VOL").ToString = "Y" Then
                    txt_Cat_TotalVol.Enabled = True
                Else
                    txt_Cat_TotalVol.Enabled = False
                End If
                If dt.Rows(0).Item("TOTAL_VOL").ToString <> "" Then
                    Me.txt_Cat_TotalVol.Text = dt.Rows(0).Item("TOTAL_VOL").ToString
                Else
                    txt_Cat_TotalVol.Text = ""
                End If
                If dt.Rows(0).Item("STANDARD_NO").ToString <> "" Then
                    Me.txt_Cat_ISBN.Text = dt.Rows(0).Item("STANDARD_NO").ToString
                Else
                    txt_Cat_ISBN.Text = ""
                End If
                If dt.Rows(0).Item("SP_NO").ToString <> "" Then
                    Me.txt_Cat_SPNo.Text = dt.Rows(0).Item("SP_NO").ToString
                Else
                    txt_Cat_SPNo.Text = ""
                End If
                If dt.Rows(0).Item("MANUAL_NO").ToString <> "" Then
                    Me.txt_Cat_ManualNo.Text = dt.Rows(0).Item("MANUAL_NO").ToString
                Else
                    txt_Cat_ManualNo.Text = ""
                End If
                If dt.Rows(0).Item("REPORT_NO").ToString <> "" Then
                    Me.txt_Cat_ReportNo.Text = dt.Rows(0).Item("REPORT_NO").ToString
                Else
                    txt_Cat_ReportNo.Text = ""
                End If
                If dt.Rows(0).Item("TITLE").ToString <> "" Then
                    Me.txt_Cat_Title.Text = dt.Rows(0).Item("TITLE").ToString
                Else
                    txt_Cat_Title.Text = ""
                End If
                If dt.Rows(0).Item("SUB_TITLE").ToString <> "" Then
                    Me.txt_Cat_SubTitle.Text = dt.Rows(0).Item("SUB_TITLE").ToString
                Else
                    txt_Cat_SubTitle.Text = ""
                End If
                If dt.Rows(0).Item("VAR_TITLE").ToString <> "" Then
                    Me.txt_Cat_VarTitle.Text = dt.Rows(0).Item("VAR_TITLE").ToString
                Else
                    txt_Cat_VarTitle.Text = ""
                End If
                If dt.Rows(0).Item("SCHOLAR_NAME").ToString <> "" Then
                    Me.txt_Cat_ScholarName.Text = dt.Rows(0).Item("SCHOLAR_NAME").ToString
                Else
                    txt_Cat_ScholarName.Text = ""
                End If
                If dt.Rows(0).Item("SCHOLAR_DEPT").ToString <> "" Then
                    Me.txt_Cat_ScholarDept.Text = dt.Rows(0).Item("SCHOLAR_DEPT").ToString
                Else
                    txt_Cat_ScholarDept.Text = ""
                End If
                If dt.Rows(0).Item("GUIDE_NAME").ToString <> "" Then
                    Me.txt_Cat_GuideName.Text = dt.Rows(0).Item("GUIDE_NAME").ToString
                Else
                    txt_Cat_GuideName.Text = ""
                End If
                If dt.Rows(0).Item("GUIDE_DEPT").ToString <> "" Then
                    Me.txt_Cat_GuideDept.Text = dt.Rows(0).Item("GUIDE_DEPT").ToString
                Else
                    txt_Cat_GuideDept.Text = ""
                End If
                If dt.Rows(0).Item("DEGREE_NAME").ToString <> "" Then
                    Me.txt_Cat_DegreeName.Text = dt.Rows(0).Item("DEGREE_NAME").ToString
                Else
                    txt_Cat_DegreeName.Text = ""
                End If
                If dt.Rows(0).Item("SP_VERSION").ToString <> "" Then
                    Me.txt_Cat_SPRevision.Text = dt.Rows(0).Item("SP_VERSION").ToString
                Else
                    txt_Cat_SPRevision.Text = ""
                End If
                If dt.Rows(0).Item("MANUAL_VER").ToString <> "" Then
                    Me.txt_Cat_ManualRevision.Text = dt.Rows(0).Item("MANUAL_VER").ToString
                Else
                    txt_Cat_ManualRevision.Text = ""
                End If
                If dt.Rows(0).Item("PATENT_NO").ToString <> "" Then
                    Me.txt_Cat_PatentNo.Text = dt.Rows(0).Item("PATENT_NO").ToString
                Else
                    txt_Cat_PatentNo.Text = ""
                End If
                If dt.Rows(0).Item("PATENTEE").ToString <> "" Then
                    Me.txt_Cat_Patentee.Text = dt.Rows(0).Item("PATENTEE").ToString
                Else
                    txt_Cat_Patentee.Text = ""
                End If
                If dt.Rows(0).Item("PATENT_INVENTOR").ToString <> "" Then
                    Me.txt_Cat_PatentInventor.Text = dt.Rows(0).Item("PATENT_INVENTOR").ToString
                Else
                    txt_Cat_PatentInventor.Text = ""
                End If
                If dt.Rows(0).Item("CONF_NAME").ToString <> "" Then
                    Me.txt_Cat_ConfName.Text = dt.Rows(0).Item("CONF_NAME").ToString
                Else
                    txt_Cat_ConfName.Text = ""
                End If
                If dt.Rows(0).Item("CONF_FROM").ToString <> "" Then
                    Me.txt_Cat_SDate.Text = Format(dt.Rows(0).Item("CONF_FROM"), "dd/MM/yyyy")
                Else
                    txt_Cat_SDate.Text = ""
                End If
                If dt.Rows(0).Item("CONF_TO").ToString <> "" Then
                    Me.txt_Cat_EDate.Text = Format(dt.Rows(0).Item("CONF_TO"), "dd/MM/yyyy")
                Else
                    txt_Cat_EDate.Text = ""
                End If
                If dt.Rows(0).Item("CONF_PLACE").ToString <> "" Then
                    Me.txt_Cat_ConfPlace.Text = dt.Rows(0).Item("CONF_PLACE").ToString
                Else
                    txt_Cat_ConfPlace.Text = ""
                End If
                If dt.Rows(0).Item("AUTHOR1").ToString <> "" Then
                    Me.txt_Cat_Author1.Text = dt.Rows(0).Item("AUTHOR1").ToString
                Else
                    txt_Cat_Author1.Text = ""
                End If
                If dt.Rows(0).Item("AUTHOR2").ToString <> "" Then
                    Me.txt_Cat_Author2.Text = dt.Rows(0).Item("AUTHOR2").ToString
                Else
                    txt_Cat_Author2.Text = ""
                End If
                If dt.Rows(0).Item("AUTHOR3").ToString <> "" Then
                    Me.txt_Cat_Author3.Text = dt.Rows(0).Item("AUTHOR3").ToString
                Else
                    txt_Cat_Author3.Text = ""
                End If
                If dt.Rows(0).Item("EDITOR").ToString <> "" Then
                    Me.txt_Cat_Editor.Text = dt.Rows(0).Item("EDITOR").ToString
                Else
                    txt_Cat_Editor.Text = ""
                End If
                If dt.Rows(0).Item("TRANSLATOR").ToString <> "" Then
                    Me.txt_Cat_Translator.Text = dt.Rows(0).Item("TRANSLATOR").ToString
                Else
                    txt_Cat_Translator.Text = ""
                End If
                If dt.Rows(0).Item("ILLUSTRATOR").ToString <> "" Then
                    Me.txt_Cat_Illustrator.Text = dt.Rows(0).Item("ILLUSTRATOR").ToString
                Else
                    txt_Cat_Illustrator.Text = ""
                End If
                If dt.Rows(0).Item("COMPILER").ToString <> "" Then
                    Me.txt_Cat_Compiler.Text = dt.Rows(0).Item("COMPILER").ToString
                Else
                    txt_Cat_Compiler.Text = ""
                End If
                If dt.Rows(0).Item("COMMENTATORS").ToString <> "" Then
                    Me.txt_Cat_Commentator.Text = dt.Rows(0).Item("COMMENTATORS").ToString
                Else
                    txt_Cat_Commentator.Text = ""
                End If
                If dt.Rows(0).Item("REVISED_BY").ToString <> "" Then
                    Me.txt_Cat_RevisedBy.Text = dt.Rows(0).Item("REVISED_BY").ToString
                Else
                    txt_Cat_RevisedBy.Text = ""
                End If
                If dt.Rows(0).Item("CORPORATE_AUTHOR").ToString <> "" Then
                    Me.txt_Cat_CorpAuthor.Text = dt.Rows(0).Item("CORPORATE_AUTHOR").ToString
                Else
                    txt_Cat_CorpAuthor.Text = ""
                End If
                If dt.Rows(0).Item("EDITION").ToString <> "" Then
                    Me.txt_Cat_Edition.Text = dt.Rows(0).Item("EDITION").ToString
                Else
                    txt_Cat_Edition.Text = ""
                End If
                If dt.Rows(0).Item("REPRINTS").ToString <> "" Then
                    Me.txt_Cat_Reprint.Text = dt.Rows(0).Item("REPRINTS").ToString
                Else
                    txt_Cat_Reprint.Text = ""
                End If
                If dt.Rows(0).Item("PUB_ID").ToString <> "" Then
                    Me.Pub_ComboBox.SelectedValue = dt.Rows(0).Item("PUB_ID").ToString
                Else
                    Pub_ComboBox.ClearSelection()
                End If
                If dt.Rows(0).Item("PLACE_OF_PUB").ToString <> "" Then
                    Me.txt_Cat_Place.Text = dt.Rows(0).Item("PLACE_OF_PUB").ToString
                Else
                    txt_Cat_Place.Text = ""
                End If
                If dt.Rows(0).Item("CON_CODE").ToString <> "" Then
                    Me.DDL_Countries.Text = dt.Rows(0).Item("CON_CODE").ToString
                Else
                    DDL_Countries.ClearSelection()
                End If
                If dt.Rows(0).Item("YEAR_OF_PUB").ToString <> "" Then
                    Me.txt_Cat_Year.Text = dt.Rows(0).Item("YEAR_OF_PUB").ToString
                Else
                    txt_Cat_Year.Text = ""
                End If
                If dt.Rows(0).Item("SERIES_TITLE").ToString <> "" Then
                    Me.txt_Cat_Series.Text = dt.Rows(0).Item("SERIES_TITLE").ToString
                Else
                    txt_Cat_Series.Text = ""
                End If
                If dt.Rows(0).Item("SERIES_EDITOR").ToString <> "" Then
                    Me.txt_Cat_SeriesEditor.Text = dt.Rows(0).Item("SERIES_EDITOR").ToString
                Else
                    txt_Cat_SeriesEditor.Text = ""
                End If
                If dt.Rows(0).Item("NOTE").ToString <> "" Then
                    Me.txt_Cat_Note.Text = dt.Rows(0).Item("NOTE").ToString
                Else
                    txt_Cat_Note.Text = ""
                End If
                If dt.Rows(0).Item("REMARKS").ToString <> "" Then
                    Me.txt_Cat_Remarks.Text = dt.Rows(0).Item("REMARKS").ToString
                Else
                    txt_Cat_Remarks.Text = ""
                End If
                If dt.Rows(0).Item("URL").ToString <> "" Then
                    Me.txt_Cat_URL.Text = dt.Rows(0).Item("URL").ToString
                Else
                    txt_Cat_URL.Text = ""
                End If
                If dt.Rows(0).Item("COMMENTS").ToString <> "" Then
                    Me.txt_Cat_Comments.Text = dt.Rows(0).Item("COMMENTS").ToString
                Else
                    txt_Cat_Comments.Text = ""
                End If
                If Convert.ToString(dt.Rows(0).Item("SUB_ID")) <> "" Then
                    Me.DDL_Subjects.SelectedValue = dt.Rows(0).Item("SUB_ID").ToString
                Else
                    DDL_Subjects.ClearSelection()
                End If
                If dt.Rows(0).Item("KEYWORDS").ToString <> "" Then
                    Me.txt_Cat_Keywords.Text = dt.Rows(0).Item("KEYWORDS").ToString
                Else
                    txt_Cat_Keywords.Text = ""
                End If
                If dt.Rows(0).Item("TR_FROM").ToString <> "" Then
                    Me.txt_Cat_TrFrom.Text = dt.Rows(0).Item("TR_FROM").ToString
                Else
                    txt_Cat_TrFrom.Text = ""
                End If
                If dt.Rows(0).Item("ABSTRACT").ToString <> "" Then
                    Me.txt_Cat_Abstract.Text = dt.Rows(0).Item("ABSTRACT").ToString
                Else
                    txt_Cat_Abstract.Text = ""
                End If
                If dt.Rows(0).Item("REFERENCE_NO").ToString <> "" Then
                    Me.txt_Cat_ReferenceNo.Text = dt.Rows(0).Item("REFERENCE_NO").ToString
                Else
                    txt_Cat_ReferenceNo.Text = ""
                End If

                If dt.Rows(0).Item("SP_TCSC").ToString <> "" Then
                    Me.txt_Cat_SPTCSC.Text = dt.Rows(0).Item("SP_TCSC").ToString
                Else
                    txt_Cat_SPTCSC.Text = ""
                End If

                If Convert.ToString(dt.Rows(0).Item("SP_REAFFIRM_YEAR")) <> "" Then
                    Me.txt_Cat_ReaffirmYear.Text = dt.Rows(0).Item("SP_REAFFIRM_YEAR").ToString
                Else
                    txt_Cat_ReaffirmYear.Text = ""
                End If
                If dt.Rows(0).Item("SP_UPDATES").ToString <> "" Then
                    Me.txt_Cat_SPUpdates.Text = dt.Rows(0).Item("SP_UPDATES").ToString
                Else
                    txt_Cat_SPUpdates.Text = ""
                End If

                If Convert.ToString(dt.Rows(0).Item("SP_WITHDRAW_YEAR")) <> "" Then
                    Me.txt_Cat_WithdrawYear.Text = dt.Rows(0).Item("SP_WITHDRAW_YEAR").ToString
                Else
                    txt_Cat_WithdrawYear.Text = ""
                End If
                If dt.Rows(0).Item("SP_AMMENDMENTS").ToString <> "" Then
                    Me.txt_Cat_SPAmmendments.Text = dt.Rows(0).Item("SP_AMMENDMENTS").ToString
                Else
                    txt_Cat_SPAmmendments.Text = ""
                End If
                If dt.Rows(0).Item("SP_ISSUE_BODY").ToString <> "" Then
                    Me.txt_Cat_SPIssueBody.Text = dt.Rows(0).Item("SP_ISSUE_BODY").ToString
                Else
                    txt_Cat_SPIssueBody.Text = ""
                End If

                If dt.Rows(0).Item("PRODUCER").ToString <> "" Then
                    Me.txt_Cat_Producer.Text = dt.Rows(0).Item("PRODUCER").ToString
                Else
                    txt_Cat_Producer.Text = ""
                End If
                If Convert.ToString(dt.Rows(0).Item("PRODUCTION_YEAR")) <> "" Then
                    Me.txt_Cat_ProductionYear.Text = dt.Rows(0).Item("PRODUCTION_YEAR").ToString
                Else
                    txt_Cat_ProductionYear.Text = ""
                End If
                If dt.Rows(0).Item("DESIGNER").ToString <> "" Then
                    Me.txt_Cat_Designer.Text = dt.Rows(0).Item("DESIGNER").ToString
                Else
                    txt_Cat_Designer.Text = ""
                End If
                If dt.Rows(0).Item("MANUFACTURER").ToString <> "" Then
                    Me.txt_Cat_Manufacturer.Text = dt.Rows(0).Item("MANUFACTURER").ToString
                Else
                    txt_Cat_Manufacturer.Text = ""
                End If
                If dt.Rows(0).Item("CREATOR").ToString <> "" Then
                    Me.txt_Cat_Creater.Text = dt.Rows(0).Item("CREATOR").ToString
                Else
                    txt_Cat_Creater.Text = ""
                End If
                If dt.Rows(0).Item("ROLE_OF_CREATOR").ToString <> "" Then
                    Me.txt_Cat_RoleofCreator.Text = dt.Rows(0).Item("ROLE_OF_CREATOR").ToString
                Else
                    txt_Cat_RoleofCreator.Text = ""
                End If
                If dt.Rows(0).Item("MATERIALS").ToString <> "" Then
                    Me.txt_Cat_Materials.Text = dt.Rows(0).Item("MATERIALS").ToString
                Else
                    txt_Cat_Materials.Text = ""
                End If
                If dt.Rows(0).Item("TECHNIQ").ToString <> "" Then
                    Me.txt_Cat_Techniq.Text = dt.Rows(0).Item("TECHNIQ").ToString
                Else
                    txt_Cat_Techniq.Text = ""
                End If

                If dt.Rows(0).Item("WORK_CATEGORY").ToString <> "" Then
                    Me.txt_Cat_WrokCategory.Text = dt.Rows(0).Item("WORK_CATEGORY").ToString
                Else
                    txt_Cat_WrokCategory.Text = ""
                End If

                If dt.Rows(0).Item("WORK_TYPE").ToString <> "" Then
                    Me.txt_Cat_WorkType.Text = dt.Rows(0).Item("WORK_TYPE").ToString
                Else
                    txt_Cat_WorkType.Text = ""
                End If

                If dt.Rows(0).Item("RELATED_WORK").ToString <> "" Then
                    Me.txt_Cat_RelatedWork.Text = dt.Rows(0).Item("RELATED_WORK").ToString
                Else
                    txt_Cat_RelatedWork.Text = ""
                End If

                If dt.Rows(0).Item("SOURCE").ToString <> "" Then
                    Me.txt_Cat_Source.Text = dt.Rows(0).Item("SOURCE").ToString
                Else
                    txt_Cat_Source.Text = ""
                End If

                If dt.Rows(0).Item("PHOTOGRAPHER").ToString <> "" Then
                    Me.txt_Cat_Photographer.Text = dt.Rows(0).Item("PHOTOGRAPHER").ToString
                Else
                    txt_Cat_Photographer.Text = ""
                End If

                If dt.Rows(0).Item("NATIONALITY").ToString <> "" Then
                    Me.txt_Cat_Nationality.Text = dt.Rows(0).Item("NATIONALITY").ToString
                Else
                    txt_Cat_Nationality.Text = ""
                End If

                If dt.Rows(0).Item("CHAIRMAN").ToString <> "" Then
                    Me.txt_Cat_Chairman.Text = dt.Rows(0).Item("CHAIRMAN").ToString
                Else
                    txt_Cat_Chairman.Text = ""
                End If

                If dt.Rows(0).Item("GOVERNMENT").ToString <> "" Then
                    Me.DDL_Government.SelectedValue = dt.Rows(0).Item("GOVERNMENT").ToString
                Else
                    DDL_Government.ClearSelection()
                End If

                If dt.Rows(0).Item("ACT_NO").ToString <> "" Then
                    Me.txt_Cat_ActNo.Text = dt.Rows(0).Item("ACT_NO").ToString
                Else
                    txt_Cat_ActNo.Text = ""
                End If

                If dt.Rows(0).Item("ACT_YEAR").ToString <> "" Then
                    Me.txt_Cat_ActYear.Text = dt.Rows(0).Item("ACT_YEAR").ToString
                Else
                    txt_Cat_ActYear.Text = ""
                End If


                Label7.Text = dt.Rows(0).Item("CAT_NO").ToString
                If dt.Rows(0).Item("PHOTO").ToString <> "" Then
                    Dim strURL As String = "~/Acquisition/Cats_GetPhoto.aspx?CAT_NO=" & Label7.Text & ""
                    Image1.ImageUrl = strURL
                    Image1.Visible = True
                Else
                    Image1.Visible = True
                End If

                If dt.Rows(0).Item("CONT_FILE").ToString <> "" Then
                    Dim strURL As String = "~/Acquisition/Cats_GetContents.aspx?CAT_NO=" & Label7.Text & ""
                    HyperLink1.Visible = True
                    HyperLink1.NavigateUrl = strURL
                    CheckBox3.Visible = True
                Else
                    HyperLink1.Visible = False
                    CheckBox3.Visible = False
                End If
                Acq_Save_Bttn.Visible = False
                Acq_Update_Bttn.Visible = True
                Acq_Delete_Bttn.Visible = True
                Acq_SaveNew_Bttn.Visible = True
                CheckBox1.Visible = True
                CheckBox1.Checked = False
                Label15.Text = "Press UPDATE Button to save the Changes if any.."
                Label6.Text = ""
                SqlConn.Close()
            Else
                Label6.Text = "No Record to Edit... "
                Label7.Text = ""
                Label15.Text = ""
                Acq_Save_Bttn.Visible = True
                Acq_Update_Bttn.Visible = False
                Acq_Delete_Bttn.Visible = False
                Acq_SaveNew_Bttn.Visible = False
                CheckBox1.Visible = False
                CheckBox1.Checked = False
            End If
            DDL_Bib_Level.Focus()
        Catch ex As Exception
            Label6.Text = "Error: " & (ex.Message())
            Label15.Text = ""
        Finally
            dt.Dispose()
            SqlConn.Close()
        End Try
    End Function
    'display Cat Record   
    Public Sub txt_Display_Value_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txt_Display_Value.TextChanged
        Me.DisplayRecord()
        txt_Display_Value.Focus()
    End Sub
    'cancel button
    Protected Sub Acq_Cancel_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Acq_Cancel_Bttn.Click
        ClearFields()
        LoadDeFormats()
        Acq_Save_Bttn.Visible = True
        Acq_Update_Bttn.Visible = False
        Acq_Delete_Bttn.Visible = False
        Acq_SaveNew_Bttn.Visible = False
    End Sub
    'update record
    Protected Sub Acq_Update_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Acq_Update_Bttn.Click
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
                Dim counter1, counter2, counter3, counter4, counter5, counter6, counter7, counter8, counter9, counter10, counter11, counter12, counter13, counter14, counter15, counter16, counter17, counter18, counter19 As Integer
                Dim counter20, counter21, counter22, counter23, counter24, counter25, counter26, counter27, counter28, counter29, counter30, counter31, counter32, counter33, counter34, counter35, counter36, counter37, counter38, counter39 As Integer
                Dim counter40, counter41, counter42, counter43, counter44, counter45, counter46, counter47, counter48, counter49, counter50, counter51, counter52, counter53, counter54 As Integer
                Dim counter55, counter56, counter57, counter58, counter59, counter60 As Integer
                Dim counter71, counter72, counter73, counter74, counter75 As Integer
                '*****************************************************************************************************************
                'validation for DDL_Bib_Level
                Dim myBibLevel As Object = Nothing
                myBibLevel = DDL_Bib_Level.SelectedValue
                If Not String.IsNullOrEmpty(myBibLevel) Then
                    myBibLevel = RemoveQuotes(myBibLevel)
                    If myBibLevel.Length > 2 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_Bib_Level.Focus()
                        Exit Sub
                    End If
                    myBibLevel = " " & myBibLevel & " "
                    If InStr(1, myBibLevel, "CREATE", 1) > 0 Or InStr(1, myBibLevel, "DELETE", 1) > 0 Or InStr(1, myBibLevel, "DROP", 1) > 0 Or InStr(1, myBibLevel, "INSERT", 1) > 1 Or InStr(1, myBibLevel, "TRACK", 1) > 1 Or InStr(1, myBibLevel, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_Bib_Level.Focus()
                        Exit Sub
                    End If
                    myBibLevel = TrimX(myBibLevel)
                    'check unwanted characters
                    c = 0
                    counter1 = 0
                    For iloop = 1 To Len(myBibLevel)
                        strcurrentchar = Mid(myBibLevel, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter1 = 1
                            End If
                        End If
                    Next
                    If counter1 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_Bib_Level.Focus()
                        Exit Sub
                    End If
                Else
                    myBibLevel = "M"
                End If

                'validation for DDL_Mat_Type
                Dim myMatType As Object = Nothing
                myMatType = DDL_Mat_Type.SelectedValue
                If Not String.IsNullOrEmpty(myMatType) Then
                    myMatType = RemoveQuotes(myMatType)
                    If myMatType.Length > 2 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_Mat_Type.Focus()
                        Exit Sub
                    End If
                    myMatType = " " & myMatType & " "
                    If InStr(1, myMatType, "CREATE", 1) > 0 Or InStr(1, myMatType, "DELETE", 1) > 0 Or InStr(1, myMatType, "DROP", 1) > 0 Or InStr(1, myMatType, "INSERT", 1) > 1 Or InStr(1, myMatType, "TRACK", 1) > 1 Or InStr(1, myMatType, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_Mat_Type.Focus()
                        Exit Sub
                    End If
                    myMatType = TrimX(myMatType)
                    'check unwanted characters
                    c = 0
                    counter2 = 0
                    For iloop = 1 To Len(myMatType)
                        strcurrentchar = Mid(myBibLevel, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter2 = 1
                            End If
                        End If
                    Next
                    If counter2 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_Mat_Type.Focus()
                        Exit Sub
                    End If
                Else
                    myMatType = "B"
                End If

                'validation for document Type
                Dim myDocType As Object = Nothing
                myDocType = DDL_Doc_Type.SelectedValue
                If Not String.IsNullOrEmpty(myDocType) Then
                    myDocType = RemoveQuotes(myDocType)
                    If myDocType.Length > 4 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_Doc_Type.Focus()
                        Exit Sub
                    End If
                    myDocType = " " & myDocType & " "
                    If InStr(1, myDocType, "CREATE", 1) > 0 Or InStr(1, myDocType, "DELETE", 1) > 0 Or InStr(1, myDocType, "DROP", 1) > 0 Or InStr(1, myDocType, "INSERT", 1) > 1 Or InStr(1, myDocType, "TRACK", 1) > 1 Or InStr(1, myDocType, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_Doc_Type.Focus()
                        Exit Sub
                    End If
                    myDocType = TrimX(myDocType)
                    'check unwanted characters
                    c = 0
                    counter3 = 0
                    For iloop = 1 To Len(myDocType)
                        strcurrentchar = Mid(myDocType, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter3 = 1
                            End If
                        End If
                    Next
                    If counter3 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_Doc_Type.Focus()
                        Exit Sub
                    End If
                Else
                    myDocType = "BK"
                End If

                'validation for Lang DDL_Lang
                Dim myLangCode As Object = Nothing
                myLangCode = DDL_Lang.SelectedValue
                If Not String.IsNullOrEmpty(myLangCode) Then
                    myLangCode = RemoveQuotes(myLangCode)
                    If myLangCode.Length > 4 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_Lang.Focus()
                        Exit Sub
                    End If
                    myLangCode = " " & myLangCode & " "
                    If InStr(1, myLangCode, "CREATE", 1) > 0 Or InStr(1, myLangCode, "DELETE", 1) > 0 Or InStr(1, myLangCode, "DROP", 1) > 0 Or InStr(1, myLangCode, "INSERT", 1) > 1 Or InStr(1, myLangCode, "TRACK", 1) > 1 Or InStr(1, myLangCode, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_Lang.Focus()
                        Exit Sub
                    End If
                    myLangCode = TrimX(myLangCode)
                    'check unwanted characters
                    c = 0
                    counter4 = 0
                    For iloop = 1 To Len(myLangCode)
                        strcurrentchar = Mid(myLangCode, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter4 = 1
                            End If
                        End If
                    Next
                    If counter4 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_Lang.Focus()
                        Exit Sub
                    End If
                Else
                    myLangCode = "ENG"
                End If

                'DDL_YesNo for Multi-Vol Y/N
                Dim myYesNo As Object = Nothing
                myYesNo = DDL_YesNo.SelectedValue
                If Not String.IsNullOrEmpty(myYesNo) Then
                    myYesNo = RemoveQuotes(myYesNo)
                    If myYesNo.Length > 2 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_YesNo.Focus()
                        Exit Sub
                    End If
                    myYesNo = " " & myYesNo & " "
                    If InStr(1, myYesNo, "CREATE", 1) > 0 Or InStr(1, myYesNo, "DELETE", 1) > 0 Or InStr(1, myYesNo, "DROP", 1) > 0 Or InStr(1, myYesNo, "INSERT", 1) > 1 Or InStr(1, myYesNo, "TRACK", 1) > 1 Or InStr(1, myYesNo, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_YesNo.Focus()
                        Exit Sub
                    End If
                    myYesNo = TrimX(myYesNo)
                    'check unwanted characters
                    c = 0
                    counter5 = 0
                    For iloop = 1 To Len(myYesNo)
                        strcurrentchar = Mid(myYesNo, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter5 = 1
                            End If
                        End If
                    Next
                    If counter5 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_YesNo.Focus()
                        Exit Sub
                    End If
                Else
                    myYesNo = "N"
                End If


                'Server Validation for  total vol - numeric only
                Dim TotalVol As Integer = Nothing
                If myYesNo = "Y" Then
                    If txt_Cat_TotalVol.Text <> "" Then
                        TotalVol = TrimX(txt_Cat_TotalVol.Text)
                        TotalVol = RemoveQuotes(TotalVol)
                        If Len(TotalVol.ToString) > 5 Then 'maximum length
                            Label6.Text = " Data must be of Proper Length.. "
                            Label15.Text = ""
                            txt_Cat_TotalVol.Focus()
                            Exit Sub
                        End If

                        If Not IsNumeric(TotalVol) = True Then 'maximum length
                            Label6.Text = " Data must be Numeric Only.. "
                            Label15.Text = ""
                            txt_Cat_TotalVol.Focus()
                            Exit Sub
                        End If

                        TotalVol = " " & TotalVol & " "
                        If InStr(1, TotalVol, "CREATE", 1) > 0 Or InStr(1, TotalVol, "DELETE", 1) > 0 Or InStr(1, TotalVol, "DROP", 1) > 0 Or InStr(1, TotalVol, "INSERT", 1) > 1 Or InStr(1, TotalVol, "TRACK", 1) > 1 Or InStr(1, TotalVol, "TRACE", 1) > 1 Then
                            Label6.Text = " Do Not use Reserve Words... "
                            Label15.Text = ""
                            Me.txt_Cat_TotalVol.Focus()
                            Exit Sub
                        End If
                        TotalVol = TrimX(TotalVol)
                        'check unwanted characters
                        c = 0
                        counter6 = 0
                        For iloop = 1 To Len(TotalVol.ToString)
                            strcurrentchar = Mid(TotalVol, iloop, 1)
                            If c = 0 Then
                                If Not InStr("'~!@#$^*+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter6 = 1
                                End If
                            End If
                        Next
                        If counter6 = 1 Then
                            Label6.Text = " Do Not use un-wanted Characters... "
                            Label15.Text = ""
                            Me.txt_Cat_TotalVol.Focus()
                            Exit Sub
                        End If
                    Else
                        TotalVol = Nothing
                    End If
                End If

                'Server Validation for txt_Cat_ISBN
                Dim myISBN As Object = Nothing
                If txt_Cat_ISBN.Text <> "" Then
                    myISBN = TrimX(txt_Cat_ISBN.Text)
                    myISBN = RemoveQuotes(myISBN)
                    If myISBN.Length > 30 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_ISBN.Focus()
                        Exit Sub
                    End If

                    myISBN = " " & myISBN & " "
                    If InStr(1, myISBN, "CREATE", 1) > 0 Or InStr(1, myISBN, "DELETE", 1) > 0 Or InStr(1, myISBN, "DROP", 1) > 0 Or InStr(1, myISBN, "INSERT", 1) > 1 Or InStr(1, myISBN, "TRACK", 1) > 1 Or InStr(1, myISBN, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_ISBN.Focus()
                        Exit Sub
                    End If
                    myISBN = TrimX(myISBN)
                    'check unwanted characters
                    c = 0
                    counter7 = 0
                    For iloop = 1 To Len(myISBN.ToString)
                        strcurrentchar = Mid(myISBN, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^*+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter7 = 1
                            End If
                        End If
                    Next
                    If counter7 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_ISBN.Focus()
                        Exit Sub
                    End If
                    'check duplicate isbn
                    Dim str As Object = Nothing
                    Dim flag As Object = Nothing
                    str = "SELECT CAT_NO FROM CATS WHERE (replace(STANDARD_NO,'-','') = '" & Replace(myISBN.ToString, "-", "") & "' AND CAT_NO<> ' " & Trim(Label7.Text) & "') "
                    Dim cmd1 As New SqlCommand(str, SqlConn)
                    SqlConn.Open()
                    flag = cmd1.ExecuteScalar
                    If flag <> Nothing Then
                        Label6.Text = "This ISBN Already Exists ! "
                        Label15.Text = ""
                        Me.txt_Cat_ISBN.Focus()
                        Exit Sub
                    End If
                    SqlConn.Close()
                Else
                    myISBN = ""
                End If

                'Server validation for Standard Specification No : txt_Cat_SPNo
                Dim mySPNo As Object = Nothing
                If txt_Cat_SPNo.Text <> "" Then
                    mySPNo = TrimX(txt_Cat_SPNo.Text)
                    mySPNo = RemoveQuotes(mySPNo)
                    If mySPNo.Length > 100 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_SPNo.Focus()
                        Exit Sub
                    End If

                    mySPNo = " " & mySPNo & " "
                    If InStr(1, mySPNo, "CREATE", 1) > 0 Or InStr(1, mySPNo, "DELETE", 1) > 0 Or InStr(1, mySPNo, "DROP", 1) > 0 Or InStr(1, mySPNo, "INSERT", 1) > 1 Or InStr(1, mySPNo, "TRACK", 1) > 1 Or InStr(1, mySPNo, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_SPNo.Focus()
                        Exit Sub
                    End If
                    mySPNo = TrimX(mySPNo)
                    'check unwanted characters
                    c = 0
                    counter8 = 0
                    For iloop = 1 To Len(mySPNo.ToString)
                        strcurrentchar = Mid(mySPNo, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^*+|?<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter8 = 1
                            End If
                        End If
                    Next
                    If counter8 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_SPNo.Focus()
                        Exit Sub
                    End If
                Else
                    mySPNo = ""
                End If

                'Server validation for Standard Specification No : txt_Cat_ManualNo
                Dim myManualNo As Object = Nothing
                If txt_Cat_ManualNo.Text <> "" Then
                    myManualNo = TrimX(txt_Cat_ManualNo.Text)
                    myManualNo = RemoveQuotes(myManualNo)
                    If myManualNo.Length > 100 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_ManualNo.Focus()
                        Exit Sub
                    End If

                    myManualNo = " " & myManualNo & " "
                    If InStr(1, myManualNo, "CREATE", 1) > 0 Or InStr(1, myManualNo, "DELETE", 1) > 0 Or InStr(1, myManualNo, "DROP", 1) > 0 Or InStr(1, myManualNo, "INSERT", 1) > 1 Or InStr(1, myManualNo, "TRACK", 1) > 1 Or InStr(1, myManualNo, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_ManualNo.Focus()
                        Exit Sub
                    End If
                    myManualNo = TrimX(myManualNo)
                    'check unwanted characters
                    c = 0
                    counter9 = 0
                    For iloop = 1 To Len(myManualNo.ToString)
                        strcurrentchar = Mid(myManualNo, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^*+|?<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter9 = 1
                            End If
                        End If
                    Next
                    If counter9 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_ManualNo.Focus()
                        Exit Sub
                    End If
                Else
                    myManualNo = ""
                End If

                'Server validation for  : txt_Cat_ReportNo
                Dim myReportNo As Object = Nothing
                If txt_Cat_ReportNo.Text <> "" Then
                    myReportNo = TrimX(txt_Cat_ReportNo.Text)
                    myReportNo = RemoveQuotes(myReportNo)
                    If myReportNo.Length > 100 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_ReportNo.Focus()
                        Exit Sub
                    End If

                    myReportNo = " " & myReportNo & " "
                    If InStr(1, myReportNo, "CREATE", 1) > 0 Or InStr(1, myReportNo, "DELETE", 1) > 0 Or InStr(1, myReportNo, "DROP", 1) > 0 Or InStr(1, myReportNo, "INSERT", 1) > 1 Or InStr(1, myReportNo, "TRACK", 1) > 1 Or InStr(1, myReportNo, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_ReportNo.Focus()
                        Exit Sub
                    End If
                    myReportNo = TrimX(myReportNo)
                    'check unwanted characters
                    c = 0
                    counter10 = 0
                    For iloop = 1 To Len(myReportNo.ToString)
                        strcurrentchar = Mid(myReportNo, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^*+|?<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter10 = 1
                            End If
                        End If
                    Next
                    If counter10 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_ReportNo.Focus()
                        Exit Sub
                    End If
                Else
                    myReportNo = ""
                End If

                'Server validation for  : txt_Cat_Title
                Dim myTitle As Object = Nothing
                If txt_Cat_Title.Text <> "" Then
                    myTitle = TrimAll(txt_Cat_Title.Text)
                    myTitle = RemoveQuotes(myTitle)
                    If myTitle.Length > 500 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Title.Focus()
                        Exit Sub
                    End If

                    myTitle = " " & myTitle & " "
                    If InStr(1, myTitle, "CREATE", 1) > 0 Or InStr(1, myTitle, "DELETE", 1) > 0 Or InStr(1, myTitle, "DROP", 1) > 0 Or InStr(1, myTitle, "INSERT", 1) > 1 Or InStr(1, myTitle, "TRACK", 1) > 1 Or InStr(1, myTitle, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_Title.Focus()
                        Exit Sub
                    End If
                    myTitle = TrimAll(myTitle)
                    'check unwanted characters
                    c = 0
                    counter11 = 0
                    For iloop = 1 To Len(myTitle.ToString)
                        strcurrentchar = Mid(myTitle, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!+|""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter11 = 1
                            End If
                        End If
                    Next
                    If counter11 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_Title.Focus()
                        Exit Sub
                    End If
                Else
                    Label6.Text = " Plz Enter Data... "
                    Label15.Text = ""
                    Me.txt_Cat_Title.Focus()
                    Exit Sub
                End If

                'Server validation for  : txt_Cat_SubTitle
                Dim mySubTitle As Object = Nothing
                If txt_Cat_SubTitle.Text <> "" Then
                    mySubTitle = TrimAll(txt_Cat_SubTitle.Text)
                    mySubTitle = RemoveQuotes(mySubTitle)
                    If mySubTitle.Length > 500 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_SubTitle.Focus()
                        Exit Sub
                    End If

                    mySubTitle = " " & mySubTitle & " "
                    If InStr(1, mySubTitle, "CREATE", 1) > 0 Or InStr(1, mySubTitle, "DELETE", 1) > 0 Or InStr(1, mySubTitle, "DROP", 1) > 0 Or InStr(1, mySubTitle, "INSERT", 1) > 1 Or InStr(1, mySubTitle, "TRACK", 1) > 1 Or InStr(1, mySubTitle, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_SubTitle.Focus()
                        Exit Sub
                    End If
                    mySubTitle = TrimAll(mySubTitle)
                    'check unwanted characters
                    c = 0
                    counter12 = 0
                    For iloop = 1 To Len(mySubTitle.ToString)
                        strcurrentchar = Mid(mySubTitle, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter12 = 1
                            End If
                        End If
                    Next
                    If counter12 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_SubTitle.Focus()
                        Exit Sub
                    End If
                Else
                    mySubTitle = ""
                End If

                'Server validation for  : txt_Cat_VarTitle
                Dim myVarTitle As Object = Nothing
                If txt_Cat_VarTitle.Text <> "" Then
                    myVarTitle = TrimAll(txt_Cat_VarTitle.Text)
                    myVarTitle = RemoveQuotes(myVarTitle)
                    If myVarTitle.Length > 500 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_VarTitle.Focus()
                        Exit Sub
                    End If

                    myVarTitle = " " & myVarTitle & " "
                    If InStr(1, myVarTitle, "CREATE", 1) > 0 Or InStr(1, myVarTitle, "DELETE", 1) > 0 Or InStr(1, myVarTitle, "DROP", 1) > 0 Or InStr(1, myVarTitle, "INSERT", 1) > 1 Or InStr(1, myVarTitle, "TRACK", 1) > 1 Or InStr(1, myVarTitle, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_VarTitle.Focus()
                        Exit Sub
                    End If
                    myVarTitle = TrimAll(myVarTitle)
                    'check unwanted characters
                    c = 0
                    counter13 = 0
                    For iloop = 1 To Len(myVarTitle.ToString)
                        strcurrentchar = Mid(myVarTitle, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter13 = 1
                            End If
                        End If
                    Next
                    If counter13 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_VarTitle.Focus()
                        Exit Sub
                    End If
                Else
                    myVarTitle = ""
                End If

                'Server validation for  : txt_Cat_ScholarName
                Dim myScholarName As Object = Nothing
                If txt_Cat_ScholarName.Text <> "" Then
                    myScholarName = TrimAll(txt_Cat_ScholarName.Text)
                    myScholarName = RemoveQuotes(myScholarName)
                    If myScholarName.Length > 500 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_ScholarName.Focus()
                        Exit Sub
                    End If

                    myScholarName = " " & myScholarName & " "
                    If InStr(1, myScholarName, "CREATE", 1) > 0 Or InStr(1, myScholarName, "DELETE", 1) > 0 Or InStr(1, myScholarName, "DROP", 1) > 0 Or InStr(1, myScholarName, "INSERT", 1) > 1 Or InStr(1, myScholarName, "TRACK", 1) > 1 Or InStr(1, myScholarName, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_ScholarName.Focus()
                        Exit Sub
                    End If
                    myScholarName = TrimAll(myScholarName)
                    'check unwanted characters
                    c = 0
                    counter14 = 0
                    For iloop = 1 To Len(myScholarName.ToString)
                        strcurrentchar = Mid(myScholarName, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter14 = 1
                            End If
                        End If
                    Next
                    If counter14 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_ScholarName.Focus()
                        Exit Sub
                    End If
                Else
                    myScholarName = ""
                End If

                'Server validation for  : txt_Cat_ScholarDept
                Dim myScholarDept As Object = Nothing
                If txt_Cat_ScholarDept.Text <> "" Then
                    myScholarDept = TrimAll(txt_Cat_ScholarDept.Text)
                    myScholarDept = RemoveQuotes(myScholarDept)
                    If myScholarDept.Length > 500 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_ScholarDept.Focus()
                        Exit Sub
                    End If

                    myScholarDept = " " & myScholarDept & " "
                    If InStr(1, myScholarDept, "CREATE", 1) > 0 Or InStr(1, myScholarDept, "DELETE", 1) > 0 Or InStr(1, myScholarDept, "DROP", 1) > 0 Or InStr(1, myScholarDept, "INSERT", 1) > 1 Or InStr(1, myScholarDept, "TRACK", 1) > 1 Or InStr(1, myScholarDept, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_ScholarDept.Focus()
                        Exit Sub
                    End If
                    myScholarDept = TrimAll(myScholarDept)
                    'check unwanted characters
                    c = 0
                    counter15 = 0
                    For iloop = 1 To Len(myScholarDept.ToString)
                        strcurrentchar = Mid(myScholarDept, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter15 = 1
                            End If
                        End If
                    Next
                    If counter15 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_ScholarDept.Focus()
                        Exit Sub
                    End If
                Else
                    myScholarDept = ""
                End If

                'Server validation for  : txt_Cat_GuideName
                Dim myGuideName As Object = Nothing
                If txt_Cat_GuideName.Text <> "" Then
                    myGuideName = TrimAll(txt_Cat_GuideName.Text)
                    myGuideName = RemoveQuotes(myGuideName)
                    If myGuideName.Length > 500 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_GuideName.Focus()
                        Exit Sub
                    End If

                    myGuideName = " " & myGuideName & " "
                    If InStr(1, myGuideName, "CREATE", 1) > 0 Or InStr(1, myGuideName, "DELETE", 1) > 0 Or InStr(1, myGuideName, "DROP", 1) > 0 Or InStr(1, myGuideName, "INSERT", 1) > 1 Or InStr(1, myGuideName, "TRACK", 1) > 1 Or InStr(1, myGuideName, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_GuideName.Focus()
                        Exit Sub
                    End If
                    myGuideName = TrimAll(myGuideName)
                    'check unwanted characters
                    c = 0
                    counter16 = 0
                    For iloop = 1 To Len(myGuideName.ToString)
                        strcurrentchar = Mid(myGuideName, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter16 = 1
                            End If
                        End If
                    Next
                    If counter16 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_GuideName.Focus()
                        Exit Sub
                    End If
                Else
                    myGuideName = ""
                End If

                'Server validation for  : txt_Cat_GuideDept
                Dim myGuideDept As Object = Nothing
                If txt_Cat_GuideDept.Text <> "" Then
                    myGuideDept = TrimAll(txt_Cat_GuideDept.Text)
                    myGuideDept = RemoveQuotes(myGuideDept)
                    If myGuideDept.Length > 500 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_GuideDept.Focus()
                        Exit Sub
                    End If

                    myGuideDept = " " & myGuideDept & " "
                    If InStr(1, myGuideDept, "CREATE", 1) > 0 Or InStr(1, myGuideDept, "DELETE", 1) > 0 Or InStr(1, myGuideDept, "DROP", 1) > 0 Or InStr(1, myGuideDept, "INSERT", 1) > 1 Or InStr(1, myGuideDept, "TRACK", 1) > 1 Or InStr(1, myGuideDept, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_GuideDept.Focus()
                        Exit Sub
                    End If
                    myGuideDept = TrimAll(myGuideDept)
                    'check unwanted characters
                    c = 0
                    counter17 = 0
                    For iloop = 1 To Len(myGuideDept.ToString)
                        strcurrentchar = Mid(myGuideDept, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter17 = 1
                            End If
                        End If
                    Next
                    If counter17 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_GuideDept.Focus()
                        Exit Sub
                    End If
                Else
                    myGuideDept = ""
                End If

                'Server validation for  : txt_Cat_DegreeName
                Dim myDegreeName As Object = Nothing
                If txt_Cat_DegreeName.Text <> "" Then
                    myDegreeName = TrimAll(txt_Cat_DegreeName.Text)
                    myDegreeName = RemoveQuotes(myDegreeName)
                    If myDegreeName.Length > 500 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_DegreeName.Focus()
                        Exit Sub
                    End If

                    myDegreeName = " " & myDegreeName & " "
                    If InStr(1, myDegreeName, "CREATE", 1) > 0 Or InStr(1, myDegreeName, "DELETE", 1) > 0 Or InStr(1, myDegreeName, "DROP", 1) > 0 Or InStr(1, myDegreeName, "INSERT", 1) > 1 Or InStr(1, myDegreeName, "TRACK", 1) > 1 Or InStr(1, myDegreeName, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_DegreeName.Focus()
                        Exit Sub
                    End If
                    myDegreeName = TrimAll(myDegreeName)
                    'check unwanted characters
                    c = 0
                    counter18 = 0
                    For iloop = 1 To Len(myDegreeName.ToString)
                        strcurrentchar = Mid(myDegreeName, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter18 = 1
                            End If
                        End If
                    Next
                    If counter18 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_DegreeName.Focus()
                        Exit Sub
                    End If
                Else
                    myDegreeName = ""
                End If

                'Server validation for  : txt_Cat_SPRevision
                Dim mySPRev As Object = Nothing
                If txt_Cat_SPRevision.Text <> "" Then
                    mySPRev = TrimAll(txt_Cat_SPRevision.Text)
                    mySPRev = RemoveQuotes(mySPRev)
                    If mySPRev.Length > 100 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_SPRevision.Focus()
                        Exit Sub
                    End If

                    mySPRev = " " & mySPRev & " "
                    If InStr(1, mySPRev, "CREATE", 1) > 0 Or InStr(1, mySPRev, "DELETE", 1) > 0 Or InStr(1, mySPRev, "DROP", 1) > 0 Or InStr(1, mySPRev, "INSERT", 1) > 1 Or InStr(1, mySPRev, "TRACK", 1) > 1 Or InStr(1, mySPRev, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_SPRevision.Focus()
                        Exit Sub
                    End If
                    mySPRev = TrimAll(mySPRev)
                    'check unwanted characters
                    c = 0
                    counter19 = 0
                    For iloop = 1 To Len(mySPRev.ToString)
                        strcurrentchar = Mid(mySPRev, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter19 = 1
                            End If
                        End If
                    Next
                    If counter19 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_SPRevision.Focus()
                        Exit Sub
                    End If
                Else
                    mySPRev = ""
                End If

                'Server validation for  : txt_Cat_ManualRevision
                Dim myManualVer As Object = Nothing
                If txt_Cat_ManualRevision.Text <> "" Then
                    myManualVer = TrimAll(txt_Cat_ManualRevision.Text)
                    myManualVer = RemoveQuotes(myManualVer)
                    If myManualVer.Length > 50 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_ManualRevision.Focus()
                        Exit Sub
                    End If

                    myManualVer = " " & myManualVer & " "
                    If InStr(1, myManualVer, "CREATE", 1) > 0 Or InStr(1, myManualVer, "DELETE", 1) > 0 Or InStr(1, myManualVer, "DROP", 1) > 0 Or InStr(1, myManualVer, "INSERT", 1) > 1 Or InStr(1, myManualVer, "TRACK", 1) > 1 Or InStr(1, myManualVer, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_ManualRevision.Focus()
                        Exit Sub
                    End If
                    myManualVer = TrimAll(myManualVer)
                    'check unwanted characters
                    c = 0
                    counter20 = 0
                    For iloop = 1 To Len(myManualVer.ToString)
                        strcurrentchar = Mid(myManualVer, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter20 = 1
                            End If
                        End If
                    Next
                    If counter20 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_ManualRevision.Focus()
                        Exit Sub
                    End If
                Else
                    myManualVer = ""
                End If

                'Server validation for  : txt_Cat_PatentNo
                Dim myPatentNo As Object = Nothing
                If txt_Cat_PatentNo.Text <> "" Then
                    myPatentNo = TrimAll(txt_Cat_PatentNo.Text)
                    myPatentNo = RemoveQuotes(myPatentNo)
                    If myPatentNo.Length > 100 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_PatentNo.Focus()
                        Exit Sub
                    End If

                    myPatentNo = " " & myPatentNo & " "
                    If InStr(1, myPatentNo, "CREATE", 1) > 0 Or InStr(1, myPatentNo, "DELETE", 1) > 0 Or InStr(1, myPatentNo, "DROP", 1) > 0 Or InStr(1, myPatentNo, "INSERT", 1) > 1 Or InStr(1, myPatentNo, "TRACK", 1) > 1 Or InStr(1, myPatentNo, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_PatentNo.Focus()
                        Exit Sub
                    End If
                    myPatentNo = TrimAll(myPatentNo)
                    'check unwanted characters
                    c = 0
                    counter21 = 0
                    For iloop = 1 To Len(myPatentNo.ToString)
                        strcurrentchar = Mid(myPatentNo, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter21 = 1
                            End If
                        End If
                    Next
                    If counter21 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_PatentNo.Focus()
                        Exit Sub
                    End If
                Else
                    myPatentNo = ""
                End If


                'Server validation for  : txt_Cat_Patentee
                Dim myPatentee As Object = Nothing
                If txt_Cat_Patentee.Text <> "" Then
                    myPatentee = TrimAll(txt_Cat_Patentee.Text)
                    myPatentee = RemoveQuotes(myPatentee)
                    If myPatentee.Length > 150 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Patentee.Focus()
                        Exit Sub
                    End If

                    myPatentee = " " & myPatentee & " "
                    If InStr(1, myPatentee, "CREATE", 1) > 0 Or InStr(1, myPatentee, "DELETE", 1) > 0 Or InStr(1, myPatentee, "DROP", 1) > 0 Or InStr(1, myPatentee, "INSERT", 1) > 1 Or InStr(1, myPatentee, "TRACK", 1) > 1 Or InStr(1, myPatentee, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_Patentee.Focus()
                        Exit Sub
                    End If
                    myPatentee = TrimAll(myPatentee)
                    'check unwanted characters
                    c = 0
                    counter22 = 0
                    For iloop = 1 To Len(myPatentee.ToString)
                        strcurrentchar = Mid(myPatentee, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter22 = 1
                            End If
                        End If
                    Next
                    If counter22 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_Patentee.Focus()
                        Exit Sub
                    End If
                Else
                    myPatentee = ""
                End If


                'Server validation for  : txt_Cat_PatentInventor
                Dim myPatentInventor As Object = Nothing
                If txt_Cat_PatentInventor.Text <> "" Then
                    myPatentInventor = TrimAll(txt_Cat_PatentInventor.Text)
                    myPatentInventor = RemoveQuotes(myPatentInventor)
                    If myPatentInventor.Length > 256 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_PatentInventor.Focus()
                        Exit Sub
                    End If

                    myPatentInventor = " " & myPatentInventor & " "
                    If InStr(1, myPatentInventor, "CREATE", 1) > 0 Or InStr(1, myPatentInventor, "DELETE", 1) > 0 Or InStr(1, myPatentInventor, "DROP", 1) > 0 Or InStr(1, myPatentInventor, "INSERT", 1) > 1 Or InStr(1, myPatentInventor, "TRACK", 1) > 1 Or InStr(1, myPatentInventor, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_PatentInventor.Focus()
                        Exit Sub
                    End If
                    myPatentInventor = TrimAll(myPatentInventor)
                    'check unwanted characters
                    c = 0
                    counter23 = 0
                    For iloop = 1 To Len(myPatentInventor.ToString)
                        strcurrentchar = Mid(myPatentInventor, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter23 = 1
                            End If
                        End If
                    Next
                    If counter23 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_PatentInventor.Focus()
                        Exit Sub
                    End If
                Else
                    myPatentInventor = ""
                End If


                'Server validation for  : txt_Cat_ConfName
                Dim myConfName As Object = Nothing
                If txt_Cat_ConfName.Text <> "" Then
                    myConfName = TrimAll(txt_Cat_ConfName.Text)
                    myConfName = RemoveQuotes(myConfName)
                    If myConfName.Length > 500 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_ConfName.Focus()
                        Exit Sub
                    End If

                    myConfName = " " & myConfName & " "
                    If InStr(1, myConfName, "CREATE", 1) > 0 Or InStr(1, myConfName, "DELETE", 1) > 0 Or InStr(1, myConfName, "DROP", 1) > 0 Or InStr(1, myConfName, "INSERT", 1) > 1 Or InStr(1, myConfName, "TRACK", 1) > 1 Or InStr(1, myConfName, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_ConfName.Focus()
                        Exit Sub
                    End If
                    myConfName = TrimAll(myConfName)
                    'check unwanted characters
                    c = 0
                    counter24 = 0
                    For iloop = 1 To Len(myConfName.ToString)
                        strcurrentchar = Mid(myConfName, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter24 = 1
                            End If
                        End If
                    Next
                    If counter24 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_ConfName.Focus()
                        Exit Sub
                    End If
                Else
                    myConfName = ""
                End If


                'Server validation for  : txt_Cat_ConfName
                Dim myConfSDate As Object = Nothing
                If txt_Cat_SDate.Text <> "" Then
                    myConfSDate = TrimX(txt_Cat_SDate.Text)
                    myConfSDate = RemoveQuotes(myConfSDate)
                    myConfSDate = Convert.ToDateTime(myConfSDate, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us
                    If myConfSDate.Length > 12 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_SDate.Focus()
                        Exit Sub
                    End If

                    myConfSDate = " " & myConfSDate & " "
                    If InStr(1, myConfSDate, "CREATE", 1) > 0 Or InStr(1, myConfSDate, "DELETE", 1) > 0 Or InStr(1, myConfSDate, "DROP", 1) > 0 Or InStr(1, myConfSDate, "INSERT", 1) > 1 Or InStr(1, myConfSDate, "TRACK", 1) > 1 Or InStr(1, myConfSDate, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_SDate.Focus()
                        Exit Sub
                    End If
                    myConfSDate = TrimX(myConfSDate)
                    'check unwanted characters
                    c = 0
                    counter25 = 0
                    For iloop = 1 To Len(myConfSDate.ToString)
                        strcurrentchar = Mid(myConfSDate, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter25 = 1
                            End If
                        End If
                    Next
                    If counter25 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_SDate.Focus()
                        Exit Sub
                    End If
                Else
                    myConfSDate = ""
                End If

                'Server validation for  : txt_Cat_EDate
                Dim myConfEDate As Object = Nothing
                If txt_Cat_EDate.Text <> "" Then
                    myConfEDate = TrimX(txt_Cat_EDate.Text)
                    myConfEDate = RemoveQuotes(myConfEDate)
                    myConfEDate = Convert.ToDateTime(myConfEDate, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us
                    If myConfEDate.Length > 12 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_EDate.Focus()
                        Exit Sub
                    End If

                    myConfEDate = " " & myConfEDate & " "
                    If InStr(1, myConfEDate, "CREATE", 1) > 0 Or InStr(1, myConfEDate, "DELETE", 1) > 0 Or InStr(1, myConfEDate, "DROP", 1) > 0 Or InStr(1, myConfEDate, "INSERT", 1) > 1 Or InStr(1, myConfEDate, "TRACK", 1) > 1 Or InStr(1, myConfEDate, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_EDate.Focus()
                        Exit Sub
                    End If
                    myConfEDate = TrimX(myConfEDate)
                    'check unwanted characters
                    c = 0
                    counter26 = 0
                    For iloop = 1 To Len(myConfEDate.ToString)
                        strcurrentchar = Mid(myConfEDate, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter26 = 1
                            End If
                        End If
                    Next
                    If counter26 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_EDate.Focus()
                        Exit Sub
                    End If
                Else
                    myConfEDate = ""
                End If

                'Server validation for  : txt_Cat_ConfPlace
                Dim myConfPlace As Object = Nothing
                If txt_Cat_ConfPlace.Text <> "" Then
                    myConfPlace = TrimAll(txt_Cat_ConfPlace.Text)
                    myConfPlace = RemoveQuotes(myConfPlace)
                    If myConfPlace.Length > 100 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_ConfName.Focus()
                        Exit Sub
                    End If

                    myConfPlace = " " & myConfPlace & " "
                    If InStr(1, myConfPlace, "CREATE", 1) > 0 Or InStr(1, myConfPlace, "DELETE", 1) > 0 Or InStr(1, myConfPlace, "DROP", 1) > 0 Or InStr(1, myConfPlace, "INSERT", 1) > 1 Or InStr(1, myConfPlace, "TRACK", 1) > 1 Or InStr(1, myConfPlace, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_ConfPlace.Focus()
                        Exit Sub
                    End If
                    myConfPlace = TrimAll(myConfPlace)
                    'check unwanted characters
                    c = 0
                    counter27 = 0
                    For iloop = 1 To Len(myConfPlace.ToString)
                        strcurrentchar = Mid(myConfPlace, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter27 = 1
                            End If
                        End If
                    Next
                    If counter27 = 1 Then
                        Label6.Text = "Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_ConfPlace.Focus()
                        Exit Sub
                    End If
                Else
                    myConfPlace = ""
                End If

                'Server validation for  : txt_Cat_Author1
                Dim myAuthor1 As Object = Nothing
                If txt_Cat_Author1.Text <> "" Then
                    myAuthor1 = TrimAll(txt_Cat_Author1.Text)
                    myAuthor1 = RemoveQuotes(myAuthor1)
                    If myAuthor1.Length > 250 Then 'maximum length
                        Label6.Text = "Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Author1.Focus()
                        Exit Sub
                    End If

                    myAuthor1 = " " & myAuthor1 & " "
                    If InStr(1, myAuthor1, "CREATE", 1) > 0 Or InStr(1, myAuthor1, "DELETE", 1) > 0 Or InStr(1, myAuthor1, "DROP", 1) > 0 Or InStr(1, myAuthor1, "INSERT", 1) > 1 Or InStr(1, myAuthor1, "TRACK", 1) > 1 Or InStr(1, myAuthor1, "TRACE", 1) > 1 Then
                        Label6.Text = "Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_Author1.Focus()
                        Exit Sub
                    End If
                    myAuthor1 = TrimAll(myAuthor1)
                    'check unwanted characters
                    c = 0
                    counter28 = 0
                    For iloop = 1 To Len(myAuthor1.ToString)
                        strcurrentchar = Mid(myAuthor1, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter28 = 1
                            End If
                        End If
                    Next
                    myAuthor1 = TrimAll(Replace(myAuthor1, ".", " "))
                    myAuthor1 = TrimAll(Replace(myAuthor1, ",", ", "))
                    myAuthor1 = TrimAll(Replace(myAuthor1, ":", ", "))

                    If counter28 = 1 Then
                        Label6.Text = "Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_Author1.Focus()
                        Exit Sub
                    End If
                Else
                    myAuthor1 = ""
                End If

                'Server validation for  : txt_Cat_Author2
                Dim myAuthor2 As Object = Nothing
                If txt_Cat_Author2.Text <> "" Then
                    myAuthor2 = TrimAll(txt_Cat_Author2.Text)
                    myAuthor2 = RemoveQuotes(myAuthor2)
                    If myAuthor2.Length > 250 Then 'maximum length
                        Label6.Text = "Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Author2.Focus()
                        Exit Sub
                    End If

                    myAuthor2 = " " & myAuthor2 & " "
                    If InStr(1, myAuthor2, "CREATE", 1) > 0 Or InStr(1, myAuthor2, "DELETE", 1) > 0 Or InStr(1, myAuthor2, "DROP", 1) > 0 Or InStr(1, myAuthor2, "INSERT", 1) > 1 Or InStr(1, myAuthor2, "TRACK", 1) > 1 Or InStr(1, myAuthor2, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_Author2.Focus()
                        Exit Sub
                    End If
                    myAuthor2 = TrimAll(myAuthor2)
                    'check unwanted characters
                    c = 0
                    counter29 = 0
                    For iloop = 1 To Len(myAuthor2.ToString)
                        strcurrentchar = Mid(myAuthor2, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter29 = 1
                            End If
                        End If
                    Next
                    myAuthor2 = TrimAll(Replace(myAuthor2, ".", " "))
                    myAuthor2 = TrimAll(Replace(myAuthor2, ",", ", "))
                    myAuthor2 = TrimAll(Replace(myAuthor2, ":", ", "))
                    If counter29 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_Author2.Focus()
                        Exit Sub
                    End If
                Else
                    myAuthor2 = ""
                End If

                'Server validation for  : txt_Cat_Author2
                Dim myAuthor3 As Object = Nothing
                If txt_Cat_Author3.Text <> "" Then
                    myAuthor3 = TrimAll(txt_Cat_Author3.Text)
                    myAuthor3 = RemoveQuotes(myAuthor3)
                    If myAuthor3.Length > 250 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Author3.Focus()
                        Exit Sub
                    End If

                    myAuthor3 = " " & myAuthor3 & " "
                    If InStr(1, myAuthor3, "CREATE", 1) > 0 Or InStr(1, myAuthor3, "DELETE", 1) > 0 Or InStr(1, myAuthor3, "DROP", 1) > 0 Or InStr(1, myAuthor3, "INSERT", 1) > 1 Or InStr(1, myAuthor3, "TRACK", 1) > 1 Or InStr(1, myAuthor3, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_Author3.Focus()
                        Exit Sub
                    End If
                    myAuthor3 = TrimAll(myAuthor3)
                    'check unwanted characters
                    c = 0
                    counter30 = 0
                    For iloop = 1 To Len(myAuthor3.ToString)
                        strcurrentchar = Mid(myAuthor3, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter30 = 1
                            End If
                        End If
                    Next
                    myAuthor3 = TrimAll(Replace(myAuthor3, ".", " "))
                    myAuthor3 = TrimAll(Replace(myAuthor3, ",", ", "))
                    myAuthor3 = TrimAll(Replace(myAuthor3, ":", ", "))
                    If counter30 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_Author3.Focus()
                        Exit Sub
                    End If
                Else
                    myAuthor3 = ""
                End If

                'Server validation for  : txt_Cat_Editor
                Dim myEditor As Object = Nothing
                If txt_Cat_Editor.Text <> "" Then
                    myEditor = TrimAll(txt_Cat_Editor.Text)
                    myEditor = RemoveQuotes(myEditor)
                    If myEditor.Length > 500 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Editor.Focus()
                        Exit Sub
                    End If

                    myEditor = " " & myEditor & " "
                    If InStr(1, myEditor, "CREATE", 1) > 0 Or InStr(1, myEditor, "DELETE", 1) > 0 Or InStr(1, myEditor, "DROP", 1) > 0 Or InStr(1, myEditor, "INSERT", 1) > 1 Or InStr(1, myEditor, "TRACK", 1) > 1 Or InStr(1, myEditor, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_Editor.Focus()
                        Exit Sub
                    End If
                    myEditor = TrimAll(myEditor)
                    'check unwanted characters
                    c = 0
                    counter31 = 0
                    For iloop = 1 To Len(myEditor.ToString)
                        strcurrentchar = Mid(myEditor, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter31 = 1
                            End If
                        End If
                    Next
                    If counter31 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_Editor.Focus()
                        Exit Sub
                    End If
                Else
                    myEditor = ""
                End If

                'Server validation for  : txt_Cat_Editor
                Dim myTr As Object = Nothing
                If txt_Cat_Translator.Text <> "" Then
                    myTr = TrimAll(txt_Cat_Translator.Text)
                    myTr = RemoveQuotes(myTr)
                    If myTr.Length > 500 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Translator.Focus()
                        Exit Sub
                    End If

                    myTr = " " & myTr & " "
                    If InStr(1, myTr, "CREATE", 1) > 0 Or InStr(1, myTr, "DELETE", 1) > 0 Or InStr(1, myTr, "DROP", 1) > 0 Or InStr(1, myTr, "INSERT", 1) > 1 Or InStr(1, myTr, "TRACK", 1) > 1 Or InStr(1, myTr, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_Translator.Focus()
                        Exit Sub
                    End If
                    myTr = TrimAll(myTr)
                    'check unwanted characters
                    c = 0
                    counter32 = 0
                    For iloop = 1 To Len(myTr.ToString)
                        strcurrentchar = Mid(myTr, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter32 = 1
                            End If
                        End If
                    Next
                    If counter32 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_Translator.Focus()
                        Exit Sub
                    End If
                Else
                    myTr = ""
                End If

                'Server validation for  : txt_Cat_Illustrator
                Dim myIllus As Object = Nothing
                If txt_Cat_Illustrator.Text <> "" Then
                    myIllus = TrimAll(txt_Cat_Illustrator.Text)
                    myIllus = RemoveQuotes(myIllus)
                    If myIllus.Length > 500 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Illustrator.Focus()
                        Exit Sub
                    End If

                    myIllus = " " & myIllus & " "
                    If InStr(1, myIllus, "CREATE", 1) > 0 Or InStr(1, myIllus, "DELETE", 1) > 0 Or InStr(1, myIllus, "DROP", 1) > 0 Or InStr(1, myIllus, "INSERT", 1) > 1 Or InStr(1, myIllus, "TRACK", 1) > 1 Or InStr(1, myIllus, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_Illustrator.Focus()
                        Exit Sub
                    End If
                    myIllus = TrimAll(myIllus)
                    'check unwanted characters
                    c = 0
                    counter33 = 0
                    For iloop = 1 To Len(myIllus.ToString)
                        strcurrentchar = Mid(myIllus, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter33 = 1
                            End If
                        End If
                    Next
                    If counter33 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_Illustrator.Focus()
                        Exit Sub
                    End If
                Else
                    myIllus = ""
                End If

                'Server validation for  : txt_Cat_Compiler
                Dim myCompiler As Object = Nothing
                If txt_Cat_Compiler.Text <> "" Then
                    myCompiler = TrimAll(txt_Cat_Compiler.Text)
                    myCompiler = RemoveQuotes(myCompiler)
                    If myCompiler.Length > 500 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Compiler.Focus()
                        Exit Sub
                    End If

                    myCompiler = " " & myCompiler & " "
                    If InStr(1, myCompiler, "CREATE", 1) > 0 Or InStr(1, myCompiler, "DELETE", 1) > 0 Or InStr(1, myCompiler, "DROP", 1) > 0 Or InStr(1, myCompiler, "INSERT", 1) > 1 Or InStr(1, myCompiler, "TRACK", 1) > 1 Or InStr(1, myCompiler, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_Compiler.Focus()
                        Exit Sub
                    End If
                    myCompiler = TrimAll(myCompiler)
                    'check unwanted characters
                    c = 0
                    counter34 = 0
                    For iloop = 1 To Len(myCompiler.ToString)
                        strcurrentchar = Mid(myCompiler, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter34 = 1
                            End If
                        End If
                    Next
                    If counter34 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_Compiler.Focus()
                        Exit Sub
                    End If
                Else
                    myCompiler = ""
                End If

                'Server validation for  : txt_Cat_Commentator
                Dim myCommentator As Object = Nothing
                If txt_Cat_Commentator.Text <> "" Then
                    myCommentator = TrimAll(txt_Cat_Commentator.Text)
                    myCommentator = RemoveQuotes(myCommentator)
                    If myCommentator.Length > 500 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Commentator.Focus()
                        Exit Sub
                    End If

                    myCommentator = " " & myCommentator & " "
                    If InStr(1, myCommentator, "CREATE", 1) > 0 Or InStr(1, myCommentator, "DELETE", 1) > 0 Or InStr(1, myCommentator, "DROP", 1) > 0 Or InStr(1, myCommentator, "INSERT", 1) > 1 Or InStr(1, myCommentator, "TRACK", 1) > 1 Or InStr(1, myCommentator, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_Commentator.Focus()
                        Exit Sub
                    End If
                    myCommentator = TrimAll(myCommentator)
                    'check unwanted characters
                    c = 0
                    counter35 = 0
                    For iloop = 1 To Len(myCommentator.ToString)
                        strcurrentchar = Mid(myCommentator, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter35 = 1
                            End If
                        End If
                    Next
                    If counter35 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_Commentator.Focus()
                        Exit Sub
                    End If
                Else
                    myCommentator = ""
                End If

                'Server validation for  : txt_Cat_RevisedBy
                Dim myRevisedBy As Object = Nothing
                If txt_Cat_RevisedBy.Text <> "" Then
                    myRevisedBy = TrimAll(txt_Cat_RevisedBy.Text)
                    myRevisedBy = RemoveQuotes(myRevisedBy)
                    If myRevisedBy.Length > 255 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_RevisedBy.Focus()
                        Exit Sub
                    End If

                    myRevisedBy = " " & myRevisedBy & " "
                    If InStr(1, myRevisedBy, "CREATE", 1) > 0 Or InStr(1, myRevisedBy, "DELETE", 1) > 0 Or InStr(1, myRevisedBy, "DROP", 1) > 0 Or InStr(1, myRevisedBy, "INSERT", 1) > 1 Or InStr(1, myRevisedBy, "TRACK", 1) > 1 Or InStr(1, myRevisedBy, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_RevisedBy.Focus()
                        Exit Sub
                    End If
                    myRevisedBy = TrimAll(myRevisedBy)
                    'check unwanted characters
                    c = 0
                    counter36 = 0
                    For iloop = 1 To Len(myRevisedBy.ToString)
                        strcurrentchar = Mid(myRevisedBy, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter36 = 1
                            End If
                        End If
                    Next
                    If counter36 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_RevisedBy.Focus()
                        Exit Sub
                    End If
                Else
                    myRevisedBy = ""
                End If

                'Server validation for  : txt_Cat_CorpAuthor
                Dim myCorpAuthor As Object = Nothing
                If txt_Cat_CorpAuthor.Text <> "" Then
                    myCorpAuthor = TrimAll(txt_Cat_CorpAuthor.Text)
                    myCorpAuthor = RemoveQuotes(myCorpAuthor)
                    If myCorpAuthor.Length > 500 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_CorpAuthor.Focus()
                        Exit Sub
                    End If

                    myCorpAuthor = " " & myCorpAuthor & " "
                    If InStr(1, myCorpAuthor, "CREATE", 1) > 0 Or InStr(1, myCorpAuthor, "DELETE", 1) > 0 Or InStr(1, myCorpAuthor, "DROP", 1) > 0 Or InStr(1, myCorpAuthor, "INSERT", 1) > 1 Or InStr(1, myCorpAuthor, "TRACK", 1) > 1 Or InStr(1, myCorpAuthor, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_CorpAuthor.Focus()
                        Exit Sub
                    End If
                    myCorpAuthor = TrimAll(myCorpAuthor)
                    'check unwanted characters
                    c = 0
                    counter37 = 0
                    For iloop = 1 To Len(myCorpAuthor.ToString)
                        strcurrentchar = Mid(myCorpAuthor, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter37 = 1
                            End If
                        End If
                    Next
                    If counter37 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_CorpAuthor.Focus()
                        Exit Sub
                    End If
                Else
                    myCorpAuthor = ""
                End If

                'Server validation for  : txt_Cat_Edition
                Dim myEdition As Object = Nothing
                If txt_Cat_Edition.Text <> "" Then
                    myEdition = TrimAll(txt_Cat_Edition.Text)
                    myEdition = RemoveQuotes(myEdition)
                    myEdition = myEdition
                    If myEdition.Length > 250 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Edition.Focus()
                        Exit Sub
                    End If

                    myEdition = " " & myEdition & " "
                    If InStr(1, myEdition, "CREATE", 1) > 0 Or InStr(1, myEdition, "DELETE", 1) > 0 Or InStr(1, myEdition, "DROP", 1) > 0 Or InStr(1, myEdition, "INSERT", 1) > 1 Or InStr(1, myEdition, "TRACK", 1) > 1 Or InStr(1, myEdition, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_Edition.Focus()
                        Exit Sub
                    End If
                    myEdition = TrimAll(myEdition)
                    'check unwanted characters
                    c = 0
                    counter38 = 0
                    For iloop = 1 To Len(myEdition.ToString)
                        strcurrentchar = Mid(myEdition, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter38 = 1
                            End If
                        End If
                    Next
                    If counter38 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_Edition.Focus()
                        Exit Sub
                    End If
                    If InStr(myEdition, "edition") <> 0 Then
                        myEdition = LCase(TrimAll(Replace(myEdition, "edition", "")))
                    End If
                    If InStr(myEdition, "ed") <> 0 Then
                        myEdition = LCase(TrimAll(Replace(myEdition, "ed", "")))
                    End If
                    If InStr(myEdition, "ed.") <> 0 Then
                        myEdition = LCase(TrimAll(Replace(myEdition, "ed.", "")))
                    End If
                    If InStr(myEdition, "edition.") <> 0 Then
                        myEdition = LCase(TrimAll(Replace(myEdition, "edition.", "")))
                    End If
                Else
                    myEdition = ""
                End If


                'Server validation for  : txt_Cat_Reprint
                Dim myReprint As Object = Nothing
                If txt_Cat_Reprint.Text <> "" Then
                    myReprint = TrimAll(txt_Cat_Reprint.Text)
                    myReprint = RemoveQuotes(myReprint)
                    If myReprint.Length > 500 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Reprint.Focus()
                        Exit Sub
                    End If

                    myReprint = " " & myReprint & " "
                    If InStr(1, myReprint, "CREATE", 1) > 0 Or InStr(1, myReprint, "DELETE", 1) > 0 Or InStr(1, myReprint, "DROP", 1) > 0 Or InStr(1, myReprint, "INSERT", 1) > 1 Or InStr(1, myReprint, "TRACK", 1) > 1 Or InStr(1, myReprint, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_Reprint.Focus()
                        Exit Sub
                    End If
                    myReprint = TrimAll(myReprint)
                    'check unwanted characters
                    c = 0
                    counter39 = 0
                    For iloop = 1 To Len(myReprint.ToString)
                        strcurrentchar = Mid(myReprint, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter39 = 1
                            End If
                        End If
                    Next
                    If counter39 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_Reprint.Focus()
                        Exit Sub
                    End If
                Else
                    myReprint = ""
                End If

                'Server validation for  : txt_Cat_Place
                Dim myPubPlace As Object = Nothing
                If txt_Cat_Place.Text <> "" Then
                    myPubPlace = TrimAll(txt_Cat_Place.Text)
                    myPubPlace = RemoveQuotes(myPubPlace)
                    If myPubPlace.Length > 150 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Place.Focus()
                        Exit Sub
                    End If

                    myPubPlace = " " & myPubPlace & " "
                    If InStr(1, myPubPlace, "CREATE", 1) > 0 Or InStr(1, myPubPlace, "DELETE", 1) > 0 Or InStr(1, myPubPlace, "DROP", 1) > 0 Or InStr(1, myPubPlace, "INSERT", 1) > 1 Or InStr(1, myPubPlace, "TRACK", 1) > 1 Or InStr(1, myPubPlace, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_Place.Focus()
                        Exit Sub
                    End If
                    myPubPlace = TrimAll(myPubPlace)
                    'check unwanted characters
                    c = 0
                    counter41 = 0
                    For iloop = 1 To Len(myPubPlace.ToString)
                        strcurrentchar = Mid(myPubPlace, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter41 = 1
                            End If
                        End If
                    Next
                    If counter41 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_Place.Focus()
                        Exit Sub
                    End If
                Else
                    myPubPlace = ""
                End If

                'validation for Country Code
                Dim myConCode As Object = Nothing
                myConCode = DDL_Countries.SelectedValue
                If Not String.IsNullOrEmpty(myConCode) Then
                    myConCode = RemoveQuotes(myConCode)
                    If myConCode.Length > 4 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        DDL_Countries.Focus()
                        Exit Sub
                    End If

                    myConCode = " " & myConCode & " "
                    If InStr(1, myConCode, "CREATE", 1) > 0 Or InStr(1, myConCode, "DELETE", 1) > 0 Or InStr(1, myConCode, "DROP", 1) > 0 Or InStr(1, myConCode, "INSERT", 1) > 1 Or InStr(1, myConCode, "TRACK", 1) > 1 Or InStr(1, myConCode, "TRACE", 1) > 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                        DDL_Countries.Focus()
                        Exit Sub
                    End If
                    myConCode = TrimX(myConCode)
                    'check unwanted characters
                    c = 0
                    counter42 = 0
                    For iloop = 1 To Len(myConCode)
                        strcurrentchar = Mid(myConCode, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter42 = 1
                            End If
                        End If
                    Next
                    If counter42 = 1 Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                        DDL_Countries.Focus()
                        Exit Sub
                    End If
                Else
                    myConCode = ""
                End If

                'validation for Publisher
                Dim myPubID As Object = Nothing
                Dim PUB_NAME As Object = Nothing
                If Pub_ComboBox.Text <> "" Then
                    myPubID = Pub_ComboBox.SelectedValue

                    If IsNumeric(myPubID) = False Then
                        PUB_NAME = Trim(Pub_ComboBox.Text)
                        If PUB_NAME <> "" Then
                            PUB_NAME = TrimAll(Replace(PUB_NAME, ".", " "))
                            PUB_NAME = TrimAll(Replace(PUB_NAME, ",", ", "))
                            PUB_NAME = TrimAll(Replace(PUB_NAME, ";", ", "))
                        End If
                        Dim PubForm As New Publishers
                        'save new pub in database
                        PubForm.PUB_SAVE(PUB_NAME, myPubPlace, myConCode, LibCode)
                        Me.PopulatePub()
                        Pub_ComboBox.Items.FindByText(PUB_NAME).Selected = True
                    End If

                    myPubID = Pub_ComboBox.SelectedValue
                    If Not String.IsNullOrEmpty(myPubID) Then
                        myPubID = RemoveQuotes(myPubID)
                        If Not IsNumeric(myPubID) = True Then
                            Label6.Text = "Error: Input is not Valid !"
                            Label15.Text = ""
                            Pub_ComboBox.Focus()
                            Exit Sub
                        End If
                        myPubID = " " & myPubID & " "
                        If InStr(1, myPubID, "CREATE", 1) > 0 Or InStr(1, myPubID, "DELETE", 1) > 0 Or InStr(1, myPubID, "DROP", 1) > 0 Or InStr(1, myPubID, "INSERT", 1) > 1 Or InStr(1, myPubID, "TRACK", 1) > 1 Or InStr(1, myPubID, "TRACE", 1) > 1 Then
                            Label6.Text = "Error: Input is not Valid !"
                            Label15.Text = ""
                            Pub_ComboBox.Focus()
                            Exit Sub
                        End If
                        myPubID = TrimX(myPubID)
                        'check unwanted characters
                        c = 0
                        counter40 = 0
                        For iloop = 1 To Len(myPubID.ToString)
                            strcurrentchar = Mid(myPubID, iloop, 1)
                            If c = 0 Then
                                If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter40 = 1
                                End If
                            End If
                        Next
                        If counter40 = 1 Then
                            Label6.Text = "Error: Input is not Valid !"
                            Label15.Text = ""
                            Pub_ComboBox.Focus()
                            Exit Sub
                        End If
                    End If
                Else
                    myPubID = Nothing
                End If

                'Server validation for  : txt_Cat_Place
                Dim myPubYear As Integer = Nothing
                If TR_Year.Visible = True Then
                    If txt_Cat_Year.Text <> "" Then
                        myPubYear = TrimX(txt_Cat_Year.Text)
                        myPubYear = RemoveQuotes(myPubYear)
                        If Not Len(myPubYear.ToString) = 4 Then 'maximum length
                            Label6.Text = " Data must be of Proper Length.. "
                            Label15.Text = ""
                            txt_Cat_Year.Focus()
                            Exit Sub
                        End If

                        myPubYear = " " & myPubYear & " "
                        If InStr(1, myPubYear, "CREATE", 1) > 0 Or InStr(1, myPubYear, "DELETE", 1) > 0 Or InStr(1, myPubYear, "DROP", 1) > 0 Or InStr(1, myPubYear, "INSERT", 1) > 1 Or InStr(1, myPubYear, "TRACK", 1) > 1 Or InStr(1, myPubYear, "TRACE", 1) > 1 Then
                            Label6.Text = " Do Not use Reserve Words... "
                            Label15.Text = ""
                            Me.txt_Cat_Year.Focus()
                            Exit Sub
                        End If
                        myPubYear = TrimX(myPubYear)
                        'check unwanted characters
                        c = 0
                        counter43 = 0
                        For iloop = 1 To Len(myPubYear.ToString)
                            strcurrentchar = Mid(myPubYear, iloop, 1)
                            If c = 0 Then
                                If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter43 = 1
                                End If
                            End If
                        Next
                        If counter43 = 1 Then
                            Label6.Text = " Do Not use un-wanted Characters... "
                            Label15.Text = ""
                            Me.txt_Cat_Year.Focus()
                            Exit Sub
                        End If
                    Else
                        myPubYear = 0
                    End If
                Else
                    myPubYear = 0
                End If

                'Server validation for  : txt_Cat_Place
                Dim mySeries As Object = Nothing
                If txt_Cat_Series.Text <> "" Then
                    mySeries = TrimAll(txt_Cat_Series.Text)
                    mySeries = RemoveQuotes(mySeries)
                    If mySeries.Length > 500 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Series.Focus()
                        Exit Sub
                    End If

                    mySeries = " " & mySeries & " "
                    If InStr(1, mySeries, "CREATE", 1) > 0 Or InStr(1, mySeries, "DELETE", 1) > 0 Or InStr(1, mySeries, "DROP", 1) > 0 Or InStr(1, mySeries, "INSERT", 1) > 1 Or InStr(1, mySeries, "TRACK", 1) > 1 Or InStr(1, mySeries, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_Series.Focus()
                        Exit Sub
                    End If
                    mySeries = TrimAll(mySeries)
                    'check unwanted characters
                    c = 0
                    counter44 = 0
                    For iloop = 1 To Len(mySeries.ToString)
                        strcurrentchar = Mid(mySeries, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter44 = 1
                            End If
                        End If
                    Next
                    If counter44 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_Series.Focus()
                        Exit Sub
                    End If
                Else
                    mySeries = ""
                End If

                'Server validation for  : txt_Cat_SeriesEditor
                Dim mySeriesEditor As Object = Nothing
                If txt_Cat_SeriesEditor.Text <> "" Then
                    mySeriesEditor = TrimAll(txt_Cat_SeriesEditor.Text)
                    mySeriesEditor = RemoveQuotes(mySeriesEditor)
                    If mySeriesEditor.Length > 500 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_SeriesEditor.Focus()
                        Exit Sub
                    End If

                    mySeriesEditor = " " & mySeriesEditor & " "
                    If InStr(1, mySeriesEditor, "CREATE", 1) > 0 Or InStr(1, mySeriesEditor, "DELETE", 1) > 0 Or InStr(1, mySeriesEditor, "DROP", 1) > 0 Or InStr(1, mySeriesEditor, "INSERT", 1) > 1 Or InStr(1, mySeriesEditor, "TRACK", 1) > 1 Or InStr(1, mySeriesEditor, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_SeriesEditor.Focus()
                        Exit Sub
                    End If
                    mySeriesEditor = TrimAll(mySeriesEditor)
                    'check unwanted characters
                    c = 0
                    counter45 = 0
                    For iloop = 1 To Len(mySeriesEditor.ToString)
                        strcurrentchar = Mid(mySeriesEditor, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter45 = 1
                            End If
                        End If
                    Next
                    If counter45 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_SeriesEditor.Focus()
                        Exit Sub
                    End If
                Else
                    mySeriesEditor = ""
                End If


                'Server validation for  : txt_Cat_Note
                Dim myNote As Object = Nothing
                If txt_Cat_Note.Text <> "" Then
                    myNote = TrimAll(txt_Cat_Note.Text)
                    myNote = RemoveQuotes(myNote)
                    If myNote.Length > 500 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Note.Focus()
                        Exit Sub
                    End If

                    myNote = " " & myNote & " "
                    If InStr(1, myNote, "CREATE", 1) > 0 Or InStr(1, myNote, "DELETE", 1) > 0 Or InStr(1, myNote, "DROP", 1) > 0 Or InStr(1, myNote, "INSERT", 1) > 1 Or InStr(1, myNote, "TRACK", 1) > 1 Or InStr(1, myNote, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_Note.Focus()
                        Exit Sub
                    End If
                    myNote = TrimAll(myNote)
                    'check unwanted characters
                    c = 0
                    counter46 = 0
                    For iloop = 1 To Len(myNote.ToString)
                        strcurrentchar = Mid(myNote, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter46 = 1
                            End If
                        End If
                    Next
                    If counter46 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_Note.Focus()
                        Exit Sub
                    End If
                Else
                    myNote = ""
                End If

                'Server validation for  : txt_Cat_Remarks
                Dim myRemarks As Object = Nothing
                If txt_Cat_Remarks.Text <> "" Then
                    myRemarks = TrimAll(txt_Cat_Remarks.Text)
                    myRemarks = RemoveQuotes(myRemarks)
                    If myRemarks.Length > 500 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Remarks.Focus()
                        Exit Sub
                    End If

                    myRemarks = " " & myRemarks & " "
                    If InStr(1, myRemarks, "CREATE", 1) > 0 Or InStr(1, myRemarks, "DELETE", 1) > 0 Or InStr(1, myRemarks, "DROP", 1) > 0 Or InStr(1, myRemarks, "INSERT", 1) > 1 Or InStr(1, myRemarks, "TRACK", 1) > 1 Or InStr(1, myRemarks, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_Remarks.Focus()
                        Exit Sub
                    End If
                    myRemarks = TrimAll(myRemarks)
                    'check unwanted characters
                    c = 0
                    counter47 = 0
                    For iloop = 1 To Len(myRemarks.ToString)
                        strcurrentchar = Mid(myRemarks, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter47 = 1
                            End If
                        End If
                    Next
                    If counter47 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_Remarks.Focus()
                        Exit Sub
                    End If
                Else
                    myRemarks = ""
                End If

                'Server validation for  : txt_Cat_URL
                Dim myURL As Object = Nothing
                If txt_Cat_URL.Text <> "" Then
                    myURL = TrimAll(txt_Cat_URL.Text)
                    myURL = RemoveQuotes(myURL)
                    If myURL.Length > 500 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_URL.Focus()
                        Exit Sub
                    End If

                    myURL = " " & myURL & " "
                    If InStr(1, myURL, "CREATE", 1) > 0 Or InStr(1, myURL, "DELETE", 1) > 0 Or InStr(1, myURL, "DROP", 1) > 0 Or InStr(1, myURL, "INSERT", 1) > 1 Or InStr(1, myURL, "TRACK", 1) > 1 Or InStr(1, myURL, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_URL.Focus()
                        Exit Sub
                    End If
                    myURL = TrimAll(myURL)
                    'check unwanted characters
                    c = 0
                    counter48 = 0
                    For iloop = 1 To Len(myURL.ToString)
                        strcurrentchar = Mid(myURL, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter48 = 1
                            End If
                        End If
                    Next
                    If Not InStr(myURL, "http://") <> 0 Then
                        myURL = "http://" & myURL
                    End If
                    If counter48 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_URL.Focus()
                        Exit Sub
                    End If
                Else
                    myURL = ""
                End If

                'Server validation for  : txt_Cat_Remarks
                Dim myComments As Object = Nothing
                If txt_Cat_Comments.Text <> "" Then
                    myComments = TrimAll(txt_Cat_Comments.Text)
                    myComments = RemoveQuotes(myComments)
                    If myComments.Length > 500 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_URL.Focus()
                        Exit Sub
                    End If
                    myComments = " " & myComments & " "
                    If InStr(1, myComments, "CREATE", 1) > 0 Or InStr(1, myComments, "DELETE", 1) > 0 Or InStr(1, myComments, "DROP", 1) > 0 Or InStr(1, myComments, "INSERT", 1) > 1 Or InStr(1, myComments, "TRACK", 1) > 1 Or InStr(1, myComments, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_URL.Focus()
                        Exit Sub
                    End If
                    myComments = TrimAll(myComments)
                    'check unwanted characters
                    c = 0
                    counter49 = 0
                    For iloop = 1 To Len(myComments.ToString)
                        strcurrentchar = Mid(myComments, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter49 = 1
                            End If
                        End If
                    Next
                    If counter49 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_Comments.Focus()
                        Exit Sub
                    End If
                Else
                    myComments = ""
                End If

                'validation for DDL_Subjects
                Dim mySubId As Integer = Nothing
                If DDL_Subjects.Text <> "" Then
                    mySubId = DDL_Subjects.SelectedValue
                    If Not String.IsNullOrEmpty(mySubId) Then
                        mySubId = RemoveQuotes(mySubId)
                        If Len(mySubId) > 10 Then 'maximum length
                            Label6.Text = " Data must be of Proper Length.. "
                            Label15.Text = ""
                            DDL_Subjects.Focus()
                            Exit Sub
                        End If

                        If Not IsNumeric(mySubId) = True Then 'maximum length
                            Label6.Text = " Data must be Numeric Only.. "
                            Label15.Text = ""
                            DDL_Subjects.Focus()
                            Exit Sub
                        End If

                        mySubId = " " & mySubId & " "
                        If InStr(1, mySubId, "CREATE", 1) > 0 Or InStr(1, mySubId, "DELETE", 1) > 0 Or InStr(1, mySubId, "DROP", 1) > 0 Or InStr(1, mySubId, "INSERT", 1) > 1 Or InStr(1, mySubId, "TRACK", 1) > 1 Or InStr(1, mySubId, "TRACE", 1) > 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input is not Valid... ');", True)
                            DDL_Subjects.Focus()
                            Exit Sub
                        End If
                        mySubId = TrimX(mySubId)
                        'check unwanted characters
                        c = 0
                        counter50 = 0
                        For iloop = 1 To Len(mySubId.ToString)
                            strcurrentchar = Mid(mySubId, iloop, 1)
                            If c = 0 Then
                                If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter50 = 1
                                End If
                            End If
                        Next
                        If counter50 = 1 Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Input  is not Valid... ');", True)
                            DDL_Subjects.Focus()
                            Exit Sub
                        End If
                    Else
                        mySubId = Nothing
                    End If
                Else
                    mySubId = Nothing
                End If

                'Server validation for  : txt_Cat_Keywords
                Dim myKeywords As Object = Nothing
                If txt_Cat_Keywords.Text <> "" Then
                    myKeywords = TrimAll(txt_Cat_Keywords.Text)
                    myKeywords = RemoveQuotes(myKeywords)
                    If myKeywords.Length > 1000 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Keywords.Focus()
                        Exit Sub
                    End If
                    myKeywords = " " & myKeywords & " "
                    If InStr(1, myKeywords, "CREATE", 1) > 0 Or InStr(1, myKeywords, "DELETE", 1) > 0 Or InStr(1, myKeywords, "DROP", 1) > 0 Or InStr(1, myKeywords, "INSERT", 1) > 1 Or InStr(1, myKeywords, "TRACK", 1) > 1 Or InStr(1, myKeywords, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_Keywords.Focus()
                        Exit Sub
                    End If
                    myKeywords = TrimAll(myKeywords)
                    'check unwanted characters
                    c = 0
                    counter51 = 0
                    For iloop = 1 To Len(myKeywords.ToString)
                        strcurrentchar = Mid(myKeywords, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter51 = 1
                            End If
                        End If
                    Next
                    If counter51 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_Keywords.Focus()
                        Exit Sub
                    End If
                    myKeywords = UCase(myKeywords)
                Else
                    myKeywords = ""
                End If

                'Server validation for  : txt_Cat_TrFrom
                Dim myTrFrom As Object = Nothing
                If txt_Cat_TrFrom.Text <> "" Then
                    myTrFrom = TrimAll(txt_Cat_TrFrom.Text)
                    myTrFrom = RemoveQuotes(myTrFrom)
                    If myTrFrom.Length > 500 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_TrFrom.Focus()
                        Exit Sub
                    End If
                    myTrFrom = " " & myTrFrom & " "
                    If InStr(1, myTrFrom, "CREATE", 1) > 0 Or InStr(1, myTrFrom, "DELETE", 1) > 0 Or InStr(1, myTrFrom, "DROP", 1) > 0 Or InStr(1, myTrFrom, "INSERT", 1) > 1 Or InStr(1, myTrFrom, "TRACK", 1) > 1 Or InStr(1, myTrFrom, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_TrFrom.Focus()
                        Exit Sub
                    End If
                    myTrFrom = TrimAll(myTrFrom)
                    'check unwanted characters
                    c = 0
                    counter52 = 0
                    For iloop = 1 To Len(myTrFrom.ToString)
                        strcurrentchar = Mid(myTrFrom, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter52 = 1
                            End If
                        End If
                    Next
                    If counter52 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_TrFrom.Focus()
                        Exit Sub
                    End If
                Else
                    myTrFrom = ""
                End If

                'Server validation for  : txt_Cat_Abstract
                Dim myAbstract As Object = Nothing
                If txt_Cat_Abstract.Text <> "" Then
                    myAbstract = TrimAll(txt_Cat_Abstract.Text)
                    myAbstract = RemoveQuotes(myAbstract)
                    If myAbstract.Length > 4000 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Abstract.Focus()
                        Exit Sub
                    End If
                    myAbstract = " " & myAbstract & " "
                    If InStr(1, myAbstract, "CREATE", 1) > 0 Or InStr(1, myAbstract, "DELETE", 1) > 0 Or InStr(1, myAbstract, "DROP", 1) > 0 Or InStr(1, myAbstract, "INSERT", 1) > 1 Or InStr(1, myAbstract, "TRACK", 1) > 1 Or InStr(1, myAbstract, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_Abstract.Focus()
                        Exit Sub
                    End If
                    myAbstract = TrimAll(myAbstract)
                    'check unwanted characters
                    c = 0
                    counter53 = 0
                    For iloop = 1 To Len(myAbstract.ToString)
                        strcurrentchar = Mid(myAbstract, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter53 = 1
                            End If
                        End If
                    Next
                    If counter53 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_Abstract.Focus()
                        Exit Sub
                    End If
                Else
                    myAbstract = ""
                End If

                'Server validation for  : txt_Cat_ReferenceNo
                Dim myRefNo As Object = Nothing
                If txt_Cat_ReferenceNo.Text <> "" Then
                    myRefNo = TrimAll(txt_Cat_ReferenceNo.Text)
                    myRefNo = RemoveQuotes(myRefNo)
                    If myRefNo.Length > 500 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_ReferenceNo.Focus()
                        Exit Sub
                    End If
                    myRefNo = " " & myRefNo & " "
                    If InStr(1, myRefNo, "CREATE", 1) > 0 Or InStr(1, myRefNo, "DELETE", 1) > 0 Or InStr(1, myRefNo, "DROP", 1) > 0 Or InStr(1, myRefNo, "INSERT", 1) > 1 Or InStr(1, myRefNo, "TRACK", 1) > 1 Or InStr(1, myRefNo, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_ReferenceNo.Focus()
                        Exit Sub
                    End If
                    myRefNo = TrimAll(myRefNo)
                    'check unwanted characters
                    c = 0
                    counter54 = 0
                    For iloop = 1 To Len(myRefNo.ToString)
                        strcurrentchar = Mid(myRefNo, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter54 = 1
                            End If
                        End If
                    Next
                    If counter1 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_ReferenceNo.Focus()
                        Exit Sub
                    End If
                Else
                    myRefNo = ""
                End If

                'Server validation for  : SP Re-Affirmation year
                Dim myReaffirmYear As Integer = Nothing
                If txt_Cat_ReaffirmYear.Text <> "" Then
                    myReaffirmYear = TrimX(txt_Cat_ReaffirmYear.Text)
                    myReaffirmYear = RemoveQuotes(myReaffirmYear)
                    If Not Len(myReaffirmYear.ToString) = 4 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_ReaffirmYear.Focus()
                        Exit Sub
                    End If

                    myReaffirmYear = " " & myReaffirmYear & " "
                    If InStr(1, myReaffirmYear, "CREATE", 1) > 0 Or InStr(1, myReaffirmYear, "DELETE", 1) > 0 Or InStr(1, myReaffirmYear, "DROP", 1) > 0 Or InStr(1, myReaffirmYear, "INSERT", 1) > 1 Or InStr(1, myReaffirmYear, "TRACK", 1) > 1 Or InStr(1, myReaffirmYear, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_ReaffirmYear.Focus()
                        Exit Sub
                    End If
                    myReaffirmYear = TrimX(myReaffirmYear)
                    'check unwanted characters
                    c = 0
                    counter55 = 0
                    For iloop = 1 To Len(myReaffirmYear.ToString)
                        strcurrentchar = Mid(myReaffirmYear, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter55 = 1
                            End If
                        End If
                    Next
                    If counter55 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_ReaffirmYear.Focus()
                        Exit Sub
                    End If
                Else
                    myReaffirmYear = 0
                End If

                'Server validation for  : SP Re-txt_Cat_WithdrawYear
                Dim myWithdrawYear As Integer = Nothing
                If txt_Cat_WithdrawYear.Text <> "" Then
                    myWithdrawYear = TrimX(txt_Cat_WithdrawYear.Text)
                    myWithdrawYear = RemoveQuotes(myWithdrawYear)
                    If Not Len(myWithdrawYear.ToString) = 4 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_WithdrawYear.Focus()
                        Exit Sub
                    End If

                    myWithdrawYear = " " & myWithdrawYear & " "
                    If InStr(1, myWithdrawYear, "CREATE", 1) > 0 Or InStr(1, myWithdrawYear, "DELETE", 1) > 0 Or InStr(1, myWithdrawYear, "DROP", 1) > 0 Or InStr(1, myWithdrawYear, "INSERT", 1) > 1 Or InStr(1, myWithdrawYear, "TRACK", 1) > 1 Or InStr(1, myWithdrawYear, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_WithdrawYear.Focus()
                        Exit Sub
                    End If
                    myWithdrawYear = TrimX(myWithdrawYear)
                    'check unwanted characters
                    c = 0
                    counter56 = 0
                    For iloop = 1 To Len(myWithdrawYear.ToString)
                        strcurrentchar = Mid(myWithdrawYear, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter56 = 1
                            End If
                        End If
                    Next
                    If counter56 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_WithdrawYear.Focus()
                        Exit Sub
                    End If
                Else
                    myWithdrawYear = 0
                End If


                'Server validation for  : SP Technical Committee
                Dim mySPCommittee As Object = Nothing
                If txt_Cat_SPTCSC.Text <> "" Then
                    mySPCommittee = TrimAll(txt_Cat_SPTCSC.Text)
                    mySPCommittee = RemoveQuotes(mySPCommittee)
                    If mySPCommittee.Length > 250 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_SPTCSC.Focus()
                        Exit Sub
                    End If
                    mySPCommittee = " " & mySPCommittee & " "
                    If InStr(1, mySPCommittee, "CREATE", 1) > 0 Or InStr(1, mySPCommittee, "DELETE", 1) > 0 Or InStr(1, mySPCommittee, "DROP", 1) > 0 Or InStr(1, mySPCommittee, "INSERT", 1) > 1 Or InStr(1, mySPCommittee, "TRACK", 1) > 1 Or InStr(1, mySPCommittee, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_SPTCSC.Focus()
                        Exit Sub
                    End If
                    mySPCommittee = TrimAll(mySPCommittee)
                    'check unwanted characters
                    c = 0
                    counter57 = 0
                    For iloop = 1 To Len(mySPCommittee.ToString)
                        strcurrentchar = Mid(mySPCommittee, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter57 = 1
                            End If
                        End If
                    Next
                    If counter57 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_SPTCSC.Focus()
                        Exit Sub
                    End If
                Else
                    mySPCommittee = ""
                End If

                'Server validation for  : txt_Cat_SPUpdates
                Dim mySPUpdates As Object = Nothing
                If txt_Cat_SPUpdates.Text <> "" Then
                    mySPUpdates = TrimAll(txt_Cat_SPUpdates.Text)
                    mySPUpdates = RemoveQuotes(mySPUpdates)
                    If mySPUpdates.Length > 250 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_SPUpdates.Focus()
                        Exit Sub
                    End If
                    mySPUpdates = " " & mySPUpdates & " "
                    If InStr(1, mySPUpdates, "CREATE", 1) > 0 Or InStr(1, mySPUpdates, "DELETE", 1) > 0 Or InStr(1, mySPUpdates, "DROP", 1) > 0 Or InStr(1, mySPUpdates, "INSERT", 1) > 1 Or InStr(1, mySPUpdates, "TRACK", 1) > 1 Or InStr(1, mySPUpdates, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_SPUpdates.Focus()
                        Exit Sub
                    End If
                    mySPUpdates = TrimAll(mySPUpdates)
                    'check unwanted characters
                    c = 0
                    counter58 = 0
                    For iloop = 1 To Len(mySPUpdates.ToString)
                        strcurrentchar = Mid(mySPUpdates, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter58 = 1
                            End If
                        End If
                    Next
                    If counter58 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_SPUpdates.Focus()
                        Exit Sub
                    End If
                Else
                    mySPUpdates = ""
                End If

                'Server validation for  : txt_Cat_SPAmmendments
                Dim mySPAmmendments As Object = Nothing
                If txt_Cat_SPAmmendments.Text <> "" Then
                    mySPAmmendments = TrimAll(txt_Cat_SPAmmendments.Text)
                    mySPAmmendments = RemoveQuotes(mySPAmmendments)
                    If mySPAmmendments.Length > 250 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_SPAmmendments.Focus()
                        Exit Sub
                    End If
                    mySPAmmendments = " " & mySPAmmendments & " "
                    If InStr(1, mySPAmmendments, "CREATE", 1) > 0 Or InStr(1, mySPAmmendments, "DELETE", 1) > 0 Or InStr(1, mySPAmmendments, "DROP", 1) > 0 Or InStr(1, mySPAmmendments, "INSERT", 1) > 1 Or InStr(1, mySPAmmendments, "TRACK", 1) > 1 Or InStr(1, mySPAmmendments, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_SPAmmendments.Focus()
                        Exit Sub
                    End If
                    mySPAmmendments = TrimAll(mySPAmmendments)
                    'check unwanted characters
                    c = 0
                    counter59 = 0
                    For iloop = 1 To Len(mySPAmmendments.ToString)
                        strcurrentchar = Mid(mySPAmmendments, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter59 = 1
                            End If
                        End If
                    Next
                    If counter59 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_SPAmmendments.Focus()
                        Exit Sub
                    End If
                Else
                    mySPAmmendments = ""
                End If

                'Server validation for  : txt_Cat_SPAmmendments
                Dim mySPIssueBody As Object = Nothing
                If txt_Cat_SPIssueBody.Text <> "" Then
                    mySPIssueBody = TrimAll(txt_Cat_SPIssueBody.Text)
                    mySPIssueBody = RemoveQuotes(mySPIssueBody)
                    If mySPAmmendments.Length > 350 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_SPIssueBody.Focus()
                        Exit Sub
                    End If
                    mySPIssueBody = " " & mySPIssueBody & " "
                    If InStr(1, mySPIssueBody, "CREATE", 1) > 0 Or InStr(1, mySPIssueBody, "DELETE", 1) > 0 Or InStr(1, mySPIssueBody, "DROP", 1) > 0 Or InStr(1, mySPIssueBody, "INSERT", 1) > 1 Or InStr(1, mySPIssueBody, "TRACK", 1) > 1 Or InStr(1, mySPIssueBody, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_SPIssueBody.Focus()
                        Exit Sub
                    End If
                    mySPIssueBody = TrimAll(mySPIssueBody)
                    'check unwanted characters
                    c = 0
                    counter60 = 0
                    For iloop = 1 To Len(mySPIssueBody.ToString)
                        strcurrentchar = Mid(mySPIssueBody, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter60 = 1
                            End If
                        End If
                    Next
                    If counter60 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_SPIssueBody.Focus()
                        Exit Sub
                    End If
                Else
                    mySPIssueBody = ""
                End If

                'Server validation for  : txt_Cat_Producer            
                Dim myProducer As Object = Nothing
                If txt_Cat_Producer.Text <> "" Then
                    myProducer = TrimAll(txt_Cat_Producer.Text)
                    myProducer = RemoveQuotes(myProducer)
                    If myProducer.Length > 250 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Producer.Focus()
                        Exit Sub
                    End If
                Else
                    myProducer = ""
                End If

                'Server validation for  : txt_Cat_Designer            
                Dim myDesigner As Object = Nothing
                If txt_Cat_Designer.Text <> "" Then
                    myDesigner = TrimAll(txt_Cat_Designer.Text)
                    myDesigner = RemoveQuotes(myDesigner)
                    If myDesigner.Length > 150 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Designer.Focus()
                        Exit Sub
                    End If
                Else
                    myDesigner = ""
                End If

                'Server validation for  : txt_Cat_Manufacturer						            
                Dim myManufacturer As Object = Nothing
                If txt_Cat_Manufacturer.Text <> "" Then
                    myManufacturer = TrimAll(txt_Cat_Manufacturer.Text)
                    myManufacturer = RemoveQuotes(myManufacturer)
                    If myManufacturer.Length > 250 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Manufacturer.Focus()
                        Exit Sub
                    End If
                Else
                    myManufacturer = ""
                End If

                'Server validation for  : txt_Cat_Creater									            
                Dim myCreator As Object = Nothing
                If txt_Cat_Creater.Text <> "" Then
                    myCreator = TrimAll(txt_Cat_Creater.Text)
                    myCreator = RemoveQuotes(myCreator)
                    If myCreator.Length > 150 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Creater.Focus()
                        Exit Sub
                    End If
                Else
                    myCreator = ""
                End If

                'Server validation for  : txt_Cat_Materials											            
                Dim myMaterials As Object = Nothing
                If txt_Cat_Materials.Text <> "" Then
                    myMaterials = TrimAll(txt_Cat_Materials.Text)
                    myMaterials = RemoveQuotes(myMaterials)
                    If myMaterials.Length > 50 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Materials.Focus()
                        Exit Sub
                    End If
                Else
                    myMaterials = ""
                End If


                'Server validation for  : txt_Cat_WrokCategory													            
                Dim myWorkCategory As Object = Nothing
                If txt_Cat_WrokCategory.Text <> "" Then
                    myWorkCategory = TrimAll(txt_Cat_WrokCategory.Text)
                    myWorkCategory = RemoveQuotes(myWorkCategory)
                    If myWorkCategory.Length > 50 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_WrokCategory.Focus()
                        Exit Sub
                    End If
                Else
                    myWorkCategory = ""
                End If

                'Server validation for  : txt_Cat_RelatedWork																			            
                Dim myRelatedWork As Object = Nothing
                If txt_Cat_RelatedWork.Text <> "" Then
                    myRelatedWork = TrimAll(txt_Cat_RelatedWork.Text)
                    myRelatedWork = RemoveQuotes(myRelatedWork)
                    If myRelatedWork.Length > 250 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_RelatedWork.Focus()
                        Exit Sub
                    End If
                Else
                    myRelatedWork = ""
                End If

                'ser rver validation for  : txt_Cat_Source																										            
                Dim mySource As Object = Nothing
                If txt_Cat_Source.Text <> "" Then
                    mySource = TrimAll(txt_Cat_Source.Text)
                    mySource = RemoveQuotes(mySource)
                    If mySource.Length > 250 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Source.Focus()
                        Exit Sub
                    End If
                Else
                    mySource = ""
                End If

                'ser rver validation for  : txt_Cat_Photographer																																            
                Dim myPhotographer As Object = Nothing
                If txt_Cat_Photographer.Text <> "" Then
                    myPhotographer = TrimAll(txt_Cat_Photographer.Text)
                    myPhotographer = RemoveQuotes(myPhotographer)
                    If myPhotographer.Length > 250 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Photographer.Focus()
                        Exit Sub
                    End If
                Else
                    myPhotographer = ""
                End If

                'ser rver validation for  : txt_Cat_Nationality																																	            
                Dim myNationality As Object = Nothing
                If txt_Cat_Nationality.Text <> "" Then
                    myNationality = TrimAll(txt_Cat_Nationality.Text)
                    myNationality = RemoveQuotes(myNationality)
                    If myNationality.Length > 50 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Nationality.Focus()
                        Exit Sub
                    End If
                Else
                    myNationality = ""
                End If

                'ser rver validation for  : txt_Cat_Techniq																																	            
                Dim myTechniq As Object = Nothing
                If txt_Cat_Techniq.Text <> "" Then
                    myTechniq = TrimAll(txt_Cat_Techniq.Text)
                    myTechniq = RemoveQuotes(myTechniq)
                    If myTechniq.Length > 150 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_Techniq.Focus()
                        Exit Sub
                    End If
                Else
                    myTechniq = ""
                End If

                'ser rver validation for  : txt_Cat_WorkType																																			            
                Dim myWorkType As Object = Nothing
                If txt_Cat_WorkType.Text <> "" Then
                    myWorkType = TrimAll(txt_Cat_WorkType.Text)
                    myWorkType = RemoveQuotes(myWorkType)
                    If myWorkType.Length > 50 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_WorkType.Focus()
                        Exit Sub
                    End If
                Else
                    myWorkType = ""
                End If

                'ser rver validation for  : txt_Cat_RoleofCreator																																					            
                Dim myRoleofCreator As Object = Nothing
                If txt_Cat_RoleofCreator.Text <> "" Then
                    myRoleofCreator = TrimAll(txt_Cat_RoleofCreator.Text)
                    myRoleofCreator = RemoveQuotes(myRoleofCreator)
                    If myRoleofCreator.Length > 50 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_RoleofCreator.Focus()
                        Exit Sub
                    End If
                Else
                    myRoleofCreator = ""
                End If

                'Server validation for  : SP Re-txt_Cat_ProductionYear		
                Dim myProductionYear As Integer = Nothing
                If txt_Cat_ProductionYear.Text <> "" Then
                    myProductionYear = TrimX(txt_Cat_ProductionYear.Text)
                    myProductionYear = RemoveQuotes(myProductionYear)
                    If Not Len(myProductionYear.ToString) = 4 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_ProductionYear.Focus()
                        Exit Sub
                    End If

                    myProductionYear = " " & myProductionYear & " "
                    If InStr(1, myProductionYear, "CREATE", 1) > 0 Or InStr(1, myProductionYear, "DELETE", 1) > 0 Or InStr(1, myProductionYear, "DROP", 1) > 0 Or InStr(1, myProductionYear, "INSERT", 1) > 1 Or InStr(1, myProductionYear, "TRACK", 1) > 1 Or InStr(1, myProductionYear, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_ProductionYear.Focus()
                        Exit Sub
                    End If
                    myProductionYear = TrimX(myProductionYear)
                    'check unwanted characters
                    c = 0
                    counter56 = 0
                    For iloop = 1 To Len(myProductionYear.ToString)
                        strcurrentchar = Mid(myProductionYear, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter56 = 1
                            End If
                        End If
                    Next
                    If counter56 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_ProductionYear.Focus()
                        Exit Sub
                    End If
                Else
                    myProductionYear = 0
                End If



                'validation for CHAIRMAN
                Dim CHAIRMAN As Object = Nothing
                If txt_Cat_Chairman.Text <> "" Then
                    CHAIRMAN = TrimAll(txt_Cat_Chairman.Text)
                    CHAIRMAN = RemoveQuotes(CHAIRMAN)
                    If CHAIRMAN.Length > 250 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Cat_Chairman.Focus()
                        Exit Sub
                    End If
                    CHAIRMAN = " " & CHAIRMAN & " "
                    If InStr(1, CHAIRMAN, "CREATE", 1) > 0 Or InStr(1, CHAIRMAN, "DELETE", 1) > 0 Or InStr(1, CHAIRMAN, "DROP", 1) > 0 Or InStr(1, CHAIRMAN, "INSERT", 1) > 1 Or InStr(1, CHAIRMAN, "TRACK", 1) > 1 Or InStr(1, CHAIRMAN, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Cat_Chairman.Focus()
                        Exit Sub
                    End If
                    CHAIRMAN = TrimAll(CHAIRMAN)
                    'check unwanted characters
                    c = 0
                    counter71 = 0
                    For iloop = 1 To Len(CHAIRMAN)
                        strcurrentchar = Mid(CHAIRMAN, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter71 = 1
                            End If
                        End If
                    Next
                    If counter71 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Cat_Chairman.Focus()
                        Exit Sub
                    End If
                Else
                    CHAIRMAN = ""
                End If


                'validation for GOVERNMENT
                Dim GOVERNMENT As Object = Nothing
                If DDL_Government.Text <> "" Then
                    GOVERNMENT = DDL_Government.SelectedValue
                    GOVERNMENT = RemoveQuotes(GOVERNMENT)
                    If GOVERNMENT.Length > 50 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_Government.Focus()
                        Exit Sub
                    End If
                    GOVERNMENT = " " & GOVERNMENT & " "
                    If InStr(1, GOVERNMENT, "CREATE", 1) > 0 Or InStr(1, GOVERNMENT, "DELETE", 1) > 0 Or InStr(1, GOVERNMENT, "DROP", 1) > 0 Or InStr(1, GOVERNMENT, "INSERT", 1) > 1 Or InStr(1, GOVERNMENT, "TRACK", 1) > 1 Or InStr(1, GOVERNMENT, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_Government.Focus()
                        Exit Sub
                    End If
                    GOVERNMENT = TrimAll(GOVERNMENT)
                    'check unwanted characters
                    c = 0
                    counter72 = 0
                    For iloop = 1 To Len(GOVERNMENT)
                        strcurrentchar = Mid(GOVERNMENT, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter72 = 1
                            End If
                        End If
                    Next
                    If counter72 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        DDL_Government.Focus()
                        Exit Sub
                    End If
                Else
                    GOVERNMENT = ""
                End If


                'validation for ACT_NO
                Dim ACT_NO As Object = Nothing
                If txt_Cat_ActNo.Text <> "" Then
                    ACT_NO = TrimAll(txt_Cat_ActNo.Text)
                    ACT_NO = RemoveQuotes(ACT_NO)
                    If ACT_NO.Length > 150 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Cat_ActNo.Focus()
                        Exit Sub
                    End If
                    ACT_NO = " " & ACT_NO & " "
                    If InStr(1, ACT_NO, "CREATE", 1) > 0 Or InStr(1, ACT_NO, "DELETE", 1) > 0 Or InStr(1, ACT_NO, "DROP", 1) > 0 Or InStr(1, ACT_NO, "INSERT", 1) > 1 Or InStr(1, ACT_NO, "TRACK", 1) > 1 Or InStr(1, ACT_NO, "TRACE", 1) > 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Cat_ActNo.Focus()
                        Exit Sub
                    End If
                    ACT_NO = TrimAll(ACT_NO)
                    'check unwanted characters
                    c = 0
                    counter73 = 0
                    For iloop = 1 To Len(ACT_NO)
                        strcurrentchar = Mid(ACT_NO, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter73 = 1
                            End If
                        End If
                    Next
                    If counter73 = 1 Then
                        Label6.Text = "Error: Input is not Valid !"
                        Label15.Text = ""
                        txt_Cat_ActNo.Focus()
                        Exit Sub
                    End If
                Else
                    ACT_NO = ""
                End If

                'Server validation for  : ACT_YEAR		
                Dim ACT_YEAR As Integer = Nothing
                If txt_Cat_ActYear.Text <> "" Then
                    ACT_YEAR = TrimX(txt_Cat_ActYear.Text)
                    ACT_YEAR = RemoveQuotes(ACT_YEAR)
                    If Not Len(ACT_YEAR.ToString) = 4 Then 'maximum length
                        Label6.Text = " Data must be of Proper Length.. "
                        Label15.Text = ""
                        txt_Cat_ActYear.Focus()
                        Exit Sub
                    End If

                    ACT_YEAR = " " & ACT_YEAR & " "
                    If InStr(1, ACT_YEAR, "CREATE", 1) > 0 Or InStr(1, ACT_YEAR, "DELETE", 1) > 0 Or InStr(1, ACT_YEAR, "DROP", 1) > 0 Or InStr(1, ACT_YEAR, "INSERT", 1) > 1 Or InStr(1, ACT_YEAR, "TRACK", 1) > 1 Or InStr(1, ACT_YEAR, "TRACE", 1) > 1 Then
                        Label6.Text = " Do Not use Reserve Words... "
                        Label15.Text = ""
                        Me.txt_Cat_ActYear.Focus()
                        Exit Sub
                    End If
                    ACT_YEAR = TrimX(ACT_YEAR)
                    'check unwanted characters
                    c = 0
                    counter56 = 0
                    For iloop = 1 To Len(ACT_YEAR.ToString)
                        strcurrentchar = Mid(ACT_YEAR, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter56 = 1
                            End If
                        End If
                    Next
                    If counter56 = 1 Then
                        Label6.Text = " Do Not use un-wanted Characters... "
                        Label15.Text = ""
                        Me.txt_Cat_ActYear.Focus()
                        Exit Sub
                    End If
                Else
                    ACT_YEAR = 0
                End If




                'upload library photo
                Dim arrContent2 As Byte()
                Dim intLength2Photo As Integer = 0
                If FileUpload1.FileName = "" Then
                    '    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Select Proper file... ');", True)
                    '    Me.FileUpload12.Focus()
                    '    Exit Sub
                Else
                    Dim fileName As String = FileUpload1.PostedFile.FileName
                    Dim ext As String = fileName.Substring(fileName.LastIndexOf("."))
                    ext = ext.ToLower
                    Dim imgType = FileUpload1.PostedFile.ContentType

                    If ext = ".jpg" Then

                    ElseIf ext = ".bmp" Then

                    ElseIf ext = ".gif" Then

                    ElseIf ext = "jpg" Then

                    ElseIf ext = "bmp" Then

                    ElseIf ext = "gif" Then

                    Else
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Select Only gif, bmp, or jpg format files supported... ');", True)
                        Me.FileUpload1.Focus()
                        Exit Sub
                    End If
                    intLength2Photo = Convert.ToInt32(FileUpload1.PostedFile.InputStream.Length)
                    ReDim arrContent2(intLength2Photo)
                    FileUpload1.PostedFile.InputStream.Read(arrContent2, 0, intLength2Photo)

                    If intLength2Photo > 6000 Then
                        Label6.Text = "Error: Photo Size is Bigger than 6 KB"
                        Label15.Text = ""
                        Exit Sub
                    End If
                    Image1.ImageUrl = FileUpload1.PostedFile.FileName '"~/Images/1.png"
                End If



                'upload content file
                Dim arrContent3 As Byte()
                Dim FileType As String = Nothing
                Dim FileExtension As String = Nothing
                Dim intLength3 As Integer = 0
                If FileUpload2.FileName = "" Then
                    '    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Select Proper file... ');", True)
                    '    Me.FileUpload12.Focus()
                    '    Exit Sub
                Else
                    Dim ContfileName As String = FileUpload2.PostedFile.FileName
                    FileExtension = ContfileName.Substring(ContfileName.LastIndexOf("."))
                    FileExtension = FileExtension.ToLower
                    FileType = FileUpload2.PostedFile.ContentType

                    intLength3 = Convert.ToInt32(FileUpload2.PostedFile.InputStream.Length)
                    ReDim arrContent3(intLength3)
                    FileUpload2.PostedFile.InputStream.Read(arrContent3, 0, intLength3)

                    If intLength3 > 90000 Then
                        Label6.Text = "Error: Photo Size is Bigger than 90 KB"
                        Label15.Text = ""
                        Exit Sub
                    End If
                End If





                Dim myUserCode As Object = Nothing
                myUserCode = Session.Item("LoggedUser")

                Dim myDateModified As Object = Nothing
                myDateModified = Now.Date
                myDateModified = Convert.ToDateTime(myDateModified, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                Dim myIP As Object = Nothing
                myIP = Request.UserHostAddress.Trim

                'check title for duplicacy
                Dim str3 As Object = Nothing
                Dim flag3 As Object = Nothing
                If myAuthor1 = String.Empty And myAuthor2 = String.Empty And myAuthor3 = String.Empty Then
                    str3 = "select CAT_NO from CATS_AUTHORS_VIEW where (TITLE= N'" & myTitle & "') "
                ElseIf myAuthor1 <> String.Empty And myAuthor2 = String.Empty And myAuthor3 = String.Empty Then
                    str3 = "select CAT_NO from CATS_AUTHORS_VIEW where (TITLE = N'" & myTitle & "' and AUTHOR1='" & myAuthor1 & "')"
                ElseIf myAuthor1 <> String.Empty And myAuthor2 <> String.Empty And myAuthor3 = String.Empty Then
                    str3 = "select CAT_NO from CATS_AUTHORS_VIEW where (TITLE = N'" & myTitle & "' and AUTHOR1='" & myAuthor1 & "' and AUTHOR2='" & myAuthor2 & "')"
                ElseIf myAuthor1 <> String.Empty And myAuthor2 <> String.Empty And myAuthor3 <> String.Empty Then
                    str3 = "select CAT_NO from CATS_AUTHORS_VIEW where (TITLE = N'" & myTitle & "' and AUTHOR1='" & myAuthor1 & "' and AUTHOR2='" & myAuthor2 & "' and AUTHOR3='" & myAuthor3 & "')"
                End If
                If myEdition <> String.Empty Then
                    str3 = str3 & " AND (EDITION= N'" & myEdition & "') "
                End If

                Dim PUBNAMEX As Object = Nothing
                If Pub_ComboBox.Text <> "" Then
                    PUBNAMEX = Pub_ComboBox.SelectedItem.ToString
                Else
                    PUBNAMEX = ""
                End If
                If PUBNAMEX <> String.Empty Then
                    str3 = str3 & " AND (PUB_NAME= N'" & PUBNAMEX & "') "
                End If
                If myPubYear <> 0 Then
                    str3 = str3 & " AND (YEAR_OF_PUB= '" & myPubYear & "') "
                End If

                str3 = str3 & " AND (CAT_NO <> '" & Trim(Label7.Text) & "')"


                Dim cmd3 As New SqlCommand(str3, SqlConn)
                SqlConn.Open()
                flag3 = cmd3.ExecuteScalar
                SqlConn.Close()
                If flag3 <> Nothing Then
                    Label6.Text = "This TITLE Already Exists ! "
                    Label15.Text = ""
                    Me.txt_Cat_ISBN.Focus()
                    Exit Sub
                End If

                '****************************************************************************************************
                'UPDATE THE LIBRARY PROFILE  
                If Label7.Text <> "" Then
                    SQL = "SELECT * FROM CATS WHERE (CAT_NO='" & Trim(Label7.Text) & "')"
                    If SqlConn.State = 0 Then
                        SqlConn.Open()
                    End If
                    da = New SqlDataAdapter(SQL, SqlConn)
                    cb = New SqlCommandBuilder(da)
                    da.Fill(ds, "CATS")
                    If ds.Tables("CATS").Rows.Count <> 0 Then
                        With ds.Tables("CATS")
                            If Not String.IsNullOrEmpty(myLangCode) Then
                                .Rows(0)("LANG_CODE") = myLangCode.Trim
                            Else
                                .Rows(0)("LANG_CODE") = "ENG"
                            End If

                            If Not String.IsNullOrEmpty(myBibLevel) Then
                                .Rows(0)("BIB_CODE") = myBibLevel.Trim
                            Else
                                .Rows(0)("BIB_CODE") = "M"
                            End If
                            If Not String.IsNullOrEmpty(myMatType) Then
                                .Rows(0)("MAT_CODE") = myMatType.Trim
                            Else
                                .Rows(0)("MAT_CODE") = "B"
                            End If
                            If Not String.IsNullOrEmpty(myDocType) Then
                                .Rows(0)("DOC_TYPE_CODE") = myDocType.Trim
                            Else
                                .Rows(0)("DOC_TYPE_CODE") = "BK"
                            End If
                            If Not String.IsNullOrEmpty(myISBN) Then
                                .Rows(0)("STANDARD_NO") = myISBN.Trim
                            Else
                                .Rows(0)("STANDARD_NO") = System.DBNull.Value
                            End If
                            If Not String.IsNullOrEmpty(myTitle) Then
                                .Rows(0)("TITLE") = myTitle.Trim
                            Else
                                .Rows(0)("TITLE") = "No Title"
                            End If
                            If Not String.IsNullOrEmpty(mySubTitle) Then
                                .Rows(0)("SUB_TITLE") = mySubTitle.Trim
                            Else
                                .Rows(0)("SUB_TITLE") = System.DBNull.Value
                            End If
                            If Not String.IsNullOrEmpty(myVarTitle) Then
                                .Rows(0)("VAR_TITLE") = myVarTitle.Trim
                            Else
                                .Rows(0)("VAR_TITLE") = System.DBNull.Value
                            End If
                            If Not String.IsNullOrEmpty(myAuthor1) Then
                                .Rows(0)("AUTHOR1") = myAuthor1.Trim
                            Else
                                .Rows(0)("AUTHOR1") = System.DBNull.Value
                            End If
                            If Not String.IsNullOrEmpty(myAuthor2) Then
                                .Rows(0)("AUTHOR2") = myAuthor2.Trim
                            Else
                                .Rows(0)("AUTHOR2") = System.DBNull.Value
                            End If
                            If Not String.IsNullOrEmpty(myAuthor3) Then
                                .Rows(0)("AUTHOR3") = myAuthor3.Trim
                            Else
                                .Rows(0)("AUTHOR3") = System.DBNull.Value
                            End If
                            If Not String.IsNullOrEmpty(myCorpAuthor) Then
                                .Rows(0)("CORPORATE_AUTHOR") = myCorpAuthor.Trim
                            Else
                                .Rows(0)("CORPORATE_AUTHOR") = System.DBNull.Value
                            End If
                            If Not String.IsNullOrEmpty(myEditor) Then
                                .Rows(0)("EDITOR") = myEditor.Trim
                            Else
                                .Rows(0)("EDITOR") = System.DBNull.Value
                            End If
                            If Not String.IsNullOrEmpty(myTr) Then
                                .Rows(0)("TRANSLATOR") = myTr.Trim
                            Else
                                .Rows(0)("TRANSLATOR") = System.DBNull.Value
                            End If
                            If Not String.IsNullOrEmpty(myIllus) Then
                                .Rows(0)("ILLUSTRATOR") = myIllus.Trim
                            Else
                                .Rows(0)("ILLUSTRATOR") = System.DBNull.Value
                            End If
                            If Not String.IsNullOrEmpty(myCompiler) Then
                                .Rows(0)("COMPILER") = myCompiler.Trim
                            Else
                                .Rows(0)("COMPILER") = System.DBNull.Value
                            End If
                            If Not String.IsNullOrEmpty(myEdition) Then
                                .Rows(0)("EDITION") = myEdition.Trim
                            Else
                                .Rows(0)("EDITION") = System.DBNull.Value
                            End If
                            If Not String.IsNullOrEmpty(myPubPlace) Then
                                .Rows(0)("PLACE_OF_PUB") = myPubPlace.Trim
                            Else
                                .Rows(0)("PLACE_OF_PUB") = System.DBNull.Value
                            End If
                            If Not String.IsNullOrEmpty(myPubID) Or myPubID <> 0 Then
                                .Rows(0)("PUB_ID") = myPubID
                            Else
                                .Rows(0)("PUB_ID") = System.DBNull.Value
                            End If
                            If myPubYear = 0 Then
                                .Rows(0)("YEAR_OF_PUB") = System.DBNull.Value
                            Else
                                .Rows(0)("YEAR_OF_PUB") = myPubYear
                            End If
                            If Not String.IsNullOrEmpty(mySeries) Then
                                .Rows(0)("SERIES_TITLE") = mySeries.Trim
                            Else
                                .Rows(0)("SERIES_TITLE") = System.DBNull.Value
                            End If
                            If Not String.IsNullOrEmpty(mySeriesEditor) Then
                                .Rows(0)("SERIES_EDITOR") = mySeriesEditor.Trim
                            Else
                                .Rows(0)("SERIES_EDITOR") = System.DBNull.Value
                            End If
                            If Not String.IsNullOrEmpty(myNote) Then
                                .Rows(0)("NOTE") = myNote.Trim
                            Else
                                .Rows(0)("NOTE") = System.DBNull.Value
                            End If
                            If Not String.IsNullOrEmpty(myRemarks) Then
                                .Rows(0)("REMARKS") = myRemarks.Trim
                            Else
                                .Rows(0)("REMARKS") = System.DBNull.Value
                            End If
                            If mySubId = 0 Then
                                .Rows(0)("SUB_ID") = System.DBNull.Value
                            Else
                                If Not String.IsNullOrEmpty(mySubId) Then 'Or mySubId <> 0 Then
                                    .Rows(0)("SUB_ID") = mySubId
                                Else
                                    .Rows(0)("SUB_ID") = System.DBNull.Value
                                End If
                            End If

                            If Not String.IsNullOrEmpty(myKeywords) Then
                                .Rows(0)("KEYWORDS") = myKeywords
                            Else
                                .Rows(0)("KEYWORDS") = System.DBNull.Value
                            End If
                            If Not String.IsNullOrEmpty(myAbstract) Then
                                .Rows(0)("ABSTRACT") = myAbstract.Trim
                            Else
                                .Rows(0)("ABSTRACT") = System.DBNull.Value
                            End If
                            If Not String.IsNullOrEmpty(myYesNo) Then
                                .Rows(0)("MULTI_VOL") = myYesNo.Trim
                            Else
                                .Rows(0)("MULTI_VOL") = "N"
                            End If

                            If TotalVol <> 0 Then
                                .Rows(0)("TOTAL_VOL") = TotalVol
                            Else
                                .Rows(0)("TOTAL_VOL") = System.DBNull.Value
                            End If
                            If Not String.IsNullOrEmpty(mySPNo) Then
                                .Rows(0)("SP_NO") = mySPNo.Trim
                            Else
                                .Rows(0)("SP_NO") = System.DBNull.Value
                            End If
                            If Not String.IsNullOrEmpty(mySPRev) Then
                                .Rows(0)("SP_VERSION") = mySPRev.Trim
                            Else
                                .Rows(0)("SP_VERSION") = System.DBNull.Value
                            End If
                            If Not String.IsNullOrEmpty(myReportNo) Then
                                .Rows(0)("REPORT_NO") = myReportNo.Trim
                            Else
                                .Rows(0)("REPORT_NO") = System.DBNull.Value
                            End If
                            If Not String.IsNullOrEmpty(myManualNo) Then
                                .Rows(0)("MANUAL_NO") = myManualNo.Trim
                            Else
                                .Rows(0)("MANUAL_NO") = System.DBNull.Value
                            End If
                            If Not String.IsNullOrEmpty(myManualVer) Then
                                .Rows(0)("MANUAL_VER") = myManualVer.Trim
                            Else
                                .Rows(0)("MANUAL_VER") = System.DBNull.Value
                            End If
                            If Not String.IsNullOrEmpty(myRefNo) Then
                                .Rows(0)("REFERENCE_NO") = myRefNo.Trim
                            Else
                                .Rows(0)("REFERENCE_NO") = System.DBNull.Value
                            End If
                            If Not String.IsNullOrEmpty(myURL) Then
                                .Rows(0)("URL") = myURL.Trim
                            Else
                                .Rows(0)("URL") = System.DBNull.Value
                            End If
                            If Not String.IsNullOrEmpty(myPatentInventor) Then
                                .Rows(0)("PATENT_INVENTOR") = myPatentInventor.Trim
                            Else
                                .Rows(0)("PATENT_INVENTOR") = System.DBNull.Value
                            End If
                            If Not String.IsNullOrEmpty(myPatentee) Then
                                .Rows(0)("PATENTEE") = myPatentee.Trim
                            Else
                                .Rows(0)("PATENTEE") = System.DBNull.Value
                            End If
                            If Not String.IsNullOrEmpty(myPatentNo) Then
                                .Rows(0)("PATENT_NO") = myPatentNo.Trim
                            Else
                                .Rows(0)("PATENT_NO") = System.DBNull.Value
                            End If
                            If Not String.IsNullOrEmpty(myReprint) Then
                                .Rows(0)("REPRINTS") = myReprint.Trim
                            Else
                                .Rows(0)("REPRINTS") = System.DBNull.Value
                            End If
                            If Not String.IsNullOrEmpty(myConCode) Then
                                .Rows(0)("CON_CODE") = myConCode.Trim
                            Else
                                .Rows(0)("CON_CODE") = System.DBNull.Value
                            End If
                            If Not String.IsNullOrEmpty(myConfName) Then
                                .Rows(0)("CONF_NAME") = myConfName.Trim
                            Else
                                .Rows(0)("CONF_NAME") = System.DBNull.Value
                            End If
                            If Not String.IsNullOrEmpty(myConfSDate) Then
                                .Rows(0)("CONF_FROM") = myConfSDate.Trim
                            Else
                                .Rows(0)("CONF_FROM") = System.DBNull.Value
                            End If
                            If Not String.IsNullOrEmpty(myConfEDate) Then
                                .Rows(0)("CONF_TO") = myConfEDate.Trim
                            Else
                                .Rows(0)("CONF_TO") = System.DBNull.Value
                            End If
                            If Not String.IsNullOrEmpty(myConfPlace) Then
                                .Rows(0)("CONF_PLACE") = myConfPlace.Trim
                            Else
                                .Rows(0)("CONF_PLACE") = System.DBNull.Value
                            End If
                            If Not String.IsNullOrEmpty(myTrFrom) Then
                                .Rows(0)("TR_FROM") = myTrFrom.Trim
                            Else
                                .Rows(0)("TR_FROM") = System.DBNull.Value
                            End If
                            If Not String.IsNullOrEmpty(myRevisedBy) Then
                                .Rows(0)("REVISED_BY") = myRevisedBy.Trim
                            Else
                                .Rows(0)("REVISED_BY") = System.DBNull.Value
                            End If
                            If Not String.IsNullOrEmpty(myCommentator) Then
                                .Rows(0)("COMMENTATORS") = myCommentator.Trim
                            Else
                                .Rows(0)("COMMENTATORS") = System.DBNull.Value
                            End If
                            If Not String.IsNullOrEmpty(myScholarName) Then
                                .Rows(0)("SCHOLAR_NAME") = myScholarName.Trim
                            Else
                                .Rows(0)("SCHOLAR_NAME") = System.DBNull.Value
                            End If
                            If Not String.IsNullOrEmpty(myScholarDept) Then
                                .Rows(0)("SCHOLAR_DEPT") = myScholarDept.Trim
                            Else
                                .Rows(0)("SCHOLAR_DEPT") = System.DBNull.Value
                            End If
                            If Not String.IsNullOrEmpty(myGuideName) Then
                                .Rows(0)("GUIDE_NAME") = myGuideName.Trim
                            Else
                                .Rows(0)("GUIDE_NAME") = System.DBNull.Value
                            End If
                            If Not String.IsNullOrEmpty(myGuideDept) Then
                                .Rows(0)("GUIDE_DEPT") = myGuideDept.Trim
                            Else
                                .Rows(0)("GUIDE_DEPT") = System.DBNull.Value
                            End If
                            If Not String.IsNullOrEmpty(myDegreeName) Then
                                .Rows(0)("DEGREE_NAME") = myDegreeName.Trim
                            Else
                                .Rows(0)("DEGREE_NAME") = System.DBNull.Value
                            End If
                            If Not String.IsNullOrEmpty(myComments) Then
                                .Rows(0)("COMMENTS") = myComments.Trim
                            Else
                                .Rows(0)("COMMENTS") = System.DBNull.Value
                            End If

                            If myReaffirmYear = 0 Then
                                .Rows(0)("SP_REAFFIRM_YEAR") = System.DBNull.Value
                            Else
                                If Not String.IsNullOrEmpty(myReaffirmYear) Then
                                    .Rows(0)("SP_REAFFIRM_YEAR") = myReaffirmYear
                                Else
                                    .Rows(0)("SP_REAFFIRM_YEAR") = System.DBNull.Value
                                End If
                            End If
                            If Not String.IsNullOrEmpty(mySPCommittee) Then
                                .Rows(0)("SP_TCSC") = mySPCommittee.Trim
                            Else
                                .Rows(0)("SP_TCSC") = System.DBNull.Value
                            End If
                            If Not String.IsNullOrEmpty(mySPUpdates) Then
                                .Rows(0)("SP_UPDATES") = mySPUpdates.Trim
                            Else
                                .Rows(0)("SP_UPDATES") = System.DBNull.Value
                            End If

                            If myWithdrawYear = 0 Then
                                .Rows(0)("SP_WITHDRAW_YEAR") = System.DBNull.Value
                            Else
                                If Not String.IsNullOrEmpty(myWithdrawYear) Then
                                    .Rows(0)("SP_WITHDRAW_YEAR") = myWithdrawYear
                                Else
                                    .Rows(0)("SP_WITHDRAW_YEAR") = System.DBNull.Value
                                End If
                            End If

                            If Not String.IsNullOrEmpty(mySPAmmendments) Then
                                .Rows(0)("SP_AMMENDMENTS") = mySPAmmendments.Trim
                            Else
                                .Rows(0)("SP_AMMENDMENTS") = System.DBNull.Value
                            End If

                            If Not String.IsNullOrEmpty(mySPIssueBody) Then
                                .Rows(0)("SP_ISSUE_BODY") = mySPIssueBody.Trim
                            Else
                                .Rows(0)("SP_ISSUE_BODY") = System.DBNull.Value
                            End If

                            If Not String.IsNullOrEmpty(myProducer) Then
                                .Rows(0)("PRODUCER") = myProducer.Trim
                            Else
                                .Rows(0)("PRODUCER") = System.DBNull.Value
                            End If

                            If Not String.IsNullOrEmpty(myDesigner) Then
                                .Rows(0)("DESIGNER") = myDesigner.Trim
                            Else
                                .Rows(0)("DESIGNER") = System.DBNull.Value
                            End If

                            If Not String.IsNullOrEmpty(myManufacturer) Then
                                .Rows(0)("MANUFACTURER") = myManufacturer.Trim
                            Else
                                .Rows(0)("MANUFACTURER") = System.DBNull.Value
                            End If

                            If Not String.IsNullOrEmpty(myCreator) Then
                                .Rows(0)("CREATOR") = myCreator.Trim
                            Else
                                .Rows(0)("CREATOR") = System.DBNull.Value
                            End If

                            If Not String.IsNullOrEmpty(myMaterials) Then
                                .Rows(0)("MATERIALS") = myMaterials.Trim
                            Else
                                .Rows(0)("MATERIALS") = System.DBNull.Value
                            End If

                            If Not String.IsNullOrEmpty(myWorkCategory) Then
                                .Rows(0)("WORK_CATEGORY") = myWorkCategory.Trim
                            Else
                                .Rows(0)("WORK_CATEGORY") = System.DBNull.Value
                            End If

                            If Not String.IsNullOrEmpty(myRelatedWork) Then
                                .Rows(0)("RELATED_WORK") = myRelatedWork.Trim
                            Else
                                .Rows(0)("RELATED_WORK") = System.DBNull.Value
                            End If

                            If Not String.IsNullOrEmpty(mySource) Then
                                .Rows(0)("SOURCE") = mySource.Trim
                            Else
                                .Rows(0)("SOURCE") = System.DBNull.Value
                            End If

                            If Not String.IsNullOrEmpty(myPhotographer) Then
                                .Rows(0)("PHOTOGRAPHER") = myPhotographer.Trim
                            Else
                                .Rows(0)("PHOTOGRAPHER") = System.DBNull.Value
                            End If

                            If Not String.IsNullOrEmpty(myNationality) Then
                                .Rows(0)("NATIONALITY") = myNationality.Trim
                            Else
                                .Rows(0)("NATIONALITY") = System.DBNull.Value
                            End If

                            If Not String.IsNullOrEmpty(myTechniq) Then
                                .Rows(0)("TECHNIQ") = myTechniq.Trim
                            Else
                                .Rows(0)("TECHNIQ") = System.DBNull.Value
                            End If

                            If Not String.IsNullOrEmpty(myWorkType) Then
                                .Rows(0)("WORK_TYPE") = myWorkType.Trim
                            Else
                                .Rows(0)("WORK_TYPE") = System.DBNull.Value
                            End If

                            If Not String.IsNullOrEmpty(myRoleofCreator) Then
                                .Rows(0)("ROLE_OF_CREATOR") = myRoleofCreator.Trim
                            Else
                                .Rows(0)("ROLE_OF_CREATOR") = System.DBNull.Value
                            End If

                            If myProductionYear = 0 Then
                                .Rows(0)("PRODUCTION_YEAR") = System.DBNull.Value
                            Else
                                If Not String.IsNullOrEmpty(myProductionYear) Then
                                    .Rows(0)("PRODUCTION_YEAR") = myProductionYear
                                Else
                                    .Rows(0)("PRODUCTION_YEAR") = System.DBNull.Value
                                End If
                            End If

                            If ACT_YEAR = 0 Then
                                .Rows(0)("ACT_YEAR") = System.DBNull.Value
                            Else
                                If Not String.IsNullOrEmpty(ACT_YEAR) Then
                                    .Rows(0)("ACT_YEAR") = ACT_YEAR
                                Else
                                    .Rows(0)("ACT_YEAR") = System.DBNull.Value
                                End If
                            End If

                            If Not String.IsNullOrEmpty(CHAIRMAN) Then
                                .Rows(0)("CHAIRMAN") = CHAIRMAN.Trim
                            Else
                                .Rows(0)("CHAIRMAN") = System.DBNull.Value
                            End If

                            If Not String.IsNullOrEmpty(ACT_NO) Then
                                .Rows(0)("ACT_NO") = ACT_NO.Trim
                            Else
                                .Rows(0)("ACT_NO") = System.DBNull.Value
                            End If

                            If Not String.IsNullOrEmpty(GOVERNMENT) Then
                                .Rows(0)("GOVERNMENT") = GOVERNMENT.Trim
                            Else
                                .Rows(0)("GOVERNMENT") = System.DBNull.Value
                            End If




                            If FileUpload1.FileName <> "" Then
                                .Rows(0)("PHOTO") = arrContent2
                            Else
                                If CheckBox1.Checked = True Then
                                    .Rows(0)("PHOTO") = System.DBNull.Value
                                End If
                            End If


                            If FileUpload2.FileName <> "" Then
                                .Rows(0)("CONT_FILE") = arrContent3
                            Else
                                If CheckBox3.Checked = True Then
                                    .Rows(0)("CONT_FILE") = System.DBNull.Value
                                End If
                            End If

                            If Not String.IsNullOrEmpty(FileExtension) Then
                                .Rows(0)("CONT_FILE_TYPE") = FileExtension.Trim
                            Else
                                .Rows(0)("CONT_FILE_TYPE") = System.DBNull.Value
                            End If


                            .Rows(0)("LIB_CODE") = LibCode
                            .Rows(0)("UPDATED_BY") = Session.Item("LoggedUser")
                            .Rows(0)("DATE_MODIFIED") = Now.Date
                            .Rows(0)("IP") = Request.UserHostAddress.Trim
                        End With
                        thisTransaction = SqlConn.BeginTransaction()
                        da.SelectCommand.Transaction = thisTransaction
                        da.Update(ds, "CATS")
                        thisTransaction.Commit()

                        Label15.Text = "Record Updated Successfully"
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Record Updated Successfully... ');", True)
                        Label6.Text = ""
                        ClearFields()
                    Else
                        Label6.Text = "Record Not Updated  - Please Contact System Administrator... "
                        Label15.Text = ""
                        Exit Sub
                    End If
                End If
            Else
                'record not selected
                Label6.Text = "Record Not Selected..."
                Label15.Text = ""
            End If
                SqlConn.Close()
                ClearFields()
                UpdatePanel1.Update()
                Label7.Text = ""
                Me.Acq_Save_Bttn.Visible = True
                Me.Acq_Update_Bttn.Visible = False
                Me.Acq_Delete_Bttn.Visible = False
                Acq_SaveNew_Bttn.Visible = False
        Catch q As SqlException
            thisTransaction.Rollback()
            Label6.Text = "Database Error -SAVE: " & (q.Message() & ", " & q.Number & ", " & q.GetType.ToString)
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'delete record    
    Protected Sub Acq_Delete_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Acq_Delete_Bttn.Click
        Try
            If Label7.Text <> "" Then
                Dim myDelCat As Object = Nothing
                myDelCat = Trim(Label7.Text)

                'check cat record in acq and holdigns table
                Dim str As Object = Nothing
                Dim flag As Object = Nothing
                str = "SELECT CAT_NO FROM ACQUISITIONS WHERE (CAT_NO ='" & Trim(myDelCat) & "') "
                Dim cmd As New SqlCommand(str, SqlConn)
                If SqlConn.State = 0 Then
                    SqlConn.Open()
                End If
                flag = cmd.ExecuteScalar
                If flag <> Nothing Then
                    Label6.Text = "Alert: Catalog Record Reference Exists in ACQUISITION Table thus can not be deleted!"
                    Label15.Text = ""
                    SqlConn.Close()
                    Exit Sub
                End If
                SqlConn.Close()

                'check in holding table
                Dim str1 As Object = Nothing
                Dim flag1 As Object = Nothing
                str1 = "SELECT CAT_NO FROM HOLDINGS WHERE (CAT_NO ='" & Trim(myDelCat) & "') "
                Dim cmd1 As New SqlCommand(str1, SqlConn)
                If SqlConn.State = 0 Then
                    SqlConn.Open()
                End If
                flag1 = cmd1.ExecuteScalar
                If flag1 <> Nothing Then
                    Label6.Text = "Alert: Catalog Record Reference Exists in HOLDIGNS Table thus can not be deleted!"
                    Label15.Text = ""
                    SqlConn.Close()
                    Exit Sub
                End If
                SqlConn.Close()

                'check in ARTICLES table
                Dim str3 As Object = Nothing
                Dim flag3 As Object = Nothing
                str3 = "SELECT CAT_NO FROM ARTICLES WHERE (CAT_NO ='" & Trim(myDelCat) & "') "
                Dim cmd3 As New SqlCommand(str3, SqlConn)
                If SqlConn.State = 0 Then
                    SqlConn.Open()
                End If
                flag3 = cmd3.ExecuteScalar
                If flag3 <> Nothing Then
                    Label6.Text = "Alert: Catalog Record Reference Exists in ARTICLES Table thus can not be deleted!"
                    Label15.Text = ""
                    SqlConn.Close()
                    Exit Sub
                End If
                SqlConn.Close()


                'delete Record
                Dim SQL As String = Nothing
                SQL = "DELETE FROM CATS WHERE (CAT_NO ='" & Trim(myDelCat) & "') "
                Dim objCommand As New SqlCommand(SQL, SqlConn)
                Dim da As New SqlDataAdapter(objCommand)
                Dim ds As New DataSet
                da.Fill(ds)
                Label15.Text = "Catalog Record deleted Successfully!"
                Label6.Text = ""
                ClearFields()
                Me.Acq_Save_Bttn.Visible = True
                Me.Acq_Update_Bttn.Visible = False
                Me.Acq_Delete_Bttn.Visible = False
                Acq_SaveNew_Bttn.Visible = False
            End If
        Catch q As SqlException
            Label6.Text = "Database Error -SAVE: " & (q.Message() & ", " & q.Number & ", " & q.GetType.ToString)
            Label15.Text = ""
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
            Label15.Text = ""
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'delete selected rows
    Protected Sub Delete_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Delete_Bttn.Click
        Try
            For i = 0 To Grid1_Search.Rows.Count - 1
                If DirectCast(Grid1_Search.Rows(i).Cells(0).FindControl("cbd"), CheckBox).Checked Then
                    Dim CatNo As Integer = Nothing
                    CatNo = Grid1_Search.Rows(i).Cells(3).Text
                    'check cat record in acq and holdigns table
                    Dim str As Object = Nothing
                    Dim flag As Object = Nothing
                    str = "SELECT CAT_NO FROM ACQUISITIONS WHERE (CAT_NO ='" & Trim(CatNo) & "') "
                    Dim cmd1 As New SqlCommand(str, SqlConn)
                    If SqlConn.State = 0 Then
                        SqlConn.Open()
                    End If
                    flag = cmd1.ExecuteScalar
                    If flag <> Nothing Then
                        Label6.Text = "Alert: Catalog Record Reference Exists in ACQUISITION Table thus can not be deleted!"
                        Label15.Text = ""
                        SqlConn.Close()
                        Exit Sub
                    End If
                    SqlConn.Close()
                    'check in holding table
                    Dim str1 As Object = Nothing
                    Dim flag1 As Object = Nothing
                    str1 = "SELECT CAT_NO FROM HOLDINGS WHERE (CAT_NO ='" & Trim(CatNo) & "') "
                    Dim cmd2 As New SqlCommand(str1, SqlConn)
                    If SqlConn.State = 0 Then
                        SqlConn.Open()
                    End If
                    flag1 = cmd2.ExecuteScalar
                    If flag <> Nothing Then
                        Label6.Text = "Alert: Catalog Record Reference Exists in HOLDIGNS Table thus can not be deleted!"
                        Label15.Text = ""
                        SqlConn.Close()
                        Exit Sub
                    End If
                    SqlConn.Close()
                    If flag <> Nothing Or flag1 <> Nothing Then
                        Label6.Text = "This Record can not be deleted)"
                    Else
                        ' Dim COM_ID As Integer = Convert.ToInt32(Grid_Pub.Rows(i).Cells(6).Text)
                        'get cat record
                        Dim SQL As String = Nothing
                        SQL = "DELETE FROM CATS WHERE (CAT_NO ='" & Trim(CatNo) & "') "
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
            'UpdatePanel1.Update()
        Catch q As SqlException
            Label6.Text = "Database Error -SAVE: " & (q.Message() & ", " & q.Number & ", " & q.GetType.ToString)
            Label15.Text = ""
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
            Label15.Text = ""
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'delete cover photo from datbase in cat record
    Private Sub Delete_Photo_Bttn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Delete_Photo_Bttn.Click
        Dim SQL As String = Nothing
        Dim da As SqlDataAdapter
        Dim cb As SqlCommandBuilder
        Dim ds As New DataSet
        Dim dt As New DataTable
        Dim thisTransaction As SqlClient.SqlTransaction
        If SqlConn.State = 0 Then
            SqlConn.Open()
        End If
        Try
            For i = 0 To Grid1_Search.Rows.Count - 1
                If DirectCast(Grid1_Search.Rows(i).Cells(0).FindControl("cbd"), CheckBox).Checked Then
                    Dim CatNo As Integer = Nothing
                    CatNo = Grid1_Search.Rows(i).Cells(3).Text

                    'UPDATE THE LIBRARY PROFILE  
                    If CatNo <> 0 Then
                        thisTransaction = SqlConn.BeginTransaction()
                        Dim intValue As Integer = 0
                        Dim objCommand As New SqlCommand
                        objCommand.Connection = SqlConn
                        objCommand.Transaction = thisTransaction
                        objCommand.CommandType = CommandType.Text
                        objCommand.CommandText = "UPDATE CATS SET DATE_MODIFIED =@DateModified, UPDATED_BY=@myLibCode, IP=@myIP, PHOTO=@myPhoto WHERE CAT_NO = @myCatNo"

                        objCommand.Parameters.Add("@myCatNo", SqlDbType.Int)
                        objCommand.Parameters("@myCatNo").Value = CatNo

                        objCommand.Parameters.Add("@DateModified", SqlDbType.DateTime)
                        objCommand.Parameters("@DateModified").Value = Now.Date

                        objCommand.Parameters.Add("@myLibCode", SqlDbType.NVarChar)
                        objCommand.Parameters("@myLibCode").Value = LibCode

                        objCommand.Parameters.Add("@myIP", SqlDbType.NVarChar)
                        objCommand.Parameters("@myIP").Value = Request.UserHostAddress.Trim

                        objCommand.Parameters.Add("@myPhoto", SqlDbType.Image)
                        objCommand.Parameters("@myPhoto").Value = System.DBNull.Value

                        objCommand.ExecuteNonQuery()

                        thisTransaction.Commit()
                        Label6.Text = ""
                        Label15.Text = "Cover Photo Deleted from Database!"
                        DirectCast(Grid1_Search.Rows(i).Cells(0).FindControl("cbd"), CheckBox).Checked = False
                    End If
                End If
            Next
        Catch q As SqlException
            thisTransaction.Rollback()
            Label6.Text = "Database Error -SAVE: " & (q.Message() & ", " & q.Number & ", " & q.GetType.ToString)
            Label15.Text = ""
        Catch ex As Exception
            Label6.Text = "Error: " & (ex.Message())
            Label15.Text = ""
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'autocomplete method for title
    <System.Web.Script.Services.ScriptMethod(), _
  System.Web.Services.WebMethod()> _
    Public Shared Function SearchTitle(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Return EG4.PopulateCombo.SearchTitle(prefixText, count)
    End Function
    'autocomplete method for author
    <System.Web.Script.Services.ScriptMethod(), _
 System.Web.Services.WebMethod()> _
    Public Shared Function SearchAuthor(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Return EG4.PopulateCombo.SearchAuthor(prefixText, count)
    End Function
    <System.Web.Script.Services.ScriptMethod(), _
System.Web.Services.WebMethod()> _
    Public Shared Function SearchSubTitle(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Return EG4.PopulateCombo.SearchSubTitle(prefixText, count)
    End Function
    <System.Web.Script.Services.ScriptMethod(), _
System.Web.Services.WebMethod()> _
    Public Shared Function SearchConfName(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Return EG4.PopulateCombo.SearchConfName(prefixText, count)
    End Function
    <System.Web.Script.Services.ScriptMethod(), _
System.Web.Services.WebMethod()> _
    Public Shared Function SearchEdition(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Return EG4.PopulateCombo.SearchEdition(prefixText, count)
    End Function
    <System.Web.Script.Services.ScriptMethod(), _
System.Web.Services.WebMethod()> _
    Public Shared Function SearchPlace(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Return EG4.PopulateCombo.SearchPlace(prefixText, count)
    End Function
    <System.Web.Script.Services.ScriptMethod(), _
System.Web.Services.WebMethod()> _
    Public Shared Function SearchYear(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Return EG4.PopulateCombo.SearchYear(prefixText, count)
    End Function
    
    <System.Web.Script.Services.ScriptMethod(), _
System.Web.Services.WebMethod()> _
    Public Shared Function SearchSeries(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Return EG4.PopulateCombo.SearchSeries(prefixText, count)
    End Function

    'save as new record  
    Protected Sub Acq_SaveNew_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Acq_SaveNew_Bttn.Click
        Me.Acq_Save_Bttn_Click(sender, e)
    End Sub
    Dim Find As Integer = 0
    Public Sub DownloadRecord_ISBN()
        Try
            Find = 0
            Dim MYISBN As Object = Nothing
            If ComboBox2.Text <> "" Then
                Dim URLstr As String
                If ComboBox2.Text = "All" Then
                    Dim q As Integer
                    q = ComboBox2.Items.Count
                    q = q - 1
                    While q <> -1
                        Dim str11 As String
                        ComboBox2.SelectedIndex = q
                        str11 = ComboBox2.Text
                        If str11 <> "All" Then
                            If str11 = "http://catalog.loc.gov" Then
                                URLstr = str11 + "/cgi-bin/Pwebrecon.cgi?DB=local&Search_Arg=" & Replace(txt_Cat_ISBN.Text, "-", "") & "&Search_Code=GKEY%5E*&CNT=100&hist=1&type=quick"
                                GetData1(URLstr)
                            ElseIf str11 = "http://isbn.nu" Then
                                URLstr = str11 + "/" & Replace(txt_Cat_ISBN.Text, "-", "")
                                GetData2(URLstr)
                            ElseIf str11 = "http://worldcat.org" Then
                                URLstr = str11 + "/isbn/" & Replace(txt_Cat_ISBN.Text, "-", "")
                                GetData3(URLstr)
                            Else
                                URLstr = str11 + "/ISBN.aspx?ISBN=" & Replace(txt_Cat_ISBN.Text, "-", "")
                                GetData(URLstr)
                            End If
                        End If
                        q = q - 1
                        If Find = 1 Then
                            q = -1
                        ElseIf q = -1 And Find = 0 Then
                            'MsgBox("NO RECORD EXISTS")
                            'Call catAddNew()
                        End If
                    End While
                ElseIf ComboBox2.Text = "http://catalog.loc.gov" Then
                    URLstr = ComboBox2.Text + "/cgi-bin/Pwebrecon.cgi?DB=local&Search_Arg=" & Replace(txt_Cat_ISBN.Text, "-", "") & "&Search_Code=GKEY%5E*&CNT=100&hist=1&type=quick"
                    Find = 1
                    GetData1(URLstr)
                ElseIf ComboBox2.Text = "http://classic.isbn.nu" Then
                    URLstr = ComboBox2.Text + "/" & Replace(txt_Cat_ISBN.Text, "-", "")
                    Find = 1
                    GetData2(URLstr)
                ElseIf ComboBox2.Text = "http://worldcat.org" Then
                    URLstr = ComboBox2.Text + "/isbn/" & Replace(txt_Cat_ISBN.Text, "-", "")
                    Find = 1
                    GetData3(URLstr)
                Else
                    URLstr = ComboBox2.Text
                    URLstr = URLstr + "/ISBN.aspx?ISBN=" & Replace(txt_Cat_ISBN.Text, "-", "")
                    Find = 1
                    GetData(URLstr)
                End If
            Else
                'MsgBox("Please select URL from Drop-Down")
                'ComboBox2.Focus()
                Exit Sub
            End If
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "eGranthalaya-Download Record")
        Finally

        End Try
    End Sub
    'downlaod record from eG3 OPAC
    Sub GetData(ByVal URLstr As String)
        Try

            Dim client As System.Net.WebClient = New System.Net.WebClient
            Dim prx As New System.Net.WebProxy
            prx = System.Net.WebProxy.GetDefaultProxy()
            prx.Credentials = CredentialCache.DefaultNetworkCredentials
            GlobalProxySelection.Select = prx
            Dim data As Stream = client.OpenRead(URLstr)
            Dim reader As StreamReader = New StreamReader(data)
            Dim str As String = ""
            str = reader.ReadToEnd

            Dim Tit1, Tit2, EXIST
            If InStr(UCase(str), "<EXIST>") <> 0 Then
                Tit1 = InStr(1, UCase(str), "<EXIST>", 1)
                Tit1 = Tit1 + Len("<EXIST>")
                Tit2 = InStr(Tit1, str, "</EXIST>", 1)
                EXIST = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
            End If

            If EXIST = "YES" Then
                Dim BIB_CODE, MAT_CODE, DOC_TYPE_CODE, STANDARD_NO
                Dim TITLE, SUB_TITLE, AUTHOR1, AUTHOR2, AUTHOR3, CORPORATE_AUTHOR As Object

                If InStr(UCase(str), "<BIB_CODE>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<BIB_CODE>", 1)
                    Tit1 = Tit1 + Len("<BIB_CODE>")
                    Tit2 = InStr(Tit1, str, "</BIB_CODE>", 1)
                    BIB_CODE = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                    BIB_CODE = TrimX(BIB_CODE)
                Else
                    BIB_CODE = "M"
                End If

                If InStr(UCase(str), "<MAT_CODE>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<MAT_CODE>", 1)
                    Tit1 = Tit1 + Len("<MAT_CODE>")
                    Tit2 = InStr(Tit1, str, "</MAT_CODE>", 1)
                    MAT_CODE = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                    MAT_CODE = TrimX(MAT_CODE)
                Else
                    MAT_CODE = "B"
                End If

                If InStr(UCase(str), "<DOC_TYPE_CODE>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<DOC_TYPE_CODE>", 1)
                    Tit1 = Tit1 + Len("<DOC_TYPE_CODE>")
                    Tit2 = InStr(Tit1, str, "</DOC_TYPE_CODE>", 1)
                    DOC_TYPE_CODE = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                    DOC_TYPE_CODE = TrimX(DOC_TYPE_CODE)
                Else
                    DOC_TYPE_CODE = "BK"
                End If

                If InStr(UCase(str), "<STANDARD_NO>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<STANDARD_NO>", 1)
                    Tit1 = Tit1 + Len("<STANDARD_NO>")
                    Tit2 = InStr(Tit1, str, "</STANDARD_NO>", 1)
                    STANDARD_NO = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                    STANDARD_NO = TrimX(STANDARD_NO)
                Else
                    STANDARD_NO = ""
                End If

                If InStr(UCase(str), "<CTITLE>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<CTITLE>", 1)
                    Tit1 = Tit1 + Len("<CTITLE>")
                    Tit2 = InStr(Tit1, str, "</CTITLE>", 1)
                    TITLE = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                    TITLE = TrimAll(TITLE)
                    TITLE = Replace(TITLE, ".", " ")
                    If InStr(TITLE, ":") <> 0 Then
                        Dim SplitTitle As Object = Nothing
                        SplitTitle = Split(TITLE, ":")
                        TITLE = SplitTitle(0)
                        SUB_TITLE = SplitTitle(1)
                    End If
                Else
                    TITLE = ""
                End If

                If SUB_TITLE = "" Then
                    If InStr(UCase(str), "<SUB_TITLE>") <> 0 Then
                        Tit1 = InStr(1, UCase(str), "<SUB_TITLE>", 1)
                        Tit1 = Tit1 + Len("<SUB_TITLE>")
                        Tit2 = InStr(Tit1, str, "</SUB_TITLE>", 1)
                        SUB_TITLE = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                        SUB_TITLE = TrimAll(SUB_TITLE)
                    Else
                        SUB_TITLE = ""
                    End If
                End If

                If InStr(UCase(str), "<AUTHOR1>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<AUTHOR1>", 1)
                    Tit1 = Tit1 + Len("<AUTHOR1>")
                    Tit2 = InStr(Tit1, str, "</AUTHOR1>", 1)
                    AUTHOR1 = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                    AUTHOR1 = TrimAll(AUTHOR1)
                Else
                    AUTHOR1 = ""
                End If

                If InStr(UCase(str), "<AUTHOR2>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<AUTHOR2>", 1)
                    Tit1 = Tit1 + Len("<AUTHOR2>")
                    Tit2 = InStr(Tit1, str, "</AUTHOR2>", 1)
                    AUTHOR2 = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                    AUTHOR2 = TrimAll(AUTHOR2)
                Else
                    AUTHOR2 = ""
                End If

                If InStr(UCase(str), "<AUTHOR3>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<AUTHOR3>", 1)
                    Tit1 = Tit1 + Len("<AUTHOR3>")
                    Tit2 = InStr(Tit1, str, "</AUTHOR3>", 1)
                    AUTHOR3 = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                    AUTHOR3 = TrimAll(AUTHOR3)
                Else
                    AUTHOR3 = ""
                End If

                If InStr(UCase(str), "<CORPORATE_AUTHOR>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<CORPORATE_AUTHOR>", 1)
                    Tit1 = Tit1 + Len("<CORPORATE_AUTHOR>")
                    Tit2 = InStr(Tit1, str, "</CORPORATE_AUTHOR>", 1)
                    CORPORATE_AUTHOR = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                    CORPORATE_AUTHOR = TrimAll(CORPORATE_AUTHOR)
                Else
                    CORPORATE_AUTHOR = ""
                End If

                Dim EDITOR, EDITION, PLACE_OF_PUB, PUB_ID, YEAR_OF_PUB As Object
                If InStr(UCase(str), "<EDITOR>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<EDITOR>", 1)
                    Tit1 = Tit1 + Len("<EDITOR>")
                    Tit2 = InStr(Tit1, str, "</EDITOR>", 1)
                    EDITOR = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                    EDITOR = TrimAll(EDITOR)
                Else
                    EDITOR = ""
                End If

                If InStr(UCase(str), "<EDITION>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<EDITION>", 1)
                    Tit1 = Tit1 + Len("<EDITION>")
                    Tit2 = InStr(Tit1, str, "</EDITION>", 1)
                    EDITION = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                    EDITION = TrimAll(EDITION)
                Else
                    EDITION = ""
                End If

                If InStr(UCase(str), "<PLACE_OF_PUB>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<PLACE_OF_PUB>", 1)
                    Tit1 = Tit1 + Len("<PLACE_OF_PUB>")
                    Tit2 = InStr(Tit1, str, "</PLACE_OF_PUB>", 1)
                    PLACE_OF_PUB = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                    PLACE_OF_PUB = TrimAll(PLACE_OF_PUB)
                Else
                    PLACE_OF_PUB = ""
                End If

                If InStr(UCase(str), "<YEAR_OF_PUB>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<YEAR_OF_PUB>", 1)
                    Tit1 = Tit1 + Len("<YEAR_OF_PUB>")
                    Tit2 = InStr(Tit1, str, "</YEAR_OF_PUB>", 1)
                    YEAR_OF_PUB = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                    YEAR_OF_PUB = TrimX(YEAR_OF_PUB)
                Else
                    YEAR_OF_PUB = Now.Year
                End If

                Dim SERIES_TITLE, SUB_ID, KEYWORDS, DATE_ADDED As Object
                If InStr(UCase(str), "<SERIES_TITLE>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<SERIES_TITLE>", 1)
                    Tit1 = Tit1 + Len("<SERIES_TITLE>")
                    Tit2 = InStr(Tit1, str, "</SERIES_TITLE>", 1)
                    SERIES_TITLE = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                    SERIES_TITLE = TrimAll(SERIES_TITLE)
                Else
                    SERIES_TITLE = ""
                End If

                If InStr(UCase(str), "<KEYWORDS>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<KEYWORDS>", 1)
                    Tit1 = Tit1 + Len("<KEYWORDS>")
                    Tit2 = InStr(Tit1, str, "</KEYWORDS>", 1)
                    KEYWORDS = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                    KEYWORDS = Replace(KEYWORDS, ";", "; ")
                    KEYWORDS = TrimAll(KEYWORDS)
                Else
                    KEYWORDS = ""
                End If

                Dim CAT_LEVEL, MULTI_VOL, TOTAL_VOL As Object
                If InStr(UCase(str), "<MULTI_VOL>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<MULTI_VOL>", 1)
                    Tit1 = Tit1 + Len("<MULTI_VOL>")
                    Tit2 = InStr(Tit1, str, "</MULTI_VOL>", 1)
                    MULTI_VOL = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                Else
                    MULTI_VOL = "N"
                End If

                If InStr(UCase(str), "<TOTAL_VOL>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<TOTAL_VOL>", 1)
                    Tit1 = Tit1 + Len("<TOTAL_VOL>")
                    Tit2 = InStr(Tit1, str, "</TOTAL_VOL>", 1)
                    TOTAL_VOL = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                Else
                    TOTAL_VOL = ""
                End If

                Dim PUB_NAME, LIB_CODE, CONF_NAME As Object
                If InStr(UCase(str), "<PUB_NAME>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<PUB_NAME>", 1)
                    Tit1 = Tit1 + Len("<PUB_NAME>")
                    Tit2 = InStr(Tit1, str, "</PUB_NAME>", 1)
                    PUB_NAME = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                    PUB_NAME = TrimAll(PUB_NAME)
                Else
                    PUB_NAME = ""
                End If

                If InStr(UCase(str), "<CONF_NAME>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<CONF_NAME>", 1)
                    Tit1 = Tit1 + Len("<CONF_NAME>")
                    Tit2 = InStr(Tit1, str, "</CONF_NAME>", 1)
                    CONF_NAME = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                    CONF_NAME = TrimAll(CONF_NAME)
                Else
                    CONF_NAME = ""
                End If

                Dim LANG_CODE
                If InStr(UCase(str), "<LANG_CODE>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<LANG_CODE>", 1)
                    Tit1 = Tit1 + Len("<LANG_CODE>")
                    Tit2 = InStr(Tit1, str, "</LANG_CODE>", 1)
                    LANG_CODE = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                Else
                    LANG_CODE = "ENG"
                End If

                Dim TRANSLATOR As Object = Nothing
                If InStr(UCase(str), "<TRANSLATOR>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<TRANSLATOR>", 1)
                    Tit1 = Tit1 + Len("<TRANSLATOR>")
                    Tit2 = InStr(Tit1, str, "</TRANSLATOR>", 1)
                    TRANSLATOR = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                Else
                    TRANSLATOR = ""
                End If

                Dim ILLUSTRATOR As Object = Nothing
                If InStr(UCase(str), "<ILLUSTRATOR>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<ILLUSTRATOR>", 1)
                    Tit1 = Tit1 + Len("<ILLUSTRATOR>")
                    Tit2 = InStr(Tit1, str, "</ILLUSTRATOR>", 1)
                    ILLUSTRATOR = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                Else
                    ILLUSTRATOR = ""
                End If

                Dim SERIES_EDITOR As Object = Nothing
                If InStr(UCase(str), "<SERIES_EDITOR>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<SERIES_EDITOR>", 1)
                    Tit1 = Tit1 + Len("<SERIES_EDITOR>")
                    Tit2 = InStr(Tit1, str, "</SERIES_EDITOR>", 1)
                    SERIES_EDITOR = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                Else
                    SERIES_EDITOR = ""
                End If

                Dim NOTE As Object = Nothing
                If InStr(UCase(str), "<NOTE>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<NOTE>", 1)
                    Tit1 = Tit1 + Len("<NOTE>")
                    Tit2 = InStr(Tit1, str, "</NOTE>", 1)
                    NOTE = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                Else
                    NOTE = ""
                End If

                Dim ABSTRACT As Object = Nothing
                If InStr(UCase(str), "<ABSTRACT>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<ABSTRACT>", 1)
                    Tit1 = Tit1 + Len("<ABSTRACT>")
                    Tit2 = InStr(Tit1, str, "</ABSTRACT>", 1)
                    ABSTRACT = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                Else
                    ABSTRACT = ""
                End If

                Dim SP_NO As Object = Nothing
                If InStr(UCase(str), "<SP_NO>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<SP_NO>", 1)
                    Tit1 = Tit1 + Len("<SP_NO>")
                    Tit2 = InStr(Tit1, str, "</SP_NO>", 1)
                    SP_NO = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                Else
                    SP_NO = ""
                End If

                Dim REPORT_NO As Object = Nothing
                If InStr(UCase(str), "<REPORT_NO>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<REPORT_NO>", 1)
                    Tit1 = Tit1 + Len("<REPORT_NO>")
                    Tit2 = InStr(Tit1, str, "</REPORT_NO>", 1)
                    REPORT_NO = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                Else
                    REPORT_NO = ""
                End If

                Dim MANUAL_NO As Object = Nothing
                If InStr(UCase(str), "<MANUAL_NO>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<MANUAL_NO>", 1)
                    Tit1 = Tit1 + Len("<MANUAL_NO>")
                    Tit2 = InStr(Tit1, str, "</MANUAL_NO>", 1)
                    MANUAL_NO = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                Else
                    MANUAL_NO = ""
                End If

                Dim REFERENCE_NO As Object = Nothing
                If InStr(UCase(str), "<REFERENCE_NO>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<REFERENCE_NO>", 1)
                    Tit1 = Tit1 + Len("<REFERENCE_NO>")
                    Tit2 = InStr(Tit1, str, "</REFERENCE_NO>", 1)
                    REFERENCE_NO = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                Else
                    REFERENCE_NO = ""
                End If

                Dim URL As Object = Nothing
                If InStr(UCase(str), "<URL>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<URL>", 1)
                    Tit1 = Tit1 + Len("<URL>")
                    Tit2 = InStr(Tit1, str, "</URL>", 1)
                    URL = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                Else
                    URL = ""
                End If

                Dim REPRINTS As Object = Nothing
                If InStr(UCase(str), "<REPRINTS>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<REPRINTS>", 1)
                    Tit1 = Tit1 + Len("<REPRINTS>")
                    Tit2 = InStr(Tit1, str, "</REPRINTS>", 1)
                    REPRINTS = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                Else
                    REPRINTS = ""
                End If

                Dim VAR_TITLE As Object = Nothing
                If InStr(UCase(str), "<VAR_TITLE>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<VAR_TITLE>", 1)
                    Tit1 = Tit1 + Len("<VAR_TITLE>")
                    Tit2 = InStr(Tit1, str, "</VAR_TITLE>", 1)
                    VAR_TITLE = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                Else
                    VAR_TITLE = ""
                End If

                Dim TR_FROM As Object = Nothing
                If InStr(UCase(str), "<TR_FROM>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<TR_FROM>", 1)
                    Tit1 = Tit1 + Len("<TR_FROM>")
                    Tit2 = InStr(Tit1, str, "</TR_FROM>", 1)
                    TR_FROM = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                Else
                    TR_FROM = ""
                End If

                'WRITE VALUE IN TEXT BOXES
                DDL_Lang.SelectedValue = LANG_CODE
                DDL_YesNo.SelectedValue = MULTI_VOL
                txt_Cat_TotalVol.Text = TOTAL_VOL
                If TR_ISBN.Visible = True Then
                    txt_Cat_ISBN.Text = STANDARD_NO
                End If
                If TR_SPNo.Visible = True Then
                    txt_Cat_SPNo.Text = SP_NO
                End If
                If TR_ManualNo.Visible = True Then
                    txt_Cat_ManualNo.Text = MANUAL_NO
                End If
                If TR_ReportNo.Visible = True Then
                    txt_Cat_ReportNo.Text = REPORT_NO
                End If
                txt_Cat_Title.Text = TITLE
                If TR_SubTitle.Visible = True Then
                    txt_Cat_SubTitle.Text = SUB_TITLE
                End If
                If TR_VarTitle.Visible = True Then
                    txt_Cat_VarTitle.Text = VAR_TITLE
                End If
                If TR_ConfName.Visible = True Then
                    txt_Cat_ConfName.Text = CONF_NAME
                End If

                If TR_Author.Visible = True Then
                    txt_Cat_Author1.Text = AUTHOR1
                    txt_Cat_Author2.Text = AUTHOR2
                    txt_Cat_Author3.Text = AUTHOR3
                End If
                If TR_Editor.Visible = True Then
                    txt_Cat_Editor.Text = EDITOR
                End If
                If TR_Translator.Visible = True Then
                    txt_Cat_Translator.Text = TRANSLATOR
                End If
                If TR_Illustrator.Visible = True Then
                    txt_Cat_Illustrator.Text = ILLUSTRATOR
                End If

                If TR_CorpAuthor.Visible = True Then
                    txt_Cat_CorpAuthor.Text = CORPORATE_AUTHOR
                End If
                If TR_Edition.Visible = True Then
                    txt_Cat_Edition.Text = EDITION
                End If
                If TR_Reprint.Visible = True Then
                    txt_Cat_Reprint.Text = REPRINTS
                End If
                If TR_Publisher.Visible = True Then
                    'Check Duplicate User Code
                    Dim str2 As Object = Nothing
                    Dim flag As Object = Nothing
                    str2 = "SELECT PUB_ID FROM PUBLISHERS WHERE (PUB_NAME ='" & Trim(PUB_NAME) & "') "
                    Dim cmd1 As New SqlCommand(str2, SqlConn)
                    SqlConn.Open()
                    flag = cmd1.ExecuteScalar
                    SqlConn.Close()
                    If flag <> Nothing Then
                        Pub_ComboBox.Items.FindByText(PUB_NAME).Selected = True
                    Else
                        Pub_ComboBox.ClearSelection()
                        Pub_ComboBox.Items.Insert(0, New ListItem(PUB_NAME))
                        Pub_ComboBox.DataBind()
                        Pub_ComboBox.Text = PUB_NAME
                    End If
                End If
                If TR_Place.Visible = True Then
                    txt_Cat_Place.Text = PLACE_OF_PUB
                End If
                If TR_Year.Visible = True Then
                    txt_Cat_Year.Text = YEAR_OF_PUB
                End If
                If TR_Series.Visible = True Then
                    txt_Cat_Series.Text = SERIES_TITLE
                End If
                If TR_SeriesEditor.Visible = True Then
                    txt_Cat_SeriesEditor.Text = SERIES_EDITOR
                End If
                If TR_Note.Visible = True Then
                    txt_Cat_Note.Text = NOTE
                End If
                If TR_Absract.Visible = True Then
                    txt_Cat_Abstract.Text = ABSTRACT
                End If
                If TR_ReferenceNo.Visible = True Then
                    txt_Cat_ReferenceNo.Text = REFERENCE_NO
                End If
                If TR_URL.Visible = True Then
                    txt_Cat_URL.Text = URL
                End If
                If TR_TranslatedFrom.Visible = True Then
                    txt_Cat_TrFrom.Text = TR_FROM
                End If

                If TR_Keyword.Visible = True Then
                    txt_Cat_Keywords.Text = KEYWORDS
                End If
                If TR_Absract.Visible = True Then
                    txt_Cat_Abstract.Text = ""
                End If

                Find = 1
            Else
                If Find <> 0 Then
                    'MsgBox("NO RECORD EXISTS")
                End If
            End If
        Catch s As Exception
            'Label6.Text = (s.Message + "eGranthalaya-Download Record-from eG3 OPAC..")
            'Label15.Text = ""
        Finally

        End Try

    End Sub
    'download recrod from http://catalog.loc.gov
    Sub GetData1(ByVal URLstr As String)
        Try
            'Get HTML data
            Dim client As WebClient = New WebClient()
            Dim pry As New WebProxy
            pry = WebProxy.GetDefaultProxy()
            pry.Credentials = CredentialCache.DefaultNetworkCredentials
            GlobalProxySelection.Select = pry
            Dim data As Stream = client.OpenRead(URLstr)
            Dim reader As StreamReader = New StreamReader(data)
            Dim str As String = ""
            str = reader.ReadToEnd
            str = Replace(str, """", "'")
            str = Replace(Replace(str, Chr(10), ""), Chr(13), "")
            str = Replace(str, "&rsquo;", "'")

            Dim Tit1, Tit2, EXIST, URL, t1, FORMAT_CODE, DOC_TYPE_CODE, PERSONAL_NAME, TITLE, SUB_TITLE, EDITION
            If InStr(str, "<div class='nohits'><div class='nohits-left'><strong>") <> 0 Then
                Tit1 = InStr(1, str, "<div class='nohits'><div class='nohits-left'><strong>", 1)
                Tit1 = Tit1 + Len("<div class='nohits'><div class='nohits-left'><strong>")
                Tit2 = InStr(Tit1, str, "</strong>", 1)
                EXIST = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
            End If

            If EXIST = "Your search found no results" Then
                If Find <> 0 Then
                    'MsgBox("NO RECORD EXISTS")
                    'Call catAddNew()
                End If
            Else
                If InStr(str, "<TR><TH NOWRAP ALIGN=RIGHT VALIGN=TOP>LCCN Permalink:</TH><TD>") <> 0 Then
                    Tit1 = InStr(1, str, "<TR><TH NOWRAP ALIGN=RIGHT VALIGN=TOP>LCCN Permalink:</TH><TD>", 1)
                    Tit1 = Tit1 + Len("<TR><TH NOWRAP ALIGN=RIGHT VALIGN=TOP>LCCN Permalink:</TH><TD>")
                    Tit2 = InStr(Tit1, str, "</TD></TR>", 1)
                    URL = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                    Tit1 = InStr(1, URL, ">", 1)
                    Tit1 = Tit1 + Len(">")
                    Tit2 = InStr(Tit1, URL, "</a>", 1)
                    URL = Trim(Mid(URL, Tit1, (Tit2 - Tit1)))
                    URL = TrimX(URL)
                Else
                    URL = ""
                End If

                If InStr(str, "<TR><TH NOWRAP ALIGN=RIGHT VALIGN=TOP>Main Title:</TH><TD dir='ltr'>") <> 0 Then
                    Tit1 = InStr(1, str, "<TR><TH NOWRAP ALIGN=RIGHT VALIGN=TOP>Main Title:</TH><TD dir='ltr'>", 1)
                    Tit1 = Tit1 + Len("<TR><TH NOWRAP ALIGN=RIGHT VALIGN=TOP>Main Title:</TH><TD dir='ltr'>")
                    Tit2 = InStr(Tit1, str, "</TD></TR>", 1)
                    t1 = Trim(Mid(str, Tit1, (Tit2 - Tit1)))

                    Tit1 = InStr(1, t1, "", 1)
                    Tit1 = Tit1 + Len("")
                    Tit2 = InStr(Tit1, t1, "/", 1)
                    TITLE = Trim(Mid(t1, Tit1, (Tit2 - Tit1)))
                    TITLE = TrimAll(TITLE)
                    If InStr(TITLE, ":") <> 0 Then
                        Dim SplitTitle As Object = Nothing
                        SplitTitle = Split(TITLE, ":")
                        TITLE = SplitTitle(0)
                        SUB_TITLE = SplitTitle(1)
                    Else
                        SUB_TITLE = ""
                    End If
                Else
                    TITLE = ""
                End If

                Dim f
                Dim AUTHOR1 As Object
                If InStr(str, "<TH NOWRAP ALIGN=RIGHT VALIGN=TOP>Personal Name:</TH><TD dir='ltr'>") <> 0 Then
                    Tit1 = InStr(1, str, "<TH NOWRAP ALIGN=RIGHT VALIGN=TOP>Personal Name:</TH><TD dir='ltr'>", 1)
                    Tit1 = Tit1 + Len("<TH NOWRAP ALIGN=RIGHT VALIGN=TOP>Personal Name:</TH><TD dir='ltr'>")
                    Tit2 = InStr(Tit1, str, "</TD></TR>", 1)
                    AUTHOR1 = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                    Tit1 = InStr(1, AUTHOR1, ">", 1)
                    Tit1 = Tit1 + Len(">")
                    Tit2 = InStr(Tit1, AUTHOR1, "</A>", 1)
                    AUTHOR1 = Trim(Mid(AUTHOR1, Tit1, (Tit2 - Tit1)))
                    f = AUTHOR1.ToString.Substring(Len(AUTHOR1) - 1)
                    If f = "-" Then
                        AUTHOR1 = AUTHOR1.ToString.Remove(Len(AUTHOR1) - 7)
                    End If
                    AUTHOR1 = Replace(AUTHOR1, ".", " ")
                    AUTHOR1 = TrimAll(AUTHOR1)
                Else
                    AUTHOR1 = ""
                End If

                If InStr(str, "<TR><TH NOWRAP ALIGN=RIGHT VALIGN=TOP>Edition Information:</TH><TD dir='ltr'>") <> 0 Then
                    Tit1 = InStr(1, str, "<TR><TH NOWRAP ALIGN=RIGHT VALIGN=TOP>Edition Information:</TH><TD dir='ltr'>", 1)
                    Tit1 = Tit1 + Len("<TR><TH NOWRAP ALIGN=RIGHT VALIGN=TOP>Edition Information:</TH><TD dir='ltr'>")
                    Tit2 = InStr(Tit1, str, "</TD></TR>", 1)
                    EDITION = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                Else
                    EDITION = ""
                End If

                Dim PUB_NAME, PLACE_OF_PUB, YEAR_OF_PUB, STANDARD_NO As Object
                Dim y As String
                If InStr(str, "<TR><TH NOWRAP ALIGN=RIGHT VALIGN=TOP>Published/Created:</TH><TD dir='ltr'>") <> 0 Then
                    Tit1 = InStr(1, str, "<TR><TH NOWRAP ALIGN=RIGHT VALIGN=TOP>Published/Created:</TH><TD dir='ltr'>", 1)
                    Tit1 = Tit1 + Len("<TR><TH NOWRAP ALIGN=RIGHT VALIGN=TOP>Published/Created:</TH><TD dir='ltr'>")
                    Tit2 = InStr(Tit1, str, "</TD></TR>", 1)
                    t1 = Trim(Mid(str, Tit1, (Tit2 - Tit1)))

                    Tit1 = InStr(1, t1, "", 1)
                    Tit1 = Tit1 + Len("")
                    Tit2 = InStr(Tit1, t1, ":", 1)
                    PLACE_OF_PUB = Trim(Mid(t1, Tit1, (Tit2 - Tit1)))

                    Tit1 = InStr(1, t1, ":", 1)
                    Tit1 = Tit1 + Len(":")
                    Tit2 = InStr(Tit1, t1, ",", 1)
                    PUB_NAME = Trim(Mid(t1, Tit1, (Tit2 - Tit1)))
                    PUB_NAME = Replace(PUB_NAME, ".", " ")
                    PUB_NAME = TrimAll(PUB_NAME)
                    y = t1.ToString.Substring(Len(t1) - 5, 4)

                    If y.Length = 4 Then
                        YEAR_OF_PUB = Trim(y)
                    Else
                        YEAR_OF_PUB = Trim(y.Substring(1, 4))
                    End If
                Else
                    PLACE_OF_PUB = ""
                    PUB_NAME = ""
                    YEAR_OF_PUB = Now.Year
                End If

                Dim std As String
                If InStr(str, "<TR><TH NOWRAP ALIGN=RIGHT VALIGN=TOP>ISBN:</TH><TD dir='ltr'>") <> 0 Then
                    Tit1 = InStr(1, str, "<TR><TH NOWRAP ALIGN=RIGHT VALIGN=TOP>ISBN:</TH><TD dir='ltr'>", 1)
                    Tit1 = Tit1 + Len("<TR><TH NOWRAP ALIGN=RIGHT VALIGN=TOP>ISBN:</TH><TD dir='ltr'>")
                    Tit2 = InStr(Tit1, str, "</TD></TR>", 1)
                    std = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                    If std.Length = 10 Or std.Length = 13 Then
                        STANDARD_NO = Trim(std)
                    Else
                        Tit1 = InStr(1, std, "", 1)
                        Tit1 = Tit1 + Len("")
                        Tit2 = InStr(Tit1, std, "(", 1)
                        STANDARD_NO = Trim(Mid(std, Tit1, (Tit2 - Tit1)))
                    End If
                    If Trim(STANDARD_NO) <> Trim(txt_Cat_ISBN.Text) Then
                        STANDARD_NO = Trim(txt_Cat_ISBN.Text)
                    End If
                Else
                    'STANDARD_NO = ""
                End If

                DDL_Lang.SelectedValue = "ENG"
                DDL_YesNo.SelectedValue = "N"
                txt_Cat_TotalVol.Text = ""

                txt_Cat_Title.Text = TITLE
                If TR_SubTitle.Visible = True Then
                    txt_Cat_SubTitle.Text = SUB_TITLE
                End If

                If TR_Author.Visible = True Then
                    txt_Cat_Author1.Text = AUTHOR1
                End If

                If TR_Edition.Visible = True Then
                    txt_Cat_Edition.Text = EDITION
                End If

                If TR_Publisher.Visible = True Then
                    'Check Duplicate User Code
                    Dim str2 As Object = Nothing
                    Dim flag As Object = Nothing
                    str2 = "SELECT PUB_ID FROM PUBLISHERS WHERE (PUB_NAME ='" & Trim(PUB_NAME) & "') "
                    Dim cmd1 As New SqlCommand(str2, SqlConn)
                    SqlConn.Open()
                    flag = cmd1.ExecuteScalar
                    SqlConn.Close()
                    If flag <> Nothing Then
                        Pub_ComboBox.Items.FindByText(PUB_NAME).Selected = True
                    Else
                        Pub_ComboBox.ClearSelection()
                        Pub_ComboBox.Items.Insert(0, New ListItem(PUB_NAME))
                        Pub_ComboBox.DataBind()
                        Pub_ComboBox.Text = PUB_NAME
                    End If
                End If
                If TR_Place.Visible = True Then
                    txt_Cat_Place.Text = PLACE_OF_PUB
                End If
                If TR_Year.Visible = True Then
                    txt_Cat_Year.Text = YEAR_OF_PUB
                End If

                If TR_URL.Visible = True Then
                    txt_Cat_URL.Text = URL
                End If
                Find = 1
            End If
        Catch s As Exception
            'MessageBox.Show(s.Message, "eGranthalaya-Download Record from Catalog.loc.gov")
        Finally

        End Try
    End Sub
    'download record from http://isbn.nu
    Sub GetData2(ByVal URLstr As String)
        Try
            ' Get HTML data
            Dim client As WebClient = New WebClient()
            Dim pry As New WebProxy
            pry = WebProxy.GetDefaultProxy()
            pry.Credentials = CredentialCache.DefaultNetworkCredentials
            GlobalProxySelection.Select = pry
            Dim data As Stream = client.OpenRead(URLstr)
            Dim reader As StreamReader = New StreamReader(data)
            Dim str As String = ""
            str = reader.ReadToEnd
            str = Replace(str, """", "'")
            str = Replace(Replace(str, Chr(10), ""), Chr(13), "")
            str = Replace(str, "&rsquo;", "'")

            Dim Tit1, Tit2, EXIST, URL, t1, t2, FORMAT_CODE, DOC_TYPE_CODE, PERSONAL_NAME, Title, Sub_Title, EDITION
            If InStr(LCase(str), "<title>") <> 0 Then
                Tit1 = InStr(1, LCase(str), "<title>", 1)
                Tit1 = Tit1 + Len("<title>")
                Tit2 = InStr(Tit1, str, "</title>", 1)
                EXIST = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
            End If

            If EXIST = "No Title Found" Or EXIST = "Invalid ISBN or EAN" Then
                If Find <> 0 Then
                    'MsgBox("NO RECORD EXISTS")
                    'Call catAddNew()
                End If
            Else
                If InStr(str, "<div id='cmpruled'>") <> 0 Then
                    Tit1 = InStr(1, str, "<div id='cmpruled'>", 1)
                    Tit1 = Tit1 + Len("<div id='cmpruled'>")
                    Tit2 = InStr(Tit1, str, "<BR>", 1)
                    URL = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                    Tit1 = InStr(1, URL, ">", 1)
                    Tit1 = Tit1 + Len(">")
                    Tit2 = InStr(Tit1, URL, "</a>", 1)
                    URL = Trim(Mid(URL, Tit1, (Tit2 - Tit1)))
                    URL = TrimX(URL)
                Else
                    URL = ""
                End If

                ' DOC_TYPE_CODE = "BK"
                ' FORMAT_CODE = "PT"

                If InStr(str, "<p class='bodytext'><span class='bolddarkbody'><b>") <> 0 Then
                    Tit1 = InStr(1, str, "<p class='bodytext'><span class='bolddarkbody'><b>", 1)
                    Tit1 = Tit1 + Len("<p class='bodytext'><span class='bolddarkbody'><b>")
                    Tit2 = InStr(Tit1, str, "</b>", 1)
                    Title = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                    Title = TrimAll(Title)
                    If InStr(Title, ":") <> 0 Then
                        Dim SplitTitle As Object = Nothing
                        SplitTitle = Split(Title, ":")
                        Title = SplitTitle(0)
                        Sub_Title = SplitTitle(1)
                    Else
                        Sub_Title = ""
                    End If
                Else
                    Title = ""
                End If

                Dim AUTHORS As String
                Dim AUTHOR1, AUTHOR2, AUTHOR3
                Dim at1 As String
                If InStr(str, "<div class='bi_col_title'>Authors</div><div class='bi_col_value'>") <> 0 Then
                    Tit1 = InStr(1, str, "<div class='bi_col_title'>Authors</div><div class='bi_col_value'>", 1)
                    Tit1 = Tit1 + Len("<div class='bi_col_title'>Authors</div><div class='bi_col_value'>")
                    Tit2 = InStr(Tit1, str, "<div class='clear_space'>", 1)
                    at1 = Trim(Mid(str, Tit1, (Tit2 - Tit1)))

                    Tit1 = InStr(1, at1, "</a>", 1)
                    Tit1 = Tit1 + Len("</a>")
                    Tit2 = InStr(Tit1, at1, "<br>", 1)
                    t2 = Mid(at1, Tit1, (Tit2 - Tit1))

                    If t2 <> Nothing Then
                        at1 = at1.Remove(Tit1, Len(t2) - 1)
                        at1 = Replace(at1, " ", "")
                    End If

                    While at1 <> "</div>"
                        Tit1 = InStr(1, at1, ">", 1)
                        Tit1 = Tit1 + Len(">")
                        Tit2 = InStr(Tit1, at1, "</a><br>", 1)
                        t2 = Trim(Mid(at1, Tit1, (Tit2 - Tit1)))
                        Tit2 = Tit2 + 7
                        at1 = at1.Substring(Tit2)
                        If at1 = "</div>" Then
                            AUTHORS = AUTHORS + t2
                        Else
                            AUTHORS = AUTHORS + t2 + ","
                        End If
                    End While
                Else
                    AUTHOR1 = ""
                    AUTHOR2 = ""
                    AUTHOR3 = ""
                End If
                If AUTHORS <> Nothing Then
                    Dim auth() As String = AUTHORS.Split(",")

                    Dim i As Integer
                    i = auth.Length
                    If i > 3 Then
                        i = 3
                    End If
                    If i = 3 Then
                        AUTHOR1 = auth(0)
                        AUTHOR1 = Replace(AUTHOR1, ".", " ")
                        AUTHOR1 = TrimAll(AUTHOR1)

                        AUTHOR2 = auth(1)
                        AUTHOR2 = Replace(AUTHOR2, ".", " ")
                        AUTHOR2 = TrimAll(AUTHOR2)

                        AUTHOR3 = auth(2)
                        AUTHOR3 = Replace(AUTHOR3, ".", " ")
                        AUTHOR3 = TrimAll(AUTHOR3)
                    ElseIf i = 2 Then
                        AUTHOR1 = auth(0)
                        AUTHOR1 = Replace(AUTHOR1, ".", " ")
                        AUTHOR1 = TrimAll(AUTHOR1)

                        AUTHOR2 = auth(1)
                        AUTHOR2 = Replace(AUTHOR2, ".", " ")
                        AUTHOR2 = TrimAll(AUTHOR2)
                        AUTHOR3 = ""
                    ElseIf i = 1 Then
                        AUTHOR1 = auth(0)
                        AUTHOR1 = Replace(AUTHOR1, ".", " ")
                        AUTHOR1 = TrimAll(AUTHOR1)
                        AUTHOR2 = ""
                        AUTHOR3 = ""
                    End If
                Else
                    AUTHOR1 = ""
                    AUTHOR2 = ""
                    AUTHOR3 = ""
                End If

                If InStr(str, "<div class='bi_col_title'>Edition</div><div class='bi_col_value'>") <> 0 Then
                    Tit1 = InStr(1, str, "<div class='bi_col_title'>Edition</div><div class='bi_col_value'>", 1)
                    Tit1 = Tit1 + Len("<div class='bi_col_title'>Edition</div><div class='bi_col_value'>")
                    Tit2 = InStr(Tit1, str, "</div>", 1)
                    EDITION = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                    EDITION = TrimAll(EDITION)
                Else
                    EDITION = ""
                End If

                Dim PUB_NAME, YEAR_OF_PUB, STANDARD_NO
                If InStr(str, "<div class='bi_col_title'>Publisher</div><div class='bi_col_value'>") <> 0 Then
                    Tit1 = InStr(1, str, "<div class='bi_col_title'>Publisher</div><div class='bi_col_value'>", 1)
                    Tit1 = Tit1 + Len("<div class='bi_col_title'>Publisher</div><div class='bi_col_value'>")
                    Tit2 = InStr(Tit1, str, "</div>", 1)
                    PUB_NAME = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                    PUB_NAME = Replace(PUB_NAME, ".", " ")
                    PUB_NAME = TrimAll(PUB_NAME)
                Else
                    PUB_NAME = ""
                End If

                If InStr(str, "<div class='bi_col_title'>Publication date</div><div class='bi_col_value'>") <> 0 Then
                    Tit1 = InStr(1, str, "<div class='bi_col_title'>Publication date</div><div class='bi_col_value'>", 1)
                    Tit1 = Tit1 + Len("<div class='bi_col_title'>Publication date</div><div class='bi_col_value'>")
                    Tit2 = InStr(Tit1, str, "</div>", 1)
                    YEAR_OF_PUB = Trim(Mid(str, Tit1, (Tit2 - Tit1)))

                    Tit1 = InStr(1, YEAR_OF_PUB, ",", 1)
                    Tit1 = Tit1 + Len(",")
                    Tit2 = Len(YEAR_OF_PUB) + 1
                    YEAR_OF_PUB = Trim(Mid(YEAR_OF_PUB, Tit1, (Tit2 - Tit1)))
                Else
                    YEAR_OF_PUB = Now.Year
                End If

                Dim DESCRIPTION
                If InStr(str, "<b>Product Description</b>:") <> 0 Then
                    Tit1 = InStr(1, str, "<b>Product Description</b>:", 1)
                    Tit1 = Tit1 + Len("<b>Product Description</b>:")
                    Tit2 = InStr(Tit1, str, ".", 1)
                    DESCRIPTION = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                    DESCRIPTION = Replace(DESCRIPTION, "<p class='bodytext'>", "")
                    DESCRIPTION = TrimAll(DESCRIPTION)
                Else
                    DESCRIPTION = ""
                End If

                txt_Cat_Title.Text = Title
                If TR_SubTitle.Visible = True Then
                    txt_Cat_SubTitle.Text = Sub_Title
                End If

                If TR_Author.Visible = True Then
                    txt_Cat_Author1.Text = AUTHOR1
                    txt_Cat_Author2.Text = AUTHOR2
                    txt_Cat_Author3.Text = AUTHOR3
                End If

                If TR_Edition.Visible = True Then
                    txt_Cat_Edition.Text = EDITION
                End If

                If TR_Publisher.Visible = True Then
                    'Check Duplicate User Code
                    Dim str2 As Object = Nothing
                    Dim flag As Object = Nothing
                    str2 = "SELECT PUB_ID FROM PUBLISHERS WHERE (PUB_NAME ='" & Trim(PUB_NAME) & "') "
                    Dim cmd1 As New SqlCommand(str2, SqlConn)
                    SqlConn.Open()
                    flag = cmd1.ExecuteScalar
                    SqlConn.Close()
                    If flag <> Nothing Then
                        Pub_ComboBox.Items.FindByText(PUB_NAME).Selected = True
                    Else
                        Pub_ComboBox.ClearSelection()
                        Pub_ComboBox.Items.Insert(0, New ListItem(PUB_NAME))
                        Pub_ComboBox.DataBind()
                        Pub_ComboBox.Text = PUB_NAME
                    End If
                End If
                If TR_Year.Visible = True Then
                    txt_Cat_Year.Text = YEAR_OF_PUB
                End If
                If TR_Note.Visible = True Then
                    txt_Cat_Note.Text = DESCRIPTION
                End If
                If TR_URL.Visible = True Then
                    txt_Cat_URL.Text = URL
                End If
                Find = 1
            End If
        Catch s As Exception
            'MessageBox.Show(s.Message, "eGranthalaya-Download from ISBN.nu")
        Finally

        End Try

    End Sub
    'download record from "http://worldcat.org"
    Sub GetData3(ByVal URLstr As String)
        Try
            ' Get HTML data
            Dim client As WebClient = New WebClient()
            Dim pry As New WebProxy
            pry = WebProxy.GetDefaultProxy()
            pry.Credentials = CredentialCache.DefaultNetworkCredentials
            GlobalProxySelection.Select = pry
            Dim data As Stream = client.OpenRead(URLstr)
            Dim reader As StreamReader = New StreamReader(data)
            Dim str As String = ""
            str = reader.ReadToEnd
            str = Replace(str, """", "'")
            str = Replace(Replace(str, Chr(10), ""), Chr(13), "")
            str = Replace(str, "&rsquo;", "'")
            str = Replace(str, "&amp;", "&")

            Dim Tit1, Tit2, EXIST, URL, t1, LANG_CODE, FORMAT_CODE, DOC_TYPE_CODE, PERSONAL_NAME, Title, Sub_Title, EDITION
            If InStr(str, "<div id='div-maincol'><br><p>") <> 0 Then
                Tit1 = InStr(1, str, "<div id='div-maincol'><br><p>", 1)
                Tit1 = Tit1 + Len("<div id='div-maincol'><br><p>")
                Tit2 = InStr(Tit1, str, ".", 1)
                EXIST = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
            End If
            If EXIST = "The page you tried was not found" Then
                If Find <> 0 Then
                    ' MsgBox("NO RECORD EXISTS")
                    'Call catAddNew()
                End If
            Else
                If InStr(str, "<input class='text' name='' id='util-permalink' value='") <> 0 Then
                    Tit1 = InStr(1, str, "<input class='text' name='' id='util-permalink' value='", 1)
                    Tit1 = Tit1 + Len("<input class='text' name='' id='util-permalink' value='")
                    Tit2 = InStr(Tit1, str, "'", 1)
                    URL = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                    URL = TrimX(URL)
                Else
                    URL = ""
                End If

                If InStr(str, "<h1 class='title'>") <> 0 Then
                    Tit1 = InStr(1, str, "<h1 class='title'>", 1)
                    Tit1 = Tit1 + Len("<h1 class='title'>")
                    Tit2 = InStr(Tit1, str, "</h1>", 1)
                    Title = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                    Title = TrimAll(Title)
                    If InStr(Title, ":") <> 0 Then
                        Dim SplitTitle As Object = Nothing
                        SplitTitle = Split(Title, ":")
                        Title = SplitTitle(0)
                        Sub_Title = SplitTitle(1)
                    Else
                        Sub_Title = ""
                    End If
                Else
                    Title = ""
                End If

                Dim t31 As String
                If InStr(str, "<span class='itemtype'><span class='bks'  id='editionFormatType'>") <> 0 Then
                    Tit1 = InStr(1, str, "<span class='itemtype'><span class='bks'  id='editionFormatType'>", 1)
                    Tit1 = Tit1 + Len("<span class='itemtype'><span class='bks'  id='editionFormatType'>")
                    Tit2 = InStr(Tit1, str, "<", 1)
                    t31 = Trim(Mid(str, Tit1, (Tit2 - Tit1)))

                    Dim d1() As String = t31.Split(":")
                    Dim j As Integer
                    j = d1.Length
                    If j = 1 Then

                    ElseIf j = 2 Then

                    ElseIf j = 3 Then

                        EDITION = Trim(d1(2))
                    End If
                Else

                End If

                Dim AUTHORS As String
                Dim AUTHOR1, AUTHOR2, AUTHOR3, CORPAUTHOR As Object
                If InStr(str, "<td id='bib-author-cell'>") <> 0 Then
                    Tit1 = InStr(1, str, "<td id='bib-author-cell'>", 1)
                    Tit1 = Tit1 + Len("<td id='bib-author-cell'>")
                    Tit2 = InStr(Tit1, str, "</td>", 1)
                    AUTHORS = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                    AUTHORS = Replace(AUTHORS, ".", " ")
                    AUTHORS = TrimAll(AUTHORS)

                    Dim auth() As String = AUTHORS.Split(";")
                    Dim i As Integer
                    i = auth.Length
                    If i > 3 Then
                        i = 3
                    End If
                    If i = 3 Then
                        AUTHOR1 = Trim(auth(0))
                        Tit1 = InStr(1, AUTHOR1, ">", 1)
                        Tit1 = Tit1 + Len(">")
                        Tit2 = InStr(Tit1, AUTHOR1, "</a>", 1)
                        AUTHOR1 = Trim(Mid(AUTHOR1, Tit1, (Tit2 - Tit1)))

                        AUTHOR2 = Trim(auth(1))
                        Tit1 = InStr(1, AUTHOR2, ">", 1)
                        Tit1 = Tit1 + Len(">")
                        Tit2 = InStr(Tit1, AUTHOR2, "</a>", 1)
                        AUTHOR2 = Trim(Mid(AUTHOR2, Tit1, (Tit2 - Tit1)))

                        AUTHOR3 = Trim(auth(2))
                        Tit1 = InStr(1, AUTHOR3, ">", 1)
                        Tit1 = Tit1 + Len(">")
                        Tit2 = InStr(Tit1, AUTHOR3, "</a>", 1)
                        If Tit2 <> 0 Then
                            AUTHOR3 = Trim(Mid(AUTHOR3, Tit1, (Tit2 - Tit1)))
                        Else
                            AUTHOR3 = ""
                        End If
                    ElseIf i = 2 Then
                        AUTHOR1 = Trim(auth(0))
                        Tit1 = InStr(1, AUTHOR1, ">", 1)
                        Tit1 = Tit1 + Len(">")
                        Tit2 = InStr(Tit1, AUTHOR1, "</a>", 1)
                        AUTHOR1 = Trim(Mid(AUTHOR1, Tit1, (Tit2 - Tit1)))

                        AUTHOR2 = Trim(auth(1))
                        Tit1 = InStr(1, AUTHOR2, ">", 1)
                        Tit1 = Tit1 + Len(">")
                        Tit2 = InStr(Tit1, AUTHOR2, "</a>", 1)
                        AUTHOR2 = Trim(Mid(AUTHOR2, Tit1, (Tit2 - Tit1)))
                        AUTHOR3 = ""
                    ElseIf i = 1 Then
                        AUTHOR1 = Trim(auth(0))
                        Tit1 = InStr(1, AUTHOR1, ">", 1)
                        Tit1 = Tit1 + Len(">")
                        Tit2 = InStr(Tit1, AUTHOR1, "</a>", 1)
                        AUTHOR1 = Trim(Mid(AUTHOR1, Tit1, (Tit2 - Tit1)))

                        AUTHOR2 = ""
                        AUTHOR3 = ""
                    End If
                Else
                    AUTHOR1 = ""
                    AUTHOR2 = ""
                    AUTHOR3 = ""
                End If

                If AUTHOR1 <> "" Then
                    If Len(AUTHOR1) > 50 Then
                        CORPAUTHOR = TrimAll(AUTHOR1)
                        AUTHOR1 = ""
                        AUTHOR2 = ""
                        AUTHOR3 = ""
                    End If
                End If

                Dim PUB_NAME, PLACE_OF_PUB, YEAR_OF_PUB, STANDARD_NO, SERIES_TITLE
                If InStr(str, "<td id='bib-publisher-cell'>") <> 0 Then
                    Tit1 = InStr(1, str, "<td id='bib-publisher-cell'>", 1)
                    Tit1 = Tit1 + Len("<td id='bib-publisher-cell'>")
                    Tit2 = InStr(Tit1, str, "</td>", 1)
                    t1 = Trim(Mid(str, Tit1, (Tit2 - Tit1)))

                    Tit1 = InStr(1, t1, "", 1)
                    Tit1 = Tit1 + Len("")
                    Tit2 = InStr(Tit1, t1, ":", 1)
                    PLACE_OF_PUB = Trim(Mid(t1, Tit1, (Tit2 - Tit1)))

                    Tit1 = InStr(1, t1, ":", 1)
                    Tit1 = Tit1 + Len(":")
                    If InStr(t1, ",") <> 0 Then
                        Tit2 = InStr(Tit1, t1, ",", 1)
                    End If
                    If InStr(t1, ".") <> 0 Then
                        Tit2 = InStr(Tit1, t1, ".", 1)
                    End If
                    If InStr(t1, ";") <> 0 Then
                        Tit2 = InStr(Tit1, t1, ";", 1)
                    End If
                    PUB_NAME = Trim(Mid(t1, Tit1, (Tit2 - Tit1)))
                    PUB_NAME = Replace(PUB_NAME, ".", " ")
                    PUB_NAME = TrimAll(PUB_NAME)
                    YEAR_OF_PUB = t1.ToString.Substring(Len(t1) - 5, 4)
                Else
                    PLACE_OF_PUB = ""
                    PUB_NAME = ""
                    YEAR_OF_PUB = Now.Year
                End If

                If InStr(str, "<td id='bib-hotSeriesTitles-cell'>") <> 0 Then
                    Tit1 = InStr(1, str, "<td id='bib-hotSeriesTitles-cell'>", 1)
                    Tit1 = Tit1 + Len("<td id='bib-hotSeriesTitles-cell'>")
                    Tit2 = InStr(Tit1, str, "</td>", 1)
                    SERIES_TITLE = Trim(Mid(str, Tit1, (Tit2 - Tit1)))

                    Tit1 = InStr(1, SERIES_TITLE, ">", 1)
                    Tit1 = Tit1 + Len(">")
                    Tit2 = InStr(Tit1, SERIES_TITLE, "</a>", 1)
                    SERIES_TITLE = Trim(Mid(SERIES_TITLE, Tit1, (Tit2 - Tit1)))
                    SERIES_TITLE = TrimAll(SERIES_TITLE)
                Else
                    SERIES_TITLE = ""
                End If

                'WRITE VALUE IN TEXT BOXES
                DDL_Lang.SelectedValue = LANG_CODE
                DDL_YesNo.SelectedValue = "N"
                txt_Cat_TotalVol.Text = ""
                txt_Cat_Title.Text = Title
                If TR_SubTitle.Visible = True Then
                    txt_Cat_SubTitle.Text = Sub_Title
                End If
                If TR_Author.Visible = True Then
                    txt_Cat_Author1.Text = AUTHOR1
                    txt_Cat_Author2.Text = AUTHOR2
                    txt_Cat_Author3.Text = AUTHOR3
                End If
                If TR_Edition.Visible = True Then
                    txt_Cat_Edition.Text = EDITION
                End If
                If TR_Publisher.Visible = True Then
                    'Check Duplicate User Code
                    Dim str2 As Object = Nothing
                    Dim flag As Object = Nothing
                    str2 = "SELECT PUB_ID FROM PUBLISHERS WHERE (PUB_NAME ='" & Trim(PUB_NAME) & "') "
                    Dim cmd1 As New SqlCommand(str2, SqlConn)
                    SqlConn.Open()
                    flag = cmd1.ExecuteScalar
                    SqlConn.Close()
                    If flag <> Nothing Then
                        Pub_ComboBox.Items.FindByText(PUB_NAME).Selected = True
                    Else
                        Pub_ComboBox.ClearSelection()
                        Pub_ComboBox.Items.Insert(0, New ListItem(PUB_NAME))
                        Pub_ComboBox.DataBind()
                        Pub_ComboBox.Text = PUB_NAME
                    End If
                End If
                If TR_Place.Visible = True Then
                    txt_Cat_Place.Text = PLACE_OF_PUB
                End If
                If TR_Year.Visible = True Then
                    txt_Cat_Year.Text = YEAR_OF_PUB
                End If
                If TR_Series.Visible = True Then
                    txt_Cat_Series.Text = SERIES_TITLE
                End If
                If TR_URL.Visible = True Then
                    txt_Cat_URL.Text = URL
                End If
                Find = 1
            End If
        Catch s As Exception
            'MessageBox.Show(s.Message, "eGranthalaya-Download record from Worldcat")
        Finally

        End Try
    End Sub
    'downlaod record from eG3 OPAC by searching from TITLE/AUTHOR,e tc from z39.50
    Sub GetDataEG3()
        Try
            Dim client As WebClient = New WebClient()
            Dim prx As New WebProxy
            prx = WebProxy.GetDefaultProxy()
            prx.Credentials = CredentialCache.DefaultNetworkCredentials
            GlobalProxySelection.Select = prx
            Dim URLstr As String = Nothing
            Dim str11 As String = Nothing
            str11 = "http://www.eglibnet.nic.in"
            URLstr = str11 + "/Download_CATNO.aspx?CAT_NO=" & txt_Cat_ISBN.Text

            Dim data As Stream = client.OpenRead(URLstr)
            Dim reader As StreamReader = New StreamReader(data)
            Dim str As String = ""
            str = reader.ReadToEnd

            Dim Tit1, Tit2, EXIST
            If InStr(UCase(str), "<EXIST>") <> 0 Then
                Tit1 = InStr(1, UCase(str), "<EXIST>", 1)
                Tit1 = Tit1 + Len("<EXIST>")
                Tit2 = InStr(Tit1, str, "</EXIST>", 1)
                EXIST = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
            End If

            If EXIST = "YES" Then
                Dim BIB_CODE, MAT_CODE, DOC_TYPE_CODE, STANDARD_NO
                Dim Title, SUB_TITLE, AUTHOR1, AUTHOR2, AUTHOR3, CORPORATE_AUTHOR As Object
                If InStr(UCase(str), "<BIB_CODE>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<BIB_CODE>", 1)
                    Tit1 = Tit1 + Len("<BIB_CODE>")
                    Tit2 = InStr(Tit1, str, "</BIB_CODE>", 1)
                    BIB_CODE = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                    BIB_CODE = TrimX(BIB_CODE)
                Else
                    BIB_CODE = "M"
                End If

                If InStr(UCase(str), "<MAT_CODE>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<MAT_CODE>", 1)
                    Tit1 = Tit1 + Len("<MAT_CODE>")
                    Tit2 = InStr(Tit1, str, "</MAT_CODE>", 1)
                    MAT_CODE = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                    MAT_CODE = TrimX(MAT_CODE)
                Else
                    MAT_CODE = "B"
                End If

                If InStr(UCase(str), "<DOC_TYPE_CODE>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<DOC_TYPE_CODE>", 1)
                    Tit1 = Tit1 + Len("<DOC_TYPE_CODE>")
                    Tit2 = InStr(Tit1, str, "</DOC_TYPE_CODE>", 1)
                    DOC_TYPE_CODE = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                    DOC_TYPE_CODE = TrimX(DOC_TYPE_CODE)
                Else
                    DOC_TYPE_CODE = "BK"
                End If

                If InStr(UCase(str), "<STANDARD_NO>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<STANDARD_NO>", 1)
                    Tit1 = Tit1 + Len("<STANDARD_NO>")
                    Tit2 = InStr(Tit1, str, "</STANDARD_NO>", 1)
                    STANDARD_NO = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                    STANDARD_NO = TrimX(STANDARD_NO)
                Else
                    STANDARD_NO = ""
                End If

                If InStr(UCase(str), "<CTITLE>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<CTITLE>", 1)
                    Tit1 = Tit1 + Len("<CTITLE>")
                    Tit2 = InStr(Tit1, str, "</CTITLE>", 1)
                    Title = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                    Title = TrimAll(Title)
                    Title = Replace(Title, ".", " ")
                    If InStr(Title, ":") <> 0 Then
                        Dim SplitTitle As Object = Nothing
                        SplitTitle = Split(Title, ":")
                        Title = SplitTitle(0)
                        SUB_TITLE = SplitTitle(1)
                    End If
                Else
                    Title = ""
                End If

                If SUB_TITLE = "" Then
                    If InStr(UCase(str), "<SUB_TITLE>") <> 0 Then
                        Tit1 = InStr(1, UCase(str), "<SUB_TITLE>", 1)
                        Tit1 = Tit1 + Len("<SUB_TITLE>")
                        Tit2 = InStr(Tit1, str, "</SUB_TITLE>", 1)
                        SUB_TITLE = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                        SUB_TITLE = TrimAll(SUB_TITLE)
                    Else
                        SUB_TITLE = ""
                    End If
                End If

                If InStr(UCase(str), "<AUTHOR1>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<AUTHOR1>", 1)
                    Tit1 = Tit1 + Len("<AUTHOR1>")
                    Tit2 = InStr(Tit1, str, "</AUTHOR1>", 1)
                    AUTHOR1 = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                    AUTHOR1 = TrimAll(AUTHOR1)
                Else
                    AUTHOR1 = ""
                End If

                If InStr(UCase(str), "<AUTHOR2>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<AUTHOR2>", 1)
                    Tit1 = Tit1 + Len("<AUTHOR2>")
                    Tit2 = InStr(Tit1, str, "</AUTHOR2>", 1)
                    AUTHOR2 = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                    AUTHOR2 = TrimAll(AUTHOR2)
                Else
                    AUTHOR2 = ""
                End If

                If InStr(UCase(str), "<AUTHOR3>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<AUTHOR3>", 1)
                    Tit1 = Tit1 + Len("<AUTHOR3>")
                    Tit2 = InStr(Tit1, str, "</AUTHOR3>", 1)
                    AUTHOR3 = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                    AUTHOR3 = TrimAll(AUTHOR3)
                Else
                    AUTHOR3 = ""
                End If

                If InStr(UCase(str), "<CORPORATE_AUTHOR>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<CORPORATE_AUTHOR>", 1)
                    Tit1 = Tit1 + Len("<CORPORATE_AUTHOR>")
                    Tit2 = InStr(Tit1, str, "</CORPORATE_AUTHOR>", 1)
                    CORPORATE_AUTHOR = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                    CORPORATE_AUTHOR = TrimAll(CORPORATE_AUTHOR)
                Else
                    CORPORATE_AUTHOR = ""
                End If

                Dim EDITOR, EDITION, PLACE_OF_PUB, PUB_ID, YEAR_OF_PUB As Object
                If InStr(UCase(str), "<EDITOR>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<EDITOR>", 1)
                    Tit1 = Tit1 + Len("<EDITOR>")
                    Tit2 = InStr(Tit1, str, "</EDITOR>", 1)
                    EDITOR = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                    EDITOR = TrimAll(EDITOR)
                Else
                    EDITOR = ""
                End If

                If InStr(UCase(str), "<EDITION>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<EDITION>", 1)
                    Tit1 = Tit1 + Len("<EDITION>")
                    Tit2 = InStr(Tit1, str, "</EDITION>", 1)
                    EDITION = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                    EDITION = TrimAll(EDITION)
                Else
                    EDITION = ""
                End If

                If InStr(UCase(str), "<PLACE_OF_PUB>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<PLACE_OF_PUB>", 1)
                    Tit1 = Tit1 + Len("<PLACE_OF_PUB>")
                    Tit2 = InStr(Tit1, str, "</PLACE_OF_PUB>", 1)
                    PLACE_OF_PUB = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                    PLACE_OF_PUB = TrimAll(PLACE_OF_PUB)
                Else
                    PLACE_OF_PUB = ""
                End If

                If InStr(UCase(str), "<YEAR_OF_PUB>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<YEAR_OF_PUB>", 1)
                    Tit1 = Tit1 + Len("<YEAR_OF_PUB>")
                    Tit2 = InStr(Tit1, str, "</YEAR_OF_PUB>", 1)
                    YEAR_OF_PUB = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                    YEAR_OF_PUB = TrimX(YEAR_OF_PUB)
                Else
                    YEAR_OF_PUB = Now.Year
                End If

                Dim FORMAT_CODE, SERIES_TITLE, SUB_ID, KEYWORDS, DATE_ADDED As Object
                If InStr(UCase(str), "<FORMAT_CODE>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<FORMAT_CODE>", 1)
                    Tit1 = Tit1 + Len("<FORMAT_CODE>")
                    Tit2 = InStr(Tit1, str, "</FORMAT_CODE>", 1)
                    FORMAT_CODE = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                Else
                    FORMAT_CODE = "PT"
                End If

                If InStr(UCase(str), "<SERIES_TITLE>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<SERIES_TITLE>", 1)
                    Tit1 = Tit1 + Len("<SERIES_TITLE>")
                    Tit2 = InStr(Tit1, str, "</SERIES_TITLE>", 1)
                    SERIES_TITLE = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                    SERIES_TITLE = TrimAll(SERIES_TITLE)
                Else
                    SERIES_TITLE = ""
                End If

                If InStr(UCase(str), "<SUB_NAME>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<SUB_NAME>", 1)
                    Tit1 = Tit1 + Len("<SUB_NAME>")
                    Tit2 = InStr(Tit1, str, "</SUB_NAME>", 1)
                    SUB_ID = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                Else
                    SUB_ID = ""
                End If

                If InStr(UCase(str), "<KEYWORDS>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<KEYWORDS>", 1)
                    Tit1 = Tit1 + Len("<KEYWORDS>")
                    Tit2 = InStr(Tit1, str, "</KEYWORDS>", 1)
                    KEYWORDS = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                    KEYWORDS = Replace(KEYWORDS, ";", "; ")
                    KEYWORDS = TrimAll(KEYWORDS)
                Else
                    KEYWORDS = ""
                End If

                Dim CAT_LEVEL, MULTI_VOL, TOTAL_VOL As Object
                If InStr(UCase(str), "<MULTI_VOL>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<MULTI_VOL>", 1)
                    Tit1 = Tit1 + Len("<MULTI_VOL>")
                    Tit2 = InStr(Tit1, str, "</MULTI_VOL>", 1)
                    MULTI_VOL = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                Else
                    MULTI_VOL = "N"
                End If

                If InStr(UCase(str), "<TOTAL_VOL>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<TOTAL_VOL>", 1)
                    Tit1 = Tit1 + Len("<TOTAL_VOL>")
                    Tit2 = InStr(Tit1, str, "</TOTAL_VOL>", 1)
                    TOTAL_VOL = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                Else
                    TOTAL_VOL = ""
                End If

                Dim PUB_NAME, LIB_CODE, CONF_NAME As Object
                If InStr(UCase(str), "<PUB_NAME>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<PUB_NAME>", 1)
                    Tit1 = Tit1 + Len("<PUB_NAME>")
                    Tit2 = InStr(Tit1, str, "</PUB_NAME>", 1)
                    PUB_NAME = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                    PUB_NAME = TrimAll(PUB_NAME)
                Else
                    PUB_NAME = ""
                End If

                If InStr(UCase(str), "<CONF_NAME>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<CONF_NAME>", 1)
                    Tit1 = Tit1 + Len("<CONF_NAME>")
                    Tit2 = InStr(Tit1, str, "</CONF_NAME>", 1)
                    CONF_NAME = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                    CONF_NAME = TrimAll(CONF_NAME)
                Else
                    CONF_NAME = ""
                End If

                Dim LANG_CODE
                If InStr(UCase(str), "<LANG_CODE>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<LANG_CODE>", 1)
                    Tit1 = Tit1 + Len("<LANG_CODE>")
                    Tit2 = InStr(Tit1, str, "</LANG_CODE>", 1)
                    LANG_CODE = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                Else
                    LANG_CODE = "ENG"
                End If

                Dim CON_CODE As Object = Nothing
                If InStr(UCase(str), "<CON_CODE>") <> 0 Then
                    Tit1 = InStr(1, UCase(str), "<CON_CODE>", 1)
                    Tit1 = Tit1 + Len("<CON_CODE>")
                    Tit2 = InStr(Tit1, str, "</CON_CODE>", 1)
                    CON_CODE = Trim(Mid(str, Tit1, (Tit2 - Tit1)))
                    CON_CODE = TrimAll(CON_CODE)
                Else
                    CON_CODE = "USA"
                End If

                If CON_CODE = "" Then
                    CON_CODE = "USA"
                End If


                '    Me.cat_AddNew_bttn_Click(Nothing, Nothing)
                '    'Me.cat_AddNew_bttn_Click(cat_AddNew_bttn, New System.EventArgs())
                '    DataCombo1.SelectedValue = Trim(BIB_CODE)
                '    DataCombo2.SelectedValue = Trim(MAT_CODE)
                '    DataCombo3.SelectedValue = Trim(DOC_TYPE_CODE)
                '    Combo_LangCode.SelectedValue = Trim(LANG_CODE)
                '    Combo_Format.SelectedValue = Trim(FORMAT_CODE)
                '    txt_cat_Title.Text = Trim(Title)
                '    txt_cat_SubTitle.Text = Trim(SUB_TITLE)
                '    CheckDuplicateTitle()

                '    Dim k1 As Integer
                '    If AUTHOR1 <> "" Then
                '        AUTHOR1 = TrimAll(Replace(AUTHOR1, ".", " "))
                '        AUTHOR1 = TrimAll(Replace(AUTHOR1, ",", ", "))
                '        AUTHOR1 = TrimAll(Replace(AUTHOR1, ";", ", "))
                '    End If
                '    k1 = Combo_Author1.FindString(AUTHOR1)
                '    If k1 <> -1 Then
                '        Combo_Author1.SelectedIndex = k1
                '    Else
                '        'save if new author is theres
                '        Dim objForm21 As New Authors()
                '        objForm21.Auth_dbSave(AUTHOR1, "", "", "", "", "")
                '    End If

                '    Dim k2 As Integer
                '    k2 = Combo_Author2.FindString(AUTHOR2)
                '    If k2 <> -1 Then
                '        Combo_Author2.SelectedIndex = k2
                '    Else
                '        Dim objForm212 As New Authors()
                '        objForm212.Auth_dbSave(AUTHOR2, "", "", "", "", "")
                '    End If

                '    Dim k3 As Integer
                '    k3 = Combo_Author3.FindString(AUTHOR3)
                '    If k3 <> -1 Then
                '        Combo_Author3.SelectedIndex = k3
                '    Else
                '        Dim objForm213 As New Authors()
                '        objForm213.Auth_dbSave(AUTHOR3, "", "", "", "", "")
                '    End If

                '    PopulateCombo_Author()
                '    Dim L1 As Integer
                '    Dim L2 As Integer
                '    Dim L3 As Integer
                '    L1 = Combo_Author1.FindString(AUTHOR1)
                '    Combo_Author1.SelectedIndex = L1
                '    L2 = Combo_Author2.FindString(AUTHOR2)
                '    Combo_Author2.SelectedIndex = L2
                '    L3 = Combo_Author3.FindString(AUTHOR3)
                '    Combo_Author3.SelectedIndex = L3

                '    txt_cat_MultiVol.Text = Trim(MULTI_VOL)
                '    txt_cat_TotalVol.Text = Trim(TOTAL_VOL)
                '    txt_cat_CorpAuthor.Text = Trim(CORPORATE_AUTHOR)
                '    txt_cat_Editor.Text = Trim(EDITOR)
                '    txt_cat_Edition.Text = Trim(EDITION)
                '    txt_cat_place.Text = Trim(PLACE_OF_PUB)
                '    txt_cat_PubYear.Text = Trim(YEAR_OF_PUB)
                '    txt_cat_StdNo.Text = Trim(STANDARD_NO)

                '    'PUB_NAME = "zzzz"
                '    Dim p As Integer
                '    p = Combo_PubName.FindString(PUB_NAME)
                '    If p <> -1 Then
                '        Combo_PubName.SelectedIndex = p
                '    Else
                '        Dim PubForm As New Publisher_Directory
                '        PubForm.txt_Pub_Name1.Text = PUB_NAME
                '        PubForm.pub_dbSave(MDIForm1.userCode, PUB_NAME, PLACE_OF_PUB, "", "", "", "", "", "", "", MDIForm1.libCode)

                '        PopulateCombo_Publishers()
                '        Dim p1 As Integer
                '        p1 = Combo_PubName.FindString(PUB_NAME)
                '        Combo_PubName.SelectedIndex = p1
                '    End If


                '    'SUB_NAME = "zzzz"
                '    Dim X As Integer
                '    X = DataCombo_sub.FindString(SUB_ID)
                '    If X <> -1 Then
                '        DataCombo_sub.SelectedIndex = X
                '    Else
                '        Dim SubForm As New Subjects
                '        SubForm.txt_Sub_Name1.Text = SUB_ID
                '        SubForm.sub_dbSave(MDIForm1.userCode, "X", SUB_ID, "", "")

                '        PopulateCombo_SubCode()
                '        Dim p1 As Integer
                '        p1 = DataCombo_sub.FindString(SUB_ID)
                '        DataCombo_sub.SelectedIndex = p1
                '    End If

                '    txt_cat_keyword.Text = Trim(KEYWORDS)
                '    txt_cat_series.Text = Trim(SERIES_TITLE)
                '    txt_Cat_Conf.Text = Trim(CONF_NAME)
                '    ComboBox1.SelectedValue = CON_CODE
                '    find = 1



                '    'write data on the form



























            Else
                If Find <> 0 Then
                    'MsgBox("NO RECORD EXISTS")
                End If
            End If

        Catch s As Exception

        Finally

        End Try

    End Sub
    'call event to download record from interenet
    Protected Sub txt_Cat_ISBN_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txt_Cat_ISBN.TextChanged
        If CheckBox2.Checked = True Then
            If Acq_Save_Bttn.Visible = True Then
                DownloadRecord_ISBN()
            End If
        Else
            Me.txt_Cat_Title.Focus()
        End If
    End Sub




   
   
End Class