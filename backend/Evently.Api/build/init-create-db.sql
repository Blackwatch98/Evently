IF DB_ID(N'Evently') IS NULL
BEGIN
  CREATE DATABASE [Evently];
END
GO

IF DB_ID(N'EventlyAnalytics') IS NULL
BEGIN
  CREATE DATABASE [EventlyAnalytics];
END
GO