using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HCAMiniEHR.Pages;
public class IndexModel : PageModel
{
    private readonly HCAMiniEHR.Data.ApplicationDbContext _context;

    public IndexModel(HCAMiniEHR.Data.ApplicationDbContext context)
    {
        _context = context;
    }

    public int PatientCount { get; set; }
    public int AppointmentsToday { get; set; }
    public int DoctorsCount { get; set; }
    public int PendingLabs { get; set; }

    public void OnGet()
    {
        var today = DateTime.Today;
        
        PatientCount = _context.Patients.Count();
        AppointmentsToday = _context.Appointments.Count(a => a.AppointmentDate.Date == today);
        DoctorsCount = _context.Doctors.Count();
        PendingLabs = _context.LabOrders.Count(l => l.Status == "Pending");
    }
}
