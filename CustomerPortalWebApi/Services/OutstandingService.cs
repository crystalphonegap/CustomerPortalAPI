using CustomerPortalWebApi.Interface;
using CustomerPortalWebApi.Models;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CustomerPortalWebApi.Services
{
    public class OutstandingService : IOutstandingService
    {
        private readonly ICustomerPortalHelper _customerPortalHelper;

        public OutstandingService(ICustomerPortalHelper customerPortalHelper)
        {
            _customerPortalHelper = customerPortalHelper;
        }

        public List<CustomerAmountModel> GetOutstanding(string CustomerCode)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("CustomerCodevtxt", CustomerCode, DbType.String);

            //var data = _customerPortalHelper.GetAll<OutStandingModel>("[dbo].[uspviewGetOutstandingByCustomerCode]", dbPara, commandType: CommandType.StoredProcedure);
            var data = _customerPortalHelper.GetAll<CustomerAmountModel>("[dbo].[uspviewGetCustomerCodeAmount]", dbPara, commandType: CommandType.StoredProcedure);
            return data ;
        }

        public List<OutStandingModel> GetOutStandingData(string customercode)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("customercode", customercode, DbType.String);

            var data = _customerPortalHelper.GetAll<OutStandingModel>("[dbo].[uspviewOutStandingByCustomercode]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }
    }
}