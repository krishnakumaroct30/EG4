Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Net
Imports EG4.FileExt
Public Class Display_Thumb_Image
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim thumbfilepath As String = Request.QueryString("ThumbImage")
        Dim fullfilepath As String = Request.QueryString("FullImage")
        Dim pdffilepath As String = Request.QueryString("ebp")
        If thumbfilepath <> String.Empty Then
            'Dim img As Image = Image.FromFile(HttpRuntime.AppDomainAppPath + "\" + thumbfilepath)
            'img.Save(Context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Png)
            'Context.Response.ContentType = "image/png"

            'Dim bmp As Bitmap = CreateThumbnail(Server.MapPath("~") + "\" + thumbfilepath.Replace("/", "\"), 50, 80)
            Dim bmp As Bitmap = CreateThumbnail(Server.MapPath(thumbfilepath.Replace("/", "\")), 80, 80)
            If bmp Is Nothing Then
                Me.ErrorResult()
                Return
            End If

            Dim OutputFilename As String = Nothing
            OutputFilename = Request.QueryString("OutputFilename")

            If Not OutputFilename Is Nothing Then
                'If Me.User.Identity.Name = "" Then
                '    ' *** Custom error display here
                '    bmp.Dispose()
                '    Me.ErrorResult()
                'End If
                Try
                    bmp.Save(OutputFilename)
                Catch ex As Exception
                    bmp.Dispose()
                    Me.ErrorResult()
                    Return
                End Try
            End If

            ' Put user code to initialize the page here
            Response.ContentType = "image/jpeg"
            bmp.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg)
            bmp.Dispose()
        ElseIf fullfilepath <> String.Empty Then
            ' Dim img As Image = Image.FromFile(Server.MapPath("~") + "\" + filepath.Replace("/", "\"))
            Dim img As Image = Image.FromFile(Server.MapPath(fullfilepath.Replace("/", "\")))
            img.Save(Context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg)
            Context.Response.ContentType = "image/jpeg"
            img.Dispose()

        ElseIf pdffilepath <> String.Empty Then
            loadpdffile(pdffilepath)
        End If

    End Sub

    Public Shared Function CreateThumbnail(ByVal lcFilename As String, ByVal lnWidth As Integer, ByVal lnHeight As Integer) As Bitmap

        Dim bmpOut As System.Drawing.Bitmap = Nothing
        Try
            Dim loBMP As Bitmap = New Bitmap(lcFilename)
            Dim loFormat As ImageFormat = loBMP.RawFormat

            Dim lnRatio As Decimal
            Dim lnNewWidth As Integer = 0
            Dim lnNewHeight As Integer = 0

            '*** If the image is smaller than a thumbnail just return it
            If loBMP.Width < lnWidth And loBMP.Height < lnHeight Then
                Return loBMP
            End If


            If loBMP.Width > loBMP.Height Then
                lnRatio = CType(lnWidth / loBMP.Width, Decimal)
                lnNewWidth = lnWidth
                Dim lnTemp As Decimal = loBMP.Height * lnRatio
                lnNewHeight = CType(lnTemp, Integer)
            Else
                lnRatio = CType(lnHeight / loBMP.Height, Decimal)
                lnNewHeight = lnHeight
                Dim lnTemp As Decimal = loBMP.Width * lnRatio
                lnNewWidth = CType(lnTemp, Integer)
            End If

            ' System.Drawing.Image imgOut =
            '      loBMP.GetThumbnailImage(lnNewWidth,lnNewHeight,
            '                              null,IntPtr.Zero);

            ' *** This code creates cleaner (though bigger) thumbnails and properly
            ' *** and handles GIF files better by generating a white background for
            ' *** transparent images (as opposed to black)
            bmpOut = New Bitmap(lnNewWidth, lnNewHeight)
            Dim g As Graphics = Graphics.FromImage(bmpOut)
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic
            g.FillRectangle(Brushes.White, 0, 0, lnNewWidth, lnNewHeight)
            g.DrawImage(loBMP, 0, 0, lnNewWidth, lnNewHeight)
            loBMP.Dispose()
        Catch
            Return Nothing
        End Try

        Return bmpOut
    End Function
    Private Sub ErrorResult()
        Response.Clear()
        Response.StatusCode = 404
        Response.End()
    End Sub
    Public Sub loadpdffile(ByVal path As String)
        Dim client As New WebClient()
        Dim buffer As [Byte]() = client.DownloadData(Server.MapPath(path.Replace("/", "\")))

        If buffer IsNot Nothing Then
            Dim ext As String
            ext = System.IO.Path.GetExtension(path)

            Dim ct As String
            ct = FileExt.ContentType(ext)
            If ct <> String.Empty Then
                Response.ContentType = ct
                Response.AddHeader("content-length", buffer.Length.ToString())
                Response.BinaryWrite(buffer)
            End If

        End If
    End Sub
End Class