using CustomerPortalWebApi.Interface;
using CustomerPortalWebApi.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CustomerPortalWebApi.Services
{
    public class LoyalityPointsService : ILoyalityPointsService
    {
        private readonly ICustomerPortalHelper _customerPortalHelper;

        public LoyalityPointsService(ICustomerPortalHelper customerPortalHelper)
        {
            _customerPortalHelper = customerPortalHelper;
        }

        public long InsertLoyalityPoints(LoyalityPointsModel model)
        {

            var dbPara = new DynamicParameters();
            dbPara.Add("Divisionvtxt", model.Divisionvtxt, DbType.String);
            dbPara.Add("TillDatedatetime", model.TillDateDatetime, DbType.DateTime);
            dbPara.Add("CustomerCodevtxt", model.CustomerCodevtxt, DbType.String);
            dbPara.Add("CustomerNamevtxt", model.CustomerNamevtxt, DbType.String);
            dbPara.Add("EarnPoints", model.EarnPoints, DbType.Int64);
            dbPara.Add("UtilizePoints", model.UtilizePoints, DbType.Int64);
            dbPara.Add("CreatedByvtxt", model.CreatedByvtxt, DbType.String);

            #region using dapper

            var data = _customerPortalHelper.Insert<long>("[dbo].[uspInsertLoyalityPoints]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;

            #endregion using dapper
        }

        public List<LoyalityPointsModel> GetLoyalityPoints(string division, int PageNo, int PageSize, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("Division", division, DbType.String);
            dbPara.Add("PageNo", PageNo, DbType.Int32);
            dbPara.Add("PageSize", PageSize, DbType.Int32);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<LoyalityPointsModel>("[dbo].[uspviewGetLoyalityPointsData]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public List<LoyalityPointsModel> DownloadLoyalityPoints(string division, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("Division", division, DbType.String);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<LoyalityPointsModel>("[dbo].[uspviewDownloadLoyalityPointsData]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public long GetLoyalityPointsListCount(string division, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("Division", division, DbType.String);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<LoyalityPointsModel>("[dbo].[uspviewDownloadLoyalityPointsData]",
                            dbPara,
                            commandType: CommandType.StoredProcedure); 
            if (data != null)
            {
                return Convert.ToInt64(data.ToList().Count);
            }
            else
            {
                return 0;
            }
        }

        public LoyalityPointsModel GetLoyalityPointsDashboard(string customercode)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("customercode", customercode, DbType.String);
            var data = _customerPortalHelper.GetAll<LoyalityPointsModel>("[dbo].[uspviewLoyalityPoints]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            if (data.Count!=0)
            {
                return data[0];
            }
            else
            {
                return null;
            }
        }
    }
}