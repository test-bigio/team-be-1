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
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Leave> Leaves { get; set; }
        public DbSet<Delegation> Delegations { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<DigitalPinLog> DigitalPinLogs { get; set; }
    }
}
