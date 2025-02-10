USE [smile-local]
GO

/****** Object:  StoredProcedure [dbo].[Find_Documents]    Script Date: 09/02/2025 17:57:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[Find_Documents]
@text nvarchar(max),
@top int = 10,
@min_similarity decimal(19,16) = 0.75
as
if (@text is null) return;

insert into Searched_Text (Searched_Text) values (@text);
declare @sid int = scope_identity();

declare @startTime as datetime2(7) = sysdatetime()

declare @retval int, @response nvarchar(max);
declare @payload nvarchar(max);
set @payload = json_object('input': @text);

begin try
    exec @retval = Get_Embeddings
        @url = 'http://4.233.194.3/api/Embedding',
        @method = 'POST',
        @credential = '8378ae6201a4426386d9a7eb35be045f',
        @payload = @payload,
        @response = @response output;
end try
begin catch
    select 
        'SQL' as error_source, 
        error_number() as error_code,
        error_message() as error_message
    return;
end catch

if (@retval != 0) begin
    select 
        'OPENAI' as error_source, 
        json_value(@response, '$.result.error.code') as error_code,
        json_value(@response, '$.result.error.message') as error_message,
        @response as error_response
    return;
end;

declare @endTime1 as datetime2(7) = sysdatetime();
update Searched_Text set MS_Rest_Call = datediff(ms, @startTime, @endTime1) where Id = @sid;

with cteVector as
(
    select 
        cast([key] as int) as [vector_value_id],
        cast([value] as float) as [vector_value]
    from 
        openjson(json_query(@response, '$.result.data[0].embedding'))
),
cteSimilarAuthors as 
(
    select 
        v2.Id as Author_Id, 
        -- Optimized as per https://platform.openai.com/docs/guides/embeddings/which-distance-function-should-i-use
        sum(v1.[vector_value] * v2.[Vector_Value]) as cosine_similarity
    from 
        cteVector v1
    inner join 
        Authors_Embeddings v2 on v1.vector_value_id = v2.vector_value_id
    group by
        v2.Id

),
cteSimilar as
(
    select 
        v2.Id as Document_Id, 
        -- Optimized as per https://platform.openai.com/docs/guides/embeddings/which-distance-function-should-i-use
        sum(v1.[vector_value] * v2.[Vector_Value]) as cosine_similarity
    from 
        cteVector v1
    inner join 
        Documents_Embeddings v2 on v1.vector_value_id = v2.vector_value_id
    group by
        v2.Id
        
    union all

    select
        ss.Document_Id,
        s.cosine_similarity
    from
        Documents_Authors ss 
    inner join
        cteSimilarAuthors s on s.Author_Id = ss.Author_Id
),
cteSimilar2 as (
    select
        *,
        rn = row_number() over (partition by Document_Id order by cosine_similarity desc)
    from
        cteSimilar
),
cteSpeakers as
(
    select 
        Document_Id, 
        json_query('["' + string_agg(string_escape(Full_Name, 'json'), '","') + '"]') as Authors
    from 
        Documents_Authors ss 
    inner join 
        Authors sp on sp.Id = ss.Author_Id 
    group by 
        Document_Id
)
select top(@top)
    a.Id,
    a.Title,
    a.DocumentText,
    a.External_Id,
    a.Start_Time,
    a.End_Time,
    a.Recording_Url,
    isnull((select top (1) Authors from cteSpeakers where Document_Id = a.Id), '[]') as Authors,
    r.cosine_similarity
from 
    cteSimilar2 r
inner join 
    Documents a on r.Document_id = a.Id
where   
    r.cosine_similarity > @min_similarity
and
    rn = 1
order by    
    r.cosine_similarity desc, a.title asc;

declare @rc int = @@rowcount;

declare @endTime2 as datetime2(7) = sysdatetime()
update 
    Searched_Text
set 
    MS_Vector_Search = datediff(ms, @endTime1, @endTime2),
    Found_Documents = @rc
where 
    Id = @sid
GO

