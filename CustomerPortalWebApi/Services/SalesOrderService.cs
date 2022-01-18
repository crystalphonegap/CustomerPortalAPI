using CustomerPortalWebApi.Entities;
using CustomerPortalWebApi.Interface;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CustomerPortalWebApi.Services
{
    public class SalesOrderService : ISalesOrderService
    {
        private readonly ICustomerPortalHelper _customerPortalHelper;

        public SalesOrderService(ICustomerPortalHelper customerPortalHelper)
        {
            _customerPortalHelper = customerPortalHelper;
        }

        public List<SalesOrder> GetSalesOrder(string SoldToPartyCode, int pageNo, int pageSize, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("SoldToPartyCodevtxt", SoldToPartyCode, DbType.String);
            dbPara.Add("PageNo", pageNo, DbType.Int32);
            dbPara.Add("PageSize", pageSize, DbType.Int32);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<SalesOrder>("[dbo].[uspviewGetSalesOrderBySoldToPartyCode]", dbPara, commandType: CommandType.StoredProcedure);
            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public List<SalesOrder> GetSalesOrderStatuswiseCount(string fromdate, string todate, string SoldToPartyCode, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            DateTime fdate = DateTime.ParseExact(fromdate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(todate, "dd-MM-yyyy", null);
            dbPara.Add("fromdate", Convert.ToDateTime(fdate).ToString("yyyy-MM-dd"), DbType.DateTime);
            dbPara.Add("todate", Convert.ToDateTime(tdate).ToString("yyyy-MM-dd"), DbType.DateTime);
            dbPara.Add("partycode", SoldToPartyCode, DbType.String);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<SalesOrder>("[dbo].[uspviewGetSalesOrderStatuswiseCount]", dbPara, commandType: CommandType.StoredProcedure);
            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public List<SalesOrder> GetSalesOrderSearch(string fromdate, string todate, string status, string SoldToPartyCode, int pageNo, int pageSize, string KeyWord)
        {
            DateTime fdate = DateTime.ParseExact(fromdate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(todate, "dd-MM-yyyy", null);
            var dbPara = new DynamicParameters();
            dbPara.Add("fromdate", Convert.ToDateTime(fdate).ToString("yyyy-MM-dd"), DbType.DateTime);
            dbPara.Add("todate", Convert.ToDateTime(tdate).ToString("yyyy-MM-dd"), DbType.DateTime);
            if (status == "All")
            {
                dbPara.Add("status", "", DbType.String);
            }
            else
            {
                dbPara.Add("status", status, DbType.String);
            }
            dbPara.Add("SoldToPartyCodevtxt", SoldToPartyCode, DbType.String);
            dbPara.Add("PageNo", pageNo, DbType.Int32);
            dbPara.Add("PageSize", pageSize, DbType.Int32);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<SalesOrder>("[dbo].[uspviewGetSalesOrderSearch]", dbPara, commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public List<SalesOrder> GetSalesOrderDownload(string fromdate, string todate, string status, string SoldToPartyCode, string KeyWord)
        {
            DateTime fdate = DateTime.ParseExact(fromdate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(todate, "dd-MM-yyyy", null);
            var dbPara = new DynamicParameters();
            dbPara.Add("fromdate", Convert.ToDateTime(fdate).ToString("yyyy-MM-dd"), DbType.DateTime);
            dbPara.Add("todate", Convert.ToDateTime(tdate).ToString("yyyy-MM-dd"), DbType.DateTime);
            if (status == "All")
            {
                dbPara.Add("status", "", DbType.String);
            }
            else
            {
                dbPara.Add("status", status, DbType.String);
            }
            dbPara.Add("SoldToPartyCodevtxt", SoldToPartyCode, DbType.String);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<SalesOrder>("[dbo].[uspviewGetSalesOrderDownload]", dbPara, commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public long GetSalesOrderSearchCount(string fromdate, string todate, string status, string SoldToPartyCode, string KeyWord)
        {
            DateTime fdate = DateTime.ParseExact(fromdate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(todate, "dd-MM-yyyy", null);
            var dbPara = new DynamicParameters();
            dbPara.Add("fromdate", Convert.ToDateTime(fdate).ToString("yyyy-MM-dd"), DbType.DateTime);
            dbPara.Add("todate", Convert.ToDateTime(tdate).ToString("yyyy-MM-dd"), DbType.DateTime);
            if (status == "All")
            {
                dbPara.Add("status", "", DbType.String);
            }
            else
            {
                dbPara.Add("status", status, DbType.String);
            }
            dbPara.Add("SoldToPartyCodevtxt", SoldToPartyCode, DbType.String);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<SalesOrder>("[dbo].[uspviewGetSalesOrderDownload]", dbPara, commandType: CommandType.StoredProcedure); 
            if (data != null)
            {
                return Convert.ToInt64(data.ToList().Count);
            }
            else
            {
                return 0;
            }
        }

        public List<SalesOrder> getAllSalesOrderDataByOrderNo(string Orderno)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("Orderno", Orderno, DbType.String);
            var data = _customerPortalHelper.GetAll<SalesOrder>("[dbo].[uspviewGetSalesOrderDetailsBySalesOrderNo]", dbPara, commandType: CommandType.StoredProcedure);
            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public List<SalesOrder> getSalesOrderHeaderDataByOrderNo(string Orderno)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("Orderno", Orderno, DbType.String);
            var data = _customerPortalHelper.GetAll<SalesOrder>("[dbo].[uspviewGetSalesOrderByOrderNo]", dbPara, commandType: CommandType.StoredProcedure);
            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public long GetSalesCount(string SoldToPartyCode, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("SoldToPartyCodevtxt", SoldToPartyCode, DbType.String);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<Count>("[dbo].[uspviewgetAllSalesOrderCount]", dbPara, commandType: CommandType.StoredProcedure); 
            if (data != null)
            {
                return Convert.ToInt64(data.ToList().Count);
            }
            else
            {
                return 0;
            }
        }

        public List<SalesOrder> GetBlockedSalesOrderSearch(string usercode, string usertype, string fromdate, string todate, int pageNo, int pageSize, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            DateTime fdate = DateTime.ParseExact(fromdate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(todate, "dd-MM-yyyy", null);
            dbPara.Add("usercode", usercode, DbType.String);
            dbPara.Add("usertype", usertype, DbType.String);
            dbPara.Add("fromdate", Convert.ToDateTime(fdate).ToString("yyyy-MM-dd"), DbType.DateTime);
            dbPara.Add("todate", Convert.ToDateTime(tdate).ToString("yyyy-MM-dd"), DbType.DateTime);
            dbPara.Add("PageNo", pageNo, DbType.Int32);
            dbPara.Add("PageSize", pageSize, DbType.Int32);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<SalesOrder>("[dbo].[uspviewGetBlockedSalesOrderSearch]", dbPara, commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public List<SalesOrder> GetBlockedSalesOrderDownload(string usercode, string usertype, string fromdate, string todate, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            DateTime fdate = DateTime.ParseExact(fromdate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(todate, "dd-MM-yyyy", null);
            dbPara.Add("usercode", usercode, DbType.String);
            dbPara.Add("usertype", usertype, DbType.String);
            dbPara.Add("fromdate", Convert.ToDateTime(fdate).ToString("yyyy-MM-dd"), DbType.DateTime);
            dbPara.Add("todate", Convert.ToDateTime(tdate).ToString("yyyy-MM-dd"), DbType.DateTime);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<SalesOrder>("[dbo].[uspviewGetBlockedSalesOrderDownload]", dbPara, commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public long GetBlockedSalesOrderountC(string usercode, string usertype, string fromdate, string todate, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            DateTime fdate = DateTime.ParseExact(fromdate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(todate, "dd-MM-yyyy", null);
            dbPara.Add("usercode", usercode, DbType.String);
            dbPara.Add("usertype", usertype, DbType.String);
            dbPara.Add("fromdate", Convert.ToDateTime(fdate).ToString("yyyy-MM-dd"), DbType.DateTime);
            dbPara.Add("todate", Convert.ToDateTime(tdate).ToString("yyyy-MM-dd"), DbType.DateTime);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<SalesOrder>("[dbo].[uspviewGetBlockedSalesOrderDownload]", dbPara, commandType: CommandType.StoredProcedure); 
            if (data != null)
            {
                return data.ToList().Count;
            }
            else
            {
                return 0;
            }
        }
    }
}