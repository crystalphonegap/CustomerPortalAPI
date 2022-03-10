namespace CustomerPortalWebApi.Models
{
    public class LoginReportFilterModel
    {
        public string fromDate { get; set; }
        public string todate { get; set; }
        public string UserType { get; set; }
        public string Zone { get; set; }
        public string Region { get; set; }
        public string Branch { get; set; }
        public string Territory { get; set; }
        public string Type { get; set; }
        public string Search { get; set; }
    }
    public class terClass
    {
        public string Type { get; set; }
        public string KeyWord { get; set; }
    }
}