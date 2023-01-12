using Microsoft.EntityFrameworkCore;
using SimpleCrudApp.Models.Domain;

namespace SimpleCrudApp.Data
{
    public class MVCDemoDBcontext : DbContext
    {
        public MVCDemoDBcontext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Employee> Employees  { get; set; }
    }
}
