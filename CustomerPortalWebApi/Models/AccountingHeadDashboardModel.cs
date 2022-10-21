using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerPortalWebApi.Models
{
    public class AccountingHeadDashboardModel
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public int NoOfDealers { get; set; }

        public int ActiveDealers { get; set; }

        public int InActiveDealers { get; set; }

        public int BalancePendingCount { get; set; }

        public int BalanceAgreedCount { get; set; }

        public int BalanceDisagreedCount { get; set; }
    }
}
