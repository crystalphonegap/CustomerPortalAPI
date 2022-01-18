using CustomerPortalWebApi.Interface;
using System;
using System.Collections.Generic;
using CustomerPortalWebApi.Models;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Data;

namespace CustomerPortalWebApi.Services
{
    public class PriceApprovalService : IPriceApproval
    {
        private readonly ICustomerPortalHelper _customerPortalHelper;

        

        public PriceApprovalService(ICustomerPortalHelper customerPortalHelper)
        {
            _customerPortalHelper = customerPortalHelper;
        }

        public long InsertPriceApproval(PriceApprovalModel model )
        {
            var dbPara = new DynamicParameters();
             
            if(!string.IsNullOrEmpty(model.Mode))
            {
                model.Mode = "R";
            }
            else
            {
                 if (model.IDBint > 0)
                {
                    model.Mode = "U";
                }
                else
                {

                    model.Mode = "A";
                } 
            }
               
            dbPara.Add("Mode", model.Mode, DbType.String);
            dbPara.Add("IDBint", model.IDBint, DbType.Int32);
            dbPara.Add("CustomerCodevtxt", model.CustomerCodevtxt, DbType.String);
            dbPara.Add("CustomerNamevtxt", model.CustomerNamevtxt, DbType.String);
            dbPara.Add("ConsigneeCodevtxt", model.ConsigneeCodevtxt, DbType.String);
            dbPara.Add("ConsigneeNamevtxt", model.ConsigneeNamevtxt, DbType.String);
            dbPara.Add("Gradevtxt", model.Gradevtxt, DbType.String);
            dbPara.Add("DepotCodevtxt", model.DepotCodevtxt, DbType.String);
            dbPara.Add("DepotNamevtxt", model.DepotNamevtxt, DbType.String);
            dbPara.Add("TransportModevtxt", model.TransportModevtxt, DbType.String);
            dbPara.Add("SupplyFromvtxt", model.SupplyFromvtxt, DbType.String);
            dbPara.Add("DeliveryTermsvtxt", model.DeliveryTermsvtxt, DbType.String);
            dbPara.Add("PaymentTermsvtxt", model.PaymentTermsvtxt, DbType.String);
            dbPara.Add("Quantitydcl", model.Quantitydcl, DbType.Decimal);
            dbPara.Add("NonTradedcl", model.NonTradedcl, DbType.Decimal);
            dbPara.Add("Tradedcl", model.Tradedcl, DbType.Decimal);
            dbPara.Add("PriceDiffdcl", model.PriceDiffdcl, DbType.Decimal);
            dbPara.Add("Unloading", model.Unloading, DbType.String);
            dbPara.Add("Discountdcl", model.Discountdcl, DbType.Decimal);
            dbPara.Add("SPCommTrade", model.SPCommTrade, DbType.Decimal);
            dbPara.Add("SPCommNonTrade", model.SPCommNonTrade, DbType.Decimal);
            dbPara.Add("CompetitionNamevtxt", model.CompetitionNamevtxt, DbType.String);
            dbPara.Add("CompetitionPricedcl", model.CompetitionPricedcl, DbType.Decimal);
            dbPara.Add("TTENamevtxt", model.TTENamevtxt, DbType.String);
            //dbPara.Add("NCRTradedcl", model.NCRTradedcl, DbType.Decimal);
            //dbPara.Add("NCRNonTradedcl", model.NCRNonTradedcl, DbType.Decimal);
            //dbPara.Add("NCRDiffdcl", model.NCRDiffdcl, DbType.Decimal);
            dbPara.Add("Statusvtxt", model.Statusvtxt, DbType.String);
            dbPara.Add("Remarksvtxt", model.Remarksvtxt, DbType.String);
            dbPara.Add("Typevtxt", model.Typevtxt, DbType.String);
            dbPara.Add("CreatedByvtxt", model.CreatedByvtxt, DbType.String);
            #region using dapper

            var data = _customerPortalHelper.Insert<long>("[dbo].[USP_InsertPriceApproval]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;

            #endregion using dapper
        }

        public long InsertFinalPriceApproval(PriceApprovalModel model)
        {
            model.IDBint= InsertPriceApproval(model);
            var dbPara = new DynamicParameters();
            dbPara.Add("IDBint", model.IDBint, DbType.Int32);
            dbPara.Add("CustomerCodevtxt", model.CustomerCodevtxt, DbType.String);
            dbPara.Add("CustomerNamevtxt", model.CustomerNamevtxt, DbType.String);
            dbPara.Add("ConsigneeCodevtxt", model.ConsigneeCodevtxt, DbType.String);
            dbPara.Add("ConsigneeNamevtxt", model.ConsigneeNamevtxt, DbType.String);
            dbPara.Add("DepotCodevtxt", model.DepotCodevtxt, DbType.String);
            dbPara.Add("DepotNamevtxt", model.DepotNamevtxt, DbType.String);
            dbPara.Add("Gradevtxt", model.Gradevtxt, DbType.String);
            dbPara.Add("TransportModevtxt", model.TransportModevtxt, DbType.String);
            dbPara.Add("SupplyFromvtxt", model.SupplyFromvtxt, DbType.String);
            dbPara.Add("DeliveryTermsvtxt", model.DeliveryTermsvtxt, DbType.String);
            dbPara.Add("PaymentTermsvtxt", model.PaymentTermsvtxt, DbType.String);
            dbPara.Add("Quantitydcl", model.Quantitydcl, DbType.Decimal);
            dbPara.Add("NonTradedcl", model.NonTradedcl, DbType.Decimal);
            dbPara.Add("Tradedcl", model.Tradedcl, DbType.Decimal);
            dbPara.Add("PriceDiffdcl", model.PriceDiffdcl, DbType.Decimal);
            dbPara.Add("Unloading", model.Unloading, DbType.String);
            dbPara.Add("NDiscountdcl", model.NDiscountdcl, DbType.String);
            dbPara.Add("Discountdcl", model.Discountdcl, DbType.Decimal);
            dbPara.Add("SPCommTrade", model.SPCommTrade, DbType.Decimal);
            dbPara.Add("SPCommNonTrade", model.SPCommNonTrade, DbType.Decimal);
            dbPara.Add("CompetitionNamevtxt", model.CompetitionNamevtxt, DbType.String);
            dbPara.Add("CompetitionPricedcl", model.CompetitionPricedcl, DbType.Decimal);
            dbPara.Add("TTENamevtxt", model.TTENamevtxt, DbType.String);
            dbPara.Add("NCRTradedcl", model.NCRTradedcl, DbType.Decimal);
            dbPara.Add("NCRNonTradedcl", model.NCRNonTradedcl, DbType.Decimal);
            dbPara.Add("NCRDiffdcl", model.NCRDiffdcl, DbType.Decimal);
            dbPara.Add("PackingTradedcl", model.PackingTradedcl, DbType.Decimal);
            dbPara.Add("PackingNonTradedcl", model.PackingNonTradedcl, DbType.Decimal);
            dbPara.Add("HandlingTradedcl", model.HandlingTradedcl, DbType.Decimal);
            dbPara.Add("HandlingNonTradedcl", model.HandlingNonTradedcl, DbType.Decimal);
            dbPara.Add("PricePerMTTradedcl", model.PricePerMTTradedcl, DbType.Decimal);
            dbPara.Add("PricePerMTNonTradedcl", model.PricePerMTNonTradedcl, DbType.Decimal);
            dbPara.Add("PrimaryFrtTradedcl", model.PrimaryFrtTradedcl, DbType.Decimal);
            dbPara.Add("PrimaryFrtNonTradedcl", model.PrimaryFrtNonTradedcl, DbType.Decimal);
            dbPara.Add("SecondaryFrtTradedcl", model.SecondaryFrtTradedcl, DbType.Decimal);
            dbPara.Add("SecondaryFrtNonTradedcl", model.SecondaryFrtNonTradedcl, DbType.Decimal);
            dbPara.Add("GSTTradedcl", model.GSTTradedcl, DbType.Decimal);
            dbPara.Add("GSTNonTradedcl", model.GSTNonTradedcl, DbType.Decimal);
            dbPara.Add("RegionCodevtxt", model.RegionCodevtxt, DbType.String);
            dbPara.Add("RegionDescvtxt", model.RegionDescvtxt, DbType.String);
            dbPara.Add("BranchCodevtxt", model.BranchCodevtxt, DbType.String);
            dbPara.Add("BranchDescvtxt", model.BranchDescvtxt, DbType.String);
            dbPara.Add("TerritoryCodevtxt", model.TerritoryCodevtxt, DbType.String);
            dbPara.Add("TerritoryDescvtxt", model.TerritoryDescvtxt, DbType.String);
            dbPara.Add("TPCAgentCodevtxt", model.TPCAgentCodevtxt, DbType.String);
            dbPara.Add("TPCAgentNamevtxt", model.TPCAgentNamevtxt, DbType.String);
            dbPara.Add("Destinationcodevtxt", model.Destinationcodevtxt, DbType.String);
            dbPara.Add("DestinationNamevtxt", model.DestinationNamevtxt, DbType.String);
            dbPara.Add("PriceZonevtxt", model.PriceZonevtxt, DbType.String);
            dbPara.Add("PriceZonedescvtxt", model.PriceZonedescvtxt, DbType.String);
            dbPara.Add("ValidityFromDate", model.ValidityFromDate, DbType.String);
            dbPara.Add("ValidityToDate", model.ValidityToDate, DbType.String);
            dbPara.Add("SAPStatusvtxt", model.SAPStatusvtxt, DbType.String);
            dbPara.Add("Remarksvtxt", model.Remarksvtxt, DbType.String);
            dbPara.Add("Statusvtxt", model.Statusvtxt, DbType.String);
            dbPara.Add("Typevtxt", model.Typevtxt, DbType.String);
            #region using dapper

            var data = _customerPortalHelper.Insert<long>("[dbo].[USP_InserFinaltPriceApproval]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;

            #endregion using dapper
        }

        public List<PriceApprovalModel> GetPriceApprovalData(string Statusvtxt,string Createdby, int PageNo, int PageSize, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("Statusvtxt", Statusvtxt, DbType.String);
            dbPara.Add("CreatedBy", Createdby, DbType.String);
            dbPara.Add("PageNo", PageNo, DbType.Int32);
            dbPara.Add("PageSize", PageSize, DbType.Int32);
            dbPara.Add("ID", 0, DbType.Int64);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<PriceApprovalModel>("[dbo].[uspviewGetPriceApprovalData]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public List<PriceApprovalModel> DownloadPriceApprovalData(string Statusvtxt, string Createdby,string KeyWord)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("Statusvtxt", Statusvtxt, DbType.String);
            dbPara.Add("CreatedBy", Createdby, DbType.String);
            dbPara.Add("PageNo", 0, DbType.Int32);
            dbPara.Add("PageSize", 0, DbType.Int32);
            dbPara.Add("ID", 0, DbType.Int64);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<PriceApprovalModel>("[dbo].[uspviewGetPriceApprovalData]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public long GetPriceApprovalDataCount(string Statusvtxt, string Createdby, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("Statusvtxt", Statusvtxt, DbType.String);
            dbPara.Add("CreatedBy", Createdby, DbType.String);
            dbPara.Add("PageNo", 0, DbType.Int32);
            dbPara.Add("PageSize", 0, DbType.Int32);
            dbPara.Add("ID", 0, DbType.Int64);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<PriceApprovalModel>("[dbo].[uspviewGetPriceApprovalData]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList().Count;
            }
            else
            {
                return 0;
            }
        }

        public PriceApprovalModel GetPriceApprovalDataById(long id)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("Statusvtxt", "", DbType.String);
            dbPara.Add("CreatedBy", "", DbType.String);
            dbPara.Add("PageNo", 0, DbType.Int32);
            dbPara.Add("PageSize", 0, DbType.Int32);
            dbPara.Add("ID", id, DbType.Int64);
            dbPara.Add("KeyWord", "", DbType.String);
            var data = _customerPortalHelper.GetAll<PriceApprovalModel>("[dbo].[uspviewGetPriceApprovalData]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data[0];
            }
            else
            {
                return null;
            }
        }
    }
}
