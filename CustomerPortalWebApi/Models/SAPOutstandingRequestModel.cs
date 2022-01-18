using SapNwRfc;
using System;

namespace CustomerPortalWebApi.Models
{
    //public class Z_KUNNR
    //{
    //    [SapName("Z_KUNNR")]
    //    public Z_KUNNR Z_KUNNR { get;set;}
    //}
    public class RFCOutstandingRequest
    {
        [SapName("Z_KUNNR")]
        public Z_KUNNR z { get; set; }
    }

    public class Z_KUNNR
    {
        [SapName("KUNNR_FROM")]
        public string KUNNR_FROM { get; set; }

        [SapName("KUNNR_TO")]
        public string KUNNR_TO { get; set; }
    }

    public class SAPLedgerRequest
    {
        [SapName("KUNNR")]
        public string KUNNRs { get; set; }

        [SapName("FDAT")]
        public DateTime FDATs { get; set; }

        [SapName("TDAT")]
        public DateTime TDATs { get; set; }
    }

    public class SAPLedgerRequestforSPCFA
    {
        [SapName("LIFNR")]
        public string LIFNRs { get; set; }

        [SapName("FDAT")]
        public DateTime FDATs { get; set; }

        [SapName("TDAT")]
        public DateTime TDATs { get; set; }
    }
}