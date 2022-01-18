using SapNwRfc;

namespace CustomerPortalWebApi.Models
{
    public class SAPPlantwiseStockResultModel
    {
        [SapName("ET_STK_OVERVIEW")]
        public ET_STK_OVERVIEW[] ET_STK_OVERVIEWs { get; set; }
    }

    public class ET_STK_OVERVIEW
    {
        [SapName("PLANT")]
        public string PLANT { get; set; }

        [SapName("PLANT_DESC")]
        public string PLANT_DESC { get; set; }

        [SapName("STORAGE_LOC")]
        public string STORAGE_LOC { get; set; }

        [SapName("STROAGE_DESC")]
        public string STROAGE_DESC { get; set; }

        [SapName("BATCH")]
        public string BATCH { get; set; }

        [SapName("MATERIAL")]
        public string MATERIAL { get; set; }

        [SapName("MATERIAL_DESC")]
        public string MATERIAL_DESC { get; set; }

        [SapName("UNRESTRICTED")]
        public string UNRESTRICTED { get; set; }

        [SapName("IN_QUAL_INSP")]
        public string IN_QUAL_INSP { get; set; }

        [SapName("RESTICTED_USE")]
        public string RESTICTED_USE { get; set; }

        [SapName("BLOCKED")]
        public string BLOCKED { get; set; }

        [SapName("UOM")]
        public string UOM { get; set; }
    }
}