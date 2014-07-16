Imports System.Web
Imports System.Text
Imports System.Web.UI
Public Class Alert
    Public Shared Sub Show(ByVal message As String)
        ' Cleans the message to allow single quotation marks 
        Dim cleanMessage As String = message.Replace("'", "\'")
        Dim script As String = "<script type=""text/javascript"">alert('" & cleanMessage & "');</script>"

        ' Gets the executing web page 
        Dim page As Page = TryCast(HttpContext.Current.CurrentHandler, Page)
        ' Checks if the handler is a Page and that the script isn't allready on the Page 
        If page IsNot Nothing AndAlso Not page.ClientScript.IsClientScriptBlockRegistered("alert") Then
            page.ClientScript.RegisterClientScriptBlock(GetType(Alert), "alert", script)
        End If
    End Sub
End Class


