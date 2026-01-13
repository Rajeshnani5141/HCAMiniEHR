using HCAMiniEHR.Models;
using HCAMiniEHR.Data;
using Microsoft.EntityFrameworkCore;
using HCAMiniEHR.Services.Dtos;

namespace HCAMiniEHR.Services
{
    public class PatientService : IPatientService
    {
        private readonly ApplicationDbContext _context;

        public PatientService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PatientDto>> GetAllPatientsAsync()
        {
            return await _context.Patients
                .OrderBy(p => p.LastName)
                .ThenBy(p => p.FirstName)
                .Select(p => new PatientDto
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    DateOfBirth = p.DateOfBirth,
                    Email = p.Email,
                    PhoneNumber = p.PhoneNumber
                })
                .ToListAsync();
        }

        public async Task<PatientDto?> GetPatientByIdAsync(int id)
        {
            var patient = await _context.Patients
                .Include(p => p.Appointments)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (patient == null) return null;

            var dto = new PatientDto
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                DateOfBirth = patient.DateOfBirth,
                Email = patient.Email,
                PhoneNumber = patient.PhoneNumber
            };

            if (patient.Appointments != null)
            {
                dto.Appointments = patient.Appointments.Select(a => new AppointmentDto
                {
                    Id = a.Id,
                    PatientId = a.PatientId,
                    DoctorId = a.DoctorId,
                    DoctorName = a.DoctorName,
                    AppointmentDate = a.AppointmentDate,
                    Reason = a.Reason,
                    Status = a.Status
                }).ToList();
            }

            return dto;
        }

        public async Task<PatientDto> CreatePatientAsync(PatientDto patientDto)
        {
            // Validation: Check for duplicate patient
            var exists = await _context.Patients.AnyAsync(p => 
                p.FirstName == patientDto.FirstName && 
                p.LastName == patientDto.LastName && 
                p.PhoneNumber == patientDto.PhoneNumber);

            if (exists)
            {
                throw new InvalidOperationException($"A patient named {patientDto.FirstName} {patientDto.LastName} with this phone number already exists.");
            }

            var patient = new Patient
            {
                FirstName = patientDto.FirstName,
                LastName = patientDto.LastName,
                DateOfBirth = patientDto.DateOfBirth,
                Email = patientDto.Email,
                PhoneNumber = patientDto.PhoneNumber
            };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            
            patientDto.Id = patient.Id;
            return patientDto;
        }

        public async Task UpdatePatientAsync(PatientDto patientDto)
        {
            var patient = await _context.Patients.FindAsync(patientDto.Id);
            if (patient != null)
            {
                patient.FirstName = patientDto.FirstName;
                patient.LastName = patientDto.LastName;
                patient.DateOfBirth = patientDto.DateOfBirth;
                patient.Email = patientDto.Email;
                patient.PhoneNumber = patientDto.PhoneNumber;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeletePatientAsync(int id)
        {
            var patient = await _context.Patients
                .Include(p => p.Appointments)
                .FirstOrDefaultAsync(p => p.Id == id);
            
            if (patient != null)
            {
                if (patient.Appointments.Any())
                {
                    throw new InvalidOperationException("Cannot delete patient because they have existing appointments.");
                }

                _context.Patients.Remove(patient);
                await _context.SaveChangesAsync();
            }
        }
    }
}
