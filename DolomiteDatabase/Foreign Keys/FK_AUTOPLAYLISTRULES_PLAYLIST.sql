﻿ALTER TABLE [dbo].[AutoplaylistRules]
	ADD CONSTRAINT [FK_AUTOPLAYLISTRULES_PLAYLIST]
	FOREIGN KEY (Autoplaylist)
	REFERENCES [Autoplaylists] (Id)
	ON UPDATE CASCADE
	ON DELETE CASCADE
