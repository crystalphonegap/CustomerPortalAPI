using System;

namespace CustomerPortalWebApi.Models
{
    public class LedgerBalanceConfirmationDetails
    {
        public long IDbint { get; set; }

        public long HeaderIDbint { get; set; }

        public string Plantvtxt { get; set; }

        public string DocumentNovtxt { get; set; }
        public Boolean edit { get; set; }

        public DateTime? DocumentDatedate { get; set; }
        public string DocumentTypevtxt { get; set; }

        public decimal? Quantitydcl { get; set; }
        public string Materialvtxt { get; set; }
        public string ChequeNovtxt { get; set; }
        public DateTime? ChequeDatedate { get; set; }
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