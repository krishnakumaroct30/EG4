Public Class FileExt
    Public Shared Function ContentType(ByVal FileExtension As String) As String
        Dim d As New Dictionary(Of String, String)
        'Images'
        d.Add(".bmp", "image/bmp")
        d.Add(".gif", "image/gif")
        d.Add(".jpeg", "image/jpeg")
        d.Add(".jpg", "image/jpeg")
        d.Add(".png", "image/png")
        d.Add(".tif", "image/tiff")
        d.Add(".tiff", "image/tiff")
        'Documents'
        d.Add(".doc", "application/msword")
        d.Add(".docx", "application/msword")
        d.Add(".pdf", "application/pdf")
        d.Add(".htm", "application/htm")
        d.Add(".html", "application/htm")
        'Slideshows'
        d.Add(".ppt", "application/vnd.ms-powerpoint")
        d.Add(".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation")
        'Data'
        d.Add(".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        d.Add(".xls", "application/vnd.ms-excel")
        d.Add(".csv", "text/csv")
        d.Add(".xml", "text/xml")
        d.Add(".txt", "text/plain")
        d.Add(".rtf", "application/msword")
        'Compressed Folders'
        d.Add(".zip", "application/zip")
        'Audio'
        d.Add(".ogg", "application/ogg")
        d.Add(".mp3", "audio/mpeg")
        d.Add(".wma", "audio/x-ms-wma")
        d.Add(".wav", "audio/x-wav")
        'Video'
        d.Add(".wmv", "application/mpeg")
        d.Add(".swf", "application/x-shockwave-flash")
        d.Add(".avi", "video/avi")
        d.Add(".mp4", "video/mp4")
        d.Add(".mpeg", "video/mpeg")
        d.Add(".mpg", "video/mpeg")
        d.Add(".VOB", "video/mped")
        d.Add(".qt", "video/quicktime")
        Return d(FileExtension)
    End Function
End Class
