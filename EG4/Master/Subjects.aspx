﻿<%@ Page  Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Subjects.aspx.vb" Inherits="EG4.Subjects" SmartNavigation ="true" MaintainScrollPositionOnPostback="true"  %>


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
           ids = ["MainContent_txt_Sub_Name", "MainContent_txt_Sub_Keywords"];
           control.makeTransliteratable(ids);
           // Show the transliteration control which can be used to toggle between         
           // English and Hindi and also choose other destination language.         
           control.showControl('translControl');
       }

       google.setOnLoadCallback(onLoad);
           
    </script>
    
     
    <script language="javascript" type="text/javascript">

           function valid1() {
                if (document.getElementById('<%=txt_Sub_Name.ClientID%>').value == "") {
                    alert("Please enter Subject Heading...");
                    document.getElementById("MainContent_txt_Sub_Name").focus();
                    return (false);
                }              

                    return (true);
                }
    </script>
    <script language ="javascript" type ="text/javascript" >
        function Select(Select) {

            var grdv = document.getElementById('<%= Grid_Subject.ClientID %>');
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
                <td  bgcolor="#003366" class="style43" colspan="2" rowspan="1" style="color: #FFFFFF"><strong>Subject 
                    Directory</strong></td>
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

                      Search Text&nbsp;
                    <asp:TextBox ID="txt_Search" runat="server" MaxLength="100"  
                        ToolTip="Enter Search Term" Wrap="False" AccessKey="r" 
                        Font-Bold="True" ForeColor="#0066FF" Width="250px"></asp:TextBox>
                    &nbsp;In
                    <asp:DropDownList ID="DropDownList1" runat="server" ForeColor="#0066FF">
                        <asp:ListItem Selected="True" Value="SUB_NAME">Subject Heading</asp:ListItem>
                        <asp:ListItem Value="CLASS_NO">Class No</asp:ListItem>
                        <asp:ListItem Value="KEYWORDS">Keywords</asp:ListItem>
                    </asp:DropDownList>&nbsp;with
                    <asp:DropDownList ID="DropDownList2" runat="server" ForeColor="#0066FF">
                        <asp:ListItem Value="AND">All Words</asp:ListItem>
                        <asp:ListItem Value="OR">Any Word</asp:ListItem>
                        <asp:ListItem Selected="True" Value="LIKE">Like</asp:ListItem>
                        <asp:ListItem Value="SW">Start With</asp:ListItem>
                        <asp:ListItem Value="EW">End With</asp:ListItem>
                    </asp:DropDownList> &nbsp;Order By
                    <asp:DropDownList ID="DropDownList3" runat="server" ForeColor="#0066FF">
                        <asp:ListItem Value="SUB_ID">ID</asp:ListItem>
                        <asp:ListItem Selected="True" Value="SUB_NAME">Subject</asp:ListItem>
                        <asp:ListItem Value="CLASS_NO">Class No</asp:ListItem>
                        <asp:ListItem Value="LIB_CODE">Library Code</asp:ListItem>
                        <asp:ListItem Value="USER_CODE">User Code</asp:ListItem>
                    </asp:DropDownList>&nbsp;Sort By
                    <asp:DropDownList ID="DropDownList4" runat="server" ForeColor="#0066FF">
                        <asp:ListItem Selected="True" Value="ASC">Ascending</asp:ListItem>
                        <asp:ListItem  Value="DESC">Descending</asp:ListItem>
                    </asp:DropDownList>  

                    <asp:Button ID="Search_Bttn" runat="server"  CssClass="styleBttn" Font-Bold="True" 
                        ForeColor="Red" TabIndex="14" Text="Search" AccessKey="s"     Width="74px" />
                    <br />
                    <br />
                    <hr />

                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                     <asp:Label ID="Label1" runat="server" Text="Record(s): "></asp:Label>
                   <div>
                   <asp:Button ID="Delete_Bttn" runat="server"  CssClass="styleBttn" Font-Bold="True" 
                        ForeColor="Red" TabIndex="14" Text="Delete Selected Row(s)" AccessKey="d"    
                            Width="165px" Height="30px" Enabled="false" />
                   </div>
                                           
                  <asp:Panel ID="Panel1" runat="server" Height="250px" ScrollBars="Auto">
                   <asp:GridView ID="Grid_Subject" runat="server" AllowPaging="True" DataKeyNames="SUB_ID"  
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
                    
                   

                    <asp:BoundField   DataField="SUB_NAME" HeaderText="Subject" 
                                SortExpression="SUB_NAME" >                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="300px" 
                                Font-Names="Arial"/>                        
                    </asp:BoundField>
                    <asp:BoundField   DataField="CLASS_NO" HeaderText="Class No" ReadOnly="True"  
                                SortExpression="CLASS_NO">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" Font-Names="Arial"  
                                width="350px" />                        
                    </asp:BoundField>
                     <asp:BoundField   DataField="SUB_KEYWORDS" HeaderText="Related Keywords">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="350px" 
                                Font-Names="Arial"/>                        
                    </asp:BoundField>
                     <asp:BoundField   DataField="SUB_ID" HeaderText="ID" visible="true">                                               
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
                   
            </asp:Panel>
                   

                   </ContentTemplate>
                   <Triggers>
                         <asp:AsyncPostBackTrigger ControlID="Delete_Bttn" EventName="Click" />  
                          <asp:AsyncPostBackTrigger ControlID="Search_Bttn" EventName="Click" />  
                          <asp:AsyncPostBackTrigger ControlID="Grid_Subject" EventName="RowCommand" />                                             
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
                    Subject Heading*</td>
                <td class="style54">
                    <asp:TextBox ID="txt_Sub_Name" runat="server" MaxLength="100"  
                        ToolTip="Enter Subject Heading" Wrap="False" style="text-transform: uppercase"
                        Font-Bold="True" ForeColor="#0066FF" Height="20px" Width="98%"></asp:TextBox>
                    &nbsp; <asp:Label ID="Label7" runat="server"  Text=""></asp:Label> 
                </td>
               
            </tr>
            <tr>
                <td class="style51"> 
                    Class No</td>
                <td class="style54">
                    <asp:TextBox ID="txt_Sub_ClassNo" runat="server" MaxLength="50"  
                        ToolTip="Enter Class Number" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Height="19px" Width="98%"></asp:TextBox>
                    &nbsp;</td>
               
            </tr>
            
            <tr>
                <td class="style51">Keywords </td>
                <td class="style53">
                    <asp:TextBox ID="txt_Sub_Keywords" runat="server" MaxLength="400"  
                        ToolTip="Enter Related Keywords separated by ; " Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="98%" style="text-transform: uppercase"  
                        AutoCompleteType="DisplayName" Height="16px"></asp:TextBox>
                    </td>
            </tr>
            
            <tr>
                <td class="style51">Parent Subject</td>
                <td class="style53">
                    <asp:DropDownList ID="DropDownList_Sub" runat="server" Font-Bold="True" 
                        ToolTip="Select Broader/Parent Subject from DropDown" ForeColor="#006699">
                    </asp:DropDownList>
                    &nbsp;Select Subject Heading which is Parent of the Subject being added.</td>
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
