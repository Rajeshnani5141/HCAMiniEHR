using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HCAMiniEHR.Data;
using HCAMiniEHR.Models;

namespace HCAMiniEHR.Pages.Admin
{
    public class AuditLogsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AuditLogsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<AuditLog> AuditLogs { get; set; } = default!;

        public async Task OnGetAsync()
        {
            try 
            {
                AuditLogs = await _context.AuditLogs
                    .OrderByDescending(a => a.ChangedAt)
                    .Take(50)
                    .ToListAsync();
            }
            catch
            {
                // Table might be deleted. Return empty list so page renders and Restore button works.
                AuditLogs = new List<AuditLog>();
                TempData["ErrorMessage"] = "CRITICAL: AuditLog table not found! Click 'Emergency Repair' immediately.";
            }
        }

        public async Task<IActionResult> OnPostTestProtectionAsync()
        {
            try
            {
                // Attempt to delete the most recent log to trigger the security block
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM Healthcare.AuditLog WHERE Id = (SELECT TOP 1 Id FROM Healthcare.AuditLog)");
                
                TempData["ErrorMessage"] = "CRITICAL FAIL: The record was deleted! The security trigger is NOT working.";
            }
            catch (Exception ex)
            {
                // This is EXPECTED! The trigger should throw an error.
                if (ex.Message.Contains("Invalid object name 'Healthcare.AuditLog'")) 
                {
                     TempData["ErrorMessage"] = "ERROR: The AuditLog table is missing! Please use the Restore button.";
                }
                else 
                {
                    TempData["SuccessMessage"] = $"SECURITY VERIFIED: logic blocked the deletion. Database said: {ex.InnerException?.Message ?? ex.Message}";
                }
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRestoreAsync()
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(@"
                    IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Healthcare].[AuditLog]') AND type in (N'U'))
                    BEGIN
                        CREATE TABLE [Healthcare].[AuditLog](
                            [Id] [int] IDENTITY(1,1) NOT NULL,
                            [TableName] [nvarchar](100) NOT NULL,
                            [Operation] [nvarchar](50) NOT NULL,
                            [RecordId] [int] NOT NULL,
                            [OldValue] [nvarchar](max) NULL,
                            [NewValue] [nvarchar](max) NULL,
                            [ChangedAt] [datetime2](7) NOT NULL,
                            [ChangedBy] [nvarchar](max) NOT NULL,
                            CONSTRAINT [PK_AuditLog] PRIMARY KEY CLUSTERED ([Id] ASC)
                        )
                    END
                ");

                await _context.Database.ExecuteSqlRawAsync(@"
                    CREATE OR ALTER TRIGGER [Healthcare].[trg_PreventAuditLogDelete]
                    ON [Healthcare].[AuditLog]
                    INSTEAD OF DELETE
                    AS
                    BEGIN
                        RAISERROR ('Security Alert: Access Denied. You cannot delete records from the AuditLog table. This action has been blocked.', 16, 1);
                    END
                ");

                TempData["SuccessMessage"] = "SYSTEM RESTORED: AuditLog table and Security Triggers have been recreated.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Restore Failed: " + ex.Message;
            }
            return RedirectToPage();
        }
    }
}
