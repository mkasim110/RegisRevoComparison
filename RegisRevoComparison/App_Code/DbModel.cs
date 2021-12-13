using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegisRevoComparison
{
    public class DbModel
    {
       // public List<EntityCnt> EntityCount { get; set; }
    }
    public class ExcludedFields
    {
        public string PlatformId { get; set; }
        public string Field_desc { get; set; }
        public string Reason { get; set; }

    }
    public class EntityCnt
    {
        public string EntityName { get; set; }
       
        public string Cnt { get; set; }
    }
    public class EntityCntStatus
    {
        public string EntityName { get; set; }
        public string Status { get; set; }
        public int Count { get; set; }
        public string UY { get; set; }
        public string ChkBox { get; set; }
    }
    public class CompareResult
    {
        public string Qyear { get; set; }
        public string EntityName { get; set; }
        public string MasterKey { get; set; }
        public string Status { get; set; }
        public string UW { get; set; }
        public string RelUW { get; set; }
        public string FieldDiff { get; set; }
        public string REGIS { get; set; }
        public string REVO { get; set; }
        public string ContractId { get; set; }
        public string PlatformId { get; set; }
        public string UY { get; set; }
        public string Program { get; set; }
        public string RptCol { get; set; }
        
    }
}