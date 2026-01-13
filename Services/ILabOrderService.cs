using HCAMiniEHR.Services.Dtos;

namespace HCAMiniEHR.Services
{
    public interface ILabOrderService
    {
        Task<IEnumerable<LabOrderDto>> GetAllLabOrdersAsync();
        Task<IEnumerable<LabOrderDto>> GetLabOrdersByAppointmentIdAsync(int appointmentId);
        Task<LabOrderDto?> GetLabOrderByIdAsync(int id);
        Task<LabOrderDto> CreateLabOrderAsync(LabOrderDto labOrderDto);
        Task UpdateLabOrderAsync(LabOrderDto labOrderDto);
        Task DeleteLabOrderAsync(int id);
    }
}
