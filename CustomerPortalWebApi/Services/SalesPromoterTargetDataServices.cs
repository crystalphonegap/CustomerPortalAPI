using CustomerPortalWebApi.Interface;
using CustomerPortalWebApi.Models;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CustomerPortalWebApi.Services
{
    public class SalesPromoterTargetDataServices : ISalesPromoterTargetDataServices
    {
        private readonly ICustomerPortalHelper _customerPortalHelper;

        public SalesPromoterTargetDataServices(ICustomerPortalHelper customerPortalHelper)
        {
            _customerPortalHelper = customerPortalHelper;
        }

        public long InsertSalesPromoterTargetDataIntoTempTable(SalesPromoterTargetData salespromoterdata)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("SalesPromotorCodevtxt", salespromoterdata.SalesPromotorCodevtxt, DbType.String);
            dbPara.Add("Monthvtxt", salespromoterdata.Monthvtxt, DbType.String);
            dbPara.Add("DealerTargetApptint", salespromoterdata.DealerTargetApptint, DbType.Int32);
            dbPara.Add("DealerActualApptint", salespromoterdata.DealerActualApptint, DbType.Int32);
            dbPara.Add("RetailerTargetApptint", salespromoterdata.RetailerTargetApptint, DbType.Int32);
            dbPara.Add("RetailerActualApptint", salespromoterdata.RetailerActualApptint, DbType.Int32);
            dbPara.Add("CreatedByvtxt", salespromoterdata.CreatedByvtxt, DbType.String);
            dbPara.Add("Yeartxt", salespromoterdata.CreatedByvtxt, DbType.String);

            #region using dapper

            var data = _customerPortalHelper.Insert<long>("[dbo].[uspSalesPromoterTargetDataintoTemp]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;

            #endregion using dapper
        }

        public long InsertSalesPromoterTargetDataIntoMainTable()
        {
            var dbPara = new DynamicParameters();
            var data = _customerPortalHelper.Execute("[dbo].[uspSalesPromoterTargetDataintoMain]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public long DeleteSalesPromoterTargetData()
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("mode", "DELETE", DbType.String);
            var data = _customerPortalHelper.Execute("[dbo].[uspgetdeleteTempSalesPromoterTargetData]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public List<SalesPromoterTargetData> GetTempSalesPromoterTargetData()
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("mode", "TempData", DbType.String);
            var data = _customerPortalHelper.GetAll<SalesPromoterTargetData>("[dbo].[uspgetdeleteTempSalesPromoterTargetData]",
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

        public List<SalesPromoterTargetData> GetSalesPromoterTargetDataList(int PageNo, int PageSize, string KeyWord)
        {
            var dbPara = new DynamicParameters();
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
            var data = _customerPortalHelper.GetAll<SalesPromoterTargetData>("[dbo].[uspviewSalesPromoterTargetData]",
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

        public List<SalesPromoterTargetData> GetSalesPromoterTargetDataInDashboard(string Usercode, string Usertype)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("Usercode", Usercode, DbType.String);
            dbPara.Add("Usertype", Usertype, DbType.String);
            var data = _customerPortalHelper.GetAll<SalesPromoterTargetData>("[dbo].[uspviewSalesPromoterTargetDataForDashboard]",
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
    }
}