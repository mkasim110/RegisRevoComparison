using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChoETL;
using JsonDiffPatchDotNet;
using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;

namespace XmlToJSON
{
    public partial class Default : System.Web.UI.Page
    {
        private SqlConnection con,con1;
        private SqlCommand cmd;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
               
                if (!IsPostBack)
                {
                    Generatereport();
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }


        }
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["Fac_conn"].ToString();
            string constr1= ConfigurationManager.ConnectionStrings["Fac_conn1"].ToString();
            con = new SqlConnection(constr);
            con1= new SqlConnection(constr1);


        }


        
       
        void Generatereport()
        {
         


            var queryResult1 = "[";

            for (int i = 3000; i < 5000; i = i + 500)
            {


                var client = new RestClient("http://bmrevoappqa:8080/api/RegisTransformer/InforceContracts/2020-07-01/" + i + "");
                // var client = new RestClient("http://bmrevoappqa:8080/api/RegisTransformer/InforceContracts/2020-07-01/4000");
                client.Authenticator = new NtlmAuthenticator("mkasim", "Amuf$0110");
                var request = new RestRequest(Method.GET);
                request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
                var queryResult = client.Execute(request);
                if (queryResult.Content.Length > 2)
                {
                    queryResult1 += queryResult.Content.Substring(1, queryResult.Content.Length - 2)+",";
                }
                else
                {
                    queryResult1 += "]";
                    break;

                }
            }
         
            var stops = JArray.Parse(queryResult1);
            List<Contract> myDeserializedClass;
            DataTable dt = new DataTable();
            DataRow dr;
            connection();

            cmd = new SqlCommand("sp_get_Regis_revo_comparison");
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@eff_date", "2020/07/01");
            var da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            for (int i = 0; i < stops.Count; i++)
            {
                myDeserializedClass = JsonConvert.DeserializeObject<List<Contract>>(stops[i]["contract"].ToString());


                foreach (var prop in myDeserializedClass)
                {
                    bool contains = dt.AsEnumerable().Any(row => prop.source_system_id.ToString() == row.Field<String>("uw_platform_id"));
                    if (contains)
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
                        dr["No_of_Reinstatement"] = prop.Cont_No_Of_Reinst;
                        dr["OccurLimit"] = string.Format("{0:#,##0.##}", prop.Cont_100_Limit_Occurance);
                        dr["OurLimitAgg"] = string.Format("{0:#,##0.##}", prop.Cont_100_Limit_Aggregate);
                        dr["OurAggDeductible"] = prop.Cont_Our_Agg_Deductible ?? 0;
                        dr["AttachmentBasis"] = prop.Cont_Attach_Basis;
                        dr["LimitBais"] = prop.Cont_Limit_Basis;
                        dr["BoundShare"] = string.Format("{0:#,##0.00000000}", prop.Cont_Bound_Share);
                        dr["Est_SPI_100"] = String.Format("{0:n0}", Double.Parse(prop.Cont_Est_SPI_100));
                        dr["Brokerage"] = string.Format("{0:#,##0.00000000}", prop.Cont_Brokerage_Pct);
                        dr["Commission"] = string.Format("{0:#,##0.00000000}", prop.Cont_Comm_Pct);
                        dr["Comm_Overide_pct"] = string.Format("{0:#,##0.00000000}", prop.Cont_Comm_Override_Pct);
                        dr["Comm_variable"] = prop.Cont_Comm_Variable_Flag;
                        dr["Comm_variable_low"] = string.Format("{0:#,##0.00000000}", prop.Cont_Comm_Variable_Low);
                        dr["Comm_variable_high"] = string.Format("{0:#,##0.00000000}", prop.Cont_Comm_Variable_High);
                        dr["OtherComm"] = string.Format("{0:#,##0.00000000}", prop.Cont_Comm_Other);
                        dr["GrossUp"] = string.Format("{0:#,##0.00000000}", prop.Cont_Gross_Up_Flag);

                        dr["GrossUpPer"] = string.Format("{0:#,##0.00000000}", prop.Cont_Gross_Up_Pct);
                        dr["FET_Taxes"] = prop.Cont_FET_Taxes;
                        dr["ReinProfitExpence"] = string.Format("{0:#,##0.00000000}", prop.Cont_PC_Reins_Profit_Exp_Pct);
                        dr["CurrencyPrimary"] = prop.Cont_Currency_Primary;
                        dr["PC_Deficit_Years"] = prop.Cont_PC_Deficit_CF_Years;
                        dr["PC_Defict_Amt"] = prop.Cont_PC_Deficit_CF_Amt;
                        dr["PC_Calc"] = prop.Cont_PC_Calc_Flag;
                        dr["PC_percent"] = string.Format("{0:#,##0.0000}", prop.Cont_PC_Pct);
                        dr["Sliding_Scale"] = prop.Cont_SS_Calc_Flag;
                        dr["PC_Calc_date"] = prop.Cont_PC_First_Calc_Date;
                        dr["SS_Max_Comm_pct"] = string.Format("{0:#,##0.00000000}", prop.Cont_SS_Max_Commission_Pct);
                        dr["SS_Max_Loss_Ratio"] = string.Format("{0:#,##0.00000000}", prop.Cont_SS_Max_Loss_Ratio);
                        dr["placement"] = prop.Cont_Placement;
                        dr["LossTrigger"] = prop.cont_loss_trigger;
                        dr["Cont_Prem_Deposit_100"] = string.Format("{0:#,##0.##}", prop.cont_premium_deposit_100);
                        dr["FlatPremium100"] = string.Format("{0:#,##0.##}", prop.cont_premium_flat_100);
                        dr["MinPremium100"] = string.Format("{0:#,##0.##}", prop.cont_premium_min_100);

                        dr["Cedant"] = prop.Cont_Report_CP_id;
                        dr["Reference"] = prop.Cont_Broker_Ref;
                        dr["ContractType"] = prop.Cont_Type;
                        dr["AttachmentPoint100"] = prop.Cont_100_Attachment_Point;
                        dr["ContractLimit100"] = prop.Cont_100_Limit;
                        dr["AggregateLimit100"] = prop.Cont_100_Limit_Aggregate;
                        dr["RiskLimit100"] = prop.Cont_100_Limit_Risk;
                        dr["AggregateDeductible100"] = prop.Cont_100_Agg_Deductible;
                        dr["EstimatedSPI100"] = String.Format("{0:n}", Double.Parse(prop.Cont_Est_SPI_100));
                        dr["SPI100"] = String.Format("{0:n}", Double.Parse(prop.Cont_SPI_100));
                        dr["Accrual"] = prop.Cont_Accrual_Calc_Flag;
                        dr["LAETerms"] = prop.Cont_LAE_Terms;
                        dr["SS_Prov_Comm_Pct"] = string.Format("{0:#,##0.00000000}", prop.Cont_SS_Prov_Comm_Pct);
                        dr["SS_Min_Comm_Pct"] = string.Format("{0:#,##0.00000000}", prop.Cont_SS_Min_Commission_Pct);
                        dr["MultiYearExpire"] = prop.multi_year_expire ?? DBNull.Value; 
                        dr["MultiYearIncept"] = prop.multi_year_incept ?? DBNull.Value;
                        dr["CCFYears"] = prop.Cont_PC_Credit_CF_Years ;
                        dr["SLidingScaleFlag"] = prop.Cont_SS_Flag;

                        dr["AdjustableRate"] = prop.Cont_Premium_Adj_Rate;
                        dr["AdjustmentBase"] = prop.Cont_Premium_Adj_XS;
                        dr["Sub_No"] = prop.stg_id ?? DBNull.Value;
                        dr["Earnings"] = prop.Cont_UPR_Code;
                        dr["PremiumEarnings"] = prop.Cont_UPR_Code;
                        dr["PremiumMethod"] = prop.Cont_Prem_Method;
                        dr["LossMethod"] = prop.Cont_Prem_Method;
                        dr["CommAcct"] = prop.Cont_Common_Acct_Flag;
                        dr["AP"] = prop.Cont_AP_Flag;
                        dr["ExperianceRate"] = prop.Cont_Stop_Loss_Flag;
                        dr["NCB"] = prop.Cont_NCB_Flag;
                        dr["NCB_pct"] = prop.Cont_NCB_Pct;
                        dr["StopLoss"] = prop.Cont_Stop_Loss_Flag;
                        dr["PercentLimit"] = prop.Cont_Stop_Loss_Limit_Pct;
                        dr["LossCorridor"] = prop.Cont_Stop_Loss_Attach_Pct;
                        dr["PC_LC_Flag"] = prop.Cont_PC_LC_Flag;
                        dr["LowerThreshold"] = prop.Cont_PC_LC_Begin;

                        dr["UpperThreshold"] = prop.Cont_PC_LC_End;
                        dr["CedantParticipation"] = prop.Cont_PC_Cedeco_Retention_Pct;
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
                        dr["ERC_pct"] = prop.Cont_ERC_Pct ?? 0; 
                        dr["BDX_Frequency"] = prop.Cont_BDX_Freq;
                        dr["ProgramId"] = stops[i]["pgm_id"].ToString();
                        dr["pgm_AC"] = stops[i]["pgm_assumed_ceded_flag"].ToString();
                        dr["Pgm_cedant"] = stops[i]["pgm_cp_ceding"].ToString();
                        dr["Pgm_description"] = stops[i]["pgm_desc"].ToString();
                        dr["Pgm_UW"] = stops[i]["pgm_uw"].ToString();
                        dr["Pgm_UW_rel"] = stops[i]["pgm_uw_rel"].ToString();
                        dr["Pgm_office"] = stops[i]["pgm_office"].ToString();
                        
                        dt.Rows.Add(dr);
                    }

                }
            }
           

            ReportViewer1.Reset();
            ReportViewer1.SizeToReportContent = true;
            ReportViewer1.ProcessingMode = ProcessingMode.Local;

            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/rptRegisREVOcomp.rdlc");
            DataSet1 rptDs = new DataSet1();
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportDataSource _rsource1 = new ReportDataSource("DataSet1", dt);
            ReportViewer1.LocalReport.DataSources.Add(_rsource1);
            var ds = new DataSet("DataSet1");
            // string crtTable= CreateTABLE("RegisRevoDt",dt);

            using (var sqlBulk = new SqlBulkCopy(ConfigurationManager.ConnectionStrings["Fac_conn"].ToString()))
            {
                sqlBulk.DestinationTableName = "RegisRevoDt";
                sqlBulk.WriteToServer(dt);
            }
            ReportViewer1.AsyncRendering = false;
           
            ReportViewer1.ZoomMode = ZoomMode.FullPage;
            ReportViewer1.Width = 75;
            ReportViewer1.LocalReport.Refresh();
        }

    }
}