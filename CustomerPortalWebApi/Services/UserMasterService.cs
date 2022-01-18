using CustomerPortalWebApi.Entities;
using CustomerPortalWebApi.Interface;
using CustomerPortalWebApi.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CustomerPortalWebApi.Services
{
    public class UserMasterService : IUserMasterService
    {
        private readonly IConfiguration _config;
        private readonly ICustomerPortalHelper _customerPortalHelper;
        private readonly ILogger _ILogger;

        public UserMasterService(ICustomerPortalHelper customerPortalHelper, ILogger ILoggerservice, IConfiguration config)
        {
            _config = config;
            _ILogger = ILoggerservice;
            _customerPortalHelper = customerPortalHelper;
        }

        public List<ErrorModel> GetError(string fromdate, string todate, int pageNo, int pageSize, string KeyWord, string Type)
        {
            var dbPara = new DynamicParameters();
            DateTime fdate = DateTime.ParseExact(fromdate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(todate, "dd-MM-yyyy", null);
            dbPara.Add("fromdate", Convert.ToDateTime(fdate).ToString("yyyy-MM-dd"), DbType.DateTime);
            dbPara.Add("todate", Convert.ToDateTime(tdate).ToString("yyyy-MM-dd"), DbType.DateTime);
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
            dbPara.Add("Type", Type, DbType.String);
            var data = _customerPortalHelper.GetAll<ErrorModel>("[dbo].[uspviewError]", dbPara, commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public int Create(UserMaster UserMasterDetails)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("UserCodetxt", UserMasterDetails.UserCodetxt, DbType.String);
            dbPara.Add("UserNametxt", UserMasterDetails.UserNametxt, DbType.String);
            dbPara.Add("UserTypetxt", UserMasterDetails.UserTypetxt, DbType.String);
            dbPara.Add("ParentCodevtxt", UserMasterDetails.ParentCodevtxt, DbType.String);
            dbPara.Add("Divisionvtxt", UserMasterDetails.Divisionvtxt, DbType.String);
            dbPara.Add("Mobilevtxt", UserMasterDetails.Mobilevtxt, DbType.String);
            dbPara.Add("Emailvtxt", UserMasterDetails.Emailvtxt, DbType.String);
            dbPara.Add("IsActivebit", UserMasterDetails.IsActivebit, DbType.Boolean);
            dbPara.Add("Passwordvtxt", UserMasterDetails.Passwordvtxt, DbType.String);
            dbPara.Add("CreatedByint", 1, DbType.Int32);
            dbPara.Add("CreatedDatedatetime", DateTime.Now, DbType.DateTime);
            dbPara.Add("ModifyByint", 1, DbType.Int32);
            dbPara.Add("ModifyDatedatetime", DateTime.Now, DbType.DateTime);

            #region using dapper

            var data = _customerPortalHelper.Insert<int>("[dbo].[uspInsertUserMaster]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;

            #endregion using dapper
        }

        public long Delete(long id)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("IDbint", id, DbType.Int64);
            var data = _customerPortalHelper.Execute("[dbo].[uspDeleteUserMaster]", dbPara,
                   commandType: CommandType.StoredProcedure);
            return data;
        }

        public UserMaster GetById(long id)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("IDbint", id, DbType.Int64);

            #region using dapper

            var data = _customerPortalHelper.Get<UserMaster>("[dbo].[uspviewUserMasterById]", dbPara,
                    commandType: CommandType.StoredProcedure);
            if (data != null)
            {
                UserMaster users = data;

                UserMaster model = new UserMaster();
                model.UserCodetxt = users.UserCodetxt;
                model.UserNametxt = users.UserNametxt;
                model.Idbint = users.Idbint;
                model.UserTypetxt = users.UserTypetxt;
                model.Mobilevtxt = users.Mobilevtxt;
                model.Emailvtxt = users.Emailvtxt;
                model.Divisionvtxt = users.Divisionvtxt;
                model.IsActivebit = users.IsActivebit;
                model.Passwordvtxt = EncryptAngularStringAES(Decrypttxt(users.Passwordvtxt));
                model.IsActivebit = users.IsActivebit;
                model.ModifyByint = users.ModifyByint;
                model.ModifyDatedatetime = users.ModifyDatedatetime;
                model.CreatedByint = users.CreatedByint;
                model.ParentCodevtxt = users.ParentCodevtxt;
                model.CreatedDatedatetime = users.CreatedDatedatetime;
                try
                {
                    var data2 = UserDetailById(id);
                    if (data2 != null)
                    {
                        model.SalesOfficeCodevtxt = data2.SalesOfficeCodevtxt;
                        model.SalesOfficeNamevtxt = data2.SalesOfficeNamevtxt;
                        model.BranchCodevtxt = data2.BranchCodevtxt;
                        model.BranchNamevtxt = data2.BranchNamevtxt;
                        model.RegionCodevtxt = data2.RegionCodevtxt;
                        model.RegionDescriptionvtxt = data2.RegionDescriptionvtxt;
                        model.ZoneCodevtxt = data2.ZoneCodevtxt;
                        model.ZoneDescriptionvtxt = data2.ZoneDescriptionvtxt;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return model;

                }
                return model;
            }
            else {
                return null;
            }
            #endregion using dapper
        }

        public UserMaster UserDetailById(long id)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("IDbint", id, DbType.Int64);

            #region using dapper

            var data = _customerPortalHelper.Get<UserMaster>("[dbo].[uspviewUserDetailById]", dbPara,
                    commandType: CommandType.StoredProcedure);
            return data;
            #endregion using dapper
        }

        public List<UserMaster> ListAll()
        {
            var data = _customerPortalHelper.GetAll<UserMaster>("[dbo].[uspviewAllUserMaster]", null, commandType: CommandType.StoredProcedure);
            List<UserMaster> users = data.ToList();
            List<UserMaster> rstusers = new List<UserMaster>();
            if (users.Count > 0)
            {
                for (int i = 0; i < users.Count; i++)
                {
                    UserMaster model = new UserMaster();
                    model.UserCodetxt = users[i].UserCodetxt;
                    model.UserNametxt = users[i].UserNametxt;
                    model.Idbint = users[i].Idbint;
                    model.UserTypetxt = users[i].UserTypetxt;
                    model.Mobilevtxt = users[i].Mobilevtxt;
                    model.Emailvtxt = users[i].Emailvtxt;
                    model.Divisionvtxt = users[i].Divisionvtxt;
                    model.IsActivebit = users[i].IsActivebit;
                    model.Passwordvtxt = Decrypttxt(users[i].Passwordvtxt);
                    model.IsActivebit = users[i].IsActivebit;
                    model.ModifyByint = users[i].ModifyByint;
                    model.ModifyDatedatetime = users[i].ModifyDatedatetime;
                    model.CreatedByint = users[i].CreatedByint;
                    model.CreatedDatedatetime = users[i].CreatedDatedatetime;
                    rstusers.Add(model);
                }
            }
            return rstusers;
        }

        string IUserMasterService.Update(UserMaster UserMaster)
        {
            var dbPara = new DynamicParameters();

            dbPara.Add("IDbint", UserMaster.Idbint, DbType.Int64);
            dbPara.Add("UserCodetxt", UserMaster.UserCodetxt, DbType.String);
            dbPara.Add("UserNametxt", UserMaster.UserNametxt, DbType.String);
            dbPara.Add("UserTypetxt", UserMaster.UserTypetxt, DbType.String);
            dbPara.Add("Divisionvtxt", UserMaster.Divisionvtxt, DbType.String);
            dbPara.Add("Mobilevtxt", UserMaster.Mobilevtxt, DbType.String);
            dbPara.Add("Emailvtxt", UserMaster.Emailvtxt, DbType.String);
            dbPara.Add("Passwordvtxt", UserMaster.Passwordvtxt, DbType.String);
            dbPara.Add("IsActivebit", UserMaster.IsActivebit, DbType.Boolean);
            dbPara.Add("ModifyByint", 1, DbType.Int32);
            dbPara.Add("ModifyDatedatetime", DateTime.Now, DbType.DateTime);

            var data = _customerPortalHelper.Update<string>("[dbo].[uspUpdateUserMaster]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }
        string IUserMasterService.UpdateStatus(UserMaster UserMaster)
        {
            var dbPara = new DynamicParameters();

            dbPara.Add("IDbint", UserMaster.Idbint, DbType.Int64);
            dbPara.Add("UserCodetxt", UserMaster.UserCodetxt, DbType.String);
            dbPara.Add("UserNametxt", UserMaster.UserNametxt, DbType.String);
            dbPara.Add("UserTypetxt", UserMaster.UserTypetxt, DbType.String);
            dbPara.Add("Divisionvtxt", UserMaster.Divisionvtxt, DbType.String);
            dbPara.Add("Mobilevtxt", UserMaster.Mobilevtxt, DbType.String);
            dbPara.Add("Emailvtxt", UserMaster.Emailvtxt, DbType.String);
            dbPara.Add("Passwordvtxt", UserMaster.Passwordvtxt, DbType.String);
            dbPara.Add("IsActivebit", UserMaster.IsActivebit, DbType.Boolean);
            dbPara.Add("ModifyByint", 1, DbType.Int32);
            dbPara.Add("ModifyDatedatetime", DateTime.Now, DbType.DateTime);

            var data = _customerPortalHelper.Update<string>("[dbo].[uspUpdateUserMasterStatus]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        string IUserMasterService.EmployeeUpdate(UserMaster UserMaster)
        {
            var dbPara = new DynamicParameters();

            dbPara.Add("IDbint", UserMaster.Idbint, DbType.Int64);
            dbPara.Add("UserCodetxt", UserMaster.UserCodetxt, DbType.String);
            dbPara.Add("UserNametxt", UserMaster.UserNametxt, DbType.String);
            dbPara.Add("UserTypetxt", UserMaster.UserTypetxt, DbType.String);
            dbPara.Add("Divisionvtxt", UserMaster.Divisionvtxt, DbType.String);
            dbPara.Add("Mobilevtxt", UserMaster.Mobilevtxt, DbType.String);
            dbPara.Add("Emailvtxt", UserMaster.Emailvtxt, DbType.String);
            dbPara.Add("Passwordvtxt", UserMaster.Passwordvtxt, DbType.String);
            dbPara.Add("IsActivebit", UserMaster.IsActivebit, DbType.Boolean);
            dbPara.Add("ModifyByint", 1, DbType.Int32);
            dbPara.Add("ModifyDatedatetime", DateTime.Now, DbType.DateTime);
            dbPara.Add("ParentCodevtxt", UserMaster.ParentCodevtxt, DbType.String);

            var data = _customerPortalHelper.Update<string>("[dbo].[uspUpdateEmployee]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        string IUserMasterService.ChangePassword(UserMaster UserMaster)
        {
            var dbPara = new DynamicParameters();

            dbPara.Add("IDbint", UserMaster.Idbint, DbType.Int64);
            dbPara.Add("Passwordvtxt", UserMaster.Passwordvtxt, DbType.String);
            dbPara.Add("NewPassword", UserMaster.NewPassword, DbType.String);

            var data = _customerPortalHelper.Update<string>("[dbo].[uspUpdateUserPassword]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        string IUserMasterService.EditProfile(UserMaster UserMaster)
        {
            var dbPara = new DynamicParameters();

            dbPara.Add("IDbint", UserMaster.Idbint, DbType.Int64);
            dbPara.Add("UserNametxt", UserMaster.UserNametxt, DbType.String);
            dbPara.Add("Mobilevtxt", UserMaster.Mobilevtxt, DbType.String);

            var data = _customerPortalHelper.Update<string>("[dbo].[uspUpdateUserProfile]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public List<UserMaster> Search(string KeyWord)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("search", KeyWord, DbType.String);
            var data = _customerPortalHelper.GetAll<UserMaster>("[dbo].[uspviewSearchUserByUserCode]", dbPara, commandType: CommandType.StoredProcedure);
            List<UserMaster> users = data.ToList();
            List<UserMaster> rstusers = new List<UserMaster>();
            if (users.Count > 0)
            {
                for (int i = 0; i < users.Count; i++)
                {
                    UserMaster model = new UserMaster();
                    model.UserCodetxt = users[i].UserCodetxt;
                    model.UserNametxt = users[i].UserNametxt;
                    model.Idbint = users[i].Idbint;
                    model.UserTypetxt = users[i].UserTypetxt;
                    model.Mobilevtxt = users[i].Mobilevtxt;
                    model.Emailvtxt = users[i].Emailvtxt;
                    model.Divisionvtxt = users[i].Divisionvtxt;
                    model.IsActivebit = users[i].IsActivebit;
                    model.Passwordvtxt = Decrypttxt(users[i].Passwordvtxt);
                    model.IsActivebit = users[i].IsActivebit;
                    model.ModifyByint = users[i].ModifyByint;
                    model.ModifyDatedatetime = users[i].ModifyDatedatetime;
                    model.CreatedByint = users[i].CreatedByint;
                    model.CreatedDatedatetime = users[i].CreatedDatedatetime;
                    rstusers.Add(model);
                }
            }
            return rstusers;
        }

        public List<UserMaster> UserPaging(int pageNo, int pageSize)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("PageNo", pageNo, DbType.Int32);
            dbPara.Add("PageSize", pageSize, DbType.Int32);
            var data = _customerPortalHelper.GetAll<UserMaster>("[dbo].[uspviewUserPaging]", dbPara, commandType: CommandType.StoredProcedure);
            return data.ToList();
        }

        public UserMaster Login(string usercode, string password)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("usercode", usercode, DbType.String);
            dbPara.Add("password", password, DbType.String);
            var data = _customerPortalHelper.Get<UserMaster>("[dbo].[uspviewCheckUser]", dbPara, commandType: CommandType.StoredProcedure);
            return data;
        }

        public UserMaster LoginLogs(string UserCode, string UserName, string UserType, string BrowserName, string IpAddress)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("UserCode", UserCode, DbType.String);
            dbPara.Add("UserName", UserName, DbType.String);
            dbPara.Add("UserType", UserType, DbType.String);
            dbPara.Add("BrowserName", BrowserName, DbType.String);
            dbPara.Add("IpAddress", IpAddress, DbType.String);
            var data = _customerPortalHelper.Get<UserMaster>("[dbo].[uspInsertLoginLogs]", dbPara, commandType: CommandType.StoredProcedure);
            return data;
        }

        public long DeleteErrorLog(string DelDate)
        {
            var dbPara = new DynamicParameters();
            DateTime tempDelDate = DateTime.ParseExact(DelDate, "dd-MM-yyyy", null);
            dbPara.Add("DeleteDate", Convert.ToDateTime(tempDelDate).ToString("yyyy-MM-dd"), DbType.String);
            var data = _customerPortalHelper.GetAll<Count>("[dbo].[uspDeleteErrorLog]", dbPara, commandType: CommandType.StoredProcedure);
          
            if (data != null)
            {
                return data[0].ListCount;
            }
            else
            {
                return 0;
            }
        }

        public long UserCount()
        {
            var data = _customerPortalHelper.GetAll<Count>("[dbo].[uspviewgetAllUsersCount]", null, commandType: CommandType.StoredProcedure); 
            if (data != null)
            {
                return Convert.ToInt64(data.ToList().Count);
            }
            else
            {
                return 0;
            }
        }

        public int SaveRefreshToken(string usercode, string reftoken)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("UserIDbint", usercode, DbType.String);
            dbPara.Add("Tokentxt", reftoken, DbType.String);
            dbPara.Add("ExpiryDatedatetime", DateTime.Now.AddDays(1), DbType.DateTime);

            #region using dapper

            var data = _customerPortalHelper.Insert<int>("[dbo].[uspInsertRefreshToken]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;

            #endregion using dapper
        }

        public string GetRefreshToken(string usercode, string reftoken)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("usercode", usercode, DbType.String);
            dbPara.Add("Tokentxt", reftoken, DbType.String);
            var data = _customerPortalHelper.GetAll<RefreshToken>("[dbo].[uspviewgettokenbyUsercode]", dbPara, commandType: CommandType.StoredProcedure);
           
            if (data != null)
            {
                return Convert.ToString(data[0].Tokentxt);
            }
            else
            {
                return null;
            }
        }

        public int DeleteRefreshToken(string usercode, string token)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("usercode", usercode, DbType.String);
            dbPara.Add("token", token, DbType.String);
            var data = _customerPortalHelper.Insert<int>("[dbo].[uspdeletetokenbyUsercode]",
                           dbPara,
                           commandType: CommandType.StoredProcedure);
            return data;
        }

        public List<UserMasterModel> GetAllUserMasterForDivisionalAdminSearch(int PageNo, int PageSize, string KeyWord)
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
            var data = _customerPortalHelper.GetAll<UserMasterModel>("[dbo].[uspviewAllUserMasterForDivisionAdminSearch]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            List<UserMasterModel> users = data.ToList();
            List<UserMasterModel> rstusers = new List<UserMasterModel>();
            if (users.Count > 0)
            {
                for (int i = 0; i < users.Count; i++)
                {
                    UserMasterModel model = new UserMasterModel();
                    model.UserCodetxt = users[i].UserCodetxt;
                    model.UserNametxt = users[i].UserNametxt;
                    model.Idbint = users[i].Idbint;
                    model.UserTypetxt = users[i].UserTypetxt;
                    model.Mobilevtxt = users[i].Mobilevtxt;
                    model.Emailvtxt = users[i].Emailvtxt;
                    model.Divisionvtxt = users[i].Divisionvtxt;
                    model.IsActivebit = users[i].IsActivebit;
                    model.Passwordvtxt = Decrypttxt(users[i].Passwordvtxt);
                    model.IsActivebit = users[i].IsActivebit;
                    model.ModifyByint = users[i].ModifyByint;
                    model.ModifyDatedatetime = users[i].ModifyDatedatetime;
                    model.CreatedByint = users[i].CreatedByint;
                    model.CreatedDatedatetime = users[i].CreatedDatedatetime;
                    rstusers.Add(model);
                }
            }
            return rstusers;
        }

        public List<UserMasterModel> GetAllUserMasterByParentCode(string status, string ParentCode, int PageNo, int PageSize, string KeyWord)
        {
            var dbPara = new DynamicParameters();

            dbPara.Add("status", status, DbType.String);
            dbPara.Add("ParentCode", ParentCode, DbType.String);
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
            var data = _customerPortalHelper.GetAll<UserMasterModel>("[dbo].[uspviewAllUserMasterByCustomerId]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            List<UserMasterModel> users = data.ToList();
            List<UserMasterModel> rstusers = new List<UserMasterModel>();
            if (users.Count > 0)
            {
                for (int i = 0; i < users.Count; i++)
                {
                    UserMasterModel model = new UserMasterModel();
                    model.UserCodetxt = users[i].UserCodetxt;
                    model.UserNametxt = users[i].UserNametxt;
                    model.Idbint = users[i].Idbint;
                    model.UserTypetxt = users[i].UserTypetxt;
                    model.Mobilevtxt = users[i].Mobilevtxt;
                    model.Emailvtxt = users[i].Emailvtxt;
                    model.Divisionvtxt = users[i].Divisionvtxt;
                    model.IsActivebit = users[i].IsActivebit;
                    model.Passwordvtxt = Decrypttxt(users[i].Passwordvtxt);
                    model.IsActivebit = users[i].IsActivebit;
                    model.ModifyByint = users[i].ModifyByint;
                    model.ModifyDatedatetime = users[i].ModifyDatedatetime;
                    model.CreatedByint = users[i].CreatedByint;
                    model.CreatedDatedatetime = users[i].CreatedDatedatetime;
                    rstusers.Add(model);
                }
            }
            return rstusers;
        }

        public long GetAllUserMasterCountByParentCode(string status, string ParentCode, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("status", status, DbType.String);
            dbPara.Add("ParentCode", ParentCode, DbType.String);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<Count>("[dbo].[uspviewAllUserMasterCountByCustomerId]",
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

        public List<UserMasterModel> GetAllUserMasterForDivisionalAdminDownload(string KeyWord)
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
            var data = _customerPortalHelper.GetAll<UserMasterModel>("[dbo].[uspviewAllUserMasterForDivisionAdminDownload]",
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

        public List<UserMasterModel> ExportToExcelForParent(string ParentCode, string status, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("ParentCode", ParentCode, DbType.String);
            dbPara.Add("status", status, DbType.String);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("KeyWord", "", DbType.String);
            }
            else
            {
                dbPara.Add("KeyWord", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<UserMasterModel>("[dbo].[uspviewAllUserMasterByParentCodeDownload]",
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

        public long GetAllUserMasterForDivisionalAdminCount(string KeyWord)
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
            var data = _customerPortalHelper.GetAll<UserMasterModel>("[dbo].[uspviewAllUserMasterForDivisionAdminDownload]",
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

        public List<UserRolesHeader> GetUserRolesHeader(string usercode)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("UserCode", usercode, DbType.String);
            var data = _customerPortalHelper.GetAll<UserRolesHeader>("[dbo].[uspviewGetRoleMasterByUserCode]",
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

        public int InsertUserRolesHeader(UserRolesHeader userRoles)
        {
            var dbPara = new DynamicParameters();

            dbPara.Add("RoleIDbint", userRoles.RoleIDbint, DbType.Int64);
            dbPara.Add("UserCodevtxt", userRoles.UserCodevtxt, DbType.String);
            dbPara.Add("CreatedByvtxt", userRoles.CreatedByvtxt, DbType.String);
            dbPara.Add("CreatedDatetimedatetime", DateTime.Now, DbType.DateTime);

            #region using dapper

            var data = _customerPortalHelper.Insert<int>("[dbo].[uspInsertUserRoles]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;

            #endregion using dapper
        }

        public long DeleteUserRoles(string usercode)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("usercode", usercode, DbType.String);
            var data = _customerPortalHelper.Execute("[dbo].[uspDeleteUserRoles]", dbPara,
                   commandType: CommandType.StoredProcedure);
            return data;
        }

        public List<UserRolesDetails> GetUserRolesDetails(string usercode)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("UserCode", usercode, DbType.String);
            var data = _customerPortalHelper.GetAll<UserRolesDetails>("[dbo].[uspviewUserRoleDetails]",
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

        public static string Encrypttxt(string clearText)
        {
            string temptext = DecryptAngularStringAES(clearText);
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(temptext);
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
                    temptext = Convert.ToBase64String(ms.ToArray());
                }
            }
            return temptext;
        }

        public static string Decrypttxt(string cipherText)
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

        public static string DecryptAngularStringAES(string cipherText)
        {
            var keybytes = Encoding.UTF8.GetBytes("4512631236589784");
            var iv = Encoding.UTF8.GetBytes("4512631236589784");

            var encrypted = Convert.FromBase64String(cipherText);
            var decriptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv);
            return decriptedFromJavascript;
        }

        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (var rijAlg = new RijndaelManaged())
            {
                //Settings
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                try
                {
                    // Create the streams used for decryption.
                    using (var msDecrypt = new MemoryStream(cipherText))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream
                                // and place them in a string.
                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
                catch
                {
                    plaintext = "keyError";
                }
            }

            return plaintext;
        }

        public static string EncryptAngularStringAES(string plainText)
        {
            var keybytes = Encoding.UTF8.GetBytes("4512631236589784");
            var iv = Encoding.UTF8.GetBytes("4512631236589784");

            var encryoFromJavascript = EncryptStringToBytes(plainText, keybytes, iv);
            return Convert.ToBase64String(encryoFromJavascript);
        }

        private static byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
            {
                throw new ArgumentNullException("plainText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            byte[] encrypted;
            // Create a RijndaelManaged object
            // with the specified key and IV.
            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.
                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        string IUserMasterService.ResetPassword(UserMaster UserMaster)
        {
            var dbPara = new DynamicParameters();

            dbPara.Add("UserCode", UserMaster.UserCodetxt, DbType.String);
            dbPara.Add("NewPassword", UserMaster.NewPassword, DbType.String);
            dbPara.Add("ResetToken", UserMaster.ResetTokenvtxt, DbType.String);

            var data = _customerPortalHelper.Update<string>("[dbo].[uspResetUserPassword]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }
    }
}