using System;

namespace CustomerPortalWebApi.Models
{
    public class SPCFALedger
    {
        public string UserCodevtxt { get; set; }
        public string UserNamevtxt { get; set; }
        public DateTime PostingDatedate { get; set; }
        public string DocumentNovtxt { get; set; }
        public DateTime DocumentDatedate { get; set; }
        public string RefDocumentNovtxt { get; set; }
        public decimal Quantitydcl { get; set; }

        public string DocumentTypevtxt { get; set; }

        public decimal TDSdcl { get; set; }

        public string ItemDescvtxt { get; set; }

        public decimal Balancedcl { get; set; }
    }
}