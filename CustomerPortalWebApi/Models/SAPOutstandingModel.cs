using SapNwRfc;
using System;

namespace CustomerPortalWebApi.Models
{
    public class SAPOutstandingModel
    {
        [SapName("ZCUSTAGE")]
        public ZCUSTAGES[] ZCUSTAGE { get; set; }
    }

    public class ZCUSTAGES
    {
        [SapName("BUKRS")]
        public string BUKRS { get; set; }

        [SapName("BUTXT")]
        public string BUTXT { get; set; }

        [SapName("KUNNR")]
        public string KUNNR { get; set; }

        //NAME1
        [SapName("KUNNR_NM")]
        public string KUNNR_NM { get; set; }

        [SapName("VKBUR")]
        public string VKBUR { get; set; }

        [SapName("VKBUR_NM")]
        public string VKBUR_NM { get; set; }

        [SapName("KKBER")]
        public string KKBER { get; set; }

        [SapName("KKBER_NM")]
        public string KKBER_NM { get; set; }

        [SapName("D3")]
        public string D3 { get; set; }

        [SapName("D7")]
        public string D7 { get; set; }

        [SapName("D15")]
        public string D15 { get; set; }

        [SapName("D30")]
        public string D30 { get; set; }

        [SapName("D45")]
        public string D45 { get; set; }

        [SapName("D60")]
        public string D60 { get; set; }

        [SapName("D90")]
        public string D90 { get; set; }

        [SapName("D120")]
        public string D120 { get; set; }

        [SapName("D150")]
        public string D150 { get; set; }

        [SapName("D180")]
        public string D180 { get; set; }

        [SapName("D360")]
        public string D360 { get; set; }

        [SapName("D360_MORE")]
        public string D360_MORE { get; set; }

        [SapName("GRAMT_BAL")]
        public string GRAMT_BAL { get; set; }

        [SapName("NETAMT_BAL")]
        public string NETAMT_BAL { get; set; }

        [SapName("SYDATE")]
        public DateTime SYDATE { get; set; }

        [SapName("SYTIME")]
        public string SYTIME { get; set; }
    }
}