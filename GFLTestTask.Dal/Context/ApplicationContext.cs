using GFLTestTask.Dal.Entities;
using GFLTestTask.Dal.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace GFLTestTask.Dal.Context
{
    public partial class ApplicationContext : DbContext, IApplicationContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> User { get; set; }   
        public virtual DbSet<Position> Position { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }

        public async Task<bool> SaveChangesAsync()
        {
            int changes = ChangeTracker
                          .Entries()
                          .Count(p => p.State == EntityState.Modified
                                   || p.State == EntityState.Deleted
                                   || p.State == EntityState.Added);

            if (changes == 0) return true;

            return await base.SaveChangesAsync() > 0;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Employee>()
                .HasOne(p => p.User)
               .WithMany()
               .HasForeignKey(p => p.Id);

            modelBuilder.Entity<Employee>()
                .HasOne(p => p.Position)
               .WithMany()
               .HasForeignKey(p => p.PositionId);

            modelBuilder.Entity<User>()
                .HasKey(p => p.Id);


        }
    }
}
