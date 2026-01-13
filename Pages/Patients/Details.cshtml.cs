using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HCAMiniEHR.Services;
using HCAMiniEHR.Services.Dtos;

namespace HCAMiniEHR.Pages.Patients
{
    public class DetailsModel : PageModel
    {
        private readonly IPatientService _patientService;

        public DetailsModel(IPatientService patientService)
        {
            _patientService = patientService;
        }

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
    }
}
