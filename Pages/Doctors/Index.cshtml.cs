using Microsoft.AspNetCore.Mvc.RazorPages;
using HCAMiniEHR.Services;
using HCAMiniEHR.Services.Dtos;

namespace HCAMiniEHR.Pages.Doctors
{
    public class IndexModel : PageModel
    {
        private readonly IDoctorService _doctorService;

        public IndexModel(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        public IEnumerable<DoctorDto> Doctors { get; set; } = new List<DoctorDto>();

        public async Task OnGetAsync()
        {
            Doctors = await _doctorService.GetAllDoctorsAsync();
        }
    }
}
