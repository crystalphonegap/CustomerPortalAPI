using System;

namespace CustomerPortalWebApi.Models
{
    public class UploadEmployeeModel
    {
        public long Idbint { get; set; }
        public string UserCodetxt { get; set; }
        public string UserNametxt { get; set; }
        public string UserTypetxt { get; set; }
        public string Divisionvtxt { get; set; }
        public string Mobilevtxt { get; set; }
        public string Emailvtxt { get; set; }
        public string Passwordvtxt { get; set; }

        public string ParentCode { get; set; }
        public int? CreatedByint { get; set; }
        public DateTime? CreatedDatedatetime { get; set; }

        public string Remarks { get; set; }
    }
}