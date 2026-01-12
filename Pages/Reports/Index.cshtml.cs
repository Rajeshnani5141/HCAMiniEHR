using Microsoft.AspNetCore.Mvc.RazorPages;
using HCAMiniEHR.Services;

namespace HCAMiniEHR.Pages.Reports
{
    public class IndexModel : PageModel
    {
        private readonly IReportService _reportService;

        public IndexModel(IReportService reportService)
        {
            _reportService = reportService;
        }

        public IEnumerable<ReportDto> PendingLabOrders { get; set; } = new List<ReportDto>();
        public IEnumerable<ReportDto> PatientsWithoutFollowUp { get; set; } = new List<ReportDto>();
        public IEnumerable<ReportDto> DoctorProductivity { get; set; } = new List<ReportDto>();

        public async Task OnGetAsync()
        {
            PendingLabOrders = await _reportService.GetPendingLabOrdersAsync();
            PatientsWithoutFollowUp = await _reportService.GetPatientsWithoutFollowUpAsync();
            DoctorProductivity = await _reportService.GetDoctorProductivityAsync();
        }
    }
}
