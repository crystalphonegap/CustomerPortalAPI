using CustomerPortalWebApi.Models;
using System.Collections.Generic;

namespace CustomerPortalWebApi.Interface
{
    public interface ILoyalityPointsService
    {
        long InsertLoyalityPoints(LoyalityPointsModel model);

        List<LoyalityPointsModel> GetLoyalityPoints(string division, int PageNo, int PageSize, string KeyWord);

        List<LoyalityPointsModel> DownloadLoyalityPoints(string division, string KeyWord);

        long GetLoyalityPointsListCount(string division, string KeyWord);

        LoyalityPointsModel GetLoyalityPointsDashboard(string customercode);
    }
}