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
    public class OrderService : IOrderService
    {
        private readonly ICustomerPortalHelper _customerPortalHelper;

        public OrderService(ICustomerPortalHelper customerPortalHelper)
        {
            _customerPortalHelper = customerPortalHelper;
        }

        public string GetOrderNo()
        {
            var dbPara = new DynamicParameters();
            var data = _customerPortalHelper.GetAll<OrderHeaderModel>("[dbo].[uspviewgetOrderNo]", dbPara, commandType: CommandType.StoredProcedure);
            if (data != null)
            {
                return data[0].ReqOrderNo.ToString();
            }
            else
            {
                return null;
            }
        }

        public List<OrderHeaderModel> GetReqOrderNo(string ReqOrderNo)
        {
            List<OrderHeaderModel> lstOrd = new List<OrderHeaderModel>();
            OrderHeaderModel ord = new OrderHeaderModel();
            ord.ReqOrderNo = ReqOrderNo;
            ord.OrderDatedate = DateTime.Now.Date;
            lstOrd.Add(ord);
            if (lstOrd!= null)
            {
                return lstOrd.ToList();
            }
            else
            {
                return null;
            }
        }

        public long Create(OrderHeaderModel orderHeaderdetails)
        {
            var dbPara = new DynamicParameters();

            dbPara.Add("OrderNovtxt", orderHeaderdetails.OrderNovtxt, DbType.String);
            dbPara.Add("OrderDatedate", orderHeaderdetails.OrderDatedate, DbType.Date);
            dbPara.Add("RefNovtxt", orderHeaderdetails.RefNovtxt, DbType.String);
            if (orderHeaderdetails.RefDatedate != null)
            {
                DateTime RefDatedate = DateTime.ParseExact(orderHeaderdetails.RefDatedate, "dd-MM-yyyy", null);
                dbPara.Add("RefDatedate", RefDatedate, DbType.Date);
            }
            else
            {
                dbPara.Add("RefDatedate", orderHeaderdetails.RefDatedate, DbType.Date);
            }
            dbPara.Add("SAPOrderNovtxt", orderHeaderdetails.SAPOrderNovtxt, DbType.String);
            dbPara.Add("SAPOrderDatedate", orderHeaderdetails.SAPOrderDatedate, DbType.Date);
            dbPara.Add("CustomerCodevtxt", orderHeaderdetails.CustomerCodevtxt, DbType.String);
            dbPara.Add("CustomerNamevtxt", orderHeaderdetails.CustomerNamevtxt, DbType.String);
            dbPara.Add("Divisionvtxt", orderHeaderdetails.Divisionvtxt, DbType.String);
            dbPara.Add("ShipToCodevtxt", orderHeaderdetails.ShipToCodevtxt, DbType.String);
            dbPara.Add("ShipToNamevtxt", orderHeaderdetails.ShipToNamevtxt, DbType.String);
            dbPara.Add("ShipToAddressvtxt", orderHeaderdetails.ShipToAddressvtxt, DbType.String);
            dbPara.Add("TotalNetValuedcl", orderHeaderdetails.TotalNetValuedcl, DbType.Decimal);
            dbPara.Add("TotalOrderQuantityint", orderHeaderdetails.TotalOrderQuantityint, DbType.Decimal);
            dbPara.Add("TotalOrderQuantityKgsint", orderHeaderdetails.TotalOrderQuantityKgsint, DbType.Decimal);
            dbPara.Add("TotalOrderQuantityMTint", orderHeaderdetails.TotalOrderQuantityMTint, DbType.Decimal);
            dbPara.Add("SAPStatusvtxt", orderHeaderdetails.SAPStatusvtxt, DbType.String);
            dbPara.Add("OtherCharges1dcl", orderHeaderdetails.OtherCharges1dcl, DbType.Decimal);
            dbPara.Add("OtherCharges2dcl", orderHeaderdetails.OtherCharges2dcl, DbType.Decimal);
            dbPara.Add("OtherCharges3dcl", orderHeaderdetails.OtherCharges3dcl, DbType.Decimal);
            dbPara.Add("OtherCharges4dcl", orderHeaderdetails.OtherCharges4dcl, DbType.Decimal);
            dbPara.Add("DeliveryAddressvtxt", orderHeaderdetails.DeliveryAddressvtxt, DbType.String);
            dbPara.Add("DeliveryTermsvtxt", orderHeaderdetails.DeliveryTermsvtxt, DbType.String);
            dbPara.Add("PaymentTermsvtxt", orderHeaderdetails.PaymentTermsvtxt, DbType.String);
            dbPara.Add("Statusvtxt", orderHeaderdetails.Statusvtxt, DbType.String);
            dbPara.Add("CreatedByvtxt", orderHeaderdetails.CreatedByvtxt, DbType.String);
            dbPara.Add("CreatedDatedatetime", DateTime.Now, DbType.DateTime);

            #region using dapper

            var data = _customerPortalHelper.Insert<long>("[dbo].[uspInsertOrderHeader]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;

            #endregion using dapper
        }

        long IOrderService.update(OrderHeaderModel orderHeaderdetails)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("OrderID", orderHeaderdetails.IDbint, DbType.Int64);
            dbPara.Add("OrderNovtxt", orderHeaderdetails.OrderNovtxt, DbType.String);
            dbPara.Add("OrderDatedate", Convert.ToDateTime(orderHeaderdetails.OrderDatedate), DbType.Date);
            dbPara.Add("RefNovtxt", orderHeaderdetails.RefNovtxt, DbType.String);
            if (orderHeaderdetails.RefDatedate != null)
            {
                DateTime RefDatedate = DateTime.ParseExact(orderHeaderdetails.RefDatedate, "dd-MM-yyyy", null);
                dbPara.Add("RefDatedate", RefDatedate, DbType.Date);
            }
            else
            {
                dbPara.Add("RefDatedate", orderHeaderdetails.RefDatedate, DbType.Date);
            }
            dbPara.Add("SAPOrderNovtxt", orderHeaderdetails.SAPOrderNovtxt, DbType.String);
            dbPara.Add("SAPOrderDatedate", orderHeaderdetails.SAPOrderDatedate, DbType.Date);
            dbPara.Add("CustomerCodevtxt", orderHeaderdetails.CustomerCodevtxt, DbType.String);
            dbPara.Add("CustomerNamevtxt", orderHeaderdetails.CustomerNamevtxt, DbType.String);
            dbPara.Add("Divisionvtxt", orderHeaderdetails.Divisionvtxt, DbType.String);
            dbPara.Add("ShipToCodevtxt", orderHeaderdetails.ShipToCodevtxt, DbType.String);
            dbPara.Add("ShipToNamevtxt", orderHeaderdetails.ShipToNamevtxt, DbType.String);
            dbPara.Add("ShipToAddressvtxt", orderHeaderdetails.ShipToAddressvtxt, DbType.String);
            dbPara.Add("TotalNetValuedcl", orderHeaderdetails.TotalNetValuedcl, DbType.Decimal);
            dbPara.Add("TotalOrderQuantityint", orderHeaderdetails.TotalOrderQuantityint, DbType.Decimal);
            dbPara.Add("TotalOrderQuantityKgsint", orderHeaderdetails.TotalOrderQuantityKgsint, DbType.Decimal);
            dbPara.Add("TotalOrderQuantityMTint", orderHeaderdetails.TotalOrderQuantityMTint, DbType.Decimal);
            dbPara.Add("SAPStatusvtxt", orderHeaderdetails.SAPStatusvtxt, DbType.String);
            dbPara.Add("OtherCharges1dcl", orderHeaderdetails.OtherCharges1dcl, DbType.Decimal);
            dbPara.Add("OtherCharges2dcl", orderHeaderdetails.OtherCharges2dcl, DbType.Decimal);
            dbPara.Add("OtherCharges3dcl", orderHeaderdetails.OtherCharges3dcl, DbType.Decimal);
            dbPara.Add("OtherCharges4dcl", orderHeaderdetails.OtherCharges4dcl, DbType.Decimal);
            dbPara.Add("DeliveryAddressvtxt", orderHeaderdetails.DeliveryAddressvtxt, DbType.String);
            dbPara.Add("DeliveryTermsvtxt", orderHeaderdetails.DeliveryTermsvtxt, DbType.String);
            dbPara.Add("PaymentTermsvtxt", orderHeaderdetails.PaymentTermsvtxt, DbType.String);
            dbPara.Add("Statusvtxt", orderHeaderdetails.Statusvtxt, DbType.String);
            dbPara.Add("CreatedByvtxt", orderHeaderdetails.CreatedByvtxt, DbType.String);
            dbPara.Add("CreatedDatedatetime", DateTime.Now, DbType.DateTime);

            #region using dapper

            var data = _customerPortalHelper.Update<long>("[dbo].[uspUpdateOrderHeader]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;

            #endregion using dapper
        }

        long IOrderService.updateStatus(OrderHeaderModel orderHeaderdetails)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("OrderID", orderHeaderdetails.IDbint, DbType.Int64);
            dbPara.Add("Statusvtxt", orderHeaderdetails.Statusvtxt, DbType.String);
            dbPara.Add("CreatedByvtxt", orderHeaderdetails.CreatedByvtxt, DbType.String);
            dbPara.Add("CreatedDatedatetime", DateTime.Now, DbType.DateTime);

            #region using dapper

            var data = _customerPortalHelper.Update<long>("[dbo].[uspUpdateOrderHeaderStatus]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;

            #endregion using dapper
        }

        public long Delete(long id)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("OrderID", id, DbType.Int64);
            var data = _customerPortalHelper.Execute("[dbo].[uspDeleteOrderDetails]", dbPara,
                   commandType: CommandType.StoredProcedure);
            return data;
        }

        public long InsertOrderDetails(OrderDetailsModel orderdetails)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("OrderID", orderdetails.OrderID, DbType.Int64);
            dbPara.Add("MaterialCodevtxt", orderdetails.MaterialCodevtxt, DbType.String);
            dbPara.Add("MaterialDescriptionvtxt", orderdetails.MaterialDescriptionvtxt, DbType.String);
            dbPara.Add("UoMvtxt", orderdetails.UoMvtxt, DbType.String);
            dbPara.Add("Quantityint", orderdetails.Quantityint, DbType.Decimal);
            dbPara.Add("Ratedcl", orderdetails.Ratedcl, DbType.Decimal);
            dbPara.Add("Amountdcl", orderdetails.Amountdcl, DbType.Decimal);

            #region using dapper

            var data = _customerPortalHelper.Insert<long>("[dbo].[uspInsertOrderDetails]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;

            #endregion using dapper
        }

        public List<OrderHeaderModel> GetOrderList(string fromdate, string todate, string status, string customercode, int PageNo, int PageSize, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            DateTime fdate = DateTime.ParseExact(fromdate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(todate, "dd-MM-yyyy", null);
            dbPara.Add("fromDate", Convert.ToDateTime(fdate).ToString("yyyy-MM-dd"), DbType.String);
            dbPara.Add("todate", Convert.ToDateTime(tdate).ToString("yyyy-MM-dd"), DbType.String);

            dbPara.Add("Status", status, DbType.String);
            dbPara.Add("CustomerCode", customercode, DbType.String);
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
            var data = _customerPortalHelper.GetAll<OrderHeaderModel>("[dbo].[uspviewGetOrderListByCustomerCode]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public List<OrderHeaderModel> DownloadOrderList(string fromdate, string todate, string status, string customercode, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            DateTime fdate = DateTime.ParseExact(fromdate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(todate, "dd-MM-yyyy", null);
            dbPara.Add("fromDate", Convert.ToDateTime(tdate).ToString("yyyy-MM-dd"), DbType.String);
            dbPara.Add("todate", Convert.ToDateTime(todate).ToString("yyyy-MM-dd"), DbType.String);
            dbPara.Add("Status", status, DbType.String);
            dbPara.Add("CustomerCode", customercode, DbType.String);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<OrderHeaderModel>("[dbo].[uspviewDownloadGetOrderListByCustomerCode]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public List<OrderHeaderModel> GetAllOrderList(string fromdate, string todate, string status, int PageNo, int PageSize, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            DateTime fdate = DateTime.ParseExact(fromdate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(todate, "dd-MM-yyyy", null);
            dbPara.Add("fromDate", Convert.ToDateTime(fdate).ToString("yyyy-MM-dd"), DbType.String);
            dbPara.Add("todate", Convert.ToDateTime(tdate).ToString("yyyy-MM-dd"), DbType.String);
            dbPara.Add("Status", status, DbType.String);
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
            var data = _customerPortalHelper.GetAll<OrderHeaderModel>("[dbo].[uspviewAllGetOrderList]",
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

        public long GetOrderListCount(string fromdate, string todate, string status, string customercode, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            DateTime fdate = DateTime.ParseExact(fromdate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(todate, "dd-MM-yyyy", null);
            dbPara.Add("fromDate", Convert.ToDateTime(fdate).ToString("yyyy-MM-dd"), DbType.String);
            dbPara.Add("todate", Convert.ToDateTime(tdate).ToString("yyyy-MM-dd"), DbType.String);
            dbPara.Add("Status", status, DbType.String);
            dbPara.Add("CustomerCode", customercode, DbType.String);
            dbPara.Add("KeyWord", KeyWord, DbType.String);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<Count>("[dbo].[uspviewGetOrderListByCustomerCodeCount]", dbPara, commandType: CommandType.StoredProcedure);


            if (data != null)
            {
                return Convert.ToInt64(data[0].ListCount);
            }
            else
            {
                return 0;
            }
        }

        public long GetAllOrderListCount(string fromdate, string todate, string status, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            DateTime fdate = DateTime.ParseExact(fromdate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(todate, "dd-MM-yyyy", null);
            dbPara.Add("fromDate", Convert.ToDateTime(fdate).ToString("yyyy-MM-dd"), DbType.String);
            dbPara.Add("todate", Convert.ToDateTime(tdate).ToString("yyyy-MM-dd"), DbType.String);
            dbPara.Add("Status", status, DbType.String);
            dbPara.Add("KeyWord", KeyWord, DbType.String);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<Count>("[dbo].[uspviewAllGetOrderListCount]", dbPara, commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return Convert.ToInt64(data[0].ListCount);
            }
            else
            {
                return 0;
            }
        }

        public List<OrderHeaderModel> GetOrderHeaderByOrderID(long orderid)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("orderid", orderid, DbType.Int64);
            dbPara.Add("Mode", "Header", DbType.String);

            var data = _customerPortalHelper.GetAll<OrderHeaderModel>("[dbo].[uspviewOrderHeaderandDetailsDataByOrderID]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public List<OrderDetailsModel> GetOrderDetailsByOrderID(long orderid)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("orderid", orderid, DbType.Int64);
            dbPara.Add("Mode", "Details", DbType.String);

            var data = _customerPortalHelper.GetAll<OrderDetailsModel>("[dbo].[uspviewOrderHeaderandDetailsDataByOrderID]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public List<UOM> GetUOM()
        {
            var dbPara = new DynamicParameters();
            var data = _customerPortalHelper.GetAll<UOM>("[dbo].[uspviewUOM]",
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

        public List<UOM> GetUOMByID(int ID)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("ID", ID, DbType.Int64);
            var data = _customerPortalHelper.GetAll<UOM>("[dbo].[uspviewUOMByID]",
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

        public List<SPMappedPlantModels> GetMappedPlantList(string usercode)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("usercode", usercode, DbType.String);
            var data = _customerPortalHelper.GetAll<SPMappedPlantModels>("[dbo].[USP_GetMappedPlantsForSP]",
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

        public long updateOrderStatus(OrderHeaderModel ordmodel)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("orderid", ordmodel.IDbint, DbType.Int64);
            dbPara.Add("SAPOrderNO", ordmodel.SAPOrderNovtxt, DbType.String);
            var data = _customerPortalHelper.Insert<long>("[dbo].[uspUpdateOrderStatus]", dbPara, commandType: CommandType.StoredProcedure);
            if (data >0)
            {
                return Convert.ToInt64(data);
            }
            else
            {
                return 0;
            }
        }

        public long InsertSApOrderResponse(SAPCreateOrderOutputModel model)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("OrderIDbint", model.OrderIDbint, DbType.Int64);
            dbPara.Add("OrderNovtxt", model.OrderNovtxt, DbType.String);
            dbPara.Add("SAPOrderNovtxt", model.SAPOrderNovtxt, DbType.String);
            dbPara.Add("ErrorIDvtxt", model.ErrorIDvtxt, DbType.String);
            dbPara.Add("Numvtxt", model.Numvtxt, DbType.String);
            dbPara.Add("Messagevtxt", model.Messagevtxt, DbType.String);
            dbPara.Add("mode", "Insert", DbType.String);
            var data = _customerPortalHelper.Insert<long>("[dbo].[uspInsertSAPOrderCreationResponse]", dbPara, commandType: CommandType.StoredProcedure);
            if (data >0)
            {
                return Convert.ToInt64(data);
            }
            else
            {
                return 0;
            }
        }

        public long DeleteSApOrderResponse(string orderno)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("OrderIDbint", 0, DbType.Int64);
            dbPara.Add("OrderNovtxt", orderno, DbType.String);
            dbPara.Add("SAPOrderNovtxt", "", DbType.String);
            dbPara.Add("ErrorIDvtxt", "", DbType.String);
            dbPara.Add("Numvtxt", "", DbType.String);
            dbPara.Add("Messagevtxt", "", DbType.String);
            dbPara.Add("mode", "Delete", DbType.String);
            var data = _customerPortalHelper.Insert<long>("[dbo].[uspInsertSAPOrderCreationResponse]", dbPara, commandType: CommandType.StoredProcedure);
            if (data >0)
            {
                return Convert.ToInt64(data);
            }
            else
            {
                return 0;
            }
        }

        public List<SAPCreateOrderOutputModel> GetSAPOrderResponse(string orderidbint)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("OrderId", orderidbint, DbType.String);
            var data = _customerPortalHelper.GetAll<SAPCreateOrderOutputModel>("[dbo].[uspGetSAPOrderCreationResponse]",
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

        public List<OrderReportModel> GetOrderReportList(string fromdate, string todate, string Region, string Branch, string Territory, string status, int PageNo, int PageSize, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            DateTime fdate = DateTime.ParseExact(fromdate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(todate, "dd-MM-yyyy", null);
            dbPara.Add("fromDate", Convert.ToDateTime(fdate).ToString("yyyy-MM-dd"), DbType.String);
            dbPara.Add("todate", Convert.ToDateTime(tdate).ToString("yyyy-MM-dd"), DbType.String);
            if (Region == "NoSearch")
            {
                dbPara.Add("Region", "", DbType.String);
            }
            else
            {
                dbPara.Add("Region", Region, DbType.String);
            }
            if (Branch == "NoSearch")
            {
                dbPara.Add("Branch", "", DbType.String);
            }
            else
            {
                dbPara.Add("Branch", Branch, DbType.String);
                dbPara.Add("Region", "", DbType.String);
            }
            if (Territory == "NoSearch")
            {
                dbPara.Add("Territory", "", DbType.String);
            }
            else
            {
                dbPara.Add("Territory", Territory, DbType.String);
                dbPara.Add("Branch", "", DbType.String);
                dbPara.Add("Region", "", DbType.String);
            }
            dbPara.Add("Status", status, DbType.String);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("Search", "", DbType.String);
            }
            else
            {
                dbPara.Add("Search", KeyWord, DbType.String);
            }
            dbPara.Add("PageNo", PageNo, DbType.Int32);
            dbPara.Add("PageSize", PageSize, DbType.Int32);
            var data = _customerPortalHelper.GetAll<OrderReportModel>("[dbo].[uspViewWebOrderReportSearch]",
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

        public long GetOrderReportListCount(string fromdate, string todate, string Region, string Branch, string Territory, string status, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            DateTime fdate = DateTime.ParseExact(fromdate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(todate, "dd-MM-yyyy", null);
            dbPara.Add("fromDate", Convert.ToDateTime(fdate).ToString("yyyy-MM-dd"), DbType.String);
            dbPara.Add("todate", Convert.ToDateTime(tdate).ToString("yyyy-MM-dd"), DbType.String);

            if (Region == "NoSearch")
            {
                dbPara.Add("Region", "", DbType.String);
            }
            else
            {
                dbPara.Add("Region", Region, DbType.String);
            }
            if (Branch == "NoSearch")
            {
                dbPara.Add("Branch", "", DbType.String);
            }
            else
            {
                dbPara.Add("Branch", Branch, DbType.String);
                dbPara.Add("Region", "", DbType.String);
            }
            if (Territory == "NoSearch")
            {
                dbPara.Add("Territory", "", DbType.String);
            }
            else
            {
                dbPara.Add("Territory", Territory, DbType.String);
                dbPara.Add("Branch", "", DbType.String);
                dbPara.Add("Region", "", DbType.String);
            }
            dbPara.Add("Status", status, DbType.String);
            dbPara.Add("Type", "Count", DbType.String);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("Search", "", DbType.String);
            }
            else
            {
                dbPara.Add("Search", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<Count>("[dbo].[uspViewWebOrderReportCountDownload]", dbPara, commandType: CommandType.StoredProcedure);
            if (data != null)
            {
                return Convert.ToInt64(data[0].ListCount);
            }
            else
            {
                return 0;
            } 
        }

        public List<OrderReportModel> GetOrderReportListDownload(string fromdate, string todate, string Region, string Branch, string Territory, string status, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            DateTime fdate = DateTime.ParseExact(fromdate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(todate, "dd-MM-yyyy", null);
            dbPara.Add("fromDate", Convert.ToDateTime(fdate).ToString("yyyy-MM-dd"), DbType.String);
            dbPara.Add("todate", Convert.ToDateTime(tdate).ToString("yyyy-MM-dd"), DbType.String);

            if (Region == "NoSearch")
            {
                dbPara.Add("Region", "", DbType.String);
            }
            else
            {
                dbPara.Add("Region", Region, DbType.String);
            }
            if (Branch == "NoSearch")
            {
                dbPara.Add("Branch", "", DbType.String);
            }
            else
            {
                dbPara.Add("Branch", Branch, DbType.String);
                dbPara.Add("Region", "", DbType.String);
            }
            if (Territory == "NoSearch")
            {
                dbPara.Add("Territory", "", DbType.String);
            }
            else
            {
                dbPara.Add("Territory", Territory, DbType.String);
                dbPara.Add("Branch", "", DbType.String);
                dbPara.Add("Region", "", DbType.String);
            }
            dbPara.Add("Status", status, DbType.String);
            dbPara.Add("Type", "Download", DbType.String);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("Search", "", DbType.String);
            }
            else
            {
                dbPara.Add("Search", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<OrderReportModel>("[dbo].[uspViewWebOrderReportCountDownload]", dbPara, commandType: CommandType.StoredProcedure);
            return data;
        }
    }
}