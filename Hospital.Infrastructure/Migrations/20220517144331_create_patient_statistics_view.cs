using Hospital.Core.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Infrastructure.Migrations
{
    public partial class create_patient_statistics_view : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = @$"
            CREATE OR ALTER VIEW [dbo].[{nameof(PatientStatistics)}] AS
                SELECT 
	                P.[SystemIdNumber],
	                P.[Name],
	                DATEDIFF(hour,p.[DateOfBirth],GETDATE())/8766 AS Age ,
	                AVG(PR.[Bill]) As AverageOfBills,
	                STDEVP(PR.Bill) AS AverageOfBillsRemovingOutliers,
	                MAX(DATENAME(MONTH, PR.TimeOfEntry)) AS Month
                FROM [dbo].[Patient] AS P
                LEFT JOIN [dbo].[PatientRecord] as PR ON P.SystemIdNumber = PR.PatientId
                GROUP By P.[SystemIdNumber],P.[Name],p.[DateOfBirth]";

            migrationBuilder.Sql(sql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@$"DROP VIEW {nameof(PatientStatistics)}");
        }
    }
}
