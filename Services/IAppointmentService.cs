using HCAMiniEHR.Models;
using System.Collections;

namespace HCAMiniEHR.Services
{
    public interface IAppointmentService
    {
        Task<IEnumerable<Appointment>> GetAllAppointmentsAsync();
        Task<IEnumerable<Appointment>> GetAppointmentsByPatientIdAsync(int patientId);
        Task<Appointment?> GetAppointmentByIdAsync(int id);
        Task<Appointment> CreateAppointmentAsync(Appointment appointment);
        Task<int> CreateAppointmentViaSPAsync(int patientId, DateTime appointmentDate, string reason, string doctorName, string status = "Scheduled");
        Task UpdateAppointmentAsync(Appointment appointment);
        Task DeleteAppointmentAsync(int id);
        Task<IEnumerable> GetDoctorsAsync();
    }
}
