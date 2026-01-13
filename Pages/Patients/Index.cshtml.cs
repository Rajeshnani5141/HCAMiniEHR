using Microsoft.AspNetCore.Mvc.RazorPages;
using HCAMiniEHR.Services;
using HCAMiniEHR.Services.Dtos;

namespace HCAMiniEHR.Pages.Patients
{
    public class IndexModel : PageModel
    {
        private readonly IPatientService _patientService;

        public IndexModel(IPatientService patientService)
        {
            _patientService = patientService;
        }

        public IEnumerable<PatientDto> Patients { get; set; } = new List<PatientDto>();

        public async Task OnGetAsync()
        {
            Patients = await _patientService.GetAllPatientsAsync();
        }
    }
}
