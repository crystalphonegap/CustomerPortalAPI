using CustomerPortalWebApi.Entities;
using CustomerPortalWebApi.Interface;
using CustomerPortalWebApi.Models;
using CustomerPortalWebApi.Security;
using Dapper;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CustomerPortalWebApi.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;

        private readonly IConfiguration _config;
        private readonly ICustomerPortalHelper _customerPortalHelper;

        public MailService(IOptions<MailSettings> mailSettings, ICustomerPortalHelper customerPortalHelper, IConfiguration config)
        {
            _mailSettings = mailSettings.Value;
            _customerPortalHelper = customerPortalHelper;
            _config = config;
        }

        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            //if (mailRequest.Attachments != null)
            //{
            //    byte[] fileBytes;
            //    foreach (var file in mailRequest.Attachments)
            //    {
            //        if (file.Length > 0)
            //        {
            //            using (var ms = new MemoryStream())
            //            {
            //                file.CopyTo(ms);
            //                fileBytes = ms.ToArray();
            //            }
            //            builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
            //        }
            //    }
            //}
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

        public List<CustomerMaster> GetCustomerDetails(string CustomerCode, string division)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("customerCode", CustomerCode, DbType.String);
            dbPara.Add("Division", division, DbType.String);
            var data = _customerPortalHelper.GetAll<CustomerMaster>("[dbo].[uspviewSendEmailToCustomer]", dbPara, commandType: CommandType.StoredProcedure);

            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public async Task SendEmailToCustomer(string CustomerCode, string division)
        {
            List<CustomerMaster> lstcustomer = new List<CustomerMaster>();
            lstcustomer = GetCustomerDetails(CustomerCode, division);
            if (lstcustomer.Count > 0)
            {
                try
                {
                    var email = new MimeMessage();
                    email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
                    if(lstcustomer[0].Emailvtxt!="" || lstcustomer[0].Emailvtxt != null)
                    {
                        if (string.IsNullOrEmpty(lstcustomer[0].Emailvtxt.Trim()))
                        {
                            email.To.Add(MailboxAddress.Parse(lstcustomer[0].Emailvtxt));
                            email.Subject = "Registration For Customer Portal  of Prism Johnson Company";
                            var builder = new BodyBuilder();
                            builder.HtmlBody = GetBody(lstcustomer[0].CustCodevtxt, lstcustomer[0].CustCodevtxt + '-' + lstcustomer[0].CustNamevtxt, lstcustomer[0].AccessTokenKeyvtxt);
                            email.Body = builder.ToMessageBody();
                            using var smtp = new SmtpClient();
                            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                            await smtp.SendAsync(email);
                            smtp.Disconnect(true);
                        }
                    }
                    else
                    {

                    }
                   
                }
                catch (Exception)
                {

                }

            }
        }

        public async Task SendEmailToCustomerBalanceConfirmation(string CustomerCode, string fromdate, string todate)
        {
            List<CustomerMaster> lstcustomer = new List<CustomerMaster>();
            lstcustomer = GetCustomerDetails(CustomerCode, "Cement");
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            if (lstcustomer[0].Emailvtxt != "")
            {
                email.To.Add(MailboxAddress.Parse(lstcustomer[0].Emailvtxt));
                email.Subject = "Balance Confirmation for Period of " + fromdate + " to " + todate + " is Submitted";
                var builder = new BodyBuilder();
                builder.HtmlBody = GetBodyForBalanceConfirmaation(lstcustomer[0].CustCodevtxt + '-' + lstcustomer[0].CustNamevtxt, fromdate, todate);
                email.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
            }
        }

        public async Task SendEmailToAccountingHeadBalanceConfirmation(string UserName, string usercode, string emailid, string fromdate, string todate, string requsetno, string customercode)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            if (emailid != "")
            {
                email.To.Add(MailboxAddress.Parse(emailid));
                email.Subject = "Balance Confirmation of customer " + customercode + " for Period of " + fromdate + " to " + todate + " is Submitted";
                var builder = new BodyBuilder();
                builder.HtmlBody = GetBodyForBalanceConfirmaationForAH(usercode + '-' + UserName, fromdate, todate, customercode, requsetno);
                email.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
            }
        }

        public string GetBodyForBalanceConfirmaation(string name, string fromdate, string todate)
        {
            StringBuilder emailBody = new StringBuilder(String.Empty);
            emailBody.Append("Dear " + name + ",");
            emailBody.Append("<br/><br/>");
            emailBody.Append("<br/>Balance Confirmation for Period of " + fromdate + " to " + todate + " has been submitted our Accounts Team.. <br/>");
            emailBody.Append("<br/> Thank You for submitting response we will get back you if any issue");
            emailBody.Append("<br/><br/>");
            emailBody.Append("<br/>Regards,");
            emailBody.Append("<br/>" + "System Administrator");
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

        public string GetBody(string customerCode, string name, string AccessTokenKeyvtxt)
        {
            StringBuilder emailBody = new StringBuilder(String.Empty);
            emailBody.Append("Dear " + name + ",");
            emailBody.Append("<br/><br/>");
            emailBody.Append("<br/>Please login with following URL to Register into the Customer Portal of Prism Johnson Limited<br/>");
            emailBody.Append("<br/>URL: https://bandhan.prismcement.com/user/Verification");
            emailBody.Append("<br/>User Code: " + customerCode);
            emailBody.Append("<br/>Access Token: " + AccessTokenKeyvtxt + "<br/>");
            emailBody.Append("<br/><br/>");
            emailBody.Append("<br/>Regards,");
            emailBody.Append("<br/>" + "System Administrator");
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

        public string GetBodyForBalanceConfirmaationForAH(string name, string fromdate, string todate, string customercode, string Requstno)
        {
            StringBuilder emailBody = new StringBuilder(String.Empty);
            emailBody.Append("Dear " + name + ",");
            emailBody.Append("<br/><br/>");
            emailBody.Append("<br/>Balance Confirmation of Customer " + customercode + " for Period of " + fromdate + " to " + todate + " has been submitted and Request No is " + Requstno + ".. <br/>");
            emailBody.Append("<br/><br/>");
            emailBody.Append("<br/><br/>");
            emailBody.Append("<br/>Regards,");
            emailBody.Append("<br/>" + "System Administrator");
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

        public string GetBodyForResetPassword(string customerCode, string name, string AccessTokenKeyvtxt)
        {
            StringBuilder emailBody = new StringBuilder(String.Empty);
            emailBody.Append("Dear " + name + ",");
            emailBody.Append("<br/><br/>");
            emailBody.Append("<br/>Please go to following url and reset your password into the Customer Portal of Prism Johnson Limited<br/>");
            emailBody.Append("<br/>URL: https://bandhan.prismcement.com/user/PasswordReset");
            // emailBody.Append("<br/>User Code: " + customerCode);
            emailBody.Append("<br/>OTP: " + AccessTokenKeyvtxt + "<br/>");
            emailBody.Append("<br/><br/>");
            emailBody.Append("<br/>Regards,");
            emailBody.Append("<br/>" + "System Administrator");
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

        public async Task<string> SendEmailForForgetPassword(string custcode, string custname, string EmailID, string ResetTokenvtxt, string mobileno)
        {
            var returndata = "";
            Boolean SMS = false;
            Boolean Mail = false;
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            if (!string.IsNullOrEmpty(mobileno))
            {
                var client = new HttpClient();
                var Message = string.Format("Dear User, Please use OTP {0} to reset your password of Bandhan Dealer Portal. This OTP is valid for 15 min", ResetTokenvtxt);
                var Url = _config["SMS:URL"];
                var urlstring = string.Format(Url, mobileno, Message);
                client.BaseAddress = (new Uri(urlstring));
                client.DefaultRequestHeaders.Add("ContentType", "application/json");
                HttpResponseMessage responsePost = await client.GetAsync(client.BaseAddress.ToString());
                //{http://bhashsms.com/api/sendmsg.php?user=prismcement&pass=Pcl@2017&sender=PRISMD&phone=8450929346&text=Dear User, Please use OTP RS651525 to reset your password of Bandhan Dealer Portal. This OTP is valid for 15 min&priority=ndnd&stype=normal}
                if (responsePost.IsSuccessStatusCode)
                {
                    SMS = true;
                }
            }
            if (!string.IsNullOrEmpty(EmailID))
            {
                email.To.Add(MailboxAddress.Parse(EmailID));
                email.Subject = "Reset Password For Customer Portal  of Prism Johnson Company";
                var builder = new BodyBuilder();
                builder.HtmlBody = GetBodyForResetPassword(custcode, custcode + '-' + custname, ResetTokenvtxt);
                email.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
                Mail = true;
            }
            if (SMS == true && Mail == true)
            {
                returndata = "SMS & Mail Send";
            }
            else if (SMS == false && Mail == true)
            {
                returndata = "Mail Send";
            }
            else if (SMS == true && Mail == false)
            {
                returndata = "SMS Send";
            }

            return returndata;
        }

        public UserMaster GetuserDetails(string UserCode)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("UserCode", UserCode, DbType.String);
            var data = _customerPortalHelper.GetAll<UserMaster>("[dbo].[uspcheckuserByUserCode]", dbPara, commandType: CommandType.StoredProcedure);
            if (data.Count != 0)
            {
                return data[0];
            }
            else
            {
                var UserMaster = new UserMaster()
                {
                    Idbint = -1
                };
                return UserMaster;
            }
        }

        public async Task<OTPSuccessfullModel> GetOTPFORLOGIN(string UserCode)
        {
            var returndata = "";
            Boolean SMS = false;

            OTPSuccessfullModel OTPSuccessfullModel = new OTPSuccessfullModel();

            //if (!string.IsNullOrEmpty(mobileno))
            //{
            Random rd = new Random();

            int num = rd.Next(10000, 99999);
            var dbPara = new DynamicParameters();
            dbPara.Add("@P_MobileNumber", "", DbType.String);
            dbPara.Add("@P_OTP", num, DbType.String);
            dbPara.Add("@P_TYPE", "INSERT", DbType.String);
            dbPara.Add("@P_USERCODE", UserCode, DbType.String);

            var data = _customerPortalHelper.GetAll<OTPSuccessfullModel>("[dbo].[PRC_OTP_LOGIN]", dbPara, commandType: CommandType.StoredProcedure);

            var client = new HttpClient();
            var Message = string.Format("Dear User, Please use OTP {0} to Login on Bandhan Dealer Portal. This OTP is valid for 15 min", num);// Dear User, Please use OTP {0} to Login on Bandhan Dealer Portal. This OTP is valid for 15 min
            var Url = _config["SMS:URL"];
            var urlstring = string.Format(Url, data[0].MOBILE, Message);
            client.BaseAddress = (new Uri(urlstring));
            client.DefaultRequestHeaders.Add("ContentType", "application/json");
            HttpResponseMessage responsePost = await client.GetAsync(client.BaseAddress.ToString());
            //{http://bhashsms.com/api/sendmsg.php?user=prismcement&pass=Pcl@2017&sender=PRISMD&phone=8450929346&text=Dear User, Please use OTP RS651525 to reset your password of Bandhan Dealer Portal. This OTP is valid for 15 min&priority=ndnd&stype=normal}
            if (responsePost.IsSuccessStatusCode)
            {
                SMS = true;
            }

            OTPSuccessfullModel.MOBILE = data[0].MOBILE;
            OTPSuccessfullModel.OTP = data[0].OTP;
            OTPSuccessfullModel.MESSAGE = data[0].MESSAGE;

            return OTPSuccessfullModel;

        }

        public UserMaster LoginWithOTP(string mobileno, string OTP, string UseCode)
        {

            UserMaster UserMaster = new UserMaster();
            var dbPara = new DynamicParameters();
            dbPara.Add("@P_MobileNumber", mobileno, DbType.String);
            dbPara.Add("@P_OTP", OTP.Trim(), DbType.String);
            dbPara.Add("@P_TYPE", "CHECKED", DbType.String);
            dbPara.Add("@P_USERCODE", UseCode, DbType.String);

            var data = _customerPortalHelper.GetAll<UserMaster>("[dbo].[PRC_OTP_LOGIN]", dbPara, commandType: CommandType.StoredProcedure);
            UserMaster.Branch = data[0].Branch;
            UserMaster.BranchCodevtxt = data[0].BranchCodevtxt;
            UserMaster.Passwordvtxt = Decrypttxt(data[0].Passwordvtxt);
            UserMaster.Idbint = data[0].Idbint;
            UserMaster.UserNametxt = data[0].UserNametxt;
            UserMaster.UserTypetxt = data[0].UserTypetxt;
            UserMaster.Divisionvtxt = data[0].Divisionvtxt;
            UserMaster.UserCodetxt = data[0].UserCodetxt;

            return UserMaster;
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
    }
}