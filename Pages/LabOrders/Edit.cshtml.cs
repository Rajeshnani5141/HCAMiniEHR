using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HCAMiniEHR.Models;
using HCAMiniEHR.Services;

namespace HCAMiniEHR.Pages.LabOrders
{
    public class EditModel : PageModel
    {
        private readonly ILabOrderService _labOrderService;
        private readonly IAppointmentService _appointmentService;

        public EditModel(ILabOrderService labOrderService, IAppointmentService appointmentService)
        {
            _labOrderService = labOrderService;
            _appointmentService = appointmentService;
        }

        [BindProperty]
        public LabOrder LabOrder { get; set; } = default!;

        public SelectList AppointmentList { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var labOrder = await _labOrderService.GetLabOrderByIdAsync(id.Value);
            if (labOrder == null)
            {
                return NotFound();
            }
            LabOrder = labOrder;

            // Ideally we should have a service method to get appointments with simple projection
            // For now, we reuse GetAllAppointmentsAsync but we might want to optimize this later
            var appointments = await _appointmentService.GetAllAppointmentsAsync();
            AppointmentList = new SelectList(appointments, "Id", "Id"); // Simplified, ideally show Patient Name + Date
            
            // Let's improve the display text for dropdown
            var appointmentItems = appointments.Select(a => new {
                Id = a.Id,
                DisplayText = $"#{a.Id} - {a.Patient?.FirstName} {a.Patient?.LastName} ({a.AppointmentDate:d})"
            });
            AppointmentList = new SelectList(appointmentItems, "Id", "DisplayText", LabOrder.AppointmentId);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var appointments = await _appointmentService.GetAllAppointmentsAsync();
                 var appointmentItems = appointments.Select(a => new {
                    Id = a.Id,
                    DisplayText = $"#{a.Id} - {a.Patient?.FirstName} {a.Patient?.LastName} ({a.AppointmentDate:d})"
                });
                AppointmentList = new SelectList(appointmentItems, "Id", "DisplayText");
                return Page();
            }

            try
            {
                await _labOrderService.UpdateLabOrderAsync(LabOrder);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await LabOrderExists(LabOrder.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private async Task<bool> LabOrderExists(int id)
        {
            var labOrder = await _labOrderService.GetLabOrderByIdAsync(id);
            return labOrder != null;
        }
    }
}
