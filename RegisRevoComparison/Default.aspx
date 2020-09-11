<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="RegisRevoComparison.Default" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" /> 
    
    <title></title>
</head>
<body>
    <form id="form1" runat="server" style="align-content:center;">
    <asp:ScriptManager runat="server"></asp:ScriptManager>        
        <%-- <rsweb:ReportViewer ID="ReportViewer1" CssClass="none" runat="server" Width="100%" ProcessingMode="Remote"></rsweb:ReportViewer>--%>

        <asp:Button ID="btnGenerateReport" Text="Generate Report" runat="server" OnClick="btnGenerateReport_Click" />
    </form>
</body>
</html>
