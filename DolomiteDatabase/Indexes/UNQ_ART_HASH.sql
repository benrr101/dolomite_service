﻿ALTER TABLE [dbo].[Art]
	ADD CONSTRAINT [UNQ_ART_HASH]
	UNIQUE NONCLUSTERED ([Hash])
