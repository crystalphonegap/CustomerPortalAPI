using CustomerPortalWebApi.Models;
using System.Collections.Generic;

namespace CustomerPortalWebApi.Interface
{
    public interface ICFAgentServices
    {
        List<CFAgentDashboardCountModels> GetCFAgentDashboardCounts(string usercode);

        List<OrderHeaderModel> GetAllOrderList(string fromdate, string todate, string status, string UserType, string UserCode, int PageNo, int PageSize, string KeyWord);

        List<OrderHeaderModel> GetAllOrderDownload(string fromdate, string todate, string status, string UserType, string UserCode, string KeyWord);

        long GetAllOrderCount(string fromdate, string todate, string status, string UserType, string UserCode, string KeyWord);

        List<SPCFALedger> GetSPCFALedger(string UserCode, int PageNo, int PageSize);

        long GetSPCFALedgerCount(string UserCode);

        List<SPCFALedger> GetSPCFALedgerExportToExcel(string UserCode);
    }
}