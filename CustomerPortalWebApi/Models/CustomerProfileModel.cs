
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerPortalWebApi.Models
{
    public class CustomerProfileModel
    {
        public string CustomerCodevtxt	{ get;set;}
        public decimal SecurityAmountdcl { get;set;}
        public string SupplySourcevtxt  { get; set; }
        public string Typevtxt  { get; set; }
        public string CustomerTypevtxt { get; set; }
        public string AssociatedWithPrism   { get; set; }
        public int NoOfMasonsAssociatedint  { get; set; }
        public decimal WareHouseSqmt  { get; set; }
        public string Staffvtxt { get; set; }
        public string Potentialvtxt { get; set; }
        public  string OtherBusinessvtxt  { get; set; }
        public decimal MasonMeetConductedint    { get; set; }

        public string Remarks { get; set; }
        public string IsActivebit { get; set; }
        public string CreatedByvtxt { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public string ModifyByvtxt { get; set; }

        public DateTime ModifyDatetime { get; set; }

        public string CustCodevtxt { get; set; }
        public string CustNamevtxt { get; set; }
        public string Address1vtxt { get; set; }
        public string Address2vtxt { get; set; }
        public string CityCdvtxt { get; set; }
        public string StateCdvtxt { get; set; }
        public string StateNamevtxt { get; set; }
        public string ContactpersonMobilevtxt { get; set; }
        public string TerritoryCodevtxt { get; set; }
        public string TerritoryNamevtxt { get; set; }
        public string SalesOfficevtxt { get; set; }
        public string Emailvtxt { get; set; }
        public string SalesOfficeDescvtxt { get; set; }
        public string Contactpersonvtxt { get; set; }
        public string PaymentTerms1vtxt { get; set; }
        public string PaymentTerms2vtxt { get; set; }

        public long BalancePoints { get; set; }
        public long EarnPoints { get; set; }
        public decimal? AvailableCreditLimitdcl { get; set; }

        public int AssociatedRetailers { get; set; }

        public string UserCodetxt { get; set; }
        public string TSE { get; set; }

        public string TTEName { get; set; }

        public string AttachmentBytesvtxt { get; set; }

        public int NoOfEffectiveRetailersCount { get; set; }

        public int NoOfNoEffectiveRetailersCount { get; set; }

        public string DealerEffectiveLastMonth { get; set; }

        public decimal? TotalActualSales { get; set; }
        public decimal CustSecurityAmount { get; set; }
        public string CustCategory { get; set; }
    }
}
