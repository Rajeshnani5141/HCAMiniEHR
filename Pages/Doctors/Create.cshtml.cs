using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HCAMiniEHR.Services;
using HCAMiniEHR.Services.Dtos;

namespace HCAMiniEHR.Pages.Doctors
{
    public class CreateModel : PageModel
    {
        private readonly IDoctorService _doctorService;

        public CreateModel(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [BindProperty]
        public DoctorDto Doctor { get; set; } = new DoctorDto();

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _doctorService.CreateDoctorAsync(Doctor);
            return RedirectToPage("./Index");
        }
    }
}
