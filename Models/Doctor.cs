using System.ComponentModel.DataAnnotations;

namespace HCAMiniEHR.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "First name is required")]
        [StringLength(100)]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "First name can only contain letters")]
        public string FirstName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Last name is required")]
        [StringLength(100)]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Last name can only contain letters")]
        public string LastName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Specialization is required")]
        [StringLength(200)]
        public string Specialization { get; set; } = string.Empty;
        
        [EmailAddress]
        [StringLength(200)]
        public string Email { get; set; } = string.Empty;
        
        [Phone]
        [StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;
        
        public string LicenseNumber { get; set; } = string.Empty;
        
        // Navigation property
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
