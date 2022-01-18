using CustomerPortalWebApi.Entities;
using CustomerPortalWebApi.Interface;
using CustomerPortalWebApi.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CustomerPortalWebApi.Services
{
    public class RoleService : IRoleService
    {
        private readonly ICustomerPortalHelper _customerPortalHelper;

        public RoleService(ICustomerPortalHelper customerPortalHelper)
        {
            _customerPortalHelper = customerPortalHelper;
        }

        public long Create(RoleMasterModel Roleheader)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("IDbint", Roleheader.IDbint, DbType.Int64);
            dbPara.Add("RoleNamevtxt", Roleheader.RoleNamevtxt, DbType.String);
            dbPara.Add("RoleDescriptionvtxt", Roleheader.RoleDescriptionvtxt, DbType.String);
            dbPara.Add("Mode", "A", DbType.String);
            dbPara.Add("CreatedByvtxt", Roleheader.CreatedByvtxt, DbType.String);
            dbPara.Add("CreatedDatedatetime", DateTime.Now, DbType.DateTime);

            #region using dapper

            var data = _customerPortalHelper.Insert<long>("[dbo].[uspInsertUpdateRoleMaster]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;

            #endregion using dapper
        }

        long IRoleService.update(RoleMasterModel Roleheader)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("IDbint", Roleheader.IDbint, DbType.Int64);
            dbPara.Add("RoleNamevtxt", Roleheader.RoleNamevtxt, DbType.String);
            dbPara.Add("RoleDescriptionvtxt", Roleheader.RoleDescriptionvtxt, DbType.String);
            dbPara.Add("Mode", "M", DbType.String);
            dbPara.Add("CreatedByvtxt", Roleheader.CreatedByvtxt, DbType.String);
            dbPara.Add("CreatedDatedatetime", DateTime.Now, DbType.DateTime);

            var data = _customerPortalHelper.Insert<long>("[dbo].[uspInsertUpdateRoleMaster]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public long Delete(long id)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("RoleID", id, DbType.Int64);
            var data = _customerPortalHelper.Execute("[dbo].[uspDeleteRoleDetails]", dbPara,
                   commandType: CommandType.StoredProcedure);
            return data;
        }

        public long InserRoleDetails(RoleDetailsModel RoleDetail)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("HeaderIDbint", RoleDetail.HeaderIDbint, DbType.Int64);
            dbPara.Add("RoleIDint", RoleDetail.RoleIDint, DbType.Int16);
            dbPara.Add("RoleNamevtxt", RoleDetail.RoleNamevtxt, DbType.String);

            #region using dapper

            var data = _customerPortalHelper.Insert<long>("[dbo].[uspInsertRoleDetails]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;

            #endregion using dapper
        }

        public List<RoleMasterModel> GetRoleMaster(int PageNo, int PageSize, string KeyWord)
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
            var data = _customerPortalHelper.GetAll<RoleMasterModel>("[dbo].[uspviewGetRoleMaster]",
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

        public List<RolesModel> GetRoleS()
        {
            var dbPara = new DynamicParameters();
            var data = _customerPortalHelper.GetAll<RolesModel>("[dbo].[uspviewgetroles]",
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

        public List<RoleMasterModel> GetRoleMasterByRoleID(long RoleID)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("RoleID", RoleID, DbType.Int32);
            var data = _customerPortalHelper.GetAll<RoleMasterModel>("[dbo].[uspviewGetRoleHeaderByRoleID]",
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

        public List<RoleDetailsModel> GetRoleDetailsByRoleID(long RoleID)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("RoleID", RoleID, DbType.Int32);
            var data = _customerPortalHelper.GetAll<RoleDetailsModel>("[dbo].[uspviewGetRoleDetailsByRoleID]",
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

        public long GetRoleMasterCount(string KeyWord)
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
            var data = _customerPortalHelper.GetAll<Count>("[dbo].[uspviewGetRoleMasterCount]", dbPara, commandType: CommandType.StoredProcedure); 
            if (data != null)
            {
                return Convert.ToInt64(data.ToList().Count);
            }
            else
            {
                return 0;
            }
        }

        public List<RoleMasterModel> GetRoleMasterForcheckBoxlist(string KeyWord)
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
            var data = _customerPortalHelper.GetAll<RoleMasterModel>("[dbo].[uspviewGetRoleMasterForList]",
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

        public CustomerMasterModel GetUserRolesDetailsByCustomercode(string CustomerCode)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("CustomerCode", CustomerCode, DbType.String);

            var data = _customerPortalHelper.Get<CustomerMasterModel>("[dbo].[uspviewGetCustomerType]", dbPara, commandType: CommandType.StoredProcedure);
            return data;
        }
    }
}