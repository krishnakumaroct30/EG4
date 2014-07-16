<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Details.aspx.vb" Inherits="EG4.Details" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<script language="javascript"  type= "text/javascript">
    function checkLogin() {
        var mem = ""
        mem = '<%= Session.Item("LoggedMemberNo") %>';
        //alert(mem);
        if (mem == "") {
            alert("Please login the site to avail this facility..");
            return false;
        }
        else {
            return true;
        }
        return false;
    }
 </script>
<head runat="server">
    <title></title>
       
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 100%;
            border-style: solid;
            border-width: 1px;
        }
        .style3
        {
            text-align: center;           
            width:100%;            
           
        }
        .style4
        {
            color: #FFFFFF;
        }
        .style6
        {
            color:Blue;
        }
        
        .style7
        {
            text-align: center;
            font-family: Arial;
            font-size: smaller;
            color: #003366;
        }
        
         .style8
        {
            color:Blue;
            width=
        }
                        
         .style14
        {
            margin-left: 10px;
            cursor: pointer;
        }
        
            .style16
        {
            width: 144px;
            text-align: right;
        }
        
        .style17
        {
             width:100%;
        }
        
            .style19
        {
            color: #003366;
        }
        .style20
        {
            width: 30%;
            text-align: right;
            color: White;
            background-color:Gray;
        }
               
            .style21
        {
            color: #003366;
            background-color: #99CCFF;
        }
       
               
            .style22
        {
            color: #0066FF;
        }
       
               
            .style23
        {
            width: 56px;
        }
       
               
            </style>


</head>
<body style="background-image: url('../Images/ob019.jpg');">
    <form id="form1" runat="server">
     <asp:ScriptManager ID="ScriptManager2" runat="server" EnablePartialRendering="true">
               
     </asp:ScriptManager>

    <div style=" vertical-align:top; float:inherit; position: relative; margin-left:10px; width:100%">
        <div class="style3">
             <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always" >
                    <ContentTemplate>
            <asp:Menu
            ID="Menu1"
            Width="100%"
            runat="server"
            Orientation="Horizontal"
            StaticEnableDefaultPopOutImage="False"
            OnMenuItemClick="Menu1_MenuItemClick">
            <Items>
                <asp:MenuItem ImageUrl="~/Images/brup.png" Text=" " Value="0" Selected="true"></asp:MenuItem>
                <asp:MenuItem ImageUrl="~/Images/fvover.png" Text=" " Value="1"></asp:MenuItem>
                <asp:MenuItem ImageUrl="~/Images/mvover.png" Text=" " Value="2"></asp:MenuItem>
            </Items>        
            </asp:Menu>

            </ContentTemplate>  
        </asp:UpdatePanel>

        </div>
        <table id ="Table6" class="style2" runat="server">
            <tr>
                <td bgcolor="#0095DD" style="width:100%" >
                    <strong>
                    <asp:Label ID="Label1" runat="server" Text="Brief View" CssClass="style4" Width="100%"></asp:Label>
                    </strong>
                </td>
            </tr>
        </table>

