using System;

namespace CustomerPortalWebApi.Models
{
    public class SAPCreateOrderOutputModel
    {
        public long IDbint { get; set; }

        public long OrderIDbint { get; set; }
        public string OrderNovtxt { get; set; }

        public string SAPOrderNovtxt { get; set; }
        public string ErrorIDvtxt { get; set; }
        public string Numvtxt { get; set; }

        public string Messagevtxt { get; set; }
        public DateTime? PostedDatetimedatetime { get; set; }
    }
}