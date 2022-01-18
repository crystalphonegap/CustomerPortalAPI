using System;

namespace CustomerPortalWebApi.Models
{
    public class ContentModel
    {
        public long IDbint { get; set; }

        public string Titlevtxt { get; set; }

        public string Typevtxt { get; set; }
        public string Contentvtxt { get; set; }
        public DateTime StartDatedate { get; set; }
        public DateTime EndDatedate { get; set; }

        public bool? Statusbit { get; set; }

        public string CreatedByvtxt { get; set; }
        public DateTime Createddatetimedatetime { get; set; }

        public string ModifyByvtxt { get; set; }
        public DateTime Modifydatetimedatetime { get; set; }
    }
}