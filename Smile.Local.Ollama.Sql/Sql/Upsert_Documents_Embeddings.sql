USE [smile-local]
GO

/****** Object:  StoredProcedure [dbo].[Upsert_Documents_Embeddings]    Script Date: 09/02/2025 17:58:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[Upsert_Documents_Embeddings]
@id int,
@embeddings nvarchar(max)
as

set xact_abort on
set transaction isolation level serializable

begin transaction

    delete from Documents_Embeddings 
    where Id = @id

    insert into Documents_Embeddings
    select @id, cast([key] as int), cast([value] as float) 
    from openjson(@embeddings)

    update 
        Documents 
    set 
        Embeddings = @embeddings,
        Require_embeddings_update = 0
    where   
        Id = @id

commit
GO

