using HCAMiniEHR.Services.Dtos;

namespace HCAMiniEHR.Services
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync();
        Task<DoctorDto?> GetDoctorByIdAsync(int id);
        Task<DoctorDto> CreateDoctorAsync(DoctorDto doctorDto);
        Task UpdateDoctorAsync(DoctorDto doctorDto);
        Task DeleteDoctorAsync(int id);
    }
}
