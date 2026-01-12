using HCAMiniEHR.Models;
using HCAMiniEHR.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;

namespace HCAMiniEHR.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _context;

        public AppointmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.LabOrders)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByPatientIdAsync(int patientId)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.LabOrders)
                .Where(a => a.PatientId == patientId)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<Appointment?> GetAppointmentByIdAsync(int id)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.LabOrders)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Appointment> CreateAppointmentAsync(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }

        public async Task<int> CreateAppointmentViaSPAsync(int patientId, DateTime appointmentDate, string reason, string doctorName, string status = "Scheduled")
        {
            var patientIdParam = new SqlParameter("@PatientId", patientId);
            var appointmentDateParam = new SqlParameter("@AppointmentDate", appointmentDate);
            var reasonParam = new SqlParameter("@Reason", reason);
            var doctorNameParam = new SqlParameter("@DoctorName", doctorName);
            var statusParam = new SqlParameter("@Status", status);
            var newIdParam = new SqlParameter
            {
                ParameterName = "@NewAppointmentId",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC [Healthcare].[sp_CreateAppointment] @PatientId, @AppointmentDate, @Reason, @DoctorName, @Status, @NewAppointmentId OUTPUT",
                patientIdParam, appointmentDateParam, reasonParam, doctorNameParam, statusParam, newIdParam);

            return (int)newIdParam.Value;
        }

        public async Task UpdateAppointmentAsync(Appointment appointment)
        {
            _context.Entry(appointment).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAppointmentAsync(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();
            }
        }
    }
}
