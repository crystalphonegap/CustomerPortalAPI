using System;

namespace CustomerPortalWebApi.Models
{
    public class BalConfirmationModel
    {
        public long IDbint { get; set; }
        public string RequestNovtxt { get; set; }
        public string DealerCodevtxt { get; set; }

        public DateTime FromDatedatetime { get; set; }

        public DateTime ToDatedatetime { get; set; }

        public DateTime ExpiryDatedatetime { get; set; }

        public string Typevtxt { get; set; }
        public string UserTypetxt { get; set; }

        public string Remarksvtxt { get; set; }
        public string Narrationvtxt { get; set; }

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
}