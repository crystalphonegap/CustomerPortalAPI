using CustomerPortalWebApi.Models;
using System.Collections.Generic;

namespace CustomerPortalWebApi.Interface
{
    public interface IItemMasterService
    {
        List<ItemMasterModel> GetTopfiveItems();

        List<ItemMasterModel> GetAllItems(string KeyWord);
    }
}