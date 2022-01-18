using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerPortalWebApi.Entities
{
    public class CustomerCreditlimit
    {
        public long Idbint { get; set; }
        public string CompanyCodevtxt { get; set; }
        public string CustomerCodevtxt { get; set; }
        public string CustomerNamevtxt { get; set; }
        public string CreditControlCodevtxt { get; set; }
        public string CreditControlDescvtxt { get; set; }

        [RegularExpression(@"^\d+\.\d{0,2}$")]
        public decimal? CreditLimitdcl { get; set; }

        [RegularExpression(@"^\d+\.\d{0,2}$")]
        public decimal? AvailableCreditLimitdcl { get; set; }

        public DateTime? SystemDateTimedatetime { get; set; }
        public string SystemTimedatetime { get; set; }
    }
}