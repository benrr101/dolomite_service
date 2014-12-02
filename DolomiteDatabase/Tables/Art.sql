﻿CREATE TABLE [dbo].[Art]
(
	[Id]			BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[GuidId]		UNIQUEIDENTIFIER NOT NULL,
	[Mimetype]		NVARCHAR(30) NOT NULL,
	[Hash]			NCHAR(40) NOT NULL
)
