﻿ALTER TABLE [dbo].[Tracks]
	ADD CONSTRAINT [FK_TRACKS_OWNER]
	FOREIGN KEY (Owner)
	REFERENCES [Users] (Id)
	ON UPDATE CASCADE
	ON DELETE CASCADE