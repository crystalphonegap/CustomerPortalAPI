using System;

namespace CustomerPortalWebApi.Models
{
    public class SAPShipToRequest
    {
        [SapNwRfc.SapName("S_ERDAT")]
        public requset S_ERDAT { get; set; }
    }

    public class requset
    {
        [SapNwRfc.SapName("DATE_TO")]
        public DateTime DATE_TO { get; set; }

        [SapNwRfc.SapName("DATE_FROM")]
        public DateTime DATE_FROM { get; set; }
    }
}