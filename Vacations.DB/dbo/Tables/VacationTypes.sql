﻿CREATE TABLE [dbo].[VacationTypes]
(
	[VacationTypesID] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT (newid()), 
    [Name] NVARCHAR(50) NOT NULL
)
