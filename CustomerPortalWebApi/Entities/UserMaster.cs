using CustomerPortalWebApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CustomerPortalWebApi.Entities
{
    public class UserMaster
    {
        [Key]
        public long Idbint { get; set; }

        public string UserCodetxt { get; set; }
        public string ParentCodevtxt { get; set; }
        public string UserNametxt { get; set; }
        public string UserTypetxt { get; set; }
        public string Divisionvtxt { get; set; }
        public string Mobilevtxt { get; set; }
        public string Emailvtxt { get; set; }
        public Boolean IsActivebit { get; set; }
        public string Flag { get; set; }
        public string Passwordvtxt { get; set; }
        public string NewPassword { get; set; }
        public int? CreatedByint { get; set; }
        public DateTime? CreatedDatedatetime { get; set; }
        public int? ModifyByint { get; set; }
        public DateTime? ModifyDatedatetime { get; set; }

        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
        public string BrowserName { get; set; }
        public string IpAddress { get; set; }
        public string Region { get; set; }
        public string Branch { get; set; }
        public string Territory { get; set; }
        public string ResetTokenvtxt { get; set; }
        public string SalesOfficeCodevtxt { get; set; }
        public string SalesOfficeNamevtxt { get; set; }
        public string BranchCodevtxt { get; set; }
        public string BranchNamevtxt { get; set; }
        public string RegionCodevtxt { get; set; }
        public string RegionDescriptionvtxt { get; set; }
        public string ZoneCodevtxt { get; set; }
        public string ZoneDescriptionvtxt { get; set; }
    }
}