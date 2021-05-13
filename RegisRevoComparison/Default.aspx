<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="RegisRevoComparison.Default"  Async="true" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" /> 
    
    <title></title>
    <script src="js/jquery.min.js" type="text/javascript"></script>
<script src="js/jquery-ui.min.js" type="text/javascript"></script>
<link href="js/jquery-ui.css" rel="Stylesheet" type="text/css" />
    <link href="js/myStyle.css" rel="Stylesheet" type="text/css" />
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
<script type="text/javascript">
    $(function () {
        $("[id*=txtDate]").datepicker({
            showOn: 'button',           
            dateFormat: "yy-mm-dd",
            changeMonth: true, changeYear: true,
            buttonText:  'Choose Date'
        });
    });
 
      function ShowProgressBar() {
        document.getElementById('dvProgressBar').style.visibility = 'visible';
      }

      function HideProgressBar() {
        document.getElementById('dvProgressBar').style.visibility = "hidden";
      }
  
</script>
   <style>
         .lblStyl {
            
            text-align: right;
           
        }
    </style>
</head>
<body style="text-align: center;" onload="javascript:HideProgressBar()" class="container">
    <form id="form1" runat="server" style="  display: inline-block;margin-top:12%;">
    <asp:ScriptManager runat="server"></asp:ScriptManager>  
        <h3>REGIS REVO CONTRACT COMPARISON</h3>
            <br />
      
      
     
       <div class="panel panel-primary">
            <br />
            <table class="table">
                
               <%-- <tr>
                    <td class="lblStyl">
                         <label>Date</label>
                    </td>
                    <td style="padding-left:0px;">
                        <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" ReadOnly = "true"></asp:TextBox>
                    </td>
                </tr>--%>
                <tr>
                    <td class="lblStyl">
                        <label >Reconciliation Report</label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlReportVers" CssClass="form-control" runat="server" autocomplete="off" >
                            <asp:ListItem>UW</asp:ListItem>
                            <asp:ListItem>Audit</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="lblStyl">
                        <label >Entity</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEntity" CssClass="form-control" runat="server" autocomplete="off" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="lblStyl">
                         <label >Master key</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtMasterKey" CssClass="form-control" runat="server" autocomplete="off" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="lblStyl">
                         <label >UW</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtUW" CssClass="form-control" runat="server" autocomplete="off" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="lblStyl">
                         <label >Segment</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtSegment" CssClass="form-control" runat="server" autocomplete="off" ></asp:TextBox>
                    </td>
                </tr>
               
                <tr>
                    
                    <td></td>
                    <td>
                      <%--  <asp:Button ID="Button1" Text="Show Report" runat="server" OnClick="btnGenerateReport_Click" />--%>
                    </td>
                </tr>
            </table>
        </div>
        <asp:Button ID="btnGenerateReport" Text="Generate Report" class="btn" runat="server" OnClick="btnGenerateReport_Click"  OnClientClick="javascript:ShowProgressBar()" />

         <div id="dvProgressBar" class="spinner" >
        <%--<img src="images/progres_bar.gif" /> Generating Report, please wait...--%>
  </div>
  <br style="clear:both" />
    </form>
    <script src="js/bootstrap.min.js"></script>
</body>
</html>
