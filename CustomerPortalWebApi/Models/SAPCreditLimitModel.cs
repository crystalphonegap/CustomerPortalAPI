using SapNwRfc;
using System;

namespace CustomerPortalWebApi.Models
{
    public class SAPCreditLimitModel
    {
        [SapName("ZCRBAL")]
        public ZCRBAL[] ZCRBAL { get; set; }
    }

    public class ZCRBAL
    {
        [SapName("KUNNR")]
        public string KUNNR { get; set; }

        [SapName("BUKRS")]
        public string BUKRS { get; set; }

        [SapName("NAME1")]
        public string NAME1 { get; set; }

        [SapName("Z_VKBUR")]
        public string Z_VKBUR { get; set; }

        //NAME1
        [SapName("Z_BEZEI")]
        public string Z_BEZEI { get; set; }

        [SapName("KLIMK")]
        public string KLIMK { get; set; }

        [SapName("CRBAL")]
        public string CRBAL { get; set; }

        [SapName("SYDATE")]
        public DateTime SYDATE { get; set; }

        [SapName("SYTIME")]
        public string SYTIME { get; set; }
    }
}