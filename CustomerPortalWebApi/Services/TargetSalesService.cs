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
    public class TargetSalesService : ITargetSalesService
    {
        private readonly ICustomerPortalHelper _customerPortalHelper;
        private readonly IConfiguration _config;
        public TargetSalesService(ICustomerPortalHelper customerPortalHelper, IConfiguration config)
        {
            _customerPortalHelper = customerPortalHelper;
            _config = config;
        }

        public long InsertTargetSales(TargetSalesModel targetsales)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("Divisionvtxt", targetsales.Divisionvtxt, DbType.String);
            dbPara.Add("Monthvtxt", targetsales.Monthvtxt, DbType.String);
            dbPara.Add("Yearvtxt", targetsales.Yearvtxt, DbType.Int16);
            dbPara.Add("CustomerCodevtxt", targetsales.CustomerCodevtxt, DbType.String);
            dbPara.Add("CustomerNamevtxt", targetsales.CustomerNamevtxt, DbType.String);
            dbPara.Add("TargetSalesdcl", targetsales.TargetSalesdcl, DbType.Decimal);
            dbPara.Add("CreatedByvtxt", targetsales.CreatedByvtxt, DbType.String);

            #region using dapper

            var data = _customerPortalHelper.Insert<long>("[dbo].[uspInsertTargetSales]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;

            #endregion using dapper
        }

        public List<TargetSalesModel> GetTargetSales(string division, int PageNo, int PageSize, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("Division", division, DbType.String);
            dbPara.Add("PageNo", PageNo, DbType.Int32);
            dbPara.Add("PageSize", PageSize, DbType.Int32);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<TargetSalesModel>("[dbo].[uspviewGetTargetSalesData]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public List<TargetSalesModel> GetTargetSalesForDashboard(string customercode, string date)
        {
            var dbPara = new DynamicParameters();
            DateTime tdate = DateTime.ParseExact(date, "dd-MM-yyyy", null);
            dbPara.Add("customercode", customercode, DbType.String);
            dbPara.Add("date", Convert.ToDateTime(tdate).ToString("yyyy-MM-dd"), DbType.DateTime);
            var data = _customerPortalHelper.GetAll<TargetSalesModel>("[dbo].[uspviewGetCustomerTargetSalesByMonthly]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public List<TargetSalesModel> GetKAMTargetSalesForDashboard(string usercode, string date)
        {
            var dbPara = new DynamicParameters();
            DateTime tdate = DateTime.ParseExact(date, "dd-MM-yyyy", null);
            dbPara.Add("usercode", usercode, DbType.String);
            dbPara.Add("date", Convert.ToDateTime(tdate).ToString("yyyy-MM-dd"), DbType.DateTime);
            var data = _customerPortalHelper.GetAll<TargetSalesModel>("[dbo].[uspviewGetKAMTargetSalesByMonthly]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public List<TargetSalesModel> GetTargetSalesForDashboardByFinance(string customercode, string date)
        {
            var dbPara = new DynamicParameters();
            DateTime tdate = DateTime.ParseExact(date, "dd-MM-yyyy", null);
            dbPara.Add("customercode", customercode, DbType.String);
            dbPara.Add("date", Convert.ToDateTime(tdate).ToString("yyyy-MM-dd"), DbType.DateTime);
            var data = _customerPortalHelper.GetAll<TargetSalesModel>("[dbo].[uspviewGetCustomerTargetSalesByYearly]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public List<TargetSalesModel> DownloadTargetSales(string division, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("Division", division, DbType.String);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<TargetSalesModel>("[dbo].[uspviewDownloadTargetSalesData]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public long GetTargetSalesListCount(string division, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("Division", division, DbType.String);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<TargetSalesModel>("[dbo].[uspviewDownloadTargetSalesData]",
                            dbPara,
                            commandType: CommandType.StoredProcedure); 
            if (data != null)
            {
                return Convert.ToInt64(data.ToList().Count);
            }
            else
            {
                return 0;
            }
        }


        public long InsertCustomerSales(CustomerSalesModel targetsales)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("Typevtxt", targetsales.Typevtxt, DbType.String);
            dbPara.Add("Monthvtxt", targetsales.Monthvtxt, DbType.String);
            dbPara.Add("Yearvtxt", targetsales.Yearvtxt, DbType.String);
            dbPara.Add("CustomerCodevtxt", targetsales.CustomerCodevtxt, DbType.String);
            dbPara.Add("CustomerNamevtxt", targetsales.CustomerNamevtxt, DbType.String);
            dbPara.Add("Sales", targetsales.Salesdcl, DbType.Decimal);
            dbPara.Add("CreatedByvtxt", targetsales.CreatedByvtxt, DbType.String);

            #region using dapper

            var data = _customerPortalHelper.Insert<long>("[dbo].[uspInsertCustomerSales]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;

            #endregion using dapper
        }


        public List<CustomerSalesModel> GetCustomerSalesData(string Type, int PageNo, int PageSize, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("Type", Type, DbType.String);
            dbPara.Add("PageNo", PageNo, DbType.Int32);
            dbPara.Add("PageSize", PageSize, DbType.Int32);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<CustomerSalesModel>("[dbo].[uspviewGetCustomerSalesData]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }


        public List<CustomerSalesModel> DownloadCustomerSales(string Type, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("Type", Type, DbType.String);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<CustomerSalesModel>("[dbo].[uspviewDownloadCustomerSalesData]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public long GetCustomerSalesListCount(string division, string KeyWord)
        {
            var dbPara = new DynamicParameters(); 
            dbPara.Add("Type", division, DbType.String);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<CustomerSalesModel>("[dbo].[uspviewDownloadCustomerSalesData]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            if (data != null)
            {
                return Convert.ToInt64(data.ToList().Count);
            }
            else
            {
                return 0;
            }
        }

        public List<CustomerSalesModel> GetCustomerProfileTranscationDataTargetSales(string customercode, string date)
        {
            var dbPara = new DynamicParameters();
            DateTime tdate = DateTime.ParseExact(date, "dd-MM-yyyy", null);
            dbPara.Add("customercode", customercode, DbType.String);
            dbPara.Add("Mode", "TargetSales", DbType.String);
            var data = _customerPortalHelper.GetAll<CustomerSalesModel>("[dbo].[uspGetCustomerTranscDataforProfile]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public List<CustomerSalesModel> GetCustomerProfileTranscationDataNCROrPayment(string customercode, string date,string Mode)
        {
            var dbPara = new DynamicParameters(); 
            dbPara.Add("customercode", customercode, DbType.String);
            dbPara.Add("Mode", Mode, DbType.String);
            var data = _customerPortalHelper.GetAll<CustomerSalesModel>("[dbo].[uspGetCustomerTranscDataforProfile]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public CustomerSalesModel GetCustomerProfileTranscationSalesHistory(string customercode, string date, string Mode)
        {
            var dbPara = new DynamicParameters();
            DateTime tdate = DateTime.ParseExact(date, "dd-MM-yyyy", null);
            dbPara.Add("customercode", customercode, DbType.String);
            dbPara.Add("Mode", Mode, DbType.String);
            var data = _customerPortalHelper.GetAll<CustomerSalesModel>("[dbo].[uspGetCustomerTranscDataforProfile]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data[0];
            }
            else
            {
                return null;
            }
        }

        public CustomerSalesModel GetCustomerProfileConsistency(string customercode, string date)
        {
            var dbPara = new DynamicParameters();
            DateTime tdate = DateTime.ParseExact(date, "dd-MM-yyyy", null);
            dbPara.Add("customercode", customercode, DbType.String);
            dbPara.Add("Mode", "Consistency", DbType.String);
            var data = _customerPortalHelper.GetAll<CustomerSalesModel>("[dbo].[uspGetCustomerTranscDataforProfile]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data[0];
            }
            else
            {
                return null;
            }
        }

        public CustomerSalesModel GetCustomerProfileEffective(string customercode, string date)
        {
            var dbPara = new DynamicParameters();
            DateTime tdate = DateTime.ParseExact(date, "dd-MM-yyyy", null);
            dbPara.Add("customercode", customercode, DbType.String);
            dbPara.Add("Mode", "EffectiveMonths", DbType.String);
            var data = _customerPortalHelper.GetAll<CustomerSalesModel>("[dbo].[uspGetCustomerTranscDataforProfile]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data[0];
            }
            else
            {
                return null;
            }
        }





        public DataTable GetSalesBreakUp(string customercode, string date)
        {
           
            DataTable dt = new DataTable();
            using (SqlConnection sqlConn = new SqlConnection(_config.GetConnectionString("DatabaseContext")))
            {
                using (SqlCommand sqlCmd = new SqlCommand("uspGetCustomerTranscDataforProfile", sqlConn))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("customercode", customercode);
                    sqlCmd.Parameters.AddWithValue("Mode", "SalesBreakUp");
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