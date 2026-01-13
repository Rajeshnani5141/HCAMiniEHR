using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HCAMiniEHR.Services;
using HCAMiniEHR.Services.Dtos;

namespace HCAMiniEHR.Pages.Appointments
{
    public class DeleteModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;
        private readonly ILabOrderService _labOrderService;

        public DeleteModel(IAppointmentService appointmentService, ILabOrderService labOrderService)
        {
            _appointmentService = appointmentService;
            _labOrderService = labOrderService;
        }

        [BindProperty]
        public AppointmentDto Appointment { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _appointmentService.GetAppointmentByIdAsync(id.Value);

            if (appointment == null)
            {
                return NotFound();
            }
            Appointment = appointment;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Pre-emptive check: Don't even try if we know child records exist
            var labOrders = await _labOrderService.GetLabOrdersByAppointmentIdAsync(id.Value);
            if (labOrders.Any())
            {
                ModelState.AddModelError(string.Empty, "Cannot delete appointment because it has associated lab reports. Please delete the lab reports first.");
                Appointment = await _appointmentService.GetAppointmentByIdAsync(id.Value) ?? new AppointmentDto();
                return Page();
            }

            try
            {
                await _appointmentService.DeleteAppointmentAsync(id.Value);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                Appointment = await _appointmentService.GetAppointmentByIdAsync(id.Value) ?? new AppointmentDto();
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
