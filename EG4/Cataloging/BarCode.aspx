<%@ Page  Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="BarCode.aspx.vb" Inherits="EG4.BarCode" %>


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
                
        .styleBttn
    {
             cursor:pointer;
            margin-left: 0px;
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
                
        </style>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="http://www.google.com/jsapi"></script> 

     <script language ="javascript" type ="text/javascript" >
         function Select(Select) {

             var grdv = document.getElementById('<%= Grid2.ClientID %>');
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
    
    

        
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                    <ContentTemplate>

        <table id="ADMT1" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35" >
            <tr>
                <td  bgcolor="#003366" class="style43" rowspan="1" style="color: #FFFFFF"><strong>Print BarCode Labels</strong></td>
            </tr>
               
            <tr>
                <td class="style56" colspan="6">
                    <asp:Label ID="Label15" runat="server" Font-Bold="True" Font-Size="Medium" ForeColor="White" Text="STEP 1: Search Records"></asp:Label>
                    <asp:Label ID="Label6" runat="server" Font-Bold="True" Font-Size="Medium" ForeColor="Yellow"></asp:Label>
                </td>
            </tr>
             <tr>
                <td colspan="6" style="text-align: center">                                         
                    Bibliographic Level:
                    <asp:DropDownList ID="DDL_Bib_Level" runat="server" AutoPostBack="True" 
                        Font-Bold="True" ForeColor="#0066CC" ToolTip="Plz Select Value from Drop-Down">
                    </asp:DropDownList>
                    &nbsp;Material Type:
                    <asp:DropDownList ID="DDL_Mat_Type" runat="server" AutoPostBack="True" 
                        Font-Bold="True" ForeColor="#0066CC" ToolTip="Plz Select Value from Drop-Down">
                    </asp:DropDownList>
                    &nbsp;Doc Type:
                    <asp:DropDownList ID="DDL_Doc_Type" runat="server" AutoPostBack="True" 
                        Font-Bold="True" ForeColor="#0066CC" ToolTip="Plz Select Value from Drop-Down">
                    </asp:DropDownList>
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
                         onkeypress="return DateOnly1 (event)" ToolTip="Click to Select Date" 
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
                        Text="Current Status:-" GroupName="Accession" AutoPostBack="True" />
                   
                    <asp:DropDownList ID="DDL_CurrentStatus" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC" ToolTip="Plz Select Value from Drop-Down" 
                        Visible="False">
                    </asp:DropDownList>
                </td>
            </tr>

             

             <tr>             
                <td  align="center">     
                                 <asp:Button ID="Search_Bttn" runat="server"  CssClass="styleBttn" Font-Bold="True" ForeColor="Red" TabIndex="14" Text="Search" AccessKey="s"     Width="74px" />
                     <br />
                </td>
                </tr>
                </table>
               
                <table id="Table2" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
                    <tr>
                     <td  align="center"> 
                     <asp:Label ID="Label1" runat="server" Text="Record(s): "></asp:Label>
                   <div class="style56">
                       <asp:Label ID="Label515" runat="server" Font-Bold="True" Font-Size="Medium" 
                           ForeColor="White" Text="STEP 2: Select BarCode Printer: "></asp:Label>
                       <asp:DropDownList ID="DDL_BarcodePrinters" runat="server" Font-Bold="True" 
                           ForeColor="#0066CC" ToolTip="Plz Select Value from Drop-Down">
                           <asp:ListItem>ARGOX - KV2-KRIBHCO</asp:ListItem>
                           <asp:ListItem>ARGOX BarCode Printer</asp:ListItem>
                           <asp:ListItem>ARGOX CP-2140 KV Meerut</asp:ListItem>
                           <asp:ListItem>ARGOX-OS-214TT BarCode Printer</asp:ListItem>
                           <asp:ListItem>ARGOX-OS-214TT BarCode Printer</asp:ListItem>
                           <asp:ListItem>ARGOX 214 Plus KV LKO Cantt</asp:ListItem>
                           <asp:ListItem>ARGOX CP2140 KV Porbandar</asp:ListItem>
                           <asp:ListItem>ARGOX CP2140 ONGC KV Mehsana</asp:ListItem>
                           <asp:ListItem>ARGOX CP-2140 KV1 DEOLALI</asp:ListItem>
                           <asp:ListItem>ARGOX 214 KV Gomti Nagar Lucknow</asp:ListItem>
                           <asp:ListItem>ARGOX OS214TT FAI Delhi</asp:ListItem>
                           <asp:ListItem>ARGOX OS214TT Siddharth College Mumbai</asp:ListItem>
                           <asp:ListItem>ARGOX CP2140 KV Koliwada</asp:ListItem>
                           <asp:ListItem>CITTZEN CLP 621</asp:ListItem>
                           <asp:ListItem>CITTZEN CL-S621</asp:ListItem>
                           <asp:ListItem>CITIZEN CLS621 JNV_Nandhurbar</asp:ListItem>
                           <asp:ListItem>Cyber-Logix BarCode Printer</asp:ListItem>
                           <asp:ListItem>Datamax I-4212 For KHC</asp:ListItem>
                           <asp:ListItem>Datamax I-4212 For KHC Staff Library</asp:ListItem>
                           <asp:ListItem>GE200 BarCode Printer</asp:ListItem>
                           <asp:ListItem>Godex EZ1105 BarCode Printer</asp:ListItem>
                           <asp:ListItem>Godex EZ1105 KV Maligaon</asp:ListItem>
                           <asp:ListItem>Intermec-PF8T Desktop Printer</asp:ListItem>
                           <asp:ListItem>KV_Barabanki</asp:ListItem>
                           <asp:ListItem>KVCCL Ranchi</asp:ListItem>
                           <asp:ListItem>KV3_JALANDHAR_CANTT</asp:ListItem>
                           <asp:ListItem>KV INS CHILKA</asp:ListItem>
                           <asp:ListItem>Printronix Printer</asp:ListItem>
                           <asp:ListItem>RING408PEL</asp:ListItem>
                           <asp:ListItem>RING408PEL+</asp:ListItem>
                           <asp:ListItem>RING4012PLM</asp:ListItem>
                           <asp:ListItem>RING4012PIM</asp:ListItem>
                           <asp:ListItem>SATO CX400</asp:ListItem>
                           <asp:ListItem>SATO CG408</asp:ListItem>
                           <asp:ListItem>SATO CG408 KVS PUNE</asp:ListItem>
                           <asp:ListItem>SNBC BTP2300E</asp:ListItem>
                           <asp:ListItem>STORM 1x200A</asp:ListItem>
                           <asp:ListItem>STORM 1x200A</asp:ListItem>
                           <asp:ListItem>TSC BarCode Printer</asp:ListItem>
                       </asp:DropDownList>
                       &nbsp;<asp:Button ID="BarCode_Generate_Bttn" runat="server"  CssClass="styleBttn" 
                           Font-Bold="True"  ToolTip="Click to Generate Barcode Labels for Selected Printer"
                        ForeColor="Red" TabIndex="14" Text="Generate Barcode Labels" AccessKey="b"    
                            Width="180px" Height="20px" Enabled="False" />

                   </div>
                   
                   </td>
                   </tr>



                <tr bgcolor="#99CCFF">             
                    <td  align="center">          
                       <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Size="Small" 
                           ForeColor="#0066FF" Text="Print This Text on the Labels: "></asp:Label>
                       &nbsp;<asp:TextBox ID="TextBox1" runat="server" Font-Bold="True" 
                           ForeColor="#0066FF" Height="20px" MaxLength="18" 
                           ToolTip="Enter Text to be printed over labels" Width="200px" Wrap="False"></asp:TextBox>                  
                   </td>
                 </tr>

                  
              </table>


                    <table id="Table1" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
                    <tr>
                     <td  align="center"> 

                      <asp:Panel ID="Panel1" runat="server" ForeColor="#FFCC99" Height="200px" 
                        ScrollBars="Both">                        

                   <asp:GridView ID="Grid2" runat="server" AllowPaging="True" DataKeyNames="HOLD_ID"    
                        style="width: 100%;  text-align: center;" allowsorting="True" 
                        AutoGenerateColumns="False" PageSize="100"  Font-Bold="True" 
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
                                
                    <asp:BoundField   DataField="ACCESSION_NO" HeaderText="Acc No" SortExpression="ACCESSION_NO">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" Width="150px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" Font-Names="Tahoma"/>                        
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
                    
                    <asp:BoundField   DataField="PHYSICAL_LOCATION" SortExpression="PHYSICAL_LOCATION" HeaderText="Location" visible="true">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="100px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="100px" Font-Names="Arial"/>                        
                    </asp:BoundField> 
                                   
                    <asp:TemplateField  ControlStyle-Width="10px"  HeaderText="Delete" FooterText="Select to Print Labels" ShowHeader="true">
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
          </table>


        </ContentTemplate>
        </asp:UpdatePanel>

      

</asp:Content>
