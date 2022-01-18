using System;

namespace CustomerPortalWebApi.Models
{
    public class LoyalityPointsModel
    {
        public long Idbint { get; set; }

        public string Divisionvtxt { get; set; }
        public string CustomerCodevtxt { get; set; }

        public string CustomerNamevtxt { get; set; }


        public long EarnPoints { get; set; }

        public long UtilizePoints { get; set; }

        public long BalPoints { get; set; }

        public string CreatedByvtxt { get; set; }


        public DateTime? TillDateDatetime { get; set; }
        public DateTime? CreatedDateTimedatetime { get; set; }

        public string ModifyByvtxt { get; set; }

        public DateTime? ModifyDateTimedatetime { get; set; }

        public string Month { get; set; }
    }
}