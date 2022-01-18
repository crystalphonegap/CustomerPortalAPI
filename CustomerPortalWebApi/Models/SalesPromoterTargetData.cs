using System;

namespace CustomerPortalWebApi.Models
{
    public class SalesPromoterTargetData
    {
        public long IDbint { get; set; }

        public string SalesPromotorCodevtxt { get; set; }
        public string Monthvtxt { get; set; }
        public int? DealerTargetApptint { get; set; }
        public int? DealerActualApptint { get; set; }

        public int? RetailerTargetApptint { get; set; }

        public int? RetailerActualApptint { get; set; }

        public string CreatedByvtxt { get; set; }
        public DateTime CreatedDatetimedatetime { get; set; }

        public string Remarks { get; set; }
        public string Yeartxt { get; set; }
    }
}