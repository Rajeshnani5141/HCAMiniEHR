using HCAMiniEHR.Models;

namespace HCAMiniEHR.Services
{
    public interface ILabOrderService
    {
        Task<IEnumerable<LabOrder>> GetAllLabOrdersAsync();
        Task<IEnumerable<LabOrder>> GetLabOrdersByAppointmentIdAsync(int appointmentId);
        Task<LabOrder?> GetLabOrderByIdAsync(int id);
        Task<LabOrder> CreateLabOrderAsync(LabOrder labOrder);
        Task UpdateLabOrderAsync(LabOrder labOrder);
        Task DeleteLabOrderAsync(int id);
    }
}
