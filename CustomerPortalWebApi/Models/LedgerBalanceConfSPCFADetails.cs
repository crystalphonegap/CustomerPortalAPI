using System;

namespace CustomerPortalWebApi.Models
{
    public class LedgerBalanceConfSPCFADetails
    {
        public long IDbint { get; set; }

        public long HeaderIDbint { get; set; }

        public string DocumentTypevtxt { get; set; }

        public string DocumentNovtxt { get; set; }
        public Boolean edit { get; set; }

        public DateTime? DocumentDatedate { get; set; }

        public decimal? Quantitydcl { get; set; }

        public decimal? TDSdcl { get; set; }
        public string ItemDescvtxt { get; set; }
        public string RefDocumentNovtxt { get; set; }
        public DateTime? PostingDatedate { get; set; }
        public decimal? Creditdcl { get; set; }
        public decimal? Debitdcl { get; set; }
        public decimal? Balancedcl { get; set; }

        public decimal? EditAmoutdcl { get; set; }

        public decimal? EditDebitdcl { get; set; }

        public decimal? EditCreditdcl { get; set; }

        public string Remarksvtxt { get; set; }

        public string Statusvtxt { get; set; }
        public string Narrationvtxt { get; set; }
    }
}