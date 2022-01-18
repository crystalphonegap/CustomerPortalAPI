using CustomerPortalWebApi.Entities;
using System.Collections.Generic;

namespace CustomerPortalWebApi.Interface
{
    public interface IDeliveryOrderService
    {
        List<DeliveryOrder> GetDeliveryOrder(string SoldToPartyCode, int pageNo, int pageSize, string KeyWord);

        List<DeliveryOrder> GetDeliveryOrderStatusCount(string fromdate, string todate, string SoldToPartyCode, string KeyWord);

        List<DeliveryOrder> GetDeliveryOrderByOrderNo(string Orderno);

        List<DeliveryOrder> getAllDeliveryOrderDataBySalesOrderNo(string Orderno);

        List<DeliveryOrder> GetDeliveryOrderHeaderByOrderNo(string Orderno);

        long GetDeliveryCount(string CustomerCode);

        long SetDeliveryStatus(DeliveryOrder model);

        List<DeliveryOrder> GetDeliveryOrderSearch(string fromdate, string todate, string status, string SoldToPartyCode, int pageNo, int pageSize, string KeyWord);

        long GetDeliveryCount(string fromdate, string todate, string status, string SoldToPartyCode, string KeyWord);

        List<DeliveryOrder> GetDeliveryOrderDownload(string fromdate, string todate, string status, string SoldToPartyCode, string KeyWord);

        List<DeliveryOrder> GetShipToDeliveryOrderSearch(string fromdate, string todate, string status, string ShipToPartyCode, int pageNo, int pageSize, string KeyWord);

        List<DeliveryOrder> GetShipToDeliveryOrderDownload(string fromdate, string todate, string status, string ShipToPartyCode, string KeyWord);

        long GetShipToDeliveryOrderCount(string fromdate, string todate, string status, string ShipToPartyCode, string KeyWord);
    }
}