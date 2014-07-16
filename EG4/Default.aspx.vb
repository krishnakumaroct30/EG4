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
Public Class _Default
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not SConnection() = True Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('Database Connection is Lost mm!');", True)
            Else
                If Page.IsPostBack = False Then
                    GetLibraryDetails()
                    GetClusterDetails()
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Sub GetClusterDetails()
        Dim dtCluster As DataTable = Nothing
        Try
            Dim ds As New DataSet
            Dim SQL As String = Nothing
            SQL = "SELECT * FROM CLUSTER; "
            SqlConn.Open()
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            da.Fill(ds)

            If ds.Tables.Count > 0 Then
                dtCluster = ds.Tables(0)
            Else
                Exit Sub
            End If

            Dim CLUSTER_NAME As String = Nothing
            Dim CLUSTER_ADD As String = Nothing
            Dim URL As String = Nothing
            Dim myClusterIntro As String = Nothing
            If dtCluster.Rows.Count <> 0 Then
                If dtCluster.Rows(0).Item("NAME").ToString <> "" Then
                    CLUSTER_NAME = dtCluster.Rows(0).Item("NAME").ToString
                Else
                    CLUSTER_NAME = ""
                End If

                If dtCluster.Rows(0).Item("URL").ToString <> "" Then
                    URL = dtCluster.Rows(0).Item("URL").ToString
                Else
                    URL = ""
                End If

                If dtCluster.Rows(0).Item("ADDRESS").ToString <> "" Then
                    CLUSTER_ADD = dtCluster.Rows(0).Item("ADDRESS").ToString
                    'Lbl_ClusterIntro.Text = Replace("Contact Us: " & vbCrLf & "Cluster Name: " & CLUSTER_NAME & vbCrLf & "Address: " & CLUSTER_ADD & vbCrLf & "URL: " & URL & vbCrLf, Environment.NewLine, "<br />")
                Else
                    CLUSTER_ADD = ""
                    'Lbl_LibIntro.Text = ""
                End If

                If dtCluster.Rows(0).Item("REMARKS").ToString <> "" Then
                    myClusterIntro = dtCluster.Rows(0).Item("REMARKS").ToString.Replace(Environment.NewLine, "<br />")

                    If myClusterIntro.Length > 500 Then ' to publish limited characters
                        If InStr(myClusterIntro, "<br />") > 0 Then
                            Dim my1 As Integer = Nothing
                            my1 = myClusterIntro.IndexOf("<br />", 500)
                            myClusterIntro = myClusterIntro.Substring(0, my1)
                            my1 = Nothing
                        End If
                    End If
                    Lbl_ClusterIntro.Text = myClusterIntro
                End If

            End If


        Catch s As Exception

        Finally
            SqlConn.Close()
        End Try
    End Sub
    Public Sub GetLibraryDetails()
        Dim dtLibrary As DataTable = Nothing
        Try
            Dim ds As New DataSet
            Dim SQL As String = Nothing
            SQL = "SELECT * FROM LIBRARIES WHERE (LIB_CODE ='" & Trim(Session.Item("LoggedLibcode")) & "'); "
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            da.Fill(ds)

            If ds.Tables.Count > 0 Then
                dtLibrary = ds.Tables(0)
            Else
                Exit Sub
            End If

            Dim myLibIntro As String = Nothing
            Dim LIBRARY_NAME As String = Nothing
            Dim CONTACT_PERSON As String = Nothing
            Dim PARENT_ORG As String = Nothing
            Dim LIBRARY_ADD As String = Nothing
            Dim LIB_FAX As String = Nothing
            Dim LIB_PHONE As String = Nothing
            Dim LIB_EMAIL As String = Nothing
            Dim LIB_INCHARGE2 As String = Nothing
            Dim LIB_NAME2 As String = Nothing
            Dim PARENT_BODY2 As String = Nothing
            Dim LIB_ADD2 As String = Nothing

            If dtLibrary.Rows.Count <> 0 Then
                If dtLibrary.Rows(0).Item("LIB_INTRO").ToString <> "" Then
                    myLibIntro = dtLibrary.Rows(0).Item("LIB_INTRO").ToString.Replace(Environment.NewLine, "<br />")

                    If myLibIntro.Length > 500 Then
                        If InStr(myLibIntro, "<br />") > 0 Then
                            Dim my1 As Integer = Nothing
                            my1 = myLibIntro.IndexOf("<br />", 500)
                            myLibIntro = myLibIntro.Substring(0, my1)
                            my1 = Nothing
                        End If
                    End If
                    Lbl_LibIntro.Text = myLibIntro
                Else
                    Lbl_LibIntro.Text = ""
                End If
                LIBRARY_NAME = dtLibrary.Rows(0).Item("LIB_NAME").ToString

                If dtLibrary.Rows(0).Item("LIB_INCHARGE").ToString <> "" Then
                    CONTACT_PERSON = dtLibrary.Rows(0).Item("LIB_INCHARGE").ToString
                Else
                    CONTACT_PERSON = ""
                End If

                If dtLibrary.Rows(0).Item("PARENT_BODY").ToString <> "" Then
                    PARENT_ORG = dtLibrary.Rows(0).Item("PARENT_BODY").ToString
                Else
                    PARENT_ORG = ""
                End If
                If dtLibrary.Rows(0).Item("LIB_ADDRESS").ToString <> "" Then
                    LIBRARY_ADD = dtLibrary.Rows(0).Item("LIB_ADDRESS").ToString
                Else
                    LIBRARY_ADD = ""
                End If

                If dtLibrary.Rows(0).Item("LIB_FAX").ToString <> "" Then
                    LIB_FAX = dtLibrary.Rows(0).Item("LIB_FAX").ToString
                Else
                    LIB_FAX = ""
                End If

                If dtLibrary.Rows(0).Item("LIB_PHONE").ToString <> "" Then
                    LIB_PHONE = dtLibrary.Rows(0).Item("LIB_PHONE").ToString
                Else
                    LIB_PHONE = ""
                End If

                If dtLibrary.Rows(0).Item("LIB_EMAIL").ToString <> "" Then
                    LIB_EMAIL = dtLibrary.Rows(0).Item("LIB_EMAIL").ToString
                Else
                    LIB_EMAIL = ""
                End If

                If dtLibrary.Rows(0).Item("LIB_INCHARGE2").ToString <> "" Then
                    LIB_INCHARGE2 = dtLibrary.Rows(0).Item("LIB_INCHARGE2").ToString
                Else
                    LIB_INCHARGE2 = ""
                End If

                If dtLibrary.Rows(0).Item("LIB_NAME2").ToString <> "" Then
                    LIB_NAME2 = dtLibrary.Rows(0).Item("LIB_NAME2").ToString
                Else
                    LIB_NAME2 = ""
                End If

                If dtLibrary.Rows(0).Item("PARENT_BODY2").ToString <> "" Then
                    PARENT_BODY2 = dtLibrary.Rows(0).Item("PARENT_BODY2").ToString
                Else
                    PARENT_BODY2 = ""
                End If

                If dtLibrary.Rows(0).Item("LIB_ADDRESS2").ToString <> "" Then
                    LIB_ADD2 = dtLibrary.Rows(0).Item("LIB_ADDRESS2").ToString
                Else
                    LIB_ADD2 = ""
                End If
                'Lbl_Contact.Text = Replace("Contact Us: " & vbCrLf & vbCrLf & CONTACT_PERSON & vbCrLf & LIBRARY_NAME & vbCrLf & PARENT_ORG & vbCrLf & LIBRARY_ADD & vbCrLf & "Phone: " & LIB_PHONE & vbCrLf & "Fax: " & LIB_FAX & vbCrLf & "Email: " & LIB_EMAIL & vbCrLf & vbCrLf & LIB_INCHARGE2 & vbCrLf & LIB_NAME2 & vbCrLf & PARENT_BODY2 & vbCrLf & LIB_ADD2 & vbCrLf & "Phone: " & LIB_PHONE & vbCrLf & "Fax: " & LIB_FAX & vbCrLf & "Email: " & LIB_EMAIL & vbCrLf & vbCrLf, Environment.NewLine, "<br />")
            Else

            End If

        Catch s As Exception

        Finally
            SqlConn.Close()
        End Try
    End Sub

End Class