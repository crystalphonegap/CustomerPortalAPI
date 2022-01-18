using CustomerPortalWebApi.Models;
using System.Collections.Generic;

namespace CustomerPortalWebApi.Interface
{
    public interface IBroadCastservice
    {
        long Insert(BroadCastModel content);

        long InsertDetail(HierarchyModel content);

        long update(BroadCastModel content);

        List<BroadCastModel> GetBroadcastById(long id);

        List<BroadCastModel> GetBroadCastByDate(string Date);

        List<BroadCastModel> GetBroadcastList(int PageNo, int PageSize, string KeyWord);

        long GetBroadcastListCount(string KeyWord);

        List<HierarchyModel> GetBroadcastDetailsById(long id);
    }
}