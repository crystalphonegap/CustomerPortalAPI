using System;

namespace CustomerPortalWebApi.Models
{
    public class BalConfirmationModel
    {
        public long IDbint { get; set; }
        public string RequestNovtxt { get; set; }
        public string DealerCodevtxt { get; set; }

        public string CustomerCodevtxt { get; set; }
        public string CustomerNamevtxt { get; set; }

        public string RegionCdvtxt { get; set; }

        public string RegionNamevtxt { get; set; }

        public string TerritoryCodevtxt { get; set; }

        public string TerritoryNamevtxt { get; set; }

        public string Branchvtxt { get; set; }

        public string BranchNamevtxt { get; set; }
        public string PendingAt { get; set; }

        public int EscalationLevel { get; set; }

        public DateTime FromDatedatetime { get; set; }

        public DateTime ToDatedatetime { get; set; }

        public DateTime ExpiryDatedatetime { get; set; }

        public string Typevtxt { get; set; }
        public string UserTypetxt { get; set; }

        public string Remarksvtxt { get; set; }
        public string Narrationvtxt { get; set; }
        
        public decimal? OpeningBalancedcl { get; set; }

        public decimal? Securitydeposit { get; set; }

        public string BalanceConfirmationAction { get; set; }

        public string BalanceConfirmationStatus { get; set; }

        public bool? Flagbit { get; set; }

        public string CreatedByvtxt { get; set; }
        public DateTime CreatedDatetimedatetime { get; set; }
    }

    public class BalConfirmationEditModel
    {
        public long IDbint { get; set; }
        public string ExpiryDatedatetime { get; set; }
        public string CreatedByvtxt { get; set; }
    }

    public class BalConfirmationActionLogModel
    {
        public long RowNum { get; set; }
        public string RequestNovtxt { get; set; }
        public string RequestDate { get; set; }

        public string CustomerCodevtxt { get; set; }
        public string CustNamevtxt { get; set; }

        public string RegionCodevtxt { get; set; }

        public string RegionDescriptionvtxt { get; set; }

        public string SalesOfficeCodevtxt { get; set; }

        public string SalesOfficeNamevtxt { get; set; }

        public string BranchCodevtxt { get; set; }

        public string BranchNamevtxt { get; set; }
        public string PendingAt { get; set; }


        public string BalanceConfirmationAction { get; set; }

        public string BalanceConfirmationStatus { get; set; }

        public string BMComments { get; set; }

        public string RMComments { get; set; }

        public string RMOAccountsComments { get; set; }

        public string CMOComments { get; set; }

        public string CustomerComments { get; set; }



    }
}