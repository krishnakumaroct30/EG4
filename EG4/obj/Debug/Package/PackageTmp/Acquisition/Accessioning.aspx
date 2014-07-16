<%@ Page  Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Accessioning.aspx.vb" Inherits="EG4.Accessioning" %>


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
    
       .style51
        {
            text-align: justify;
            border-style: none;
            border-color: inherit;
            width: 280px;
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
            height: 18px;
        }
        .style55
        {
            text-align: justify;
            border-style: none;
            border-color: inherit;
            width: 25%;
            padding: 0px;
             background-color:#D5EAFF;  
            font-size: small;
            height: 18px;
        }
        .style56
        {
            text-align: center;
            width: 98%;
            background-color:#336699;
                      
        }
                
        .styleBttn
    {
             cursor:pointer;
            margin-left: 0px;
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
            font-style:italic;  
            font-weight:bold;  
            font-family:Arial;
            background-color:Silver;  
            height:25px;    
            }    
                
        .style56
        {
            text-align: center;
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
         ids = ["MainContent_txt_Hold_Description", "MainContent_txt_Hold_RecordingStorageTech", "MainContent_txt_Hold_Location", "MainContent_txt_Hold_Remarks", "MainContent_txt_Hold_VolEditors", "MainContent_txt_Hold_VolTitle"];
         control.makeTransliteratable(ids);
         // Show the transliteration control which can be used to toggle between         
         // English and Hindi and also choose other destination language.         
         control.showControl('translControl');
     }

     google.setOnLoadCallback(onLoad);
                   
           
    </script> 

 <script type ="text/javascript">
  //alpha-numeric only
     function suppressNonEng(event) {
         var chCode = ('charCode' in event) ? event.charCode : event.keyCode;
         if (48 <= chCode && chCode <= 57) {
             return (true);
         }
         if (97 <= chCode && chCode <= 122) {
             return (true);
         }

         if (chCode == 0 || chCode == 13 || chCode == 32) {
             return (true);
         }

         else {
             alert("Please Enter ENG Only Characters!");
             document.getElementById("MainContent_txt_Hold_AccNo").focus();
             return (false);
         }
     }
    </script>
     <script type ="text/javascript">
         //alpha-numeric only
         function EngOnly(event) {
             var chCode = ('charCode' in event) ? event.charCode : event.keyCode;
             if (65 <= chCode && chCode <= 90) {
                 return (true);
             }
             if (97 <= chCode && chCode <= 122) {
                 return (true);
             }

             if (chCode == 0 || chCode == 13 || chCode == 32) {
                 return (true);
             }

             else {
                 alert("Please Enter ENG Only Characters!");
                 document.getElementById("MainContent_txt_Hold_AccNo").focus();
                 return (false);
             }
         }
    </script>
    <script type ="text/javascript">
        //alpha-numeric only
        function NumericOnly(event) {
            var chCode = ('charCode' in event) ? event.charCode : event.keyCode;
            if (48 <= chCode && chCode <= 57) {
                return (true);
            }

            else {
                alert("Plz Enter Digits Only in yyyy format!");
                document.getElementById("MainContent_txt_Hold_VolYear").focus();
                return (false);
            }
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
                document.getElementById("MainContent_txt_Hold_AccDate").focus();
                return (false);
            }
        }
    </script>
    <script type ="text/javascript">
        //alpha-numeric only
        function DateOnly1(event) {
            var chCode = ('charCode' in event) ? event.charCode : event.keyCode;
            if (47 <= chCode && chCode <= 57) {
                return (true);
            }

            else {
                alert("Date Is Invalid, Enter in dd/MM/yyyy format only!");
                document.getElementById("MainContent_txt_Hold_DataGatheringDate").focus();
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
                document.getElementById("MainContent_txt_Hold_CreationDate").focus();
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
                document.getElementById("MainContent_txt_Hold_CompilationDate").focus();
                return (false);
            }
        }
    </script>
    <script type ="text/javascript">
        //alpha-numeric only
        function DateOnly4(event) {
            var chCode = ('charCode' in event) ? event.charCode : event.keyCode;
            if (47 <= chCode && chCode <= 57) {
                return (true);
            }

            else {
                alert("Date Is Invalid, Enter in dd/MM/yyyy format only!");
                document.getElementById("MainContent_txt_Hold_InspectionDate").focus();
                return (false);
            }
        }
    </script>

     <script type ="text/javascript">
         //alpha-numeric only
         function DateOnly5(event) {
             var chCode = ('charCode' in event) ? event.charCode : event.keyCode;
             if (47 <= chCode && chCode <= 57) {
                 return (true);
             }

             else {
                 alert("Date Is Invalid, Enter in dd/MM/yyyy format only!");
                 document.getElementById("MainContent_txt_Hold_ViewDate").focus();
                 return (false);
             }
         }
    </script>

     <script type ="text/javascript">
         //alpha-numeric only
         function DateOnly6(event) {
             var chCode = ('charCode' in event) ? event.charCode : event.keyCode;
             if (47 <= chCode && chCode <= 57) {
                 return (true);
             }

             else {
                 alert("Date Is Invalid, Enter in dd/MM/yyyy format only!");
                 document.getElementById("MainContent_txt_Hold_AlterDate").focus();
                 return (false);
             }
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
                         document.getElementById("MainContent_txt_Hold_AccSereis").focus();
                         return (false);
                     }
                 }
                 else {
                     alert("Please Enter ENG Only Characters!");
                     document.getElementById("MainContent_txt_Hold_AccSereis").focus();
                     return (false);
                 }
             }
             else {
                 alert("Please Enter ENG Only Characters!");
                 document.getElementById("MainContent_txt_Hold_AccSereis").focus();
                 return (false);
             }
         }
    </script>
    <script language="javascript" type="text/javascript">

        function valid1() {
            var bibtype = "";
            var mattype = "";
            var doctype = "";
            var lang = "";
            var yesno = "";
            var pub = "";
            var country = "";
            var currentyear = "";

           
            if (document.getElementById('<%=txt_Hold_AccNo.ClientID%>').value == "") {
                alert("Please enter proper \"Accession No\" field.");
                document.getElementById("MainContent_txt_Hold_AccNo").focus();
                return (false);
            }
            if (document.getElementById('<%=txt_Hold_AccDate.ClientID%>').value == "") {
                alert("Please enter proper \"Accession Date\" field.");
                document.getElementById("MainContent_txt_Hold_AccDate").focus();
                return (false);
            }

           
           
            return (true);
        }

    </script>

     <script language ="javascript" type ="text/javascript" >
         function Select(Select) {

             var grdv = document.getElementById('<%= Grid2.ClientID %>');
             var chbk = "cbd";

             var Inputs = grdv.getElementsByTagName("input");

             for (var n = 0; n < Inputs.length; ++n) {
                 if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(chbk, 0) >= 0) {
                     Inputs[n].checked = Select;
                 }
             }


             //         for (var n = 0; n < document.forms[0].length; n++) {
             //             //if (document.forms[0].elements[n].type == 'checkbox') {
             //             if (document.getElementById("cbd")== true) {
             //                 document.forms[0].elements[n].checked = Select;
             //             }
             //         }
             return false;
         }

    </script> 

     <script type="text/javascript">
         function formfocus() {
             document.getElementById('<%= DDL_Orders.ClientID %>').focus();
         }
         window.onload = formfocus;
    </script>
  

        <table id="ADMT1" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
            <tr>
                <td  bgcolor="#003366" class="style43" rowspan="1" style="color: #FFFFFF">Accessioning</td>
            </tr>  
             <tr>
                <td  bgcolor="#99CCFF"  colspan="2" style="text-align: center">
                     Type in Indian languages (Press Ctrl+g to toggle between English and India Language)
                     <div id='translControl'></div></td>
            </tr>          
        </table>    
                   

        
 <div class="style4">
       
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                    <ContentTemplate>

     <table id="Table3" runat="server" border="1" cellpadding="2" cellspacing="2" class="style35">
        
         <tr>
             <td bgcolor="#336699" class="style56" colspan="5">
                 <asp:Label ID="Label8" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="White">STEP1: Select Order from Drop-Down</asp:Label>
                 <asp:Label ID="Label10" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="Yellow"></asp:Label>
             </td>
         </tr>
         </table>
         <table id="Table1" runat="server" cellspacing="2" border="1"  cellpadding="3" class="style35">
           
            <tr style=" color:Green; font-weight:bold">
                <td class="style53">Select Order</td>
                <td class="style55">
                    <asp:DropDownList ID="DDL_Orders" runat="server" AutoPostBack="True" 
                        Font-Bold="True" ForeColor="#0066CC" Width="98%"></asp:DropDownList>
                    <ajaxToolkit:ListSearchExtender ID="ListSearchExtender1" runat="server" Enabled="True" IsSorted="True" PromptCssClass="PromptCSS" TargetControlID="DDL_Orders">
                    </ajaxToolkit:ListSearchExtender>
                 </td>

                

                <td class="style55">
                    <asp:Label ID="Label14" runat="server" Font-Bold="True"  Font-Size="Medium"></asp:Label>
                </td> 
                <td class="style55" colspan="2">
                    &nbsp;</td>
                
            </tr>
              <tr style=" color:Green; font-weight:bold">
                <td class="style53"> Vendor</td>
                <td class="style54" colspan="2">
                    <asp:Label ID="Label13" runat="server" Font-Bold="True"  Font-Size="Small"></asp:Label></td>               
                <td class="style54" colspan="2"> &nbsp;</td>               
            </tr>       
           

