using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HCAMiniEHR.Services;
using HCAMiniEHR.Services.Dtos;

namespace HCAMiniEHR.Pages.Patients
{
    public class DeleteModel : PageModel
    {
        private readonly IPatientService _patientService;
        private readonly IAppointmentService _appointmentService;

        public DeleteModel(IPatientService patientService, IAppointmentService appointmentService)
        {
            _patientService = patientService;
            _appointmentService = appointmentService;
        }

        [BindProperty]
        public PatientDto Patient { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _patientService.GetPatientByIdAsync(id.Value);

            if (patient == null)
            {
                return NotFound();
            }
            Patient = patient;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                await _patientService.DeletePatientAsync(id.Value);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                Patient = await _patientService.GetPatientByIdAsync(id.Value) ?? new PatientDto();
                return Page();
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again.");
                Patient = await _patientService.GetPatientByIdAsync(id.Value) ?? new PatientDto();
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
