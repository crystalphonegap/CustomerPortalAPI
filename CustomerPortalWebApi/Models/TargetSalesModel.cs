using System;

namespace CustomerPortalWebApi.Models
{
    public class TargetSalesModel
    {
        public long IDbint { get; set; }

        public string Divisionvtxt { get; set; }

        public string Monthvtxt { get; set; }

        public int Yearvtxt { get; set; }

        public string CustomerCodevtxt { get; set; }

        public string CustomerNamevtxt { get; set; }

        public decimal? TargetSalesdcl { get; set; }

        public string CreatedByvtxt { get; set; }

        public DateTime? CreatedDateTimedatetime { get; set; }

        public string ModifyByvtxt { get; set; }

        public DateTime? ModifyDateTimedatetime { get; set; }

        public decimal? TargetSales { get; set; }

        public decimal? ActualSales { get; set; }

        public decimal? Achivement { get; set; }

        public decimal? TotalActualSales { get; set; }

        public decimal? TotalTargetSales { get; set; }

        //New Added Premium branded Wise 
        public decimal? PremiumSalesChamp { get; set; }

        public decimal? PremiumSalesChampPlus { get; set; }

        public decimal? PremiumSalesDuratech { get; set; }

        public decimal? PremiumSalesChampPer { get; set; }
        public decimal? PremiumSalesChampPlusPer { get; set; }
        public decimal? PremiumSalesDuratechPer { get; set; }

        //New Added Premium branded Wise 


        public string Month { get; set; }

        public decimal? PremiumSales { get; set; }

        public decimal? PremiumSalesPer { get; set; }

        public int MyARS { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public decimal? SortPoll { get; set; }
    }
}