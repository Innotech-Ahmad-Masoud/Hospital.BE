using System.ComponentModel.DataAnnotations;

namespace Hospital.Application.Models.Patient
{
    public class PatientModel
    {
        public int? SystemIdNumber { get; set; }

        [Required(ErrorMessage = "Official ID Number is required")]
        public string? OfficialIdNumber { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [EmailAddress]
        public string? EmailAddress { get; set; }
    }
}
