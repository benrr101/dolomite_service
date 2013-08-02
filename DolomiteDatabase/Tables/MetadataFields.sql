﻿CREATE TABLE [dbo].[MetadataFields]
(
	[Id]			INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[TagName]		NVARCHAR(20) NOT NULL,
	[DisplayName]	NVARCHAR(20) NOT NULL, 
    [AllowedRules]	NVARCHAR(150) NULL
)
