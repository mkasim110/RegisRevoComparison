<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegisRevoFilter.aspx.cs" Inherits="RegisRevoComparison.RegisRevoFilter" EnableEventValidation="false"  Async="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    
<head runat="server">
    <title></title>
    <style>
        html,body {
    height: 97vh;
     overflow: hidden;
     font-family:Calibri;
}
        
         .HeaderStyle
        {
            position: absolute;
            margin-top: -1px;
            font-size:10px;
            font-family:Calibri;
            border-top: 1px solid black;
            border-bottom: 1px solid black;
        }
           .centerHeaderText th {
        text-align: center;
    }
        .radioButtonList label{
    display:inline;
    padding-right: 30px;
    margin-bottom:3px;
}
         .hiddencol
  {
    display: none;
  }
        .navbar-nav > li{
  padding-left:10px;
  padding-top:10px;
  padding-right:10px;
  color:white;
}
      
        #loaderDiv {
  width: 100%;
  height: 100%;
  top: 0;
  left: 0;
  position: fixed;
  display: block;
  opacity: 0.7;
  background-color: #fff;
  z-index: 99;
  text-align: center;
}
        table tr td .scroll{
  height:25vh;
  margin:5px;
   overflow-y: scroll;
   overflow-x: hidden;
  margin-bottom:10px;
}
         .scroll2{
 height:58vh;
 width:99%;
  margin:5px;
   overflow-y: scroll;
   overflow-x: hidden;
  margin-top:10px;
}
     
                  .Mainscroll{
 height: 58vh;
width:99%;
margin:5px;
   overflow-y: scroll;
   overflow-x: hidden;
  margin-top:10px;
}
         
    </style>
     <script src="js/jquery.min.js" type="text/javascript"></script>
<script src="js/jquery-ui.min.js" type="text/javascript"></script>
<link href="js/jquery-ui.css" rel="Stylesheet" type="text/css" />
    <link href="js/myStyle.css" rel="Stylesheet" type="text/css" />
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <script src="js/REGREV.js"></script>
    <script src="js/jquery-1.10.2.min.js"></script>
    <script src="js/ScrollableGrid.js"></script>
    <script type="text/javascript">
   
 
      function ShowProgressBar() {
        document.getElementById('dvProgressBar').style.visibility = 'visible';
      }

        function HideProgressBar() {
          
          document.getElementById('dvProgressBar').style.visibility = "hidden";
          document.getElementById("txtReason").style.visibility="hidden";
      }
  
</script>
     
</head>
<body  onload="javascript:HideProgressBar()" style="font-family:Calibri;">
    <form id="form1" runat="server">
         <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeOut="1000">
    </asp:ScriptManager>
       <div class="container-fluid">
        <div class="row">
             <asp:UpdatePanel ID="UpdatePanel1" runat="server" updatemode="Conditional">
                 <Triggers>
            <asp:AsyncPostBackTrigger controlid="rdBtnRptType" eventname="SelectedIndexChanged" />
                     
                     
        </Triggers>
                <ContentTemplate>
            <nav class="navbar navbar-inverse">
  <div class="container-fluid">
   
    <ul class="nav navbar-nav">
    <li class="radio">
                    <asp:RadioButtonList runat="server" ID="rdBtnRptType" CssClass="radioButtonList" AutoPostBack="True" 
                        OnSelectedIndexChanged="rdBtnRptType_SelectedIndexChanged" RepeatDirection="Horizontal" >
                    <asp:ListItem Value="U" Selected="True">UW Report</asp:ListItem>
                     <asp:ListItem Value="A">Audit Report</asp:ListItem>
                </asp:RadioButtonList>
        </li>
        <li> <asp:textbox id="txtSearch" runat="server" autocomplete="off" CssClass="form-control" font-family="Calibri" ForeColor="Black"  Placeholder="Search Rel UW" onkeyup="Search_Gridview(this, 'grdUYCnt')"></asp:textbox></li>
    </ul>
                  
                   
