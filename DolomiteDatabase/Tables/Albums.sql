﻿CREATE TABLE [dbo].[Albums]
(
	[Id]		INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Name]		NVARCHAR(100) NOT NULL,
	[Artist]	INT NOT NULL,
	[Art]		UNIQUEIDENTIFIER NULL
)
