using SapNwRfc;
using System;
using System.Collections.Generic;

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

    public class LEDGER_HEADERNEW1
    {

        public List<LEDGER_HEADERNEW> LEDGER_HEADERNEW { get; set; }

    }

    public class LEDGER_HEADERNEW
    {
       
        public string KUNNR { get; set; }
        public string NAME1 { get; set; }
        public string STRAS { get; set; }
        public string ORT01 { get; set; }
        public string STCD3 { get; set; }
        public string J_1IPANNO { get; set; }
        public string KLIMK { get; set; }
        public decimal TOTQTY2 { get; set; }
        public decimal DMBTR { get; set; }
        public decimal SD_DMBTR { get; set; }
    }

    public class LEDGER_DETAILSNEW
    {

        public string BUDAT { get; set; }
        public string BLART { get; set; }
        public string WERKS { get; set; }
        public string BELNR { get; set; }
        public string MATWA { get; set; }
        public string FKIMG { get; set; }
        public string SGTXT { get; set; }
        public decimal BLDAT { get; set; }
        public decimal DMBTRD { get; set; }
        public decimal DMBTRC { get; set; }
        public decimal BAL { get; set; }
    }
}