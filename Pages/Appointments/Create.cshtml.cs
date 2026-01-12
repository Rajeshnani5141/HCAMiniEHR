using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using HCAMiniEHR.Models;
using HCAMiniEHR.Services;

namespace HCAMiniEHR.Pages.Appointments
{
    public class CreateModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IPatientService _patientService;

        public CreateModel(IAppointmentService appointmentService, IPatientService patientService)
        {
            _appointmentService = appointmentService;
            _patientService = patientService;
        }

        [BindProperty]
        public Appointment Appointment { get; set; } = new Appointment();
        
        public SelectList PatientList { get; set; } = new SelectList(new List<Patient>(), "Id", "FirstName");

        public async Task<IActionResult> OnGetAsync()
        {
            var patients = await _patientService.GetAllPatientsAsync();
            PatientList = new SelectList(patients, "Id", "FirstName");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var patients = await _patientService.GetAllPatientsAsync();
                PatientList = new SelectList(patients, "Id", "FirstName");
                return Page();
            }

            await _appointmentService.CreateAppointmentAsync(Appointment);
            return RedirectToPage("./Index");
        }
    }
}
