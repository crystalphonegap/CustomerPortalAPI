using System.ComponentModel.DataAnnotations;

namespace CustomerPortalWebApi.Entities
{
    public class Creditlimit
    {
        [RegularExpression(@"^\d+\.\d{0,2}$")]
        public decimal? AvailableCreditLimitdcl { get; set; }
    }
}