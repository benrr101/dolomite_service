﻿ALTER TABLE [dbo].[Sessions]
	ADD CONSTRAINT [FK_SESSION_APIKEY]
	FOREIGN KEY (ApiKey)
	REFERENCES [ApiKeys] (Id)
	ON UPDATE CASCADE
	ON DELETE NO ACTION
