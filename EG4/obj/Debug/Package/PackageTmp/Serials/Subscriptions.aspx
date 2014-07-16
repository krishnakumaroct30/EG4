<%@ Page  Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Subscriptions.aspx.vb" Inherits="EG4.Subscriptions" %>


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
            width: 15%;
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
            text-align: left;
            border-style: none;
            border-color: inherit;
            width: 80%;
            padding: 0px;
            background-color: #D5EAFF;
            font-size: small;
            height: 14px;
        }
                
        .style59
        {
            color: #0033CC;
        }
        .style61
        {
            text-align: left;
            border-style: none;
            border-color: inherit;
            padding: 0px;
            background-color: #D5EAFF;
            font-size: small;
            height: 18px;
            color: #0066FF;
        }
        .style62
        {
            text-align: left;
            border-style: none;
            padding: 0px;
            font-weight: bold;
            background-color: #D5EAFF;
            color: #0033CC;
            font-size: small;
        }
        .style63
        {
            text-align: left;
            border-style: none;
            border-color: inherit;
            padding: 0px;
            background-color: #D5EAFF;
            font-size: small;
            height: 18px;
            color: #0033CC;
        }
                
        </style>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

<script type="text/javascript">
    function formfocus() {
        document.getElementById('<%= DDL_Titles.ClientID %>').focus();
    }
    window.onload = formfocus;
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
       //alpha-numeric only
       function DateOnly1(event) {
           var chCode = ('charCode' in event) ? event.charCode : event.keyCode;
           if (47 <= chCode && chCode <= 57) {
               return (true);
           }

           else {
               alert("Date Is Invalid, Enter in dd/MM/yyyy format only!");
               document.getElementById("MainContent_txt_Subs_SDate").focus();
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
                document.getElementById("MainContent_txt_Subs_EDate").focus();
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
                document.getElementById("MainContent_txt_Subs_FisrtIssueDate").focus();
                return (false);
            }
        }
    </script>
      
      <script language="javascript" type="text/javascript">

          function valid1() {

              if (document.getElementById('<%=DDL_Titles.ClientID%>').value == "") {
                  alert("Please Select \"Title\" from Drop-Down.");
                  document.getElementById("MainContent_DDL_Titles").focus();
                  return (false);
              }

              if (document.getElementById('<%=DDL_SubscriptionYears.ClientID%>').value == "") {
                  alert("Please Select \"Subscription Year\" from Drop-Down.");
                  document.getElementById("MainContent_DDL_SubscriptionYears").focus();
                  return (false);
              }

              if (document.getElementById('<%=txt_Subs_SDate.ClientID%>').value == "") {
                  alert("Please enter proper \"Subscription Start Date\" field.");
                  document.getElementById("MainContent_txt_Subs_SDate").focus();
                  return (false);
              }
              if (document.getElementById('<%=txt_Subs_SDate.ClientID%>').value.length === 8) {
                  alert("Plz Enter \"Subscription Start Date\" in dd/MM/yyyy Format.");
                  document.getElementById("MainContent_txt_Subs_SDate").focus();
                  return (false);
              }

               if (document.getElementById('<%=txt_Subs_EDate.ClientID%>').value == "") {
                  alert("Please enter proper \"Subscription End Date\" field.");
                  document.getElementById("MainContent_txt_Subs_EDate").focus();
                  return (false);
              }
              if (document.getElementById('<%=txt_Subs_EDate.ClientID%>').value.length === 8) {
                  alert("Plz Enter \"Subscription End Date\" in dd/MM/yyyy Format.");
                  document.getElementById("MainContent_txt_Subs_EDate").focus();
                  return (false);
              }

               if (document.getElementById('<%=txt_Subs_FisrtIssueDate.ClientID%>').value == "") {
                  alert("Please enter proper \"First Issue Date\" field.");
                  document.getElementById("MainContent_txt_Subs_FisrtIssueDate").focus();
                  return (false);
              }
              if (document.getElementById('<%=txt_Subs_FisrtIssueDate.ClientID%>').value.length === 8) {
                  alert("Plz Enter \"First Issue Date\" in dd/MM/yyyy Format.");
                  document.getElementById("MainContent_txt_Subs_FisrtIssueDate").focus();
                  return (false);
              }

               if (document.getElementById('<%=DDL_Frequencies.ClientID%>').value == "") {
                  alert("Please enter proper \"Current Frequency\" field.");
                  document.getElementById("MainContent_DDL_Frequencies").focus();
                  return (false);
              }

              if (document.getElementById('<%=txt_Subs_IssuesPerYear.ClientID%>').value == "") {
                  alert("Please enter proper \"Issues Per Year\" field.");
                  document.getElementById("MainContent_txt_Subs_IssuesPerYear").focus();
                  return (false);
              }

              

              return (true);
          }

    </script>

    

        <table id="ADMT1" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
            <tr>
                <td  bgcolor="#003366" class="style43" rowspan="1" style="color: #FFFFFF">Manage
                    <strong>Subscription</strong></td>
            </tr>
            
        </table>      
                   

    
 <div class="style4">
        
 <br />
