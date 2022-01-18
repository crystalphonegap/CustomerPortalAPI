using CustomerPortalWebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerPortalWebApi.Interface
{
    public interface IRFCCallServices
    {

        Task<string> invoicePDFDownloadAsync(string InvoiceNo);

        Task<KAMPriceApprovalSAPResponseModel> KAMFirstRequestPriceApproval(KAMPriceApprovalSAPRequestModel model);

        Task<KAMPriceApprovalSencondSAPResponseModel> KAMSecondRequestPriceApproval(KAMPriceApprovalSencondSAPRequestModel model);

        long GetCreditLimitFromRFC(string customercode);

        long GetOutStandingFromRFC(string customercode);

        long GetLedgerFromRFC(string customercode, string fromdate, string todate);

        long GetOutStandingFromRFCEmployeeWise(string usercode, string usertype);

        SAPOrderOutputModel InsertOrderIntoRFC(I_SO_DATA requstmodel);

        List<ET_STK_OVERVIEW> GetAvailableStock(I_STR_STK_OV_SRCH requstmodel);

        long GetLedgerFromRFCforSPCFA(string usercode, string username, string usertype, string fromdate, string todate);

        string InsertCDPayment(CDPaymentModel model);
    }
}