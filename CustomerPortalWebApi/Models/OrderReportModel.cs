using System;

namespace CustomerPortalWebApi.Models
{
    public class OrderReportModel
    {
        public string RegionCodevtxt { get; set; }
        public string RegionDescriptionvtxt { get; set; }
        public string BranchCodevtxt { get; set; }
        public string BranchNamevtxt { get; set; }
        public string SalesOfficeCodevtxt { get; set; }
        public string SalesOfficeNamevtxt { get; set; }
        public string CustCodevtxt { get; set; }
        public string CustNamevtxt { get; set; }
        public string OrderNovtxt { get; set; }
        public decimal? TotalOrderQuantityint { get; set; }
        public string Statusvtxt { get; set; }
        public string SAPOrderNovtxt { get; set; }
        public DateTime? SAPOrderDatedate { get; set; }
        public DateTime? OrderDatedate { get; set; }
        public string UserCodetxt { get; set; }
        public string UserNametxt { get; set; }
        public string UserTypetxt { get; set; }
    }
}