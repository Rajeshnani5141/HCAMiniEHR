using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HCAMiniEHR.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Healthcare");

            migrationBuilder.CreateTable(
                name: "AuditLog",
                schema: "Healthcare",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TableName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Operation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RecordId = table.Column<int>(type: "int", nullable: false),
                    OldValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChangedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patient",
                schema: "Healthcare",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Appointment",
                schema: "Healthcare",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    AppointmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DoctorName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointment_Patient_PatientId",
                        column: x => x.PatientId,
                        principalSchema: "Healthcare",
                        principalTable: "Patient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LabOrder",
                schema: "Healthcare",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentId = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TestName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Results = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LabOrder_Appointment_AppointmentId",
                        column: x => x.AppointmentId,
                        principalSchema: "Healthcare",
                        principalTable: "Appointment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "Healthcare",
                table: "Patient",
                columns: new[] { "Id", "DateOfBirth", "Email", "FirstName", "LastName", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, new DateTime(1980, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "john.doe@email.com", "John", "Doe", "555-0101" },
                    { 2, new DateTime(1992, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "jane.smith@email.com", "Jane", "Smith", "555-0102" },
                    { 3, new DateTime(1975, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "michael.j@email.com", "Michael", "Johnson", "555-0103" },
                    { 4, new DateTime(1988, 11, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "emily.w@email.com", "Emily", "Williams", "555-0104" },
                    { 5, new DateTime(1995, 7, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "david.b@email.com", "David", "Brown", "555-0105" },
                    { 6, new DateTime(1983, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "sarah.d@email.com", "Sarah", "Davis", "555-0106" },
                    { 7, new DateTime(1970, 9, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "robert.m@email.com", "Robert", "Miller", "555-0107" },
                    { 8, new DateTime(1990, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "lisa.w@email.com", "Lisa", "Wilson", "555-0108" },
                    { 9, new DateTime(1985, 12, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "james.m@email.com", "James", "Moore", "555-0109" },
                    { 10, new DateTime(1978, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "maria.g@email.com", "Maria", "Garcia", "555-0110" }
                });

            migrationBuilder.InsertData(
                schema: "Healthcare",
                table: "Appointment",
                columns: new[] { "Id", "AppointmentDate", "DoctorName", "PatientId", "Reason", "Status" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dr. Anderson", 1, "Annual checkup", "Scheduled" },
                    { 2, new DateTime(2026, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dr. Martinez", 2, "Follow-up consultation", "Completed" },
                    { 3, new DateTime(2026, 1, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dr. Thompson", 3, "Blood pressure monitoring", "Scheduled" },
                    { 4, new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dr. Lee", 4, "Diabetes management", "Completed" },
                    { 5, new DateTime(2026, 1, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dr. Anderson", 5, "Vaccination", "Scheduled" }
                });

            migrationBuilder.InsertData(
                schema: "Healthcare",
                table: "LabOrder",
                columns: new[] { "Id", "AppointmentId", "OrderDate", "Results", "Status", "TestName" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Pending", "Complete Blood Count (CBC)" },
                    { 2, 2, new DateTime(2026, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Normal cholesterol levels", "Completed", "Lipid Panel" },
                    { 3, 3, new DateTime(2026, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Pending", "Blood Pressure Test" },
                    { 4, 4, new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "6.2% - Good control", "Completed", "HbA1c Test" },
                    { 5, 5, new DateTime(2026, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Pending", "Antibody Titer" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_PatientId",
                schema: "Healthcare",
                table: "Appointment",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_LabOrder_AppointmentId",
                schema: "Healthcare",
                table: "LabOrder",
                column: "AppointmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLog",
                schema: "Healthcare");

            migrationBuilder.DropTable(
                name: "LabOrder",
                schema: "Healthcare");

            migrationBuilder.DropTable(
                name: "Appointment",
                schema: "Healthcare");

            migrationBuilder.DropTable(
                name: "Patient",
                schema: "Healthcare");
        }
    }
}
