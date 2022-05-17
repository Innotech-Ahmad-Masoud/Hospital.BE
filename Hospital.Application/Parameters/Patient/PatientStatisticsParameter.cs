using System.ComponentModel.DataAnnotations;

namespace Hospital.Application.Parameters.Patient
{
    public class PatientStatisticsParameter
    {
        [Required(ErrorMessage = "Patient ID is required")]
        public int PatientId { get; set; }
    }
}
