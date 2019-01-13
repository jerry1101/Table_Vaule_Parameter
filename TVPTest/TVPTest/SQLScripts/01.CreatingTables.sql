USE [TvpTestDB]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[presentation_level](
	[date] [datetime] NOT NULL,
	[store_key] [int] NOT NULL,
	[item_key] [int] NOT NULL,
	[quantity] [numeric](9, 2) NOT NULL,
 CONSTRAINT [PK_presentation_level] PRIMARY KEY CLUSTERED 
(
	[date] DESC,
	[store_key] ASC,
	[item_key] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO




