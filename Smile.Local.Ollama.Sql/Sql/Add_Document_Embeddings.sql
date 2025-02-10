USE [smile-local]
GO

/****** Object:  Table [dbo].[Documents_Embeddings]    Script Date: 09/02/2025 19:24:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Documents_Embeddings](
	[Id] [int] NOT NULL,
	[Vector_Value_Id] [int] NOT NULL,
	[Vector_Value] [decimal](19, 16) NOT NULL
) ON [PRIMARY]
GO

