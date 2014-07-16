<%@ Page  Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="SystemData.aspx.vb" Inherits="EG4.SystemData" %>



<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        
        /*Modal Popup*/
                .ModalPopupBG
        {
            background-color: #666699;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }

         .modalXBackground
        {
          background-color:#CCCCFF;
          filter:alpha(opacity=40);
          opacity:0.5;
        }

        .modalPopup
        {
        background-color: Gray;
        border-width: 3px;
        border-style: solid;
        border-color: #165EA9;
        padding: 3px;
        width: 600px;
        height: 150px;
        background-position:center;
        margin-bottom: 100;
         vertical-align:top;
         
        }
   
        .modalBackground {background-color:#fff; filter:alpha(opacity=70); opacity:0.7px; }

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
            height: 26px;
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
        #Table2
        {
            width: 926px;
        }
        .style53
        {
            text-align: justify;
            border-style: none;
            border-color: inherit;
            width: 82px;
            padding: 0px;
            background-color: #99CCFF;
            font-size: small;
            height: 18px;
        }
        
          .style56
        {
            text-align: center;
            width: 100%;
            background-color:#336699;
    </style>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

  <script language="javascript" type="text/javascript">
        function valid1() {
          var code = "";
          var name = "";
                 
          code = document.getElementById('<%=txt_Sys_Code.ClientID%>').value;
          name = document.getElementById('<%=txt_Sys_Name.ClientID%>').value;
         

          if (code == "") {
              alert("Please enter proper \" Code\" field.");
              document.getElementById("MainContent_txt_Sys_Code").focus();
              return (false);
          }


          if (document.getElementById('<%=txt_Sys_Code.ClientID%>').value.length < 1) {
              alert("Length of \"Code\" should be Min 1 characters.");
              document.getElementById("MainContent_txt_Sys_Code").focus();
              return (false);
          }
          if (document.getElementById('<%=txt_Sys_Code.ClientID%>').value.length > 3) {
              alert("Length of \" Code\" should be Max 3 characters.");
              document.getElementById("MainContent_txt_Sys_Code").focus();
              return (false);
          }
          if (document.getElementById('<%=txt_Sys_Name.ClientID%>').value == "") {
              alert("Please enter proper \" Name\" field.");
              document.getElementById("MainContent_txt_Sys_Name").focus();
              return (false);
          }          
          else {
             
              return (true);
          }
          //return (false);
      }
    </script>
     
   <script type="text/javascript">
       function EngOnlyInput(event) {
           var chCode = ('charCode' in event) ? event.charCode : event.keyCode;
           //if (chCode < 48 /* '0' */ || chCode > 57 /* '9' */) {
           //alert(chCode);
           if (chCode > 64 || chCode == 0 || chCode == 13 || chCode == 32) {
               if (chCode < 91 || chCode == 0 || chCode == 13 || chCode == 32) {
                   return (true);
               }
               else if (chCode > 96 || chCode == 0 || chCode == 13 || chCode == 32) {
                   if (chCode < 123 || chCode == 0 || chCode == 13 || chCode == 32) {
                       return (true);
                   }
                   else {
                       alert("Please Enter ENG Only Characters!");
                       document.getElementById("MainContent_txt_Sys_Marc").focus();
                       return (false);
                   }
               }
               else {
                   alert("Please Enter ENG Only Characters!");
                   document.getElementById("MainContent_txt_Sys_Marc").focus();
                   return (false);
               }
           }
           else {
               alert("Please Enter ENG Only Characters!");
               document.getElementById("MainContent_txt_Sys_Marc").focus();
               return (false);
           }
       }
    </script>

    
         <table id="ADMT1" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
            <tr>
                <td  bgcolor="#003366" class="style43" colspan="2" rowspan="1" style="color: #FFFFFF"><strong>Manage System Data</strong></td>
            </tr>
            
            <tr>                
                <td  align="center" colspan="2">      
                
                    Select System Table:            
                
                <asp:DropDownList ID="DropDownList1" runat="server" ForeColor="#0066FF" 
                        AutoPostBack="True"  >
                        <asp:ListItem Value="BIB_LEVELS" Selected="True">Bibliographic Levels</asp:ListItem>
                        <asp:ListItem Value="MATERIALS">Materials</asp:ListItem>
                        <asp:ListItem Value="DOC_TYPES">Document Types</asp:ListItem>
                        <asp:ListItem Value="ACC_MATERIALS">Accompanying Materials</asp:ListItem>
                        <asp:ListItem Value="ACQMODES">Acquisition Modes</asp:ListItem>
                        <asp:ListItem Value="BINDINGS">Binding Types</asp:ListItem>
                        <asp:ListItem Value="BOOKSTATUS">Copy Status</asp:ListItem>
                        <asp:ListItem Value="COUNTRIES">Countries</asp:ListItem>
                        <asp:ListItem Value="CURRENCIES">Currencies</asp:ListItem>
                        <asp:ListItem Value="PHYSICAL_FORMATS">Physical Formats and Mediums</asp:ListItem>
                        <asp:ListItem Value="LANGUAGES">Languages</asp:ListItem>
                        <asp:ListItem Value="FREQUENCIES">Serial Frequencies</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;and Press ENTER<br />
             </td>
            </tr>
             <tr>                
                <td  align="left" colspan="2" style="text-align: left">      
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode ="Conditional">
                    <ContentTemplate>
               <asp:Label ID="Label2" runat="server" Text="Record(s)"></asp:Label>                             
                        
              <asp:Panel ID="Panel1" runat="server" Height="250px" ScrollBars="Auto">                      

                 <asp:GridView ID="Grid1_SysData" runat="server" AllowPaging="True"  
                        style="width: 100%;  " allowsorting="True"  
                        AutoGenerateColumns="False"  Font-Bold="True" 
                        Font-Names="Tahoma" Font-Size="8pt" Height="100px"  ShowFooter="True"  
                            HorizontalAlign="Left" AutoPostBack="true" PageSize="100"  >
                    
                    <PagerStyle BackColor="#3399FF" BorderColor="#C0C000" BorderStyle="Solid" 
                        Font-Bold="True" ForeColor="White" HorizontalAlign="Center" 
                        VerticalAlign="Middle" Font-Size="Small" />
                    <RowStyle BackColor="#DADADA" BorderColor="Desktop" ForeColor="#3399FF" />
                    <SelectedRowStyle BackColor="Desktop" BorderColor="SteelBlue" BorderStyle="Solid" />
                     <FooterStyle BackColor="#3399FF" />
                    <HeaderStyle BackColor="Desktop" Font-Bold="True" Font-Italic="False" Font-Names="Tahoma"
                        Font-Overline="False" Font-Underline="False" ForeColor="White" Width="80%" 
                            Font-Size="Larger" />
                    <PagerSettings Position="TopAndBottom" FirstPageText="First" LastPageText="Last" PageButtonCount="20" Mode="NumericFirstLast" />
                    <AlternatingRowStyle BackColor="#EFEFEF" Font-Names="Tahoma" ForeColor="#0066FF" />
                </asp:GridView>                  

                 </asp:Panel>

                </ContentTemplate>
                 <Triggers>
                        <asp:AsyncPostBackTrigger  ControlID="DropDownList1" EventName="TextChanged"   />
                        <asp:AsyncPostBackTrigger ControlID="Grid1_SysData" EventName="RowCommand" />                                                                 
                   </Triggers>
                 </asp:UpdatePanel>                 
                </td>
            </tr>
                       
        </table>

        
        
         <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
              <ContentTemplate>

               

         <table id="Table1" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
             <tr>
                <td class="style56" colspan="6" bgcolor="#003366">
                   <asp:Label ID="Label4" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">Select Print Format:</asp:Label>
                    <asp:DropDownList ID="DDL_PrintFormats" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC" ToolTip="Plz Select Value from Drop-Down">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem Selected="True" Value="PDF">Pdf Format</asp:ListItem>
                        <asp:ListItem Value="DOC">Doc Format</asp:ListItem>
                        <asp:ListItem Value="HTML">Html Format</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Button ID="Print_Summary_Bttn" runat="server" CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red" Height="20px" TabIndex="14" 
                        Text="Summary Report" Visible="False" Width="130px" />
                </td>
            </tr>
           
             <tr>
                <td class="style56" colspan="6" bgcolor="#003366">
                   <asp:Label ID="Label3" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style47" colspan="2">
                     <asp:Label ID="Label6" runat="server" Font-Size="Medium" ForeColor="#CC3300" 
                         style="font-weight: 700"></asp:Label>
                 </td>
            </tr>
            
            <tr>
                <td class="style51">Code *</td>
                <td class="style52">
                    <asp:TextBox ID="txt_Sys_Code" runat="server" MaxLength="3"  
                        ToolTip="Enter Distinct Code, Max 3 Chr, Alpha, ENG Only." Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" style="text-transform: uppercase" 
                        Height="20px" Width="96px"></asp:TextBox>
                &nbsp;1-3 Chrs Length, Alpha, ENG Only, Distinct Code.
                    </td>
            </tr>
            <tr>
                <td class="style51">Name*</td>
                <td class="style52">
                    <asp:TextBox ID="txt_Sys_Name" runat="server" MaxLength="100"  
                        ToolTip="Enter Name" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="465px"  
                        AutoCompleteType="DisplayName" Height="22px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style51">Remarks</td>
                <td class="style52">
                    <asp:TextBox ID="txt_Sys_Desc" runat="server" MaxLength="250"  
                        ToolTip="Enter Remarks" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" Width="465px" Height="23px"></asp:TextBox>
                 </td>
            </tr>
             <tr id="MarcTag" runat="server">
                <td class="style51">MARC Code</td>
                <td class="style52">
                    <asp:TextBox ID="txt_Sys_Marc" runat="server" MaxLength="3"  
                        ToolTip="Enter Distinct MARC Code, Max 3 Chr, Alpha, ENG Only." Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF" style="text-transform: uppercase" 
                        Height="20px" Width="96px" onkeypress="return EngOnlyInput (event)"></asp:TextBox>
                &nbsp;3 Chrs Length, Alpha, ENG Only, Distinct Code.
                    </td>
            </tr>
             <tr id ="BibLvlTr" runat ="server">
                <td class="style51">Bibliographic Level</td>
                <td class="style52">
                    
                    <asp:DropDownList ID="DropDownList3" runat="server"  ForeColor="#3399FF" 
                        AutoPostBack="True">
                    </asp:DropDownList>
                    
                 </td>
            </tr>
             <tr  id ="MatCodeTr" runat ="server">
                <td class="style51">Materials Type</td>
                <td class="style52">
                      <asp:DropDownList ID="DropDownList4" runat="server" ForeColor="#3399FF">
                    </asp:DropDownList>
                    
                 </td>
            </tr>
           
            <tr>
                <td class="style47" colspan="2">
                    <asp:Button ID="bttn_Save" runat="server"  CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red"
                        OnClientClick="return valid1();" TabIndex="14" Text="Save" AccessKey="s" 
                        Width="74px" Visible="False" ToolTip="Press to SAVE Record" />
                    <asp:Button ID="bttn_Update" runat="server"  CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red"
                        OnClientClick="return valid1();" TabIndex="14" Text="Update" AccessKey="s" 
                        Width="74px" Visible="False" ToolTip="Press to Update Record" />
                    <asp:Button ID="bttn_Delete" runat="server" AccessKey="d" CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red" TabIndex="14" Text="Delete" Visible="False" 
                        Width="74px" ToolTip="Press to Delete Record" />
                    <asp:Button ID="Cancel" runat="server"  CssClass="styleBttn" Font-Bold="True" ForeColor="Red" 
                        TabIndex="15" Text="Cancel" Width="71px" AccessKey="c" 
                        ToolTip="Press to Cancel the funciton"/>
                </td>
            </tr>
            
            <tr>
                <td  bgcolor="#99CCFF" class="style44" colspan="2" rowspan="0">
                     <strong>*<span class="style48">Mandatory Fields</span></strong></td>
            </tr>
                        

        </table>

                    
                   <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server"                        
                              TargetControlID="bttn_Delete"
                             PopupControlID="ModalPanel1"
                              DropShadow="true"
                                PopupDragHandleControlID="PopupHeader"
                                Drag="true"
                              
 
                             BackgroundCssClass="ModalPopupBG"
            
                          
                            Y="150"
                             RepositionMode="RepositionOnWindowResize">
                    </ajaxToolkit:ModalPopupExtender>



                   </ContentTemplate>
                   <Triggers>
                        <asp:PostBackTrigger  ControlID="bttn_Save"   />    
                        <asp:PostBackTrigger ControlID ="bttn_Update"  />   
                        <asp:PostBackTrigger ControlID ="bttn_Delete"  /> 
                        <asp:PostBackTrigger ControlID ="Cancel"  />  
                        <asp:PostBackTrigger  ControlID="Print_Summary_Bttn" />                               
                   </Triggers>



                   </asp:UpdatePanel>
       
        <table id="Table3" runat="server" cellspacing="2" border="1" class="style35" 
         cellpadding="1"  style="vertical-align: middle;"         width="100%" 
         align="center" >
            <tr>
                <td class="style47" colspan="2">
                     
                 </td>
            </tr>
       </table>


       <asp:Panel ID="ModalPanel1" runat="server" Style="display: none;" CssClass="modalPopup">
      
       <table id="Table2" runat="server" cellspacing="2" border="1"  
         cellpadding="1"  style="vertical-align: middle;"         width="100%" 
         align="center"  >
            <tr>
                <td class="style47" colspan="2">
                     <asp:Label ID="Label1" runat="server" Font-Size="Large">Do You wish to Delete this Record?</asp:Label>
                 </td>
            </tr>
            
            <tr>
                <td class="style53">Code *</td>
                <td class="style52">
                    <asp:Label ID="Label7" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style53">Name*</td>
                <td class="style52">
                    <asp:Label ID="Label8" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style53">Remarks</td>
                <td class="style52">
                    <asp:Label ID="Label9" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
           
            <tr>
                <td class="style47" colspan="2">
                    <asp:Button ID="bttn_Yes" runat="server"  CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red" TabIndex="14" Text="Yes" AccessKey="s" 
                        Width="74px" ToolTip="Press to SAVE Record" />
                    <asp:Button ID="bttn_No" runat="server"  CssClass="styleBttn" Font-Bold="True" ForeColor="Red" 
                        TabIndex="15" Text="No" Width="71px" AccessKey="c" 
                        ToolTip="Press to Cancel the funciton"/>
                </td>
            </tr>
            
            
        </table>

          </asp:Panel> 


    
      
    
</asp:Content>
