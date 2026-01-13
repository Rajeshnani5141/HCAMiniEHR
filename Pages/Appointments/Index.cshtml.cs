using Microsoft.AspNetCore.Mvc.RazorPages;
using HCAMiniEHR.Services;
using HCAMiniEHR.Services.Dtos;

namespace HCAMiniEHR.Pages.Appointments
{
    public class IndexModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;

        public IndexModel(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public IEnumerable<AppointmentDto> Appointments { get; set; } = new List<AppointmentDto>();

        public async Task OnGetAsync()
        {
            Appointments = await _appointmentService.GetAllAppointmentsAsync();
        }
    }
}