<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always" >
                    <ContentTemplate>

   
        <table id="Table1" runat="server" cellspacing="2" border="1"  cellpadding="2" class="style35">
            <tr>
                <td class="style56" colspan="9" bgcolor="#336699">
                   <asp:Label ID="Label3" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True" style="font-size: medium">Add Subscription Record</asp:Label>
                </td>
            </tr>
             <tr>
                <td class="style56" colspan="9" bgcolor="#336699">
                   <asp:Label ID="Label14" runat="server" Font-Size="Small" ForeColor="Yellow" 
                        Font-Bold="True" style="font-size: medium"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style56" colspan="9" bgcolor="#336699">
                   <asp:Label ID="Label2" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">STEP 1: Search Record (s): Type Search String in Text Box and Pres ENTER / Display Title Details</asp:Label>
                </td>
            </tr>
             <tr style=" color:Green ; font-weight:bold">
                <td class="style53"> 
                    Search Record</td>
                <td class="style55" colspan="2">
                    <asp:TextBox ID="txt_Search_SearchString" runat="server" 
                        AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                        Height="18px" MaxLength="250" ToolTip="Enter Data and press ENTER to Search Record" 
                        Width="99%" Wrap="False" AutoPostBack="True"></asp:TextBox> 
                 </td>
                <td class="style55" colspan="2">
                    In
                    <asp:DropDownList ID="DropDownList5" runat="server" ForeColor="#0066FF">
                        <asp:ListItem Selected="True" Value="CAT_NO">Cat Number</asp:ListItem>
                        <asp:ListItem Value="ISBN">ISSN</asp:ListItem>
                        <asp:ListItem Value="ACCESSION_NO">Accession Number</asp:ListItem>
                        <asp:ListItem Value="TITLE">Title</asp:ListItem>
                        <asp:ListItem Value="SUB_TITLE">Sub Title</asp:ListItem>
                        <asp:ListItem Value="CORPORATE_AUTHOR">Corporate Author</asp:ListItem>
                        <asp:ListItem Value="EDITOR">Editor</asp:ListItem>
                        <asp:ListItem Value="KEYWORDS">Keywords</asp:ListItem>
                        <asp:ListItem>Note</asp:ListItem>
                        <asp:ListItem Value="TAGS">Tags</asp:ListItem>
                        <asp:ListItem Value="PUB_NAME">Publisher</asp:ListItem>
                        <asp:ListItem Value="SUB_NAME">Subject</asp:ListItem>
                    </asp:DropDownList>
                 </td>
                <td class="style55" colspan="4">
                    <asp:DropDownList ID="DropDownList6" runat="server" ForeColor="#0066FF">
                        <asp:ListItem Selected="True" Value="AND">All Words</asp:ListItem>
                        <asp:ListItem Value="OR">Any Word</asp:ListItem>
                        <asp:ListItem Value="LIKE">Like</asp:ListItem>
                        <asp:ListItem Value="SW">Start With</asp:ListItem>
                        <asp:ListItem Value="EW">End With</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Button ID="Search_Bttn" runat="server" CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red" TabIndex="14" Text="Search" 
                        Width="74px" ToolTip="Press to SEARCH Titles" AccessKey="r" />
                 </td>
                
            </tr>
            <tr style=" color:Green; font-weight:bold">
                <td class="style53"> 
                    Select Title</td>
                <td class="style54" colspan="8">
                    <asp:DropDownList ID="DDL_Titles" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC" Width="78%" AutoPostBack="True">
                    </asp:DropDownList>
          
                    
                    <ajaxToolkit:ListSearchExtender ID="DDL_Titles_ListSearchExtender" 
                        runat="server" Enabled="True" TargetControlID="DDL_Titles" PromptText="Type to search" PromptCssClass="PromptCSS">
                    </ajaxToolkit:ListSearchExtender>
          
                    
                    &nbsp;Preess ENTER
                    <asp:Label ID="Label33" runat="server" Font-Bold="True" 
                        Font-Size="Smaller" ForeColor="#0066FF" style="font-size: x-small"></asp:Label>
                </td>                
            </tr>
           
        
            <tr>
                <td class="style56" colspan="9" bgcolor="#336699">
                   <asp:Label ID="Label1" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">Title Details</asp:Label>
                </td>
            </tr>
            
            
             <tr style=" color:Green; font-weight:bold">
                <td class="style53"> 
                    Cat Number</td>
                 <td class="style54" colspan="6">
                     <asp:Label ID="Label19" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                 </td>                
                 <td align="right" class="style54" rowspan="4" valign="middle" colspan="2">
                     <asp:Image ID="Image4" runat="server" Height="50px" Width="36px" />
                 </td>
            </tr>
            <tr style=" color:Green; font-weight:bold">
                <td class="style53">
                    Title Details</td>
                <td class="style54" colspan="6">
                    <asp:Label ID="Label16" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                </td>
            </tr>
            <tr style=" color:Green; font-weight:bold">
                <td class="style53"> 
                    Editor(s)</td>
                <td class="style54" colspan="6">
                    
                    <asp:Label ID="Label17" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                    
                </td>  
                               
            </tr>   
            <tr style=" color:Green; font-weight:bold">
                <td class="style53"> 
                    Imprint</td>
                <td class="style54" colspan="6">
                    <asp:Label ID="Label18" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                </td>                
            </tr> 
            <tr style=" color:Green; font-weight:bold">
                <td class="style53"> 
                    Corp Author</td>
                <td class="style54" colspan="6">
                    <asp:Label ID="Label23" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                </td>                
            </tr>     
            
            
            
            <tr>
                <td class="style56" colspan="9" bgcolor="#336699">
                   <asp:Label ID="Label5" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">STEP 2: Select Subscription Year to Display Acquisition Details</asp:Label>
                    :
                    <asp:DropDownList ID="DDL_SubscriptionYears" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC" ToolTip="Plz Select Value from Drop-Down" 
                        AutoPostBack="True">
                    </asp:DropDownList>
                </td>
            </tr>
           
        
            <tr>
                <td class="style56" colspan="9" bgcolor="#336699">
                   <asp:Label ID="Label7" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">Acquisition Details</asp:Label>
                </td>
            </tr>
            
            
             <tr style=" color:Green; font-weight:bold">
                <td class="style53"> 
                    ACQ ID</td>
                 <td class="style54" colspan="2">
                     <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                 </td>                
                 <td class="style54" colspan="2">
                     <span class="style59">Acquisition Mode:</span>
                     <asp:Label ID="Label26" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                 </td>
                 <td class="style54" colspan="2">
                     <span class="style59">Committee</span>:
                     <asp:Label ID="Label28" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                 </td>
                 



            </tr>
            <tr style=" color:Green; font-weight:bold">
                <td class="style53">
                    Approval No</td>
                <td class="style54" colspan="2">
                    <asp:Label ID="Label10" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                </td>
                <td class="style62" colspan="2">
                    Approval Date</td>
                <td class="style54" colspan="2">
                    <asp:Label ID="Label25" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                </td>
            </tr>
            <tr style=" color:Green; font-weight:bold">
                <td class="style53"> 
                    Subs.Year</td>
                <td class="style54" colspan="2">
                    
                    <asp:Label ID="Label11" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                    
                </td>  
                               
                <td class="style54" colspan="2">
                    <span class="style59">Copy Ordered</span></td>
                <td class="style54" colspan="2">
                    <asp:Label ID="Label29" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                </td>
                               
            </tr>   
            <tr style=" color:Green; font-weight:bold">
                <td class="style53"> 
                    Oder No</td>
                <td class="style54" colspan="2">
                    <asp:Label ID="Label12" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                </td>                
                <td class="style62" colspan="2">
                    Order Date</td>
                <td class="style54" colspan="2">
                    <asp:Label ID="Label27" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                </td>
            </tr> 
            <tr style=" color:Green; font-weight:bold">
                <td class="style53"> 
                    Subs.Rate</td>
                <td class="style54" colspan="2">
                    <asp:Label ID="Label30" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                </td>                
                <td class="style62" colspan="2">
                    Process Status</td>
                <td class="style54" colspan="2">
                    <asp:Label ID="Label31" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                </td>
            </tr>
            
            
            <tr style=" color:Green; font-weight:bold">
                <td class="style53">
                    Vendor</td>
                <td class="style54" colspan="6">
                    <asp:Label ID="Label13" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                </td>
            </tr>
            <tr style=" color:Green; font-weight:bold">
                <td class="style53">
                    Note</td>
                <td class="style54" colspan="6">
                    <asp:Label ID="Label32" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                </td>
            </tr>
            
            
            <tr>
                <td class="style56" colspan="9">
                   <asp:Label ID="Label15" runat="server" Font-Size="Medium" ForeColor="White" 
                        Font-Bold="True"></asp:Label>
                </td>
            </tr>
                              
            <tr>
                <td class="style56" colspan="9">
                   <asp:Label ID="Label6" runat="server" Font-Size="Medium" ForeColor="Yellow" 
                        Font-Bold="True"></asp:Label>
                </td>
            </tr>            
            <tr>
                <td class="style53"> 
                    Start Date*</td>
                <td class="style55">
                    <asp:TextBox ID="txt_Subs_SDate" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="10" 
                        onkeypress="return DateOnly1 (event)" ToolTip="Click to Select Date" 
                        Width="71px"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="txt_Subs_SDate_CalendarExtender" 
                        runat="server" Enabled="True" Format="dd/MM/yyyy" 
                        TargetControlID="txt_Subs_SDate">
                    </ajaxToolkit:CalendarExtender>
                     <asp:Label ID="Label34" runat="server" Font-Bold="True" Font-Size="Smaller" 
                        ForeColor="#0066FF" style="font-size: x-small"></asp:Label>
                </td>
                <td class="style63" colspan="2">
                    <strong>End Date*</strong></td>
                <td class="style55" colspan="2">
                    <asp:TextBox ID="txt_Subs_EDate" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="10" 
                        onkeypress="return DateOnly2 (event)" ToolTip="Click to Select Date" 
                        Width="70px"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="txt_Subs_EDate_CalendarExtender" 
                        runat="server" Enabled="True" Format="dd/MM/yyyy" 
                        TargetControlID="txt_Subs_EDate">
                    </ajaxToolkit:CalendarExtender>
                </td>
                <td class="style61" colspan="2">
                    <strong>First Issue Date*</strong></td>
                <td class="style55">
                    <asp:TextBox ID="txt_Subs_FisrtIssueDate" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="10" 
                        onkeypress="return DateOnly3 (event)" ToolTip="Click to Select Date" 
                        Width="70px"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="txt_Subs_FisrtIssueDate_CalendarExtender" 
                        runat="server" Enabled="True" Format="dd/MM/yyyy" 
                        TargetControlID="txt_Subs_FisrtIssueDate">
                    </ajaxToolkit:CalendarExtender>
                   
                </td>
            </tr>
            <tr>
                <td class="style53">
                    Frequency*</td>
                <td class="style55" colspan="2">
                    <asp:DropDownList ID="DDL_Frequencies" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC" ToolTip="Plz Select Value from Drop-Down" 
                        Width="100px">
                    </asp:DropDownList>
                </td>
                <td class="style55" colspan="2">
                    <span class="style59"><strong>Volume Per Year</strong></span>:<asp:TextBox 
                        ID="txt_Subs_VolPerYear" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="18px" MaxLength="4" 
                        onkeypress="return isNumberKey(event)" ToolTip="Enter Volume Per Year" 
                        Width="40px" Wrap="False"></asp:TextBox>
                </td>
                <td class="style55" colspan="4">
                    <span class="style59"><strong>Issues Per Year*</strong></span>:<asp:TextBox 
                        ID="txt_Subs_IssuesPerYear" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="18px" MaxLength="4" 
                        onkeypress="return isNumberKey(event)" ToolTip="Enter Issues Per Year" 
                        Width="40px" Wrap="False"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style53"> 
                    Issues Per Vol</td>
                <td class="style55" colspan="2">
                    <asp:TextBox ID="txt_Subs_IssuesPerVol" runat="server" 
                        AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                        Height="18px" MaxLength="4" onkeypress="return isNumberKey(event)" 
                        ToolTip="Enter Issues Per Volume" Width="50px" Wrap="False"></asp:TextBox>
                </td>
                <td class="style55" colspan="2">
                    <span class="style59"><strong>Start Vol No</strong></span><asp:TextBox 
                        ID="txt_Subs_StartVolNo" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="18px" MaxLength="6" 
                        ToolTip="Enter Item Cost" Width="40px" Wrap="False" 
                       onkeypress="return isNumberKey(event)"></asp:TextBox>
                </td>
                <td class="style55" colspan="4">
                    <asp:Label ID="Label8" runat="server" Text="Start Issue No" 
                        style="color: #0033CC; font-weight: 700"></asp:Label>                    
                    :
                    <asp:TextBox ID="txt_Subs_StartIssueNo" runat="server" 
                        AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                        Height="18px" MaxLength="6" onkeypress="return isNumberKey(event)" 
                        ToolTip="Enter Item Cost" Width="40px" Wrap="False"></asp:TextBox>
                </td>              
            </tr>
             <tr>
                <td class="style53"> 
                    Grace Days</td>
                <td class="style54" colspan="8">
                    <asp:TextBox ID="txt_Subs_GraceDays" runat="server" 
                        AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                        Height="18px" MaxLength="4" onkeypress="return isNumberKey(event)" 
                        ToolTip="Enter Volume Per Year" Width="50px" Wrap="False"></asp:TextBox>
                    &nbsp;Issue Continue&nbsp;
                    <asp:DropDownList ID="DDL_Continue" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC" ToolTip="Plz Select Value from Drop-Down">
                        <asp:ListItem Selected="True">N</asp:ListItem>
                        <asp:ListItem>Y</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;<asp:Label ID="Label4" runat="server" Text="Copy" Enabled="False"></asp:Label>
                    <asp:TextBox ID="txt_Subs_Copy" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="18px" MaxLength="4" 
                        onkeypress="return isNumberKey(event)" ToolTip="Enter Volume Per Year" 
                        Width="40px" Wrap="False" Enabled="False"></asp:TextBox>
                    &nbsp; Subs.No:
                    <asp:TextBox ID="txt_Subs_SubsNo" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="18px" MaxLength="100" 
                        ToolTip="Enter Subscription No, if any" 
                        Width="100px" Wrap="False"></asp:TextBox>
                    &nbsp;</td>               
            </tr>        

             <tr>
                <td class="style53"> 
                    Location</td>
                <td class="style58" colspan="8">                 
                                     
                    <asp:TextBox ID="txt_Subs_Location" runat="server" 
                        AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                        Height="18px" MaxLength="50"  
                        ToolTip="Enter Display Location" Width="100px" Wrap="False"></asp:TextBox>
                                     
                 </td>                
            </tr>
        
            <tr>
                <td class="style53">
                    Remarks</td>
                <td class="style58" colspan="8">
                    <asp:TextBox ID="txt_Subs_Remarks" runat="server" 
                        AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                        Height="18px" MaxLength="250" ToolTip="Enter Remarks, if any." Width="99%" 
                        Wrap="False"></asp:TextBox>
                </td>
            </tr>
        
            <tr>
                <td class="style47" colspan="9">
                    &nbsp; * Mandatory</td>
            </tr>
             <tr>
                <td class="style47" colspan="9">
                    <asp:Button ID="Subs_Save_Bttn" runat="server" AccessKey="s" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" Height="23px" 
                         TabIndex="14" Text="Save" OnClientClick="return valid1();"
                        Width="74px" Visible="False" ToolTip="Press to SAVE Record"/>
                    <asp:Button ID="Subs_Update_Bttn" runat="server" AccessKey="u" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" Height="23px" 
                         TabIndex="14" Text="Update" OnClientClick="return valid1();"
                        Width="74px" Visible="False" ToolTip="Press to UPDATE Record" />
                    <asp:Button ID="Subs_Cancel_Bttn" runat="server" AccessKey="c" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" Height="23px" 
                        TabIndex="14" Text="Cancel" 
                        Width="74px" ToolTip="Press to Cancel" />
                    <asp:Button ID="Subs_Delete_Bttn" runat="server" AccessKey="d" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" Height="23px" 
                        TabIndex="14" Text="Delete" ToolTip="Press to delete Record" 
                        Width="74px" />
                    <asp:Button ID="Subs_GetData_Bttn" runat="server" AccessKey="g" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" Height="23px" 
                        TabIndex="14" Text="Get Data From Prev Year" 
                        ToolTip="Press to Get Subscription data from prev year" Visible="False" 
                        Width="180px" />
                 </td>
            </tr>

             

             <tr>  
                <td class="style56" colspan="9" bgcolor="#336699">                
                     
                 


                    &nbsp;</td>
            </tr>
            
                        

        </table>

                
               
   
       
  

  
 
 
                </ContentTemplate>  
                     <Triggers> 
                        <asp:AsyncPostBackTrigger  ControlID="DDL_Titles" EventName="TextChanged"   />  
                        <asp:AsyncPostBackTrigger  ControlID="Subs_Save_Bttn" EventName="Click"   />   
                        <asp:AsyncPostBackTrigger  ControlID="Subs_Update_Bttn" EventName="Click"   />  
                        <asp:AsyncPostBackTrigger  ControlID="Subs_Delete_Bttn" EventName="Click"   />                                  
                   </Triggers>                
                    </asp:UpdatePanel>                
                           
</div>

     
        
                   

</asp:Content>
