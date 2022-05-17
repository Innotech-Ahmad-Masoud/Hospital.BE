using Bogus;
using Hospital.Application.Interfaces;
using Hospital.Application.Models.Patient;
using Hospital.Application.Parameters.Patient;
using Hospital.Application.Results;
using Hospital.Application.Results.Patient;
using Hospital.Core.Entities;
using Hospital.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Hospital.Application.Services
{
    public class PatientService : IPatientService
    {
        private readonly ApplicationDbContext _dbContext;

        public PatientService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResponse<List<PatientResult>>> GetPatients(PatientParameter parameter)
        {
            var pagedData = await _dbContext.Patient
                .Skip((parameter.PageNumber - 1) * parameter.PageSize)
                .Take(parameter.PageSize)
                .Select(c => new PatientResult()
                {
                    Name = c.Name,
                    DateOfBirth = c.DateOfBirth,
                    SystemIdNumber = c.SystemIdNumber,
                    LastEntry = c.PatientRecords.OrderByDescending(p => p.TimeOfEntry).FirstOrDefault()!.TimeOfEntry
                })
                .ToListAsync();

            var totalRecords = await _dbContext.Patient.CountAsync();
            var totalPages = totalRecords / (double)parameter.PageSize;
            var result = new PagedResponse<List<PatientResult>>(pagedData, parameter.PageNumber, parameter.PageSize)
            {
                Status = nameof(HttpStatusCode.OK),
                TotalRecords = totalRecords,
                TotalPages = Convert.ToInt32(Math.Ceiling(totalPages))
            };
            return result;
        }

        public async Task<Response<PatientResult>> CreatePatient(PatientModel model)
        {
            try
            {
                var newPatient = new Patient()
                {
                    Name = model.Name,
                    OfficialIdNumber = model.OfficialIdNumber,
                    DateOfBirth = model.DateOfBirth,
                    EmailAddress = model.EmailAddress
                };

                var patient = await _dbContext.Patient.AddAsync(newPatient);
                await _dbContext.SaveChangesAsync();
                return new Response<PatientResult>()
                {
                    Status = nameof(HttpStatusCode.OK),
                    Message = "Patient Added Successfully!",
                    Data = new PatientResult()
                    {
                        SystemIdNumber = patient.Entity.SystemIdNumber,
                        Name = patient.Entity.Name,
                        DateOfBirth = patient.Entity.DateOfBirth,
                        LastEntry = null,
                    }
                };
            }
            catch (Exception ex)
            {
                return new Response<PatientResult>()
                {
                    Status = nameof(HttpStatusCode.BadRequest),
                    Message = ex.Message
                };
            }
        }

        public async Task<Response<PatientResult>> UpdatePatient(PatientModel model)
        {
            try
            {
                if (model.SystemIdNumber == null)
                    return new Response<PatientResult>()
                    {
                        Status = nameof(HttpStatusCode.BadRequest),
                        Message = "Id is required"
                    };
                var patient = await _dbContext.Patient.FirstOrDefaultAsync(c => c.SystemIdNumber == model.SystemIdNumber);
                if (patient == null)
                    return new Response<PatientResult>()
                    {
                        Status = nameof(HttpStatusCode.NotFound),
                        Message = $"There is no patient with Id '{model.SystemIdNumber}'"
                    };

                patient.Name = model.Name;
                patient.OfficialIdNumber = model.OfficialIdNumber;
                patient.DateOfBirth = model.DateOfBirth;
                patient.EmailAddress = model.EmailAddress;

                var updatedPatient = _dbContext.Patient.Update(patient);
                await _dbContext.SaveChangesAsync();
                return new Response<PatientResult>()
                {
                    Status = nameof(HttpStatusCode.OK),
                    Message = "Patient Updated Successfully!",
                    Data = new PatientResult()
                    {
                        SystemIdNumber = updatedPatient.Entity.SystemIdNumber,
                        Name = updatedPatient.Entity.Name,
                        DateOfBirth = updatedPatient.Entity.DateOfBirth,
                        LastEntry = null,
                    }
                };
            }
            catch (Exception ex)
            {
                return new Response<PatientResult>()
                {
                    Status = nameof(HttpStatusCode.BadRequest),
                    Message = ex.Message
                };
            }
        }

        public async Task<Response<PatientRecordResult>> CreatePatientRecord(PatientRecordModel model)
        {
            try
            {
                var patient = await _dbContext.Patient.FirstOrDefaultAsync(c => c.SystemIdNumber == model.PatientId);
                if (patient == null)
                    return new Response<PatientRecordResult>()
                    {
                        Status = nameof(HttpStatusCode.NotFound),
                        Message = $"There is no patient with Id '{model.Id}'"
                    };

                var newPatientRecord = new PatientRecord()
                {
                    DiseaseName = model.DiseaseName,
                    PatientId = model.PatientId,
                    TimeOfEntry = model.TimeOfEntry ?? DateTime.Now,
                    Bill = model.Bill,
                    Description = model.Description
                };

                var patientRecord = await _dbContext.PatientRecord.AddAsync(newPatientRecord);
                await _dbContext.SaveChangesAsync();
                return new Response<PatientRecordResult>()
                {
                    Status = nameof(HttpStatusCode.OK),
                    Message = "Patient Added Successfully!",
                    Data = new PatientRecordResult()
                    {
                        TimeOfEntry = patientRecord.Entity.TimeOfEntry,
                        Bill = patientRecord.Entity.Bill,
                        Description = patientRecord.Entity.Description,
                        DiseaseName = patientRecord.Entity.DiseaseName
                    }
                };
            }
            catch (Exception ex)
            {
                return new Response<PatientRecordResult>()
                {
                    Status = nameof(HttpStatusCode.BadRequest),
                    Message = ex.Message
                };
            }
        }

        public async Task<Response<PatientStatisticsResult>> GetPatientStatistics(PatientStatisticsParameter parameter)
        {
            try
            {
                var patient = await _dbContext.Patient.Include(c => c.PatientRecords).FirstOrDefaultAsync(c => c.SystemIdNumber == parameter.PatientId);
                if (patient == null)
                    return new Response<PatientStatisticsResult>()
                    {
                        Status = nameof(HttpStatusCode.NotFound),
                        Message = $"There is no patient with Id '{parameter.PatientId}'"
                    };
                var patientStatistics = _dbContext.PatientStatistics.First(c => c.SystemIdNumber == patient.SystemIdNumber);
                var the5RecordEntryOfPatient = patient.PatientRecords.OrderByDescending(c => c.TimeOfEntry).Skip(4).Take(1).FirstOrDefault();

                var patientDiseases = patient.PatientRecords.Select(c => c.DiseaseName);
                var patientsWithSimilarDiseases = _dbContext.PatientRecord.Include(c=>c.Patient)
                                                            .Where(c => patientDiseases.Contains(c.DiseaseName))
                                                            .AsEnumerable()
                                                            .GroupBy(c => c.Patient)
                                                            .Where(c => c.Count() >= 2)
                                                            .Select(c => c.Key);

                var statistics = new PatientStatisticsResult
                {
                    Name = patientStatistics.Name,
                    Age = patientStatistics.Age,
                    AverageOfBills = patientStatistics.AverageOfBills,
                    AverageOfBillsOutliers = patientStatistics.AverageOfBillsRemovingOutliers,
                    The5RecordEntryOfPatient = the5RecordEntryOfPatient != null ? new PatientRecordResult()
                    {
                        Id = the5RecordEntryOfPatient.Id,
                        Bill = the5RecordEntryOfPatient.Bill,
                        Description = the5RecordEntryOfPatient.Description,
                        DiseaseName = the5RecordEntryOfPatient.DiseaseName,
                        TimeOfEntry = the5RecordEntryOfPatient.TimeOfEntry,
                    } : null,
                    PatientsWithSimilarDiseases = patientsWithSimilarDiseases.Select(c => new PatientResult()
                    {
                        Name = c.Name,
                        SystemIdNumber = c.SystemIdNumber,
                        DateOfBirth = c.DateOfBirth
                    }).ToList(),
                    MonthHighestNumberOfVisits = patientStatistics.Month
                };

                return new Response<PatientStatisticsResult>()
                {
                    Status = nameof(HttpStatusCode.OK),
                    Message = "Patient Statistics Successfully!",
                    Data = statistics
                };
            }
            catch (Exception ex)
            {
                return new Response<PatientStatisticsResult>()
                {
                    Status = nameof(HttpStatusCode.BadRequest),
                    Message = ex.Message
                };
            }
        }

        public async Task<Response<string>> AddSampleData()
        {
            var officialId = 10;
            var diseaseNames = new[] {"Anthrax",
                "Botulism",
                "Brucellosis",
                "Chikungunya virus disease",
                "Cholera" ,
                "Coronavirus disease (COVID-19)",
                "Dengue",
                "Ebola virus disease"};

            var patients = new Faker<Patient>("en_US")
                .RuleFor(u => u.Name, (f, u) => f.Person.FullName)
                .RuleFor(u => u.EmailAddress, (f, u) => f.Internet.Email(u.Name))
                .RuleFor(u => u.OfficialIdNumber, (f, u) => (officialId += 10).ToString())
                .RuleFor(u => u.DateOfBirth, (f, u) => f.Person.DateOfBirth)
                .RuleFor(u => u.PatientRecords, f =>
                    new Faker<PatientRecord>("en_US")
                        .RuleFor(u => u.DiseaseName, faker => faker.PickRandom(diseaseNames))
                        .RuleFor(u => u.Description, faker => faker.Random.Words(20))
                        .RuleFor(u => u.Bill, faker => faker.Random.Decimal(1, 200))
                        .RuleFor(u => u.TimeOfEntry, faker => faker.Date.Between(new DateTime(2000, 1, 1), DateTime.Now))
                        .Generate(10))
                .Generate(500);

            await _dbContext.Patient.AddRangeAsync(patients);
            await _dbContext.SaveChangesAsync();

            return new Response<string>()
            {
                Status = nameof(HttpStatusCode.OK),
                Message = "Sample Data Added Successfully!"
            };
        }

        public int GetAge(DateTime dateOfBirth)
        {
            var age = DateTime.Now.Subtract(dateOfBirth).Days;
            age /= 365;
            return age;
        }

        public decimal? GetAverage(List<PatientRecord>? patientRecords)
        {
            if (patientRecords != null && patientRecords.Any())
            {
                return patientRecords.Average(c => c.Bill);
            }

            return null;
        }
    }
}