<ul class="nav navbar-nav navbar-right">
    
     
                     
    <li>                   <asp:Button ID="BtnExport" Text="Export Excel" class="btn btn-success" runat="server"  OnClick="BtnExport_Click"   /></li>
    <li>                   <asp:Button ID="btnExportPdf" Text="Export Pdf" class="btn btn-success" runat="server"  OnClick="btnExportPdf_Click"   /></li>
                    <li><asp:Button ID="BtnRefresh" Text="Refresh" class="btn btn-primary" runat="server" OnClick="BtnRefresh_Click"  OnClientClick="javascript:ShowProgressBar()" /></li>
    
     <li><asp:Button ID="btnEntUWRpt" Text="Ent/UW Report"   Visible="false" class="btn btn-primary" runat="server" OnClick="btnEntUWRpt_Click"   /></li>
   <li><asp:Button ID="btnEntUw" Text="Excluded Data Report"   class="btn btn-primary" runat="server" OnClick="btnEntUw_Click"   /></li>
                    <li><label style="margin-right:50px;" runat="server" id="lblUser">Windows User</label></li>
    </ul>
                        
                </div>
                </nav>
                    </ContentTemplate>
                 </asp:UpdatePanel>
            </div>
        

        <!-- main -->
        <div class="row">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" updatemode="Conditional">
                 <Triggers>
                     <asp:AsyncPostBackTrigger ControlID="grdEntityCnt" />
                      <asp:AsyncPostBackTrigger ControlID="grdUYCnt" />
                 </Triggers>
                <ContentTemplate>
          <table style="width:100%;">
                      <tr style="width:100%;">
                

                 <td style="width:26%;">
                <div class="scroll">
                 <asp:GridView runat="server"  ID="grdUYCnt"   DataKeyNames="EntityName" ShowHeaderWhenEmpty="true" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-condensed">
                     <HeaderStyle BackColor="Black"  Font-Bold="false" CssClass="centerHeaderText"  ForeColor="White"  />
                     <Columns>  
                         <asp:TemplateField>
<ItemTemplate>
<asp:CheckBox ID="chkUY" runat="server" AutoPostBack="true"  OnCheckedChanged="chkEnt_CheckedChanged1" />
</ItemTemplate>
</asp:TemplateField>
                          <asp:BoundField DataField="EntityName" HeaderText="Rel UW" SortExpression="UY"  /> 
                    
                          
                     <%--<asp:BoundField DataField="Count"  DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="Right" HtmlEncode="false" HeaderText="Count" SortExpression="Count"  /> --%>
                        
</Columns>
                 </asp:GridView>
                    </div>
                </td>

                <td style="width:0%;display:none;">
                    <div class="scroll">
                 <asp:GridView runat="server"  ID="grdUWCount" ShowHeaderWhenEmpty="true" AutoGenerateColumns="False"
                 OnRowDataBound="grdUWCount_RowDataBound"    CssClass="table table-striped table-bordered table-hover table-condensed">
                      <HeaderStyle BackColor="Black" CssClass="centerHeaderText" Font-Bold="false" ForeColor="White"  />
                     <Columns>  
                         <asp:TemplateField>
<ItemTemplate>
<asp:CheckBox ID="chkEntStatus" runat="server" AutoPostBack="true"  OnCheckedChanged="chkEntStatus_CheckedChangedUW" />
</ItemTemplate>
</asp:TemplateField>
                         
                     <asp:BoundField DataField="EntityName" HeaderText="Underwriter" ReadOnly="True" SortExpression="EntityName" />  
                         
                    <%-- <asp:BoundField DataField="Count" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="Right" HeaderText="Count" SortExpression="Cnt"  /> --%>
                        
