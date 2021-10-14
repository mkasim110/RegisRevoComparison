using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Reflection;

namespace RegisRevoComparison
{
    public class DbAdapter : IDisposable
    {
        private string _constring = ConfigurationManager.ConnectionStrings["New_REGREV_Conn"].ConnectionString;
        private readonly SqlConnection _regisRevoCon = null;
        private string _constring2 = ConfigurationManager.ConnectionStrings["TB_conn"].ConnectionString;
        private readonly SqlConnection _regisRevoCon2 = null;
        public DbAdapter()
        {
            if (_regisRevoCon == null)
            {
                _regisRevoCon = new SqlConnection(_constring);
                _regisRevoCon.Open();
            }
            if (_regisRevoCon2 == null)
            {
                _regisRevoCon2 = new SqlConnection(_constring2);
                _regisRevoCon2.Open();
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

        public int ChkReasons(string Reason)
        {
            var sql = @"select count(*) from tblReasons where reason =@Reason";
            using (var cmd = new SqlCommand(sql, _regisRevoCon))
            {
                cmd.Parameters.Add(new SqlParameter("@Reason", Reason));
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
        public DateTime GetDataLastUpdateDate()
        {
            var sql = @"select date_start from [dbo].[Is_data_reload]";
            using (var cmd = new SqlCommand(sql, _regisRevoCon))
            {
                
                return Convert.ToDateTime(cmd.ExecuteScalar().ToString());
            }
        }
        public DataTable GetRegRevoDT()
        {
            var sql = @"select * from regisrevodt where 1=0";
          using (var dt = new DataTable())
            {
                using (var cmd = new SqlCommand(sql, _regisRevoCon))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        public DataTable GetRegRevoReasonsDT()
        {
            var sql = @"select * from tblReasons order by Id Desc";
            using (var dt = new DataTable())
            {
                using (var cmd = new SqlCommand(sql, _regisRevoCon))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        public DataTable GetCompareRsltAsDt()
        {
            var sql = @"sp_regrev_get_results_DT_v1";
            using (var dt = new DataTable())
            {
                using (var cmd = new SqlCommand(sql, _regisRevoCon))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
        }
        public void BlkInsertRegREVDt(DataTable dt)
        {
            using (var sqlBulk = new SqlBulkCopy(_regisRevoCon))
            {
                sqlBulk.DestinationTableName = "RegisRevoDt";
                sqlBulk.WriteToServer(dt);
            }
        }

        public void BlkInsertTB(DataTable dt)
        {
            using (var sqlBulk = new SqlBulkCopy(_regisRevoCon2))
            {
                sqlBulk.DestinationTableName = "tbl_tb_rpt";
                sqlBulk.WriteToServer(dt);
            }
        }
        public DataTable GetTBDT1()
        {
            var sql = @"select * from tbl_tb_rpt where 1=0";
            using (var dt = new DataTable())
            {
                using (var cmd = new SqlCommand(sql, _regisRevoCon2))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
        }
        public DataTable GetExcludedData()
        {
            var sql = @"select * from RegRevComp_exclude";
            using (var dt = new DataTable())
            {
                using (var cmd = new SqlCommand(sql, _regisRevoCon))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
        }
        public DataTable GetTB_DT(int period,string legalENt)
        {
            var sql = @"sp_run_tb";
            using (var dt = new DataTable())
            {
                using (var cmd = new SqlCommand(sql, _regisRevoCon2))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 900;
                    cmd.Parameters.Add(new SqlParameter("@period", period));
                    cmd.Parameters.Add(new SqlParameter("@le", legalENt));
                    using (var da = new SqlDataAdapter(cmd))
                    {

                        da.Fill(dt);
                        return dt;
                    }
                }
            }
        }
        public int ExcCompSP(string datafile)
        {
            var sql = @"sp_regis_revo_comp_no_view_DT_v1";
            using (var dt = new DataTable())
            {
                using (var cmd = new SqlCommand(sql, _regisRevoCon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout =500;
                    cmd.Parameters.Add(new SqlParameter("@datafile", datafile));
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public List<EntityCntStatus> GetUYCount(string rptType,string ent)
        {
            var items = new List<EntityCntStatus>();
            var sql = @"sp_regrev_get_uy_DT_v1";
           
            using (var cmd = new SqlCommand(sql, _regisRevoCon))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@rpt_type", rptType));
                cmd.Parameters.Add(new SqlParameter("@ent", ent != "" || ent != string.Empty ? "'" + ent + "'" : null));
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        items.Add(new EntityCntStatus
                        {
                          
                            ChkBox = rdr["chkbox"].ToString(),
                            Count =Convert.ToInt32(rdr["cnt"]),
                            UY = rdr["cont_uy"].ToString(),
                        });
                    }
                }
            }
            return items;
        }

        public int InsReasons( string reason)
        {
            
              string  sql = @"insert into  tblReasons(reason)
            values (@reason)";
           


            using (var cmd = new SqlCommand(sql, _regisRevoCon))
            {
                cmd.CommandType = CommandType.Text;                
                cmd.Parameters.Add(new SqlParameter("@reason", reason));
                return cmd.ExecuteNonQuery();

            }
            return 0;
        }

        public static T? ConvertReader<T>(object dbValue)
            where T : struct
        {
            if (dbValue == null || dbValue is System.DBNull || string.IsNullOrEmpty(dbValue.ToString())) return null;

            T value = (T)Convert.ChangeType(dbValue, typeof(T)); ;

            return value;

        }

        public List<EntityCnt> GetEntityCount(string rptType,string reluw,string uw)
        {
            var items = new List<EntityCnt>();
            var sql = @"sp_regrev_get_ent_DT_v2";

            using (var cmd = new SqlCommand(sql, _regisRevoCon))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                
                cmd.Parameters.Add(new SqlParameter("@rpt_type", rptType));
                cmd.Parameters.Add(new SqlParameter("@reluw", reluw != "" || reluw != string.Empty ?   reluw  : null));
                cmd.Parameters.Add(new SqlParameter("@uw", uw != "" || uw != string.Empty ?  uw  : null));
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        items.Add(new EntityCnt
                        {
                            EntityName = rdr["legal_ent_code"].ToString(),
                            Cnt =rdr["cnt"].ToString()
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
                            Count = Convert.ToInt32(rdr["cntpgm"]),
                            Status = rdr["pgm_program"].ToString()
                           
                        });
                    }
                }
            }
            return items;
        }

        public List<EntityCntStatus> GetUWCount(string rptType,string ent,string UY)
        {
            var items = new List<EntityCntStatus>();
            var sql = @"[sp_regrev_get_uw_DT_v2]";

            using (var cmd = new SqlCommand(sql, _regisRevoCon))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@rpt_type", rptType));
                //cmd.Parameters.Add(new SqlParameter("@ent", ent != "" || ent != string.Empty ? "'" + ent + "'" : null));
                cmd.Parameters.Add(new SqlParameter("@reluw", UY != "" || UY != string.Empty ? UY  : null));
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        items.Add(new EntityCntStatus
                        {
                            EntityName = rdr["uw_fullname"].ToString(),
                            ChkBox = rdr["chkbox"].ToString(),
                            Count = Convert.ToInt32(rdr["cnt"])


                        });
                    }
                }
            }
            return items;
        }

        public List<EntityCntStatus> GetRelUWCount(string rptType, string ent, string UY)
        {
            var items = new List<EntityCntStatus>();
            var sql = @"[sp_regrev_get_reluw_DT_v1]";

            using (var cmd = new SqlCommand(sql, _regisRevoCon))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@rpt_type", rptType));
                //cmd.Parameters.Add(new SqlParameter("@ent", ent != "" || ent != string.Empty ? "'" + ent + "'" : null));
                //cmd.Parameters.Add(new SqlParameter("@uy", UY));
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        items.Add(new EntityCntStatus
                        {
                            EntityName = rdr["Rel_UW_Fullname"].ToString(),
                            ChkBox = rdr["chkbox"].ToString(),
                            Count = Convert.ToInt32(rdr["cnt"])


                        });
                    }
                }
            }
            return items;
        }


        public List<EntityCntStatus> GetStatusCount(string rptType, string ent, string UY,string UW)
        {
            var items = new List<EntityCntStatus>();
            var sql = @"[sp_regrev_get_QYear_DT_v2]";

            using (var cmd = new SqlCommand(sql, _regisRevoCon))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@rpt_type", rptType));
                cmd.Parameters.Add(new SqlParameter("@ent", ent != "" || ent != string.Empty ? ent : null));
                cmd.Parameters.Add(new SqlParameter("@uw", UW != "" || UW != string.Empty ? UW : null));
                cmd.Parameters.Add(new SqlParameter("@reluw", UY != "" || UY != string.Empty ? UY : null));
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        items.Add(new EntityCntStatus
                        {
                            Status = rdr["bounddate"].ToString(),
                            ChkBox = rdr["chkbox"].ToString(),
                            Count = Convert.ToInt32(rdr["cnt"])


                        });
                    }
                }
            }
            return items;
        }

