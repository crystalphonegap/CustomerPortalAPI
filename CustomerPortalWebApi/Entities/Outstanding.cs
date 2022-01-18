using System.ComponentModel.DataAnnotations;

namespace CustomerPortalWebApi.Entities
{
    public class Outstanding
    {
        [RegularExpression(@"^\d+\.\d{0,2}$")]
        public decimal? NetOutstandingAmtdcl { get; set; }
    }
}