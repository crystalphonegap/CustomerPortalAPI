using CustomerPortalWebApi.Interface;
using CustomerPortalWebApi.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CustomerPortalWebApi.Services
{
    public class BroadCastservice : IBroadCastservice
    {
        private readonly ICustomerPortalHelper _customerPortalHelper;

        public BroadCastservice(ICustomerPortalHelper customerPortalHelper)
        {
            _customerPortalHelper = customerPortalHelper;
        }

        public long Insert(BroadCastModel content)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("IDbint", content.IDbint, DbType.Int64);
            dbPara.Add("Titlevtxt", content.Titlevtxt, DbType.String);
            dbPara.Add("Typevtxt", content.Typevtxt, DbType.String);
            dbPara.Add("Messagevtxt", content.Messagevtxt, DbType.String);
            dbPara.Add("StartDatedate", content.StartDatedate, DbType.Date);
            dbPara.Add("EndDatedate", content.EndDatedate, DbType.Date);
            dbPara.Add("Statusbit", content.Statusbit, DbType.Boolean);
            dbPara.Add("CreatedByvtxt", content.CreatedByvtxt, DbType.String);

            var data = _customerPortalHelper.Insert<long>("[dbo].[uspInsertUpdateBroadcastMaster]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);

            return data;
        }

        public long update(BroadCastModel content)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("IDbint", content.IDbint, DbType.Int64);
            dbPara.Add("Titlevtxt", content.Titlevtxt, DbType.String);
            dbPara.Add("Typevtxt", content.Typevtxt, DbType.String);
            dbPara.Add("Messagevtxt", content.Messagevtxt, DbType.String);
            dbPara.Add("StartDatedate", content.StartDatedate, DbType.Date);
            dbPara.Add("EndDatedate", content.EndDatedate, DbType.Date);
            dbPara.Add("Statusbit", content.Statusbit, DbType.Boolean);
            dbPara.Add("CreatedByvtxt", content.CreatedByvtxt, DbType.String);

            var dbPara2 = new DynamicParameters();
            dbPara2.Add("IDbint", content.IDbint, DbType.Int64);
            dbPara2.Add("mode", "DeleteBroadCast", DbType.String);
            var data2 = _customerPortalHelper.Insert<long>("[dbo].[uspviewBroadcastAndContentbyid]",
                                dbPara2,
                                commandType: CommandType.StoredProcedure);

            var data = _customerPortalHelper.Insert<long>("[dbo].[uspInsertUpdateBroadcastMaster]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);

            return data;
        }

        public long InsertDetail(HierarchyModel content)
        {
            var dbPara = new DynamicParameters();

            var dbPara1 = new DynamicParameters();
            dbPara1.Add("HeaderIDbint", content.HeaderIDbint, DbType.Int64);
            dbPara1.Add("Codevtxt", content.Codevtxt, DbType.String);
            dbPara1.Add("Namevtxt", content.Namevtxt, DbType.String);

            var data = _customerPortalHelper.Insert<long>("[dbo].[uspInsertBroadCastDetails]",
                            dbPara1,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public List<HierarchyModel> GetBroadcastDetailsById(long id)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("IDbint", id, DbType.Int64);
            dbPara.Add("mode", "DetailBroadCast", DbType.String);

            var data = _customerPortalHelper.GetAll<HierarchyModel>("[dbo].[uspviewBroadcastAndContentbyid]",
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

        public List<BroadCastModel> GetBroadcastById(long id)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("IDbint", id, DbType.Int64);
            dbPara.Add("mode", "BroadCast", DbType.String);

            var data = _customerPortalHelper.GetAll<BroadCastModel>("[dbo].[uspviewBroadcastAndContentbyid]",
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

        public List<BroadCastModel> GetBroadCastByDate(string Date)
        {
            var dbPara = new DynamicParameters();
            DateTime tempDate = DateTime.ParseExact(Date, "dd-MM-yyyy", null);
            dbPara.Add("date", Convert.ToDateTime(tempDate).ToString("yyyy-MM-dd"), DbType.DateTime);
            dbPara.Add("mode", "BroadCast", DbType.String);

            var data = _customerPortalHelper.GetAll<BroadCastModel>("[dbo].[uspviewBroadcastAndContentForDashboard]",
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

        public List<BroadCastModel> GetBroadcastList(int PageNo, int PageSize, string KeyWord)
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
            var data = _customerPortalHelper.GetAll<BroadCastModel>("[dbo].[uspviewBroadcastDetails]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public long GetBroadcastListCount(string KeyWord)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("KeyWord", KeyWord, DbType.String);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<BroadCastModel>("[dbo].[uspviewBroadCastDetailscount]", dbPara, commandType: CommandType.StoredProcedure); 
            if (data != null)
            {
                return Convert.ToInt64(data.ToList().Count);
            }
            else
            {
                return 0;
            }
        }
    }
}