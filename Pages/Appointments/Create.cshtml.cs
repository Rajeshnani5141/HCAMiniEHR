using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using HCAMiniEHR.Services;
using HCAMiniEHR.Services.Dtos;

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
        public AppointmentDto Appointment { get; set; } = new AppointmentDto();
        
        public SelectList PatientList { get; set; } = new SelectList(new List<PatientDto>(), "Id", "FirstName");
        public SelectList DoctorList { get; set; } = new SelectList(new List<DoctorDto>(), "Id", "LastName");

        public async Task<IActionResult> OnGetAsync()
        {
            Appointment.AppointmentDate = DateTime.Now;

            var patients = await _patientService.GetAllPatientsAsync();
            PatientList = new SelectList(patients, "Id", "FullName");
            
            var doctors = await _appointmentService.GetDoctorsAsync();
            DoctorList = new SelectList(doctors, "Id", "FullName");
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var patients = await _patientService.GetAllPatientsAsync();
                PatientList = new SelectList(patients, "Id", "FullName");
                
                var doctors = await _appointmentService.GetDoctorsAsync();
                DoctorList = new SelectList(doctors, "Id", "FullName");
                
                return Page();
            }

            // If a doctor is selected, update the DoctorName string for backward compatibility
            if (Appointment.DoctorId.HasValue)
            {
                var doctors = await _appointmentService.GetDoctorsAsync();
                var selectedDoctor = doctors.FirstOrDefault(d => d.Id == Appointment.DoctorId);
                if (selectedDoctor != null)
                {
                    Appointment.DoctorName = selectedDoctor.FullName;
                }
            }

            try
            {
                await _appointmentService.CreateAppointmentAsync(Appointment);
                return RedirectToPage("./Index");
            }
            catch (InvalidOperationException ex)
            {
                // Conflict detected: Show error and reload lists
                ModelState.AddModelError("Appointment.AppointmentDate", ex.Message);
                
                var patients = await _patientService.GetAllPatientsAsync();
                PatientList = new SelectList(patients, "Id", "FullName");
                
                var doctors = await _appointmentService.GetDoctorsAsync();
                DoctorList = new SelectList(doctors, "Id", "FullName");

                return Page();
            }
        }
    }
}
