using Microsoft.AspNetCore.Mvc.RazorPages;
using HCAMiniEHR.Services;
using HCAMiniEHR.Services.Dtos;

namespace HCAMiniEHR.Pages.LabOrders
{
    public class IndexModel : PageModel
    {
        private readonly ILabOrderService _labOrderService;

        public IndexModel(ILabOrderService labOrderService)
        {
            _labOrderService = labOrderService;
        }

        public IEnumerable<LabOrderDto> LabOrders { get; set; } = new List<LabOrderDto>();

        public async Task OnGetAsync()
        {
            LabOrders = await _labOrderService.GetAllLabOrdersAsync();
        }
    }
}
