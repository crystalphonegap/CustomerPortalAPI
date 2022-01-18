using CustomerPortalWebApi.Entities;
using CustomerPortalWebApi.Interface;
using CustomerPortalWebApi.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CustomerPortalWebApi.Services
{
    public class RetailOrderService : IRetailOrderService
    {
        private readonly ICustomerPortalHelper _customerPortalHelper;

        public RetailOrderService(ICustomerPortalHelper customerPortalHelper)
        {
            _customerPortalHelper = customerPortalHelper;
        }

        public List<RetailOrder> GetRetailOrderDetailsByOrderID(long orderid)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("orderid", orderid, DbType.Int64);

            var data = _customerPortalHelper.GetAll<RetailOrder>("[dbo].[uspviewRetailOrderDetailsDataByOrderID]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        long IRetailOrderService.update(RetailOrder model)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("OrderID", model.IDbint, DbType.Int64);
            dbPara.Add("OrderNovtxt", model.OrderNovtxt, DbType.String);
            dbPara.Add("OrderDatedate", model.OrderDatedate, DbType.Date);
            dbPara.Add("MaterialCodevtxt", model.MaterialCodevtxt, DbType.String);
            dbPara.Add("MaterialDescvtxt", model.MaterialDescvtxt, DbType.String);
            dbPara.Add("Quantitydcl", model.Quantitydcl, DbType.Int64);
            dbPara.Add("TotalOrderQuantityKgsint", model.TotalOrderQuantityKgsint, DbType.Int64);
            dbPara.Add("TotalOrderQuantityMTint", model.TotalOrderQuantityMTint, DbType.Int64);
            dbPara.Add("DeliveryAddressvtxt", model.DeliveryAddressvtxt, DbType.String);
            dbPara.Add("Statusvtxt", model.Statusvtxt, DbType.String);
            dbPara.Add("UOMvtxt", model.UOMvtxt, DbType.String);
            dbPara.Add("CreatedBytxt", model.CreatedBytxt, DbType.String);
            dbPara.Add("CreatedDatedatetime", DateTime.Now, DbType.DateTime);

            #region using dapper

            var data = _customerPortalHelper.Update<long>("[dbo].[uspUpdateRetailOrder]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;

            #endregion using dapper
        }

        long IRetailOrderService.Insert(RetailOrder model)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("OrderNovtxt", model.OrderNovtxt, DbType.String);
            dbPara.Add("OrderDatedate", model.OrderDatedate, DbType.Date);
            dbPara.Add("MaterialCodevtxt", model.MaterialCodevtxt, DbType.String);
            dbPara.Add("MaterialDescvtxt", model.MaterialDescvtxt, DbType.String);
            dbPara.Add("UOMvtxt", model.UOMvtxt, DbType.String);
            dbPara.Add("Quantitydcl", model.Quantitydcl, DbType.Int64);
            dbPara.Add("TotalOrderQuantityKgsint", model.TotalOrderQuantityKgsint, DbType.Int64);
            dbPara.Add("TotalOrderQuantityMTint", model.TotalOrderQuantityMTint, DbType.Int64);
            dbPara.Add("DeliveryAddressvtxt", model.DeliveryAddressvtxt, DbType.String);
            dbPara.Add("RetailerCodevtxt", model.RetailerCodevtxt, DbType.String);
            dbPara.Add("Statusvtxt", model.Statusvtxt, DbType.String);

            #region using dapper

            var data = _customerPortalHelper.Insert<long>("[dbo].[uspInsertRetailOrders]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;

            #endregion using dapper
        }

        public List<RetailOrder> GetRetailOrderSearch(string fromdate, string todate, string status, string UserCode, string UserType, int pageNo, int pageSize, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            DateTime fdate = DateTime.ParseExact(fromdate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(todate, "dd-MM-yyyy", null);
            dbPara.Add("fromdate", Convert.ToDateTime(fdate).ToString("yyyy-MM-dd"), DbType.DateTime);
            dbPara.Add("todate", Convert.ToDateTime(tdate).ToString("yyyy-MM-dd"), DbType.DateTime);
            dbPara.Add("status", status, DbType.String);
            dbPara.Add("UserCode", UserCode, DbType.String);
            dbPara.Add("UserType", UserType, DbType.String);
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
            var data = _customerPortalHelper.GetAll<RetailOrder>("[dbo].[uspviewGetRetailOrderSearch]", dbPara, commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public List<ShipToModel> GetRetailSearch(string status, string UserCode, string UserType, int pageNo, int pageSize, string KeyWord, string Type)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("status", status, DbType.String);
            dbPara.Add("UserCode", UserCode, DbType.String);
            dbPara.Add("UserType", UserType, DbType.String);
            dbPara.Add("Type", Type, DbType.String);
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
            var data = _customerPortalHelper.GetAll<ShipToModel>("[dbo].[uspviewGetRetailSearch]", dbPara, commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public long GetRetailCount(string status, string UserCode, string UserType, string KeyWord, string Type)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("status", status, DbType.String);
            dbPara.Add("UserCode", UserCode, DbType.String);
            dbPara.Add("UserType", UserType, DbType.String);
            dbPara.Add("Type", Type, DbType.String);
            dbPara.Add("PageNo", 0, DbType.Int32);
            dbPara.Add("PageSize", 0, DbType.Int32);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<Count>("[dbo].[uspviewGetRetailSearch]", dbPara, commandType: CommandType.StoredProcedure);
            
            if (data != null)
            {
                return data[0].ListCount;
            }
            else
            {
                return 0;
            }
        }

        public List<ShipToModel> GetRetailDownload(string status, string UserCode, string UserType, string KeyWord, string Type)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("status", status, DbType.String);
            dbPara.Add("UserCode", UserCode, DbType.String);
            dbPara.Add("UserType", UserType, DbType.String);
            dbPara.Add("Type", Type, DbType.String);
            dbPara.Add("PageNo", 0, DbType.Int32);
            dbPara.Add("PageSize", 0, DbType.Int32);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<ShipToModel>("[dbo].[uspviewGetRetailSearch]", dbPara, commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public long GetRetailOrderSearchCount(string fromdate, string todate, string status, string UserCode, string UserType, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            DateTime fdate = DateTime.ParseExact(fromdate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(todate, "dd-MM-yyyy", null);
            dbPara.Add("fromdate", Convert.ToDateTime(fdate).ToString("yyyy-MM-dd"), DbType.DateTime);
            dbPara.Add("todate", Convert.ToDateTime(tdate).ToString("yyyy-MM-dd"), DbType.DateTime);
            dbPara.Add("status", status, DbType.String);
            dbPara.Add("UserCode", UserCode, DbType.String);
            dbPara.Add("UserType", UserType, DbType.String);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<RetailOrder>("[dbo].[uspviewGetRetailOrderDownload]", dbPara, commandType: CommandType.StoredProcedure); 
            if (data != null)
            {
                return Convert.ToInt64(data.ToList().Count);
            }
            else
            {
                return 0;
            }
        }




        public List<RetailOrder> GetRetailOrderDownload(string fromdate, string todate, string status, string UserCode, string UserType, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            DateTime fdate = DateTime.ParseExact(fromdate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(todate, "dd-MM-yyyy", null);
            dbPara.Add("fromdate", Convert.ToDateTime(fdate).ToString("yyyy-MM-dd"), DbType.DateTime);
            dbPara.Add("todate", Convert.ToDateTime(tdate).ToString("yyyy-MM-dd"), DbType.DateTime);
            dbPara.Add("status", status, DbType.String);
            dbPara.Add("UserCode", UserCode, DbType.String);
            dbPara.Add("UserType", UserType, DbType.String);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<RetailOrder>("[dbo].[uspviewGetRetailOrderDownload]", dbPara, commandType: CommandType.StoredProcedure);

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