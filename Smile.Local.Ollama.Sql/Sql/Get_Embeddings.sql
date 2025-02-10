USE [smile-local]
GO

/****** Object:  StoredProcedure [dbo].[Get_Embeddings]    Script Date: 09/02/2025 18:02:24 ******/
SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[Get_Embeddings]
	@url [nvarchar](max),
	@method [nvarchar](max),
	@credential [nvarchar](max),
	@payload [nvarchar](max),
	@response [nvarchar](max) OUTPUT
WITH EXECUTE AS CALLER
AS
EXTERNAL NAME [Smile].[Smile.Local.Ollama.Sql.Smile].[DoRest]
GO

