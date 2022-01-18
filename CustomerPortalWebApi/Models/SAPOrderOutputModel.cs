using SapNwRfc;

namespace CustomerPortalWebApi.Models
{
    public class SAPOrderOutputModel
    {
        [SapName("E_ORD_NUM")]
        public string E_ORD_NUMs { get; set; }

        [SapName("ET_RETURN")]
        public SAPET_RETURN[] ET_RETURNs { get; set; }
    }

    public class SAPET_RETURN
    {
        [SapName("TYPE")]
        public string TYPE { get; set; }

        [SapName("ID")]
        public string ID { get; set; }

        [SapName("NUMBER")]
        public string NUMBER { get; set; }

        [SapName("MESSAGE")]
        public string MESSAGE { get; set; }

        [SapName("LOG_NO")]
        public string LOG_NO { get; set; }

        [SapName("LOG_MSG_NO")]
        public string LOG_MSG_NO { get; set; }

        [SapName("MESSAGE_V1")]
        public string MESSAGE_V1 { get; set; }

        [SapName("MESSAGE_V2")]
        public string MESSAGE_V2 { get; set; }

        [SapName("MESSAGE_V3")]
        public string MESSAGE_V3 { get; set; }

        [SapName("MESSAGE_V4")]
        public string MESSAGE_V4 { get; set; }

        [SapName("PARAMETER")]
        public string PARAMETER { get; set; }

        [SapName("ROW")]
        public string ROW { get; set; }

        [SapName("FIELD")]
        public string FIELD { get; set; }

        [SapName("SYSTEM")]
        public string SYSTEM { get; set; }
    }
}