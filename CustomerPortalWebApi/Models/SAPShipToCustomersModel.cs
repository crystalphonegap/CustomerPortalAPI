using SapNwRfc;
using System;

namespace CustomerPortalWebApi.Models
{
    public class SAPShipToCustomersModel
    {
        [SapName("ZCUSTMST")]
        public ZCUSTMSTS[] ZSALEORD { get; set; }

        //[SapName("BUTXT")]
        //public string BUTXT { get; set; }

        //[SapName("KUNNR")]
        //public string KUNNR { get; set; }

        //[SapName("NAME1")]
        //public string NAME1 { get; set; }
    }

    public class ZCUSTMSTS
    {
        [SapName("BUKRS")]
        public string BUKRS { get; set; }

        [SapName("BUTXT")]
        public string BUTXT { get; set; }

        [SapName("KUNNR")]
        public string KUNNR { get; set; }

        //NAME1
        [SapName("NAME1")]
        public string NAME1 { get; set; }

        [SapName("KUNAG")]
        public string KUNAG { get; set; }

        [SapName("NAME2")]
        public string NAME2 { get; set; }

        [SapName("STREET")]
        public string STREET { get; set; }

        [SapName("CITY1")]
        public string CITY1 { get; set; }

        [SapName("BSTNK")]
        public string BSTNK { get; set; }

        [SapName("VBELN")]
        public string VBELN { get; set; }

        [SapName("AUART")]
        public string AUART { get; set; }

        [SapName("ERDAT")]
        public DateTime ERDAT { get; set; }

        [SapName("VKORG")]
        public string VKORG { get; set; }

        [SapName("VKORG_NM")]
        public string VKORG_NM { get; set; }

        [SapName("VKBUR")]
        public string VKBUR { get; set; }

        [SapName("VKBUR_NM")]
        public string VKBUR_NM { get; set; }

        [SapName("VTWEG")]
        public string VTWEG { get; set; }

        [SapName("VTWEG_NM")]
        public string VTWEG_NM { get; set; }

        [SapName("SPART")]
        public string SPART { get; set; }

        [SapName("SPART_NM")]
        public string SPART_NM { get; set; }

        [SapName("POSNR")]
        public string POSNR { get; set; }

        [SapName("MATNR")]
        public string MATNR { get; set; }

        [SapName("MAKTX")]
        public string MAKTX { get; set; }

        [SapName("KWMENG")]
        public decimal KWMENG { get; set; }

        [SapName("MEINS")]
        public string MEINS { get; set; }

        [SapName("NETWR")]
        public decimal NETWR { get; set; }

        [SapName("WBSTK")]
        public string WBSTK { get; set; }

        [SapName("SYDATE")]
        public DateTime SYDATE { get; set; }

        [SapName("SYTIME")]
        public string SYTIME { get; set; }
    }
}