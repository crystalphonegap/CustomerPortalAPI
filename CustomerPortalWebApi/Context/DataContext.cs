using CustomerPortalWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerPortalWebApi.Context
{
    public class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public virtual DbSet<UserMasterModel> UserMaster { get; set; }
    }
}