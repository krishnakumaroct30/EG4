Imports System
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Drawing.Text
Namespace CaptchaImage
    Public Class CaptchaImage
        ' Public properties (all read-only). 
        Public ReadOnly Property Text() As String
            Get
                Return Me.m_text
            End Get
        End Property
        Public ReadOnly Property Image() As Bitmap
            Get
                Return Me.m_image
            End Get
        End Property
        Public ReadOnly Property Width() As Integer
            Get
                Return Me.m_width
            End Get
        End Property
        Public ReadOnly Property Height() As Integer
            Get
                Return Me.m_height
            End Get
        End Property

        ' Internal properties. 
        Private m_text As String
        Private m_width As Integer
        Private m_height As Integer
        Private familyName As String
        Private m_image As Bitmap

        ' For generating random numbers. 
        Private random As New Random()

        ' ==================================================================== 
        ' Initializes a new instance of the CaptchaImage class using the 
        ' specified text, width and height. 
        ' ==================================================================== 
        Public Sub New(ByVal s As String, ByVal width As Integer, ByVal height As Integer)
            Me.m_text = s
            Me.SetDimensions(width, height)
            Me.GenerateImage()
        End Sub

        ' ==================================================================== 
        ' Initializes a new instance of the CaptchaImage class using the 
        ' specified text, width, height and font family. 
        ' ==================================================================== 
        Public Sub New(ByVal s As String, ByVal width As Integer, ByVal height As Integer, ByVal familyName As String)
            Me.m_text = s
            Me.SetDimensions(width, height)
            Me.SetFamilyName(familyName)
            Me.GenerateImage()
        End Sub
        Protected Overrides Sub Finalize()
            Try

                ' ==================================================================== 
                ' This member overrides Object.Finalize. 
                ' ==================================================================== 
                Dispose(False)
            Finally
                MyBase.Finalize()
            End Try
        End Sub

        ' ==================================================================== 
        ' Releases all resources used by this object. 
        ' ==================================================================== 
        Public Sub Dispose()
            GC.SuppressFinalize(Me)
            Me.Dispose(True)
        End Sub

        ' ==================================================================== 
        ' Custom Dispose method to clean up unmanaged resources. 
        ' ==================================================================== 
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
                ' Dispose of the bitmap. 
                Me.m_image.Dispose()
            End If
        End Sub

        ' ==================================================================== 
        ' Sets the image width and height. 
        ' ==================================================================== 
        Private Sub SetDimensions(ByVal width As Integer, ByVal height As Integer)
            ' Check the width and height. 
            If width <= 0 Then
                Throw New ArgumentOutOfRangeException("width", width, "Argument out of range, must be greater than zero.")
            End If
            If height <= 0 Then
                Throw New ArgumentOutOfRangeException("height", height, "Argument out of range, must be greater than zero.")
            End If
            Me.m_width = width
            Me.m_height = height
        End Sub

        ' ==================================================================== 
        ' Sets the font used for the image text. 
        ' ==================================================================== 
        Private Sub SetFamilyName(ByVal familyName As String)
            ' If the named font is not installed, default to a system font. 
            Try
                Dim font As New Font(Me.familyName, 12.0F)
                Me.familyName = familyName
                font.Dispose()
            Catch ex As Exception
                Me.familyName = System.Drawing.FontFamily.GenericSerif.Name
            End Try
        End Sub

        ' ==================================================================== 
        ' Creates the bitmap image. 
        ' ==================================================================== 
        Private Sub GenerateImage()
            ' Create a new 32-bit bitmap image. 
            Dim bitmap As New Bitmap(Me.m_width, Me.m_height, PixelFormat.Format32bppArgb)

            ' Create a graphics object for drawing. 
            Dim g As Graphics = Graphics.FromImage(bitmap)
            g.SmoothingMode = SmoothingMode.AntiAlias
            Dim rect As New Rectangle(0, 0, Me.m_width, Me.m_height)

            ' Fill in the background. 
            Dim hatchBrush As New HatchBrush(HatchStyle.SmallConfetti, Color.LightGray, Color.White)
            g.FillRectangle(hatchBrush, rect)

            ' Set up the text font. 
            Dim size As SizeF
            Dim fontSize As Single = rect.Height + 1
            Dim font As Font
            ' Adjust the font size until the text fits within the image. 
            Do
                fontSize -= 1
                font = New Font(Me.familyName, fontSize, FontStyle.Bold)
                size = g.MeasureString(Me.m_text, font)
            Loop While size.Width > rect.Width

            ' Set up the text format. 
            Dim format As New StringFormat()
            format.Alignment = StringAlignment.Center
            format.LineAlignment = StringAlignment.Center

            ' Create a path using the text and warp it randomly. 
            Dim path As New GraphicsPath()
            path.AddString(Me.m_text, font.FontFamily, CInt(font.Style), font.Size, rect, format)
            Dim v As Single = 4.0F
            Dim points As PointF() = {New PointF(Me.random.[Next](rect.Width) / v, Me.random.[Next](rect.Height) / v), New PointF(rect.Width - Me.random.[Next](rect.Width) / v, Me.random.[Next](rect.Height) / v), New PointF(Me.random.[Next](rect.Width) / v, rect.Height - Me.random.[Next](rect.Height) / v), New PointF(rect.Width - Me.random.[Next](rect.Width) / v, rect.Height - Me.random.[Next](rect.Height) / v)}
            Dim matrix As New Matrix()
            matrix.Translate(0.0F, 0.0F)
            path.Warp(points, rect, matrix, WarpMode.Perspective, 0.0F)

            ' Draw the text. 
            hatchBrush = New HatchBrush(HatchStyle.LargeConfetti, Color.LightGray, Color.DarkGray)
            g.FillPath(hatchBrush, path)

            ' Add some random noise. 
            Dim m As Integer = Math.Max(rect.Width, rect.Height)
            For i As Integer = 0 To CInt((rect.Width * rect.Height / 30.0F)) - 1
                Dim x As Integer = Me.random.[Next](rect.Width)
                Dim y As Integer = Me.random.[Next](rect.Height)
                Dim w As Integer = Me.random.[Next](m / 50)
                Dim h As Integer = Me.random.[Next](m / 50)
                g.FillEllipse(hatchBrush, x, y, w, h)
            Next

            ' Clean up. 
            font.Dispose()
            hatchBrush.Dispose()
            g.Dispose()

            ' Set the image. 
            Me.m_image = bitmap
        End Sub
    End Class

End Namespace
