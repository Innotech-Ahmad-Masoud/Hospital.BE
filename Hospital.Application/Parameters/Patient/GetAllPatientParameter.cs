namespace Hospital.Application.Parameters.Patient
{
    public class PatientParameter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public PatientParameter()
        {
            PageNumber = 1;
            PageSize = 10;
        }
    }
}
