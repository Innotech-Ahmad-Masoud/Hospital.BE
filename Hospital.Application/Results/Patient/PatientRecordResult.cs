namespace Hospital.Application.Results.Patient
{
    public class PatientRecordResult
    {
        public int Id { get; set; }
        public string? DiseaseName { get; set; }
        public DateTime TimeOfEntry { get; set; }
        public string? Description { get; set; }
        public decimal Bill { get; set; }
    }
}
