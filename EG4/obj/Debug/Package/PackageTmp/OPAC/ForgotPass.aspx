<%@ Page  Title="Forgot Password" Language="vb" AutoEventWireup="false" MasterPageFile="~/OPAC/OPAC.Master" CodeBehind="ForgotPass.aspx.vb" Inherits="EG4.ForgotPass" %>

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
        font-size: small;
        color: #336699;
        height: 17px;
        width: 98%;
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
        text-align: left;
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
    
       .styleBttn
    {
     cursor:pointer;
    }
               
                
        #translControl
        {
            width: 830px;
        }
               
                
        .style52
    {
        text-align: center;
        border-style: none;
        padding: 0px;
        font-weight:bold;
        background-color:#D5EAFF; 
    }
                
    </style>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <script language="javascript" type="text/javascript">

        function valid1() {
         var libcode = "";
         var usercode = "";
         var SecQ = "";
         var SecAns = "";

         libcode = document.getElementById('<%=txt_LibCode.ClientID%>').value;
         usercode = document.getElementById('<%=txt_MemNo.ClientID%>').value;
         SecQ = document.getElementById('<%=DropDownList1.ClientID%>').value;
         SecAns = document.getElementById('<%=txt_UserAns.ClientID%>').value;

         if (usercode == "") {
             alert("Please enter proper \"Member No\" field.");
             document.getElementById("MainContent_txt_MemNo").focus();
             return (false);
         }
        
         if (document.getElementById('<%=txt_MemNo.ClientID%>').value.length > 15) {
             alert("Length of \" Member No\" should be Max 15 characters.");
             document.getElementById("MainContent_txt_MemNo").focus();
             return (false);
         }

         if (libcode == "") {
             alert("Please enter proper \" Library Code\" field.");
             document.getElementById("MainContent_txt_LibCode").focus();
             return (false);
         }

         if (document.getElementById('<%=txt_LibCode.ClientID%>').value.length > 10) {
             alert("Length of \" Library Code\" should be Max 10 characters.");
             document.getElementById("MainContent_txt_LibCode").focus();
             return (false);
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
         return (true);
     }
    </script>

    
      <asp:Panel ID="Panel1" runat="server" DefaultButton="Submit" >
        <table id="ADMT1" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
            <tr>
                <td  bgcolor="#003366" class="style43" colspan="2" rowspan="1" style="color: #FFFFFF">
                     <strong>Forgot Password</strong></td>
            </tr>
            <tr>
                <td class="style47">
                    <asp:Label ID="lbl_UserCode" runat="server" Text="Member No *"></asp:Label>
                 </td>
                <td class="style46">
                    <asp:TextBox ID="txt_MemNo" runat="server" MaxLength="10"  
                        ToolTip="Enter Member No" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" style="text-transform: uppercase" 
                        Height="20px" Width="96px"></asp:TextBox>
                    </td>
            </tr>

             <tr>
                <td class="style47">
                    <asp:Label ID="Label1" runat="server" Text="Library Code *"></asp:Label>
                </td>
                <td class="style46">
                    <asp:TextBox ID="txt_LibCode" runat="server" MaxLength="10"  
                        ToolTip="Enter Library Code" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" style="text-transform: uppercase" 
                        Height="20px" Width="96px" AutoCompleteType="Disabled" AUTOCOMPLETE="off"></asp:TextBox>
                    &nbsp;
                    </td>
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
&nbsp;Select the One Security Question.</td>
            </tr>
            <tr>
                <td class="style47">Answer*</td>
                <td class="style46">
                    <asp:TextBox ID="txt_UserAns" runat="server" MaxLength="150" ToolTip="Enter Mobile Number" Wrap="False" Font-Bold="True" ForeColor="#0066FF" Width="297px"></asp:TextBox>&nbsp;Enter Answer to above Question.</td>
            </tr>
            <tr>
                <td class="style47">&nbsp;</td>
                <td class="style46">&nbsp;</td>
            </tr>
            <tr>
                <td class="style52" colspan="2" align="center">
                    <asp:Button ID="Submit" runat="server"   CssClass="styleBttn" Font-Bold="True" ForeColor="Red" TabIndex="1" Text="Submit " AccessKey="s" OnClientClick="return valid1();" />
                &nbsp;<asp:Button ID="Cancel" runat="server"  CssClass="styleBttn" Font-Bold="True" ForeColor="Red" 
                        TabIndex="2" Text="Cancel" Width="71px"  AccessKey="c"/>
                </td>
            </tr>
            <tr>
                <td class="style46" colspan="2"><span class="style49">HELP</span>: Submit your 
                    correct Answer to get new password.</td>
            </tr>
            <tr>
                <td  bgcolor="#99CCFF" class="style44" colspan="2" rowspan="0" valign="middle">
                     <strong>*<span class="style48"> Mandatory Fields</span></strong></td>
            </tr>
        </table>
    </asp:Panel>
       </asp:Content>
