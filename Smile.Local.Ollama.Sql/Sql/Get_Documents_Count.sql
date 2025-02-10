USE [smile-local]
GO

/****** Object:  StoredProcedure [dbo].[Get_Documents_Count]    Script Date: 09/02/2025 17:57:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[Get_Documents_Count]
as
select count(*) as Total_Documents from Documents;
GO

