using System;

namespace CustomerPortalWebApi.Models
{
    public class CustomerLedger
    {
        public long IDbint { get; set; }
        public string CustomerCodevtxt { get; set; }
        public string DocumentTypevtxt { get; set; }
        public string DocumentNovtxt { get; set; }
        public string Plantvtxt { get; set; }
        public string Narrationvtxt { get; set; }
        public DateTime? DocumentDatedate { get; set; }
        public decimal? Quantitydcl { get; set; }
        public string Materialvtxt { get; set; }
        public string ChequeNovtxt { get; set; }
        public DateTime? ChequeDatedate { get; set; }
        public decimal? Creditdcl { get; set; }
        public decimal? Debitdcl { get; set; }
        public decimal? Balancedcl { get; set; }
    }
}