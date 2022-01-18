using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerPortalWebApi.Models
{
    public class KAMPriceApprovalSAPRequestModel
    {
        public string APPNO { get; set; }

        public string EDAT { get; set; }

        public string VFROM { get; set; }
        public string VTO { get; set; }
        public string KUNAG { get; set; }
        public string KUNNR { get; set; }
        public string MATNR { get; set; }
        public string TRAGR { get; set; }
        public string SFROM { get; set; }

        public string WERKS { get; set; }
        public string DLCON { get; set; }
        public string PLTYP { get; set; }

        public string KWMENG { get; set; }

        public string NPRICE { get; set; }

        public string RNPRICE { get; set; }

        public string NUNLOAD { get; set; }

        public string COMP_NM { get; set; }

        public string COMP_PR { get; set; }

        public string TTE { get; set; }

    }
}
