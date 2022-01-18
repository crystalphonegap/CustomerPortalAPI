using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerPortalWebApi.Models
{
    public class ItemMasterModel
    {
        public int Idint { get; set; }
        public string ItemCodevtxt { get; set; }
        public string ItemDescvtxt { get; set; }
        public string Uomnvtxt { get; set; }
        public string HSNCodevtxt { get; set; }

        [RegularExpression(@"^\d+\.\d{0,2}$")]
        public decimal? Ratedcl { get; set; }

        public bool Statusbit { get; set; }
        public DateTime? SystemDateTime { get; set; }
    }
}