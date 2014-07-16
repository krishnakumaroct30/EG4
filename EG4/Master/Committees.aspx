<%@ Page  Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Committees.aspx.vb" Inherits="EG4.Committees" SmartNavigation ="true" MaintainScrollPositionOnPostback="true"  %>


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
           ids = ["MainContent_txt_Com_Name", "MainContent_txt_Com_Chairman", "MainContent_txt_Com_Members", "MainContent_txt_Com_Remarks"];
           control.makeTransliteratable(ids);
           // Show the transliteration control which can be used to toggle between         
           // English and Hindi and also choose other destination language.         
           control.showControl('translControl');
       }

       google.setOnLoadCallback(onLoad);
           
    </script>
    
     
    <script language="javascript" type="text/javascript">

           function valid1() {
               var code = "";
               var email = "";

               code = document.getElementById('<%=txt_Com_Code.ClientID%>').value;
               email = document.getElementById('<%=txt_Com_Email.ClientID%>').value;

               if (code == "") {
                   alert("Please enter proper and distinct  \"committee code\" field.");
                   document.getElementById("MainContent_txt_Com_Code").focus();
                   return (false);
               }
               if (document.getElementById('<%=txt_Com_Code.ClientID%>').value.length < 2) {
                   alert("Length of \"Committee Code\" should be Min 2 characters.");
                   document.getElementById("MainContent_txt_Com_Code").focus();
                   return (false);
               }
               if (document.getElementById('<%=txt_Com_Code.ClientID%>').value.length > 10) {
                   alert("Length of \"Committee Code\" should be Max 10 characters.");
                   document.getElementById("MainContent_txt_Com_Code").focus();
                   return (false);
               }

               if (document.getElementById('<%=txt_Com_Name.ClientID%>').value == "") {
                    alert("Please enter Committee Name...");
                    document.getElementById("MainContent_txt_Com_Name").focus();
                    return (false);
                }

                if (document.getElementById('<%=txt_Com_Chairman.ClientID%>').value == "") {
                    alert("Please enter Committee Chairman Name...");
                    document.getElementById("MainContent_txt_Com_Chairman").focus();
                    return (false);
                }
                if (document.getElementById('<%=txt_Com_Members.ClientID%>').value == "") {
                    alert("Please enter Members of the Committeee...");
                    document.getElementById("MainContent_txt_Com_Members").focus();
                    return (false);
                }
                if (document.getElementById('<%=txt_Com_Email.ClientID%>').value.length > 0) {
                    re = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
                    if (!re.test(document.getElementById('<%=txt_Com_Email.ClientID%>').value)) {
                        alert("Error: Not a proper mail address!");
                        document.getElementById("MainContent_txt_Com_Email").focus();
                        return false;
                    }
                }

                if (document.getElementById('<%=txt_Com_SDate.ClientID%>').value == "") {
                    alert("Please enter Start Date of the Committeee...");
                    document.getElementById("MainContent_txt_Com_SDate").focus();
                    return (false);
                }
                if (document.getElementById('<%=txt_Com_EDate.ClientID%>').value == "") {
                    alert("Please enter Close Date of the Committeee...");
                    document.getElementById("MainContent_txt_Com_EDate").focus();
                    return (false);
                }
               

                    return (true);
                }
    </script>
    <script language ="javascript" type ="text/javascript" >
        function Select(Select) {

            var grdv = document.getElementById('<%= Grid_Committee.ClientID %>');
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
    
     <script type ="text/javascript">
         function suppressNonEng(event) {
             var chCode = ('charCode' in event) ? event.charCode : event.keyCode;
             if (49 <= chCode && chCode <= 57) {
                 return (true);
             }
             if (97 <= chCode && chCode <= 122) {
                 return (true);
             }

              if ( chCode == 0 || chCode == 13 || chCode == 32) {
                      return (true);
                  }

             else {
                 alert("Please Enter ENG Only Characters!");
                 document.getElementById("MainContent_txt_Com_Code").focus();
                 return (false);
             }
         }
    </script>

     

         <table id="ADMT1" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
            <tr>
                <td  bgcolor="#003366" class="style43" colspan="2" rowspan="1" style="color: #FFFFFF"><strong>Library 
                    Committees</strong></td>
            </tr>
            <tr>
                <td  bgcolor="#99CCFF"  colspan="2" style="text-align: center">
                     Type in Indian languages (Press Ctrl+g to toggle between English and India Language)
                     <div id='translControl'></div></td>
            </tr>
            
            <tr>                
                <td  align="center" colspan="2">     

                   
                    <hr />

                      <ajaxToolkit:Accordion ID="Accordion2" runat="server" CssClass="accordion"  
                        HeaderCssClass="accordionHeader"  
                        HeaderSelectedCssClass="accordionHeaderSelected"  
                        ContentCssClass="accordionContent" Enabled="true" Visible="true" 
                         RequireOpenedPane="false"     SuppressHeaderPostbacks="true"   
                        SelectedIndex="1"   Width="100%" Height="237px"  >  
                            <Panes>  
                                <ajaxToolkit:AccordionPane ID="SearchPane" runat="server" >  
                                    <Header>Click To View / Hide Search Pane</Header>  
                                        <Content>     

                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                     <asp:Label ID="Label1" runat="server" Text="Record(s): "></asp:Label>
                   <div>
                   <asp:Button ID="Delete_Bttn" runat="server"  CssClass="styleBttn" Font-Bold="True" 
                        ForeColor="Red" TabIndex="14" Text="Delete Selected Row(s)" AccessKey="d"    
                            Width="165px" Height="30px" Enabled="false" />
                   </div>
                                           

                   <asp:GridView ID="Grid_Committee" runat="server" AllowPaging="True" DataKeyNames="COM_ID"  
                        style="width: 100%;  text-align: center;" allowsorting="True" 
                        AutoGenerateColumns="False" PageSize="100"  Font-Bold="True" 
                        Font-Names="Tahoma" Font-Size="8pt" Height="100px"  
                        HorizontalAlign="Center"  Width="6px" ShowFooter="True">
                        <Columns >                
                            <asp:TemplateField HeaderText="S.N.">
                                <ItemTemplate>
                                    <asp:Label ID="lblsr" runat="server" CssClass="MBody"  SkinID="" width="25px"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                                <ItemStyle ForeColor="#336699" Font-Names="Tahoma" Width="20px" 
                                    Font-Bold="True" Font-Size="Small" />
                            </asp:TemplateField>
                    
                   

                            <asp:ButtonField HeaderText="Edit"  Text="Edit" CommandName="Select">
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" />
                            <ItemStyle ForeColor="#CC0000" Width="20px" Font-Bold="true" />
                            </asp:ButtonField>
                    
                   

                    <asp:BoundField   DataField="COM_CODE" HeaderText="Code" 
                                SortExpression="COM_CODE" >                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="100px" 
                                Font-Names="Arial"/>                        
                    </asp:BoundField>
                    <asp:BoundField   DataField="COM_NAME" HeaderText="Committee Name" ReadOnly="True"  
                                SortExpression="COM_NAME">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" Font-Names="Arial"  
                                width="350px" />                        
                    </asp:BoundField>
                     <asp:BoundField   DataField="COM_CHAIRMAN" HeaderText="Chairman" 
                                SortExpression="COM_CHAIRMAN" >                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" 
                                Font-Names="Arial"/>                        
                    </asp:BoundField>
                     <asp:BoundField   DataField="COM_MEMB" HeaderText="Members" >                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" 
                                Font-Names="Arial"/>                        
                    </asp:BoundField>
                      <asp:BoundField   DataField="COM_ID" HeaderText="ID" visible="true">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="10px" 
                                Font-Names="Arial"/>                        
                    </asp:BoundField>
                     
                    <asp:TemplateField  ControlStyle-Width="10px"  HeaderText="Delete" FooterText="Select to Delete" ShowHeader="true" >
                        <HeaderTemplate>
                            <asp:ImageButton ID="ImageButton1" runat="server" Height="16px" Width="16px" ToolTip="Select All" ImageUrl="~/Images/check_all.gif" onClientclick="return Select(true)"  />
                            <asp:ImageButton ID="ImageButton2" runat="server" Height="16px" Width="16px" ToolTip="Deselect All" ImageUrl="~/Images/uncheck_all.gif"  OnClientClick ="return Select(false)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cbd"  runat="server" />
                        </ItemTemplate>

                        <ControlStyle Width="50px"></ControlStyle>
                    </asp:TemplateField>
                    
                    
                </Columns>
                    
                    <PagerStyle BackColor="#3399FF" BorderColor="#C0C000" BorderStyle="Solid" 
                        Font-Bold="True" ForeColor="White" HorizontalAlign="Center" 
                        VerticalAlign="Middle" Font-Size="Small" />
                    <RowStyle BackColor="#DADADA" BorderColor="Desktop" ForeColor="#3399FF" />
                    <SelectedRowStyle BackColor="Desktop" BorderColor="SteelBlue" BorderStyle="Solid" />
                    <HeaderStyle BackColor="Desktop" Font-Bold="True" Font-Italic="False" Font-Names="Times new roman"
                        Font-Overline="False" Font-Underline="False" ForeColor="White" Width="80%" />
                    <PagerSettings Position="TopAndBottom" FirstPageText="First" LastPageText="Last" PageButtonCount="20" Mode="NumericFirstLast" />
                    <AlternatingRowStyle BackColor="#EFEFEF" Font-Names="Tahoma" ForeColor="#0066FF" />
                </asp:GridView>
                   

                   

                   </ContentTemplate>
                   <Triggers>
                         <asp:AsyncPostBackTrigger ControlID="Delete_Bttn" EventName="Click" />
                         <asp:AsyncPostBackTrigger ControlID="Grid_Committee" EventName="RowCommand" />                                              
                   </Triggers>
                    </asp:UpdatePanel>

                   

                           
                            </Content>  
                        </ajaxToolkit:AccordionPane>  
                    </Panes>
                </ajaxToolkit:Accordion>
                           
                           
                                                                 
                </td>

            </tr>            
        </table>


         <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
              <ContentTemplate>

        
         <table id="Table1" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
             <tr>
                <td class="style47" colspan="2">
                     <asp:Label ID="Label6" runat="server" Font-Size="Medium" ForeColor="#CC3300" 
                         style="font-weight: 700"></asp:Label>
                 </td>
            </tr>
            
            <tr>
                <td class="style51"> 
                    Committe Code*</td>
                <td class="style54">
                    <asp:TextBox ID="txt_Com_Code" runat="server" MaxLength="10"  
                        ToolTip="Enter Committee Code" Wrap="False" style="text-transform: uppercase"
                        Font-Bold="True" ForeColor="#0066FF" Height="21px" Width="75px"  onkeypress="return suppressNonEng (event)"></asp:TextBox>
                    &nbsp; <asp:Label ID="Label7" runat="server"  Text=""></asp:Label> <asp:Label ID="Label2" runat="server"  Text="Enter Distinct Committee Code - ENG &amp; Digits Only; Min 2, Max 10 Char."></asp:Label>
                </td>
               
            </tr>
            <tr>
                <td class="style51"> 
                    Committee Name *</td>
                <td class="style54">
                    <asp:TextBox ID="txt_Com_Name" runat="server" MaxLength="250"  
                        ToolTip="Enter Committee Name" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Height="19px" Width="98%"></asp:TextBox>
                    &nbsp;</td>
               
            </tr>
             <tr>
                <td class="style51"> 
                    Chairman *</td>
                <td class="style54">
                    <asp:TextBox ID="txt_Com_Chairman" runat="server" MaxLength="100"  
                        ToolTip="EnterCommittee Chairman Name" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Height="19px" Width="98%"></asp:TextBox>
                    &nbsp;</td>
               
            </tr>
             <tr>
                <td class="style51"> 
                    Members(;) *</td>
                <td class="style54">
                    <asp:TextBox ID="txt_Com_Members" runat="server" MaxLength="250"  
                        ToolTip="Enter Member Name separated with ; " Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Height="19px" Width="98%"></asp:TextBox>
                    &nbsp;</td>
               
            </tr>
             <tr>
                <td class="style51"> 
                    Committee Mail </td>
                <td class="style54">
                    <asp:TextBox ID="txt_Com_Email" runat="server" MaxLength="200"  
                        ToolTip="Enter Email of Committee" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Height="19px" Width="98%"></asp:TextBox>
                    &nbsp;</td>
               
            </tr>
            <tr>
                <td class="style51">Remarks </td>
                <td class="style53">
                    <asp:TextBox ID="txt_Com_Remarks" runat="server" MaxLength="250"  
                        ToolTip="Enter Remarks" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="98%"  
                        AutoCompleteType="DisplayName" Height="96px" TextMode="MultiLine"></asp:TextBox>
                    </td>
            </tr>
            
             <tr>
                <td class="style51"> 
                    Start Date *</td>
                <td class="style54">
                    <asp:TextBox ID="txt_Com_SDate" runat="server" Height="16px" Width="71px" ForeColor="#0066FF"
                        MaxLength="10" ToolTip="Click to Select Date"></asp:TextBox>
                         <ajaxToolkit:CalendarExtender ID="txt_Com_SDate_CalendarExtender" runat="server" 
                        Enabled="True" TargetControlID="txt_Com_SDate"  Format="dd/MM/yyyy"  > 
                    </ajaxToolkit:CalendarExtender>
                    </td>
               
            </tr>
            <tr>
                <td class="style51"> 
                    End Date *</td>
                <td class="style54">
                    <asp:TextBox ID="txt_Com_EDate" runat="server" Height="16px" Width="71px" ForeColor="#0066FF"
                        MaxLength="10" ToolTip="Click to Select End Date "></asp:TextBox>
                         <ajaxToolkit:CalendarExtender ID="txt_Com_EDate_CalendarExtender1" runat="server" 
                        Enabled="True" TargetControlID="txt_Com_EDate"  Format="dd/MM/yyyy" > 
                    </ajaxToolkit:CalendarExtender>
                    
                    <asp:Label ID="Label8" runat="server" Text="Committee Close Date"></asp:Label>
                    
                    </td>
               
            </tr>
            <tr>
                <td class="style47" colspan="2">
                    <asp:Button ID="bttn_Save" runat="server" AccessKey="s" CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red" OnClientClick="return valid1();" TabIndex="14" 
                        Text="Save" ToolTip="Press to SAVE Record" Visible="False" Width="74px" />
                    <asp:Button ID="bttn_Update" runat="server"  CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red"
                        OnClientClick="return valid1();" TabIndex="14" Text="Update" AccessKey="s" 
                        Width="74px" ToolTip="Press to UPDATE Record" />
                    <asp:Button ID="Cancel" runat="server"  CssClass="styleBttn" Font-Bold="True" ForeColor="Red" 
                        TabIndex="15" Text="Cancel" Width="71px" AccessKey="c" 
                        ToolTip="Press to Cancel the process"/>
                </td>
            </tr>
            
            <tr>
                <td  bgcolor="#99CCFF" class="style44" colspan="2" rowspan="0">
                     <strong>*<span class="style48"> Mandatory Fields</span></strong></td>
            </tr>


             

        </table>





      
   
                  </ContentTemplate>
                   <Triggers>
                        <asp:AsyncPostBackTrigger  ControlID="bttn_Save"   EventName="Click"   />    
                        <asp:AsyncPostBackTrigger ControlID ="bttn_Update"  EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID ="Cancel"  EventName="Click" />                             
                   </Triggers>
                    </asp:UpdatePanel>
        
</asp:Content>
