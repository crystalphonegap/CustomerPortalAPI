using CustomerPortalWebApi.Interface;
using CustomerPortalWebApi.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CustomerPortalWebApi.Services
{
    public class CFAgentServices : ICFAgentServices
    {
        private readonly ICustomerPortalHelper _customerPortalHelper;

        public CFAgentServices(ICustomerPortalHelper customerPortalHelper)
        {
            _customerPortalHelper = customerPortalHelper;
        }

        public List<CFAgentDashboardCountModels> GetCFAgentDashboardCounts(string usercode)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("usercode", usercode, DbType.String);
            var data = _customerPortalHelper.GetAll<CFAgentDashboardCountModels>("[dbo].[uspviewCFagentDashboardcount]",
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

        public List<OrderHeaderModel> GetAllOrderList(string fromdate, string todate, string status, string usertype, string UserCode, int PageNo, int PageSize, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            DateTime fdate = DateTime.ParseExact(fromdate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(todate, "dd-MM-yyyy", null);

            dbPara.Add("fromDate", Convert.ToDateTime(fdate).ToString("yyyy-MM-dd"), DbType.String);
            dbPara.Add("todate", Convert.ToDateTime(tdate).ToString("yyyy-MM-dd"), DbType.String);

            dbPara.Add("Status", status, DbType.String);
            dbPara.Add("UserType", usertype, DbType.String);
            dbPara.Add("UserCode", UserCode, DbType.String);
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
            var data = _customerPortalHelper.GetAll<OrderHeaderModel>("[dbo].[uspviewGetOrderListByCFCode]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public List<OrderHeaderModel> GetAllOrderDownload(string fromdate, string todate, string status, string usertype, string UserCode, string KeyWord)
        {
            var dbPara = new DynamicParameters();

            DateTime fdate = DateTime.ParseExact(fromdate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(todate, "dd-MM-yyyy", null);

            dbPara.Add("fromDate", Convert.ToDateTime(fdate).ToString("yyyy-MM-dd"), DbType.String);
            dbPara.Add("todate", Convert.ToDateTime(tdate).ToString("yyyy-MM-dd"), DbType.String);

            dbPara.Add("Status", status, DbType.String);
            dbPara.Add("UserType", usertype, DbType.String);
            dbPara.Add("UserCode", UserCode, DbType.String);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<OrderHeaderModel>("[dbo].[uspviewGetOrderListByCFCodeDownload]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public long GetAllOrderCount(string fromdate, string todate, string status, string usertype, string UserCode, string KeyWord)
        {
            var dbPara = new DynamicParameters();

            DateTime fdate = DateTime.ParseExact(fromdate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(todate, "dd-MM-yyyy", null);

            dbPara.Add("fromDate", Convert.ToDateTime(fdate).ToString("yyyy-MM-dd"), DbType.String);
            dbPara.Add("todate", Convert.ToDateTime(tdate).ToString("yyyy-MM-dd"), DbType.String);

            dbPara.Add("Status", status, DbType.String);
            dbPara.Add("UserType", usertype, DbType.String);
            dbPara.Add("UserCode", UserCode, DbType.String);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<OrderHeaderModel>("[dbo].[uspviewGetOrderListByCFCodeDownload]",
                            dbPara,
                            commandType: CommandType.StoredProcedure); 
            if (data != null)
            {
                return data.ToList().Count;
            }
            else
            {
                return 0;
            }
        }

        public List<SPCFALedger> GetSPCFALedger(string UserCode, int PageNo, int PageSize)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("UserCodevtxt", UserCode, DbType.String);
            dbPara.Add("PageNo", PageNo, DbType.Int32);
            dbPara.Add("PageSize", PageSize, DbType.Int32);
            var data = _customerPortalHelper.GetAll<SPCFALedger>("[dbo].[uspviewSPCFALedger]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public long GetSPCFALedgerCount(string UserCode)
        {
            var dbPara = new DynamicParameters();

            //dbPara.Add("fromDate", Convert.ToDateTime(fromdate).ToString("yyyy-MM-dd"), DbType.DateTime);
            //dbPara.Add("todate", Convert.ToDateTime(todate).ToString("yyyy-MM-dd"), DbType.DateTime);
            dbPara.Add("UserCodevtxt", UserCode, DbType.String);
            var data = _customerPortalHelper.GetAll<SPCFALedger>("[dbo].[uspviewSPCFALedgerCount]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            
            if (data != null)
            {
                return Convert.ToInt64(data.Count);
            }
            else
            {
                return 0;
            }
        }

        public List<SPCFALedger> GetSPCFALedgerExportToExcel(string UserCode)
        {
            var dbPara = new DynamicParameters();

            //dbPara.Add("fromDate", Convert.ToDateTime(fromdate).ToString("yyyy-MM-dd"), DbType.DateTime);
            //dbPara.Add("todate", Convert.ToDateTime(todate).ToString("yyyy-MM-dd"), DbType.DateTime);
            dbPara.Add("UserCodevtxt", UserCode, DbType.String);
            var data = _customerPortalHelper.GetAll<SPCFALedger>("[dbo].[uspviewSPCFALedgerCount]",
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
    }
}