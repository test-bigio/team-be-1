using BigioHrServices.Db.Entities;
using Microsoft.EntityFrameworkCore;

namespace BigioHrServices.Db
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<LogActivity> LogActivities { get; set; }
    }
}
