using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using HCAMiniEHR.Models;
using HCAMiniEHR.Services;

namespace HCAMiniEHR.Pages.LabOrders
{
    public class CreateModel : PageModel
    {
        private readonly ILabOrderService _labOrderService;
        private readonly IAppointmentService _appointmentService;

        public CreateModel(ILabOrderService labOrderService, IAppointmentService appointmentService)
        {
            _labOrderService = labOrderService;
            _appointmentService = appointmentService;
        }

        [BindProperty]
        public LabOrder LabOrder { get; set; } = new LabOrder { OrderDate = DateTime.Today };
        
        public SelectList AppointmentList { get; set; } = new SelectList(new List<Appointment>(), "Id", "Reason");

        public async Task<IActionResult> OnGetAsync()
        {
            var appointments = await _appointmentService.GetAllAppointmentsAsync();
            AppointmentList = new SelectList(appointments, "Id", "Reason");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var appointments = await _appointmentService.GetAllAppointmentsAsync();
                AppointmentList = new SelectList(appointments, "Id", "Reason");
                return Page();
            }

            await _labOrderService.CreateLabOrderAsync(LabOrder);
            return RedirectToPage("./Index");
        }
    }
}
