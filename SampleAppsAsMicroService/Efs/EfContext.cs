using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SampleAppsAsMicroService.Models;

namespace SampleAppsAsMicroService.Efs
{
    public class EfContext:DbContext
    {
        public EfContext(DbContextOptions<EfContext> options):base(options)
        {
            
        }

        public  DbSet<Employee> Employees { get; set; }
        public DbSet<JobRole> JobRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasOne<JobRole>(a=>a.Role);

            modelBuilder.Entity<JobRole>().HasData(new JobRole{Id=1,Level = "I1", Title = "Intern"}, new JobRole { Id = 2, Level = "I2", Title = "Trainee" }, new JobRole { Id = 3, Level = "I3", Title = "Analyst" });
        }
    }
}
