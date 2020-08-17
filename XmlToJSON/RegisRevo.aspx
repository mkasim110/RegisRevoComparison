<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegisRevo.aspx.cs" Inherits="XmlToJSON.RegisRevo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table>
                <tr>
                    <td>
                        Effective Date
                    </td>
                    <td>
                        <asp:TextBox ID="txtEffDt" runat="server"></asp:TextBox>                  

                    </td>
                </tr>
                 <tr>
                    <td>
                        Filter Options
                    </td>
                    <td>
                       
                      
                    </td>
                </tr>
                 <tr>
                    <td>
                        Legal Ent Code
                    </td>
                    <td>
                       <asp:CheckBox ID="CheckBox2" runat="server" oncheckedchanged="CheckBox1_CheckedChanged" AutoPostBack="true" />
                      
                    </td>
                </tr>
                 <tr>
                    <td>
                        PGM UW
                    </td>
                    <td>
                       <asp:CheckBox ID="CheckBox3" runat="server" oncheckedchanged="CheckBox1_CheckedChanged" AutoPostBack="true" />
                      
                    </td>
                </tr>
                 <tr>
                    <td>
                       Segment
                    </td>
                    <td>
                       <asp:CheckBox ID="CheckBox1" runat="server" oncheckedchanged="CheckBox1_CheckedChanged" AutoPostBack="true" />
                      
                    </td>
                </tr>
                 <tr>
                    <td>
                       
                    </td>
                    <td>
                       <asp:Button ID="btnGenerateRpt" runat="server" Text="Generate Report" AutoPostBack="true" />
                      
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
