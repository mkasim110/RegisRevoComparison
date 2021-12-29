using ClosedXML.Excel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Font = iTextSharp.text.Font;
using System.Linq;
using System.Text.RegularExpressions;

namespace RegisRevoComparison
{
    public partial class RegisRevoFilter : System.Web.UI.Page
    {
        List<Contract> myDeserializedClass;
        DataTable dt = new DataTable();
        DataRow dr;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack && !Page.IsCallback)
            {
                BindRefresh();
                ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "Popup", "HideProgressBar();", true);
            }
            System.Web.UI.ScriptManager.GetCurrent(this).RegisterPostBackControl(BtnExport);
            System.Web.UI.ScriptManager.GetCurrent(this).RegisterPostBackControl(btnExportPdf);
            System.Web.UI.ScriptManager.GetCurrent(this).RegisterPostBackControl(btnEntUw);
            System.Web.UI.ScriptManager.GetCurrent(this).RegisterPostBackControl(btnEntUWRpt);
        }

        public void BindRefresh()
        {

            using (var Context = new DbAdapter())
            {
                lblData.Text = "Data as at : " +  Context.GetDataLastUpdateDate().ToString("MMM dd yyyy hh:mmtt");
                grdUYCnt.DataSource = Context.GetRelUWCount(rdBtnRptType.SelectedValue,"","");
                grdUYCnt.DataBind();
                //ScriptManager.RegisterClientScriptBlock(this, typeof(string), "SearchBox", "Search_Gridview(this, 'grdUYCnt');", true);
                if (txtSearch.Text != "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "Search_Gridview('"+txtSearch.Text+"', 'grdUYCnt');", true);
                }
                string usernm = HttpContext.Current.User.Identity.Name.ToString();

                //usernm = usernm.Substring(usernm.IndexOf("\\")+1);
                lblUser.InnerText = usernm;
                btnShowExcludedFields.Visible = false;
                DtUW();
                UpdatePanel2.Update();
                if (txtSearch.Text != "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "Search_Gridview("+txtSearch.ClientID+", 'grdUYCnt');", true);
                }
            }
        }
        protected void DtUW()
        {
            DataTable table = new DataTable();

            table.Columns.Add(new DataColumn("UW", typeof(string)));
            table.Columns.Add(new DataColumn("Count", typeof(int)));
            table.Columns.Add(new DataColumn("Status", typeof(string)));
            table.Columns.Add(new DataColumn("Cnt", typeof(string)));
            table.Columns.Add(new DataColumn("EntityName", typeof(string)));
            table.Columns.Add(new DataColumn("UY", typeof(string)));


            table.Columns.Add(new DataColumn("PlatformId", typeof(string)));
            table.Columns.Add(new DataColumn("MasterKey", typeof(string)));

            table.Columns.Add(new DataColumn("FieldDiff", typeof(string)));
            table.Columns.Add(new DataColumn("RelUW", typeof(string)));
            table.Columns.Add(new DataColumn("REGIS", typeof(string)));
            table.Columns.Add(new DataColumn("REVO", typeof(string)));

            grdUWCount.DataSource = table;
            grdUWCount.DataBind();
            grdEntityCnt.DataSource = table;
            grdEntityCnt.DataBind();
            grdFieldCount.DataSource = table;
            grdFieldCount.DataBind();
            grdStatusCount.DataSource = table;
            grdStatusCount.DataBind();
            grdResult.DataSource = table;
            grdResult.DataBind();
        }

        protected void BindDtResult()
        {
            DataTable table = new DataTable();

            table.Columns.Add(new DataColumn("UW", typeof(string)));
            table.Columns.Add(new DataColumn("Count", typeof(int)));
            table.Columns.Add(new DataColumn("Status", typeof(string)));
            table.Columns.Add(new DataColumn("EntityName", typeof(string)));
            table.Columns.Add(new DataColumn("UY", typeof(string)));
            table.Columns.Add(new DataColumn("bounddate", typeof(string)));


            table.Columns.Add(new DataColumn("PlatformId", typeof(string)));
            table.Columns.Add(new DataColumn("MasterKey", typeof(string)));

            table.Columns.Add(new DataColumn("FieldDiff", typeof(string)));
            table.Columns.Add(new DataColumn("RelUW", typeof(string)));
            table.Columns.Add(new DataColumn("REGIS", typeof(string)));
            table.Columns.Add(new DataColumn("REVO", typeof(string)));


           // grdResult.DataSource = table;
           // grdResult.DataBind();
            UpdatePanel3.Update();
        }
        public void BindFilters(string RptType, string ent, string uw, string uy, string program, string status)
        {
            using (var Context = new DbAdapter())
            {

                grdEntityCnt.DataSource = Context.GetEntityCount(RptType, uy,uw);
                grdEntityCnt.DataBind();
                grdStatusCount.DataSource = Context.GetStatusCount(RptType, ent, uy, uw);
                grdStatusCount.DataBind();
                grdUWCount.DataSource = Context.GetUWCount(RptType, ent, uy);
                grdUWCount.DataBind();
                grdFieldCount.DataSource = Context.GetFieldCount(RptType, ent, uw, uy, program, status);
                grdFieldCount.DataBind();

                grdResult.DataSource = Context.GetCompareResult(RptType, program, ent, uy, uw, "", status);
                grdResult.DataBind();
                if(grdResult.Rows.Count>0)
                    ShowingGroupingDataInGridView(grdResult.Rows,0,8);

                UpdatePanel2.Update();
                UpdatePanel5.Update();
                if (txtSearch.Text != "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "Search_Gridview("+txtSearch.ClientID+", 'grdUYCnt');", true);
                }
            }
            //ShowingGroupingDataInGridView(grdResult.Rows,0,8);
        }
        public void BindFiltersWithoutUY(string RptType, string ent, string uw, string uy, string program, string status, string field)
        {
            using (var Context = new DbAdapter())
            {

                if (status == "")
                {
                    grdStatusCount.DataSource = Context.GetStatusCount(RptType, ent, uy, uw);
                    grdStatusCount.DataBind();
                    grdFieldCount.DataSource = Context.GetFieldCount(RptType, ent, uw, uy, program, status);
                    grdFieldCount.DataBind();
                }
                if (uw == "")
                {
                    grdUWCount.DataSource = Context.GetUWCount(RptType, ent, uy);
                    grdUWCount.DataBind();
                    grdStatusCount.DataSource = Context.GetStatusCount(RptType, ent, uy, uw);
                    grdStatusCount.DataBind();
                    grdFieldCount.DataSource = Context.GetFieldCount(RptType, ent, uw, uy, program, status);
                    grdFieldCount.DataBind();
                }

                if (uy != "")
                {
                    grdFieldCount.DataSource = Context.GetFieldCount(RptType, ent, uw, uy, program, status);
                    grdFieldCount.DataBind();
                }
                if (field == "")
                {
                    grdFieldCount.DataSource = Context.GetFieldCount(RptType, ent, uw, uy, program, status);
                    grdFieldCount.DataBind();
                }


                UpdatePanel2.Update();
                UpdatePanel5.Update();
                if (txtSearch.Text != "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "Search_Gridview("+txtSearch.ClientID+", 'grdUYCnt');", true);
                }
            }
            //ShowingGroupingDataInGridView(grdResult.Rows,0,8);
        }


        void ShowingGroupingDataInGridView(GridViewRowCollection gridViewRows, int startIndex, int totalColumns)
        {
            if (totalColumns == 0) return;
            int i, count = 1;
            ArrayList lst = new ArrayList();
            lst.Add(gridViewRows[0]);
            var ctrl = gridViewRows[0].Cells[startIndex];
            for (i = 1; i < gridViewRows.Count; i++)
            {
                TableCell nextTbCell = gridViewRows[i].Cells[startIndex];
                if (ctrl.Text == nextTbCell.Text)
                {
                    count++;
                    nextTbCell.Visible = false;
                    lst.Add(gridViewRows[i]);
                }
                else
                {
                    if (count > 1)
                    {
                        ctrl.RowSpan = count;
                        ShowingGroupingDataInGridView(new GridViewRowCollection(lst), startIndex + 1, totalColumns - 1);
                    }
                    count = 1;
                    lst.Clear();
                    ctrl = gridViewRows[i].Cells[startIndex];
                    lst.Add(gridViewRows[i]);
                }
            }
            if (count > 1)
            {
                ctrl.RowSpan = count;
                ShowingGroupingDataInGridView(new GridViewRowCollection(lst), startIndex + 1, totalColumns - 1);
            }
            count = 1;
            lst.Clear();
        }

        protected void BtnExport_Click(object sender, EventArgs e)
        {
            if (grdResult.Rows.Count > 0)
            {
                DataTable dt = new DataTable("Datagrid");
                foreach (TableCell cell in grdResult.HeaderRow.Cells)
                {
                    dt.Columns.Add(cell.Text);
                }
                foreach (GridViewRow row in grdResult.Rows)
                {
                    dt.Rows.Add();
                    for (int i = 0; i < row.Cells.Count; i++)
                    {
                        dt.Rows[dt.Rows.Count - 1][i] = Regex.Replace(row.Cells[i].Text, @"<[^>]+>|&nbsp;", "").Trim();
                    }
                }
                dt.Columns.RemoveAt(10);                
                dt.Columns.RemoveAt(2);
                dt.Columns.RemoveAt(3);
                var appDataPath = Server.MapPath("~/images/");
                if (!Directory.Exists(appDataPath))
                {
                    Directory.CreateDirectory(appDataPath);
                }
                var filePath = Path.Combine(appDataPath, "REGIS_REVO_Comparison.xlsx");
                using (XLWorkbook wb = new XLWorkbook(filePath))
                {
                   // wb.Worksheets.Delete("GridView_Data");
                    var ws = wb.Worksheets.Add(dt);
                    int lastrow = ws.LastRowUsed().RowNumber();

                    //for(int i=0;i <lastrow;i++)
                    //{
                    //    ws.Row(i).Delete();
                    //}
                        
                   // var table = ws.Cell(1, 1).InsertTable(dt, "", true);

                    //wb.ws.Range("B1:C1").Delete(XLShiftDeletedCells.ShiftCellsUp);
                    // Get a range object
                    //var rngHeaders = ws.Range("B3:F3");

                    // Insert some rows/columns before the range
                    ws.Row(1).InsertRowsAbove(2);
                    ws.Row(1).Cell(1).Value = "Report Created Date";
                    ws.Row(1).Cell(2).Value = DateTime.Now.Date;
                    ws.Column(1).InsertColumnsBefore(2);
                    ws.Worksheet.Columns().AdjustToContents();
                    //var ptSheet = wb.Worksheets.Add("PivotTable");

                    //// Create the pivot table, using the data from the "PastrySalesData" table
                    //var pt = ptSheet.PivotTables.Add("PivotTable", ptSheet.Cell(1, 1), table.AsRange());

                    //// The rows in our pivot table will be the names of the pastries
                    //pt.RowLabels.Add("Rel Underwriter");
                    //pt.RowLabels.Add("Underwriter");
                    //pt.RowLabels.Add("Entity");
                    //pt.RowLabels.Add("MasterKey");
                    //pt.RowLabels.Add("Field Difference");
                    //pt.RowLabels.Add("REGIS");
                    //pt.RowLabels.Add("REVO");

                    // The columns will be the months
                    // pt.ColumnLabels.Add("Month");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=REGIS_REVO_Comparison_" + DateTime.Now + ".xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }


        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            //
        }

        protected void BtnRefresh_Click(object sender, EventArgs e)
        {
            try
            {    //getnewmethod();
                CallAsysnAsync();
                BindRefresh();

            }
            catch(Exception ex)
            {
                string jsMethodName = "HideProgressBar();";
                ScriptManager.RegisterClientScriptBlock(this, typeof(string), "uniqueKey", jsMethodName, true);

            }
        }

        private bool tryMethod()
        {
            throw new NotImplementedException();
        }

        protected void rdBtnRptType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindRefresh();
            grdResult.DataSource = null;
            grdResult.DataBind();
            UpdatePanel2.Update();
            UpdatePanel5.Update();
            UpdatePanel3.Update();
            if (txtSearch.Text != "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "Search_Gridview("+txtSearch.ClientID+", 'grdUYCnt');", true);
            }
        }


      
        public void BindResultGrid()
        {

            //this.UWTxt.Clear();
            string lstEntity = "";
            string lstUY = "";
            string lstYear = "";
            string lstQ = "";
            var IsEnt = false;


            string lstUW = "";
            foreach (GridViewRow item in grdUYCnt.Rows)
            {
                // check row is datarow
                if (item.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chk = (item.FindControl("chkUY") as CheckBox);
                    if (chk.Checked)
                    {
                        lstUY += ("'" + (item.Cells[1].Text).Trim() + "',");
                        IsEnt = true;
                        // break;
                    }
                }
            }
            lstUY = lstUY.TrimEnd(',');


            if (IsEnt)
            {
                BindFilters(rdBtnRptType.SelectedValue, lstEntity, lstUW, lstUY, lstYear, lstQ);
                foreach (GridViewRow item in grdEntityCnt.Rows)
                {
                    // check row is datarow
                    if (item.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chk = (item.FindControl("chkENT") as CheckBox);
                        if (chk.Checked)
                        {
                            lstEntity += ("'" + (item.Cells[1].Text).Trim() + "',");

                        }
                    }
                }
                lstEntity = lstEntity.TrimEnd(',');

                foreach (GridViewRow item in grdStatusCount.Rows)
                {
                    // check row is datarow
                    if (item.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chk = (item.FindControl("chkEntStatus") as CheckBox);
                        if (chk.Checked)
                        {

                            lstQ += ((item.Cells[1].Text).Trim()[1] + ",");
                            lstYear += ((item.Cells[1].Text).Split('-').Last() + ",");
                            //chk = true;
                            //break;

                        }
                    }
                }
                lstQ = lstQ.TrimEnd(',');
                lstYear = lstYear.TrimEnd(',');
                using (var contxt = new DbAdapter())
                {
                    grdFieldCount.DataSource = contxt.GetFieldCount(rdBtnRptType.SelectedValue, lstEntity, "", lstUY, lstYear, lstQ);
                    grdFieldCount.DataBind();
                    grdResult.DataSource = contxt.GetCompareResult(rdBtnRptType.SelectedValue, lstYear, lstEntity, lstUY, lstUW, "", lstQ);
                    grdResult.DataBind();
                }

                // BindDtResult();
            }
            else
            {
                BindRefresh();
                grdResult.DataSource = null;
                grdResult.DataBind();
            }





            if (grdResult.Rows.Count > 0)
                ShowingGroupingDataInGridView(grdResult.Rows,0,8);
            UpdatePanel3.Update();
            UpdatePanel5.Update();
        }
        protected void grdResult_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "OpenPopup")
            {
                string field_name = (e.CommandArgument).ToString();
                string platformId = field_name.Substring(field_name.LastIndexOf(',') + 1);
                string rpt_col = field_name.Substring(0, field_name.IndexOf(","));
                BindExcl(platformId, rpt_col, "Are you sure you want to Exclude the field ");
                btnInc.Visible = false;
                btnExc.Visible = true;

                ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "Popup", "ShowPopup();", true);
            }
            else if (e.CommandName == "HistoryPopup")
            {
                string field_name = (e.CommandArgument).ToString();
                string platformId = field_name.Substring(field_name.LastIndexOf(',') + 1);
                string rpt_col = field_name.Substring(0, field_name.IndexOf(","));
                selectedFieldName.Text = rpt_col;
                BindHistory(platformId, rpt_col,"GrdHistory");

                ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "HistoryPopup", "ShowHistoryPopup();", true);
            }
            else return;

        }

        public void BindHistory(string plat_id, string FieldNm,string grdType)
        {
            using (var contxt = new DbAdapter())
            {
                if (grdType == "GrdHistory")
                {
                    grdHistory.DataSource = contxt.GetFieldHistory(plat_id, FieldNm);
                    grdHistory.DataBind();
                }
                else if (grdType == "ExclGrdHistory")
                {
                    GrdExclHistory.DataSource = contxt.GetFieldHistory(plat_id, FieldNm);
                    GrdExclHistory.DataBind();
                }

            }
            
            UpdatePanel6.Update();


        }
        public void BindExcl(string plat_id, string FieldNm, string Msg)
        {
            using (var contxt = new DbAdapter())
            {
                ddlReason.DataTextField = "Reason";
                ddlReason.DataValueField = "Id";

                ddlReason.DataSource = contxt.GetRegRevoReasonsDT();
                ddlReason.DataBind();
                grdExcluded.DataSource = contxt.GetExcludeField(plat_id);
                grdExcluded.DataBind();

            }
            lblMsg.Text = Msg;
            lblField.Text = FieldNm;
            lblPID.Text = plat_id;
            UpdatePanel4.Update();


        }
        protected void btnExclude_Click(object sender, EventArgs e)
        {

        }

        protected void grdExcluded_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "cmdExcludeField") return;

            string field_name = (e.CommandArgument).ToString();
            string remainder = field_name.Substring(field_name.LastIndexOf(',') + 1);
            string last = field_name.Substring(0, field_name.IndexOf(","));
            ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "Popup", " ShowPopup2(); ", true);

            lblField.Text = last;
            lblPID.Text = remainder;
            lblMsg.Text = "Are you Sure you want to Include the field ";
            btnInc.Visible = true;
            btnExc.Visible = false;
            UpdatePanel4.Update();
        }

        protected void btnExc_Click(object sender, EventArgs e)
        {
            string reason = "";
            using (var contxt = new DbAdapter())
            {
                if (ddlReason.SelectedItem.Text != "Others")
                {
                    reason = ddlReason.SelectedItem.Text;
                }
                else
                {
                    if (txtReason.Text != "" && txtReason.Text != string.Empty)
                    {
                        reason = txtReason.Text;
                        if (contxt.ChkReasons(reason) > 0)
                        {
                            lblMsg.Text = "Reason Already Exist. Please select from Dropdown";
                            ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "Popup", "ShowPopup2();", true);
                            return;
                        }
                        else
                        {
                           // contxt.InsReasons(reason);
                        }
                    }
                   else
                    {
                        lblMsg.Text = "Please Enter Reason for Exclusion";
                    }
                }

                if (reason != "")
                {


                    contxt.PutExcludeField(lblPID.Text, lblField.Text, reason, Page.User.Identity.Name, "Insert");
                    BindResultGrid();
                    BindExcl(lblPID.Text, lblField.Text, "Successfully Excluded ");
                    btnExc.Visible = false;
                    btnInc.Visible = false;
                    lblMsg.ForeColor = Color.Green;
                    UpdatePanel4.Update();

                }
                else
                {
                    lblMsg.Text = "Please select Reason for Exclusion";
                }
            }
            ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "Popup", "ShowPopup2();", true);
        }

        protected void btnInc_Click(object sender, EventArgs e)
        {
            using (var contxt = new DbAdapter())
            {
                contxt.PutExcludeField(lblPID.Text, lblField.Text, txtReason.Text, Page.User.Identity.Name, "Delete");
                BindResultGrid();
                BindExcl(lblPID.Text, lblField.Text, "Successfully Included ");
                btnExc.Visible = false;
                btnInc.Visible = false;
                lblMsg.ForeColor = Color.Green;
                UpdatePanel4.Update();
                
            }
            
            ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "Popup", "ShowPopup2();", true);
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            BindRefresh();
            grdResult.DataSource = null;
            grdResult.DataBind();
            UpdatePanel3.Update();
            UpdatePanel5.Update();
        }

        protected void chkEnt_CheckedChanged(object sender, EventArgs e)
        {
            var hasChecked = false;
            foreach (GridViewRow item in grdEntityCnt.Rows)
            {
                // check row is datarow
                if (item.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chk = (item.FindControl("chkEnt") as CheckBox);
                    if (chk.Checked)
                    {
                        BindFilters(rdBtnRptType.SelectedValue, item.Cells[1].Text, "", "", "", "");
                        hasChecked = true;
                        break;
                    }

                }
            }
            if (!hasChecked)
            {
                DtUW();
                UpdatePanel2.Update();
                UpdatePanel5.Update();
                if (txtSearch.Text != "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "Search_Gridview("+txtSearch.ClientID+", 'grdUYCnt');", true);
                }
            }
        }

        protected void chkUY_CheckedChanged(object sender, EventArgs e)
        {
            var hasUYChecked = false;
            var hasEntChecked = false;
            var Ent = "";
            var UY = "";

            foreach (GridViewRow item in grdEntityCnt.Rows)
            {
                // check row is datarow
                if (item.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chk = (item.FindControl("chkEnt") as CheckBox);
                    if (chk.Checked)
                    {
                        Ent = item.Cells[1].Text;
                        hasEntChecked = true;
                        break;
                    }

                }
            }
            foreach (GridViewRow item in grdUYCnt.Rows)
            {
                // check row is datarow
                if (item.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chk = (item.FindControl("chkUY") as CheckBox);
                    if (chk.Checked)
                    {
                        // BindFilters(rdBtnRptType.SelectedValue,Ent,"",item.Cells[1].Text);
                        hasUYChecked = true;
                        break;
                    }

                }
            }
            if (!hasUYChecked)
            {

                UpdatePanel2.Update();
                UpdatePanel5.Update();
            }
            if (!hasEntChecked)
            {
                DtUW();
                UpdatePanel2.Update();
                UpdatePanel5.Update();
            }
            if (txtSearch.Text != "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "Search_Gridview("+txtSearch.ClientID+", 'grdUYCnt');", true);
            }
        }

        #region APIPULL
        private async static Task<IRestResponse> NewMethod(RestRequest request)
        {
            var client = new RestClient(System.Web.Configuration.WebConfigurationManager.AppSettings["ApiURL"]);
            client.Authenticator = new NtlmAuthenticator(System.Web.Configuration.WebConfigurationManager.AppSettings["ApiUserName"], System.Web.Configuration.WebConfigurationManager.AppSettings["ApiPassword"]);
            var cancellationTokenSource = new CancellationTokenSource();
            return await client.ExecuteAsync(request, cancellationTokenSource.Token);
        }
        private async static Task<IRestResponse> NewMethod2(RestRequest request)
        {
            var client = new RestClient(System.Web.Configuration.WebConfigurationManager.AppSettings["ApiURL"]);
            client.Authenticator = new NtlmAuthenticator(System.Web.Configuration.WebConfigurationManager.AppSettings["ApiUserName"], System.Web.Configuration.WebConfigurationManager.AppSettings["ApiPassword"]);
            return await client.ExecuteAsync(request);
        }


        public async void CallAsysnAsync()
        {
            var Revodate = new DateTime(DateTime.Now.Year - 1, 7, 1).ToString("MM-dd-yyyy");
            var datafile = DateTime.Now.ToString("yyyyMMddHHmmssfff");


            var request = new RestRequest();

            var tasks = new List<Task<IRestResponse>>();
            var tasks2 = new List<Task<IRestResponse>>();
            var tasks3 = new List<Task<IRestResponse>>();
            var tasks4 = new List<Task<IRestResponse>>();
            var tasks5 = new List<Task<IRestResponse>>();
            var tasks6 = new List<Task<IRestResponse>>();
            var tasks7 = new List<Task<IRestResponse>>();
            var tasks8 = new List<Task<IRestResponse>>();
            var taskExcp = new List<Task<IRestResponse>>();
            var taskExcp2 = new List<Task<IRestResponse>>();
            for (int i = 0; i < 40000; i = i + 500)
            {

                request = new RestRequest("/" + i, Method.GET);
                if (i >= 0 && i < 5000)
                    tasks.Add(NewMethod(request));
                if (i >= 5000 && i < 10000)
                    tasks2.Add(NewMethod(request));
                if (i >= 10000 && i < 15000)
                    tasks3.Add(NewMethod(request));
                if (i >= 15000 && i < 20000)
                    tasks4.Add(NewMethod(request));
                if (i >= 20000 && i < 25000)
                    tasks5.Add(NewMethod(request));
                if (i >= 25000 && i < 30000)
                    tasks6.Add(NewMethod(request));

            }
            //for (int i = 10000; i < 20000; i = i + 500)
            //{

            //    request = new RestRequest("/" + i, Method.GET);
            //    tasks2.Add(NewMethod(request));
            //}
            //for (int i = 20000; i < 30000; i = i + 500)
            //{

            //    request = new RestRequest("/" + i, Method.GET);
            //    tasks3.Add(NewMethod(request));
            //}


            var result = await Task.WhenAll(tasks);
            var result2 = await Task.WhenAll(tasks2);
            var result3 = await Task.WhenAll(tasks3);
            var result4 = await Task.WhenAll(tasks4);
            var result5 = await Task.WhenAll(tasks5);
            var result6 = await Task.WhenAll(tasks6);

            var queryResult1 = "[";






            using (var contxt = new DbAdapter())
            {
                dt = contxt.GetRegRevoDT();
            }

            foreach (var lst in tasks)
            {
                if (lst.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    if (lst.Result.Content.Length > 2)
                    {
                        InstDataToRegis(lst.Result.Content, datafile);
                    }

                    else
                    {
                        queryResult1 += "]";
                        break;


                    }
                }
                else
                {
                    request = new RestRequest(lst.Result.ResponseUri.ToString(), Method.GET);
                    taskExcp.Add(NewMethod2(request));
                }
            }
            if (tasks2.Count > 0)
            {
                // var result2 = await Task.WhenAll(tasks2);
                foreach (var lst in tasks2)
                {
                    if (lst.Result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        if (lst.Result.Content.Length > 2)
                        {
                            InstDataToRegis(lst.Result.Content, datafile);
                        }

                        else
                        {
                            queryResult1 += "]";
                            break;


                        }
                    }
                    else
                    {
                        request = new RestRequest(lst.Result.ResponseUri.ToString(), Method.GET);
                        taskExcp.Add(NewMethod(request));
                    }
                }
            }
            if (tasks3.Count > 0)
            {
                //  var result3 = await Task.WhenAll(tasks3);
                foreach (var lst in tasks3)
                {
                    if (lst.Result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        if (lst.Result.Content.Length > 2)
                        {
                            InstDataToRegis(lst.Result.Content, datafile);
                        }

                        else
                        {
                            queryResult1 += "]";
                            break;


                        }
                    }
                    else
                    {
                        request = new RestRequest(lst.Result.ResponseUri.ToString(), Method.GET);
                        taskExcp.Add(NewMethod(request));
                    }
                }
            }
            if (tasks4.Count > 0)
            {
                // var resultExp = await Task.WhenAll(taskExcp);

                foreach (var lst in tasks4)
                {
                    if (lst.Result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        if (lst.Result.Content.Length > 2)
                        {
                            InstDataToRegis(lst.Result.Content, datafile);
                        }

                        else
                        {
                            queryResult1 += "]";
                            break;


                        }
                    }
                    else
                    {
                        request = new RestRequest(lst.Result.ResponseUri.ToString(), Method.GET);
                        taskExcp.Add(NewMethod(request));
                    }
                }
            }
            if (tasks5.Count > 0)
            {
                //var resultExp2 = await Task.WhenAll(taskExcp2);
                foreach (var lst in tasks5)
                {
                    if (lst.Result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        if (lst.Result.Content.Length > 2)
                        {
                            InstDataToRegis(lst.Result.Content, datafile);
                        }

                        else
                        {
                            queryResult1 += "]";
                            break;


                        }
                    }
                    else
                    {
                        request = new RestRequest(lst.Result.ResponseUri.ToString(), Method.GET);
                        taskExcp.Add(NewMethod(request));
                    }

                }
            }
            if (tasks6.Count > 0)
            {
                //var resultExp2 = await Task.WhenAll(taskExcp2);
                foreach (var lst in tasks6)
                {
                    if (lst.Result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        if (lst.Result.Content.Length > 2)
                        {
                            InstDataToRegis(lst.Result.Content, datafile);
                        }

                        else
                        {
                            queryResult1 += "]";
                            break;


                        }
                    }
                    else
                    {
                        request = new RestRequest(lst.Result.ResponseUri.ToString(), Method.GET);
                        taskExcp.Add(NewMethod(request));
                    }

                }
            }
            if (taskExcp.Count > 0)
            {
                var resultExp2 = await Task.WhenAll(taskExcp);
                foreach (var lst in taskExcp)
                {
                    if (lst.Result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        if (lst.Result.Content.Length > 2)
                        {
                            InstDataToRegis(lst.Result.Content, datafile);
                        }

                        else
                        {
                            queryResult1 += "]";
                            break;


                        }
                    }
                    else
                    {
                        request = new RestRequest(lst.Result.ResponseUri.ToString(), Method.GET);
                        taskExcp.Add(NewMethod(request));
                    }

                }
            }
            using (var contxt = new DbAdapter())
            {
                contxt.BlkInsertRegREVDt(dt);
                //contxt.ExcGET_REGIS_Data_SP(datafile);
                int isExc = contxt.ExcCompSP(datafile);
            }
            dt.Rows.Clear();
            // connection();

            string jsMethodName = "HideProgressBar();";
           
            ScriptManager.RegisterClientScriptBlock(this, typeof(string), "uniqueKey", jsMethodName, true);
           
        }

        private IRestResponse NewMethod()
        {
            throw new NotImplementedException();
        }

        public async void getnewmethod()
        {
            var httpClientHandler = new HttpClientHandler()
            {

                Credentials = new NetworkCredential("Regis_service", "Arl441bm!") // real password instead of "pw"

            };
            var client = new System.Net.Http.HttpClient(httpClientHandler);
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            client.BaseAddress = new Uri("http://bmrevoapp1-uat:8080/api/RegisTransformer/GetAllContracts/1000");
            var list = new List<string>();
            var list2 = new List<string>();
            var list3 = new List<string>();
            var list4 = new List<string>();
            var list5 = new List<string>();

            var listResults = new List<string>();
            for (int i = 0; i < 40000; i = i + 500)
            {

                if (i >= 0 && i < 5000)
                    list.Add("http://bmrevoapp1-uat:8080/api/RegisTransformer/GetAllContracts/" + i);
                if (i >= 5000 && i < 10000)
                    list2.Add("http://bmrevoapp1-uat:8080/api/RegisTransformer/GetAllContracts/" + i);
                if (i >= 10000 && i < 15000)
                    list3.Add("http://bmrevoapp1-uat:8080/api/RegisTransformer/GetAllContracts/" + i);
                if (i >= 15000 && i < 20000)
                    list4.Add("http://bmrevoapp1-uat:8080/api/RegisTransformer/GetAllContracts/" + i);
                if (i >= 20000 && i < 25000)
                    list5.Add("http://bmrevoapp1-uat:8080/api/RegisTransformer/GetAllContracts/" + i);



            }
            var tasks = new List<Task>();
            var tasks2 = new List<Task>();
            var tasks3 = new List<Task>();
            var tasks4 = new List<Task>();
            var tasks5 = new List<Task>();
            foreach (var post in list)
            {
                async Task<string> func()
                {
                    var response = await client.GetAsync(post);
                    return await response.Content.ReadAsStringAsync();
                }
                tasks.Add(func());
            }

            foreach (var post in list2)
            {
                async Task<string> func()
                {
                    var response = await client.GetAsync(post);
                    return await response.Content.ReadAsStringAsync();
                }
                tasks2.Add(func());
            }
            foreach (var post in list3)
            {
                async Task<string> func()
                {
                    var response = await client.GetAsync(post);
                    return await response.Content.ReadAsStringAsync();
                }
                tasks3.Add(func());
            }
            foreach (var post in list4)
            {
                async Task<string> func()
                {
                    var response = await client.GetAsync(post);
                    return await response.Content.ReadAsStringAsync();
                }
                tasks4.Add(func());
            }
            foreach (var post in list5)
            {
                async Task<string> func()
                {
                    var response = await client.GetAsync(post);
                    return await response.Content.ReadAsStringAsync();
                }
                tasks5.Add(func());
            }
            await Task.WhenAll(tasks);
            await Task.WhenAll(tasks2);
            await Task.WhenAll(tasks3);
            await Task.WhenAll(tasks4);
            await Task.WhenAll(tasks5);

            var postResponses = new List<string>();

            foreach (var t in tasks)
            {
                var postResponse = t.Status.ToString(); //t.Result would be okay too.
                postResponses.Add(postResponse);
                Console.WriteLine(postResponse);
            }

        }

        public void InstDataToRegis(string stops1, string datafile)
        {
            var stops = JArray.Parse(stops1);
            for (int i = 0; i < stops.Count; i++)
            {
                myDeserializedClass = JsonConvert.DeserializeObject<List<Contract>>(stops[i]["contract"].ToString());


                foreach (var prop in myDeserializedClass)
                {
                    //changes made
                    dr = dt.NewRow();
                    dr["source_system"] = prop.uw_source;
                    dr["legal_ent_code"] = "";
                    dr["cont_master_key"] = prop.Cont_Master_Key;
                    dr["cont_uy"] = prop.Cont_UY;
                    dr["contract_id"] = prop.contract_id;
                    dr["Cont_subno"] = prop.Cont_subno;
                    dr["uw_platform_id"] = prop.source_system_id;
                    dr["cont_doc_status"] = "S";
                    dr["Cont_layer_code"] = prop.Cont_Layer_Code;
                    dr["Cont_layer_desc"] = prop.Cont_Layer_Desc;
                    dr["Cont_Type"] = prop.Cont_Type;
                    dr["Facility_code"] = prop.Facility_Code;
                    dr["counterparty"] = prop.Cont_Report_CP_Name;
                    dr["Reinsurer"] = prop.Cont_Reinsurer_Name;
                    dr["Broker"] = prop.Cont_Broker_Name;
                    dr["Segement"] = prop.Cont_Segment;
                    dr["Assumed_Ceded"] = prop.Cont_Assumed_Ceded_Flag;
                    dr["Renewal"] = prop.Cont_Renewal_Flag;
                    dr["TypeIns"] = prop.Cont_Type_Ins;
                    dr["Geography"] = prop.Cont_Geography;
                    dr["cont_lob"] = prop.Cont_UW_LOB;
                    dr["EffectiveDate"] = prop.Cont_Date_Effective;

                    dr["ExpiredDate"] = prop.Cont_Date_Expiration;
                    dr["Arrived"] = prop.Cont_Date_Arrived;
                    if (prop.cont_reins.Count > 0)
                    {
                        int smqty = 0;
                        for(int isqty=0;isqty<prop.cont_reins.Count;isqty++)
                        {
                            smqty += Convert.ToInt32(prop.cont_reins[isqty].cont_reins_qty);
                        }
                        dr["No_of_Reinstatement"] = smqty.ToString();
                    }
                    else
                    {
                        dr["No_of_Reinstatement"] = "0";
                    }
                    dr["OccurLimit"] = string.Format("{0:n0}", prop.Cont_100_Limit_Occurance);
                    dr["OurLimitAgg"] = string.Format("{0:n0}", prop.Cont_100_Limit_Aggregate);
                    dr["OurAggDeductible"] = string.Format("{0:n0}", prop.Cont_Our_Agg_Deductible ?? 0.00);
                    dr["AttachmentBasis"] = prop.Cont_Attach_Basis;
                    dr["LimitBais"] = prop.Cont_Limit_Basis;
                    dr["BoundShare"] = string.Format("{0:#,##0.00}", prop.Cont_Bound_Share);
                    dr["Est_SPI_100"] = string.Format("{0:n0}", prop.Cont_Est_SPI_100);
                    dr["Brokerage"] = string.Format("{0:#,##0.0000}", prop.Cont_Brokerage_Pct);
                    dr["Commission"] = string.Format("{0:#,##0.0000}", prop.Cont_Comm_Pct);
                    dr["Comm_Overide_pct"] = string.Format("{0:#,##0.0000}", prop.Cont_Comm_Override_Pct);
                    dr["Comm_variable"] = prop.Cont_Comm_Variable_Flag;
                    dr["Comm_variable_low"] = string.Format("{0:#,##0.0000}", prop.Cont_Comm_Variable_Low);
                    dr["Comm_variable_high"] = string.Format("{0:#,##0.0000}", prop.Cont_Comm_Variable_High);
                    dr["OtherComm"] = string.Format("{0:#,##0.0000}", prop.Cont_Comm_Other);
                    dr["GrossUp"] = string.Format("{0:#,##0.0000}", prop.Cont_Gross_Up_Flag);

                    dr["GrossUpPer"] = string.Format("{0:#,##0.0000}", prop.Cont_Gross_Up_Pct);
                    dr["FET_Taxes"] = string.Format("{0:#,##0.0000}", prop.Cont_FET_Taxes);
                    dr["ReinProfitExpence"] = string.Format("{0:#,##0.0000}", prop.Cont_PC_Reins_Profit_Exp_Pct);
                    dr["CurrencyPrimary"] = prop.Cont_Currency_Primary;
                    dr["PC_Deficit_Years"] = prop.Cont_PC_Deficit_CF_Years;
                    dr["PC_Defict_Amt"] = prop.Cont_PC_Deficit_CF_Amt;
                    dr["PC_Calc"] = prop.Cont_PC_Calc_Flag;
                    dr["PC_percent"] = string.Format("{0:#,##0.0000}", prop.Cont_PC_Pct);
                    dr["Sliding_Scale"] = prop.Cont_SS_Flag;
                    dr["PC_Calc_date"] = prop.Cont_PC_First_Calc_Date;
                    dr["SS_Max_Comm_pct"] = string.Format("{0:#,##0.0000}", prop.Cont_SS_Max_Commission_Pct);
                    dr["SS_Max_Loss_Ratio"] = string.Format("{0:#,##0.0000}", prop.Cont_SS_Max_Loss_Ratio);
                    dr["placement"] = string.Format("{0:#,##0.0000}", prop.Cont_Placement);
                    dr["LossTrigger"] = prop.cont_loss_trigger;
                    dr["Cont_Prem_Deposit_100"] = string.Format("{0:n0}", prop.cont_premium_deposit_100);
                    dr["FlatPremium100"] = string.Format("{0:n0}", prop.cont_premium_flat_100);
                    dr["MinPremium100"] = string.Format("{0:n0}", prop.cont_premium_min_100);

                    dr["Cedant"] = prop.Cont_Report_CP_id;
                    dr["Reference"] = prop.Cont_Broker_Ref;
                    dr["ContractType"] = prop.Cont_Type;
                    dr["AttachmentPoint100"] = string.Format("{0:n}", prop.Cont_100_Attachment_Point);
                    dr["ContractLimit100"] = string.Format("{0:n}", prop.Cont_100_Limit);
                    dr["AggregateLimit100"] = string.Format("{0:n}", prop.Cont_100_Limit_Aggregate);
                    dr["RiskLimit100"] = string.Format("{0:n}", prop.Cont_100_Limit_Risk);
                    dr["AggregateDeductible100"] = string.Format("{0:n0}", prop.Cont_100_Agg_Deductible);
                    dr["EstimatedSPI100"] = string.Format("{0:n0}", prop.Cont_Est_SPI_100);
                    dr["SPI100"] = string.Format("{0:n0}", prop.Cont_SPI_100);
                    dr["Accrual"] = prop.Cont_Accrual_Calc_Flag;
                    dr["LAETerms"] = prop.Cont_LAE_Terms;
                    dr["SS_Prov_Comm_Pct"] = string.Format("{0:#,##0.0000}", prop.Cont_SS_Prov_Comm_Pct);
                    dr["SS_Min_Comm_Pct"] = string.Format("{0:#,##0.0000}", prop.Cont_SS_Min_Commission_Pct);
                    dr["MultiYearExpire"] = prop.multi_year_expire ?? DBNull.Value;
                    dr["MultiYearIncept"] = prop.multi_year_incept ?? DBNull.Value;
                    dr["CCFYears"] = prop.Cont_PC_Credit_CF_Years;
                    dr["SLidingScaleFlag"] = prop.Cont_SS_Calc_Flag;

                    dr["AdjustableRate"] = string.Format("{0:#,##0.0000}", prop.Cont_Premium_Adj_Rate);
                    dr["AdjustmentBase"] = prop.Cont_Premium_Adj_XS;
                    dr["Sub_No"] = prop.stg_id ?? DBNull.Value;
                    if (prop.Cont_Prem_Method == "DEP")
                    {
                        dr["PortInEarnings"] = "";
                        dr["PortOutEarnings"] = "";
                        dr["PremiumEarnings"] = "";
                        dr["Earnings"] = prop.Cont_UPR_Code;
                    }
                    else
                    {
                        dr["PortInEarnings"] = prop.Cont_UPR_Code;
                        dr["PortOutEarnings"] = prop.Cont_UPR_Code;
                        dr["PremiumEarnings"] = prop.Cont_UPR_Code;
                        dr["Earnings"] = "";
                    }

                    dr["PremiumMethod"] = prop.Cont_Prem_Method;

                    if (prop.Cont_Type == "QS" || prop.Cont_Type == "RPQ")
                        dr["LossMethod"] = "BDX";
                    else
                        dr["LossMethod"] = "IND";
                    //dr["LossMethod"] = prop.Cont_Prem_Method;
                    dr["CommAcct"] = prop.Cont_Common_Acct_Flag;
                    dr["AP"] = prop.Cont_AP_Flag;
                    dr["ExperianceRate"] = prop.Cont_Stop_Loss_Flag;
                    dr["NCB"] = prop.Cont_NCB_Flag;
                    dr["NCB_pct"] = prop.Cont_NCB_Pct;
                    dr["StopLoss"] = prop.Cont_Stop_Loss_Flag;
                    dr["PercentLimit"] = string.Format("{0:#,##0.0000}", prop.Cont_Stop_Loss_Limit_Pct);
                    dr["LossCorridor"] = string.Format("{0:#,##0.0000}", prop.Cont_Stop_Loss_Attach_Pct);
                    dr["PC_LC_Flag"] = prop.Cont_PC_LC_Flag;
                    dr["LowerThreshold"] = prop.Cont_PC_LC_Begin;

                    dr["UpperThreshold"] = prop.Cont_PC_LC_End;
                    dr["CedantParticipation"] = string.Format("{0:#,##0.0000}", prop.Cont_PC_Cedeco_Retention_Pct);
                    dr["NthEvent"] = prop.cont_nth_event;
                    dr["SettlementDays"] = prop.Cont_Install_Settlement_Days;
                    dr["ReportingDays"] = prop.Cont_Bdx_Report_Due_Days;
                    dr["Install"] = prop.Cont_Install_Equal_Flag;
                    if (prop.Cont_Prem_Method != "")
                        {
                        dr["AdjustmentDate"] = prop.Cont_Install_Adjust_Date ?? "";
                    }
                    else
                    {
                        dr["AdjustmentDate"] = "";
                    }
                   
                    dr["AsCollected"] = prop.Cont_Install_As_Collected_Flag;
                    dr["Est_ult_Arch_Premium"] = prop.Cont_Est_Ult_Arch_Prem;
                    dr["PortsFlag"] = prop.Cont_Port_Flag;
                    dr["QS_Of_XS"] = prop.Cont_QS_Of_XS;
                    dr["Frequency"] = prop.Cont_Install_Freq;
                    dr["ERC"] = prop.Cont_ERC_Flg;
                    dr["ERC_pct"] = string.Format("{0:#,##0.0000}", prop.Cont_ERC_Pct ?? 0.0000);
                    dr["BDX_Frequency"] = prop.Cont_BDX_Freq;
                    dr["ProgramId"] = stops[i]["pgm_id"].ToString();
                    dr["pgm_AC"] = stops[i]["pgm_assumed_ceded_flag"].ToString();
                    dr["Pgm_cedant"] = stops[i]["pgm_cp_ceding"].ToString();
                    dr["Pgm_description"] = stops[i]["pgm_desc"].ToString();
                    dr["Pgm_UW"] = stops[i]["pgm_uw"].ToString();
                    dr["Pgm_UW_rel"] = stops[i]["pgm_uw_rel"].ToString();
                    dr["Pgm_office"] = stops[i]["pgm_office"].ToString();

                    dr["datafile"] = datafile;

                    dt.Rows.Add(dr);


                }
            }
        }
        #endregion

        protected void chkEntStatus_CheckedChanged1(object sender, EventArgs e)
        {
            //var uychk = false;
            //var uwchk = false;
            //var statuschk = false;
            var Fieldchk = false;
            //string lstEntity = "";
            //string lstUY = "";
            //string lstProgram = "";
            //string lstStatus = "";
            string lstField = "";
           
           

           // this.UWTxt.Clear();
            List<FilterValues> fltval = CheckFieldFilters();
            foreach (GridViewRow item in grdFieldCount.Rows)
            {
                // check row is datarow
                if (item.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chk = (item.FindControl("chkEntStatus") as CheckBox);
                    if (chk.Checked)
                    {
                        // lstField =(item.Cells[1].Text);
                        // lstField += "''" + (item.Cells[1].Text) + "'',";
                        lstField += ("'" + (item.Cells[1].Text).Trim() + "',");
                        Fieldchk = true;
                        // break;
                    }
                }
            }
            // lstField = "''Accrual''";
            lstField = lstField.TrimEnd(',');
            using (var contxt = new DbAdapter())
            {
                if (fltval[0].lstUY.ToString() != "" || fltval[0].lstUW.ToString() != "" || fltval[0].lstStatus.ToString() != "" || fltval[0].lstField.ToString() != "")
                {
                    grdResult.DataSource = contxt.GetCompareResult(rdBtnRptType.SelectedValue, fltval[0].lstYear, fltval[0].lstENT, fltval[0].lstUY, fltval[0].lstUW, lstField, fltval[0].lstStatus);
                    GetExcludedData();

                }
                else
                {
                    BindFilters(rdBtnRptType.SelectedValue, fltval[0].lstENT, fltval[0].lstUW, fltval[0].lstUY, "", fltval[0].lstStatus);
                }
                grdResult.DataBind();

            }
            if (grdResult.Rows.Count > 0)
                ShowingGroupingDataInGridView(grdResult.Rows,0,8);
            else
                BindDtResult();


            UpdatePanel3.Update();
        }

        protected void chkEnt_CheckedChanged1(object sender, EventArgs e)
        {
           //this.UWTxt.Clear();
            string lstEntity = "";
            string lstUY = "";
            string lstYear = "";
            string lstQ = "";
            var IsEnt = false;


            string lstUW = "";
            foreach (GridViewRow item in grdUYCnt.Rows)
            {
                // check row is datarow
                if (item.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chk = (item.FindControl("chkUY") as CheckBox);
                    if (chk.Checked)
                    {
                        lstUY += ("'" + (item.Cells[1].Text).Trim() + "',");
                        IsEnt = true;
                       // break;
                    }
                }
            }
            lstUY = lstUY.TrimEnd(',');


            if (IsEnt)
            {
                BindFilters(rdBtnRptType.SelectedValue, lstEntity, lstUW, lstUY,lstYear, lstQ);
                foreach (GridViewRow item in grdEntityCnt.Rows)
                {
                    // check row is datarow
                    if (item.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chk = (item.FindControl("chkENT") as CheckBox);
                        if (chk.Checked)
                        {
                            lstEntity += ("'" + (item.Cells[1].Text).Trim() + "',");
                           
                        }
                    }
                }
                lstEntity = lstEntity.TrimEnd(',');

                foreach (GridViewRow item in grdStatusCount.Rows)
                {
                    // check row is datarow
                    if (item.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chk = (item.FindControl("chkEntStatus") as CheckBox);
                        if (chk.Checked)
                        {

                            lstQ += ((item.Cells[1].Text).Trim()[1] + ",");
                            lstYear += ((item.Cells[1].Text).Split('-').Last() + ",");
                            //chk = true;
                            //break;

                        }
                    }
                }
                lstQ = lstQ.TrimEnd(',');
                lstYear = lstYear.TrimEnd(',');
                using (var contxt = new DbAdapter())
                {
                    grdFieldCount.DataSource = contxt.GetFieldCount(rdBtnRptType.SelectedValue, lstEntity, "", lstUY, lstYear, lstQ);
                    grdFieldCount.DataBind();
                    grdResult.DataSource = contxt.GetCompareResult(rdBtnRptType.SelectedValue, lstYear, lstEntity, lstUY, lstUW,"", lstQ);
                    grdResult.DataBind();
                    GetExcludedData();
                }
                   
                   // BindDtResult();
            }
            else
            {
                BindRefresh();
                grdResult.DataSource = null;
                grdResult.DataBind();
               
            }





            if (grdResult.Rows.Count > 0)
                ShowingGroupingDataInGridView(grdResult.Rows,0,8);
            UpdatePanel3.Update();
            UpdatePanel5.Update();
           
        }

        
        void GetExcludedData()
        {
            List<FilterValues> fltval = CheckAllFilters();
            using (var contxt = new DbAdapter())
            {
                using (var dt2 = contxt.GetResultWithExcludedDataUI(rdBtnRptType.SelectedValue, fltval[0].lstYear, fltval[0].lstENT, fltval[0].lstUY, fltval[0].lstUW, fltval[0].lstField, fltval[0].lstStatus))
                {
                    if (dt2.Rows.Count > 0)
                    {
                        btnShowExcludedFields.Visible = true;
                        grdExcluded1.DataSource = dt2;
                        grdExcluded1.DataBind();
                        UpdatePanel6.Update();
                    }
                    else
                    {
                        btnShowExcludedFields.Visible = false;
                    }
                }
            }
        }
        protected void chkEntStatus_CheckedChangedUW(object sender, EventArgs e)
        {
           // this.UWTxt.Clear();
            List<FilterValues> fltval = CheckFilters();
            if (fltval[0].lstUY.ToString() != "")
            {
                using (var contxt = new DbAdapter())
                {
                    if (fltval[0].lstUY.ToString() != "" || fltval[0].lstUW.ToString() != "" || fltval[0].lstStatus.ToString() != "" || fltval[0].lstField.ToString() != "")
                    {
                        grdResult.DataSource = contxt.GetCompareResult(rdBtnRptType.SelectedValue, "", fltval[0].lstENT, fltval[0].lstUY, fltval[0].lstUW, fltval[0].lstField, fltval[0].lstStatus);
                        grdResult.DataBind();
                    }
                    else
                    {
                        BindFilters(rdBtnRptType.SelectedValue, fltval[0].lstENT, fltval[0].lstUW, fltval[0].lstUY, "", fltval[0].lstStatus);
                    }
                    grdResult.DataBind();

                    grdFieldCount.DataSource = contxt.GetFieldCount(rdBtnRptType.SelectedValue, fltval[0].lstENT, fltval[0].lstUW, fltval[0].lstUY, fltval[0].lstYear, fltval[0].lstStatus);
                    grdFieldCount.DataBind();
                    grdEntityCnt.DataSource = contxt.GetEntityCount(rdBtnRptType.SelectedValue, fltval[0].lstUY, fltval[0].lstUW);
                    grdEntityCnt.DataBind();
                    grdStatusCount.DataSource = contxt.GetStatusCount(rdBtnRptType.SelectedValue, fltval[0].lstENT, fltval[0].lstUY, fltval[0].lstUW);
                    grdStatusCount.DataBind();
                }
            }
            else
            {
                BindRefresh();
                grdResult.DataSource = null;
                grdResult.DataBind();
            }
            DefaultBind();
        }

        protected void chkEntStatus_CheckedChangedUY(object sender, EventArgs e)
        {
            //this.UWTxt.Clear();
            List<FilterValues> fltval = CheckFilters();
            if (fltval[0].lstUY.ToString() != "")
            {
                using (var contxt = new DbAdapter())
                {
                    


                    grdUWCount.DataSource = contxt.GetUWCount(rdBtnRptType.SelectedValue, fltval[0].lstENT, fltval[0].lstUY);
                    grdUWCount.DataBind();
                   
                    grdStatusCount.DataSource = contxt.GetStatusCount(rdBtnRptType.SelectedValue, fltval[0].lstENT, fltval[0].lstUY, fltval[0].lstUW);
                    grdStatusCount.DataBind();


                    foreach (GridViewRow item in grdStatusCount.Rows)
                    {
                        // check row is datarow
                        if (item.RowType == DataControlRowType.DataRow)
                        {
                            CheckBox chkSelect = (item.FindControl("chkEntStatus") as CheckBox);
                            if (chkSelect != null)
                            {
                                string uwtxt = item.Cells[1].Text.Trim();


                                if (chkSelect.Checked)
                                {
                                    if (!this.StatusTxt.Contains(uwtxt))
                                        {
                                        this.StatusTxt.Add(uwtxt);
                                    }
                                    fltval[0].lstStatus += ((item.Cells[1].Text).Trim()[1] + ",");
                                    fltval[0].lstYear += ((item.Cells[1].Text).Split('-').Last() + ",");
                                    // uwchk = true;
                                    // break;
                                }
                                else if (!chkSelect.Checked && this.StatusTxt.Contains(uwtxt))
                                {
                                    this.StatusTxt.Remove(uwtxt);
                                }

                            }
                        }
                    }
                    fltval[0].lstStatus = fltval[0].lstStatus.TrimEnd(',');
                    fltval[0].lstYear = fltval[0].lstYear.TrimEnd(',');

                    if (fltval[0].lstUY.ToString() != "" || fltval[0].lstUW.ToString() != "" || fltval[0].lstStatus.ToString() != "" || fltval[0].lstField.ToString() != "")
                    {
                        grdFieldCount.DataSource = contxt.GetFieldCount(rdBtnRptType.SelectedValue, fltval[0].lstENT, fltval[0].lstUW, fltval[0].lstUY, fltval[0].lstYear, fltval[0].lstStatus);
                        grdFieldCount.DataBind();
                        grdResult.DataSource = contxt.GetCompareResult(rdBtnRptType.SelectedValue, fltval[0].lstYear, fltval[0].lstENT, fltval[0].lstUY, fltval[0].lstUW, fltval[0].lstField, fltval[0].lstStatus);
                        grdResult.DataBind();
                        GetExcludedData();

                    }
                    else
                    {
                        BindFilters(rdBtnRptType.SelectedValue, fltval[0].lstENT, fltval[0].lstUW, fltval[0].lstUY, "", fltval[0].lstStatus);
                    }
                    grdResult.DataBind();
                }
            }
            else
            {
                BindRefresh();
                grdResult.DataSource = null;
                grdResult.DataBind();
            }
            DefaultBind();
        }

        protected void chkEntStatus_CheckedChangedStatus(object sender, EventArgs e)
        {
            this.StatusTxt.Clear();
            List<FilterValues> fltval = CheckFilters();
            if (fltval[0].lstUY.ToString() != "")
            {
                using (var contxt = new DbAdapter())
                {

                    foreach (GridViewRow item in grdStatusCount.Rows)
                    {
                        // check row is datarow
                        if (item.RowType == DataControlRowType.DataRow)
                        {
                            CheckBox chkSelect = (item.FindControl("chkEntStatus") as CheckBox);
                            if (chkSelect != null)
                            {
                                string uwtxt = item.Cells[1].Text.Trim();


                                if (chkSelect.Checked)
                                {
                                    if (!this.StatusTxt.Contains(uwtxt))
                                    {
                                        this.StatusTxt.Add(uwtxt);
                                    }
                                    fltval[0].lstStatus += ((item.Cells[1].Text).Trim()[1] + ",");
                                    fltval[0].lstYear += ((item.Cells[1].Text).Split('-').Last() + ",");
                                    // uwchk = true;
                                    // break;
                                }
                                else if (!chkSelect.Checked && this.StatusTxt.Contains(uwtxt))
                                {
                                    this.StatusTxt.Remove(uwtxt);
                                }

                            }
                        }
                    }
                    fltval[0].lstStatus = fltval[0].lstStatus.TrimEnd(',');
                    fltval[0].lstYear = fltval[0].lstYear.TrimEnd(',');
                    grdUWCount.DataSource = contxt.GetUWCount(rdBtnRptType.SelectedValue, fltval[0].lstENT, fltval[0].lstUY);
                    grdUWCount.DataBind();
                   
                    if (fltval[0].lstUY.ToString() != "" || fltval[0].lstUW.ToString() != "" || fltval[0].lstStatus.ToString() != "" || fltval[0].lstField.ToString() != "")
                    {
                        grdResult.DataSource = contxt.GetCompareResult(rdBtnRptType.SelectedValue, fltval[0].lstYear, fltval[0].lstENT, fltval[0].lstUY, fltval[0].lstUW, fltval[0].lstField, fltval[0].lstStatus);
                        grdResult.DataBind();
                        grdFieldCount.DataSource = contxt.GetFieldCount(rdBtnRptType.SelectedValue, fltval[0].lstENT, fltval[0].lstUW, fltval[0].lstUY, fltval[0].lstYear, fltval[0].lstStatus);
                        grdFieldCount.DataBind();
                        GetExcludedData();
                    }
                    else
                    {
                        BindFilters(rdBtnRptType.SelectedValue, fltval[0].lstENT, fltval[0].lstUW, fltval[0].lstUY, "", fltval[0].lstStatus);
                    }
                    // grdStatusCount.DataSource = contxt.GetStatusCount(rdBtnRptType.SelectedValue, fltval[0].lstENT, fltval[0].lstUY, fltval[0].lstUW);
                    // grdStatusCount.DataBind();

                }
            }
            else
            {
                BindRefresh();
                grdResult.DataSource = null;
                grdResult.DataBind();
            }
            DefaultBind();
        }

        public void DefaultBind()
        {

            if (grdResult.Rows.Count > 0)
                ShowingGroupingDataInGridView(grdResult.Rows,0,8);
            else
                BindDtResult();
            UpdatePanel3.Update();
            UpdatePanel5.Update();
        }
        public List<FilterValues> CheckFilters()
        {
            List<FilterValues> fltVal = new List<FilterValues>();
            var uychk = false;
            var uwchk = false;
            var Entchk = false;
            var Fieldchk = false;
            string lstEntity = "";
            string lstUY = "";
            string lstYear = "";
            string lstStatus = "";
            string lstField = "";
            string lstUW = "";
            foreach (GridViewRow item in grdUYCnt.Rows)
            {
                // check row is datarow
                if (item.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chk = (item.FindControl("chkUY") as CheckBox);
                    if (chk.Checked)
                    {
                        lstUY += ("'" + (item.Cells[1].Text).Trim() + "',");
                        //break;
                    }
                }
            }
            lstUY = lstUY.TrimEnd(',');
            //this.UWTxt.Clear();
            foreach (GridViewRow item in grdEntityCnt.Rows)
            {
                // check row is datarow
                if (item.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkSelect = (item.FindControl("chkENT") as CheckBox);
                    if (chkSelect != null)
                    {
                        string uwtxt = item.Cells[1].Text.Trim();


                        if (chkSelect.Checked)
                        {
                            if (!this.UWTxt.Contains(uwtxt))
                            {
                                this.UWTxt.Add(uwtxt);
                            }
                            lstEntity += ("'" + (item.Cells[1].Text).Trim() + "',");
                            //uwchk = true;
                           // break;
                        }
                        else if (!chkSelect.Checked && this.UWTxt.Contains(uwtxt))
                        {
                            this.UWTxt.Remove(uwtxt);
                        }

                    }
                }
            }
            lstEntity = lstEntity.TrimEnd(',');
            //lstUW = lstUW.TrimEnd(',');
            //foreach (GridViewRow item in grdEntityCnt.Rows)
            //{
            //    // check row is datarow
            //    if (item.RowType == DataControlRowType.DataRow)
            //    {
            //        CheckBox chk = (item.FindControl("chkENT") as CheckBox);
            //        if (chk.Checked)
            //        {
            //            lstEntity += ("'" + (item.Cells[1].Text).Trim() + "',");
            //            Entchk = true;
            //            //break;

            //        }
            //    }
            //}


            //foreach (GridViewRow item in grdStatusCount.Rows)
            //{
            //    // check row is datarow
            //    if (item.RowType == DataControlRowType.DataRow)
            //    {
            //        CheckBox chk = (item.FindControl("chkEntStatus") as CheckBox);
            //        if (chk.Checked)
            //        {

            //            lstStatus += ( (item.Cells[1].Text).Trim()[1] + ",");
            //            lstYear += ( (item.Cells[1].Text).Split('-').Last() + ",");
            //            //chk = true;
            //            //break;

            //        }
            //    }
            //}
            //lstStatus = lstStatus.TrimEnd(',');
            //lstYear = lstYear.TrimEnd(',');

            //if (!uwchk)
            //    UWTxt.Clear();



            //if (UWTxt.Count > 0)
            //    lstUW = UWTxt[0];

            fltVal.Add(new FilterValues
            {
                lstUW = lstUW,
                lstENT = lstEntity,
                lstField = lstField,
                lstStatus = lstStatus,
                lstYear=lstYear,
                lstUY = lstUY
            });
            return fltVal;
        }

        public List<FilterValues> CheckFieldFilters()
        {
            List<FilterValues> fltVal = new List<FilterValues>();
            var uychk = false;
            var uwchk = false;
            var Entchk = false;
            var Fieldchk = false;
            string lstEntity = "";
            string lstUY = "";
            string lstYear = "";
            string lstStatus = "";
            string lstField = "";
            string lstUW = "";
            foreach (GridViewRow item in grdUYCnt.Rows)
            {
                // check row is datarow
                if (item.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chk = (item.FindControl("chkUY") as CheckBox);
                    if (chk.Checked)
                    {
                        lstUY += ("'" + (item.Cells[1].Text).Trim() + "',");
                        //break;
                    }
                }
            }
            lstUY = lstUY.TrimEnd(',');
           // this.UWTxt.Clear();
            foreach (GridViewRow item in grdUWCount.Rows)
            {
                // check row is datarow
                if (item.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkSelect = (item.FindControl("chkEntStatus") as CheckBox);
                    if (chkSelect != null)
                    {
                        string uwtxt = item.Cells[1].Text.Trim();


                        if (chkSelect.Checked && !this.UWTxt.Contains(uwtxt))
                        {

                            this.UWTxt.Add(uwtxt);
                            lstUW += ("'" + item.Cells[1].Text.Trim() + "',");
                            uwchk = true;
                            // break;
                        }
                        else if (!chkSelect.Checked && this.UWTxt.Contains(uwtxt))
                        {
                            this.UWTxt.Remove(uwtxt);
                        }

                    }
                }
            }
            lstUW = lstUW.TrimEnd(',');
            foreach (GridViewRow item in grdEntityCnt.Rows)
            {
                // check row is datarow
                if (item.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chk = (item.FindControl("chkENT") as CheckBox);
                    if (chk.Checked)
                    {
                        lstEntity += ("'" + (item.Cells[1].Text).Trim() + "',");
                        Entchk = true;
                        //break;

                    }
                }
            }
            lstEntity = lstEntity.TrimEnd(',');

            foreach (GridViewRow item in grdStatusCount.Rows)
            {
                // check row is datarow
                if (item.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chk = (item.FindControl("chkEntStatus") as CheckBox);
                    if (chk.Checked)
                    {

                        lstStatus += ((item.Cells[1].Text).Trim()[1] + ",");
                        lstYear += ((item.Cells[1].Text).Split('-').Last() + ",");
                        //chk = true;
                        //break;

                    }
                }
            }
            lstStatus = lstStatus.TrimEnd(',');
            lstYear = lstYear.TrimEnd(',');

            if (!uwchk)
                UWTxt.Clear();



            //if (UWTxt.Count > 0)
            //    lstUW = UWTxt[0];

            fltVal.Add(new FilterValues
            {
                lstUW = lstUW,
                lstENT = lstEntity,
                lstField = lstField,
                lstStatus = lstStatus,
                lstYear = lstYear,
                lstUY = lstUY
            });
            return fltVal;
        }

        protected void grdStatusCount_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow gvr = e.Row;

            if (gvr.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkSelect = gvr.FindControl("chkEntStatus") as CheckBox;

                string lbltxt1 = e.Row.Cells[1].Text;
                if (chkSelect != null)
                {

                    if (this.StatusTxt.Contains(lbltxt1))
                        chkSelect.Checked = true;
                    else
                        chkSelect.Checked = false;
                }
            }
        }
        private List<string> StatusTxt
        {
            get
            {
                if (this.ViewState["StatusTxt"] == null)
                {
                    this.ViewState["StatusTxt"] = new List<string>();
                }

                return this.ViewState["StatusTxt"] as List<string>;
            }
        }
        private List<string> UWTxt
        {
            get
            {
                if (this.ViewState["UWTxt"] == null)
                {
                    this.ViewState["UWTxt"] = new List<string>();
                }

                return this.ViewState["UWTxt"] as List<string>;
            }
        }

        protected void grdUWCount_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow gvr = e.Row;

            if (gvr.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkSelect = gvr.FindControl("chkEntStatus") as CheckBox;

                string lbltxt1 = e.Row.Cells[1].Text;
                if (chkSelect != null)
                {

                    if (this.UWTxt.Contains(lbltxt1))
                        chkSelect.Checked = true;
                    else
                        chkSelect.Checked = false;
                }
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            //dt = new DataTable();
            //DataTable dt2 = new DataTable();
            //DataRow dr2;
            //using (var contxt = new DbAdapter())
            //{
            //    dt2 = contxt.GetTBDT1();
            //}
            //using (var contxt = new DbAdapter())
            //{
            //    dt = contxt.GetTB_DT(Convert.ToInt32(txtPeriod.Text),txtLe.Text);
            //}

            //foreach(DataRow dr in dt.Rows)
            //{
            //    dr2 = dt2.NewRow();
            //    dr2["Office Code"] =Convert.ToInt32(dr["Office Code"]);
            //    dr2["LegalEnt"] = txtLe.Text;
            //    dr2["Period_year_month"] = txtPeriod.Text;
            //    dr2["gl_bal_flg"] = dr["gl_bal_flg"];
            //    dr2["GL Acct"] = dr["GL Acct"];
            //    dr2["GL Acct Desc"] = dr["GL Acct Desc"];
            //    dr2["PriorAmt"] = dr["PriorAmt"];
            //    dr2["CurrentAmt"] = dr["CurrentAmt"];
            //    dt2.Rows.Add(dr2);
            //}

            //using (var contxt = new DbAdapter())
            //{
            //    contxt.BlkInsertTB(dt2);

            //}
            //string jsMethodName = "HideProgressBar();";
            ////string script = "window.onload = function() { HideProgressBar(); };";
            //ScriptManager.RegisterClientScriptBlock(this, typeof(string), "uniqueKey", jsMethodName, true);
        }

        protected void btnEntUw_Click(object sender, EventArgs e)
        {
            dt = new DataTable();


            using (var contxt = new DbAdapter())
            {
                dt = contxt.GetExcludedData();
            }
            if (dt.Rows.Count > 0)
            {

                dt.Columns.RemoveAt(0);
                dt.Columns["uw_platform_id"].ColumnName = "Master Key";
                dt.Columns["rpt_col"].ColumnName = "Description";
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt, "Excluded_Data");
                    var ws = wb.Worksheet(1);

                    // Get a range object
                    var rngHeaders = ws.Range("B3:F3");

                    // Insert some rows/columns before the range
                    ws.Row(1).InsertRowsAbove(2);
                    ws.Row(1).Cell(1).Value = "Report Created Date";
                    ws.Row(1).Cell(2).Value = DateTime.Now.Date;
                    ws.Column(1).InsertColumnsBefore(2);
                    ws.Worksheet.Columns().AdjustToContents();
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=REGIS_REVO_Excluded_Data_Report_" + DateTime.Now + ".xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
        }

        protected void btnEntUWRpt_Click(object sender, EventArgs e)
        {
            string lstEntity = "";
            foreach (GridViewRow item in grdEntityCnt.Rows)
            {
                // check row is datarow
                if (item.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chk = (item.FindControl("chkEnt") as CheckBox);
                    if (chk.Checked)
                    {
                        lstEntity = (item.Cells[1].Text);
                        break;
                    }
                }
            }
            dt = new DataTable();
            using (var contxt = new DbAdapter())
            {
                List<CompareResult> listItems = contxt.GetCompareResult(rdBtnRptType.SelectedValue, "", lstEntity, "", "", "", "");
                dt = contxt.ToDataTable(listItems);
            }
            if (dt.Rows.Count > 0)
            {



                // dt.Columns.RemoveAt(9);
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt, "Entity UW Report");
                    var ws = wb.Worksheet(1);

                    // Get a range object
                    var rngHeaders = ws.Range("B3:F3");

                    // Insert some rows/columns before the range
                    ws.Row(1).InsertRowsAbove(2);
                    ws.Row(1).Cell(1).Value = "Report Created Date";
                    ws.Row(1).Cell(2).Value = DateTime.Now.Date;
                    ws.Column(1).InsertColumnsBefore(2);
                    ws.Worksheet.Columns().AdjustToContents();
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=REGIS_REVO_Comparison_" + DateTime.Now + ".xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
        }
        public void ExportToPdf(DataTable dt)
        {
            try
            {
                Font fontH1   = FontFactory.GetFont("Calibri", 10, Font.NORMAL, BaseColor.BLACK);
                Document document = new Document(PageSize.A4, 88f, 88f, 10f, 10f);
                Font NormalFont = FontFactory.GetFont("Calibri", 12, Font.NORMAL, BaseColor.BLACK);
                using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
                {
                    PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                    Phrase phrase = null;
                    PdfPCell cell = null;
                    PdfPTable table = null;
                    Color color = new Color();
                    Font font2 = new Font();
font2.SetColor(100,0,0);
                    document.Open();

                    //Header Table
                    table = new PdfPTable(2);
                    table.TotalWidth = 500f;
                    table.LockedWidth = true;
                    table.SetWidths(new float[] { 0.3f, 0.7f });

                    //Company Logo
                    cell = ImageCell("~/images/arch_logo.jpg", 100f, PdfPCell.ALIGN_CENTER);
                    table.AddCell(cell);

                    //Company Name and Address
                    phrase = new Phrase();
                    phrase.Add(new Chunk("Underwriting Comparison\n\n", FontFactory.GetFont("Calibri", 16, Font.BOLD, BaseColor.BLACK)));
                    phrase.Add(new Chunk("Production REGIS and REVO Data Comparison\n", FontFactory.GetFont("Calibri", 11, Font.NORMAL, BaseColor.BLACK)));
                    phrase.Add(new Chunk("Reporting Date\n", FontFactory.GetFont("Arial", 11, Font.NORMAL, BaseColor.BLACK)));
                    phrase.Add(new Chunk(DateTime.Now.ToLongDateString() , FontFactory.GetFont("Calibri", 10, Font.NORMAL, BaseColor.BLACK)));
                    cell = PhraseCell(phrase, Element.ALIGN_LEFT);
                    cell.Padding = 5f;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    table.AddCell(cell);

                    //Separater Line
                    color = new Color();
                    DrawLine(writer, 25f, document.Top - 79f, document.PageSize.Width - 25f, document.Top - 79f, BaseColor.BLACK);
                   // DrawLine(writer, 25f, document.Top - 80f, document.PageSize.Width - 25f, document.Top - 80f, BaseColor.BLACK);
                    document.Add(table);

                    table = new PdfPTable(3);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 2f, 2f, 2f });
                    table.SpacingBefore = 20f;

                    // Details
                    cell = PhraseCell(new Phrase(" Record", FontFactory.GetFont("Arial", 12, Font.UNDERLINE, BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);

                   
                    table = new PdfPTable(3);
                    table.SetWidths(new float[] { 2f, 2f, 2f });
                    table.TotalWidth = 340f;
                    table.LockedWidth = true;
                    table.SpacingBefore = 20f;
                    table.HorizontalAlignment = Element.ALIGN_RIGHT;

                    
                    DataTable sTable = dt;
                    var grouped = from x in sTable.AsEnumerable()
                                  group x by new { a = x["Masterkey"] } into g
                                  select new
                                  {
                                      Value = g.Key,
                                      ColumnValues = g
                                  };
                    DataTable dtfinal = null;
                    foreach (var key in grouped)
                    {
                        dtfinal = sTable.Clone();
                        foreach (var columnValue in key.ColumnValues)
                        {
                            dtfinal.ImportRow(columnValue);
                        }

                        PdfPTable table1 = new PdfPTable(2);
                        table1.DefaultCell.Padding = 10f;
                        table1.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                        table1.DefaultCell.Border = 0;
                        table1.HorizontalAlignment = Element.ALIGN_CENTER;
                        table1.TotalWidth = 500f;
                        table1.LockedWidth = true;
                        float[] widths1 = new float[] { 0.7f, 3f };

                        PdfPTable tableb = new PdfPTable(4);
                        float[] widthim = new float[] { 0.1f, 0.1f, 0.1f, 0.05f };
                        tableb.SetWidths(widthim);
                        tableb.DefaultCell.PaddingTop = 10f;
                        tableb.HorizontalAlignment = Element.ALIGN_CENTER;
                        tableb.DefaultCell.Border = 0;
                        tableb.TotalWidth = 550f;
                        tableb.LockedWidth = true;
                        PdfPCell header = new PdfPCell(new Phrase(" Q-year: " + dtfinal.Rows[0]["Q-Year"] + " MasterKey: " + dtfinal.Rows[0]["MasterKey"]+"      UW: " + dtfinal.Rows[0]["Underwriter"]+ "      Rel UW: " + dtfinal.Rows[0]["Rel Underwriter"] +Environment.NewLine+ "    Cedant: " + dtfinal.Rows[0]["Cedant"], FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)));
                        header.Indent = 10;
                        header.HorizontalAlignment = 1;
                        header.Padding = 10f;
                        header.Border = 0;
                        header.Colspan = 4;
                        tableb.AddCell(header);

                        PdfPTable table3 = new PdfPTable(3);
                        table3.TotalWidth = 500f;
                        table3.LockedWidth = true;
                        float[] widths2 = new float[] { 0.1f, 0.1f, 0.1f };
                        table3.SetWidths(widths2);
                        table3.DefaultCell.Padding = 5f;
                        
                        table3.HorizontalAlignment = Element.ALIGN_CENTER;
                        table3.SpacingBefore = 5f;
                        table3.AddCell("Fields".ToString());
                        table3.AddCell("Regis".ToString());
                        table3.AddCell("REVO".ToString()); 

                        document.Add(tableb);
                        document.Add(table3);
                        for (int j = 0; j < dtfinal.Rows.Count; j++)
                        {
                            table1 = new PdfPTable(3);
                            table1.TotalWidth = 500f;
                            table1.LockedWidth = true;
                            float[] widths = new float[] { 0.1f, 0.1f, 0.1f };
                            table1.SetWidths(widths);
                            table1.DefaultCell.Padding = 0f;
                            table1.DefaultCell.Border = 0;
                            table1.HorizontalAlignment = Element.ALIGN_CENTER;
                            table1.SpacingBefore = 5f;
                         
                           
                            if (dtfinal.Rows[j]["REVO"].ToString().Contains("Reason"))
                            {
                                table1.AddCell(new Phrase(dtfinal.Rows[j]["Field Difference"].ToString(), font2) );
                                table1.AddCell(new Phrase(dtfinal.Rows[j]["Regis"].ToString(), font2) );
                                table1.AddCell(new Phrase(dtfinal.Rows[j]["REVO"].ToString(),font2) );
                                
                            }
                            else
                            {
                                table1.AddCell(dtfinal.Rows[j]["Field Difference"].ToString());
                                table1.AddCell(dtfinal.Rows[j]["Regis"].ToString());
                                table1.AddCell(dtfinal.Rows[j]["REVO"].ToString());
                            }
                            
                           // table1.DefaultCell.Phrase = new Phrase() { BorderStyle.None.ToString() };
                            document.Add(table1);
                        }
                        Paragraph p = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
                        document.Add(p);

                    }
                    document.Close();
                    byte[] bytes = memoryStream.ToArray();
                    memoryStream.Close();
                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", "attachment; filename=REGIS_REVO_Data_Comparison.pdf");
                    Response.ContentType = "application/pdf";
                    Response.Buffer = true;
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.BinaryWrite(bytes);
                    HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
                    HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    Response.Close();
                }
            }
            catch(Exception ex)
            {
                string script = "<script>alert('" + ex.Message + "');</script>";

                ScriptManager.RegisterClientScriptBlock(this, typeof(string), "uniqueKey", script, true);

            }
        }

        private static void DrawLine(PdfWriter writer, float x1, float y1, float x2, float y2, BaseColor color)
        {
            PdfContentByte contentByte = writer.DirectContent;
            contentByte.SetColorStroke(color);
            contentByte.MoveTo(x1, y1);
            contentByte.LineTo(x2, y2);
            contentByte.Stroke();
        }
        private static PdfPCell PhraseCell(Phrase phrase, int align)
        {
            PdfPCell cell = new PdfPCell(phrase);
            cell.BorderColor = BaseColor.WHITE;
            cell.VerticalAlignment = Element.ALIGN_TOP;
            cell.HorizontalAlignment = align;
            cell.PaddingBottom = 2f;
            cell.PaddingTop = 0f;
            return cell;
        }
        private static PdfPCell ImageCell(string path, float scale, int align)
        {
            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath(path));
            image.ScalePercent(scale);
            PdfPCell cell = new PdfPCell(image);
            cell.BorderColor = BaseColor.WHITE;
            cell.VerticalAlignment = Element.ALIGN_TOP;
            cell.HorizontalAlignment = align;
            cell.PaddingBottom = 0f;
            cell.PaddingTop = 0f;
            return cell;
        }
        protected void ddlReason_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlReason.Text == "Others")
            {
                txtReason.Visible = true;

            }
            else
            {
                txtReason.Visible = false;
            }
            UpdatePanel4.Update();
        }



        protected void btnExportPdf_Click(object sender, EventArgs e)
        {
            try
            {
                
                    DataTable dt = new DataTable("GridView_Data");
                    foreach (TableCell cell in grdResult.HeaderRow.Cells)
                    {
                        dt.Columns.Add(cell.Text);
                    }
                    foreach (GridViewRow row in grdResult.Rows)
                    {
                        dt.Rows.Add();
                        for (int i = 0; i < row.Cells.Count; i++)
                        {
                            dt.Rows[dt.Rows.Count - 1][i] = Regex.Replace(row.Cells[i].Text, @"<[^>]+>|&nbsp;", "").Trim();
                        }
                    }
                    dt.Columns.RemoveAt(10);
                   // dt.Columns.Add("Q-Year");
                   
                    //var list = dt.AsEnumerable().Select(r => r["MasterKey"].ToString());
                    //string value ="";
                    //foreach (var vls in list)
                    //{
                    //    value += ("'" + (list) + "',");
                    //}
                    //value = value.TrimEnd(',');

                    List<FilterValues> fltval = CheckAllFilters();
                    using (var contxt = new DbAdapter())
                    {


                        using (var dt2 = contxt.GetResultWithExcludedData(rdBtnRptType.SelectedValue, fltval[0].lstYear, fltval[0].lstENT, fltval[0].lstUY, fltval[0].lstUW, fltval[0].lstField, fltval[0].lstStatus))
                        {
                            foreach (DataRow dr in dt2.Rows)
                            {
                                dt.Rows.Add(dr.ItemArray);
                            }
                        }
                    }
                if (dt.Rows.Count > 0)
                {
                    ExportToPdf(dt);
                }
                
            }
            catch (Exception ex)
            {

            }
        }

        public List<FilterValues> CheckAllFilters()
        {
            List<FilterValues> fltVal = new List<FilterValues>();
            var uychk = false;
            var uwchk = false;
            var Entchk = false;
            var Fieldchk = false;
            string lstEntity = "";
            string lstUY = "";
            string lstYear = "";
            string lstStatus = "";
            string lstField = "";
            string lstUW = "";
            foreach (GridViewRow item in grdUYCnt.Rows)
            {
                // check row is datarow
                if (item.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chk = (item.FindControl("chkUY") as CheckBox);
                    if (chk.Checked)
                    {
                        lstUY += ("'" + (item.Cells[1].Text).Trim() + "',");
                        //break;
                    }
                }
            }
            lstUY = lstUY.TrimEnd(',');
            // this.UWTxt.Clear();
            foreach (GridViewRow item in grdUWCount.Rows)
            {
                // check row is datarow
                if (item.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkSelect = (item.FindControl("chkEntStatus") as CheckBox);
                    if (chkSelect != null)
                    {
                        string uwtxt = item.Cells[1].Text.Trim();


                        if (chkSelect.Checked && !this.UWTxt.Contains(uwtxt))
                        {

                            this.UWTxt.Add(uwtxt);
                            lstUW += ("'" + item.Cells[1].Text.Trim() + "',");
                            uwchk = true;
                            // break;
                        }
                        else if (!chkSelect.Checked && this.UWTxt.Contains(uwtxt))
                        {
                            this.UWTxt.Remove(uwtxt);
                        }

                    }
                }
            }
            lstUW = lstUW.TrimEnd(',');
            foreach (GridViewRow item in grdEntityCnt.Rows)
            {
                // check row is datarow
                if (item.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chk = (item.FindControl("chkENT") as CheckBox);
                    if (chk.Checked)
                    {
                        lstEntity += ("'" + (item.Cells[1].Text).Trim() + "',");
                        Entchk = true;
                        //break;

                    }
                }
            }
            lstEntity = lstEntity.TrimEnd(',');

            foreach (GridViewRow item in grdStatusCount.Rows)
            {
                // check row is datarow
                if (item.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chk = (item.FindControl("chkEntStatus") as CheckBox);
                    if (chk.Checked)
                    {

                        lstStatus += ((item.Cells[1].Text).Trim()[1] + ",");
                        lstYear += ((item.Cells[1].Text).Split('-').Last() + ",");
                        //chk = true;
                        //break;

                    }
                }
            }
            lstStatus = lstStatus.TrimEnd(',');
            lstYear = lstYear.TrimEnd(',');




           
            foreach (GridViewRow item in grdFieldCount.Rows)
            {
                // check row is datarow
                if (item.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chk = (item.FindControl("chkEntStatus") as CheckBox);
                    if (chk.Checked)
                    {
                        // lstField =(item.Cells[1].Text);
                        // lstField += "''" + (item.Cells[1].Text) + "'',";
                        lstField += ("'" + (item.Cells[1].Text).Trim() + "',");
                        Fieldchk = true;
                        // break;
                    }
                }
            }
            // lstField = "''Accrual''";
            lstField = lstField.TrimEnd(',');

            fltVal.Add(new FilterValues
            {
                lstUW = lstUW,
                lstENT = lstEntity,
                lstField = lstField,
                lstStatus = lstStatus,
                lstYear = lstYear,
                lstUY = lstUY
            });
            return fltVal;
        }
        protected void grdEntityCnt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow gvr = e.Row;

            if (gvr.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkSelect = gvr.FindControl("chkEnt") as CheckBox;

                string lbltxt1 = e.Row.Cells[1].Text;
                if (chkSelect != null)
                {

                    if (this.UWTxt.Contains(lbltxt1))
                        chkSelect.Checked = true;
                    else
                        chkSelect.Checked = false;
                }
            }
        }
        protected void OnDataBound(object sender, EventArgs e)
        {
            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            for (int i = 0; i < grdUYCnt.Columns.Count; i++)
            {
                if (i != 0)
                {
                    TableHeaderCell cell = new TableHeaderCell();
                    TextBox txtSearch = new TextBox();
                    txtSearch.Attributes["placeholder"] = grdUYCnt.Columns[1].HeaderText;
                    txtSearch.CssClass = "search_textbox";
                    cell.Controls.Add(txtSearch);
                    row.Controls.Add(cell);
                }
            }
            grdUYCnt.HeaderRow.Parent.Controls.AddAt(1, row);
        }

        protected void grdExcluded1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "cmdExcludeField")
            {
                string field_name = (e.CommandArgument).ToString();
                string remainder = field_name.Substring(field_name.LastIndexOf(',') + 1);
                string last = field_name.Substring(0, field_name.IndexOf(","));
                using (var contxt = new DbAdapter())
                {
                    contxt.PutExcludeField(remainder, last, "", Page.User.Identity.Name, "Delete");
                    GetExcludedData();

                }
                ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "Popup", " ShowExclPopup2(); ", true);
            }else if (e.CommandName == "HistoryPopup")
            {
                string field_name = (e.CommandArgument).ToString();
                string platformId = field_name.Substring(field_name.LastIndexOf(',') + 1);
                string rpt_col = field_name.Substring(0, field_name.IndexOf(","));
                slcField.Visible = true;
                selectedFieldName2.Text = rpt_col;
                BindHistory(platformId, rpt_col, "ExclGrdHistory");

                ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "Popup", " ShowExclPopup2(); ", true);
            }
        }
    }
}