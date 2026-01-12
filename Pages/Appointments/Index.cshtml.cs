using Microsoft.AspNetCore.Mvc.RazorPages;
using HCAMiniEHR.Models;
using HCAMiniEHR.Services;

namespace HCAMiniEHR.Pages.Appointments
{
    public class IndexModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;

        public IndexModel(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public IEnumerable<Appointment> Appointments { get; set; } = new List<Appointment>();

        public async Task OnGetAsync()
        {
            Appointments = await _appointmentService.GetAllAppointmentsAsync();
        }
    }
}
