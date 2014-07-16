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

Public Class Bills
    Inherits System.Web.UI.Page
    Dim index As Integer = 0
    Public Srno As Integer = 0
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session.Item("IsThisU") <> Nothing Then
                If Not SConnection() = True Then
                    lbl_Error.Text = "Database Connection is lost..Try Again !'"
                    lbl_msg.Text = ""
                Else
                    If Page.IsPostBack = False Then
                        Bill_Calculate_Bttn.Visible = True
                        Bill_Cancel_Bttn.Visible = True
                        Bill_Save_Bttn.Visible = False
                        Bill_Update_Bttn.Visible = False
                        Bill_DeleteAll_Bttn.Visible = False
                        lbl_msg.Text = "Enter Data and Press SAVE Button to save the record.."
                        lbl_Error.Text = ""
                        PopulateCurrencies()
                        DDL_Currencies.SelectedValue = "INR"
                        PopulateBudgetYear()
                        PopulateVendors()
                        PopulateVendors2()
                        PopulateBillsGrid()
                    End If
                    Dim CreateSUserButton As Object 'System.Web.UI.WebControls.Button
                    CreateSUserButton = Master.FindControl("Accordion1").FindControl("BudgPane").FindControl("Budg_BillProcessing_Bttn")
                    CreateSUserButton.ForeColor = Drawing.Color.Red
                    myPaneName = "BudgPane"
                End If
            Else
                Response.Redirect("~/Default.aspx", False)
            End If
            LibCode = Session.Item("LoggedLibcode")
        Catch ex As Exception
            lbl_Error.Text = "Error: " & (ex.Message())
            lbl_msg.Text = ""
        End Try
    End Sub
   
    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As MenuEventArgs) Handles Menu1.MenuItemClick
        MultiView1.ActiveViewIndex = Int32.Parse(e.Item.Value)
        If MultiView1.ActiveViewIndex = 0 Then
            Menu1.Items(0).ImageUrl = "~/Images/AddBills_Up.png"
            Menu1.Items(1).ImageUrl = "~/Images/AttachBills_Over.png"
            Menu1.Items(2).ImageUrl = "~/Images/PostBills_Over.png"
            Menu1.Items(3).ImageUrl = "~/Images/PayBills_Over.png"
            PopulateBillsGrid()
            DDL_BudgYear.Focus()
        End If
        If MultiView1.ActiveViewIndex = 1 Then '
            Menu1.Items(0).ImageUrl = "~/Images/AddBills_Over.png"
            Menu1.Items(1).ImageUrl = "~/Images/AttachBills_Up.png"
            Menu1.Items(2).ImageUrl = "~/Images/PostBills_Over.png"
            Menu1.Items(3).ImageUrl = "~/Images/PayBills_Over.png"
            DDL_Vendors2.Text = ""
            DDL_Orders.Text = ""
            DDL_Bills.Items.Clear()
            Grid2.DataSource = Nothing
            Grid2.DataBind()
            DDL_Vendors2.Focus()
        End If
        If MultiView1.ActiveViewIndex = 2 Then
            Menu1.Items(0).ImageUrl = "~/Images/AddBills_Over.png"
            Menu1.Items(1).ImageUrl = "~/Images/AttachBills_Over.png"
            Menu1.Items(2).ImageUrl = "~/Images/PostBills_Up.png"
            Menu1.Items(3).ImageUrl = "~/Images/PayBills_Over.png"
            PopulateBillsGrid3()
            txt_Bill_PmtReqNo.Focus()
        End If
        If MultiView1.ActiveViewIndex = 3 Then
            Menu1.Items(0).ImageUrl = "~/Images/AddBills_Over.png"
            Menu1.Items(1).ImageUrl = "~/Images/AttachBills_Over.png"
            Menu1.Items(2).ImageUrl = "~/Images/PostBills_Over.png"
            Menu1.Items(3).ImageUrl = "~/Images/PayBills_Up.png"
            TR_BANK.Visible = False
            TR_CHKNO.Visible = False
            PopulatePmtReqNo()
            DDL_PmtReqNo.Focus()
        End If
    End Sub
    Protected Sub Bill_Cancel_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Bill_Cancel_Bttn.Click
        ClearBillFields()
        Me.Bill_Calculate_Bttn.Visible = True
        Bill_Save_Bttn.Visible = False
        Bill_Update_Bttn.Visible = False
        lbl_msg.Text = "Enter Data and Press SAVE Button to save the record.."
        lbl_Error.Text = ""
    End Sub
    Public Sub ClearBillFields()
        txt_Bill_BillNo.Text = ""
        txt_Bill_BillDate.Text = ""
        txt_Bill_Amount.Text = ""
        txt_Bill_Discount.Text = ""
        txt_Bill_AmountAfterDiscount.Text = ""
        txt_Bill_OtherCharges.Text = ""
        txt_Bill_AmountBilled.Text = ""
        txt_Bill_Deduction.Text = ""
        txt_Bill_AmountPassed.Text = ""
        txt_Bill_Remarks.Text = ""
        txt_Bill_ConversionRate.Text = ""
        txt_Bill_RsAmount.Text = ""
        Label23.Text = ""
    End Sub
    Public Sub PopulateCurrencies()
        Me.DDL_Currencies.DataSource = GetCurrenciesList()
        Me.DDL_Currencies.DataTextField = "CUR_NAME"
        Me.DDL_Currencies.DataValueField = "CUR_CODE"
        Me.DDL_Currencies.DataBind()
        DDL_Currencies.Items.Insert(0, "")
    End Sub
    Public Sub PopulateBudgetYear()
        Dim SQL As String = Nothing
        Dim dtBudgYear As DataTable = Nothing
        Try
            SQL = "SELECT DISTINCT BUDG_YEAR FROM BUDGETS WHERE (LIB_CODE ='" & Trim(LibCode) & "') ORDER BY BUDG_YEAR "
            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            da.Fill(ds)

            dtBudgYear = ds.Tables(0).Copy
            If dtBudgYear.Rows.Count = 0 Then
                Me.DDL_BudgYear.DataSource = Nothing
                DDL_BudgYear.DataBind()
            Else
                Me.DDL_BudgYear.DataSource = dtBudgYear
                Me.DDL_BudgYear.DataTextField = "BUDG_YEAR"
                Me.DDL_BudgYear.DataValueField = "BUDG_YEAR"
                Me.DDL_BudgYear.DataBind()
                DDL_BudgYear.Items.Insert(0, "")
            End If
        Catch s As Exception
            lbl_Error.Text = "Error: " & (s.Message())
            lbl_msg.Text = ""
        Finally
            SqlConn.Close()
        End Try
    End Sub
    Public Sub DDL_BudgYear_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDL_BudgYear.SelectedIndexChanged
        Dim SQL As String = Nothing
        Dim dtBudgHead As DataTable = Nothing
        Try
            Dim myBudgYear As Integer = Nothing
            If DDL_BudgYear.Text <> "" Then
                myBudgYear = DDL_BudgYear.SelectedValue
                SQL = "SELECT BUDG_ID, BUDG_HEAD FROM BUDGETS WHERE (LIB_CODE ='" & Trim(LibCode) & "') AND (BUDG_YEAR = '" & Trim(myBudgYear) & "') ORDER BY BUDG_HEAD "
                Dim ds As New DataSet
                Dim da As New SqlDataAdapter(SQL, SqlConn)
                If SqlConn.State = 0 Then
                    SqlConn.Open()
                End If
                da.Fill(ds)

                dtBudgHead = ds.Tables(0).Copy
                If dtBudgHead.Rows.Count = 0 Then
                    Me.DDL_BudgHead.DataSource = Nothing
                    DDL_BudgHead.DataBind()
                Else
                    Me.DDL_BudgHead.DataSource = dtBudgHead
                    Me.DDL_BudgHead.DataTextField = "BUDG_HEAD"
                    Me.DDL_BudgHead.DataValueField = "BUDG_ID"
                    Me.DDL_BudgHead.DataBind()
                    DDL_BudgHead.Items.Insert(0, "")
                    DDL_BudgHead.Focus()
                End If
            Else
                Me.DDL_BudgHead.DataSource = Nothing
                DDL_BudgHead.DataBind()
                DDL_BudgHead.Items.Clear()
                DDL_BudgYear.Focus()
            End If
        Catch s As Exception
            lbl_Error.Text = "Error: " & (s.Message())
            lbl_msg.Text = ""
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'autocomplete method for discount
    <System.Web.Script.Services.ScriptMethod(), _
  System.Web.Services.WebMethod()> _
    Public Shared Function SearchDis(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Return EG4.PopulateCombo.SearchDiscount(prefixText, count)
    End Function
    'autocomplete method for discount
    <System.Web.Script.Services.ScriptMethod(), _
  System.Web.Services.WebMethod()> _
    Public Shared Function SearchAllBills(ByVal prefixText As String, ByVal count As Integer, ByVal LIB_CODE As Object) As List(Of String)
        Return EG4.PopulateCombo.SearchBills(prefixText, count, LibCode)
    End Function
    Public Sub PopulateVendors()
        DDL_Vendors.DataTextField = "VEND_NAME"
        DDL_Vendors.DataValueField = "VEND_ID"
        DDL_Vendors.DataSource = GetVendorList()
        DDL_Vendors.DataBind()
        DDL_Vendors.Items.Insert(0, "")
    End Sub
    Public Sub PopulateVendors2()
        DDL_Vendors2.DataTextField = "VEND_NAME"
        DDL_Vendors2.DataValueField = "VEND_ID"
        DDL_Vendors2.DataSource = GetVendorList()
        DDL_Vendors2.DataBind()
        DDL_Vendors2.Items.Insert(0, "")
    End Sub
    Protected Sub Bill_Calculate_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Bill_Calculate_Bttn.Click
        Try
            If IsPostBack = True Then
                If txt_Bill_Amount.Text <> "" Then
                    Dim myGrossAmount As Decimal = Nothing
                    If txt_Bill_Amount.Text <> "" Then
                        myGrossAmount = CDec(txt_Bill_Amount.Text)
                    Else
                        myGrossAmount = 0
                    End If

                    'calculate conversion rate
                    If txt_Bill_ConversionRate.Text <> "" Then
                        If DDL_Currencies.SelectedValue <> "INR" Then
                            txt_Bill_RsAmount.Text = myGrossAmount * CDec(txt_Bill_ConversionRate.Text)
                        Else
                            txt_Bill_RsAmount.Text = myGrossAmount
                        End If
                    Else
                        txt_Bill_RsAmount.Text = myGrossAmount
                    End If

                    Dim myDiscount As Integer = Nothing
                    If txt_Bill_Discount.Text = "" Then
                        myDiscount = 0
                    Else
                        myDiscount = CInt(txt_Bill_Discount.Text)
                    End If

                    If myDiscount > 100 Then
                        lbl_Error.Text = "Discount can not be greater than 100%)"
                        lbl_msg.Text = ""
                        Exit Sub
                    End If

                    'calcualte amountafter discount
                    Dim myLessDiscountAmount As Decimal = Nothing
                    If txt_Bill_Discount.Text = "" Then
                        txt_Bill_AmountAfterDiscount.Text = txt_Bill_RsAmount.Text
                        myLessDiscountAmount = 0
                    Else
                        myLessDiscountAmount = CDec(txt_Bill_RsAmount.Text) - ((myDiscount * CDec(txt_Bill_RsAmount.Text)) / (100))
                        txt_Bill_AmountAfterDiscount.Text = myLessDiscountAmount '(txt_Bill_Amount.Text) - (txt_Bill_Discount.Text * txt_Bill_Amount.Text)
                    End If

                    'calculate amount billed
                    Dim AdditionalCharges As Decimal = Nothing
                    If txt_Bill_OtherCharges.Text <> "" Then
                        AdditionalCharges = CDec(txt_Bill_OtherCharges.Text)
                        txt_Bill_AmountBilled.Text = CDec(txt_Bill_AmountAfterDiscount.Text) + CDec(txt_Bill_OtherCharges.Text) 'myLessDiscountAmount + AdditionalCharges
                    Else
                        txt_Bill_AmountBilled.Text = txt_Bill_AmountAfterDiscount.Text
                    End If

                    'calcualte deductuiion
                    Dim myDeduction As Decimal = Nothing
                    If txt_Bill_Deduction.Text <> "" Then
                        myDeduction = CDec(txt_Bill_Deduction.Text)
                    Else
                        myDeduction = 0
                    End If

                    If myDeduction = 0 Then
                        txt_Bill_AmountPassed.Text = CDec(txt_Bill_AmountBilled.Text)
                    Else
                        txt_Bill_AmountPassed.Text = CDec(txt_Bill_AmountBilled.Text) - (myDeduction)
                    End If

                    If Label23.Text <> "" Then
                        Me.Bill_Save_Bttn.Visible = False
                        Bill_Update_Bttn.Visible = True
                        Me.Bill_Calculate_Bttn.Visible = False
                    Else
                        Me.Bill_Save_Bttn.Visible = True
                        Me.Bill_Calculate_Bttn.Visible = False
                        Bill_Update_Bttn.Visible = False
                    End If
                End If
            End If
        Catch s As Exception
            lbl_Error.Text = "Error: " & (s.Message())
            lbl_msg.Text = ""
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'save record
    Protected Sub Bill_Save_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Bill_Save_Bttn.Click
        If IsPostBack = True Then
            'check unwanted characters
            Dim iloop As Integer
            Dim strcurrentchar As Object
            Dim c As Integer
            Dim counter1, counter2, counter3, counter4, counter5, counter6, counter7, counter8, counter9, counter10, counter11, counter12, counter13, counter14, counter15 As Integer
            Dim thisTransaction As SqlClient.SqlTransaction = Nothing
            Try
                'Server Validation for BUDG_HEAD
                Dim BUDG_ID As Integer = Nothing
                If DDL_BudgHead.Text <> "" Then
                    BUDG_ID = DDL_BudgHead.SelectedValue
                    BUDG_ID = RemoveQuotes(BUDG_ID)

                    If IsNumeric(BUDG_ID) = False Then
                        lbl_Error.Text = "Plz Select Budget Head from Drop-Down!"
                        lbl_msg.Text = ""
                        DDL_BudgHead.Focus()
                        Exit Sub
                    End If
                    If BUDG_ID.ToString.Length > 5 Then
                        lbl_Error.Text = "Plz Enter Data with Proper Length!"
                        lbl_msg.Text = ""
                        DDL_BudgHead.Focus()
                        Exit Sub
                    End If
                    BUDG_ID = " " & BUDG_ID & " "
                    If InStr(1, BUDG_ID, "CREATE", 1) > 0 Or InStr(1, BUDG_ID, "DELETE", 1) > 0 Or InStr(1, BUDG_ID, "DROP", 1) > 0 Or InStr(1, BUDG_ID, "INSERT", 1) > 1 Or InStr(1, BUDG_ID, "TRACK", 1) > 1 Or InStr(1, BUDG_ID, "TRACE", 1) > 1 Then
                        lbl_Error.Text = "Do Not Use Reserve Words!"
                        lbl_msg.Text = ""
                        DDL_BudgHead.Focus()
                        Exit Sub
                    End If
                    BUDG_ID = TrimX(BUDG_ID)

                    c = 0
                    counter1 = 0
                    For iloop = 1 To Len(BUDG_ID.ToString)
                        strcurrentchar = Mid(BUDG_ID, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter1 = 1
                            End If
                        End If
                    Next
                    If counter1 = 1 Then
                        lbl_Error.Text = "Do Not Use Un-Wanted Characters!"
                        lbl_msg.Text = ""
                        DDL_BudgHead.Focus()
                        Exit Sub
                    End If
                Else
                    lbl_Error.Text = "Plz Select Budget Head from Drop-Down!"
                    lbl_msg.Text = ""
                    Me.DDL_BudgHead.Focus()
                    Exit Sub
                End If

                'Server Validation for VENDOR
                Dim VEND_ID As Integer = Nothing
                If DDL_Vendors.Text <> "" Then
                    VEND_ID = DDL_Vendors.SelectedValue
                    VEND_ID = RemoveQuotes(VEND_ID)

                    If IsNumeric(VEND_ID) = False Then
                        lbl_Error.Text = "Plz Select Vendor from Drop-Down!"
                        lbl_msg.Text = ""
                        DDL_Vendors.Focus()
                        Exit Sub
                    End If
                    If VEND_ID.ToString.Length > 5 Then
                        lbl_Error.Text = "Plz Enter Data with Proper Length!"
                        lbl_msg.Text = ""
                        DDL_Vendors.Focus()
                        Exit Sub
                    End If
                    VEND_ID = " " & VEND_ID & " "
                    If InStr(1, VEND_ID, "CREATE", 1) > 0 Or InStr(1, VEND_ID, "DELETE", 1) > 0 Or InStr(1, VEND_ID, "DROP", 1) > 0 Or InStr(1, VEND_ID, "INSERT", 1) > 1 Or InStr(1, VEND_ID, "TRACK", 1) > 1 Or InStr(1, VEND_ID, "TRACE", 1) > 1 Then
                        lbl_Error.Text = "Do Not Use Reserve Words!"
                        lbl_msg.Text = ""
                        DDL_Vendors.Focus()
                        Exit Sub
                    End If
                    VEND_ID = TrimX(VEND_ID)

                    c = 0
                    counter2 = 0
                    For iloop = 1 To Len(VEND_ID.ToString)
                        strcurrentchar = Mid(VEND_ID, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter2 = 1
                            End If
                        End If
                    Next
                    If counter2 = 1 Then
                        lbl_Error.Text = "Do Not Use Un-Wanted Characters!"
                        lbl_msg.Text = ""
                        DDL_Vendors.Focus()
                        Exit Sub
                    End If
                Else
                    lbl_Error.Text = "Plz Select Budget Head from Drop-Down!"
                    lbl_msg.Text = ""
                    Me.DDL_Vendors.Focus()
                    Exit Sub
                End If

                'Server Validation for BUDG_HEAD
                Dim SUPPLEMENT As Object = Nothing
                If DDL_BudgSupp.Text <> "" Then
                    SUPPLEMENT = DDL_BudgSupp.SelectedValue
                    SUPPLEMENT = RemoveQuotes(SUPPLEMENT)

                    If SUPPLEMENT.ToString.Length > 2 Then
                        lbl_Error.Text = "Plz Enter Data with Proper Length!"
                        lbl_msg.Text = ""
                        DDL_BudgSupp.Focus()
                        Exit Sub
                    End If
                    SUPPLEMENT = " " & SUPPLEMENT & " "
                    If InStr(1, SUPPLEMENT, "CREATE", 1) > 0 Or InStr(1, SUPPLEMENT, "DELETE", 1) > 0 Or InStr(1, SUPPLEMENT, "DROP", 1) > 0 Or InStr(1, SUPPLEMENT, "INSERT", 1) > 1 Or InStr(1, SUPPLEMENT, "TRACK", 1) > 1 Or InStr(1, SUPPLEMENT, "TRACE", 1) > 1 Then
                        lbl_Error.Text = "Do Not Use Reserve Words!"
                        lbl_msg.Text = ""
                        DDL_BudgSupp.Focus()
                        Exit Sub
                    End If
                    SUPPLEMENT = TrimX(SUPPLEMENT)

                    c = 0
                    counter3 = 0
                    For iloop = 1 To Len(SUPPLEMENT)
                        strcurrentchar = Mid(SUPPLEMENT, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter3 = 1
                            End If
                        End If
                    Next
                    If counter3 = 1 Then
                        lbl_Error.Text = "Do Not Use Un-Wanted Characters!"
                        lbl_msg.Text = ""
                        DDL_BudgSupp.Focus()
                        Exit Sub
                    End If
                Else
                    SUPPLEMENT = "N"
                End If

                'Server Validation for txt_Bill_BillNo
                Dim INV_NO As Object = Nothing
                If txt_Bill_BillNo.Text <> "" Then
                    INV_NO = TrimX(txt_Bill_BillNo.Text)
                    INV_NO = RemoveQuotes(INV_NO)

                    If INV_NO.ToString.Length > 100 Then
                        lbl_Error.Text = "Plz Enter Data with Proper Length!"
                        lbl_msg.Text = ""
                        txt_Bill_BillNo.Focus()
                        Exit Sub
                    End If
                    INV_NO = " " & INV_NO & " "
                    If InStr(1, INV_NO, "CREATE", 1) > 0 Or InStr(1, INV_NO, "DELETE", 1) > 0 Or InStr(1, INV_NO, "DROP", 1) > 0 Or InStr(1, INV_NO, "INSERT", 1) > 1 Or InStr(1, INV_NO, "TRACK", 1) > 1 Or InStr(1, INV_NO, "TRACE", 1) > 1 Then
                        lbl_Error.Text = "Do Not Use Reserve Words!"
                        lbl_msg.Text = ""
                        txt_Bill_BillNo.Focus()
                        Exit Sub
                    End If
                    INV_NO = TrimX(INV_NO)

                    c = 0
                    counter4 = 0
                    For iloop = 1 To Len(INV_NO)
                        strcurrentchar = Mid(INV_NO, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter4 = 1
                            End If
                        End If
                    Next
                    If counter4 = 1 Then
                        lbl_Error.Text = "Do Not Use Un-Wanted Characters!"
                        lbl_msg.Text = ""
                        txt_Bill_BillNo.Focus()
                        Exit Sub
                    End If
                    'check this bill no status
                    Dim str As Object = Nothing
                    Dim flag As Object = Nothing
                    str = "SELECT INV_NO FROM BILLS WHERE (INV_NO ='" & Trim(INV_NO) & "') AND (VEND_ID = '" & Trim(VEND_ID) & "') AND (LIB_CODE ='" & Trim(LibCode) & "')"
                    Dim cmd1 As New SqlCommand(str, SqlConn)
                    SqlConn.Open()
                    flag = cmd1.ExecuteScalar
                    If flag <> Nothing Then
                        lbl_Error.Text = "This Bill No has already been Processed!"
                        lbl_msg.Text = ""
                        txt_Bill_BillNo.Focus()
                        Exit Sub
                    End If
                    SqlConn.Close()
                Else
                    lbl_Error.Text = "Plz Select Budget Head from Drop-Down!"
                    lbl_msg.Text = ""
                    Me.txt_Bill_BillNo.Focus()
                    Exit Sub
                End If

                'search BILL date
                Dim INV_DATE As Object = Nothing
                If txt_Bill_BillDate.Text <> "" Then
                    INV_DATE = TrimX(txt_Bill_BillDate.Text)
                    INV_DATE = RemoveQuotes(INV_DATE)
                    INV_DATE = Convert.ToDateTime(INV_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                    If Len(INV_DATE) > 12 Then
                        Label15.Text = " Input is not Valid..."
                        Label1.Text = ""
                        Me.txt_Bill_BillDate.Focus()
                        Exit Sub
                    End If
                    INV_DATE = " " & INV_DATE & " "
                    If InStr(1, INV_DATE, "CREATE", 1) > 0 Or InStr(1, INV_DATE, "DELETE", 1) > 0 Or InStr(1, INV_DATE, "DROP", 1) > 0 Or InStr(1, INV_DATE, "INSERT", 1) > 1 Or InStr(1, INV_DATE, "TRACK", 1) > 1 Or InStr(1, INV_DATE, "TRACE", 1) > 1 Then
                        Label15.Text = "  Input is not Valid... "
                        Label1.Text = ""
                        Me.txt_Bill_BillDate.Focus()
                        Exit Sub
                    End If
                    INV_DATE = TrimX(INV_DATE)
                    'check unwanted characters
                    c = 0
                    counter5 = 0
                    For iloop = 1 To Len(INV_DATE)
                        strcurrentchar = Mid(INV_DATE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>=%abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ,;:_", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter5 = 1
                            End If
                        End If
                    Next
                    If counter5 = 1 Then
                        Label15.Text = "data is not Valid... "
                        Label1.Text = ""
                        Me.txt_Bill_BillDate.Focus()
                        Exit Sub
                    End If
                Else
                    INV_DATE = Now.Date
                    INV_DATE = Convert.ToDateTime(INV_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us
                End If
              
                'Server Validation for CURRENCY
                Dim CUR_CODE As Object = Nothing
                If DDL_Currencies.Text <> "" Then
                    CUR_CODE = DDL_Currencies.SelectedValue
                    CUR_CODE = RemoveQuotes(CUR_CODE)

                    If CUR_CODE.ToString.Length > 4 Then
                        lbl_Error.Text = "Plz Enter Data with Proper Length!"
                        lbl_msg.Text = ""
                        DDL_Currencies.Focus()
                        Exit Sub
                    End If
                    CUR_CODE = " " & CUR_CODE & " "
                    If InStr(1, CUR_CODE, "CREATE", 1) > 0 Or InStr(1, CUR_CODE, "DELETE", 1) > 0 Or InStr(1, CUR_CODE, "DROP", 1) > 0 Or InStr(1, CUR_CODE, "INSERT", 1) > 1 Or InStr(1, CUR_CODE, "TRACK", 1) > 1 Or InStr(1, CUR_CODE, "TRACE", 1) > 1 Then
                        lbl_Error.Text = "Do Not Use Reserve Words!"
                        lbl_msg.Text = ""
                        DDL_Currencies.Focus()
                        Exit Sub
                    End If
                    CUR_CODE = TrimX(CUR_CODE)

                    c = 0
                    counter6 = 0
                    For iloop = 1 To Len(CUR_CODE)
                        strcurrentchar = Mid(CUR_CODE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter6 = 1
                            End If
                        End If
                    Next
                    If counter6 = 1 Then
                        lbl_Error.Text = "Do Not Use Un-Wanted Characters!"
                        lbl_msg.Text = ""
                        DDL_Currencies.Focus()
                        Exit Sub
                    End If
                Else
                    CUR_CODE = "INR"
                End If


                'Server Validation for Gross Amount
                Dim INV_AMOUNT As Object = Nothing
                If txt_Bill_Amount.Text <> "" Then
                    INV_AMOUNT = TrimX(txt_Bill_Amount.Text)
                    INV_AMOUNT = RemoveQuotes(INV_AMOUNT)
                    If INV_AMOUNT.ToString.Length > 15 Then
                        lbl_Error.Text = "Plz Enter Data with Proper Length!"
                        lbl_msg.Text = ""
                        txt_Bill_Amount.Focus()
                        Exit Sub
                    End If
                    INV_AMOUNT = " " & INV_AMOUNT & " "
                    If InStr(1, INV_AMOUNT, "CREATE", 1) > 0 Or InStr(1, INV_AMOUNT, "DELETE", 1) > 0 Or InStr(1, INV_AMOUNT, "DROP", 1) > 0 Or InStr(1, INV_AMOUNT, "INSERT", 1) > 1 Or InStr(1, INV_AMOUNT, "TRACK", 1) > 1 Or InStr(1, INV_AMOUNT, "TRACE", 1) > 1 Then
                        lbl_Error.Text = "Do Not Use Reserve Words!"
                        lbl_msg.Text = ""
                        txt_Bill_Amount.Focus()
                        Exit Sub
                    End If
                    INV_AMOUNT = TrimX(INV_AMOUNT)
                    c = 0
                    counter7 = 0
                    For iloop = 1 To Len(INV_AMOUNT.ToString)
                        strcurrentchar = Mid(INV_AMOUNT, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?()<>=%abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ,;:_", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter7 = 1
                            End If
                        End If
                    Next
                    If counter7 = 1 Then
                        lbl_Error.Text = "Do Not Use Un-Wanted Characters!"
                        lbl_msg.Text = ""
                        txt_Bill_Amount.Focus()
                        Exit Sub
                    End If
                Else
                    lbl_Error.Text = "Plz Enter Gross Amount Billed!"
                    lbl_msg.Text = ""
                    Me.txt_Bill_Amount.Focus()
                    Exit Sub
                End If

                'Server Validation for CONVERSION_RATE
                Dim CONVERSION_RATE As Object = Nothing
                If txt_Bill_ConversionRate.Text <> "" Then
                    CONVERSION_RATE = TrimX(txt_Bill_ConversionRate.Text)
                    CONVERSION_RATE = RemoveQuotes(CONVERSION_RATE)
                    If CONVERSION_RATE.ToString.Length > 15 Then
                        lbl_Error.Text = "Plz Enter Data with Proper Length!"
                        lbl_msg.Text = ""
                        txt_Bill_ConversionRate.Focus()
                        Exit Sub
                    End If
                    CONVERSION_RATE = " " & CONVERSION_RATE & " "
                    If InStr(1, CONVERSION_RATE, "CREATE", 1) > 0 Or InStr(1, CONVERSION_RATE, "DELETE", 1) > 0 Or InStr(1, CONVERSION_RATE, "DROP", 1) > 0 Or InStr(1, CONVERSION_RATE, "INSERT", 1) > 1 Or InStr(1, CONVERSION_RATE, "TRACK", 1) > 1 Or InStr(1, CONVERSION_RATE, "TRACE", 1) > 1 Then
                        lbl_Error.Text = "Do Not Use Reserve Words!"
                        lbl_msg.Text = ""
                        txt_Bill_ConversionRate.Focus()
                        Exit Sub
                    End If
                    CONVERSION_RATE = TrimX(CONVERSION_RATE)
                    c = 0
                    counter8 = 0
                    For iloop = 1 To Len(CONVERSION_RATE.ToString)
                        strcurrentchar = Mid(CONVERSION_RATE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>()=%abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ,;:_", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter8 = 1
                            End If
                        End If
                    Next
                    If counter8 = 1 Then
                        lbl_Error.Text = "Do Not Use Un-Wanted Characters!"
                        lbl_msg.Text = ""
                        txt_Bill_ConversionRate.Focus()
                        Exit Sub
                    End If
                Else
                    CONVERSION_RATE = 0
                End If

                'Server Validation for RS_AMOUNT
                Dim RS_AMOUNT As Object = Nothing
                If txt_Bill_RsAmount.Text <> "" Then
                    RS_AMOUNT = TrimX(txt_Bill_RsAmount.Text)
                    RS_AMOUNT = RemoveQuotes(RS_AMOUNT)
                    If RS_AMOUNT.ToString.Length > 15 Then
                        lbl_Error.Text = "Plz Enter Data with Proper Length!"
                        lbl_msg.Text = ""
                        txt_Bill_RsAmount.Focus()
                        Exit Sub
                    End If
                    RS_AMOUNT = " " & RS_AMOUNT & " "
                    If InStr(1, RS_AMOUNT, "CREATE", 1) > 0 Or InStr(1, RS_AMOUNT, "DELETE", 1) > 0 Or InStr(1, RS_AMOUNT, "DROP", 1) > 0 Or InStr(1, RS_AMOUNT, "INSERT", 1) > 1 Or InStr(1, RS_AMOUNT, "TRACK", 1) > 1 Or InStr(1, RS_AMOUNT, "TRACE", 1) > 1 Then
                        lbl_Error.Text = "Do Not Use Reserve Words!"
                        lbl_msg.Text = ""
                        txt_Bill_RsAmount.Focus()
                        Exit Sub
                    End If
                    RS_AMOUNT = TrimX(RS_AMOUNT)
                    c = 0
                    counter9 = 0
                    For iloop = 1 To Len(RS_AMOUNT)
                        strcurrentchar = Mid(RS_AMOUNT, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>()=%abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ,;:_", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter9 = 1
                            End If
                        End If
                    Next
                    If counter9 = 1 Then
                        lbl_Error.Text = "Do Not Use Un-Wanted Characters!"
                        lbl_msg.Text = ""
                        txt_Bill_RsAmount.Focus()
                        Exit Sub
                    End If
                Else
                    lbl_Error.Text = "Plz Enter Amount in Rupees Afeter Conversion!"
                    lbl_msg.Text = ""
                    Me.txt_Bill_RsAmount.Focus()
                    Exit Sub
                End If

                'Server Validation for DISCOUNT
                Dim DISCOUNT As Integer = Nothing
                If txt_Bill_Discount.Text <> "" Then
                    DISCOUNT = txt_Bill_Discount.Text
                    DISCOUNT = RemoveQuotes(DISCOUNT)

                    If IsNumeric(DISCOUNT) = False Then
                        lbl_Error.Text = "Plz Enter Numeric Value Only!"
                        lbl_msg.Text = ""
                        txt_Bill_Discount.Focus()
                        Exit Sub
                    End If
                    If DISCOUNT.ToString.Length > 5 Then
                        lbl_Error.Text = "Plz Enter Data with Proper Length!"
                        lbl_msg.Text = ""
                        txt_Bill_Discount.Focus()
                        Exit Sub
                    End If
                    DISCOUNT = " " & DISCOUNT & " "
                    If InStr(1, DISCOUNT, "CREATE", 1) > 0 Or InStr(1, DISCOUNT, "DELETE", 1) > 0 Or InStr(1, DISCOUNT, "DROP", 1) > 0 Or InStr(1, DISCOUNT, "INSERT", 1) > 1 Or InStr(1, DISCOUNT, "TRACK", 1) > 1 Or InStr(1, DISCOUNT, "TRACE", 1) > 1 Then
                        lbl_Error.Text = "Do Not Use Reserve Words!"
                        lbl_msg.Text = ""
                        txt_Bill_Discount.Focus()
                        Exit Sub
                    End If
                    DISCOUNT = TrimX(DISCOUNT)

                    c = 0
                    counter10 = 0
                    For iloop = 1 To Len(DISCOUNT.ToString)
                        strcurrentchar = Mid(DISCOUNT, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>()=%abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ,;:_", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter10 = 1
                            End If
                        End If
                    Next
                    If counter10 = 1 Then
                        lbl_Error.Text = "Do Not Use Un-Wanted Characters!"
                        lbl_msg.Text = ""
                        txt_Bill_Discount.Focus()
                        Exit Sub
                    End If
                Else
                    DISCOUNT = 0
                End If

                'Server Validation for Amount After Discount
                Dim LESSDISCOUNT_AMOUNT As Object = Nothing
                If txt_Bill_AmountAfterDiscount.Text <> "" Then
                    LESSDISCOUNT_AMOUNT = TrimX(txt_Bill_AmountAfterDiscount.Text)
                    LESSDISCOUNT_AMOUNT = RemoveQuotes(LESSDISCOUNT_AMOUNT)
                    If LESSDISCOUNT_AMOUNT.ToString.Length > 15 Then
                        lbl_Error.Text = "Plz Enter Data with Proper Length!"
                        lbl_msg.Text = ""
                        txt_Bill_AmountAfterDiscount.Focus()
                        Exit Sub
                    End If
                    LESSDISCOUNT_AMOUNT = " " & LESSDISCOUNT_AMOUNT & " "
                    If InStr(1, LESSDISCOUNT_AMOUNT, "CREATE", 1) > 0 Or InStr(1, LESSDISCOUNT_AMOUNT, "DELETE", 1) > 0 Or InStr(1, LESSDISCOUNT_AMOUNT, "DROP", 1) > 0 Or InStr(1, LESSDISCOUNT_AMOUNT, "INSERT", 1) > 1 Or InStr(1, LESSDISCOUNT_AMOUNT, "TRACK", 1) > 1 Or InStr(1, LESSDISCOUNT_AMOUNT, "TRACE", 1) > 1 Then
                        lbl_Error.Text = "Do Not Use Reserve Words!"
                        lbl_msg.Text = ""
                        txt_Bill_AmountAfterDiscount.Focus()
                        Exit Sub
                    End If
                    LESSDISCOUNT_AMOUNT = TrimX(LESSDISCOUNT_AMOUNT)
                    c = 0
                    counter11 = 0
                    For iloop = 1 To Len(LESSDISCOUNT_AMOUNT.ToString)
                        strcurrentchar = Mid(LESSDISCOUNT_AMOUNT, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>()=%abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ,;:_", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter11 = 1
                            End If
                        End If
                    Next
                    If counter11 = 1 Then
                        lbl_Error.Text = "Do Not Use Un-Wanted Characters!"
                        lbl_msg.Text = ""
                        txt_Bill_AmountAfterDiscount.Focus()
                        Exit Sub
                    End If
                Else
                    lbl_Error.Text = "Plz Enter Gross Amount Billed!"
                    lbl_msg.Text = ""
                    Me.txt_Bill_AmountAfterDiscount.Focus()
                    Exit Sub
                End If

                'Server Validation for OTHER_CHARGES
                Dim OTHER_CHARGES As Object = Nothing
                If txt_Bill_OtherCharges.Text <> "" Then
                    OTHER_CHARGES = TrimX(txt_Bill_OtherCharges.Text)
                    OTHER_CHARGES = RemoveQuotes(OTHER_CHARGES)
                    If OTHER_CHARGES.ToString.Length > 15 Then
                        lbl_Error.Text = "Plz Enter Data with Proper Length!"
                        lbl_msg.Text = ""
                        txt_Bill_OtherCharges.Focus()
                        Exit Sub
                    End If
                    OTHER_CHARGES = " " & OTHER_CHARGES & " "
                    If InStr(1, OTHER_CHARGES, "CREATE", 1) > 0 Or InStr(1, OTHER_CHARGES, "DELETE", 1) > 0 Or InStr(1, OTHER_CHARGES, "DROP", 1) > 0 Or InStr(1, OTHER_CHARGES, "INSERT", 1) > 1 Or InStr(1, OTHER_CHARGES, "TRACK", 1) > 1 Or InStr(1, OTHER_CHARGES, "TRACE", 1) > 1 Then
                        lbl_Error.Text = "Do Not Use Reserve Words!"
                        lbl_msg.Text = ""
                        txt_Bill_OtherCharges.Focus()
                        Exit Sub
                    End If
                    OTHER_CHARGES = TrimX(OTHER_CHARGES)
                    c = 0
                    counter12 = 0
                    For iloop = 1 To Len(OTHER_CHARGES.ToString)
                        strcurrentchar = Mid(OTHER_CHARGES, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>()=%abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ,;:_", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter12 = 1
                            End If
                        End If
                    Next
                    If counter12 = 1 Then
                        lbl_Error.Text = "Do Not Use Un-Wanted Characters!"
                        lbl_msg.Text = ""
                        txt_Bill_OtherCharges.Focus()
                        Exit Sub
                    End If
                Else
                    OTHER_CHARGES = 0
                End If

                'Server Validation for BILLED_AMOUNT
                Dim BILLED_AMOUNT As Object = Nothing
                If txt_Bill_AmountBilled.Text <> "" Then
                    BILLED_AMOUNT = TrimX(txt_Bill_AmountBilled.Text)
                    BILLED_AMOUNT = RemoveQuotes(BILLED_AMOUNT)
                    If OTHER_CHARGES.ToString.Length > 15 Then
                        lbl_Error.Text = "Plz Enter Data with Proper Length!"
                        lbl_msg.Text = ""
                        txt_Bill_AmountBilled.Focus()
                        Exit Sub
                    End If
                    BILLED_AMOUNT = " " & BILLED_AMOUNT & " "
                    If InStr(1, BILLED_AMOUNT, "CREATE", 1) > 0 Or InStr(1, BILLED_AMOUNT, "DELETE", 1) > 0 Or InStr(1, BILLED_AMOUNT, "DROP", 1) > 0 Or InStr(1, BILLED_AMOUNT, "INSERT", 1) > 1 Or InStr(1, BILLED_AMOUNT, "TRACK", 1) > 1 Or InStr(1, BILLED_AMOUNT, "TRACE", 1) > 1 Then
                        lbl_Error.Text = "Do Not Use Reserve Words!"
                        lbl_msg.Text = ""
                        txt_Bill_AmountBilled.Focus()
                        Exit Sub
                    End If
                    BILLED_AMOUNT = TrimX(BILLED_AMOUNT)
                    c = 0
                    counter13 = 0
                    For iloop = 1 To Len(BILLED_AMOUNT.ToString)
                        strcurrentchar = Mid(BILLED_AMOUNT, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>()=%abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ,;:_", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter13 = 1
                            End If
                        End If
                    Next
                    If counter13 = 1 Then
                        lbl_Error.Text = "Do Not Use Un-Wanted Characters!"
                        lbl_msg.Text = ""
                        txt_Bill_AmountBilled.Focus()
                        Exit Sub
                    End If
                Else
                    lbl_Error.Text = "Plz Enter Amount Billed!"
                    lbl_msg.Text = ""
                    Me.txt_Bill_AmountBilled.Focus()
                    Exit Sub
                End If

                'Server Validation for DEDUCTION
                Dim DEDUCTION As Object = Nothing
                If txt_Bill_Deduction.Text <> "" Then
                    DEDUCTION = TrimX(txt_Bill_Deduction.Text)
                    DEDUCTION = RemoveQuotes(DEDUCTION)
                    If OTHER_CHARGES.ToString.Length > 15 Then
                        lbl_Error.Text = "Plz Enter Data with Proper Length!"
                        lbl_msg.Text = ""
                        txt_Bill_Deduction.Focus()
                        Exit Sub
                    End If
                    DEDUCTION = " " & DEDUCTION & " "
                    If InStr(1, DEDUCTION, "CREATE", 1) > 0 Or InStr(1, DEDUCTION, "DELETE", 1) > 0 Or InStr(1, DEDUCTION, "DROP", 1) > 0 Or InStr(1, DEDUCTION, "INSERT", 1) > 1 Or InStr(1, DEDUCTION, "TRACK", 1) > 1 Or InStr(1, DEDUCTION, "TRACE", 1) > 1 Then
                        lbl_Error.Text = "Do Not Use Reserve Words!"
                        lbl_msg.Text = ""
                        txt_Bill_Deduction.Focus()
                        Exit Sub
                    End If
                    DEDUCTION = TrimX(DEDUCTION)
                    c = 0
                    counter14 = 0
                    For iloop = 1 To Len(DEDUCTION.ToString)
                        strcurrentchar = Mid(DEDUCTION, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>()=%abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ,;:_", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter14 = 1
                            End If
                        End If
                    Next
                    If counter14 = 1 Then
                        lbl_Error.Text = "Do Not Use Un-Wanted Characters!"
                        lbl_msg.Text = ""
                        txt_Bill_Deduction.Focus()
                        Exit Sub
                    End If
                Else
                    DEDUCTION = 0
                End If

                'Server Validation for AMOUNT_PASSED
                Dim AMOUNT_PASSED As Object = Nothing
                If txt_Bill_AmountPassed.Text <> "" Then
                    AMOUNT_PASSED = TrimX(txt_Bill_AmountPassed.Text)
                    AMOUNT_PASSED = RemoveQuotes(AMOUNT_PASSED)
                    If AMOUNT_PASSED.ToString.Length > 15 Then
                        lbl_Error.Text = "Plz Enter Data with Proper Length!"
                        lbl_msg.Text = ""
                        txt_Bill_AmountPassed.Focus()
                        Exit Sub
                    End If
                    AMOUNT_PASSED = " " & AMOUNT_PASSED & " "
                    If InStr(1, AMOUNT_PASSED, "CREATE", 1) > 0 Or InStr(1, AMOUNT_PASSED, "DELETE", 1) > 0 Or InStr(1, AMOUNT_PASSED, "DROP", 1) > 0 Or InStr(1, AMOUNT_PASSED, "INSERT", 1) > 1 Or InStr(1, AMOUNT_PASSED, "TRACK", 1) > 1 Or InStr(1, AMOUNT_PASSED, "TRACE", 1) > 1 Then
                        lbl_Error.Text = "Do Not Use Reserve Words!"
                        lbl_msg.Text = ""
                        txt_Bill_AmountPassed.Focus()
                        Exit Sub
                    End If
                    AMOUNT_PASSED = TrimX(AMOUNT_PASSED)
                    c = 0
                    counter15 = 0
                    For iloop = 1 To Len(AMOUNT_PASSED.ToString)
                        strcurrentchar = Mid(AMOUNT_PASSED, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>()=%abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ,;:_", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter15 = 1
                            End If
                        End If
                    Next
                    If counter15 = 1 Then
                        lbl_Error.Text = "Do Not Use Un-Wanted Characters!"
                        lbl_msg.Text = ""
                        txt_Bill_AmountPassed.Focus()
                        Exit Sub
                    End If
                Else
                    lbl_Error.Text = "Plz Enter Amount Passed After discount adn deduction!"
                    lbl_msg.Text = ""
                    Me.txt_Bill_AmountPassed.Focus()
                    Exit Sub
                End If

                'Server Validation for REMARKS
                Dim REMARKS As Object = Nothing
                If txt_Bill_Remarks.Text <> "" Then
                    REMARKS = txt_Bill_Remarks.Text
                    REMARKS = RemoveQuotes(REMARKS)

                    If REMARKS.ToString.Length > 250 Then
                        lbl_Error.Text = "Plz Enter Data with Proper Length!"
                        lbl_msg.Text = ""
                        txt_Bill_Remarks.Focus()
                        Exit Sub
                    End If

                Else
                    REMARKS = ""
                End If

                Dim LIB_CODE As Object = Nothing
                If Session.Item("LoggedLibcode") <> "" Then
                    LIB_CODE = Session.Item("LoggedLibcode")
                Else
                    lbl_Error.Text = "Plz Enter Data with Proper Length!"
                    lbl_msg.Text = ""
                    Exit Sub
                End If

                Dim USER_CODE As Object = Nothing
                USER_CODE = Session.Item("LoggedUser")

                Dim DATE_ADDED As Object = Nothing
                DATE_ADDED = Now.Date
                DATE_ADDED = Convert.ToDateTime(DATE_ADDED, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                Dim STATUS As Object = Nothing
                STATUS = "New"

                Dim IP As Object = Nothing

                IP = Request.UserHostAddress.Trim

                '********************************************************************************************
                If SqlConn.State = 0 Then
                    SqlConn.Open()
                End If
                thisTransaction = SqlConn.BeginTransaction()
                Dim intValue As Integer = 0
                Dim objCommand As New SqlCommand
                objCommand.Connection = SqlConn
                objCommand.Transaction = thisTransaction
                objCommand.CommandType = CommandType.Text
                objCommand.CommandText = "INSERT INTO BILLS (INV_NO, INV_DATE, INV_AMOUNT, CUR_CODE, CONVERSION_RATE, RS_AMOUNT, DISCOUNT, LESSDISCOUNT_AMOUNT, OTHER_CHARGES, BILLED_AMOUNT, SUPPLEMENT, DEDUCTION, AMOUNT_PASSED, BUDG_ID, STATUS, VEND_ID,  REMARKS, DATE_ADDED, USER_CODE, LIB_CODE, IP) " & _
                                 " VALUES (@INV_NO, @INV_DATE, @INV_AMOUNT, @CUR_CODE, @CONVERSION_RATE, @RS_AMOUNT, @DISCOUNT, @LESSDISCOUNT_AMOUNT, @OTHER_CHARGES, @BILLED_AMOUNT, @SUPPLEMENT, @DEDUCTION, @AMOUNT_PASSED, @BUDG_ID, @STATUS, @VEND_ID,  @REMARKS, @DATE_ADDED, @USER_CODE, @LIB_CODE, @IP);  "


                If LIB_CODE = "" Then LIB_CODE = System.DBNull.Value
                objCommand.Parameters.Add("@LIB_CODE", SqlDbType.NVarChar)
                objCommand.Parameters("@LIB_CODE").Value = LIB_CODE

                If INV_NO = "" Then INV_NO = System.DBNull.Value
                objCommand.Parameters.Add("@INV_NO", SqlDbType.NVarChar)
                objCommand.Parameters("@INV_NO").Value = INV_NO

                If INV_DATE = "" Then INV_DATE = System.DBNull.Value
                objCommand.Parameters.Add("@INV_DATE", SqlDbType.DateTime)
                objCommand.Parameters("@INV_DATE").Value = INV_DATE

                If INV_AMOUNT = 0 Then INV_AMOUNT = System.DBNull.Value
                objCommand.Parameters.Add("@INV_AMOUNT", SqlDbType.Decimal)
                objCommand.Parameters("@INV_AMOUNT").Value = INV_AMOUNT

                If CUR_CODE = "" Then CUR_CODE = System.DBNull.Value
                objCommand.Parameters.Add("@CUR_CODE", SqlDbType.NVarChar)
                objCommand.Parameters("@CUR_CODE").Value = CUR_CODE

                If CONVERSION_RATE = 0 Then CONVERSION_RATE = System.DBNull.Value
                objCommand.Parameters.Add("@CONVERSION_RATE", SqlDbType.Decimal)
                objCommand.Parameters("@CONVERSION_RATE").Value = CONVERSION_RATE

                If RS_AMOUNT = 0 Then RS_AMOUNT = System.DBNull.Value
                objCommand.Parameters.Add("@RS_AMOUNT", SqlDbType.Decimal)
                objCommand.Parameters("@RS_AMOUNT").Value = RS_AMOUNT

                objCommand.Parameters.Add("@DISCOUNT", SqlDbType.Decimal)
                If DISCOUNT = 0 Then
                    objCommand.Parameters("@DISCOUNT").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@DISCOUNT").Value = DISCOUNT
                End If

                If LESSDISCOUNT_AMOUNT = 0 Then LESSDISCOUNT_AMOUNT = System.DBNull.Value
                objCommand.Parameters.Add("@LESSDISCOUNT_AMOUNT", SqlDbType.Decimal)
                objCommand.Parameters("@LESSDISCOUNT_AMOUNT").Value = LESSDISCOUNT_AMOUNT

                If OTHER_CHARGES = 0 Then OTHER_CHARGES = System.DBNull.Value
                objCommand.Parameters.Add("@OTHER_CHARGES", SqlDbType.Decimal)
                objCommand.Parameters("@OTHER_CHARGES").Value = OTHER_CHARGES

                If BILLED_AMOUNT = 0 Then BILLED_AMOUNT = System.DBNull.Value
                objCommand.Parameters.Add("@BILLED_AMOUNT", SqlDbType.Decimal)
                objCommand.Parameters("@BILLED_AMOUNT").Value = BILLED_AMOUNT

                If SUPPLEMENT = "" Then SUPPLEMENT = System.DBNull.Value
                objCommand.Parameters.Add("@SUPPLEMENT", SqlDbType.VarChar)
                objCommand.Parameters("@SUPPLEMENT").Value = SUPPLEMENT

                If DEDUCTION = 0 Then DEDUCTION = System.DBNull.Value
                objCommand.Parameters.Add("@DEDUCTION", SqlDbType.Decimal)
                objCommand.Parameters("@DEDUCTION").Value = DEDUCTION

                If AMOUNT_PASSED = 0 Then AMOUNT_PASSED = System.DBNull.Value
                objCommand.Parameters.Add("@AMOUNT_PASSED", SqlDbType.Decimal)
                objCommand.Parameters("@AMOUNT_PASSED").Value = AMOUNT_PASSED

                objCommand.Parameters.Add("@BUDG_ID", SqlDbType.Int)
                If BUDG_ID = 0 Then
                    objCommand.Parameters("@BUDG_ID").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@BUDG_ID").Value = BUDG_ID
                End If

                objCommand.Parameters.Add("@VEND_ID", SqlDbType.Int)
                If VEND_ID = 0 Then
                    objCommand.Parameters("@VEND_ID").Value = System.DBNull.Value
                Else
                    objCommand.Parameters("@VEND_ID").Value = VEND_ID
                End If

                If STATUS = "" Then STATUS = System.DBNull.Value
                objCommand.Parameters.Add("@STATUS", SqlDbType.VarChar)
                objCommand.Parameters("@STATUS").Value = STATUS

                If REMARKS = "" Then REMARKS = System.DBNull.Value
                objCommand.Parameters.Add("@REMARKS", SqlDbType.NVarChar)
                objCommand.Parameters("@REMARKS").Value = REMARKS

                If DATE_ADDED = "" Then DATE_ADDED = System.DBNull.Value
                objCommand.Parameters.Add("@DATE_ADDED", SqlDbType.DateTime)
                objCommand.Parameters("@DATE_ADDED").Value = DATE_ADDED

                If USER_CODE = "" Then USER_CODE = System.DBNull.Value
                objCommand.Parameters.Add("@USER_CODE", SqlDbType.NVarChar)
                objCommand.Parameters("@USER_CODE").Value = USER_CODE

                If IP = "" Then IP = System.DBNull.Value
                objCommand.Parameters.Add("@IP", SqlDbType.VarChar)
                objCommand.Parameters("@IP").Value = IP

                objCommand.ExecuteNonQuery()
                thisTransaction.Commit()
                SqlConn.Close()

                lbl_Error.Text = ""
                lbl_msg.Text = "Data Added Successfully!"
                Bill_Calculate_Bttn.Visible = True
                Me.Bill_Save_Bttn.Visible = False
                Bill_Update_Bttn.Visible = False
                ClearBillFields()
                PopulateBillsGrid()
                DDL_BudgYear.Focus()
            Catch q As SqlException
            thisTransaction.Rollback()
                lbl_Error.Text = "Database Error -SAVE: " & (q.Message() & ", " & q.Number & ", " & q.GetType.ToString)
                lbl_msg.Text = ""
        Catch ex As Exception
                lbl_Error.Text = "Error-SAVE: " & (ex.Message())
                lbl_msg.Text = ""
        Finally
            SqlConn.Close()
        End Try
        End If
    End Sub
    'populate acq records of a title
    Public Sub PopulateBillsGrid()
        Dim dtSearch As DataTable = Nothing
        Try

            Dim SQL As String = Nothing
            SQL = " SELECT BILLS.BILL_ID, BILLS.INV_NO, BILLS.INV_DATE, BILLS.INV_AMOUNT, BILLS.CUR_CODE, BILLS.CONVERSION_RATE, BILLS.RS_AMOUNT, BILLS.DISCOUNT, BILLS.LESSDISCOUNT_AMOUNT, BILLS.OTHER_CHARGES, BILLS.BILLED_AMOUNT,BILLS.DEDUCTION, BILLS.AMOUNT_PASSED, BILLS.STATUS, VENDORS.VEND_NAME FROM BILLS LEFT OUTER JOIN VENDORS ON BILLS.VEND_ID = VENDORS.VEND_ID WHERE (BILLS.STATUS='New') AND (BILLS.LIB_CODE = '" & Trim(LibCode) & "')  ORDER BY BILLS.BILL_ID DESC"

            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            da.Fill(ds)

            dtSearch = ds.Tables(0).Copy
            Dim RecordCount As Long = 0
            If dtSearch.Rows.Count = 0 Then
                Me.Grid1.DataSource = Nothing
                Grid1.DataBind()
                Label4.Text = "Total Record(s): 0 "
                Bill_Calculate_Bttn.Visible = True
                Bill_DeleteAll_Bttn.Visible = False
                Bill_Save_Bttn.Visible = False
                Bill_Update_Bttn.Visible = False
            Else
                Grid1.Visible = True
                RecordCount = dtSearch.Rows.Count
                Grid1.DataSource = dtSearch
                Grid1.DataBind()
                Label4.Text = "Total Record(s): " & RecordCount
                Bill_DeleteAll_Bttn.Visible = True
                Bill_Save_Bttn.Visible = False
                Bill_Calculate_Bttn.Visible = True
                Bill_Update_Bttn.Visible = False
            End If
            SqlConn.Close()
            ViewState("dt") = dtSearch
        Catch s As Exception
            lbl_Error.Text = "Error-SAVE: " & (s.Message())
            lbl_msg.Text = ""
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
            lbl_Error.Text = "Error- There is error in page index "
            lbl_msg.Text = ""
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
    Protected Sub Grid1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles Grid1.Sorting
        Dim temp As DataTable = CType(ViewState("dt"), DataTable)
        GridViewSortExpression = e.SortExpression
        Grid1.DataSource = temp
        Dim pageIndex As Integer = Grid1.PageIndex
        Grid1.DataSource = SortDataTable(Grid1.DataSource, False)
        Grid1.DataBind()
        Grid1.PageIndex = pageIndex
    End Sub
    Protected Sub Grid1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Grid1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Lblsr As Label = e.Row.FindControl("lblsr")
            Srno = index + 1
            If Lblsr Is Nothing = False Then
                Lblsr.Text = Srno
                index += 1
            End If
        End If
    End Sub
    Private Shared Function t() As Object
        Throw New NotImplementedException
    End Function
    'get value of row from grid
    Private Sub Grid1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles Grid1.RowCommand
        Dim Command As SqlCommand = Nothing
        Dim dr As SqlDataReader = Nothing
        Dim myBudgID As Integer = Nothing
        Try
            If e.CommandName = "Select" Then
                Dim myRowID, BILL_ID As Integer
                myRowID = e.CommandArgument.ToString()
                BILL_ID = Grid1.DataKeys(myRowID).Value
                If Not String.IsNullOrEmpty(BILL_ID) And BILL_ID <> 0 Then
                    Label23.Text = BILL_ID
                    Dim iloop As Integer
                    Dim strcurrentchar As Object
                    Dim c As Integer
                    Dim counter1 As Integer

                    BILL_ID = TrimX(BILL_ID)
                    BILL_ID = UCase(BILL_ID)

                    BILL_ID = RemoveQuotes(BILL_ID)
                    If Len(BILL_ID).ToString > 10 Then
                        lbl_Error.Text = "Length of Input is not Proper!"
                        lbl_msg.Text = ""
                        Exit Sub
                    End If

                    If IsNumeric(BILL_ID) = False Then
                        lbl_Error.Text = "Plz Enter Numeric Value Only!"
                        lbl_msg.Text = ""
                        txt_Bill_Discount.Focus()
                        Exit Sub
                    End If

                    BILL_ID = " " & BILL_ID & " "
                    If InStr(1, BILL_ID, " CREATE ", 1) > 0 Or InStr(1, BILL_ID, " DELETE ", 1) > 0 Or InStr(1, BILL_ID, " DROP ", 1) > 0 Or InStr(1, BILL_ID, " INSERT ", 1) > 1 Or InStr(1, BILL_ID, " TRACK ", 1) > 1 Or InStr(1, BILL_ID, " TRACE ", 1) > 1 Then
                        lbl_Error.Text = "Do not use reserve words... !"
                        lbl_msg.Text = ""
                        Exit Sub
                    End If
                    BILL_ID = TrimX(BILL_ID)

                    'get record details from database
                    Dim SQL As String = Nothing
                    SQL = " SELECT *  FROM BILLS WHERE (BILL_ID = '" & Trim(BILL_ID) & "') "
                      Command = New SqlCommand(SQL, SqlConn)
                    SqlConn.Open()
                    dr = Command.ExecuteReader(CommandBehavior.CloseConnection)
                    dr.Read()
                    If dr.HasRows = True Then
                        If dr.Item("BUDG_ID").ToString <> "" Then
                            myBudgID = dr.Item("BUDG_ID").ToString
                        Else
                            myBudgID = 0
                        End If

                        If dr.Item("SUPPLEMENT").ToString <> "" Then
                            DDL_BudgSupp.SelectedValue = dr.Item("SUPPLEMENT").ToString
                        Else
                            DDL_BudgSupp.Items.Clear()
                        End If

                        If dr.Item("INV_NO").ToString <> "" Then
                            txt_Bill_BillNo.Text = dr.Item("INV_NO").ToString
                        Else
                            txt_Bill_BillNo.Text = ""
                        End If

                        If dr.Item("INV_DATE").ToString <> "" Then
                            txt_Bill_BillDate.Text = Format(dr.Item("INV_DATE"), "dd/MM/yyyy").ToString
                        Else
                            txt_Bill_BillDate.Text = ""
                        End If

                        If dr.Item("CUR_CODE").ToString <> "" Then
                            DDL_Currencies.SelectedValue = dr.Item("CUR_CODE").ToString
                        Else
                            DDL_Currencies.Items.Clear()
                        End If

                        If dr.Item("INV_AMOUNT").ToString <> "" Then
                            txt_Bill_Amount.Text = dr.Item("INV_AMOUNT").ToString
                        Else
                            txt_Bill_Amount.Text = ""
                        End If

                        If dr.Item("CONVERSION_RATE").ToString <> "" Then
                            txt_Bill_ConversionRate.Text = dr.Item("CONVERSION_RATE").ToString
                        Else
                            txt_Bill_ConversionRate.Text = ""
                        End If

                        If dr.Item("RS_AMOUNT").ToString <> "" Then
                            txt_Bill_RsAmount.Text = dr.Item("RS_AMOUNT").ToString
                        Else
                            txt_Bill_RsAmount.Text = ""
                        End If

                        If dr.Item("DISCOUNT").ToString <> "" Then
                            txt_Bill_Discount.Text = dr.Item("DISCOUNT").ToString
                        Else
                            txt_Bill_Discount.Text = ""
                        End If

                        If dr.Item("LESSDISCOUNT_AMOUNT").ToString <> "" Then
                            txt_Bill_AmountAfterDiscount.Text = dr.Item("LESSDISCOUNT_AMOUNT").ToString
                        Else
                            txt_Bill_AmountAfterDiscount.Text = ""
                        End If

                        If dr.Item("OTHER_CHARGES").ToString <> "" Then
                            txt_Bill_OtherCharges.Text = dr.Item("OTHER_CHARGES").ToString
                        Else
                            txt_Bill_OtherCharges.Text = ""
                        End If

                        If dr.Item("BILLED_AMOUNT").ToString <> "" Then
                            txt_Bill_AmountBilled.Text = dr.Item("BILLED_AMOUNT").ToString
                        Else
                            txt_Bill_AmountBilled.Text = ""
                        End If

                        If dr.Item("DEDUCTION").ToString <> "" Then
                            txt_Bill_Deduction.Text = dr.Item("DEDUCTION").ToString
                        Else
                            txt_Bill_Deduction.Text = ""
                        End If

                        If dr.Item("AMOUNT_PASSED").ToString <> "" Then
                            txt_Bill_AmountPassed.Text = dr.Item("AMOUNT_PASSED").ToString
                        Else
                            txt_Bill_AmountPassed.Text = ""
                        End If

                        If dr.Item("REMARKS").ToString <> "" Then
                            txt_Bill_Remarks.Text = dr.Item("REMARKS").ToString
                        Else
                            txt_Bill_Remarks.Text = ""
                        End If

                        If dr.Item("VEND_ID").ToString <> "" Then
                            DDL_Vendors.SelectedValue = dr.Item("VEND_ID").ToString
                        Else
                            DDL_Vendors.Items.Clear()
                        End If

                        lbl_msg.Text = "Press CALCULATE Button to save the Changes if any.."
                        lbl_Error.Text = ""
                        dr.Close()
                        SqlConn.Close()

                        'populate budget head and bind
                        Dim SQL2 As String = Nothing
                        Dim dtBudgHead As DataTable = Nothing

                        SQL2 = "SELECT BUDG_ID, BUDG_HEAD FROM BUDGETS WHERE (LIB_CODE ='" & Trim(LibCode) & "') AND (BUDG_ID = '" & Trim(myBudgID) & "') "
                        Dim ds As New DataSet
                        Dim da As New SqlDataAdapter(SQL2, SqlConn)
                        If SqlConn.State = 0 Then
                            SqlConn.Open()
                        End If
                        da.Fill(ds)

                        dtBudgHead = ds.Tables(0).Copy
                        If dtBudgHead.Rows.Count = 0 Then
                            Me.DDL_BudgHead.DataSource = Nothing
                            DDL_BudgHead.DataBind()
                        Else
                            Me.DDL_BudgHead.DataSource = dtBudgHead
                            Me.DDL_BudgHead.DataTextField = "BUDG_HEAD"
                            Me.DDL_BudgHead.DataValueField = "BUDG_ID"
                            Me.DDL_BudgHead.DataBind()
                            DDL_BudgHead.SelectedValue = myBudgID
                        End If
                    Else
                        Label4.Text = ""
                        lbl_Error.Text = "Record Not Selected for EDIT!"
                        lbl_msg.Text = ""
                        Label23.Text = ""
                    End If
                Else
                    lbl_Error.Text = "Record Not Selected for EDIT!"
                    lbl_msg.Text = ""
                    Label23.Text = ""
                End If
            End If
            Bill_Update_Bttn.Visible = False
            Bill_Calculate_Bttn.Visible = True
            Bill_Save_Bttn.Visible = False
        Catch s As Exception
            lbl_Error.Text = "Error: " & (s.Message())
            lbl_msg.Text = ""
            Label23.Text = ""
        Finally
            SqlConn.Close()
        End Try
    End Sub 'Grid1_ItemCommand
    'update record
    Protected Sub Bill_Update_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Bill_Update_Bttn.Click
        Dim SQL As String = Nothing
        Dim da As SqlDataAdapter
        Dim cb As SqlCommandBuilder
        Dim ds As New DataSet
        Dim dt As New DataTable
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing
        Try
            If IsPostBack = True Then
                'Server Validation for Lib Code
                Dim iloop As Integer
                Dim strcurrentchar As Object
                Dim c As Integer
                Dim counter1, counter2, counter3, counter4, counter5, counter6, counter7, counter8, counter9, counter10, counter11, counter12, counter13, counter14, counter15 As Integer
                '****************************************************************************************************
                'Server Validation for BUDG_HEAD
                Dim BILL_ID As Integer = Nothing
                If Label23.Text <> "" Then
                    BILL_ID = Trim(Label23.Text)
                    BILL_ID = RemoveQuotes(BILL_ID)

                    If IsNumeric(BILL_ID) = False Then
                        lbl_Error.Text = "Plz Select Record to Edit!"
                        lbl_msg.Text = ""
                        Exit Sub
                    End If
                    If BILL_ID.ToString.Length > 10 Then
                        lbl_Error.Text = "Plz Enter Data with Proper Length!"
                        lbl_msg.Text = ""
                        Exit Sub
                    End If
                    BILL_ID = " " & BILL_ID & " "
                    If InStr(1, BILL_ID, "CREATE", 1) > 0 Or InStr(1, BILL_ID, "DELETE", 1) > 0 Or InStr(1, BILL_ID, "DROP", 1) > 0 Or InStr(1, BILL_ID, "INSERT", 1) > 1 Or InStr(1, BILL_ID, "TRACK", 1) > 1 Or InStr(1, BILL_ID, "TRACE", 1) > 1 Then
                        lbl_Error.Text = "Do Not Use Reserve Words!"
                        lbl_msg.Text = ""
                        Exit Sub
                    End If
                    BILL_ID = TrimX(BILL_ID)

                    c = 0
                    counter1 = 0
                    For iloop = 1 To Len(BILL_ID.ToString)
                        strcurrentchar = Mid(BILL_ID, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter1 = 1
                            End If
                        End If
                    Next
                    If counter1 = 1 Then
                        lbl_Error.Text = "Do Not Use Un-Wanted Characters!"
                        lbl_msg.Text = ""
                        Exit Sub
                    End If
                Else
                    lbl_Error.Text = "Plz Select Record Record for Edit!"
                    lbl_msg.Text = ""
                    Exit Sub
                End If

                'Server Validation for BUDG_HEAD
                Dim BUDG_ID As Integer = Nothing
                If DDL_BudgHead.Text <> "" Then
                    BUDG_ID = DDL_BudgHead.SelectedValue
                    BUDG_ID = RemoveQuotes(BUDG_ID)

                    If IsNumeric(BUDG_ID) = False Then
                        lbl_Error.Text = "Plz Select Budget Head from Drop-Down!"
                        lbl_msg.Text = ""
                        DDL_BudgHead.Focus()
                        Exit Sub
                    End If
                    If BUDG_ID.ToString.Length > 5 Then
                        lbl_Error.Text = "Plz Enter Data with Proper Length!"
                        lbl_msg.Text = ""
                        DDL_BudgHead.Focus()
                        Exit Sub
                    End If
                    BUDG_ID = " " & BUDG_ID & " "
                    If InStr(1, BUDG_ID, "CREATE", 1) > 0 Or InStr(1, BUDG_ID, "DELETE", 1) > 0 Or InStr(1, BUDG_ID, "DROP", 1) > 0 Or InStr(1, BUDG_ID, "INSERT", 1) > 1 Or InStr(1, BUDG_ID, "TRACK", 1) > 1 Or InStr(1, BUDG_ID, "TRACE", 1) > 1 Then
                        lbl_Error.Text = "Do Not Use Reserve Words!"
                        lbl_msg.Text = ""
                        DDL_BudgHead.Focus()
                        Exit Sub
                    End If
                    BUDG_ID = TrimX(BUDG_ID)

                    c = 0
                    counter1 = 0
                    For iloop = 1 To Len(BUDG_ID.ToString)
                        strcurrentchar = Mid(BUDG_ID, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter1 = 1
                            End If
                        End If
                    Next
                    If counter1 = 1 Then
                        lbl_Error.Text = "Do Not Use Un-Wanted Characters!"
                        lbl_msg.Text = ""
                        DDL_BudgHead.Focus()
                        Exit Sub
                    End If
                Else
                    lbl_Error.Text = "Plz Select Budget Head from Drop-Down!"
                    lbl_msg.Text = ""
                    Me.DDL_BudgHead.Focus()
                    Exit Sub
                End If

                'Server Validation for VENDOR
                Dim VEND_ID As Integer = Nothing
                If DDL_Vendors.Text <> "" Then
                    VEND_ID = DDL_Vendors.SelectedValue
                    VEND_ID = RemoveQuotes(VEND_ID)

                    If IsNumeric(VEND_ID) = False Then
                        lbl_Error.Text = "Plz Select Vendor from Drop-Down!"
                        lbl_msg.Text = ""
                        DDL_Vendors.Focus()
                        Exit Sub
                    End If
                    If VEND_ID.ToString.Length > 5 Then
                        lbl_Error.Text = "Plz Enter Data with Proper Length!"
                        lbl_msg.Text = ""
                        DDL_Vendors.Focus()
                        Exit Sub
                    End If
                    VEND_ID = " " & VEND_ID & " "
                    If InStr(1, VEND_ID, "CREATE", 1) > 0 Or InStr(1, VEND_ID, "DELETE", 1) > 0 Or InStr(1, VEND_ID, "DROP", 1) > 0 Or InStr(1, VEND_ID, "INSERT", 1) > 1 Or InStr(1, VEND_ID, "TRACK", 1) > 1 Or InStr(1, VEND_ID, "TRACE", 1) > 1 Then
                        lbl_Error.Text = "Do Not Use Reserve Words!"
                        lbl_msg.Text = ""
                        DDL_Vendors.Focus()
                        Exit Sub
                    End If
                    VEND_ID = TrimX(VEND_ID)

                    c = 0
                    counter2 = 0
                    For iloop = 1 To Len(VEND_ID.ToString)
                        strcurrentchar = Mid(VEND_ID, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter2 = 1
                            End If
                        End If
                    Next
                    If counter2 = 1 Then
                        lbl_Error.Text = "Do Not Use Un-Wanted Characters!"
                        lbl_msg.Text = ""
                        DDL_Vendors.Focus()
                        Exit Sub
                    End If
                Else
                    lbl_Error.Text = "Plz Select Budget Head from Drop-Down!"
                    lbl_msg.Text = ""
                    Me.DDL_Vendors.Focus()
                    Exit Sub
                End If

                'Server Validation for BUDG_HEAD
                Dim SUPPLEMENT As Object = Nothing
                If DDL_BudgSupp.Text <> "" Then
                    SUPPLEMENT = DDL_BudgSupp.SelectedValue
                    SUPPLEMENT = RemoveQuotes(SUPPLEMENT)

                    If SUPPLEMENT.ToString.Length > 2 Then
                        lbl_Error.Text = "Plz Enter Data with Proper Length!"
                        lbl_msg.Text = ""
                        DDL_BudgSupp.Focus()
                        Exit Sub
                    End If
                    SUPPLEMENT = " " & SUPPLEMENT & " "
                    If InStr(1, SUPPLEMENT, "CREATE", 1) > 0 Or InStr(1, SUPPLEMENT, "DELETE", 1) > 0 Or InStr(1, SUPPLEMENT, "DROP", 1) > 0 Or InStr(1, SUPPLEMENT, "INSERT", 1) > 1 Or InStr(1, SUPPLEMENT, "TRACK", 1) > 1 Or InStr(1, SUPPLEMENT, "TRACE", 1) > 1 Then
                        lbl_Error.Text = "Do Not Use Reserve Words!"
                        lbl_msg.Text = ""
                        DDL_BudgSupp.Focus()
                        Exit Sub
                    End If
                    SUPPLEMENT = TrimX(SUPPLEMENT)

                    c = 0
                    counter3 = 0
                    For iloop = 1 To Len(SUPPLEMENT)
                        strcurrentchar = Mid(SUPPLEMENT, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter3 = 1
                            End If
                        End If
                    Next
                    If counter3 = 1 Then
                        lbl_Error.Text = "Do Not Use Un-Wanted Characters!"
                        lbl_msg.Text = ""
                        DDL_BudgSupp.Focus()
                        Exit Sub
                    End If
                Else
                    SUPPLEMENT = "N"
                End If

                'Server Validation for txt_Bill_BillNo
                Dim INV_NO As Object = Nothing
                If txt_Bill_BillNo.Text <> "" Then
                    INV_NO = TrimX(txt_Bill_BillNo.Text)
                    INV_NO = RemoveQuotes(INV_NO)

                    If INV_NO.ToString.Length > 100 Then
                        lbl_Error.Text = "Plz Enter Data with Proper Length!"
                        lbl_msg.Text = ""
                        txt_Bill_BillNo.Focus()
                        Exit Sub
                    End If
                    INV_NO = " " & INV_NO & " "
                    If InStr(1, INV_NO, "CREATE", 1) > 0 Or InStr(1, INV_NO, "DELETE", 1) > 0 Or InStr(1, INV_NO, "DROP", 1) > 0 Or InStr(1, INV_NO, "INSERT", 1) > 1 Or InStr(1, INV_NO, "TRACK", 1) > 1 Or InStr(1, INV_NO, "TRACE", 1) > 1 Then
                        lbl_Error.Text = "Do Not Use Reserve Words!"
                        lbl_msg.Text = ""
                        txt_Bill_BillNo.Focus()
                        Exit Sub
                    End If
                    INV_NO = TrimX(INV_NO)

                    c = 0
                    counter4 = 0
                    For iloop = 1 To Len(INV_NO)
                        strcurrentchar = Mid(INV_NO, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter4 = 1
                            End If
                        End If
                    Next
                    If counter4 = 1 Then
                        lbl_Error.Text = "Do Not Use Un-Wanted Characters!"
                        lbl_msg.Text = ""
                        txt_Bill_BillNo.Focus()
                        Exit Sub
                    End If

                    'check this bill no status
                    Dim str As Object = Nothing
                    Dim flag As Object = Nothing
                    str = "SELECT INV_NO FROM BILLS WHERE (BILL_ID <> '" & Trim(BILL_ID) & "') AND (INV_NO ='" & Trim(INV_NO) & "'  AND VEND_ID = '" & Trim(VEND_ID) & "' AND LIB_CODE ='" & Trim(LibCode) & "')"
                    Dim cmd1 As New SqlCommand(str, SqlConn)
                    SqlConn.Open()
                    flag = cmd1.ExecuteScalar
                    If flag <> Nothing Then
                        lbl_Error.Text = "This Bill No has already been Processed!"
                        lbl_msg.Text = ""
                        txt_Bill_BillNo.Focus()
                        Exit Sub
                    End If
                    SqlConn.Close()
                Else
                    lbl_Error.Text = "Plz Select Budget Head from Drop-Down!"
                    lbl_msg.Text = ""
                    Me.txt_Bill_BillNo.Focus()
                    Exit Sub
                End If

                'search BILL date
                Dim INV_DATE As Object = Nothing
                If txt_Bill_BillDate.Text <> "" Then
                    INV_DATE = TrimX(txt_Bill_BillDate.Text)
                    INV_DATE = RemoveQuotes(INV_DATE)
                    INV_DATE = Convert.ToDateTime(INV_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                    If Len(INV_DATE) > 12 Then
                        Label15.Text = " Input is not Valid..."
                        Label1.Text = ""
                        Me.txt_Bill_BillDate.Focus()
                        Exit Sub
                    End If
                    INV_DATE = " " & INV_DATE & " "
                    If InStr(1, INV_DATE, "CREATE", 1) > 0 Or InStr(1, INV_DATE, "DELETE", 1) > 0 Or InStr(1, INV_DATE, "DROP", 1) > 0 Or InStr(1, INV_DATE, "INSERT", 1) > 1 Or InStr(1, INV_DATE, "TRACK", 1) > 1 Or InStr(1, INV_DATE, "TRACE", 1) > 1 Then
                        Label15.Text = "  Input is not Valid... "
                        Label1.Text = ""
                        Me.txt_Bill_BillDate.Focus()
                        Exit Sub
                    End If
                    INV_DATE = TrimX(INV_DATE)
                    'check unwanted characters
                    c = 0
                    counter5 = 0
                    For iloop = 1 To Len(INV_DATE)
                        strcurrentchar = Mid(INV_DATE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>=%abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ,;:_", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter5 = 1
                            End If
                        End If
                    Next
                    If counter5 = 1 Then
                        Label15.Text = "data is not Valid... "
                        Label1.Text = ""
                        Me.txt_Bill_BillDate.Focus()
                        Exit Sub
                    End If
                Else
                    INV_DATE = Now.Date
                    INV_DATE = Convert.ToDateTime(INV_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us
                End If

                'Server Validation for CURRENCY
                Dim CUR_CODE As Object = Nothing
                If DDL_Currencies.Text <> "" Then
                    CUR_CODE = DDL_Currencies.SelectedValue
                    CUR_CODE = RemoveQuotes(CUR_CODE)

                    If CUR_CODE.ToString.Length > 4 Then
                        lbl_Error.Text = "Plz Enter Data with Proper Length!"
                        lbl_msg.Text = ""
                        DDL_Currencies.Focus()
                        Exit Sub
                    End If
                    CUR_CODE = " " & CUR_CODE & " "
                    If InStr(1, CUR_CODE, "CREATE", 1) > 0 Or InStr(1, CUR_CODE, "DELETE", 1) > 0 Or InStr(1, CUR_CODE, "DROP", 1) > 0 Or InStr(1, CUR_CODE, "INSERT", 1) > 1 Or InStr(1, CUR_CODE, "TRACK", 1) > 1 Or InStr(1, CUR_CODE, "TRACE", 1) > 1 Then
                        lbl_Error.Text = "Do Not Use Reserve Words!"
                        lbl_msg.Text = ""
                        DDL_Currencies.Focus()
                        Exit Sub
                    End If
                    CUR_CODE = TrimX(CUR_CODE)

                    c = 0
                    counter6 = 0
                    For iloop = 1 To Len(CUR_CODE)
                        strcurrentchar = Mid(CUR_CODE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter6 = 1
                            End If
                        End If
                    Next
                    If counter6 = 1 Then
                        lbl_Error.Text = "Do Not Use Un-Wanted Characters!"
                        lbl_msg.Text = ""
                        DDL_Currencies.Focus()
                        Exit Sub
                    End If
                Else
                    CUR_CODE = "INR"
                End If

                'Server Validation for Gross Amount
                Dim INV_AMOUNT As Object = Nothing
                If txt_Bill_Amount.Text <> "" Then
                    INV_AMOUNT = TrimX(txt_Bill_Amount.Text)
                    INV_AMOUNT = RemoveQuotes(INV_AMOUNT)
                    If INV_AMOUNT.ToString.Length > 15 Then
                        lbl_Error.Text = "Plz Enter Data with Proper Length!"
                        lbl_msg.Text = ""
                        txt_Bill_Amount.Focus()
                        Exit Sub
                    End If
                    INV_AMOUNT = " " & INV_AMOUNT & " "
                    If InStr(1, INV_AMOUNT, "CREATE", 1) > 0 Or InStr(1, INV_AMOUNT, "DELETE", 1) > 0 Or InStr(1, INV_AMOUNT, "DROP", 1) > 0 Or InStr(1, INV_AMOUNT, "INSERT", 1) > 1 Or InStr(1, INV_AMOUNT, "TRACK", 1) > 1 Or InStr(1, INV_AMOUNT, "TRACE", 1) > 1 Then
                        lbl_Error.Text = "Do Not Use Reserve Words!"
                        lbl_msg.Text = ""
                        txt_Bill_Amount.Focus()
                        Exit Sub
                    End If
                    INV_AMOUNT = TrimX(INV_AMOUNT)
                    c = 0
                    counter7 = 0
                    For iloop = 1 To Len(INV_AMOUNT.ToString)
                        strcurrentchar = Mid(INV_AMOUNT, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?()<>=%abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ,;:_", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter7 = 1
                            End If
                        End If
                    Next
                    If counter7 = 1 Then
                        lbl_Error.Text = "Do Not Use Un-Wanted Characters!"
                        lbl_msg.Text = ""
                        txt_Bill_Amount.Focus()
                        Exit Sub
                    End If
                Else
                    lbl_Error.Text = "Plz Enter Gross Amount Billed!"
                    lbl_msg.Text = ""
                    Me.txt_Bill_Amount.Focus()
                    Exit Sub
                End If

                'Server Validation for CONVERSION_RATE
                Dim CONVERSION_RATE As Object = Nothing
                If txt_Bill_ConversionRate.Text <> "" Then
                    CONVERSION_RATE = TrimX(txt_Bill_ConversionRate.Text)
                    CONVERSION_RATE = RemoveQuotes(CONVERSION_RATE)
                    If CONVERSION_RATE.ToString.Length > 15 Then
                        lbl_Error.Text = "Plz Enter Data with Proper Length!"
                        lbl_msg.Text = ""
                        txt_Bill_ConversionRate.Focus()
                        Exit Sub
                    End If
                    CONVERSION_RATE = " " & CONVERSION_RATE & " "
                    If InStr(1, CONVERSION_RATE, "CREATE", 1) > 0 Or InStr(1, CONVERSION_RATE, "DELETE", 1) > 0 Or InStr(1, CONVERSION_RATE, "DROP", 1) > 0 Or InStr(1, CONVERSION_RATE, "INSERT", 1) > 1 Or InStr(1, CONVERSION_RATE, "TRACK", 1) > 1 Or InStr(1, CONVERSION_RATE, "TRACE", 1) > 1 Then
                        lbl_Error.Text = "Do Not Use Reserve Words!"
                        lbl_msg.Text = ""
                        txt_Bill_ConversionRate.Focus()
                        Exit Sub
                    End If
                    CONVERSION_RATE = TrimX(CONVERSION_RATE)
                    c = 0
                    counter8 = 0
                    For iloop = 1 To Len(CONVERSION_RATE.ToString)
                        strcurrentchar = Mid(CONVERSION_RATE, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>()=%abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ,;:_", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter8 = 1
                            End If
                        End If
                    Next
                    If counter8 = 1 Then
                        lbl_Error.Text = "Do Not Use Un-Wanted Characters!"
                        lbl_msg.Text = ""
                        txt_Bill_ConversionRate.Focus()
                        Exit Sub
                    End If
                Else
                    CONVERSION_RATE = 0
                End If

                'Server Validation for RS_AMOUNT
                Dim RS_AMOUNT As Object = Nothing
                If txt_Bill_RsAmount.Text <> "" Then
                    RS_AMOUNT = TrimX(txt_Bill_RsAmount.Text)
                    RS_AMOUNT = RemoveQuotes(RS_AMOUNT)
                    If RS_AMOUNT.ToString.Length > 15 Then
                        lbl_Error.Text = "Plz Enter Data with Proper Length!"
                        lbl_msg.Text = ""
                        txt_Bill_RsAmount.Focus()
                        Exit Sub
                    End If
                    RS_AMOUNT = " " & RS_AMOUNT & " "
                    If InStr(1, RS_AMOUNT, "CREATE", 1) > 0 Or InStr(1, RS_AMOUNT, "DELETE", 1) > 0 Or InStr(1, RS_AMOUNT, "DROP", 1) > 0 Or InStr(1, RS_AMOUNT, "INSERT", 1) > 1 Or InStr(1, RS_AMOUNT, "TRACK", 1) > 1 Or InStr(1, RS_AMOUNT, "TRACE", 1) > 1 Then
                        lbl_Error.Text = "Do Not Use Reserve Words!"
                        lbl_msg.Text = ""
                        txt_Bill_RsAmount.Focus()
                        Exit Sub
                    End If
                    RS_AMOUNT = TrimX(RS_AMOUNT)
                    c = 0
                    counter9 = 0
                    For iloop = 1 To Len(RS_AMOUNT)
                        strcurrentchar = Mid(RS_AMOUNT, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>()=%abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ,;:_", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter9 = 1
                            End If
                        End If
                    Next
                    If counter9 = 1 Then
                        lbl_Error.Text = "Do Not Use Un-Wanted Characters!"
                        lbl_msg.Text = ""
                        txt_Bill_RsAmount.Focus()
                        Exit Sub
                    End If
                Else
                    lbl_Error.Text = "Plz Enter Amount in Rupees Afeter Conversion!"
                    lbl_msg.Text = ""
                    Me.txt_Bill_RsAmount.Focus()
                    Exit Sub
                End If

                'Server Validation for DISCOUNT
                Dim DISCOUNT As Integer = Nothing
                If txt_Bill_Discount.Text <> "" Then
                    DISCOUNT = txt_Bill_Discount.Text
                    DISCOUNT = RemoveQuotes(DISCOUNT)

                    If IsNumeric(DISCOUNT) = False Then
                        lbl_Error.Text = "Plz Enter Numeric Value Only!"
                        lbl_msg.Text = ""
                        txt_Bill_Discount.Focus()
                        Exit Sub
                    End If
                    If DISCOUNT.ToString.Length > 5 Then
                        lbl_Error.Text = "Plz Enter Data with Proper Length!"
                        lbl_msg.Text = ""
                        txt_Bill_Discount.Focus()
                        Exit Sub
                    End If
                    DISCOUNT = " " & DISCOUNT & " "
                    If InStr(1, DISCOUNT, "CREATE", 1) > 0 Or InStr(1, DISCOUNT, "DELETE", 1) > 0 Or InStr(1, DISCOUNT, "DROP", 1) > 0 Or InStr(1, DISCOUNT, "INSERT", 1) > 1 Or InStr(1, DISCOUNT, "TRACK", 1) > 1 Or InStr(1, DISCOUNT, "TRACE", 1) > 1 Then
                        lbl_Error.Text = "Do Not Use Reserve Words!"
                        lbl_msg.Text = ""
                        txt_Bill_Discount.Focus()
                        Exit Sub
                    End If
                    DISCOUNT = TrimX(DISCOUNT)

                    c = 0
                    counter10 = 0
                    For iloop = 1 To Len(DISCOUNT.ToString)
                        strcurrentchar = Mid(DISCOUNT, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>()=%abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ,;:_", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter10 = 1
                            End If
                        End If
                    Next
                    If counter10 = 1 Then
                        lbl_Error.Text = "Do Not Use Un-Wanted Characters!"
                        lbl_msg.Text = ""
                        txt_Bill_Discount.Focus()
                        Exit Sub
                    End If
                Else
                    DISCOUNT = 0
                End If

                'Server Validation for Amount After Discount
                Dim LESSDISCOUNT_AMOUNT As Object = Nothing
                If txt_Bill_AmountAfterDiscount.Text <> "" Then
                    LESSDISCOUNT_AMOUNT = TrimX(txt_Bill_AmountAfterDiscount.Text)
                    LESSDISCOUNT_AMOUNT = RemoveQuotes(LESSDISCOUNT_AMOUNT)
                    If LESSDISCOUNT_AMOUNT.ToString.Length > 15 Then
                        lbl_Error.Text = "Plz Enter Data with Proper Length!"
                        lbl_msg.Text = ""
                        txt_Bill_AmountAfterDiscount.Focus()
                        Exit Sub
                    End If
                    LESSDISCOUNT_AMOUNT = " " & LESSDISCOUNT_AMOUNT & " "
                    If InStr(1, LESSDISCOUNT_AMOUNT, "CREATE", 1) > 0 Or InStr(1, LESSDISCOUNT_AMOUNT, "DELETE", 1) > 0 Or InStr(1, LESSDISCOUNT_AMOUNT, "DROP", 1) > 0 Or InStr(1, LESSDISCOUNT_AMOUNT, "INSERT", 1) > 1 Or InStr(1, LESSDISCOUNT_AMOUNT, "TRACK", 1) > 1 Or InStr(1, LESSDISCOUNT_AMOUNT, "TRACE", 1) > 1 Then
                        lbl_Error.Text = "Do Not Use Reserve Words!"
                        lbl_msg.Text = ""
                        txt_Bill_AmountAfterDiscount.Focus()
                        Exit Sub
                    End If
                    LESSDISCOUNT_AMOUNT = TrimX(LESSDISCOUNT_AMOUNT)
                    c = 0
                    counter11 = 0
                    For iloop = 1 To Len(LESSDISCOUNT_AMOUNT.ToString)
                        strcurrentchar = Mid(LESSDISCOUNT_AMOUNT, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>()=%abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ,;:_", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter11 = 1
                            End If
                        End If
                    Next
                    If counter11 = 1 Then
                        lbl_Error.Text = "Do Not Use Un-Wanted Characters!"
                        lbl_msg.Text = ""
                        txt_Bill_AmountAfterDiscount.Focus()
                        Exit Sub
                    End If
                Else
                    lbl_Error.Text = "Plz Enter Gross Amount Billed!"
                    lbl_msg.Text = ""
                    Me.txt_Bill_AmountAfterDiscount.Focus()
                    Exit Sub
                End If

                'Server Validation for OTHER_CHARGES
                Dim OTHER_CHARGES As Object = Nothing
                If txt_Bill_OtherCharges.Text <> "" Then
                    OTHER_CHARGES = TrimX(txt_Bill_OtherCharges.Text)
                    OTHER_CHARGES = RemoveQuotes(OTHER_CHARGES)
                    If OTHER_CHARGES.ToString.Length > 15 Then
                        lbl_Error.Text = "Plz Enter Data with Proper Length!"
                        lbl_msg.Text = ""
                        txt_Bill_OtherCharges.Focus()
                        Exit Sub
                    End If
                    OTHER_CHARGES = " " & OTHER_CHARGES & " "
                    If InStr(1, OTHER_CHARGES, "CREATE", 1) > 0 Or InStr(1, OTHER_CHARGES, "DELETE", 1) > 0 Or InStr(1, OTHER_CHARGES, "DROP", 1) > 0 Or InStr(1, OTHER_CHARGES, "INSERT", 1) > 1 Or InStr(1, OTHER_CHARGES, "TRACK", 1) > 1 Or InStr(1, OTHER_CHARGES, "TRACE", 1) > 1 Then
                        lbl_Error.Text = "Do Not Use Reserve Words!"
                        lbl_msg.Text = ""
                        txt_Bill_OtherCharges.Focus()
                        Exit Sub
                    End If
                    OTHER_CHARGES = TrimX(OTHER_CHARGES)
                    c = 0
                    counter12 = 0
                    For iloop = 1 To Len(OTHER_CHARGES.ToString)
                        strcurrentchar = Mid(OTHER_CHARGES, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>()=%abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ,;:_", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter12 = 1
                            End If
                        End If
                    Next
                    If counter12 = 1 Then
                        lbl_Error.Text = "Do Not Use Un-Wanted Characters!"
                        lbl_msg.Text = ""
                        txt_Bill_OtherCharges.Focus()
                        Exit Sub
                    End If
                Else
                    OTHER_CHARGES = 0
                End If

                'Server Validation for BILLED_AMOUNT
                Dim BILLED_AMOUNT As Object = Nothing
                If txt_Bill_AmountBilled.Text <> "" Then
                    BILLED_AMOUNT = TrimX(txt_Bill_AmountBilled.Text)
                    BILLED_AMOUNT = RemoveQuotes(BILLED_AMOUNT)
                    If OTHER_CHARGES.ToString.Length > 15 Then
                        lbl_Error.Text = "Plz Enter Data with Proper Length!"
                        lbl_msg.Text = ""
                        txt_Bill_AmountBilled.Focus()
                        Exit Sub
                    End If
                    BILLED_AMOUNT = " " & BILLED_AMOUNT & " "
                    If InStr(1, BILLED_AMOUNT, "CREATE", 1) > 0 Or InStr(1, BILLED_AMOUNT, "DELETE", 1) > 0 Or InStr(1, BILLED_AMOUNT, "DROP", 1) > 0 Or InStr(1, BILLED_AMOUNT, "INSERT", 1) > 1 Or InStr(1, BILLED_AMOUNT, "TRACK", 1) > 1 Or InStr(1, BILLED_AMOUNT, "TRACE", 1) > 1 Then
                        lbl_Error.Text = "Do Not Use Reserve Words!"
                        lbl_msg.Text = ""
                        txt_Bill_AmountBilled.Focus()
                        Exit Sub
                    End If
                    BILLED_AMOUNT = TrimX(BILLED_AMOUNT)
                    c = 0
                    counter13 = 0
                    For iloop = 1 To Len(BILLED_AMOUNT.ToString)
                        strcurrentchar = Mid(BILLED_AMOUNT, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>()=%abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ,;:_", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter13 = 1
                            End If
                        End If
                    Next
                    If counter13 = 1 Then
                        lbl_Error.Text = "Do Not Use Un-Wanted Characters!"
                        lbl_msg.Text = ""
                        txt_Bill_AmountBilled.Focus()
                        Exit Sub
                    End If
                Else
                    lbl_Error.Text = "Plz Enter Amount Billed!"
                    lbl_msg.Text = ""
                    Me.txt_Bill_AmountBilled.Focus()
                    Exit Sub
                End If

                'Server Validation for DEDUCTION
                Dim DEDUCTION As Object = Nothing
                If txt_Bill_Deduction.Text <> "" Then
                    DEDUCTION = TrimX(txt_Bill_Deduction.Text)
                    DEDUCTION = RemoveQuotes(DEDUCTION)
                    If OTHER_CHARGES.ToString.Length > 15 Then
                        lbl_Error.Text = "Plz Enter Data with Proper Length!"
                        lbl_msg.Text = ""
                        txt_Bill_Deduction.Focus()
                        Exit Sub
                    End If
                    DEDUCTION = " " & DEDUCTION & " "
                    If InStr(1, DEDUCTION, "CREATE", 1) > 0 Or InStr(1, DEDUCTION, "DELETE", 1) > 0 Or InStr(1, DEDUCTION, "DROP", 1) > 0 Or InStr(1, DEDUCTION, "INSERT", 1) > 1 Or InStr(1, DEDUCTION, "TRACK", 1) > 1 Or InStr(1, DEDUCTION, "TRACE", 1) > 1 Then
                        lbl_Error.Text = "Do Not Use Reserve Words!"
                        lbl_msg.Text = ""
                        txt_Bill_Deduction.Focus()
                        Exit Sub
                    End If
                    DEDUCTION = TrimX(DEDUCTION)
                    c = 0
                    counter14 = 0
                    For iloop = 1 To Len(DEDUCTION.ToString)
                        strcurrentchar = Mid(DEDUCTION, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>()=%abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ,;:_", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter14 = 1
                            End If
                        End If
                    Next
                    If counter14 = 1 Then
                        lbl_Error.Text = "Do Not Use Un-Wanted Characters!"
                        lbl_msg.Text = ""
                        txt_Bill_Deduction.Focus()
                        Exit Sub
                    End If
                Else
                    DEDUCTION = 0
                End If

                'Server Validation for AMOUNT_PASSED
                Dim AMOUNT_PASSED As Object = Nothing
                If txt_Bill_AmountPassed.Text <> "" Then
                    AMOUNT_PASSED = TrimX(txt_Bill_AmountPassed.Text)
                    AMOUNT_PASSED = RemoveQuotes(AMOUNT_PASSED)
                    If AMOUNT_PASSED.ToString.Length > 15 Then
                        lbl_Error.Text = "Plz Enter Data with Proper Length!"
                        lbl_msg.Text = ""
                        txt_Bill_AmountPassed.Focus()
                        Exit Sub
                    End If
                    AMOUNT_PASSED = " " & AMOUNT_PASSED & " "
                    If InStr(1, AMOUNT_PASSED, "CREATE", 1) > 0 Or InStr(1, AMOUNT_PASSED, "DELETE", 1) > 0 Or InStr(1, AMOUNT_PASSED, "DROP", 1) > 0 Or InStr(1, AMOUNT_PASSED, "INSERT", 1) > 1 Or InStr(1, AMOUNT_PASSED, "TRACK", 1) > 1 Or InStr(1, AMOUNT_PASSED, "TRACE", 1) > 1 Then
                        lbl_Error.Text = "Do Not Use Reserve Words!"
                        lbl_msg.Text = ""
                        txt_Bill_AmountPassed.Focus()
                        Exit Sub
                    End If
                    AMOUNT_PASSED = TrimX(AMOUNT_PASSED)
                    c = 0
                    counter15 = 0
                    For iloop = 1 To Len(AMOUNT_PASSED.ToString)
                        strcurrentchar = Mid(AMOUNT_PASSED, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>()=%abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ,;:_", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter15 = 1
                            End If
                        End If
                    Next
                    If counter15 = 1 Then
                        lbl_Error.Text = "Do Not Use Un-Wanted Characters!"
                        lbl_msg.Text = ""
                        txt_Bill_AmountPassed.Focus()
                        Exit Sub
                    End If
                Else
                    lbl_Error.Text = "Plz Enter Amount Passed After discount adn deduction!"
                    lbl_msg.Text = ""
                    Me.txt_Bill_AmountPassed.Focus()
                    Exit Sub
                End If

                'Server Validation for REMARKS
                Dim REMARKS As Object = Nothing
                If txt_Bill_Remarks.Text <> "" Then
                    REMARKS = txt_Bill_Remarks.Text
                    REMARKS = RemoveQuotes(REMARKS)

                    If REMARKS.ToString.Length > 250 Then
                        lbl_Error.Text = "Plz Enter Data with Proper Length!"
                        lbl_msg.Text = ""
                        txt_Bill_Remarks.Focus()
                        Exit Sub
                    End If

                Else
                    REMARKS = ""
                End If

                Dim LIB_CODE As Object = Nothing
                If Session.Item("LoggedLibcode") <> "" Then
                    LIB_CODE = Session.Item("LoggedLibcode")
                Else
                    lbl_msg.Text = "No Library Code Exists..Login Again  "
                    Label15.Text = ""
                    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('No Library Code Exists..Login Again !');", True)
                    Exit Sub
                End If

                Dim USER_CODE As Object = Nothing
                USER_CODE = Session.Item("LoggedUser")

                Dim DATE_MODIFIED As Object = Nothing
                DATE_MODIFIED = Now.Date
                DATE_MODIFIED = Convert.ToDateTime(DATE_MODIFIED, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                Dim IP As Object = Nothing
                IP = Request.UserHostAddress.Trim

                'UPDATE THE LIBRARY PROFILE  
                If Label4.Text <> "" Then
                    SQL = "SELECT * FROM BILLS WHERE (BILL_ID='" & Trim(BILL_ID) & "')"
                    SqlConn.Open()
                    da = New SqlDataAdapter(SQL, SqlConn)
                    cb = New SqlCommandBuilder(da)
                    da.Fill(ds, "BILLS")
                    If ds.Tables("BILLS").Rows.Count <> 0 Then
                        If Not String.IsNullOrEmpty(INV_NO) Then
                            ds.Tables("BILLS").Rows(0)("INV_NO") = INV_NO.Trim
                        Else
                            ds.Tables("BILLS").Rows(0)("INV_NO") = System.DBNull.Value
                        End If
                        If Not String.IsNullOrEmpty(INV_DATE) Then
                            ds.Tables("BILLS").Rows(0)("INV_DATE") = INV_DATE
                        Else
                            ds.Tables("BILLS").Rows(0)("INV_DATE") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(INV_AMOUNT) Then
                            ds.Tables("BILLS").Rows(0)("INV_AMOUNT") = INV_AMOUNT
                        Else
                            ds.Tables("BILLS").Rows(0)("INV_AMOUNT") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(CUR_CODE) Then
                            ds.Tables("BILLS").Rows(0)("CUR_CODE") = CUR_CODE.Trim
                        Else
                            ds.Tables("BILLS").Rows(0)("CUR_CODE") = System.DBNull.Value
                        End If

                        If CONVERSION_RATE <> 0 Then
                            ds.Tables("BILLS").Rows(0)("CONVERSION_RATE") = CONVERSION_RATE
                        Else
                            ds.Tables("BILLS").Rows(0)("CONVERSION_RATE") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(RS_AMOUNT) Then
                            ds.Tables("BILLS").Rows(0)("RS_AMOUNT") = RS_AMOUNT
                        Else
                            ds.Tables("BILLS").Rows(0)("RS_AMOUNT") = System.DBNull.Value
                        End If

                        If DISCOUNT <> 0 Then
                            ds.Tables("BILLS").Rows(0)("DISCOUNT") = DISCOUNT
                        Else
                            ds.Tables("BILLS").Rows(0)("DISCOUNT") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(LESSDISCOUNT_AMOUNT) Then
                            ds.Tables("BILLS").Rows(0)("LESSDISCOUNT_AMOUNT") = LESSDISCOUNT_AMOUNT
                        Else
                            ds.Tables("BILLS").Rows(0)("LESSDISCOUNT_AMOUNT") = System.DBNull.Value
                        End If

                        If OTHER_CHARGES <> 0 Then
                            ds.Tables("BILLS").Rows(0)("OTHER_CHARGES") = OTHER_CHARGES.ToString
                        Else
                            ds.Tables("BILLS").Rows(0)("OTHER_CHARGES") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(BILLED_AMOUNT) Then
                            ds.Tables("BILLS").Rows(0)("BILLED_AMOUNT") = BILLED_AMOUNT
                        Else
                            ds.Tables("BILLS").Rows(0)("BILLED_AMOUNT") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(SUPPLEMENT) Then
                            ds.Tables("BILLS").Rows(0)("SUPPLEMENT") = SUPPLEMENT
                        Else
                            ds.Tables("BILLS").Rows(0)("SUPPLEMENT") = System.DBNull.Value
                        End If

                        If DEDUCTION <> 0 Then
                            ds.Tables("BILLS").Rows(0)("DEDUCTION") = DEDUCTION
                        Else
                            ds.Tables("BILLS").Rows(0)("DEDUCTION") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(AMOUNT_PASSED) Then
                            ds.Tables("BILLS").Rows(0)("AMOUNT_PASSED") = AMOUNT_PASSED
                        Else
                            ds.Tables("BILLS").Rows(0)("AMOUNT_PASSED") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(BUDG_ID) Then
                            ds.Tables("BILLS").Rows(0)("BUDG_ID") = BUDG_ID
                        Else
                            ds.Tables("BILLS").Rows(0)("BUDG_ID") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(VEND_ID) Then
                            ds.Tables("BILLS").Rows(0)("VEND_ID") = VEND_ID
                        Else
                            ds.Tables("BILLS").Rows(0)("VEND_ID") = System.DBNull.Value
                        End If

                        If Not String.IsNullOrEmpty(REMARKS) Then
                            ds.Tables("BILLS").Rows(0)("REMARKS") = REMARKS.ToString.Trim
                        Else
                            ds.Tables("BILLS").Rows(0)("REMARKS") = System.DBNull.Value
                        End If

                        ds.Tables("BILLS").Rows(0)("UPDATED_BY") = USER_CODE
                        ds.Tables("BILLS").Rows(0)("DATE_MODIFIED") = DATE_MODIFIED
                        ds.Tables("BILLS").Rows(0)("IP") = IP

                        thisTransaction = SqlConn.BeginTransaction()
                        da.SelectCommand.Transaction = thisTransaction
                        da.Update(ds, "BILLS")
                        thisTransaction.Commit()
                        lbl_msg.Text = "Record Updated Successfully"
                        lbl_Error.Text = ""
                        ClearBillFields()
                        PopulateBillsGrid()
                    Else
                        lbl_msg.Text = "Error in Record Updation!"
                        lbl_Error.Text = ""
                    End If
                End If
            Else
                'record not selected
                lbl_msg.Text = "Record not Selected for Edit!"
                lbl_Error.Text = ""
            End If
            SqlConn.Close()
        Catch q As SqlException
            thisTransaction.Rollback()
        Catch s As Exception
            lbl_Error.Text = "Error: " & (s.Message())
            lbl_msg.Text = ""
        Finally
            SqlConn.Close()
            Label23.Text = ""
        End Try
    End Sub
    'delete selected rows
    Protected Sub Bill_DeleteAll_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Bill_DeleteAll_Bttn.Click
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing
        Try
            For Each row As GridViewRow In Grid1.Rows
                Dim cb As CheckBox = DirectCast(row.FindControl("cbd"), CheckBox)
                If cb IsNot Nothing AndAlso cb.Checked = True Then
                    Dim ID As Integer = Convert.ToInt32(Grid1.DataKeys(row.RowIndex).Value)

                    If SqlConn.State = 0 Then
                        SqlConn.Open()
                    End If

                    thisTransaction = SqlConn.BeginTransaction()
                    Dim objCommand As New SqlCommand
                    objCommand.Connection = SqlConn
                    objCommand.Transaction = thisTransaction
                    objCommand.CommandType = CommandType.Text
                    objCommand.CommandText = "DELETE FROM BILLS WHERE (BILL_ID =@BILL_ID) "

                    objCommand.Parameters.Add("@BILL_ID", SqlDbType.Int)
                    objCommand.Parameters("@BILL_ID").Value = ID

                    objCommand.ExecuteNonQuery()
                    thisTransaction.Commit()
                    SqlConn.Close()
                End If
            Next
            lbl_msg.Text = "Selected Record(s) Deleted!"
            lbl_Error.Text = ""
            Dim s As Object = Nothing
            Dim e1 As EventArgs = Nothing
            PopulateBillsGrid()
        Catch s As Exception
            thisTransaction.Rollback()
            lbl_msg.Text = ""
            lbl_Error.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    '*********************************** ATACH TITLES
    'populate bills
    Public Sub DDL_Vendors2_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDL_Vendors2.SelectedIndexChanged
        Dim SQL As String = Nothing
        Dim dt As DataTable = Nothing
        Try
            Dim VEND_ID As Integer = Nothing
            If DDL_Vendors2.Text <> "" Then
                VEND_ID = DDL_Vendors2.SelectedValue
                SQL = "SELECT BILL_ID, INV_NO FROM BILLS WHERE (LIB_CODE ='" & Trim(LibCode) & "') AND (STATUS = 'New' OR STATUS ='Billed') AND (VEND_ID = '" & Trim(VEND_ID) & "') ORDER BY BILL_ID DESC "
                Dim ds As New DataSet
                Dim da As New SqlDataAdapter(SQL, SqlConn)
                If SqlConn.State = 0 Then
                    SqlConn.Open()
                End If
                da.Fill(ds)

                dt = ds.Tables(0).Copy
                If dt.Rows.Count = 0 Then
                    Me.DDL_Bills.DataSource = Nothing
                    DDL_Bills.DataBind()
                    DDL_Bills.Items.Clear()
                    DDL_Vendors2.Focus()
                    DDL_Orders.Items.Clear()
                Else
                    Me.DDL_Bills.DataSource = dt
                    Me.DDL_Bills.DataTextField = "INV_NO"
                    Me.DDL_Bills.DataValueField = "BILL_ID"
                    Me.DDL_Bills.DataBind()
                    DDL_Bills.Items.Insert(0, "")
                    DDL_Vendors2.Focus()
                    PopulateOrders(VEND_ID)
                End If
            Else
                Me.DDL_Bills.DataSource = Nothing
                DDL_Bills.DataBind()
                DDL_Bills.Items.Clear()
                DDL_Orders.Items.Clear()
                DDL_Vendors2.Focus()
            End If
        Catch s As Exception
            Label20.Text = "Error: " & (s.Message())
            Label7.Text = ""
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'populate Orders
    Public Sub PopulateOrders(ByVal VEND_ID As Integer)
        Dim rdr As SqlDataReader = Nothing
        Try
            If DDL_Vendors2.Text <> "" Then
                'Dim Sel As String = "SELECT DISTINCT ORDER_NO FROM ACQUISITIONS WHERE (PROCESS_STATUS = 'Accessioned' OR PROCESS_STATUS ='Billed') AND (LIB_CODE ='" & (LibCode) & "') AND  (VEND_ID ='" & Trim(VEND_ID) & "') ORDER BY ORDER_NO DESC"
                Dim Sel As String = "SELECT DISTINCT ORDER_NO FROM ACQUISITIONS WHERE (LIB_CODE ='" & Trim(LibCode) & "' AND  VEND_ID ='" & Trim(VEND_ID) & "') AND ((PROCESS_STATUS = 'Accessioned' OR PROCESS_STATUS ='Billed') OR (PROCESS_STATUS = 'Ordered' OR PROCESS_STATUS ='Billed' AND SUBS_YEAR<>Null))  ORDER BY ORDER_NO DESC;"

                Dim cmd As New SqlCommand(Sel, SqlConn)
                If SqlConn.State = 0 Then
                    SqlConn.Open()
                End If
                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                If rdr.HasRows = True Then
                    Me.DDL_Orders.DataTextField = "ORDER_NO"
                    Me.DDL_Orders.DataValueField = "ORDER_NO"
                    Me.DDL_Orders.DataSource = rdr
                    Me.DDL_Orders.DataBind()
                    DDL_Orders.Items.Insert(0, "")
                Else
                    Me.DDL_Orders.DataSource = rdr
                    Me.DDL_Orders.DataBind()
                End If
                SqlConn.Close()
            End If
        Catch ex As System.Exception
            If rdr IsNot Nothing Then
                rdr.Close()
            Else
                SqlConn.Close()
            End If
            Label20.Text = ex.Message.ToString()
            Label7.Text = ""
        End Try
    End Sub
    'populate acq records in 2nd grid
    Protected Sub DDL_Orders_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDL_Orders.SelectedIndexChanged
        Dim ORDER_NO As Object = Nothing
        Try
            If DDL_Orders.Text <> "" Then
                ORDER_NO = DDL_Orders.SelectedValue
                Dim dtSearch As DataTable = Nothing
                If ORDER_NO <> "" Then
                    Dim SQL As String = Nothing
                    ' SQL = "SELECT ACQ_ID, TITLE, VOL_NO, PROCESS_STATUS, CUR_CODE, ITEM_PRICE, CONVERSION_RATE, OTHER_CHARGES, ITEM_RUPEES FROM APPROVAL_VIEW2 WHERE (ORDER_NO = '" & ORDER_NO & "')  AND (PROCESS_STATUS = 'Accessioned' OR PROCESS_STATUS = 'Billed') AND (LIB_CODE ='" & (LibCode) & "') ORDER BY ACQ_ID DESC"
                    SQL = "SELECT ACQ_ID, TITLE, VOL_NO, PROCESS_STATUS, CUR_CODE, ITEM_PRICE, CONVERSION_RATE, OTHER_CHARGES, ITEM_RUPEES FROM APPROVAL_VIEW2 WHERE (LIB_CODE ='" & (LibCode) & "' AND ORDER_NO  = '" & ORDER_NO & "')  AND ((PROCESS_STATUS = 'Accessioned' OR PROCESS_STATUS ='Billed') OR (PROCESS_STATUS = 'Ordered' OR PROCESS_STATUS ='Billed' AND SUBS_YEAR<>Null))  ORDER BY ACQ_ID DESC; "
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
                        Label9.Text = "Total Record(s): 0 "
                        Bill_Dettach_Bttn.Visible = False
                        Bill_Attach_Bttn.Visible = False
                    Else
                        RecordCount = dtSearch.Rows.Count
                        Grid2.DataSource = dtSearch
                        Grid2.DataBind()
                        Label9.Text = "Total Record(s): " & RecordCount
                        Bill_Dettach_Bttn.Visible = True
                        Bill_Attach_Bttn.Visible = True
                    End If
                    ViewState("dt") = dtSearch
                Else
                    Me.Grid2.DataSource = Nothing
                    Grid2.DataBind()
                    Label9.Text = "Total Record(s): 0 "
                    Bill_Dettach_Bttn.Visible = False
                    Bill_Attach_Bttn.Visible = False
                End If
            Else
                Me.Grid2.DataSource = Nothing
                Grid2.DataBind()
                Label9.Text = "Total Record(s): 0 "
                Bill_Dettach_Bttn.Visible = False
                Bill_Attach_Bttn.Visible = False
            End If
        Catch s As Exception
            Label20.Text = "Error: " & (s.Message())
            Label7.Text = ""
        Finally
            SqlConn.Close()
            DDL_Orders.Focus()
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
            lbl_Error.Text = "Error- There is error in page index "
            lbl_msg.Text = ""
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

            'fill conversion rate,  other charges in  text box when it is blank
            Dim txtConv As TextBox = DirectCast(e.Row.FindControl("txt_Bill_ConversionRate2"), TextBox)
            If txtConv.Text = "" Then
                txtConv.Text = e.Row.Cells(6).Text
            End If
            Dim txtOChar As TextBox = DirectCast(e.Row.FindControl("txt_Bill_OtherCharges2"), TextBox)
            If txtOChar.Text = "" Then
                txtOChar.Text = e.Row.Cells(7).Text
            End If
        End If
    End Sub
    'attach titles in selected biils   
    Protected Sub Bill_Attach_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Bill_Attach_Bttn.Click
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing

        Try
            If DDL_Bills.SelectedValue <> "" Then
                If Grid2.Rows.Count <> 0 Then
                    For Each row As GridViewRow In Grid2.Rows
                        If row.RowType = DataControlRowType.DataRow Then

                            'Server Validation for Lib Code
                            Dim iloop As Integer
                            Dim strcurrentchar As Object
                            Dim c As Integer
                            Dim counter0, counter1, counter2, counter3 As Integer

                            Dim cb As CheckBox = DirectCast(row.FindControl("cbd"), CheckBox)
                            Dim ACQ_ID As Integer = Convert.ToInt32(Grid2.DataKeys(row.RowIndex).Value)

                            If cb IsNot Nothing AndAlso cb.Checked = True Then 'selected
                                Dim txtConversion As TextBox = DirectCast(row.FindControl("txt_Bill_ConversionRate2"), TextBox)
                                Dim CONVERSION_RATE As Object = Nothing
                                If Grid2.Rows(row.RowIndex).Cells(4).Text <> "" Then
                                    If txtConversion.Text <> "" Then
                                        CONVERSION_RATE = TrimX(Convert.ToDecimal(txtConversion.Text))
                                        CONVERSION_RATE = RemoveQuotes(CONVERSION_RATE)

                                        If CONVERSION_RATE.ToString.Length > 15 Then
                                            Label20.Text = "Plz Enter Data with Proper Length!"
                                            Label7.Text = ""
                                            txtConversion.Focus()
                                            Exit Sub
                                        End If
                                        CONVERSION_RATE = " " & CONVERSION_RATE & " "
                                        If InStr(1, CONVERSION_RATE, "CREATE", 1) > 0 Or InStr(1, CONVERSION_RATE, "DELETE", 1) > 0 Or InStr(1, CONVERSION_RATE, "DROP", 1) > 0 Or InStr(1, CONVERSION_RATE, "INSERT", 1) > 1 Or InStr(1, CONVERSION_RATE, "TRACK", 1) > 1 Or InStr(1, CONVERSION_RATE, "TRACE", 1) > 1 Then
                                            Label20.Text = "Do Not Use Reserve Words!"
                                            Label7.Text = ""
                                            txtConversion.Focus()
                                            Exit Sub
                                        End If
                                        CONVERSION_RATE = TrimX(CONVERSION_RATE)
                                        c = 0
                                        counter0 = 0
                                        For iloop = 1 To Len(CONVERSION_RATE.ToString)
                                            strcurrentchar = Mid(CONVERSION_RATE, iloop, 1)
                                            If c = 0 Then
                                                If Not InStr("'~!@#$^&*=+|[]{}?<>()=%abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ,;:_", strcurrentchar) <= 0 Then
                                                    c = c + 1
                                                    counter0 = 1
                                                End If
                                            End If
                                        Next
                                        If counter0 = 1 Then
                                            lbl_Error.Text = "Do Not Use Un-Wanted Characters!"
                                            lbl_msg.Text = ""
                                            txt_Bill_ConversionRate.Focus()
                                            Exit Sub
                                        End If
                                    Else
                                        CONVERSION_RATE = 0
                                    End If
                                Else
                                    CONVERSION_RATE = 0
                                End If

                                Dim txtOtherCharges As TextBox = DirectCast(row.FindControl("txt_Bill_OtherCharges2"), TextBox)
                                Dim OTHER_CHARGES As Object = Nothing
                                If txtOtherCharges.Text <> "" Then
                                    'Server Validation for OTHER_CHARGES
                                    OTHER_CHARGES = TrimX(txtOtherCharges.Text)
                                    OTHER_CHARGES = RemoveQuotes(OTHER_CHARGES)
                                    If OTHER_CHARGES.ToString.Length > 15 Then
                                        Label20.Text = "Plz Enter Data with Proper Length!"
                                        Label7.Text = ""
                                        txtOtherCharges.Focus()
                                        Exit Sub
                                    End If
                                    OTHER_CHARGES = " " & OTHER_CHARGES & " "
                                    If InStr(1, OTHER_CHARGES, "CREATE", 1) > 0 Or InStr(1, OTHER_CHARGES, "DELETE", 1) > 0 Or InStr(1, OTHER_CHARGES, "DROP", 1) > 0 Or InStr(1, OTHER_CHARGES, "INSERT", 1) > 1 Or InStr(1, OTHER_CHARGES, "TRACK", 1) > 1 Or InStr(1, OTHER_CHARGES, "TRACE", 1) > 1 Then
                                        Label20.Text = "Do Not Use Reserve Words!"
                                        Label7.Text = ""
                                        txtOtherCharges.Focus()
                                        Exit Sub
                                    End If
                                    OTHER_CHARGES = TrimX(OTHER_CHARGES)
                                    c = 0
                                    counter1 = 0
                                    For iloop = 1 To Len(OTHER_CHARGES.ToString)
                                        strcurrentchar = Mid(OTHER_CHARGES, iloop, 1)
                                        If c = 0 Then
                                            If Not InStr("'~!@#$^&*=+|[]{}?<>()=%abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ,;:_", strcurrentchar) <= 0 Then
                                                c = c + 1
                                                counter1 = 1
                                            End If
                                        End If
                                    Next
                                    If counter1 = 1 Then
                                        Label20.Text = "Do Not Use Un-Wanted Characters!"
                                        Label7.Text = ""
                                        txtOtherCharges.Focus()
                                        Exit Sub
                                    End If
                                Else
                                    OTHER_CHARGES = 0
                                End If

                                Dim ITEM_PRICE As Object = Nothing
                                If Grid2.Rows(row.RowIndex).Cells(5).Text <> "" Then
                                    ITEM_PRICE = Convert.ToDecimal(Grid2.Rows(row.RowIndex).Cells(5).Text)
                                Else
                                    ITEM_PRICE = 0
                                End If

                                Dim ITEM_RUPEES As Object = Nothing
                                If CONVERSION_RATE <> 0 Then
                                    ITEM_RUPEES = ITEM_PRICE * CDec(CONVERSION_RATE)
                                Else
                                    ITEM_RUPEES = CDec(ITEM_PRICE)
                                End If

                                If OTHER_CHARGES <> 0 Then
                                    ITEM_RUPEES = CDec(ITEM_RUPEES) + CDec(OTHER_CHARGES)
                                End If

                                'bill ID
                                Dim BILL_ID As Integer = Nothing
                                If DDL_Bills.Text <> "" Then
                                    BILL_ID = Trim(DDL_Bills.SelectedValue)
                                    BILL_ID = RemoveQuotes(BILL_ID)

                                    If IsNumeric(BILL_ID) = False Then
                                        Label20.Text = "Plz Select Bill No from Drop-Down to Edit!"
                                        Label7.Text = ""
                                        DDL_Bills.Focus()
                                        Exit Sub
                                    End If
                                    If BILL_ID.ToString.Length > 10 Then
                                        Label20.Text = "Plz Enter Data with Proper Length!"
                                        Label7.Text = ""
                                        DDL_Bills.Focus()
                                        Exit Sub
                                    End If
                                    BILL_ID = " " & BILL_ID & " "
                                    If InStr(1, BILL_ID, "CREATE", 1) > 0 Or InStr(1, BILL_ID, "DELETE", 1) > 0 Or InStr(1, BILL_ID, "DROP", 1) > 0 Or InStr(1, BILL_ID, "INSERT", 1) > 1 Or InStr(1, BILL_ID, "TRACK", 1) > 1 Or InStr(1, BILL_ID, "TRACE", 1) > 1 Then
                                        Label20.Text = "Do Not Use Reserve Words!"
                                        Label7.Text = ""
                                        DDL_Bills.Focus()
                                        Exit Sub
                                    End If
                                    BILL_ID = TrimX(BILL_ID)

                                    c = 0
                                    counter1 = 0
                                    For iloop = 1 To Len(BILL_ID.ToString)
                                        strcurrentchar = Mid(BILL_ID, iloop, 1)
                                        If c = 0 Then
                                            If Not InStr("'~!@#$^&*=+|[]{}?<>=%""", strcurrentchar) <= 0 Then
                                                c = c + 1
                                                counter1 = 1
                                            End If
                                        End If
                                    Next
                                    If counter1 = 1 Then
                                        Label20.Text = "Do Not Use Un-Wanted Characters!"
                                        Label7.Text = ""
                                        DDL_Bills.Focus()
                                        Exit Sub
                                    End If
                                Else
                                    Label20.Text = "Plz Select Bill No from Drop-Down!"
                                    Label7.Text = ""
                                    DDL_Bills.Focus()
                                    Exit Sub
                                End If

                                Dim DATE_MODIFIED As Object = Nothing
                                DATE_MODIFIED = Now.Date
                                DATE_MODIFIED = Convert.ToDateTime(DATE_MODIFIED, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                                Dim IP As Object = Nothing
                                IP = Request.UserHostAddress.Trim

                                'update acq record
                                Dim SQL As String = Nothing
                                'SQL = "SELECT ACQ_ID, PROCESS_STATUS, CONVERSION_RATE, OTHER_CHARGES, ITEM_RUPEES, DATE_MODIFIED,UPDATED_BY, IP, BILL_ID FROM ACQUISITIONS WHERE (ACQ_ID = '" & ACQ_ID & "')  AND (PROCESS_STATUS = 'Accessioned' or PROCESS_STATUS = 'Billed') AND (LIB_CODE ='" & (LibCode) & "') "
                                SQL = "SELECT ACQ_ID, PROCESS_STATUS, CONVERSION_RATE, OTHER_CHARGES, ITEM_RUPEES, DATE_MODIFIED,UPDATED_BY, IP, BILL_ID, SUBS_YEAR FROM ACQUISITIONS WHERE (ACQ_ID = '" & ACQ_ID & "' AND LIB_CODE ='" & (LibCode) & "')  AND ((PROCESS_STATUS = 'Accessioned' OR PROCESS_STATUS ='Billed') OR (PROCESS_STATUS = 'Ordered' OR PROCESS_STATUS ='Billed' AND SUBS_YEAR<>Null)) ;"
                                SqlConn.Open()

                                Dim da As SqlDataAdapter
                                Dim cmdb As SqlCommandBuilder
                                Dim ds As New DataSet
                                Dim dt As New DataTable

                                da = New SqlDataAdapter(SQL, SqlConn)
                                cmdb = New SqlCommandBuilder(da)
                                da.Fill(ds, "ACQ")
                                If ds.Tables("ACQ").Rows.Count <> 0 Then
                                    If CONVERSION_RATE <> 0 Then
                                        ds.Tables("ACQ").Rows(0)("CONVERSION_RATE") = Trim(CONVERSION_RATE)
                                    Else
                                        ds.Tables("ACQ").Rows(0)("CONVERSION_RATE") = System.DBNull.Value
                                    End If
                                    If OTHER_CHARGES <> 0 Then
                                        ds.Tables("ACQ").Rows(0)("OTHER_CHARGES") = Trim(OTHER_CHARGES)
                                    Else
                                        ds.Tables("ACQ").Rows(0)("OTHER_CHARGES") = System.DBNull.Value
                                    End If
                                    If ITEM_RUPEES <> 0 Then
                                        ds.Tables("ACQ").Rows(0)("ITEM_RUPEES") = Trim(ITEM_RUPEES)
                                    Else
                                        ds.Tables("ACQ").Rows(0)("ITEM_RUPEES") = System.DBNull.Value
                                    End If
                                    If BILL_ID <> 0 Then
                                        ds.Tables("ACQ").Rows(0)("BILL_ID") = Trim(BILL_ID)
                                    Else
                                        ds.Tables("ACQ").Rows(0)("BILL_ID") = System.DBNull.Value
                                    End If

                                    ds.Tables("ACQ").Rows(0)("UPDATED_BY") = UserCode
                                    ds.Tables("ACQ").Rows(0)("DATE_MODIFIED") = DATE_MODIFIED
                                    ds.Tables("ACQ").Rows(0)("IP") = IP
                                    ds.Tables("ACQ").Rows(0)("PROCESS_STATUS") = "Billed"

                                    thisTransaction = SqlConn.BeginTransaction()
                                    da.SelectCommand.Transaction = thisTransaction
                                    da.Update(ds, "ACQ")

                                    'update BILLS Table with Status ='Billed'
                                    Dim objCommand As New SqlCommand
                                    objCommand.Connection = SqlConn
                                    objCommand.Transaction = thisTransaction
                                    objCommand.CommandType = CommandType.Text
                                    'objCommand.CommandText = "UPDATE BILLS SET STATUS=@STATUS, LIB_CODE=@LIB_CODE, UPDATED_BY=@UPDATED_BY, DATE_MODIFIED=@DATE_MODIFIED, IP=@IP FROM BILLS WHERE (BILL_ID = @BILL_ID and STATUS = 'New'  AND LIB_CODE =@LIB_CODE) "
                                    objCommand.CommandText = "UPDATE BILLS SET STATUS=@STATUS, LIB_CODE=@LIB_CODE, UPDATED_BY=@UPDATED_BY, DATE_MODIFIED=@DATE_MODIFIED, IP=@IP WHERE (BILL_ID = @BILL_ID and STATUS = 'New'  AND LIB_CODE =@LIB_CODE) "

                                    objCommand.Parameters.Add("@BILL_ID", SqlDbType.Int)
                                    If BILL_ID = 0 Then
                                        objCommand.Parameters("@BILL_ID").Value = System.DBNull.Value
                                    Else
                                        objCommand.Parameters("@BILL_ID").Value = BILL_ID
                                    End If

                                    objCommand.Parameters.Add("@STATUS", SqlDbType.VarChar)
                                    objCommand.Parameters("@STATUS").Value = "Billed"

                                    objCommand.Parameters.Add("@LIB_CODE", SqlDbType.NVarChar)
                                    objCommand.Parameters("@LIB_CODE").Value = LibCode

                                    objCommand.Parameters.Add("@UPDATED_BY", SqlDbType.NVarChar)
                                    objCommand.Parameters("@UPDATED_BY").Value = UserCode

                                    objCommand.Parameters.Add("@DATE_MODIFIED", SqlDbType.NVarChar)
                                    objCommand.Parameters("@DATE_MODIFIED").Value = DATE_MODIFIED

                                    objCommand.Parameters.Add("@IP", SqlDbType.VarChar)
                                    objCommand.Parameters("@IP").Value = DATE_MODIFIED

                                    objCommand.ExecuteNonQuery()

                                    thisTransaction.Commit()
                                End If
                            End If
                            End If
                            SqlConn.Close()
                    Next
                    Label7.Text = "Record(s) Updated Successfully!"
                    Label20.Text = ""
                    PopulateAcqGrid2()
                Else
                    Label7.Text = ""
                    Label20.Text = ""
                End If
            Else
                Label20.Text = "Plz Select Bill No from Drop-Down!"
                Label7.Text = ""
                DDL_Bills.Focus()
            End If
        Catch q As SqlException
            thisTransaction.Rollback()
            Label7.Text = "Record(s) Not Updated!"
            Label20.Text = ""
        Catch s As Exception
            Label7.Text = ""
            Label20.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    Public Sub PopulateAcqGrid2()
        Dim ORDER_NO As Object = Nothing
        Try
            If DDL_Orders.Text <> "" Then
                ORDER_NO = DDL_Orders.SelectedValue
                Dim dtSearch As DataTable = Nothing
                If ORDER_NO <> "" Then
                    Dim SQL As String = Nothing
                    SQL = "SELECT ACQ_ID, TITLE, VOL_NO, PROCESS_STATUS, CUR_CODE, ITEM_PRICE, CONVERSION_RATE, OTHER_CHARGES, ITEM_RUPEES FROM APPROVAL_VIEW2 WHERE (ORDER_NO = '" & ORDER_NO & "')  AND (PROCESS_STATUS = 'Accessioned' OR PROCESS_STATUS = 'Billed') AND (LIB_CODE ='" & (LibCode) & "') ORDER BY ACQ_ID DESC"

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
                        Label9.Text = "Total Record(s): 0 "
                        Bill_Dettach_Bttn.Visible = False
                        Bill_Attach_Bttn.Visible = False
                    Else
                        RecordCount = dtSearch.Rows.Count
                        Grid2.DataSource = dtSearch
                        Grid2.DataBind()
                        Label9.Text = "Total Record(s): " & RecordCount
                        Bill_Dettach_Bttn.Visible = True
                        Bill_Attach_Bttn.Visible = True
                    End If
                    ViewState("dt") = dtSearch
                Else
                    Me.Grid2.DataSource = Nothing
                    Grid2.DataBind()
                    Label9.Text = "Total Record(s): 0 "
                    Bill_Dettach_Bttn.Visible = False
                    Bill_Attach_Bttn.Visible = False
                End If
            Else
                Me.Grid2.DataSource = Nothing
                Grid2.DataBind()
                Label9.Text = "Total Record(s): 0 "
                Bill_Dettach_Bttn.Visible = False
                Bill_Attach_Bttn.Visible = False
            End If
        Catch s As Exception
            Label20.Text = "Error: " & (s.Message())
            Label7.Text = ""
        Finally

            SqlConn.Close()
            DDL_Orders.Focus()
        End Try
    End Sub
    'dettach bills from acq records
    Protected Sub Bill_Dettach_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Bill_Dettach_Bttn.Click
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing

        Try
            If Grid2.Rows.Count <> 0 Then
                For Each row As GridViewRow In Grid2.Rows
                    If row.RowType = DataControlRowType.DataRow Then

                        'Server Validation for Lib Code
                        Dim iloop As Integer
                        Dim strcurrentchar As Object
                        Dim c As Integer
                        Dim counter0, counter1, counter2, counter3 As Integer

                        Dim cb As CheckBox = DirectCast(row.FindControl("cbd"), CheckBox)
                        Dim ACQ_ID As Integer = Convert.ToInt32(Grid2.DataKeys(row.RowIndex).Value)

                        If cb IsNot Nothing AndAlso cb.Checked = True Then 'selected
                            Dim DATE_MODIFIED As Object = Nothing
                            DATE_MODIFIED = Now.Date
                            DATE_MODIFIED = Convert.ToDateTime(DATE_MODIFIED, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                            Dim IP As Object = Nothing
                            IP = Request.UserHostAddress.Trim

                            'update acq record
                            Dim SQL As String = Nothing
                            SQL = "SELECT ACQ_ID, PROCESS_STATUS, DATE_MODIFIED, UPDATED_BY, IP, BILL_ID FROM ACQUISITIONS WHERE (ACQ_ID = '" & ACQ_ID & "')  AND (PROCESS_STATUS = 'Billed') AND (LIB_CODE ='" & (LibCode) & "') "

                            SqlConn.Open()

                            Dim da As SqlDataAdapter
                            Dim cmdb As SqlCommandBuilder
                            Dim ds As New DataSet
                            Dim dt As New DataTable

                            da = New SqlDataAdapter(SQL, SqlConn)
                            cmdb = New SqlCommandBuilder(da)
                            da.Fill(ds, "ACQ")
                            If ds.Tables("ACQ").Rows.Count <> 0 Then

                                'Get Bill_ID to change status of that bill_id
                                Dim myBILL_ID As Integer = Nothing
                                dt = ds.Tables("ACQ").Copy

                                If dt.Rows(0).Item("BILL_ID").ToString <> "" Then
                                    myBILL_ID = dt.Rows(0).Item("BILL_ID").ToString
                                Else
                                    myBILL_ID = 0
                                End If

                                ds.Tables("ACQ").Rows(0)("BILL_ID") = System.DBNull.Value
                                ds.Tables("ACQ").Rows(0)("UPDATED_BY") = UserCode
                                ds.Tables("ACQ").Rows(0)("DATE_MODIFIED") = DATE_MODIFIED
                                ds.Tables("ACQ").Rows(0)("IP") = IP
                                ds.Tables("ACQ").Rows(0)("PROCESS_STATUS") = "Accessioned"

                                thisTransaction = SqlConn.BeginTransaction()
                                da.SelectCommand.Transaction = thisTransaction
                                da.Update(ds, "ACQ")

                                'update BILLS Table with Status ='Billed'
                                Dim objCommand As New SqlCommand
                                objCommand.Connection = SqlConn
                                objCommand.Transaction = thisTransaction
                                objCommand.CommandType = CommandType.Text
                                objCommand.CommandText = "UPDATE BILLS SET STATUS=@STATUS, UPDATED_BY=@UPDATED_BY, DATE_MODIFIED=@DATE_MODIFIED, IP=@IP FROM BILLS WHERE (BILL_ID = @BILL_ID and STATUS = 'Billed'  AND LIB_CODE =@LIB_CODE) "

                                objCommand.Parameters.Add("@BILL_ID", SqlDbType.Int)
                                objCommand.Parameters("@BILL_ID").Value = myBILL_ID

                                objCommand.Parameters.Add("@LIB_CODE", SqlDbType.NVarChar)
                                objCommand.Parameters("@LIB_CODE").Value = LibCode



                                objCommand.Parameters.Add("@STATUS", SqlDbType.VarChar)
                                objCommand.Parameters("@STATUS").Value = "New"

                                objCommand.Parameters.Add("@UPDATED_BY", SqlDbType.NVarChar)
                                objCommand.Parameters("@UPDATED_BY").Value = UserCode

                                objCommand.Parameters.Add("@DATE_MODIFIED", SqlDbType.NVarChar)
                                objCommand.Parameters("@DATE_MODIFIED").Value = DATE_MODIFIED

                                objCommand.Parameters.Add("@IP", SqlDbType.VarChar)
                                objCommand.Parameters("@IP").Value = DATE_MODIFIED

                                objCommand.ExecuteNonQuery()
                                thisTransaction.Commit()

                            End If
                        End If
                    End If
                    SqlConn.Close()
                Next
                Label7.Text = "Record(s) Updated Successfully!"
                Label20.Text = ""
                PopulateAcqGrid2()
            Else
                Label7.Text = ""
                Label20.Text = ""
            End If
        Catch q As SqlException
            thisTransaction.Rollback()
            Label7.Text = "Record(s) Not Updated!"
            Label20.Text = ""
        Catch s As Exception
            Label7.Text = ""
            Label20.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    '*********************************************POST BILLS
    'populate acq records of a title
    Public Sub PopulateBillsGrid3()
        Dim dtSearch As DataTable = Nothing
        Try

            Dim SQL As String = Nothing
            SQL = " SELECT BILLS.BILL_ID, BILLS.INV_NO, BILLS.INV_DATE, BILLS.INV_AMOUNT, BILLS.CUR_CODE, BILLS.CONVERSION_RATE, BILLS.RS_AMOUNT, BILLS.DISCOUNT, BILLS.LESSDISCOUNT_AMOUNT, BILLS.OTHER_CHARGES, BILLS.BILLED_AMOUNT,BILLS.DEDUCTION, BILLS.AMOUNT_PASSED, BILLS.STATUS, BILLS.PMT_REQ_NO, VENDORS.VEND_NAME FROM BILLS LEFT OUTER JOIN VENDORS ON BILLS.VEND_ID = VENDORS.VEND_ID WHERE (BILLS.STATUS='Billed' OR BILLS.STATUS='Posted') AND (BILLS.LIB_CODE = '" & Trim(LibCode) & "')  ORDER BY BILLS.BILL_ID DESC"

            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            da.Fill(ds)

            dtSearch = ds.Tables(0).Copy
            Dim RecordCount As Long = 0
            If dtSearch.Rows.Count = 0 Then
                Me.Grid3.DataSource = Nothing
                Grid3.DataBind()
                Label13.Text = "Total Record(s): 0 "
                Bill_Post_Bttn.Visible = False
                Bill_UnPost_Bttn.Visible = False
            Else
                Grid3.Visible = True
                RecordCount = dtSearch.Rows.Count
                Grid3.DataSource = dtSearch
                Grid3.DataBind()
                Label13.Text = "Total Record(s): " & RecordCount
                Bill_Post_Bttn.Visible = True
                Bill_UnPost_Bttn.Visible = True
            End If
            SqlConn.Close()
            ViewState("dt") = dtSearch
        Catch s As Exception
            Label12.Text = "Error-SAVE: " & (s.Message())
            Label11.Text = ""
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'grid view page index changing event
    Protected Sub Grid3_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles Grid3.PageIndexChanging
        Try
            'rebind datagrid
            Grid3.DataSource = ViewState("dt") 'temp
            Grid3.PageIndex = e.NewPageIndex
            index = e.NewPageIndex * Grid3.PageSize
            Grid3.DataBind()
        Catch s As Exception
            Label12.Text = "Error- There is error in page index "
            Label11.Text = ""
        End Try
    End Sub
    'gridview sorting event
    Protected Sub Grid3_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles Grid3.Sorting
        Dim temp As DataTable = CType(ViewState("dt"), DataTable)
        GridViewSortExpression = e.SortExpression
        Grid3.DataSource = temp
        Dim pageIndex As Integer = Grid3.PageIndex
        Grid3.DataSource = SortDataTable(Grid3.DataSource, False)
        Grid3.DataBind()
        Grid3.PageIndex = pageIndex
    End Sub
    Protected Sub Grid3_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Grid3.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Lblsr As Label = e.Row.FindControl("lblsr")
            Srno = index + 1
            If Lblsr Is Nothing = False Then
                Lblsr.Text = Srno
                index += 1
            End If
        End If
    End Sub
    'autocomplete method for Pmt Req No
    <System.Web.Script.Services.ScriptMethod(), _
  System.Web.Services.WebMethod()> _
    Public Shared Function SearchAllPMR(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Return SearchPMR(prefixText, count)
    End Function
    <System.Web.Script.Services.ScriptMethod(), _
System.Web.Services.WebMethod()> _
    Public Shared Function SearchPMR(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim sdr As SqlDataReader
        Dim cmd As SqlCommand = New SqlCommand
        cmd.CommandText = "select DISTINCT PMT_REQ_NO from BILLS where (LIB_CODE = '" & Trim(LibCode) & "') Order by PMT_REQ_NO DESC "
        cmd.Connection = SqlConn
        Try
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            Dim PMR As List(Of String) = New List(Of String)
            sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            While sdr.Read
                PMR.Add(sdr("PMT_REQ_NO").ToString)
            End While
            Return PMR
        Catch e As Exception
            If sdr IsNot Nothing Then
                sdr.Close()
            Else
                SqlConn.Close()
            End If
        Finally
            SqlConn.Close()
        End Try
    End Function
    'post selected Records
    Protected Sub Bill_Post_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Bill_Post_Bttn.Click
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing
        Try
            If Grid3.Rows.Count <> 0 Then
                For Each row As GridViewRow In Grid3.Rows
                    If row.RowType = DataControlRowType.DataRow Then

                        'Server Validation for Lib Code
                        Dim iloop As Integer
                        Dim strcurrentchar As Object
                        Dim c As Integer
                        Dim counter0, counter1 As Integer

                        Dim cb As CheckBox = DirectCast(row.FindControl("cbd"), CheckBox)
                        Dim BILL_ID As Integer = Convert.ToInt32(Grid3.DataKeys(row.RowIndex).Value)

                        If cb IsNot Nothing AndAlso cb.Checked = True Then 'selected
                            'Server Validation for PMT REQ NO
                            Dim PMT_REQ_NO As Integer = Nothing
                            If txt_Bill_PmtReqNo.Text <> "" Then
                                PMT_REQ_NO = TrimX(txt_Bill_PmtReqNo.Text)
                                PMT_REQ_NO = RemoveQuotes(PMT_REQ_NO)

                                If IsNumeric(PMT_REQ_NO) = False Then
                                    Label12.Text = "Plz Select Bill No from Drop-Down to Edit!"
                                    Label11.Text = ""
                                    txt_Bill_PmtReqNo.Focus()
                                    Exit Sub
                                End If
                                If PMT_REQ_NO.ToString.Length > 10 Then
                                    Label12.Text = "Plz Enter Data with Proper Length!"
                                    Label11.Text = ""
                                    txt_Bill_PmtReqNo.Focus()
                                    Exit Sub
                                End If
                                PMT_REQ_NO = " " & PMT_REQ_NO & " "
                                If InStr(1, PMT_REQ_NO, "CREATE", 1) > 0 Or InStr(1, PMT_REQ_NO, "DELETE", 1) > 0 Or InStr(1, PMT_REQ_NO, "DROP", 1) > 0 Or InStr(1, PMT_REQ_NO, "INSERT", 1) > 1 Or InStr(1, PMT_REQ_NO, "TRACK", 1) > 1 Or InStr(1, PMT_REQ_NO, "TRACE", 1) > 1 Then
                                    Label12.Text = "Do Not Use Reserve Words!"
                                    Label11.Text = ""
                                    txt_Bill_PmtReqNo.Focus()
                                    Exit Sub
                                End If
                                PMT_REQ_NO = TrimX(PMT_REQ_NO)

                                c = 0
                                counter0 = 0
                                For iloop = 1 To Len(PMT_REQ_NO.ToString)
                                    strcurrentchar = Mid(PMT_REQ_NO, iloop, 1)
                                    If c = 0 Then
                                        If Not InStr("'~!@#$^&*=+|[]{}?<>=%abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ,;:_", strcurrentchar) <= 0 Then
                                            c = c + 1
                                            counter0 = 1
                                        End If
                                    End If
                                Next
                                If counter0 = 1 Then
                                    Label12.Text = "Do Not Use Un-Wanted Characters!"
                                    Label11.Text = ""
                                    txt_Bill_PmtReqNo.Focus()
                                    Exit Sub
                                End If

                                'check status of pmt req no
                                Dim str As Object = Nothing
                                Dim flag As Object = Nothing
                                str = "SELECT BILL_ID FROM BILLS WHERE (PMT_REQ_NO = '" & Trim(PMT_REQ_NO) & "' AND LIB_CODE ='" & Trim(LibCode) & "' AND STATUS ='Paid') "
                                Dim cmd1 As New SqlCommand(str, SqlConn)
                                SqlConn.Open()
                                flag = cmd1.ExecuteScalar
                                If flag <> Nothing Then
                                    Label2.Text = "This Payment Request No is Already Processed ! "
                                    Label11.Text = ""
                                    txt_Bill_PmtReqNo.Focus()
                                    Exit Sub
                                End If
                                SqlConn.Close()
                            Else
                                Label12.Text = "Plz Enter Payment Request No!"
                                Label11.Text = ""
                                txt_Bill_PmtReqNo.Focus()
                                Exit Sub
                            End If

                            'validation for pamt req date
                            Dim PMT_REQ_DATE As Object = Nothing
                            If txt_Bill_PmtReqDate.Text <> "" Then
                                PMT_REQ_DATE = TrimX(txt_Bill_PmtReqDate.Text)
                                PMT_REQ_DATE = RemoveQuotes(PMT_REQ_DATE)
                                PMT_REQ_DATE = Convert.ToDateTime(PMT_REQ_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                                If Len(PMT_REQ_DATE) > 12 Then
                                    Label12.Text = " Input is not Valid..."
                                    Label11.Text = ""
                                    Me.txt_Bill_PmtReqDate.Focus()
                                    Exit Sub
                                End If
                                PMT_REQ_DATE = " " & PMT_REQ_DATE & " "
                                If InStr(1, PMT_REQ_DATE, "CREATE", 1) > 0 Or InStr(1, PMT_REQ_DATE, "DELETE", 1) > 0 Or InStr(1, PMT_REQ_DATE, "DROP", 1) > 0 Or InStr(1, PMT_REQ_DATE, "INSERT", 1) > 1 Or InStr(1, PMT_REQ_DATE, "TRACK", 1) > 1 Or InStr(1, PMT_REQ_DATE, "TRACE", 1) > 1 Then
                                    Label12.Text = "  Input is not Valid... "
                                    Label11.Text = ""
                                    Me.txt_Bill_PmtReqDate.Focus()
                                    Exit Sub
                                End If
                                PMT_REQ_DATE = TrimX(PMT_REQ_DATE)
                                'check unwanted characters
                                c = 0
                                counter1 = 0
                                For iloop = 1 To Len(PMT_REQ_DATE)
                                    strcurrentchar = Mid(PMT_REQ_DATE, iloop, 1)
                                    If c = 0 Then
                                        If Not InStr("'~!@#$^&*=+|[]{}?<>=%abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ,;:_", strcurrentchar) <= 0 Then
                                            c = c + 1
                                            counter1 = 1
                                        End If
                                    End If
                                Next
                                If counter1 = 1 Then
                                    Label12.Text = "data is not Valid... "
                                    Label11.Text = ""
                                    Me.txt_Bill_PmtReqDate.Focus()
                                    Exit Sub
                                End If
                            Else
                                PMT_REQ_DATE = Now.Date
                                PMT_REQ_DATE = Convert.ToDateTime(PMT_REQ_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us
                            End If

                            Dim DATE_MODIFIED As Object = Nothing
                            DATE_MODIFIED = Now.Date
                            DATE_MODIFIED = Convert.ToDateTime(DATE_MODIFIED, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                            Dim IP As Object = Nothing
                            IP = Request.UserHostAddress.Trim

                            If Grid3.Rows(row.RowIndex).Cells(4).Text = "Billed" Then
                                'update acq record
                                Dim SQL As String = Nothing
                                SQL = "SELECT * FROM BILLS WHERE (BILL_ID = '" & BILL_ID & "')  AND (STATUS = 'Billed') AND (LIB_CODE ='" & (LibCode) & "') "

                                SqlConn.Open()
                                Dim da As SqlDataAdapter
                                Dim cmdb As SqlCommandBuilder
                                Dim ds As New DataSet
                                Dim dt As New DataTable

                                da = New SqlDataAdapter(SQL, SqlConn)
                                cmdb = New SqlCommandBuilder(da)
                                da.Fill(ds, "BILLS")
                                If ds.Tables("BILLS").Rows.Count <> 0 Then
                                    ds.Tables("BILLS").Rows(0)("PMT_REQ_NO") = PMT_REQ_NO
                                    ds.Tables("BILLS").Rows(0)("PMT_REQ_DATE") = PMT_REQ_DATE
                                    ds.Tables("BILLS").Rows(0)("UPDATED_BY") = UserCode
                                    ds.Tables("BILLS").Rows(0)("DATE_MODIFIED") = DATE_MODIFIED
                                    ds.Tables("BILLS").Rows(0)("IP") = IP
                                    ds.Tables("BILLS").Rows(0)("STATUS") = "Posted"

                                    thisTransaction = SqlConn.BeginTransaction()
                                    da.SelectCommand.Transaction = thisTransaction
                                    da.Update(ds, "BILLS")

                                    thisTransaction.Commit()
                                End If
                            End If
                        End If
                    End If
                    SqlConn.Close()
                Next
                Label11.Text = "Record(s) Updated Successfully!"
                Label12.Text = ""
                PopulateBillsGrid3()
            Else
                Label11.Text = ""
                Label12.Text = ""
            End If
        Catch q As SqlException
            thisTransaction.Rollback()
            Label2.Text = "Record(s) Not Updated!"
            Label1.Text = ""
        Catch s As Exception
            Label12.Text = ""
            Label1.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'unpost bill
    Protected Sub Bill_UnPost_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Bill_UnPost_Bttn.Click
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing
        Try
            If Grid3.Rows.Count <> 0 Then
                For Each row As GridViewRow In Grid3.Rows
                    If row.RowType = DataControlRowType.DataRow Then

                        'Server Validation for Lib Code
                        Dim iloop As Integer
                        Dim strcurrentchar As Object
                        Dim c As Integer
                        Dim counter0, counter1 As Integer

                        Dim cb As CheckBox = DirectCast(row.FindControl("cbd"), CheckBox)
                        Dim BILL_ID As Integer = Convert.ToInt32(Grid3.DataKeys(row.RowIndex).Value)

                        If cb IsNot Nothing AndAlso cb.Checked = True Then 'selected
                            Dim DATE_MODIFIED As Object = Nothing
                            DATE_MODIFIED = Now.Date
                            DATE_MODIFIED = Convert.ToDateTime(DATE_MODIFIED, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                            Dim IP As Object = Nothing
                            IP = Request.UserHostAddress.Trim

                            If Grid3.Rows(row.RowIndex).Cells(4).Text = "Posted" Then
                                'update acq record
                                Dim SQL As String = Nothing
                                SQL = "SELECT * FROM BILLS WHERE (BILL_ID = '" & BILL_ID & "')  AND (STATUS = 'Posted') AND (LIB_CODE ='" & (LibCode) & "') "

                                SqlConn.Open()
                                Dim da As SqlDataAdapter
                                Dim cmdb As SqlCommandBuilder
                                Dim ds As New DataSet
                                Dim dt As New DataTable

                                da = New SqlDataAdapter(SQL, SqlConn)
                                cmdb = New SqlCommandBuilder(da)
                                da.Fill(ds, "BILLS")
                                If ds.Tables("BILLS").Rows.Count <> 0 Then
                                    ds.Tables("BILLS").Rows(0)("PMT_REQ_NO") = System.DBNull.Value
                                    ds.Tables("BILLS").Rows(0)("PMT_REQ_DATE") = System.DBNull.Value
                                    ds.Tables("BILLS").Rows(0)("UPDATED_BY") = UserCode
                                    ds.Tables("BILLS").Rows(0)("DATE_MODIFIED") = DATE_MODIFIED
                                    ds.Tables("BILLS").Rows(0)("IP") = IP
                                    ds.Tables("BILLS").Rows(0)("STATUS") = "Billed"

                                    thisTransaction = SqlConn.BeginTransaction()
                                    da.SelectCommand.Transaction = thisTransaction
                                    da.Update(ds, "BILLS")

                                    thisTransaction.Commit()
                                End If
                            End If
                        End If
                    End If
                    SqlConn.Close()
                Next
                Label11.Text = "Record(s) Updated Successfully!"
                Label12.Text = ""
                PopulateBillsGrid3()
            Else
                Label11.Text = ""
                Label12.Text = ""
            End If
        Catch q As SqlException
            thisTransaction.Rollback()
            Label2.Text = "Record(s) Not Updated!"
            Label1.Text = ""
        Catch s As Exception
            Label12.Text = ""
            Label1.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'populate pmt req no for updation
    Public Sub PopulatePmtReqNo()
        Dim SQL As String = Nothing
        Dim dt As DataTable = Nothing
        Try
            SQL = "SELECT DISTINCT PMT_REQ_NO FROM BILLS WHERE (LIB_CODE ='" & Trim(LibCode) & "') AND (STATUS ='Posted' or STATUS ='Paid') ORDER BY PMT_REQ_NO DESC "
            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(SQL, SqlConn)
            If SqlConn.State = 0 Then
                SqlConn.Open()
            End If
            da.Fill(ds)

            dt = ds.Tables(0).Copy
            If dt.Rows.Count = 0 Then
                Me.DDL_PmtReqNo.DataSource = Nothing
                DDL_PmtReqNo.DataBind()
            Else
                Me.DDL_PmtReqNo.DataSource = dt
                Me.DDL_PmtReqNo.DataTextField = "PMT_REQ_NO"
                Me.DDL_PmtReqNo.DataValueField = "PMT_REQ_NO"
                Me.DDL_PmtReqNo.DataBind()
                DDL_PmtReqNo.Items.Insert(0, "")
            End If

        Catch s As Exception
            Label17.Text = "Error: " & (s.Message())
            Label16.Text = ""
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'populate vendros for pmt updation
    Public Sub DDL_PmtReqNo_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDL_PmtReqNo.SelectedIndexChanged
        Dim SQL As String = Nothing
        Dim dt As DataTable = Nothing
        Try
            Dim myPmtReqNO As Integer = Nothing
            If DDL_PmtReqNo.Text <> "" Then
                myPmtReqNO = DDL_PmtReqNo.SelectedValue
                SQL = "SELECT DISTINCT BILLS.VEND_ID, VENDORS.VEND_NAME FROM  BILLS LEFT OUTER JOIN VENDORS ON BILLS.VEND_ID = VENDORS.VEND_ID WHERE (BILLS.LIB_CODE ='" & Trim(LibCode) & "') AND (BILLS.PMT_REQ_NO = '" & Trim(myPmtReqNO) & "') AND (BILLS.STATUS ='Posted' or BILLS.STATUS='Paid') ORDER BY VENDORS.VEND_NAME "
                Dim ds As New DataSet
                Dim da As New SqlDataAdapter(SQL, SqlConn)
                If SqlConn.State = 0 Then
                    SqlConn.Open()
                End If
                da.Fill(ds)

                dt = ds.Tables(0).Copy
                If dt.Rows.Count = 0 Then
                    Me.DDL_Vendors4.DataSource = Nothing
                    DDL_Vendors4.DataBind()
                    DDL_Vendors4.Items.Clear()
                    TR_BANK.Visible = False
                    TR_CHKNO.Visible = False
                    Bill_UpdatePmt_Bttn.Visible = False
                    txt_Bills_CheckAmount.Text = ""
                    DDL_PmtReqNo.Focus()
                Else
                    Me.DDL_Vendors4.DataSource = dt
                    Me.DDL_Vendors4.DataTextField = "VEND_NAME"
                    Me.DDL_Vendors4.DataValueField = "VEND_ID"
                    Me.DDL_Vendors4.DataBind()
                    DDL_Vendors4.Items.Insert(0, "")
                    DDL_Vendors4.Focus()
                End If
            Else
                Me.DDL_Vendors4.DataSource = Nothing
                DDL_Vendors4.DataBind()
                DDL_Vendors4.Items.Clear()
                TR_BANK.Visible = False
                TR_CHKNO.Visible = False
                Bill_UpdatePmt_Bttn.Visible = False
                txt_Bills_CheckAmount.Text = ""
                DDL_PmtReqNo.Focus()
            End If
            PopulateBillsGrid4()
        Catch s As Exception
            Label17.Text = "Error: " & (s.Message())
            Label16.Text = ""
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'populate acq records of a title
    Public Sub PopulateBillsGrid4()
        Dim dtSearch As DataTable = Nothing
        Try
            Dim myPmtReqNO As Integer = Nothing
            Dim myVend As Integer = Nothing
            If DDL_PmtReqNo.Text <> "" Then
                myPmtReqNO = DDL_PmtReqNo.SelectedValue
                If DDL_Vendors4.Text <> "" Then
                    myVend = DDL_Vendors4.SelectedValue
                Else
                    myVend = 0
                End If


                Dim SQL As String = Nothing
                If myVend = 0 Then
                    SQL = " SELECT BILLS.BILL_ID, BILLS.INV_NO, BILLS.INV_DATE, BILLS.INV_AMOUNT, BILLS.CUR_CODE, BILLS.AMOUNT_PASSED, BILLS.STATUS, BILLS.PMT_REQ_NO, BILLS.CHEQUE_NO, BILLS.CHEQUE_DATE, BILLS.CHEQUE_AMOUNT, BILLS.REMARKS, VENDORS.VEND_NAME FROM BILLS LEFT OUTER JOIN VENDORS ON BILLS.VEND_ID = VENDORS.VEND_ID WHERE (BILLS.STATUS='Paid' OR BILLS.STATUS='Posted') AND (BILLS.LIB_CODE = '" & Trim(LibCode) & "') AND (PMT_REQ_NO = '" & Trim(myPmtReqNO) & "')  ORDER BY VENDORS.VEND_NAME "
                Else
                    SQL = " SELECT BILLS.BILL_ID, BILLS.INV_NO, BILLS.INV_DATE, BILLS.INV_AMOUNT, BILLS.CUR_CODE, BILLS.AMOUNT_PASSED, BILLS.STATUS, BILLS.PMT_REQ_NO, BILLS.CHEQUE_NO, BILLS.CHEQUE_DATE, BILLS.CHEQUE_AMOUNT, BILLS.REMARKS, VENDORS.VEND_NAME FROM BILLS LEFT OUTER JOIN VENDORS ON BILLS.VEND_ID = VENDORS.VEND_ID WHERE (BILLS.STATUS='Paid' OR BILLS.STATUS='Posted') AND (BILLS.LIB_CODE = '" & Trim(LibCode) & "') AND (PMT_REQ_NO = '" & Trim(myPmtReqNO) & "') AND (BILLS.VEND_ID = '" & Trim(myVend) & "')  ORDER BY VENDORS.VEND_NAME "
                End If

                Dim ds As New DataSet
                Dim da As New SqlDataAdapter(SQL, SqlConn)
                If SqlConn.State = 0 Then
                    SqlConn.Open()
                End If
                da.Fill(ds)

                dtSearch = ds.Tables(0).Copy
                Dim RecordCount As Long = 0
                If dtSearch.Rows.Count = 0 Then
                    Me.Grid4.DataSource = Nothing
                    Grid4.DataBind()
                    Label18.Text = "Total Record(s): 0 "
                Else
                    Grid4.Visible = True
                    RecordCount = dtSearch.Rows.Count
                    Grid4.DataSource = dtSearch
                    Grid4.DataBind()
                    Label18.Text = "Total Record(s): " & RecordCount
                End If
                SqlConn.Close()
                ViewState("dt") = dtSearch
            Else
                Me.Grid4.DataSource = Nothing
                Grid4.DataBind()
                Label18.Text = "Total Record(s): 0 "
            End If
        Catch s As Exception
            Label17.Text = "Error-SAVE: " & (s.Message())
            Label16.Text = ""
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'grid view page index changing event
    Protected Sub Grid4_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles Grid4.PageIndexChanging
        Try
            'rebind datagrid
            Grid4.DataSource = ViewState("dt") 'temp
            Grid4.PageIndex = e.NewPageIndex
            index = e.NewPageIndex * Grid4.PageSize
            Grid4.DataBind()
        Catch s As Exception
            Label17.Text = "Error- There is error in page index "
            Label16.Text = ""
        End Try
    End Sub
    'gridview sorting event
    Protected Sub Grid4_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles Grid4.Sorting
        Dim temp As DataTable = CType(ViewState("dt"), DataTable)
        GridViewSortExpression = e.SortExpression
        Grid4.DataSource = temp
        Dim pageIndex As Integer = Grid4.PageIndex
        Grid4.DataSource = SortDataTable(Grid4.DataSource, False)
        Grid4.DataBind()
        Grid4.PageIndex = pageIndex
    End Sub
    Protected Sub Grid4_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Grid4.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Lblsr As Label = e.Row.FindControl("lblsr")
            Srno = index + 1
            If Lblsr Is Nothing = False Then
                Lblsr.Text = Srno
                index += 1
            End If
        End If
    End Sub
    'populate vendros for pmt updation
    Public Sub DDL_Vendor4_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDL_Vendors4.SelectedIndexChanged
        Dim SQL As String = Nothing
        Dim dt As DataTable = Nothing
        Try
            Dim myPmtReqNO As Integer = Nothing
            Dim myVendID As Integer = Nothing
            If DDL_Vendors4.Text <> "" Then
                TR_BANK.Visible = True
                TR_CHKNO.Visible = True
                Bill_UpdatePmt_Bttn.Visible = True
                Bill_DeletePmt_Bttn.Visible = True
                myPmtReqNO = DDL_PmtReqNo.SelectedValue
                myVendID = DDL_Vendors4.SelectedValue

                SQL = "SELECT BILL_ID, AMOUNT_PASSED, CHEQUE_AMOUNT, CHEQUE_DATE, STATUS, CHEQUE_NO, BANK_NAME, CHEQUE_DATE from BILLS WHERE (LIB_CODE ='" & Trim(LibCode) & "') AND (PMT_REQ_NO = '" & Trim(myPmtReqNO) & "') AND (VEND_ID = '" & Trim(myVendID) & "') " 'AND (STATUS ='Posted')"
                Dim ds As New DataSet
                Dim da As New SqlDataAdapter(SQL, SqlConn)
                If SqlConn.State = 0 Then
                    SqlConn.Open()
                End If
                da.Fill(ds)
                dt = ds.Tables(0).Copy
                If dt.Rows.Count <> 0 Then
                    If dt.Rows(0).Item("STATUS").ToString = "Posted" Then
                        Dim ChequeAmount As Decimal = Nothing
                        For i As Integer = 0 To dt.Rows.Count - 1
                            ChequeAmount = ChequeAmount + Convert.ToDecimal(dt.Rows(i).Item("AMOUNT_PASSED"))
                        Next
                        txt_Bills_CheckAmount.Text = ChequeAmount
                    Else
                        txt_Bills_CheckAmount.Text = Convert.ToDecimal(dt.Rows(0).Item("CHEQUE_AMOUNT"))
                        txt_Bills_ChequeNo.Text = dt.Rows(0).Item("CHEQUE_NO")
                        txt_Bills_ChequeDate.Text = Format(dt.Rows(0).Item("CHEQUE_DATE"), "dd/MM/yyyy")
                        txt_Bills_BankName.Text = dt.Rows(0).Item("BANK_NAME")
                    End If
                Else
                    txt_Bills_CheckAmount.Text = ""
                End If
            Else
                txt_Bills_CheckAmount.Text = ""
                TR_BANK.Visible = False
                TR_CHKNO.Visible = False
                Bill_UpdatePmt_Bttn.Visible = False
                Bill_DeletePmt_Bttn.Visible = False
            End If
            PopulateBillsGrid4()
            DDL_Vendors4.Focus()
        Catch s As Exception
            Label17.Text = "Error: " & (s.Message())
            Label16.Text = ""
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'update payment details in bills and in ACQ records
    Protected Sub Bill_UpdatePmt_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Bill_UpdatePmt_Bttn.Click
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing
        Try
            If IsPostBack = True Then
                If DDL_PmtReqNo.Text <> "" Then
                    Dim iloop As Integer
                    Dim strcurrentchar As Object
                    Dim c As Integer
                    Dim counter0, counter1, counter2, counter3, counter4, counter5 As Integer

                    'get PMT_REQ_NO
                    Dim PMT_REQ_NO As Integer = Nothing
                    PMT_REQ_NO = Trim(DDL_PmtReqNo.SelectedValue)
                    PMT_REQ_NO = RemoveQuotes(PMT_REQ_NO)

                    If IsNumeric(PMT_REQ_NO) = False Then
                        Label17.Text = "Plz Select Payment Request No from Drop-Down!"
                        Label16.Text = ""
                        DDL_PmtReqNo.Focus()
                        Exit Sub
                    End If
                    If PMT_REQ_NO.ToString.Length > 10 Then
                        Label17.Text = "Plz Enter Data with Proper Length!"
                        Label16.Text = ""
                        DDL_PmtReqNo.Focus()
                        Exit Sub
                    End If
                    PMT_REQ_NO = " " & PMT_REQ_NO & " "
                    If InStr(1, PMT_REQ_NO, "CREATE", 1) > 0 Or InStr(1, PMT_REQ_NO, "DELETE", 1) > 0 Or InStr(1, PMT_REQ_NO, "DROP", 1) > 0 Or InStr(1, PMT_REQ_NO, "INSERT", 1) > 1 Or InStr(1, PMT_REQ_NO, "TRACK", 1) > 1 Or InStr(1, PMT_REQ_NO, "TRACE", 1) > 1 Then
                        Label17.Text = "Do Not Use Reserve Words!"
                        Label16.Text = ""
                        DDL_PmtReqNo.Focus()
                        Exit Sub
                    End If
                    PMT_REQ_NO = TrimX(PMT_REQ_NO)

                    c = 0
                    counter0 = 0
                    For iloop = 1 To Len(PMT_REQ_NO.ToString)
                        strcurrentchar = Mid(PMT_REQ_NO, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>=%abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ,;:_", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter0 = 1
                            End If
                        End If
                    Next
                    If counter0 = 1 Then
                        Label17.Text = "Do Not Use Un-Wanted Characters!"
                        Label16.Text = ""
                        DDL_PmtReqNo.Focus()
                        Exit Sub
                    End If


                    If DDL_Vendors4.Text <> "" Then
                        'get VEND_ID
                        Dim VEND_ID As Integer = Nothing
                        VEND_ID = Trim(DDL_Vendors4.SelectedValue)
                        VEND_ID = RemoveQuotes(VEND_ID)

                        If IsNumeric(VEND_ID) = False Then
                            Label17.Text = "Plz Select Vendor from Drop-Down!"
                            Label16.Text = ""
                            DDL_Vendors4.Focus()
                            Exit Sub
                        End If
                        If VEND_ID.ToString.Length > 10 Then
                            Label17.Text = "Plz Enter Data with Proper Length!"
                            Label16.Text = ""
                            DDL_Vendors4.Focus()
                            Exit Sub
                        End If
                        VEND_ID = " " & VEND_ID & " "
                        If InStr(1, VEND_ID, "CREATE", 1) > 0 Or InStr(1, VEND_ID, "DELETE", 1) > 0 Or InStr(1, VEND_ID, "DROP", 1) > 0 Or InStr(1, VEND_ID, "INSERT", 1) > 1 Or InStr(1, VEND_ID, "TRACK", 1) > 1 Or InStr(1, VEND_ID, "TRACE", 1) > 1 Then
                            Label17.Text = "Do Not Use Reserve Words!"
                            Label16.Text = ""
                            DDL_Vendors4.Focus()
                            Exit Sub
                        End If
                        VEND_ID = TrimX(VEND_ID)

                        c = 0
                        counter1 = 0
                        For iloop = 1 To Len(VEND_ID.ToString)
                            strcurrentchar = Mid(VEND_ID, iloop, 1)
                            If c = 0 Then
                                If Not InStr("'~!@#$^&*=+|[]{}?<>=%abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ,;:_", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter1 = 1
                                End If
                            End If
                        Next
                        If counter1 = 1 Then
                            Label17.Text = "Do Not Use Un-Wanted Characters!"
                            Label16.Text = ""
                            DDL_Vendors4.Focus()
                            Exit Sub
                        End If

                        'Server Validation for CHEQUE_NO
                        Dim CHEQUE_NO As Object = Nothing
                        If txt_Bills_ChequeNo.Text <> "" Then
                            CHEQUE_NO = TrimAll(txt_Bills_ChequeNo.Text)
                            CHEQUE_NO = RemoveQuotes(CHEQUE_NO)

                            If CHEQUE_NO.ToString.Length > 50 Then
                                Label17.Text = "Plz Enter Data with Proper Length!"
                                Label16.Text = ""
                                txt_Bills_ChequeNo.Focus()
                                Exit Sub
                            End If
                            CHEQUE_NO = " " & CHEQUE_NO & " "
                            If InStr(1, CHEQUE_NO, "CREATE", 1) > 0 Or InStr(1, CHEQUE_NO, "DELETE", 1) > 0 Or InStr(1, CHEQUE_NO, "DROP", 1) > 0 Or InStr(1, CHEQUE_NO, "INSERT", 1) > 1 Or InStr(1, CHEQUE_NO, "TRACK", 1) > 1 Or InStr(1, CHEQUE_NO, "TRACE", 1) > 1 Then
                                Label17.Text = "Do Not Use Reserve Words!"
                                Label16.Text = ""
                                txt_Bills_ChequeNo.Focus()
                                Exit Sub
                            End If
                            CHEQUE_NO = TrimAll(CHEQUE_NO)

                            c = 0
                            counter2 = 0
                            For iloop = 1 To Len(CHEQUE_NO.ToString)
                                strcurrentchar = Mid(CHEQUE_NO, iloop, 1)
                                If c = 0 Then
                                    If Not InStr("'~!@#$^&*=+|[]{}?<>=%,;:_", strcurrentchar) <= 0 Then
                                        c = c + 1
                                        counter2 = 1
                                    End If
                                End If
                            Next
                            If counter2 = 1 Then
                                Label17.Text = "Do Not Use Un-Wanted Characters!"
                                Label16.Text = ""
                                txt_Bills_ChequeNo.Focus()
                                Exit Sub
                            End If
                        Else
                            CHEQUE_NO = ""
                        End If

                        'search CHEQUE date
                        Dim CHEQUE_DATE As Object = Nothing
                        If txt_Bills_ChequeDate.Text <> "" Then
                            CHEQUE_DATE = TrimX(txt_Bills_ChequeDate.Text)
                            CHEQUE_DATE = RemoveQuotes(CHEQUE_DATE)
                            CHEQUE_DATE = Convert.ToDateTime(CHEQUE_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                            If Len(CHEQUE_DATE.ToString) > 12 Then
                                Label17.Text = " Input is not Valid..."
                                Label16.Text = ""
                                Me.txt_Bills_ChequeDate.Focus()
                                Exit Sub
                            End If
                            CHEQUE_DATE = " " & CHEQUE_DATE & " "
                            If InStr(1, CHEQUE_DATE, "CREATE", 1) > 0 Or InStr(1, CHEQUE_DATE, "DELETE", 1) > 0 Or InStr(1, CHEQUE_DATE, "DROP", 1) > 0 Or InStr(1, CHEQUE_DATE, "INSERT", 1) > 1 Or InStr(1, CHEQUE_DATE, "TRACK", 1) > 1 Or InStr(1, CHEQUE_DATE, "TRACE", 1) > 1 Then
                                Label17.Text = "  Input is not Valid... "
                                Label16.Text = ""
                                Me.txt_Bills_ChequeDate.Focus()
                                Exit Sub
                            End If
                            CHEQUE_DATE = TrimX(CHEQUE_DATE)
                            'check unwanted characters
                            c = 0
                            counter3 = 0
                            For iloop = 1 To Len(CHEQUE_DATE.ToString)
                                strcurrentchar = Mid(CHEQUE_DATE, iloop, 1)
                                If c = 0 Then
                                    If Not InStr("'~!@#$^&*=+|[]{}?<>=%abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ,;:_", strcurrentchar) <= 0 Then
                                        c = c + 1
                                        counter3 = 1
                                    End If
                                End If
                            Next
                            If counter3 = 1 Then
                                Label17.Text = "data is not Valid... "
                                Label16.Text = ""
                                Me.txt_Bills_ChequeDate.Focus()
                                Exit Sub
                            End If
                        Else
                            CHEQUE_DATE = Now.Date
                            CHEQUE_DATE = Convert.ToDateTime(CHEQUE_DATE, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us
                        End If

                        'Server Validation for BANK_NAME
                        Dim BANK_NAME As Object = Nothing
                        If txt_Bills_BankName.Text <> "" Then
                            BANK_NAME = TrimAll(txt_Bills_BankName.Text)
                            BANK_NAME = RemoveQuotes(BANK_NAME)

                            If BANK_NAME.ToString.Length > 250 Then
                                Label17.Text = "Plz Enter Data with Proper Length!"
                                Label16.Text = ""
                                txt_Bills_BankName.Focus()
                                Exit Sub
                            End If
                            BANK_NAME = " " & BANK_NAME & " "
                            If InStr(1, BANK_NAME, "CREATE", 1) > 0 Or InStr(1, BANK_NAME, "DELETE", 1) > 0 Or InStr(1, BANK_NAME, "DROP", 1) > 0 Or InStr(1, BANK_NAME, "INSERT", 1) > 1 Or InStr(1, BANK_NAME, "TRACK", 1) > 1 Or InStr(1, BANK_NAME, "TRACE", 1) > 1 Then
                                Label17.Text = "Do Not Use Reserve Words!"
                                Label16.Text = ""
                                txt_Bills_BankName.Focus()
                                Exit Sub
                            End If
                            BANK_NAME = TrimAll(BANK_NAME)

                            c = 0
                            counter4 = 0
                            For iloop = 1 To Len(BANK_NAME.ToString)
                                strcurrentchar = Mid(BANK_NAME, iloop, 1)
                                If c = 0 Then
                                    If Not InStr("'~!@#$^&*=+|[]{}?<>=%,;:_", strcurrentchar) <= 0 Then
                                        c = c + 1
                                        counter4 = 1
                                    End If
                                End If
                            Next
                            If counter4 = 1 Then
                                Label17.Text = "Do Not Use Un-Wanted Characters!"
                                Label16.Text = ""
                                txt_Bills_BankName.Focus()
                                Exit Sub
                            End If
                        Else
                            BANK_NAME = ""
                        End If

                        'Server Validation for CHEQUE_AMOUNT
                        Dim CHEQUE_AMOUNT As Object = Nothing
                        If txt_Bills_CheckAmount.Text <> "" Then
                            CHEQUE_AMOUNT = TrimX(txt_Bills_CheckAmount.Text)
                            CHEQUE_AMOUNT = RemoveQuotes(CHEQUE_AMOUNT)
                            If CHEQUE_AMOUNT.ToString.Length > 15 Then
                                Label17.Text = "Plz Enter Data with Proper Length!"
                                Label16.Text = ""
                                txt_Bills_CheckAmount.Focus()
                                Exit Sub
                            End If
                            CHEQUE_AMOUNT = " " & CHEQUE_AMOUNT & " "
                            If InStr(1, CHEQUE_AMOUNT, "CREATE", 1) > 0 Or InStr(1, CHEQUE_AMOUNT, "DELETE", 1) > 0 Or InStr(1, CHEQUE_AMOUNT, "DROP", 1) > 0 Or InStr(1, CHEQUE_AMOUNT, "INSERT", 1) > 1 Or InStr(1, CHEQUE_AMOUNT, "TRACK", 1) > 1 Or InStr(1, CHEQUE_AMOUNT, "TRACE", 1) > 1 Then
                                Label17.Text = "Do Not Use Reserve Words!"
                                Label16.Text = ""
                                txt_Bills_CheckAmount.Focus()
                                Exit Sub
                            End If
                            CHEQUE_AMOUNT = TrimX(CHEQUE_AMOUNT)
                            c = 0
                            counter5 = 0
                            For iloop = 1 To Len(CHEQUE_AMOUNT.ToString)
                                strcurrentchar = Mid(CHEQUE_AMOUNT, iloop, 1)
                                If c = 0 Then
                                    If Not InStr("'~!@#$^&*=+|[]{}?<>()=%abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ,;:_", strcurrentchar) <= 0 Then
                                        c = c + 1
                                        counter5 = 1
                                    End If
                                End If
                            Next
                            If counter5 = 1 Then
                                Label17.Text = "Do Not Use Un-Wanted Characters!"
                                Label16.Text = ""
                                txt_Bills_CheckAmount.Focus()
                                Exit Sub
                            End If
                        Else
                            Label17.Text = "Plz Enter Cheque Amount!"
                            Label16.Text = ""
                            Me.txt_Bills_CheckAmount.Focus()
                            Exit Sub
                        End If


                        If Grid4.Rows.Count <> 0 Then
                            For Each row As GridViewRow In Grid4.Rows
                                If row.RowType = DataControlRowType.DataRow Then
                                    Dim BILL_ID As Integer = Convert.ToInt32(Grid4.DataKeys(row.RowIndex).Value)
                                    'bill ID
                                    BILL_ID = RemoveQuotes(BILL_ID)
                                    If IsNumeric(BILL_ID) = False Then
                                        Label17.Text = "Plz Select Proper Data!"
                                        Label16.Text = ""
                                        Exit Sub
                                    End If
                                    If BILL_ID.ToString.Length > 10 Then
                                        Label17.Text = "Plz Enter Data with Proper Length!"
                                        Label16.Text = ""
                                        Exit Sub
                                    End If
                                    BILL_ID = " " & BILL_ID & " "
                                    If InStr(1, BILL_ID, "CREATE", 1) > 0 Or InStr(1, BILL_ID, "DELETE", 1) > 0 Or InStr(1, BILL_ID, "DROP", 1) > 0 Or InStr(1, BILL_ID, "INSERT", 1) > 1 Or InStr(1, BILL_ID, "TRACK", 1) > 1 Or InStr(1, BILL_ID, "TRACE", 1) > 1 Then
                                        Label17.Text = "Do Not Use Reserve Words!"
                                        Label16.Text = ""
                                        Exit Sub
                                    End If

                                    Dim DATE_MODIFIED As Object = Nothing
                                    DATE_MODIFIED = Now.Date
                                    DATE_MODIFIED = Convert.ToDateTime(DATE_MODIFIED, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                                    Dim IP As Object = Nothing
                                    IP = Request.UserHostAddress.Trim

                                    SqlConn.Open()
                                    thisTransaction = SqlConn.BeginTransaction()
                                    'update Bills record
                                    Dim objCommand1 As New SqlCommand
                                    objCommand1.Connection = SqlConn
                                    objCommand1.Transaction = thisTransaction
                                    objCommand1.CommandType = CommandType.Text
                                    objCommand1.CommandText = "UPDATE ACQUISITIONS SET PROCESS_STATUS=@PROCESS_STATUS , UPDATED_BY=@UPDATED_BY, DATE_MODIFIED=@DATE_MODIFIED, IP=@IP FROM ACQUISITIONS WHERE (BILL_ID = @BILL_ID and PROCESS_STATUS = 'Billed'  AND LIB_CODE =@LIB_CODE) "

                                    objCommand1.Parameters.Add("@BILL_ID", SqlDbType.Int)
                                    objCommand1.Parameters("@BILL_ID").Value = BILL_ID

                                    objCommand1.Parameters.Add("@PROCESS_STATUS", SqlDbType.VarChar)
                                    objCommand1.Parameters("@PROCESS_STATUS").Value = "Paid"

                                    objCommand1.Parameters.Add("@LIB_CODE", SqlDbType.NVarChar)
                                    objCommand1.Parameters("@LIB_CODE").Value = LibCode

                                    objCommand1.Parameters.Add("@UPDATED_BY", SqlDbType.NVarChar)
                                    objCommand1.Parameters("@UPDATED_BY").Value = UserCode

                                    objCommand1.Parameters.Add("@DATE_MODIFIED", SqlDbType.NVarChar)
                                    objCommand1.Parameters("@DATE_MODIFIED").Value = DATE_MODIFIED

                                    objCommand1.Parameters.Add("@IP", SqlDbType.VarChar)
                                    objCommand1.Parameters("@IP").Value = IP

                                    objCommand1.ExecuteNonQuery()



                                    'update BILLS Table with Status ='Billed'
                                    Dim objCommand As New SqlCommand
                                    objCommand.Connection = SqlConn
                                    objCommand.Transaction = thisTransaction
                                    objCommand.CommandType = CommandType.Text
                                    objCommand.CommandText = "UPDATE BILLS SET STATUS=@STATUS, CHEQUE_NO=@CHEQUE_NO, CHEQUE_DATE=@CHEQUE_DATE, BANK_NAME=@BANK_NAME, CHEQUE_AMOUNT=@CHEQUE_AMOUNT, UPDATED_BY=@UPDATED_BY, DATE_MODIFIED=@DATE_MODIFIED, IP=@IP FROM BILLS WHERE (BILL_ID = @BILL_ID and STATUS = 'Posted'  AND LIB_CODE =@LIB_CODE AND PMT_REQ_NO=@PMT_REQ_NO AND VEND_ID=@VEND_ID) "

                                    objCommand.Parameters.Add("@BILL_ID", SqlDbType.Int)
                                    objCommand.Parameters("@BILL_ID").Value = BILL_ID

                                    objCommand.Parameters.Add("@PMT_REQ_NO", SqlDbType.Int)
                                    objCommand.Parameters("@PMT_REQ_NO").Value = PMT_REQ_NO

                                    objCommand.Parameters.Add("@VEND_ID", SqlDbType.Int)
                                    objCommand.Parameters("@VEND_ID").Value = VEND_ID

                                    objCommand.Parameters.Add("@CHEQUE_NO", SqlDbType.NVarChar)
                                    If CHEQUE_NO = "" Then
                                        objCommand.Parameters("@CHEQUE_NO").Value = System.DBNull.Value
                                    Else
                                        objCommand.Parameters("@CHEQUE_NO").Value = CHEQUE_NO
                                    End If

                                    objCommand.Parameters.Add("@CHEQUE_DATE", SqlDbType.DateTime)
                                    If CHEQUE_DATE = "" Then
                                        objCommand.Parameters("@CHEQUE_DATE").Value = System.DBNull.Value
                                    Else
                                        objCommand.Parameters("@CHEQUE_DATE").Value = CHEQUE_DATE
                                    End If

                                    objCommand.Parameters.Add("@BANK_NAME", SqlDbType.NVarChar)
                                    If BANK_NAME = "" Then
                                        objCommand.Parameters("@BANK_NAME").Value = System.DBNull.Value
                                    Else
                                        objCommand.Parameters("@BANK_NAME").Value = BANK_NAME
                                    End If

                                    objCommand.Parameters.Add("@CHEQUE_AMOUNT", SqlDbType.Decimal)
                                    If CHEQUE_AMOUNT = 0 Then
                                        objCommand.Parameters("@CHEQUE_AMOUNT").Value = System.DBNull.Value
                                    Else
                                        objCommand.Parameters("@CHEQUE_AMOUNT").Value = CHEQUE_AMOUNT
                                    End If

                                    objCommand.Parameters.Add("@STATUS", SqlDbType.VarChar)
                                    objCommand.Parameters("@STATUS").Value = "Paid"

                                    objCommand.Parameters.Add("@LIB_CODE", SqlDbType.NVarChar)
                                    objCommand.Parameters("@LIB_CODE").Value = LibCode

                                    objCommand.Parameters.Add("@UPDATED_BY", SqlDbType.NVarChar)
                                    objCommand.Parameters("@UPDATED_BY").Value = UserCode

                                    objCommand.Parameters.Add("@DATE_MODIFIED", SqlDbType.NVarChar)
                                    objCommand.Parameters("@DATE_MODIFIED").Value = DATE_MODIFIED

                                    objCommand.Parameters.Add("@IP", SqlDbType.VarChar)
                                    objCommand.Parameters("@IP").Value = IP

                                    objCommand.ExecuteNonQuery()

                                    thisTransaction.Commit()
                                    'End If
                                End If
                                SqlConn.Close()
                            Next
                            Label16.Text = "Record(s) Updated Successfully!"
                            Label17.Text = ""
                            PopulateBillsGrid4()

                        Else
                            Label17.Text = ""
                            Label16.Text = ""
                        End If
                    Else
                        Label17.Text = "Plz Select Vendor No from Drop-Down!"
                        Label16.Text = ""
                        DDL_Vendors4.Focus()
                    End If
                Else
                    Label17.Text = "Plz Select Payment Request No from Drop-Down!"
                    Label16.Text = ""
                    DDL_PmtReqNo.Focus()
                End If
            End If
        Catch q As SqlException
            thisTransaction.Rollback()
            Label17.Text = "Record(s) Not Updated!"
            Label16.Text = ""
        Catch s As Exception
            Label16.Text = ""
            Label17.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
    'delete payment details from ACQ and BILLS table
    Protected Sub Bill_DeletePmt_Bttn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Bill_DeletePmt_Bttn.Click
        Dim thisTransaction As SqlClient.SqlTransaction = Nothing
        Try
            If IsPostBack = True Then
                If DDL_PmtReqNo.Text <> "" Then
                    Dim iloop As Integer
                    Dim strcurrentchar As Object
                    Dim c As Integer
                    Dim counter0, counter1, counter2, counter3, counter4, counter5 As Integer

                    'get PMT_REQ_NO
                    Dim PMT_REQ_NO As Integer = Nothing
                    PMT_REQ_NO = Trim(DDL_PmtReqNo.SelectedValue)
                    PMT_REQ_NO = RemoveQuotes(PMT_REQ_NO)

                    If IsNumeric(PMT_REQ_NO) = False Then
                        Label17.Text = "Plz Select Payment Request No from Drop-Down!"
                        Label16.Text = ""
                        DDL_PmtReqNo.Focus()
                        Exit Sub
                    End If
                    If PMT_REQ_NO.ToString.Length > 10 Then
                        Label17.Text = "Plz Enter Data with Proper Length!"
                        Label16.Text = ""
                        DDL_PmtReqNo.Focus()
                        Exit Sub
                    End If
                    PMT_REQ_NO = " " & PMT_REQ_NO & " "
                    If InStr(1, PMT_REQ_NO, "CREATE", 1) > 0 Or InStr(1, PMT_REQ_NO, "DELETE", 1) > 0 Or InStr(1, PMT_REQ_NO, "DROP", 1) > 0 Or InStr(1, PMT_REQ_NO, "INSERT", 1) > 1 Or InStr(1, PMT_REQ_NO, "TRACK", 1) > 1 Or InStr(1, PMT_REQ_NO, "TRACE", 1) > 1 Then
                        Label17.Text = "Do Not Use Reserve Words!"
                        Label16.Text = ""
                        DDL_PmtReqNo.Focus()
                        Exit Sub
                    End If
                    PMT_REQ_NO = TrimX(PMT_REQ_NO)

                    c = 0
                    counter0 = 0
                    For iloop = 1 To Len(PMT_REQ_NO.ToString)
                        strcurrentchar = Mid(PMT_REQ_NO, iloop, 1)
                        If c = 0 Then
                            If Not InStr("'~!@#$^&*=+|[]{}?<>=%abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ,;:_", strcurrentchar) <= 0 Then
                                c = c + 1
                                counter0 = 1
                            End If
                        End If
                    Next
                    If counter0 = 1 Then
                        Label17.Text = "Do Not Use Un-Wanted Characters!"
                        Label16.Text = ""
                        DDL_PmtReqNo.Focus()
                        Exit Sub
                    End If


                    If DDL_Vendors4.Text <> "" Then
                        'get VEND_ID
                        Dim VEND_ID As Integer = Nothing
                        VEND_ID = Trim(DDL_Vendors4.SelectedValue)
                        VEND_ID = RemoveQuotes(VEND_ID)

                        If IsNumeric(VEND_ID) = False Then
                            Label17.Text = "Plz Select Vendor from Drop-Down!"
                            Label16.Text = ""
                            DDL_Vendors4.Focus()
                            Exit Sub
                        End If
                        If VEND_ID.ToString.Length > 10 Then
                            Label17.Text = "Plz Enter Data with Proper Length!"
                            Label16.Text = ""
                            DDL_Vendors4.Focus()
                            Exit Sub
                        End If
                        VEND_ID = " " & VEND_ID & " "
                        If InStr(1, VEND_ID, "CREATE", 1) > 0 Or InStr(1, VEND_ID, "DELETE", 1) > 0 Or InStr(1, VEND_ID, "DROP", 1) > 0 Or InStr(1, VEND_ID, "INSERT", 1) > 1 Or InStr(1, VEND_ID, "TRACK", 1) > 1 Or InStr(1, VEND_ID, "TRACE", 1) > 1 Then
                            Label17.Text = "Do Not Use Reserve Words!"
                            Label16.Text = ""
                            DDL_Vendors4.Focus()
                            Exit Sub
                        End If
                        VEND_ID = TrimX(VEND_ID)

                        c = 0
                        counter1 = 0
                        For iloop = 1 To Len(VEND_ID.ToString)
                            strcurrentchar = Mid(VEND_ID, iloop, 1)
                            If c = 0 Then
                                If Not InStr("'~!@#$^&*=+|[]{}?<>=%abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ,;:_", strcurrentchar) <= 0 Then
                                    c = c + 1
                                    counter1 = 1
                                End If
                            End If
                        Next
                        If counter1 = 1 Then
                            Label17.Text = "Do Not Use Un-Wanted Characters!"
                            Label16.Text = ""
                            DDL_Vendors4.Focus()
                            Exit Sub
                        End If


                        If Grid4.Rows.Count <> 0 Then
                            For Each row As GridViewRow In Grid4.Rows
                                If row.RowType = DataControlRowType.DataRow Then
                                    Dim BILL_ID As Integer = Convert.ToInt32(Grid4.DataKeys(row.RowIndex).Value)
                                    'bill ID
                                    BILL_ID = RemoveQuotes(BILL_ID)
                                    If IsNumeric(BILL_ID) = False Then
                                        Label17.Text = "Plz Select Proper Data!"
                                        Label16.Text = ""
                                        Exit Sub
                                    End If
                                    If BILL_ID.ToString.Length > 10 Then
                                        Label17.Text = "Plz Enter Data with Proper Length!"
                                        Label16.Text = ""
                                        Exit Sub
                                    End If
                                    BILL_ID = " " & BILL_ID & " "
                                    If InStr(1, BILL_ID, "CREATE", 1) > 0 Or InStr(1, BILL_ID, "DELETE", 1) > 0 Or InStr(1, BILL_ID, "DROP", 1) > 0 Or InStr(1, BILL_ID, "INSERT", 1) > 1 Or InStr(1, BILL_ID, "TRACK", 1) > 1 Or InStr(1, BILL_ID, "TRACE", 1) > 1 Then
                                        Label17.Text = "Do Not Use Reserve Words!"
                                        Label16.Text = ""
                                        Exit Sub
                                    End If

                                    Dim DATE_MODIFIED As Object = Nothing
                                    DATE_MODIFIED = Now.Date
                                    DATE_MODIFIED = Convert.ToDateTime(DATE_MODIFIED, System.Globalization.CultureInfo.GetCultureInfo("hi-IN")).ToString("MM/dd/yyyy") ' convert from indian to us

                                    Dim IP As Object = Nothing
                                    IP = Request.UserHostAddress.Trim

                                    SqlConn.Open()
                                    thisTransaction = SqlConn.BeginTransaction()
                                    'update Bills record
                                    Dim objCommand1 As New SqlCommand
                                    objCommand1.Connection = SqlConn
                                    objCommand1.Transaction = thisTransaction
                                    objCommand1.CommandType = CommandType.Text
                                    objCommand1.CommandText = "UPDATE ACQUISITIONS SET PROCESS_STATUS=@PROCESS_STATUS , UPDATED_BY=@UPDATED_BY, DATE_MODIFIED=@DATE_MODIFIED, IP=@IP FROM ACQUISITIONS WHERE (BILL_ID = @BILL_ID and PROCESS_STATUS = 'Paid'  AND LIB_CODE =@LIB_CODE) "

                                    objCommand1.Parameters.Add("@BILL_ID", SqlDbType.Int)
                                    objCommand1.Parameters("@BILL_ID").Value = BILL_ID

                                    objCommand1.Parameters.Add("@PROCESS_STATUS", SqlDbType.VarChar)
                                    objCommand1.Parameters("@PROCESS_STATUS").Value = "Billed"

                                    objCommand1.Parameters.Add("@LIB_CODE", SqlDbType.NVarChar)
                                    objCommand1.Parameters("@LIB_CODE").Value = LibCode

                                    objCommand1.Parameters.Add("@UPDATED_BY", SqlDbType.NVarChar)
                                    objCommand1.Parameters("@UPDATED_BY").Value = UserCode

                                    objCommand1.Parameters.Add("@DATE_MODIFIED", SqlDbType.NVarChar)
                                    objCommand1.Parameters("@DATE_MODIFIED").Value = DATE_MODIFIED

                                    objCommand1.Parameters.Add("@IP", SqlDbType.VarChar)
                                    objCommand1.Parameters("@IP").Value = IP

                                    objCommand1.ExecuteNonQuery()



                                    'update BILLS Table with Status ='Billed'
                                    Dim objCommand As New SqlCommand
                                    objCommand.Connection = SqlConn
                                    objCommand.Transaction = thisTransaction
                                    objCommand.CommandType = CommandType.Text
                                    objCommand.CommandText = "UPDATE BILLS SET STATUS=@STATUS, CHEQUE_NO=@CHEQUE_NO, CHEQUE_DATE=@CHEQUE_DATE, BANK_NAME=@BANK_NAME, CHEQUE_AMOUNT=@CHEQUE_AMOUNT, UPDATED_BY=@UPDATED_BY, DATE_MODIFIED=@DATE_MODIFIED, IP=@IP FROM BILLS WHERE (BILL_ID = @BILL_ID and STATUS = 'Paid'  AND LIB_CODE =@LIB_CODE AND PMT_REQ_NO=@PMT_REQ_NO AND VEND_ID=@VEND_ID) "

                                    objCommand.Parameters.Add("@BILL_ID", SqlDbType.Int)
                                    objCommand.Parameters("@BILL_ID").Value = BILL_ID

                                    objCommand.Parameters.Add("@PMT_REQ_NO", SqlDbType.Int)
                                    objCommand.Parameters("@PMT_REQ_NO").Value = PMT_REQ_NO

                                    objCommand.Parameters.Add("@VEND_ID", SqlDbType.Int)
                                    objCommand.Parameters("@VEND_ID").Value = VEND_ID

                                    objCommand.Parameters.Add("@CHEQUE_NO", SqlDbType.NVarChar)
                                    objCommand.Parameters("@CHEQUE_NO").Value = System.DBNull.Value

                                    objCommand.Parameters.Add("@CHEQUE_DATE", SqlDbType.DateTime)
                                    objCommand.Parameters("@CHEQUE_DATE").Value = System.DBNull.Value

                                    objCommand.Parameters.Add("@BANK_NAME", SqlDbType.NVarChar)
                                    objCommand.Parameters("@BANK_NAME").Value = System.DBNull.Value

                                    objCommand.Parameters.Add("@CHEQUE_AMOUNT", SqlDbType.Decimal)
                                    objCommand.Parameters("@CHEQUE_AMOUNT").Value = System.DBNull.Value

                                    objCommand.Parameters.Add("@STATUS", SqlDbType.VarChar)
                                    objCommand.Parameters("@STATUS").Value = "Posted"

                                    objCommand.Parameters.Add("@LIB_CODE", SqlDbType.NVarChar)
                                    objCommand.Parameters("@LIB_CODE").Value = LibCode

                                    objCommand.Parameters.Add("@UPDATED_BY", SqlDbType.NVarChar)
                                    objCommand.Parameters("@UPDATED_BY").Value = UserCode

                                    objCommand.Parameters.Add("@DATE_MODIFIED", SqlDbType.NVarChar)
                                    objCommand.Parameters("@DATE_MODIFIED").Value = DATE_MODIFIED

                                    objCommand.Parameters.Add("@IP", SqlDbType.VarChar)
                                    objCommand.Parameters("@IP").Value = IP

                                    objCommand.ExecuteNonQuery()

                                    thisTransaction.Commit()
                                End If
                                SqlConn.Close()
                            Next
                            Label16.Text = "Record(s) Updated Successfully!"
                            Label17.Text = ""
                            PopulateBillsGrid4()
                        Else
                            Label17.Text = ""
                            Label16.Text = ""
                        End If
                    Else
                        Label17.Text = "Plz Select Vendor No from Drop-Down!"
                        Label16.Text = ""
                        DDL_Vendors4.Focus()
                    End If
                Else
                    Label17.Text = "Plz Select Payment Request No from Drop-Down!"
                    Label16.Text = ""
                    DDL_PmtReqNo.Focus()
                End If
            End If
        Catch q As SqlException
            thisTransaction.Rollback()
            Label17.Text = "Record(s) Not Updated!"
            Label16.Text = ""
        Catch s As Exception
            Label16.Text = ""
            Label17.Text = "Error: " & (s.Message())
        Finally
            SqlConn.Close()
        End Try
    End Sub
End Class