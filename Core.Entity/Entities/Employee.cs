using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entity.Entities
{
    [Table("Employee", Schema = "dbo")]
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Employee Id")]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        [Display(Name = "Employee Name")]
        public string Name { get; set; }

        [ForeignKey("DesignationInfo")]  
        [Required]
        public int DesignationId { get; set; }

        [NotMapped]
        public string DesignationName { get; set; }

        public virtual Designation DesignationInfo { get; set; }
    }
}
