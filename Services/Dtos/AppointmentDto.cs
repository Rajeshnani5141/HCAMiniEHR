using System.ComponentModel.DataAnnotations;
using HCAMiniEHR.Validation;

namespace HCAMiniEHR.Services.Dtos
{
    public class AppointmentDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Patient is required")]
        [Display(Name = "Patient")]
        public int PatientId { get; set; }
        
        public string? PatientName { get; set; }

        [Display(Name = "Doctor")]
        public int? DoctorId { get; set; }
        
        [Display(Name = "Doctor Name")]
        public string? DoctorName { get; set; }

        [Required(ErrorMessage = "Appointment date is required")]
        [Display(Name = "Appointment Date")]
        [FutureDate(ErrorMessage = "Appointment date must be in the future")]
        public DateTime AppointmentDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Reason is required")]
        [StringLength(500, ErrorMessage = "Reason cannot exceed 500 characters")]
        public string Reason { get; set; } = string.Empty;

        public string Status { get; set; } = "Scheduled";

        public ICollection<LabOrderDto> LabOrders { get; set; } = new List<LabOrderDto>();
    }
}