</Columns>
                 </asp:GridView>
                        </div>
               </td>
               <td style="width:25%;">
                   <div class="scroll">
                 <asp:GridView runat="server" ID="grdEntityCnt" ShowHeaderWhenEmpty="true" DataKeyNames="EntityName" 
                     AutoGenerateColumns="False" OnRowDataBound="grdEntityCnt_RowDataBound" CssClass="table table-striped table-bordered table-hover table-condensed">
                    <HeaderStyle BackColor="Black" Font-Bold="false" CssClass="centerHeaderText"  ForeColor="White"  />
                     <Columns>  
                          <asp:TemplateField>
<ItemTemplate>
<asp:CheckBox ID="chkEnt" runat="server" AutoPostBack="true"  OnCheckedChanged="chkEntStatus_CheckedChangedUY" />
</ItemTemplate>
</asp:TemplateField>
                     <asp:BoundField DataField="EntityName" HeaderText="Entity" ReadOnly="True" SortExpression="EntityName" />  
                         <%--<asp:BoundField DataField="Cnt" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="Right" HeaderText="Count" SortExpression="Cnt"  /> --%>
                     
</Columns>
                 </asp:GridView>
                       </div>
                   </td>
                                <td style="width:25%;">

                   <div class="scroll">
                 <asp:GridView runat="server"  ID="grdStatusCount" ShowHeaderWhenEmpty="true"  DataKeyNames="EntityName" 
                    OnRowDataBound="grdStatusCount_RowDataBound" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-condensed">
                     <HeaderStyle BackColor="Black" Font-Bold="false" CssClass="centerHeaderText"  ForeColor="White"  />
                     <Columns>  
                         <asp:TemplateField>
<ItemTemplate>
<asp:CheckBox ID="chkEntStatus" runat="server" AutoPostBack="true"  OnCheckedChanged="chkEntStatus_CheckedChangedStatus" />
</ItemTemplate>
</asp:TemplateField>
                         
                   
                          <asp:BoundField DataField="Status" HeaderText="Quarter-Year" SortExpression="Cnt"  /> 
                        
                     <asp:BoundField DataField="Count" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="Right" HeaderText="Count" SortExpression="Cnt"  /> 
                        
</Columns>
                 </asp:GridView>
                    </div>
               </td>

               <td style="width:19%;">
                   <ul style="list-style-type: none;">
    
                    
                    <li><asp:Button ID="btnClear" Text="Clear Criteria" class="btn btn-primary" runat="server" OnClick="btnClear_Click"  /></li>
                    <li style="margin-top:10px;"><asp:TextBox Visible="false" CssClass="form-control" runat="server" autocomplete="off" ID="txtProgramNumber" placeholder="At least 4 Number of Progam" TextMode="Number" ></asp:TextBox></li>
                       <li style="margin-top:10px;"><asp:Label  runat="server"  ID="lblData"  ></asp:Label></li>
                        <li><asp:Button ID="btnShowExcludedFields" Visible="false" Text="Show Excluded Fields" class="btn btn-primary" runat="server" OnClientClick="ShowExluPopup();"  /></li>
                       </ul>

               </td>
                </tr></table>
                </ContentTemplate>
                </asp:UpdatePanel>
        </div>

        <!-- Footer-->
         <div class="row">
             
                  <table style="width:100%;">
                      <tr style="width:100%;">
                           <td style="width:25%;">
                               <div class="scroll2">
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" updatemode="Conditional">
                <Triggers>
            <asp:AsyncPostBackTrigger controlid="grdResult"  />
        </Triggers>
                <ContentTemplate>
                                   <asp:GridView runat="server"  ID="grdFieldCount" ShowHeaderWhenEmpty="true" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-condensed">
                      <HeaderStyle BackColor="Black" Font-Bold="false" CssClass="centerHeaderText" ForeColor="White"  />
                     <Columns>  
                         <asp:TemplateField>
<ItemTemplate>
<asp:CheckBox ID="chkEntStatus" runat="server" AutoPostBack="true"  OnCheckedChanged="chkEntStatus_CheckedChanged1" />
</ItemTemplate>
</asp:TemplateField>
                         
                     <asp:BoundField DataField="EntityName" HeaderText="Field" ReadOnly="True" SortExpression="EntityName" />  
                          
                     <asp:BoundField DataField="Count" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="Right" HeaderText="Count" SortExpression="Cnt"  /> 
                        
