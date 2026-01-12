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
        public SelectList DoctorList { get; set; } = new SelectList(new List<Doctor>(), "Id", "LastName");

        public async Task<IActionResult> OnGetAsync()
        {
            var patients = await _patientService.GetAllPatientsAsync();
            PatientList = new SelectList(patients, "Id", "FirstName");
            
            var doctors = await _appointmentService.GetDoctorsAsync();
            DoctorList = new SelectList(doctors, "Id", "LastName", null, "Specialization");
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var patients = await _patientService.GetAllPatientsAsync();
                PatientList = new SelectList(patients, "Id", "FirstName");
                
                var doctors = await _appointmentService.GetDoctorsAsync();
                DoctorList = new SelectList(doctors, "Id", "LastName", null, "Specialization");
                
                return Page();
            }

            // If a doctor is selected, update the DoctorName string for backward compatibility
            if (Appointment.DoctorId.HasValue)
            {
                var doctors = await _appointmentService.GetDoctorsAsync();
                var selectedDoctor = doctors.FirstOrDefault(d => d.Id == Appointment.DoctorId);
                if (selectedDoctor != null)
                {
                    Appointment.DoctorName = $"Dr. {selectedDoctor.LastName}";
                }
            }

            await _appointmentService.CreateAppointmentAsync(Appointment);
            return RedirectToPage("./Index");
        }
    }
}
