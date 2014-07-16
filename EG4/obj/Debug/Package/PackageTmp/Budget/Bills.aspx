<%@ Page  Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Bills.aspx.vb" Inherits="EG4.Bills" %>


<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
         .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 550px;
            border-style: solid;
            border-width: 1px;
        }
        .style3
        {
            text-align: center;
            vertical-align: middle;
            height: 38px;
            width: 97%;
            margin-left:10px;  
        }
         .style4
        {
            text-align: center;
            vertical-align: middle;
            width: 100%;
        }
                             
        .style13
        {
            width: 19%;
            border-collapse: collapse;
            border: 1px solid #4E667D;
        }
        .style15
        {
            width: 40%;
            border-collapse: collapse;
            border: 1px solid #4E667D;
        }
        .style16
        {
            text-align: center;
        }
        .style17
        {
            width: 40%;
            border-collapse: collapse;
            border: 1px solid #4E5766;
        }
        .style21
        {
            width: 19%;
            border-collapse: collapse;
            text-align: center;
            border: 1px solid #4E667D;
            margin:    10px auto 0px  25px;
            padding: 0px;
            height: 157px;
        }
                        
        .style35
        {
            width: 98%;
            color: #003399;
            text-align: right;
            border-width: 0px;   
            margin-left:10px;          
        }
    
      .style43
    {
        text-align: center;
        font-size: large;
        color: #336699;
        height: 17px;       
        border-style:outset;
    }
        .style44
    {
        font-size: large;
        color: #336699;    
        border-style:outset;
    }
                
        .style47
    {
        text-align: center;
        border-style: none;
        border-color: inherit;
        padding: 0px;
        background-color:#99CCFF;
        font-size:small;
    }
                  
        .styleBttn
    {
             cursor:pointer;
            margin-left: 0px;
            }
               
                
        .style53
        {
            text-align: justify;
            border-style: none;
            border-color: inherit;
            width: 170px;
            padding: 0px;
            background-color: #99CCFF;
            font-size: small;
            height: 18px;
        }
        
       
        .style54
        {
             text-align: left;
            border-style: none;
            padding: 0px;
            font-weight: bold;
            background-color: #D5EAFF;
        }
        .style55
        {
            text-align: justify;
            border-style: none;
            border-color: inherit;
            padding: 0px;
             background-color:#D5EAFF;  
            font-size: small;
            height: 18px;
        }
        .style56
        {
            text-align: center;
            width: 100%;
            background-color:#336699;
                      
        }
         #upload {
      border: solid 3px #ccc;
    }

    .preview-image {
      display: block;
      margin: 10px 0;
      border: solid 3px #aaa;
      padding: 1px;
      background: #fff;
    }    
    
     .PromptCSS  
        {  
            color:Blue;  
            font-size:small;  
            font-weight:bold;  
            font-family:Arial;
            background-color:Silver;  
            height:20px;    
            }    
      
      
        .style57
        {
            text-align: justify;
            border-style: none;
            border-color: inherit;
            width: 15%;
            padding: 0px;
            background-color: #99CCFF;
            font-size: small;
            height: 14px;
        }
        .style58
        {
            text-align: justify;
            border-style: none;
            border-color: inherit;
            width: 80%;
            padding: 0px;
            background-color: #D5EAFF;
            font-size: small;
            height: 14px;
        }
                
        </style>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

     <script type ="text/javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                alert("Please Enter Numeric Characters Only!");
                return false;
            }

            return true;
        }

   </script>
   <script type ="text/javascript">
       function isCurrencyKey(evt) {
           var charCode = (evt.which) ? evt.which : event.keyCode
           if (charCode != 46 && (charCode < 48 || charCode > 57) && charCode != 32) {
               alert("Please Enter Numeric with Decimal Characters Only!");
               return false;
           }

           return true;
       }

   </script>

    <script type="text/javascript">
        function formfocus() {
            document.getElementById('<%= DDL_BudgYear.ClientID %>').focus();
        }
        window.onload = formfocus;
    </script>
    <script type ="text/javascript">
        //alpha-numeric only
        function DateOnly(event) {
            var chCode = ('charCode' in event) ? event.charCode : event.keyCode;
            if (47 <= chCode && chCode <= 57) {
                return (true);
            }

            else {
                alert("Date Is Invalid, Enter in dd/MM/yyyy format only!");
                document.getElementById("MainContent_txt_Bill_BillDate").focus();
                return (false);
            }
        }
    </script>
     <script type ="text/javascript">
         //alpha-numeric only
         function DateOnly2(event) {
             var chCode = ('charCode' in event) ? event.charCode : event.keyCode;
             if (47 <= chCode && chCode <= 57) {
                 return (true);
             }

             else {
                 alert("Date Is Invalid, Enter in dd/MM/yyyy format only!");
                 document.getElementById("MainContent_txt_Bill_PmtReqDate").focus();
                 return (false);
             }
         }
    </script>
    <script type ="text/javascript">
        //alpha-numeric only
        function DateOnly3(event) {
            var chCode = ('charCode' in event) ? event.charCode : event.keyCode;
            if (47 <= chCode && chCode <= 57) {
                return (true);
            }

            else {
                alert("Date Is Invalid, Enter in dd/MM/yyyy format only!");
                document.getElementById("MainContent_txt_Bills_ChequeDate").focus();
                return (false);
            }
        }
    </script>


     <script language="javascript" type="text/javascript">

         function valid1() {

             if (document.getElementById('<%=DDL_BudgHead.ClientID%>').value == "") {
                 alert("Please select Prper \"Budget Head\" from Drop-Down.");
                 document.getElementById("MainContent_DDL_BudgHead").focus();
                 return (false);
             }

             if (document.getElementById('<%=txt_Bill_BillNo.ClientID%>').value == "") {
                 alert("Please Enter Proper \" Bill No\" field.");
                 document.getElementById("MainContent_txt_Bill_BillNo").focus();
                 return (false);
             }
             if (document.getElementById('<%=txt_Bill_BillDate.ClientID%>').value == "") {
                 alert("Please Enter Proper \" Bill Date\" field.");
                 document.getElementById("MainContent_txt_Bill_BillDate").focus();
                 return (false);
             }

             if (document.getElementById('<%=DDL_Currencies.ClientID%>').value == "") {
                 alert("Please Select  Proper \" Currency \" from Drop-Down.");
                 document.getElementById("MainContent_DDL_Currencies").focus();
                 return (false);
             }
             if (document.getElementById('<%=txt_Bill_Amount.ClientID%>').value == "") {
                 alert("Please Enter Proper \" Gross Amount \" Field.");
                 document.getElementById("MainContent_txt_Bill_Amount").focus();
                 return (false);
             }
             if (document.getElementById('<%=DDL_Vendors.ClientID%>').value == "") {
                 alert("Please Select \" Vendor \" Field from Drop-Down.");
                 document.getElementById("MainContent_DDL_Vendors").focus();
                 return (false);
             }
             return (true);
         }
    </script>
     
      <script language ="javascript" type ="text/javascript" >
          function GetCheckStatus() {
              var srcControlId = event.srcElement.id;
              var targetControlId = event.srcElement.id.replace('cbd', 'txt_Bill_ConversionRate2');
              if (document.getElementById(srcControlId).checked)
                  document.getElementById(targetControlId).disabled = false;
              else
                  document.getElementById(targetControlId).disabled = true;

              targetControlId = event.srcElement.id.replace('cbd', 'txt_Bill_OtherCharges2');
              if (document.getElementById(srcControlId).checked)
                  document.getElementById(targetControlId).disabled = false;
              else
                  document.getElementById(targetControlId).disabled = true;





          }

          function Select(Select) {

              var grdv = document.getElementById('<%= Grid1.ClientID %>');
              var chbk = "cbd";

              var Inputs = grdv.getElementsByTagName("input");

              for (var n = 0; n < Inputs.length; ++n) {
                  if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(chbk, 0) >= 0) {
                      Inputs[n].checked = Select;
                  }
              }
              return false;
          }

    </script> 

    
      <script language ="javascript" type ="text/javascript" >
          function Select2(Select2) {

              var grdv = document.getElementById('<%= Grid2.ClientID %>');
              var chbk = "cbd";
             // var txtbk = "txt_Bill_ConversionRate";
              var txtbk = new Array("txt_Bill_ConversionRate2", "txt_Bill_OtherCharges2");

              var Inputs = grdv.getElementsByTagName("input");
              for (var i = 0; i<txtbk.length; i++) {
                  for (var n = 0; n < Inputs.length; ++n) {
                      if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(chbk, 0) >= 0) {
                          Inputs[n].checked = Select2;
                      }
                      if (Inputs[n].type == 'text' && Inputs[n].id.indexOf(txtbk[i], 0) >= 0) {
                          if (Select2 == true) {
                              Inputs[n].disabled = false;
                          }
                          else {
                              Inputs[n].disabled = true;
                          }
                      }
                  }
              }
              return false;
          }
        
      

    </script> 

     <script language ="javascript" type ="text/javascript" >
         function Select3(Select3) {

             var grdv = document.getElementById('<%= Grid3.ClientID %>');
             var chbk = "cbd";

             var Inputs = grdv.getElementsByTagName("input");

             for (var n = 0; n < Inputs.length; ++n) {
                 if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(chbk, 0) >= 0) {
                     Inputs[n].checked = Select3;
                 }
             }
             return false;
         }

    </script> 
   
     <script language="javascript" type="text/javascript">

         function valid3() {

              if (document.getElementById('<%=txt_Bill_PmtReqNo.ClientID%>').value == "") {
                 alert("Please Enter Proper \" Payment Request No\" field.");
                 document.getElementById("MainContent_txt_Bill_PmtReqNo").focus();
                 return (false);
             }
             if (document.getElementById('<%=txt_Bill_PmtReqDate.ClientID%>').value == "") {
                 alert("Please Enter Proper \" Payment Request Date\" field.");
                 document.getElementById("MainContent_txt_Bill_PmtReqDate").focus();
                 return (false);
             }

            
             return (true);
         }
    </script>

     <script language="javascript" type="text/javascript">

         function valid4() {

             if (document.getElementById('<%=DDL_PmtReqNo.ClientID%>').value == "") {
                 alert("Please select Prper \"Payment Request No\" from Drop-Down.");
                 document.getElementById("MainContent_DDL_PmtReqNo").focus();
                 return (false);
             }

             if (document.getElementById('<%=DDL_Vendors4.ClientID%>').value == "") {
                 alert("Please select Prper \" Vendor\" from Drop-Down.");
                 document.getElementById("MainContent_DDL_Vendors4").focus();
                 return (false);
             }


             if (document.getElementById('<%=txt_Bills_ChequeNo.ClientID%>').value == "") {
                 alert("Please Enter Proper \" Cheque No\" field.");
                 document.getElementById("MainContent_txt_Bills_ChequeNo").focus();
                 return (false);
             }

             if (document.getElementById('<%=txt_Bills_ChequeDate.ClientID%>').value == "") {
                 alert("Please Enter Proper \" Cheque Date\" field.");
                 document.getElementById("MainContent_txt_Bills_ChequeDate").focus();
                 return (false);
             }

             if (document.getElementById('<%=txt_Bills_CheckAmount.ClientID%>').value == "") {
                 alert("Please Enter  Proper \" Cheque Amount \" field.");
                 document.getElementById("MainContent_txt_Bills_CheckAmount").focus();
                 return (false);
             }
            
             return (true);
         }
    </script>


        <table id="ADMT1" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
            <tr>
                <td  bgcolor="#003366" class="style43" rowspan="1" style="color: #FFFFFF">Manage
                    <strong>Bills</strong></td>
            </tr>
            
        </table>      
                   

        
        <div class="style3">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always" >
                    <ContentTemplate>
            <asp:Menu
                ID="Menu1"
                Width="98%"
                runat="server"
                Orientation="Horizontal"
                StaticEnableDefaultPopOutImage="False"
                 OnMenuItemClick="Menu1_MenuItemClick" MaximumDynamicDisplayLevels="4">
                <Items>
                    <asp:MenuItem ImageUrl="~/Images/AddBills_Up.png" Text="" Value="0" Selected="true"></asp:MenuItem>
                    <asp:MenuItem ImageUrl="~/Images/AttachBills_Over.png" Text="" Value="1"></asp:MenuItem>
                    <asp:MenuItem ImageUrl="~/Images/PostBills_Over.png" Text="" Value="2"></asp:MenuItem>
                    <asp:MenuItem ImageUrl="~/Images/PayBills_Over.png" Text="" Value="3"></asp:MenuItem>
                </Items>
        
            </asp:Menu>
             </ContentTemplate>  
        </asp:UpdatePanel>
    </div>
    
 <div class="style4">
        
 <br />
