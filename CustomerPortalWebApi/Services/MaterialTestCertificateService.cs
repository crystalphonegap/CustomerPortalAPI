using CustomerPortalWebApi.Interface;
using CustomerPortalWebApi.Security;
using Microsoft.Extensions.Options;
using CustomerPortalWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Data;

namespace CustomerPortalWebApi.Services
{
    public class MaterialTestCertificateService : IMaterialTestCertificateService
    {
        private readonly MailSettings _mailSettings;
        private readonly ICustomerPortalHelper _customerPortalHelper;

        public MaterialTestCertificateService(IOptions<MailSettings> mailSettings, ICustomerPortalHelper customerPortalHelper)
        {
            _mailSettings = mailSettings.Value;
            _customerPortalHelper = customerPortalHelper;
        }

        public string GetDocNo()
        {
            var dbPara = new DynamicParameters();
            var data = _customerPortalHelper.GetAll<MaterialTestCertificateModel>("[dbo].[uspviewgetMaterialTestCertificateNo]", dbPara, commandType: CommandType.StoredProcedure);
            if (data != null)
            {
                return data[0].DocNovtxt.ToString();
            }
            else
            {
                return null;
            }
        }

        public List<MaterialTestCertificateModel> GetMaterialTestCertificates(MaterialTestCertificateModel model)
        {
            var dbPara = new DynamicParameters();
            if (model.mode == "List")
            {
                dbPara.Add("PageNo", model.PageNo, DbType.Int32);
                dbPara.Add("PageSize", model.PageSize, DbType.Int32);
                dbPara.Add("mode", model.mode, DbType.String);
                dbPara.Add("Id", 0, DbType.Int64);
                //dbPara.Add("DocNovtxt", model.DocNovtxt, DbType.String);
               
                if (model.DocDatedatetime != null)
                {
                    model.DocDatedatetime = model.DocDatedatetime.Replace("-", "/");
                    DateTime date = DateTime.ParseExact(model.DocDatedatetime, "dd/MM/yyyy", null);
                    dbPara.Add("DocDatedatetime", date, DbType.DateTime);
                }
                else
                {
                    dbPara.Add("DocDatedatetime", null, DbType.DateTime);
                }

                dbPara.Add("Yeartxt", model.Yeartxt, DbType.String);
                dbPara.Add("Gradetxt", model.Gradetxt, DbType.String);
                dbPara.Add("Daystxt", model.Daystxt, DbType.String);
                //dbPara.Add("Depotvtxt", model.Depotvtxt, DbType.String);
                dbPara.Add("Weektxt", model.Weektxt, DbType.String);
            }
            else if (model.mode == "Count")
            {
                dbPara.Add("PageNo", 0, DbType.Int32);
                dbPara.Add("PageSize", 0, DbType.Int32);
                dbPara.Add("mode", model.mode, DbType.String);
                dbPara.Add("Id", 0, DbType.Int64);
                //dbPara.Add("DocNovtxt", "", DbType.String);  
                if (model.DocDatedatetime != null)
                {
                    model.DocDatedatetime = model.DocDatedatetime.Replace("-", "/");
                    DateTime date = DateTime.ParseExact(model.DocDatedatetime, "dd/MM/yyyy", null);
                    dbPara.Add("DocDatedatetime", date, DbType.DateTime);
                }
                else
                {
                    dbPara.Add("DocDatedatetime", null, DbType.DateTime);
                }
                dbPara.Add("Yeartxt", model.Yeartxt, DbType.String);
                dbPara.Add("Gradetxt", model.Gradetxt, DbType.String);
                dbPara.Add("Daystxt", model.Daystxt, DbType.String);
                //dbPara.Add("Depotvtxt", model.Depotvtxt, DbType.String);
                dbPara.Add("Weektxt", model.Weektxt, DbType.String);
            }
            else if (model.mode == "Detail")
            {
                dbPara.Add("PageNo", 0, DbType.Int32);
                dbPara.Add("PageSize", 0, DbType.Int32);
                dbPara.Add("mode", model.mode, DbType.String);
                dbPara.Add("Id", model.IDbint, DbType.Int64);
                //dbPara.Add("DocNovtxt", "", DbType.String);
                dbPara.Add("DocDatedatetime", null, DbType.DateTime);
                dbPara.Add("Yeartxt", "", DbType.String);
                dbPara.Add("Gradetxt", "", DbType.String);
                dbPara.Add("Daystxt", "", DbType.String);
                //dbPara.Add("Depotvtxt", "", DbType.String);
                dbPara.Add("Weektxt", "", DbType.String);
            }
            else if (model.mode == "Image")
            {
                dbPara.Add("PageNo", 0, DbType.Int32);
                dbPara.Add("PageSize", 0, DbType.Int32);
                dbPara.Add("mode", model.mode, DbType.String);
                dbPara.Add("Id", model.IDbint, DbType.Int64);
                //dbPara.Add("DocNovtxt", "", DbType.String);
                dbPara.Add("DocDatedatetime", null, DbType.DateTime);
                dbPara.Add("Yeartxt", "", DbType.String);
                dbPara.Add("Gradetxt", "", DbType.String);
                dbPara.Add("Daystxt", "", DbType.String);
                //dbPara.Add("Depotvtxt", "", DbType.String);
                dbPara.Add("Weektxt", "", DbType.String);
            }
            var data = _customerPortalHelper.GetAll<MaterialTestCertificateModel>("[dbo].[uspGetMaterialTestCertificatesList]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public List<MaterialTestCertificateModel> GetReqOrderNo(string ReqOrderNo)
        {
            List<MaterialTestCertificateModel> lstOrd = new List<MaterialTestCertificateModel>();
            MaterialTestCertificateModel ord = new MaterialTestCertificateModel();
            ord.DocNovtxt = ReqOrderNo;
            lstOrd.Add(ord);

            if (lstOrd != null)
            {
                return lstOrd.ToList();
            }
            else
            {
                return null;
            }
        }
        //public long CreateMaterialCertificate(MaterialTestCertificateModel model)
        //{
        //    var dbPara = new DynamicParameters();
        //    dbPara.Add("DocNovtxt", model.DocNovtxt, DbType.String);
        //    dbPara.Add("CertificateNovtxt", model.CertificateNovtxt, DbType.String);
        //    dbPara.Add("DateOfIssue", model.DateOfIssue, DbType.DateTime);
        //    dbPara.Add("BatchNovtxt", model.BatchNovtxt, DbType.String);
        //    dbPara.Add("Depotvtxt", model.Depotvtxt, DbType.String);
        //    dbPara.Add("ValidTillDate", model.ValidTillDate, DbType.DateTime);
        //    dbPara.Add("AttachmentFileNamevtxt", model.AttachmentFileNamevtxt, DbType.String);
        //    dbPara.Add("AttachmentFilePathvtxt", model.AttachmentFilePathvtxt, DbType.String);
        //    dbPara.Add("AttachmentBytesvtxt", model.AttachmentBytesvtxt, DbType.String);
        //    #region using dapper

        //    var data = _customerPortalHelper.Insert<long>("[dbo].[uspInsertMaterialTestCertificate]",
        //                    dbPara,
        //                    commandType: CommandType.StoredProcedure);
        //    return data;

        //    #endregion using dapper
        //}

        public long InsertMaterialCertificate(MaterialTestCertificateModel model)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("DocNovtxt", model.DocNovtxt, DbType.String); 
            model.DocDatedatetime = model.DocDatedatetime.Replace("-", "/");
            DateTime date = DateTime.ParseExact(model.DocDatedatetime, "dd/MM/yyyy", null);
            dbPara.Add("DocDatedatetime", date, DbType.DateTime);
            dbPara.Add("Yeartxt", model.Yeartxt, DbType.String);
            dbPara.Add("Gradetxt", model.Gradetxt, DbType.String);  
            dbPara.Add("Daystxt", model.Daystxt, DbType.String);
            dbPara.Add("Depotvtxt", model.Depotvtxt, DbType.String);
            dbPara.Add("Weektxt", model.Weektxt, DbType.String);
            dbPara.Add("CreatedBytxt", model.CreatedBytxt, DbType.String);
            
            #region using dapper

            var data = _customerPortalHelper.Insert<long>("[dbo].[uspInsertMaterialTestCertificate]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;

            #endregion using dapper
        }


        public long InsertMaterialCertificateDetail(List< MaterialTestCertificateDetailModel> model)
        {
            long data=0;
            for (int count =0; count < model.Count; count++)
            {
                var dbPara = new DynamicParameters();
                dbPara.Add("HeaderIdint", model[0].HeaderIdint, DbType.String);
                dbPara.Add("AttachmentFileNamevtxt", model[count].AttachmentFileNamevtxt, DbType.String);
                dbPara.Add("AttachmentBytesvtxt", model[count].AttachmentBytesvtxt, DbType.String); 
                #region using dapper
                data = _customerPortalHelper.Insert<long>("[dbo].[uspInsertMaterialTestDetailCertificate]",
                                dbPara,
                                commandType: CommandType.StoredProcedure);
                #endregion using dapper
            }

            return data;
        }

        public int InsertDealerProfileDataIntoTemp(CustomerProfileModel model)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("CustomerCodevtxt", model.CustomerCodevtxt, DbType.String);
            dbPara.Add("SecurityAmountdcl", model.SecurityAmountdcl, DbType.Decimal);
            dbPara.Add("SupplySourcevtxt", model.SupplySourcevtxt, DbType.String);
            dbPara.Add("Typevtxt", model.Typevtxt, DbType.String);
            dbPara.Add("AssociatedWithPrism", model.AssociatedWithPrism, DbType.String);
            dbPara.Add("NoOfMasonsAssociatedint", model.NoOfMasonsAssociatedint, DbType.Int32);
            dbPara.Add("WareHouseSqmt", model.WareHouseSqmt, DbType.Int32);
            dbPara.Add("Staffvtxt", model.Staffvtxt, DbType.String);
            dbPara.Add("Potentialvtxt", model.Potentialvtxt, DbType.String);
            dbPara.Add("OtherBusinessvtxt", model.OtherBusinessvtxt, DbType.String);
            dbPara.Add("MasonMeetConductedint", model.MasonMeetConductedint, DbType.Int32);
            dbPara.Add("CreatedByvtxt", "SA001", DbType.String);

            #region using dapper

            var data = _customerPortalHelper.Insert<int>("[dbo].[uspInsertDealerProfileIntoTemp]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;

            #endregion using dapper
        }


        public long DeleteTempDealerProfileData()
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("mode", "Delete", DbType.String);
            var data = _customerPortalHelper.Execute("[dbo].[uspgetdeleteTempDealerProfileData]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public List<CustomerProfileModel> GetTempDealerProfileData()
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("mode", "Get", DbType.String);
            var data = _customerPortalHelper.GetAll<CustomerProfileModel>("[dbo].[uspgetdeleteTempDealerProfileData]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            List<CustomerProfileModel> users = data.ToList();
            return users;
        }

        public long InsertDealerProfileDataIntoMain()
        {
            var dbPara = new DynamicParameters();
            var data = _customerPortalHelper.Execute("[dbo].[uspInsertDealerProfileDataintoMain]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

       

        public List<CustomerProfileModel> GetDealerProfiledata(string Dealercode)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("DealerCode", Dealercode, DbType.String);
            var data = _customerPortalHelper.GetAll<CustomerProfileModel>("[dbo].[uspGetCustomerProfileData]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            List<CustomerProfileModel> users = data.ToList();
            return users;
        }

        public long UpdateUserProfileImage(CustomerProfileModel model)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("usercode", model.UserCodetxt, DbType.String);
            dbPara.Add("AttachmentFile", model.AttachmentBytesvtxt, DbType.String);
            var data = _customerPortalHelper.Get<int> ("[dbo].[USP_UpdateUserProfileImage]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public long GetCustomerProfileListCount(string KeyWord)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("Customercode", "", DbType.String);
            dbPara.Add("PageNo", 0, DbType.Int32);
            dbPara.Add("PageSize", 0, DbType.Int32);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            dbPara.Add("Mode", "Count", DbType.String);
            var data = _customerPortalHelper.GetAll<CustomerProfileModel>("[dbo].[uspviewDownloadCustomerProfileData]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            if (data != null)
            {
                return Convert.ToInt64(data.ToList().Count);
            }
            else
            {
                return 0;
            }
        }
          

        public List<CustomerProfileModel> GetCustomerProfileListDownload(string KeyWord)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("Customercode", "", DbType.String);
            dbPara.Add("PageNo", 0, DbType.Int32);
            dbPara.Add("PageSize", 0, DbType.Int32);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            dbPara.Add("Mode", "Download", DbType.String);
            var data = _customerPortalHelper.GetAll<CustomerProfileModel>("[dbo].[uspviewDownloadCustomerProfileData]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
                return data.ToList();
           
        }

        public List<CustomerProfileModel> GetCustomerProfileList(int PageNo, int PageSize, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("Customercode", "", DbType.String);
            dbPara.Add("PageNo", PageNo, DbType.Int32);
            dbPara.Add("PageSize", PageSize, DbType.Int32);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            dbPara.Add("Mode", "List", DbType.String);
            var data = _customerPortalHelper.GetAll<CustomerProfileModel>("[dbo].[uspviewDownloadCustomerProfileData]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data.ToList();
        }
        

        public string DeleteMaterialTestCertificate(int ID)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("ID", ID, DbType.Int32); 
            var data = _customerPortalHelper.Get<string>("[dbo].[DeleteMaterialCertificate]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;

        }
    }
}
