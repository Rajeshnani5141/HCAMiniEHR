using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HCAMiniEHR.Services;
using HCAMiniEHR.Services.Dtos;

namespace HCAMiniEHR.Pages.LabOrders
{
    public class DeleteModel : PageModel
    {
        private readonly ILabOrderService _labOrderService;

        public DeleteModel(ILabOrderService labOrderService)
        {
            _labOrderService = labOrderService;
        }

        [BindProperty]
        public LabOrderDto LabOrder { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var laborder = await _labOrderService.GetLabOrderByIdAsync(id.Value);

            if (laborder == null)
            {
                return NotFound();
            }
            LabOrder = laborder;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var labOrder = await _labOrderService.GetLabOrderByIdAsync(id.Value);
            if (labOrder == null)
            {
                return NotFound();
            }

            // Removed old check that blocked Completed orders. 
            // The logic is now centralized in LabOrderService.DeleteLabOrderAsync
            // which BLOCKS Pending and ALLOWS Completed.

            try
            {
                await _labOrderService.DeleteLabOrderAsync(id.Value);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                LabOrder = await _labOrderService.GetLabOrderByIdAsync(id.Value) ?? new LabOrderDto();
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
