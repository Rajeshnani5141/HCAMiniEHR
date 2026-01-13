using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HCAMiniEHR.Services;
using HCAMiniEHR.Services.Dtos;

namespace HCAMiniEHR.Pages.LabOrders
{
    public class DetailsModel : PageModel
    {
        private readonly ILabOrderService _labOrderService;

        public DetailsModel(ILabOrderService labOrderService)
        {
            _labOrderService = labOrderService;
        }

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
    }
}
