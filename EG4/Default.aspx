<%@ Page Title="e-Granthalaya: A Digital Agenda for Library Automation and Networking" Language="vb" MasterPageFile="~/Site.Master" AutoEventWireup="false"
    CodeBehind="Default.aspx.vb" Inherits="EG4._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
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
                
        
         .style34
        {
            border-style: none;
            border-color: inherit;            
            width: 97%;
            
           
        }
        .style35
        {
            border-style: none;
            border-color: inherit;            
            width: 97%;
            margin-left: 15px;
            height: 34px;
        }
        
    
        .style43
    {
        text-align: center;
        font-size: small;
        color: #336699;
        height: 25px;
        width: 50%;
    }
                
        .style44
    {
        text-align: center;
        font-size: small;
        color: #336699;
        height: 17px;
        width: 50%;
    }
        
                
        .style47
        {
            text-align: justify;
            width: 50%;
            font-size: small;
        }
                 
                
        .style49
        {
            width: 50%;
            font-size: small;
            text-align: justify;
        }
                 
                
        #tech
        {
            height: 199px;
        }
                 
                
        </style>
    </asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">  

        
        <table align="center" cellspacing="2" border="0"  cellpadding="5" class="style34">
            <tr>
                <td  bgcolor="#99CCFF" class="style43">
                     <strong>Welcome to e-Granthalaya 4.0&nbsp;</strong></td>
                <td  bgcolor="#99CCFF" class="style43">
                     <strong>Union Catalog of Libraries</strong></td>
            </tr>
         </table>

        <table cellspacing="2" border="0"  cellpadding="5" class="style35">
            <tr>
            <td class="style47" valign="top">
                <strong>e-Granthalaya</strong>: A Digital 
                        Agenda for Library Automation and Networking - 
                        is a Library Management Software, developed and maintained by <strong>
                <a target="_blank" href="http://www.nic.in">National 
                        Informatics Centre</a></strong>, <a target="_blank" href="http://deity.gov.in">
                Department of Electronics and Information Technolog</a>, Ministry of 
                Communciations and Inforamtion Technology, Government of India.&nbsp; The software is being provided at 
                        ZERO cost to the Libraries in the Country. Besides, NIC is providing support and 
                        training for the software to automate Indian Libraries. The Software runs on 
                        Windows platform and uses MS SQL Server as well as PostgreSQL as back-end solutions. The Software is 
                        multi-lingual, UNICODE compliant, provides data entry solution for local/LAN/WAN 
                        based impelmentation. The current version of the software is 4.0 - Enterprise 
                        Editions, web-based data entry solution and implemented / hosted in NIC Server 
                        in cloud computing environment. <a target="_blank" href="http://egranthalaya.nic.in">
                Read More....</a></td>
            <td class="style47" valign="top">
                <strong>National Informatics Centre</strong> (NIC) has taken inititative to setup a Network of Libraries powered by e-Granthalaya Software. NIC wishes to strengthen the libraries primarily in the Government sector by computerization of these libraries. For this purpose these libraries are being provided e-Granthalaya Software at zero cost along with the training and support for automation and networking. National Informatics Centre (NIC) provides FREE hosting facility 
                for Catalogs of the Libraries powered by e-Granthalaya Software. Any Government library can host its catalog in a shared database 
                to make its catalog online. In the first phase, only those libraries catalog will be hosted which are using e-Granthalaya Software developed by NIC, later catalogs from other ILMS will also be accomodated. Union 
                Catalog is available at <a target="_blank" href="http://eglibnet.nic.in"><strong>
                http://www.eGLibNet.Gov.in.</strong></a>
                <a target="_blank" href="http://eglibnet.nic.in">Read More.. </a></td>
           </tr>
            <tr><td bgcolor="Silver" height="18px" colspan="2">
            <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000"  
                    codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,0,0"                     
                    style="height: 50px; width: 100%">
                <param name="movie" value="images/flashvortex.swf" />
                <param name="quality" value="best" />
                <param name="menu" value="true" />
                <param name="allowScriptAccess" value="sameDomain" />
                <embed src="images/flashvortex.swf" quality="best" menu="true" width="100%" height="50" type="application/x-shockwave-flash" pluginspage="http://www.macromedia.com/go/getflashplayer" allowscriptaccess="sameDomain" />
            </object>
            </td></tr>
        </table>

        <table cellspacing="2" border="0"  cellpadding="5" class="style35">
            <tr>
                <td class="style49" align="justify" valign="top" rowspan="5">
                    <asp:Label ID="Lbl_ClusterIntro" runat="server"></asp:Label>
               </td>
                <td class="style49" align="justify" valign="top" rowspan="5">
                    <asp:Label ID="Lbl_LibIntro" runat="server"></asp:Label>
                    <asp:Image ID="Image4" runat="server" ImageUrl="~/Images/Library2.jpg" 
                        Height="250px" Width="450px" ImageAlign="Top" />
                </td>
            </tr>
              
           
        </table>

         <table cellspacing="2" border="0"  cellpadding="1" class="style35" 
    bgcolor="#CCCCCC">
          <tr>
                <td class="style49" align="justify" valign="top" rowspan="5"> </td>
                 <td class="style49" align="justify" valign="top" rowspan="5"></td>
         </table>
       
         
        
        
        
                
        
    
    
    
    
    
    
    
    
    
    
    
</asp:Content>