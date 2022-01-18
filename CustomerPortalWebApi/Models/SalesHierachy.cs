using System;

namespace CustomerPortalWebApi.Models
{
    public class SalesHierachy
    {
        public long IDBint { get; set; }

        public string SalesOfficeCodevtxt { get; set; }
        public string SalesOfficeNamevtxt { get; set; }
        public string BranchCodevtxt { get; set; }
        public string BranchNamevtxt { get; set; }
        public string RegionCodevtxt { get; set; }
        public string RegionDescriptionvtxt { get; set; }
        public string ZoneCodevtxt { get; set; }
        public string ZoneDescriptionvtxt { get; set; }
        public string HODCodevtxt { get; set; }
        public string HODNamevtxt { get; set; }
        public string CompanyCodevtxt { get; set; }
        public string CompanyNamevtxt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        public string Remarks { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}