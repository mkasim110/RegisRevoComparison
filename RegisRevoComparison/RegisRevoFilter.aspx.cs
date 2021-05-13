using ClosedXML.Excel;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace RegisRevoComparison
{
    public partial class RegisRevoFilter : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!Page.IsPostBack && !Page.IsCallback)
            {
                BindFilters(rdBtnRptType.SelectedValue);
              
            }
            System.Web.UI.ScriptManager.GetCurrent(this).RegisterPostBackControl(BtnExport);
        }
       
       
        public void BindFilters(string RptType)
        {
            using (var Context = new DbAdapter())
            {
                grdEntityCnt.DataSource= Context.GetEntityCount(RptType);
                grdEntityCnt.DataBind();
                grdEntityCntStatus.DataSource = Context.GetEntityStatusCount(RptType);
                grdEntityCntStatus.DataBind();
                grdProgramCount.DataSource = Context.GetEntityProgramCount(RptType);
                grdProgramCount.DataBind();
                grdUWCount.DataSource = Context.GetUWCount(RptType);
                grdUWCount.DataBind();
                grdFieldCount.DataSource = Context.GetFieldCount(RptType);
                grdFieldCount.DataBind();
                //grdResult.DataSource = Context.GetCompareResult(RptType, "418410,417770",("'ARL',"+"'ARE'"), "2020,2019", "'Martin Mello','Paula Lewin'", "'Bound Share','Flat Premium 100%'");
                //grdResult.DataBind();
                UpdatePanel2.Update();
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

        }

        protected void rdBtnRptType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindFilters(rdBtnRptType.SelectedValue);
        }

        
        protected void chkEntStatus_CheckedChanged(object sender, EventArgs e)
        {
            BindResultGrid();
        }
        public void BindResultGrid()
        {
            List<string> UY = new List<string>();
            List<string> Ent_code = new List<string>();
            List<string> lstProgram = new List<string>();
            List<string> lstStatus = new List<string>();
            List<string> lstField = new List<string>();
            List<string> lstUW = new List<string>();
            foreach (GridViewRow item in grdEntityCntStatus.Rows)
            {
                // check row is datarow
                if (item.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chk = (item.FindControl("chkEntStatus") as CheckBox);
                    if (chk.Checked)
                    {
                        UY.Add(item.Cells[1].Text);
                        Ent_code.Add(item.Cells[2].Text);
                        lstStatus.Add(item.Cells[3].Text);
                    }
                }
            }
            foreach (GridViewRow item in grdProgramCount.Rows)
            {
                // check row is datarow
                if (item.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chk = (item.FindControl("chkEntStatus") as CheckBox);
                    if (chk.Checked)
                    {
                        lstProgram.Add(item.Cells[2].Text);
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
                        lstUW.Add(item.Cells[1].Text);
                    }
                }
            }

            foreach (GridViewRow item in grdFieldCount.Rows)
            {
                // check row is datarow
                if (item.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chk = (item.FindControl("chkEntStatus") as CheckBox);
                    if (chk.Checked)
                    {
                        lstField.Add(item.Cells[1].Text);
                    }
                }
            }
            using (var contxt = new DbAdapter())
            {
                if (lstField.Count > 0 || lstProgram.Count > 0 || lstUW.Count > 0 || lstStatus.Count > 0 || Ent_code.Count > 0 || UY.Count > 0)
                {
                    grdResult.DataSource = contxt.GetCompareResult(rdBtnRptType.SelectedValue, lstProgram, Ent_code, UY, lstUW, lstField, lstStatus);
                }
                grdResult.DataBind();

            }
            if (grdResult.Rows.Count > 0)
                ShowingGroupingDataInGridView(grdResult.Rows, 0, 6);
            else
                grdResult.DataBind();


            UpdatePanel3.Update();
        }
        protected void grdResult_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "OpenPopup") return;
            
            string field_name = (e.CommandArgument).ToString();
            string rpt_col = field_name.Substring(field_name.LastIndexOf(',') + 1);
            string platformId = field_name.Substring(0, field_name.IndexOf(","));
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
            lblMsg.Text= Msg;
            lblField.Text = FieldNm;
            lblPID.Text = plat_id;
            UpdatePanel4.Update();


        }
        protected void btnExclude_Click(object sender, EventArgs e)
        {
           
        }

        protected void grdExcluded_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if(e.CommandName != "cmdExcludeField") return;

            string field_name = (e.CommandArgument).ToString();
            string last = field_name.Substring(field_name.LastIndexOf(',') + 1);
            string remainder = field_name.Substring(0, field_name.IndexOf(","));
            ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "Popup", " ShowPopup2(); ", true);
            
            lblField.Text =last;
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
                contxt.PutExcludeField(lblPID.Text, lblField.Text, "Insert");
                BindResultGrid();
                BindExcl( lblPID.Text, lblField.Text, "Successfully Excluded ");
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
                contxt.PutExcludeField(lblPID.Text, lblField.Text, "Delete");
                BindResultGrid();
                BindExcl(  lblPID.Text, lblField.Text, "Successfully Included ");
                btnExc.Visible = false;
                btnInc.Visible = false;
                lblMsg.ForeColor = Color.Green;
                UpdatePanel4.Update();
            }
            ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "Popup", "ShowPopup2();", true);
        }
    }
}