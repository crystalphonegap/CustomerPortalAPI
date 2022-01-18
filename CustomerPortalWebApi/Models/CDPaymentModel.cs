using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerPortalWebApi.Models
{
    public class CDPaymentModel
    {
        public DateTime Date { get; set; }
        public string CustomerCodevtxt { get; set; }
        public decimal Quantitydcl { get; set; }
        public decimal AdvanceCDdcl { get; set; }
        public decimal DebitNotedcl { get; set; }

    }
}
