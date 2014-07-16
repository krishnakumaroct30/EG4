<%@ Page  Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="PhotoGallery.aspx.vb" Inherits="EG4.PhotoGallery" %>

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
               
                
        .style52
        {
            text-align: center;
            border-style: none;
            padding: 0px;
            font-weight: bold;
            background-color: #D5EAFF;
            height: 18px;
        }
               
                
        .style54
        {
            text-align: left;
            border-style: none;
            padding: 0px;
            font-weight: bold;
            background-color: #D5EAFF;
            height: 18px;
            width: 772px;
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
               
                
    </style>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

   
<script type="text/javascript">
    function SetPlaceHolders(sender, args) {
        var SelectedNode_Tree = args._node.get_text();
        alert(SelectedNode_Tree);
    }
</script>

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

     <script language ="javascript" type ="text/javascript" >
         function Select(Select) {

             var grdv = document.getElementById('<%= pnlThumbs.ClientID %>');
             //var chbk = "cbd";

             var Inputs = grdv.getElementsByTagName("input");

             for (var n = 0; n < Inputs.length; ++n) {
                 if (Inputs[n].type == 'checkbox' ){// && Inputs[n].id.indexOf(chbk, 0) >= 0) {
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
                <td  bgcolor="#003366" class="style43" rowspan="1" style="color: #FFFFFF"><strong>
                    Photo Gallery Manager</strong></td>
            </tr>
             <tr>
                <td class="style56" colspan="5" bgcolor="#336699">
                   <asp:Label ID="Label2" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">HELP: Create / Delete Folder and Sub-Folder for Uploading Photo</asp:Label>
                </td>
            </tr>            
            </table>

            
      

         <table id="Table1" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
             <tr>
                <td class="style47" colspan="2">
                     <asp:Label ID="Label6" runat="server" Font-Size="Medium" ForeColor="#CC3300" 
                         style="font-weight: 700"></asp:Label>
                 </td>
            </tr>
            
            <tr>
                <td class="style57"> 
                    Select 
                    Parent Folder</td>
                <td class="style54">
                    &nbsp; Folder Name:
                    <asp:TextBox ID="txt_Folder_Name" runat="server" 
                        onkeypress="return suppressNonEng (event)" Width="223px"></asp:TextBox>
                    <asp:Button ID="CreateFolder_Bttn" runat="server"  CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red" TabIndex="14" Text="Create Folder" 
                        Width="120px" 
                        ToolTip="Press to create folder and subfolder in the selected folder" 
                        Height="20px" AccessKey="c"   />
                &nbsp;<asp:Button ID="DeleteFolder_Bttn" runat="server"  CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red" TabIndex="14" 
                        Text="Delete Selected Folder" AccessKey="d" 
                        Width="160px" 
                        ToolTip="Press to Delete folder and subfolder in the selected folder" 
                        Height="20px"   />
                    <asp:Label ID="Label8" runat="server"></asp:Label>
                </td>               
            </tr>
            <tr>
                <td class="style57">                

                    <asp:TreeView ID="Treeview1" runat="server" AutoGenerateDataBindings="False"   
                        EnableClientScript="true" ExpandDepth="0" Font-Names="Palatino Linotype" 
                        Font-Size="Smaller" ImageSet="Simple" PopulateNodesFromClient="False">
                        <SelectedNodeStyle BackColor="#FFCC00" Font-Underline="True" ForeColor="Black" 
                            HorizontalPadding="0px" VerticalPadding="0px" BorderColor="#66FF99" />
                        <Nodes>
                            <asp:TreeNode  SelectAction="Select" Text="Photos"  PopulateOnDemand="true"
                                Value="Photos" Selected="True" ShowCheckBox="False"></asp:TreeNode>
                        </Nodes>
                        <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" 
                            HorizontalPadding="0px" NodeSpacing="0px" VerticalPadding="0px" />
                        <HoverNodeStyle Font-Underline="True" ForeColor="Transparent" 
                            BackColor="#CC9900" />
                        <ParentNodeStyle Font-Bold="False" />
                    </asp:TreeView>
                    
                </td>
                <td class="style54" align="center" valign="top"> 
                     &nbsp; Browse File:&nbsp;&nbsp; 
                     <asp:FileUpload ID="FileUpload1" runat="server" Font-Bold="True" 
                        ViewStateMode="Enabled" Width="223px" />
                    &nbsp;<asp:Button ID="bttn_Upload" runat="server" CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red" Height="22px" TabIndex="14" 
                        Text="Upload Photo" ToolTip="Press BACKUP Button " Width="120px" />
                    &nbsp;<asp:Button ID="bttn_DeletePhoto" runat="server" CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red" Height="22px" TabIndex="14" 
                        Text="Delete Selected Photo" ToolTip="Press BACKUP Button " Width="160px"  
                         CommandName ="Delete"/>
                    </td>
            </tr>
            </table>
           
            
                 <br />
      
        <asp:Panel ID="Panel1" runat="server" ScrollBars="Both" Width="100%" >   
           <table id="Table2" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
            <tr>
              
                <td class="style57" valign="top">
                 <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode ="Conditional">
              <ContentTemplate>
                    <asp:ImageButton ID="ImageButton1" runat="server" ToolTip="Select All" ImageUrl="~/Images/check_all.gif" onClientclick="return Select(true)" />
                    <asp:ImageButton ID="ImageButton2" runat="server" ToolTip="Deselect All" ImageUrl="~/Images/uncheck_all.gif"  OnClientClick ="return Select(false)"/>
                    
                   
                    <asp:Label ID="Label1" runat="server"></asp:Label>
                    <asp:Panel ID="pnlThumbs" runat="server" Height="700px" 
                        HorizontalAlign="Center" ScrollBars="Auto" Width="130px">
                    </asp:Panel>
                    <br>
                    </br>
                    </ContentTemplate>
                 <%--  <Triggers>  --%>                    
                        <%--<asp:AsyncPostBackTrigger ControlID ="bttn_DeletePhoto"  EventName="Click" /> --%>                                   
                                                   
                 <%-- </Triggers>--%>
                 </asp:UpdatePanel>

                </td>
                
                 
                <td class="style52" align="center" valign="middle" 
                    style="background-image: url('http://localhost:62163/Images/wood.jpg')">                                      
                        <asp:Image ID="imgMain" runat="server" BorderColor="#003399" 
                            BorderStyle="Outset" BorderWidth="4px" ImageAlign="Baseline"/>
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
          
               
 </asp:Content>
