using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entity.Entities
{
    [Table("Designation", Schema = "dbo")]
    public class Designation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(20)")]
        [Display(Name = "Designation Code")]
        public string Code { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        [Display(Name = "Designation Name")]
        public string Name { get; set; }

    }
}
