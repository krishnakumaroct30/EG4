<%@ Page  Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Approvals.aspx.vb" Inherits="EG4.Approvals" %>


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
        .style3
        {
            text-align: center;
            vertical-align: middle;
            height: 38px;
            width: 97%;
            margin-left:10px;  
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
      
      
        .style57
        {
            text-align: justify;
            border-style: none;
            border-color: inherit;
            width: 15%;
            padding: 0px;
            background-color: #99CCFF;
            font-size: small;
            height: 14px;
        }
        .style58
        {
            text-align: justify;
            border-style: none;
            border-color: inherit;
            width: 80%;
            padding: 0px;
            background-color: #D5EAFF;
            font-size: small;
            height: 14px;
        }
                
        </style>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

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


       <script language ="javascript" type ="text/javascript" >


           function GetCheckStatus() {
               var srcControlId = event.srcElement.id;
               var targetControlId = event.srcElement.id.replace('cbd', 'txt_App_CopyAppd');
               if (document.getElementById(srcControlId).checked)
                   document.getElementById(targetControlId).disabled = false;
               else
                   document.getElementById(targetControlId).disabled = true;
           }
   
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
        function Select3(Select3) {

            var grdv = document.getElementById('<%= Grid3.ClientID %>');
            var chbk = "cbd";
            var txtbk = "txt_App_CopyAppd";

            var Inputs = grdv.getElementsByTagName("input");
           
            for (var n = 0; n < Inputs.length; ++n) {
                if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(chbk, 0) >= 0) {
                    Inputs[n].checked = Select3;                 
                }
                if (Inputs[n].type == 'text' && Inputs[n].id.indexOf(txtbk, 0) >= 0) {
                    if (Select3 == true) {
                        Inputs[n].disabled = false;
                    }
                    else {
                        Inputs[n].disabled = true;
                    }                    
                }
            }
            return false;
        }
        
      

    </script> 
     
     <script language="javascript" type="text/javascript">

         function valid1() {

             if (document.getElementById('<%=DDL_Titles.ClientID%>').value == "") {
                 alert("Please enter \" Select title from Drop-Down\" field.");
                 document.getElementById("MainContent_DDL_Titles").focus();
                 return (false);
             }
             if (document.getElementById('<%=txt_Acq_AppNo.ClientID%>').value == "") {
                 alert("Please enter proper \"Approval No/List No\" field.");
                 document.getElementById("MainContent_txt_Acq_AppNo").focus();
                 return (false);
             }

             if (document.getElementById('<%=txt_Acq_Copy.ClientID%>').value == "") {
                 alert("Please enter \" No of Copy(ies) being Proposed\" field.");
                 document.getElementById("MainContent_txt_Acq_Copy").focus();
                 return (false);
             }
             if (document.getElementById('<%=DDL_Currencies.ClientID%>').value == "") {
                 alert("Please enter \" Select Currency from Drop-Down\" field.");
                 document.getElementById("MainContent_DDL_Currencies").focus();
                 return (false);
             }
             if (document.getElementById('<%=txt_Acq_ItemPrice.ClientID%>').value == "") {
                 alert("Please enter \" Item Price in Original Currency\" field.");
                 document.getElementById("MainContent_txt_Acq_ItemPrice").focus();
                 return (false);
             }

             return (true);
         }
    </script>

     <script language="javascript" type="text/javascript">

         function valid2() {

             if (document.getElementById('<%=DDL_Approvals.ClientID%>').value == "") {
                 alert("Please enter \" Select Approval from Drop-Down\" field.");
                 document.getElementById("MainContent_DDL_Approvals").focus();
                 return (false);
             }

             if (document.getElementById('<%=DDL_Committees.ClientID%>').value == "") {
                 alert("Please enter \" Select committee from Drop-Down\" field.");
                 document.getElementById("MainContent_DDL_Committees").focus();
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
                 document.getElementById("MainContent_txt_Acq_RecommendedDate").focus();
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
                 document.getElementById("MainContent_txt_Acq_AppDate").focus();
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
                 document.getElementById("MainContent_txt_App_AppDate3").focus();
                 return (false);
             }
         }
    </script>


    

        <table id="ADMT1" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
            <tr>
                <td  bgcolor="#003366" class="style43" rowspan="1" style="color: #FFFFFF">Manage
                    <strong>Approval</strong></td>
            </tr>
            
        </table>      
                   

        
        <div class="style3">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always" >
                    <ContentTemplate>
            <asp:Menu
                ID="Menu1"
                Width="98%"
                runat="server"
                Orientation="Horizontal"
                StaticEnableDefaultPopOutImage="False"
                 OnMenuItemClick="Menu1_MenuItemClick">
                <Items>
                    <asp:MenuItem ImageUrl="~/Images/button1up.png" Text="" Value="0" Selected="true"></asp:MenuItem>
                    <asp:MenuItem ImageUrl="~/Images/button2over.png" Text="" Value="1"></asp:MenuItem>
                    <asp:MenuItem ImageUrl="~/Images/button3over.png" Text="" Value="2"></asp:MenuItem>
                </Items>
        
            </asp:Menu>
             </ContentTemplate>  
        </asp:UpdatePanel>
    </div>
    
 <div class="style4">
        
 <br />
