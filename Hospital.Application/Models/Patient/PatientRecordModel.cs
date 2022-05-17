using System.ComponentModel.DataAnnotations;

namespace Hospital.Application.Models.Patient
{
    public class PatientRecordModel
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "Patient ID is required")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Disease Name is required")]
        public string? DiseaseName { get; set; }

        public DateTime? TimeOfEntry { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Bill is required")]
        public decimal Bill { get; set; }
    }
}
