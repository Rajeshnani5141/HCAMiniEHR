using Microsoft.EntityFrameworkCore;
using HCAMiniEHR.Models;

namespace HCAMiniEHR.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<LabOrder> LabOrders { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Doctor> Doctors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure schema
            modelBuilder.HasDefaultSchema("Healthcare");

            // Patient configuration
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.ToTable("Patient");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(p => p.LastName).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Email).IsRequired().HasMaxLength(200);
                entity.Property(p => p.PhoneNumber).HasMaxLength(20);
            });

            // Appointment configuration
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.ToTable("Appointment");
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Reason).IsRequired().HasMaxLength(500);
                entity.Property(a => a.DoctorName).IsRequired().HasMaxLength(200);
                entity.Property(a => a.Status).HasMaxLength(50);

                entity.HasOne(a => a.Patient)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(a => a.PatientId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(a => a.Doctor)
                    .WithMany(d => d.Appointments)
                    .HasForeignKey(a => a.DoctorId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // LabOrder configuration
            modelBuilder.Entity<LabOrder>(entity =>
            {
                entity.ToTable("LabOrder");
                entity.HasKey(l => l.Id);
                entity.Property(l => l.TestName).IsRequired().HasMaxLength(200);
                entity.Property(l => l.Status).HasMaxLength(50);
                entity.Property(l => l.Results).HasMaxLength(1000);

                entity.HasOne(l => l.Appointment)
                    .WithMany(a => a.LabOrders)
                    .HasForeignKey(l => l.AppointmentId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // AuditLog configuration
            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.ToTable("AuditLog");
                entity.HasKey(a => a.Id);
                entity.Property(a => a.TableName).IsRequired().HasMaxLength(100);
                entity.Property(a => a.Operation).IsRequired().HasMaxLength(50);
            });

            // Doctor configuration
            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.ToTable("Doctor");
                entity.HasKey(d => d.Id);
                entity.Property(d => d.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(d => d.LastName).IsRequired().HasMaxLength(200);
                entity.Property(d => d.Specialization).IsRequired().HasMaxLength(200);
                entity.Property(d => d.Email).HasMaxLength(200);
                entity.Property(d => d.PhoneNumber).HasMaxLength(20);
            });

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Patients
            modelBuilder.Entity<Patient>().HasData(
                new Patient { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1980, 5, 15), Email = "john.doe@email.com", PhoneNumber = "555-0101" },
                new Patient { Id = 2, FirstName = "Jane", LastName = "Smith", DateOfBirth = new DateTime(1992, 8, 22), Email = "jane.smith@email.com", PhoneNumber = "555-0102" },
                new Patient { Id = 3, FirstName = "Michael", LastName = "Johnson", DateOfBirth = new DateTime(1975, 3, 10), Email = "michael.j@email.com", PhoneNumber = "555-0103" },
                new Patient { Id = 4, FirstName = "Emily", LastName = "Williams", DateOfBirth = new DateTime(1988, 11, 5), Email = "emily.w@email.com", PhoneNumber = "555-0104" },
                new Patient { Id = 5, FirstName = "David", LastName = "Brown", DateOfBirth = new DateTime(1995, 7, 18), Email = "david.b@email.com", PhoneNumber = "555-0105" },
                new Patient { Id = 6, FirstName = "Sarah", LastName = "Davis", DateOfBirth = new DateTime(1983, 2, 28), Email = "sarah.d@email.com", PhoneNumber = "555-0106" },
                new Patient { Id = 7, FirstName = "Robert", LastName = "Miller", DateOfBirth = new DateTime(1970, 9, 12), Email = "robert.m@email.com", PhoneNumber = "555-0107" },
                new Patient { Id = 8, FirstName = "Lisa", LastName = "Wilson", DateOfBirth = new DateTime(1990, 4, 25), Email = "lisa.w@email.com", PhoneNumber = "555-0108" },
                new Patient { Id = 9, FirstName = "James", LastName = "Moore", DateOfBirth = new DateTime(1985, 12, 8), Email = "james.m@email.com", PhoneNumber = "555-0109" },
                new Patient { Id = 10, FirstName = "Maria", LastName = "Garcia", DateOfBirth = new DateTime(1978, 6, 30), Email = "maria.g@email.com", PhoneNumber = "555-0110" }
            );

            // Seed Doctors
            modelBuilder.Entity<Doctor>().HasData(
                new Doctor { Id = 1, FirstName = "Sarah", LastName = "Anderson", Specialization = "Family Medicine", Email = "s.anderson@hospital.com", PhoneNumber = "555-1001", LicenseNumber = "MD-12345" },
                new Doctor { Id = 2, FirstName = "Carlos", LastName = "Martinez", Specialization = "Cardiology", Email = "c.martinez@hospital.com", PhoneNumber = "555-1002", LicenseNumber = "MD-12346" },
                new Doctor { Id = 3, FirstName = "Jennifer", LastName = "Thompson", Specialization = "Internal Medicine", Email = "j.thompson@hospital.com", PhoneNumber = "555-1003", LicenseNumber = "MD-12347" },
                new Doctor { Id = 4, FirstName = "David", LastName = "Lee", Specialization = "Endocrinology", Email = "d.lee@hospital.com", PhoneNumber = "555-1004", LicenseNumber = "MD-12348" }
            );

            // Seed Appointments - using static dates
            modelBuilder.Entity<Appointment>().HasData(
                new Appointment { Id = 1, PatientId = 1, DoctorId = 1, AppointmentDate = new DateTime(2026, 1, 20), Reason = "Annual checkup", DoctorName = "Dr. Anderson", Status = "Scheduled" },
                new Appointment { Id = 2, PatientId = 2, DoctorId = 2, AppointmentDate = new DateTime(2026, 1, 8), Reason = "Follow-up consultation", DoctorName = "Dr. Martinez", Status = "Completed" },
                new Appointment { Id = 3, PatientId = 3, DoctorId = 3, AppointmentDate = new DateTime(2026, 1, 16), Reason = "Blood pressure monitoring", DoctorName = "Dr. Thompson", Status = "Scheduled" },
                new Appointment { Id = 4, PatientId = 4, DoctorId = 4, AppointmentDate = new DateTime(2026, 1, 3), Reason = "Diabetes management", DoctorName = "Dr. Lee", Status = "Completed" },
                new Appointment { Id = 5, PatientId = 5, DoctorId = 1, AppointmentDate = new DateTime(2026, 1, 27), Reason = "Vaccination", DoctorName = "Dr. Anderson", Status = "Scheduled" }
            );

            // Seed LabOrders
            modelBuilder.Entity<LabOrder>().HasData(
                new LabOrder { Id = 1, AppointmentId = 1, OrderDate = new DateTime(2026, 1, 12), TestName = "Complete Blood Count (CBC)", Status = "Pending" },
                new LabOrder { Id = 2, AppointmentId = 2, OrderDate = new DateTime(2026, 1, 8), TestName = "Lipid Panel", Status = "Completed", Results = "Normal cholesterol levels" },
                new LabOrder { Id = 3, AppointmentId = 3, OrderDate = new DateTime(2026, 1, 12), TestName = "Blood Pressure Test", Status = "Pending" },
                new LabOrder { Id = 4, AppointmentId = 4, OrderDate = new DateTime(2026, 1, 3), TestName = "HbA1c Test", Status = "Completed", Results = "6.2% - Good control" },
                new LabOrder { Id = 5, AppointmentId = 5, OrderDate = new DateTime(2026, 1, 12), TestName = "Antibody Titer", Status = "Pending" }
            );
        }
    }
}
