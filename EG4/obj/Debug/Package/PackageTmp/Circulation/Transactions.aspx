<%@ Page  Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Transactions.aspx.vb" Inherits="EG4.Transactions"  %>

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
            width: 98%;
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
            width: 100%;
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
         width: 98%;

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
                  
        .styleBttn
            {
            cursor:pointer;
            margin-left: 0px;
            font-size: small;
           }
               
                
        .style51
        {
            text-align: justify;
            border-style: none;
            border-color: inherit;
            width: 15%;
            padding: 0px;
            background-color: #99CCFF;
            font-size:x-small;
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
            text-align:center;
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
            font-style:italic;  
            font-weight:bold;  
            font-family:Arial;
            background-color:Silver;  
            height:25px;    
        }    
                
       
        </style>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

     

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
        //alpha-numeric only
        function DateOnly(event) {
            var chCode = ('charCode' in event) ? event.charCode : event.keyCode;
            if (47 <= chCode && chCode <= 57) {
                return (true);
            }

            else {
                alert("Date Is Invalid, Enter in dd/MM/yyyy format only!");
                document.getElementById("MainContent_txt_Tra_SDate").focus();
                return (false);
            }
        }
    </script>
    <script type ="text/javascript">
        //alpha-numeric only
        function DateOnly2(event) {
            var chCode = ('charCode' in event) ? event.charCode : event.keyCode;
            if (47 <= chCode && chCode <= 57) {
                return (true);
            }

            else {
                alert("Date Is Invalid, Enter in dd/MM/yyyy format only!");
                document.getElementById("MainContent_txt_Tra_EDate").focus();
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
   <div class="style4">
     
         <table id="ADMT1" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
            <tr>
                <td  bgcolor="#003366" class="style43" colspan="2" rowspan="1" style="color: #FFFFFF" ><strong>Circulation Transactions</strong></td>
            </tr>
                    
        </table>


 
 
        
       



         <table id="Table1" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
            
             <tr>
                <td class="style56" colspan="6">
                   <asp:Label ID="Label2" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True" Text="STEP 1: Select Category of Documents"></asp:Label>
                    <asp:RadioButton ID="RadioButton1" runat="server" AutoPostBack="True" 
                        Checked="True" Font-Bold="True" ForeColor="Yellow" GroupName="DocType" 
                        Text="Books and Bound Journals" />
                    &nbsp;<asp:RadioButton ID="RadioButton2" runat="server" AutoPostBack="True" 
                        Font-Bold="True" ForeColor="Yellow" GroupName="DocType" Text="Loose Issues" />
                 </td>
            </tr>

             <tr>
                <td class="style56" colspan="6">
                    <asp:Label ID="Lbl_Error" runat="server" 
                        style="font-weight: 700; font-size: small; color: #FFFF00"></asp:Label>
                 </td>
            </tr>
           
            
            <tr>
                <td class="style51"> 
                    <asp:Label ID="Label8" runat="server"  Text="Member No" 
                        style="font-weight: 700"></asp:Label>
                </td>
                <td class="style54">
                    <asp:DropDownList ID="DDL_MemberNo" 
                        runat="server" Enabled="True" 
                        Font-Bold="True" ForeColor="#0066CC" 
                        ToolTip="Plz Select Value from Drop-Down" AutoPostBack="True">
                    </asp:DropDownList>
                     <ajaxToolkit:ListSearchExtender 
                        ID="DDL_MemberNo_ListSearchExtender" 
                        runat="server" 
                        Enabled="True" 
                        PromptCssClass="PromptCSS" 
                        TargetControlID="DDL_MemberNo">
                    </ajaxToolkit:ListSearchExtender>
                 </td>
               
                <td class="style54">
                    <asp:Label ID="Label9" runat="server"  Text="Member Name" 
                        style="font-weight: 700"></asp:Label>
                </td>
               
                <td class="style54">
                    <asp:DropDownList ID="DDL_MemberName" 
                        runat="server" Enabled="True" 
                        Font-Bold="True" ForeColor="#0066CC" 
                        ToolTip="Plz Select Value from Drop-Down" AutoPostBack="True">
                    </asp:DropDownList>
                    <ajaxToolkit:ListSearchExtender 
                        ID="DDL_MemberName_ListSearchExtender" 
                        runat="server" 
                        Enabled="True" 
                        PromptCssClass="PromptCSS" 
                        TargetControlID="DDL_MemberName">
                    </ajaxToolkit:ListSearchExtender>
                    </td>
               
                <td class="style54">
                    <asp:Label ID="Label10" runat="server"  Text="Categories" 
                        style="font-weight: 700"></asp:Label>
                </td>
               
                <td class="style54">
                    <asp:DropDownList ID="DDL_MemberCategories" 
                        runat="server" Enabled="True" 
                        Font-Bold="True" ForeColor="#0066CC" 
                        ToolTip="Plz Select Value from Drop-Down" AutoPostBack="True">
                    </asp:DropDownList>
                    <ajaxToolkit:ListSearchExtender 
                        ID="DDL_MemberCategories_ListSearchExtender" 
                        runat="server" 
                        Enabled="True" 
                        PromptCssClass="PromptCSS" 
                        TargetControlID="DDL_MemberCategories">
                    </ajaxToolkit:ListSearchExtender>
                 </td>
               
            </tr>

             <tr>
                <td class="style51"> 
                    <asp:Label ID="Label6" runat="server"  Text="Sub Category" 
                        style="font-weight: 700"></asp:Label>
                </td>
                <td class="style54">
                    <asp:DropDownList ID="DDL_MemberSubCategories" 
                        runat="server" Enabled="True" 
                        Font-Bold="True" ForeColor="#0066CC" 
                        ToolTip="Plz Select Value from Drop-Down" AutoPostBack="True">
                    </asp:DropDownList>
                     <ajaxToolkit:ListSearchExtender 
                        ID="DDL_MemberSubCategories_ListSearchExtender" 
                        runat="server" 
                        Enabled="True" 
                        PromptCssClass="PromptCSS" 
                        TargetControlID="DDL_MemberSubCategories">
                    </ajaxToolkit:ListSearchExtender>
                     
                   
                  </td>
               
                <td class="style54">
                    <asp:Label ID="Label7" runat="server"  Text="Library Staff" 
                        style="font-weight: 700"></asp:Label>
                </td>
               
                <td class="style54">
                    <asp:DropDownList ID="DDL_LibraryStaff" 
                        runat="server" Enabled="True" 
                        Font-Bold="True" ForeColor="#0066CC" 
                        ToolTip="Plz Select Value from Drop-Down" AutoPostBack="True">
                    </asp:DropDownList>
                    <ajaxToolkit:ListSearchExtender 
                        ID="DDL_LibraryStaff_ListSearchExtender" 
                        runat="server" 
                        Enabled="True" 
                        PromptCssClass="PromptCSS" 
                        TargetControlID="DDL_LibraryStaff">
                    </ajaxToolkit:ListSearchExtender>
                    
                     
                    </td>
               
                <td class="style54">
                    &nbsp;</td>
               
                <td class="style54">
                    
                 </td>
               
            </tr>


            <tr>
                <td class="style51"> 
                    <asp:Label ID="Label12" runat="server"  Text="Acc.No" 
                        style="font-weight: 700"></asp:Label>
                </td>
                <td class="style53">
                    <asp:DropDownList ID="DDL_AccessionNo" 
                        runat="server" Enabled="True" 
                        Font-Bold="True" ForeColor="#0066CC" 
                        ToolTip="Plz Select Value from Drop-Down" AutoPostBack="True">
                    </asp:DropDownList>
                    <ajaxToolkit:ListSearchExtender 
                        ID="DDL_AccessionNo_ListSearchExtender" 
                        runat="server" 
                        Enabled="True" 
                        PromptCssClass="PromptCSS" 
                        TargetControlID="DDL_AccessionNo">
                    </ajaxToolkit:ListSearchExtender>
                </td>
                <td class="style53">
                    <asp:Label ID="Label13" runat="server"  Text="Status" 
                        style="font-weight: 700"></asp:Label>
                </td>
                <td class="style53">
                    <asp:DropDownList ID="DDL_Status" 
                        runat="server" Enabled="True" 
                        Font-Bold="True" ForeColor="#0066CC" 
                        ToolTip="Plz Select Value from Drop-Down">
                        <asp:ListItem>Issued</asp:ListItem>
                        <asp:ListItem>Renewed</asp:ListItem>
                        <asp:ListItem>Reserved</asp:ListItem>
                        <asp:ListItem>Returned</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="style53">
                    <asp:Label ID="Label14" runat="server"  Text="Collection" 
                        style="font-weight: 700"></asp:Label>
                </td>
                <td class="style53">
                     <asp:DropDownList ID="DDL_CollectionType" runat="server" Enabled="False" 
                         Font-Bold="True" ForeColor="#0066FF">
                         <asp:ListItem Value="C">Circulation</asp:ListItem>
                         <asp:ListItem Value="R">Reference</asp:ListItem>
                         <asp:ListItem Value="G">Book Bank (General)</asp:ListItem>
                         <asp:ListItem Value="S">Book Bank (SCST)</asp:ListItem>
                         <asp:ListItem Value="N">Non-Returnable</asp:ListItem>
                     </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="style51"> 
                     
                    <asp:Label ID="Label16" runat="server"  Text="Date" 
                        style="font-weight: 700"></asp:Label>
                </td>
                <td class="style52">
                    <asp:DropDownList ID="DDL_Date" 
                        runat="server" Enabled="True" 
                        Font-Bold="True" ForeColor="#0066CC" 
                        ToolTip="Plz Select Value from Drop-Down">
                        <asp:ListItem Selected="True" Value="ISSUE_DATE">Issue Date</asp:ListItem>
                        <asp:ListItem Value="RETURN_DATE">Return Date</asp:ListItem>
                        <asp:ListItem Value="RESERVE_DATE">Reserve Date</asp:ListItem>
                        <asp:ListItem Value="RENEW_DATE">Renew Date</asp:ListItem>
                        <asp:ListItem Value="DUE_DATE">Due Date</asp:ListItem>
                    </asp:DropDownList>
                     
                </td>
                <td class="style52">
                    <asp:Label ID="Label20" runat="server"  Text="Date From" 
                        style="font-weight: 700"></asp:Label>
                </td>
                <td class="style52">
                    <asp:TextBox ID="txt_Tra_SDate" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="10" 
                        Width="71px" onkeypress="return DateOnly (event)"></asp:TextBox>
                    
                     <ajaxToolkit:CalendarExtender ID="txt_Tra_SDate_CalendarExtender" 
                        runat="server" Enabled="True" Format="dd/MM/yyyy" 
                        TargetControlID="txt_Tra_SDate">
                    </ajaxToolkit:CalendarExtender>
                </td>
                <td class="style52">
                    <asp:Label ID="Label17" runat="server"  Text="Date To" 
                        style="font-weight: 700"></asp:Label>
                </td>
                <td class="style52">
                    <asp:TextBox ID="txt_Tra_EDate" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="10" 
                        Width="71px" onkeypress="return DateOnly2 (event)"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="txt_Tra_EDate_CalendarExtender" 
                        runat="server" Enabled="True" Format="dd/MM/yyyy" 
                        TargetControlID="txt_Tra_EDate">
                    </ajaxToolkit:CalendarExtender>
                </td>
            </tr>
             <tr>
                <td class="style51"> 
                    <asp:Label ID="Label11" runat="server"  Text="Order By" 
                        style="font-weight: 700"></asp:Label>
                </td>
                <td class="style53" colspan="5">
                    <asp:DropDownList ID="DDL_OrderBy" 
                        runat="server" Enabled="True" 
                        Font-Bold="True" ForeColor="#0066CC" 
                        ToolTip="Plz Select Value from Drop-Down">
                        <asp:ListItem>TITLE</asp:ListItem>
                        <asp:ListItem Value="MEM_NO">Member No</asp:ListItem>
                        <asp:ListItem Value="MEM_NAME">Member Name</asp:ListItem>
                        <asp:ListItem Value="ACCESSION_NO">Accession No</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;<asp:Label ID="Label19" runat="server"  Text="Sort By" 
                        style="font-weight: 700"></asp:Label>
                    <asp:DropDownList ID="DDL_SortBy" 
                        runat="server" Enabled="True" 
                        Font-Bold="True" ForeColor="#0066CC" 
                        ToolTip="Plz Select Value from Drop-Down">
                        <asp:ListItem Value="ASC">Ascending</asp:ListItem>
                        <asp:ListItem Value="DESC">Descending</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;| Overdue Items After :
                    
                     <asp:TextBox ID="txt_Cir_DueDays" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" Height="16px" MaxLength="6" 
                         onkeypress="return isNumberKey (event)" ToolTip="Enter No of Days in Advance to See the Overdue Items" 
                         Width="40px" Visible="True"></asp:TextBox>
                   &nbsp; Days</td>
            </tr>
            <tr>
                <td class="style56" colspan="6">
                    <asp:Label ID="Label3" runat="server"  Text="Step2: Select Optional Parameters from above and press SEARCH"
                        style="font-weight: 700; font-size: small; color: #FFFFFF"></asp:Label>
                 </td>
            </tr>
                   
           
        </table>

         <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                    <ContentTemplate>
          <table id="Table3" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
           
             <tr>
                <td class="style56" colspan="11">
                    <asp:Button ID="Tra_Search_Bttn" runat="server" CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red" TabIndex="14" Text="Search" 
                        Width="74px" ToolTip="Press to Search Results" AccessKey="s" 
                        Height="21px" />
                    <asp:Button ID="Tra_Delete_Bttn" runat="server" CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red" TabIndex="14" Text="Delete Selected Transactions" 
                        Width="210px" ToolTip="Press to Delete" AccessKey="d" 
                        Height="21px" Visible="False" />
                    <asp:Button ID="Tra_AllOverdue_Bttn" runat="server" AccessKey="o"  Visible="true"
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" Height="21px" 
                        TabIndex="14" Text="View Overdue Items" Width="150px" />
                    <asp:Button ID="Tra_MostIssuedBooks_Bttn" runat="server" AccessKey="m" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" Height="21px" 
                        TabIndex="14" Text="Most Issued Books" Visible="true" Width="140px" />
                    <asp:Button ID="Tra_TopBorrowers_Bttn" runat="server" AccessKey="b" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" Height="21px" 
                        TabIndex="14" Text="Top Borrowers" Visible="true" Width="140px" />
                 </td>
            </tr>
             
            
            

             <tr id="TR_Grid1" runat="server">
                <td  bgcolor="#99CCFF" class="style47" colspan="11">

                 <asp:Label ID="Label1" runat="server" Text="Record(s): " 
                            style="color: #FFFFFF; font-size: x-small"></asp:Label>
                  <asp:Panel ID="Panel2" runat="server" Height="250px" ScrollBars="Auto">
                   <asp:GridView ID="Grid1" runat="server" AllowPaging="True" DataKeyNames="CIR_ID"  
                        style="width: 100%;  text-align:  left;" allowsorting="True" 
                        AutoGenerateColumns="False" PageSize="100"  Font-Bold="True" 
                        Font-Names="Tahoma" Font-Size="8pt" Height="100px"  
                        HorizontalAlign="Center"  Width="6px" ShowFooter="True">
                        <Columns >                
                            <asp:TemplateField HeaderText="S.N."><ItemTemplate><asp:Label ID="lblsr"  runat="server" CssClass="MBody"   Text = '<%# Container.dataitemindex+1 %>' SkinID="" width="25px"></asp:Label></ItemTemplate><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" /><ItemStyle ForeColor="#336699" Font-Names="Tahoma" Width="20px" Font-Bold="True" Font-Size="Small" /></asp:TemplateField>
                    
                            <asp:BoundField   DataField="MEM_NO" HeaderText="Member No" SortExpression="MEM_NO" ><HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" /><ItemStyle forecolor="#0066FF" horizontalalign="Left" width="80px"   Font-Names="Arial"/></asp:BoundField>
                            <asp:BoundField   DataField="MEM_NAME" HeaderText="Member Name" SortExpression="MEM_NAME" ><HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" /><ItemStyle forecolor="#0066FF" horizontalalign="Left" width="80px"   Font-Names="Arial"/></asp:BoundField>
                            <asp:BoundField   DataField="ACCESSION" HeaderText="Accession No" SortExpression="ACCESSION" ><HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" /><ItemStyle forecolor="#0066FF" horizontalalign="Left" width="80px"   Font-Names="Arial"/></asp:BoundField>
                            <asp:BoundField   DataField="TITLE" HeaderText="Title" SortExpression="TITLE" ReadOnly="True"><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" /><ItemStyle forecolor="#0066FF" horizontalalign="Left" Font-Names="Arial"  width="350px" /></asp:BoundField>
                            <asp:BoundField   DataField="ISSUE_DATE" HeaderText="Issue Date" SortExpression="ISSUE_DATE" ReadOnly="True"><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" /><ItemStyle forecolor="#0066FF" horizontalalign="Left" Font-Names="Arial"  width="100px" /></asp:BoundField>
                            <asp:BoundField   DataField="DUE_DATE" HeaderText="Due Date" SortExpression="DUE_DATE" ReadOnly="True"><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" /><ItemStyle forecolor="#0066FF" horizontalalign="Left" Font-Names="Arial"  width="100px" /></asp:BoundField>
                            <asp:BoundField   DataField="RESERVE_DATE" HeaderText="Reserve Date" SortExpression="RESERVE_DATE" ReadOnly="True"><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" /><ItemStyle forecolor="#0066FF" horizontalalign="Left" Font-Names="Arial"  width="100px" /></asp:BoundField>
                            <asp:BoundField   DataField="RENEW_DATE" HeaderText="Renew Date" SortExpression="RENEW_DATE" ReadOnly="True"><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" /><ItemStyle forecolor="#0066FF" horizontalalign="Left" Font-Names="Arial"  width="100px" /></asp:BoundField>
                            <asp:BoundField   DataField="RETURN_DATE" HeaderText="Return Date" SortExpression="RETURN_DATE" ReadOnly="True"><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" /><ItemStyle forecolor="#0066FF" horizontalalign="Left" Font-Names="Arial"  width="100px" /></asp:BoundField>
                            <asp:BoundField   DataField="STATUS" HeaderText="Status" SortExpression="STATUS" ReadOnly="True"><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" /><ItemStyle forecolor="#0066FF" horizontalalign="Left" Font-Names="Arial"  width="100px" /></asp:BoundField>
                            
                             <asp:TemplateField  ControlStyle-Width="20px"  HeaderText="Delete" FooterText="Select to Delete" ShowHeader="true" ControlStyle-BackColor="Red">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbd"  runat="server" />
                                </ItemTemplate>

                                <HeaderTemplate>
                                    <asp:ImageButton ID="ImageButton1" runat="server" Height="16px" Width="16px" ToolTip="Select All" ImageUrl="~/Images/check_all.gif" onClientclick="return Select(true)"  />
                                    <asp:ImageButton ID="ImageButton2" runat="server" Height="16px" Width="16px" ToolTip="Deselect All" ImageUrl="~/Images/uncheck_all.gif"  OnClientClick ="return Select(false)" />
                                </HeaderTemplate>

                                <ControlStyle Width="50px" ForeColor="White"></ControlStyle>
                                <ItemStyle ForeColor="White" />
                                 <FooterStyle  ForeColor="White" />
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
             

              <tr id="TR_Grid2" runat="server">
                <td  bgcolor="#99CCFF" class="style47" colspan="11">

                 <asp:Label ID="Label4" runat="server" Text="Record(s): " 
                            style="color: #FFFFFF; font-size: x-small"></asp:Label>
                  <asp:Panel ID="Panel1" runat="server" Height="250px" ScrollBars="Auto">
                   <asp:GridView ID="Grid2" runat="server" AllowPaging="True"   
                        style="width: 100%;  text-align:  left;" allowsorting="True" 
                        AutoGenerateColumns="False" PageSize="100"  Font-Bold="True" 
                        Font-Names="Tahoma" Font-Size="8pt" Height="100px"  
                        HorizontalAlign="Center"  Width="6px" ShowFooter="True">
                        <Columns >                
                            <asp:TemplateField HeaderText="S.N."><ItemTemplate><asp:Label ID="lblsr"  runat="server" CssClass="MBody"   Text = '<%# Container.dataitemindex+1 %>' SkinID="" width="25px"></asp:Label></ItemTemplate><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" /><ItemStyle ForeColor="#336699" Font-Names="Tahoma" Width="20px" Font-Bold="True" Font-Size="Small" /></asp:TemplateField>
                            <asp:BoundField   DataField="ACCESSION" HeaderText="Accession No" SortExpression="ACCESSION" ><HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" /><ItemStyle forecolor="#0066FF" horizontalalign="Left" width="80px"   Font-Names="Arial"/></asp:BoundField>
                            <asp:BoundField   DataField="TITLE" HeaderText="Title" SortExpression="TITLE" ReadOnly="True"><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" /><ItemStyle forecolor="#0066FF" horizontalalign="Left" Font-Names="Arial"  width="350px" /></asp:BoundField>
                            <asp:BoundField   DataField="TIMES" HeaderText="No of Times" SortExpression="TIMES" ReadOnly="True"><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" /><ItemStyle forecolor="#0066FF" horizontalalign="Left" Font-Names="Arial"  width="100px" /></asp:BoundField>
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

             <tr id="TR_Grid3" runat="server">
                <td  bgcolor="#99CCFF" class="style47" colspan="11">

                 <asp:Label ID="Label5" runat="server" Text="Record(s): " 
                            style="color: #FFFFFF; font-size: x-small"></asp:Label>
                  <asp:Panel ID="Panel3" runat="server" Height="250px" ScrollBars="Auto">
                   <asp:GridView ID="Grid3" runat="server" AllowPaging="True"   
                        style="width: 100%;  text-align:  left;" allowsorting="True" 
                        AutoGenerateColumns="False" PageSize="100"  Font-Bold="True" 
                        Font-Names="Tahoma" Font-Size="8pt" Height="100px"  
                        HorizontalAlign="Center"  Width="6px" ShowFooter="True">
                        <Columns >                
                            <asp:TemplateField HeaderText="S.N."><ItemTemplate><asp:Label ID="lblsr"  runat="server" CssClass="MBody"   Text = '<%# Container.dataitemindex+1 %>' SkinID="" width="25px"></asp:Label></ItemTemplate><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" /><ItemStyle ForeColor="#336699" Font-Names="Tahoma" Width="20px" Font-Bold="True" Font-Size="Small" /></asp:TemplateField>
                            <asp:BoundField   DataField="MEM_NO" HeaderText="Member No" SortExpression="MEM_NO" ><HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" /><ItemStyle forecolor="#0066FF" horizontalalign="Left" width="80px"   Font-Names="Arial"/></asp:BoundField>
                            <asp:BoundField   DataField="MEM_NAME" HeaderText="Member Name" SortExpression="MEM_NAME" ReadOnly="True"><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" /><ItemStyle forecolor="#0066FF" horizontalalign="Left" Font-Names="Arial"  width="350px" /></asp:BoundField>
                            <asp:BoundField   DataField="TIMES" HeaderText="No of Times" SortExpression="TIMES" ReadOnly="True"><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" /><ItemStyle forecolor="#0066FF" horizontalalign="Left" Font-Names="Arial"  width="100px" /></asp:BoundField>
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
                           <asp:ListItem Value="EXCEL">Excel Format</asp:ListItem>
                       </asp:DropDownList>

                       &nbsp;Report Group By:
                    <asp:DropDownList ID="DDL_GroupBy" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC" ToolTip="Plz Select Value from Drop-Down">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem Value="MEM_NAME">Member Name</asp:ListItem>
                        <asp:ListItem Value="MEM_NO">Member Number</asp:ListItem>
                        <asp:ListItem Value="ISSUE_DATE">Issue Date</asp:ListItem>
                        <asp:ListItem Value="DUE_DATE">Due Date</asp:ListItem>
                        <asp:ListItem Value="RETURN_DATE">Return Date</asp:ListItem>
                        <asp:ListItem Value="STATUS">Status</asp:ListItem>
                        <asp:ListItem Value="ACCESSION">Accession No</asp:ListItem>
                        <asp:ListItem Value="TITLE">Title-Wise</asp:ListItem>
                    </asp:DropDownList>

                    <asp:Button ID="Print_Summary_Bttn" runat="server" CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red" Height="20px" TabIndex="14" 
                        Text="Summary Report" Visible="False" Width="130px" />
                    <asp:Button ID="Print_MostIssuedBooks_Bttn" runat="server" CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red" Height="20px" TabIndex="14" 
                        Text="Most Issued Books Report" Visible="False" Width="190px" />
                    <asp:Button ID="Print_TopBorrowers_Bttn" runat="server" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" Height="20px" 
                        TabIndex="14" Text="Top Borrowers Report" Visible="False" Width="190px" />
                </td>
            </tr>
             <tr>
                <td class="style56" colspan="9" bgcolor="#336699">
                                     
                       <asp:Label ID="Label18" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">Select Letter Template: </asp:Label>
                    <asp:DropDownList ID="DDL_Letters" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC">
                    </asp:DropDownList>
                    &nbsp;<asp:Button ID="Print_Reminder_Bttn" runat="server" CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red" Height="20px" TabIndex="14" 
                        Text="Print Reminder" Visible="False" Width="130px" />
                </td>
            </tr>     
        </table>
         </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="Tra_Search_Bttn" EventName="Click" />
                        <asp:PostBackTrigger ControlID="Print_Summary_Bttn" />
                        <asp:PostBackTrigger ControlID="Print_Reminder_Bttn" />
                        <asp:PostBackTrigger ControlID="Print_MostIssuedBooks_Bttn" />
                        <asp:PostBackTrigger ControlID="Print_TopBorrowers_Bttn" />
                    </Triggers>
                    </asp:UpdatePanel>

                
       
      


      

        


      
   
               
        </div>
</asp:Content>
