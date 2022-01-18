using CustomerPortalWebApi.Entities;
using CustomerPortalWebApi.Interface;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CustomerPortalWebApi.Services
{
    public class DeliveryOrderService : IDeliveryOrderService
    {
        private readonly ICustomerPortalHelper _customerPortalHelper;

        public DeliveryOrderService(ICustomerPortalHelper customerPortalHelper)
        {
            _customerPortalHelper = customerPortalHelper;
        }

        public List<DeliveryOrder> GetDeliveryOrder(string SoldToPartyCode, int pageNo, int pageSize, string KeyWord)
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
            var data = _customerPortalHelper.GetAll<DeliveryOrder>("[dbo].[uspviewGetDeliveryOrderBySoldToPartyCode]", dbPara, commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public List<DeliveryOrder> GetDeliveryOrderStatusCount(string fromdate, string todate, string SoldToPartyCode, string KeyWord)
        {
            var dbPara = new DynamicParameters();

            DateTime fdate = DateTime.ParseExact(fromdate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(todate, "dd-MM-yyyy", null);
            dbPara.Add("fromdate", fdate, DbType.DateTime);
            dbPara.Add("todate", tdate, DbType.DateTime);
            dbPara.Add("partycode", SoldToPartyCode, DbType.String);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<DeliveryOrder>("[dbo].[uspviewGetDeliveryOrderStatuswiseCount]", dbPara, commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public List<DeliveryOrder> GetDeliveryOrderSearch(string fromdate, string todate, string status, string SoldToPartyCode, int pageNo, int pageSize, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            DateTime fdate = DateTime.ParseExact(fromdate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(todate, "dd-MM-yyyy", null);
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
            var data = _customerPortalHelper.GetAll<DeliveryOrder>("[dbo].[uspviewDeliveryOrderSearch]", dbPara, commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public List<DeliveryOrder> GetDeliveryOrderDownload(string fromdate, string todate, string status, string SoldToPartyCode, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            DateTime fdate = DateTime.ParseExact(fromdate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(todate, "dd-MM-yyyy", null);
            dbPara.Add("fromdate", fdate, DbType.DateTime);
            dbPara.Add("todate", tdate, DbType.DateTime);
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
            var data = _customerPortalHelper.GetAll<DeliveryOrder>("[dbo].[uspviewDeliveryOrderDownload]", dbPara, commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public List<DeliveryOrder> GetDeliveryOrderCount(string SoldToPartyCode, int pageNo, int pageSize, string KeyWord)
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
            var data = _customerPortalHelper.GetAll<DeliveryOrder>("[dbo].[uspviewGetDeliveryOrderBySoldToPartyCode]", dbPara, commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public List<DeliveryOrder> GetDeliveryOrderByOrderNo(string Orderno)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("Orderno", Orderno, DbType.String);
            var data = _customerPortalHelper.GetAll<DeliveryOrder>("[dbo].[uspviewGetDeliveryOrderByOrderNo]", dbPara, commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public List<DeliveryOrder> getAllDeliveryOrderDataBySalesOrderNo(string Orderno)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("SalesOrderNo", Orderno, DbType.String);
            var data = _customerPortalHelper.GetAll<DeliveryOrder>("[dbo].[uspviewGetDeliveryOrderBySalesOrderNo]", dbPara, commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public List<DeliveryOrder> GetDeliveryOrderHeaderByOrderNo(string Orderno)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("Orderno", Orderno, DbType.String);
            var data = _customerPortalHelper.GetAll<DeliveryOrder>("[dbo].[uspviewGetDeliveryOrderHeaderByOrderNo]", dbPara, commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public long GetDeliveryCount(string fromdate, string todate, string status, string SoldToPartyCode, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            DateTime fdate = DateTime.ParseExact(fromdate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(todate, "dd-MM-yyyy", null);
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
            var data = _customerPortalHelper.GetAll<Count>("[dbo].[uspviewgetAllDeliveryOrderCount]", dbPara, commandType: CommandType.StoredProcedure);
            if (data != null)
            {
                return Convert.ToInt64(data[0].ListCount);
            }
            else
            {
                return 0;
            }
        }

        public long GetDeliveryCount(string CustomerCode)
        {
            var dbPara = new DynamicParameters();

            dbPara.Add("SoldToPartyCodevtxt", CustomerCode, DbType.String);

            var data = _customerPortalHelper.GetAll<Count>("[dbo].[uspviewgetAllDeliveryOrderCountForCustomerDashboard]", dbPara, commandType: CommandType.StoredProcedure); 
            if (data != null)
            {
                return Convert.ToInt64(data[0].ListCount);
            }
            else
            {
                return 0;
            }
        }

        public long SetDeliveryStatus(DeliveryOrder model)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("OrderNo", model.DeliveryOrderNovtxt, DbType.String);
            dbPara.Add("Status", model.Status, DbType.String);
            if (model.Remark != "No Remark")
            {
                dbPara.Add("Remark", model.Remark, DbType.String);
            }
            else
            {
                dbPara.Add("Remark", "", DbType.String);
            }
            dbPara.Add("OrderRecivedDate", model.OrderRecivedDate, DbType.DateTime);

            var data = _customerPortalHelper.GetAll<Count>("[dbo].[uspUpdateDeliveryOrderStatus]", dbPara, commandType: CommandType.StoredProcedure); 
            if (data != null)
            {
                return Convert.ToInt64(data[0].ListCount);
            }
            else
            {
                return 0;
            }
        }

        public List<DeliveryOrder> GetShipToDeliveryOrderSearch(string fromdate, string todate, string status, string ShipToPartyCode, int pageNo, int pageSize, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            DateTime fdate = DateTime.ParseExact(fromdate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(todate, "dd-MM-yyyy", null);
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
            dbPara.Add("ShipToPartyCodevtxt", ShipToPartyCode, DbType.String);
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
            var data = _customerPortalHelper.GetAll<DeliveryOrder>("[dbo].[uspviewShipToDeliveryOrderSearch]", dbPara, commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public List<DeliveryOrder> GetShipToDeliveryOrderDownload(string fromdate, string todate, string status, string ShipToPartyCode, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            DateTime fdate = DateTime.ParseExact(fromdate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(todate, "dd-MM-yyyy", null);
            dbPara.Add("fromdate", fdate, DbType.DateTime);
            dbPara.Add("todate", tdate, DbType.DateTime);
            if (status == "All")
            {
                dbPara.Add("status", "", DbType.String);
            }
            else
            {
                dbPara.Add("status", status, DbType.String);
            }
            dbPara.Add("ShipToPartyCodevtxt", ShipToPartyCode, DbType.String);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<DeliveryOrder>("[dbo].[uspviewShipToDeliveryOrderDownload]", dbPara, commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public long GetShipToDeliveryOrderCount(string fromdate, string todate, string status, string ShipToPartyCode, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            DateTime fdate = DateTime.ParseExact(fromdate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(todate, "dd-MM-yyyy", null);
            dbPara.Add("fromdate", fdate, DbType.DateTime);
            dbPara.Add("todate", tdate, DbType.DateTime);
            if (status == "All")
            {
                dbPara.Add("status", "", DbType.String);
            }
            else
            {
                dbPara.Add("status", status, DbType.String);
            }
            dbPara.Add("ShipToPartyCodevtxt", ShipToPartyCode, DbType.String);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<DeliveryOrder>("[dbo].[uspviewShipToDeliveryOrderDownload]", dbPara, commandType: CommandType.StoredProcedure); 
            if (data != null)
            {
                return data.ToList().Count();
            }
            else
            {
                return 0;
            }
        }
    }
}