using CustomerPortalWebApi.Entities;
using CustomerPortalWebApi.Interface;
using CustomerPortalWebApi.Models;
using CustomerPortalWebApi.Security;
using Dapper;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerPortalWebApi.Services
{
    public class TicketService : ITicketService
    {
        private readonly MailSettings _mailSettings;
        private readonly ICustomerPortalHelper _customerPortalHelper;

        public TicketService(IOptions<MailSettings> mailSettings, ICustomerPortalHelper customerPortalHelper)
        {
            _mailSettings = mailSettings.Value;
            _customerPortalHelper = customerPortalHelper;
        }

        public string GetOrderNo()
        {
            var dbPara = new DynamicParameters();
            var data = _customerPortalHelper.GetAll<TicketModel>("[dbo].[uspviewgetTicketNo]", dbPara, commandType: CommandType.StoredProcedure);
         
            if (data != null)
            {
                return data[0].RefNovtxt.ToString();
            }
            else
            {
                return null;
            }
        }

        public List<TicketModel> GetReqOrderNo(string RefNovtxt)
        {
            List<TicketModel> lstOrd = new List<TicketModel>();
            TicketModel ord = new TicketModel();
            ord.RefNovtxt = RefNovtxt;
            ord.RefDatedate = DateTime.Now.Date;
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

        public long Create(TicketModel orderHeaderdetails)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("RefNovtxt", orderHeaderdetails.RefNovtxt, DbType.String);
            dbPara.Add("CustomerCodevtxt", orderHeaderdetails.CustomerCodevtxt, DbType.String);
            dbPara.Add("CustomerNamevtxt", orderHeaderdetails.CustomerNamevtxt, DbType.String);
            dbPara.Add("Priorityvtxt", orderHeaderdetails.Priorityvtxt, DbType.String);
            dbPara.Add("Departmentidint", orderHeaderdetails.Departmentidint, DbType.Int64);
            dbPara.Add("DepartmentNamevtxt", orderHeaderdetails.DepartmentNamevtxt, DbType.String);
            dbPara.Add("Typevtxt", orderHeaderdetails.Typevtxt, DbType.String);
            dbPara.Add("Subjectvtxt", orderHeaderdetails.Subjectvtxt, DbType.String);
            dbPara.Add("Descriptionvtxt", orderHeaderdetails.Descriptionvtxt, DbType.String);
            dbPara.Add("CreatedByvtxt", orderHeaderdetails.CreatedByvtxt, DbType.String);

            #region using dapper

            var data = _customerPortalHelper.Insert<long>("[dbo].[uspInsertCustomerFeedback]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;

            #endregion using dapper
        }

        public long InsertDetaial(TicketModel Ticketdetails)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("HIDbint", Ticketdetails.HIDbint, DbType.Int64);
            dbPara.Add("CustomerCodevtxt", Ticketdetails.CustomerCodevtxt, DbType.String);
            dbPara.Add("UserCodevtxt", Ticketdetails.UserCodevtxt, DbType.String);
            dbPara.Add("UserTypevtxt", Ticketdetails.UserTypevtxt, DbType.String);
            dbPara.Add("Remarksvtxt", Ticketdetails.Remarksvtxt, DbType.String);
            dbPara.Add("Departmentidint", Ticketdetails.Departmentidint, DbType.Int64);
            dbPara.Add("DepartmentNamevtxt", Ticketdetails.DepartmentNamevtxt, DbType.String);
            dbPara.Add("Statusvtxt", Ticketdetails.Statusvtxt, DbType.String);
            dbPara.Add("Priorityvtxt", Ticketdetails.Priorityvtxt, DbType.String);
            dbPara.Add("CreatedByvtxt", Ticketdetails.CreatedByvtxt, DbType.String);

            #region using dapper

            var data = _customerPortalHelper.Insert<long>("[dbo].[uspInsertFeedbackDetail]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;

            #endregion using dapper
        }

        public long update(TicketModel orderHeaderdetails)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("Idbint", orderHeaderdetails.Idbint, DbType.Int64);
            dbPara.Add("RefNovtxt", orderHeaderdetails.RefNovtxt, DbType.String);
            dbPara.Add("CustomerCodevtxt", orderHeaderdetails.CustomerCodevtxt, DbType.String);
            dbPara.Add("CustomerNamevtxt", orderHeaderdetails.CustomerNamevtxt, DbType.Date);
            dbPara.Add("Priorityvtxt", orderHeaderdetails.Priorityvtxt, DbType.String);
            dbPara.Add("Departmentidint", orderHeaderdetails.Departmentidint, DbType.Int64);
            dbPara.Add("DepartmentNamevtxt", orderHeaderdetails.DepartmentNamevtxt, DbType.String);
            dbPara.Add("Typevtxt", orderHeaderdetails.Typevtxt, DbType.String);
            dbPara.Add("Subjectvtxt", orderHeaderdetails.Subjectvtxt, DbType.String);
            dbPara.Add("Descriptionvtxt", orderHeaderdetails.Descriptionvtxt, DbType.String);
            dbPara.Add("Statusvtxt", orderHeaderdetails.Statusvtxt, DbType.String);
            dbPara.Add("CreatedByvtxt", orderHeaderdetails.CreatedByvtxt, DbType.String);

            #region using dapper

            var data = _customerPortalHelper.Update<long>("[dbo].[uspUpdateFeedback]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;

            #endregion using dapper
        }

        public List<TicketModel> GetTicketHeaderList(string fromdate, string todate, string usercode, string usertype, int PageNo, int PageSize, string status, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            DateTime fdate = DateTime.ParseExact(fromdate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(todate, "dd-MM-yyyy", null);
            dbPara.Add("fromDate", Convert.ToDateTime(fdate).ToString("yyyy-MM-dd"), DbType.String);
            dbPara.Add("todate", Convert.ToDateTime(tdate).ToString("yyyy-MM-dd"), DbType.String);

            dbPara.Add("UserCode", usercode, DbType.String);
            dbPara.Add("UserType", usertype, DbType.String);
            dbPara.Add("PageNo", PageNo, DbType.Int32);
            dbPara.Add("PageSize", PageSize, DbType.Int32);
            dbPara.Add("Status", status, DbType.String);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("Keyword", "", DbType.String);
            }
            else
            {
                dbPara.Add("Keyword", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<TicketModel>("[dbo].[uspViewTickets]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public long GetTicketHeaderListCount(string fromdate, string todate, string usercode, string usertype, string status, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            DateTime fdate = DateTime.ParseExact(fromdate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(todate, "dd-MM-yyyy", null);
            dbPara.Add("fromDate", Convert.ToDateTime(fdate).ToString("yyyy-MM-dd"), DbType.String);
            dbPara.Add("todate", Convert.ToDateTime(tdate).ToString("yyyy-MM-dd"), DbType.String);

            dbPara.Add("UserCode", usercode, DbType.String);
            dbPara.Add("UserType", usertype, DbType.String);
            dbPara.Add("Status", status, DbType.String);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("Keyword", "", DbType.String);
            }
            else
            {
                dbPara.Add("Keyword", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<Count>("[dbo].[uspViewTicketsCount]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            if (data != null)
            {
                return data[0].ListCount;
            }
            else
            {
                return 0;
            }
        }

        public List<TicketModel> GetTicketDetailList(string TokenNo)
        {
            var dbPara = new DynamicParameters();

            dbPara.Add("Token", TokenNo, DbType.String);
            dbPara.Add("Mode", "List", DbType.String);
            var data = _customerPortalHelper.GetAll<TicketModel>("[dbo].[uspviewTokenDetailsByTokenNO]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public TicketModel GetTicketHeaderDetail(string TokenNo)
        {
            var dbPara = new DynamicParameters();

            dbPara.Add("Token", TokenNo, DbType.String);
            dbPara.Add("Mode", "Details", DbType.String);
            var data = _customerPortalHelper.GetAll<TicketModel>("[dbo].[uspviewTokenDetailsByTokenNO]",
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

        public TicketModel GetTicketById(long ID, string Type)
        {
            var dbPara = new DynamicParameters();

            dbPara.Add("ID", ID, DbType.Int64);
            dbPara.Add("Mode", Type, DbType.String);
            var data = _customerPortalHelper.GetAll<TicketModel>("[dbo].[uspviewAttachmentDetailsByTokenNO]",
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

        public long AddAttachment(int ID, string AttachmentFileNamevtxt, string AttachmentFilePathvtxt, string Type)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("ID", ID, DbType.Int64);
            dbPara.Add("AttachmentFileNamevtxt", AttachmentFileNamevtxt, DbType.String);
            dbPara.Add("AttachmentFilePathvtxt", AttachmentFilePathvtxt, DbType.String);
            dbPara.Add("Type", Type, DbType.String);

            #region using dapper

            var data = _customerPortalHelper.Insert<long>("[dbo].[USP_addedAttachmentForTicket]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;

            #endregion using dapper
        }

        public List<TicketModel> DownloadTicketHeaderList(string fromdate, string todate, string usercode, string usertype, string status, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            DateTime fdate = DateTime.ParseExact(fromdate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(todate, "dd-MM-yyyy", null);
            dbPara.Add("fromDate", Convert.ToDateTime(fdate).ToString("yyyy-MM-dd"), DbType.String);
            dbPara.Add("todate", Convert.ToDateTime(tdate).ToString("yyyy-MM-dd"), DbType.String);

            dbPara.Add("UserCode", usercode, DbType.String);
            dbPara.Add("UserType", usertype, DbType.String);
            dbPara.Add("Status", status, DbType.String);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("Keyword", "", DbType.String);
            }
            else
            {
                dbPara.Add("Keyword", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<TicketModel>("[dbo].[uspDownloadTickets]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public async Task SendEmailForTicketRisedToCustomer(TicketModel model)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            if (model.CustomerEmail != "")
            {
                email.To.Add(MailboxAddress.Parse(model.CustomerEmail));
                email.Subject = "Ticket has been successfully submitted to Our Team";
                var builder = new BodyBuilder();
                builder.HtmlBody = GetBodyForTicketRisedForCustomer(model.CustomerCodevtxt + '-' + model.CustomerNamevtxt, model.RefNovtxt, model.RefDatedate.ToString(), model.DepartmentNamevtxt, model.Subjectvtxt, model.Descriptionvtxt, model.Statusvtxt, model.Priorityvtxt);
                email.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
            }
        }

        public async Task SendEmailForTicketRisedToUser(TicketModel model)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            if (model.EmployeeEmail != "")
            {
                email.To.Add(MailboxAddress.Parse(model.EmployeeEmail));
                email.Subject = "Ticket has been successfully submitted to Our Team";
                var builder = new BodyBuilder();
                builder.HtmlBody = GetBodyForTicketRisedForUser(model.UserCodevtxt + '-' + model.UserNametxt, model.CustomerCodevtxt + '-' + model.CustomerNamevtxt, model.RefNovtxt, model.RefDatedate.ToString(), model.DepartmentNamevtxt, model.Subjectvtxt, model.Descriptionvtxt, model.Statusvtxt, model.Priorityvtxt);
                email.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
            }
        }

        public string GetBodyForTicketRisedForCustomer(string customername, string refno, string refdate, string category, string subject, string descption, string status, string priority)
        {
            StringBuilder emailBody = new StringBuilder(String.Empty);
            emailBody.Append("Dear " + customername + ",");
            emailBody.Append("<br/>Ticket Genarated No : " + refno + ", Priority : " + priority + ", Status : " + status);
            emailBody.Append("<br/>");
            emailBody.Append("<br/>Find the Following details of Ticket is");
            // emailBody.Append("<br/>User Code: " + customerCode);
            emailBody.Append("<br/>Ticket Date : " + refdate);
            emailBody.Append("<br/>Category    : " + category);
            emailBody.Append("<br/>Subject     : " + subject);
            emailBody.Append("<br/>Description : " + descption);
            emailBody.Append("<br/>");
            emailBody.Append("<br/>Ticket has been Successfully submitted to Our Team");
            emailBody.Append("<br/>");
            emailBody.Append("<br/>Regards,");
            emailBody.Append("<br/>" + "Help Desk Team");
            emailBody.Append("<br/>==========================================================================================<br/>");
            emailBody.Append("<br/>" + "LEGAL DISCLAIMER:");
            emailBody.Append("<br/>" + "This e-mail is an electronic communication / message along - with attachment / s, if any, either in part or whole.It is intended solely for");
            emailBody.Append("<br/>" + "the addressee or addressees and is");
            emailBody.Append("<br/>" + "confidential, proprietary or legally privileged information, which may exempt from disclosure.It should not be");
            emailBody.Append("<br/>" + "used by anyone who is not the original intended recipient.Please accordingly also note that any views or opinions");
            emailBody.Append("<br/>" + "presented in this E - Mail is solely those of the author and may not represent those of the company or bind the company.");
            emailBody.Append("<br/>==========================================================================================<br/>");
            return emailBody.ToString();
        }

        public string GetBodyForTicketRisedForUser(string username, string customername, string refno, string refdate, string category, string subject, string descption, string status, string priority)
        {
            StringBuilder emailBody = new StringBuilder(String.Empty);
            emailBody.Append("Dear " + username + ",");
            emailBody.Append("<br/>Ticket Genarated No : " + refno + ", Priority : " + priority + ", Status : " + status);
            emailBody.Append("<br/>");
            emailBody.Append("<br/>Find the Following details of Ticket is");
            // emailBody.Append("<br/>User Code: " + customerCode);
            emailBody.Append("<br/>Customer Code & Name   : " + customername);
            emailBody.Append("<br/>Ticket Date : " + refdate);
            emailBody.Append("<br/>Category    : " + category);
            emailBody.Append("<br/>Subject     : " + subject);
            emailBody.Append("<br/>Description : " + descption);
            emailBody.Append("<br/>");
            emailBody.Append("<br/>Ticket has been raised for customer " + customername);
            emailBody.Append("<br/>");
            emailBody.Append("<br/>Regards,");
            emailBody.Append("<br/>" + "Help Desk Team");
            emailBody.Append("<br/>==========================================================================================<br/>");
            emailBody.Append("<br/>" + "LEGAL DISCLAIMER:");
            emailBody.Append("<br/>" + "This e-mail is an electronic communication / message along - with attachment / s, if any, either in part or whole.It is intended solely for");
            emailBody.Append("<br/>" + "the addressee or addressees and is");
            emailBody.Append("<br/>" + "confidential, proprietary or legally privileged information, which may exempt from disclosure.It should not be");
            emailBody.Append("<br/>" + "used by anyone who is not the original intended recipient.Please accordingly also note that any views or opinions");
            emailBody.Append("<br/>" + "presented in this E - Mail is solely those of the author and may not represent those of the company or bind the company.");
            emailBody.Append("<br/>==========================================================================================<br/>");
            return emailBody.ToString();
        }

        public async Task SendEmailForActionTicketRisedToCustomer(TicketModel model)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            if (model.CustomerEmail != "")
            {
                email.To.Add(MailboxAddress.Parse(model.CustomerEmail));
                email.Subject = model.RefNovtxt + " is on process.";
                var builder = new BodyBuilder();
                builder.HtmlBody = GetBodyForActionTicketRisedForCustomer(model.CustomerCodevtxt + '-' + model.CustomerNamevtxt, model.RefNovtxt, model.RefDatedate.ToString(), model.DepartmentNamevtxt, model.Subjectvtxt, model.Descriptionvtxt, model.Statusvtxt, model.Priorityvtxt, model.DetailDate, model.Remarksvtxt, model.DetailCreatedBy);
                email.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
            }
        }

        public async Task SendEmailForActionTicketRisedToUser(TicketModel model)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            if (model.EmployeeEmail != "")
            {
                email.To.Add(MailboxAddress.Parse(model.EmployeeEmail));
                email.Subject = "Ticket has been successfully submitted to Our Team";
                var builder = new BodyBuilder();
                builder.HtmlBody = GetBodyForTicketRisedForUser(model.UserCodevtxt + '-' + model.UserNametxt, model.CustomerCodevtxt + '-' + model.CustomerNamevtxt, model.RefNovtxt, model.RefDatedate.ToString(), model.DepartmentNamevtxt, model.Subjectvtxt, model.Descriptionvtxt, model.Statusvtxt, model.Priorityvtxt);
                email.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
            }
        }

        public string GetBodyForActionTicketRisedForCustomer(string customername, string refno, string refdate, string category, string subject, string descption, string status, string priority, string date, string Remarks, string createdby)
        {
            StringBuilder emailBody = new StringBuilder(String.Empty);
            emailBody.Append("Dear " + customername + ",");
            emailBody.Append("<br/>Updated Ticket for Ticket No : " + refno + ", Priority : " + priority + ", Status : " + status);
            emailBody.Append("<br/>Find the Following details of Ticket is");
            // emailBody.Append("<br/>User Code: " + customerCode);
            emailBody.Append("<br/>Ticket Date : " + refdate);
            emailBody.Append("<br/>Category    : " + category);
            emailBody.Append("<br/>Subject     : " + subject);
            emailBody.Append("<br/>Description : " + descption);
            emailBody.Append("<br/>Remarks     : " + Remarks);
            emailBody.Append("<br/>ActionDate  : " + date);
            emailBody.Append("<br/>ActionBy    : " + createdby);
            emailBody.Append("<br/>");
            emailBody.Append("<br/>Ticket has been updated by Our Team");
            emailBody.Append("<br/>");
            emailBody.Append("<br/>Regards,");
            emailBody.Append("<br/>" + "Help Desk Team");
            emailBody.Append("<br/>==========================================================================================<br/>");
            emailBody.Append("<br/>" + "LEGAL DISCLAIMER:");
            emailBody.Append("<br/>" + "This e-mail is an electronic communication / message along - with attachment / s, if any, either in part or whole.It is intended solely for");
            emailBody.Append("<br/>" + "the addressee or addressees and is");
            emailBody.Append("<br/>" + "confidential, proprietary or legally privileged information, which may exempt from disclosure.It should not be");
            emailBody.Append("<br/>" + "used by anyone who is not the original intended recipient.Please accordingly also note that any views or opinions");
            emailBody.Append("<br/>" + "presented in this E - Mail is solely those of the author and may not represent those of the company or bind the company.");
            emailBody.Append("<br/>==========================================================================================<br/>");
            return emailBody.ToString();
        }

        public string GetBodyForActionTicketRisedForUser(string username, string customername, string refno, string refdate, string category, string subject, string descption, string status, string priority, string date, string Remarks, string createdby)
        {
            StringBuilder emailBody = new StringBuilder(String.Empty);
            emailBody.Append("Dear " + username + ",");
            emailBody.Append("<br/>Updated Ticket for Ticket No : " + refno + ", Priority : " + priority + ", Status : " + status);
            emailBody.Append("<br/>");
            emailBody.Append("<br/>Find the Following details of Ticket is");
            // emailBody.Append("<br/>User Code: " + customerCode);
            emailBody.Append("<br/>Customer Code & Name   : " + customername + "<br/>");
            emailBody.Append("<br/>Ticket Date : " + refdate);
            emailBody.Append("<br/>Category    : " + category);
            emailBody.Append("<br/>Subject     : " + subject);
            emailBody.Append("<br/>Description : " + descption);
            emailBody.Append("<br/>Remarks     : " + Remarks);
            emailBody.Append("<br/>ActionDate  : " + date);
            emailBody.Append("<br/>ActionBy    : " + createdby);
            emailBody.Append("<br/>");
            emailBody.Append("<br/>Ticket has been updated for customer " + customername);
            emailBody.Append("<br/>");
            emailBody.Append("<br/>Regards,");
            emailBody.Append("<br/>" + "Help Desk Team");
            emailBody.Append("<br/>==========================================================================================<br/>");
            emailBody.Append("<br/>" + "LEGAL DISCLAIMER:");
            emailBody.Append("<br/>" + "This e-mail is an electronic communication / message along - with attachment / s, if any, either in part or whole.It is intended solely for");
            emailBody.Append("<br/>" + "the addressee or addressees and is");
            emailBody.Append("<br/>" + "confidential, proprietary or legally privileged information, which may exempt from disclosure.It should not be");
            emailBody.Append("<br/>" + "used by anyone who is not the original intended recipient.Please accordingly also note that any views or opinions");
            emailBody.Append("<br/>" + "presented in this E - Mail is solely those of the author and may not represent those of the company or bind the company.");
            emailBody.Append("<br/>==========================================================================================<br/>");
            return emailBody.ToString();
        }

        public List<TicketModel> MISReportCategoryWise(string usercode, string usertype)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("UserCode", usercode, DbType.String);
            dbPara.Add("UserType", usertype, DbType.String);
            var data = _customerPortalHelper.GetAll<TicketModel>("[dbo].[uspviewTicketMISForCategoryWise]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public List<TicketModel> MISReportAssignToWise(string usercode, string usertype)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("UserCode", usercode, DbType.String);
            dbPara.Add("UserType", usertype, DbType.String);

            var data = _customerPortalHelper.GetAll<TicketModel>("[dbo].[uspviewTicketMISForAssignToWise]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public List<TicketModel> MISReportCategoryWiseList(string usercode, string usertype, string Priority, string Type, string Category, int From, int To, int PageNo, int PageSize, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("UserCode", usercode, DbType.String);
            dbPara.Add("UserType", usertype, DbType.String);
            dbPara.Add("Priority", Priority, DbType.String);
            dbPara.Add("Type", Type, DbType.String);
            dbPara.Add("Category", Category, DbType.String);
            dbPara.Add("From", From, DbType.Int32);
            dbPara.Add("To", To, DbType.Int32);
            dbPara.Add("PageNo", PageNo, DbType.Int32);
            dbPara.Add("PageSize", PageSize, DbType.Int32);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("Keyword", "", DbType.String);
            }
            else
            {
                dbPara.Add("Keyword", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<TicketModel>("[dbo].[uspviewTicketMISForCategoryWiseList]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public List<TicketModel> MISReportAssignToWiseList(string usercode, string usertype, string Priority, string Type, string AssignTo, int From, int To, int PageNo, int PageSize, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("UserCode", usercode, DbType.String);
            dbPara.Add("UserType", usertype, DbType.String);
            dbPara.Add("Priority", Priority, DbType.String);
            dbPara.Add("Type", Type, DbType.String);
            dbPara.Add("AssignTo", AssignTo, DbType.String);
            dbPara.Add("From", From, DbType.Int32);
            dbPara.Add("To", To, DbType.Int32);
            dbPara.Add("PageNo", PageNo, DbType.Int32);
            dbPara.Add("PageSize", PageSize, DbType.Int32);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("Keyword", "", DbType.String);
            }
            else
            {
                dbPara.Add("Keyword", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<TicketModel>("[dbo].[uspviewTicketMISForAssignToWiseList]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public long MISReportCategoryWiseListCount(string usercode, string usertype, string Priority, string Type, string Category, int From, int To, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("UserCode", usercode, DbType.String);
            dbPara.Add("UserType", usertype, DbType.String);
            dbPara.Add("Priority", Priority, DbType.String);
            dbPara.Add("Type", Type, DbType.String);
            dbPara.Add("Category", Category, DbType.String);
            dbPara.Add("From", From, DbType.Int32);
            dbPara.Add("To", To, DbType.Int32);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("Keyword", "", DbType.String);
            }
            else
            {
                dbPara.Add("Keyword", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<TicketModel>("[dbo].[uspviewTicketMISForCategoryWiseListCount]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
           
            if (data != null)
            {
                return data.Count;
            }
            else
            {
                return 0;
            }
        }

        public List<TicketModel> MISReportCategoryWiseListDownload(string usercode, string usertype, string Priority, string Type, string Category, int From, int To, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("UserCode", usercode, DbType.String);
            dbPara.Add("UserType", usertype, DbType.String);
            dbPara.Add("Priority", Priority, DbType.String);
            dbPara.Add("Type", Type, DbType.String);
            dbPara.Add("Category", Category, DbType.String);
            dbPara.Add("From", From, DbType.Int32);
            dbPara.Add("To", To, DbType.Int32);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("Keyword", "", DbType.String);
            }
            else
            {
                dbPara.Add("Keyword", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<TicketModel>("[dbo].[uspviewTicketMISForCategoryWiseListCount]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public long MISReportAssignToWiseListCount(string usercode, string usertype, string Priority, string Type, string AssignTo, int From, int To, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("UserCode", usercode, DbType.String);
            dbPara.Add("UserType", usertype, DbType.String);
            dbPara.Add("Priority", Priority, DbType.String);
            dbPara.Add("Type", Type, DbType.String);
            dbPara.Add("AssignTo", AssignTo, DbType.String);
            dbPara.Add("From", From, DbType.Int32);
            dbPara.Add("To", To, DbType.Int32);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("Keyword", "", DbType.String);
            }
            else
            {
                dbPara.Add("Keyword", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<TicketModel>("[dbo].[uspviewTicketMISForAssignToWiseListCount]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            if (data != null)
            {
                return data.Count;
            }
            else
            {
                return 0;
            }
        }
        public List<TicketModel> MISReportAssignToWiseListDownload(string usercode, string usertype, string Priority, string Type, string AssignTo, int From, int To, string KeyWord)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("UserCode", usercode, DbType.String);
            dbPara.Add("UserType", usertype, DbType.String);
            dbPara.Add("Priority", Priority, DbType.String);
            dbPara.Add("Type", Type, DbType.String);
            dbPara.Add("AssignTo", AssignTo, DbType.String);
            dbPara.Add("From", From, DbType.Int32);
            dbPara.Add("To", To, DbType.Int32);
            if (KeyWord == "NoSearch")
            {
                dbPara.Add("Keyword", "", DbType.String);
            }
            else
            {
                dbPara.Add("Keyword", KeyWord, DbType.String);
            }
            var data = _customerPortalHelper.GetAll<TicketModel>("[dbo].[uspviewTicketMISForAssignToWiseListCount]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }
    }
}