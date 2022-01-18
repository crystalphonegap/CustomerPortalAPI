using CustomerPortalWebApi.Entities;
using CustomerPortalWebApi.Interface;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CustomerPortalWebApi.Services
{
    public class InvoiceMasterService : IInvoiceMasterService
    {
        private readonly ICustomerPortalHelper _customerPortalHelper;

        public InvoiceMasterService(ICustomerPortalHelper customerPortalHelper)
        {
            _customerPortalHelper = customerPortalHelper;
        }

        public List<InvoiceMaster> GetInvoice(string SoldToPartyCode, int pageNo, int pageSize, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("SoldToPartyCodevtxt", SoldToPartyCode, DbType.String);
            dbPara.Add("PageNo", pageNo, DbType.Int32);
            dbPara.Add("PageSize", pageSize, DbType.Int32);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<InvoiceMaster>("[dbo].[uspviewGetInvoiceBySoldToPartyCode]", dbPara, commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public List<InvoiceMaster> getAllInvoiceDataByInvoiceNo(string InvoiceNo)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("InvoiceNo", InvoiceNo, DbType.String);
            var data = _customerPortalHelper.GetAll<InvoiceMaster>("[dbo].[uspviewGetInvoiceByInvoiceNo]", dbPara, commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public List<InvoiceMaster> getInvoiceHeaderDataByInvoiceNo(string InvoiceNo)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("InvoiceNo", InvoiceNo, DbType.String);
            var data = _customerPortalHelper.GetAll<InvoiceMaster>("[dbo].[uspviewGetInvoiceHeaderByInvoiceNo]", dbPara, commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public long GetInvoiceCount(string SoldToPartyCode, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("SoldToPartyCodevtxt", SoldToPartyCode, DbType.String);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<Count>("[dbo].[uspviewgetAllInvoiceOrderCount]", dbPara, commandType: CommandType.StoredProcedure);
            if (data != null)
            {
                return Convert.ToInt64(data[0].ListCount);
            }
            else
            {
                return 0;
            }
        }

        public List<InvoiceMaster> GetInvoiceSearch(string fromdate, string todate, string status, string SoldToPartyCode, int pageNo, int pageSize, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            DateTime fdate = DateTime.ParseExact(fromdate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(todate, "dd-MM-yyyy", null);
            dbPara.Add("fromdate", Convert.ToDateTime(fdate).ToString("yyyy-MM-dd"), DbType.DateTime);
            dbPara.Add("todate", Convert.ToDateTime(tdate).ToString("yyyy-MM-dd"), DbType.DateTime);
            if (status == "All")
            {
                dbPara.Add("status", "", DbType.String);
            }
            else
            {
                dbPara.Add("status", status, DbType.String);
            }
            dbPara.Add("SoldToPartyCodevtxt", SoldToPartyCode, DbType.String);
            dbPara.Add("PageNo", pageNo, DbType.Int32);
            dbPara.Add("PageSize", pageSize, DbType.Int32);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<InvoiceMaster>("[dbo].[uspviewGetInvoiceBySoldToPartyCodeSearch]", dbPara, commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public List<InvoiceMaster> GetInvoiceDownload(string fromdate, string todate, string status, string SoldToPartyCode, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            DateTime fdate = DateTime.ParseExact(fromdate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(todate, "dd-MM-yyyy", null);
            dbPara.Add("fromdate", Convert.ToDateTime(fdate).ToString("yyyy-MM-dd"), DbType.DateTime);
            dbPara.Add("todate", Convert.ToDateTime(tdate).ToString("yyyy-MM-dd"), DbType.DateTime);

            if (status == "All")
            {
                dbPara.Add("status", "", DbType.String);
            }
            else
            {
                dbPara.Add("status", status, DbType.String);
            }
            dbPara.Add("SoldToPartyCodevtxt", SoldToPartyCode, DbType.String);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<InvoiceMaster>("[dbo].[uspviewGetInvoiceBySoldToPartyCodeDownload]", dbPara, commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public long GetInvoiceSearchCount(string fromdate, string todate, string status, string SoldToPartyCode, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            DateTime fdate = DateTime.ParseExact(fromdate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(todate, "dd-MM-yyyy", null);
            dbPara.Add("fromdate", Convert.ToDateTime(fdate).ToString("yyyy-MM-dd"), DbType.DateTime);
            dbPara.Add("todate", Convert.ToDateTime(tdate).ToString("yyyy-MM-dd"), DbType.DateTime);

            if (status == "All")
            {
                dbPara.Add("status", "", DbType.String);
            }
            else
            {
                dbPara.Add("status", status, DbType.String);
            }
            dbPara.Add("SoldToPartyCodevtxt", SoldToPartyCode, DbType.String);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<InvoiceMaster>("[dbo].[uspviewgetAllInvoiceOrderCount]", dbPara, commandType: CommandType.StoredProcedure);
           
            if (data != null)
            {
                return Convert.ToInt64(data.ToList().Count);
            }
            else
            {
                return 0;
            }
        }

        public List<InvoiceMaster> GetInvoiceStatuswiseCount(string fromdate, string todate, string SoldToPartyCode, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            DateTime fdate = DateTime.ParseExact(fromdate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(todate, "dd-MM-yyyy", null);
            dbPara.Add("fromdate", Convert.ToDateTime(fdate).ToString("yyyy-MM-dd"), DbType.DateTime);
            dbPara.Add("todate", Convert.ToDateTime(tdate).ToString("yyyy-MM-dd"), DbType.DateTime);
            dbPara.Add("partycode", SoldToPartyCode, DbType.String);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<InvoiceMaster>("[dbo].[uspviewGetInvoiceStatuswiseCount]", dbPara, commandType: CommandType.StoredProcedure);

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