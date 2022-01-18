using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerPortalWebApi.Models
{
    public class InvoiceMasterModel
    {
        public long Idbint { get; set; }
        public string CompanyCodevtxt { get; set; }
        public string InvoiceDocumentNovtxt { get; set; }

        [RegularExpression(@"^\d+\.\d{0,2}$")]
        public decimal? InvoiceQuantityint { get; set; }

        public string InvoiceUoMvtxt { get; set; }
        public DateTime? BillingDatedate { get; set; }
        public string BillingTypevtxt { get; set; }
        public string BillingCategoryvtxt { get; set; }
        public string Plantvtxt { get; set; }
        public string SalesOrgvtxt { get; set; }
        public string DistributionChannelCodevtxt { get; set; }
        public string DistributionChannelvtxt { get; set; }
        public string SalesOfficeCodevtxt { get; set; }
        public string SalesOfficevtxt { get; set; }
        public string SoldToPartyCodevtxt { get; set; }
        public string SoldTopPartyNamevtxt { get; set; }
        public string ShipToPartyCodevtxt { get; set; }
        public string ShipToPartyNamevtxt { get; set; }
        public string SalesOrderNumbervtxt { get; set; }

        [RegularExpression(@"^\d+\.\d{0,2}$")]
        public decimal? BillingValuedcl { get; set; }

        [RegularExpression(@"^\d+\.\d{0,2}$")]
        public decimal? TaxAmountdcl { get; set; }

        public string IncoTermsvtxt { get; set; }
        public string Descriptionvtxt { get; set; }
        public string MaterialCodevtxt { get; set; }
        public string MaterialDescriptionvtxt { get; set; }
        public string Statusvtxt { get; set; }
        public DateTime? SystemDateTimevtxt { get; set; }
        public decimal? QtyKg { get; set; }
        public decimal? QtyMt { get; set; }
    }
}