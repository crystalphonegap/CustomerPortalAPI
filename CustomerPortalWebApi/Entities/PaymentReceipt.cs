using System;

namespace CustomerPortalWebApi.Entities
{
    public class PaymentReceipt
    {
        public long Idbint { get; set; }
        public string CompanyCodevtxt { get; set; }
        public string CustomerCodevtxt { get; set; }
        public string CustomerNamevtxt { get; set; }
        public string RefenrenceNovtxt { get; set; }
        public string DocumentNovtxt { get; set; }
        public string ItemDescvtxt { get; set; }
        public decimal? Amountdcl { get; set; }
        public string ClearingNovtxt { get; set; }
        public DateTime? ClearingDatedate { get; set; }
        public DateTime? PostingDatedate { get; set; }
        public DateTime? SystemDateTimedatetime { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string KeyWord { get; set; }
    }
}