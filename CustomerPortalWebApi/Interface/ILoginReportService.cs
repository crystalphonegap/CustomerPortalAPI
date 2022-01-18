using CustomerPortalWebApi.Entities;
using CustomerPortalWebApi.Models;
using System.Collections.Generic;
using System.Data;

namespace CustomerPortalWebApi.Interface
{
    public interface ILoginReportService
    {
        long LoginReportCount(string FromDate, string Todate, string type, string Keyword);

        List<UserMaster> LoginReportDownload(string FromDate, string Todate, string Keyword);

        List<UserMaster> LoginReport(string FromDate, string Todate, int pageNo, int pageSize, string Keyword);

        DataTable NewLoginReport(LoginReportFilterModel model);

        DataTable NewLoginReportDownload(LoginReportFilterModel model);

        List<SalesHierachy> GetArea(string Type, string Keyword);
    }
}