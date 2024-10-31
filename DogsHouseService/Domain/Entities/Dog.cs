using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Dog
    {
        [Key]
        public string Name { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        [Column("tail_length")]
        public int TailLength { get; set; }
        [Required]
        public int Weight { get; set; }
    }
}
