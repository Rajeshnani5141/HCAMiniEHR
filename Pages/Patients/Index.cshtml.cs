using Microsoft.AspNetCore.Mvc.RazorPages;
using HCAMiniEHR.Models;
using HCAMiniEHR.Services;

namespace HCAMiniEHR.Pages.Patients
{
    public class IndexModel : PageModel
    {
        private readonly IPatientService _patientService;

        public IndexModel(IPatientService patientService)
        {
            _patientService = patientService;
        }

        public IEnumerable<Patient> Patients { get; set; } = new List<Patient>();

        public async Task OnGetAsync()
        {
            Patients = await _patientService.GetAllPatientsAsync();
        }
    }
}
