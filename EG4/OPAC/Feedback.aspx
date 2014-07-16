<%@ Page  Title="Feedback" Language="vb" AutoEventWireup="false" MasterPageFile="~/OPAC/OPAC.Master" CodeBehind="Feedback.aspx.vb" Inherits="EG4.Feedback" %>
    <%@ Register TagPrefix="cc1" Namespace="WebControlCaptcha" Assembly="WebControlCaptcha" %>
    

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
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
            margin:    10px auto 0px 25px;
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
                         
        .style36
        {
            font-family: Arial;
            width: 476px;
            color:  Blue;
        }
        
                
        .style42
        {
            font-size: small;
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
            width: 100%;
            border-width: 0px;
            
        }
        
                
        .style46
    {
        text-align: center;
        border-style: none;
        padding: 0px;
        font-weight:bold;
        background-color:#D5EAFF;
        width:85%;   
    }
                  
        .style47
    {
        text-align: left;
        border-style: none;
        border-color: inherit;
        padding: 0px;
        background-color:#99CCFF;
        font-size:small;
         width:15%; 
    }
                
                
        .stule46
        {
            text-align: left;
        }
        
                
        .style48
        {
            color: #0033CC;
        }
        
                
        </style>
    </asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">  
        
      <table  cellspacing="2" border="1"  cellpadding="1" class="style35">
          <tr>
                <td  bgcolor="#003366" class="style43" colspan="2" rowspan="1" style="color: #FFFFFF">
                     <strong>Feedback</strong></td>
            </tr>
            <tr>
                <td  class="style47"> Name *</td>
                <td  class="style46">
                    <asp:TextBox ID="TxtName" runat="server" Width="99%" Font-Bold="True" 
                        Height="18px" MaxLength="200"  AutoCompleteType="Disabled" 
                        ForeColor="#0066FF"></asp:TextBox>
                 </td>
            </tr>
            <tr>
                <td  class="style47">Designation</td>
                <td class="style46">
                    <asp:TextBox ID="TxtDesig" runat="server" Width="99%" Font-Bold="True" 
                         MaxLength="150" AutoCompleteType="Disabled" ForeColor="#0066FF"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="style47">Organization *</td>
                <td class="style46">
                <asp:TextBox ID="TxtOrg" runat="server" Width="99%" Font-Bold="True" 
                         MaxLength="250" CausesValidation="True" 
                        AutoCompleteType="Disabled" ForeColor="#0066FF"></asp:TextBox></td>
            </tr>
             <tr>
                <td class="style47"> Address(O)</td>
                <td class="style46">
                    <asp:TextBox ID="TxtOfficeAdd" runat="server" TextMode="MultiLine" Width="99%" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="250" CausesValidation="True" 
                        AutoCompleteType="Disabled"></asp:TextBox></td>
            </tr> 
            <tr>
                <td class="style47">Address(R)</td>
                <td class="style46">
                    <asp:TextBox ID="TxtResidentialAdd" runat="server" TextMode="MultiLine" Width="99%" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="250" CausesValidation="True" 
                        AutoCompleteType="Disabled"></asp:TextBox></td>
            </tr> 
            <tr>
                <td class="style47"> Phoe(O)</td>
                <td class="style46">
                    <asp:TextBox ID="TxtOfficePhone" runat="server" Font-Bold="True" ForeColor="#0066FF" 
                        Width="99%" MaxLength="150" CausesValidation="True" 
                        AutoCompleteType="Disabled"></asp:TextBox></td>
            </tr> 
            <tr>
                <td class="style47">Phoe(R)</td>
                <td class="style46">
                    <asp:TextBox ID="TxtResidentialPhone" runat="server" Font-Bold="True" ForeColor="#0066FF" 
                        Width="99%" MaxLength="150" CausesValidation="True" 
                        AutoCompleteType="Disabled"></asp:TextBox></td>
            </tr> 
            <tr>
                <td class="style47">Moble No.</td>
                <td class="style46">
                    <asp:TextBox ID="TxtMobileNo" runat="server" Font-Bold="True" ForeColor="#0066FF" 
                        Width="99%" MaxLength="150" CausesValidation="True" 
                        AutoCompleteType="Disabled"></asp:TextBox></td>
            </tr> 
            <tr>
                <td class="style47">EMail *</td>
                <td class="style46">
                <asp:TextBox ID="TxtEmailID" runat="server" Font-Bold="True" ForeColor="#0066FF" 
                        Width="99%" MaxLength="250" CausesValidation="True" 
                        AutoCompleteType="Disabled"></asp:TextBox></td>
            </tr> 
            <tr>
                <td class="style47">Comments *</td>
                <td class="style46">
                    <asp:TextBox ID="TxtComments" runat="server" TextMode="MultiLine" 
                        Font-Bold="True" ForeColor="#0066FF" Width="99%" 
                        Height="140px" MaxLength="1000" CausesValidation="True" 
                        AutoCompleteType="Disabled"></asp:TextBox></td>
            </tr> 
          <tr>
              <td class="style47"></td>
              <td class="style46">
                 
                  <cc1:captchacontrol id="CAPTCHA" runat="server" layoutstyle="Vertical" showsubmitbutton="False"
                      tabindex="4" ForeColor="#0066FF"></cc1:captchacontrol>
              </td>
          </tr>
            <tr>
            <td class="style47"></td>
                <td class="style46">
                    <asp:Button ForeColor="Red" ID="Submit" runat="server" Text="Submit " Font-Bold="True" />
                    <asp:Button ForeColor="Red" ID="Reset" runat="server" Text="Reset" Width="71px" Font-Bold="True" />
                </td>
            </tr>
          <tr>
          <td class="style47"></td>
              <td class="style46">
                  <span style="font-weight: bold;" class="style48">* Mandatory Fields</span>
              </td>
          </tr>
          
        </table>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TxtName"
            Display="None" ErrorMessage="Name Should Not be Empty" SetFocusOnError="True"></asp:RequiredFieldValidator>&nbsp;
        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TxtOrg"
            Display="None" ErrorMessage="Organization Should Not be Empty" SetFocusOnError="True"></asp:RequiredFieldValidator>
       
        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="TxtEmailID"
            Display="None" ErrorMessage="EmailID Should Not be Empty"></asp:RequiredFieldValidator>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="TxtComments"
            Display="None" ErrorMessage="Comments Should Not be Empty" Width="148px"></asp:RequiredFieldValidator>
        <br />
       
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TxtEmailID"
            Display="None" ErrorMessage="E-Mail Must be Required Right ID" SetFocusOnError="True"
            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
        <br />
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
            ShowSummary="False" />
        <br />
           

            
       

        
        
        
                
        
    
    
    
    
    
    
    
    
    
    
    
    
</asp:Content>