</Columns>
                     
                 </asp:GridView>
                    </ContentTemplate>
                                        </asp:UpdatePanel>
                              </div>
                          </td>
                          <td style="width:68%;">
                       <div class="Mainscroll">
                
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" updatemode="Conditional">
                <Triggers>
            <asp:AsyncPostBackTrigger controlid="grdResult"  />
        </Triggers>
                <ContentTemplate>


                    <asp:GridView runat="server" ShowHeaderWhenEmpty="true" ID="grdResult"  OnRowCommand="grdResult_RowCommand"
                        DataKeyNames="PlatformId" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-condensed">
                     <HeaderStyle BackColor="Black" CssClass="centerHeaderText"  ForeColor="White"  />
                     <Columns>  
                         
                          <asp:BoundField DataField="Qyear" HeaderText="Q-Year" ReadOnly="True" SortExpression="EntityName" />  
                     <asp:BoundField DataField="EntityName" HeaderText="Entity" ReadOnly="True" SortExpression="EntityName" />  
                          <asp:BoundField DataField="PlatformId" HeaderText="Master Key" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" SortExpression="Cnt"  />
                     <asp:BoundField DataField="MasterKey" HeaderText="MasterKey" SortExpression="Cnt"  /> 
                         <asp:BoundField DataField="Status" HeaderText="Cedant" ReadOnly="True" SortExpression="EntityName" />  
                          
                     <asp:BoundField DataField="UW" HeaderText="Underwriter" SortExpression="Cnt"  />
                         <asp:BoundField DataField="RelUW" HeaderText="Rel Underwriter" ReadOnly="True" SortExpression="EntityName" />  
                          
                     <asp:BoundField DataField="FieldDiff" HeaderText="Field Difference" SortExpression="Cnt"  />
                         <asp:BoundField DataField="REGIS" HeaderText="REGIS" ReadOnly="True" SortExpression="EntityName" />  
                          
                     <asp:BoundField DataField="REVO" HeaderText="REVO" SortExpression="Cnt"  />
                      <asp:TemplateField HeaderText="Exclude">
            <ItemTemplate>
                <asp:Button ID="Button1" runat="server" class="btn btn-primary btn-sm" CausesValidation="false" CommandName="OpenPopup"
                    Text="Exclude" CommandArgument='<%# Eval("RptCol")+","+Eval("MasterKey") %>' />
            </ItemTemplate>
        </asp:TemplateField>

                          <asp:TemplateField HeaderText="History">
            <ItemTemplate>
                <asp:Button ID="btnHisory" runat="server" class="btn btn-primary btn-sm" CausesValidation="false" CommandName="HistoryPopup"
                    Text="View" CommandArgument='<%# Eval("RptCol")+","+Eval("PlatformId") %>' />
            </ItemTemplate>
        </asp:TemplateField>
                        
</Columns>
                        <EmptyDataTemplate>
        <div style="text-align:center;">No records found.</div>
    </EmptyDataTemplate>
                 </asp:GridView>
                   
                
                    </ContentTemplate>
                        </asp:UpdatePanel>
                             </div>
                </td>
           
                      </tr>
                  </table>
            </div>
      
           <div id="dvProgressBar" class="spinner" >
      
  </div>
           <div id="dialog" style="display: none">
              <%-- <asp:Button ID="btnExclude" runat="server" Text="Exclude"   OnClick="btnExclude_Click" />--%>
    
