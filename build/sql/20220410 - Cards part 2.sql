SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER TABLE [dbo].[Recipe] DROP COLUMN Ingredients
GO

-- Run ./build/dbApplyMigration.ps1 after this script, from now on we will be using EF Code-First migrations
-- See the readme for details.
