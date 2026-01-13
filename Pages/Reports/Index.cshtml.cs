using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HCAMiniEHR.Services;

namespace HCAMiniEHR.Pages.Reports
{
    public class IndexModel : PageModel
    {
        private readonly IReportService _reportService;
        private readonly ILabOrderService _labOrderService;

        public IndexModel(IReportService reportService, ILabOrderService labOrderService)
        {
            _reportService = reportService;
            _labOrderService = labOrderService;
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
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                await _labOrderService.DeleteLabOrderAsync(id);
                TempData["SuccessMessage"] = "Lab order deleted successfully.";
            }
            catch (Exception ex)
            {
                // In a real app, log the error
                TempData["ErrorMessage"] = "Error deleting lab order: " + ex.Message;
            }
            return RedirectToPage();
        }
    }
}
