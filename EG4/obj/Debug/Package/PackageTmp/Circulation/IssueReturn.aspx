<%@ Page  Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="IssueReturn.aspx.vb" Inherits="EG4.IssueReturn"  %>

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
            height: 35px;
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
        background-color:#336699;
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
            width: 15%;
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
            }
                     
         .style56
        {
            text-align:left;
            background-color:#336699;
                      
        }
           
               
                
        .style57
        {
            text-align: center;
            background-color: #336699;
            height: 17px;
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
            font-style:italic;  
            font-weight:bold;  
            font-family:Arial;
            background-color:Silver;  
            height:25px;    
            }    
                
       
        </style>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

  
      <script type="text/javascript">
          function formfocus() {
              document.getElementById('<%= txt_Cir_MemNo.ClientID %>').focus();
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
     
         <table id="ADMT1" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
            <tr>
                <td  bgcolor="#003366" class="style43" colspan="2" rowspan="1" style="color: #FFFFFF" ><strong>Issue/Reserve/Return/Renew</strong></td>
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
                    <asp:MenuItem ImageUrl="~/Images/Issue_ReserveUP.png" Text="" Value="0" Selected="true"></asp:MenuItem>
                    <asp:MenuItem ImageUrl="~/Images/return_renew_over.png" Text="" Value="1"></asp:MenuItem>
                </Items>
        
            </asp:Menu>
             </ContentTemplate>  
        </asp:UpdatePanel>
    </div>
 
 
 
 <div class="style4">
        
  <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
              <ContentTemplate>

   <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">

    <asp:View ID="Tab1" runat="server">
   
     <table id="Table2" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
           <tr>
                <td class="style57" colspan="3">
                    <asp:Label ID="Label12" runat="server" Font-Bold="True" Font-Size="Medium" 
                        ForeColor="White">Issue and Reserve Documents</asp:Label>
                 </td>
            </tr>
             <tr>
                <td class="style57" colspan="3">
                   <asp:Label ID="Lbl_Error" runat="server" Font-Bold="True" Font-Size="Medium" 
                        ForeColor="Yellow"></asp:Label>
                 </td>
            </tr>
             <tr>
                <td class="style56" colspan="3">
                   <asp:Label ID="Label6" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True" Text="STEP 1: Display Member Record: "></asp:Label>
                    &nbsp;</td>
            </tr>
            
            <tr>
                <td class="style51"> 
                    <asp:Label ID="Label7" runat="server" style="font-weight: 700; color: #FF3300;" 
                        Text="Member No *"></asp:Label>
                </td>
                <td class="style54" colspan="2">
                    <asp:TextBox ID="txt_Cir_MemNo" runat="server" MaxLength="15" 
                        ToolTip="Enter Member No" Wrap="False"  Font-Bold="True" ForeColor="#0066FF" 
                        Height="18px" Width="90px" style="text-transform: uppercase" 
                        AutoPostBack="True"></asp:TextBox>
                    &nbsp;OR Select Member Name:&nbsp;<asp:DropDownList ID="DDL_Members" 
                        runat="server" Enabled="True" 
                        Font-Bold="True" ForeColor="#0066CC" 
                        ToolTip="Plz Select Value from Drop-Down" AutoPostBack="True">
                    </asp:DropDownList>
                    &nbsp;<asp:Label ID="Label16" runat="server" Font-Bold="True" 
                        Font-Size="Smaller"></asp:Label>
                </td>
               
            </tr>
            <tr>
                <td class="style51">Mem.Category</td>
                <td class="style53">
                    <asp:DropDownList ID="DDL_Categories" runat="server" 
                        Font-Bold="True" ForeColor="#0066CC" Enabled="False">
                    </asp:DropDownList>
                    &nbsp;Member Sub Category:
                    <asp:DropDownList ID="DDL_SubCategories" runat="server" 
                        Font-Bold="True" ForeColor="#0066CC" Enabled="False">
                    </asp:DropDownList>
                    &nbsp; Status:
                    <asp:DropDownList ID="DDL_MemStatus" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC" TabIndex="200" Enabled="False">
                        <asp:ListItem Selected="True" Value="CU">Current</asp:ListItem>
                        <asp:ListItem Value="CN">Cancel</asp:ListItem>
                        <asp:ListItem Value="CL">Closed</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td rowspan="3" width="10px">
                    <asp:Image ID="Image2" runat="server" Height="50px" Width="40px" />
                </td>
            </tr>
            <tr>
                <td class="style51">Entitlement</td>
                <td class="style52">
                     <strong>
                     <asp:TextBox ID="txt_Cir_Entitlement" runat="server" 
                         AutoCompleteType="DisplayName" 
                         Font-Bold="True" ForeColor="#0066FF" Height="16px" MaxLength="4" 
                         onkeypress="return isNumberKey(event)" TabIndex="104" 
                         ToolTip="Enter No of Documents to Be Issued" Width="50px" Wrap="False" 
                         Enabled="False"></asp:TextBox>
                     &nbsp;Due Days:
                     <asp:TextBox ID="txt_Cir_DueDays" runat="server" AutoCompleteType="DisplayName" 
                         Font-Bold="True" ForeColor="#0066FF" 
                         Height="16px" MaxLength="4" onkeypress="return isNumberKey(event)" 
                         ToolTip="Enter No of Days Documetns will be Issued" Width="50px" 
                         Wrap="False" Enabled="False"></asp:TextBox>
                     &nbsp;Already Issued:
                     <asp:TextBox ID="txt_Cir_Issued" runat="server" AutoCompleteType="DisplayName" 
                         Font-Bold="True" ForeColor="#0066FF" Height="16px" MaxLength="10" 
                         onkeypress="return isNumberKey(event)" Width="50px" Wrap="False" 
                         Enabled="False"></asp:TextBox>
                     &nbsp;Over-Ride?
                     <asp:TextBox ID="txt_Cir_OverRide" runat="server" 
                         AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                         Height="16px" MaxLength="2" onkeypress="return isNumberKey(event)" Width="15px" 
                         Wrap="False" Enabled="False"></asp:TextBox>
                     &nbsp;Mobile No:
                     <asp:TextBox ID="txt_Cir_Mobile" runat="server" AutoCompleteType="DisplayName" 
                         Font-Bold="True" ForeColor="#0066FF" Height="16px" MaxLength="12" 
                         onkeypress="return isNumberKey(event)" TabIndex="104" Width="80px" 
                         Wrap="False" Enabled="False"></asp:TextBox>
                     </strong></td>
            </tr>
            <tr>
                <td class="style51">Email</td>
                <td class="style52">
                     <strong>
                     <asp:TextBox ID="txt_Cir_MemEmail" runat="server" 
                         AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                         Height="16px" MaxLength="100" onkeypress="return isNumberKey(event)" 
                         TabIndex="104" ToolTip="Enter No of Documents to Be Issued" Width="30%" 
                         Wrap="False" Enabled="False"></asp:TextBox>
                     &nbsp;Admission Date:
                     <asp:TextBox ID="txt_Cir_AdmDate" runat="server" AutoCompleteType="DisplayName" 
                         Font-Bold="True" ForeColor="#0066FF" Height="16px" MaxLength="12" 
                         onkeypress="return isNumberKey(event)" TabIndex="104" Width="80px" 
                         Wrap="False" Enabled="False"></asp:TextBox>
                     &nbsp;Closing Date:
                     <asp:TextBox ID="txt_Cir_ClosingDate" runat="server" 
                         AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                         Height="16px" MaxLength="12" onkeypress="return isNumberKey(event)" 
                         TabIndex="104" Width="80px" Wrap="False" Enabled="False"></asp:TextBox>
                     </strong></td>
            </tr>
          
        </table>



         <table id="Table1" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
            
             <tr>
                <td class="style56" colspan="2">
                   <asp:Label ID="Label2" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True" Text="STEP 2: Select Category of Documents & Display Doc Record: "></asp:Label>
                    <asp:RadioButton ID="RadioButton1" runat="server" AutoPostBack="True" 
                        Checked="True" Font-Bold="True" ForeColor="Yellow" GroupName="DocType" 
                        Text="Books and Bound Journals" />
                    &nbsp;<asp:RadioButton ID="RadioButton2" runat="server" AutoPostBack="True" 
                        Font-Bold="True" ForeColor="Yellow" GroupName="DocType" Text="Loose Issues" />
                 </td>
            </tr>
            
            <tr>
                <td class="style51"> 
                    <asp:Label ID="Label8" runat="server" style="font-weight: 700; color: #FF3300;" 
                        Text="Acc.No *"></asp:Label>
                </td>
                <td class="style54">
                    <asp:TextBox ID="txt_Cir_AccessionNo" runat="server" MaxLength="15" 
                        ToolTip="Enter Accession No / Item ID" Wrap="False"  Font-Bold="True" ForeColor="#0066FF" 
                        Height="18px" Width="80px" style="text-transform: uppercase" 
                        AutoPostBack="True"></asp:TextBox>
                    &nbsp;
                    <asp:Label ID="Label15" runat="server" Font-Bold="True" Font-Size="Smaller"></asp:Label>
                    &nbsp;Bibliographic Level:
                    <asp:DropDownList ID="DDL_Bib_Level" runat="server" Enabled="False" 
                        Font-Bold="True" ForeColor="#0066CC">
                    </asp:DropDownList>
                </td>
               
            </tr>
            <tr>
                <td class="style51">Material:</td>
                <td class="style53">
                    <asp:DropDownList ID="DDL_Mat_Type" runat="server" Enabled="False" 
                        Font-Bold="True" ForeColor="#0066CC" ToolTip="Plz Select Value from Drop-Down">
                    </asp:DropDownList>
                    &nbsp;
                    <asp:DropDownList ID="DDL_Doc_Type" runat="server" Enabled="False" 
                        Font-Bold="True" ForeColor="#0066CC">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="style51">Title Details</td>
                <td class="style53">
                    <asp:Label ID="Label10" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                </td>
                <td rowspan="3" width="10px">
                    <asp:Image ID="Image1" runat="server" Height="50px" Width="40px" />
                </td>
            </tr>
            <tr>
                <td class="style51">ISBN</td>
                <td class="style52">
                     <asp:Label ID="Label11" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style51">Collection Type</td>
                <td class="style52">
                     <asp:DropDownList ID="DDL_CollectionType" runat="server" Enabled="False" 
                         Font-Bold="True" ForeColor="#0066FF">
                         <asp:ListItem Selected="True" Value="C">Circulation</asp:ListItem>
                         <asp:ListItem Value="R">Reference</asp:ListItem>
                         <asp:ListItem Value="G">Book Bank (General)</asp:ListItem>
                         <asp:ListItem Value="S">Book Bank (SCST)</asp:ListItem>
                         <asp:ListItem Value="N">Non-Returnable</asp:ListItem>
                     </asp:DropDownList>
                     &nbsp;Current Status:
                     <asp:DropDownList ID="DDL_Status" runat="server" Enabled="False" 
                         Font-Bold="True" ForeColor="#0066FF">
                     </asp:DropDownList>
                     <asp:Label ID="Label13" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                     <asp:Label ID="Label14" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                </td>
            </tr>
                   
           
        </table>

          <table id="Table3" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
           
             <tr>
                <td class="style56" colspan="11">
                   <asp:Label ID="Label3" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True" Text="STEP 3: Circulation Data"></asp:Label>
                    &nbsp;</td>
            </tr>
            
            <tr>
                <td class="style51"> 
                    Issue Date</td>
                <td class="style54">
                    <strong>
                    <asp:TextBox ID="txt_Cir_IssueDate" runat="server" 
                        AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="12" onkeypress="return isNumberKey(event)" 
                        TabIndex="104" Width="80px" Wrap="False" Enabled="False"></asp:TextBox>
                    </strong></td>
               
                <td class="style54">
                    Issue Time</td>
                <td class="style54">
                    <strong>
                    <asp:TextBox ID="txt_Cir_IssueTime" runat="server" 
                        AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="12" onkeypress="return isNumberKey(event)" 
                        TabIndex="104" Width="80px" Wrap="False" Enabled="False"></asp:TextBox>
                    </strong>
                </td>
                <td class="style54">
                    Due Date</td>
                <td class="style54" colspan="2">
                    <strong>
                    <asp:TextBox ID="txt_Cir_DueDate" runat="server" 
                        AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="12" onkeypress="return isNumberKey(event)" 
                        TabIndex="104" Width="80px" Wrap="False" Enabled="False"></asp:TextBox>
                    </strong>
                </td>
                <td class="style54">
                    Reserve Date</td>
                <td class="style54">
                    <strong>
                    <asp:TextBox ID="txt_Cir_ReserveDate" runat="server" 
                        AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="12" onkeypress="return isNumberKey(event)" 
                        TabIndex="104" Width="80px" Wrap="False" Enabled="False"></asp:TextBox>
                    </strong>
                </td>
                <td class="style54">
                    Reserve Time</td>
                <td class="style54">
                    <strong>
                    <asp:TextBox ID="txt_Cir_ReserveTime" runat="server" 
                        AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="12" onkeypress="return isNumberKey(event)" 
                        TabIndex="104" Width="80px" Wrap="False" Enabled="False"></asp:TextBox>
                    </strong>
                </td>
               
            </tr>

            <tr>
                <td class="style51">Received by</td>
                <td class="style52" colspan="5">
                     <strong>
                     <asp:TextBox ID="txt_Cir_ReceivedBy" runat="server" AutoPostBack="True" 
                         Font-Bold="True" ForeColor="#0066FF" Height="18px" MaxLength="100" 
                          Width="98%" Wrap="False"></asp:TextBox>
                     </strong></td>
                <td class="style52" colspan="5">
                    &nbsp;<asp:CheckBox ID="CheckBox1" runat="server" Text="Send Mail" />
                    &nbsp;<asp:CheckBox ID="CheckBox2" runat="server" Text="Send SMS" />
                    &nbsp;<asp:CheckBox ID="CheckBox3" runat="server" Text="Generate Gate Pass" />
                </td>
            </tr>
             <tr>
                <td class="style51">Remarks</td>
                <td class="style52" colspan="10">
                  
                     <asp:TextBox ID="txt_Cir_Remarks" runat="server" AutoPostBack="True" 
                         Font-Bold="True" ForeColor="#0066FF" Height="18px" MaxLength="100" 
                         Width="98%" Wrap="False"></asp:TextBox>
                    </td>
            </tr>
           
            
            <tr>
                <td class="style47" colspan="11">
                    
                   
                    <asp:Button ID="Cir_Issue_Bttn" runat="server" AccessKey="i" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" TabIndex="14" 
                        Text="Issue" ToolTip="Press to Issue Document" 
                        Width="74px" />
                    <asp:Button ID="Cir_Reserve_Bttn" runat="server" AccessKey="r" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" TabIndex="14" 
                        Text="Reserve" ToolTip="Press to Reserve the Document" Width="74px" />
                    <asp:Button ID="Cir_Cancel_Bttn" runat="server" AccessKey="c" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" TabIndex="15" 
                        Text="Cancel" ToolTip="Press to Cancel the process" Width="71px" />
                    
                   
                </td>
            </tr>
            
            <tr>
                <td  bgcolor="#99CCFF" class="style44" colspan="11">
                     <strong>*<span class="style48"> Mandatory Fields</span></strong></td>
            </tr>

             <tr>
                <td  bgcolor="#99CCFF" class="style47" colspan="11">
                
                  <asp:Panel ID="Panel2" runat="server" Height="250px" ScrollBars="Auto">
                   <asp:GridView ID="Grid1" runat="server" AllowPaging="True" DataKeyNames="CIR_ID"  
                        style="width: 100%;  text-align: center;" allowsorting="True" 
                        AutoGenerateColumns="False" PageSize="100"  Font-Bold="True" 
                        Font-Names="Tahoma" Font-Size="8pt" Height="100px"  
                        HorizontalAlign="Center"  Width="6px" ShowFooter="True">
                        <Columns >                
                            <asp:TemplateField HeaderText="S.N."><ItemTemplate><asp:Label ID="lblsr"  runat="server" CssClass="MBody"   Text = '<%# Container.dataitemindex+1 %>' SkinID="" width="25px"></asp:Label></ItemTemplate><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" /><ItemStyle ForeColor="#336699" Font-Names="Tahoma" Width="20px" Font-Bold="True" Font-Size="Small" /></asp:TemplateField>
                    
                            <asp:BoundField   DataField="ACCESSION" HeaderText="Accession No" SortExpression="ACCESSION" ><HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" /><ItemStyle forecolor="#0066FF" horizontalalign="Left" width="80px"   Font-Names="Arial"/></asp:BoundField>
                            <asp:BoundField   DataField="TITLE" HeaderText="Title" SortExpression="TITLE" ReadOnly="True"><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" /><ItemStyle forecolor="#0066FF" horizontalalign="Left" Font-Names="Arial"  width="350px" /></asp:BoundField>
                            <asp:BoundField   DataField="ISSUE_DATE" HeaderText="Issue Date" SortExpression="ISSUE_DATE" ReadOnly="True"><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" /><ItemStyle forecolor="#0066FF" horizontalalign="Left" Font-Names="Arial"  width="100px" /></asp:BoundField>
                            <asp:BoundField   DataField="DUE_DATE" HeaderText="Due Date" SortExpression="DUE_DATE" ReadOnly="True"><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" /><ItemStyle forecolor="#0066FF" horizontalalign="Left" Font-Names="Arial"  width="100px" /></asp:BoundField>
                            <asp:BoundField   DataField="RESERVE_DATE" HeaderText="Reserve Date" SortExpression="RESERVE_DATE" ReadOnly="True"><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" /><ItemStyle forecolor="#0066FF" horizontalalign="Left" Font-Names="Arial"  width="100px" /></asp:BoundField>
                            <asp:BoundField   DataField="STATUS" HeaderText="Status" SortExpression="STATUS" ReadOnly="True"><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" /><ItemStyle forecolor="#0066FF" horizontalalign="Left" Font-Names="Arial"  width="100px" /></asp:BoundField>
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
        
         </asp:View>
       <asp:View ID="Tab2" runat="server">

                <table id="Table4" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
             <tr>
                <td class="style57" colspan="3">
                    <asp:Label ID="Label20" runat="server" Font-Bold="True" Font-Size="Medium" 
                        ForeColor="White">Return and Renew Documents</asp:Label>
                 </td>
            </tr>
               <tr>
                <td class="style57" colspan="3">
                   <asp:Label ID="Lbl_Error2" runat="server" Font-Bold="True" Font-Size="Medium" 
                        ForeColor="Yellow"></asp:Label>
                 </td>
            </tr>
             <tr>
                <td class="style56" colspan="3">
                   <asp:Label ID="Label21" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True" Text="STEP 1: Select Category of Documents & Display Doc Record: "></asp:Label>
                    <asp:RadioButton ID="RadioButton3" runat="server" AutoPostBack="True" 
                        Checked="True" Font-Bold="True" ForeColor="Yellow" GroupName="DocType" 
                        Text="Books and Bound Journals" />
                    &nbsp;<asp:RadioButton ID="RadioButton4" runat="server" AutoPostBack="True" 
                        Font-Bold="True" ForeColor="Yellow" GroupName="DocType" Text="Loose Issues" />
                 </td>
            </tr>
            
            
            <tr>
                <td class="style51"> 
                    <asp:Label ID="Label4" runat="server" style="font-weight: 700; color: #FF3300;" 
                        Text="Acc.No *"></asp:Label>
                </td>
                <td class="style54" colspan="2">
                    <asp:TextBox ID="txt_Ret_AccessionNo" runat="server" MaxLength="15" 
                        ToolTip="Enter Accession No / Item ID" Wrap="False"  Font-Bold="True" ForeColor="#0066FF" 
                        Height="18px" Width="90px" style="text-transform: uppercase" 
                        AutoPostBack="True"></asp:TextBox>
                    &nbsp;Bib Level:
                    <asp:DropDownList ID="DDL_BibLevel" runat="server" Enabled="False" 
                        Font-Bold="True" ForeColor="#0066CC">
                    </asp:DropDownList>
                    &nbsp;
                    <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Size="Smaller"></asp:Label>
                </td>
               
            </tr>
            <tr>
                <td class="style51">Material</td>
                <td class="style53">
                    <asp:DropDownList ID="DDL_Materials" runat="server" Enabled="False" 
                        Font-Bold="True" ForeColor="#0066CC">
                    </asp:DropDownList>
                    &nbsp;Do. Type:<asp:DropDownList ID="DDL_Documents" runat="server" Enabled="False" 
                        Font-Bold="True" ForeColor="#0066CC">
                    </asp:DropDownList>
                </td>
                <td rowspan="3" width="10px">
                    <asp:Image ID="Image3" runat="server" Height="50px" Width="40px" />
                </td>
            </tr>
            <tr>
                <td class="style51">Title Details</td>
                <td class="style53">
                    <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style51">ISBN</td>
                <td class="style52">
                     <asp:Label ID="Label17" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style51">Collection</td>
                <td class="style52">
                     <asp:DropDownList ID="DDL_Collections" runat="server" Enabled="False" 
                         Font-Bold="True" ForeColor="#0066FF">
                         <asp:ListItem Selected="True" Value="C">Circulation</asp:ListItem>
                         <asp:ListItem Value="R">Reference</asp:ListItem>
                         <asp:ListItem Value="G">Book Bank (General)</asp:ListItem>
                         <asp:ListItem Value="S">Book Bank (SCST)</asp:ListItem>
                         <asp:ListItem Value="N">Non-Returnable</asp:ListItem>
                     </asp:DropDownList>
                     &nbsp;Current Status:
                     <asp:DropDownList ID="DDL_CurrentStatus" runat="server" Enabled="False" 
                         Font-Bold="True" ForeColor="#0066FF">
                     </asp:DropDownList>
                     <asp:Label ID="Label18" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                     <asp:Label ID="Label19" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                     <asp:Label ID="Label28" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                </td>
            </tr>
                   
           
        </table>
       
        <table id="Table5" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
           
             <tr>
                <td class="style56" colspan="11">
                   <asp:Label ID="Label1" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True" Text="Circulation Data"></asp:Label>
                    &nbsp;</td>
            </tr>
            
            <tr>
                <td class="style51"> 
                    Issue Date</td>
                <td class="style54">
                    <strong>
                    <asp:TextBox ID="txt_Ret_IssueDate" runat="server" 
                        AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="12"  
                        TabIndex="104" Width="80px" Wrap="False" Enabled="False"></asp:TextBox>
                    </strong></td>
               
                <td class="style54">
                    Issue Time</td>
                <td class="style54">
                    <strong>
                    <asp:TextBox ID="txt_Ret_IssueTime" runat="server" 
                        AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="12" TabIndex="104" Width="80px" Wrap="False" Enabled="False"></asp:TextBox>
                    </strong>
                </td>
                <td class="style54">
                    Due Date</td>
                <td class="style54" colspan="2">
                    <strong>
                    <asp:TextBox ID="txt_Ret_DueDate" runat="server" 
                        AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="12" TabIndex="104" Width="80px" Wrap="False" Enabled="False"></asp:TextBox>
                    </strong>
                </td>
                <td class="style54">
                    Return Date</td>
                <td class="style54">
                    <strong>
                    <asp:TextBox ID="txt_Ret_ReturnDate" runat="server" 
                        AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="12" TabIndex="104" Width="80px" Wrap="False" Enabled="False"></asp:TextBox>
                    </strong>
                </td>
                <td class="style54">
                    Return Time</td>
                <td class="style54">
                    <strong>
                    <asp:TextBox ID="txt_Ret_ReturnTime" runat="server" 
                        AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="12" TabIndex="104" Width="80px" Wrap="False" Enabled="False"></asp:TextBox>
                    </strong>
                </td>
               
            </tr>
             <tr>
                <td class="style51"> 
                    Renew Date</td>
                <td class="style54">
                    <strong>
                    <asp:TextBox ID="txt_Ret_RenewDate" runat="server" 
                        AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="12" TabIndex="104" Width="80px" Wrap="False" Enabled="False"></asp:TextBox>
                    </strong></td>
               
                <td class="style54">
                    Fine Due (Rs)</td>
                <td class="style54">
                    <strong>
                    <asp:TextBox ID="txt_Ret_FineDue" runat="server" 
                        AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="12" TabIndex="104" Width="80px" Wrap="False" Enabled="False"></asp:TextBox>
                    </strong>
                </td>
                <td class="style54">
                    Fine Collected (Rs)</td>
                <td class="style54" colspan="2">
                    <strong>
                    <asp:TextBox ID="txt_Ret_FineCollected" runat="server" 
                        AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                        Height="16px" MaxLength="12" onkeypress="return isCurrencyKey(event)" 
                        TabIndex="104" Width="80px" Wrap="False" Enabled="true"></asp:TextBox>
                    </strong>
                </td>
                <td class="style54">
                    Exempt Fine?</td>
                <td class="style54">
                    <asp:CheckBox ID="CheckBox7" runat="server" Text="Exempt?" />
                </td>
                <td class="style54">
                    <asp:Label ID="Label27" runat="server" Font-Bold="True" Font-Size="Smaller"></asp:Label>
                    </td>
                <td class="style54">
                    &nbsp;</td>
               
            </tr>

            <tr>
                <td class="style51">Remarks</td>
                <td class="style52" colspan="5">
                     <asp:TextBox ID="txt_Ret_Remarks" runat="server" AutoPostBack="True" Font-Bold="True" 
                         ForeColor="#0066FF" Height="16px" MaxLength="100" 
                         Width="98%" Wrap="False"></asp:TextBox>
                </td>
                <td class="style52" colspan="5">
                    <asp:CheckBox ID="CheckBox4" runat="server" Text="Send Mail" />
                    <asp:CheckBox ID="CheckBox5" runat="server" Text="Send SMS" />
                    <asp:CheckBox ID="CheckBox6" runat="server" Text="Generate Fine Receipt" />
                </td>
            </tr>
                      
            
           


        </table>
         <table id="Table6" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
          
             <tr>
                <td class="style56" colspan="5">
                   <asp:Label ID="Label24" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True" Text="Member Record: "></asp:Label>
                    &nbsp;</td>
            </tr>
            
            <tr>
                <td class="style51"> 
                    Member No</td>
                <td class="style54">
                    <asp:TextBox ID="txt_Ret_MemNo" runat="server" MaxLength="15" 
                        Wrap="False"  Font-Bold="True" ForeColor="#0066FF" 
                        Height="18px" Width="90px" style="text-transform: uppercase" 
                        AutoPostBack="True" Enabled="False"></asp:TextBox>
                    &nbsp;Member Name:
                    <asp:TextBox ID="txt_Ret_MemName" runat="server" Font-Bold="True"  Enabled="false"
                        ForeColor="#0066FF" Height="18px" MaxLength="15" Width="60%" Wrap="False"></asp:TextBox>
                </td>
               
                <td class="style54" colspan="2">
                    &nbsp;</td>
                <td class="style54">
                    <asp:Label ID="Label26" runat="server" Font-Bold="True" Font-Size="Smaller"></asp:Label>
                </td>
               
            </tr>
            <tr>
                <td class="style51">Category</td>
                <td class="style53" colspan="2">
                    <asp:DropDownList ID="DDL_MemberCategories" runat="server" 
                        Font-Bold="True" ForeColor="#0066CC" Enabled="False">
                    </asp:DropDownList>
                    &nbsp;Member Sub Category:
                    <asp:DropDownList ID="DDL_MemberSubCategories" runat="server" 
                        Font-Bold="True" ForeColor="#0066CC" Enabled="False">
                    </asp:DropDownList>
                    &nbsp; 
                </td>
                <td rowspan="3" width="10px" colspan="2">
                    <asp:Image ID="Image4" runat="server" Height="50px" Width="40px" />
                </td>
            </tr>
            <tr>
                <td class="style51">Status</td>
                <td class="style53" colspan="2">
                    <asp:DropDownList ID="DDL_MemberStatus" runat="server" Enabled="False" 
                        Font-Bold="True" ForeColor="#0066CC" TabIndex="200">
                        <asp:ListItem Selected="True" Value="CU">Current</asp:ListItem>
                        <asp:ListItem Value="CN">Cancel</asp:ListItem>
                        <asp:ListItem Value="CL">Closed</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;Fine:
                    <asp:DropDownList ID="DDL_FineSystem" runat="server" AutoPostBack="True" 
                        Enabled="False" Font-Bold="True" ForeColor="#0066CC" TabIndex="101" 
                        ToolTip="Select Fine System">
                        <asp:ListItem Selected="True" Value="F">Flat Fine Rate</asp:ListItem>
                        <asp:ListItem Value="V">Variable Fine Rate</asp:ListItem>
                        <asp:ListItem Value="N">No Fine</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;Mobile No: <strong>
                    <asp:TextBox ID="txt_Ret_Mobile" runat="server" AutoCompleteType="DisplayName" 
                        Enabled="False" Font-Bold="True" ForeColor="#0066FF" Height="16px" 
                        MaxLength="12" TabIndex="104" Width="80px" Wrap="False"></asp:TextBox>
                    </strong>
                </td>
            </tr>
            <tr>
                <td class="style51">Due Days</td>
                <td class="style52" colspan="2">
                     <strong>
                     <asp:TextBox ID="txt_Ret_DueDays" runat="server" AutoCompleteType="DisplayName" 
                         Font-Bold="True" ForeColor="#0066FF" 
                         Height="16px" MaxLength="4" Enabled="false" 
                         Width="50px" 
                         Wrap="False"></asp:TextBox>
                     &nbsp;First
                     <asp:TextBox ID="txt_Ret_Gap1" runat="server" AutoCompleteType="DisplayName" 
                         BorderColor="#0066FF" BorderStyle="Solid" Font-Bold="True" ForeColor="#0066FF" 
                         Height="16px" MaxLength="3"  
                         TabIndex="103"  Enabled="false"  Width="5%" Wrap="False"></asp:TextBox>
                     &nbsp;Days Fine Per Day:
                     <asp:TextBox ID="txt_Ret_Fine1" runat="server" AutoCompleteType="DisplayName" Enabled="false"
                         BorderColor="#0066FF" BorderStyle="Solid" Font-Bold="True" ForeColor="#0066FF" 
                         Height="16px" MaxLength="6" Width="50px" 
                         Wrap="False"></asp:TextBox>
                     &nbsp;Rest of Days Fine Per Day:
                     <asp:TextBox ID="txt_Ret_Fine2" runat="server" Enabled="false"
                         AutoCompleteType="DisplayName" BorderColor="#0066FF" BorderStyle="Solid" 
                         Font-Bold="True" ForeColor="#0066FF" Height="16px" MaxLength="6" 
                         Width="50px" 
                         Wrap="False"></asp:TextBox>
                     </strong></td>
            </tr>
            <tr>
                <td class="style51">Email</td>
                <td class="style52" colspan="2">
                     <strong>
                     <asp:TextBox ID="txt_Ret_MemEmail" runat="server" 
                         AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                         Height="16px" MaxLength="100" onkeypress="return isNumberKey(event)" 
                         TabIndex="104" ToolTip="Enter No of Documents to Be Issued" Width="40%" 
                         Wrap="False" Enabled="False"></asp:TextBox>
                     &nbsp;Adm.Date:
                     <asp:TextBox ID="txt_Ret_AdmDate" runat="server" AutoCompleteType="DisplayName" 
                         Font-Bold="True" ForeColor="#0066FF" Height="16px" MaxLength="12" 
                         TabIndex="104" Width="80px" 
                         Wrap="False" Enabled="False"></asp:TextBox>
                     &nbsp;Closing Date:
                     <asp:TextBox ID="txt_Ret_ClosingDate" runat="server" 
                         AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                         Height="16px" MaxLength="12"  
                         TabIndex="104" Width="80px" Wrap="False" Enabled="False"></asp:TextBox>
                    
                     </strong></td>
            </tr>
          
           <tr>
                <td class="style47" colspan="5">
                    
                   
                    <asp:Button ID="Cir_Return_Bttn" runat="server" AccessKey="r" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" TabIndex="14" 
                        Text="Return" ToolTip="Press to Return Document" 
                        Width="74px" />
                    <asp:Button ID="Cir_Renew_Bttn" runat="server" AccessKey="r" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" TabIndex="14" 
                        Text="Renew" ToolTip="Press to Renew the Document" Width="74px" />
                    <asp:Button ID="Ret_Cancel_Bttn" runat="server" AccessKey="c" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" TabIndex="15" 
                        Text="Cancel" ToolTip="Press to Cancel the process" Width="71px" />
                    
                   
                </td>
            </tr>
            
            <tr>
                <td  bgcolor="#99CCFF" class="style44" colspan="5">
                     <strong>*<span class="style48"> Mandatory Fields</span></strong></td>
            </tr>
        </table>


      

         </asp:View>
         </asp:MultiView>


       </ContentTemplate>  
            </asp:UpdatePanel>
   
               
        </div>
</asp:Content>
