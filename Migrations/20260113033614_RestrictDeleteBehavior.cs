using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HCAMiniEHR.Migrations
{
    /// <inheritdoc />
    public partial class RestrictDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Patient_PatientId",
                schema: "Healthcare",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_LabOrder_Appointment_AppointmentId",
                schema: "Healthcare",
                table: "LabOrder");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Patient_PatientId",
                schema: "Healthcare",
                table: "Appointment",
                column: "PatientId",
                principalSchema: "Healthcare",
                principalTable: "Patient",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LabOrder_Appointment_AppointmentId",
                schema: "Healthcare",
                table: "LabOrder",
                column: "AppointmentId",
                principalSchema: "Healthcare",
                principalTable: "Appointment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Patient_PatientId",
                schema: "Healthcare",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_LabOrder_Appointment_AppointmentId",
                schema: "Healthcare",
                table: "LabOrder");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Patient_PatientId",
                schema: "Healthcare",
                table: "Appointment",
                column: "PatientId",
                principalSchema: "Healthcare",
                principalTable: "Patient",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LabOrder_Appointment_AppointmentId",
                schema: "Healthcare",
                table: "LabOrder",
                column: "AppointmentId",
                principalSchema: "Healthcare",
                principalTable: "Appointment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
