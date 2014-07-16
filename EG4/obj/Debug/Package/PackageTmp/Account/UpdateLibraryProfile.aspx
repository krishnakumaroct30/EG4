<%@ Page  Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="UpdateLibraryProfile.aspx.vb" Inherits="EG4.UpdateLibraryProfile" %>

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
            width: 252px;
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
               
                
        .style55
        {
            text-align: justify;
            border-style: none;
            border-color: inherit;
            width: 252px;
            padding: 0px;
            background-color: #99CCFF;
            font-size: small;
        }
        .style56
        {
            text-align: left;
            border-style: none;
            padding: 0px;
            font-weight: bold;
            background-color: #D5EAFF;
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
           ids = ["MainContent_txt_Lib_Name2", "MainContent_txt_Lib_Parent2", "MainContent_txt_Lib_Add2","MainContent_txt_Lib_City2", "MainContent_txt_Lib_District2", "MainContent_txt_Lib_State2", "MainContent_txt_Lib_Incharge2", "MainContent_txt_Lib_Remarks", "MainContent_txt_Lib_Intro2", "MainContent_txt_Lib_Objective2", "MainContent_txt_Lib_History2", "MainContent_txt_Lib_Services2"];
           control.makeTransliteratable(ids);
           // Show the transliteration control which can be used to toggle between         
           // English and Hindi and also choose other destination language.         
           control.showControl('translControl');
       }

       google.setOnLoadCallback(onLoad);
                   
    </script> 

    <script language ="javascript" src ="../md5.js" type ="text/javascript" ></script> 
     <script language="javascript" type="text/javascript">

         var MyHashed;

         function test1() {
             var LibCode = "";
             var email = "";
             var pwd = "";
             var repwd = "";
             LibCode = document.getElementById('<%=txt_Lib_Code.ClientID%>').value;
             email = document.getElementById('<%=txt_Lib_Email.ClientID%>').value;


             if (LibCode == "") {
                 alert("Please enter proper \"Library Code\" field.");
                 document.getElementById("MainContent_txt_Lib_Name").focus();
                 return (false);
             }
             if (document.getElementById('<%=txt_Lib_Code.ClientID%>').value.length < 5) {
                 alert("Length of \"Library Code\" should be Min 3 characters.");
                 document.getElementById("MainContent_txt_UserCode").focus();
                 return (false);
             }
             if (document.getElementById('<%=txt_Lib_Code.ClientID%>').value.length > 10) {
                 alert("Length of \"Library Code\" should be Max 10 characters.");
                 document.getElementById("MainContent_txt_UserCode").focus();
                 return (false);
             }
             if (document.getElementById('<%=txt_Lib_Name.ClientID%>').value == "") {
                 alert("Please enter proper \"Library Name\" field.");
                 document.getElementById("MainContent_txt_Lib_Name").focus();
                 return (false);
             }
             if (document.getElementById('<%=txt_Lib_Parent.ClientID%>').value == "") {
                 alert("Please enter proper \"Organization\" field.");
                 document.getElementById("MainContent_txt_Lib_Parent").focus();
                 return (false);
             }
             if (document.getElementById('<%=txt_Lib_Add.ClientID%>').value == "") {
                 alert("Please enter proper \"City\" field.");
                 document.getElementById("MainContent_txt_Lib_Add").focus();
                 return (false);
             }
             if (document.getElementById('<%=txt_Lib_City.ClientID%>').value == "") {
                 alert("Please enter proper \"City\" field.");
                 document.getElementById("MainContent_txt_Lib_City").focus();
                 return (false);
             }
             if (document.getElementById('<%=txt_Lib_State.ClientID%>').value == "") {
                 alert("Please enter proper \" State\" field.");
                 document.getElementById("MainContent_txt_Lib_State").focus();
                 return (false);
             }
                  

             if (email == "") {
                 alert("Please enter proper \"E-Mail\" field.");
                 document.getElementById("MainContent_txt_Lib_Email").focus();
                 return (false);
             }
             re = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
             if (!re.test(document.getElementById('<%=txt_Lib_Email.ClientID%>').value)) {
                 alert("Error: Not a proper mail address!");
                 document.getElementById("MainContent_txt_Lib_Email").focus();
                 return false;
             }
        
        return (true);
     }
         
    </script>
    
         <table id="ADMT1" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
            <tr>
                <td  bgcolor="#003366" class="style43" rowspan="1" style="color: #FFFFFF"><strong>Update Library Profile</strong></td>
            </tr>
            <tr>
                <td  bgcolor="#99CCFF" style="text-align: center">
                     Type in Indian languages (Press Ctrl+g to toggle between English and India Language)
                     <div id='translControl'></div></td>
            </tr>
        </table>

         <table id="Table1" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
             <tr>
                <td class="style47" colspan="2">
                     <asp:Label ID="Label6" runat="server" Font-Size="Medium" ForeColor="#CC3300" 
                         style="font-weight: 700"></asp:Label>
                 </td>
            </tr>
            
            <tr>
                <td class="style51"> 
                    <asp:Label ID="lbl_LibCode" runat="server" Text="Libraryr Code*"></asp:Label>
                </td>
                <td class="style54">
                    <asp:TextBox ID="txt_Lib_Code" runat="server" MaxLength="10"  
                        ToolTip="Enter Distinct Library Code, Max 10 Chr, Alpha, ENG Only." Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" style="text-transform: uppercase" 
                        Height="16px" Width="11%" onkeypress="return EngOnlyInput (event)" 
                        Enabled="False"></asp:TextBox>
                    &nbsp;5-10 Chrs Length, Alpha, ENG Only, Distinct User Code.
                    </td>
               
            </tr>
            <tr>
                <td class="style51">Library Name*</td>
                <td class="style53">
                    <asp:TextBox ID="txt_Lib_Name" runat="server" MaxLength="255"  
                        ToolTip="Enter Library Name" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="98%"  
                        AutoCompleteType="DisplayName"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td class="style51">Library Name(L)</td>
                <td class="style52">
                    <asp:TextBox ID="txt_Lib_Name2" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" MaxLength="255" 
                        ToolTip="Enter Library Name in Local Language" Width="98%" 
                        Wrap="False"></asp:TextBox>
                    </td>
            </tr>
             <tr>
                <td class="style51">Organization*</td>
                <td class="style52">
                    <asp:TextBox ID="txt_Lib_Parent" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" MaxLength="400" ToolTip="Enter Organization Name" Width="98%" 
                        Wrap="False"></asp:TextBox>
                    </td>
            </tr>
             <tr>
                <td class="style51">Organization(L)</td>
                <td class="style52">
                    <asp:TextBox ID="txt_Lib_Parent2" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" MaxLength="400" 
                        ToolTip="Enter Organization Name in Local Language" Width="98%" 
                        Wrap="False"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td class="style55">Library Address*</td>
                <td class="style56">
                    <asp:TextBox ID="txt_Lib_Add" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" MaxLength="255" ToolTip="Enter Library Address" 
                        Width="50%" Height="90px" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style51">Library Address(L)</td>
                <td class="style52">
                    <asp:TextBox ID="txt_Lib_Add2" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" MaxLength="255" 
                        ToolTip="Enter Library Address in Local Language" Width="50%" 
                        Height="101px" TextMode="MultiLine"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td class="style51">City*</td>
                <td class="style52">
                    <asp:TextBox ID="txt_Lib_City" runat="server" MaxLength="100"  
                        ToolTip="Enter City" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="50%"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td class="style51">City (L)</td>
                <td class="style52">
                    <asp:TextBox ID="txt_Lib_City2" runat="server" MaxLength="100"  
                        ToolTip="Enter Library City in  Local Language" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="50%"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td class="style51">District</td>
                <td class="style52">
                    <asp:TextBox ID="txt_Lib_District" runat="server" MaxLength="100"  
                        ToolTip="Enter District" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="50%"></asp:TextBox>
                    &nbsp;</td>
            </tr>
             <tr>
                <td class="style51">District (L)</td>
                <td class="style52">
                    <asp:TextBox ID="txt_Lib_District2" runat="server" MaxLength="100"  
                        ToolTip="Enter District in Local Language" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="50%"></asp:TextBox>
                    &nbsp;</td>
            </tr>
             <tr>
                <td class="style51">State*</td>
                <td class="style52">
                    <asp:TextBox ID="txt_Lib_State" runat="server" MaxLength="100"  
                        ToolTip="Enter State Name" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="50%"></asp:TextBox>
                    &nbsp;</td>
            </tr>
             <tr>
                <td class="style51">State (L)</td>
                <td class="style52">
                    <asp:TextBox ID="txt_Lib_State2" runat="server" MaxLength="100"  
                        ToolTip="Enter State Name in Local Language" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="50%"></asp:TextBox>
                    &nbsp;</td>
            </tr>
             <tr>
                <td class="style51">Phone</td>
                <td class="style52">
                    <asp:TextBox ID="txt_Lib_Phone" runat="server" MaxLength="100"  
                        ToolTip="Enter Phone Number" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="50%"></asp:TextBox>
                    &nbsp;</td>
            </tr>
             <tr>
                <td class="style51">Fax</td>
                <td class="style52">
                    <asp:TextBox ID="txt_Lib_Fax" runat="server" MaxLength="100"  
                        ToolTip="Enter FAX Number" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="50%"></asp:TextBox>
                    &nbsp;</td>
            </tr>
             <tr>
                <td class="style51">Email*</td>
                <td class="style52">
                    <asp:TextBox ID="txt_Lib_Email" runat="server" MaxLength="200"  
                        ToolTip="Enter Email" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="50%"></asp:TextBox>
                    &nbsp;</td>
            </tr>
             <tr>
                <td class="style51">Library URL</td>
                <td class="style52">
                    <asp:TextBox ID="txt_Lib_URL" runat="server" MaxLength="255"  
                        ToolTip="Enter Library Web Site URL" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="98%"></asp:TextBox>
                    &nbsp;</td>
            </tr>
             <tr>
                <td class="style51">Organization URL</td>
                <td class="style52">
                    <asp:TextBox ID="txt_Lib_ParentURL" runat="server" MaxLength="255"  
                        ToolTip="Enter Organization URL" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="98%"></asp:TextBox>
                    &nbsp;</td>
            </tr>
             <tr>
                <td class="style51">Library In-Charge</td>
                <td class="style52">
                    <asp:TextBox ID="txt_Lib_Incharge" runat="server" MaxLength="250"  
                        ToolTip="Enter Library In-Charge Name" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="50%"></asp:TextBox>
                    &nbsp;</td>
            </tr>
             <tr>
                <td class="style51">Library In-Charge(L)</td>
                <td class="style52">
                    <asp:TextBox ID="txt_Lib_Incharge2" runat="server" MaxLength="250"  
                        ToolTip="Enter Library In-Charge in Local Language" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="50%"></asp:TextBox>
                    &nbsp;</td>
            </tr>
             <tr>
                <td class="style51">Library Timing</td>
                <td class="style52">
                    <asp:TextBox ID="txt_Lib_Timing" runat="server" MaxLength="250"  
                        ToolTip="Enter Library Timing" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="50%"></asp:TextBox>
                    &nbsp;</td>
            </tr>
             <tr>
                <td class="style51">SMS UID</td>
                <td class="style52">
                    <asp:TextBox ID="txt_Lib_SMSuid" runat="server" MaxLength="150"  
                        ToolTip="Enter SMS UID" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="50%"></asp:TextBox>
                    &nbsp;</td>
            </tr>
             <tr>
                <td class="style51">SMS Password</td>
                <td class="style52">
                    <asp:TextBox ID="txt_Lib_SMSpw" runat="server" MaxLength="150"  
                        ToolTip="Enter SMS Password" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="50%" TextMode="Password"></asp:TextBox>
                    &nbsp;</td>
            </tr>
             <tr>
                <td class="style51">SMS Sender</td>
                <td class="style52">
                    <asp:TextBox ID="txt_Lib_SMSsender" runat="server" MaxLength="150"  
                        ToolTip="Enter SMS Sender name/code" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="50%"></asp:TextBox>
                    &nbsp;</td>
            </tr>
             <tr>
                <td class="style51">SMS IP Address</td>
                <td class="style52">
                    <asp:TextBox ID="txt_Lib_SMSip" runat="server" MaxLength="150"  
                        ToolTip="Enter SMS Server IP" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="50%"></asp:TextBox>
                    &nbsp;</td>
            </tr>
             <tr>
                <td class="style51">Barcode Printer Model</td>
                <td class="style52">
                    <asp:TextBox ID="txt_Lib_Barcode" runat="server" MaxLength="150"  
                        ToolTip="Enter BarCode Printer Model/Make" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="50%"></asp:TextBox>
                    &nbsp;</td>
            </tr>
             <tr id ="tr1" runat="server">
                 <td class="style51">Classification Scheme</td>
                 <td class="style52">
                     <asp:DropDownList ID="DropDownList1" runat="server" 
                         ToolTip="Select Classification Scheme being followed" Font-Bold="True">
                         <asp:ListItem>DDC</asp:ListItem>
                         <asp:ListItem>UDC</asp:ListItem>
                         <asp:ListItem>LC</asp:ListItem>
                         <asp:ListItem>Others</asp:ListItem>
                     </asp:DropDownList>
                 </td>
             </tr>
              <tr id ="tr2" runat="server">
                 <td class="style51">Cataloging Code</td>
                 <td class="style52">
                     <asp:DropDownList ID="DropDownList2" runat="server" 
                         ToolTip="Enter Catalog Code is in Use" Font-Bold="True">
                         <asp:ListItem>AACR2</asp:ListItem>
                         <asp:ListItem>CCC</asp:ListItem>
                         <asp:ListItem>Other</asp:ListItem>
                     </asp:DropDownList>
                 </td>
             </tr>
              <tr id ="tr9" runat="server">
                 <td class="style51">Purchasing Data</td>
                 <td class="style52">
                     <asp:DropDownList ID="DDL_Acq" runat="server" 
                         ToolTip="Do You Wish to Include Cost in Retro-Conversion?" 
                         Font-Bold="True">
                         <asp:ListItem Selected="True">Y</asp:ListItem>
                         <asp:ListItem>N</asp:ListItem>
                     </asp:DropDownList>
                     Do You wish to Include Purchasing Data (Cost) in Retro-Conversion?</td>
             </tr>
            <tr>
                <td class="style51">Remarks</td>
                <td class="style52">
                    <asp:TextBox ID="txt_Lib_Remarks" runat="server" MaxLength="8000"  
                        ToolTip="Enter Remarks" 
                        Font-Bold="True" ForeColor="#0066FF" Width="98%" Height="62px" 
                        TextMode="MultiLine"></asp:TextBox>
                    </td>
            </tr>

            
             <tr id ="trPw1" runat="server">
                 <td class="style51">Library Introduction</td>
                 <td class="style52">
                    <asp:TextBox ID="txt_Lib_Intro" runat="server" MaxLength="8000"  
                        ToolTip="Enter Library Introduction" 
                        Font-Bold="True" ForeColor="#0066FF" Width="98%" Height="62px" 
                        TextMode="MultiLine"></asp:TextBox>
                 </td>
             </tr>
             <tr id ="trRpw1" runat="server">
                 <td class="style51">
                     Library Introduction (L)</td>
                 <td class="style52">
                    <asp:TextBox ID="txt_Lib_Intro2" runat="server" MaxLength="8000"  
                        ToolTip="Enter Introduction in Local Language" 
                        Font-Bold="True" ForeColor="#0066FF" Width="98%" Height="62px" 
                        TextMode="MultiLine"></asp:TextBox>
                 </td>
             </tr>
             <tr id ="tr3" runat="server">
                 <td class="style51">Library Objectives</td>
                 <td class="style52">
                    <asp:TextBox ID="txt_Lib_Objective" runat="server" MaxLength="8000"  
                        ToolTip="Enter Library Objectives" 
                        Font-Bold="True" ForeColor="#0066FF" Width="98%" Height="62px" 
                        TextMode="MultiLine"></asp:TextBox>
                 </td>
             </tr>
             <tr id ="tr4" runat="server">
                 <td class="style51">
                     Library Objectives (L)</td>
                 <td class="style52">
                    <asp:TextBox ID="txt_Lib_Objective2" runat="server" MaxLength="8000"  
                        ToolTip="Enter Objectives in Local Language" 
                        Font-Bold="True" ForeColor="#0066FF" Width="98%" Height="62px" 
                        TextMode="MultiLine"></asp:TextBox>
                 </td>
             </tr>
             <tr id ="tr5" runat="server">
                 <td class="style51">Library History</td>
                 <td class="style52">
                    <asp:TextBox ID="txt_Lib_History" runat="server" MaxLength="8000"  
                        ToolTip="Enter Library History" 
                        Font-Bold="True" ForeColor="#0066FF" Width="98%" Height="62px" 
                        TextMode="MultiLine"></asp:TextBox>
                 </td>
             </tr>
             <tr id ="tr6" runat="server">
                 <td class="style51">
                     Library History (L)</td>
                 <td class="style52">
                    <asp:TextBox ID="txt_Lib_History2" runat="server" MaxLength="8000"  
                        ToolTip="Enter History in Local Language" 
                        Font-Bold="True" ForeColor="#0066FF" Width="98%" Height="62px" 
                        TextMode="MultiLine"></asp:TextBox>
                 </td>
             </tr>
             <tr id ="tr7" runat="server">
                 <td class="style51">Library Services</td>
                 <td class="style52">
                    <asp:TextBox ID="txt_Lib_Services" runat="server" MaxLength="8000"  
                        ToolTip="Enter Library Services" 
                        Font-Bold="True" ForeColor="#0066FF" Width="98%" Height="62px" 
                        TextMode="MultiLine"></asp:TextBox>
                 </td>
             </tr>
             <tr id ="tr8" runat="server">
                 <td class="style51">
                     Library Services (L)</td>
                 <td class="style52">
                    <asp:TextBox ID="txt_Lib_Services2" runat="server" MaxLength="8000"  
                        ToolTip="Enter Services in Local Language" 
                        Font-Bold="True" ForeColor="#0066FF" Width="98%" Height="62px" 
                        TextMode="MultiLine"></asp:TextBox>
                 </td>
             </tr>
            <tr>
                <td class="style51">Select Library Photo</td>
                <td class="style52">
                    <asp:FileUpload ID="FileUpload13" runat="server"  />
                &nbsp;<asp:Image ID="Image22" runat="server" Height="69px" 
                         Width="66px" BorderStyle="None" 
                        ImageAlign="Middle"/> 
                </td>
            </tr>
             <tr>
                <td class="style51">Select Library Logo</td>
                <td class="style52">
                    <asp:FileUpload ID="FileUpload1" runat="server" />
                    <asp:Image ID="Image23" runat="server" BorderStyle="Solid" 
                        ImageAlign="Middle" BorderWidth="2px"/> 
                </td>
            </tr>
            <tr>
                <td class="style47" colspan="2">
                    <asp:Button ID="Cancel" runat="server"  CssClass="styleBttn" Font-Bold="True" ForeColor="Red" 
                        TabIndex="15" Text="Cancel" Width="71px" AccessKey="c"/>
                    <asp:Button ID="bttn_Update" runat="server"  CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red"
                        OnClientClick="return test1();" TabIndex="14" Text="Update" AccessKey="s" 
                        Width="74px" />
                </td>
            </tr>
            
            <tr>
                <td  bgcolor="#99CCFF" class="style44" colspan="2" rowspan="0">
                     <strong>*<span class="style48"> Mandatory Fields</span></strong></td>
            </tr>


             

        </table>

 
        
</asp:Content>
