using System;
using System.Collections.Generic;

namespace CustomerPortalWebApi.Models
{
    public class LedgerBalanceConfirmationHeader
    {
        public long IDbint { get; set; }

        public long RequestIdbint { get; set; }

        public string RequestNovtxt { get; set; }

        public string CustomerCodevtxt { get; set; }

        public string CustomerNamevtxt { get; set; }

        public string Addressvtxt { get; set; }

        public string Cityvtxt { get; set; }

        public string GSTNovtxt { get; set; }

        public string PANNovtxt { get; set; }

        public decimal? CreditLimitdcl { get; set; }

        public decimal? TotalQtydcl { get; set; }

        public decimal? OpeningBalancedcl { get; set; }

        public decimal? Securitydeposit { get; set; }

        public DateTime FromDatedatetime { get; set; }

        public DateTime ToDatedatetime { get; set; }
        public DateTime ExpiryDatedatetime { get; set; }

        public DateTime InsertDatetimedatetime { get; set; }

        public bool BalanceConfirmationStatus { get; set; }

        public string BalanceConfirmationAction { get; set; }

        public string BalanceConfirmationUser { get; set; }

        public DateTime BalanceConfirmationDateTime { get; set; }

        public string Remarksvtxt { get; set; }

        public string AttachmentFileNamevtxt { get; set; }

        public string AttachmentFilevtxt { get; set; }

        public string AttachmentPathvtxt { get; set; }

        public string PendingAt { get; set; }

        public int EscalationLevel { get; set; }

        public string UserCode { get; set; }

        public string UserType { get; set; }

        public List<LedgerBalanceConfirmationAttachments> Attachments { get; set;}
    }

    public class LedgerBalanceConfirmationAttachments
    {
        public long IDbint { get; set; }

        public long AttachID { get; set; }
        public long DetailsIdbint { get; set; }

        public string ActionUserType { get; set; }
        public string ActionUserCode { get; set; }

        public string ActionUserDate { get; set; }


        public string AttachmentFileNamevtxt { get; set; }

        public string AttachmentFilevtxt { get; set; }

        public string AttachmentPathvtxt { get; set; }

    }


    public class LedgerBalanceConfirmationHeader2
    {
        public long IDbint { get; set; }

        public long RequestIdbint { get; set; }

        public string RequestNovtxt { get; set; }

        public string CustomerCodevtxt { get; set; }

        public string CustomerNamevtxt { get; set; }

        public string Addressvtxt { get; set; }

        public string Cityvtxt { get; set; }

        public string GSTNovtxt { get; set; }

        public string PANNovtxt { get; set; }

        public decimal? CreditLimitdcl { get; set; }

        public decimal? TotalQtydcl { get; set; }

        public decimal? OpeningBalancedcl { get; set; }

        public decimal? Securitydeposit { get; set; }

        public DateTime FromDatedatetime { get; set; }

        public DateTime ToDatedatetime { get; set; }
        public DateTime ExpiryDatedatetime { get; set; }

        public DateTime InsertDatetimedatetime { get; set; }

        public bool BalanceConfirmationStatus { get; set; }

        public string BalanceConfirmationAction { get; set; }

        public string BalanceConfirmationUser { get; set; }

        public DateTime BalanceConfirmationDateTime { get; set; }

        public string Remarksvtxt { get; set; }

        public string AttachmentFileNamevtxt { get; set; }

        public string AttachmentFilevtxt { get; set; }

        public string AttachmentPathvtxt { get; set; }

        public string PendingAt { get; set; }

        public int EscalationLevel { get; set; }

        public string UserCode { get; set; }

        public string UserType { get; set; }

        public string RegionCdvtxt { get; set; }
        public string BranchNamevtxt { get; set; }
        public string TerritoryNamevtxt { get; set; }
        public string SPNamevtxt { get; set; }
        public string OrderBlockvtxt { get; set; }
        


        public List<LedgerBalanceConfirmationAttachments> Attachments { get; set; }
    }
}