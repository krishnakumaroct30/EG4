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
Imports EG4.PopulateCombo
Public Class IssueReturn
    Inherits System.Web.UI.Page
    Dim index As Integer = 0
    Public Srno As Integer = 0
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session.Item("IsThisU") <> Nothing Then
                If Not SConnection() = True Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' Database Connection is lost..Try Again !');", True)
                Else
                    If Page.IsPostBack = False Then
                        RadioButton1.Checked = True
                        RadioButton2.Checked = False
                        Label8.Text = "Accession No*"
                        PopulateBibCodes()
                        PopulateMaterials()
                        PopulateDocType()
                        PopulateStatus()
                        DDL_MemStatus.Items.Insert(0, "")
                        DDL_MemStatus.ClearSelection()
                        DDL_CollectionType.Items.Insert(0, "")
                        DDL_CollectionType.ClearSelection()

                        'members details
                        PopulateMembersDropDown()
                        PopulateCategories()
                        PopulateSubCategories()
                        DDL_MemStatus.Items.Insert(0, "")

                        'return
                        PopulateBibCodes2()
                        PopulateMaterials2()
                        PopulateDocType2()
                        PopulateStatus2()
                        DDL_Collections.Items.Insert(0, "")
                        DDL_Collections.ClearSelection()
                        PopulateCategories2()
                        PopulateSubCategories2()
                        DDL_MemberStatus.Items.Insert(0, "")
                        DDL_MemberStatus.ClearSelection()
                        DDL_FineSystem.Items.Insert(0, "")
                        DDL_FineSystem.ClearSelection()
                    End If
                End If
                Dim CreateSUserButton As Object 'System.Web.UI.WebControls.Button
                CreateSUserButton = Master.FindControl("Accordion1").FindControl("CirPane").FindControl("Cir_Issue_Bttn")
                CreateSUserButton.ForeColor = Drawing.Color.Red
                myPaneName = "CirPane" 'paneSelectedIndex = 0
            Else
                Response.Redirect("~/Default.aspx", False)
            End If
            LibCode = Session.Item("LoggedLibcode")
        Catch ex As Exception
            Lbl_Error.Text = "Error: " & (ex.Message())
        End Try
    End Sub
    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As MenuEventArgs) Handles Menu1.MenuItemClick
        MultiView1.ActiveViewIndex = Int32.Parse(e.Item.Value)
        If MultiView1.ActiveViewIndex = 0 Then
            Menu1.Items(0).ImageUrl = "~/Images/Issue_ReserveUP.png"
            Menu1.Items(1).ImageUrl = "~/Images/return_renew_over.png"
            txt_Cir_MemNo.Focus()
        End If
        If MultiView1.ActiveViewIndex = 1 Then 'generate approval
            Menu1.Items(0).ImageUrl = "~/Images/Issue_Reserve_over.png"
            Menu1.Items(1).ImageUrl = "~/Images/return_renew_up.png"
            txt_Ret_AccessionNo.focus
        End If
    End Sub
    'chek books and bound jor
    Private Sub RadioButton1_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButton1.CheckedChanged
        Label8.Text = "Accession No*"
        txt_Cir_AccessionNo.Text = ""

        Lbl_Error.Text = "Plz Enter Accession No/Item Id!"
        Label10.Text = ""
        Label11.Text = ""
        Label13.Text = ""
        Label14.Text = ""
        Label15.Text = ""
        txt_Cir_Entitlement.Text = ""
        txt_Cir_DueDays.Text = ""
        txt_Cir_Issued.Text = ""
        Image1.ImageUrl = Nothing
        DDL_Bib_Level.ClearSelection()
        DDL_Mat_Type.ClearSelection()
        DDL_Doc_Type.ClearSelection()
        DDL_CollectionType.ClearSelection()
        DDL_Status.ClearSelection()
        txt_Cir_AccessionNo.Focus()
    End Sub
    'chk loose
    Private Sub RadioButton2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButton2.CheckedChanged
        Label8.Text = "Item ID*"
        txt_Cir_AccessionNo.Text = ""

        Lbl_Error.Text = "Plz Enter Accession No/Item Id!"
        Label10.Text = ""
        Label11.Text = ""
        Label13.Text = ""
        Label14.Text = ""
        Label15.Text = ""
        txt_Cir_Entitlement.Text = ""
        txt_Cir_DueDays.Text = ""
        txt_Cir_Issued.Text = ""
        Image1.ImageUrl = Nothing
        DDL_Bib_Level.ClearSelection()
        DDL_Mat_Type.ClearSelection()
        DDL_Doc_Type.ClearSelection()
        DDL_CollectionType.ClearSelection()
        DDL_Status.ClearSelection()
        txt_Cir_AccessionNo.Focus()
    End Sub
    '*************************************************************
    'MEMBERS DETAILS
    Public Sub PopulateMembersDropDown()
        Dim Command As SqlCommand = Nothing
        Dim dt As DataTable = Nothing
        Try
            Command = New SqlCommand("SELECT MEM_NO, MEM_NAME FROM MEMBERSHIPS WHERE (LIB_CODE = '" & Trim(LibCode) & "') AND (MEM_STATUS='CU') ORDER BY MEM_NAME ", SqlConn)
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
            Dr("MEM_NO") = ""
            Dr("MEM_NAME") = ""
            dt.Rows.InsertAt(Dr, 0)

            If dt.Rows.Count = 0 Then
                Me.DDL_Members.DataSource = Nothing
            Else
                Me.DDL_Members.DataSource = dt
                Me.DDL_Members.DataTextField = "MEM_NAME"
                Me.DDL_Members.DataValueField = "MEM_NO"
                Me.DDL_Members.DataBind()
            End If
            dt.Dispose()
            SqlConn.Close()
        Catch s As Exception
            Lbl_Error.Text = "Error: " & (s.Message())
            Label12.Text = ""
        Finally
            SqlConn.Close()
        End Try
    End Sub
    Public Sub PopulateCategories()
        Dim Command As SqlCommand = Nothing
        Dim dt As DataTable = Nothing
        Try
            Command = New SqlCommand("SELECT CAT_ID, CAT_NAME FROM CATEGORIES WHERE (LIB_CODE = '" & Trim(LibCode) & "') ORDER BY CAT_NAME ", SqlConn)
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
            Dr("CAT_ID") = 0
            Dr("CAT_NAME") = ""
            dt.Rows.InsertAt(Dr, 0)

            If dt.Rows.Count = 0 Then
                Me.DDL_Categories.DataSource = Nothing
            Else
                Me.DDL_Categories.DataSource = dt
                Me.DDL_Categories.DataTextField = "CAT_NAME"
                Me.DDL_Categories.DataValueField = "CAT_ID"
                Me.DDL_Categories.DataBind()
            End If
            dt.Dispose()
            SqlConn.Close()
        Catch s As Exception
            Lbl_Error.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    Public Sub PopulateSubCategories()
        Dim Command As SqlCommand = Nothing
        Dim dt As DataTable = Nothing
        Try
            Command = New SqlCommand("SELECT SUBCAT_ID, SUBCAT_NAME FROM SUB_CATEGORIES WHERE (LIB_CODE = '" & Trim(LibCode) & "') ORDER BY SUBCAT_NAME ", SqlConn)
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
            Dr("SUBCAT_ID") = 0
            Dr("SUBCAT_NAME") = ""
            dt.Rows.InsertAt(Dr, 0)

            If dt.Rows.Count = 0 Then
                Me.DDL_SubCategories.DataSource = Nothing
            Else
                Me.DDL_SubCategories.DataSource = dt
                Me.DDL_SubCategories.DataTextField = "SUBCAT_NAME"
                Me.DDL_SubCategories.DataValueField = "SUBCAT_ID"
                Me.DDL_SubCategories.DataBind()
            End If
            dt.Dispose()
            SqlConn.Close()
        Catch s As Exception
            Lbl_Error.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    Public Sub PopulateCategories2()
        Dim Command As SqlCommand = Nothing
        Dim dt As DataTable = Nothing
        Try
            Command = New SqlCommand("SELECT CAT_ID, CAT_NAME FROM CATEGORIES WHERE (LIB_CODE = '" & Trim(LibCode) & "') ORDER BY CAT_NAME ", SqlConn)
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
            Dr("CAT_ID") = 0
            Dr("CAT_NAME") = ""
            dt.Rows.InsertAt(Dr, 0)

            If dt.Rows.Count = 0 Then
                Me.DDL_MemberCategories.DataSource = Nothing
            Else
                Me.DDL_MemberCategories.DataSource = dt
                Me.DDL_MemberCategories.DataTextField = "CAT_NAME"
                Me.DDL_MemberCategories.DataValueField = "CAT_ID"
                Me.DDL_MemberCategories.DataBind()
            End If
            dt.Dispose()
            SqlConn.Close()
        Catch s As Exception
            Lbl_Error.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    Public Sub PopulateSubCategories2()
        Dim Command As SqlCommand = Nothing
        Dim dt As DataTable = Nothing
        Try
            Command = New SqlCommand("SELECT SUBCAT_ID, SUBCAT_NAME FROM SUB_CATEGORIES WHERE (LIB_CODE = '" & Trim(LibCode) & "') ORDER BY SUBCAT_NAME ", SqlConn)
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
            Dr("SUBCAT_ID") = 0
            Dr("SUBCAT_NAME") = ""
            dt.Rows.InsertAt(Dr, 0)

            If dt.Rows.Count = 0 Then
                Me.DDL_MemberSubCategories.DataSource = Nothing
            Else
                Me.DDL_MemberSubCategories.DataSource = dt
                Me.DDL_MemberSubCategories.DataTextField = "SUBCAT_NAME"
                Me.DDL_MemberSubCategories.DataValueField = "SUBCAT_ID"
                Me.DDL_MemberSubCategories.DataBind()
            End If
            dt.Dispose()
            SqlConn.Close()
        Catch s As Exception
            Lbl_Error.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    Protected Sub txt_Cir_MemNo_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txt_Cir_MemNo.TextChanged
        GetMemberDetails()
        PopulateGridIssuedBooks()
    End Sub
    'get member details
    Public Sub GetMemberDetails()
        Dim dt As New DataTable
        Try
            Dim iloop As Integer
            Dim strcurrentchar As Object
            Dim c As Integer
            Dim counter1 As Integer

            Dim MEM_NO As Object = Nothing
            If Trim(txt_Cir_MemNo.Text) <> "" Then
                MEM_NO = TrimX(txt_Cir_MemNo.Text)
                MEM_NO = RemoveQuotes(MEM_NO)
            Else
                Lbl_Error.Text = "Plz Enter valid Member No!"
                Image2.ImageUrl = Nothing
                DDL_Members.ClearSelection()
                DDL_Categories.ClearSelection()
                DDL_SubCategories.ClearSelection()
                DDL_MemStatus.ClearSelection()
                txt_Cir_Entitlement.Text = ""
                txt_Cir_DueDays.Text = ""
                txt_Cir_Issued.Text = ""
                txt_Cir_OverRide.Text = ""
                txt_Cir_Mobile.Text = ""
                txt_Cir_MemEmail.Text = ""
                txt_Cir_AdmDate.Text = ""
                txt_Cir_ClosingDate.Text = ""
                Label16.Text = ""
                txt_Cir_MemNo.Focus()
                Exit Sub
            End If

            Dim SQL As String = Nothing
            SQL = "SELECT MEMBERSHIPS.MEM_ID, MEMBERSHIPS.MEM_NO, MEMBERSHIPS.MEM_NAME, MEMBERSHIPS.CAT_ID, CATEGORIES.CAT_NAME,  MEMBERSHIPS.SUBCAT_ID, SUB_CATEGORIES.SUBCAT_NAME, SUB_CATEGORIES.FINE_SYSTEM, SUB_CATEGORIES.ENTITLEMENT_LOOSE, SUB_CATEGORIES.ENTITLEMENT_BOOKS, SUB_CATEGORIES.ENTITLEMENT_BOUNDJ, SUB_CATEGORIES.ENTITLEMENT_MANUALS, SUB_CATEGORIES.ENTITLEMENT_PATENTS, SUB_CATEGORIES.ENTITLEMENT_REPORTS, SUB_CATEGORIES.ENTITLEMENT_STANDARDS, SUB_CATEGORIES.ENTITLEMENT_AV, SUB_CATEGORIES.ENTITLEMENT_CARTOGRAPHIC, SUB_CATEGORIES.ENTITLEMENT_MANUSCRIPTS, SUB_CATEGORIES.ENTITLEMENT_BBGENERAL, SUB_CATEGORIES.ENTITLEMENT_BBRESERVE, SUB_CATEGORIES.ENTITLEMENT_NONRETURNABLE, SUB_CATEGORIES.DUEDAYS_LOOSE, SUB_CATEGORIES.DUEDAYS_BOOKS, SUB_CATEGORIES.DUEDAYS_BOUNDJ, SUB_CATEGORIES.DUEDAYS_MANUALS,  SUB_CATEGORIES.DUEDAYS_PATENTS, SUB_CATEGORIES.DUEDAYS_REPORTS, SUB_CATEGORIES.DUEDAYS_STANDARDS,  SUB_CATEGORIES.DUEDAYS_AV, SUB_CATEGORIES.DUEDAYS_CARTOGRAPHIC, SUB_CATEGORIES.DUEDAYS_MANUSCRIPTS, SUB_CATEGORIES.DUEDAYS_BBGENERAL, SUB_CATEGORIES.DUEDAYS_BBRESERVE, SUB_CATEGORIES.DUEDAYS_NONRETURNABLE, MEMBERSHIPS.MEM_EMAIL,  MEMBERSHIPS.MEM_TELEPHONE, MEMBERSHIPS.MEM_MOBILE, MEMBERSHIPS.MEM_OVERRIDE, MEMBERSHIPS.MEM_ADM_DATE, MEMBERSHIPS.MEM_CLOSE_DATE, MEMBERSHIPS.MEM_REMARKS, MEMBERSHIPS.MEM_STATUS, MEMBERSHIPS.TYPE, MEMBERSHIPS.LIB_CODE, MEMBERSHIPS.PHOTO FROM         MEMBERSHIPS LEFT OUTER JOIN SUB_CATEGORIES ON MEMBERSHIPS.SUBCAT_ID = SUB_CATEGORIES.SUBCAT_ID LEFT OUTER JOIN CATEGORIES ON MEMBERSHIPS.CAT_ID = CATEGORIES.CAT_ID WHERE (MEMBERSHIPS.LIB_CODE ='" & Trim(LibCode) & "'  AND MEMBERSHIPS.MEM_NO ='" & Trim(MEM_NO) & "' AND MEMBERSHIPS.MEM_STATUS='CU')"

            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            da.Fill(ds)

            dt = ds.Tables(0).Copy

            If dt.Rows.Count <> 0 Then
                If dt.Rows(0).Item("MEM_ID").ToString <> "" Then
                    Label16.Text = dt.Rows(0).Item("MEM_ID").ToString
                Else
                    Label16.Text = ""
                End If
                If dt.Rows(0).Item("MEM_NO").ToString <> "" Then
                    DDL_Members.SelectedValue = dt.Rows(0).Item("MEM_NO").ToString
                Else
                    DDL_Members.ClearSelection()
                End If

                If dt.Rows(0).Item("CAT_ID").ToString <> "" Then
                    DDL_Categories.SelectedValue = dt.Rows(0).Item("CAT_ID").ToString
                Else
                    DDL_Categories.ClearSelection()
                End If

                If dt.Rows(0).Item("SUBCAT_ID").ToString <> "" Then
                    DDL_SubCategories.SelectedValue = dt.Rows(0).Item("SUBCAT_ID").ToString
                Else
                    DDL_SubCategories.ClearSelection()
                End If

                If dt.Rows(0).Item("MEM_STATUS").ToString <> "" Then
                    DDL_MemStatus.SelectedValue = dt.Rows(0).Item("MEM_STATUS").ToString
                Else
                    DDL_MemStatus.ClearSelection()
                End If

                If dt.Rows(0).Item("MEM_OVERRIDE").ToString <> "" Then
                    txt_Cir_OverRide.Text = dt.Rows(0).Item("MEM_OVERRIDE").ToString
                Else
                    txt_Cir_OverRide.Text = "N"
                End If

                If dt.Rows(0).Item("MEM_MOBILE").ToString <> "" Then
                    txt_Cir_Mobile.Text = dt.Rows(0).Item("MEM_MOBILE").ToString
                Else
                    txt_Cir_Mobile.Text = ""
                End If

                If dt.Rows(0).Item("MEM_EMAIL").ToString <> "" Then
                    txt_Cir_MemEmail.Text = dt.Rows(0).Item("MEM_EMAIL").ToString
                Else
                    txt_Cir_MemEmail.Text = ""
                End If

                If dt.Rows(0).Item("MEM_ADM_DATE").ToString <> "" Then
                    txt_Cir_AdmDate.Text = Format(dt.Rows(0).Item("MEM_ADM_DATE"), "dd/MM/yyyy")
                Else
                    txt_Cir_AdmDate.Text = ""
                End If

                If dt.Rows(0).Item("PHOTO").ToString <> "" Then
                    Dim strURL As String = "~/Circulation/Member_GetPhoto.aspx?MEM_ID=" & Trim(dt.Rows(0).Item("MEM_ID").ToString) & ""
                    Image2.ImageUrl = strURL
                    Image2.Visible = True
                Else
                    Image2.Visible = False
                End If

                If dt.Rows(0).Item("MEM_CLOSE_DATE").ToString <> "" Then
                    txt_Cir_ClosingDate.Text = Format(dt.Rows(0).Item("MEM_CLOSE_DATE"), "dd/MM/yyyy")
                    Dim CLOSING_DATE As Date = Nothing
                    CLOSING_DATE = dt.Rows(0).Item("MEM_CLOSE_DATE")  'Convert.ToDateTime(Format(txt_Cir_ClosingDate.Text))
                    CLOSING_DATE = Convert.ToDateTime(CLOSING_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert to indian 

                    Dim TODAY_DATE As Date = Nothing
                    TODAY_DATE = Now.Date
                    TODAY_DATE = Convert.ToDateTime(TODAY_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert to indian 

                    'compare dates
                    Dim result As Integer = DateTime.Compare(CLOSING_DATE, TODAY_DATE)
                    If result < 0 Then
                        txt_Cir_ClosingDate.ForeColor = Drawing.Color.Red
                        Lbl_Error.Text = "Closing Date is expired!"
                    Else
                        txt_Cir_ClosingDate.ForeColor = Drawing.Color.Blue
                        Lbl_Error.Text = ""
                    End If
                Else
                    txt_Cir_ClosingDate.Text = ""
                End If
                txt_Cir_Entitlement.Text = ""
                txt_Cir_DueDays.Text = ""
                txt_Cir_Issued.Text = ""
                txt_Cir_AccessionNo.Focus()
            Else
                Image2.ImageUrl = Nothing
                DDL_Members.ClearSelection()
                DDL_Categories.ClearSelection()
                DDL_SubCategories.ClearSelection()
                DDL_MemStatus.ClearSelection()
                txt_Cir_Entitlement.Text = ""
                txt_Cir_DueDays.Text = ""
                txt_Cir_Issued.Text = ""
                txt_Cir_OverRide.Text = ""
                txt_Cir_Mobile.Text = ""
                txt_Cir_MemEmail.Text = ""
                txt_Cir_AdmDate.Text = ""
                txt_Cir_ClosingDate.Text = ""
                Label16.Text = ""
                Lbl_Error.Text = "Plz Enter valid Member No!"
                txt_Cir_MemNo.Focus()
            End If
        Catch ex As Exception
            Lbl_Error.Text = "Error: " & (ex.Message())
            Label16.Text = ""
        Finally
            SqlConn.Close()
        End Try
    End Sub
    Public Sub txt_Cir_AccessionNo_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txt_Cir_AccessionNo.TextChanged
        Me.GetDocDetails()
    End Sub
    Public Sub GetDocDetails()
        Dim dt As New DataTable
        Try
            Dim iloop As Integer
            Dim strcurrentchar As Object
            Dim c As Integer
            Dim counter1 As Integer

            Dim ACCESSION_NO As Object = Nothing
            If Trim(txt_Cir_AccessionNo.Text) <> "" Then
                ACCESSION_NO = TrimX(txt_Cir_AccessionNo.Text)
                ACCESSION_NO = RemoveQuotes(ACCESSION_NO)
            Else
                Label10.Text = ""
                Label11.Text = ""
                Label13.Text = ""
                Label14.Text = ""
                Label15.Text = ""
                txt_Cir_Issued.Text = ""
                txt_Cir_Entitlement.Text = ""
                txt_Cir_DueDays.Text = ""
                Image1.ImageUrl = Nothing
                DDL_Bib_Level.ClearSelection()
                DDL_Mat_Type.ClearSelection()
                DDL_Doc_Type.ClearSelection()
                DDL_CollectionType.Items.Insert(0, "")
                DDL_CollectionType.ClearSelection()
                DDL_Status.ClearSelection()
                txt_Cir_AccessionNo.Focus()
                Exit Sub
            End If

            If RadioButton1.Checked = True Then ' for books
                Dim SQL As String = Nothing
                SQL = "SELECT HOLDINGS_CATS_AUTHORS_VIEW.*, CATS.PHOTO  FROM HOLDINGS_CATS_AUTHORS_VIEW INNER JOIN CATS ON HOLDINGS_CATS_AUTHORS_VIEW.CAT_NO = CATS.CAT_NO WHERE (HOLDINGS_CATS_AUTHORS_VIEW.LIB_CODE ='" & Trim(LibCode) & "'  AND HOLDINGS_CATS_AUTHORS_VIEW.ACCESSION_NO ='" & Trim(ACCESSION_NO) & "')"

                Dim ds As New DataSet
                Dim da As New SqlDataAdapter(SQL, SqlConn)
                If SqlConn.State = 0 Then
                    SqlConn.Open()
                End If
                da.Fill(ds)

                dt = ds.Tables(0).Copy

                Dim myDetails As String = Nothing
                If dt.Rows.Count <> 0 Then
                    If dt.Rows(0).Item("BIB_CODE").ToString <> "" Then
                        DDL_Bib_Level.SelectedValue = dt.Rows(0).Item("BIB_CODE").ToString
                    Else
                        DDL_Bib_Level.ClearSelection()
                    End If

                    If dt.Rows(0).Item("HOLD_ID").ToString <> "" Then
                        Label15.Text = dt.Rows(0).Item("HOLD_ID").ToString
                    Else
                        Label15.Text = ""
                    End If

                    If dt.Rows(0).Item("MAT_CODE").ToString <> "" Then
                        DDL_Mat_Type.SelectedValue = dt.Rows(0).Item("MAT_CODE").ToString
                    Else
                        DDL_Mat_Type.ClearSelection()
                    End If

                    If dt.Rows(0).Item("DOC_TYPE_CODE").ToString <> "" Then
                        DDL_Doc_Type.SelectedValue = dt.Rows(0).Item("DOC_TYPE_CODE").ToString
                    Else
                        DDL_Doc_Type.ClearSelection()
                    End If

                    If dt.Rows(0).Item("SUB_TITLE").ToString <> "" Then
                        myDetails = dt.Rows(0).Item("TITLE").ToString & ": " & dt.Rows(0).Item("SUB_TITLE").ToString
                    Else
                        myDetails = dt.Rows(0).Item("TITLE").ToString
                    End If

                    If dt.Rows(0).Item("AUTHOR1").ToString <> "" And dt.Rows(0).Item("AUTHOR2").ToString <> "" And dt.Rows(0).Item("AUTHOR3").ToString <> "" Then
                        myDetails = myDetails & " /By " & dt.Rows(0).Item("AUTHOR1").ToString & ";  " & dt.Rows(0).Item("AUTHOR2").ToString & " and " & dt.Rows(0).Item("AUTHOR3").ToString
                    End If
                    If dt.Rows(0).Item("AUTHOR1").ToString <> "" And dt.Rows(0).Item("AUTHOR2").ToString <> "" And dt.Rows(0).Item("AUTHOR3").ToString = "" Then
                        myDetails = myDetails & " /By " & dt.Rows(0).Item("AUTHOR1").ToString & " and " & dt.Rows(0).Item("AUTHOR2").ToString
                    End If
                    If dt.Rows(0).Item("AUTHOR1").ToString <> "" And dt.Rows(0).Item("AUTHOR2").ToString = "" And dt.Rows(0).Item("AUTHOR3").ToString = "" Then
                        myDetails = myDetails & " /By " & dt.Rows(0).Item("AUTHOR1").ToString
                    End If

                    If dt.Rows(0).Item("EDITOR").ToString <> "" Then
                        myDetails = myDetails & " /Edited By " & dt.Rows(0).Item("EDITOR").ToString
                    End If
                    If dt.Rows(0).Item("EDITION").ToString <> "" Then
                        Dim myEdition As Object = Nothing
                        myEdition = dt.Rows(0).Item("EDITION").ToString

                        If InStr(myEdition, "edition") = 0 Or InStr(myEdition, "ed") = 0 Or InStr(myEdition, "ed.") = 0 Or InStr(myEdition, "edition.") = 0 Then
                            myDetails = myDetails & ". " & dt.Rows(0).Item("EDITION").ToString & " ed. "
                        Else
                            myDetails = myDetails & " . " & dt.Rows(0).Item("EDITION").ToString
                        End If
                    End If
                    If dt.Rows(0).Item("PUB_PLACE").ToString <> "" Then
                        myDetails = myDetails & ", " & dt.Rows(0).Item("PUB_PLACE").ToString
                    End If
                    If dt.Rows(0).Item("PUB_NAME").ToString <> "" Then
                        myDetails = myDetails & ", " & dt.Rows(0).Item("PUB_NAME").ToString
                    End If
                    If dt.Rows(0).Item("YEAR_OF_PUB").ToString <> "" Then
                        myDetails = myDetails & ", " & dt.Rows(0).Item("YEAR_OF_PUB").ToString
                    End If
                    If dt.Rows(0).Item("VOL_NO").ToString <> "" Then
                        myDetails = myDetails & ". Vol No.: " & dt.Rows(0).Item("VOL_NO").ToString
                    End If
                    If dt.Rows(0).Item("PAGINATION").ToString <> "" Then
                        myDetails = myDetails & ", " & dt.Rows(0).Item("PAGINATION").ToString
                    End If

                    Label10.Text = myDetails
                    If dt.Rows(0).Item("STANDARD_NO").ToString <> "" Then
                        Label11.Text = dt.Rows(0).Item("STANDARD_NO").ToString
                    Else
                        Label11.Text = ""
                    End If

                    If dt.Rows(0).Item("LANG_CODE").ToString <> "" Then
                        Label13.Text = "Language: " & dt.Rows(0).Item("LANG_CODE").ToString
                    Else
                        Label13.Text = ""
                    End If

                    If dt.Rows(0).Item("SP_NO").ToString <> "" Then
                        Label14.Text = "Standard No: " & dt.Rows(0).Item("SP_NO").ToString
                    Else
                        Label14.Text = ""
                    End If

                    If dt.Rows(0).Item("REPORT_NO").ToString <> "" Then
                        Label14.Text = "; Report No: " & dt.Rows(0).Item("REPORT_NO").ToString
                    ElseIf dt.Rows(0).Item("MANUAL_NO").ToString <> "" Then
                        Label14.Text = "; Manual No: " & dt.Rows(0).Item("MANUAL_NO").ToString
                    ElseIf dt.Rows(0).Item("PATENT_NO").ToString <> "" Then
                        Label14.Text = "; Patent No: " & dt.Rows(0).Item("PATENT_NO").ToString
                    Else
                        Label14.Text = ""
                    End If

                    Image1.ImageUrl = Nothing
                    Dim strURL As String = "~/Acquisition/Cats_GetPhoto.aspx?CAT_NO=" & dt.Rows(0).Item("CAT_NO").ToString
                    Image1.ImageUrl = strURL
                    Image1.Visible = True

                    If dt.Rows(0).Item("COLLECTION_TYPE").ToString <> "" Then
                        DDL_CollectionType.SelectedValue = dt.Rows(0).Item("COLLECTION_TYPE").ToString
                        If dt.Rows(0).Item("COLLECTION_TYPE").ToString = "R" Then
                            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' This Book can not be Issue as it belongs to REFERENCE Collection!');", True)
                        Else
                            Lbl_Error.Text = ""
                        End If
                    Else
                        DDL_CollectionType.ClearSelection()
                    End If

                    If dt.Rows(0).Item("STA_CODE").ToString <> "" Then
                        DDL_Status.SelectedValue = dt.Rows(0).Item("STA_CODE").ToString
                        If dt.Rows(0).Item("STA_CODE").ToString <> "1" Then
                            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert(' This Book can not be Issue as current Status is NOT Available!');", True)
                        End If
                    Else
                        DDL_Status.ClearSelection()
                    End If
                Else
                    Label10.Text = ""
                    Label11.Text = ""
                    Label13.Text = ""
                    Label14.Text = ""
                    Label15.Text = ""
                    txt_Cir_Issued.Text = ""
                    txt_Cir_Entitlement.Text = ""
                    txt_Cir_DueDays.Text = ""
                    Image1.ImageUrl = Nothing
                    DDL_Bib_Level.ClearSelection()
                    DDL_Mat_Type.ClearSelection()
                    DDL_Doc_Type.ClearSelection()
                    DDL_CollectionType.ClearSelection()
                    DDL_Status.ClearSelection()
                    txt_Cir_AccessionNo.Focus()
                End If
            End If
            If RadioButton2.Checked = True Then ' for losse issues
                Dim SQL As String = Nothing
                SQL = "SELECT * FROM CATS_LOOSE_ISSUES_COPIES_VIEW WHERE (LOOSE_ISSUE_LIB_CODE ='" & Trim(LibCode) & "'  AND COPY_ID ='" & Trim(ACCESSION_NO) & "')"

                Dim ds As New DataSet
                Dim da As New SqlDataAdapter(SQL, SqlConn)
                If SqlConn.State = 0 Then
                    SqlConn.Open()
                End If
                da.Fill(ds)

                dt = ds.Tables(0).Copy

                Dim myDetails As String = Nothing
                If dt.Rows.Count <> 0 Then
                    If dt.Rows(0).Item("BIB_CODE").ToString <> "" Then
                        DDL_Bib_Level.SelectedValue = dt.Rows(0).Item("BIB_CODE").ToString
                    Else
                        DDL_Bib_Level.ClearSelection()
                    End If

                    If dt.Rows(0).Item("COPY_ID").ToString <> "" Then
                        Label15.Text = dt.Rows(0).Item("COPY_ID").ToString
                    Else
                        Label15.Text = ""
                    End If

                    If dt.Rows(0).Item("MAT_CODE").ToString <> "" Then
                        DDL_Mat_Type.SelectedValue = dt.Rows(0).Item("MAT_CODE").ToString
                    Else
                        DDL_Mat_Type.ClearSelection()
                    End If

                    If dt.Rows(0).Item("DOC_TYPE_CODE").ToString <> "" Then
                        DDL_Doc_Type.SelectedValue = dt.Rows(0).Item("DOC_TYPE_CODE").ToString
                    Else
                        DDL_Doc_Type.ClearSelection()
                    End If

                    If dt.Rows(0).Item("SUB_TITLE").ToString <> "" Then
                        myDetails = dt.Rows(0).Item("TITLE").ToString & ": " & dt.Rows(0).Item("SUB_TITLE").ToString
                    Else
                        myDetails = dt.Rows(0).Item("TITLE").ToString
                    End If

                    If dt.Rows(0).Item("EDITOR").ToString <> "" Then
                        myDetails = myDetails & " /Edited By " & dt.Rows(0).Item("EDITOR").ToString
                    End If
                   
                    If dt.Rows(0).Item("PUB_PLACE").ToString <> "" Then
                        myDetails = myDetails & ", " & dt.Rows(0).Item("PUB_PLACE").ToString
                    End If
                    If dt.Rows(0).Item("PUB_NAME").ToString <> "" Then
                        myDetails = myDetails & ", " & dt.Rows(0).Item("PUB_NAME").ToString
                    End If
                   
                    If dt.Rows(0).Item("VOL_NO").ToString <> "" Then
                        myDetails = myDetails & ". Vol No.: " & dt.Rows(0).Item("VOL_NO").ToString
                    End If
                    If dt.Rows(0).Item("ISSUE_NO").ToString <> "" Then
                        myDetails = myDetails & " (" & dt.Rows(0).Item("ISSUE_NO").ToString & ")"
                    End If
                    If dt.Rows(0).Item("PART_NO").ToString <> "" Then
                        myDetails = myDetails & ", Part NO:" & dt.Rows(0).Item("PART_NO").ToString
                    End If

                    If dt.Rows(0).Item("ISS_DATE").ToString <> "" Then
                        myDetails = myDetails & ", Issue Date:" & Format(dt.Rows(0).Item("ISS_DATE"), "dd/MM/yyyy")
                    End If

                    Label10.Text = myDetails
                    If dt.Rows(0).Item("STANDARD_NO").ToString <> "" Then
                        Label11.Text = dt.Rows(0).Item("STANDARD_NO").ToString
                    Else
                        Label11.Text = ""
                    End If

                    If dt.Rows(0).Item("LANG_CODE").ToString <> "" Then
                        Label13.Text = "Language: " & dt.Rows(0).Item("LANG_CODE").ToString
                    Else
                        Label13.Text = ""
                    End If

                    Label14.Text = ""
                    Label14.Text = ""

                    Image1.ImageUrl = Nothing
                    Dim strURL As String = "~/Acquisition/Cats_GetPhoto.aspx?CAT_NO=" & Label15.Text & ""
                    Image1.ImageUrl = strURL
                    Image1.Visible = True

                    If dt.Rows(0).Item("COLLECTION_TYPE").ToString <> "" Then
                        DDL_CollectionType.SelectedValue = dt.Rows(0).Item("COLLECTION_TYPE").ToString
                        If dt.Rows(0).Item("COLLECTION_TYPE").ToString = "R" Then

                        Else
                            Lbl_Error.Text = ""
                        End If
                    Else
                        DDL_CollectionType.ClearSelection()
                    End If

                    If dt.Rows(0).Item("STA_CODE").ToString <> "" Then
                        DDL_Status.SelectedValue = dt.Rows(0).Item("STA_CODE").ToString
                        If dt.Rows(0).Item("STA_CODE").ToString <> "1" Then
                           
                        Else
                            Lbl_Error.Text = ""
                        End If
                    Else
                        DDL_Status.ClearSelection()
                    End If
                    txt_Cir_MemNo.Focus()
                Else
                    Label10.Text = ""
                    Label11.Text = ""
                    Label13.Text = ""
                    Label14.Text = ""
                    Label15.Text = ""
                    txt_Cir_Issued.Text = ""
                    txt_Cir_Entitlement.Text = ""
                    txt_Cir_DueDays.Text = ""
                    Image1.ImageUrl = Nothing
                    DDL_Bib_Level.ClearSelection()
                    DDL_Mat_Type.ClearSelection()
                    DDL_Doc_Type.ClearSelection()
                    DDL_CollectionType.ClearSelection()
                    DDL_Status.ClearSelection()
                    txt_Cir_AccessionNo.Focus()
                End If
            End If
            SqlConn.Close()

            'get Entitlement
            GetMemberEntitlementNDues()
            AlreadyIssuedBooks() ' get already issued books
            FillDateTime()

        Catch ex As Exception
            Lbl_Error.Text = "Error: " & (ex.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    Public Sub GetMemberEntitlementNDues()
        Try
            Dim dt As DataTable = Nothing
            Dim MEM_NO As Object = Nothing
            If Trim(txt_Cir_MemNo.Text) <> "" Then
                MEM_NO = TrimX(txt_Cir_MemNo.Text)
                MEM_NO = RemoveQuotes(MEM_NO)
            Else
                Lbl_Error.Text = "Plz Enter valid Member No!"
                Image2.ImageUrl = Nothing
                DDL_Members.ClearSelection()
                DDL_Categories.ClearSelection()
                DDL_SubCategories.ClearSelection()
                DDL_MemStatus.ClearSelection()
                txt_Cir_Entitlement.Text = ""
                txt_Cir_DueDays.Text = ""
                txt_Cir_Issued.Text = ""
                txt_Cir_OverRide.Text = ""
                txt_Cir_Mobile.Text = ""
                txt_Cir_MemEmail.Text = ""
                txt_Cir_AdmDate.Text = ""
                txt_Cir_ClosingDate.Text = ""
                Label16.Text = ""
                txt_Cir_MemNo.Focus()
                Exit Sub
            End If

            Dim SQL As String = Nothing
            SQL = "SELECT MEMBERSHIPS.MEM_ID, MEMBERSHIPS.MEM_NO, SUB_CATEGORIES.FINE_SYSTEM, SUB_CATEGORIES.ENTITLEMENT_LOOSE, SUB_CATEGORIES.ENTITLEMENT_BOOKS, SUB_CATEGORIES.ENTITLEMENT_BOUNDJ, SUB_CATEGORIES.ENTITLEMENT_MANUALS, SUB_CATEGORIES.ENTITLEMENT_PATENTS, SUB_CATEGORIES.ENTITLEMENT_REPORTS, SUB_CATEGORIES.ENTITLEMENT_STANDARDS, SUB_CATEGORIES.ENTITLEMENT_AV, SUB_CATEGORIES.ENTITLEMENT_CARTOGRAPHIC, SUB_CATEGORIES.ENTITLEMENT_MANUSCRIPTS, SUB_CATEGORIES.ENTITLEMENT_BBGENERAL, SUB_CATEGORIES.ENTITLEMENT_BBRESERVE, SUB_CATEGORIES.ENTITLEMENT_NONRETURNABLE, SUB_CATEGORIES.DUEDAYS_LOOSE, SUB_CATEGORIES.DUEDAYS_BOOKS, SUB_CATEGORIES.DUEDAYS_BOUNDJ, SUB_CATEGORIES.DUEDAYS_MANUALS,  SUB_CATEGORIES.DUEDAYS_PATENTS, SUB_CATEGORIES.DUEDAYS_REPORTS, SUB_CATEGORIES.DUEDAYS_STANDARDS,  SUB_CATEGORIES.DUEDAYS_AV, SUB_CATEGORIES.DUEDAYS_CARTOGRAPHIC, SUB_CATEGORIES.DUEDAYS_MANUSCRIPTS, SUB_CATEGORIES.DUEDAYS_BBGENERAL, SUB_CATEGORIES.DUEDAYS_BBRESERVE, SUB_CATEGORIES.DUEDAYS_NONRETURNABLE, MEMBERSHIPS.MEM_EMAIL,  MEMBERSHIPS.MEM_TELEPHONE, MEMBERSHIPS.MEM_MOBILE, MEMBERSHIPS.MEM_OVERRIDE, MEMBERSHIPS.MEM_ADM_DATE, MEMBERSHIPS.MEM_CLOSE_DATE, MEMBERSHIPS.MEM_REMARKS, MEMBERSHIPS.MEM_STATUS, MEMBERSHIPS.TYPE, MEMBERSHIPS.LIB_CODE FROM         MEMBERSHIPS LEFT OUTER JOIN SUB_CATEGORIES ON MEMBERSHIPS.SUBCAT_ID = SUB_CATEGORIES.SUBCAT_ID LEFT OUTER JOIN CATEGORIES ON MEMBERSHIPS.CAT_ID = CATEGORIES.CAT_ID WHERE (MEMBERSHIPS.LIB_CODE ='" & Trim(LibCode) & "'  AND MEMBERSHIPS.MEM_NO ='" & Trim(MEM_NO) & "' AND MEMBERSHIPS.MEM_STATUS='CU')"

            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            da.Fill(ds)

            dt = ds.Tables(0).Copy
            If dt.Rows.Count <> 0 Then

                'Entitlement/due days
                If DDL_Mat_Type.SelectedItem.ToString <> "" Then
                    If DDL_CollectionType.SelectedValue = "C" Then
                        'entitlement
                        If DDL_Mat_Type.SelectedItem.ToString = "Books" Then
                            If dt.Rows(0).Item("ENTITLEMENT_BOOKS").ToString <> "" Then
                                txt_Cir_Entitlement.Text = dt.Rows(0).Item("ENTITLEMENT_BOOKS").ToString
                            Else
                                txt_Cir_Entitlement.Text = ""
                            End If
                        End If
                        If DDL_Mat_Type.SelectedItem.ToString = "Manuals" Then
                            If dt.Rows(0).Item("ENTITLEMENT_MANUALS").ToString <> "" Then
                                txt_Cir_Entitlement.Text = dt.Rows(0).Item("ENTITLEMENT_MANUALS").ToString
                            Else
                                txt_Cir_Entitlement.Text = ""
                            End If
                        End If
                        If DDL_Mat_Type.SelectedItem.ToString = "Patents" Then
                            If dt.Rows(0).Item("ENTITLEMENT_PATENTS").ToString <> "" Then
                                txt_Cir_Entitlement.Text = dt.Rows(0).Item("ENTITLEMENT_PATENTS").ToString
                            Else
                                txt_Cir_Entitlement.Text = ""
                            End If
                        End If
                        If DDL_Mat_Type.SelectedItem.ToString = "Reports" Then
                            If dt.Rows(0).Item("ENTITLEMENT_REPORTS").ToString <> "" Then
                                txt_Cir_Entitlement.Text = dt.Rows(0).Item("ENTITLEMENT_REPORTS").ToString
                            Else
                                txt_Cir_Entitlement.Text = ""
                            End If
                        End If
                        If DDL_Mat_Type.SelectedItem.ToString = "Standards and Specifications" Then
                            If dt.Rows(0).Item("ENTITLEMENT_STANDARDS").ToString <> "" Then
                                txt_Cir_Entitlement.Text = dt.Rows(0).Item("ENTITLEMENT_STANDARDS").ToString
                            Else
                                txt_Cir_Entitlement.Text = ""
                            End If
                        End If

                        If RadioButton1.Checked = True Then
                            If DDL_Mat_Type.SelectedItem.ToString = "Annuals" Or DDL_Mat_Type.SelectedItem.ToString = "Newspapers" Or DDL_Mat_Type.SelectedItem.ToString = "Periodicals" Then
                                If dt.Rows(0).Item("ENTITLEMENT_BOUNDJ").ToString <> "" Then
                                    txt_Cir_Entitlement.Text = dt.Rows(0).Item("ENTITLEMENT_BOUNDJ").ToString
                                Else
                                    txt_Cir_Entitlement.Text = ""
                                End If
                            End If
                        Else ' for loose issue
                            If dt.Rows(0).Item("ENTITLEMENT_LOOSE").ToString <> "" Then
                                txt_Cir_Entitlement.Text = dt.Rows(0).Item("ENTITLEMENT_LOOSE").ToString
                            Else
                                txt_Cir_Entitlement.Text = ""
                            End If
                        End If

                        If DDL_Mat_Type.SelectedItem.ToString = "Cartographic" Then
                            If dt.Rows(0).Item("ENTITLEMENT_CARTOGRAPHIC").ToString <> "" Then
                                txt_Cir_Entitlement.Text = dt.Rows(0).Item("ENTITLEMENT_CARTOGRAPHIC").ToString
                            Else
                                txt_Cir_Entitlement.Text = ""
                            End If
                        End If
                        If DDL_Mat_Type.SelectedItem.ToString = "AV Materials" Then
                            If dt.Rows(0).Item("ENTITLEMENT_AV").ToString <> "" Then
                                txt_Cir_Entitlement.Text = dt.Rows(0).Item("ENTITLEMENT_AV").ToString
                            Else
                                txt_Cir_Entitlement.Text = ""
                            End If
                        End If
                        If DDL_Mat_Type.SelectedItem.ToString = "Manuscripts" Then
                            If dt.Rows(0).Item("ENTITLEMENT_MANUSCRIPTS").ToString <> "" Then
                                txt_Cir_Entitlement.Text = dt.Rows(0).Item("ENTITLEMENT_MANUSCRIPTS").ToString
                            Else
                                txt_Cir_Entitlement.Text = ""
                            End If
                        End If

                        'due days
                        If DDL_Mat_Type.SelectedItem.ToString = "Books" Then
                            If dt.Rows(0).Item("DUEDAYS_BOOKS").ToString <> "" Then
                                txt_Cir_DueDays.Text = dt.Rows(0).Item("DUEDAYS_BOOKS").ToString
                            Else
                                txt_Cir_DueDays.Text = ""
                            End If
                        End If
                        If DDL_Mat_Type.SelectedItem.ToString = "Manuals" Then
                            If dt.Rows(0).Item("DUEDAYS_MANUALS").ToString <> "" Then
                                txt_Cir_DueDays.Text = dt.Rows(0).Item("DUEDAYS_MANUALS").ToString
                            Else
                                txt_Cir_DueDays.Text = ""
                            End If
                        End If
                        If DDL_Mat_Type.SelectedItem.ToString = "Patents" Then
                            If dt.Rows(0).Item("DUEDAYS_PATENTS").ToString <> "" Then
                                txt_Cir_DueDays.Text = dt.Rows(0).Item("DUEDAYS_PATENTS").ToString
                            Else
                                txt_Cir_DueDays.Text = ""
                            End If
                        End If
                        If DDL_Mat_Type.SelectedItem.ToString = "Reports" Then
                            If dt.Rows(0).Item("DUEDAYS_REPORTS").ToString <> "" Then
                                txt_Cir_DueDays.Text = dt.Rows(0).Item("DUEDAYS_REPORTS").ToString
                            Else
                                txt_Cir_DueDays.Text = ""
                            End If
                        End If
                        If DDL_Mat_Type.SelectedItem.ToString = "Standards and Specifications" Then
                            If dt.Rows(0).Item("DUEDAYS_STANDARDS").ToString <> "" Then
                                txt_Cir_DueDays.Text = dt.Rows(0).Item("DUEDAYS_STANDARDS").ToString
                            Else
                                txt_Cir_DueDays.Text = ""
                            End If
                        End If

                        If RadioButton1.Checked = True Then 'for books/bound journal
                            If DDL_Mat_Type.SelectedItem.ToString = "Annuals" Or DDL_Mat_Type.SelectedItem.ToString = "Newspapers" Or DDL_Mat_Type.SelectedItem.ToString = "Periodicals" Then
                                If dt.Rows(0).Item("DUEDAYS_BOUNDJ").ToString <> "" Then
                                    txt_Cir_DueDays.Text = dt.Rows(0).Item("DUEDAYS_BOUNDJ").ToString
                                Else
                                    txt_Cir_DueDays.Text = ""
                                End If
                            End If
                        Else
                            If dt.Rows(0).Item("DUEDAYS_LOOSE").ToString <> "" Then
                                txt_Cir_DueDays.Text = dt.Rows(0).Item("DUEDAYS_LOOSE").ToString
                            Else
                                txt_Cir_DueDays.Text = ""
                            End If
                        End If

                        If DDL_Mat_Type.SelectedItem.ToString = "Cartographic" Then
                            If dt.Rows(0).Item("DUEDAYS_CARTOGRAPHIC").ToString <> "" Then
                                txt_Cir_DueDays.Text = dt.Rows(0).Item("DUEDAYS_CARTOGRAPHIC").ToString
                            Else
                                txt_Cir_DueDays.Text = ""
                            End If
                        End If
                        If DDL_Mat_Type.SelectedItem.ToString = "AV Materials" Then
                            If dt.Rows(0).Item("DUEDAYS_AV").ToString <> "" Then
                                txt_Cir_DueDays.Text = dt.Rows(0).Item("DUEDAYS_AV").ToString
                            Else
                                txt_Cir_DueDays.Text = ""
                            End If
                        End If
                        If DDL_Mat_Type.SelectedItem.ToString = "Manuscripts" Then
                            If dt.Rows(0).Item("DUEDAYS_MANUSCRIPTS").ToString <> "" Then
                                txt_Cir_DueDays.Text = dt.Rows(0).Item("DUEDAYS_MANUSCRIPTS").ToString
                            Else
                                txt_Cir_DueDays.Text = ""
                            End If
                        End If
                    Else ' if collection type is book-bank/non-returnable
                        If DDL_CollectionType.SelectedValue = "G" Then 'book bank general
                            If dt.Rows(0).Item("ENTITLEMENT_BBGENERAL").ToString <> "" Then
                                txt_Cir_Entitlement.Text = dt.Rows(0).Item("ENTITLEMENT_BBGENERAL").ToString
                            Else
                                txt_Cir_Entitlement.Text = ""
                            End If
                        End If
                        If DDL_CollectionType.SelectedValue = "S" Then 'book bank general
                            If dt.Rows(0).Item("ENTITLEMENT_BBRESERVE").ToString <> "" Then
                                txt_Cir_Entitlement.Text = dt.Rows(0).Item("ENTITLEMENT_BBRESERVE").ToString
                            Else
                                txt_Cir_Entitlement.Text = ""
                            End If
                        End If
                        If DDL_CollectionType.SelectedValue = "N" Then 'book non-refudnable
                            If dt.Rows(0).Item("ENTITLEMENT_NONRETURNABLE").ToString <> "" Then
                                txt_Cir_Entitlement.Text = dt.Rows(0).Item("ENTITLEMENT_NONRETURNABLE").ToString
                            Else
                                txt_Cir_Entitlement.Text = ""
                            End If
                        End If

                        If DDL_CollectionType.SelectedValue = "R" Then 'reference
                            If dt.Rows(0).Item("ENTITLEMENT_BBGENERAL").ToString <> "" Then
                                txt_Cir_Entitlement.Text = ""
                            Else
                                txt_Cir_Entitlement.Text = ""
                            End If
                        End If

                        'duedays
                        If DDL_CollectionType.SelectedValue = "G" Then 'book bank general
                            If dt.Rows(0).Item("DUEDAYS_BBGENERAL").ToString <> "" Then
                                txt_Cir_DueDays.Text = dt.Rows(0).Item("DUEDAYS_BBGENERAL").ToString
                            Else
                                txt_Cir_DueDays.Text = ""
                            End If
                        End If
                        If DDL_CollectionType.SelectedValue = "S" Then 'book bank general
                            If dt.Rows(0).Item("DUEDAYS_BBRESERVE").ToString <> "" Then
                                txt_Cir_DueDays.Text = dt.Rows(0).Item("DUEDAYS_BBRESERVE").ToString
                            Else
                                txt_Cir_DueDays.Text = ""
                            End If
                        End If
                        If DDL_CollectionType.SelectedValue = "N" Then 'book bank general
                            If dt.Rows(0).Item("DUEDAYS_NONRETURNABLE").ToString <> "" Then
                                txt_Cir_DueDays.Text = dt.Rows(0).Item("DUEDAYS_NONRETURNABLE").ToString
                            Else
                                txt_Cir_DueDays.Text = ""
                            End If
                        End If
                        If DDL_CollectionType.SelectedValue = "R" Then 'reference
                            If dt.Rows(0).Item("DUEDAYS_NONRETURNABLE").ToString <> "" Then
                                txt_Cir_DueDays.Text = ""
                            Else
                                txt_Cir_DueDays.Text = ""
                            End If
                        End If
                    End If
                    Else
                        txt_Cir_Entitlement.Text = ""
                        txt_Cir_DueDays.Text = ""
                    End If
            End If
        Catch ex As Exception
            Lbl_Error.Text = "Error: " & (ex.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    Public Sub AlreadyIssuedBooks()
        Try
            Dim dt As DataTable = Nothing
            Dim SQL As String = Nothing

            If DDL_CollectionType.Text <> "" Then
                If Label16.Text <> "" Then

                    If RadioButton1.Checked = True Then
                        If DDL_CollectionType.SelectedValue = "G" Or DDL_CollectionType.SelectedValue = "S" Or DDL_CollectionType.SelectedValue = "N" Then
                            SQL = "SELECT COUNT(CIRCULATION.CIR_ID) AS Numbers, HOLDINGS.COLLECTION_TYPE FROM CATS INNER JOIN HOLDINGS ON CATS.CAT_NO = HOLDINGS.CAT_NO RIGHT OUTER JOIN CIRCULATION ON HOLDINGS.HOLD_ID = CIRCULATION.HOLD_ID  WHERE (CIRCULATION.LIB_CODE ='" & Trim(LibCode) & "'  AND CIRCULATION.MEM_ID ='" & Trim(Label16.Text) & "' AND CIRCULATION.STATUS='Issued' AND HOLDINGS.COLLECTION_TYPE ='" & Trim(DDL_CollectionType.SelectedValue) & "') GROUP BY HOLDINGS.COLLECTION_TYPE"
                        ElseIf DDL_CollectionType.SelectedValue = "C" Then
                            SQL = "SELECT COUNT(CIRCULATION.CIR_ID) AS Numbers, CATS.MAT_CODE FROM CATS INNER JOIN HOLDINGS ON CATS.CAT_NO = HOLDINGS.CAT_NO RIGHT OUTER JOIN CIRCULATION ON HOLDINGS.HOLD_ID = CIRCULATION.HOLD_ID  WHERE (CIRCULATION.LIB_CODE ='" & Trim(LibCode) & "'  AND CIRCULATION.MEM_ID ='" & Trim(Label16.Text) & "' AND CIRCULATION.STATUS='Issued' AND CATS.MAT_CODE ='" & Trim(DDL_Mat_Type.SelectedValue) & "') GROUP BY CATS.MAT_CODE"
                        Else
                            Lbl_Error.Text = "This copy belongs to Reference Section, can not be issued/Reserved!"
                            Exit Sub
                        End If
                    Else
                        SQL = "SELECT COUNT (CIRCULATION.CIR_ID) AS Numbers FROM CIRCULATION WHERE (COPY_ID IS NOT NULL) AND (LIB_CODE ='" & Trim(LibCode) & "') AND (MEM_ID ='" & Trim(Label16.Text) & "') AND (STATUS='Issued')"
                    End If

                    Dim ds As New DataSet
                    Dim da As New SqlDataAdapter(SQL, SqlConn)
                    If SqlConn.State = 0 Then
                        SqlConn.Open()
                    End If
                    da.Fill(ds)

                    dt = ds.Tables(0).Copy
                    If dt.Rows.Count <> 0 Then
                        txt_Cir_Issued.Text = dt.Rows(0).Item("NUMBERS").ToString
                    Else
                        txt_Cir_Issued.Text = ""
                    End If
                End If
                End If
        Catch ex As Exception
            Lbl_Error.Text = "Error: " & (ex.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    Public Sub PopulateBibCodes()
        Me.DDL_Bib_Level.DataTextField = "BIB_NAME"
        Me.DDL_Bib_Level.DataValueField = "BIB_CODE"
        Me.DDL_Bib_Level.DataSource = GetBibLevelist()
        Me.DDL_Bib_Level.DataBind()
        DDL_Bib_Level.Items.Insert(0, "")
    End Sub
Public Sub PopulateBibCodes2()
        Me.DDL_BibLevel.DataTextField = "BIB_NAME"
        Me.DDL_BibLevel.DataValueField = "BIB_CODE"
        Me.DDL_BibLevel.DataSource = GetBibLevelist()
        Me.DDL_BibLevel.DataBind()
        DDL_BibLevel.Items.Insert(0, "")
    End Sub
    Public Sub PopulateMaterials()
        Me.DDL_Mat_Type.DataTextField = "MAT_NAME"
        Me.DDL_Mat_Type.DataValueField = "MAT_CODE"
        Me.DDL_Mat_Type.DataSource = GetMaterialsList()
        Me.DDL_Mat_Type.DataBind()
        DDL_Mat_Type.Items.Insert(0, "")
    End Sub
    Public Sub PopulateMaterials2()
        Me.DDL_Materials.DataTextField = "MAT_NAME"
        Me.DDL_Materials.DataValueField = "MAT_CODE"
        Me.DDL_Materials.DataSource = GetMaterialsList()
        Me.DDL_Materials.DataBind()
        DDL_Materials.Items.Insert(0, "")
    End Sub
    Public Sub PopulateDocType()
        Me.DDL_Doc_Type.DataTextField = "DOC_TYPE_NAME"
        Me.DDL_Doc_Type.DataValueField = "DOC_TYPE_CODE"
        Me.DDL_Doc_Type.DataSource = GetDocTypeList()
        Me.DDL_Doc_Type.DataBind()
        DDL_Doc_Type.Items.Insert(0, "")
    End Sub
    Public Sub PopulateDocType2()
        Me.DDL_Documents.DataTextField = "DOC_TYPE_NAME"
        Me.DDL_Documents.DataValueField = "DOC_TYPE_CODE"
        Me.DDL_Documents.DataSource = GetDocTypeList()
        Me.DDL_Documents.DataBind()
        DDL_Documents.Items.Insert(0, "")
    End Sub
    Public Sub PopulateStatus()
        DDL_Status.DataTextField = "STA_NAME"
        DDL_Status.DataValueField = "STA_CODE"
        DDL_Status.DataSource = GetCopyStatusList()
        DDL_Status.DataBind()
        DDL_Status.Items.Insert(0, "")
    End Sub
    Public Sub PopulateStatus2()
        DDL_CurrentStatus.DataTextField = "STA_NAME"
        DDL_CurrentStatus.DataValueField = "STA_CODE"
        DDL_CurrentStatus.DataSource = GetCopyStatusList()
        DDL_CurrentStatus.DataBind()
        DDL_CurrentStatus.Items.Insert(0, "")
    End Sub
    Public Sub FillDateTime()
        Dim TODAY_DATE As Date = Nothing
        TODAY_DATE = Now.Date
        TODAY_DATE = Convert.ToDateTime(TODAY_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert to indian 

        Dim DUE_DATE As Date = Nothing
        If txt_Cir_DueDays.Text <> "" Then
            DUE_DATE = (TODAY_DATE.AddDays(txt_Cir_DueDays.Text))
            txt_Cir_DueDate.Text = Format(DUE_DATE, "dd/MM/yyyy")
        Else
            txt_Cir_DueDate.Text = ""
        End If

        txt_Cir_IssueDate.Text = Format(TODAY_DATE, "dd/MM/yyyy")
        txt_Cir_ReserveDate.Text = Format(TODAY_DATE, "dd/MM/yyyy")

        Dim TIME_NOW As DateTime = Nothing
        TIME_NOW = Now.TimeOfDay.ToString

        txt_Cir_IssueTime.Text = TIME_NOW
        txt_Cir_ReserveTime.Text = TIME_NOW
        Cir_Issue_Bttn.Focus()
    End Sub
    Protected Sub Cir_Cancel_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Cir_Cancel_Bttn.Click
        txt_Cir_AccessionNo.Text = ""
        txt_Cir_AccessionNo_TextChanged(sender, e)
        txt_Cir_MemNo.Text = ""
        txt_Cir_MemNo_TextChanged(sender, e)
        txt_Cir_IssueDate.Text = ""
        txt_Cir_IssueTime.Text = ""
        txt_Cir_DueDate.Text = ""
        txt_Cir_ReserveDate.Text = ""
        txt_Cir_ReserveTime.Text = ""
        txt_Cir_ReceivedBy.Text = ""
        txt_Cir_Remarks.Text = ""
        Lbl_Error.Text = ""
    End Sub
    'issue book
    Protected Sub Cir_Issue_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Cir_Issue_Bttn.Click
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing
        Try

            If IsPostBack = True Then
                Dim iloop As Integer
                Dim strcurrentchar As Object
                Dim c As Integer
                Dim counter1, counter2, counter3, counter4, counter5, counter6, counter7, counter8, counter9, counter10, counter11, counter12, counter13, counter14, counter15, counter16, counter17, counter18, counter19 As Integer

                Dim MEM_ID As Integer = Nothing
                If Label16.Text <> "" Then
                    MEM_ID = Trim(Label16.Text)
                    If IsNumeric(MEM_ID) = False Then
                        Lbl_Error.Text = "Error: Member ID is not Proper!"
                        txt_Cir_MemNo.Focus()
                        Exit Sub
                    End If
                    If Len(MEM_ID) > 10 Then
                        Lbl_Error.Text = "Error: Member ID is not Proper!"
                        txt_Cir_MemNo.Focus()
                        Exit Sub
                    End If
                    MEM_ID = " " & MEM_ID & " "
                    If InStr(1, MEM_ID, "CREATE", 1) > 0 Or InStr(1, MEM_ID, "DELETE", 1) > 0 Or InStr(1, MEM_ID, "DROP", 1) > 0 Or InStr(1, MEM_ID, "INSERT", 1) > 1 Or InStr(1, MEM_ID, "TRACK", 1) > 1 Or InStr(1, MEM_ID, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        txt_Cir_MemNo.Focus()
                        Exit Sub
                    End If
                    MEM_ID = TrimX(MEM_ID)
                    'check unwanted characters
                    c = 0
                    counter1 = 0
                    For iloop = 1 To Len(MEM_ID.ToString)
                        strcurrentchar = Mid(MEM_ID, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter1 = 1
                            End If
                        End If
                    Next
                    If counter1 = 1 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        txt_Cir_MemNo.Focus()
                        Exit Sub
                    End If
                Else
                    Lbl_Error.Text = "Error: Member not displayed for Transaction!"
                    txt_Cir_MemNo.Focus()
                    Exit Sub
                End If

                Dim HOLD_ID As Integer = Nothing
                If RadioButton1.Checked = True Then
                    If Label15.Text <> "" Then
                        HOLD_ID = Trim(Label15.Text)
                        If IsNumeric(HOLD_ID) = False Then
                            Lbl_Error.Text = "Error: Item ID is not Proper!"
                            txt_Cir_AccessionNo.Focus()
                            Exit Sub
                        End If
                        If Len(HOLD_ID) > 10 Then
                            Lbl_Error.Text = "Error: Item ID is not Proper!"
                            txt_Cir_AccessionNo.Focus()
                            Exit Sub
                        End If
                        HOLD_ID = " " & HOLD_ID & " "
                        If InStr(1, HOLD_ID, "CREATE", 1) > 0 Or InStr(1, HOLD_ID, "DELETE", 1) > 0 Or InStr(1, HOLD_ID, "DROP", 1) > 0 Or InStr(1, HOLD_ID, "INSERT", 1) > 1 Or InStr(1, HOLD_ID, "TRACK", 1) > 1 Or InStr(1, HOLD_ID, "TRACE", 1) > 1 Then
                            Lbl_Error.Text = "Error: Input is not Valid !"
                            txt_Cir_AccessionNo.Focus()
                            Exit Sub
                        End If
                        HOLD_ID = TrimX(HOLD_ID)
                        'check unwanted characters
                        c = 0
                        counter2 = 0
                        For iloop = 1 To Len(HOLD_ID.ToString)
                            strcurrentchar = Mid(HOLD_ID, iloop, 1)
                            If c = 0 Then
                                If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter2 = 1
                                End If
                            End If
                        Next
                        If counter2 = 1 Then
                            Lbl_Error.Text = "Error: Input is not Valid !"
                            txt_Cir_AccessionNo.Focus()
                            Exit Sub
                        End If

                        'check holding status of this book
                        Dim str As Object = Nothing
                        Dim flag As Object = Nothing
                        str = "SELECT HOLD_ID FROM HOLDINGS WHERE (HOLD_ID ='" & Trim(HOLD_ID) & "') AND (STA_CODE ='2') AND (LIB_CODE ='" & Trim(LibCode) & "')"
                        Dim cmd1 As New SqlCommand(str, SqlConn)
                        SqlConn.Open()
                        flag = cmd1.ExecuteScalar
                        SqlConn.Close()
                        If flag <> Nothing Then
                            Lbl_Error.Text = "This Book is already issued!"
                            txt_Cir_AccessionNo.Focus()
                            Exit Sub
                        End If
                    Else
                        Lbl_Error.Text = "Error: Item details not displayed for Transaction!"
                        txt_Cir_AccessionNo.Focus()
                        Exit Sub
                    End If
                End If

                Dim COPY_ID As Integer = Nothing
                If RadioButton2.Checked = True Then
                    If Label15.Text <> "" Then
                        COPY_ID = Trim(Label15.Text)
                        If IsNumeric(COPY_ID) = False Then
                            Lbl_Error.Text = "Error: Item ID is not Proper!"
                            txt_Cir_AccessionNo.Focus()
                            Exit Sub
                        End If
                        If Len(COPY_ID) > 10 Then
                            Lbl_Error.Text = "Error: Item ID is not Proper!"
                            txt_Cir_AccessionNo.Focus()
                            Exit Sub
                        End If
                        COPY_ID = " " & COPY_ID & " "
                        If InStr(1, COPY_ID, "CREATE", 1) > 0 Or InStr(1, COPY_ID, "DELETE", 1) > 0 Or InStr(1, COPY_ID, "DROP", 1) > 0 Or InStr(1, COPY_ID, "INSERT", 1) > 1 Or InStr(1, COPY_ID, "TRACK", 1) > 1 Or InStr(1, COPY_ID, "TRACE", 1) > 1 Then
                            Lbl_Error.Text = "Error: Input is not Valid !"
                            txt_Cir_AccessionNo.Focus()
                            Exit Sub
                        End If
                        COPY_ID = TrimX(COPY_ID)
                        'check unwanted characters
                        c = 0
                        counter2 = 0
                        For iloop = 1 To Len(COPY_ID.ToString)
                            strcurrentchar = Mid(COPY_ID, iloop, 1)
                            If c = 0 Then
                                If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter2 = 1
                                End If
                            End If
                        Next
                        If counter2 = 1 Then
                            Lbl_Error.Text = "Error: Input is not Valid !"
                            txt_Cir_AccessionNo.Focus()
                            Exit Sub
                        End If

                        'check holding status of this book
                        Dim str7 As Object = Nothing
                        Dim flag7 As Object = Nothing
                        str7 = "SELECT COPY_ID FROM LOOSE_ISSUES_COPIES WHERE (COPY_ID ='" & Trim(COPY_ID) & "') AND (STA_CODE ='2') AND (LIB_CODE ='" & Trim(LibCode) & "')"
                        Dim cmd7 As New SqlCommand(str7, SqlConn)
                        SqlConn.Open()
                        flag7 = cmd7.ExecuteScalar
                        SqlConn.Close()
                        If flag7 <> Nothing Then
                            Lbl_Error.Text = "This Issue of Journal / Magazine is already issued!"
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myalert", "alert('This Issue of Journal / Magazine is already issued!');", True)
                            txt_Cir_AccessionNo.Focus()
                            Exit Sub
                        End If
                    Else
                        Lbl_Error.Text = "Error: Item details not displayed for Transaction!"
                        txt_Cir_AccessionNo.Focus()
                        Exit Sub
                    End If
                End If

                'Issue Date
                Dim ISSUE_DATE As Object = Nothing
                If txt_Cir_IssueDate.Text <> "" Then
                    ISSUE_DATE = TrimX(txt_Cir_IssueDate.Text)
                    ISSUE_DATE = RemoveQuotes(ISSUE_DATE)
                    ISSUE_DATE = Convert.ToDateTime(ISSUE_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                    If Len(ISSUE_DATE) > 12 Then
                        Lbl_Error.Text = " Input is not Valid..."
                        Me.txt_Cir_IssueDate.Focus()
                        Exit Sub
                    End If
                    ISSUE_DATE = " " & ISSUE_DATE & " "
                    If InStr(1, ISSUE_DATE, "CREATE", 1) > 0 Or InStr(1, ISSUE_DATE, "DELETE", 1) > 0 Or InStr(1, ISSUE_DATE, "DROP", 1) > 0 Or InStr(1, ISSUE_DATE, "INSERT", 1) > 1 Or InStr(1, ISSUE_DATE, "TRACK", 1) > 1 Or InStr(1, ISSUE_DATE, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "  Input is not Valid... "
                        Me.txt_Cir_IssueDate.Focus()
                        Exit Sub
                    End If
                    ISSUE_DATE = TrimX(ISSUE_DATE)
                    'check unwanted characters
                    c = 0
                    counter3 = 0
                    For iloop = 1 To Len(ISSUE_DATE)
                        strcurrentchar = Mid(ISSUE_DATE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter3 = 1
                            End If
                        End If
                    Next
                    If counter3 = 1 Then
                        Lbl_Error.Text = "data is not Valid... "
                        Me.txt_Cir_IssueDate.Focus()
                        Exit Sub
                    End If
                Else
                    ISSUE_DATE = Now.Date
                    ISSUE_DATE = Convert.ToDateTime(ISSUE_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us
                End If

                'Issue Time
                Dim ISSUE_TIME As Object = Nothing
                If txt_Cir_IssueTime.Text <> "" Then
                    ISSUE_TIME = TrimX(txt_Cir_IssueTime.Text)
                    ISSUE_TIME = RemoveQuotes(ISSUE_TIME)

                    If Len(ISSUE_TIME) > 12 Then
                        Lbl_Error.Text = " Input is not Valid..."
                        Me.txt_Cir_IssueTime.Focus()
                        Exit Sub
                    End If
                    ISSUE_TIME = " " & ISSUE_TIME & " "
                    If InStr(1, ISSUE_TIME, "CREATE", 1) > 0 Or InStr(1, ISSUE_TIME, "DELETE", 1) > 0 Or InStr(1, ISSUE_TIME, "DROP", 1) > 0 Or InStr(1, ISSUE_TIME, "INSERT", 1) > 1 Or InStr(1, ISSUE_TIME, "TRACK", 1) > 1 Or InStr(1, ISSUE_TIME, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "  Input is not Valid... "
                        Me.txt_Cir_IssueTime.Focus()
                        Exit Sub
                    End If
                    ISSUE_TIME = TrimX(ISSUE_TIME)
                    'check unwanted characters
                    c = 0
                    counter4 = 0
                    For iloop = 1 To Len(ISSUE_TIME)
                        strcurrentchar = Mid(ISSUE_TIME, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter4 = 1
                            End If
                        End If
                    Next
                    If counter4 = 1 Then
                        Lbl_Error.Text = "data is not Valid... "
                        Me.txt_Cir_IssueTime.Focus()
                        Exit Sub
                    End If
                Else
                    ISSUE_TIME = Now.TimeOfDay
                    'ISSUE_TIME = Convert.ToDateTime(ISSUE_TIME, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us
                End If

                'Due Date
                Dim DUE_DATE As Object = Nothing
                If txt_Cir_DueDate.Text <> "" Then
                    DUE_DATE = TrimX(txt_Cir_DueDate.Text)
                    DUE_DATE = RemoveQuotes(DUE_DATE)
                    DUE_DATE = Convert.ToDateTime(DUE_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                    If Len(DUE_DATE) > 12 Then
                        Lbl_Error.Text = " Input is not Valid..."
                        Me.txt_Cir_DueDate.Focus()
                        Exit Sub
                    End If
                    DUE_DATE = " " & DUE_DATE & " "
                    If InStr(1, DUE_DATE, "CREATE", 1) > 0 Or InStr(1, DUE_DATE, "DELETE", 1) > 0 Or InStr(1, DUE_DATE, "DROP", 1) > 0 Or InStr(1, DUE_DATE, "INSERT", 1) > 1 Or InStr(1, DUE_DATE, "TRACK", 1) > 1 Or InStr(1, DUE_DATE, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "  Input is not Valid... "
                        Me.txt_Cir_DueDate.Focus()
                        Exit Sub
                    End If
                    DUE_DATE = TrimX(DUE_DATE)
                    'check unwanted characters
                    c = 0
                    counter5 = 0
                    For iloop = 1 To Len(DUE_DATE)
                        strcurrentchar = Mid(DUE_DATE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter5 = 1
                            End If
                        End If
                    Next
                    If counter5 = 1 Then
                        Lbl_Error.Text = "data is not Valid... "
                        Me.txt_Cir_DueDate.Focus()
                        Exit Sub
                    End If
                Else
                    If DDL_CollectionType.SelectedValue = "R" Then
                        Lbl_Error.Text = "This copy belongs to REFERENCE Collection Type!  "
                        Me.txt_Cir_DueDate.Focus()
                        Exit Sub
                    Else
                        Lbl_Error.Text = "Due Date is not defined, plz define DUE DAYS of this document in member registration!  "
                        Me.txt_Cir_DueDate.Focus()
                        Exit Sub
                    End If
                End If

                'REMARKS
                Dim REMARKS As Object = Nothing
                If txt_Cir_Remarks.Text <> "" Then
                    REMARKS = TrimAll(txt_Cir_Remarks.Text)
                    REMARKS = RemoveQuotes(REMARKS)
                    If REMARKS.Length > 255 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        txt_Cir_Remarks.Focus()
                        Exit Sub
                    End If

                    REMARKS = " " & REMARKS & " "
                    If InStr(1, REMARKS, "CREATE", 1) > 0 Or InStr(1, REMARKS, "DELETE", 1) > 0 Or InStr(1, REMARKS, "DROP", 1) > 0 Or InStr(1, REMARKS, "INSERT", 1) > 1 Or InStr(1, REMARKS, "TRACK", 1) > 1 Or InStr(1, REMARKS, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = " Do Not use Reserve Words... "
                        Me.txt_Cir_Remarks.Focus()
                        Exit Sub
                    End If
                    REMARKS = TrimAll(REMARKS)
                    'check unwanted characters
                    c = 0
                    counter6 = 0
                    For iloop = 1 To Len(REMARKS.ToString)
                        strcurrentchar = Mid(REMARKS, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter6 = 1
                            End If
                        End If
                    Next
                    If counter6 = 1 Then
                        Lbl_Error.Text = " Do Not use un-wanted Characters... "
                        Me.txt_Cir_Remarks.Focus()
                        Exit Sub
                    End If
                Else
                    REMARKS = ""
                End If

                'RECEIVED_BY
                Dim RECEIVED_BY As Object = Nothing
                If txt_Cir_ReceivedBy.Text <> "" Then
                    RECEIVED_BY = TrimAll(txt_Cir_ReceivedBy.Text)
                    RECEIVED_BY = RemoveQuotes(RECEIVED_BY)
                    If RECEIVED_BY.Length > 100 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        txt_Cir_ReceivedBy.Focus()
                        Exit Sub
                    End If

                    RECEIVED_BY = " " & RECEIVED_BY & " "
                    If InStr(1, RECEIVED_BY, "CREATE", 1) > 0 Or InStr(1, RECEIVED_BY, "DELETE", 1) > 0 Or InStr(1, RECEIVED_BY, "DROP", 1) > 0 Or InStr(1, RECEIVED_BY, "INSERT", 1) > 1 Or InStr(1, RECEIVED_BY, "TRACK", 1) > 1 Or InStr(1, RECEIVED_BY, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = " Do Not use Reserve Words... "
                        Me.txt_Cir_ReceivedBy.Focus()
                        Exit Sub
                    End If
                    RECEIVED_BY = TrimAll(RECEIVED_BY)
                    'check unwanted characters
                    c = 0
                    counter6 = 0
                    For iloop = 1 To Len(RECEIVED_BY.ToString)
                        strcurrentchar = Mid(RECEIVED_BY, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter6 = 1
                            End If
                        End If
                    Next
                    If counter6 = 1 Then
                        Lbl_Error.Text = " Do Not use un-wanted Characters... "
                        Me.txt_Cir_ReceivedBy.Focus()
                        Exit Sub
                    End If
                Else
                    RECEIVED_BY = ""
                End If


                Dim STATUS As String = Nothing
                STATUS = "Issued"

                Dim DATE_ADDED As Object = Nothing
                DATE_ADDED = Now.Date
                DATE_ADDED = Convert.ToDateTime(DATE_ADDED, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                Dim IP As Object = Nothing
                IP = Request.UserHostAddress.Trim

                Dim USER_CODE As Object = Nothing
                USER_CODE = UserCode

                'check over-ride
                If txt_Cir_OverRide.Text = "N" Then
                    Dim ENTITLEMENT As Integer = Nothing
                    Dim ALREADY_ISSUED As Integer = Nothing
                    If txt_Cir_Entitlement.Text = "" Then
                        Lbl_Error.Text = "There is no ENTITLEMENT is defined of " & DDL_SubCategories.Text & " for this member!"
                        Exit Sub
                    Else
                        ENTITLEMENT = TrimX(txt_Cir_Entitlement.Text)
                    End If

                    If txt_Cir_Issued.Text <> "" Then
                        ALREADY_ISSUED = TrimX(txt_Cir_Issued.Text)
                    Else
                        ALREADY_ISSUED = 0
                    End If

                    If ENTITLEMENT = ALREADY_ISSUED Then
                        Lbl_Error.Text = "No more copy can be issued as Quota of This Member is Exhausted for this Member Sub Category: " & DDL_SubCategories.SelectedItem.Text
                        Exit Sub
                    ElseIf ENTITLEMENT < ALREADY_ISSUED Then
                        Lbl_Error.Text = "No more copy can be issued as Quota of This Member is Exhausted for this Memer Sub Category: " & DDL_SubCategories.SelectedItem.Text
                        Exit Sub
                    End If

                    'check closing date
                    If txt_Cir_ClosingDate.Text <> "" Then
                        Dim CLOSING_DATE As Date = Nothing
                        Dim TODAY_DATE As Date = Nothing
                        Dim nd As Object
                        nd = datechk(txt_Cir_ClosingDate.Text)
                        CLOSING_DATE = Date.Parse(nd, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))

                        'compare dates
                        nd = Nothing
                        nd = datechk(Now.Date)
                        TODAY_DATE = Date.Parse(nd, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))
                        Dim result As Integer = DateTime.Compare(CLOSING_DATE, TODAY_DATE)

                        If result < 0 Then
                            txt_Cir_ClosingDate.ForeColor = Drawing.Color.Red
                            Lbl_Error.Text = "Closing Date is expired!"
                        End If
                    Else
                        Lbl_Error.Text = "Closing Date is not Defined in Member Details!"
                        Exit Sub
                    End If
                End If

                If DDL_CollectionType.Text <> "" Then
                    If DDL_CollectionType.SelectedValue = "R" Then
                        Lbl_Error.Text = "This is REFERENCE BOOK - can not be issued!"
                        Exit Sub
                    End If
                Else
                    Lbl_Error.Text = "Collection Type of This books is not Defined!"
                    Exit Sub
                End If

                If DDL_Status.Text <> "" Then
                    If Not DDL_Status.SelectedValue = "1" Then
                        Lbl_Error.Text = "This Copy Status is not AVAILABLE - can not be issued!"
                        Exit Sub
                    End If
                Else
                    Lbl_Error.Text = "Copy Status is not Defined!"
                    Exit Sub
                End If

                'check if book is reserved to the same member
                Dim str8 As Object = Nothing
                Dim flagReserve As Integer = Nothing
                If RadioButton1.Checked = True Then
                    str8 = "SELECT CIR_ID FROM CIRCULATION WHERE (HOLD_ID ='" & Trim(HOLD_ID) & "') AND (STATUS ='Reserved') AND (LIB_CODE ='" & Trim(LibCode) & "') AND (MEM_ID ='" & Trim(MEM_ID) & "') "
                Else
                    str8 = "SELECT CIR_ID FROM CIRCULATION WHERE (COPY_ID ='" & Trim(COPY_ID) & "') AND (STATUS ='Reserved') AND (LIB_CODE ='" & Trim(LibCode) & "') AND (MEM_ID ='" & Trim(MEM_ID) & "') "
                End If
                Dim cmd8 As New SqlCommand(str8, SqlConn)
                SqlConn.Open()
                flagReserve = cmd8.ExecuteScalar
                SqlConn.Close()

                If SqlConn.State = 0 Then
                    SqlConn.Open()
                End If
                thisTransaction = SqlConn.BeginTransaction()

                Dim intValue As Integer = 0
                Dim objCommand As New SqlCommand
                objCommand.Connection = SqlConn
                objCommand.Transaction = thisTransaction
                objCommand.CommandType = CommandType.Text
                If flagReserve = Nothing Then
                    objCommand.CommandText = "INSERT INTO CIRCULATION (MEM_ID, HOLD_ID, COPY_ID, ISSUE_DATE, ISSUE_TIME, DUE_DATE, REMARKS, RECEIVED_BY, STATUS, DATE_ADDED, USER_CODE, LIB_CODE, IP) " & _
                                             " VALUES (@MEM_ID, @HOLD_ID, @COPY_ID, @ISSUE_DATE, @ISSUE_TIME, @DUE_DATE, @REMARKS, @RECEIVED_BY, @STATUS, @DATE_ADDED, @USER_CODE, @LIB_CODE, @IP); " & _
                                             " SELECT SCOPE_IDENTITY();"
                Else
                    objCommand.CommandText = "UPDATE CIRCULATION SET MEM_ID=@MEM_ID, HOLD_ID=@HOLD_ID, COPY_ID=@COPY_ID, ISSUE_DATE=@ISSUE_DATE, ISSUE_TIME=@ISSUE_TIME, DUE_DATE=@DUE_DATE, REMARKS=@REMARKS, RECEIVED_BY=@RECEIVED_BY, STATUS=@STATUS, DATE_ADDED=@DATE_ADDED, USER_CODE=@USER_CODE, LIB_CODE=@LIB_CODE, IP=@IP  WHERE (CIR_ID = @CIR_ID  AND LIB_CODE =@LIB_CODE);"
                End If

                objCommand.Parameters.Add("@CIR_ID", SqlDbType.Int)
                If flagReserve = 0 Then
                    objCommand.Parameters("@CIR_ID").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@CIR_ID").Value = flagReserve
                End If

                objCommand.Parameters.Add("@MEM_ID", SqlDbType.Int)
                If MEM_ID = 0 Then
                    objCommand.Parameters("@MEM_ID").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@MEM_ID").Value = MEM_ID
                End If

                objCommand.Parameters.Add("@HOLD_ID", SqlDbType.Int)
                If HOLD_ID = 0 Then
                    objCommand.Parameters("@HOLD_ID").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@HOLD_ID").Value = HOLD_ID
                End If

                objCommand.Parameters.Add("@COPY_ID", SqlDbType.Int)
                If COPY_ID = 0 Then
                    objCommand.Parameters("@COPY_ID").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@COPY_ID").Value = COPY_ID
                End If

                objCommand.Parameters.Add("@ISSUE_DATE", SqlDbType.DateTime)
                If ISSUE_DATE = "" Then
                    objCommand.Parameters("@ISSUE_DATE").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@ISSUE_DATE").Value = ISSUE_DATE
                End If

                objCommand.Parameters.Add("@ISSUE_TIME", SqlDbType.DateTime)
                If ISSUE_TIME = "" Then
                    objCommand.Parameters("@ISSUE_TIME").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@ISSUE_TIME").Value = ISSUE_TIME
                End If

                objCommand.Parameters.Add("@DUE_DATE", SqlDbType.DateTime)
                If DUE_DATE = "" Then
                    objCommand.Parameters("@DUE_DATE").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@DUE_DATE").Value = DUE_DATE
                End If

                objCommand.Parameters.Add("@REMARKS", SqlDbType.NVarChar)
                If REMARKS = "" Then
                    objCommand.Parameters("@REMARKS").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@REMARKS").Value = REMARKS
                End If

                objCommand.Parameters.Add("@RECEIVED_BY", SqlDbType.NVarChar)
                If RECEIVED_BY = "" Then
                    objCommand.Parameters("@RECEIVED_BY").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@RECEIVED_BY").Value = RECEIVED_BY
                End If

                objCommand.Parameters.Add("@STATUS", SqlDbType.VarChar)
                If STATUS = "" Then
                    objCommand.Parameters("@STATUS").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@STATUS").Value = STATUS
                End If

                objCommand.Parameters.Add("@DATE_ADDED", SqlDbType.DateTime)
                If DATE_ADDED = "" Then
                    objCommand.Parameters("@DATE_ADDED").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@DATE_ADDED").Value = DATE_ADDED
                End If

                objCommand.Parameters.Add("@IP", SqlDbType.VarChar)
                If IP = "" Then
                    objCommand.Parameters("@IP").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@IP").Value = IP
                End If

                objCommand.Parameters.Add("@USER_CODE", SqlDbType.NVarChar)
                If USER_CODE = "" Then
                    objCommand.Parameters("@USER_CODE").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@USER_CODE").Value = USER_CODE
                End If

                objCommand.Parameters.Add("@LIB_CODE", SqlDbType.NVarChar)
                If LibCode = "" Then
                    objCommand.Parameters("@LIB_CODE").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@LIB_CODE").Value = LibCode
                End If

                Dim dr As SqlDataReader
                dr = objCommand.ExecuteReader()

                If flagReserve = Nothing Then
                    If dr.Read Then
                        intValue = dr.GetValue(0)
                    End If
                Else
                    intValue = flagReserve
                End If
                dr.Close()

                'update HOLDINGS TABLE
                If intValue <> 0 Then
                    If RadioButton1.Checked = True Then 'books
                        Dim objCommand4 As New SqlCommand
                        objCommand4.Connection = SqlConn
                        objCommand4.Transaction = thisTransaction
                        objCommand4.CommandType = CommandType.Text
                        objCommand4.CommandText = "UPDATE HOLDINGS SET STA_CODE ='2' WHERE (HOLD_ID = @HOLD_ID) AND (LIB_CODE =@LIB_CODE);"

                        objCommand4.Parameters.Add("@HOLD_ID", SqlDbType.Int)
                        objCommand4.Parameters("@HOLD_ID").Value = HOLD_ID

                        objCommand4.Parameters.Add("@LIB_CODE", SqlDbType.NVarChar)
                        objCommand4.Parameters("@LIB_CODE").Value = LibCode

                        Dim dr4 As SqlDataReader
                        dr4 = objCommand4.ExecuteReader()
                        dr4.Close()
                    End If
                    If RadioButton2.Checked = True Then 'loose 
                        Dim objCommand4 As New SqlCommand
                        objCommand4.Connection = SqlConn
                        objCommand4.Transaction = thisTransaction
                        objCommand4.CommandType = CommandType.Text
                        objCommand4.CommandText = "UPDATE LOOSE_ISSUES_COPIES SET STA_CODE ='2' WHERE (COPY_ID = @COPY_ID) AND (LIB_CODE =@LIB_CODE);"

                        objCommand4.Parameters.Add("@COPY_ID", SqlDbType.Int)
                        objCommand4.Parameters("@COPY_ID").Value = COPY_ID

                        objCommand4.Parameters.Add("@LIB_CODE", SqlDbType.NVarChar)
                        objCommand4.Parameters("@LIB_CODE").Value = LibCode

                        Dim dr4 As SqlDataReader
                        dr4 = objCommand4.ExecuteReader()
                        dr4.Close()
                    End If
                End If

                thisTransaction.Commit()
                SqlConn.Close()
                flagReserve = Nothing
                txt_Cir_AccessionNo.Text = ""
                txt_Cir_DueDate.Text = ""
                txt_Cir_AccessionNo_TextChanged(sender, e)
            End If
                PopulateGridIssuedBooks()
        Catch q As SqlException
            thisTransaction.Rollback()
            Lbl_Error.Text = "Error: " & (q.Message())
        Catch ex As Exception
            Lbl_Error.Text = "Error: " & (ex.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    Public Sub PopulateGridIssuedBooks()
        Dim dt As DataTable = Nothing
        Try
            If Label16.Text <> "" Then
                Dim SQL As String = Nothing
                SQL = "  SELECT CIRCULATION.HOLD_ID, Accession = CASE (isnull(cast (HOLDINGS_CATS_AUTHORS_VIEW.ACCESSION_NO as varchar(50)),'')) WHen ''  " & _
                        " THEN cast(CIRCULATION.copy_id as varchar(20)) ELSE ACCESSION_NO END,CIRCULATION.COPY_ID," & _
                        " Title = CASE (isnull(HOLDINGS_CATS_AUTHORS_VIEW.Title,'')) WHen ''  THEN CATS_LOOSE_ISSUES_COPIES_VIEW.TITLE ELSE HOLDINGS_CATS_AUTHORS_VIEW.title END, " & _
                        " convert(char(10),CIRCULATION.ISSUE_DATE,103) as ISSUE_DATE, CIRCULATION.ISSUE_TIME, " & _
                        " convert(char(10),CIRCULATION.DUE_DATE,103) as DUE_DATE, CIRCULATION.STATUS, convert(char(10),CIRCULATION.RESERVE_DATE,103) as RESERVE_DATE,  CIRCULATION.CIR_ID " & _
                        " FROM CIRCULATION LEFT OUTER JOIN    CATS_LOOSE_ISSUES_COPIES_VIEW ON CIRCULATION.COPY_ID = CATS_LOOSE_ISSUES_COPIES_VIEW.COPY_ID LEFT OUTER JOIN " & _
                        " HOLDINGS_CATS_AUTHORS_VIEW ON CIRCULATION.HOLD_ID = HOLDINGS_CATS_AUTHORS_VIEW.HOLD_ID  " & _
                        " WHERE (CIRCULATION.LIB_CODE = '" & Trim(LibCode) & "') AND (CIRCULATION.MEM_ID = '" & (Label16.Text) & "') and (CIRCULATION.STATUS='Issued' or CIRCULATION.STATUS='Reserved') ORDER BY CIRCULATION. CIR_ID DESC"
                Dim ds As New DataSet
                Dim da As New SqlDataAdapter(SQL, SqlConn)
                If SqlConn.State = 0 Then
                    SqlConn.Open()
                End If
                da.Fill(ds)

                dt = ds.Tables(0).Copy
                Dim RecordCount As Long = 0
                If dt.Rows.Count = 0 Then
                    Grid1.DataSource = Nothing
                    Grid1.DataBind()
                Else
                    RecordCount = dt.Rows.Count
                    Me.Grid1.DataSource = dt
                    Me.Grid1.DataBind()
                    Grid1.Visible = True
                End If
                ViewState("dt") = dt
            Else
                Grid1.DataSource = Nothing
                Grid1.DataBind()
                ' Grid1.Columns.Clear()
            End If
        Catch s As Exception
            Lbl_Error.Text = "Error: " & (s.Message())
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
    Protected Sub Grid1_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles Grid1.PageIndexChanging
        Try
            'rebind datagrid
            Grid1.DataSource = ViewState("dt") 'temp
            Grid1.PageIndex = e.NewPageIndex
            index = e.NewPageIndex * Grid1.PageSize
            Grid1.DataBind()
        Catch s As Exception
            Label6.Text = "Error:  there is error in page index !"
            Label15.Text = ""
        End Try
    End Sub
    'gridview sorting event
    Protected Sub Grid1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles Grid1.Sorting
        Dim temp As DataTable = CType(ViewState("dt"), DataTable)
        GridViewSortExpression = e.SortExpression
        Grid1.DataSource = temp
        Dim pageIndex As Integer = Grid1.PageIndex
        Grid1.DataSource = SortDataTable(Grid1.DataSource, False)
        Grid1.DataBind()
        Grid1.PageIndex = pageIndex
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
    'on select of member name from drop-down
    Protected Sub DDL_Members_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDL_Members.SelectedIndexChanged
        If DDL_Members.Text <> "" Then
            txt_Cir_MemNo.Text = DDL_Members.SelectedValue
            txt_Cir_MemNo_TextChanged(sender, e)
            txt_Cir_AccessionNo.Text = ""
            txt_Cir_AccessionNo_TextChanged(sender, e)
        End If
    End Sub
    'rserve book already issued to others  
    Protected Sub Cir_Reserve_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Cir_Reserve_Bttn.Click
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing
        Try
            If IsPostBack = True Then
                Dim iloop As Integer
                Dim strcurrentchar As Object
                Dim c As Integer
                Dim counter1, counter2, counter3, counter4, counter5, counter6, counter7, counter8, counter9, counter10, counter11, counter12, counter13, counter14, counter15, counter16, counter17, counter18, counter19 As Integer


                Dim MEM_ID As Integer = Nothing
                If Label16.Text <> "" Then
                    MEM_ID = Trim(Label16.Text)
                    If IsNumeric(MEM_ID) = False Then
                        Lbl_Error.Text = "Error: Member ID is not Proper!"
                        txt_Cir_MemNo.Focus()
                        Exit Sub
                    End If
                    If Len(MEM_ID) > 10 Then
                        Lbl_Error.Text = "Error: Member ID is not Proper!"
                        txt_Cir_MemNo.Focus()
                        Exit Sub
                    End If
                    MEM_ID = " " & MEM_ID & " "
                    If InStr(1, MEM_ID, "CREATE", 1) > 0 Or InStr(1, MEM_ID, "DELETE", 1) > 0 Or InStr(1, MEM_ID, "DROP", 1) > 0 Or InStr(1, MEM_ID, "INSERT", 1) > 1 Or InStr(1, MEM_ID, "TRACK", 1) > 1 Or InStr(1, MEM_ID, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        txt_Cir_MemNo.Focus()
                        Exit Sub
                    End If
                    MEM_ID = TrimX(MEM_ID)
                    'check unwanted characters
                    c = 0
                    counter1 = 0
                    For iloop = 1 To Len(MEM_ID.ToString)
                        strcurrentchar = Mid(MEM_ID, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter1 = 1
                            End If
                        End If
                    Next
                    If counter1 = 1 Then
                        Lbl_Error.Text = "Error: Input is not Valid !"
                        txt_Cir_MemNo.Focus()
                        Exit Sub
                    End If
                Else
                    Lbl_Error.Text = "Error: Member not displayed for Transaction!"
                    txt_Cir_MemNo.Focus()
                    Exit Sub
                End If

                Dim HOLD_ID As Integer = Nothing
                If RadioButton1.Checked = True Then
                    If Label15.Text <> "" Then
                        HOLD_ID = Trim(Label15.Text)
                        If IsNumeric(HOLD_ID) = False Then
                            Lbl_Error.Text = "Error: Item ID is not Proper!"
                            txt_Cir_AccessionNo.Focus()
                            Exit Sub
                        End If
                        If Len(HOLD_ID) > 10 Then
                            Lbl_Error.Text = "Error: Item ID is not Proper!"
                            txt_Cir_AccessionNo.Focus()
                            Exit Sub
                        End If
                        HOLD_ID = " " & HOLD_ID & " "
                        If InStr(1, HOLD_ID, "CREATE", 1) > 0 Or InStr(1, HOLD_ID, "DELETE", 1) > 0 Or InStr(1, HOLD_ID, "DROP", 1) > 0 Or InStr(1, HOLD_ID, "INSERT", 1) > 1 Or InStr(1, HOLD_ID, "TRACK", 1) > 1 Or InStr(1, HOLD_ID, "TRACE", 1) > 1 Then
                            Lbl_Error.Text = "Error: Input is not Valid !"
                            txt_Cir_AccessionNo.Focus()
                            Exit Sub
                        End If
                        HOLD_ID = TrimX(HOLD_ID)
                        'check unwanted characters
                        c = 0
                        counter2 = 0
                        For iloop = 1 To Len(HOLD_ID.ToString)
                            strcurrentchar = Mid(HOLD_ID, iloop, 1)
                            If c = 0 Then
                                If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter2 = 1
                                End If
                            End If
                        Next
                        If counter2 = 1 Then
                            Lbl_Error.Text = "Error: Input is not Valid !"
                            txt_Cir_AccessionNo.Focus()
                            Exit Sub
                        End If

                        'check holding status of this book
                        Dim str As Object = Nothing
                        Dim flag As Object = Nothing
                        str = "SELECT HOLD_ID FROM HOLDINGS WHERE (HOLD_ID ='" & Trim(HOLD_ID) & "') AND (STA_CODE ='2') AND (LIB_CODE ='" & Trim(LibCode) & "')"
                        Dim cmd1 As New SqlCommand(str, SqlConn)
                        SqlConn.Open()
                        flag = cmd1.ExecuteScalar
                        SqlConn.Close()

                        If flag <> Nothing Then ' if books is issued
                            Dim str2 As Object = Nothing
                            Dim flag2 As Object = Nothing
                            str2 = "SELECT CIR_ID FROM CIRCULATION WHERE (HOLD_ID ='" & Trim(HOLD_ID) & "' AND STATUS ='Issued' AND MEM_ID = '" & Trim(MEM_ID) & "' AND LIB_CODE ='" & Trim(LibCode) & "')"
                            Dim cmd2 As New SqlCommand(str2, SqlConn)
                            SqlConn.Open()
                            flag2 = cmd2.ExecuteScalar
                            SqlConn.Close()
                            If flag2 <> Nothing Then
                                Lbl_Error.Text = "This Book is Already Issued to This Member!"
                                txt_Cir_AccessionNo.Focus()
                                Exit Sub
                            Else
                                Dim str3 As Object = Nothing
                                Dim flag3 As Object = Nothing
                                str3 = "SELECT CIR_ID FROM CIRCULATION WHERE (HOLD_ID ='" & Trim(HOLD_ID) & "' AND STATUS ='Reserved' AND MEM_ID = '" & Trim(MEM_ID) & "' AND LIB_CODE ='" & Trim(LibCode) & "')"
                                Dim cmd3 As New SqlCommand(str3, SqlConn)
                                SqlConn.Open()
                                flag3 = cmd3.ExecuteScalar
                                SqlConn.Close()
                                If flag3 <> Nothing Then
                                    Lbl_Error.Text = "This Book is Already Reserved to This Member!"
                                    txt_Cir_AccessionNo.Focus()
                                    Exit Sub
                                End If
                            End If
                        Else
                            Lbl_Error.Text = "This Book is Available!"
                            txt_Cir_AccessionNo.Focus()
                            Exit Sub
                        End If
                    Else
                        Lbl_Error.Text = "Error: Item details not displayed for Transaction!"
                        txt_Cir_AccessionNo.Focus()
                        Exit Sub
                    End If
                End If

                Dim COPY_ID As Integer = Nothing
                If RadioButton2.Checked = True Then
                    If Label15.Text <> "" Then
                        COPY_ID = Trim(Label15.Text)
                        If IsNumeric(COPY_ID) = False Then
                            Lbl_Error.Text = "Error: Item ID is not Proper!"
                            txt_Cir_AccessionNo.Focus()
                            Exit Sub
                        End If
                        If Len(COPY_ID) > 10 Then
                            Lbl_Error.Text = "Error: Item ID is not Proper!"
                            txt_Cir_AccessionNo.Focus()
                            Exit Sub
                        End If
                        COPY_ID = " " & COPY_ID & " "
                        If InStr(1, COPY_ID, "CREATE", 1) > 0 Or InStr(1, COPY_ID, "DELETE", 1) > 0 Or InStr(1, COPY_ID, "DROP", 1) > 0 Or InStr(1, COPY_ID, "INSERT", 1) > 1 Or InStr(1, COPY_ID, "TRACK", 1) > 1 Or InStr(1, COPY_ID, "TRACE", 1) > 1 Then
                            Lbl_Error.Text = "Error: Input is not Valid !"
                            txt_Cir_AccessionNo.Focus()
                            Exit Sub
                        End If
                        COPY_ID = TrimX(COPY_ID)
                        'check unwanted characters
                        c = 0
                        counter2 = 0
                        For iloop = 1 To Len(COPY_ID.ToString)
                            strcurrentchar = Mid(COPY_ID, iloop, 1)
                            If c = 0 Then
                                If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter2 = 1
                                End If
                            End If
                        Next
                        If counter2 = 1 Then
                            Lbl_Error.Text = "Error: Input is not Valid !"
                            txt_Cir_AccessionNo.Focus()
                            Exit Sub
                        End If

                        'check holding status of this copy
                        Dim str As Object = Nothing
                        Dim flag As Object = Nothing
                        str = "SELECT COPY_ID FROM LOOSE_ISSUES_COPIES WHERE (COPY_ID ='" & Trim(COPY_ID) & "') AND (STA_CODE ='2') AND (LIB_CODE ='" & Trim(LibCode) & "')"
                        Dim cmd1 As New SqlCommand(str, SqlConn)
                        SqlConn.Open()
                        flag = cmd1.ExecuteScalar
                        SqlConn.Close()

                        If flag <> Nothing Then ' if copy id is issued
                            Dim str2 As Object = Nothing
                            Dim flag2 As Object = Nothing
                            str2 = "SELECT CIR_ID FROM CIRCULATION WHERE (COPY_ID ='" & Trim(COPY_ID) & "' AND STATUS ='Issued' AND MEM_ID = '" & Trim(MEM_ID) & "' AND LIB_CODE ='" & Trim(LibCode) & "')"
                            Dim cmd2 As New SqlCommand(str2, SqlConn)
                            SqlConn.Open()
                            flag2 = cmd2.ExecuteScalar
                            SqlConn.Close()
                            If flag2 <> Nothing Then
                                Lbl_Error.Text = "This Item is Already Issued to This Member!"
                                txt_Cir_AccessionNo.Focus()
                                Exit Sub
                            Else
                                Dim str3 As Object = Nothing
                                Dim flag3 As Object = Nothing
                                str3 = "SELECT CIR_ID FROM CIRCULATION WHERE (COPY_ID ='" & Trim(COPY_ID) & "' AND STATUS ='Reserved' AND MEM_ID = '" & Trim(MEM_ID) & "' AND LIB_CODE ='" & Trim(LibCode) & "')"
                                Dim cmd3 As New SqlCommand(str3, SqlConn)
                                SqlConn.Open()
                                flag3 = cmd3.ExecuteScalar
                                SqlConn.Close()
                                If flag3 <> Nothing Then
                                    Lbl_Error.Text = "This Item is Already Reserved to This Member!"
                                    txt_Cir_AccessionNo.Focus()
                                    Exit Sub
                                End If
                            End If
                        Else
                            Lbl_Error.Text = "This Item is Available!"
                            txt_Cir_AccessionNo.Focus()
                            Exit Sub
                        End If
                    End If
                End If

                'Issue Date
                Dim RESERVE_DATE As Object = Nothing
                If txt_Cir_ReserveDate.Text <> "" Then
                    RESERVE_DATE = TrimX(txt_Cir_ReserveDate.Text)
                    RESERVE_DATE = RemoveQuotes(RESERVE_DATE)
                    RESERVE_DATE = datechk(RESERVE_DATE)
                    RESERVE_DATE = Date.Parse(RESERVE_DATE, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))

                    If Len(RESERVE_DATE) > 12 Then
                        Lbl_Error.Text = " Input is not Valid..."
                        Me.txt_Cir_ReserveDate.Focus()
                        Exit Sub
                    End If
                    RESERVE_DATE = " " & RESERVE_DATE & " "
                    If InStr(1, RESERVE_DATE, "CREATE", 1) > 0 Or InStr(1, RESERVE_DATE, "DELETE", 1) > 0 Or InStr(1, RESERVE_DATE, "DROP", 1) > 0 Or InStr(1, RESERVE_DATE, "INSERT", 1) > 1 Or InStr(1, RESERVE_DATE, "TRACK", 1) > 1 Or InStr(1, RESERVE_DATE, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "  Input is not Valid... "
                        Me.txt_Cir_ReserveDate.Focus()
                        Exit Sub
                    End If
                    RESERVE_DATE = TrimX(RESERVE_DATE)
                    'check unwanted characters
                    c = 0
                    counter3 = 0
                    For iloop = 1 To Len(RESERVE_DATE)
                        strcurrentchar = Mid(RESERVE_DATE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter3 = 1
                            End If
                        End If
                    Next
                    If counter3 = 1 Then
                        Lbl_Error.Text = "data is not Valid... "
                        Me.txt_Cir_ReserveDate.Focus()
                        Exit Sub
                    End If
                Else
                    RESERVE_DATE = Now.Date
                    RESERVE_DATE = datechk(RESERVE_DATE)
                    RESERVE_DATE = Date.Parse(RESERVE_DATE, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))
                End If

                'Reserve Time
                Dim RESERVE_TIME As Object = Nothing
                If txt_Cir_ReserveTime.Text <> "" Then
                    RESERVE_TIME = TrimX(txt_Cir_ReserveTime.Text)
                    RESERVE_TIME = RemoveQuotes(RESERVE_TIME)

                    If Len(RESERVE_TIME) > 12 Then
                        Lbl_Error.Text = " Input is not Valid..."
                        Me.txt_Cir_ReserveTime.Focus()
                        Exit Sub
                    End If
                    RESERVE_TIME = " " & RESERVE_TIME & " "
                    If InStr(1, RESERVE_TIME, "CREATE", 1) > 0 Or InStr(1, RESERVE_TIME, "DELETE", 1) > 0 Or InStr(1, RESERVE_TIME, "DROP", 1) > 0 Or InStr(1, RESERVE_TIME, "INSERT", 1) > 1 Or InStr(1, RESERVE_TIME, "TRACK", 1) > 1 Or InStr(1, RESERVE_TIME, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = "  Input is not Valid... "
                        Me.txt_Cir_ReserveTime.Focus()
                        Exit Sub
                    End If
                    RESERVE_TIME = TrimX(RESERVE_TIME)
                    'check unwanted characters
                    c = 0
                    counter4 = 0
                    For iloop = 1 To Len(RESERVE_TIME)
                        strcurrentchar = Mid(RESERVE_TIME, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter4 = 1
                            End If
                        End If
                    Next
                    If counter4 = 1 Then
                        Lbl_Error.Text = "data is not Valid... "
                        Me.txt_Cir_ReserveTime.Focus()
                        Exit Sub
                    End If
                Else
                    RESERVE_TIME = Now.TimeOfDay
                    'ISSUE_TIME = Convert.ToDateTime(ISSUE_TIME, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us
                End If

                'REMARKS
                Dim REMARKS As Object = Nothing
                If txt_Cir_Remarks.Text <> "" Then
                    REMARKS = TrimAll(txt_Cir_Remarks.Text)
                    REMARKS = RemoveQuotes(REMARKS)
                    If REMARKS.Length > 255 Then 'maximum length
                        Lbl_Error.Text = " Data must be of Proper Length.. "
                        txt_Cir_Remarks.Focus()
                        Exit Sub
                    End If

                    REMARKS = " " & REMARKS & " "
                    If InStr(1, REMARKS, "CREATE", 1) > 0 Or InStr(1, REMARKS, "DELETE", 1) > 0 Or InStr(1, REMARKS, "DROP", 1) > 0 Or InStr(1, REMARKS, "INSERT", 1) > 1 Or InStr(1, REMARKS, "TRACK", 1) > 1 Or InStr(1, REMARKS, "TRACE", 1) > 1 Then
                        Lbl_Error.Text = " Do Not use Reserve Words... "
                        Me.txt_Cir_Remarks.Focus()
                        Exit Sub
                    End If
                    REMARKS = TrimAll(REMARKS)
                    'check unwanted characters
                    c = 0
                    counter6 = 0
                    For iloop = 1 To Len(REMARKS.ToString)
                        strcurrentchar = Mid(REMARKS, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter6 = 1
                            End If
                        End If
                    Next
                    If counter6 = 1 Then
                        Lbl_Error.Text = " Do Not use un-wanted Characters... "
                        Me.txt_Cir_Remarks.Focus()
                        Exit Sub
                    End If
                Else
                    REMARKS = ""
                End If

                Dim STATUS As String = Nothing
                STATUS = "Reserved"

                Dim DATE_ADDED As Object = Nothing
                DATE_ADDED = Now.Date
                DATE_ADDED = datechk(DATE_ADDED)
                DATE_ADDED = Date.Parse(DATE_ADDED, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))

                Dim IP As Object = Nothing
                IP = Request.UserHostAddress.Trim

                Dim USER_CODE As Object = Nothing
                USER_CODE = UserCode

                'check closing date
                If txt_Cir_ClosingDate.Text <> "" Then
                    Dim CLOSING_DATE As Date = Nothing
                    Dim TODAY_DATE As Date = Nothing
                    Dim nd As Object
                    nd = datechk(txt_Cir_ClosingDate.Text)
                    CLOSING_DATE = Date.Parse(nd, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))

                    'compare dates
                    nd = Nothing
                    nd = datechk(Now.Date)
                    TODAY_DATE = Date.Parse(nd, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))
                    Dim result As Integer = DateTime.Compare(CLOSING_DATE, TODAY_DATE)

                    If result < 0 Then
                        txt_Cir_ClosingDate.ForeColor = Drawing.Color.Red
                        Lbl_Error.Text = "Closing Date is expired!"
                    End If
                Else
                    Lbl_Error.Text = "Closing Date is not Defined in Member Details!"
                    Exit Sub
                End If

                If DDL_CollectionType.Text <> "" Then
                    If DDL_CollectionType.SelectedValue = "R" Then
                        Lbl_Error.Text = "This is REFERENCE BOOK - can not be issued!"
                        Exit Sub
                    End If
                Else
                    Lbl_Error.Text = "Collection Type of This books is not Defined!"
                    Exit Sub
                End If

                If DDL_Status.Text <> "" Then
                    If DDL_Status.SelectedValue = "1" Then
                        Lbl_Error.Text = "This Copy Status is AVAILABLE - can not be Reserved!"
                        Exit Sub
                    End If
                Else
                    Lbl_Error.Text = "Copy Status is not Defined!"
                    Exit Sub
                End If

                If SqlConn.State = 0 Then
                    SqlConn.Open()
                End If
                thisTransaction = SqlConn.BeginTransaction()

                Dim intValue As Integer = 0
                Dim objCommand As New SqlCommand
                objCommand.Connection = SqlConn
                objCommand.Transaction = thisTransaction
                objCommand.CommandType = CommandType.Text
                objCommand.CommandText = "INSERT INTO CIRCULATION (MEM_ID, HOLD_ID, COPY_ID, RESERVE_DATE, RESERVE_TIME, REMARKS, STATUS, DATE_ADDED, USER_CODE, LIB_CODE, IP) " & _
                                         " VALUES (@MEM_ID, @HOLD_ID, @COPY_ID, @RESERVE_DATE, @RESERVE_TIME, @REMARKS, @STATUS, @DATE_ADDED, @USER_CODE, @LIB_CODE, @IP); " & _
                                         " SELECT SCOPE_IDENTITY();"

                objCommand.Parameters.Add("@MEM_ID", SqlDbType.Int)
                If MEM_ID = 0 Then
                    objCommand.Parameters("@MEM_ID").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@MEM_ID").Value = MEM_ID
                End If

                objCommand.Parameters.Add("@HOLD_ID", SqlDbType.Int)
                If HOLD_ID = 0 Then
                    objCommand.Parameters("@HOLD_ID").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@HOLD_ID").Value = HOLD_ID
                End If

                objCommand.Parameters.Add("@COPY_ID", SqlDbType.Int)
                If COPY_ID = 0 Then
                    objCommand.Parameters("@COPY_ID").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@COPY_ID").Value = COPY_ID
                End If

                objCommand.Parameters.Add("@RESERVE_DATE", SqlDbType.DateTime)
                If RESERVE_DATE = "" Then
                    objCommand.Parameters("@RESERVE_DATE").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@RESERVE_DATE").Value = RESERVE_DATE
                End If

                objCommand.Parameters.Add("@RESERVE_TIME", SqlDbType.DateTime)
                If RESERVE_TIME = "" Then
                    objCommand.Parameters("@RESERVE_TIME").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@RESERVE_TIME").Value = RESERVE_TIME
                End If

                objCommand.Parameters.Add("@REMARKS", SqlDbType.NVarChar)
                If REMARKS = "" Then
                    objCommand.Parameters("@REMARKS").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@REMARKS").Value = REMARKS
                End If

                objCommand.Parameters.Add("@STATUS", SqlDbType.VarChar)
                If STATUS = "" Then
                    objCommand.Parameters("@STATUS").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@STATUS").Value = STATUS
                End If

                objCommand.Parameters.Add("@DATE_ADDED", SqlDbType.DateTime)
                If DATE_ADDED = Nothing Then
                    objCommand.Parameters("@DATE_ADDED").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@DATE_ADDED").Value = DATE_ADDED
                End If

                objCommand.Parameters.Add("@IP", SqlDbType.VarChar)
                If IP = "" Then
                    objCommand.Parameters("@IP").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@IP").Value = IP
                End If

                objCommand.Parameters.Add("@USER_CODE", SqlDbType.NVarChar)
                If USER_CODE = "" Then
                    objCommand.Parameters("@USER_CODE").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@USER_CODE").Value = USER_CODE
                End If

                objCommand.Parameters.Add("@LIB_CODE", SqlDbType.NVarChar)
                If LibCode = "" Then
                    objCommand.Parameters("@LIB_CODE").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@LIB_CODE").Value = LibCode
                End If

                Dim dr As SqlDataReader
                dr = objCommand.ExecuteReader()
                If dr.Read Then
                    intValue = dr.GetValue(0)
                End If
                dr.Close()

                thisTransaction.Commit()
                SqlConn.Close()
                txt_Cir_AccessionNo.Text = ""
                txt_Cir_DueDate.Text = ""
                txt_Cir_AccessionNo_TextChanged(sender, e)
            End If
            PopulateGridIssuedBooks()
        Catch q As SqlException
            thisTransaction.Rollback()
            Lbl_Error.Text = "Error: " & (q.Message())
        Catch ex As Exception
            Lbl_Error.Text = "Error: " & (ex.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    '****************************************************************
    'RETURN
    '*******************************************************************
    'return books/bound jr
    Protected Sub RadioButton3_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles RadioButton3.CheckedChanged
        Ret_Cancel_Bttn_Click(sender, e)
        Label4.Text = "Accession No*"
    End Sub
    'return loose issues
    Protected Sub RadioButton4_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles RadioButton4.CheckedChanged
        Ret_Cancel_Bttn_Click(sender, e)
        Label4.Text = "Item ID*"
    End Sub
    'return book
    Protected Sub txt_Ret_AccessionNo_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txt_Ret_AccessionNo.TextChanged
        GetDocumentDetails()
        GetCirculationData()
        GetReturnMemberDetails()
        CalcualteFine()
    End Sub
    Public Sub GetDocumentDetails()
        Dim dt As New DataTable
        Try
            Lbl_Error2.Text = ""
            Dim iloop As Integer
            Dim strcurrentchar As Object
            Dim c As Integer
            Dim counter1 As Integer

            Dim ACCESSION_NO As Object = Nothing
            If Trim(txt_Ret_AccessionNo.Text) <> "" Then
                ACCESSION_NO = TrimX(txt_Ret_AccessionNo.Text)
                ACCESSION_NO = RemoveQuotes(ACCESSION_NO)
            Else
                Label5.Text = ""
                Label9.Text = ""
                Label17.Text = ""
                Label18.Text = ""
                Label19.Text = ""
                Label28.Text = ""
                txt_Ret_DueDays.Text = ""
                Image3.ImageUrl = Nothing
                DDL_BibLevel.ClearSelection()
                DDL_Materials.ClearSelection()
                DDL_Documents.ClearSelection()
                DDL_Collections.ClearSelection()
                DDL_CurrentStatus.ClearSelection()
                txt_Ret_AccessionNo.Focus()
                Exit Sub
            End If

            If RadioButton3.Checked = True Then ' for books
                Dim SQL As String = Nothing
                SQL = "SELECT HOLDINGS_CATS_AUTHORS_VIEW.*, CATS.PHOTO  FROM HOLDINGS_CATS_AUTHORS_VIEW INNER JOIN CATS ON HOLDINGS_CATS_AUTHORS_VIEW.CAT_NO = CATS.CAT_NO WHERE (HOLDINGS_CATS_AUTHORS_VIEW.LIB_CODE ='" & Trim(LibCode) & "'  AND HOLDINGS_CATS_AUTHORS_VIEW.ACCESSION_NO ='" & Trim(ACCESSION_NO) & "')"

                Dim ds As New DataSet
                Dim da As New SqlDataAdapter(SQL, SqlConn)
                If SqlConn.State = 0 Then
                    SqlConn.Open()
                End If
                da.Fill(ds)

                dt = ds.Tables(0).Copy

                Dim myDetails As String = Nothing
                If dt.Rows.Count <> 0 Then
                    If dt.Rows(0).Item("BIB_CODE").ToString <> "" Then
                        DDL_BibLevel.SelectedValue = dt.Rows(0).Item("BIB_CODE").ToString
                    Else
                        DDL_BibLevel.ClearSelection()
                    End If

                    If dt.Rows(0).Item("HOLD_ID").ToString <> "" Then
                        Label5.Text = dt.Rows(0).Item("HOLD_ID").ToString
                    Else
                        Label5.Text = ""
                    End If

                    If dt.Rows(0).Item("MAT_CODE").ToString <> "" Then
                        DDL_Materials.SelectedValue = dt.Rows(0).Item("MAT_CODE").ToString
                    Else
                        DDL_Materials.ClearSelection()
                    End If

                    If dt.Rows(0).Item("DOC_TYPE_CODE").ToString <> "" Then
                        DDL_Documents.SelectedValue = dt.Rows(0).Item("DOC_TYPE_CODE").ToString
                    Else
                        DDL_Documents.ClearSelection()
                    End If

                    If dt.Rows(0).Item("SUB_TITLE").ToString <> "" Then
                        myDetails = dt.Rows(0).Item("TITLE").ToString & ": " & dt.Rows(0).Item("SUB_TITLE").ToString
                    Else
                        myDetails = dt.Rows(0).Item("TITLE").ToString
                    End If

                    If dt.Rows(0).Item("AUTHOR1").ToString <> "" And dt.Rows(0).Item("AUTHOR2").ToString <> "" And dt.Rows(0).Item("AUTHOR3").ToString <> "" Then
                        myDetails = myDetails & " /By " & dt.Rows(0).Item("AUTHOR1").ToString & ";  " & dt.Rows(0).Item("AUTHOR2").ToString & " and " & dt.Rows(0).Item("AUTHOR3").ToString
                    End If
                    If dt.Rows(0).Item("AUTHOR1").ToString <> "" And dt.Rows(0).Item("AUTHOR2").ToString <> "" And dt.Rows(0).Item("AUTHOR3").ToString = "" Then
                        myDetails = myDetails & " /By " & dt.Rows(0).Item("AUTHOR1").ToString & " and " & dt.Rows(0).Item("AUTHOR2").ToString
                    End If
                    If dt.Rows(0).Item("AUTHOR1").ToString <> "" And dt.Rows(0).Item("AUTHOR2").ToString = "" And dt.Rows(0).Item("AUTHOR3").ToString = "" Then
                        myDetails = myDetails & " /By " & dt.Rows(0).Item("AUTHOR1").ToString
                    End If

                    If dt.Rows(0).Item("EDITOR").ToString <> "" Then
                        myDetails = myDetails & " /Edited By " & dt.Rows(0).Item("EDITOR").ToString
                    End If
                    If dt.Rows(0).Item("EDITION").ToString <> "" Then
                        Dim myEdition As Object = Nothing
                        myEdition = dt.Rows(0).Item("EDITION").ToString

                        If InStr(myEdition, "edition") = 0 Or InStr(myEdition, "ed") = 0 Or InStr(myEdition, "ed.") = 0 Or InStr(myEdition, "edition.") = 0 Then
                            myDetails = myDetails & ". " & dt.Rows(0).Item("EDITION").ToString & " ed. "
                        Else
                            myDetails = myDetails & " . " & dt.Rows(0).Item("EDITION").ToString
                        End If
                    End If
                    If dt.Rows(0).Item("PUB_PLACE").ToString <> "" Then
                        myDetails = myDetails & ", " & dt.Rows(0).Item("PUB_PLACE").ToString
                    End If
                    If dt.Rows(0).Item("PUB_NAME").ToString <> "" Then
                        myDetails = myDetails & ", " & dt.Rows(0).Item("PUB_NAME").ToString
                    End If
                    If dt.Rows(0).Item("YEAR_OF_PUB").ToString <> "" Then
                        myDetails = myDetails & ", " & dt.Rows(0).Item("YEAR_OF_PUB").ToString
                    End If
                    If dt.Rows(0).Item("VOL_NO").ToString <> "" Then
                        myDetails = myDetails & ". Vol No.: " & dt.Rows(0).Item("VOL_NO").ToString
                    End If
                    If dt.Rows(0).Item("PAGINATION").ToString <> "" Then
                        myDetails = myDetails & ", " & dt.Rows(0).Item("PAGINATION").ToString
                    End If

                    Label9.Text = myDetails
                    If dt.Rows(0).Item("STANDARD_NO").ToString <> "" Then
                        Label17.Text = dt.Rows(0).Item("STANDARD_NO").ToString
                    Else
                        Label17.Text = ""
                    End If

                    If dt.Rows(0).Item("LANG_CODE").ToString <> "" Then
                        Label18.Text = "Language: " & dt.Rows(0).Item("LANG_CODE").ToString
                    Else
                        Label18.Text = ""
                    End If

                    If dt.Rows(0).Item("SP_NO").ToString <> "" Then
                        Label19.Text = "Standard No: " & dt.Rows(0).Item("SP_NO").ToString
                    Else
                        Label19.Text = ""
                    End If

                    If dt.Rows(0).Item("PHYSICAL_LOCATION").ToString <> "" Then
                        Label28.Text = " -Location: " & dt.Rows(0).Item("PHYSICAL_LOCATION").ToString
                    Else
                        Label28.Text = ""
                    End If

                    If dt.Rows(0).Item("REPORT_NO").ToString <> "" Then
                        Label19.Text = "; Report No: " & dt.Rows(0).Item("REPORT_NO").ToString
                    ElseIf dt.Rows(0).Item("MANUAL_NO").ToString <> "" Then
                        Label19.Text = "; Manual No: " & dt.Rows(0).Item("MANUAL_NO").ToString
                    ElseIf dt.Rows(0).Item("PATENT_NO").ToString <> "" Then
                        Label19.Text = "; Patent No: " & dt.Rows(0).Item("PATENT_NO").ToString
                    Else
                        Label19.Text = ""
                    End If

                    Image3.ImageUrl = Nothing
                    Dim strURL As String = "~/Acquisition/Cats_GetPhoto.aspx?CAT_NO=" & dt.Rows(0).Item("CAT_NO").ToString
                    Image3.ImageUrl = strURL
                    Image3.Visible = True

                    If dt.Rows(0).Item("COLLECTION_TYPE").ToString <> "" Then
                        DDL_Collections.SelectedValue = dt.Rows(0).Item("COLLECTION_TYPE").ToString
                    Else
                        DDL_Collections.ClearSelection()
                    End If

                    If dt.Rows(0).Item("STA_CODE").ToString <> "" Then
                        DDL_CurrentStatus.SelectedValue = dt.Rows(0).Item("STA_CODE").ToString
                    Else
                        DDL_CurrentStatus.ClearSelection()
                    End If
                Else
                    Label5.Text = ""
                    Label9.Text = ""
                    Label17.Text = ""
                    Label18.Text = ""
                    Label19.Text = ""
                    Label28.Text = ""
                    txt_Ret_DueDays.Text = ""
                    Image3.ImageUrl = Nothing
                    DDL_BibLevel.ClearSelection()
                    DDL_Materials.ClearSelection()
                    DDL_Documents.ClearSelection()
                    DDL_Collections.ClearSelection()
                    DDL_CurrentStatus.ClearSelection()
                    txt_Ret_AccessionNo.Focus()
                End If
            End If
            If RadioButton4.Checked = True Then ' for losse issues
                Dim SQL As String = Nothing
                SQL = "SELECT * FROM CATS_LOOSE_ISSUES_COPIES_VIEW WHERE (LOOSE_ISSUE_LIB_CODE ='" & Trim(LibCode) & "'  AND COPY_ID ='" & Trim(ACCESSION_NO) & "')"

                Dim ds As New DataSet
                Dim da As New SqlDataAdapter(SQL, SqlConn)
                If SqlConn.State = 0 Then
                    SqlConn.Open()
                End If
                da.Fill(ds)

                dt = ds.Tables(0).Copy

                Dim myDetails As String = Nothing
                If dt.Rows.Count <> 0 Then
                    If dt.Rows(0).Item("BIB_CODE").ToString <> "" Then
                        DDL_BibLevel.SelectedValue = dt.Rows(0).Item("BIB_CODE").ToString
                    Else
                        DDL_BibLevel.ClearSelection()
                    End If

                    If dt.Rows(0).Item("COPY_ID").ToString <> "" Then
                        Label5.Text = dt.Rows(0).Item("COPY_ID").ToString
                    Else
                        Label5.Text = ""
                    End If

                    If dt.Rows(0).Item("MAT_CODE").ToString <> "" Then
                        DDL_Materials.SelectedValue = dt.Rows(0).Item("MAT_CODE").ToString
                    Else
                        DDL_Materials.ClearSelection()
                    End If

                    If dt.Rows(0).Item("DOC_TYPE_CODE").ToString <> "" Then
                        DDL_Documents.SelectedValue = dt.Rows(0).Item("DOC_TYPE_CODE").ToString
                    Else
                        DDL_Documents.ClearSelection()
                    End If

                    If dt.Rows(0).Item("SUB_TITLE").ToString <> "" Then
                        myDetails = dt.Rows(0).Item("TITLE").ToString & ": " & dt.Rows(0).Item("SUB_TITLE").ToString
                    Else
                        myDetails = dt.Rows(0).Item("TITLE").ToString
                    End If

                    If dt.Rows(0).Item("EDITOR").ToString <> "" Then
                        myDetails = myDetails & " /Edited By " & dt.Rows(0).Item("EDITOR").ToString
                    End If

                    If dt.Rows(0).Item("PUB_PLACE").ToString <> "" Then
                        myDetails = myDetails & ", " & dt.Rows(0).Item("PUB_PLACE").ToString
                    End If
                    If dt.Rows(0).Item("PUB_NAME").ToString <> "" Then
                        myDetails = myDetails & ", " & dt.Rows(0).Item("PUB_NAME").ToString
                    End If

                    If dt.Rows(0).Item("VOL_NO").ToString <> "" Then
                        myDetails = myDetails & ". Vol No.: " & dt.Rows(0).Item("VOL_NO").ToString
                    End If
                    If dt.Rows(0).Item("ISSUE_NO").ToString <> "" Then
                        myDetails = myDetails & " (" & dt.Rows(0).Item("ISSUE_NO").ToString & ")"
                    End If
                    If dt.Rows(0).Item("PART_NO").ToString <> "" Then
                        myDetails = myDetails & ", Part NO:" & dt.Rows(0).Item("PART_NO").ToString
                    End If

                    If dt.Rows(0).Item("ISS_DATE").ToString <> "" Then
                        myDetails = myDetails & ", Issue Date:" & Format(dt.Rows(0).Item("ISS_DATE"), "dd/MM/yyyy")
                    End If

                    Label9.Text = myDetails
                    If dt.Rows(0).Item("STANDARD_NO").ToString <> "" Then
                        Label17.Text = dt.Rows(0).Item("STANDARD_NO").ToString
                    Else
                        Label17.Text = ""
                    End If

                    If dt.Rows(0).Item("LANG_CODE").ToString <> "" Then
                        Label18.Text = "Language: " & dt.Rows(0).Item("LANG_CODE").ToString
                    Else
                        Label18.Text = ""
                    End If

                    Label19.Text = ""
                    Label28.Text = ""

                    Image3.ImageUrl = Nothing
                    Dim strURL As String = "~/Acquisition/Cats_GetPhoto.aspx?CAT_NO=" & dt.Rows(0).Item("CAT_NO").ToString
                    Image3.ImageUrl = strURL
                    Image3.Visible = True

                    If dt.Rows(0).Item("COLLECTION_TYPE").ToString <> "" Then
                        DDL_Collections.SelectedValue = dt.Rows(0).Item("COLLECTION_TYPE").ToString
                    Else
                        DDL_Collections.ClearSelection()
                    End If

                    If dt.Rows(0).Item("STA_CODE").ToString <> "" Then
                        DDL_CurrentStatus.SelectedValue = dt.Rows(0).Item("STA_CODE").ToString
                    Else
                        DDL_CurrentStatus.ClearSelection()
                    End If
                Else
                    Label5.Text = ""
                    Label9.Text = ""
                    Label17.Text = ""
                    Label18.Text = ""
                    Label19.Text = ""
                    Label28.Text = ""
                    txt_Ret_DueDays.Text = ""
                    Image3.ImageUrl = Nothing
                    DDL_BibLevel.ClearSelection()
                    DDL_Materials.ClearSelection()
                    DDL_Documents.ClearSelection()
                    DDL_Collections.ClearSelection()
                    DDL_CurrentStatus.ClearSelection()
                    txt_Ret_AccessionNo.Focus()
                End If
            End If


            SqlConn.Close()
        Catch ex As Exception
            Lbl_Error2.Text = "Error: " & (ex.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'get circulation details of documents
    Public Sub GetCirculationData()
        Dim dt As New DataTable
        Try
            Dim iloop As Integer
            Dim strcurrentchar As Object
            Dim c As Integer
            Dim counter1 As Integer

            If RadioButton3.Checked = True Then ' for books
                Dim HOLD_ID As Integer = Nothing
                If Trim(Label5.Text) <> "" Then
                    HOLD_ID = TrimX(Label5.Text)
                    HOLD_ID = RemoveQuotes(HOLD_ID)
                Else
                    Label26.Text = ""
                    Label27.Text = ""
                    txt_Ret_IssueDate.Text = ""
                    txt_Ret_IssueTime.Text = ""
                    txt_Ret_DueDate.Text = ""
                    txt_Ret_Remarks.Text = ""
                    txt_Ret_ReturnDate.Text = ""
                    txt_Ret_RenewDate.Text = ""
                    txt_Ret_ReturnTime.Text = ""
                    txt_Ret_AccessionNo.Focus()
                    Exit Sub
                End If

                Dim SQL As String = Nothing
                SQL = "SELECT * FROM CIRCULATION WHERE (LIB_CODE ='" & Trim(LibCode) & "'  AND HOLD_ID ='" & Trim(HOLD_ID) & "' AND STATUS = 'Issued')"

                Dim ds As New DataSet
                Dim da As New SqlDataAdapter(SQL, SqlConn)
                If SqlConn.State = 0 Then
                    SqlConn.Open()
                End If
                da.Fill(ds)

                dt = ds.Tables(0).Copy

                Dim myDetails As String = Nothing
                If dt.Rows.Count <> 0 Then
                    If dt.Rows(0).Item("CIR_ID").ToString <> "" Then
                        Label27.Text = dt.Rows(0).Item("CIR_ID").ToString
                    Else
                        Label27.Text = ""
                    End If

                    If dt.Rows(0).Item("ISSUE_DATE").ToString <> "" Then
                        txt_Ret_IssueDate.Text = Format(dt.Rows(0).Item("ISSUE_DATE"), "dd/MM/yyyy")
                    Else
                        txt_Ret_IssueDate.Text = ""
                    End If

                    If dt.Rows(0).Item("ISSUE_TIME").ToString <> "" Then
                        txt_Ret_IssueTime.Text = Format(dt.Rows(0).Item("ISSUE_TIME"), "HH:MM:ss")
                    Else
                        txt_Ret_IssueTime.Text = ""
                    End If

                    If dt.Rows(0).Item("DUE_DATE").ToString <> "" Then
                        txt_Ret_DueDate.Text = Format(dt.Rows(0).Item("DUE_DATE"), "dd/MM/yyyy")
                    Else
                        txt_Ret_DueDate.Text = ""
                    End If

                    If dt.Rows(0).Item("REMARKS").ToString <> "" Then
                        txt_Ret_Remarks.Text = dt.Rows(0).Item("REMARKS").ToString
                    Else
                        txt_Ret_Remarks.Text = ""
                    End If
                    If dt.Rows(0).Item("MEM_ID").ToString <> "" Then
                        Label26.Text = dt.Rows(0).Item("MEM_ID").ToString
                    Else
                        Label26.Text = ""
                    End If

                    Dim TODAY_DATE As Date = Nothing
                    TODAY_DATE = Now.Date
                    TODAY_DATE = Convert.ToDateTime(TODAY_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert to indian 

                    txt_Ret_ReturnDate.Text = Format(TODAY_DATE, "dd/MM/yyyy")
                    txt_Ret_RenewDate.Text = Format(TODAY_DATE, "dd/MM/yyyy")
                   
                    Dim TIME_NOW As DateTime = Nothing
                    TIME_NOW = Now.TimeOfDay.ToString

                    txt_Ret_ReturnTime.Text = TIME_NOW
                Else
                    Label26.Text = ""
                    Label27.Text = ""
                    txt_Ret_IssueDate.Text = ""
                    txt_Ret_IssueTime.Text = ""
                    txt_Ret_DueDate.Text = ""
                    txt_Ret_Remarks.Text = ""
                    txt_Ret_ReturnDate.Text = ""
                    txt_Ret_RenewDate.Text = ""
                    txt_Ret_ReturnTime.Text = ""
                    Lbl_Error2.Text = "The Document is not Issued"
                End If
            End If

            If RadioButton4.Checked = True Then ' for loose issues
                Dim COPY_ID As Integer = Nothing
                If Trim(Label5.Text) <> "" Then
                    COPY_ID = TrimX(Label5.Text)
                    COPY_ID = RemoveQuotes(COPY_ID)
                Else
                    Label26.Text = ""
                    Label27.Text = ""
                    txt_Ret_IssueDate.Text = ""
                    txt_Ret_IssueTime.Text = ""
                    txt_Ret_DueDate.Text = ""
                    txt_Ret_Remarks.Text = ""
                    txt_Ret_ReturnDate.Text = ""
                    txt_Ret_RenewDate.Text = ""
                    txt_Ret_ReturnTime.Text = ""
                    txt_Ret_AccessionNo.Focus()
                    Exit Sub
                End If

                Dim SQL As String = Nothing
                SQL = "SELECT * FROM CIRCULATION WHERE (LIB_CODE ='" & Trim(LibCode) & "'  AND COPY_ID ='" & Trim(COPY_ID) & "' AND STATUS = 'Issued')"

                Dim ds As New DataSet
                Dim da As New SqlDataAdapter(SQL, SqlConn)
                If SqlConn.State = 0 Then
                    SqlConn.Open()
                End If
                da.Fill(ds)

                dt = ds.Tables(0).Copy

                Dim myDetails As String = Nothing
                If dt.Rows.Count <> 0 Then
                    If dt.Rows(0).Item("CIR_ID").ToString <> "" Then
                        Label27.Text = dt.Rows(0).Item("CIR_ID").ToString
                    Else
                        Label27.Text = ""
                    End If

                    If dt.Rows(0).Item("ISSUE_DATE").ToString <> "" Then
                        txt_Ret_IssueDate.Text = Format(dt.Rows(0).Item("ISSUE_DATE"), "dd/MM/yyyy")
                    Else
                        txt_Ret_IssueDate.Text = ""
                    End If

                    If dt.Rows(0).Item("ISSUE_TIME").ToString <> "" Then
                        txt_Ret_IssueTime.Text = dt.Rows(0).Item("ISSUE_TIME").ToString
                    Else
                        txt_Ret_IssueTime.Text = ""
                    End If

                    If dt.Rows(0).Item("DUE_DATE").ToString <> "" Then
                        txt_Ret_DueDate.Text = Format(dt.Rows(0).Item("DUE_DATE"), "dd/MM/yyyy")
                    Else
                        txt_Ret_DueDate.Text = ""
                    End If

                    If dt.Rows(0).Item("REMARKS").ToString <> "" Then
                        txt_Ret_Remarks.Text = dt.Rows(0).Item("REMARKS").ToString
                    Else
                        txt_Ret_Remarks.Text = ""
                    End If
                    If dt.Rows(0).Item("MEM_ID").ToString <> "" Then
                        Label26.Text = dt.Rows(0).Item("MEM_ID").ToString
                    Else
                        Label26.Text = ""
                    End If

                    Dim TODAY_DATE As Date = Nothing
                    TODAY_DATE = Now.Date
                    TODAY_DATE = Convert.ToDateTime(TODAY_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert to indian 

                    txt_Ret_ReturnDate.Text = Format(TODAY_DATE, "dd/MM/yyyy")
                    txt_Ret_RenewDate.Text = Format(TODAY_DATE, "dd/MM/yyyy")

                    Dim TIME_NOW As DateTime = Nothing
                    TIME_NOW = Now.TimeOfDay.ToString

                    txt_Ret_ReturnTime.Text = TIME_NOW
                Else
                    Label26.Text = ""
                    Label27.Text = ""
                    txt_Ret_IssueDate.Text = ""
                    txt_Ret_IssueTime.Text = ""
                    txt_Ret_DueDate.Text = ""
                    txt_Ret_Remarks.Text = ""
                    txt_Ret_ReturnDate.Text = ""
                    txt_Ret_RenewDate.Text = ""
                    txt_Ret_ReturnTime.Text = ""
                    Lbl_Error2.Text = "The Document is not Issued"
                End If
            End If

            SqlConn.Close()

        Catch ex As Exception
            Lbl_Error2.Text = "Error: " & (ex.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'get member details
    Public Sub GetReturnMemberDetails()
        Dim dt As New DataTable
        Try
            Dim iloop As Integer
            Dim strcurrentchar As Object
            Dim c As Integer
            Dim counter1 As Integer

            Dim MEM_ID As Object = Nothing
            If Trim(Label26.Text) <> "" Then
                MEM_ID = TrimX(Label26.Text)
                MEM_ID = RemoveQuotes(MEM_ID)
            Else
                Label26.Text = ""
                Image4.ImageUrl = Nothing
                txt_Ret_MemNo.Text = ""
                txt_Ret_MemName.Text = ""
                DDL_MemberCategories.ClearSelection()
                DDL_MemberSubCategories.ClearSelection()
                DDL_MemberStatus.ClearSelection()
                DDL_FineSystem.ClearSelection()
                txt_Ret_Mobile.Text = ""
                txt_Ret_MemEmail.Text = ""
                txt_Ret_AdmDate.Text = ""
                txt_Ret_ClosingDate.Text = ""
                txt_Ret_DueDays.Text = ""
                txt_Ret_Gap1.Text = ""
                txt_Ret_Fine1.Text = ""
                txt_Ret_Fine2.Text = ""
                txt_Ret_AccessionNo.Focus()
                Exit Sub
            End If

            Dim SQL As String = Nothing
            SQL = "SELECT MEMBERSHIPS.MEM_ID, MEMBERSHIPS.MEM_NO, MEMBERSHIPS.MEM_NAME, MEMBERSHIPS.CAT_ID, CATEGORIES.CAT_NAME,  MEMBERSHIPS.SUBCAT_ID, SUB_CATEGORIES.SUBCAT_NAME, SUB_CATEGORIES.FINE_SYSTEM, SUB_CATEGORIES.DUEDAYS_LOOSE, SUB_CATEGORIES.DUEDAYS_BOOKS, SUB_CATEGORIES.DUEDAYS_BOUNDJ, SUB_CATEGORIES.DUEDAYS_MANUALS,  SUB_CATEGORIES.DUEDAYS_PATENTS, SUB_CATEGORIES.DUEDAYS_REPORTS, SUB_CATEGORIES.DUEDAYS_STANDARDS,  SUB_CATEGORIES.DUEDAYS_AV, SUB_CATEGORIES.DUEDAYS_CARTOGRAPHIC, SUB_CATEGORIES.DUEDAYS_MANUSCRIPTS, SUB_CATEGORIES.DUEDAYS_BBGENERAL, SUB_CATEGORIES.DUEDAYS_BBRESERVE, SUB_CATEGORIES.DUEDAYS_NONRETURNABLE, SUB_CATEGORIES.FINE1_GAP, SUB_CATEGORIES.FINE1_LOOSE, SUB_CATEGORIES.FINE1_BOOKS,SUB_CATEGORIES.FINE1_BOUNDJ, SUB_CATEGORIES.FINE1_MANUALS, SUB_CATEGORIES.FINE1_PATENTS, SUB_CATEGORIES.FINE1_REPORTS, SUB_CATEGORIES.FINE1_STANDARDS, SUB_CATEGORIES.FINE1_AV, SUB_CATEGORIES.FINE1_CARTOGRAPHIC, SUB_CATEGORIES.FINE1_MANUSCRIPTS, SUB_CATEGORIES.FINE1_BBGENERAL, SUB_CATEGORIES.FINE1_BBRESERVE, SUB_CATEGORIES.FINE2_LOOSE, SUB_CATEGORIES.FINE2_BOOKS,SUB_CATEGORIES.FINE2_BOUNDJ, SUB_CATEGORIES.FINE2_MANUALS, SUB_CATEGORIES.FINE2_PATENTS, SUB_CATEGORIES.FINE2_REPORTS, SUB_CATEGORIES.FINE2_STANDARDS, SUB_CATEGORIES.FINE2_AV, SUB_CATEGORIES.FINE2_CARTOGRAPHIC, SUB_CATEGORIES.FINE2_MANUSCRIPTS, SUB_CATEGORIES.FINE2_BBGENERAL, SUB_CATEGORIES.FINE2_BBRESERVE, MEMBERSHIPS.MEM_EMAIL,  MEMBERSHIPS.MEM_TELEPHONE, MEMBERSHIPS.MEM_MOBILE, MEMBERSHIPS.MEM_OVERRIDE, MEMBERSHIPS.MEM_ADM_DATE, MEMBERSHIPS.MEM_CLOSE_DATE, MEMBERSHIPS.MEM_REMARKS, MEMBERSHIPS.MEM_STATUS, MEMBERSHIPS.TYPE, MEMBERSHIPS.LIB_CODE, MEMBERSHIPS.PHOTO FROM         MEMBERSHIPS LEFT OUTER JOIN SUB_CATEGORIES ON MEMBERSHIPS.SUBCAT_ID = SUB_CATEGORIES.SUBCAT_ID LEFT OUTER JOIN CATEGORIES ON MEMBERSHIPS.CAT_ID = CATEGORIES.CAT_ID WHERE (MEMBERSHIPS.LIB_CODE ='" & Trim(LibCode) & "'  AND MEMBERSHIPS.MEM_ID ='" & Trim(MEM_ID) & "' AND MEMBERSHIPS.MEM_STATUS='CU')"

            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            da.Fill(ds)

            dt = ds.Tables(0).Copy

            If dt.Rows.Count <> 0 Then
                If dt.Rows(0).Item("MEM_NO").ToString <> "" Then
                    txt_Ret_MemNo.Text = dt.Rows(0).Item("MEM_NO").ToString
                Else
                    txt_Ret_MemNo.Text = ""
                End If

                If dt.Rows(0).Item("MEM_NAME").ToString <> "" Then
                    txt_Ret_MemName.Text = dt.Rows(0).Item("MEM_NAME").ToString
                Else
                    txt_Ret_MemName.Text = ""
                End If

                If dt.Rows(0).Item("CAT_ID").ToString <> "" Then
                    DDL_MemberCategories.SelectedValue = dt.Rows(0).Item("CAT_ID").ToString
                Else
                    DDL_MemberCategories.ClearSelection()
                End If

                If dt.Rows(0).Item("SUBCAT_ID").ToString <> "" Then
                    DDL_MemberSubCategories.SelectedValue = dt.Rows(0).Item("SUBCAT_ID").ToString
                Else
                    DDL_MemberSubCategories.ClearSelection()
                End If

                If dt.Rows(0).Item("MEM_STATUS").ToString <> "" Then
                    DDL_MemberStatus.SelectedValue = dt.Rows(0).Item("MEM_STATUS").ToString
                Else
                    DDL_MemberStatus.ClearSelection()
                End If

                If dt.Rows(0).Item("FINE_SYSTEM").ToString <> "" Then
                    DDL_FineSystem.SelectedValue = dt.Rows(0).Item("FINE_SYSTEM").ToString
                Else
                    DDL_FineSystem.ClearSelection()
                End If

                If dt.Rows(0).Item("MEM_MOBILE").ToString <> "" Then
                    txt_ret_Mobile.Text = dt.Rows(0).Item("MEM_MOBILE").ToString
                Else
                    txt_Ret_Mobile.Text = ""
                End If

                If dt.Rows(0).Item("MEM_EMAIL").ToString <> "" Then
                    txt_ret_MemEmail.Text = dt.Rows(0).Item("MEM_EMAIL").ToString
                Else
                    txt_Ret_MemEmail.Text = ""
                End If

                If dt.Rows(0).Item("MEM_ADM_DATE").ToString <> "" Then
                    txt_Ret_AdmDate.Text = Format(dt.Rows(0).Item("MEM_ADM_DATE"), "dd/MM/yyyy")
                Else
                    txt_Ret_AdmDate.Text = ""
                End If

                If dt.Rows(0).Item("MEM_CLOSE_DATE").ToString <> "" Then
                    txt_Ret_ClosingDate.Text = Format(dt.Rows(0).Item("MEM_CLOSE_DATE"), "dd/MM/yyyy")
                Else
                    txt_Ret_ClosingDate.Text = ""
                End If

                If dt.Rows(0).Item("FINE1_GAP").ToString <> "" Then
                    txt_Ret_Gap1.Text = dt.Rows(0).Item("MEM_CLOSE_DATE").ToString
                Else
                    txt_Ret_Gap1.Text = ""
                End If

                If dt.Rows(0).Item("PHOTO").ToString <> "" Then
                    Dim strURL As String = "~/Circulation/Member_GetPhoto.aspx?MEM_ID=" & Trim(dt.Rows(0).Item("MEM_ID").ToString) & ""
                    Image4.ImageUrl = strURL
                    Image4.Visible = True
                Else
                    Image4.Visible = False
                End If
                'Entitlement/due days
                If DDL_Materials.SelectedItem.ToString <> "" Then
                    If DDL_Collections.SelectedValue = "C" Then
                        'fine1
                        If DDL_Materials.SelectedItem.ToString = "Books" Then
                            If dt.Rows(0).Item("FINE1_BOOKS").ToString <> "" Then
                                txt_Ret_Fine1.Text = dt.Rows(0).Item("FINE1_BOOKS").ToString
                            Else
                                txt_Ret_Fine1.Text = ""
                            End If
                        End If
                        If DDL_Materials.SelectedItem.ToString = "Manuals" Then
                            If dt.Rows(0).Item("FINE1_MANUALS").ToString <> "" Then
                                txt_Ret_Fine1.Text = dt.Rows(0).Item("FINE1_MANUALS").ToString
                            Else
                                txt_Ret_Fine1.Text = ""
                            End If
                        End If
                        If DDL_Materials.SelectedItem.ToString = "Patents" Then
                            If dt.Rows(0).Item("FINE1_PATENTS").ToString <> "" Then
                                txt_Ret_Fine1.Text = dt.Rows(0).Item("FINE1_PATENTS").ToString
                            Else
                                txt_Ret_Fine1.Text = ""
                            End If
                        End If
                        If DDL_Materials.SelectedItem.ToString = "Reports" Then
                            If dt.Rows(0).Item("FINE1_REPORTS").ToString <> "" Then
                                txt_Ret_Fine1.Text = dt.Rows(0).Item("FINE1_REPORTS").ToString
                            Else
                                txt_Ret_Fine1.Text = ""
                            End If
                        End If
                        If DDL_Materials.SelectedItem.ToString = "Standards and Specifications" Then
                            If dt.Rows(0).Item("FINE1_STANDARDS").ToString <> "" Then
                                txt_Ret_Fine1.Text = dt.Rows(0).Item("FINE1_STANDARDS").ToString
                            Else
                                txt_Ret_Fine1.Text = ""
                            End If
                        End If

                        If RadioButton3.Checked = True Then
                            If DDL_Materials.SelectedItem.ToString = "Annuals" Or DDL_Materials.SelectedItem.ToString = "Newspapers" Or DDL_Materials.SelectedItem.ToString = "Periodicals" Then
                                If dt.Rows(0).Item("FINE1_BOUNDJ").ToString <> "" Then
                                    txt_Ret_Fine1.Text = dt.Rows(0).Item("FINE1_BOUNDJ").ToString
                                Else
                                    txt_Ret_Fine1.Text = ""
                                End If
                            End If
                        End If
                        If RadioButton4.Checked = True Then
                            If dt.Rows(0).Item("FINE1_LOOSE").ToString <> "" Then
                                txt_Ret_Fine1.Text = dt.Rows(0).Item("FINE1_LOOSE").ToString
                            Else
                                txt_Ret_Fine1.Text = ""
                            End If
                        End If


                        If DDL_Materials.SelectedItem.ToString = "Cartographic" Then
                            If dt.Rows(0).Item("FINE1_CARTOGRAPHIC").ToString <> "" Then
                                txt_Ret_Fine1.Text = dt.Rows(0).Item("FINE1_CARTOGRAPHIC").ToString
                            Else
                                txt_Ret_Fine1.Text = ""
                            End If
                        End If
                        If DDL_Materials.SelectedItem.ToString = "AV Materials" Then
                            If dt.Rows(0).Item("FINE1_AV").ToString <> "" Then
                                txt_Ret_Fine1.Text = dt.Rows(0).Item("FINE1_AV").ToString
                            Else
                                txt_Ret_Fine1.Text = ""
                            End If
                        End If
                        If DDL_Materials.SelectedItem.ToString = "Manuscripts" Then
                            If dt.Rows(0).Item("FINE1_MANUSCRIPTS").ToString <> "" Then
                                txt_Ret_Fine1.Text = dt.Rows(0).Item("FINE1_MANUSCRIPTS").ToString
                            Else
                                txt_Ret_Fine1.Text = ""
                            End If
                        End If

                        'fine2
                        If DDL_Materials.SelectedItem.ToString = "Books" Then
                            If dt.Rows(0).Item("FINE2_BOOKS").ToString <> "" Then
                                txt_Ret_Fine2.Text = dt.Rows(0).Item("FINE2_BOOKS").ToString
                            Else
                                txt_Ret_Fine2.Text = ""
                            End If
                        End If
                        If DDL_Materials.SelectedItem.ToString = "Manuals" Then
                            If dt.Rows(0).Item("FINE2_MANUALS").ToString <> "" Then
                                txt_Ret_Fine2.Text = dt.Rows(0).Item("FINE2_MANUALS").ToString
                            Else
                                txt_Ret_Fine2.Text = ""
                            End If
                        End If
                        If DDL_Materials.SelectedItem.ToString = "Patents" Then
                            If dt.Rows(0).Item("FINE2_PATENTS").ToString <> "" Then
                                txt_Ret_Fine2.Text = dt.Rows(0).Item("FINE2_PATENTS").ToString
                            Else
                                txt_Ret_Fine2.Text = ""
                            End If
                        End If
                        If DDL_Materials.SelectedItem.ToString = "Reports" Then
                            If dt.Rows(0).Item("FINE2_REPORTS").ToString <> "" Then
                                txt_Ret_Fine2.Text = dt.Rows(0).Item("FINE2_REPORTS").ToString
                            Else
                                txt_Ret_Fine2.Text = ""
                            End If
                        End If
                        If DDL_Materials.SelectedItem.ToString = "Standards and Specifications" Then
                            If dt.Rows(0).Item("FINE2_STANDARDS").ToString <> "" Then
                                txt_Ret_Fine2.Text = dt.Rows(0).Item("FINE2_STANDARDS").ToString
                            Else
                                txt_Ret_Fine2.Text = ""
                            End If
                        End If

                        If RadioButton3.Checked = True Then
                            If DDL_Materials.SelectedItem.ToString = "Annuals" Or DDL_Materials.SelectedItem.ToString = "Newspapers" Or DDL_Materials.SelectedItem.ToString = "Periodicals" Then
                                If dt.Rows(0).Item("FINE2_BOUNDJ").ToString <> "" Then
                                    txt_Ret_Fine2.Text = dt.Rows(0).Item("FINE2_BOUNDJ").ToString
                                Else
                                    txt_Ret_Fine2.Text = ""
                                End If
                            End If
                        End If

                        If RadioButton4.Checked = True Then
                            If dt.Rows(0).Item("FINE2_LOOSE").ToString <> "" Then
                                txt_Ret_Fine2.Text = dt.Rows(0).Item("FINE2_LOOSE").ToString
                            Else
                                txt_Ret_Fine2.Text = ""
                            End If
                        End If

                        If DDL_Materials.SelectedItem.ToString = "Cartographic" Then
                            If dt.Rows(0).Item("FINE2_CARTOGRAPHIC").ToString <> "" Then
                                txt_Ret_Fine2.Text = dt.Rows(0).Item("FINE2_CARTOGRAPHIC").ToString
                            Else
                                txt_Ret_Fine2.Text = ""
                            End If
                        End If
                        If DDL_Materials.SelectedItem.ToString = "AV Materials" Then
                            If dt.Rows(0).Item("FINE2_AV").ToString <> "" Then
                                txt_Ret_Fine2.Text = dt.Rows(0).Item("FINE2_AV").ToString
                            Else
                                txt_Ret_Fine2.Text = ""
                            End If
                        End If
                        If DDL_Materials.SelectedItem.ToString = "Manuscripts" Then
                            If dt.Rows(0).Item("FINE2_MANUSCRIPTS").ToString <> "" Then
                                txt_Ret_Fine2.Text = dt.Rows(0).Item("FINE2_MANUSCRIPTS").ToString
                            Else
                                txt_Ret_Fine2.Text = ""
                            End If
                        End If

                        'due days
                        If DDL_Materials.SelectedItem.ToString = "Books" Then
                            If dt.Rows(0).Item("DUEDAYS_BOOKS").ToString <> "" Then
                                txt_Ret_DueDays.Text = dt.Rows(0).Item("DUEDAYS_BOOKS").ToString
                            Else
                                txt_Ret_DueDays.Text = ""
                            End If
                        End If
                        If DDL_Materials.SelectedItem.ToString = "Manuals" Then
                            If dt.Rows(0).Item("DUEDAYS_MANUALS").ToString <> "" Then
                                txt_Ret_DueDays.Text = dt.Rows(0).Item("DUEDAYS_MANUALS").ToString
                            Else
                                txt_Ret_DueDays.Text = ""
                            End If
                        End If
                        If DDL_Materials.SelectedItem.ToString = "Patents" Then
                            If dt.Rows(0).Item("DUEDAYS_PATENTS").ToString <> "" Then
                                txt_Ret_DueDays.Text = dt.Rows(0).Item("DUEDAYS_PATENTS").ToString
                            Else
                                txt_Ret_DueDays.Text = ""
                            End If
                        End If
                        If DDL_Materials.SelectedItem.ToString = "Reports" Then
                            If dt.Rows(0).Item("DUEDAYS_REPORTS").ToString <> "" Then
                                txt_Ret_DueDays.Text = dt.Rows(0).Item("DUEDAYS_REPORTS").ToString
                            Else
                                txt_Ret_DueDays.Text = ""
                            End If
                        End If
                        If DDL_Materials.SelectedItem.ToString = "Standards and Specifications" Then
                            If dt.Rows(0).Item("DUEDAYS_STANDARDS").ToString <> "" Then
                                txt_Ret_DueDays.Text = dt.Rows(0).Item("DUEDAYS_STANDARDS").ToString
                            Else
                                txt_Ret_DueDays.Text = ""
                            End If
                        End If

                        If RadioButton3.Checked = True Then 'books
                            If DDL_Materials.SelectedItem.ToString = "Annuals" Or DDL_Materials.SelectedItem.ToString = "Newspapers" Or DDL_Materials.SelectedItem.ToString = "Periodicals" Then
                                If dt.Rows(0).Item("DUEDAYS_BOUNDJ").ToString <> "" Then
                                    txt_Ret_DueDays.Text = dt.Rows(0).Item("DUEDAYS_BOUNDJ").ToString
                                Else
                                    txt_Ret_DueDays.Text = ""
                                End If
                            End If
                        End If

                        If RadioButton4.Checked = True Then 'loose
                            If dt.Rows(0).Item("DUEDAYS_LOOSE").ToString <> "" Then
                                txt_Ret_DueDays.Text = dt.Rows(0).Item("DUEDAYS_LOOSE").ToString
                            Else
                                txt_Ret_DueDays.Text = ""
                            End If
                        End If

                        If DDL_Materials.SelectedItem.ToString = "Cartographic" Then
                            If dt.Rows(0).Item("DUEDAYS_CARTOGRAPHIC").ToString <> "" Then
                                txt_Ret_DueDays.Text = dt.Rows(0).Item("DUEDAYS_CARTOGRAPHIC").ToString
                            Else
                                txt_Ret_DueDays.Text = ""
                            End If
                        End If
                        If DDL_Materials.SelectedItem.ToString = "AV Materials" Then
                            If dt.Rows(0).Item("DUEDAYS_AV").ToString <> "" Then
                                txt_Ret_DueDays.Text = dt.Rows(0).Item("DUEDAYS_AV").ToString
                            Else
                                txt_Ret_DueDays.Text = ""
                            End If
                        End If
                        If DDL_Materials.SelectedItem.ToString = "Manuscripts" Then
                            If dt.Rows(0).Item("DUEDAYS_MANUSCRIPTS").ToString <> "" Then
                                txt_Ret_DueDays.Text = dt.Rows(0).Item("DUEDAYS_MANUSCRIPTS").ToString
                            Else
                                txt_Ret_DueDays.Text = ""
                            End If
                        End If
                    Else ' if collection type is book-bank/non-returnable
                        'fine1
                        If DDL_Collections.SelectedValue = "G" Then 'book bank general
                            If dt.Rows(0).Item("FINE1_BBGENERAL").ToString <> "" Then
                                txt_Ret_Fine1.Text = dt.Rows(0).Item("FINE1_BBGENERAL").ToString
                            Else
                                txt_Ret_Fine1.Text = ""
                            End If
                        End If
                        If DDL_Collections.SelectedValue = "S" Then 'book bank general
                            If dt.Rows(0).Item("FINE1_BBRESERVE").ToString <> "" Then
                                txt_Ret_Fine1.Text = dt.Rows(0).Item("FINE1_BBRESERVE").ToString
                            Else
                                txt_Ret_Fine1.Text = ""
                            End If
                        End If
                        If DDL_Collections.SelectedValue = "N" Then 'book non-refudnable
                            If dt.Rows(0).Item("FINE1_NONRETURNABLE").ToString <> "" Then
                                txt_Ret_Fine1.Text = dt.Rows(0).Item("FINE1_NONRETURNABLE").ToString
                            Else
                                txt_Ret_Fine1.Text = ""
                            End If
                        End If

                        If DDL_Collections.SelectedValue = "R" Then 'reference
                            If dt.Rows(0).Item("FINE1_BBGENERAL").ToString <> "" Then
                                txt_Ret_Fine1.Text = ""
                            Else
                                txt_Ret_Fine1.Text = ""
                            End If
                        End If

                        'fine2
                        If DDL_Collections.SelectedValue = "G" Then 'book bank general
                            If dt.Rows(0).Item("FINE2_BBGENERAL").ToString <> "" Then
                                txt_Ret_Fine2.Text = dt.Rows(0).Item("FINE2_BBGENERAL").ToString
                            Else
                                txt_Ret_Fine2.Text = ""
                            End If
                        End If
                        If DDL_Collections.SelectedValue = "S" Then 'book bank general
                            If dt.Rows(0).Item("FINE2_BBRESERVE").ToString <> "" Then
                                txt_Ret_Fine2.Text = dt.Rows(0).Item("FINE2_BBRESERVE").ToString
                            Else
                                txt_Ret_Fine2.Text = ""
                            End If
                        End If
                        If DDL_Collections.SelectedValue = "N" Then 'book non-refudnable
                            If dt.Rows(0).Item("FINE2_NONRETURNABLE").ToString <> "" Then
                                txt_Ret_Fine2.Text = dt.Rows(0).Item("FINE2_NONRETURNABLE").ToString
                            Else
                                txt_Ret_Fine2.Text = ""
                            End If
                        End If

                        If DDL_Collections.SelectedValue = "R" Then 'reference
                            If dt.Rows(0).Item("FINE2_BBGENERAL").ToString <> "" Then
                                txt_Ret_Fine2.Text = ""
                            Else
                                txt_Ret_Fine2.Text = ""
                            End If
                        End If

                        'duedays
                        If DDL_Collections.SelectedValue = "G" Then 'book bank general
                            If dt.Rows(0).Item("DUEDAYS_BBGENERAL").ToString <> "" Then
                                txt_Ret_DueDays.Text = dt.Rows(0).Item("DUEDAYS_BBGENERAL").ToString
                            Else
                                txt_Ret_DueDays.Text = ""
                            End If
                        End If
                        If DDL_Collections.SelectedValue = "S" Then 'book bank general
                            If dt.Rows(0).Item("DUEDAYS_BBRESERVE").ToString <> "" Then
                                txt_Ret_DueDays.Text = dt.Rows(0).Item("DUEDAYS_BBRESERVE").ToString
                            Else
                                txt_Ret_DueDays.Text = ""
                            End If
                        End If
                        If DDL_Collections.SelectedValue = "N" Then 'book bank general
                            If dt.Rows(0).Item("DUEDAYS_NONRETURNABLE").ToString <> "" Then
                                txt_Ret_DueDays.Text = dt.Rows(0).Item("DUEDAYS_NONRETURNABLE").ToString
                            Else
                                txt_Ret_DueDays.Text = ""
                            End If
                        End If
                        If DDL_Collections.SelectedValue = "R" Then 'reference
                            If dt.Rows(0).Item("DUEDAYS_NONRETURNABLE").ToString <> "" Then
                                txt_Ret_DueDays.Text = ""
                            Else
                                txt_Ret_DueDays.Text = ""
                            End If
                        End If
                    End If
                Else
                    txt_Ret_DueDays.Text = ""
                    txt_Ret_Fine1.Text = ""
                    txt_Ret_Fine2.Text = ""
                End If
            Else
                Label26.Text = ""
                Image4.ImageUrl = Nothing
                txt_Ret_MemNo.Text = ""
                txt_Ret_MemName.Text = ""
                DDL_MemberCategories.ClearSelection()
                DDL_MemberSubCategories.ClearSelection()
                DDL_MemberStatus.ClearSelection()
                DDL_FineSystem.ClearSelection()
                txt_Ret_Mobile.Text = ""
                txt_Ret_MemEmail.Text = ""
                txt_Ret_AdmDate.Text = ""
                txt_Ret_ClosingDate.Text = ""
                txt_Ret_DueDays.Text = ""
                txt_Ret_Gap1.Text = ""
                txt_Ret_Fine1.Text = ""
                txt_Ret_Fine2.Text = ""
                txt_Ret_AccessionNo.Focus()
            End If
            SqlConn.Close()
            CalcualteFine()
        Catch ex As Exception
            Lbl_Error.Text = "Error: " & (ex.Message())
            Label16.Text = ""
        Finally
            SqlConn.Close()
        End Try
    End Sub
    Public Sub CalcualteFine()
        txt_Ret_FineDue.Text = ""
        txt_Ret_FineCollected.Text = ""
        If txt_Ret_DueDate.Text <> "" Then
            Dim DUE_DATE As Date = Nothing
            Dim RETURN_DATE As Date = Nothing
            Dim nd As Object = Nothing
            nd = datechk(txt_Ret_DueDate.Text)
            DUE_DATE = Date.Parse(nd, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))

            RETURN_DATE = Now.Date
            Dim result As Integer = DateTime.Compare(DUE_DATE, RETURN_DATE)

            If result < 0 Then
                Dim OverDueDays As Integer = RETURN_DATE.Subtract(DUE_DATE).TotalDays

                'check holidays
                Dim str As Object = Nothing
                Dim flag As Integer = Nothing
                str = "SELECT count(HOLI_ID) as Number FROM HOLIDAYS WHERE (LIB_CODE = '" & Trim(LibCode) & "') AND (HOLI_DATE >= '" & Trim(DUE_DATE) & "' and HOLI_DATE <= '" & Trim(RETURN_DATE) & "')"
                Dim cmd1 As New SqlCommand(str, SqlConn)
                SqlConn.Open()
                flag = cmd1.ExecuteScalar
                SqlConn.Close()
                If flag <> Nothing Then
                    OverDueDays = (OverDueDays - flag)
                End If

                Dim FineDue As Decimal = Nothing
                FineDue = txt_Ret_Fine2.Text * OverDueDays
                txt_Ret_FineDue.Text = FineDue
                txt_Ret_FineCollected.Text = FineDue
            Else
                txt_Ret_FineDue.Text = ""
                txt_Ret_FineCollected.Text = ""
            End If
        End If

    End Sub
    'return documents    
    Protected Sub Cir_Return_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Cir_Return_Bttn.Click
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing
        Try
            If IsPostBack = True Then
                Dim iloop As Integer
                Dim strcurrentchar As Object
                Dim c As Integer
                Dim counter1, counter2, counter3, counter4, counter5, counter6, counter7, counter8, counter9, counter10, counter11, counter12, counter13, counter14, counter15, counter16, counter17, counter18, counter19 As Integer

                If Label5.Text = "" Then
                    Lbl_Error2.Text = "Document being Returned is not displayed!"
                    Exit Sub
                End If

                If DDL_CurrentStatus.Text <> "" Then
                    If DDL_CurrentStatus.SelectedValue <> 2 Then
                        Lbl_Error2.Text = "The Current Status of this document is not issued!"
                        Exit Sub
                    End If
                Else
                    Lbl_Error2.Text = "The Current Status of this document is not displayed!"
                    Exit Sub
                End If

                If txt_Ret_IssueDate.Text = "" Then
                    Lbl_Error2.Text = "The document is not Issued as Issue Date is not displayed!"
                    Exit Sub
                End If

                If Label26.Text = "" Then
                    Lbl_Error2.Text = "Member Details not displayed!"
                    Exit Sub
                End If

                Dim CIR_ID As Integer = Nothing
                If Label27.Text <> "" Then
                    CIR_ID = Trim(Label27.Text)
                    If IsNumeric(CIR_ID) = False Then
                        Lbl_Error2.Text = "Error: ID is not Proper!"
                        txt_Ret_AccessionNo.Focus()
                        Exit Sub
                    End If
                    If Len(CIR_ID) > 10 Then
                        Lbl_Error2.Text = "Error: ID is not Proper!"
                        txt_Ret_AccessionNo.Focus()
                        Exit Sub
                    End If
                    CIR_ID = " " & CIR_ID & " "
                    If InStr(1, CIR_ID, "CREATE", 1) > 0 Or InStr(1, CIR_ID, "DELETE", 1) > 0 Or InStr(1, CIR_ID, "DROP", 1) > 0 Or InStr(1, CIR_ID, "INSERT", 1) > 1 Or InStr(1, CIR_ID, "TRACK", 1) > 1 Or InStr(1, CIR_ID, "TRACE", 1) > 1 Then
                        Lbl_Error2.Text = "Error: Input is not Valid !"
                        txt_Ret_AccessionNo.Focus()
                        Exit Sub
                    End If
                    CIR_ID = TrimX(CIR_ID)
                    'check unwanted characters
                    c = 0
                    counter1 = 0
                    For iloop = 1 To Len(CIR_ID.ToString)
                        strcurrentchar = Mid(CIR_ID, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter1 = 1
                            End If
                        End If
                    Next
                    If counter1 = 1 Then
                        Lbl_Error2.Text = "Error: Input is not Valid !"
                        txt_Ret_AccessionNo.Focus()
                        Exit Sub
                    End If
                Else
                    Lbl_Error2.Text = "Error: Record not displayed for Transaction!"
                    txt_Cir_MemNo.Focus()
                    Exit Sub
                End If

                Dim HOLD_ID As Integer = Nothing
                If RadioButton3.Checked = True Then
                    If Label5.Text <> "" Then
                        HOLD_ID = Trim(Label5.Text)
                        If IsNumeric(HOLD_ID) = False Then
                            Lbl_Error2.Text = "Error: Item ID is not Proper!"
                            txt_Cir_AccessionNo.Focus()
                            Exit Sub
                        End If
                        If Len(HOLD_ID) > 10 Then
                            Lbl_Error2.Text = "Error: Item ID is not Proper!"
                            txt_Cir_AccessionNo.Focus()
                            Exit Sub
                        End If
                        HOLD_ID = " " & HOLD_ID & " "
                        If InStr(1, HOLD_ID, "CREATE", 1) > 0 Or InStr(1, HOLD_ID, "DELETE", 1) > 0 Or InStr(1, HOLD_ID, "DROP", 1) > 0 Or InStr(1, HOLD_ID, "INSERT", 1) > 1 Or InStr(1, HOLD_ID, "TRACK", 1) > 1 Or InStr(1, HOLD_ID, "TRACE", 1) > 1 Then
                            Lbl_Error2.Text = "Error: Input is not Valid !"
                            txt_Cir_AccessionNo.Focus()
                            Exit Sub
                        End If
                        HOLD_ID = TrimX(HOLD_ID)
                        'check unwanted characters
                        c = 0
                        counter2 = 0
                        For iloop = 1 To Len(HOLD_ID.ToString)
                            strcurrentchar = Mid(HOLD_ID, iloop, 1)
                            If c = 0 Then
                                If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter2 = 1
                                End If
                            End If
                        Next
                        If counter2 = 1 Then
                            Lbl_Error2.Text = "Error: Input is not Valid !"
                            txt_Cir_AccessionNo.Focus()
                            Exit Sub
                        End If
                    Else
                        Lbl_Error2.Text = "Error: Item details not displayed for Transaction!"
                        txt_Cir_AccessionNo.Focus()
                        Exit Sub
                    End If
                End If

                Dim COPY_ID As Integer = Nothing
                If RadioButton4.Checked = True Then
                    If Label5.Text <> "" Then
                        COPY_ID = Trim(Label5.Text)
                        If IsNumeric(COPY_ID) = False Then
                            Lbl_Error2.Text = "Error: Item ID is not Proper!"
                            txt_Cir_AccessionNo.Focus()
                            Exit Sub
                        End If
                        If Len(COPY_ID) > 10 Then
                            Lbl_Error2.Text = "Error: Item ID is not Proper!"
                            txt_Cir_AccessionNo.Focus()
                            Exit Sub
                        End If
                        COPY_ID = " " & COPY_ID & " "
                        If InStr(1, COPY_ID, "CREATE", 1) > 0 Or InStr(1, COPY_ID, "DELETE", 1) > 0 Or InStr(1, COPY_ID, "DROP", 1) > 0 Or InStr(1, COPY_ID, "INSERT", 1) > 1 Or InStr(1, COPY_ID, "TRACK", 1) > 1 Or InStr(1, COPY_ID, "TRACE", 1) > 1 Then
                            Lbl_Error2.Text = "Error: Input is not Valid !"
                            txt_Cir_AccessionNo.Focus()
                            Exit Sub
                        End If
                        COPY_ID = TrimX(COPY_ID)
                        'check unwanted characters
                        c = 0
                        counter3 = 0
                        For iloop = 1 To Len(COPY_ID.ToString)
                            strcurrentchar = Mid(COPY_ID, iloop, 1)
                            If c = 0 Then
                                If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter3 = 1
                                End If
                            End If
                        Next
                        If counter3 = 1 Then
                            Lbl_Error2.Text = "Error: Input is not Valid !"
                            txt_Cir_AccessionNo.Focus()
                            Exit Sub
                        End If
                    Else
                        Lbl_Error2.Text = "Error: Item details not displayed for Transaction!"
                        txt_Cir_AccessionNo.Focus()
                        Exit Sub
                    End If
                End If

                Dim MEM_ID As Integer = Nothing
                If Label26.Text <> "" Then
                    MEM_ID = Trim(Label26.Text)
                    If IsNumeric(MEM_ID) = False Then
                        Lbl_Error2.Text = "Error: Item ID is not Proper!"
                        txt_Cir_AccessionNo.Focus()
                        Exit Sub
                    End If
                    If Len(MEM_ID) > 10 Then
                        Lbl_Error2.Text = "Error: Item ID is not Proper!"
                        txt_Cir_AccessionNo.Focus()
                        Exit Sub
                    End If
                    MEM_ID = " " & MEM_ID & " "
                    If InStr(1, MEM_ID, "CREATE", 1) > 0 Or InStr(1, MEM_ID, "DELETE", 1) > 0 Or InStr(1, MEM_ID, "DROP", 1) > 0 Or InStr(1, MEM_ID, "INSERT", 1) > 1 Or InStr(1, MEM_ID, "TRACK", 1) > 1 Or InStr(1, MEM_ID, "TRACE", 1) > 1 Then
                        Lbl_Error2.Text = "Error: Input is not Valid !"
                        txt_Cir_AccessionNo.Focus()
                        Exit Sub
                    End If
                    MEM_ID = TrimX(MEM_ID)
                    'check unwanted characters
                    c = 0
                    counter11 = 0
                    For iloop = 1 To Len(MEM_ID.ToString)
                        strcurrentchar = Mid(MEM_ID, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter11 = 1
                            End If
                        End If
                    Next
                    If counter11 = 1 Then
                        Lbl_Error2.Text = "Error: Input is not Valid !"
                        txt_Cir_AccessionNo.Focus()
                        Exit Sub
                    End If
                Else
                    Lbl_Error2.Text = "Error: Member Record not displayed !"
                    txt_Cir_AccessionNo.Focus()
                    Exit Sub
                End If

                'return Date
                Dim RETURN_DATE As Object = Nothing
                If txt_Ret_ReturnDate.Text <> "" Then
                    RETURN_DATE = TrimX(txt_Ret_ReturnDate.Text)
                    RETURN_DATE = RemoveQuotes(RETURN_DATE)
                    RETURN_DATE = Convert.ToDateTime(RETURN_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                    If Len(RETURN_DATE) > 12 Then
                        Lbl_Error2.Text = " Input is not Valid..."
                        Me.txt_Ret_AccessionNo.Focus()
                        Exit Sub
                    End If
                    RETURN_DATE = " " & RETURN_DATE & " "
                    If InStr(1, RETURN_DATE, "CREATE", 1) > 0 Or InStr(1, RETURN_DATE, "DELETE", 1) > 0 Or InStr(1, RETURN_DATE, "DROP", 1) > 0 Or InStr(1, RETURN_DATE, "INSERT", 1) > 1 Or InStr(1, RETURN_DATE, "TRACK", 1) > 1 Or InStr(1, RETURN_DATE, "TRACE", 1) > 1 Then
                        Lbl_Error2.Text = "  Input is not Valid... "
                        txt_Ret_AccessionNo.Focus()
                        Exit Sub
                    End If
                    RETURN_DATE = TrimX(RETURN_DATE)
                    'check unwanted characters
                    c = 0
                    counter4 = 0
                    For iloop = 1 To Len(RETURN_DATE)
                        strcurrentchar = Mid(RETURN_DATE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter4 = 1
                            End If
                        End If
                    Next
                    If counter4 = 1 Then
                        Lbl_Error2.Text = "data is not Valid... "
                        txt_Ret_AccessionNo.Focus()
                        Exit Sub
                    End If
                Else
                    RETURN_DATE = Now.Date
                    RETURN_DATE = Convert.ToDateTime(RETURN_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us
                End If

                'return Time
                Dim RETURN_TIME As Object = Nothing
                If txt_Ret_ReturnTime.Text <> "" Then
                    RETURN_TIME = TrimX(txt_Ret_ReturnTime.Text)
                    RETURN_TIME = RemoveQuotes(RETURN_TIME)

                    If Len(RETURN_TIME) > 12 Then
                        Lbl_Error2.Text = " Input is not Valid..."
                        Me.txt_Ret_AccessionNo.Focus()
                        Exit Sub
                    End If
                    RETURN_TIME = " " & RETURN_TIME & " "
                    If InStr(1, RETURN_TIME, "CREATE", 1) > 0 Or InStr(1, RETURN_TIME, "DELETE", 1) > 0 Or InStr(1, RETURN_TIME, "DROP", 1) > 0 Or InStr(1, RETURN_TIME, "INSERT", 1) > 1 Or InStr(1, RETURN_TIME, "TRACK", 1) > 1 Or InStr(1, RETURN_TIME, "TRACE", 1) > 1 Then
                        Lbl_Error2.Text = "  Input is not Valid... "
                        Me.txt_Ret_AccessionNo.Focus()
                        Exit Sub
                    End If
                    RETURN_TIME = TrimX(RETURN_TIME)
                    'check unwanted characters
                    c = 0
                    counter5 = 0
                    For iloop = 1 To Len(RETURN_TIME)
                        strcurrentchar = Mid(RETURN_TIME, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter5 = 1
                            End If
                        End If
                    Next
                    If counter5 = 1 Then
                        Lbl_Error2.Text = "data is not Valid... "
                        Me.txt_Ret_AccessionNo.Focus()
                        Exit Sub
                    End If
                Else
                    RETURN_TIME = Now.TimeOfDay
                End If

                'Server Validation for FINE_DUE
                Dim FINE_DUE As Object = Nothing
                If txt_Ret_FineDue.Text <> "" Then
                    FINE_DUE = TrimX(txt_Ret_FineDue.Text)
                    FINE_DUE = RemoveQuotes(FINE_DUE)
                    If FINE_DUE.ToString.Length > 15 Then
                        Lbl_Error2.Text = "Plz Enter data with  Proper Length!"
                        txt_Ret_AccessionNo.Focus()
                        Exit Sub
                    End If
                    FINE_DUE = " " & FINE_DUE & " "
                    If InStr(1, FINE_DUE, "CREATE", 1) > 0 Or InStr(1, FINE_DUE, "DELETE", 1) > 0 Or InStr(1, FINE_DUE, "DROP", 1) > 0 Or InStr(1, FINE_DUE, "INSERT", 1) > 1 Or InStr(1, FINE_DUE, "TRACK", 1) > 1 Or InStr(1, FINE_DUE, "TRACE", 1) > 1 Then
                        Lbl_Error2.Text = "Do Not Use Reserve Words!"
                        txt_Ret_AccessionNo.Focus()
                        Exit Sub
                    End If
                    FINE_DUE = TrimX(FINE_DUE)
                    c = 0
                    counter6 = 0
                    For iloop = 1 To Len(FINE_DUE)
                        strcurrentchar = Mid(FINE_DUE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>=%abcdefghijklmnopqrstuvwxyz,/;:_)(""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter6 = 1
                            End If
                        End If
                    Next
                    If counter6 = 1 Then
                        Lbl_Error2.Text = "Do Not Use Un-Wanted Charactrs!"
                        txt_Ret_AccessionNo.Focus()
                        Exit Sub
                    End If
                Else
                    FINE_DUE = Nothing
                End If

                'Server Validation for FINE_DUE
                Dim FINE_COLLECTED As Object = Nothing
                If txt_Ret_FineCollected.Text <> "" Then
                    If txt_Ret_FineCollected.Text <> 0 Then
                        FINE_COLLECTED = TrimX(txt_Ret_FineCollected.Text)
                        FINE_COLLECTED = RemoveQuotes(FINE_COLLECTED)
                        If FINE_DUE.ToString.Length > 15 Then
                            Lbl_Error2.Text = "Plz Enter data with  Proper Length!"
                            txt_Ret_AccessionNo.Focus()
                            Exit Sub
                        End If
                        FINE_COLLECTED = " " & FINE_COLLECTED & " "
                        If InStr(1, FINE_COLLECTED, "CREATE", 1) > 0 Or InStr(1, FINE_COLLECTED, "DELETE", 1) > 0 Or InStr(1, FINE_COLLECTED, "DROP", 1) > 0 Or InStr(1, FINE_COLLECTED, "INSERT", 1) > 1 Or InStr(1, FINE_COLLECTED, "TRACK", 1) > 1 Or InStr(1, FINE_COLLECTED, "TRACE", 1) > 1 Then
                            Lbl_Error2.Text = "Do Not Use Reserve Words!"
                            txt_Ret_AccessionNo.Focus()
                            Exit Sub
                        End If
                        FINE_COLLECTED = TrimX(FINE_COLLECTED)
                        c = 0
                        counter7 = 0
                        For iloop = 1 To Len(FINE_COLLECTED)
                            strcurrentchar = Mid(FINE_COLLECTED, iloop, 1)
                            If c = 0 Then
                                If Not InStr("'~!@#$^&*=+|[]{}?<>=%abcdefghijklmnopqrstuvwxyz,/;:_)(""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter7 = 1
                                End If
                            End If
                        Next
                        If counter7 = 1 Then
                            Lbl_Error2.Text = "Do Not Use Un-Wanted Charactrs!"
                            txt_Ret_AccessionNo.Focus()
                            Exit Sub
                        End If
                    Else
                        FINE_COLLECTED = Nothing
                    End If
                Else
                    FINE_COLLECTED = Nothing
                End If

                'REMARKS
                Dim REMARKS As Object = Nothing
                If txt_Ret_Remarks.Text <> "" Then
                    REMARKS = TrimAll(txt_Ret_Remarks.Text)
                    REMARKS = RemoveQuotes(REMARKS)
                    If REMARKS.Length > 255 Then 'maximum length
                        Lbl_Error2.Text = " Data must be of Proper Length.. "
                        txt_Ret_AccessionNo.Focus()
                        Exit Sub
                    End If

                    REMARKS = " " & REMARKS & " "
                    If InStr(1, REMARKS, "CREATE", 1) > 0 Or InStr(1, REMARKS, "DELETE", 1) > 0 Or InStr(1, REMARKS, "DROP", 1) > 0 Or InStr(1, REMARKS, "INSERT", 1) > 1 Or InStr(1, REMARKS, "TRACK", 1) > 1 Or InStr(1, REMARKS, "TRACE", 1) > 1 Then
                        Lbl_Error2.Text = " Do Not use Reserve Words... "
                        Me.txt_Ret_AccessionNo.Focus()
                        Exit Sub
                    End If
                    REMARKS = TrimAll(REMARKS)
                    'check unwanted characters
                    c = 0
                    counter8 = 0
                    For iloop = 1 To Len(REMARKS.ToString)
                        strcurrentchar = Mid(REMARKS, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter8 = 1
                            End If
                        End If
                    Next
                    If counter8 = 1 Then
                        Lbl_Error2.Text = " Do Not use un-wanted Characters... "
                        Me.txt_Ret_AccessionNo.Focus()
                        Exit Sub
                    End If
                Else
                    REMARKS = ""
                End If

                'EXEMPT_FINE
                Dim EXEMPT_FINE As Object = Nothing
                If txt_Ret_FineDue.Text <> "" Then
                    If CheckBox7.Checked = True Then
                        EXEMPT_FINE = "Y"
                    Else
                        EXEMPT_FINE = "N"
                    End If
                Else
                    EXEMPT_FINE = Nothing
                End If

                If EXEMPT_FINE = "Y" Then
                    FINE_COLLECTED = Nothing
                End If

                Dim STATUS As String = Nothing
                STATUS = "Returned"

                Dim DATE_MODIFIED As Object = Nothing
                DATE_MODIFIED = Now.Date
                DATE_MODIFIED = Convert.ToDateTime(DATE_MODIFIED, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                Dim IP As Object = Nothing
                IP = Request.UserHostAddress.Trim

                Dim USER_CODE As Object = Nothing
                USER_CODE = UserCode

                If SqlConn.State = 0 Then
                    SqlConn.Open()
                End If
                thisTransaction = SqlConn.BeginTransaction()

                Dim intValue As Integer = 0
                Dim objCommand As New SqlCommand
                objCommand.Connection = SqlConn
                objCommand.Transaction = thisTransaction
                objCommand.CommandType = CommandType.Text
                objCommand.CommandText = "UPDATE CIRCULATION SET RETURN_DATE=@RETURN_DATE, RETURN_TIME=@RETURN_TIME, FINE_DUE=@FINE_DUE, FINE_COLLECTED=@FINE_COLLECTED, EXEMPT_FINE=@EXEMPT_FINE, REMARKS=@REMARKS, STATUS=@STATUS, DATE_MODIFIED=@DATE_MODIFIED, IP=@IP, UPDATED_BY=@UPDATED_BY  WHERE (CIR_ID = @CIR_ID  AND LIB_CODE =@LIB_CODE);"

                objCommand.Parameters.Add("@CIR_ID", SqlDbType.Int)
                If CIR_ID = 0 Then
                    objCommand.Parameters("@CIR_ID").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@CIR_ID").Value = CIR_ID
                End If

                objCommand.Parameters.Add("@RETURN_DATE", SqlDbType.DateTime)
                If RETURN_DATE = "" Then
                    objCommand.Parameters("@RETURN_DATE").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@RETURN_DATE").Value = RETURN_DATE
                End If

                objCommand.Parameters.Add("@RETURN_TIME", SqlDbType.DateTime)
                If RETURN_TIME = "" Then
                    objCommand.Parameters("@RETURN_TIME").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@RETURN_TIME").Value = RETURN_TIME
                End If

                objCommand.Parameters.Add("@FINE_DUE", SqlDbType.Decimal)
                If FINE_DUE = "" Then
                    objCommand.Parameters("@FINE_DUE").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@FINE_DUE").Value = FINE_DUE
                End If

                objCommand.Parameters.Add("@FINE_COLLECTED", SqlDbType.Decimal)
                If FINE_COLLECTED = "" Then
                    objCommand.Parameters("@FINE_COLLECTED").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@FINE_COLLECTED").Value = FINE_COLLECTED
                End If

                objCommand.Parameters.Add("@REMARKS", SqlDbType.NVarChar)
                If REMARKS = "" Then
                    objCommand.Parameters("@REMARKS").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@REMARKS").Value = REMARKS
                End If

                objCommand.Parameters.Add("@EXEMPT_FINE", SqlDbType.VarChar)
                If EXEMPT_FINE = "" Then
                    objCommand.Parameters("@EXEMPT_FINE").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@EXEMPT_FINE").Value = EXEMPT_FINE
                End If

                objCommand.Parameters.Add("@STATUS", SqlDbType.VarChar)
                If STATUS = "" Then
                    objCommand.Parameters("@STATUS").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@STATUS").Value = STATUS
                End If

                objCommand.Parameters.Add("@DATE_MODIFIED", SqlDbType.DateTime)
                If DATE_MODIFIED = "" Then
                    objCommand.Parameters("@DATE_MODIFIED").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@DATE_MODIFIED").Value = DATE_MODIFIED
                End If

                objCommand.Parameters.Add("@IP", SqlDbType.VarChar)
                If IP = "" Then
                    objCommand.Parameters("@IP").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@IP").Value = IP
                End If

                objCommand.Parameters.Add("@UPDATED_BY", SqlDbType.NVarChar)
                If USER_CODE = "" Then
                    objCommand.Parameters("@UPDATED_BY").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@UPDATED_BY").Value = USER_CODE
                End If

                objCommand.Parameters.Add("@LIB_CODE", SqlDbType.NVarChar)
                If LibCode = "" Then
                    objCommand.Parameters("@LIB_CODE").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@LIB_CODE").Value = LibCode
                End If

                Dim dr As SqlDataReader
                dr = objCommand.ExecuteReader()
                dr.Close()

                'update HOLDINGS TABLE
                If RadioButton3.Checked = True Then 'books
                    Dim objCommand4 As New SqlCommand
                    objCommand4.Connection = SqlConn
                    objCommand4.Transaction = thisTransaction
                    objCommand4.CommandType = CommandType.Text
                    objCommand4.CommandText = "UPDATE HOLDINGS SET STA_CODE ='1' WHERE (HOLD_ID = @HOLD_ID AND LIB_CODE =@LIB_CODE);"

                    objCommand4.Parameters.Add("@HOLD_ID", SqlDbType.Int)
                    objCommand4.Parameters("@HOLD_ID").Value = HOLD_ID

                    objCommand4.Parameters.Add("@LIB_CODE", SqlDbType.NVarChar)
                    objCommand4.Parameters("@LIB_CODE").Value = LibCode

                    Dim dr4 As SqlDataReader
                    dr4 = objCommand4.ExecuteReader()
                    dr4.Close()
                End If
                If RadioButton4.Checked = True Then 'loose 
                    Dim objCommand4 As New SqlCommand
                    objCommand4.Connection = SqlConn
                    objCommand4.Transaction = thisTransaction
                    objCommand4.CommandType = CommandType.Text
                    objCommand4.CommandText = "UPDATE LOOSE_ISSUES_COPIES SET STA_CODE ='1' WHERE (COPY_ID = @COPY_ID) AND (LIB_CODE =@LIB_CODE);"

                    objCommand4.Parameters.Add("@COPY_ID", SqlDbType.Int)
                    objCommand4.Parameters("@COPY_ID").Value = COPY_ID

                    objCommand4.Parameters.Add("@LIB_CODE", SqlDbType.NVarChar)
                    objCommand4.Parameters("@LIB_CODE").Value = LibCode

                    Dim dr4 As SqlDataReader
                    dr4 = objCommand4.ExecuteReader()
                    dr4.Close()
                End If

                'save fine
                If txt_Ret_FineDue.Text <> "" Then
                    If CheckBox7.Checked = False Then
                        Dim FINE_STATUS As Object = Nothing

                        If txt_Ret_FineCollected.Text <> "" Then
                            If txt_Ret_FineCollected.Text <> 0 Then
                                If Convert.ToDecimal(txt_Ret_FineDue.Text) = Convert.ToDecimal(txt_Ret_FineCollected.Text) Then
                                    FINE_STATUS = "Paid"
                                Else
                                    FINE_STATUS = "Pending"
                                End If
                            Else
                                FINE_STATUS = "Pending"
                            End If
                        Else
                            FINE_STATUS = "Pending"
                        End If

                        Dim DATE_ADDED As Object = Nothing
                        DATE_ADDED = Now.Date
                        DATE_ADDED = Convert.ToDateTime(DATE_ADDED, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us


                            Dim REC_ID As Integer = Nothing
                            Dim objCommand5 As New SqlCommand
                            objCommand5.Connection = SqlConn
                            objCommand5.Transaction = thisTransaction
                            objCommand5.CommandType = CommandType.Text
                            objCommand5.CommandText = "INSERT INTO RECEIPTS (CIR_ID, MEM_ID, HOLD_ID, COPY_ID, AMOUNT_DUE, AMOUNT_RECD, DATE_RECD, RECD_FOR, MEM_PERIOD, REMARKS, STATUS, DATE_ADDED, USER_CODE, LIB_CODE, IP) " & _
                                                     " VALUES (@CIR_ID, @MEM_ID, @HOLD_ID, @COPY_ID, @AMOUNT_DUE, @AMOUNT_RECD, @DATE_RECD, @RECD_FOR, @MEM_PERIOD, @REMARKS, @STATUS, @DATE_ADDED, @USER_CODE, @LIB_CODE, @IP); " & _
                                                     " SELECT SCOPE_IDENTITY();"


                            objCommand5.Parameters.Add("@CIR_ID", SqlDbType.Int)
                            If CIR_ID = 0 Then
                                objCommand5.Parameters("@CIR_ID").Value = System.DBNull.Value
                            Else
                                objCommand5.Parameters("@CIR_ID").Value = CIR_ID
                            End If

                            objCommand5.Parameters.Add("@MEM_ID", SqlDbType.Int)
                            If MEM_ID = 0 Then
                                objCommand5.Parameters("@MEM_ID").Value = System.DBNull.Value
                            Else
                                objCommand5.Parameters("@MEM_ID").Value = MEM_ID
                            End If

                            objCommand5.Parameters.Add("@HOLD_ID", SqlDbType.Int)
                            If HOLD_ID = 0 Then
                                objCommand5.Parameters("@HOLD_ID").Value = System.DBNull.Value
                            Else
                                objCommand5.Parameters("@HOLD_ID").Value = HOLD_ID
                            End If

                            objCommand5.Parameters.Add("@COPY_ID", SqlDbType.Int)
                            If COPY_ID = 0 Then
                                objCommand5.Parameters("@COPY_ID").Value = System.DBNull.Value
                            Else
                                objCommand5.Parameters("@COPY_ID").Value = COPY_ID
                            End If

                            objCommand5.Parameters.Add("@AMOUNT_DUE", SqlDbType.Float)
                            If FINE_DUE = "" Then
                                objCommand5.Parameters("@AMOUNT_DUE").Value = System.DBNull.Value
                            Else
                                objCommand5.Parameters("@AMOUNT_DUE").Value = FINE_DUE
                            End If

                            objCommand5.Parameters.Add("@AMOUNT_RECD", SqlDbType.Float)
                            If FINE_COLLECTED = "" Then
                                objCommand5.Parameters("@AMOUNT_RECD").Value = System.DBNull.Value
                            Else
                                objCommand5.Parameters("@AMOUNT_RECD").Value = FINE_COLLECTED
                            End If

                            objCommand5.Parameters.Add("@DATE_RECD", SqlDbType.DateTime)
                            If RETURN_DATE = "" Then
                                objCommand5.Parameters("@DATE_RECD").Value = System.DBNull.Value
                            Else
                                objCommand5.Parameters("@DATE_RECD").Value = RETURN_DATE
                            End If

                            objCommand5.Parameters.Add("@REMARKS", SqlDbType.NVarChar)
                            If REMARKS = "" Then
                                objCommand5.Parameters("@REMARKS").Value = System.DBNull.Value
                            Else
                                objCommand5.Parameters("@REMARKS").Value = REMARKS
                            End If

                            objCommand5.Parameters.Add("@RECD_FOR", SqlDbType.VarChar)
                            objCommand5.Parameters("@RECD_FOR").Value = "F"

                            objCommand5.Parameters.Add("@STATUS", SqlDbType.VarChar)
                            If FINE_STATUS = "" Then
                                objCommand5.Parameters("@STATUS").Value = System.DBNull.Value
                            Else
                                objCommand5.Parameters("@STATUS").Value = FINE_STATUS
                            End If

                            objCommand5.Parameters.Add("@MEM_PERIOD", SqlDbType.VarChar)
                            objCommand5.Parameters("@MEM_PERIOD").Value = Now.Year

                            objCommand5.Parameters.Add("@DATE_ADDED", SqlDbType.DateTime)
                            If DATE_ADDED = "" Then
                                objCommand5.Parameters("@DATE_ADDED").Value = System.DBNull.Value
                            Else
                                objCommand5.Parameters("@DATE_ADDED").Value = DATE_ADDED
                            End If

                            objCommand5.Parameters.Add("@IP", SqlDbType.VarChar)
                            If IP = "" Then
                                objCommand5.Parameters("@IP").Value = System.DBNull.Value
                            Else
                                objCommand5.Parameters("@IP").Value = IP
                            End If

                            objCommand5.Parameters.Add("@USER_CODE", SqlDbType.NVarChar)
                            If USER_CODE = "" Then
                                objCommand5.Parameters("@USER_CODE").Value = System.DBNull.Value
                            Else
                                objCommand5.Parameters("@USER_CODE").Value = USER_CODE
                            End If

                            objCommand5.Parameters.Add("@LIB_CODE", SqlDbType.NVarChar)
                            If LibCode = "" Then
                                objCommand5.Parameters("@LIB_CODE").Value = System.DBNull.Value
                            Else
                                objCommand5.Parameters("@LIB_CODE").Value = LibCode
                            End If

                            Dim dr5 As SqlDataReader
                            dr5 = objCommand5.ExecuteReader()
                            If dr5.Read Then
                                REC_ID = dr5.GetValue(0)
                            End If
                            dr5.Close()
                        End If
                End If

                thisTransaction.Commit()
                SqlConn.Close()
                ReserveMessage()
                txt_Ret_AccessionNo.Text = ""
                txt_Ret_AccessionNo_TextChanged(sender, e)
            End If
        Catch q As SqlException
            thisTransaction.Rollback()
            Lbl_Error2.Text = "Error: " & (q.Message())
        Catch ex As Exception
            Lbl_Error2.Text = "Error: " & (ex.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    Public Sub ReserveMessage()
        Dim dt As New DataTable
        Try
            Dim iloop As Integer
            Dim strcurrentchar As Object
            Dim c As Integer
            Dim counter1 As Integer

            Dim HOLD_ID As Object = Nothing
            If RadioButton3.Checked = True Then
                If Trim(Label5.Text) <> "" Then
                    HOLD_ID = TrimX(Label5.Text)
                    HOLD_ID = RemoveQuotes(HOLD_ID)
                Else
                    HOLD_ID = Nothing
                End If
            End If

            Dim COPY_ID As Object = Nothing
            If RadioButton4.Checked = True Then
                If Trim(Label5.Text) <> "" Then
                    COPY_ID = TrimX(Label5.Text)
                    COPY_ID = RemoveQuotes(HOLD_ID)
                Else
                    COPY_ID = Nothing
                End If
            End If

            Dim SQL As String = Nothing
            If RadioButton3.Checked = True Then
                SQL = "SELECT CIRCULATION.CIR_ID, CIRCULATION.MEM_ID, CIRCULATION.HOLD_ID, MEMBERSHIPS.MEM_NO, MEMBERSHIPS.MEM_NAME FROM   CIRCULATION , MEMBERSHIPS WHERE (CIRCULATION.LIB_CODE ='" & Trim(LibCode) & "'  AND CIRCULATION.HOLD_ID  ='" & Trim(HOLD_ID) & "' AND CIRCULATION.STATUS='Reserved') AND (CIRCULATION.LIB_CODE ='" & Trim(LibCode) & "'  AND CIRCULATION.MEM_ID  = MEMBERSHIPS.MEM_ID) ORDER BY CIRCULATION.RESERVE_DATE, CIRCULATION.RESERVE_TIME "
            Else
                SQL = "SELECT CIRCULATION.CIR_ID, CIRCULATION.MEM_ID, CIRCULATION.COPY_ID, MEMBERSHIPS.MEM_NO, MEMBERSHIPS.MEM_NAME FROM   CIRCULATION , MEMBERSHIPS WHERE (CIRCULATION.LIB_CODE ='" & Trim(LibCode) & "'  AND CIRCULATION.COPY_ID  ='" & Trim(COPY_ID) & "' AND CIRCULATION.STATUS='Reserved') AND (CIRCULATION.LIB_CODE ='" & Trim(LibCode) & "'  AND CIRCULATION.MEM_ID  = MEMBERSHIPS.MEM_ID) ORDER BY CIRCULATION.RESERVE_DATE, CIRCULATION.RESERVE_TIME "
            End If
            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(Sql, SqlConn)
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            da.Fill(ds)

            dt = ds.Tables(0).Copy

            If dt.Rows.Count <> 0 Then
                Dim i As Integer = Nothing
                For i = 0 To dt.Rows.Count - 1
                    If dt.Rows(0).Item("MEM_NO").ToString <> "" Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert(' This Book / Journal is Reserved First to Member No: " & dt.Rows(i)("MEM_NO").ToString & "; " & dt.Rows(i)("MEM_NAME").ToString & "');", True)
                    End If
                Next
            End If
        Catch ex As Exception
            Lbl_Error2.Text = "Error: " & (ex.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'cancel button
    Protected Sub Ret_Cancel_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Ret_Cancel_Bttn.Click
        txt_Ret_FineDue.Text = ""
        txt_Ret_FineCollected.Text = ""
        Label5.Text = ""
        Label26.Text = ""
        Label15.Text = ""
        Label18.Text = ""
        Label19.Text = ""
        Label27.Text = ""
        Label28.Text = ""
        Image3.ImageUrl = Nothing
        Image4.ImageUrl = Nothing
        txt_Ret_MemNo.Text = ""
        txt_Ret_MemName.Text = ""
        DDL_MemberCategories.ClearSelection()
        DDL_MemberSubCategories.ClearSelection()
        DDL_MemberStatus.ClearSelection()
        DDL_FineSystem.ClearSelection()
        txt_Ret_Mobile.Text = ""
        txt_Ret_MemEmail.Text = ""
        txt_Ret_AdmDate.Text = ""
        txt_Ret_ClosingDate.Text = ""
        txt_Ret_DueDays.Text = ""
        txt_Ret_Gap1.Text = ""
        txt_Ret_Fine1.Text = ""
        txt_Ret_Fine2.Text = ""
        txt_Ret_IssueDate.Text = ""
        txt_Ret_IssueTime.Text = ""
        txt_Ret_DueDate.Text = ""
        txt_Ret_DueDays.Text = ""
        txt_Ret_ReturnDate.Text = ""
        txt_Ret_ReturnTime.Text = ""
        txt_Ret_RenewDate.Text = ""
        txt_Ret_Remarks.Text = ""
        txt_Ret_AccessionNo.Text = ""
        Label9.Text = ""
        Label17.Text = ""
        Label18.Text = ""
        Label19.Text = ""

        DDL_BibLevel.ClearSelection()
        DDL_Materials.ClearSelection()
        DDL_Documents.ClearSelection()
        DDL_Collections.ClearSelection()
        DDL_CurrentStatus.ClearSelection()
        Lbl_Error2.Text = ""
        txt_Ret_AccessionNo.Focus()
    End Sub
    'renew book
    Protected Sub Cir_Renew_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Cir_Renew_Bttn.Click
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing
        Try
            If IsPostBack = True Then
                Dim iloop As Integer
                Dim strcurrentchar As Object
                Dim c As Integer
                Dim counter1, counter2, counter3, counter4, counter5, counter6, counter7, counter8, counter9, counter10, counter11, counter12, counter13, counter14, counter15, counter16, counter17, counter18, counter19 As Integer

                If txt_Ret_FineDue.Text <> "" Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert(' The Due Date is expired and thus document can not be RENEWED! Plz Return the Document, Pay the Fine if any and get it Re-Issued!');", True)
                    txt_Ret_AccessionNo.Focus()
                    Exit Sub
                End If

                Dim CIR_ID As Integer = Nothing
                If Label27.Text <> "" Then
                    CIR_ID = Trim(Label27.Text)
                    If IsNumeric(CIR_ID) = False Then
                        Lbl_Error2.Text = "Error: ID is not Proper!"
                        txt_Ret_AccessionNo.Focus()
                        Exit Sub
                    End If
                    If Len(CIR_ID) > 10 Then
                        Lbl_Error2.Text = "Error: ID is not Proper!"
                        txt_Ret_AccessionNo.Focus()
                        Exit Sub
                    End If
                    CIR_ID = " " & CIR_ID & " "
                    If InStr(1, CIR_ID, "CREATE", 1) > 0 Or InStr(1, CIR_ID, "DELETE", 1) > 0 Or InStr(1, CIR_ID, "DROP", 1) > 0 Or InStr(1, CIR_ID, "INSERT", 1) > 1 Or InStr(1, CIR_ID, "TRACK", 1) > 1 Or InStr(1, CIR_ID, "TRACE", 1) > 1 Then
                        Lbl_Error2.Text = "Error: Input is not Valid !"
                        txt_Ret_AccessionNo.Focus()
                        Exit Sub
                    End If
                    CIR_ID = TrimX(CIR_ID)
                    'check unwanted characters
                    c = 0
                    counter1 = 0
                    For iloop = 1 To Len(CIR_ID.ToString)
                        strcurrentchar = Mid(CIR_ID, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter1 = 1
                            End If
                        End If
                    Next
                    If counter1 = 1 Then
                        Lbl_Error2.Text = "Error: Input is not Valid !"
                        txt_Ret_AccessionNo.Focus()
                        Exit Sub
                    End If
                Else
                    Lbl_Error2.Text = "Error: Record not displayed for Transaction!"
                    txt_Cir_MemNo.Focus()
                    Exit Sub
                End If

                Dim HOLD_ID As Integer = Nothing
                If RadioButton3.Checked = True Then
                    If Label5.Text <> "" Then
                        HOLD_ID = Trim(Label5.Text)
                        If IsNumeric(HOLD_ID) = False Then
                            Lbl_Error2.Text = "Error: Item ID is not Proper!"
                            txt_Cir_AccessionNo.Focus()
                            Exit Sub
                        End If
                        If Len(HOLD_ID) > 10 Then
                            Lbl_Error2.Text = "Error: Item ID is not Proper!"
                            txt_Cir_AccessionNo.Focus()
                            Exit Sub
                        End If
                        HOLD_ID = " " & HOLD_ID & " "
                        If InStr(1, HOLD_ID, "CREATE", 1) > 0 Or InStr(1, HOLD_ID, "DELETE", 1) > 0 Or InStr(1, HOLD_ID, "DROP", 1) > 0 Or InStr(1, HOLD_ID, "INSERT", 1) > 1 Or InStr(1, HOLD_ID, "TRACK", 1) > 1 Or InStr(1, HOLD_ID, "TRACE", 1) > 1 Then
                            Lbl_Error2.Text = "Error: Input is not Valid !"
                            txt_Cir_AccessionNo.Focus()
                            Exit Sub
                        End If
                        HOLD_ID = TrimX(HOLD_ID)
                        'check unwanted characters
                        c = 0
                        counter2 = 0
                        For iloop = 1 To Len(HOLD_ID.ToString)
                            strcurrentchar = Mid(HOLD_ID, iloop, 1)
                            If c = 0 Then
                                If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter2 = 1
                                End If
                            End If
                        Next
                        If counter2 = 1 Then
                            Lbl_Error2.Text = "Error: Input is not Valid !"
                            txt_Cir_AccessionNo.Focus()
                            Exit Sub
                        End If
                    Else
                        Lbl_Error2.Text = "Error: Item details not displayed for Transaction!"
                        txt_Cir_AccessionNo.Focus()
                        Exit Sub
                    End If
                End If

                Dim COPY_ID As Integer = Nothing
                If RadioButton4.Checked = True Then
                    If Label5.Text <> "" Then
                        COPY_ID = Trim(Label5.Text)
                        If IsNumeric(COPY_ID) = False Then
                            Lbl_Error2.Text = "Error: Item ID is not Proper!"
                            txt_Cir_AccessionNo.Focus()
                            Exit Sub
                        End If
                        If Len(COPY_ID) > 10 Then
                            Lbl_Error2.Text = "Error: Item ID is not Proper!"
                            txt_Cir_AccessionNo.Focus()
                            Exit Sub
                        End If
                        COPY_ID = " " & COPY_ID & " "
                        If InStr(1, COPY_ID, "CREATE", 1) > 0 Or InStr(1, COPY_ID, "DELETE", 1) > 0 Or InStr(1, COPY_ID, "DROP", 1) > 0 Or InStr(1, COPY_ID, "INSERT", 1) > 1 Or InStr(1, COPY_ID, "TRACK", 1) > 1 Or InStr(1, COPY_ID, "TRACE", 1) > 1 Then
                            Lbl_Error2.Text = "Error: Input is not Valid !"
                            txt_Cir_AccessionNo.Focus()
                            Exit Sub
                        End If
                        COPY_ID = TrimX(COPY_ID)
                        'check unwanted characters
                        c = 0
                        counter3 = 0
                        For iloop = 1 To Len(COPY_ID.ToString)
                            strcurrentchar = Mid(COPY_ID, iloop, 1)
                            If c = 0 Then
                                If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter3 = 1
                                End If
                            End If
                        Next
                        If counter3 = 1 Then
                            Lbl_Error2.Text = "Error: Input is not Valid !"
                            txt_Cir_AccessionNo.Focus()
                            Exit Sub
                        End If
                    Else
                        Lbl_Error2.Text = "Error: Item details not displayed for Transaction!"
                        txt_Cir_AccessionNo.Focus()
                        Exit Sub
                    End If
                End If

                Dim MEM_ID As Integer = Nothing
                If Label26.Text <> "" Then
                    MEM_ID = Trim(Label26.Text)
                    If IsNumeric(MEM_ID) = False Then
                        Lbl_Error2.Text = "Error: Item ID is not Proper!"
                        txt_Cir_AccessionNo.Focus()
                        Exit Sub
                    End If
                    If Len(MEM_ID) > 10 Then
                        Lbl_Error2.Text = "Error: Item ID is not Proper!"
                        txt_Cir_AccessionNo.Focus()
                        Exit Sub
                    End If
                    MEM_ID = " " & MEM_ID & " "
                    If InStr(1, MEM_ID, "CREATE", 1) > 0 Or InStr(1, MEM_ID, "DELETE", 1) > 0 Or InStr(1, MEM_ID, "DROP", 1) > 0 Or InStr(1, MEM_ID, "INSERT", 1) > 1 Or InStr(1, MEM_ID, "TRACK", 1) > 1 Or InStr(1, MEM_ID, "TRACE", 1) > 1 Then
                        Lbl_Error2.Text = "Error: Input is not Valid !"
                        txt_Cir_AccessionNo.Focus()
                        Exit Sub
                    End If
                    MEM_ID = TrimX(MEM_ID)
                    'check unwanted characters
                    c = 0
                    counter4 = 0
                    For iloop = 1 To Len(MEM_ID.ToString)
                        strcurrentchar = Mid(MEM_ID, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter4 = 1
                            End If
                        End If
                    Next
                    If counter4 = 1 Then
                        Lbl_Error2.Text = "Error: Input is not Valid !"
                        txt_Cir_AccessionNo.Focus()
                        Exit Sub
                    End If
                Else
                    Lbl_Error2.Text = "Error: Member Record not displayed !"
                    txt_Cir_AccessionNo.Focus()
                    Exit Sub
                End If


                'Due Date
                Dim DUE_DATE As Object = Nothing
                If txt_Ret_DueDate.Text <> "" Then
                    DUE_DATE = TrimX(txt_Ret_DueDate.Text)
                    DUE_DATE = RemoveQuotes(DUE_DATE)
                    DUE_DATE = Convert.ToDateTime(DUE_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                    If Len(DUE_DATE) > 12 Then
                        Lbl_Error2.Text = " Input is not Valid..."
                        Me.txt_Cir_AccessionNo.Focus()
                        Exit Sub
                    End If
                    DUE_DATE = " " & DUE_DATE & " "
                    If InStr(1, DUE_DATE, "CREATE", 1) > 0 Or InStr(1, DUE_DATE, "DELETE", 1) > 0 Or InStr(1, DUE_DATE, "DROP", 1) > 0 Or InStr(1, DUE_DATE, "INSERT", 1) > 1 Or InStr(1, DUE_DATE, "TRACK", 1) > 1 Or InStr(1, DUE_DATE, "TRACE", 1) > 1 Then
                        Lbl_Error2.Text = "  Input is not Valid... "
                        Me.txt_Cir_AccessionNo.Focus()
                        Exit Sub
                    End If
                    DUE_DATE = TrimX(DUE_DATE)
                    'check unwanted characters
                    c = 0
                    counter5 = 0
                    For iloop = 1 To Len(DUE_DATE)
                        strcurrentchar = Mid(DUE_DATE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter5 = 1
                            End If
                        End If
                    Next
                    If counter5 = 1 Then
                        Lbl_Error2.Text = "data is not Valid... "
                        Me.txt_Cir_AccessionNo.Focus()
                        Exit Sub
                    End If
                Else
                    If DDL_CollectionType.SelectedValue = "R" Then
                        Lbl_Error.Text = "This copy belongs to REFERENCE Collection Type!  "
                        Me.txt_Cir_AccessionNo.Focus()
                        Exit Sub
                    Else
                        Lbl_Error.Text = "Due Date is not defined, plz define DUE DAYS of this document in member registration!  "
                        Me.txt_Cir_AccessionNo.Focus()
                        Exit Sub
                    End If
                End If

                'DUE_DAYS
                Dim DUE_DAYS As Integer = Nothing
                Dim NEW_DUE_DATE As Object = Nothing
                If txt_Ret_DueDays.Text <> "" Then
                    DUE_DAYS = TrimX(txt_Ret_DueDays.Text)
                    NEW_DUE_DATE = DateAdd(Microsoft.VisualBasic.DateInterval.Day, DUE_DAYS, DUE_DATE)
                    NEW_DUE_DATE = FormatDateTime(NEW_DUE_DATE, DateFormat.ShortDate)
                End If


                'return Date
                Dim RENEW_DATE As Object = Nothing
                If txt_Ret_RenewDate.Text <> "" Then
                    RENEW_DATE = TrimX(txt_Ret_RenewDate.Text)
                    RENEW_DATE = RemoveQuotes(RENEW_DATE)
                    RENEW_DATE = Convert.ToDateTime(RENEW_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                    If Len(RENEW_DATE) > 12 Then
                        Lbl_Error2.Text = " Input is not Valid..."
                        Me.txt_Ret_AccessionNo.Focus()
                        Exit Sub
                    End If
                    RENEW_DATE = " " & RENEW_DATE & " "
                    If InStr(1, RENEW_DATE, "CREATE", 1) > 0 Or InStr(1, RENEW_DATE, "DELETE", 1) > 0 Or InStr(1, RENEW_DATE, "DROP", 1) > 0 Or InStr(1, RENEW_DATE, "INSERT", 1) > 1 Or InStr(1, RENEW_DATE, "TRACK", 1) > 1 Or InStr(1, RENEW_DATE, "TRACE", 1) > 1 Then
                        Lbl_Error2.Text = "  Input is not Valid... "
                        txt_Ret_AccessionNo.Focus()
                        Exit Sub
                    End If
                    RENEW_DATE = TrimX(RENEW_DATE)
                    'check unwanted characters
                    c = 0
                    counter6 = 0
                    For iloop = 1 To Len(RENEW_DATE)
                        strcurrentchar = Mid(RENEW_DATE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("';<>=()%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter6 = 1
                            End If
                        End If
                    Next
                    If counter6 = 1 Then
                        Lbl_Error2.Text = "data is not Valid... "
                        txt_Ret_AccessionNo.Focus()
                        Exit Sub
                    End If
                Else
                    RENEW_DATE = Now.Date
                    RENEW_DATE = Convert.ToDateTime(RENEW_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us
                End If

                'REMARKS
                Dim REMARKS As Object = Nothing
                If txt_Ret_Remarks.Text <> "" Then
                    REMARKS = TrimAll(txt_Ret_Remarks.Text)
                    REMARKS = RemoveQuotes(REMARKS)
                    If REMARKS.Length > 255 Then 'maximum length
                        Lbl_Error2.Text = " Data must be of Proper Length.. "
                        txt_Ret_AccessionNo.Focus()
                        Exit Sub
                    End If

                    REMARKS = " " & REMARKS & " "
                    If InStr(1, REMARKS, "CREATE", 1) > 0 Or InStr(1, REMARKS, "DELETE", 1) > 0 Or InStr(1, REMARKS, "DROP", 1) > 0 Or InStr(1, REMARKS, "INSERT", 1) > 1 Or InStr(1, REMARKS, "TRACK", 1) > 1 Or InStr(1, REMARKS, "TRACE", 1) > 1 Then
                        Lbl_Error2.Text = " Do Not use Reserve Words... "
                        Me.txt_Ret_AccessionNo.Focus()
                        Exit Sub
                    End If
                    REMARKS = TrimAll(REMARKS)
                    'check unwanted characters
                    c = 0
                    counter7 = 0
                    For iloop = 1 To Len(REMARKS.ToString)
                        strcurrentchar = Mid(REMARKS, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#^*+|?%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter7 = 1
                            End If
                        End If
                    Next
                    If counter7 = 1 Then
                        Lbl_Error2.Text = " Do Not use un-wanted Characters... "
                        Me.txt_Ret_AccessionNo.Focus()
                        Exit Sub
                    End If
                Else
                    REMARKS = ""
                End If

                Dim DATE_MODIFIED As Object = Nothing
                DATE_MODIFIED = Now.Date
                DATE_MODIFIED = Convert.ToDateTime(DATE_MODIFIED, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                Dim IP As Object = Nothing
                IP = Request.UserHostAddress.Trim

                Dim USER_CODE As Object = Nothing
                USER_CODE = UserCode


                If SqlConn.State = 0 Then
                    SqlConn.Open()
                End If
                thisTransaction = SqlConn.BeginTransaction()

                Dim intValue As Integer = 0
                Dim objCommand As New SqlCommand
                objCommand.Connection = SqlConn
                objCommand.Transaction = thisTransaction
                objCommand.CommandType = CommandType.Text
                objCommand.CommandText = "UPDATE CIRCULATION SET RENEW_DATE=@RENEW_DATE, DUE_DATE=@DUE_DATE, REMARKS=@REMARKS, DATE_MODIFIED=@DATE_MODIFIED, IP=@IP, UPDATED_BY=@UPDATED_BY  WHERE (CIR_ID = @CIR_ID  AND LIB_CODE =@LIB_CODE);"

                objCommand.Parameters.Add("@CIR_ID", SqlDbType.Int)
                If CIR_ID = 0 Then
                    objCommand.Parameters("@CIR_ID").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@CIR_ID").Value = CIR_ID
                End If

                objCommand.Parameters.Add("@RENEW_DATE", SqlDbType.DateTime)
                If RENEW_DATE = "" Then
                    objCommand.Parameters("@RENEW_DATE").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@RENEW_DATE").Value = RENEW_DATE
                End If

                objCommand.Parameters.Add("@DUE_DATE", SqlDbType.DateTime)
                If NEW_DUE_DATE = "" Then
                    objCommand.Parameters("@DUE_DATE").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@DUE_DATE").Value = NEW_DUE_DATE
                End If


                objCommand.Parameters.Add("@REMARKS", SqlDbType.NVarChar)
                If REMARKS = "" Then
                    objCommand.Parameters("@REMARKS").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@REMARKS").Value = REMARKS
                End If

                objCommand.Parameters.Add("@DATE_MODIFIED", SqlDbType.DateTime)
                If DATE_MODIFIED = "" Then
                    objCommand.Parameters("@DATE_MODIFIED").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@DATE_MODIFIED").Value = DATE_MODIFIED
                End If

                objCommand.Parameters.Add("@IP", SqlDbType.VarChar)
                If IP = "" Then
                    objCommand.Parameters("@IP").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@IP").Value = IP
                End If

                objCommand.Parameters.Add("@UPDATED_BY", SqlDbType.NVarChar)
                If USER_CODE = "" Then
                    objCommand.Parameters("@UPDATED_BY").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@UPDATED_BY").Value = USER_CODE
                End If

                objCommand.Parameters.Add("@LIB_CODE", SqlDbType.NVarChar)
                If LibCode = "" Then
                    objCommand.Parameters("@LIB_CODE").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@LIB_CODE").Value = LibCode
                End If

                Dim dr As SqlDataReader
                dr = objCommand.ExecuteReader()
                dr.Close()

                thisTransaction.Commit()
                SqlConn.Close()
                ReserveMessage()
                txt_Ret_AccessionNo.Text = ""
                txt_Ret_AccessionNo_TextChanged(sender, e)
            End If
        Catch q As SqlException
            thisTransaction.Rollback()
            Lbl_Error2.Text = "Error: " & (q.Message())
        Catch ex As Exception
            Lbl_Error2.Text = "Error: " & (ex.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub

    
    
   
End Class