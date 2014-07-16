<%@ Page  Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Digitals.aspx.vb" Inherits="EG4.Digitals" %>
<%@ Import Namespace="System.IO" %>
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
               
          .style52
        {
            text-align: center;
            border-style: none;
            padding: 0px;
            font-weight: bold;
            background-color: #D5EAFF;
            height: 18px;
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
        
         .style57
        {
            text-align: center;
            border-style: none;
            border-color: inherit;
            width: 135px;
            padding: 0px;
            background-color: #99CCFF;
            font-size: small;
            height: 18px;
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
     //alpha-numeric only with Caps
     function suppressNonEng(event) {
         var chCode = ('charCode' in event) ? event.charCode : event.keyCode;
         if (48 <= chCode && chCode <= 57) {
             return (true);
         }
         if (97 <= chCode && chCode <= 122) {
             return (true);
         }
         if (65 <= chCode && chCode <= 90) {
             return (true);
         }

         if (chCode == 0 || chCode == 13 || chCode == 32) {
             return (true);
         }

         else {
             alert("Please Enter ENG Only Characters!");
             document.getElementById("MainContent_txt_Folder_Name").focus();
             return (false);
         }
     }
    </script>


         <script type="text/javascript">
             function formfocus() {
                 document.getElementById('<%= txt_Search_SearchString.ClientID %>').focus();
             }
             window.onload = formfocus;
        </script>


        <script type="text/javascript" src="../Scripts/imageloader.js"></script>
      <script type="text/javascript">
          function loadImage(url) {
              //alert(url);
              var n = url.replace("ThumbImage", "FullImage");
             // alert(n)
              var loader = new ImageLoader(n);
             // var loader = new ImageLoader(url);
              //setStatusText('loading image...', 'status');
              loader.loadEvent = function (url, image) {
                  //setStatusText(url + ' loaded', 'status');
                  putImage(image);
              }
              loader.load();
              
          }

          function putImage(image) {
              var h = document.getElementById('imageHolder');
              while (h.firstChild) {
                  h.removeChild(h.firstChild);
              }
              h.appendChild(image);
          }

          function setIframe(location) {
              //alert(location);
              var theIframe = document.createElement("iframe");
              theIframe.src = "Display_Thumb_Image.aspx?ebp=" + location;
              theIframe.width = 700;
              theIframe.height = 1024;
              //element.appendChild(theIframe);
            
              var h = document.getElementById('imageHolder');
              while (h.firstChild) {
                  h.removeChild(h.firstChild);
              }
              h.appendChild(theIframe);
          }

</script>
 <script language ="javascript" type ="text/javascript" >
     function Select(Select) {

         var grdv = document.getElementById('<%= Panel2.ClientID %>');
         //var chbk = "cbd";

         var Inputs = grdv.getElementsByTagName("input");

         for (var n = 0; n < Inputs.length; ++n) {
             if (Inputs[n].type == 'checkbox') {// && Inputs[n].id.indexOf(chbk, 0) >= 0) {
                 Inputs[n].checked = Select;
             }
         }
         return false;
     }

    </script> 



        <table id="ADMT1" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
            <tr>
                <td  bgcolor="#003366" class="style43" rowspan="1" style="color: #FFFFFF">
                    <strong>e-Books Manager</strong></td>
            </tr>
            
        </table>      
 
 <div class="style4">
        
         <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always" >
                    <ContentTemplate>
                         
  

        <table id="Table1" runat="server" cellspacing="2" border="1"  cellpadding="2" class="style35">
            <tr>
                <td class="style56" colspan="6" bgcolor="#336699">
                   <asp:Label ID="Label15" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True" style="font-size: medium">STEP 1: Search Title</asp:Label>
                    <asp:Label ID="Label6" runat="server" Font-Bold="True" Font-Size="Medium" 
                        ForeColor="Yellow"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style56" colspan="6" bgcolor="#336699">
                   <asp:Label ID="Label2" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">Search Record (s): Type Search String in Text Box and Pres ENTER</asp:Label>
                </td>
            </tr>
             <tr style=" color:Green ; font-weight:bold">
                <td class="style53"> 
                    Search Record</td>
                <td class="style55">
                    <asp:TextBox ID="txt_Search_SearchString" runat="server" 
                        AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                        Height="18px" MaxLength="250" ToolTip="Enter Data and press ENTER to Search Record" 
                        Width="99%" Wrap="False" AutoPostBack="True"></asp:TextBox> 
                 </td>
                <td class="style55" colspan="2">
                    In
                    <asp:DropDownList ID="DropDownList5" runat="server" ForeColor="#0066FF">
                        <asp:ListItem Selected="True" Value="CAT_NO">Cat Number</asp:ListItem>
                        <asp:ListItem>ISBN</asp:ListItem>
                        <asp:ListItem Value="ACCESSION_NO">Accession Number</asp:ListItem>
                        <asp:ListItem Value="TITLE">Title</asp:ListItem>
                        <asp:ListItem Value="SUB_TITLE">Sub Title</asp:ListItem>
                        <asp:ListItem Value="AUTHOR1">Author1</asp:ListItem>
                        <asp:ListItem Value="AUTHOR2">Author2</asp:ListItem>
                        <asp:ListItem Value="AUTHOR3">Author3</asp:ListItem>
                        <asp:ListItem Value="CORPORATE_AUTHOR">Corporate Author</asp:ListItem>
                        <asp:ListItem Value="EDITOR">Editor</asp:ListItem>
                        <asp:ListItem Value="SERIES_TITLE">Series</asp:ListItem>
                        <asp:ListItem Value="KEYWORDS">Keywords</asp:ListItem>
                        <asp:ListItem Value="YEAR_OF_PUB">Year</asp:ListItem>
                        <asp:ListItem>Note</asp:ListItem>
                        <asp:ListItem Value="CONF_NAME">Conference Name</asp:ListItem>
                        <asp:ListItem Value="TAGS">Tags</asp:ListItem>
                        <asp:ListItem Value="PUB_NAME">Publisher</asp:ListItem>
                        <asp:ListItem Value="SUB_NAME">Subject</asp:ListItem>
                    </asp:DropDownList>
                 </td>
                <td class="style55" colspan="2">
                    <asp:DropDownList ID="DropDownList6" runat="server" ForeColor="#0066FF">
                        <asp:ListItem Selected="True" Value="AND">All Words</asp:ListItem>
                        <asp:ListItem Value="OR">Any Word</asp:ListItem>
                        <asp:ListItem Value="LIKE">Like</asp:ListItem>
                        <asp:ListItem Value="SW">Start With</asp:ListItem>
                        <asp:ListItem Value="EW">End With</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Button ID="Search_Bttn" runat="server" CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red" TabIndex="14" Text="Search" 
                        Width="74px" ToolTip="Press to SEARCH Titles" />
                 </td>
                
            </tr>
            <tr style=" color:Green; font-weight:bold">
                <td class="style53"> 
                    Select Title</td>
                <td class="style54" colspan="5">
                    <asp:DropDownList ID="DDL_Titles" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC" Width="78%" AutoPostBack="True">
                    </asp:DropDownList>
          
                    
                   <%-- <ajaxToolkit:ListSearchExtender ID="DDL_Titles_ListSearchExtender" 
                        runat="server" Enabled="True" TargetControlID="DDL_Titles" PromptText="Type to search" PromptCssClass="PromptCSS">
                    </ajaxToolkit:ListSearchExtender>--%>
          
                    
                    &nbsp;Preess ENTER</td>                
            </tr>
           
        
            <tr>
                <td class="style56" colspan="6" bgcolor="#336699">
                   <asp:Label ID="Label1" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">Display Record</asp:Label>
                </td>
            </tr>
            
            
             <tr style=" color:Green; font-weight:bold">
                <td class="style53"> 
                    Cat Number</td>
                 <td class="style54" colspan="2">
                     <asp:Label ID="Label19" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                 </td>                
                 <td class="style54" colspan="2">
                     <asp:Label ID="Label24" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                 </td>
                 <td align="right"  rowspan="4" valign="middle" width="6%">
                     <asp:Image ID="Image4" runat="server" Height="57px" Width="29px" />
                 </td>
            </tr>
            <tr style=" color:Green; font-weight:bold">
                <td class="style53">
                    Title Details</td>
                <td class="style54" colspan="4">
                    <asp:Label ID="Label16" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                </td>
            </tr>
            <tr style=" color:Green; font-weight:bold">
                <td class="style53"> 
                    Author(s)</td>
                <td class="style54" colspan="4">
                    <asp:Label ID="Label17" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                </td>  
                               
            </tr>   
            <tr style=" color:Green; font-weight:bold">
                <td class="style53"> 
                    Imprint</td>
                <td class="style54" colspan="4">
                    <asp:Label ID="Label18" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                </td>                
            </tr> 
            <tr style=" color:Green; font-weight:bold">
                <td class="style53"> 
                    Multi-vol?</td>
                <td class="style54" colspan="4">
                    <asp:Label ID="Label23" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                </td>                
            </tr>                       
           
             <tr>
                <td class="style56" colspan="6" bgcolor="#336699">
                    &nbsp;</td>
            </tr> 
        </table>

       <table id="Table2" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
           <tr>
                <td class="style56" colspan="5" bgcolor="#336699">
                   <asp:Label ID="Label3" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">STEP 2: Upload Files</asp:Label>
                </td>
            </tr>            
      </table>

          
         <table id="Table3" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
             <tr>
                <td class="style47" colspan="2">
                     <asp:Label ID="Label4" runat="server" Font-Size="Medium" ForeColor="#CC3300" 
                         style="font-weight: 700"></asp:Label>
                 </td>
            </tr>
                       
            <tr id="TR_Folder" runat="server">                      
                <td class="style53">       

                    <asp:TreeView ID="Treeview1" runat="server" AutoGenerateDataBindings="False"   
                        EnableClientScript="true" ExpandDepth="0" Font-Names="Palatino Linotype" 
                        Font-Size="Smaller" ImageSet="Simple"  PopulateNodesFromClient="False">
                        <SelectedNodeStyle BackColor="#FFCC00" Font-Underline="True" ForeColor="Black" 
                            HorizontalPadding="0px" VerticalPadding="0px" BorderColor="#66FF99" />
                        <Nodes>
                            <asp:TreeNode  SelectAction="Select" Text="Files"  PopulateOnDemand="true"
                                Value="Files" Selected="True" ShowCheckBox="False"></asp:TreeNode>
                        </Nodes>
                        <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" 
                            HorizontalPadding="0px" NodeSpacing="0px" VerticalPadding="0px" />
                        <HoverNodeStyle Font-Underline="True" ForeColor="Transparent" 
                            BackColor="#CC9900" />
                        <ParentNodeStyle Font-Bold="False" />
                    </asp:TreeView>
                                   
                </td>

                <td class="style54">
                    Folder Name: &nbsp;&nbsp;<asp:TextBox ID="txt_Folder_Name" runat="server" 
                        onkeypress="return suppressNonEng (event)" Width="223px"></asp:TextBox>
                    <asp:Button ID="CreateFolder_Bttn" runat="server" AccessKey="c" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" Height="20px" 
                        TabIndex="14" Text="Create Sub Folder" 
                        ToolTip="Press to create folder and subfolder in the selected folder" 
                        Width="140px" />
                    <asp:Button ID="DeleteFolder_Bttn" runat="server" AccessKey="d" 
                        CssClass="styleBttn" Font-Bold="True" ForeColor="Red" Height="20px" 
                        TabIndex="14" Text="Delete Selected Folder" 
                        ToolTip="Press to Delete folder and subfolder in the selected folder" 
                        Width="160px" />
                    <asp:Label ID="Label8" runat="server"></asp:Label>
                </td>

                </tr>
                </table>


                        </ContentTemplate>  
                            <Triggers>                      
                                <asp:AsyncPostBackTrigger ControlID ="Search_Bttn"  EventName="Click" />                                   
                            </Triggers>    
                    </asp:UpdatePanel>  

                 <table id="Table5" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
                 
                    <tr id ="TR_Upload" runat="server"> 
                     <td class="style54" align="center" valign="top"> 
                         &nbsp; Browse File:&nbsp;&nbsp; 
                         <asp:FileUpload ID="FileUpload1" runat="server" Font-Bold="True" 
                            ViewStateMode="Enabled" Width="223px"  />
                        &nbsp;<asp:Button ID="bttn_Upload" runat="server" CssClass="styleBttn" 
                            Font-Bold="True" ForeColor="Red" Height="22px" TabIndex="14" 
                            Text="Upload File" ToolTip="Press BACKUP Button " Width="140px" />
                    
                        </td>
                </tr>
                 
                

                 <tr>
                <td class="style56" colspan="5" bgcolor="#336699">
                   <asp:Label ID="Label7" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">File Viewer</asp:Label>
                </td>
            </tr>
         </table>
           
                               
        <asp:Panel ID="Panel1" runat="server" ScrollBars="Both" Width="100%" >  
           <table id="Table4" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
            <tr>
              
                <td class="style57" valign="top">

                 <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode ="Always">
              <ContentTemplate>
                    <asp:ImageButton ID="ImageButton1" runat="server" ToolTip="Select All" ImageUrl="~/Images/check_all.gif" onClientclick="return Select(true)" />
                    <asp:ImageButton ID="ImageButton2" runat="server" ToolTip="Deselect All" ImageUrl="~/Images/uncheck_all.gif"  OnClientClick ="return Select(false)"/>
                   
                   
                    <asp:Label ID="Label5" runat="server"></asp:Label>

                    <asp:Button ID="bttn_DeletePhoto" runat="server" CssClass="styleBttn" 
                            Font-Bold="True" ForeColor="Red" Height="22px" TabIndex="14" 
                            Text="Delete Selected Files" ToolTip="Press to Delete File " Width="140px"  
                             CommandName ="Delete" Visible="False"/>


                      <asp:Panel ID="Panel2" runat="server" Height="700px"  HorizontalAlign="Left" 
                        ScrollBars="Auto" Width="135px" BackColor="#996600" BorderColor="#0066FF" 
                        BorderStyle="Solid" ForeColor="White">
                        <asp:Repeater ID="Repeater1" runat="server">
                            <ItemTemplate>    
                                <div>  
                                    <asp:CheckBox ID="chk" runat="server" AutoPostBack="false"   Text='<%# path.getfilename(Container.DataItem) %>'/>                  
                                    <img src='<%# "Display_Thumb_Image.aspx?ThumbImage="+ cstr(Container.DataItem) %>' onclick="loadImage(this.src)" />
                                </div>   
                            </ItemTemplate> 
                        </asp:Repeater>
                    <asp:Repeater ID="Repeater2" runat="server">
                        <ItemTemplate>    
                            <div>                     
                                <asp:CheckBox ID="chk1" runat="server" AutoPostBack="false"  Text='<%# Path.GetFileName(CStr(Container.DataItem))%>'> </asp:CheckBox>
                                <a href="#" id="<%# CStr(Container.DataItem)%>"  onclick="setIframe(this.id)">Click</a>
                            </div>    
                        </ItemTemplate> 
                    </asp:Repeater>

                    </asp:Panel>

                
                   
                    </ContentTemplate>
                  </asp:UpdatePanel>

                </td>
                
                 <td class="style52" align="center" valign="middle"   
                    style="background-image: url('../Images/wood.jpg')">  
                     <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode ="Always">
                            <ContentTemplate>  
                            <asp:ImageButton ID="ImgBttn_Prev" runat="server" 
                                        ImageUrl="~/Images/prev.jpg"  /> 
                            <asp:ImageButton ID="ImgBttn_Next" runat="server" 
                                        ImageUrl="~/Images/next.jpg" />                  
                                <div id ="imageHolder">
                                    
                                </div>
                             </ContentTemplate>
                     </asp:UpdatePanel>
                </td>
            </tr>
           
            <tr>
                <td class="style47" colspan="2">
                    &nbsp;</td>
            </tr>
            
            <tr>
                <td  bgcolor="#99CCFF" class="style44" colspan="2" rowspan="0">
                     &nbsp;</td>
            </tr>
        </table>
        </asp:Panel>
              
                           
</div>

     
        
                   

</asp:Content>