</div>
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" updatemode="Conditional">
                <Triggers>
            <asp:AsyncPostBackTrigger controlid="grdExcluded"  />
                     <asp:AsyncPostBackTrigger controlid="ddlReason"  />
        </Triggers>
                <ContentTemplate>

            <div class="modal fade" id="myModal">
            <div class="modal-dialog">
                <div class="modal-content">

                    <div class="modal-header">
                        <a href="#" class="close" data-dismiss="modal">&times;</a>
                        <h3 class="modal-title">Exclude and Include Fields</h3>

                    </div>
                  <div class="modal-body" > 
                      <div class="row">
                          <table style="margin:0 auto">
                                <tr>
                                    <td>
                            <asp:Label ID="lblReason" Text="Reason" Font-Bold="true" runat="server"></asp:Label>
                                        </td><td>
                            <asp:DropDownList  ID="ddlReason" runat="server" Width="250px"  onchange="return FilterStatus()" CssClass="form-control">
                               
                               <%-- <asp:ListItem Value="Others">Others</asp:ListItem>--%>
                            </asp:DropDownList>
                                            </td>
                                    </tr>
                                <tr>
                                    <td></td>
                                    <td>
                            <asp:TextBox ID="txtReason" CssClass="form-control" Width="250px" runat="server" autocomplete="off" ></asp:TextBox>
                                        </td>
                                    </tr>
                                <tr>
                                    <td></td>
                                  <td>
                                      <asp:Label ID="lblMsg"  runat="server" />
                             <asp:Label ID="lblField" Font-Bold="true" runat="server" />
                             
                             <asp:Label ID="lblPID" runat="server" Visible="false" />
                            <asp:Button ID="btnExc" Text="Exclude" class="btn btn-danger btn-sm" runat="server" Visible="false" OnClick="btnExc_Click"   /></li>
                             <asp:Button ID="btnInc" Text="Include" class="btn btn-success btn-sm" runat="server" Visible="false" OnClick="btnInc_Click"   /></li>
                                  </td>
                                </tr>
                           </table>
                            <br />
                      </div>
                        <div class="row">
                            <asp:GridView ID="grdExcluded" runat="server" OnRowCommand="grdExcluded_RowCommand"
                                AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover table-condensed">
                     <HeaderStyle BackColor="Black" CssClass="centerHeaderText"  ForeColor="White"  />
        <Columns>
            <asp:TemplateField HeaderText="Field Desc">
                    <ItemTemplate>
                        <asp:Label ID="lblCustomerID" Text='<%#Eval("Field_desc") %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Master Key">
                    <ItemTemplate>
                        <asp:Label ID="lblName" Text='<%#Eval("PlatformId") %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
             <asp:TemplateField HeaderText="Reason">
                    <ItemTemplate>
                        <asp:Label ID="lblReason" Text='<%#Eval("Reason") %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>

          <asp:TemplateField HeaderText="Include">
            <ItemTemplate>
                <asp:Button ID="Button1" runat="server" class="btn btn-primary btn-sm" CausesValidation="false" CommandName="cmdExcludeField"
                    Text="Include" CommandArgument='<%# Eval("Field_desc")+","+Eval("PlatformId") %>' />
            </ItemTemplate>
        </asp:TemplateField>
             
        </Columns>
                                <EmptyDataTemplate>
        <div style="text-align:center;">No records found.</div>
    </EmptyDataTemplate>
    </asp:GridView>
                            
                            
                        </div>
                         </div>
                    <div class="modal-footer">
                        <label id="SearchstsLbl" style="color:red"></label>
                        <a href="#" class="btn btn-default" data-dismiss="modal">Close</a>
                        


                    </div>
            </div>
