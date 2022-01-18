using System;

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
    }
}