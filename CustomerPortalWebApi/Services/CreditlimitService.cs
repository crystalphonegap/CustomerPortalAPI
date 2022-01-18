using CustomerPortalWebApi.Interface;
using CustomerPortalWebApi.Models;
using Dapper;
using System;
using System.Data;

namespace CustomerPortalWebApi.Services
{
    public class CreditlimitService : ICreditlimitService
    {
        private readonly ICustomerPortalHelper _customerPortalHelper;

        public CreditlimitService(ICustomerPortalHelper customerPortalHelper)
        {
            _customerPortalHelper = customerPortalHelper;
        }

        public Decimal GetCreditlimit(string SoldToPartyCodevtxt)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("CustomerCodevtxt", SoldToPartyCodevtxt, DbType.String);

            var data = _customerPortalHelper.GetAll<CustomerMasterModel>("[dbo].[uspviewGetAvailableCreditLimitByCustomerCode]", dbPara, commandType: CommandType.StoredProcedure);
           
            if (data != null)
            {
                return Convert.ToDecimal(data[0].CreditLimitdcl);
            }
            else
            {
                return 0;
            }
        }

        public Decimal GetAvailableCreditlimit(string SoldToPartyCodevtxt)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("CustomerCodevtxt", SoldToPartyCodevtxt, DbType.String);

            var data = _customerPortalHelper.GetAll<CustomerMasterModel>("[dbo].[uspviewGetAvailableCreditLimitByCustomerCode]", dbPara, commandType: CommandType.StoredProcedure); 
            if (data != null)
            {
                return Convert.ToDecimal(data[0].AvailableCreditLimitdcl);
            }
            else
            {
                return 0;
            }
        }
    }
}