</div>
        </div>
                     </ContentTemplate>
                        </asp:UpdatePanel>

           <asp:UpdatePanel ID="UpdatePanel6" runat="server" updatemode="Conditional">
                <Triggers>
            <asp:AsyncPostBackTrigger controlid="grdExcluded"  />
                     <asp:AsyncPostBackTrigger controlid="ddlReason"  />
        </Triggers>
                <ContentTemplate>

            <div class="modal fade" id="myExclModal">
            <div class="modal-dialog">
                <div class="modal-content">

                    <div class="modal-header">
                        <a href="#" class="close" data-dismiss="modal">&times;</a>
                        <h3 class="modal-title">Excluded Fields by Current Filter</h3>

                    </div>
                  <div class="modal-body" > 
                     
                        <div class="row">
                            <asp:GridView ID="grdExcluded1" runat="server" OnRowCommand="grdExcluded1_RowCommand"
                                AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover table-condensed">
                     <HeaderStyle BackColor="Black" CssClass="centerHeaderText"  ForeColor="White"  />
        <Columns>

            <asp:TemplateField HeaderText="Field Desc">
                    <ItemTemplate>
                        <asp:Label ID="lblFieldID" Text='<%#Eval("[Field Difference]") %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Master Key">
                    <ItemTemplate>
                        <asp:Label ID="lblName" Text='<%#Eval("[MasterKey]") %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
             <asp:TemplateField HeaderText="Reason">
                    <ItemTemplate>
                        <asp:Label ID="lblReason" Text='<%#Eval("Reason") %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>

          <asp:TemplateField HeaderText="Include">
            <ItemTemplate>
                <asp:Button ID="Button1" runat="server" class="btn btn-primary btn-sm" CausesValidation="false" CommandName="cmdExcludeField"
                    Text="Include" CommandArgument='<%# Eval("[Field Difference]")+","+Eval("MasterKey") %>' />
            </ItemTemplate>
        </asp:TemplateField>
             <asp:TemplateField HeaderText="History">
            <ItemTemplate>
                <asp:Button ID="btnHisory1" runat="server" class="btn btn-primary btn-sm" CausesValidation="false" CommandName="HistoryPopup"
                    Text="View" CommandArgument='<%# Eval("[Field Difference]")+","+Eval("PlatformId") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        </Columns>
                                <EmptyDataTemplate>
        <div style="text-align:center;">No records found.</div>
    </EmptyDataTemplate>
    </asp:GridView>                           
                        </div>
                       <div>
                         <table>
                             <tr>
                                    <td>
                            <asp:Label ID="slcField" Text="Field Name: " Visible="false" Font-Bold="true" runat="server"></asp:Label>
                                        </td><td>
                             <asp:Label ID="selectedFieldName2"  Font-Bold="true" runat="server"></asp:Label>
                                            </td>
                                    </tr>
                         </table>
                     </div>
                       <div class="row">
                            <asp:GridView ID="GrdExclHistory" runat="server" 
                                AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover table-condensed">
                     <HeaderStyle BackColor="Black" CssClass="centerHeaderText"  ForeColor="White"  />
        <Columns>
            <asp:TemplateField HeaderText="Date Refreshed">
                    <ItemTemplate>
                        <asp:Label ID="lblName" Text='<%#Eval("RefreshedDated") %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
            <asp:TemplateField HeaderText="REGIS">
                    <ItemTemplate>
                        <asp:Label ID="lblFieldID" Text='<%#Eval("Regis") %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>                
             <asp:TemplateField HeaderText="REVO">
                    <ItemTemplate>
                        <asp:Label ID="lblReason" Text='<%#Eval("Revo") %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>

          
        </Columns>
                                <EmptyDataTemplate>
        <div style="text-align:center;">No records found.</div>
    </EmptyDataTemplate>
    </asp:GridView>
                            
                            
                        </div>
                         </div>
                    <div class="modal-footer">
                        <label id="SearchstsLbl1" style="color:red"></label>
                        <a href="#" class="btn btn-default" data-dismiss="modal">Close</a>
                        


                    </div>
            </div>
