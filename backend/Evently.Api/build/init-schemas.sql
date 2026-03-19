USE [Evently];
GO

IF SCHEMA_ID(N'Main') IS NULL
  EXEC(N'CREATE SCHEMA [Main]');
GO

USE [EventlyAnalytics];
GO

IF SCHEMA_ID(N'Analytics') IS NULL
  EXEC(N'CREATE SCHEMA [Analytics]');
GO