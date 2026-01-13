using System.ComponentModel.DataAnnotations;
using HCAMiniEHR.Validation;

namespace HCAMiniEHR.Services.Dtos
{
    public class LabOrderDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Appointment is required")]
        [Display(Name = "Appointment")]
        public int AppointmentId { get; set; }
        
        public string? PatientName { get; set; }
        
        [Display(Name = "Appointment Date")]
        public DateTime? AppointmentDate { get; set; }

        [Required(ErrorMessage = "Order date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Order Date")]
        [FutureDate(ErrorMessage = "Order date must be in the future")]
        public DateTime OrderDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Test name is required")]
        [StringLength(200, ErrorMessage = "Test name cannot exceed 200 characters")]
        [Display(Name = "Test Name")]
        public string TestName { get; set; } = string.Empty;

        public string Status { get; set; } = "Pending";
        public string? Results { get; set; }
    }
}
