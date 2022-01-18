using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerPortalWebApi.Models
{
    public class PriceApprovalModel
    {
        public long IDBint { get; set; }
        public string ApprovalNovtxt { get; set; }
        public string CustomerCodevtxt { get; set; }
        public string CustomerNamevtxt { get; set; }
        public string ConsigneeCodevtxt { get; set; }
        public string ConsigneeNamevtxt { get; set; }

        public string DepotCodevtxt { get; set; }

        public string DepotNamevtxt { get; set; }
        public string Gradevtxt { get; set; }
        public string TransportModevtxt { get; set; }
        public string SupplyFromvtxt { get; set; }
        public string DeliveryTermsvtxt { get; set; }
        public string PaymentTermsvtxt { get; set; }
        public int Quantitydcl { get; set; }
        public decimal NonTradedcl { get; set; }
        public decimal Tradedcl { get; set; }
        public decimal PriceDiffdcl { get; set; }
        public string Unloading { get; set; }
        public decimal NDiscountdcl { get; set; }
        public decimal Discountdcl { get; set; }
        public decimal SPCommTrade { get; set; }
        public decimal SPCommNonTrade { get; set; }
        public string CompetitionNamevtxt { get; set; }
        public decimal CompetitionPricedcl { get; set; }
        public string TTENamevtxt { get; set; }
        public decimal NCRTradedcl { get; set; }
        public decimal NCRNonTradedcl { get; set; }
        public decimal NCRDiffdcl { get; set; }
        public decimal PackingTradedcl { get; set; }
        public decimal PackingNonTradedcl { get; set; }
        public decimal HandlingTradedcl { get; set; }
        public decimal HandlingNonTradedcl { get; set; }
        public decimal PricePerMTTradedcl { get; set; }
        public decimal PricePerMTNonTradedcl { get; set; }
        public decimal PrimaryFrtTradedcl { get; set; }
        public decimal PrimaryFrtNonTradedcl { get; set; }
        public decimal SecondaryFrtTradedcl { get; set; }
        public decimal SecondaryFrtNonTradedcl { get; set; }
        public decimal GSTTradedcl { get; set; }
        public decimal GSTNonTradedcl { get; set; }
        public string RegionCodevtxt { get; set; }
        public string RegionDescvtxt { get; set; }
        public string BranchCodevtxt { get; set; }
        public string BranchDescvtxt { get; set; }
        public string TerritoryCodevtxt { get; set; }
        public string TerritoryDescvtxt { get; set; }
        public string TPCAgentCodevtxt { get; set; }
        public string TPCAgentNamevtxt { get; set; }
        public string Destinationcodevtxt { get; set; }
        public string DestinationNamevtxt { get; set; }
        public string PriceZonevtxt { get; set; }
        public string PriceZonedescvtxt { get; set; }
        public string ValidityFromDate { get; set; }
        public string ValidityToDate { get; set; }
        public string Statusvtxt { get; set; }
        public string SAPStatusvtxt { get; set; }

        public string Remarksvtxt { get; set; }
        public string Typevtxt { get; set; }
        public string CreatedByvtxt { get; set; }
        public string CreatedDatetime { get; set; }
        public string Mode { get; set; }

    }
    }
