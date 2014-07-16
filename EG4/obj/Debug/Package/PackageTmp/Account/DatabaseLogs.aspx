<%@ Page  Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="DatabaseLogs.aspx.vb" Inherits="EG4.DatabaseLogs" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>


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
               
       
                
    </style>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

<script type="text/javascript">
    function ValidateDates() {
        var startDate  = "";
        var endDate  = "";

        startDate = document.getElementById('<%=TextBox1.ClientID%>').value;
        endDate = document.getElementById('<%=TextBox2.ClientID%>').value;
        
        if (startDate > endDate) {
           alert( "The start date must be before the end date.");
            return false;
        }  
        else {

            return (true);
        }
        
    }
    
   
</script> 
 <script language ="javascript" type ="text/javascript" >
     function Select(Select) {

         var grdv = document.getElementById('<%= Grid1_Logs.ClientID %>');
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
                <td  bgcolor="#003366" class="style43" rowspan="1" style="color: #FFFFFF"><strong>Manage 
                    Database Logs</strong></td>
            </tr>
            <tr>
                <td  bgcolor="#99CCFF" style="text-align: center">
                     Delete Logs from time to time to save the space in database
                     <div id='translControl'></div></td>
            </tr>

            
            <tr> 
                           
                <td  align="center" bgcolor="#99CCFF">     
                    
                                        
                    


                
               
                    <asp:RadioButton ID="RadioButton1" runat="server" Checked="True" Text="Data Entry Logs" GroupName="dates" />
                    <asp:RadioButton ID="RadioButton2" runat="server" Text="OPAC Logs" GroupName="dates" />
                    <br />
                    Date From: <asp:TextBox ID="TextBox1" runat="server" Height="16px" Width="71px"></asp:TextBox>
                    
                    <asp:CalendarExtender ID="TextBox1_CalendarExtender" runat="server" 
                        Enabled="True" TargetControlID="TextBox1"  Format="dd/MM/yyyy" > 
                    </asp:CalendarExtender>

                     Date To: <asp:TextBox ID="TextBox2" runat="server" Height="16px" Width="71px"></asp:TextBox>
                    
                    <asp:CalendarExtender ID="TextBox2_CalendarExtender1" runat="server" 
                        Enabled="True" TargetControlID="TextBox2"  Format="dd/MM/yyyy" > 
                    </asp:CalendarExtender>
                   
              
                   
                 
                   
              
                   
                 
                    User Code
                    <asp:DropDownList ID="DD_UserCode" runat="server" ForeColor="#0066FF">
                    </asp:DropDownList>
                &nbsp;Action
                    <asp:DropDownList ID="DD_Action" runat="server" ForeColor="#0066FF">
                        <asp:ListItem Value="LoggedIn">Successfully Login</asp:ListItem>
                        <asp:ListItem>Failure</asp:ListItem>
                        <asp:ListItem Selected="True" Value="ALL">All</asp:ListItem>
                    </asp:DropDownList>
                    <br />
                    &nbsp;<asp:Button ID="Search_Bttn" runat="server"  CssClass="styleBttn" Font-Bold="True" 
                        ForeColor="Red" TabIndex="14" Text="Search" AccessKey="s"    OnClientClick="return ValidateDates();"  Width="74px" />
                    <br />
                    <hr />

      </td>
      </tr>   
                    
       <tr> 
                           
                <td  align="center" bgcolor="#99CCFF">  
               


                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                                           
                    <asp:Label ID="Label1" runat="server" Text="Record(s): "></asp:Label>
                   <div>
                   <asp:Button ID="Delete_Bttn" runat="server"  CssClass="styleBttn" Font-Bold="True" 
                        ForeColor="Red" TabIndex="14" Text="Delete Selected Row(s)" AccessKey="d"    
                            Width="165px" Height="26px" />
                   </div>
                    

                    <asp:GridView ID="Grid1_Logs" runat="server" AllowPaging="True" DataKeyNames="USER_LOG_ID"  
                        style="width: 100%;  text-align: center;" allowsorting="True" 
                        AutoGenerateColumns="False" PageSize="100"  Font-Bold="True" 
                        Font-Names="Tahoma" Font-Size="8pt" Height="100px"  
                        HorizontalAlign="Center"  Width="6px" ShowFooter="True">
                        <Columns >                
                            <asp:TemplateField HeaderText="Sr No">
                                <ItemTemplate>
                                    <asp:Label ID="lblsr" runat="server" CssClass="MBody"  SkinID="" width="50px"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                                <ItemStyle ForeColor="#336699" Font-Names="Tahoma" Width="50px" 
                                    Font-Bold="True" Font-Size="Small" />
                            </asp:TemplateField>
                    
                    <asp:BoundField   DataField="USER_CODE" HeaderText="User Code" 
                                SortExpression="USER_CODE" >                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="100px" 
                                Font-Names="Arial"/>                        
                    </asp:BoundField>
                    <asp:BoundField   DataField="LOGIN_DATE" HeaderText="Login Date" ReadOnly="True"  
                                DataFormatString="{0:dd/MM/yyyy}" SortExpression="LOGIN_DATE">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" Font-Names="Arial"  
                                width="100px" />                        
                    </asp:BoundField>
                     <asp:BoundField   DataField="LOGIN_TIME" HeaderText="Login Time" 
                                SortExpression="LOGIN_TIME" >                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="90px" 
                                Font-Names="Arial"/>                        
                    </asp:BoundField>
                     <asp:BoundField   DataField="PAGE_VISITED" HeaderText="Page Visited" >                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="90px" 
                                Font-Names="Arial"/>                        
                    </asp:BoundField>
                     <asp:BoundField   DataField="SUCCESS" HeaderText="Success" SortExpression="SUCCESS" >                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="90px" 
                                Font-Names="Arial"/>                        
                    </asp:BoundField>
                     <asp:BoundField   DataField="ACTION" HeaderText="Action" SortExpression="ACTION" >                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="90px" 
                                Font-Names="Arial"/>                        
                    </asp:BoundField>
                     <asp:BoundField   DataField="UI_TYPE" HeaderText="User Interface" 
                                SortExpression="UI_TYPE" >                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="80px" 
                                Font-Names="Arial"/>                        
                    </asp:BoundField>
                     <asp:BoundField   DataField="LOGOUT_DATE" HeaderText="Logout Date" 
                                SortExpression="LOGOUT_DATE">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="90px" 
                                Font-Names="Arial"/>                        
                    </asp:BoundField>
                     <asp:BoundField   DataField="LOGOUT_TIME" HeaderText="Logout Time" 
                                SortExpression="LOGOUT_TIME" >                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="100px" 
                                Font-Names="Arial"/>                        
                    </asp:BoundField>
                     <asp:BoundField   DataField="REMARKS" HeaderText="Remarks" >                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="90px" 
                                Font-Names="Arial"/>                        
                    </asp:BoundField>
                    <asp:TemplateField  ControlStyle-Width="20px"  HeaderText="Delete" FooterText="Select to Delete" ShowHeader="true" >
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
                         <asp:AsyncPostBackTrigger ControlID="Delete_Bttn" EventName="Click" />                                     
                   </Triggers>
                    </asp:UpdatePanel>

                   

                        
                           
                          
                                                                 
                </td>

            </tr>
            
            
            
        </table>


        
       
        
</asp:Content>
