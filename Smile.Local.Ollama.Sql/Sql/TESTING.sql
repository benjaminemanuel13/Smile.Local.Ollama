USE [smile-local]
GO

DECLARE	@return_value int,
		@response nvarchar(max)

EXEC	@return_value = [dbo].[Get_Embeddings]
		@url = N'https://localhost:7053/embeddings',
		@method = N'POST',
		@credential = N'1234',
		@payload = N'{ "text": "Who Is Ben" }',
		@response = @response OUTPUT

SELECT	@response as N'@response'

SELECT	'Return Value' = @return_value

GO
