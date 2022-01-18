using System;

namespace CustomerPortalWebApi.Models
{
    public class DepartMentMasterModel
    {
        public long Idbint { get; set; }
        public string DepartmentNamevtxt { get; set; }
        public string CreatedByvtxt { get; set; }
        public DateTime? CreatedDateTimedatetime { get; set; }
        public string Modifyvtxt { get; set; }
        public DateTime? ModifyDateTimedatetime { get; set; }
    }
}