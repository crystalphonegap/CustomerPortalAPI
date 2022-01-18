using Microsoft.AspNetCore.Http;

namespace CustomerPortalWebApi.Entities
{
    public class UploadCustomerEvent
    {
       // public string Name { get; set; }
       // public string CustomerCode { get; set; }
       // public string SiteMobile { get; set; }
       // public string SiteCode{get;set;}
        public string Descripation { get; set; }
        public IFormFile SiteFile { get; set; } 
    }
}