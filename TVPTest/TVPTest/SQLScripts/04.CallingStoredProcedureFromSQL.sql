USE [TvpTestDB]
GO



DECLARE @tmp dbo.TwoIntKeysAndTwoDateValues
INSERT INTO @tmp
VALUES(9,101,'2018-11-01 00:00:00.000','2018-12-05 00:00:00.000')
DECLARE	@return_value int

EXEC	@return_value = [dbo].[apx_getMaxPresentationLevelsAtStoreItemLevel]
		@item_store_nextmonday_outtodate = @tmp

SELECT	'Return Value' = @return_value

GO
