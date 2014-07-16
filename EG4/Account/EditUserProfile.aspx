<%@ Page  Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="EditUserProfile.aspx.vb" Inherits="EG4.EditUserProfile" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
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
                  
        .style48
        {
            font-size: xx-small;
            color: #FF0000;
        }
                
        .styleBttn
    {
     cursor:pointer;
            margin-left: 0px;
            }
               
                
        .style51
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
        .style52
        {
            text-align: left;
            border-style: none;
            padding: 0px;
            font-weight: bold;
            background-color: #D5EAFF;
            height: 18px;
        }
               
                
        .style53
        {
            text-align: left;
            border-style: none;
            padding: 0px;
            font-weight: bold;
            background-color: #D5EAFF;
            height: 18px;
            width: 713px;
        }
               
                
        .style54
        {
            text-align: left;
            border-style: none;
            padding: 0px;
            font-weight: bold;
            background-color: #D5EAFF;
            height: 18px;
            width: 1041px;
        }
               
                
    </style>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

 <script type="text/javascript" src="http://www.google.com/jsapi"></script> 
 <script type="text/javascript">
       // Load the Google Transliteration API   

       google.load("elements", "1", {
           packages: "transliteration"
       });
       var ids = ""
       function onLoad() {
           var options = {
               sourceLanguage: 'en',
               destinationLanguage: ['hi', 'bn', 'gu', 'kn', 'ml', 'mr', 'pa', 'sa', 'ta', 'te', 'ur'],
               shortcutKey: 'ctrl+g',
               transliterationEnabled: false
           };
           // Create an instance on TransliterationControl with the required         
           // options.         
           var control = new google.elements.transliteration.TransliterationControl(options);
           ids = ["MainContent_txt_UserName", "MainContent_txt_UserDesig",  "MainContent_txt_Remarks"];
           control.makeTransliteratable(ids);
           // Show the transliteration control which can be used to toggle between         
           // English and Hindi and also choose other destination language.         
           control.showControl('translControl');
       }

       google.setOnLoadCallback(onLoad);
                   
    </script> 
     <script type="text/javascript">
         function EngOnlyInput(event) {
             var chCode = ('charCode' in event) ? event.charCode : event.keyCode;
             //if (chCode < 48 /* '0' */ || chCode > 57 /* '9' */) {
             //alert(chCode);
             if (chCode > 64 || chCode == 0 || chCode == 13 || chCode == 32) {
                 if (chCode < 91 || chCode == 0 || chCode == 13 || chCode == 32) {
                     return (true);
                 }
                 else if (chCode > 96 || chCode == 0 || chCode == 13 || chCode == 32) {
                     if (chCode < 123 || chCode == 0 || chCode == 13 || chCode == 32) {
                         return (true);
                     }
                     else {
                         alert("Please Enter ENG Only Characters!");
                         document.getElementById("MainContent_txt_UserCode").focus();
                         return (false);
                     }
                 }
                 else {
                     alert("Please Enter ENG Only Characters!");
                     document.getElementById("MainContent_txt_UserCode").focus();
                     return (false);
                 }
             }
             else {
                 alert("Please Enter ENG Only Characters!");
                 document.getElementById("MainContent_txt_UserCode").focus();
                 return (false);
             }
         }
    </script>

    <script language ="javascript" src ="../md5.js" type ="text/javascript" ></script> 
     <script language="javascript" type="text/javascript">

         var MyHashed;

         function test1() {
             var usercode = "";
             var userName = "";
             var desig = "";
             var email = "";
             var pwd = "";
             var repwd = "";          
             usercode = document.getElementById('<%=txt_UserCode.ClientID%>').value;
             userName = document.getElementById('<%=txt_UserName.ClientID%>').value;
             desig = document.getElementById('<%=txt_UserDesig.ClientID%>').value;
             email = document.getElementById('<%=txt_UserEmail.ClientID%>').value;
             pwd = document.getElementById('<%=txt_UserPass.ClientID%>').value;
             repwd = document.getElementById('<%=txt_UserRePass.ClientID%>').value;
        

             if (usercode == "") {
                 alert("Please enter proper \"user code\" field.");
                 document.getElementById("MainContent_txt_UserCode").focus();
                 return (false);
             }
             if (document.getElementById('<%=txt_UserCode.ClientID%>').value.length < 3) {
                 alert("Length of \"User Code\" should be Min 3 characters.");
                 document.getElementById("MainContent_txt_UserCode").focus();
                 return (false);
             }
             if (document.getElementById('<%=txt_UserCode.ClientID%>').value.length > 10) {
                 alert("Length of \"User Code\" should be Max 10 characters.");
                 document.getElementById("MainContent_txt_UserCode").focus();
                 return (false);
             }
             if (document.getElementById('<%=txt_UserName.ClientID%>').value == "") {
                 alert("Please enter proper \"Full Name\" field.");
                 document.getElementById("MainContent_txt_UserName").focus();
                 return (false);
             }
             if (document.getElementById('<%=txt_UserDesig.ClientID%>').value == "") {
                 alert("Please enter proper \"Designation\" field.");
                 document.getElementById("MainContent_txt_UserDesig").focus();
                 return (false);
             }
                  

             if (email == "") {
                 alert("Please enter proper \"E-Mail\" field.");
                 document.getElementById("MainContent_txt_UserEmail").focus();
                 return (false);
             }
             re = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
             if (!re.test(document.getElementById('<%=txt_UserEmail.ClientID%>').value)) {
                 alert("Error: Not a proper mail address!");
                 document.getElementById("MainContent_txt_UserEmail").focus();
                 return false;
             }

             if (document.getElementById("MainContent_CheckBox1").checked == true) {
                 
            
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
                <td  bgcolor="#003366" class="style43" rowspan="1" style="color: #FFFFFF"><strong>Edit My Profile</strong></td>
            </tr>
            <tr>
                <td  bgcolor="#99CCFF" style="text-align: center">
                     Type in Indian languages (Press Ctrl+g to toggle between English and India Language)
                     <div id='translControl'></div></td>
            </tr>
        </table>

         <table id="Table1" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
             <tr>
                <td class="style47" colspan="3">
                     <asp:Label ID="Label6" runat="server" Font-Size="Medium" ForeColor="#CC3300" 
                         style="font-weight: 700"></asp:Label>
                 </td>
            </tr>
            
            <tr>
                <td class="style51"> 
                    <asp:Label ID="lbl_UserCode" runat="server" Text="User Code *"></asp:Label>
                </td>
                <td class="style54">
                    <asp:TextBox ID="txt_UserCode" runat="server" MaxLength="10"  
                        ToolTip="Enter Distinct User Code, Max 10 Chr, Alpha, ENG Only." Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" style="text-transform: uppercase" 
                        Height="20px" Width="96px" onkeypress="return EngOnlyInput (event)" 
                        Enabled="False"></asp:TextBox>
                    &nbsp;5-10 Chrs Length, Alpha, ENG Only, Distinct User Code.
                    </td>
               
                <td rowspan="4" style="background-color:  #99CCFF" align="center" 
                    valign="middle"><asp:Image ID="Image21" runat="server" Height="100px" 
                         Width="80px" BorderStyle="None" 
                        ImageAlign="Middle"/> </td>
               
            </tr>
            <tr>
                <td class="style51">User Name*</td>
                <td class="style53">
                    <asp:TextBox ID="txt_UserName" runat="server" MaxLength="250"  
                        ToolTip="Enter User Name" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="356px"  
                        AutoCompleteType="DisplayName" Height="22px"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td class="style51">Designation*</td>
                <td class="style52">
                    <asp:TextBox ID="txt_UserDesig" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" MaxLength="150" ToolTip="Enter Designation" Width="250px" 
                        Wrap="False"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td class="style51">Phone</td>
                <td class="style52" colspan="2">
                    <asp:TextBox ID="txt_UserPhone" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" MaxLength="150" ToolTip="Enter Phone Number" Width="250px" 
                        Wrap="False"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style51">Mobile </td>
                <td class="style52" colspan="2">
                    <asp:TextBox ID="txt_UserMobile" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" MaxLength="150" ToolTip="Enter Mobile Number" Width="250px" 
                        Wrap="False"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td class="style51">Email*</td>
                <td class="style52" colspan="2">
                    <asp:TextBox ID="txt_UserEmail" runat="server" MaxLength="150"  
                        ToolTip="Enter Email" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="250px"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td class="style51">Security Question</td>
                <td class="style52" colspan="2">
                    <asp:DropDownList ID="DropDownList1" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Width="300px">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem>What is your pets name?</asp:ListItem>
                        <asp:ListItem>What is your favorite color?</asp:ListItem>
                        <asp:ListItem>What was the name of your first school?</asp:ListItem>
                        <asp:ListItem>Who was your favorite hero?</asp:ListItem>
                        <asp:ListItem>What is your grand fathers name?</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="style51">Answer</td>
                <td class="style52" colspan="2">
                    <asp:TextBox ID="txt_UserAns" runat="server" MaxLength="150"  
                        ToolTip="Enter Mobile Number" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="297px"></asp:TextBox>
                    </td>
            </tr>
              <tr id="tr_download" runat ="server">
                <td class="style51">Download Record?</td>
                <td class="style52" colspan="2">
                    &nbsp;<asp:DropDownList 
                        ID="DropDownList2" runat="server" Font-Bold="True" ForeColor="#0066FF">
                        <asp:ListItem Selected="True" Value="Y">Yes</asp:ListItem>
                        <asp:ListItem Value="N">No</asp:ListItem>
                    </asp:DropDownList>
&nbsp;Download Record from Internet based on ISBN?</td>
            </tr>
             <tr  id="tr_dispaly" runat ="server">
                <td class="style51">Display Record?</td>
                <td class="style52" colspan="2">
                    &nbsp;<asp:DropDownList 
                        ID="DropDownList3" runat="server" Font-Bold="True" ForeColor="#0066FF">
                        <asp:ListItem Selected="True" Value="Y">Yes</asp:ListItem>
                        <asp:ListItem Value="N">No</asp:ListItem>
                    </asp:DropDownList>
&nbsp;Display Record after SAVE/EDIT?</td>
            </tr>
            <tr>
                <td class="style51">Remarks</td>
                <td class="style52" colspan="2">
                    <asp:TextBox ID="txt_Remarks" runat="server" MaxLength="250"  
                        ToolTip="Enter Remarks" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="357px" Height="62px" 
                        TextMode="MultiLine"></asp:TextBox>
                    </td>
            </tr>

             <tr id ="tr1" runat="server">
                 <td class="style51"></td>
                 <td class="style52" colspan="2">
                     <asp:CheckBox ID="CheckBox1" runat="server" Text="Re-Set Super User Password" />
                 </td>
             </tr>
             <tr id ="trPw1" runat="server">
                 <td class="style51">Password*</td>
                 <td class="style52" colspan="2">
                     <asp:TextBox ID="txt_UserPass" runat="server" AutoCompleteType="Disabled" 
                         Columns="15" Font-Bold="True" ForeColor="#0066FF" MaxLength="15" 
                         TextMode="Password" ToolTip="Enter Strong Password" Wrap="False"></asp:TextBox>
                     6-10 Chars Length, Alpha-Numeric with Spl Char, atleast one Caps Letter (Strong Pw)
                     <input id="HashPass2" type="hidden" name="HashPass2" runat ="server" />
                 </td>
             </tr>
             <tr id ="trRpw1" runat="server">
                 <td class="style51">
                     Re-Password*</td>
                 <td class="style52" colspan="2">
                     <asp:TextBox ID="txt_UserRePass" runat="server" AutoCompleteType="Disabled" 
                         Columns="15" Font-Bold="True" ForeColor="#0066FF" MaxLength="15" 
                         TextMode="Password" ToolTip="Enter Password Again" Wrap="False"></asp:TextBox>
                 </td>
             </tr>
            <tr>
                <td class="style51">Select Photo</td>
                <td class="style52" colspan="2">
                    <asp:FileUpload ID="FileUpload13" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="style47" colspan="3">
                    <asp:Button ID="Cancel" runat="server"  CssClass="styleBttn" Font-Bold="True" ForeColor="Red" 
                        TabIndex="15" Text="Cancel" Width="71px" AccessKey="c"/>
                    <asp:Button ID="bttn_Update" runat="server"  CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red"
                        OnClientClick="return test1();" TabIndex="14" Text="Update" AccessKey="s" 
                        Width="74px" />
                </td>
            </tr>
            
            <tr>
                <td  bgcolor="#99CCFF" class="style44" colspan="3" rowspan="0">
                     <strong>*<span class="style48"> Mandatory Fields</span></strong></td>
            </tr>


             

        </table>

 
        
</asp:Content>
