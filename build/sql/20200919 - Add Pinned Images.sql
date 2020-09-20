-- This script will add image pinning support

ALTER TABLE [dbo].[Recipe] ADD PinnedImageId [int] NULL
GO

ALTER TABLE [dbo].[Recipe] ADD CONSTRAINT FK_Recipe_PinnedImage FOREIGN KEY (PinnedImageId) REFERENCES [Image] (Id)
ALTER TABLE [dbo].[Recipe] CHECK CONSTRAINT FK_Recipe_PinnedImage
GO
