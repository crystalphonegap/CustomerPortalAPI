using CustomerPortalWebApi.Models;
using System.Collections.Generic;

namespace CustomerPortalWebApi.Interface
{
    public interface IOutstandingService
    {
        List<CustomerAmountModel> GetOutstanding(string CustomerCode);

        List<OutStandingModel> GetOutStandingData(string customercode);
    }
}