using System;

namespace CustomerPortalWebApi.Entities
{
    public class RetailOrder
    {
        public long IDbint { get; set; }
        public long UoMint { get; set; }
        public string OrderNovtxt { get; set; }
        public DateTime? OrderDatedate { get; set; }
        public string DealerCodevtxt { get; set; }
        public string DealerNamevtxt { get; set; }
        public string MaterialCodevtxt { get; set; }
        public string MaterialDescvtxt { get; set; }
        public string DeliveryAddressvtxt { get; set; }
        public decimal? Quantitydcl { get; set; }
        public decimal? TotalOrderQuantityKgsint { get; set; }
        public decimal? TotalOrderQuantityMTint { get; set; }
        public string UOMvtxt { get; set; }
        public string RetailerCodevtxt { get; set; }

        public string ShipToAddressvtxt { get; set; }
        public string RetailerNamevtxt { get; set; }
        public string Statusvtxt { get; set; }
        public string CreatedBytxt { get; set; }
        public DateTime? CreatedDatetxt { get; set; }
    }
}