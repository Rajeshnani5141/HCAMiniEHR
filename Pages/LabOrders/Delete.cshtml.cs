using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HCAMiniEHR.Models;
using HCAMiniEHR.Services;

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
        public LabOrder LabOrder { get; set; } = default!;

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

            await _labOrderService.DeleteLabOrderAsync(id.Value);

            return RedirectToPage("./Index");
        }
    }
}
