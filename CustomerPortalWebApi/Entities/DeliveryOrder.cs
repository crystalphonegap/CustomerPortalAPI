using System;

namespace CustomerPortalWebApi.Entities
{
    public class DeliveryOrder
    {
        public long Idbint { get; set; }
        public string CompanyCodevtxt { get; set; }
        public string SoldToPartyCodevtxt { get; set; }
        public string SoldTopPartyNamevtxt { get; set; }
        public string ShipToPartyCodevtxt { get; set; }
        public string ShipToPartyNamevtxt { get; set; }
        public string SalesOrderNovtxt { get; set; }
        public string UOMvtxt { get; set; }
        public decimal? QtyMt { get; set; }
        public string DeliveryOrderNovtxt { get; set; }
        public DateTime? DeliveryDatedate { get; set; }
        public DateTime? OrderRecivedDate { get; set; }
        public DateTime? BillingDatedate { get; set; }
        public string InvoiceNumbervtxt { get; set; }
        public DateTime? Pgidatedate { get; set; }
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
        public DateTime? SystemDateTimedatetime { get; set; }

        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string KeyWord { get; set; }
        public Boolean Status { get; set; }
        public string Remark { get; set; }
        public string dest_nm { get; set; }
    }
}