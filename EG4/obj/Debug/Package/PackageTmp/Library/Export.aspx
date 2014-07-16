<%@ Page  Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Export.aspx.vb" Inherits="EG4.Export" SmartNavigation ="true" MaintainScrollPositionOnPostback="true" EnableEventValidation="false"  %>


<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>


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
           
            .style56
        {
            text-align: center;
            width: 100%;
            background-color:#336699;
            color: #FFFFFF;
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
      
      
        </style>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <script type ="text/javascript">
    //alpha-numeric only
    function DateOnly(event) {
        var chCode = ('charCode' in event) ? event.charCode : event.keyCode;
        if (47 <= chCode && chCode <= 57) {
            return (true);
        }

        else {
            alert("Date Is Invalid, Enter in dd/MM/yyyy format only!");
            document.getElementById("MainContent_txt_Status_AccDateFrom").focus();
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
                document.getElementById("MainContent_txt_Status_AccDateTo").focus();
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
       <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
              <ContentTemplate>
       

         <table id="ADMT1" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
            <tr>
                <td  bgcolor="#003366" class="style43" colspan="2" rowspan="1" style="color: #FFFFFF"><strong>Export Data</strong></td>
            </tr>
            <tr>
                <td class="style56" colspan="9" bgcolor="#336699">
                   <asp:Label ID="Label5" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">STEP 1: Search Recrods</asp:Label>
                </td>
            </tr>   
            <tr>
                <td class="style56" colspan="9" bgcolor="#336699">
                   <asp:Label ID="Lbl_Error" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True" style="color: #FFFF00"></asp:Label>
                </td>
            </tr>         
        </table>




         <table id="Table2" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
             <tr>
                <td colspan="6" style="text-align: center">                                         
                    <asp:RadioButton ID="RB_Monographs" runat="server" Font-Bold="True" 
                        Text="Books and Monographs" Checked="True" GroupName="Materials" 
                        ForeColor="#CC3300" />
                    <asp:RadioButton ID="RB_Serials" runat="server" Font-Bold="True" Text="Serials" 
                        GroupName="Materials" ForeColor="#CC3300" />
                </td>
            </tr>
             <tr>
                <td  colspan="6" style="text-align: center">           
                &nbsp;<asp:RadioButton ID="RadioButton6" runat="server" Font-Bold="True" 
                        Text="All Accession No" GroupName="Accession" Checked="True" 
                        AutoPostBack="True" />
                &nbsp;|
                    <asp:RadioButton ID="RadioButton7" runat="server" Font-Bold="True" 
                        Text="Random Accession No: " GroupName="Accession" AutoPostBack="True" />
                    <span>
                    <asp:TextBox ID="txt_Status_RandomAccession" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="18px" ToolTip="Enter Random Accession No separated by ;" 
                        Width="400px" Wrap="False" style="text-transform: uppercase" 
                        Visible="False"></asp:TextBox>
                         <ajaxToolkit:AutoCompleteExtender ID="txt_Status_RandomAccession_AutoCompleteExtender" 
                        runat="server" CompletionSetCount="10" EnableCaching="true" 
                        FirstRowSelected="false" MinimumPrefixLength="1" ServiceMethod="SearchClass" 
                        TargetControlID="txt_Status_RandomAccession">
                    </ajaxToolkit:AutoCompleteExtender>
                    &nbsp;Separated by ;</span></td>
            </tr>

             <tr>
                <td  colspan="6" style="text-align: center">             
                    <asp:RadioButton ID="RadioButton5" runat="server" Font-Bold="True" 
                        Text="Range of Accession NO - " GroupName="Accession" 
                        AutoPostBack="True" />
                      FROM
                    <asp:TextBox ID="txt_Status_AccessionFrom" runat="server" Height="16px" 
                        Width="69px" MaxLength="30" style="text-transform: uppercase" 
                        AutoPostBack="True" Visible="False"></asp:TextBox>
                       
                       <ajaxToolkit:AutoCompleteExtender ID="txt_Status_AccessionFrom_AutoCompleteExtender" 
                        runat="server" CompletionSetCount="10" EnableCaching="true" 
                        FirstRowSelected="false" MinimumPrefixLength="1" ServiceMethod="SearchAccessionNo" 
                        TargetControlID="txt_Status_AccessionFrom">
                    </ajaxToolkit:AutoCompleteExtender>
                        &nbsp;TO
                    <asp:TextBox ID="txt_Status_AccessionTo" runat="server" Height="16px" 
                        Width="69px" MaxLength="30" style="text-transform: uppercase" 
                        Visible="False"></asp:TextBox>
                         <ajaxToolkit:AutoCompleteExtender ID="txt_Status_AccessionTo_AutoCompleteExtender" 
                        runat="server" CompletionSetCount="10" EnableCaching="true" 
                        FirstRowSelected="false" MinimumPrefixLength="1" ServiceMethod="SearchAccessionNo" 
                        TargetControlID="txt_Status_AccessionTo">
                    </ajaxToolkit:AutoCompleteExtender>
                   
                &nbsp;<asp:RadioButton ID="RadioButton8" runat="server" Font-Bold="True" 
                        Text="Range of Accession Date: -" GroupName="Accession" 
                        AutoPostBack="True" />
                   
                    &nbsp;-&nbsp;FROM
                    <span>
                     <asp:TextBox ID="txt_Status_AccDateFrom" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" Height="16px" MaxLength="10" 
                         onkeypress="return DateOnly (event)" ToolTip="Click to Select Date" 
                         Width="71px" Visible="False"></asp:TextBox>
                     <ajaxToolkit:CalendarExtender ID="txt_Status_AccDateFrom_CalendarExtender" 
                         runat="server" Enabled="True" Format="dd/MM/yyyy" 
                         TargetControlID="txt_Status_AccDateFrom">
                     </ajaxToolkit:CalendarExtender>
                    </span>&nbsp;TO
                    <span>
                     <asp:TextBox ID="txt_Status_AccDateTo" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" Height="16px" MaxLength="10" 
                         onkeypress="return DateOnly2 (event)" ToolTip="Click to Select Date" 
                         Width="71px" Visible="False"></asp:TextBox>
                     <ajaxToolkit:CalendarExtender ID="txt_Status_AccDateTo_CalendarExtender" 
                         runat="server" Enabled="True" Format="dd/MM/yyyy" 
                         TargetControlID="txt_Status_AccDateTo">
                     </ajaxToolkit:CalendarExtender>
                    </span>&nbsp;&nbsp;</td>
            </tr>

            <tr>
                <td  colspan="6" style="text-align: center">             
                    &nbsp;<asp:RadioButton ID="RadioButton1" runat="server" Font-Bold="True" 
                        Text="Acquisition Mode:-" GroupName="Accession" AutoPostBack="True" />
                   
                    &nbsp;<asp:DropDownList ID="DDL_AcqModes" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC" ToolTip="Plz Select Value from Drop-Down" 
                        Visible="False">
                    </asp:DropDownList>
                &nbsp;<asp:RadioButton ID="RadioButton9" runat="server" Font-Bold="True" 
                        Text="Section: - " GroupName="Accession" AutoPostBack="True" />
                   
                    <asp:DropDownList ID="DDL_Section1" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" ToolTip="Select Section" Visible="False">
                    </asp:DropDownList>
                &nbsp;<asp:RadioButton ID="RadioButton10" runat="server" Font-Bold="True" 
                        Text="Location:-" GroupName="Accession" AutoPostBack="True" />
                   
                &nbsp;<asp:DropDownList ID="DDL_Location" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC" ToolTip="Plz Select Value from Drop-Down" 
                        Visible="False">
                    </asp:DropDownList>
                &nbsp;<asp:RadioButton ID="RadioButton11" runat="server" Font-Bold="True" 
                        Text="Class No :-" GroupName="Accession" AutoPostBack="True" />
                   
                    <asp:DropDownList ID="DDL_ClassNo" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC" ToolTip="Plz Select Value from Drop-Down" 
                        Visible="False">
                    </asp:DropDownList>
                &nbsp;<asp:RadioButton ID="RadioButton2" runat="server" Font-Bold="True" 
                        Text="Copy Status:-" GroupName="Accession" AutoPostBack="True" />
                   
                    <asp:DropDownList ID="DDL_Status" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC" ToolTip="Plz Select Value from Drop-Down" 
                        Visible="False">
                    </asp:DropDownList>
                    <br />
                    <asp:RadioButton ID="RB_DDC" runat="server" Checked="True" GroupName="Scheme" 
                        Text="DDC" />
                    <asp:RadioButton ID="RB_UDC" runat="server" GroupName="Scheme" Text="UDC" />
                    <asp:RadioButton ID="RB_Other" runat="server" GroupName="Scheme" Text="Other" />
                </td>
            </tr>

             <tr>             
                <td  align="center">     
                                 <asp:Button ID="Search_Bttn" runat="server"  CssClass="styleBttn" Font-Bold="True" ForeColor="Red" TabIndex="14" Text="Search" AccessKey="s"     Width="74px" />

                     <br />
                    <hr />

             

                     <asp:Label ID="Label9" runat="server" Text="Record(s): "></asp:Label>
                   <div class="style56">
                       <asp:Label ID="Label515" runat="server" Font-Bold="True" Font-Size="Medium" 
                           ForeColor="White" 
                           Text="STEP 2: Select Format to Export" 
                           style="font-size: small"></asp:Label>
                       <asp:DropDownList ID="DDL_Formats" runat="server" Font-Bold="True" 
                           ForeColor="#0066CC" ToolTip="Plz Select Value from Drop-Down">
                           <asp:ListItem></asp:ListItem>
                           <asp:ListItem Value="CSV">CSV </asp:ListItem>
                           <asp:ListItem Value="MARC21D">MARC21 Display Format</asp:ListItem>
                           <asp:ListItem Value="MARC21C">MARC21 Comminication Format</asp:ListItem>
                           <asp:ListItem Value="MARCXML">MARCXML</asp:ListItem>
                           <asp:ListItem Value="ISI2709">ISO:2709</asp:ListItem>
                           <asp:ListItem Value ="EXCEL">EXCEL</asp:ListItem>
                       </asp:DropDownList>
                       &nbsp;<asp:Button ID="Export_Bttn" runat="server"  CssClass="styleBttn" 
                           Font-Bold="True"  ToolTip="Click to export Selected Record(s)"
                        ForeColor="Red" TabIndex="14" Text="Export Selected Records" AccessKey="e"    
                            Width="200px" Height="20px" Enabled="false" />

                   </div>
                                           
                  <asp:Panel ID="Panel3" runat="server" ForeColor="#FFCC99" Height="200px" ScrollBars="Both">
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
                                <ItemStyle ForeColor="#336699" Font-Names="Tahoma" Width="20px" 
                                    Font-Bold="True" Font-Size="Small" />
                            </asp:TemplateField>
               
                            <asp:ButtonField HeaderText="Edit"  Text="View" CommandName="Select"  
                                CausesValidation="True">
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" />
                            <ItemStyle ForeColor="#CC0000" Width="50px" Font-Bold="true" />
                            </asp:ButtonField>
                                
                    <asp:BoundField   DataField="ACCESSION_NO" HeaderText="Acc No" SortExpression="ACCESSION_NO">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" Width="100px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="100px" Font-Names="Tahoma"/>                        
                    </asp:BoundField>

                      <asp:BoundField   DataField="TITLE" HeaderText="Title" SortExpression="TITLE">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" Width="250px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="250px" Font-Names="Tahoma"/>                        
                    </asp:BoundField>
                                      
                    <asp:BoundField   DataField="ACCESSION_DATE" SortExpression="ACCESSION_DATE" HeaderText="Acc Date" visible="true"  DataFormatString="{0:dd/MM/yyyy}">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="100px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="100px" Font-Names="Arial"/>                        
                    </asp:BoundField>

                      <asp:BoundField   DataField="VOL_NO" SortExpression="VOL_NO" HeaderText="Vol" visible="true">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="80px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="50px" Font-Names="Arial"/>                        
                    </asp:BoundField>

                      <asp:BoundField   DataField="CLASS_NO" SortExpression="CLASS_NO" HeaderText="Class No" visible="true">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="80px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="50px" Font-Names="Arial"/>                        
                    </asp:BoundField>
                    
                     <asp:BoundField   DataField="STA_NAME" SortExpression="STA_NAME" HeaderText="Current Status" visible="true">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="100px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="100px" Font-Names="Arial"/>                        
                    </asp:BoundField>
                     
                    <asp:TemplateField  ControlStyle-Width="10px"  HeaderText="Delete" FooterText="Select to Update Status" ShowHeader="true">
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
                   


                   

                           
                           
                                                                 
                </td>
            </tr>  
            <tr>
                <td class="style56" colspan="9" bgcolor="#336699">
                   
                   <asp:Label ID="Label516" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">STEP 3: Select Print Format</asp:Label>
                       <asp:DropDownList ID="DDL_PrintFormats" runat="server" Font-Bold="True" 
                           ForeColor="#0066CC" ToolTip="Plz Select Value from Drop-Down">
                           <asp:ListItem></asp:ListItem>
                           <asp:ListItem Value="PDF" Selected="True">Pdf Format</asp:ListItem>
                           <asp:ListItem Value="DOC">Doc Format</asp:ListItem>
                           <asp:ListItem Value="HTML">Html Format</asp:ListItem>
                       </asp:DropDownList>

                       &nbsp;Report Group By:
                    <asp:DropDownList ID="DDL_GroupBy" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC" ToolTip="Plz Select Value from Drop-Down">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem Value="ACQMODE_CODE">Acquisition Mode</asp:ListItem>
                        <asp:ListItem Value="CAT_NO">CAT No</asp:ListItem>
                        <asp:ListItem Value="CLASS_NO">Class No</asp:ListItem>
                        <asp:ListItem Value="COLLECTION_TYPE">Collection Type</asp:ListItem>
                        <asp:ListItem Value="DOC_TYPE_CODE">Document Type</asp:ListItem>
                        <asp:ListItem Value="LANG_CODE">Language</asp:ListItem>
                        <asp:ListItem Value="PHYSICAL_LOCATION">Location</asp:ListItem>
                        <asp:ListItem Value="PUB_NAME">Publisher</asp:ListItem>
                        <asp:ListItem Value="PLACE_OF_PUB">Place</asp:ListItem>
                        <asp:ListItem Value="SEC_CODE">Section-Wise</asp:ListItem>
                        <asp:ListItem Value="SUB_NAME">Subject-Wise</asp:ListItem>
                        <asp:ListItem Value="VEND_NAME">Vendor-Wise</asp:ListItem>
                        <asp:ListItem Value="YEAR_OF_PUB">Year-Wise</asp:ListItem>
                        <asp:ListItem Value="STANDARD_NO">ISBN</asp:ListItem>
                        <asp:ListItem Value="CON_CODE">Country-Wise</asp:ListItem>
                        <asp:ListItem Value="VEND_NAME">Vendor-Wise</asp:ListItem>
                    </asp:DropDownList>

                       <asp:Button ID="Print_Compact_Bttn" runat="server"  CssClass="styleBttn" 
                           Font-Bold="True"
                        ForeColor="Red" TabIndex="14" Text="Compact Report"    
                            Width="120px" Height="20px" Visible="False" />

                    <asp:Button ID="Print_Summary_Bttn" runat="server" CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red" Height="20px" TabIndex="14" 
                        Text="Summary Report" Visible="False" Width="120px" />
                    <asp:Button ID="Print_Detail_Bttn" runat="server" CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red" Height="20px" TabIndex="14" 
                        Text="Detail Report" Visible="False" Width="120px" />

                </td>
            </tr>             
          </table>

                    </ContentTemplate>
                        <Triggers>
                           <asp:PostBackTrigger ControlID="Print_Compact_Bttn" />  
                           <asp:PostBackTrigger ControlID="Print_Summary_Bttn" />  
                           <asp:PostBackTrigger ControlID="Print_Detail_Bttn" />  
                           <asp:PostBackTrigger ControlID="Export_Bttn" />                                                    
                        </Triggers>
                   </asp:UpdatePanel>


      
      <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
              <ContentTemplate>
        
         <table id="Table1" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
             <tr>
                <td class="style47" colspan="2">
                     <asp:Label ID="Label6" runat="server" Font-Size="Medium" ForeColor="#CC3300" 
                         style="font-weight: 700; color: #0066FF; font-size: small;">Details</asp:Label>
                 </td>
            </tr>
            
            <tr>
                <td class="style51"> 
                    Accession No</td>
                <td class="style54">
                      
                     <asp:Label ID="Label517" runat="server" Font-Size="Medium" ForeColor="#CC3300" 
                         style="font-weight: 700; color: #0066FF; font-size: small;"></asp:Label>
                </td>
               
            </tr>
            <tr>
                <td class="style51"> 
                    Title</td>
                <td class="style54">

         
                     <asp:Label ID="Label518" runat="server" Font-Size="Medium" ForeColor="#CC3300" 
                         style="font-weight: 700; color: #0066FF; font-size: small;"></asp:Label>

         
                </td>
               
            </tr>
            
            <tr>
                <td class="style51">Other Details</td>
                <td class="style53">
                     <asp:Label ID="Label519" runat="server" Font-Size="Medium" ForeColor="#CC3300" 
                         style="font-weight: 700; color: #0066FF; font-size: small;"></asp:Label>
                 </td>
            </tr>
            <tr>
                <td class="style47" colspan="2">
                    &nbsp;</td>
            </tr>
            
            


             

        </table>

         </ContentTemplate>
                    
                   </asp:UpdatePanel>



      
   
                 
        
</asp:Content>
