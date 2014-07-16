<%@ Page  Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Commiittee_Members.aspx.vb" Inherits="EG4.Commiittee_Members" %>

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
           ids = ["MainContent_txt_ComMember_Name", "MainContent_txt_ComMember_Desig", "MainContent_txt_ComMember_Qual", "MainContent_txt_ComMember_Resp", "MainContent_txt_ComMember_Remarks", "MainContent_txt_Search"];
           control.makeTransliteratable(ids);
           // Show the transliteration control which can be used to toggle between         
           // English and Hindi and also choose other destination language.         
           control.showControl('translControl');
       }

       google.setOnLoadCallback(onLoad);
                   
           
    </script> 
    <script language="javascript" type="text/javascript">

           function valid1() {
                var name = "";
                var desig = "";
                var email = "";
              
                name = document.getElementById('<%=txt_ComMember_Name.ClientID%>').value;
                desig = document.getElementById('<%=txt_ComMember_Desig.ClientID%>').value;
                email = document.getElementById('<%=txt_ComMember_Email.ClientID%>').value;
                
                if (document.getElementById('<%=txt_ComMember_Name.ClientID%>').value == "") {
                    alert("Please enter proper \"Full Name\" field.");
                    document.getElementById("MainContent_txt_ComMember_Name").focus();
                    return (false);
                }

                if (document.getElementById('<%=txt_ComMember_Desig.ClientID%>').value == "") {
                    alert("Please enter proper \"Designation\" field.");
                    document.getElementById("MainContent_txt_ComMember_Desig").focus();
                    return (false);
                }


                if (email == "") {
                    alert("Please enter proper \"E-Mail\" field.");
                    document.getElementById("MainContent_txt_ComMember_Email").focus();
                    return (false);
                }
                re = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
                if (!re.test(document.getElementById('<%=txt_ComMember_Email.ClientID%>').value)) {
                    alert("Error: Not a proper mail address!");
                    document.getElementById("MainContent_txt_ComMember_Email").focus();
                    return false;
                }
                    return (true);
                }
    </script>
    <script language ="javascript" type ="text/javascript" >
        function Select(Select) {

            var grdv = document.getElementById('<%= Grid1_LibTeam.ClientID %>');
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
                <td  bgcolor="#003366" class="style43" colspan="2" rowspan="1" style="color: #FFFFFF"><strong>Manage Library Team</strong></td>
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
                
                        
                    Search Text&nbsp;
                    <asp:TextBox ID="txt_Search" runat="server" MaxLength="100"  
                        ToolTip="Enter Search Term" Wrap="False" AccessKey="r" 
                        Font-Bold="True" ForeColor="#0066FF" Width="250px"></asp:TextBox>
                    &nbsp;In
                    <asp:DropDownList ID="DropDownList2" runat="server" ForeColor="#0066FF">
                        <asp:ListItem Selected="True" Value="MEMBER_NAME">Name</asp:ListItem>
                        <asp:ListItem Value="MEMBER_DESIG">Designation</asp:ListItem>
                    </asp:DropDownList>&nbsp;with
                    <asp:DropDownList ID="DropDownList3" runat="server" ForeColor="#0066FF">
                        <asp:ListItem Value="AND">All Words</asp:ListItem>
                        <asp:ListItem Value="OR">Any Word</asp:ListItem>
                        <asp:ListItem Selected="True" Value="LIKE">Like</asp:ListItem>
                        <asp:ListItem Value="SW">Start With</asp:ListItem>
                        <asp:ListItem Value="EW">End With</asp:ListItem>
                    </asp:DropDownList> &nbsp; Status
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
                                           

                   <asp:GridView ID="Grid1_LibTeam" runat="server" AllowPaging="True" DataKeyNames="COMMEM_ID"  
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
                    
                   

                    <asp:BoundField   DataField="MEMBER_NAME" HeaderText="Name" 
                                SortExpression="MEMBER_NAME" >                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="130px" 
                                Font-Names="Arial"/>                        
                    </asp:BoundField>
                    <asp:BoundField   DataField="MEMBER_DESIG" HeaderText="Designation" ReadOnly="True"  
                                SortExpression="MEMBER_DESIG">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" Font-Names="Arial"  
                                width="150px" />                        
                    </asp:BoundField>
                     <asp:BoundField   DataField="MEMBER_RANK" HeaderText="Rank" 
                                SortExpression="MEMBER_RANK" >                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="50px" 
                                Font-Names="Arial"/>                        
                    </asp:BoundField>
                     <asp:BoundField   DataField="MEMBER_QUL" HeaderText="Qualification" >                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" 
                                Font-Names="Arial"/>                        
                    </asp:BoundField>
                     <asp:BoundField   DataField="MEMBER_RESP" HeaderText="Responsibility">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="200px" 
                                Font-Names="Arial"/>                        
                    </asp:BoundField>
                     <asp:BoundField   DataField="MEMBER_REMARKS" HeaderText="Remarks">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="200px" 
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
                        <asp:AsyncPostBackTrigger ControlID="Search_Bttn" EventName="Click" />  
                        <asp:AsyncPostBackTrigger ControlID="Grid1_LibTeam" EventName="RowCommand" />                                     
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
                <td class="style47" colspan="3">
                     <asp:Label ID="Label6" runat="server" Font-Size="Medium" ForeColor="#CC3300" 
                         style="font-weight: 700"></asp:Label>
                 </td>
            </tr>
            
            <tr>
                <td class="style51"> 
                    Name *</td>
                <td class="style54">
                    <asp:TextBox ID="txt_ComMember_Name" runat="server" MaxLength="250"  
                        ToolTip="Enter Proper Name" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Height="22px" Width="403px"></asp:TextBox>
                    &nbsp;<asp:Label ID="Label7" runat="server" Text="Label"></asp:Label>
                </td>
               
                <td rowspan="4" style="background-color:  #99CCFF" align="center" 
                    valign="middle"><asp:Image ID="Image21" runat="server" Height="100px" 
                         Width="80px" BorderStyle="None" 
                        ImageAlign="Middle"/> </td>
               
            </tr>
            <tr>
                <td class="style51">Designation *</td>
                <td class="style53">
                    <asp:TextBox ID="txt_ComMember_Desig" runat="server" MaxLength="250"  
                        ToolTip="Designation" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="402px"  
                        AutoCompleteType="DisplayName" Height="23px"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td class="style51">Rank *</td>
                <td class="style52">
                    <asp:DropDownList ID="DropDownList_Rank" runat="server" 
                        ToolTip="Select Rank of the ComMember">
                        <asp:ListItem Selected="True" Value="1">Chairman</asp:ListItem>
                        <asp:ListItem Value="2">Member</asp:ListItem>
                        <asp:ListItem Value="3">Convenor</asp:ListItem>
                        <asp:ListItem Value="4">Other</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;1= Library In-Charge, 2 downwrds for lower rank</td>
            </tr>
             <tr>
                <td class="style51">Committee*</td>
                <td class="style52" colspan="2">
                    <asp:DropDownList ID="DDL_Committees" runat="server" 
                        Font-Bold="True" ForeColor="#0066CC" Width="80%">
                    </asp:DropDownList>
                    <ajaxToolkit:ListSearchExtender ID="DDL_Committees_ListSearchExtender" 
                        runat="server" Enabled="True" IsSorted="True" PromptCssClass="PromptCSS" 
                        TargetControlID="DDL_Committees">
                    </ajaxToolkit:ListSearchExtender>
                </td>
            </tr>
            <tr>
                <td class="style51">Phone</td>
                <td class="style52" colspan="2">
                    <asp:TextBox ID="txt_ComMember_Phone" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" MaxLength="150" ToolTip="Enter Phone Number" Width="250px" 
                        Wrap="False"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style51">Mobile </td>
                <td class="style52" colspan="2">
                    <asp:TextBox ID="txt_ComMember_Mobile" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" MaxLength="150" ToolTip="Enter Mobile Number" Width="250px" 
                        Wrap="False"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td class="style51">Email*</td>
                <td class="style52" colspan="2">
                    <asp:TextBox ID="txt_ComMember_Email" runat="server" MaxLength="150"  
                        ToolTip="Enter Email" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="250px"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td class="style51">Qualification</td>
                <td class="style52" colspan="2">
                    &nbsp;<asp:TextBox ID="txt_ComMember_Qual" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="62px" MaxLength="250" TextMode="MultiLine" 
                        ToolTip="Enter Qualifications" Width="400px" Wrap="False"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td class="style51">Responisbilities</td>
                <td class="style52" colspan="2">
                    <asp:TextBox ID="txt_ComMember_Resp" runat="server" MaxLength="1000"  
                        ToolTip="Enter Responsibilities" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="400px" Height="62px" 
                        TextMode="MultiLine"></asp:TextBox>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style51">Remarks</td>
                <td class="style52" colspan="2">
                    <asp:TextBox ID="txt_ComMember_Remarks" runat="server" MaxLength="250"  
                        ToolTip="Enter Remarks" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="400px" Height="62px" 
                        TextMode="MultiLine"></asp:TextBox>
                    </td>
            </tr>

             
            <tr>
                <td class="style51">Select Photo</td>
                <td class="style52" colspan="2">
                    <asp:FileUpload ID="FileUpload13" runat="server" ToolTip="Browse Photo" />
                </td>
            </tr>
            <tr>
                <td class="style47" colspan="3">
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
                <td  bgcolor="#99CCFF" class="style44" colspan="3" rowspan="0">
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
