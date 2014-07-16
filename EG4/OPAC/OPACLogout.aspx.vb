Public Class OPACLogout
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'If Session.Item("LoggedUser") = Nothing Then
            '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Successfully Logut !');", True)
            '    Response.Redirect("Default.aspx")
            'Else
            '    Dim authcookie1, var1, var2 As Object
            '    authcookie1 = Request.Cookies("AUTHCookie").Value

            '    var1 = Trim(Session("authcookie"))
            '    var2 = Trim(authcookie1)
            '    If (var1 = "") Then
            '        Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Sorry..You are not looged in...Please Restart The Application');", True)
            '        Response.Redirect("Default.aspx")
            '        Session.Abandon()
            '    Else
            '        If var1 <> var2 Then
            '            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Sorry..Invalid Session..Please Restart The Application');", True)
            '            Response.Redirect("Default.aspx")
            '            Session.Abandon()
            '            Response.End()
            '        Else
            '            If Session.Item("LoggedUser") <> Nothing Then

            If Not SqlConn.State = ConnectionState.Open Then
                SqlConn.Close()
            End If


            'If IsPostBack = False Then
            Session.Clear()
            Session.Abandon()
            Session.RemoveAll()
            myPaneName = Nothing

            'End If
            'End If
            'End If
            '            Else
            'ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Sorry..Session Is Out..Please Restart The Application');", True)
            Response.Redirect("~/OPAC/Default.aspx", False)
            'Session.Abandon()
            'Response.End()
            '            End If
            '        End If
            '    End If
            'End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Close the Application !');", True)
        End Try
    End Sub

End Class