using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerPortalWebApi.Models
{
    public class LedgerBalanceConfirmationLog
    {
        public long IDbint { get; set; }

        public long HeaderIDbint { get; set; }
        public string UserTypevtxt { get; set; }
        public string UserNametxt { get; set; }
        public string CreatedDatetime { get; set; }

        public string UserCodevtxt { get; set; }
            
        public string Remarksvtxt { get; set; }

        public string Statusvtxt { get; set; }
    }
}
