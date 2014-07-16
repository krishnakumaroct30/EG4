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

Public Class BasicSearch
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
                    LibCode = Session.Item("LoggedLibcode")
                    If Page.IsPostBack = False Then
                        GetUserSettings()
                        GetLibrarySettings()
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
                        PopulateCurrencies()
                        DDL_Currencies.SelectedValue = "INR"
                        PopulateAcqModes()
                        DDL_AcqModes.SelectedValue = "P"
                        PopulateVendors()


                        'holdings
                        PopulateSections()
                        PopulateLibrary()
                        PopulateAccMaterials()
                        PopulateBindings()
                        PopulateFormats()
                        PopulateSections()
                        PopulateStatus()
                        DDL_Format.SelectedValue = "PT"
                        DDL_Show.SelectedValue = "Y"
                        DDL_Issuable.SelectedValue = "Y"
                        DDL_CollectionType.SelectedValue = "C"
                        DDL_Status.SelectedValue = "1"
                        DDL_Binding.SelectedValue = "P"

                    End If
                    Dim CreateSUserButton As Object 'System.Web.UI.WebControls.Button
                    CreateSUserButton = Master.FindControl("Accordion1").FindControl("SearchPane").FindControl("Basic_Search_bttn")
                    CreateSUserButton.ForeColor = Drawing.Color.Red
                    myPaneName = "SearchPane"
                End If
            Else
                Response.Redirect("~/Default.aspx", False)
            End If
            LibCode = Session.Item("LoggedLibcode")
            DDL_Library.SelectedValue = LibCode

        Catch ex As Exception
            Label6.Text = "Error in this Page!"
            Label15.Text = ""
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
    Public Sub GetLibrarySettings()
        Dim Command As SqlCommand = Nothing
        Dim dr As SqlDataReader = Nothing
        Try
            'get record details from database
            Dim SQL As String = Nothing
            SQL = "SELECT LIB_ID, ACQ_RETRO FROM LIBRARIES WHERE (LIB_CODE = '" & TrimX(LibCode) & "') "
            Command = New SqlCommand(SQL, SqlConn)
            SqlConn.Open()
            dr = Command.ExecuteReader(CommandBehavior.CloseConnection)
            dr.Read()
            If dr.HasRows = True Then
                If dr.Item("ACQ_RETRO").ToString <> "" Then
                    AcqRetro = dr.Item("ACQ_RETRO").ToString
                Else
                    AcqRetro = "N"
                End If
            End If
        Catch s As Exception
            If dr IsNot Nothing Then
                dr.Close()
            Else
                SqlConn.Close()
            End If
        Finally
            SqlConn.Close()
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
            SQL = "SELECT DEF_ID, CAT_FORMAT,HOLD_FORMAT,DOC_TYPE_NAME  FROM DEFORMATS WHERE (LIB_CODE = '" & Trim(LibCode) & "'  AND DOC_TYPE_CODE = '" & Trim(myDocName) & "') "
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
                        TR_LANG.Visible = True
                        TR_TITLE.Visible = True
                        TR_CONTENTS.Visible = True
                        TR_PHOTO.Visible = True

                        TR_ACQMODE.Visible = True
                        TR_CONVERSION.Visible = True
                        TR_VENDOR.Visible = True

                        TR_ACCSERIES.Visible = True
                        TR_LIBRARY.Visible = True
                        TR_HOLDREMARKS.Visible = True

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
                        'Label6.Text = "Data Entry Format is not proper..Create Data Entry Format for this Document Type in Library Administrator Module"
                        ' Label15.Text = ""
                    End If
                Else
                    'Label6.Text = "Data Entry Format has not been added..Create Data Entry Format for this Document Type in Library Administrator Module"
                    'Label15.Text = ""
                End If
            Else
                Label6.Text = "Data Entry Format has not been added..Create Data Entry Format for this Document Type in Library Administrator Module"
                Label15.Text = ""

                TR_LANG.Visible = False
                TR_TITLE.Visible = False
                TR_CONTENTS.Visible = False
                TR_PHOTO.Visible = False

                TR_ACQMODE.Visible = False
                TR_CONVERSION.Visible = False
                TR_VENDOR.Visible = False

                TR_ACCSERIES.Visible = False
                TR_LIBRARY.Visible = False
                TR_HOLDREMARKS.Visible = False

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
                TR_ACT.Visible = False
            End If
            SqlConn.Close()
            DDL_Doc_Type.Focus()
            If Label7.Text = "" Then
                LoadHoldFormat()
            End If
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
            Label15.Text = ""
        Finally
            SqlConn.Close()
        End Try

    End Sub
    Public Sub LoadHoldFormat()
        Dim Command As SqlCommand = Nothing
        Dim dt As DataTable = Nothing
        Try
            Dim myHoldFields As Object = Nothing
            Dim myDocName As Object = Nothing
            If DDL_Doc_Type.Text <> "" Then
                myDocName = DDL_Doc_Type.SelectedValue
            Else
                Exit Sub
            End If

            Dim SQL As String = Nothing
            SQL = "SELECT DEF_ID, HOLD_FORMAT, DOC_TYPE_NAME  FROM DEFORMATS WHERE (LIB_CODE = '" & Trim(LibCode) & "'  AND DOC_TYPE_CODE = '" & Trim(myDocName) & "') "
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
                If dt.Rows(0).Item("HOLD_FORMAT").ToString <> "" Then
                    myHoldFields = dt.Rows(0).Item("HOLD_FORMAT")
                    If myHoldFields <> "" Then
                        Table2.Visible = True

                        If InStr(myHoldFields, "CLASS_NO") <> 0 Then
                            TR_CLASS_NO.Visible = True
                        Else
                            TR_CLASS_NO.Visible = False
                        End If

                        If InStr(myHoldFields, "PAGINATION") <> 0 Then
                            TR_PAGINATION.Visible = True
                        Else
                            TR_PAGINATION.Visible = False
                        End If

                        If InStr(myHoldFields, "ILLUSTRATION") <> 0 Then
                            TR_ILLUSTRATION.Visible = True
                        Else
                            TR_ILLUSTRATION.Visible = False
                        End If

                        If InStr(myHoldFields, "SIZE") <> 0 Then
                            TR_SIZE.Visible = True
                        Else
                            TR_SIZE.Visible = False
                        End If

                        If InStr(myHoldFields, "COLLECTION_TYPE") <> 0 Then
                            TR_COLLECTION_TYPE.Visible = True
                        Else
                            TR_COLLECTION_TYPE.Visible = False
                        End If
                        If InStr(myHoldFields, "PHYSICAL_LOCATION") <> 0 Then
                            TR_PHYSICAL_LOCATION.Visible = True
                        Else
                            TR_PHYSICAL_LOCATION.Visible = False
                        End If
                        If InStr(myHoldFields, "STA_CODE") <> 0 Then
                            TR_STA_CODE.Visible = True
                        Else
                            TR_STA_CODE.Visible = False
                        End If
                        If InStr(myHoldFields, "BIND_CODE") <> 0 Then
                            TR_BIND_CODE.Visible = True
                        Else
                            TR_BIND_CODE.Visible = False
                        End If
                        If InStr(myHoldFields, "ACC_MAT_CODE") <> 0 Then
                            TR_ACC_MAT_CODE.Visible = True
                        Else
                            TR_ACC_MAT_CODE.Visible = False
                        End If
                        If InStr(myHoldFields, "SEC_CODE") <> 0 Then
                            TR_SEC_CODE.Visible = True
                        Else
                            TR_SEC_CODE.Visible = False
                        End If
                        If InStr(myHoldFields, "FORMAT_CODE") <> 0 Then
                            TR_FORMAT_CODE.Visible = True
                        Else
                            TR_FORMAT_CODE.Visible = False
                        End If

                        If InStr(myHoldFields, "REFERENCE_NO") <> 0 Then
                            TR_REFERENCE_NO.Visible = True
                        Else
                            TR_REFERENCE_NO.Visible = False
                        End If
                        If InStr(myHoldFields, "REMARKS") <> 0 Then
                            TR_HOLDREMARKS.Visible = True
                        Else
                            TR_HOLDREMARKS.Visible = False
                        End If
                        If InStr(myHoldFields, "MEDIUM") <> 0 Then
                            TR_MEDIUM.Visible = True
                        Else
                            TR_MEDIUM.Visible = False
                        End If
                        If InStr(myHoldFields, "RECORDING_CATEGORY") <> 0 Then
                            TR_RECORDING_CATEGORY.Visible = True
                        Else
                            TR_RECORDING_CATEGORY.Visible = False
                        End If
                        If InStr(myHoldFields, "RECORDING_FORM") <> 0 Then
                            TR_RECORDING_FORM.Visible = True
                        Else
                            TR_RECORDING_FORM.Visible = False
                        End If
                        If InStr(myHoldFields, "RECORDING_FORMAT") <> 0 Then
                            TR_RECORDING_FORMAT.Visible = True
                        Else
                            TR_RECORDING_FORMAT.Visible = False
                        End If
                        If InStr(myHoldFields, "RECORDING_SPEED") <> 0 Then
                            TR_RECORDING_SPEED.Visible = True
                        Else
                            TR_RECORDING_SPEED.Visible = False
                        End If
                        If InStr(myHoldFields, "RECORDING_STORAGE_TECH") <> 0 Then
                            TR_RECORDING_STORAGE_TECH.Visible = True
                        Else
                            TR_RECORDING_STORAGE_TECH.Visible = False
                        End If
                        If InStr(myHoldFields, "RECORDING_PLAY_DURATION") <> 0 Then
                            TR_RECORDING_PLAY_DURATION.Visible = True
                        Else
                            TR_RECORDING_PLAY_DURATION.Visible = False
                        End If
                        If InStr(myHoldFields, "VIDEO_TYPEOFVISUAL") <> 0 Then
                            TR_VIDEO_TYPEOFVISUAL.Visible = True
                        Else
                            TR_VIDEO_TYPEOFVISUAL.Visible = False
                        End If
                        If InStr(myHoldFields, "VIDEO_COLOR") <> 0 Then
                            TR_VIDEO_COLOR.Visible = True
                        Else
                            TR_VIDEO_COLOR.Visible = False
                        End If
                        If InStr(myHoldFields, "PLAYBACK_CHANNELS") <> 0 Then
                            TR_PLAYBACK_CHANNELS.Visible = True
                        Else
                            TR_PLAYBACK_CHANNELS.Visible = False
                        End If
                        If InStr(myHoldFields, "TAPE_WIDTH") <> 0 Then
                            TR_TAPE_WIDTH.Visible = True
                        Else
                            TR_TAPE_WIDTH.Visible = False
                        End If
                        If InStr(myHoldFields, "TAPE_CONFIGURATION") <> 0 Then
                            TR_TAPE_CONFIGURATION.Visible = True
                        Else
                            TR_TAPE_CONFIGURATION.Visible = False
                        End If
                        If InStr(myHoldFields, "KIND_OF_DISK") <> 0 Then
                            TR_KIND_OF_DISK.Visible = True
                        Else
                            TR_KIND_OF_DISK.Visible = False
                        End If
                        If InStr(myHoldFields, "KIND_OF_CUTTING") <> 0 Then
                            TR_KIND_OF_CUTTING.Visible = True
                        Else
                            TR_KIND_OF_CUTTING.Visible = False
                        End If
                        If InStr(myHoldFields, "ENCODING_STANDARD") <> 0 Then
                            TR_ENCODING_STANDARD.Visible = True
                        Else
                            TR_ENCODING_STANDARD.Visible = False
                        End If
                        If InStr(myHoldFields, "CAPTURE_TECHNIQUE") <> 0 Then
                            TR_CAPTURE_TECHNIQUE.Visible = True
                        Else
                            TR_CAPTURE_TECHNIQUE.Visible = False
                        End If
                        If InStr(myHoldFields, "CARTOGRAPHIC_MEDIUM") <> 0 Then
                            TR_CARTOGRAPHIC_MEDIUM.Visible = True
                        Else
                            TR_CARTOGRAPHIC_MEDIUM.Visible = False
                        End If
                        If InStr(myHoldFields, "CARTOGRAPHIC_COORDINATES") <> 0 Then
                            TR_CARTOGRAPHIC_COORDINATES.Visible = True
                        Else
                            TR_CARTOGRAPHIC_COORDINATES.Visible = False
                        End If
                        If InStr(myHoldFields, "CARTOGRAPHIC_GEOGRAPHIC_LOCATION") <> 0 Then
                            TR_CARTOGRAPHIC_GEOGRAPHIC_LOCATION.Visible = True
                        Else
                            TR_CARTOGRAPHIC_GEOGRAPHIC_LOCATION.Visible = False
                        End If
                        If InStr(myHoldFields, "CARTOGRAPHIC_DATAGATHERING_DATE") <> 0 Then
                            TR_CARTOGRAPHIC_DATAGATHERING_DATE.Visible = True
                        Else
                            TR_CARTOGRAPHIC_DATAGATHERING_DATE.Visible = False
                        End If
                        If InStr(myHoldFields, "CARTOGRAPHIC_COMPILATION_DATE") <> 0 Then
                            TR_CARTOGRAPHIC_COMPILATION_DATE.Visible = True
                        Else
                            TR_CARTOGRAPHIC_COMPILATION_DATE.Visible = False
                        End If
                        If InStr(myHoldFields, "CARTOGRAPHIC_INSPECTION_DATE") <> 0 Then
                            TR_CARTOGRAPHIC_INSPECTION_DATE.Visible = True
                        Else
                            TR_CARTOGRAPHIC_INSPECTION_DATE.Visible = False
                        End If
                        If InStr(myHoldFields, "CARTOGRAPHIC_SCALE") <> 0 Then
                            TR_CARTOGRAPHIC_SCALE.Visible = True
                        Else
                            TR_CARTOGRAPHIC_SCALE.Visible = False
                        End If
                        If InStr(myHoldFields, "CARTOGRAPHIC_PROJECTION") <> 0 Then
                            TR_CARTOGRAPHIC_PROJECTION.Visible = True
                        Else
                            TR_CARTOGRAPHIC_PROJECTION.Visible = False
                        End If
                        If InStr(myHoldFields, "CREATION_DATE") <> 0 Then
                            TR_CREATION_DATE.Visible = True
                        Else
                            TR_CREATION_DATE.Visible = False
                        End If
                        If InStr(myHoldFields, "PHOTO_NO") <> 0 Then
                            TR_PHOTO_NO.Visible = True
                        Else
                            TR_PHOTO_NO.Visible = False
                        End If
                        If InStr(myHoldFields, "PHOTO_ALBUM_NO") <> 0 Then
                            TR_PHOTO_ALBUM_NO.Visible = True
                        Else
                            TR_PHOTO_ALBUM_NO.Visible = False
                        End If
                        If InStr(myHoldFields, "PHOTO_OCASION") <> 0 Then
                            TR_PHOTO_OCASION.Visible = True
                        Else
                            TR_PHOTO_OCASION.Visible = False
                        End If

                        If InStr(myHoldFields, "IMAGE_VIEW_TYPE") <> 0 Then
                            TR_IMAGE_VIEW_TYPE.Visible = True
                        Else
                            TR_IMAGE_VIEW_TYPE.Visible = False
                        End If

                        If InStr(myHoldFields, "VIEW_DATE") <> 0 Then
                            TR_VIEW_DATE.Visible = True
                        Else
                            TR_VIEW_DATE.Visible = False
                        End If

                        If InStr(myHoldFields, "THEME") <> 0 Then
                            TR_THEME.Visible = True
                        Else
                            TR_THEME.Visible = False
                        End If

                        If InStr(myHoldFields, "STYLE") <> 0 Then
                            TR_STYLE.Visible = True
                        Else
                            TR_STYLE.Visible = False
                        End If

                        If InStr(myHoldFields, "CULTURE") <> 0 Then
                            TR_CULTURE.Visible = True
                        Else
                            TR_CULTURE.Visible = False
                        End If

                        If InStr(myHoldFields, "CURRENT_STIE") <> 0 Then
                            TR_CURRENT_SITE.Visible = True
                        Else
                            TR_CURRENT_SITE.Visible = False
                        End If

                        If InStr(myHoldFields, "CREATION_SITE") <> 0 Then
                            TR_CREATION_SITE.Visible = True
                        Else
                            TR_CREATION_SITE.Visible = False
                        End If

                        If InStr(myHoldFields, "YARN_COUNT") <> 0 Then
                            TR_YARNCOUNT.Visible = True
                        Else
                            TR_YARNCOUNT.Visible = False
                        End If

                        If InStr(myHoldFields, "MATERIAL_TYPE") <> 0 Then
                            TR_MATERIAL_TYPE.Visible = True
                        Else
                            TR_MATERIAL_TYPE.Visible = False
                        End If

                        If InStr(myHoldFields, "TECHNIQUE") <> 0 Then
                            TR_TECHNIQUE.Visible = True
                        Else
                            TR_TECHNIQUE.Visible = False
                        End If

                        If InStr(myHoldFields, "TECH_DETAILS") <> 0 Then
                            TR_TECH_DETAILS.Visible = True
                        Else
                            TR_TECH_DETAILS.Visible = False
                        End If

                        If InStr(myHoldFields, "INSCRIPTIONS") <> 0 Then
                            TR_INSCRIPTIONS.Visible = True
                        Else
                            TR_INSCRIPTIONS.Visible = False
                        End If

                        If InStr(myHoldFields, "DESCRIPTION") <> 0 Then
                            TR_DESCRIPTION.Visible = True
                        Else
                            TR_DESCRIPTION.Visible = False
                        End If

                        If InStr(myHoldFields, "GLOBE_TYPE") <> 0 Then
                            TR_GLOBE_TYPE.Visible = True
                        Else
                            TR_GLOBE_TYPE.Visible = False
                        End If

                        If InStr(myHoldFields, "ALTER_DATE") <> 0 Then
                            TR_ALTER_DATE.Visible = True
                        Else
                            TR_ALTER_DATE.Visible = False
                        End If

                        If DDL_YesNo.Text = "Y" Then
                            TR_VOL_NO.Visible = True
                            TR_VOL_EDITORS.Visible = True
                            TR_VOL_TITLE.Visible = True
                        Else
                            TR_VOL_NO.Visible = False
                            TR_VOL_EDITORS.Visible = False
                            TR_VOL_TITLE.Visible = False
                        End If
                    Else
                        Table2.Visible = False
                    End If
                Else
                   Table2.Visible = False
                End If
            Else
                Table2.Visible = False
            End If
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
            Label15.Text = ""
        Finally
            Command.Dispose()
            dt.Dispose()
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
                Grid1_Search.DataBind()
                Label1.Text = "Total Record(s): 0 "
            Else
                Grid1_Search.Visible = True
                RecordCount = dtSearch.Rows.Count
                Grid1_Search.DataSource = dtSearch
                Grid1_Search.DataBind()
                Label1.Text = "Total Record(s): " & RecordCount
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
        ' Me.Label7.Text = ""
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

        DDL_AcqModes.ClearSelection()
        DDL_Currencies.ClearSelection()
        DDL_Vendors.ClearSelection()
        txt_Acq_ItemPrice.Text = ""
        txt_Acq_ConversionRate.Text = ""
        txt_Acq_ItemRupees.Text = ""
        txt_Acq_OtherCharges.Text = ""

        Me.txt_Hold_AccDate.Text = ""
        Me.txt_Hold_AccNo.Text = ""
        Me.txt_Hold_BookNo.Text = ""
        Me.txt_Hold_ClassNo.Text = ""
        Me.txt_Hold_CompilationDate.Text = ""
        Me.txt_Hold_Coordinates.Text = ""
        Me.txt_Hold_CopyISBN.Text = ""
        Me.txt_Hold_CreationDate.Text = ""
        Me.txt_Hold_DataGatheringDate.Text = ""
        Me.txt_Hold_GeographicLocation.Text = ""
        Me.txt_Hold_InspectionDate.Text = ""
        Me.txt_Hold_Location.Text = ""
        Me.txt_Hold_Pages.Text = ""
        Me.txt_Hold_Projection.Text = ""
        Me.txt_Hold_RecordingCategory.Text = ""
        Me.txt_Hold_RecordingDuration.Text = ""
        Me.txt_Hold_RecordingForm.Text = ""
        Me.txt_Hold_RecordingFormat.Text = ""
        Me.txt_Hold_RecordingMedium.Text = ""
        Me.txt_Hold_RecordingSpeed.Text = ""
        Me.txt_Hold_RecordingStorageTech.Text = ""
        Me.txt_Hold_ReferenceNo.Text = ""
        Me.txt_Hold_Remarks.Text = ""
        Me.txt_Hold_Scale.Text = ""
        Me.txt_Hold_Size.Text = ""
        Me.txt_Hold_TypeOfVisuals.Text = ""
        Me.txt_Hold_VolEditors.Text = ""
        Me.txt_Hold_VolNo.Text = ""
        Me.txt_Hold_VolTitle.Text = ""
        Me.txt_Hold_VolYear.Text = ""

        txt_Hold_Color.Text = ""
        txt_Hold_PlayBackChannel.Text = ""
        txt_Hold_TapeWidth.Text = ""
        txt_Hold_TapeConfiguration.Text = ""
        txt_Hold_KindofDisk.Text = ""
        txt_Hold_KindofCutting.Text = ""
        txt_Hold_EncodingStandard.Text = ""
        txt_Hold_CaptureTechnique.Text = ""
        txt_Hold_PhotoNo.Text = ""
        txt_Hold_PhotoAlbumNo.Text = ""
        txt_Hold_Color.Text = ""
        txt_Hold_Ocasion.Text = ""
        txt_Hold_ImageViewType.Text = ""
        txt_Hold_ViewDate.Text = ""
        txt_Hold_Theme.Text = ""
        txt_Hold_Style.Text = ""
        txt_Hold_Culture.Text = ""
        txt_Hold_CurrentSite.Text = ""
        txt_Hold_CreationSite.Text = ""
        txt_Hold_YarnCount.Text = ""
        txt_Hold_MaterialsType.Text = ""
        txt_Hold_Technique.Text = ""
        txt_Hold_TechDetails.Text = ""
        txt_Hold_Inscription.Text = ""
        txt_Hold_Description.Text = ""
        txt_Hold_GlobeType.Text = ""
        txt_Hold_AlterDate.Text = ""
        DDL_AccMaterials.ClearSelection()
        Me.DDL_GeographicMedium.ClearSelection()
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

                Label_CatLevel.Text = dt.Rows(0).Item("CAT_LEVEL").ToString

                If dt.Rows(0).Item("LANG_CODE").ToString <> "" Then
                    Me.DDL_Lang.SelectedValue = dt.Rows(0).Item("LANG_CODE").ToString
                Else
                    DDL_Lang.SelectedValue = "ENG"
                End If
                If dt.Rows(0).Item("MULTI_VOL").ToString <> "" Then
                    Me.DDL_YesNo.SelectedValue = dt.Rows(0).Item("MULTI_VOL").ToString
                    TR_VOL_NO.Visible = True
                    TR_VOL_EDITORS.Visible = True
                    TR_VOL_TITLE.Visible = True
                Else
                    DDL_YesNo.ClearSelection()
                    TR_VOL_NO.Visible = False
                    TR_VOL_EDITORS.Visible = False
                    TR_VOL_TITLE.Visible = False
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
                SqlConn.Close()

                CheckBox1.Visible = True
                CheckBox1.Checked = False
                Label15.Text = "Press UPDATE Button to save the Changes if any.."
                Label6.Text = ""
                Table2.Visible = False
                Table4.Visible = False
            Else
                Label6.Text = "No Record to Edit... "
                Label7.Text = ""
                Label15.Text = ""
                Label_CatLevel.Text = ""
               
                CheckBox1.Visible = False
                CheckBox1.Checked = False
                Table2.Visible = True
                If AcqRetro = "Y" Then
                    Table4.Visible = True
                Else
                    Table4.Visible = False
                End If
            End If
            PopulateHoldingsGrid()
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
    Public Sub DisplayAcq()
        Dim dt As DataTable
        Try
            Dim CAT_NO As Long = Nothing
            If Label_ACQID.Text <> "" Then
                CAT_NO = Label7.Text
                'get record details from database
                Dim SQL As String = Nothing
                SQL = "SELECT *  FROM ACQUISITIONS WHERE (CAT_NO = '" & Trim(CAT_NO) & "') AND (LIB_CODE = '" & Trim(LibCode) & "')  AND (ACQ_ID = '" & Trim(Label_ACQID.Text) & "')"

                Dim ds As New DataSet
                Dim da As New SqlDataAdapter(SQL, SqlConn)
                If SqlConn.State = 0 Then
                    SqlConn.Open()
                End If
                da.Fill(ds)

                dt = ds.Tables(0).Copy

                If dt.Rows.Count <> 0 Then
                    Table4.Visible = True
                    Label_ProcessStatus.Text = dt.Rows(0).Item("PROCESS_STATUS").ToString
                    If dt.Rows(0).Item("ACQMODE_CODE").ToString <> "" Then
                        Me.DDL_AcqModes.SelectedValue = dt.Rows(0).Item("ACQMODE_CODE").ToString
                    Else
                        DDL_AcqModes.ClearSelection()
                    End If

                    If dt.Rows(0).Item("CUR_CODE").ToString <> "" Then
                        Me.DDL_Currencies.SelectedValue = dt.Rows(0).Item("CUR_CODE").ToString
                    Else
                        DDL_Currencies.ClearSelection()
                    End If

                    If dt.Rows(0).Item("VEND_ID").ToString <> "" Then
                        Me.DDL_Vendors.SelectedValue = dt.Rows(0).Item("VEND_ID").ToString
                    Else
                        DDL_Vendors.ClearSelection()
                    End If

                    If dt.Rows(0).Item("ITEM_PRICE").ToString <> "" Then
                        Me.txt_Acq_ItemPrice.Text = dt.Rows(0).Item("ITEM_PRICE").ToString
                    Else
                        txt_Acq_ItemPrice.Text = ""
                    End If

                    If dt.Rows(0).Item("CONVERSION_RATE").ToString <> "" Then
                        Me.txt_Acq_ConversionRate.Text = dt.Rows(0).Item("CONVERSION_RATE").ToString
                    Else
                        txt_Acq_ConversionRate.Text = ""
                    End If

                    If dt.Rows(0).Item("ITEM_RUPEES").ToString <> "" Then
                        Me.txt_Acq_ItemRupees.Text = dt.Rows(0).Item("ITEM_RUPEES").ToString
                    Else
                        txt_Acq_ItemRupees.Text = ""
                    End If

                    If dt.Rows(0).Item("OTHER_CHARGES").ToString <> "" Then
                        Me.txt_Acq_OtherCharges.Text = dt.Rows(0).Item("OTHER_CHARGES").ToString
                    Else
                        txt_Acq_OtherCharges.Text = ""
                    End If
                Else
                    If AcqRetro = "Y" Then
                        Table4.Visible = True
                    Else
                        Table4.Visible = False
                    End If
                    Label_ProcessStatus.Text = ""
                End If
            Else
                If AcqRetro = "Y" Then
                    Table4.Visible = True
                Else
                    Table4.Visible = False
                End If
                Label_ProcessStatus.Text = ""
            End If
        Catch ex As Exception
            Label6.Text = "Error in Displaying the Record!"
            Label15.Text = ""
            Label_ProcessStatus.Text = ""
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'fill Grid with holdings
    Public Sub PopulateHoldingsGrid()
        Dim dtSearch As DataTable = Nothing
        Try
            If Label7.Text <> "" Then
                Dim SQL As String = Nothing
                SQL = "SELECT HOLD_ID, CAT_NO, ACQ_ID, ACCESSION_NO, ACCESSION_DATE, VOL_NO, CLASS_NO, BOOK_NO, PAGINATION, PHYSICAL_LOCATION, COLLECTION_TYPE, LIB_CODE FROM HOLDINGS WHERE (CAT_NO = '" & Label7.Text & "')   AND (LIB_CODE = '" & Trim(LibCode) & "') ORDER BY HOLD_ID DESC"

                Dim ds As New DataSet
                Dim da As New SqlDataAdapter(SQL, SqlConn)
                If SqlConn.State = 0 Then
                    SqlConn.Open()
                End If
                da.Fill(ds)

                dtSearch = ds.Tables(0).Copy
                Dim RecordCount As Long = 0
                If dtSearch.Rows.Count = 0 Then
                    Me.Grid2.DataSource = Nothing
                    Grid2.DataBind()
                Else
                    Grid2.Visible = True
                    RecordCount = dtSearch.Rows.Count
                    Grid2.DataSource = dtSearch
                    Grid2.DataBind()
                End If
                ViewState("dt") = dtSearch
            Else
                Me.Grid2.DataSource = Nothing
                Grid2.DataBind()
            End If
            UpdatePanel1.Update()
        Catch s As Exception
            Label16.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'grid view page index changing event
    Protected Sub Grid2_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles Grid2.PageIndexChanging
        Try
            'rebind datagrid
            Grid2.DataSource = ViewState("dt") 'temp
            Grid2.PageIndex = e.NewPageIndex
            index = e.NewPageIndex * Grid2.PageSize
            Grid2.DataBind()
        Catch s As Exception
            Label16.Text = "Error:  there is error in page index !"
        End Try
    End Sub
    'gridview sorting event
    Protected Sub Grid2_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles Grid2.Sorting
        Dim temp As DataTable = CType(ViewState("dt"), DataTable)
        GridViewSortExpression = e.SortExpression
        Grid2.DataSource = temp
        Dim pageIndex As Integer = Grid2.PageIndex
        Grid2.DataSource = SortDataTable(Grid2.DataSource, False)
        Grid2.DataBind()
        Grid2.PageIndex = pageIndex
    End Sub
    Protected Sub Grid2_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Grid2.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Lblsr As Label = e.Row.FindControl("lblsr")
            Srno = index + 1
            If Lblsr Is Nothing = False Then
                Lblsr.Text = Srno
                index += 1
            End If
        End If
    End Sub
    'get value of row from grid
    Private Sub Grid2_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles Grid2.RowCommand
        Dim Command As SqlCommand = Nothing
        Dim dr As SqlDataReader = Nothing
        Try
            If e.CommandName = "Select" Then
                Dim myRowID, HOLD_ID As Integer
                myRowID = e.CommandArgument.ToString()
                HOLD_ID = Grid2.DataKeys(myRowID).Value
                If Not String.IsNullOrEmpty(HOLD_ID) And HOLD_ID <> 0 Then
                    Dim iloop As Integer
                    Dim strcurrentchar As Object
                    Dim c As Integer
                    Dim counter1 As Integer

                    HOLD_ID = TrimX(HOLD_ID)
                    HOLD_ID = UCase(HOLD_ID)

                    HOLD_ID = RemoveQuotes(HOLD_ID)
                    If Len(HOLD_ID).ToString > 10 Then
                        Label15.Text = "Length of Input is not Proper!"
                        Label1.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Length of Input is not Proper... ');", True)
                        Exit Sub
                    End If
                    HOLD_ID = " " & HOLD_ID & " "
                    If InStr(1, HOLD_ID, " CREATE ", 1) > 0 Or InStr(1, HOLD_ID, " DELETE ", 1) > 0 Or InStr(1, HOLD_ID, " DROP ", 1) > 0 Or InStr(1, HOLD_ID, " INSERT ", 1) > 1 Or InStr(1, HOLD_ID, " TRACK ", 1) > 1 Or InStr(1, HOLD_ID, " TRACE ", 1) > 1 Then
                        Label15.Text = "Do not use reserve words... !"
                        Label1.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Do not use reserve words... ');", True)
                        Exit Sub
                    End If
                    HOLD_ID = TrimX(HOLD_ID)
                    'get record details from database
                    Dim SQL As String = Nothing
                    SQL = " SELECT *  FROM HOLDINGS WHERE (HOLD_ID = '" & Trim(HOLD_ID) & "') AND (LIB_CODE = '" & Trim(LibCode) & "')"
                    Command = New SqlCommand(SQL, SqlConn)
                    SqlConn.Open()
                    dr = Command.ExecuteReader(CommandBehavior.CloseConnection)
                    dr.Read()
                    'ClearFields()

                    If dr.HasRows = True Then

                        If dr.Item("ACQ_ID").ToString <> "" Then
                            Label_ACQID.Text = dr.Item("ACQ_ID").ToString
                        Else
                            Label_ACQID.Text = ""
                        End If

                        If dr.Item("HOLD_ID").ToString <> "" Then
                            Label_HoldID.Text = dr.Item("HOLD_ID").ToString
                        Else
                            Label_HoldID.Text = ""
                        End If

                        If dr.Item("ACCESSION_NO").ToString <> "" Then
                            txt_Hold_AccNo.Text = dr.Item("ACCESSION_NO").ToString
                        Else
                            txt_Hold_AccNo.Text = ""
                        End If

                        If dr.Item("ACCESSION_DATE").ToString <> "" Then
                            txt_Hold_AccDate.Text = Format(dr.Item("ACCESSION_DATE"), "dd/MM/yyyy")
                        Else
                            txt_Hold_AccDate.Text = ""
                        End If

                        If dr.Item("FORMAT_CODE").ToString <> "" Then
                            DDL_Format.SelectedValue = dr.Item("FORMAT_CODE").ToString
                        Else
                            DDL_Format.ClearSelection()
                        End If

                        If dr.Item("SHOW").ToString <> "" Then
                            DDL_Show.SelectedValue = dr.Item("SHOW").ToString
                        Else
                            DDL_Show.ClearSelection()
                        End If

                        If dr.Item("ISSUEABLE").ToString <> "" Then
                            DDL_Issuable.SelectedValue = dr.Item("ISSUEABLE").ToString
                        Else
                            DDL_Issuable.ClearSelection()
                        End If

                        If dr.Item("VOL_NO").ToString <> "" Then
                            txt_Hold_VolNo.Text = dr.Item("VOL_NO").ToString
                        Else
                            txt_Hold_VolNo.Text = ""
                        End If

                        If dr.Item("VOL_YEAR").ToString <> "" Then
                            txt_Hold_VolYear.Text = dr.Item("VOL_YEAR").ToString
                        Else
                            txt_Hold_VolYear.Text = ""
                        End If

                        If dr.Item("VOL_EDITORS").ToString <> "" Then
                            txt_Hold_VolEditors.Text = dr.Item("VOL_EDITORS").ToString
                        Else
                            txt_Hold_VolEditors.Text = ""
                        End If

                        If dr.Item("VOL_TITLE").ToString <> "" Then
                            txt_Hold_VolTitle.Text = dr.Item("VOL_TITLE").ToString
                        Else
                            txt_Hold_VolTitle.Text = ""
                        End If

                        If dr.Item("COPY_ISBN").ToString <> "" Then
                            txt_Hold_CopyISBN.Text = dr.Item("COPY_ISBN").ToString
                        Else
                            txt_Hold_CopyISBN.Text = ""
                        End If

                        If dr.Item("CLASS_NO").ToString <> "" Then
                            txt_Hold_ClassNo.Text = dr.Item("CLASS_NO").ToString
                        Else
                            txt_Hold_ClassNo.Text = ""
                        End If

                        If dr.Item("BOOK_NO").ToString <> "" Then
                            txt_Hold_BookNo.Text = dr.Item("BOOK_NO").ToString
                        Else
                            txt_Hold_BookNo.Text = ""
                        End If

                        If dr.Item("PAGINATION").ToString <> "" Then
                            txt_Hold_Pages.Text = dr.Item("PAGINATION").ToString
                        Else
                            txt_Hold_Pages.Text = ""
                        End If

                        If dr.Item("SIZE").ToString <> "" Then
                            txt_Hold_Size.Text = dr.Item("SIZE").ToString
                        Else
                            txt_Hold_Size.Text = ""
                        End If

                        If dr.Item("ILLUSTRATION").ToString <> "" Then
                            CB_Illus.Checked = True
                        Else
                            CB_Illus.Checked = False
                        End If

                        If dr.Item("COLLECTION_TYPE").ToString <> "" Then
                            DDL_CollectionType.SelectedValue = dr.Item("COLLECTION_TYPE").ToString
                        Else
                            DDL_CollectionType.ClearSelection()
                        End If

                        If dr.Item("STA_CODE").ToString <> "" Then
                            DDL_Status.SelectedValue = dr.Item("STA_CODE").ToString
                        Else
                            DDL_Status.ClearSelection()
                        End If

                        If dr.Item("BIND_CODE").ToString <> "" Then
                            DDL_Binding.SelectedValue = dr.Item("BIND_CODE").ToString
                        Else
                            DDL_Binding.ClearSelection()
                        End If

                        If dr.Item("SEC_CODE").ToString <> "" Then
                            DDL_Section.SelectedValue = dr.Item("SEC_CODE").ToString
                        Else
                            DDL_Section.ClearSelection()
                        End If

                        If dr.Item("LIB_CODE").ToString <> "" Then
                            DDL_Library.SelectedValue = dr.Item("LIB_CODE").ToString
                        Else
                            DDL_Library.ClearSelection()
                        End If

                        If dr.Item("ACC_MAT_CODE").ToString <> "" Then
                            DDL_AccMaterials.SelectedValue = dr.Item("ACC_MAT_CODE").ToString
                        Else
                            DDL_AccMaterials.ClearSelection()
                        End If

                        If dr.Item("REMARKS").ToString <> "" Then
                            txt_Hold_Remarks.Text = dr.Item("REMARKS").ToString
                        Else
                            txt_Hold_Remarks.Text = ""
                        End If

                        If dr.Item("PHYSICAL_LOCATION").ToString <> "" Then
                            txt_Hold_Location.Text = dr.Item("PHYSICAL_LOCATION").ToString
                        Else
                            txt_Hold_Location.Text = ""
                        End If

                        If dr.Item("REFERENCE_NO").ToString <> "" Then
                            txt_Hold_ReferenceNo.Text = dr.Item("REFERENCE_NO").ToString
                        Else
                            txt_Hold_ReferenceNo.Text = ""
                        End If

                        If dr.Item("MEDIUM").ToString <> "" Then
                            txt_Hold_RecordingMedium.Text = dr.Item("MEDIUM").ToString
                        Else
                            txt_Hold_RecordingMedium.Text = ""
                        End If

                        If dr.Item("RECORDING_CATEGORY").ToString <> "" Then
                            txt_Hold_RecordingCategory.Text = dr.Item("RECORDING_CATEGORY").ToString
                        Else
                            txt_Hold_RecordingCategory.Text = ""
                        End If

                        If dr.Item("RECORDING_FORM").ToString <> "" Then
                            txt_Hold_RecordingForm.Text = dr.Item("RECORDING_FORM").ToString
                        Else
                            txt_Hold_RecordingForm.Text = ""
                        End If

                        If dr.Item("RECORDING_FORMAT").ToString <> "" Then
                            txt_Hold_RecordingFormat.Text = dr.Item("RECORDING_FORMAT").ToString
                        Else
                            txt_Hold_RecordingFormat.Text = ""
                        End If

                        If dr.Item("RECORDING_SPEED").ToString <> "" Then
                            txt_Hold_RecordingSpeed.Text = dr.Item("RECORDING_SPEED").ToString
                        Else
                            txt_Hold_RecordingSpeed.Text = ""
                        End If

                        If dr.Item("RECORDING_STORAGE_TECH").ToString <> "" Then
                            txt_Hold_RecordingStorageTech.Text = dr.Item("RECORDING_STORAGE_TECH").ToString
                        Else
                            txt_Hold_RecordingStorageTech.Text = ""
                        End If

                        If dr.Item("RECORDING_PLAY_DURATION").ToString <> "" Then
                            txt_Hold_RecordingDuration.Text = dr.Item("RECORDING_PLAY_DURATION").ToString
                        Else
                            txt_Hold_RecordingDuration.Text = ""
                        End If

                        If dr.Item("VIDEO_TYPEOFVISUAL").ToString <> "" Then
                            txt_Hold_TypeOfVisuals.Text = dr.Item("VIDEO_TYPEOFVISUAL").ToString
                        Else
                            txt_Hold_TypeOfVisuals.Text = ""
                        End If

                        If dr.Item("CARTOGRAPHIC_SCALE").ToString <> "" Then
                            txt_Hold_Scale.Text = dr.Item("CARTOGRAPHIC_SCALE").ToString
                        Else
                            txt_Hold_Scale.Text = ""
                        End If

                        If dr.Item("CARTOGRAPHIC_PROJECTION").ToString <> "" Then
                            txt_Hold_Projection.Text = dr.Item("CARTOGRAPHIC_PROJECTION").ToString
                        Else
                            txt_Hold_Projection.Text = ""
                        End If

                        If dr.Item("CARTOGRAPHIC_COORDINATES").ToString <> "" Then
                            txt_Hold_Coordinates.Text = dr.Item("CARTOGRAPHIC_COORDINATES").ToString
                        Else
                            txt_Hold_Coordinates.Text = ""
                        End If

                        If dr.Item("CARTOGRAPHIC_GEOGRAPHIC_LOCATION").ToString <> "" Then
                            txt_Hold_GeographicLocation.Text = dr.Item("CARTOGRAPHIC_GEOGRAPHIC_LOCATION").ToString
                        Else
                            txt_Hold_GeographicLocation.Text = ""
                        End If

                        If dr.Item("CARTOGRAPHIC_MEDIUM").ToString <> "" Then
                            DDL_GeographicMedium.SelectedValue = dr.Item("CARTOGRAPHIC_MEDIUM").ToString
                        Else
                            DDL_GeographicMedium.ClearSelection()
                        End If

                        If dr.Item("CARTOGRAPHIC_DATAGATHERING_DATE").ToString <> "" Then
                            txt_Hold_DataGatheringDate.Text = Format(dr.Item("CARTOGRAPHIC_DATAGATHERING_DATE"), "dd/MM/yyyy")
                        Else
                            txt_Hold_DataGatheringDate.Text = ""
                        End If

                        If dr.Item("CREATION_DATE").ToString <> "" Then
                            txt_Hold_CreationDate.Text = Format(dr.Item("CREATION_DATE"), "dd/MM/yyyy")
                        Else
                            txt_Hold_CreationDate.Text = ""
                        End If

                        If dr.Item("CARTOGRAPHIC_COMPILATION_DATE").ToString <> "" Then
                            txt_Hold_CompilationDate.Text = Format(dr.Item("CARTOGRAPHIC_COMPILATION_DATE"), "dd/MM/yyyy")
                        Else
                            txt_Hold_CompilationDate.Text = ""
                        End If

                        If dr.Item("CARTOGRAPHIC_INSPECTION_DATE").ToString <> "" Then
                            txt_Hold_InspectionDate.Text = Format(dr.Item("CARTOGRAPHIC_INSPECTION_DATE"), "dd/MM/yyyy")
                        Else
                            txt_Hold_InspectionDate.Text = ""
                        End If

                        If dr.Item("ALTER_DATE").ToString <> "" Then
                            txt_Hold_AlterDate.Text = Format(dr.Item("ALTER_DATE"), "dd/MM/yyyy")
                        Else
                            txt_Hold_AlterDate.Text = ""
                        End If

                        If dr.Item("VIEW_DATE").ToString <> "" Then
                            txt_Hold_ViewDate.Text = Format(dr.Item("VIEW_DATE"), "dd/MM/yyyy")
                        Else
                            txt_Hold_ViewDate.Text = ""
                        End If

                        If dr.Item("VIDEO_COLOR").ToString <> "" Then
                            txt_Hold_Color.Text = dr.Item("VIDEO_COLOR").ToString
                        Else
                            txt_Hold_Color.Text = ""
                        End If

                        If dr.Item("PLAYBACK_CHANNELS").ToString <> "" Then
                            txt_Hold_PlayBackChannel.Text = dr.Item("PLAYBACK_CHANNELS").ToString
                        Else
                            txt_Hold_PlayBackChannel.Text = ""
                        End If

                        If dr.Item("TAPE_WIDTH").ToString <> "" Then
                            txt_Hold_TapeWidth.Text = dr.Item("TAPE_WIDTH").ToString
                        Else
                            txt_Hold_TapeWidth.Text = ""
                        End If

                        If dr.Item("TAPE_CONFIGURATION").ToString <> "" Then
                            txt_Hold_TapeConfiguration.Text = dr.Item("TAPE_CONFIGURATION").ToString
                        Else
                            txt_Hold_TapeConfiguration.Text = ""
                        End If

                        If dr.Item("KIND_OF_DISK").ToString <> "" Then
                            txt_Hold_KindofDisk.Text = dr.Item("KIND_OF_DISK").ToString
                        Else
                            txt_Hold_KindofDisk.Text = ""
                        End If

                        If dr.Item("KIND_OF_CUTTING").ToString <> "" Then
                            txt_Hold_KindofCutting.Text = dr.Item("KIND_OF_CUTTING").ToString
                        Else
                            txt_Hold_KindofCutting.Text = ""
                        End If

                        If dr.Item("ENCODING_STANDARD").ToString <> "" Then
                            txt_Hold_EncodingStandard.Text = dr.Item("ENCODING_STANDARD").ToString
                        Else
                            txt_Hold_EncodingStandard.Text = ""
                        End If

                        If dr.Item("CAPTURE_TECHNIQUE").ToString <> "" Then
                            txt_Hold_CaptureTechnique.Text = dr.Item("CAPTURE_TECHNIQUE").ToString
                        Else
                            txt_Hold_CaptureTechnique.Text = ""
                        End If

                        If dr.Item("PHOTO_NO").ToString <> "" Then
                            txt_Hold_PhotoNo.Text = dr.Item("PHOTO_NO").ToString
                        Else
                            txt_Hold_PhotoNo.Text = ""
                        End If

                        If dr.Item("PHOTO_ALBUM_NO").ToString <> "" Then
                            txt_Hold_PhotoAlbumNo.Text = dr.Item("PHOTO_ALBUM_NO").ToString
                        Else
                            txt_Hold_PhotoAlbumNo.Text = ""
                        End If

                        If dr.Item("PHOTO_OCASION").ToString <> "" Then
                            txt_Hold_Ocasion.Text = dr.Item("PHOTO_OCASION").ToString
                        Else
                            txt_Hold_Ocasion.Text = ""
                        End If

                        If dr.Item("IMAGE_VIEW_TYPE").ToString <> "" Then
                            txt_Hold_ImageViewType.Text = dr.Item("IMAGE_VIEW_TYPE").ToString
                        Else
                            txt_Hold_TapeConfiguration.Text = ""
                        End If

                        If dr.Item("THEME").ToString <> "" Then
                            txt_Hold_Theme.Text = dr.Item("THEME").ToString
                        Else
                            txt_Hold_Theme.Text = ""
                        End If

                        If dr.Item("STYLE").ToString <> "" Then
                            txt_Hold_Style.Text = dr.Item("STYLE").ToString
                        Else
                            txt_Hold_Style.Text = ""
                        End If

                        If dr.Item("CULTURE").ToString <> "" Then
                            txt_Hold_Culture.Text = dr.Item("CULTURE").ToString
                        Else
                            txt_Hold_Culture.Text = ""
                        End If

                        If dr.Item("CURRENT_SITE").ToString <> "" Then
                            txt_Hold_CurrentSite.Text = dr.Item("CURRENT_SITE").ToString
                        Else
                            txt_Hold_CurrentSite.Text = ""
                        End If

                        If dr.Item("CREATION_SITE").ToString <> "" Then
                            txt_Hold_CreationDate.Text = dr.Item("CREATION_SITE").ToString
                        Else
                            txt_Hold_CreationSite.Text = ""
                        End If

                        If dr.Item("YARNCOUNT").ToString <> "" Then
                            txt_Hold_YarnCount.Text = dr.Item("YARNCOUNT").ToString
                        Else
                            txt_Hold_YarnCount.Text = ""
                        End If

                        If dr.Item("MATERIAL_TYPE").ToString <> "" Then
                            txt_Hold_MaterialsType.Text = dr.Item("MATERIAL_TYPE").ToString
                        Else
                            txt_Hold_MaterialsType.Text = ""
                        End If

                        If dr.Item("TECHNIQUE").ToString <> "" Then
                            txt_Hold_Technique.Text = dr.Item("TECHNIQUE").ToString
                        Else
                            txt_Hold_Technique.Text = ""
                        End If

                        If dr.Item("TECH_DETAILS").ToString <> "" Then
                            txt_Hold_TechDetails.Text = dr.Item("TECH_DETAILS").ToString
                        Else
                            txt_Hold_TechDetails.Text = ""
                        End If

                        If dr.Item("INSCRIPTIONS").ToString <> "" Then
                            txt_Hold_Inscription.Text = dr.Item("INSCRIPTIONS").ToString
                        Else
                            txt_Hold_Inscription.Text = ""
                        End If

                        If dr.Item("DESCRIPTION").ToString <> "" Then
                            txt_Hold_Description.Text = dr.Item("DESCRIPTION").ToString
                        Else
                            txt_Hold_Description.Text = ""
                        End If

                        If dr.Item("GLOBE_TYPE").ToString <> "" Then
                            txt_Hold_GlobeType.Text = dr.Item("GLOBE_TYPE").ToString
                        Else
                            txt_Hold_GlobeType.Text = ""
                        End If
                        dr.Close()
                    Else
                        Label_ACQID.Text = ""
                        Label_HoldID.Text = ""
                    End If
                Else
                    Label_ACQID.Text = ""
                    Label_HoldID.Text = ""
                End If
            End If
            LoadHoldFormat()
            DisplayAcq()
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
            Label15.Text = ""
            Label_ACQID.Text = ""
            Label_HOLDID.Text = ""
        Finally
            SqlConn.Close()
            Me.txt_Hold_AccNo.Focus()
        End Try
    End Sub 'Grid1_ItemCommand
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
    <System.Web.Script.Services.ScriptMethod(), _
