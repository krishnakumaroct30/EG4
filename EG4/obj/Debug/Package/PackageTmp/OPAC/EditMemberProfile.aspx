<%@ Page  Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/OPAC/OPAC.Master" CodeBehind="EditMemberProfile.aspx.vb" Inherits="EG4.EditMemberProfile" %>

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
           ids = ["MainContent_txt_Mem_Name", "MainContent_txt_Mem_Ans", "MainContent_txt_Mem_ResAdd", "MainContent_txt_Mem_Remarks"];
           control.makeTransliteratable(ids);
           // Show the transliteration control which can be used to toggle between         
           // English and Hindi and also choose other destination language.         
           control.showControl('translControl');
       }

       google.setOnLoadCallback(onLoad);
                   
    </script> 
     
    <script language ="javascript" src ="../md5.js" type ="text/javascript" ></script> 
     <script language="javascript" type="text/javascript">

           function test1() {
             var memid = "";
             var memname = "";
             var email = "";
             var sq = "";
             var sq = "";
             memid = document.getElementById('<%=txt_Mem_MemID.ClientID%>').value;
             memname = document.getElementById('<%=txt_Mem_Name.ClientID%>').value;
             email = document.getElementById('<%=txt_Mem_Email.ClientID%>').value;
             sq = document.getElementById('<%=DropDownList1.ClientID%>').value;
             sa = document.getElementById('<%=txt_Mem_Ans.ClientID%>').value;

             if (memid == "") {
                 alert("Please enter proper \"Member Id\" field.");
                 document.getElementById("MainContent_txt_Mem_Name").focus();
                 return (false);
             }

             if (document.getElementById('<%=txt_Mem_Name.ClientID%>').value == "") {
                 alert("Please enter proper \"Full Name\" field.");
                 document.getElementById("MainContent_txt_Mem_Name").focus();
                 return (false);
             }          

             if (email == "") {
                 alert("Please enter proper \"E-Mail\" field.");
                 document.getElementById("MainContent_txt_Mem_Email").focus();
                 return (false);
             }
             re = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
             if (!re.test(document.getElementById('<%=txt_Mem_Email.ClientID%>').value)) {
                 alert("Error: Not a proper mail address!");
                 document.getElementById("MainContent_txt_Mem_Email").focus();
                 return false;
             }

              if (document.getElementById('<%=DropDownList1.ClientID%>').value == "") {
                 alert("Please enter proper \" Security Question \" field.");
                 document.getElementById("MainContent_DropDownList1").focus();
                 return (false);
             } 

              if (document.getElementById('<%=txt_Mem_Ans.ClientID%>').value == "") {
                 alert("Please enter proper \" Security Answer \" field.");
                 document.getElementById("MainContent_txt_Mem_Ans").focus();
                 return (false);
             }

             return (true);
         }
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
                 document.getElementById("MainContent_txt_Mem_DoB").focus();
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
                     <asp:Label ID="Lbl_Error" runat="server" Font-Size="Medium" ForeColor="#CC3300" 
                         style="font-weight: 700"></asp:Label>
                 </td>
            </tr>
            
            <tr>
                <td class="style51"> 
                    <asp:Label ID="lbl_MemID" runat="server" Text="Member ID"></asp:Label>
                    *</td>
                <td class="style54">
                    <asp:TextBox ID="txt_Mem_MemID" runat="server" MaxLength="10" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" style="text-transform: uppercase" 
                        Height="20px" Width="96px" onkeypress="return EngOnlyInput (event)" 
                        Enabled="False"></asp:TextBox>
                    &nbsp;</td>
               
                <td rowspan="4" style="background-color:  #99CCFF" align="center" 
                    valign="middle"><asp:Image ID="Image21" runat="server" Height="100px" 
                         Width="80px" BorderStyle="None" 
                        ImageAlign="Middle"/> </td>
               
            </tr>
            <tr>
                <td class="style51">Member Name*</td>
                <td class="style53">
                    <asp:TextBox ID="txt_Mem_Name" runat="server" MaxLength="150"  
                        ToolTip="Enter Member Name" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="356px"  
                        AutoCompleteType="DisplayName" Height="22px"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td class="style51">Date of Birth</td>
                <td class="style52">
                 <asp:TextBox ID="txt_Mem_DoB" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="22px" MaxLength="10" ToolTip="Click to Select Date" 
                        Width="71px" onkeypress="return DateOnly (event)"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="txt_Mem_DoB_CalendarExtender" 
                        runat="server" Enabled="True" Format="dd/MM/yyyy" 
                        TargetControlID="txt_Mem_DoB">
                    </ajaxToolkit:CalendarExtender>
                 </td>
            </tr>
            <tr>
                <td class="style51">Phone</td>
                <td class="style52" colspan="2">
                    <asp:TextBox ID="txt_Mem_Phone" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" MaxLength="50" ToolTip="Enter Phone Number" Width="250px" 
                        Wrap="False"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style51">Mobile </td>
                <td class="style52" colspan="2">
                    <asp:TextBox ID="txt_Mem_Mobile" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" MaxLength="50" ToolTip="Enter Mobile Number" Width="250px" 
                        Wrap="False"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td class="style51">Email*</td>
                <td class="style52" colspan="2">
                    <asp:TextBox ID="txt_Mem_Email" runat="server" MaxLength="100"  
                        ToolTip="Enter Email" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="250px"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td class="style51">Security Question *</td>
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
                <td class="style51">Answer *</td>
                <td class="style52" colspan="2">
                    <asp:TextBox ID="txt_Mem_Ans" runat="server" MaxLength="150"  
                        ToolTip="Enter Mobile Number" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="297px"></asp:TextBox>
                    </td>
            </tr>
              <tr id="tr_download" runat ="server">
                <td class="style51">Subject</td>
                <td class="style52" colspan="2">
                    &nbsp;<asp:DropDownList 
                        ID="DDL_Subjects" runat="server" Font-Bold="True" ForeColor="#0066FF">
                    </asp:DropDownList>
                    &nbsp;Select Main Subject of your Interest</td>
            </tr>
             <tr  id="tr_dispaly" runat ="server">
                <td class="style51">Keywords</td>
                <td class="style52" colspan="2">
                    <asp:TextBox ID="txt_Mem_Keywords" runat="server" MaxLength="250"  
                        ToolTip="Enter Topic of Interest; separated by semicolon." Wrap="False" style="text-transform: uppercase" 
                        Font-Bold="True" ForeColor="#0066FF" Width="98%"></asp:TextBox>
                    </td>
            </tr>

            <tr>
                <td class="style51">Res.Address</td>
                <td class="style52" colspan="2">
                    <asp:TextBox ID="txt_Mem_ResAdd" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" MaxLength="250" ToolTip="Enter Residence Add" Width="357px" 
                        Wrap="False" Height="79px" TextMode="MultiLine"></asp:TextBox>
                    </td>
            </tr>

            <tr>
                <td class="style51">Remarks</td>
                <td class="style52" colspan="2">
                    <asp:TextBox ID="txt_Mem_Remarks" runat="server" MaxLength="250"  
                        ToolTip="Enter Remarks" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="357px" Height="62px" 
                        TextMode="MultiLine"></asp:TextBox>
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
