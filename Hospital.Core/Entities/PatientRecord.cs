using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Core.Entities
{
    [Table("PatientRecord")]
    public class PatientRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string? DiseaseName { get; set; }

        public DateTime TimeOfEntry { get; set; }

        public string? Description { get; set; }

        [Required]
        public decimal Bill { get; set; }

        [Required]
        public int PatientId { get; set; }

        public Patient? Patient { get; set; }
    }
}