<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always" >
                    <ContentTemplate>


    <asp:MultiView 
        ID="MultiView1"
        runat="server"
        ActiveViewIndex="0"  >
     <asp:View ID="Tab1" runat="server">
        <table  border="0" cellpadding="0" cellspacing="2" class="style1">
            <%  If myISBN <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>ISBN</strong></td>
                <td class="style6" style="width: 80%"><% = myISBN%></td>
            </tr>
            <% End If%>

            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Title</strong></td>
                <td class="style6" style="width: 80%"><% Response.Write(Highlight((myTitle), DETAILSX, "<font color=red>", "</font>"))%></td>
            </tr>

            <%  If myConf <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Conference</strong></td>
                <td class="style6" style="width: 80%"><% Response.Write(Highlight((myConf), DETAILSX, "<font color=red>", "</font>"))%></td>
            </tr>
            <% End If%>

            <%  If myAuthor <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Author(s)</strong></td>
                <td class="style6" style="width: 80%"><% Response.Write(Highlight((myAuthor), DETAILSX, "<font color=red>", "</font>"))%></td>
            </tr>
            <% End If%>

           <%  If myEdition <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Edition</strong></td>
                <td class="style6" style="width: 80%"><% Response.Write(myEdition)%></td>
            </tr>
            <% end if %>

            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Imprint</strong></td>
                <td class="style6" style="width: 80%; text-align: left;" align="center"><% Response.Write(Highlight((myPub), DETAILSX, "<font color=red>", "</font>"))%></td>
            </tr>

            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Date Added</strong></td>
                <td class="style6" style="width: 80%"><% Response.Write(FormatDateTime(myDateAdded, DateFormat.LongDate))%></td>
            </tr>
        </table>
        
  
        <hr />
     </asp:View>
        <asp:View ID="Tab2" runat="server">
        <table  border="0" cellpadding="0" cellspacing="2" class="style1">
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Cat No</strong></td>
                <td class="style6" style="width: 80%"><% = myDispCatNo%></td>
                <td align="center" class="style8" rowspan="6" valign="top" 
                    width="100px" height="50px">
                    <asp:Image ID="Image1" runat="server" />
                </td>
            </tr>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Bib Level</strong></td>
                <td class="style6" style="width: 80%"><% = BIB_CODE%></td>
            </tr>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Materials Type</strong></td>
                <td class="style6" style="width: 80%"><% = MAT_CODE%></td>
            </tr>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Document</strong></td>
                <td class="style6" style="width: 80%"><% = myDocType%></td>
            </tr>

             <%  If SP_NO <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Standard No</strong></td>
                <td class="style6" style="width: 80%"><% = SP_NO%></td>
            </tr>
            <% end if %>

             <%  If SP_VERSION <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Version</strong></td>
                <td class="style6" style="width: 80%"><% = SP_VERSION%></td>
            </tr>
            <% end if %>


             <%  If SP_ISSUE_BODY <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Issue Body</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% = SP_ISSUE_BODY%></td>
            </tr>
            <% end if %>

             <%  If SP_AMMENDMENTS <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Ammendment</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% = SP_AMMENDMENTS%></td>
            </tr>
            <% end if %>

             <%  If SP_TCSC <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Committee</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% = SP_TCSC%></td>
            </tr>
            <% end if %>

             <%  If SP_REAFFIRM_YEAR <> 0 Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Re-Affirm Year</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% = SP_REAFFIRM_YEAR%></td>
            </tr>
            <% end if %>


             <%  If SP_WITHDRAW_YEAR <> 0 Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Widthdaw Year</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% = SP_WITHDRAW_YEAR%></td>
            </tr>
            <% end if %>

             <%  If SP_UPDATES <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Updates</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% =SP_UPDATES%></td>
            </tr>
            <% end if %>


             <%  If REPORT_NO <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Report No</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% = REPORT_NO%></td>
            </tr>
            <% end if %>


             <%  If MANUAL_NO <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Manual No</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% = MANUAL_NO%></td>
            </tr>
            <% end if %>

             <%  If MANUAL_VER <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Version</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% = MANUAL_VER%></td>
            </tr>
            <% end if %>


             <%  If PATENT_NO <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Patent No</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% = PATENT_NO%></td>
            </tr>
            <% end if %>

             <%  If PATENT_INVENTOR <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Inventor</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% = PATENT_INVENTOR%></td>
            </tr>
            <% end if %>

            

            <%  If myISBN <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>ISBN</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% = myISBN%></td>
            </tr>
            <% End If%>


            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Title</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% Response.Write(Highlight((myTitle), DETAILSX, "<font color=red>", "</font>"))%></td>
            </tr>

            <%  If myVarTitle <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Var Title</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% Response.Write(Highlight((myVarTitle), DETAILSX, "<font color=red>", "</font>"))%></td>
            </tr>
            <% End If %> 

            <%  If myConf <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Conference</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% Response.Write(Highlight((myConf), DETAILSX, "<font color=red>", "</font>"))%></td>
            </tr>
            <% End If%>
             <%  If CONF_FROM <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Conference From</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% = CONF_FROM%></td>
            </tr>
            <% End If%>

             <%  If CONF_TO <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Conference To</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% = CONF_TO%></td>
            </tr>
            <% End If%>

             <%  If PATENTEE <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Patentee</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% =PATENTEE%></td>
            </tr>
            <% end if %>


            <%  If myAuthor <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Author(s)</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% = myAuthor%></td>
            </tr>
            <% End If%>

            <%  If myCorpAuthor <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Corp Author</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% Response.Write(Highlight((myCorpAuthor), DETAILSX, "<font color=red>", "</font>"))%></td>
            </tr>
            <% end if %>

            <%  If myEditors <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Editors</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% = myEditors%></td>
            </tr>
            <% end if %>

            <%  If myTr <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Translators</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% Response.Write(Highlight((myTr), DETAILSX, "<font color=red>", "</font>"))%></td>
            </tr>
            <% end if %>

            <%  If myIllus <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Illustrators</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% Response.Write(Highlight((myIllus), DETAILSX, "<font color=red>", "</font>"))%></td>
            </tr>
            <% end if %>

             <%  If COMMENTATORS <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Commentators</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% =COMMENTATORS%></td>
            </tr>
            <% end if %>

             <%  If COMPILER <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Compiler</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% = COMPILER%></td>
            </tr>
            <% end if %>

             <%  If REVISED_BY <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Revised By</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% = REVISED_BY%></td>
            </tr>
            <% end if %>


             <%  If SCHOLAR_NAME <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Scholar Name</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% = SCHOLAR_NAME%></td>
            </tr>
            <% end if %>

             <%  If SCHOLAR_DEPT <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Department</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% =SCHOLAR_DEPT%></td>
            </tr>
            <% end if %>

             <%  If GUIDE_NAME <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Guide Name</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% = GUIDE_NAME%></td>
            </tr>
            <% end if %>

             <%  If GUIDE_DEPT <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Guide Department</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% = GUIDE_DEPT%></td>
            </tr>
            <% end if %>

             <%  If DEGREE_NAME <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Degree</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% = DEGREE_NAME%></td>
            </tr>
            <% end if %>

            <%  If myEdition <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Edition</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% = myEdition%></td>
            </tr>
            <% end if %>

             <%  If MULTI_VOL <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Multi-Vol?</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% = MULTI_VOL%></td>
            </tr>
            <% end if %>

             <%  If TOTAL_VOL <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Total Vol</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% =TOTAL_VOL%></td>
            </tr>
            <% end if %>


             <%  If REPRINTS <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Reprints</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% =REPRINTS%></td>
            </tr>
            <% end if %>


            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Imprint</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% Response.Write(Highlight((myPub), DETAILSX, "<font color=red>", "</font>"))%></td>
            </tr>

            <%  If mySeries <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Series</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% Response.Write(Highlight((mySeries), DETAILSX, "<font color=red>", "</font>"))%></td>
            </tr>
            <% end if %>

            <%  If mySub <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Subject</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% Response.Write(Highlight((mySub), DETAILSX, "<font color=red>", "</font>"))%></td>
            </tr>
            <% End If%>

            <%  If myKeyword <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Keywords</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% Response.Write(Highlight((myKeyword), DETAILSX, "<font color=red>", "</font>"))%></td>
            </tr>
            <% End If%>

            <%  If myURL <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>URL</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% = myURL%></td>
            </tr>
            <% end if %>

            <%  If myCountry <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Country</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% = myCountry%></td>
            </tr>
            <% End If%>

             <%  If NOTE <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Note</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% = NOTE%></td>
            </tr>
            <% end if %>

             <%  If COMMENTS <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Comments</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% = COMMENTS%></td>
            </tr>
            <% end if %>

             <%  If PRODUCER <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Producer</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% = PRODUCER%></td>
            </tr>
            <% end if %>


             <%  If DESIGNER <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Designer</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% = DESIGNER%></td>
            </tr>
            <% end if %>

             <%  If MANUFACTURER <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Manufacturer</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% = MANUFACTURER%></td>
            </tr>
            <% end if %>

             <%  If MATERIALS <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Material</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% = MATERIALS%></td>
            </tr>
            <% end if %>

             <%  If TECHNIQ <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Technique</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% = TECHNIQ%></td>
            </tr>
            <% end if %>

             <%  If WORK_CATEGORY <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Work Category</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% = WORK_CATEGORY%></td>
            </tr>
            <% end if %>

             <%  If WORK_TYPE <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Work Type</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% = WORK_TYPE%></td>
            </tr>
            <% end if %>

             <%  If CREATOR <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Creator</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% = CREATOR%></td>
            </tr>
            <% end if %>

             <%  If ROLE_OF_CREATOR <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Role of Creator</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% = ROLE_OF_CREATOR%></td>
            </tr>
            <% end if %>

             <%  If RELATED_WORK <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Related Work</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% = RELATED_WORK%></td>
            </tr>
            <% end if %>

             <%  If LITERARY_FORM <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Literary Form</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% = LITERARY_FORM%></td>
            </tr>
            <% end if %>

             <%  If SOURCE <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Source</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% = SOURCE%></td>
            </tr>
            <% end if %>

             <%  If PHOTOGRAPHER <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Photographer</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% = PHOTOGRAPHER%></td>
            </tr>
            <% end if %>

             <%  If NATIONALITY <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Nationality</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% = NATIONALITY%></td>
            </tr>
            <% end if %>

             <%  If CHAIRMAN <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Chairman</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% = CHAIRMAN%></td>
            </tr>
            <% end if %>


             <%  If GOVERNMENT <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Government</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% = GOVERNMENT%></td>
            </tr>
            <% end if %>

             <%  If ACT_NO <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Act No</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% = ACT_NO%></td>
            </tr>
            <% end if %>

             <%  If ACT_YEAR <> 0 Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Act Year</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% = ACT_YEAR%></td>
            </tr>
            <% end if %>

            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Date Added</strong></td>
                <td class="style6" style="width: 80%" colspan="2"><% = FormatDateTime(myDateAdded,DateFormat.LongDate)%></td>
            </tr>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>Add User Tags</strong></td>
                <td class="style6" style="width: 80%" colspan="2">
                    <asp:TextBox ID="TextBox1" runat="server" Font-Bold="True" ForeColor="#3399FF" 
                        Height="17px" MaxLength="100" Width="305px"></asp:TextBox>
                    <asp:Button ID="Button1" runat="server"  CssClass="style14" Font-Bold="True" ForeColor="#FF3300" 
                        Text="Update" />
                </td>
            </tr>
        </table>


    </asp:View>
    <asp:View ID="Tab3" runat="server">
        <table  border="0" cellpadding="0" cellspacing="2" class="style1">
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>001</strong></td>
                <td class="style6" style="width: 80%"><% = Leader%></td>
            </tr>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>001</strong></td>
                <td class="style6" style="width: 80%"><% = my001%></td>
            </tr>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>003</strong></td>
                <td class="style6" style="width: 80%"><% = my003%></td>
            </tr>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>005</strong></td>
                <td class="style6" style="width: 80%"><% = my005%></td>
            </tr>

            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>008</strong></td>
                <td class="style6" style="width: 80%"><% = my008%></td>
            </tr>
            <%  If my020 <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>020</strong></td>
                <td class="style6" style="width: 80%"><% = my020%></td>
            </tr>
            <% End If%>

            <%  If my040 <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>040</strong></td>
                <td class="style6" style="width: 80%"><% = my040%></td>
            </tr>
            <% End If %> 

            <%  If my080 <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>080</strong></td>
                <td class="style6" style="width: 80%"><% = my080%></td>
            </tr>
            <% End If%>

            <%  If my088 <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>088</strong></td>
                <td class="style6" style="width: 80%"><% = my088%></td>
            </tr>
            <% End If%>

            <%  If my100 <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>100</strong></td>
                <td class="style6" style="width: 80%"><% = my100%></td>
            </tr>
            <% end if %>

            <%  If my110 <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>110</strong></td>
                <td class="style6" style="width: 80%"><% = my110%></td>
            </tr>
            <% end if %>

            <%  If my111 <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>111</strong></td>
                <td class="style6" style="width: 80%"><% = my111%></td>
            </tr>
            <% end if %>

            <%  If my130 <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>130</strong></td>
                <td class="style6" style="width: 80%"><% = my130%></td>
            </tr>
            <% end if %>

            <%  If my245 <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>245</strong></td>
                <td class="style6" style="width: 80%"><% = my245%></td>
            </tr>
            <% end if %>

            <%  If my246 <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>246</strong></td>
                <td class="style6" style="width: 80%"><% = my246%></td>
            </tr>
            <% end if %>

            <%  If my250 <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>250</strong></td>
                <td class="style6" style="width: 80%"><% = my250%></td>
            </tr>
            <% end if %>

            <%  If my260 <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>260</strong></td>
                <td class="style6" style="width: 80%"><% = my260%></td>
            </tr>
            <% End If%>

            <%  If my300 <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>300</strong></td>
                <td class="style6" style="width: 80%"><% = my300%></td>
            </tr>
            <% End If%>

            <%  If my490 <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>490</strong></td>
                <td class="style6" style="width: 80%"><% = my490%></td>
            </tr>
            <% end if %>

            <%  If my500 <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>500</strong></td>
                <td class="style6" style="width: 80%"><% = my500%></td>
            </tr>
            <% End If%>

            <%  If my520 <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>520</strong></td>
                <td class="style6" style="width: 80%"><% = my520%></td>
            </tr>
            <% End If%>

            <%  If my650 <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>650</strong></td>
                <td class="style6" style="width: 80%"><% = my650%></td>
            </tr>
            <% End If%>

            <%  If my700 <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>700</strong></td>
                <td class="style6" style="width: 80%"><% = my700%></td>
            </tr>
            <% End If%>

            <%  If my710 <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>710</strong></td>
                <td class="style6" style="width: 80%"><% = my710%></td>
            </tr>
            <%End If%>

        <%  If my711 <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>711</strong></td>
                <td class="style6" style="width: 80%"><% = my711%></td>
            </tr>
            <% End If%>

            <%  If my800 <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>800</strong></td>
                <td class="style6" style="width: 80%"><% = my800%></td>
            </tr>
            <% End If%>

            <%  If my830 <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>830</strong></td>
                <td class="style6" style="width: 80%"><% = my830%></td>
            </tr>
            <% End If%>

            <%  If my852 <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>850</strong></td>
                <td class="style6" style="width: 80%"><% = my852%></td>
            </tr>
            <% End If%>

            <%  If my856 <> "" Then%>
            <tr valign="top">
                <td class="style21" style="width: 20%" valign="middle"><strong>856</strong></td>
                <td class="style6" style="width: 80%"><% = my856%></td>
            </tr>
            <% End If%>

        </table>
    </asp:View>
    </asp:MultiView>
       

 

             </ContentTemplate>  
                     <Triggers>
                        <asp:PostBackTrigger  ControlID="Menu1"     />
                        <asp:PostBackTrigger ControlID="Button1" />
                   </Triggers>                
                    </asp:UpdatePanel>



    <p class="style7" style="background-color: #99CCFF; width:100%; vertical-align:middle" align="center" >
        <strong>Holdings Data</strong></p>
    
    <div class="style3"  align="left">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
                    <ContentTemplate>
                     
        <asp:GridView ID="GridView1" runat="server"   AutoGenerateColumns="False"  DataKeyNames="HOLD_ID"
            ForeColor="#3399FF" Font-Bold="True" allowsorting="True"
            Font-Names="Tahoma" Font-Size="8pt" 
            AutoGenerateSelectButton="False"  Width="98%" >
            
            <Columns>
                <asp:TemplateField HeaderText="S.N.">
                    <ItemTemplate >  <asp:Label ID="lblsr" runat="server" CssClass="MBody"  Text = '<%# Container.dataitemindex+1 %>' SkinID="" width="25px"></asp:Label></ItemTemplate>
                    <ItemStyle ForeColor="#0066FF" Width="5%"/>
                </asp:TemplateField>

                   <asp:ButtonField HeaderText="History"  Text="History" CommandName="Select">
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Smaller" />
                            <ItemStyle ForeColor="#CC0000" Width="20px" Font-Bold="true" />
                   </asp:ButtonField>

                <asp:BoundField   DataField="ACCESSION_NO" HeaderText="Acc.No" >                                               
                    <ItemStyle forecolor="#0066FF" horizontalalign="Left" Width="15%" />                        
                </asp:BoundField>

                <asp:BoundField   DataField="ACCESSION_DATE" HeaderText="Date" SortExpression="ACCESSION_DATE" DataFormatString=" {0:dd/MM/yyyy}">                                              
                     <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Smaller" />
                    <ItemStyle forecolor="#0066FF" horizontalalign="Left" Width="10%"/>                        
                </asp:BoundField> 

                <asp:BoundField   DataField="VOL_NO" HeaderText="Vol" ReadOnly="True"  SortExpression="VOL_NO">   
                    <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Smaller" />                                            
                    <ItemStyle forecolor="#0066FF" horizontalalign="Left" Width="5%"/>                        
                </asp:BoundField>  
                 
                <asp:BoundField   DataField="CLASS_NO" HeaderText="Class No" ReadOnly="True" >  
                    <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Smaller" />                                             
                    <ItemStyle forecolor="#0066FF" horizontalalign="Left" Width="20%"/>                        
                </asp:BoundField>

                <asp:BoundField   DataField="BOOK_NO" HeaderText="Book No" ReadOnly="True" >                                               
                    <ItemStyle forecolor="#0066FF" horizontalalign="Left" Width="10%"/> 
                    <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Smaller" />                       
                </asp:BoundField>

                <asp:BoundField   DataField="PAGINATION" HeaderText="Pages" ReadOnly="True" >                                               
                    <ItemStyle forecolor="#0066FF" horizontalalign="Left" Width="20%"/>    
                    <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Smaller" />                    
                </asp:BoundField>

                <asp:BoundField   DataField="LIB_CODE" HeaderText="Library" ReadOnly="True"   SortExpression="LIB_CODE" ItemStyle-Font-Size="X-Small">                                               
                    <ItemStyle forecolor="#0066FF" horizontalalign="Left" Width="10%" />  
                    <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Smaller" />                      
                </asp:BoundField>

                <asp:BoundField   DataField="STA_NAME" HeaderText="Status" ReadOnly="True"  SortExpression="STA_NAME">                                               
                    <ItemStyle forecolor="#0066FF" horizontalalign="Left" Width="20%"/> 
                    <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Smaller" />                       
                </asp:BoundField>

                 <asp:BoundField   DataField="PHYSICAL_LOCATION" HeaderText="Location" ReadOnly="True"  SortExpression="PHYSICAL_LOCATION">                                               
                    <ItemStyle forecolor="#0066FF" horizontalalign="Left" Width="20%"/>  
                    <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Smaller" />                      
                </asp:BoundField>
                                 
              
                 <asp:ButtonField HeaderText="Email"  Text="Email" CommandName="View">
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Smaller" />
                            <ItemStyle ForeColor="#CC0000" Width="20px" Font-Bold="true" />
                   </asp:ButtonField>
               
            
            </Columns>
            <RowStyle BackColor="#FFFFC0" BorderColor="Desktop" ForeColor="#3399FF" />
            <SelectedRowStyle BackColor="Tan" BorderColor="Olive" BorderStyle="Outset"  />
            <HeaderStyle BackColor="Desktop" Font-Bold="True" Font-Italic="False" Font-Names="Times new roman"
                Font-Overline="False" Font-Underline="False" ForeColor="White" Width="80%" />
            <AlternatingRowStyle BackColor="Silver" />
        </asp:GridView>
        
                 </ContentTemplate>  
                    <Triggers>
                        <asp:PostBackTrigger  ControlID="GridView1"     />
                   </Triggers>   
                    </asp:UpdatePanel>

    </div>
    <hr />
    <div  id="DIV_CH" runat="Server"  class="style3"  align="left"><span class="style22"><strong>Circulation Hisotry</strong></span>
        
         <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always" >
                    <ContentTemplate>

        <asp:GridView ID="GridView2" runat="server"   AutoGenerateColumns="False"  
            Font-Bold="True"  allowsorting="True"
            Font-Names="Tahoma" Font-Size="8pt"  Width="98%" >
            
            <Columns>
                 <asp:TemplateField HeaderText="S.N.">
                    <ItemTemplate >  <asp:Label ID="lblsr" runat="server" CssClass="MBody"  Text = '<%# Container.dataitemindex+1 %>' SkinID="" width="25px"></asp:Label></ItemTemplate>
                    <ItemStyle ForeColor="#009999" Width="5%"/>
                </asp:TemplateField>

                 <asp:BoundField   DataField="MEM_NO" HeaderText="Mem No" ReadOnly="True" >                                               
                    <ItemStyle  horizontalalign="Left" Width="20%"/>    
                    <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Smaller" />                    
                </asp:BoundField>

                 <asp:BoundField   DataField="MEM_NAME" HeaderText="Member Name" ReadOnly="True" >                                               
                    <ItemStyle horizontalalign="Left" Width="20%"/>    
                    <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Smaller" />                    
                </asp:BoundField>

                <asp:BoundField   DataField="ACCESSION" HeaderText="Acc.No." ReadOnly="True" >                                               
                    <ItemStyle  horizontalalign="Left" Width="20%"/>    
                    <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Smaller" />                    
                </asp:BoundField>

                <asp:BoundField   DataField="ISSUE_DATE" HeaderText="Issue Date" ReadOnly="True"  SortExpression="ISSUE_DATE"  DataFormatString=" {0:dd/MM/yyyy}">                                              
                    <ItemStyle  horizontalalign="Left" Width="10%"/>                        
                </asp:BoundField> 

                <asp:BoundField   DataField="DUE_DATE" HeaderText="Due Date" ReadOnly="True"  SortExpression="DUE_DATE"  DataFormatString=" {0:dd/MM/yyyy}">                                              
                    <ItemStyle  horizontalalign="Left" Width="10%"/>                        
                </asp:BoundField> 

                 <asp:BoundField   DataField="RETURN_DATE" HeaderText="Return Date" ReadOnly="True"  SortExpression="RETURN_DATE"  DataFormatString=" {0:dd/MM/yyyy}">                                              
                    <ItemStyle  horizontalalign="Left" Width="10%"/>                        
                </asp:BoundField> 

                 <asp:BoundField   DataField="STATUS" HeaderText="Status" ReadOnly="True"  SortExpression="STATUS">                                               
                    <ItemStyle  horizontalalign="Left" Width="20%"/>    
                    <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Smaller" />                    
                </asp:BoundField>




                
               
            
            </Columns>
            <RowStyle BackColor="#FFFFC0" BorderColor="Desktop" ForeColor="#3399FF" />
            <SelectedRowStyle BackColor="Tan" BorderColor="Olive" BorderStyle="Outset"  />
            <HeaderStyle BackColor="Desktop" Font-Bold="True" Font-Italic="False" Font-Names="Times new roman"
                Font-Overline="False" Font-Underline="False" ForeColor="White" Width="80%" />
            <AlternatingRowStyle BackColor="Silver" />
        </asp:GridView>
        


                    </ContentTemplate>  
                    
                    </asp:UpdatePanel>

    </div>
 

 <hr />
 <div  id="DIV_MICRO" runat="Server"  class="style3"  align="left"><span class="style22"><strong>List of Micro-Documents Indexed</strong></span>
        
         <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Always" >
                    <ContentTemplate>
                   
              <asp:GridView ID="GridView3" runat="server"   AutoGenerateColumns="False"   DataKeyNames="ART_NO"
                         Font-Bold="True"  allowsorting="True" Font-Names="Tahoma" Font-Size="8pt"  
                            Width="98%" ForeColor="#003366"  >
            
                    <Columns>
                         <asp:TemplateField HeaderText="S.N.">
                            <ItemTemplate >  <asp:Label ID="lblsr" runat="server" CssClass="MBody"  Text = '<%# Container.dataitemindex+1 %>' SkinID="" width="25px"></asp:Label></ItemTemplate>
                            <ItemStyle ForeColor="#003366" Width="5%"/>
                        </asp:TemplateField>

                         <asp:BoundField   DataField="TITLE" HeaderText="Title" ReadOnly="True" >                                               
                            <ItemStyle  horizontalalign="Left" Width="70%" ForeColor="#003366"/>    
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Smaller" />                    
                        </asp:BoundField>

                        <asp:BoundField   DataField="ART_NO" HeaderText="No" ReadOnly="True" >                                               
                            <ItemStyle  horizontalalign="Left" ForeColor="#003366"/>    
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Smaller" />                    
                        </asp:BoundField>

                        <asp:BoundField   DataField="LIB_CODE" HeaderText="Library" ReadOnly="True" >                                               
                            <ItemStyle  horizontalalign="Left" ForeColor="#003366"/>    
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Smaller" />                    
                        </asp:BoundField>

                        <asp:TemplateField HeaderText="Full-Text">
                            <ItemTemplate>
                                <asp:HyperLink ID ="HyperLink1"  Text="View File" runat="server" width="60px" Height="5px" onClick ="return checkLogin();" ></asp:HyperLink>
                            </ItemTemplate>
                            <ItemStyle  Width="60px"   Height="5px" ForeColor="#003366"/>
                        </asp:TemplateField>

                             
               
            
                    </Columns>
                    <RowStyle BackColor="#FFFFC0" BorderColor="Desktop" ForeColor="#3399FF" />
                    <SelectedRowStyle BackColor="Tan" BorderColor="Olive" BorderStyle="Outset"  />
                    <HeaderStyle BackColor="Desktop" Font-Bold="True" Font-Italic="False" Font-Names="Times new roman"
                        Font-Overline="False" Font-Underline="False" ForeColor="White" Width="80%" />
                    <AlternatingRowStyle BackColor="Silver" />
                </asp:GridView>
        



                    </ContentTemplate>  
                    
                    </asp:UpdatePanel>

