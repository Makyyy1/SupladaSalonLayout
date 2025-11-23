-- Update database schema to rename Service1 to Service Name and remove Service2 column

-- First, create a backup of existing data if needed
-- SELECT * INTO Appointments_Backup FROM Appointments;

-- Add the new Service Name column
ALTER TABLE Appointments ADD [Service Name] NVARCHAR(MAX) NULL;

-- Copy data from Service1 to Service Name
UPDATE Appointments SET [Service Name] = Service1 WHERE Service1 IS NOT NULL;

-- Drop the old Service1 column
ALTER TABLE Appointments DROP COLUMN Service1;

-- Drop the Service2 column
ALTER TABLE Appointments DROP COLUMN Service2;

-- Update any queries that reference Service1 to use [Service Name] instead
