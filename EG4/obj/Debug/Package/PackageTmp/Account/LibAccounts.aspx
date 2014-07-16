<%@ Page  Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="LibAccounts.aspx.vb" Inherits="EG4.LibAccounts" %>



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
            height: 26px;
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
           ids = ["MainContent_txt_LibName", "MainContent_txt_ParentBody", "MainContent_txt_LibAdd", "MainContent_txt_LibCity", "MainContent_txt_LibDist", "MainContent_txt_LibState", "MainContent_txt_Search"];
           control.makeTransliteratable(ids);
           // Show the transliteration control which can be used to toggle between         
           // English and Hindi and also choose other destination language.         
           control.showControl('translControl');
       }

       google.setOnLoadCallback(onLoad);
                   
           
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
                document.getElementById("MainContent_txt_LibCode").focus();
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
                         document.getElementById("MainContent_txt_LibCode").focus();
                         return (false);
                     }
                 }
                 else {
                     alert("Please Enter ENG Only Characters!");
                     document.getElementById("MainContent_txt_LibCode").focus();
                     return (false);
                 }
             }
             else {
                 alert("Please Enter ENG Only Characters!");
                 document.getElementById("MainContent_txt_LibCode").focus();
                 return (false);
             }
         }
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
                document.getElementById("MainContent_txt_LibCode").focus();
                return (false);
            }
        }
    </script>
  <script language="javascript" type="text/javascript">

    //function to clear dropdown
      function ddlclr() {
          if (document.getElementById('<%=DropDownList1.ClientID%>').value == "M") {
              document.getElementById('<%=Drop_Libraries.ClientID%>').value = ""
              document.getElementById('<%=DropDownList1.ClientID%>').value = "M"
          }
          else {
              document.getElementById('<%=DropDownList1.ClientID%>').value = "B"
          }

      
      }


      function valid1() {
          var libcode = "";
          var libname = "";
          var parent = "";
          var libtype = "";

          libcode = document.getElementById('<%=txt_LibCode.ClientID%>').value;
          libname = document.getElementById('<%=txt_LibName.ClientID%>').value;
          parent = document.getElementById('<%=txt_ParentBody.ClientID%>').value;
          libtype = document.getElementById('<%=DropDownList1.ClientID%>').value;

          if (libcode == "") {
              alert("Please enter proper \"Library Code\" field.");
              document.getElementById("MainContent_txt_LibCode").focus();
              return (false);
          }


          if (document.getElementById('<%=txt_LibCode.ClientID%>').value.length < 5) {
              alert("Length of \"Library Code\" should be Min 5 characters.");
              document.getElementById("MainContent_txt_LibCode").focus();
              return (false);
          }
          if (document.getElementById('<%=txt_LibCode.ClientID%>').value.length > 10) {
              alert("Length of \"Library Code\" should be Max 10 characters.");
              document.getElementById("MainContent_txt_LibCode").focus();
              return (false);
          }
          if (document.getElementById('<%=txt_LibName.ClientID%>').value == "") {
              alert("Please enter proper \"Library Name\" field.");
              document.getElementById("MainContent_txt_LibName").focus();
              return (false);
          }
          if (document.getElementById('<%=txt_ParentBody.ClientID%>').value == "") {
              alert("Please enter proper \"Parent Organization Name\" field.");
              document.getElementById("MainContent_txt_ParentBody").focus();
              return (false);
          }

          if (libtype == "") {
              alert("Please Seledt  \"Library Type \" field.");
              document.getElementById("DropDownList1").focus();
              return (false);
          }

          
          if (libtype == "B") {
              var s;
              s = document.getElementById('<%=Drop_Libraries.ClientID%>').value;
              if (s == "") {
                  alert("Please Select  \"Main Library  \" field.");
                  document.getElementById("MainContent_Drop_Libraries").focus();
                  return (false);
              }
          }

          
          else {
             
              return (true);
          }
          //return (false);
      }
    </script>
     <script language ="javascript" type ="text/javascript" >
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
  
         <table id="ADMT1" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
            <tr>
                <td  bgcolor="#003366" class="style43" colspan="2" rowspan="1" style="color: #FFFFFF"><strong>Manage Library Accounts</strong></td>
            </tr>
            <tr>
                <td  bgcolor="#99CCFF"  colspan="2" style="text-align: center">
                     Type in Indian languages (Press Ctrl+g to toggle between English and India Language)
                     <div id='translControl'></div></td>
            </tr>
            <tr>
                 <td  bgcolor="#99CCFF"  colspan="2" style="text-align: center">
                     <asp:Label ID="Label5" runat="server" Text="Record(s)"></asp:Label>
                 </td>
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
                
                        
                    Search Text&nbsp;
                    <asp:TextBox ID="txt_Search" runat="server" MaxLength="100"  
                        ToolTip="Enter search Term" Wrap="False" AccessKey="r" 
                        Font-Bold="True" ForeColor="#0066FF" Width="250px"></asp:TextBox>
                    &nbsp;In
                    <asp:DropDownList ID="DropDownList2" runat="server" ForeColor="#0066FF" 
                        onchange="ddlclr()"  >
                        <asp:ListItem Value="LIB_CODE">Library Code</asp:ListItem>
                        <asp:ListItem Selected="True" Value="LIB_NAME">Library Name</asp:ListItem>
                        <asp:ListItem Value="PARENT_BODY">Organization</asp:ListItem>
                        <asp:ListItem Value="LIB_CITY">City</asp:ListItem>
                        <asp:ListItem Value="LIB_STATE">State</asp:ListItem>
                    </asp:DropDownList>&nbsp;with
                    <asp:DropDownList ID="DropDownList3" runat="server" ForeColor="#0066FF" 
                        onchange="ddlclr()"  >
                        <asp:ListItem Value="AND">All Words</asp:ListItem>
                        <asp:ListItem Value="OR">Any Word</asp:ListItem>
                        <asp:ListItem Selected="True" Value="LIKE">Like</asp:ListItem>
                        <asp:ListItem Value="SW">Start With</asp:ListItem>
                        <asp:ListItem Value="EW">End With</asp:ListItem>
                    </asp:DropDownList>&nbsp;Library Status
                    <asp:DropDownList ID="DropDownList4" runat="server" ForeColor="#0066FF" 
                        onchange="ddlclr()"  >
                        <asp:ListItem Selected="True" Value="CU">Current</asp:ListItem>
                        <asp:ListItem Value="CL">Closed</asp:ListItem>
                    </asp:DropDownList>&nbsp;<asp:Button ID="Search_Bttn" runat="server"  CssClass="styleBttn" Font-Bold="True" 
                        ForeColor="Red" TabIndex="14" Text="Search" AccessKey="s"     Width="74px" />
                    <br />
                    <br />
                    <hr />

                    
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                    <ContentTemplate>               
                        <asp:Panel ID="Panel1" runat="server" Height="250px" ScrollBars="Auto">
                        
                   <asp:GridView ID="Grid1" runat="server" AllowPaging="True" DataKeyNames="LIB_CODE"  
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
                            <ItemStyle ForeColor="#CC0000" Width="30px" Font-Bold="true" />
                            </asp:ButtonField>
                    
                   

                    <asp:BoundField   DataField="LIB_CODE" HeaderText="Library Code" 
                                SortExpression="LIB_CODE" >                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="450px" 
                                Font-Names="Arial"/>                        
                    </asp:BoundField>
                    <asp:BoundField   DataField="LIB_NAME" HeaderText="Library Name" ReadOnly="True"  
                                SortExpression="LIB_NAME">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" Font-Names="Arial"  
                                width="150px" />                        
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
                   
                   </asp:Panel>
                   

                   </ContentTemplate>
                   <Triggers>
                          <asp:AsyncPostBackTrigger ControlID="Search_Bttn" EventName="Click" />                                              
                   </Triggers>
                    </asp:UpdatePanel>

                   

                           
                            </Content>  
                        </ajaxToolkit:AccordionPane>  
                    </Panes>
                </ajaxToolkit:Accordion>
                       
                                                                 
                </td>

            </tr>
            
            
            
        </table>


         <asp:UpdatePanel ID="UpdatePanel2" runat="server">
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
                    <asp:Label ID="lbl_LibCode" runat="server" Text="Library Code *"></asp:Label>
                </td>
                <td class="style52">
                    <asp:TextBox ID="txt_LibCode" runat="server" MaxLength="10"  
                        ToolTip="Enter Distinct Library Code, Max 10 Chr, Alpha, ENG Only." Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" style="text-transform: uppercase" 
                        Height="20px" Width="96px" onkeypress="return EngOnly (event)"></asp:TextBox>
                &nbsp;5-10 Chrs Length, Alpha, ENG Only, Distinct Library Code.
                    </td>
            </tr>
            <tr>
                <td class="style51">Library Name*</td>
                <td class="style52">
                    <asp:TextBox ID="txt_LibName" runat="server" MaxLength="250"  
                        ToolTip="Enter Library Name" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="465px"  
                        AutoCompleteType="DisplayName" Height="22px"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td class="style51">Parent Organization*</td>
                <td class="style52">
                    <asp:TextBox ID="txt_ParentBody" runat="server" MaxLength="250"  
                        ToolTip="Enter Organization Name" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="465px" Height="23px"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td class="style51">Address</td>
                <td class="style52">
                    <asp:TextBox ID="txt_LibAdd" runat="server" MaxLength="250"  
                        ToolTip="Enter Address" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="465px" Height="58px" 
                        TextMode="MultiLine" Font-Names="Arial"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style51">City/Town/Village </td>
                <td class="style52">
                    <asp:TextBox ID="txt_LibCity" runat="server" MaxLength="100"  
                        ToolTip="Enter City Name" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="250px"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td class="style51">District</td>
                <td class="style52">
                    <asp:TextBox ID="txt_LibDist" runat="server" MaxLength="100"  
                        ToolTip="Enter District Name" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="250px"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td class="style51">State </td>
                <td class="style52">
                    <asp:TextBox ID="txt_LibState" runat="server" MaxLength="100"  
                        ToolTip="Enter State Name" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="250px"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td class="style51">Library Type *</td>
                <td class="style52">
                    <asp:DropDownList ID="DropDownList1" runat="server" ForeColor="#0066FF" 
                        onchange="ddlclr()" AutoPostBack="True"  >
                        <asp:ListItem Value="M">Main</asp:ListItem>
                        <asp:ListItem Value="B">Branch</asp:ListItem>
                    </asp:DropDownList>&nbsp; 
                    <asp:Label ID="lbl_LibCode0" runat="server" Text="Select Parent Library "></asp:Label>
                    <asp:DropDownList ID="Drop_Libraries" runat="server" ForeColor="#0066FF">
                    </asp:DropDownList>
                &nbsp;</td>
            </tr>
            <tr id="tr_status" runat="server">
                <td class="style51">Status</td>
                <td class="style52">
                    <asp:RadioButton ID="RB_Current" runat="server" Text="Current" GroupName="ss" />
                    <asp:RadioButton ID="RB_Closed" runat="server" Text="Closed" GroupName="ss" />
                </td>
            </tr>
            <tr>
                <td class="style47" colspan="2">
                    <asp:Button ID="bttn_Save" runat="server"  CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red"
                        OnClientClick="return valid1();" TabIndex="14" Text="Save" AccessKey="s" 
                        Width="74px" Visible="False" />
                    <asp:Button ID="Cancel" runat="server"  CssClass="styleBttn" Font-Bold="True" ForeColor="Red" 
                        TabIndex="15" Text="Cancel" Width="71px" AccessKey="c"/>
                    <asp:Button ID="bttn_Update" runat="server"  CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red"
                        OnClientClick="return valid1();" TabIndex="14" Text="Update" AccessKey="s" 
                        Width="74px" Visible="False" />
                </td>
            </tr>
            
            <tr>
                <td  bgcolor="#99CCFF" class="style44" colspan="2" rowspan="0">
                     <strong>*<span class="style48"> Mandatory Fields</span></strong></td>
            </tr>


             

        </table>

      
   
                   </ContentTemplate>
                   <Triggers>
                        <asp:PostBackTrigger  ControlID="bttn_Save"   />    
                        <asp:PostBackTrigger ControlID ="bttn_Update"  />                                
                   </Triggers>
                    </asp:UpdatePanel>
       
        
</asp:Content>
