using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SampleAppsAsMicroService.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [ForeignKey("RoleId")]
        public int RoleId { get; set; }
        public virtual JobRole Role { get; set; }
    }
}