<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always" >
                    <ContentTemplate>

   <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
   <asp:View ID="Tab1" runat="server">
        <table id="Table1" runat="server" cellspacing="2" border="1"  cellpadding="2" class="style35">
            <tr>
                <td class="style56" colspan="5" bgcolor="#336699">
                   <asp:Label ID="Label3" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True" style="font-size: medium">Add Bill Details</asp:Label>
                </td>
            </tr>
            
             <tr>
                <td class="style56" colspan="5" bgcolor="#336699">
                   <asp:Label ID="lbl_Error" runat="server" Font-Size="Small" ForeColor="Yellow" 
                        Font-Bold="True" style="font-size: medium">
                       <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label></asp:Label>
                       <asp:Label ID="lbl_msg" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True" style="font-size: medium">
                       <asp:Label ID="Label15" runat="server" Text="Label"></asp:Label></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style53"> 
                    Budget Year*</td>
                <td class="style55">
                    <asp:DropDownList ID="DDL_BudgYear" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC" ToolTip="Plz Select Value from Drop-Down" 
                        AutoPostBack="True">
                    </asp:DropDownList>
                    <asp:Label ID="Label23" runat="server"></asp:Label>
                </td>
                <td class="style55">
                    Budget Head*&nbsp; <asp:DropDownList ID="DDL_BudgHead" runat="server" 
                        Font-Bold="True" ForeColor="#0066CC" ToolTip="Plz Select Value from Drop-Down">
                    </asp:DropDownList>
                </td>
                <td class="style55">
                    Suppl.Bill?*:<asp:DropDownList ID="DDL_BudgSupp" runat="server" 
                        Font-Bold="True" ForeColor="#0066CC" ToolTip="Plz Select Value from Drop-Down">
                        <asp:ListItem Selected="True">N</asp:ListItem>
                        <asp:ListItem>Y</asp:ListItem>
                    </asp:DropDownList>
                </td>                
                <td class="style55">
                   </td>                
            </tr>
            <tr>
                <td class="style53"> 
                    Bill No*</td>
                <td class="style55">
                    <asp:TextBox ID="txt_Bill_BillNo" runat="server" 
                        AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                        Height="18px" MaxLength="100"  
                        ToolTip="Enter Bill/Invoice No" Width="260px" Wrap="False"></asp:TextBox>
                         <ajaxToolkit:AutoCompleteExtender 
                        ID="txt_Bill_BillNo_AutoCompleteExtender" 
                        runat="server" 
                        MinimumPrefixLength="1"
                        EnableCaching="true" 
                        CompletionSetCount="10"
                        ServiceMethod="SearchAllBills"
                        TargetControlID="txt_Bill_BillNo" 
                        FirstRowSelected = "false">
                    </ajaxToolkit:AutoCompleteExtender>
                </td>
                <td class="style55">
                    Bill Date*&nbsp;
                    <asp:TextBox ID="txt_Bill_BillDate" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="10" ToolTip="Click to Select Date" 
                        Width="71px"  onkeypress="return DateOnly (event)"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="txt_Bill_BillDate_CalendarExtender" 
                        runat="server" Enabled="True" Format="dd/MM/yyyy" 
                        TargetControlID="txt_Bill_BillDate">
                    </ajaxToolkit:CalendarExtender>
                </td>
                <td class="style55" colspan="2">
                    <asp:Label ID="Label8" runat="server" Text="Currency:*"></asp:Label>                    
                    <asp:DropDownList ID="DDL_Currencies" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC" ToolTip="Plz Select Value from Drop-Down">
                    </asp:DropDownList>
                </td>              
            </tr>
             <tr>
                <td class="style53"> 
                    Gross Amount*</td>
                <td class="style55">
                    <asp:TextBox ID="txt_Bill_Amount" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="18px" MaxLength="15" 
                        ToolTip="Enter Total/Gross Amount Billed after rate conversion ..Before Discount/additional charges, etc" 
                        Width="150px" Wrap="False" onkeypress="return isCurrencyKey(event)" ></asp:TextBox>
                 </td>
                <td class="style55">
                    Conversion Rate&nbsp;
                    <asp:TextBox ID="txt_Bill_ConversionRate" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="4" ToolTip="Enter Conversion Rate in Decimal" 
                        Width="71px" onkeypress="return isCurrencyKey(event)"></asp:TextBox>
                         
                </td>

                
                <td class="style55" colspan="2">
                    <asp:Label ID="Label1" runat="server" Text="Amount in Rupees:*"></asp:Label>                    
                    <asp:TextBox ID="txt_Bill_RsAmount" runat="server" 
                        AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                        Height="18px" MaxLength="15" onkeypress="return isCurrencyKey(event)" 
                        ToolTip="Amount in Rupees after conversion." Width="150px" 
                        Wrap="False" Enabled="False"></asp:TextBox>
                </td>              
            </tr>
             <tr>
                <td class="style53"> 
                    Discount %</td>
                <td class="style55">
                    <asp:TextBox ID="txt_Bill_Discount" runat="server" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="4" onkeypress="return isNumberKey(event)" 
                        ToolTip="Enter Discount in %" Width="71px"></asp:TextBox>
                        <ajaxToolkit:AutoCompleteExtender 
                        ID="txt_Bill_Discount_AutoCompleteExtender1" 
                        runat="server" 
                        MinimumPrefixLength="1"
                        EnableCaching="true" 
                        CompletionSetCount="10"
                        ServiceMethod="SearchDis"
                        TargetControlID="txt_Bill_Discount" 
                        FirstRowSelected = "false">
                    </ajaxToolkit:AutoCompleteExtender>
                 </td>
                <td class="style55">
                    Amount After Discount&nbsp;                         
                </td>

                
                <td class="style55" colspan="2">
                                    
                    <asp:TextBox ID="txt_Bill_AmountAfterDiscount" runat="server" 
                        AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                        Height="18px" MaxLength="15" onkeypress="return isCurrencyKey(event)" 
                        ToolTip="Enter Total/Gross Amount Billed after discount." Width="150px" 
                        Wrap="False" Enabled="False"></asp:TextBox>
                </td>              
            </tr>
             <tr>
                <td class="style53"> 
                    Additional Charges</td>
                <td class="style55">
                    <asp:TextBox ID="txt_Bill_OtherCharges" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="18px" MaxLength="15" 
                        ToolTip="Any Other / Additional Charges" 
                        Width="150px" Wrap="False" onkeypress="return isCurrencyKey(event)" ></asp:TextBox>
                 </td>
                <td class="style55">
                    Amount Billed &nbsp;
                    <asp:TextBox ID="txt_Bill_AmountBilled" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="15"  
                        Width="150px" onkeypress="return isCurrencyKey(event)" Enabled="False" ></asp:TextBox>
                </td>
                <td class="style55" colspan="2">
                    Deduction, If Any
                    <asp:TextBox ID="txt_Bill_Deduction" runat="server" 
                        AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                        Height="18px" MaxLength="15" ToolTip="Enter Deduction (Amount), if any" 
                        Width="150px" Wrap="False" onkeypress="return isCurrencyKey(event)" ></asp:TextBox>
                 </td>              
            </tr>
             <tr>
                <td class="style53">Amount Passed</td>
                <td class="style55">
                    <asp:TextBox ID="txt_Bill_AmountPassed" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="15" Width="150px" 
                        onkeypress="return isCurrencyKey(event)" Enabled="False" ></asp:TextBox>
                 </td>
                <td class="style55" colspan="3">Vendor:*
                     <asp:DropDownList ID="DDL_Vendors" runat="server" Font-Bold="True" 
                         ForeColor="#0066CC" ToolTip="Plz Select Value from Drop-Down" Width="450px">
                     </asp:DropDownList>
                    </td>
            </tr>
                 
           
             <tr>
                <td class="style57">Remarks</td>
                <td class="style58" colspan="4">             
                    <asp:TextBox ID="txt_Bill_Remarks" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="18px" MaxLength="250" 
                        ToolTip="Enter Remarks, if any." Width="99%" Wrap="False"></asp:TextBox>
                                     
                 </td>                
            </tr>
        
            <tr>
                <td class="style47" colspan="5">
                    &nbsp; * Mandatory</td>
            </tr>
             <tr>
                <td class="style47" colspan="5">
                    <asp:Button ID="Bill_Calculate_Bttn" runat="server" AccessKey="l" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" Height="23px" 
                         TabIndex="14" Text="Calculate" 
                        Width="74px" Visible="False" ToolTip="Press to Calcualte Amount" OnClientClick="return valid1();"/>
                    <asp:Button ID="Bill_Save_Bttn" runat="server" AccessKey="s" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" Height="23px" 
                        TabIndex="14" Text="Save" 
                        ToolTip="Press to SAVE Record" Visible="False" Width="74px" OnClientClick="return valid1();"/>
                    <asp:Button ID="Bill_Update_Bttn" runat="server" AccessKey="u" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" Height="23px" 
                         TabIndex="14" Text="Update" 
                        Width="74px" Visible="False" ToolTip="Press to UPDATE Record" 
                       OnClientClick="return valid1();"/>
                    <asp:Button ID="Bill_Cancel_Bttn" runat="server" AccessKey="c" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" Height="23px" 
                        TabIndex="14" Text="Cancel" 
                        Width="74px" ToolTip="Press to Cancel" />
                    <asp:Button ID="Bill_DeleteAll_Bttn" runat="server" AccessKey="d" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" Height="23px" 
                        TabIndex="14" Text="Delete Selected Record(s)" ToolTip="Press to Cancel" 
                        Width="180px" />
                 </td>
            </tr>
            <tr>
                <td class="style56" colspan="5" bgcolor="#336699">
                   <asp:Label ID="Label4" runat="server" Font-Size="X-Small" ForeColor="White" 
                        Font-Bold="True" style="font-size:   small">Record(s)</asp:Label>
                </td>
            </tr>

             <tr>  
                <td class="style56" colspan="5" bgcolor="#336699">                
                     
                 


         <asp:GridView ID="Grid1" runat="server" AllowPaging="True" DataKeyNames="BILL_ID"    
                        style="width: 100%;  text-align: center;" allowsorting="True" 
                        AutoGenerateColumns="False"  Font-Bold="True" 
                        Font-Names="Tahoma" Font-Size="8pt" Height="100px"  
                        HorizontalAlign="Center"   ShowFooter="True">
                        <Columns >                
                            <asp:TemplateField HeaderText="S.N.">
                                <ItemTemplate>
                                    <asp:Label ID="lblsr" runat="server" CssClass="MBody"  SkinID="" width="25px"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                                <ItemStyle ForeColor="#336699" Font-Names="Tahoma" Width="25px" 
                                    Font-Bold="True" Font-Size="Small" />
                            </asp:TemplateField>
                    
                   

                            <asp:ButtonField HeaderText="Edit"  Text="Edit" CommandName="Select" 
                                CausesValidation="True">
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" />
                            <ItemStyle ForeColor="#CC0000" Width="20px" Font-Bold="true" />
                            </asp:ButtonField>
                    
                     

                    <asp:BoundField   DataField="INV_NO" HeaderText="Bill No" SortExpression="INV_NO">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" Width="650px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" Font-Names="Tahoma"/>                        
                    </asp:BoundField>
                                      
                    <asp:BoundField   DataField="INV_DATE" SortExpression="INV_DATE" HeaderText="Bill Date" visible="true"  DataFormatString="{0:dd/MM/yyyy}">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="20px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" Font-Names="Arial"/>                        
                    </asp:BoundField>

                     <asp:BoundField   DataField="VEND_NAME" SortExpression="VEND_NAME" HeaderText="Vendor" visible="true">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="20px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" Font-Names="Arial"/>                        
                    </asp:BoundField>

                     <asp:BoundField   DataField="STATUS" SortExpression="STATUS" HeaderText="Status" visible="true">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="20px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" Font-Names="Arial"/>                        
                    </asp:BoundField>
                    <asp:BoundField   DataField="CUR_CODE"  HeaderText="Currency" visible="true">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="20px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="50px" Font-Names="Arial"/>                        
                    </asp:BoundField>
                     <asp:BoundField   DataField="INV_AMOUNT"  HeaderText="Amount Billed" visible="true">                                               
                            <HeaderStyle Font-Bold="True"  Font-Names="Arial" Font-Size="Small" Width="20px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" Font-Names="Arial"/>                        
                    </asp:BoundField>
                     
                    <asp:BoundField   DataField="RS_AMOUNT"  HeaderText="Amount in INR" visible="true">                                               
                            <HeaderStyle Font-Bold="True"  Font-Names="Arial" Font-Size="Small" Width="20px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" Font-Names="Arial"/>                        
                    </asp:BoundField>
                    <asp:BoundField   DataField="DISCOUNT"  HeaderText="Discount%" visible="true">                                               
                            <HeaderStyle Font-Bold="True"  Font-Names="Arial" Font-Size="Small" Width="20px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" Font-Names="Arial"/>                        
                    </asp:BoundField>
                     <asp:BoundField   DataField="AMOUNT_PASSED"  HeaderText="Amount Passed" visible="true">                                               
                            <HeaderStyle Font-Bold="True"  Font-Names="Arial" Font-Size="Small" Width="20px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" Font-Names="Arial"/>                        
                    </asp:BoundField>
                     
                    <asp:TemplateField  ControlStyle-Width="20px"  HeaderText="Delete" FooterText="Select to Delete" ShowHeader="true" ControlStyle-BackColor="Red">
                        <ItemTemplate>
                            <asp:CheckBox ID="cbd"  runat="server" />
                        </ItemTemplate>

                        <HeaderTemplate>
                            <asp:ImageButton ID="ImageButton1" runat="server" Height="16px" Width="16px" ToolTip="Select All" ImageUrl="~/Images/check_all.gif" onClientclick="return Select(true)"  />
                            <asp:ImageButton ID="ImageButton2" runat="server" Height="16px" Width="16px" ToolTip="Deselect All" ImageUrl="~/Images/uncheck_all.gif"  OnClientClick ="return Select(false)" />
                        </HeaderTemplate>

                        <ControlStyle Width="50px" ForeColor="White"></ControlStyle>
                        <ItemStyle ForeColor="White" />
                         <FooterStyle  ForeColor="White" />
                    </asp:TemplateField>
                    
                    
                </Columns>
                    
                    <PagerStyle BackColor="#3399FF" BorderColor="#C0C000" BorderStyle="Solid" 
                        Font-Bold="True" ForeColor="White" HorizontalAlign="Center" 
                        VerticalAlign="Middle" Font-Size="Small" />
                    <RowStyle BackColor="#DADADA" BorderColor="Desktop" ForeColor="#3399FF" />
                    <SelectedRowStyle BackColor="Desktop" BorderColor="SteelBlue" BorderStyle="Solid" />
                    <HeaderStyle BackColor="Desktop" Font-Bold="True" Font-Italic="False" Font-Names="Times new roman"
                        Font-Overline="False" Font-Underline="False" ForeColor="White" Width="80%" />
                    <PagerSettings Position="TopAndBottom" FirstPageText="First" LastPageText="Last" PageButtonCount="10" Mode="NumericFirstLast" />
                    <AlternatingRowStyle BackColor="#EFEFEF" Font-Names="Tahoma" ForeColor="#0066FF" />
                </asp:GridView>
                   
                                   
                      
                 </td>
            </tr>
        </table>

                
        
  
        
     </asp:View>
    <asp:View ID="Tab2" runat="server">
        <table id="Table2" runat="server" cellspacing="2" border="1"  cellpadding="2" class="style35">
            <tr>
                <td class="style56" colspan="5" bgcolor="#336699">
                   <asp:Label ID="Label5" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True" style="font-size: medium">Attach Bills with Purchasing Records/Titles</asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style56" colspan="5" bgcolor="#336699">
                   <asp:Label ID="Label7" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">Select Vendor/Bill No/Order from Drop-Downs and Process it</asp:Label>
                    <asp:Label ID="Label20" runat="server" Font-Bold="True" Font-Size="Medium" 
                        ForeColor="Yellow"></asp:Label>
                </td>
            </tr>
           
              <tr>
                <td class="style53"> 
                    Select Vendor*</td>
                <td class="style55" colspan="2">
                    <asp:DropDownList ID="DDL_Vendors2" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC" ToolTip="Plz Select Value from Drop-Down" 
                        Width="400px" AutoPostBack="True">
                    </asp:DropDownList>
                </td>               
                <td class="style55" colspan="2">
                    Select Bill*
                    <asp:DropDownList ID="DDL_Bills" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC" ToolTip="Plz Select Value from Drop-Down">
                    </asp:DropDownList>
                :</td>               
            </tr> 
            
            <tr>
                <td class="style53"> 
                    Select Order*</td>
                <td class="style55" colspan="2">
                    <asp:DropDownList ID="DDL_Orders" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC" ToolTip="Plz Select Value from Drop-Down" 
                        Width="400px" AutoPostBack="True">
                    </asp:DropDownList>
                    <asp:Button ID="Bill_Dettach_Bttn" runat="server" AccessKey="b" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" Height="23px" 
                        TabIndex="14" Text="Dettach Selected Records" ToolTip="Press to Process" 
                        Width="180px" />
                </td>               
                <td class="style55" colspan="2">
                    <asp:Button ID="Bill_Attach_Bttn" runat="server" AccessKey="a" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" Height="23px" 
                        TabIndex="14" Text="Attach Selected Record" ToolTip="Press to Process" 
                        Width="180px" />
                </td>               
            </tr>        
           
        
            <tr>
                <td class="style56" colspan="5" bgcolor="#336699">
                   <asp:Label ID="Label9" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">Display Record(s)</asp:Label>
                </td>
            </tr>
             <tr>
                <td class="style56" colspan="5" bgcolor="#336699">
                   <asp:Label ID="Label22" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">HELP: Select Acquisition Record(s) To Process with the Bill Selected!</asp:Label>
                </td>
            </tr>
            <tr>  
                <td class="style56" colspan="5" bgcolor="#336699">                    


         <asp:GridView ID="Grid2" runat="server" AllowPaging="True" DataKeyNames="ACQ_ID"    
                        style="width: 100%;  text-align: center;" allowsorting="True" 
                        AutoGenerateColumns="False"  Font-Bold="True" 
                        Font-Names="Tahoma" Font-Size="8pt" Height="100px"  
                        HorizontalAlign="Center"   ShowFooter="True">
                        <Columns >                
                            <asp:TemplateField HeaderText="S.N.">
                                <ItemTemplate>
                                    <asp:Label ID="lblsr" runat="server" CssClass="MBody"  SkinID="" width="25px"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                                <ItemStyle ForeColor="#336699" Font-Names="Tahoma" Width="25px" 
                                    Font-Bold="True" Font-Size="Small" />
                            </asp:TemplateField>
                       
                     <asp:BoundField   DataField="TITLE"  HeaderText="Title" SortExpression="TITLE" visible="true">                                               
                            <HeaderStyle Font-Bold="True"  Font-Names="Arial" Font-Size="Small" Width="950px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="950px" Font-Names="Arial"/>                        
                    </asp:BoundField> 

                    <asp:BoundField   DataField="VOL_NO" SortExpression="VOL_NO" HeaderText="Vol No" visible="true">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="100px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="100px" Font-Names="Arial"/>                        
                    </asp:BoundField>
                     <asp:BoundField   DataField="PROCESS_STATUS" SortExpression="PROCESS_STATUS" HeaderText="Status" visible="true">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="160px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="160px" Font-Names="Arial"/>                        
                    </asp:BoundField>
                     <asp:BoundField   DataField="CUR_CODE"  HeaderText="Currency" visible="true">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="160px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="160px" Font-Names="Arial"/>                        
                    </asp:BoundField>
                    
                     <asp:BoundField   DataField="ITEM_PRICE"  HeaderText="Item Cost" visible="true">                                               
                            <HeaderStyle Font-Bold="True"  Font-Names="Arial" Font-Size="Small" Width="150px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" Font-Names="Arial"/>                        
                    </asp:BoundField>

                      <asp:TemplateField HeaderText="Coversion Rate">
                        <ItemTemplate>                       
                            <asp:TextBox ID="txt_Bill_ConversionRate2"  runat ="server"  Enabled="false"   ForeColor="Red" MaxLength="10" Font-Bold="true"  Text='<%#  Eval("CONVERSION_RATE") %>' Width="45px"  visible="true" onkeypress="return isCurrencyKey(event)"></asp:TextBox>
                        </ItemTemplate>
                        <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                        <ItemStyle ForeColor="#336699" Font-Names="Tahoma" Width="150px" 
                            Font-Bold="True" Font-Size="Small" BackColor="Red"/>
                     </asp:TemplateField>
                    
                       <asp:TemplateField HeaderText="Other Charges">
                        <ItemTemplate>                       
                            <asp:TextBox ID="txt_Bill_OtherCharges2"  runat ="server"  Enabled="false"   ForeColor="Red" MaxLength="10" Font-Bold="true"  Text='<%#  Eval("OTHER_CHARGES") %>' Width="45px"  visible="true" onkeypress="return isCurrencyKey(event)"></asp:TextBox>
                        </ItemTemplate>
                        <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                        <ItemStyle ForeColor="#336699" Font-Names="Tahoma" Width="150px" 
                            Font-Bold="True" Font-Size="Small" BackColor="Red"/>
                     </asp:TemplateField>

                       <asp:BoundField   DataField="ITEM_RUPEES"  HeaderText="Item Cost (Rs)" visible="true">                                               
                            <HeaderStyle Font-Bold="True"  Font-Names="Arial" Font-Size="Small" Width="150px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" Font-Names="Arial"/>                        
                    </asp:BoundField>
                   
                    <asp:TemplateField  ControlStyle-Width="20px"  HeaderText="Delete" FooterText="Select to Process" ShowHeader="true" ControlStyle-BackColor="Red">
                        <ItemTemplate>
                            <asp:CheckBox ID="cbd"  runat="server"  onclick="GetCheckStatus()"/>
                        </ItemTemplate>

                        <HeaderTemplate>
                            <asp:ImageButton ID="ImageButton1" runat="server" Height="16px" Width="16px" ToolTip="Select All" ImageUrl="~/Images/check_all.gif" onClientclick="return Select2(true)"  />
                            <asp:ImageButton ID="ImageButton2" runat="server" Height="16px" Width="16px" ToolTip="Deselect All" ImageUrl="~/Images/uncheck_all.gif"  OnClientClick ="return Select2(false)" />
                        </HeaderTemplate>

                        <ControlStyle Width="50px" ForeColor="White"></ControlStyle>
                        <ItemStyle ForeColor="White" />
                         <FooterStyle  ForeColor="White" />
                    </asp:TemplateField>
                    
                </Columns>
                    
                    <PagerStyle BackColor="#3399FF" BorderColor="#C0C000" BorderStyle="Solid" 
                        Font-Bold="True" ForeColor="White" HorizontalAlign="Center" 
                        VerticalAlign="Middle" Font-Size="Small" />
                    <RowStyle BackColor="#DADADA" BorderColor="Desktop" ForeColor="#3399FF" />
                    <SelectedRowStyle BackColor="Desktop" BorderColor="SteelBlue" BorderStyle="Solid" />
                    <HeaderStyle BackColor="Desktop" Font-Bold="True" Font-Italic="False" Font-Names="Times new roman"
                        Font-Overline="False" Font-Underline="False" ForeColor="White" Width="90%" />
                    <PagerSettings Position="TopAndBottom" FirstPageText="First" LastPageText="Last" PageButtonCount="10" Mode="NumericFirstLast" />
                    <AlternatingRowStyle BackColor="#EFEFEF" Font-Names="Tahoma" ForeColor="#0066FF" />
                </asp:GridView>
                   

                  
                      
                   </td>
            </tr>
            
                        

        </table>

                
        


    </asp:View>

    <asp:View ID="Tab3" runat="server">
        <table id="Table3" runat="server" cellspacing="2" border="1"  cellpadding="2" class="style35">
            <tr>
                <td class="style56" colspan="4" bgcolor="#336699">
                   <asp:Label ID="Label10" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True" style="font-size: medium">Post Bills to Accounts</asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style56" colspan="4" bgcolor="#336699">
                   <asp:Label ID="Label11" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">Enter Unique Payment Request No under which many bills can be clubbed and posted</asp:Label>
                    <asp:Label ID="Label12" runat="server" Font-Bold="True" Font-Size="Medium" 
                        ForeColor="Yellow"></asp:Label>
                </td>
            </tr>
            <tr style=" font-weight:bold">
                <td class="style53"> 
                    Payment Request No*</td>
                <td class="style55">
                      <asp:TextBox ID="txt_Bill_PmtReqNo" runat="server" AutoPostBack="True" 
                        Font-Bold="True" MaxLength="50" onkeypress="return isNumberKey(event)"></asp:TextBox>
                         <ajaxToolkit:AutoCompleteExtender 
                        ID="txt_Bill_PmtReqNo_AutoCompleteExtender" 
                        runat="server" 
                        MinimumPrefixLength="1"
                        EnableCaching="true" 
                        CompletionSetCount="10"
                        ServiceMethod="SearchAllPMR"
                        TargetControlID="txt_Bill_PmtReqNo" 
                        FirstRowSelected = "false">
                    </ajaxToolkit:AutoCompleteExtender>
                </td>
                <td class="style55">
                    Payment Request Date:*&nbsp;
                    <asp:TextBox ID="txt_Bill_PmtReqDate" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="10" 
                        Width="71px" onkeypress="return DateOnly2 (event)"></asp:TextBox>
                         <ajaxToolkit:CalendarExtender ID="txt_Bill_PmtReqDate_CalendarExtender" 
                        runat="server" Enabled="True" Format="dd/MM/yyyy" 
                        TargetControlID="txt_Bill_PmtReqDate">
                    </ajaxToolkit:CalendarExtender>
                   
                 </td>
                <td class="style55">
                    <asp:Button ID="Bill_Post_Bttn" runat="server" CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red" TabIndex="14" Text="Post Selected Records" 
                        Width="160px" ToolTip="Press to Post the Bills Selected" AccessKey="p" 
                        Visible="False" OnClientClick="return valid3();" />
                    <asp:Button ID="Bill_UnPost_Bttn" runat="server" AccessKey="p" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" TabIndex="14" 
                        Text="Un-Post Selected Records" ToolTip="Press to Post the Bills Selected" 
                        Visible="False" Width="180px" />
                 </td>
                
            </tr>
              <tr style=" color:Green; font-weight:bold">
                <td class="style53"> 
                    Letter Template</td>
                <td class="style54" colspan="3">
                   
                    &nbsp;</td>               
            </tr>       
           
        
            <tr>
                <td class="style56" colspan="4" bgcolor="#336699">
                   <asp:Label ID="Label13" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">Record(s)</asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style56" colspan="4" bgcolor="#336699">
                   <asp:Label ID="Label14" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">HELP: Select Record to Post to Accounts Section for Payment</asp:Label>
                </td>
            </tr>
            
            
            

             <tr>  
                <td class="style56" colspan="4" bgcolor="#336699">                
                     
                 <asp:GridView ID="Grid3" runat="server" AllowPaging="True" DataKeyNames="BILL_ID"    
                        style="width: 100%;  text-align: center;" allowsorting="True" 
                        AutoGenerateColumns="False"  Font-Bold="True" 
                        Font-Names="Tahoma" Font-Size="8pt" Height="100px"  
                        HorizontalAlign="Center"   ShowFooter="True">
                        <Columns >                
                            <asp:TemplateField HeaderText="S.N.">
                                <ItemTemplate>
                                    <asp:Label ID="lblsr" runat="server" CssClass="MBody"  SkinID="" width="25px"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                                <ItemStyle ForeColor="#336699" Font-Names="Tahoma" Width="25px" 
                                    Font-Bold="True" Font-Size="Small" />
                            </asp:TemplateField>
                        
                    <asp:BoundField   DataField="INV_NO" HeaderText="Bill No" SortExpression="INV_NO">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" Width="650px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" Font-Names="Tahoma"/>                        
                    </asp:BoundField>
                                      
                    <asp:BoundField   DataField="INV_DATE" SortExpression="INV_DATE" HeaderText="Bill Date" visible="true"  DataFormatString="{0:dd/MM/yyyy}">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="20px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" Font-Names="Arial"/>                        
                    </asp:BoundField>

                     <asp:BoundField   DataField="VEND_NAME" SortExpression="VEND_NAME" HeaderText="Vendor" visible="true">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="20px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" Font-Names="Arial"/>                        
                    </asp:BoundField>

                     <asp:BoundField   DataField="STATUS" SortExpression="STATUS" HeaderText="Status" visible="true">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="20px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" Font-Names="Arial"/>                        
                    </asp:BoundField>
                    <asp:BoundField   DataField="CUR_CODE"  HeaderText="Currency" visible="true">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="20px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="50px" Font-Names="Arial"/>                        
                    </asp:BoundField>
                     <asp:BoundField   DataField="INV_AMOUNT"  HeaderText="Amount Billed" visible="true">                                               
                            <HeaderStyle Font-Bold="True"  Font-Names="Arial" Font-Size="Small" Width="20px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" Font-Names="Arial"/>                        
                    </asp:BoundField>
                     
                    <asp:BoundField   DataField="RS_AMOUNT"  HeaderText="Amount in INR" visible="true">                                               
                            <HeaderStyle Font-Bold="True"  Font-Names="Arial" Font-Size="Small" Width="20px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" Font-Names="Arial"/>                        
                    </asp:BoundField>

                     <asp:BoundField   DataField="PMT_REQ_NO"  HeaderText="Pmt Req No" visible="true">                                               
                            <HeaderStyle Font-Bold="True"  Font-Names="Arial" Font-Size="Small" Width="20px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" Font-Names="Arial"/>                        
                    </asp:BoundField>
                   
                     <asp:BoundField   DataField="AMOUNT_PASSED"  HeaderText="Amount Passed" visible="true">                                               
                            <HeaderStyle Font-Bold="True"  Font-Names="Arial" Font-Size="Small" Width="20px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" Font-Names="Arial"/>                        
                    </asp:BoundField>
                     
                    <asp:TemplateField  ControlStyle-Width="20px"  HeaderText="Delete" FooterText="Select to Post/un-Post" ShowHeader="true" ControlStyle-BackColor="Red">
                        <ItemTemplate>
                            <asp:CheckBox ID="cbd"  runat="server" />
                        </ItemTemplate>

                        <HeaderTemplate>
                            <asp:ImageButton ID="ImageButton1" runat="server" Height="16px" Width="16px" ToolTip="Select All" ImageUrl="~/Images/check_all.gif" onClientclick="return Select3(true)"  />
                            <asp:ImageButton ID="ImageButton2" runat="server" Height="16px" Width="16px" ToolTip="Deselect All" ImageUrl="~/Images/uncheck_all.gif"  OnClientClick ="return Select3(false)" />
                        </HeaderTemplate>

                        <ControlStyle Width="50px" ForeColor="White"></ControlStyle>
                        <ItemStyle ForeColor="White" />
                         <FooterStyle  ForeColor="White" />
                    </asp:TemplateField>
                                        
                </Columns>
                    
                    <PagerStyle BackColor="#3399FF" BorderColor="#C0C000" BorderStyle="Solid" 
                        Font-Bold="True" ForeColor="White" HorizontalAlign="Center" 
                        VerticalAlign="Middle" Font-Size="Small" />
                    <RowStyle BackColor="#DADADA" BorderColor="Desktop" ForeColor="#3399FF" />
                    <SelectedRowStyle BackColor="Desktop" BorderColor="SteelBlue" BorderStyle="Solid" />
                    <HeaderStyle BackColor="Desktop" Font-Bold="True" Font-Italic="False" Font-Names="Times new roman"
                        Font-Overline="False" Font-Underline="False" ForeColor="White" Width="80%" />
                    <PagerSettings Position="TopAndBottom" FirstPageText="First" LastPageText="Last" PageButtonCount="100" Mode="NumericFirstLast" />
                    <AlternatingRowStyle BackColor="#EFEFEF" Font-Names="Tahoma" ForeColor="#0066FF" />
                </asp:GridView>


                  
                      
                   </td>
            </tr>
            
                        

        </table>
                      
    </asp:View>

  




    <asp:View ID="Tab4" runat="server">
        <table id="Table4" runat="server" cellspacing="2" border="1"  cellpadding="2" class="style35">
            <tr>
                <td class="style56" colspan="4" bgcolor="#336699">
                   <asp:Label ID="Label6" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True" style="font-size: medium">Update Payment Details</asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style56" colspan="4" bgcolor="#336699">
                   <asp:Label ID="Label16" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">Select Payment Request No from Drop-Down and then Select Vendor To Display Bills Record</asp:Label>
                    <asp:Label ID="Label17" runat="server" Font-Bold="True" Font-Size="Medium" 
                        ForeColor="Yellow"></asp:Label>
                </td>
            </tr>
            <tr style=" font-weight:bold">
                <td class="style53"> 
                    Payment Request No*</td>
                <td class="style55">
                      <asp:DropDownList ID="DDL_PmtReqNo" runat="server" AutoPostBack="True" 
                          Font-Bold="True" ForeColor="#0066CC" ToolTip="Plz Select Value from Drop-Down" 
                          Width="150px">
                      </asp:DropDownList>
                         <ajaxToolkit:AutoCompleteExtender 
                        ID="AutoCompleteExtender1" 
                        runat="server" 
                        MinimumPrefixLength="1"
                        EnableCaching="true" 
                        CompletionSetCount="10"
                        ServiceMethod="SearchAllPMR"
                        TargetControlID="txt_Bill_PmtReqNo" 
                        FirstRowSelected = "false">
                    </ajaxToolkit:AutoCompleteExtender>
                </td>
                <td class="style55">
                    &nbsp;<ajaxToolkit:CalendarExtender ID="CalendarExtender1" 
                        runat="server" Enabled="True" Format="dd/MM/yyyy" 
                        TargetControlID="txt_Bill_PmtReqDate">
                    </ajaxToolkit:CalendarExtender>
                   
                 </td>
                <td class="style55">
                    <asp:Button ID="Bill_UpdatePmt_Bttn" runat="server" CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red" TabIndex="14" Text="Update Payment Details" 
                        Width="170px" ToolTip="Press to Update Payment Details" 
                        Visible="False" OnClientClick="return valid4();" />
                    <asp:Button ID="Bill_DeletePmt_Bttn" runat="server" AccessKey="d" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" TabIndex="14" 
                        Text="Delete Payment Details" ToolTip="Press to Delete Payment Details" 
                        Visible="False" Width="180px" />
                 </td>
                
            </tr>
              <tr>
                <td class="style53" style=" font-weight:bold"> 
                    Select Vendor</td>
                <td class="style54" colspan="3">
                   
                    <asp:DropDownList ID="DDL_Vendors4" runat="server" AutoPostBack="True" 
                        Font-Bold="True" ForeColor="#0066CC" ToolTip="Plz Select Value from Drop-Down" 
                        Width="400px">
                    </asp:DropDownList>
                  </td>               
            </tr>   
            
            <tr id="TR_CHKNO" runat="server">
                <td class="style53" style=" font-weight:bold"> 
                    Cheque No*</td>
                <td class="style55">
                    <asp:TextBox ID="txt_Bills_ChequeNo" runat="server" 
                        AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                        Height="18px" MaxLength="50"  
                        ToolTip="Enter cheque  No" Width="260px" Wrap="False"></asp:TextBox>
                        
                </td>
                <td class="style55">
                    <strong>Cheque Date*</strong>&nbsp;
                    
                </td>
                <td class="style55" colspan="2">
                    <asp:TextBox ID="txt_Bills_ChequeDate" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="10" 
                        onkeypress="return DateOnly3 (event)" ToolTip="Click to Select Date" 
                        Width="71px"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" 
                        runat="server" Enabled="True" Format="dd/MM/yyyy" 
                        TargetControlID="txt_Bills_ChequeDate">
                    </ajaxToolkit:CalendarExtender>
                </td>              
            </tr>    
           
        
         <tr id="TR_BANK" runat="server">
                <td class="style53" style=" font-weight:bold"> 
                    Bank Name*</td>
                <td class="style55">
                    <asp:TextBox ID="txt_Bills_BankName" runat="server" 
                        AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                        Height="18px" MaxLength="250"  
                        ToolTip="Enter cheque  No" Width="260px" Wrap="False"></asp:TextBox>
                         
                </td>
                <td class="style55">
                    <strong>Cheque Amount*</strong>&nbsp;
                    
                </td>
                <td class="style55" colspan="2">
                    <asp:TextBox ID="txt_Bills_CheckAmount" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="15" 
                         onkeypress="return isCurrencyKey(event)"   ToolTip="Enter Cheque Amount" 
                        Width="71px"></asp:TextBox>
                        
                </td>              
            </tr>   

             <tr>
                <td class="style56" colspan="4" bgcolor="#336699">
                   <asp:Label ID="Label18" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">Record(s)</asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style56" colspan="4" bgcolor="#336699">
                   <asp:Label ID="Label19" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">HELP: Select Record to Post to Accounts Section for Payment</asp:Label>
                </td>
            </tr>
            
            
            

             <tr>  
                <td class="style56" colspan="4" bgcolor="#336699">                
                     
                 <asp:GridView ID="Grid4" runat="server" AllowPaging="True" DataKeyNames="BILL_ID"    
                        style="width: 100%;  text-align: center;" allowsorting="True" 
                        AutoGenerateColumns="False"  Font-Bold="True" 
                        Font-Names="Tahoma" Font-Size="8pt" Height="100px"  
                        HorizontalAlign="Center"   ShowFooter="True">
                        <Columns >                
                            <asp:TemplateField HeaderText="S.N.">
                                <ItemTemplate>
                                    <asp:Label ID="lblsr" runat="server" CssClass="MBody"  SkinID="" width="25px"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                                <ItemStyle ForeColor="#336699" Font-Names="Tahoma" Width="25px" 
                                    Font-Bold="True" Font-Size="Small" />
                            </asp:TemplateField>
                        
                    <asp:BoundField   DataField="INV_NO" HeaderText="Bill No" SortExpression="INV_NO">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" Width="650px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" Font-Names="Tahoma"/>                        
                    </asp:BoundField>
                                      
                    <asp:BoundField   DataField="INV_DATE" SortExpression="INV_DATE" HeaderText="Bill Date" visible="true"  DataFormatString="{0:dd/MM/yyyy}">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="20px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" Font-Names="Arial"/>                        
                    </asp:BoundField>

                     <asp:BoundField   DataField="VEND_NAME" SortExpression="VEND_NAME" HeaderText="Vendor" visible="true">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="20px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" Font-Names="Arial"/>                        
                    </asp:BoundField>

                     <asp:BoundField   DataField="STATUS" SortExpression="STATUS" HeaderText="Status" visible="true">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="20px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" Font-Names="Arial"/>                        
                    </asp:BoundField>
                    <asp:BoundField   DataField="CUR_CODE"  HeaderText="Currency" visible="true">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="20px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="50px" Font-Names="Arial"/>                        
                    </asp:BoundField>
                     <asp:BoundField   DataField="INV_AMOUNT"  HeaderText="Amount Billed" visible="true">                                               
                            <HeaderStyle Font-Bold="True"  Font-Names="Arial" Font-Size="Small" Width="20px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" Font-Names="Arial"/>                        
                    </asp:BoundField>
                     
                    <asp:BoundField   DataField="PMT_REQ_NO"  HeaderText="Pmt Req No" visible="true">                                               
                            <HeaderStyle Font-Bold="True"  Font-Names="Arial" Font-Size="Small" Width="20px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" Font-Names="Arial"/>                        
                    </asp:BoundField>
                   
                     <asp:BoundField   DataField="AMOUNT_PASSED"  HeaderText="Amount Passed" visible="true">                                               
                            <HeaderStyle Font-Bold="True"  Font-Names="Arial" Font-Size="Small" Width="20px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" Font-Names="Arial"/>                        
                    </asp:BoundField>

                      <asp:BoundField   DataField="CHEQUE_NO"  HeaderText="Cheque No" visible="true">                                               
                            <HeaderStyle Font-Bold="True"  Font-Names="Arial" Font-Size="Small" Width="20px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" Font-Names="Arial"/>                        
                    </asp:BoundField>
                       <asp:BoundField   DataField="CHEQUE_AMOUNT"  HeaderText="Cheque Amount" visible="true">                                               
                            <HeaderStyle Font-Bold="True"  Font-Names="Arial" Font-Size="Small" Width="20px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" Font-Names="Arial"/>                        
                    </asp:BoundField>
                     
                    
                                        
                </Columns>
                    
                    <PagerStyle BackColor="#3399FF" BorderColor="#C0C000" BorderStyle="Solid" 
                        Font-Bold="True" ForeColor="White" HorizontalAlign="Center" 
                        VerticalAlign="Middle" Font-Size="Small" />
                    <RowStyle BackColor="#DADADA" BorderColor="Desktop" ForeColor="#3399FF" />
                    <SelectedRowStyle BackColor="Desktop" BorderColor="SteelBlue" BorderStyle="Solid" />
                    <HeaderStyle BackColor="Desktop" Font-Bold="True" Font-Italic="False" Font-Names="Times new roman"
                        Font-Overline="False" Font-Underline="False" ForeColor="White" Width="80%" />
                    <PagerSettings Position="TopAndBottom" FirstPageText="First" LastPageText="Last" PageButtonCount="100" Mode="NumericFirstLast" />
                    <AlternatingRowStyle BackColor="#EFEFEF" Font-Names="Tahoma" ForeColor="#0066FF" />
                </asp:GridView>


                  
                      
                   </td>
            </tr>
            
                        

        </table>
                      
    </asp:View>
</asp:MultiView>





  
 
 
             </ContentTemplate>  
                   <Triggers>
                        <asp:AsyncPostBackTrigger  ControlID="Menu1" EventName="MenuItemClick"   /> 
                        <asp:AsyncPostBackTrigger  ControlID="Bill_Save_Bttn" EventName="Click"   />   
                        <asp:AsyncPostBackTrigger  ControlID="Grid1" EventName="RowCommand"   />                                   
                   </Triggers>                
               </asp:UpdatePanel>          
                           
</div>

     
        
                   

</asp:Content>
