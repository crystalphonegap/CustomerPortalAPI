using System;

namespace CustomerPortalWebApi.Entities
{
    public class SalesOrder
    {
        public long Idbint { get; set; }
        public string CompanyCodetxt { get; set; }
        public string SoldToPartyCodevtxt { get; set; }
        public string SoldTopPartyNamevtxt { get; set; }
        public string ShipToPartyCodevtxt { get; set; }
        public string ShipToPartyNamevtxt { get; set; }
        public string WebOrderNo { get; set; }
        public string Addressvtxt { get; set; }
        public string Cityvtxt { get; set; }
        public int BalanceQuantity { get; set; }
        public int TotalDeliveryQuantity { get; set; }
        public int TotalOrderValue { get; set; }
        public int TotalOrderQuantity { get; set; }
        public string Ponumbervtxt { get; set; }
        public string SalesOrderNumbervtxt { get; set; }
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

        public string ItemStatusvtxt { get; set; }
        public string MaterialCodevtxt { get; set; }
        public string MateriaDescriptionvtxt { get; set; }
        public int? OrderQuantityint { get; set; }
        public string UnitOfMeasurevtxt { get; set; }
        public decimal? NetValuedcl { get; set; }
        public string SalesOrderStatusvtxt { get; set; }
        public DateTime? DeliveryDatedate { get; set; }
        public TimeSpan? CreationTimetime { get; set; }
        public int? DeliveryQuantityint { get; set; }
        public DateTime? GoodsIssueDatedate { get; set; }
        public string Statusvtxt { get; set; }
        public string Plantvtxt { get; set; }
        public DateTime? SystemDateTimedatetime { get; set; }

        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string KeyWord { get; set; }

        public string DDeliveryOrderNovtxt { get; set; }
        public DateTime? DDeliveryDatedate { get; set; }
        public decimal? DQtyint { get; set; }

        public string IInvoiceDocumentNovtxt { get; set; }
        public DateTime? IBillingDatedate { get; set; }
        public decimal? IInvoiceQuantityint { get; set; }
        public decimal? TotalDeliveryQuantityMt { get; set; }
        public decimal? OrderQuantityintMt { get; set; }
        public decimal? BalanceQuantityMt { get; set; }

        public string OrderBlockListCodevtxt { get; set; }

        public string OrderBlockListDescvtxt { get; set; }
    }
}