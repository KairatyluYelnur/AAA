using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    
        public class AppDbContext : DbContext

        {
            public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
            {
            }
            public DbSet<WorkTimeEntity> WorkTimes { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                // Можно добавить конфигурации для сущностей
                modelBuilder.Entity<WorkTimeEntity>()
                    .HasIndex(w => w.EmployeeId);
            }

        
    }
    }

