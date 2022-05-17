using Hospital.Application.Models.Patient;
using Hospital.Application.Parameters.Patient;
using Hospital.Application.Results;
using Hospital.Application.Results.Patient;

namespace Hospital.Application.Interfaces
{
    public interface IPatientService
    {
        Task<PagedResponse<List<PatientResult>>> GetPatients(PatientParameter parameter);
        Task<Response<PatientResult>> CreatePatient(PatientModel model);
        Task<Response<PatientResult>> UpdatePatient(PatientModel model);
        Task<Response<PatientRecordResult>> CreatePatientRecord(PatientRecordModel model);    
        Task<Response<PatientStatisticsResult>> GetPatientStatistics(PatientStatisticsParameter parameter);
        Task<Response<string>> AddSampleData();
    }
}
