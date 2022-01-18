namespace CustomerPortalWebApi.Entities
{
    public class UserAuthenticationObject
    {
        // public string AccessToken { get; set; }
        // public string RefreshsToken { get; set; }

        public long IDbint { get; set; }
        public string UserCodetxt { get; set; }
        public string UserNametxt { get; set; }
        public string UserTypetxt { get; set; }
        public string Divisionvtxt { get; set; }
        public bool IsAuthenticated { get; set; }
        public string BearerToken { get; set; }
        public string Flag { get; set; }

        public string RefreshToken { get; set; }
    }
}