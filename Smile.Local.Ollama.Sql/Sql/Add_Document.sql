USE [smile-local]
GO

/****** Object:  StoredProcedure [dbo].[Add_Document]    Script Date: 09/02/2025 17:57:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ben Emanuel
-- Create date: 07/09/2024
-- Description:	Add Document
-- =============================================
CREATE PROCEDURE [dbo].[Add_Document]	
	@title nvarchar(200),
	@text nvarchar(MAX),
	@externalId varchar(100),
	@startTime datetime2(7),
	@endTime datetime2(7),
	@require bit,
	@newId INT OUTPUT
AS
BEGIN
	INSERT INTO Documents (Title, DocumentText, External_Id, Start_Time, End_Time, Require_Embeddings_Update)
	VALUES
	(@title, @text, @externalId, @startTime, @endTime, @require)

	Set @newId = @@IDENTITY
END
GO

