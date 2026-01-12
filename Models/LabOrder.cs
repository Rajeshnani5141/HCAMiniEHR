using System.ComponentModel.DataAnnotations;

namespace HCAMiniEHR.Models
{
    public class LabOrder
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Appointment is required")]
        [Display(Name = "Appointment")]
        public int AppointmentId { get; set; }
        
        [Required(ErrorMessage = "Order date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }
        
        [Required(ErrorMessage = "Test name is required")]
        [StringLength(200, ErrorMessage = "Test name cannot exceed 200 characters")]
        [Display(Name = "Test Name")]
        public string TestName { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending"; // Pending, Completed, Cancelled
        public string? Results { get; set; }
        
        // Navigation property
        public Appointment Appointment { get; set; } = null!;
    }
}
