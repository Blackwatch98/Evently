IF NOT EXISTS (SELECT 1 FROM sys.sql_logins WHERE name = N'appuser')
BEGIN
  CREATE LOGIN [appuser] WITH PASSWORD = N'Dev!Passw0rd2026';
END
GO

USE [Evently];
GO

IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = N'appuser')
BEGIN
  CREATE USER [appuser] FOR LOGIN [appuser];
END
GO

IF IS_ROLEMEMBER(N'db_owner', N'appuser') = 0
BEGIN
  ALTER ROLE [db_owner] ADD MEMBER [appuser];
END
GO

USE [EventlyAnalytics];
GO

IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = N'appuser')
BEGIN
  CREATE USER [appuser] FOR LOGIN [appuser];
END
GO

IF IS_ROLEMEMBER(N'db_owner', N'appuser') = 0
BEGIN
  ALTER ROLE [db_owner] ADD MEMBER [appuser];
END
GO