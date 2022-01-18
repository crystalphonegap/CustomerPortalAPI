namespace CustomerPortalWebApi.Models
{
    public class UOM
    {
        public int IDint { get; set; }
        public string BaseUnit { get; set; }

        public string AlternativeUnit { get; set; }
        public decimal? RateOfConversion { get; set; }

        public decimal? Weight { get; set; }
    }
}