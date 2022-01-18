using CustomerPortalWebApi.Interface;
using CustomerPortalWebApi.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerPortalWebApi.Services
{
    public class BalanceConfirmationService : IBalanceConfirmationService
    {
        private readonly ICustomerPortalHelper _customerPortalHelper;

        private readonly IMailService _MailService;

        public BalanceConfirmationService(ICustomerPortalHelper customerPortalHelper, IMailService mailService)
        {
            _customerPortalHelper = customerPortalHelper;
            _MailService = mailService;
        }

        public long InsertBalConfirmationIntoTempTable(BalConfirmationModel balconf)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("RequestNovtxt", balconf.RequestNovtxt, DbType.String);
            dbPara.Add("FromDatedatetime", balconf.FromDatedatetime, DbType.DateTime);
            dbPara.Add("ToDatedatetime", balconf.ToDatedatetime, DbType.DateTime);
            dbPara.Add("ExpiryDatedatetime", balconf.ExpiryDatedatetime, DbType.DateTime);
            dbPara.Add("DealerCodevtxt", balconf.DealerCodevtxt, DbType.String);
            dbPara.Add("Typevtxt", balconf.Typevtxt, DbType.String);
            dbPara.Add("CreatedByvtxt", balconf.CreatedByvtxt, DbType.String);

            #region using dapper

            var data = _customerPortalHelper.Insert<long>("[dbo].[uspInsertBalConfirmIntoTemp]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;

            #endregion using dapper
        }

        public string GetOrderNo()
        {
            var dbPara = new DynamicParameters();
            var data = _customerPortalHelper.GetAll<BalConfirmationModel>("[dbo].[uspviewgetRequestNo]", dbPara, commandType: CommandType.StoredProcedure);
            if (data != null)
            {
                return data[0].RequestNovtxt.ToString();
            }
            else
            {
                return null;
            }
        }

        public List<BalConfirmationModel> GetReqOrderNo(string ReqOrderNo)
        {
            List<BalConfirmationModel> lstOrd = new List<BalConfirmationModel>();
            BalConfirmationModel ord = new BalConfirmationModel();
            ord.RequestNovtxt = ReqOrderNo;
            lstOrd.Add(ord);

            if (lstOrd != null)
            {
                return lstOrd.ToList();
            }
            else
            {
                return null;
            } 
        }

        public long InsertBalConfirmationIntoMainTable(string strcreatedby)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("CreatedBy", strcreatedby, DbType.String);
            var data = _customerPortalHelper.Execute("[dbo].[uspInsertBalanceConfintoMain]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public long DeleteTempBalConf(string strcreatedby)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("mode", "Delete", DbType.String);
            dbPara.Add("Createdby", strcreatedby, DbType.String);
            var data = _customerPortalHelper.Execute("[dbo].[uspgetdeleteTempBalConfirm]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public List<BalConfirmationModel> GetTempBalConfirm(string strcreatedby)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("mode", "Get", DbType.String);
            dbPara.Add("Createdby", strcreatedby, DbType.String);
            var data = _customerPortalHelper.GetAll<BalConfirmationModel>("[dbo].[uspgetdeleteTempBalConfirm]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public List<BalConfirmationModel> GetBalanceConfHeaderforAccountingHead(string usercode, int PageNo, int PageSize)
        {
            var dbPara = new DynamicParameters();

            dbPara.Add("usercode", usercode, DbType.String);
            dbPara.Add("PageNo", PageNo, DbType.Int32);
            dbPara.Add("PageSize", PageSize, DbType.Int32);
            var data = _customerPortalHelper.GetAll<BalConfirmationModel>("[dbo].[uspviewBalanceConfHeaderdataByAccountingHead]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public long GetBalanceConfHeaderforAccountingHeadCount(string usercode)
        {
            var dbPara = new DynamicParameters();

            dbPara.Add("usercode", usercode, DbType.String);
            var data = _customerPortalHelper.GetAll<BalConfirmationModel>("[dbo].[uspviewBalanceConfHeaderdataByAccountingHeadcount]",
                            dbPara,
                            commandType: CommandType.StoredProcedure); 

            if (data != null)
            {
                return data.ToList().Count();
            }
            else
            {
                return 0;
            }
        }

        public List<BalConfirmationModel> GetBalanceConfDetailData(long idbint)
        {
            var dbPara = new DynamicParameters();

            dbPara.Add("idbint", idbint, DbType.Int64);
            var data = _customerPortalHelper.GetAll<BalConfirmationModel>("[dbo].[uspviewBalanceConfDetailsdata]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public List<LedgerBalanceConfirmationHeader> GetBalanceConfHeaderforCustomer(string CustomerCode, int PageNo, int PageSize, string mode, long id)
        {
            var dbPara = new DynamicParameters();
            if (mode == "List")
            {
                dbPara.Add("CustomerCode", CustomerCode, DbType.String);
                dbPara.Add("PageNo", PageNo, DbType.Int32);
                dbPara.Add("PageSize", PageSize, DbType.Int32);
                dbPara.Add("mode", mode, DbType.String);
                dbPara.Add("Id", 0, DbType.Int64);
            }
            else if (mode == "Count")
            {
                dbPara.Add("CustomerCode", CustomerCode, DbType.String);
                dbPara.Add("PageNo", 0, DbType.Int32);
                dbPara.Add("PageSize", 0, DbType.Int32);
                dbPara.Add("mode", mode, DbType.String);
                dbPara.Add("Id", 0, DbType.Int64);
            }
            else if (mode == "Detail")
            {
                dbPara.Add("CustomerCode", CustomerCode, DbType.String);
                dbPara.Add("PageNo", 0, DbType.Int32);
                dbPara.Add("PageSize", 0, DbType.Int32);
                dbPara.Add("mode", mode, DbType.String);
                dbPara.Add("Id", id, DbType.Int64);
            }
            var data = _customerPortalHelper.GetAll<LedgerBalanceConfirmationHeader>("[dbo].[uspGetBalanceConfirmationHeader]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public List<LedgerBalanceConfirmationDetails> GetBalanceConfDetailforCustomer(string CustomerCode, string mode, long id)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("CustomerCode", CustomerCode, DbType.String);
            dbPara.Add("PageNo", 0, DbType.Int32);
            dbPara.Add("PageSize", 0, DbType.Int32);
            dbPara.Add("mode", mode, DbType.String);
            dbPara.Add("Id", id, DbType.Int64);
            var data = _customerPortalHelper.GetAll<LedgerBalanceConfirmationDetails>("[dbo].[uspGetBalanceConfirmationHeader]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public long UpdateExpiryDate(BalConfirmationEditModel content)
        {
            var dbPara = new DynamicParameters();
            DateTime date = DateTime.ParseExact(content.ExpiryDatedatetime, "dd-MM-yyyy", null);
            dbPara.Add("idbint", content.IDbint, DbType.Int64);
            dbPara.Add("ExpiryDatedatetime", date, DbType.Date);
            dbPara.Add("modify", content.CreatedByvtxt, DbType.String);
            var data = _customerPortalHelper.Insert<long>("[dbo].[uspupdateExpiryBalanceConfHeaderData]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public async Task UpdateCustomerLedgerbalanceconfStatus(LedgerBalanceConfirmationHeader model)
        {
            var dbPara = new DynamicParameters();

            dbPara.Add("IDbint", model.IDbint, DbType.Int64);
            dbPara.Add("BalanceConfirmationStatus", model.BalanceConfirmationStatus, DbType.Boolean);
            dbPara.Add("BalanceConfirmationAction", model.BalanceConfirmationAction, DbType.String);
            dbPara.Add("BalanceConfirmationUser", model.BalanceConfirmationUser, DbType.String);
            dbPara.Add("Remarksvtxt", model.Remarksvtxt, DbType.String);
            dbPara.Add("AttachmentFileNamevtxt", model.AttachmentFileNamevtxt, DbType.String);
            dbPara.Add("AttachmentFilevtxt", model.AttachmentFilevtxt, DbType.String);
            dbPara.Add("AttachmentPathvtxt", model.AttachmentPathvtxt, DbType.String);
            var data = _customerPortalHelper.Insert<long>("[dbo].[uspUpdateCustomerLedgerBalance]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            await _MailService.SendEmailToCustomerBalanceConfirmation(model.CustomerCodevtxt, model.FromDatedatetime.ToString(), model.ToDatedatetime.ToString());
            UserMasterModel usermodel = GetAccountingHeadEmailID(model.RequestIdbint);
            await _MailService.SendEmailToAccountingHeadBalanceConfirmation(usermodel.UserNametxt, usermodel.UserCodetxt, usermodel.Emailvtxt, model.FromDatedatetime.ToString(), model.ToDatedatetime.ToString(), model.RequestNovtxt, model.CustomerCodevtxt);
        }

        public UserMasterModel GetAccountingHeadEmailID(long requestid)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("Requestedid", requestid, DbType.Int64);
            var data = _customerPortalHelper.GetAll<UserMasterModel>("[dbo].[uspviewEmailIdofAccountingHead]",
                            dbPara,
                            commandType: CommandType.StoredProcedure); 

            if (data != null)
            {
                return data[0];
            }
            else
            {
                return null;
            }
        }

        public long UpdateCustomerLedgerbalanceconfDetails(LedgerBalanceConfirmationDetails model)
        {
            var dbPara = new DynamicParameters();

            dbPara.Add("IDbint", model.IDbint, DbType.Int64);
            dbPara.Add("Amount", model.EditAmoutdcl, DbType.Decimal);
            dbPara.Add("Remarksvtxt", model.Remarksvtxt, DbType.String);
            dbPara.Add("Statusvtxt", model.Statusvtxt, DbType.String);
            var data = _customerPortalHelper.Insert<long>("[dbo].[UpdateCustomerLedgerBalanceDetails]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public List<LedgerBalanceConfirmationHeader> GetBalanceConfHeaderListForEmployee(string fromdate, string todate, string status, string usertype, string usercode, int PageNo, int PageSize, string KeyWord)
        {
            var dbPara = new DynamicParameters();

            DateTime fdate = DateTime.ParseExact(fromdate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(todate, "dd-MM-yyyy", null);
            dbPara.Add("fromDate", Convert.ToDateTime(fdate).ToString("yyyy-MM-dd"), DbType.String);
            dbPara.Add("todate", Convert.ToDateTime(tdate).ToString("yyyy-MM-dd"), DbType.String);
            dbPara.Add("Status", status, DbType.String);
            dbPara.Add("UserType", usertype, DbType.String);
            dbPara.Add("UserCode", usercode, DbType.String);
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
            var data = _customerPortalHelper.GetAll<LedgerBalanceConfirmationHeader>("[dbo].[uspviewbalanceconfirmHeaderByUserType]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public long GetBalanceConfHeaderListForEmployeeCount(string fromdate, string todate, string status, string usertype, string usercode, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            DateTime fdate = DateTime.ParseExact(fromdate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(todate, "dd-MM-yyyy", null);
            dbPara.Add("fromDate", Convert.ToDateTime(fdate).ToString("yyyy-MM-dd"), DbType.String);
            dbPara.Add("todate", Convert.ToDateTime(tdate).ToString("yyyy-MM-dd"), DbType.String);
            dbPara.Add("Status", status, DbType.String);
            dbPara.Add("UserType", usertype, DbType.String);
            dbPara.Add("UserCode", usercode, DbType.String);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<LedgerBalanceConfirmationDetails>("[dbo].[uspviewbalanceconfirmHeaderByUserType]",
                            dbPara,
                            commandType: CommandType.StoredProcedure); 

            if (data != null)
            {
                return data.ToList().Count;
            }
            else
            {
                return 0;
            }
        }

        //use for SPCFA balance Confirmation
        public List<LedgerBalanceConfirmationHeader> GetBalanceConfHeaderforSPCFA(string UserCode, int PageNo, int PageSize, string mode, long id)
        {
            var dbPara = new DynamicParameters();
            if (mode == "List")
            {
                dbPara.Add("UserCode", UserCode, DbType.String);
                dbPara.Add("PageNo", PageNo, DbType.Int32);
                dbPara.Add("PageSize", PageSize, DbType.Int32);
                dbPara.Add("mode", mode, DbType.String);
                dbPara.Add("Id", 0, DbType.Int64);
            }
            else if (mode == "Count")
            {
                dbPara.Add("UserCode", UserCode, DbType.String);
                dbPara.Add("PageNo", 0, DbType.Int32);
                dbPara.Add("PageSize", 0, DbType.Int32);
                dbPara.Add("mode", mode, DbType.String);
                dbPara.Add("Id", 0, DbType.Int64);
            }
            else if (mode == "Detail")
            {
                dbPara.Add("UserCode", UserCode, DbType.String);
                dbPara.Add("PageNo", 0, DbType.Int32);
                dbPara.Add("PageSize", 0, DbType.Int32);
                dbPara.Add("mode", mode, DbType.String);
                dbPara.Add("Id", id, DbType.Int64);
            }
            var data = _customerPortalHelper.GetAll<LedgerBalanceConfirmationHeader>("[dbo].[uspGetBalanceConfirmationHeaderForSP]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public List<LedgerBalanceConfSPCFADetails> GetBalanceConfDetailforSPCFA(string UserCode, string mode, long id)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("UserCode", UserCode, DbType.String);
            dbPara.Add("PageNo", 0, DbType.Int32);
            dbPara.Add("PageSize", 0, DbType.Int32);
            dbPara.Add("mode", mode, DbType.String);
            dbPara.Add("Id", id, DbType.Int64);
            var data = _customerPortalHelper.GetAll<LedgerBalanceConfSPCFADetails>("[dbo].[uspGetBalanceConfirmationHeaderForSP]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public async Task UpdateSPCFALedgerbalanceconfStatus(LedgerBalanceConfirmationHeader model)
        {
            var dbPara = new DynamicParameters();

            dbPara.Add("IDbint", model.IDbint, DbType.Int64);
            dbPara.Add("BalanceConfirmationStatus", model.BalanceConfirmationStatus, DbType.Boolean);
            dbPara.Add("BalanceConfirmationAction", model.BalanceConfirmationAction, DbType.String);
            dbPara.Add("BalanceConfirmationUser", model.BalanceConfirmationUser, DbType.String);
            dbPara.Add("Remarksvtxt", model.Remarksvtxt, DbType.String);
            dbPara.Add("AttachmentFileNamevtxt", model.AttachmentFileNamevtxt, DbType.String);
            dbPara.Add("AttachmentFilevtxt", model.AttachmentFilevtxt, DbType.String);
            dbPara.Add("AttachmentPathvtxt", model.AttachmentPathvtxt, DbType.String);
            var data = _customerPortalHelper.Insert<long>("[dbo].[uspUpdateSPCFALedgerBalance]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            await _MailService.SendEmailToCustomerBalanceConfirmation(model.CustomerCodevtxt, model.FromDatedatetime.ToString(), model.ToDatedatetime.ToString());
            UserMasterModel usermodel = GetAccountingHeadEmailID(model.RequestIdbint);
            await _MailService.SendEmailToAccountingHeadBalanceConfirmation(usermodel.UserNametxt, usermodel.UserCodetxt, usermodel.Emailvtxt, model.FromDatedatetime.ToString(), model.ToDatedatetime.ToString(), model.RequestNovtxt, model.CustomerCodevtxt);
        }

        public long UpdateSPCFALedgerbalanceconfDetails(LedgerBalanceConfSPCFADetails model)
        {
            var dbPara = new DynamicParameters();

            dbPara.Add("IDbint", model.IDbint, DbType.Int64);
            dbPara.Add("Amount", model.EditAmoutdcl, DbType.Decimal);
            dbPara.Add("Remarksvtxt", model.Remarksvtxt, DbType.String);
            dbPara.Add("Statusvtxt", model.Statusvtxt, DbType.String);
            var data = _customerPortalHelper.Insert<long>("[dbo].[UpdateSPCFALedgerBalanceDetails]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }



        public long InsertBalanceConfLog(LedgerBalanceConfirmationLog model)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("HeaderIDbint", model.HeaderIDbint, DbType.Int64);
            dbPara.Add("UserCodevtxt", model.UserCodevtxt, DbType.String);
            dbPara.Add("UserTypevtxt", model.UserTypevtxt, DbType.String);
            dbPara.Add("Remarksvtxt", model.Remarksvtxt, DbType.String);
            dbPara.Add("Statusvtxt", model.Statusvtxt, DbType.String);
            var data = _customerPortalHelper.Insert<long>("[dbo].[uspInsertBalConfirmLog]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }


        public List<LedgerBalanceConfirmationLog> GetBalanceConfLog(long HeaderIDbint)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("HeaderIDint", HeaderIDbint, DbType.Int64);
            var data = _customerPortalHelper.GetAll<LedgerBalanceConfirmationLog>("[dbo].[uspGetBalanceConfLog]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public List<LedgerBalanceConfirmationHeader> GetSPCFABalanceConfHeaderListForEmployee(string fromdate, string todate, string status, string usertype, string usercode, int PageNo, int PageSize, string KeyWord)
        {
            var dbPara = new DynamicParameters();

            DateTime fdate = DateTime.ParseExact(fromdate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(todate, "dd-MM-yyyy", null);
            dbPara.Add("fromDate", Convert.ToDateTime(fdate).ToString("yyyy-MM-dd"), DbType.String);
            dbPara.Add("todate", Convert.ToDateTime(tdate).ToString("yyyy-MM-dd"), DbType.String);
            dbPara.Add("Status", status, DbType.String);
            dbPara.Add("UserType", usertype, DbType.String);
            dbPara.Add("UserCode", usercode, DbType.String);
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
            var data = _customerPortalHelper.GetAll<LedgerBalanceConfirmationHeader>("[dbo].[uspviewSPCFAbalanceconfirmHeaderByUserType]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public long GetSPCFABalanceConfHeaderListForEmployeeCount(string fromdate, string todate, string status, string usertype, string usercode, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            DateTime fdate = DateTime.ParseExact(fromdate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(todate, "dd-MM-yyyy", null);
            dbPara.Add("fromDate", Convert.ToDateTime(fdate).ToString("yyyy-MM-dd"), DbType.String);
            dbPara.Add("todate", Convert.ToDateTime(tdate).ToString("yyyy-MM-dd"), DbType.String);
            dbPara.Add("Status", status, DbType.String);
            dbPara.Add("UserType", usertype, DbType.String);
            dbPara.Add("UserCode", usercode, DbType.String);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<LedgerBalanceConfirmationDetails>("[dbo].[uspviewSPCFAbalanceconfirmHeaderByUserTypeCount]",
                            dbPara,
                            commandType: CommandType.StoredProcedure); 
            if (data != null)
            {
                return data.ToList().Count;
            }
            else
            {
                return 0;
            }
        }

        public LedgerBalanceConfirmationHeader GetSPCFABalanceConfHeaderDetailForEmployee(string Mode, int ID)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("Mode", Mode, DbType.String);
            dbPara.Add("ID", ID, DbType.Int64);
            var data = _customerPortalHelper.Get<LedgerBalanceConfirmationHeader>("[dbo].[uspviewbalanceconfirmHeaderDetail]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }
    }
}