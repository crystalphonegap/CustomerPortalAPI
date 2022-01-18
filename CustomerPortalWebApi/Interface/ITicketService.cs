using CustomerPortalWebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerPortalWebApi.Interface
{
    public interface ITicketService
    {
        string GetOrderNo();

        List<TicketModel> GetReqOrderNo(string ReqOrderNo);

        long InsertDetaial(TicketModel Ticketdetails);

        long Create(TicketModel orderHeaderdetails);

        long update(TicketModel orderHeaderdetails);

        List<TicketModel> GetTicketHeaderList(string fromdate, string todate, string usercode, string usertype, int PageNo, int PageSize, string status, string KeyWord);

        long GetTicketHeaderListCount(string fromdate, string todate, string usercode, string usertype, string status, string KeyWord);

        List<TicketModel> GetTicketDetailList(string TokenNo);

        List<TicketModel> DownloadTicketHeaderList(string fromdate, string todate, string usercode, string usertype, string status, string KeyWord);

        TicketModel GetTicketHeaderDetail(string TokenNo);

        long AddAttachment(int ID, string AttachmentFileNamevtxt, string AttachmentFilePathvtxt, string Type);

        TicketModel GetTicketById(long ID, string Type);

        Task SendEmailForTicketRisedToCustomer(TicketModel model);

        Task SendEmailForTicketRisedToUser(TicketModel model);

        Task SendEmailForActionTicketRisedToCustomer(TicketModel model);

        Task SendEmailForActionTicketRisedToUser(TicketModel model);

        List<TicketModel> MISReportCategoryWise(string usercode, string usertype);

        List<TicketModel> MISReportAssignToWise(string usercode, string usertype);

        List<TicketModel> MISReportCategoryWiseList(string usercode, string usertype, string Priority, string Type, string Category, int From, int To, int PageNo, int PageSize, string KeyWord);

        List<TicketModel> MISReportAssignToWiseList(string usercode, string usertype, string Priority, string Type, string AssignTo, int From, int To, int PageNo, int PageSize, string KeyWord);

        long MISReportCategoryWiseListCount(string usercode, string usertype, string Priority, string Type, string Category, int From, int To, string KeyWord);

        long MISReportAssignToWiseListCount(string usercode, string usertype, string Priority, string Type, string AssignTo, int From, int To, string KeyWord);

        List<TicketModel> MISReportAssignToWiseListDownload(string usercode, string usertype, string Priority, string Type, string AssignTo, int From, int To, string KeyWord);

        List<TicketModel> MISReportCategoryWiseListDownload(string usercode, string usertype, string Priority, string Type, string Category, int From, int To, string KeyWord);
    }
}