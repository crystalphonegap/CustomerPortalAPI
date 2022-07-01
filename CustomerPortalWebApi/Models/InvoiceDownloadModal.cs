namespace CustomerPortalWebApi.Models
{
    public class InvoiceDownloadModal
    {
        public string invoice { get; set; }
    }

    public class LadgerDataModel
    {
        public string R_KUNNR { get; set; }
        public string FDAT { get; set; }
        public string TDAT { get; set; }
    }
}