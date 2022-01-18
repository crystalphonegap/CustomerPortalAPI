using System;

namespace CustomerPortalWebApi.Models
{
    public class CustomerEventFilesModels
    {
        public long Idbint { get; set; }
        public string CustomerCodevtxt { get; set; }
        public string EventCodevtxt { get; set; }
        public string EventTypevtxt { get; set; }
        public string Filevtxt { get; set; }
        public string CreatedByvtxt { get; set; }
       public DateTime CreatedDateTime { get; set; }
        public string ModifyByvtxt { get; set; }
        public DateTime ModifyDatetime { get; set; }
        public bool ISACTIVE { get; set; }

    }
}