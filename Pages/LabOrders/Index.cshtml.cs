using Microsoft.AspNetCore.Mvc.RazorPages;
using HCAMiniEHR.Models;
using HCAMiniEHR.Services;

namespace HCAMiniEHR.Pages.LabOrders
{
    public class IndexModel : PageModel
    {
        private readonly ILabOrderService _labOrderService;

        public IndexModel(ILabOrderService labOrderService)
        {
            _labOrderService = labOrderService;
        }

        public IEnumerable<LabOrder> LabOrders { get; set; } = new List<LabOrder>();

        public async Task OnGetAsync()
        {
            LabOrders = await _labOrderService.GetAllLabOrdersAsync();
        }
    }
}
