using CustomerPortalWebApi.Interface;
using CustomerPortalWebApi.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CustomerPortalWebApi.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ICustomerPortalHelper _customerPortalHelper;

        public EmployeeService(ICustomerPortalHelper customerPortalHelper)
        {
            _customerPortalHelper = customerPortalHelper;
        }

        public List<EmployeeDashboardCountModel> GetEmployeeDashboardCounts(string usercode, string usertype, string Type)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("usercode", usercode, DbType.String);
            dbPara.Add("usertype", usertype, DbType.String);
            dbPara.Add("Trade", Type, DbType.String);
            var data = _customerPortalHelper.GetAll<EmployeeDashboardCountModel>("[dbo].[uspviewEmployeeDashboardcount]",
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

        public List<TargetSalesModel> GetEmployeeWiseTargetVsActualSalesData(string usercode, string usertype, string date,string Type)
        {
            var dbPara = new DynamicParameters();

            DateTime tempdate = DateTime.ParseExact(date, "dd-MM-yyyy", null);
            dbPara.Add("usercode", usercode, DbType.String);
            dbPara.Add("usertype", usertype, DbType.String);
            dbPara.Add("date", Convert.ToDateTime(tempdate).ToString("yyyy-MM-dd"), DbType.DateTime);
            dbPara.Add("Trade", Type, DbType.String);
            var data = _customerPortalHelper.GetAll<TargetSalesModel>("[dbo].[uspviewGetEmployeeTargetSalesByYearly]",
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

        public List<TargetSalesModel> GetEmployeeWiseReport(string mode, string code, string date,string Type)
        {
            var dbPara = new DynamicParameters();
            DateTime tempdate = DateTime.ParseExact(date, "dd-MM-yyyy", null);
            dbPara.Add("mode", mode, DbType.String);
            dbPara.Add("code", code, DbType.String);
            dbPara.Add("date", Convert.ToDateTime(tempdate).ToString("yyyy-MM-dd"), DbType.DateTime);
            dbPara.Add("Trade", Type, DbType.String);
            var data = _customerPortalHelper.GetAll<TargetSalesModel>("[dbo].[uspviewEmployeeWiseReport]",
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

        public List<TargetSalesModel> GetAccountingHeadEmployeeWiseReport(string mode, string code, string fdate,string tdate)
        {
            var dbPara = new DynamicParameters();
            DateTime fromdate = DateTime.ParseExact(fdate, "dd-MM-yyyy", null);
            DateTime todate = DateTime.ParseExact(tdate, "dd-MM-yyyy", null);
            dbPara.Add("mode", mode, DbType.String);
            dbPara.Add("code", code, DbType.String);
            dbPara.Add("fdate", Convert.ToDateTime(fromdate).ToString("yyyy-MM-dd"), DbType.DateTime);
            dbPara.Add("tdate", Convert.ToDateTime(todate).ToString("yyyy-MM-dd"), DbType.DateTime);
            var data = _customerPortalHelper.GetAll<TargetSalesModel>("[dbo].[uspviewAccountingHeadEmployeeWiseReport]",
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

        public List<TargetSalesModel> GetAreaNameByAreaCode(string mode, string code)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("mode", mode, DbType.String);
            dbPara.Add("code", code, DbType.String);
            var data = _customerPortalHelper.GetAll<TargetSalesModel>("[dbo].[uspGetAreaNameByAreaCode]",
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

        public List<TargetSalesModel> GetEmployeeWiseSalesCount(string usercode, string usertype, string date,string Type)
        {
            var dbPara = new DynamicParameters();
            DateTime tempdate = DateTime.ParseExact(date, "dd-MM-yyyy", null);
            dbPara.Add("usercode", usercode, DbType.String);
            dbPara.Add("usertype", usertype, DbType.String);
            dbPara.Add("date", Convert.ToDateTime(tempdate).ToString("yyyy-MM-dd"), DbType.DateTime);
            dbPara.Add("Trade", Type, DbType.String);
            var data = _customerPortalHelper.GetAll<TargetSalesModel>("[dbo].[uspviewEmployeeWiseTargetSalesByMonthly]",
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

        public List<CFSPOustandingCountModel> GetCFSPOustingDataCount(string usercode, string usertype, string mode)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("usercode", usercode, DbType.String);
            dbPara.Add("usertype", usertype, DbType.String);
            dbPara.Add("mode", mode, DbType.String);
            var data = _customerPortalHelper.GetAll<CFSPOustandingCountModel>("[dbo].[uspgetnodayswiseoutstandingdata]",
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

        public List<CFSPOustandingListModel> GetCFSPOustingDataList(string usercode, string usertype, string mode)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("usercode", usercode, DbType.String);
            dbPara.Add("usertype", usertype, DbType.String);
            dbPara.Add("mode", mode, DbType.String);
            var data = _customerPortalHelper.GetAll<CFSPOustandingListModel>("[dbo].[uspgetnodayswiseoutstandingdata]",
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



        public List<SalesHierachy> GetSalesHierachyforRAH(string usercode, string usertype, string mode,string search)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("UserType", usertype, DbType.String);
            dbPara.Add("UserCode", usercode, DbType.String);
            dbPara.Add("Mode", mode, DbType.String);
            dbPara.Add("Search", search, DbType.String);
            var data = _customerPortalHelper.GetAll<SalesHierachy>("[dbo].[USP_GetSalesHierarchyforRCH]",
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

        public List<CFSPOustandingListModel> GetDealerListInEmployeeDashboard(string usercode, string usertype, string date, string mode,string Trade, string FillterType)
        {
            var dbPara = new DynamicParameters();
            DateTime tempdate = DateTime.ParseExact(date, "dd-MM-yyyy", null);
            dbPara.Add("usercode", usercode, DbType.String);
            dbPara.Add("usertype", usertype, DbType.String);
            dbPara.Add("mode", mode, DbType.String);
            dbPara.Add("date", Convert.ToDateTime(tempdate).ToString("yyyy-MM-dd"), DbType.DateTime);
            dbPara.Add("Trade", Trade, DbType.String);
            dbPara.Add("Type", FillterType, DbType.String);
            var data = _customerPortalHelper.GetAll<CFSPOustandingListModel>("[dbo].[uspGETDealerListEmployeeWise]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                if(data.Count > 0)
                return data.ToList();
                else
                    return null;
            }
            else
            {
                return null;
            }
        }
    }
}