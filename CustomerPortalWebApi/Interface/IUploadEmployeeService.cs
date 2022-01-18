using CustomerPortalWebApi.Models;
using System.Collections.Generic;

namespace CustomerPortalWebApi.Interface
{
    public interface IUploadEmployeeService
    {
        long InsertSalesHierachyIntoTempTable(SalesHierachy saleshierachy);

        long InsertSalesHierachyIntoMainTable();

        long DeleteTempSaleshierachy();

        List<SalesHierachy> GetTempSalesHierachy();

        List<SalesHierachy> GetSalesHierachy(int PageNo, int PageSize, string KeyWord);

        List<SalesHierachy> DownloadSalesHierachy(string KeyWord);

        long GetSalesHierachyCount(string KeyWord);

        int InsertUserMasterIntoTemp(UploadEmployeeModel UserMasterDetails);


        int InsertUserMasterIntoTempkAM(UploadEmployeeModel UserMasterDetails);

        long InsertUserMasterIntoMainkAM(string UserType);

        long InsertUserMasterIntoMain(string UserType);

        long DeleteTempUserMaster(string UserType);

        List<UploadEmployeeModel> GetTempUserMaster(string UserType);

        List<SalesHierachy> GetSalesOffices();

        long InsertOrderAnalystMapping(OrderAnalystMappingModel model);

        List<OrderAnalystMappingModel> GetOrderAnalystMappingData(string usercode);

        long Delete(string usercode);
    }
}