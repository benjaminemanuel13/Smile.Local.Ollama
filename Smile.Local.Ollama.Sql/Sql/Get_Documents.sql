USE [smile-local]
GO

/****** Object:  StoredProcedure [dbo].[Get_Documents]    Script Date: 22/02/2025 12:47:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Get_Documents] 
	@text nvarchar (100),
	@threshold float
AS
BEGIN
	
declare @retval int, @response nvarchar(max);
declare @payload nvarchar(max);
set @payload = json_object('text': @text);

begin try
    exec @retval = Get_Embeddings
        @url = 'http://localhost:5267/embeddings',
        @method = 'POST',
        @credential = '8378ae6201a4426386d9a7eb35be045f',
        @payload = @payload,
        @response = @response output;
end try
begin catch
    select 
        'SQL' as error_source, 
        error_number() as error_code,
        error_message() as error_message,
		error_line() as error_line
    return;
end catch;

with cteVector as
(
    select 
        cast([key] as int) as [vector_value_id],
        cast([value] as float) as [vector_value]
    from 
        openjson(json_query(@response, '$.data[0].embedding'))
),
cteSimilar as
(
    select 
        v2.id as Document_Id, 
        -- Optimized as per https://platform.openai.com/docs/guides/embeddings/which-distance-function-should-i-use
        sum(v1.[vector_value] * v2.[vector_value]) as cosine_similarity
    from 
        cteVector v1
    inner join 
        Documents_Embeddings v2 on v1.vector_value_id = v2.vector_value_id
    group by
        v2.id
 )       
	
	select sim.Document_Id, doc.DocumentText, sim.cosine_similarity from cteSimilar as sim 
	inner join Documents as doc on doc.Id = sim.Document_Id
	where cosine_similarity > @threshold
END
GO

