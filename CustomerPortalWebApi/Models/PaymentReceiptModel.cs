using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerPortalWebApi.Models
{
    public class PaymentReceiptModel
    {
        public long Idbint { get; set; }
        public string CompanyCodevtxt { get; set; }
        public string CustomerCodevtxt { get; set; }
        public string CustomerNamevtxt { get; set; }
        public string RefenrenceNovtxt { get; set; }
        public string DocumentNovtxt { get; set; }
        public string ItemDescvtxt { get; set; }

        [RegularExpression(@"^\d+\.\d{0,2}$")]
        public decimal? Amountdcl { get; set; }

        public string ClearingNovtxt { get; set; }
        public DateTime? ClearingDatedate { get; set; }
        public DateTime? PostingDatedate { get; set; }
        public DateTime? SystemDateTimedatetime { get; set; }
    }
}