using CustomerPortalWebApi.Models;
using System.Collections.Generic;

namespace CustomerPortalWebApi.Interface
{
    public interface ISalesPromoterTargetDataServices
    {
        long InsertSalesPromoterTargetDataIntoTempTable(SalesPromoterTargetData salespromoterdata);

        long InsertSalesPromoterTargetDataIntoMainTable();

        long DeleteSalesPromoterTargetData();

        List<SalesPromoterTargetData> GetTempSalesPromoterTargetData();

        List<SalesPromoterTargetData> GetSalesPromoterTargetDataList(int PageNo, int PageSize, string KeyWord);

        List<SalesPromoterTargetData> GetSalesPromoterTargetDataInDashboard(string Usercode, string usertype);
    }
}