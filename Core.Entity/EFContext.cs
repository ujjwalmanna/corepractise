using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace Core.Entity
{
    public class EFContext: DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Designation> Designations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\\LOCALSQL;Initial Catalog=testdb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Entity<Camp>()
            //    .Property(c => c.Moniker)
            //    .IsRequired();
            //builder.Entity<Camp>()
            //    .Property(c => c.RowVersion)
            //    .ValueGeneratedOnAddOrUpdate()
            //    .IsConcurrencyToken();
            //builder.Entity<Speaker>()
            //    .Property(c => c.RowVersion)
            //    .ValueGeneratedOnAddOrUpdate()
            //    .IsConcurrencyToken();
            //builder.Entity<Talk>()
            //    .Property(c => c.RowVersion)
            //    .ValueGeneratedOnAddOrUpdate()
            //    .IsConcurrencyToken();
        }
    }
}
