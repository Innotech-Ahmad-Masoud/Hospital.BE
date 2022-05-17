using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Core.Entities
{
    [Table("Patient")]
    public class Patient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SystemIdNumber { get; set; }

        [Required]
        public string? OfficialIdNumber { get; set; }

        [Required]
        public string? Name { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? EmailAddress { get; set; }

        public ICollection<PatientRecord> PatientRecords { get; set; } = new HashSet<PatientRecord>();
    }
}
