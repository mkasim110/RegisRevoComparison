using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XmlToJSON
{
    public class REVOFields
    {
    }
    // Root myDeserializedClass = JsonConvert.Deserializenumeric<Root>(myJsonResponse); 
    public class Contract
    {
        public List<object> cont_inst { get; set; }
        public object stg_id { get; set; }
        public string contract_id { get; set; }
        public string Cont_subno { get; set; }
        public string Cont_Report_CP_Name { get; set; }
        public string Cont_Reinsurer_Name { get; set; }
        public string Cont_Broker_Name { get; set; }
        public string pgm_id { get; set; }
        public string Cont_Report_CP_id { get; set; }
        public object Cont_Layer_Code { get; set; }
        public string Cont_Layer_Desc { get; set; }
        public int Cont_UY { get; set; }
        public object Cont_Master_Key { get; set; }
        public string Cont_Segment { get; set; }
        public string Cont_Reinsurer { get; set; }
        public string Cont_Broker { get; set; }
        public string Cont_Assumed_Ceded_Flag { get; set; }
        public string Cont_Type { get; set; }
        public string Cont_Type_Ins { get; set; }
        public string Facility_Code { get; set; }
        public string Cont_Geography { get; set; }
        public object Cont_Broker_Ref { get; set; }
        public string Cont_Renewal_Flag { get; set; }
        public string Cont_Date_Arrived { get; set; }
        public int Cont_No_Of_Reinst { get; set; }
        public object Cont_Retention { get; set; }
        public object Cont_EPI_OC_100 { get; set; }
        public object Cont_EPI_OC_Our_Share { get; set; }
        public object Cont_EPI_BC_100 { get; set; }
        public object Cont_Est_SPI_Our_Share { get; set; }
        public object Cont_EPI_BC_Our_Share { get; set; }
        public object Cont_Loss_Ratio_Ceded { get; set; }
        public string Cont_Currency_Primary { get; set; }
        public object Cont_Accrual_Flag { get; set; }
        public string Cont_UW_LOB { get; set; }
        public string Cont_Date_Effective { get; set; }
        public string Cont_Date_Expiration { get; set; }
        public double? Cont_Placement { get; set; }
        public double? Cont_100_Limit_Occurance { get; set; }
        public double? Cont_100_Limit_Aggregate { get; set; }
        public double? Cont_100_Limit_Risk { get; set; }
        public double? Cont_100_Agg_Deductible { get; set; }
        public object Cont_Our_Attachment_Point { get; set; }
        public object Cont_Our_Limit { get; set; }
        public object Cont_Our_Limit_Occurance { get; set; }
        public object Cont_Our_Limit_Risk { get; set; }
        public object Cont_Our_Limit_Agg { get; set; }
        public object Cont_Our_Agg_Deductible { get; set; }
        public double? Cont_100_Limit { get; set; }
        public double? Cont_100_Attachment_Point { get; set; }
        public string Cont_Attach_Basis { get; set; }
        public string Cont_Limit_Basis { get; set; }
        public double? Cont_Bound_Share { get; set; }
        public object Cont_Rate_On_Line { get; set; }
        public double? Cont_Loss_Ratio_Gross { get; set; }
        public double? cont_loss_ratio_pricing { get; set; }
        public double? Cont_Expense_Ratio_Gross { get; set; }
        public double? cont_expense_ratio_pricing { get; set; }
        public object Cont_Expense_Ratio_Ceded { get; set; }
        public object Cont_Combined_Ratio_Gross { get; set; }
        public object cont_combined_ratio_pricing { get; set; }
        public object Cont_Combined_Ratio_Ceded { get; set; }
        public object Cont_Comm_Accrual_Pct { get; set; }
        public double? Cont_Brokerage_Pct { get; set; }
        public double? Cont_Comm_Pct { get; set; }
        public double? Cont_Comm_Override_Pct { get; set; }
        public double? Cont_SS_Prov_Comm_Pct { get; set; }
        public string Cont_Comm_Variable_Flag { get; set; }
        public double? Cont_Comm_Variable_Low { get; set; }
        public double? Cont_Comm_Variable_High { get; set; }
        public double? Cont_Comm_Other { get; set; }
        public object Cont_Comm_Total { get; set; }
        public string Cont_Gross_Up_Flag { get; set; }
        public double? Cont_Gross_Up_Pct { get; set; }
        public double? Cont_FET_Taxes { get; set; }
        public double? Cont_PC_Reins_Profit_Exp_Pct { get; set; }
        public int Cont_PC_Deficit_CF_Years { get; set; }
        public int Cont_PC_Credit_CF_Years { get; set; }
        public int Cont_PC_Deficit_CF_Amt { get; set; }
        public string Cont_PC_Flag { get; set; }
        public double? Cont_PC_Pct { get; set; }
        public object Cont_IBNR_Calc_Type { get; set; }
        public object Cont_IBNR_Calc_Type_2 { get; set; }
        public object Cont_IBNR_Pct { get; set; }
        public object Cont_NC_Reason { get; set; }
        public object Cont_PC_Credit_CF_Flag { get; set; }
        public object Cont_Limit_100 { get; set; }
        public object Cont_Limit_Our_Share { get; set; }
        public object Cont_PC_Calc_Flag { get; set; }
        public object Cont_PC_Deficit { get; set; }
        public int source_system_id { get; set; }
        public string uw_source { get; set; }
        public int treaty_id { get; set; }
        public string Cont_SS_Calc_Flag { get; set; }
        public string Cont_SS_Flag { get; set; }
        public double? Cont_SS_Max_Commission_Pct { get; set; }
        public double? Cont_SS_Max_Loss_Ratio { get; set; }
        public double? Cont_SS_Min_Loss_Ratio { get; set; }
        public double? Cont_SS_Min_Commission_Pct { get; set; }
        public object cont_specific_cession_flag { get; set; }
        public object Cont_LAE_Terms { get; set; }
        public object Cont_PC_First_Calc_Date { get; set; }
        public object Contract_Renew_From_ID { get; set; }
        public object multi_year_expire { get; set; }
        public object multi_year_incept { get; set; }
        public object cont_loss_trigger { get; set; }
        public double? cont_premium_flat_100 { get; set; }
        public double? cont_premium_min_100 { get; set; }
        public double? cont_premium_deposit_100 { get; set; }
        public string Cont_Premium_Adj_XS { get; set; }
        public double? Cont_Premium_Adj_Rate { get; set; }
        public List<object> cont_reins { get; set; }
        public string Cont_Common_Acct_Flag { get; set; }
        public string Cont_AP_Flag { get; set; }
        public string Cont_NCB_Flag { get; set; }
        public double? Cont_NCB_Pct { get; set; }
        public string Cont_Stop_Loss_Flag { get; set; }
        public double? Cont_Stop_Loss_Limit_Pct { get; set; }
        public double? Cont_Stop_Loss_Attach_Pct { get; set; }
        public string Cont_PC_LC_Flag { get; set; }
        public double? Cont_PC_LC_Begin { get; set; }
        public double? Cont_PC_LC_End { get; set; }
        public double? Cont_PC_Cedeco_Retention_Pct { get; set; }
        public object cont_nth_event { get; set; }
        public string Cont_Accrual_Calc_Flag { get; set; }
        public object Cont_Currency_Secondary { get; set; }
        public string Cont_Install_Adjust_Date { get; set; }
        public string Cont_Install_As_Collected_Flag { get; set; }
        public string Cont_Prem_Method { get; set; }
        public double? Cont_Est_Ult_Arch_Prem { get; set; }
        public string Cont_Port_Flag { get; set; }
        public string Cont_QS_Of_XS { get; set; }
        public string Cont_Install_Freq { get; set; }
        public string Cont_Install_Equal_Flag { get; set; }
        public int Cont_Install_Settlement_Days { get; set; }
        public object Cont_BDX_Freq { get; set; }
        public int Cont_BDX_Settlement_Due_Days { get; set; }
        public int Cont_Bdx_Report_Due_Days { get; set; }
        public string Cont_ERC_Flg { get; set; }
        public double? Cont_ERC_Pct { get; set; }
        public object Cont_UPR_Code { get; set; }
        public string Cont_Est_SPI_100 { get; set; }
        public string Cont_SPI_100 { get; set; }

    }

    public class MyArray
    {
        public string pgm_id { get; set; }
        public object pgm_nbr { get; set; }
        public object pgm_sub_nbr { get; set; }
        public string pgm_assumed_ceded_flag { get; set; }
        public string pgm_cp_ceding { get; set; }
        public object pgm_desc { get; set; }
        public string pgm_uw { get; set; }
        public string pgm_office { get; set; }
        public object pgm_nav_nbr { get; set; }
        public object pgm_created_by { get; set; }
        public object pgm_created_when { get; set; }
        public object who_chg { get; set; }
        public object when_chg { get; set; }
        public object pgm_uw_orig { get; set; }
        public string pgm_uw_rel { get; set; }
        public List<Contract> contract { get; set; }

    }

    public class Root
    {
        public List<MyArray> MyArray { get; set; }

    }
}