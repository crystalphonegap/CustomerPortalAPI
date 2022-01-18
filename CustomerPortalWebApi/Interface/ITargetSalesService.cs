using CustomerPortalWebApi.Models;
using System.Collections.Generic;
using System.Data;

namespace CustomerPortalWebApi.Interface
{
    public interface ITargetSalesService
    {
        long InsertTargetSales(TargetSalesModel targetsales);

        List<TargetSalesModel> GetTargetSales(string division, int PageNo, int PageSize, string KeyWord);

        List<TargetSalesModel> DownloadTargetSales(string division, string KeyWord);

        long GetTargetSalesListCount(string division, string KeyWord);

        List<TargetSalesModel> GetTargetSalesForDashboard(string customercode, string date);

        List<TargetSalesModel> GetTargetSalesForDashboardByFinance(string customercode, string date);

        long InsertCustomerSales(CustomerSalesModel targetsales);

        List<CustomerSalesModel> GetCustomerSalesData(string Type, int PageNo, int PageSize, string KeyWord);

        long GetCustomerSalesListCount(string division, string KeyWord);

        List<CustomerSalesModel> DownloadCustomerSales(string division, string KeyWord);

        List<CustomerSalesModel> GetCustomerProfileTranscationDataTargetSales(string customercode, string date);

        List<CustomerSalesModel> GetCustomerProfileTranscationDataNCROrPayment(string customercode, string date, string Mode);

        DataTable GetSalesBreakUp(string customercode, string date);


        CustomerSalesModel GetCustomerProfileTranscationSalesHistory(string customercode, string date, string Mode);

        CustomerSalesModel GetCustomerProfileEffective(string customercode, string date);

        CustomerSalesModel GetCustomerProfileConsistency(string customercode, string date);

        List<TargetSalesModel> GetKAMTargetSalesForDashboard(string usercode, string date);
    }
}