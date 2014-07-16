<%@ Page  Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Budgets.aspx.vb" Inherits="EG4.Budgets" %>

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
        background-color:#336699;
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
           ids = ["MainContent_txt_Budg_Head", "MainContent_txt_Budg_Period", "MainContent_txt_Budg_Remarks"];
           control.makeTransliteratable(ids);
           // Show the transliteration control which can be used to toggle between         
           // English and Hindi and also choose other destination language.         
           control.showControl('translControl');
       }

       google.setOnLoadCallback(onLoad);
                   
           
    </script> 
     <script type ="text/javascript">
         function isNumberKey(evt) {
             var charCode = (evt.which) ? evt.which : event.keyCode
             if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 32) {
                 alert("Please Enter Numeric Characters Only!");
                 return false;
             }

             return true;
         }

   </script>
    <script type ="text/javascript">
        function isCurrencyKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && (charCode < 48 || charCode > 57) && charCode != 32) {
                  alert("Please Enter Numeric with Decimal Characters Only!");
                  document.getElementById("MainContent_txt_Budg_Amount").focus();
                return false;
            }

            return true;
        }

   </script>
    <script language="javascript" type="text/javascript">

           function valid1() {
              
               if (document.getElementById('<%=txt_Budg_Year.ClientID%>').value == "") {
                    alert("Please Enter Budget Year in YYYY Format!");
                    document.getElementById("MainContent_txt_Budg_Year").focus();
                    return (false);
                }

                if (document.getElementById('<%=txt_Budg_Head.ClientID%>').value == "") {
                    alert("Please Enter Budget Head!");
                    document.getElementById("MainContent_txt_Budg_Head").focus();
                    return (false);
                }
                if (document.getElementById('<%=txt_Budg_Amount.ClientID%>').value == "") {
                    alert("Please Enter Amount in Rupees !");
                    document.getElementById("MainContent_txt_Budg_Amount").focus();
                    return (false);
                }
               
                    return (true);
                }
    </script>
    <script language ="javascript" type ="text/javascript" >
        function Select(Select) {

            var grdv = document.getElementById('<%= Grid_Budgets.ClientID %>');
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
      <script type="text/javascript">
          function formfocus() {
              document.getElementById('<%= txt_Budg_Year.ClientID %>').focus();
          }
          window.onload = formfocus;
    </script>
     
         <table id="ADMT1" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
            <tr>
                <td  bgcolor="#003366" class="style43" colspan="2" rowspan="1" style="color: #FFFFFF" ><strong>Manage 
                    Budgets</strong></td>
            </tr>
            <tr>
                <td  bgcolor="#99CCFF"  colspan="2" style="text-align: center">
                     Type in Indian languages (Press Ctrl+g to toggle between English and India Language)
                     <div id='translControl'></div></td>
            </tr>
            
            <tr>                
                <td  align="center" colspan="2">     




                 <ajaxToolkit:Accordion ID="Accordion2" runat="server" CssClass="accordion"  
                        HeaderCssClass="accordionHeader"  
                        HeaderSelectedCssClass="accordionHeaderSelected"  
                        ContentCssClass="accordionContent" Enabled="true" Visible="true" 
                         RequireOpenedPane="false"     SuppressHeaderPostbacks="true"   
                        SelectedIndex="1"   Width="100%" Height="237px"   >  
                            <Panes>  
                                <ajaxToolkit:AccordionPane ID="SearchPane" runat="server" >  
                                    <Header>Click To View / Hide Search Pane</Header>  
                                        <Content>     

                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode = "Conditional">
                    <ContentTemplate>
                     <asp:Label ID="Label1" runat="server" Text="Record(s): "></asp:Label>
                   <div>
                   <asp:Button ID="Delete_Bttn" runat="server"  CssClass="styleBttn" Font-Bold="True" 
                        ForeColor="Red" TabIndex="14" Text="Delete Selected Row(s)" AccessKey="d"    
                            Width="165px" Height="30px" Enabled="false" />
                   </div>
                                           

                   <asp:GridView ID="Grid_Budgets" runat="server" AllowPaging="True" DataKeyNames="BUDG_ID"  
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
                    
                   

                    <asp:BoundField   DataField="BUDG_YEAR" HeaderText="Year" SortExpression="BUDG_YEAR" >                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" />
                            <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="100px" Font-Names="Arial"/>                        
                    </asp:BoundField>
                    <asp:BoundField   DataField="BUDG_HEAD" HeaderText="Budget Head" ReadOnly="True"  
                                SortExpression="BUDG_HEAD">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" Font-Names="Arial"  
                                width="250px" />                        
                    </asp:BoundField>
                     <asp:BoundField   DataField="BUDG_AMOUNT" HeaderText="Budget Amount" 
                                SortExpression="BUDG_AMOUNT" >                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="200px" 
                                Font-Names="Arial"/>                        
                    </asp:BoundField>
                     <asp:BoundField   DataField="BUDG_PERIOD" HeaderText="Period" >                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" 
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
                        <asp:AsyncPostBackTrigger ControlID="Grid_Budgets" EventName="RowCommand" />                                                
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
                     <asp:Label ID="Label6" runat="server" Font-Size="Medium" ForeColor="White" 
                         style="font-weight: 700"></asp:Label>
                     <asp:Label ID="Label8" runat="server" Font-Size="Medium" ForeColor="Yellow" 
                         style="font-weight: 700"></asp:Label>
                 </td>
            </tr>
            
            <tr>
                <td class="style51"> 
                    Budget Year *</td>
                <td class="style54">
                    <asp:TextBox ID="txt_Budg_Year" runat="server" MaxLength="4" ToolTip="Enter Budget Year" Wrap="False"  Font-Bold="True" ForeColor="#0066FF" Height="19px" Width="75px" onkeypress="return isNumberKey(event)"></asp:TextBox>
                     <ajaxToolkit:AutoCompleteExtender 
                        ID="txt_Budg_Year_AutoCompleteExtender" 
                        runat="server" 
                        MinimumPrefixLength="1"
                        EnableCaching="true" 
                        CompletionSetCount="10"
                        ServiceMethod="SearchBudgYear"
                        TargetControlID="txt_Budg_Year" 
                        FirstRowSelected = "false">
                    </ajaxToolkit:AutoCompleteExtender>
                    Enter Year in YYYY Format &nbsp;<asp:Label ID="Label7" runat="server"></asp:Label>
                </td>
               
            </tr>
            <tr>
                <td class="style51">Budget Head *</td>
                <td class="style53">
                    <asp:TextBox ID="txt_Budg_Head" runat="server" MaxLength="150"  
                        ToolTip="Type Budget Head" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="672px"  
                        AutoCompleteType="DisplayName" Height="18px"></asp:TextBox>
                        <ajaxToolkit:AutoCompleteExtender 
                        ID="txt_Budg_Head_AutoCompleteExtender" 
                        runat="server" 
                        MinimumPrefixLength="1"
                        EnableCaching="true" 
                        CompletionSetCount="10"
                        ServiceMethod="SearchBudgHead"
                        TargetControlID="txt_Budg_Head" 
                        FirstRowSelected = "false">
                    </ajaxToolkit:AutoCompleteExtender>
                    </td>
            </tr>
            <tr>
                <td class="style51">Period</td>
                <td class="style52">
                     <asp:TextBox ID="txt_Budg_Period" runat="server" MaxLength="100"  
                        ToolTip="Enter Budget Period" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Height="19px" Width="672px"></asp:TextBox>
                 </td>
            </tr>
            <tr>
                <td class="style51">Amount (In Rs)*</td>
                <td class="style52">
                     <asp:TextBox ID="txt_Budg_Amount" runat="server" MaxLength="12"  
                        ToolTip="Enter Amount Allocated" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Height="19px" Width="672px" onkeypress="return isCurrencyKey(event)"></asp:TextBox>                   
                    </td>
            </tr>
            <tr>
                <td class="style51">Remarks</td>
                <td class="style52">
                    <asp:TextBox ID="txt_Budg_Remarks" runat="server" MaxLength="150"  
                        ToolTip="Enter Remarks" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="672px" Height="62px" 
                        TextMode="MultiLine"></asp:TextBox>
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
