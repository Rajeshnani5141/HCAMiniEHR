using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HCAMiniEHR.Services;
using HCAMiniEHR.Services.Dtos;

namespace HCAMiniEHR.Pages.Doctors
{
    public class EditModel : PageModel
    {
        private readonly IDoctorService _doctorService;

        public EditModel(IDoctorService doctorService)
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            try
            {
                await _doctorService.UpdateDoctorAsync(Doctor);
            }
            catch (Exception)
            {
                if (!await DoctorExists(Doctor.Id)) return NotFound();
                else throw;
            }

            return RedirectToPage("./Index");
        }

        private async Task<bool> DoctorExists(int id)
        {
            var d = await _doctorService.GetDoctorByIdAsync(id);
            return d != null;
        }
    }
}
