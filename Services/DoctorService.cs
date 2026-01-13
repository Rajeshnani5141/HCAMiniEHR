using HCAMiniEHR.Models;
using HCAMiniEHR.Data;
using Microsoft.EntityFrameworkCore;
using HCAMiniEHR.Services.Dtos;

namespace HCAMiniEHR.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly ApplicationDbContext _context;

        public DoctorService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync()
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

        public async Task<DoctorDto?> GetDoctorByIdAsync(int id)
        {
            var d = await _context.Doctors.FindAsync(id);
            if (d == null) return null;

            return new DoctorDto
            {
                Id = d.Id,
                FirstName = d.FirstName,
                LastName = d.LastName,
                Specialization = d.Specialization
            };
        }

        public async Task<DoctorDto> CreateDoctorAsync(DoctorDto doctorDto)
        {
            var doctor = new Doctor
            {
                FirstName = doctorDto.FirstName,
                LastName = doctorDto.LastName,
                Specialization = doctorDto.Specialization
            };

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();
            
            doctorDto.Id = doctor.Id;
            return doctorDto;
        }

        public async Task UpdateDoctorAsync(DoctorDto doctorDto)
        {
            var d = await _context.Doctors.FindAsync(doctorDto.Id);
            if (d != null)
            {
                d.FirstName = doctorDto.FirstName;
                d.LastName = doctorDto.LastName;
                d.Specialization = doctorDto.Specialization;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteDoctorAsync(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor != null)
            {
                // Check for associated appointments
                var hasAppointments = await _context.Appointments.AnyAsync(a => a.DoctorId == id);
                if (hasAppointments)
                {
                    throw new InvalidOperationException("Cannot delete doctor because they have associated appointment records.");
                }

                _context.Doctors.Remove(doctor);
                await _context.SaveChangesAsync();
            }
        }
    }
}
