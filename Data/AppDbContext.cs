using InternalBudgetTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace InternalBudgetTracker.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Department>Departments { get; set; }
        //public DbSet<Expense> Expenses { get; set; }
        //public DbSet<Report> Reports { get; set; }
    }
}
