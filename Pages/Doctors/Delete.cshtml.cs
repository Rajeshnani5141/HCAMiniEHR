using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HCAMiniEHR.Services;
using HCAMiniEHR.Services.Dtos;

namespace HCAMiniEHR.Pages.Doctors
{
    public class DeleteModel : PageModel
    {
        private readonly IDoctorService _doctorService;

        public DeleteModel(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [BindProperty]
        public DoctorDto Doctor { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            var doctor = await _doctorService.GetDoctorByIdAsync(id.Value);
            if (doctor == null) return NotFound();

            Doctor = doctor;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                await _doctorService.DeleteDoctorAsync(id.Value);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                Doctor = await _doctorService.GetDoctorByIdAsync(id.Value) ?? new DoctorDto();
                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                Doctor = await _doctorService.GetDoctorByIdAsync(id.Value) ?? new DoctorDto();
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
