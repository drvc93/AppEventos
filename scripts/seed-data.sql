-- Script de datos iniciales para EventDb (SQL Server)
-- Ejecutar después de aplicar las migraciones de EF Core
-- Las bases de datos se crean automáticamente via EF Core Migrate()

USE EventDb;
GO

-- Insertar evento de ejemplo
IF NOT EXISTS (SELECT 1 FROM Events WHERE Name = 'Concierto Inaugural')
BEGIN
    DECLARE @EventId UNIQUEIDENTIFIER = NEWID();
    
    INSERT INTO Events (Id, Name, Date, Location, Status, CreatedAt)
    VALUES (@EventId, 'Concierto Inaugural', '2026-06-15T20:00:00', 'Arena CDMX', 'Published', GETUTCDATE());

    INSERT INTO Zones (Id, EventId, Name, Price, Capacity)
    VALUES 
        (NEWID(), @EventId, 'VIP', 2500.00, 500),
        (NEWID(), @EventId, 'General', 800.00, 5000),
        (NEWID(), @EventId, 'Preferente', 1500.00, 1000);
END
GO
