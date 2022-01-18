using CustomerPortalWebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerPortalWebApi.Interface
{
    public interface IBalanceConfirmationService
    {
        long InsertBalConfirmationIntoTempTable(BalConfirmationModel balconf);

        long InsertBalConfirmationIntoMainTable(string strcreatedby);

        long DeleteTempBalConf(string strcreatedby);

        string GetOrderNo();

        List<BalConfirmationModel> GetReqOrderNo(string ReqOrderNo);

        List<BalConfirmationModel> GetTempBalConfirm(string strcreatedby);

        List<BalConfirmationModel> GetBalanceConfHeaderforAccountingHead(string usercode, int PageNo, int PageSize);

        long GetBalanceConfHeaderforAccountingHeadCount(string usercode);

        List<BalConfirmationModel> GetBalanceConfDetailData(long idbint);

        List<LedgerBalanceConfirmationHeader> GetBalanceConfHeaderforCustomer(string CustomerCode, int PageNo, int PageSize, string mode, long id);

        List<LedgerBalanceConfirmationDetails> GetBalanceConfDetailforCustomer(string CustomerCode, string mode, long id);

        long UpdateExpiryDate(BalConfirmationEditModel content);

        Task UpdateCustomerLedgerbalanceconfStatus(LedgerBalanceConfirmationHeader model);

        long UpdateCustomerLedgerbalanceconfDetails(LedgerBalanceConfirmationDetails model);

        List<LedgerBalanceConfirmationHeader> GetBalanceConfHeaderListForEmployee(string fromdate, string todate, string status, string usertype, string usercode, int PageNo, int PageSize, string KeyWord);

        long GetBalanceConfHeaderListForEmployeeCount(string fromdate, string todate, string status, string usertype, string usercode, string KeyWord);

        List<LedgerBalanceConfirmationHeader> GetBalanceConfHeaderforSPCFA(string UserCode, int PageNo, int PageSize, string mode, long id);

        List<LedgerBalanceConfSPCFADetails> GetBalanceConfDetailforSPCFA(string UserCode, string mode, long id);

        Task UpdateSPCFALedgerbalanceconfStatus(LedgerBalanceConfirmationHeader model);

        long UpdateSPCFALedgerbalanceconfDetails(LedgerBalanceConfSPCFADetails model);

        List<LedgerBalanceConfirmationHeader> GetSPCFABalanceConfHeaderListForEmployee(string fromdate, string todate, string status, string usertype, string usercode, int PageNo, int PageSize, string KeyWord);

        long GetSPCFABalanceConfHeaderListForEmployeeCount(string fromdate, string todate, string status, string usertype, string usercode, string KeyWord);

        LedgerBalanceConfirmationHeader GetSPCFABalanceConfHeaderDetailForEmployee(string Mode, int ID);

        long InsertBalanceConfLog(LedgerBalanceConfirmationLog model);

        List<LedgerBalanceConfirmationLog> GetBalanceConfLog(long HeaderIDbint);
    }
}