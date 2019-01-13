USE [TvpTestDB]
GO

--Dropping the stored procedure "apx_getMaxPresentationLevelsAtStoreItemLevel" if already exists in a database
IF EXISTS
(
  SELECT * FROM dbo.sysobjects
  WHERE id = object_id(N'[dbo].[apx_getMaxPresentationLevelsAtStoreItemLevel]')
         AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
DROP PROCEDURE [dbo].[apx_getMaxPresentationLevelsAtStoreItemLevel]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[apx_getMaxPresentationLevelsAtStoreItemLevel]
@item_store_nextmonday_outtodate dbo.TwoIntKeysAndTwoDateValues READONLY


AS

SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED



CREATE TABLE #InputValues(itemKey INT, storeKey INT, nextMonday DATETIME, outToDate DATETIME PRIMARY KEY (itemKey, storeKey))

INSERT INTO #InputValues
	SELECT
		*
	FROM @item_store_nextmonday_outtodate



SELECT [store_key]
	, [item_key]
	, CAST(MAX([quantity]) AS SMALLINT) as quantity 
	FROM  #InputValues as inputValues 
	JOIN [dbo].[presentation_level] AS pl
		ON inputValues.[itemKey] = pl.item_key and inputValues.[storeKey] = pl.store_key 
		and pl.date between inputValues.[nextMonday] and inputValues.[outToDate]
	Group by [store_key], [item_key]
GO