using System;

namespace CustomerPortalWebApi.Models
{
    public class UserRolesHeader
    {
        public long IDbint { get; set; }

        public long RoleIDbint { get; set; }
        public string RoleNamevtxt { get; set; }

        public string UserCodevtxt { get; set; }

        public string CreatedByvtxt { get; set; }

        public DateTime? CreatedDatetimedatetime { get; set; }
    }
}