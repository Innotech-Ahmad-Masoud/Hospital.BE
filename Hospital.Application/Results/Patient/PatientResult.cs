namespace Hospital.Application.Results.Patient
{
    public class PatientResult
    {
        public string? Name { get; set; }
        public int SystemIdNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? LastEntry { get; set; }
    }
}