</div>
        </div>

                    <div class="modal fade" id="myHistoryModal">
            <div class="modal-dialog">
                <div class="modal-content">

                    <div class="modal-header">
                        <a href="#" class="close" data-dismiss="modal">&times;</a>
                        <h3 class="modal-title">History of the Field</h3>

                    </div>
                  <div class="modal-body" > 
                     <div>
                         <table>
                             <tr>
                                    <td>
                            <asp:Label ID="Label1" Text="Field Name: " Font-Bold="true" runat="server"></asp:Label>
                                        </td><td>
                             <asp:Label ID="selectedFieldName" Text="Field Name: " Font-Bold="true" runat="server"></asp:Label>
                                            </td>
                                    </tr>
                         </table>
                     </div>
                        <div class="row">
                            <asp:GridView ID="grdHistory" runat="server" 
                                AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover table-condensed">
                     <HeaderStyle BackColor="Black" CssClass="centerHeaderText"  ForeColor="White"  />
        <Columns>
            <asp:TemplateField HeaderText="Date Refreshed">
                    <ItemTemplate>
                        <asp:Label ID="lblName" Text='<%#Eval("RefreshedDated") %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
            <asp:TemplateField HeaderText="REGIS">
                    <ItemTemplate>
                        <asp:Label ID="lblFieldID" Text='<%#Eval("Regis") %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>                
             <asp:TemplateField HeaderText="REVO">
                    <ItemTemplate>
                        <asp:Label ID="lblReason" Text='<%#Eval("Revo") %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>

          
        </Columns>
                                <EmptyDataTemplate>
        <div style="text-align:center;">No records found.</div>
    </EmptyDataTemplate>
    </asp:GridView>
                            
                            
                        </div>
                         </div>
                    <div class="modal-footer">
                        
                        <a href="#" class="btn btn-default" data-dismiss="modal">Close</a>
                        


                    </div>
            </div>
</div>
        </div>
                     </ContentTemplate>
                        </asp:UpdatePanel>
    </div>
        
        <script src="js/bootstrap.min.js"></script>
       <script lang="javascript" type="text/javascript">  
           
         
           function ShowPopup() {
                 document.getElementById("txtReason").style.visibility="hidden";
                 $("#myModal").modal('show');
               
           };
           function ShowExluPopup() {
              
               $("#myExclModal").modal('show');

           };
           function ShowHistoryPopup() {

               $("#myHistoryModal").modal('show');

           };
           function ShowPopup2() {
               $("#myModal").modal('hide');
               $('body').removeClass('modal-open');
               $('.modal-backdrop').remove();
                 $("#myModal").modal('show');
               
           };
           function ShowExclPopup2() {
               $("#myExclModal").modal('hide');
               $('body').removeClass('modal-open');
               $('.modal-backdrop').remove();
               $("#myExclModal").modal('show');

           };
         function CheckSingleCheckbox(ob) {
        var grid = ob.parentNode.parentNode.parentNode;
        var inputs = grid.getElementsByTagName("input");
        for (var i = 0; i < inputs.length; i++) {
            if (inputs[i].type == "checkbox") {
                if (ob.checked && inputs[i] != ob && inputs[i].checked) {
                    inputs[i].checked = false;
                }
            }
        }
           }       

           function FilterStatus()
           {
               var drpFilterType = document.getElementById("ddlReason");
var selectedFilterType = drpFilterType.options[drpFilterType.selectedIndex].text;

if (selectedFilterType == "Others")
{

document.getElementById("txtReason").style.visibility="visible";

}

else
{
document.getElementById("txtReason").style.visibility="hidden";
}

}
   
   
     
       </script> 
        <script type="text/javascript">
    function Search_Gridview(strKey, strGV) {
        var strData = strKey.value.toLowerCase().split(" ");
        var tblData = document.getElementById(strGV);
        var rowData;
        for (var i = 1; i < tblData.rows.length; i++) {
            rowData = tblData.rows[i].innerHTML;
            var styleDisplay = 'none';
            for (var j = 0; j < strData.length; j++) {
                if (rowData.toLowerCase().indexOf(strData[j]) >= 0)
                    styleDisplay = '';
                else {
                    styleDisplay = 'none';
                    break;
                }
            }
            tblData.rows[i].style.display = styleDisplay;
        }
    }    
</script>
    </form>
</body>
</html>
