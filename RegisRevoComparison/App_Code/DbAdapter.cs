using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace RegisRevoComparison
{
    public class DbAdapter : IDisposable
    {
        private string _constring = ConfigurationManager.ConnectionStrings["New_REGREV_Conn"].ConnectionString;
        private readonly SqlConnection _regisRevoCon = null;
       
        public DbAdapter()
        {
            if (_regisRevoCon == null)
            {
                _regisRevoCon = new SqlConnection(_constring);
                _regisRevoCon.Open();
            }
        }

        public void Dispose()
        {
            if (_regisRevoCon != null)
            {
                _regisRevoCon.Dispose();
            }
        }
        public int GetControl(string UserNm)
        {
            var sql = @"select Control from [dbo].[aspnet_UsersHistory] where UserName=@Usernmae and RoleName='Accounting Feed'
";
            using (var cmd = new SqlCommand(sql, _regisRevoCon))
            {
                cmd.Parameters.Add(new SqlParameter("@Usernmae", UserNm));
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }


        public List<EntityCntStatus> GetEntityStatusCount(string rptType)
        {
            var items = new List<EntityCntStatus>();
            var sql = @"sp_regrev_get_ent_uy_cnt";
           
            using (var cmd = new SqlCommand(sql, _regisRevoCon))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@rpt_type", rptType));
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        items.Add(new EntityCntStatus
                        {
                           EntityName = rdr["legal_ent_code"].ToString(),
                            ChkBox = rdr["chkbox"].ToString(),
                            Count = rdr["cntstatus"].ToString(),
                            Status = rdr["status"].ToString(),
                            UY = rdr["cont_uy"].ToString()
                        });
                    }
                }
            }
            return items;
        }

        public List<EntityCnt> GetEntityCount(string rptType)
        {
            var items = new List<EntityCnt>();
            var sql = @"sp_regrev_get_ent_cnt";

            using (var cmd = new SqlCommand(sql, _regisRevoCon))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@rpt_type", rptType));
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        items.Add(new EntityCnt
                        {
                            EntityName = rdr["legal_ent_code"].ToString(),
                            Cnt = rdr["cntent"].ToString()
                        });
                    }
                }
            }
            return items;
        }

        public List<EntityCntStatus> GetEntityProgramCount(string rptType)
        {
            var items = new List<EntityCntStatus>();
            var sql = @"[sp_regrev_get_prog_cnt]";

            using (var cmd = new SqlCommand(sql, _regisRevoCon))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@rpt_type", rptType));
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        items.Add(new EntityCntStatus
                        {
                            EntityName = rdr["legal_ent_code"].ToString(),
                            ChkBox = rdr["chkbox"].ToString(),
                            Count = rdr["cntpgm"].ToString(),
                            Status = rdr["pgm_program"].ToString()
                           
                        });
                    }
                }
            }
            return items;
        }

        public List<EntityCntStatus> GetUWCount(string rptType)
        {
            var items = new List<EntityCntStatus>();
            var sql = @"sp_regrev_get_uw_cnt";

            using (var cmd = new SqlCommand(sql, _regisRevoCon))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@rpt_type", rptType));
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        items.Add(new EntityCntStatus
                        {
                            EntityName = rdr["uw_fullname"].ToString(),
                            ChkBox = rdr["chkbox"].ToString(),
                            Count = rdr["cntstat"].ToString(),
                            Status = rdr["status"].ToString()

                        });
                    }
                }
            }
            return items;
        }

        public List<EntityCntStatus> GetFieldCount(string rptType)
        {
            var items = new List<EntityCntStatus>();
            var sql = @"[sp_regrev_get_comp_cnt]";

            using (var cmd = new SqlCommand(sql, _regisRevoCon))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@rpt_type", rptType));
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        items.Add(new EntityCntStatus
                        {
                            EntityName = rdr["field_desc_name"].ToString(),
                            ChkBox = rdr["chkbox"].ToString(),
                            Count = rdr["cntcomp"].ToString()
                           

                        });
                    }
                }
            }
            return items;
        }

        public List<ExcludedFields> GetExcludeField(string platformId)
        {
            var items = new List<ExcludedFields>();
            var sql = @"select * from RegRevComp_exclude where uw_platform_id=@p_id";

            using (var cmd = new SqlCommand(sql, _regisRevoCon))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@p_id", platformId));
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        items.Add(new ExcludedFields
                        {
                            Field_desc = rdr["rpt_col"].ToString(),
                            PlatformId = rdr["uw_platform_id"].ToString()
                           


                        });
                    }
                }
            }
            return items;
        }
        public int PutExcludeField(string platformId,string rpt_col,string type)
        {
            var items = new List<ExcludedFields>();
            var sql = @"";
            if(type=="Insert")
                sql=   @"insert into  RegRevComp_exclude(uw_platform_id,rpt_col,who_excluded,date_excluded)
            values (@p_id,@rpt_col,'windows user',getdate())";
            else if (type == "Delete")
                sql = @"delete from RegRevComp_exclude where uw_platform_id=@p_id
and rpt_col=@rpt_col";


            using (var cmd = new SqlCommand(sql, _regisRevoCon))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@p_id",Convert.ToInt32( platformId)));
                cmd.Parameters.Add(new SqlParameter("@rpt_col", rpt_col));
                return cmd.ExecuteNonQuery();
                
            }
            return 0;
        }
        public List<CompareResult> GetCompareResult(string rptType, List<string> program,List<string> entity, List<string> uy, List<string> uw, List<string> reason, List<string> status)
        {
            var items = new List<CompareResult>();
            var sql = @"[sp_regrev_get_results]";

            using (var cmd = new SqlCommand(sql, _regisRevoCon))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                
                cmd.Parameters.Add("@rpt_type", SqlDbType.NVarChar).Value = rptType;
                cmd.Parameters.Add("@entity", SqlDbType.NVarChar).Value = (entity.Count > 0 ? "'" + string.Join("','", entity) + "'" : null);
                cmd.Parameters.Add("@status", SqlDbType.NVarChar).Value = (status.Count > 0 ? "'" + string.Join("','", status) + "'" : null);
                cmd.Parameters.Add("@uy", SqlDbType.NVarChar).Value = (uy.Count > 0 ? string.Join(",", uy) : null);
                cmd.Parameters.Add("@uw", SqlDbType.NVarChar).Value = (uw.Count > 0 ? "'" + string.Join("','", uw) + "'" : null);
                cmd.Parameters.Add("@program", SqlDbType.NVarChar).Value =( program.Count > 0 ? "'" + string.Join("','", program) + "'":null);
                cmd.Parameters.Add("@reason", SqlDbType.NVarChar).Value = (reason.Count > 0 ? "'" + string.Join("','", reason) + "'" : null);
                DataSet ds = new DataSet();
                SqlDataAdapter sAdap = new SqlDataAdapter(cmd);
                sAdap.Fill(ds);
                
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        items.Add(new CompareResult
                        {
                            EntityName = rdr["legal_ent_code"].ToString(),
                            MasterKey = rdr["cont_master_key"].ToString(),
                            PlatformId = rdr["uw_platform_id"].ToString(),
                            ContractId = rdr["contract_id"].ToString(),
                            RelUW = rdr["Rel_UW_Fullname"].ToString(),
                            FieldDiff = rdr["field_desc_name"].ToString(),
                            REGIS = rdr["regis"].ToString(),
                            UW = rdr["UW_Fullname"].ToString(),
                            Status = rdr["status"].ToString(),
                            REVO = rdr["revo"].ToString(),
                            UY = rdr["cont_uy"].ToString(),
                            Program = rdr["pgm_program"].ToString(),
                            RptCol = rdr["rpt_col"].ToString()

                        });
                    }
                }
            }
            return items;
        }
    }
}