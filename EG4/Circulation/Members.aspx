<%@ Page  Title="Membership Manager" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Members.aspx.vb" Inherits="EG4.Members" %>


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
            width: 18%;
            padding: 0px;
            background-color: #99CCFF;
            font-size: small;
            height: 18px;
            }
        
       
        .style54
        {
            text-align:left;
            border-style: none;
            padding: 0px;
            font-weight: 700;
            background-color: #D5EAFF;
            margin-left: 120px;
            font-size: small;
        }
        .style55
        {
            text-align:left;
            border-style: none;
            border-color: inherit;
            padding: 0px;
             background-color:#D5EAFF;  
            font-size: small;
            height: 18px;
            font-weight: 700;
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
      
      
        .style58
        {
            text-align: center;
            border-style: none;
            border-color: inherit;
            width: 10%;
            padding: 0px;
            background-color: #D5EAFF;
            font-size: small;
            height: 14px;
        }
                
        .style59
        {
            text-decoration: underline;
        }
                
        .style60
        {
            font-weight: normal;
        }
                
        </style>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript" src="http://www.google.com/jsapi"></script><script language ="javascript" type ="text/javascript" >
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

             var Inputs = grdv.getElementsByTagName("input");

             for (var n = 0; n < Inputs.length; ++n) {
                 if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(chbk, 0) >= 0) {
                     Inputs[n].checked = Select2;
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

  

        function valid1() {

            if (document.getElementById('<%=txt_Cir_Category.ClientID%>').value == "") {
                alert("Please enter proper \"Category Name \" field.");
                document.getElementById("MainContent_txt_Cir_Category").focus();
                return (false);
            }

            return (true);
        }


    </script>
    <script language="javascript" type="text/javascript">

         function valid2() {

             if (document.getElementById('<%=txt_Cir_SubCatName.ClientID%>').value == "") {
                 alert("Please enter proper \"Sub Category Name \" field.");
                 document.getElementById("MainContent_txt_Cir_SubCatName").focus();
                 return (false);
             }

             if (document.getElementById('<%=DDL_FineSystem.ClientID%>').value == "") {
                alert("Please Select \" Fine system\" field.");
                document.getElementById("MainContent_DDL_FineSystem").focus();
                return (false);
            }

             return (true);
         }


    </script>

     <script language="javascript" type="text/javascript">

         function valid3() {

             if (document.getElementById('<%=txt_Mem_MemNo.ClientID%>').value == "") {
                 alert("Please enter  \"Member No \" field.");
                 document.getElementById("MainContent_txt_Mem_MemNo").focus();
                 return (false);
             }

             if (document.getElementById('<%=txt_Mem_MemName.ClientID%>').value == "") {
                 alert("Please enter  \"Member Name \" field.");
                 document.getElementById("MainContent_txt_Mem_MemName").focus();
                 return (false);
             }

             if (document.getElementById('<%=DDL_Gender.ClientID%>').value == "") {
                 alert("Please Select \" Gender\" field.");
                 document.getElementById("MainContent_DDL_Gender").focus();
                 return (false);
             }

             if (document.getElementById('<%=DDL_Categories.ClientID%>').value == "") {
                 alert("Please Select \" Member Category\" field.");
                 document.getElementById("MainContent_DDL_Categories").focus();
                 return (false);
             }

             if (document.getElementById('<%=DDL_SubCategories.ClientID%>').value == "") {
                 alert("Please Select \" Member Sub Category\" field.");
                 document.getElementById("MainContent_DDL_SubCategories").focus();
                 return (false);
             }

             if (document.getElementById('<%=txt_Mem_AdmissionDate.ClientID%>').value == "") {
                 alert("Please Select \" Admission Date\" field.");
                 document.getElementById("MainContent_txt_Mem_AdmissionDate").focus();
                 return (false);
             }

             if (document.getElementById('<%=txt_Mem_ClosingDate.ClientID%>').value == "") {
                 alert("Please Select \" Closing Date \" field.");
                 document.getElementById("MainContent_txt_Mem_ClosingDate").focus();
                 return (false);
             }

             



             return (true);
         }
         </script>

     <script language ="javascript" src ="../md5.js" type ="text/javascript" ></script> 
     <script language="javascript" type="text/javascript">

         var MyHashed;

         function test1() {
             var pwd = "";
             var repwd = "";
            
             pwd = document.getElementById('<%=txt_UserPass.ClientID%>').value;
             repwd = document.getElementById('<%=txt_UserRePass.ClientID%>').value;


             if (document.getElementById('<%=txt_Mem_MemNo.ClientID%>').value == "") {
                 alert("Please enter  \"Member No \" field.");
                 document.getElementById("MainContent_txt_Mem_MemNo").focus();
                 return (false);
             }

             if (document.getElementById('<%=txt_Mem_MemName.ClientID%>').value == "") {
                 alert("Please enter  \"Member Name \" field.");
                 document.getElementById("MainContent_txt_Mem_MemName").focus();
                 return (false);
             }

             if (document.getElementById('<%=DDL_Gender.ClientID%>').value == "") {
                 alert("Please Select \" Gender\" field.");
                 document.getElementById("MainContent_DDL_Gender").focus();
                 return (false);
             }

             if (document.getElementById('<%=DDL_Categories.ClientID%>').value == "") {
                 alert("Please Select \" Member Category\" field.");
                 document.getElementById("MainContent_DDL_Categories").focus();
                 return (false);
             }

             if (document.getElementById('<%=DDL_SubCategories.ClientID%>').value == "") {
                 alert("Please Select \" Member Sub Category\" field.");
                 document.getElementById("MainContent_DDL_SubCategories").focus();
                 return (false);
             }

             if (document.getElementById('<%=txt_Mem_AdmissionDate.ClientID%>').value == "") {
                 alert("Please Select \" Admission Date\" field.");
                 document.getElementById("MainContent_txt_Mem_AdmissionDate").focus();
                 return (false);
             }

             if (document.getElementById('<%=txt_Mem_ClosingDate.ClientID%>').value == "") {
                 alert("Please Select \" Closing Date \" field.");
                 document.getElementById("MainContent_txt_Mem_ClosingDate").focus();
                 return (false);
             }



             if (document.getElementById("MainContent_CheckBox2").checked == true) {


                 if (pwd == "") {
                     alert("Please enter proper \"Password\" field.");
                     document.getElementById("MainContent_txt_UserPass").focus();
                     return (false);
                 }
                 if (document.getElementById('<%=txt_UserPass.ClientID%>').value.length < 5) {
                     alert("Length of \"Password\" should be Min 5 characters.");
                     document.getElementById("MainContent_txt_UserPass").focus();
                     return (false);
                 }
                 if (document.getElementById('<%=txt_UserPass.ClientID%>').value.length > 10) {
                     alert("Length of \"Password\" should be Max 10 characters.");
                     document.getElementById("MainContent_txt_UserPass").focus();
                     return (false);
                 }

                 re = /[0-9]/;
                 if (!re.test(document.getElementById('<%=txt_UserPass.ClientID%>').value)) {
                     alert("Error: password must contain at least one number (0-9)!");
                     document.getElementById("MainContent_txt_UserPass").focus();
                     return false;
                 }
                 re = /[a-z]/;
                 if (!re.test(document.getElementById('<%=txt_UserPass.ClientID%>').value)) {
                     alert("Error: password must contain at least one lowercase letter (a-z)!");
                     document.getElementById("MainContent_txt_UserPass").focus();
                     return false;
                 }
                 re = /[A-Z]/;
                 if (!re.test(document.getElementById('<%=txt_UserPass.ClientID%>').value)) {
                     alert("Error: password must contain at least one uppercase letter (A-Z)!");
                     document.getElementById("MainContent_txt_UserPass").focus();
                     return false;
                 }

                 re = /[\!\@\#\$\%\^\&\*\-]/;
                 if (!re.test(document.getElementById('<%=txt_UserPass.ClientID%>').value)) {
                     alert("Error: password must contain at least one One special Character (!@#$%^&*-)!");
                     document.getElementById("MainContent_txt_UserPass").focus();
                     return false;
                 }

                 if (repwd == "") {
                     alert("Please enter proper \"Re-Password\" field.");
                     document.getElementById("MainContent_txt_UserRePass").focus();
                     return (false);
                 }
                 if (document.getElementById('<%=txt_UserRePass.ClientID%>').value.length < 5) {
                     alert("Length of \"Re-Password\" should be Min 5 characters.");
                     document.getElementById("MainContent_txt_UserRePass").focus();
                     return (false);
                 }
                 if (document.getElementById('<%=txt_UserRePass.ClientID%>').value.length > 10) {
                     alert("Length of \"Re-Password\" should be Max 10 characters.");
                     document.getElementById("MainContent_txt_UserRePass").focus();
                     return (false);
                 }
                 if (pwd != repwd) {
                     alert("Please Re-Type the correct \"Password\" field.");
                     document.getElementById("MainContent_txt_UserRePass").focus();
                     return (false);
                 }

                 else {
                     MyHashed = hex_md5(document.getElementById('<%=txt_UserPass.ClientID%>').value);
                     document.getElementById("MainContent_HashPass2").value = MyHashed;
                     document.getElementById('<%=txt_UserPass.ClientID%>').value = "";
                     document.getElementById('<%=txt_UserRePass.ClientID%>').value = "";
                     return (true);
                 }

                 return (false);
             }
         }
    </script>
    
   
    
        <table id="ADMT1" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
            <tr>
                <td  bgcolor="#003366" class="style43" rowspan="1" style="color: #FFFFFF">
                    <strong>Manage Memberships</strong></td>
            </tr>
        </table>      
                   

        
        <div class="style3">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always" >
                    <ContentTemplate>
            <asp:Menu
                ID="Menu1"
                Width="98%"
                runat="server"
                Orientation="Horizontal"
                StaticEnableDefaultPopOutImage="False"
                 OnMenuItemClick="Menu1_MenuItemClick">
                <Items>
                    <asp:MenuItem ImageUrl="~/Images/CategoriesUP.png" Text="" Value="0" Selected="true"></asp:MenuItem>
                    <asp:MenuItem ImageUrl="~/Images/Sub-CategoriesOver.png" Text="" Value="1"></asp:MenuItem>
                    <asp:MenuItem ImageUrl="~/Images/RegistrationOver.png" Text="" Value="2"></asp:MenuItem>
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
        <table id="Table1" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
            <tr>
                <td class="style56" colspan="5" bgcolor="#336699">
                   <asp:Label ID="Label3" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True" style="font-size: medium">Member Categories (Groups/Division/Section)</asp:Label>
                </td>
            </tr>
           
             <tr>
                 <td class="style54" colspan="5">
                   <ajaxToolkit:Accordion ID="Accordion2" runat="server" CssClass="accordion"  
                        HeaderCssClass="accordionHeader"  
                        HeaderSelectedCssClass="accordionHeaderSelected"  
                        ContentCssClass="accordionContent" Enabled="true" Visible="true" 
                         RequireOpenedPane="false"     SuppressHeaderPostbacks="true"   
                        SelectedIndex="1"   Width="100%" Height="237px" 
                         style="text-align: center"   ><Panes><ajaxToolkit:AccordionPane ID="SearchPane" runat="server" ><Header>Click To View / Hide Search Pane</Header><Content><asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional"><ContentTemplate><asp:Label ID="Label1" runat="server" Text="Record(s): "></asp:Label><div><asp:Button ID="Cat_DeleteAll_Bttn" runat="server"  CssClass="styleBttn" Font-Bold="True" 
                        ForeColor="Red" TabIndex="14" Text="Delete Selected Row(s)" AccessKey="d"    
                            Width="165px" Height="30px" Enabled="false" /></div><asp:Panel ID="Panel1" runat="server" Height="250px" ScrollBars="Auto"><asp:GridView ID="Grid1" runat="server" AllowPaging="True" DataKeyNames="CAT_ID"  
                        style="width: 100%;  text-align: center;" allowsorting="True" 
                        AutoGenerateColumns="False" PageSize="10"  Font-Bold="True" 
                        Font-Names="Tahoma" Font-Size="8pt" Height="100px"  
                        HorizontalAlign="Center"  Width="6px" ShowFooter="True"><Columns ><asp:TemplateField HeaderText="S.N."><ItemTemplate><asp:Label ID="lblsr"  runat="server" CssClass="MBody"   Text = '<%# Container.dataitemindex+1 %>' SkinID="" width="25px"></asp:Label></ItemTemplate><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" /><ItemStyle ForeColor="#336699" Font-Names="Tahoma" Width="20px" Font-Bold="True" Font-Size="Small" /></asp:TemplateField><asp:ButtonField HeaderText="Edit"  Text="Edit" CommandName="Select"><HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" /><ItemStyle ForeColor="#CC0000" Width="20px" Font-Bold="true" /></asp:ButtonField><asp:BoundField   DataField="CAT_NAME" HeaderText="Member Categories" SortExpression="CAT_NAME" ><HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" /><ItemStyle forecolor="#0066FF" horizontalalign="Left" width="630px" 
                                Font-Names="Arial"/></asp:BoundField><asp:BoundField   DataField="CAT_DESC" HeaderText="Remarks" ReadOnly="True"><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" /><ItemStyle forecolor="#0066FF" horizontalalign="Left" Font-Names="Arial"  
                                width="150px" /></asp:BoundField><asp:TemplateField  ControlStyle-Width="10px"  HeaderText="Delete" FooterText="Select to Delete" ShowHeader="true" ><HeaderTemplate><asp:ImageButton ID="ImageButton1" runat="server" Height="16px" Width="16px" ToolTip="Select All" ImageUrl="~/Images/check_all.gif" onClientclick="return Select(true)"  /><asp:ImageButton ID="ImageButton2" runat="server" Height="16px" Width="16px" ToolTip="Deselect All" ImageUrl="~/Images/uncheck_all.gif"  OnClientClick ="return Select(false)" /></HeaderTemplate><ItemTemplate><asp:CheckBox ID="cbd"  runat="server" /></ItemTemplate><ControlStyle Width="50px"></ControlStyle></asp:TemplateField></Columns><PagerStyle BackColor="#3399FF" BorderColor="#C0C000" BorderStyle="Solid" 
                        Font-Bold="True" ForeColor="White" HorizontalAlign="Center" 
                        VerticalAlign="Middle" Font-Size="Small" /><RowStyle BackColor="#DADADA" BorderColor="Desktop" ForeColor="#3399FF" /><SelectedRowStyle BackColor="Desktop" BorderColor="SteelBlue" BorderStyle="Solid" /><HeaderStyle BackColor="Desktop" Font-Bold="True" Font-Italic="False" Font-Names="Times new roman"
                        Font-Overline="False" Font-Underline="False" ForeColor="White" Width="80%" /><PagerSettings Position="TopAndBottom" FirstPageText="First" LastPageText="Last" PageButtonCount="20" Mode="NumericFirstLast" /><AlternatingRowStyle BackColor="#EFEFEF" Font-Names="Tahoma" ForeColor="#0066FF" /></asp:GridView></asp:Panel></ContentTemplate></asp:UpdatePanel></Content></ajaxToolkit:AccordionPane></Panes></ajaxToolkit:Accordion>
                       
                </td>
             </tr>
                        
            <tr>
                <td class="style56" colspan="5">
                   <asp:Label ID="Label6" runat="server" Font-Size="Medium" ForeColor="Yellow" 
                        Font-Bold="True"></asp:Label>
                    <asp:Label ID="Label15" runat="server" Font-Bold="True" Font-Size="Medium" 
                        ForeColor="White"></asp:Label>
                </td>
            </tr>            
            <tr>
                <td class="style53"> 
                    Category Name*</td>
                <td class="style55" colspan="3">
                    <asp:TextBox ID="txt_Cir_Category" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="18px" MaxLength="150" 
                        ToolTip="Enter Distinct Member Category " Width="99%" Wrap="False" 
                        TabIndex="1"></asp:TextBox>
                        <ajaxToolkit:AutoCompleteExtender 
                        ID="txt_Cir_Category_AutoCompleteExtender" 
                        runat="server" 
                        MinimumPrefixLength="1"
                        EnableCaching="true" 
                        CompletionSetCount="10"
                        ServiceMethod="SearchCategories"
                        TargetControlID="txt_Cir_Category" 
                        FirstRowSelected = "false"></ajaxToolkit:AutoCompleteExtender>
                </td>
                <td class="style55">
                    <asp:Label ID="Label23" runat="server"></asp:Label>
                </td>                
            </tr>
           
             

             <tr>
                <td class="style53"> 
                    Remarks</td>
                <td class="style55" colspan="4">                 
                                     
                    <asp:TextBox ID="txt_Cat_Remarks" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="18px" MaxLength="250" 
                        ToolTip="Enter Remarks, if any." Width="99%" Wrap="False" TabIndex="2"></asp:TextBox>
                                     
                 </td>                
            </tr>
        
            <tr>
                <td class="style47" colspan="5">
                    &nbsp; * Mandatory</td>
            </tr>
             <tr>
                <td class="style47" colspan="5">
                    <asp:Button ID="Cat_Save_Bttn" runat="server" AccessKey="s" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" Height="23px" 
                         TabIndex="3" Text="Save" 
                        Width="74px" Visible="False" ToolTip="Press to SAVE Record" 
                        OnClientClick="return valid1();"/>
                    <asp:Button ID="Cat_Update_Bttn" runat="server" AccessKey="u" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" Height="23px" 
                         TabIndex="5" Text="Update" 
                        Width="74px" Visible="False" ToolTip="Press to UPDATE Record" 
                        OnClientClick="return valid1();" />
                    <asp:Button ID="Cat_Cancel_Bttn" runat="server" AccessKey="c" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" Height="23px" 
                        TabIndex="6" Text="Cancel" 
                        Width="74px" ToolTip="Press to Cancel" />
                 </td>
            </tr>

             <tr>  
                <td class="style56" colspan="5" bgcolor="#336699">&nbsp;</td>
            </tr>       

        </table>

                
          
  
        
     </asp:View>
    



    
    <asp:View ID="Tab2" runat="server">
       
        <table id="Table2" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
            <tr>
                <td class="style56" colspan="5" bgcolor="#336699">
                   <asp:Label ID="Label2" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True" style="font-size: medium">Member Sub-Categories (Designations/Class)</asp:Label>
                </td>
            </tr>
           
             <tr>
                 <td class="style54" colspan="5">
                         




                    <ajaxToolkit:Accordion ID="Accordion1" runat="server" CssClass="accordion"  
                        HeaderCssClass="accordionHeader"  
                        HeaderSelectedCssClass="accordionHeaderSelected"  
                        ContentCssClass="accordionContent" Enabled="true" Visible="true" 
                         RequireOpenedPane="false"     SuppressHeaderPostbacks="true"   
                        SelectedIndex="1"   Width="100%" Height="237px" 
                         style="text-align: center"   ><Panes><ajaxToolkit:AccordionPane ID="AccordionPane1" runat="server" ><Header>Click To View / Hide Search Pane</Header><Content><asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional"><ContentTemplate><asp:Label ID="Label4" runat="server" Text="Record(s): "></asp:Label><div><asp:Button ID="SubCat_DeleteAll_Bttn" runat="server"  CssClass="styleBttn" Font-Bold="True" 
                        ForeColor="Red" TabIndex="14" Text="Delete Selected Row(s)" AccessKey="d"    
                            Width="165px" Height="30px" Enabled="false" /></div><asp:Panel ID="Panel2" runat="server" Height="250px" ScrollBars="Auto"><asp:GridView ID="Grid2" runat="server" AllowPaging="True" DataKeyNames="SUBCAT_ID"  
                        style="width: 100%;  text-align: center;" allowsorting="True" 
                        AutoGenerateColumns="False" PageSize="10"  Font-Bold="True" 
                        Font-Names="Tahoma" Font-Size="8pt" Height="100px"  
                        HorizontalAlign="Center"  Width="6px" ShowFooter="True"><Columns ><asp:TemplateField HeaderText="S.N."><ItemTemplate><asp:Label ID="lblsr"  runat="server" CssClass="MBody"   Text = '<%# Container.dataitemindex+1 %>' SkinID="" width="25px"></asp:Label></ItemTemplate><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" /><ItemStyle ForeColor="#336699" Font-Names="Tahoma" Width="20px" Font-Bold="True" Font-Size="Small" /></asp:TemplateField><asp:ButtonField HeaderText="Edit"  Text="Edit" CommandName="Select"><HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" /><ItemStyle ForeColor="#CC0000" Width="20px" Font-Bold="true" /></asp:ButtonField><asp:BoundField   DataField="SUBCAT_NAME" HeaderText="Member Sub Categories" SortExpression="SUBCAT_NAME" ><HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" /><ItemStyle forecolor="#0066FF" horizontalalign="Left" width="630px" 
                                Font-Names="Arial"/></asp:BoundField><asp:BoundField   DataField="SUBCAT_DESC" HeaderText="Remarks" ReadOnly="True"><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" /><ItemStyle forecolor="#0066FF" horizontalalign="Left" Font-Names="Arial"  
                                width="150px" /></asp:BoundField><asp:TemplateField  ControlStyle-Width="10px"  HeaderText="Delete" FooterText="Select to Delete" ShowHeader="true" ><HeaderTemplate><asp:ImageButton ID="ImageButton1" runat="server" Height="16px" Width="16px" ToolTip="Select All" ImageUrl="~/Images/check_all.gif" onClientclick="return Select2(true)"  /><asp:ImageButton ID="ImageButton2" runat="server" Height="16px" Width="16px" ToolTip="Deselect All" ImageUrl="~/Images/uncheck_all.gif"  OnClientClick ="return Select2(false)" /></HeaderTemplate><ItemTemplate><asp:CheckBox ID="cbd"  runat="server" /></ItemTemplate><ControlStyle Width="50px"></ControlStyle></asp:TemplateField></Columns><PagerStyle BackColor="#3399FF" BorderColor="#C0C000" BorderStyle="Solid" 
                        Font-Bold="True" ForeColor="White" HorizontalAlign="Center" 
                        VerticalAlign="Middle" Font-Size="Small" /><RowStyle BackColor="#DADADA" BorderColor="Desktop" ForeColor="#3399FF" /><SelectedRowStyle BackColor="Desktop" BorderColor="SteelBlue" BorderStyle="Solid" /><HeaderStyle BackColor="Desktop" Font-Bold="True" Font-Italic="False" Font-Names="Times new roman"
                        Font-Overline="False" Font-Underline="False" ForeColor="White" Width="80%" /><PagerSettings Position="TopAndBottom" FirstPageText="First" LastPageText="Last" PageButtonCount="20" Mode="NumericFirstLast" /><AlternatingRowStyle BackColor="#EFEFEF" Font-Names="Tahoma" ForeColor="#0066FF" /></asp:GridView></asp:Panel></ContentTemplate></asp:UpdatePanel></Content></ajaxToolkit:AccordionPane></Panes></ajaxToolkit:Accordion>
                </td>
             </tr>
                        
            <tr>
                <td class="style56" colspan="5">
                   <asp:Label ID="Label5" runat="server" Font-Size="Medium" ForeColor="Yellow" 
                        Font-Bold="True"></asp:Label>
                    <asp:Label ID="Label7" runat="server" Font-Bold="True" Font-Size="Medium" 
                        ForeColor="White"></asp:Label>
                </td>
            </tr>            
            <tr>
                <td class="style53"> 
                    <strong>Sub Category Name*</strong></td>
                <td class="style55" colspan="2">
                    <asp:TextBox ID="txt_Cir_SubCatName" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="18px" MaxLength="150" 
                        ToolTip="Enter Distinct Member Sub Category " Width="70%" Wrap="False" 
                        TabIndex="15"></asp:TextBox>
                        <ajaxToolkit:AutoCompleteExtender 
                        ID="txt_Cir_SubCatName_AutoCompleteExtender1" 
                        runat="server" 
                        MinimumPrefixLength="1"
                        EnableCaching="true" 
                        CompletionSetCount="10"
                        ServiceMethod="SearchSubCategories"
                        TargetControlID="txt_Cir_SubCatName" 
                        FirstRowSelected = "false"></ajaxToolkit:AutoCompleteExtender>
                </td>
                <td class="style55" colspan="2">
                    <asp:Label ID="Label8" runat="server"></asp:Label>
                    &nbsp;Fine System*
                    <asp:DropDownList ID="DDL_FineSystem" runat="server" AutoPostBack="True" 
                        Font-Bold="True" ForeColor="#0066CC" ToolTip="Select Fine System" 
                        TabIndex="16">
                        <asp:ListItem Selected="True" Value="F">Flat Fine Rate</asp:ListItem>
                        <asp:ListItem Value="V">Variable Fine Rate</asp:ListItem>
                        <asp:ListItem Value="N">No Fine</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;(Rs./Day)</td>                
            </tr>

            <tr>
                <td class="style53"> 
                    <strong>Remarks</strong></td>
                <td class="style55" colspan="4">                        
                    <asp:TextBox ID="txt_Cir_SubCatRemarks" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="18px" MaxLength="250" 
                        ToolTip="Enter Remarks, if any." Width="99%" Wrap="False" TabIndex="17"></asp:TextBox>           
                 </td>                
            </tr>

             <tr>
                <td class="style53"> 
                    <strong>Fine System</strong></td>
                <td class="style55" colspan="4">                        
                            
                    <asp:Label ID="Label24" runat="server" ForeColor="#CC3300"></asp:Label>
                            
                 </td>                
            </tr>
        
            <tr>
                <td class="style53">
                    <strong>Material Type</strong></td>
                <td class="style58">
                    <strong>Entitlement/No of Docs</strong></td>
                <td class="style58">
                    <strong>Due Days/No of Days</strong></td>
                <td class="style58">
                    <strong>First&nbsp;
                    <asp:TextBox ID="txt_Cir_Gap1" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="16px" MaxLength="3" 
                        onkeypress="return isNumberKey(event)" 
                        ToolTip="Enter No of Days Fine will be Charged as First Rate" Width="10%" 
                        Wrap="False" BorderColor="#0066FF" BorderStyle="Solid" TabIndex="44"></asp:TextBox>
                   
                    &nbsp;Days Fine Per Day</strong></td>
                <td class="style58">
                    <strong>Rest of Days Fine Per Day</strong></td>
            </tr>
             <tr>
                <td class="style53">
                    Books</td>
                <td class="style58">
                    <strong>
                    <asp:TextBox ID="txt_Ent_Books" runat="server" AutoCompleteType="DisplayName" 
                        BorderColor="#0066FF" BorderStyle="Solid" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="4" onkeypress="return isNumberKey(event)" 
                        ToolTip="Enter No of Documents to Be Issued" Width="50px" 
                        Wrap="False" TabIndex="18"></asp:TextBox>
                    </strong></td>
                <td class="style58">
                    <strong>
                    <asp:TextBox ID="txt_DueDays_Books" runat="server" 
                        AutoCompleteType="DisplayName" BorderColor="#0066FF" BorderStyle="Solid" 
                        Font-Bold="True" ForeColor="#0066FF" Height="16px" MaxLength="4" 
                        onkeypress="return isNumberKey(event)" 
                        ToolTip="Enter No of Days Documetns will be Issued" Width="50px" 
                        Wrap="False" TabIndex="31"></asp:TextBox>
                    </strong></td>
                <td class="style58">
                    <strong>&nbsp;<asp:TextBox ID="txt_Fine1_Books" runat="server" AutoCompleteType="DisplayName" 
                        BorderColor="#0066FF" BorderStyle="Solid" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="6" onkeypress="return isCurrencyKey(event)" 
                        ToolTip="Enter No of Days Fine will be Charged as First Rate" Width="50px" 
                        Wrap="False" TabIndex="45"></asp:TextBox>
                    </strong></td>
                <td class="style58">
                    <strong>&nbsp;<asp:TextBox ID="txt_Fine2_Books" runat="server" AutoCompleteType="DisplayName" 
                        BorderColor="#0066FF" BorderStyle="Solid" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="6" onkeypress="return isCurrencyKey(event)" 
                        ToolTip="Enter Fine in Rupees per Day for Rest of the overdue Days" Width="50px" 
                        Wrap="False" TabIndex="57"></asp:TextBox>
                    </strong></td>
            </tr>
             <tr>
                <td class="style53">
                    Manuals</td>
                <td class="style58">
                    <strong>
                    <asp:TextBox ID="txt_Ent_Manuals" runat="server" AutoCompleteType="DisplayName" 
                        BorderColor="#0066FF" BorderStyle="Solid" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="4" onkeypress="return isNumberKey(event)" 
                        ToolTip="Enter No of Documents to Be Issued" Width="50px" 
                        Wrap="False" TabIndex="19"></asp:TextBox>
                    </strong></td>
                <td class="style58">
                    <strong>
                    <asp:TextBox ID="txt_DueDays_Manuals" runat="server" 
                        AutoCompleteType="DisplayName" BorderColor="#0066FF" BorderStyle="Solid" 
                        Font-Bold="True" ForeColor="#0066FF" Height="16px" MaxLength="4" 
                        onkeypress="return isNumberKey(event)" 
                        ToolTip="Enter No of Days Documetns will be Issued" Width="50px" 
                        Wrap="False" TabIndex="32"></asp:TextBox>
                    </strong></td>
                <td class="style58">
                    <strong>&nbsp;<asp:TextBox ID="txt_Fine1_Manuals" runat="server" AutoCompleteType="DisplayName" 
                        BorderColor="#0066FF" BorderStyle="Solid" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="6" onkeypress="return isCurrencyKey(event)" 
                        ToolTip="Enter No of Days Fine will be Charged as First Rate" Width="50px" 
                        Wrap="False" TabIndex="46"></asp:TextBox>
                    </strong></td>
                <td class="style58">
                    <strong>&nbsp;<asp:TextBox ID="txt_Fine2_Manuals" runat="server" AutoCompleteType="DisplayName" 
                        BorderColor="#0066FF" BorderStyle="Solid" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="6" onkeypress="return isCurrencyKey(event)" 
                        ToolTip="Enter Fine in Rupees per Day for Rest of the overdue Days" Width="50px" 
                        Wrap="False" TabIndex="58"></asp:TextBox>
                    </strong></td>
            </tr>
             <tr>
                <td class="style53">
                    Patents</td>
                <td class="style58">
                    <strong>
                    <asp:TextBox ID="txt_Ent_Patents" runat="server" AutoCompleteType="DisplayName" 
                        BorderColor="#0066FF" BorderStyle="Solid" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="4" onkeypress="return isNumberKey(event)" 
                        ToolTip="Enter No of Documents to Be Issued" Width="50px" 
                        Wrap="False" TabIndex="20"></asp:TextBox>
                    </strong></td>
                <td class="style58">
                    <strong>
                    <asp:TextBox ID="txt_DueDays_Patents" runat="server" 
                        AutoCompleteType="DisplayName" BorderColor="#0066FF" BorderStyle="Solid" 
                        Font-Bold="True" ForeColor="#0066FF" Height="16px" MaxLength="4" 
                        onkeypress="return isNumberKey(event)" 
                        ToolTip="Enter No of Days Documetns will be Issued" Width="50px" 
                        Wrap="False" TabIndex="33"></asp:TextBox>
                    </strong></td>
                <td class="style58">
                    <strong>&nbsp;<asp:TextBox ID="txt_Fine1_Patents" runat="server" AutoCompleteType="DisplayName" 
                        BorderColor="#0066FF" BorderStyle="Solid" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="6" onkeypress="return isCurrencyKey(event)" 
                        ToolTip="Enter No of Days Fine will be Charged as First Rate" Width="50px" 
                        Wrap="False" TabIndex="47"></asp:TextBox>
                    </strong></td>
                <td class="style58">
                    <strong>&nbsp;<asp:TextBox ID="txt_Fine2_Patents" runat="server" AutoCompleteType="DisplayName" 
                        BorderColor="#0066FF" BorderStyle="Solid" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="6" onkeypress="return isCurrencyKey(event)" 
                        ToolTip="Enter Fine in Rupees per Day for Rest of the overdue Days" Width="50px" 
                        Wrap="False" TabIndex="59"></asp:TextBox>
                    </strong></td>
            </tr>
             <tr>
                <td class="style53">
                    Reports</td>
                <td class="style58">
                    <strong>
                    <asp:TextBox ID="txt_Ent_Reports" runat="server" AutoCompleteType="DisplayName" 
                        BorderColor="#0066FF" BorderStyle="Solid" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="4" onkeypress="return isNumberKey(event)" 
                        ToolTip="Enter No of Documents to Be Issued" Width="50px" 
                        Wrap="False" TabIndex="21"></asp:TextBox>
                    </strong></td>
                <td class="style58">
                    <strong>
                    <asp:TextBox ID="txt_DueDays_Reports" runat="server" 
                        AutoCompleteType="DisplayName" BorderColor="#0066FF" BorderStyle="Solid" 
                        Font-Bold="True" ForeColor="#0066FF" Height="16px" MaxLength="4" 
                        onkeypress="return isNumberKey(event)" 
                        ToolTip="Enter No of Days Documetns will be Issued" Width="50px" 
                        Wrap="False" TabIndex="34"></asp:TextBox>
                    </strong></td>
                <td class="style58">
                    <strong>&nbsp;<asp:TextBox ID="txt_Fine1_Reports" runat="server" AutoCompleteType="DisplayName" 
                        BorderColor="#0066FF" BorderStyle="Solid" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="6" onkeypress="return isCurrencyKey(event)" 
                        ToolTip="Enter No of Days Fine will be Charged as First Rate" Width="50px" 
                        Wrap="False" TabIndex="48"></asp:TextBox>
                    </strong></td>
                <td class="style58">
                    <strong>&nbsp;<asp:TextBox ID="txt_Fine2_Reports" runat="server" AutoCompleteType="DisplayName" 
                        BorderColor="#0066FF" BorderStyle="Solid" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="6" onkeypress="return isCurrencyKey(event)" 
                        ToolTip="Enter Fine in Rupees per Day for Rest of the overdue Days" Width="50px" 
                        Wrap="False" TabIndex="60"></asp:TextBox>
                    </strong></td>
            </tr>
             <tr>
                <td class="style53">
                    Standards</td>
                <td class="style58">
                    <strong>
                    <asp:TextBox ID="txt_Ent_Standards" runat="server" AutoCompleteType="DisplayName" 
                        BorderColor="#0066FF" BorderStyle="Solid" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="4" onkeypress="return isNumberKey(event)" 
                        ToolTip="Enter No of Documents to Be Issued" Width="50px" 
                        Wrap="False" TabIndex="22"></asp:TextBox>
                    </strong></td>
                <td class="style58">
                    <strong>
                    <asp:TextBox ID="txt_DueDays_Standards" runat="server" 
                        AutoCompleteType="DisplayName" BorderColor="#0066FF" BorderStyle="Solid" 
                        Font-Bold="True" ForeColor="#0066FF" Height="16px" MaxLength="4" 
                        onkeypress="return isNumberKey(event)" 
                        ToolTip="Enter No of Days Documetns will be Issued" Width="50px" 
                        Wrap="False" TabIndex="35"></asp:TextBox>
                    </strong></td>
                <td class="style58">
                    <strong>&nbsp;<asp:TextBox ID="txt_Fine1_Standards" runat="server" AutoCompleteType="DisplayName" 
                        BorderColor="#0066FF" BorderStyle="Solid" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="6" onkeypress="return isCurrencyKey(event)" 
                        ToolTip="Enter No of Days Fine will be Charged as First Rate" Width="50px" 
                        Wrap="False" TabIndex="49"></asp:TextBox>
                    </strong></td>
                <td class="style58">
                    <strong>&nbsp;<asp:TextBox ID="txt_Fine2_Standards" runat="server" AutoCompleteType="DisplayName" 
                        BorderColor="#0066FF" BorderStyle="Solid" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="6" onkeypress="return isCurrencyKey(event)" 
                        ToolTip="Enter Fine in Rupees per Day for Rest of the overdue Days" Width="50px" 
                        Wrap="False" TabIndex="61"></asp:TextBox>
                    </strong></td>
            </tr>
             <tr>
                <td class="style53">
                    Loose Issues</td>
                <td class="style58">
                    <strong>
                    <asp:TextBox ID="txt_Ent_LooseIssues" runat="server" AutoCompleteType="DisplayName" 
                        BorderColor="#0066FF" BorderStyle="Solid" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="4" onkeypress="return isNumberKey(event)" 
                        ToolTip="Enter No of Documents to Be Issued" Width="50px" 
                        Wrap="False" TabIndex="23"></asp:TextBox>
                    </strong></td>
                <td class="style58">
                    <strong>
                    <asp:TextBox ID="txt_DueDays_Loose" runat="server" 
                        AutoCompleteType="DisplayName" BorderColor="#0066FF" BorderStyle="Solid" 
                        Font-Bold="True" ForeColor="#0066FF" Height="16px" MaxLength="4" 
                        onkeypress="return isNumberKey(event)" 
                        ToolTip="Enter No of Days Documetns will be Issued" Width="50px" 
                        Wrap="False" TabIndex="36"></asp:TextBox>
                    </strong></td>
                <td class="style58">
                    <strong>&nbsp;<asp:TextBox ID="txt_Fine1_Loose" runat="server" AutoCompleteType="DisplayName" 
                        BorderColor="#0066FF" BorderStyle="Solid" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="6" onkeypress="return isCurrencyKey(event)" 
                        ToolTip="Enter No of Days Fine will be Charged as First Rate" Width="50px" 
                        Wrap="False" TabIndex="50"></asp:TextBox>
                    </strong></td>
                <td class="style58">
                    <strong>&nbsp;<asp:TextBox ID="txt_Fine2_Loose" runat="server" AutoCompleteType="DisplayName" 
                        BorderColor="#0066FF" BorderStyle="Solid" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="6" onkeypress="return isCurrencyKey(event)" 
                        ToolTip="Enter Fine in Rupees per Day for Rest of the overdue Days" Width="50px" 
                        Wrap="False" TabIndex="61"></asp:TextBox>
                    </strong></td>
            </tr>
             <tr>
                <td class="style53">
                    Bound Journals</td>
                <td class="style58">
                    <strong>
                    <asp:TextBox ID="txt_Ent_BoundJ" runat="server" AutoCompleteType="DisplayName" 
                        BorderColor="#0066FF" BorderStyle="Solid" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="4" onkeypress="return isNumberKey(event)" 
                        ToolTip="Enter No of Documents to Be Issued" Width="50px" 
                        Wrap="False" TabIndex="24"></asp:TextBox>
                    </strong></td>
                <td class="style58">
                    <strong>
                    <asp:TextBox ID="txt_DueDays_BoundJ" runat="server" 
                        AutoCompleteType="DisplayName" BorderColor="#0066FF" BorderStyle="Solid" 
                        Font-Bold="True" ForeColor="#0066FF" Height="16px" MaxLength="4" 
                        onkeypress="return isNumberKey(event)" 
                        ToolTip="Enter No of Days Documetns will be Issued" Width="50px" 
                        Wrap="False" TabIndex="37"></asp:TextBox>
                    </strong></td>
                <td class="style58">
                    <strong>&nbsp;<asp:TextBox ID="txt_Fine1_BoundJ" runat="server" AutoCompleteType="DisplayName" 
                        BorderColor="#0066FF" BorderStyle="Solid" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="6" onkeypress="return isCurrencyKey(event)" 
                        ToolTip="Enter No of Days Fine will be Charged as First Rate" Width="50px" 
                        Wrap="False" TabIndex="51"></asp:TextBox>
                    </strong></td>
                <td class="style58">
                    <strong>&nbsp;<asp:TextBox ID="txt_Fine2_BoundJ" runat="server" AutoCompleteType="DisplayName" 
                        BorderColor="#0066FF" BorderStyle="Solid" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="6" onkeypress="return isCurrencyKey(event)" 
                        ToolTip="Enter Fine in Rupees per Day for Rest of the overdue Days" Width="50px" 
                        Wrap="False" TabIndex="62"></asp:TextBox>
                    </strong></td>
            </tr>
             <tr>
                <td class="style53">
                    Audio Visuals</td>
                <td class="style58">
                    <strong>
                    <asp:TextBox ID="txt_Ent_AV" runat="server" AutoCompleteType="DisplayName" 
                        BorderColor="#0066FF" BorderStyle="Solid" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="4" onkeypress="return isNumberKey(event)" 
                        ToolTip="Enter No of Documents to Be Issued" Width="50px" 
                        Wrap="False" TabIndex="25"></asp:TextBox>
                    </strong></td>
                <td class="style58">
                    <strong>
                    <asp:TextBox ID="txt_DueDays_AV" runat="server" 
                        AutoCompleteType="DisplayName" BorderColor="#0066FF" BorderStyle="Solid" 
                        Font-Bold="True" ForeColor="#0066FF" Height="16px" MaxLength="4" 
                        onkeypress="return isNumberKey(event)" 
                        ToolTip="Enter No of Days Documetns will be Issued" Width="50px" 
                        Wrap="False" TabIndex="38"></asp:TextBox>
                    </strong></td>
                <td class="style58">
                    <strong>&nbsp;<asp:TextBox ID="txt_Fine1_AV" runat="server" AutoCompleteType="DisplayName" 
                        BorderColor="#0066FF" BorderStyle="Solid" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="6" onkeypress="return isCurrencyKey(event)" 
                        ToolTip="Enter No of Days Fine will be Charged as First Rate" Width="50px" 
                        Wrap="False" TabIndex="52"></asp:TextBox>
                    </strong></td>
                <td class="style58">
                    <strong>&nbsp;<asp:TextBox ID="txt_Fine2_AV" runat="server" AutoCompleteType="DisplayName" 
                        BorderColor="#0066FF" BorderStyle="Solid" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="6" onkeypress="return isCurrencyKey(event)" 
                        ToolTip="Enter Fine in Rupees per Day for Rest of the overdue Days" Width="50px" 
                        Wrap="False" TabIndex="63"></asp:TextBox>
                    </strong></td>
            </tr>
             <tr>
                <td class="style53">
                    Cartographic</td>
                <td class="style58">
                    <strong>
                    <asp:TextBox ID="txt_Ent_Cartographic" runat="server" AutoCompleteType="DisplayName" 
                        BorderColor="#0066FF" BorderStyle="Solid" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="4" onkeypress="return isNumberKey(event)" 
                        ToolTip="Enter No of Documents to Be Issued" Width="50px" 
                        Wrap="False" TabIndex="26"></asp:TextBox>
                    </strong></td>
                <td class="style58">
                    <strong>
                    <asp:TextBox ID="txt_DueDays_Cartographic" runat="server" 
                        AutoCompleteType="DisplayName" BorderColor="#0066FF" BorderStyle="Solid" 
                        Font-Bold="True" ForeColor="#0066FF" Height="16px" MaxLength="4" 
                        onkeypress="return isNumberKey(event)" 
                        ToolTip="Enter No of Days Documetns will be Issued" Width="50px" 
                        Wrap="False" TabIndex="39"></asp:TextBox>
                    </strong></td>
                <td class="style58">
                    <strong>&nbsp;<asp:TextBox ID="txt_Fine1_Cartographic" runat="server" AutoCompleteType="DisplayName" 
                        BorderColor="#0066FF" BorderStyle="Solid" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="6" onkeypress="return isCurrencyKey(event)" 
                        ToolTip="Enter No of Days Fine will be Charged as First Rate" Width="50px" 
                        Wrap="False" TabIndex="53"></asp:TextBox>
                    </strong></td>
                <td class="style58">
                    <strong>&nbsp;<asp:TextBox ID="txt_Fine2_Cartographic" runat="server" AutoCompleteType="DisplayName" 
                        BorderColor="#0066FF" BorderStyle="Solid" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="6" onkeypress="return isCurrencyKey(event)" 
                        ToolTip="Enter Fine in Rupees per Day for Rest of the overdue Days" Width="50px" 
                        Wrap="False" TabIndex="64"></asp:TextBox>
                    </strong></td>
            </tr>
             <tr>
                <td class="style53">
                    Manuscripts</td>
                <td class="style58">
                    <strong>
                    <asp:TextBox ID="txt_Ent_Manuscripts" runat="server" AutoCompleteType="DisplayName" 
                        BorderColor="#0066FF" BorderStyle="Solid" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="4" onkeypress="return isNumberKey(event)" 
                        ToolTip="Enter No of Documents to Be Issued" Width="50px" 
                        Wrap="False" TabIndex="27"></asp:TextBox>
                    </strong></td>
                <td class="style58">
                    <strong>
                    <asp:TextBox ID="txt_DueDays_Manuscripts" runat="server" 
                        AutoCompleteType="DisplayName" BorderColor="#0066FF" BorderStyle="Solid" 
                        Font-Bold="True" ForeColor="#0066FF" Height="16px" MaxLength="4" 
                        onkeypress="return isNumberKey(event)" 
                        ToolTip="Enter No of Days Documetns will be Issued" Width="50px" 
                        Wrap="False" TabIndex="40"></asp:TextBox>
                    </strong></td>
                <td class="style58">
                    <strong>&nbsp;<asp:TextBox ID="txt_Fine1_Manuscripts" runat="server" AutoCompleteType="DisplayName" 
                        BorderColor="#0066FF" BorderStyle="Solid" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="6" onkeypress="return isCurrencyKey(event)" 
                        ToolTip="Enter No of Days Fine will be Charged as First Rate" Width="50px" 
                        Wrap="False" TabIndex="54"></asp:TextBox>
                    </strong></td>
                <td class="style58">
                    <strong>&nbsp;<asp:TextBox ID="txt_Fine2_Manuscripts" runat="server" AutoCompleteType="DisplayName" 
                        BorderColor="#0066FF" BorderStyle="Solid" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="6" onkeypress="return isCurrencyKey(event)" 
                        ToolTip="Enter Fine in Rupees per Day for Rest of the overdue Days" Width="50px" 
                        Wrap="False" TabIndex="65"></asp:TextBox>
                    </strong></td>
            </tr>
            <tr>
                <td class="style53">
                    Book Bank (General)</td>
                <td class="style58">
                    <strong>
                    <asp:TextBox ID="txt_Ent_BBGeneral" runat="server" AutoCompleteType="DisplayName" 
                        BorderColor="#0066FF" BorderStyle="Solid" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="4" onkeypress="return isNumberKey(event)" 
                        ToolTip="Enter No of Documents to Be Issued" Width="50px" 
                        Wrap="False" TabIndex="28"></asp:TextBox>
                    </strong></td>
                <td class="style58">
                    <strong>
                    <asp:TextBox ID="txt_DueDays_BBGeneral" runat="server" 
                        AutoCompleteType="DisplayName" BorderColor="#0066FF" BorderStyle="Solid" 
                        Font-Bold="True" ForeColor="#0066FF" Height="16px" MaxLength="4" 
                        onkeypress="return isNumberKey(event)" 
                        ToolTip="Enter No of Days Documetns will be Issued" Width="50px" 
                        Wrap="False" TabIndex="41"></asp:TextBox>
                    </strong></td>
                <td class="style58">
                    <strong>&nbsp;<asp:TextBox ID="txt_Fine1_BBGeneral" runat="server" AutoCompleteType="DisplayName" 
                        BorderColor="#0066FF" BorderStyle="Solid" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="6" onkeypress="return isCurrencyKey(event)" 
                        ToolTip="Enter No of Days Fine will be Charged as First Rate" Width="50px" 
                        Wrap="False" TabIndex="55"></asp:TextBox>
                    </strong></td>
                <td class="style58">
                    <strong>&nbsp;<asp:TextBox ID="txt_Fine2_BBGeneral" runat="server" AutoCompleteType="DisplayName" 
                        BorderColor="#0066FF" BorderStyle="Solid" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="6" onkeypress="return isCurrencyKey(event)" 
                        ToolTip="Enter Fine in Rupees per Day for Rest of the overdue Days" Width="50px" 
                        Wrap="False" TabIndex="66"></asp:TextBox>
                    </strong></td>
            </tr>
            <tr>
                <td class="style53">
                    Book Bank (Reserve)</td>
                <td class="style58">
                    <strong>
                    <asp:TextBox ID="txt_Ent_BBReserve" runat="server" AutoCompleteType="DisplayName" 
                        BorderColor="#0066FF" BorderStyle="Solid" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="4" onkeypress="return isNumberKey(event)" 
                        ToolTip="Enter No of Documents to Be Issued" Width="50px" 
                        Wrap="False" TabIndex="29"></asp:TextBox>
                    </strong></td>
                <td class="style58">
                    <strong>
                    <asp:TextBox ID="txt_DueDays_BBReserve" runat="server" 
                        AutoCompleteType="DisplayName" BorderColor="#0066FF" BorderStyle="Solid" 
                        Font-Bold="True" ForeColor="#0066FF" Height="16px" MaxLength="4" 
                        onkeypress="return isNumberKey(event)" 
                        ToolTip="Enter No of Days Documetns will be Issued" Width="50px" 
                        Wrap="False" TabIndex="42"></asp:TextBox>
                    </strong></td>
                <td class="style58">
                    <strong>&nbsp;<asp:TextBox ID="txt_Fine1_BBReserve" runat="server" AutoCompleteType="DisplayName" 
                        BorderColor="#0066FF" BorderStyle="Solid" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="6" onkeypress="return isCurrencyKey(event)" 
                        ToolTip="Enter No of Days Fine will be Charged as First Rate" Width="50px" 
                        Wrap="False" TabIndex="56"></asp:TextBox>
                    </strong></td>
                <td class="style58">
                    <strong>&nbsp;<asp:TextBox ID="txt_Fine2_BBReserve" runat="server" AutoCompleteType="DisplayName" 
                        BorderColor="#0066FF" BorderStyle="Solid" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="6" onkeypress="return isCurrencyKey(event)" 
                        ToolTip="Enter Fine in Rupees per Day for Rest of the overdue Days" Width="50px" 
                        Wrap="False" TabIndex="67"></asp:TextBox>
                    </strong></td>
            </tr>
             <tr>
                <td class="style53">
                    Non-Returnable</td>
                <td class="style58">
                    <strong>
                    <asp:TextBox ID="txt_Ent_NonReturnable" runat="server" AutoCompleteType="DisplayName" 
                        BorderColor="#0066FF" BorderStyle="Solid" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="4" onkeypress="return isNumberKey(event)" 
                        ToolTip="Enter No of Documents to Be Issued" Width="50px" 
                        Wrap="False" TabIndex="30"></asp:TextBox>
                    </strong></td>
                <td class="style58">
                    <strong>
                    <asp:TextBox ID="txt_DueDays_NonReturnable" runat="server" 
                        AutoCompleteType="DisplayName" BorderColor="#0066FF" BorderStyle="Solid" 
                        Font-Bold="True" ForeColor="#0066FF" Height="16px" MaxLength="4" 
                        onkeypress="return isNumberKey(event)" 
                        ToolTip="Enter No of Days Documetns will be Issued" Width="50px" 
                        Wrap="False" TabIndex="43"></asp:TextBox>
                    </strong></td>
                <td class="style58">
                    <strong>&nbsp;</strong></td>
                <td class="style58">
                    <strong>&nbsp;</strong></td>
            </tr>

            <tr>
                <td class="style47" colspan="5">
                    &nbsp; * Mandatory</td>
            </tr>
             <tr>
                <td class="style47" colspan="5">
                    &nbsp; <span class="style59"><strong>HELP</strong></span>: In case of Flat Fine 
                    System - Rates of Rest of the Days will be Executed.</td>
            </tr>
             <tr>
                <td class="style47" colspan="5">
                    <asp:Button ID="SubCat_Save_Bttn" runat="server" AccessKey="s" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" Height="23px" 
                         TabIndex="3" Text="Save" 
                        Width="74px" Visible="False" ToolTip="Press to SAVE Record" 
                        OnClientClick="return valid2();"/>
                    <asp:Button ID="SubCat_Update_Bttn" runat="server" AccessKey="u" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" Height="23px" 
                         TabIndex="5" Text="Update" 
                        Width="74px" Visible="False" ToolTip="Press to UPDATE Record" 
                        OnClientClick="return valid2();" />
                    <asp:Button ID="SubCat_Cancel_Bttn" runat="server" AccessKey="c" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" Height="23px" 
                        TabIndex="4" Text="Cancel" 
                        Width="74px" ToolTip="Press to Cancel" />
                 </td>
            </tr>

             <tr>  
                <td class="style56" colspan="5" bgcolor="#336699">&nbsp;</td>
            </tr>       

        </table>

          
                
        


    </asp:View>


    <asp:View ID="Tab3" runat="server">

   

        <table id="Table3" runat="server" cellspacing="2" border="1"  cellpadding="2" class="style35">
           
           
            <tr>
            
                <td class="style56" colspan="2" bgcolor="#336699">
                   <asp:Label ID="Label10" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True" style="font-size: medium">Member Registration</asp:Label>

               






                        <ajaxToolkit:Accordion ID="Accordion3" runat="server" CssClass="accordion"  
                        HeaderCssClass="accordionHeader"  
                        HeaderSelectedCssClass="accordionHeaderSelected"  
                        ContentCssClass="accordionContent" Enabled="true" Visible="true" 
                         RequireOpenedPane="false"     SuppressHeaderPostbacks="true"   
                        SelectedIndex="1"   Width="100%" Height="237px"   ><Panes><ajaxToolkit:AccordionPane ID="AccordionPane2" runat="server" ><Header>Click To View / Hide Search Pane</Header><Content>Search By:&#160; Member NO: <asp:DropDownList ID="DDL_MemNo" runat="server" ForeColor="#0066FF" 
                        Font-Bold="True" TabIndex="200" ToolTip="Plz Select Value from Drop-Down" AutoPostBack="true"></asp:DropDownList><ajaxToolkit:ListSearchExtender ID="DDL_MemNo_ListSearchExtender" 
                        runat="server" Enabled="True" PromptCssClass="PromptCSS" 
                        TargetControlID="DDL_MemNo"></ajaxToolkit:ListSearchExtender>&#160;Member Name: <asp:DropDownList ID="DDL_MemName" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" TabIndex="200" ToolTip="Plz Select Value from Drop-Down" AutoPostBack="true"></asp:DropDownList><ajaxToolkit:ListSearchExtender ID="DDL_MemName_ListSearchExtender" 
                        runat="server" Enabled="True" PromptCssClass="PromptCSS" 
                        TargetControlID="DDL_MemName"></ajaxToolkit:ListSearchExtender>&#160;Categories: <asp:DropDownList ID="DDL_AllCategories" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" TabIndex="200" ToolTip="Plz Select Value from Drop-Down" AutoPostBack="true"></asp:DropDownList><ajaxToolkit:ListSearchExtender ID="DDL_AllCategories_ListSearchExtender" 
                        runat="server" Enabled="True" PromptCssClass="PromptCSS" 
                        TargetControlID="DDL_AllCategories"></ajaxToolkit:ListSearchExtender>&#160;Sub Categories: <asp:DropDownList ID="DDL_AllSubCategories" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" TabIndex="200" ToolTip="Plz Select Value from Drop-Down" AutoPostBack="true"></asp:DropDownList><ajaxToolkit:ListSearchExtender ID="DDL_AllSubCategories_ListSearchExtender" 
                        runat="server" Enabled="True" PromptCssClass="PromptCSS" 
                        TargetControlID="DDL_AllSubCategories"></ajaxToolkit:ListSearchExtender>&#160; Status: <asp:DropDownList ID="DDL_Status2" runat="server"  AutoPostBack="true"
                        Font-Bold="True" ForeColor="#0066CC" TabIndex="200" 
                        ToolTip="Select Status"><asp:ListItem Value=""></asp:ListItem><asp:ListItem Value="CU">Current</asp:ListItem><asp:ListItem Value="CN">Cancel</asp:ListItem><asp:ListItem Value="CL">Closed</asp:ListItem></asp:DropDownList>&#160;&#160;<asp:Button ID="Search_Bttn" runat="server"  CssClass="styleBttn" Font-Bold="True" 
                        ForeColor="Red" TabIndex="14" Text="Search" AccessKey="s"     Width="74px" /><hr /><asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional"><ContentTemplate><asp:Label ID="Label16" runat="server" Text="Record(s): "></asp:Label><div style="background-image: url('../Images/back.gif')"><asp:Button ID="Mem_Delete_All" runat="server"  CssClass="styleBttn" Font-Bold="True" 
                        ForeColor="Red" TabIndex="14" Text="Delete Selected Row(s)" AccessKey="d"    
                            Width="170px" Height="30px" Enabled="false" /><asp:Button ID="Mem_DeletedPhoto_All" runat="server"  CssClass="styleBttn" Font-Bold="True" 
                        ForeColor="Red" TabIndex="14" Text="Delete Photo for Selected Members" AccessKey="p"    
                            Width="240px" Height="30px" Enabled="false" /><asp:Button ID="Mem_ChangeSubCatergory_Bttn" runat="server"  CssClass="styleBttn" Font-Bold="True" 
                        ForeColor="Red" TabIndex="14" Text="Change Class of Selected Members" AccessKey="p"    
                            Width="240px" Height="30px" Enabled="false" /><asp:Label ID="Label14" runat="server" Text="With News Sub-Category:" ForeColor="Yellow"></asp:Label><asp:DropDownList ID="DDL_NewSubCategories" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" TabIndex="200" ToolTip="Plz Select Value from Drop-Down" AutoPostBack="true"></asp:DropDownList><ajaxToolkit:ListSearchExtender ID="DDL_NewSubCategories_ListSearchExtender" 
                        runat="server" Enabled="True" PromptCssClass="PromptCSS" 
                        TargetControlID="DDL_NewSubCategories"></ajaxToolkit:ListSearchExtender></div><asp:Panel ID="Panel3" runat="server" Height="250px" ScrollBars="Auto" DefaultButton="Search_Bttn"><asp:GridView ID="Grid3" runat="server" AllowPaging="True" DataKeyNames="MEM_ID"  
                        style="width: 100%;  text-align: center;" allowsorting="True" 
                        AutoGenerateColumns="False" PageSize="100"  Font-Bold="True" 
                        Font-Names="Tahoma" Font-Size="8pt" Height="100px"  
                        HorizontalAlign="Center"  Width="6px" ShowFooter="True"><Columns ><asp:TemplateField HeaderText="S.N."><ItemTemplate><asp:Label ID="lblsr"  runat="server" CssClass="MBody"   Text = '<%# Container.dataitemindex+1 %>' SkinID="" width="25px"></asp:Label></ItemTemplate><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" /><ItemStyle ForeColor="#336699" Font-Names="Tahoma" Width="20px" Font-Bold="True" Font-Size="Small" /></asp:TemplateField><asp:ButtonField HeaderText="Edit"  Text="Edit" CommandName="Select"><HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" /><ItemStyle ForeColor="#CC0000" Width="20px" Font-Bold="true" /></asp:ButtonField><asp:BoundField   DataField="MEM_NO" HeaderText="Member No" SortExpression="MEM_NO" ><HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" /><ItemStyle forecolor="#0066FF" horizontalalign="Left" width="130px" Font-Names="Arial"/></asp:BoundField><asp:BoundField   DataField="MEM_NAME" HeaderText="Member Name" SortExpression="MEM_NAME" ><HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" /><ItemStyle forecolor="#0066FF" horizontalalign="Left" width="330px" Font-Names="Arial"/></asp:BoundField><asp:BoundField   DataField="CAT_NAME" HeaderText="Category" ReadOnly="True" SortExpression="CAT_NAME"><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" /><ItemStyle forecolor="#0066FF" horizontalalign="Left" Font-Names="Arial"  width="150px" /></asp:BoundField><asp:BoundField   DataField="SUBCAT_NAME" HeaderText="SubCategory" ReadOnly="True" SortExpression="SUBCAT_NAME"><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" /><ItemStyle forecolor="#0066FF" horizontalalign="Left" Font-Names="Arial"  width="150px" /></asp:BoundField><asp:TemplateField  ControlStyle-Width="10px"  HeaderText="Delete" FooterText="Select to Delete" ShowHeader="true" ><HeaderTemplate><asp:ImageButton ID="ImageButton1" runat="server" Height="16px" Width="16px" ToolTip="Select All" ImageUrl="~/Images/check_all.gif" onClientclick="return Select3(true)"  /><asp:ImageButton ID="ImageButton2" runat="server" Height="16px" Width="16px" ToolTip="Deselect All" ImageUrl="~/Images/uncheck_all.gif"  OnClientClick ="return Select3(false)" /></HeaderTemplate><ItemTemplate><asp:CheckBox ID="cbd"  runat="server" /></ItemTemplate><ControlStyle Width="50px"></ControlStyle></asp:TemplateField></Columns><PagerStyle BackColor="#3399FF" BorderColor="#C0C000" BorderStyle="Solid" 
                        Font-Bold="True" ForeColor="White" HorizontalAlign="Center" 
                        VerticalAlign="Middle" Font-Size="Small" /><RowStyle BackColor="#DADADA" BorderColor="Desktop" ForeColor="#3399FF" /><SelectedRowStyle BackColor="Desktop" BorderColor="SteelBlue" BorderStyle="Solid" /><HeaderStyle BackColor="Desktop" Font-Bold="True" Font-Italic="False" Font-Names="Times new roman"
                        Font-Overline="False" Font-Underline="False" ForeColor="White" Width="80%" /><PagerSettings Position="TopAndBottom" FirstPageText="First" LastPageText="Last" PageButtonCount="20" Mode="NumericFirstLast" /><AlternatingRowStyle BackColor="#EFEFEF" Font-Names="Tahoma" ForeColor="#0066FF" /></asp:GridView></asp:Panel></ContentTemplate></asp:UpdatePanel></Content></ajaxToolkit:AccordionPane></Panes></ajaxToolkit:Accordion>
                </td>
            </tr>
            <tr>
                <td class="style56" colspan="2" bgcolor="#336699">
                   <asp:Label ID="Label11" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">Enter Data and press SAVE to Save the Record!</asp:Label>
                    <asp:Label ID="Label12" runat="server" Font-Bold="True" Font-Size="Medium" 
                        ForeColor="Yellow"></asp:Label>
                </td>
            </tr>
            <tr style="font-weight:bold">
                <td class="style53"> 
                      Member Photo</td>
                <td class="style55">
                   <asp:Label ID="Label9" runat="server"></asp:Label> 
                    <asp:FileUpload ID="FileUpload1" runat="server" ViewStateMode="Enabled" />
                    <asp:Image ID="Image1" runat="server" BorderColor="#0033CC" 
                        BorderStyle="Double" BorderWidth="4px" Height="40px" ImageAlign="Middle" 
                        Width="25px" />
                    <asp:CheckBox ID="CheckBox1" runat="server" Text="Delete Photo?" 
                        Visible="False" />
                </td>
            </tr>
              <tr>
                <td class="style53"> 
                    Member No *</td>
                <td class="style54">
                       <asp:TextBox ID="txt_Mem_MemNo" runat="server" AutoCompleteType="DisplayName" 
                           Font-Bold="True" ForeColor="#0066FF" Height="18px" MaxLength="50" 
                           TabIndex="200" ToolTip="Enter Distinct Member No" Width="150px" 
                           Wrap="False" style="text-transform: uppercase" ></asp:TextBox>
                            <ajaxToolkit:AutoCompleteExtender 
                        ID="txt_Mem_MemNo_AutoCompleteExtender" 
                        runat="server" 
                        MinimumPrefixLength="1"
                        EnableCaching="true" 
                        CompletionSetCount="10"
                        ServiceMethod="SearchMemNo"
                        TargetControlID="txt_Mem_MemNo" 
                        FirstRowSelected = "false"></ajaxToolkit:AutoCompleteExtender>
                       &nbsp; Member Name *<asp:TextBox ID="txt_Mem_MemName" runat="server" 
                           AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                           Height="18px" MaxLength="150" TabIndex="200" 
                           ToolTip="Enter Distinct Member Name" Width="200px" Wrap="False"></asp:TextBox>
                       <ajaxToolkit:AutoCompleteExtender ID="txt_Mem_MemName_AutoCompleteExtender" 
                           runat="server" CompletionSetCount="10" EnableCaching="true" 
                           FirstRowSelected="false" MinimumPrefixLength="1" ServiceMethod="SearchMemName" 
                           TargetControlID="txt_Mem_MemName">
                       </ajaxToolkit:AutoCompleteExtender>
                       Gender:
                       <asp:DropDownList ID="DDL_Gender" runat="server" Font-Bold="True" 
                           ForeColor="#0066CC" TabIndex="200" ToolTip="Select Gender">
                           <asp:ListItem Selected="True" Value="M">Male</asp:ListItem>
                           <asp:ListItem Value="F">Female</asp:ListItem>
                       </asp:DropDownList>
                </td>               
            </tr>  
            
             <tr>
                <td class="style53"> 
                    Office Add</td>
                <td  class="style54">
                    <asp:TextBox ID="txt_Mem_ResAdd" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="79px" MaxLength="250" 
                        TabIndex="200" TextMode="MultiLine" ToolTip="Enter Residential Address" 
                        Width="250px" Wrap="False"></asp:TextBox>
                    Res. Add:&nbsp;
                    <asp:TextBox ID="txt_Mem_OffAdd" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="79px" MaxLength="250" 
                        TabIndex="200" TextMode="MultiLine" ToolTip="Enter Official Address" 
                        Width="250px" Wrap="False"></asp:TextBox>
                 </td>
                
            </tr>   
            
             <tr>
                <td class="style53"> 
                    Email</td>
                <td class="style54">
                   
                       <asp:TextBox ID="txt_Mem_Mail" runat="server" AutoCompleteType="DisplayName" 
                           Font-Bold="True" ForeColor="#0066FF" Height="18px" MaxLength="100" 
                           TabIndex="200" ToolTip="Enter Mail Addres" Width="200px" Wrap="False"></asp:TextBox>
                       Phone No<asp:TextBox ID="txt_Mem_Phone" runat="server" 
                           AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                           Height="18px" MaxLength="50" TabIndex="200" ToolTip="Enter Phone Number" 
                           Width="100px" Wrap="False"></asp:TextBox>
                       Mobile No.<asp:TextBox ID="txt_Mem_Mobile" runat="server" 
                           AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                           Height="18px" MaxLength="50" TabIndex="200" ToolTip="Enter Mobile Number" 
                           Width="120px" Wrap="False"></asp:TextBox>
                </td>               
            </tr>   

             <tr>
                <td class="style53"> 
                    Category *</td>
                <td class="style54">
                    <asp:DropDownList ID="DDL_Categories" runat="server" 
                        Font-Bold="True" ForeColor="#0066FF" 
                        ToolTip="Plz Select Value from Drop-Down" TabIndex="200">
                    </asp:DropDownList>
                    <ajaxToolkit:ListSearchExtender ID="ListSearchExtender1" runat="server" 
                        Enabled="True" PromptCssClass="PromptCSS" TargetControlID="DDL_Categories"></ajaxToolkit:ListSearchExtender>
                   
                      
                    Sub Category*<asp:DropDownList ID="DDL_SubCategories" runat="server" 
                        Font-Bold="True" ForeColor="#0066FF" TabIndex="200" 
                        ToolTip="Plz Select Value from Drop-Down">
                    </asp:DropDownList>
                    <ajaxToolkit:ListSearchExtender ID="DDL_SubCategories_ListSearchExtender" 
                        runat="server" Enabled="True" PromptCssClass="PromptCSS" 
                        TargetControlID="DDL_SubCategories">
                    </ajaxToolkit:ListSearchExtender>
                    Over-Ride?<asp:DropDownList ID="DDL_OverRide" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC" TabIndex="200" ToolTip="Y/N">
                        <asp:ListItem Selected="True" Value="N">No</asp:ListItem>
                        <asp:ListItem Value="Y">Yes</asp:ListItem>
                    </asp:DropDownList>
                    </td>               
            </tr>  

             <tr>
                <td class="style53"> 
                    Admission Date*</td>
                <td class="style54">
                   <asp:TextBox ID="txt_Mem_AdmissionDate" runat="server" ForeColor="#0066FF" 
                        Height="16px" MaxLength="10" ToolTip="Click to Select Date" Width="71px" 
                        TabIndex="200"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="txt_Mem_AdmissionDate_CalendarExtender" 
                        runat="server" Enabled="True" Format="dd/MM/yyyy" 
                        TargetControlID="txt_Mem_AdmissionDate"></ajaxToolkit:CalendarExtender>
                    Closing Date*<asp:TextBox ID="txt_Mem_ClosingDate" runat="server" 
                        ForeColor="#0066FF" Height="16px" MaxLength="10" TabIndex="200" 
                        ToolTip="Click to Select Date" Width="71px"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="txt_Mem_ClosingDate_CalendarExtender" 
                        runat="server" Enabled="True" Format="dd/MM/yyyy" 
                        TargetControlID="txt_Mem_ClosingDate">
                    </ajaxToolkit:CalendarExtender>
                    Subject<asp:DropDownList ID="DDL_Subjects" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" TabIndex="200" ToolTip="Plz Select Value from Drop-Down">
                    </asp:DropDownList>
                    <ajaxToolkit:ListSearchExtender ID="ListSearchExtender2" runat="server" 
                        Enabled="True" PromptCssClass="PromptCSS" TargetControlID="DDL_Subjects">
                    </ajaxToolkit:ListSearchExtender>
                </td>               
            </tr> 
            
             <tr>
                <td class="style53"> 
                    Keywords</td>
                <td class="style54">
                     <asp:TextBox ID="txt_Mem_Keywords" runat="server" 
                        AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                        MaxLength="250" style="text-transform: uppercase" 
                        ToolTip="Enter Keyword(s) separated by semicolon (;) and space" Width="99%" 
                        Wrap="False" TabIndex="200"></asp:TextBox>             
                </td>               
            </tr> 
            
             <tr>
                <td class="style53"> 
                    Remarks</td>
                <td class="style54">
                     <asp:TextBox ID="txt_Mem_Remarks" runat="server" 
                        AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                        MaxLength="250" 
                        ToolTip="Enter Remarks if any" Width="99%" 
                        Wrap="False" TabIndex="200"></asp:TextBox>             
                </td>               
            </tr>   

            <tr>
                <td class="style53"> 
                    Status</td>
                <td class="style54">
                   
                    <asp:DropDownList ID="DDL_Status" runat="server" 
                        Font-Bold="True" ForeColor="#0066CC" TabIndex="200" 
                        ToolTip="Select Status">
                        <asp:ListItem Selected="True" Value="CU">Current</asp:ListItem>
                        <asp:ListItem Value="CN">Cancel</asp:ListItem>
                         <asp:ListItem Value="CL">Closed</asp:ListItem>
                    </asp:DropDownList>
                   
                    No Due Date<asp:TextBox ID="txt_Mem_NoDueDate" runat="server" 
                        ForeColor="#0066FF" Height="16px" MaxLength="10" TabIndex="200" 
                        ToolTip="Click to Select Date" Width="71px"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="txt_Mem_NoDueDate_CalendarExtender" 
                        runat="server" Enabled="True" Format="dd/MM/yyyy" 
                        TargetControlID="txt_Mem_NoDueDate">
                    </ajaxToolkit:CalendarExtender>
                    Date of Birth<asp:TextBox ID="txt_Mem_DoB" runat="server" ForeColor="#0066FF" 
                        Height="16px" MaxLength="10" TabIndex="200" ToolTip="Click to Select Date" 
                        Width="71px"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="txt_Mem_DoB_CalendarExtender" runat="server" 
                        Enabled="True" Format="dd/MM/yyyy" TargetControlID="txt_Mem_DoB">
                    </ajaxToolkit:CalendarExtender>
                    Send Reminder
                    <asp:DropDownList ID="DDL_Reminder" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC" TabIndex="200" ToolTip="Select Value">
                        <asp:ListItem Selected="True" Value="Y">Yes</asp:ListItem>
                        <asp:ListItem Value="N">No</asp:ListItem>
                    </asp:DropDownList>
                   
                </td>               
            </tr> 

              <tr>
                <td class="style53"> 
                    Father Name</td>
                <td class="style54">
                     <asp:TextBox ID="txt_Mem_FatherName" runat="server" AutoCompleteType="DisplayName" 
                         Font-Bold="True" ForeColor="#0066FF" Height="18px" MaxLength="50" 
                         TabIndex="200" ToolTip="Enter Father Name" Width="300px" Wrap="False"></asp:TextBox>
                     Surity Name<asp:TextBox ID="txt_Mem_SurityName" runat="server" 
                         AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                         Height="18px" MaxLength="50" TabIndex="200" ToolTip="Enter Surity Name" 
                         Width="200px" Wrap="False"></asp:TextBox>
                </td>               
            </tr>   
            <tr>
                <td class="style53"> 
                    Profession</td>
                <td class="style54">
                     <asp:TextBox ID="txt_Mem_Profession" runat="server" AutoCompleteType="DisplayName" 
                         Font-Bold="True" ForeColor="#0066FF" Height="18px" MaxLength="50" 
                         TabIndex="200" ToolTip="Enter Profession" Width="200px" Wrap="False"></asp:TextBox>
                     Qualification<asp:TextBox ID="txt_Mem_Qualification" runat="server" 
                         AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                         Height="18px" MaxLength="50" TabIndex="200" ToolTip="Enter Qualification" 
                         Width="200px" Wrap="False"></asp:TextBox>
                </td>               
            </tr>   
            <tr id ="tr1" runat="server">
                 <td class="style53"></td>
                 <td class="style54" colspan="2">
                     <asp:CheckBox ID="CheckBox2" runat="server" Text="Set Password" 
                         AutoPostBack="True" />
                     <asp:CheckBox ID="CheckBox3" runat="server" Text="Send Password in Email" />
                 </td>
             </tr>
             <tr id ="trPw1" runat="server">
                 <td class="style53">Password*</td>
                 <td class="style54" colspan="2">
                     <asp:TextBox ID="txt_UserPass" runat="server" AutoCompleteType="Disabled" 
                         Columns="15" Font-Bold="True" ForeColor="#0066FF" MaxLength="15" 
                         TextMode="Password" ToolTip="Enter Strong Password" Wrap="False" 
                         Enabled="False"></asp:TextBox>
                     <span class="style60">5-10 Chars Length, Alpha-Numeric with Spl Char, atleast 
                     one Caps Letter (Strong Pw) </span>
                     <input id="HashPass2" type="hidden" name="HashPass2" runat ="server" />
                 </td>
             </tr>
             <tr id ="trRpw1" runat="server">
                 <td class="style53">
                     Re-Password*</td>
                 <td class="style54" colspan="2">
                     <asp:TextBox ID="txt_UserRePass" runat="server" AutoCompleteType="Disabled" 
                         Columns="15" Font-Bold="True" ForeColor="#0066FF" MaxLength="15" 
                         TextMode="Password" ToolTip="Enter Password Again" Wrap="False" 
                         Enabled="False"></asp:TextBox>
                 </td>
             </tr>
        
            <tr>
                <td class="style56" colspan="2" bgcolor="#336699">
                   <asp:Label ID="Label13" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">* Mandatory Fields</asp:Label>
                </td>
            </tr>
             <tr>
                <td class="style56" colspan="2" bgcolor="#336699">
                    <asp:Button ID="Mem_Save_Bttn" runat="server" AccessKey="s" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" Height="23px" 
                        OnClientClick="return test1();" TabIndex="3" Text="Save" 
                        ToolTip="Press to SAVE Record" Visible="False" Width="74px" />
                    <asp:Button ID="Mem_Update_Bttn" runat="server" AccessKey="u" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" Height="23px" 
                        OnClientClick="return test1();" TabIndex="5" Text="Update" 
                        ToolTip="Press to UPDATE Record" Visible="False" Width="74px" />
                    <asp:Button ID="Mem_Cancel_Bttn" runat="server" AccessKey="c" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" Height="23px" 
                        TabIndex="4" Text="Cancel" ToolTip="Press to Cancel" Width="74px" />
                </td>
            </tr>
                     
            
            

             <tr>  
                <td class="style56" colspan="2" bgcolor="#336699">                
                     
                    &nbsp;</td>
            </tr>
            
                        

        </table>
                      
    </asp:View>
           
</asp:MultiView>
  
  
  
                 </ContentTemplate>  
                     <Triggers>
                        <asp:PostBackTrigger  ControlID="Menu1"   />                      
                        <asp:PostBackTrigger  ControlID="Mem_Save_Bttn"  />   
                        <asp:PostBackTrigger  ControlID="Mem_Update_Bttn"  />                                
                   </Triggers>                
                    </asp:UpdatePanel>            
                           
</div>

     
        
                   

</asp:Content>
