ALTER TABLE Recipe
ADD CreatedOnUtc DateTime,
	ModifiedOnUtc DateTime
GO

DECLARE @timeZone int = 0 -- <yourTimeZoneHere>
UPDATE Recipe
Set CreatedOnUtc = SwitchOffset(TODATETIMEOFFSET(CreatedOn,@timeZone * 60), 0),
	ModifiedOnUtc = SwitchOffset(TODATETIMEOFFSET(ModifiedOn, @timeZone * 60), 0)

ALTER TABLE Recipe
DROP COLUMN CreatedOn,
	 ModifiedOn
GO