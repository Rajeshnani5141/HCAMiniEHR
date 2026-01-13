
CREATE OR ALTER TRIGGER [Healthcare].[trg_Appointment_Audit]
ON [Healthcare].[Appointment]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [Healthcare].[AuditLog] (TableName, Operation, RecordId, OldValue, NewValue, ChangedAt, ChangedBy)
    SELECT 
        'Appointment',
        'UPDATE',
        i.Id,
        CONCAT('Status: ', d.Status, ', Date: ', CONVERT(VARCHAR, d.AppointmentDate, 120)),
        CONCAT('Status: ', i.Status, ', Date: ', CONVERT(VARCHAR, i.AppointmentDate, 120)),
        GETDATE(),
        SYSTEM_USER
    FROM inserted i
    INNER JOIN deleted d ON i.Id = d.Id
    WHERE i.Status <> d.Status OR i.AppointmentDate <> d.AppointmentDate;
END
GO


CREATE OR ALTER PROCEDURE [Healthcare].[sp_CreateAppointment]
    @PatientId INT,
    @AppointmentDate DATETIME,
    @Reason NVARCHAR(500),
    @DoctorName NVARCHAR(200),
    @Status NVARCHAR(50) = 'Scheduled',
    @NewAppointmentId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        
        IF NOT EXISTS (SELECT 1 FROM [Healthcare].[Patient] WHERE Id = @PatientId)
        BEGIN
            RAISERROR('Patient with ID %d does not exist.', 16, 1, @PatientId);
            RETURN;
        END

        -- Insert appointment
        INSERT INTO [Healthcare].[Appointment] (PatientId, AppointmentDate, Reason, DoctorName, Status)
        VALUES (@PatientId, @AppointmentDate, @Reason, @DoctorName, @Status);

        SET @NewAppointmentId = SCOPE_IDENTITY();

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        
        THROW;
    END CATCH
END
GO
