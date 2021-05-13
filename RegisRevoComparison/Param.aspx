<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Param.aspx.cs" Inherits="RegisRevoComparison.Param" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
         .lblStyl {
            
            text-align: right;
           
        }
    </style>
</head>
<body style="text-align: center;">
    <form id="form1" runat="server" style="  display: inline-block;margin-top:12%;">
        <div>
            <h3>REGIS REVO Contract Comparison</h3>
            <br />
            <table>
                
                
                <tr>
                    <td class="lblStyl">
                        <label >Entity</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEntity" runat="server" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="lblStyl">
                         <label >Master key</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtMasterKey" runat="server" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="lblStyl">
                         <label >UW</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtUW" runat="server" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="lblStyl">
                         <label >Segment</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtSegment" runat="server" ></asp:TextBox>
                    </td>
                </tr>
               
                <tr>
                    
                    <td></td>
                    <td>
                        <asp:Button ID="btnGenerateReport" Text="Show Report" runat="server" OnClick="btnGenerateReport_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