<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always" >
                    <ContentTemplate>

   <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
   <asp:View ID="Tab1" runat="server">
        <table id="Table1" runat="server" cellspacing="2" border="1"  cellpadding="2" class="style35">
            <tr>
                <td class="style56" colspan="5" bgcolor="#336699">
                   <asp:Label ID="Label3" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True" style="font-size: medium">Add Approval</asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style56" colspan="5" bgcolor="#336699">
                   <asp:Label ID="Label2" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">Search Record (s): Type Search String in Text Box and Pres ENTER</asp:Label>
                </td>
            </tr>
             <tr style=" color:Green ; font-weight:bold">
                <td class="style53"> 
                    Search Record</td>
                <td class="style55">
                    <asp:TextBox ID="txt_Search_SearchString" runat="server" 
                        AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                        Height="18px" MaxLength="250" ToolTip="Enter Data and press ENTER to Search Record" 
                        Width="99%" Wrap="False" AutoPostBack="True"></asp:TextBox> 
                 </td>
                <td class="style55">
                    In
                    <asp:DropDownList ID="DropDownList5" runat="server" ForeColor="#0066FF">
                        <asp:ListItem Selected="True" Value="CAT_NO">Cat Number</asp:ListItem>
                        <asp:ListItem>ISBN</asp:ListItem>
                        <asp:ListItem Value="ACCESSION_NO">Accession Number</asp:ListItem>
                        <asp:ListItem Value="TITLE">Title</asp:ListItem>
                        <asp:ListItem Value="SUB_TITLE">Sub Title</asp:ListItem>
                        <asp:ListItem Value="AUTHOR1">Author1</asp:ListItem>
                        <asp:ListItem Value="AUTHOR2">Author2</asp:ListItem>
                        <asp:ListItem Value="AUTHOR3">Author3</asp:ListItem>
                        <asp:ListItem Value="CORPORATE_AUTHOR">Corporate Author</asp:ListItem>
                        <asp:ListItem Value="EDITOR">Editor</asp:ListItem>
                        <asp:ListItem Value="SERIES_TITLE">Series</asp:ListItem>
                        <asp:ListItem Value="KEYWORDS">Keywords</asp:ListItem>
                        <asp:ListItem Value="YEAR_OF_PUB">Year</asp:ListItem>
                        <asp:ListItem>Note</asp:ListItem>
                        <asp:ListItem Value="CONF_NAME">Conference Name</asp:ListItem>
                        <asp:ListItem Value="TAGS">Tags</asp:ListItem>
                        <asp:ListItem Value="PUB_NAME">Publisher</asp:ListItem>
                        <asp:ListItem Value="SUB_NAME">Subject</asp:ListItem>
                    </asp:DropDownList>
                 </td>
                <td class="style55" colspan="2">
                    <asp:DropDownList ID="DropDownList6" runat="server" ForeColor="#0066FF">
                        <asp:ListItem Selected="True" Value="AND">All Words</asp:ListItem>
                        <asp:ListItem Value="OR">Any Word</asp:ListItem>
                        <asp:ListItem Value="LIKE">Like</asp:ListItem>
                        <asp:ListItem Value="SW">Start With</asp:ListItem>
                        <asp:ListItem Value="EW">End With</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Button ID="Search_Bttn" runat="server" CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red" TabIndex="14" Text="Search" 
                        Width="74px" ToolTip="Press to SEARCH Titles" />
                 </td>
                
            </tr>
            <tr style=" color:Green; font-weight:bold">
                <td class="style53"> 
                    Select Title</td>
                <td class="style54" colspan="4">
                    <asp:DropDownList ID="DDL_Titles" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC" Width="78%" AutoPostBack="True">
                    </asp:DropDownList>
          
                    
                    <ajaxToolkit:ListSearchExtender ID="DDL_Titles_ListSearchExtender" 
                        runat="server" Enabled="True" TargetControlID="DDL_Titles" PromptText="Type to search" PromptCssClass="PromptCSS">
                    </ajaxToolkit:ListSearchExtender>
          
                    
                    &nbsp;Preess ENTER</td>                
            </tr>
           
        
            <tr>
                <td class="style56" colspan="5" bgcolor="#336699">
                   <asp:Label ID="Label1" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">Display Record</asp:Label>
                </td>
            </tr>
            
            
             <tr style=" color:Green; font-weight:bold">
                <td class="style53"> 
                    Cat Number</td>
                 <td class="style54" colspan="3">
                     <asp:Label ID="Label19" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                 </td>                
                 <td align="right" class="style54" rowspan="4" valign="middle">
                     <asp:Image ID="Image4" runat="server" Height="50px" Width="36px" />
                 </td>
            </tr>
            <tr style=" color:Green; font-weight:bold">
                <td class="style53">
                    Title Details</td>
                <td class="style54" colspan="3">
                    <asp:Label ID="Label16" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                </td>
            </tr>
            <tr id="TR_AUTHORS" runat="server" style=" color:Green; font-weight:bold">
                <td class="style53"> 
                    Author(s)</td>
                <td class="style54" colspan="3">
                    
                    <asp:Label ID="Label17" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                    
                </td>  
                               
            </tr>   
            <tr id="TR_IMPRINT" runat="server" style=" color:Green; font-weight:bold">
                <td class="style53"> 
                    Imprint</td>
                <td class="style54" colspan="3">
                    <asp:Label ID="Label18" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                </td>                
            </tr> 
            <tr style=" color:Green; font-weight:bold">
                <td class="style53"> 
                    Multi-vol?</td>
                <td class="style54" colspan="3">
                    <asp:Label ID="Label23" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                </td>                
            </tr>                       
            <tr>
                <td class="style56" colspan="5">
                   <asp:Label ID="Label6" runat="server" Font-Size="Medium" ForeColor="Yellow" 
                        Font-Bold="True"></asp:Label>
                    <asp:Label ID="Label15" runat="server" Font-Bold="True" Font-Size="Medium" 
                        ForeColor="White"></asp:Label>
                </td>
            </tr>            
            <tr>
                <td class="style53"> 
                    Approval No*</td>
                <td class="style55">
                    <asp:TextBox ID="txt_Acq_AppNo" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="18px" MaxLength="50" 
                        ToolTip="Enter Distinct Approval/List No " Width="95%" Wrap="False" style="text-transform: uppercase"></asp:TextBox>
                        <ajaxToolkit:AutoCompleteExtender 
                        ID="txt_Acq_AppNo_AutoCompleteExtender" 
                        runat="server" 
                        MinimumPrefixLength="1"
                        EnableCaching="true" 
                        CompletionSetCount="10"
                        ServiceMethod="SearchAppNo"
                        TargetControlID="txt_Acq_AppNo" 
                        FirstRowSelected = "false">
                    </ajaxToolkit:AutoCompleteExtender>
                </td>
                <td class="style55">
                    Approval Date:</td>
                <td class="style55">
                    Volume No:<asp:TextBox ID="txt_Acq_VolNo" runat="server" 
                        AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                        Height="18px" MaxLength="25" ToolTip="Enter Volume No " Width="55px" 
                        Wrap="False"></asp:TextBox>
                </td>                
                <td class="style55">
                    Copy*:<asp:TextBox ID="txt_Acq_Copy" runat="server" 
                        AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                        Height="18px" MaxLength="5" 
                        ToolTip="Enter No of Copies Proposed (Numeric Only)" Width="35px" Wrap="False" onkeypress="return isNumberKey(event)"></asp:TextBox>
                </td>                
            </tr>
            <tr>
                <td class="style53"> 
                    Currency*</td>
                <td class="style55">
                    <asp:DropDownList ID="DDL_Currencies" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC" ToolTip="Plz Select Value from Drop-Down">
                    </asp:DropDownList>
                    <asp:Label ID="Label4" runat="server"></asp:Label>
                </td>
                <td class="style55">
                    Item Cost*&nbsp;
                    <asp:TextBox ID="txt_Acq_ItemPrice" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="18px" MaxLength="15" 
                        ToolTip="Enter Item Cost" Width="60px" Wrap="False" onkeypress="return isCurrencyKey(event)"></asp:TextBox>
                </td>
                <td class="style55" colspan="2">
                    <asp:Label ID="Label8" runat="server" Text="Acquisition Mode:"></asp:Label>                    
                    <asp:DropDownList ID="DDL_AcqModes" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC" ToolTip="Plz Select Value from Drop-Down">
                    </asp:DropDownList>
                </td>              
            </tr>
             <tr>
                <td class="style53"> 
                    Recommended By</td>
                <td class="style54" colspan="2">
                    <asp:TextBox ID="txt_Acq_RecommendedBy" runat="server" 
                        AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                        Height="18px" MaxLength="150" 
                        ToolTip="Enter Name of Person who Recommended this book." Width="99%" 
                        Wrap="False"></asp:TextBox>
                </td>               
                <td class="style54" colspan="2">
                    Recommended Date:<asp:TextBox ID="txt_Acq_RecommendedDate" runat="server" 
                        Font-Bold="True" ForeColor="#0066FF" Height="16px" MaxLength="10" 
                        ToolTip="Click to Select Date" Width="71px" onkeypress="return DateOnly (event)"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="txt_Acq_RecommendedDate_CalendarExtender" 
                        runat="server" Enabled="True" Format="dd/MM/yyyy" 
                        TargetControlID="txt_Acq_RecommendedDate">
                    </ajaxToolkit:CalendarExtender>
                 </td>               
            </tr>        

             <tr>
                <td class="style57"> 
                    Remarks</td>
                <td class="style58" colspan="4">                 
                                     
                    <asp:TextBox ID="txt_Acq_Remarks" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="18px" MaxLength="250" 
                        ToolTip="Enter Remarks, if any." Width="99%" Wrap="False"></asp:TextBox>
                                     
                 </td>                
            </tr>
        
            <tr>
                <td class="style47" colspan="5">
                    &nbsp; * Mandatory</td>
            </tr>
             <tr>
                <td class="style47" colspan="5">
                    <asp:Button ID="App_Save_Bttn" runat="server" AccessKey="s" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" Height="23px" 
                         TabIndex="14" Text="Save" 
                        Width="74px" Visible="False" ToolTip="Press to SAVE Record" OnClientClick="return valid1();"/>
                    <asp:Button ID="App_Update_Bttn" runat="server" AccessKey="u" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" Height="23px" 
                         TabIndex="14" Text="Update" 
                        Width="74px" Visible="False" ToolTip="Press to UPDATE Record" OnClientClick="return valid1();" />
                    <asp:Button ID="App_Cancel_Bttn" runat="server" AccessKey="c" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" Height="23px" 
                        TabIndex="14" Text="Cancel" 
                        Width="74px" ToolTip="Press to Cancel" />
                    <asp:Button ID="App_DeleteAll_Bttn" runat="server" AccessKey="d" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" Height="23px" 
                        TabIndex="14" Text="Delete Selected Record(s)" ToolTip="Press to Cancel" 
                        Width="180px" />
                 </td>
            </tr>

             <tr>
                <td class="style56" colspan="5" bgcolor="#336699">
                   <asp:Label ID="Label24" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">Display Record</asp:Label>
                </td>
            </tr>

             <tr>  
                <td class="style56" colspan="5" bgcolor="#336699">                
                     
                 


         <asp:GridView ID="Grid1" runat="server" AllowPaging="True" DataKeyNames="ACQ_ID"    
                        style="width: 100%;  text-align: center;" allowsorting="True" 
                        AutoGenerateColumns="False"  Font-Bold="True" 
                        Font-Names="Tahoma" Font-Size="8pt" Height="100px"  
                        HorizontalAlign="Center"   ShowFooter="True">
                        <Columns >                
                            <asp:TemplateField HeaderText="S.N.">
                                <ItemTemplate>
                                    <asp:Label ID="lblsr" runat="server" CssClass="MBody"  SkinID="" width="25px"></asp:Label>
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
                    
                     

                    <asp:BoundField   DataField="APP_NO" HeaderText="Approval No" SortExpression="APP_NO">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" Width="650px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" Font-Names="Tahoma"/>                        
                    </asp:BoundField>
                                      
                    <asp:BoundField   DataField="APP_DATE" SortExpression="APP_DATE" HeaderText="Approval Date" visible="true">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="20px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" Font-Names="Arial"/>                        
                    </asp:BoundField>
                     <asp:BoundField   DataField="VOL_NO" SortExpression="VOL_NO" HeaderText="Vol No" visible="true">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="20px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" Font-Names="Arial"/>                        
                    </asp:BoundField>
                     <asp:BoundField   DataField="PROCESS_STATUS" SortExpression="PROCESS_STATUS" HeaderText="Status" visible="true">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="20px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" Font-Names="Arial"/>                        
                    </asp:BoundField>
                     <asp:BoundField   DataField="COPY_PROPOSED"  HeaderText="Copy Proposed" visible="true">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="20px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" Font-Names="Arial"/>                        
                    </asp:BoundField>
                     <asp:BoundField   DataField="CUR_CODE"  HeaderText="Currency" visible="true">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="20px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="50px" Font-Names="Arial"/>                        
                    </asp:BoundField>
                     <asp:BoundField   DataField="ITEM_PRICE"  HeaderText="Item Cost" visible="true">                                               
                            <HeaderStyle Font-Bold="True"  Font-Names="Arial" Font-Size="Small" Width="20px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" Font-Names="Arial"/>                        
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
                        Font-Bold="True" ForeColor="White" HorizontalAlign="Center" 
                        VerticalAlign="Middle" Font-Size="Small" />
                    <RowStyle BackColor="#DADADA" BorderColor="Desktop" ForeColor="#3399FF" />
                    <SelectedRowStyle BackColor="Desktop" BorderColor="SteelBlue" BorderStyle="Solid" />
                    <HeaderStyle BackColor="Desktop" Font-Bold="True" Font-Italic="False" Font-Names="Times new roman"
                        Font-Overline="False" Font-Underline="False" ForeColor="White" Width="80%" />
                    <PagerSettings Position="TopAndBottom" FirstPageText="First" LastPageText="Last" PageButtonCount="10" Mode="NumericFirstLast" />
                    <AlternatingRowStyle BackColor="#EFEFEF" Font-Names="Tahoma" ForeColor="#0066FF" />
                </asp:GridView>
                   

                  
                      
                   </td>
            </tr>
            
                        

        </table>

                
        
  
        
     </asp:View>
    <asp:View ID="Tab2" runat="server">
        <table id="Table2" runat="server" cellspacing="2" border="1"  cellpadding="2" class="style35">
            <tr>
                <td class="style56" colspan="5" bgcolor="#336699">
                   <asp:Label ID="Label5" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True" style="font-size: medium">Generate Approval</asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style56" colspan="5" bgcolor="#336699">
                   <asp:Label ID="Label7" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">Select Approval from Drop-Down and Process it</asp:Label>
                    <asp:Label ID="Label20" runat="server" Font-Bold="True" Font-Size="Medium" 
                        ForeColor="Yellow"></asp:Label>
                </td>
            </tr>
            <tr style=" color:Green; font-weight:bold">
                <td class="style53"> 
                    Select Approval *</td>
                <td class="style55">
                    <asp:DropDownList ID="DDL_Approvals" runat="server" AutoPostBack="True" 
                        Font-Bold="True" ForeColor="#0066CC" Width="78%">
                    </asp:DropDownList>
                    <ajaxToolkit:ListSearchExtender ID="DDL_Approvals_ListSearchExtender" 
                        runat="server" Enabled="True" IsSorted="True" PromptCssClass="PromptCSS" 
                        TargetControlID="DDL_Approvals">
                    </ajaxToolkit:ListSearchExtender>
                 </td>
                <td class="style55">
                    App Date *:&nbsp;
                    <asp:TextBox ID="txt_Acq_AppDate" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="10" ToolTip="Click to Select Date" 
                        Width="71px" onkeypress="return DateOnly2 (event)"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="txt_Acq_AppDate_CalendarExtender" 
                        runat="server" Enabled="True" Format="dd/MM/yyyy" 
                        TargetControlID="txt_Acq_AppDate">
                    </ajaxToolkit:CalendarExtender>
                 </td>
                <td class="style55" colspan="2">
                    <asp:Button ID="App_Process_Bttn" runat="server" CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red" TabIndex="14" Text="Process" 
                        Width="74px" ToolTip="Press to Process the Approval" AccessKey="p" OnClientClick="return valid2();" />
                 </td>
                
            </tr>
              <tr style=" color:Green; font-weight:bold">
                <td class="style53"> 
                    Select Committee *</td>
                <td class="style55" colspan="2">
                    <asp:DropDownList ID="DDL_Committees" runat="server" AutoPostBack="True" 
                        Font-Bold="True" ForeColor="#0066CC" Width="95%">
                    </asp:DropDownList>
                    <ajaxToolkit:ListSearchExtender ID="DDL_Committees_ListSearchExtender" 
                        runat="server" Enabled="True" IsSorted="True" PromptCssClass="PromptCSS" 
                        TargetControlID="DDL_Committees">
                    </ajaxToolkit:ListSearchExtender>
                </td>               
                <td class="style54" colspan="2">
                   
                                        
            
                 </td>               
            </tr>       
           <tr>
                <td class="style56" colspan="9" bgcolor="#336699">
                   
                   <asp:Label ID="Label516" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">Select Print Format: </asp:Label>
                       <asp:DropDownList ID="DDL_PrintFormats" runat="server" Font-Bold="True" 
                           ForeColor="#0066CC" ToolTip="Plz Select Value from Drop-Down">
                           <asp:ListItem></asp:ListItem>
                           <asp:ListItem Value="PDF" Selected="True">Pdf Format</asp:ListItem>
                           <asp:ListItem Value="DOC">Doc Format</asp:ListItem>
                       </asp:DropDownList>

                       

                    &nbsp;<asp:Button ID="App_Print_Bttn" runat="server" AccessKey="r" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" TabIndex="14" 
                        Text="Print Approval" ToolTip="Press to Print Approval Form" Visible="False" 
                        Width="110px" />
                    &nbsp;<asp:Label ID="Label25" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">Select Letter Template: </asp:Label>
                    <asp:DropDownList ID="DDL_Letters" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC">
                    </asp:DropDownList>

                       

                </td>
            </tr> 
        
            <tr>
                <td class="style56" colspan="5" bgcolor="#336699">
                   <asp:Label ID="Label9" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">Display Record(s)</asp:Label>
                </td>
            </tr>
             <tr>
                <td class="style56" colspan="5" bgcolor="#336699">
                   <asp:Label ID="Label22" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">HELP: Acquisition Record(s) with Status 'Requested' will only be processed!</asp:Label>
                </td>
            </tr>
            
            
            
            

             <tr>  
                <td class="style56" colspan="5" bgcolor="#336699">                
                     
                 


         <asp:GridView ID="Grid2" runat="server" AllowPaging="True" DataKeyNames="ACQ_ID"    
                        style="width: 100%;  text-align: center;" allowsorting="True" 
                        AutoGenerateColumns="False"  Font-Bold="True" 
                        Font-Names="Tahoma" Font-Size="8pt" Height="100px"  
                        HorizontalAlign="Center"   ShowFooter="True">
                        <Columns >                
                            <asp:TemplateField HeaderText="S.N.">
                                <ItemTemplate>
                                    <asp:Label ID="lblsr" runat="server" CssClass="MBody"  SkinID="" width="25px"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                                <ItemStyle ForeColor="#336699" Font-Names="Tahoma" Width="25px" 
                                    Font-Bold="True" Font-Size="Small" />
                            </asp:TemplateField>
                       
                     <asp:BoundField   DataField="TITLE"  HeaderText="Title" SortExpression="TITLE" visible="true">                                               
                            <HeaderStyle Font-Bold="True"  Font-Names="Arial" Font-Size="Small" Width="950px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="950px" Font-Names="Arial"/>                        
                    </asp:BoundField> 

                    <asp:BoundField   DataField="APP_NO" HeaderText="Approval No" SortExpression="APP_NO">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" Width="100px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" Font-Names="Tahoma"/>                        
                    </asp:BoundField>
                                      
                    <asp:BoundField   DataField="APP_DATE" SortExpression="APP_DATE" HeaderText="Approval Date" visible="true"  DataFormatString="{0:dd/MM/yyyy}">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="150px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="160px" Font-Names="Arial"/>                        
                    </asp:BoundField>
                     <asp:BoundField   DataField="VOL_NO" SortExpression="VOL_NO" HeaderText="Vol No" visible="true">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="100px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="100px" Font-Names="Arial"/>                        
                    </asp:BoundField>
                     <asp:BoundField   DataField="PROCESS_STATUS" SortExpression="PROCESS_STATUS" HeaderText="Status" visible="true">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="160px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="160px" Font-Names="Arial"/>                        
                    </asp:BoundField>
                     <asp:BoundField   DataField="COM_CODE"  HeaderText="Committee" visible="true">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="160px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="160px" Font-Names="Arial"/>                        
                    </asp:BoundField>
                     <asp:BoundField   DataField="COPY_PROPOSED"  HeaderText="Copy Proposed" visible="true">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="180px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="180px" Font-Names="Arial"/>                        
                    </asp:BoundField>
                     <asp:BoundField   DataField="CUR_CODE"  HeaderText="Currency" visible="true">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="100px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="100px" Font-Names="Arial"/>                        
                    </asp:BoundField>
                     <asp:BoundField   DataField="ITEM_PRICE"  HeaderText="Item Cost" visible="true">                                               
                            <HeaderStyle Font-Bold="True"  Font-Names="Arial" Font-Size="Small" Width="150px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" Font-Names="Arial"/>                        
                    </asp:BoundField>
                    
                     
                   
                    
                    
                </Columns>
                    
                    <PagerStyle BackColor="#3399FF" BorderColor="#C0C000" BorderStyle="Solid" 
                        Font-Bold="True" ForeColor="White" HorizontalAlign="Center" 
                        VerticalAlign="Middle" Font-Size="Small" />
                    <RowStyle BackColor="#DADADA" BorderColor="Desktop" ForeColor="#3399FF" />
                    <SelectedRowStyle BackColor="Desktop" BorderColor="SteelBlue" BorderStyle="Solid" />
                    <HeaderStyle BackColor="Desktop" Font-Bold="True" Font-Italic="False" Font-Names="Times new roman"
                        Font-Overline="False" Font-Underline="False" ForeColor="White" Width="90%" />
                    <PagerSettings Position="TopAndBottom" FirstPageText="First" LastPageText="Last" PageButtonCount="10" Mode="NumericFirstLast" />
                    <AlternatingRowStyle BackColor="#EFEFEF" Font-Names="Tahoma" ForeColor="#0066FF" />
                </asp:GridView>
                   

                  
                      
                   </td>
            </tr>
            
                        

        </table>

                
        


    </asp:View>
    <asp:View ID="Tab3" runat="server">
        <table id="Table3" runat="server" cellspacing="2" border="1"  cellpadding="2" class="style35">
            <tr>
                <td class="style56" colspan="4" bgcolor="#336699">
                   <asp:Label ID="Label10" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True" style="font-size: medium">Update Approval</asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style56" colspan="4" bgcolor="#336699">
                   <asp:Label ID="Label11" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">Select Approval from Drop-Down and Process it</asp:Label>
                    <asp:Label ID="Label12" runat="server" Font-Bold="True" Font-Size="Medium" 
                        ForeColor="Yellow"></asp:Label>
                </td>
            </tr>
            <tr style=" color:Green; font-weight:bold">
                <td class="style53"> 
                    Select Approval</td>
                <td class="style55">
                    <asp:DropDownList ID="DDL_Approval3" runat="server" 
                        Font-Bold="True" ForeColor="#0066CC" Width="78%" 
                        ToolTip="Select Approval from Here" AutoPostBack="True">
                    </asp:DropDownList>
                    <ajaxToolkit:ListSearchExtender ID="ListSearchExtender1" 
                        runat="server" Enabled="True" IsSorted="True"  PromptCssClass="PromptCSS" 
                        TargetControlID="DDL_Approvals">
                    </ajaxToolkit:ListSearchExtender>
                 </td>
                <td class="style55">
                    App Date:&nbsp;
                    <asp:TextBox ID="txt_App_AppDate3" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="10" 
                        Width="71px" onkeypress="return DateOnly3 (event)"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" 
                        runat="server" Enabled="True" Format="dd/MM/yyyy" 
                        TargetControlID="txt_Acq_AppDate">
                    </ajaxToolkit:CalendarExtender>
                 </td>
                <td class="style55">
                    <asp:Button ID="App_UpdateApproval_Bttn" runat="server" CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red" TabIndex="14" Text="Update Approval" 
                        Width="150px" ToolTip="Press to Update the Approval" AccessKey="p" 
                        Visible="False" />
                 </td>
                
            </tr>
              <tr style=" color:Green; font-weight:bold">
                <td class="style53"> 
                    Committee</td>
                <td class="style54" colspan="3">
                   
                    <asp:DropDownList ID="DDL_Committees3" runat="server" AutoPostBack="True" 
                        Font-Bold="True" ForeColor="#0066CC" Width="95%">
                    </asp:DropDownList>
                    
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" 
                        Enabled="True" Format="dd/MM/yyyy" TargetControlID="txt_Acq_RecommendedDate">
                    </ajaxToolkit:CalendarExtender>
                </td>               
            </tr>       
           




             







        
            <tr>
                <td class="style56" colspan="4" bgcolor="#336699">
                   <asp:Label ID="Label13" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">Display Record(s)</asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style56" colspan="4" bgcolor="#336699">
                   <asp:Label ID="Label14" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">HELP: Check the Record if Approved, Un-Check the Record if Rejected - Press UPDATE Button</asp:Label>
                    <asp:Label ID="Label21" runat="server" Font-Bold="True" Font-Size="Medium" 
                        ForeColor="Yellow"></asp:Label>
                </td>
            </tr>
            
            
            

             <tr>  
                <td class="style56" colspan="4" bgcolor="#336699">                
                     
                  <asp:GridView ID="Grid3" runat="server" AllowPaging="True" DataKeyNames="ACQ_ID"  
                        style="width: 100%;  text-align: center;" allowsorting="True" 
                        AutoGenerateColumns="False" PageSize="2000"  Font-Bold="True" 
                        Font-Names="Tahoma" Font-Size="8pt" Height="100px"  
                        HorizontalAlign="Center" ShowFooter="True" CellSpacing="2" 
                        >
                        <Columns >                
                            <asp:TemplateField HeaderText="S.N.">
                                <ItemTemplate>
                                    <asp:Label ID="lblsr" runat="server" CssClass="MBody"  SkinID="" width="25px"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                                <ItemStyle ForeColor="#336699" Font-Names="Tahoma" Width="20px" 
                                    Font-Bold="True" Font-Size="Small" />
                            </asp:TemplateField>                       
                                     
                    <asp:BoundField   DataField="TITLE" HeaderText="Title" 
                                SortExpression="TITLE" >                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="350px" 
                                Font-Names="Arial"/>                        
                    </asp:BoundField>
                    <asp:BoundField   DataField="VOL_NO" HeaderText="Vol" ReadOnly="True"  SortExpression="VOL_NO">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" Font-Names="Arial"  
                                width="150px" />                        
                    </asp:BoundField>
                     <asp:BoundField   DataField="PROCESS_STATUS" HeaderText="Status" >                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" 
                                Font-Names="Arial"/>                        
                    </asp:BoundField>

                     <asp:BoundField   DataField="COPY_PROPOSED" HeaderText="Copy Proposed" 
                                SortExpression="COPY_PROPOSED" >                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" 
                                Font-Names="Arial"/>                        
                    </asp:BoundField>
                     
                     <asp:TemplateField HeaderText="Copy Approved">
                        <ItemTemplate>                       
                            <asp:TextBox ID="txt_App_CopyAppd"  runat ="server"  Enabled="false"   ForeColor="Red" MaxLength="4" Font-Bold="true"  Text='<%#  Eval("COPY_APPROVED") %>' Width="45px"  visible="true" onkeypress="return isNumberKey(event)"></asp:TextBox>
                        </ItemTemplate>
                        <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                        <ItemStyle ForeColor="#336699" Font-Names="Tahoma" Width="150px" 
                            Font-Bold="True" Font-Size="Small" BackColor="Red"/>
                     </asp:TemplateField>

                                      
                    <asp:TemplateField  ControlStyle-Width="10px"  HeaderText="Approve/Reject" FooterText="Select If Approved" ShowHeader="true">
                        <HeaderTemplate>
                            <asp:ImageButton ID="ImageButton1" runat="server" Height="16px" Width="16px" ToolTip="Select All" ImageUrl="~/Images/check_all.gif" onClientclick="return Select3(true)"  />
                            <asp:ImageButton ID="ImageButton2" runat="server" Height="16px" Width="16px" ToolTip="Deselect All" ImageUrl="~/Images/uncheck_all.gif"  OnClientClick ="return Select3(false)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cbd"  runat="server"   onclick="GetCheckStatus()"  />
                        </ItemTemplate>
                        <FooterStyle  ForeColor="White" />


                        <ControlStyle Width="50px"></ControlStyle>
                    </asp:TemplateField>
                   
                           
                   
                    
                    
                </Columns>
                    
                    <PagerStyle BackColor="#3399FF" BorderColor="#C0C000" BorderStyle="Solid" 
                        Font-Bold="True" ForeColor="White" HorizontalAlign="Center" 
                        VerticalAlign="Middle" Font-Size="Small" />
                    <RowStyle BackColor="#DADADA" BorderColor="Desktop" ForeColor="#3399FF" />
                    <SelectedRowStyle BackColor="Desktop" BorderColor="SteelBlue" BorderStyle="Solid" />
                        <EditRowStyle ForeColor="#CC3300" />
                    <HeaderStyle BackColor="Desktop" Font-Bold="True" Font-Italic="False" Font-Names="Times new roman"
                        Font-Overline="False" Font-Underline="False" ForeColor="White" Width="80%" />
                    <PagerSettings Position="TopAndBottom" FirstPageText="First" LastPageText="Last" PageButtonCount="2000" Mode="NumericFirstLast" />
                    <AlternatingRowStyle BackColor="#EFEFEF" Font-Names="Tahoma" ForeColor="#0066FF" />
                </asp:GridView>
                   


                  
                      
                   </td>
            </tr>
            
                        

        </table>
                      
    </asp:View>
</asp:MultiView>
  

  
 
 
                </ContentTemplate>  
                     <Triggers>
                        <asp:AsyncPostBackTrigger  ControlID="Menu1" EventName="MenuItemClick"   /> 
                        <asp:AsyncPostBackTrigger  ControlID="DDL_Titles" EventName="TextChanged"   />  
                        <asp:AsyncPostBackTrigger  ControlID="App_Save_Bttn" EventName="Click"   />   
                        <asp:AsyncPostBackTrigger  ControlID="App_UpdateApproval_Bttn" EventName="Click"   />  
                        <asp:PostBackTrigger ControlID="App_Print_Bttn" />                                 
                   </Triggers>                
                    </asp:UpdatePanel>                
                           
</div>

     
        
                   

</asp:Content>
