using HCAMiniEHR.Services.Dtos;

namespace HCAMiniEHR.Services
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientDto>> GetAllPatientsAsync();
        Task<PatientDto?> GetPatientByIdAsync(int id);
        Task<PatientDto> CreatePatientAsync(PatientDto patientDto);
        Task UpdatePatientAsync(PatientDto patientDto);
        Task DeletePatientAsync(int id);
    }
}
