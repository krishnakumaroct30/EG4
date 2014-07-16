<%@ Page  Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Receipts.aspx.vb" Inherits="EG4.Receipts"%>


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
            width: 15%;
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
            width: 100%;
        }
          
           .style56
        {
            text-align: center;
            width: 100%;
            background-color:#336699;
                      
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
            font-weight:bold;  
            font-family:Arial;
            background-color:Silver;  
            height:20px;    
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
           ids = ["MainContent_txt_Rect_Remarks"];
           control.makeTransliteratable(ids);
           // Show the transliteration control which can be used to toggle between         
           // English and Hindi and also choose other destination language.         
           control.showControl('translControl');
       }

       google.setOnLoadCallback(onLoad);
           
    </script>
    
    <script type="text/javascript">
//        function formfocus() {
//            document.getElementById('<%= DDL_Members.ClientID %>').focus();
//        }
//        window.onload = formfocus;
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

     <script language ="javascript" type ="text/javascript" >
         function Select2(Select2) {

             var grdv = document.getElementById('<%= Grid1_Search.ClientID %>');
             var chbk = "cbd";

             var Inputs = grdv.getElementsByTagName("input");

             for (var n = 0; n < Inputs.length; ++n) {
                 if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(chbk, 0) >= 0) {
                     Inputs[n].checked = Select2;
                 }
             }
             return false;
         }

    </script> 
         
      <script language="javascript" type="text/javascript">

          function valid1() {
             
              if (document.getElementById('<%=DDL_Members.ClientID%>').value == "") {
                  alert("Please Select \"Member\" From Drop-Down.");
                  document.getElementById("MainContent_DDL_Members").focus();
                  return (false);
              }
              if (document.getElementById('<%=txt_Rect_Date.ClientID%>').value == "") {
                  alert("Please enter proper \"Receipt Date\" field.");
                  document.getElementById("MainContent_txt_Rect_Date").focus();
                  return (false);
              }
              if (document.getElementById('<%=txt_Rect_Date.ClientID%>').value.length === 8) {
                  alert("Plz Enter \"Receipt Date\" in dd/MM/yyyy Format.");
                  document.getElementById("MainContent_txt_Rect_Date").focus();
                  return (false);
              }

              return (true);
          }

    </script>

    <script language="javascript" type="text/javascript">

        function valid2() {

            if (document.getElementById('<%=DDL_Members.ClientID%>').value == "") {
                alert("Please Select \"Member\" From Drop-Down.");
                document.getElementById("MainContent_DDL_Members").focus();
                return (false);
            }
            if (document.getElementById('<%=txt_Rect_Date.ClientID%>').value == "") {
                alert("Please enter proper \"Receipt Date\" field.");
                document.getElementById("MainContent_txt_Rect_Date").focus();
                return (false);
            }
            if (document.getElementById('<%=txt_Rect_Date.ClientID%>').value.length === 8) {
                alert("Plz Enter \"Receipt Date\" in dd/MM/yyyy Format.");
                document.getElementById("MainContent_txt_Rect_Date").focus();
                return (false);
            }

            return (true);
        }

    </script>
     <script type ="text/javascript">
         function isNumberKey(evt) {
             var charCode = (evt.which) ? evt.which : event.keyCode
             if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                 alert("Please Enter Numeric Characters Only!");
                 return false;
             }

             return true;
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
                document.getElementById("MainContent_txt_Rect_Date").focus();
                return (false);
            }
        }
    </script>
    <script type ="text/javascript">
        function isCurrencyKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && (charCode < 48 || charCode > 57) && charCode != 32) {
                alert("Please Enter Numeric with Decimal Characters Only!");
                return false;
            }

            return true;
        }

   </script>

    <div>
    </div>
         <table id="ADMT1" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
            <tr>
                <td  bgcolor="#003366" class="style43" colspan="2" rowspan="1" style="color: #FFFFFF"><strong>Payment Receipt Manager 
                    </strong></td>
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
                
                    Select Parameter from below and press SEARCH button
                    <br />    
                    Rect.ID<asp:DropDownList ID="DDL_Receipts" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC"  AutoPostBack="True" 
                        ToolTip="Plz Select Receipt ID from Here">
                    </asp:DropDownList>
                    <ajaxToolkit:ListSearchExtender ID="DDL_Receipts_ListSearchExtender" 
                        runat="server" Enabled="True" TargetControlID="DDL_Receipts" 
                        PromptText="Type to search" PromptCssClass="PromptCSS">
                    </ajaxToolkit:ListSearchExtender>

                    &nbsp;Member Name:               
                
                        
                    <asp:DropDownList ID="DDL_Members2" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC"  AutoPostBack="True">
                    </asp:DropDownList>
                    <ajaxToolkit:ListSearchExtender ID="DDL_Members2_ListSearchExtender" 
                        runat="server" Enabled="True" TargetControlID="DDL_Members2" 
                        PromptText="Type to search" PromptCssClass="PromptCSS">
                    </ajaxToolkit:ListSearchExtender>
                    &nbsp;Accession No:               
              
                    <asp:DropDownList ID="DDL_AccessionNo2" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC"  AutoPostBack="True" 
                        ToolTip="Plz Select Accession No from here">
                    </asp:DropDownList>
                    <br />
                    <ajaxToolkit:ListSearchExtender ID="DDL_AccessionNo2_ListSearchExtender" 
                        runat="server" Enabled="True" TargetControlID="DDL_AccessionNo2" 
                        PromptText="Type to search" PromptCssClass="PromptCSS">
                    </ajaxToolkit:ListSearchExtender>

                    &nbsp;Copy ID:               
                
                        
                    <asp:DropDownList ID="DDL_CopyID2" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC"  AutoPostBack="True" 
                        ToolTip="Plz Select Copy ID from Here">
                    </asp:DropDownList>
                    <ajaxToolkit:ListSearchExtender ID="DDL_CopyID2_ListSearchExtender" 
                        runat="server" Enabled="True" TargetControlID="DDL_CopyID2" 
                        PromptText="Type to search" PromptCssClass="PromptCSS">
                    </ajaxToolkit:ListSearchExtender>

                    &nbsp;Payment Recd For:               
                
                        
                    <asp:DropDownList ID="DDL_PmtFor2" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC"  AutoPostBack="True">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem Value="N">New Member</asp:ListItem>
                        <asp:ListItem Value="R">Membership Renewal</asp:ListItem>
                        <asp:ListItem Value="F">Overdue Fine</asp:ListItem>
                        <asp:ListItem Value="L">Lost/Damage Book</asp:ListItem>
                        <asp:ListItem Value="S">Security Deposit</asp:ListItem>
                        <asp:ListItem Value="O">Others</asp:ListItem>
                    </asp:DropDownList>
                    
                    &nbsp; Status:               
                                      
                    <asp:DropDownList ID="DDL_Status" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC"  AutoPostBack="True" 
                        ToolTip="Select Status of Receipt">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem>Paid</asp:ListItem>
                        <asp:ListItem>Pending</asp:ListItem>
                    </asp:DropDownList>
                    <br />
                    Payment Date FROM:
                    <asp:TextBox ID="txt_Rect_SDate" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="10" 
                        onkeypress="return DateOnly (event)" ToolTip="Click to Select Date" 
                        Width="71px"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="txt_Rect_SDate_CalendarExtender" 
                        runat="server" Enabled="True" Format="dd/MM/yyyy" 
                        TargetControlID="txt_Rect_SDate">
                    </ajaxToolkit:CalendarExtender>
                     TO:
                     <asp:TextBox ID="txt_Rect_EDate" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="10" 
                        onkeypress="return DateOnly (event)" ToolTip="Click to Select Date" 
                        Width="71px"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="txt_Rect_EDate_CalendarExtender" 
                        runat="server" Enabled="True" Format="dd/MM/yyyy" 
                        TargetControlID="txt_Rect_EDate">
                    </ajaxToolkit:CalendarExtender>
                    <br />
                    &nbsp;<asp:Button ID="Search_Bttn" runat="server"  CssClass="styleBttn" Font-Bold="True" 
                        ForeColor="Red" TabIndex="14" Text="Search" AccessKey="s"     Width="74px" />
                    <br />
                      <hr />

                    
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                     <asp:Label ID="Label7" runat="server" Text="Record(s): "></asp:Label>
                   <div>
                   <asp:Button ID="Delete_Bttn" runat="server"  CssClass="styleBttn" Font-Bold="True" 
                        ForeColor="Red" TabIndex="14" Text="Delete Selected Row(s)" AccessKey="d"    
                            Width="165px" Height="30px" Enabled="false" />
                   </div>
                                           

                   <asp:GridView ID="Grid1_Search" runat="server" AllowPaging="True" DataKeyNames="REC_ID"  
                        style="width: 100%;  text-align: center;" allowsorting="True" 
                        AutoGenerateColumns="False" PageSize="100"  Font-Bold="True" 
                        Font-Names="Tahoma" Font-Size="8pt" Height="100px"  
                        HorizontalAlign="Center"  Width="6px" ShowFooter="True">
                        <Columns >                
                            <asp:TemplateField HeaderText="S.N.">
                                <ItemTemplate>
                                    <asp:Label ID="lblsr" runat="server" CssClass="MBody"  Text = '<%# Container.dataitemindex+1 %>' SkinID="" width="25px"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                                <ItemStyle ForeColor="#336699" Font-Names="Tahoma" Width="20px" 
                                    Font-Bold="True" Font-Size="Small" />
                            </asp:TemplateField>
                    
                   

                            <asp:ButtonField HeaderText="Edit"  Text="Edit" CommandName="Select">
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" />
                            <ItemStyle ForeColor="#CC0000" Width="20px" Font-Bold="true" />
                            </asp:ButtonField>
                    
                   

                    <asp:BoundField   DataField="ACCESSION_NO" SortExpression="ACCESSION_NO" HeaderText="Accession No" >                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="150px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" Font-Names="Arial"/>                        
                    </asp:BoundField>

                    <asp:BoundField   DataField="COPY_ID" SortExpression="COPY_ID" HeaderText="Loose Issue ID" >                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="100px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="100px" Font-Names="Arial"/>                        
                    </asp:BoundField>

                     <asp:BoundField   DataField="SECURITY_DEPOSIT" SortExpression="SECURITY_DEPOSIT" HeaderText="Security Deposit" >                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="150px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" Font-Names="Arial"/>                        
                    </asp:BoundField>

                    <asp:BoundField   DataField="AMOUNT_DUE" HeaderText="Amount Due" SortExpression="AMOUNT_DUE">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" Width="120px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="120px" Font-Names="Tahoma"/>                        
                    </asp:BoundField>
                                     
                      <asp:BoundField   DataField="AMOUNT_RECD" HeaderText="Amount Received">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" Width="150px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" Font-Names="Tahoma"/>                        
                    </asp:BoundField>

                    <asp:BoundField   DataField="DATE_RECD" HeaderText="Date" SortExpression="DATE_RECD" DataFormatString="{0:dd/MM/yyyy}">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" Width="100px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="120px" Font-Names="Tahoma"/>                        
                    </asp:BoundField>

                     <asp:BoundField   DataField="MEM_PERIOD" HeaderText="Period/Year" SortExpression="MEM_PERIOD">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" Width="100px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="120px" Font-Names="Tahoma"/>                        
                    </asp:BoundField>

                    <asp:BoundField   DataField="STATUS" HeaderText="Status" SortExpression="STATUS">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" Width="100px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="100px" Font-Names="Tahoma"/>                        
                    </asp:BoundField>

                    <asp:BoundField   DataField="RECD_FOR" HeaderText="Received For" SortExpression="RECD_FOR">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" Width="100px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="100px" Font-Names="Tahoma"/>                        
                    </asp:BoundField>
                    
                     
                    <asp:TemplateField  ControlStyle-Width="10px"  HeaderText="Delete" FooterText="Select to Delete" ShowHeader="true" >
                        <HeaderTemplate>
                            <asp:ImageButton ID="ImageButton1" runat="server" Height="16px" Width="16px" ToolTip="Select All" ImageUrl="~/Images/check_all.gif" onClientclick="return Select2(true)"  />
                            <asp:ImageButton ID="ImageButton2" runat="server" Height="16px" Width="16px" ToolTip="Deselect All" ImageUrl="~/Images/uncheck_all.gif"  OnClientClick ="return Select2(false)" />
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
                   

                   



                   

                   <asp:Label ID="Label516" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">Select Print Format</asp:Label>
                       <asp:DropDownList ID="DDL_PrintFormats" runat="server" Font-Bold="True" 
                           ForeColor="#0066CC" ToolTip="Plz Select Value from Drop-Down">
                           <asp:ListItem></asp:ListItem>
                           <asp:ListItem Value="PDF" Selected="True">Pdf Format</asp:ListItem>
                           <asp:ListItem Value="DOC">Doc Format</asp:ListItem>
                           <asp:ListItem Value="HTML">Html Format</asp:ListItem>
                           <asp:ListItem Value="EXCEL">Excel Format</asp:ListItem>
                       </asp:DropDownList>

                       &nbsp;Report Group By:
                    <asp:DropDownList ID="DDL_GroupBy" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC" ToolTip="Plz Select Value from Drop-Down">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem Value="MEM_NAME">Member Name</asp:ListItem>
                        <asp:ListItem Value="MEM_NO">Member No</asp:ListItem>
                        <asp:ListItem Value="RECD_FOR">Received For</asp:ListItem>
                        <asp:ListItem Value="STATUS">Status</asp:ListItem>
                    </asp:DropDownList>

                    <asp:Button ID="Print_Summary_Bttn" runat="server" CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red" Height="20px" TabIndex="14" 
                        Text="Summary Report" Visible="False" Width="130px" />

                    <asp:Button ID="Print_Details_Bttn" runat="server" CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red" Height="20px" TabIndex="14" 
                        Text="Detail Report" Visible="False" Width="130px" />
                    


                   </ContentTemplate>
                       <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="Search_Bttn" EventName="Click" /> 
                            <asp:AsyncPostBackTrigger ControlID="Grid1_Search" EventName="RowCommand" />  
                            <asp:PostBackTrigger  ControlID="DDL_Receipts"   /> 
                            <asp:PostBackTrigger  ControlID="DDL_Members2"   />  
                            <asp:PostBackTrigger  ControlID="DDL_AccessionNo2"   />  
                            <asp:PostBackTrigger  ControlID="DDL_CopyID2"   />  
                            <asp:PostBackTrigger  ControlID="DDL_PmtFor2"   />  
                            <asp:PostBackTrigger  ControlID="DDL_Status"   /> 
                            <asp:PostBackTrigger  ControlID="Print_Summary_Bttn"   /> 
                            <asp:PostBackTrigger ControlID="Print_Details_Bttn" />                                       
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


         <table id="Table2" runat="server" cellspacing="2" border="1"  cellpadding="2" class="style35">
            <tr>
                <td class="style56" colspan="5" bgcolor="#336699">
                   <asp:Label ID="Label13" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True" style="font-size: small">STEP 1: Select Member from Drop-Down!</asp:Label>
                </td>
            </tr>
            
             <tr style=" color:Green; font-weight:bold">
                <td class="style51"> 
                    Select Member</td>
                <td class="style54" colspan="4">
                    <asp:DropDownList ID="DDL_Members" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC" Width="78%" AutoPostBack="True">
                    </asp:DropDownList>
                             
                    <ajaxToolkit:ListSearchExtender ID="DDL_Members_ListSearchExtender" 
                        runat="server" Enabled="True" TargetControlID="DDL_Members" 
                        PromptText="Type to search" PromptCssClass="PromptCSS">
                    </ajaxToolkit:ListSearchExtender>
          
                    
                    <span>
                    <asp:Label ID="Label24" runat="server" Text="Record(s): "></asp:Label>
                    </span>
          
                    
                   </td>                
            </tr>
           
        
            <tr>
                <td class="style56" colspan="5" bgcolor="#336699">
                   <asp:Label ID="Label4" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">Display Member Details</asp:Label>
                </td>
            </tr>
            
            
             <tr style=" color:Green; font-weight:bold">
                <td class="style51"> 
                    Member Number</td>
                 <td class="style54" colspan="3">
                     <asp:Label ID="Label19" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                 </td>                
                 <td align="right" class="style54" rowspan="4" valign="middle">
                     <asp:Image ID="Image4" runat="server" Height="50px" Width="36px" />
                 </td>
            </tr>
            <tr style=" color:Green; font-weight:bold">
                <td class="style51">
                    Member Name</td>
                <td class="style54" colspan="3">
                    <asp:Label ID="Label16" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                </td>
            </tr>
            <tr style=" color:Green; font-weight:bold">
                <td class="style51"> 
                    Category</td>
                <td class="style54" colspan="3">
                    <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                </td>  
                               
            </tr>   
            <tr style=" color:Green; font-weight:bold">
                <td class="style51"> 
                    Sub-Category</td>
                <td class="style54" colspan="3">
                    <asp:Label ID="Label17" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                </td>  
                               
            </tr>   
            
                              
            <tr>
                <td class="style56" colspan="5">
                   <asp:Label ID="Lbl_Error" runat="server" Font-Size="Medium" ForeColor="Yellow" 
                        Font-Bold="True"></asp:Label>
                </td>
            </tr>  
        </table>

        

         <table id="Table1" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
             <tr>
                <td class="style56" colspan="2">
                     <asp:Label ID="Label6" runat="server" Font-Size="Medium" ForeColor="White" 
                         style="font-weight: 700; font-size: small;">STEP2: Enter Data and press SAVE to Save Record!</asp:Label>
                 </td>
            </tr>
            
            <tr>
                <td class="style51"> 
                    Date of Payment</td>
                <td class="style54">
                     <asp:TextBox ID="txt_Rect_Date" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="10" 
                        onkeypress="return DateOnly (event)" ToolTip="Click to Select Date" 
                        Width="71px"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="txt_Rect_Date_CalendarExtender" 
                        runat="server" Enabled="True" Format="dd/MM/yyyy" 
                        TargetControlID="txt_Rect_Date">
                    </ajaxToolkit:CalendarExtender>
                    &nbsp;Payment Received For:
                    <asp:DropDownList ID="DDL_PmtFor" runat="server" AutoPostBack="True" 
                        Font-Bold="True" ForeColor="#0066CC">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem Value="N">New Member</asp:ListItem>
                        <asp:ListItem Value="R">Membership Renewal</asp:ListItem>
                        <asp:ListItem Value="F">Overdue Fine</asp:ListItem>
                        <asp:ListItem Value="L">Lost/Damage Book</asp:ListItem>
                        <asp:ListItem Value="S">Security Deposit</asp:ListItem>
                        <asp:ListItem Value="O">Others</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Label ID="Label26" runat="server"></asp:Label>
                     <asp:Label ID="Label27" runat="server" style="font-size: small; color: #CC3300"></asp:Label>
                </td>
               
            </tr>
            <tr id="TR_SECURITY_DEPOSIT" runat="server">
                <td class="style51"> 
                    Security Deposit (Rs)</td>
                <td class="style54">
                    <asp:TextBox ID="txt_Rect_SecurityDeposit" runat="server" MaxLength="350"  
                        ToolTip="Enter Amount taken as Security Depost" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Height="18px" Width="100px" onkeypress="return isCurrencyKey (event)"></asp:TextBox>
                    &nbsp;</td>
               
            </tr>
             <tr id="TR_AMOUNT_RECD" runat="server">
                <td class="style51"> 
                    Amount Due (Rs)</td>
                <td class="style54">
                    <asp:TextBox ID="txt_Rect_AmountDue" runat="server" MaxLength="350"  
                        ToolTip="Enter Amount Due, If any" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Height="18px" Width="100px" onkeypress="return isCurrencyKey (event)"></asp:TextBox>
                    &nbsp; Amount Received: <asp:TextBox ID="txt_Rect_AmountRecd" runat="server" MaxLength="250"  
                        ToolTip="Enter Authors Name; Separated by ; " Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Height="18px" Width="100px" onkeypress="return isCurrencyKey (event)"></asp:TextBox></td>
               
            </tr>
             
             <tr>
                <td class="style51"> 
                    Period/Year</td>
                <td class="style54">
                    <asp:TextBox ID="txt_Rect_Period" runat="server" 
                        AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                        Height="18px" MaxLength="4000" ToolTip="Enter Period/Year for which amount is recd" 
                        Width="100px" Wrap="False"></asp:TextBox>
                        &nbsp;Payment Mode:
                        <asp:DropDownList ID="DDL_PmtMode" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" ToolTip="Plz Select Value from Drop-Down">
                        <asp:ListItem Value="C">Cash</asp:ListItem>
                        <asp:ListItem Value="Q">Cheque</asp:ListItem>
                        <asp:ListItem Value="E">Electronic Transer</asp:ListItem>
                        <asp:ListItem Value="O">Others</asp:ListItem>
                    </asp:DropDownList>
                   
                    &nbsp;Cheque No:
                    <asp:TextBox ID="txt_Rect_ChqNo" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="18px" MaxLength="50" 
                        ToolTip="Enter Pages" Width="75px" Wrap="False"></asp:TextBox>

                         &nbsp;Date:
                    <asp:TextBox ID="txt_Rect_ChqDate" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="10" 
                        onkeypress="return DateOnly (event)" ToolTip="Click to Select Date" 
                        Width="71px"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="txt_Rect_ChqDate_CalendarExtender1" 
                        runat="server" Enabled="True" Format="dd/MM/yyyy" 
                        TargetControlID="txt_Rect_ChqDate">
                    </ajaxToolkit:CalendarExtender>
                 </td>
               
            </tr>

          <tr id ="TR_ACCESSION" runat="server">
                <td class="style51"> 
                    Accession No</td>
                <td class="style54" colspan="5">
                    <asp:DropDownList ID="DDL_AccessionNo" runat="server" AutoPostBack="True" 
                        Enabled="True" Font-Bold="True" ForeColor="#0066CC" 
                        ToolTip="Plz Select Value from Drop-Down" Width="150px">
                    </asp:DropDownList>
                    <ajaxToolkit:ListSearchExtender ID="DDL_AccessionNo_ListSearchExtender" 
                        runat="server" Enabled="True" PromptCssClass="PromptCSS" 
                        TargetControlID="DDL_AccessionNo">
                    </ajaxToolkit:ListSearchExtender>
                    &nbsp;<asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
                 </td>
             
                
            </tr>

             <tr id ="TR_COPY" runat="server">
                <td class="style51"> 
                    Loose Issue ID</td>
                <td class="style54" colspan="5">
                    <asp:DropDownList ID="DDL_CopyID" runat="server" AutoPostBack="True" 
                        Enabled="True" Font-Bold="True" ForeColor="#0066CC" 
                        ToolTip="Plz Select Value from Drop-Down" Width="150px">
                    </asp:DropDownList>
                    <ajaxToolkit:ListSearchExtender ID="DDL_CopyID_ListSearchExtender" 
                        runat="server" Enabled="True" PromptCssClass="PromptCSS" 
                        TargetControlID="DDL_CopyID">
                    </ajaxToolkit:ListSearchExtender>
                    &nbsp;<asp:Label ID="Label5" runat="server" Text="Label"></asp:Label>
                 </td>
                
                
            </tr>
             
             <tr>
                <td class="style51"> 
                    Remarks</td>
                <td class="style54" colspan="5">
                    <asp:TextBox ID="txt_Rect_Remarks" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="350" ToolTip="Enter Remarks, If any" 
                        Width="99%" Wrap="False"></asp:TextBox>
                 </td>
            </tr>
             
            
        </table>

         
           
        

           

        <table id="Table4" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
            <tr>
                <td class="style56" colspan="2">
             
                    <asp:Button ID="Rect_Save_Bttn" runat="server" AccessKey="s" CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red" TabIndex="14" OnClientClick="return valid1();"
                        Text="Save" ToolTip="Press to SAVE Record" Visible="False" Width="74px" />
                    <asp:Button ID="Rect_Update_Bttn" runat="server"  CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red" TabIndex="14" Text="Update" AccessKey="u" 
                        Width="74px" ToolTip="Press to UPDATE Record"  OnClientClick="return valid2(); "/>
                    <asp:Button ID="Rect_Cancel_Bttn" runat="server"  CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red" 
                        TabIndex="15" Text="Cancel" Width="71px" AccessKey="c" 
                        ToolTip="Press to Cancel the process"/>
                    <asp:Button ID="Rect_Delete_Bttn" runat="server" AccessKey="d" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" TabIndex="14" 
                        Text="Delete" ToolTip="Press to DELETE Record" Visible="False" 
                        Width="74px" />

                         

                </td>
            </tr>

            <tr>
                <td class="style56" colspan="2">   
                    <asp:Label ID="Label25" runat="server" style="color: #FFFFFF"></asp:Label> 
                </td>
            </tr>

             <tr>
                <td class="style56" colspan="2">   
                    <asp:Label ID="Label1" runat="server" style="color: #FFFFFF">HELP: S=Security Deposit; N=New Membership Fee; R=Member Renewal Fee; F= Overdue Fine; L=Fine against Lost; O=Other</asp:Label> 
                </td>
            </tr>

            <tr>
                <td class="style56" colspan="2">   
                    <asp:Button ID="Rect_DeleteAll_Bttn" runat="server"  
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" TabIndex="14" 
                        Text="Delete Selected Records" ToolTip="Press to DELETE Records" 
                        Visible="False" Width="170px" Height="21px" />
                </td>
            </tr>

             <tr>
                <td class="style56" colspan="9" bgcolor="#336699">
                
                
               <asp:Panel ID="Panel1" runat="server" Height ="250px" ScrollBars="Auto">
                       
                   <asp:GridView ID="Grid1" runat="server" AllowPaging="True" DataKeyNames="REC_ID"    
                        style="width: 100%;  text-align: center;" allowsorting="True" 
                        AutoGenerateColumns="False" PageSize="100"  Font-Bold="True" 
                        Font-Names="Tahoma" Font-Size="8pt" Height="100px"  
                        HorizontalAlign="Center"   ShowFooter="True">
                        <Columns >                
                            <asp:TemplateField HeaderText="S.N.">
                                <ItemTemplate>
                                    <asp:Label ID="lblsr" runat="server" CssClass="MBody"  Text = '<%# Container.dataitemindex+1 %>' SkinID="" width="25px"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                                <ItemStyle ForeColor="#336699" Font-Names="Tahoma" Width="25px" 
                                    Font-Bold="True" Font-Size="Small" />
                            </asp:TemplateField>
                    
                           <asp:ButtonField HeaderText="Edit"  Text="Edit" CommandName="Select" 
                                CausesValidation="True">
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" />
                            <ItemStyle ForeColor="#CC0000" Width="20px" Font-Bold="true" />
                            </asp:ButtonField>
                    
                   <asp:BoundField   DataField="ACCESSION_NO" SortExpression="ACCESSION_NO" HeaderText="Accession No" >                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="150px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" Font-Names="Arial"/>                        
                    </asp:BoundField>

                    <asp:BoundField   DataField="COPY_ID" SortExpression="COPY_ID" HeaderText="Loose Issue ID" >                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="100px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="100px" Font-Names="Arial"/>                        
                    </asp:BoundField>

                     <asp:BoundField   DataField="SECURITY_DEPOSIT" SortExpression="SECURITY_DEPOSIT" HeaderText="Security Deposit" >                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="150px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" Font-Names="Arial"/>                        
                    </asp:BoundField>

                    <asp:BoundField   DataField="AMOUNT_DUE" HeaderText="Amount Due" SortExpression="AMOUNT_DUE">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" Width="120px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="120px" Font-Names="Tahoma"/>                        
                    </asp:BoundField>
                                     
                      <asp:BoundField   DataField="AMOUNT_RECD" HeaderText="Amount Received">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" Width="150px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" Font-Names="Tahoma"/>                        
                    </asp:BoundField>

                    <asp:BoundField   DataField="DATE_RECD" HeaderText="Date" SortExpression="DATE_RECD" DataFormatString="{0:dd/MM/yyyy}">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" Width="100px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="120px" Font-Names="Tahoma"/>                        
                    </asp:BoundField>

                     <asp:BoundField   DataField="MEM_PERIOD" HeaderText="Period/Year" SortExpression="MEM_PERIOD">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" Width="100px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="120px" Font-Names="Tahoma"/>                        
                    </asp:BoundField>

                    <asp:BoundField   DataField="STATUS" HeaderText="Status" SortExpression="STATUS">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" Width="100px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="100px" Font-Names="Tahoma"/>                        
                    </asp:BoundField>

                    <asp:BoundField   DataField="RECD_FOR" HeaderText="Received For" SortExpression="RECD_FOR">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" Width="100px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="100px" Font-Names="Tahoma"/>                        
                    </asp:BoundField>
                   

                    <asp:TemplateField  ControlStyle-Width="10px"  HeaderText="Delete" FooterStyle-ForeColor="White" FooterText="Select to Delete" ShowHeader="true">
                        <ItemTemplate>
                            <asp:CheckBox ID="cbd"  runat="server" />
                        </ItemTemplate>

                        <HeaderTemplate>
                            <asp:ImageButton ID="ImageButton1" runat="server" Height="16px" Width="16px" ToolTip="Select All" ImageUrl="~/Images/check_all.gif" onClientclick="return Select(true)"  />
                            <asp:ImageButton ID="ImageButton2" runat="server" Height="16px" Width="16px" ToolTip="Deselect All" ImageUrl="~/Images/uncheck_all.gif"  OnClientClick ="return Select(false)" />
                        </HeaderTemplate>

                        <ControlStyle Width="50px"></ControlStyle>
                    </asp:TemplateField>
                    
                    
                </Columns>
                    
                    <PagerStyle BackColor="White" BorderColor="white" BorderStyle="Solid" 
                        Font-Bold="True" ForeColor="White" HorizontalAlign="Center" 
                        VerticalAlign="Middle" Font-Size="Small" />
                    <RowStyle BackColor="#DADADA" BorderColor="Desktop" ForeColor="White" />
                    <SelectedRowStyle BackColor="Desktop" BorderColor="SteelBlue" BorderStyle="Solid" />
                    <HeaderStyle BackColor="Desktop" Font-Bold="True" Font-Italic="False" Font-Names="Times new roman"
                        Font-Overline="False" Font-Underline="False" ForeColor="White" Width="80%" />
                    <PagerSettings Position="TopAndBottom" FirstPageText="First" LastPageText="Last" PageButtonCount="20" Mode="NumericFirstLast" />
                    <AlternatingRowStyle BackColor="#EFEFEF" Font-Names="Tahoma" ForeColor="#0066FF" />
                </asp:GridView>
                   
                    </asp:Panel>
                               
                
                </td>
            </tr>
            
                        

        </table>

     



      
   
                  </ContentTemplate>
                   <Triggers>
                        <asp:AsyncPostBackTrigger  ControlID="Rect_Save_Bttn" EventName="Click"  /> 
                        <asp:PostBackTrigger  ControlID="Rect_Update_Bttn"   /> 
                        <asp:AsyncPostBackTrigger ControlID="Rect_Cancel_Bttn" EventName="Click" />
                                                 
                   </Triggers>
                    </asp:UpdatePanel>
        
</asp:Content>
