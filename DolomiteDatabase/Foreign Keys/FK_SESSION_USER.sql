﻿ALTER TABLE [dbo].[Sessions]
	ADD CONSTRAINT [FK_SESSION_USER]
	FOREIGN KEY ([User])
	REFERENCES [Users] (Id)
