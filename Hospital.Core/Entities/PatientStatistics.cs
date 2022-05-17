using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Hospital.Core.Entities
{
    [Keyless]
    public class PatientStatistics
    {
        public int SystemIdNumber { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public decimal AverageOfBills { get; set; }
        public double AverageOfBillsRemovingOutliers { get; set; }
        public string Month { get; set; }
    }
}
