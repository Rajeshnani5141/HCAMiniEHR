using HCAMiniEHR.Models;
using HCAMiniEHR.Data;
using Microsoft.EntityFrameworkCore;

namespace HCAMiniEHR.Services
{
    public class LabOrderService : ILabOrderService
    {
        private readonly ApplicationDbContext _context;

        public LabOrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LabOrder>> GetAllLabOrdersAsync()
        {
            return await _context.LabOrders
                .Include(l => l.Appointment)
                .ThenInclude(a => a.Patient)
                .OrderByDescending(l => l.OrderDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<LabOrder>> GetLabOrdersByAppointmentIdAsync(int appointmentId)
        {
            return await _context.LabOrders
                .Include(l => l.Appointment)
                .Where(l => l.AppointmentId == appointmentId)
                .OrderByDescending(l => l.OrderDate)
                .ToListAsync();
        }

        public async Task<LabOrder?> GetLabOrderByIdAsync(int id)
        {
            return await _context.LabOrders
                .Include(l => l.Appointment)
                .ThenInclude(a => a.Patient)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<LabOrder> CreateLabOrderAsync(LabOrder labOrder)
        {
            _context.LabOrders.Add(labOrder);
            await _context.SaveChangesAsync();
            return labOrder;
        }

        public async Task UpdateLabOrderAsync(LabOrder labOrder)
        {
            _context.Entry(labOrder).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLabOrderAsync(int id)
        {
            var labOrder = await _context.LabOrders.FindAsync(id);
            if (labOrder != null)
            {
                _context.LabOrders.Remove(labOrder);
                await _context.SaveChangesAsync();
            }
        }
    }
}
