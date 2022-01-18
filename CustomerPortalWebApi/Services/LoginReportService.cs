using CustomerPortalWebApi.Entities;
using CustomerPortalWebApi.Interface;
using CustomerPortalWebApi.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CustomerPortalWebApi.Services
{
    public class LoginReportService : ILoginReportService
    {
        private readonly IConfiguration _config;
        private readonly ICustomerPortalHelper _customerPortalHelper;
        private readonly ILogger _ILogger;

        public LoginReportService(ICustomerPortalHelper customerPortalHelper, ILogger ILoggerservice, IConfiguration config)
        {
            _config = config;
            _ILogger = ILoggerservice;
            _customerPortalHelper = customerPortalHelper;
        }

        public long LoginReportCount(string fromdate, string todate, string type, string Keyword)
        {
            var dbPara = new DynamicParameters();
            DateTime fdate = DateTime.ParseExact(fromdate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(todate, "dd-MM-yyyy", null);
            dbPara.Add("fromDate", Convert.ToDateTime(fdate).ToString("yyyy-MM-dd"), DbType.String);
            dbPara.Add("todate", Convert.ToDateTime(tdate).ToString("yyyy-MM-dd"), DbType.String);

            dbPara.Add("PageNo", 0, DbType.Int32);
            dbPara.Add("PageSize", 0, DbType.Int32);
            if (Keyword == "NoSearch")
            {
                dbPara.Add("Keyword", "", DbType.String);
            }
            else
            {
                dbPara.Add("Keyword", Keyword, DbType.String);
            }
            dbPara.Add("Type", type, DbType.String);
            var data = _customerPortalHelper.Get<Count>("[dbo].[uspViewLoginReport]", dbPara, commandType: CommandType.StoredProcedure); 
            if (data != null)
            {
                return data.ListCount;
            }
            else
            {
                return 0;
            }
        }

        public List<UserMaster> LoginReportDownload(string fromdate, string todate, string Keyword)
        {
            var dbPara = new DynamicParameters();
            DateTime fdate = DateTime.ParseExact(fromdate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(todate, "dd-MM-yyyy", null);
            dbPara.Add("fromDate", Convert.ToDateTime(fdate).ToString("yyyy-MM-dd"), DbType.String);
            dbPara.Add("todate", Convert.ToDateTime(tdate).ToString("yyyy-MM-dd"), DbType.String);

            dbPara.Add("PageNo", 0, DbType.Int32);
            dbPara.Add("PageSize", 0, DbType.Int32);
            if (Keyword == "NoSearch")
            {
                dbPara.Add("Keyword", "", DbType.String);
            }
            else
            {
                dbPara.Add("Keyword", Keyword, DbType.String);
            }
            dbPara.Add("Type", "Download", DbType.String);
            var data = _customerPortalHelper.GetAll<UserMaster>("[dbo].[uspViewLoginReport]", dbPara, commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public DataTable NewLoginReport(LoginReportFilterModel model)
        {
            DateTime fdate = DateTime.ParseExact(model.fromDate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(model.todate, "dd-MM-yyyy", null);
            DataTable dt = new DataTable();
            using (SqlConnection sqlConn = new SqlConnection(_config.GetConnectionString("DatabaseContext")))
            {
                using (SqlCommand sqlCmd = new SqlCommand("uspViewNewLoginReportSearch", sqlConn))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("fromDate", Convert.ToDateTime(fdate).ToString("yyyy-MM-dd"));
                    sqlCmd.Parameters.AddWithValue("todate", Convert.ToDateTime(tdate).ToString("yyyy-MM-dd"));
                    sqlCmd.Parameters.AddWithValue("UserType", model.UserType);
                    if (model.Zone == "NoSearch")
                    {
                        sqlCmd.Parameters.AddWithValue("Zone", "");
                    }
                    else
                    {
                        sqlCmd.Parameters.AddWithValue("Zone", model.Zone);
                    }
                    if (model.Region == "NoSearch")
                    {
                        sqlCmd.Parameters.AddWithValue("Region", "");
                    }
                    else
                    {
                        sqlCmd.Parameters.AddWithValue("Region", model.Region);
                    }
                    if (model.Branch == "NoSearch")
                    {
                        sqlCmd.Parameters.AddWithValue("Branch", "");
                    }
                    else
                    {
                        sqlCmd.Parameters.AddWithValue("Branch", model.Branch);
                    }
                    if (model.Territory == "NoSearch")
                    {
                        sqlCmd.Parameters.AddWithValue("Territory", "");
                    }
                    else
                    {
                        sqlCmd.Parameters.AddWithValue("Territory", model.Territory);
                    }
                    sqlCmd.Parameters.AddWithValue("Type", model.Type);
                    sqlCmd.Parameters.AddWithValue("Search", model.Search);
                    sqlConn.Open();
                    using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCmd))
                    {
                        sqlAdapter.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public List<UserMaster> LoginReport(string fromdate, string todate, int pageNo, int pageSize, string Keyword)
        {
            var dbPara = new DynamicParameters();
            DateTime fdate = DateTime.ParseExact(fromdate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(todate, "dd-MM-yyyy", null);
            dbPara.Add("fromDate", Convert.ToDateTime(fdate).ToString("yyyy-MM-dd"), DbType.String);
            dbPara.Add("todate", Convert.ToDateTime(tdate).ToString("yyyy-MM-dd"), DbType.String);

            dbPara.Add("PageNo", pageNo, DbType.Int32);
            dbPara.Add("PageSize", pageSize, DbType.Int32);
            if (Keyword == "NoSearch")
            {
                dbPara.Add("Keyword", "", DbType.String);
            }
            else
            {
                dbPara.Add("Keyword", Keyword, DbType.String);
            }
            dbPara.Add("Type", "List", DbType.String);
            var data = _customerPortalHelper.GetAll<UserMaster>("[dbo].[uspViewLoginReport]", dbPara, commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public List<SalesHierachy> GetArea(string Type, string Keyword)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("Type", Type, DbType.String);
            if (Keyword == "NoSearch")
            {
                dbPara.Add("Keyword", "", DbType.String);
            }
            else
            {
                dbPara.Add("Keyword", Keyword, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<SalesHierachy>("[dbo].[uspViewArea]", dbPara, commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public DataTable NewLoginReportDownload(LoginReportFilterModel model)
        {
            DateTime fdate = DateTime.ParseExact(model.fromDate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(model.todate, "dd-MM-yyyy", null);
            DataTable dt = new DataTable();
            using (SqlConnection sqlConn = new SqlConnection(_config.GetConnectionString("DatabaseContext")))
            {
                using (SqlCommand sqlCmd = new SqlCommand("uspViewNewLoginReportSearch", sqlConn))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("fromDate", Convert.ToDateTime(fdate).ToString("yyyy-MM-dd"));
                    sqlCmd.Parameters.AddWithValue("todate", Convert.ToDateTime(tdate).ToString("yyyy-MM-dd"));
                    sqlCmd.Parameters.AddWithValue("UserType", model.UserType);
                    if (model.Zone == "NoSearch")
                    {
                        sqlCmd.Parameters.AddWithValue("Zone", "");
                    }
                    else
                    {
                        sqlCmd.Parameters.AddWithValue("Zone", model.Zone);
                    }
                    if (model.Region == "NoSearch")
                    {
                        sqlCmd.Parameters.AddWithValue("Region", "");
                    }
                    else
                    {
                        sqlCmd.Parameters.AddWithValue("Region", model.Region);
                    }
                    if (model.Branch == "NoSearch")
                    {
                        sqlCmd.Parameters.AddWithValue("Branch", "");
                    }
                    else
                    {
                        sqlCmd.Parameters.AddWithValue("Branch", model.Branch);
                    }
                    if (model.Territory == "NoSearch")
                    {
                        sqlCmd.Parameters.AddWithValue("Territory", "");
                    }
                    else
                    {
                        sqlCmd.Parameters.AddWithValue("Territory", model.Territory);
                    }
                    sqlCmd.Parameters.AddWithValue("Type", model.Type);
                    sqlCmd.Parameters.AddWithValue("Search", model.Search);
                    sqlConn.Open();
                    using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCmd))
                    {
                        sqlAdapter.Fill(dt);
                    }
                }
            }
            return dt;
        }
    }
}