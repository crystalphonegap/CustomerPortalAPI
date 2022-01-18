using System;

namespace CustomerPortalWebApi.Interface
{
    public interface ICreditlimitService
    {
        Decimal GetCreditlimit(string CustomerCode);

        Decimal GetAvailableCreditlimit(string SoldToPartyCodevtxt);
    }
}