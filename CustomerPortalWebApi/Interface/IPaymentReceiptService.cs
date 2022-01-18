using CustomerPortalWebApi.Entities;
using System.Collections.Generic;

namespace CustomerPortalWebApi.Interface
{
    public interface IPaymentReceiptService
    {
        List<PaymentReceipt> GetPaymentReceipt(string CustomerCode, int pageNo, int pageSize, string KeyWord);
    }
}