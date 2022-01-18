using SapNwRfc;
using System;

namespace CustomerPortalWebApi.Models
{
    public class SAPLedgerClass
    {
        [SapName("LEDGER_DETAIL")]
        public LEDGER_DETAIL[] LEDGER_DETAIL { get; set; }

        [SapName("LEDGER_HEADER")]
        public LEDGER_HEADER[] LEDGER_HEADER { get; set; }
    }

    public class LEDGER_DETAIL
    {
        [SapName("BLART")]
        public string BLART { get; set; }

        [SapName("WERKS")]
        public string WERKS { get; set; }

        [SapName("BELNR")]
        public string BELNR { get; set; }

        [SapName("MATWA")]
        public string MATWA { get; set; }

        [SapName("FKIMG")]
        public string FKIMG { get; set; }

        [SapName("SGTXT")]
        public string SGTXT { get; set; }

        [SapName("BLDAT")]
        public DateTime BLDAT { get; set; }

        [SapName("DMBTRD")]
        public string DMBTRD { get; set; }

        [SapName("DMBTRC")]
        public string DMBTRC { get; set; }

        [SapName("BAL")]
        public string BAL { get; set; }

        [SapName("BUDAT")]
        public DateTime BUDAT { get; set; }

         
    }

    public class LEDGER_HEADER
    {
        [SapName("NAME1")]
        public string NAME1 { get; set; }

        [SapName("DMBTR")]
        public string DMBTR { get; set; }
    }
}