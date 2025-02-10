USE [smile-local]
GO

/****** Object:  Table [dbo].[Searched_Text]    Script Date: 09/02/2025 19:24:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Searched_Text](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Searched_Text] [nvarchar](max) NOT NULL,
	[Search_DateTime] [datetime2](7) NOT NULL,
	[MS_Rest_Call] [int] NULL,
	[MS_Vector_Search] [int] NULL,
	[Found_Documents] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Searched_Text] ADD  CONSTRAINT [DF_Searched_Text]  DEFAULT (sysdatetime()) FOR [Search_DateTime]
GO

