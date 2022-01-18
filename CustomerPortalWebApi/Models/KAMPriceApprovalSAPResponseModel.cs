using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerPortalWebApi.Models
{
    public class KAMPriceApprovalSAPResponseModel
    {
        public string CUSNAME { get; set; }

        public string CONNAME { get; set; }

        public string VFROM { get; set; }
        public string VTO { get; set; }
        public string KUNAG { get; set; }
        public string KUNNR { get; set; }
        public string REGION { get; set; }
        public string BRANCH { get; set; }

        public string TERRITORY { get; set; }
        public string RNAME { get; set; }
        public string BNAME { get; set; }

        public string TNAME { get; set; }

        public string PRNAME { get; set; }

        public string LZONE { get; set; }

        public string MATNR { get; set; }

        public decimal KWMENG { get; set; }

        public string ZTERM { get; set; }
        public string ZTERM_NAME { get; set; }

        public string PYNAME { get; set; }

        public string TRAGR { get; set; }

        public string SFROM { get; set; }

        public string WERKS { get; set; }

        public string DPNM { get; set; }

        public string LIFNR { get; set; }

        public string PLTYP { get; set; }
        public string DNAME { get; set; }

        public decimal TPRICE { get; set; }
        public decimal NPRICE { get; set; }

        public decimal DPRICE { get; set; }

        public decimal RTPRICE { get; set; }

        public decimal RNPRICE { get; set; }

        public decimal PTFRT { get; set; }

        public decimal PNFRT { get; set; }

        public decimal STFRT { get; set; }

        public decimal SNFRT { get; set; }

        public decimal TGST { get; set; }


        public decimal NGST { get; set; }

        public decimal TPACK { get; set; }

        public decimal NPACK { get; set; }

        public decimal THAND { get; set; }

        public decimal NHAND { get; set; }

        public decimal TSP { get; set; }

        public decimal NSP { get; set; }

        public decimal TDISCOUNT { get; set; }

        public decimal NDISCOUNT { get; set; }

        public decimal TUNLOAD { get; set; }

        public decimal NUNLOAD { get; set; }

        public decimal TNCR { get; set; }
        public decimal NNCR { get; set; }

        public decimal DNCR { get; set; }

        public string TPC { get; set; }

        public string COMP_NM { get; set; }

        public decimal COMP_PR { get; set; }

        public string REMARK { get; set; }

        public string TTE { get; set; }

        public string STATUS { get; set; }
    }
}
