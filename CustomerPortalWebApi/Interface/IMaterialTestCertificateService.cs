using CustomerPortalWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerPortalWebApi.Interface
{
    public interface IMaterialTestCertificateService
    {
        string GetDocNo();

        //long CreateMaterialCertificate(MaterialTestCertificateModel model);

        long InsertMaterialCertificate(MaterialTestCertificateModel model);

        long InsertMaterialCertificateDetail(List<MaterialTestCertificateDetailModel> model);

        List<MaterialTestCertificateModel> GetReqOrderNo(string ReqOrderNo);

        List<MaterialTestCertificateModel> GetMaterialTestCertificates(MaterialTestCertificateModel model);

        int InsertDealerProfileDataIntoTemp(CustomerProfileModel model);

        long DeleteTempDealerProfileData();
        List<CustomerProfileModel> GetTempDealerProfileData();

        long InsertDealerProfileDataIntoMain();

        List<CustomerProfileModel> GetDealerProfiledata(string Dealercode);

        long UpdateUserProfileImage(CustomerProfileModel model);

        long GetCustomerProfileListCount(string KeyWord);

        List<CustomerProfileModel> GetCustomerProfileList(int PageNo, int PageSize, string KeyWord);
        List<CustomerProfileModel> GetCustomerProfileListDownload(string KeyWord);

        string DeleteMaterialTestCertificate(int  ID);
    }
}
