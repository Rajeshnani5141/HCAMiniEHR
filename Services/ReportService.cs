using HCAMiniEHR.Data;
using Microsoft.EntityFrameworkCore;

namespace HCAMiniEHR.Services
{
    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _context;

        public ReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Report 1: Pending Lab Orders
        public async Task<IEnumerable<ReportDto>> GetPendingLabOrdersAsync()
        {
            var pendingOrders = await _context.LabOrders
                .Where(l => l.Status == "Pending")
                .Include(l => l.Appointment)
                .ThenInclude(a => a.Patient)
                .Select(l => new ReportDto
                {
                    LabOrderId = l.Id,
                    TestName = l.TestName,
                    OrderDate = l.OrderDate,
                    PatientName = l.Appointment.Patient.FirstName + " " + l.Appointment.Patient.LastName,
                    AppointmentDate = l.Appointment.AppointmentDate,
                    Status = l.Status
                })
                .OrderBy(r => r.OrderDate)
                .ToListAsync();

            return pendingOrders;
        }

        // Report 2: Patients Without Follow-Up (no future appointments)
        public async Task<IEnumerable<ReportDto>> GetPatientsWithoutFollowUpAsync()
        {
            var today = DateTime.Today;
            
            var patientsWithoutFollowUp = await _context.Patients
                .Where(p => !p.Appointments.Any(a => a.AppointmentDate > today))
                .Select(p => new ReportDto
                {
                    PatientId = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Email = p.Email,
                    LastAppointmentDate = p.Appointments
                        .OrderByDescending(a => a.AppointmentDate)
                        .Select(a => (DateTime?)a.AppointmentDate)
                        .FirstOrDefault()
                })
                .OrderBy(r => r.LastName)
                .ThenBy(r => r.FirstName)
                .ToListAsync();

            return patientsWithoutFollowUp;
        }

        // Report 3: Doctor Productivity (appointments grouped by doctor)
        public async Task<IEnumerable<ReportDto>> GetDoctorProductivityAsync()
        {
            var doctorStats = await _context.Appointments
                .GroupBy(a => a.DoctorName)
                .Select(g => new ReportDto
                {
                    DoctorName = g.Key,
                    TotalAppointments = g.Count(),
                    CompletedAppointments = g.Count(a => a.Status == "Completed"),
                    ScheduledAppointments = g.Count(a => a.Status == "Scheduled")
                })
                .OrderByDescending(r => r.TotalAppointments)
                .ToListAsync();

            return doctorStats;
        }
    }
}
