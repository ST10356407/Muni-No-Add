using Microsoft.EntityFrameworkCore;
using Municicpality.Models;

namespace Municicpality.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<IssueReport> IssueReports { get; set; }  
    }
}
