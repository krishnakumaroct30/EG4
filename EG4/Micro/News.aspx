<%@ Page  Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="News.aspx.vb" Inherits="EG4.News"%>


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
           ids = ["MainContent_txt_News_Title", "MainContent_txt_News_SubTitle", "MainContent_txt_News_Authors", "MainContent_txt_News_Abstract", "MainContent_txt_News_Body", "MainContent_txt_News_Keywords", "MainContent_txt_Search"];
           control.makeTransliteratable(ids);
           // Show the transliteration control which can be used to toggle between         
           // English and Hindi and also choose other destination language.         
           control.showControl('translControl');
       }

       google.setOnLoadCallback(onLoad);
           
    </script>
    
    <script type="text/javascript">
        function formfocus() {
            document.getElementById('<%= DDL_Titles.ClientID %>').focus();
        }
        window.onload = formfocus;
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
             
              if (document.getElementById('<%=DDL_Titles.ClientID%>').value == "") {
                  alert("Please Select \"Title\" From Drop-Down.");
                  document.getElementById("MainContent_DDL_Titles").focus();
                  return (false);
              }
              if (document.getElementById('<%=txt_News_Title.ClientID%>').value.length < 3) {
                  alert("Length of \"Title\" should be Min 3 characters.");
                  document.getElementById("MainContent_txt_News_Title").focus();
                  return (false);
              }

              return (true);
          }

    </script>

    <script language="javascript" type="text/javascript">

        function valid2() {

            if (document.getElementById('<%=DDL_Titles.ClientID%>').value == "") {
                alert("Please Select \"Title\" From Drop-Down.");
                document.getElementById("MainContent_DDL_Titles").focus();
                return (false);
            }
            if (document.getElementById('<%=txt_News_Title.ClientID%>').value.length < 3) {
                alert("Length of \"Title\" should be Min 3 characters.");
                document.getElementById("MainContent_txt_News_Title").focus();
                return (false);
            }

            if (document.getElementById('<%=Label26.ClientID%>').value == "") {
                alert("Please Select \"News Item\" From Grid.");
                document.getElementById("MainContent_Label26").focus();
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

    <script language ="javascript" type ="text/javascript" >
        function Select1(Select1) {

            var grdv = document.getElementById('<%= Grid1_Search.ClientID %>');
            var chbk = "cbd";

            var Inputs = grdv.getElementsByTagName("input");

            for (var n = 0; n < Inputs.length; ++n) {
                if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(chbk, 0) >= 0) {
                    Inputs[n].checked = Select1;
                }
            }
            return false;
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
                document.getElementById("MainContent_txt_News_NewsDate").focus();
                return (false);
            }
        }
    </script>

    <div>
    </div>
         <table id="ADMT1" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
            <tr>
                <td  bgcolor="#003366" class="style43" colspan="2" rowspan="1" style="color: #FFFFFF"><strong>News Clipping Manager 
                    </strong></td>
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
                        SelectedIndex="1"   Width="100%" Height="237px"   >  
                            <Panes>  
                                <ajaxToolkit:AccordionPane ID="Search_Pane" runat="server" >  
                                    <Header>Click To View / Hide Search Pane</Header>  
                                        <Content>               
                
                        
                    Search Text&nbsp;
                    <asp:TextBox ID="txt_Search" runat="server" MaxLength="100"  
                        ToolTip="Enter Search Term" Wrap="False" AccessKey="r" 
                        Font-Bold="True" ForeColor="#0066FF" Width="250px"></asp:TextBox>
                    &nbsp;In: 
                    <asp:DropDownList ID="DropDownList1" runat="server" ForeColor="#0066FF">
                        <asp:ListItem Selected="True" Value="TITLE">Source</asp:ListItem>
                        <asp:ListItem Value="ART_TITLE">News Heading</asp:ListItem>
                        <asp:ListItem Value="AUTHORS">Report/Correnspondent</asp:ListItem>
                        <asp:ListItem Value="ABSTRACT">Summary</asp:ListItem>
                        <asp:ListItem Value="KEYWORDS">Keywords</asp:ListItem>
                        <asp:ListItem Value="ART_YEAR">Year</asp:ListItem> 
                        <asp:ListItem Value="NEWS_DATE">News Date</asp:ListItem>                          
                    </asp:DropDownList>&nbsp;Operator: 
                    <asp:DropDownList ID="DropDownList2" runat="server" ForeColor="#0066FF">
                        <asp:ListItem Value="AND" Selected="True">All Words</asp:ListItem>
                        <asp:ListItem Value="OR">Any Word</asp:ListItem>
                        <asp:ListItem Value="LIKE">Like</asp:ListItem>
                        <asp:ListItem Value="SW">Start With</asp:ListItem>
                        <asp:ListItem Value="EW">End With</asp:ListItem>
                    </asp:DropDownList> &nbsp;Order By: 
                    <asp:DropDownList ID="DropDownList3" runat="server" ForeColor="#0066FF">
                        <asp:ListItem Selected="True" Value="TITLE">Source</asp:ListItem>
                        <asp:ListItem Value="ART_TITLE">Title</asp:ListItem>
                        <asp:ListItem  Value="ART_YEAR">Year</asp:ListItem>
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
                        ForeColor="Red" TabIndex="14" Text="Delete Selected Row(s)" AccessKey="d"    
                            Width="165px" Height="20px" Enabled="false" />

                  
                                           
                        <asp:Panel ID="Panel2" runat="server" Height ="250px" ScrollBars="Auto">
                       
                   <asp:GridView ID="Grid1_Search" runat="server" AllowPaging="True" DataKeyNames="ART_NO"    
                        style="width: 100%;  text-align: center;" allowsorting="True" 
                        AutoGenerateColumns="False" PageSize="10"  Font-Bold="True" 
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
                    
                   

                    <asp:BoundField   DataField="TITLE" HeaderText="Source" SortExpression="TITLE">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" Width="350px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="350px" Font-Names="Tahoma"/>                        
                    </asp:BoundField>
                                      
                    <asp:BoundField   DataField="ART_TITLE" SortExpression="ART_TITLE" HeaderText="News Heading" visible="true">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="320px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="350px" Font-Names="Arial"/>                        
                    </asp:BoundField>
                     
                      <asp:BoundField   DataField="AUTHORS" SortExpression="AUTHORS" HeaderText="Reporter" >                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="300px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="300px" Font-Names="Arial"/>                        
                    </asp:BoundField>

                     <asp:BoundField   DataField="VOL" SortExpression="VOL" HeaderText="Vol No" >                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="100px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="120px" Font-Names="Arial"/>                        
                    </asp:BoundField>

                    <asp:BoundField   DataField="ISSUE" HeaderText="Issue No" SortExpression="ISSUE">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" Width="100px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="120px" Font-Names="Tahoma"/>                        
                    </asp:BoundField>
                                      
                   
                      <asp:BoundField   DataField="PERIOD" HeaderText="Month">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" Width="100px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="120px" Font-Names="Tahoma"/>                        
                    </asp:BoundField>

                    <asp:BoundField   DataField="ART_YEAR" HeaderText="Year" SortExpression="ART_YEAR">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" Width="100px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="120px" Font-Names="Tahoma"/>                        
                    </asp:BoundField>

                    <asp:BoundField   DataField="NEWS_DATE" HeaderText="Date" SortExpression="NEWS_DATE" DataFormatString="{0:dd/MM/yyyy}">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" Width="100px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="120px" Font-Names="Tahoma"/>                        
                    </asp:BoundField>

                    <asp:TemplateField  ControlStyle-Width="10px"  HeaderText="Delete" FooterText="Select to Delete" ShowHeader="true">
                        <ItemTemplate>
                            <asp:CheckBox ID="cbd"  runat="server" />
                        </ItemTemplate>

                        <HeaderTemplate>
                            <asp:ImageButton ID="ImageButton1" runat="server" Height="16px" Width="16px" ToolTip="Select All" ImageUrl="~/Images/check_all.gif" onClientclick="return Select1(true)"  />
                            <asp:ImageButton ID="ImageButton2" runat="server" Height="16px" Width="16px" ToolTip="Deselect All" ImageUrl="~/Images/uncheck_all.gif"  OnClientClick ="return Select1(false)" />
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


         <table id="Table2" runat="server" cellspacing="2" border="1"  cellpadding="2" class="style35">
            <tr>
                <td class="style56" colspan="5" bgcolor="#336699">
                   <asp:Label ID="Label13" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True" style="font-size: small">STEP 1: Select Newspaper from Drop-Down!</asp:Label>
                </td>
            </tr>
            
             <tr style=" color:Green; font-weight:bold">
                <td class="style51"> 
                    Select Title</td>
                <td class="style54" colspan="4">
                    <asp:DropDownList ID="DDL_Titles" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC" Width="78%" AutoPostBack="True">
                    </asp:DropDownList>
          
                    
                    <ajaxToolkit:ListSearchExtender ID="DDL_Titles_ListSearchExtender" 
                        runat="server" Enabled="True" TargetControlID="DDL_Titles" PromptText="Type to search" PromptCssClass="PromptCSS">
                    </ajaxToolkit:ListSearchExtender>
          
                    
                    <span>
                    <asp:Label ID="Label24" runat="server" Text="Record(s): "></asp:Label>
                    </span>
          
                    
                   </td>                
            </tr>
           
        
            <tr>
                <td class="style56" colspan="5" bgcolor="#336699">
                   <asp:Label ID="Label4" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">Display Document Record</asp:Label>
                </td>
            </tr>
            
            
             <tr style=" color:Green; font-weight:bold">
                <td class="style51"> 
                    Cat Number</td>
                 <td class="style54" colspan="3">
                     <asp:Label ID="Label19" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                 </td>                
                 <td align="right" class="style54" rowspan="4" valign="middle">
                     <asp:Image ID="Image4" runat="server" Height="50px" Width="36px" />
                 </td>
            </tr>
            <tr style=" color:Green; font-weight:bold">
                <td class="style51">
                    Title Details</td>
                <td class="style54" colspan="3">
                    <asp:Label ID="Label16" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                </td>
            </tr>
            <tr style=" color:Green; font-weight:bold">
                <td class="style51"> 
                    Editors(s)</td>
                <td class="style54" colspan="3">
                    <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                </td>  
                               
            </tr>   
            <tr style=" color:Green; font-weight:bold">
                <td class="style51"> 
                    Imprint(s)</td>
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
                    Vol No</td>
                <td class="style54">
                    <asp:TextBox ID="txt_News_VolNo" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="18px" MaxLength="50" 
                        ToolTip="Enter Vol No" Width="55px" Wrap="False"></asp:TextBox>
                   
                    &nbsp;Issue No:
                    <asp:TextBox ID="txt_News_IssueNo" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="18px" MaxLength="50" 
                        ToolTip="Enter Issue No" Width="55px" Wrap="False"></asp:TextBox>

                        &nbsp;Month:
                    <asp:TextBox ID="txt_News_Period" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="18px" MaxLength="50" 
                        ToolTip="Enter Month/Period" Width="75px" Wrap="False"></asp:TextBox>
                    
                    &nbsp;Year:
                    <asp:TextBox ID="txt_News_Year" runat="server" 
                        AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                        Height="18px" MaxLength="4" onkeypress="return isNumberKey(event)" 
                        ToolTip="Enter Year in YYYY Format" Width="40px" Wrap="False"></asp:TextBox>
                    
                    &nbsp;&nbsp;Show?
                    <asp:DropDownList ID="DDL_Show" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC" TabIndex="16" ToolTip="Select Type">
                        <asp:ListItem Selected="True" Value="Y">Y</asp:ListItem>
                        <asp:ListItem Value="N">N</asp:ListItem>
                    </asp:DropDownList>
                    
                    &nbsp;News Date*:
                    <asp:TextBox ID="txt_News_NewsDate" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="10" 
                        onkeypress="return DateOnly (event)" ToolTip="Click to Select Date" 
                        Width="71px"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="txt_News_NewsDate_CalendarExtender" 
                        runat="server" Enabled="True" Format="dd/MM/yyyy" 
                        TargetControlID="txt_News_NewsDate">
                    </ajaxToolkit:CalendarExtender>
                    
                    
                    &nbsp;Language:
                    <asp:DropDownList ID="DDL_Lang" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC" ToolTip="Plz Select Value from Drop-Down">
                    </asp:DropDownList>
                    
                    
                    <asp:Label ID="Label26" runat="server"></asp:Label>
                    
                </td>
               
            </tr>
            <tr>
                <td class="style51"> 
                    Heading*</td>
                <td class="style54">
                    <asp:TextBox ID="txt_News_Title" runat="server" MaxLength="350"  
                        ToolTip="Enter Title of the Articles/Special Issue of Journal" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Height="18px" Width="98%"></asp:TextBox>
                    &nbsp;</td>
               
            </tr>
             <tr>
                <td class="style51"> 
                    Sub Heading</td>
                <td class="style54">
                    <asp:TextBox ID="txt_News_SubTitle" runat="server" MaxLength="350"  
                        ToolTip="Enter Sub Title of Micro-Documents, If any" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Height="18px" Width="98%"></asp:TextBox>
                    &nbsp;</td>
               
            </tr>
             <tr>
                <td class="style51"> 
                    Correspondent/Reporter</td>
                <td class="style54">
                    <asp:TextBox ID="txt_News_Authors" runat="server" MaxLength="250"  
                        ToolTip="Enter Authors Name; Separated by ; " Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Height="18px" Width="98%"></asp:TextBox>
                    &nbsp;</td>
               
            </tr>
             <tr>
                <td class="style51"> 
                    Summary</td>
                <td class="style54">
                    <asp:TextBox ID="txt_News_Abstract" runat="server" 
                        AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                        Height="96px" MaxLength="4000" TextMode="MultiLine" ToolTip="Enter Remarks" 
                        Width="98%" Wrap="False"></asp:TextBox>
                 </td>
               
            </tr>

            <tr>
             <td class="style51"> 
                    Main Subject</td>
                <td class="style54" colspan="5">
                    <asp:DropDownList ID="DDL_Subjects" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" ToolTip="Plz Select Value from Drop-Down">
                    </asp:DropDownList>
                     <ajaxToolkit:ListSearchExtender 
                        ID="DDL_Subjects_ListSearchExtender1" 
                        runat="server" 
                        Enabled="True" 
                        PromptCssClass="PromptCSS" 
                        TargetControlID="DDL_Subjects">
                    </ajaxToolkit:ListSearchExtender>
                    &nbsp;Pagination:
                    <asp:TextBox ID="txt_News_Page" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="18px" MaxLength="50" 
                        ToolTip="Enter Pages" Width="75px" Wrap="False"></asp:TextBox>
                 </td>
               
            </tr>
             <tr>
                <td class="style51"> 
                    Keyword(s) ;</td>
                <td class="style54" colspan="5">
                    <asp:TextBox ID="txt_News_Keywords" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="350" ToolTip="Enter Keyword(s) separated by semicolon (;) and space" 
                        Width="98%" Wrap="False"  style="text-transform: uppercase"></asp:TextBox>
                 </td>
                
            </tr>
             <tr>
                <td class="style51"> 
                    News URL</td>
                <td class="style54" colspan="5">
                    <asp:TextBox ID="txt_News_URL" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="350" ToolTip="Enter URL of resource" 
                        Width="98%" Wrap="False"></asp:TextBox>
                 </td>
                
            </tr>
            <tr>
                <td class="style51">News Body</td>
                <td class="style54">
                    <asp:TextBox ID="txt_News_Body" runat="server" MaxLength="250"  
                        ToolTip="Enter Remarks" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="98%"  
                        AutoCompleteType="DisplayName" Height="176px" TextMode="MultiLine"></asp:TextBox>
                    </td>
            </tr>
            
        </table>

         
           
        

           <table id="Table3" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
             <tr>
                <td class="style51">Upload Photo</td>
                <td class="style54" colspan="5">
                    <asp:FileUpload ID="FileUpload2" runat="server" ViewStateMode="Enabled"     />
                      
                    <asp:Image ID="Image1" runat="server" BorderStyle="Double"  ImageAlign="Middle" BorderColor="#0033CC" BorderWidth="4px"/>             
                    <asp:CheckBox ID="CheckBox1" runat="server" Text="Delete This Picture from Database? press UPDATE Button." Visible="False" />
                           
                </td>
            </tr>

              <tr>
                <td class="style51">Upload News File</td>
                <td class="style54" colspan="5">
                    <asp:FileUpload ID="FileUpload1" runat="server" ViewStateMode="Enabled"      />
                </td>
              </tr>
           </table>
        
            <table cellspacing="2" border="1"  cellpadding="1" class="style35">
              <tr>
                <td class="style51">View File</td>
                <td class="style54" colspan="5">
                   <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <asp:HyperLink ID="HyperLink1" runat="server" Text="View File">View File</asp:HyperLink>
                                <asp:CheckBox ID="CheckBox3" runat="server" Text="Delete File? Press UPDATE Button." Visible="False" />
                                <asp:Label ID="Label27" runat="server"></asp:Label>
                            </ContentTemplate>
                  </asp:UpdatePanel>
                 </td>
                
            </tr>

             <tr>
                <td  bgcolor="#99CCFF" class="style44" colspan="2" rowspan="0">
                     <strong>*<span class="style48"> Mandatory Fields</span></strong></td>
            </tr>

            </table>

        <table id="Table4" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
            <tr>
                <td class="style56" colspan="2">
             
                    <asp:Button ID="News_Save_Bttn" runat="server" AccessKey="s" CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red" TabIndex="14" OnClientClick="return valid1();"
                        Text="Save" ToolTip="Press to SAVE Record" Visible="False" Width="74px" />
                    <asp:Button ID="News_Update_Bttn" runat="server"  CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red" TabIndex="14" Text="Update" AccessKey="u" 
                        Width="74px" ToolTip="Press to UPDATE Record"  OnClientClick="return valid2(); "/>
                    <asp:Button ID="News_Cancel_Bttn" runat="server"  CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red" 
                        TabIndex="15" Text="Cancel" Width="71px" AccessKey="c" 
                        ToolTip="Press to Cancel the process"/>
                    <asp:Button ID="News_Delete_Bttn" runat="server" AccessKey="d" 
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
                    <asp:Button ID="News_DeleteAll_Bttn" runat="server"  
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" TabIndex="14" 
                        Text="Delete Selected Records" ToolTip="Press to DELETE Records" 
                        Visible="False" Width="170px" Height="21px" />
                </td>
            </tr>

             <tr>
                <td class="style56" colspan="9" bgcolor="#336699">
                
                
               <asp:Panel ID="Panel1" runat="server" Height ="250px" ScrollBars="Auto">
                       
                   <asp:GridView ID="Grid1" runat="server" AllowPaging="True" DataKeyNames="ART_NO"    
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
                    
                   <asp:BoundField   DataField="TITLE" SortExpression="TITLE" HeaderText="Heading" >                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="450px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="450px" Font-Names="Arial"/>                        
                    </asp:BoundField>

                    <asp:BoundField   DataField="AUTHORS" SortExpression="AUTHORS" HeaderText="Reporter" >                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="200px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="200px" Font-Names="Arial"/>                        
                    </asp:BoundField>

                     <asp:BoundField   DataField="VOL" SortExpression="VOL" HeaderText="Vol No" >                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="70px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="70px" Font-Names="Arial"/>                        
                    </asp:BoundField>

                    <asp:BoundField   DataField="ISSUE" HeaderText="Issue No" SortExpression="ISSUE">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" Width="100px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="120px" Font-Names="Tahoma"/>                        
                    </asp:BoundField>
                                      
                   
                      <asp:BoundField   DataField="PERIOD" HeaderText="Month">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" Width="70px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="70px" Font-Names="Tahoma"/>                        
                    </asp:BoundField>

                    <asp:BoundField   DataField="ART_YEAR" HeaderText="Year" SortExpression="ART_YEAR">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" Width="70px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="70px" Font-Names="Tahoma"/>                        
                    </asp:BoundField>

                    <asp:BoundField   DataField="NEWS_DATE" HeaderText="Date" SortExpression="NEWS_DATE" DataFormatString="{0:dd/MM/yyyy}">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" Width="100px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="120px" Font-Names="Tahoma"/>                        
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
                        <asp:PostBackTrigger  ControlID="News_Save_Bttn"   /> 
                        <asp:PostBackTrigger  ControlID="News_Update_Bttn"   /> 
                        <asp:PostBackTrigger  ControlID="News_Cancel_Bttn"   />                             
                   </Triggers>
                    </asp:UpdatePanel>
        
</asp:Content>
