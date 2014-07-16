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
Imports System.Drawing
Imports System.Drawing.Bitmap
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports EG4.PopulateCombo
Public Class Digitals
    Inherits System.Web.UI.Page
    Dim index As Integer = 0
    Public Srno As Integer = 0
    Dim files() As String
    Dim ImagePAth As String = Nothing
    Dim RecordCount As Long = 0
    Public image As New ArrayList
    Public nonimage As New ArrayList
    Public flag As String = ""
    Public FileName() As String = Nothing
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session.Item("IsThisU") <> Nothing Then
                If Not SConnection() = True Then
                    Label6.Text = "Database Connection is lost..Try Again !'"
                    Label15.Text = ""
                Else
                    LibCode = Session.Item("LoggedLibcode")
                    If Page.IsPostBack = False Then
                        CreateRootFolder()
                        TR_Folder.Visible = False
                        SearchTitle()
                    End If
                    Dim CreateSUserButton As Object 'System.Web.UI.WebControls.Button
                    CreateSUserButton = Master.FindControl("Accordion1").FindControl("CatPane").FindControl("Cat_Digitals_Bttn")
                    CreateSUserButton.ForeColor = Drawing.Color.Red
                    myPaneName = "CatPane"
                End If
            Else
                Response.Redirect("~/Default.aspx", False)
            End If
        Catch ex As Exception
            Label6.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    'Search Catalog
    Public Sub Search_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Search_Bttn.Click
        SearchTitle()
    End Sub
    Public Sub SearchTitle()
        Dim dtSearch As DataTable = Nothing
        Try
            Dim c, counter1, counter2, counter3, counter4, counter5 As Integer
            Dim iloop As Integer
            Dim strcurrentchar As Object

            'search string validation
            Dim mySearchString As Object = Nothing
            If txt_Search_SearchString.Text <> "" Then
                mySearchString = TrimAll(txt_Search_SearchString.Text)
                mySearchString = RemoveQuotes(mySearchString)
                If mySearchString.Length > 250 Then
                    Label6.Text = "Error:  Input is not Valid!"
                    Label15.Text = ""
                    Me.txt_Search_SearchString.Focus()
                    Exit Sub
                End If
                mySearchString = " " & mySearchString & " "
                If InStr(1, mySearchString, "CREATE", 1) > 0 Or InStr(1, mySearchString, "DELETE", 1) > 0 Or InStr(1, mySearchString, "DROP", 1) > 0 Or InStr(1, mySearchString, "INSERT", 1) > 1 Or InStr(1, mySearchString, "TRACK", 1) > 1 Or InStr(1, mySearchString, "TRACE", 1) > 1 Then
                    Label6.Text = "Error:  Input is not Valid !"
                    Label15.Text = ""
                    Me.txt_Search_SearchString.Focus()
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
                    Me.txt_Search_SearchString.Focus()
                    Exit Sub
                End If
            Else
                mySearchString = String.Empty
            End If

            'Field Name validation
            Dim myfield As String = Nothing
            If DropDownList5.Text <> "" Then
                myfield = TrimAll(DropDownList5.SelectedValue)
                myfield = RemoveQuotes(myfield)
                If myfield.Length > 50 Then
                    Label6.Text = "Error: Input is not Valid !"
                    Label15.Text = ""
                    Me.DropDownList5.Focus()
                    Exit Sub
                End If
                myfield = " " & myfield & " "
                If InStr(1, myfield, "CREATE", 1) > 0 Or InStr(1, myfield, "DELETE", 1) > 0 Or InStr(1, myfield, "DROP", 1) > 0 Or InStr(1, myfield, "INSERT", 1) > 1 Or InStr(1, myfield, "TRACK", 1) > 1 Or InStr(1, myfield, "TRACE", 1) > 1 Then
                    Label6.Text = "Error: Input is not Valid !"
                    Label15.Text = ""
                    Me.DropDownList5.Focus()
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
                    DropDownList5.Focus()
                    Exit Sub
                End If
            Else
                myfield = "CAT_NO"
            End If

            'Boolean Operator validation
            Dim myBoolean As String = Nothing
            If DropDownList6.Text <> "" Then
                myBoolean = TrimAll(DropDownList6.SelectedValue)
                myBoolean = RemoveQuotes(myBoolean)
                If myBoolean.Length > 20 Then
                    Label6.Text = "Error: Input is not Valid !"
                    Label15.Text = ""
                    Me.DropDownList6.Focus()
                    Exit Sub
                End If
                myBoolean = " " & myBoolean & " "
                If InStr(1, myBoolean, "CREATE", 1) > 0 Or InStr(1, myBoolean, "DELETE", 1) > 0 Or InStr(1, myBoolean, "DROP", 1) > 0 Or InStr(1, myBoolean, "INSERT", 1) > 1 Or InStr(1, myBoolean, "TRACK", 1) > 1 Or InStr(1, myBoolean, "TRACE", 1) > 1 Then
                    Label6.Text = "Error: Input is not Valid !"
                    Label15.Text = ""
                    Me.DropDownList6.Focus()
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
                    DropDownList6.Focus()
                    Exit Sub
                End If
            Else
                myBoolean = "AND"
            End If

            '**********************************************************************************
            Dim SQL As String = Nothing

            If txt_Search_SearchString.Text <> "" Then
                If myfield = "CAT_NO" Then
                    If IsNumeric(mySearchString) = False Then
                        Label6.Text = "Cat Number value must be Numeric Only!"
                        Label15.Text = ""
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Cat No must be Numeric Only ');", True)
                        Exit Sub
                    Else
                        SQL = "SELECT *  FROM CATS WHERE (CAT_NO IS NOT NULL) AND (BIB_CODE <>'S') AND (CAT_NO = '" & Trim(mySearchString) & "') "
                    End If
                End If

                If myfield = "ACCESSION_NO" Then
                    SQL = "SELECT *  FROM CATS where CAT_NO = (SELECT CAT_NO FROM HOLDINGS WHERE (LIB_CODE ='" & Trim(LibCode) & "'  AND ACCESSION_NO ='" & mySearchString & "'))"
                End If

                If myfield.ToString <> "CAT_NO" And myfield.ToString <> "ACCESSION_NO" Then
                    SQL = "SELECT CAT_NO, TITLE FROM CATS_AUTHORS_VIEW WHERE (CAT_NO IS NOT NULL) AND (BIB_CODE <>'S') "
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
            End If
            If SQL <> "" Then
                SQL = SQL & " ORDER BY TITLE ASC "
            Else
                SQL = "select CAT_NO, TITLE from CATS WHERE (BIB_CODE='M') ORDER BY CAT_NO DESC "
            End If
            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            da.Fill(ds)

            dtSearch = ds.Tables(0).Copy
            If dtSearch.Rows.Count = 0 Then
                Me.DDL_Titles.DataSource = Nothing
                DDL_Titles.DataBind()
                DDL_Titles.Items.Clear()
                Label1.Text = "Total Record(s): 0 "
            Else
                RecordCount = dtSearch.Rows.Count
                Me.DDL_Titles.DataSource = dtSearch
                Me.DDL_Titles.DataTextField = "TITLE"
                Me.DDL_Titles.DataValueField = "CAT_NO"
                Me.DDL_Titles.DataBind()
                DDL_Titles.Items.Insert(0, "")
                Label1.Text = "Total Record(s): " & RecordCount
                DDL_Titles.Focus()
            End If
            ViewState("dt") = dtSearch
        Catch s As Exception
            Label6.Text = "Error: " & (s.Message())
            Label15.Text = ""
        Finally
            SqlConn.Close()
            DDL_Titles.Focus()
        End Try
    End Sub
    'display Cat Record   
    Public Sub txt_Search_SearchString_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txt_Search_SearchString.TextChanged
        Me.SearchTitle()
    End Sub
    'load / display fields
    Protected Sub DDL_Titles_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDL_Titles.SelectedIndexChanged
        Dim dt As New DataTable
        Try
            Label4.Text = ""
            Dim myCatNo As Integer = Nothing
            If DDL_Titles.Text <> "" Then
                myCatNo = DDL_Titles.SelectedValue

                Dim SQL As String = Nothing
                If myCatNo <> 0 Then
                    SQL = "SELECT *  FROM CATS_AUTHORS_VIEW WHERE (CAT_NO = '" & Trim(myCatNo) & "') "
                End If
                Dim ds As New DataSet
                Dim da As New SqlDataAdapter(SQL, SqlConn)
                If SqlConn.State = 0 Then
                    SqlConn.Open()
                End If
                da.Fill(ds)

                dt = ds.Tables(0).Copy
                SqlConn.Close()

                If dt.Rows.Count <> 0 Then
                    TR_Folder.Visible = True
                    If dt.Rows(0).Item("TITLE").ToString <> "" Then
                        Label19.Text = dt.Rows(0).Item("CAT_NO").ToString
                        If dt.Rows(0).Item("SUB_TITLE").ToString <> "" Then
                            Label16.Text = dt.Rows(0).Item("TITLE").ToString & ": " & dt.Rows(0).Item("SUB_TITLE").ToString
                        Else
                            Label16.Text = dt.Rows(0).Item("TITLE").ToString
                        End If
                    Else
                        Label16.Text = ""
                        Label19.Text = ""
                    End If

                    'authors
                    If dt.Rows(0).Item("AUTHOR1").ToString <> "" Then
                        If dt.Rows(0).Item("AUTHOR2").ToString <> "" Then
                            If dt.Rows(0).Item("AUTHOR3").ToString <> "" Then
                                Label17.Text = dt.Rows(0).Item("AUTHOR1").ToString & "; " & dt.Rows(0).Item("AUTHOR2").ToString & " and  " & dt.Rows(0).Item("AUTHOR3").ToString
                            Else
                                Label17.Text = dt.Rows(0).Item("AUTHOR1").ToString & " and " & dt.Rows(0).Item("AUTHOR2").ToString
                            End If
                        Else
                            Label17.Text = dt.Rows(0).Item("AUTHOR1").ToString
                        End If
                    Else
                        Label17.Text = ""
                    End If
                    'publisher
                    If dt.Rows(0).Item("PUB_NAME").ToString <> "" Then
                        If dt.Rows(0).Item("PLACE_OF_PUB").ToString <> "" Then
                            If dt.Rows(0).Item("YEAR_OF_PUB").ToString <> "" Then
                                Label18.Text = dt.Rows(0).Item("PUB_NAME").ToString & "; " & dt.Rows(0).Item("PLACE_OF_PUB").ToString & "; " & dt.Rows(0).Item("YEAR_OF_PUB").ToString
                            Else
                                Label18.Text = dt.Rows(0).Item("PUB_NAME").ToString & "; " & dt.Rows(0).Item("PLACE_OF_PUB").ToString
                            End If
                        Else
                            Label18.Text = dt.Rows(0).Item("PUB_NAME").ToString
                        End If
                    Else
                        Label18.Text = ""
                    End If

                    'multi-vol
                    If dt.Rows(0).Item("MULTI_VOL").ToString <> "" Then
                        Label23.Text = dt.Rows(0).Item("MULTI_VOL").ToString
                    Else
                        Label23.Text = ""
                    End If

                    If dt.Rows(0).Item("URL").ToString <> "" Then
                        Label24.Text = "<a href =" & dt.Rows(0).Item("URL").ToString & " target =new>URL</a>"
                    Else
                        Label24.Text = ""
                    End If

                    'photo
                    If dt.Rows(0).Item("PHOTO").ToString <> "" Then
                        Dim strURL As String = "~/Acquisition/Cats_GetPhoto.aspx?CAT_NO=" & myCatNo & ""
                        Image4.ImageUrl = strURL
                        Image4.Visible = True
                    Else
                        Image4.ImageUrl = Nothing
                        Image4.Visible = True
                    End If
                    Label8.Text = ""
                    PopulateImages()
                Else
                    Label19.Text = ""
                    Label16.Text = ""
                    Label17.Text = ""
                    Label18.Text = ""
                    Label23.Text = ""
                    Label24.Text = ""
                    TR_Folder.Visible = False
                    Me.bttn_DeletePhoto.Visible = False
                    Label5.Text = "Total: 0 File(s)"
                    Repeater1.DataSource = Nothing
                    Repeater1.DataBind()

                    Repeater2.DataSource = Nothing
                    Repeater2.DataBind()
                End If
            Else
                Label19.Text = ""
                Label16.Text = ""
                Label17.Text = ""
                Label18.Text = ""
                Label23.Text = ""
                Label24.Text = ""
                TR_Folder.Visible = False
                Me.bttn_DeletePhoto.Visible = False
                Label5.Text = "Total: 0 File(s)"
                Repeater1.DataSource = Nothing
                Repeater1.DataBind()

                Repeater2.DataSource = Nothing
                Repeater2.DataBind()
            End If
        Catch ex As Exception
            Label6.Text = "Error: " & (ex.Message())
            Label15.Text = ""
        Finally
            dt.Dispose()
            SqlConn.Close()
            DDL_Titles.Focus()
        End Try
    End Sub
    'create LIBCODE as photo folder if not exist
    Public Sub CreateRootFolder() 'Libcode
        Try
            Dim pathToCreate As String = "~/DFILES/" & Trim(LibCode)
            ' folder exist message
            If Not Directory.Exists(Server.MapPath(pathToCreate)) Then
                Directory.CreateDirectory(Server.MapPath(pathToCreate))
            End If
        Catch ex As Exception
            Label4.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    'display folders in treeview
    Dim hddPath As String
    Public Sub Treeview1_TreeNodePopulate(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.TreeNodeEventArgs) Handles Treeview1.TreeNodePopulate
        Dim pathToCreate As String = Nothing
        pathToCreate = "~/DFILES/" & Session.Item("LoggedLibcode") & "/" & DDL_Titles.SelectedValue & "/"
        hddPath = Server.MapPath(pathToCreate)
        If e.Node.ChildNodes.Count = 0 Then
            LoadChildNode(hddPath, e.Node)
        End If
    End Sub
    Private Sub LoadChildNode(ByVal p1 As String, ByVal node As TreeNode)
        Dim directory As DirectoryInfo
        directory = New DirectoryInfo(p1)

        For Each subtree As DirectoryInfo In directory.GetDirectories()
            Dim subNode As TreeNode = New TreeNode(subtree.Name)
            subNode.Value = subtree.FullName.Replace(hddPath, "")

            If subtree.GetDirectories().Length > 0 Then
                LoadChildNode(subtree.FullName, subNode)
                subNode.NavigateUrl = ""
            End If

            subNode.SelectAction = TreeNodeSelectAction.SelectExpand
            node.ChildNodes.Add(subNode)
        Next
    End Sub
    Private Sub Treeview1_SelectedNodeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Treeview1.SelectedNodeChanged
        If Treeview1.SelectedNode.Value = "Files" Then
            Label8.Text = ""
            PopulateImages() '(ImagePAth)
        Else
            Label8.Text = Treeview1.SelectedNode.Value
            PopulateImages() '(ImagePAth)
        End If
    End Sub
    'create folder in selected folder
    Protected Sub CreateFolder_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CreateFolder_Bttn.Click
        Try
            If txt_Folder_Name.Text <> "" Then
                Dim pathToCreate As String = Nothing
                If Label8.Text <> "" Then
                    pathToCreate = "~/DFILES/" & Trim(LibCode) & "/" & DDL_Titles.SelectedValue & "/" & TrimX(Label8.Text) & "/" & TrimX(txt_Folder_Name.Text)
                Else
                    pathToCreate = "~/DFILES/" & Trim(LibCode) & "/" & DDL_Titles.SelectedValue & "/" & TrimX(txt_Folder_Name.Text)
                End If

                If Not Directory.Exists(Server.MapPath(pathToCreate)) Then
                    Directory.CreateDirectory(Server.MapPath(pathToCreate))
                    Label4.Text = "Folder created!"

                    Dim t1 As TreeNode
                    t1 = Treeview1.Nodes(0)
                    If t1.ChildNodes.Count > 0 Then
                        t1.ChildNodes.Clear()
                    End If
                    hddPath = Server.MapPath("~/DFILES/" & Trim(LibCode) & "/" & DDL_Titles.SelectedValue)  'Server.MapPath(pathToCreate) 'Server.MapPath("~/PHOTO/") & Session.Item("LoggedLibcode")
                    LoadChildNode(hddPath, t1)
                    Treeview1.ExpandAll()
                Else
                    Label4.Text = "Folder Already Exists!"
                End If
            Else
                Label4.Text = "Type Folder Name in the Text Box!"
                txt_Folder_Name.Focus()
            End If

        Catch ex As Exception
            Label4.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    'delte selected folder
    Protected Sub DeleteFolder_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles DeleteFolder_Bttn.Click
        Try

            Dim pathToDelete As String = Nothing
            If Label8.Text <> "" Then
                pathToDelete = "~/DFILES/" & Trim(LibCode) & "/" & DDL_Titles.SelectedValue & "/" & TrimX(Label8.Text) & "/"
                'pathToDelete = Replace(pathToDelete, "\", "/")
                If Directory.Exists(Server.MapPath(pathToDelete)) = True Then
                    Directory.Delete(Server.MapPath(pathToDelete))
                    Label4.Text = "Folder Deleted!"
                    Dim t1 As TreeNode
                    t1 = Treeview1.Nodes(0)
                    If t1.ChildNodes.Count > 0 Then
                        t1.ChildNodes.Clear()
                    End If
                    hddPath = Server.MapPath("~/DFILES/" & Trim(LibCode) & "/" & DDL_Titles.SelectedValue)
                    LoadChildNode(hddPath, t1)
                    Treeview1.ExpandAll()
                Else
                    Label4.Text = "Folder does not Exists!"
                End If
            Else
                Label4.Text = "No Folder Exists!"
            End If
        Catch ex As Exception
            Label4.Text = "Error: " & (ex.Message()) & " First Delete Files and Sub-Folder"
        End Try
    End Sub
    Protected Sub Chb_Changed(ByVal sender As Object, ByVal e As EventArgs)
        If sender IsNot Nothing Then
            Dim cb As CheckBox = DirectCast(sender, CheckBox)
            Dim clickedCheckBoxID As String = cb.ID
        End If
    End Sub
    Public Sub PopulateImages() '(ByVal ImagePath As String)
        Try
            If ImagePAth = "" Then
                If Label8.Text <> "" Then
                    ImagePAth = "~/DFILES/" & Trim(LibCode) & "/" & DDL_Titles.SelectedValue & "/" & TrimX(Label8.Text)
                    ImagePAth = Replace(ImagePAth, "\", "/")
                    ImagePAth = Replace(ImagePAth, "//", "/")
                Else
                    ImagePAth = "~/DFILES/" & Trim(LibCode) & "/" & DDL_Titles.SelectedValue
                End If
                ' folder exist message
                If Not Directory.Exists(Server.MapPath(ImagePAth)) Then
                    Treeview1.Nodes.Clear()
                    Treeview1.Dispose()
                    TR_Folder.Visible = False
                    Label5.Text = "Total: 0 File(s)"
                    Repeater1.DataSource = Nothing
                    Repeater1.DataBind()

                    Repeater2.DataSource = Nothing
                    Repeater2.DataBind()
                    bttn_DeletePhoto.Visible = False
                    Exit Sub
                Else
                    TR_Folder.Visible = False
                    Treeview1.Nodes.Clear()
                    Treeview1.Dispose()
                    Dim ParentNode As TreeNode = New TreeNode
                    ParentNode.Text = "Files"
                    Treeview1.Nodes.Add(ParentNode)

                    Dim pathToCreate As String = Nothing
                    pathToCreate = "~/DFILES/" & Session.Item("LoggedLibcode") & "/" & DDL_Titles.SelectedValue & "/"
                    hddPath = Server.MapPath(pathToCreate)
                    LoadChildNode((hddPath), Treeview1.Nodes(0))
                    Label5.Text = "Total: 0 File(s)"
                    bttn_DeletePhoto.Visible = False
                End If
            End If

            If ImagePAth <> "" Then
                If Not Directory.Exists(Server.MapPath(ImagePAth)) Then
                    Treeview1.Nodes.Clear()
                    Treeview1.Dispose()
                    TR_Folder.Visible = False
                    Label5.Text = "Total: 0 File(s)"
                    Exit Sub
                End If
                TR_Folder.Visible = True
                files = Directory.GetFiles(Server.MapPath(ImagePAth))
                'files = Directory.GetFiles(Server.MapPath(ImagePAth),"*.*",SearchOption.AllDirectories).Where(s => s.ToLower().EndsWith(".jpg") || s.ToLower().EndsWith(".txt") || s.ToLower().EndsWith(".asp")))

                Dim fCount As Integer = 0
                fCount = files.Count()
                If fCount > 0 Then
                    bttn_DeletePhoto.Visible = True
                Else
                    bttn_DeletePhoto.Visible = False
                End If
                Label5.Text = "Total: " & fCount & " File(s)"

                Dim arrIbs(files.Length) As ImageButton
                For i = 0 To files.Length - 1
                    Dim FileExtn As String = Nothing
                    FileExtn = System.IO.Path.GetExtension(files(i).ToString())
                    bttn_DeletePhoto.Visible = True
                    If LCase(FileExtn) = ".bmp" Or FileExtn = ".emf" Or FileExtn = ".exif" Or FileExtn = ".gif" Or FileExtn = ".icon" Or FileExtn = ".jpeg" Or FileExtn = ".jpg" Or FileExtn = ".png" Or FileExtn = ".tiff" Or FileExtn = ".tif" Or FileExtn = ".wmf" Then
                        image.Add(ImagePAth + "/" + Path.GetFileName(files(i).ToString))
                    Else
                        nonimage.Add(ImagePAth + "/" + Path.GetFileName(files(i).ToString))
                    End If
                Next

                Repeater1.DataSource = image
                Repeater1.DataBind()

                Repeater2.DataSource = nonimage
                Repeater2.DataBind()

                Treeview1.ExpandAll()
            End If
            ImagePAth = Nothing
        Catch ex As Exception
            Label6.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    Protected Sub bttn_Upload_Click(ByVal sender As Object, ByVal e As EventArgs) Handles bttn_Upload.Click
        If IsPostBack = True Then
            Try
                If Label19.Text <> "" Then
                    If FileUpload1.FileName = "" Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Select Proper file... ');", True)
                        Label4.Text = "File not selected!"
                        Me.FileUpload1.Focus()
                        Exit Sub
                    Else
                        'If FileUpload1.PostedFile.ContentLength > 1500000 Then
                        '    Label6.Text = "Error: Photo Size is Bigger than 15 KB"
                        '    Exit Sub
                        'Else
                        ImagePAth = "~/DFILES/" & Session.Item("LoggedLibcode") & "/" & DDL_Titles.SelectedValue & "/" & TrimX(Label8.Text)
                        ImagePAth = Replace(ImagePAth, "\", "/")
                        ImagePAth = Replace(ImagePAth, "//", "/")
                        If Not Directory.Exists(Server.MapPath(ImagePAth)) Then
                            Directory.CreateDirectory(Server.MapPath(ImagePAth))
                        End If

                        If (FileUpload1.PostedFile IsNot Nothing) AndAlso (FileUpload1.PostedFile.ContentLength > 0) Then
                            Dim fn As String = System.IO.Path.GetFileName(FileUpload1.PostedFile.FileName)
                            Dim SaveLocation As String = Server.MapPath(ImagePAth) & "\" & fn
                            FileUpload1.PostedFile.SaveAs(SaveLocation)
                            Label4.Text = "The file has been uploaded."
                            PopulateImages()
                        Else
                            Response.Write("Please select a file to upload.")
                        End If
                    End If
                Else
                    Label4.Text = "Title not selected!"
                End If
            Catch ex As Exception
                Label4.Text = "Error: " & (ex.Message())
            End Try
        End If
    End Sub
    'delete selected photo
    Protected Sub bttn_DeletePhoto_Click(ByVal sender As Object, ByVal e As EventArgs) Handles bttn_DeletePhoto.Click
        Dim content As ContentPlaceHolder = Nothing
        content = Page.Master.FindControl("MainContent")
        Dim allTextBoxValues As String = ""
        Try
            For Each ri As RepeaterItem In Repeater1.Items
                Dim cb As CheckBox
                cb = ri.FindControl("chk")

                If cb.Checked = True Then
                    'delete                 
                    Dim myDelPath As String
                    If Label8.Text <> "" Then
                        myDelPath = "~/DFILES/" & Trim(LibCode) & "/" & DDL_Titles.SelectedValue & "/" & TrimX(Label8.Text) & "/" & CType(cb, CheckBox).Text
                    Else
                        myDelPath = "~/DFILES/" & Trim(LibCode) & "/" & DDL_Titles.SelectedValue & "/" & CType(cb, CheckBox).Text
                    End If
                    myDelPath = Replace(myDelPath, "\", "/")
                    myDelPath = Replace(myDelPath, "//", "/")
                    If File.Exists(Server.MapPath(myDelPath)) = True Then
                        File.Delete(Server.MapPath(myDelPath))
                        allTextBoxValues &= CType(cb, CheckBox).Text & ","
                    End If
                End If
            Next

            For Each ri As RepeaterItem In Repeater2.Items
                For Each c As Control In ri.Controls
                    If TypeOf c Is CheckBox Then
                        If CType(c, CheckBox).Checked = True Then
                            'delete                 
                            Dim myDelPath As String
                            If Label8.Text <> "" Then
                                myDelPath = "~/DFILES/" & Trim(LibCode) & "/" & DDL_Titles.SelectedValue & "/" & TrimX(Label8.Text) & "/" & CType(c, CheckBox).Text
                            Else
                                myDelPath = "~/DFILES/" & Trim(LibCode) & "/" & DDL_Titles.SelectedValue & "/" & CType(c, CheckBox).Text
                            End If
                            myDelPath = Replace(myDelPath, "\", "/")
                            myDelPath = Replace(myDelPath, "//", "/")
                            If File.Exists(Server.MapPath(myDelPath)) = True Then
                                File.Delete(Server.MapPath(myDelPath))
                                allTextBoxValues &= CType(c, CheckBox).Text & ","
                            End If
                        End If
                    End If
                Next
            Next
            Label4.Text = allTextBoxValues & " File(s) deleted successfully!"
            PopulateImages() '(ImagePAth) '(ImagePAth)
        Catch ex As Exception
            Label4.Text = "Error: " & (ex.Message())
        End Try
    End Sub


   
   
End Class