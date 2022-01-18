using CustomerPortalWebApi.Interface;
using CustomerPortalWebApi.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CustomerPortalWebApi.Services
{
    public class UploadEmployeeService : IUploadEmployeeService
    {
        private readonly ICustomerPortalHelper _customerPortalHelper;

        public UploadEmployeeService(ICustomerPortalHelper customerPortalHelper)
        {
            _customerPortalHelper = customerPortalHelper;
        }

        public long InsertSalesHierachyIntoTempTable(SalesHierachy saleshierachy)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("SalesOfficeCodevtxt", saleshierachy.SalesOfficeCodevtxt, DbType.String);
            dbPara.Add("SalesOfficeNamevtxt", saleshierachy.SalesOfficeNamevtxt, DbType.String);
            dbPara.Add("BranchCodevtxt", saleshierachy.BranchCodevtxt, DbType.String);
            dbPara.Add("BranchNamevtxt", saleshierachy.BranchNamevtxt, DbType.String);
            dbPara.Add("RegionCodevtxt", saleshierachy.RegionCodevtxt, DbType.String);
            dbPara.Add("RegionDescriptionvtxt", saleshierachy.RegionDescriptionvtxt, DbType.String);
            dbPara.Add("ZoneCodevtxt", saleshierachy.ZoneCodevtxt, DbType.String);
            dbPara.Add("ZoneDescriptionvtxt", saleshierachy.ZoneDescriptionvtxt, DbType.String);
            dbPara.Add("HODCodevtxt", saleshierachy.HODCodevtxt, DbType.String);
            dbPara.Add("HODNamevtxt", saleshierachy.HODNamevtxt, DbType.String);
            dbPara.Add("CompanyCodevtxt", saleshierachy.CompanyCodevtxt, DbType.String);
            dbPara.Add("CompanyNamevtxt", saleshierachy.CompanyNamevtxt, DbType.String);
            dbPara.Add("CreatedBy", saleshierachy.CreatedBy, DbType.String);

            #region using dapper

            var data = _customerPortalHelper.Insert<long>("[dbo].[uspInsertsaleshierachyintoTemp]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;

            #endregion using dapper
        }

        public long InsertSalesHierachyIntoMainTable()
        {
            var dbPara = new DynamicParameters();
            var data = _customerPortalHelper.Execute("[dbo].[uspInsertsaleshierachyintoMain]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public long DeleteTempSaleshierachy()
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("mode", "Delete", DbType.String);
            var data = _customerPortalHelper.Execute("[dbo].[uspgetdeleteTempSalesHierachy]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public List<SalesHierachy> GetTempSalesHierachy()
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("mode", "Get", DbType.String);
            var data = _customerPortalHelper.GetAll<SalesHierachy>("[dbo].[uspgetdeleteTempSalesHierachy]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public List<SalesHierachy> GetSalesHierachy(int PageNo, int PageSize, string KeyWord)
        {
            var dbPara = new DynamicParameters();
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
            var data = _customerPortalHelper.GetAll<SalesHierachy>("[dbo].[uspviewGetSalesHierachy]",
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

        public List<SalesHierachy> DownloadSalesHierachy(string KeyWord)
        {
            var dbPara = new DynamicParameters();
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<SalesHierachy>("[dbo].[uspviewDownloadSalesHierachyData]",
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

        public long GetSalesHierachyCount(string KeyWord)
        {
            var dbPara = new DynamicParameters();
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<SalesHierachy>("[dbo].[uspviewDownloadSalesHierachyData]",
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

        public int InsertUserMasterIntoTemp(UploadEmployeeModel UserMasterDetails)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("UserCodetxt", UserMasterDetails.UserCodetxt, DbType.String);
            dbPara.Add("UserNametxt", UserMasterDetails.UserNametxt, DbType.String);
            dbPara.Add("UserTypetxt", UserMasterDetails.UserTypetxt, DbType.String);
            dbPara.Add("Divisionvtxt", UserMasterDetails.Divisionvtxt, DbType.String);
            dbPara.Add("Mobilevtxt", UserMasterDetails.Mobilevtxt, DbType.String);
            dbPara.Add("Emailvtxt", UserMasterDetails.Emailvtxt, DbType.String);
            dbPara.Add("Passwordvtxt", Encrypt(UserMasterDetails.Passwordvtxt), DbType.String);
            dbPara.Add("ParentCode", UserMasterDetails.ParentCode, DbType.String);
            dbPara.Add("CreatedByint", 1, DbType.Int32);
            dbPara.Add("CreatedDatedatetime", DateTime.Now, DbType.DateTime);

            #region using dapper

            var data = _customerPortalHelper.Insert<int>("[dbo].[uspInsertUserMasterIntoTemp]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;

            #endregion using dapper
        }

        public int InsertUserMasterIntoTempkAM(UploadEmployeeModel UserMasterDetails)
        {
            string[] parentcodes = UserMasterDetails.ParentCode.Split(",");
            if (parentcodes.Length > 0)
            {
                for (int i = 0; i < parentcodes.Length; i++) 
                {
                    var dbPara = new DynamicParameters();
                    dbPara.Add("UserCodetxt", UserMasterDetails.UserCodetxt, DbType.String);
                    dbPara.Add("UserNametxt", UserMasterDetails.UserNametxt, DbType.String);
                    dbPara.Add("UserTypetxt", UserMasterDetails.UserTypetxt, DbType.String);
                    dbPara.Add("Divisionvtxt", UserMasterDetails.Divisionvtxt, DbType.String);
                    dbPara.Add("Mobilevtxt", UserMasterDetails.Mobilevtxt, DbType.String);
                    dbPara.Add("Emailvtxt", UserMasterDetails.Emailvtxt, DbType.String);
                    dbPara.Add("Passwordvtxt", Encrypt(UserMasterDetails.Passwordvtxt), DbType.String);
                    dbPara.Add("ParentCode", parentcodes[i], DbType.String);
                    dbPara.Add("CreatedByint", 1, DbType.Int32);
                    dbPara.Add("CreatedDatedatetime", DateTime.Now, DbType.DateTime);


                    var data = _customerPortalHelper.Insert<int>("[dbo].[uspInsertUserMasterIntoTemp]",
                                    dbPara,
                                    commandType: CommandType.StoredProcedure);
                    //return data;
                }
                return 1;
            }
            else
            {
                var dbPara = new DynamicParameters();
                dbPara.Add("UserCodetxt", UserMasterDetails.UserCodetxt, DbType.String);
                dbPara.Add("UserNametxt", UserMasterDetails.UserNametxt, DbType.String);
                dbPara.Add("UserTypetxt", UserMasterDetails.UserTypetxt, DbType.String);
                dbPara.Add("Divisionvtxt", UserMasterDetails.Divisionvtxt, DbType.String);
                dbPara.Add("Mobilevtxt", UserMasterDetails.Mobilevtxt, DbType.String);
                dbPara.Add("Emailvtxt", UserMasterDetails.Emailvtxt, DbType.String);
                dbPara.Add("Passwordvtxt", Encrypt(UserMasterDetails.Passwordvtxt), DbType.String);
                dbPara.Add("ParentCode", "", DbType.String);
                dbPara.Add("CreatedByint", 1, DbType.Int32);
                dbPara.Add("CreatedDatedatetime", DateTime.Now, DbType.DateTime);

                #region using dapper

                var data = _customerPortalHelper.Insert<int>("[dbo].[uspInsertUserMasterIntoTemp]",
                                dbPara,
                                commandType: CommandType.StoredProcedure);
                return data;
            }

            #endregion using dapper
        }

        public long InsertUserMasterIntoMainkAM(string UserType)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("UserType", UserType, DbType.String);
            var data = _customerPortalHelper.Execute("[dbo].[uspInsertUserMasterintoMain]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public long InsertUserMasterIntoMain(string UserType)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("UserType", UserType, DbType.String);
            var data = _customerPortalHelper.Execute("[dbo].[uspInsertUserMasterintoMain]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public long DeleteTempUserMaster(string UserType)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("UserType", UserType, DbType.String);
            dbPara.Add("mode", "Delete", DbType.String);
            var data = _customerPortalHelper.Execute("[dbo].[uspgetdeleteTempUserMaster]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public List<UploadEmployeeModel> GetTempUserMaster(string UserType)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("UserType", UserType, DbType.String);
            dbPara.Add("mode", "Get", DbType.String);
            var data = _customerPortalHelper.GetAll<UploadEmployeeModel>("[dbo].[uspgetdeleteTempUserMaster]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            List<UploadEmployeeModel> users = data.ToList();
            List<UploadEmployeeModel> rstusers = new List<UploadEmployeeModel>();
            if (users.Count > 0)
            {
                for (int i = 0; i < users.Count; i++)
                {
                    UploadEmployeeModel model = new UploadEmployeeModel();
                    model.UserCodetxt = users[i].UserCodetxt;
                    model.UserNametxt = users[i].UserNametxt;
                    model.Idbint = users[i].Idbint;
                    model.UserTypetxt = users[i].UserTypetxt;
                    model.Mobilevtxt = users[i].Mobilevtxt;
                    model.Emailvtxt = users[i].Emailvtxt;
                    model.Divisionvtxt = users[i].Divisionvtxt;
                    model.Passwordvtxt = Decrypt(users[i].Passwordvtxt);
                    model.Remarks = users[i].Remarks;
                    model.CreatedByint = users[i].CreatedByint;
                    model.CreatedDatedatetime = users[i].CreatedDatedatetime;
                    rstusers.Add(model);
                }
            }
            return rstusers;
        }

        public long InsertOrderAnalystMapping(OrderAnalystMappingModel model)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("UserCodevtxt", model.UserCodevtxt, DbType.String);
            dbPara.Add("CustomerTypevtxt", model.CustomerTypevtxt, DbType.String);
            dbPara.Add("SalesOfficeCodevtxt", model.SalesOfficeCodevtxt, DbType.String);
            dbPara.Add("SalesOfficeNamevtxt", model.SalesOfficeNamevtxt, DbType.String);
            dbPara.Add("Createdbyvtxt", model.Createdbyvtxt, DbType.String);
            dbPara.Add("CreatedDatetimedatetime", model.CreatedDatetimedatetime, DbType.DateTime);

            #region using dapper

            var data = _customerPortalHelper.Insert<long>("[dbo].[uspInsertOrderAnalystMapping]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
            #endregion using dapper

        }

        public List<OrderAnalystMappingModel> GetOrderAnalystMappingData(string usercode)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("usercode", usercode, DbType.String);
            var data = _customerPortalHelper.GetAll<OrderAnalystMappingModel>("[dbo].[uspviewOrderanalystmappingdata]",
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

        public long Delete(string usercode)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("usercode", usercode, DbType.String);
            var data = _customerPortalHelper.Execute("[dbo].[uspdeleteOrderAnalystData]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public List<SalesHierachy> GetSalesOffices()
        {
            var dbPara = new DynamicParameters();
            var data = _customerPortalHelper.GetAll<SalesHierachy>("[dbo].[uspviewSalesOffices]",
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

        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
    }
}