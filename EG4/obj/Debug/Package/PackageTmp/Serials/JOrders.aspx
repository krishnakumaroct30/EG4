<%@ Page  Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="JOrders.aspx.vb" Inherits="EG4.JOrders" %>


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
            text-align: justify;
            border-style: none;
            border-color: inherit;
            width: 170px;
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
        }
        .style55
        {
            text-align: justify;
            border-style: none;
            border-color: inherit;
            width: 25%;
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
            color:Blue;  
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

 <script type="text/javascript">
     function formfocus() {
         document.getElementById('<%= DDL_Approvals.ClientID %>').focus();
     }
     window.onload = formfocus;
    </script>
    <script language ="javascript" type ="text/javascript" >

     function GetCheckStatus() {
         var srcControlId = event.srcElement.id;
         var targetControlId = event.srcElement.id.replace('cbd', 'txt_App_CopyOrd');
         if (document.getElementById(srcControlId).checked)
             document.getElementById(targetControlId).disabled = false;
         else
             document.getElementById(targetControlId).disabled = true;
     }

     function Select(Select) {

         var grdv = document.getElementById('<%= Grid3.ClientID %>');
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
        function Select3(Select3) {

            var grdv = document.getElementById('<%= Grid3.ClientID %>');
            var chbk = "cbd";
            var txtbk = "txt_App_CopyOrd";

            var Inputs = grdv.getElementsByTagName("input");

            for (var n = 0; n < Inputs.length; ++n) {
                if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(chbk, 0) >= 0) {
                    Inputs[n].checked = Select3;
                }
                if (Inputs[n].type == 'text' && Inputs[n].id.indexOf(txtbk, 0) >= 0) {
                    if (Select3 == true) {
                        Inputs[n].disabled = false;
                    }
                    else {
                        Inputs[n].disabled = true;
                    }
                }
            }
            return false;
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
   <script language="javascript" type="text/javascript">

        function valid1() {

            if (document.getElementById('<%=txt_Acq_OrderNo.ClientID%>').value == "") {
                alert("Please enter proper \"Order No\" field.");
                document.getElementById("MainContent_txt_Acq_OrderNo").focus();
                return (false);
            }

            if (document.getElementById('<%=DDL_Approvals.ClientID%>').value == "") {
                alert("Please enter \" Select Approval No  from Drop-Down\" field.");
                document.getElementById("MainContent_DDL_Approvals").focus();
                return (false);
            }
            
            return (true);
        }
    </script>
    <script language="javascript" type="text/javascript">

         function valid2() {

             if (document.getElementById('<%=DDL_Vendors.ClientID%>').value == "") {
                 alert("Please enter \" Select Vendor No  from Drop-Down\" field.");
                 document.getElementById("MainContent_DDL_Vendors").focus();
                 return (false);
             }

             if (document.getElementById('<%=DDL_Orders.ClientID%>').value == "") {
                 alert("Please enter \" Select Order No  from Drop-Down\" field.");
                 document.getElementById("MainContent_DDL_Orders").focus();
                 return (false);
             }

             return (true);
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
                document.getElementById("MainContent_txt_Acq_OrdDate").focus();
                return (false);
            }
        }
    </script>
    <table id="ADMT1" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
            <tr>
                <td  bgcolor="#003366" class="style43" rowspan="1" style="color: #FFFFFF">Manage
                    <strong>Orders</strong></td>
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
                    <asp:MenuItem ImageUrl="~/Images/OrderUp_bttn.png" Text="" Value="0" Selected="true"></asp:MenuItem>
                    <asp:MenuItem ImageUrl="~/Images/OrderGenerateOver_bttn.png" Text="" Value="1"></asp:MenuItem>
                </Items>
        
            </asp:Menu>
             </ContentTemplate>  
        </asp:UpdatePanel>
    </div>
    
 <div class="style4">
        
 <br />
<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always" >
                    <ContentTemplate>

   <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
   <asp:View ID="Tab1" runat="server">
        <table id="Table1" runat="server" cellspacing="2" border="1"  cellpadding="2" class="style35">
            <tr>
                <td class="style56" colspan="5" bgcolor="#336699">
                   <asp:Label ID="Label3" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True" style="font-size: medium">Add Order</asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style56" colspan="5" bgcolor="#336699">
                   <asp:Label ID="Label2" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">Select Approval No from Drop-Down and Add Order Info in the Selected Acquisition Record(S) in the Grid</asp:Label>
                </td>
            </tr>
             <tr style=" color:Green ; font-weight:bold">
                <td class="style53"> 
                    Select Approval *</td>
                <td class="style55">
                    <asp:DropDownList ID="DDL_Approvals" runat="server" AutoPostBack="True" 
                        Font-Bold="True" ForeColor="#0066CC" Width="98%">
                    </asp:DropDownList>
                    <ajaxToolkit:ListSearchExtender ID="DDL_Approvals_ListSearchExtender" 
                        runat="server" Enabled="True" TargetControlID="DDL_Approvals" PromptCssClass="PromptCSS"></ajaxToolkit:ListSearchExtender>
                 </td>
                <td class="style55">
                    <asp:Label ID="Label23" runat="server" Text="App Date: "></asp:Label>
                 </td>
                <td class="style55" colspan="2">
                    <asp:Label ID="Label24" runat="server" Text="Committe Code:"></asp:Label>
                 </td>
                
            </tr>
            
        
                               
            <tr>
                <td class="style56" colspan="5">
                   <asp:Label ID="Label6" runat="server" Font-Size="Medium" ForeColor="Yellow" 
                        Font-Bold="True"></asp:Label>
                    <asp:Label ID="Label15" runat="server" Font-Bold="True" Font-Size="Medium" 
                        ForeColor="White"></asp:Label>
                </td>
            </tr>            
            
             <tr>
                <td class="style53"> 
                    <strong>Order No*</strong></td>
                <td class="style54" colspan="2">
                    <asp:TextBox ID="txt_Acq_OrderNo" runat="server" 
                        AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                        Height="18px" MaxLength="150" 
                        ToolTip="Enter Order No" Width="99%" 
                        Wrap="False" style="text-transform: uppercase"></asp:TextBox>
                    <ajaxToolkit:AutoCompleteExtender 
                        ID="txt_Acq_OrderNo_AutoCompleteExtender" 
                        runat="server" 
                        MinimumPrefixLength="1"
                        EnableCaching="true" 
                        CompletionSetCount="10"
                        ServiceMethod="SearchOrdNo"
                        TargetControlID="txt_Acq_OrderNo" 
                        FirstRowSelected = "false"></ajaxToolkit:AutoCompleteExtender>
                </td>               
                <td class="style54" colspan="2">
                    &nbsp;</td>               
            </tr>        
             <tr>
                <td class="style57"> 
                    <strong></strong></td>
                <td class="style58" colspan="4">                 
                                     
                    &nbsp;* Mandatory</td>                
            </tr>
            
        
            
             <tr>
                <td class="style47" colspan="5">
                    <asp:Button ID="Ord_Save_Bttn" runat="server" AccessKey="s" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" Height="23px" 
                         TabIndex="14" Text="Add Order in Selected Record(s)" 
                        Width="250px" Visible="False" ToolTip="Press to SAVE Record" OnClientClick="return valid1();"/>
                    <asp:Button ID="Ord_Cancel_Bttn" runat="server" AccessKey="c" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" Height="23px" 
                        TabIndex="14" Text="Cancel" 
                        Width="74px" ToolTip="Press to Cancel" Visible="False" />
                    <asp:Button ID="Ord_Delete_Bttn" runat="server" AccessKey="d" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" Height="23px" 
                        TabIndex="14" Text="Delete Order From Selected Record(s)" ToolTip="Press to Cancel" 
                        Width="250px" Visible="False" />
                 </td>
            </tr>
             <tr>
                <td class="style56" colspan="5" bgcolor="#336699">
                   <asp:Label ID="Label1" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">Record(s)</asp:Label>
                </td>
            </tr>
             <tr>  
                <td class="style56" colspan="5" bgcolor="#336699">                
                     

         <asp:GridView ID="Grid3" runat="server" AllowPaging="True" DataKeyNames="ACQ_ID"    
                        style="width: 100%;  text-align: center;" allowsorting="True" 
                        AutoGenerateColumns="False"  Font-Bold="True" 
                        Font-Names="Tahoma" Font-Size="8pt" Height="100px"  
                        HorizontalAlign="Center"   ShowFooter="True" CellSpacing="2" 
                        PageSize="2000">
                        <Columns >                
                            <asp:TemplateField HeaderText="S.N."><ItemTemplate><asp:Label ID="lblsr1" runat="server" CssClass="MBody"  SkinID="" width="25px"></asp:Label></ItemTemplate><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" /><ItemStyle ForeColor="#336699" Font-Names="Tahoma" Width="20px" 
                                    Font-Bold="True" Font-Size="Small" /></asp:TemplateField>                    

                     <asp:BoundField   DataField="TITLE"  HeaderText="Title" SortExpression="TITLE"><HeaderStyle Font-Bold="True"  Font-Names="Tahoma" Font-Size="Small" /><ItemStyle forecolor="#0066FF" horizontalalign="Left" width="350px" 
                                Font-Names="Arial"/></asp:BoundField>
                     <asp:BoundField   DataField="SUBS_YEAR" SortExpression="SUBS_YEAR" HeaderText="Subs.Year" 
                                ReadOnly="True"><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" /><ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" 
                                Font-Names="Arial"/></asp:BoundField>
                     <asp:BoundField   DataField="PROCESS_STATUS"  HeaderText="Status"><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" /><ItemStyle forecolor="Red" horizontalalign="Left" width="150px" 
                                Font-Names="Arial"/></asp:BoundField>
                    <asp:BoundField   DataField="ORDER_NO"  HeaderText="Order No" SortExpression="ORDER_NO"><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" /><ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" 
                                Font-Names="Arial"/></asp:BoundField>
                    <asp:BoundField   DataField="COPY_PROPOSED"  HeaderText="Copy Proposed" 
                                SortExpression="COPY_PROPOSED"><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" /><ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" Font-Names="Arial"/></asp:BoundField>
                    <asp:BoundField   DataField="COPY_APPROVED"  HeaderText="Copy Appd" 
                                SortExpression="COPY_APPROVED"><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" /><ItemStyle forecolor="#0066FF" horizontalalign="Left" width="150px" Font-Names="Arial"/></asp:BoundField>
                     
                     
                    <asp:TemplateField  HeaderText="Copy Ordered"><ItemTemplate><asp:TextBox ID="txt_App_CopyOrd" runat="server" Enabled="false" Font-Bold="true" ForeColor="Red" MaxLength="4" onkeypress="return isNumberKey(event)" Text='<%#  Eval("COPY_ORDERED") %>' visible="true" Width="45px"></asp:TextBox></ItemTemplate><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" /><ItemStyle ForeColor="#336699" BackColor="Red" Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" Width="150px" /></asp:TemplateField>
                    
                    
                    <asp:TemplateField  ControlStyle-Width="10px"  HeaderText="Select" 
                                FooterText="Select" ShowHeader="true"><ItemTemplate><asp:CheckBox ID="cbd"  runat="server" onclick="GetCheckStatus()" /></ItemTemplate><FooterStyle  ForeColor="White" /><HeaderTemplate><asp:ImageButton ID="ImageButton5" runat="server" Height="16px" Width="16px" ToolTip="Select All" ImageUrl="~/Images/check_all.gif" onClientclick="return Select3(true)"  /><asp:ImageButton ID="ImageButton6" runat="server" Height="16px" Width="16px" ToolTip="Deselect All" ImageUrl="~/Images/uncheck_all.gif"  onClientClick ="return Select3(false)" /></HeaderTemplate><ControlStyle Width="50px"></ControlStyle></asp:TemplateField>
                    
                    
                </Columns>
                    
                    <PagerStyle BackColor="#3399FF" BorderColor="#C0C000" BorderStyle="Solid" 
                        Font-Bold="True" ForeColor="White" HorizontalAlign="Center" 
                        VerticalAlign="Middle" Font-Size="Small" />
                    <RowStyle BackColor="#DADADA" BorderColor="Desktop" ForeColor="#3399FF" />
                    <SelectedRowStyle BackColor="Desktop" BorderColor="SteelBlue" BorderStyle="Solid" />
                        <EditRowStyle ForeColor="#CC3300" />
                    <HeaderStyle BackColor="Desktop" Font-Bold="True" Font-Italic="False" Font-Names="Times new roman"
                        Font-Overline="False" Font-Underline="False" ForeColor="White" Width="80%" />
                    <PagerSettings Position="TopAndBottom" FirstPageText="First" LastPageText="Last" 
                            PageButtonCount="2000" Mode="NumericFirstLast" />
                    <AlternatingRowStyle BackColor="#EFEFEF" Font-Names="Tahoma" ForeColor="#0066FF" />
                </asp:GridView>
                   

                  
                      
                   </td>
            </tr>
            
                        

        </table>

                
        
  
        
     </asp:View>
    <asp:View ID="Tab2" runat="server">
        <table id="Table2" runat="server" cellspacing="2" border="1"  cellpadding="2" class="style35">
            <tr>
                <td class="style56" colspan="5" bgcolor="#336699">
                   <asp:Label ID="Label5" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True" style="font-size: medium">Generate Order</asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style56" colspan="5" bgcolor="#336699">
                   <asp:Label ID="Label7" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">Select Order No from Drop-Down and Process it</asp:Label>
                    <asp:Label ID="Label20" runat="server" Font-Bold="True" Font-Size="Medium" 
                        ForeColor="Yellow"></asp:Label>
                </td>
            </tr>
            <tr style=" color:Green; font-weight:bold">
                <td class="style53"> 
                    Select Order *</td>
                <td class="style55">
                    <asp:DropDownList ID="DDL_Orders" runat="server" AutoPostBack="True" 
                        Font-Bold="True" ForeColor="#0066CC" Width="95%">
                    </asp:DropDownList>
                    <ajaxToolkit:ListSearchExtender ID="DDL_Orders_ListSearchExtender" 
                        runat="server" Enabled="True" IsSorted="True" PromptCssClass="PromptCSS" 
                        TargetControlID="DDL_Orders"></ajaxToolkit:ListSearchExtender>
                 </td>
                <td class="style55">
                    Order Date *&nbsp;
                    <asp:TextBox ID="txt_Acq_OrdDate" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="10" ToolTip="Click to Select Date" 
                        Width="71px"  onkeypress="return DateOnly (event)"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="txt_Acq_OrdDate_CalendarExtender" 
                        runat="server" Enabled="True" Format="dd/MM/yyyy" 
                        TargetControlID="txt_Acq_OrdDate"></ajaxToolkit:CalendarExtender>
                 </td>
                <td class="style55" colspan="2">
                    <asp:Button ID="Ord_Process_Bttn" runat="server" CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red" TabIndex="14" Text="Process" 
                        Width="100px" ToolTip="Press to Process the Approval" AccessKey="p" 
                        OnClientClick="return valid2();" />
                 </td>
                
            </tr>
              <tr style=" font-weight:bold">
                <td class="style53"> 
                    Select Vendor *</td>
                <td class="style54" colspan="2">
                    <asp:DropDownList ID="DDL_Vendors" runat="server" 
                        Font-Bold="True" ForeColor="#0066CC" Width="98%">
                    </asp:DropDownList>
                    <ajaxToolkit:ListSearchExtender ID="DDL_Vendors_ListSearchExtender" 
                        runat="server" Enabled="True" IsSorted="True" PromptCssClass="PromptCSS" 
                        TargetControlID="DDL_Vendors"></ajaxToolkit:ListSearchExtender>
                  </td>               
                <td class="style54" colspan="2">
                    <asp:Button ID="Ord_UnProcess_Bttn" runat="server" AccessKey="u" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" TabIndex="14" 
                        Text="Cancel Order" ToolTip="Press to Un-Order the Records" 
                        Width="100px" />
                  </td>               
            </tr>       
           
            <tr>
                <td class="style56" colspan="9" bgcolor="#336699">
                   
                   <asp:Label ID="Label516" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">Select Print Format: </asp:Label>
                       <asp:DropDownList ID="DDL_PrintFormats" runat="server" Font-Bold="True" 
                           ForeColor="#0066CC" ToolTip="Plz Select Value from Drop-Down">
                           <asp:ListItem></asp:ListItem>
                           <asp:ListItem Value="PDF" Selected="True">Pdf Format</asp:ListItem>
                           <asp:ListItem Value="DOC">Doc Format</asp:ListItem>
                       </asp:DropDownList>

                       

                    &nbsp;<asp:Button ID="Ord_Print_Bttn" runat="server" AccessKey="r" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" TabIndex="14" 
                        Text="Print Order" ToolTip="Press to Print Order Form" Visible="False" 
                        Width="110px" />
                    &nbsp;<asp:Label ID="Label25" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">Select Letter Template: </asp:Label>
                    <asp:DropDownList ID="DDL_Letters" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC">
                    </asp:DropDownList>

                       

                </td>
            </tr> 
        
            <tr>
                <td class="style56" colspan="5" bgcolor="#336699">
                   <asp:Label ID="Label9" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">Display Record(s)</asp:Label>
                </td>
            </tr>
             <tr>
                <td class="style56" colspan="5" bgcolor="#336699">
                   <asp:Label ID="Label22" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">HELP: All Records will be processed!</asp:Label>
                </td>
            </tr>
            
            
            
            

             <tr>  
                <td class="style56" colspan="5" bgcolor="#336699">                
                     
                 <asp:GridView ID="Grid2" runat="server" AllowPaging="True" allowsorting="True" 
                        AutoGenerateColumns="False" DataKeyNames="ACQ_ID" Font-Bold="True" 
                        Font-Names="Tahoma" Font-Size="8pt" Height="100px" HorizontalAlign="Center" 
                        ShowFooter="True" style="width: 100%;  text-align: center;">
                        <Columns>
                            <asp:TemplateField HeaderText="S.N."><ItemTemplate><asp:Label ID="lblsr" runat="server" CssClass="MBody" SkinID="" width="25px"></asp:Label></ItemTemplate><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" /><ItemStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" ForeColor="#336699" Width="25px" /></asp:TemplateField>

                            <asp:BoundField DataField="TITLE" HeaderText="Title" SortExpression="TITLE" visible="true"><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="350px" /><ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left" width="350px" /></asp:BoundField>

                             <asp:BoundField DataField="SUBS_YEAR" HeaderText="Subs.Year" SortExpression="SUBS_YEAR" visible="true"><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="100px" /><ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left" width="100px" /></asp:BoundField>

                            <asp:BoundField DataField="ORDER_NO" HeaderText="Order No"><HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" Width="150px" /><ItemStyle Font-Names="Tahoma" forecolor="#0066FF" horizontalalign="Left" width="150px" /></asp:BoundField>

                            <asp:BoundField DataField="ORDER_DATE" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Order Date" visible="true"><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="150px" /><ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left" width="150px" /></asp:BoundField>
                           
                            <asp:BoundField DataField="PROCESS_STATUS" HeaderText="Status"  visible="true"><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="160px" /><ItemStyle Font-Names="Arial" forecolor="Red" horizontalalign="Left" width="160px" /></asp:BoundField>
                            <asp:BoundField DataField="VEND_NAME" HeaderText="Vendor" visible="true"><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="200px" /><ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left" width="200px" /></asp:BoundField>
                            <asp:BoundField DataField="COPY_PROPOSED" HeaderText="Copy Proposed" visible="true"><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="180px" /><ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left" width="180px" /></asp:BoundField>

                            <asp:BoundField DataField="COPY_APPROVED" HeaderText="Copy Approved" visible="true"><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="180px" /><ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left" width="180px" /></asp:BoundField>

                            <asp:BoundField DataField="COPY_ORDERED" HeaderText="Copy Ordered" visible="true"><HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="180px" /><ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left" width="180px" /></asp:BoundField>
                        </Columns>
                        <PagerStyle BackColor="#3399FF" BorderColor="#C0C000" BorderStyle="Solid" Font-Bold="True" Font-Size="Small" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />
                        <RowStyle BackColor="#DADADA" BorderColor="Desktop" ForeColor="#3399FF" />
                        <SelectedRowStyle BackColor="Desktop" BorderColor="SteelBlue" BorderStyle="Solid" />
                        <HeaderStyle BackColor="Desktop" Font-Bold="True" Font-Italic="False" Font-Names="Times new roman" Font-Overline="False" Font-Underline="False" ForeColor="White" Width="90%" />
                        <PagerSettings FirstPageText="First" LastPageText="Last"  Mode="NumericFirstLast" PageButtonCount="10" Position="TopAndBottom" />
                        <AlternatingRowStyle BackColor="#EFEFEF" Font-Names="Tahoma" ForeColor="#0066FF" />
                    </asp:GridView>
                   

                  
                      
                   </td>
            </tr>
            
                        

        </table>

                
        


   
    

                
        


    </asp:View>
</asp:MultiView>
  

  
 
 
                </ContentTemplate>  
                     <Triggers>
                        <asp:AsyncPostBackTrigger  ControlID="Menu1" EventName="MenuItemClick"   /> 
                        <asp:AsyncPostBackTrigger  ControlID="DDL_Approvals" EventName="TextChanged"   />  
                        <asp:AsyncPostBackTrigger  ControlID="Ord_Save_Bttn" EventName="Click"   />   
                        <asp:AsyncPostBackTrigger  ControlID="Ord_Delete_Bttn" EventName="Click"   />  
                       <asp:PostBackTrigger ControlID="Ord_Print_Bttn" />                                 
                   </Triggers>                
                    </asp:UpdatePanel>                
                           
</div>

     
        
                   

</asp:Content>
