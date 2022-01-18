using SapNwRfc;
using System;

namespace CustomerPortalWebApi.Models
{
    public class SAPLedgerClassForSPCFA
    {
        [SapName("ZLEDGER_DETAIL")]
        public SPLEDGER_DETAIL[] LEDGER_DETAIL { get; set; }

        [SapName("ZLEDGER_HEADER")]
        public SPLEDGER_HEADER[] LEDGER_HEADER { get; set; }
    }

    public class SPLEDGER_DETAIL
    {
        [SapName("BUDAT")]
        public DateTime BUDAT { get; set; }

        [SapName("BELNR")]
        public string BELNR { get; set; }

        [SapName("BLDAT")]
        public DateTime BLDAT { get; set; }

        [SapName("XBLNR")]
        public string XBLNR { get; set; }

        [SapName("FKIMG")]
        public string FKIMG { get; set; }

        [SapName("CD")]
        public string CD { get; set; }

        [SapName("WT_QBSHH")]
        public string WT_QBSHH { get; set; }

        [SapName("SGTXT")]
        public string SGTXT { get; set; }

        [SapName("BAL")]
        public string BAL { get; set; }
    }

    public class SPLEDGER_HEADER
    {
        [SapName("NAME1")]
        public string NAME1 { get; set; }

        [SapName("OPBAL")]
        public string OPBAL { get; set; }
    }
}