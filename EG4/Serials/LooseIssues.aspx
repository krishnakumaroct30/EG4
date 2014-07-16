<%@ Page  Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="LooseIssues.aspx.vb" Inherits="EG4.LooseIssues" %>


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
                
        .style47
    {
        text-align: center;
        border-style: none;
        border-color: inherit;
        padding: 0px;
        background-color:#99CCFF;
        font-size:small;
    }
                  
        .styleBttn
    {
             cursor:pointer;
            margin-left: 0px;
            }
               
                
        .style53
        {
            text-align: left;
            border-style: none;
            border-color: inherit;
            width: 13%;
            padding: 0px;
            background-color: #99CCFF;
            font-size: small;
            height: 18px;
            color: #0033CC;
            font-weight: 700;
        }
        
       
        .style54
        {
             text-align: left;
            border-style: none;
            padding: 0px;
            font-weight: bold;
            background-color: #D5EAFF;
            font-size: small;
        }
        .style55
        {
            text-align: left;
            border-style: none;
            border-color: inherit;
            padding: 0px;
             background-color:#D5EAFF;  
            font-size: small;
            height: 18px;
        }
        .style56
        {
            text-align: center;
            width: 100%;
            background-color:#336699;
                      
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
            font-weight:bold;  
            font-family:Arial;
            background-color:Silver;  
            height:20px;    
            }    
      
      
        .style58
        {
            text-align:  left;
            border-style: none;
            border-color: inherit;
            width: 80%;
            padding: 0px;
            background-color: #D5EAFF;
            font-size: small;
            height: 14px;
            color: #0033CC;
            font-weight: 700;
        }
                
        </style>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

