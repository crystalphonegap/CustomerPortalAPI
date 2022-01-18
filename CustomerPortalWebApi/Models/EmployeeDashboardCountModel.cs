namespace CustomerPortalWebApi.Models
{
    public class EmployeeDashboardCountModel
    {
        public long DealerCount { get; set; }
        public long MYARs { get; set; }
        public decimal? Outstanding { get; set; }

        public long MyCustomers { get; set; }
    }
}