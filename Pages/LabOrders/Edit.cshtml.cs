using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using HCAMiniEHR.Services;
using HCAMiniEHR.Services.Dtos;

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
        public LabOrderDto LabOrder { get; set; } = default!;

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

            var appointments = await _appointmentService.GetAllAppointmentsAsync();
            var appointmentItems = appointments.Select(a => new {
                Id = a.Id,
                DisplayText = $"#{a.Id} - {a.PatientName} ({a.AppointmentDate:d})"
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
                    DisplayText = $"#{a.Id} - {a.PatientName} ({a.AppointmentDate:d})"
                });
                AppointmentList = new SelectList(appointmentItems, "Id", "DisplayText", LabOrder.AppointmentId);
                return Page();
            }

            try
            {
                await _labOrderService.UpdateLabOrderAsync(LabOrder);
            }
            catch (Exception)
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
