<%@ Page  Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ClusterProfile.aspx.vb" Inherits="EG4.ClusterProfile" %>

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
            height: 26px;
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
               
                
        .style55
        {
            text-align: justify;
            border-style: none;
            border-color: inherit;
            width: 230px;
            padding: 0px;
            background-color: #99CCFF;
            font-size: small;
        }
        .style56
        {
            text-align: left;
            border-style: none;
            padding: 0px;
            font-weight: bold;
            background-color: #D5EAFF;
        }
               
                
    </style>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

 <script type="text/javascript" src="http://www.google.com/jsapi"></script> 
 <script type="text/javascript">
       // Load the Google Transliteration API   

       google.load("elements", "1", {
           packages: "transliteration"
       });
       var ids = ""
       function onLoad() {
           var options = {
               sourceLanguage: 'en',
               destinationLanguage: ['hi', 'bn', 'gu', 'kn', 'ml', 'mr', 'pa', 'sa', 'ta', 'te', 'ur'],
               shortcutKey: 'ctrl+g',
               transliterationEnabled: false
           };
           // Create an instance on TransliterationControl with the required         
           // options.         
           var control = new google.elements.transliteration.TransliterationControl(options);
           ids = ["MainContent_txt_Cluster_Name", "MainContent_txt_Cluster_Add", "MainContent_TextArea1"];
           control.makeTransliteratable(ids);
           // Show the transliteration control which can be used to toggle between         
           // English and Hindi and also choose other destination language.         
           control.showControl('translControl');
       }

       google.setOnLoadCallback(onLoad);
                   
    </script> 
    <script language ="javascript" src ="../md5.js" type ="text/javascript" ></script> 
     <script language="javascript" type="text/javascript">

         var MyHashed;

         function test1() {
             var Name = "";
             Name = document.getElementById('<%=txt_Cluster_Name.ClientID%>').value;

             if (Name == "") {
                 alert("Please enter proper \"Cluster Name\" field.");
                 document.getElementById("MainContent_txt_Cluster_Name").focus();
                 return (false);
             }

            if (document.getElementById('<%=TextArea1.ClientID%>').value == "") {
              alert("Plz Enter Brief Introduction of Library Cluster!");
              document.getElementById("MainContent_TextArea1").focus();
              return (false);
         }
    </script>
    
         <table id="ADMT1" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
            <tr>
                <td  bgcolor="#003366" class="style43" rowspan="1" style="color: #FFFFFF"><strong>Library Cluster Details</strong></td>
            </tr>
            <tr>
                <td  bgcolor="#99CCFF" style="text-align: center">
                     Type in Indian languages (Press Ctrl+g to toggle between English and India Language)
                     <div id='translControl'></div></td>
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
                <td class="style51"> 
                    <asp:Label ID="lbl_UserCode" runat="server" Text="Name of Cluster *"></asp:Label>
                </td>
                <td class="style54">
                    <asp:TextBox ID="txt_Cluster_Name" runat="server" MaxLength="750"  
                        ToolTip="Enter Name of Library Cluster (Name of Organization)" Wrap="False" 
                        Font-Bold="True" ForeColor="#0066FF"  
                        Height="20px" Width="98%"></asp:TextBox>
                    </td>
               
            </tr>
            <tr>
                <td class="style51">Address</td>
                <td class="style53" align="center" valign="top">
                    <asp:TextBox ID="txt_Cluster_Add" runat="server" MaxLength="250"  
                        ToolTip="Enter Address" 
                        Font-Bold="True" ForeColor="#0066FF" Width="86%"  
                        AutoCompleteType="DisplayName" Height="94px" TextMode="MultiLine"></asp:TextBox>
                    <asp:Image ID="Image21" runat="server" />
                    </td>
            </tr>
            <tr>
                <td class="style51">URL</td>
                <td class="style52">
                    <asp:TextBox ID="txt_Cluster_URL" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" MaxLength="450" ToolTip="Enter URL of Cluster" Width="98%" 
                        Wrap="False"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td class="style55">Introduction *</td>
                <td class="style56">
                    <textarea id="TextArea1"  rows = "2" cols="60" runat="server" 
                        style ="width:98%; height: 146px;"></textarea>
                </td>
            </tr>
            
            <tr>
                <td class="style51">Select Photo</td>
                <td class="style52">
                    <asp:FileUpload ID="FileUpload13" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="style47" colspan="2">
                    <asp:Button ID="bttn_Save" runat="server"  CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red"
                        OnClientClick="return test1();" TabIndex="14" Text="Save" AccessKey="s" 
                        Width="74px" />
                    <asp:Button ID="Cancel" runat="server"  CssClass="styleBttn" Font-Bold="True" ForeColor="Red" 
                        TabIndex="15" Text="Cancel" Width="71px" AccessKey="c"/>
                    <asp:Button ID="bttn_Update" runat="server"  CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red"
                        OnClientClick="return test1();" TabIndex="14" Text="Update" AccessKey="s" 
                        Width="74px" />
                </td>
            </tr>
            
            <tr>
                <td  bgcolor="#99CCFF" class="style44" colspan="2" rowspan="0">
                     <strong>*<span class="style48"> Mandatory Fields</span></strong></td>
            </tr>


             

        </table>

 
        
</asp:Content>
