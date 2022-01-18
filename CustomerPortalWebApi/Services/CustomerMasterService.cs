using CustomerPortalWebApi.Entities;
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
    public class CustomerMasterService : ICustomerMasterService
    {
        private readonly ICustomerPortalHelper _customerPortalHelper;

        string ICustomerMasterService.EditProfile(UserMaster UserMaster)
        {
            var dbPara = new DynamicParameters();

            dbPara.Add("IDbint", UserMaster.Idbint, DbType.Int64);
            dbPara.Add("UserNametxt", UserMaster.UserNametxt, DbType.String);
            dbPara.Add("Mobilevtxt", UserMaster.Mobilevtxt, DbType.String);

            var data = _customerPortalHelper.Update<string>("[dbo].[uspUpdateCustomerProfile]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public CustomerMasterService(ICustomerPortalHelper customerPortalHelper)
        {
            _customerPortalHelper = customerPortalHelper;
        }

        public List<CustomerMaster> GetCustomerMaster(string Division, int pageNo, int pageSize, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("Division", Division, DbType.String);
            dbPara.Add("PageNo", pageNo, DbType.Int32);
            dbPara.Add("PageSize", pageSize, DbType.Int32);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<CustomerMaster>("[dbo].[uspviewGetCustomerByDivisionwise]", dbPara, commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public List<CustomerMaster> GetCustomerDataByUserId(string UserId)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("UserId", UserId, DbType.String);

            var data = _customerPortalHelper.GetAll<CustomerMaster>("[dbo].[uspviewGetCustomerByUserId]", dbPara, commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public List<CustomerMaster> GetCustomerByUser(string UserCode)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("UserCode", UserCode, DbType.String);

            var data = _customerPortalHelper.GetAll<CustomerMaster>("[dbo].[uspviewGetCustomerByUserCode]", dbPara, commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public List<ShipToModel> GetShipTo(string CustomerCode)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("customercode", CustomerCode, DbType.String);
            var data = _customerPortalHelper.GetAll<ShipToModel>("[dbo].[uspviewShipToByCustomerCode]", dbPara, commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public long CustomerCount(string Division)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("Division", Division, DbType.String);
            var data = _customerPortalHelper.GetAll<Count>("[dbo].[uspviewgetAllCustomerCount]", dbPara, commandType: CommandType.StoredProcedure);
           
            if (data != null)
            {
                return Convert.ToInt64(data[0].ListCount);
            }
            else
            {
                return 0;
            }
        }

        public List<ShipToModel> GetShipToAddress(string ShipToCode)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("ShipToCode", ShipToCode, DbType.String);
            var data = _customerPortalHelper.GetAll<ShipToModel>("[dbo].[uspviewShipToAddressByShipToCode]", dbPara, commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public List<CustomerMasterModel> CheckValidCustomer(string customercode, string accesskey)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("customercode", customercode, DbType.String);
            dbPara.Add("accesstoken", accesskey, DbType.String);
            var data = _customerPortalHelper.GetAll<CustomerMasterModel>("[dbo].[uspviewCheckValidCustomer]", dbPara, commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        long ICustomerMasterService.update(CustomerMasterModel Customermasterdeatils)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("customercode", Customermasterdeatils.CustCodevtxt, DbType.String);
            dbPara.Add("customername", Customermasterdeatils.CustNamevtxt, DbType.String);
            dbPara.Add("Mobile", Customermasterdeatils.TelNumber1vtxt, DbType.String);
            dbPara.Add("Email", Customermasterdeatils.Emailvtxt, DbType.String);
            dbPara.Add("Address", Customermasterdeatils.Address1vtxt, DbType.String);
            dbPara.Add("ContactPerson", Customermasterdeatils.Contactpersonvtxt, DbType.String);
            dbPara.Add("Password", Encrypt(Customermasterdeatils.Password), DbType.String);

            #region using dapper

            var data = _customerPortalHelper.Update<long>("[dbo].[uspUpdateCustomerMaster]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;

            #endregion using dapper
        }

        long ICustomerMasterService.UpdateCustomerStatus(CustomerMasterModel Customermasterdeatils)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("customercode", Customermasterdeatils.CustCodevtxt, DbType.String);
            dbPara.Add("isActive", Customermasterdeatils.IsActivebit, DbType.Boolean);
            dbPara.Add("UpdateBy", Customermasterdeatils.Modifiertxt, DbType.String);

            #region using dapper

            var data = _customerPortalHelper.Update<long>("[dbo].[uspUpdateCustomerMasterStatus]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;

            #endregion using dapper
        }

        public List<CustomerMasterModel> GetCustomerMasterUserType(string usercode, string usertype, int PageNo, int PageSize, string KeyWord, Boolean status, Boolean isActive)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("usercode", usercode, DbType.String);
            dbPara.Add("usertype", usertype, DbType.String);
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
            dbPara.Add("status", status, DbType.Boolean);
            dbPara.Add("isActive", isActive, DbType.Boolean);
            var data = _customerPortalHelper.GetAll<CustomerMasterModel>("[dbo].[uspviewCustomerlistByUserTypeSearch]", dbPara, commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public long GetCustomerMasterUserTypeCount(string usercode, string usertype, string KeyWord, Boolean status, Boolean isActive)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("usercode", usercode, DbType.String);
            dbPara.Add("usertype", usertype, DbType.String);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            dbPara.Add("status", status, DbType.Boolean);
            dbPara.Add("isActive", isActive, DbType.Boolean);
            var data = _customerPortalHelper.GetAll<Count>("[dbo].[uspviewCustomerlistByUserTypeCount]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            if (data != null)
            {
                return Convert.ToInt64(data[0].ListCount);
            }
            else
            {
                return 0;
            }
        }

        public List<CustomerMasterModel> GetCustomerMasterUserTypeDownload(string usercode, string usertype, string KeyWord, Boolean status, Boolean isActive)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("usercode", usercode, DbType.String);
            dbPara.Add("usertype", usertype, DbType.String);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            dbPara.Add("status", status, DbType.Boolean);
            dbPara.Add("isActive", isActive, DbType.Boolean);
            var data = _customerPortalHelper.GetAll<CustomerMasterModel>("[dbo].[uspviewCustomerlistByUserTypeDownload]",
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

        public List<CustomerMasterModel> GetCustomerMasterSystemAdminSearch(int PageNo, int PageSize, string status, string division, string KeyWord)
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
            dbPara.Add("status", status, DbType.String);
            dbPara.Add("division", division, DbType.String);
            var data = _customerPortalHelper.GetAll<CustomerMasterModel>("[dbo].[uspviewCustomerlistSearch]", dbPara, commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public long GetCustomerMasterSystemAdminSearchCount(string status, string division, string KeyWord)
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
            dbPara.Add("status", status, DbType.String);
            dbPara.Add("division", division, DbType.String);
            var data = _customerPortalHelper.GetAll<CustomerMasterModel>("[dbo].[uspviewCustomerlistDownlaod]", dbPara, commandType: CommandType.StoredProcedure);
            if (data != null)
            {
                return Convert.ToInt64(data.ToList().Count);
            }
            else
            {
                return 0;
            }
        }

        public List<CustomerMasterModel> GetCustomerMasterSystemAdminDownload(string status, string division, string KeyWord)
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
            dbPara.Add("status", status, DbType.String);
            dbPara.Add("division", division, DbType.String);
            var data = _customerPortalHelper.GetAll<CustomerMasterModel>("[dbo].[uspviewCustomerlistDownlaod]", dbPara, commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public List<CustomerLedger> GetCustomerLedger(string CustomerCode, int PageNo, int PageSize)
        {
            var dbPara = new DynamicParameters();

            dbPara.Add("CustomerCodevtxt", CustomerCode, DbType.String);
            dbPara.Add("PageNo", PageNo, DbType.Int32);
            dbPara.Add("PageSize", PageSize, DbType.Int32);
            var data = _customerPortalHelper.GetAll<CustomerLedger>("[dbo].[uspviewCustomerLedger]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public long GetCustomerLedgerCount(string CustomerCode)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("CustomerCodevtxt", CustomerCode, DbType.String);
            var data = _customerPortalHelper.GetAll<CustomerLedger>("[dbo].[uspviewCustomerLedgerCount]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            
            if (data != null)
            {
                return Convert.ToInt64(data.Count);
            }
            else
            {
                return 0;
            }
        }

        public List<CustomerLedger> GetCustomerLedgerForExportToExcel(string CustomerCode)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("CustomerCodevtxt", CustomerCode, DbType.String);
            var data = _customerPortalHelper.GetAll<CustomerLedger>("[dbo].[uspviewCustomerLedgerCount]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
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

        public List<CustomerMaster> GetCustomerDataforKAM(string UserId)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("UserCode", UserId, DbType.String);
            var data = _customerPortalHelper.GetAll<CustomerMaster>("[dbo].[uspviewGetCustomerforKAM]", dbPara, commandType: CommandType.StoredProcedure);
            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public List<PlantMasterModel> GetPlantMaster(string customercode)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("CustomerCode", customercode, DbType.String);
            var data = _customerPortalHelper.GetAll<PlantMasterModel>("[dbo].[GetPlantByCustomerCode]", dbPara, commandType: CommandType.StoredProcedure);
            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

         


        public int  UploadMason (MasonModel Model)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("Idbint", Model.Idbint, DbType.Int32);
            dbPara.Add("MasonCodetxt", Model.MasonCodetxt, DbType.String);
            dbPara.Add("MasonNametxt", Model.MasonNametxt, DbType.String);
            dbPara.Add("MasonMobileNumber", Model.MasonMobileNumber, DbType.String);
            dbPara.Add("MasonCustomerCode", Model.MasonCustomerCode, DbType.String);
            var data = _customerPortalHelper.Get<int>("[dbo].[uspInsertUpdateMason]", dbPara, commandType: CommandType.StoredProcedure);
            return data;
        }
        public List<MasonModel> GetTempMason(int PageNo, int PageSize,string keyword)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("PageNo", PageNo, DbType.Int32);
            dbPara.Add("PageSize", PageSize, DbType.Int32);
            if (keyword == "NoSearch")
            {

                dbPara.Add("keyword", "", DbType.String);
            }
            else
            {
                dbPara.Add("keyword", keyword, DbType.String);

            }
            var data = _customerPortalHelper.GetAll<MasonModel>("[dbo].[USP_GetMasonMaster]", dbPara, commandType: CommandType.StoredProcedure);
            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }
    }


}