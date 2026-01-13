using HCAMiniEHR.Models;
using HCAMiniEHR.Data;
using Microsoft.EntityFrameworkCore;
using HCAMiniEHR.Services.Dtos;

namespace HCAMiniEHR.Services
{
    public class LabOrderService : ILabOrderService
    {
        private readonly ApplicationDbContext _context;

        public LabOrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LabOrderDto>> GetAllLabOrdersAsync()
        {
            return await _context.LabOrders
                .Include(l => l.Appointment)
                .ThenInclude(a => a.Patient)
                .OrderByDescending(l => l.OrderDate)
                .Select(l => new LabOrderDto
                {
                    Id = l.Id,
                    AppointmentId = l.AppointmentId,
                    PatientName = l.Appointment != null && l.Appointment.Patient != null 
                        ? l.Appointment.Patient.FirstName + " " + l.Appointment.Patient.LastName 
                        : "Unknown",
                    AppointmentDate = l.Appointment != null ? (DateTime?)l.Appointment.AppointmentDate : null,
                    OrderDate = l.OrderDate,
                    TestName = l.TestName,
                    Status = l.Status,
                    Results = l.Results
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<LabOrderDto>> GetLabOrdersByAppointmentIdAsync(int appointmentId)
        {
            return await _context.LabOrders
                .Where(l => l.AppointmentId == appointmentId)
                .OrderByDescending(l => l.OrderDate)
                .Select(l => new LabOrderDto
                {
                    Id = l.Id,
                    AppointmentId = l.AppointmentId,
                    OrderDate = l.OrderDate,
                    TestName = l.TestName,
                    Status = l.Status,
                    Results = l.Results
                })
                .ToListAsync();
        }

        public async Task<LabOrderDto?> GetLabOrderByIdAsync(int id)
        {
            var l = await _context.LabOrders
                .Include(l => l.Appointment)
                .ThenInclude(a => a.Patient)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (l == null) return null;

            return new LabOrderDto
            {
                Id = l.Id,
                AppointmentId = l.AppointmentId,
                PatientName = l.Appointment != null && l.Appointment.Patient != null 
                    ? l.Appointment.Patient.FirstName + " " + l.Appointment.Patient.LastName 
                    : "Unknown",
                AppointmentDate = l.Appointment?.AppointmentDate,
                OrderDate = l.OrderDate,
                TestName = l.TestName,
                Status = l.Status,
                Results = l.Results
            };
        }

        public async Task<LabOrderDto> CreateLabOrderAsync(LabOrderDto dto)
        {
            var labOrder = new LabOrder
            {
                AppointmentId = dto.AppointmentId,
                OrderDate = dto.OrderDate,
                TestName = dto.TestName,
                Status = dto.Status,
                Results = dto.Results
            };

            _context.LabOrders.Add(labOrder);
            await _context.SaveChangesAsync();
            
            dto.Id = labOrder.Id;
            return dto;
        }

        public async Task UpdateLabOrderAsync(LabOrderDto dto)
        {
            var l = await _context.LabOrders.FindAsync(dto.Id);
            if (l != null)
            {
                l.AppointmentId = dto.AppointmentId;
                l.OrderDate = dto.OrderDate;
                l.TestName = dto.TestName;
                l.Status = dto.Status;
                l.Results = dto.Results;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteLabOrderAsync(int id)
        {
            var labOrder = await _context.LabOrders.FindAsync(id);
            if (labOrder != null)
            {
                // New Business Rule:
                // 1. Pending orders CANNOT be deleted (must be processed).
                // 2. Completed orders CAN be deleted (to clean up reports).
                
                if (labOrder.Status == "Pending")
                {
                    throw new InvalidOperationException("Pending Lab Orders cannot be deleted. Please complete or cancel them first.");
                }

                _context.LabOrders.Remove(labOrder);
                await _context.SaveChangesAsync();
            }
        }
    }
}
