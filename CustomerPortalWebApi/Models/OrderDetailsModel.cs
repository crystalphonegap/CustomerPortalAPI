namespace CustomerPortalWebApi.Models
{
    public class OrderDetailsModel
    {
        public long IDbint { get; set; }
        public long OrderID { get; set; }
        public string MaterialCodevtxt { get; set; }
        public string MaterialDescriptionvtxt { get; set; }

        public string HSNCodevtxt { get; set; }
        public string UoMvtxt { get; set; }

        //[RegularExpression(@"^\d+\.\d{0,2}$")]
        public decimal? Quantityint { get; set; }

        //[RegularExpression(@"^\d+\.\d{0,2}$")]
        public decimal? Ratedcl { get; set; }

        //[RegularExpression(@"^\d+\.\d{0,2}$")]
        public decimal? Amountdcl { get; set; }

        public string UoMint { get; set; }
        public decimal? QtyKg { get; set; }
        public decimal? QtyMt { get; set; }
    }
}