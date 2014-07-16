<%@ Page  Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ResetPassword.aspx.vb" Inherits="EG4.ResetPassword" %>
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
        text-align: center;
        font-size: small;
        color: #336699;
        height: 17px;
       
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
    
       .style52
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
               
                
        .style53
        {
            text-align: left;
            border-style: none;
            border-color: inherit;
            padding: 0px;
            background-color: #99CCFF;
            font-size: small;
        }
               
                
    </style>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

 <script language ="javascript" src ="../md5.js" type ="text/javascript" ></script> 

 <script language="javascript" type="text/javascript">

            var MyHashed;

            function test3() {
                var usercode = "";
                var pwd = "";
                var repwd = "";
               
                usercode = document.getElementById('<%=txt_UserCode.ClientID%>').value;
                pwd = document.getElementById('<%=txt_UserPass.ClientID%>').value;
                repwd = document.getElementById('<%=txt_UserRePass.ClientID%>').value;
               
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

                    salt = '<% = myGenS %>'
                    var hashchk = hex_md5(salt);
                    document.getElementById("MainContent_HashPasschk").value = hashchk;


                    MyHashed = hex_md5(document.getElementById('<%=txt_UserPass.ClientID%>').value);
                   document.getElementById("MainContent_HashPass2").value = MyHashed;
                    document.getElementById('<%=txt_UserPass.ClientID%>').value = "";
                    document.getElementById('<%=txt_UserRePass.ClientID%>').value = "";
                    return (true);
                }
                return (false);
            }
            function pwdStrength(password) {
                var desc = new Array();
                desc[0] = "<font color='red'>Very Weak</font>";
                desc[1] = "<font color='red'>Weak</font>";
                desc[2] = "<font color='orange'>Better</font>";
                desc[3] = "<font color='orange'>Medium</font>";
                desc[4] = "<font color='green'>Strong</font>";
                desc[5] = "<font color='green'>Strongest</font>";
                var score = 0;
                //if password bigger than 6 give 1 point
                if (password.length > 6) score++;
                //if password has both lower and uppercase characters give 1 point
                if ((password.match(/[a-z]/)) && (password.match(/[A-Z]/))) score++;
                //if password has at least one number give 1 point
                if (password.match(/\d+/)) score++;
                //if password has at least one special caracther give 1 point
                if (password.match(/.[!,@,#,$,%,^,&,*,?,_,~,-,(,)]/)) score++;
                //if password bigger than 12 give another 1 point
                if (password.length > 12) score++;
                document.getElementById("pwdDescription").innerHTML = desc[score];
                document.getElementById("pwdStrength").className = "strength" + score;
            }
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
     <%  If myGenT <> "" Then%>
                    <% myGenT = ""%>
                    <%Else%>
                    <%  Response.Redirect("..\Default.aspx")
                        %>
                     <% End If%>
    <asp:Panel ID="Panel1" runat="server" DefaultButton="Submit">

  <input id="HashPass2" type="hidden" name="HashPass2" runat ="server" /> 
    <input id="HashPasschk" type="hidden" name="HashPass2" runat ="server" /> 
        <table id="ADMT1" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
            <tr>
                <td  bgcolor="#003366" class="style43" colspan="2" rowspan="1" style="color: #FFFFFF">
                     <strong>Reset Password</strong></td>
            </tr>
            <tr>
                <td  bgcolor="#99CCFF" class="style44" colspan="2">
                     &nbsp;</td>
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
                        Height="20px" Width="96px" Enabled="False" onkeypress="return EngOnlyInput (event)"></asp:TextBox>
                &nbsp;</td>
            </tr>
            <tr>
                <td class="style47">New Password*</td>
                <td class="style46">
                    <asp:TextBox ID="txt_UserPass" runat="server" Columns="15" MaxLength="15"  
                        ToolTip="Enter Strong Password" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" TextMode="Password" 
                        AutoCompleteType="Disabled" onkeyup="pwdStrength(this.value)" ></asp:TextBox>
                &nbsp;6-10 Chars Length, Alpha-Numeric with Spl Char, atleast one Caps Letter (Strong Pw)
                 <div id="pwdDescription" class="explain" style="color: red"></div>
                </td>
            </tr>
            <tr>
                <td class="style53">Confirm New Password*</td>
                <td class="style46">
                    <asp:TextBox ID="txt_UserRePass" runat="server" Columns="15" MaxLength="15"  
                        ToolTip="Enter Password Again" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" TextMode="Password" 
                        AutoCompleteType="Disabled"></asp:TextBox>
                </td>
            </tr>
            <tr>               
                <td class="style46" colspan="2">
                   <cc1:CaptchaControl ID="CAPTCHA1" runat="server" EnableTheming="True" 
                        layoutstyle="Vertical" ShowSubmitButton="False" />
                  
                  </td>
            </tr>
            <tr>
                <td class="style52" colspan="2">
                    <asp:Button ID="Submit" runat="server" AccessKey="s" CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red" OnClientClick="return test3();" TabIndex="14" 
                        Text="Submit " />
                  
                </td>
            </tr>
             
            <tr>
                <td class="style46" colspan="2"><span class="style49">HELP</span>: You must note 
                    down your password and must keep it safely.</td>
            </tr>
            <tr>
                <td  bgcolor="#99CCFF" class="style44" colspan="2" rowspan="0">
                     <strong>*<span class="style48"> Mandatory Fields</span></strong></td>
            </tr>
        </table>

       <table id="ADMT2" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
            <tr>
                <td  bgcolor="#99CCFF" class="style43" colspan="2" rowspan="1">
                     <strong>Password Reset Sucessfully!</strong></td>
            </tr>
            
            
            <tr>
                <td class="style52" colspan="2" align="center">
                    &nbsp;</td>
            </tr>
           
            <tr>
                <td class="style47">&nbsp;</td>
                <td class="style46"><span class="style49">HELP: Keep the Account Info safely.</span></td>
            </tr>
            
        </table>
        </asp:Panel>
</asp:Content>
