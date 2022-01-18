using CustomerPortalWebApi.Models;
using System.Collections.Generic;

namespace CustomerPortalWebApi.Interface
{
    public interface IRoleService
    {
        long Create(RoleMasterModel Roleheader);

        long update(RoleMasterModel roleDetails);

        long InserRoleDetails(RoleDetailsModel RoleDetail);

        long Delete(long id);

        List<RoleMasterModel> GetRoleMaster(int PageNo, int PageSize, string KeyWord);

        List<RolesModel> GetRoleS();

        List<RoleMasterModel> GetRoleMasterByRoleID(long RoleID);

        List<RoleDetailsModel> GetRoleDetailsByRoleID(long RoleID);

        long GetRoleMasterCount(string KeyWord);

        CustomerMasterModel GetUserRolesDetailsByCustomercode(string CustomerCode);

        List<RoleMasterModel> GetRoleMasterForcheckBoxlist(string KeyWord);
    }
}