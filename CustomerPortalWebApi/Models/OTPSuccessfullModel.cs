using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerPortalWebApi.Models
{
    public class OTPSuccessfullModel
    {
        public string MOBILE { get; set; }
        public string OTP { get; set; }
        public string MESSAGE { get; set; }
    }
}
