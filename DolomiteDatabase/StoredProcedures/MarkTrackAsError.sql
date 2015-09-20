﻿CREATE PROCEDURE [dbo].[MarkTrackAsError]
	@trackId	BIGINT,
	@userError	NVARCHAR(140),
	@adminError	NVARCHAR(MAX)
AS
	SET XACT_ABORT ON
	BEGIN TRANSACTION
	
	-- Add the error record to the 
	INSERT INTO [ErrorInfo] (UserError, AdminError)
	VALUES (@userError, @adminError)

	-- Mark the track as error
	UPDATE [Tracks] 
	SET 
		[Status] = 5,
		[ErrorInfo] = @@IDENTITY,
		[Locked] = 0
	WHERE [Id] = @trackId

	COMMIT TRANSACTION
	RETURN 1
