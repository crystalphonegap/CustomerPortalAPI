using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerPortalWebApi.Models
{
    public class OrderHeaderModel
    {
        public long IDbint { get; set; }
        public string OrderNovtxt { get; set; }
        public DateTime? OrderDatedate { get; set; }
        public string RefNovtxt { get; set; }
        public string RefDatedate { get; set; }
        public string SAPOrderNovtxt { get; set; }
        public DateTime? SAPOrderDatedate { get; set; }
        public string CustomerCodevtxt { get; set; }
        public string CustomerNamevtxt { get; set; }
        public string Divisionvtxt { get; set; }
        public string ShipToCodevtxt { get; set; }
        public string ShipToNamevtxt { get; set; }
        public string ShipToAddressvtxt { get; set; }

        [RegularExpression(@"^\d+\.\d{0,2}$")]
        public decimal? TotalOrderQuantityint { get; set; }

        [RegularExpression(@"^\d+\.\d{0,2}$")]
        public decimal? TotalNetValuedcl { get; set; }

        public string SAPStatusvtxt { get; set; }

        [RegularExpression(@"^\d+\.\d{0,2}$")]
        public decimal? OtherCharges1dcl { get; set; }

        [RegularExpression(@"^\d+\.\d{0,2}$")]
        public decimal? OtherCharges2dcl { get; set; }

        [RegularExpression(@"^\d+\.\d{0,2}$")]
        public decimal? OtherCharges3dcl { get; set; }

        [RegularExpression(@"^\d+\.\d{0,2}$")]
        public decimal? OtherCharges4dcl { get; set; }

        public string Statusvtxt { get; set; }
        public string CreatedByvtxt { get; set; }
        public DateTime? CreatedDatedatetime { get; set; }

        public string ModifyByvtxt { get; set; }

        public DateTime? ModifyDatedatetime { get; set; }

        public string ReqOrderNo { get; set; }

        public string CompanyNamevtxt { get; set; }

        public string SalesOrgNamevtxt { get; set; }
        public string Address { get; set; }

        public string DeliveryAddressvtxt { get; set; }

        public string PaymentTermsvtxt { get; set; }
        public string SPCodevtxt { get; set; }
        public string SPNamevtxt { get; set; }

        public string DeliveryTermsvtxt { get; set; }
        public decimal? QtyKg { get; set; }
        public decimal? QtyMt { get; set; }
        public decimal? TotalOrderQuantityKgsint { get; set; }
        public decimal? TotalOrderQuantityMTint { get; set; }

        public string SalesOrgNovtxt { get; set; }

        public string DivisionCdvtxt { get; set; }

        public string DistributionChannelCodevtxt { get; set; }
        public string DeliveryTermsCodevtxt { get; set; }

        public string PriceListvtxt { get; set; }

        public string DeliveryTerms1vtxt { get; set; }
    }
}