        public List<EntityCntStatus> GetFieldCount(string rptType,string ent,string UW,string UY,string Program,string status)
        {
            var items = new List<EntityCntStatus>();
            var sql = @"[sp_regrev_get_comp_DT_v2]";

            using (var cmd = new SqlCommand(sql, _regisRevoCon))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@rpt_type", rptType));
                cmd.Parameters.Add(new SqlParameter("@ent", ent != "" || ent != string.Empty ?  ent  : null));
                cmd.Parameters.Add(new SqlParameter("@uw", UW != "" || UW != string.Empty ?  UW  : null));
                cmd.Parameters.Add(new SqlParameter("@reluw", UY != "" || UY != string.Empty ? UY  : null));
                cmd.Parameters.Add(new SqlParameter("@year", Program != "" || Program != string.Empty ? Program : null));
                cmd.Parameters.Add(new SqlParameter("@quarter", status != "" || status != string.Empty ? status : null));
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        items.Add(new EntityCntStatus
                        {
                            EntityName = rdr["field_desc_name"].ToString(),
                            ChkBox = rdr["chkbox"].ToString(),
                            Count = Convert.ToInt32(rdr["cntcomp"]),


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
                            PlatformId = rdr["uw_platform_id"].ToString(),
                            Reason = rdr["Reason"].ToString()


                        });
                    }
                }
            }
            return items;
        }
        public int PutExcludeField(string platformId,string rpt_col,string reason,string Usernme,string type)
        {
            var items = new List<ExcludedFields>();
            var sql = @"";
            if(type=="Insert")
                sql=   @"insert into  RegRevComp_exclude(uw_platform_id,rpt_col,who_excluded,date_excluded,reason)
            values (@p_id,@rpt_col,@windUser,getdate(),@reason)";
            else if (type == "Delete")
                sql = @"delete from RegRevComp_exclude where uw_platform_id=@p_id
and rpt_col=@rpt_col";


            using (var cmd = new SqlCommand(sql, _regisRevoCon))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@p_id", platformId));
                cmd.Parameters.Add(new SqlParameter("@rpt_col", rpt_col));
                cmd.Parameters.Add(new SqlParameter("@reason", reason));
                cmd.Parameters.Add(new SqlParameter("@windUser", Usernme));
                return cmd.ExecuteNonQuery();
                
            }
            return 0;
        }

        public DataTable GetCompareRsltAsDt(string rptType, string program, string entity, string uy, string uw, string field, string status)
        {
            var sql = @"sp_regrev_get_results_DT_v1";
            using (var dt = new DataTable())
            {
                using (var cmd = new SqlCommand(sql, _regisRevoCon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@rpt_type", SqlDbType.NVarChar).Value = rptType;
                    cmd.Parameters.Add("@ent", SqlDbType.NVarChar).Value = (entity != "" ? "'" + entity + "'" : null);
                    cmd.Parameters.Add("@status", SqlDbType.NVarChar).Value = (status != "" ? "'" + status + "'" : null);
                    cmd.Parameters.Add("@uy", SqlDbType.NVarChar).Value = (uy != "" ? uy : null);
                    cmd.Parameters.Add("@uw", SqlDbType.NVarChar).Value = (uw != "" ? "'" + uw + "'" : null);
                    cmd.Parameters.Add("@program", SqlDbType.NVarChar).Value = (program != "" ? program : null);
                    cmd.Parameters.Add("@reason", SqlDbType.NVarChar).Value = (field != "" ? "'" + field.TrimEnd() + "'" : null);
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        public  DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public List<CompareResult> GetCompareResult(string rptType, string program,string entity, string uy, string uw, string field, string status)
        {
            var items = new List<CompareResult>();
            var sql = @"[sp_regrev_get_results_DT_v2]";

            using (var cmd = new SqlCommand(sql, _regisRevoCon))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                
                cmd.Parameters.Add("@rpt_type", SqlDbType.NVarChar).Value = rptType;
                cmd.Parameters.Add("@ent", SqlDbType.NVarChar).Value = (entity != "" ?   entity  : null);
                cmd.Parameters.Add("@year", SqlDbType.NVarChar).Value = (program != "" ?   program    : null);
                cmd.Parameters.Add("@reluw", SqlDbType.NVarChar).Value = (uy != "" ?   uy : null);
                cmd.Parameters.Add("@uw", SqlDbType.NVarChar).Value = (uw != "" ?   uw : null);
                cmd.Parameters.Add("@quarter", SqlDbType.NVarChar).Value =( status != "" ?    status :null);
                cmd.Parameters.Add("@reason", SqlDbType.NVarChar).Value = (field != "" ? field : null);
                //DataSet ds = new DataSet();
                //SqlDataAdapter sAdap = new SqlDataAdapter(cmd);
                //sAdap.Fill(ds);

                var reu = cmd.ExecuteScalar();

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


        public DataTable GetResultWithExcludedData(string rptType, string program, string entity, string uy, string uw, string field, string status)
        {
           
            var sql = @"[sp_regrev_get_ExcludedData_V1]";

            using (var cmd = new SqlCommand(sql, _regisRevoCon))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@rpt_type", SqlDbType.NVarChar).Value = rptType;
                cmd.Parameters.Add("@ent", SqlDbType.NVarChar).Value = (entity != "" ? entity : null);
                cmd.Parameters.Add("@year", SqlDbType.NVarChar).Value = (program != "" ? program : null);
                cmd.Parameters.Add("@reluw", SqlDbType.NVarChar).Value = (uy != "" ? uy : null);
                cmd.Parameters.Add("@uw", SqlDbType.NVarChar).Value = (uw != "" ? uw : null);
                cmd.Parameters.Add("@quarter", SqlDbType.NVarChar).Value = (status != "" ? status : null);
                cmd.Parameters.Add("@reason", SqlDbType.NVarChar).Value = (field != "" ? field : null);
                //DataSet ds = new DataSet();
                using (var da = new SqlDataAdapter(cmd))
                {
                    using (var dt = new DataTable())
                    {
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
             
        }

        public List<CompareResult> GetCompareResultWithFields(string rptType, string program, string entity, string uy, string uw, string field, string status)
        {
            var items = new List<CompareResult>();
            var sql = @"[sp_regrev_get_results_DT_v2]";

            using (var cmd = new SqlCommand(sql, _regisRevoCon))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@rpt_type", SqlDbType.NVarChar).Value = rptType;
                cmd.Parameters.Add("@ent", SqlDbType.NVarChar).Value = (entity != "" ? "'" + entity + "'" : null);
                cmd.Parameters.Add("@status", SqlDbType.NVarChar).Value = (status != "" ? "'" + status + "'" : null);
                cmd.Parameters.Add("@reluw", SqlDbType.NVarChar).Value = (uy != "" ? "'" + uy + "'" : null);
                cmd.Parameters.Add("@uw", SqlDbType.NVarChar).Value = (uw != "" ? "'" + uw + "'" : null);
                cmd.Parameters.Add("@program", SqlDbType.NVarChar).Value = (program != "" ? program : null);
                cmd.Parameters.Add("@reason", SqlDbType.NVarChar).Value = (field != "" ? field  : null);
                DataSet ds = new DataSet();
                SqlDataAdapter sAdap = new SqlDataAdapter(cmd);
                sAdap.Fill(ds);

                var reu = cmd.ExecuteScalar();

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