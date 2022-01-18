using CustomerPortalWebApi.Entities;
using System.Collections.Generic;

namespace CustomerPortalWebApi.Interface
{
    public interface ISalesOrderService
    {
        List<SalesOrder> GetSalesOrder(string SoldToPartyCode, int pageNo, int pageSize, string KeyWord);

        List<SalesOrder> getAllSalesOrderDataByOrderNo(string Orderno);

        List<SalesOrder> getSalesOrderHeaderDataByOrderNo(string Orderno);

        long GetSalesCount(string SoldToPartyCode, string KeyWord);

        List<SalesOrder> GetSalesOrderStatuswiseCount(string fromdate, string todate, string SoldToPartyCode, string KeyWord);

        List<SalesOrder> GetSalesOrderSearch(string fromdate, string todate, string status, string SoldToPartyCode, int pageNo, int pageSize, string KeyWord);

        List<SalesOrder> GetSalesOrderDownload(string fromdate, string todate, string SoldToPartyCode, string status, string KeyWord);

        long GetSalesOrderSearchCount(string fromdate, string todate, string SoldToPartyCode, string status, string KeyWord);

        List<SalesOrder> GetBlockedSalesOrderSearch(string usercode, string usertype, string fromdate, string todate, int pageNo, int pageSize, string KeyWord);

        List<SalesOrder> GetBlockedSalesOrderDownload(string usercode, string usertype, string fromdate, string todate, string KeyWord);

        long GetBlockedSalesOrderountC(string usercode, string usertype, string fromdate, string todate, string KeyWord);
    }
}