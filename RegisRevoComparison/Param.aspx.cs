using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RegisRevoComparison
{
    public partial class Param : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGenerateReport_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("http://financeapps1:8080/Regis_Reports/REGREVCOMP.rpt?prompt0=" + txtEntity.Text + "&prompt1=" + txtMasterKey.Text + "&prompt2=" + txtUW.Text + "&prompt3=" + txtSegment.Text + "", false);
                Context.ApplicationInstance.CompleteRequest();
            }catch(Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}