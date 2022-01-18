using SapNwRfc;

namespace CustomerPortalWebApi.Models
{
    public class SAPPlantwiseStockRequestModel
    {
        [SapName("I_STR_STK_OV_SRCH")]
        public I_STR_STK_OV_SRCH I_SO_DATAS { get; set; }

        [SapName("I_NO_ZERO")]
        public string I_NO_ZERO { get; set; }
    }

    public class I_STR_STK_OV_SRCH
    {
        [SapName("MATERIAL")]
        public string MATERIAL { get; set; }

        [SapName("PLANT_FROM")]
        public string PLANT_FROM { get; set; }

        [SapName("PLANT_TO")]
        public string PLANT_TO { get; set; }

        [SapName("BATCH_FROM")]
        public string BATCH_FROM { get; set; }

        [SapName("BATCH_TO")]
        public string BATCH_TO { get; set; }

        [SapName("STOR_LOCATION_FROM")]
        public string STOR_LOCATION_FROM { get; set; }

        [SapName("STOR_LOCATION_TO")]
        public string STOR_LOCATION_TO { get; set; }
    }
}