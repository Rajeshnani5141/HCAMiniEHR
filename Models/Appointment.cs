using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using HCAMiniEHR.Validation;

namespace HCAMiniEHR.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Patient is required")]
        [Display(Name = "Patient")]
        public int PatientId { get; set; }
        
        public int? DoctorId { get; set; }
        
        [Required(ErrorMessage = "Appointment date is required")]
        [Display(Name = "Appointment Date")]
        [FutureDate(ErrorMessage = "Appointment date must be in the future")]
        public DateTime AppointmentDate { get; set; }
        
        [Required(ErrorMessage = "Reason is required")]
        [StringLength(500, ErrorMessage = "Reason cannot exceed 500 characters")]
        public string Reason { get; set; } = string.Empty;
        
        [StringLength(200)]
        [Display(Name = "Doctor Name")]
        public string DoctorName { get; set; } = string.Empty;

        public string Status { get; set; } = "Scheduled"; // Scheduled, Completed, Cancelled
        
        // Navigation properties
        public Patient Patient { get; set; } = null!;
        public Doctor? Doctor { get; set; }
        public ICollection<LabOrder> LabOrders { get; set; } = new List<LabOrder>();
    }
}
