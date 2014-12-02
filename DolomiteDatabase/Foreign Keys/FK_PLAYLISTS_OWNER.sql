﻿ALTER TABLE [dbo].[Playlists]
	ADD CONSTRAINT [FK_PLAYLISTS_OWNER]
	FOREIGN KEY (Owner)
	REFERENCES [Users] (Id)
	ON UPDATE CASCADE
	ON DELETE CASCADE
