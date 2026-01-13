using HCAMiniEHR.Services.Dtos;

namespace HCAMiniEHR.Services
{
    public interface IAppointmentService
    {
        Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync();
        Task<IEnumerable<AppointmentDto>> GetAppointmentsByPatientIdAsync(int patientId);
        Task<AppointmentDto?> GetAppointmentByIdAsync(int id);
        Task<AppointmentDto> CreateAppointmentAsync(AppointmentDto appointment);
        Task<int> CreateAppointmentViaSPAsync(int patientId, DateTime appointmentDate, string reason, string doctorName, string status = "Scheduled");
        Task UpdateAppointmentAsync(AppointmentDto appointment);
        Task DeleteAppointmentAsync(int id);
        Task<IEnumerable<DoctorDto>> GetDoctorsAsync();
    }
}
