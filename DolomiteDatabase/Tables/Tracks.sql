﻿CREATE TABLE [dbo].[Tracks]
(
	[Id]	UNIQUEIDENTIFIER NOT NULL,
	[Owner] INT NOT NULL,
	[Hash]	NCHAR(40) NULL, 
    [Album] INT NULL,
	[Art] UNIQUEIDENTIFIER NULL,
	[OriginalBitrate] INT NULL,
	[OriginalSampling] INT NULL,
	[OriginalMimetype] NVARCHAR(30) NULL,
	[OriginalExtension] NVARCHAR(10) NULL,
    [HasBeenOnboarded] BIT NOT NULL DEFAULT 0, 
    [Locked] BIT NOT NULL DEFAULT 0,
    [TrackInTempStorage] BIT NOT NULL DEFAULT 0, 
    [ArtChange] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_Tracks] PRIMARY KEY ([Id]) 
)
