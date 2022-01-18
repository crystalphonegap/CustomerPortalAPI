using CustomerPortalWebApi.Entities;
using CustomerPortalWebApi.Models;
using System;
using System.Collections.Generic;

namespace CustomerPortalWebApi.Interface
{
    public interface ICustomerMasterService
    {
        List<CustomerMaster> GetCustomerMaster(string Division, int pageNo, int pageSize, string KeyWord);

        List<CustomerMaster> GetCustomerDataByUserId(string UserId);

        string EditProfile(UserMaster UserMaster);

        List<ShipToModel> GetShipTo(string CustomerCode);

        List<CustomerMaster> GetCustomerByUser(string CustomerCode);

        List<ShipToModel> GetShipToAddress(string ShipToCode);

        long CustomerCount(string Division);

        List<CustomerMasterModel> CheckValidCustomer(string customercode, string accesskey);

        long update(CustomerMasterModel Customermasterdeatils);

        long UpdateCustomerStatus(CustomerMasterModel Customermasterdeatils);

        List<CustomerMasterModel> GetCustomerMasterUserType(string usercode, string usertype, int PageNo, int PageSize, string KeyWord, Boolean status, Boolean isActive);

        List<CustomerMasterModel> GetCustomerMasterUserTypeDownload(string usercode, string usertype, string KeyWord, Boolean status, Boolean isActive);

        long GetCustomerMasterUserTypeCount(string usercode, string usertype, string KeyWord, Boolean status, Boolean isActive);

        List<CustomerLedger> GetCustomerLedger(string CustomerCode, int PageNo, int PageSize);

        long GetCustomerLedgerCount(string CustomerCode);

        long GetCustomerMasterSystemAdminSearchCount(string status, string division, string KeyWord);

        List<CustomerMasterModel> GetCustomerMasterSystemAdminDownload(string status, string division, string KeyWord);

        List<CustomerMasterModel> GetCustomerMasterSystemAdminSearch(int PageNo, int PageSize, string status, string division, string KeyWord);

        List<CustomerLedger> GetCustomerLedgerForExportToExcel(string CustomerCode);

        List<CustomerMaster> GetCustomerDataforKAM(string UserId);

        List<PlantMasterModel> GetPlantMaster(string customercode);
        int UploadMason(MasonModel Model);

        List<MasonModel> GetTempMason(int PageNo, int PageSize, string keyword);
    }
}