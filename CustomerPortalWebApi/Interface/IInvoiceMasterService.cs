using CustomerPortalWebApi.Entities;
using System.Collections.Generic;

namespace CustomerPortalWebApi.Interface
{
    public interface IInvoiceMasterService
    {
        List<InvoiceMaster> GetInvoice(string SoldToPartyCode, int pageNo, int pageSize, string KeyWord);

        List<InvoiceMaster> getAllInvoiceDataByInvoiceNo(string InvoiceNO);

        List<InvoiceMaster> getInvoiceHeaderDataByInvoiceNo(string InvoiceNO);

        long GetInvoiceCount(string SoldToPartyCode, string KeyWord);

        List<InvoiceMaster> GetInvoiceSearch(string fromdate, string todate, string status, string SoldToPartyCode, int pageNo, int pageSize, string KeyWord);

        List<InvoiceMaster> GetInvoiceDownload(string fromdate, string todate, string SoldToPartyCode, string status, string KeyWord);

        long GetInvoiceSearchCount(string fromdate, string todate, string SoldToPartyCode, string status, string KeyWord);

        List<InvoiceMaster> GetInvoiceStatuswiseCount(string fromdate, string todate, string SoldToPartyCode, string KeyWord);
    }
}