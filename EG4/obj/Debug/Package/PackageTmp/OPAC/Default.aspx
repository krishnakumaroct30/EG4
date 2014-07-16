<%@ Page Title="e-Granthalaya: A Digital Agenda for Library Automation and Networking" Language="vb" MasterPageFile="~/OPAC/OPAC.Master" AutoEventWireup="false"
    CodeBehind="~/OPAC/Default.aspx.vb" Inherits="EG4.OPACDefault" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
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
                
        
         .style34
        {
            border-style: none;
            border-color: inherit;            
            width: 74%;
            height:auto;
           
        }
        .style35
        {
            border-style: none;
            border-color: inherit;            
            width: 25%;
            margin-left: 0px;
            margin-right:0px;
            height:auto;
        }
        
    
        .style43
    {
        text-align: center;
        font-size: small;
        color: #336699;
        height: 15px;
        width: 100%;
    }
                
        .style44
    {
        text-align: center;
        font-size: small;
        color: #336699;
        height: 17px;
        width: 20%;
    }
        
                
        .style49
        {
            width: 99%;
            font-size: small;
            text-align: justify;
            height:auto;
        }
        
         .style63
        {
            width: 602px;
        }
        
                         
        #tech
        {
            height: 199px;
        }
                 
                
        .style50
        {
            text-decoration: underline;
        }
       
        .styleTitle
        {
            text-decoration:  none;
        }          
                
       
                
                
        
         .accordion {  
            width: 200px;  
        }  
          
        .accordionHeader {  
            border: 1px solid #2F4F4F;  
            color: White;  
            background-color: #2E4d7B ;  
            
            font-family: Arial, Sans-Serif;  
            font-size: 12px;  
            font-weight: bold;  
            padding: 5px;  
            margin-top: 5px;  
            cursor:  pointer;  
        }  
          
        .accordionHeaderSelected {  
            border: 1px solid #2F4F4F;  
            color: White;  
            background-color: #5078B3;  
            font-family: Arial, Sans-Serif;  
            font-size: 12px;  
            font-weight: bold;  
            padding: 5px;  
            margin-top: 5px;  
            cursor: pointer;  
        }  
          
        .accordionContent {  
            background-color: #D3DEEF;  
            border: 1px dashed #2F4F4F;  
            border-top: none;  
            padding: 5px;  
            padding-top: 10px;  
        }  
        
        #tooltip {
            display: none;
            border: solid 1px   #708069;
            background-color: #289642;
            color: #fff;
            line-height: 10px;
            padding: 5px 10px;
            position: absolute;
            z-index:3001;
            width:auto;
            height:auto;
            
            
        }
        #tooltip h3, #tooltip div  {
             margin: 0; 
        }
        
        
        
                
        </style>
    </asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">  

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
            ids = ["MainContent_TextBox1"];
            control.makeTransliteratable(ids);
            // Show the transliteration control which can be used to toggle between         
            // English and Hindi and also choose other destination language.         
            control.showControl('translControl');
        }
        google.setOnLoadCallback(onLoad);                  
     </script> 

    
    
    <script language="JavaScript" type="text/javascript">
           
            var scrollspeed		= "1"		// SET SCROLLER SPEED 1 = SLOWEST
            var speedjump		= "30"		// ADJUST SCROLL JUMPING = RANGE 20 TO 40
            var startdelay 		= "2" 		// START SCROLLING DELAY IN SECONDS
            var nextdelay		= "0" 		// SECOND SCROLL DELAY IN SECONDS 0 = QUICKEST
            var topspace		= "2px"		// TOP SPACING FIRST TIME SCROLLING
            var frameheight		= "200px"	// IF YOU RESIZE THE WINDOW EDIT THIS HEIGHT TO MATCH

            current = (scrollspeed)

            function HeightData(){
            AreaHeight=dataobj.offsetHeight
            if (AreaHeight==0){
            setTimeout("HeightData()",( startdelay * 1000 ))
            }
            else {
            ScrollNewsDiv()
            }}

            function NewsScrollStart(){
            dataobj=document.all? document.all.NewsDiv : document.getElementById("NewsDiv")
            dataobj.style.top=topspace
            setTimeout("HeightData()",( startdelay * 1000 ))
            }

            function ScrollNewsDiv(){
            dataobj.style.top=parseInt(dataobj.style.top)-(scrollspeed)
            if (parseInt(dataobj.style.top)<AreaHeight*(-1)) {
            dataobj.style.top=frameheight
            setTimeout("ScrollNewsDiv()",( nextdelay * 1000 ))
            }
            else {
            setTimeout("ScrollNewsDiv()",speedjump)
            }}

            // END HIDE CODE -->
    </script>

    <script language="javascript"  type= "text/javascript">
        function checkLogin() {
            var mem = ""
            mem = '<%= Session.Item("LoggedMemberNo") %>';
            //alert(mem);
            if (mem == "") {
                alert("Please login the site to avail this facility..");
                return false;
            }
            else {
                return true;
            }
            return false;
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
    <script language ="javascript" type ="text/javascript" >
        function Select2(Select2) {

            var grdv = document.getElementById('<%= Grid3.ClientID %>');
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

    <script src="../Scripts/jquery-1.8.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tooltip.min.js" type="text/javascript"></script>
    <script type="text/javascript">
            function InitializeToolTip() {
                $(".gridViewToolTip").tooltip({
                    track: true,
                    delay: 0,
                    showURL: false,
                    fade: 100,
                    bodyHandler: function () {
                        return $($(this).next().html());
                    },
                    showURL: false
                });
            }
    </script>
    <script type="text/javascript">
        $(function () {
            InitializeToolTip();
        })
    </script>

<script type="text/javascript">
    function pageLoad(sender, args) {    
   if (args.get_isPartialLoad()) {        
      InitializeToolTip();    
   }
}
</script>

                

   <%--  <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode= "Always">
                    <ContentTemplate>--%>
        
       
       

        <table id ="Table1" runat="server" align="left" cellspacing="4" border="0"  cellpadding="5" class="style34">
            <tr>
                <td  bgcolor="#99CCFF" class="style43" colspan="2">
                     <asp:Label ID="Label5" runat="server" Text="Welcome to e-Granthalaya 4.0 OPAC" 
                         style="font-size: medium; color: #0066CC;"></asp:Label>
                 </td>
            </tr>
        
            <tr id ="TR_EG_INTRO" runat="server">
                <td class="style49" valign="top" colspan="2">
                    <strong>e-Granthalaya</strong>: A Digital Agenda for Library Automation and 
                    Networking - is a Library Management Software, developed and maintained by
                    <strong>National Informatics Centre</strong>, Department of Electronics and 
                    Information Technology, Ministry of Communications and Information Technology, 
                    Government of India.&nbsp; The software is being provided at ZERO cost to the 
                    Government Libraries in the Country. Besides, NIC is providing support and 
                    training for the software to automate such Libraries. The Software runs on 
                    Windows platform and uses MS SQL Server and PostgreSQL as back-end solutions. 
                    The Software is multi-lingual, UNICODE compliant, provides a web based data 
                    entry solution over Intranet/Internet. The current version of the software is 
                    4.0 - Enterprise Editions, provides a web-based data entry solution with a 
                    centralized database for a cluster of libraries and implemented / hosted in NIC 
                    Server in cloud computing environment. <a href="http://egranthalaya.nic.in" 
                        target="_blank">Read More....</a>
                    <br /> </td>
            </tr>

            <tr id ="TR_Trans" runat="server" align="center">
                <td align="center" class="style49" valign="top" colspan="2">&nbsp; 
                <center><strong style="color: #ff0000; font-size: 10pt;">Type in Indian languages (Press Ctrl+g to toggle between English and India Language)</strong>
                 <div id='translControl' class="style63"></div></center>
                </td>
            
            </tr>
          
              

                <tr align="center" bgcolor="#CCCCCC">
                    <td align="center" class="style43" valign="top" colspan="2">
                                           Enter Search Text
                        <asp:TextBox ID="TextBox1" runat="server" AutoCompleteType="DisplayName" 
                            Font-Bold="True" ForeColor="#3399FF" Height="16px" ViewStateMode="Enabled" 
                            Width="215px"></asp:TextBox>
                        
                        <asp:Button ID="Basic_Search_Bttn" runat="server" BorderColor="Red" 
                            BorderStyle="Solid" CssClass="style38" Font-Bold="True" ForeColor="#FF3300" 
                            Height="20px" Text="Submit" UseSubmitBehavior="true" Width="70px" />
                        
                    </td>
                </tr>
                 
                  <tr ID="TR_RB" runat="server">
                    <td class="style49" valign="top" colspan="2">



                          <ajaxToolkit:Accordion ID="Accordion2" runat="server"   CssClass="accordion"
                                  HeaderCssClass="accordionHeader"
                                HeaderSelectedCssClass="accordionHeaderSelected"  
                                ContentCssClass="accordionContent" 
                                 RequireOpenedPane="False"     SuppressHeaderPostbacks="True"   
                                SelectedIndex="0"   Width="100%" Height="237px" FramesPerSecond="30" 
                                TransitionDuration="250"   >  
                            <Panes>  
                                <ajaxToolkit:AccordionPane ID="RecentBooks" runat="server" >  
                                    <Header>Click To View / Hide List of Recent Arrivals - Books and Monographs</Header>  
                                        <Content> 
                                        <asp:Button ID="Book_Reserve_Bttn" runat="server" BorderColor="Red" 
                                        BorderStyle="Solid" CssClass="style38" Font-Bold="True" ForeColor="#FF3300" 
                                        Height="20px" Text="Reserve Selected Records" UseSubmitBehavior="true" Width="200px" OnClientClick ="return checkLogin();"/>
                                        &nbsp;

                                <asp:Panel ID="Panel8" runat="server" Height="300px" ScrollBars="Auto">

                                     <asp:GridView ID="Grid1" runat="server" AllowPaging="True" allowsorting="True" 
                                         AutoGenerateColumns="False" DataKeyNames="HOLD_ID" Font-Bold="True"  PageSize="100"
                                         Font-Names="Tahoma" Font-Size="8pt" Height="100px" HorizontalAlign="Center" 
                                         ShowFooter="True" style="width: 100%;  text-align: center;">
                                     <Columns>
                                                                       


                                         <asp:TemplateField HeaderText="S.N.">
                                             <ItemTemplate>
                                                 <asp:Label ID="lblsr" runat="server" CssClass="MBody"  Text = '<%# Container.dataitemindex+1 %>' SkinID="" width="25px"></asp:Label>
                                             </ItemTemplate>
                                             <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                                             <ItemStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" 
                                                 ForeColor="#336699" Width="25px" />
                                         </asp:TemplateField>

                                         <asp:BoundField DataField="ACCESSION_NO" HeaderText="Accession No" SortExpression="ACCESSION_NO" visible="true">
                                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small"  Width="100px" />
                                            <ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left"  width="100px" />
                                         </asp:BoundField>

                                         <asp:BoundField DataField="TITLE" HeaderText="Title" SortExpression="TITLE" visible="true">
                                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small"  Width="450px" />
                                            <ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left"  width="450px" />
                                         </asp:BoundField>

                                          <asp:BoundField DataField="YEAR_OF_PUB" HeaderText="Year" SortExpression="YEAR_OF_PUB" visible="true">
                                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small"  Width="100px" />
                                            <ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left"  width="100px" />
                                         </asp:BoundField>

                                          <asp:BoundField DataField="STA_NAME" HeaderText="Status" SortExpression="STA_NAME" visible="true">
                                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small"  Width="100px" />
                                            <ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left"  width="100px" />
                                         </asp:BoundField>

                                          <asp:BoundField DataField="LIB_CODE" HeaderText="Library"  SortExpression="LIB_CODE" visible="true">
                                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="100px" />
                                            <ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left" width="100px" />
                                         </asp:BoundField>

                        
                                        <asp:TemplateField  ControlStyle-Width="10px"  HeaderText="Reserve" FooterText="Select to Reserve" ShowHeader="true">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cbd" runat="server"/>
                                            </ItemTemplate>
                                            <HeaderTemplate>
                                                <asp:ImageButton ID="ImageButton1" runat="server" Height="16px" ImageUrl="~/Images/check_all.gif" onClientclick="return Select(true)" ToolTip="Select All" Width="16px" />
                                                <asp:ImageButton ID="ImageButton2" runat="server" Height="16px" ImageUrl="~/Images/uncheck_all.gif" OnClientClick="return Select(false)" ToolTip="Deselect All" Width="16px" />
                                            </HeaderTemplate>
                                            <ControlStyle Width="50px" />
                                        </asp:TemplateField>
                         
                         

                        
                              
                         
                                     </Columns>
                                     <PagerStyle BackColor="#3399FF" BorderColor="#C0C000" BorderStyle="Solid" 
                                         Font-Bold="True" Font-Size="Small" ForeColor="White" HorizontalAlign="Center" 
                                         VerticalAlign="Middle" />
                                     <RowStyle BackColor="#DADADA" BorderColor="Desktop" ForeColor="#3399FF" />
                                     <SelectedRowStyle BackColor="#ff9933" BorderColor="SteelBlue" 
                                         BorderStyle="Solid" />
                                     <HeaderStyle BackColor="Desktop" Font-Bold="True" Font-Italic="False" 
                                         Font-Names="Times new roman" Font-Overline="False" Font-Underline="False" 
                                         ForeColor="White" Width="90%" />
                                     <PagerSettings FirstPageText="First" LastPageText="Last" 
                                         Mode="NumericFirstLast" PageButtonCount="10" Position="TopAndBottom" />
                                     <AlternatingRowStyle BackColor="#EFEFEF" Font-Names="Tahoma" 
                                         ForeColor="#0066FF" />
                                 </asp:GridView>
                         </asp:Panel>

                                  </Content>  
                                </ajaxToolkit:AccordionPane>  

                                <ajaxToolkit:AccordionPane ID="RecentArticles" runat="server">  
                                    <Header>Click To View / Hide List of Recent Articles (Micro-Documents)</Header>  
                                        <Content>

                                                <asp:Panel ID="Panel9" runat="server" Height="300px" ScrollBars="Auto">

                                                 <asp:GridView ID="Grid2" runat="server" AllowPaging="True" allowsorting="True" 
                                                     AutoGenerateColumns="False" DataKeyNames="ART_NO" Font-Bold="True"  PageSize="100"
                                                     Font-Names="Tahoma" Font-Size="8pt" Height="100px" HorizontalAlign="Center" 
                                                    ShowFooter="True" style="width: 100%;  text-align: center;">
                                                 <Columns>
                                                     <asp:TemplateField HeaderText="S.N.">
                                                         <ItemTemplate>
                                                             <asp:Label ID="lblsr" runat="server" CssClass="MBody"  Text = '<%# Container.dataitemindex+1 %>' SkinID="" width="25px"></asp:Label>
                                                               
                                                         </ItemTemplate>
                                                         <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                                                         <ItemStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" 
                                                             ForeColor="#336699" Width="25px" />
                                                     </asp:TemplateField>

                                                     <asp:BoundField DataField="ART_TITLE" HeaderText="Title" SortExpression="ART_TITLE" visible="true">
                                                        <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small"  Width="400px" />
                                                        <ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left"  width="400px" />
                                                     </asp:BoundField>

                                                     <asp:BoundField DataField="SOURCE" HeaderText="Source" SortExpression="SOURCE" visible="true">
                                                        <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small"  Width="450px" />
                                                        <ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left"  width="450px" />
                                                     </asp:BoundField>

                                                      <asp:BoundField DataField="LIB_CODE" HeaderText="Library"  SortExpression="LIB_CODE" visible="true">
                                                        <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="100px" />
                                                        <ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left" width="100px" />
                                                     </asp:BoundField>

                        
                                                     <asp:TemplateField HeaderText="Details">
                                                        <ItemTemplate>
                                                             <a href="#" class="gridViewToolTip"><asp:Image ID="Image1" runat="server" ImageUrl="~/Images/zoom.jpg" /></a>
                                                               <div id="tooltip1" style="display: none;">
                                                                    <table align="left" cellspacing="1"  cellpadding="1" border="1">
                                                                        <tr>
                                                                            <td align="left" style="white-space: nowrap;"><b>Title:</b>&nbsp;</td>
                                                                            <td align="left"><%# Eval("ART_TITLE")%></td>
                                                                        </tr>
                                                                        <tr id="TR11" runat="server" Visible='<%# not Eval("SOURCE") is DBNull.Value %>' > 
                                                                            <td align="left" style="white-space: nowrap;"><b>Source:</b>&nbsp;</td>
                                                                            <td align="left"><%# Eval("SOURCE")%></td>
                                                                        </tr>
                                                                         <tr id="TR10" runat="server" Visible='<%# not Eval("AUTHORS") is DBNull.Value %>' > 
                                                                            <td align="left" style="white-space: nowrap;"><b>Authors:</b>&nbsp;</td>
                                                                            <td align="left"><%# Eval("AUTHORS")%></td>
                                                                        </tr>
                                                                        <tr id="TR9" runat="server" Visible='<%# not Eval("VOL") is DBNull.Value %>' > 
                                                                            <td align="left" style="white-space: nowrap;"><b>Vol:</b>&nbsp;</td>
                                                                            <td align="left"><%# Eval("VOL")%></td>
                                                                        </tr>
                                                                        <tr id="TR8" runat="server" Visible='<%# not Eval("ISSUE") is DBNull.Value %>' > 
                                                                            <td align="left" style="white-space: nowrap;"><b>Issue:</b>&nbsp;</td>
                                                                            <td align="left"><%# Eval("ISSUE")%></td>
                                                                        </tr>
                                                                        <tr id="TR7" runat="server" Visible='<%# not Eval("PERIOD") is DBNull.Value %>' > 
                                                                            <td align="left" style="white-space: nowrap;"><b>Period:</b>&nbsp;</td>
                                                                            <td align="left"><%# Eval("PERIOD")%></td>
                                                                        </tr>
                                                                         <tr id="TR6" runat="server" Visible='<%# not Eval("PAGE") is DBNull.Value %>' > 
                                                                            <td align="left" style="white-space: nowrap;"><b>Page:</b>&nbsp;</td>
                                                                            <td align="left"><%# Eval("PAGE")%></td>
                                                                        </tr>
                                                                    </table>
                                                                 </div>
                                                        </ItemTemplate>
                                                        <ItemStyle  Width= "40px"   Height="5px" ForeColor="#003366"/>
                                                    </asp:TemplateField>
                         
                         

                        
                              
                         
                                                 </Columns>
                                                 <PagerStyle BackColor="#3399FF" BorderColor="#C0C000" BorderStyle="Solid" 
                                                     Font-Bold="True" Font-Size="Small" ForeColor="White" HorizontalAlign="Center" 
                                                     VerticalAlign="Middle" />
                                                 <RowStyle BackColor="#DADADA" BorderColor="Desktop" ForeColor="#3399FF" />
                                                 <SelectedRowStyle BackColor="#ff9933" BorderColor="SteelBlue" 
                                                     BorderStyle="Solid" />
                                                 <HeaderStyle BackColor="Desktop" Font-Bold="True" Font-Italic="False" 
                                                     Font-Names="Times new roman" Font-Overline="False" Font-Underline="False" 
                                                     ForeColor="White" Width="90%" />
                                                 <PagerSettings FirstPageText="First" LastPageText="Last" 
                                                     Mode="NumericFirstLast" PageButtonCount="10" Position="TopAndBottom" />
                                                 <AlternatingRowStyle BackColor="#EFEFEF" Font-Names="Tahoma" 
                                                     ForeColor="#0066FF" />
                                             </asp:GridView>
                                            </asp:Panel>


                                        </Content>  
                                     </ajaxToolkit:AccordionPane> 
                            




                             <ajaxToolkit:AccordionPane ID="LooseIssues" runat="server" >  
                                    <Header>Click To View / Hide List of Recent Loose Issues of Journals</Header>  
                                        <Content> 
                                        <asp:Button ID="Loose_Reserve_Bttn" runat="server" BorderColor="Red" BorderStyle="Solid" CssClass="style38" Font-Bold="True" ForeColor="#FF3300" 
                                        Height="20px" Text="Reserve Selected Records" UseSubmitBehavior="true" Width="200px" OnClientClick ="return checkLogin();"/>
                                        &nbsp;

                                <asp:Panel ID="Panel10" runat="server" Height="300px" ScrollBars="Auto">

                                     <asp:GridView ID="Grid3" runat="server" AllowPaging="True" allowsorting="True" 
                                         AutoGenerateColumns="False" DataKeyNames="COPY_ID" Font-Bold="True"  PageSize="100"
                                         Font-Names="Tahoma" Font-Size="8pt" Height="100px" HorizontalAlign="Center" 
                                        ShowFooter="True" style="width: 100%;  text-align: center;">
                                     <Columns>
                                         <asp:TemplateField HeaderText="S.N.">
                                             <ItemTemplate>
                                                  <asp:Label ID="lblsr" runat="server" CssClass="MBody"  Text = '<%# Container.dataitemindex+1 %>' SkinID="" width="25px"></asp:Label>
                                             </ItemTemplate>
                                             <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                                             <ItemStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" 
                                                 ForeColor="#336699" Width="25px" />
                                         </asp:TemplateField>

                                         <asp:BoundField DataField="COPY_ID" HeaderText="Item No" SortExpression="COPY_ID" visible="true">
                                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small"  Width="100px" />
                                            <ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left"  width="100px" />
                                         </asp:BoundField>

                                         <asp:BoundField DataField="TITLE" HeaderText="Title" SortExpression="TITLE" visible="true">
                                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small"  Width="450px" />
                                            <ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left"  width="450px" />
                                         </asp:BoundField>

                                          <asp:BoundField DataField="VOL_NO" HeaderText="Vol" SortExpression="VOL_NO" visible="true">
                                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small"  Width="100px" />
                                            <ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left"  width="100px" />
                                         </asp:BoundField>

                                          <asp:BoundField DataField="ISSUE_NO" HeaderText="Issue" SortExpression="ISSUE_NO" visible="true">
                                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small"  Width="100px" />
                                            <ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left"  width="100px" />
                                         </asp:BoundField>

                                           <asp:BoundField DataField="PART_NO" HeaderText="Part" SortExpression="PART_NO" visible="true">
                                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small"  Width="100px" />
                                            <ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left"  width="100px" />
                                         </asp:BoundField>

                                          <asp:BoundField DataField="ISS_DATE" HeaderText="Issue Date" SortExpression="ISS_DATE" visible="true" DataFormatString="{0:dd/MM/yyyy}"  >
                                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small"  Width="100px" />
                                            <ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left"  width="100px" />
                                         </asp:BoundField>

                                          <asp:BoundField DataField="LOOSE_ISSUE_LIB_CODE" HeaderText="Library"  SortExpression="LOOSE_ISSUE_LIB_CODE" visible="true">
                                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="100px" />
                                            <ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left" width="100px" />
                                         </asp:BoundField>

                                          <asp:BoundField DataField="STA_NAME" HeaderText="Status"  SortExpression="STA_NAME" visible="true">
                                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="100px" />
                                            <ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left" width="100px" />
                                         </asp:BoundField>

                        
                         
                                        <asp:TemplateField  ControlStyle-Width="10px"  HeaderText="Reserve" FooterText="Select to Reserve" ShowHeader="true">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cbd" runat="server"/>
                                            </ItemTemplate>
                                            <HeaderTemplate>
                                                <asp:ImageButton ID="ImageButton1" runat="server" Height="16px" ImageUrl="~/Images/check_all.gif" onClientclick="return Select2(true)" ToolTip="Select All" Width="16px" />
                                                <asp:ImageButton ID="ImageButton2" runat="server" Height="16px" ImageUrl="~/Images/uncheck_all.gif" OnClientClick="return Select2(false)" ToolTip="Deselect All" Width="16px" />
                                            </HeaderTemplate>
                                            <ControlStyle Width="50px" />
                                        </asp:TemplateField>
                         

                        
                              
                         
                                     </Columns>
                                     <PagerStyle BackColor="#3399FF" BorderColor="#C0C000" BorderStyle="Solid" 
                                         Font-Bold="True" Font-Size="Small" ForeColor="White" HorizontalAlign="Center" 
                                         VerticalAlign="Middle" />
                                     <RowStyle BackColor="#DADADA" BorderColor="Desktop" ForeColor="#3399FF" />
                                     <SelectedRowStyle BackColor="#ff9933" BorderColor="SteelBlue" 
                                         BorderStyle="Solid" />
                                     <HeaderStyle BackColor="Desktop" Font-Bold="True" Font-Italic="False" 
                                         Font-Names="Times new roman" Font-Overline="False" Font-Underline="False" 
                                         ForeColor="White" Width="90%" />
                                     <PagerSettings FirstPageText="First" LastPageText="Last" 
                                         Mode="NumericFirstLast" PageButtonCount="10" Position="TopAndBottom" />
                                     <AlternatingRowStyle BackColor="#EFEFEF" Font-Names="Tahoma" 
                                         ForeColor="#0066FF" />
                                 </asp:GridView>
                         </asp:Panel>

                                  </Content>  
                                </ajaxToolkit:AccordionPane>  
                            </Panes>
                        </ajaxToolkit:Accordion>

                    </td>
                </tr>




                <tr ID="TR_TRANSACTIONS" runat="server">
                    <td class="style49" valign="top" colspan="2">

                  

                        <asp:Panel ID="Panel11" runat="server" Height="250px" ScrollBars="Auto">

                           <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always" >
                                 <ContentTemplate>

                           <asp:GridView ID="Grid4" runat="server" AllowPaging="true" DataKeyNames="CIR_ID"  
                                style="width: 100%;  text-align:  left;" allowsorting="true" 
                                AutoGenerateColumns="False"  PageSize="100"  Font-Bold="True" 
                                Font-Names="Tahoma" Font-Size="8pt" Height="100px"  
                                HorizontalAlign="Center"  Width="6px" ShowFooter="True">
                                <Columns >                
                                    <asp:TemplateField HeaderText="S.N.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblsr"  runat="server" CssClass="MBody"   
                                                Text = '<%# Container.dataitemindex+1 %>' width="25px"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                                        <ItemStyle  Font-Names="Tahoma" Width="20px" Font-Bold="True" Font-Size="Small" />
                                   </asp:TemplateField>
                    
                                    <asp:BoundField   DataField="MEM_NO" HeaderText="Member No" SortExpression="MEM_NO" >
                                        <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" />
                                        <ItemStyle  horizontalalign="Left" width="80px"   Font-Names="Arial"/>
                                    </asp:BoundField>

                                    <asp:BoundField   DataField="ACCESSION" HeaderText="Accession No" SortExpression="ACCESSION" ><HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" /><ItemStyle  horizontalalign="Left" width="80px"   Font-Names="Arial"/></asp:BoundField>
                                    <asp:BoundField   DataField="TITLE" HeaderText="Title" SortExpression="TITLE" ReadOnly="True"><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" /><ItemStyle  horizontalalign="Left" Font-Names="Arial"  width="350px" /></asp:BoundField>
                                    <asp:BoundField   DataField="ISSUE_DATE" HeaderText="Issue Date" SortExpression="ISSUE_DATE" ReadOnly="True"><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" /><ItemStyle  horizontalalign="Left" Font-Names="Arial"  width="100px" /></asp:BoundField>
                                    <asp:BoundField   DataField="DUE_DATE" HeaderText="Due Date" SortExpression="DUE_DATE" ReadOnly="True"><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" /><ItemStyle  horizontalalign="Left" Font-Names="Arial"  width="100px" /></asp:BoundField>
                                    <asp:BoundField   DataField="RESERVE_DATE" HeaderText="Reserve Date" SortExpression="RESERVE_DATE" ReadOnly="True"><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" /><ItemStyle  horizontalalign="Left" Font-Names="Arial"  width="100px" /></asp:BoundField>
                                    <asp:BoundField   DataField="RENEW_DATE" HeaderText="Renew Date" SortExpression="RENEW_DATE" ReadOnly="True"><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" /><ItemStyle  horizontalalign="Left" Font-Names="Arial"  width="100px" /></asp:BoundField>
                                    <asp:BoundField   DataField="RETURN_DATE" HeaderText="Return Date" SortExpression="RETURN_DATE" ReadOnly="True"><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" /><ItemStyle  horizontalalign="Left" Font-Names="Arial"  width="100px" /></asp:BoundField>
                                    <asp:BoundField   DataField="STATUS" HeaderText="Status" SortExpression="STATUS" ReadOnly="True"><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" /><ItemStyle  horizontalalign="Left" Font-Names="Arial"  width="100px" /></asp:BoundField>
                            
                                     
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
                    </asp:UpdatePanel>

                 </asp:Panel>                     
                    </td>
                </tr>

                <tr ID="TR_CLUSTER_INTRO" runat="server">
                    <td class="style49" valign="top" colspan="2">
                        <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                         <asp:Label ID="Lbl_Contact" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style49" valign="top" colspan="2">
                          <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode= "Always">
                    <ContentTemplate>

                        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
                            allowsorting="True" AutoGenerateColumns="False" DataKeyNames="CAT_NO" 
                            Font-Bold="True" Font-Names="Times New Roman" Font-Size="8pt" 
                            HorizontalAlign="Center" PageSize="100" 
                            style="width: 98%;  text-align: center;" Width="6px">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblsr" runat="server" CssClass="MBody" SkinID="" 
                                            Text="<%# Container.dataitemindex+1 %>" width="25px"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Font-Names="Tahoma" ForeColor="#0066FF" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Title">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hltitle" runat="server" class="styleTitle" 
                                            NavigateUrl='<%# "Details.aspx?ctr="+ cstr(Eval("CAT_NO")) %>' 
                                            onclick="return GB_showCenter('Details', this.href, 710, 710)" Target="_blank"><%# DataBinder.Eval(Container.DataItem, "TITLE")%></asp:HyperLink>
                                    </ItemTemplate>
                                    <ItemStyle Font-Names="Arial" ForeColor="#3399FF" horizontalalign="Left" 
                                        width="80%" />
                                </asp:TemplateField>


                              


                                 <asp:TemplateField HeaderText="Details">
                                    <ItemTemplate>
                                        <a href="#" class="gridViewToolTip" style="text-decoration: none"> 
                                            <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/zoom.jpg"/></a>
                                             <div id="tooltip" style="display: none;">
                                                <table  align="left" cellspacing="1"  cellpadding="1" border="1">
                                                    <tr> 
                                                        <td align="left" style="white-space: nowrap;"><b>CAT NO:</b>&nbsp;</td>
                                                        <td align="left"><%# Eval("CAT_NO")%></td>
                                                    </tr>
                                                    <tr> 
                                                        <td align="left" style="white-space: nowrap;"><b>TITLE:</b>&nbsp;</td>
                                                        <td align="left"><%# Eval("TITLE")%></td>
                                                    </tr>
                                                    <tr> 
                                                        <td align="left" style="white-space: nowrap;"><b>LANGUAGE:</b>&nbsp;</td>
                                                        <td align="left"><%# Eval("LANG_CODE")%></td>
                                                    </tr>
                                                                                                                                                     
                                                    <tr id="TR6" runat="server" Visible='<%# not Eval("AUTHOR1") is DBNull.Value %>' > 
                                                        <td align="left" style="white-space: nowrap;"><b>AUTHOR1:</b>&nbsp;</td>
                                                        <td align="left"><%# Eval("AUTHOR1")%></td>
                                                    </tr>

                                                    <tr id="TR5" runat="server" Visible='<%# not Eval("AUTHOR2") is DBNull.Value %>' > 
                                                        <td align="left" style="white-space: nowrap;"><b>AUTHOR2:</b>&nbsp;</td>
                                                        <td align="left"><%# Eval("AUTHOR2")%></td>
                                                    </tr>
                                                   <tr id="TR4" runat="server" Visible='<%# not Eval("AUTHOR3") is DBNull.Value %>' > 
                                                        <td align="left" style="white-space: nowrap;"><b>AUTHOR3:</b>&nbsp;</td>
                                                        <td align="left"><%# Eval("AUTHOR3")%></td>
                                                    </tr>
                                                    <tr id="TR3" runat="server" Visible='<%# not Eval("PUB_NAME") is DBNull.Value %>' > 
                                                        <td align="left" style="white-space: nowrap;"><b>PUBLISHER:</b>&nbsp;</td>
                                                        <td align="left"><%# Eval("PUB_NAME")%></td>
                                                    </tr>
                                                    <tr id="TR2" runat="server" Visible='<%# not Eval("PLACE_OF_PUB") is DBNull.Value %>' > 
                                                        <td align="left" style="white-space: nowrap;"><b>PLACE:</b>&nbsp;</td>
                                                        <td align="left"><%# Eval("PLACE_OF_PUB")%></td>
                                                    </tr>

                                                     <tr id="TR1" runat="server" Visible='<%# not Eval("YEAR_OF_PUB") is DBNull.Value %>' > 
                                                        <td align="left" style="white-space: nowrap;"><b>YEAR:</b>&nbsp;</td>
                                                        <td align="left"><%# Eval("YEAR_OF_PUB")%></td>
                                                    </tr>
                                                   
                                                     <tr id="TR_PUB_NAME" runat="server" Visible='<%# not Eval("SUB_NAME") is DBNull.Value %>' > 
                                                        <td align="left" style="white-space: nowrap;"><b>SUBJECT:</b>&nbsp;</td>
                                                        <td align="left"><%# Eval("SUB_NAME")%></td>
                                                    </tr>
                                                   
                                                </table>
                                            </div>
                         
                                    </ItemTemplate>
                                    <ItemStyle  Width="60px"   Height="5px" ForeColor="#003366"/>
                                </asp:TemplateField>



                                 
                                </Columns>
                            <PagerStyle BackColor="Gray" BorderColor="#E4E4E4" BorderStyle="Solid" 
                                Font-Bold="True" ForeColor="White" HorizontalAlign="Center" 
                                VerticalAlign="Middle" />
                            <RowStyle BackColor="#DADADA" BorderColor="Desktop" ForeColor="#3399FF" />
                            <SelectedRowStyle BackColor="Desktop" BorderColor="SteelBlue" 
                                BorderStyle="Solid" />
                            <HeaderStyle BackColor="Desktop" Font-Bold="True" Font-Italic="False" 
                                Font-Names="Times new roman" Font-Overline="False" Font-Underline="False" 
                                ForeColor="White" Width="80%" />
                            <PagerSettings FirstPageText="First" LastPageText="Last" 
                                Mode="NumericFirstLast" PageButtonCount="20" Position="TopAndBottom" />
                            <AlternatingRowStyle BackColor="#EFEFEF" />
                        </asp:GridView>
                         
                        
                         </ContentTemplate>
                          </asp:UpdatePanel>
                    </td>
                </tr>

              
           
      

                
        
        </table>

         <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode= "Always">
                    <ContentTemplate>


         <table id ="Table2" runat="server" align="left" cellspacing="4" border="0" cellpadding="5"  class="style35">
            <tr>
                <td  bgcolor="#99CCFF" class="style43" colspan="2">
                     <asp:Label ID="Label2" runat="server" Text="Statistics" 
                         style="font-size: medium; color: #0066CC;"></asp:Label>
                 </td>
            </tr>
        
            <tr align="center">
                <td  id ="TD_STAT" runat="server" class="style49" valign="top" align ="center" 
                    colspan="2">
                     <asp:GridView ID="Grid1_Stat" runat="server" AutoGenerateColumns="False" 
                         Font-Size="Small" ForeColor="#CC3300" ShowFooter="True" Width="100%">
                         <AlternatingRowStyle BackColor="#CCCCCC" />
                         <Columns>
                            

                              <asp:TemplateField HeaderText="Library">
                                    <ItemTemplate>
                                        <a href="#" class="gridViewToolTip" style="text-decoration: none"><%# Eval("LIB_CODE")%></a>
                                             <div id="tooltip" style="display: none;">
                                                <table  align="left" cellspacing="1"  cellpadding="1" border="1">
                                                    <tr> 
                                                        <td align="left" style="white-space: nowrap;"><b>LIBRARY:</b>&nbsp;</td>
                                                        <td align="left"><%# Eval("LIB_NAME")%></td>
                                                    </tr>
                                                      <tr> 
                                                        <td align="left" style="white-space: nowrap;"><b>ORG.:</b>&nbsp;</td>
                                                        <td align="left"><%# Eval("PARENT_BODY")%></td>
                                                    </tr>
                                                     <tr id="LIB_ADDRESS" runat="server" Visible='<%# not Eval("LIB_ADDRESS") is DBNull.Value %>' > 
                                                        <td align="left" style="white-space: nowrap;"><b>ADDRESS:</b>&nbsp;</td>
                                                        <td align="left"><%# Eval("LIB_ADDRESS")%></td>
                                                    </tr>

                                                     <tr id="Tr12" runat="server" Visible='<%# not Eval("LIB_EMAIL") is DBNull.Value %>' > 
                                                        <td align="left" style="white-space: nowrap;"><b>EMAIL:</b>&nbsp;</td>
                                                        <td align="left"><%# Eval("LIB_EMAIL")%></td>
                                                    </tr>

                                                     <tr id="Tr13" runat="server" Visible='<%# not Eval("LIB_PHONE") is DBNull.Value %>' > 
                                                        <td align="left" style="white-space: nowrap;"><b>PHONE:</b>&nbsp;</td>
                                                        <td align="left"><%# Eval("LIB_PHONE")%></td>
                                                    </tr>
                                                   
                                                </table>
                                            </div>
                         
                                    </ItemTemplate>
                                    <ItemStyle  Width="60px"   Height="5px" ForeColor="#003366"/>
                                </asp:TemplateField>

                             <asp:BoundField DataField="HOLD_COUNT" HeaderText="Holdings">
                             <HeaderStyle Font-Bold="False" Font-Names="Arial" Font-Size="Small" />
                             <ItemStyle Font-Names="Arial" Font-Size="Smaller" forecolor="#0066FF" 
                                 horizontalalign="Right" />
                             </asp:BoundField>
                             <asp:BoundField DataField="MEMBER_COUNT" HeaderText="Members">
                             <HeaderStyle Font-Bold="False" Font-Names="Arial" Font-Size="Small" />
                             <ItemStyle Font-Names="Arial" Font-Size="Smaller" forecolor="#0066FF" 
                                 horizontalalign="Right" />
                             </asp:BoundField>
                             <asp:BoundField DataField="CIR_COUNT" HeaderText="Cir.Tr.">
                             <HeaderStyle Font-Bold="False" Font-Names="Arial" Font-Size="Small" />
                             <ItemStyle Font-Names="Arial" Font-Size="Smaller" forecolor="#0066FF" 
                                 horizontalalign="Right" />
                             </asp:BoundField>
                         </Columns>
                         <PagerStyle BackColor="White" BorderColor="white" BorderStyle="Solid" 
                             Font-Bold="True" Font-Size="Smaller" ForeColor="White" HorizontalAlign="Center" 
                             VerticalAlign="Middle" />
                         <RowStyle BackColor="#DADADA" BorderColor="Desktop" ForeColor="White" />
                         <SelectedRowStyle BackColor="Desktop" BorderColor="SteelBlue" 
                             BorderStyle="Solid" />
                         <HeaderStyle BackColor="Desktop" Font-Bold="True" Font-Italic="False" 
                             Font-Names="Tahoma" Font-Overline="False" Font-Underline="False" 
                             ForeColor="White" Width="80%" />
                         <PagerSettings FirstPageText="First" LastPageText="Last" 
                             Mode="NumericFirstLast" PageButtonCount="20" Position="TopAndBottom" />
                         <AlternatingRowStyle BackColor="#EFEFEF" Font-Names="Tahoma" 
                             ForeColor="#0066FF" />
                     </asp:GridView>
                     </td>
           </tr>
            <tr id ="TR_FILTER" runat="server">
                <td class="style43" bgcolor="#99CCFF" valign="top" colspan="2">
                    <asp:Label ID="Label3" runat="server" style="color: #0066CC"></asp:Label></td>
            </tr>
            
            <tr id ="TR_DOC" runat ="server">
                <td class="style49" valign="top" colspan="2"><span class="style50">Document-Wise</span>
                    <asp:Panel ID="Panel2" runat="server"  HorizontalAlign="Left" ScrollBars="Auto" Width="230px">
                          <asp:CheckBoxList ID="CheckBoxList3" runat="server" AutoPostBack="True" 
                              Font-Size="Smaller"></asp:CheckBoxList>
                    </asp:Panel>
                </td>
            </tr>
            <tr id ="TR_LANG" runat ="server">
                <td class="style49" valign="top" colspan="2"><span class="style50">Language-Wise</span>
                    <asp:Panel ID="Panel3" runat="server"  HorizontalAlign="Left" ScrollBars="Auto" Width="230px">
                          <asp:CheckBoxList ID="CheckBoxList1" runat="server" AutoPostBack="True" 
                              Font-Size="Smaller"></asp:CheckBoxList>
                    </asp:Panel>
                </td>
            </tr>
            <tr id ="TR_YEAR" runat ="server">
                <td class="style49" valign="top" colspan="2"><span class="style50">Year-Wise</span>
                    <asp:Panel ID="Panel1" runat="server"  HorizontalAlign="Left" ScrollBars="Auto" Width="230px">
                          <asp:CheckBoxList ID="CheckBoxList2" runat="server" AutoPostBack="True" 
                              Font-Size="Smaller"></asp:CheckBoxList>
                    </asp:Panel>
                </td>
            </tr>
            <tr id ="TR_COUNTRY" runat ="server">
                <td class="style49" valign="top" colspan="2"><span class="style50">Country-Wise</span>
                    <asp:Panel ID="Panel4" runat="server"  HorizontalAlign="Left" ScrollBars="Auto" Width="230px">
                          <asp:CheckBoxList ID="CheckBoxList4" runat="server" AutoPostBack="True" 
                              Font-Size="Smaller"></asp:CheckBoxList>
                    </asp:Panel>
                </td>
            </tr>
            <tr id ="TR_SUB" runat ="server">
                <td class="style49" valign="top" colspan="2"><span class="style50">Subject-Wise</span>
                    <asp:Panel ID="Panel5" runat="server"  HorizontalAlign="Left" ScrollBars="Auto" Width="230px">
                          <asp:CheckBoxList ID="CheckBoxList5" runat="server" AutoPostBack="True" 
                              Font-Size="Smaller"></asp:CheckBoxList>
                    </asp:Panel>
                </td>
            </tr>
            <tr id ="TR_PLACE" runat ="server">
                <td class="style49" valign="top" colspan="2"><span class="style50">Place-Wise</span>
                    <asp:Panel ID="Panel6" runat="server"  HorizontalAlign="Left" ScrollBars="Auto" Width="230px">
                          <asp:CheckBoxList ID="CheckBoxList6" runat="server" AutoPostBack="True" 
                              Font-Size="Smaller"></asp:CheckBoxList>
                    </asp:Panel>
                </td>
            </tr>
            <tr id ="TR_PUB" runat ="server">
                <td class="style49" valign="top" colspan="2"><span class="style50">Publisher-Wise</span>
                    <asp:Panel ID="Panel7" runat="server"  HorizontalAlign="Left" ScrollBars="Auto" Width="230px">
                          <asp:CheckBoxList ID="CheckBoxList7" runat="server" AutoPostBack="True" 
                              Font-Size="Smaller"></asp:CheckBoxList>
                    </asp:Panel>
                </td>
            </tr>
            <tr id ="TR_NEWS" runat ="server" align="right" valign="middle">
                   
                <td style="width:100%; text-align: right;" align="right" valign="middle" colspan="2" >
                    <marquee  id="newsTicker"   runat="server"  scrollamount="2" direction="up" loop="true" behavior="scroll" align="left"   scrolldelay="1" height="300px"  width="100%" > 
                        <span  onClick ="return checkLogin();" onmouseover="MainContent_newsTicker.stop()" onmouseout="MainContent_newsTicker.start()" style="font-size: 12px; width:100%;">
                             <%= strdata%> 
                        </span></marquee>
               </td>
                
           </tr>
           
            
          
           
          
          
         
        </table>

              </ContentTemplate>
                          </asp:UpdatePanel>      


        
    
         
        
                <%--</ContentTemplate>
                  <Triggers>
                    <asp:PostBackTrigger ControlID="Basic_Search_Bttn" />
                    <asp:PostBackTrigger ControlID="CheckBoxList1" />
                    <asp:PostBackTrigger ControlID="CheckBoxList2" />
                    <asp:PostBackTrigger ControlID="CheckBoxList3" /> 
                    <asp:PostBackTrigger ControlID="CheckBoxList4" />
                    <asp:PostBackTrigger ControlID="CheckBoxList5" />
                    <asp:PostBackTrigger ControlID="CheckBoxList6" />
                    <asp:PostBackTrigger ControlID="CheckBoxList7" />
                  </Triggers>
               </asp:UpdatePanel>--%>
        
                
        
    
    
    
    
    
    
    
    
    
    
    
</asp:Content>