<script type="text/javascript">
    function formfocus() {
        document.getElementById('<%= DDL_SubscriptionYears.ClientID %>').focus();
    }
    window.onload = formfocus;
 </script>

   <script type ="text/javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 45) {
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
               return false;
           }

           return true;
       }

   </script>
   <script type ="text/javascript">
       function YN(event) {
           var charCode = (event.which) ? event.which : event.keyCode
           if (charCode == 78 || charCode == 110 || charCode == 80 || charCode == 112 || charCode == 88 || charCode == 120 ) {
               return (true);
           }
           else {
               alert("Plz Enter N X P only!");
               document.getElementById("MainContent_txt_Loose_Status").focus();
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
               document.getElementById("MainContent_txt_Loose_IssueDate").focus();
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
                document.getElementById("MainContent_txt_Loose_DueDate").focus();
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
                document.getElementById("MainContent_txt_Loose_DueDate").focus();
                return (false);
            }
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
    
     <script language="javascript" type="text/javascript">

         function valid1() {

              if (document.getElementById('<%=Label36.ClientID%>').value == "") {
                 alert("Please Select \" Record from Grid\" to Receive!");
                 return (false);
             }

             if (document.getElementById('<%=txt_Loose_IssueDate.ClientID%>').value == "") {
                 alert("Please Enter proper \" Issue Date\" in dd/MM/yyyy format!");
                 document.getElementById("MainContent_txt_Loose_IssueDate").focus();
                 return (false);
             }
             if (document.getElementById('<%=txt_Loose_IssueDate.ClientID%>').value.length != 10) {
                 alert("Plz Enter \" Loose Issue Date\" in dd/MM/yyyy Format.");
                 document.getElementById("MainContent_txt_Loose_IssueDate").focus();
                 return (false);
             }

             if (document.getElementById('<%=txt_Loose_Copy.ClientID%>').value.length == "") {
                 alert("Plz Enter \" No of Copeis being Received\" in the Specified Field!");
                 document.getElementById("MainContent_txt_Loose_Copy").focus();
                 return (false);
             }

            return (true);
         }

    </script>


     <div class="style4">

        <table id="ADMT1" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
            <tr>
                <td  bgcolor="#003366" class="style43" rowspan="1" style="color: #FFFFFF">Loose Issues Manager</td>
            </tr>
            
        </table>      
                   

    

        
 <br />
<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always" >
                    <ContentTemplate>

   
        <table id="Table1" runat="server" cellspacing="2" border="1"  cellpadding="2" class="style35">
            <tr>
                <td class="style56" colspan="3" bgcolor="#336699">
                   <asp:Label ID="Label3" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True" style="font-size: medium">Receive Loose Issues</asp:Label>
                </td>
            </tr>
             <tr>
                <td class="style56" colspan="3" bgcolor="#336699">
                   <asp:Label ID="Label14" runat="server" Font-Size="Small" ForeColor="Yellow" 
                        Font-Bold="True" style="font-size: medium"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style56" colspan="3" bgcolor="#336699">
                   <asp:Label ID="Label2" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">STEP 1: Select Subscription Year from Drop-Down and Then Select Title from Drop-Down to Display the Title Details.</asp:Label>
                </td>
            </tr>
             <tr style=" color:Green ; font-weight:bold">
                <td class="style53"> 
                    Select Subs. Year</td>
                <td class="style55" colspan="2">
                    <asp:DropDownList ID="DDL_SubscriptionYears" runat="server" AutoPostBack="True" 
                        Font-Bold="True" ForeColor="#0066CC" ToolTip="Plz Select Value from Drop-Down">
                    </asp:DropDownList>
                    <asp:Label ID="Label33" runat="server" Font-Bold="True" Font-Size="Smaller" 
                        ForeColor="#0066FF" style="font-size: x-small"></asp:Label>
                 </td>
                
            </tr>
            <tr style=" color:Green; font-weight:bold">
                <td class="style53"> 
                    Select Title</td>
                <td class="style54" colspan="2">
                    <asp:DropDownList ID="DDL_Titles" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC" Width="78%" AutoPostBack="True">
                    </asp:DropDownList>
          
                    
                    <ajaxToolkit:ListSearchExtender ID="DDL_Titles_ListSearchExtender" 
                        runat="server" Enabled="True" TargetControlID="DDL_Titles" PromptText="Type to search" PromptCssClass="PromptCSS">
                    </ajaxToolkit:ListSearchExtender>
          
                    
                    &nbsp;Preess ENTER
                    </td>                
            </tr>
           
        
            <tr>
                <td class="style56" colspan="3" bgcolor="#336699">
                   <asp:Label ID="Label1" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">Title Details</asp:Label>
                </td>
            </tr>
            
            
             <tr style=" color:Green; font-weight:bold">
                <td class="style53"> 
                    Cat Number</td>
                 <td class="style54">
                     <asp:Label ID="Label19" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                 </td>                
                 <td align="right" class="style54" rowspan="4" valign="middle">
                     <asp:Image ID="Image4" runat="server" Height="50px" Width="36px" />
                 </td>
            </tr>
            <tr style=" color:Green; font-weight:bold">
                <td class="style53">
                    Title Details</td>
                <td class="style54">
                    <asp:Label ID="Label16" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                </td>
            </tr>
            <tr style=" color:Green; font-weight:bold">
                <td class="style53"> 
                    Editor(s)</td>
                <td class="style54">
                    
                    <asp:Label ID="Label17" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                    
                </td>  
                               
            </tr>   
            <tr style=" color:Green; font-weight:bold">
                <td class="style53"> 
                    Imprint</td>
                <td class="style54">
                    <asp:Label ID="Label18" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                </td>                
            </tr> 
            <tr style=" color:Green; font-weight:bold">
                <td class="style53"> 
                    Corp Author</td>
                <td class="style54">
                    <asp:Label ID="Label23" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                </td>                
            </tr>     
            
            
            
           
            <tr>
                <td class="style56" colspan="3" bgcolor="#336699">
                   <asp:Label ID="Label4" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">STEP 2: Select Loose Issue from Below Grid and press Receive Link </asp:Label>
                    <asp:Label ID="Label35" runat="server" style="color: #FFFFFF"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style56" colspan="3" bgcolor="#336699">
                   
                
                
                 <asp:Panel ID="Panel1" runat="server" Height ="250px" ScrollBars="Auto">
                       
                   <asp:GridView ID="Grid1" runat="server" AllowPaging="True" DataKeyNames="ISS_ID"    
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
                    
                           <asp:ButtonField HeaderText="Click to Receive"  Text="Select" CommandName="Select" 
                                CausesValidation="True">
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" />
                            <ItemStyle ForeColor="#CC0000" Width="20px" Font-Bold="true" />
                            </asp:ButtonField>
                    
                   

                    <asp:BoundField   DataField="VOL_NO" HeaderText="Vol.No" SortExpression="VOL_NO">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" Width="100px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="120px" Font-Names="Tahoma"/>                        
                    </asp:BoundField>
                                      
                    <asp:BoundField   DataField="ISSUE_NO" SortExpression="ISSUE_NO" HeaderText="Issue No">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="100px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="120px" Font-Names="Arial"/>                        
                    </asp:BoundField>

                    <asp:BoundField   DataField="PART_NO" SortExpression="PART_NO" HeaderText="Part No">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="100px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="120px" Font-Names="Arial"/>                        
                    </asp:BoundField>

                    <asp:BoundField   DataField="ISS_DATE" SortExpression="ISS_DATE" HeaderText="Issue Date" DataFormatString="{0:dd/MM/yyyy}">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="100px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="120px" Font-Names="Arial"/>                        
                    </asp:BoundField>

                    <asp:BoundField   DataField="DUE_DATE" SortExpression="DUE_DATE" HeaderText="Due Date" DataFormatString="{0:dd/MM/yyyy}">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="100px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="120px" Font-Names="Arial"/>                        
                    </asp:BoundField>

                    <asp:BoundField   DataField="COPY_ORDERED" HeaderText="Copy Ordered">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="100px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="120px" Font-Names="Arial"/>                        
                    </asp:BoundField>

                    <asp:BoundField   DataField="RECEIVED" SortExpression="RECEIVED" HeaderText="Recd?">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="100px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="120px" Font-Names="Arial"/>                        
                    </asp:BoundField>

                     <asp:BoundField   DataField="COPY_RECEIVED" HeaderText="Copy Recd">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="100px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="120px" Font-Names="Arial"/>                        
                    </asp:BoundField>

                    <asp:BoundField   DataField="RECEIVED_DATE" SortExpression="RECEIVED_DATE" HeaderText="Recd Date" DataFormatString="{0:dd/MM/yyyy}">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="100px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="120px" Font-Names="Arial"/>                        
                    </asp:BoundField>

                   
                    
                    
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
             

             <tr>
                <td class="style47" colspan="3">
                    Status: N = Not Received /&nbsp; Y= Received / P = Not Published / X= Credit Note 
                    Received</td>
            </tr>

              <tr>
                <td class="style56" colspan="9" bgcolor="#336699">
                   <asp:Label ID="Label6" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">STEP 3: Select Record from Above Grid and Press Receive Button</asp:Label>
                    </td>
            </tr>

             <tr>
                <td class="style53">
                    Vol No</td>
                <td class="style58" colspan="8">
                    <asp:TextBox ID="txt_Loose_VolNo" runat="server" 
                        AutoCompleteType="DisplayName" Enabled="True" Font-Bold="True" 
                        ForeColor="#0066FF" Height="18px" MaxLength="25" Width="50px" Wrap="False" 
                        ToolTip="Plz Enter Vol Number"></asp:TextBox>
                    Issue No
                    <asp:TextBox ID="txt_Loose_IssueNo" runat="server" 
                        AutoCompleteType="DisplayName" Enabled="True" Font-Bold="True" 
                        ForeColor="#0066FF" Height="18px" MaxLength="25" Width="50px" Wrap="False" 
                        ToolTip="Plz Enter Issue No"></asp:TextBox>
                    Part No
                    <asp:TextBox ID="txt_Loose_PartNo" runat="server" 
                        AutoCompleteType="DisplayName" Enabled="True" Font-Bold="True" 
                        ForeColor="#0066FF" Height="18px" MaxLength="25" Width="50px" Wrap="False" 
                        ToolTip="Plz Enter Part No"></asp:TextBox>
                    Issue Date*
                    <asp:TextBox ID="txt_Loose_IssueDate" runat="server" Enabled="True" 
                        Font-Bold="True" ForeColor="#0066FF" Height="16px" MaxLength="10" 
                        Width="71px" onkeypress="return DateOnly1 (event)"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="txt_Loose_IssueDate_CalendarExtender"  
                        runat="server" Enabled="True" Format="dd/MM/yyyy" 
                        TargetControlID="txt_Loose_IssueDate">
                    </ajaxToolkit:CalendarExtender>

                    Due Date
                    <asp:TextBox ID="txt_Loose_DueDate" runat="server" Enabled="True" 
                        Font-Bold="True" ForeColor="#0066FF" Height="16px" MaxLength="10" 
                        Width="71px" onkeypress="return DateOnly2 (event)"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="txt_Loose_DueDate_CalendarExtender"  
                        runat="server" Enabled="True" Format="dd/MM/yyyy" 
                        TargetControlID="txt_Loose_DueDate">
                    </ajaxToolkit:CalendarExtender>

                     &nbsp;Date Recd
                    <asp:TextBox ID="txt_Loose_ReceivedDate" runat="server" Enabled="True" 
                        Font-Bold="True" ForeColor="#0066FF" Height="16px" MaxLength="10" Width="71px" onkeypress="return DateOnly3 (event)"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="txt_Loose_ReceivedDate_CalendarExtender" 
                        runat="server" Enabled="True" Format="dd/MM/yyyy" 
                        TargetControlID="txt_Loose_ReceivedDate">
                    </ajaxToolkit:CalendarExtender>
                    <asp:Label ID="Label36" runat="server" 
                        style="color: #0066FF; font-size: xx-small;"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style53">
                    Status/Recd?</td>
                <td class="style58" colspan="8">
                    <asp:TextBox ID="txt_Loose_Recd" runat="server" 
                        AutoCompleteType="DisplayName" Enabled="False" Font-Bold="True" style="text-transform: uppercase" 
                        onkeypress="return YN (event)" ForeColor="#0066FF" Height="18px" 
                        MaxLength="1" Width="20px" Wrap="False"></asp:TextBox>
                   
                     &nbsp; Copy Being Received* 
                    <asp:TextBox ID="txt_Loose_Copy" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="18px" MaxLength="5" 
                         Width="20px" onkeypress="return isNumberKey (event)"
                        Wrap="False"></asp:TextBox>&nbsp;(For Un-Recv Copy, Plz use Minus Sign (-), e.g. -2)
                   
                </td>
            </tr>
             <tr>
                <td class="style53">
                    Type of Issue</td>
                <td class="style58" colspan="8">
                   
                    <asp:RadioButton ID="RadioButton1" runat="server" Checked="True" 
                        GroupName="Type" Text="Normal" />
                    <asp:RadioButton ID="RadioButton2" runat="server" GroupName="Type" 
                        Text="Supplement" />
                    <asp:RadioButton ID="RadioButton3" runat="server" GroupName="Type" 
                        Text="Special" />
                    <asp:RadioButton ID="RadioButton4" runat="server" GroupName="Type" 
                        Text="Index" />
                    &nbsp; / Copy Already Received 
                    <asp:TextBox ID="txt_Loose_CopyAlreadyRecd" runat="server" AutoCompleteType="DisplayName" 
                        Enabled="False" Font-Bold="True" ForeColor="#0066FF" Height="18px" MaxLength="6" 
                        Width="20px" 
                        Wrap="False"></asp:TextBox>
                 </td>
            </tr>
            <tr>
                <td class="style53">
                    Remarks</td>
                <td class="style58" colspan="8">
                    <asp:TextBox ID="txt_Loose_Remarks" runat="server" AutoCompleteType="DisplayName" 
                        Enabled="True" Font-Bold="True" ForeColor="#0066FF" Height="18px" 
                        MaxLength="250" ToolTip="Plz Enter Remarks, if any!" Width="98%" 
                        Wrap="False"></asp:TextBox>
                </td>
            </tr>






             <tr>
                <td class="style47" colspan="9">
                    &nbsp; * Mandatory</td>
            </tr>

             <tr>  
                <td class="style56" colspan="9" bgcolor="#336699">                
                     
                 <asp:Button ID="Loose_Save_Bttn" runat="server" AccessKey="s" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" Height="23px" 
                         TabIndex="14" Text="Receive" OnClientClick="return valid1();"
                        Width="74px" Visible="False" ToolTip="Press to SAVE Record"/>
                    <asp:Button ID="Loose_Cancel_Bttn" runat="server" AccessKey="c" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" Height="23px" 
                        TabIndex="14" Text="Cancel" 
                        Width="74px" ToolTip="Press to Cancel" />
                    <asp:Button ID="Loose_Delete_Bttn" runat="server" AccessKey="d" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" Height="23px" 
                        TabIndex="14" Text="Delete" ToolTip="Press to delete Record" 
                        Width="74px" Visible="False" />


                    &nbsp;</td>
            </tr>
            








        </table>

                
               
   
       
  

  
 
 
                </ContentTemplate>  
                     <Triggers> 
                        <asp:AsyncPostBackTrigger  ControlID="DDL_Titles" EventName="TextChanged"   />  
                        <asp:AsyncPostBackTrigger  ControlID="Loose_Save_Bttn" EventName="Click"   />   
                        <asp:AsyncPostBackTrigger  ControlID="Loose_Delete_Bttn" EventName="Click"   />                                  
                   </Triggers>                
                    </asp:UpdatePanel>                
                           
</div>

     
        
                   

</asp:Content>
