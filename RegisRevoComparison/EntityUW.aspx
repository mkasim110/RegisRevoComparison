﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EntityUW.aspx.cs" Inherits="RegisRevoComparison.EntityUW" EnableEventValidation="false"  Async="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    
<head runat="server">
    <title></title>
    <style>
        html,body {
    height: 97vh;
     overflow: hidden;
}
        
         .HeaderStyle
        {
            position: absolute;
            margin-top: -1px;
            border-top: 1px solid black;
            border-bottom: 1px solid black;
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
      }
  
</script>
     
</head>
<body  onload="javascript:HideProgressBar()">
    <form id="form1" runat="server">
         <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeOut="300">
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
                     <asp:ListItem Value="A">AUDIT Report</asp:ListItem>
                </asp:RadioButtonList>
        </li></ul>
                  
                   
<ul class="nav navbar-nav navbar-right">
    
     
                     <%--<li style="margin-right:20px;"><asp:TextBox CssClass="form-control" runat="server"  autocomplete="off" ID="txtUW"  placeholder="Search UnderWriter" ></asp:TextBox></li>--%>
    <li>                   <asp:Button ID="BtnExport" Text="Export ENT/UW Report" class="btn btn-success" runat="server"  OnClick="BtnExport_Click"   /></li>
                    <li><asp:Button ID="BtnRefresh" Text="Refresh" class="btn btn-primary" runat="server" OnClick="BtnRefresh_Click"  OnClientClick="javascript:ShowProgressBar()" /></li>
    <li><asp:TextBox ID="txtPeriod" Visible="false" runat="server" ForeColor="Black" ></asp:TextBox></li>
    <li><asp:TextBox ID="txtLe" Visible="false" runat="server" ForeColor="Black" ></asp:TextBox></li>
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
           <table><tr>
               <td style="width:20%;">
                   <div class="scroll">
                 <asp:GridView runat="server" ID="grdEntityCnt" DataKeyNames="EntityName" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-condensed">
                    <HeaderStyle BackColor="Black" Font-Bold="false"  ForeColor="White"  />
                     <Columns>  
                          <asp:TemplateField>
<ItemTemplate>
<asp:CheckBox ID="chkEnt" runat="server" AutoPostBack="true" onclick="CheckSingleCheckbox(this);" OnCheckedChanged="chkEnt_CheckedChanged1" />
</ItemTemplate>
</asp:TemplateField>
                     <asp:BoundField DataField="EntityName" HeaderText="Entity" ReadOnly="True" SortExpression="EntityName" />  
                     
</Columns>
                 </asp:GridView>
                       </div>
                   </td> 
               <td style="width:30%;">
                    <div class="scroll">
                 <asp:GridView runat="server"  ID="grdUWCount" ShowHeaderWhenEmpty="true" AutoGenerateColumns="False"
                 OnRowDataBound="grdUWCount_RowDataBound"    CssClass="table table-striped table-bordered table-hover table-condensed">
                      <HeaderStyle BackColor="Black" Font-Bold="false" ForeColor="White"  />
                     <Columns>  
                         <asp:TemplateField>
<ItemTemplate>
<asp:CheckBox ID="chkEntStatus" runat="server" AutoPostBack="true" onclick="CheckSingleCheckbox(this);" OnCheckedChanged="chkEntStatus_CheckedChangedUW" />
</ItemTemplate>
</asp:TemplateField>
                         
                     <asp:BoundField DataField="EntityName" HeaderText="Underwriter" ReadOnly="True" SortExpression="EntityName" />  
                         
                     <asp:BoundField DataField="Count" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="Right" HeaderText="Count" SortExpression="Cnt"  /> 
                        
</Columns>
                 </asp:GridView>
                        </div>
               </td>
                 <td style="width:20%;">
                   <ul style="list-style-type: none;">
    
                    
                    <li><asp:Button ID="btnClear" Visible="false" Text="Clear Criteria" class="btn btn-primary" runat="server" OnClick="btnClear_Click"  /></li>
                    <li style="margin-top:10px;"><asp:TextBox CssClass="form-control" Visible="false" runat="server" autocomplete="off" ID="txtProgramNumber" placeholder="Atleast 4 Number of Progam" TextMode="Number" ></asp:TextBox></li>
                       <li style="margin-top:10px;"><asp:Label  runat="server"  ID="lblData"  ></asp:Label></li>
                       </ul>

               </td>
                 <td style="width:20%;">
                <div style="display:none;" class="scroll">
                 <asp:GridView runat="server"  Visible="false" ID="grdUYCnt" DataKeyNames="UY" ShowHeaderWhenEmpty="true" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-condensed">
                     <HeaderStyle BackColor="Black" Font-Bold="false"  ForeColor="White"  />
                     <Columns>  
                         <asp:TemplateField>
<ItemTemplate>
<asp:CheckBox ID="chkUY" runat="server" AutoPostBack="true" onclick="CheckSingleCheckbox(this);" OnCheckedChanged="chkUY_CheckedChangedUY" />
</ItemTemplate>
</asp:TemplateField>
                          <asp:BoundField DataField="UY" HeaderText="UY" SortExpression="UY"  /> 
                    
                          
                     <asp:BoundField DataField="Count" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="Right" HtmlEncode="false" HeaderText="Count" SortExpression="Count"  /> 
                        
</Columns>
                 </asp:GridView>
                    </div>
                </td>

                

                                <td style="width:20%;">

                    <div style="display:none;" class="scroll">
                 <asp:GridView runat="server"  ID="grdStatusCount" ShowHeaderWhenEmpty="true"  DataKeyNames="EntityName" 
                    OnRowDataBound="grdStatusCount_RowDataBound" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-condensed">
                     <HeaderStyle BackColor="Black" Font-Bold="false"  ForeColor="White"  />
                     <Columns>  
                         <asp:TemplateField>
