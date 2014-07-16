<%@ Page  Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Admin_CreateAccount.aspx.vb" Inherits="EG4.Admin_CreateAccount" %>
<%@ Register TagPrefix="cc1" Namespace="WebControlCaptcha" Assembly="WebControlCaptcha" %>

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
                
        .style46
    {
        text-align: left;
        border-style: none;
        padding: 0px;
        font-weight:bold;
        background-color:#D5EAFF;   
    }
                  
        .style47
    {
        text-align: justify;
        border-style: none;
        border-color: inherit;
        width: 170px;
        padding: 0px;
        background-color:#99CCFF;
        font-size:small;
    }
                  
        .style48
        {
            font-size: xx-small;
            color: #FF0000;
        }
                
        .style49
    {
        text-decoration: underline;
    }
    
       .style50
    {
        text-align: center;
        border-style: none;
        padding: 0px;
        font-weight:bold;
        background-color:#D5EAFF; 
    }
    
    .styleBttn
    {
     cursor:pointer;
    }
               
                
        .style51
        {
            font-weight: normal;
            font-size: xx-small;
        }
        .style52
        {
            font-size: xx-small;
        }
               
                
    </style>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

 <script language ="javascript" src ="../md5.js" type ="text/javascript" ></script> 

 <script type="text/javascript" src="http://www.google.com/jsapi"></script> 
 <script language ="javascript" type="text/javascript">
        // Load the Google Transliteration API   
        if(<% =(Not Page.isPostBack).ToString().ToLower() %>)
        {
        //alert('PAGE NOT POST BACK');

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
            ids = ["MainContent_txt_UserName", "MainContent_txt_UserDesig"];
                control.makeTransliteratable(ids);
                // Show the transliteration control which can be used to toggle between         
                // English and Hindi and also choose other destination language.         
                control.showControl('translControl');
            }
            
            google.setOnLoadCallback(onLoad);
                   

        }     
        else
        {
         //alert('PAGE POST BACK');
        }           
           
    </script> 
    
 <script language="javascript" type="text/javascript">

            var MyHashed;

            function test1() {
                var usercode = "";
                var userName = "";
                var desig = "";
                var email = "";
                var pwd = "";
                var repwd = "";
                var SecQ = "";
                var SecAns = "";

                usercode = document.getElementById('<%=txt_UserCode.ClientID%>').value;
                userName = document.getElementById('<%=txt_UserName.ClientID%>').value;
                desig = document.getElementById('<%=txt_UserDesig.ClientID%>').value;
                email = document.getElementById('<%=txt_UserEmail.ClientID%>').value;
                pwd = document.getElementById('<%=txt_UserPass.ClientID%>').value;
                repwd = document.getElementById('<%=txt_UserRePass.ClientID%>').value;
                SecQ = document.getElementById('<%=DropDownList1.ClientID%>').value;
                SecAns = document.getElementById('<%=txt_UserAns.ClientID%>').value;

                if (usercode == "") {
                    alert("Please enter proper \"user code\" field.");
                    document.getElementById("MainContent_txt_UserCode").focus();
                    return (false);
                }
                if (document.getElementById('<%=txt_UserCode.ClientID%>').value.length < 5) {
                    alert("Length of \"User Code\" should be Min 5 characters.");
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


                if (SecQ == "") {
                    alert("Please select  \"Security Question\" field.");
                    document.getElementById("MainContent_DropDownList1").focus();
                    return (false);
                }

                if (SecAns == "") {
                    alert("Please Enter  \"Security Answer\" field.");
                    document.getElementById("MainContent_txt_UserAns").focus();
                    return (false);
                }

                if (pwd == "") {
                    alert("Please enter proper \"Password\" field.");
                    document.getElementById("MainContent_txt_UserPass").focus();
                    return (false);
                }
                if (document.getElementById('<%=txt_UserPass.ClientID%>').value.length < 6) {
                    alert("Length of \"Password\" should be Min 6 characters.");
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
                if (document.getElementById('<%=txt_UserRePass.ClientID%>').value.length < 6) {
                    alert("Length of \"Re-Password\" should be Min 6 characters.");
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
    </script>
    
     <script type="text/javascript">
         function EngOnlyInput(event) {
             var chCode = ('charCode' in event) ? event.charCode : event.keyCode;
             //if (chCode < 48 /* '0' */ || chCode > 57 /* '9' */) {
             //alert(chCode);
             if (chCode > 64 || chCode == 0 || chCode == 13 || chCode == 32)
              {
                  if (chCode < 91 || chCode == 0 || chCode == 13 || chCode == 32)
                    {
                     return (true);
                    }
                 else if (chCode > 96 || chCode == 0 || chCode == 13 || chCode == 32) 
                    {
                        if (chCode < 123 || chCode == 0 || chCode == 13 || chCode == 32)
                      {
                          return (true);
                     }
                      else 
                     {
                         alert("Please Enter ENG Only Characters!");
                         document.getElementById("MainContent_txt_UserCode").focus();
                         return (false);
                     }
                 }
                 else 
                 {
                     alert("Please Enter ENG Only Characters!");
                     document.getElementById("MainContent_txt_UserCode").focus();
                     return (false);
                 }             
             }
            else
            {
                alert("Please Enter ENG Only Characters!");
                document.getElementById("MainContent_txt_UserCode").focus();
                return (false);
            }
        }
    </script>
  
  



        <table id="ADMT1" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
            <tr>
                <td  bgcolor="#003366" class="style43" colspan="2" rowspan="1" style="color: #FFFFFF">
                     <strong>Create Admin Account</strong></td>
            </tr>
            <tr>
                <td  bgcolor="#99CCFF"  colspan="2" style="text-align: center">
                     Type in Indian languages (Press Ctrl+g to toggle between English and India Language)
                     <div id='translControl'></div></td>
            </tr>
            <tr>
                <td class="style47"> 
                    <asp:Label ID="lbl_UserCode" runat="server" Text=" User Code *"   
                        AssociatedControlID="txt_UserCode" AccessKey="U"></asp:Label>
                    </td>
                <td class="style46">
                    <asp:TextBox ID="txt_UserCode" runat="server" MaxLength="10"  
                        ToolTip="Enter Distinct User Code, Max 10 Chr, Alpha, ENG Only." Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" style="text-transform: uppercase" 
                        Height="20px" Width="96px" onkeypress="return EngOnlyInput (event)"></asp:TextBox>
                &nbsp;5-10 Chrs Length, Alpha, ENG Only.
                    </td>
            </tr>
            <tr>
                <td class="style47">Full Name*</td>
                <td class="style46">
                    <asp:TextBox ID="txt_UserName" runat="server" MaxLength="150"  
                        ToolTip="Enter Full Name" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="250px"  
                        AutoCompleteType="DisplayName"></asp:TextBox>
                &nbsp;</td>
            </tr>
            <tr>
                <td class="style47">Designation*</td>
                <td class="style46">
                    <asp:TextBox ID="txt_UserDesig" runat="server" MaxLength="150"  
                        ToolTip="Enter Designation" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="250px"></asp:TextBox>
                &nbsp;</td>
            </tr>
            <tr>
                <td class="style47">Phone</td>
                <td class="style46">
                    <asp:TextBox ID="txt_UserPhone" runat="server" MaxLength="150"  
                        ToolTip="Enter Phone Number" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="250px"></asp:TextBox>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style47">Mobile</td>
                <td class="style46">
                    <asp:TextBox ID="txt_UserMobile" runat="server" MaxLength="150"  
                        ToolTip="Enter Mobile Number" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="250px"></asp:TextBox>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style47">EMail*</td>
                <td class="style46">
                    <asp:TextBox ID="txt_UserEmail" runat="server" MaxLength="150"  
                        ToolTip="Enter Email" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="250px"></asp:TextBox>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style47">Security Question*</td>
                <td class="style46">
                    <asp:DropDownList ID="DropDownList1" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Width="300px">
                        <asp:ListItem>What is your pets name?</asp:ListItem>
                        <asp:ListItem>What is your favorite color?</asp:ListItem>
                        <asp:ListItem>What was the name of your first school?</asp:ListItem>
                        <asp:ListItem>Who was your favorite hero?</asp:ListItem>
                        <asp:ListItem>What is your grand fathers name?</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="style47">Answer*</td>
                <td class="style46">
                    <asp:TextBox ID="txt_UserAns" runat="server" MaxLength="150"  
                        ToolTip="Enter Mobile Number" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="297px"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td class="style47">Password*</td>
                <td class="style46">
                    <asp:TextBox ID="txt_UserPass" runat="server" Columns="15" MaxLength="15"  
                        ToolTip="Enter Strong Password" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" TextMode="Password" 
                        AutoCompleteType="Disabled"></asp:TextBox>
                &nbsp;<span class="style51">6-10 Chars Length, Alpha-Numeric with Spl Char, atleast one Caps Letter</span><input 
                        id="HashPass2" type="hidden" name="HashPass2" runat ="server" class="style52" /></td>
            </tr>
            <tr>
                <td class="style47">Re-Password*</td>
                <td class="style46">
                    <asp:TextBox ID="txt_UserRePass" runat="server" Columns="15" MaxLength="15"  
                        ToolTip="Enter Password Again" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" TextMode="Password" 
                        AutoCompleteType="Disabled"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style47">&nbsp;</td>
                <td class="style46">
                    <cc1:CaptchaControl ID="CAPTCHA1" runat="server" EnableTheming="True" 
                        layoutstyle="Vertical" ShowSubmitButton="False" />
                </td>
            </tr>
            <tr>
                <td class="style47">&nbsp;</td>
                <td class="style50" align="center">
                    <asp:Button ID="Submit" runat="server"  CssClass="styleBttn" Font-Bold="True" ForeColor="Red"
                        OnClientClick="return test1();" TabIndex="14" Text="Save" AccessKey="s" />
                    <asp:Button ID="Cancel" runat="server"  CssClass="styleBttn" Font-Bold="True" ForeColor="Red" 
                        TabIndex="15" Text="Cancel" Width="71px" AccessKey="c"/>
                </td>
            </tr>
            <tr>
                <td class="style47">&nbsp;</td>
                <td class="style46">&nbsp;</td>
            </tr>
            <tr>
                <td class="style47">&nbsp;</td>
                <td class="style46"><span class="style49">HELP</span>: Admin Account is required to 
                    administer the database, to create Library Account and to create Library Super 
                    User Account.</td>
            </tr>
            <tr>
                <td  bgcolor="#99CCFF" class="style44" colspan="2" rowspan="0">
                     <strong>*<span class="style48"> Mandatory Fields</span></strong></td>
            </tr>
        </table>

       <table id="ADMT2" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
            <tr>
                <td  bgcolor="#99CCFF" class="style43" colspan="2" rowspan="1">
                     <strong>Admin Account created Sucessfully!</strong></td>
            </tr>
            <tr>
                <td class="style47">
                    <asp:Label ID="Label1" runat="server" Text=" User Code "></asp:Label>
                    </td>
                <td class="style46">
                    <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style47">Full Name</td>
                <td class="style46">
                    <asp:Label ID="Label3" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style47">Designation</td>
                <td class="style46">
                    <asp:Label ID="Label4" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style47">Phone</td>
                <td class="style46">
                    <asp:Label ID="Label5" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style47">Mobile</td>
                <td class="style46">
                    <asp:Label ID="Label6" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style47">EMail</td>
                <td class="style46">
                    <asp:Label ID="Label7" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            
            
            <tr>
                <td class="style47">&nbsp;</td>
                <td class="style50" align="center">
                    <asp:Button ID="Close_Bttn" runat="server"  CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red" 
                        TabIndex="15" Text="Close" Width="71px" />
                </td>
            </tr>
           
            <tr>
                <td class="style47">&nbsp;</td>
                <td class="style46"><span class="style49">HELP: Keep the Account Info safely.</span></td>
            </tr>
            
        </table>



   



       
</asp:Content>
