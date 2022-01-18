using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerPortalWebApi.Models
{
    public class CustomerSalesModel
    {
        public long IDbint { get; set; }

        public string Typevtxt { get; set; }
        public string Monthvtxt { get; set; }

        public string Yearvtxt { get; set; }

        public string CustomerCodevtxt { get; set; }

        public string CustomerNamevtxt { get; set; }

        public decimal? Salesdcl { get; set; }

        public string CreatedByvtxt { get; set; }

        public DateTime? CreatedDateTimedatetime { get; set; }

        public string ModifyByvtxt { get; set; }

        public DateTime? ModifyDateTimedatetime { get; set; }

        //Use for Customer Profile
        public string YEAR { get; set; }

        public string Month { get; set; }

        public decimal? TotalActualSales { get; set; }

        public decimal? TotalTargetSales { get; set; }

        public decimal? TotalNCRSales { get; set; }
        public decimal? TotalPaymentSales { get; set; }

        public decimal? TotalSalesHistory { get; set; }

        public decimal? ChampionPlusPerHistory { get; set; }

        public decimal? DuraTechPerHistory { get; set; }

        public int TotalConsistency { get; set; }

        public int TotalEffectiveMonths { get; set; }

        public int TotalNonEffectiveMonths { get; set; }

        //Use for Customer Profile

    }
}
