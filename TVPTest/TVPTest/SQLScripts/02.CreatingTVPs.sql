USE [TvpTestDB]
GO


CREATE TYPE [dbo].[TwoIntKeysAndTwoDateValues] AS TABLE(
	[KeyOne] [int] NOT NULL,
	[KeyTwo] [int] NOT NULL,
	[ValueOne] [datetime] NOT NULL,
	[ValueTwo] [datetime] NOT NULL,
	PRIMARY KEY NONCLUSTERED 
(
	[KeyOne] ASC,
	[KeyTwo] ASC,
	[ValueOne] ASC,
	[ValueTwo] ASC
)WITH (IGNORE_DUP_KEY = OFF)
)
GO