System.Web.Services.WebMethod()> _
    Public Shared Function SearchISBN(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Return EG4.PopulateCombo.SearchISBN(prefixText, count)
    End Function
    <System.Web.Script.Services.ScriptMethod(), _
System.Web.Services.WebMethod()> _
    Public Shared Function SearchSP(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Return EG4.PopulateCombo.SearchSPNo(prefixText, count)
    End Function
    <System.Web.Script.Services.ScriptMethod(), _
System.Web.Services.WebMethod()> _
    Public Shared Function SearchReport(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Return EG4.PopulateCombo.SearchReportNo(prefixText, count)
    End Function
    <System.Web.Script.Services.ScriptMethod(), _
System.Web.Services.WebMethod()> _
    Public Shared Function SearchManual(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Return EG4.PopulateCombo.SearchManualNo(prefixText, count)
    End Function
    <System.Web.Script.Services.ScriptMethod(), _
System.Web.Services.WebMethod()> _
    Public Shared Function SearchAct(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Return EG4.PopulateCombo.SearchActNo(prefixText, count)
    End Function
    <System.Web.Script.Services.ScriptMethod(), _
System.Web.Services.WebMethod()> _
    Public Shared Function SearchAccNo(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Return EG4.PopulateCombo.SearchAccNo(prefixText, count, LibCode)
    End Function
    <System.Web.Script.Services.ScriptMethod(), _
System.Web.Services.WebMethod()> _
    Public Shared Function SearchClass(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Return EG4.PopulateCombo.SearchClassNo(prefixText, count, LibCode)
    End Function
    'populate currencies
    Public Sub PopulateCurrencies()
        Me.DDL_Currencies.DataSource = GetCurrenciesList()
        Me.DDL_Currencies.DataTextField = "CUR_NAME"
        Me.DDL_Currencies.DataValueField = "CUR_CODE"
        Me.DDL_Currencies.DataBind()
        DDL_Currencies.Items.Insert(0, "")
    End Sub
    Public Sub PopulateAcqModes()
        Me.DDL_AcqModes.DataTextField = "ACQMODE_NAME"
        Me.DDL_AcqModes.DataValueField = "ACQMODE_CODE"
        Me.DDL_AcqModes.DataSource = GetAcqModesList()
        Me.DDL_AcqModes.DataBind()
    End Sub
    Public Sub PopulateVendors()
        DDL_Vendors.DataTextField = "VEND_NAME"
        DDL_Vendors.DataValueField = "VEND_ID"
        DDL_Vendors.DataSource = GetVendorList()
        DDL_Vendors.DataBind()
        DDL_Vendors.Items.Insert(0, "")
    End Sub

    Public Sub PopulateFormats()
        DDL_Format.DataTextField = "FORMAT_NAME"
        DDL_Format.DataValueField = "FORMAT_CODE"
        DDL_Format.DataSource = GetFormatList()
        DDL_Format.DataBind()
        DDL_Format.Items.Insert(0, "")
    End Sub
    Public Sub PopulateStatus()
        DDL_Status.DataTextField = "STA_NAME"
        DDL_Status.DataValueField = "STA_CODE"
        DDL_Status.DataSource = GetCopyStatusList()
        DDL_Status.DataBind()
        DDL_Status.Items.Insert(0, "")
    End Sub
    Public Sub PopulateBindings()
        DDL_Binding.DataTextField = "BIND_NAME"
        DDL_Binding.DataValueField = "BIND_CODE"
        DDL_Binding.DataSource = GetBindingList()
        DDL_Binding.DataBind()
        DDL_Binding.Items.Insert(0, "")
    End Sub
    Public Sub PopulateAccMaterials()
        DDL_AccMaterials.DataTextField = "ACC_MAT_NAME"
        DDL_AccMaterials.DataValueField = "ACC_MAT_CODE"
        DDL_AccMaterials.DataSource = GetAccMaterialsList()
        DDL_AccMaterials.DataBind()
        DDL_AccMaterials.Items.Insert(0, "")
    End Sub
    'populate libraries
    Public Sub PopulateLibrary()
        Dim Command As SqlCommand = Nothing
        Dim dt As DataTable = Nothing
        Try
            Command = New SqlCommand("SELECT  LIB_CODE, LIB_NAME FROM LIBRARIES WHERE (LIB_CODE = '" & Trim(LibCode) & "' OR MAIN_LIB_CODE = '" & Trim(LibCode) & "') ORDER BY LIB_NAME ", SqlConn)
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
            Dr("LIB_CODE") = ""
            Dr("LIB_NAME") = ""
            dt.Rows.InsertAt(Dr, 0)

            If dt.Rows.Count = 0 Then
                Me.DDL_Library.DataSource = Nothing
            Else
                Me.DDL_Library.DataSource = dt
                Me.DDL_Library.DataTextField = "LIB_NAME"
                Me.DDL_Library.DataValueField = "LIB_CODE"
                Me.DDL_Library.DataBind()
            End If
            dt.Dispose()
            SqlConn.Close()
            DDL_Library.Text = ""
        Catch s As Exception
            Label15.Text = "Error: " & (s.Message())
            Label1.Text = ""
        Finally
            Command.Dispose()
            dt.Dispose()
            SqlConn.Close()
        End Try
    End Sub
    'populate libraries
    Public Sub PopulateSections()
        Dim Command As SqlCommand = Nothing
        Dim dt As DataTable = Nothing
        Try
            Command = New SqlCommand("SELECT  SEC_CODE, SEC_NAME FROM SECTIONS WHERE (LIB_CODE = '" & Trim(LibCode) & "') ORDER BY SEC_NAME ", SqlConn)
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
            Dr("SEC_CODE") = ""
            Dr("SEC_NAME") = ""
            dt.Rows.InsertAt(Dr, 0)

            If dt.Rows.Count = 0 Then
                Me.DDL_Section.DataSource = Nothing
            Else
                Me.DDL_Section.DataSource = dt
                Me.DDL_Section.DataTextField = "SEC_NAME"
                Me.DDL_Section.DataValueField = "SEC_CODE"
                Me.DDL_Section.DataBind()
            End If
            dt.Dispose()
            SqlConn.Close()
            DDL_Section.Text = ""
        Catch s As Exception
            Label15.Text = "Error: " & (s.Message())
            Label1.Text = ""
        Finally
            Command.Dispose()
            dt.Dispose()
            SqlConn.Close()
        End Try

    End Sub
    'enter multi-copies checked event   
    Protected Sub CB_RecvAll_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles CB_RecvAll.CheckedChanged
        If CB_RecvAll.Checked = True Then
            txt_Hold_Copies.Focus()
        End If
    End Sub
    Public Sub DisplayLastHoldRecord()
        Dim dt As DataTable
        Try
            Dim CAT_NO As Long = Nothing
            If Label7.Text <> "" Then
                CAT_NO = Label7.Text
                'get record details from database
                Dim SQL As String = Nothing
                SQL = "SELECT TOP 1 * FROM HOLD_ACQ_VIEW WHERE (CAT_NO = '" & Trim(CAT_NO) & "') AND (LIB_CODE = '" & Trim(LibCode) & "') ORDER BY HOLD_ID DESC"

                Dim ds As New DataSet
                Dim da As New SqlDataAdapter(SQL, SqlConn)
                If SqlConn.State = 0 Then
                    SqlConn.Open()
                End If
                da.Fill(ds)

                dt = ds.Tables(0).Copy

                If dt.Rows.Count <> 0 Then
                    If Table4.Visible = True Then
                        If dt.Rows(0).Item("ACQ_ID").ToString <> "" Then
                            Me.Label_ACQID.Text = dt.Rows(0).Item("ACQ_ID").ToString
                        Else
                            Me.Label_ACQID.Text = ""
                        End If

                        If dt.Rows(0).Item("PROCESS_STATUS").ToString <> "" Then
                            Me.Label_ProcessStatus.Text = dt.Rows(0).Item("PROCESS_STATUS").ToString
                        Else
                            Me.Label_ProcessStatus.Text = ""
                        End If

                        If dt.Rows(0).Item("ACQMODE_CODE").ToString <> "" Then
                            Me.DDL_AcqModes.SelectedValue = dt.Rows(0).Item("ACQMODE_CODE").ToString
                        Else
                            DDL_AcqModes.ClearSelection()
                        End If

                        If dt.Rows(0).Item("CUR_CODE").ToString <> "" Then
                            Me.DDL_Currencies.SelectedValue = dt.Rows(0).Item("CUR_CODE").ToString
                        Else
                            DDL_Currencies.ClearSelection()
                        End If

                        If dt.Rows(0).Item("VEND_ID").ToString <> "" Then
                            Me.DDL_Vendors.SelectedValue = dt.Rows(0).Item("VEND_ID").ToString
                        Else
                            DDL_Vendors.ClearSelection()
                        End If

                        If dt.Rows(0).Item("ITEM_PRICE").ToString <> "" Then
                            Me.txt_Acq_ItemPrice.Text = dt.Rows(0).Item("ITEM_PRICE").ToString
                        Else
                            txt_Acq_ItemPrice.Text = ""
                        End If

                        If dt.Rows(0).Item("CONVERSION_RATE").ToString <> "" Then
                            Me.txt_Acq_ConversionRate.Text = dt.Rows(0).Item("CONVERSION_RATE").ToString
                        Else
                            txt_Acq_ConversionRate.Text = ""
                        End If

                        If dt.Rows(0).Item("ITEM_RUPEES").ToString <> "" Then
                            Me.txt_Acq_ItemRupees.Text = dt.Rows(0).Item("ITEM_RUPEES").ToString
                        Else
                            txt_Acq_ItemRupees.Text = ""
                        End If

                        If dt.Rows(0).Item("OTHER_CHARGES").ToString <> "" Then
                            Me.txt_Acq_OtherCharges.Text = dt.Rows(0).Item("OTHER_CHARGES").ToString
                        Else
                            txt_Acq_OtherCharges.Text = ""
                        End If
                    End If 'if tabl4 acq not visible

                    If dt.Rows(0).Item("HOLD_ID").ToString <> "" Then
                        Label_HoldID.Text = dt.Rows(0).Item("HOLD_ID").ToString
                    Else
                        Label_HoldID.Text = ""
                    End If

                    If TR_FORMAT_CODE.Visible = True Then
                        If dt.Rows(0).Item("FORMAT_CODE").ToString <> "" Then
                            DDL_Format.SelectedValue = dt.Rows(0).Item("FORMAT_CODE").ToString
                        Else
                            DDL_Format.ClearSelection()
                        End If
                    End If

                    If dt.Rows(0).Item("SHOW").ToString <> "" Then
                        DDL_Show.SelectedValue = dt.Rows(0).Item("SHOW").ToString
                    Else
                        DDL_Show.ClearSelection()
                    End If

                    If dt.Rows(0).Item("ISSUEABLE").ToString <> "" Then
                        DDL_Issuable.SelectedValue = dt.Rows(0).Item("ISSUEABLE").ToString
                    Else
                        DDL_Issuable.ClearSelection()
                    End If

                    If TR_VOL_NO.Visible = True Then
                        If dt.Rows(0).Item("VOL_NO").ToString <> "" Then
                            txt_Hold_VolNo.Text = dt.Rows(0).Item("VOL_NO").ToString
                        Else
                            txt_Hold_VolNo.Text = ""
                        End If
                    End If

                    If TR_VOL_NO.Visible = True Then
                        If dt.Rows(0).Item("VOL_YEAR").ToString <> "" Then
                            txt_Hold_VolYear.Text = dt.Rows(0).Item("VOL_YEAR").ToString
                        Else
                            txt_Hold_VolYear.Text = ""
                        End If
                    End If

                    If TR_VOL_EDITORS.Visible = True Then
                        If dt.Rows(0).Item("VOL_EDITORS").ToString <> "" Then
                            txt_Hold_VolEditors.Text = dt.Rows(0).Item("VOL_EDITORS").ToString
                        Else
                            txt_Hold_VolEditors.Text = ""
                        End If
                    End If

                    If TR_VOL_TITLE.Visible = True Then
                        If dt.Rows(0).Item("VOL_TITLE").ToString <> "" Then
                            txt_Hold_VolTitle.Text = dt.Rows(0).Item("VOL_TITLE").ToString
                        Else
                            txt_Hold_VolTitle.Text = ""
                        End If
                    End If

                    If TR_VOL_NO.Visible = True Then
                        If dt.Rows(0).Item("COPY_ISBN").ToString <> "" Then
                            txt_Hold_CopyISBN.Text = dt.Rows(0).Item("COPY_ISBN").ToString
                        Else
                            txt_Hold_CopyISBN.Text = ""
                        End If
                    End If

                    If TR_CLASS_NO.Visible = True Then
                        If dt.Rows(0).Item("CLASS_NO").ToString <> "" Then
                            txt_Hold_ClassNo.Text = dt.Rows(0).Item("CLASS_NO").ToString
                        Else
                            txt_Hold_ClassNo.Text = ""
                        End If
                    End If

                    If TR_CLASS_NO.Visible = True Then
                        If dt.Rows(0).Item("BOOK_NO").ToString <> "" Then
                            txt_Hold_BookNo.Text = dt.Rows(0).Item("BOOK_NO").ToString
                        Else
                            txt_Hold_BookNo.Text = ""
                        End If
                    End If

                    If TR_PAGINATION.Visible = True Then
                        If dt.Rows(0).Item("PAGINATION").ToString <> "" Then
                            txt_Hold_Pages.Text = dt.Rows(0).Item("PAGINATION").ToString
                        Else
                            txt_Hold_Pages.Text = ""
                        End If
                    End If

                    If TR_SIZE.Visible = True Then
                        If dt.Rows(0).Item("SIZE").ToString <> "" Then
                            txt_Hold_Size.Text = dt.Rows(0).Item("SIZE").ToString
                        Else
                            txt_Hold_Size.Text = ""
                        End If
                    End If

                    If TR_ILLUSTRATION.Visible = True Then
                        If dt.Rows(0).Item("ILLUSTRATION").ToString <> "" Then
                            CB_Illus.Checked = True
                        Else
                            CB_Illus.Checked = False
                        End If
                    End If

                    If TR_COLLECTION_TYPE.Visible = True Then
                        If dt.Rows(0).Item("COLLECTION_TYPE").ToString <> "" Then
                            DDL_CollectionType.SelectedValue = dt.Rows(0).Item("COLLECTION_TYPE").ToString
                        Else
                            DDL_CollectionType.ClearSelection()
                        End If
                    End If

                    If TR_STA_CODE.Visible = True Then
                        If dt.Rows(0).Item("STA_CODE").ToString <> "" Then
                            DDL_Status.SelectedValue = dt.Rows(0).Item("STA_CODE").ToString
                        Else
                            DDL_Status.ClearSelection()
                        End If
                    End If

                    If TR_BIND_CODE.Visible = True Then
                        If dt.Rows(0).Item("BIND_CODE").ToString <> "" Then
                            DDL_Binding.SelectedValue = dt.Rows(0).Item("BIND_CODE").ToString
                        Else
                            DDL_Binding.ClearSelection()
                        End If
                    End If

                    If TR_SEC_CODE.Visible = True Then
                        If dt.Rows(0).Item("SEC_CODE").ToString <> "" Then
                            DDL_Section.SelectedValue = dt.Rows(0).Item("SEC_CODE").ToString
                        Else
                            DDL_Section.ClearSelection()
                        End If
                    End If

                    If TR_LIBRARY.Visible = True Then
                        If dt.Rows(0).Item("LIB_CODE").ToString <> "" Then
                            DDL_Library.SelectedValue = dt.Rows(0).Item("LIB_CODE").ToString
                        Else
                            DDL_Library.ClearSelection()
                        End If
                    End If

                    If TR_ACC_MAT_CODE.Visible = True Then
                        If dt.Rows(0).Item("ACC_MAT_CODE").ToString <> "" Then
                            DDL_AccMaterials.SelectedValue = dt.Rows(0).Item("ACC_MAT_CODE").ToString
                        Else
                            DDL_AccMaterials.ClearSelection()
                        End If
                    End If

                    If TR_HOLDREMARKS.Visible = True Then
                        If dt.Rows(0).Item("REMARKS").ToString <> "" Then
                            txt_Hold_Remarks.Text = dt.Rows(0).Item("REMARKS").ToString
                        Else
                            txt_Hold_Remarks.Text = ""
                        End If
                    End If

                    If TR_PHYSICAL_LOCATION.Visible = True Then
                        If dt.Rows(0).Item("PHYSICAL_LOCATION").ToString <> "" Then
                            txt_Hold_Location.Text = dt.Rows(0).Item("PHYSICAL_LOCATION").ToString
                        Else
                            txt_Hold_Location.Text = ""
                        End If
                    End If

                    If TR_REFERENCE_NO.Visible = True Then
                        If dt.Rows(0).Item("REFERENCE_NO").ToString <> "" Then
                            txt_Hold_ReferenceNo.Text = dt.Rows(0).Item("REFERENCE_NO").ToString
                        Else
                            txt_Hold_ReferenceNo.Text = ""
                        End If
                    End If

                    If TR_MEDIUM.Visible = True Then
                        If dt.Rows(0).Item("MEDIUM").ToString <> "" Then
                            txt_Hold_RecordingMedium.Text = dt.Rows(0).Item("MEDIUM").ToString
                        Else
                            txt_Hold_RecordingMedium.Text = ""
                        End If
                    End If

                    If TR_RECORDING_CATEGORY.Visible = True Then
                        If dt.Rows(0).Item("RECORDING_CATEGORY").ToString <> "" Then
                            txt_Hold_RecordingCategory.Text = dt.Rows(0).Item("RECORDING_CATEGORY").ToString
                        Else
                            txt_Hold_RecordingCategory.Text = ""
                        End If
                    End If

                    If TR_RECORDING_FORM.Visible = True Then
                        If dt.Rows(0).Item("RECORDING_FORM").ToString <> "" Then
                            txt_Hold_RecordingForm.Text = dt.Rows(0).Item("RECORDING_FORM").ToString
                        Else
                            txt_Hold_RecordingForm.Text = ""
                        End If
                    End If


                    If TR_RECORDING_FORMAT.Visible = True Then
                        If dt.Rows(0).Item("RECORDING_FORMAT").ToString <> "" Then
                            txt_Hold_RecordingFormat.Text = dt.Rows(0).Item("RECORDING_FORMAT").ToString
                        Else
                            txt_Hold_RecordingFormat.Text = ""
                        End If
                    End If

                    If TR_RECORDING_SPEED.Visible = True Then
                        If dt.Rows(0).Item("RECORDING_SPEED").ToString <> "" Then
                            txt_Hold_RecordingSpeed.Text = dt.Rows(0).Item("RECORDING_SPEED").ToString
                        Else
                            txt_Hold_RecordingSpeed.Text = ""
                        End If
                    End If

                    If TR_RECORDING_STORAGE_TECH.Visible = True Then
                        If dt.Rows(0).Item("RECORDING_STORAGE_TECH").ToString <> "" Then
                            txt_Hold_RecordingStorageTech.Text = dt.Rows(0).Item("RECORDING_STORAGE_TECH").ToString
                        Else
                            txt_Hold_RecordingStorageTech.Text = ""
                        End If
                    End If

                    If TR_RECORDING_PLAY_DURATION.Visible = True Then
                        If dt.Rows(0).Item("RECORDING_PLAY_DURATION").ToString <> "" Then
                            txt_Hold_RecordingDuration.Text = dt.Rows(0).Item("RECORDING_PLAY_DURATION").ToString
                        Else
                            txt_Hold_RecordingDuration.Text = ""
                        End If
                    End If

                    If TR_VIDEO_TYPEOFVISUAL.Visible = True Then
                        If dt.Rows(0).Item("VIDEO_TYPEOFVISUAL").ToString <> "" Then
                            txt_Hold_TypeOfVisuals.Text = dt.Rows(0).Item("VIDEO_TYPEOFVISUAL").ToString
                        Else
                            txt_Hold_TypeOfVisuals.Text = ""
                        End If
                    End If

                    If TR_CARTOGRAPHIC_SCALE.Visible = True Then
                        If dt.Rows(0).Item("CARTOGRAPHIC_SCALE").ToString <> "" Then
                            txt_Hold_Scale.Text = dt.Rows(0).Item("CARTOGRAPHIC_SCALE").ToString
                        Else
                            txt_Hold_Scale.Text = ""
                        End If
                    End If

                    If TR_CARTOGRAPHIC_PROJECTION.Visible = True Then
                        If dt.Rows(0).Item("CARTOGRAPHIC_PROJECTION").ToString <> "" Then
                            txt_Hold_Projection.Text = dt.Rows(0).Item("CARTOGRAPHIC_PROJECTION").ToString
                        Else
                            txt_Hold_Projection.Text = ""
                        End If
                    End If

                    If TR_CARTOGRAPHIC_COORDINATES.Visible = True Then
                        If dt.Rows(0).Item("CARTOGRAPHIC_COORDINATES").ToString <> "" Then
                            txt_Hold_Coordinates.Text = dt.Rows(0).Item("CARTOGRAPHIC_COORDINATES").ToString
                        Else
                            txt_Hold_Coordinates.Text = ""
                        End If
                    End If

                    If TR_CARTOGRAPHIC_GEOGRAPHIC_LOCATION.Visible = True Then
                        If dt.Rows(0).Item("CARTOGRAPHIC_GEOGRAPHIC_LOCATION").ToString <> "" Then
                            txt_Hold_GeographicLocation.Text = dt.Rows(0).Item("CARTOGRAPHIC_GEOGRAPHIC_LOCATION").ToString
                        Else
                            txt_Hold_GeographicLocation.Text = ""
                        End If
                    End If

                    If TR_CARTOGRAPHIC_MEDIUM.Visible = True Then
                        If dt.Rows(0).Item("CARTOGRAPHIC_MEDIUM").ToString <> "" Then
                            DDL_GeographicMedium.SelectedValue = dt.Rows(0).Item("CARTOGRAPHIC_MEDIUM").ToString
                        Else
                            DDL_GeographicMedium.ClearSelection()
                        End If
                    End If

                    If TR_CARTOGRAPHIC_DATAGATHERING_DATE.Visible = True Then
                        If dt.Rows(0).Item("CARTOGRAPHIC_DATAGATHERING_DATE").ToString <> "" Then
                            txt_Hold_DataGatheringDate.Text = Format(dt.Rows(0).Item("CARTOGRAPHIC_DATAGATHERING_DATE"), "dd/MM/yyyy")
                        Else
                            txt_Hold_DataGatheringDate.Text = ""
                        End If
                    End If

                    If TR_CREATION_DATE.Visible = True Then
                        If dt.Rows(0).Item("CREATION_DATE").ToString <> "" Then
                            txt_Hold_CreationDate.Text = Format(dt.Rows(0).Item("CREATION_DATE"), "dd/MM/yyyy")
                        Else
                            txt_Hold_CreationDate.Text = ""
                        End If
                    End If

                    If TR_CARTOGRAPHIC_COMPILATION_DATE.Visible = True Then
                        If dt.Rows(0).Item("CARTOGRAPHIC_COMPILATION_DATE").ToString <> "" Then
                            txt_Hold_CompilationDate.Text = Format(dt.Rows(0).Item("CARTOGRAPHIC_COMPILATION_DATE"), "dd/MM/yyyy")
                        Else
                            txt_Hold_CompilationDate.Text = ""
                        End If
                    End If

                    If TR_CARTOGRAPHIC_INSPECTION_DATE.Visible = True Then
                        If dt.Rows(0).Item("CARTOGRAPHIC_INSPECTION_DATE").ToString <> "" Then
                            txt_Hold_InspectionDate.Text = Format(dt.Rows(0).Item("CARTOGRAPHIC_INSPECTION_DATE"), "dd/MM/yyyy")
                        Else
                            txt_Hold_InspectionDate.Text = ""
                        End If
                    End If

                    If TR_ALTER_DATE.Visible = True Then
                        If dt.Rows(0).Item("ALTER_DATE").ToString <> "" Then
                            txt_Hold_AlterDate.Text = Format(dt.Rows(0).Item("ALTER_DATE"), "dd/MM/yyyy")
                        Else
                            txt_Hold_AlterDate.Text = ""
                        End If
                    End If

                    If TR_VIEW_DATE.Visible = True Then
                        If dt.Rows(0).Item("VIEW_DATE").ToString <> "" Then
                            txt_Hold_ViewDate.Text = Format(dt.Rows(0).Item("VIEW_DATE"), "dd/MM/yyyy")
                        Else
                            txt_Hold_ViewDate.Text = ""
                        End If
                    End If

                    If TR_VIDEO_COLOR.Visible = True Then
                        If dt.Rows(0).Item("VIDEO_COLOR").ToString <> "" Then
                            txt_Hold_Color.Text = dt.Rows(0).Item("VIDEO_COLOR").ToString
                        Else
                            txt_Hold_Color.Text = ""
                        End If
                    End If

                    If TR_PLAYBACK_CHANNELS.Visible = True Then
                        If dt.Rows(0).Item("PLAYBACK_CHANNELS").ToString <> "" Then
                            txt_Hold_PlayBackChannel.Text = dt.Rows(0).Item("PLAYBACK_CHANNELS").ToString
                        Else
                            txt_Hold_PlayBackChannel.Text = ""
                        End If
                    End If

                    If TR_TAPE_WIDTH.Visible = True Then
                        If dt.Rows(0).Item("TAPE_WIDTH").ToString <> "" Then
                            txt_Hold_TapeWidth.Text = dt.Rows(0).Item("TAPE_WIDTH").ToString
                        Else
                            txt_Hold_TapeWidth.Text = ""
                        End If
                    End If

                    If TR_TAPE_CONFIGURATION.Visible = True Then
                        If dt.Rows(0).Item("TAPE_CONFIGURATION").ToString <> "" Then
                            txt_Hold_TapeConfiguration.Text = dt.Rows(0).Item("TAPE_CONFIGURATION").ToString
                        Else
                            txt_Hold_TapeConfiguration.Text = ""
                        End If
                    End If


                    If TR_KIND_OF_DISK.Visible = True Then
                        If dt.Rows(0).Item("KIND_OF_DISK").ToString <> "" Then
                            txt_Hold_KindofDisk.Text = dt.Rows(0).Item("KIND_OF_DISK").ToString
                        Else
                            txt_Hold_KindofDisk.Text = ""
                        End If
                    End If

                    If TR_KIND_OF_CUTTING.Visible = True Then
                        If dt.Rows(0).Item("KIND_OF_CUTTING").ToString <> "" Then
                            txt_Hold_KindofCutting.Text = dt.Rows(0).Item("KIND_OF_CUTTING").ToString
                        Else
                            txt_Hold_KindofCutting.Text = ""
                        End If
                    End If


                    If TR_ENCODING_STANDARD.Visible = True Then
                        If dt.Rows(0).Item("ENCODING_STANDARD").ToString <> "" Then
                            txt_Hold_EncodingStandard.Text = dt.Rows(0).Item("ENCODING_STANDARD").ToString
                        Else
                            txt_Hold_EncodingStandard.Text = ""
                        End If
                    End If

                    If TR_CAPTURE_TECHNIQUE.Visible = True Then
                        If dt.Rows(0).Item("CAPTURE_TECHNIQUE").ToString <> "" Then
                            txt_Hold_CaptureTechnique.Text = dt.Rows(0).Item("CAPTURE_TECHNIQUE").ToString
                        Else
                            txt_Hold_CaptureTechnique.Text = ""
                        End If
                    End If

                    If TR_PHOTO_NO.Visible = True Then
                        If dt.Rows(0).Item("PHOTO_NO").ToString <> "" Then
                            txt_Hold_PhotoNo.Text = dt.Rows(0).Item("PHOTO_NO").ToString
                        Else
                            txt_Hold_PhotoNo.Text = ""
                        End If
                    End If

                    If TR_PHOTO_ALBUM_NO.Visible = True Then
                        If dt.Rows(0).Item("PHOTO_ALBUM_NO").ToString <> "" Then
                            txt_Hold_PhotoAlbumNo.Text = dt.Rows(0).Item("PHOTO_ALBUM_NO").ToString
                        Else
                            txt_Hold_PhotoAlbumNo.Text = ""
                        End If
                    End If

                    If TR_PHOTO_OCASION.Visible = True Then
                        If dt.Rows(0).Item("PHOTO_OCASION").ToString <> "" Then
                            txt_Hold_Ocasion.Text = dt.Rows(0).Item("PHOTO_OCASION").ToString
                        Else
                            txt_Hold_Ocasion.Text = ""
                        End If
                    End If

                    If TR_IMAGE_VIEW_TYPE.Visible = True Then
                        If dt.Rows(0).Item("IMAGE_VIEW_TYPE").ToString <> "" Then
                            txt_Hold_ImageViewType.Text = dt.Rows(0).Item("IMAGE_VIEW_TYPE").ToString
                        Else
                            txt_Hold_TapeConfiguration.Text = ""
                        End If
                    End If

                    If TR_THEME.Visible = True Then
                        If dt.Rows(0).Item("THEME").ToString <> "" Then
                            txt_Hold_Theme.Text = dt.Rows(0).Item("THEME").ToString
                        Else
                            txt_Hold_Theme.Text = ""
                        End If
                    End If

                    If TR_STYLE.Visible = True Then
                        If dt.Rows(0).Item("STYLE").ToString <> "" Then
                            txt_Hold_Style.Text = dt.Rows(0).Item("STYLE").ToString
                        Else
                            txt_Hold_Style.Text = ""
                        End If
                    End If

                    If TR_CULTURE.Visible = True Then
                        If dt.Rows(0).Item("CULTURE").ToString <> "" Then
                            txt_Hold_Culture.Text = dt.Rows(0).Item("CULTURE").ToString
                        Else
                            txt_Hold_Culture.Text = ""
                        End If
                    End If

                    If TR_CURRENT_SITE.Visible = True Then
                        If dt.Rows(0).Item("CURRENT_SITE").ToString <> "" Then
                            txt_Hold_CurrentSite.Text = dt.Rows(0).Item("CURRENT_SITE").ToString
                        Else
                            txt_Hold_CurrentSite.Text = ""
                        End If
                    End If

                    If TR_CREATION_SITE.Visible = True Then
                        If dt.Rows(0).Item("CREATION_SITE").ToString <> "" Then
                            txt_Hold_CreationDate.Text = dt.Rows(0).Item("CREATION_SITE").ToString
                        Else
                            txt_Hold_CreationSite.Text = ""
                        End If
                    End If

                    If TR_YARNCOUNT.Visible = True Then
                        If dt.Rows(0).Item("YARNCOUNT").ToString <> "" Then
                            txt_Hold_YarnCount.Text = dt.Rows(0).Item("YARNCOUNT").ToString
                        Else
                            txt_Hold_YarnCount.Text = ""
                        End If
                    End If

                    If TR_MATERIAL_TYPE.Visible = True Then
                        If dt.Rows(0).Item("MATERIAL_TYPE").ToString <> "" Then
                            txt_Hold_MaterialsType.Text = dt.Rows(0).Item("MATERIAL_TYPE").ToString
                        Else
                            txt_Hold_MaterialsType.Text = ""
                        End If
                    End If

                    If TR_TECHNIQUE.Visible = True Then
                        If dt.Rows(0).Item("TECHNIQUE").ToString <> "" Then
                            txt_Hold_Technique.Text = dt.Rows(0).Item("TECHNIQUE").ToString
                        Else
                            txt_Hold_Technique.Text = ""
                        End If
                    End If

                    If TR_TECH_DETAILS.Visible = True Then
                        If dt.Rows(0).Item("TECH_DETAILS").ToString <> "" Then
                            txt_Hold_TechDetails.Text = dt.Rows(0).Item("TECH_DETAILS").ToString
                        Else
                            txt_Hold_TechDetails.Text = ""
                        End If
                    End If

                    If TR_INSCRIPTIONS.Visible = True Then
                        If dt.Rows(0).Item("INSCRIPTIONS").ToString <> "" Then
                            txt_Hold_Inscription.Text = dt.Rows(0).Item("INSCRIPTIONS").ToString
                        Else
                            txt_Hold_Inscription.Text = ""
                        End If
                    End If

                    If TR_DESCRIPTION.Visible = True Then
                        If dt.Rows(0).Item("DESCRIPTION").ToString <> "" Then
                            txt_Hold_Description.Text = dt.Rows(0).Item("DESCRIPTION").ToString
                        Else
                            txt_Hold_Description.Text = ""
                        End If
                    End If

                    If TR_GLOBE_TYPE.Visible = True Then
                        If dt.Rows(0).Item("GLOBE_TYPE").ToString <> "" Then
                            txt_Hold_GlobeType.Text = dt.Rows(0).Item("GLOBE_TYPE").ToString
                        Else
                            txt_Hold_GlobeType.Text = ""
                        End If
                    End If


                End If
            End If
        Catch ex As Exception
            Label6.Text = "Error in Display of Copy/Acq Record " & (ex.Message())
            Label15.Text = ""
        Finally
            SqlConn.Close()
        End Try
    End Sub
   

   
   
End Class