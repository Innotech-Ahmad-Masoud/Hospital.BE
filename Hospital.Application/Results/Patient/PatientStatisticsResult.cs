namespace Hospital.Application.Results.Patient
{
    public class PatientStatisticsResult
    {
        public string? Name { get; set; }
        public int? Age { get; set; }
        public decimal AverageOfBills { get; set; }
        public double AverageOfBillsOutliers { get; set; }
        public PatientRecordResult? The5RecordEntryOfPatient { get; set; }
        public List<PatientResult> PatientsWithSimilarDiseases { get; set; } = new List<PatientResult>();
        public string? MonthHighestNumberOfVisits { get; set; }
    }
}
