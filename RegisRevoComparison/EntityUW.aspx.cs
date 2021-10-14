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


namespace RegisRevoComparison
{
    public partial class EntityUW : System.Web.UI.Page
    {
        List<Contract> myDeserializedClass;
        DataTable dt = new DataTable();
        DataRow dr;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack && !Page.IsCallback)
            {
                BindRefresh();

            }
            System.Web.UI.ScriptManager.GetCurrent(this).RegisterPostBackControl(BtnExport);
            System.Web.UI.ScriptManager.GetCurrent(this).RegisterPostBackControl(btnEntUw);
        }

        public void BindRefresh()
        {

            using (var Context = new DbAdapter())
            {
                lblData.Text = "Data as at : " + Context.GetDataLastUpdateDate().ToString("MMM dd yyyy hh:mmtt");
                //grdEntityCnt.DataSource = Context.GetEntityCount(rdBtnRptType.SelectedValue,);
                //grdEntityCnt.DataBind();
                string usernm = HttpContext.Current.User.Identity.Name.ToString();
               
                //usernm = usernm.Substring(usernm.IndexOf("\\")+1);
                lblUser.InnerText = usernm;
                DtUW();
                UpdatePanel2.Update();
            }
        }
        protected void DtUW()
        {
            DataTable table = new DataTable();

            table.Columns.Add(new DataColumn("UW", typeof(string)));
            table.Columns.Add(new DataColumn("Count", typeof(int)));
            table.Columns.Add(new DataColumn("Status", typeof(string)));
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
            grdStatusCount.DataSource = table;
            grdStatusCount.DataBind();
            grdFieldCount.DataSource = table;
            grdFieldCount.DataBind();
            grdUYCnt.DataSource = table;
            grdUYCnt.DataBind();
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


            table.Columns.Add(new DataColumn("PlatformId", typeof(string)));
            table.Columns.Add(new DataColumn("MasterKey", typeof(string)));

            table.Columns.Add(new DataColumn("FieldDiff", typeof(string)));
            table.Columns.Add(new DataColumn("RelUW", typeof(string)));
            table.Columns.Add(new DataColumn("REGIS", typeof(string)));
            table.Columns.Add(new DataColumn("REVO", typeof(string)));


            grdResult.DataSource = table;
            grdResult.DataBind();
            UpdatePanel3.Update();
        }
        public void BindFilters(string RptType, string ent, string uw, string uy, string program, string status)
        {
            using (var Context = new DbAdapter())
            {

                grdUYCnt.DataSource = Context.GetUYCount(RptType, ent);
                grdUYCnt.DataBind();
                grdStatusCount.DataSource = Context.GetStatusCount(RptType, ent, uy, uw);
                grdStatusCount.DataBind();
                grdUWCount.DataSource = Context.GetUWCount(RptType, ent, uy);
                grdUWCount.DataBind();
                grdFieldCount.DataSource = Context.GetFieldCount(RptType, ent, uw, uy, program, status);
                grdFieldCount.DataBind();

                grdResult.DataSource = Context.GetCompareResult(RptType, program, ent, uy, uw, "", status);
                grdResult.DataBind();
                ShowingGroupingDataInGridView(grdResult.Rows, 0, 6);

                UpdatePanel2.Update();
                UpdatePanel5.Update();
            }
            //ShowingGroupingDataInGridView(grdResult.Rows, 0, 6);
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
            }
            //ShowingGroupingDataInGridView(grdResult.Rows, 0, 6);
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
                        dt.Rows[dt.Rows.Count - 1][i] = row.Cells[i].Text;
                    }
                }
               // dt.Columns.RemoveAt(9);
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt);

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
           // getnewmethod();
            CallAsysnAsync();
            BindRefresh();
           

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
        }


        protected void chkEntStatus_CheckedChanged(object sender, EventArgs e)
        {
            BindResultGrid();
        }
        public void BindResultGrid()
        {
            var uychk = false;
            var uwchk = false;
            var statuschk = false;
            var Fieldchk = false;
            string lstEntity = "";
            string lstUY = "";
            string lstProgram = "";
            string lstStatus = "";
            string lstField = "";
            string lstUW = "";
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
            foreach (GridViewRow item in grdUYCnt.Rows)
            {
                // check row is datarow
                if (item.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chk = (item.FindControl("chkUY") as CheckBox);
                    if (chk.Checked)
                    {
                        lstUY = (item.Cells[1].Text);
                        uychk = true;
                        break;

                    }
                }
            }
            if (!uychk)
                goto FIlterpart;

            foreach (GridViewRow item in grdUWCount.Rows)
            {
                // check row is datarow
                if (item.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chk = (item.FindControl("chkEntStatus") as CheckBox);
                    if (chk.Checked)
                    {
                        lstUW = (item.Cells[1].Text);
                        uwchk = true;
                        break;
                    }
                }
            }
            if (!uwchk)
                goto FIlterpart;
            foreach (GridViewRow item in grdStatusCount.Rows)
            {
                // check row is datarow
                if (item.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chk = (item.FindControl("chkEntStatus") as CheckBox);
                    if (chk.Checked)
                    {
                        lstStatus = (item.Cells[1].Text);
                        statuschk = true;
                        break;
                    }
                }
            }
        //if (!statuschk)
        //    goto FIlterpart;
        FIlterpart:





            using (var contxt = new DbAdapter())
            {

                if (uychk)
                {
                    BindFiltersWithoutUY(rdBtnRptType.SelectedValue, lstEntity, lstUW, lstUY, lstProgram, lstStatus, lstField);
                }
                else if (uwchk)
                {
                    BindFiltersWithoutUY(rdBtnRptType.SelectedValue, lstEntity, lstUW, lstUY, lstProgram, lstStatus, lstField);
                }
                else if (statuschk)
                {
                    BindFiltersWithoutUY(rdBtnRptType.SelectedValue, lstEntity, lstUW, lstUY, lstProgram, lstStatus, lstField);
                }
                else if (Fieldchk)
                {
                    BindFiltersWithoutUY(rdBtnRptType.SelectedValue, lstEntity, lstUW, lstUY, lstProgram, lstStatus, lstField);
                }


                if (lstProgram != "" || lstUW != "" || lstStatus != "" || lstUY != "")
                {
                    grdResult.DataSource = contxt.GetCompareResult(rdBtnRptType.SelectedValue, lstProgram, lstEntity, lstUY, lstUW, lstField, lstStatus);

                }
                else
                {
                    BindFilters(rdBtnRptType.SelectedValue, lstEntity, lstUW, lstUY, lstProgram, lstStatus);
                }
                grdResult.DataBind();

            }
            if (grdResult.Rows.Count > 0)
                ShowingGroupingDataInGridView(grdResult.Rows, 0, 6);
            else
                BindDtResult();


            UpdatePanel3.Update();
        }
        protected void grdResult_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "OpenPopup") return;

            string field_name = (e.CommandArgument).ToString();
            string platformId = field_name.Substring(field_name.LastIndexOf(',') + 1);
            string rpt_col = field_name.Substring(0, field_name.IndexOf(","));
            BindExcl(platformId, rpt_col, "Are you sure you want to Include the field ");
            btnInc.Visible = false;
            btnExc.Visible = true;

            ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "Popup", "ShowPopup();", true);

        }
        public void BindExcl(string plat_id, string FieldNm, string Msg)
        {
            using (var contxt = new DbAdapter())
            {

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
            using (var contxt = new DbAdapter())
            {
                contxt.PutExcludeField(lblPID.Text, lblField.Text,"", Page.User.Identity.Name, "Insert");
                BindResultGrid();
                BindExcl(lblPID.Text, lblField.Text, "Successfully Excluded ");
                btnExc.Visible = false;
                btnInc.Visible = false;
                lblMsg.ForeColor = Color.Green;
                UpdatePanel4.Update();
            }
            ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "Popup", "ShowPopup2();", true);
        }

        protected void btnInc_Click(object sender, EventArgs e)
        {
            using (var contxt = new DbAdapter())
            {
                contxt.PutExcludeField(lblPID.Text, lblField.Text,"", Page.User.Identity.Name, "Delete");
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
        }

        #region APIPULL
        private async static Task<IRestResponse> NewMethod(RestRequest request)
        {
            var client = new RestClient(System.Web.Configuration.WebConfigurationManager.AppSettings["ApiURL"]);
            client.Authenticator = new NtlmAuthenticator(System.Web.Configuration.WebConfigurationManager.AppSettings["ApiUserName"], System.Web.Configuration.WebConfigurationManager.AppSettings["ApiPassword"]);
            var cancellationTokenSource = new CancellationTokenSource();
            return await client.ExecuteAsync(request,cancellationTokenSource.Token);
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
                }else
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
                int isExc = contxt.ExcCompSP(datafile);
            }
            dt.Rows.Clear();
            // connection();

             string jsMethodName = "HideProgressBar();";
            //string script = "window.onload = function() { HideProgressBar(); };";
             ScriptManager.RegisterClientScriptBlock(this, typeof(string), "uniqueKey", jsMethodName, true);
            //   ClientScript.RegisterStartupScript(this.GetType(), "HideProgressBar", script, true);
            // Page.Response.Redirect(Page.Request.Url.ToString(), true);

            //return await dmrs;
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
                var postResponse =  t.Status.ToString(); //t.Result would be okay too.
                postResponses.Add(postResponse);
                Console.WriteLine(postResponse);
            }
            
        }

        public void InstDataToRegis(string stops1,string datafile)
        {
            var stops = JArray.Parse(stops1);
            for (int i = 0; i < stops.Count; i++)
            {
                myDeserializedClass = JsonConvert.DeserializeObject<List<Contract>>(stops[i]["contract"].ToString());


                foreach (var prop in myDeserializedClass)
                {

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
                        dr["No_of_Reinstatement"] = prop.cont_reins[0].cont_reins_qty;
                    else
                        dr["No_of_Reinstatement"] = "NULL";
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
                    dr["Sliding_Scale"] = prop.Cont_SS_Calc_Flag;
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
                    dr["SLidingScaleFlag"] = prop.Cont_SS_Flag;

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
                    dr["AdjustmentDate"] = prop.Cont_Install_Adjust_Date ?? "";
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
            var uychk = false;
            var uwchk = false;
            var statuschk = false;
            var Fieldchk = false;
            string lstEntity = "";
            string lstUY = "";
            string lstProgram = "";
            string lstStatus = "";
            string lstField = "";
            string lstUW = "";
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
            foreach (GridViewRow item in grdUYCnt.Rows)
            {
                // check row is datarow
                if (item.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chk = (item.FindControl("chkUY") as CheckBox);
                    if (chk.Checked)
                    {
                        lstUY = (item.Cells[1].Text);
                        uychk = true;
                        break;

                    }
                }
            }
           

            foreach (GridViewRow item in grdUWCount.Rows)
            {
                // check row is datarow
                if (item.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chk = (item.FindControl("chkEntStatus") as CheckBox);
                    if (chk.Checked)
                    {
                        lstUW = (item.Cells[1].Text);
                        uwchk = true;
                        break;
                    }
                }
            }
           
            foreach (GridViewRow item in grdStatusCount.Rows)
            {
                // check row is datarow
                if (item.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chk = (item.FindControl("chkEntStatus") as CheckBox);
                    if (chk.Checked)
                    {
                        lstStatus = (item.Cells[1].Text);
                        statuschk = true;
                        break;
                    }
                }
            }
        //if (!statuschk)
        //    goto FIlterpart;
        FIlterpart:
            foreach (GridViewRow item in grdFieldCount.Rows)
            {
                // check row is datarow
                if (item.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chk = (item.FindControl("chkEntStatus") as CheckBox);
                    if (chk.Checked)
                    {
                        lstField = (item.Cells[1].Text);
                        Fieldchk = true;
                        break;
                    }
                }
            }



            using (var contxt = new DbAdapter())
            {






                if (lstField != "" || lstProgram != "" || lstUW != "" || lstStatus != "" || lstUY != "")
                {
                    grdResult.DataSource = contxt.GetCompareResult(rdBtnRptType.SelectedValue, lstProgram, lstEntity, lstUY, lstUW, lstField, lstStatus);

                }
                else
                {
                    BindFilters(rdBtnRptType.SelectedValue, lstEntity, lstUW, lstUY, lstProgram, lstStatus);
                }
                grdResult.DataBind();

            }
            if (grdResult.Rows.Count > 0)
                ShowingGroupingDataInGridView(grdResult.Rows, 0, 6);
            else
                BindDtResult();


            UpdatePanel3.Update();
        }

        protected void chkEnt_CheckedChanged1(object sender, EventArgs e)
        {

            //string jsEndMethodName = "ShowProgressBar();";
            //ScriptManager.RegisterClientScriptBlock(this, typeof(string), "uniqueKey", jsEndMethodName, true);


            string lstEntity = "";
            string lstUY = "";
            string lstProgram = "";
            string lstStatus = "";
            var IsEnt = false;


            string lstUW = "";
            foreach (GridViewRow item in grdEntityCnt.Rows)
            {
                // check row is datarow
                if (item.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chk = (item.FindControl("chkEnt") as CheckBox);
                    if (chk.Checked)
                    {
                        lstEntity = (item.Cells[1].Text);
                        IsEnt = true;
                        break;
                    }
                }
            }





            if (IsEnt)
            {

                BindFilters(rdBtnRptType.SelectedValue, lstEntity, lstUW, lstUY, lstProgram, lstStatus);
              
               // BindDtResult();
            }
            else
            {
                BindRefresh();
                grdResult.DataSource = null;
                grdResult.DataBind();
            }




            //string jsMethodName = "HideProgressBar();";            
            //ScriptManager.RegisterClientScriptBlock(this, typeof(string), "uniqueKey", jsMethodName, true);


            UpdatePanel3.Update();
            UpdatePanel5.Update();
        }

        protected void chkUY_CheckedChangedUY(object sender, EventArgs e)
        {
            string lstEntity = "";
            string lstUY = "";
            var IsEnt = false;
            var uychk = false;
            foreach (GridViewRow item in grdEntityCnt.Rows)
            {
                // check row is datarow
                if (item.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chk = (item.FindControl("chkEnt") as CheckBox);
                    if (chk.Checked)
                    {
                        lstEntity = (item.Cells[1].Text);
                        IsEnt = true;
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
                        lstUY = (item.Cells[1].Text);
                        uychk = true;
                        break;

                    }
                }
            }
            if (IsEnt)
            {
                if (uychk)
                {
                    BindFiltersWithoutUY(rdBtnRptType.SelectedValue, lstEntity, "", lstUY, "", "", "");
                    using (var contxt = new DbAdapter())
                    {
                        grdResult.DataSource = contxt.GetCompareResult(rdBtnRptType.SelectedValue, "", lstEntity, lstUY, "", "", "");


                        grdResult.DataBind();
                    }


                    if (grdResult.Rows.Count > 0)
                        ShowingGroupingDataInGridView(grdResult.Rows, 0, 6);
                    else
                        BindDtResult();
                }
                else
                {
                    BindFilters(rdBtnRptType.SelectedValue, lstEntity, "", lstUY, "", "");
                }
            }
            else
            {
                BindRefresh();
                grdResult.DataSource = null;
                grdResult.DataBind();

            }
            UpdatePanel3.Update();
            UpdatePanel5.Update();
        }

        protected void chkEntStatus_CheckedChangedUW(object sender, EventArgs e)
        {
            this.UWTxt.Clear();
            List<FilterValues> fltval = CheckFilters();
            if (fltval[0].lstENT.ToString() != "")
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

                    grdFieldCount.DataSource = contxt.GetFieldCount(rdBtnRptType.SelectedValue, fltval[0].lstENT, fltval[0].lstUW, fltval[0].lstUY, "", fltval[0].lstStatus);
                    grdFieldCount.DataBind();
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
      
        protected void chkEntStatus_CheckedChangedStatus(object sender, EventArgs e)
        {
            this.StatusTxt.Clear();
            List<FilterValues> fltval = CheckFilters();
            if (fltval[0].lstENT.ToString() != "")
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
                    grdFieldCount.DataSource = contxt.GetFieldCount(rdBtnRptType.SelectedValue, fltval[0].lstENT, fltval[0].lstUW, fltval[0].lstUY, "", fltval[0].lstStatus);
                    grdFieldCount.DataBind();
                   
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
                ShowingGroupingDataInGridView(grdResult.Rows, 0, 6);
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
            var statuschk = false;
            var Fieldchk = false;
            string lstEntity = "";
            string lstUY = "";
            string lstProgram = "";
            string lstStatus = "";
            string lstField = "";
            string lstUW = "";
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
            foreach (GridViewRow item in grdUYCnt.Rows)
            {
                // check row is datarow
                if (item.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chk = (item.FindControl("chkUY") as CheckBox);
                    if (chk.Checked)
                    {
                        lstUY = (item.Cells[1].Text);
                        uychk = true;
                        break;

                    }
                }
            }
           

            foreach (GridViewRow item in grdUWCount.Rows)
            {
                // check row is datarow
                if (item.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkSelect = (item.FindControl("chkEntStatus") as CheckBox);
                    if (chkSelect != null)
                    {
                        string uwtxt = item.Cells[1].Text;
                        

                        if (chkSelect.Checked && !this.UWTxt.Contains(uwtxt))
                        {
                            this.UWTxt.Clear();
                            this.UWTxt.Add(uwtxt);
                            lstUW = uwtxt;
                            uwchk = true;
                            break;
                        }
                        else if (!chkSelect.Checked && this.UWTxt.Contains(uwtxt))
                        {
                            this.UWTxt.Remove(uwtxt);
                        }

                    }
                }
            }
           
            foreach (GridViewRow item in grdStatusCount.Rows)
            {
                // check row is datarow
                if (item.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkSelect = (item.FindControl("chkEntStatus") as CheckBox);
                    if (chkSelect != null)
                    {
                        string ststxt = item.Cells[1].Text;
                       
                        
                        if (chkSelect.Checked && !this.StatusTxt.Contains(ststxt))
                        {
                            this.StatusTxt.Clear();
                            this.StatusTxt.Add(ststxt);
                            lstStatus = ststxt;
                            statuschk = true;
                            break;
                        }
                        else if (!chkSelect.Checked && this.StatusTxt.Contains(ststxt))
                        {
                            this.StatusTxt.Remove(ststxt);
                        }
                      
                    }
                    
                }
            }
            if (!statuschk)
                StatusTxt.Clear();
            if (!uwchk)
                UWTxt.Clear();

            if (StatusTxt.Count >0)
             lstStatus = StatusTxt[0];

            if (UWTxt.Count > 0)
                lstUW = UWTxt[0];

            fltVal.Add(new FilterValues
            {
            lstUW = lstUW,
            lstENT=lstEntity,
            lstField=lstField,
            lstStatus=lstStatus,
            lstUY=lstUY
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
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt,"Excluded_Data");
                   
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
    }
}