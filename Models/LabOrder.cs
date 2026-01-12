namespace HCAMiniEHR.Models
{
    public class LabOrder
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public DateTime OrderDate { get; set; }
        public string TestName { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending"; // Pending, Completed, Cancelled
        public string? Results { get; set; }
        
        // Navigation property
        public Appointment Appointment { get; set; } = null!;
    }
}
