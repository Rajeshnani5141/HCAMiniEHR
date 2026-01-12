namespace HCAMiniEHR.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string DoctorName { get; set; } = string.Empty;
        public string Status { get; set; } = "Scheduled"; // Scheduled, Completed, Cancelled
        
        // Navigation properties
        public Patient Patient { get; set; } = null!;
        public ICollection<LabOrder> LabOrders { get; set; } = new List<LabOrder>();
    }
}
