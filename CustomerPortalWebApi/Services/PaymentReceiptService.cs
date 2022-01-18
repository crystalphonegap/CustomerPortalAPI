using CustomerPortalWebApi.Entities;
using CustomerPortalWebApi.Interface;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CustomerPortalWebApi.Services
{
    public class PaymentReceiptService : IPaymentReceiptService
    {
        private readonly ICustomerPortalHelper _customerPortalHelper;

        public PaymentReceiptService(ICustomerPortalHelper customerPortalHelper)
        {
            _customerPortalHelper = customerPortalHelper;
        }

        public List<PaymentReceipt> GetPaymentReceipt(string CustomerCode, int pageNo, int pageSize, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("CustomerCodevtxt", CustomerCode, DbType.String);
            dbPara.Add("PageNo", pageNo, DbType.Int32);
            dbPara.Add("PageSize", pageSize, DbType.Int32);
            dbPara.Add("KeyWord", KeyWord, DbType.String);
            var data = _customerPortalHelper.GetAll<PaymentReceipt>("[dbo].[uspviewGetPaymentReceiptByCustomerCode]", dbPara, commandType: CommandType.StoredProcedure);

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