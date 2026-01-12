namespace HCAMiniEHR.Services
{
    public class ReportDto
    {
        // For Pending Lab Orders Report
        public int LabOrderId { get; set; }
        public string TestName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; } = string.Empty;

        // For Patients Without Follow-Up Report
        public int PatientId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime? LastAppointmentDate { get; set; }

        // For Doctor Productivity Report
        public string DoctorName { get; set; } = string.Empty;
        public int TotalAppointments { get; set; }
        public int CompletedAppointments { get; set; }
        public int ScheduledAppointments { get; set; }
    }
}
