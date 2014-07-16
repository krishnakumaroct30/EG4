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
Imports System.Drawing
Imports System.Drawing.Bitmap
Imports System.Drawing.Drawing2D
Public Class PhotoGallery
    Inherits System.Web.UI.Page
    Dim files() As String
    Dim ImagePAth As String = Nothing
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session.Item("IsThisU") <> Nothing Then
                If Not SConnection() = True Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Database Connection is lost..Try Again !');", True)
                    Label6.Text = "Database Connection is Lost!"
                Else
                    If Page.IsPostBack = False Then
                        CreatePhotoFolder()
                    End If
                End If
                Dim CreateSUserButton As Object
                CreateSUserButton = Master.FindControl("Accordion1").FindControl("LibAdminPane").FindControl("Lib_PhotoGallery_Bttn")
                CreateSUserButton.ForeColor = Drawing.Color.Red
                myPaneName = "LibAdminPane" 'paneSelectedIndex = 0
            Else
                Response.Redirect("~/Default.aspx", False)
            End If
            LibCode = Session.Item("LoggedLibcode")

            Dim myP As String = Nothing
            myP = Request.QueryString("Img")
            If myP <> "" Then
                imgMain.ImageUrl = myP
                If ImagePAth = "" Then
                    ImagePAth = "~/PHOTO/" & Trim(LibCode) & "/" & TrimX(Label8.Text)
                    PopulateImages(ImagePAth)
                End If
            Else
                If Label8.Text <> "" Then
                    ImagePAth = "~/PHOTO/" & Trim(LibCode) & "/" & TrimX(Label8.Text)
                Else
                    ImagePAth = "~/PHOTO/" & Trim(LibCode)
                End If
                PopulateImages(ImagePAth)
            End If

        Catch ex As Exception
            Label6.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    'create LIBCODE as photo folder if not exist
    Public Sub CreatePhotoFolder() 'Libcode
        Try
            Dim pathToCreate As String = "~/PHOTO/" & Trim(LibCode)
            ' folder exist message
            If Not Directory.Exists(Server.MapPath(pathToCreate)) Then
                Directory.CreateDirectory(Server.MapPath(pathToCreate))
            End If
        Catch ex As Exception
            Label6.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    'display folders in treeview
    Dim hddPath As String
    Public Sub Treeview1_TreeNodePopulate(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.TreeNodeEventArgs) Handles Treeview1.TreeNodePopulate
        Dim pathToCreate As String = Nothing
        pathToCreate = "~/PHOTO/" & Session.Item("LoggedLibcode")
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
        If Treeview1.SelectedNode.Value = "Photos" Then
            Label8.Text = ""
        Else
            Label8.Text = Treeview1.SelectedNode.Value
            ImagePAth = "~/PHOTO/" & Trim(LibCode) & "/" & TrimX(Label8.Text)
            ImagePAth = Replace(ImagePAth, "\", "/")
            ImagePAth = Replace(ImagePAth, "//", "/")
            PopulateImages(ImagePAth)
        End If
    End Sub
    'create folder in selected folder
    Protected Sub CreateFolder_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CreateFolder_Bttn.Click
        Try
            Dim pathToCreate As String = Nothing
            If Label8.Text <> "" Then
                pathToCreate = "~/PHOTO/" & Trim(LibCode) & "/" & Label8.Text & "/" & TrimX(txt_Folder_Name.Text)
            Else
                pathToCreate = "~/PHOTO/" & Trim(LibCode) & "/" & TrimX(txt_Folder_Name.Text)
            End If

            If txt_Folder_Name.Text <> "" Then
                If Not Directory.Exists(Server.MapPath(pathToCreate)) Then
                    Directory.CreateDirectory(Server.MapPath(pathToCreate))
                    Label6.Text = "Folder created!"

                    Dim t1 As TreeNode
                    t1 = Treeview1.Nodes(0)
                    If t1.ChildNodes.Count > 0 Then
                        t1.ChildNodes.Clear()
                    End If
                    hddPath = Server.MapPath("~/PHOTO/") & Session.Item("LoggedLibcode")
                    LoadChildNode(hddPath, t1)
                    Treeview1.ExpandAll()
                Else
                    Label6.Text = "Folder Already Exists!"
                End If
            Else
                Label6.Text = "Type Folder Name in the Text Box!"
                txt_Folder_Name.Focus()
            End If

        Catch ex As Exception
            Label6.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    'delte selected folder
    Protected Sub DeleteFolder_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles DeleteFolder_Bttn.Click
        Try

            Dim pathToDelete As String = Nothing
            If Label8.Text <> "" Then
                pathToDelete = "~/PHOTO/" & Trim(LibCode) & "/" & Label8.Text & "/"
                'pathToDelete = Replace(pathToDelete, "\", "/")
                If Directory.Exists(Server.MapPath(pathToDelete)) = True Then
                    Directory.Delete(Server.MapPath(pathToDelete))
                    Label6.Text = "Folder Deleted!"
                    Label8.Text = ""
                    Dim t1 As TreeNode
                    t1 = Treeview1.Nodes(0)
                    If t1.ChildNodes.Count > 0 Then
                        t1.ChildNodes.Clear()
                    End If
                    hddPath = Server.MapPath("~/PHOTO/") & Session.Item("LoggedLibcode")
                    LoadChildNode(hddPath, t1)
                    Treeview1.ExpandAll()
                Else
                    Label6.Text = "Folder does not Exists!"
                End If
            Else
                Label6.Text = "No Folder Exists!"
            End If
        Catch ex As Exception
            Label6.Text = "Error: " & (ex.Message()) & " First Delete Files and Sub-Folder"
        End Try
    End Sub
    Public Sub PopulateImages(ByVal ImagePath As String)
        Try
            'If Label8.Text <> "" Then
            pnlThumbs.Controls.Clear()
            If ImagePath = "" Then
                ImagePath = "~/PHOTO/" & Trim(LibCode) & "/" & TrimX(Label8.Text)
            End If

            If ImagePath <> "" Then
                Dim i As Integer
                files = Directory.GetFiles(Server.MapPath(ImagePath))
                Dim fCount As Integer = 0
                fCount = files.Count()
                Label1.Text = "Total: " & fCount & " File(s)"

                Dim arrIbs(files.Length) As ImageButton

                For i = 0 To files.Length - 1
                    Dim image As New Bitmap(files(i).ToString)
                    arrIbs(i) = New ImageButton()
                    arrIbs(i).ImageUrl = ImagePath & "/" & System.IO.Path.GetFileName(files(i).ToString()) '"/ashok.GIF"
                    arrIbs(i).Width = 100
                    arrIbs(i).Height = 100
                    arrIbs(i).BorderStyle = BorderStyle.Inset
                    arrIbs(i).BorderWidth = 2
                    arrIbs(i).AlternateText = _
                        System.IO.Path.GetFileName(files(i).ToString())
                    arrIbs(i).PostBackUrl = "PhotoGallery.aspx?Img=" & ImagePath & "/" & System.IO.Path.GetFileName(files(i).ToString())
                    Me.pnlThumbs.Controls.Add(arrIbs(i))

                    Dim chk As New CheckBox
                    chk.Text = System.IO.Path.GetFileName(files(i).ToString())
                    chk.Checked = False
                    pnlThumbs.Controls.Add(chk)

                    image.Dispose()
                    image = Nothing
                Next
                UpdatePanel1.Update()
            End If
            ' End If
        Catch ex As Exception
            Label6.Text = "Error: " & (ex.Message())
        End Try

    End Sub
    'upload photo
    Protected Sub bttn_Upload_Click(ByVal sender As Object, ByVal e As EventArgs) Handles bttn_Upload.Click
        ImagePAth = "~/PHOTO/" & Trim(LibCode) & "/" & TrimX(Label8.Text)
        ImagePAth = Replace(ImagePAth, "\", "/")
        ImagePAth = Replace(ImagePAth, "//", "/")

        If IsPostBack = True Then
            Try
                If FileUpload1.FileName = "" Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Please Select Proper file... ');", True)
                    Label6.Text = "File not selected!"
                    Me.FileUpload1.Focus()
                    Exit Sub
                Else
                    If FileUpload1.PostedFile.ContentLength > 1500000 Then
                        Label6.Text = "Error: Photo Size is Bigger than 15 KB"
                        Exit Sub
                    Else
                        If (FileUpload1.PostedFile IsNot Nothing) AndAlso (FileUpload1.PostedFile.ContentLength > 0) Then
                            Dim fn As String = System.IO.Path.GetFileName(FileUpload1.PostedFile.FileName)
                            Dim SaveLocation As String = Server.MapPath(ImagePAth) & "\" & fn
                            FileUpload1.PostedFile.SaveAs(SaveLocation)
                            Label6.Text = "The file has been uploaded."
                            imgMain.ImageUrl = ImagePAth & "/" & FileUpload1.FileName
                            PopulateImages(ImagePAth)
                        Else
                            Response.Write("Please select a file to upload.")
                        End If
                    End If
                End If
            Catch ex As Exception
                Label6.Text = "Error: " & (ex.Message())
            End Try
        End If
    End Sub
    'delete selected photo
    Protected Sub bttn_DeletePhoto_Click(ByVal sender As Object, ByVal e As EventArgs) Handles bttn_DeletePhoto.Click
        Dim content As ContentPlaceHolder = Nothing
        content = Page.Master.FindControl("MainContent")
        Dim allTextBoxValues As String = ""
        Dim mypnl As Panel = Nothing
        mypnl = content.FindControl("pnlThumbs")
        Try
            For Each c As Control In mypnl.Controls
                If TypeOf c Is CheckBox Then
                    If CType(c, CheckBox).Checked = True Then
                        'delete                 
                        Dim myDelPath As String
                        myDelPath = "~/PHOTO/" & Trim(LibCode) & "/" & TrimX(Label8.Text) & "/" & CType(c, CheckBox).Text
                        myDelPath = Replace(myDelPath, "\", "/")
                        myDelPath = Replace(myDelPath, "//", "/")
                        If File.Exists(Server.MapPath(myDelPath)) = True Then
                            File.Delete(Server.MapPath(myDelPath))
                            allTextBoxValues &= CType(c, CheckBox).Text & ","
                        End If
                    End If
                End If
            Next
            Label6.Text = allTextBoxValues & " File(s) deleted successfully!"
            PopulateImages(ImagePAth)
        Catch ex As Exception
            Label6.Text = "Error: " & (ex.Message())
        End Try
    End Sub
      
   
End Class