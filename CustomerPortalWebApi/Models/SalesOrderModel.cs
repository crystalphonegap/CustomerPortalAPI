using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerPortalWebApi.Models
{
    public class SalesOrderModel
    {
        public long Idbint { get; set; }
        public string CompanyCodetxt { get; set; }
        public string SoldToPartyCodevtxt { get; set; }
        public string SoldTopPartyNamevtxt { get; set; }
        public string ShipToPartyCodevtxt { get; set; }
        public string ShipToPartyNamevtxt { get; set; }
        public string Addressvtxt { get; set; }
        public string Cityvtxt { get; set; }
        public string Ponumbervtxt { get; set; }
        public string SalesOrderNumbervtxt { get; set; }
        public string ItemStatusvtxt { get; set; }
        public string SalesOrderTypevtxt { get; set; }
        public DateTime? CreatedOndatetime { get; set; }
        public string SalesOrgCodevtxt { get; set; }
        public string SalesOrgDescriptionvtxt { get; set; }
        public string SalesOfficeCodevtxt { get; set; }
        public string SalesOfficeDescriptionvtxt { get; set; }
        public string DistributionChannelCodevtxt { get; set; }
        public string DistributionChannelvtxt { get; set; }
        public string DivisionCodevtxt { get; set; }
        public string Divisionvtxt { get; set; }
        public string ItemNumbervtxt { get; set; }
        public string MaterialCodevtxt { get; set; }
        public string MateriaDescriptionvtxt { get; set; }

        [RegularExpression(@"^\d+\.\d{0,2}$")]
        public decimal? OrderQuantityint { get; set; }

        public decimal? TotalDeliveryQuantityMt { get; set; }
        public decimal? OrderQuantityintMt { get; set; }
        public decimal? BalanceQuantityMt { get; set; }
        public string UnitOfMeasurevtxt { get; set; }

        [RegularExpression(@"^\d+\.\d{0,2}$")]
        public decimal? NetValuedcl { get; set; }

        public string SalesOrderStatusvtxt { get; set; }
        public DateTime? DeliveryDatedate { get; set; }
        public TimeSpan? CreationTimetime { get; set; }

        [RegularExpression(@"^\d+\.\d{0,2}$")]
        public decimal? DeliveryQuantityint { get; set; }

        public DateTime? GoodsIssueDatedate { get; set; }
        public string Statusvtxt { get; set; }
        public string Plantvtxt { get; set; }
        public DateTime? SystemDateTimedatetime { get; set; }

        public long? Completelyprocessed { get; set; }
        public long? Partiallyprocessed { get; set; }
        public long? Notyetprocessed { get; set; }
        public decimal? QtyKg { get; set; }
        public decimal? QtyMt { get; set; }
    }
}