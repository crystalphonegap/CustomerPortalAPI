using CustomerPortalWebApi.Models;
using System.Collections.Generic;

namespace CustomerPortalWebApi.Interface
{
    public interface IOrderService
    {
        string GetOrderNo();

        List<OrderHeaderModel> GetReqOrderNo(string ReqOrderNo);

        long Create(OrderHeaderModel orderHeaderdetails);

        long update(OrderHeaderModel orderHeaderdetails);

        long updateStatus(OrderHeaderModel orderHeaderdetails);

        long Delete(long id);

        long InsertOrderDetails(OrderDetailsModel orderdetails);

        List<OrderHeaderModel> GetOrderList(string fromdate, string todate, string status, string customercode, int PageNo, int PageSize, string KeyWord);

        List<OrderHeaderModel> DownloadOrderList(string fromdate, string todate, string status, string customercode, string KeyWord);

        List<OrderHeaderModel> GetOrderHeaderByOrderID(long orderid);

        List<OrderDetailsModel> GetOrderDetailsByOrderID(long orderid);

        long GetOrderListCount(string fromdate, string todate, string status, string customercode, string KeyWord);

        long GetAllOrderListCount(string fromdate, string todate, string status, string KeyWord);

        List<OrderHeaderModel> GetAllOrderList(string fromdate, string todate, string status, int PageNo, int PageSize, string KeyWord);

        List<UOM> GetUOM();

        List<UOM> GetUOMByID(int ID);

        List<SPMappedPlantModels> GetMappedPlantList(string usercode);

        long updateOrderStatus(OrderHeaderModel ordmodel);

        long InsertSApOrderResponse(SAPCreateOrderOutputModel model);

        long DeleteSApOrderResponse(string orderno);

        List<SAPCreateOrderOutputModel> GetSAPOrderResponse(string orderidbint);

        List<OrderReportModel> GetOrderReportList(string fromdate, string todate, string Region, string Branch, string Territory, string status, int PageNo, int PageSize, string KeyWord);

        long GetOrderReportListCount(string fromdate, string todate, string Region, string Branch, string Territory, string status, string KeyWord);

        List<OrderReportModel> GetOrderReportListDownload(string fromdate, string todate, string Region, string Branch, string Territory, string status, string KeyWord);
    }
}