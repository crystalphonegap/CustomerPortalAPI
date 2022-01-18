using System;

namespace CustomerPortalWebApi.Models
{
    public class OrderAnalystMappingModel
    {
        public long IDbint { get; set; }

        public string UserCodevtxt { get; set; }
        public string SalesOfficeCodevtxt { get; set; }
        public string SalesOfficeNamevtxt { get; set; }
        public string CustomerTypevtxt { get; set; }
        public string Createdbyvtxt { get; set; }

        public DateTime? CreatedDatetimedatetime { get; set; }
    }
}