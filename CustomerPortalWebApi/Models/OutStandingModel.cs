using System;

namespace CustomerPortalWebApi.Models
{
    public class OutStandingModel
    {
        public decimal? Advancedcl { get; set; }

        public decimal? D3dcl { get; set; }
        public decimal? D7dcl { get; set; }
        public decimal? D15dcl { get; set; }
        public decimal? D30dcl { get; set; }
        public decimal? D45dcl { get; set; }
        public decimal? D60dcl { get; set; }
        public decimal? D90dcl { get; set; }
        public decimal? gD90dcl { get; set; }
        public decimal? D120dcl { get; set; }
        public decimal? D150dcl { get; set; }

        public decimal? D180dcl { get; set; }
        public decimal? D360dcl { get; set; }
        public decimal? gD360dcl { get; set; }
        public decimal? GrossOutsatndingAmtdcl { get; set; }
        public decimal? NetOutstandingAmtdcl { get; set; }
        public DateTime? SystemDateTimedatetime { get; set; }
    }

    public class CustomerAmountModel
    {
        public decimal? OutStandingdcl { get; set; }
        public decimal? AvailableCreditLimitdcl { get; set; }
        public decimal? CreditLimitdcl { get; set; }
        public decimal? CustSecurityAmount { get; set; }
        public DateTime? SystemDateTimedatetime { get; set; }
    }
}