<%@ Page  Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="BasicSearch.aspx.vb" Inherits="EG4.BasicSearch" %>


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
            color:DodgerBlue;  
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
    <script type="text/javascript" src="http://www.google.com/jsapi"></script> 
 <script type="text/javascript">
     // Load the Google Transliteration API   

     google.load("elements", "1", {
         packages: "transliteration"
     });
     var ids = ""
     var t1 = ""
     var k=""
     function onLoad() {
         var options = {
             sourceLanguage: 'en',
             destinationLanguage: ['hi', 'bn', 'gu', 'kn', 'ml', 'mr', 'pa', 'sa', 'ta', 'te', 'ur'],
             shortcutKey: 'ctrl+g',
             transliterationEnabled: false
         };
         // Create an instance on TransliterationControl with the required              
         var control = new google.elements.transliteration.TransliterationControl(options);
         var elem = "MainContent_txt_Cat_Title,MainContent_txt_Cat_VarTitle,MainContent_txt_Cat_SubTitle,MainContent_txt_Cat_GuideName,MainContent_txt_Cat_ScholarName,MainContent_txt_Cat_ScholarDept,MainContent_txt_Cat_GuideDept,MainContent_txt_Cat_DegreeName,MainContent_txt_Cat_Patentee,MainContent_txt_Cat_PatentInventor,MainContent_txt_Cat_ConfName,MainContent_txt_Cat_Author1,MainContent_txt_Cat_Author2,MainContent_txt_Cat_Author3,MainContent_txt_Cat_Editor,MainContent_txt_Cat_Translator,MainContent_txt_Cat_Illustrator,MainContent_txt_Cat_Compiler,MainContent_txt_Cat_Commentator,MainContent_txt_Cat_RevisedBy,MainContent_txt_Cat_CorpAuthor,MainContent_txt_Cat_Place,MainContent_txt_Cat_Note,MainContent_txt_Cat_Remarks,MainContent_txt_Cat_Comments,MainContent_txt_Cat_Keywords,MainContent_txt_Cat_TrFrom,MainContent_txt_Cat_Abstract, MainContent_txt_Hold_Remarks";
         //alert(elements);
         var e = elem.split(",");
         var idArray = new Array();
         //alert(e);
         for (var j = 0; j < e.length; j++) {
             //alert(e[j]);
             var docs = document.getElementById(e[j]);
             //alert(docs);
             if (docs != null) {
                 idArray.push(e[j]);
             }
         }

//         ids = ["MainContent_txt_Search", "MainContent_txt_Cat_Title"];

           control.makeTransliteratable(idArray);
         // Show the transliteration control which can be used to toggle between         
         // English and Hindi and also choose other destination language.         
         control.showControl('translControl');
     }

     google.setOnLoadCallback(onLoad);
           
    </script>

 <script type="text/javascript" src="http://www.google.com/jsapi"></script> 
  <script type ="text/javascript">
      function suppressNonEng(event) {
          var chCode = ('charCode' in event) ? event.charCode : event.keyCode;
          if (49 <= chCode && chCode <= 57) {
              return (true);
          }
          if (97 <= chCode && chCode <= 122) {
              return (true);
          }

          if (chCode == 0 || chCode == 13 || chCode == 32) {
              return (true);
          }

          else {
              alert("Please Enter ENG Only Characters!");
              document.getElementById("MainContent_txt_Cat_TotalVol").focus();
              return (false);
          }
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
    <script type ="text/javascript">
       function isCurrencyKey(evt) {
           var charCode = (evt.which) ? evt.which : event.keyCode
           if (charCode != 46 && (charCode < 48 || charCode > 57) && charCode != 32) {
               alert("Please Enter Numeric with Decimal Characters Only!");
               return false;
           }

           return true;
       }

   </script>
    <script language="javascript" type="text/javascript">

         function valid1() {
            var bibtype = "";
            var mattype = "";
            var doctype = "";
            var lang = "";
            var yesno = "";
            var pub = "";
            var country = "";
            var currentyear = "";
   
            bibtype = document.getElementById('<%=DDL_Bib_Level.ClientID%>').value;
            mattype = document.getElementById('<%=DDL_Mat_Type.ClientID%>').value;
            doctype = document.getElementById('<%=DDL_Doc_Type.ClientID%>').value;
            lang = document.getElementById('<%=DDL_Lang.ClientID%>').value;
            yesno = document.getElementById('<%=DDL_YesNo.ClientID%>').value;
            currentyear = new Date().getFullYear()+1;
           
            
            if (bibtype == "") {
                alert("Please Select  \"Bibliographic Level \" field.");
                document.getElementById("MainContent_DDL_Bib_Level").focus();
                return (false);
            }
            if (mattype == "") {
                alert("Please Select  \"Material Type \" field.");
                document.getElementById("MainContent_DDL_Mat_Type").focus();
                return (false);
            }
            if (doctype == "") {
                alert("Please Select  \"Document Type  \" field.");
                document.getElementById("MainContent_DDL_Doc_Type").focus();
                return (false);
            }
            if (lang == "") {
                alert("Please Select  \"Language \" field.");
                document.getElementById("MainContent_DDL_Lang").focus();
                return (false);
            }
            if (yesno == "") {
                alert("Please Select  \"Value from (Y/N) \" field.");
                document.getElementById("MainContent_DDL_YesNo").focus();
                return (false);
            }

            if (document.getElementById('<%=txt_Cat_Title.ClientID%>').value == "") {
                alert("Please enter proper \"Title\" field.");
                document.getElementById("MainContent_txt_Cat_Title").focus();
                return (false);
            }
            if (document.getElementById('<%=txt_Cat_Title.ClientID%>').value.length < 3) {
                alert("Length of \"Title\" should be Min 3 characters.");
                document.getElementById("MainContent_txt_Cat_Title").focus();
                return (false);
            }

            if (document.getElementById('<%=txt_Hold_AccNo.ClientID%>').value == "") {
                alert("Please enter proper \"Accession No\" field.");
                document.getElementById("MainContent_txt_Hold_AccNo").focus();
                return (false);
            }
            if (document.getElementById('<%=txt_Hold_AccDate.ClientID%>').value == "") {
                alert("Please enter proper \"Accession Date\" field.");
                document.getElementById("MainContent_txt_Hold_AccDate").focus();
                return (false);
            }
//            if (document.getElementById('<%=DDL_Countries.ClientID%>').value == "") {
//                alert("Please enter proper \"Country of Publication\" field.");
//                document.getElementById("MainContent_DDL_Countries").focus();
//                return (false);
//            }
            if (document.getElementById('<%=DDL_Status.ClientID%>').value == "") {
                alert("Please enter proper \"Status\" field.");
                document.getElementById("MainContent_DDL_Status").focus();
                return (false);
            }



            return (true);
        }

    </script>
      <script language="javascript" type="text/javascript">

          function valid2() {
              var bibtype = "";
              var mattype = "";
              var doctype = "";
              var lang = "";
              var yesno = "";
              var pub = "";
              var country = "";
              var currentyear = "";

              bibtype = document.getElementById('<%=DDL_Bib_Level.ClientID%>').value;
              mattype = document.getElementById('<%=DDL_Mat_Type.ClientID%>').value;
              doctype = document.getElementById('<%=DDL_Doc_Type.ClientID%>').value;
              lang = document.getElementById('<%=DDL_Lang.ClientID%>').value;
              yesno = document.getElementById('<%=DDL_YesNo.ClientID%>').value;
              currentyear = new Date().getFullYear() + 1;


              if (bibtype == "") {
                  alert("Please Select  \"Bibliographic Level \" field.");
                  document.getElementById("MainContent_DDL_Bib_Level").focus();
                  return (false);
              }
              if (mattype == "") {
                  alert("Please Select  \"Material Type \" field.");
                  document.getElementById("MainContent_DDL_Mat_Type").focus();
                  return (false);
              }
              if (doctype == "") {
                  alert("Please Select  \"Document Type  \" field.");
                  document.getElementById("MainContent_DDL_Doc_Type").focus();
                  return (false);
              }
              if (lang == "") {
                  alert("Please Select  \"Language \" field.");
                  document.getElementById("MainContent_DDL_Lang").focus();
                  return (false);
              }
              if (yesno == "") {
                  alert("Please Select  \"Value from (Y/N) \" field.");
                  document.getElementById("MainContent_DDL_YesNo").focus();
                  return (false);
              }

              if (document.getElementById('<%=txt_Cat_Title.ClientID%>').value == "") {
                  alert("Please enter proper \"Title\" field.");
                  document.getElementById("MainContent_txt_Cat_Title").focus();
                  return (false);
              }
              if (document.getElementById('<%=txt_Cat_Title.ClientID%>').value.length < 3) {
                  alert("Length of \"Title\" should be Min 3 characters.");
                  document.getElementById("MainContent_txt_Cat_Title").focus();
                  return (false);
              }

           



//              if (document.getElementById('<%=txt_Hold_AccNo.ClientID%>').value == "") {
//                  alert("Please enter proper \"Accession No\" field.");
//                  document.getElementById("MainContent_txt_Hold_AccNo").focus();
//                  return (false);
//              }
//              if (document.getElementById('<%=txt_Hold_AccDate.ClientID%>').value == "") {
//                  alert("Please enter proper \"Accession Date\" field.");
//                  document.getElementById("MainContent_txt_Hold_AccDate").focus();
//                  return (false);
//              }
//              if (document.getElementById('<%=DDL_Countries.ClientID%>').value == "") {
//                  alert("Please enter proper \"Country of Publication\" field.");
//                  document.getElementById("MainContent_DDL_Countries").focus();
//                  return (false);
//              }
//              if (document.getElementById('<%=DDL_Status.ClientID%>').value == "") {
//                  alert("Please enter proper \"Status\" field.");
//                  document.getElementById("MainContent_DDL_Status").focus();
//                  return (false);
//              }



              return (true);
          }

    </script>
     <script language ="javascript" type ="text/javascript" >
         function Select(Select) {

             var grdv = document.getElementById('<%= Grid2.ClientID %>');
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
         function Select(Select) {

             var grdv = document.getElementById('<%= Grid1_Search.ClientID %>');
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


    <script type="text/javascript">
        function formfocus() {
            document.getElementById('<%= DDL_Bib_Level.ClientID %>').focus();
        }
        window.onload = formfocus;
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
               document.getElementById("MainContent_txt_Cat_SDate").focus();
               return (false);
           }
       }
    </script>
    <script type ="text/javascript">
        //alpha-numeric only
        function DateOnly1(event) {
            var chCode = ('charCode' in event) ? event.charCode : event.keyCode;
            if (47 <= chCode && chCode <= 57) {
                return (true);
            }

            else {
                alert("Date Is Invalid, Enter in dd/MM/yyyy format only!");
                document.getElementById("MainContent_txt_Cat_EDate").focus();
                return (false);
            }
        }
    </script>

     <script type ="text/javascript">
         //alpha-numeric only
         function suppressNonEng(event) {
             var chCode = ('charCode' in event) ? event.charCode : event.keyCode;
             if (48 <= chCode && chCode <= 57) {
                 return (true);
             }
             if (97 <= chCode && chCode <= 122) {
                 return (true);
             }

             if (chCode == 0 || chCode == 13 || chCode == 32) {
                 return (true);
             }

             else {
                 alert("Please Enter ENG Only Characters!");
                 document.getElementById("MainContent_txt_Hold_AccNo").focus();
                 return (false);
             }
         }
    </script>
     <script type ="text/javascript">
         //alpha-numeric only
         function EngOnly(event) {
             var chCode = ('charCode' in event) ? event.charCode : event.keyCode;
             if (65 <= chCode && chCode <= 90) {
                 return (true);
             }
             if (97 <= chCode && chCode <= 122) {
                 return (true);
             }

             if (chCode == 0 || chCode == 13 || chCode == 32) {
                 return (true);
             }

             else {
                 alert("Please Enter ENG Only Characters!");
                 document.getElementById("MainContent_txt_Hold_AccNo").focus();
                 return (false);
             }
         }
    </script>
    <script type ="text/javascript">
        //alpha-numeric only
        function NumericOnly(event) {
            var chCode = ('charCode' in event) ? event.charCode : event.keyCode;
            if (48 <= chCode && chCode <= 57) {
                return (true);
            }

            else {
                alert("Plz Enter Digits Only in yyyy format!");
                document.getElementById("MainContent_txt_Hold_VolYear").focus();
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
                document.getElementById("MainContent_txt_Hold_CreationDate").focus();
                return (false);
            }
        }
    </script>
    <script type ="text/javascript">
        //alpha-numeric only
        function DateOnly3(event) {
            var chCode = ('charCode' in event) ? event.charCode : event.keyCode;
            if (47 <= chCode && chCode <= 57) {
                return (true);
            }

            else {
                alert("Date Is Invalid, Enter in dd/MM/yyyy format only!");
                document.getElementById("MainContent_txt_Hold_CompilationDate").focus();
                return (false);
            }
        }
    </script>
    <script type ="text/javascript">
        //alpha-numeric only
        function DateOnly4(event) {
            var chCode = ('charCode' in event) ? event.charCode : event.keyCode;
            if (47 <= chCode && chCode <= 57) {
                return (true);
            }

            else {
                alert("Date Is Invalid, Enter in dd/MM/yyyy format only!");
                document.getElementById("MainContent_txt_Hold_InspectionDate").focus();
                return (false);
            }
        }
    </script>

     <script type ="text/javascript">
         //alpha-numeric only
         function DateOnly5(event) {
             var chCode = ('charCode' in event) ? event.charCode : event.keyCode;
             if (47 <= chCode && chCode <= 57) {
                 return (true);
             }

             else {
                 alert("Date Is Invalid, Enter in dd/MM/yyyy format only!");
                 document.getElementById("MainContent_txt_Hold_ViewDate").focus();
                 return (false);
             }
         }
    </script>

     <script type ="text/javascript">
         //alpha-numeric only
         function DateOnly6(event) {
             var chCode = ('charCode' in event) ? event.charCode : event.keyCode;
             if (47 <= chCode && chCode <= 57) {
                 return (true);
             }

             else {
                 alert("Date Is Invalid, Enter in dd/MM/yyyy format only!");
                 document.getElementById("MainContent_txt_Hold_AlterDate").focus();
                 return (false);
             }
         }
    </script>
    <script type ="text/javascript">
        //alpha-numeric only
        function DateOnly7(event) {
            var chCode = ('charCode' in event) ? event.charCode : event.keyCode;
            if (47 <= chCode && chCode <= 57) {
                return (true);
            }

            else {
                alert("Date Is Invalid, Enter in dd/MM/yyyy format only!");
                document.getElementById("MainContent_txt_Hold_AccDate").focus();
                return (false);
            }
        }
    </script>
    <script type ="text/javascript">
        //alpha-numeric only
        function DateOnly8(event) {
            var chCode = ('charCode' in event) ? event.charCode : event.keyCode;
            if (47 <= chCode && chCode <= 57) {
                return (true);
            }

            else {
                alert("Date Is Invalid, Enter in dd/MM/yyyy format only!");
                document.getElementById("MainContent_txt_Hold_DataGatheringDate").focus();
                return (false);
            }
        }
    </script>
     <script type="text/javascript">
         function EngOnlyInput(event) {
             var chCode = ('charCode' in event) ? event.charCode : event.keyCode;
             //if (chCode < 48 /* '0' */ || chCode > 57 /* '9' */) {
             //alert(chCode);
             if (chCode > 64 || chCode == 0 || chCode == 13 || chCode == 32) {
                 if (chCode < 91 || chCode == 0 || chCode == 13 || chCode == 32) {
                     return (true);
                 }
                 else if (chCode > 96 || chCode == 0 || chCode == 13 || chCode == 32) {
                     if (chCode < 123 || chCode == 0 || chCode == 13 || chCode == 32) {
                         return (true);
                     }
                     else {
                         alert("Please Enter ENG Only Characters!");
                         document.getElementById("MainContent_txt_Hold_AccSereis").focus();
                         return (false);
                     }
                 }
                 else {
                     alert("Please Enter ENG Only Characters!");
                     document.getElementById("MainContent_txt_Hold_AccSereis").focus();
                     return (false);
                 }
             }
             else {
                 alert("Please Enter ENG Only Characters!");
                 document.getElementById("MainContent_txt_Hold_AccSereis").focus();
                 return (false);
             }
         }
    </script>
    
     
        <table id="ADMT1" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
            <tr>
                <td  bgcolor="#003366" class="style43" rowspan="1" style="color: #FFFFFF"><strong>Basic Search</strong></td>
            </tr>
            <tr>  
                <td  bgcolor="#99CCFF"  colspan="2" style="text-align: center">
                 
                     Type in Indian languages (Press Ctrl+g to toggle between English and India Language)
                     <div id='translControl'></div>
                </td>
            </tr>

             <tr>                
                <td  align="center">         
                                       
                    Search Text&nbsp;
                    <asp:TextBox ID="txt_Search" runat="server" MaxLength="100"  
                        ToolTip="Enter Search Term" Wrap="False" AccessKey="r" 
                        Font-Bold="True" ForeColor="#0066FF" Width="250px"></asp:TextBox>
                    &nbsp;In: 
                    <asp:DropDownList ID="DropDownList1" runat="server" ForeColor="#0066FF">
                        <asp:ListItem Selected="True" Value="TITLE">Title</asp:ListItem>
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
                    </asp:DropDownList>&nbsp;Operator: 
                    <asp:DropDownList ID="DropDownList2" runat="server" ForeColor="#0066FF">
                        <asp:ListItem Value="AND" Selected="True">All Words</asp:ListItem>
                        <asp:ListItem Value="OR">Any Word</asp:ListItem>
                        <asp:ListItem Value="LIKE">Like</asp:ListItem>
                        <asp:ListItem Value="SW">Start With</asp:ListItem>
                        <asp:ListItem Value="EW">End With</asp:ListItem>
                    </asp:DropDownList> &nbsp;Order By: 
                    <asp:DropDownList ID="DropDownList3" runat="server" ForeColor="#0066FF">
                        <asp:ListItem Selected="True" Value="TITLE">Title</asp:ListItem>
                        <asp:ListItem Value="PUB_NAME">Publisher</asp:ListItem>
                        <asp:ListItem  Value="YEAR_OF_PUB">Year</asp:ListItem>
                        <asp:ListItem Value="CAT_NO">Cat No</asp:ListItem>
                    </asp:DropDownList> &nbsp;Sort By: 
                     <asp:DropDownList ID="DropDownList4" runat="server" ForeColor="#0066FF">
                        <asp:ListItem Selected="True" Value="ASC">Ascending</asp:ListItem>
                        <asp:ListItem Value="DESC">Descending</asp:ListItem>
                    </asp:DropDownList> 
                    <asp:Button ID="Search_Bttn" runat="server"  CssClass="styleBttn" Font-Bold="True" 
                        ForeColor="Red" TabIndex="14" Text="Search" AccessKey="s"     Width="74px" />

                         
                    <br />
                    <br />
                    <hr />

                    
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>

                     <asp:Label ID="Label1" runat="server" Text="Record(s): "></asp:Label>
                   <div>
                   </div>
                                           
                        <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Height="200px">
                        
                   <asp:GridView ID="Grid1_Search" runat="server" AllowPaging="True" DataKeyNames="CAT_NO"    
                        style="width: 100%;  text-align: center;" allowsorting="True" 
                        AutoGenerateColumns="False" PageSize="100"  Font-Bold="True" 
                        Font-Names="Tahoma" Font-Size="8pt" Height="100px"  
                        HorizontalAlign="Center"   ShowFooter="True">
                        <Columns >                
                            <asp:TemplateField HeaderText="S.N.">
                                <ItemTemplate>
                                    <asp:Label ID="lblsr" runat="server" CssClass="MBody"  SkinID="" width="25px"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                                <ItemStyle ForeColor="#336699" Font-Names="Tahoma" Width="25px" 
                                    Font-Bold="True" Font-Size="Small" />
                            </asp:TemplateField>
                    
                   

                            <asp:ButtonField HeaderText="Edit"  Text="Edit" CommandName="Select" 
                                CausesValidation="True">
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" />
                            <ItemStyle ForeColor="#CC0000" Width="20px" Font-Bold="true" />
                            </asp:ButtonField>
                    
                   

                    <asp:BoundField   DataField="TITLE" HeaderText="Title" SortExpression="TITLE">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" Width="650px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="850px" Font-Names="Tahoma"/>                        
                    </asp:BoundField>
                                      
                    <asp:BoundField   DataField="CAT_NO" SortExpression="CAT_NO" HeaderText="Cat No" visible="true">                                               
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="20px" />
                        <ItemStyle forecolor="#0066FF" horizontalalign="Left" width="50px" Font-Names="Arial"/>                        
                    </asp:BoundField>
                     
                    <asp:TemplateField  ControlStyle-Width="10px"  HeaderText="Delete" FooterText="Select to Delete" ShowHeader="true">
                        <ItemTemplate>
                            <asp:CheckBox ID="cbd"  runat="server" />
                        </ItemTemplate>

                        <HeaderTemplate>
                            <asp:ImageButton ID="ImageButton1" runat="server" Height="16px" Width="16px" ToolTip="Select All" ImageUrl="~/Images/check_all.gif" onClientclick="return Select(true)"  />
                            <asp:ImageButton ID="ImageButton2" runat="server" Height="16px" Width="16px" ToolTip="Deselect All" ImageUrl="~/Images/uncheck_all.gif"  OnClientClick ="return Select(false)" />
                        </HeaderTemplate>

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
                        <asp:AsyncPostBackTrigger ControlID="Search_Bttn" EventName="Click" />   
                        <asp:AsyncPostBackTrigger ControlID="Grid1_Search" EventName="RowCommand" />                                              
                   </Triggers>
                    </asp:UpdatePanel>                          
                           
                                                                 
                </td>

            </tr>    


            
           
        </table>


        
         <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
              <ContentTemplate>

         <table id="Table1" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
         <tr>
                <td class="style56" colspan="14" bgcolor="#003366">
                   <asp:Label ID="Label2" runat="server" Font-Size="Small" ForeColor="White" 
                        Font-Bold="True">Display Record: Type Cat No / ISBN /Accession No or Search in the Above Pane</asp:Label>
                </td>
         </tr>
         

             <tr style=" color:Green">
                <td class="style53"> 
                    Display Record:</td>
                <td class="style54" colspan="4">
                    <asp:TextBox ID="txt_Display_Value" runat="server" 
                        AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                        Height="18px" MaxLength="15" ToolTip="Enter Data and press ENTER to Display Record" 
                        Width="44%" Wrap="False" AutoPostBack="True"></asp:TextBox> 
                    <asp:DropDownList ID="DDL_Display" runat="server" 
                        Font-Bold="True" ForeColor="#0066CC" 
                        ToolTip="Plz Select Value from Drop-Down">
                        <asp:ListItem Selected="True" Value="CAT_NO">Cat No</asp:ListItem>
                        <asp:ListItem Value="STANDARD_NO">ISBN</asp:ListItem>
                        <asp:ListItem Value="ACCESSION_NO">Accession No</asp:ListItem>
                    </asp:DropDownList>
                 </td>
                <td class="style54" colspan="5">
                    <asp:Label ID="Label14" runat="server" Text="Press ENTER after Typing the Data" 
                        style="text-align: left" Width="100%"></asp:Label>
                 </td>
                <td class="style54" colspan="4">
                    <asp:Label ID="Label_CatLevel" runat="server" style="text-align: left" 
                        Width="100%"></asp:Label>
                 </td>
                
            </tr>
            
            <tr>
                <td class="style56" colspan="14">
                   <asp:Label ID="Label6" runat="server" Font-Size="Medium" ForeColor="Yellow" 
                        Font-Bold="True"></asp:Label>
                    <asp:Label ID="Label15" runat="server" Font-Bold="True" Font-Size="Medium" 
                        ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr align="left">
                <td class="style56" colspan="14" align="left">
                   <asp:Label ID="Label3" runat="server" Font-Size="Medium" ForeColor="White" 
                        Font-Bold="True" Text="CATALOGING DATA"></asp:Label>
                </td>
            </tr>
            
            <tr>
                <td class="style53"> 
                    Document Category*</td>
                <td class="style54" colspan="4">
                    <asp:DropDownList ID="DDL_Bib_Level" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC" AutoPostBack="True" 
                        ToolTip="Plz Select Value from Drop-Down">
                    </asp:DropDownList>
                    &nbsp;<asp:Label ID="Label7" runat="server" Text=""></asp:Label>
                </td>
                <td class="style54" colspan="5">
                    <asp:DropDownList ID="DDL_Mat_Type" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC" AutoPostBack="True" 
                        ToolTip="Plz Select Value from Drop-Down">
                    </asp:DropDownList>
                </td>
                <td class="style54" colspan="4">
                    <asp:DropDownList ID="DDL_Doc_Type" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC" AutoPostBack="True" 
                        ToolTip="Plz Select Value from Drop-Down" Width="98%">
                    </asp:DropDownList>
                </td>
                
            </tr>
            <tr id ="TR_LANG" runat="server">
                <td class="style53"> 
                    Language*</td>
                <td class="style54" colspan="4">
                    <asp:DropDownList ID="DDL_Lang" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC" ToolTip="Plz Select Value from Drop-Down">
                    </asp:DropDownList>
                </td>
                <td class="style54" colspan="5">
                    Multi-Vol/Part?* Y/N:&nbsp;
                    <asp:DropDownList ID="DDL_YesNo" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC" ToolTip="Plz Select Value from Drop-Down" 
                        AutoPostBack="True">
                        <asp:ListItem>Y</asp:ListItem>
                        <asp:ListItem Selected="True">N</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="style54" colspan="4">
                    <asp:Label ID="Label8" runat="server" Text="Total Vol:  "></asp:Label>
                    <asp:TextBox ID="txt_Cat_TotalVol" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="19px" MaxLength="4" 
                        ToolTip="Enter Total No of Volumes" Width="25px" Wrap="False" onkeypress="return isNumberKey(event)"></asp:TextBox>
                    <asp:Label ID="Label12" runat="server" Font-Size="Smaller"></asp:Label>
                </td>
              
            </tr>
             <tr id ="TR_ISBN" runat="server">
                <td class="style53"> 
                    ISBN</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_ISBN" runat="server" AutoCompleteType="DisplayName"  
                        Font-Bold="True" ForeColor="#0066FF" Height="20px" MaxLength="50" 
                        ToolTip="Enter ISBN" Width="20%" Wrap="False" AutoPostBack="True"></asp:TextBox>
                        <ajaxToolkit:AutoCompleteExtender ID="txt_Cat_ISBN_AutoCompleteExtender" 
                         runat="server" CompletionSetCount="10" EnableCaching="true" Enabled="True" 
                         FirstRowSelected="false" MinimumPrefixLength="1" ServiceMethod="SearchISBN" 
                         TargetControlID="txt_Cat_ISBN">
                     </ajaxToolkit:AutoCompleteExtender>
                    <asp:Label ID="Label9" runat="server" Font-Size="Smaller" 
                        Text="Ex.: 978-81-8274-383-0"></asp:Label>
                    <asp:CheckBox ID="CheckBox2" runat="server" 
                        Text="Download Record from Internet" />
                    <asp:DropDownList ID="ComboBox2" runat="server" Font-Bold="True" 
                        ForeColor="#0066CC" Enabled="False">
                        <asp:ListItem Selected="True">All</asp:ListItem>
                        <asp:ListItem>http://catalog.loc.gov</asp:ListItem>
                        <asp:ListItem>http://worldcat.org</asp:ListItem>
                        <asp:ListItem>http://classic.isbn.nu</asp:ListItem>
                        <asp:ListItem>http://icarlibrary.nic.in</asp:ListItem>
                        <asp:ListItem>http://niclibraries.nic.in</asp:ListItem>
                        <asp:ListItem>http://zsilibraries.nic.in</asp:ListItem>
                        <asp:ListItem>http://bsilibraries.nic.in</asp:ListItem>
                        <asp:ListItem>http://moflibraries.nic.in</asp:ListItem>
                        <asp:ListItem>http://bprdlibrary.nic.in</asp:ListItem>
                        <asp:ListItem>http://124.124.221.8:81/opac</asp:ListItem>
                        <asp:ListItem>http://tsu.trp.nic.in/courtopac</asp:ListItem>
                        <asp:ListItem>http://kvblibrary.nic.in</asp:ListItem>
                        <asp:ListItem>http://shillong.nic.in/necopac</asp:ListItem>
                        <asp:ListItem>http://www.eglibnet.nic.in</asp:ListItem>
                    </asp:DropDownList>
                 </td>
                
            </tr>
            <tr id="TR_SPNo" runat="server">
                <td class="style53"> 
                    Standard Number</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_SPNo" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="100" 
                        ToolTip="Enter Standard Number" Width="99%" Wrap="False"></asp:TextBox>
                          <ajaxToolkit:AutoCompleteExtender ID="txt_Cat_SPNo_AutoCompleteExtender" 
                         runat="server" CompletionSetCount="10" EnableCaching="true" Enabled="True" 
                         FirstRowSelected="false" MinimumPrefixLength="1" ServiceMethod="SearchSP" 
                         TargetControlID="txt_Cat_SPNo">
                     </ajaxToolkit:AutoCompleteExtender>
                </td>
               
            </tr>
             <tr id="TR_ManualNo" runat="server">
                <td class="style53"> 
                    Manual Number</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_ManualNo" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="100" 
                        ToolTip="Enter Manual No" Width="200px" Wrap="False"></asp:TextBox>
                         <ajaxToolkit:AutoCompleteExtender ID="txt_Cat_ManualNo_AutoCompleteExtender" 
                         runat="server" CompletionSetCount="10" EnableCaching="true" Enabled="True" 
                         FirstRowSelected="false" MinimumPrefixLength="1" ServiceMethod="SearchManual" 
                         TargetControlID="txt_Cat_ManualNo">
                     </ajaxToolkit:AutoCompleteExtender>
                 </td>
               
            </tr>
             <tr id="TR_ReportNo" runat="server">
                <td class="style53"> 
                    Report Number</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_ReportNo" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="100" 
                        ToolTip="Enter Report Number" Width="200px" Wrap="False"></asp:TextBox>
                         <ajaxToolkit:AutoCompleteExtender ID="txt_Cat_ReportNo_AutoCompleteExtender" 
                         runat="server" CompletionSetCount="10" EnableCaching="true" Enabled="True" 
                         FirstRowSelected="false" MinimumPrefixLength="1" ServiceMethod="SearchReport" 
                         TargetControlID="txt_Cat_ReportNo">
                     </ajaxToolkit:AutoCompleteExtender>
                 </td>
               
            </tr>

             <tr id="TR_ACT" runat="server">
                <td class="style53"> 
                    Act No</td>
                <td class="style54" colspan="7">
                    <asp:TextBox ID="txt_Cat_ActNo" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="22px" MaxLength="150" 
                        ToolTip="Enter Act No" Width="350px" Wrap="False"></asp:TextBox>
                         <ajaxToolkit:AutoCompleteExtender ID="txt_Cat_ActNo_AutoCompleteExtender" 
                         runat="server" CompletionSetCount="10" EnableCaching="true" Enabled="True" 
                         FirstRowSelected="false" MinimumPrefixLength="1" ServiceMethod="SearchAct" 
                         TargetControlID="txt_Cat_ActNo">
                     </ajaxToolkit:AutoCompleteExtender>
                  </td>
                 <td class="style54" colspan="6">
                     Year of Act:
                      <ajaxToolkit:ListSearchExtender 
                        ID="ListSearchExtender3" 
                        runat="server" 
                        Enabled="True" 
                        PromptCssClass="PromptCSS" 
                        TargetControlID="DDL_Countries">
                    </ajaxToolkit:ListSearchExtender>
                     <asp:TextBox ID="txt_Cat_ActYear" runat="server" AutoCompleteType="DisplayName" 
                         Font-Bold="True" ForeColor="#0066FF" Height="16px" MaxLength="4" 
                         onkeypress="return isNumberKey(event)" 
                         ToolTip="Enter Year in YYYY format" Width="10%" Wrap="False"></asp:TextBox>
                     <ajaxToolkit:AutoCompleteExtender ID="txt_Cat_ActYear_AutoCompleteExtender" 
                         runat="server" CompletionSetCount="10" EnableCaching="true" Enabled="True" 
                         FirstRowSelected="false" MinimumPrefixLength="1" ServiceMethod="SearchYear" 
                         TargetControlID="txt_Cat_ActYear">
                     </ajaxToolkit:AutoCompleteExtender>
                     
                 </td>
               
            </tr>
             <tr id="TR_TITLE" runat="server">
                <td class="style53"> 
                    Title *</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_Title" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" ToolTip="Enter Title Proper" 
                        Width="99%" Wrap="False"></asp:TextBox>
                    <ajaxToolkit:AutoCompleteExtender 
                        ID="txt_Cat_Title_AutoCompleteExtender" 
                        runat="server" 
                        MinimumPrefixLength="1"
                        EnableCaching="true" 
                        CompletionSetCount="10"
                        ServiceMethod="SearchTitle"
                        TargetControlID="txt_Cat_Title" 
                        FirstRowSelected = "false">
                    </ajaxToolkit:AutoCompleteExtender>
                 </td>
                
            </tr>
             <tr id="TR_SUBTITLE" runat="server">
                <td class="style53"> 
                    Sub Title</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_SubTitle" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter Sub Title" Width="99%" Wrap="False"></asp:TextBox>
                    <ajaxToolkit:AutoCompleteExtender 
                        ID="txt_Cat_SubTitle_AutoCompleteExtender" 
                        runat="server" 
                        MinimumPrefixLength="1"
                        EnableCaching="true" 
                        CompletionSetCount="10"
                        ServiceMethod="SearchSubTitle"
                        FirstRowSelected = "false"                       
                        TargetControlID="txt_Cat_SubTitle" 
                        UseContextKey="True">
                    </ajaxToolkit:AutoCompleteExtender>
                 </td>
               
            </tr>
             <tr id="TR_VARTITLE" runat="server">
                <td class="style53"> 
                    Var Title</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_VarTitle" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter Var Title" Width="99%" Wrap="False"></asp:TextBox>
                 </td>
                
            </tr>
             <tr id="TR_ScholarName" runat="server">
                <td class="style53"> 
                    Scholar Name</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_ScholarName" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter Name of Scholar" Width="99%" Wrap="False"></asp:TextBox>
                 </td>
               
            </tr>
             <tr id="TR_ScholarDepartment" runat="server">
                <td class="style53"> 
                    Scholar Department</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_ScholarDept" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter Scholar Department" Width="99%" Wrap="False"></asp:TextBox>
                 </td>
               
            </tr>
             <tr id="TR_GuideName" runat="server">
                <td class="style53"> 
                    Guide Name</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_GuideName" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter Guide Name" Width="99%" Wrap="False"></asp:TextBox>
                 </td>
                
            </tr>
             <tr id="TR_GuideDepartment" runat="server">
                <td class="style53"> 
                    Guide Department</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_GuideDept" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter Guide Department" Width="99%" Wrap="False"></asp:TextBox>
                 </td>
                
            </tr>
             <tr id="TR_DegreeName" runat="server">
                <td class="style53"> 
                    Degree Name</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_DegreeName" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter Degree Name" Width="99%" Wrap="False"></asp:TextBox>
                 </td>
               
            </tr>
             <tr id="TR_SPRevision" runat="server">
                <td class="style53"> 
                    Standard Rev. No</td>
                <td class="style54" colspan="4">
                    <asp:TextBox ID="txt_Cat_SPRevision" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="100" 
                        ToolTip="Enter Standard Rev/Ver No." Width="80%" Wrap="False"></asp:TextBox>
                 </td>
               <td class="style53" colspan="3"> 
                    Re-Affirmation Year</td>
                 <td class="style54" colspan="2">
                     <asp:TextBox ID="txt_Cat_ReaffirmYear" runat="server" 
                         AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                         Height="16px" MaxLength="4" onkeypress="return isNumberKey(event)" 
                         ToolTip="Enter Year of Re-Affirmation in YYYY format" Width="50px" Wrap="False"></asp:TextBox>
                     <ajaxToolkit:AutoCompleteExtender ID="txt_Cat_ReaffirmYear_AutoCompleteExtender" 
                         runat="server" CompletionSetCount="10" EnableCaching="true" Enabled="True" 
                         FirstRowSelected="false" MinimumPrefixLength="1" ServiceMethod="SearchYear" 
                         TargetControlID="txt_Cat_ReaffirmYear">
                     </ajaxToolkit:AutoCompleteExtender>
                 </td>
                 <td class="style53" colspan="2">
                     Withdraw Year</td>
                 
               
                 <td class="style54" colspan="2">
                     <asp:TextBox ID="txt_Cat_WithdrawYear" runat="server" 
                         AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                         Height="16px" MaxLength="4" onkeypress="return isNumberKey(event)" 
                         ToolTip="Enter Year of Re-Affirmation in YYYY format" Width="50px" Wrap="False"></asp:TextBox>
                     <ajaxToolkit:AutoCompleteExtender ID="txt_Cat_WithdrawYear_AutoCompleteExtender" 
                         runat="server" CompletionSetCount="10" EnableCaching="true" Enabled="True" 
                         FirstRowSelected="false" MinimumPrefixLength="1" ServiceMethod="SearchYear" 
                         TargetControlID="txt_Cat_WithdrawYear">
                     </ajaxToolkit:AutoCompleteExtender>
                 </td>
                 
               
            </tr>
             <tr id="TR_SP_ISSUE_BODY" runat="server">
                <td class="style53"> 
                    Issuing Body</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_SPIssueBody" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="150" 
                        ToolTip="Enter Standard Issuing Body Details" Width="99%" Wrap="False"></asp:TextBox>
                 </td>
               
            </tr>
            <tr id="TR_SP_TCSC" runat="server">
                <td class="style53"> 
                    Technical Committee</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_SPTCSC" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="150" 
                        ToolTip="Enter Technical Committee Details" Width="99%" Wrap="False"></asp:TextBox>
                 </td>
               
            </tr>
             <tr id="TR_SP_UPDATES" runat="server">
                <td class="style53"> 
                    Updates Details</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_SPUpdates" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="150" 
                        ToolTip="Enter Updates Details" Width="99%" Wrap="False"></asp:TextBox>
                 </td>
               
            </tr>
             <tr id="TR_SP_AMMENDMENTS" runat="server">
                <td class="style53"> 
                    Ammendments Details</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_SPAmmendments" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="150" 
                        ToolTip="Enter Ammendments Details" Width="99%" Wrap="False"></asp:TextBox>
                 </td>
               
            </tr>
             <tr id="TR_ManualRev" runat="server">
                <td class="style53"> 
                    Manual Ver/Rev No</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_ManualRevision" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="50" 
                        ToolTip="Enter Manual Ver/Rev No" Width="200px" Wrap="False"></asp:TextBox>
                 </td>
               
            </tr>
             <tr id="TR_PatentNo" runat="server">
                <td class="style53"> 
                    Patent Number</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_PatentNo" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="100" 
                        ToolTip="Enter Patent Number" Width="200px" Wrap="False"></asp:TextBox>
                 </td>
                
            </tr>
             <tr id="TR_Patentee" runat="server">
                <td class="style53"> 
                    Patentee</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_Patentee" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="150" 
                        ToolTip="Enter Patentee" Width="99%" Wrap="False"></asp:TextBox>
                 </td>
               
            </tr>
             <tr id="TR_PatentInventor" runat="server">
                <td class="style53"> 
                    Patent Inventors;</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_PatentInventor" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="255" 
                        ToolTip="Plz Enter Patent Inventor" Width="99%" Wrap="False"></asp:TextBox>
                 </td>
               
            </tr>
             <tr id="TR_ConfName" runat="server">
                <td class="style53"> 
                    Conference Name</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_ConfName" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter Conference Name" Width="99%" Wrap="False"></asp:TextBox>
                    <ajaxToolkit:AutoCompleteExtender 
                        ID="txt_Cat_ConfName_AutoCompleteExtender" 
                        runat="server" 
                        Enabled="True" 
                        MinimumPrefixLength="1"
                        EnableCaching="true" 
                        CompletionSetCount="10"
                        ServiceMethod="SearchConfName"
                        FirstRowSelected = "false"     
                        TargetControlID="txt_Cat_ConfName">
                    </ajaxToolkit:AutoCompleteExtender>
                 </td>
               
            </tr>
             <tr id="TR_ConfDetails" runat="server">
                <td class="style53"> 
                    Conference Details</td>
                <td class="style54" colspan="4">
                    Date From:
                    <asp:TextBox ID="txt_Cat_SDate" runat="server" ForeColor="#0066FF" 
                        Height="16px" MaxLength="10" ToolTip="Click to Select Date" Width="71px" onkeypress="return DateOnly (event)"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="txt_Cat_SDate_CalendarExtender" 
                        runat="server" Enabled="True" Format="dd/MM/yyyy" 
                        TargetControlID="txt_Cat_SDate">
                    </ajaxToolkit:CalendarExtender>
                </td>
                <td class="style54" colspan="5">
                    Date To:&nbsp;
                    <asp:TextBox ID="txt_Cat_EDate" runat="server" EnableTheming="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="10" ToolTip="Click to Select Date" 
                        Width="71px" onkeypress="return DateOnly1 (event)"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="txt_Cat_EDate_CalendarExtender" 
                        runat="server" Enabled="True" Format="dd/MM/yyyy" 
                        TargetControlID="txt_Cat_EDate">
                    </ajaxToolkit:CalendarExtender>
                </td>
                <td class="style54" colspan="4">
                    Conf. Place:
                    <asp:TextBox ID="txt_Cat_ConfPlace" runat="server" 
                        AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                        Height="20px" MaxLength="100" ToolTip="Enter Conference Place" Width="70%" 
                        Wrap="False"></asp:TextBox>
                </td>
                
            </tr>
            <tr id="TR_Author" runat="server">
                <td class="style53"> 
                    Authors</td>
                <td class="style54" colspan="4">
                    Author1: 
                    <asp:TextBox ID="txt_Cat_Author1" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="20px" MaxLength="250" 
                        ToolTip="Enter Author1" Width="175px" Wrap="False"></asp:TextBox> 
                    <ajaxToolkit:AutoCompleteExtender 
                        ID="txt_Cat_Author1_AutoCompleteExtender" 
                        runat="server" 
                        MinimumPrefixLength="1"
                        EnableCaching="true" 
                        CompletionSetCount="10"
                        ServiceMethod="SearchAuthor"
                        TargetControlID="txt_Cat_Author1" 
                        FirstRowSelected = "false">
                        
                    </ajaxToolkit:AutoCompleteExtender>
                </td>
                <td class="style54" colspan="5">
                    Author2: 
                    <asp:TextBox ID="txt_Cat_Author2" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="20px" MaxLength="250" 
                        ToolTip="Enter Author2" Width="175px" Wrap="False"></asp:TextBox>
                        <ajaxToolkit:AutoCompleteExtender 
                        ID="txt_Cat_Author2_AutoCompleteExtender" 
                        runat="server" 
                        MinimumPrefixLength="1"
                        EnableCaching="true" 
                        CompletionSetCount="10"
                        ServiceMethod="SearchAuthor"
                        TargetControlID="txt_Cat_Author2"                        
                        FirstRowSelected = "false">
                        
                    </ajaxToolkit:AutoCompleteExtender>
                </td>
                <td class="style54" colspan="4">
                    Author3: 
                    <asp:TextBox ID="txt_Cat_Author3" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="20px" MaxLength="250" 
                        ToolTip="Enter Author3" Width="175px" Wrap="False" 
                        style="text-align: left"></asp:TextBox>
                        <ajaxToolkit:AutoCompleteExtender 
                        ID="txt_Cat_Author3_AutoCompleteExtender" 
                        runat="server" 
                        MinimumPrefixLength="1"
                        EnableCaching="true" 
                        CompletionSetCount="10"
                        ServiceMethod="SearchAuthor"
                        TargetControlID="txt_Cat_Author3" 
                        FirstRowSelected = "false">
                        
                    </ajaxToolkit:AutoCompleteExtender>
                </td>
                
            </tr>
             <tr id="TR_Editor" runat="server">
                <td class="style53"> 
                    Editor(s) ;</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_Editor" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter Editor(s) separated by semicolon (; ) and space" 
                        Width="99%" Wrap="False"></asp:TextBox>
                 </td>
              
            </tr>
             <tr id="TR_Translator" runat="server">
                <td class="style53"> 
                    Translator(s) ;</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_Translator" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter Translator(s) separated by semicolon (; ) and space" 
                        Width="99%" Wrap="False"></asp:TextBox>
                 </td>
               
            </tr>
             <tr id="TR_Illustrator" runat="server">
                <td class="style53"> 
                    Illustrator(s) ;</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_Illustrator" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter Illustrator(s) separated by semicolon (; ) and space" 
                        Width="99%" Wrap="False"></asp:TextBox>
                 </td>
               
            </tr>
             <tr id="TR_Compiler" runat="server">
                <td class="style53"> 
                    Compiler(s) ;</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_Compiler" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter Compiler(s) separated by semicolon (; ) and space" 
                        Width="99%" Wrap="False"></asp:TextBox>
                 </td>
               
            </tr>
             <tr id="TR_Commentator" runat="server">
                <td class="style53"> 
                    Comementator(s) ;</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_Commentator" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter Commentator(s) separated by semicolon (; ) and space" 
                        Width="99%" Wrap="False"></asp:TextBox>
                 </td>
               
            </tr>
             <tr id="TR_RevisedBy" runat="server">
                <td class="style53"> 
                    Revised By ;</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_RevisedBy" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="255" 
                        ToolTip="Enter Revised By,  separated by semicolon (; ) and space" 
                        Width="99%" Wrap="False"></asp:TextBox>
                 </td>               
            </tr>

            <tr id="TR_Producer" runat="server">
                <td class="style53"> 
                    Produced By</td>
                <td class="style54" colspan="7">
                    <asp:TextBox ID="txt_Cat_Producer" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="22px" MaxLength="250" 
                        ToolTip="Enter Name of Producer" Width="250px" Wrap="False"></asp:TextBox>
                 </td>
                 <td class="style54" colspan="6">
                     Production Year
                     <asp:TextBox ID="txt_Cat_ProductionYear" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="4" 
                        ToolTip="Enter Year of Production in YYYY format" Width="20%" Wrap="False" 
                        Height="16px" onkeypress="return isNumberKey(event)"></asp:TextBox>
                    <ajaxToolkit:AutoCompleteExtender 
                        ID="AutoCompleteExtender1" 
                        runat="server" 
                        Enabled="True"
                        MinimumPrefixLength="1"
                        EnableCaching="true" 
                        CompletionSetCount="10"
                        ServiceMethod="SearchYear"
                        FirstRowSelected = "false"                       
                        TargetControlID="txt_Cat_Year">
                    </ajaxToolkit:AutoCompleteExtender>
                 </td>               
            </tr>

            <tr id="TR_Designer" runat="server">
                <td class="style53"> 
                    Designed By ;</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_Designer" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="150" 
                        ToolTip="Enter Name of Designer" 
                        Width="99%" Wrap="False"></asp:TextBox>
                 </td>               
            </tr>

            <tr id="TR_Manufacturer" runat="server">
                <td class="style53"> 
                    Manufactured By ;</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_Manufacturer" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="250" 
                        ToolTip="Enter Manufacturer Name" 
                        Width="99%" Wrap="False"></asp:TextBox>
                 </td>               
            </tr>

             <tr id="TR_Creator" runat="server">
                <td class="style53"> 
                    Created By</td>
                <td class="style54" colspan="7">
                    <asp:TextBox ID="txt_Cat_Creater" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="22px" MaxLength="150" 
                        ToolTip="Enter Name of Creator" Width="250px" Wrap="False"></asp:TextBox>
                 </td>
                 <td class="style54" colspan="6">
                     Role of Creator
                     <asp:TextBox ID="txt_Cat_RoleofCreator" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="22px" MaxLength="150" 
                        ToolTip="Enter Role of Creator" Width="150px" Wrap="False"></asp:TextBox>
                 </td>               
            </tr>

            <tr id="TR_Materials" runat="server">
                <td class="style53"> 
                    Materials Used</td>
                <td class="style54" colspan="7">
                    <asp:TextBox ID="txt_Cat_Materials" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="22px" MaxLength="150" 
                        ToolTip="Enter Name and Type of Materials made of " Width="250px" 
                        Wrap="False"></asp:TextBox>
                 </td>
                 <td class="style54" colspan="6">
                     Technique
                     <asp:TextBox ID="txt_Cat_Techniq" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="22px" MaxLength="150" 
                        ToolTip="Enter Technique Used" Width="250px" Wrap="False"></asp:TextBox>
                 </td>               
            </tr>

            <tr id="TR_Work" runat="server">
                <td class="style53"> 
                    Work Category</td>
                <td class="style54" colspan="7">
                    <asp:TextBox ID="txt_Cat_WrokCategory" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="22px" MaxLength="150" 
                        ToolTip="Enter about Work Category" Width="250px" Wrap="False"></asp:TextBox>
                 </td>
                 <td class="style54" colspan="6">
                     Work Type
                     <asp:TextBox ID="txt_Cat_WorkType" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="22px" MaxLength="150" 
                        ToolTip="Enter Type of Work" Width="250px" Wrap="False"></asp:TextBox>
                 </td>               
            </tr>

             <tr id="TR_RelatedWork" runat="server">
                <td class="style53"> 
                    Related Work</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_RelatedWork" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="250" 
                        ToolTip="Enter Related Work Details" Width="99%" Wrap="False"></asp:TextBox>
                 </td>                
            </tr>

             <tr id="TR_Source" runat="server">
                <td class="style53"> 
                    Source</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_Source" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="250" 
                        ToolTip="Enter Source Details" Width="99%" Wrap="False"></asp:TextBox>
                 </td>                
            </tr>

             <tr id="TR_Photographer" runat="server">
                <td class="style53"> 
                    Photographer</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_Photographer" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="250" 
                        ToolTip="Enter Photographer Details" Width="99%" Wrap="False"></asp:TextBox>
                 </td>                
            </tr>



             <tr id="TR_CorpAuthor" runat="server">
                <td class="style53"> 
                    Corporate Author</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_CorpAuthor" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter Corporate Author" Width="99%" Wrap="False"></asp:TextBox>
                 </td>
                
            </tr>

             <tr id="TR_CHAIRMAN" runat="server">
                <td class="style53"> 
                    Chairman</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_Chairman" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="150" 
                        ToolTip="Enter Name of Chairman" 
                        Width="99%" Wrap="False"></asp:TextBox>
                 </td>               
            </tr>
             <tr id="TR_GOVERNMENT" runat="server">
                <td class="style53"> 
                    Government</td>
                <td class="style54" colspan="13">
                    <asp:DropDownList ID="DDL_Government" runat="server" 
                        Font-Bold="True" ForeColor="#0066CC" 
                        ToolTip="Plz Select Value from Drop-Down">
                        <asp:ListItem>State</asp:ListItem>
                        <asp:ListItem Selected="True">Centre</asp:ListItem>
                        <asp:ListItem>Others</asp:ListItem>
                    </asp:DropDownList>
                 </td>               
            </tr>
             <tr id="TR_Edition" runat="server">
                <td class="style53"> 
                    Edition</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_Edition" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter Edition (Do not type Ed.) " Width="300px" Wrap="False"></asp:TextBox>
                    <ajaxToolkit:AutoCompleteExtender 
                        ID="txt_Cat_Edition_AutoCompleteExtender" 
                        runat="server" 
                        MinimumPrefixLength="1"
                        EnableCaching="true" 
                        CompletionSetCount="10"
                        ServiceMethod="SearchEdition"
                        FirstRowSelected = "false"     
                        TargetControlID="txt_Cat_Edition">
                    </ajaxToolkit:AutoCompleteExtender>
                    <asp:Label ID="Label10" runat="server" Font-Size="X-Small" 
                        Text="Ed. (e.g. : 2nd Revised; Do Not Put &quot;Ed.&quot; word)"></asp:Label>
                 </td>
                
            </tr>
             <tr id="TR_Reprint" runat="server">
                <td class="style53"> 
                    Reprints</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_Reprint" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter Reprints" Width="500px" Wrap="False"></asp:TextBox>
                 </td>
                
            </tr>
             <tr id="TR_Publisher" runat="server">
                <td class="style53"> 
                    Publisher *</td>
                <td class="style54" colspan="13">
                   
                    <ajaxToolkit:ComboBox ID="Pub_ComboBox" runat="server" 
                        AutoCompleteMode="SuggestAppend" AutoPostBack="True" Font-Bold="True" 
                        ForeColor="#0066FF" BorderStyle="Double" ListItemHoverCssClass="PromptCSS" >
                    </ajaxToolkit:ComboBox>                   
                 </td>
                
            </tr>
             <tr id="TR_Place" runat="server">
                <td class="style53"> 
                    Place *</td>
                <td class="style54" colspan="7">
                    <asp:TextBox ID="txt_Cat_Place" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" Height="22px" MaxLength="150" 
                        ToolTip="Enter Place of Publication" Width="350px" Wrap="False"></asp:TextBox>
                    <ajaxToolkit:AutoCompleteExtender 
                        ID="txt_Cat_Place_AutoCompleteExtender" 
                        runat="server" 
                        MinimumPrefixLength="1"
                        EnableCaching="true" 
                        CompletionSetCount="10"
                        ServiceMethod="SearchPlace"
                        FirstRowSelected = "false"    
                        TargetControlID="txt_Cat_Place">
                    </ajaxToolkit:AutoCompleteExtender>
                 </td>
                 <td class="style54" colspan="6">
                     Country of Publications: *
                     <asp:DropDownList ID="DDL_Countries" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" ToolTip="Select Country from Drop-Down">
                     </asp:DropDownList>
                      <ajaxToolkit:ListSearchExtender 
                        ID="ListSearchExtender2" 
                        runat="server" 
                        Enabled="True" 
                        PromptCssClass="PromptCSS" 
                        TargetControlID="DDL_Countries">
                    </ajaxToolkit:ListSearchExtender>
                 </td>
               
            </tr>
             <tr id="TR_Year" runat="server">
                <td class="style53"> 
                    Year *</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_Year" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="4" 
                        ToolTip="Enter Year of Publication in YYYY format" Width="6%" Wrap="False" 
                        Height="16px" onkeypress="return isNumberKey(event)"></asp:TextBox>
                    <ajaxToolkit:AutoCompleteExtender 
                        ID="txt_Cat_Year_AutoCompleteExtender" 
                        runat="server" 
                        Enabled="True"
                        MinimumPrefixLength="1"
                        EnableCaching="true" 
                        CompletionSetCount="10"
                        ServiceMethod="SearchYear"
                        FirstRowSelected = "false"                       
                        TargetControlID="txt_Cat_Year">
                    </ajaxToolkit:AutoCompleteExtender>
                    <asp:Label ID="Label11" runat="server" Font-Size="Smaller" 
                        Text="YYYY (e.g. 2013)"></asp:Label>
                 </td>
                
            </tr>
             <tr id="TR_Series" runat="server">
                <td class="style53"> 
                    Series</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_Series" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter Series" Width="99%" Wrap="False"></asp:TextBox>
                    <ajaxToolkit:AutoCompleteExtender 
                        ID="txt_Cat_Series_AutoCompleteExtender" 
                        runat="server" 
                        Enabled="True"
                        MinimumPrefixLength="1"
                        EnableCaching="true" 
                        CompletionSetCount="10"
                        ServiceMethod="SearchSeries"
                        FirstRowSelected = "false"  
                        TargetControlID="txt_Cat_Series">
                    </ajaxToolkit:AutoCompleteExtender>
                 </td>
               
            </tr>
             <tr id="TR_SeriesEditor" runat="server">
                <td class="style53"> 
                    Series Editor(s) ;</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_SeriesEditor" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter series Editor(S); use semicolon (;) between two Editors" 
                        Width="99%" Wrap="False"></asp:TextBox>
                 </td>
                
            </tr>
             <tr id="TR_Note" runat="server">
                <td class="style53"> 
                    Note</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_Note" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter Note" Width="99%" Wrap="False" Height="106px" 
                        TextMode="MultiLine"></asp:TextBox>
                 </td>
                
            </tr>
             <tr id="TR_Remarks" runat="server">
                <td class="style53"> 
                    Remarks</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_Remarks" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter Remarks" Width="99%" Wrap="False" Height="120px" 
                        TextMode="MultiLine"></asp:TextBox>
                 </td>
               
            </tr>
             <tr id="TR_URL" runat="server">
                <td class="style53"> 
                    URL</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_URL" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter URL" Width="99%" Wrap="False"></asp:TextBox>
                 </td>
               
            </tr>
             <tr id="TR_Comments" runat="server">
                <td class="style53"> 
                    Comments</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_Comments" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter Comments" Width="99%" Wrap="False" Height="93px" 
                        TextMode="MultiLine"></asp:TextBox>
                 </td>
                
            </tr>
             <tr id="TR_Subject" runat="server">
                <td class="style53"> 
                    Main Subject</td>
                <td class="style54" colspan="13">
                    <asp:DropDownList ID="DDL_Subjects" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" ToolTip="Plz Select Value from Drop-Down" 
                        AutoPostBack="True">
                    </asp:DropDownList>
                     <ajaxToolkit:ListSearchExtender 
                        ID="ListSearchExtender1" 
                        runat="server" 
                        Enabled="True" 
                        PromptCssClass="PromptCSS" 
                        TargetControlID="DDL_Subjects">
                    </ajaxToolkit:ListSearchExtender>
                 </td>
               
            </tr>
             <tr id="TR_Keyword" runat="server">
                <td class="style53"> 
                    Keyword(s) ;</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_Keywords" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="1000" ToolTip="Enter Keyword(s) separated by semicolon (;) and space" 
                        Width="99%" Wrap="False"  style="text-transform: uppercase"></asp:TextBox>
                 </td>
                
            </tr>
             <tr id="TR_TranslatedFrom" runat="server">
                <td class="style53"> 
                    Translated From</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_TrFrom" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Translated From : Title" Width="99%" Wrap="False"></asp:TextBox>
                 </td>               
            </tr>
            <tr id="TR_Absract" runat="server">
                <td class="style53"> 
                    Abstract</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_Abstract" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter Abstract" Width="99%" Wrap="False"></asp:TextBox>
                 </td>               
            </tr>
            <tr id="TR_ReferenceNo" runat="server">
                <td class="style53"> Reference No</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_ReferenceNo" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="500" 
                        ToolTip="Enter Reference No" Width="99%" Wrap="False"></asp:TextBox>
                 </td>               
            </tr>

            <tr id="TR_Nationality" runat="server">
                <td class="style53"> Nationality</td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Cat_Nationality" runat="server" AutoCompleteType="DisplayName" 
                        Font-Bold="True" ForeColor="#0066FF" MaxLength="50" 
                        ToolTip="Enter Nationality Details" Width="40%" Wrap="False"></asp:TextBox>
                 </td>               
            </tr>

             <tr id="TR_PHOTO" runat="server">
                <td class="style53">Select Cover Photo</td>
                <td class="style54" colspan="13">
                    <asp:FileUpload ID="FileUpload1" runat="server" ViewStateMode="Enabled"     />
                      
                    <asp:Image ID="Image1" runat="server" BorderStyle="Double"  ImageAlign="Middle" BorderColor="#0033CC" BorderWidth="4px"/>             
                    <asp:CheckBox ID="CheckBox1" runat="server" Text="Delete This Picture from Database? press UPDATE Button." Visible="False" />
                           
                </td>
            </tr>
             <tr id="TR_CONTENTS" runat="server">
                <td class="style53">Upload Content Page/file</td>
                <td class="style54" colspan="13">
                    <asp:FileUpload ID="FileUpload2" runat="server" ViewStateMode="Enabled"     />
                      
                    <asp:HyperLink ID="HyperLink1" runat="server" Text="View Content File">View Content File</asp:HyperLink>
                    <asp:CheckBox ID="CheckBox3" runat="server" Text="Delete Content Page from Database? press UPDATE Button." Visible="False" />
                           
                </td>
            </tr>


            </table>






             <table id="Table4" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
             <tr>
                <td class="style56" colspan="14">
                   <asp:Label ID="Label5" runat="server" Font-Size="Medium" ForeColor="White" 
                        Font-Bold="True" Text="ACQUISITION / PURCHASING DATA"></asp:Label>
                </td>
            </tr>
             <tr id="TR_ACQMODE" runat="server">
                <td class="style53"> 
                    Acquisition Mode</td>
                <td class="style54" colspan="4">
                    <asp:DropDownList ID="DDL_AcqModes" runat="server" 
                        Font-Bold="True" ForeColor="#0066CC" 
                        ToolTip="Plz Select Value from Drop-Down">
                    </asp:DropDownList>
                    <asp:Label ID="Label_ACQID" runat="server"></asp:Label>
                 </td>               
                 <td class="style54" colspan="3">
                     Currency</td>
                 <td class="style54" colspan="2">
                     <asp:DropDownList ID="DDL_Currencies" runat="server" 
                         Font-Bold="True" ForeColor="#0066CC" 
                         ToolTip="Plz Select Value from Drop-Down">
                     </asp:DropDownList>
                 </td>
                 <td class="style54" colspan="2">
                     Item Price</td>
                 <td class="style54" colspan="2">
                     <asp:TextBox ID="txt_Acq_ItemPrice" runat="server" 
                         AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                         Height="18px" MaxLength="15" onkeypress="return isCurrencyKey(event)" 
                         ToolTip="Enter Item Price" Width="150px" Wrap="False"></asp:TextBox>
                 </td>
            </tr>
             <tr id="TR_CONVERSION" runat="server">
                <td class="style53"> 
                    Conversion Rate</td>
                <td class="style54" colspan="4">
                   <asp:TextBox ID="txt_Acq_ConversionRate" runat="server" 
                         AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                         Height="18px" MaxLength="15" onkeypress="return isCurrencyKey(event)" 
                         ToolTip="Enter Conversion Rate" Width="150px" Wrap="False"></asp:TextBox>
                 </td>               
                 <td class="style54" colspan="3">
                     Item Cost in Rupees</td>
                 <td class="style54" colspan="2">
                     <asp:TextBox ID="txt_Acq_ItemRupees" runat="server" 
                         AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                         Height="18px" MaxLength="15" onkeypress="return isCurrencyKey(event)" 
                         ToolTip="Enter Item cost in Rupees" Width="150px" Wrap="False"></asp:TextBox>
                 </td>
                 <td class="style54" colspan="2">
                     Other Charges(Rs)</td>
                 <td class="style54" colspan="2">
                     <asp:TextBox ID="txt_Acq_OtherCharges" runat="server" 
                         AutoCompleteType="DisplayName" Font-Bold="True" ForeColor="#0066FF" 
                         Height="18px" MaxLength="15" onkeypress="return isCurrencyKey(event)" 
                         ToolTip="Enter Additional Charges" Width="150px" Wrap="False"></asp:TextBox>
                 </td>
            </tr>
             <tr id="TR_VENDOR" runat="server">
                <td class="style53"> 
                    Source/Vendor</td>
                <td class="style54" colspan="13">
                    <asp:DropDownList ID="DDL_Vendors" runat="server" 
                        Font-Bold="True" ForeColor="#0066CC" ToolTip="Plz Select Value from Drop-Down" 
                        Width="450px">
                    </asp:DropDownList>
                    <asp:Label ID="Label_ProcessStatus" runat="server"></asp:Label>
                 </td>
                
            </tr>

            </table>





             <table id="Table2" runat="server" cellspacing="2" border="1"  cellpadding="1" class="style35">
             <tr>
                <td class="style56" colspan="14">
                   <asp:Label ID="Label13" runat="server" Font-Size="Medium" ForeColor="White" 
                        Font-Bold="True" Text="HOLDINGS/COPY DATA"></asp:Label>
                </td>
            </tr>

             

             

              <tr style=" color:Red; font-weight:bold">
                <td class="style53"> 
                    Accession Multi-Copies</td>
                <td class="style54" colspan="13">  
                    <asp:CheckBox ID="CB_RecvAll" runat="server" Font-Bold="True" 
                        Font-Size="Small" 
                        
                        
                        Text="Accession Multi-Copies with Single Click with Next Available Acc.No u hv typed below." 
                        AutoPostBack="True" />
                    <asp:TextBox ID="txt_Hold_Copies" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="10" 
                         ToolTip="Enter No of Copies to be Accessioned" 
                        Width="50px"  onkeypress="return isNumberKey (event)" ></asp:TextBox>No of Copies
                   
                </td>               
            </tr>



            
             <tr  id="TR_ACCSERIES" runat="server">
                <td class="style53"> 
                    <asp:Label ID="Label514" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Acc. Series"></asp:Label>
                    </td>
                <td class="style54">
                   
                    <asp:TextBox ID="txt_Hold_AccSereis" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="18px" MaxLength="2" 
                        onkeypress="return EngOnlyInput (event)" style="text-transform: uppercase" 
                        ToolTip="Enter Accession Sereis - Optional/Alpha Device" Width="40px" 
                        Wrap="False"></asp:TextBox>
                    <ajaxToolkit:AutoCompleteExtender ID="txt_Hold_AccSereis_AutoCompleteExtender" 
                        runat="server" CompletionSetCount="10" EnableCaching="true" 
                        FirstRowSelected="false" MinimumPrefixLength="1" ServiceMethod="SearchAccNo" 
                        TargetControlID="txt_Hold_AccSereis">
                    </ajaxToolkit:AutoCompleteExtender>
                   
                </td>
                <td class="style54">
                   
                    <asp:Label ID="Label56" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Acc. No.*" Width="100px"></asp:Label>
                   
                </td>
                <td class="style54" colspan="2">
                    
                    <asp:TextBox ID="txt_Hold_AccNo" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="18px" MaxLength="20" 
                        onkeypress="return suppressNonEng (event)" style="text-transform: uppercase" 
                        ToolTip="Enter Accession No" Width="50px" Wrap="False"></asp:TextBox>
                    <ajaxToolkit:AutoCompleteExtender ID="txt_Hold_AccNo_AutoCompleteExtender" 
                        runat="server" CompletionSetCount="10" EnableCaching="true" 
                        FirstRowSelected="false" MinimumPrefixLength="1" ServiceMethod="SearchAccNo" 
                        TargetControlID="txt_Hold_AccNo">
                    </ajaxToolkit:AutoCompleteExtender>
                    
                </td>
              
                <td class="style54">
                    
                   
                    <asp:Label ID="Label47" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Acc Date*" Width="100px"></asp:Label>
                    
                   
                </td>
                 <td class="style54" colspan="3">
                     <asp:TextBox ID="txt_Hold_AccDate" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" Height="16px" MaxLength="10" 
                         onkeypress="return DateOnly7 (event)" ToolTip="Click to Select Date" 
                         Width="71px"></asp:TextBox>
                     <ajaxToolkit:CalendarExtender ID="txt_Hold_AccDate_CalendarExtender" 
                         runat="server" Enabled="True" Format="dd/MM/yyyy" 
                         TargetControlID="txt_Hold_AccDate">
                     </ajaxToolkit:CalendarExtender>
                 </td>
                 <td class="style54">
                     
                     <asp:Label ID="Label_HoldID" runat="server"></asp:Label>
                     
                 </td>
                <td class="style54">
                    <asp:Label ID="Label49" runat="server" Font-Bold="True" Font-Size="Small" 
                         Text="Show ?"></asp:Label>
                    <asp:DropDownList ID="DDL_Show" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" ToolTip="Select Y/N ">
                        <asp:ListItem>Y</asp:ListItem>
                        <asp:ListItem>N</asp:ListItem>
                    </asp:DropDownList>
                    
                </td>
              
                 <td class="style54" colspan="2">
                     
                 </td>
                 <td class="style54">
                     <asp:Label ID="Label50" runat="server" Font-Bold="True" Font-Size="Small" 
                         Text="Issuable ?"></asp:Label>
                     <asp:DropDownList ID="DDL_Issuable" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" ToolTip="Select Y/N">
                         <asp:ListItem>Y</asp:ListItem>
                         <asp:ListItem>N</asp:ListItem>
                     </asp:DropDownList>
                 </td>
              
            </tr>

            <tr id ="TR_VOL_NO" runat ="server">
                <td class="style53">
                    <asp:Label ID="Label16" runat="server" Font-Bold="True" Font-Size="Small" Text="Volume No"></asp:Label>
                    </td>
                <td class="style54">
                    <asp:TextBox ID="txt_Hold_VolNo" runat="server" Font-Bold="True" ForeColor="#0066FF" Height="18px" MaxLength="50" ToolTip="Enter Volume No" Width="98%" Wrap="False"></asp:TextBox>
                    </td>
                <td class="style54" colspan="3">
                    <asp:Label ID="Label512" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Volume Year"></asp:Label>
                </td>
                <td class="style54" colspan="3">
                    <asp:TextBox ID="txt_Hold_VolYear" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="18px" MaxLength="4" 
                        onkeypress="return NumericOnly (event)" ToolTip="Enter 4 Digit Volume Year" 
                        Width="50px"></asp:TextBox>
                </td>
                <td class="style54" colspan="3">
                    <asp:Label ID="Label513" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Volume ISBN"></asp:Label>
                </td>
                <td class="style54" colspan="3">
                    <asp:TextBox ID="txt_Hold_CopyISBN" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="18px" MaxLength="50" ToolTip="Enter Volume ISBN No" 
                        Width="50%" Wrap="False"></asp:TextBox>
                </td>
            </tr>
           
            <tr  id ="TR_VOL_TITLE" runat ="server">
                <td class="style53">
                    <asp:Label ID="Label61" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Volume Title"></asp:Label>
                    </td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Hold_VolTitle" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="18px" MaxLength="500" ToolTip="Enter Volume Title" 
                        Width="98%" Wrap="False"></asp:TextBox>
                    </td>
            </tr>

             <tr  id ="TR_VOL_EDITORS" runat ="server">
                 <td class="style53">
                     <asp:Label ID="Label17" runat="server" Font-Bold="True" Font-Size="Small" 
                         Text="Vol Editor(s)"></asp:Label>
                 </td>
                 <td class="style54" colspan="13">
                     <asp:TextBox ID="txt_Hold_VolEditors" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" Height="18px" MaxLength="500" 
                         ToolTip="Enter Volume Editors separated by semicolon ;" Width="98%" 
                         Wrap="False"></asp:TextBox>
                 </td>
             </tr>
             
            


             <tr  id ="TR_CLASS_NO" runat ="server">
                <td class="style53">
                    <asp:Label ID="Label62" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Class No"></asp:Label>
                </td>
                <td class="style54" colspan="3">
                    <asp:TextBox ID="txt_Hold_ClassNo" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="18px" MaxLength="150" ToolTip="Enter Class No" 
                        Width="98%" Wrap="False"></asp:TextBox>
                         <ajaxToolkit:AutoCompleteExtender ID="txt_Hold_ClassNo_AutoCompleteExtender" 
                        runat="server" CompletionSetCount="10" EnableCaching="true" 
                        FirstRowSelected="false" MinimumPrefixLength="1" ServiceMethod="SearchClass" 
                        TargetControlID="txt_Hold_ClassNo">
                    </ajaxToolkit:AutoCompleteExtender>
                 </td>
                <td class="style54" colspan="3" align="right">
                    <asp:Label ID="Label511" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Book No" Width="100px"></asp:Label>
                 </td>
                 <td class="style54" colspan="3">
                     <asp:TextBox ID="txt_Hold_BookNo" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" Height="18px" MaxLength="50" 
                         style="text-transform: uppercase" ToolTip="Enter Book No" Width="150px" 
                         Wrap="False"></asp:TextBox>
                 </td>
                <td class="style54" colspan="4">
                    &nbsp;</td>
            </tr>


            

             <tr  id ="TR_PAGINATION" runat ="server">
                 <td class="style53">
                     <asp:Label ID="Label64" runat="server" Font-Bold="True" Font-Size="Small" 
                         Height="16px" Text="Pages*"></asp:Label>
                 </td>
                 <td class="style54" colspan="9">
                     <asp:TextBox ID="txt_Hold_Pages" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" Height="18px" MaxLength="250" ToolTip="Enter Pagination " 
                         Width="50%" Wrap="False"></asp:TextBox>
                 </td>
                 <td class="style54" colspan="4">
                     do not type P. with pages.</td>
             </tr>

             <tr  id ="TR_SIZE" runat ="server">
                 <td class="style53">
                     <asp:Label ID="Label74" runat="server" Font-Bold="True" Font-Size="Small" 
                         Text="Dimension"></asp:Label>
                 </td>
                 <td class="style54" colspan="9">
                     <asp:TextBox ID="txt_Hold_Size" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" Height="18px" MaxLength="50" 
                         ToolTip="Enter Size and Dimension" Width="150px" Wrap="False"></asp:TextBox>
                     in cm</td>
                 <td class="style54" colspan="4">
                     &nbsp;</td>
             </tr>



              <tr  id ="TR_ILLUSTRATION" runat ="server">
                <td class="style53">
                    <asp:Label ID="Label22" runat="server" Font-Bold="True" Font-Size="Small" Text="Illustration"></asp:Label>
                </td>
                <td class="style54" colspan="9">
                    <asp:CheckBox ID="CB_Illus" runat="server" Checked="True" Font-Bold="True" 
                        Font-Size="Small" Text="Illustration" />
                </td>
                <td class="style54" colspan="4">
                    &nbsp;</td>
            </tr>


            <tr  id ="TR_COLLECTION_TYPE" runat ="server">
                <td class="style53">
                    <asp:Label ID="Label75" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Collection*"></asp:Label>
                </td>
                <td class="style54" colspan="9">
                    <asp:DropDownList ID="DDL_CollectionType" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" ToolTip="Select Collection Type">
                        <asp:ListItem Selected="True" Value="C">Circulation</asp:ListItem>
                        <asp:ListItem Value="R">Reference</asp:ListItem>
                        <asp:ListItem Value="G">Book Bank (General)</asp:ListItem>
                        <asp:ListItem Value="S">Book Bank (SCST)</asp:ListItem>
                        <asp:ListItem Value="N">Non-Returnable</asp:ListItem>
                    </asp:DropDownList>
                 </td>
                <td class="style54" colspan="4">
                    &nbsp;</td>
            </tr>
            
             <tr  id ="TR_STA_CODE" runat ="server">
                 <td class="style53">
                     <asp:Label ID="Label76" runat="server" Font-Bold="True" Font-Size="Small" 
                         Text="Status*"></asp:Label>
                 </td>
                 <td class="style54" colspan="9">
                     <asp:DropDownList ID="DDL_Status" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" ToolTip="Select Status">
                     </asp:DropDownList>
                 </td>
                 <td class="style54" colspan="4">
                     &nbsp;</td>
             </tr>

             <tr  id ="TR_BIND_CODE" runat ="server">
                 <td class="style53">
                     <asp:Label ID="Label77" runat="server" Font-Bold="True" Font-Size="Small" 
                         Text="Binding"></asp:Label>
                 </td>
                 <td class="style54" colspan="9">
                     <asp:DropDownList ID="DDL_Binding" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" ToolTip="Select Binding">
                     </asp:DropDownList>
                 </td>
                 <td class="style54" colspan="4">
                     &nbsp;</td>
             </tr>


            <tr  id ="TR_SEC_CODE" runat ="server">
                <td class="style53">
                    <asp:Label ID="Label78" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Section"></asp:Label>
                </td>
                <td class="style54" colspan="9">
                    <asp:DropDownList ID="DDL_Section" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" ToolTip="Select Section">
                    </asp:DropDownList>
                 </td>
                <td class="style54" colspan="4">
                    &nbsp;</td>
            </tr>
            
             <tr  id ="TR_ACC_MAT_CODE" runat ="server">
                 <td class="style53">
                     <asp:Label ID="Label81" runat="server" Font-Bold="True" Font-Size="Small" 
                         Text="Accompanying Materials"></asp:Label>
                 </td>
                 <td class="style54" colspan="10">
                     <asp:DropDownList ID="DDL_AccMaterials" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" ToolTip="Select Accompanying Materials">
                     </asp:DropDownList>
                     &nbsp;Select Accompanying Materials received with books</td>
                 <td class="style54" colspan="3">
                     &nbsp;</td>
             </tr>

             <tr  id ="TR_FORMAT_CODE" runat ="server">
                 <td class="style53">
                     <asp:Label ID="Label82" runat="server" Font-Bold="True" Font-Size="Small" 
                         Text="Format*"></asp:Label>
                 </td>
                 <td class="style54" colspan="10">
                     <asp:DropDownList ID="DDL_Format" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" ToolTip="Select Physical Format/Medium">
                     </asp:DropDownList>
                 </td>
                 <td class="style54" colspan="3">
                     &nbsp;</td>
             </tr>
             
            <tr  id="TR_LIBRARY" runat="server">
                <td class="style53">
                    <asp:Label ID="Label23" runat="server" Font-Bold="True" Font-Size="Small"  
                        Text="Library*"></asp:Label>
                </td>
                <td class="style54" colspan="13">
                    <asp:DropDownList ID="DDL_Library" runat="server" Font-Bold="True"  
                        ForeColor="#0066FF" ToolTip="Select Library" Width="98%">
                    </asp:DropDownList>
                </td>
            </tr>



              <tr id ="TR_HOLDREMARKS" runat ="server">
                <td class="style53">
                    <asp:Label ID="Label24" runat="server" Font-Bold="True" Font-Size="Small"  Text="Remarks"></asp:Label>
                </td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Hold_Remarks" runat="server" MaxLength="250"   ToolTip="Enter Remarks" Wrap="False"  Font-Bold="True" ForeColor="#0066FF" Width="672px" Height="62px" TextMode="MultiLine"></asp:TextBox>
                    </td>
            </tr>

             <tr  id ="TR_PHYSICAL_LOCATION" runat ="server">
                <td class="style53">
                    <asp:Label ID="Label25" runat="server" Font-Bold="True" Font-Size="Small" Text="Location"></asp:Label>
                 </td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Hold_Location" runat="server" MaxLength="250" 
                        ToolTip="Type Location of Copy" Wrap="False" Font-Bold="True" 
                        ForeColor="#0066FF" Width="50%" Height="23px"></asp:TextBox>
                    </td>
            </tr>

             <tr id ="TR_REFERENCE_NO" runat ="server">
                <td class="style53">
                    <asp:Label ID="Label40" runat="server" Font-Bold="True" Font-Size="Small" Text="Reference No"></asp:Label>
                 </td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Hold_ReferenceNo" runat="server" MaxLength="250"  ToolTip="Type some Local Information" Wrap="False" Font-Bold="True" ForeColor="#0066FF" Width="98%" Height="23px"></asp:TextBox>
                    </td>
            </tr>
            
             <tr  id ="TR_MEDIUM" runat ="server">
                <td class="style53">
                    <asp:Label ID="Label83" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Recording Medium"></asp:Label>
                </td>
                <td class="style54" colspan="9">
                    <asp:TextBox ID="txt_Hold_RecordingMedium" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="18px" MaxLength="50" 
                        ToolTip="Enter Recording Medium" Width="150px"></asp:TextBox>
                 </td>
                <td class="style54" colspan="4">
                    &nbsp;</td>
            </tr>

             <tr  id ="TR_RECORDING_CATEGORY" runat ="server">
                 <td class="style53">
                     <asp:Label ID="Label84" runat="server" Font-Bold="True" Font-Size="Small" 
                         Text="Recording Category"></asp:Label>
                 </td>
                 <td class="style54" colspan="9">
                     <asp:TextBox ID="txt_Hold_RecordingCategory" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" Height="18px" MaxLength="50" 
                         ToolTip="Enter Recording Category" Width="150px"></asp:TextBox>
                 </td>
                 <td class="style54" colspan="4">
                     &nbsp;</td>
             </tr>
             <tr  id ="TR_RECORDING_FORM" runat ="server">
                 <td class="style53">
                     <asp:Label ID="Label85" runat="server" Font-Bold="True" Font-Size="Small" 
                         Text="Recording Form"></asp:Label>
                 </td>
                 <td class="style54" colspan="9">
                     <asp:TextBox ID="txt_Hold_RecordingForm" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" Height="18px" MaxLength="50" 
                         ToolTip="Enter Recording Form" Width="150px"></asp:TextBox>
                 </td>
                 <td class="style54" colspan="4">
                     &nbsp;</td>
             </tr>

             <tr id ="TR_RECORDING_FORMAT" runat ="server">
                <td class="style53">
                    <asp:Label ID="Label86" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Recording Format"></asp:Label>
                </td>
                <td class="style54" colspan="9">
                    <asp:TextBox ID="txt_Hold_RecordingFormat" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="18px" MaxLength="50" 
                        ToolTip="Enter Recording Format" Width="150px"></asp:TextBox>
                 </td>
                <td class="style54" colspan="4">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_RECORDING_SPEED" runat ="server">
                <td class="style53">
                    <asp:Label ID="Label35" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Recording Speed"></asp:Label>
                </td>
                <td class="style54" colspan="9">
                    <asp:TextBox ID="txt_Hold_RecordingSpeed" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="18px" MaxLength="50" 
                        ToolTip="Enter Recording Speed" Width="150px" Wrap="False"></asp:TextBox>
                 </td>
                <td class="style54" colspan="4">
                    &nbsp;</td>
            </tr>


             <tr id ="TR_RECORDING_STORAGE_TECH" runat ="server">
                 <td class="style53">
                     <asp:Label ID="Label36" runat="server" Font-Bold="True" Font-Size="Small" 
                         Text="Storage Technology"></asp:Label>
                 </td>
                 <td class="style54" colspan="9">
                     <asp:TextBox ID="txt_Hold_RecordingStorageTech" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" Height="18px" MaxLength="50" 
                         ToolTip="Enter Recording Storage Technology" Width="98%" Wrap="False"></asp:TextBox>
                 </td>
                 <td class="style54" colspan="4">
                     &nbsp;</td>
             </tr>

             <tr id ="TR_RECORDING_PLAY_DURATION" runat ="server">
                 <td class="style53">
                     <asp:Label ID="Label37" runat="server" Font-Bold="True" Font-Size="Small" 
                         Text="Play Duration" Width="100px"></asp:Label>
                 </td>
                 <td class="style54" colspan="9">
                     <asp:TextBox ID="txt_Hold_RecordingDuration" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" Height="18px" MaxLength="50" 
                         ToolTip="Enter Play Duration in Minutes" Width="98%" Wrap="False"></asp:TextBox>
                 </td>
                 <td class="style54" colspan="4">
                     &nbsp;</td>
             </tr>

             <tr id ="TR_VIDEO_TYPEOFVISUAL" runat ="server">
                 <td class="style53">
                     <asp:Label ID="Label38" runat="server" Font-Bold="True" Font-Size="Small" 
                         Text="Type of Visuals"></asp:Label>
                 </td>
                 <td class="style54" colspan="9">
                     <asp:TextBox ID="txt_Hold_TypeOfVisuals" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" Height="18px" MaxLength="50" 
                         ToolTip="Enter Type of Visuals" Width="98%"></asp:TextBox>
                 </td>
                 <td class="style54" colspan="4">
                     &nbsp;</td>
             </tr>


               <tr id ="TR_CARTOGRAPHIC_SCALE" runat ="server">
                <td class="style53">
                    <asp:Label ID="Label54" runat="server" Font-Bold="True" 
                        Font-Size="Small" Text="Scale"></asp:Label>
                    </td>
                <td class="style54" colspan="8">
                    <asp:TextBox ID="txt_Hold_Scale" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="18px" MaxLength="50" 
                        ToolTip="Enter Scale" Width="98%" Wrap="False"></asp:TextBox>
                    </td>
                <td class="style54" colspan="5">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_CARTOGRAPHIC_PROJECTION" runat ="server">
                 <td class="style53">
                     <asp:Label ID="Label55" runat="server" Font-Bold="True" Font-Size="Small" 
                         Text="Projection"></asp:Label>
                 </td>
                 <td class="style54" colspan="8">
                     <asp:TextBox ID="txt_Hold_Projection" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" Height="18px" MaxLength="150" ToolTip="Enter Projection" 
                         Width="200px"></asp:TextBox>
                 </td>
                 <td class="style54" colspan="5">
                     &nbsp;</td>
             </tr>

             <tr id ="TR_CARTOGRAPHIC_COORDINATES" runat ="server">
                <td class="style53">
                    <asp:Label ID="Label19" runat="server" Font-Bold="True" 
                        Font-Size="Small" Text="Co-Ordinates"></asp:Label>
                    </td>
                <td class="style54" colspan="8">
                    <asp:TextBox ID="txt_Hold_Coordinates" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="18px" MaxLength="150" 
                        ToolTip="Enter Co-Ordinates" Width="200px" Wrap="False"></asp:TextBox>
                    </td>
                <td class="style54" colspan="5">
                    &nbsp;</td>
            </tr>
           
             <tr id ="TR_CARTOGRAPHIC_GEOGRAPHIC_LOCATION" runat ="server">
                 <td class="style53">
                     <asp:Label ID="Label20" runat="server" Font-Bold="True" Font-Size="Small" 
                         Text="Geographic Location"></asp:Label>
                 </td>
                 <td class="style54" colspan="8">
                     <asp:TextBox ID="txt_Hold_GeographicLocation" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" Height="18px" MaxLength="150" 
                         ToolTip="Enter Geographic Location" Width="200px"></asp:TextBox>
                 </td>
                 <td class="style54" colspan="5">
                     &nbsp;</td>
             </tr>

             <tr id ="TR_CARTOGRAPHIC_MEDIUM" runat ="server">
                 <td class="style53">
                     <asp:Label ID="Label226" runat="server" Font-Bold="True" Font-Size="Small" 
                         Text="Cartographic Medium"></asp:Label>
                 </td>
                 <td class="style54" colspan="8">
                     <asp:DropDownList ID="DDL_GeographicMedium" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" ToolTip="Select Cartographic Medium">
                         <asp:ListItem Value="Paper">Paper</asp:ListItem>
                         <asp:ListItem Value="Wood">Wood</asp:ListItem>
                         <asp:ListItem Value="Stone">Stone</asp:ListItem>
                         <asp:ListItem>Metal</asp:ListItem>
                         <asp:ListItem>Synthetic</asp:ListItem>
                         <asp:ListItem>Textile</asp:ListItem>
                         <asp:ListItem>Plastic</asp:ListItem>
                         <asp:ListItem>Glass</asp:ListItem>
                         <asp:ListItem>Venyl</asp:ListItem>
                         <asp:ListItem>Vellum</asp:ListItem>
                         <asp:ListItem>Plaster</asp:ListItem>
                         <asp:ListItem>Leather</asp:ListItem>
                         <asp:ListItem>Others</asp:ListItem>
                     </asp:DropDownList>
                 </td>
                 <td class="style54" colspan="5">
                     &nbsp;</td>
             </tr>

              <tr id ="TR_GLOBE_TYPE" runat ="server">
                <td class="style53">
                    <asp:Label ID="Label65" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Globe Type"></asp:Label>
                </td>
                <td class="style54" colspan="5">
                    <asp:TextBox ID="txt_Hold_GlobeType" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="50" 
                        onkeypress="return DateOnly4 (event)" ToolTip="Enter Globe Type" 
                        Width="200px"></asp:TextBox>
                 </td>
                <td class="style54" colspan="8">
                    &nbsp;</td>
            </tr>
           
           <tr id ="TR_CARTOGRAPHIC_DATAGATHERING_DATE" runat ="server">
                <td class="style53">
                    <asp:Label ID="Label87" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Data Gathering Date"></asp:Label>
                </td>
                <td class="style54" colspan="5">
                    <asp:TextBox ID="txt_Hold_DataGatheringDate" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="10" ToolTip="Click to Select Date" 
                        Width="71px" onkeypress="return DateOnly8 (event)"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="txt_Hold_DataGatheringDate_CalendarExtender" 
                        runat="server" Enabled="True" Format="dd/MM/yyyy" 
                        TargetControlID="txt_Hold_DataGatheringDate">
                    </ajaxToolkit:CalendarExtender>
                    &nbsp;dd/MM/yyyy</td>
                <td class="style54" colspan="8">
                    &nbsp;</td>
            </tr>

                        
             <tr id ="TR_CREATION_DATE" runat ="server">
                 <td class="style53">
                     <asp:Label ID="Label88" runat="server" Font-Bold="True" Font-Size="Small" 
                         Text="Creation Date"></asp:Label>
                 </td>
                 <td class="style54" colspan="5">
                     <asp:TextBox ID="txt_Hold_CreationDate" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" Height="16px" MaxLength="10" 
                         onkeypress="return DateOnly2 (event)" ToolTip="Click to Select Date" 
                         Width="71px"></asp:TextBox>
                     <ajaxToolkit:CalendarExtender ID="txt_Hold_CreationDate_CalendarExtender" 
                         runat="server" Enabled="True" Format="dd/MM/yyyy" 
                         TargetControlID="txt_Hold_CreationDate">
                     </ajaxToolkit:CalendarExtender>
                     dd/MM/yyyy</td>
                 <td class="style54" colspan="8">
                     &nbsp;</td>
             </tr>

             <tr id ="TR_CARTOGRAPHIC_COMPILATION_DATE" runat ="server">
                 <td class="style53">
                     <asp:Label ID="Label89" runat="server" Font-Bold="True" Font-Size="Small" 
                         Text="Compilation Date"></asp:Label>
                 </td>
                 <td class="style54" colspan="5">
                     <asp:TextBox ID="txt_Hold_CompilationDate" runat="server" Font-Bold="True" 
                         ForeColor="#0066FF" Height="16px" MaxLength="10" 
                         onkeypress="return DateOnly3 (event)" ToolTip="Click to Select Date" 
                         Width="71px"></asp:TextBox>
                     <ajaxToolkit:CalendarExtender ID="txt_Hold_CompilationDate_CalendarExtender" 
                         runat="server" Enabled="True" Format="dd/MM/yyyy" 
                         TargetControlID="txt_Hold_CompilationDate">
                     </ajaxToolkit:CalendarExtender>
                     dd/MM/yyyy</td>
                 <td class="style54" colspan="8">
                     &nbsp;</td>
             </tr>
           
           <tr id ="TR_CARTOGRAPHIC_INSPECTION_DATE" runat ="server">
                <td class="style53">
                    <asp:Label ID="Label90" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Inspection Date"></asp:Label>
                </td>
                <td class="style54" colspan="5">
                    <asp:TextBox ID="txt_Hold_InspectionDate" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="10" 
                        onkeypress="return DateOnly4 (event)" ToolTip="Click to Select Date" 
                        Width="71px"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="txt_Hold_InspectionDate_CalendarExtender" 
                        runat="server" Enabled="True" Format="dd/MM/yyyy" 
                        TargetControlID="txt_Hold_InspectionDate">
                    </ajaxToolkit:CalendarExtender>
                    dd/MM/yyyy</td>
                <td class="style54" colspan="8">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_VIDEO_COLOR" runat ="server">
                <td class="style53">
                    <asp:Label ID="Label21" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Color"></asp:Label>
                </td>
                <td class="style54" colspan="5">
                    <asp:TextBox ID="txt_Hold_Color" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="50" 
                        ToolTip="Enter Color" 
                        Width="200px"></asp:TextBox>
                 </td>
                <td class="style54" colspan="8">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_PLAYBACK_CHANNELS" runat ="server">
                <td class="style53">
                    <asp:Label ID="Label26" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Playback Channel"></asp:Label>
                </td>
                <td class="style54" colspan="5">
                    <asp:TextBox ID="txt_Hold_PlayBackChannel" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="50" 
                        onkeypress="return DateOnly4 (event)" ToolTip="Enter Playback Channel" 
                        Width="200px"></asp:TextBox>
                 </td>
                <td class="style54" colspan="8">
                    &nbsp;</td>
            </tr> 

             <tr id ="TR_TAPE_WIDTH" runat ="server">
                <td class="style53">
                    <asp:Label ID="Label27" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Tape Width"></asp:Label>
                </td>
                <td class="style54" colspan="5">
                    <asp:TextBox ID="txt_Hold_TapeWidth" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="50" 
                        onkeypress="return DateOnly4 (event)" ToolTip="Enter Tape Width" 
                        Width="200px"></asp:TextBox>
                 </td>
                <td class="style54" colspan="8">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_TAPE_CONFIGURATION" runat ="server">
                <td class="style53">
                    <asp:Label ID="Label28" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Tape Configuration"></asp:Label>
                </td>
                <td class="style54" colspan="5">
                    <asp:TextBox ID="txt_Hold_TapeConfiguration" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="50" 
                        onkeypress="return DateOnly4 (event)" ToolTip="Enter Tape Configuration" 
                        Width="200px"></asp:TextBox>
                 </td>
                <td class="style54" colspan="8">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_KIND_OF_DISK" runat ="server">
                <td class="style53">
                    <asp:Label ID="Label29" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Disk Type"></asp:Label>
                </td>
                <td class="style54" colspan="5">
                    <asp:TextBox ID="txt_Hold_KindofDisk" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="50" 
                        onkeypress="return DateOnly4 (event)" ToolTip="Enter Disk Type" 
                        Width="200px"></asp:TextBox>
                 </td>
                <td class="style54" colspan="8">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_KIND_OF_CUTTING" runat ="server">
                <td class="style53">
                    <asp:Label ID="Label30" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Disk Cutting Type"></asp:Label>
                </td>
                <td class="style54" colspan="5">
                    <asp:TextBox ID="txt_Hold_KindofCutting" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="50" 
                        onkeypress="return DateOnly4 (event)" ToolTip="Enter Disk Cutting Technology" 
                        Width="200px"></asp:TextBox>
                 </td>
                <td class="style54" colspan="8">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_ENCODING_STANDARD" runat ="server">
                <td class="style53">
                    <asp:Label ID="Label31" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Encoding Standard"></asp:Label>
                </td>
                <td class="style54" colspan="5">
                    <asp:TextBox ID="txt_Hold_EncodingStandard" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="50" 
                        onkeypress="return DateOnly4 (event)" ToolTip="Enter Encoding Standard" 
                        Width="200px"></asp:TextBox>
                 </td>
                <td class="style54" colspan="8">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_CAPTURE_TECHNIQUE" runat ="server">
                <td class="style53">
                    <asp:Label ID="Label32" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Capture Technique"></asp:Label>
                </td>
                <td class="style54" colspan="5">
                    <asp:TextBox ID="txt_Hold_CaptureTechnique" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="50" 
                        onkeypress="return DateOnly4 (event)" ToolTip="Enter Audio/Video Capture Technique" 
                        Width="200px"></asp:TextBox>
                 </td>
                <td class="style54" colspan="8">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_PHOTO_NO" runat ="server">
                <td class="style53">
                    <asp:Label ID="Label33" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Photo No"></asp:Label>
                </td>
                <td class="style54" colspan="5">
                    <asp:TextBox ID="txt_Hold_PhotoNo" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="50" 
                        onkeypress="return DateOnly4 (event)" ToolTip="Enter Photo No" 
                        Width="200px"></asp:TextBox>
                 </td>
                <td class="style54" colspan="8">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_PHOTO_ALBUM_NO" runat ="server">
                <td class="style53">
                    <asp:Label ID="Label34" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Photo Album No"></asp:Label>
                </td>
                <td class="style54" colspan="5">
                    <asp:TextBox ID="txt_Hold_PhotoAlbumNo" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="150" 
                        onkeypress="return DateOnly4 (event)" ToolTip="Enter Photo Album No" 
                        Width="200px"></asp:TextBox>
                 </td>
                <td class="style54" colspan="8">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_PHOTO_OCASION" runat ="server">
                <td class="style53">
                    <asp:Label ID="Label39" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Ocasion"></asp:Label>
                </td>
                <td class="style54" colspan="5">
                    <asp:TextBox ID="txt_Hold_Ocasion" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="150" 
                       ToolTip="Enter Ocasion of Photo" 
                        Width="200px"></asp:TextBox>
                 </td>
                <td class="style54" colspan="8">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_IMAGE_VIEW_TYPE" runat ="server">
                <td class="style53">
                    <asp:Label ID="Label41" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Image View Type"></asp:Label>
                </td>
                <td class="style54" colspan="5">
                    <asp:TextBox ID="txt_Hold_ImageViewType" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="50" 
                   
                        ToolTip="Enter Image View Details" Width="200px"></asp:TextBox>
                 </td>
                <td class="style54" colspan="8">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_VIEW_DATE" runat ="server">
                <td class="style53">
                    <asp:Label ID="Label42" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="View Date"></asp:Label>
                </td>
                <td class="style54" colspan="5">
                    <asp:TextBox ID="txt_Hold_ViewDate" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="10" 
                        onkeypress="return DateOnly5 (event)" ToolTip="Click to Select Date" 
                        Width="71px"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="txt_Hold_ViewDate_CalendarExtender" 
                        runat="server" Enabled="True" Format="dd/MM/yyyy" 
                        TargetControlID="txt_Hold_ViewDate">
                    </ajaxToolkit:CalendarExtender>
                    dd/MM/yyyy</td>
                <td class="style54" colspan="8">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_THEME" runat ="server">
                <td class="style53">
                    <asp:Label ID="Label43" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Theme"></asp:Label>
                </td>
                <td class="style54" colspan="5">
                    <asp:TextBox ID="txt_Hold_Theme" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="150" 
                        ToolTip="Enter Theme" 
                        Width="200px"></asp:TextBox>
                 </td>
                <td class="style54" colspan="8">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_STYLE" runat ="server">
                <td class="style53">
                    <asp:Label ID="Label44" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Style"></asp:Label>
                </td>
                <td class="style54" colspan="5">
                    <asp:TextBox ID="txt_Hold_Style" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="150" 
                         ToolTip="Enter Style" 
                        Width="200px"></asp:TextBox>
                 </td>
                <td class="style54" colspan="8">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_CULTURE" runat ="server">
                <td class="style53">
                    <asp:Label ID="Label45" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Culture"></asp:Label>
                </td>
                <td class="style54" colspan="5">
                    <asp:TextBox ID="txt_Hold_Culture" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="150" 
                        ToolTip="Enter Eulture" 
                        Width="200px"></asp:TextBox>
                 </td>
                <td class="style54" colspan="8">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_CURRENT_SITE" runat ="server">
                <td class="style53">
                    <asp:Label ID="Label46" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Current Site"></asp:Label>
                </td>
                <td class="style54" colspan="5">
                    <asp:TextBox ID="txt_Hold_CurrentSite" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="150" 
                        ToolTip="Enter Current Site/Location" 
                        Width="200px"></asp:TextBox>
                 </td>
                <td class="style54" colspan="8">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_CREATION_SITE" runat ="server">
                <td class="style53">
                    <asp:Label ID="Label48" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Creation Site"></asp:Label>
                </td>
                <td class="style54" colspan="5">
                    <asp:TextBox ID="txt_Hold_CreationSite" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="150" 
                       ToolTip="Enter Creation Site/Location" 
                        Width="200px"></asp:TextBox>
                 </td>
                <td class="style54" colspan="8">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_YARNCOUNT" runat ="server">
                <td class="style53">
                    <asp:Label ID="Label51" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Yarn Count"></asp:Label>
                </td>
                <td class="style54" colspan="5">
                    <asp:TextBox ID="txt_Hold_YarnCount" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="150" 
                        ToolTip="Enter Yarn Count" Width="200px"></asp:TextBox>
                 </td>
                <td class="style54" colspan="8">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_MATERIAL_TYPE" runat ="server">
                <td class="style53">
                    <asp:Label ID="Label52" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Materials Type"></asp:Label>
                </td>
                <td class="style54" colspan="5">
                    <asp:TextBox ID="txt_Hold_MaterialsType" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="150" 
                        ToolTip="Enter Material Type" 
                        Width="200px"></asp:TextBox>
                 </td>
                <td class="style54" colspan="8">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_TECHNIQUE" runat ="server">
                <td class="style53">
                    <asp:Label ID="Label53" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Technique"></asp:Label>
                </td>
                <td class="style54" colspan="13">
                    <asp:TextBox ID="txt_Hold_Technique" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="150" 
                        ToolTip="Enter Technique adopted" 
                        Width="200px"></asp:TextBox>
                 </td>
                
            </tr>

             <tr id ="TR_TECH_DETAILS" runat ="server">
                <td class="style53">
                    <asp:Label ID="Label57" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Technical Details"></asp:Label>
                </td>
                <td class="style54" colspan="5">
                    <asp:TextBox ID="txt_Hold_TechDetails" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="250" 
                        ToolTip="Enter Technical Details" 
                        Width="200px"></asp:TextBox>
                 </td>
                <td class="style54" colspan="8">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_INSCRIPTIONS" runat ="server">
                <td class="style53">
                    <asp:Label ID="Label58" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Inscriptions"></asp:Label>
                </td>
                <td class="style54" colspan="5">
                    <asp:TextBox ID="txt_Hold_Inscription" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="150" 
                        ToolTip="Enter Transcription" 
                        Width="200px"></asp:TextBox>
                 </td>
                <td class="style54" colspan="8">
                    &nbsp;</td>
            </tr>

            <tr id ="TR_DESCRIPTION" runat ="server">
                <td class="style53">
                    <asp:Label ID="Label510" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Description"></asp:Label>
                </td>
                <td class="style54" colspan="5">
                    <asp:TextBox ID="txt_Hold_Description" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="250" 
                        ToolTip="Enter Description" 
                        Width="200px"></asp:TextBox>
                 </td>
                <td class="style54" colspan="8">
                    &nbsp;</td>
            </tr>

             <tr id ="TR_ALTER_DATE" runat ="server">
                <td class="style53">
                    <asp:Label ID="Label59" runat="server" Font-Bold="True" Font-Size="Small" 
                        Text="Alter Date"></asp:Label>
                </td>
                <td class="style54" colspan="5">
                    <asp:TextBox ID="txt_Hold_AlterDate" runat="server" Font-Bold="True" 
                        ForeColor="#0066FF" Height="16px" MaxLength="10" 
                        onkeypress="return DateOnly6 (event)" ToolTip="Click to Select Date" 
                        Width="71px"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="txt_Hold_AlterDate_CalendarExtender" 
                        runat="server" Enabled="True" Format="dd/MM/yyyy" 
                        TargetControlID="txt_Hold_AlterDate">
                    </ajaxToolkit:CalendarExtender>
                    dd/MM/yyyy</td>
                <td class="style54" colspan="8">
                    &nbsp;</td>
            </tr>


            </table>
            










        




         <table id="Table5" runat="server" cellspacing="2" border="1"  cellpadding="2" class="style35">
           <tr>
             <td bgcolor="#336699" class="style56" colspan="5">
                 <asp:Label ID="Label515" runat="server" Text="List of Holdings" 
                     style="font-weight: 700; font-size: small; color: #FFFFFF"></asp:Label></td>
         </tr>
         <tr>

             <td bgcolor="#336699" class="style56" colspan="5">
                 <asp:GridView ID="Grid2" runat="server" AllowPaging="True" allowsorting="True" 
                     AutoGenerateColumns="False" DataKeyNames="HOLD_ID" Font-Bold="True" 
                     Font-Names="Tahoma" Font-Size="8pt" Height="100px" HorizontalAlign="Center" 
                     ShowFooter="True" style="width: 100%;  text-align: center;">
                     <Columns>
                         <asp:TemplateField HeaderText="S.N.">
                             <ItemTemplate>
                                 <asp:Label ID="lblsr" runat="server" CssClass="MBody" SkinID="" width="25px"></asp:Label>
                             </ItemTemplate>
                             <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                             <ItemStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" 
                                 ForeColor="#336699" Width="25px" />
                         </asp:TemplateField>

                          <asp:ButtonField HeaderText="Edit"  Text="Edit" CommandName="Select">
                            <HeaderStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" />
                            <ItemStyle ForeColor="#CC0000" Width="20px" Font-Bold="true" />
                            </asp:ButtonField>



                         <asp:BoundField DataField="ACCESSION_NO" HeaderText="Accession No" SortExpression="ACCESSION_NO" visible="true">
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small"  Width="150px" />
                            <ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left"  width="150px" />
                         </asp:BoundField>

                         <asp:BoundField DataField="ACCESSION_DATE" HeaderText="Acc.Date" SortExpression="ACCESSION_DATE" DataFormatString="{0:dd/MM/yyyy}"  visible="true">
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="120px" />
                            <ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left" width="120px" />
                         </asp:BoundField>

                         <asp:BoundField DataField="VOL_NO" HeaderText="Vol" visible="true" SortExpression="VOL_NO">
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="100px" />
                            <ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left" width="100px" />
                         </asp:BoundField>

                         <asp:BoundField DataField="CLASS_NO" HeaderText="Class No" visible="true" SortExpression="CLASS_NO">
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="150px" />
                            <ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left" width="150px" />
                         </asp:BoundField>

                         <asp:BoundField DataField="BOOK_NO" HeaderText="Book No" visible="true">
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="100px" />
                            <ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left" width="100px" />
                         </asp:BoundField>

                         <asp:BoundField DataField="PAGINATION" HeaderText="Pages"   visible="true">
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="180px" />
                            <ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left" width="180px" />
                         </asp:BoundField>

                         <asp:BoundField DataField="PHYSICAL_LOCATION" HeaderText="Location" visible="true" SortExpression="PHYSICAL_LOCATION">
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="180px" />
                            <ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left" width="180px" />
                         </asp:BoundField>
                         <asp:BoundField DataField="COLLECTION_TYPE" HeaderText="Collection"  SortExpression="COLLECTION_TYPE" visible="true">
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="100px" />
                            <ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left" width="100px" />
                         </asp:BoundField>
                          <asp:BoundField DataField="LIB_CODE" HeaderText="Library"  SortExpression="LIB_CODE" visible="true">
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="100px" />
                            <ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left" width="100px" />
                         </asp:BoundField>

                         <asp:BoundField DataField="ACQ_ID" HeaderText="ACQ_ID"  visible="true">
                            <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" Width="60px" />
                            <ItemStyle Font-Names="Arial" forecolor="#0066FF" horizontalalign="Left" width="60px" />
                         </asp:BoundField>
                         
                         
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
                         Font-Bold="True" Font-Size="Small" ForeColor="White" HorizontalAlign="Center" 
                         VerticalAlign="Middle" />
                     <RowStyle BackColor="#DADADA" BorderColor="Desktop" ForeColor="#3399FF" />
                     <SelectedRowStyle BackColor="#ff9933" BorderColor="SteelBlue" 
                         BorderStyle="Solid" />
                     <HeaderStyle BackColor="Desktop" Font-Bold="True" Font-Italic="False" 
                         Font-Names="Times new roman" Font-Overline="False" Font-Underline="False" 
                         ForeColor="White" Width="90%" />
                     <PagerSettings FirstPageText="First" LastPageText="Last" 
                         Mode="NumericFirstLast" PageButtonCount="10" Position="TopAndBottom" />
                     <AlternatingRowStyle BackColor="#EFEFEF" Font-Names="Tahoma" 
                         ForeColor="#0066FF" />
                 </asp:GridView>

             </td>
         </tr>
     </table>

            
   
                   </ContentTemplate>
                  
                    </asp:UpdatePanel>
       
        
</asp:Content>
