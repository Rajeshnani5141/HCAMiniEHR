using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HCAMiniEHR.Models;
using HCAMiniEHR.Services;

namespace HCAMiniEHR.Pages.Appointments
{
    public class EditModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IPatientService _patientService;

        public EditModel(IAppointmentService appointmentService, IPatientService patientService)
        {
            _appointmentService = appointmentService;
            _patientService = patientService;
        }

        [BindProperty]
        public Appointment Appointment { get; set; } = default!;

        public SelectList PatientList { get; set; } = default!;
        public SelectList DoctorList { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _appointmentService.GetAppointmentByIdAsync(id.Value);
            if (appointment == null)
            {
                return NotFound();
            }
            Appointment = appointment;

            var patients = await _patientService.GetAllPatientsAsync();
            PatientList = new SelectList(patients, "Id", "FirstName");

            var doctors = await _appointmentService.GetDoctorsAsync();
            DoctorList = new SelectList(doctors, "Id", "LastName", Appointment.DoctorId, "Specialization");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var patients = await _patientService.GetAllPatientsAsync();
                PatientList = new SelectList(patients, "Id", "FirstName");
                
                var doctors = await _appointmentService.GetDoctorsAsync();
                DoctorList = new SelectList(doctors, "Id", "LastName", Appointment.DoctorId, "Specialization");
                
                return Page();
            }

            // Update DoctorName based on selection
            if (Appointment.DoctorId.HasValue)
            {
                var doctors = await _appointmentService.GetDoctorsAsync();
                var selectedDoctor = doctors.FirstOrDefault(d => d.Id == Appointment.DoctorId);
                if (selectedDoctor != null)
                {
                    Appointment.DoctorName = $"Dr. {selectedDoctor.LastName}";
                }
            }

            try
            {
                await _appointmentService.UpdateAppointmentAsync(Appointment);
            }
            catch (DbUpdateConcurrencyException)
            {
                 if (!await AppointmentExists(Appointment.Id))
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

        private async Task<bool> AppointmentExists(int id)
        {
             var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
             return appointment != null;
        }
    }
}
