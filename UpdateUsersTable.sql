-- Update Users table to include additional fields for cashier management
-- Run this script to update your existing Users table

-- Add new columns if they don't exist
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND name = 'FullName')
BEGIN
    ALTER TABLE Users ADD FullName NVARCHAR(100) NULL;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND name = 'ContactNumber')
BEGIN
    ALTER TABLE Users ADD ContactNumber NVARCHAR(20) NULL;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND name = 'Address')
BEGIN
    ALTER TABLE Users ADD Address NVARCHAR(255) NULL;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND name = 'SecurityQuestion')
BEGIN
    ALTER TABLE Users ADD SecurityQuestion NVARCHAR(255) NULL;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND name = 'SecurityAnswer')
BEGIN
    ALTER TABLE Users ADD SecurityAnswer NVARCHAR(255) NULL;
END

