using System;

namespace CustomerPortalWebApi.Models
{
    public class RefreshToken
    {
        public long TokenIDint { get; set; }
        public string UserIDbint { get; set; }
        public string Tokentxt { get; set; }
        public DateTime ExpiryDatedatetime { get; set; }
        public virtual UserMasterModel UserMasterModel { get; set; }
    }
}