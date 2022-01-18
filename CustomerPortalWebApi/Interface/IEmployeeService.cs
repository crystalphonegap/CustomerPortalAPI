using CustomerPortalWebApi.Models;
using System.Collections.Generic;

namespace CustomerPortalWebApi.Interface
{
    public interface IEmployeeService
    {
        List<EmployeeDashboardCountModel> GetEmployeeDashboardCounts(string usercode, string usertype, string Type);

        List<TargetSalesModel> GetEmployeeWiseTargetVsActualSalesData(string usercode, string usertype, string date, string Type);

        List<TargetSalesModel> GetEmployeeWiseReport(string mode, string code, string date, string Type);

        List<TargetSalesModel> GetAreaNameByAreaCode(string mode, string code);

        List<TargetSalesModel> GetEmployeeWiseSalesCount(string usercode, string usertype, string date, string Type);

        List<CFSPOustandingCountModel> GetCFSPOustingDataCount(string usercode, string usertype, string mode);

        List<CFSPOustandingListModel> GetCFSPOustingDataList(string usercode, string usertype, string mode);

        List<CFSPOustandingListModel> GetDealerListInEmployeeDashboard(string usercode, string usertype, string date, string mode,string Trade,string FillterType);
    }
}