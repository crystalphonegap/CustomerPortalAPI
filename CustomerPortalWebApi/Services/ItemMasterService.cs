using CustomerPortalWebApi.Interface;
using CustomerPortalWebApi.Models;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CustomerPortalWebApi.Services
{
    public class ItemMasterService : IItemMasterService
    {
        private readonly ICustomerPortalHelper _customerPortalHelper;

        public ItemMasterService(ICustomerPortalHelper customerPortalHelper)
        {
            _customerPortalHelper = customerPortalHelper;
        }

        public List<ItemMasterModel> GetTopfiveItems()
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("Mode", "Top5", DbType.String);
            dbPara.Add("Search", "", DbType.String);

            var data = _customerPortalHelper.GetAll<ItemMasterModel>("[dbo].[uspviewItemMaster]", dbPara, commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public List<ItemMasterModel> GetAllItems(string KeyWord)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("Mode", "All", DbType.String);
            dbPara.Add("Search", KeyWord, DbType.String);

            var data = _customerPortalHelper.GetAll<ItemMasterModel>("[dbo].[uspviewItemMaster]", dbPara, commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }
    }
}