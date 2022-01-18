using CustomerPortalWebApi.Entities;
using CustomerPortalWebApi.Models;
using System.Collections.Generic;

namespace CustomerPortalWebApi.Interface
{
    public interface IUserMasterService
    {
        long Delete(long id);

        UserMaster GetById(long id);
        UserMaster UserDetailById(long id);

        List<ErrorModel> GetError(string fromdate, string todate, int PageNo, int PageSize, string KeyWord, string Type);

        string Update(UserMaster UserMaster);
        string UpdateStatus(UserMaster UserMaster);

        string EditProfile(UserMaster UserMaster);

        string ChangePassword(UserMaster UserMaster);

        int Create(UserMaster UserMasterDetails);

        long UserCount();
        string EmployeeUpdate(UserMaster UserMaster);
        long DeleteErrorLog(string DelDate);

        List<UserMaster> ListAll();

        List<UserMaster> Search(string keyword);

        List<UserMaster> UserPaging(int pageNo, int pageSize);

        UserMaster Login(string usercode, string password);

        UserMaster LoginLogs(string UserCode, string UserName, string UserType, string BrowserName, string IpAddress);

        int SaveRefreshToken(string username, string refreshToken);

        string GetRefreshToken(string usercode, string reftoken);

        int DeleteRefreshToken(string usercode, string token);

        List<UserMasterModel> GetAllUserMasterForDivisionalAdminSearch(int PageNo, int PageSize, string KeyWord);

        List<UserMasterModel> GetAllUserMasterByParentCode(string status, string CustomerId, int PageNo, int PageSize, string KeyWord);

        long GetAllUserMasterCountByParentCode(string status, string CustomerId, string KeyWord);

        List<UserMasterModel> GetAllUserMasterForDivisionalAdminDownload(string KeyWord);

        long GetAllUserMasterForDivisionalAdminCount(string KeyWord);

        List<UserRolesHeader> GetUserRolesHeader(string usercode);

        int InsertUserRolesHeader(UserRolesHeader userRoles);

        long DeleteUserRoles(string usercode);

        List<UserRolesDetails> GetUserRolesDetails(string usercode);

        List<UserMasterModel> ExportToExcelForParent(string ParentCode, string status, string KeyWord);

        string ResetPassword(UserMaster UserMaster);
    }
}