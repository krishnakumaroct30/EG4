<%@ Page  Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Holidays.aspx.vb" Inherits="EG4.Holidays" SmartNavigation ="true" MaintainScrollPositionOnPostback="true"  %>


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
               
                
        .style53
        {
            text-align: center;
            border-style: none;
            padding: 0px;
            font-weight: bold;
            background-color: #D5EAFF;
            height: 18px;
            width: 713px;
        }
               
                
                        
    </style>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

 
   
    
     
      <script language ="javascript" type ="text/javascript" >
          function Select(Select) {

              var grdv = document.getElementById('<%= Grid_Holidays.ClientID %>');
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
                <td  bgcolor="#003366" class="style43" colspan="2" rowspan="1" style="color: #FFFFFF"><strong>Holiday 
                    Calendar</strong></td>
            </tr>
            
            
            <tr>                
                <td  align="center" colspan="2">     

                   
                    <hr />

                      <ajaxToolkit:Accordion ID="Accordion2" runat="server" CssClass="accordion"  
                        HeaderCssClass="accordionHeader"  
                        HeaderSelectedCssClass="accordionHeaderSelected"  
                        ContentCssClass="accordionContent" Enabled="true" Visible="true" 
                         RequireOpenedPane="false"     SuppressHeaderPostbacks="true"   
                        SelectedIndex="1"   Width="100%" Height="237px"  >  
                            <Panes>  
                                <ajaxToolkit:AccordionPane ID="SearchPane" runat="server" >  
                                    <Header>Click To View / Hide Search Pane</Header>  
                                        <Content>     


                    Select YEAR:  


                    <asp:DropDownList ID="DropDownList_Year" runat="server"  ForeColor="#3399FF" 
                        AutoPostBack="True">
                    </asp:DropDownList>

                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                     <asp:Label ID="Label1" runat="server" Text="Record(s): "></asp:Label>
                   <div>
                   <asp:Button ID="Delete_Bttn" runat="server"  CssClass="styleBttn" Font-Bold="True" 
                        ForeColor="Red" TabIndex="14" Text="Delete Selected Row(s)" AccessKey="d"    
                            Width="165px" Height="30px" Enabled="false" />
                   </div>
                                           
                  <asp:Panel ID="Panel1" runat="server" Height="250px" ScrollBars="Auto">
                   <asp:GridView ID="Grid_Holidays" runat="server" AllowPaging="True" DataKeyNames="HOLI_ID"  
                        style="width: 100%;  text-align: center;" allowsorting="True" 
                        AutoGenerateColumns="False" PageSize="100"  Font-Bold="True" 
                        Font-Names="Tahoma" Font-Size="8pt" Height="100px"  
                        HorizontalAlign="Center"  Width="6px" ShowFooter="True">
                        <Columns >                
                            <asp:TemplateField HeaderText="S.N.">
                                <ItemTemplate>
                                    <asp:Label ID="lblsr" runat="server" CssClass="MBody"  SkinID="" width="25px"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                                <ItemStyle ForeColor="#336699" Font-Names="Tahoma" Width="20px" 
                                    Font-Bold="True" Font-Size="Small" />
                            </asp:TemplateField>                   
                   

                    <asp:BoundField   DataField="HOLI_DATE" HeaderText="Holiday Date" 
                                DataFormatString="{0:dd/MM/yyyy}"  SortExpression="HOLI_DATE" >                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="250px" 
                                Font-Names="Arial"/>                        
                    </asp:BoundField>
                    <asp:BoundField   DataField="HOLI_YEAR" HeaderText="Year" ReadOnly="True"  
                                SortExpression="HOLI_YEAR">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" Font-Names="Arial"  
                                width="150px" />                        
                    </asp:BoundField>
                     <asp:BoundField   DataField="HOLI_DAY" HeaderText="Day" 
                                SortExpression="HOLI_DAY" >                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="250px" 
                                Font-Names="Arial"/>                        
                    </asp:BoundField>
                      <asp:BoundField   DataField="HOLI_ID" HeaderText="ID" visible="true">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="50px" 
                                Font-Names="Arial"/>                        
                    </asp:BoundField>
                     
                    <asp:TemplateField  ControlStyle-Width="10px"  HeaderText="Delete" FooterText="Select to Delete" ShowHeader="true" >
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
                   
                </asp:Panel>
                   

                   </ContentTemplate>
                   <Triggers>
                         <asp:AsyncPostBackTrigger ControlID="Delete_Bttn" EventName="Click" />   
                         <asp:AsyncPostBackTrigger ControlID="Grid_Holidays" EventName="RowCommand" />                                           
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
                <td class="style47" colspan="2">
                     <asp:Label ID="Label6" runat="server" Font-Size="Medium" ForeColor="Red" 
                         style="font-weight: 700"></asp:Label>
                 </td>
            </tr>
            
            <tr>
               
                <td class="style53">
                    <asp:Calendar  
                                ID="Calendar1"   
                                runat="server"  
                                ForeColor="WhiteSmoke"  
                                DayNameFormat="Full"  
                                Font-Names="Book Antiqua"  
                                Font-Size="Medium"  
                                OnSelectionChanged="Calendar1_SelectionChanged"  
                                OnDayRender="Calendar1_DayRender" ShowGridLines="True" 
                        Width="100%" CellSpacing="1"  
                                >  
                                <DayHeaderStyle  
                                     BackColor="#FF9966"  
                                     />  
                                <DayStyle  
                                     BackColor="#669900"  
                                     BorderColor="Orange"  
                                     BorderWidth="1"  
                                     Font-Bold="true"  
                                     Font-Italic="true"  
                                     Font-Size="Large"  
                                     />  
                                <NextPrevStyle  
                                     Font-Italic="true"  
                                     Font-Names="Arial CE"  
                                     />  
                                <SelectedDayStyle  
                                     BackColor="DarkOrange"  
                                     BorderColor="Pink"  
                                     />  
                                <SelectorStyle BorderStyle="Solid" />
                                <TitleStyle  
                                     BackColor="MidnightBlue"  
                                     Height="36"  
                                     Font-Size="Large"  
                                     Font-Names="Courier New Baltic"  
                                     />  
                                <TodayDayStyle BackColor="#006600" BorderColor="#0066FF" />
                                <WeekendDayStyle ForeColor="#993333" />
                         </asp:Calendar> 




                         <br />
                    <br />
                    <asp:CheckBoxList ID="CheckBoxList2" runat="server" BackColor="#993333" 
                        ForeColor="Yellow" RepeatColumns="10" RepeatDirection="Horizontal">
                    </asp:CheckBoxList>
                    <br />
                    <br />




                         </td> 
            </tr>
            
            <tr>
                <td class="style47" colspan="2">
                    <asp:Button ID="bttn_Save" runat="server" AccessKey="s" CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red" TabIndex="14" 
                        Text="Save" ToolTip="Press to SAVE Record" Visible="False" Width="74px" />
                    <asp:Button ID="bttn_Update" runat="server"  CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red"
                        TabIndex="14" Text="Update" AccessKey="s" 
                        Width="74px" ToolTip="Press to UPDATE Record" />
                    <asp:Button ID="Cancel" runat="server"  CssClass="styleBttn" Font-Bold="True" ForeColor="Red" 
                        TabIndex="15" Text="Cancel" Width="71px" AccessKey="c" 
                        ToolTip="Press to Cancel the process"/>
                </td>
            </tr>
            
            <tr>
                <td  bgcolor="#99CCFF" class="style44" colspan="2" rowspan="0">
                     <strong>*<span class="style48"> Mandatory Fields</span></strong></td>
            </tr>


             

        </table>





      
   
                  </ContentTemplate>
                   <Triggers>
                        <asp:AsyncPostBackTrigger  ControlID="bttn_Save"   EventName="Click"   />    
                        <asp:AsyncPostBackTrigger ControlID ="bttn_Update"  EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID ="Cancel"  EventName="Click" />                             
                   </Triggers>
                    </asp:UpdatePanel>
        
</asp:Content>
