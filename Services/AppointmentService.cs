using HCAMiniEHR.Models;
using HCAMiniEHR.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using HCAMiniEHR.Services.Dtos;

namespace HCAMiniEHR.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _context;

        public AppointmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync()
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .OrderByDescending(a => a.AppointmentDate)
                .Select(a => new AppointmentDto
                {
                    Id = a.Id,
                    PatientId = a.PatientId,
                    PatientName = a.Patient.FirstName + " " + a.Patient.LastName,
                    DoctorId = a.DoctorId,
                    DoctorName = a.DoctorName,
                    AppointmentDate = a.AppointmentDate,
                    Reason = a.Reason,
                    Status = a.Status
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<AppointmentDto>> GetAppointmentsByPatientIdAsync(int patientId)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.PatientId == patientId)
                .OrderByDescending(a => a.AppointmentDate)
                .Select(a => new AppointmentDto
                {
                    Id = a.Id,
                    PatientId = a.PatientId,
                    PatientName = a.Patient.FirstName + " " + a.Patient.LastName,
                    DoctorId = a.DoctorId,
                    DoctorName = a.DoctorName,
                    AppointmentDate = a.AppointmentDate,
                    Reason = a.Reason,
                    Status = a.Status
                })
                .ToListAsync();
        }

        public async Task<AppointmentDto?> GetAppointmentByIdAsync(int id)
        {
            var a = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(a => a.LabOrders)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (a == null) return null;

            var dto = new AppointmentDto
            {
                Id = a.Id,
                PatientId = a.PatientId,
                PatientName = a.Patient != null ? a.Patient.FirstName + " " + a.Patient.LastName : "Unknown",
                DoctorId = a.DoctorId,
                DoctorName = a.DoctorName,
                AppointmentDate = a.AppointmentDate,
                Reason = a.Reason,
                Status = a.Status
            };

            if (a.LabOrders != null)
            {
                dto.LabOrders = a.LabOrders.Select(l => new LabOrderDto
                {
                    Id = l.Id,
                    AppointmentId = l.AppointmentId,
                    OrderDate = l.OrderDate,
                    TestName = l.TestName,
                    Status = l.Status,
                    Results = l.Results
                }).ToList();
            }

            return dto;
        }

        public async Task<AppointmentDto> CreateAppointmentAsync(AppointmentDto dto)
        {
            // 0. Normalize Time: Remove seconds and milliseconds to ensure slot limits work correctly (e.g. 10:00:05 -> 10:00:00)
            dto.AppointmentDate = new DateTime(
                dto.AppointmentDate.Year, 
                dto.AppointmentDate.Month, 
                dto.AppointmentDate.Day, 
                dto.AppointmentDate.Hour, 
                dto.AppointmentDate.Minute, 
                0);

            // 1. Validation: Check if the doctor is available at this time
            // 1. Validation: Prevent Same Patient, Same Doctor, Same Day (Duplicate Booking)
            var isDuplicateBooking = await _context.Appointments
                .AnyAsync(a => a.PatientId == dto.PatientId 
                            && a.DoctorId == dto.DoctorId 
                            && a.AppointmentDate.Date == dto.AppointmentDate.Date
                            && a.Status != "Cancelled");

            if (isDuplicateBooking)
            {
                throw new InvalidOperationException("You already have an appointment with this doctor on this day.");
            }

            // 2. Validation: Daily Limit (Max 2 Patients per Doctor per Day)
            var dailyCount = await _context.Appointments
                .CountAsync(a => a.DoctorId == dto.DoctorId 
                            && a.AppointmentDate.Date == dto.AppointmentDate.Date
                            && a.Status != "Cancelled");

            if (dailyCount >= 2)
            {
                throw new InvalidOperationException($"Doctor is fully booked for this day (Max 2 patients). Please choose another date.");
            }

            var appointment = new Appointment
            {
                PatientId = dto.PatientId,
                DoctorId = dto.DoctorId,
                AppointmentDate = dto.AppointmentDate,
                Reason = dto.Reason,
                DoctorName = dto.DoctorName ?? string.Empty,
                Status = dto.Status
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            
            dto.Id = appointment.Id;
            return dto;
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

        public async Task UpdateAppointmentAsync(AppointmentDto dto)
        {
            var a = await _context.Appointments.FindAsync(dto.Id);
            if (a != null)
            {
                a.PatientId = dto.PatientId;
                a.DoctorId = dto.DoctorId;
                a.AppointmentDate = dto.AppointmentDate;
                a.Reason = dto.Reason;
                a.DoctorName = dto.DoctorName ?? string.Empty;
                a.Status = dto.Status;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAppointmentAsync(int id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.LabOrders)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment != null)
            {
                // Cascade Delete: Remove associated LabOrders first
                if (appointment.LabOrders.Any())
                {
                    _context.LabOrders.RemoveRange(appointment.LabOrders);
                }

                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<DoctorDto>> GetDoctorsAsync()
        {
            return await _context.Doctors
                .OrderBy(d => d.LastName)
                .Select(d => new DoctorDto
                {
                    Id = d.Id,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    Specialization = d.Specialization
                })
                .ToListAsync();
        }
    }
}