</table>


 

 <table id="Table2" runat="server" cellspacing="2" border="1"  cellpadding="2" class="style35">
         <tr>
             <td bgcolor="#336699" class="style56" colspan="5">
                 <asp:Label ID="Label11" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="White">Display Record(s)</asp:Label>
             </td>
         </tr>
         <tr>
             <td bgcolor="#336699" class="style56" colspan="5">
                 <asp:Label ID="Label12" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="White">STEP 2: Click the 'Click' link from Grid below to start Accessioning!</asp:Label>
             </td>
         </tr>
         <tr>

             <td bgcolor="#336699" class="style56" colspan="5">
                 <asp:GridView ID="Grid1" runat="server" AllowPaging="True" allowsorting="True" 
                     AutoGenerateColumns="False" DataKeyNames="ACQ_ID" Font-Bold="True" 
                     Font-Names="Tahoma" Font-Size="8pt" Height="100px" HorizontalAlign="Center" 
                     ShowFooter="True" style="width: 98%;  text-align: center;">
                     <Columns>
                         <asp:TemplateField HeaderText="S.N.">
                             <ItemTemplate>
                                 <asp:Label ID="lblsr" runat="server" CssClass="MBody" SkinID="" width="25px"></asp:Label>
                             </ItemTemplate>
                             <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                             <ItemStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" 
                                 ForeColor="#336699" Width="25px" />
                         </asp:TemplateField>

                          <asp:ButtonField HeaderText="Click To Accession"  Text="Click" CommandName="Select">
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" />
                            <ItemStyle ForeColor="#CC0000" Width="20px" Font-Bold="true" />
                            </asp:ButtonField>



                         <asp:BoundField DataField="TITLE" HeaderText="Title" SortExpression="TITLE" 
                             visible="true">
                         <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" 
                             Width="350px" />
                         <ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left" 
                             width="350px" />
                         </asp:BoundField>
                         <asp:BoundField DataField="VOL_NO" HeaderText="Vol No" SortExpression="VOL_NO" 
                             visible="true">
                         <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" 
                             Width="100px" />
                         <ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left" 
                             width="100px" />
                         </asp:BoundField>
                         <asp:BoundField DataField="ORDER_DATE" DataFormatString="{0:dd/MM/yyyy}" 
                             HeaderText="Order Date" visible="true">
                         <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" 
                             Width="150px" />
                         <ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left" 
                             width="150px" />
                         </asp:BoundField>
                         <asp:BoundField DataField="PROCESS_STATUS" HeaderText="Status" visible="true">
                         <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" 
                             Width="160px" />
                         <ItemStyle Font-Names="Arial" forecolor="Red" horizontalalign="Left" 
                             width="160px" />
                         </asp:BoundField>
                         <asp:BoundField DataField="VEND_NAME" HeaderText="Vendor" visible="true">
                         <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" 
                             Width="200px" />
                         <ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left" 
                             width="200px" />
                         </asp:BoundField>
                         <asp:BoundField DataField="COPY_PROPOSED" HeaderText="Copy Proposed" 
                             visible="true">
                         <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" 
                             Width="180px" />
                         <ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left" 
                             width="180px" />
                         </asp:BoundField>
                         <asp:BoundField DataField="COPY_APPROVED" HeaderText="Copy Approved" 
                             visible="true">
                         <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" 
                             Width="180px" />
                         <ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left" 
                             width="180px" />
                         </asp:BoundField>
                         <asp:BoundField DataField="COPY_ORDERED" HeaderText="Copy Ordered" visible="true">
                         <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="180px" />
                         <ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left" width="180px" />
                         </asp:BoundField>

                          <asp:BoundField DataField="COPY_RECEIVED" HeaderText="Copy Recd" visible="true">
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="180px" />
                            <ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left" width="180px" />
                         </asp:BoundField>
                          <asp:BoundField DataField="COPY_ACCESSIONED" HeaderText="Copy Accd." visible="true">
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="180px" />
                            <ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left" width="180px" />
                         </asp:BoundField>
                        
                         
                     </Columns>
                     <PagerStyle BackColor="#3399FF" BorderColor="#C0C000" BorderStyle="Solid" 
                         Font-Bold="True" Font-Size="Small" ForeColor="White" HorizontalAlign="Center" 
                         VerticalAlign="Middle" />
                     <RowStyle BackColor="#DADADA" BorderColor="Desktop" ForeColor="#3399FF" />
                     <SelectedRowStyle BackColor="#ff9933" BorderColor="SteelBlue" 
                         BorderStyle="Solid" />
                     <HeaderStyle BackColor="Desktop" Font-Bold="True" Font-Italic="False" 
                         Font-Names="Times new roman" Font-Overline="False" Font-Underline="False" 
                         ForeColor="White" Width="90%" />
                     <PagerSettings FirstPageText="First" LastPageText="Last" 
                         Mode="NumericFirstLast" PageButtonCount="10" Position="TopAndBottom" />
                     <AlternatingRowStyle BackColor="#EFEFEF" Font-Names="Tahoma" 
                         ForeColor="#0066FF" />
                 </asp:GridView>

             </td>
         </tr>
     </table>

                </ContentTemplate>
                   <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="DDL_Orders" EventName="SelectedIndexChanged" />                                     
                   </Triggers>
                    </asp:UpdatePanel>        
                           
