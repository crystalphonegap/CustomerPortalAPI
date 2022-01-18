using CustomerPortalWebApi.Entities;
using CustomerPortalWebApi.Models;
using System.Collections.Generic;

namespace CustomerPortalWebApi.Interface
{
    public interface IRetailOrderService
    {
        long update(RetailOrder model);

        long Insert(RetailOrder model);

        List<ShipToModel> GetRetailSearch(string status, string UserCode, string UserType, int pageNo, int pageSize, string KeyWord, string Type);

        List<ShipToModel> GetRetailDownload(string status, string UserCode, string UserType, string KeyWord, string Type);

        long GetRetailCount(string status, string UserCode, string UserType, string KeyWord, string Type);

        List<RetailOrder> GetRetailOrderSearch(string fromdate, string todate, string status, string UserCode, string UserType, int pageNo, int pageSize, string KeyWord);

        List<RetailOrder> GetRetailOrderDownload(string fromdate, string todate, string UserCode, string UserType, string status, string KeyWord);

        List<RetailOrder> GetRetailOrderDetailsByOrderID(long OrderID);

        long GetRetailOrderSearchCount(string fromdate, string todate, string UserCode, string UserType, string status, string KeyWord);
    }
}