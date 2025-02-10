USE [smile-local]
GO

/****** Object:  StoredProcedure [dbo].[GetMaxID_Documents_Embeddings]    Script Date: 09/02/2025 17:58:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetMaxID_Documents_Embeddings]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT MAX(Id) from Documents_Embeddings
END
GO

