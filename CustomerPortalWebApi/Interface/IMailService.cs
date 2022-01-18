using CustomerPortalWebApi.Entities;
using CustomerPortalWebApi.Models;
using System.Threading.Tasks;

namespace CustomerPortalWebApi.Interface
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);

        Task SendEmailToCustomer(string CustomerCode, string division);

        Task<string> SendEmailForForgetPassword(string custcode, string custname, string EmailID, string ResetTokenvtxt, string mobileno);
        Task<OTPSuccessfullModel> GetOTPFORLOGIN(string UserCode);

        UserMaster LoginWithOTP(string mobileno,string OTP,string UserCode);
    
        Task SendEmailToCustomerBalanceConfirmation(string CustomerCode, string fromdate, string todate);

        Task SendEmailToAccountingHeadBalanceConfirmation(string UserName, string usercode, string emailid, string fromdate, string todate, string requsetno, string customercode);

        UserMaster GetuserDetails(string UserCode);
    }
}