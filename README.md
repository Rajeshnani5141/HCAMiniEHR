# HCAMiniEHR - Healthcare Management System

## Overview
HCAMiniEHR is a mini Electronic Health Records (EHR) system built with ASP.NET Core Razor Pages, Entity Framework Core, and SQL Server. This project demonstrates a complete end-to-end healthcare module managing patients, appointments, and laboratory orders.

## Tech Stack
- **Framework**: ASP.NET Core 10.0 (Razor Pages)
- **ORM**: Entity Framework Core 10.0.1
- **Database**: SQL Server (LocalDB)
- **Version Control**: Git

## Features

### Core Functionality
1. **Patient Management**: Full CRUD operations for patient records
2. **Appointment Scheduling**: Create and manage patient appointments
3. **Lab Order Tracking**: Track laboratory test orders and results
4. **Reporting**: Three LINQ-based reports with analytics

### Database Features
- **Healthcare Schema**: All tables organized under `[Healthcare]` schema
- **Audit Trigger**: Automatic logging of appointment changes to `AuditLog` table
- **Stored Procedure**: `sp_CreateAppointment` for creating appointments via SQL
- **Seed Data**: 10 patients, 5 appointments, 5 lab orders pre-populated

### Reports (LINQ Queries)
1. **Pending Lab Orders**: Shows all lab orders with "Pending" status
2. **Patients Without Follow-Up**: Identifies patients with no future appointments
3. **Doctor Productivity**: Groups appointments by doctor with completion statistics

## Design Decisions

### Architecture
- **Service Layer Pattern**: Separated business logic into service interfaces and implementations
- **Repository Pattern**: Services act as repositories for data access
- **Dependency Injection**: All services registered in `Program.cs` for loose coupling

### Data Model
- **One-to-Many Relationships**: 
  - Patient → Appointments (cascade delete)
  - Appointment → LabOrders (cascade delete)
- **Navigation Properties**: Configured for eager loading with `.Include()`
- **Audit Logging**: Separate `AuditLog` table for tracking changes

### UI/UX
- **Bootstrap 5**: Responsive design with cards, tables, and badges
- **Color-Coded Status**: Visual indicators for appointment and lab order statuses
- **Tabbed Reports**: Bootstrap tabs for easy navigation between reports
- **Dashboard**: Central hub with cards linking to all major sections

### Database Strategy
- **Code-First Migrations**: EF Core migrations for version control of schema
- **Static Seed Data**: Hardcoded dates to avoid migration conflicts
- **LocalDB**: Development database for easy setup without SQL Server installation

## Project Structure
```
HCAMiniEHR/
├── Data/
│   └── ApplicationDbContext.cs       # EF Core DbContext
├── Models/
│   ├── Patient.cs
│   ├── Appointment.cs
│   ├── LabOrder.cs
│   └── AuditLog.cs
├── Services/
│   ├── IPatientService.cs / PatientService.cs
│   ├── IAppointmentService.cs / AppointmentService.cs
│   ├── ILabOrderService.cs / LabOrderService.cs
│   ├── IReportService.cs / ReportService.cs
│   └── ReportDto.cs
├── Pages/
│   ├── Patients/                     # Patient CRUD pages
│   ├── Appointments/                 # Appointment CRUD pages
│   ├── LabOrders/                    # Lab Order CRUD pages
│   └── Reports/                      # Reporting pages
├── SQL/
│   └── Scripts.sql                   # Trigger and Stored Procedure
└── Migrations/                       # EF Core migrations
```

## Setup Instructions

### Prerequisites
- .NET 8 SDK or later
- SQL Server LocalDB (or SQL Server)

### Installation
1. Clone the repository
2. Navigate to project directory: `cd HCAMiniEHR`
3. Restore packages: `dotnet restore`
4. Update database: `dotnet ef database update`
5. Apply SQL scripts (trigger and SP) manually in SSMS or Azure Data Studio
6. Run the application: `dotnet run`

### Applying SQL Scripts
After running migrations, execute the scripts in `SQL/Scripts.sql` to create:
- Audit trigger on `[Healthcare].[Appointment]` table
- Stored procedure `[Healthcare].[sp_CreateAppointment]`

## Git Workflow
This project follows a feature branch workflow:
- `main`: Production-ready code
- `feature/patient-management`: Feature branch for patient-related enhancements

## Future Enhancements
- Add Doctor entity and assign doctors to appointments
- Implement DTOs for all data transfer
- Add comprehensive validation and error handling
- Create Edit and Delete pages for all entities
- Add authentication and authorization
- Deploy to Azure App Service

## License
Educational project for HCA Healthcare training program.
