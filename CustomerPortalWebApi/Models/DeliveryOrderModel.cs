using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerPortalWebApi.Models
{
    public class DeliveryOrderModel
    {
        public long Idbint { get; set; }
        public string CompanyCodevtxt { get; set; }
        public string SoldToPartyCodevtxt { get; set; }
        public string SoldTopPartyNamevtxt { get; set; }
        public string ShipToPartyCodevtxt { get; set; }
        public string ShipToPartyNamevtxt { get; set; }
        public string SalesOrderNovtxt { get; set; }
        public string DeliveryOrderNovtxt { get; set; }
        public DateTime? DeliveryDatedate { get; set; }
        public string InvoiceNumbervtxt { get; set; }
        public DateTime? Pgidatedate { get; set; }

        [RegularExpression(@"^\d+\.\d{0,2}$")]
        public decimal? Qtyint { get; set; }

        public string Transportervtxt { get; set; }
        public string TruckNovtxt { get; set; }
        public string Drivervtxt { get; set; }
        public string DriverMobileNovtxt { get; set; }
        public string FreightTypevtxt { get; set; }
        public string Cityvtxt { get; set; }
        public string Lrnumbervtxt { get; set; }
        public DateTime? Lrdatedate { get; set; }
        public string MaterialCodevtxt { get; set; }
        public string MaterialDescriptionvtxt { get; set; }
        public string DeliveryStatusvtxt { get; set; }
        public string UOMvtxt { get; set; }
        public decimal? QtyKg { get; set; }
        public decimal? QtyMt { get; set; }
        public DateTime? SystemDateTimedatetime { get; set; }

        public long Completelyprocessed { get; set; }
        public long Partiallyprocessed { get; set; }
        public long Notyetprocessed { get; set; }
    }
}