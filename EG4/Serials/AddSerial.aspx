<%@ Page  Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="AddSerial.aspx.vb" Inherits="EG4.AddSerial" %>


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
            width: 18%;
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
        }
        .style55
        {
            text-align: justify;
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
            color:DodgerBlue;  
            font-size:small;  
            font-style:italic;  
            font-weight:bold;  
            font-family:Arial;
            background-color:Silver;  
            height:25px;    
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
    <script type="text/javascript" src="http://www.google.com/jsapi"></script> 
 <script type="text/javascript">
     // Load the Google Transliteration API   

     google.load("elements", "1", {
         packages: "transliteration"
     });
     var ids = ""
     var t1 = ""
     var k=""
     function onLoad() {
         var options = {
             sourceLanguage: 'en',
             destinationLanguage: ['hi', 'bn', 'gu', 'kn', 'ml', 'mr', 'pa', 'sa', 'ta', 'te', 'ur'],
             shortcutKey: 'ctrl+g',
             transliterationEnabled: false
         };
         // Create an instance on TransliterationControl with the required              
         var control = new google.elements.transliteration.TransliterationControl(options);
         var elem = "MainContent_txt_Cat_Title,MainContent_txt_Cat_VarTitle,MainContent_txt_Cat_SubTitle,MainContent_txt_Cat_GuideName,MainContent_txt_Cat_ScholarName,MainContent_txt_Cat_ScholarDept,MainContent_txt_Cat_GuideDept,MainContent_txt_Cat_DegreeName,MainContent_txt_Cat_Patentee,MainContent_txt_Cat_PatentInventor,MainContent_txt_Cat_ConfName,MainContent_txt_Cat_Author1,MainContent_txt_Cat_Author2,MainContent_txt_Cat_Author3,MainContent_txt_Cat_Editor,MainContent_txt_Cat_Translator,MainContent_txt_Cat_Illustrator,MainContent_txt_Cat_Compiler,MainContent_txt_Cat_Commentator,MainContent_txt_Cat_RevisedBy,MainContent_txt_Cat_CorpAuthor,MainContent_txt_Cat_Place,MainContent_txt_Cat_Note,MainContent_txt_Cat_Remarks,MainContent_txt_Cat_Comments,MainContent_txt_Cat_Keywords,MainContent_txt_Cat_TrFrom,MainContent_txt_Cat_Abstract";
         //alert(elements);
         var e = elem.split(",");
         var idArray = new Array();
         //alert(e);
         for (var j = 0; j < e.length; j++) {
             //alert(e[j]);
             var docs = document.getElementById(e[j]);
             //alert(docs);
             if (docs != null) {
                 idArray.push(e[j]);
             }
         }

//         ids = ["MainContent_txt_Search", "MainContent_txt_Cat_Title"];

           control.makeTransliteratable(idArray);
         // Show the transliteration control which can be used to toggle between         
         // English and Hindi and also choose other destination language.         
         control.showControl('translControl');
     }

     google.setOnLoadCallback(onLoad);
           
    </script>

 <script type="text/javascript" src="http://www.google.com/jsapi"></script> 
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
    <script language="javascript" type="text/javascript">

         function valid1() {
            var bibtype = "";
            var mattype = "";
            var doctype = "";
            var lang = "";
            var pub = "";
            var country = "";
            var currentyear = "";
   
            bibtype = document.getElementById('<%=DDL_Bib_Level.ClientID%>').value;
            mattype = document.getElementById('<%=DDL_Mat_Type.ClientID%>').value;
            doctype = document.getElementById('<%=DDL_Doc_Type.ClientID%>').value;
            lang = document.getElementById('<%=DDL_Lang.ClientID%>').value;
            currentyear = new Date().getFullYear()+1;
           
            
            if (bibtype == "") {
                alert("Please Select  \"Bibliographic Level \" field.");
                document.getElementById("MainContent_DDL_Bib_Level").focus();
                return (false);
            }
            if (mattype == "") {
                alert("Please Select  \"Material Type \" field.");
                document.getElementById("MainContent_DDL_Mat_Type").focus();
                return (false);
            }
            if (doctype == "") {
                alert("Please Select  \"Document Type  \" field.");
                document.getElementById("MainContent_DDL_Doc_Type").focus();
                return (false);
            }
            if (lang == "") {
                alert("Please Select  \"Language \" field.");
                document.getElementById("MainContent_DDL_Lang").focus();
                return (false);
            }
           
            if (document.getElementById('<%=txt_Cat_Title.ClientID%>').value == "") {
                alert("Please enter proper \"Title\" field.");
                document.getElementById("MainContent_txt_Cat_Title").focus();
                return (false);
            }
            if (document.getElementById('<%=txt_Cat_Title.ClientID%>').value.length < 3) {
                alert("Length of \"Title\" should be Min 3 characters.");
                document.getElementById("MainContent_txt_Cat_Title").focus();
                return (false);
            }

            return (true);
        }

    </script>
     <script language ="javascript" type ="text/javascript" >
         function Select(Select) {

             var grdv = document.getElementById('<%= Grid1_Search.ClientID %>');
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
            document.getElementById('<%= DDL_Bib_Level.ClientID %>').focus();
        }
        window.onload = formfocus;
  </script>
     
        <table id="ADMT1" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
            <tr>
                <td  bgcolor="#003366" class="style43" rowspan="1" style="color: #FFFFFF"><strong>Add New Serial Details</strong></td>
            </tr>
            <tr>  
                <td  bgcolor="#99CCFF"  colspan="2" style="text-align: center">
                 
                     Type in Indian languages (Press Ctrl+g to toggle between English and India Language)
                     <div id='translControl'></div>
                </td>
            </tr>

             <tr>                
                <td  align="center">   
                 <ajaxToolkit:Accordion ID="Accordion2" runat="server" CssClass="accordion"  
                        HeaderCssClass="accordionHeader"  
                        HeaderSelectedCssClass="accordionHeaderSelected"  
                        ContentCssClass="accordionContent" Enabled="true" Visible="true" 
                         RequireOpenedPane="false"     SuppressHeaderPostbacks="true"   
                        SelectedIndex="1"   Width="100%" Height="237px"   >  
                            <Panes>  
                                <ajaxToolkit:AccordionPane ID="Search_Pane" runat="server" >  
                                    <Header>Click To View / Hide Search Pane</Header>  
                                        <Content>    
                &nbsp; Bib Level: 
                    <asp:DropDownList ID="DDL_Bib_LevelSearch" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC" AutoPostBack="True" 
                        ToolTip="Plz Select Value from Drop-Down" Enabled="False">
                    </asp:DropDownList>
                     &nbsp; Materials Type: 
                    <asp:DropDownList ID="DDL_Mat_TypeSearch" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC" AutoPostBack="True" 
                        ToolTip="Plz Select Value from Drop-Down">
                    </asp:DropDownList>
                     &nbsp; Documents Type: 
                    <asp:DropDownList ID="DDL_Doc_TypeSearch" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC" 
                        ToolTip="Plz Select Value from Drop-Down">
                    </asp:DropDownList>
                    <br />
                        
                    Search Text&nbsp;
                    <asp:TextBox ID="txt_Search" runat="server" MaxLength="100"  
                        ToolTip="Enter Search Term" Wrap="False" AccessKey="r" 
                        Font-Bold="True" ForeColor="#0066FF" Width="250px"></asp:TextBox>
                    &nbsp;In: 
                    <asp:DropDownList ID="DropDownList1" runat="server" ForeColor="#0066FF">
                        <asp:ListItem Selected="True" Value="TITLE">Title</asp:ListItem>
                        <asp:ListItem Value="SUB_TITLE">Sub Title</asp:ListItem>
                        <asp:ListItem Value="CORPORATE_AUTHOR">Corporate Author</asp:ListItem>
                        <asp:ListItem Value="EDITOR">Editor</asp:ListItem>
                        <asp:ListItem Value="KEYWORDS">Keywords</asp:ListItem>                      
                        <asp:ListItem>Note</asp:ListItem>
                        <asp:ListItem Value="TAGS">Tags</asp:ListItem>
                        <asp:ListItem Value="PUB_NAME">Publisher</asp:ListItem>
                        <asp:ListItem Value="SUB_NAME">Subject</asp:ListItem>
                    </asp:DropDownList>&nbsp;Operator: 
                    <asp:DropDownList ID="DropDownList2" runat="server" ForeColor="#0066FF">
                        <asp:ListItem Value="AND" Selected="True">All Words</asp:ListItem>
                        <asp:ListItem Value="OR">Any Word</asp:ListItem>
                        <asp:ListItem Value="LIKE">Like</asp:ListItem>
                        <asp:ListItem Value="SW">Start With</asp:ListItem>
                        <asp:ListItem Value="EW">End With</asp:ListItem>
                    </asp:DropDownList> &nbsp;Order By: 
                    <asp:DropDownList ID="DropDownList3" runat="server" ForeColor="#0066FF">
                        <asp:ListItem Selected="True" Value="TITLE">Title</asp:ListItem>
                        <asp:ListItem Value="PUB_NAME">Publisher</asp:ListItem>
                        <asp:ListItem Value="CAT_NO">Cat No</asp:ListItem>
                    </asp:DropDownList> &nbsp;Sort By: 
                     <asp:DropDownList ID="DropDownList4" runat="server" ForeColor="#0066FF">
                        <asp:ListItem Selected="True" Value="ASC">Ascending</asp:ListItem>
                        <asp:ListItem Value="DESC">Descending</asp:ListItem>
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
                   <asp:Button ID="Delete_Bttn" runat="server"  CssClass="styleBttn" Font-Bold="True"  ToolTip="Click to DELETE Catalog Record(s) from Database"
                        ForeColor="Red" TabIndex="14" Text="Delete Selected Titles" AccessKey="t"    
                            Width="165px" Height="20px" Enabled="false" />

                   <asp:Button ID="Delete_Photo_Bttn" runat="server"  CssClass="styleBttn" Font-Bold="True"  ToolTip="Click to Delete Cover Photo from Database"
                        ForeColor="Red" TabIndex="14" Text="Delete Cover Photo from Selected Titles" AccessKey="p"    
                            Width="270px" Height="20px" Enabled="false" />
                   </div>
                                           
                        <asp:Panel ID="Panel1" runat="server" Height ="250px" ScrollBars="Auto">
                       
                   <asp:GridView ID="Grid1_Search" runat="server" AllowPaging="True" DataKeyNames="CAT_NO"    
                        style="width: 100%;  text-align: center;" allowsorting="True" 
                        AutoGenerateColumns="False" PageSize="10"  Font-Bold="True" 
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
                    
                   

                    <asp:BoundField   DataField="TITLE" HeaderText="Title" SortExpression="TITLE">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" Width="650px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="850px" Font-Names="Tahoma"/>                        
                    </asp:BoundField>
                                      
                    <asp:BoundField   DataField="CAT_NO" SortExpression="CAT_NO" HeaderText="Cat No" visible="true">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="20px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="50px" Font-Names="Arial"/>                        
                    </asp:BoundField>
                     
                    <asp:TemplateField  ControlStyle-Width="10px"  HeaderText="Delete" FooterText="Select to Delete" ShowHeader="true">
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
                        <asp:AsyncPostBackTrigger ControlID="Delete_Bttn" EventName="Click" /> 
                        <asp:AsyncPostBackTrigger ControlID="Delete_Photo_Bttn" EventName="Click" />  
                        <asp:AsyncPostBackTrigger ControlID="Grid1_Search" EventName="RowCommand" />  
                                                                             
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
                <td class="style56" colspan="12" bgcolor="#003366">
                   <asp:Label ID="Label2" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">Display Record: Type Cat No / ISBN or Search in the Above Pane</asp:Label>
                </td>
            </tr>
             <tr style=" color:Green">
                <td class="style53"> 
                    Display Record:</td>
                <td class="style55" colspan="3">
                    <asp:TextBox ID="txt_Display_Value" runat="server" 
                        AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                        Height="18px" MaxLength="15" ToolTip="Enter Data and press ENTER to Display Record" 
                        Width="44%" Wrap="False" AutoPostBack="True"></asp:TextBox> 
                    <asp:DropDownList ID="DDL_Display" runat="server" 
                        Font-Bold="True" ForeColor="#0066CC" 
                        ToolTip="Plz Select Value from Drop-Down">
                        <asp:ListItem Selected="True" Value="CAT_NO">Cat No</asp:ListItem>
                        <asp:ListItem Value="STANDARD_NO">ISBN</asp:ListItem>
                        <asp:ListItem Value="ACCESSION_NO">Accession No</asp:ListItem>
                    </asp:DropDownList>
                 </td>
                <td class="style55" colspan="8">
                    <asp:Label ID="Label14" runat="server" Text="Press ENTER after Typing the Data"></asp:Label>
                 </td>
                
            </tr>
             <tr>
                <td class="style56" colspan="12">
                    <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Size="Small" 
                        ForeColor="White">HELP: In case no fields are displayed to Enter Data - Create Data Entry Format for Selected Doc type in LIBRARY ADMIN Module.</asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style56" colspan="12">
                   <asp:Label ID="Label6" runat="server" Font-Size="Medium" ForeColor="Yellow" 
                        Font-Bold="True"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style56" colspan="12">
                    <asp:Label ID="Label15" runat="server" Font-Bold="True" Font-Size="Medium" 
                        ForeColor="White"></asp:Label>
                </td>
            </tr>
            
            <tr>
                <td class="style53"> 
                    Document Category*</td>
                <td class="style55" colspan="3">
                    <asp:DropDownList ID="DDL_Bib_Level" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC" AutoPostBack="True" 
                        ToolTip="Plz Select Value from Drop-Down">
                    </asp:DropDownList>
                    &nbsp;<asp:Label ID="Label7" runat="server" Text=""></asp:Label>
                </td>
                <td class="style55" colspan="5">
                    <asp:DropDownList ID="DDL_Mat_Type" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC" AutoPostBack="True" 
                        ToolTip="Plz Select Value from Drop-Down">
                    </asp:DropDownList>
                </td>
                <td class="style55" colspan="3">
                    <asp:DropDownList ID="DDL_Doc_Type" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC" AutoPostBack="True" 
                        ToolTip="Plz Select Value from Drop-Down">
                    </asp:DropDownList>
                </td>
                
            </tr>
            <tr>
                <td class="style53"> 
                    Language*</td>
                <td class="style55" colspan="3">
                    <asp:DropDownList ID="DDL_Lang" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC" ToolTip="Plz Select Value from Drop-Down">
                    </asp:DropDownList>
                </td>
                <td class="style55" colspan="5">
                    &nbsp;</td>
                <td class="style55" colspan="3">
                    <asp:Label ID="Label12" runat="server" Font-Size="Smaller"></asp:Label>
                </td>
              
            </tr>
             <tr id ="TR_ISBN" runat="server">
                <td class="style53"> 
                    ISSN</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_ISBN" runat="server" AutoCompleteType="DisplayName"  
                        Font-Bold="True" ForeColor="#0066FF" Height="20px" MaxLength="50" 
                        ToolTip="Enter ISBN" Width="20%" Wrap="False"></asp:TextBox>
                 </td>
                
            </tr>
            <tr id="TR_SPNo" runat="server">
                <td class="style53"> 
                    Standard Number</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_SPNo" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="100" 
                        ToolTip="Enter Standard Number" Width="99%" Wrap="False"></asp:TextBox>
                </td>
               
            </tr>
             <tr id="TR_ManualNo" runat="server">
                <td class="style53"> 
                    Manual Number</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_ManualNo" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="100" 
                        ToolTip="Enter Manual No" Width="200px" Wrap="False"></asp:TextBox>
                 </td>
               
            </tr>
             <tr id="TR_ReportNo" runat="server">
                <td class="style53"> 
                    Report Number</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_ReportNo" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="100" 
                        ToolTip="Enter Report Number" Width="200px" Wrap="False"></asp:TextBox>
                 </td>
               
            </tr>

             <tr id="TR_ACT" runat="server">
                <td class="style53"> 
                    Act No</td>
                <td class="style54" colspan="5">
                    <asp:TextBox ID="txt_Cat_ActNo" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="22px" MaxLength="150" 
                        ToolTip="Enter Act No" Width="350px" Wrap="False"></asp:TextBox>
                  </td>
                 <td class="style55" colspan="6">
                     Year of Act:
                      <ajaxToolkit:ListSearchExtender 
                        ID="ListSearchExtender3" 
                        runat="server" 
                        Enabled="True" 
                        PromptCssClass="PromptCSS" 
                        TargetControlID="DDL_Countries">
                    </ajaxToolkit:ListSearchExtender>
                     <asp:TextBox ID="txt_Cat_ActYear" runat="server" AutoCompleteType="DisplayName" 
                         Font-Bold="True" ForeColor="#0066FF" Height="16px" MaxLength="4" 
                         onkeypress="return isNumberKey(event)" 
                         ToolTip="Enter Year in YYYY format" Width="6%" Wrap="False"></asp:TextBox>
                     <ajaxToolkit:AutoCompleteExtender ID="txt_Cat_ActYear_AutoCompleteExtender" 
                         runat="server" CompletionSetCount="10" EnableCaching="true" Enabled="True" 
                         FirstRowSelected="false" MinimumPrefixLength="1" ServiceMethod="SearchYear" 
                         TargetControlID="txt_Cat_ActYear">
                     </ajaxToolkit:AutoCompleteExtender>
                     
                 </td>
               
            </tr>
             <tr>
                <td class="style53"> 
                    Title *</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_Title" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" ToolTip="Enter Title Proper" 
                        Width="99%" Wrap="False" style="text-transform: uppercase"  ></asp:TextBox>
                    <ajaxToolkit:AutoCompleteExtender 
                        ID="txt_Cat_Title_AutoCompleteExtender" 
                        runat="server" 
                        MinimumPrefixLength="1"
                        EnableCaching="true" 
                        CompletionSetCount="10"
                        ServiceMethod="SearchTitle"
                        TargetControlID="txt_Cat_Title" 
                        FirstRowSelected = "false">
                    </ajaxToolkit:AutoCompleteExtender>
                 </td>
                
            </tr>
             <tr id="TR_SubTitle" runat="server">
                <td class="style53"> 
                    Sub Title</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_SubTitle" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter Sub Title" Width="99%" Wrap="False"></asp:TextBox>
                    <ajaxToolkit:AutoCompleteExtender 
                        ID="txt_Cat_SubTitle_AutoCompleteExtender" 
                        runat="server" 
                        MinimumPrefixLength="1"
                        EnableCaching="true" 
                        CompletionSetCount="10"
                        ServiceMethod="SearchSubTitle"
                        FirstRowSelected = "false"                       
                        TargetControlID="txt_Cat_SubTitle" 
                        UseContextKey="True">
                    </ajaxToolkit:AutoCompleteExtender>
                 </td>
               
            </tr>
             <tr id="TR_VarTitle" runat="server">
                <td class="style53"> 
                    Var Title</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_VarTitle" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter Var Title" Width="99%" Wrap="False"></asp:TextBox>
                 </td>
                
            </tr>
             <tr id="TR_ScholarName" runat="server">
                <td class="style53"> 
                    Scholar Name</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_ScholarName" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter Name of Scholar" Width="99%" Wrap="False"></asp:TextBox>
                 </td>
               
            </tr>
             <tr id="TR_ScholarDepartment" runat="server">
                <td class="style53"> 
                    Scholar Department</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_ScholarDept" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter Scholar Department" Width="99%" Wrap="False"></asp:TextBox>
                 </td>
               
            </tr>
             <tr id="TR_GuideName" runat="server">
                <td class="style53"> 
                    Guide Name</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_GuideName" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter Guide Name" Width="99%" Wrap="False"></asp:TextBox>
                 </td>
                
            </tr>
             <tr id="TR_GuideDepartment" runat="server">
                <td class="style53"> 
                    Guide Department</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_GuideDept" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter Guide Department" Width="99%" Wrap="False"></asp:TextBox>
                 </td>
                
            </tr>
             <tr id="TR_DegreeName" runat="server">
                <td class="style53"> 
                    Degree Name</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_DegreeName" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter Degree Name" Width="99%" Wrap="False"></asp:TextBox>
                 </td>
               
            </tr>
             <tr id="TR_SPRevision" runat="server">
                <td class="style53"> 
                    Standard Rev. No</td>
                <td class="style54" colspan="3">
                    <asp:TextBox ID="txt_Cat_SPRevision" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="100" 
                        ToolTip="Enter Standard Rev/Ver No." Width="80%" Wrap="False"></asp:TextBox>
                 </td>
               <td class="style53" colspan="2"> 
                    Re-Affirmation Year</td>
                 <td class="style54" colspan="3">
                     <asp:TextBox ID="txt_Cat_ReaffirmYear" runat="server" 
                         AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                         Height="16px" MaxLength="4" onkeypress="return isNumberKey(event)" 
                         ToolTip="Enter Year of Re-Affirmation in YYYY format" Width="50px" Wrap="False"></asp:TextBox>
                     <ajaxToolkit:AutoCompleteExtender ID="txt_Cat_ReaffirmYear_AutoCompleteExtender" 
                         runat="server" CompletionSetCount="10" EnableCaching="true" Enabled="True" 
                         FirstRowSelected="false" MinimumPrefixLength="1" ServiceMethod="SearchYear" 
                         TargetControlID="txt_Cat_ReaffirmYear">
                     </ajaxToolkit:AutoCompleteExtender>
                 </td>
                 <td class="style53" colspan="2">
                     Withdraw Year</td>
                 
               
                 <td class="style54">
                     <asp:TextBox ID="txt_Cat_WithdrawYear" runat="server" 
                         AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                         Height="16px" MaxLength="4" onkeypress="return isNumberKey(event)" 
                         ToolTip="Enter Year of Re-Affirmation in YYYY format" Width="50px" Wrap="False"></asp:TextBox>
                     <ajaxToolkit:AutoCompleteExtender ID="txt_Cat_WithdrawYear_AutoCompleteExtender" 
                         runat="server" CompletionSetCount="10" EnableCaching="true" Enabled="True" 
                         FirstRowSelected="false" MinimumPrefixLength="1" ServiceMethod="SearchYear" 
                         TargetControlID="txt_Cat_WithdrawYear">
                     </ajaxToolkit:AutoCompleteExtender>
                 </td>
                 
               
            </tr>
             <tr id="TR_SP_ISSUE_BODY" runat="server">
                <td class="style53"> 
                    Issuing Body</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_SPIssueBody" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="150" 
                        ToolTip="Enter Standard Issuing Body Details" Width="99%" Wrap="False"></asp:TextBox>
                 </td>
               
            </tr>
            <tr id="TR_SP_TCSC" runat="server">
                <td class="style53"> 
                    Technical Committee</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_SPTCSC" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="150" 
                        ToolTip="Enter Technical Committee Details" Width="99%" Wrap="False"></asp:TextBox>
                 </td>
               
            </tr>
             <tr id="TR_SP_UPDATES" runat="server">
                <td class="style53"> 
                    Updates Details</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_SPUpdates" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="150" 
                        ToolTip="Enter Updates Details" Width="99%" Wrap="False"></asp:TextBox>
                 </td>
               
            </tr>
             <tr id="TR_SP_AMMENDMENTS" runat="server">
                <td class="style53"> 
                    Ammendments Details</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_SPAmmendments" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="150" 
                        ToolTip="Enter Ammendments Details" Width="99%" Wrap="False"></asp:TextBox>
                 </td>
               
            </tr>
             <tr id="TR_ManualRev" runat="server">
                <td class="style53"> 
                    Manual Ver/Rev No</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_ManualRevision" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="50" 
                        ToolTip="Enter Manual Ver/Rev No" Width="200px" Wrap="False"></asp:TextBox>
                 </td>
               
            </tr>
             <tr id="TR_PatentNo" runat="server">
                <td class="style53"> 
                    Patent Number</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_PatentNo" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="100" 
                        ToolTip="Enter Patent Number" Width="200px" Wrap="False"></asp:TextBox>
                 </td>
                
            </tr>
             <tr id="TR_Patentee" runat="server">
                <td class="style53"> 
                    Patentee</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_Patentee" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="150" 
                        ToolTip="Enter Patentee" Width="99%" Wrap="False"></asp:TextBox>
                 </td>
               
            </tr>
             <tr id="TR_PatentInventor" runat="server">
                <td class="style53"> 
                    Patent Inventors;</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_PatentInventor" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="255" 
                        ToolTip="Plz Enter Patent Inventor" Width="99%" Wrap="False"></asp:TextBox>
                 </td>
               
            </tr>
             <tr id="TR_ConfName" runat="server">
                <td class="style53"> 
                    Conference Name</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_ConfName" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter Conference Name" Width="99%" Wrap="False"></asp:TextBox>
                    <ajaxToolkit:AutoCompleteExtender 
                        ID="txt_Cat_ConfName_AutoCompleteExtender" 
                        runat="server" 
                        Enabled="True" 
                        MinimumPrefixLength="1"
                        EnableCaching="true" 
                        CompletionSetCount="10"
                        ServiceMethod="SearchConfName"
                        FirstRowSelected = "false"     
                        TargetControlID="txt_Cat_ConfName">
                    </ajaxToolkit:AutoCompleteExtender>
                 </td>
               
            </tr>
             <tr id="TR_ConfDetails" runat="server">
                <td class="style53"> 
                    Conference Details</td>
                <td class="style55" colspan="3">
                    Date From:
                    <asp:TextBox ID="txt_Cat_SDate" runat="server" ForeColor="#0066FF" 
                        Height="16px" MaxLength="10" ToolTip="Click to Select Date" Width="71px"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="txt_Cat_SDate_CalendarExtender" 
                        runat="server" Enabled="True" Format="dd/MM/yyyy" 
                        TargetControlID="txt_Cat_SDate">
                    </ajaxToolkit:CalendarExtender>
                </td>
                <td class="style55" colspan="5">
                    Date To:&nbsp;
                    <asp:TextBox ID="txt_Cat_EDate" runat="server" EnableTheming="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="10" ToolTip="Click to Select Date" 
                        Width="71px"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="txt_Cat_EDate_CalendarExtender" 
                        runat="server" Enabled="True" Format="dd/MM/yyyy" 
                        TargetControlID="txt_Cat_EDate">
                    </ajaxToolkit:CalendarExtender>
                </td>
                <td class="style55" colspan="3">
                    Conf. Place:
                    <asp:TextBox ID="txt_Cat_ConfPlace" runat="server" 
                        AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                        Height="20px" MaxLength="100" ToolTip="Enter Conference Place" Width="70%" 
                        Wrap="False"></asp:TextBox>
                </td>
                
            </tr>
             <tr id="TR_Author" runat="server">
                <td class="style53"> 
                    Authors</td>
                <td class="style55" colspan="3">
                    Author1: <asp:TextBox ID="txt_Cat_Author1" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="20px" MaxLength="250" 
                        ToolTip="Enter Author1" Width="200px" Wrap="False"></asp:TextBox> 
                    <ajaxToolkit:AutoCompleteExtender 
                        ID="txt_Cat_Author1_AutoCompleteExtender" 
                        runat="server" 
                        MinimumPrefixLength="1"
                        EnableCaching="true" 
                        CompletionSetCount="10"
                        ServiceMethod="SearchAuthor"
                        TargetControlID="txt_Cat_Author1" 
                        FirstRowSelected = "false">
                        
                    </ajaxToolkit:AutoCompleteExtender>
                </td>
                <td class="style55" colspan="5">
                    Author2: 
                    <asp:TextBox ID="txt_Cat_Author2" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="20px" MaxLength="250" 
                        ToolTip="Enter Author2" Width="200px" Wrap="False"></asp:TextBox>
                        <ajaxToolkit:AutoCompleteExtender 
                        ID="txt_Cat_Author2_AutoCompleteExtender" 
                        runat="server" 
                        MinimumPrefixLength="1"
                        EnableCaching="true" 
                        CompletionSetCount="10"
                        ServiceMethod="SearchAuthor"
                        TargetControlID="txt_Cat_Author2"                        
                        FirstRowSelected = "false">
                        
                    </ajaxToolkit:AutoCompleteExtender>
                </td>
                <td class="style55" colspan="3">
                    Author3: 
                    <asp:TextBox ID="txt_Cat_Author3" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="20px" MaxLength="250" 
                        ToolTip="Enter Author3" Width="200px" Wrap="False" 
                        style="text-align: left"></asp:TextBox>
                        <ajaxToolkit:AutoCompleteExtender 
                        ID="txt_Cat_Author3_AutoCompleteExtender" 
                        runat="server" 
                        MinimumPrefixLength="1"
                        EnableCaching="true" 
                        CompletionSetCount="10"
                        ServiceMethod="SearchAuthor"
                        TargetControlID="txt_Cat_Author3" 
                        FirstRowSelected = "false">
                        
                    </ajaxToolkit:AutoCompleteExtender>
                </td>
                
            </tr>
             <tr id="TR_Editor" runat="server">
                <td class="style53"> 
                    Editor(s) ;</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_Editor" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter Editor(s) separated by semicolon (; ) and space" 
                        Width="99%" Wrap="False"></asp:TextBox>
                 </td>
              
            </tr>
             <tr id="TR_Translator" runat="server">
                <td class="style53"> 
                    Translator(s) ;</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_Translator" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter Translator(s) separated by semicolon (; ) and space" 
                        Width="99%" Wrap="False"></asp:TextBox>
                 </td>
               
            </tr>
             <tr id="TR_Illustrator" runat="server">
                <td class="style53"> 
                    Illustrator(s) ;</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_Illustrator" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter Illustrator(s) separated by semicolon (; ) and space" 
                        Width="99%" Wrap="False"></asp:TextBox>
                 </td>
               
            </tr>
             <tr id="TR_Compiler" runat="server">
                <td class="style53"> 
                    Compiler(s) ;</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_Compiler" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter Compiler(s) separated by semicolon (; ) and space" 
                        Width="99%" Wrap="False"></asp:TextBox>
                 </td>
               
            </tr>
             <tr id="TR_Commentator" runat="server">
                <td class="style53"> 
                    Comementator(s) ;</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_Commentator" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter Commentator(s) separated by semicolon (; ) and space" 
                        Width="99%" Wrap="False"></asp:TextBox>
                 </td>
               
            </tr>
             <tr id="TR_RevisedBy" runat="server">
                <td class="style53"> 
                    Revised By ;</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_RevisedBy" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="255" 
                        ToolTip="Enter Revised By,  separated by semicolon (; ) and space" 
                        Width="99%" Wrap="False"></asp:TextBox>
                 </td>               
            </tr>

            <tr id="TR_Producer" runat="server">
                <td class="style53"> 
                    Produced By</td>
                <td class="style54" colspan="5">
                    <asp:TextBox ID="txt_Cat_Producer" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="22px" MaxLength="250" 
                        ToolTip="Enter Name of Producer" Width="250px" Wrap="False"></asp:TextBox>
                 </td>
                 <td class="style55" colspan="6">
                     Production Year
                     <asp:TextBox ID="txt_Cat_ProductionYear" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="4" 
                        ToolTip="Enter Year of Production in YYYY format" Width="20%" Wrap="False" 
                        Height="16px" onkeypress="return isNumberKey(event)"></asp:TextBox>
                    <ajaxToolkit:AutoCompleteExtender 
                        ID="AutoCompleteExtender1" 
                        runat="server" 
                        Enabled="True"
                        MinimumPrefixLength="1"
                        EnableCaching="true" 
                        CompletionSetCount="10"
                        ServiceMethod="SearchYear"
                        FirstRowSelected = "false"                       
                        TargetControlID="txt_Cat_Year">
                    </ajaxToolkit:AutoCompleteExtender>
                 </td>               
            </tr>

            <tr id="TR_Designer" runat="server">
                <td class="style53"> 
                    Designed By ;</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_Designer" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="150" 
                        ToolTip="Enter Name of Designer" 
                        Width="99%" Wrap="False"></asp:TextBox>
                 </td>               
            </tr>

            <tr id="TR_Manufacturer" runat="server">
                <td class="style53"> 
                    Manufactured By ;</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_Manufacturer" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="250" 
                        ToolTip="Enter Manufacturer Name" 
                        Width="99%" Wrap="False"></asp:TextBox>
                 </td>               
            </tr>

             <tr id="TR_Creator" runat="server">
                <td class="style53"> 
                    Created By</td>
                <td class="style54" colspan="5">
                    <asp:TextBox ID="txt_Cat_Creater" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="22px" MaxLength="150" 
                        ToolTip="Enter Name of Creator" Width="250px" Wrap="False"></asp:TextBox>
                 </td>
                 <td class="style55" colspan="6">
                     Role of Creator
                     <asp:TextBox ID="txt_Cat_RoleofCreator" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="22px" MaxLength="150" 
                        ToolTip="Enter Role of Creator" Width="150px" Wrap="False"></asp:TextBox>
                 </td>               
            </tr>

            <tr id="TR_Materials" runat="server">
                <td class="style53"> 
                    Materials Used</td>
                <td class="style54" colspan="5">
                    <asp:TextBox ID="txt_Cat_Materials" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="22px" MaxLength="150" 
                        ToolTip="Enter Name and Type of Materials made of " Width="350px" Wrap="False"></asp:TextBox>
                 </td>
                 <td class="style55" colspan="6">
                     Technique
                     <asp:TextBox ID="txt_Cat_Techniq" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="22px" MaxLength="150" 
                        ToolTip="Enter Technique Used" Width="350px" Wrap="False"></asp:TextBox>
                 </td>               
            </tr>

            <tr id="TR_Work" runat="server">
                <td class="style53"> 
                    Work Category</td>
                <td class="style54" colspan="5">
                    <asp:TextBox ID="txt_Cat_WrokCategory" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="22px" MaxLength="150" 
                        ToolTip="Enter about Work Category" Width="250px" Wrap="False"></asp:TextBox>
                 </td>
                 <td class="style55" colspan="6">
                     Work Type
                     <asp:TextBox ID="txt_Cat_WorkType" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="22px" MaxLength="150" 
                        ToolTip="Enter Type of Work" Width="250px" Wrap="False"></asp:TextBox>
                 </td>               
            </tr>

             <tr id="TR_RelatedWork" runat="server">
                <td class="style53"> 
                    Related Work</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_RelatedWork" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="250" 
                        ToolTip="Enter Related Work Details" Width="99%" Wrap="False"></asp:TextBox>
                 </td>                
            </tr>

             <tr id="TR_Source" runat="server">
                <td class="style53"> 
                    Source</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_Source" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="250" 
                        ToolTip="Enter Source Details" Width="99%" Wrap="False"></asp:TextBox>
                 </td>                
            </tr>

             <tr id="TR_Photographer" runat="server">
                <td class="style53"> 
                    Photographer</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_Photographer" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="250" 
                        ToolTip="Enter Photographer Details" Width="99%" Wrap="False"></asp:TextBox>
                 </td>                
            </tr>



             <tr id="TR_CorpAuthor" runat="server">
                <td class="style53"> 
                    Corp Author</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_CorpAuthor" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter Corporate Author" Width="99%" Wrap="False"></asp:TextBox>
                 </td>
                
            </tr>

             <tr id="TR_CHAIRMAN" runat="server">
                <td class="style53"> 
                    Chairman</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_Chairman" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="150" 
                        ToolTip="Enter Name of Chairman" 
                        Width="99%" Wrap="False"></asp:TextBox>
                 </td>               
            </tr>
             <tr id="TR_GOVERNMENT" runat="server">
                <td class="style53"> 
                    Government</td>
                <td class="style54" colspan="11">
                    <asp:DropDownList ID="DDL_Government" runat="server" 
                        Font-Bold="True" ForeColor="#0066CC" 
                        ToolTip="Plz Select Value from Drop-Down">
                        <asp:ListItem Selected="True"></asp:ListItem>
                        <asp:ListItem>State</asp:ListItem>
                        <asp:ListItem>Centre</asp:ListItem>
                        <asp:ListItem>Others</asp:ListItem>
                    </asp:DropDownList>
                 </td>               
            </tr>
             <tr id="TR_Edition" runat="server">
                <td class="style53"> 
                    Edition</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_Edition" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter Edition (Do not type Ed.) " Width="300px" Wrap="False"></asp:TextBox>
                    <ajaxToolkit:AutoCompleteExtender 
                        ID="txt_Cat_Edition_AutoCompleteExtender" 
                        runat="server" 
                        MinimumPrefixLength="1"
                        EnableCaching="true" 
                        CompletionSetCount="10"
                        ServiceMethod="SearchEdition"
                        FirstRowSelected = "false"     
                        TargetControlID="txt_Cat_Edition">
                    </ajaxToolkit:AutoCompleteExtender>
                    <asp:Label ID="Label10" runat="server" Font-Size="X-Small" 
                        Text="Ed. (e.g. : 2nd Revised; Do Not Put &quot;Ed.&quot; word)"></asp:Label>
                 </td>
                
            </tr>
             <tr id="TR_Reprint" runat="server">
                <td class="style53"> 
                    Reprints</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_Reprint" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter Reprints" Width="500px" Wrap="False"></asp:TextBox>
                 </td>
                
            </tr>
             <tr id="TR_Publisher" runat="server">
                <td class="style57"> 
                    Publisher *</td>
                <td class="style58" colspan="11">
                   
                    <ajaxToolkit:ComboBox ID="Pub_ComboBox" runat="server" 
                        AutoCompleteMode="SuggestAppend" AutoPostBack="True" Font-Bold="True" 
                        ForeColor="#0066FF" Width="500px">
                    </ajaxToolkit:ComboBox>                   
                 </td>
                
            </tr>
             <tr id="TR_Place" runat="server">
                <td class="style53"> 
                    Place *</td>
                <td class="style54" colspan="5">
                    <asp:TextBox ID="txt_Cat_Place" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="22px" MaxLength="150" 
                        ToolTip="Enter Place of Publication" Width="50%" Wrap="False"></asp:TextBox>
                    <ajaxToolkit:AutoCompleteExtender 
                        ID="txt_Cat_Place_AutoCompleteExtender" 
                        runat="server" 
                        MinimumPrefixLength="1"
                        EnableCaching="true" 
                        CompletionSetCount="10"
                        ServiceMethod="SearchPlace"
                        FirstRowSelected = "false"    
                        TargetControlID="txt_Cat_Place">
                    </ajaxToolkit:AutoCompleteExtender>
                 </td>
                 <td class="style55" colspan="6">
                     <asp:Label ID="Label8" runat="server" Text="Country*"></asp:Label>
                     <asp:DropDownList ID="DDL_Countries" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" ToolTip="Select Country from Drop-Down" Width="50%">
                     </asp:DropDownList>
                      <ajaxToolkit:ListSearchExtender 
                        ID="ListSearchExtender2" 
                        runat="server" 
                        Enabled="True" 
                        PromptCssClass="PromptCSS" 
                        TargetControlID="DDL_Countries">
                    </ajaxToolkit:ListSearchExtender>
                 </td>
               
            </tr>
             <tr id="TR_Year" runat="server">
                <td class="style53"> 
                    Year *</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_Year" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="4" 
                        ToolTip="Enter Year of Publication in YYYY format" Width="6%" Wrap="False" 
                        Height="16px" onkeypress="return isNumberKey(event)"></asp:TextBox>
                    <ajaxToolkit:AutoCompleteExtender 
                        ID="txt_Cat_Year_AutoCompleteExtender" 
                        runat="server" 
                        Enabled="True"
                        MinimumPrefixLength="1"
                        EnableCaching="true" 
                        CompletionSetCount="10"
                        ServiceMethod="SearchYear"
                        FirstRowSelected = "false"                       
                        TargetControlID="txt_Cat_Year">
                    </ajaxToolkit:AutoCompleteExtender>
                    <asp:Label ID="Label11" runat="server" Font-Size="Smaller" 
                        Text="YYYY (e.g. 2013)"></asp:Label>
                 </td>
                
            </tr>
             <tr id="TR_Series" runat="server">
                <td class="style53"> 
                    Series</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_Series" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter Series" Width="99%" Wrap="False"></asp:TextBox>
                    <ajaxToolkit:AutoCompleteExtender 
                        ID="txt_Cat_Series_AutoCompleteExtender" 
                        runat="server" 
                        Enabled="True"
                        MinimumPrefixLength="1"
                        EnableCaching="true" 
                        CompletionSetCount="10"
                        ServiceMethod="SearchSeries"
                        FirstRowSelected = "false"  
                        TargetControlID="txt_Cat_Series">
                    </ajaxToolkit:AutoCompleteExtender>
                 </td>
               
            </tr>
             <tr id="TR_SeriesEditor" runat="server">
                <td class="style53"> 
                    Series Editor(s) ;</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_SeriesEditor" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter series Editor(S); use semicolon (;) between two Editors" 
                        Width="99%" Wrap="False"></asp:TextBox>
                 </td>
                
            </tr>
             <tr id="TR_Note" runat="server">
                <td class="style53"> 
                    Note</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_Note" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter Note" Width="99%" Wrap="False" Height="106px" 
                        TextMode="MultiLine"></asp:TextBox>
                 </td>
                
            </tr>
             <tr id="TR_Remarks" runat="server">
                <td class="style53"> 
                    Remarks</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_Remarks" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter Remarks" Width="99%" Wrap="False" Height="120px" 
                        TextMode="MultiLine"></asp:TextBox>
                 </td>
               
            </tr>
             <tr id="TR_URL" runat="server">
                <td class="style53"> 
                    URL</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_URL" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter URL" Width="99%" Wrap="False"></asp:TextBox>
                 </td>
               
            </tr>
             <tr id="TR_Comments" runat="server">
                <td class="style53"> 
                    Comments</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_Comments" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter Comments" Width="99%" Wrap="False" Height="93px" 
                        TextMode="MultiLine"></asp:TextBox>
                 </td>
                
            </tr>
             <tr id="TR_Subject" runat="server">
                <td class="style53"> 
                    Main Subject</td>
                <td class="style54" colspan="11">
                    <asp:DropDownList ID="DDL_Subjects" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" ToolTip="Plz Select Value from Drop-Down" 
                        AutoPostBack="True">
                    </asp:DropDownList>
                     <ajaxToolkit:ListSearchExtender 
                        ID="ListSearchExtender1" 
                        runat="server" 
                        Enabled="True" 
                        PromptCssClass="PromptCSS" 
                        TargetControlID="DDL_Subjects">
                    </ajaxToolkit:ListSearchExtender>
                 </td>
               
            </tr>
             <tr id="TR_Keyword" runat="server">
                <td class="style53"> 
                    Keyword(s) ;</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_Keywords" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="1000" ToolTip="Enter Keyword(s) separated by semicolon (;) and space" 
                        Width="99%" Wrap="False"  style="text-transform: uppercase"></asp:TextBox>
                 </td>
                
            </tr>
             <tr id="TR_TranslatedFrom" runat="server">
                <td class="style53"> 
                    Translated From</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_TrFrom" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Translated From : Title" Width="99%" Wrap="False"></asp:TextBox>
                 </td>               
            </tr>
            <tr id="TR_Absract" runat="server">
                <td class="style53"> 
                    Abstract</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_Abstract" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter Abstract" Width="99%" Wrap="False"></asp:TextBox>
                 </td>               
            </tr>
            <tr id="TR_ReferenceNo" runat="server">
                <td class="style53"> Reference No</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_ReferenceNo" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter Reference No" Width="99%" Wrap="False"></asp:TextBox>
                 </td>               
            </tr>

            <tr id="TR_Nationality" runat="server">
                <td class="style53"> Nationality</td>
                <td class="style54" colspan="11">
                    <asp:TextBox ID="txt_Cat_Nationality" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="50" 
                        ToolTip="Enter Nationality Details" Width="40%" Wrap="False"></asp:TextBox>
                 </td>               
            </tr>

             <tr>
                <td class="style53">Select Cover Photo</td>
                <td class="style54" colspan="11">
                    <asp:FileUpload ID="FileUpload1" runat="server" ViewStateMode="Enabled"     />
                      
                    <asp:Image ID="Image1" runat="server" BorderStyle="Double"  ImageAlign="Middle" BorderColor="#0033CC" BorderWidth="4px"/>             
                    <asp:CheckBox ID="CheckBox1" runat="server" Text="Delete This Picture from Database? press UPDATE Button." Visible="False" />
                           
                </td>
            </tr>
            

            <tr>
                <td class="style56" colspan="12">
                    <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Size="Medium" 
                        ForeColor="White" Text="Serial Start History"></asp:Label>
                </td>
            </tr>

           
           <tr>
                <td class="style53"> 
                    <asp:Label ID="Label20" runat="server" Text="CODEN"></asp:Label>
                </td>
                <td class="style54">
                    <asp:TextBox ID="txt_Ser_CODEN" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="25" style="text-transform: uppercase" 
                        Width="80%" Wrap="False"></asp:TextBox>
                 </td>
                
                <td class="style53">
                    <asp:Label ID="Label21" runat="server" Text=" Start Volume"></asp:Label>
                </td>
                <td class="style54">
                    <asp:TextBox ID="txt_Ser_SVol" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="20" ToolTip="Enter Start Volume No" 
                        Width="50px" Wrap="False"></asp:TextBox>
                </td>
                <td class="style53" colspan="2">
                    <asp:Label ID="Label22" runat="server" Text=" Start Issue"></asp:Label>
                </td>
                <td class="style54">
                    <asp:TextBox ID="txt_Ser_SIssue" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="50" ToolTip="Enter Start Issue No" 
                        Width="50px" Wrap="False"></asp:TextBox>
                </td>
                <td class="style53" colspan="2">
                    <asp:Label ID="Label23" runat="server" Text=" Start Month"></asp:Label>
                </td>
                <td class="style54">
                    <asp:DropDownList ID="DDL_Months" runat="server" 
                        Font-Bold="True" ForeColor="#0066FF" 
                        ToolTip="Plz Select Value from Drop-Down">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem>January</asp:ListItem>
                        <asp:ListItem>February</asp:ListItem>
                        <asp:ListItem>March</asp:ListItem>
                        <asp:ListItem>April</asp:ListItem>
                        <asp:ListItem>May</asp:ListItem>
                        <asp:ListItem>June</asp:ListItem>
                        <asp:ListItem>July</asp:ListItem>
                        <asp:ListItem>August</asp:ListItem>
                        <asp:ListItem>September</asp:ListItem>
                        <asp:ListItem>October</asp:ListItem>
                        <asp:ListItem>November</asp:ListItem>
                        <asp:ListItem>December</asp:ListItem>
                    </asp:DropDownList>
                    
                </td>
                <td class="style53">
                    <asp:Label ID="Label24" runat="server" Text="Start Year"></asp:Label>
                </td>
                <td class="style54">
                    <asp:TextBox ID="txt_Ser_SYear" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="4" ToolTip="Enter Start Year" 
                        Width="50px" Wrap="False" onkeypress="return isNumberKey(event)" ></asp:TextBox>
                </td>
                
            </tr>
            <tr>
                <td class="style53"> 
                    <asp:Label ID="Label5" runat="server" Text="Start Frequency"></asp:Label>
                </td>
                <td class="style54">
                    <asp:DropDownList ID="DDL_FREQ" runat="server" 
                        Font-Bold="True" ForeColor="#0066FF" 
                        ToolTip="Plz Select Value from Drop-Down">
                    </asp:DropDownList>
                   
                 </td>
                
                <td class="style53">
                    <asp:Label ID="Label9" runat="server" Text=" Close Volume"></asp:Label>
                </td>
                <td class="style54">
                    <asp:TextBox ID="txt_Ser_CloseVol" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="20" ToolTip="Enter Close Volume No" 
                        Width="50px" Wrap="False"></asp:TextBox>
                </td>
                <td class="style53" colspan="2">
                    <asp:Label ID="Label13" runat="server" Text=" Close Issue"></asp:Label>
                </td>
                <td class="style54">
                    <asp:TextBox ID="txt_Ser_CloseIssue" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="50" ToolTip="Enter Close Issue No" 
                        Width="50px" Wrap="False"></asp:TextBox>
                </td>
                <td class="style53" colspan="2">
                    <asp:Label ID="Label16" runat="server" Text=" Close Month"></asp:Label>
                </td>
                <td class="style54">
                    <asp:DropDownList ID="DDL_CloseMonths" runat="server" 
                        Font-Bold="True" ForeColor="#0066FF" 
                        ToolTip="Plz Select Value from Drop-Down">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem>January</asp:ListItem>
                        <asp:ListItem>February</asp:ListItem>
                        <asp:ListItem>March</asp:ListItem>
                        <asp:ListItem>April</asp:ListItem>
                        <asp:ListItem>May</asp:ListItem>
                        <asp:ListItem>June</asp:ListItem>
                        <asp:ListItem>July</asp:ListItem>
                        <asp:ListItem>August</asp:ListItem>
                        <asp:ListItem>September</asp:ListItem>
                        <asp:ListItem>October</asp:ListItem>
                        <asp:ListItem>November</asp:ListItem>
                        <asp:ListItem>December</asp:ListItem>
                    </asp:DropDownList>
                    
                </td>
                <td class="style53">
                    <asp:Label ID="Label17" runat="server" Text="Close Year"></asp:Label>
                </td>
                <td class="style54">
                    <asp:TextBox ID="txt_Ser_CloseYear" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="4" ToolTip="Enter Close Year" 
                        Width="50px" Wrap="False" onkeypress="return isNumberKey(event)" ></asp:TextBox>
                </td>
                
            </tr>

             <tr>
                <td class="style53">
                    <asp:Label ID="Label25" runat="server" Text="Remarks"></asp:Label>
                 </td>
                <td class="style54" colspan="4">
                   
                    <asp:TextBox ID="txt_Ser_Remarks" runat="server" 
                        AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                        MaxLength="500" ToolTip="Enter Remarks, if any" Width="80%" Wrap="False"></asp:TextBox>
                   
                 </td>               
                 <td class="style54" colspan="3">
                     <asp:Label ID="Label26" runat="server" style="font-weight: 700" 
                         Text="Full Text?"></asp:Label>
                 </td>
                 <td class="style54" colspan="4">
                     <asp:DropDownList ID="DDL_FullText" runat="server" AutoPostBack="True" 
                         Font-Bold="True" ForeColor="#0066FF" ToolTip="Plz Select Value from Drop-Down">
                         <asp:ListItem Value="Y">Yes</asp:ListItem>
                         <asp:ListItem Selected="True" Value="N">No</asp:ListItem>
                     </asp:DropDownList>
                 </td>
            </tr>
           
           
           <tr>
                <td class="style56" colspan="12">
                    <asp:Label ID="Label18" runat="server" Font-Bold="True" Font-Size="Medium" 
                        ForeColor="White" Text="Serial Subscription History"></asp:Label>
                </td>
            </tr>

           
           <tr>
                <td class="style53"> 
                    <asp:Label ID="Label19" runat="server" Text="Subscribed? *" 
                        style="color: #CC3300;"></asp:Label>
                </td>
                <td class="style54">
                    <asp:DropDownList ID="DDL_Subscribed" runat="server" AutoPostBack="True" 
                        Font-Bold="True" ForeColor="#0066FF" 
                        ToolTip="Plz Select Value from Drop-Down">
                        <asp:ListItem Selected="True" Value="Y">Yes</asp:ListItem>
                        <asp:ListItem Value="N">No</asp:ListItem>
                    </asp:DropDownList>
                 </td>
                
                <td class="style53">
                    <asp:Label ID="Label27" runat="server" Text="Start Volume"></asp:Label>
                </td>
                <td class="style54">
                    <asp:TextBox ID="txt_Ser_SubsStartVol" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="20" ToolTip="Enter Subscription Start Volume No" 
                        Width="50px" Wrap="False"></asp:TextBox>
                </td>
                <td class="style53" colspan="2">
                    <asp:Label ID="Label28" runat="server" Text="Start Issue"></asp:Label>
                </td>
                <td class="style54">
                    <asp:TextBox ID="txt_Ser_SubsStartIssue" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="50" ToolTip="Enter subscription Start Issue No" 
                        Width="50px" Wrap="False"></asp:TextBox>
                </td>
                <td class="style53" colspan="2">
                    <asp:Label ID="Label29" runat="server" Text="Start Month"></asp:Label>
                </td>
                <td class="style54">
                    <asp:DropDownList ID="DDL_SUBSMonths" runat="server" 
                        Font-Bold="True" ForeColor="#0066FF" 
                        ToolTip="Plz Select Value from Drop-Down">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem>January</asp:ListItem>
                        <asp:ListItem>February</asp:ListItem>
                        <asp:ListItem>March</asp:ListItem>
                        <asp:ListItem>April</asp:ListItem>
                        <asp:ListItem>May</asp:ListItem>
                        <asp:ListItem>June</asp:ListItem>
                        <asp:ListItem>July</asp:ListItem>
                        <asp:ListItem>August</asp:ListItem>
                        <asp:ListItem>September</asp:ListItem>
                        <asp:ListItem>October</asp:ListItem>
                        <asp:ListItem>November</asp:ListItem>
                        <asp:ListItem>December</asp:ListItem>
                    </asp:DropDownList>
                    
                </td>
                <td class="style53">
                    <asp:Label ID="Label30" runat="server" Text="Start Year"></asp:Label>
                </td>
                <td class="style54">
                    <asp:TextBox ID="txt_Ser_SubsStartYear" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="4" ToolTip="Enter Subscription Start Year" 
                        Width="50px" Wrap="False" onkeypress="return isNumberKey(event)" ></asp:TextBox>
                </td>
                
            </tr>
            <tr>
                <td class="style53"> 
                    <asp:Label ID="Label37" runat="server" style="font-size: x-small" Text=""></asp:Label>
                </td>
                <td class="style54">
                    <asp:Label ID="Label36" runat="server" Text=""></asp:Label>
                </td>
                
                <td class="style53">
                    <asp:Label ID="Label32" runat="server" Text="Close Volume"></asp:Label>
                </td>
                <td class="style54">
                    <asp:TextBox ID="txt_Ser_SubsCloseVol" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="20" ToolTip="Enter Subscription Close Volume No" 
                        Width="50px" Wrap="False"></asp:TextBox>
                </td>
                <td class="style53" colspan="2">
                    <asp:Label ID="Label33" runat="server" Text="Close Issue"></asp:Label>
                </td>
                <td class="style54">
                    <asp:TextBox ID="txt_Ser_SubsCloseIssue" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="50" ToolTip="Enter Subscription Close Issue No" 
                        Width="50px" Wrap="False"></asp:TextBox>
                </td>
                <td class="style53" colspan="2">
                    <asp:Label ID="Label34" runat="server" Text="Close Month"></asp:Label>
                </td>
                <td class="style54">
                    <asp:DropDownList ID="DDL_SubsCloseMonths" runat="server" 
                        Font-Bold="True" ForeColor="#0066FF" 
                        ToolTip="Plz Select Value from Drop-Down">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem>January</asp:ListItem>
                        <asp:ListItem>February</asp:ListItem>
                        <asp:ListItem>March</asp:ListItem>
                        <asp:ListItem>April</asp:ListItem>
                        <asp:ListItem>May</asp:ListItem>
                        <asp:ListItem>June</asp:ListItem>
                        <asp:ListItem>July</asp:ListItem>
                        <asp:ListItem>August</asp:ListItem>
                        <asp:ListItem>September</asp:ListItem>
                        <asp:ListItem>October</asp:ListItem>
                        <asp:ListItem>November</asp:ListItem>
                        <asp:ListItem>December</asp:ListItem>
                    </asp:DropDownList>
                    
                </td>
                <td class="style53">
                    <asp:Label ID="Label35" runat="server" Text="Close Year"></asp:Label>
                </td>
                <td class="style54">
                    <asp:TextBox ID="txt_Ser_SubsCloseYear" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="4" ToolTip="Enter Close Year" 
                        Width="50px" Wrap="False" onkeypress="return isNumberKey(event)" ></asp:TextBox>
                </td>
                
            </tr>

            
           



            <tr>
                <td class="style47" colspan="12">
                    &nbsp; * Mandatory. NOTE: Put semicolon (; ) between two items in the same text box</td>
            </tr>
             <tr>
                <td class="style56" colspan="12">
                    <asp:Button ID="Acq_Save_Bttn" runat="server" AccessKey="s" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" Height="23px" 
                        OnClientClick="return valid1();" TabIndex="14" Text="Save" 
                        Width="74px" Visible="False" />
                    <asp:Button ID="Acq_Update_Bttn" runat="server" AccessKey="u" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" Height="23px" 
                        OnClientClick="return valid1();" TabIndex="14" Text="Update" 
                        Width="74px" Visible="False" />
                    <asp:Button ID="Acq_Delete_Bttn" runat="server" AccessKey="d" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" Height="23px" 
                        TabIndex="14" Text="Delete" 
                        Width="74px" Visible="False" />
                    <asp:Button ID="Acq_Cancel_Bttn" runat="server" AccessKey="c" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" Height="23px" 
                        TabIndex="14" Text="Cancel" 
                        Width="74px" />
                 </td>
            </tr>
            
            

             

        </table>

            
   
                   </ContentTemplate>
                   <Triggers>
                        <asp:PostBackTrigger  ControlID="Acq_Update_Bttn"   />    
                        <asp:PostBackTrigger ControlID ="Acq_Save_Bttn"   /> 
                        <asp:PostBackTrigger ControlID ="Acq_Delete_Bttn"   />  
                        <asp:PostBackTrigger ControlID ="Acq_Cancel_Bttn"   />                           
                   </Triggers>
                    </asp:UpdatePanel>
       
        
</asp:Content>
