<%@ Page  Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="DEFormats.aspx.vb" Inherits="EG4.DEFormats" %>


<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        
        /*Modal Popup*/
        .ModalPopupBG
        {
            background-color: #666699;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }

         .modalXBackground
        {
          background-color:#CCCCFF;
          filter:alpha(opacity=40);
          opacity:0.5;
        }

        .modalPopup
        {
        background-color: Gray;
        border-width: 3px;
        border-style: solid;
        border-color: #165EA9;
        padding: 3px;
        width: 600px;
        height: 150px;
        background-position:center;
        margin-bottom: 100;
         vertical-align:top;
         
        }
   
        .modalBackground {background-color:#fff; filter:alpha(opacity=70); opacity:0.7px; }

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
        #Table2
        {
            width: 926px;
        }
        .style53
        {
            text-align: justify;
            border-style: none;
            border-color: inherit;
            width: 82px;
            padding: 0px;
            background-color: #99CCFF;
            font-size: small;
            height: 18px;
        }
        </style>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
     
    


         <table id="ADMT1" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
            <tr>
                <td  bgcolor="#003366" class="style43" colspan="2" rowspan="1" style="color: #FFFFFF"><strong>Data Entry 
                    Formats</strong></td>
            </tr>
            
            <tr>                
                <td  align="center" colspan="2">      
                
                    Select Document Type:            
                
                <asp:DropDownList ID="DropDownList1" runat="server" ForeColor="#0066FF" 
                        AutoPostBack="True"  >
                        <asp:ListItem Value="BIB_LEVELS" Selected="True">Bibliographic Levels</asp:ListItem>
                        <asp:ListItem Value="MATERIALS">Materials</asp:ListItem>
                        <asp:ListItem Value="DOC_TYPES">Document Types</asp:ListItem>
                        <asp:ListItem Value="ACC_MATERIALS">Accompanying Materials</asp:ListItem>
                        <asp:ListItem Value="ACQMODES">Acquisition Modes</asp:ListItem>
                        <asp:ListItem Value="BINDINGS">Binding Types</asp:ListItem>
                        <asp:ListItem Value="BOOKSTATUS">Copy Status</asp:ListItem>
                        <asp:ListItem Value="COUNTRIES">Countries</asp:ListItem>
                        <asp:ListItem Value="CURRENCIES">Currencies</asp:ListItem>
                        <asp:ListItem Value="LANGUAGES">Languages</asp:ListItem>
                        <asp:ListItem Value="FREQUENCIES">Serial Frequencies</asp:ListItem>
                    </asp:DropDownList>
                   
             &nbsp;and Press ENTER</td>
            </tr>
             <tr>                
                <td  align="left" colspan="2" style="text-align: left">      
                 <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
                     <Triggers>
                            <asp:AsyncPostBackTrigger  ControlID="DropDownList1" EventName="TextChanged"   /> 
                             <asp:AsyncPostBackTrigger  ControlID="bttn_Save"   />    
                            <asp:AsyncPostBackTrigger ControlID ="bttn_Update"  />   
                            <asp:AsyncPostBackTrigger ControlID ="bttn_Delete"  />                                       
                       </Triggers>
                 </asp:UpdatePanel>                 
                </td>
            </tr>                       
        </table>
               
        
         <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
              <ContentTemplate>
  
  


  





         <table id="Table1" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
          <tr>
                <td class="style47" colspan="2">
                     &nbsp;<asp:Button ID="bttn_Save" runat="server"  CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red"
                         TabIndex="14" Text="Save" AccessKey="s" 
                        Width="74px" Visible="False" ToolTip="Press to SAVE Record" />
                    &nbsp;<asp:Button ID="bttn_Update" runat="server"  CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red"
                         TabIndex="14" Text="Update" AccessKey="s" 
                        Width="74px" Visible="False" ToolTip="Press to Update Record" />
                    &nbsp;<asp:Button ID="bttn_Delete" runat="server" AccessKey="d" CssClass="styleBttn" 
                        Font-Bold="True" ForeColor="Red" TabIndex="14" Text="Delete" Visible="False" 
                        Width="74px" ToolTip="Press to Delete Record" />
                     <asp:Label ID="Label10" runat="server" Text="Label"></asp:Label>
                 </td>
            </tr>
            <tr>
                <td class="style47" colspan="2">
                     <asp:Label ID="Label6" runat="server" Font-Size="Medium" ForeColor="#CC3300" 
                         style="font-weight: 700"></asp:Label>
                 </td>
            </tr>
            <tr>
                <td class="style47" colspan="2">
                    HELP: Here Data Entry Formats for a particular Document Type can be created with 
                    Desired Fields by selecting the Fields, Mandatory Fields already Selected. 
                    Selected Fields will only be available during Data Entry.</td>
            </tr>
            
            <tr id="TR_General" runat="server">
                <td class="style51">
                    General Fields</td>
                <td class="style52">
                    <asp:TreeView ID="TreeView_General" runat="server" ShowCheckBoxes="All" 
                        ShowLines="True">
                        <Nodes>
                            <asp:TreeNode Expanded="True" ShowCheckBox="False" 
                                Text="General Fields" Value="General Fields">
                                <asp:TreeNode Checked="True" Expanded="True" ShowCheckBox="False" 
                                    Text="Language" Value="LANG_CODE" ImageUrl="~/Images/tick.png"></asp:TreeNode>
                                <asp:TreeNode Checked="True" ShowCheckBox="False" Text="Bib Level" 
                                    Value="BIB_CODE" ImageUrl="~/Images/tick.png" Expanded="True"></asp:TreeNode>
                                <asp:TreeNode Checked="True" Expanded="True" ShowCheckBox="False" 
                                    Text="Material Type" Value="MAT_CODE" ImageUrl="~/Images/tick.png"></asp:TreeNode>
                                <asp:TreeNode Checked="True" Expanded="True" ShowCheckBox="False" 
                                    Text="Document Type" Value="DOC_TYPE_CODE" ImageUrl="~/Images/tick.png" 
                                    Selected="True"></asp:TreeNode>
                            </asp:TreeNode>
                        </Nodes>
                    </asp:TreeView>
                </td>
            </tr>
            <tr id="TR_Reports" runat="server">
                <td class="style51">Reports and Theses</td>
                <td class="style52">
                    &nbsp;<asp:TreeView ID="TreeView_Reports" runat="server" 
                        ShowCheckBoxes="All" ShowLines="True">
                        <Nodes>
                            <asp:TreeNode Expanded="True" ShowCheckBox="False" 
                                Text="Reports, Dissertations and Theses" Value="REPORTS">
                                <asp:TreeNode Checked="True" Expanded="True" ShowCheckBox="False" 
                                    Text="Report No" Value="REPORT_NO" ImageUrl="~/Images/tick.png" 
                                    Selected="True"></asp:TreeNode>
                                <asp:TreeNode ShowCheckBox="True" Text="Scholar Name" Value="SCHOLAR_NAME">
                                </asp:TreeNode>
                                <asp:TreeNode ShowCheckBox="True" Text="Scholar Department" 
                                    Value="SCHOLAR_DEPT"></asp:TreeNode>
                                <asp:TreeNode ShowCheckBox="True" Text="Guide Name" Value="GUIDE_NAME">
                                </asp:TreeNode>
                                <asp:TreeNode ShowCheckBox="True" Text="Guide Department" Value="GUIDE_DEPT">
                                </asp:TreeNode>
                                <asp:TreeNode ShowCheckBox="True" Text="Degree Name" Value="DEGREE_NAME">
                                </asp:TreeNode>
                            </asp:TreeNode>
                        </Nodes>
                    </asp:TreeView>
                </td>
            </tr>
            <tr id="TR_Standards" runat="server">
                <td class="style51">Standards Specification</td>
                <td class="style52">
                    <asp:TreeView ID="TreeView_SP" runat="server" ShowCheckBoxes="All" ShowLines="True">
                        <Nodes>
                            <asp:TreeNode Expanded="True" ShowCheckBox="False" Text="Standards Specifications Fields" Value="Standard Specification">
                                <asp:TreeNode Checked="True" Expanded="True" ShowCheckBox="False" Text="Standard Specification No" Value="SP_NO" ImageUrl="~/Images/tick.png"></asp:TreeNode>
                                <asp:TreeNode ShowCheckBox="True" Text="Standard Version/Revision No" Value="SP_VERSION"></asp:TreeNode>
                                <asp:TreeNode ShowCheckBox="True" Text="Reaffirmation Year" Value="SP_REAFFIRM_YEAR"></asp:TreeNode>
                                <asp:TreeNode ShowCheckBox="True" Text="Technical/Sectional Committee" Value="SP_TCSC"></asp:TreeNode>
                                <asp:TreeNode ShowCheckBox="True" Text="Updates Details" Value="SP_UPDATES"></asp:TreeNode>
                                <asp:TreeNode ShowCheckBox="True" Text="Withdraw Year" Value="SP_WITHDRAW_YEAR"></asp:TreeNode>
                                <asp:TreeNode ShowCheckBox="True" Text="Ammendments Details" Value="SP_AMMENDMENTS"></asp:TreeNode>
                                <asp:TreeNode ShowCheckBox="True" Text="Issuing Body" Value="SP_ISSUE_BODY"></asp:TreeNode>
                            </asp:TreeNode>
                        </Nodes>
                    </asp:TreeView>
                </td>
            </tr>
             <tr id="TR_Manuals" runat="server">
                <td class="style51">Manuals</td>
                <td class="style52">
                    <asp:TreeView ID="TreeView_Manuals" runat="server" 
                        ShowCheckBoxes="All" ShowLines="True">
                        <Nodes>
                            <asp:TreeNode Expanded="True" ShowCheckBox="False" 
                                Text="Manuals Fields" Value="MANUALS">
                                <asp:TreeNode Checked="True" Expanded="True" ShowCheckBox="False" 
                                    Text="Manual No" Value="MANUAL_NO" ImageUrl="~/Images/tick.png"></asp:TreeNode>
                                <asp:TreeNode ShowCheckBox="True" Text="Manual Version" 
                                    Value="MANUAL_VERSION"></asp:TreeNode>
                            </asp:TreeNode>
                        </Nodes>
                    </asp:TreeView>
                 </td>
            </tr>
             <tr id ="TR_Patents" runat ="server">
                <td class="style51">Patents</td>
                <td class="style52">
                    
                    <asp:TreeView ID="TreeView_Patents" runat="server" ShowCheckBoxes="All" ShowLines="True">
                        <Nodes>
                            <asp:TreeNode Expanded="True" ShowCheckBox="False" Text="Patent Fields" Value="PATENTS">
                                <asp:TreeNode Checked="True" Expanded="True" ShowCheckBox="False" Text="Patent No" Value="PATENT_NO" ImageUrl="~/Images/tick.png"></asp:TreeNode>
                                <asp:TreeNode Checked="True" ShowCheckBox="False" Text="Patentee" Value="PATENTEE" ImageUrl="~/Images/tick.png"></asp:TreeNode>
                                <asp:TreeNode Checked="True" Expanded="True" ShowCheckBox="False" Text="Patent Inventor" Value="PATENT_INVENTOR" ImageUrl="~/Images/tick.png"></asp:TreeNode>
                            </asp:TreeNode>
                        </Nodes>
                    </asp:TreeView>
                 </td>
            </tr>
             <tr id="TR_NBM" runat="server">
                <td class="style51">Non-Book Materials</td>
                <td class="style52">
                    <asp:TreeView ID="TreeView_NBM" runat="server" ShowCheckBoxes="All" ShowLines="True">
                        <Nodes>
                            <asp:TreeNode Expanded="True" ShowCheckBox="False" Text="Non Book Materials" Value="NBM">
                                <asp:TreeNode ShowCheckBox="True" Text="Producer / Production Year" Value="PRODUCER"></asp:TreeNode>
                                <asp:TreeNode ShowCheckBox="True" Text="Designer" Value="DESIGNER"></asp:TreeNode>
                                <asp:TreeNode ShowCheckBox="True" Text="Manufacturer" Value="MANUFACTURER"></asp:TreeNode>
                                <asp:TreeNode ShowCheckBox="True" Text="Materials Type / Technique" Value="MATERIALS"></asp:TreeNode>
                                <asp:TreeNode ShowCheckBox="True" Text="Work Category / Work Type" Value="WORK_CATEGORY"></asp:TreeNode>
                                <asp:TreeNode ShowCheckBox="True" Text="Creater / Role of Creator" Value="CREATOR"></asp:TreeNode>
                                <asp:TreeNode ShowCheckBox="True" Text="Related Work" Value="RELATED_WORK"></asp:TreeNode>
                                <asp:TreeNode ShowCheckBox="True" Text="Source" Value="SOURCE"></asp:TreeNode>
                                <asp:TreeNode ShowCheckBox="True" Text="Photographer" Value="PHOTOGRAPHER"></asp:TreeNode>
                                <asp:TreeNode ShowCheckBox="True" Text="Nationality" Value="NATIONALITY"></asp:TreeNode>
                            </asp:TreeNode>
                        </Nodes>
                    </asp:TreeView>
                </td>
            </tr>
            <tr id ="TR_ISBN" runat ="server">
                <td class="style51">ISBN/ISSN/ISMN</td>
                <td class="style52">
                    
                    <asp:TreeView ID="TreeView_ISBN" runat="server" 
                        ShowCheckBoxes="All" ShowLines="True">
                        <Nodes>
                            <asp:TreeNode Expanded="True" ShowCheckBox="False" Text="ISBN/ISSN/ISMN" Value="STD_NO">
                                <asp:TreeNode Checked="True" Expanded="True" ShowCheckBox="True" Text="ISBN/ISSN/ISMN" Value="STANDARD_NO"></asp:TreeNode>
                                <asp:TreeNode ShowCheckBox="True" Text="Doc ID" Value="DOC_ID"></asp:TreeNode>
                            </asp:TreeNode>
                        </Nodes>
                    </asp:TreeView>
                 </td>
            </tr>
             <tr  id ="TR_Titles" runat ="server">
                <td class="style51">Title Statement</td>
                <td class="style52">
                      <asp:TreeView ID="TreeView_Titles" runat="server" ShowCheckBoxes="All" 
                          ShowLines="True">
                          <Nodes>
                              <asp:TreeNode Expanded="True" ShowCheckBox="False" 
                                  Text="Title Statements" Value="TITLE_STATEMENTS">
                                  <asp:TreeNode Checked="True" Expanded="True" ShowCheckBox="false" Text="Title" 
                                      Value="TITLE" ImageUrl="~/Images/tick.png"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Sub Title" Value="SUB_TITLE">
                                  </asp:TreeNode>
                                  <asp:TreeNode Expanded="True" ShowCheckBox="True" Text="Variable/Alt Title" 
                                      Value="VAR_TITLE"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Conference Title" Value="CONF_NAME">
                                  </asp:TreeNode>
                                  <asp:TreeNode Text="Conference From Date" Value="CONF_FROM"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Conference To Date" Value="CONF_TO">
                                  </asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Conference Place" Value="CONF_PLACE">
                                  </asp:TreeNode>
                              </asp:TreeNode>
                          </Nodes>
                      </asp:TreeView>
                 </td>
            </tr>
             <tr  id ="TR_Authors" runat ="server">
                <td class="style51">Statement of Responsibility</td>
                <td class="style52">
                      <asp:TreeView ID="TreeView_Authors" runat="server" ShowCheckBoxes="All" 
                          ShowLines="True">
                          <Nodes>
                              <asp:TreeNode Expanded="True" ShowCheckBox="False" 
                                  Text="Statement of Responsibility" Value="RESPONSIBILITIES">
                                  <asp:TreeNode Expanded="True" ShowCheckBox="True" Text="Author1" 
                                      Value="AUTHOR1"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Author2" Value="AUTHOR2"></asp:TreeNode>
                                  <asp:TreeNode Expanded="True" ShowCheckBox="True" Text="Author3" 
                                      Value="AUTHOR3"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Editor" Value="EDITOR"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Translator" Value="TRANSLATOR">
                                  </asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Illustrator" Value="ILLUSTRATORS">
                                  </asp:TreeNode>
                                  <asp:TreeNode Text="Compiler" Value="COMPILER"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Commentators" Value="COMMENTATORS">
                                  </asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Revised By" Value="REVISED_BY">
                                  </asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Chairman (Law Reports/Acts)" 
                                      Value="CHAIRMAN">
                                  </asp:TreeNode>
                                   <asp:TreeNode ShowCheckBox="True" Text="Government (Law Reports/Acts)" 
                                      Value="GOVERNMENT">
                                  </asp:TreeNode>
                              </asp:TreeNode>
                          </Nodes>
                      </asp:TreeView>
                 </td>
            </tr>
             <tr  id ="TR_CorpAuthors" runat ="server">
                <td class="style51">Corporate Author</td>
                <td class="style52">
                      <asp:TreeView ID="TreeView_CorpAuthor" runat="server" ShowCheckBoxes="All" 
                          ShowLines="True">
                          <Nodes>
                              <asp:TreeNode Expanded="True" ShowCheckBox="False" 
                                  Text="Corporate Author" Value="Corporate Author">
                                  <asp:TreeNode Expanded="True" ShowCheckBox="True" 
                                      Text="Corporate Author" Value="CORPORATE_AUTHOR"></asp:TreeNode>
                              </asp:TreeNode>
                          </Nodes>
                      </asp:TreeView>
                 </td>
            </tr>
             <tr  id ="TR_Edition" runat ="server">
                <td class="style51">Edition Statement</td>
                <td class="style52">
                      <asp:TreeView ID="TreeView_Edition" runat="server" ShowCheckBoxes="All" 
                          ShowLines="True" EnableClientScript="False">
                          <Nodes>
                              <asp:TreeNode Expanded="True" ShowCheckBox="False" 
                                  Text="Edition Statement" Value="Edition Statement">
                                  <asp:TreeNode Expanded="True" ShowCheckBox="True" Text="Edition" 
                                      Value="EDITION"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Reprints" Value="REPRINTS">
                                  </asp:TreeNode>
                              </asp:TreeNode>
                          </Nodes>
                      </asp:TreeView>
                 </td>
            </tr>
             <tr  id ="TR_Imprint" runat ="server">
                <td class="style51">Imprint Area</td>
                <td class="style52">
                      <asp:TreeView ID="TreeView_Imprint" runat="server" ShowCheckBoxes="All" 
                          ShowLines="True" EnableClientScript="False">
                          <Nodes>
                              <asp:TreeNode Expanded="True" ShowCheckBox="False" Text="Imprint Information" Value="IMPRINT">
                                  <asp:TreeNode Expanded="True" ShowCheckBox="True" Text="Publisher" Value="PUB_ID" Checked="True"></asp:TreeNode>
                                  <asp:TreeNode Text="Place" Value="PLACE_OF_PUB" ShowCheckBox="True" Checked="True"></asp:TreeNode>
                                  <asp:TreeNode Text="Year" Value="YEAR_OF_PUB" Checked="True" Selected="True" ShowCheckBox="True"></asp:TreeNode>
                              </asp:TreeNode>
                          </Nodes>
                      </asp:TreeView>
                 </td>
            </tr>

             <tr  id ="TR_Series" runat ="server">
                <td class="style51">Series Statement</td>
                <td class="style52">
                      <asp:TreeView ID="TreeView_Sereis" runat="server" ShowCheckBoxes="All" 
                          ShowLines="True">
                          <Nodes>
                              <asp:TreeNode Expanded="True" ShowCheckBox="False" 
                                  Text="Series Statement" Value="Series Statement">
                                  <asp:TreeNode Expanded="True" ShowCheckBox="True" Text="Series Title" 
                                      Value="SERIES_TITLE"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Series Editor" Value="SERIES_EDITOR">
                                  </asp:TreeNode>
                              </asp:TreeNode>
                          </Nodes>
                      </asp:TreeView>
                 </td>
            </tr>
            <tr  id ="TR_Notes" runat ="server">
                <td class="style51">Note Area</td>
                <td class="style52">
                      <asp:TreeView ID="TreeView_Note" runat="server" ShowCheckBoxes="All" 
                          ShowLines="True">
                          <Nodes>
                              <asp:TreeNode Expanded="True" ShowCheckBox="False" 
                                  Text="Note Area" Value="Note Area">
                                  <asp:TreeNode Expanded="True" ShowCheckBox="True" Text="Note" 
                                      Value="NOTE"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Remarks" Value="REMARKS">
                                  </asp:TreeNode>
                                  <asp:TreeNode Text="Reference No" Value="REFERENCE_NO"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="False" ImageUrl="~/Images/tick.png" Text="Multi-Vol?" 
                                      Value="MULTI_VOL" Checked="True" Selected="True"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Total Vol" Value="TOTAL_VOL">
                                  </asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="URL" Value="URL"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Comments" Value="COMMENTS">
                                  </asp:TreeNode>
                              </asp:TreeNode>
                          </Nodes>
                      </asp:TreeView>
                 </td>
            </tr>
             <tr  id ="TR_Abstracts" runat ="server">
                <td class="style51">Abstract &amp; Indexing</td>
                <td class="style52">
                      <asp:TreeView ID="TreeView_Subject" runat="server" ShowCheckBoxes="All" 
                          ShowLines="True">
                          <Nodes>
                              <asp:TreeNode Expanded="True" ShowCheckBox="False" 
                                  Text="Subjet and Keywords" Value="Subjet and Keywords">
                                  <asp:TreeNode Expanded="True" ShowCheckBox="True" Text="Subject" Value="SUB_ID">
                                  </asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Keywords" Value="KEYWORDS">
                                  </asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Abstract" Value="ABSTRACT">
                                  </asp:TreeNode>
                              </asp:TreeNode>
                          </Nodes>
                      </asp:TreeView>
                 </td>
            </tr>
             <tr  id ="TR_Others" runat ="server">
                <td class="style51">Other Fields</td>
                <td class="style52">
                      <asp:TreeView ID="TreeView_Other" runat="server" ShowCheckBoxes="All" 
                          ShowLines="True">
                          <Nodes>
                              <asp:TreeNode Expanded="True" ShowCheckBox="False" 
                                  Text="Other Fields" Value="OTHERS">
                                  <asp:TreeNode Expanded="True" ShowCheckBox="False" 
                                      Text="Country of Publication" Value="CON_CODE" 
                                      ImageUrl="~/Images/tick.png" Checked="True">
                                  </asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Translated From" 
                                      Value="TRANSLATED_FROM">
                                  </asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Act No" 
                                      Value="ACT_NO">
                                  </asp:TreeNode>
                                   <asp:TreeNode ShowCheckBox="True" Text="Year of Act" 
                                      Value="ACT_YEAR">
                                  </asp:TreeNode>
                              </asp:TreeNode>
                          </Nodes>
                      </asp:TreeView>
                 </td>
            </tr>
             <tr  id ="TR_Holdings" runat ="server">
                <td class="style51">Holdings Fields</td>
                <td class="style52">
                      <asp:TreeView ID="TreeView_Holdings" runat="server" ShowCheckBoxes="All" ShowLines="True">
                          <Nodes>
                              <asp:TreeNode Expanded="True" ShowCheckBox="False" Text="Holdings Fields" Value="HOLDINGS">
                                  <asp:TreeNode Expanded="True" ImageUrl="~/Images/tick.png" ShowCheckBox="False" Text="Accession No" Value="ACCESSION_NO" Checked="True"></asp:TreeNode>
                                  <asp:TreeNode Expanded="True" ImageUrl="~/Images/tick.png" ShowCheckBox="False" Text="Accession Date" Value="ACCESSION_DATE" Checked="True"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True"  Text="Pagination" Value="PAGINATION" Checked="True"></asp:TreeNode>
                                  <asp:TreeNode Expanded="True" ImageUrl="~/Images/tick.png" ShowCheckBox="False" Text="Status" Value="STA_CODE" Checked="True"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True"  Text="Format/Medium" Value="FORMAT_CODE" Checked="True"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Class No" Value="CLASS_NO"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Book No" Value="BOOK_NO"></asp:TreeNode>                                  
                                  <asp:TreeNode ShowCheckBox="True" Text="Illus" Value="ILLUSTRATION"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Dimension" Value="SIZE"></asp:TreeNode>
                                  <asp:TreeNode Text="Collection Type" Value="COLLECTION_TYPE" Checked="True"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Location" Value="PHYSICAL_LOCATION"></asp:TreeNode>                                  
                                  <asp:TreeNode ShowCheckBox="True" Text="Binding Type" Value="BIND_CODE" Checked="True"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Accompanying Materials" Value="ACC_MAT_CODE"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Section" Value="SEC_CODE"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Vol ISBN" Value="COPY_ISBN"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Reference No" Value="REFERENCE_NO"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Remarks" Value="REMARKS"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Bound Volume Issue No (From/To)" Value="ISSUE_NO"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Missing Issues" Value="MISSING_ISSUES"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Bound Volume Period (From/To)" Value="PERIOD"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Bound Volume  Year" Value="JYEAR"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Medium" Value="MEDIUM"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Recording Categories" Value="SOUND_CATEGORIES"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Recording Composition Form" Value="RECORDING_FORMS"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Recording Format" Value="RECORDING_FORMATS"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Recording Play Speed" Value="RECORDING_SPEED"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Recording Storage Technology" Value="RECORDING_STORAGE_TECH"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Recording Play Duration" Value="RECORDING_PLAY_DURATION"></asp:TreeNode>                                 
                                  <asp:TreeNode ShowCheckBox="True" Text="Type of Visuals" Value="VIDEO_TYPEOFVISUAL"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Color" Value="VIDEO_COLOR"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Playback Channel Type" Value="PLAYBACK_CHANNELS"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Tape Width" Value="TAPE_WIDTH"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Tape Configuration" Value="TAPE_CONFIGURATION"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Disk Type" Value="KIND_OF_DISK"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Disk Cutting Type" Value="KIND_OF_CUTTING"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Encoding Standard/Technology" Value="ENCODING_STANDARD"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Capture Technique" Value="CAPTURE_TECHNIQUE"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Cartographic Medium" Value="CARTOGRAPHIC_MEDIUM"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Cartographic Co-Ordinates" Value="CARTOGRAPHIC_COORDINATES"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Geographic Location" Value="CARTOGRAPHIC_GEOGRAPHIC_LOCATION"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Data Gathering Date" Value="CARTOGRAPHIC_DATAGATHERING_DATE"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Creation Date" Value="CREATION_DATE"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Compilation Date" Value="CARTOGRAPHIC_COMPILATION_DATE"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Inspection Date" Value="CARTOGRAPHIC_INSPECTION_DATE"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Scale" Value="CARTOGRAPHIC_SCALE"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Projection" Value="CARTOGRAPHIC_PROJECTION"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Photo No" Value="PHOTO_NO"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Album No" Value="PHOTO_ALBUM_NO"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Ocasion" Value="PHOTO_OCASION"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Image View Type" Value="IMAGE_VIEW_TYPE"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="View Date" Value="VIEW_DATE"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Theme" Value="THEME"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Style" Value="STYLE"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Culture" Value="CULTURE"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Current Site" Value="CURRENT_SITE"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Creation Site" Value="CREATION_SITE"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Yarn Count" Value="YARN_COUNT"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Material Type" Value="VIDEO_COLOR"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Technique" Value="TECHNIQUE"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Technical Details" Value="TECH_DETAILS"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Inscriptions" Value="INSCRIPTIONS"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Description" Value="DESCRIPTION"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Globe Type" Value="GLOBE_TYPE"></asp:TreeNode>
                                  <asp:TreeNode ShowCheckBox="True" Text="Alter Date" Value="ALTER_DATE"></asp:TreeNode>

                              </asp:TreeNode>
                          </Nodes>
                      </asp:TreeView>
                 </td>
            </tr>
           
            <tr>
                <td class="style47" colspan="2">
                    &nbsp;</td>
            </tr>
            
            <tr>
                <td  bgcolor="#99CCFF" class="style44" colspan="2" rowspan="0">
                     <strong>*<span class="style48">Mandatory Fields</span></strong></td>
            </tr>
         </table>

                    
                   </ContentTemplate>
                   <Triggers>
                                                   
                   </Triggers>
                 </asp:UpdatePanel>
       
        <table id="Table3" runat="server" cellspacing="2" border="1" class="style35" 
         cellpadding="1"  style="vertical-align: middle;"         width="100%" 
         align="center" >
            <tr>
                <td class="style47" colspan="2">
                     
                 </td>
            </tr>
       </table>


       <asp:Panel ID="ModalPanel1" runat="server" Style="display: none;" CssClass="modalPopup">
      
       <table id="Table2" runat="server" cellspacing="2" border="1"  
         cellpadding="1"  style="vertical-align: middle;"         width="100%" 
         align="center"  >
            <tr>
                <td class="style47" colspan="2">
                     <asp:Label ID="Label1" runat="server" Font-Size="Large">Do You wish to Delete this Record?</asp:Label>
                 </td>
            </tr>
            
            <tr>
                <td class="style53">Code *</td>
                <td class="style52">
                    <asp:Label ID="Label7" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style53">Name*</td>
                <td class="style52">
                    <asp:Label ID="Label8" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style53">Remarks</td>
                <td class="style52">
                    <asp:Label ID="Label9" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
           
            
            
            
        </table>

      </asp:Panel> 

      
    
      
    
</asp:Content>
