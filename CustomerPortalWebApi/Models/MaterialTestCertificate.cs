using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerPortalWebApi.Models
{
    public class MaterialTestCertificateModel : MaterialTestCertificateDetailModel
    { 
        public string DocNovtxt { get; set; }
        public string DocDatedatetime { get; set; }
        public string Yeartxt { get; set; }
        public string Gradetxt { get; set; }
        public string Daystxt { get; set; }
        public string Depotvtxt { get; set; }
        public string Weektxt { get; set; }
        public string CreatedBytxt { get; set; }
        public DateTime? CreatedDateTimedatetime { get; set; }
        public int? PageNo { get; set; }
        public int? PageSize { get; set; }
        public string mode { get; set; }
        
    }
    public class MaterialTestCertificateDetailModel
    {
        public long? IDbint { get; set; }
        public long HeaderIdint { get; set; } 
        public string AttachmentFileNamevtxt { get; set; }
        public string AttachmentBytesvtxt { get; set; }

    }
}
