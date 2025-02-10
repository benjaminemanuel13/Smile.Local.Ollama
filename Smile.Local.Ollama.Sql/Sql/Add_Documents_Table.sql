USE [smile-local]
GO

/****** Object:  Table [dbo].[Documents]    Script Date: 09/02/2025 19:23:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Documents](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](200) NOT NULL,
	[DocumentText] [nvarchar](max) NOT NULL,
	[External_Id] [varchar](100) NOT NULL,
	[Last_Fetched] [datetime2](7) NULL,
	[Start_Time] [datetime2](7) NOT NULL,
	[End_Time] [datetime2](7) NOT NULL,
	[Tags] [nvarchar](max) NULL,
	[Recording_Url] [varchar](1000) NULL,
	[Require_Embeddings_Update] [bit] NOT NULL,
	[Embeddings] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