</div>

 <div class="style4">
 
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
              <ContentTemplate>

        
         <table id="Table4"  runat="server" cellspacing="2" border="1"  cellpadding="2" class="style35">
             <tr>
             <td bgcolor="#336699" class="style56" colspan="13">
                 <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="White">STEP 3: Type Data and Press SAVE/UPDATE Button!</asp:Label>
                 <asp:Label ID="Label15" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="Yellow"></asp:Label>
             </td>
         </tr>
            
             <tr style=" color:Green; font-weight:bold">
                <td class="style51">Cat No</td>
                <td class="style54" colspan="2">  
                    <asp:Label ID="Label_CATNO" runat="server" Font-Bold="True" Font-Size="Small" 
                        style="font-size: x-small"></asp:Label>
                </td>
               
                 <td class="style54" colspan="3">
                     <asp:Label ID="Label_ACQID" runat="server" Font-Bold="True" Font-Size="Small" 
                         style="font-size: x-small"></asp:Label>
                 </td>
                 <td class="style54" colspan="4">
                     <asp:Label ID="Label_HOLDID" runat="server" Font-Bold="True" Font-Size="Small" 
                         style="font-size: x-small"></asp:Label>
                 </td>
                 <td class="style54" colspan="2">
                     &nbsp;</td>
                 <td class="style54">
                     &nbsp;</td>
               
            </tr>

             <tr style=" color:Green; font-weight:bold">
                <td class="style51">Multi-Vol?</td>
                <td class="style54" colspan="2">  
                    <asp:DropDownList ID="DDL_YN" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" ToolTip="Select Target Audience">
                        <asp:ListItem>Y</asp:ListItem>
                        <asp:ListItem>N</asp:ListItem>
                    </asp:DropDownList>
                </td>
               
                 <td class="style54" colspan="3">
                     <asp:Label ID="Label" runat="server" Font-Bold="True" Font-Size="Small" 
                         style="font-size: x-small">Materials: </asp:Label>
                 </td>
                 <td class="style54" colspan="4">
                     <asp:Label ID="Label_MATTYPE" runat="server" Font-Bold="True" Font-Size="Small" 
                         style="font-size: x-small"></asp:Label>
                 </td>
                 <td class="style54" colspan="2">
                     <asp:Label ID="Label_DOCTYPE" runat="server" Font-Bold="True" Font-Size="Small" 
                         style="font-size: x-small"></asp:Label>
                 </td>
                 <td class="style54">
                     <asp:Label ID="Label48" runat="server" Font-Bold="True" Font-Size="Small" 
                         style="font-size: x-small"></asp:Label>
                 </td>
               
            </tr>
            <tr style=" color:Green; font-weight:bold">
                <td class="style51"> 
                    Title Details</td>
                <td class="style54" colspan="12">  
                    <asp:Label ID="Label_TITLE" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                </td>               
            </tr>
             <tr style=" color:Red; font-weight:bold">
                <td class="style51"> 
                    Accession All Copies</td>
                <td class="style54" colspan="12">  
                    <asp:CheckBox ID="CB_RecvAll" runat="server" Font-Bold="True" 
                        Font-Size="Small" 
                        
                        
                        Text="Accession All Copies Recd. with Single Click with Range of Acc.No u hv typed below. HELP: Type the Alpha Series in Series Text Box and Enter First Accession No (Numeric Only) in the Acc No Text Box." 
                        Width="98%" />
                </td>               
            </tr>
            <tr>
                <td class="style51">
                    <asp:Label ID="Label21" runat="server" Font-Bold="True" 
                        Font-Size="Small" Text="Acc. Series"></asp:Label></td>
                <td class="style52">
                    <asp:TextBox ID="txt_Hold_AccSereis" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="18px" MaxLength="2" 
                        onkeypress="return EngOnlyInput (event)" style="text-transform: uppercase" 
                        ToolTip="Enter Accession Sereis - Optional/Alpha Device" Width="40px" 
                        Wrap="False"></asp:TextBox>
                    <ajaxToolkit:AutoCompleteExtender ID="txt_Hold_AccSereis_AutoCompleteExtender" 
                        runat="server" CompletionSetCount="10" EnableCaching="true" 
                        FirstRowSelected="false" MinimumPrefixLength="1" ServiceMethod="SearchAccNo" 
                        TargetControlID="txt_Hold_AccSereis">
                    </ajaxToolkit:AutoCompleteExtender>
                 </td>
                <td class="style52" colspan="2">
                    <asp:Label ID="Label56" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Acc. No.*"></asp:Label>
                </td>
                <td class="style52" colspan="2">
                    <asp:TextBox ID="txt_Hold_AccNo" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="18px" MaxLength="20" 
                        onkeypress="return suppressNonEng (event)" style="text-transform: uppercase" 
                        ToolTip="Enter Accession No" Width="96%" Wrap="False"></asp:TextBox>
                    <ajaxToolkit:AutoCompleteExtender ID="txt_Hold_AccNo_AutoCompleteExtender" 
                        runat="server" CompletionSetCount="10" EnableCaching="true" 
                        FirstRowSelected="false" MinimumPrefixLength="1" ServiceMethod="SearchAccessionNo" 
                        TargetControlID="txt_Hold_AccNo">
                    </ajaxToolkit:AutoCompleteExtender>
                </td>
                <td class="style52" colspan="2">
                    <asp:Label ID="Label47" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Acc Date*"></asp:Label>
                </td>
                <td class="style52" colspan="3">
                    <asp:TextBox ID="txt_Hold_AccDate" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="10" 
                        onkeypress="return DateOnly (event)" ToolTip="Click to Select Date" 
                        Width="71px"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="txt_Hold_AccDate_CalendarExtender" 
                        runat="server" Enabled="True" Format="dd/MM/yyyy" 
                        TargetControlID="txt_Hold_AccDate">
                    </ajaxToolkit:CalendarExtender>
                </td>
                <td class="style52" colspan="2">
                    &nbsp;</td>
            </tr>           

             <tr>
                <td class="style51">
                    <asp:Label ID="Label44" runat="server" Font-Bold="True" Font-Size="Small" Text="Show?"></asp:Label>
                    </td>
                <td class="style52">
                    <asp:DropDownList ID="DDL_Show" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" ToolTip="Select Y/N ">
                        <asp:ListItem>Y</asp:ListItem>
                        <asp:ListItem>N</asp:ListItem>
                    </asp:DropDownList>
                 </td>
                <td class="style52" colspan="11">
                    Issuable?
                    <asp:DropDownList ID="DDL_Issuable" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" ToolTip="Select Y/N">
                        <asp:ListItem>Y</asp:ListItem>
                        <asp:ListItem>N</asp:ListItem>
                    </asp:DropDownList>
                 </td>
            </tr>

            <tr id ="TR_VOL_NO" runat ="server">
                <td class="style51">
                    <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="Small" Text="Volume No"></asp:Label>
                    </td>
                <td class="style52">
                    <asp:TextBox ID="txt_Hold_VolNo" runat="server" Font-Bold="True" ForeColor="#0066FF" Height="18px" MaxLength="50" ToolTip="Enter Volume No" Width="98%" Wrap="False"></asp:TextBox>
                    </td>
                <td class="style52" colspan="11">
                    &nbsp;</td>
            </tr>
            
             <tr  id ="TR_VOL_YEAR" runat ="server">
                <td class="style51">
                    <asp:Label ID="Label60" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Volume Year"></asp:Label>
                    </td>
                <td class="style52">
                    <asp:TextBox ID="txt_Hold_VolYear" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="18px" MaxLength="4" 
                        onkeypress="return NumericOnly (event)" ToolTip="Enter 4 Digit Volume Year" 
                        Width="50px"></asp:TextBox>
                    </td>
                <td class="style52" colspan="11">
                    &nbsp;</td>
            </tr>

            <tr  id ="TR_VOL_TITLE" runat ="server">
                <td class="style51">
                    <asp:Label ID="Label61" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Volume Title"></asp:Label>
                    </td>
                <td class="style52" colspan="12">
                    <asp:TextBox ID="txt_Hold_VolTitle" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="18px" MaxLength="500" ToolTip="Enter Volume Title" 
                        Width="98%" Wrap="False"></asp:TextBox>
                    </td>
            </tr>

             <tr  id ="TR_VOL_EDITORS" runat ="server">
                 <td class="style51">
                     <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Size="Small" 
                         Text="Vol Editor(s)"></asp:Label>
                 </td>
                 <td class="style52" colspan="12">
                     <asp:TextBox ID="txt_Hold_VolEditors" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" Height="18px" MaxLength="500" 
                         ToolTip="Enter Volume Editors separated by semicolon ;" Width="98%" 
                         Wrap="False"></asp:TextBox>
                 </td>
             </tr>
             
             <tr id ="TR_COPY_ISBN" runat ="server">
                <td class="style51">
                    <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Size="Small" Text="Volume ISBN"></asp:Label>
                    </td>
                 <td class="style52" colspan="7">
                     <asp:TextBox ID="txt_Hold_CopyISBN" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" Height="18px" MaxLength="50" ToolTip="Enter Volume ISBN No" 
                         Width="50%" Wrap="False"></asp:TextBox>
                 </td>
                <td class="style52" colspan="5">
                    &nbsp;</td>
            </tr>

            <tr  id ="TR_CLASS_NO" runat ="server">
                <td class="style51">
                    <asp:Label ID="Label62" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Class No"></asp:Label>
                </td>
                <td class="style52" colspan="7">
                    <asp:TextBox ID="txt_Hold_ClassNo" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="18px" MaxLength="150" ToolTip="Enter Class No" 
                        Width="98%" Wrap="False"></asp:TextBox>
                         <ajaxToolkit:AutoCompleteExtender ID="txt_Hold_ClassNo_AutoCompleteExtender" 
                        runat="server" CompletionSetCount="10" EnableCaching="true" 
                        FirstRowSelected="false" MinimumPrefixLength="1" ServiceMethod="SearchClassNumber" 
                        TargetControlID="txt_Hold_ClassNo">
                    </ajaxToolkit:AutoCompleteExtender>
                 </td>
                <td class="style52" colspan="5">
                    &nbsp;</td>
            </tr>


             <tr  id ="TR_BOOK_NO" runat ="server">
                 <td class="style51">
                     <asp:Label ID="Label63" runat="server" Font-Bold="True" Font-Size="Small" 
                         Text="Book No" Width="100px"></asp:Label>
                 </td>
                 <td class="style52" colspan="7">
                     <asp:TextBox ID="txt_Hold_BookNo" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" Height="18px" MaxLength="50" 
                         style="text-transform: uppercase" ToolTip="Enter Book No" Width="150px" 
                         Wrap="False"></asp:TextBox>
                 </td>
                 <td class="style52" colspan="5">
                     &nbsp;</td>
             </tr>

             <tr  id ="TR_PAGINATION" runat ="server">
                 <td class="style51">
                     <asp:Label ID="Label64" runat="server" Font-Bold="True" Font-Size="Small" 
                         Height="16px" Text="Pages*"></asp:Label>
                 </td>
                 <td class="style52" colspan="7">
                     <asp:TextBox ID="txt_Hold_Pages" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" Height="18px" MaxLength="250" ToolTip="Enter Pagination " 
                         Width="98%" Wrap="False"></asp:TextBox>
                 </td>
                 <td class="style52" colspan="5">
                     do not type P. with pages.</td>
             </tr>

             <tr  id ="TR_SIZE" runat ="server">
                 <td class="style51">
                     <asp:Label ID="Label74" runat="server" Font-Bold="True" Font-Size="Small" 
                         Text="Dimension"></asp:Label>
                 </td>
                 <td class="style52" colspan="7">
                     <asp:TextBox ID="txt_Hold_Size" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" Height="18px" MaxLength="50" 
                         ToolTip="Enter Size and Dimension" Width="150px" Wrap="False"></asp:TextBox>
                     in cm</td>
                 <td class="style52" colspan="5">
                     &nbsp;</td>
             </tr>

            <tr  id ="TR_ILLUSTRATION" runat ="server">
                <td class="style51">
                    <asp:Label ID="Label22" runat="server" Font-Bold="True" Font-Size="Small" Text="Illustration"></asp:Label>
                </td>
                <td class="style52" colspan="7">
                    <asp:CheckBox ID="CB_Illus" runat="server" Checked="True" Font-Bold="True" 
                        Font-Size="Small" Text="Illustration" />
                </td>
                <td class="style52" colspan="5">
                    &nbsp;</td>
            </tr>


            <tr  id ="TR_COLLECTION_TYPE" runat ="server">
                <td class="style51">
                    <asp:Label ID="Label75" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Collection*"></asp:Label>
                </td>
                <td class="style52" colspan="7">
                    <asp:DropDownList ID="DDL_CollectionType" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" ToolTip="Select Collection Type">
                        <asp:ListItem Selected="True" Value="C">Circulation</asp:ListItem>
                        <asp:ListItem Value="R">Reference</asp:ListItem>
                        <asp:ListItem Value="G">Book Bank (General)</asp:ListItem>
                        <asp:ListItem Value="S">Book Bank (SCST)</asp:ListItem>
                        <asp:ListItem Value="N">Non-Returnable</asp:ListItem>
                    </asp:DropDownList>
                 </td>
                <td class="style52" colspan="5">
                    &nbsp;</td>
            </tr>
            
             <tr  id ="TR_STA_CODE" runat ="server">
                 <td class="style51">
                     <asp:Label ID="Label76" runat="server" Font-Bold="True" Font-Size="Small" 
                         Text="Status*"></asp:Label>
                 </td>
                 <td class="style52" colspan="7">
                     <asp:DropDownList ID="DDL_Status" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" ToolTip="Select Status">
                     </asp:DropDownList>
                 </td>
                 <td class="style52" colspan="5">
                     &nbsp;</td>
             </tr>

             <tr  id ="TR_BIND_CODE" runat ="server">
                 <td class="style51">
                     <asp:Label ID="Label77" runat="server" Font-Bold="True" Font-Size="Small" 
                         Text="Binding"></asp:Label>
                 </td>
                 <td class="style52" colspan="7">
                     <asp:DropDownList ID="DDL_Binding" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" ToolTip="Select Binding">
                     </asp:DropDownList>
                 </td>
                 <td class="style52" colspan="5">
                     &nbsp;</td>
             </tr>


            <tr  id ="TR_SEC_CODE" runat ="server">
                <td class="style51">
                    <asp:Label ID="Label78" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Section"></asp:Label>
                </td>
                <td class="style52" colspan="7">
                    <asp:DropDownList ID="DDL_Section" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" ToolTip="Select Section">
                    </asp:DropDownList>
                 </td>
                <td class="style52" colspan="5">
                    &nbsp;</td>
            </tr>
            
             <tr  id ="TR_ACC_MAT_CODE" runat ="server">
                 <td class="style51">
                     <asp:Label ID="Label81" runat="server" Font-Bold="True" Font-Size="Small" 
                         Text="Accompanying Materials"></asp:Label>
                 </td>
                 <td class="style52" colspan="8">
                     <asp:DropDownList ID="DDL_AccMaterials" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" ToolTip="Select Accompanying Materials">
                     </asp:DropDownList>
                     &nbsp;Select Accompanying Materials received with books</td>
                 <td class="style52" colspan="4">
                     &nbsp;</td>
             </tr>

             <tr  id ="TR_FORMAT_CODE" runat ="server">
                 <td class="style51">
                     <asp:Label ID="Label82" runat="server" Font-Bold="True" Font-Size="Small" 
                         Text="Format*"></asp:Label>
                 </td>
                 <td class="style52" colspan="8">
                     <asp:DropDownList ID="DDL_Format" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" ToolTip="Select Physical Format/Medium">
                     </asp:DropDownList>
                 </td>
                 <td class="style52" colspan="4">
                     &nbsp;</td>
             </tr>
             
            <tr>
                <td class="style51">
                    <asp:Label ID="Label23" runat="server" Font-Bold="True" Font-Size="Small"  
                        Text="Library*"></asp:Label>
                </td>
                <td class="style52" colspan="12">
                    <asp:DropDownList ID="DDL_Library" runat="server" Font-Bold="True"  
                        ForeColor="#0066FF" ToolTip="Select Library" Width="98%">
                    </asp:DropDownList>
                </td>
            </tr>

            <tr id ="TR_REMARKS" runat ="server">
                <td class="style51">
                    <asp:Label ID="Label24" runat="server" Font-Bold="True" Font-Size="Small"  Text="Remarks"></asp:Label>
                </td>
                <td class="style52" colspan="12">
                    <asp:TextBox ID="txt_Hold_Remarks" runat="server" MaxLength="250"   
                        ToolTip="Enter Remarks" Wrap="False"  Font-Bold="True" ForeColor="#0066FF" 
                        Width="98%" Height="62px" TextMode="MultiLine"></asp:TextBox>
                    </td>
            </tr>

             <tr  id ="TR_PHYSICAL_LOCATION" runat ="server">
                <td class="style51">
                    <asp:Label ID="Label25" runat="server" Font-Bold="True" Font-Size="Small" Text="Location"></asp:Label>
                 </td>
                <td class="style52" colspan="12">
                    <asp:TextBox ID="txt_Hold_Location" runat="server" MaxLength="250" ToolTip="Type Location of Copy" Wrap="False" Font-Bold="True" ForeColor="#0066FF" Width="98%" Height="23px"></asp:TextBox>
                    </td>
            </tr>

             <tr id ="TR_REFERENCE_NO" runat ="server">
                <td class="style51">
                    <asp:Label ID="Label40" runat="server" Font-Bold="True" Font-Size="Small" Text="Reference No"></asp:Label>
                 </td>
                <td class="style52" colspan="12">
                    <asp:TextBox ID="txt_Hold_ReferenceNo" runat="server" MaxLength="250"  ToolTip="Type some Local Information" Wrap="False" Font-Bold="True" ForeColor="#0066FF" Width="98%" Height="23px"></asp:TextBox>
                    </td>
            </tr>
            
             <tr  id ="TR_MEDIUM" runat ="server">
                <td class="style51">
                    <asp:Label ID="Label83" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Recording Medium"></asp:Label>
                </td>
                <td class="style52" colspan="7">
                    <asp:TextBox ID="txt_Hold_RecordingMedium" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="18px" MaxLength="50" 
                        ToolTip="Enter Recording Medium" Width="150px"></asp:TextBox>
                 </td>
                <td class="style52" colspan="5">
                    &nbsp;</td>
            </tr>

             <tr  id ="TR_RECORDING_CATEGORY" runat ="server">
                 <td class="style51">
                     <asp:Label ID="Label84" runat="server" Font-Bold="True" Font-Size="Small" 
                         Text="Recording Category"></asp:Label>
                 </td>
                 <td class="style52" colspan="7">
                     <asp:TextBox ID="txt_Hold_RecordingCategory" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" Height="18px" MaxLength="50" 
                         ToolTip="Enter Recording Category" Width="150px"></asp:TextBox>
                 </td>
                 <td class="style52" colspan="5">
                     &nbsp;</td>
             </tr>
             <tr  id ="TR_RECORDING_FORM" runat ="server">
                 <td class="style51">
                     <asp:Label ID="Label85" runat="server" Font-Bold="True" Font-Size="Small" 
                         Text="Recording Form"></asp:Label>
                 </td>
                 <td class="style52" colspan="7">
                     <asp:TextBox ID="txt_Hold_RecordingForm" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" Height="18px" MaxLength="50" 
                         ToolTip="Enter Recording Form" Width="150px"></asp:TextBox>
                 </td>
                 <td class="style52" colspan="5">
                     &nbsp;</td>
             </tr>

             <tr id ="TR_RECORDING_FORMAT" runat ="server">
                <td class="style51">
                    <asp:Label ID="Label86" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Recording Format"></asp:Label>
                </td>
                <td class="style52" colspan="7">
                    <asp:TextBox ID="txt_Hold_RecordingFormat" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="18px" MaxLength="50" 
                        ToolTip="Enter Recording Format" Width="150px"></asp:TextBox>
                 </td>
                <td class="style52" colspan="5">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_RECORDING_SPEED" runat ="server">
                <td class="style51">
                    <asp:Label ID="Label35" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Recording Speed"></asp:Label>
                </td>
                <td class="style52" colspan="7">
                    <asp:TextBox ID="txt_Hold_RecordingSpeed" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="18px" MaxLength="50" 
                        ToolTip="Enter Recording Speed" Width="150px" Wrap="False"></asp:TextBox>
                 </td>
                <td class="style52" colspan="5">
                    &nbsp;</td>
            </tr>


             <tr id ="TR_RECORDING_STORAGE_TECH" runat ="server">
                 <td class="style51">
                     <asp:Label ID="Label36" runat="server" Font-Bold="True" Font-Size="Small" 
                         Text="Storage Technology"></asp:Label>
                 </td>
                 <td class="style52" colspan="7">
                     <asp:TextBox ID="txt_Hold_RecordingStorageTech" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" Height="18px" MaxLength="50" 
                         ToolTip="Enter Recording Storage Technology" Width="98%" Wrap="False"></asp:TextBox>
                 </td>
                 <td class="style52" colspan="5">
                     &nbsp;</td>
             </tr>

             <tr id ="TR_RECORDING_PLAY_DURATION" runat ="server">
                 <td class="style51">
                     <asp:Label ID="Label37" runat="server" Font-Bold="True" Font-Size="Small" 
                         Text="Play Duration" Width="100px"></asp:Label>
                 </td>
                 <td class="style52" colspan="7">
                     <asp:TextBox ID="txt_Hold_RecordingDuration" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" Height="18px" MaxLength="50" 
                         ToolTip="Enter Play Duration in Minutes" Width="98%" Wrap="False"></asp:TextBox>
                 </td>
                 <td class="style52" colspan="5">
                     &nbsp;</td>
             </tr>

             <tr id ="TR_VIDEO_TYPEOFVISUAL" runat ="server">
                 <td class="style51">
                     <asp:Label ID="Label38" runat="server" Font-Bold="True" Font-Size="Small" 
                         Text="Type of Visuals"></asp:Label>
                 </td>
                 <td class="style52" colspan="7">
                     <asp:TextBox ID="txt_Hold_TypeOfVisuals" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" Height="18px" MaxLength="50" 
                         ToolTip="Enter Type of Visuals" Width="98%"></asp:TextBox>
                 </td>
                 <td class="style52" colspan="5">
                     &nbsp;</td>
             </tr>


             <tr id ="TR_CARTOGRAPHIC_SCALE" runat ="server">
                <td class="style51">
                    <asp:Label ID="Label54" runat="server" Font-Bold="True" 
                        Font-Size="Small" Text="Scale"></asp:Label>
                    </td>
                <td class="style52" colspan="6">
                    <asp:TextBox ID="txt_Hold_Scale" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="18px" MaxLength="50" 
                        ToolTip="Enter Scale" Width="98%" Wrap="False"></asp:TextBox>
                    </td>
                <td class="style52" colspan="6">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_CARTOGRAPHIC_PROJECTION" runat ="server">
                 <td class="style51">
                     <asp:Label ID="Label55" runat="server" Font-Bold="True" Font-Size="Small" 
                         Text="Projection"></asp:Label>
                 </td>
                 <td class="style52" colspan="6">
                     <asp:TextBox ID="txt_Hold_Projection" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" Height="18px" MaxLength="150" ToolTip="Enter Projection" 
                         Width="200px"></asp:TextBox>
                 </td>
                 <td class="style52" colspan="6">
                     &nbsp;</td>
             </tr>

             <tr id ="TR_CARTOGRAPHIC_COORDINATES" runat ="server">
                <td class="style51">
                    <asp:Label ID="Label18" runat="server" Font-Bold="True" 
                        Font-Size="Small" Text="Co-Ordinates"></asp:Label>
                    </td>
                <td class="style52" colspan="6">
                    <asp:TextBox ID="txt_Hold_Coordinates" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="18px" MaxLength="150" 
                        ToolTip="Enter Co-Ordinates" Width="200px" Wrap="False"></asp:TextBox>
                    </td>
                <td class="style52" colspan="6">
                    &nbsp;</td>
            </tr>
           
             <tr id ="TR_CARTOGRAPHIC_GEOGRAPHIC_LOCATION" runat ="server">
                 <td class="style51">
                     <asp:Label ID="Label19" runat="server" Font-Bold="True" Font-Size="Small" 
                         Text="Geographic Location"></asp:Label>
                 </td>
                 <td class="style52" colspan="6">
                     <asp:TextBox ID="txt_Hold_GeographicLocation" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" Height="18px" MaxLength="150" 
                         ToolTip="Enter Geographic Location" Width="200px"></asp:TextBox>
                 </td>
                 <td class="style52" colspan="6">
                     &nbsp;</td>
             </tr>

             <tr id ="TR_CARTOGRAPHIC_MEDIUM" runat ="server">
                 <td class="style51">
                     <asp:Label ID="Label26" runat="server" Font-Bold="True" Font-Size="Small" 
                         Text="Cartographic Medium"></asp:Label>
                 </td>
                 <td class="style52" colspan="6">
                     <asp:DropDownList ID="DDL_GeographicMedium" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" ToolTip="Select Cartographic Medium">
                         <asp:ListItem Value="Paper">Paper</asp:ListItem>
                         <asp:ListItem Value="Wood">Wood</asp:ListItem>
                         <asp:ListItem Value="Stone">Stone</asp:ListItem>
                         <asp:ListItem>Metal</asp:ListItem>
                         <asp:ListItem>Synthetic</asp:ListItem>
                         <asp:ListItem>Textile</asp:ListItem>
                         <asp:ListItem>Plastic</asp:ListItem>
                         <asp:ListItem>Glass</asp:ListItem>
                         <asp:ListItem>Venyl</asp:ListItem>
                         <asp:ListItem>Vellum</asp:ListItem>
                         <asp:ListItem>Plaster</asp:ListItem>
                         <asp:ListItem>Leather</asp:ListItem>
                         <asp:ListItem>Others</asp:ListItem>
                     </asp:DropDownList>
                 </td>
                 <td class="style52" colspan="6">
                     &nbsp;</td>
             </tr>

              <tr id ="TR_GLOBE_TYPE" runat ="server">
                <td class="style51">
                    <asp:Label ID="Label65" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Globe Type"></asp:Label>
                </td>
                <td class="style52" colspan="4">
                    <asp:TextBox ID="txt_Hold_GlobeType" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="50" 
                        onkeypress="return DateOnly4 (event)" ToolTip="Enter Globe Type" 
                        Width="200px"></asp:TextBox>
                 </td>
                <td class="style52" colspan="8">
                    &nbsp;</td>
            </tr>
           
           <tr id ="TR_CARTOGRAPHIC_DATAGATHERING_DATE" runat ="server">
                <td class="style51">
                    <asp:Label ID="Label87" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Data Gathering Date"></asp:Label>
                </td>
                <td class="style52" colspan="4">
                    <asp:TextBox ID="txt_Hold_DataGatheringDate" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="10" ToolTip="Click to Select Date" 
                        Width="71px" onkeypress="return DateOnly1 (event)"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="txt_Hold_DataGatheringDate_CalendarExtender" 
                        runat="server" Enabled="True" Format="dd/MM/yyyy" 
                        TargetControlID="txt_Hold_DataGatheringDate">
                    </ajaxToolkit:CalendarExtender>
                    &nbsp;dd/MM/yyyy</td>
                <td class="style52" colspan="8">
                    &nbsp;</td>
            </tr>

                        
             <tr id ="TR_CREATION_DATE" runat ="server">
                 <td class="style51">
                     <asp:Label ID="Label88" runat="server" Font-Bold="True" Font-Size="Small" 
                         Text="Creation Date"></asp:Label>
                 </td>
                 <td class="style52" colspan="4">
                     <asp:TextBox ID="txt_Hold_CreationDate" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" Height="16px" MaxLength="10" 
                         onkeypress="return DateOnly2 (event)" ToolTip="Click to Select Date" 
                         Width="71px"></asp:TextBox>
                     <ajaxToolkit:CalendarExtender ID="txt_Hold_CreationDate_CalendarExtender" 
                         runat="server" Enabled="True" Format="dd/MM/yyyy" 
                         TargetControlID="txt_Hold_CreationDate">
                     </ajaxToolkit:CalendarExtender>
                     dd/MM/yyyy</td>
                 <td class="style52" colspan="8">
                     &nbsp;</td>
             </tr>

             <tr id ="TR_CARTOGRAPHIC_COMPILATION_DATE" runat ="server">
                 <td class="style51">
                     <asp:Label ID="Label89" runat="server" Font-Bold="True" Font-Size="Small" 
                         Text="Compilation Date"></asp:Label>
                 </td>
                 <td class="style52" colspan="4">
                     <asp:TextBox ID="txt_Hold_CompilationDate" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" Height="16px" MaxLength="10" 
                         onkeypress="return DateOnly3 (event)" ToolTip="Click to Select Date" 
                         Width="71px"></asp:TextBox>
                     <ajaxToolkit:CalendarExtender ID="txt_Hold_CompilationDate_CalendarExtender" 
                         runat="server" Enabled="True" Format="dd/MM/yyyy" 
                         TargetControlID="txt_Hold_CompilationDate">
                     </ajaxToolkit:CalendarExtender>
                     dd/MM/yyyy</td>
                 <td class="style52" colspan="8">
                     &nbsp;</td>
             </tr>
           
           <tr id ="TR_CARTOGRAPHIC_INSPECTION_DATE" runat ="server">
                <td class="style51">
                    <asp:Label ID="Label90" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Inspection Date"></asp:Label>
                </td>
                <td class="style52" colspan="4">
                    <asp:TextBox ID="txt_Hold_InspectionDate" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="10" 
                        onkeypress="return DateOnly4 (event)" ToolTip="Click to Select Date" 
                        Width="71px"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="txt_Hold_InspectionDate_CalendarExtender" 
                        runat="server" Enabled="True" Format="dd/MM/yyyy" 
                        TargetControlID="txt_Hold_InspectionDate">
                    </ajaxToolkit:CalendarExtender>
                    dd/MM/yyyy</td>
                <td class="style52" colspan="8">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_VIDEO_COLOR" runat ="server">
                <td class="style51">
                    <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Color"></asp:Label>
                </td>
                <td class="style52" colspan="4">
                    <asp:TextBox ID="txt_Hold_Color" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="50" 
                        ToolTip="Enter Color" 
                        Width="200px"></asp:TextBox>
                 </td>
                <td class="style52" colspan="8">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_PLAYBACK_CHANNELS" runat ="server">
                <td class="style51">
                    <asp:Label ID="Label6" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Playback Channel"></asp:Label>
                </td>
                <td class="style52" colspan="4">
                    <asp:TextBox ID="txt_Hold_PlayBackChannel" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="50" 
                        onkeypress="return DateOnly4 (event)" ToolTip="Enter Playback Channel" 
                        Width="200px"></asp:TextBox>
                 </td>
                <td class="style52" colspan="8">
                    &nbsp;</td>
            </tr> 

             <tr id ="TR_TAPE_WIDTH" runat ="server">
                <td class="style51">
                    <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Tape Width"></asp:Label>
                </td>
                <td class="style52" colspan="4">
                    <asp:TextBox ID="txt_Hold_TapeWidth" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="50" 
                        onkeypress="return DateOnly4 (event)" ToolTip="Enter Tape Width" 
                        Width="200px"></asp:TextBox>
                 </td>
                <td class="style52" colspan="8">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_TAPE_CONFIGURATION" runat ="server">
                <td class="style51">
                    <asp:Label ID="Label17" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Tape Configuration"></asp:Label>
                </td>
                <td class="style52" colspan="4">
                    <asp:TextBox ID="txt_Hold_TapeConfiguration" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="50" 
                        onkeypress="return DateOnly4 (event)" ToolTip="Enter Tape Configuration" 
                        Width="200px"></asp:TextBox>
                 </td>
                <td class="style52" colspan="8">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_KIND_OF_DISK" runat ="server">
                <td class="style51">
                    <asp:Label ID="Label20" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Disk Type"></asp:Label>
                </td>
                <td class="style52" colspan="4">
                    <asp:TextBox ID="txt_Hold_KindofDisk" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="50" 
                         ToolTip="Enter Disk Type" 
                        Width="200px"></asp:TextBox>
                 </td>
                <td class="style52" colspan="8">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_KIND_OF_CUTTING" runat ="server">
                <td class="style51">
                    <asp:Label ID="Label27" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Disk Cutting Type"></asp:Label>
                </td>
                <td class="style52" colspan="4">
                    <asp:TextBox ID="txt_Hold_KindofCutting" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="50" 
                        ToolTip="Enter Disk Cutting Technology" 
                        Width="200px"></asp:TextBox>
                 </td>
                <td class="style52" colspan="8">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_ENCODING_STANDARD" runat ="server">
                <td class="style51">
                    <asp:Label ID="Label28" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Encoding Standard"></asp:Label>
                </td>
                <td class="style52" colspan="4">
                    <asp:TextBox ID="txt_Hold_EncodingStandard" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="50" 
                        ToolTip="Enter Encoding Standard" 
                        Width="200px"></asp:TextBox>
                 </td>
                <td class="style52" colspan="8">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_CAPTURE_TECHNIQUE" runat ="server">
                <td class="style51">
                    <asp:Label ID="Label7" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Capture Technique"></asp:Label>
                </td>
                <td class="style52" colspan="4">
                    <asp:TextBox ID="txt_Hold_CaptureTechnique" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="50" 
                        ToolTip="Enter Audio/Video Capture Technique" 
                        Width="200px"></asp:TextBox>
                 </td>
                <td class="style52" colspan="8">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_PHOTO_NO" runat ="server">
                <td class="style51">
                    <asp:Label ID="Label29" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Photo No"></asp:Label>
                </td>
                <td class="style52" colspan="4">
                    <asp:TextBox ID="txt_Hold_PhotoNo" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="50" 
                         ToolTip="Enter Photo No" 
                        Width="200px"></asp:TextBox>
                 </td>
                <td class="style52" colspan="8">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_PHOTO_ALBUM_NO" runat ="server">
                <td class="style51">
                    <asp:Label ID="Label31" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Photo Album No"></asp:Label>
                </td>
                <td class="style52" colspan="4">
                    <asp:TextBox ID="txt_Hold_PhotoAlbumNo" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="150" 
                         ToolTip="Enter Photo Album No" 
                        Width="200px"></asp:TextBox>
                 </td>
                <td class="style52" colspan="8">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_PHOTO_OCASION" runat ="server">
                <td class="style51">
                    <asp:Label ID="Label32" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Ocasion"></asp:Label>
                </td>
                <td class="style52" colspan="4">
                    <asp:TextBox ID="txt_Hold_Ocasion" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="150" 
                       ToolTip="Enter Ocasion of Photo" 
                        Width="200px"></asp:TextBox>
                 </td>
                <td class="style52" colspan="8">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_IMAGE_VIEW_TYPE" runat ="server">
                <td class="style51">
                    <asp:Label ID="Label34" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Image View Type"></asp:Label>
                </td>
                <td class="style52" colspan="4">
                    <asp:TextBox ID="txt_Hold_ImageViewType" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="50" 
                   
                        ToolTip="Enter Image View Details" Width="200px"></asp:TextBox>
                 </td>
                <td class="style52" colspan="8">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_VIEW_DATE" runat ="server">
                <td class="style51">
                    <asp:Label ID="Label39" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="View Date"></asp:Label>
                </td>
                <td class="style52" colspan="4">
                    <asp:TextBox ID="txt_Hold_ViewDate" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="10" 
                        onkeypress="return DateOnly5 (event)" ToolTip="Click to Select Date" 
                        Width="71px"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="txt_Hold_ViewDate_CalendarExtender" 
                        runat="server" Enabled="True" Format="dd/MM/yyyy" 
                        TargetControlID="txt_Hold_ViewDate">
                    </ajaxToolkit:CalendarExtender>
                    dd/MM/yyyy</td>
                <td class="style52" colspan="8">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_THEME" runat ="server">
                <td class="style51">
                    <asp:Label ID="Label41" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Theme"></asp:Label>
                </td>
                <td class="style52" colspan="4">
                    <asp:TextBox ID="txt_Hold_Theme" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="150" 
                        ToolTip="Enter Theme" 
                        Width="200px"></asp:TextBox>
                 </td>
                <td class="style52" colspan="8">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_STYLE" runat ="server">
                <td class="style51">
                    <asp:Label ID="Label42" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Style"></asp:Label>
                </td>
                <td class="style52" colspan="4">
                    <asp:TextBox ID="txt_Hold_Style" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="150" 
                         ToolTip="Enter Style" 
                        Width="200px"></asp:TextBox>
                 </td>
                <td class="style52" colspan="8">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_CULTURE" runat ="server">
                <td class="style51">
                    <asp:Label ID="Label43" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Culture"></asp:Label>
                </td>
                <td class="style52" colspan="4">
                    <asp:TextBox ID="txt_Hold_Culture" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="150" 
                        ToolTip="Enter Eulture" 
                        Width="200px"></asp:TextBox>
                 </td>
                <td class="style52" colspan="8">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_CURRENT_STIE" runat ="server">
                <td class="style51">
                    <asp:Label ID="Label45" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Current Site"></asp:Label>
                </td>
                <td class="style52" colspan="4">
                    <asp:TextBox ID="txt_Hold_CurrentSite" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="150" 
                        ToolTip="Enter Current Site/Location" 
                        Width="200px"></asp:TextBox>
                 </td>
                <td class="style52" colspan="8">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_CREATION_SITE" runat ="server">
                <td class="style51">
                    <asp:Label ID="Label46" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Creation Site"></asp:Label>
                </td>
                <td class="style52" colspan="4">
                    <asp:TextBox ID="txt_Hold_CreationSite" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="150" 
                       ToolTip="Enter Creation Site/Location" 
                        Width="200px"></asp:TextBox>
                 </td>
                <td class="style52" colspan="8">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_YARNCOUNT" runat ="server">
                <td class="style51">
                    <asp:Label ID="Label51" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Yarn Count"></asp:Label>
                </td>
                <td class="style52" colspan="4">
                    <asp:TextBox ID="txt_Hold_YarnCount" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="150" 
                        ToolTip="Enter Yarn Count" Width="200px"></asp:TextBox>
                 </td>
                <td class="style52" colspan="8">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_MATERIAL_TYPE" runat ="server">
                <td class="style51">
                    <asp:Label ID="Label52" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Materials Type"></asp:Label>
                </td>
                <td class="style52" colspan="4">
                    <asp:TextBox ID="txt_Hold_MaterialsType" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="150" 
                        ToolTip="Enter Material Type" 
                        Width="200px"></asp:TextBox>
                 </td>
                <td class="style52" colspan="8">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_TECHNIQUE" runat ="server">
                <td class="style51">
                    <asp:Label ID="Label53" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Technique"></asp:Label>
                </td>
                <td class="style52" colspan="4">
                    <asp:TextBox ID="txt_Hold_Technique" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="150" 
                        ToolTip="Enter Technique adopted" 
                        Width="200px"></asp:TextBox>
                 </td>
                <td class="style52" colspan="8">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_TECH_DETAILS" runat ="server">
                <td class="style51">
                    <asp:Label ID="Label57" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Technical Details"></asp:Label>
                </td>
                <td class="style52" colspan="4">
                    <asp:TextBox ID="txt_Hold_TechDetails" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="250" 
                        ToolTip="Enter Technical Details" 
                        Width="200px"></asp:TextBox>
                 </td>
                <td class="style52" colspan="8">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_INSCRIPTIONS" runat ="server">
                <td class="style51">
                    <asp:Label ID="Label58" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Inscriptions"></asp:Label>
                </td>
                <td class="style52" colspan="4">
                    <asp:TextBox ID="txt_Hold_Inscription" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="150" 
                        ToolTip="Enter Transcription" 
                        Width="200px"></asp:TextBox>
                 </td>
                <td class="style52" colspan="8">
                    &nbsp;</td>
            </tr>

            <tr id ="TR_DESCRIPTION" runat ="server">
                <td class="style51">
                    <asp:Label ID="Label33" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Description"></asp:Label>
                </td>
                <td class="style52" colspan="4">
                    <asp:TextBox ID="txt_Hold_Description" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="250" 
                        ToolTip="Enter Description" 
                        Width="200px"></asp:TextBox>
                 </td>
                <td class="style52" colspan="8">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_ALTER_DATE" runat ="server">
                <td class="style51">
                    <asp:Label ID="Label59" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Alter Date"></asp:Label>
                </td>
                <td class="style52" colspan="4">
                    <asp:TextBox ID="txt_Hold_AlterDate" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="10" 
                        onkeypress="return DateOnly6 (event)" ToolTip="Click to Select Date" 
                        Width="71px"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="txt_Hold_AlterDate_CalendarExtender" 
                        runat="server" Enabled="True" Format="dd/MM/yyyy" 
                        TargetControlID="txt_Hold_AlterDate">
                    </ajaxToolkit:CalendarExtender>
                    dd/MM/yyyy</td>
                <td class="style52" colspan="8">
                    &nbsp;</td>
            </tr>




             
            <tr>
                <td bgcolor="#336699" class="style56" colspan="13">
                    <asp:Button ID="Hold_Save_Bttn" runat="server" AccessKey="s" CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red" OnClientClick="return valid1();" TabIndex="14" 
                        Text="Save" ToolTip="Press to SAVE Record" Visible="False" Width="74px" />
                    <asp:Button ID="Hold_Update_Bttn" runat="server"  CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red"
                        OnClientClick="return valid1();" TabIndex="14" Text="Update" AccessKey="u" 
                        Width="74px" ToolTip="Press to UPDATE Record" Visible="False" />
                    <asp:Button ID="Hold_Cancel_Bttn" runat="server"  CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red" 
                        TabIndex="15" Text="Cancel" Width="71px" AccessKey="c" 
                        ToolTip="Press to Cancel the process"/>
                </td>
            </tr>
            
            <tr>
                <td  bgcolor="#99CCFF" class="style44" colspan="13" rowspan="0">
                     <strong>*<span class="style48"> Mandatory Fields</span></strong></td>
            </tr>


             

        </table>





         <table id="Table5" runat="server" cellspacing="2" border="1"  cellpadding="2" class="style35">
         <tr>
             <td bgcolor="#336699" class="style56" colspan="5">
                 <asp:Label ID="Label16" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="White">Display Holdings/Copies Record(s)</asp:Label>
             </td>
         </tr>
         <tr>
             <td bgcolor="#336699" class="style56" colspan="5">
                 <asp:Label ID="Label30" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="White">HELP: Click the 'Edit' link from Grid below to Edit Copy Record!</asp:Label>
             </td>
         </tr>
          <tr>
             <td bgcolor="#336699" class="style56" colspan="5">
                 <asp:Button ID="Hold_Delete_Bttn" runat="server" AccessKey="d" 
                     CssClass="styleBttn" Font-Bold="True" ForeColor="Red" TabIndex="14" 
                     Text="Delete Selected Records" ToolTip="Press to UPDATE Record" Visible="False" 
                     Width="180px" />
             </td>
         </tr>
         <tr>

             <td bgcolor="#336699" class="style56" colspan="5">
                 <asp:GridView ID="Grid2" runat="server" AllowPaging="True" allowsorting="True" 
                     AutoGenerateColumns="False" DataKeyNames="HOLD_ID" Font-Bold="True" 
                     Font-Names="Tahoma" Font-Size="8pt" Height="100px" HorizontalAlign="Center" 
                     ShowFooter="True" style="width: 100%;  text-align: center;">
                     <Columns>
                         <asp:TemplateField HeaderText="S.N.">
                             <ItemTemplate>
                                 <asp:Label ID="lblsr" runat="server" CssClass="MBody" SkinID="" width="25px"></asp:Label>
                             </ItemTemplate>
                             <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                             <ItemStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" 
                                 ForeColor="#336699" Width="25px" />
                         </asp:TemplateField>

                          <asp:ButtonField HeaderText="Edit"  Text="Edit" CommandName="Select">
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" />
                            <ItemStyle ForeColor="#CC0000" Width="20px" Font-Bold="true" />
                            </asp:ButtonField>



                         <asp:BoundField DataField="ACCESSION_NO" HeaderText="Accession No" SortExpression="ACCESSION_NO" visible="true">
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small"  Width="150px" />
                            <ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left"  width="150px" />
                         </asp:BoundField>

                         <asp:BoundField DataField="ACCESSION_DATE" HeaderText="Acc.Date" SortExpression="ACCESSION_DATE" DataFormatString="{0:dd/MM/yyyy}"  visible="true">
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="120px" />
                            <ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left" width="120px" />
                         </asp:BoundField>

                         <asp:BoundField DataField="VOL_NO" HeaderText="Vol" visible="true" SortExpression="VOL_NO">
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="100px" />
                            <ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left" width="100px" />
                         </asp:BoundField>

                         <asp:BoundField DataField="CLASS_NO" HeaderText="Class No" visible="true" SortExpression="CLASS_NO">
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="150px" />
                            <ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left" width="150px" />
                         </asp:BoundField>

                         <asp:BoundField DataField="BOOK_NO" HeaderText="Book No" visible="true">
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="100px" />
                            <ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left" width="100px" />
                         </asp:BoundField>

                         <asp:BoundField DataField="PAGINATION" HeaderText="Pages"   visible="true">
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="180px" />
                            <ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left" width="180px" />
                         </asp:BoundField>

                         <asp:BoundField DataField="PHYSICAL_LOCATION" HeaderText="Location" visible="true" SortExpression="PHYSICAL_LOCATION">
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="180px" />
                            <ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left" width="180px" />
                         </asp:BoundField>
                         <asp:BoundField DataField="COLLECTION_TYPE" HeaderText="Collection"  SortExpression="COLLECTION_TYPE" visible="true">
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="100px" />
                            <ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left" width="100px" />
                         </asp:BoundField>
                          <asp:BoundField DataField="LIB_CODE" HeaderText="Library"  SortExpression="LIB_CODE" visible="true">
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="100px" />
                            <ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left" width="100px" />
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
                         Font-Bold="True" Font-Size="Small" ForeColor="White" HorizontalAlign="Center" 
                         VerticalAlign="Middle" />
                     <RowStyle BackColor="#DADADA" BorderColor="Desktop" ForeColor="#3399FF" />
                     <SelectedRowStyle BackColor="#ff9933" BorderColor="SteelBlue" 
                         BorderStyle="Solid" />
                     <HeaderStyle BackColor="Desktop" Font-Bold="True" Font-Italic="False" 
                         Font-Names="Times new roman" Font-Overline="False" Font-Underline="False" 
                         ForeColor="White" Width="90%" />
                     <PagerSettings FirstPageText="First" LastPageText="Last" 
                         Mode="NumericFirstLast" PageButtonCount="10" Position="TopAndBottom" />
                     <AlternatingRowStyle BackColor="#EFEFEF" Font-Names="Tahoma" 
                         ForeColor="#0066FF" />
                 </asp:GridView>

             </td>
         </tr>
     </table>

                

      
   
              </ContentTemplate>
                   <Triggers>
                        <asp:AsyncPostBackTrigger  ControlID="Hold_Save_Bttn"   EventName="Click"   />    
                        <asp:AsyncPostBackTrigger ControlID ="Hold_Update_Bttn"  EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID ="Hold_Cancel_Bttn"  EventName="Click" />                             
                   </Triggers>
                   </asp:UpdatePanel>  
    </div>  

</asp:Content>
