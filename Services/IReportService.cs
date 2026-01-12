namespace HCAMiniEHR.Services
{
    public interface IReportService
    {
        Task<IEnumerable<ReportDto>> GetPendingLabOrdersAsync();
        Task<IEnumerable<ReportDto>> GetPatientsWithoutFollowUpAsync();
        Task<IEnumerable<ReportDto>> GetDoctorProductivityAsync();
    }
}
