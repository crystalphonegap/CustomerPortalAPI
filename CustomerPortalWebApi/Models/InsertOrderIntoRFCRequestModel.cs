using SapNwRfc;
using System;

namespace CustomerPortalWebApi.Models
{
    public class InsertOrderIntoRFCRequestModel
    {
        [SapName("I_SO_DATA")]
        public I_SO_DATA I_SO_DATAS { get; set; }
    }

    public class I_SO_DATA
    {
        [SapName("ORD_TYPE")]
        public string ORD_TYPE { get; set; }

        [SapName("SALES_ORG")]
        public string SALES_ORG { get; set; }

        [SapName("DIST_CHNL")]
        public string DIST_CHNL { get; set; }

        [SapName("DIVISION")]
        public string DIVISION { get; set; }

        [SapName("SOLD_TO")]
        public string SOLD_TO { get; set; }

        [SapName("SHIP_TO")]
        public string SHIP_TO { get; set; }

        [SapName("PO_NUM")]
        public string PO_NUM { get; set; }

        [SapName("PO_DATE")]
        public string PO_DATE { get; set; }

        [SapName("DOC_DATE")]
        public DateTime DOC_DATE { get; set; }

        [SapName("INCO_TERMS1")]
        public string INCO_TERMS1 { get; set; }

        [SapName("INCO_TERMS2")]
        public string INCO_TERMS2 { get; set; }

        [SapName("PRICE_LIST")]
        public string PRICE_LIST { get; set; }

        [SapName("TRANS_GROUP")]
        public string TRANS_GROUP { get; set; }

        [SapName("MAT_NUM")]
        public string MAT_NUM { get; set; }

        [SapName("QTY")]
        public string QTY { get; set; }

        [SapName("DEVLY_PLANT")]
        public string DEVLY_PLANT { get; set; }

        [SapName("WEB_ORD")]
        public string WEB_ORD { get; set; }

        [SapName("SP_CODE")]
        public string SP_CODE { get; set; }
    }
}