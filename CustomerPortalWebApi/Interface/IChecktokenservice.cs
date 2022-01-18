namespace CustomerPortalWebApi.Interface
{
    public interface IChecktokenservice
    {
        bool CheckToken(string Token, string usercode);

        bool CheckTokenForCustomer(string Token, string usercode);

        bool CheckTokenForCustomerByID(string Token, long id);

        bool CheckTokenByID(string Token, long id);

        string GetUserId(string Token);
        string GetUserRole(string Token);
        string GetUserCode(string Token);
        
    }
}