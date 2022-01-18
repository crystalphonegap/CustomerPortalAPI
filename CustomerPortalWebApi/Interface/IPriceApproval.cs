using CustomerPortalWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerPortalWebApi.Interface
{
    public interface IPriceApproval 
    {
        long InsertPriceApproval(PriceApprovalModel model);
        long InsertFinalPriceApproval(PriceApprovalModel model);
        List<PriceApprovalModel> GetPriceApprovalData(string Statusvtxt, string Createdby, int PageNo, int PageSize, string KeyWord);
        List<PriceApprovalModel> DownloadPriceApprovalData(string Statusvtxt, string Createdby, string KeyWord);
        long GetPriceApprovalDataCount(string Statusvtxt, string Createdby, string KeyWord);
        PriceApprovalModel GetPriceApprovalDataById( long id);
    }
}
