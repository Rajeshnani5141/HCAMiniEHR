using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HCAMiniEHR.Migrations
{
    /// <inheritdoc />
    public partial class AddDoctorTableAndValidations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DoctorId",
                schema: "Healthcare",
                table: "Appointment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Doctor",
                schema: "Healthcare",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Specialization = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LicenseNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctor", x => x.Id);
                });

            migrationBuilder.UpdateData(
                schema: "Healthcare",
                table: "Appointment",
                keyColumn: "Id",
                keyValue: 1,
                column: "DoctorId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Healthcare",
                table: "Appointment",
                keyColumn: "Id",
                keyValue: 2,
                column: "DoctorId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Healthcare",
                table: "Appointment",
                keyColumn: "Id",
                keyValue: 3,
                column: "DoctorId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Healthcare",
                table: "Appointment",
                keyColumn: "Id",
                keyValue: 4,
                column: "DoctorId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Healthcare",
                table: "Appointment",
                keyColumn: "Id",
                keyValue: 5,
                column: "DoctorId",
                value: 1);

            migrationBuilder.InsertData(
                schema: "Healthcare",
                table: "Doctor",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "LicenseNumber", "PhoneNumber", "Specialization" },
                values: new object[,]
                {
                    { 1, "s.anderson@hospital.com", "Sarah", "Anderson", "MD-12345", "555-1001", "Family Medicine" },
                    { 2, "c.martinez@hospital.com", "Carlos", "Martinez", "MD-12346", "555-1002", "Cardiology" },
                    { 3, "j.thompson@hospital.com", "Jennifer", "Thompson", "MD-12347", "555-1003", "Internal Medicine" },
                    { 4, "d.lee@hospital.com", "David", "Lee", "MD-12348", "555-1004", "Endocrinology" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_DoctorId",
                schema: "Healthcare",
                table: "Appointment",
                column: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Doctor_DoctorId",
                schema: "Healthcare",
                table: "Appointment",
                column: "DoctorId",
                principalSchema: "Healthcare",
                principalTable: "Doctor",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Doctor_DoctorId",
                schema: "Healthcare",
                table: "Appointment");

            migrationBuilder.DropTable(
                name: "Doctor",
                schema: "Healthcare");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_DoctorId",
                schema: "Healthcare",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                schema: "Healthcare",
                table: "Appointment");
        }
    }
}
