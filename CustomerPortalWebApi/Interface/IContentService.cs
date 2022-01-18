using CustomerPortalWebApi.Models;
using System.Collections.Generic;

namespace CustomerPortalWebApi.Interface
{
    public interface IContentService
    {
        long Insert(ContentModel content);

        long InsertDetail(HierarchyModel content);

        long update(ContentModel content);

        List<ContentModel> GetContentById(long id);

        List<ContentModel> GetContentByDate(string Date);

        List<ContentModel> GetContentList(int PageNo, int PageSize, string KeyWord);

        long GetContentListCount(string KeyWord);

        List<SalesHierachy> GetHierachyWiseCodeName(string type);

        List<HierarchyModel> GetContentDetailsById(long id);
    }
}