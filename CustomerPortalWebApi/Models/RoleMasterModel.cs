using System;

namespace CustomerPortalWebApi.Models
{
    public class RoleMasterModel
    {
        public long IDbint { get; set; }

        public string RoleNamevtxt { get; set; }

        public string RoleDescriptionvtxt { get; set; }

        public string CreatedByvtxt { get; set; }
        public DateTime? CreatedDatedatetime { get; set; }
        public string ModifyByvtxt { get; set; }
        public DateTime? ModifyDatedatetime { get; set; }
    }
}