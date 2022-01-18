using System;
using System.Collections.Generic;

namespace CustomerPortalWebApi.Models
{
    public class UserMasterModel
    {
        public long Idbint { get; set; }
        public string UserCodetxt { get; set; }
        public string UserNametxt { get; set; }
        public string ParentCodevtxt { get; set; }
        public string UserTypetxt { get; set; }
        public string Divisionvtxt { get; set; }
        public string Mobilevtxt { get; set; }
        public string Emailvtxt { get; set; }
        public string Passwordvtxt { get; set; }
        public string NewPassword { get; set; }
        public Boolean IsActivebit { get; set; }
        public int? CreatedByint { get; set; }
        public DateTime? CreatedDatedatetime { get; set; }
        public int? ModifyByint { get; set; }
        public DateTime? ModifyDatedatetime { get; set; }

        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}