</div>







<div class="style3">


    <table id ="Table1" runat="server" align="center" width="100%"  cellspacing="4" clientidmode="Static">
        <tr>
            <td colspan="2" class="style19">
                <strong>Request By Email</strong></td>
        </tr>
        <tr>
            <td class="style20">
                To</td>
            <td class="style3">
                <asp:TextBox ID="TextBox2" runat="server" Height="23px" Width="500px" 
                    CssClass="style6"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style20">
                Cc</td>
            <td class="style3">
                <asp:TextBox ID="TextBox3" runat="server" Height="23px" Width="500px" 
                    CssClass="style6"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style20">
                Bcc</td>
            <td class="style3">
                <asp:TextBox ID="TextBox4" runat="server" Height="23px" Width="500px" 
                    CssClass="style6"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style20">
                Subject</td>
            <td class="style3">
                <asp:TextBox ID="TextBox6" runat="server" Height="23px" Width="500px" 
                    CssClass="style6"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style20">
                Message</td>
            <td class="style3">
                <asp:TextBox ID="TextBox7" runat="server" Height="145px" Width="500px" 
                    CssClass="style6" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style16">
                &nbsp;</td>
            <td class="style3">
                <asp:Button ID="Button3" runat="server" BorderColor="#FF3300" 
                    BorderStyle="Solid" BorderWidth="3px" Font-Bold="True" ForeColor="#CC3300" 
                    Text="Send" Width="55px" OnClientClick ="return checkLogin();" />
&nbsp;
                <asp:Button ID="Button4" runat="server" BorderColor="#FF3300" 
                    BorderStyle="Solid" BorderWidth="3px" Font-Bold="True" ForeColor="#CC3300" 
                    Text="Cancel" />
            </td>
        </tr>
    </table>
    <br />

    <hr class="style17" width="60%" />
    
    <br />
    <br />


</div> 
<br />


    
        
   
    
 </div>  
</form>
</body>
</html>
