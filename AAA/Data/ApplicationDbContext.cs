using AAA.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AAA.Data
{


    // что делает ApplicationDbContext:
    //Связывает модели с SQL
    //используем фреймворк indentity


    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }



        public DbSet<Employee> Employees { get; set; }
        public DbSet<WorkLog> WorkLogs { get; set; }
        //public DbSet<User> User { get; set; }


        //Если удалить сотрудника — его рабочие записи (WorkLogs) тоже удалятся автоматически 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WorkLog>()
                .HasOne(w => w.Employee)
                .WithMany(e => e.WorkLogs)
                .HasForeignKey(w => w.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
    
}