<ItemTemplate>
<asp:CheckBox ID="chkEntStatus" runat="server" AutoPostBack="true" onclick="CheckSingleCheckbox(this);" OnCheckedChanged="chkEntStatus_CheckedChangedStatus" />
</ItemTemplate>
</asp:TemplateField>
                         
                   
                          <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Cnt"  /> 
                        
                     <asp:BoundField DataField="Count" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="Right" HeaderText="Count" SortExpression="Cnt"  /> 
                        
</Columns>
                 </asp:GridView>
                    </div>
               </td>

             
                </tr></table>
                </ContentTemplate>
                </asp:UpdatePanel>
        </div>

        <!-- Footer-->
         <div class="row">
             
                  <table style="width:100%;">
                      <tr style="width:100%;">
                           <td style="width:100%;">
                       <div class="Mainscroll">
                
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" updatemode="Conditional">
                <Triggers>
            <asp:AsyncPostBackTrigger controlid="grdResult"  />
        </Triggers>
                <ContentTemplate>


                    <asp:GridView runat="server" ShowHeaderWhenEmpty="true" ID="grdResult"  OnRowCommand="grdResult_RowCommand"
                        DataKeyNames="ContractId" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-condensed">
                     <HeaderStyle BackColor="Black"  ForeColor="White"  />
                     <Columns>  
                         
                         
                     <asp:BoundField DataField="EntityName" HeaderText="Entity" ReadOnly="True" SortExpression="EntityName" />  
                          <asp:BoundField DataField="PlatformId" HeaderText="PlatformId" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" SortExpression="Cnt"  />
                     <asp:BoundField DataField="MasterKey" HeaderText="MasterKey" SortExpression="Cnt"  /> 
                         <asp:BoundField DataField="Status" HeaderText="Status" ReadOnly="True" SortExpression="EntityName" />  
                          
                     <asp:BoundField DataField="UW" HeaderText="Underwriter" SortExpression="Cnt"  />
                         <asp:BoundField DataField="RelUW" HeaderText="Rel Underwriter" ReadOnly="True" SortExpression="EntityName" />  
                          
                     <asp:BoundField DataField="FieldDiff" HeaderText="Field Difference" SortExpression="Cnt"  />
                         <asp:BoundField DataField="REGIS" HeaderText="REGIS" ReadOnly="True" SortExpression="EntityName" />  
                          
                     <asp:BoundField DataField="REVO" HeaderText="REVO" SortExpression="Cnt"  />
                      <%--<asp:TemplateField ShowHeader="False">
            <ItemTemplate>
                <asp:Button ID="Button1" runat="server" class="btn btn-primary btn-sm" CausesValidation="false" CommandName="OpenPopup"
                    Text="Exc/Inc" CommandArgument='<%# Eval("RptCol")+","+Eval("PlatformId") %>' />
            </ItemTemplate>
        </asp:TemplateField>--%>
                        
</Columns>
                        <EmptyDataTemplate>
        <div style="text-align:center;">No records found.</div>
    </EmptyDataTemplate>
                 </asp:GridView>
                   
                
                    </ContentTemplate>
                        </asp:UpdatePanel>
                             </div>
                </td>
                           <td style="width:25%;">
                               <div style="display:none;" class="scroll2">
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" updatemode="Conditional">
                <Triggers>
            <asp:AsyncPostBackTrigger controlid="grdResult"  />
        </Triggers>
                <ContentTemplate>
                                   <asp:GridView runat="server"  ID="grdFieldCount" ShowHeaderWhenEmpty="true" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-condensed">
                      <HeaderStyle BackColor="Black" Font-Bold="false" ForeColor="White"  />
                     <Columns>  
                         <asp:TemplateField>
<ItemTemplate>
<asp:CheckBox ID="chkEntStatus" runat="server" AutoPostBack="true" onclick="CheckSingleCheckbox(this);" OnCheckedChanged="chkEntStatus_CheckedChanged1" />
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
                            <asp:GridView ID="grdExcluded" runat="server" OnRowCommand="grdExcluded_RowCommand"
                                AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover table-condensed">
                     <HeaderStyle BackColor="Black"  ForeColor="White"  />
        <Columns>
            <asp:TemplateField HeaderText="Field Desc">
                    <ItemTemplate>
                        <asp:Label ID="lblCustomerID" Text='<%#Eval("Field_desc") %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Platform Id">
                    <ItemTemplate>
                        <asp:Label ID="lblName" Text='<%#Eval("PlatformId") %>' runat="server" />
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
                            
                            <asp:Label ID="lblMsg"  runat="server" />
                             <asp:Label ID="lblField"  runat="server" />
                             
                             <asp:Label ID="lblPID" runat="server" Visible="false" />
                            <asp:Button ID="btnExc" Text="Exclude" class="btn btn-danger btn-sm" runat="server" Visible="false" OnClick="btnExc_Click"   /></li>
                             <asp:Button ID="btnInc" Text="Include" class="btn btn-success btn-sm" runat="server" Visible="false" OnClick="btnInc_Click"   /></li>
                        </div>
                         </div>
                    <div class="modal-footer">
                        <label id="SearchstsLbl" style="color:red"></label>
                        <a href="#" class="btn btn-default" data-dismiss="modal">Cancel</a>
                        


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
                 $("#myModal").modal('show');
               
    };
           function ShowPopup2() {
               $("#myModal").modal('hide');
               $('body').removeClass('modal-open');
               $('.modal-backdrop').remove();
                 $("#myModal").modal('show');
               
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
         
</script> 
    </form>
</body>
</html>