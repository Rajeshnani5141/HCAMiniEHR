using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using HCAMiniEHR.Services;
using HCAMiniEHR.Services.Dtos;

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
        public LabOrderDto LabOrder { get; set; } = new LabOrderDto { OrderDate = DateTime.Today };
        
        public SelectList AppointmentList { get; set; } = new SelectList(new List<AppointmentDto>(), "Id", "Reason");

        public async Task<IActionResult> OnGetAsync()
        {
            var appointments = await _appointmentService.GetAllAppointmentsAsync();
            var appointmentItems = appointments.Select(a => new {
                Id = a.Id,
                DisplayText = $"#{a.Id} - {a.PatientName} ({a.AppointmentDate:d})"
            });
            AppointmentList = new SelectList(appointmentItems, "Id", "DisplayText");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var appointments = await _appointmentService.GetAllAppointmentsAsync();
                var appointmentItems = appointments.Select(a => new {
                    Id = a.Id,
                    DisplayText = $"#{a.Id} - {a.PatientName} ({a.AppointmentDate:d})"
                });
                AppointmentList = new SelectList(appointmentItems, "Id", "DisplayText");
                return Page();
            }

            await _labOrderService.CreateLabOrderAsync(LabOrder);
            return RedirectToPage("./Index");
        }
    }
}
