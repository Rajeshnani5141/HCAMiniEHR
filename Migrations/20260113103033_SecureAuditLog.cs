using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HCAMiniEHR.Migrations
{
    /// <inheritdoc />
    public partial class SecureAuditLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE TRIGGER [Healthcare].[trg_PreventAuditLogDelete]
                ON [Healthcare].[AuditLog]
                INSTEAD OF DELETE
                AS
                BEGIN
                    RAISERROR ('Security Alert: Access Denied. You cannot delete records from the AuditLog table. This action has been blocked.', 16, 1);
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS [Healthcare].[trg_PreventAuditLogDelete]");
        }
    }
}
