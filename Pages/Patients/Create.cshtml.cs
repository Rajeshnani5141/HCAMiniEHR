using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HCAMiniEHR.Services;
using HCAMiniEHR.Services.Dtos;

namespace HCAMiniEHR.Pages.Patients
{
    public class CreateModel : PageModel
    {
        private readonly IPatientService _patientService;

        public CreateModel(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [BindProperty]
        public PatientDto Patient { get; set; } = new PatientDto();

        public IActionResult OnGet()
        {
            Patient.DateOfBirth = DateTime.Today;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _patientService.CreatePatientAsync(Patient);
            return RedirectToPage("./Index");
        }
    }
}
