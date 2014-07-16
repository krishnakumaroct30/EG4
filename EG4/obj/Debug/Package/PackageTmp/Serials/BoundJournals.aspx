<%@ Page  Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="BoundJournals.aspx.vb" Inherits="EG4.BoundJournals" %>


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
                
          .style100
        {
            background-color: #D5EAFF;
             
            
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
        function DateOnly(event) {
            var chCode = ('charCode' in event) ? event.charCode : event.keyCode;
            if (47 <= chCode && chCode <= 57) {
                return (true);
            }

            else {
                alert("Date Is Invalid, Enter in dd/MM/yyyy format only!");
                document.getElementById("MainContent_txt_JHold_AccessionDate").focus();
                return (false);
            }
        }
    </script>

    <script language="javascript" type="text/javascript">

        function valid1() {
          
            if (document.getElementById('<%=DDL_Titles.ClientID%>').value == "") {
                alert("Please Select \"Title\" From Drop-Down.");
                document.getElementById("MainContent_DDL_Titles").focus();
                return (false);
            }
            if (document.getElementById('<%=txt_JHold_AccessionNo.ClientID%>').value == "") {
                alert("Please enter proper \"Accession No\" field.");
                document.getElementById("MainContent_txt_JHold_AccessionNo").focus();
                return (false);
            }
            if (document.getElementById('<%=txt_JHold_AccessionDate.ClientID%>').value == "") {
                alert("Please enter proper \"Accession Date\" field.");
                document.getElementById("MainContent_txt_JHold_AccessionDate").focus();
                return (false);
            }
            if (document.getElementById('<%=txt_JHold_AccessionDate.ClientID%>').value.length != 10) {
                alert("Plz Enter \" Loose Issue Date\" in dd/MM/yyyy Format.");
                document.getElementById("MainContent_txt_JHold_AccessionDate").focus();
                return (false);
            }
           



            return (true);
        }

    </script>

    <script language="javascript" type="text/javascript">

        function valid2() {

            if (document.getElementById('<%=Label36.ClientID%>').value == "") {
                alert("Please Select \"Title\" From Drop-Down.");
                document.getElementById("MainContent_Label36").focus();
                return (false);
            }
            if (document.getElementById('<%=txt_JHold_AccessionNo.ClientID%>').value == "") {
                alert("Please enter proper \"Accession No\" field.");
                document.getElementById("MainContent_txt_JHold_AccessionNo").focus();
                return (false);
            }
            if (document.getElementById('<%=txt_JHold_AccessionDate.ClientID%>').value == "") {
                alert("Please enter proper \"Accession Date\" field.");
                document.getElementById("MainContent_txt_JHold_AccessionDate").focus();
                return (false);
            }
            if (document.getElementById('<%=txt_JHold_AccessionDate.ClientID%>').value.length != 10) {
                alert("Plz Enter \" Loose Issue Date\" in dd/MM/yyyy Format.");
                document.getElementById("MainContent_txt_JHold_AccessionDate").focus();
                return (false);
            }




            return (true);
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


             //         for (var n = 0; n < document.forms[0].length; n++) {
             //             //if (document.forms[0].elements[n].type == 'checkbox') {
             //             if (document.getElementById("cbd")== true) {
             //                 document.forms[0].elements[n].checked = Select;
             //             }
             //         }
             return false;
         }

    </script> 



        <table id="ADMT1" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
            <tr>
                <td  bgcolor="#003366" class="style43" rowspan="1" style="color: #FFFFFF">Manage
                    <strong>Bound Volumes of Journals</strong></td>
            </tr>
            
        </table>      
                   

    
 <div class="style4">
        
 <br />
<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always" >
                    <ContentTemplate>

   
        <table id="Table1" runat="server" cellspacing="2" border="1"  cellpadding="2" class="style35">
            
             <tr>
                <td class="style56" colspan="9" bgcolor="#336699">
                   <asp:Label ID="Lbl_Error" runat="server" Font-Size="Small" ForeColor="Yellow" 
                        Font-Bold="True" style="font-size: medium"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style56" colspan="9" bgcolor="#336699">
                   <asp:Label ID="Label2" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">STEP 1: Search Journal and Select Title from below Drop-Down to Display the Title Details.</asp:Label>
                </td>
            </tr>
             <tr style=" color:Green ; font-weight:bold">
                <td class="style53"> 
                    Search by Acc No</td>
                <td class="style55" colspan="8">
                    <asp:TextBox ID="txt_JHold_Search" runat="server" 
                        AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                        Height="18px" MaxLength="50" style="text-transform: uppercase"  
                        ToolTip="Enter Accession No of Bound Set of Journal and Press Search" 
                        Width="97px" Wrap="False" AutoPostBack="True"></asp:TextBox>
                    &nbsp;Enter Acc.No to Search by Accession No &nbsp;
                    <asp:Label ID="Label37" runat="server" Font-Bold="True" Font-Size="Small" 
                        style="font-size: smaller"></asp:Label>
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
                    </td>                
            </tr>
           
        
            <tr>
                <td class="style56" colspan="9" bgcolor="#336699">
                   <asp:Label ID="Label1" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">Title Details</asp:Label>
                </td>
            </tr>
            
            
             <tr >
                <td class="style53"> 
                    Cat Number</td>
                 <td class="style54" colspan="6">
                     <asp:Label ID="Label19" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                 </td>                
                 <td align="right" class="style100"  rowspan="4" valign="middle" colspan="2">
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
                     
            
            
            <tr>
                <td class="style56" colspan="9" bgcolor="#336699">
                   <asp:Label ID="Label5" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">STEP 2: Enter Details and Press SAVE Button to Save the Data</asp:Label>
                    :
                    </td>
            </tr>
           
               
              <tr>
                <td class="style53">
                    Accession No*</td>
                <td class="style58" colspan="8">
                    
                    <asp:TextBox ID="txt_JHold_AccessionNo" runat="server" Enabled="True" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="30" 
                        ToolTip="Enter Proper Accession No (Use Alpha Device e.g. P or J)" 
                        Width="100px" style="text-transform: uppercase" ></asp:TextBox>
                    <asp:Label ID="Label36" runat="server" Font-Size="Smaller"></asp:Label>
                    &nbsp;Accession Date*:
                    <asp:TextBox ID="txt_JHold_AccessionDate" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="10" 
                        onkeypress="return DateOnly (event)" ToolTip="Click to Select Date" 
                        Width="71px"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="txt_JHold_AccessionDate_CalendarExtender" 
                        runat="server" Enabled="True" Format="dd/MM/yyyy" 
                        TargetControlID="txt_JHold_AccessionDate">
                    </ajaxToolkit:CalendarExtender>
                    &nbsp;Vol No:
                    <asp:TextBox ID="txt_JHold_VolNo" runat="server" Enabled="True" 
                        Font-Bold="True" ForeColor="#0066FF" Height="16px" MaxLength="30" 
                        ToolTip="Enter Proper Volume Nos" 
                        Width="100px"></asp:TextBox>
                 
                   &nbsp;Issue No:
                    <asp:TextBox ID="txt_JHold_IssueNo" runat="server" Enabled="True" 
                        Font-Bold="True" ForeColor="#0066FF" Height="16px" MaxLength="30" 
                        ToolTip="Enter Proper Issues No" 
                        Width="100px"></asp:TextBox>

                         &nbsp;Year*:
                    <asp:TextBox ID="txt_JHold_Year" runat="server" Enabled="True" 
                        Font-Bold="True" ForeColor="#0066FF" Height="16px" MaxLength="20" 
                        ToolTip="Enter Year of Bound Volumes" 
                        Width="70px"></asp:TextBox>
                  </td>
            </tr>
            
           
                              
           
                   
                <tr>
                <td class="style53">
                    Vol Title</td>
                <td class="style58" colspan="8">
                    
                    <asp:TextBox ID="txt_JHold_VolTitle" runat="server" Enabled="True" 
                        Font-Bold="True" ForeColor="#0066FF" Height="16px" MaxLength="250" 
                        ToolTip="Enter Volume Title, if any" Width="98%"></asp:TextBox>
                    
                </td>
            </tr>
            <tr>
                <td class="style53">
                    Vol Editors(;) </td>
                <td class="style58" colspan="8">
                    <asp:TextBox ID="txt_JHold_VolEditors" runat="server" Enabled="True" 
                        Font-Bold="True" ForeColor="#0066FF" Height="16px" MaxLength="250" 
                        ToolTip="Enter Volume Title, if any" Width="98%"></asp:TextBox>
                </td>
            </tr>

            <tr>
                <td class="style53">
                    Class No</td>
                <td class="style58" colspan="8">
                    <asp:TextBox ID="txt_JHold_ClassNo" runat="server" Enabled="True" 
                        Font-Bold="True" ForeColor="#0066FF" Height="16px" MaxLength="30" 
                        ToolTip="Enter Class No, if Classified" 
                        Width="100px"></asp:TextBox>
                 
                   &nbsp;Book No:
                    <asp:TextBox ID="txt_JHold_BookNo" runat="server" Enabled="True" 
                        Font-Bold="True" ForeColor="#0066FF" Height="16px" MaxLength="30" 
                        ToolTip="Enter Proper Book No" 
                        Width="50px"></asp:TextBox>

                         &nbsp;Pagination:
                    <asp:TextBox ID="txt_JHold_Pagination" runat="server" Enabled="True" 
                        Font-Bold="True" ForeColor="#0066FF" Height="16px" MaxLength="20" 
                        ToolTip="Enter Proper Pagination" 
                        Width="70px"></asp:TextBox>
                    Collection:
                    <asp:DropDownList ID="DDL_CollectionType" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" ToolTip="Select Collection Type">
                        <asp:ListItem Selected="True" Value="C">Circulation</asp:ListItem>
                        <asp:ListItem Value="R">Reference</asp:ListItem>
                        <asp:ListItem Value="G">Book Bank (General)</asp:ListItem>
                        <asp:ListItem Value="S">Book Bank (SCST)</asp:ListItem>
                        <asp:ListItem Value="N">Non-Returnable</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;Months: <asp:TextBox ID="txt_JHold_Period" runat="server" Enabled="True" 
                        Font-Bold="True" ForeColor="#0066FF" Height="16px" MaxLength="100" 
                        ToolTip="Enter Proper Issues No" Width="100px"></asp:TextBox>
                </td>
            </tr>

            <tr>
                <td class="style53">
                    Status*</td>
                <td class="style58" colspan="8">
                     <asp:DropDownList ID="DDL_Status" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" ToolTip="Select Status">
                     </asp:DropDownList>
                 
                  &nbsp;Binding:
                     <asp:DropDownList ID="DDL_Binding" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" ToolTip="Select Binding">
                     </asp:DropDownList>
                     &nbsp;Section:
                     <asp:DropDownList ID="DDL_Section" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" ToolTip="Select Section">
                     </asp:DropDownList>
                     &nbsp;Format*:
                     <asp:DropDownList ID="DDL_Format" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" ToolTip="Select Physical Format/Medium">
                     </asp:DropDownList>
                                        
                </td>
            </tr>
             
              <tr>
                <td class="style53">
                    Missing Issues</td>
                <td class="style58" colspan="8">
                     <asp:TextBox ID="txt_JHold_MissingIssues" runat="server" Enabled="True" 
                         Font-Bold="True" ForeColor="#0066FF" Height="16px" MaxLength="100" 
                         ToolTip="Enter Missing Issues, separated by ," Width="200px"></asp:TextBox>
                     &nbsp;Location:
                     <asp:TextBox ID="txt_JHold_Location" runat="server" Enabled="True" 
                         Font-Bold="True" ForeColor="#0066FF" Height="16px" MaxLength="100" 
                         ToolTip="Enter Location on the Self" Width="200px"></asp:TextBox>
                </td>
            </tr>
        
            <tr>
                <td class="style53">
                    Remarks</td>
                <td class="style58" colspan="8">
                    
                    <asp:TextBox ID="txt_JHold_Remarks" runat="server" Enabled="True" 
                        Font-Bold="True" ForeColor="#0066FF" Height="16px" MaxLength="250" 
                        ToolTip="Enter Missing Issues, separated by ," Width="98%"></asp:TextBox>
                    
                </td>
            </tr>
       
            <tr>
                <td class="style56" colspan="9" bgcolor="#336699">
                    <asp:Button ID="JHold_Save_Bttn" runat="server" AccessKey="s" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" Height="23px" 
                         TabIndex="14" Text="Save" OnClientClick="return valid1();"
                        ToolTip="Press to SAVE Record" Visible="False" Width="74px" />
                    <asp:Button ID="JHold_Update_Bttn" runat="server" AccessKey="u" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" Height="23px" OnClientClick="return valid2();" 
                        TabIndex="14" Text="Update" ToolTip="Press to UPDATE Record" Visible="False" 
                        Width="74px" />
                    <asp:Button ID="JHold_Cancel_Bttn" runat="server" AccessKey="c" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" Height="23px" 
                        TabIndex="14" Text="Cancel" ToolTip="Press to Cancel" Width="74px" />
                    <asp:Button ID="JHold_Delete_Bttn" runat="server" AccessKey="d" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" Height="23px" 
                        TabIndex="14" Text="Delete" ToolTip="Press to delete Record" 
                        Width="74px" />
                </td>
            </tr>

             <tr>
                <td class="style56" colspan="9" bgcolor="#336699">
                    <asp:Label ID="Label3" runat="server" Text="Journal Holdings / Copies:  " 
                        Font-Size="Medium" ForeColor="White"></asp:Label>
                    <asp:Label ID="Label35" runat="server" Font-Bold="True" Font-Size="Smaller" 
                        ForeColor="Yellow" style="font-size: x-small"></asp:Label>
                </td>
            </tr>

             <tr>
                <td class="style56" colspan="9" bgcolor="#336699">
                    <asp:Button ID="JHold_DeleteAll_Bttn" runat="server" CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red" Height="23px" TabIndex="14" 
                        Text="Delete Selected Records" 
                        ToolTip="Press to Generate Loose Issues Publishing Schedules." Visible="False" 
                        Width="200px" />
                </td>
            </tr>
            <tr>
                <td class="style56" colspan="9" bgcolor="#336699">
                   
                
                
                 <asp:Panel ID="Panel1" runat="server" Height ="250px" ScrollBars="Auto">
                       
                   <asp:GridView ID="Grid1" runat="server" AllowPaging="True" DataKeyNames="HOLD_ID"    
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
                    
                    <asp:BoundField   DataField="ACCESSION_NO" HeaderText="Accession No" SortExpression="ACCESSION_NO">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" Width="120px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="120px" Font-Names="Tahoma"/>                        
                    </asp:BoundField>
                     <asp:BoundField   DataField="ACCESSION_DATE" SortExpression="ACCESSION_DATE" HeaderText="Acc.Date" DataFormatString="{0:dd/MM/yyyy}">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="100px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="120px" Font-Names="Arial"/>                        
                    </asp:BoundField>

                    <asp:BoundField   DataField="VOL_NO" HeaderText="Volume No" SortExpression="VOL_NO">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" Width="100px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="120px" Font-Names="Tahoma"/>                        
                    </asp:BoundField>
                                      
                    <asp:BoundField   DataField="ISSUE_NO" SortExpression="ISSUE_NO" HeaderText="Issue No">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="100px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="120px" Font-Names="Arial"/>                        
                    </asp:BoundField>

                    <asp:BoundField   DataField="JYEAR" SortExpression="JYEAR" HeaderText="Year">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="100px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="120px" Font-Names="Arial"/>                        
                    </asp:BoundField>

                      <asp:BoundField   DataField="PERIOD" HeaderText="Month">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" Width="100px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="120px" Font-Names="Tahoma"/>                        
                    </asp:BoundField>

                     <asp:BoundField   DataField="PHYSICAL_LOCATION" SortExpression="PHYSICAL_LOCATION" HeaderText="Location">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="100px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="120px" Font-Names="Arial"/>                        
                    </asp:BoundField>

                     <asp:BoundField   DataField="STA_NAME" SortExpression="STA_NAME" HeaderText="Status">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="100px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="120px" Font-Names="Arial"/>                        
                    </asp:BoundField>

                     <asp:BoundField   DataField="MISSING_ISSUES"  HeaderText="Missing Issues">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="100px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="120px" Font-Names="Arial"/>                        
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
                        <asp:AsyncPostBackTrigger  ControlID="DDL_Titles" EventName="TextChanged"   />  
                        <asp:AsyncPostBackTrigger  ControlID="JHold_Save_Bttn" EventName="Click"   />   
                        <asp:AsyncPostBackTrigger  ControlID="JHold_Update_Bttn" EventName="Click"   />  
                        <asp:AsyncPostBackTrigger  ControlID="JHold_Delete_Bttn" EventName="Click"   />                                  
                   </Triggers>                
             </asp:UpdatePanel>                
                           
</div>

     
        
                   

</asp:Content>
